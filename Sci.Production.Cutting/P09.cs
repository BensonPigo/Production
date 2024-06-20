using Ict;
using Ict.Win;
using Ict.Win.UI;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Class;
using Sci.Production.Class.Command;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
#pragma warning disable SA1600 // Elements should be documented
    /// <inheritdoc/>
    public partial class P09 : Win.Tems.Input6
    {
        private readonly Win.UI.BindingSource2 bindingSourceDetail = new Win.UI.BindingSource2(); // 右上使用,綁主表欄位
        private Ict.Win.UI.DataGridViewTextBoxColumn col_CutRef;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_OrderID;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq1;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq2;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Layer;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_WKETA;
        private Ict.Win.UI.DataGridViewDateBoxColumn EstCutDate;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_SpreadingNoID;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_CutCellID;
        private Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_MarkerLength;
        private Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_ActCuttingPerimeter;
        private Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_StraightLength;
        private Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_CurvedLength;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_MarkerNo;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_SizeRatio_Size;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_SizeRatio_Qty;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Distribute_SP;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Distribute_Article;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Distribute_Size;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Distribute_Qty;
        private DataTable dtWorkOrderForOutput_SizeRatio; // 弟3層表:新刪修
        private DataTable dtWorkOrderForOutput_Distribute; // 弟3層表:新刪修
        private DataTable dtWorkOrderForOutput_PatternPanel; // 弟3層表:新刪修
        private bool ReUpdateP20 = true;

        /// <inheritdoc/>
        public P09(ToolStripMenuItem menuitem, string history)
            : base(menuitem)
        {
            this.InitializeComponent();

            if (history == "0")
            {
                this.Text = "P09.WorkOrder For Output";
                this.IsSupportEdit = true;
            }
            else
            {
                this.Text = "P09.WorkOrder For Output(History)";
                this.IsSupportEdit = false;
            }

            this.displayBoxFabricTypeRefno.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "FabricTypeRefNo", true));
            this.displayBoxDescription.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "FabricDescription", true));
            this.numConsPC.DataBindings.Add(new Binding("Value", this.bindingSourceDetail, "ConsPC", true));
            this.displayBoxCons.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "Cons", true));
            this.displayBoxTotalCutQty.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "TotalCutQty_CONCAT", true));
            this.displayBoxTtlDistributeQty.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "TotalDistributeQty", true));
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string querySql = $@"
SELECT FTYGroup = '' 
UNION
SELECT DISTINCT FTYGroup
FROM Factory WITH (NOLOCK)
WHERE MDivisionID = '{Sci.Env.User.Keyword}'
";
            DBProxy.Current.Select(null, querySql, out DataTable queryDT);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);
            this.queryfors.SelectedIndex = 0;
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        this.DefaultWhere = $"FactoryID = '{this.queryfors.SelectedValue}'";
                        break;
                }

                this.ReloadDatas();
            };
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["id"].ToString();
            this.DetailSelectCommand = $@"
SELECT
    wo.*
   ,PatternPanel_CONCAT
   ,FabricPanelCode_CONCAT
   ,Article_CONCAT
   ,SizeCode_CONCAT
   ,TotalCutQty_CONCAT
   ,Fabeta
   ,Sewinline
   ,Actcutdate
   ,MarkerLength_Mask = ''
   ,ActCuttingPerimeter_Mask = ''
   ,StraightLength_Mask = ''
   ,CurvedLength_Mask = ''
   ,AddUser = p1.Name
   ,EditUser = p2.Name
   ,FabricTypeRefNo = CONCAT(f.WeaveTypeID, ' /' + wo.RefNo)
   ,FabricDescription = f.Description
   ,TotalDistributeQty = (SELECT SUM(Qty) FROM WorkOrderForOutput_Distribute WITH (NOLOCK) WHERE WorkOrderForOutputUkey = wo.Ukey)

   --沒有顯示的欄位
   ,tmpKey = CAST(0 AS BIGINT)--控制新加的資料用,SizeRatio/SpreadingFabric/Distribute/PatternPanel
   ,HasBundle = CAST(IIF(EXISTS(SELECT 1 FROM Bundle WHERE CutRef <> '' AND CutRef = wo.CutRef), 1, 0) AS BIT)
   ,HasCuttingOutput = CAST(IIF(EXISTS(SELECT 1 FROM CuttingOutput_Detail WHERE CutRef <> '' AND CutRef = wo.CutRef), 1, 0) AS BIT)
   ,HasMarkerReq = CAST(IIF(EXISTS(SELECT 1 FROM MarkerReq_Detail WHERE CutRef <> '' AND CutRef = wo.CutRef), 1, 0) AS BIT)

FROM WorkOrderForOutput wo WITH (NOLOCK)
LEFT JOIN Fabric f WITH (NOLOCK) ON f.SCIRefno = wo.SCIRefno
LEFT JOIN Construction cs WITH (NOLOCK) ON cs.ID = ConstructionID
LEFT JOIN Order_Eachcons oe WITH (NOLOCK) ON oe.Ukey = wo.Order_EachconsUkey
LEFT JOIN Pass1 p1 WITH (NOLOCK) ON p1.ID = wo.AddName
LEFT JOIN Pass1 p2 WITH (NOLOCK) ON p2.ID = wo.EditName
OUTER APPLY (
    SELECT PatternPanel_CONCAT = STUFF((
        SELECT CONCAT('+ ', PatternPanel)
        FROM WorkOrderForOutput_PatternPanel WITH (NOLOCK)
        WHERE WorkOrderForOutputUkey = wo.Ukey
        ORDER BY FabricPanelCode
        FOR XML PATH ('')), 1, 1, '')
) wp1
OUTER APPLY (
    SELECT FabricPanelCode_CONCAT = STUFF((
        SELECT CONCAT('+ ', FabricPanelCode)
        FROM WorkOrderForOutput_PatternPanel WITH (NOLOCK)
        WHERE WorkOrderForOutputUkey = wo.Ukey
        ORDER BY FabricPanelCode
        FOR XML PATH (''))
    , 1, 1, '')
) wp2
OUTER APPLY (
    SELECT Article_CONCAT = STUFF((
        SELECT DISTINCT CONCAT('/', Article)
        FROM WorkOrderForOutput_Distribute WITH (NOLOCK)
        WHERE WorkOrderForOutputUkey = wo.Ukey
        AND Article != ''
        FOR XML PATH ('')), 1, 1, '')
) wd
OUTER APPLY (
    SELECT SizeCode_CONCAT = STUFF((
        SELECT CONCAT(', ', SizeCode, '/ ', Qty)
        FROM WorkOrderForOutput_SizeRatio WITH (NOLOCK)
        WHERE WorkOrderForOutputUkey = wo.Ukey
        FOR XML PATH ('')), 1, 1, '')
) ws1
OUTER APPLY (
    SELECT TotalCutQty_CONCAT = STUFF((
        SELECT CONCAT(', ', Sizecode, '/ ', Qty * wo.Layer)
        FROM WorkOrderForOutput_SizeRatio WITH (NOLOCK)
        WHERE WorkOrderForOutputUkey = wo.Ukey
        FOR XML PATH ('')), 1, 1, '')
) ws2
OUTER APPLY (
    SELECT Fabeta = IIF(psd.Complete = 1, psd.FinalETA, IIF(psd.Eta IS NOT NULL, psd.eta, IIF(psd.shipeta IS NOT NULL, psd.shipeta, psd.finaletd)))
    FROM PO_Supp_Detail psd WITH (NOLOCK)
    WHERE EXISTS (SELECT 1 FROM Orders WITH (NOLOCK) WHERE CuttingSp = wo.ID AND POID = psd.id)
    AND psd.seq1 = wo.seq1
    AND psd.seq2 = wo.seq2
) psd
OUTER APPLY (
    SELECT Sewinline = MIN(ss.Inline)
    FROM SewingSchedule ss WITH (NOLOCK)
    INNER JOIN SewingSchedule_detail ssd WITH (NOLOCK) ON ssd.id = ss.id
    INNER JOIN WorkOrderForOutput_Distribute wd WITH (NOLOCK) ON wd.OrderID = ssd.OrderID AND wd.Article = ssd.Article AND wd.SizeCode = ssd.SizeCode
    WHERE wd.WorkOrderForOutputUkey = wo.ukey
) ss
OUTER APPLY (
    SELECT Actcutdate = IIF(SUM(cod.Layer) = wo.Layer, MAX(co.cdate), NULL)
    FROM CuttingOutput co WITH (NOLOCK)
    INNER JOIN CuttingOutput_Detail cod WITH (NOLOCK)ON co.id = cod.id
    WHERE cod.WorkOrderForOutputUkey = wo.Ukey
    AND co.Status != 'New'
) co
WHERE wo.id = '{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.displayBoxStyle.Text = MyUtility.GetValue.Lookup($"SELECT StyleID FROM Orders WITH(NOLOCK) WHERE ID = '{this.CurrentMaintain["ID"]}'");

            foreach (DataRow row in this.DetailDatas)
            {
                row["MarkerLength_Mask"] = FormatMarkerLength(row["MarkerLength"].ToString());
                row["ActCuttingPerimeter_Mask"] = FormatData(row["ActCuttingPerimeter"].ToString());
                row["StraightLength_Mask"] = FormatData(row["StraightLength"].ToString());
                row["CurvedLength_Mask"] = FormatData(row["CurvedLength"].ToString());
            }

            this.GetAllDetailData();
        }

        private void GetAllDetailData()
        {
            string sqlcmd = $@"
SELECT *, tmpKey = CAST(0 AS BIGINT) FROM WorkOrderForOutput_PatternPanel WITH (NOLOCK) WHERE ID = '{this.CurrentMaintain["ID"]}'
SELECT *, tmpKey = CAST(0 AS BIGINT) FROM WorkOrderForOutput_SizeRatio WITH (NOLOCK) WHERE ID = '{this.CurrentMaintain["ID"]}'

SELECT *
    ,tmpKey = CAST(0 AS BIGINT)
    ,Sewinline = (
        SELECT MIN(ss.Inline)
        FROM SewingSchedule_Detail ssd WITH (NOLOCK)
        LEFT JOIN SewingSchedule ss WITH (NOLOCK) ON ssd.ID = ss.ID
        WHERE ssd.OrderID = wd.OrderID
        AND ssd.Article = wd.Article
        AND ssd.SizeCode = wd.SizeCode
    )
FROM WorkOrderForOutput_Distribute wd WITH (NOLOCK)
WHERE wd.ID = '{this.CurrentMaintain["ID"]}'

SELECT *, tmpKey = CAST(0 AS BIGINT) FROM WorkOrderForOutput_SpreadingFabric WITH (NOLOCK) WHERE POID = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable[] dts);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.dtWorkOrderForOutput_PatternPanel = dts[0];
            this.dtWorkOrderForOutput_SizeRatio = dts[1];
            this.dtWorkOrderForOutput_Distribute = dts[2];

            this.sizeRatiobs.DataSource = this.dtWorkOrderForOutput_SizeRatio;
            this.distributebs.DataSource = this.dtWorkOrderForOutput_Distribute;
            this.spreadingfabricbs.DataSource = dts[3];

            // 右下角 Qty Break Down
            sqlcmd = $@"
