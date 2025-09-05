namespace Sci.Production.Shipping
{
    partial class P02_AddNewItem
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.txtReceiver = new Sci.Win.UI.TextBox();
            this.labelReceiver = new Sci.Win.UI.Label();
            this.numNW = new Sci.Win.UI.NumericBox();
            this.txtunit_ftyUnit = new Sci.Production.Class.Txtunit_fty();
            this.labelUnit = new Sci.Win.UI.Label();
            this.numQty = new Sci.Win.UI.NumericBox();
            this.txtCTNNo = new Sci.Win.UI.TextBox();
            this.labelCTNNo = new Sci.Win.UI.Label();
            this.numPrice = new Sci.Win.UI.NumericBox();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.displaySPNo = new Sci.Win.UI.DisplayBox();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelRemark = new Sci.Win.UI.Label();
            this.labelTeamLeader = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelNW = new Sci.Win.UI.Label();
            this.labelQty = new Sci.Win.UI.Label();
            this.labelPrice = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtTeamLeader = new Sci.Win.UI.TextBox();
            this.displayTeamLeader = new Sci.Win.UI.DisplayBox();
            this.editRemark = new Sci.Production.Class.WatermarkEditBox();
            this.comboDoxItem = new Sci.Win.UI.ComboBox();
            this.labRefno = new Sci.Win.UI.Label();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.comboReason = new Sci.Win.UI.ComboBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtStyleName = new Sci.Win.UI.TextBox();
            this.labGender = new Sci.Win.UI.Label();
            this.txtAplType = new Sci.Win.UI.TextBox();
            this.labType = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.txtHscode = new Sci.Win.UI.TextBox();
            this.txtGender = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 376);
            this.btmcont.Size = new System.Drawing.Size(800, 40);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(710, 5);
            this.undo.TabIndex = 99;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(630, 5);
            this.save.TabIndex = 98;
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(316, 41);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(80, 23);
            this.labelStyle.TabIndex = 156;
            this.labelStyle.Text = "Style";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(532, 41);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(95, 23);
            this.labelSeason.TabIndex = 154;
            this.labelSeason.Text = "Season";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(316, 9);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(80, 23);
            this.labelBrand.TabIndex = 152;
            this.labelBrand.Text = "Brand";
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Category", true));
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(99, 8);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(109, 24);
            this.comboCategory.TabIndex = 1;
            this.comboCategory.SelectedIndexChanged += new System.EventHandler(this.ComboCategory_SelectedIndexChanged);
            this.comboCategory.SelectedValueChanged += new System.EventHandler(this.ComboCategory_SelectedValueChanged);
            this.comboCategory.Validated += new System.EventHandler(this.ComboCategory_Validated);
            // 
            // txtReceiver
            // 
            this.txtReceiver.BackColor = System.Drawing.Color.White;
            this.txtReceiver.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtReceiver.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Receiver", true));
            this.txtReceiver.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReceiver.Location = new System.Drawing.Point(399, 265);
            this.txtReceiver.Name = "txtReceiver";
            this.txtReceiver.Size = new System.Drawing.Size(130, 23);
            this.txtReceiver.TabIndex = 49;
            // 
            // labelReceiver
            // 
            this.labelReceiver.Location = new System.Drawing.Point(316, 265);
            this.labelReceiver.Name = "labelReceiver";
            this.labelReceiver.Size = new System.Drawing.Size(79, 23);
            this.labelReceiver.TabIndex = 149;
            this.labelReceiver.Text = "Receiver";
            // 
            // numNW
            // 
            this.numNW.BackColor = System.Drawing.Color.White;
            this.numNW.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NW", true));
            this.numNW.DecimalPlaces = 2;
            this.numNW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numNW.Location = new System.Drawing.Point(399, 232);
            this.numNW.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
            this.numNW.Name = "numNW";
            this.numNW.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numNW.Size = new System.Drawing.Size(60, 23);
            this.numNW.TabIndex = 43;
            this.numNW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtunit_ftyUnit
            // 
            this.txtunit_ftyUnit.BackColor = System.Drawing.Color.White;
            this.txtunit_ftyUnit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "UnitID", true));
            this.txtunit_ftyUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtunit_ftyUnit.Location = new System.Drawing.Point(236, 232);
            this.txtunit_ftyUnit.Name = "txtunit_ftyUnit";
            this.txtunit_ftyUnit.Size = new System.Drawing.Size(66, 23);
            this.txtunit_ftyUnit.TabIndex = 42;
            // 
            // labelUnit
            // 
            this.labelUnit.Location = new System.Drawing.Point(161, 232);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(70, 23);
            this.labelUnit.TabIndex = 148;
            this.labelUnit.Text = "Unit";
            // 
            // numQty
            // 
            this.numQty.BackColor = System.Drawing.Color.White;
            this.numQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Qty", true));
            this.numQty.DecimalPlaces = 2;
            this.numQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numQty.Location = new System.Drawing.Point(100, 232);
            this.numQty.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            131072});
            this.numQty.Name = "numQty";
            this.numQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numQty.Size = new System.Drawing.Size(57, 23);
            this.numQty.TabIndex = 41;
            this.numQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtCTNNo
            // 
            this.txtCTNNo.BackColor = System.Drawing.Color.White;
            this.txtCTNNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CTNNo", true));
            this.txtCTNNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCTNNo.Location = new System.Drawing.Point(606, 232);
            this.txtCTNNo.Name = "txtCTNNo";
            this.txtCTNNo.Size = new System.Drawing.Size(172, 23);
            this.txtCTNNo.TabIndex = 44;
            this.txtCTNNo.Validated += new System.EventHandler(this.TxtCTNNo_Validated);
            // 
            // labelCTNNo
            // 
            this.labelCTNNo.Location = new System.Drawing.Point(532, 232);
            this.labelCTNNo.Name = "labelCTNNo";
            this.labelCTNNo.Size = new System.Drawing.Size(71, 23);
            this.labelCTNNo.TabIndex = 147;
            this.labelCTNNo.Text = "CTN No.";
            // 
            // numPrice
            // 
            this.numPrice.BackColor = System.Drawing.Color.White;
            this.numPrice.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Price", true));
            this.numPrice.DecimalPlaces = 4;
            this.numPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numPrice.Location = new System.Drawing.Point(100, 199);
            this.numPrice.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            262144});
            this.numPrice.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPrice.Name = "numPrice";
            this.numPrice.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPrice.Size = new System.Drawing.Size(57, 23);
            this.numPrice.TabIndex = 31;
            this.numPrice.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(100, 107);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(678, 82);
            this.editDescription.TabIndex = 23;
            // 
            // displaySPNo
            // 
            this.displaySPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySPNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Seq1", true));
            this.displaySPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySPNo.Location = new System.Drawing.Point(272, 41);
            this.displaySPNo.Name = "displaySPNo";
            this.displaySPNo.Size = new System.Drawing.Size(30, 23);
            this.displaySPNo.TabIndex = 7;
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OrderID", true));
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(99, 41);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(169, 23);
            this.txtSPNo.TabIndex = 6;
            this.txtSPNo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSPNo_Validating);
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(9, 296);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(87, 23);
            this.labelRemark.TabIndex = 145;
            this.labelRemark.Text = "Remark";
            // 
            // labelTeamLeader
            // 
            this.labelTeamLeader.Location = new System.Drawing.Point(9, 265);
            this.labelTeamLeader.Name = "labelTeamLeader";
            this.labelTeamLeader.Size = new System.Drawing.Size(87, 23);
            this.labelTeamLeader.TabIndex = 144;
            this.labelTeamLeader.Text = "Team Leader";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(9, 9);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(87, 23);
            this.labelCategory.TabIndex = 143;
            this.labelCategory.Text = "Category";
            // 
            // labelNW
            // 
            this.labelNW.Location = new System.Drawing.Point(316, 232);
            this.labelNW.Name = "labelNW";
            this.labelNW.Size = new System.Drawing.Size(80, 23);
            this.labelNW.TabIndex = 142;
            this.labelNW.Text = "N.W. (kg)";
            // 
            // labelQty
            // 
            this.labelQty.Location = new System.Drawing.Point(9, 232);
            this.labelQty.Name = "labelQty";
            this.labelQty.Size = new System.Drawing.Size(87, 23);
            this.labelQty.TabIndex = 141;
            this.labelQty.Text = "Q\'ty";
            // 
            // labelPrice
            // 
            this.labelPrice.Location = new System.Drawing.Point(9, 199);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.Size = new System.Drawing.Size(87, 23);
            this.labelPrice.TabIndex = 140;
            this.labelPrice.Text = "Price";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(9, 107);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(87, 23);
            this.labelDescription.TabIndex = 139;
            this.labelDescription.Text = "Description";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(9, 41);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(87, 23);
            this.labelSPNo.TabIndex = 138;
            this.labelSPNo.Text = "SP#";
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SeasonID", true));
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(630, 41);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(80, 23);
            this.txtseason.TabIndex = 9;
            this.txtseason.Validated += new System.EventHandler(this.Txtseason_Validated);
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "StyleID", true));
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(399, 41);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.SeasonObjectName = null;
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 8;
            this.txtstyle.TarBrand = this.txtbrand;
            this.txtstyle.TarSeason = this.txtseason;
            this.txtstyle.Leave += new System.EventHandler(this.Txtstyle_Leave);
            this.txtstyle.Validated += new System.EventHandler(this.Txtstyle_Validated);
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(399, 9);
            this.txtbrand.MyDocumentdName = null;
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 3;
            // 
            // txtTeamLeader
            // 
            this.txtTeamLeader.BackColor = System.Drawing.Color.White;
            this.txtTeamLeader.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Leader", true));
            this.txtTeamLeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTeamLeader.Location = new System.Drawing.Point(100, 265);
            this.txtTeamLeader.Name = "txtTeamLeader";
            this.txtTeamLeader.Size = new System.Drawing.Size(57, 23);
            this.txtTeamLeader.TabIndex = 45;
            this.txtTeamLeader.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtTeamLeader_PopUp);
            this.txtTeamLeader.Validating += new System.ComponentModel.CancelEventHandler(this.TxtTeamLeader_Validating);
            this.txtTeamLeader.Validated += new System.EventHandler(this.TxtTeamLeader_Validated);
            // 
            // displayTeamLeader
            // 
            this.displayTeamLeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTeamLeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTeamLeader.Location = new System.Drawing.Point(161, 265);
            this.displayTeamLeader.Name = "displayTeamLeader";
            this.displayTeamLeader.Size = new System.Drawing.Size(141, 23);
            this.displayTeamLeader.TabIndex = 48;
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(100, 296);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(678, 60);
            this.editRemark.TabIndex = 60;
            this.editRemark.WatermarkColor = System.Drawing.Color.Empty;
            this.editRemark.WatermarkText = null;
            // 
            // comboDoxItem
            // 
            this.comboDoxItem.BackColor = System.Drawing.Color.White;
            this.comboDoxItem.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "SubCategory", true));
            this.comboDoxItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDoxItem.FormattingEnabled = true;
            this.comboDoxItem.IsSupportUnselect = true;
            this.comboDoxItem.Location = new System.Drawing.Point(214, 8);
            this.comboDoxItem.Name = "comboDoxItem";
            this.comboDoxItem.OldText = "";
            this.comboDoxItem.Size = new System.Drawing.Size(88, 24);
            this.comboDoxItem.TabIndex = 2;
            this.comboDoxItem.Visible = false;
            this.comboDoxItem.SelectedValueChanged += new System.EventHandler(this.ComboDoxItem_SelectedValueChanged);
            // 
            // labRefno
            // 
            this.labRefno.Location = new System.Drawing.Point(532, 7);
            this.labRefno.Name = "labRefno";
            this.labRefno.Size = new System.Drawing.Size(95, 23);
            this.labRefno.TabIndex = 164;
            this.labRefno.Text = "Refno";
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Refno", true));
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(631, 7);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(147, 23);
            this.txtRefno.TabIndex = 5;
            this.txtRefno.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtRefno_PopUp);
            this.txtRefno.Validating += new System.ComponentModel.CancelEventHandler(this.TxtRefno_Validating);
            this.txtRefno.Validated += new System.EventHandler(this.TxtRefno_Validated);
            // 
            // comboReason
            // 
            this.comboReason.BackColor = System.Drawing.Color.White;
            this.comboReason.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Reason", true));
            this.comboReason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboReason.FormattingEnabled = true;
            this.comboReason.IsSupportUnselect = true;
            this.comboReason.Location = new System.Drawing.Point(631, 7);
            this.comboReason.Name = "comboReason";
            this.comboReason.OldText = "";
            this.comboReason.Size = new System.Drawing.Size(147, 24);
            this.comboReason.TabIndex = 4;
            this.comboReason.Visible = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 23);
            this.label1.TabIndex = 167;
            this.label1.Text = "Style Name";
            // 
            // txtStyleName
            // 
            this.txtStyleName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtStyleName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtStyleName.IsSupportEditMode = false;
            this.txtStyleName.Location = new System.Drawing.Point(99, 73);
            this.txtStyleName.Name = "txtStyleName";
            this.txtStyleName.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.NonReadOnly;
            this.txtStyleName.ReadOnly = true;
            this.txtStyleName.Size = new System.Drawing.Size(203, 23);
            this.txtStyleName.TabIndex = 20;
            // 
            // labGender
            // 
            this.labGender.Location = new System.Drawing.Point(316, 73);
            this.labGender.Name = "labGender";
            this.labGender.Size = new System.Drawing.Size(80, 23);
            this.labGender.TabIndex = 169;
            this.labGender.Text = "Gender";
            // 
            // txtAplType
            // 
            this.txtAplType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtAplType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtAplType.IsSupportEditMode = false;
            this.txtAplType.Location = new System.Drawing.Point(631, 73);
            this.txtAplType.Name = "txtAplType";
            this.txtAplType.ReadOnly = true;
            this.txtAplType.Size = new System.Drawing.Size(147, 23);
            this.txtAplType.TabIndex = 22;
            // 
            // labType
            // 
            this.labType.Location = new System.Drawing.Point(533, 73);
            this.labType.Name = "labType";
            this.labType.Size = new System.Drawing.Size(95, 23);
            this.labType.TabIndex = 171;
            this.labType.Text = "Apparel Type";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(532, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 23);
            this.label5.TabIndex = 175;
            this.label5.Text = "HS Code";
            // 
            // txtHscode
            // 
            this.txtHscode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtHscode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "HScode", true));
            this.txtHscode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtHscode.IsSupportEditMode = false;
            this.txtHscode.Location = new System.Drawing.Point(606, 199);
            this.txtHscode.Name = "txtHscode";
            this.txtHscode.ReadOnly = true;
            this.txtHscode.Size = new System.Drawing.Size(172, 23);
            this.txtHscode.TabIndex = 32;
            // 
            // txtGender
            // 
            this.txtGender.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtGender.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtGender.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtGender.IsSupportEditMode = false;
            this.txtGender.Location = new System.Drawing.Point(399, 73);
            this.txtGender.Name = "txtGender";
            this.txtGender.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.NonReadOnly;
            this.txtGender.ReadOnly = true;
            this.txtGender.Size = new System.Drawing.Size(130, 23);
            this.txtGender.TabIndex = 21;
            // 
            // P02_AddNewItem
            // 
            this.ClientSize = new System.Drawing.Size(800, 416);
            this.Controls.Add(this.txtGender);
            this.Controls.Add(this.txtHscode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtAplType);
            this.Controls.Add(this.labType);
            this.Controls.Add(this.labGender);
            this.Controls.Add(this.txtStyleName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboReason);
            this.Controls.Add(this.txtRefno);
            this.Controls.Add(this.labRefno);
            this.Controls.Add(this.comboDoxItem);
            this.Controls.Add(this.editRemark);
            this.Controls.Add(this.displayTeamLeader);
            this.Controls.Add(this.txtTeamLeader);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.txtstyle);
            this.Controls.Add(this.txtseason);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.labelSeason);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.txtReceiver);
            this.Controls.Add(this.labelReceiver);
            this.Controls.Add(this.numNW);
            this.Controls.Add(this.txtunit_ftyUnit);
            this.Controls.Add(this.labelUnit);
            this.Controls.Add(this.numQty);
            this.Controls.Add(this.txtCTNNo);
            this.Controls.Add(this.labelCTNNo);
            this.Controls.Add(this.numPrice);
            this.Controls.Add(this.editDescription);
            this.Controls.Add(this.displaySPNo);
            this.Controls.Add(this.txtSPNo);
            this.Controls.Add(this.labelRemark);
            this.Controls.Add(this.labelTeamLeader);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelNW);
            this.Controls.Add(this.labelQty);
            this.Controls.Add(this.labelPrice);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelSPNo);
            this.Name = "P02_AddNewItem";
            this.OnLineHelpID = "Sci.Win.Subs.Input2A";
            this.Text = "International Air/Express - Add new item";
            this.WorkAlias = "Express_Detail";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelDescription, 0);
            this.Controls.SetChildIndex(this.labelPrice, 0);
            this.Controls.SetChildIndex(this.labelQty, 0);
            this.Controls.SetChildIndex(this.labelNW, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.labelTeamLeader, 0);
            this.Controls.SetChildIndex(this.labelRemark, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.Controls.SetChildIndex(this.displaySPNo, 0);
            this.Controls.SetChildIndex(this.editDescription, 0);
            this.Controls.SetChildIndex(this.numPrice, 0);
            this.Controls.SetChildIndex(this.labelCTNNo, 0);
            this.Controls.SetChildIndex(this.txtCTNNo, 0);
            this.Controls.SetChildIndex(this.numQty, 0);
            this.Controls.SetChildIndex(this.labelUnit, 0);
            this.Controls.SetChildIndex(this.txtunit_ftyUnit, 0);
            this.Controls.SetChildIndex(this.numNW, 0);
            this.Controls.SetChildIndex(this.labelReceiver, 0);
            this.Controls.SetChildIndex(this.txtReceiver, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelSeason, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.txtseason, 0);
            this.Controls.SetChildIndex(this.txtstyle, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtTeamLeader, 0);
            this.Controls.SetChildIndex(this.displayTeamLeader, 0);
            this.Controls.SetChildIndex(this.editRemark, 0);
            this.Controls.SetChildIndex(this.comboDoxItem, 0);
            this.Controls.SetChildIndex(this.labRefno, 0);
            this.Controls.SetChildIndex(this.txtRefno, 0);
            this.Controls.SetChildIndex(this.comboReason, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtStyleName, 0);
            this.Controls.SetChildIndex(this.labGender, 0);
            this.Controls.SetChildIndex(this.labType, 0);
            this.Controls.SetChildIndex(this.txtAplType, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtHscode, 0);
            this.Controls.SetChildIndex(this.txtGender, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelStyle;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelBrand;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.TextBox txtReceiver;
        private Win.UI.Label labelReceiver;
        private Win.UI.NumericBox numNW;
        private Class.Txtunit_fty txtunit_ftyUnit;
        private Win.UI.Label labelUnit;
        private Win.UI.NumericBox numQty;
        private Win.UI.TextBox txtCTNNo;
        private Win.UI.Label labelCTNNo;
        private Win.UI.NumericBox numPrice;
        private Win.UI.EditBox editDescription;
        private Win.UI.DisplayBox displaySPNo;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelTeamLeader;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelNW;
        private Win.UI.Label labelQty;
        private Win.UI.Label labelPrice;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelSPNo;
        private Class.Txtseason txtseason;
        private Class.Txtstyle txtstyle;
        private Class.Txtbrand txtbrand;
        private Win.UI.TextBox txtTeamLeader;
        private Win.UI.DisplayBox displayTeamLeader;
        private Class.WatermarkEditBox editRemark;
        private Win.UI.ComboBox comboDoxItem;
        private Win.UI.Label labRefno;
        private Win.UI.TextBox txtRefno;
        private Win.UI.ComboBox comboReason;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtStyleName;
        private Win.UI.Label labGender;
        private Win.UI.TextBox txtAplType;
        private Win.UI.Label labType;
        private Win.UI.Label label5;
        private Win.UI.TextBox txtHscode;
        private Win.UI.TextBox txtGender;
    }
}
