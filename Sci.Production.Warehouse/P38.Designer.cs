namespace Sci.Production.Warehouse
{
    partial class P38
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.checkBalance = new Sci.Win.UI.CheckBox();
            this.checkHidden = new Sci.Win.UI.CheckBox();
            this.txtToneGrp = new Sci.Win.UI.TextBox();
            this.label8 = new Sci.Win.UI.Label();
            this.txtDyelot = new Sci.Win.UI.TextBox();
            this.label7 = new Sci.Win.UI.Label();
            this.txtReamark = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.comboFIR = new Sci.Win.UI.ComboBox();
            this.label5 = new Sci.Win.UI.Label();
            this.labLocation = new Sci.Win.UI.Label();
            this.dateATA = new Sci.Win.UI.DateRange();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtReceivingid = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txtwkno = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.labelSEQ = new Sci.Win.UI.Label();
            this.comboStockType = new Sci.Win.UI.ComboBox();
            this.labelStockType = new Sci.Win.UI.Label();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.labelSP = new Sci.Win.UI.Label();
            this.labelStatus = new Sci.Win.UI.Label();
            this.comboStatus = new Sci.Win.UI.ComboBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.panel2 = new Sci.Win.UI.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridMaterialLock = new Sci.Win.UI.Grid();
            this.totalQtyGrid = new Sci.Win.UI.Grid();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnUpdate = new Sci.Win.UI.Button();
            this.txtBatchRemark = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.btnExcel = new Sci.Win.UI.Button();
            this.btnLock = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnUnlock = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.txtMtlLocation = new Sci.Production.Class.TxtMtlLocation(this.components);
            this.comboDropDownList1 = new Sci.Production.Class.ComboDropDownList(this.components);
            this.txtSeq = new Sci.Production.Class.TxtSeq();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMaterialLock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.totalQtyGrid)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBalance);
            this.panel1.Controls.Add(this.checkHidden);
            this.panel1.Controls.Add(this.txtToneGrp);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtDyelot);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtReamark);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.comboFIR);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.labLocation);
            this.panel1.Controls.Add(this.txtMtlLocation);
            this.panel1.Controls.Add(this.dateATA);
            this.panel1.Controls.Add(this.comboDropDownList1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtReceivingid);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtwkno);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelSEQ);
            this.panel1.Controls.Add(this.txtSeq);
            this.panel1.Controls.Add(this.comboStockType);
            this.panel1.Controls.Add(this.labelStockType);
            this.panel1.Controls.Add(this.txtSP);
            this.panel1.Controls.Add(this.labelSP);
            this.panel1.Controls.Add(this.labelStatus);
            this.panel1.Controls.Add(this.comboStatus);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 153);
            this.panel1.TabIndex = 0;
            // 
            // checkBalance
            // 
            this.checkBalance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBalance.AutoSize = true;
            this.checkBalance.Checked = true;
            this.checkBalance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBalance.Location = new System.Drawing.Point(839, 129);
            this.checkBalance.Name = "checkBalance";
            this.checkBalance.Size = new System.Drawing.Size(102, 21);
            this.checkBalance.TabIndex = 28;
            this.checkBalance.Text = "Balance > 0";
            this.checkBalance.UseVisualStyleBackColor = true;
            this.checkBalance.CheckedChanged += new System.EventHandler(this.CheckBalance_CheckedChanged);
            // 
            // checkHidden
            // 
            this.checkHidden.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkHidden.AutoSize = true;
            this.checkHidden.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkHidden.Location = new System.Drawing.Point(839, 105);
            this.checkHidden.Name = "checkHidden";
            this.checkHidden.Size = new System.Drawing.Size(158, 21);
            this.checkHidden.TabIndex = 27;
            this.checkHidden.Text = "Hidden total qty form";
            this.checkHidden.UseVisualStyleBackColor = true;
            this.checkHidden.Click += new System.EventHandler(this.CheckHidden_Click);
            // 
            // txtToneGrp
            // 
            this.txtToneGrp.BackColor = System.Drawing.Color.White;
            this.txtToneGrp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtToneGrp.Location = new System.Drawing.Point(499, 76);
            this.txtToneGrp.MaxLength = 13;
            this.txtToneGrp.Name = "txtToneGrp";
            this.txtToneGrp.Size = new System.Drawing.Size(122, 23);
            this.txtToneGrp.TabIndex = 25;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(410, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 23);
            this.label8.TabIndex = 26;
            this.label8.Text = "Tone/Grp";
            // 
            // txtDyelot
            // 
            this.txtDyelot.BackColor = System.Drawing.Color.White;
            this.txtDyelot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDyelot.Location = new System.Drawing.Point(285, 76);
            this.txtDyelot.MaxLength = 13;
            this.txtDyelot.Name = "txtDyelot";
            this.txtDyelot.Size = new System.Drawing.Size(122, 23);
            this.txtDyelot.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(196, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 23);
            this.label7.TabIndex = 24;
            this.label7.Text = "Dyelot";
            // 
            // txtReamark
            // 
            this.txtReamark.BackColor = System.Drawing.Color.White;
            this.txtReamark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReamark.Location = new System.Drawing.Point(71, 107);
            this.txtReamark.MaxLength = 32767;
            this.txtReamark.Name = "txtReamark";
            this.txtReamark.Size = new System.Drawing.Size(336, 23);
            this.txtReamark.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(9, 107);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 23);
            this.label6.TabIndex = 22;
            this.label6.Text = "Remark";
            // 
            // comboFIR
            // 
            this.comboFIR.BackColor = System.Drawing.Color.White;
            this.comboFIR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFIR.FormattingEnabled = true;
            this.comboFIR.IsSupportUnselect = true;
            this.comboFIR.Location = new System.Drawing.Point(71, 75);
            this.comboFIR.Name = "comboFIR";
            this.comboFIR.OldText = "";
            this.comboFIR.Size = new System.Drawing.Size(122, 24);
            this.comboFIR.TabIndex = 20;
            this.comboFIR.TextChanged += new System.EventHandler(this.ComboFIR_TextChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 23);
            this.label5.TabIndex = 19;
            this.label5.Text = "FIR";
            // 
            // labLocation
            // 
            this.labLocation.Location = new System.Drawing.Point(782, 11);
            this.labLocation.Name = "labLocation";
            this.labLocation.Size = new System.Drawing.Size(91, 23);
            this.labLocation.TabIndex = 14;
            this.labLocation.Text = "Location";
            // 
            // dateATA
            // 
            // 
            // 
            // 
            this.dateATA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateATA.DateBox1.Name = "";
            this.dateATA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateATA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateATA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateATA.DateBox2.Name = "";
            this.dateATA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateATA.DateBox2.TabIndex = 1;
            this.dateATA.Location = new System.Drawing.Point(499, 43);
            this.dateATA.Name = "dateATA";
            this.dateATA.Size = new System.Drawing.Size(280, 23);
            this.dateATA.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(782, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 23);
            this.label4.TabIndex = 18;
            this.label4.Text = "Material Type";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(410, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 23);
            this.label3.TabIndex = 17;
            this.label3.Text = "Material ATA ";
            // 
            // txtReceivingid
            // 
            this.txtReceivingid.BackColor = System.Drawing.Color.White;
            this.txtReceivingid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReceivingid.Location = new System.Drawing.Point(285, 43);
            this.txtReceivingid.MaxLength = 13;
            this.txtReceivingid.Name = "txtReceivingid";
            this.txtReceivingid.Size = new System.Drawing.Size(122, 23);
            this.txtReceivingid.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(196, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 23);
            this.label2.TabIndex = 16;
            this.label2.Text = "Receiving ID";
            // 
            // txtwkno
            // 
            this.txtwkno.BackColor = System.Drawing.Color.White;
            this.txtwkno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtwkno.Location = new System.Drawing.Point(71, 43);
            this.txtwkno.MaxLength = 13;
            this.txtwkno.Name = "txtwkno";
            this.txtwkno.Size = new System.Drawing.Size(122, 23);
            this.txtwkno.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 23);
            this.label1.TabIndex = 15;
            this.label1.Text = "WK NO";
            // 
            // labelSEQ
            // 
            this.labelSEQ.Location = new System.Drawing.Point(196, 11);
            this.labelSEQ.Name = "labelSEQ";
            this.labelSEQ.Size = new System.Drawing.Size(86, 23);
            this.labelSEQ.TabIndex = 11;
            this.labelSEQ.Text = "SEQ";
            // 
            // comboStockType
            // 
            this.comboStockType.BackColor = System.Drawing.Color.White;
            this.comboStockType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStockType.FormattingEnabled = true;
            this.comboStockType.IsSupportUnselect = true;
            this.comboStockType.Items.AddRange(new object[] {
            "ALL",
            "Bulk",
            "Inventory"});
            this.comboStockType.Location = new System.Drawing.Point(680, 10);
            this.comboStockType.Name = "comboStockType";
            this.comboStockType.OldText = "";
            this.comboStockType.Size = new System.Drawing.Size(100, 24);
            this.comboStockType.TabIndex = 3;
            // 
            // labelStockType
            // 
            this.labelStockType.Location = new System.Drawing.Point(582, 11);
            this.labelStockType.Name = "labelStockType";
            this.labelStockType.Size = new System.Drawing.Size(95, 23);
            this.labelStockType.TabIndex = 13;
            this.labelStockType.Text = "Stock Type";
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(71, 11);
            this.txtSP.MaxLength = 13;
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(122, 23);
            this.txtSP.TabIndex = 0;
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(9, 11);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(59, 23);
            this.labelSP.TabIndex = 10;
            this.labelSP.Text = "SP#";
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(410, 11);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(74, 23);
            this.labelStatus.TabIndex = 12;
            this.labelStatus.Text = "Status";
            // 
            // comboStatus
            // 
            this.comboStatus.BackColor = System.Drawing.Color.White;
            this.comboStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.IsSupportUnselect = true;
            this.comboStatus.Items.AddRange(new object[] {
            "All",
            "Locked",
            "Unlocked"});
            this.comboStatus.Location = new System.Drawing.Point(487, 10);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.OldText = "";
            this.comboStatus.Size = new System.Drawing.Size(94, 24);
            this.comboStatus.TabIndex = 2;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(917, 69);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 9;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Location = new System.Drawing.Point(0, 159);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 546);
            this.panel2.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 6);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridMaterialLock);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.totalQtyGrid);
            this.splitContainer1.Size = new System.Drawing.Size(999, 534);
            this.splitContainer1.SplitterDistance = 684;
            this.splitContainer1.TabIndex = 1;
            // 
            // gridMaterialLock
            // 
            this.gridMaterialLock.AllowUserToAddRows = false;
            this.gridMaterialLock.AllowUserToDeleteRows = false;
            this.gridMaterialLock.AllowUserToResizeRows = false;
            this.gridMaterialLock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMaterialLock.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridMaterialLock.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridMaterialLock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMaterialLock.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridMaterialLock.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridMaterialLock.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridMaterialLock.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridMaterialLock.Location = new System.Drawing.Point(6, 6);
            this.gridMaterialLock.Name = "gridMaterialLock";
            this.gridMaterialLock.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridMaterialLock.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridMaterialLock.RowTemplate.Height = 24;
            this.gridMaterialLock.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMaterialLock.ShowCellToolTips = false;
            this.gridMaterialLock.Size = new System.Drawing.Size(673, 525);
            this.gridMaterialLock.TabIndex = 0;
            this.gridMaterialLock.TabStop = false;
            // 
            // totalQtyGrid
            // 
            this.totalQtyGrid.AllowUserToAddRows = false;
            this.totalQtyGrid.AllowUserToDeleteRows = false;
            this.totalQtyGrid.AllowUserToResizeRows = false;
            this.totalQtyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.totalQtyGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.totalQtyGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.totalQtyGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.totalQtyGrid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.totalQtyGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.totalQtyGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.totalQtyGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.totalQtyGrid.Location = new System.Drawing.Point(6, 6);
            this.totalQtyGrid.Name = "totalQtyGrid";
            this.totalQtyGrid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.totalQtyGrid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.totalQtyGrid.RowTemplate.Height = 24;
            this.totalQtyGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.totalQtyGrid.ShowCellToolTips = false;
            this.totalQtyGrid.Size = new System.Drawing.Size(302, 525);
            this.totalQtyGrid.TabIndex = 1;
            this.totalQtyGrid.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnUpdate);
            this.panel3.Controls.Add(this.txtBatchRemark);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.btnExcel);
            this.panel3.Controls.Add(this.btnLock);
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Controls.Add(this.btnUnlock);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 705);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1008, 60);
            this.panel3.TabIndex = 1;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpdate.Location = new System.Drawing.Point(413, 11);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(80, 30);
            this.btnUpdate.TabIndex = 25;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // txtBatchRemark
            // 
            this.txtBatchRemark.BackColor = System.Drawing.Color.White;
            this.txtBatchRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBatchRemark.Location = new System.Drawing.Point(71, 15);
            this.txtBatchRemark.MaxLength = 32767;
            this.txtBatchRemark.Name = "txtBatchRemark";
            this.txtBatchRemark.Size = new System.Drawing.Size(336, 23);
            this.txtBatchRemark.TabIndex = 23;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(9, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 23);
            this.label9.TabIndex = 24;
            this.label9.Text = "Remark";
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Location = new System.Drawing.Point(664, 15);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(80, 30);
            this.btnExcel.TabIndex = 0;
            this.btnExcel.Text = "To Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.BtnExcel_Click);
            // 
            // btnLock
            // 
            this.btnLock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLock.Location = new System.Drawing.Point(750, 15);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(80, 30);
            this.btnLock.TabIndex = 1;
            this.btnLock.Text = "Lock";
            this.btnLock.UseVisualStyleBackColor = true;
            this.btnLock.Click += new System.EventHandler(this.BtnLock_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(922, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnUnlock
            // 
            this.btnUnlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnlock.Location = new System.Drawing.Point(836, 15);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(80, 30);
            this.btnUnlock.TabIndex = 2;
            this.btnUnlock.Text = "Unlock";
            this.btnUnlock.UseVisualStyleBackColor = true;
            this.btnUnlock.Click += new System.EventHandler(this.BtnUnlock_Click);
            // 
            // txtMtlLocation
            // 
            this.txtMtlLocation.BackColor = System.Drawing.Color.White;
            this.txtMtlLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMtlLocation.Location = new System.Drawing.Point(876, 11);
            this.txtMtlLocation.Name = "txtMtlLocation";
            this.txtMtlLocation.Size = new System.Drawing.Size(121, 23);
            this.txtMtlLocation.StockTypeFilte = "B,I";
            this.txtMtlLocation.TabIndex = 4;
            // 
            // comboDropDownList1
            // 
            this.comboDropDownList1.AddAllItem = false;
            this.comboDropDownList1.BackColor = System.Drawing.Color.White;
            this.comboDropDownList1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownList1.FormattingEnabled = true;
            this.comboDropDownList1.IsSupportUnselect = true;
            this.comboDropDownList1.Location = new System.Drawing.Point(876, 42);
            this.comboDropDownList1.Name = "comboDropDownList1";
            this.comboDropDownList1.OldText = "";
            this.comboDropDownList1.Size = new System.Drawing.Size(121, 24);
            this.comboDropDownList1.TabIndex = 8;
            this.comboDropDownList1.Type = "Pms_FabricType";
            // 
            // txtSeq
            // 
            this.txtSeq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq.Location = new System.Drawing.Point(285, 11);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Seq1 = "";
            this.txtSeq.Seq2 = "";
            this.txtSeq.Size = new System.Drawing.Size(72, 23);
            this.txtSeq.TabIndex = 1;
            // 
            // P38
            // 
            this.ClientSize = new System.Drawing.Size(1008, 765);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "txtSP";
            this.DefaultControlForEdit = "txtSP";
            this.Name = "P38";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P38. Material Lock/Unlock for All Transaction";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMaterialLock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.totalQtyGrid)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Button btnQuery;
        private Win.UI.Grid gridMaterialLock;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnUnlock;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label labelStatus;
        private Win.UI.ComboBox comboStatus;
        private Win.UI.TextBox txtSP;
        private Win.UI.Label labelSP;
        private Win.UI.Label labelStockType;
        private Win.UI.ComboBox comboStockType;
        private Win.UI.Button btnLock;
        private Win.UI.Button btnExcel;
        private Class.TxtSeq txtSeq;
        private Win.UI.Label labelSEQ;
        private Win.UI.TextBox txtReceivingid;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtwkno;
        private Win.UI.Label label1;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Class.ComboDropDownList comboDropDownList1;
        private Win.UI.DateRange dateATA;
        private Win.UI.Label labLocation;
        private Class.TxtMtlLocation txtMtlLocation;
        private Win.UI.ComboBox comboFIR;
        private Win.UI.Label label5;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Grid totalQtyGrid;
        private Win.UI.TextBox txtToneGrp;
        private Win.UI.Label label8;
        private Win.UI.TextBox txtDyelot;
        private Win.UI.Label label7;
        private Win.UI.TextBox txtReamark;
        private Win.UI.Label label6;
        private Win.UI.CheckBox checkHidden;
        private Win.UI.Button btnUpdate;
        private Win.UI.TextBox txtBatchRemark;
        private Win.UI.Label label9;
        private Win.UI.CheckBox checkBalance;
    }
}
