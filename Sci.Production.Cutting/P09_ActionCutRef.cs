using Ict;
using Ict.Win;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Data;
using System.Windows.Forms;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P09_ActionCutRef : Win.Tems.QueryForm
    {
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1401 // Elements should be documented
        private static CuttingForm formType = CuttingForm.P09;
        public string WorkType;
        public DialogAction Action;
        public DataRow CurrentDetailData_Ori; // save 才更新, 按 Cancel 不變更
        public DataTable dt_SizeRatio_Ori;
        public DataTable dt_Distribute_Ori;
        public DataTable dt_PatternPanel_Ori;
        public DataRow CurrentDetailData; // 此視窗編輯用
        public DataTable dt_SizeRatio; // 此視窗編輯用
        public DataTable dt_Distribute; // 此視窗編輯用
        public DataTable dt_PatternPanel; // 此視窗編輯用
        public bool editByUseCutRefToRequestFabric = true; // P09 useCutRefToRequestFabric = 1 為 False
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

        private string CuttingID;

        /// <inheritdoc/>
        public P09_ActionCutRef(bool canEditLayer)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.cmsPatternPanel.Enabled = true;
            this.cmsSizeRatio.Enabled = true;
            this.cmsDistribute.Enabled = true;
            this.txtCell.MDivisionID = Env.User.Keyword;
            this.numLayers.Enabled = canEditLayer;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.Text = $"P09. {this.Action} CutRef";
            this.btnModify.Text = this.Action == DialogAction.Edit ? "Save" : $"{this.Action}";
            base.OnFormLoaded();
            this.SetData();
            this.GridSetup();
            this.InitialEnable();
        }

        private void InitialEnable()
        {
            this.numCutno.ReadOnly = !this.editByUseCutRefToRequestFabric;
            this.numLayers.ReadOnly = !this.editByUseCutRefToRequestFabric;
            this.txtSeq1.ReadOnly = !this.editByUseCutRefToRequestFabric;
            this.txtSeq2.ReadOnly = !this.editByUseCutRefToRequestFabric;
            this.txtRefNo.ReadOnly = !this.editByUseCutRefToRequestFabric;
            this.txtColor.ReadOnly = !this.editByUseCutRefToRequestFabric;
            this.numConsPC.ReadOnly = !this.editByUseCutRefToRequestFabric;
            this.txtMarkerNo.ReadOnly = !this.editByUseCutRefToRequestFabric;
            this.txtActCuttingPerimeter.ReadOnly = !this.editByUseCutRefToRequestFabric;
            this.txtStraightLength.ReadOnly = !this.editByUseCutRefToRequestFabric;
            this.txtCurvedLength.ReadOnly = !this.editByUseCutRefToRequestFabric;
            this.txtDropDownList1.ReadOnly = !this.editByUseCutRefToRequestFabric;

            this.cmsSizeRatio.Enabled = this.editByUseCutRefToRequestFabric;
            this.cmsDistribute.Enabled = this.editByUseCutRefToRequestFabric;
            this.cmsPatternPanel.Enabled = this.editByUseCutRefToRequestFabric;

            this.gridSizeRatio.IsEditingReadOnly = !this.editByUseCutRefToRequestFabric;
            this.gridDistributeToSP.IsEditingReadOnly = !this.editByUseCutRefToRequestFabric;
            this.gridPatternPanel.IsEditingReadOnly = !this.editByUseCutRefToRequestFabric;
        }

        private void SetData()
        {
            this.CuttingID = this.CurrentDetailData["ID"].ToString();

            // 使用 BindingSource 進行綁定
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = this.CurrentDetailData.Table;
            bindingSource.Position = this.CurrentDetailData.Table.Rows.IndexOf(this.CurrentDetailData);

            this.numCutno.DataBindings.Add(new Binding("Value", bindingSource, "CutNo", true, DataSourceUpdateMode.OnPropertyChanged));
            this.numLayers.DataBindings.Add(new Binding("Value", bindingSource, "Layer", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtSeq1.DataBindings.Add(new Binding("Text", bindingSource, "SEQ1", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtSeq2.DataBindings.Add(new Binding("Text", bindingSource, "SEQ2", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtRefNo.DataBindings.Add(new Binding("Text", bindingSource, "RefNo", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtColor.DataBindings.Add(new Binding("Text", bindingSource, "ColorID", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtTone.DataBindings.Add(new Binding("Text", bindingSource, "Tone", true, DataSourceUpdateMode.OnPropertyChanged));
            this.numConsPC.DataBindings.Add(new Binding("Value", bindingSource, "ConsPC", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtMarkerName.DataBindings.Add(new Binding("Text", bindingSource, "MarkerName", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtMarkerNo.DataBindings.Add(new Binding("Text", bindingSource, "MarkerNo", true, DataSourceUpdateMode.OnPropertyChanged));
            this.dateBoxEstCutDate.DataBindings.Add(new Binding("Value", bindingSource, "EstCutDate", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtSpreadingNo.DataBindings.Add(new Binding("Text", bindingSource, "SpreadingNoID", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtCell.DataBindings.Add(new Binding("Text", bindingSource, "CutCellID", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtDropDownList1.DataBindings.Add(new Binding("Text", bindingSource, "Shift", true, DataSourceUpdateMode.OnPropertyChanged));

            this.txtMarkerLength.Text = Prgs.ConvertFullWidthToHalfWidth(FormatMarkerLength(this.CurrentDetailData_Ori["MarkerLength"].ToString()));
            this.txtActCuttingPerimeter.Text = Prgs.ConvertFullWidthToHalfWidth(FormatLengthData(this.CurrentDetailData_Ori["ActCuttingPerimeter_Mask"].ToString()));
            this.txtStraightLength.Text = Prgs.ConvertFullWidthToHalfWidth(FormatLengthData(this.CurrentDetailData_Ori["StraightLength_Mask"].ToString()));
            this.txtCurvedLength.Text = Prgs.ConvertFullWidthToHalfWidth(FormatLengthData(this.CurrentDetailData_Ori["CurvedLength_Mask"].ToString()));

            this.patternpanelbs.DataSource = this.dt_PatternPanel;
            this.sizeRatiobs.DataSource = this.dt_SizeRatio;
            this.distributebs.DataSource = this.dt_Distribute;
        }

        private void GridSetup()
        {
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
            this.dt_PatternPanel.Select("PatternPanel = '' OR FabricPanelCode = ''").Delete();
            this.dt_PatternPanel.AcceptChanges();

            // 把 PatternPanel 刪光, 這些要清空
            DataRow minFabricPanelCode = GetMinFabricPanelCode(this.CurrentDetailData_Ori, this.dt_PatternPanel, formType);
            if (minFabricPanelCode == null)
            {
                this.txtSeq1.Text = string.Empty;
                this.txtSeq2.Text = string.Empty;
                this.txtRefNo.Text = string.Empty;
                this.txtColor.Text = string.Empty;
                this.CurrentDetailData["SCIRefno"] = string.Empty;
            }

            // 把 CurrentDetailData 值填入 CurrentDetailData_Ori
            this.CurrentDetailData_Ori["CutNo"] = this.CurrentDetailData["CutNo"];
            this.CurrentDetailData_Ori["Layer"] = this.CurrentDetailData["Layer"];
            this.CurrentDetailData_Ori["SEQ1"] = this.CurrentDetailData["SEQ1"];
            this.CurrentDetailData_Ori["SEQ2"] = this.CurrentDetailData["SEQ2"];
            this.CurrentDetailData_Ori["RefNo"] = this.CurrentDetailData["RefNo"];
            this.CurrentDetailData_Ori["ColorID"] = this.CurrentDetailData["ColorID"];
            this.CurrentDetailData_Ori["Tone"] = this.CurrentDetailData["Tone"];
            this.CurrentDetailData_Ori["ConsPC"] = this.CurrentDetailData["ConsPC"];
            this.CurrentDetailData_Ori["MarkerName"] = this.CurrentDetailData["MarkerName"];
            this.CurrentDetailData_Ori["MarkerNo"] = this.CurrentDetailData["MarkerNo"];
            this.CurrentDetailData_Ori["EstCutDate"] = this.CurrentDetailData["EstCutDate"];
            this.CurrentDetailData_Ori["SpreadingNoID"] = this.CurrentDetailData["SpreadingNoID"];
            this.CurrentDetailData_Ori["CutCellID"] = this.CurrentDetailData["CutCellID"];
            this.CurrentDetailData_Ori["Shift"] = this.CurrentDetailData["Shift"];
            this.CurrentDetailData_Ori["SCIRefno"] = this.CurrentDetailData["SCIRefno"];
            this.CurrentDetailData_Ori["Cons"] = this.CurrentDetailData["Cons"];

            this.CurrentDetailData_Ori["MarkerLength"] = this.CurrentDetailData_Ori["MarkerLength_Mask"] = this.txtMarkerLength.FullText;
            this.CurrentDetailData_Ori["ActCuttingPerimeter"] = this.CurrentDetailData_Ori["ActCuttingPerimeter_Mask"] = this.txtActCuttingPerimeter.Text == "Yd  \"" ? string.Empty : this.txtActCuttingPerimeter.Text;
            this.CurrentDetailData_Ori["StraightLength"] = this.CurrentDetailData_Ori["StraightLength_Mask"] = this.txtStraightLength.Text == "Yd  \"" ? string.Empty : this.txtStraightLength.Text;
            this.CurrentDetailData_Ori["CurvedLength"] = this.CurrentDetailData_Ori["CurvedLength_Mask"] = this.txtCurvedLength.Text == "Yd  \"" ? string.Empty : this.txtCurvedLength.Text;

            UpdateConcatString(this.CurrentDetailData_Ori, this.dt_SizeRatio, formType);
            UpdateMinOrderID(this.WorkType, this.CurrentDetailData_Ori, this.dt_Distribute, formType);
            UpdateArticle_CONCAT(this.CurrentDetailData_Ori, this.dt_Distribute, formType);
            UpdateTotalDistributeQty(this.CurrentDetailData_Ori, this.dt_Distribute, formType);
            UpdateMinSewinline(this.CurrentDetailData_Ori, this.dt_Distribute, formType);
            UpdatebyPatternPanel(this.CurrentDetailData_Ori, this.dt_PatternPanel, formType);

            this.CurrentDetailData_Ori.EndEdit();

            // Edit 先刪除, 再把修改的塞回去
            string filter = GetFilter(this.CurrentDetailData_Ori, formType);
            this.dt_SizeRatio_Ori.Select(filter).Delete();
            this.dt_Distribute_Ori.Select(filter).Delete();
            this.dt_PatternPanel_Ori.Select(filter).Delete();

            this.dt_SizeRatio_Ori.Merge(this.dt_SizeRatio);
            this.dt_Distribute_Ori.Merge(this.dt_Distribute);
            this.dt_PatternPanel_Ori.Merge(this.dt_PatternPanel);
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
            DataRow dr = this.CurrentDetailData;

            int layer = MyUtility.Convert.GetInt(dr["Layer"]);
            UpdateExcess(dr, layer, this.dt_SizeRatio, this.dt_Distribute, formType);

            dr["Cons"] = CalculateCons(dr, MyUtility.Convert.GetDecimal(dr["ConsPC"]), MyUtility.Convert.GetDecimal(dr["Layer"]), this.dt_SizeRatio, formType);
            UpdateConcatString(dr, this.dt_SizeRatio, formType);
            dr.EndEdit();
        }

        private void TxtSeq_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            DataRow minFabricPanelCode = GetMinFabricPanelCode(this.CurrentDetailData_Ori, this.dt_PatternPanel, formType);
            if (minFabricPanelCode == null)
            {
                MyUtility.Msg.WarningBox("Please select Pattern Panel first!");
                this.txtSeq1.Text = string.Empty;
                this.txtSeq2.Text = string.Empty;
                this.txtRefNo.Text = string.Empty;
                this.txtColor.Text = string.Empty;
                this.CurrentDetailData["SCIRefno"] = string.Empty;
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
            this.CurrentDetailData["SCIRefno"] = selectItem.GetSelecteds()[0]["SCIRefno"].ToString();
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

            DataRow minFabricPanelCode = GetMinFabricPanelCode(this.CurrentDetailData_Ori, this.dt_PatternPanel, formType);
            if (minFabricPanelCode == null)
            {
                MyUtility.Msg.WarningBox("Please select Pattern Panel first!");
                this.txtSeq1.Text = string.Empty;
                this.txtSeq2.Text = string.Empty;
                this.txtRefNo.Text = string.Empty;
                this.txtColor.Text = string.Empty;
                this.CurrentDetailData["SCIRefno"] = string.Empty;
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

                this.CurrentDetailData["SCIRefno"] = string.Empty;
            }

            // 唯一值時
            if (dtValidating.Rows.Count == 1)
            {
                this.CurrentDetailData["SCIRefno"] = dtValidating.Rows[0]["SCIRefno"].ToString();
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
            //this.txtCell.Text = selectItem.GetSelecteds()[0]["CutCellID"].ToString();
        }

        private void TxtSpreadingNo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ValidatingSpreadingNo(this.txtSpreadingNo.Text, out DataRow drV))
            {
                this.txtSpreadingNo.Text = string.Empty;
                e.Cancel = true;
            }

            if (drV == null)
            {
                return;
            }

            //this.txtCell.Text = MyUtility.Convert.GetString(drV["CutCellID"]);
        }

        private void NumConsPC_Validated(object sender, EventArgs e)
        {
            this.CurrentDetailData["Cons"] = CalculateCons(this.CurrentDetailData, MyUtility.Convert.GetDecimal(this.CurrentDetailData["ConsPC"]), MyUtility.Convert.GetDecimal(this.CurrentDetailData["Layer"]), this.dt_SizeRatio, formType);
        }

        private void TxtMarkerLength_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.CurrentDetailData["ConsPC"] = CalculateConsPC(this.txtMarkerLength.FullText, this.CurrentDetailData, this.dt_SizeRatio, formType);
            this.CurrentDetailData["Cons"] = CalculateCons(this.CurrentDetailData, MyUtility.Convert.GetDecimal(this.CurrentDetailData["ConsPC"]), MyUtility.Convert.GetDecimal(this.CurrentDetailData["Layer"]), this.dt_SizeRatio, formType);
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
        #endregion

        #region Grid 欄位事件 顏色/開窗/驗證 PS:編輯後"只顯示", 按下 Edit/Create 才將值更新到P09主表 this.CurrentDetailData
        private void GridEventSet()
        {
            ConfigureColumnEvents(this.gridSizeRatio, this.CanEditDataByGrid);
            ConfigureColumnEvents(this.gridDistributeToSP, this.CanEditDataByGrid);
            ConfigureColumnEvents(this.gridPatternPanel, this.CanEditDataByGrid);

            // "不"更新(CurrentDetailData)，只有在按下 Edit/Create 才更新
            BindPatternPanelEvents(this.col_PatternPanel, this.CuttingID);
            BindPatternPanelEvents(this.col_FabricPanelCode, this.CuttingID);

            #region SizeRatio
            this.col_SizeRatio_Size.EditingMouseDown += (s, e) =>
            {
                SizeCodeCellEditingMouseDown(e, this.gridSizeRatio, this.CurrentDetailData_Ori, this.dt_Distribute, formType, MyUtility.Convert.GetInt(this.numLayers.Value));
            };
            this.col_SizeRatio_Size.CellValidating += (s, e) =>
            {
                SizeCodeCellValidating(e, this.gridSizeRatio, this.CurrentDetailData_Ori, this.dt_Distribute, formType, MyUtility.Convert.GetInt(this.numLayers.Value));
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
                if (QtyCellValidating(e, this.CurrentDetailData_Ori, grid, this.dt_SizeRatio, this.dt_Distribute, formType, MyUtility.Convert.GetInt(this.numLayers.Value)))
                {
                    if (grid.Name == "gridSizeRatio")
                    {
                        this.CurrentDetailData["ConsPC"] = CalculateConsPC(this.CurrentDetailData, MyUtility.Convert.GetDecimal(this.CurrentDetailData["Cons"]), MyUtility.Convert.GetDecimal(this.CurrentDetailData["Layer"]), this.dt_SizeRatio, formType);
                    }
                }
            };
        }

        private void BindDistributeEvents(Ict.Win.UI.DataGridViewTextBoxColumn column)
        {
            column.EditingMouseDown += (s, e) =>
            {
                Distribute3CellEditingMouseDown(e, this.CurrentDetailData_Ori, this.dt_SizeRatio, this.gridDistributeToSP, formType, MyUtility.Convert.GetInt(this.numLayers.Value));
            };
            column.CellValidating += (s, e) =>
            {
                Distribute3CellValidating(e, this.CurrentDetailData_Ori, this.dt_SizeRatio, this.gridDistributeToSP, formType, MyUtility.Convert.GetInt(this.numLayers.Value));
            };
        }
        #endregion

        #region Grid 右鍵 Menu
        private void MenuItemInsertPatternPanel_Click(object sender, EventArgs e)
        {
            DataRow newrow = this.dt_PatternPanel.NewRow();
            newrow["ID"] = this.CurrentDetailData_Ori["ID"];
            newrow["WorkOrderForOutputUkey"] = this.CurrentDetailData_Ori["Ukey"];
            newrow["tmpKey"] = this.CurrentDetailData_Ori["tmpKey"];
            newrow["PatternPanel"] = string.Empty;
            newrow["FabricPanelCode"] = string.Empty;
            this.dt_PatternPanel.Rows.Add(newrow);
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
            DataRow newrow = this.dt_SizeRatio.NewRow();
            newrow["ID"] = this.CurrentDetailData_Ori["ID"];
            newrow["WorkOrderForOutputUkey"] = this.CurrentDetailData_Ori["Ukey"];
            newrow["tmpKey"] = this.CurrentDetailData_Ori["tmpKey"];
            newrow["Qty"] = 0;
            newrow["SizeCode"] = string.Empty;
            this.dt_SizeRatio.Rows.Add(newrow);
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
            string filter = GetFilter(this.CurrentDetailData_Ori, formType);
            this.dt_Distribute.Select(filter + $" AND SizeCode = '{sizeCode}'").Delete();

            // 後刪除 SizeRatio
            dr.Delete();

            UpdateExcess(this.CurrentDetailData_Ori, MyUtility.Convert.GetInt(this.numLayers.Value), this.dt_SizeRatio, this.dt_Distribute, formType);
        }

        private void MenuItemInsertDistribute_Click(object sender, EventArgs e)
        {
            if (this.dt_SizeRatio.DefaultView.ToTable().Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Please insert size ratio data first!");
                return;
            }

            DataRow newrow = this.dt_Distribute.NewRow();
            newrow["ID"] = this.CurrentDetailData_Ori["ID"];
            newrow["WorkOrderForOutputUkey"] = this.CurrentDetailData_Ori["Ukey"];
            newrow["tmpKey"] = this.CurrentDetailData_Ori["tmpKey"];
            newrow["Qty"] = 0;
            newrow["OrderID"] = string.Empty;
            newrow["Article"] = string.Empty;
            newrow["SizeCode"] = string.Empty;
            this.dt_Distribute.Rows.Add(newrow);
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

            UpdateExcess(this.CurrentDetailData_Ori, MyUtility.Convert.GetInt(this.numLayers.Value), this.dt_SizeRatio, this.dt_Distribute, formType);
        }
        #endregion

        private bool CanEditDataByGrid(Sci.Win.UI.Grid grid, DataRow dr, string columNname)
        {
            if (grid.Name == "gridDistributeToSP" && dr["OrderID"].ToString().EqualString("EXCESS"))
            {
                return false;
            }

            return this.editByUseCutRefToRequestFabric;
        }
    }
}
