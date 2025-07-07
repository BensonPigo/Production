using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Class.Command;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P09_ImportFromWorkOrderForPlanning : Sci.Win.Subs.Base
    {
        private readonly string id;
        private DataTable WorkOrderForOutput;
        private DataTable WorkOrderForOutput_PatternPanel;
        private DataTable WorkOrderForOutput_SizeRatio;
        private DataTable workOrderForOutput_Distribute;
        private bool editByUseCutRefToRequestFabric;

        /// <summary>
        /// P09_ImportFromWorkOrderForPlanning，寫入Form表身的資料
        /// </summary>
        /// <param name="id">WorkOrderForPlanning的ID</param>
        /// <param name="workOrderForOutput">P09的 this.detailgridbs.DataSource</param>
        /// <param name="workOrderForOutput_PatternPanel">P09的 this.dtWorkOrderForOutput_PatternPanel</param>
        /// <param name="workOrderForOutput_SizeRatio">P09的 this.dtWorkOrderForOutput_SizeRatio</param>
        /// <param name="workOrderForOutput_Distribute">P09的 this.workOrderForOutput_Distribute</param>
        /// <param name="editByUseCutRefToRequestFabric">P09的 this.editByUseCutRefToRequestFabric</param>
        public P09_ImportFromWorkOrderForPlanning(string id, DataTable workOrderForOutput, DataTable workOrderForOutput_PatternPanel, DataTable workOrderForOutput_SizeRatio, DataTable workOrderForOutput_Distribute, bool editByUseCutRefToRequestFabric)
        {
            this.InitializeComponent();
            this.id = id;
            this.WorkOrderForOutput = workOrderForOutput;
            this.WorkOrderForOutput_PatternPanel = workOrderForOutput_PatternPanel;
            this.WorkOrderForOutput_SizeRatio = workOrderForOutput_SizeRatio;
            this.workOrderForOutput_Distribute = workOrderForOutput_Distribute;
            this.editByUseCutRefToRequestFabric = editByUseCutRefToRequestFabric;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.InitialData();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (this.lcbs.DataSource == null)
            {
                return;
            }

            #region 校驗
            DataTable dt = this.lcbs.DataSource as DataTable;
            List<DataRow> importDatas = dt.AsEnumerable().Where(r => MyUtility.Convert.GetBool(r["Sel"])).ToList();
            if (!importDatas.Any())
            {
                MyUtility.Msg.ErrorBox("Please choose at least one CutRef.");
                return;
            }

            DataTable dtExistCutRef = new DataTable();
            string sqlcmd = @"
SELECT DISTINCT wo.CutRef 
FROM WorkOrderForOutput wo WITH(NOLOCK)
INNER JOIN #tmp t ON t.ID = wo.ID and t.CutRef = wo.CutRef
WHERE t.Sel = 1";
            var result = MyUtility.Tool.ProcessWithDatatable(dt, "Sel,ID,CutRef", sqlcmd, out dtExistCutRef);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Get data failure!!!\n" + result.ToString());
                return;
            }

            if (dtExistCutRef != null && dtExistCutRef.Rows.Count > 0)
            {
                MyUtility.Msg.ErrorBox("The following CutRef already exists in work order for output and cannot be imported." +
                    Environment.NewLine +
                    dtExistCutRef.AsEnumerable().Select(r => r["CutRef"].ToString()).JoinToString(Environment.NewLine));
                return;
            }
            #endregion

            #region 執行

            // 匯入表身
            int maxKey = MyUtility.Convert.GetInt(this.WorkOrderForOutput.Compute("Max(tmpKey)", string.Empty)) + 1;

            // 1. WorkOrderForOutput
            DataTable import_WorkOrderForOutput = this.WorkOrderForOutput.Clone();
            importDatas.ForEach(data =>
            {
                DataRow row = import_WorkOrderForOutput.NewRow();
                row["Ukey"] = 0;
                row["tmpKey"] = maxKey++;
                row["ID"] = data["ID"];
                row["FactoryID"] = data["FactoryID"];
                row["MDivisionID"] = data["MDivisionID"];
                row["Seq1"] = data["Seq1"];
                row["Seq2"] = data["Seq2"];
                row["CutRef"] = data["CutRef"];
                row["CutNo"] = data["Seq"];
                row["OrderID"] = data["OrderID"];
                row["RefNo"] = data["RefNo"];
                row["SCIRefNo"] = data["SCIRefNo"];
                row["ColorID"] = data["ColorID"];
                row["Tone"] = data["Tone"];
                row["Layer"] = data["Layer"];
                row["FabricCombo"] = data["FabricCombo"];
                row["FabricCode"] = data["FabricCode"];
                row["FabricPanelCode"] = data["FabricPanelCode"];
                row["EstCutDate"] = MyUtility.Convert.GetDate(data["EstCutDate"]) == null ? (object)DBNull.Value : data["EstCutDate"];
                row["ConsPC"] = data["ConsPC"];
                row["Cons"] = data["Cons"];
                row["MarkerNo"] = data["MarkerNo"];
                row["MarkerName"] = data["MarkerName"];
                row["MarkerLength"] = data["MarkerLength"];
                row["MarkerVersion"] = data["MarkerVersion"];
                row["ActCuttingPerimeter"] = data["ActCuttingPerimeter"];
                row["StraightLength"] = data["StraightLength"];
                row["CurvedLength"] = data["CurvedLength"];
                row["Shift"] = string.Empty;
                row["CutCellID"] = data["CutCellID"];
                row["SpreadingNoID"] = string.Empty;
                row["UnfinishedCuttingReason"] = string.Empty;
                row["IsCreateByUser"] = data["IsCreateByUser"];
                row["SpreadingStatus"] = "Ready";
                row["SpreadingRemark"] = string.Empty;
                row["GroupID"] = string.Empty;
                row["SourceFrom"] = 1;
                row["WorkOrderForPlanningUkey"] = data["Ukey"];
                row["Order_EachconsUkey"] = data["Order_EachconsUkey"];
                row["CutPlanID"] = data["CutPlanID"];
                row["AddName"] = Env.User.UserID;
                row["AddDate"] = DateTime.Now;
                row["EditName"] = string.Empty;
                row["EditDate"] = DBNull.Value;

                row["PatternPanel_CONCAT"] = data["PatternPanel_CONCAT"];
                row["FabricPanelCode_CONCAT"] = data["FabricPanelCode_CONCAT"];
                row["SizeCode_CONCAT"] = data["SizeCode_CONCAT"];
                row["TotalCutQty_CONCAT"] = data["TotalCutQty_CONCAT"];
                row["MarkerLength_Mask"] = data["MarkerLength_Mask"];
                row["ActCuttingPerimeter_Mask"] = data["ActCuttingPerimeter_Mask"];
                row["StraightLength_Mask"] = data["StraightLength_Mask"];
                row["CurvedLength_Mask"] = data["CurvedLength_Mask"];
                row["Fabeta"] = data["Fabeta"];
                row["FabricTypeRefNo"] = data["FabricTypeRefNo"];
                row["FabricDescription"] = data["FabricDescription"];
                row["ImportWP"] = true;
                row["CanEdit"] = true;
                import_WorkOrderForOutput.Rows.Add(row);

                this.WorkOrderForOutput.ImportRow(row);
            });

            // 2. WorkOrderForOutput_PatternPanel
            sqlcmd = @"
SELECT
    WorkOrderForOutputUkey = 0,
    t.ID,
    wpp.PatternPanel,
    wpp.FabricPanelCode,
    t.tmpKey
FROM #tmp t
INNER JOIN WorkOrderForPlanning_PatternPanel wpp WITH(NOLOCK) ON t.WorkOrderForPlanningUkey = wpp.WorkOrderForPlanningUkey";
            DataTable dtPatternPanel = new DataTable();
            result = MyUtility.Tool.ProcessWithDatatable(import_WorkOrderForOutput, "tmpKey,ID,WorkOrderForPlanningUkey", sqlcmd, out dtPatternPanel);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Get data failure!!!\n" + result.ToString());
                return;
            }

            if (dtPatternPanel != null && dtPatternPanel.Rows.Count > 0)
            {
                dtPatternPanel.AsEnumerable().ToList().ForEach(data =>
                {
                    var row = this.WorkOrderForOutput_PatternPanel.NewRow();
                    row.ItemArray = data.ItemArray;
                    this.WorkOrderForOutput_PatternPanel.Rows.Add(row);
                });
            }

            // 3. WorkOrderForOutput_SizeRatio
            sqlcmd = @"