SELECT
    wd.OrderID
   ,wd.Article
   ,wd.SizeCode
   ,wo.FabricCombo
   ,Qty = SUM(wd.Qty)
INTO #tmp
FROM WorkOrderForOutput wo WITH (NOLOCK)
INNER JOIN WorkOrderForOutput_Distribute wd WITH (NOLOCK) ON wo.ukey = wd.WorkOrderForOutputukey
WHERE wo.id = '{this.CurrentMaintain["ID"]}'
GROUP BY wo.FabricCombo
        ,wd.article
        ,wd.SizeCode
        ,wd.OrderID

SELECT
    oq.ID
   ,oq.Article
   ,oq.SizeCode
   ,oq.Qty
   ,Balance = ISNULL(balc.minQty - oq.qty, 0)
FROM Order_Qty oq WITH (NOLOCK)
INNER JOIN Orders o WITH (NOLOCK) ON oq.id = o.id
OUTER APPLY (
    SELECT minQty = MIN(Qty)
    FROM #tmp t
    WHERE t.OrderID = oq.ID
    AND t.article = oq.Article
    AND t.SizeCode = oq.SizeCode
) balc
WHERE o.CuttingSP = '{this.CurrentMaintain["ID"]}'
ORDER BY ID, Article, SizeCode
DROP TABLE #tmp
";
            result = DBProxy.Current.Select(null, sqlcmd, out DataTable dtQtyBreakDown);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.qtybreakds.DataSource = dtQtyBreakDown;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("CutRef", header: "CutRef#", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true).Get(out this.col_CutRef)
                .Numeric("Cutno", header: "Cut#", width: Ict.Win.Widths.AnsiChars(5))
                .Text("MarkerName", header: "Marker\r\nName", width: Ict.Win.Widths.AnsiChars(5))
                .Text("PatternPanel_CONCAT", header: "Pattern Panel", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("FabricPanelCode_CONCAT", header: "Fabric\r\nPanel Code", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("OrderId", header: "SP#", width: Ict.Win.Widths.AnsiChars(13)).Get(out this.col_OrderID)
                .Text("SEQ1", header: "Seq1", width: Ict.Win.Widths.AnsiChars(3)).Get(out this.col_Seq1)
                .Text("SEQ2", header: "Seq2", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_Seq2)
                .Text("Article_CONCAT", header: "Article", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ColorId", header: "Color", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Tone", header: "Tone", width: Ict.Win.Widths.AnsiChars(4))
                .Text("SizeCode_CONCAT", header: "Size", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Layer", header: "Layers", width: Ict.Win.Widths.AnsiChars(5), integer_places: 5, maximum: 99999).Get(out this.col_Layer)
                .Text("TotalCutQty_CONCAT", header: "Total CutQty", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("WKETA", header: "WK ETA", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true).Get(out this.col_WKETA)
                .Date("Fabeta", header: "Fabric Arr Date", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("Sewinline", header: "Sewing inline", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("EstCutDate", header: "Est. Cut Date", width: Ict.Win.Widths.AnsiChars(10)).Get(out this.EstCutDate)
                .Date("Actcutdate", header: "Act. Cut Date", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SpreadingNoID", header: "Spreading No", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_SpreadingNoID)
                .Text("CutCellID", header: "Cut Cell", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_CutCellID)
                .Text("Shift", header: "Shift", width: Ict.Win.Widths.AnsiChars(2), settings: CellTextDropDownList.GetGridCell("Pms_WorkOrderShift"))
                .MaskedText("MarkerLength_Mask", "00Y00-0/0+0\"", "Marker Length", name: "MarkerLength", width: Ict.Win.Widths.AnsiChars(16)).Get(out this.col_MarkerLength)
                .MaskedText("ActCuttingPerimeter_Mask", "000Yd00\"00", "ActCutting Perimeter", name: "ActCuttingPerimeter", width: Ict.Win.Widths.AnsiChars(16)).Get(out this.col_ActCuttingPerimeter)
                .MaskedText("StraightLength_Mask", "000Yd00\"00", "StraightLength", name: "StraightLength", width: Ict.Win.Widths.AnsiChars(16)).Get(out this.col_StraightLength)
                .MaskedText("CurvedLength_Mask", "000Yd00\"00", "CurvedLength", name: "CurvedLength", width: Ict.Win.Widths.AnsiChars(16)).Get(out this.col_CurvedLength)
                .Text("MarkerNo", header: "Pattern No.", width: Ict.Win.Widths.AnsiChars(10)).Get(out this.col_MarkerNo)
                .Text("Adduser", header: "Add Name", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("AddDate", header: "Add Date", width: Ict.Win.Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Edituser", header: "Edit Name", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("EditDate", header: "Edit Date", width: Ict.Win.Widths.AnsiChars(20), iseditingreadonly: true)
                ;
            this.Helper.Controls.Grid.Generator(this.gridSizeRatio)
                .Text("SizeCode", header: "Size", width: Ict.Win.Widths.AnsiChars(3)).Get(out this.col_SizeRatio_Size)
                .Numeric("Qty", header: "Ratio", width: Ict.Win.Widths.AnsiChars(6), integer_places: 5, maximum: 99999, minimum: 0).Get(out this.col_SizeRatio_Qty)
                ;
            this.Helper.Controls.Grid.Generator(this.gridDistributeToSP)
                .Text("OrderID", header: "SP#", width: Ict.Win.Widths.AnsiChars(13)).Get(out this.col_Distribute_SP)
                .Text("Article", header: "Article", width: Ict.Win.Widths.AnsiChars(7)).Get(out this.col_Distribute_Article)
                .Text("SizeCode", header: "Size", width: Ict.Win.Widths.AnsiChars(4)).Get(out this.col_Distribute_Size)
                .Numeric("Qty", header: "Qty", width: Ict.Win.Widths.AnsiChars(3), integer_places: 6, maximum: 999999, minimum: 0).Get(out this.col_Distribute_Qty)
                .Date("SewInline", header: "Inline Date", width: Ict.Win.Widths.AnsiChars(8), iseditingreadonly: true)
                ;
            this.Helper.Controls.Grid.Generator(this.gridSpreadingFabric)
                .Text("Seq1", header: "Seq1", width: Ict.Win.Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Seq2", header: "Seq2", width: Ict.Win.Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Roll", header: "Roll", width: Ict.Win.Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Ict.Win.Widths.AnsiChars(3), iseditingreadonly: true)
                ;
            this.Helper.Controls.Grid.Generator(this.gridQtyBreakDown)
                .Text("ID", header: "SP#", width: Ict.Win.Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Ict.Win.Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Ict.Win.Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("Qty", header: "Order\nQty", width: Ict.Win.Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("Balance", header: "Balance", width: Ict.Win.Widths.AnsiChars(5), iseditingreadonly: true);

            this.GridEventSet();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridRowChanged()
        {
            this.GridValidateControl();
            base.OnDetailGridRowChanged();

            if (this.CurrentDetailData == null)
            {
                return;
            }

            this.bindingSourceDetail.SetRow(this.CurrentDetailData);

            // 變更子表可否編輯
            bool canEdit = this.CanEditData(this.CurrentDetailData);
            this.cmsSizeRatio.Enabled = canEdit;
            this.cmsDistribute.Enabled = canEdit;
            this.gridSizeRatio.IsEditingReadOnly = !canEdit;
            this.gridDistributeToSP.IsEditingReadOnly = !canEdit;

            string filter = GetFilter(this.CurrentDetailData, CuttingForm.P09);
            this.sizeRatiobs.Filter = filter;
            this.distributebs.Filter = filter;
            this.gridDistributeToSP.SelectRowToFirst();

            this.spreadingfabricbs.Filter = $"CutRef = '{this.CurrentDetailData["CutRef"]}'";
        }

        #region Import 與 Save , 刪除/新增 與 AGV
        private void BtnImportFromWorkOrderForPlanning_Click(object sender, EventArgs e)
        {
            this.OnDetailEntered();
            #region 校驗
            string sqlcmdCheck;
            DataTable dtCheck;
            DualResult result;
            string msg;
            if (this.DetailDatas.Count > 0)
            {
                // 1. 存在 P10 Bundle
                msg = "The following bundle data exists and cannot be imported again. If you need to re-import, please go to [Cutting_P10. Bundle Card] to delete the bundle data.";
                if (!CheckBundleAndShowData(this.DetailDatas.AsEnumerable().Select(r => MyUtility.Convert.GetString(r["CutRef"])).ToList(), msg))
                {
                    return;
                }

                // 2. 存在 P20 CuttingOutput
                msg = "The following cutting output data exists and cannot be imported again. If you need to re-import, please go to [Cutting_P20. Cutting Daily Output] to delete the cutting output data.";
                if (!CheckCuttingOutputAndShowData(this.DetailDatas.AsEnumerable().Select(r => MyUtility.Convert.GetString(r["CutRef"])).ToList(), msg))
                {
                    return;
                }

                // 3. 存在 P05 MarkerReq_Detail
                msg = "The following marker request data exists and cannot be imported again. If you need to re-import, please go to [Cutting_P05. Bulk Marker Request] to delete the marker request data.";
                if (!CheckMarkerReqAndShowData(this.DetailDatas.AsEnumerable().Select(r => MyUtility.Convert.GetString(r["CutRef"])).ToList(), msg))
                {
                    return;
                }

                // 4. 欄位 SpreadingStatus
                // DataTable dt = this.DetailDatas.AsEnumerable().Where(row => !MyUtility.Convert.GetString(row["SpreadingStatus"]).Equals("Ready", StringComparison.OrdinalIgnoreCase)).TryCopyToDataTable((DataTable)this.detailgridbs.DataSource);
                sqlcmdCheck = $@"
SELECT
    [Cut Ref#] = WorkOrderForOutput.CutRef
   ,[Cut #] = WorkOrderForOutput.CutNo
   ,[Marker Name] = WorkOrderForOutput.MarkerName
   ,[Pattern Panel] = PatternPanel.PatternPanel
   ,[Fabric Panel Code] = FabricPanelCode.FabricPanelCode
   ,[Spreading Status] = WorkOrderForOutput.SpreadingStatus
   ,[Spreading Remark] = WorkOrderForOutput.SpreadingRemark
FROM WorkOrderForOutput WITH (NOLOCK)
OUTER APPLY (
    SELECT PatternPanel = STUFF((
        SELECT ', ' + PatternPanel
        FROM WorkOrderForOutput_PatternPanel WITH (NOLOCK)
        WHERE WorkOrderForOutput_PatternPanel.WorkOrderForOutputUkey = WorkOrderForOutput.Ukey
        GROUP BY WorkOrderForOutput_PatternPanel.PatternPanel
        ORDER BY WorkOrderForOutput_PatternPanel.PatternPanel
        FOR XML PATH ('')), 1, 2, '')
) PatternPanel
OUTER APPLY (
    SELECT FabricPanelCode = STUFF((
        SELECT ', ' + FabricPanelCode
        FROM WorkOrderForOutput_PatternPanel WITH (NOLOCK)
        WHERE WorkOrderForOutput_PatternPanel.WorkOrderForOutputUkey = WorkOrderForOutput.Ukey
        GROUP BY WorkOrderForOutput_PatternPanel.FabricPanelCode
        ORDER BY WorkOrderForOutput_PatternPanel.FabricPanelCode
        FOR XML PATH ('')), 1, 2, '')
) FabricPanelCode
WHERE ID = '{this.CurrentMaintain["ID"]}'
AND WorkOrderForOutput.SpreadingStatus <> 'Ready'
";
                result = DBProxy.Current.Select(string.Empty, sqlcmdCheck, out dtCheck);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                if (dtCheck.Rows.Count > 0)
                {
                    msg = "The following digitail spreading data exists and cannot be imported";
                    MsgGridForm m = new MsgGridForm(dtCheck, msg, "Exists digitail spreading data") { Width = 950 };
                    m.grid1.Columns[0].Width = 140;
                    m.text_Find.Width = 140;
                    m.btn_Find.Location = new Point(450, 6);
                    m.btn_Find.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    this.FormClosing += (s, args) =>
                    {
                        if (m.Visible)
                        {
                            m.Close();
                        }
                    };
                    m.Show(this);
                    return;
                }

                // 5. 有 WorkOrderForOutput 才跳視窗
                DialogResult confirmResult = MessageBoxEX.Show("Work order data already exists, do you want to overwrite it?", "Warning", MessageBoxButtons.YesNo, new string[] { "Yes", "No" }, MessageBoxDefaultButton.Button2);
                if (confirmResult != DialogResult.Yes)
                {
                    return;
                }
            }
            #endregion

            #region 執行

            // 1. Delete
            string sqlcmd = $@"
DELETE WorkOrderForOutput WHERE ID = '{this.CurrentMaintain["ID"]}'
DELETE WorkOrderForOutput_Distribute WHERE ID = '{this.CurrentMaintain["ID"]}'
DELETE WorkOrderForOutput_PatternPanel WHERE ID = '{this.CurrentMaintain["ID"]}'
DELETE WorkOrderForOutput_SizeRatio WHERE ID = '{this.CurrentMaintain["ID"]}'
DELETE WorkOrderForOutput_SpreadingFabric WHERE POID = '{this.CurrentMaintain["ID"]}'
DELETE WorkOrderForOutputHistory WHERE ID = '{this.CurrentMaintain["ID"]}'
DELETE WorkOrderForOutputDelete WHERE ID = '{this.CurrentMaintain["ID"]}'
";

            // 2. WorkOrderForOutput
            sqlcmd += $@"
INSERT INTO WorkOrderForOutput (
    ID,
    FactoryID,
    MDivisionID,
    Seq1,
    Seq2,
    CutRef,
    CutNo,
    OrderID,
    RefNo,
    SCIRefNo,
    ColorID,
    Tone,
    Layer,
    FabricCombo,
    FabricCode,
    FabricPanelCode,
    EstCutDate,
    ConsPC,
    Cons,
    MarkerNo,
    MarkerVersion,
    MarkerName,
    MarkerLength,
    ActCuttingPerimeter,
    StraightLength,
    CurvedLength,
    Shift,
    CutCellID,
    SpreadingNoID,
    UnfinishedCuttingReason,
    IsCreateByUser,
    SpreadingStatus,
    SpreadingRemark,
    GroupID,
    WorkOrderForPlanningUkey,
    Order_EachconsUkey,
    AddName,
    AddDate,
    EditName,
    EditDate
)
SELECT
    WorkOrderForPlanning.ID
    ,WorkOrderForPlanning.FactoryID
    ,WorkOrderForPlanning.MDivisionID
    ,WorkOrderForPlanning.Seq1
    ,WorkOrderForPlanning.Seq2
    ,WorkOrderForPlanning.CutRef
    ,''--CutNo
    ,''--OrderID 在寫入 WorkOrderForOutput_Distribute 之後,取最小的填入
    ,WorkOrderForPlanning.RefNo
    ,WorkOrderForPlanning.SCIRefNo
    ,WorkOrderForPlanning.ColorID
    ,WorkOrderForPlanning.Tone
    ,WorkOrderForPlanning.Layer
    ,WorkOrderForPlanning.FabricCombo
    ,WorkOrderForPlanning.FabricCode
    ,WorkOrderForPlanning.FabricPanelCode
    ,WorkOrderForPlanning.EstCutDate
    ,WorkOrderForPlanning.ConsPC
    ,WorkOrderForPlanning.Cons
    ,WorkOrderForPlanning.MarkerNo
    ,WorkOrderForPlanning.MarkerVersion
    ,WorkOrderForPlanning.MarkerName
    ,WorkOrderForPlanning.MarkerLength
    ,Order_EachCons.ActCuttingPerimeter
    ,Order_EachCons.StraightLength
    ,Order_EachCons.CurvedLength
    ,''--Shift
    ,''--CutCellID
    ,''--SpreadingNoID
    ,''--UnfinishedCuttingReason
    ,WorkOrderForPlanning.IsCreateByUser
    ,''--SpreadingStatus
    ,''--SpreadingRemark
    ,''--GroupID
    ,WorkOrderForPlanning.Ukey
    ,WorkOrderForPlanning.Order_EachconsUkey
    ,'{Env.User.UserID}'
    ,GETDATE()
    ,''--EditName
    ,NULL
FROM WorkOrderForPlanning
INNER JOIN Order_EachCons ON WorkOrderForPlanning.Order_EachconsUkey = Order_EachCons.Ukey
WHERE WorkOrderForPlanning.ID = '{this.CurrentMaintain["ID"]}'
ORDER BY WorkOrderForPlanning.Ukey
";

            // 3.WorkOrderForOutput_PatternPanel
            sqlcmd += $@"
INSERT INTO WorkOrderForOutput_PatternPanel (
    WorkOrderForOutputUkey,
    ID,
    PatternPanel,
    FabricPanelCode
)
SELECT
    WorkOrderForOutput.Ukey,
    WorkOrderForOutput.ID,
    WorkOrderForPlanning_PatternPanel.PatternPanel,
    WorkOrderForPlanning_PatternPanel.FabricPanelCode
FROM WorkOrderForOutput
JOIN WorkOrderForPlanning_PatternPanel ON WorkOrderForOutput.WorkOrderForPlanningUkey = WorkOrderForPlanning_PatternPanel.WorkOrderForPlanningUkey
";

            // 4.WorkOrderForOutput_SizeRatio
            sqlcmd += $@"
INSERT INTO WorkOrderForOutput_SizeRatio (
    WorkOrderForOutputUkey,
    ID,
    SizeCode,
    Qty
)
SELECT
    WorkOrderForOutput.Ukey,
    WorkOrderForPlanning_SizeRatio.ID,
    WorkOrderForPlanning_SizeRatio.SizeCode,
    WorkOrderForPlanning_SizeRatio.Qty
FROM WorkOrderForOutput
JOIN WorkOrderForPlanning_SizeRatio ON WorkOrderForOutput.WorkOrderForPlanningUkey = WorkOrderForPlanning_SizeRatio.WorkOrderForPlanningUkey
";

            // 撈出所有 Ukey 後續分配 Distribute
            sqlcmd += $@"
SELECT Ukey
FROM WorkOrderForOutput WITH(NOLOCK)
WHERE WorkOrderForOutput.ID = '{this.CurrentMaintain["ID"]}'
ORDER BY Ukey
";

            // Distribute 寫入之後才執行
            string sqlUpdateOrderID = $@"
UPDATE WorkOrderForOutput
SET OrderID = (SELECT MIN(OrderID) FROM WorkOrderForOutput_Distribute WITH(NOLOCK) WHERE WorkOrderForOutputUkey = WorkOrderForOutput.Ukey AND OrderID <> 'EXCESS')
WHERE ID = ''
";

            result = DBProxy._OpenConnection(null, out SqlConnection sqlConn);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5)))
            {
                using (sqlConn)
                {
                    try
                    {
                        result = DBProxy.Current.SelectByConn(sqlConn, sqlcmd, out DataTable dtUkey);
                        if (!result)
                        {
                            this.ShowErr(result);
                            return;
                        }

                        if (dtUkey.Rows.Count == 0)
                        {
                            return;
                        }

                        List<long> listWorkOrderUkey = dtUkey.AsEnumerable().Select(x => MyUtility.Convert.GetLong(x["Ukey"])).ToList();
                        result = InsertWorkOrder_Distribute(this.CurrentMaintain["ID"].ToString(), listWorkOrderUkey, CuttingForm.P09, sqlConn);
                        if (!result)
                        {
                            this.ShowErr(result);
                            return;
                        }

                        result = DBProxy.Current.ExecuteByConn(sqlConn, sqlUpdateOrderID);
                        if (!result)
                        {
                            this.ShowErr(result);
                            return;
                        }

                        transactionscope.Complete();
                    }
                    catch (Exception ex)
                    {
                        this.ShowErr(ex);
                        return;
                    }
                }
            }
            #endregion

            this.OnDetailEntered();
            MyUtility.Msg.InfoBox("Import successful");
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            this.GridValidateControl();

            #region 檢查 主表身
            if (!ValidateDetailDatasEmpty(this.DetailDatas, this.detailgrid))
            {
                return false;
            }

            if (!ValidateCutNoAndFabricCombo(this.DetailDatas, this.detailgrid))
            {
                return false;
            }

            if (!ValidateCutNoAndMarkerDetails(this.DetailDatas, this.CheckContinue))
            {
                return false;
            }

            if (!ValidateCutref(this.DetailDatas, this.CheckContinue))
            {
                return false;
            }
            #endregion

            #region 第3層 處理

            // Step 1. 刪除空值
            this.dtWorkOrderForOutput_SizeRatio.Select("Qty = 0 OR SizeCode = ''").Delete();
            this.dtWorkOrderForOutput_Distribute.Select("Qty = 0 OR OrderID = '' OR SizeCode = ''").Delete();
            this.dtWorkOrderForOutput_Distribute.Select("OrderID <> 'EXCESS' AND Article = ''").Delete(); // EXCESS 項 Article 為空

            // 檢查 第3層 重複 Key
            var checkSizeRatio = new List<string> { "WorkOrderForOutputUkey", "tmpKey", "SizeCode" };
            if (!Prgs.CheckForDuplicateKeys(this.dtWorkOrderForOutput_SizeRatio, checkSizeRatio, out DataTable dtCheck))
            {
                MyUtility.Msg.WarningBox("The SizeRatio duplicate ,Please see below <Ukey>");
                return false;
            }

            var checkDistribute = new List<string> { "WorkOrderForOutputUkey", "tmpKey", "OrderID", "Article", "SizeCode" };
            if (!Prgs.CheckForDuplicateKeys(this.dtWorkOrderForOutput_Distribute, checkDistribute, out dtCheck))
            {
                MyUtility.Msg.WarningBox("The Distribute duplicate ,Please see below <Ukey>");
                return false;
            }

            var checkPatternPanel = new List<string> { "WorkOrderForOutputUkey", "tmpKey", "PatternPanel", "FabricPanelCode" };
            if (!Prgs.CheckForDuplicateKeys(this.dtWorkOrderForOutput_PatternPanel, checkPatternPanel, out dtCheck))
            {
                MyUtility.Msg.WarningBox("The PatternPanel duplicate ,Please see below <Ukey>");
                return false;
            }

            // 檢查第3層  Total distributionQty 是否大於 TotalCutQty 總和
            foreach (DataRow dr in this.DetailDatas)
            {
                string filter = GetFilter(dr, CuttingForm.P09);
                decimal ttlCutQty = this.dtWorkOrderForOutput_SizeRatio.Select(filter).Sum(row => MyUtility.Convert.GetInt(row["Qty"])) * MyUtility.Convert.GetDecimal(dr["Layer"]);
                decimal ttlDisQty = this.dtWorkOrderForOutput_Distribute.Select(filter).Sum(row => MyUtility.Convert.GetInt(row["Qty"]));
                if (ttlCutQty < ttlDisQty)
                {
                    this.ShowErr($"Key:{dr["Ukey"]} Distribution Qty can not exceed total Cut qty");
                    return false;
                }
            }
            #endregion

            // 更新 Cutting 欄位
            this.CurrentMaintain["CutForOutputInline"] = this.DetailDatas.AsEnumerable().Min(row => MyUtility.Convert.GetDate(row["EstCutDate"])) ?? (object)DBNull.Value;
            this.CurrentMaintain["CutForOutputOffline"] = this.DetailDatas.AsEnumerable().Max(row => MyUtility.Convert.GetDate(row["EstCutDate"])) ?? (object)DBNull.Value;

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            // Stpe 1. 給第3層填入對應 WorkOrderForOutputUkey
            foreach (DataRow dr in this.DetailDatas.Where(row => row.RowState == DataRowState.Added))
            {
                long ukey = MyUtility.Convert.GetLong(dr["Ukey"]); // ClickSavePost 時,底層已取得 Key 值
                string filterAddData = $"tmpkey = {dr["tmpkey"]}";
                this.dtWorkOrderForOutput_SizeRatio.Select(filterAddData).AsEnumerable().ToList().ForEach(row => row["WorkOrderForOutputUkey"] = ukey);
                this.dtWorkOrderForOutput_Distribute.Select(filterAddData).AsEnumerable().ToList().ForEach(row => row["WorkOrderForOutputUkey"] = ukey);
                this.dtWorkOrderForOutput_PatternPanel.Select(filterAddData).AsEnumerable().ToList().ForEach(row => row["WorkOrderForOutputUkey"] = ukey);
            }

            #region 處理 SizeRatio
            string sqlDeleteSizeRatio = $@"
DELETE wd
OUTPUT DELETED.*
FROM WorkOrderForOutput_SizeRatio wd
LEFT JOIN #tmp t ON t.WorkOrderForOutputUkey = wd.WorkOrderForOutputUkey AND t.SizeCode = wd.SizeCode
WHERE wd.id = '{this.CurrentMaintain["ID"]}'
AND t.WorkOrderForOutputUkey IS NULL
";

            string sqlUpdateSizeRatio = $@"
UPDATE wd
SET wd.Qty = t.Qty
OUTPUT INSERTED.*
FROM WorkOrderForOutput_SizeRatio wd
INNER JOIN #tmp t ON t.WorkOrderForOutputUkey = wd.WorkOrderForOutputUkey AND t.SizeCode = wd.SizeCode
WHERE wd.id = '{this.CurrentMaintain["ID"]}'
";

            string sqlInsertSizeRatio = $@"

INSERT INTO WorkOrderForOutput_SizeRatio (WorkOrderForOutputUkey, ID, SizeCode, Qty)
OUTPUT INSERTED.*
SELECT
    t.WorkOrderForOutputUkey
    ,t.ID
    ,t.SizeCode
    ,t.Qty
FROM #tmp t
LEFT JOIN WorkOrderForOutput_SizeRatio wd ON t.WorkOrderForOutputUkey = wd.WorkOrderForOutputUkey AND t.SizeCode = wd.SizeCode
WHERE wd.WorkOrderForOutputUkey IS NULL
";
            DualResult result;
            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dtWorkOrderForOutput_SizeRatio, string.Empty, sqlDeleteSizeRatio, out DataTable dtDeleteSizeRatio)))
            {
                return result;
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dtWorkOrderForOutput_SizeRatio, string.Empty, sqlUpdateSizeRatio, out DataTable dtUpdateSizeRatio)))
            {
                return result;
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dtWorkOrderForOutput_SizeRatio, string.Empty, sqlInsertSizeRatio, out DataTable dtInsertSizeRatio)))
            {
                return result;
            }
            #endregion

            #region 處理 PatternPanel, 沒有 update 因為一定是同 ID
            string sqlDeletePatternPanel = $@"
DELETE wd
FROM WorkOrderForOutput_PatternPanel wd
LEFT JOIN #tmp t ON t.WorkOrderForOutputUkey = wd.WorkOrderForOutputUkey AND t.PatternPanel = wd.PatternPanel AND t.FabricPanelCode = wd.FabricPanelCode
WHERE wd.id = '{this.CurrentMaintain["ID"]}'
AND t.WorkOrderForOutputUkey IS NULL
";
            string sqlInsertPatternPanel = $@"
INSERT INTO WorkOrderForOutput_PatternPanel (WorkOrderForOutputUkey, ID, PatternPanel, FabricPanelCode)
SELECT
    t.WorkOrderForOutputUkey
    ,t.ID
    ,t.PatternPanel
    ,t.FabricPanelCode
FROM #tmp t
LEFT JOIN WorkOrderForOutput_PatternPanel wd ON t.WorkOrderForOutputUkey = wd.WorkOrderForOutputUkey AND t.PatternPanel = wd.PatternPanel AND t.FabricPanelCode = wd.FabricPanelCode
WHERE wd.WorkOrderForOutputUkey IS NULL
";
            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dtWorkOrderForOutput_PatternPanel, string.Empty, sqlDeletePatternPanel, out DataTable dtDeletePatternPanel)))
            {
                return result;
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dtWorkOrderForOutput_PatternPanel, string.Empty, sqlInsertPatternPanel, out DataTable dtInsertPatternPanel)))
            {
                return result;
            }
            #endregion

            #region 處理 Distribute 有要傳 API

            // API需要 Distribute 新刪修 同時 out 資訊
            string sqlDeleteDistribute = $@"
