using Ict;
using Ict.Win;
using Ict.Win.UI;
using Sci.Andy;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
#pragma warning disable SA1401 // Fields should be private
#pragma warning disable SA1600 // Elements should be documented
    /// <inheritdoc/>
    public partial class P02 : Win.Tems.Input6
    {
        private readonly Win.UI.BindingSource2 bindingSourceDetail = new Win.UI.BindingSource2();
        private Ict.Win.UI.DataGridViewTextBoxColumn col_CutRef;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_SP;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Article;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq1;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq2;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Tone;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_WKETA;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_EstCutDate;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_MarkerNo;
        private Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_MarkerLength;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_SizeRatio_Size;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_SizeRatio_Qty;

        public static DialogAction dialogAction;
        private DataTable dt_Layers;
        private DataTable dt_SizeRatio;
        private DataTable dt_OrderList;
        private DataTable dt_PatternPanel;

        private DataRow drTEMP;  // 紀錄目前表身選擇的資料，避免按列印時模組會重LOAD資料，導致永遠只能印到第一筆資料

        /// <inheritdoc/>
        public P02(ToolStripMenuItem menuitem, string history)
            : base(menuitem)
        {
            this.InitializeComponent();

            if (history == "0")
            {
                this.Text = "P02. WorkOrder For Planning";
                this.IsSupportEdit = true;
            }
            else
            {
                this.Text = "P02. WorkOrder For Planning(History)";
                this.IsSupportEdit = false;
            }

            this.disFabricTypeRefno.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "FabricTypeRefNo", true));
            this.disDescription.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "FabricDescription", true));
            this.numUnitCons.DataBindings.Add(new Binding("Value", this.bindingSourceDetail, "ConsPC", true));
            this.numCons.DataBindings.Add(new Binding("Value", this.bindingSourceDetail, "Cons", true));

            this.detailgrid.Click += this.Detailgrid_Click;
        }

        private void Detailgrid_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgrid.CurrentCell))
            {
                return;
            }

            // 游標直接進入 Cell, 才不用點兩下
            this.detailgrid.CurrentCell = this.detailgrid[this.detailgrid.CurrentCell.ColumnIndex, this.detailgrid.CurrentCell.RowIndex];
            this.detailgrid.BeginEdit(true);
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
    ,ImportML = cast(0 as bit)
   ,tmpKey = CAST(0 AS BIGINT) --控制新加的資料用,SizeRatio/PatternPanel
   ,EachconsMarkerNo = oe.markerNo
   ,EachconsMarkerVersion = oe.MarkerVersion
   ,HasBundle = CAST(IIF(EXISTS(SELECT 1 FROM Bundle WHERE CutRef <> '' AND CutRef = wo.CutRef), 1, 0) AS BIT)
   ,HasCuttingOutput = CAST(IIF(EXISTS(SELECT 1 FROM CuttingOutput_Detail WHERE CutRef <> '' AND CutRef = wo.CutRef), 1, 0) AS BIT)
   ,HasMarkerReq = CAST(IIF(EXISTS(SELECT 1 FROM MarkerReq_Detail WHERE OrderID = wo.ID), 1, 0) AS BIT)
FROM WorkOrderForPlanning wo WITH (NOLOCK)
LEFT JOIN Fabric f WITH (NOLOCK) ON f.SCIRefno = wo.SCIRefno
LEFT JOIN Construction cs WITH (NOLOCK) ON cs.ID = ConstructionID
LEFT JOIN Order_Eachcons oe WITH (NOLOCK) ON oe.Ukey = wo.Order_EachconsUkey
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
    SELECT val = IIF(psd.Complete = 1, psd.FinalETA, IIF(psd.Eta IS NOT NULL, psd.eta, IIF(psd.shipeta IS NOT NULL, psd.shipeta, psd.finaletd)))
    FROM PO_Supp_Detail psd WITH (NOLOCK)
    WHERE EXISTS (SELECT 1 FROM Orders WITH (NOLOCK) WHERE CuttingSp = wo.ID AND POID = psd.id)
    AND psd.seq1 = wo.seq1
    AND psd.seq2 = wo.seq2
) FabricArrDate
WHERE wo.id = '{masterID}'
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
            DualResult r = DBProxy.Current.Select(null, cmdsql, out this.dt_Layers);
            if (!r)
            {
                this.ShowErr(cmdsql, r);
            }

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.displayBoxStyle.Text = MyUtility.GetValue.Lookup($"SELECT StyleID FROM Orders WITH(NOLOCK) WHERE ID = '{this.CurrentMaintain["ID"]}'");

            this.btnImportMarker.Enabled = this.CurrentMaintain["WorkType"].ToString() == "1" && !this.EditMode;

            // Grid的Marker Length需要格式化後再貼上Grid cell
            foreach (DataRow row in this.DetailDatas)
            {
                row["MarkerLength_Mask"] = FormatMarkerLength(row["MarkerLength"].ToString());
            }

            this.GetAllDetailData();

            #region Fabric Type/ RefNo、Description
            if (this.DetailDatas.Any())
            {
                var d = this.DetailDatas.Select(o => o["FabricTypeRefNo"].ToString()).Distinct().ToList();
                this.disFabricTypeRefno.Text = string.Join(",", d);
            }

            if (this.DetailDatas.Any())
            {
                var d = this.DetailDatas.Select(o => o["FabricDescription"].ToString()).Distinct().ToList();
                this.disDescription.Text = string.Join(",", d);
            }
            #endregion

        }

        private void GetAllDetailData()
        {
            string cuttingID = MyUtility.Check.Empty(this.CurrentMaintain) ? string.Empty : MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);

            string sqlcmd = $@"