SELECT
    WorkOrderForOutputUkey = 0,
    wsr.ID,
    wsr.SizeCode,
    wsr.Qty,
    t.tmpKey
FROM #tmp t
INNER JOIN WorkOrderForPlanning_SizeRatio wsr WITH(NOLOCK) ON t.WorkOrderForPlanningUkey = wsr.WorkOrderForPlanningUkey";
            DataTable dtSizeRatio = new DataTable();
            result = MyUtility.Tool.ProcessWithDatatable(import_WorkOrderForOutput, "tmpKey,ID,WorkOrderForPlanningUkey", sqlcmd, out dtSizeRatio);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Get data failure!!!\n" + result.ToString());
                return;
            }

            if (dtSizeRatio != null && dtSizeRatio.Rows.Count > 0)
            {
                dtSizeRatio.AsEnumerable().ToList().ForEach(data =>
                {
                    var row = this.WorkOrderForOutput_SizeRatio.NewRow();
                    row.ItemArray = data.ItemArray;
                    this.WorkOrderForOutput_SizeRatio.Rows.Add(row);
                });
            }

            // 4. WorkOrderForOutput_Distribute
            //if (this.editByUseCutRefToRequestFabric)  
            //{ ISP20250495 Import From P02 一律照抄P02，不論Cutting.UseCutRefToRequestFabric的值是多少，所以去掉 editByUseCutRefToRequestFabric 判斷
            sqlcmd = @"
