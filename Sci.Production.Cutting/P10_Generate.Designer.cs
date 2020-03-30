namespace Sci.Production.Cutting
{
    partial class P10_Generate
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grid_qty = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.grid_art = new Sci.Win.UI.Grid();
            this.art_contextMenuStrip = new Sci.Win.UI.ContextMenuStrip();
            this.insertIntoRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grid_allpart = new Sci.Win.UI.Grid();
            this.allpart_contextMenuStrip = new Sci.Win.UI.ContextMenuStrip();
            this.allpart_insert = new System.Windows.Forms.ToolStripMenuItem();
            this.allpart_delete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOK = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnGarment = new Sci.Win.UI.Button();
            this.button_RighttoLeft = new Sci.Win.UI.Button();
            this.button_LefttoRight = new Sci.Win.UI.Button();
            this.button_Qty = new Sci.Win.UI.Button();
            this.labelNoOfBundle = new Sci.Win.UI.Label();
            this.numNoOfBundle = new Sci.Win.UI.NumericBox();
            this.labelTotalQty = new Sci.Win.UI.Label();
            this.labelTotalCutOutput = new Sci.Win.UI.Label();
            this.displayTotalQty = new Sci.Win.UI.DisplayBox();
            this.displayTotalCutOutput = new Sci.Win.UI.DisplayBox();
            this.label3 = new Sci.Win.UI.Label();
            this.labelPatternPanel = new Sci.Win.UI.Label();
            this.displayPatternPanel = new Sci.Win.UI.DisplayBox();
            this.labelTotalParts = new Sci.Win.UI.Label();
            this.numTotalParts = new Sci.Win.UI.NumericBox();
            this.label6 = new Sci.Win.UI.Label();
            this.grid_Size = new Sci.Win.UI.Grid();
            this.chkTone = new Sci.Win.UI.CheckBox();
            this.label1 = new Sci.Win.UI.Label();
            this.numTone = new Sci.Win.UI.NumericBox();
            ((System.ComponentModel.ISupportInitialize)(this.grid_qty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_art)).BeginInit();
            this.art_contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_allpart)).BeginInit();
            this.allpart_contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Size)).BeginInit();
            this.SuspendLayout();
            // 
            // grid_qty
            // 
            this.grid_qty.AllowUserToAddRows = false;
            this.grid_qty.AllowUserToDeleteRows = false;
            this.grid_qty.AllowUserToResizeRows = false;
            this.grid_qty.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid_qty.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid_qty.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_qty.DataSource = this.listControlBindingSource1;
            this.grid_qty.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid_qty.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid_qty.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid_qty.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid_qty.Location = new System.Drawing.Point(10, 45);
            this.grid_qty.Name = "grid_qty";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid_qty.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid_qty.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_qty.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_qty.RowTemplate.Height = 24;
            this.grid_qty.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_qty.ShowCellToolTips = false;
            this.grid_qty.Size = new System.Drawing.Size(493, 138);
            this.grid_qty.TabIndex = 0;
            this.grid_qty.TabStop = false;
            // 
            // grid_art
            // 
            this.grid_art.AllowUserToAddRows = false;
            this.grid_art.AllowUserToDeleteRows = false;
            this.grid_art.AllowUserToResizeRows = false;
            this.grid_art.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid_art.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid_art.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_art.ContextMenuStrip = this.art_contextMenuStrip;
            this.grid_art.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid_art.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid_art.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid_art.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid_art.Location = new System.Drawing.Point(7, 238);
            this.grid_art.Name = "grid_art";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid_art.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grid_art.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_art.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_art.RowTemplate.Height = 24;
            this.grid_art.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_art.ShowCellToolTips = false;
            this.grid_art.Size = new System.Drawing.Size(496, 208);
            this.grid_art.TabIndex = 1;
            this.grid_art.TabStop = false;
            // 
            // art_contextMenuStrip
            // 
            this.art_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertIntoRecordToolStripMenuItem,
            this.deleteRecordToolStripMenuItem});
            this.art_contextMenuStrip.Name = "art_contextMenuStrip";
            this.art_contextMenuStrip.Size = new System.Drawing.Size(176, 48);
            // 
            // insertIntoRecordToolStripMenuItem
            // 
            this.insertIntoRecordToolStripMenuItem.Name = "insertIntoRecordToolStripMenuItem";
            this.insertIntoRecordToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.insertIntoRecordToolStripMenuItem.Text = "Insert into Record";
            this.insertIntoRecordToolStripMenuItem.Click += new System.EventHandler(this.insertIntoRecordToolStripMenuItem_Click);
            // 
            // deleteRecordToolStripMenuItem
            // 
            this.deleteRecordToolStripMenuItem.Name = "deleteRecordToolStripMenuItem";
            this.deleteRecordToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.deleteRecordToolStripMenuItem.Text = "Delete Record";
            this.deleteRecordToolStripMenuItem.Click += new System.EventHandler(this.deleteRecordToolStripMenuItem_Click);
            // 
            // grid_allpart
            // 
            this.grid_allpart.AllowUserToAddRows = false;
            this.grid_allpart.AllowUserToDeleteRows = false;
            this.grid_allpart.AllowUserToResizeRows = false;
            this.grid_allpart.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid_allpart.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid_allpart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_allpart.ContextMenuStrip = this.allpart_contextMenuStrip;
            this.grid_allpart.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid_allpart.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid_allpart.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid_allpart.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid_allpart.Location = new System.Drawing.Point(553, 238);
            this.grid_allpart.Name = "grid_allpart";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid_allpart.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.grid_allpart.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_allpart.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_allpart.RowTemplate.Height = 24;
            this.grid_allpart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_allpart.ShowCellToolTips = false;
            this.grid_allpart.Size = new System.Drawing.Size(498, 208);
            this.grid_allpart.TabIndex = 2;
            this.grid_allpart.TabStop = false;
            // 
            // allpart_contextMenuStrip
            // 
            this.allpart_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allpart_insert,
            this.allpart_delete});
            this.allpart_contextMenuStrip.Name = "art_contextMenuStrip";
            this.allpart_contextMenuStrip.Size = new System.Drawing.Size(176, 48);
            // 
            // allpart_insert
            // 
            this.allpart_insert.Name = "allpart_insert";
            this.allpart_insert.Size = new System.Drawing.Size(175, 22);
            this.allpart_insert.Text = "Insert into Record";
            this.allpart_insert.Click += new System.EventHandler(this.allpart_insert_Click);
            // 
            // allpart_delete
            // 
            this.allpart_delete.Name = "allpart_delete";
            this.allpart_delete.Size = new System.Drawing.Size(175, 22);
            this.allpart_delete.Text = "Delete Record";
            this.allpart_delete.Click += new System.EventHandler(this.allpart_delete_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(885, 453);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 30);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.OK_button_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(971, 453);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGarment
            // 
            this.btnGarment.Location = new System.Drawing.Point(9, 453);
            this.btnGarment.Name = "btnGarment";
            this.btnGarment.Size = new System.Drawing.Size(111, 30);
            this.btnGarment.TabIndex = 8;
            this.btnGarment.Text = "Garment List";
            this.btnGarment.UseVisualStyleBackColor = true;
            this.btnGarment.Click += new System.EventHandler(this.btnGarment_Click);
            // 
            // button_RighttoLeft
            // 
            this.button_RighttoLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.button_RighttoLeft.Location = new System.Drawing.Point(509, 357);
            this.button_RighttoLeft.Name = "button_RighttoLeft";
            this.button_RighttoLeft.Size = new System.Drawing.Size(38, 30);
            this.button_RighttoLeft.TabIndex = 7;
            this.button_RighttoLeft.Text = "<";
            this.button_RighttoLeft.UseVisualStyleBackColor = true;
            this.button_RighttoLeft.Click += new System.EventHandler(this.button_RighttoLeft_Click);
            // 
            // button_LefttoRight
            // 
            this.button_LefttoRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.button_LefttoRight.Location = new System.Drawing.Point(509, 288);
            this.button_LefttoRight.Name = "button_LefttoRight";
            this.button_LefttoRight.Size = new System.Drawing.Size(38, 30);
            this.button_LefttoRight.TabIndex = 6;
            this.button_LefttoRight.Text = ">";
            this.button_LefttoRight.UseVisualStyleBackColor = true;
            this.button_LefttoRight.Click += new System.EventHandler(this.button_LefttoRight_Click);
            // 
            // button_Qty
            // 
            this.button_Qty.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.button_Qty.Location = new System.Drawing.Point(509, 88);
            this.button_Qty.Name = "button_Qty";
            this.button_Qty.Size = new System.Drawing.Size(38, 30);
            this.button_Qty.TabIndex = 3;
            this.button_Qty.Text = "<";
            this.button_Qty.UseVisualStyleBackColor = true;
            this.button_Qty.Click += new System.EventHandler(this.button_Qty_Click);
            // 
            // labelNoOfBundle
            // 
            this.labelNoOfBundle.Location = new System.Drawing.Point(12, 9);
            this.labelNoOfBundle.Name = "labelNoOfBundle";
            this.labelNoOfBundle.Size = new System.Drawing.Size(88, 23);
            this.labelNoOfBundle.TabIndex = 53;
            this.labelNoOfBundle.Text = "No of Bundle";
            // 
            // numNoOfBundle
            // 
            this.numNoOfBundle.BackColor = System.Drawing.Color.White;
            this.numNoOfBundle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numNoOfBundle.IsSupportEditMode = false;
            this.numNoOfBundle.Location = new System.Drawing.Point(103, 9);
            this.numNoOfBundle.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numNoOfBundle.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numNoOfBundle.Name = "numNoOfBundle";
            this.numNoOfBundle.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numNoOfBundle.Size = new System.Drawing.Size(37, 23);
            this.numNoOfBundle.TabIndex = 0;
            this.numNoOfBundle.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numNoOfBundle.Validating += new System.ComponentModel.CancelEventHandler(this.NumNoOfBundle_Validating);
            // 
            // labelTotalQty
            // 
            this.labelTotalQty.Location = new System.Drawing.Point(143, 9);
            this.labelTotalQty.Name = "labelTotalQty";
            this.labelTotalQty.Size = new System.Drawing.Size(70, 23);
            this.labelTotalQty.TabIndex = 1;
            this.labelTotalQty.Text = "Total Qty";
            // 
            // labelTotalCutOutput
            // 
            this.labelTotalCutOutput.Location = new System.Drawing.Point(274, 9);
            this.labelTotalCutOutput.Name = "labelTotalCutOutput";
            this.labelTotalCutOutput.Size = new System.Drawing.Size(105, 23);
            this.labelTotalCutOutput.TabIndex = 0;
            this.labelTotalCutOutput.Text = "Total Cut Output";
            // 
            // displayTotalQty
            // 
            this.displayTotalQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTotalQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTotalQty.Location = new System.Drawing.Point(216, 9);
            this.displayTotalQty.Name = "displayTotalQty";
            this.displayTotalQty.Size = new System.Drawing.Size(52, 23);
            this.displayTotalQty.TabIndex = 1;
            // 
            // displayTotalCutOutput
            // 
            this.displayTotalCutOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTotalCutOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTotalCutOutput.Location = new System.Drawing.Point(382, 9);
            this.displayTotalCutOutput.Name = "displayTotalCutOutput";
            this.displayTotalCutOutput.Size = new System.Drawing.Size(64, 23);
            this.displayTotalCutOutput.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label3.Location = new System.Drawing.Point(9, 205);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 23);
            this.label3.TabIndex = 59;
            this.label3.Text = "Bundle Card Data";
            this.label3.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // labelPatternPanel
            // 
            this.labelPatternPanel.Location = new System.Drawing.Point(171, 205);
            this.labelPatternPanel.Name = "labelPatternPanel";
            this.labelPatternPanel.Size = new System.Drawing.Size(93, 23);
            this.labelPatternPanel.TabIndex = 60;
            this.labelPatternPanel.Text = "Pattern Panel";
            // 
            // displayPatternPanel
            // 
            this.displayPatternPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPatternPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPatternPanel.Location = new System.Drawing.Point(266, 205);
            this.displayPatternPanel.Name = "displayPatternPanel";
            this.displayPatternPanel.Size = new System.Drawing.Size(52, 23);
            this.displayPatternPanel.TabIndex = 4;
            // 
            // labelTotalParts
            // 
            this.labelTotalParts.Location = new System.Drawing.Point(320, 205);
            this.labelTotalParts.Name = "labelTotalParts";
            this.labelTotalParts.Size = new System.Drawing.Size(82, 23);
            this.labelTotalParts.TabIndex = 62;
            this.labelTotalParts.Text = "Total Parts";
            // 
            // numTotalParts
            // 
            this.numTotalParts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalParts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalParts.IsSupportEditMode = false;
            this.numTotalParts.Location = new System.Drawing.Point(404, 205);
            this.numTotalParts.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalParts.Name = "numTotalParts";
            this.numTotalParts.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalParts.ReadOnly = true;
            this.numTotalParts.Size = new System.Drawing.Size(37, 23);
            this.numTotalParts.TabIndex = 5;
            this.numTotalParts.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label6.Location = new System.Drawing.Point(517, 205);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(163, 23);
            this.label6.TabIndex = 64;
            this.label6.Text = "All Parts Detail";
            this.label6.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // grid_Size
            // 
            this.grid_Size.AllowUserToAddRows = false;
            this.grid_Size.AllowUserToDeleteRows = false;
            this.grid_Size.AllowUserToResizeRows = false;
            this.grid_Size.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid_Size.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid_Size.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Size.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid_Size.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid_Size.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid_Size.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid_Size.Location = new System.Drawing.Point(551, 45);
            this.grid_Size.Name = "grid_Size";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid_Size.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.grid_Size.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_Size.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_Size.RowTemplate.Height = 24;
            this.grid_Size.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_Size.ShowCellToolTips = false;
            this.grid_Size.Size = new System.Drawing.Size(253, 138);
            this.grid_Size.TabIndex = 65;
            this.grid_Size.TabStop = false;
            this.grid_Size.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_Size_CellDoubleClick);
            // 
            // chkTone
            // 
            this.chkTone.AutoSize = true;
            this.chkTone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkTone.Location = new System.Drawing.Point(509, 14);
            this.chkTone.Name = "chkTone";
            this.chkTone.Size = new System.Drawing.Size(15, 14);
            this.chkTone.TabIndex = 66;
            this.chkTone.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(527, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 23);
            this.label1.TabIndex = 67;
            this.label1.Text = "Generate by Tone";
            // 
            // numTone
            // 
            this.numTone.BackColor = System.Drawing.Color.White;
            this.numTone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numTone.IsSupportEditMode = false;
            this.numTone.Location = new System.Drawing.Point(652, 9);
            this.numTone.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numTone.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTone.Name = "numTone";
            this.numTone.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTone.Size = new System.Drawing.Size(37, 23);
            this.numTone.TabIndex = 68;
            this.numTone.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // P10_Generate
            // 
            this.ClientSize = new System.Drawing.Size(1064, 489);
            this.Controls.Add(this.numTone);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkTone);
            this.Controls.Add(this.grid_Size);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numTotalParts);
            this.Controls.Add(this.labelTotalParts);
            this.Controls.Add(this.displayPatternPanel);
            this.Controls.Add(this.labelPatternPanel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.displayTotalCutOutput);
            this.Controls.Add(this.displayTotalQty);
            this.Controls.Add(this.labelTotalCutOutput);
            this.Controls.Add(this.labelTotalQty);
            this.Controls.Add(this.numNoOfBundle);
            this.Controls.Add(this.labelNoOfBundle);
            this.Controls.Add(this.button_Qty);
            this.Controls.Add(this.button_LefttoRight);
            this.Controls.Add(this.button_RighttoLeft);
            this.Controls.Add(this.btnGarment);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grid_allpart);
            this.Controls.Add(this.grid_art);
            this.Controls.Add(this.grid_qty);
            this.Name = "P10_Generate";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Bundle Card Generate";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.P10_Generate_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.grid_qty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_art)).EndInit();
            this.art_contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_allpart)).EndInit();
            this.allpart_contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_Size)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid grid_qty;
        private Win.UI.Grid grid_art;
        private Win.UI.Grid grid_allpart;
        private Win.UI.Button btnOK;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnGarment;
        private Win.UI.Button button_RighttoLeft;
        private Win.UI.Button button_LefttoRight;
        private Win.UI.Button button_Qty;
        private Win.UI.Label labelNoOfBundle;
        private Win.UI.NumericBox numNoOfBundle;
        private Win.UI.Label labelTotalQty;
        private Win.UI.Label labelTotalCutOutput;
        private Win.UI.DisplayBox displayTotalQty;
        private Win.UI.DisplayBox displayTotalCutOutput;
        private Win.UI.Label label3;
        private Win.UI.Label labelPatternPanel;
        private Win.UI.DisplayBox displayPatternPanel;
        private Win.UI.Label labelTotalParts;
        private Win.UI.NumericBox numTotalParts;
        private Win.UI.Label label6;
        private Win.UI.Grid grid_Size;
        private Win.UI.ContextMenuStrip art_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem insertIntoRecordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRecordToolStripMenuItem;
        private Win.UI.ContextMenuStrip allpart_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem allpart_insert;
        private System.Windows.Forms.ToolStripMenuItem allpart_delete;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.CheckBox chkTone;
        private Win.UI.Label label1;
        private Win.UI.NumericBox numTone;
    }
}
