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
            this.panel7 = new Sci.Win.UI.Panel();
            this.gridBLNo = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel6 = new Sci.Win.UI.Panel();
            this.panel9 = new Sci.Win.UI.Panel();
            this.panel11 = new Sci.Win.UI.Panel();
            this.gridAccountID = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel10 = new Sci.Win.UI.Panel();
            this.panel8 = new Sci.Win.UI.Panel();
            this.labelDetails = new Sci.Win.UI.Label();
            this.panel5 = new Sci.Win.UI.Panel();
            this.btnDeleteAll = new Sci.Win.UI.Button();
            this.btnAppend = new Sci.Win.UI.Button();
            this.btnReCalculate = new Sci.Win.UI.Button();
            this.btnUndo = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.btnDelete = new Sci.Win.UI.Button();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBLNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridAccountID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.panel8.SuspendLayout();
            this.panel5.SuspendLayout();
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
            this.panel2.Location = new System.Drawing.Point(824, 0);
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
            this.panel3.Size = new System.Drawing.Size(819, 38);
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
            this.labelTtlNumberofDeliverySheets.Lines = 0;
            this.labelTtlNumberofDeliverySheets.Location = new System.Drawing.Point(586, 6);
            this.labelTtlNumberofDeliverySheets.Name = "labelTtlNumberofDeliverySheets";
            this.labelTtlNumberofDeliverySheets.Size = new System.Drawing.Size(190, 23);
            this.labelTtlNumberofDeliverySheets.TabIndex = 8;
            this.labelTtlNumberofDeliverySheets.Text = "Ttl Number of Delivery Sheets";
            // 
            // numTtlCBM
            // 
            this.numTtlCBM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTtlCBM.DecimalPlaces = 2;
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
            this.labelTtlCBM.Lines = 0;
            this.labelTtlCBM.Location = new System.Drawing.Point(432, 6);
            this.labelTtlCBM.Name = "labelTtlCBM";
            this.labelTtlCBM.Size = new System.Drawing.Size(57, 23);
            this.labelTtlCBM.TabIndex = 6;
            this.labelTtlCBM.Text = "Ttl CBM";
            // 
            // numTtlGW
            // 
            this.numTtlGW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTtlGW.DecimalPlaces = 2;
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
            this.labelTtlGW.Lines = 0;
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
            this.labelTtlAmt.Lines = 0;
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
            this.labelCurrency.Lines = 0;
            this.labelCurrency.Location = new System.Drawing.Point(4, 6);
            this.labelCurrency.Name = "labelCurrency";
            this.labelCurrency.Size = new System.Drawing.Size(63, 23);
            this.labelCurrency.TabIndex = 0;
            this.labelCurrency.Text = "Currency";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel7);
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(5, 38);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(819, 501);
            this.panel4.TabIndex = 3;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.gridBLNo);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(693, 292);
            this.panel7.TabIndex = 2;
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
            this.gridBLNo.Size = new System.Drawing.Size(693, 292);
            this.gridBLNo.TabIndex = 0;
            this.gridBLNo.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel9);
            this.panel6.Controls.Add(this.panel8);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 292);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(693, 209);
            this.panel6.TabIndex = 1;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.panel11);
            this.panel9.Controls.Add(this.panel10);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(0, 35);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(693, 174);
            this.panel9.TabIndex = 1;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.gridAccountID);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel11.Location = new System.Drawing.Point(0, 0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(693, 169);
            this.panel11.TabIndex = 2;
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
            this.gridAccountID.Location = new System.Drawing.Point(0, 0);
            this.gridAccountID.Name = "gridAccountID";
            this.gridAccountID.RowHeadersVisible = false;
            this.gridAccountID.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridAccountID.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridAccountID.RowTemplate.Height = 24;
            this.gridAccountID.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAccountID.Size = new System.Drawing.Size(693, 169);
            this.gridAccountID.TabIndex = 0;
            this.gridAccountID.TabStop = false;
            // 
            // panel10
            // 
            this.panel10.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel10.Location = new System.Drawing.Point(0, 169);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(693, 5);
            this.panel10.TabIndex = 1;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.labelDetails);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(693, 35);
            this.panel8.TabIndex = 0;
            // 
            // labelDetails
            // 
            this.labelDetails.Lines = 0;
            this.labelDetails.Location = new System.Drawing.Point(4, 8);
            this.labelDetails.Name = "labelDetails";
            this.labelDetails.Size = new System.Drawing.Size(49, 23);
            this.labelDetails.TabIndex = 0;
            this.labelDetails.Text = "Details";
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
            this.panel5.Location = new System.Drawing.Point(693, 0);
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
            // P08_ShareExpense
            // 
            this.ClientSize = new System.Drawing.Size(829, 539);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.EditMode = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "P08_ShareExpense";
            this.Text = "Share Expense";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBLNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridAccountID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.panel8.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
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
        private Win.UI.Panel panel7;
        private Win.UI.Grid gridBLNo;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel6;
        private Win.UI.Panel panel9;
        private Win.UI.Grid gridAccountID;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Panel panel8;
        private Win.UI.Label labelDetails;
        private Win.UI.Panel panel5;
        private Win.UI.Button btnReCalculate;
        private Win.UI.Button btnUndo;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnDelete;
        private Win.UI.Panel panel11;
        private Win.UI.Panel panel10;
        private Win.UI.Button btnDeleteAll;
        private Win.UI.Button btnAppend;
    }
}