SELECT *, tmpKey = CAST(0 AS BIGINT) FROM WorkOrderForPlanning_PatternPanel WHERE ID = '{this.CurrentMaintain["ID"]}'

select b.*
,a.Layer
,TotalCutQty_CONCAT = ''
,tmpKey = CAST(0 AS BIGINT)
from WorkOrderForPlanning a  WITH (NOLOCK)
inner join WorkOrderForPlanning_SizeRatio b  WITH (NOLOCK) on a.Ukey = b.WorkOrderForPlanningUkey
where  a.id = '{cuttingID}'

SELECT InlineDate = convert(date, Min(SewingSchedule.Inline), 111)
	,SP = SewingSchedule.OrderID
	,Qty = SUM(SewingSchedule.AlloQty)
FROM SewingSchedule WITH (nolock)
INNER JOIN Orders ON SewingSchedule.OrderID=Orders.ID
WHERE  Orders.POID = '{cuttingID}'
group by SewingSchedule.OrderID
order by convert(date, Min(SewingSchedule.Inline), 111) asc
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable[] dts);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.dt_PatternPanel = dts[0];
            this.dt_SizeRatio = dts[1];
            this.dt_OrderList = dts[2];

            foreach (DataRow dr in this.dt_SizeRatio.Rows)
            {
                dr["TotalCutQty_CONCAT"] = this.Cal_CurrentCutQty(dr["workOrderForPlanningUkey"], dr["tmpKey"], dr["SizeCode"].ToString());
            }

            // set Size Ratio data source
            this.sizeRatioBindingSource.DataSource = this.dt_SizeRatio;

            // set Order List data source
            this.orderListBindingSource.DataSource = this.dt_OrderList;

            // 右下角 Qty Break Down待整合...
            sqlcmd = $@"
