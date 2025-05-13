using Ict;
using Ict.Win;
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
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
#pragma warning disable SA1600 // Elements should be documented
    /// <inheritdoc/>
    public partial class P09 : Win.Tems.Input6
    {
        #region 全域變數
        private readonly CuttingForm formType = CuttingForm.P09;
        private readonly Win.UI.BindingSource2 bindingSourceDetail = new Win.UI.BindingSource2(); // 右上使用,綁主表欄位
        private Ict.Win.UI.DataGridViewTextBoxColumn col_CutRef;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq1;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq2;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Layer;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_SpreadingNoID;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_CutCellID;
        private Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_MarkerLength;
        private Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_ActCuttingPerimeter;
        private Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_StraightLength;
        private Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_CurvedLength;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_SizeRatio_Size;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_SizeRatio_Qty;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Distribute_SP;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Distribute_Article;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Distribute_Size;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Distribute_Qty;
        private DataTable dt_SizeRatio; // 第3層表:新刪修
        private DataTable dt_Distribute; // 第3層表:新刪修
        private DataTable dt_PatternPanel; // 第3層表:新刪修
        private DataTable dtDeleteUkey_HasGroup; // 如果WorkOrderForOutput.GroupID != '' 刪除後存檔時要寫入WorkOrderForOutputDelete;
        private DataTable[] dtsHistory; // 要判斷按鈕顏色, 進表身就先撈, 還要用在傳入 History
        private bool detaildatasHasChange = false;
        private bool ReUpdateP20 = true;
        private DataRow drBeforeDoPrintDetailData;  // 紀錄目前表身選擇的資料，因為base.DoPrint() 時會LOAD資料,並將this.CurrentDetailData移動到第一筆
        private string detailSort = "SORT_NUM, PatternPanel_CONCAT, multisize DESC, Article_CONCAT, Order_SizeCode_Seq DESC, MarkerName, Ukey";
        private DataTable dt_Layers;
        private bool editByUseCutRefToRequestFabric;
        #endregion

        #region 程式開啟時, 只會執行一次

        /// <inheritdoc/>
        public P09(ToolStripMenuItem menuitem, string history)
            : base(menuitem)
        {
            this.InitializeComponent();

            if (history == "0")
            {
                this.Text = "P09.WorkOrder For Output";
                this.DefaultFilter = $"MDivisionid = '{Sci.Env.User.Keyword}' AND WorkType <> '' AND Finished = 0";
            }
            else
            {
                this.Text = "[P091. WorkOrder For Output(History)]";
                this.IsSupportEdit = false;
                this.DefaultFilter = $"MDivisionid = '{Sci.Env.User.Keyword}' AND WorkType <> '' AND Finished = 1";
                this.btnImportFromWorkOrderForPlanning.EditMode = AdvEditModes.None;
                this.btnImportFromWorkOrderForPlanning.Enabled = false;
                this.btnAutoRef.EditMode = AdvEditModes.None;
                this.btnAutoRef.Enabled = false;
                this.gridSizeRatio.ContextMenuStrip = null;
                this.gridDistributeToSP.ContextMenuStrip = null;
            }

            this.displayBoxFabricTypeRefno.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "FabricTypeRefNo", true));
            this.displayBoxDescription.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "FabricDescription", true));
            this.numConsPC.DataBindings.Add(new Binding("Value", this.bindingSourceDetail, "ConsPC", true));
            this.numCons.DataBindings.Add(new Binding("Value", this.bindingSourceDetail, "Cons", true));
            this.displayBoxTotalCutQty.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "TotalCutQty_CONCAT", true));
            this.displayBoxTtlDistributeQty.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "TotalDistributeQty", true));
            this.txtPatternNo.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "MarkerNo", true));
            this.txtMarkerLength.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "MarkerLength", true));

            this.detailgrid.Click += Grid_ClickBeginEdit;
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
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("CutRef", header: "CutRef#", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true).Get(out this.col_CutRef)
                .NumericNull("CutNo", "CutNo", Ict.Win.Widths.AnsiChars(5), this.CanEditData)
                .Text("MarkerName", header: "Marker\r\nName", width: Ict.Win.Widths.AnsiChars(5))
                .MarkerLength("MarkerLength_Mask", "Marker Length", "MarkerLength", Ict.Win.Widths.AnsiChars(10), this.CanEditNotWithUseCutRefToRequestFabric).Get(out this.col_MarkerLength)
                .Text("PatternPanel_CONCAT", header: "Pattern\r\nPanel", width: Ict.Win.Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("FabricPanelCode_CONCAT", header: "Fabric\r\nPanel Code", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Article_CONCAT", header: "Article", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ColorId", header: "Color", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Tone", header: "Tone", width: Ict.Win.Widths.AnsiChars(4))
                .Text("SizeCode_CONCAT", header: "Size", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Layer", header: "Layers", width: Ict.Win.Widths.AnsiChars(5), integer_places: 5, maximum: 99999).Get(out this.col_Layer)
                .Text("TotalCutQty_CONCAT", header: "Total CutQty", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .WorkOrderSP("OrderId", "SP#", Ict.Win.Widths.AnsiChars(13), this.GetWorkType, this.CanEditData)
                .Text("SEQ1", header: "Seq1", width: Ict.Win.Widths.AnsiChars(3)).Get(out this.col_Seq1)
                .Text("SEQ2", header: "Seq2", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_Seq2)
                .Date("Fabeta", header: "Fabric Arr Date", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .WorkOrderWKETA("WKETA", "WK ETA", Ict.Win.Widths.AnsiChars(10), true, this.CanEditData)
                .EstCutDate("EstCutDate", "Est. Cut Date", Ict.Win.Widths.AnsiChars(10), this.CanEditNotWithUseCutRefToRequestFabric)
                .Date("Sewinline", header: "Sewing inline", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SpreadingNoID", header: "Spreading No", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_SpreadingNoID)
                .Text("CutCellID", header: "Cut Cell", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_CutCellID)
                .Text("Shift", header: "Shift", width: Ict.Win.Widths.AnsiChars(2), settings: CellTextDropDownList.GetGridCell("Pms_WorkOrderShift"))
                .Date("Actcutdate", header: "Act. Cut Date", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SpreadingStatus", header: "Spreading\r\nStatus", width: Ict.Win.Widths.AnsiChars(8), iseditingreadonly: true)
                .MaskedText("ActCuttingPerimeter_Mask", "000Yd00\"00", "ActCutting\r\nPerimeter", name: "ActCuttingPerimeter", width: Ict.Win.Widths.AnsiChars(10)).Get(out this.col_ActCuttingPerimeter)
                .MaskedText("StraightLength_Mask", "000Yd00\"00", "StraightLength", name: "StraightLength", width: Ict.Win.Widths.AnsiChars(10)).Get(out this.col_StraightLength)
                .MaskedText("CurvedLength_Mask", "000Yd00\"00", "CurvedLength", name: "CurvedLength", width: Ict.Win.Widths.AnsiChars(10)).Get(out this.col_CurvedLength)
                .MarkerNo("MarkerNo", "Pattern No.", Ict.Win.Widths.AnsiChars(11), this.CanEditData)
                .Text("CuttingPlannerRemark", header: "Cutting Planner Remark", width: Ict.Win.Widths.AnsiChars(33), iseditingreadonly: false)
                .Text("Adduser", header: "Add Name", width: Ict.Win.Widths.AnsiChars(12), iseditingreadonly: true)
                .DateTime("AddDate", header: "Add Date", width: Ict.Win.Widths.AnsiChars(19), iseditingreadonly: true)
                .Text("Edituser", header: "Edit Name", width: Ict.Win.Widths.AnsiChars(12), iseditingreadonly: true)
                .DateTime("EditDate", header: "Edit Date", width: Ict.Win.Widths.AnsiChars(19), iseditingreadonly: true)
                ;
            this.Helper.Controls.Grid.Generator(this.gridSizeRatio)
                .Text("SizeCode", header: "Size", width: Ict.Win.Widths.AnsiChars(3)).Get(out this.col_SizeRatio_Size)
                .Numeric("Qty", header: "Ratio", width: Ict.Win.Widths.AnsiChars(6), integer_places: 5, maximum: 99999, minimum: 0).Get(out this.col_SizeRatio_Qty)
                ;
            this.Helper.Controls.Grid.Generator(this.gridDistributeToSP)
                .Text("OrderID", header: "SP#", width: Ict.Win.Widths.AnsiChars(13)).Get(out this.col_Distribute_SP)
                .Text("Article", header: "Article", width: Ict.Win.Widths.AnsiChars(6)).Get(out this.col_Distribute_Article)
                .Text("SizeCode", header: "Size", width: Ict.Win.Widths.AnsiChars(4)).Get(out this.col_Distribute_Size)
                .Numeric("Qty", header: "Qty", width: Ict.Win.Widths.AnsiChars(3), integer_places: 6, maximum: 999999, minimum: 0).Get(out this.col_Distribute_Qty)
                .Date("SewInline", header: "Inline Date", width: Ict.Win.Widths.AnsiChars(8), iseditingreadonly: true)
                ;
            this.Helper.Controls.Grid.Generator(this.gridSpreadingFabric)
                .Text("Seq1", header: "Seq1", width: Ict.Win.Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Seq2", header: "Seq2", width: Ict.Win.Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Roll", header: "Roll", width: Ict.Win.Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Ict.Win.Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("SpreadingLayers", header: "Layers", width: Ict.Win.Widths.AnsiChars(3), iseditingreadonly: true)
                ;
            this.Helper.Controls.Grid.Generator(this.gridQtyBreakDown)
                .Text("ID", header: "SP#", width: Ict.Win.Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Ict.Win.Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Ict.Win.Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("Qty", header: "Order\nQty", width: Ict.Win.Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("Balance", header: "Balance", width: Ict.Win.Widths.AnsiChars(5), iseditingreadonly: true);
            this.GridEventSet();

            // 設定所有欄位的 AutoSizeMode
            foreach (DataGridViewColumn column in this.detailgrid.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }
        #endregion

        #region 進入 Detail 撈資料, By 主表 Pkey

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
   ,MarkerLength_Mask = ''-- 4個_Mask欄位在下方 OnDetailEntered 會把實體欄位資訊格式化後填入
   ,ActCuttingPerimeter_Mask = ''
   ,StraightLength_Mask = ''
   ,CurvedLength_Mask = ''
   ,AddUser = p1.Name
   ,EditUser = p2.Name
   ,FabricTypeRefNo = CONCAT(f.WeaveTypeID, ' /' + wo.RefNo)
   ,FabricDescription = f.Description
   ,TotalDistributeQty = (SELECT SUM(Qty) FROM WorkOrderForOutput_Distribute WITH (NOLOCK) WHERE WorkOrderForOutputUkey = wo.Ukey)

   --沒有顯示的欄位
   ,tmpKey = CAST(0 AS BIGINT)--控制新加的資料用,SizeRatio/Distribute/PatternPanel
   ,CanEdit = dbo.GetCuttingP09CanEdit(wo.CutRef) -- 判斷此筆是否能編輯
   ,ImportML = CAST(0 AS BIT)
   ,ImportWP = CAST(0 AS BIT)
   ,CanDoAutoDistribute = CAST(0 AS BIT)
   --- 排序用
   ,SORT_NUM = 0 -- 避免編輯過程資料跑來跑去
   ,multisize.multisize
   ,Order_SizeCode_Seq.Order_SizeCode_Seq

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
        SELECT CONCAT(', ', ws.SizeCode, '/ ', ws.Qty)
        FROM WorkOrderForOutput_SizeRatio ws WITH (NOLOCK)
		OUTER APPLY(SELECT TOP 1 osc.SizeGroup,osc.Seq FROM Order_SizeCode osc WITH (NOLOCK) WHERE osc.Id = ws.ID AND osc.SizeCode = ws.SizeCode) osc
        WHERE ws.WorkOrderForOutputUkey = wo.Ukey
		ORDER BY ws.WorkOrderForOutputUkey,osc.SizeGroup,osc.Seq,ws.SizeCode
        FOR XML PATH ('')), 1, 1, '')
) ws1
OUTER APPLY (
    SELECT TotalCutQty_CONCAT = STUFF((
        SELECT CONCAT(', ', ws.Sizecode, '/ ', ws.Qty * wo.Layer)
        FROM WorkOrderForOutput_SizeRatio ws WITH (NOLOCK)
		OUTER APPLY(SELECT TOP 1 osc.SizeGroup,osc.Seq FROM Order_SizeCode osc WITH (NOLOCK) WHERE osc.Id = ws.ID AND osc.SizeCode = ws.SizeCode) osc
        WHERE ws.WorkOrderForOutputUkey = wo.Ukey
		ORDER BY ws.WorkOrderForOutputUkey,osc.SizeGroup,osc.Seq,ws.SizeCode
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
OUTER APPLY (
	SELECT multisize = IIF(COUNT(SizeCode) > 1, 2, 1) 
    FROM WorkOrderForOutput_SizeRatio WITH (NOLOCK)
	Where wo.Ukey = WorkOrderForOutputUkey
) as multisize
OUTER APPLY (
	SELECT Order_SizeCode_Seq = max(osc.Seq)
    FROM WorkOrderForOutput_SizeRatio ws WITH (NOLOCK)
	LEFT JOIN Order_SizeCode osc WITH (NOLOCK) ON osc.ID = ws.ID and osc.SizeCode = ws.SizeCode
	WHERE ws.WorkOrderForOutputUkey = wo.Ukey
) as Order_SizeCode_Seq
WHERE wo.id = '{masterID}'
ORDER BY {this.detailSort}
";

            string cmdsql = $@"
Select a.MarkerName,a.ColorID,a.Order_EachconsUkey
	,layer = isnull(sum(a.layer),0)
    ,TotalLayer_byOrder_EachCons =             
    (
        Select isnull(sum(c.layer),0) as TL
	    from Order_EachCons b WITH (NOLOCK) 
		inner join Order_EachCons_Color c WITH (NOLOCK) on c.ID = b.ID and c.Order_EachConsUkey = b.ukey
	    where b.ID = a.ID and b.Markername = a.MarkerName and c.ColorID = a.ColorID and b.Ukey = a.Order_EachconsUkey 
    )
    ,TotalLayerMarker =
    (
        Select isnull(sum(c.layer),0) as TL2
	    from Order_EachCons b WITH (NOLOCK) 
		inner join Order_EachCons_Color c WITH (NOLOCK) on b.ID = c.ID and c.Order_EachConsUkey = b.ukey
	    where b.ID = a.ID and b.Markername = a.MarkerName and c.ColorID = a.ColorID
    )
From WorkOrderForPlanning a WITH (NOLOCK) 
Where a.ID = '{masterID}' 
group by a.MarkerName,a.ColorID,a.Order_EachconsUkey,a.ID 
Order by a.MarkerName,a.ColorID,a.Order_EachconsUkey
";
            DualResult result = DBProxy.Current.Select(null, cmdsql, out this.dt_Layers);
            if (!result)
            {
                this.ShowErr(result);
            }

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            int useCutRefToRequestFabric = MyUtility.Convert.GetInt(this.CurrentMaintain["UseCutRefToRequestFabric"]);
            this.editByUseCutRefToRequestFabric = useCutRefToRequestFabric != 1;

            this.btnImportMarker.Enabled = !this.EditMode && this.IsSupportEdit && this.editByUseCutRefToRequestFabric;
            this.btnImportMarkerLectra.Enabled = this.EditMode && this.editByUseCutRefToRequestFabric;
            this.btnAutoRef.Enabled = !this.EditMode && this.editByUseCutRefToRequestFabric;
            this.btnAutoCut.Enabled = this.EditMode && this.editByUseCutRefToRequestFabric;
            this.btnAllSPDistribute.Enabled = this.EditMode && this.editByUseCutRefToRequestFabric;
            this.btnDistributeThisCutRef.Enabled = this.EditMode && this.editByUseCutRefToRequestFabric;
            this.displayBoxStyle.Text = MyUtility.GetValue.Lookup($"SELECT StyleID FROM Orders WITH(NOLOCK) WHERE ID = '{this.CurrentMaintain["ID"]}'");
            this.displayLastCreateCutRef.Text = this.DetailDatas.AsEnumerable().OrderByDescending(row => MyUtility.Convert.GetDate(row["LastCreateCutRefDate"]))
                .ThenByDescending(row => MyUtility.Convert.GetString(row["CutRef"])).Select(row => MyUtility.Convert.GetString(row["CutRef"])).FirstOrDefault();
            this.DetailDatas.AsEnumerable().ToList().ForEach(row => Format4LengthColumn(row)); // 4 個_Mask 欄位 用來顯示用, 若有編輯會寫回原欄位
            this.GetAllDetailData();
            this.Sorting();
            this.dtDeleteUkey_HasGroup = ((DataTable)this.detailgridbs.DataSource).Clone();
            this.gridSpreadingFabric.AutoResizeColumns();
            ((DataTable)this.detailgridbs.DataSource).AcceptChanges();
        }

        private void GetAllDetailData()
        {
            string cuttingID = MyUtility.Check.Empty(this.CurrentMaintain) ? string.Empty : MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);

            string sqlcmd = $@"
SELECT *, tmpKey = CAST(0 AS BIGINT) FROM WorkOrderForOutput_PatternPanel WITH (NOLOCK) WHERE ID = '{cuttingID}'

SELECT ws.*, tmpKey = CAST(0 AS BIGINT) FROM WorkOrderForOutput_SizeRatio ws WITH (NOLOCK)
OUTER APPLY(SELECT TOP 1 osc.SizeGroup,osc.Seq FROM Order_SizeCode osc WITH (NOLOCK) WHERE osc.Id = ws.ID AND osc.SizeCode = ws.SizeCode) osc
WHERE ws.ID = '{cuttingID}'
ORDER BY osc.SizeGroup,osc.Seq

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
WHERE wd.ID = '{cuttingID}'
ORDER BY wd.OrderID, wd.Article, wd.SizeCode

SELECT *, tmpKey = CAST(0 AS BIGINT)
FROM WorkOrderForOutput_SpreadingFabric sf WITH (NOLOCK)
WHERE EXISTS(SELECT 1 FROM WorkOrderForOutput WHERE CutRef = sf.CutRef AND ID = '{cuttingID}')
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable[] dts);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.dt_PatternPanel = dts[0];
            this.dt_SizeRatio = dts[1];
            this.dt_Distribute = dts[2];

            this.sizeRatiobs.DataSource = this.dt_SizeRatio;
            this.distributebs.DataSource = this.dt_Distribute;
            this.spreadingfabricbs.DataSource = dts[3];

            // 右下角 Qty Break Down
            this.qtybreakds.DataSource = QueryQtyBreakDown(cuttingID, this.formType);

            // Histyoy 資訊
            sqlcmd = $@"
SELECT CutRef, Layer, GroupID FROM WorkOrderForOutputHistory WITH (NOLOCK) WHERE ID = '{this.CurrentMaintain["ID"]}' AND GroupID <> '' ORDER BY GroupID, CutRef
SELECT CutRef, Layer, GroupID FROM WorkOrderForOutput WITH (NOLOCK) WHERE ID = '{this.CurrentMaintain["ID"]}' AND GroupID <> '' ORDER BY GroupID, CutRef
SELECT CutRef, Layer, GroupID FROM WorkOrderForOutputDelete WITH (NOLOCK) WHERE ID = '{this.CurrentMaintain["ID"]}' AND GroupID <> '' ORDER BY GroupID, CutRef
";
            result = DBProxy.Current.Select(null, sqlcmd, out this.dtsHistory);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.ChangeQtyBreakDownRow();
            this.ChangeBtnColor();
        }

        private void ChangeBtnColor()
        {
            this.btnQtyBreakdown.ForeColor = this.qtybreakds.Count > 0 ? Color.Blue : Color.Black;

            bool hasHistory = this.dtsHistory[0].AsEnumerable().Any() || this.dtsHistory[1].AsEnumerable().Any() || this.dtsHistory[2].AsEnumerable().Any();
            this.btnHistory.ForeColor = hasHistory ? Color.Blue : Color.Black;
            this.btnPackingMethod.ForeColor = MyUtility.Check.Seek(this.CurrentMaintain["ID"].ToString(), "orders", "cuttingsp") ? Color.Blue : Color.Black;
        }

        private void Sorting()
        {
            this.detailgrid.ValidateControl();
            if (this.CurrentDetailData == null)
            {
                return;
            }

            DataView dv = ((DataTable)this.detailgridbs.DataSource).DefaultView;
            dv.Sort = this.detailSort;
        }

        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetInt(this.CurrentMaintain["UseCutRefToRequestFabric"]) == 0)
            {
                this.OnRefreshClick();
                MyUtility.Msg.WarningBox("Please go to Cutting_P01 to set [Use Cutting Cutref# to Request Fabric].");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();

            // 編輯時，將[SORT_NUM]賦予流水號
            int serial = 1;
            this.detailgridbs.SuspendBinding();
            this.DetailDatas.AsEnumerable().ToList().ForEach(row => row["SORT_NUM"] = serial++);
            this.detailgridbs.ResumeBinding();
            ((DataTable)this.detailgridbs.DataSource).AcceptChanges();
        }
        #endregion

        #region Grid 連動

        /// <inheritdoc/>
        protected override void OnDetailGridRowChanged()
        {
            this.GridValidateControl();
            base.OnDetailGridRowChanged(); // 此後 CurrentDetailData 才會是新的

            this.bindingSourceDetail.SetRow(this.CurrentDetailData);

            // 變更子表可否編輯
            bool canEdit = this.CanEditData(this.CurrentDetailData);
            bool canEditNotWithUseCutRefToRequestFabric = this.CanEditNotWithUseCutRefToRequestFabric(this.CurrentDetailData);
            this.btnEdit.Enabled = canEditNotWithUseCutRefToRequestFabric;
            this.gridicon.Insert.Enabled = this.editByUseCutRefToRequestFabric;
            this.gridicon.Append.Enabled = this.editByUseCutRefToRequestFabric;
            this.gridicon.Remove.Enabled = canEditNotWithUseCutRefToRequestFabric;
            this.btnDistributeThisCutRef.Enabled = canEdit;
            this.numConsPC.ReadOnly = !canEdit;
            this.cmsSizeRatio.Enabled = canEdit;
            this.cmsDistribute.Enabled = canEdit;
            this.txtPatternNo.ReadOnly = !canEdit;
            this.txtMarkerLength.ReadOnly = !canEditNotWithUseCutRefToRequestFabric;
            this.gridSizeRatio.IsEditingReadOnly = !canEdit;
            this.gridDistributeToSP.IsEditingReadOnly = !canEdit;

            // 避免後面炸掉, 即使 0 筆上面也要執行
            if (this.CurrentDetailData == null)
            {
                return;
            }

            #region Total Layer / Balance Layer
            /*
                Total Layer計算方式
                根據CurrentDetailData的Order_EachconsUkey 判斷
                1. Order_EachconsUkey 為空 => 使用 TotalLayerMarker
                2. Order_EachconsUkey 不為空 => 使用 TotalLayer_byOrder_EachCons

                Balance Layer計算方式
                群組加總"當前"WorkOrderForPlanning表身的 Layer 欄位，根據CurrentDetailData的Order_EachconsUkey 判斷群組條件
                1. Order_EachconsUkey 為空  => MarkerName、Colorid
                2. Order_EachconsUkey 不為空 =>Order_EachconsUkey、Colorid
             */

            int sumlayer = 0;
            string filterLayer;

            if (MyUtility.Check.Empty(this.CurrentDetailData["Order_EachConsUkey"]))
            {
                filterLayer = $"MarkerName = '{this.CurrentDetailData["MarkerName"]}' and Colorid = '{this.CurrentDetailData["Colorid"]}'";
            }
            else
            {
                filterLayer = $"Order_EachConsUkey = '{this.CurrentDetailData["Order_EachConsUkey"]}' and Colorid = '{this.CurrentDetailData["Colorid"]}'";
            }

            DataRow[] cur = ((DataTable)this.detailgridbs.DataSource).Select(filterLayer);
            sumlayer = cur.AsEnumerable().Sum(l => MyUtility.Convert.GetInt(l["layer"]));

            if (MyUtility.Check.Empty(this.CurrentDetailData["Order_EachConsUkey"]))
            {
                this.numTotalLayer.Text = string.Empty;
                this.numBalanceLayer.Text = string.Empty;
            }
            else
            {
                DataRow[] laydr = this.dt_Layers.Select($"Order_EachConsUkey = '{this.CurrentDetailData["Order_EachConsUkey"]}' and ColorID = '{this.CurrentDetailData["Colorid"]}'");
                if (!laydr.Any())
                {
                    this.numTotalLayer.Value = 0;
                    this.numBalanceLayer.Value = 0;
                }
                else
                {
                    decimal totalLayer = (decimal)laydr[0]["TotalLayer_byOrder_EachCons"];

                    this.numTotalLayer.Value = totalLayer;
                    this.numBalanceLayer.Value = sumlayer - totalLayer;
                }
            }

            #endregion

            // 根據左邊Grid Filter 右邊資訊
            string filter = GetFilter(this.CurrentDetailData, this.formType);
            this.sizeRatiobs.Filter = filter;
            this.distributebs.Filter = filter;
            this.spreadingfabricbs.Filter = $"CutRef = '{this.CurrentDetailData["CutRef"]}' AND SCIRefno = '{this.CurrentDetailData["SCIRefno"]}' AND ColorID = '{this.CurrentDetailData["ColorID"]}'";

            this.ChangeQtyBreakDownRow();
            Grid_ClickBeginEdit((object)this.detailgrid, null);
        }

        private void GridDistributeToSP_SelectionChanged(object sender, EventArgs e)
        {
            this.ChangeQtyBreakDownRow();
        }

        private void ChangeQtyBreakDownRow()
        {
            // 更換qtybreakdown index
            DataRow dr = this.gridDistributeToSP.CurrentDataRow;
            if (MyUtility.Check.Empty(dr))
            {
                return;
            }

            string article = MyUtility.Convert.GetString(dr["Article"]);
            string sizeCode = MyUtility.Convert.GetString(dr["SizeCode"]);
            string spNo = MyUtility.Convert.GetString(dr["Orderid"]);

            if (this.dt_Distribute.Rows.Count > 1)
            {
                var findRow = this.gridQtyBreakDown.Rows
                    .Cast<DataGridViewRow>()
                    .Select((row, index) => new { Row = row, Index = index })
                    .FirstOrDefault(x =>
                        MyUtility.Convert.GetString(((DataRowView)x.Row.DataBoundItem).Row["article"]) == article &&
                        MyUtility.Convert.GetString(((DataRowView)x.Row.DataBoundItem).Row["SizeCode"]) == sizeCode &&
                        MyUtility.Convert.GetString(((DataRowView)x.Row.DataBoundItem).Row["id"]) == spNo)?.Index ?? 0;

                this.gridQtyBreakDown.SelectRowTo(findRow);
            }
        }
        #endregion

        #region Import 與 Save 刪除/新增 與 AGV
        private void BtnImportFromWorkOrderForPlanning_Click(object sender, EventArgs e)
        {
            using (var frm = new P09_ImportFromWorkOrderForPlanning(this.CurrentMaintain["ID"].ToString(), (DataTable)this.detailgridbs.DataSource, this.dt_PatternPanel, this.dt_SizeRatio, this.dt_Distribute, this.editByUseCutRefToRequestFabric))
            {
                if (frm.ShowDialog() == DialogResult.OK && !this.editByUseCutRefToRequestFabric)
                {
                    // 開始重新分配WorkOrderForOutput_Distribute
                    var canEditDetailDatas = this.DetailDatas.Where(r => MyUtility.Convert.GetBool(r["ImportWP"])).ToList();
                    foreach (DataRow drWorkOrder in canEditDetailDatas)
                    {
                        var p09_AutoDistToSP = new P09_AutoDistToSP(drWorkOrder, this.dt_SizeRatio, this.dt_Distribute, this.dt_PatternPanel, this.formType);
                        //DualResult result = p09_AutoDistToSP.DoAutoDistribute(); ISP20250495 不需要重新計算，直接帶入P02的 Distribute 即可
                        //if (!result)
                        //{
                        //    this.ShowErr(result);
                        //    return;
                        //}
                    }

                    this.btnAutoCut.PerformClick();
                }
            }
        }

        private void BtnImportMarker_Click(object sender, EventArgs e)
        {
            CuttingWorkOrder cw = new CuttingWorkOrder();
            this.ShowWaitMessage("Processing...");
            DualResult result = cw.ImportMarkerExcel(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), Sci.Env.User.Keyword, Sci.Env.User.Factory, this.formType);
            this.HideWaitMessage();
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (result.Description == "NotImport")
            {
                return;
            }

            this.OnRefreshClick();
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            CuttingWorkOrder cuttingWorkOrder = new CuttingWorkOrder();
            string errMsg;
            if (!cuttingWorkOrder.DownloadFile(this.formType, this.CurrentMaintain, out errMsg))
            {
                MyUtility.Msg.ErrorBox(errMsg);
            }
        }

        // 編輯模式下使用
        private void BtnImportMarkerLectra_Click(object sender, EventArgs e)
        {
            // 寫入表身, 不是寫入DB
            string id = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);
            string sqlcmd = $@"
select top 1 s.SizeGroup, s.PatternNo, oe.markerNo, s.ID, p.Version
from Order_EachCons oe
inner join dbo.SMNotice s on oe.SMNoticeID = s.ID
inner join SMNotice_Detail sd with(nolock)on sd.id = s.id
inner join Pattern p with(nolock)on p.id = sd.id
where oe.ID = '{id}'
and sd.PhaseID = 'Bulk'
and p.Status='Completed'
order by p.EditDate desc
";
            if (MyUtility.Check.Seek(sqlcmd, out DataRow drSMNotice))
            {
                string styleUkey = MyUtility.GetValue.Lookup($@"select o.StyleUkey from Orders o where o.id = '{id}'");
                var form = new ImportML(this.formType, styleUkey, id, drSMNotice, (DataTable)this.detailgridbs.DataSource);
                form.ShowDialog();
            }
            else
            {
                MyUtility.Msg.InfoBox("Not found SMNotice Data."); // 正常不會發生這狀況
            }

            #region 產生第3層 PatternPanel 只有一筆
            this.DetailDatas.AsEnumerable().Where(w => MyUtility.Convert.GetBool(w["ImportML"])).ToList().ForEach(row =>
            {
                DataRow drNEW = this.dt_PatternPanel.NewRow();
                drNEW["id"] = this.CurrentMaintain["ID"];
                drNEW["WorkOrderForOutputUkey"] = 0;  // 新增 WorkOrderForOutputUkey 塞0
                drNEW["PatternPanel"] = row["PatternPanel_CONCAT"]; // 這邊只會有一筆，因為資料來源是DB
                drNEW["FabricPanelCode"] = row["FabricPanelCode"];
                drNEW["tmpKey"] = row["tmpKey"];
                this.dt_PatternPanel.Rows.Add(drNEW);
            });
            #endregion

            int icount = this.DetailDatas.AsEnumerable().Where(w => MyUtility.Convert.GetBool(w["ImportML"])).Count();
            if (icount > 0)
            {
                for (int i = 0; i < icount; i++)
                {
                    if (this.detailgrid.CurrentCell != null)
                    {
                        this.detailgrid.CurrentCell = this.detailgrid.Rows[i].Cells["Layer"]; // 移動到指定cell 觸發 Con 計算
                    }
                }
            }

            if (icount > 0)
            {
                this.detailgrid.CurrentCell = this.detailgrid.Rows[0].Cells["Layer"];
                this.detailgrid.SelectRowTo(0);
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            this.GridValidateControl();

            // 在 SaveBefore 先判斷是否有變更任何資訊, 因為底層過了 ClickSavePre 狀態就會改變, 這是用來控制 P20 是否重新執行 Confrim 其中一變數
            this.detaildatasHasChange = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Any(w => w.RowState != DataRowState.Unchanged);
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

            // 找出表身 CutRef 有值 && LastCreateCutRefDate 為空的資訊, 填入 表身 LastCreateCutRefDate = 現在時間
            this.DetailDatas.AsEnumerable().Where(w => !MyUtility.Check.Empty(w["CutRef"]) && MyUtility.Check.Empty(w["LastCreateCutRefDate"])).ToList().ForEach(row => row["LastCreateCutRefDate"] = DateTime.Now);

            #region 第3層 處理

            // Step 1. 刪除空值
            var needDeleteSize = this.dt_SizeRatio.Select("Qty = 0 OR SizeCode = ''");
            bool changeSizeData = needDeleteSize.Length > 0;
            needDeleteSize.Delete();
            this.dt_Distribute.Select("Qty = 0 OR OrderID = '' OR SizeCode = ''").Delete();
            this.dt_Distribute.Select("OrderID <> 'EXCESS' AND Article = ''").Delete(); // EXCESS 項 Article 為空

            // 檢查 第3層 重複 Key
            var checkSizeRatio = new List<string> { "WorkOrderForOutputUkey", "tmpKey", "SizeCode" }; // 檢查的 Key
            if (!CheckDuplicateAndShowMessage(this.dt_SizeRatio, checkSizeRatio, "SizeRatio", this.DetailDatas, this.formType))
            {
                return false;
            }

            var checkDistribute = new List<string> { "WorkOrderForOutputUkey", "tmpKey", "OrderID", "Article", "SizeCode" }; // 檢查的 Key
            if (!CheckDuplicateAndShowMessage(this.dt_Distribute, checkDistribute, "Distribute", this.DetailDatas, this.formType))
            {
                return false;
            }

            var checkPatternPanel = new List<string> { "WorkOrderForOutputUkey", "tmpKey", "PatternPanel", "FabricPanelCode" }; // 檢查的 Key
            if (!CheckDuplicateAndShowMessage(this.dt_PatternPanel, checkPatternPanel, "PatternPanel", this.DetailDatas, this.formType))
            {
                return false;
            }

            // 檢查第3層 Total distributionQty 是否大於 TotalCutQty 總和
            foreach (DataRow dr in this.DetailDatas)
            {
                string filter = GetFilter(dr, this.formType);
                decimal ttlCutQty = this.dt_SizeRatio.Select(filter).Sum(row => MyUtility.Convert.GetInt(row["Qty"])) * MyUtility.Convert.GetDecimal(dr["Layer"]);
                decimal ttlDisQty = this.dt_Distribute.Select(filter).Sum(row => MyUtility.Convert.GetInt(row["Qty"]));
                if (ttlCutQty < ttlDisQty)
                {
                    MyUtility.Msg.WarningBox($"CutRef:{dr["CutRef"]},CutNo:{dr["CutNo"]},MarkerName:{dr["MarkerName"]} Distribution Qty can not exceed total Cut qty");
                    return false;
                }
            }
            #endregion

            // 刪除 SizeRatio 之後重算 ConsPC
            if (changeSizeData)
            {
                BeforeSaveCalculateConsPC(this.DetailDatas, this.dt_SizeRatio, this.formType);
            }

            // 更新 Cutting 欄位
            this.CurrentMaintain["CutForOutputInline"] = this.DetailDatas.AsEnumerable().Min(row => MyUtility.Convert.GetDate(row["EstCutDate"])) ?? (object)DBNull.Value;
            this.CurrentMaintain["CutForOutputOffline"] = this.DetailDatas.AsEnumerable().Max(row => MyUtility.Convert.GetDate(row["EstCutDate"])) ?? (object)DBNull.Value;

            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePre()
        {
            // 在 base.ClickSavePre() 之前表身在資料庫還沒有刪除
            DualResult result;
            #region WorkOrderForOutputDelete
            string sqlcmdWorkOrderForOutputDelete = @"
INSERT INTO [dbo].[WorkOrderForOutputDelete]
           ([ID]
           ,[GroupID]
           ,[CutRef]
           ,[Layer])
SELECT wo.ID, wo.GroupID, wo.CutRef, wo.Layer
FROM #tmp t
INNER JOIN WorkOrderForOutput wo WITH(NOLOCK) ON wo.Ukey = t.Ukey
";
            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dtDeleteUkey_HasGroup, "Ukey", sqlcmdWorkOrderForOutputDelete, out DataTable odt)))
            {
                return result;
            }
            #endregion
            return base.ClickSavePre();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            DualResult result;

            // Stpe 1. 給第3層填入對應 WorkOrderForOutputUkey
            foreach (DataRow dr in this.DetailDatas.Where(row => row.RowState == DataRowState.Added))
            {
                long ukey = MyUtility.Convert.GetLong(dr["Ukey"]); // ClickSavePost 時,底層已取得 Key 值
                string filterAddData = $"tmpkey = {dr["tmpkey"]}";
                this.dt_SizeRatio.Select(filterAddData).AsEnumerable().ToList().ForEach(row => row["WorkOrderForOutputUkey"] = ukey);
                this.dt_Distribute.Select(filterAddData).AsEnumerable().ToList().ForEach(row => row["WorkOrderForOutputUkey"] = ukey);
                this.dt_PatternPanel.Select(filterAddData).AsEnumerable().ToList().ForEach(row => row["WorkOrderForOutputUkey"] = ukey);
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
AND wd.Qty <> t.Qty
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
            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dt_SizeRatio, string.Empty, sqlDeleteSizeRatio, out DataTable dtDeleteSizeRatio)))
            {
                return result;
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dt_SizeRatio, string.Empty, sqlUpdateSizeRatio, out DataTable dtUpdateSizeRatio)))
            {
                return result;
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dt_SizeRatio, string.Empty, sqlInsertSizeRatio, out DataTable dtInsertSizeRatio)))
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
            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dt_PatternPanel, string.Empty, sqlDeletePatternPanel, out DataTable dtDeletePatternPanel)))
            {
                return result;
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dt_PatternPanel, string.Empty, sqlInsertPatternPanel, out DataTable dtInsertPatternPanel)))
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
AND wd.Qty <> t.Qty
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
            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dt_Distribute, string.Empty, sqlDeleteDistribute, out DataTable dtDeleteDistribute)))
            {
                return result;
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dt_Distribute, string.Empty, sqlUpdateDistribute, out DataTable dtUpdateDistribute)))
            {
                return result;
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.dt_Distribute, string.Empty, sqlInsertDistribute, out DataTable dtInsertDistribute)))
            {
                return result;
            }
            #endregion

            #region 回寫Orders CutInLine, CutOffLine
            var maxEstCutDate = this.DetailDatas.Max(row => MyUtility.Convert.GetDate(row["EstCutDate"])) ?? (object)DBNull.Value;
            var minEstCutDate = this.DetailDatas.Min(row => MyUtility.Convert.GetDate(row["EstCutDate"])) ?? (object)DBNull.Value;
            string sqlcmdOrders = $@"UPDATE Orders SET CutInLine = @CutInLine, CutOffLine = @CutOffLine WHERE POID = '{this.CurrentMaintain["ID"]}'";
            result = DBProxy.Current.ExecuteEx(sqlcmdOrders, "CutInLine", minEstCutDate, "CutOffLine", maxEstCutDate);
            if (!result)
            {
                return result;
            }
            #endregion

            // sent data to GZ WebAPI
            dtUpdateDistribute.Merge(dtInsertDistribute);
            List<long> listDeleteUkey = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(s => s.RowState == DataRowState.Deleted).Select(s => MyUtility.Convert.GetLong(s["Ukey", DataRowVersion.Original])).ToList();
            List<long> cutRefToEmptyUkey = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(s => s.RowState == DataRowState.Modified && MyUtility.Check.Empty(s["CutRef", DataRowVersion.Current]) && !MyUtility.Check.Empty(s["CutRef", DataRowVersion.Original])).Select(s => MyUtility.Convert.GetLong(s["Ukey", DataRowVersion.Original])).ToList();
            this.SentChangeDataToGuozi_AGV(dtUpdateDistribute);
            this.SentDeleteDataToGuozi_AGV(listDeleteUkey, cutRefToEmptyUkey, dtDeleteDistribute);

            // 這要控制好 P20 重新 Confirm 有些資料量很大
            this.ReUpdateP20 =
                this.detaildatasHasChange ||
                (dtDeleteSizeRatio?.AsEnumerable().Any() ?? false) ||
                (dtUpdateSizeRatio?.AsEnumerable().Any() ?? false) ||
                (dtInsertSizeRatio?.AsEnumerable().Any() ?? false) ||
                (dtDeleteDistribute?.AsEnumerable().Any() ?? false) ||
                (dtUpdateDistribute?.AsEnumerable().Any() ?? false) ||
                (dtInsertDistribute?.AsEnumerable().Any() ?? false) ||
                (dtDeletePatternPanel?.AsEnumerable().Any() ?? false) ||
                (dtInsertPatternPanel?.AsEnumerable().Any() ?? false);

            return base.ClickSavePost();
        }

        private void SentChangeDataToGuozi_AGV(DataTable dtChangeDistribute)
        {
            // 傳送新增 & 調整資訊
            List<Guozi_AGV.WorkOrder_Distribute> editWorkOrder_Distribute = new List<Guozi_AGV.WorkOrder_Distribute>();
            dtChangeDistribute.ExtNotDeletedRowsForeach(row => editWorkOrder_Distribute.Add(new Guozi_AGV.WorkOrder_Distribute() { WorkOrderUkey = MyUtility.Convert.GetLong(row["WorkOrderForOutputUkey"]) }));

            // 準備調整清單, CutRef 有值時, 此3狀況要傳 1.新增 2.compareCol欄位真的有變動 3.有變動 Distribute
            string compareCol = "CutRef,EstCutDate,ID,OrderID,CutCellID";
            var listChangedDetail = this.DetailDatas
                .Where(s =>
                {
                    return !MyUtility.Check.Empty(s["CutRef"]) &&
                        (s.RowState == DataRowState.Added ||
                         (s.RowState == DataRowState.Modified && s.CompareDataRowVersionValue(compareCol)) ||
                         editWorkOrder_Distribute.Any(ed => ed.WorkOrderUkey == MyUtility.Convert.GetLong(s["Ukey"])));
                });

            // 傳送
            if (listChangedDetail.Any())
            {
                DataTable dtWorkOrder = listChangedDetail.CopyToDataTable();
                Task.Run(() => new Guozi_AGV().SentWorkOrderToAGV(dtWorkOrder)).ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void SentDeleteDataToGuozi_AGV(List<long> listDeleteUkey, List<long> cutRefToEmptyUkey, DataTable dtDeleteDistribute)
        {
            // 傳送刪除資訊
            List<Guozi_AGV.WorkOrder_Distribute> deleteWorkOrder_Distribute = new List<Guozi_AGV.WorkOrder_Distribute>();
            List<long> deleteWorkOrder = new List<long>();

            // 加入被刪除的 WorkOrder, Distribute
            deleteWorkOrder.AddRange(listDeleteUkey);
            deleteWorkOrder_Distribute.AddRange(
                dtDeleteDistribute.AsEnumerable()
                .Select(row => new Guozi_AGV.WorkOrder_Distribute
                {
                    WorkOrderUkey = MyUtility.Convert.GetLong(row["WorkOrderForOutputUkey"]),
                    SizeCode = MyUtility.Convert.GetString(row["SizeCode"]),
                    Article = MyUtility.Convert.GetString(row["Article"]),
                    OrderID = MyUtility.Convert.GetString(row["OrderID"]),
                }));

            // CutRef 被清空要傳 Delete 給廠商
            foreach (var ukey in cutRefToEmptyUkey)
            {
                deleteWorkOrder.Add(ukey);

                // 不是刪除的,是被清空CutRef 對應的 Distribute
                deleteWorkOrder_Distribute.AddRange(
                    this.dt_Distribute.AsEnumerable()
                    .Where(x => x.RowState != DataRowState.Deleted && (MyUtility.Convert.GetLong(x["WorkOrderForOutputUkey"]) == ukey))
                    .Select(s => new Guozi_AGV.WorkOrder_Distribute
                    {
                        WorkOrderUkey = MyUtility.Convert.GetLong(s["WorkOrderForOutputUkey"]),
                        SizeCode = MyUtility.Convert.GetString(s["SizeCode"]),
                        Article = MyUtility.Convert.GetString(s["Article"]),
                        OrderID = MyUtility.Convert.GetString(s["OrderID"]),
                    }));
            }

            // 傳送
            Task.Run(() => new Guozi_AGV().SentDeleteWorkOrder(deleteWorkOrder));
            Task.Run(() => new Guozi_AGV().SentDeleteWorkOrder_Distribute(deleteWorkOrder_Distribute));
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();

            // 更新 P20
            this.BackgroundWorker1.RunWorkerAsync();
            this.OnRefreshClick();
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

        #region 單筆操作 彈窗ActionCutRef 新/修/刪 & 批次更新視窗
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            #region 校驗
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("There is no Workorder data that can be modified.");
                return;
            }

            if (!this.CheckAndMsg("modify", this.CurrentDetailData))
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
            base.OnDetailGridInsert(index == -1 ? 0 : index);

            // 先取得當前編輯狀態的最新 tmpKey
            this.CurrentDetailData["tmpKey"] = this.DetailDatas.AsEnumerable().Max(row => MyUtility.Convert.GetLong(row["tmpKey"])) + 1;
            this.CurrentDetailData["CutNo"] = DBNull.Value;
            this.CurrentDetailData["CanEdit"] = true;
            this.CurrentDetailData["SpreadingStatus"] = "Ready";
            this.CurrentDetailData["SourceFrom"] = 2;
            this.CurrentDetailData["Adduser"] = MyUtility.GetValue.Lookup($"SELECT NAME FROM Pass1 WITH (NOLOCK) Where ID = '{this.CurrentDetailData["AddName"]}'");
            if (oldRow == null)
            {
                // 按 + 或 插入, 無表身時, 第一筆只能從 Cutting 欄位帶入
                this.CurrentDetailData["FactoryID"] = this.CurrentMaintain["FactoryID"];
                this.CurrentDetailData["MDivisionId"] = this.CurrentMaintain["MDivisionId"];
                this.CurrentDetailData["OrderID"] = this.CurrentMaintain["ID"];
            }
            else
            {
                this.CurrentDetailData["FactoryID"] = oldRow["FactoryID"];
                this.CurrentDetailData["MDivisionId"] = oldRow["MDivisionId"];
                this.CurrentDetailData["MarkerNo"] = oldRow["MarkerNo"];
                this.CurrentDetailData["SORT_NUM"] = oldRow["SORT_NUM"];

                if (index == -1 || this.CurrentMaintain["WorkType"].ToString() == "1")
                {
                    this.CurrentDetailData["OrderID"] = this.CurrentMaintain["ID"];
                }
                else
                {
                    this.CurrentDetailData["OrderID"] = oldRow["OrderID"];
                }
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
                    "CutRef",
                    "CutNo",
                    "SourceFrom",
                    "CanEdit",
                    "SpreadingStatus",
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
                AddThirdDatas(this.CurrentDetailData, oldRow, this.dt_SizeRatio, this.formType);
                AddThirdDatas(this.CurrentDetailData, oldRow, this.dt_Distribute, this.formType);
                AddThirdDatas(this.CurrentDetailData, oldRow, this.dt_PatternPanel, this.formType);
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
            form.CurrentDetailData_Ori = this.CurrentDetailData;
            form.dt_SizeRatio_Ori = this.dt_SizeRatio;
            form.dt_Distribute_Ori = this.dt_Distribute;
            form.dt_PatternPanel_Ori = this.dt_PatternPanel;
            string filter = GetFilter(this.CurrentDetailData, this.formType);

            DataTable clonedTable = this.CurrentDetailData.Table.Clone();
            DataRow newRow = clonedTable.NewRow();
            newRow.ItemArray = this.CurrentDetailData.ItemArray.Clone() as object[];
            clonedTable.Rows.Add(newRow);
            form.CurrentDetailData = newRow;

            form.dt_SizeRatio = this.dt_SizeRatio.Select(filter).TryCopyToDataTable(this.dt_SizeRatio);
            form.dt_Distribute = this.dt_Distribute.Select(filter).TryCopyToDataTable(this.dt_Distribute);
            form.dt_PatternPanel = this.dt_PatternPanel.Select(filter).TryCopyToDataTable(this.dt_PatternPanel);
            form.editByUseCutRefToRequestFabric = this.editByUseCutRefToRequestFabric;
            return form.ShowDialog();
        }

        protected override void OnDetailGridDelete()
        {
            if (this.detailgrid.SelectedRows.Count == 0 || this.CurrentDetailData == null)
            {
                return;
            }

            // 選擇多筆刪除時,先檢查是否有不可刪除的資料
            foreach (DataGridViewRow item in this.detailgrid.SelectedRows)
            {
                if (item.DataBoundItem is DataRowView dataRowView)
                {
                    DataRow currentDetailData = dataRowView.Row;
                    if (!this.CheckAndMsg("delete", currentDetailData))
                    {
                        return;
                    }
                }
            }

            // 刪除第3層資訊
            foreach (DataGridViewRow item in this.detailgrid.SelectedRows)
            {
                if (item.DataBoundItem is DataRowView dataRowView)
                {
                    DataRow currentDetailData = dataRowView.Row;
                    this.dt_SizeRatio.Select(GetFilter(currentDetailData, this.formType)).Delete();
                    this.dt_Distribute.Select(GetFilter(currentDetailData, this.formType)).Delete();
                    this.dt_PatternPanel.Select(GetFilter(currentDetailData, this.formType)).Delete();
                }
            }

            // 如果WorkOrderForOutput.GroupID != '' 刪除後存檔時要寫入WorkOrderForOutputDelete;
            if (!MyUtility.Check.Empty(this.CurrentDetailData["GroupID"]))
            {
                this.dtDeleteUkey_HasGroup.ImportRow(this.CurrentDetailData);
            }

            base.OnDetailGridDelete(); // 刪除表身
        }

        private void BtnBatchAssign_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();
            var detailDatas = this.DetailDatas.Where(row => this.CanEditNotWithUseCutRefToRequestFabric(row)).ToList();
            if (!detailDatas.Any())
            {
                MyUtility.Msg.InfoBox("No editable data.");
                return;
            }

            var frm = new Cutting_BatchAssign(detailDatas, this.CurrentMaintain["ID"].ToString(), this.formType);
            frm.editByUseCutRefToRequestFabric = this.editByUseCutRefToRequestFabric;
            frm.ShowDialog(this);
        }
        #endregion

        #region Grid 欄位事件 顏色/開窗/驗證
        private void NumConsPC_Validated(object sender, EventArgs e)
        {
            this.CurrentDetailData["Cons"] = CalculateCons(this.CurrentDetailData, MyUtility.Convert.GetDecimal(this.CurrentDetailData["ConsPC"]), MyUtility.Convert.GetDecimal(this.CurrentDetailData["Layer"]), this.dt_SizeRatio, this.formType);
        }

        private void TxtPatternNo_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            string cuttingID = MyUtility.Check.Empty(this.CurrentMaintain) ? string.Empty : MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);
            SelectItem selectItem = PopupMarkerNo(cuttingID, this.txtPatternNo.Text);
            if (selectItem == null)
            {
                return;
            }

            this.txtPatternNo.Text = selectItem.GetSelectedString();
        }

        private void TxtPatternNo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string cuttingID = MyUtility.Check.Empty(this.CurrentMaintain) ? string.Empty : MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);
            if (!ValidatingMarkerNo(cuttingID, this.txtPatternNo.Text))
            {
                this.txtPatternNo.Text = string.Empty;
                e.Cancel = true;
            }
        }

        private void TxtMarkerLength_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.CurrentDetailData["MarkerLength"] = this.txtMarkerLength.FullText;
            this.DetailDatas.AsEnumerable().ToList().ForEach(row => Format4LengthColumn(row)); // 將更新後的 MarkerLength ，按照格式回填表身 4 個_Mask 欄位 用來顯示用

            this.CurrentDetailData["ConsPC"] = CalculateConsPC(this.txtMarkerLength.FullText, this.CurrentDetailData, this.dt_SizeRatio, this.formType);
            this.CurrentDetailData["Cons"] = CalculateCons(this.CurrentDetailData, MyUtility.Convert.GetDecimal(this.CurrentDetailData["ConsPC"]), MyUtility.Convert.GetDecimal(this.CurrentDetailData["Layer"]), this.dt_SizeRatio, this.formType);
        }

        private void GridEventSet()
        {
            // 可否編輯 && 顏色
            ConfigureColumnEvents(this.detailgrid, this.CanEditDataByGrid);
            ConfigureColumnEvents(this.gridSizeRatio, this.CanEditDataByGrid);
            ConfigureColumnEvents(this.gridDistributeToSP, this.CanEditDataByGrid);

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
                        dr["LastCreateCutRefDate"] = DBNull.Value;
                        e.EditingControl.Text = string.Empty;
                        dr.EndEdit();
                    }
                }
            };

            // Seq 兩個欄位
            ConfigureSeqColumnEvents(this.col_Seq1, this.detailgrid, this.CanEditData);
            ConfigureSeqColumnEvents(this.col_Seq2, this.detailgrid, this.CanEditData);

            this.col_Layer.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                int oldvalue = MyUtility.Convert.GetInt(dr["Layer"]);
                int newvalue = MyUtility.Convert.GetInt(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                dr["Layer"] = newvalue;
                int layer = MyUtility.Convert.GetInt(dr["Layer"]);
                UpdateExcess(dr, layer, this.dt_SizeRatio, this.dt_Distribute, this.formType);

                dr["Cons"] = CalculateCons(dr, MyUtility.Convert.GetDecimal(dr["ConsPC"]), MyUtility.Convert.GetDecimal(dr["Layer"]), this.dt_SizeRatio, this.formType);
                UpdateConcatString(dr, this.dt_SizeRatio, this.formType);
                dr.EndEdit();
            };

            BindGridSpreadingNo(this.col_SpreadingNoID, this.detailgrid, this.CanEditData);
            BindGridCutCell(this.col_CutCellID, this.detailgrid, this.CanEditNotWithUseCutRefToRequestFabric);

            this.col_MarkerLength.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string newValue = Prgs.SetMarkerLengthMaskString(e.FormattedValue.ToString());

                if (dr.RowState != DataRowState.Added)
                {
                    string oldValue = MyUtility.Convert.GetString(dr["MarkerLength", DataRowVersion.Original]);

                    if (!this.CanEditNotWithUseCutRefToRequestFabric(dr))
                    {
                        dr["MarkerLength"] = oldValue;
                        return;
                    }
                    else if (oldValue == newValue)
                    {
                        return;
                    }
                }

                dr["ConsPC"] = CalculateConsPC(dr["MarkerLength"].ToString(), dr, this.dt_SizeRatio, this.formType);
                dr["Cons"] = CalculateCons(dr, MyUtility.Convert.GetDecimal(dr["ConsPC"]), MyUtility.Convert.GetDecimal(dr["Layer"]), this.dt_SizeRatio, this.formType);
                dr.EndEdit();
            };
            this.col_ActCuttingPerimeter.CellValidating += this.MaskedCellValidatingHandler;
            this.col_StraightLength.CellValidating += this.MaskedCellValidatingHandler;
            this.col_CurvedLength.CellValidating += this.MaskedCellValidatingHandler;

            #endregion

            #region SizeRatio
            this.col_SizeRatio_Size.EditingMouseDown += (s, e) =>
            {
                if (SizeCodeCellEditingMouseDown(e, this.gridSizeRatio, this.CurrentDetailData, this.dt_Distribute, this.formType, MyUtility.Convert.GetInt(this.CurrentDetailData["Layer"])))
                {
                    UpdateConcatString(this.CurrentDetailData, this.dt_SizeRatio, this.formType);
                }
            };
            this.col_SizeRatio_Size.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData != null && SizeCodeCellValidating(e, this.gridSizeRatio, this.CurrentDetailData, this.dt_Distribute, this.formType, MyUtility.Convert.GetInt(this.CurrentDetailData["Layer"])))
                {
                    UpdateConcatString(this.CurrentDetailData, this.dt_SizeRatio, this.formType);
                    UpdateTotalDistributeQty(this.CurrentDetailData, this.dt_Distribute, this.formType);
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

        private bool CanEditDataByGrid(Sci.Win.UI.Grid grid, DataRow dr, string columNname)
        {
            try
            {
                if (grid.Name == "detailgrid")
                {
                    if (columNname.ToLower() == "orderid" && this.CurrentMaintain["WorkType"].ToString() == "1")
                    {
                        return false;
                    }

                    if (columNname.ToLower() == "cuttingplannerremark" && this.EditMode)
                    {
                        return true;
                    }

                    if (columNname.EqualString("EstCutDate") || columNname.EqualString("CutCellID") || columNname.EqualString("Tone") || columNname.EqualString("MarkerName") || columNname.EqualString("MarkerLength"))
                    {
                        return this.CanEditNotWithUseCutRefToRequestFabric(dr);
                    }

                    return this.CanEditData(dr);
                }
                else
                {
                    if (grid.Name == "gridDistributeToSP" && dr["OrderID"].ToString().EqualString("EXCESS"))
                    {
                        return false;
                    }

                    return this.CanEditData(this.CurrentDetailData);
                }
            }
            catch (Exception)
            {
                // 不接錯誤是因為 新增一筆 後 undo 時,底層取 CurrentDetailData index 跳錯, 不是 null
                throw;
            }
        }

        private void MaskedCellValidatingHandler(object sender, Ict.Win.UI.DataGridViewCellValidatingEventArgs e)
        {
            DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
            if (!this.CanEditData(dr))
            {
                return;
            }

            string columnName = (sender as DataGridViewColumn)?.Name;
            dr[columnName] = dr[columnName + "_Mask"] = SetMaskString(e.FormattedValue.ToString());
            dr.EndEdit();
        }

        private void BindDistributeEvents(Ict.Win.UI.DataGridViewTextBoxColumn column)
        {
            column.EditingMouseDown += (s, e) =>
            {
                if (Distribute3CellEditingMouseDown(e, this.CurrentDetailData, this.dt_SizeRatio, this.gridDistributeToSP, this.formType, MyUtility.Convert.GetInt(this.CurrentDetailData["Layer"])))
                {
                    UpdateMinSewinline(this.CurrentDetailData, this.dt_Distribute, this.formType);
                    string columnName = ((DataGridViewElement)s).DataGridView.Columns[e.ColumnIndex].Name.ToLower();
                    switch (columnName)
                    {
                        case "orderid":
                            UpdateMinOrderID(this.CurrentMaintain["WorkType"].ToString(), this.CurrentDetailData, this.dt_Distribute, this.formType);
                            break;
                        case "article":
                            UpdateArticle_CONCAT(this.CurrentDetailData, this.dt_Distribute, this.formType);
                            break;
                    }
                }
            };
            column.CellValidating += (s, e) =>
            {
                // 多選主表身按刪除會觸發,( this.CurrentDetailData不是 Null, 而是呈現無法執回的錯誤), 只好用 try catch 包住
                try
                {
                    if (this.CurrentDetailData != null && Distribute3CellValidating(e, this.CurrentDetailData, this.dt_SizeRatio, this.gridDistributeToSP, this.formType, MyUtility.Convert.GetInt(this.CurrentDetailData["Layer"])))
                    {
                        UpdateTotalDistributeQty(this.CurrentDetailData, this.dt_Distribute, this.formType);
                        UpdateMinSewinline(this.CurrentDetailData, this.dt_Distribute, this.formType);
                        string columnName = ((DataGridViewElement)s).DataGridView.Columns[e.ColumnIndex].Name.ToLower();
                        switch (columnName)
                        {
                            case "orderid":
                                UpdateMinOrderID(this.CurrentMaintain["WorkType"].ToString(), this.CurrentDetailData, this.dt_Distribute, this.formType);
                                break;
                            case "article":
                                UpdateArticle_CONCAT(this.CurrentDetailData, this.dt_Distribute, this.formType);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.ShowErr(ex);
                }
            };
        }

        private void BindQtyEvents(Ict.Win.UI.DataGridViewNumericBoxColumn column)
        {
            column.CellValidating += (s, e) =>
            {
                Sci.Win.UI.Grid grid = (Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                if (this.CurrentDetailData != null && QtyCellValidating(e, this.CurrentDetailData, grid, this.dt_SizeRatio, this.dt_Distribute, this.formType, MyUtility.Convert.GetInt(this.CurrentDetailData["Layer"])))
                {
                    UpdateConcatString(this.CurrentDetailData, this.dt_SizeRatio, this.formType);
                    UpdateTotalDistributeQty(this.CurrentDetailData, this.dt_Distribute, this.formType);
                    if (grid.Name == "gridSizeRatio")
                    {
                        if (!MyUtility.Check.Empty(this.CurrentDetailData["MarkerLength"]))
                        {
                            this.CurrentDetailData["ConsPC"] = CalculateConsPC(MyUtility.Convert.GetString(this.CurrentDetailData["MarkerLength"]), this.CurrentDetailData, this.dt_SizeRatio, this.formType);
                        }
                        else
                        {
                            this.CurrentDetailData["ConsPC"] = CalculateConsPC(this.CurrentDetailData, MyUtility.Convert.GetDecimal(this.CurrentDetailData["Cons"]), MyUtility.Convert.GetDecimal(this.CurrentDetailData["Layer"]), this.dt_SizeRatio, this.formType);
                        }

                        this.CurrentDetailData["Cons"] = CalculateCons(this.CurrentDetailData, MyUtility.Convert.GetDecimal(this.CurrentDetailData["ConsPC"]), MyUtility.Convert.GetDecimal(this.CurrentDetailData["Layer"]), this.dt_SizeRatio, this.formType);
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

            DataRow newrow = this.dt_SizeRatio.NewRow();
            newrow["ID"] = this.CurrentDetailData["ID"];
            newrow["WorkOrderForOutputUkey"] = this.CurrentDetailData["Ukey"];
            newrow["tmpKey"] = this.CurrentDetailData["tmpKey"];
            newrow["Qty"] = 0;
            newrow["SizeCode"] = string.Empty;
            this.dt_SizeRatio.Rows.Add(newrow);
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
            string filter = GetFilter(this.CurrentDetailData, this.formType);
            this.dt_Distribute.Select(filter + $" AND SizeCode = '{sizeCode}'").Delete();

            // 後刪除 SizeRatio
            dr.Delete();

            UpdateExcess(this.CurrentDetailData, MyUtility.Convert.GetInt(this.CurrentDetailData["Layer"]), this.dt_SizeRatio, this.dt_Distribute, this.formType);
            UpdateMinOrderID(this.CurrentMaintain["WorkType"].ToString(), this.CurrentDetailData, this.dt_Distribute, this.formType);
            UpdateArticle_CONCAT(this.CurrentDetailData, this.dt_Distribute, this.formType);
        }

        private void MenuItemInsertDistribute_Click(object sender, EventArgs e)
        {
            if (this.dt_SizeRatio.DefaultView.ToTable().Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Please insert size ratio data first!");
                return;
            }

            DataRow newrow = this.dt_Distribute.NewRow();
            newrow["ID"] = this.CurrentDetailData["ID"];
            newrow["WorkOrderForOutputUkey"] = this.CurrentDetailData["Ukey"];
            newrow["tmpKey"] = this.CurrentDetailData["tmpKey"];
            newrow["Qty"] = 0;
            newrow["OrderID"] = string.Empty;
            newrow["Article"] = string.Empty;
            newrow["SizeCode"] = string.Empty;
            this.dt_Distribute.Rows.Add(newrow);
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

            UpdateExcess(this.CurrentDetailData, MyUtility.Convert.GetInt(this.CurrentDetailData["Layer"]), this.dt_SizeRatio, this.dt_Distribute, this.formType);
            UpdateMinOrderID(this.CurrentMaintain["WorkType"].ToString(), this.CurrentDetailData, this.dt_Distribute, this.formType);
            UpdateArticle_CONCAT(this.CurrentDetailData, this.dt_Distribute, this.formType);
        }
        #endregion

        #region 4 項檢查 無訊息 & 有訊息
        private bool CanEditNotWithUseCutRefToRequestFabric(DataRow dr)
        {
            return this.EditMode && this.CheckContinue(dr);
        }

        private bool CanEditData(DataRow dr)
        {
            return this.EditMode && this.CheckContinue(dr) && this.editByUseCutRefToRequestFabric;
        }

        private bool CheckContinue(DataRow dr)
        {
            if (dr == null)
            {
                return false;
            }

            // 此欄位是和表身一起撈取 (若及時去DB判斷會卡到爆炸)
            return MyUtility.Convert.GetBool(dr["CanEdit"]);
        }

        private bool CheckAndMsg(string action, DataRow currentDetailData)
        {
            // 前 3 個都是及時去 DB 撈資料判斷, 並彈出訊息
            // 1. 存在 P10 Bundle
            string msg = $"The following bundle data exists and cannot be {action}. If you need to {action}, please go to [Cutting_P10. Bundle Card] to {action} the bundle data.";
            if (!CheckBundleAndShowData(currentDetailData["CutRef"].ToString(), msg))
            {
                return false;
            }

            // 2. 存在 P20 CuttingOutput
            msg = $"The following cutting output data exists and cannot be {action}. If you need to {action}, please go to [Cutting_P20. Cutting Daily Output] to {action} the cutting output data.";
            if (!CheckCuttingOutputAndShowData(currentDetailData["CutRef"].ToString(), msg))
            {
                return false;
            }

            // 3. 存在 P05 MarkerReq_Detail
            msg = $"The following marker request data exists and cannot be {action}. If you need to {action}, please go to [Cutting_P05. Bulk Marker Request] to {action} the marker request data.";
            if (!CheckMarkerReqAndShowData(currentDetailData["CutRef"].ToString(), msg))
            {
                return false;
            }

            // 4 檢查欄位 SpreadingStatus
            if (!CheckSpreadingStatus(currentDetailData, $"The following digitail spreading data exists and cannot be {action}"))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 列印
        protected override void DoPrint()
        {
            this.drBeforeDoPrintDetailData = this.CurrentDetailData;
            base.DoPrint(); // 會重新載入資訊,並將this.CurrentDetailData移動到第一筆
        }

        protected override bool ClickPrint()
        {
            Cutting_Print_P09 callNextForm;
            if (this.drBeforeDoPrintDetailData != null)
            {
                callNextForm = new Cutting_Print_P09(this.drBeforeDoPrintDetailData);
                callNextForm.ShowDialog(this);
            }
            else if (this.drBeforeDoPrintDetailData == null && this.CurrentDetailData != null)
            {
                callNextForm = new Cutting_Print_P09(this.CurrentDetailData);
                callNextForm.ShowDialog(this);
            }
            else
            {
                MyUtility.Msg.InfoBox("No datas");
                return false;
            }

            return base.ClickPrint();
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            this.detailgrid.ToExcel(false);
        }
        #endregion

        #region Other
        private void GridValidateControl()
        {
            // 若 Cell 還在編輯中, 即游標還在 Cell 中, 進行此 Cell 驗證事件, 並結束編輯
            // 例如 P09:在最大值的 Cutno:5 按下 Backspace 此 cell 還沒結束編輯, 直接點 Auto CutNo 功能, 下一筆會編碼成 6, 且原本 5 這筆會是空白, 所以要先結束 Cell 編輯狀態
            this.detailgrid.ValidateControl();
            this.gridSizeRatio.ValidateControl();
            this.gridDistributeToSP.ValidateControl();
        }

        // 用在傳入 Column 使用, 因為 gridset 是一開啟就會跑完, 直接傳 WorkType 字串, 換筆資料就不會變動了
        private string GetWorkType()
        {
            return MyUtility.Convert.GetString(this.CurrentMaintain["WorkType"]);
        }

        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.OnRefreshClick();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.OnRefreshClick();
        }

        private void P09_SizeChanged(object sender, EventArgs e)
        {
            if (this.Width > 1252)
            {
                SetControlFontSize(this.tableLayoutPanel_Button1, 10);
                SetControlFontSize(this.tableLayoutPanel_Button2, 10);
            }
            else
            {
                SetControlFontSize(this.tableLayoutPanel_Button1, 9);
                SetControlFontSize(this.tableLayoutPanel_Button2, 9);
            }
        }

        #endregion

        #region 自動 編碼 / 分配
        private void BtnAutoRef_Click(object sender, EventArgs e)
        {
            this.OnRefreshClick();
            AutoCutRef(this.CurrentMaintain["ID"].ToString(), Sci.Env.User.Keyword, (DataTable)this.detailgridbs.DataSource, this.formType);
            this.OnRefreshClick();
        }

        private void BtnAutoCut_Click(object sender, EventArgs e)
        {
            this.GridValidateControl();
            AutoCut(this.DetailDatas);
        }

        private void BtnAllSPDistribute_Click(object sender, EventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            this.detailgrid.EndEdit();

            // 先將可分配的WorkOrderForOutput 下面的WorkOrderForOutput_Distribute清空
            var canEditDetailDatas = this.DetailDatas.Where(row => this.CheckContinue(row));
            foreach (DataRow drWorkOrder in canEditDetailDatas)
            {
                this.dt_Distribute.Select(GetFilter(drWorkOrder, this.formType)).Delete();
            }

            // 開始重新分配WorkOrderForOutput_Distribute
            foreach (DataRow drWorkOrder in canEditDetailDatas)
            {
                var p09_AutoDistToSP = new P09_AutoDistToSP(drWorkOrder, this.dt_SizeRatio, this.dt_Distribute, this.dt_PatternPanel, this.formType);
                DualResult result = p09_AutoDistToSP.DoAutoDistribute();
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
            }
        }

        private void BtnDistributeThisCutRef_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null || this.DetailDatas.Count == 0)
            {
                return;
            }

            this.detailgrid.ValidateControl();
            var frm = new P09_AutoDistToSP(this.CurrentDetailData, this.dt_SizeRatio, this.dt_Distribute, this.dt_PatternPanel, this.formType);
            frm.ShowDialog(this);
        }
        #endregion

        #region 打開看看的視窗
        private void BtnCutPartsCheck_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            var frm = new Cutpartcheck(this.formType, this.CurrentMaintain["ID"].ToString(), this.DetailDatas, this.dt_SizeRatio, this.dt_Distribute);
            frm.ShowDialog(this);
        }

        private void BtnCutPartsCheckSummary_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            var frm = new Cutpartchecksummary(this.formType, this.CurrentMaintain["ID"].ToString(), this.DetailDatas, this.dt_SizeRatio, this.dt_Distribute);
            frm.ShowDialog(this);
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            new P09_History(this.dtsHistory).ShowDialog();
        }

        private void BtnQtyBreakdown_Click(object sender, EventArgs e)
        {
            MyUtility.Check.Seek($@"select isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList from Orders o WITH (NOLOCK) where ID = '{this.CurrentMaintain["ID"]}'", out DataRow dr);
            PPIC.P01_Qty callNextForm = new PPIC.P01_Qty(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), dr["PoList"].ToString());
            callNextForm.ShowDialog(this);
        }

        private void BtnPackingMethod_Click(object sender, EventArgs e)
        {
            this.gridSizeRatio.ValidateControl();
            this.gridQtyBreakDown.ValidateControl();
            this.detailgrid.ValidateControl();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            var frm = new PackingMethod(false, this.CurrentMaintain["id"].ToString(), null, null);
            frm.ShowDialog(this);
            this.RenewData();
            DataView dv = ((DataTable)this.detailgridbs.DataSource).DefaultView;
            dv.Sort = this.detailSort;
            this.OnDetailEntered();
        }
        #endregion
    }
#pragma warning restore SA1600 // Elements should be documented
}