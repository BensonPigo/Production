using Ict;
using Ict.Win;
using Sci.Win.Tools;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P02_BatchAssign : Win.Subs.Base
    {
        /// <summary>
        /// 原始Detail
        /// </summary>
        private DataTable dt_OriDetail;

        /// <summary>
        /// 原始Detail排除不可修改
        /// </summary>
        private DataTable dt_CurentDetail;
        private DataTable sp;
        private string Poid;

        private Ict.Win.UI.DataGridViewDateBoxColumn col_WKETA = new Ict.Win.UI.DataGridViewDateBoxColumn();
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq1;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Seq2;

        /// <summary>
        /// Initializes a new instance of the <see cref="P02_BatchAssign"/> class.
        /// </summary>
        /// <param name="detailTable">Detail Table</param>
        /// <param name="cuttingPoid">cuttingPoid</param>
        public P02_BatchAssign(DataTable detailTable, string cuttingPoid)
        {
            this.InitializeComponent();
            this.Poid = cuttingPoid;
            this.dt_OriDetail = detailTable;
            this.dt_CurentDetail = detailTable;
            this.dt_CurentDetail.Columns.Add("Sel", typeof(bool));
            this.Gridsetup();
            this.BtnFilter_Click(null, null);  // 1390: CUTTING_P02_BatchAssignCellCutDate，當進去此功能時應直接預帶資料。

            MyUtility.Tool.ProcessWithDatatable(this.dt_CurentDetail, "OrderID", $@"select distinct OrderID from #tmp", out this.sp);
            if (this.dt_CurentDetail != null)
            {
                DataTable dtcopy = this.dt_CurentDetail.Copy();
                dtcopy.AcceptChanges();
            }
        }

        private void Gridsetup()
        {
            DataGridViewGeneratorDateColumnSettings col_EstCutDate = new DataGridViewGeneratorDateColumnSettings();
            Ict.Win.UI.DataGridViewMaskedTextBoxColumn col_MarkerLength = new Ict.Win.UI.DataGridViewMaskedTextBoxColumn();
            Ict.Win.UI.DataGridViewTextBoxColumn col_MarkerNo = new Ict.Win.UI.DataGridViewTextBoxColumn();

            col_EstCutDate.CellValidating += (s, e) =>
            {
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    DataRow dr = ((Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                    if (e.FormattedValue.ToString() == dr["EstCutDate"].ToString())
                    {
                        return;
                    }

                    if (DateTime.Compare(DateTime.Today, Convert.ToDateTime(e.FormattedValue)) > 0)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("[Est. Cut Date] can not be passed !!");
                    }
                }
            };

            col_MarkerLength.CellValidating += (s, e) =>
            {
                DataRow dr = ((Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["CutPlanID"]))
                {
                    return;
                }

                string columnName = (s as DataGridViewColumn)?.Name;
                if (columnName == "MarkerLength")
                {
                    dr["MarkerLength"] = dr["MarkerLength_Mask"] = CuttingWorkOrder.SetMarkerLengthMaskString(e.FormattedValue.ToString());
                }
                else
                {
                    dr[columnName] = dr[columnName + "_Mask"] = CuttingWorkOrder.SetMaskString(e.FormattedValue.ToString());
                }
            };

            this.gridBatchAssign.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.Helper.Controls.Grid.Generator(this.gridBatchAssign)
             .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
             .Text("CutRef", header: "CutRef#", width: Widths.AnsiChars(6), iseditingreadonly: true)
             .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(5))
             .MaskedText("MarkerLength_Mask", "00Y00-0/0+0\"", "Marker Length", name: "Marker Length", width: Ict.Win.Widths.AnsiChars(16)).Get(out col_MarkerLength)
             .Text("PatternPanel_CONCAT", header: "Pattern Panel", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
             .Text("FabricPanelCode_CONCAT", header: "Fabric\r\nPanel Code", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
             .Text("OrderId", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
             .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3)).Get(out this.col_Seq1)
             .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2)).Get(out this.col_Seq2)
             .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
             .Text("Tone", header: "Tone", width: Widths.AnsiChars(10), iseditingreadonly: false)
             .Text("SizeCode_CONCAT", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
             .Text("TotalCutQty_CONCAT", header: "Total CutQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Date("WKETA", header: "WK ETA", width: Widths.AnsiChars(10), iseditingreadonly: true).Get(out this.col_WKETA)
             .Date("Fabeta", header: "Fabric Arr Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Date("EstCutDate", header: "Est. Cut Date", width: Widths.AnsiChars(10), iseditingreadonly: false, settings: col_EstCutDate)
             .Text("CutPlanID", header: "Cut Plan", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
             .Text("MarkerNo", header: "Pattern No", width: Ict.Win.Widths.AnsiChars(10)).Get(out col_MarkerNo);

            this.GridEventSet();
            this.Filter();
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            this.Filter();
        }

        private void Filter()
        {
            string sp = this.txtSPNo.Text;
            string article = this.txtArticle.Text;
            string markername = this.txtMarkerName.Text;
            string sizecode = this.txtSizeCode.Text;
            string fabricpanelcode = this.txtFabricPanelCode.Text;
            string estCutDate = this.txtEstCutDate.Text.ToString();
            string filter = " 1=1 ";
            if (!MyUtility.Check.Empty(sp))
            {
                filter = filter + string.Format(" and OrderId ='{0}'", sp);
            }

            if (!MyUtility.Check.Empty(article))
            {
                filter = filter + string.Format(" and Article like '%{0}%'", article);
            }

            if (!MyUtility.Check.Empty(markername))
            {
                filter = filter + string.Format(" and MarkerName ='{0}'", markername);
            }

            if (!MyUtility.Check.Empty(sizecode))
            {
                filter = filter + string.Format(" and SizeCode_CONCAT like '%{0}%'", sizecode);
            }

            if (!MyUtility.Check.Empty(fabricpanelcode))
            {
                filter = filter + string.Format(" and FabricPanelCode_CONCAT like'{0}'", fabricpanelcode);
            }

            if (!MyUtility.Check.Empty(this.txtEstCutDate.Value))
            {
                filter = filter + string.Format(" and EstCutDate ='{0}'", estCutDate);
            }

            if (this.checkOnlyShowEmptyEstCutDate.Checked)
            {
                filter = filter + " and EstCutDate is null ";
            }

            this.dt_CurentDetail.DefaultView.RowFilter = filter;
            this.gridBatchAssign.DataSource = this.dt_CurentDetail;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBatchUpdate_Click(object sender, EventArgs e)
        {
            this.gridBatchAssign.ValidateControl();
            string estCutDate = string.Empty;
            if (!MyUtility.Check.Empty(this.txtBatchUpdateEstCutDate.Value))
            {
                estCutDate = this.txtBatchUpdateEstCutDate.Text;
            }

            if (!string.IsNullOrEmpty(estCutDate))
            {
                // Est. Cut Date
                foreach (DataRow dr in this.dt_CurentDetail.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    if (dr["Sel"].ToString() == "True")
                    {
                        if (estCutDate != string.Empty)
                        {
                            dr["EstCutDate"] = MyUtility.Convert.GetDate(estCutDate);
                        }
                        else
                        {
                            dr["EstCutDate"] = DBNull.Value;
                        }
                    }
                }
            }

            // Seq、WK ETA (Seq會影響 WK ETA，因此一併判斷+寫入)
            string seq1 = this.txtSeq1.Text;
            string seq2 = this.txtSeq2.Text;
            string wkETA = string.Empty;
            if (!MyUtility.Check.Empty(this.txtWKETA.Text))
            {
                wkETA = this.txtWKETA.Text;
            }

            if (!MyUtility.Check.Empty(seq1) && !MyUtility.Check.Empty(seq2))
            {
                foreach (DataRow dr in this.dt_CurentDetail.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    // FabricCode、Refno、Colorid 為空不可新增Seq，同P02 Grid驗證方式
                    if (MyUtility.Check.Empty(dr["FabricCode"]) || MyUtility.Check.Empty(dr["Refno"]) || MyUtility.Check.Empty(dr["Colorid"]))
                    {
                        continue;
                    }

                    if (dr["Sel"].ToString() == "True")
                    {
                        // 逐一檢查Seq 正確性
                        bool isValid = ValidatingSeqWithoutFabricCode(this.Poid, dr["FabricCode"].ToString(), seq1, seq2, dr["Refno"].ToString(), dr["Colorid"].ToString(), out DataTable dtValidating);

                        if (isValid)
                        {
                            dr["Seq1"] = seq1;
                            dr["Seq2"] = seq2;

                            if (!string.IsNullOrEmpty(wkETA))
                            {
                                dr["WKETA"] = MyUtility.Convert.GetDate(wkETA);
                            }
                        }

                        dr.EndEdit();
                    }
                }
            }

            // Marker Name
            string markerName = this.txtUpdateMakerName.Text;
            string markerLength = this.txtMarkerLength.Text;
            if (!MyUtility.Check.Empty(markerName))
            {
                foreach (DataRow dr in this.dt_CurentDetail.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    if (dr["Sel"].ToString() == "True")
                    {
                        dr["MarkerName"] = markerName;
                    }
                }
            }

            // Marker Length
            if (markerLength != "00Y00-0/0+0\"" && markerLength != "Y  - / + \"")
            {
                foreach (DataRow dr in this.dt_CurentDetail.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    if (dr["Sel"].ToString() == "True")
                    {
                        dr["MarkerLength_Mask"] = markerLength;
                    }
                }
            }
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            this.gridBatchAssign.ValidateControl();

            foreach (DataRow dr in this.dt_CurentDetail.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                if (dr["Sel"].ToString() == "True")
                {
                    DataRow[] detaildr;
                    if (MyUtility.Check.Empty(dr["Ukey"]))
                    {
                        detaildr = this.dt_OriDetail.Select(string.Format("tmpKey = '{0}'", dr["tmpKey"]));
                    }
                    else
                    {
                        detaildr = this.dt_OriDetail.Select(string.Format("Ukey = '{0}'", dr["Ukey"]));
                    }

                    detaildr[0]["EstCutDate"] = dr["EstCutDate"];
                    detaildr[0]["WKETA"] = dr["WKETA"];
                    detaildr[0]["Tone"] = dr["Tone"];
                    detaildr[0]["MarkerName"] = dr["MarkerName"];
                    detaildr[0]["MarkerLength_Mask"] = dr["MarkerLength_Mask"];

                    // FabricPanelCode 為空不可新增Seq，同P02 Grid驗證方式
                    if (!MyUtility.Check.Empty(detaildr[0]["FabricPanelCode"]))
                    {
                        detaildr[0]["Seq1"] = dr["Seq1"];
                        detaildr[0]["Seq2"] = dr["Seq2"];
                    }
                }
            }

            this.Close();
        }

        #region 文字方塊 開窗/驗證，非單筆資料
        private void TxtBatchUpdateEstCutDate_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtBatchUpdateEstCutDate.Value))
            {
                if (DateTime.Compare(DateTime.Today, Convert.ToDateTime(this.txtBatchUpdateEstCutDate.Value)) > 0)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("[Est. Cut Date] can not be passed !!");
                }
            }
        }

        private void TxtSPNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            SelectItem sele;
            sele = new SelectItem(this.sp, "OrderID", "15@300,400", this.sp.Columns["OrderID"].ToString(), columndecimals: "50");
            DialogResult result = sele.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtSPNo.Text = sele.GetSelectedString();
            this.Filter();
        }

        private void TxtSeq1_Validating(object sender, CancelEventArgs e)
        {
            string seq1 = this.txtSeq1.Text;
            if (!MyUtility.Check.Empty(seq1))
            {
                DataTable filter = GetFilterAllSeqRefnoColor(this.Poid, this.txtSeq1.Text, this.txtSeq2.Text, string.Empty, string.Empty);

                // Seq 會影響WK ETA，因此一併清除
                this.txtWKETA.Text = string.Empty;

                // 這邊的驗證無法取得詳細資訊，採用最低限度的條件，後續Confirm再詳細檢驗
                if (MyUtility.Check.Empty(filter) || filter.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox($@"Seq1: {seq1} data not found!");
                    this.txtSeq1.Text = string.Empty;
                    this.txtSeq1.Focus();
                    return;
                }
            }
            else
            {
                this.txtSeq1.Text = string.Empty;
            }
        }

        private void TxtSeq2_Validating(object sender, CancelEventArgs e)
        {
            string seq2 = this.txtSeq2.Text;
            if (!MyUtility.Check.Empty(seq2))
            {
                DataTable filter = GetFilterAllSeqRefnoColor(this.Poid, this.txtSeq1.Text, this.txtSeq2.Text, string.Empty, string.Empty);

                // Seq 會影響WK ETA，因此一併清除
                this.txtWKETA.Text = string.Empty;

                // 這邊的驗證無法取得詳細資訊，採用最低限度的條件，後續Confirm再詳細檢驗
                if (MyUtility.Check.Empty(filter) || filter.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox($@"Seq2: {seq2} data not found!");
                    this.txtSeq2.Text = string.Empty;
                    this.txtSeq2.Focus();
                    return;
                }
            }
        }

        private void TxtSeq1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            // 這邊的開窗無法取得詳細資訊，採用最低限度的條件，後續Confirm再詳細檢驗
            SelectItem item = PopupAllSeqRefnoColor(this.Poid);
            if (item == null)
            {
                return;
            }

            // Seq 會影響WK ETA，因此一併清除
            this.txtWKETA.Text = string.Empty;

            this.txtSeq1.Text = item.GetSelecteds()[0]["Seq1"].ToString();
            this.txtSeq2.Text = item.GetSelecteds()[0]["Seq2"].ToString();
        }

        private void TxtSeq2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            // 這邊的開窗無法取得詳細資訊，採用最低限度的條件，後續Confirm再詳細檢驗
            SelectItem item = PopupAllSeqRefnoColor(this.Poid);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            // Seq 會影響WK ETA，因此一併清除
            this.txtWKETA.Text = string.Empty;

            this.txtSeq1.Text = item.GetSelecteds()[0]["Seq1"].ToString();
            this.txtSeq2.Text = item.GetSelecteds()[0]["Seq2"].ToString();
        }

        private void TxtMakerLength_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtMarkerLength.Text != "Y  - / + \"")
            {
                this.txtMarkerLength.Text = SetMarkerLengthMaskString(this.txtMarkerLength.Text);
            }
        }

        private void TxtWKETA_Click(object sender, EventArgs e)
        {
            DataTable dt = this.dt_CurentDetail.Clone();
            DataRow dr = dt.NewRow();
            dr["ID"] = this.Poid;
            dr["Seq1"] = this.txtSeq1.Text;
            dr["Seq2"] = this.txtSeq2.Text;
            P02_WKETA item = new P02_WKETA(dr);
            DialogResult result = item.ShowDialog();
            switch (result)
            {
                case DialogResult.Cancel:
                    break;
                case DialogResult.Yes:
                    this.txtWKETA.Text = Itemx.WKETA.HasValue ? Itemx.WKETA.Value.ToString("yyyy/MM/dd") : string.Empty;
                    break;
                case DialogResult.No:
                    this.txtWKETA.Text = null;
                    break;
            }
        }
        #endregion

        #region Grid 單筆資料的欄位開窗/驗證
        private void GridEventSet()
        {
            // 可否編輯 && 顏色
            ConfigureColumnEvents(this.gridBatchAssign, this.CanEditDataByGrid);

            ConfigureSeqColumnEvents(this.col_Seq1, this.gridBatchAssign, this.CanEditData);
            ConfigureSeqColumnEvents(this.col_Seq2, this.gridBatchAssign, this.CanEditData);

            this.col_WKETA.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridBatchAssign.GetDataRow(e.RowIndex);

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

                e.CellStyle.BackColor = Color.Pink;
                e.CellStyle.ForeColor = Color.Red;
            };
        }
        #endregion

        private bool CanEditDataByGrid(Sci.Win.UI.Grid grid, DataRow dr, string columNname)
        {
            return true; // 能帶進來的資料必定是能編輯的
        }

        private bool CanEditData(DataRow dr)
        {
            return true; // 能帶進來的資料必定是能編輯的
        }
    }
}