/*
SELECT
    wd.OrderID
   ,wd.Article
   ,wd.SizeCode
   ,wo.FabricCombo
   ,Qty = SUM(wd.Qty)
INTO #tmp
FROM WorkOrderForPlanning wo WITH (NOLOCK)
INNER JOIN WorkOrderForPlanning_Distribute wd WITH (NOLOCK) ON wo.ukey = wd.WorkOrderForPlanningukey
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
*/
";
            //result = DBProxy.Current.Select(null, sqlcmd, out DataTable dtQtyBreakDown);
            //if (!result)
            //{
            //    this.ShowErr(result);
            //    return;
            //}

            //this.qtybreakds.DataSource = dtQtyBreakDown;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            DataGridViewGeneratorDateColumnSettings wKETA = new DataGridViewGeneratorDateColumnSettings();
            wKETA.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    P02_WKETA item = new P02_WKETA(dr);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    if (result == DialogResult.No)
                    {
                        dr["WKETA"] = DBNull.Value;
                    }

                    if (result == DialogResult.Yes)
                    {
                        dr["WKETA"] = Itemx.WKETA;
                    }

                    dr.EndEdit();
                }
            };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("CutRef", header: "CutRef#", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true).Get(out this.col_CutRef)
                .Text("MarkerName", header: "Marker\r\nName", width: Ict.Win.Widths.AnsiChars(5))
                .Text("PatternPanel_CONCAT", header: "Pattern Panel", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("FabricPanelCode_CONCAT", header: "Fabric\r\nPanel Code", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("OrderId", header: "SP#", width: Ict.Win.Widths.AnsiChars(13)).Get(out this.col_SP)
                .Text("SEQ1", header: "Seq1", width: Ict.Win.Widths.AnsiChars(3)).Get(out this.col_Seq1)
                .Text("SEQ2", header: "Seq2", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_Seq2)
                .Text("Article", header: "Article", width: Ict.Win.Widths.AnsiChars(10)).Get(out this.col_Article)
                .Text("ColorId", header: "Color", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Tone", header: "Tone", width: Ict.Win.Widths.AnsiChars(4)).Get(out this.col_Tone)
                .Text("SizeCode_CONCAT", header: "Size", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Layer", header: "Layers", width: Ict.Win.Widths.AnsiChars(5), integer_places: 5, maximum: 99999)
                .Text("TotalCutQty_CONCAT", header: "Total CutQty", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("WKETA", header: "WK ETA", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true).Get(out this.col_WKETA)
                .Date("Fabeta", header: "Fabric Arr Date", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("EstCutDate", header: "Est. Cut Date", width: Ict.Win.Widths.AnsiChars(10)).Get(out this.col_EstCutDate)
                .Text("CutPlanID", header: "Cut Plan", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .MaskedText("MarkerLength_Mask", "00Y00-0/0+0\"", "Marker Length", name: "Marker Length", width: Ict.Win.Widths.AnsiChars(16)).Get(out this.col_MarkerLength)
                .Text("MarkerNo", header: "Pattern No", width: Ict.Win.Widths.AnsiChars(10)).Get(out this.col_MarkerNo)
                .Text("Adduser", header: "Add Name", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("AddDate", header: "Add Date", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Edituser", header: "Edit Name", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("EditDate", header: "Edit Date", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                ;

            this.Helper.Controls.Grid.Generator(this.gridSizeRatio)
                .Text("SizeCode", header: "Size", width: Ict.Win.Widths.AnsiChars(5)).Get(out this.col_SizeRatio_Size)
                .Numeric("Qty", header: "Ratio", width: Ict.Win.Widths.AnsiChars(5), integer_places: 6, maximum: 999999, minimum: 0).Get(out this.col_SizeRatio_Qty)
                .Numeric("Layer", header: "Layers", width: Ict.Win.Widths.AnsiChars(5), integer_places: 5, maximum: 99999)
                .Text("TotalCutQty_CONCAT", header: "Tlt. Qty", width: Ict.Win.Widths.AnsiChars(5))
            ;

            this.Helper.Controls.Grid.Generator(this.gridOrderList)
                .Date("InlineDate", header: "Inline Date", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SP", header: "SP#", width: Ict.Win.Widths.AnsiChars(13), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Ict.Win.Widths.AnsiChars(5), iseditingreadonly: true)
            ;
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

            DataRow[] laydr;
            if (MyUtility.Check.Empty(this.CurrentDetailData["Order_EachConsUkey"]))
            {
                laydr = this.dt_Layers.Select($"MarkerName = '{this.CurrentDetailData["MarkerName"]}' and ColorID = '{this.CurrentDetailData["Colorid"]}'");
            }
            else
            {
                laydr = this.dt_Layers.Select($"Order_EachConsUkey = '{this.CurrentDetailData["Order_EachConsUkey"]}' and ColorID = '{this.CurrentDetailData["Colorid"]}'");
            }

            if (!laydr.Any())
            {
                this.numTotalLayer.Value = 0;
                this.numBalanceLayer.Value = 0;
            }
            else
            {
                decimal totalLayer = MyUtility.Check.Empty(this.CurrentDetailData["Order_EachConsUkey"])
                    ? (decimal)laydr[0]["TotalLayerMarker"]
                    : (decimal)laydr[0]["TotalLayerUkey"];

                this.numTotalLayer.Value = totalLayer;
                this.numBalanceLayer.Value = sumlayer - totalLayer;
            }

            #endregion

            // 變更子表可否編輯
            bool canEdit = this.CanEditData(this.CurrentDetailData);
            this.sizeratioMenuStrip.Enabled = canEdit;
            this.gridSizeRatio.IsEditingReadOnly = !canEdit;

            // 根據左邊Grid Filter 右邊 Size Ratio 資訊
            string filter = GetFilter(this.CurrentDetailData, CuttingForm.P02);
            this.sizeRatioBindingSource.Filter = filter;

            // 根據左邊Grid Filter 右邊 Order List 資訊
            this.orderListBindingSource.Filter = $@"SP = '{this.CurrentDetailData["OrderID"]}' ";
        }

        protected override void DoPrint()
        {
            // 1394: CUTTING_P02_Cutting Work Order。KEEP當前的資料。
            this.drTEMP = this.CurrentDetailData;
            base.DoPrint();
        }

        protected override bool ClickPrint()
        {
            Cutting_Print callNextForm;
            if (this.drTEMP != null)
            {
                callNextForm = new Cutting_Print(CuttingForm.P02, this.drTEMP);
                callNextForm.ShowDialog(this);
            }
            else if (this.drTEMP == null && this.CurrentDetailData != null)
            {
                callNextForm = new Cutting_Print(CuttingForm.P02, this.CurrentDetailData);
                callNextForm.ShowDialog(this);
            }
            else
            {
                MyUtility.Msg.InfoBox("No datas");
                return false;
            }

            return base.ClickPrint();
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();

            if (this.btnImportMarkerLectra != null)
            {
                this.btnImportMarkerLectra.Enabled = this.EditMode;
            }
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

            if (!this.ValidateMarkerNameAndNo())
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
            #endregion

            #region 檢查 SizeRatio、PatternPanel 重複 Key

            var checkSizeRatio = new List<string> { "WorkOrderForPlanningUkey", "tmpKey", "SizeCode" }; // 檢查的 Key
            if (!CheckDuplicateAndShowMessage(this.dt_SizeRatio, checkSizeRatio, "SizeRatio", this.DetailDatas, CuttingForm.P02))
            {
                return false;
            }

            var checkPatternPanel = new List<string> { "WorkOrderForPlanningUkey", "tmpKey", "PatternPanel", "FabricPanelCode" }; // 檢查的 Key
            if (!CheckDuplicateAndShowMessage(this.dt_PatternPanel, checkPatternPanel, "PatternPanel", this.DetailDatas, CuttingForm.P09))
            {
                return false;
            }
            #endregion

            // 計算CutInLine、CutOffLine
            this.CurrentMaintain["CutForPlanningInline"] = ((DataTable)this.detailgridbs.DataSource).Compute("Min(EstCutDate)", null);
            this.CurrentMaintain["CutForPlanningOffLine"] = ((DataTable)this.detailgridbs.DataSource).Compute("Max(EstCutDate)", null);

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

            #region 回寫Orders CutInLine,CutOffLine

            StringBuilder updatesql = new StringBuilder();
            string cutInLine, cutOffLine;

            cutInLine = ((DataTable)this.detailgridbs.DataSource).Compute("Min(EstCutDate)", null) == DBNull.Value ? string.Empty : Convert.ToDateTime(((DataTable)this.detailgridbs.DataSource).Compute("Min(EstCutDate)", null)).ToString("yyyy-MM-dd HH:mm:ss");

            cutOffLine = ((DataTable)this.detailgridbs.DataSource).Compute("Max(EstCutDate)", null) == DBNull.Value ? string.Empty : Convert.ToDateTime(((DataTable)this.detailgridbs.DataSource).Compute("Max(EstCutDate)", null)).ToString("yyyy-MM-dd HH:mm:ss");

            updatesql.AppendLine($@"UPDATE Orders set CutInLine = iif('{cutInLine}' = '',null,'{cutInLine}'),CutOffLine =  iif('{cutOffLine}' = '',null,'{cutOffLine}') where POID = '{this.CurrentMaintain["ID"]}';");

            DualResult upResult;

            if (!MyUtility.Check.Empty(updatesql.ToString()))
            {
                if (!(upResult = DBProxy.Current.Execute(null, updatesql.ToString())))
                {
                    return upResult;
                }
            }
            #endregion

            return base.ClickSavePost();
        }

        // 檢查 主表身 不可空欄位, 並移動到那列
        private bool ValidateDetailDatas()
        {
            var validationRules = new Dictionary<string, string>
            {
                { "MarkerNo", "Marker No cannot be empty." },
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

        // 檢查MarkerName, MarkerNo 的group， Markerlength、EstCutDate 必須一樣
        private bool ValidateMarkerNameAndNo()
        {
            var groupData = this.DetailDatas
                .Where(x => x.RowState != DataRowState.Deleted && !MyUtility.Check.Empty(x["MarkerName"]) && !MyUtility.Check.Empty(x["MarkerNo"]))
                .GroupBy(x => new { MarkerName = x["MarkerName"].ToString(), MarkerNo = x["MarkerNo"].ToString() })
                .Where(
                group => group.Select(row => new { Markerlength = row["Markerlength"].ToString(), EstCutDate = MyUtility.Convert.GetDate(row["EstCutDate"]) })
                               .Distinct()
                               .Count() > 1)
                .SelectMany(group => group);

            if (groupData.Any())
            {
                DataTable dt = groupData.Select(o => new { MarkerName = o["MarkerName"].ToString(), MarkerNo = o["MarkerNo"].ToString(), MarkerLength = o["Markerlength"].ToString() }).Distinct().LinqToDataTable();

                string msg = "The following MarkerName, MarkerNo combinations have different Markerlength or EstCutDate:";
                MsgGridForm m = new MsgGridForm(dt, msg, "Exists different Markerlength or EstCutDate");
                m.grid1.Columns[1].HeaderText = "Pattern No";
                m.grid1.ColumnsAutoSize();
                m.ShowDialog();
                return false;
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

        // 表身若沒有第三層 SizeRatio 資料則移除
        private void DeleteNonSizeRatio()
        {
            this.DetailDatas.Where(x => x.RowState != DataRowState.Deleted &&
                !this.dt_SizeRatio.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted
                    && o["tmpKey"].ToString() == x["tmpKey"].ToString()
                    && o["WorkOrderForPlanningUkey"].ToString() == x["Ukey"].ToString())
                .Any()).Delete();
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
            form.CurrentDetailData = this.CurrentDetailData;
            form.CurrentMaintain = this.CurrentMaintain;
            form.dtWorkOrderForPlanning_SizeRatio_Ori = this.dt_SizeRatio;
            form.dtWorkOrderForPlanning_PatternPanel_Ori = this.dt_PatternPanel;
            form.dtWorkOrderForPlanning_OrderList_Ori = this.dt_OrderList;

            string filter = GetFilter(this.CurrentDetailData, CuttingForm.P02);

            form.dtWorkOrderForPlanning_SizeRatio = this.dt_SizeRatio.Select(filter).TryCopyToDataTable(this.dt_SizeRatio);
            form.dtWorkOrderForPlanning_PatternPanel = this.dt_PatternPanel.Select(filter).TryCopyToDataTable(this.dt_PatternPanel);
            form.dtWorkOrderForPlanning_OrderList = this.dt_OrderList;
            return form.ShowDialog();
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
            this.CurrentDetailData["Adduser"] = MyUtility.GetValue.Lookup($"SELECT NAME FROM Pass1 WITH (NOLOCK) Where ID = '{this.CurrentDetailData["AddName"]}'");
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

                // WorkType = 2 才會有SP#
                if (index == -1 && this.CurrentMaintain["WorkType"].ToString() == "2")
                {
                    this.CurrentDetailData["OrderID"] = oldRow["OrderID"];
                }
            }

            // 按+號 = -1, 其它 = 按插入, 複製原先停留row的部分欄位資訊
            if (index != -1)
            {
                // 不複製 CutRef, CutNo, 以及(這4個底層會自動處理 Addname, AddDate, EditName, EditDate)
                // 定義不需要複製的欄位名稱列表
                HashSet<string> excludeColumns = new HashSet<string>
                {
                    "Ukey", // 此表 Pkey 底層處理
                    "tmpKey", // 上方有填不同值不複製
                    "ID", // 對應 Cutting 的 Key, 在 base.OnDetailGridInsert 會自動寫入
                    "CutRef",
                    "CutNo",
                    "CutPlanID",
                    "Addname",
                    "AddDate",
                    "EditName",
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
                AddThirdDatas(this.CurrentDetailData, oldRow, this.dt_SizeRatio, CuttingForm.P02);
                AddThirdDatas(this.CurrentDetailData, oldRow, this.dt_PatternPanel, CuttingForm.P02);
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

            if (!MyUtility.Check.Empty(this.CurrentDetailData) && MyUtility.Convert.GetInt(this.CurrentDetailData["Ukey"]) != 0 && !this.CanEditData(this.CurrentDetailData))
            {
                MyUtility.Msg.WarningBox("There is cut plan ,so can not be modified.");
                return;
            }

            this.dt_SizeRatio.Select(GetFilter(this.CurrentDetailData, CuttingForm.P02)).Delete();
            this.dt_PatternPanel.Select(GetFilter(this.CurrentDetailData, CuttingForm.P02)).Delete();
            base.OnDetailGridDelete();
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

            this.col_SP.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                SpEditingMouseDown(e, this, this.detailgrid, this.CurrentMaintain["ID"].ToString(), MyUtility.Convert.GetString(this.CurrentMaintain["WorkType"]));
            };
            this.col_SP.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                if (!SpCellValidating(s, e, this, this.detailgrid, this.CurrentMaintain["ID"].ToString(), MyUtility.Convert.GetString(this.CurrentMaintain["WorkType"])))
                {
                    e.Cancel = true;
                }
            };

            this.col_Article.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                ArticleEditingMouseDown(s, e, this, this.detailgrid, this.CurrentMaintain["ID"].ToString(), MyUtility.Convert.GetString(this.CurrentMaintain["WorkType"]));
            };
            this.col_Article.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                if (!ArticleCellValidating(s, e, this, this.detailgrid, this.CurrentMaintain["ID"].ToString(), MyUtility.Convert.GetString(this.CurrentMaintain["WorkType"])))
                {
                    e.Cancel = true;
                }
            };

            this.col_Seq1.EditingMouseDown += this.SeqCellEditingMouseDown;
            this.col_Seq2.EditingMouseDown += this.SeqCellEditingMouseDown;
            this.col_Seq1.CellValidating += this.SeqCelllValidatingHandler;
            this.col_Seq2.CellValidating += this.SeqCelllValidatingHandler;

            this.col_Tone.MaxLength = 15;

            this.col_WKETA.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

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
            };
            this.col_WKETA.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };

            this.col_MarkerNo.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    SelectItem selectItem = PopupMarkerNo(this.CurrentMaintain["ID"].ToString(), dr["MarkerNo"].ToString());
                    if (selectItem == null)
                    {
                        return;
                    }

                    dr["MarkerNo"] = selectItem.GetSelectedString();
                    dr.EndEdit();
                }
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
                    return;
                }

                dr["MarkerNo"] = e.FormattedValue;
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
                dr.EndEdit();
            };
            this.col_EstCutDate.CellValidating += (s, e) =>
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
            #endregion

            #region SizeRatio
            this.col_SizeRatio_Size.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.RowIndex < 0)
                    {
                        return;
                    }

                    DataRow dr = this.gridSizeRatio.GetDataRow(e.RowIndex);

                    if (!this.CanEditData(this.CurrentDetailData))
                    {
                        return;
                    }

                    if (SizeCodeCellEditingMouseDown(e, this.gridSizeRatio, this.CurrentDetailData, null, CuttingForm.P02))
                    {
                        dr["TotalCutQty_CONCAT"] = this.Cal_CurrentCutQty(dr["WorkOrderForPlanningUkey"], dr["tmpKey"], dr["SizeCode"].ToString());
                        dr.EndEdit();
                        UpdateConcatString(this.CurrentDetailData, this.dt_SizeRatio, CuttingForm.P02);
                    }
                }
            };
            this.col_SizeRatio_Size.CellValidating += (s, e) =>
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                DataRow dr = this.gridSizeRatio.GetDataRow(e.RowIndex);

                if (!this.CanEditData(this.CurrentDetailData))
                {
                    return;
                }

                if (SizeCodeCellValidating(e, this.gridSizeRatio, this.CurrentDetailData, null, CuttingForm.P02))
                {
                    dr["TotalCutQty_CONCAT"] = this.Cal_CurrentCutQty(dr["WorkOrderForPlanningUkey"], dr["tmpKey"], dr["SizeCode"].ToString());
                    dr.EndEdit();
                    UpdateConcatString(this.CurrentDetailData, this.dt_SizeRatio, CuttingForm.P02);
                }
            };

            this.BindQtyEvents(this.col_SizeRatio_Qty);
            #endregion
        }

        private void CustomCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // 粉底紅字
            DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
            if (this.CanEditData(dr))
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

        private void SeqCellEditingMouseDown(object sender, Ict.Win.UI.DataGridViewEditingControlMouseEventArgs e)
        {
            DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
            if (!this.CanEditData(dr))
            {
                return;
            }

            DataRow minFabricPanelCode = GetMinFabricPanelCode(dr, this.dt_PatternPanel, CuttingForm.P02);
            if (minFabricPanelCode == null)
            {
                MyUtility.Msg.WarningBox("Please select Pattern Panel first!");
                return;
            }

            string columnName = this.detailgrid.Columns[e.ColumnIndex].Name;
            string id = this.CurrentMaintain["ID"].ToString();
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
            if (!this.CanEditData(dr))
            {
                return;
            }

            DataRow minFabricPanelCode = GetMinFabricPanelCode(dr, this.dt_PatternPanel, CuttingForm.P02);
            if (minFabricPanelCode == null)
            {
                MyUtility.Msg.WarningBox("Please select Pattern Panel first!");
                return;
            }

            string columnName = this.detailgrid.Columns[e.ColumnIndex].Name;
            string newvalue = e.FormattedValue.ToString();
            string oldvalue = this.CurrentDetailData[columnName].ToString();
            if (newvalue.IsNullOrWhiteSpace() || newvalue == oldvalue)
            {
                return;
            }

            string id = this.CurrentMaintain["ID"].ToString();
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

        private void BindQtyEvents(Ict.Win.UI.DataGridViewNumericBoxColumn column)
        {
            column.CellValidating += (s, e) =>
            {
                Sci.Win.UI.Grid grid = (Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                DataRow dr = this.gridSizeRatio.GetDataRow(e.RowIndex);
                if (QtyCellValidating(e, this.CurrentDetailData, grid, this.dt_SizeRatio, null, CuttingForm.P02))
                {
                    dr["TotalCutQty_CONCAT"] = this.Cal_CurrentCutQty(dr["WorkOrderForPlanningUkey"], dr["tmpKey"], dr["SizeCode"].ToString());
                    dr.EndEdit();
                    UpdateConcatString(this.CurrentDetailData, this.dt_SizeRatio, CuttingForm.P02);
                }
            };
        }
        #endregion

        #region Grid 右鍵 Menu

        private void InsertSizeRatioToolStripMenuItem_Click(object sender, EventArgs e)
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

            int layer = MyUtility.Convert.GetInt(this.CurrentDetailData["Layer"]);
            ndr["Layer"] = layer;

            ndr["TotalCutQty_CONCAT"] = string.Empty;
            this.dt_SizeRatio.Rows.Add(ndr);
        }

        private void DeleteSizeRatioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectRow = this.gridSizeRatio.GetSelecteds(SelectedSort.Index);
            if (!this.CanEditData(this.CurrentDetailData) || selectRow.Count == 0)
            {
                return;
            }

            DataRow dr = ((DataRowView)selectRow[0]).Row;

            // 刪除 SizeRatio
            dr.Delete();
        }
        #endregion

        #region 右邊Size Ratio表格相關(舊方法，先保留，改用共用)

        /// <summary>
        /// SizeRatio 表格個欄位計算Total Cut Qty，Layer使用自己身上的就好
        /// </summary>
        /// <param name="workOrderForPlanningUkey">workOrderForPlanningUkey</param>
        /// <param name="tmpKey">tmpKey</param>
        /// <param name="sizeCode">sizeCode</param>
        /// <returns>cutQtystr</returns>
        private string Cal_CurrentCutQty(object workOrderForPlanningUkey, object tmpKey, string sizeCode)
        {
            DataRow dr = this.dt_SizeRatio.Select(
                string.Format(
                    "WorkOrderForPlanningUkey={0} and tmpKey = {1} and SizeCode = '{2}'",
                    MyUtility.Convert.GetInt(workOrderForPlanningUkey),
                    MyUtility.Convert.GetLong(tmpKey),
                    sizeCode)).FirstOrDefault();

            string cutQtystr = dr["SizeCode"].ToString().Trim() + "/" + (Convert.ToDecimal(dr["Qty"]) * Convert.ToDecimal(MyUtility.Check.Empty(dr["Layer"]) ? 0 : dr["Layer"])).ToString();

            return cutQtystr;
        }

        private void Cal_TotalCutQty(object workOrderForPlanningUkey, object tmpKey)
        {
            string totalCutQtystr;
            totalCutQtystr = string.Empty;
            DataRow[] sizeview = this.dt_SizeRatio.Select(string.Format("WorkOrderForPlanningUkey={0} and tmpKey = {1} ", Convert.ToInt32(workOrderForPlanningUkey), Convert.ToInt64(tmpKey)));

            foreach (DataRow dr in sizeview)
            {
                if (totalCutQtystr == string.Empty)
                {
                    totalCutQtystr = totalCutQtystr + dr["SizeCode"].ToString().Trim() + "/" + (Convert.ToDecimal(dr["Qty"]) * Convert.ToDecimal(MyUtility.Check.Empty(this.CurrentDetailData["Layer"]) ? 0 : this.CurrentDetailData["Layer"])).ToString();
                }
                else
                {
                    totalCutQtystr = totalCutQtystr + "," + dr["SizeCode"].ToString().Trim() + "/" + (Convert.ToDecimal(dr["Qty"]) * Convert.ToDecimal(MyUtility.Check.Empty(this.CurrentDetailData["Layer"]) ? 0 : this.CurrentDetailData["Layer"])).ToString();
                }
            }

            this.CurrentDetailData["CutQty"] = totalCutQtystr;
        }

        #endregion

        #region Button Event

        // 等待整合...
        private void BtnQtyBreakdown_Click(object sender, EventArgs e)
        {
            MyUtility.Check.Seek($@"select isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList from Orders o WITH (NOLOCK) where ID = '{this.CurrentMaintain["ID"]}'", out DataRow dr);
            PPIC.P01_Qty callNextForm = new PPIC.P01_Qty(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), dr["PoList"].ToString());
            callNextForm.ShowDialog(this);
        }

        // 等待整合...
        private void BtnAutoRef_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();
            #region 變更先將同d,Cutref, FabricPanelCode, CutNo, MarkerName, estcutdate 且有cutref,Cuno無cutplanid 的cutref值找出來Group by→cutref 會相同
            string cmdsql = $@"
