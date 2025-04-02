using Ict;
using Ict.Win;
using Sci.Andy;
using Sci.Data;
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
using System.Windows.Forms;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
#pragma warning disable SA1600 // Elements should be documented
    /// <inheritdoc/>
    public partial class P02 : Win.Tems.Input6
    {
        #region 全域變數
        private readonly CuttingForm formType = CuttingForm.P02;
        private readonly BindingSource2 bindingSourceDetail = new BindingSource2();
        private Ict.Win.UI.DataGridViewTextBoxColumn col_CutRef;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq1;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq2;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Tone;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Layer;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_CutCellID;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_SizeRatio_Size;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_SizeRatio_Qty;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Distribute_SP;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Distribute_Article;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Distribute_Size;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Distribute_Qty;
        private DataTable dt_SizeRatio; // 第3層表:新刪修
        private DataTable dt_Distribute; // 第3層表:新刪修
        private DataTable dt_PatternPanel; // 第3層表:新刪修
        private DataTable dt_Layers;
        private DataRow drBeforeDoPrintDetailData;  // 紀錄目前表身選擇的資料，因為base.DoPrint() 時會LOAD資料,並將this.CurrentDetailData移動到第一筆
        private string detailSort = "SORT_NUM, PatternPanel_CONCAT, multisize DESC, Article_CONCAT, Order_SizeCode_Seq DESC, MarkerName, Ukey";
        private bool editByUseCutRefToRequestFabric;
        #endregion

        #region 程式開啟時

        /// <inheritdoc/>
        public P02(ToolStripMenuItem menuitem, string history)
            : base(menuitem)
        {
            this.InitializeComponent();

            if (history == "0")
            {
                this.Text = "P02. WorkOrder For Planning";
                this.DefaultFilter = $"MDivisionid = '{Sci.Env.User.Keyword}' AND WorkType <> '' AND Finished = 0";
            }
            else
            {
                this.Text = "P02. WorkOrder For Planning(History)";
                this.IsSupportEdit = false;
                this.DefaultFilter = $"MDivisionid = '{Sci.Env.User.Keyword}' AND WorkType <> '' AND Finished = 1";
                this.btnAutoRef.EditMode = AdvEditModes.None;
                this.btnAutoRef.Enabled = false;
                this.gridSizeRatio.ContextMenuStrip = null;
                this.gridDistributeToSP.ContextMenuStrip = null;
            }

            this.displayBoxFabricTypeRefno.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "FabricTypeRefNo", true));
            this.displayBoxDescription.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "FabricDescription", true));
            this.numUnitCons.DataBindings.Add(new Binding("Value", this.bindingSourceDetail, "ConsPC", true));
            this.numCons.DataBindings.Add(new Binding("Value", this.bindingSourceDetail, "Cons", true));
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
WHERE MDivisionID = '{Env.User.Keyword}'
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
        #endregion

        #region 大表身相關

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = $@"
SELECT
    wo.*
   ,wp1.PatternPanel_CONCAT
   ,wp2.FabricPanelCode_CONCAT
   ,Article_CONCAT
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
   ,ImportML = CAST(0 AS BIT)
   --- 排序用
   ,SORT_NUM = 0 -- 避免編輯過程資料跑來跑去
   ,multisize.multisize
   ,Order_SizeCode_Seq.Order_SizeCode_Seq
   ,CanEdit = dbo.GetCuttingP02CanEdit(wo.Ukey, wo.CutPlanID, wo.CutRef) -- 判斷此筆是否能編輯