DELETE wd
OUTPUT DELETED.*
FROM WorkOrderForOutput_Distribute wd
LEFT JOIN #tmp t ON t.WorkOrderForOutputUkey = wd.WorkOrderForOutputUkey AND t.OrderID = wd.OrderID AND t.Article = wd.Article AND t.SizeCode = wd.SizeCode
WHERE wd.id = '{this.CurrentMaintain["ID"]}'
AND t.WorkOrderForOutputUkey IS NULL
";
            string sqlUpdateDistribute = $@"
UPDATE wd
SET wd.Qty = t.Qty
OUTPUT INSERTED.*
FROM WorkOrderForOutput_Distribute wd
INNER JOIN #tmp t ON t.WorkOrderForOutputUkey = wd.WorkOrderForOutputUkey AND t.OrderID = wd.OrderID AND t.Article = wd.Article AND t.SizeCode = wd.SizeCode
WHERE wd.id = '{this.CurrentMaintain["ID"]}'
";
            string sqlInsertDistribute = $@"
INSERT INTO WorkOrderForOutput_Distribute (WorkOrderForOutputUkey, ID, OrderID, Article, SizeCode, Qty)
OUTPUT INSERTED.*
SELECT
    t.WorkOrderForOutputUkey
    ,t.ID
    ,t.OrderID
    ,ISNULL(t.Article, '')
    ,t.SizeCode
    ,t.Qty