SELECT isnull(Cutref,'') as cutref, isnull(FabricCombo,'') as FabricCombo, CutNo,
isnull(MarkerName,'') as MarkerName, estcutdate
FROM Workorder WITH (NOLOCK) 
WHERE (cutplanid is null or cutplanid ='') AND (CutNo is not null )
AND (cutref is not null and cutref !='') and id = '{this.CurrentMaintain["ID"]}' and mDivisionid = '{Env.User.Keyword}'
GROUP BY Cutref, FabricCombo, CutNo, MarkerName, estcutdate
";
            DualResult cutrefresult = DBProxy.Current.Select(null, cmdsql, out DataTable cutreftb);
            if (!cutrefresult)
            {
                this.ShowErr(cmdsql, cutrefresult);
                return;
            }
            #endregion

            // 找出空的cutref
            cmdsql = $@"
Select * 
From workorder WITH (NOLOCK) 
Where (CutNo is not null ) and (cutref is null or cutref ='') 
and (estcutdate is not null and estcutdate !='' )
and (CutCellid is not null and CutCellid !='' )
and id = '{this.CurrentMaintain["ID"]}' and mDivisionid = '{Env.User.Keyword}'
order by FabricCombo,cutno
";
            cutrefresult = DBProxy.Current.Select(null, cmdsql, out DataTable workordertmp);
            if (!cutrefresult)
            {
                this.ShowErr(cmdsql, cutrefresult);
                return;
            }

            string maxref = MyUtility.GetValue.Lookup("Select isnull(Max(cutref),'000000') from Workorder WITH (NOLOCK)"); // 找最大Cutref
            if (MyUtility.Check.Empty(maxref))
            {
                maxref = "000000";
            }

            string updatecutref = @"
