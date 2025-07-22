using Ict;
using Ict.Win;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class Cutting_BatchAssign : Win.Subs.Base
    {
#pragma warning disable SA1401 // Fields should be private
#pragma warning disable SA1600 // Elements should be documented
        public bool editByUseCutRefToRequestFabric = true; // P09 useCutRefToRequestFabric = 1 為 False
        private List<DataRow> detailDatas_Ori; // 原始Detail
        private DataTable dt_CurentDetail; // 用來修改的 DataTable
        private DataTable sp; // 用在 Filter 開窗選項
        private string ID;
        private CuttingForm form;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq1;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq2;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_SpreadingNoID;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_CutCellID; // P09 才有
        private DataTable dtAllSEQ_FabricCode;
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// Batch Assign
        /// </summary>
        /// <param name="detailDatas">Detail Table</param>
        /// <param name="id">cutting ID</param>
        /// <param name="form">P02或P09</param>
        /// <inheritdoc/>
        public Cutting_BatchAssign(List<DataRow> detailDatas, string id, CuttingForm form)
        {
            this.InitializeComponent();

            this.Text = form.ToString() + ". Batch Assign";
            this.ID = id;
            this.form = form;
            this.detailDatas_Ori = detailDatas;
            this.dt_CurentDetail = detailDatas.CopyToDataTable();
            this.dt_CurentDetail.Columns.Add("Selected", typeof(bool));
            this.GridSetup();
            this.detailgridbs.DataSource = this.dt_CurentDetail;
            this.txtSpreadingNo.MDivision = Sci.Env.User.Keyword;
            this.txtSpreadingNo.IncludeJunk = false;

            this.dtAllSEQ_FabricCode = GetAllSEQ_FabricCode(id); // 先準備用來驗證 Seq 全部資訊, 避免逐筆去DB撈資料驗證會很卡
            this.sp = this.dt_CurentDetail.DefaultView.ToTable(true, "OrderID"); // 用在 Filter 開窗選項
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.txtMakerName.Enabled = this.editByUseCutRefToRequestFabric;
            this.dateWKETA.Enabled = this.editByUseCutRefToRequestFabric;
            this.txtMarkerLength.Enabled = this.editByUseCutRefToRequestFabric;
            this.txtSeq1.Enabled = this.editByUseCutRefToRequestFabric;
            this.txtSeq2.Enabled = this.editByUseCutRefToRequestFabric;
            if (this.form == CuttingForm.P02)
            {
                this.chkShowEmptyCutRef.Checked = true;
            }
            else
            {
                this.chkShowEmptyCutRef.Checked = this.editByUseCutRefToRequestFabric;
            }

            this.BtnFilter_Click(null, null);
        }

        private void GridSetup()
        {
            this.gridBatchAssign.IsEditingReadOnly = false;

            if (this.form == CuttingForm.P09)
            {
                this.Helper.Controls.Grid.Generator(this.gridBatchAssign)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("CutRef", header: "CutRef#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("CutNo", header: "Cut#", width: Widths.AnsiChars(4), integer_places: 5)
                .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(5))
                .MarkerLength("MarkerLength_Mask", "Marker Length", "MarkerLength", Ict.Win.Widths.AnsiChars(16), this.CanEditData)
                .Text("PatternPanel_CONCAT", header: "Pattern Panel", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("FabricPanelCode_CONCAT", header: "Fabric\r\nPanel Code", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SpreadingNoID", header: "Spreading No", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_SpreadingNoID)
                .Text("CutCellID", header: "Cut Cell", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_CutCellID)
                .Text("CutPlanID", header: "Cut Plan", width: Ict.Win.Widths.AnsiChars(15))
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3)).Get(out this.col_Seq1)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2)).Get(out this.col_Seq2)
                .Text("Article_CONCAT", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Tone", header: "Tone", width: Widths.AnsiChars(10), iseditingreadonly: false)
                .Text("SizeCode_CONCAT", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
                .Text("TotalCutQty_CONCAT", header: "Total CutQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .WorkOrderWKETA("WKETA", "WK ETA", Ict.Win.Widths.AnsiChars(10), true, this.CanEditData)
                .Date("Fabeta", header: "Fabric Arr Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .EstCutDate("EstCutDate", "Est. Cut Date", Ict.Win.Widths.AnsiChars(10), this.CanEditNotWithUseCutRefToRequestFabric)
                .MarkerNo("MarkerNo", "Pattern No.", Ict.Win.Widths.AnsiChars(12), this.CanEditData)
                ;
            }
            else
            {
                this.Helper.Controls.Grid.Generator(this.gridBatchAssign)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("CutRef", header: "CutRef#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Seq", header: "Seq", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(5))
                .MarkerLength("MarkerLength_Mask", "Marker Length", "MarkerLength", Ict.Win.Widths.AnsiChars(16), this.CanEditData)
                .Text("PatternPanel_CONCAT", header: "Pattern Panel", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("FabricPanelCode_CONCAT", header: "Fabric\r\nPanel Code", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3)).Get(out this.col_Seq1)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2)).Get(out this.col_Seq2)
                .Text("Article_CONCAT", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Tone", header: "Tone", width: Widths.AnsiChars(10), iseditingreadonly: false)
                .Text("SizeCode_CONCAT", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
                .Text("TotalCutQty_CONCAT", header: "Total CutQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .WorkOrderWKETA("WKETA", "WK ETA", Ict.Win.Widths.AnsiChars(10), true, this.CanEditData)
                .Date("Fabeta", header: "Fabric Arr Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .EstCutDate("EstCutDate", "Est. Cut Date", Ict.Win.Widths.AnsiChars(10), this.CanEditData)
                .Text("CutPlanID", header: "Cut Plan", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SpreadingNoID", header: "Spreading No", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_SpreadingNoID)
                .Text("CutCellID", header: "Cut Cell", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_CutCellID)
                .MarkerNo("MarkerNo", "Pattern No.", Ict.Win.Widths.AnsiChars(12), this.CanEditData)
                ;
            }

            this.GridEventSet();
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            this.Filter();
        }

        private void Filter()
        {
            string filter = " 1=1 ";
            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                filter += $" AND OrderID ='{this.txtSPNo.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtArticle.Text))
            {
                filter += $" AND Article_CONCAT like '%{this.txtArticle.Text}%'";
            }

            if (!MyUtility.Check.Empty(this.txtMarkerName_Filter.Text))
            {
                filter += $" AND MarkerName ='{this.txtMarkerName_Filter.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSizeCode.Text))
            {
                filter += $" AND SizeCode_CONCAT like '%{this.txtSizeCode.Text}%'";
            }

            if (!MyUtility.Check.Empty(this.txtFabricPanelCode.Text))
            {
                filter += $" AND FabricPanelCode_CONCAT like '%{this.txtFabricPanelCode.Text}%'";
            }

            if (!MyUtility.Check.Empty(this.DateEstCutDate.Value))
            {
                filter += $" AND EstCutDate ='{this.DateEstCutDate.Text}'";
            }

            if (this.checkOnlyShowEmptyEstCutDate.Checked)
            {
                filter += " AND EstCutDate is null ";
            }

            if (this.chkShowEmptyCutRef.Checked)
            {
                filter += " AND ISNULL(CutRef, '') = ''";
            }

            this.detailgridbs.Filter = filter;
        }

        private void BtnBatchUpdate_Click(object sender, EventArgs e)
        {
            this.gridBatchAssign.ValidateControl();

            foreach (DataRow dr in this.dt_CurentDetail.Select("Selected = 1"))
            {
                if (this.chkEstCutDate.Checked)
                {
                    dr["EstCutDate"] = this.dateBoxEstCutDate.Value.HasValue ? (object)this.dateBoxEstCutDate.Value : DBNull.Value;
                }

                if (this.chkMarkerLength.Checked)
                {
                    dr["MarkerLength"] = dr["MarkerLength_Mask"] = this.txtMarkerLength.Text;
                }

                if (this.chkMarkerName.Checked)
                {
                    dr["MarkerName"] = this.txtMakerName.Text;
                }

                if (this.chkCutCell.Checked)
                {
                    dr["CutCellID"] = this.txtCell.Text;
                }

                if (this.chkSpreadingNo.Checked)
                {
                    dr["SpreadingNoID"] = this.txtSpreadingNo.Text;
                }
            }

            // 驗證用
            DataTable dtWKETA = GetWKETA(this.ID);

            foreach (DataRow dr in this.dt_CurentDetail.Select("Selected = 1"))
            {
                #region 驗證 Seq
                // 先填入再驗證
                if (this.chkSeq.Checked)
                {
                    dr["Seq1"] = this.txtSeq1.Text;
                }

                if (this.chkSeq.Checked)
                {
                    dr["Seq2"] = this.txtSeq2.Text;
                }

                if (this.chkWKETA.Checked)
                {
                    dr["WKETA"] = this.dateWKETA.Value.HasValue ? (object)this.dateWKETA.Value : DBNull.Value;
                }

                // 驗證存在 dtAllSEQ_FabricCode
                string seq1 = MyUtility.Convert.GetString(dr["Seq1"]);
                string seq2 = MyUtility.Convert.GetString(dr["Seq2"]);
                string refno = MyUtility.Convert.GetString(dr["Refno"]);
                string colorID = MyUtility.Convert.GetString(dr["ColorID"]);

                // 驗證失敗, 清空, Seq, 此筆 FabricCode 為空=沒有PatternPanel資訊, 也要清空
                if (GetFilterSEQ(seq1, seq2, refno, colorID, this.dtAllSEQ_FabricCode).Rows.Count == 0 || MyUtility.Check.Empty(dr["FabricCode"]))
                {
                    if (!MyUtility.Check.Empty(this.txtSeq1.Text))
                    {
                        dr["Seq1"] = string.Empty;
                    }

                    if (!MyUtility.Check.Empty(this.txtSeq2.Text))
                    {
                        dr["Seq2"] = string.Empty;
                    }
                }
                #endregion

                // 再 Seq 之後驗證 WKETA, 更新 SEQ, 沒更新 WKETA, 驗證失敗也要清空
                if (!MyUtility.Check.Empty(dr["WKETA"]) && dtWKETA.Select($"Seq1 = '{dr["Seq1"]}' AND Seq2 = '{dr["Seq2"]}' AND WKETA = '{dr["WKETA"]}'").Length == 0)
                {
                    dr["WKETA"] = DBNull.Value;
                }
            }
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            this.gridBatchAssign.ValidateControl();

            foreach (DataRow dr in this.dt_CurentDetail.Select($"Selected = 1"))
            {
                // 更新回表身, 1對1
                var detaildr = this.detailDatas_Ori.Where(row => MyUtility.Convert.GetInt(row["Ukey"]) == MyUtility.Convert.GetInt(dr["Ukey"]) && MyUtility.Convert.GetInt(row["tmpKey"]) == MyUtility.Convert.GetInt(dr["tmpKey"])).First();

                detaildr["EstCutDate"] = dr["EstCutDate"];
                detaildr["WKETA"] = dr["WKETA"];
                detaildr["Tone"] = dr["Tone"];
                detaildr["MarkerName"] = dr["MarkerName"];
                detaildr["MarkerLength"] = dr["MarkerLength"];
                detaildr["MarkerLength_Mask"] = dr["MarkerLength_Mask"];
                detaildr["MarkerNo"] = dr["MarkerNo"];
                detaildr["Seq1"] = dr["Seq1"];
                detaildr["Seq2"] = dr["Seq2"];
                detaildr["CutCellID"] = dr["CutCellID"];
                detaildr["SpreadingNoID"] = dr["SpreadingNoID"];

                if (this.form == CuttingForm.P09)
                {
                    detaildr["CutNo"] = dr["CutNo"];
                }

                if (this.form == CuttingForm.P02)
                {
                    detaildr["Seq"] = dr["Seq"];
                }
            }

            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 文字方塊 開窗/驗證，非單筆資料 PS: 有連動其它欄位,全部都在按下 Batch Assign 才進行驗證

        // Filter 區塊
        private void TxtSPNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            SelectItem selectitem = new SelectItem(this.sp, "OrderID", "20", this.txtSPNo.Text, headercaptions: "SPNo");
            if (selectitem.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            this.txtSPNo.Text = selectitem.GetSelectedString();
            this.Filter(); // 立即 Filter
        }

        // 用在多選更新區塊
        private void TxtSeq_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            SelectItem item = PopupAllSeqRefnoColor(this.dtAllSEQ_FabricCode);
            if (item == null)
            {
                return;
            }

            this.txtSeq1.Text = item.GetSelecteds()[0]["Seq1"].ToString();
            this.txtSeq2.Text = item.GetSelecteds()[0]["Seq2"].ToString();

            this.TxtSeqCheck(this.txtSeq1.Text, this.txtSeq2.Text);
        }

        private void TxtSeq_Validating(object sender, CancelEventArgs e)
        {
            Win.UI.TextBox txtSeq = (Win.UI.TextBox)sender;
            string seqValue = txtSeq.Text;
            if (MyUtility.Check.Empty(seqValue))
            {
                return;
            }

            // 這邊的驗證只先驗證 Seq1 + Seq2, Confirm 會再詳細驗證
            if (GetFilterSEQ(this.txtSeq1.Text, this.txtSeq2.Text, string.Empty, string.Empty, this.dtAllSEQ_FabricCode).Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox($@"Seq: {seqValue} data not found!");
                txtSeq.Text = string.Empty;
                e.Cancel = true;
                return;
            }
            this.TxtSeqCheck(this.txtSeq1.Text, this.txtSeq2.Text);
        }
        #endregion

        #region Grid 單筆資料的欄位開窗/驗證
        private void GridEventSet()
        {
            // 可否編輯 && 顏色
            ConfigureColumnEvents(this.gridBatchAssign, this.CanEditDataByGrid);

            // Seq 兩個欄位
            ConfigureSeqColumnEvents(this.col_Seq1, this.gridBatchAssign, this.CanEditData);
            ConfigureSeqColumnEvents(this.col_Seq2, this.gridBatchAssign, this.CanEditData);

            BindGridCutCell(this.col_CutCellID, this.gridBatchAssign, this.CanEditNotWithUseCutRefToRequestFabric);
            BindGridSpreadingNo(this.col_SpreadingNoID, this.gridBatchAssign, this.CanEditNotWithUseCutRefToRequestFabric);
        }
        #endregion

        #region Other
        private bool CanEditNotWithUseCutRefToRequestFabric(DataRow dr)
        {
            return true;
        }

        private bool CanEditDataByGrid(Sci.Win.UI.Grid grid, DataRow dr, string columNname)
        {
            if (columNname.EqualString("EstCutDate") || columNname.EqualString("CutCellID") || columNname.EqualString("SpreadingNoID"))
            {
                return this.CanEditNotWithUseCutRefToRequestFabric(dr);
            }

            return this.CanEditData(dr);
        }

        private bool CanEditData(DataRow dr)
        {
            return this.editByUseCutRefToRequestFabric;
        }
        #endregion

        private void DateBoxEstCutDate_Validating(object sender, CancelEventArgs e)
        {
            if (this.dateBoxEstCutDate.Value.HasValue)
            {
                this.chkEstCutDate.Checked = true;
            }
        }

        private void DateWKETA_Validating(object sender, CancelEventArgs e)
        {
            if (this.dateWKETA.Value.HasValue)
            {
                this.chkWKETA.Checked = true;
            }
        }

        private void TxtMakerName_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtMakerName.Text))
            {
                this.chkMarkerName.Checked = true;
            }
        }

        private void TxtCell_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtCell.Text))
            {
                this.chkCutCell.Checked = true;
            }
        }

        private void TxtMarkerLength_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtMarkerLength.Text))
            {
                this.chkMarkerLength.Checked = true;
            }
        }

        private void TxtSeqCheck(string seq1, string seq2)
        {
            if (!string.IsNullOrEmpty(seq1) || !string.IsNullOrEmpty(seq2))
            {
                this.chkSeq.Checked = true;
            }
        }

        private void DateBoxEstCutDate_Validated(object sender, EventArgs e)
        {
            if (this.dateBoxEstCutDate.Value.HasValue)
            {
                this.chkEstCutDate.Checked = true;
            }
        }

        private void TxtSpreadingNo_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtSpreadingNo.Text))
            {
                this.chkSpreadingNo.Checked = true;
            }
        }
    }
}
