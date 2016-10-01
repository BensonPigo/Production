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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grid_qty = new Sci.Win.UI.Grid();
            this.grid_art = new Sci.Win.UI.Grid();
            this.art_contextMenuStrip = new Sci.Win.UI.ContextMenuStrip();
            this.insertIntoRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grid_allpart = new Sci.Win.UI.Grid();
            this.allpart_contextMenuStrip = new Sci.Win.UI.ContextMenuStrip();
            this.allpart_insert = new System.Windows.Forms.ToolStripMenuItem();
            this.allpart_delete = new System.Windows.Forms.ToolStripMenuItem();
            this.OK_button = new Sci.Win.UI.Button();
            this.button2 = new Sci.Win.UI.Button();
            this.button3 = new Sci.Win.UI.Button();
            this.button_RighttoLeft = new Sci.Win.UI.Button();
            this.button_LefttoRight = new Sci.Win.UI.Button();
            this.button_Qty = new Sci.Win.UI.Button();
            this.label13 = new Sci.Win.UI.Label();
            this.numericBox_noBundle = new Sci.Win.UI.NumericBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.displayBox_Cutoutput = new Sci.Win.UI.DisplayBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.displayBox_pattern = new Sci.Win.UI.DisplayBox();
            this.label5 = new Sci.Win.UI.Label();
            this.totalpart_numericBox = new Sci.Win.UI.NumericBox();
            this.label6 = new Sci.Win.UI.Label();
            this.grid_Size = new Sci.Win.UI.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.grid_qty)).BeginInit();
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
            this.grid_qty.Size = new System.Drawing.Size(450, 138);
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
            this.grid_art.ReadOnly = true;
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
            this.grid_art.Size = new System.Drawing.Size(453, 208);
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
            this.grid_allpart.Location = new System.Drawing.Point(521, 238);
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
            this.grid_allpart.Size = new System.Drawing.Size(463, 208);
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
            // OK_button
            // 
            this.OK_button.Location = new System.Drawing.Point(817, 453);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(80, 30);
            this.OK_button.TabIndex = 4;
            this.OK_button.Text = "OK";
            this.OK_button.UseVisualStyleBackColor = true;
            this.OK_button.Click += new System.EventHandler(this.OK_button_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(903, 453);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 30);
            this.button2.TabIndex = 5;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(9, 453);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(111, 30);
            this.button3.TabIndex = 48;
            this.button3.Text = "Garment List";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button_RighttoLeft
            // 
            this.button_RighttoLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.button_RighttoLeft.Location = new System.Drawing.Point(467, 357);
            this.button_RighttoLeft.Name = "button_RighttoLeft";
            this.button_RighttoLeft.Size = new System.Drawing.Size(38, 30);
            this.button_RighttoLeft.TabIndex = 50;
            this.button_RighttoLeft.Text = "<";
            this.button_RighttoLeft.UseVisualStyleBackColor = true;
            this.button_RighttoLeft.Click += new System.EventHandler(this.button_RighttoLeft_Click);
            // 
            // button_LefttoRight
            // 
            this.button_LefttoRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.button_LefttoRight.Location = new System.Drawing.Point(467, 288);
            this.button_LefttoRight.Name = "button_LefttoRight";
            this.button_LefttoRight.Size = new System.Drawing.Size(38, 30);
            this.button_LefttoRight.TabIndex = 51;
            this.button_LefttoRight.Text = ">";
            this.button_LefttoRight.UseVisualStyleBackColor = true;
            this.button_LefttoRight.Click += new System.EventHandler(this.button_LefttoRight_Click);
            // 
            // button_Qty
            // 
            this.button_Qty.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.button_Qty.Location = new System.Drawing.Point(467, 88);
            this.button_Qty.Name = "button_Qty";
            this.button_Qty.Size = new System.Drawing.Size(38, 30);
            this.button_Qty.TabIndex = 52;
            this.button_Qty.Text = "<";
            this.button_Qty.UseVisualStyleBackColor = true;
            this.button_Qty.Click += new System.EventHandler(this.button_Qty_Click);
            // 
            // label13
            // 
            this.label13.Lines = 0;
            this.label13.Location = new System.Drawing.Point(12, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(88, 23);
            this.label13.TabIndex = 53;
            this.label13.Text = "No of Bundle";
            // 
            // numericBox_noBundle
            // 
            this.numericBox_noBundle.BackColor = System.Drawing.Color.White;
            this.numericBox_noBundle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox_noBundle.IsSupportEditMode = false;
            this.numericBox_noBundle.Location = new System.Drawing.Point(103, 9);
            this.numericBox_noBundle.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericBox_noBundle.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericBox_noBundle.Name = "numericBox_noBundle";
            this.numericBox_noBundle.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox_noBundle.Size = new System.Drawing.Size(37, 23);
            this.numericBox_noBundle.TabIndex = 54;
            this.numericBox_noBundle.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox_noBundle.Validated += new System.EventHandler(this.numericBox_noBundle_Validated);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(153, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 23);
            this.label1.TabIndex = 55;
            this.label1.Text = "Total Qty";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(284, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 23);
            this.label2.TabIndex = 56;
            this.label2.Text = "Total Cut Output";
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(226, 9);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(52, 23);
            this.displayBox1.TabIndex = 57;
            // 
            // displayBox_Cutoutput
            // 
            this.displayBox_Cutoutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox_Cutoutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox_Cutoutput.Location = new System.Drawing.Point(392, 9);
            this.displayBox_Cutoutput.Name = "displayBox_Cutoutput";
            this.displayBox_Cutoutput.Size = new System.Drawing.Size(64, 23);
            this.displayBox_Cutoutput.TabIndex = 58;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(9, 205);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 23);
            this.label3.TabIndex = 59;
            this.label3.Text = "Bundle Card Data";
            this.label3.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(171, 205);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 23);
            this.label4.TabIndex = 60;
            this.label4.Text = "Pattern Panel";
            // 
            // displayBox_pattern
            // 
            this.displayBox_pattern.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox_pattern.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox_pattern.Location = new System.Drawing.Point(266, 205);
            this.displayBox_pattern.Name = "displayBox_pattern";
            this.displayBox_pattern.Size = new System.Drawing.Size(52, 23);
            this.displayBox_pattern.TabIndex = 61;
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(320, 205);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 23);
            this.label5.TabIndex = 62;
            this.label5.Text = "Total Parts";
            // 
            // totalpart_numericBox
            // 
            this.totalpart_numericBox.BackColor = System.Drawing.Color.White;
            this.totalpart_numericBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.totalpart_numericBox.IsSupportEditMode = false;
            this.totalpart_numericBox.Location = new System.Drawing.Point(404, 205);
            this.totalpart_numericBox.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.totalpart_numericBox.Name = "totalpart_numericBox";
            this.totalpart_numericBox.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.totalpart_numericBox.Size = new System.Drawing.Size(37, 23);
            this.totalpart_numericBox.TabIndex = 63;
            this.totalpart_numericBox.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(485, 205);
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
            this.grid_Size.Location = new System.Drawing.Point(519, 45);
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
            this.grid_Size.Size = new System.Drawing.Size(253, 138);
            this.grid_Size.TabIndex = 65;
            this.grid_Size.TabStop = false;
            // 
            // P10_Generate
            // 
            this.ClientSize = new System.Drawing.Size(991, 489);
            this.Controls.Add(this.grid_Size);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.totalpart_numericBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.displayBox_pattern);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.displayBox_Cutoutput);
            this.Controls.Add(this.displayBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericBox_noBundle);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.button_Qty);
            this.Controls.Add(this.button_LefttoRight);
            this.Controls.Add(this.button_RighttoLeft);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.OK_button);
            this.Controls.Add(this.grid_allpart);
            this.Controls.Add(this.grid_art);
            this.Controls.Add(this.grid_qty);
            this.Name = "P10_Generate";
            this.Text = "Bundle Card Generate";
            ((System.ComponentModel.ISupportInitialize)(this.grid_qty)).EndInit();
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
        private Win.UI.Button OK_button;
        private Win.UI.Button button2;
        private Win.UI.Button button3;
        private Win.UI.Button button_RighttoLeft;
        private Win.UI.Button button_LefttoRight;
        private Win.UI.Button button_Qty;
        private Win.UI.Label label13;
        private Win.UI.NumericBox numericBox_noBundle;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.DisplayBox displayBox_Cutoutput;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.DisplayBox displayBox_pattern;
        private Win.UI.Label label5;
        private Win.UI.NumericBox totalpart_numericBox;
        private Win.UI.Label label6;
        private Win.UI.Grid grid_Size;
        private Win.UI.ContextMenuStrip art_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem insertIntoRecordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRecordToolStripMenuItem;
        private Win.UI.ContextMenuStrip allpart_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem allpart_insert;
        private System.Windows.Forms.ToolStripMenuItem allpart_delete;
    }
}