FROM WorkOrderForPlanning wo WITH (NOLOCK)
LEFT JOIN Fabric f WITH (NOLOCK) ON f.SCIRefno = wo.SCIRefno
LEFT JOIN Construction cs WITH (NOLOCK) ON cs.ID = ConstructionID
LEFT JOIN Pass1 p1 WITH (NOLOCK) ON p1.ID = wo.AddName
LEFT JOIN Pass1 p2 WITH (NOLOCK) ON p2.ID = wo.EditName
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
    SELECT Article_CONCAT = STUFF((
        SELECT DISTINCT CONCAT('/', Article)
        FROM WorkOrderForPlanning_Distribute WITH (NOLOCK)
        WHERE WorkOrderForPlanningUkey = wo.Ukey
        AND Article != ''
        FOR XML PATH ('')), 1, 1, '')
) wd
OUTER APPLY (
    SELECT SizeCode_CONCAT = STUFF((
        SELECT CONCAT(', ', ws.SizeCode, '/ ', ws.Qty)
        FROM WorkOrderForPlanning_SizeRatio ws WITH (NOLOCK)
		OUTER APPLY(SELECT TOP 1 osc.SizeGroup,osc.Seq FROM Order_SizeCode osc WITH (NOLOCK) WHERE osc.Id = ws.ID AND osc.SizeCode = ws.SizeCode) osc
        WHERE ws.WorkOrderForPlanningUkey = wo.Ukey
		ORDER BY ws.WorkOrderForPlanningUkey,osc.SizeGroup,osc.Seq,ws.SizeCode
        FOR XML PATH ('')), 1, 1, '')
) ws1
OUTER APPLY (
    SELECT TotalCutQty_CONCAT = STUFF((
        SELECT CONCAT(', ', ws.Sizecode, '/ ', ws.Qty * wo.Layer)
        FROM WorkOrderForPlanning_SizeRatio ws WITH (NOLOCK)
		OUTER APPLY(SELECT TOP 1 osc.SizeGroup,osc.Seq FROM Order_SizeCode osc WITH (NOLOCK) WHERE osc.Id = ws.ID AND osc.SizeCode = ws.SizeCode) osc
        WHERE ws.WorkOrderForPlanningUkey = wo.Ukey
		ORDER BY ws.WorkOrderForPlanningUkey,osc.SizeGroup,osc.Seq,ws.SizeCode
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
WHERE wo.id = '{masterID}'
ORDER BY {this.detailSort}
";

            string cmdsql = $@"
Select a.MarkerName,a.ColorID,a.Order_EachconsUkey
	,layer = isnull(sum(a.layer),0)
    ,TotalLayerUkey =             
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
            this.editByUseCutRefToRequestFabric = useCutRefToRequestFabric != 0 && useCutRefToRequestFabric != 2;

            this.btnAllSPDistribute.Enabled = this.EditMode && this.editByUseCutRefToRequestFabric;
            this.btnImportMarker.Enabled = !this.Text.Contains("History") && !this.EditMode && this.editByUseCutRefToRequestFabric;
            this.btnAutoRef.Enabled = !this.EditMode;
            this.displayBoxStyle.Text = MyUtility.GetValue.Lookup($"SELECT StyleID FROM Orders WITH(NOLOCK) WHERE ID = '{this.CurrentMaintain["ID"]}'");
            this.DetailDatas.AsEnumerable().ToList().ForEach(row => row["MarkerLength_Mask"] = Prgs.ConvertFullWidthToHalfWidth(FormatMarkerLength(row["MarkerLength"].ToString()))); // _Mask 欄位 用來顯示用, 若有編輯會寫回原欄位
            this.GetAllDetailData();
            this.detailgrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.Sorting();
        }

        private void GetAllDetailData()
        {
            string cuttingID = MyUtility.Check.Empty(this.CurrentMaintain) ? string.Empty : MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);

            string sqlcmd = $@"
SELECT *, tmpKey = CAST(0 AS BIGINT) FROM WorkOrderForPlanning_PatternPanel WITH (NOLOCK) WHERE ID = '{this.CurrentMaintain["ID"]}'

SELECT
    ws.*
   ,wp.Layer
   ,TotalCutQty_CONCAT = ''
   ,tmpKey = CAST(0 AS BIGINT),osc.Seq
FROM WorkOrderForPlanning wp WITH (NOLOCK)
INNER JOIN WorkOrderForPlanning_SizeRatio ws WITH (NOLOCK) ON wp.Ukey = ws.WorkOrderForPlanningUkey
OUTER APPLY(SELECT TOP 1 osc.SizeGroup,osc.Seq FROM Order_SizeCode osc WITH (NOLOCK) WHERE osc.Id = ws.ID AND osc.SizeCode = ws.SizeCode) osc
WHERE wp.ID = '{cuttingID}'
ORDER BY ws.WorkOrderForPlanningUkey,osc.SizeGroup,osc.Seq,ws.SizeCode

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
FROM WorkOrderForPlanning_Distribute wd WITH (NOLOCK)
WHERE wd.ID = '{this.CurrentMaintain["ID"]}'
ORDER BY wd.OrderID, wd.Article, wd.SizeCode
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

            foreach (DataRow dr in this.dt_SizeRatio.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    dr["TotalCutQty_CONCAT"] = ConcatTTLCutQty(dr);
                }
            }

            // 右下角 Qty Break Down
            this.qtybreakds.DataSource = QueryQtyBreakDown(cuttingID, this.formType);

            // set Size Ratio data source
            this.sizeRatiobs.DataSource = this.dt_SizeRatio;
            this.distributebs.DataSource = this.dt_Distribute;

            this.ChangeQtyBreakDownRow();
            this.ChangeBtnColor();
        }

        private void ChangeBtnColor()
        {
            this.btnQtyBreakdown.ForeColor = MyUtility.Check.Seek(this.CurrentMaintain["ID"].ToString(), "Order_Qty", "ID") ? Color.Blue : Color.Black;
            this.btnPackingMethod.ForeColor = MyUtility.Check.Seek(this.CurrentMaintain["ID"].ToString(), "orders", "cuttingsp") ? Color.Blue : Color.Black;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("CutRef", header: "CutRef#", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true).Get(out this.col_CutRef)
                .NumericNull("Seq", "Seq", Ict.Win.Widths.AnsiChars(5), this.CanEditData)
                .Text("MarkerName", header: "Marker\r\nName", width: Ict.Win.Widths.AnsiChars(5))
                .Text("PatternPanel_CONCAT", header: "Pattern\r\nPanel", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("FabricPanelCode_CONCAT", header: "Fabric\r\nPanel Code", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Article_CONCAT", header: "Article", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ColorId", header: "Color", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Tone", header: "Tone", width: Ict.Win.Widths.AnsiChars(4)).Get(out this.col_Tone)
                .Text("SizeCode_CONCAT", header: "Size", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Layer", header: "Layers", width: Ict.Win.Widths.AnsiChars(5), integer_places: 5, maximum: 99999).Get(out this.col_Layer)
                .Text("TotalCutQty_CONCAT", header: "Total CutQty", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .WorkOrderSP("OrderId", "SP#", Ict.Win.Widths.AnsiChars(12), this.GetWorkType, this.CanEditData)
                .Text("SEQ1", header: "Seq1", width: Ict.Win.Widths.AnsiChars(3)).Get(out this.col_Seq1)
                .Text("SEQ2", header: "Seq2", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_Seq2)
                .Date("Fabeta", header: "Fabric Arr Date", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .WorkOrderWKETA("WKETA", "WK ETA", Ict.Win.Widths.AnsiChars(10), true, this.CanEditData)
                .EstCutDate("EstCutDate", "Est. Cut Date", Ict.Win.Widths.AnsiChars(10), this.CanEditData)
                .Text("CutCellID", header: "Cut Cell", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_CutCellID)
                .Text("CutPlanID", header: "Cut Plan", width: Ict.Win.Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Edituser", header: "Edit Name", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                .DateTime("EditDate", header: "Edit Date", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Adduser", header: "Add Name", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                .DateTime("AddDate", header: "Add Date", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                ;

            this.Helper.Controls.Grid.Generator(this.gridSizeRatio)
                .Text("SizeCode", header: "Size", width: Ict.Win.Widths.AnsiChars(5)).Get(out this.col_SizeRatio_Size)
                .Numeric("Qty", header: "Ratio", width: Ict.Win.Widths.AnsiChars(6), integer_places: 5, maximum: 99999, minimum: 0).Get(out this.col_SizeRatio_Qty)
                .Numeric("Layer", header: "Layers", width: Ict.Win.Widths.AnsiChars(5), integer_places: 5, maximum: 99999, iseditingreadonly: true)
                .Text("TotalCutQty_CONCAT", header: "Tlt. Qty", width: Ict.Win.Widths.AnsiChars(5), iseditingreadonly: true)
                ;
            this.Helper.Controls.Grid.Generator(this.gridDistributeToSP)
                .Text("OrderID", header: "SP#", width: Ict.Win.Widths.AnsiChars(13)).Get(out this.col_Distribute_SP)
                .Text("Article", header: "Article", width: Ict.Win.Widths.AnsiChars(6)).Get(out this.col_Distribute_Article)
                .Text("SizeCode", header: "Size", width: Ict.Win.Widths.AnsiChars(4)).Get(out this.col_Distribute_Size)
                .Numeric("Qty", header: "Qty", width: Ict.Win.Widths.AnsiChars(3), integer_places: 6, maximum: 999999, minimum: 0).Get(out this.col_Distribute_Qty)
                .Date("SewInline", header: "Inline Date", width: Ict.Win.Widths.AnsiChars(8), iseditingreadonly: true)
                ;
            this.Helper.Controls.Grid.Generator(this.gridQtyBreakDown)
                .Text("ID", header: "SP#", width: Ict.Win.Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Ict.Win.Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Ict.Win.Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("Qty", header: "Order\nQty", width: Ict.Win.Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("Balance", header: "Balance", width: Ict.Win.Widths.AnsiChars(5), iseditingreadonly: true)
                ;

            this.GridEventSet();

            // 設定所有欄位的 AutoSizeMode
            foreach (DataGridViewColumn column in this.detailgrid.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailGridRowChanged()
        {
            this.GridValidateControl();
            base.OnDetailGridRowChanged(); // 此後 CurrentDetailData 才會是新的

            this.bindingSourceDetail.SetRow(this.CurrentDetailData);

            // 變更子表可否編輯
            bool canEdit = this.CanEditData(this.CurrentDetailData);
            this.btnEdit.Enabled = canEdit;
            this.cmsSizeRatio.Enabled = canEdit;
            this.numUnitCons.ReadOnly = !canEdit;
            this.txtPatternNo.ReadOnly = !canEdit;
            this.txtMarkerLength.ReadOnly = !canEdit;
            this.gridSizeRatio.IsEditingReadOnly = !canEdit;

            bool canEditDisturbute = this.CanEditDistribute(this.CurrentDetailData);
            this.btnDistributeThisCutRef.Enabled = canEditDisturbute;
            this.cmsDistribute.Enabled = canEditDisturbute;
            this.gridDistributeToSP.IsEditingReadOnly = !canEditDisturbute;

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
                2. Order_EachconsUkey 不為空 => 使用 TotalLayerUkey

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
                this.numTotalLayer.Value = 0;
                this.numBalanceLayer.Value = 0;
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
                    decimal totalLayer = (decimal)laydr[0]["TotalLayerMarker"];

                    this.numTotalLayer.Value = totalLayer;
                    this.numBalanceLayer.Value = sumlayer - totalLayer;
                }
            }

            #endregion

            // 根據左邊Grid Filter 右邊 Size Ratio 資訊
            string filter = GetFilter(this.CurrentDetailData, this.formType);
            this.sizeRatiobs.Filter = filter;
            this.distributebs.Filter = filter;

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

        protected override void DoPrint()
        {
            this.drBeforeDoPrintDetailData = this.CurrentDetailData;
            base.DoPrint(); // 會重新載入資訊,並將this.CurrentDetailData移動到第一筆
        }

        protected override bool ClickPrint()
        {
            Cutting_Print_P02 callNextForm;
            if (this.drBeforeDoPrintDetailData != null)
            {
                callNextForm = new Cutting_Print_P02(this.drBeforeDoPrintDetailData);
                callNextForm.ShowDialog(this);
            }
            else if (this.drBeforeDoPrintDetailData == null && this.CurrentDetailData != null)
            {
                callNextForm = new Cutting_Print_P02(this.CurrentDetailData);
                callNextForm.ShowDialog(this);
            }
            else
            {
                MyUtility.Msg.InfoBox("No datas");
                return false;
            }

            return base.ClickPrint();
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

        #region Click Save Event

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            this.GridValidateControl();

            #region 檢查 主表身

            // 不可空欄位, 並移動到錯誤列
            if (!this.ValidateDetailDatas())
            {
                return false;
            }

            if (!this.ValidateMarkerNo())
            {
                return false;
            }

            if (!this.ValidateCutRef())
            {
                return false;
            }

            #endregion

            #region 清除 第3層 空值
            this.dt_SizeRatio.Select("Qty = 0 OR SizeCode = ''").Delete();
            this.dt_Distribute.Select("Qty = 0 OR OrderID = '' OR SizeCode = ''").Delete();
            this.dt_Distribute.Select("OrderID <> 'EXCESS' AND Article = ''").Delete(); // EXCESS 項 Article 為空
            #endregion

            #region 檢查 SizeRatio、PatternPanel 重複 Key

            var checkSizeRatio = new List<string> { "WorkOrderForPlanningUkey", "tmpKey", "SizeCode" }; // 檢查的 Key
            if (!CheckDuplicateAndShowMessage(this.dt_SizeRatio, checkSizeRatio, "SizeRatio", this.DetailDatas, this.formType))
            {
                return false;
            }

            var checkDistribute = new List<string> { "WorkOrderForPlanningUkey", "tmpKey", "OrderID", "Article", "SizeCode" }; // 檢查的 Key
            if (!CheckDuplicateAndShowMessage(this.dt_Distribute, checkDistribute, "Distribute", this.DetailDatas, this.formType))
            {
                return false;
            }

            var checkPatternPanel = new List<string> { "WorkOrderForPlanningUkey", "tmpKey", "PatternPanel", "FabricPanelCode" }; // 檢查的 Key
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
                    MyUtility.Msg.WarningBox($"CutRef:{dr["CutRef"]},Seq:{dr["Seq"]},MarkerName:{dr["MarkerName"]} Distribution Qty can not exceed total Cut qty");
                    return false;
                }
            }
            #endregion

            // 刪除 SizeRatio 之後重算 ConsPC
            BeforeSaveCalculateConsPC(this.DetailDatas, this.dt_SizeRatio, this.formType);

            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePost()
        {
            // Stpe 1. 給第3層填入對應 WorkOrderForPlanningUkey
            foreach (DataRow dr in this.DetailDatas.Where(row => row.RowState == DataRowState.Added))
            {
                long ukey = MyUtility.Convert.GetLong(dr["Ukey"]); // ClickSavePost 時,底層已取得 Key 值
                string filterAddData = $"tmpkey = {dr["tmpkey"]} ";
                this.dt_SizeRatio.Select(filterAddData).AsEnumerable().ToList().ForEach(row => row["WorkOrderForPlanningUkey"] = ukey);
                this.dt_Distribute.Select(filterAddData).AsEnumerable().ToList().ForEach(row => row["WorkOrderForPlanningUkey"] = ukey);
                this.dt_PatternPanel.Select(filterAddData).AsEnumerable().ToList().ForEach(row => row["WorkOrderForPlanningUkey"] = ukey);
            }

            #region 處理 SizeRatio
            string sqlDeleteSizeRatio = $@"
DELETE wd
OUTPUT DELETED.*
FROM WorkOrderForPlanning_SizeRatio wd
LEFT JOIN #tmp t ON t.WorkOrderForPlanningUkey = wd.WorkOrderForPlanningUkey AND t.SizeCode = wd.SizeCode
WHERE wd.id = '{this.CurrentMaintain["ID"]}'
AND t.WorkOrderForPlanningUkey IS NULL
";

            string sqlUpdateSizeRatio = $@"
UPDATE wd
SET wd.Qty = t.Qty
OUTPUT INSERTED.*
FROM WorkOrderForPlanning_SizeRatio wd
INNER JOIN #tmp t ON t.WorkOrderForPlanningUkey = wd.WorkOrderForPlanningUkey AND t.SizeCode = wd.SizeCode
WHERE wd.id = '{this.CurrentMaintain["ID"]}'
";

            string sqlInsertSizeRatio = $@"

INSERT INTO WorkOrderForPlanning_SizeRatio (WorkOrderForPlanningUkey, ID, SizeCode, Qty)
OUTPUT INSERTED.*
SELECT
    t.WorkOrderForPlanningUkey
    ,t.ID
    ,t.SizeCode
    ,ISNULL(t.Qty,0)
FROM #tmp t
LEFT JOIN WorkOrderForPlanning_SizeRatio wd ON t.WorkOrderForPlanningUkey = wd.WorkOrderForPlanningUkey AND t.SizeCode = wd.SizeCode
WHERE wd.WorkOrderForPlanningUkey IS NULL
";
            DualResult result;
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
FROM WorkOrderForPlanning_PatternPanel wd
LEFT JOIN #tmp t ON t.WorkOrderForPlanningUkey = wd.WorkOrderForPlanningUkey AND t.PatternPanel = wd.PatternPanel AND t.FabricPanelCode = wd.FabricPanelCode
WHERE wd.id = '{this.CurrentMaintain["ID"]}'
AND t.WorkOrderForPlanningUkey IS NULL
";
            string sqlInsertPatternPanel = $@"
INSERT INTO WorkOrderForPlanning_PatternPanel (WorkOrderForPlanningUkey, ID, PatternPanel, FabricPanelCode)
SELECT
    t.WorkOrderForPlanningUkey
    ,t.ID
    ,t.PatternPanel
    ,t.FabricPanelCode
FROM #tmp t
LEFT JOIN WorkOrderForPlanning_PatternPanel wd ON t.WorkOrderForPlanningUkey = wd.WorkOrderForPlanningUkey AND t.PatternPanel = wd.PatternPanel AND t.FabricPanelCode = wd.FabricPanelCode
WHERE wd.WorkOrderForPlanningUkey IS NULL
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

            #region 處理 Distribute

            // API需要 Distribute 新刪修 同時 out 資訊
            string sqlDeleteDistribute = $@"
DELETE wd
OUTPUT DELETED.*
FROM WorkOrderForPlanning_Distribute wd
LEFT JOIN #tmp t ON t.WorkOrderForPlanningUkey = wd.WorkOrderForPlanningUkey AND t.OrderID = wd.OrderID AND t.Article = wd.Article AND t.SizeCode = wd.SizeCode
WHERE wd.id = '{this.CurrentMaintain["ID"]}'
AND t.WorkOrderForPlanningUkey IS NULL
";
            string sqlUpdateDistribute = $@"
UPDATE wd
SET wd.Qty = t.Qty
OUTPUT INSERTED.*
FROM WorkOrderForPlanning_Distribute wd
INNER JOIN #tmp t ON t.WorkOrderForPlanningUkey = wd.WorkOrderForPlanningUkey AND t.OrderID = wd.OrderID AND t.Article = wd.Article AND t.SizeCode = wd.SizeCode
WHERE wd.id = '{this.CurrentMaintain["ID"]}'
AND wd.Qty <> t.Qty
";
            string sqlInsertDistribute = $@"
INSERT INTO WorkOrderForPlanning_Distribute (WorkOrderForPlanningUkey, ID, OrderID, Article, SizeCode, Qty)
OUTPUT INSERTED.*
SELECT
    t.WorkOrderForPlanningUkey
    ,t.ID
    ,t.OrderID
    ,ISNULL(t.Article, '')
    ,t.SizeCode
    ,t.Qty
FROM #tmp t
LEFT JOIN WorkOrderForPlanning_Distribute wd ON t.WorkOrderForPlanningUkey = wd.WorkOrderForPlanningUkey AND t.OrderID = wd.OrderID AND t.Article = wd.Article AND t.SizeCode = wd.SizeCode
WHERE wd.WorkOrderForPlanningUkey IS NULL
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

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            this.OnRefreshClick();
        }

        // 檢查 主表身 不可空欄位, 並移動到那列
        private bool ValidateDetailDatas()
        {
            var validationRules = new Dictionary<string, string>
            {
                { "MarkerNo", "Pattern No cannot be empty." },
                { "FabricPanelCode", "Fabric Panel Code cannot be empty." },
                { "MarkerName", "Marker Name cannot be empty." },
                { "Layer", "Layer cannot be empty." },
                { "SEQ1", "SEQ1 cannot be empty." },
                { "SEQ2", "SEQ2 cannot be empty." },
            };

            foreach (var rule in validationRules)
            {
                foreach (DataRow row in this.DetailDatas.Where(row => MyUtility.Check.Empty(row[rule.Key])))
                {
                    int index = this.DetailDatas.IndexOf(row);
                    this.detailgrid.SelectRowTo(index);
                    MyUtility.Msg.WarningBox(rule.Value);
                    return false;
                }
            }

            return true;
        }

        // 檢查MarkerName, CutRef 的group， MarkerNo 必須一樣
        private bool ValidateMarkerNo()
        {
            var groupData = this.DetailDatas
                .Where(x => x.RowState != DataRowState.Deleted && !MyUtility.Check.Empty(x["MarkerName"]) && !MyUtility.Check.Empty(x["CutRef"]))
                .GroupBy(x => new { MarkerName = x["MarkerName"].ToString(), CutRef = x["CutRef"].ToString() })
                .Where(
                group => group.Select(row => new { MarkerNo = row["MarkerNo"].ToString() })
                               .Distinct()
                               .Count() > 1)
                .SelectMany(group => group);

            if (groupData.Any())
            {
                DataTable dt = groupData.Select(o => new { CutRef = o["CutRef"].ToString(), MarkerName = o["MarkerName"].ToString(), MarkerNo = o["MarkerNo"].ToString() }).Distinct().LinqToDataTable();

                string msg = "For the following Cutref, MarkerName combinations, different MarkerNo values were found:";
                MsgGridForm m = new MsgGridForm(dt, msg, "Exists different MarkerNo");
                m.grid1.Columns[2].HeaderText = "Pattern No";
                m.grid1.ColumnsAutoSize();
                m.ShowDialog();
                return false;
            }

            return true;
        }

        // 檢查EstCutDate 的group， CutRef 必須一樣
        private bool ValidateCutRef()
        {
            var groupData = this.DetailDatas
                .Where(x => x.RowState != DataRowState.Deleted && !MyUtility.Check.Empty(x["EstCutDate"]) && !MyUtility.Check.Empty(x["CutRef"]))
                .GroupBy(x => new { CutRef = x["CutRef"].ToString() })
                .Where(
                group => group.Select(row => new { EstCutDate = MyUtility.Convert.GetDate(row["EstCutDate"]) })
                               .Distinct()
                               .Count() > 1)
                .SelectMany(group => group);

            if (groupData.Any())
            {
                DataTable dt = groupData.Select(o => new { CutRef = MyUtility.Convert.GetString(o["CutRef"]), EstCutDate = MyUtility.Convert.GetDate(o["EstCutDate"]).Value }).Distinct().LinqToDataTable();

                string msg = "You can't set different [Est.CutDate] in same CutRef#:";
                MsgGridForm m = new MsgGridForm(dt, msg, "Exists CutRef#");
                m.grid1.ColumnsAutoSize();
                m.ShowDialog();
                return false;
            }

            return true;
        }
        #endregion

        #region 單筆操作：包含表身三個Icon、彈窗ActionCutRef 新/修/刪

        /// <summary>
        /// Edit按鈕開啟「單筆編輯視窗」
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            #region 校驗
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("There is no Workorder data that can be modified.");
                return;
            }

            if (!this.CanEditData(this.CurrentDetailData))
            {
                MyUtility.Msg.WarningBox("There is cut plan ,so can not be modified.");
                return;
            }
            #endregion

            // 單筆編輯視窗
            this.ShowDialogActionCutRef(DialogAction.Edit);
            this.OnDetailGridRowChanged();
        }

        /// <summary>
        /// 單筆編輯視窗
        /// </summary>
        /// <param name="action">DialogAction</param>
        /// <returns>DialogResult</returns>
        private DialogResult ShowDialogActionCutRef(DialogAction action)
        {
            var form = new P02_ActionCutRef();
            form.Action = action;
            form.CurrentMaintain = this.CurrentMaintain;
            form.CurrentDetailData_Ori = this.CurrentDetailData;
            form.dt_SizeRatio_Ori = this.dt_SizeRatio;
            form.dt_Distribute_Ori = this.dt_Distribute;
            form.dt_PatternPanel_Ori = this.dt_PatternPanel;

            // 複製開窗使用的資料, 取消時不用還原
            DataTable detailDatas = this.CurrentDetailData.Table.Copy();
            form.CurrentDetailData = detailDatas.Select($"Ukey = {this.CurrentDetailData["Ukey"]} AND tmpKey = {this.CurrentDetailData["tmpKey"]}")[0];
            form.dt_SizeRatio = this.dt_SizeRatio.Copy();
            form.dt_Distribute = this.dt_Distribute.Copy();
            form.dt_PatternPanel = this.dt_PatternPanel.Copy();
            return form.ShowDialog();
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
            this.CurrentDetailData["Adduser"] = MyUtility.GetValue.Lookup($"SELECT NAME FROM Pass1 WITH (NOLOCK) Where ID = '{this.CurrentDetailData["AddName"]}'");
            this.CurrentDetailData["CanEdit"] = true;
            this.CurrentDetailData["Seq"] = DBNull.Value;
            if (oldRow == null)
            {
                // 按 + 或 插入, 無表身時, 第一筆只能從 Cutting 欄位帶入
                this.CurrentDetailData["FactoryID"] = this.CurrentMaintain["FactoryID"];
                this.CurrentDetailData["MDivisionId"] = this.CurrentMaintain["MDivisionId"];

                // WorkType = 2 才會有SP#
                if (index == -1 && this.CurrentMaintain["WorkType"].ToString() == "2")
                {
                    this.CurrentDetailData["OrderID"] = this.CurrentMaintain["ID"];
                }
            }
            else
            {
                // 其餘從現有Row帶入
                this.CurrentDetailData["FactoryID"] = oldRow["FactoryID"];
                this.CurrentDetailData["MDivisionId"] = oldRow["MDivisionId"];
                this.CurrentDetailData["MarkerNo"] = oldRow["MarkerNo"];
                this.CurrentDetailData["SORT_NUM"] = oldRow["SORT_NUM"];

                // WorkType = 2 才會有SP#
                if (index == -1 && this.CurrentMaintain["WorkType"].ToString() == "2")
                {
                    this.CurrentDetailData["OrderID"] = oldRow["OrderID"];
                }
            }

            // 按+號 = -1, 其它 = 按插入, 複製原先停留row的部分欄位資訊
            if (index != -1)
            {
                // 不複製 CutRef, Seq, 以及(這4個底層會自動處理 Addname, AddDate, EditName, EditDate)
                // 定義不需要複製的欄位名稱列表
                HashSet<string> excludeColumns = new HashSet<string>
                {
                    "Ukey", // 此表 Pkey 底層處理
                    "tmpKey", // 上方有填不同值不複製
                    "ID", // 對應 Cutting 的 Key, 在 base.OnDetailGridInsert 會自動寫入
                    "Seq",
                    "CutRef",
                    "CutPlanID",
                    "CanEdit",
                    "Addname",
                    "AddDate",
                    "EditName",
                    "EditDate",
                    "EditDate",
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

                // 複製第3層資訊,並對應到新的 this.CurrentMaintain
                AddThirdDatas(this.CurrentDetailData, oldRow, this.dt_SizeRatio, this.formType);
                AddThirdDatas(this.CurrentDetailData, oldRow, this.dt_Distribute, this.formType);
                AddThirdDatas(this.CurrentDetailData, oldRow, this.dt_PatternPanel, this.formType);

                if (MyUtility.Convert.GetInt(this.CurrentMaintain["UseCutRefToRequestFabric"]) == 2)
                {
                    var p09_AutoDistToSP = new P09_AutoDistToSP(this.CurrentDetailData, this.dt_SizeRatio, this.dt_Distribute, this.dt_PatternPanel, this.formType);
                    DualResult resultDis = p09_AutoDistToSP.DoAutoDistribute();
                    if (!resultDis)
                    {
                        this.ShowErr(resultDis);
                        return;
                    }
                }
            }

            DialogResult result = this.ShowDialogActionCutRef(DialogAction.Create);
            if (result == DialogResult.Cancel)
            {
                this.OnDetailGridDelete();
            }

            this.OnDetailGridRowChanged();
        }

        protected override void OnDetailGridDelete()
        {
            if (this.CurrentDetailData == null)
            {
                return;
            }

            // 選擇多筆刪除時,先檢查是否有不可刪除的資料
            foreach (DataGridViewRow item in this.detailgrid.SelectedRows)
            {
                if (item.DataBoundItem is DataRowView dataRowView)
                {
                    DataRow currentDetailData = dataRowView.Row;
                    if (!MyUtility.Check.Empty(currentDetailData) && MyUtility.Convert.GetInt(currentDetailData["Ukey"]) != 0 && !this.CanEditData(currentDetailData))
                    {
                        MyUtility.Msg.WarningBox("There is cut plan ,so can not be modified.");
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

            base.OnDetailGridDelete(); // 刪除表身
        }
        #endregion

        #region Grid 欄位事件 顏色/開窗/驗證

        /// <summary>
        /// // Grid Cell 物件設定
        /// </summary>
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

                // 沒有排入裁剪計畫的裁次才可異動
                if (this.CanEditData(dr))
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

                // 沒有排入裁剪計畫的裁次才可異動
                if (this.CanEditData(dr))
                {
                    if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                    {
                        e.EditingControl.Text = string.Empty;
                    }
                }
            };

            // Seq 兩個欄位
            ConfigureSeqColumnEvents(this.col_Seq1, this.detailgrid, this.CanEditData);
            ConfigureSeqColumnEvents(this.col_Seq2, this.detailgrid, this.CanEditData);

            // Cut Cell
            BindGridCutCell(this.col_CutCellID, this.detailgrid, this.CanEditData);

            this.col_Layer.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                P02_ValidatingLayers(this.CurrentMaintain, this.CurrentDetailData, MyUtility.Convert.GetInt(e.FormattedValue), this.dt_SizeRatio, this.dt_Distribute, this.dt_PatternPanel, this.formType);
            };

            this.col_Tone.MaxLength = 15;
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
                    DataRow dr = this.gridSizeRatio.GetDataRow(e.RowIndex);
                    dr["TotalCutQty_CONCAT"] = ConcatTTLCutQty(dr);
                    dr.EndEdit();
                    UpdateConcatString(this.CurrentDetailData, this.dt_SizeRatio, this.formType);
                }
            };

            this.col_SizeRatio_Qty.CellValidating += (s, e) =>
            {
                P02_SizeRationQtyValidating(this.gridSizeRatio, e, this.CurrentMaintain, this.CurrentDetailData, this.dt_SizeRatio, this.dt_Distribute, this.dt_PatternPanel, this.formType);
            };
            #endregion

            #region Distribute
            this.BindDistributeEvents(this.col_Distribute_SP);
            this.BindDistributeEvents(this.col_Distribute_Article);
            this.BindDistributeEvents(this.col_Distribute_Size);
            this.BindQtyEvents(this.col_Distribute_Qty);
            #endregion
        }

        private void BindDistributeEvents(Ict.Win.UI.DataGridViewTextBoxColumn column)
        {
            column.EditingMouseDown += (s, e) =>
            {
                if (Distribute3CellEditingMouseDown(e, this.CurrentDetailData, this.dt_SizeRatio, this.gridDistributeToSP, this.formType, MyUtility.Convert.GetInt(this.CurrentDetailData["Layer"])))
                {
                    UpdateMinSewinline(this.CurrentDetailData, this.dt_Distribute, this.formType);
                    string columnName = ((DataGridViewElement)s).DataGridView.Columns[e.ColumnIndex].Name.ToLower();
                    UpdateMinOrderID(this.CurrentMaintain["WorkType"].ToString(), this.CurrentDetailData, this.dt_Distribute, this.formType);
                    UpdateArticle_CONCAT(this.CurrentDetailData, this.dt_Distribute, this.formType);
                }
            };
            column.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData != null && Distribute3CellValidating(e, this.CurrentDetailData, this.dt_SizeRatio, this.gridDistributeToSP, this.formType, MyUtility.Convert.GetInt(this.CurrentDetailData["Layer"])))
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

                    this.CurrentDetailData["ConsPC"] = CalculateConsPC(this.CurrentDetailData, MyUtility.Convert.GetDecimal(this.CurrentDetailData["Cons"]), MyUtility.Convert.GetDecimal(this.CurrentDetailData["Layer"]), this.dt_SizeRatio, this.formType);
                }
            };
        }

        private void BindQtyEvents(Ict.Win.UI.DataGridViewNumericBoxColumn column)
        {
            column.CellValidating += (s, e) =>
            {
                Grid grid = (Grid)((DataGridViewColumn)s).DataGridView;
                if (this.CurrentDetailData != null && QtyCellValidating(e, this.CurrentDetailData, grid, this.dt_SizeRatio, this.dt_Distribute, this.formType, MyUtility.Convert.GetInt(this.CurrentDetailData["Layer"])))
                {
                    UpdateConcatString(this.CurrentDetailData, this.dt_SizeRatio, this.formType);
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

            DataRow ndr = this.dt_SizeRatio.NewRow();
            ndr["ID"] = this.CurrentMaintain["ID"];
            ndr["tmpKey"] = this.CurrentDetailData["tmpKey"];
            ndr["WorkOrderForPlanningUkey"] = this.CurrentDetailData["Ukey"];
            ndr["Qty"] = 0;
            ndr["SizeCode"] = string.Empty;

            int layer = MyUtility.Convert.GetInt(this.CurrentDetailData["Layer"]);
            ndr["Layer"] = layer;

            ndr["TotalCutQty_CONCAT"] = string.Empty;
            this.dt_SizeRatio.Rows.Add(ndr);
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
            newrow["WorkOrderForPlanningUkey"] = this.CurrentDetailData["Ukey"];
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
            if (!this.CanEditDistribute(this.CurrentDetailData) || selectRow.Count == 0)
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

        #region Button Event

        private void BtnQtyBreakdown_Click(object sender, EventArgs e)
        {
            MyUtility.Check.Seek($@"select isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList from Orders o WITH (NOLOCK) where ID = '{this.CurrentMaintain["ID"]}'", out DataRow dr);
            PPIC.P01_Qty callNextForm = new PPIC.P01_Qty(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), dr["PoList"].ToString());
            callNextForm.ShowDialog(this);
        }

        private void BtnAutoRef_Click(object sender, EventArgs e)
        {
            this.OnRefreshClick();
            AutoCutRef(this.CurrentMaintain["ID"].ToString(), Sci.Env.User.Keyword, (DataTable)this.detailgridbs.DataSource, this.formType);
            this.OnRefreshClick();
        }

        private void BtnBatchAssign_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();
            var detailDatas = this.DetailDatas.Where(row => this.CanEditData(row)).ToList();
            if (!detailDatas.Any())
            {
                MyUtility.Msg.InfoBox("No editable data.");
                return;
            }

            var frm = new Cutting_BatchAssign(detailDatas, this.CurrentMaintain["ID"].ToString(), this.formType);
            frm.ShowDialog(this);
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

        private void BtnImportMarkerLectra_Click(object sender, EventArgs e)
        {
            // P02似乎不需要
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
                drNEW["WorkOrderForPlanningUkey"] = 0;  // 新增 WorkOrderForPlanningUkey 塞0
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

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            CuttingWorkOrder cuttingWorkOrder = new CuttingWorkOrder();
            string errMsg;
            if (!cuttingWorkOrder.DownloadFile(this.formType, this.CurrentMaintain, out errMsg))
            {
                MyUtility.Msg.ErrorBox(errMsg);
            }
        }

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

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            this.detailgrid.ToExcel(false);
        }
        #endregion

        private bool CanEditDataByGrid(Grid grid, DataRow dr, string columNname)
        {
            try
            {
                if (this.CurrentDetailData == null)
                {
                    return false;
                }

                switch (grid.Name)
                {
                    case "detailgrid":
                        if (columNname.ToLower() == "orderid" && this.CurrentMaintain["WorkType"].ToString() != "2")
                        {
                            return false;
                        }

                        return this.CanEditData(dr);
                    case "gridDistributeToSP":
                        if (dr["OrderID"].ToString().EqualString("EXCESS"))
                        {
                            return false;
                        }

                        return this.CanEditDistribute(this.CurrentDetailData);
                    default:
                        return this.CanEditData(this.CurrentDetailData);
                }
            }
            catch (Exception)
            {
                // 不接錯誤是因為 新增一筆 後 undo 時,底層取 CurrentDetailData index 跳錯, 不是 null
                throw;
            }
        }

        private bool CanEditDistribute(DataRow dr)
        {
            if (MyUtility.Check.Empty(dr))
            {
                return false;
            }

            bool canEdit = this.CanEditData(dr);
            return canEdit && this.editByUseCutRefToRequestFabric;
        }

        private bool CanEditData(DataRow dr)
        {
            if (MyUtility.Check.Empty(dr))
            {
                return false;
            }

            // 沒有排入裁剪計畫 && 不存在 SpreadingSchedule_Detail 的裁次才可異動
            return this.EditMode && MyUtility.Convert.GetBool(dr["CanEdit"]);
        }

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

        private void NumUnitCons_Validated(object sender, EventArgs e)
        {
            this.CurrentDetailData["Cons"] = CalculateCons(this.CurrentDetailData, MyUtility.Convert.GetDecimal(this.CurrentDetailData["ConsPC"]), MyUtility.Convert.GetDecimal(this.CurrentDetailData["Layer"]), this.dt_SizeRatio, this.formType);
        }

        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.OnRefreshClick();
        }

        private void P02_SizeChanged(object sender, EventArgs e)
        {
            Font f = new Font(this.btnBatchAssign.Font.FontFamily, 9);
            if (this.Width > 1252)
            {
                f = new Font(this.btnBatchAssign.Font.FontFamily, 10);
            }

            this.btnBatchAssign.Font = f;
            this.btnImportMarker.Font = f;
            this.btnExcludeSetting.Font = f;
            this.btnDownload.Font = f;
            this.btnImportMarkerLectra.Font = f;
            this.btnEdit.Font = f;
            this.btnCutPartsCheck.Font = f;
            this.btnCutPartsCheckSummary.Font = f;
            this.btnQtyBreakdown.Font = f;
            this.btnToExcel.Font = f;
            this.refresh.Font = f;
            this.btnPackingMethod.Font = f;
        }

        private void BtnAutoSeq_Click(object sender, EventArgs e)
        {
            this.gridSizeRatio.ValidateControl();
            this.detailgrid.ValidateControl();
            int maxSeq;

            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["Seq"]) && !MyUtility.Check.Empty(dr["estcutdate"]))
                {
                    DataTable wk = (DataTable)this.detailgridbs.DataSource;
                    string temp = wk.Compute("Max(Seq)", string.Format("(PatternPanel_CONCAT ='{0}' or ('{0}'in ('FA+FC','FC+FA') and PatternPanel_CONCAT in ('FA+FC','FC+FA')))", dr["PatternPanel_CONCAT"])).ToString();
                    if (MyUtility.Check.Empty(temp))
                    {
                        maxSeq = 1;
                    }
                    else
                    {
                        int maxno = Convert.ToInt32(wk.Compute("Max(Seq)", string.Format("(PatternPanel_CONCAT ='{0}' or ('{0}'in ('FA+FC','FC+FA') and PatternPanel_CONCAT in ('FA+FC','FC+FA')))", dr["PatternPanel_CONCAT"])).ToString());
                        maxSeq = maxno + 1;
                    }

                    dr["Seq"] = maxSeq;
                }
            }
        }

        private void BtnPackingMethod_Click(object sender, EventArgs e)
        {
            this.gridSizeRatio.ValidateControl();
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
            decimal totlaLayer = MyUtility.Convert.GetInt(this.numTotalLayer.Value) + MyUtility.Convert.GetDecimal(this.numBalanceLayer.Value);
            this.numUnitCons.Value = CalculateConsPC(this.txtMarkerLength.FullText, this.CurrentDetailData, this.dt_SizeRatio, this.formType);
            this.numCons.Value = CalculateCons(this.CurrentDetailData, MyUtility.Convert.GetDecimal(this.numUnitCons.Value), totlaLayer, this.dt_SizeRatio, this.formType);
        }

        private void BtnExcludeSetting_Click(object sender, EventArgs e)
        {
            // 當 P091 History, IsSupportEdit = false
            var exwip = new P02_ExcludeSetting(this.CurrentMaintain["ID"].ToString(), this.IsSupportEdit);
            exwip.ShowDialog();
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
    }
#pragma warning restore SA1600 // Elements should be documented
}
