using Ict;
using Ict.Win;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P02_BatchAssign : Win.Subs.Base
    {
        private List<DataRow> detailDatas_Ori; // 原始Detail
        private DataTable dt_CurentDetail; // 用來修改的 DataTable
        private DataTable sp; // 用在 Filter 開窗選項
        private string ID;
        private string Refno;
        private string ColorID;
        private CuttingForm form;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq1;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq2;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_SpreadingNoID; // P09 才有
        private Ict.Win.UI.DataGridViewTextBoxColumn col_CutCellID; // P09 才有
        private DataTable dt_AllSeqRefnoColor;

        /// <summary>
        /// Batch Assign
        /// </summary>
        /// <param name="detailDatas">Detail Table</param>
        /// <param name="id">cutting ID</param>
        /// <param name="form">P02或P09</param>
        /// <inheritdoc/>
        public P02_BatchAssign(List<DataRow> detailDatas, string id, CuttingForm form)
        {
            this.InitializeComponent();

            this.panel_P09.Visible = form == CuttingForm.P09;
            this.Text = CuttingForm.P09.ToString() + ".Batch Assign Cell/Est. Cut Date";
            this.ID = id;
            this.form = form;
            this.detailDatas_Ori = detailDatas;
            this.dt_CurentDetail = detailDatas.CopyToDataTable();
            this.dt_CurentDetail.Columns.Add("Selected", typeof(bool));
            this.Gridsetup();
            this.detailgridbs.DataSource = this.dt_CurentDetail;

            this.dt_AllSeqRefnoColor = GetAllSeqRefnoColor(id); // 先準備用來驗證 Seq 全部資訊, 避免逐筆去DB撈資料驗證會很卡
            this.sp = this.dt_CurentDetail.DefaultView.ToTable(true, "OrderID"); // 用在 Filter 開窗選項
        }

        private void Gridsetup()
        {
            this.gridBatchAssign.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridBatchAssign)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("CutRef", header: "CutRef#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(5))
                .MarkerLength("MarkerLength_Mask", "Marker Length", "MarkerLength", Ict.Win.Widths.AnsiChars(16), this.CanEditData)
                .Text("PatternPanel_CONCAT", header: "Pattern Panel", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("FabricPanelCode_CONCAT", header: "Fabric\r\nPanel Code", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                ;

            if (this.form == CuttingForm.P09)
            {
                this.Helper.Controls.Grid.Generator(this.gridBatchAssign)
                    .Text("SpreadingNoID", header: "Spreading No", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_SpreadingNoID)
                    .Text("CutCellID", header: "Cut Cell", width: Ict.Win.Widths.AnsiChars(2)).Get(out this.col_CutCellID)
                    ;
            }

            this.Helper.Controls.Grid.Generator(this.gridBatchAssign)
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3)).Get(out this.col_Seq1)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2)).Get(out this.col_Seq2)
                .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Tone", header: "Tone", width: Widths.AnsiChars(10), iseditingreadonly: false)
                .Text("SizeCode_CONCAT", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
                .Text("TotalCutQty_CONCAT", header: "Total CutQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .WorkOrderWKETA("WKETA", "WK ETA", Ict.Win.Widths.AnsiChars(10), true, this.CanEditData)
                .Date("Fabeta", header: "Fabric Arr Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .EstCutDate("EstCutDate", "Est. Cut Date", Ict.Win.Widths.AnsiChars(10), this.CanEditData)
                ;
            if (this.form == CuttingForm.P02)
            {
                this.Helper.Controls.Grid.Generator(this.gridBatchAssign)
                    .Text("CutPlanID", header: "Cut Plan", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true);
            }

            this.Helper.Controls.Grid.Generator(this.gridBatchAssign)
                .Text("MarkerNo", header: "Pattern No", width: Ict.Win.Widths.AnsiChars(10));

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
                filter += $" AND Article like '%{this.txtArticle.Text}%'";
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

            this.detailgridbs.Filter = filter;
        }

        private void BtnBatchUpdate_Click(object sender, EventArgs e)
        {
            this.gridBatchAssign.ValidateControl();

            foreach (DataRow dr in this.dt_CurentDetail.Select("Selected = 1"))
            {
                if (MyUtility.Check.Empty(this.dateBoxEstCutDate.Value))
                {
                    dr["EstCutDate"] = MyUtility.Convert.GetDate(this.dateBoxEstCutDate.Value);
                }

                if (this.txtMarkerLength.Text != "00Y00-0/0+0\"" && this.txtMarkerLength.Text != "Y  - / + \"")
                {
                    dr["MarkerLength"] = dr["MarkerLength_Mask"] = this.txtMarkerLength.Text;
                }

                if (!MyUtility.Check.Empty(this.txtMakerName.Text))
                {
                    dr["MarkerName"] = this.txtMakerName.Text;
                }

                if (this.form == CuttingForm.P09)
                {
                    dr["SpreadingNoID"] = this.txtSpreadingNo.Text;
                    dr["CutCellID"] = this.txtCell.Text;
                }
            }

            // Seq、WK ETA (Seq會影響 WK ETA，因此一併判斷+寫入)
            string seq1 = this.txtSeq1.Text;
            string seq2 = this.txtSeq2.Text;
            string wkETA = string.Empty;

            // Seq 部分一定要先有填過第3層 PatternPanel 資料,也就是有 FabricCode 註:ID + FabricCode 是找 SEQ 不發散的必要條件
            foreach (DataRow dr in this.dt_CurentDetail.Select("Selected = 1 AND FabricCode <> ''"))
            {

            }

                if (!MyUtility.Check.Empty(seq1) && !MyUtility.Check.Empty(seq2))
            {
                foreach (DataRow dr in this.dt_CurentDetail.Select("Selected = 1"))
                {
                    // FabricCode、Refno、Colorid 為空不可新增Seq，同P02 Grid驗證方式
                    if (MyUtility.Check.Empty(dr["FabricCode"]) || MyUtility.Check.Empty(dr["Refno"]) || MyUtility.Check.Empty(dr["Colorid"]))
                    {
                        continue;
                    }

                    // 逐一檢查Seq 正確性
                    bool isValid = ValidatingSeqWithoutFabricCode(this.ID, dr["FabricCode"].ToString(), seq1, seq2, dr["Refno"].ToString(), dr["Colorid"].ToString(), this.dt_AllSeqRefnoColor);

                    if (isValid)
                    {
                        dr["Seq1"] = seq1;
                        dr["Seq2"] = seq2;
                        /*dr["SCIRefno"] = ;
                        dr["Refno"] = ;
                        dr["ColorID"] = ;*/

                        if (!string.IsNullOrEmpty(wkETA))
                        {
                            dr["WKETA"] = MyUtility.Convert.GetDate(wkETA);
                        }
                    }

                    dr.EndEdit();
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
                detaildr["MarkerLength_Mask"] = dr["MarkerLength_Mask"];

                // FabricPanelCode 為空不可新增Seq，同P02 Grid驗證方式
                if (!MyUtility.Check.Empty(detaildr["FabricPanelCode"]))
                {
                    detaildr["Seq1"] = dr["Seq1"];
                    detaildr["Seq2"] = dr["Seq2"];
                }

                if (this.form == CuttingForm.P09)
                {
                    detaildr["SpreadingNoID"] = dr["SpreadingNoID"];
                    detaildr["CutCellID"] = dr["CutCellID"];
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
            SelectItem item = PopupAllSeqRefnoColor(this.ID, this.dt_AllSeqRefnoColor);
            if (item == null)
            {
                return;
            }

            this.txtSeq1.Text = item.GetSelecteds()[0]["Seq1"].ToString();
            this.txtSeq2.Text = item.GetSelecteds()[0]["Seq2"].ToString();
        }

        private void TxtSeq_Validating(object sender, CancelEventArgs e)
        {
            Win.UI.TextBox txtSeq = (Win.UI.TextBox)sender;
            string seqValue = txtSeq.Text;
            if (MyUtility.Check.Empty(seqValue))
            {
                return;
            }

            // 這邊的驗證無法取得詳細資訊，採用最低限度的條件，後續Confirm再詳細檢驗
            if (GetFilterAllSeqRefnoColor(this.ID, this.txtSeq1.Text, this.txtSeq2.Text, string.Empty, string.Empty, this.dt_AllSeqRefnoColor).Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox($@"Seq: {seqValue} data not found!");
                txtSeq.Text = string.Empty;
                e.Cancel = true;
                return;
            }

            this.txtSeq1.Text = string.Empty;
        }

        private void TxtMakerLength_Validating(object sender, CancelEventArgs e)
        {
            this.txtMarkerLength.Text = Prgs.SetMarkerLengthMaskString(this.txtMarkerLength.Text);
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

            if (this.form == CuttingForm.P09)
            {
                BindGridSpreadingNo(this.col_SpreadingNoID, this.gridBatchAssign, this.CanEditData);
                BindGridCutCell(this.col_CutCellID, this.gridBatchAssign, this.CanEditData);
            }
        }
        #endregion

        #region Other
        private bool CanEditDataByGrid(Sci.Win.UI.Grid grid, DataRow dr, string columNname)
        {
            return true; // 能帶進來的資料必定是能編輯的
        }

        private bool CanEditData(DataRow dr)
        {
            return true; // 能帶進來的資料必定是能編輯的
        }
        #endregion
    }
}
