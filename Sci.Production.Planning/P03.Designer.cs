namespace Sci.Production.Planning
{
    partial class P03
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(P03));
            this.btnSPFind = new Sci.Win.UI.Button();
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtLocateForSPNo = new Sci.Win.UI.TextBox();
            this.labelLocateForSPNo = new Sci.Win.UI.Label();
            this.dateSewingInline = new Sci.Win.UI.DateRange();
            this.labelSewingInline = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.panel3 = new Sci.Win.UI.Panel();
            this.numCheckedQty = new Sci.Win.UI.NumericBox();
            this.pictureBox4 = new Sci.Win.UI.PictureBox();
            this.pictureBox3 = new Sci.Win.UI.PictureBox();
            this.dateArtworkOffLine = new Sci.Win.UI.DateBox();
            this.dateArtworkInLine = new Sci.Win.UI.DateBox();
            this.btnCheckData = new Sci.Win.UI.Button();
            this.comboInHouseOSP = new Sci.Win.UI.ComboBox();
            this.txtsubconLocalSuppid = new Sci.Production.Class.TxtsubconNoConfirm();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.pictureBox2 = new Sci.Win.UI.PictureBox();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.labelCheckedQty = new Sci.Win.UI.Label();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.txtmfactory = new Sci.Production.Class.Txtfactory();
            this.label16 = new Sci.Win.UI.Label();
            this.numWorkHours = new Sci.Win.UI.NumericBox();
            this.numEfficiency = new Sci.Win.UI.NumericBox();
            this.labelWorkHours = new Sci.Win.UI.Label();
            this.labelEfficiency = new Sci.Win.UI.Label();
            this.dateInlineDate = new Sci.Win.UI.DateRange();
            this.labelInlineDate = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.labelOSPInHouse = new Sci.Win.UI.Label();
            this.comboOSPInHouse = new Sci.Win.UI.ComboBox();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.txtsubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.label9 = new Sci.Win.UI.Label();
            this.txtseason1 = new Sci.Production.Class.Txtseason();
            this.labelStyle = new Sci.Win.UI.Label();
            this.gridSupplier = new Sci.Win.UI.Grid();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.btnStyleFind = new Sci.Win.UI.Button();
            this.txtLocateForStyleNo = new Sci.Win.UI.TextBox();
            this.labelLocateForStyleNo = new Sci.Win.UI.Label();
            this.labelFilterEmpty = new Sci.Win.UI.Label();
            this.checkSuppID = new Sci.Win.UI.CheckBox();
            this.checkInLine = new Sci.Win.UI.CheckBox();
            this.btnUpdateInline = new Sci.Win.UI.Button();
            this.gridFactoryID = new Sci.Win.UI.Grid();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSupplier)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFactoryID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSPFind
            // 
            this.btnSPFind.Location = new System.Drawing.Point(512, 13);
            this.btnSPFind.Name = "btnSPFind";
            this.btnSPFind.Size = new System.Drawing.Size(53, 30);
            this.btnSPFind.TabIndex = 3;
            this.btnSPFind.Text = "Find";
            this.btnSPFind.UseVisualStyleBackColor = true;
            this.btnSPFind.Click += new System.EventHandler(this.BtnSPFind_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(532, 123);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 10;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtLocateForSPNo
            // 
            this.txtLocateForSPNo.BackColor = System.Drawing.Color.White;
            this.txtLocateForSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateForSPNo.Location = new System.Drawing.Point(385, 17);
            this.txtLocateForSPNo.Name = "txtLocateForSPNo";
            this.txtLocateForSPNo.Size = new System.Drawing.Size(124, 23);
            this.txtLocateForSPNo.TabIndex = 2;
            // 
            // labelLocateForSPNo
            // 
            this.labelLocateForSPNo.Location = new System.Drawing.Point(287, 17);
            this.labelLocateForSPNo.Name = "labelLocateForSPNo";
            this.labelLocateForSPNo.Size = new System.Drawing.Size(98, 23);
            this.labelLocateForSPNo.TabIndex = 16;
            this.labelLocateForSPNo.Text = "Locate for SP#";
            // 
            // dateSewingInline
            // 
            // 
            // 
            // 
            this.dateSewingInline.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSewingInline.DateBox1.Name = "";
            this.dateSewingInline.DateBox1.Size = new System.Drawing.Size(98, 23);
            this.dateSewingInline.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSewingInline.DateBox2.Location = new System.Drawing.Point(120, 0);
            this.dateSewingInline.DateBox2.Name = "";
            this.dateSewingInline.DateBox2.Size = new System.Drawing.Size(98, 23);
            this.dateSewingInline.DateBox2.TabIndex = 1;
            this.dateSewingInline.IsRequired = false;
            this.dateSewingInline.Location = new System.Drawing.Point(343, 44);
            this.dateSewingInline.Name = "dateSewingInline";
            this.dateSewingInline.Size = new System.Drawing.Size(218, 23);
            this.dateSewingInline.TabIndex = 8;
            // 
            // labelSewingInline
            // 
            this.labelSewingInline.Location = new System.Drawing.Point(254, 44);
            this.labelSewingInline.Name = "labelSewingInline";
            this.labelSewingInline.Size = new System.Drawing.Size(87, 23);
            this.labelSewingInline.TabIndex = 11;
            this.labelSewingInline.Text = "Sewing Inline";
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(98, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(120, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(98, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(343, 15);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(218, 23);
            this.dateSCIDelivery.TabIndex = 7;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(254, 15);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(87, 23);
            this.labelSCIDelivery.TabIndex = 9;
            this.labelSCIDelivery.Text = "SCI  Delivery";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.numCheckedQty);
            this.panel3.Controls.Add(this.pictureBox4);
            this.panel3.Controls.Add(this.pictureBox3);
            this.panel3.Controls.Add(this.dateArtworkOffLine);
            this.panel3.Controls.Add(this.dateArtworkInLine);
            this.panel3.Controls.Add(this.btnCheckData);
            this.panel3.Controls.Add(this.comboInHouseOSP);
            this.panel3.Controls.Add(this.txtsubconLocalSuppid);
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.pictureBox2);
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Controls.Add(this.labelCheckedQty);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 616);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1008, 45);
            this.panel3.TabIndex = 1;
            // 
            // numCheckedQty
            // 
            this.numCheckedQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numCheckedQty.DecimalPlaces = 3;
            this.numCheckedQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numCheckedQty.IsSupportEditMode = false;
            this.numCheckedQty.Location = new System.Drawing.Point(95, 12);
            this.numCheckedQty.Name = "numCheckedQty";
            this.numCheckedQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCheckedQty.ReadOnly = true;
            this.numCheckedQty.Size = new System.Drawing.Size(100, 23);
            this.numCheckedQty.TabIndex = 0;
            this.numCheckedQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // pictureBox4
            // 
            this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(764, 5);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(25, 31);
            this.pictureBox4.TabIndex = 35;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.WaitOnLoad = true;
            this.pictureBox4.Click += new System.EventHandler(this.PictureBox4_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(632, 5);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(25, 31);
            this.pictureBox3.TabIndex = 34;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.WaitOnLoad = true;
            this.pictureBox3.Click += new System.EventHandler(this.PictureBox3_Click);
            // 
            // dateArtworkOffLine
            // 
            this.dateArtworkOffLine.Location = new System.Drawing.Point(663, 12);
            this.dateArtworkOffLine.Name = "dateArtworkOffLine";
            this.dateArtworkOffLine.Size = new System.Drawing.Size(97, 23);
            this.dateArtworkOffLine.TabIndex = 3;
            // 
            // dateArtworkInLine
            // 
            this.dateArtworkInLine.Location = new System.Drawing.Point(535, 12);
            this.dateArtworkInLine.Name = "dateArtworkInLine";
            this.dateArtworkInLine.Size = new System.Drawing.Size(95, 23);
            this.dateArtworkInLine.TabIndex = 2;
            this.dateArtworkInLine.Value = new System.DateTime(2017, 2, 1, 0, 0, 0, 0);
            // 
            // btnCheckData
            // 
            this.btnCheckData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckData.Location = new System.Drawing.Point(795, 8);
            this.btnCheckData.Name = "btnCheckData";
            this.btnCheckData.Size = new System.Drawing.Size(93, 30);
            this.btnCheckData.TabIndex = 4;
            this.btnCheckData.Text = "Check Data";
            this.btnCheckData.UseVisualStyleBackColor = true;
            this.btnCheckData.Click += new System.EventHandler(this.BtnCheckData_Click);
            // 
            // comboInHouseOSP
            // 
            this.comboInHouseOSP.BackColor = System.Drawing.Color.White;
            this.comboInHouseOSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboInHouseOSP.FormattingEnabled = true;
            this.comboInHouseOSP.IsSupportUnselect = true;
            this.comboInHouseOSP.Location = new System.Drawing.Point(200, 12);
            this.comboInHouseOSP.Name = "comboInHouseOSP";
            this.comboInHouseOSP.Size = new System.Drawing.Size(99, 24);
            this.comboInHouseOSP.TabIndex = 0;
            // 
            // txtsubconLocalSuppid
            // 
            this.txtsubconLocalSuppid.DisplayBox1Binding = "";
            this.txtsubconLocalSuppid.IsIncludeJunk = false;
            this.txtsubconLocalSuppid.Location = new System.Drawing.Point(335, 12);
            this.txtsubconLocalSuppid.Name = "txtsubconLocalSuppid";
            this.txtsubconLocalSuppid.Size = new System.Drawing.Size(170, 23);
            this.txtsubconLocalSuppid.TabIndex = 1;
            this.txtsubconLocalSuppid.TextBox1Binding = "";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(947, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(58, 30);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(890, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(55, 30);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.InitialImage = global::Sci.Production.Planning.Properties.Resources.trffc15;
            this.pictureBox2.Location = new System.Drawing.Point(506, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(25, 31);
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.WaitOnLoad = true;
            this.pictureBox2.Click += new System.EventHandler(this.PictureBox2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = global::Sci.Production.Planning.Properties.Resources.trffc15;
            this.pictureBox1.Location = new System.Drawing.Point(304, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(25, 31);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            this.pictureBox1.Click += new System.EventHandler(this.PictureBox1_Click);
            // 
            // labelCheckedQty
            // 
            this.labelCheckedQty.Location = new System.Drawing.Point(4, 12);
            this.labelCheckedQty.Name = "labelCheckedQty";
            this.labelCheckedQty.Size = new System.Drawing.Size(88, 23);
            this.labelCheckedQty.TabIndex = 1;
            this.labelCheckedQty.Text = "Checked Qty";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.gridSupplier);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridFactoryID);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(1008, 616);
            this.splitContainer1.SplitterDistance = 235;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtmfactory);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.numWorkHours);
            this.groupBox2.Controls.Add(this.numEfficiency);
            this.groupBox2.Controls.Add(this.labelWorkHours);
            this.groupBox2.Controls.Add(this.labelEfficiency);
            this.groupBox2.Controls.Add(this.dateInlineDate);
            this.groupBox2.Controls.Add(this.labelInlineDate);
            this.groupBox2.Controls.Add(this.labelFactory);
            this.groupBox2.Controls.Add(this.txtstyle);
            this.groupBox2.Controls.Add(this.labelOSPInHouse);
            this.groupBox2.Controls.Add(this.dateSewingInline);
            this.groupBox2.Controls.Add(this.labelSewingInline);
            this.groupBox2.Controls.Add(this.comboOSPInHouse);
            this.groupBox2.Controls.Add(this.dateSCIDelivery);
            this.groupBox2.Controls.Add(this.labelSupplier);
            this.groupBox2.Controls.Add(this.btnQuery);
            this.groupBox2.Controls.Add(this.txtsubconSupplier);
            this.groupBox2.Controls.Add(this.labelSCIDelivery);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtseason1);
            this.groupBox2.Controls.Add(this.labelStyle);
            this.groupBox2.Location = new System.Drawing.Point(9, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(618, 159);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // txtmfactory
            // 
            this.txtmfactory.BackColor = System.Drawing.Color.White;
            this.txtmfactory.FilteMDivision = false;
            this.txtmfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtmfactory.IssupportJunk = false;
            this.txtmfactory.Location = new System.Drawing.Point(120, 133);
            this.txtmfactory.Name = "txtmfactory";
            this.txtmfactory.Size = new System.Drawing.Size(66, 23);
            this.txtmfactory.TabIndex = 1;
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(446, 103);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(19, 23);
            this.label16.TabIndex = 41;
            this.label16.Text = "%";
            // 
            // numWorkHours
            // 
            this.numWorkHours.BackColor = System.Drawing.Color.White;
            this.numWorkHours.DecimalPlaces = 1;
            this.numWorkHours.DisplayStyle = Ict.Win.UI.NumericBoxDisplayStyle.ThousandSeparator;
            this.numWorkHours.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numWorkHours.Location = new System.Drawing.Point(406, 133);
            this.numWorkHours.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            65536});
            this.numWorkHours.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numWorkHours.Name = "numWorkHours";
            this.numWorkHours.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWorkHours.Size = new System.Drawing.Size(100, 23);
            this.numWorkHours.TabIndex = 6;
            this.numWorkHours.Value = new decimal(new int[] {
            150,
            0,
            0,
            65536});
            // 
            // numEfficiency
            // 
            this.numEfficiency.BackColor = System.Drawing.Color.White;
            this.numEfficiency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numEfficiency.IsSupportEditMode = false;
            this.numEfficiency.Location = new System.Drawing.Point(404, 103);
            this.numEfficiency.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numEfficiency.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numEfficiency.Name = "numEfficiency";
            this.numEfficiency.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numEfficiency.Size = new System.Drawing.Size(39, 23);
            this.numEfficiency.TabIndex = 5;
            this.numEfficiency.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // labelWorkHours
            // 
            this.labelWorkHours.Location = new System.Drawing.Point(294, 133);
            this.labelWorkHours.Name = "labelWorkHours";
            this.labelWorkHours.Size = new System.Drawing.Size(109, 23);
            this.labelWorkHours.TabIndex = 38;
            this.labelWorkHours.Text = "Work Hours";
            // 
            // labelEfficiency
            // 
            this.labelEfficiency.Location = new System.Drawing.Point(294, 103);
            this.labelEfficiency.Name = "labelEfficiency";
            this.labelEfficiency.Size = new System.Drawing.Size(109, 23);
            this.labelEfficiency.TabIndex = 36;
            this.labelEfficiency.Text = "Efficiency";
            // 
            // dateInlineDate
            // 
            // 
            // 
            // 
            this.dateInlineDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInlineDate.DateBox1.Name = "";
            this.dateInlineDate.DateBox1.Size = new System.Drawing.Size(98, 23);
            this.dateInlineDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInlineDate.DateBox2.Location = new System.Drawing.Point(120, 0);
            this.dateInlineDate.DateBox2.Name = "";
            this.dateInlineDate.DateBox2.Size = new System.Drawing.Size(98, 23);
            this.dateInlineDate.DateBox2.TabIndex = 1;
            this.dateInlineDate.IsRequired = false;
            this.dateInlineDate.Location = new System.Drawing.Point(343, 73);
            this.dateInlineDate.Name = "dateInlineDate";
            this.dateInlineDate.Size = new System.Drawing.Size(218, 23);
            this.dateInlineDate.TabIndex = 9;
            // 
            // labelInlineDate
            // 
            this.labelInlineDate.Location = new System.Drawing.Point(254, 73);
            this.labelInlineDate.Name = "labelInlineDate";
            this.labelInlineDate.Size = new System.Drawing.Size(87, 23);
            this.labelInlineDate.TabIndex = 34;
            this.labelInlineDate.Text = "Inline Date";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(7, 133);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(109, 23);
            this.labelFactory.TabIndex = 32;
            this.labelFactory.Text = "Factory";
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(87, 15);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 0;
            // 
            // labelOSPInHouse
            // 
            this.labelOSPInHouse.Location = new System.Drawing.Point(7, 73);
            this.labelOSPInHouse.Name = "labelOSPInHouse";
            this.labelOSPInHouse.Size = new System.Drawing.Size(109, 23);
            this.labelOSPInHouse.TabIndex = 31;
            this.labelOSPInHouse.Text = "OSP/InHouse";
            // 
            // comboOSPInHouse
            // 
            this.comboOSPInHouse.BackColor = System.Drawing.Color.White;
            this.comboOSPInHouse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboOSPInHouse.FormattingEnabled = true;
            this.comboOSPInHouse.IsSupportUnselect = true;
            this.comboOSPInHouse.Location = new System.Drawing.Point(119, 72);
            this.comboOSPInHouse.Name = "comboOSPInHouse";
            this.comboOSPInHouse.Size = new System.Drawing.Size(121, 24);
            this.comboOSPInHouse.TabIndex = 4;
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(7, 103);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(109, 23);
            this.labelSupplier.TabIndex = 27;
            this.labelSupplier.Text = "Supplier";
            // 
            // txtsubconSupplier
            // 
            this.txtsubconSupplier.DisplayBox1Binding = "";
            this.txtsubconSupplier.IsIncludeJunk = false;
            this.txtsubconSupplier.Location = new System.Drawing.Point(120, 103);
            this.txtsubconSupplier.Name = "txtsubconSupplier";
            this.txtsubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtsubconSupplier.TabIndex = 3;
            this.txtsubconSupplier.TextBox1Binding = "";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(7, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 23);
            this.label9.TabIndex = 25;
            this.label9.Text = "Season";
            // 
            // txtseason1
            // 
            this.txtseason1.BackColor = System.Drawing.Color.White;
            this.txtseason1.BrandObjectName = null;
            this.txtseason1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason1.Location = new System.Drawing.Point(87, 44);
            this.txtseason1.Name = "txtseason1";
            this.txtseason1.Size = new System.Drawing.Size(80, 23);
            this.txtseason1.TabIndex = 2;
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(7, 15);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(75, 23);
            this.labelStyle.TabIndex = 22;
            this.labelStyle.Text = "Style";
            // 
            // gridSupplier
            // 
            this.gridSupplier.AllowUserToAddRows = false;
            this.gridSupplier.AllowUserToDeleteRows = false;
            this.gridSupplier.AllowUserToResizeRows = false;
            this.gridSupplier.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSupplier.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSupplier.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridSupplier.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSupplier.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSupplier.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSupplier.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSupplier.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSupplier.Location = new System.Drawing.Point(632, 12);
            this.gridSupplier.Name = "gridSupplier";
            this.gridSupplier.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSupplier.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSupplier.RowTemplate.Height = 24;
            this.gridSupplier.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSupplier.Size = new System.Drawing.Size(369, 159);
            this.gridSupplier.TabIndex = 17;
            this.gridSupplier.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnStyleFind);
            this.groupBox1.Controls.Add(this.txtLocateForStyleNo);
            this.groupBox1.Controls.Add(this.labelLocateForStyleNo);
            this.groupBox1.Controls.Add(this.labelFilterEmpty);
            this.groupBox1.Controls.Add(this.checkSuppID);
            this.groupBox1.Controls.Add(this.btnSPFind);
            this.groupBox1.Controls.Add(this.txtLocateForSPNo);
            this.groupBox1.Controls.Add(this.checkInLine);
            this.groupBox1.Controls.Add(this.labelLocateForSPNo);
            this.groupBox1.Controls.Add(this.btnUpdateInline);
            this.groupBox1.Location = new System.Drawing.Point(9, 175);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(996, 49);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            // 
            // btnStyleFind
            // 
            this.btnStyleFind.Location = new System.Drawing.Point(828, 13);
            this.btnStyleFind.Name = "btnStyleFind";
            this.btnStyleFind.Size = new System.Drawing.Size(50, 30);
            this.btnStyleFind.TabIndex = 5;
            this.btnStyleFind.Text = "Find";
            this.btnStyleFind.UseVisualStyleBackColor = true;
            this.btnStyleFind.Click += new System.EventHandler(this.BtnStyleFind_Click);
            // 
            // txtLocateForStyleNo
            // 
            this.txtLocateForStyleNo.BackColor = System.Drawing.Color.White;
            this.txtLocateForStyleNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateForStyleNo.Location = new System.Drawing.Point(680, 17);
            this.txtLocateForStyleNo.Name = "txtLocateForStyleNo";
            this.txtLocateForStyleNo.Size = new System.Drawing.Size(145, 23);
            this.txtLocateForStyleNo.TabIndex = 4;
            // 
            // labelLocateForStyleNo
            // 
            this.labelLocateForStyleNo.Location = new System.Drawing.Point(568, 17);
            this.labelLocateForStyleNo.Name = "labelLocateForStyleNo";
            this.labelLocateForStyleNo.Size = new System.Drawing.Size(109, 23);
            this.labelLocateForStyleNo.TabIndex = 36;
            this.labelLocateForStyleNo.Text = "Locate for Style#";
            // 
            // labelFilterEmpty
            // 
            this.labelFilterEmpty.Location = new System.Drawing.Point(5, 17);
            this.labelFilterEmpty.Name = "labelFilterEmpty";
            this.labelFilterEmpty.Size = new System.Drawing.Size(80, 23);
            this.labelFilterEmpty.TabIndex = 33;
            this.labelFilterEmpty.Text = "Filter empty";
            // 
            // checkSuppID
            // 
            this.checkSuppID.AutoSize = true;
            this.checkSuppID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkSuppID.Location = new System.Drawing.Point(89, 18);
            this.checkSuppID.Name = "checkSuppID";
            this.checkSuppID.Size = new System.Drawing.Size(101, 21);
            this.checkSuppID.TabIndex = 0;
            this.checkSuppID.Text = "< Supp ID >";
            this.checkSuppID.UseVisualStyleBackColor = true;
            this.checkSuppID.CheckedChanged += new System.EventHandler(this.CheckSuppID_CheckedChanged);
            // 
            // checkInLine
            // 
            this.checkInLine.AutoSize = true;
            this.checkInLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkInLine.Location = new System.Drawing.Point(190, 18);
            this.checkInLine.Name = "checkInLine";
            this.checkInLine.Size = new System.Drawing.Size(93, 21);
            this.checkInLine.TabIndex = 1;
            this.checkInLine.Text = "< In Line >";
            this.checkInLine.UseVisualStyleBackColor = true;
            this.checkInLine.CheckedChanged += new System.EventHandler(this.CheckSuppID_CheckedChanged);
            // 
            // btnUpdateInline
            // 
            this.btnUpdateInline.Location = new System.Drawing.Point(879, 13);
            this.btnUpdateInline.Name = "btnUpdateInline";
            this.btnUpdateInline.Size = new System.Drawing.Size(115, 30);
            this.btnUpdateInline.TabIndex = 6;
            this.btnUpdateInline.Text = "Update Inline";
            this.btnUpdateInline.UseVisualStyleBackColor = true;
            this.btnUpdateInline.Click += new System.EventHandler(this.BtnUpdateInline_Click);
            // 
            // gridFactoryID
            // 
            this.gridFactoryID.AllowUserToAddRows = false;
            this.gridFactoryID.AllowUserToDeleteRows = false;
            this.gridFactoryID.AllowUserToResizeRows = false;
            this.gridFactoryID.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFactoryID.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridFactoryID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridFactoryID.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridFactoryID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFactoryID.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridFactoryID.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridFactoryID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridFactoryID.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridFactoryID.Location = new System.Drawing.Point(3, 3);
            this.gridFactoryID.Name = "gridFactoryID";
            this.gridFactoryID.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridFactoryID.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridFactoryID.RowTemplate.Height = 24;
            this.gridFactoryID.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFactoryID.Size = new System.Drawing.Size(1002, 360);
            this.gridFactoryID.TabIndex = 34;
            this.gridFactoryID.TabStop = false;
            // 
            // P03
            // 
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel3);
            this.DefaultControl = "txtstyle";
            this.DefaultControlForEdit = "txtstyle";
            this.Name = "P03";
            this.Text = "P03. Laser Quick Adjust";
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSupplier)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFactoryID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel3;
        private Win.UI.Button btnSPFind;
        private Win.UI.Button btnQuery;
        private Win.UI.TextBox txtLocateForSPNo;
        private Win.UI.Label labelLocateForSPNo;
        private Win.UI.DateRange dateSewingInline;
        private Win.UI.Label labelSewingInline;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave;
        private Win.UI.PictureBox pictureBox2;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.Label labelCheckedQty;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.CheckBox checkSuppID;
        private Win.UI.CheckBox checkInLine;
        private Win.UI.ComboBox comboOSPInHouse;
        private Win.UI.Label labelSupplier;
        private Class.TxtsubconNoConfirm txtsubconSupplier;
        private Win.UI.Label label9;
        private Class.Txtseason txtseason1;
        private Class.Txtstyle txtstyle;
        private Win.UI.Label labelStyle;
        private Win.UI.Button btnUpdateInline;
        private Win.UI.Grid gridSupplier;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelOSPInHouse;
        private Win.UI.Label labelFilterEmpty;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.ComboBox comboInHouseOSP;
        private Class.TxtsubconNoConfirm txtsubconLocalSuppid;
        private Win.UI.Button btnCheckData;
        private Win.UI.NumericBox numWorkHours;
        private Win.UI.NumericBox numEfficiency;
        private Win.UI.Label labelWorkHours;
        private Win.UI.Label labelEfficiency;
        private Win.UI.DateRange dateInlineDate;
        private Win.UI.Label labelInlineDate;
        private Win.UI.Button btnStyleFind;
        private Win.UI.TextBox txtLocateForStyleNo;
        private Win.UI.Label labelLocateForStyleNo;
        private Win.UI.Grid gridFactoryID;
        private Win.UI.PictureBox pictureBox3;
        private Win.UI.DateBox dateArtworkOffLine;
        private Win.UI.DateBox dateArtworkInLine;
        private Win.UI.PictureBox pictureBox4;
        private Win.UI.Label label16;
        private Win.UI.NumericBox numCheckedQty;
        private Class.Txtfactory txtmfactory;
    }
}
