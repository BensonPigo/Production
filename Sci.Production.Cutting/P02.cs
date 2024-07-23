using Ict;
using Ict.Win;
using Ict.Win.UI;
using Sci.Andy;
using Sci.Data;
using Sci.Production.Class.Command;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
#pragma warning disable SA1600 // Elements should be documented
    /// <inheritdoc/>
    public partial class P02 : Win.Tems.Input6
    {
        #region 全域變數
        private readonly Win.UI.BindingSource2 bindingSourceDetail = new Win.UI.BindingSource2();
        private Ict.Win.UI.DataGridViewTextBoxColumn col_CutRef;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Article;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq1;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq2;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Tone;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Layer;
        private Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_MarkerLength;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_SizeRatio_Size;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_SizeRatio_Qty;
        private DataTable dt_SizeRatio; // 第3層表:新刪修
        private DataTable dt_PatternPanel; // 第3層表:新刪修
        private DataTable dt_Layers;
        private DataTable dt_OrderList;
        private DataRow drBeforeDoPrintDetailData;  // 紀錄目前表身選擇的資料，因為base.DoPrint() 時會LOAD資料,並將this.CurrentDetailData移動到第一筆
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
                this.IsSupportEdit = true;
                this.btnAutoRef.Enabled = true;
                this.insertSizeRatioToolStripMenuItem.Enabled = true;
                this.deleteSizeRatioToolStripMenuItem.Enabled = true;
            }
            else
            {
                this.Text = "P02. WorkOrder For Planning(History)";
                this.DefaultFilter = $"MDivisionid = '{Sci.Env.User.Keyword}' AND WorkType <> '' AND Finished = 1";
                this.IsSupportEdit = false;
                this.btnAutoRef.Enabled = false;
                this.insertSizeRatioToolStripMenuItem.Enabled = false;
                this.deleteSizeRatioToolStripMenuItem.Enabled = false;
            }

            this.displayBoxFabricTypeRefno.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "FabricTypeRefNo", true));
            this.displayBoxDescription.DataBindings.Add(new Binding("Text", this.bindingSourceDetail, "FabricDescription", true));
            this.numUnitCons.DataBindings.Add(new Binding("Value", this.bindingSourceDetail, "ConsPC", true));
            this.numCons.DataBindings.Add(new Binding("Value", this.bindingSourceDetail, "Cons", true));

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
WHERE wo.id = '{masterID}'
ORDER BY SORT_NUM, PatternPanel_CONCAT, multisize DESC, Article, Order_SizeCode_Seq DESC, MarkerName, Ukey
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
            this.BtnImportMarkerEnabled();
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
   ,tmpKey = CAST(0 AS BIGINT)
FROM WorkOrderForPlanning wp WITH (NOLOCK)
INNER JOIN WorkOrderForPlanning_SizeRatio ws WITH (NOLOCK) ON wp.Ukey = ws.WorkOrderForPlanningUkey
WHERE wp.ID = '{cuttingID}'

---- Date有值的話，然後根據Date ASC、OrderID ASC排序
---- Date沒有值的話排最後面，然後根據OrderID ASC排序
SELECT
    InlineDate = CONVERT(DATE, MIN(s.Inline), 111)
   ,SP = o.ID
   ,Qty = SUM(ISNULL(o.Qty,0) + ISNULL(o.FOC,0))
