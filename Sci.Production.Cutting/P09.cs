using Ict;
using Ict.Win;
using Ict.Win.UI;
using Sci.Data;
using Sci.Production.Class;
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
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq1;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq2;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_WKETA;
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
        private DataTable dtWorkOrderForOutput_SpreadingFabric; // 只顯示

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
   ,HasMarkerReq = CAST(IIF(EXISTS(SELECT 1 FROM MarkerReq_Detail WHERE OrderID = wo.ID), 1, 0) AS BIT)

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
        FOR XML PATH ('')), 1, 1, '')
) wp1
OUTER APPLY (
    SELECT FabricPanelCode_CONCAT = STUFF((
        SELECT CONCAT('+ ', FabricPanelCode)
        FROM WorkOrderForOutput_PatternPanel WITH (NOLOCK)
        WHERE WorkOrderForOutputUkey = wo.Ukey
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
    WHERE cod.WorkOrderUkey = wo.Ukey
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
                row["MarkerLength_Mask"] = CuttingWorkOrder.FormatMarkerLength(row["MarkerLength"].ToString());
                row["ActCuttingPerimeter_Mask"] = CuttingWorkOrder.FormatData(row["ActCuttingPerimeter"].ToString());
                row["StraightLength_Mask"] = CuttingWorkOrder.FormatData(row["StraightLength"].ToString());
                row["CurvedLength_Mask"] = CuttingWorkOrder.FormatData(row["CurvedLength"].ToString());
            }

            this.GetAllDetailData();
        }

        private void GetAllDetailData()
        {
            string sqlcmd = $@"
SELECT *, tmpKey = CAST(0 AS BIGINT) FROM WorkOrderForOutput_PatternPanel WHERE ID = '{this.CurrentMaintain["ID"]}'
SELECT *, tmpKey = CAST(0 AS BIGINT) FROM WorkOrderForOutput_SizeRatio WHERE ID = '{this.CurrentMaintain["ID"]}'
SELECT *, tmpKey = CAST(0 AS BIGINT) FROM WorkOrderForOutput_Distribute WHERE ID = '{this.CurrentMaintain["ID"]}'
SELECT *, tmpKey = CAST(0 AS BIGINT) FROM WorkOrderForOutput_SpreadingFabric WHERE POID = '{this.CurrentMaintain["ID"]}'
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
            this.dtWorkOrderForOutput_SpreadingFabric = dts[3];

            this.sizeRatiobs.DataSource = this.dtWorkOrderForOutput_SizeRatio;
            this.distributebs.DataSource = this.dtWorkOrderForOutput_Distribute;
            this.spreadingfabricbs.DataSource = this.dtWorkOrderForOutput_SpreadingFabric;
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
                .Text("OrderId", header: "SP#", width: Ict.Win.Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SEQ1", header: "Seq1", width: Ict.Win.Widths.AnsiChars(3)).Get(out this.col_Seq1)
                .Text("SEQ2", header: "Seq2", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_Seq2)
                .Text("Article_CONCAT", header: "Article", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ColorId", header: "Color", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Tone", header: "Tone", width: Ict.Win.Widths.AnsiChars(4))
                .Text("SizeCode_CONCAT", header: "Size", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Layer", header: "Layers", width: Ict.Win.Widths.AnsiChars(5), integer_places: 5, maximum: 99999)
                .Text("TotalCutQty_CONCAT", header: "Total CutQty", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("WKETA", header: "WK ETA", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true).Get(out this.col_WKETA)
                .Date("Fabeta", header: "Fabric Arr Date", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("Sewinline", header: "Sewing inline", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("EstCutDate", header: "Est. Cut Date", width: Ict.Win.Widths.AnsiChars(10))
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

            this.GridEventSet();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridRowChanged()
        {
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

            string filter = CuttingWorkOrder.GetFilter(this.CurrentDetailData, CuttingForm.P09);
            this.sizeRatiobs.Filter = filter;
            this.distributebs.Filter = filter;

            if (!MyUtility.Check.Empty(this.CurrentDetailData["Ukey"]))
            {
                this.gridDistributeToSP.SelectRowTo(0);
                for (int i = 0; i < this.gridDistributeToSP.Rows.Count; i++)
                {
                    if (this.gridDistributeToSP.Rows[i].Cells["OrderID"].Value.Equals(this.CurrentDetailData["OrderID"]))
                    {
                        this.gridDistributeToSP.SelectRowTo(i);
                        break;
                    }
                }
            }

            this.spreadingfabricbs.Filter = $"CutRef = '{this.CurrentDetailData["CutRef"]}'";
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 檢查
            #endregion

            #region 清除
            this.dtWorkOrderForOutput_SizeRatio.Select("SizeCode = ''").Delete();
            this.dtWorkOrderForOutput_Distribute.Select("OrderID = ''").Delete();
            this.dtWorkOrderForOutput_Distribute.Select("OrderID <> 'EXCESS' AND Article = ''").Delete(); // EXCESS 項 Article 為空
            this.dtWorkOrderForOutput_Distribute.Select("SizeCode = ''").Delete(); // EXCESS 項 SizeCode 有值
            #endregion

            return base.ClickSaveBefore();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            #region 校驗
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("There is no Workorder data that can be modified.");
                return;
            }

            // 1. 存在 P10 Bundle
            string msg = "The following bundle data exists and cannot be modify. If you need to modify, please go to [Cutting_P10. Bundle Card] to delete the bundle data.";
            if (!CuttingWorkOrder.CheckBundleAndShowData(this.CurrentDetailData["CutRef"].ToString(), msg))
            {
                return;
            }

            // 2. 存在 P20 CuttingOutput
            msg = "The following cutting output data exists and cannot be modify. If you need to delete, please go to [Cutting_P20. Cutting Daily Output] to delete the cutting output data.";
            if (!CuttingWorkOrder.CheckCuttingOutputCuttingOutputAndShowData(this.CurrentDetailData["CutRef"].ToString(), msg))
            {
                return;
            }

            // 3. 存在 P05 MarkerReq_Detail
            msg = "The following marker request data exists and cannot be modify. If you need to delete, please go to [Cutting_P05. Bulk Marker Request] to delete the marker request data.";
            if (!CuttingWorkOrder.CheckMarkerReqAndShowData(msg, this.CurrentMaintain["ID"].ToString()))
            {
                return;
            }
            #endregion

            // 單筆編輯視窗
            this.ShowDialogActionCutRef(DialogAction.Edit);
        }

        // 按 + 號 Icon
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            DialogResult result = this.ShowDialogActionCutRef(DialogAction.Create);
            if (result == DialogResult.Cancel)
            {
                this.OnDetailGridDelete();
            }
        }

        protected override void OnDetailGridInsertClick()
        {

        }

        private DialogResult ShowDialogActionCutRef(DialogAction action)
        {
            var form = new P09_ActionCutRef();
            form.Action = action;
            form.CurrentDetailData = this.CurrentDetailData;
            form.dtWorkOrderForOutput_SizeRatio_Ori = this.dtWorkOrderForOutput_SizeRatio;
            form.dtWorkOrderForOutput_Distribute_Ori = this.dtWorkOrderForOutput_Distribute;
            form.dtWorkOrderForOutput_PatternPanel_Ori = this.dtWorkOrderForOutput_PatternPanel;
            string filter = CuttingWorkOrder.GetFilter(this.CurrentDetailData, CuttingForm.P09);
            form.dtWorkOrderForOutput_SizeRatio = this.dtWorkOrderForOutput_SizeRatio.Select(filter).TryCopyToDataTable(this.dtWorkOrderForOutput_SizeRatio);
            form.dtWorkOrderForOutput_Distribute = this.dtWorkOrderForOutput_Distribute.Select(filter).TryCopyToDataTable(this.dtWorkOrderForOutput_Distribute);
            form.dtWorkOrderForOutput_PatternPanel = this.dtWorkOrderForOutput_PatternPanel.Select(filter).TryCopyToDataTable(this.dtWorkOrderForOutput_PatternPanel);
            return form.ShowDialog();
        }

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
                if (!CuttingWorkOrder.CheckBundleAndShowData(this.DetailDatas.AsEnumerable().Select(r => MyUtility.Convert.GetString(r["CutRef"])).ToList(), msg))
                {
                    return;
                }

                // 2. 存在 P20 CuttingOutput
                msg = "The following cutting output data exists and cannot be imported again. If you need to re-import, please go to [Cutting_P20. Cutting Daily Output] to delete the cutting output data.";
                if (!CuttingWorkOrder.CheckCuttingOutputAndShowData(this.DetailDatas.AsEnumerable().Select(r => MyUtility.Convert.GetString(r["CutRef"])).ToList(), msg))
                {
                    return;
                }
            }

            // 3. 存在 P05 MarkerReq_Detail
            msg = "The following marker request data exists and cannot be imported again. If you need to re-import, please go to [Cutting_P05. Bulk Marker Request] to delete the marker request data.";
            if (!CuttingWorkOrder.CheckMarkerReqAndShowData(msg, this.CurrentMaintain["ID"].ToString()))
            {
                return;
            }

            if (this.DetailDatas.Count > 0)
            {
                // 4. 欄位 SpreadingStatus
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
    ,'{Sci.Env.User.UserID}'
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
                        result = CuttingWorkOrder.InsertWorkOrder_Distribute(this.CurrentMaintain["ID"].ToString(), listWorkOrderUkey, "ForOutput", sqlConn);
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

            this.col_Seq1.EditingMouseDown += this.SeqCellEditingMouseDown;
            this.col_Seq2.EditingMouseDown += this.SeqCellEditingMouseDown;
            this.col_Seq1.CellValidating += this.SeqCelllValidatingHandler;
            this.col_Seq2.CellValidating += this.SeqCelllValidatingHandler;

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

            this.col_SpreadingNoID.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    SelectItem selectItem = CuttingWorkOrder.PopupSpreadingNo(dr["SpreadingNoID"].ToString());
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

                if (!CuttingWorkOrder.ValidatingSpreadingNo(e.FormattedValue.ToString(), out DataRow drV))
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
                    SelectItem selectItem = CuttingWorkOrder.PopupCutCell(dr["CutCellID"].ToString());
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

                if (!CuttingWorkOrder.ValidatingCutCell(e.FormattedValue.ToString()))
                {
                    dr["CutCellID"] = string.Empty;
                    e.Cancel = true;
                }

                dr["CutCellID"] = e.FormattedValue;
                dr.EndEdit();
            };

            this.col_MarkerLength.CellValidating += this.MaskedCellValidatingHandler;
            this.col_ActCuttingPerimeter.CellValidating += this.MaskedCellValidatingHandler;
            this.col_StraightLength.CellValidating += this.MaskedCellValidatingHandler;
            this.col_CurvedLength.CellValidating += this.MaskedCellValidatingHandler;

            this.col_MarkerNo.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.CanEditData(dr))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    SelectItem selectItem = CuttingWorkOrder.PopupMarkerNo(this.CurrentMaintain["ID"].ToString(), dr["MarkerNo"].ToString());
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

                if (!CuttingWorkOrder.ValidatingMarkerNo(this.CurrentMaintain["ID"].ToString(), e.FormattedValue.ToString()))
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
                if (CuttingWorkOrder.SizeCodeCellEditingMouseDown(e, this.gridSizeRatio, this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09))
                {
                    CuttingWorkOrder.UpdateConcatString(this.CurrentDetailData, this.dtWorkOrderForOutput_SizeRatio, CuttingForm.P09);
                }
            };
            this.col_SizeRatio_Size.CellValidating += (s, e) =>
            {
                if (CuttingWorkOrder.SizeCodeCellValidating(e, this.gridSizeRatio, this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09))
                {
                    CuttingWorkOrder.UpdateConcatString(this.CurrentDetailData, this.dtWorkOrderForOutput_SizeRatio, CuttingForm.P09);
                    CuttingWorkOrder.UpdateTotalDistributeQty(this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09);
                }
            };
            this.col_SizeRatio_Qty.CellValidating += (s, e) =>
            {
                if (CuttingWorkOrder.SizeRatioQtyCellValidating(e, this.gridSizeRatio, this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09))
                {
                    CuttingWorkOrder.UpdateConcatString(this.CurrentDetailData, this.dtWorkOrderForOutput_SizeRatio, CuttingForm.P09);
                    CuttingWorkOrder.UpdateTotalDistributeQty(this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09);
                }
            };
            #endregion

            #region Distribut
            this.col_Distribute_SP.EditingMouseDown += (s, e) =>
            {
                CuttingWorkOrder.Distribute3CellEditingMouseDown(e, this.CurrentDetailData["ID"].ToString(), this.dtWorkOrderForOutput_SizeRatio, this.gridDistributeToSP);
            };
            this.col_Distribute_SP.CellValidating += (s, e) =>
            {
                if (CuttingWorkOrder.Distribute3CellValidating(e, this.CurrentDetailData["ID"].ToString(), this.dtWorkOrderForOutput_SizeRatio, this.gridDistributeToSP))
                {
                    CuttingWorkOrder.UpdateTotalDistributeQty(this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09);
                }
            };
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
            bool canEdit = this.CanEditDatByGrid(grid, dr);
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
            bool canEdit = this.CanEditDatByGrid(grid, dr);
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

        private bool CanEditDatByGrid(Sci.Win.UI.Grid grid, DataRow dr)
        {
            if (grid.Name == "detailgrid")
            {
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
            if (!this.CanEditData(dr))
            {
                return;
            }

            SelectItem selectItem = CuttingWorkOrder.PopupSEQ(this.CurrentMaintain["ID"].ToString(), dr["SEQ1"].ToString(), dr["ColorID"].ToString());
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

            if (columnName == "MarkerLength")
            {
                dr["MarkerLength"] = dr["MarkerLength_Mask"] = CuttingWorkOrder.SetMarkerLengthMaskString(e.FormattedValue.ToString());
            }
            else
            {
                dr[columnName] = dr[columnName + "_Mask"] = CuttingWorkOrder.SetMaskString(e.FormattedValue.ToString());
            }
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
            string filter = CuttingWorkOrder.GetFilter(this.CurrentDetailData, CuttingForm.P09);
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
            this.dtWorkOrderForOutput_Distribute.Rows.Add(newrow);
        }

        private void MenuItemDeleteDistribute_Click(object sender, EventArgs e)
        {
            var selectRow = this.gridDistributeToSP.GetSelecteds(SelectedSort.Index);
            if (!this.CanEditData(this.CurrentDetailData) || selectRow.Count == 0)
            {
                return;
            }

            ((DataRowView)selectRow[0]).Row.Delete();
        }
        #endregion

        private bool CanEditData(DataRow dr)
        {
            return this.EditMode &&
                !(MyUtility.Convert.GetString(dr["SpreadingStatus"]).Equals("Ready", StringComparison.OrdinalIgnoreCase) ||
                  MyUtility.Convert.GetBool(dr["HasBundle"]) ||
                  MyUtility.Convert.GetBool(dr["HasCuttingOutput"]) ||
                  MyUtility.Convert.GetBool(dr["HasMarkerReq"]));
        }
    }
#pragma warning restore SA1600 // Elements should be documented
}
