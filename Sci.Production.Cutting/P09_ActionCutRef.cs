using Ict;
using Ict.Win;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P09_ActionCutRef : Win.Tems.QueryForm
    {
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1401 // Elements should be documented
        public string WorkType;
        public DialogAction Action;
        public DataRow CurrentDetailData;
        public DataTable dtWorkOrderForOutput_SizeRatio_Ori;
        public DataTable dtWorkOrderForOutput_Distribute_Ori;
        public DataTable dtWorkOrderForOutput_PatternPanel_Ori;
        public DataTable dtWorkOrderForOutput_SizeRatio; // 此視窗編輯用
        public DataTable dtWorkOrderForOutput_Distribute; // 此視窗編輯用
        public DataTable dtWorkOrderForOutput_PatternPanel; // 此視窗編輯用
#pragma warning restore SA1401 // Elements should be documented
#pragma warning restore SA1600 // Elements should be documented
        private Ict.Win.UI.DataGridViewTextBoxColumn col_PatternPanel;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_FabricPanelCode;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_SizeRatio_Size;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_SizeRatio_Qty;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Distribute_SP;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Distribute_Article;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Distribute_Size;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Distribute_Qty;

        private string ID;
        private string SCIRefno = string.Empty;

        /// <inheritdoc/>
        public P09_ActionCutRef()
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.cmsPatternPanel.Enabled = true;
            this.cmsSizeRatio.Enabled = true;
            this.cmsDistribute.Enabled = true;
            this.txtCell.MDivisionID = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.Text = $"P09. {this.Action} CutRef";
            this.btnModify.Text = $"{this.Action}";
            base.OnFormLoaded();
            this.SetData();
            this.GridSetup();
        }

        private void SetData()
        {
            this.ID = this.CurrentDetailData["ID"].ToString();
            this.numCutno.Value = (int?)this.CurrentDetailData["CutNo"];
            this.numLayers.Text = this.CurrentDetailData["Layer"].ToString();
            this.txtSeq1.Text = this.CurrentDetailData["SEQ1"].ToString();
            this.txtSeq2.Text = this.CurrentDetailData["SEQ2"].ToString();
            this.txtRefNo.Text = this.CurrentDetailData["RefNo"].ToString();
            this.txtColor.Text = this.CurrentDetailData["ColorID"].ToString();
            this.txtTone.Text = this.CurrentDetailData["Tone"].ToString();
            this.numConsPC.Text = this.CurrentDetailData["ConsPC"].ToString();
            this.txtMarkerName.Text = this.CurrentDetailData["MarkerName"].ToString();
            this.txtMarkerNo.Text = this.CurrentDetailData["MarkerNo"].ToString();
            this.txtMarkerLength.Text = this.CurrentDetailData["MarkerLength"].ToString();
            this.txtActCuttingPerimeter.Text = this.CurrentDetailData["ActCuttingPerimeter"].ToString();
            this.txtStraightLength.Text = this.CurrentDetailData["StraightLength"].ToString();
            this.txtCurvedLength.Text = this.CurrentDetailData["CurvedLength"].ToString();
            this.dateBoxEstCutDate.Value = MyUtility.Convert.GetDate(this.CurrentDetailData["EstCutDate"]);
            this.txtSpreadingNo.Text = this.CurrentDetailData["SpreadingNoID"].ToString();
            this.txtCell.Text = this.CurrentDetailData["CutCellID"].ToString();
            this.txtDropDownList1.Text = this.CurrentDetailData["Shift"].ToString();
            this.SCIRefno = this.CurrentDetailData["SCIRefno"].ToString();

            this.patternpanelbs.DataSource = this.dtWorkOrderForOutput_PatternPanel;
            this.sizeRatiobs.DataSource = this.dtWorkOrderForOutput_SizeRatio;
            this.distributebs.DataSource = this.dtWorkOrderForOutput_Distribute;
        }

        private void GridSetup()
        {
            this.gridPatternPanel.IsEditingReadOnly = false;
            this.gridSizeRatio.IsEditingReadOnly = false;
            this.gridDistributeToSP.IsEditingReadOnly = false;

            this.Helper.Controls.Grid.Generator(this.gridPatternPanel)
                .Text("PatternPanel", header: "Pattern Panel", width: Widths.AnsiChars(5)).Get(out this.col_PatternPanel)
                .Text("FabricPanelCode", header: "Fabric Panel Code ", width: Widths.AnsiChars(5)).Get(out this.col_FabricPanelCode)
                ;
            this.Helper.Controls.Grid.Generator(this.gridSizeRatio)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(3)).Get(out this.col_SizeRatio_Size)
                .Numeric("Qty", header: "Ratio", width: Widths.AnsiChars(6), integer_places: 5, maximum: 99999, minimum: 0).Get(out this.col_SizeRatio_Qty)
                ;
            this.Helper.Controls.Grid.Generator(this.gridDistributeToSP)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13)).Get(out this.col_Distribute_SP)
                .Text("Article", header: "Article", width: Widths.AnsiChars(7)).Get(out this.col_Distribute_Article)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(4)).Get(out this.col_Distribute_Size)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(3), integer_places: 6, maximum: 999999, minimum: 0).Get(out this.col_Distribute_Qty)
                .Date("SewInline", header: "Inline Date", width: Widths.AnsiChars(8), iseditingreadonly: true)
                ;

            // 能編輯才能開窗, 直接給 Pink
            this.gridPatternPanel.Columns["PatternPanel"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridPatternPanel.Columns["FabricPanelCode"].DefaultCellStyle.BackColor = Color.Pink;

            this.gridSizeRatio.Columns["SizeCode"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridSizeRatio.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;

            this.gridDistributeToSP.Columns["OrderID"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridDistributeToSP.Columns["Article"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridDistributeToSP.Columns["SizeCode"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridDistributeToSP.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;

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
            // 要先清除不完整資訊
            this.dtWorkOrderForOutput_PatternPanel.Select("PatternPanel = '' OR FabricPanelCode = ''").Delete();
            this.dtWorkOrderForOutput_PatternPanel.AcceptChanges();

            this.CurrentDetailData["CutNo"] = this.numCutno.Value ?? (object)DBNull.Value;
            this.CurrentDetailData["Layer"] = this.numLayers.Value;
            this.CurrentDetailData["SEQ1"] = this.txtSeq1.Text;
            this.CurrentDetailData["SEQ2"] = this.txtSeq2.Text;
            this.CurrentDetailData["RefNo"] = this.txtRefNo.Text;
            this.CurrentDetailData["ColorID"] = this.txtColor.Text;
            this.CurrentDetailData["Tone"] = this.txtTone.Text;
            this.CurrentDetailData["ConsPC"] = this.numConsPC.Value;
            this.CurrentDetailData["MarkerName"] = this.txtMarkerName.Text;
            this.CurrentDetailData["MarkerNo"] = this.txtMarkerNo.Text;
            this.CurrentDetailData["MarkerLength"] = this.CurrentDetailData["MarkerLength_Mask"] = this.txtMarkerLength.Text == "Y  - / + \"" ? string.Empty : this.txtMarkerLength.Text;
            this.CurrentDetailData["ActCuttingPerimeter"] = this.CurrentDetailData["ActCuttingPerimeter_Mask"] = this.txtActCuttingPerimeter.Text == "Yd  \"" ? string.Empty : this.txtActCuttingPerimeter.Text;
            this.CurrentDetailData["StraightLength"] = this.CurrentDetailData["StraightLength_Mask"] = this.txtStraightLength.Text == "Yd  \"" ? string.Empty : this.txtStraightLength.Text;
            this.CurrentDetailData["CurvedLength"] = this.CurrentDetailData["CurvedLength_Mask"] = this.txtCurvedLength.Text == "Yd  \"" ? string.Empty : this.txtCurvedLength.Text;
            this.CurrentDetailData["EstCutDate"] = this.dateBoxEstCutDate.Value ?? (object)DBNull.Value;
            this.CurrentDetailData["SpreadingNoID"] = this.txtSpreadingNo.Text;
            this.CurrentDetailData["CutCellID"] = this.txtCell.Text;
            this.CurrentDetailData["Shift"] = this.txtDropDownList1.Text;
            this.CurrentDetailData["SCIRefno"] = this.SCIRefno;
            this.CurrentDetailData["Cons"] = CalculateCons(this.CurrentDetailData, this.dtWorkOrderForOutput_SizeRatio, CuttingForm.P09);
            UpdateMinOrderID(this.WorkType, this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09);
            UpdateConcatString(this.CurrentDetailData, this.dtWorkOrderForOutput_SizeRatio, CuttingForm.P09);
            UpdateTotalDistributeQty(this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09);
            UpdateMinSewinline(this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09);
            UpdatebyPatternPanel(this.CurrentDetailData, this.dtWorkOrderForOutput_PatternPanel, CuttingForm.P09);

            this.CurrentDetailData.EndEdit();

            // Edit 先刪除, 再把修改的塞回去
            string filter = GetFilter(this.CurrentDetailData, CuttingForm.P09);
            this.dtWorkOrderForOutput_SizeRatio_Ori.Select(filter).Delete();
            this.dtWorkOrderForOutput_Distribute_Ori.Select(filter).Delete();
            this.dtWorkOrderForOutput_PatternPanel_Ori.Select(filter).Delete();

            this.dtWorkOrderForOutput_SizeRatio_Ori.Merge(this.dtWorkOrderForOutput_SizeRatio);
            this.dtWorkOrderForOutput_Distribute_Ori.Merge(this.dtWorkOrderForOutput_Distribute);
            this.dtWorkOrderForOutput_PatternPanel_Ori.Merge(this.dtWorkOrderForOutput_PatternPanel);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion

        #region 欄位 開窗/驗證 PS:編輯後"只顯示", 按下 Edit/Create 才將值更新到P09主表 this.CurrentDetailData

        private void NumLayers_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UpdateExcess(this.CurrentDetailData, MyUtility.Convert.GetInt(this.numLayers.Value), this.dtWorkOrderForOutput_SizeRatio, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09);
        }

        private void TxtSeq_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            DataRow minFabricPanelCode = GetMinFabricPanelCode(this.CurrentDetailData, this.dtWorkOrderForOutput_PatternPanel, CuttingForm.P09);
            if (minFabricPanelCode == null)
            {
                MyUtility.Msg.WarningBox("Please select Pattern Panel first!");
                return;
            }

            DataTable dt = GetPatternPanel(this.ID);
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
            SelectItem selectItem = PopupSEQ(this.ID, fabricCode, seq1, seq2, refno, colorID, iscolor);
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
            if (MyUtility.Check.Empty(newvalue) || newvalue == oldvalue)
            {
                return;
            }

            DataRow minFabricPanelCode = GetMinFabricPanelCode(this.CurrentDetailData, this.dtWorkOrderForOutput_PatternPanel, CuttingForm.P09);
            if (minFabricPanelCode == null)
            {
                MyUtility.Msg.WarningBox("Please select Pattern Panel first!");
                return;
            }

            DataTable dt = GetPatternPanel(this.ID);
            DataRow[] drs = dt.Select($"FabricPanelCode = '{minFabricPanelCode["FabricPanelCode"]}'");
            string fabricCode = drs[0]["FabricCode"].ToString(); // 一定找的到
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

            if (!ValidatingSEQ(this.ID, fabricCode, seq1, seq2, refno, colorID, out DataTable dtValidating))
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
            }

            // 唯一值時
            if (dtValidating.Rows.Count == 1)
            {
                this.SCIRefno = dtValidating.Rows[0]["SCIRefno"].ToString();
            }
        }

        private void TxtSpreadingNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            SelectItem selectItem = PopupSpreadingNo(this.txtSpreadingNo.Text);
            if (selectItem == null)
            {
                return;
            }

            this.txtSpreadingNo.Text = selectItem.GetSelectedString();
            this.txtCell.Text = selectItem.GetSelecteds()[0]["CutCellID"].ToString();
        }

        private void TxtSpreadingNo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ValidatingSpreadingNo(this.txtSpreadingNo.Text, out DataRow drV))
            {
                this.txtSpreadingNo.Text = string.Empty;
                e.Cancel = true;
            }

            this.txtCell.Text = drV["CutCellID"].ToString();
        }

        private void TxtMarkerLength_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.txtMarkerLength.Text != "Y  - / + \"")
            {
                this.txtMarkerLength.Text = SetMarkerLengthMaskString(this.txtMarkerLength.Text);
                this.numConsPC.Value = CalculateConsPC(this.txtMarkerLength.Text, this.CurrentDetailData, this.dtWorkOrderForOutput_SizeRatio, CuttingForm.P09);
            }
        }

        private void TxtMasked_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (((Sci.Win.UI.TextBox)sender).Text != "Yd  \"")
            {
                ((Sci.Win.UI.TextBox)sender).Text = SetMaskString(((Sci.Win.UI.TextBox)sender).Text);
            }
        }

        private void TxtMarkerNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            SelectItem selectItem = PopupMarkerNo(this.ID, this.txtMarkerNo.Text);
            if (selectItem == null)
            {
                return;
            }

            this.txtMarkerNo.Text = selectItem.GetSelectedString();
        }

        private void TxtMarkerNo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ValidatingMarkerNo(this.ID, this.txtMarkerNo.Text))
            {
                this.txtMarkerNo.Text = string.Empty;
                e.Cancel = true;
            }
        }
        #endregion

        #region Grid 欄位事件 顏色/開窗/驗證 PS:編輯後"只顯示", 按下 Edit/Create 才將值更新到P09主表 this.CurrentDetailData
        private void GridEventSet()
        {
            // "不"更新(CurrentDetailData)，只有在按下 Edit/Create 才更新
            BindPatternPanelEvents(this.col_PatternPanel, this.ID);
            BindPatternPanelEvents(this.col_FabricPanelCode, this.ID);

            #region SizeRatio
            this.col_SizeRatio_Size.EditingMouseDown += (s, e) =>
            {
                SizeCodeCellEditingMouseDown(e, this.gridSizeRatio, this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09, MyUtility.Convert.GetInt(this.numLayers.Value));
            };
            this.col_SizeRatio_Size.CellValidating += (s, e) =>
            {
                SizeCodeCellValidating(e, this.gridSizeRatio, this.CurrentDetailData, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09, MyUtility.Convert.GetInt(this.numLayers.Value));
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

        private void BindQtyEvents(Ict.Win.UI.DataGridViewNumericBoxColumn column)
        {
            column.CellValidating += (s, e) =>
            {
                Sci.Win.UI.Grid grid = (Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                QtyCellValidating(e, this.CurrentDetailData, this.gridDistributeToSP, this.dtWorkOrderForOutput_SizeRatio, this.dtWorkOrderForOutput_Distribute, CuttingForm.P09, MyUtility.Convert.GetInt(this.CurrentDetailData["Layer"]));
            };
        }

        private void BindDistributeEvents(Ict.Win.UI.DataGridViewTextBoxColumn column)
        {
            column.EditingMouseDown += (s, e) =>
            {
                Distribute3CellEditingMouseDown(e, this.CurrentDetailData, this.dtWorkOrderForOutput_SizeRatio, this.gridDistributeToSP, CuttingForm.P09, MyUtility.Convert.GetInt(this.numLayers.Value));
            };
            column.CellValidating += (s, e) =>
            {
                Distribute3CellValidating(e, this.CurrentDetailData, this.dtWorkOrderForOutput_SizeRatio, this.gridDistributeToSP, CuttingForm.P09, MyUtility.Convert.GetInt(this.numLayers.Value));
            };
        }
        #endregion

        #region Grid 右鍵 Menu
        private void MenuItemInsertPatternPanel_Click(object sender, EventArgs e)
        {
            DataRow newrow = this.dtWorkOrderForOutput_PatternPanel.NewRow();
            newrow["ID"] = this.CurrentDetailData["ID"];
            newrow["WorkOrderForOutputUkey"] = this.CurrentDetailData["Ukey"];
            newrow["tmpKey"] = this.CurrentDetailData["tmpKey"];
            this.dtWorkOrderForOutput_PatternPanel.Rows.Add(newrow);
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
            if (selectRow.Count == 0)
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
            if (selectRow.Count == 0)
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
    }
}
