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
        private readonly CuttingForm formType = CuttingForm.P02;
        public string WorkType;
        public DialogAction Action;
        public DataRow CurrentMaintain;
        public DataRow CurrentDetailData_Ori; // save 才更新, 按 Cancel 不變更
        public DataTable dt_SizeRatio_Ori;
        public DataTable dt_Distribute_Ori;
        public DataTable dt_PatternPanel_Ori;
        public DataRow CurrentDetailData; // 此視窗編輯用
        public DataTable dt_SizeRatio; // 此視窗編輯用
        public DataTable dt_Distribute; // 此視窗編輯用
        public DataTable dt_PatternPanel; // 此視窗編輯用
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
            this.btnModify.Text = this.Action == DialogAction.Edit ? "Save" : $"{this.Action}";
            base.OnFormLoaded();
            this.SetData();
            this.GridSetup();
            this.txtWKETA.ReadOnly = true;
            this.txtWKETA.BackColor = Color.White;
            this.txtWKETA.ForeColor = Color.Red;

            bool canEditDisturbute = this.CanEditDistribute();
            this.cmsDistribute.Enabled = canEditDisturbute;
            this.gridDistributeToSP.IsEditingReadOnly = !canEditDisturbute;
        }

        private void SetData()
        {
            // 去掉 Delete 的資料
            this.dt_SizeRatio.AcceptChanges();
            this.dt_Distribute.AcceptChanges();
            this.dt_PatternPanel.AcceptChanges();

            this.CuttingID = this.CurrentMaintain["ID"].ToString();

            // 使用 BindingSource 進行綁定
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = this.CurrentDetailData.Table;
            bindingSource.Position = this.CurrentDetailData.Table.Rows.IndexOf(this.CurrentDetailData);

            // 不要綁 Layer, 驗證會有新舊值檢查
            this.txtSP.DataBindings.Add(new Binding("Text", bindingSource, "OrderID", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtSeq1.DataBindings.Add(new Binding("Text", bindingSource, "SEQ1", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtSeq2.DataBindings.Add(new Binding("Text", bindingSource, "SEQ2", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtRefNo.DataBindings.Add(new Binding("Text", bindingSource, "RefNo", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtColor.DataBindings.Add(new Binding("Text", bindingSource, "ColorID", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtTone.DataBindings.Add(new Binding("Text", bindingSource, "Tone", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtSeq.DataBindings.Add(new Binding("Text", bindingSource, "Seq", true, DataSourceUpdateMode.OnPropertyChanged));
            this.numConsPC.DataBindings.Add(new Binding("Value", bindingSource, "ConsPC", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtMarkerName.DataBindings.Add(new Binding("Text", bindingSource, "MarkerName", true, DataSourceUpdateMode.OnPropertyChanged));
            this.txtMarkerNo.DataBindings.Add(new Binding("Text", bindingSource, "MarkerNo", true, DataSourceUpdateMode.OnPropertyChanged));
            this.dateBoxEstCutDate.DataBindings.Add(new Binding("Value", bindingSource, "EstCutDate", true, DataSourceUpdateMode.OnPropertyChanged));
            this.numLayers.Value = MyUtility.Convert.GetInt(this.CurrentDetailData["Layer"]);

            this.txtMarkerLength.Text = Prgs.ConvertFullWidthToHalfWidth(FormatMarkerLength(this.CurrentDetailData_Ori["MarkerLength"].ToString()));
            this.txtWKETA.Text = MyUtility.Convert.GetDate(this.CurrentDetailData_Ori["WKETA"]).HasValue ? MyUtility.Convert.GetDate(this.CurrentDetailData_Ori["WKETA"]).Value.ToString("yyyy/MM/dd") : string.Empty;

            this.txtSP.ReadOnly = MyUtility.Convert.GetString(this.CurrentMaintain["WorkType"]) != "2";

            this.patternpanelbs.DataSource = this.dt_PatternPanel;
            this.sizeRatiobs.DataSource = this.dt_SizeRatio;
            this.distributebs.DataSource = this.dt_Distribute;

            string filter = GetFilter(this.CurrentDetailData, this.formType);
            this.patternpanelbs.Filter = filter;
            this.sizeRatiobs.Filter = filter;
            this.distributebs.Filter = filter;
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
            this.dt_PatternPanel.Select("PatternPanel = '' OR FabricPanelCode = ''").Delete();
            this.dt_PatternPanel.AcceptChanges();

            // 把 PatternPanel 刪光, 這些要清空
            DataRow minFabricPanelCode = GetMinFabricPanelCode(this.CurrentDetailData_Ori, this.dt_PatternPanel, this.formType);
            if (minFabricPanelCode == null)
            {
                this.txtSeq1.Text = string.Empty;
                this.txtSeq2.Text = string.Empty;
                this.txtRefNo.Text = string.Empty;
                this.txtColor.Text = string.Empty;
                this.CurrentDetailData["SCIRefno"] = string.Empty;
            }

            this.CurrentDetailData_Ori["Layer"] = this.CurrentDetailData["Layer"];
            this.CurrentDetailData_Ori["OrderID"] = this.CurrentDetailData["OrderID"];
            this.CurrentDetailData_Ori["SEQ1"] = this.CurrentDetailData["SEQ1"];
            this.CurrentDetailData_Ori["SEQ2"] = this.CurrentDetailData["SEQ2"];
            this.CurrentDetailData_Ori["RefNo"] = this.CurrentDetailData["RefNo"];
            this.CurrentDetailData_Ori["ColorID"] = this.CurrentDetailData["ColorID"];
            this.CurrentDetailData_Ori["Tone"] = this.CurrentDetailData["Tone"];
            this.CurrentDetailData_Ori["Seq"] = this.CurrentDetailData["Seq"];
            this.CurrentDetailData_Ori["ConsPC"] = this.CurrentDetailData["ConsPC"];
            this.CurrentDetailData_Ori["MarkerName"] = this.CurrentDetailData["MarkerName"];
            this.CurrentDetailData_Ori["MarkerNo"] = this.CurrentDetailData["MarkerNo"];
            this.CurrentDetailData_Ori["SCIRefno"] = this.CurrentDetailData["SCIRefno"];
            this.CurrentDetailData_Ori["Cons"] = this.CurrentDetailData["Cons"];
            this.CurrentDetailData_Ori["EstCutDate"] = this.CurrentDetailData["EstCutDate"];

            this.CurrentDetailData_Ori["MarkerLength"] = this.CurrentDetailData_Ori["MarkerLength_Mask"] = this.txtMarkerLength.FullText;

            if (this.txtWKETA.Text == string.Empty)
            {
                this.CurrentDetailData_Ori["WKETA"] = DBNull.Value;
            }
            else
            {
                this.CurrentDetailData_Ori["WKETA"] = MyUtility.Convert.GetDate(this.txtWKETA.Text).Value;
            }

            // Size Ratio補上Layer和計算Ttl Qty
            string filter = GetFilter(this.CurrentDetailData_Ori, this.formType);
            this.dt_SizeRatio.Select(filter).AsEnumerable().ToList().ForEach(row =>
            {
                row["Layer"] = this.numLayers.Value;
                row["TotalCutQty_CONCAT"] = ConcatTTLCutQty(row);
            });

            UpdateConcatString(this.CurrentDetailData_Ori, this.dt_SizeRatio, this.formType);
            UpdateArticle_CONCAT(this.CurrentDetailData_Ori, this.dt_Distribute, this.formType);
            UpdateMinSewinline(this.CurrentDetailData_Ori, this.dt_Distribute, this.formType);
            UpdatebyPatternPanel(this.CurrentDetailData_Ori, this.dt_PatternPanel, this.formType);

            this.CurrentDetailData_Ori.EndEdit();

            // Edit 先刪除, 再把修改的塞回去
            this.dt_SizeRatio_Ori.Select(filter).Delete();
            this.dt_Distribute_Ori.Select(filter).Delete();
            this.dt_PatternPanel_Ori.Select(filter).Delete();

            this.dt_SizeRatio.Select(filter).AsEnumerable().ToList().ForEach(row => this.dt_SizeRatio_Ori.ImportRow(row));
            this.dt_Distribute.Select(filter).AsEnumerable().ToList().ForEach(row => this.dt_Distribute_Ori.ImportRow(row));
            this.dt_PatternPanel.Select(filter).AsEnumerable().ToList().ForEach(row => this.dt_PatternPanel_Ori.ImportRow(row));
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
                MyUtility.Msg.WarningBox("Only <By SP#> can modify.");
            }
        }

        private void TxtSP_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSP.Text) || this.txtSP.Text == this.CurrentDetailData_Ori["OrderID"].ToString())
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
            }
            else
            {
                MyUtility.Msg.WarningBox("Only <By SP#> can modify.");
                this.txtSP.Text = this.CurrentDetailData_Ori["OrderID"].ToString();
            }
        }

        private void TxtSeq_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            DataRow minFabricPanelCode = GetMinFabricPanelCode(this.CurrentDetailData_Ori, this.dt_PatternPanel, this.formType);
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

            DataRow minFabricPanelCode = GetMinFabricPanelCode(this.CurrentDetailData_Ori, this.dt_PatternPanel, this.formType);
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

        private void NumConsPC_Validated(object sender, EventArgs e)
        {
            this.CurrentDetailData["Cons"] = CalculateCons(this.CurrentDetailData, MyUtility.Convert.GetDecimal(this.CurrentDetailData["ConsPC"]), MyUtility.Convert.GetDecimal(this.CurrentDetailData["Layer"]), this.dt_SizeRatio, this.formType);
        }

        private void NumLayers_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            P02_ValidatingLayers(this.CurrentMaintain, this.CurrentDetailData, MyUtility.Convert.GetInt(this.numLayers.Value), this.dt_SizeRatio, this.dt_Distribute, this.dt_PatternPanel, this.formType);
        }

        private void TxtMarkerLength_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.numConsPC.Value = CalculateConsPC(this.txtMarkerLength.FullText, this.CurrentDetailData_Ori, this.dt_SizeRatio, this.formType);
            this.CurrentDetailData["Cons"] = CalculateCons(this.CurrentDetailData, MyUtility.Convert.GetDecimal(this.CurrentDetailData["ConsPC"]), MyUtility.Convert.GetDecimal(this.CurrentDetailData["Layer"]), this.dt_SizeRatio, this.formType);
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
            P02_WKETA item = new P02_WKETA(this.CurrentDetailData_Ori);
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
            ConfigureColumnEvents(this.gridDistributeToSP, this.CanEditDataByGrid);

            // "不"更新(CurrentDetailData)，只有在按下 Edit/Create 才更新
            BindPatternPanelEvents(this.col_PatternPanel, this.CuttingID);
            BindPatternPanelEvents(this.col_FabricPanelCode, this.CuttingID);

            #region SizeRatio
            this.col_SizeRatio_Size.EditingMouseDown += (s, e) =>
            {
                SizeCodeCellEditingMouseDown(e, this.gridSizeRatio, this.CurrentDetailData_Ori, this.dt_Distribute, this.formType);
            };
            this.col_SizeRatio_Size.CellValidating += (s, e) =>
            {
                SizeCodeCellValidating(e, this.gridSizeRatio, this.CurrentDetailData_Ori, this.dt_Distribute, this.formType);
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

        private void BindQtyEvents(Ict.Win.UI.DataGridViewNumericBoxColumn column)
        {
            column.CellValidating += (s, e) =>
            {
                Sci.Win.UI.Grid grid = (Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                QtyCellValidating(e, this.CurrentDetailData_Ori, grid, this.dt_SizeRatio, this.dt_Distribute, this.formType, MyUtility.Convert.GetInt(this.numLayers.Value));
            };
        }

        private void BindDistributeEvents(Ict.Win.UI.DataGridViewTextBoxColumn column)
        {
            column.EditingMouseDown += (s, e) =>
            {
                Distribute3CellEditingMouseDown(e, this.CurrentDetailData_Ori, this.dt_SizeRatio, this.gridDistributeToSP, this.formType, MyUtility.Convert.GetInt(this.numLayers.Value));
            };
            column.CellValidating += (s, e) =>
            {
                Distribute3CellValidating(e, this.CurrentDetailData_Ori, this.dt_SizeRatio, this.gridDistributeToSP, this.formType, MyUtility.Convert.GetInt(this.numLayers.Value));
            };
        }
        #endregion

        #region Grid 右鍵 Menu
        private void MenuItemInsertPatternPanel_Click(object sender, EventArgs e)
        {
            DataRow newrow = this.dt_PatternPanel.NewRow();
            newrow["ID"] = this.CurrentDetailData_Ori["ID"];
            newrow["WorkOrderForPlanningUkey"] = this.CurrentDetailData_Ori["Ukey"];
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
            newrow["WorkOrderForPlanningUkey"] = this.CurrentDetailData_Ori["Ukey"];
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
            string filter = GetFilter(this.CurrentDetailData_Ori, this.formType);
            this.dt_Distribute.Select(filter + $" AND SizeCode = '{sizeCode}'").Delete();

            // 後刪除 SizeRatio
            dr.Delete();

            UpdateExcess(this.CurrentDetailData_Ori, MyUtility.Convert.GetInt(this.numLayers.Value), this.dt_SizeRatio, this.dt_Distribute, this.formType);
        }

        private void MenuItemInsertDistribute_Click(object sender, EventArgs e)
        {
            if (!this.CanEditDistribute())
            {
                return;
            }

            if (this.dt_SizeRatio.DefaultView.ToTable().Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Please insert size ratio data first!");
                return;
            }

            DataRow newrow = this.dt_Distribute.NewRow();
            newrow["ID"] = this.CurrentDetailData_Ori["ID"];
            newrow["WorkOrderForPlanningUkey"] = this.CurrentDetailData_Ori["Ukey"];
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
            if (!this.CanEditDistribute() || selectRow.Count == 0)
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

            UpdateExcess(this.CurrentDetailData_Ori, MyUtility.Convert.GetInt(this.numLayers.Value), this.dt_SizeRatio, this.dt_Distribute, this.formType);
        }
        #endregion

        private bool CanEditDataByGrid(Sci.Win.UI.Grid grid, DataRow dr, string columNname)
        {
            switch (grid.Name)
            {
                case "gridDistributeToSP":
                    if (dr["OrderID"].ToString().EqualString("EXCESS"))
                    {
                        return false;
                    }

                    return this.CanEditDistribute();
                default:
                    return true;
            }
        }

        private bool CanEditDistribute()
        {
            int useCutRefToRequestFabric = MyUtility.Convert.GetInt(this.CurrentMaintain["UseCutRefToRequestFabric"]);
            return useCutRefToRequestFabric != 0 && useCutRefToRequestFabric != 2;
        }
    }
}