Create table #tmpWorkorder
	(
		Ukey bigint
	)
DECLARE @chk tinyint
SET @chk = 0
Begin Transaction [Trans_Name] -- Trans_Name 
";

            // 寫入空的Cutref
            foreach (DataRow dr in workordertmp.Rows)
            {
                DataRow[] findrow = cutreftb.Select(string.Format(@"MarkerName = '{0}' and FabricCombo = '{1}' and Cutno = {2} and estcutdate = '{3}' ", dr["MarkerName"], dr["FabricCombo"], dr["Cutno"], dr["estcutdate"]));
                string newcutref;

                // 若有找到同馬克同部位同Cutno同裁剪日就寫入同cutref
                if (findrow.Length != 0)
                {
                    newcutref = findrow[0]["cutref"].ToString();
                }
                else
                {
                    maxref = MyUtility.GetValue.GetNextValue(maxref, 0);
                    DataRow newdr = cutreftb.NewRow();
                    newdr["MarkerName"] = dr["MarkerName"] ?? string.Empty;
                    newdr["FabricCombo"] = dr["FabricCombo"] ?? string.Empty;
                    newdr["Cutno"] = dr["Cutno"];
                    newdr["estcutdate"] = dr["estcutdate"] ?? string.Empty;
                    newdr["cutref"] = maxref;
                    cutreftb.Rows.Add(newdr);
                    newcutref = maxref;
                }

                updatecutref += string.Format($@"
    if (select COUNT(1) from Workorder WITH (NOLOCK) where cutref = '{newcutref}' and id != '{this.CurrentMaintain["id"]}')>0
	begin
		RAISERROR ('Duplicate Cutref. Please redo Auto Ref#',12, 1) 
		Rollback Transaction [Trans_Name] -- 復原所有操作所造成的變更
	end
    Update Workorder set cutref = '{newcutref}' 
    output	INSERTED.Ukey
	into #tmpWorkorder
    where ukey = '{dr["ukey"]}';");
            }

            updatecutref += @"
    IF @@Error <> 0 BEGIN SET @chk = 1 END
IF @chk <> 0 BEGIN -- 若是新增資料發生錯誤
    Rollback Transaction [Trans_Name] -- 復原所有操作所造成的變更
END
ELSE BEGIN
    select w.* 
    from #tmpWorkorder tw
    inner join WorkOrder w with (nolock) on tw.Ukey = w.Ukey

    Commit Transaction [Trans_Name] -- 提交所有操作所造成的變更
END";

            DualResult upResult;
            DataTable dtWorkorder = new DataTable();
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                if (!(upResult = DBProxy.Current.Select(null, updatecutref, out dtWorkorder)))
                {
                    if (upResult.ToString().Contains("Duplicate Cutref. Please redo Auto Ref#"))
                    {
                        transactionscope.Dispose();
                        MyUtility.Msg.WarningBox("Duplicate Cutref. Please redo Auto Ref#");
                    }
                    else
                    {
                        transactionscope.Dispose();
                        this.ShowErr(upResult);
                    }
                }
                else
                {
                    transactionscope.Complete();
                    if (dtWorkorder.Rows.Count > 0)
                    {
                        //Task.Run(() => new Guozi_AGV().SentWorkOrderToAGV(dtWorkorder));
                    }
                }
            }

            this.RenewData();
            this.OnDetailEntered();
        }

        private void BtnBatchAssign_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();
            var dt = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(o => MyUtility.Check.Empty(o["CutPlanID"]));

            if (dt.Any())
            {
                var frm = new P02_BatchAssign(dt.CopyToDataTable(), this.CurrentMaintain["ID"].ToString());
                frm.ShowDialog(this);
            }
            else
            {
                MyUtility.Msg.InfoBox("No editable data.");
            }

        }

        // 等待整合...
        private void BtnImportMarker_Click(object sender, EventArgs e)
        {
            CuttingWorkOrder cw = new CuttingWorkOrder();
            DualResult result = cw.ImportMarkerExcel(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), Sci.Env.User.Keyword, Sci.Env.User.Factory, CuttingForm.P02);
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

        // 等待整合...
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
                var form = new ImportML(styleUkey, id, drSMNotice, (DataTable)this.detailgridbs.DataSource);
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
                drNEW["WorkOrderUkey"] = 0;  // 新增WorkOrderUkey塞0
                drNEW["PatternPanel"] = row["PatternPanel"];
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

        // 等待整合...
        private void BtnDownload_Click(object sender, EventArgs e)
        {
            CuttingWorkOrder cuttingWorkOrder = new CuttingWorkOrder();
            string errMsg;
            if (!cuttingWorkOrder.DownloadSampleFile(CuttingForm.P02, out errMsg))
            {
                MyUtility.Msg.ErrorBox(errMsg);
            }
        }

        // 等待整合...
        private void BtnCutPartsCheck_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null || this.DetailDatas.Count == 0)
            {
                return;
            }

            DataTable dtDetail = this.DetailDatas.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).CopyToDataTable();

            var frm = new Cutpartcheck(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain["WorkType"].ToString(), "WorkOrderForPlanning", dtDetail, null, this.dt_PatternPanel, this.dt_SizeRatio);
            frm.ShowDialog(this);
        }

        // 等待整合...
        private void BtnCutPartsCheckSummary_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null || this.DetailDatas.Count == 0)
            {
                return;
            }

            DataTable dtDetail = this.DetailDatas.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).CopyToDataTable();

            var frm = new Cutpartchecksummary(this.CurrentMaintain["ID"].ToString(), "WorkOrderForPlanning", dtDetail, null, this.dt_SizeRatio);
            frm.ShowDialog(this);
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            this.detailgrid.ToExcel(false);
        }
        #endregion

        private bool CanEditDataByGrid(Sci.Win.UI.Grid grid, DataRow dr, string columNname)
        {
            if (grid.Name == "detailgrid")
            {
                if (columNname.ToLower() == "OrderID" && this.CurrentMaintain["WorkType"].ToString() != "2")
                {
                    return false;
                }

                return this.CanEditData(dr);
            }
            else
            {
                return this.CanEditData(this.CurrentDetailData);
            }
        }

        private bool CanEditData(DataRow dr)
        {
            if (MyUtility.Check.Empty(dr))
            {
                return this.EditMode;
            }

            // 沒有排入裁剪計畫的裁次才可異動
            return this.EditMode && MyUtility.Check.Empty(dr["CutPlanID"]);
        }

        private void GridValidateControl()
        {
            this.detailgrid.ValidateControl();
            this.gridSizeRatio.ValidateControl();
        }
    }
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore SA1600 // Elements should be documented
}