FROM Orders o WITH (NOLOCK)
LEFT JOIN SewingSchedule s WITH (NOLOCK) ON s.OrderID = o.ID
WHERE o.POID = '{cuttingID}'
GROUP BY o.ID
ORDER BY 
    CASE 
        WHEN CONVERT(DATE, MIN(s.Inline), 111) IS NULL THEN 1 
        ELSE 0 
    END, 
    InlineDate ASC,
    SP ASC;
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
                dr["TotalCutQty_CONCAT"] = this.ConcatTTLCutQty(dr);
            }

            // set Size Ratio data source
            this.sizeRatiobs.DataSource = this.dt_SizeRatio;

            // set Order List data source
            this.orderListBindingSource.DataSource = this.dt_OrderList;
            this.ChangeBtnColor();
        }

        private void ChangeBtnColor()
        {
            this.btnQtyBreakdown.ForeColor = MyUtility.Check.Seek(this.CurrentMaintain["ID"].ToString(), "Order_Qty", "ID") ? Color.Blue : Color.Black;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("CutRef", header: "CutRef#", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true).Get(out this.col_CutRef)
                .Text("MarkerName", header: "Marker\r\nName", width: Ict.Win.Widths.AnsiChars(5))
                .Text("PatternPanel_CONCAT", header: "Pattern\r\nPanel", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("FabricPanelCode_CONCAT", header: "Fabric\r\nPanel Code", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .WorkOrderSP("OrderId", "SP#", Ict.Win.Widths.AnsiChars(12), this.GetWorkType, this.CanEditData)
                .Text("SEQ1", header: "Seq1", width: Ict.Win.Widths.AnsiChars(3)).Get(out this.col_Seq1)
                .Text("SEQ2", header: "Seq2", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_Seq2)
                .Text("Article", header: "Article", width: Ict.Win.Widths.AnsiChars(10)).Get(out this.col_Article)
                .Text("ColorId", header: "Color", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Tone", header: "Tone", width: Ict.Win.Widths.AnsiChars(4)).Get(out this.col_Tone)
                .Text("SizeCode_CONCAT", header: "Size", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Layer", header: "Layers", width: Ict.Win.Widths.AnsiChars(5), integer_places: 5, maximum: 99999).Get(out this.col_Layer)
                .Text("TotalCutQty_CONCAT", header: "Total CutQty", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .WorkOrderWKETA("WKETA", "WK ETA", Ict.Win.Widths.AnsiChars(10), true, this.CanEditData)
                .Date("Fabeta", header: "Fabric Arr Date", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .EstCutDate("EstCutDate", "Est. Cut Date", Ict.Win.Widths.AnsiChars(10), this.CanEditData)
                .Text("CutPlanID", header: "Cut Plan", width: Ict.Win.Widths.AnsiChars(13), iseditingreadonly: true)
                .MarkerLength("MarkerLength_Mask", "Marker Length", "MarkerLength", Ict.Win.Widths.AnsiChars(13), this.CanEditData).Get(out this.col_MarkerLength)
                .MarkerNo("MarkerNo", "Pattern No.", Ict.Win.Widths.AnsiChars(12), this.CanEditData)
                .Text("Adduser", header: "Add Name", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                .DateTime("AddDate", header: "Add Date", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Edituser", header: "Edit Name", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                .DateTime("EditDate", header: "Edit Date", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                ;

            this.Helper.Controls.Grid.Generator(this.gridSizeRatio)
                .Text("SizeCode", header: "Size", width: Ict.Win.Widths.AnsiChars(5)).Get(out this.col_SizeRatio_Size)
                .Numeric("Qty", header: "Ratio", width: Ict.Win.Widths.AnsiChars(6), integer_places: 5, maximum: 99999, minimum: 0).Get(out this.col_SizeRatio_Qty)
                .Numeric("Layer", header: "Layers", width: Ict.Win.Widths.AnsiChars(5), integer_places: 5, maximum: 99999, iseditingreadonly: true)
                .Text("TotalCutQty_CONCAT", header: "Tlt. Qty", width: Ict.Win.Widths.AnsiChars(5), iseditingreadonly: true)
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
            base.OnDetailGridRowChanged(); // 此後 CurrentDetailData 才會是新的

            this.bindingSourceDetail.SetRow(this.CurrentDetailData);

            // 變更子表可否編輯
            bool canEdit = this.CanEditData(this.CurrentDetailData);
            this.cmsSizeRatio.Enabled = canEdit;
            this.numUnitCons.ReadOnly = !canEdit;
            this.gridSizeRatio.IsEditingReadOnly = !canEdit;

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

            // 根據左邊Grid Filter 右邊 Size Ratio 資訊
            string filter = GetFilter(this.CurrentDetailData, CuttingForm.P02);
            this.sizeRatiobs.Filter = filter;

            // 根據左邊Grid Filter 右邊 Order List 資訊
            //this.orderListBindingSource.Filter = $@"SP = '{this.CurrentDetailData["OrderID"]}' ";
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

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            this.BtnImportMarkerEnabled();
        }

        private void BtnImportMarkerEnabled()
        {
            if (this.btnImportMarker != null)
            {
                this.btnImportMarker.Enabled = !this.Text.Contains("History") && this.GetWorkType() == "1" && !this.EditMode;
            }
        }

        private void Sorting()
        {
            this.detailgrid.ValidateControl();
            if (this.CurrentDetailData == null)
            {
                return;
            }

            DataView dv = ((DataTable)this.detailgridbs.DataSource).DefaultView;
            dv.Sort = "SORT_NUM,PatternPanel_CONCAT,multisize DESC,Article,Order_SizeCode_Seq DESC,MarkerName,Ukey";
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();

            // 編輯時，將[SORT_NUM]賦予流水號
            int serial = 1;
            ((DataTable)this.detailgridbs.DataSource).ExtNotDeletedRowsForeach(row => row["SORT_NUM"] = serial++);
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
            if (!CheckDuplicateAndShowMessage(this.dt_PatternPanel, checkPatternPanel, "PatternPanel", this.DetailDatas, CuttingForm.P02))
            {
                return false;
            }
            #endregion

            // 刪除 SizeRatio 之後重算 ConsPC
            BeforeSaveCalculateConsPC(this.DetailDatas, this.dt_SizeRatio, CuttingForm.P02);

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
                    group => group.Select(row => new
                    {
                        Markerlength = Prgs.ConvertFullWidthToHalfWidth(row["Markerlength"].ToString()),
                        EstCutDate = MyUtility.Convert.GetDate(row["EstCutDate"]),
                    }).Distinct().Count() > 1)
                .SelectMany(group => group);

            if (groupData.Any())
            {
                DataTable dt = groupData.Select(o => new
                {
                    MarkerName = o["MarkerName"].ToString(),
                    MarkerNo = o["MarkerNo"].ToString(),
                    MarkerLength = o["Markerlength"].ToString(),
                    EstCutDate = MyUtility.Convert.GetDate(o["EstCutDate"]).HasValue ? MyUtility.Convert.GetDate(o["EstCutDate"]).Value.ToString("yyyy/MM/dd") : string.Empty,
                }).Distinct().LinqToDataTable();

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
            form.dtWorkOrderForPlanning_OrderList = this.dt_OrderList;
            form.dtWorkOrderForPlanning_SizeRatio_Ori = this.dt_SizeRatio;
            form.dtWorkOrderForPlanning_PatternPanel_Ori = this.dt_PatternPanel;
            string filter = GetFilter(this.CurrentDetailData, CuttingForm.P02);
            form.dtWorkOrderForPlanning_SizeRatio = this.dt_SizeRatio.Select(filter).TryCopyToDataTable(this.dt_SizeRatio);
            form.dtWorkOrderForPlanning_PatternPanel = this.dt_PatternPanel.Select(filter).TryCopyToDataTable(this.dt_PatternPanel);
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

                // 更新右邊的Size Ratio Grid
                var workOrderForPlanningUkey = MyUtility.Convert.GetLong(dr["Ukey"]);
                var tmpKey = MyUtility.Convert.GetLong(dr["tmpKey"]);

                this.dt_SizeRatio.AsEnumerable()
                .Where(o => MyUtility.Convert.GetLong(o["WorkOrderForPlanningUkey"]) == workOrderForPlanningUkey && MyUtility.Convert.GetLong(o["tmpKey"]) == tmpKey)
                .ToList()
                .ForEach(o =>
                {
                    o["Layer"] = newvalue;
                    o["TotalCutQty_CONCAT"] = this.ConcatTTLCutQty(o);
                });

                dr["Cons"] = CalculateCons(dr, MyUtility.Convert.GetDecimal(dr["ConsPC"]), MyUtility.Convert.GetDecimal(dr["Layer"]), this.dt_SizeRatio, CuttingForm.P02);
                UpdateConcatString(dr, this.dt_SizeRatio, CuttingForm.P02);
                dr.EndEdit();
            };

            this.col_MarkerLength.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                dr["ConsPC"] = CalculateConsPC(dr["MarkerLength"].ToString(), dr, this.dt_SizeRatio, CuttingForm.P02);
                dr["Cons"] = CalculateCons(dr, MyUtility.Convert.GetDecimal(dr["ConsPC"]), MyUtility.Convert.GetDecimal(dr["Layer"]), this.dt_SizeRatio, CuttingForm.P02);
                dr.EndEdit();
            };
            this.col_Tone.MaxLength = 15;
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
                        dr["TotalCutQty_CONCAT"] = this.ConcatTTLCutQty(dr);
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
                    dr["TotalCutQty_CONCAT"] = this.ConcatTTLCutQty(dr);
                    dr.EndEdit();
                    UpdateConcatString(this.CurrentDetailData, this.dt_SizeRatio, CuttingForm.P02);
                }
            };

            this.BindQtyEvents(this.col_SizeRatio_Qty);
            #endregion
        }

        private void BindQtyEvents(Ict.Win.UI.DataGridViewNumericBoxColumn column)
        {
            column.CellValidating += (s, e) =>
            {
                Sci.Win.UI.Grid grid = (Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView;

                if (this.CurrentDetailData != null && QtyCellValidating(e, this.CurrentDetailData, grid, this.dt_SizeRatio, null, CuttingForm.P02, MyUtility.Convert.GetInt(this.CurrentDetailData["Layer"])))
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    dr["TotalCutQty_CONCAT"] = this.ConcatTTLCutQty(dr);
                    dr.EndEdit();
                    UpdateConcatString(this.CurrentDetailData, this.dt_SizeRatio, CuttingForm.P02);
                    if (grid.Name == "gridSizeRatio")
                    {
                        this.CurrentDetailData["ConsPC"] = CalculateConsPC(this.CurrentDetailData, MyUtility.Convert.GetDecimal(this.CurrentDetailData["Cons"]), MyUtility.Convert.GetDecimal(this.CurrentDetailData["Layer"]), this.dt_SizeRatio, CuttingForm.P02);
                    }
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
        /// <returns>TotalCutQty_CONCAT</returns>
        private string ConcatTTLCutQty(DataRow dr)
        {
            int layerQty = MyUtility.Convert.GetInt(dr["Layer"]) * MyUtility.Convert.GetInt(dr["Qty"]);
            return $"{dr["SizeCode"]}/{layerQty}";
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
            AutoCutRef(this.CurrentMaintain["ID"].ToString(), Sci.Env.User.Keyword, (DataTable)this.detailgridbs.DataSource, CuttingForm.P02);
            this.OnRefreshClick();
            this.Sorting();  // 避免順序亂掉
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

            var frm = new Cutting_BatchAssign(detailDatas, this.CurrentMaintain["ID"].ToString(), CuttingForm.P02);
            frm.ShowDialog(this);
        }

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
                var form = new ImportML(CuttingForm.P02, styleUkey, id, drSMNotice, (DataTable)this.detailgridbs.DataSource);
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
            if (!cuttingWorkOrder.DownloadSampleFile(CuttingForm.P02, out errMsg))
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

            var frm = new Cutpartcheck(CuttingForm.P02, this.CurrentMaintain["ID"].ToString(), this.DetailDatas, this.dt_SizeRatio);
            frm.ShowDialog(this);
        }

        private void BtnCutPartsCheckSummary_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            var frm = new Cutpartchecksummary(CuttingForm.P02, this.CurrentMaintain["ID"].ToString(), this.DetailDatas, this.dt_SizeRatio);
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
                if (columNname.ToLower() == "orderid" && this.CurrentMaintain["WorkType"].ToString() != "2")
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
                return false;
            }

            // 沒有排入裁剪計畫的裁次才可異動
            return this.EditMode && MyUtility.Check.Empty(dr["CutPlanID"]);
        }

        private void GridValidateControl()
        {
            // 若 Cell 還在編輯中, 即游標還在 Cell 中, 進行此 Cell 驗證事件, 並結束編輯
            // 例如 P09:在最大值的 Cutno:5 按下 Backspace 此 cell 還沒結束編輯, 直接點 Auto CutNo 功能, 下一筆會編碼成 6, 且原本 5 這筆會是空白, 所以要先結束 Cell 編輯狀態
            this.detailgrid.ValidateControl();
            this.gridSizeRatio.ValidateControl();
        }

        // 用在傳入 Column 使用, 因為 gridset 是一開啟就會跑完, 直接傳 WorkType 字串, 換筆資料就不會變動了
        private string GetWorkType()
        {
            return MyUtility.Convert.GetString(this.CurrentMaintain["WorkType"]);
        }

        private void NumUnitCons_Validated(object sender, EventArgs e)
        {
            this.CurrentDetailData["Cons"] = CalculateCons(this.CurrentDetailData, MyUtility.Convert.GetDecimal(this.CurrentDetailData["ConsPC"]), MyUtility.Convert.GetDecimal(this.CurrentDetailData["Layer"]), this.dt_SizeRatio, CuttingForm.P02);
        }

        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.OnRefreshClick();
        }
    }
#pragma warning restore SA1600 // Elements should be documented
}