SELECT
WorkOrderForOutputUkey = 0,
wdp.ID,
wdp.OrderID,
wdp.Article,
wdp.SizeCode,
wdp.Qty,
t.tmpKey
,Sewinline = (
    SELECT MIN(ss.Inline)
    FROM SewingSchedule_Detail ssd WITH (NOLOCK)
    LEFT JOIN SewingSchedule ss WITH (NOLOCK) ON ssd.ID = ss.ID
    WHERE ssd.OrderID = wdp.OrderID
    AND ssd.Article = wdp.Article
    AND ssd.SizeCode = wdp.SizeCode
)
FROM #tmp t
INNER JOIN WorkOrderForPlanning_Distribute wdp WITH(NOLOCK)
ON t.WorkOrderForPlanningUkey = wdp.WorkOrderForPlanningUkey";
            DataTable dtDistribute = new DataTable();
            result = MyUtility.Tool.ProcessWithDatatable(import_WorkOrderForOutput, "tmpKey,ID,WorkOrderForPlanningUkey", sqlcmd, out dtDistribute);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Get data failure!!!\n" + result.ToString());
                return;
            }

            if (dtDistribute != null && dtDistribute.Rows.Count > 0)
            {
                dtDistribute.AsEnumerable().ToList().ForEach(data =>
                {
                    var row = this.workOrderForOutput_Distribute.NewRow();
                    row.ItemArray = data.ItemArray;
                    this.workOrderForOutput_Distribute.Rows.Add(row);
                });
            }
            //}
            #endregion

            this.DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Sel", header: string.Empty, width: Ict.Win.Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false)
                .Text("CutRef", header: "CutRef#", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Seq", header: "Seq", width: Ict.Win.Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("MarkerName", header: "Marker\r\nName", width: Ict.Win.Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("MarkerNo", "Pattern No.", width: Ict.Win.Widths.AnsiChars(12), iseditingreadonly: true)
                .MaskedText("MarkerLength_Mask", "00Y00-0/0+0\"", "Marker Length", width: Ict.Win.Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("PatternPanel_CONCAT", header: "Pattern\r\nPanel", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("FabricPanelCode_CONCAT", header: "Fabric\r\nPanel Code", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ColorId", header: "Color", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Tone", header: "Tone", width: Ict.Win.Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("SizeCode_CONCAT", header: "Size", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Layer", header: "Layers", width: Ict.Win.Widths.AnsiChars(5), integer_places: 5, maximum: 99999, iseditingreadonly: true)
                .Text("TotalCutQty_CONCAT", header: "Total CutQty", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("OrderId", "SP#", width: Ict.Win.Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("SEQ1", header: "Seq1", width: Ict.Win.Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("SEQ2", header: "Seq2", width: Ict.Win.Widths.AnsiChars(2), iseditingreadonly: true)
                .Date("Fabeta", header: "Fabric Arr Date", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("WKETA", "WK ETA", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("EstCutDate", "Est. Cut Date", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CutCellID", header: "Cut\r\nCell", width: Ict.Win.Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("CutPlanID", header: "Cut Plan", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Edituser", header: "Edit Name", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                .DateTime("EditDate", header: "Edit Date", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Adduser", header: "Add Name", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                .DateTime("AddDate", header: "Add Date", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                ;

            this.grid1.IsEditingReadOnly = false;
        }

        private void InitialData()
        {
            this.lcbs.DataSource = null;
            string sqlcmd = @"
SELECT
    sel = CAST(1 AS BIT)
   ,wo.*
   ,[Article] = ISNULL(Article.VALUE,'')
   ,wp1.PatternPanel_CONCAT
   ,wp2.FabricPanelCode_CONCAT
   ,ws1.SizeCode_CONCAT
   ,ws2.TotalCutQty_CONCAT
   ,Fabeta = FabricArrDate.val
   ,MarkerLength_Mask = ''
   ,ActCuttingPerimeter_Mask = ''
   ,StraightLength_Mask = ''
   ,CurvedLength_Mask = ''
   ,AddUser = p1.Name
   ,EditUser = p2.Name
   ,FabricTypeRefNo = CONCAT(f.WeaveTypeID, ' /' + wo.RefNo)
   ,FabricDescription = f.Description

   --沒有顯示的欄位
   ,tmpKey = CAST(0 AS BIGINT) --控制新加的資料用,SizeRatio/PatternPanel
   ,ImportWP = CAST(0 AS BIT)
   --- 排序用
   ,SORT_NUM = 0 -- 避免編輯過程資料跑來跑去
   ,multisize.multisize
   ,Order_SizeCode_Seq.Order_SizeCode_Seq
   ,ActCuttingPerimeter = ISNULL(Order_EachCons.ActCuttingPerimeter, '')
   ,StraightLength = ISNULL(Order_EachCons.StraightLength, '')
   ,CurvedLength = ISNULL(Order_EachCons.CurvedLength, '')
FROM WorkOrderForPlanning wo WITH (NOLOCK)
LEFT JOIN #tmp t ON t.WorkOrderForPlanningUkey = wo.Ukey
LEFT JOIN Fabric f WITH (NOLOCK) ON f.SCIRefno = wo.SCIRefno
LEFT JOIN Construction cs WITH (NOLOCK) ON cs.ID = ConstructionID
LEFT JOIN Pass1 p1 WITH (NOLOCK) ON p1.ID = wo.AddName
LEFT JOIN Pass1 p2 WITH (NOLOCK) ON p2.ID = wo.EditName
LEFT JOIN Order_EachCons ON wo.Order_EachconsUkey = Order_EachCons.Ukey
OUTER APPLY (
    SELECT PatternPanel_CONCAT = STUFF((
        SELECT DISTINCT CONCAT('+', PatternPanel)
        FROM WorkOrderForPlanning_PatternPanel WITH (NOLOCK) 
        WHERE WorkOrderForPlanningUkey = wo.Ukey
        FOR XML PATH ('')), 1, 1, '')
) wp1
OUTER APPLY (
    SELECT FabricPanelCode_CONCAT = STUFF((
        SELECT CONCAT('+', FabricPanelCode)
        FROM WorkOrderForPlanning_PatternPanel WITH (NOLOCK)
        WHERE WorkOrderForPlanningUkey = wo.Ukey
        FOR XML PATH (''))
    , 1, 1, '')
) wp2
OUTER APPLY (
    SELECT SizeCode_CONCAT = STUFF((
        SELECT CONCAT(', ', SizeCode, '/ ', Qty)
        FROM WorkOrderForPlanning_SizeRatio WITH (NOLOCK)
        WHERE WorkOrderForPlanningUkey = wo.Ukey
        FOR XML PATH ('')), 1, 1, '')
) ws1
OUTER APPLY (
    SELECT TotalCutQty_CONCAT = STUFF((
        SELECT CONCAT(', ', Sizecode, '/ ', Qty * wo.Layer)
        FROM WorkOrderForPlanning_SizeRatio WITH (NOLOCK)
        WHERE WorkOrderForPlanningUkey = wo.Ukey
        FOR XML PATH ('')), 1, 1, '')
) ws2
OUTER APPLY (
	SELECT multisize = IIF(COUNT(SizeCode) > 1, 2, 1) 
    FROM WorkOrderForPlanning_SizeRatio WITH (NOLOCK)
	Where wo.Ukey = WorkOrderForPlanningUkey
) as multisize
OUTER APPLY (
	SELECT Order_SizeCode_Seq = max(osc.Seq)
    FROM WorkOrderForPlanning_SizeRatio ws WITH (NOLOCK)
	LEFT JOIN Order_SizeCode osc WITH (NOLOCK) ON osc.ID = ws.ID and osc.SizeCode = ws.SizeCode
	WHERE ws.WorkOrderForPlanningUkey = wo.Ukey
) as Order_SizeCode_Seq
OUTER APPLY (
    SELECT val = IIF(psd.Complete = 1, psd.FinalETA, IIF(psd.Eta IS NOT NULL, psd.eta, IIF(psd.shipeta IS NOT NULL, psd.shipeta, psd.finaletd)))
    FROM PO_Supp_Detail psd WITH (NOLOCK)
    WHERE EXISTS (SELECT 1 FROM Orders WITH (NOLOCK) WHERE CuttingSp = wo.ID AND POID = psd.id)
    AND psd.seq1 = wo.seq1
    AND psd.seq2 = wo.seq2
) FabricArrDate
OUTER APPLY (
    SELECT DISTINCT value = 1 
    FROM WorkOrderForOutputHistory wh WITH (NOLOCK)
    INNER JOIN WorkOrderForOutput woo WITH (NOLOCK) ON woo.ID = wh.ID and woo.GroupID = wh.GroupID and woo.ID = wo.ID and wh.CutRef = wo.CutRef
) ExistHistory
OUTER APPLY (
    SELECT DISTINCT value = 1   
    FROM WorkOrderForOutputDelete wd WITH (NOLOCK)
    INNER JOIN WorkOrderForOutput woo WITH (NOLOCK) ON woo.ID = wd.ID and woo.GroupID = wd.GroupID and woo.ID = wo.ID and wd.CutRef = wo.CutRef
) ExistDelete
outer apply(
    select value = STUFF((
        select CONCAT(',',Article)
        from(
            select distinct Article
            from Production.dbo.WorkOrderForPlanning_Distribute
            where WorkOrderForPlanningUkey = WO.Ukey
            and Article !=''
            )s
    for xml path('')
    ),1,1,'')
) Article
WHERE wo.ID = @ID
AND t.WorkOrderForPlanningUkey IS NULL
AND ExistHistory.value IS NULL
AND ExistDelete.value IS NULL
AND NOT EXISTS (SELECT 1 FROM WorkOrderForOutput woo WITH (NOLOCK) WHERE woo.CutRef = wo.CutRef)
ORDER BY CutRef,Seq,MarkerName,MarkerNo,MarkerLength_Mask,PatternPanel_CONCAT,FabricPanelCode_CONCAT,Article.Value,ColorId,Tone,SizeCode_CONCAT,Layer,TotalCutQty_CONCAT,OrderId,Seq1,Seq2,Fabeta,WKETA,EstCutDate,CutPlanID,Edituser,EditDate,Adduser,AddDate

";
            DataTable dtImport = new DataTable();
            var paramters = new List<SqlParameter>() { new SqlParameter("@ID", this.id) };
            var result = MyUtility.Tool.ProcessWithDatatable(this.WorkOrderForOutput, "WorkOrderForPlanningUkey", sqlcmd, out dtImport, paramters: paramters);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Get data failure!!!\n" + result.ToString());
                return;
            }

            if (dtImport != null && dtImport.Rows.Count > 0)
            {
                dtImport.AsEnumerable().ToList().ForEach(row => Format4LengthColumn(row));
                this.lcbs.DataSource = dtImport;
            }
        }
    }
}
