namespace Sci.Production.Shipping
{
    partial class P08_ShareExpense
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.numTtlNumberofDeliverySheets = new Sci.Win.UI.NumericBox();
            this.labelTtlNumberofDeliverySheets = new Sci.Win.UI.Label();
            this.numTtlCBM = new Sci.Win.UI.NumericBox();
            this.labelTtlCBM = new Sci.Win.UI.Label();
            this.numTtlGW = new Sci.Win.UI.NumericBox();
            this.labelTtlGW = new Sci.Win.UI.Label();
            this.numTtlAmt = new Sci.Win.UI.NumericBox();
            this.labelTtlAmt = new Sci.Win.UI.Label();
            this.displayCurrency = new Sci.Win.UI.DisplayBox();
            this.labelCurrency = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.gridBLNo = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.tabControl1 = new Sci.Win.UI.TabControl();
            this.tabDetails = new System.Windows.Forms.TabPage();
            this.gridAccountID = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.tabSAPP = new System.Windows.Forms.TabPage();
            this.gridSAPP = new Sci.Win.UI.Grid();
            this.listControlBindingSource3 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel5 = new Sci.Win.UI.Panel();
            this.btnDeleteAll = new Sci.Win.UI.Button();
            this.btnAppend = new Sci.Win.UI.Button();
            this.btnReCalculate = new Sci.Win.UI.Button();
            this.btnUndo = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.btnDelete = new Sci.Win.UI.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBLNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridAccountID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.tabSAPP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSAPP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource3)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 539);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(854, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 539);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.numTtlNumberofDeliverySheets);
            this.panel3.Controls.Add(this.labelTtlNumberofDeliverySheets);
            this.panel3.Controls.Add(this.numTtlCBM);
            this.panel3.Controls.Add(this.labelTtlCBM);
            this.panel3.Controls.Add(this.numTtlGW);
            this.panel3.Controls.Add(this.labelTtlGW);
            this.panel3.Controls.Add(this.numTtlAmt);
            this.panel3.Controls.Add(this.labelTtlAmt);
            this.panel3.Controls.Add(this.displayCurrency);
            this.panel3.Controls.Add(this.labelCurrency);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(849, 38);
            this.panel3.TabIndex = 2;
            // 
            // numTtlNumberofDeliverySheets
            // 
            this.numTtlNumberofDeliverySheets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTtlNumberofDeliverySheets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTtlNumberofDeliverySheets.IsSupportEditMode = false;
            this.numTtlNumberofDeliverySheets.Location = new System.Drawing.Point(780, 6);
            this.numTtlNumberofDeliverySheets.Name = "numTtlNumberofDeliverySheets";
            this.numTtlNumberofDeliverySheets.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTtlNumberofDeliverySheets.ReadOnly = true;
            this.numTtlNumberofDeliverySheets.Size = new System.Drawing.Size(30, 23);
            this.numTtlNumberofDeliverySheets.TabIndex = 9;
            this.numTtlNumberofDeliverySheets.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelTtlNumberofDeliverySheets
            // 
            this.labelTtlNumberofDeliverySheets.Location = new System.Drawing.Point(586, 6);
            this.labelTtlNumberofDeliverySheets.Name = "labelTtlNumberofDeliverySheets";
            this.labelTtlNumberofDeliverySheets.Size = new System.Drawing.Size(190, 23);
            this.labelTtlNumberofDeliverySheets.TabIndex = 8;
            this.labelTtlNumberofDeliverySheets.Text = "Ttl Number of Delivery Sheets";
            // 
            // numTtlCBM
            // 
            this.numTtlCBM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTtlCBM.DecimalPlaces = 3;
            this.numTtlCBM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTtlCBM.IsSupportEditMode = false;
            this.numTtlCBM.Location = new System.Drawing.Point(493, 6);
            this.numTtlCBM.Name = "numTtlCBM";
            this.numTtlCBM.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTtlCBM.ReadOnly = true;
            this.numTtlCBM.Size = new System.Drawing.Size(75, 23);
            this.numTtlCBM.TabIndex = 7;
            this.numTtlCBM.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelTtlCBM
            // 
            this.labelTtlCBM.Location = new System.Drawing.Point(432, 6);
            this.labelTtlCBM.Name = "labelTtlCBM";
            this.labelTtlCBM.Size = new System.Drawing.Size(57, 23);
            this.labelTtlCBM.TabIndex = 6;
            this.labelTtlCBM.Text = "Ttl CBM";
            // 
            // numTtlGW
            // 
            this.numTtlGW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTtlGW.DecimalPlaces = 3;
            this.numTtlGW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTtlGW.IsSupportEditMode = false;
            this.numTtlGW.Location = new System.Drawing.Point(337, 6);
            this.numTtlGW.Name = "numTtlGW";
            this.numTtlGW.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTtlGW.ReadOnly = true;
            this.numTtlGW.Size = new System.Drawing.Size(75, 23);
            this.numTtlGW.TabIndex = 5;
            this.numTtlGW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelTtlGW
            // 
            this.labelTtlGW.Location = new System.Drawing.Point(277, 6);
            this.labelTtlGW.Name = "labelTtlGW";
            this.labelTtlGW.Size = new System.Drawing.Size(56, 23);
            this.labelTtlGW.TabIndex = 4;
            this.labelTtlGW.Text = "Ttl G.W.";
            // 
            // numTtlAmt
            // 
            this.numTtlAmt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTtlAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTtlAmt.IsSupportEditMode = false;
            this.numTtlAmt.Location = new System.Drawing.Point(179, 6);
            this.numTtlAmt.Name = "numTtlAmt";
            this.numTtlAmt.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTtlAmt.ReadOnly = true;
            this.numTtlAmt.Size = new System.Drawing.Size(79, 23);
            this.numTtlAmt.TabIndex = 3;
            this.numTtlAmt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelTtlAmt
            // 
            this.labelTtlAmt.Location = new System.Drawing.Point(122, 6);
            this.labelTtlAmt.Name = "labelTtlAmt";
            this.labelTtlAmt.Size = new System.Drawing.Size(53, 23);
            this.labelTtlAmt.TabIndex = 2;
            this.labelTtlAmt.Text = "Ttl Amt";
            // 
            // displayCurrency
            // 
            this.displayCurrency.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCurrency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCurrency.Location = new System.Drawing.Point(70, 6);
            this.displayCurrency.Name = "displayCurrency";
            this.displayCurrency.Size = new System.Drawing.Size(30, 23);
            this.displayCurrency.TabIndex = 1;
            // 
            // labelCurrency
            // 
            this.labelCurrency.Location = new System.Drawing.Point(4, 6);
            this.labelCurrency.Name = "labelCurrency";
            this.labelCurrency.Size = new System.Drawing.Size(63, 23);
            this.labelCurrency.TabIndex = 0;
            this.labelCurrency.Text = "Currency";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.splitContainer1);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(5, 38);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(849, 501);
            this.panel4.TabIndex = 3;
            // 
            // gridBLNo
            // 
            this.gridBLNo.AllowUserToAddRows = false;
            this.gridBLNo.AllowUserToDeleteRows = false;
            this.gridBLNo.AllowUserToResizeRows = false;
            this.gridBLNo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBLNo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBLNo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBLNo.DataSource = this.listControlBindingSource1;
            this.gridBLNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBLNo.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBLNo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBLNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBLNo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBLNo.Location = new System.Drawing.Point(0, 0);
            this.gridBLNo.Name = "gridBLNo";
            this.gridBLNo.RowHeadersVisible = false;
            this.gridBLNo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBLNo.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBLNo.RowTemplate.Height = 24;
            this.gridBLNo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBLNo.ShowCellToolTips = false;
            this.gridBLNo.Size = new System.Drawing.Size(723, 250);
            this.gridBLNo.TabIndex = 0;
            this.gridBLNo.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabDetails);
            this.tabControl1.Controls.Add(this.tabSAPP);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(723, 247);
            this.tabControl1.TabIndex = 1;
            // 
            // tabDetails
            // 
            this.tabDetails.Controls.Add(this.gridAccountID);
            this.tabDetails.Location = new System.Drawing.Point(4, 25);
            this.tabDetails.Name = "tabDetails";
            this.tabDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tabDetails.Size = new System.Drawing.Size(715, 218);
            this.tabDetails.TabIndex = 0;
            this.tabDetails.Text = "Details";
            // 
            // gridAccountID
            // 
            this.gridAccountID.AllowUserToAddRows = false;
            this.gridAccountID.AllowUserToDeleteRows = false;
            this.gridAccountID.AllowUserToResizeRows = false;
            this.gridAccountID.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridAccountID.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridAccountID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAccountID.DataSource = this.listControlBindingSource2;
            this.gridAccountID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridAccountID.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridAccountID.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridAccountID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridAccountID.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridAccountID.Location = new System.Drawing.Point(3, 3);
            this.gridAccountID.Name = "gridAccountID";
            this.gridAccountID.RowHeadersVisible = false;
            this.gridAccountID.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridAccountID.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridAccountID.RowTemplate.Height = 24;
            this.gridAccountID.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAccountID.ShowCellToolTips = false;
            this.gridAccountID.Size = new System.Drawing.Size(709, 212);
            this.gridAccountID.TabIndex = 0;
            this.gridAccountID.TabStop = false;
            // 
            // tabSAPP
            // 
            this.tabSAPP.Controls.Add(this.gridSAPP);
            this.tabSAPP.Location = new System.Drawing.Point(4, 22);
            this.tabSAPP.Name = "tabSAPP";
            this.tabSAPP.Padding = new System.Windows.Forms.Padding(3);
            this.tabSAPP.Size = new System.Drawing.Size(715, 178);
            this.tabSAPP.TabIndex = 1;
            this.tabSAPP.Text = "Shared Amt  by APP";
            // 
            // gridSAPP
            // 
            this.gridSAPP.AllowUserToAddRows = false;
            this.gridSAPP.AllowUserToDeleteRows = false;
            this.gridSAPP.AllowUserToResizeRows = false;
            this.gridSAPP.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSAPP.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSAPP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSAPP.DataSource = this.listControlBindingSource3;
            this.gridSAPP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSAPP.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSAPP.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSAPP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSAPP.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSAPP.Location = new System.Drawing.Point(3, 3);
            this.gridSAPP.Name = "gridSAPP";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridSAPP.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridSAPP.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSAPP.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSAPP.RowTemplate.Height = 24;
            this.gridSAPP.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSAPP.ShowCellToolTips = false;
            this.gridSAPP.Size = new System.Drawing.Size(709, 172);
            this.gridSAPP.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnDeleteAll);
            this.panel5.Controls.Add(this.btnAppend);
            this.panel5.Controls.Add(this.btnReCalculate);
            this.panel5.Controls.Add(this.btnUndo);
            this.panel5.Controls.Add(this.btnSave);
            this.panel5.Controls.Add(this.btnImport);
            this.panel5.Controls.Add(this.btnDelete);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(723, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(126, 501);
            this.panel5.TabIndex = 0;
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnDeleteAll.Location = new System.Drawing.Point(13, 83);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(106, 30);
            this.btnDeleteAll.TabIndex = 8;
            this.btnDeleteAll.Text = "Delete All";
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.BtnDeleteAll_Click);
            // 
            // btnAppend
            // 
            this.btnAppend.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnAppend.Location = new System.Drawing.Point(13, 11);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(106, 30);
            this.btnAppend.TabIndex = 7;
            this.btnAppend.Text = "Append";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.BtnAppend_Click);
            // 
            // btnReCalculate
            // 
            this.btnReCalculate.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnReCalculate.Location = new System.Drawing.Point(14, 261);
            this.btnReCalculate.Name = "btnReCalculate";
            this.btnReCalculate.Size = new System.Drawing.Size(106, 30);
            this.btnReCalculate.TabIndex = 6;
            this.btnReCalculate.Text = "Re-Calculate";
            this.btnReCalculate.UseVisualStyleBackColor = true;
            this.btnReCalculate.Click += new System.EventHandler(this.BtnReCalculate_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Location = new System.Drawing.Point(14, 216);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(106, 30);
            this.btnUndo.TabIndex = 5;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.BtnUndo_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(14, 179);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(106, 30);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnImport
            // 
            this.btnImport.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImport.Location = new System.Drawing.Point(13, 119);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(106, 30);
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnDelete.Location = new System.Drawing.Point(13, 47);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(106, 30);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridBLNo);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(723, 501);
            this.splitContainer1.SplitterDistance = 250;
            this.splitContainer1.TabIndex = 2;
            // 
            // P08_ShareExpense
            // 
            this.ClientSize = new System.Drawing.Size(859, 539);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.EditMode = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "P08_ShareExpense";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Share Expense";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBLNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridAccountID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.tabSAPP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSAPP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource3)).EndInit();
            this.panel5.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.NumericBox numTtlNumberofDeliverySheets;
        private Win.UI.Label labelTtlNumberofDeliverySheets;
        private Win.UI.NumericBox numTtlCBM;
        private Win.UI.Label labelTtlCBM;
        private Win.UI.NumericBox numTtlGW;
        private Win.UI.Label labelTtlGW;
        private Win.UI.NumericBox numTtlAmt;
        private Win.UI.Label labelTtlAmt;
        private Win.UI.DisplayBox displayCurrency;
        private Win.UI.Label labelCurrency;
        private Win.UI.Panel panel4;
        private Win.UI.Grid gridBLNo;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Grid gridAccountID;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Panel panel5;
        private Win.UI.Button btnReCalculate;
        private Win.UI.Button btnUndo;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnDelete;
        private Win.UI.Button btnDeleteAll;
        private Win.UI.Button btnAppend;
        private Win.UI.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabDetails;
        private System.Windows.Forms.TabPage tabSAPP;
        private Win.UI.Grid gridSAPP;
        private Win.UI.ListControlBindingSource listControlBindingSource3;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
