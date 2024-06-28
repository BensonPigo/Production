using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P02_ActionCutRef : Win.Tems.QueryForm
    {
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1401 // Elements should be documented
        public DialogAction Action;
        public DataRow CurrentMaintain;
        public DataRow CurrentDetailData;
        public DataTable dtWorkOrderForPlanning_SizeRatio_Ori;
        public DataTable dtWorkOrderForPlanning_PatternPanel_Ori;
        public DataTable dtWorkOrderForPlanning_SizeRatio;
        public DataTable dtWorkOrderForPlanning_PatternPanel;
        public DataTable dtWorkOrderForPlanning_OrderList;
#pragma warning restore SA1401 // Elements should be documented
#pragma warning restore SA1600 // Elements should be documented

        private Ict.Win.UI.DataGridViewTextBoxColumn col_PatternPanel;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_FabricPanelCode;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_SizeRatio_Size;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_SizeRatio_Qty;

        private string CuttingID;
        private string SCIRefno = string.Empty;

        /// <inheritdoc/>
        public P02_ActionCutRef()
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.cmsPatternPanel.Enabled = true;
            this.cmsSizeRatio.Enabled = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.Text = $"P02. {this.Action} CutRef";
            this.btnModify.Text = $"{this.Action}";
            base.OnFormLoaded();
            this.SetData();
            this.GridSetup();
            this.txtWKETA.ReadOnly = true;
            this.txtWKETA.BackColor = Color.White;
            this.txtWKETA.ForeColor = Color.Red;
        }

        private void SetData()
        {
            this.CuttingID = this.CurrentMaintain["ID"].ToString();
            this.numLayers.Text = this.CurrentDetailData["Layer"].ToString();
            this.txtSP.Text = this.CurrentDetailData["OrderID"].ToString();
            this.txtSeq1.Text = this.CurrentDetailData["SEQ1"].ToString();
            this.txtSeq2.Text = this.CurrentDetailData["SEQ2"].ToString();
            this.txtRefNo.Text = this.CurrentDetailData["RefNo"].ToString();
            this.txtColor.Text = this.CurrentDetailData["ColorID"].ToString();
            this.txtTone.Text = this.CurrentDetailData["Tone"].ToString();
            this.numConsPC.Text = this.CurrentDetailData["ConsPC"].ToString();
            this.txtMarkerName.Text = this.CurrentDetailData["MarkerName"].ToString();
            this.txtMarkerNo.Text = this.CurrentDetailData["MarkerNo"].ToString();
            this.txtMarkerLength.Text = this.CurrentDetailData["MarkerLength"].ToString();
            this.dateBoxEstCutDate.Value = MyUtility.Convert.GetDate(this.CurrentDetailData["EstCutDate"]);
            this.txtWKETA.Text = MyUtility.Convert.GetDate(this.CurrentDetailData["WKETA"]).HasValue ? MyUtility.Convert.GetDate(this.CurrentDetailData["WKETA"]).Value.ToString("yyyy/MM/dd") : string.Empty;
            this.SCIRefno = this.CurrentDetailData["SCIRefno"].ToString();

            this.patternpanelbs.DataSource = this.dtWorkOrderForPlanning_PatternPanel;
            this.sizeRatiobs.DataSource = this.dtWorkOrderForPlanning_SizeRatio;
            this.orderList.DataSource = this.dtWorkOrderForPlanning_OrderList;
        }

        private void GridSetup()
        {
            this.gridPatternPanel.IsEditingReadOnly = false;
            this.gridSizeRatio.IsEditingReadOnly = false;
            this.gridOrderList.IsEditingReadOnly = false;

            this.Helper.Controls.Grid.Generator(this.gridPatternPanel)
                .Text("PatternPanel", header: "Pattern Panel", width: Widths.AnsiChars(5)).Get(out this.col_PatternPanel)
                .Text("FabricPanelCode", header: "Fabric Panel Code ", width: Widths.AnsiChars(5)).Get(out this.col_FabricPanelCode)
                ;
            this.Helper.Controls.Grid.Generator(this.gridSizeRatio)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(3)).Get(out this.col_SizeRatio_Size)
                .Numeric("Qty", header: "Ratio", width: Widths.AnsiChars(6), integer_places: 5, maximum: 99999, minimum: 0).Get(out this.col_SizeRatio_Qty)
                ;
            this.Helper.Controls.Grid.Generator(this.gridOrderList)
                .Date("InlineDate", header: "Inline Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SP", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                ;

            this.gridPatternPanel.Columns["PatternPanel"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridPatternPanel.Columns["FabricPanelCode"].DefaultCellStyle.BackColor = Color.Pink;

            this.gridSizeRatio.Columns["SizeCode"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridSizeRatio.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;

            this.GridEventSet();
        }

        #region 確認或取消 → 關閉視窗
        private void BtnModify_Click(object sender, EventArgs e)
        {
            this.UpdateToDetail();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void UpdateToDetail()
        {
            this.dtWorkOrderForPlanning_PatternPanel.Select("PatternPanel = '' OR FabricPanelCode = ''").Delete();
            this.dtWorkOrderForPlanning_PatternPanel.AcceptChanges();

            // 把 PatternPanel 刪光, 這些要清空
            DataRow minFabricPanelCode = GetMinFabricPanelCode(this.CurrentDetailData, this.dtWorkOrderForPlanning_PatternPanel, CuttingForm.P02);
            if (minFabricPanelCode == null)
            {
                this.txtSeq1.Text = string.Empty;
                this.txtSeq2.Text = string.Empty;
                this.txtRefNo.Text = string.Empty;
                this.txtColor.Text = string.Empty;
                this.SCIRefno = string.Empty;
            }

            this.CurrentDetailData["Layer"] = this.numLayers.Value;
            this.CurrentDetailData["OrderID"] = this.txtSP.Text;
            this.CurrentDetailData["SEQ1"] = this.txtSeq1.Text;
            this.CurrentDetailData["SEQ2"] = this.txtSeq2.Text;
            this.CurrentDetailData["RefNo"] = this.txtRefNo.Text;
            this.CurrentDetailData["ColorID"] = this.txtColor.Text;
            this.CurrentDetailData["Tone"] = this.txtTone.Text;
            this.CurrentDetailData["ConsPC"] = this.numConsPC.Value;
            this.CurrentDetailData["MarkerName"] = this.txtMarkerName.Text;
            this.CurrentDetailData["MarkerNo"] = this.txtMarkerNo.Text;
            this.CurrentDetailData["MarkerLength"] = this.CurrentDetailData["MarkerLength_Mask"] = this.txtMarkerLength.Text == "Y  - / + \"" ? string.Empty : this.txtMarkerLength.Text;
            this.CurrentDetailData["SCIRefno"] = this.SCIRefno;
            this.CurrentDetailData["EstCutDate"] = this.dateBoxEstCutDate.Value ?? (object)DBNull.Value;

            if (this.txtWKETA.Text == string.Empty)
            {
                this.CurrentDetailData["WKETA"] = DBNull.Value;
            }
            else
            {
                this.CurrentDetailData["WKETA"] = MyUtility.Convert.GetDate(this.txtWKETA.Text).Value;
            }

            // Size Ratio補上Layer和計算Ttl Qty
            if (this.dtWorkOrderForPlanning_SizeRatio.Rows.Count > 0)
            {
                this.dtWorkOrderForPlanning_SizeRatio.Select().ToList<DataRow>().ForEach(row =>
                {
                    row["Layer"] = this.numLayers.Value;

                    string cutQtystr = row["SizeCode"].ToString().Trim() + "/" + (MyUtility.Convert.GetDecimal(row["Qty"]) * MyUtility.Convert.GetDecimal(MyUtility.Check.Empty(row["Layer"]) ? 0 : row["Layer"])).ToString();
                    row["TotalCutQty_CONCAT"] = cutQtystr;
                });
            }

            UpdateConcatString(this.CurrentDetailData, this.dtWorkOrderForPlanning_SizeRatio, CuttingForm.P02);
            UpdatebyPatternPanel(this.CurrentDetailData, this.dtWorkOrderForPlanning_PatternPanel, CuttingForm.P02);

            this.CurrentDetailData.EndEdit();

            // Edit 先刪除, 再把修改的塞回去
            string filter = GetFilter(this.CurrentDetailData, CuttingForm.P02);
            this.dtWorkOrderForPlanning_SizeRatio_Ori.Select(filter).Delete();
            this.dtWorkOrderForPlanning_PatternPanel_Ori.Select(filter).Delete();

            this.dtWorkOrderForPlanning_SizeRatio_Ori.Merge(this.dtWorkOrderForPlanning_SizeRatio);
            this.dtWorkOrderForPlanning_PatternPanel_Ori.Merge(this.dtWorkOrderForPlanning_PatternPanel);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion

        #region 欄位 開窗/驗證 PS:編輯後"只顯示", 按下 Edit/Create 才將值更新到P02主表上
        private void TxtSP_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["WorkType"]) == "2")
            {
                string cmd = $@"SELECT ID FROM Orders WHERE POID = '{this.CuttingID}' AND Junk=0";
                DBProxy.Current.Select(null, cmd, out DataTable dtSP);

                if (dtSP == null)
                {
                    return;
                }

                SelectItem sele = new SelectItem(dtSP, "ID", "20", this.txtSP.Text, false, ",");
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.txtSP.Text = MyUtility.Convert.GetString(sele.GetSelecteds()[0]["ID"]);
            }
            else
            {
                MyUtility.Msg.WarningBox("Only <By SP#> can use.");
                this.txtSP.Text = string.Empty;
            }
        }

        private void TxtSP_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSP.Text))
            {
                return;
            }

            if (MyUtility.Convert.GetString(this.CurrentMaintain["WorkType"]) == "2")
            {
                string cmd = $@"SELECT ID FROM Orders WHERE POID = '{this.CuttingID}' AND Junk=0 AND ID = @ID";
                DBProxy.Current.Select(null, cmd, new List<SqlParameter>() { new SqlParameter("@ID", this.txtSP.Text) }, out DataTable dtSP);

                if (dtSP == null)
                {
                    return;
                }

                if (dtSP == null || dtSP.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox(string.Format("<SP#> : {0} data not found!", this.txtSP.Text));
                    this.txtSP.Text = string.Empty;
                }

                this.orderList.DataSource = this.dtWorkOrderForPlanning_OrderList.Select($@" SP = '{this.txtSP.Text}' ").CopyToDataTable();
            }
            else
            {
                MyUtility.Msg.WarningBox("Only <By SP#> can use.");
                this.txtSP.Text = string.Empty;
            }
        }

        private void TxtSeq_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            DataRow minFabricPanelCode = GetMinFabricPanelCode(this.CurrentDetailData, this.dtWorkOrderForPlanning_PatternPanel, CuttingForm.P02);
            if (minFabricPanelCode == null)
            {
                MyUtility.Msg.WarningBox("Please select Pattern Panel first!");
                this.txtSeq1.Text = string.Empty;
                this.txtSeq2.Text = string.Empty;
                this.txtRefNo.Text = string.Empty;
                this.txtColor.Text = string.Empty;
                this.SCIRefno = string.Empty;
                return;
            }

            DataTable dt = GetPatternPanel(this.CuttingID);
            DataRow[] drs = dt.Select($"FabricPanelCode = '{minFabricPanelCode["FabricPanelCode"]}'");
            string fabricCode = drs[0]["FabricCode"].ToString(); // 一定找的到
            string columnName = ((Win.UI.TextBox)sender).Name;
            string seq1 = this.txtSeq1.Text;
            string seq2 = this.txtSeq2.Text;
            string refno = this.txtRefNo.Text;
            string colorID = this.txtColor.Text;

            // 觸發的欄位不作為篩選條件
            switch (columnName)
            {
                case "txtSeq1":
                    seq1 = string.Empty;
                    break;
                case "txtSeq2":
                    seq2 = string.Empty;
                    break;
                case "txtRefNo":
                    refno = string.Empty;
                    break;
                case "txtColor":
                    colorID = string.Empty;
                    break;
            }

            bool iscolor = columnName.ToLower() == "txtcolor";
            SelectItem selectItem = PopupSEQ(this.CuttingID, fabricCode, seq1, seq2, refno, colorID, iscolor);
            if (selectItem == null)
            {
                return;
            }

            this.txtSeq1.Text = selectItem.GetSelecteds()[0]["SEQ1"].ToString();
            this.txtSeq2.Text = selectItem.GetSelecteds()[0]["SEQ2"].ToString();
            this.txtRefNo.Text = selectItem.GetSelecteds()[0]["Refno"].ToString();
            this.txtColor.Text = selectItem.GetSelecteds()[0]["ColorID"].ToString();
            this.SCIRefno = selectItem.GetSelecteds()[0]["SCIRefno"].ToString();
        }

        private void TxtSeq_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string newvalue = string.Empty;
            string oldvalue = ((Win.UI.TextBox)sender).OldValue;
            string columnName = ((Win.UI.TextBox)sender).Name;
            string seq1 = this.txtSeq1.Text;
            string seq2 = this.txtSeq2.Text;
            string refno = this.txtRefNo.Text;
            string colorID = this.txtColor.Text;

            switch (columnName)
            {
                case "txtSeq1":
                    newvalue = seq1;
                    break;
                case "txtSeq2":
                    newvalue = seq2;
                    break;
                case "txtRefNo":
                    newvalue = refno;
                    break;
                case "txtColor":
                    newvalue = colorID;
                    break;
            }

            if (MyUtility.Check.Empty(newvalue) || newvalue == oldvalue)
            {
                return;
            }

            DataRow minFabricPanelCode = GetMinFabricPanelCode(this.CurrentDetailData, this.dtWorkOrderForPlanning_PatternPanel, CuttingForm.P02);
            if (minFabricPanelCode == null)
            {
                MyUtility.Msg.WarningBox("Please select Pattern Panel first!");
                this.txtSeq1.Text = string.Empty;
                this.txtSeq2.Text = string.Empty;
                this.txtRefNo.Text = string.Empty;
                this.txtColor.Text = string.Empty;
                this.SCIRefno = string.Empty;
                return;
            }

            DataTable dt = GetPatternPanel(this.CuttingID);
            DataRow[] drs = dt.Select($"FabricPanelCode = '{minFabricPanelCode["FabricPanelCode"]}'");
            string fabricCode = drs[0]["FabricCode"].ToString(); // 一定找的到

            if (!ValidatingSEQ(this.CuttingID, fabricCode, seq1, seq2, refno, colorID, out DataTable dtValidating))
            {
                switch (columnName)
                {
                    case "txtSeq1":
                        this.txtSeq1.Text = string.Empty;
                        break;
                    case "txtSeq2":
                        this.txtSeq2.Text = string.Empty;
                        break;
                    case "txtRefNo":
                        this.txtRefNo.Text = string.Empty;
                        break;
                    case "txtColor":
                        this.txtColor.Text = string.Empty;
                        break;
                }

                this.SCIRefno = string.Empty;
            }

            // 唯一值時
            if (dtValidating.Rows.Count == 1)
            {
                this.SCIRefno = dtValidating.Rows[0]["SCIRefno"].ToString();
            }
        }

        private void TxtMarkerLength_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string markerLength = Prgs.SetMarkerLengthMaskString(this.txtMarkerLength.Text);
            this.txtMarkerLength.Text = markerLength;
        }

        private void TxtMarkerNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            SelectItem selectItem = PopupMarkerNo(this.CuttingID, this.txtMarkerNo.Text);
            if (selectItem == null)
            {
                return;
            }

            this.txtMarkerNo.Text = selectItem.GetSelectedString();
        }

        private void TxtMarkerNo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ValidatingMarkerNo(this.CuttingID, this.txtMarkerNo.Text))
            {
                this.txtMarkerNo.Text = string.Empty;
                e.Cancel = true;
            }
        }

        private void TxtWKETA_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            P02_WKETA item = new P02_WKETA(this.CurrentDetailData);
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

        #region Grid 欄位事件 顏色/開窗/驗證 PS:編輯後"只顯示", 按下 Edit/Create 才將值更新到P02主表上
        private void GridEventSet()
        {
            // "不"更新(CurrentDetailData)，只有在按下 Edit/Create 才更新
            BindPatternPanelEvents(this.col_PatternPanel, this.CuttingID);
            BindPatternPanelEvents(this.col_FabricPanelCode, this.CuttingID);

            #region SizeRatio
            this.col_SizeRatio_Size.EditingMouseDown += (s, e) =>
            {
                SizeCodeCellEditingMouseDown(e, this.gridSizeRatio, this.CurrentDetailData, null, CuttingForm.P02);
            };
            this.col_SizeRatio_Size.CellValidating += (s, e) =>
            {
                SizeCodeCellValidating(e, this.gridSizeRatio, this.CurrentDetailData, null, CuttingForm.P02);
            };
            this.col_SizeRatio_Qty.CellValidating += (s, e) =>
            {
                QtyCellValidating(e, this.CurrentDetailData, this.gridSizeRatio, this.dtWorkOrderForPlanning_SizeRatio, null, CuttingForm.P02);
            };
            this.BindQtyEvents(this.col_SizeRatio_Qty);
            #endregion

        }

        private void BindQtyEvents(Ict.Win.UI.DataGridViewNumericBoxColumn column)
        {
            column.CellValidating += (s, e) =>
            {
                Sci.Win.UI.Grid grid = (Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                QtyCellValidating(e, this.CurrentDetailData, this.gridSizeRatio, this.dtWorkOrderForPlanning_SizeRatio, null, CuttingForm.P02);
            };
        }
        #endregion

        #region Grid 右鍵 Menu
        private void MenuItemInsertPatternPanel_Click(object sender, EventArgs e)
        {
            DataRow newrow = this.dtWorkOrderForPlanning_PatternPanel.NewRow();
            newrow["ID"] = this.CurrentDetailData["ID"];
            newrow["WorkOrderForPlanningUkey"] = this.CurrentDetailData["Ukey"];
            newrow["tmpKey"] = this.CurrentDetailData["tmpKey"];
            this.dtWorkOrderForPlanning_PatternPanel.Rows.Add(newrow);
        }

        private void MenuItemDeletePatternPanel_Click(object sender, EventArgs e)
        {
            var selectRow = this.gridPatternPanel.GetSelecteds(SelectedSort.Index);
            if (selectRow.Count == 0)
            {
                return;
            }

            ((DataRowView)selectRow[0]).Row.Delete();
        }

        private void MenuItemInsertSizeRatio_Click(object sender, EventArgs e)
        {
            DataRow newrow = this.dtWorkOrderForPlanning_SizeRatio.NewRow();
            newrow["ID"] = this.CurrentDetailData["ID"];
            newrow["WorkOrderForPlanningUkey"] = this.CurrentDetailData["Ukey"];
            newrow["tmpKey"] = this.CurrentDetailData["tmpKey"];
            this.dtWorkOrderForPlanning_SizeRatio.Rows.Add(newrow);
        }

        private void MenuItemDeleteSizeRatio_Click(object sender, EventArgs e)
        {
            var selectRow = this.gridSizeRatio.GetSelecteds(SelectedSort.Index);
            if (selectRow.Count == 0)
            {
                return;
            }

            ((DataRowView)selectRow[0]).Row.Delete();
        }

        #endregion
    }
}
