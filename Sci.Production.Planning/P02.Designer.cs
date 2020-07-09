namespace Sci.Production.Planning
{
    partial class P02
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(P02));
            this.btnFind = new Sci.Win.UI.Button();
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtLocateForSPNo = new Sci.Win.UI.TextBox();
            this.labelLocateForSPNo = new Sci.Win.UI.Label();
            this.checkPrice = new Sci.Win.UI.CheckBox();
            this.dateSewingInline = new Sci.Win.UI.DateRange();
            this.labelSewingInline = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnCheckData = new Sci.Win.UI.Button();
            this.comboinhouseOsp2 = new Sci.Win.UI.ComboBox();
            this.txtsubconLocalSuppid = new Sci.Production.Class.TxtsubconNoConfirm();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.pictureBox2 = new Sci.Win.UI.PictureBox();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.labelCheckedQty = new Sci.Win.UI.Label();
            this.displayCheckedQty = new Sci.Win.UI.DisplayBox();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridPrintingQuickAdjust = new Sci.Win.UI.Grid();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.labelFilterEmpty = new Sci.Win.UI.Label();
            this.checkSuppID = new Sci.Win.UI.CheckBox();
            this.checkInLine = new Sci.Win.UI.CheckBox();
            this.gridSupplier = new Sci.Win.UI.Grid();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.txtmfactory = new Sci.Production.Class.Txtfactory();
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.labelOSPInHouse = new Sci.Win.UI.Label();
            this.comboOSPInHouse = new Sci.Win.UI.ComboBox();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.txtsubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.labelSeason = new Sci.Win.UI.Label();
            this.btnUpdateInline = new Sci.Win.UI.Button();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.btnSetDefaultSuppFromStyle = new Sci.Win.UI.Button();
            this.labelStyle = new Sci.Win.UI.Label();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPrintingQuickAdjust)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSupplier)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(277, 369);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(62, 30);
            this.btnFind.TabIndex = 3;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(5, 237);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(72, 30);
            this.btnQuery.TabIndex = 8;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtLocateForSPNo
            // 
            this.txtLocateForSPNo.BackColor = System.Drawing.Color.White;
            this.txtLocateForSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateForSPNo.Location = new System.Drawing.Point(126, 373);
            this.txtLocateForSPNo.Name = "txtLocateForSPNo";
            this.txtLocateForSPNo.Size = new System.Drawing.Size(145, 23);
            this.txtLocateForSPNo.TabIndex = 2;
            // 
            // labelLocateForSPNo
            // 
            this.labelLocateForSPNo.Location = new System.Drawing.Point(14, 373);
            this.labelLocateForSPNo.Name = "labelLocateForSPNo";
            this.labelLocateForSPNo.Size = new System.Drawing.Size(109, 23);
            this.labelLocateForSPNo.TabIndex = 4;
            this.labelLocateForSPNo.Text = "Locate for SP#";
            // 
            // checkPrice
            // 
            this.checkPrice.AutoSize = true;
            this.checkPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkPrice.Location = new System.Drawing.Point(246, 161);
            this.checkPrice.Name = "checkPrice";
            this.checkPrice.Size = new System.Drawing.Size(83, 21);
            this.checkPrice.TabIndex = 6;
            this.checkPrice.Text = "Price > 0";
            this.checkPrice.UseVisualStyleBackColor = true;
            // 
            // dateSewingInline
            // 
            // 
            // 
            // 
            this.dateSewingInline.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSewingInline.DateBox1.Name = "";
            this.dateSewingInline.DateBox1.Size = new System.Drawing.Size(97, 23);
            this.dateSewingInline.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSewingInline.DateBox2.Location = new System.Drawing.Point(119, 0);
            this.dateSewingInline.DateBox2.Name = "";
            this.dateSewingInline.DateBox2.Size = new System.Drawing.Size(97, 23);
            this.dateSewingInline.DateBox2.TabIndex = 1;
            this.dateSewingInline.IsRequired = false;
            this.dateSewingInline.Location = new System.Drawing.Point(120, 102);
            this.dateSewingInline.Name = "dateSewingInline";
            this.dateSewingInline.Size = new System.Drawing.Size(217, 23);
            this.dateSewingInline.TabIndex = 3;
            // 
            // labelSewingInline
            // 
            this.labelSewingInline.Location = new System.Drawing.Point(7, 102);
            this.labelSewingInline.Name = "labelSewingInline";
            this.labelSewingInline.Size = new System.Drawing.Size(109, 23);
            this.labelSewingInline.TabIndex = 14;
            this.labelSewingInline.Text = "Sewing Inline";
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(97, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(119, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(97, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(120, 73);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(217, 23);
            this.dateSCIDelivery.TabIndex = 2;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(7, 73);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(109, 23);
            this.labelSCIDelivery.TabIndex = 13;
            this.labelSCIDelivery.Text = "SCI  Delivery";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnCheckData);
            this.panel3.Controls.Add(this.comboinhouseOsp2);
            this.panel3.Controls.Add(this.txtsubconLocalSuppid);
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.pictureBox2);
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Controls.Add(this.labelCheckedQty);
            this.panel3.Controls.Add(this.displayCheckedQty);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 616);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1008, 45);
            this.panel3.TabIndex = 0;
            // 
            // btnCheckData
            // 
            this.btnCheckData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckData.Location = new System.Drawing.Point(739, 8);
            this.btnCheckData.Name = "btnCheckData";
            this.btnCheckData.Size = new System.Drawing.Size(96, 30);
            this.btnCheckData.TabIndex = 3;
            this.btnCheckData.Text = "Check Data";
            this.btnCheckData.UseVisualStyleBackColor = true;
            this.btnCheckData.Click += new System.EventHandler(this.BtnCheckData_Click);
            // 
            // comboinhouseOsp2
            // 
            this.comboinhouseOsp2.BackColor = System.Drawing.Color.White;
            this.comboinhouseOsp2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboinhouseOsp2.FormattingEnabled = true;
            this.comboinhouseOsp2.IsSupportUnselect = true;
            this.comboinhouseOsp2.Location = new System.Drawing.Point(262, 15);
            this.comboinhouseOsp2.Name = "comboinhouseOsp2";
            this.comboinhouseOsp2.Size = new System.Drawing.Size(121, 24);
            this.comboinhouseOsp2.TabIndex = 1;
            // 
            // txtsubconLocalSuppid
            // 
            this.txtsubconLocalSuppid.DisplayBox1Binding = "";
            this.txtsubconLocalSuppid.IsIncludeJunk = false;
            this.txtsubconLocalSuppid.Location = new System.Drawing.Point(460, 15);
            this.txtsubconLocalSuppid.Name = "txtsubconLocalSuppid";
            this.txtsubconLocalSuppid.Size = new System.Drawing.Size(170, 23);
            this.txtsubconLocalSuppid.TabIndex = 2;
            this.txtsubconLocalSuppid.TextBox1Binding = "";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(923, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(839, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(636, 8);
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
            this.pictureBox1.Location = new System.Drawing.Point(389, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(25, 31);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            this.pictureBox1.Click += new System.EventHandler(this.PictureBox1_Click);
            // 
            // labelCheckedQty
            // 
            this.labelCheckedQty.Location = new System.Drawing.Point(25, 12);
            this.labelCheckedQty.Name = "labelCheckedQty";
            this.labelCheckedQty.Size = new System.Drawing.Size(88, 23);
            this.labelCheckedQty.TabIndex = 1;
            this.labelCheckedQty.Text = "Checked Qty";
            // 
            // displayCheckedQty
            // 
            this.displayCheckedQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCheckedQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCheckedQty.Location = new System.Drawing.Point(116, 12);
            this.displayCheckedQty.Name = "displayCheckedQty";
            this.displayCheckedQty.Size = new System.Drawing.Size(100, 23);
            this.displayCheckedQty.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridPrintingQuickAdjust);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.gridSupplier);
            this.splitContainer1.Panel2.Controls.Add(this.btnFind);
            this.splitContainer1.Panel2.Controls.Add(this.txtLocateForSPNo);
            this.splitContainer1.Panel2.Controls.Add(this.labelLocateForSPNo);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(1008, 616);
            this.splitContainer1.SplitterDistance = 653;
            this.splitContainer1.TabIndex = 0;
            // 
            // gridPrintingQuickAdjust
            // 
            this.gridPrintingQuickAdjust.AllowUserToAddRows = false;
            this.gridPrintingQuickAdjust.AllowUserToDeleteRows = false;
            this.gridPrintingQuickAdjust.AllowUserToResizeRows = false;
            this.gridPrintingQuickAdjust.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridPrintingQuickAdjust.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridPrintingQuickAdjust.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridPrintingQuickAdjust.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridPrintingQuickAdjust.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPrintingQuickAdjust.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridPrintingQuickAdjust.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridPrintingQuickAdjust.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridPrintingQuickAdjust.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridPrintingQuickAdjust.Location = new System.Drawing.Point(3, 3);
            this.gridPrintingQuickAdjust.Name = "gridPrintingQuickAdjust";
            this.gridPrintingQuickAdjust.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridPrintingQuickAdjust.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridPrintingQuickAdjust.RowTemplate.Height = 24;
            this.gridPrintingQuickAdjust.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPrintingQuickAdjust.Size = new System.Drawing.Size(647, 610);
            this.gridPrintingQuickAdjust.TabIndex = 0;
            this.gridPrintingQuickAdjust.TabStop = false;
            this.gridPrintingQuickAdjust.ColumnDividerDoubleClick += new System.Windows.Forms.DataGridViewColumnDividerDoubleClickEventHandler(this.GridPrintingQuickAdjust_ColumnDividerDoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelFilterEmpty);
            this.groupBox1.Controls.Add(this.checkSuppID);
            this.groupBox1.Controls.Add(this.checkInLine);
            this.groupBox1.Location = new System.Drawing.Point(9, 304);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(339, 60);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // labelFilterEmpty
            // 
            this.labelFilterEmpty.Location = new System.Drawing.Point(7, 23);
            this.labelFilterEmpty.Name = "labelFilterEmpty";
            this.labelFilterEmpty.Size = new System.Drawing.Size(109, 23);
            this.labelFilterEmpty.TabIndex = 2;
            this.labelFilterEmpty.Text = "Filter empty";
            // 
            // checkSuppID
            // 
            this.checkSuppID.AutoSize = true;
            this.checkSuppID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkSuppID.Location = new System.Drawing.Point(120, 25);
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
            this.checkInLine.Location = new System.Drawing.Point(227, 25);
            this.checkInLine.Name = "checkInLine";
            this.checkInLine.Size = new System.Drawing.Size(93, 21);
            this.checkInLine.TabIndex = 1;
            this.checkInLine.Text = "< In Line >";
            this.checkInLine.UseVisualStyleBackColor = true;
            this.checkInLine.CheckedChanged += new System.EventHandler(this.CheckSuppID_CheckedChanged);
            // 
            // gridSupplier
            // 
            this.gridSupplier.AllowUserToAddRows = false;
            this.gridSupplier.AllowUserToDeleteRows = false;
            this.gridSupplier.AllowUserToResizeRows = false;
            this.gridSupplier.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSupplier.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSupplier.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridSupplier.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSupplier.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSupplier.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSupplier.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSupplier.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSupplier.Location = new System.Drawing.Point(9, 405);
            this.gridSupplier.Name = "gridSupplier";
            this.gridSupplier.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSupplier.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSupplier.RowTemplate.Height = 24;
            this.gridSupplier.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSupplier.Size = new System.Drawing.Size(330, 205);
            this.gridSupplier.TabIndex = 17;
            this.gridSupplier.TabStop = false;
            this.gridSupplier.ColumnDividerDoubleClick += new System.Windows.Forms.DataGridViewColumnDividerDoubleClickEventHandler(this.GridSupplier_ColumnDividerDoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtmfactory);
            this.groupBox2.Controls.Add(this.labelFactory);
            this.groupBox2.Controls.Add(this.txtstyle);
            this.groupBox2.Controls.Add(this.labelOSPInHouse);
            this.groupBox2.Controls.Add(this.checkPrice);
            this.groupBox2.Controls.Add(this.dateSewingInline);
            this.groupBox2.Controls.Add(this.labelSewingInline);
            this.groupBox2.Controls.Add(this.comboOSPInHouse);
            this.groupBox2.Controls.Add(this.dateSCIDelivery);
            this.groupBox2.Controls.Add(this.labelSupplier);
            this.groupBox2.Controls.Add(this.btnQuery);
            this.groupBox2.Controls.Add(this.txtsubconSupplier);
            this.groupBox2.Controls.Add(this.labelSCIDelivery);
            this.groupBox2.Controls.Add(this.labelSeason);
            this.groupBox2.Controls.Add(this.btnUpdateInline);
            this.groupBox2.Controls.Add(this.txtseason);
            this.groupBox2.Controls.Add(this.btnSetDefaultSuppFromStyle);
            this.groupBox2.Controls.Add(this.labelStyle);
            this.groupBox2.Location = new System.Drawing.Point(9, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(339, 292);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // txtmfactory
            // 
            this.txtmfactory.BackColor = System.Drawing.Color.White;
            this.txtmfactory.FilteMDivision = false;
            this.txtmfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtmfactory.IssupportJunk = false;
            this.txtmfactory.Location = new System.Drawing.Point(119, 189);
            this.txtmfactory.Name = "txtmfactory";
            this.txtmfactory.Size = new System.Drawing.Size(66, 23);
            this.txtmfactory.TabIndex = 7;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(7, 189);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(109, 23);
            this.labelFactory.TabIndex = 17;
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
            this.labelOSPInHouse.Location = new System.Drawing.Point(7, 159);
            this.labelOSPInHouse.Name = "labelOSPInHouse";
            this.labelOSPInHouse.Size = new System.Drawing.Size(109, 23);
            this.labelOSPInHouse.TabIndex = 16;
            this.labelOSPInHouse.Text = "OSP/InHouse";
            // 
            // comboOSPInHouse
            // 
            this.comboOSPInHouse.BackColor = System.Drawing.Color.White;
            this.comboOSPInHouse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboOSPInHouse.FormattingEnabled = true;
            this.comboOSPInHouse.IsSupportUnselect = true;
            this.comboOSPInHouse.Location = new System.Drawing.Point(119, 159);
            this.comboOSPInHouse.Name = "comboOSPInHouse";
            this.comboOSPInHouse.Size = new System.Drawing.Size(121, 24);
            this.comboOSPInHouse.TabIndex = 5;
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(7, 131);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(109, 23);
            this.labelSupplier.TabIndex = 15;
            this.labelSupplier.Text = "Supplier";
            // 
            // txtsubconSupplier
            // 
            this.txtsubconSupplier.DisplayBox1Binding = "";
            this.txtsubconSupplier.IsIncludeJunk = false;
            this.txtsubconSupplier.Location = new System.Drawing.Point(120, 130);
            this.txtsubconSupplier.Name = "txtsubconSupplier";
            this.txtsubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtsubconSupplier.TabIndex = 4;
            this.txtsubconSupplier.TextBox1Binding = "";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(7, 44);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(75, 23);
            this.labelSeason.TabIndex = 12;
            this.labelSeason.Text = "Season";
            // 
            // btnUpdateInline
            // 
            this.btnUpdateInline.Location = new System.Drawing.Point(78, 237);
            this.btnUpdateInline.Name = "btnUpdateInline";
            this.btnUpdateInline.Size = new System.Drawing.Size(116, 30);
            this.btnUpdateInline.TabIndex = 10;
            this.btnUpdateInline.Text = "Update Inline";
            this.btnUpdateInline.UseVisualStyleBackColor = true;
            this.btnUpdateInline.Click += new System.EventHandler(this.BtnUpdateInline_Click);
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(87, 44);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(80, 23);
            this.txtseason.TabIndex = 1;
            // 
            // btnSetDefaultSuppFromStyle
            // 
            this.btnSetDefaultSuppFromStyle.Location = new System.Drawing.Point(197, 237);
            this.btnSetDefaultSuppFromStyle.Name = "btnSetDefaultSuppFromStyle";
            this.btnSetDefaultSuppFromStyle.Size = new System.Drawing.Size(137, 43);
            this.btnSetDefaultSuppFromStyle.TabIndex = 9;
            this.btnSetDefaultSuppFromStyle.Text = "Set Default Supp from Style";
            this.btnSetDefaultSuppFromStyle.UseVisualStyleBackColor = true;
            this.btnSetDefaultSuppFromStyle.Click += new System.EventHandler(this.BtnSetDefaultSuppFromStyle_Click);
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(7, 15);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(75, 23);
            this.labelStyle.TabIndex = 11;
            this.labelStyle.Text = "Style";
            // 
            // P02
            // 
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel3);
            this.DefaultControl = "txtstyle";
            this.DefaultControlForEdit = "txtstyle";
            this.Name = "P02";
            this.Text = "P02. Printing Quick Adjust";
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridPrintingQuickAdjust)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSupplier)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel3;
        private Win.UI.Button btnFind;
        private Win.UI.Button btnQuery;
        private Win.UI.TextBox txtLocateForSPNo;
        private Win.UI.Label labelLocateForSPNo;
        private Win.UI.CheckBox checkPrice;
        private Win.UI.DateRange dateSewingInline;
        private Win.UI.Label labelSewingInline;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave;
        private Win.UI.PictureBox pictureBox2;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.Label labelCheckedQty;
        private Win.UI.DisplayBox displayCheckedQty;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Grid gridPrintingQuickAdjust;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.CheckBox checkSuppID;
        private Win.UI.CheckBox checkInLine;
        private Win.UI.ComboBox comboOSPInHouse;
        private Win.UI.Label labelSupplier;
        private Class.TxtsubconNoConfirm txtsubconSupplier;
        private Win.UI.Label labelSeason;
        private Class.Txtseason txtseason;
        private Class.Txtstyle txtstyle;
        private Win.UI.Label labelStyle;
        private Win.UI.Button btnSetDefaultSuppFromStyle;
        private Win.UI.Button btnUpdateInline;
        private Win.UI.Grid gridSupplier;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelOSPInHouse;
        private Win.UI.Label labelFilterEmpty;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.ComboBox comboinhouseOsp2;
        private Class.TxtsubconNoConfirm txtsubconLocalSuppid;
        private Win.UI.Button btnCheckData;
        private Class.Txtfactory txtmfactory;
    }
}