FROM #tmp t
LEFT JOIN WorkOrderForOutput_Distribute wd ON t.WorkOrderForOutputUkey = wd.WorkOrderForOutputUkey AND t.OrderID = wd.OrderID AND t.Article = wd.Article AND t.SizeCode = wd.SizeCode
WHERE wd.WorkOrderForOutputUkey IS NULL
";
            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dtWorkOrderForOutput_Distribute, string.Empty, sqlDeleteDistribute, out DataTable dtDeleteDistribute)))
            {
                return result;
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dtWorkOrderForOutput_Distribute, string.Empty, sqlUpdateDistribute, out DataTable dtUpdateDistribute)))
            {
                return result;
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dtWorkOrderForOutput_Distribute, string.Empty, sqlInsertDistribute, out DataTable dtInsertDistribute)))
            {
                return result;
            }
            #endregion

            #region sent data to GZ WebAPI
            List<Guozi_AGV.WorkOrder_Distribute> deleteWorkOrder_Distribute = new List<Guozi_AGV.WorkOrder_Distribute>();
            List<Guozi_AGV.WorkOrder_Distribute> editWorkOrder_Distribute = new List<Guozi_AGV.WorkOrder_Distribute>();
            List<long> deleteWorkOrder = new List<long>();
            foreach (DataRow dr in dtDeleteDistribute.Rows)
            {
                deleteWorkOrder_Distribute.Add(new Guozi_AGV.WorkOrder_Distribute()
                {
                    WorkOrderUkey = MyUtility.Convert.GetLong(dr["WorkOrderForOutputUkey"]),
                    SizeCode = MyUtility.Convert.GetString(dr["SizeCode"]),
                    Article = MyUtility.Convert.GetString(dr["Article"]),
                    OrderID = MyUtility.Convert.GetString(dr["OrderID"]),
                });
            }

            dtUpdateDistribute.ExtNotDeletedRowsForeach(row => editWorkOrder_Distribute.Add(new Guozi_AGV.WorkOrder_Distribute() { WorkOrderUkey = MyUtility.Convert.GetLong(row["WorkOrderForOutputUkey"]) }));
            dtInsertDistribute.ExtNotDeletedRowsForeach(row => editWorkOrder_Distribute.Add(new Guozi_AGV.WorkOrder_Distribute() { WorkOrderUkey = MyUtility.Convert.GetLong(row["WorkOrderForOutputUkey"]) }));

            string compareCol = "CutRef,EstCutDate,ID,OrderID,CutCellID";

            // 準備調整清單, 此3狀況要傳 1.新增 2.compareCol欄位真的有變動 3.有變動 Distribute
            var listChangedDetail = this.DetailDatas
                .Where(s =>
                {
                    return !MyUtility.Check.Empty(s["CutRef"]) &&
                        (s.RowState == DataRowState.Added ||
                         (s.RowState == DataRowState.Modified && s.CompareDataRowVersionValue(compareCol)) ||
                         editWorkOrder_Distribute.Any(ed => ed.WorkOrderUkey == MyUtility.Convert.GetLong(s["Ukey"])));
                });

            // 傳送變更資訊
            if (listChangedDetail.Any())
            {
                DataTable dtWorkOrder = listChangedDetail.CopyToDataTable();
                Task.Run(() => new Guozi_AGV().SentWorkOrderToAGV(dtWorkOrder))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }

            // CutRef 被清空要傳 Delete 給廠商
            var cutRefToEmpty = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(s => s.RowState == DataRowState.Modified &&
                                                             MyUtility.Check.Empty(s["CutRef", DataRowVersion.Current]) &&
                                                             !MyUtility.Check.Empty(s["CutRef", DataRowVersion.Original]));

            deleteWorkOrder_Distribute.AddRange(
                dtDeleteDistribute.AsEnumerable()
                .Select(row => new Guozi_AGV.WorkOrder_Distribute
                {
                    WorkOrderUkey = MyUtility.Convert.GetLong(row["WorkOrderForOutputUkey", DataRowVersion.Original]),
                    SizeCode = MyUtility.Convert.GetString(row["SizeCode", DataRowVersion.Original]),
                    Article = MyUtility.Convert.GetString(row["Article", DataRowVersion.Original]),
                    OrderID = MyUtility.Convert.GetString(row["OrderID", DataRowVersion.Original]),
                }));

            foreach (var item in cutRefToEmpty)
            {
                deleteWorkOrder.Add(MyUtility.Convert.GetLong(item["Ukey"]));

                // 不是刪除的,是被清空CutRef 對應的 Distribute
                deleteWorkOrder_Distribute.AddRange(
                    this.dtWorkOrderForOutput_Distribute.AsEnumerable()
                    .Where(x => x.RowState != DataRowState.Deleted && (MyUtility.Convert.GetLong(x["WorkOrderForOutputUkey"]) == MyUtility.Convert.GetLong(item["Ukey"])))
                    .Select(s => new Guozi_AGV.WorkOrder_Distribute
                    {
                        WorkOrderUkey = MyUtility.Convert.GetLong(s["WorkOrderForOutputUkey", DataRowVersion.Original]),
                        SizeCode = MyUtility.Convert.GetString(s["SizeCode", DataRowVersion.Original]),
                        Article = MyUtility.Convert.GetString(s["Article", DataRowVersion.Original]),
                        OrderID = MyUtility.Convert.GetString(s["OrderID", DataRowVersion.Original]),
                    }));
            }

            deleteWorkOrder.AddRange(
                ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                .Where(s => s.RowState == DataRowState.Deleted)
                .Select(s => MyUtility.Convert.GetLong(s["Ukey", DataRowVersion.Original])));

            // 傳送刪除資訊
            Task.Run(() => new Guozi_AGV().SentDeleteWorkOrder(deleteWorkOrder));
            Task.Run(() => new Guozi_AGV().SentDeleteWorkOrder_Distribute(deleteWorkOrder_Distribute));

            #endregion

            this.ReUpdateP20 = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(w => w.RowState != DataRowState.Unchanged).Any() ||
                dtDeleteSizeRatio.AsEnumerable().Any() || dtUpdateSizeRatio.AsEnumerable().Any() || dtInsertSizeRatio.AsEnumerable().Any() ||
                dtDeleteDistribute.AsEnumerable().Any() || dtUpdateDistribute.AsEnumerable().Any() || dtUpdateDistribute.AsEnumerable().Any() ||
                dtDeletePatternPanel.AsEnumerable().Any() || dtInsertPatternPanel.AsEnumerable().Any();

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();

            // 更新 P20
            this.BackgroundWorker1.RunWorkerAsync();

            this.OnDetailEntered();
        }

        private void BackgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (!this.ReUpdateP20)
            {
                e.Cancel = true;
            }
            else
            {
                string sqlcmd = $@"
DECLARE @ID varchar(13),@cDate date,@Manpower  int,@ManHours numeric(5,1)
DECLARE CURSOR_ CURSOR FOR
SELECT DISTINCT co.ID,co.cDate,co.Manpower,co.ManHours
FROM WorkOrderForOutput w WITH (NOLOCK)
INNER JOIN CuttingOutput_Detail cod WITH (NOLOCK) ON cod.WorkOrderForOutputUkey = w.Ukey
INNER JOIN CuttingOutput co WITH (NOLOCK) ON co.ID = cod.id
WHERE w.id = '{this.CurrentMaintain["ID"]}'
ORDER BY co.cDate

OPEN CURSOR_
FETCH NEXT FROM CURSOR_ INTO  @ID ,@cDate ,@Manpower  ,@ManHours 
While @@FETCH_STATUS = 0
Begin
    EXEC Cutting_P20_CFM_Update @ID,@cDate,@Manpower,@ManHours,'Confirm'
FETCH NEXT FROM CURSOR_ INTO @ID, @cDate, @Manpower, @ManHours
END
CLOSE CURSOR_
DEALLOCATE CURSOR_
";
                DBProxy.Current.Execute(null, sqlcmd);
            }
        }
        #endregion

        #region 單筆操作 彈窗ActionCutRef 新/修/刪
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            #region 校驗
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("There is no Workorder data that can be modified.");
                return;
            }

            if (!this.CheckAndMsg("modify"))
            {
                return;
            }
            #endregion

            // 單筆編輯視窗
            this.ShowDialogActionCutRef(DialogAction.Edit);
            this.OnDetailGridRowChanged();
        }

        // 按 + 或 插入 Icon
        protected override void OnDetailGridInsert(int index = -1)
        {
            // 先保留原本, 按插入按鈕時複製用
            DataRow oldRow = this.CurrentDetailData;

            // 底層插入之後 this.CurrentDetailData 是新的那筆
            base.OnDetailGridInsert(index);

            // 先取得當前編輯狀態的最新 tmpKey
            this.CurrentDetailData["tmpKey"] = this.DetailDatas.AsEnumerable().Max(row => MyUtility.Convert.GetLong(row["tmpKey"])) + 1;
            this.CurrentDetailData["SpreadingStatus"] = "Ready";

            if (oldRow == null)
            {
                // 第一筆只能從 Cutting 欄位帶入
                this.CurrentDetailData["FactoryID"] = this.CurrentMaintain["FactoryID"];
                this.CurrentDetailData["MDivisionId"] = this.CurrentMaintain["MDivisionId"];
                this.CurrentDetailData["OrderID"] = this.CurrentMaintain["ID"];
            }
            else
            {
                this.CurrentDetailData["FactoryID"] = oldRow["FactoryID"];
                this.CurrentDetailData["MDivisionId"] = oldRow["MDivisionId"];
                this.CurrentDetailData["MarkerNo"] = oldRow["MarkerNo"];
                this.CurrentDetailData["OrderID"] = this.CurrentMaintain["WorkType"].ToString() == "1" ? this.CurrentMaintain["ID"] : oldRow["OrderID"];
            }

            // 按+號 = -1, 其它 = 按插入, 複製原先停留row的部分欄位資訊
            if (index != -1)
            {
                // 定義不需要複製的欄位名稱列表
                HashSet<string> excludeColumns = new HashSet<string>
                {
                    "Ukey", // 此表 Pkey 底層處理
                    "tmpKey", // 上方有填不同值不複製
                    "ID", // 對應 Cutting 的 Key, 在 base.OnDetailGridInsert 會自動寫入
                    "OrderID", // 上方處理
                    "CutRef",
                    "CutNo",
                    "HasBundle",
                    "HasCuttingOutput",
                    "HasMarkerReq",
                    "Addname", // 這4欄位 base.OnDetailGridInsert 會自動寫入
                    "AddDate",
                    "Adduser",
                    "EditName",
                    "EditDate",
                    "Edituser",
                };

                foreach (DataColumn column in oldRow.Table.Columns)
                {
                    string columnName = column.ColumnName;

                    // 如果當前欄位名稱不在排除列表中，則進行複製
                    if (!excludeColumns.Contains(columnName))
                    {
                        this.CurrentDetailData[columnName] = oldRow[columnName];
                    }
                }

                // 複製第3層資訊,並對應到新的 this.CurrentDetailData
                AddThirdDatas(this.CurrentDetailData, oldRow, this.dtWorkOrderForOutput_SizeRatio, CuttingForm.P09);
                AddThirdDatas(this.CurrentDetailData, oldRow, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09);
                AddThirdDatas(this.CurrentDetailData, oldRow, this.dtWorkOrderForOutput_PatternPanel, CuttingForm.P09);
            }

            DialogResult result = this.ShowDialogActionCutRef(DialogAction.Create);
            if (result == DialogResult.Cancel)
            {
                this.OnDetailGridDelete();
            }

            this.OnDetailGridRowChanged();
        }

        private DialogResult ShowDialogActionCutRef(DialogAction action)
        {
            var form = new P09_ActionCutRef();
            form.Action = action;
            form.WorkType = this.CurrentMaintain["WorkType"].ToString();
            form.CurrentDetailData = this.CurrentDetailData;
            form.dtWorkOrderForOutput_SizeRatio_Ori = this.dtWorkOrderForOutput_SizeRatio;
            form.dtWorkOrderForOutput_Distribute_Ori = this.dtWorkOrderForOutput_Distribute;
            form.dtWorkOrderForOutput_PatternPanel_Ori = this.dtWorkOrderForOutput_PatternPanel;
            string filter = GetFilter(this.CurrentDetailData, CuttingForm.P09);
            form.dtWorkOrderForOutput_SizeRatio = this.dtWorkOrderForOutput_SizeRatio.Select(filter).TryCopyToDataTable(this.dtWorkOrderForOutput_SizeRatio);
            form.dtWorkOrderForOutput_Distribute = this.dtWorkOrderForOutput_Distribute.Select(filter).TryCopyToDataTable(this.dtWorkOrderForOutput_Distribute);
            form.dtWorkOrderForOutput_PatternPanel = this.dtWorkOrderForOutput_PatternPanel.Select(filter).TryCopyToDataTable(this.dtWorkOrderForOutput_PatternPanel);
            return form.ShowDialog();
        }

        protected override void OnDetailGridDelete()
        {
            if (this.CurrentDetailData == null)
            {
                return;
            }

            if (!this.CheckAndMsg("delete"))
            {
                return;
            }

            this.dtWorkOrderForOutput_SizeRatio.Select(GetFilter(this.CurrentDetailData, CuttingForm.P09)).Delete();
            this.dtWorkOrderForOutput_Distribute.Select(GetFilter(this.CurrentDetailData, CuttingForm.P09)).Delete();
            this.dtWorkOrderForOutput_PatternPanel.Select(GetFilter(this.CurrentDetailData, CuttingForm.P09)).Delete();

            base.OnDetailGridDelete();
        }
        #endregion

        #region Grid 欄位事件 顏色/開窗/驗證
        private void GridEventSet()
        {
            // 可否編輯 && 顏色
            this.ConfigureColumnEvents(this.detailgrid);
            this.ConfigureColumnEvents(this.gridSizeRatio);
            this.ConfigureColumnEvents(this.gridDistributeToSP);

            #region 主表

            this.col_CutRef.CellFormatting += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (this.CanEditData(dr) && MyUtility.Convert.GetString(dr["SourceFrom"]) != "1")
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Black;
                }
            };
            this.col_CutRef.EditingKeyDown += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (this.CanEditData(dr) && MyUtility.Convert.GetString(dr["SourceFrom"]) != "1")
                {
                    if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                    {
                        e.EditingControl.Text = string.Empty;
                    }
                }
            };

            this.col_OrderID.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr) || this.CurrentMaintain["WorkType"].ToString() == "1" || e.Button != MouseButtons.Right)
                {
                    return;
                }

                DataTable dt = ((DataTable)this.qtybreakds.DataSource).DefaultView.ToTable(true, "ID");
                SelectItem sele = new SelectItem(dt, "ID", "15@300,400", dr["OrderID"].ToString(), columndecimals: "50");
                if (sele.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                e.EditingControl.Text = sele.GetSelectedString();
            };
            this.col_OrderID.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = dr["OrderID"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.CanEditData(dr) || this.CurrentMaintain["WorkType"].ToString() == "1" || oldvalue == newvalue)
                {
                    return;
                }

                DataTable dt = ((DataTable)this.qtybreakds.DataSource).DefaultView.ToTable(true, "ID");
                if (dt.Select($"ID = '{newvalue}'").Length == 0)
                {
                    dr["OrderID"] = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox($"<SP> : {newvalue} data not found!");
                }
                else
                {
                    dr["OrderID"] = newvalue;
                }

                dr.EndEdit();
            };

            this.col_Seq1.EditingMouseDown += this.SeqCellEditingMouseDown;
            this.col_Seq2.EditingMouseDown += this.SeqCellEditingMouseDown;
            this.col_Seq1.CellValidating += this.SeqCelllValidatingHandler;
            this.col_Seq2.CellValidating += this.SeqCelllValidatingHandler;

            this.col_Layer.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                string oldvalue = dr["Layer"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                dr["Layer"] = newvalue;
                UpdateExcess(dr, this.dtWorkOrderForOutput_SizeRatio, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09);

                dr["Cons"] = CalculateCons(dr, this.dtWorkOrderForOutput_SizeRatio, CuttingForm.P09);
                UpdateConcatString(dr, this.dtWorkOrderForOutput_SizeRatio, CuttingForm.P09);
                dr.EndEdit();
            };
            this.col_WKETA.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    P02_WKETA item = new P02_WKETA(dr);
                    DialogResult result = item.ShowDialog();
                    switch (result)
                    {
                        case DialogResult.Cancel:
                            break;
                        case DialogResult.Yes:
                            dr["WKETA"] = Itemx.WKETA;
                            break;
                        case DialogResult.No:
                            dr["WKETA"] = DBNull.Value;
                            break;
                    }

                    dr.EndEdit();
                }
            };
            this.EstCutDate.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                DateTime? oldvalue = MyUtility.Convert.GetDate(dr["EstCutDate"]);
                DateTime? newvalue = MyUtility.Convert.GetDate(e.FormattedValue);
                if (MyUtility.Check.Empty(e.FormattedValue) || oldvalue == newvalue)
                {
                    return;
                }

                if (DateTime.Compare(DateTime.Today, Convert.ToDateTime(e.FormattedValue)) > 0)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("[Est. Cut Date] can not be passed !!");
                }
            };
            this.col_SpreadingNoID.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    SelectItem selectItem = PopupSpreadingNo(dr["SpreadingNoID"].ToString());
                    if (selectItem == null)
                    {
                        return;
                    }

                    dr["SpreadingNoID"] = selectItem.GetSelectedString();
                    dr["CutCellID"] = selectItem.GetSelecteds()[0]["CutCellID"];
                    dr.EndEdit();
                }
            };
            this.col_SpreadingNoID.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                if (!ValidatingSpreadingNo(e.FormattedValue.ToString(), out DataRow drV))
                {
                    dr["SpreadingNoID"] = string.Empty;
                    e.Cancel = true;
                }

                dr["SpreadingNoID"] = e.FormattedValue;
                dr["CutCellID"] = drV["CutCellID"];
                dr.EndEdit();
            };

            this.col_CutCellID.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    SelectItem selectItem = PopupCutCell(dr["CutCellID"].ToString());
                    if (selectItem == null)
                    {
                        return;
                    }

                    dr["CutCellID"] = selectItem.GetSelectedString();
                    dr.EndEdit();
                }
            };
            this.col_CutCellID.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                if (!ValidatingCutCell(e.FormattedValue.ToString()))
                {
                    dr["CutCellID"] = string.Empty;
                    e.Cancel = true;
                }

                dr["CutCellID"] = e.FormattedValue;
                dr.EndEdit();
            };

            this.col_MarkerLength.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                dr["MarkerLength"] = dr["MarkerLength_Mask"] = SetMarkerLengthMaskString(e.FormattedValue.ToString());
                dr["ConsPC"] = CalculateConsPC(dr["MarkerLength"].ToString(), dr, this.dtWorkOrderForOutput_SizeRatio, CuttingForm.P09);
                dr["Cons"] = CalculateCons(dr, this.dtWorkOrderForOutput_SizeRatio, CuttingForm.P09);
                dr.EndEdit();
            };
            this.col_ActCuttingPerimeter.CellValidating += this.MaskedCellValidatingHandler;
            this.col_StraightLength.CellValidating += this.MaskedCellValidatingHandler;
            this.col_CurvedLength.CellValidating += this.MaskedCellValidatingHandler;

            this.col_MarkerNo.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr) || e.Button != MouseButtons.Right)
                {
                    return;
                }

                SelectItem selectItem = PopupMarkerNo(this.CurrentMaintain["ID"].ToString(), dr["MarkerNo"].ToString());
                if (selectItem == null)
                {
                    return;
                }

                dr["MarkerNo"] = selectItem.GetSelectedString();
                dr.EndEdit();
            };
            this.col_MarkerNo.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                if (!ValidatingMarkerNo(this.CurrentMaintain["ID"].ToString(), e.FormattedValue.ToString()))
                {
                    dr["MarkerNo"] = string.Empty;
                    e.Cancel = true;
                }

                dr["MarkerNo"] = e.FormattedValue;
                dr.EndEdit();
            };

            #endregion

            #region SizeRatio
            this.col_SizeRatio_Size.EditingMouseDown += (s, e) =>
            {
                if (SizeCodeCellEditingMouseDown(e, this.gridSizeRatio, this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09))
                {
                    UpdateConcatString(this.CurrentDetailData, this.dtWorkOrderForOutput_SizeRatio, CuttingForm.P09);
                }
            };
            this.col_SizeRatio_Size.CellValidating += (s, e) =>
            {
                if (SizeCodeCellValidating(e, this.gridSizeRatio, this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09))
                {
                    UpdateConcatString(this.CurrentDetailData, this.dtWorkOrderForOutput_SizeRatio, CuttingForm.P09);
                    UpdateTotalDistributeQty(this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09);
                }
            };
            this.BindQtyEvents(this.col_SizeRatio_Qty);
            #endregion

            #region Distribute
            this.BindDistributeEvents(this.col_Distribute_SP);
            this.BindDistributeEvents(this.col_Distribute_Article);
            this.BindDistributeEvents(this.col_Distribute_Size);
            this.BindQtyEvents(this.col_Distribute_Qty);
            #endregion
        }

        private void ConfigureColumnEvents(Sci.Win.UI.Grid grid)
        {
            foreach (var column in grid.Columns)
            {
                // 欄位沒設定 IsEditingReadOnly 才作變更
                if (column is DataGridViewTextBoxBaseColumn textBoxBase && !textBoxBase.IsEditingReadOnly)
                {
                    textBoxBase.EditingControlShowing += this.CustomEditingControlShowing;
                    textBoxBase.CellFormatting += this.CustomCellFormatting;
                }
                else if (column is DataGridViewTextBoxBase2Column textBoxBase2 && !textBoxBase2.IsEditingReadOnly)
                {
                    textBoxBase2.EditingControlShowing += this.CustomEditingControlShowing;
                    textBoxBase2.CellFormatting += this.CustomCellFormatting;
                }
                else if (column is DataGridViewMaskedTextBoxBaseColumn maskedtextBoxBase && !maskedtextBoxBase.IsEditingReadOnly)
                {
                    maskedtextBoxBase.EditingControlShowing += this.CustomEditingControlShowing;
                    maskedtextBoxBase.CellFormatting += this.CustomCellFormatting;
                }
            }
        }

        private void CustomEditingControlShowing(object sender, Ict.Win.UI.DataGridViewEditingControlShowingEventArgs e)
        {
            // 可否編輯
            Sci.Win.UI.Grid grid = (Sci.Win.UI.Grid)((DataGridViewColumn)sender).DataGridView;
            DataRow dr = grid.GetDataRow(e.RowIndex);
            bool canEdit = this.CanEditDatByGrid(grid, dr, grid.Columns[e.ColumnIndex].Name);
            if (e.Control is Ict.Win.UI.TextBoxBase textBoxBase)
            {
                textBoxBase.ReadOnly = !canEdit;
            }
            else if (e.Control is Ict.Win.UI.TextBoxBase2 textBoxBase2)
            {
                textBoxBase2.ReadOnly = !canEdit;
            }
        }

        private void CustomCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // 粉底紅字
            Sci.Win.UI.Grid grid = (Sci.Win.UI.Grid)((DataGridViewColumn)sender).DataGridView;
            DataRow dr = grid.GetDataRow(e.RowIndex);
            bool canEdit = this.CanEditDatByGrid(grid, dr, grid.Columns[e.ColumnIndex].Name);
            if (canEdit)
            {
                e.CellStyle.BackColor = Color.Pink;
                e.CellStyle.ForeColor = Color.Red;
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
            }
        }

        private bool CanEditDatByGrid(Sci.Win.UI.Grid grid, DataRow dr, string columNname)
        {
            if (grid.Name == "detailgrid")
            {
                if (columNname.ToLower() == "orderid" && this.CurrentMaintain["WorkType"].ToString() == "1")
                {
                    return false;
                }

                return this.CanEditData(dr);
            }
            else
            {
                if (grid.Name == "gridDistributeToSP" && dr["OrderID"].ToString().Equals("EXCESS", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                return this.CanEditData(this.CurrentDetailData);
            }
        }

        private void SeqCellEditingMouseDown(object sender, Ict.Win.UI.DataGridViewEditingControlMouseEventArgs e)
        {
            DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
            if (!this.CanEditData(dr) || e.Button != MouseButtons.Right)
            {
                return;
            }

            DataRow minFabricPanelCode = GetMinFabricPanelCode(dr, this.dtWorkOrderForOutput_PatternPanel, CuttingForm.P09);
            if (minFabricPanelCode == null)
            {
                MyUtility.Msg.WarningBox("Please select Pattern Panel first!");
                return;
            }

            string columnName = this.detailgrid.Columns[e.ColumnIndex].Name;
            string id = this.CurrentDetailData["ID"].ToString();
            string fabricCode = this.CurrentDetailData["FabricCode"].ToString();
            string seq1 = this.CurrentDetailData["SEQ1"].ToString();
            string seq2 = this.CurrentDetailData["SEQ2"].ToString();
            string refno = this.CurrentDetailData["Refno"].ToString();
            string colorID = this.CurrentDetailData["ColorID"].ToString();

            // 觸發的欄位不作為篩選條件
            switch (columnName.ToLower())
            {
                case "seq1":
                    seq1 = string.Empty;
                    break;
                case "seq2":
                    seq2 = string.Empty;
                    break;
            }

            SelectItem selectItem = PopupSEQ(id, fabricCode, seq1, seq2, refno, colorID, false);
            if (selectItem == null)
            {
                return;
            }

            dr["SEQ1"] = selectItem.GetSelecteds()[0]["SEQ1"];
            dr["SEQ2"] = selectItem.GetSelecteds()[0]["SEQ2"];
            dr["Refno"] = selectItem.GetSelecteds()[0]["Refno"];
            dr["ColorID"] = selectItem.GetSelecteds()[0]["ColorID"];
            dr["SCIRefno"] = selectItem.GetSelecteds()[0]["SCIRefno"];
            dr.EndEdit();
        }

        private void SeqCelllValidatingHandler(object sender, Ict.Win.UI.DataGridViewCellValidatingEventArgs e)
        {
            DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
            string columnName = this.detailgrid.Columns[e.ColumnIndex].Name;
            string newvalue = e.FormattedValue.ToString();
            string oldvalue = this.CurrentDetailData[columnName].ToString();
            if (!this.CanEditData(dr) || MyUtility.Check.Empty(newvalue) || newvalue == oldvalue)
            {
                return;
            }

            DataRow minFabricPanelCode = GetMinFabricPanelCode(dr, this.dtWorkOrderForOutput_PatternPanel, CuttingForm.P09);
            if (minFabricPanelCode == null)
            {
                MyUtility.Msg.WarningBox("Please select Pattern Panel first!");
                return;
            }

            string id = this.CurrentDetailData["ID"].ToString();
            string fabricCode = this.CurrentDetailData["FabricCode"].ToString();
            string seq1 = this.CurrentDetailData["SEQ1"].ToString();
            string seq2 = this.CurrentDetailData["SEQ2"].ToString();
            string refno = this.CurrentDetailData["Refno"].ToString();
            string colorID = this.CurrentDetailData["ColorID"].ToString();
            switch (columnName.ToLower())
            {
                case "seq1":
                    seq1 = newvalue;
                    break;
                case "seq2":
                    seq2 = newvalue;
                    break;
            }

            if (ValidatingSEQ(id, fabricCode, seq1, seq2, refno, colorID, out DataTable dtValidating))
            {
                this.CurrentDetailData[columnName] = newvalue;

                // 唯一值時
                if (dtValidating.Rows.Count == 1)
                {
                    dr["SCIRefno"] = dtValidating.Rows[0]["SCIRefno"].ToString();
                }
            }
            else
            {
                this.CurrentDetailData[columnName] = string.Empty;
            }

            this.CurrentDetailData.EndEdit();
        }

        private void MaskedCellValidatingHandler(object sender, Ict.Win.UI.DataGridViewCellValidatingEventArgs e)
        {
            DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
            if (!this.CanEditData(dr))
            {
                return;
            }

            string columnName = (sender as DataGridViewColumn)?.Name;
            if (MyUtility.Check.Empty(e.FormattedValue))
            {
                dr[columnName] = dr[columnName + "_Mask"] = string.Empty;
                return;
            }

            dr[columnName] = dr[columnName + "_Mask"] = SetMaskString(e.FormattedValue.ToString());
            dr.EndEdit();
        }

        private void BindQtyEvents(Ict.Win.UI.DataGridViewNumericBoxColumn column)
        {
            column.CellValidating += (s, e) =>
            {
                Sci.Win.UI.Grid grid = (Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                if (QtyCellValidating(e, this.CurrentDetailData, grid, this.dtWorkOrderForOutput_SizeRatio, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09))
                {
                    UpdateConcatString(this.CurrentDetailData, this.dtWorkOrderForOutput_SizeRatio, CuttingForm.P09);
                    UpdateTotalDistributeQty(this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09);
                    if (grid.Name == "gridSizeRatio")
                    {
                        DataRow dr = grid.GetDataRow(e.RowIndex);
                        this.CurrentDetailData["Cons"] = CalculateCons(dr, this.dtWorkOrderForOutput_SizeRatio, CuttingForm.P09);
                    }
                }
            };
        }

        private void BindDistributeEvents(Ict.Win.UI.DataGridViewTextBoxColumn column)
        {
            column.EditingMouseDown += (s, e) =>
            {
                if (Distribute3CellEditingMouseDown(e, this.CurrentDetailData, this.dtWorkOrderForOutput_SizeRatio, this.gridDistributeToSP))
                {
                    UpdateMinSewinline(this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09);
                    if (((DataGridViewElement)s).DataGridView.Columns[e.ColumnIndex].Name.ToLower() == "orderid")
                    {
                        UpdateMinOrderID(this.CurrentMaintain["WorkType"].ToString(), this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09);
                    }
                }
            };
            column.CellValidating += (s, e) =>
            {
                if (Distribute3CellValidating(e, this.CurrentDetailData, this.dtWorkOrderForOutput_SizeRatio, this.gridDistributeToSP, CuttingForm.P09))
                {
                    UpdateTotalDistributeQty(this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09);
                    UpdateMinSewinline(this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09);
                    if (((DataGridViewElement)s).DataGridView.Columns[e.ColumnIndex].Name.ToLower() == "orderid")
                    {
                        UpdateMinOrderID(this.CurrentMaintain["WorkType"].ToString(), this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09);
                    }
                }
            };
        }

        #endregion

        #region Grid 右鍵 Menu
        private void MenuItemInsertSizeRatio_Click(object sender, EventArgs e)
        {
            if (!this.CanEditData(this.CurrentDetailData))
            {
                return;
            }

            DataRow newrow = this.dtWorkOrderForOutput_SizeRatio.NewRow();
            newrow["ID"] = this.CurrentDetailData["ID"];
            newrow["WorkOrderForOutputUkey"] = this.CurrentDetailData["Ukey"];
            newrow["tmpKey"] = this.CurrentDetailData["tmpKey"];
            newrow["Qty"] = 0;
            this.dtWorkOrderForOutput_SizeRatio.Rows.Add(newrow);
        }

        private void MenuItemDeleteSizeRatio_Click(object sender, EventArgs e)
        {
            var selectRow = this.gridSizeRatio.GetSelecteds(SelectedSort.Index);
            if (!this.CanEditData(this.CurrentDetailData) || selectRow.Count == 0)
            {
                return;
            }

            DataRow dr = ((DataRowView)selectRow[0]).Row;

            // 先刪除 Distribute
            string sizeCode = MyUtility.Convert.GetString(dr["SizeCode"]);
            string filter = GetFilter(this.CurrentDetailData, CuttingForm.P09);
            this.dtWorkOrderForOutput_Distribute.Select(filter + $" AND SizeCode = '{sizeCode}'").Delete();

            // 後刪除 SizeRatio
            dr.Delete();
        }

        private void MenuItemInsertDistribute_Click(object sender, EventArgs e)
        {
            if (this.dtWorkOrderForOutput_SizeRatio.DefaultView.ToTable().Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Please insert size ratio data first!");
                return;
            }

            DataRow newrow = this.dtWorkOrderForOutput_Distribute.NewRow();
            newrow["ID"] = this.CurrentDetailData["ID"];
            newrow["WorkOrderForOutputUkey"] = this.CurrentDetailData["Ukey"];
            newrow["tmpKey"] = this.CurrentDetailData["tmpKey"];
            newrow["Qty"] = 0;
            this.dtWorkOrderForOutput_Distribute.Rows.Add(newrow);
        }

        private void MenuItemDeleteDistribute_Click(object sender, EventArgs e)
        {
            var selectRow = this.gridDistributeToSP.GetSelecteds(SelectedSort.Index);
            if (!this.CanEditData(this.CurrentDetailData) || selectRow.Count == 0)
            {
                return;
            }

            // Excess 不可刪除
            DataRow dr = ((DataRowView)selectRow[0]).Row;
            if (dr["OrderID"].ToString().Equals("EXCESS", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            dr.Delete();
        }
        #endregion

        #region 4 項檢查 無訊息 & 有訊息
        private bool CanEditData(DataRow dr)
        {
            return this.EditMode && this.CheckContinue(dr);
        }

        private bool CheckContinue(DataRow dr)
        {
            if (dr == null)
            {
                return false;
            }

            // 此4個欄位是和表身一起撈取 (若及時去DB判斷會卡到爆炸)
            return !(
                 !MyUtility.Convert.GetString(dr["SpreadingStatus"]).Equals("Ready", StringComparison.OrdinalIgnoreCase) ||
                  MyUtility.Convert.GetBool(dr["HasBundle"]) ||
                  MyUtility.Convert.GetBool(dr["HasCuttingOutput"]) ||
                  MyUtility.Convert.GetBool(dr["HasMarkerReq"]));
        }

        private bool CheckAndMsg(string actrion)
        {
            if (MyUtility.Check.Empty(this.CurrentDetailData["CutRef"]))
            {
                return true;
            }

            // 前 3 個都是及時去 DB 撈資料判斷, 並彈出訊息
            // 1. 存在 P10 Bundle
            string msg = $"The following bundle data exists and cannot be {actrion}. If you need to {actrion}, please go to [Cutting_P10. Bundle Card] to delete the bundle data.";
            if (!CheckBundleAndShowData(this.CurrentDetailData["CutRef"].ToString(), msg))
            {
                return false;
            }

            // 2. 存在 P20 CuttingOutput
            msg = $"The following cutting output data exists and cannot be {actrion}. If you need to delete, please go to [Cutting_P20. Cutting Daily Output] to delete the cutting output data.";
            if (!CheckCuttingOutputCuttingOutputAndShowData(this.CurrentDetailData["CutRef"].ToString(), msg))
            {
                return false;
            }

            // 3. 存在 P05 MarkerReq_Detail
            msg = $"The following marker request data exists and cannot be {actrion}. If you need to delete, please go to [Cutting_P05. Bulk Marker Request] to delete the marker request data.";
            if (!CheckMarkerReqAndShowData(this.CurrentDetailData["CutRef"].ToString(), msg))
            {
                return false;
            }

            // 4 檢查欄位 SpreadingStatus
            if (!CheckSpreadingStatus(this.CurrentDetailData, $"The following digitail spreading data exists and cannot be {actrion}"))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Other

        private void GridValidateControl()
        {
            this.detailgrid.ValidateControl();
            this.gridSizeRatio.ValidateControl();
            this.gridDistributeToSP.ValidateControl();
        }
        #endregion

        protected override void DoPrint()
        {
            base.DoPrint();
        }

        protected override bool ClickPrint()
        {
            return base.ClickPrint();
        }

        private void BtnExcludeSetting_Click(object sender, EventArgs e)
        {

        }

        private void BtnBatchAssign_Click(object sender, EventArgs e)
        {

        }

        private void BtnImportMarker_Click(object sender, EventArgs e)
        {

        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {

        }

        private void BtnImportMarkerLectra_Click(object sender, EventArgs e)
        {

        }

        private void BtnAutoRef_Click(object sender, EventArgs e)
        {

        }

        private void BtnAutoCut_Click(object sender, EventArgs e)
        {

        }

        private void BtnAllSPDistribute_Click(object sender, EventArgs e)
        {

        }

        private void BtnDistributeThisCutRef_Click(object sender, EventArgs e)
        {

        }

        private void BtnCutPartsCheck_Click(object sender, EventArgs e)
        {

        }

        private void BtnCutPartsCheckSummary_Click(object sender, EventArgs e)
        {

        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {

        }

        private void BtnQtyBreakdown_Click(object sender, EventArgs e)
        {

        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {

        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {

        }
    }
#pragma warning restore SA1600 // Elements should be documented
}
