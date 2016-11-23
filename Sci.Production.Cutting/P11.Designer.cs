namespace Sci.Production.Cutting
{
    partial class P11
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.withcuto = new Sci.Win.UI.RadioButton();
            this.radioGroup1 = new Sci.Win.UI.RadioGroup();
            this.begin1 = new Sci.Win.UI.RadioButton();
            this.Close_Button = new Sci.Win.UI.Button();
            this.BatchCreate_Button = new Sci.Win.UI.Button();
            this.label_TotalQty = new Sci.Win.UI.Label();
            this.copy_to_same_Cutref = new Sci.Win.UI.Button();
            this.copy_to_other_Cutref = new Sci.Win.UI.Button();
            this.button1 = new Sci.Win.UI.Button();
            this.totalpart_numericBox = new Sci.Win.UI.NumericBox();
            this.label7 = new Sci.Win.UI.Label();
            this.button3 = new Sci.Win.UI.Button();
            this.label5 = new Sci.Win.UI.Label();
            this.button_LefttoRight = new Sci.Win.UI.Button();
            this.button_RighttoLeft = new Sci.Win.UI.Button();
            this.numericBox_noofbundle = new Sci.Win.UI.NumericBox();
            this.label_TotalCutOutput = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.CutDate_dateBox = new Sci.Win.UI.DateBox();
            this.POID_TextBox = new Sci.Win.UI.TextBox();
            this.SP_label = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.Cutref_textBox = new Sci.Win.UI.TextBox();
            this.Cutref_label = new Sci.Win.UI.Label();
            this.Cutpart_grid = new Sci.Win.UI.Grid();
            this.art_contextMenuStrip = new Sci.Win.UI.ContextMenuStrip();
            this.insertIntoRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ArticleSize_grid = new Sci.Win.UI.Grid();
            this.AllPart_grid = new Sci.Win.UI.Grid();
            this.allpart_contextMenuStrip = new Sci.Win.UI.ContextMenuStrip();
            this.allpart_insert = new System.Windows.Forms.ToolStripMenuItem();
            this.allpart_delete = new System.Windows.Forms.ToolStripMenuItem();
            this.Qty_grid = new Sci.Win.UI.Grid();
            this.CutRef_grid = new Sci.Win.UI.Grid();
            this.Query_button = new Sci.Win.UI.Button();
            this.label3 = new Sci.Win.UI.Label();
            this.radioGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Cutpart_grid)).BeginInit();
            this.art_contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ArticleSize_grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AllPart_grid)).BeginInit();
            this.allpart_contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Qty_grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CutRef_grid)).BeginInit();
            this.SuspendLayout();
            // 
            // withcuto
            // 
            this.withcuto.AutoSize = true;
            this.withcuto.Checked = true;
            this.withcuto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.withcuto.Location = new System.Drawing.Point(10, 2);
            this.withcuto.Name = "withcuto";
            this.withcuto.Size = new System.Drawing.Size(74, 19);
            this.withcuto.TabIndex = 0;
            this.withcuto.TabStop = true;
            this.withcuto.Text = "with auto";
            this.withcuto.UseVisualStyleBackColor = true;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.begin1);
            this.radioGroup1.Controls.Add(this.withcuto);
            this.radioGroup1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.radioGroup1.IsSupportEditMode = false;
            this.radioGroup1.Location = new System.Drawing.Point(685, 582);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(114, 43);
            this.radioGroup1.TabIndex = 103;
            this.radioGroup1.TabStop = false;
            // 
            // begin1
            // 
            this.begin1.AutoSize = true;
            this.begin1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.begin1.Location = new System.Drawing.Point(10, 21);
            this.begin1.Name = "begin1";
            this.begin1.Size = new System.Drawing.Size(91, 19);
            this.begin1.TabIndex = 1;
            this.begin1.Text = "begin with 1";
            this.begin1.UseVisualStyleBackColor = true;
            // 
            // Close_Button
            // 
            this.Close_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Close_Button.Location = new System.Drawing.Point(917, 584);
            this.Close_Button.Name = "Close_Button";
            this.Close_Button.Size = new System.Drawing.Size(80, 30);
            this.Close_Button.TabIndex = 102;
            this.Close_Button.Text = "Close";
            this.Close_Button.UseVisualStyleBackColor = true;
            this.Close_Button.Click += new System.EventHandler(this.Close_Button_Click);
            // 
            // BatchCreate_Button
            // 
            this.BatchCreate_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.BatchCreate_Button.Location = new System.Drawing.Point(811, 584);
            this.BatchCreate_Button.Name = "BatchCreate_Button";
            this.BatchCreate_Button.Size = new System.Drawing.Size(100, 30);
            this.BatchCreate_Button.TabIndex = 101;
            this.BatchCreate_Button.Text = "Batch Create";
            this.BatchCreate_Button.UseVisualStyleBackColor = true;
            this.BatchCreate_Button.Click += new System.EventHandler(this.BatchCreate_Button_Click);
            // 
            // label_TotalQty
            // 
            this.label_TotalQty.BackColor = System.Drawing.Color.Transparent;
            this.label_TotalQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label_TotalQty.Lines = 0;
            this.label_TotalQty.Location = new System.Drawing.Point(87, 585);
            this.label_TotalQty.Name = "label_TotalQty";
            this.label_TotalQty.Size = new System.Drawing.Size(54, 23);
            this.label_TotalQty.TabIndex = 100;
            this.label_TotalQty.Text = "0";
            this.label_TotalQty.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label_TotalQty.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // copy_to_same_Cutref
            // 
            this.copy_to_same_Cutref.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.copy_to_same_Cutref.Location = new System.Drawing.Point(158, 582);
            this.copy_to_same_Cutref.Name = "copy_to_same_Cutref";
            this.copy_to_same_Cutref.Size = new System.Drawing.Size(224, 30);
            this.copy_to_same_Cutref.TabIndex = 99;
            this.copy_to_same_Cutref.Text = "Copy Cutpart/Artwork to same CutRef#";
            this.copy_to_same_Cutref.UseVisualStyleBackColor = true;
            this.copy_to_same_Cutref.Click += new System.EventHandler(this.copy_to_same_Cutref_Click);
            // 
            // copy_to_other_Cutref
            // 
            this.copy_to_other_Cutref.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.copy_to_other_Cutref.Location = new System.Drawing.Point(383, 582);
            this.copy_to_other_Cutref.Name = "copy_to_other_Cutref";
            this.copy_to_other_Cutref.Size = new System.Drawing.Size(219, 30);
            this.copy_to_other_Cutref.TabIndex = 98;
            this.copy_to_other_Cutref.Text = "Copy Cutpart/Artwork to other CutRef#";
            this.copy_to_other_Cutref.UseVisualStyleBackColor = true;
            this.copy_to_other_Cutref.Click += new System.EventHandler(this.copy_to_other_Cutref_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.button1.Location = new System.Drawing.Point(902, 310);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 30);
            this.button1.TabIndex = 97;
            this.button1.Text = "Color Comb";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // totalpart_numericBox
            // 
            this.totalpart_numericBox.BackColor = System.Drawing.Color.White;
            this.totalpart_numericBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.totalpart_numericBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.totalpart_numericBox.IsSupportEditMode = false;
            this.totalpart_numericBox.Location = new System.Drawing.Point(446, 312);
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
            this.totalpart_numericBox.Size = new System.Drawing.Size(37, 21);
            this.totalpart_numericBox.TabIndex = 96;
            this.totalpart_numericBox.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(362, 312);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 23);
            this.label7.TabIndex = 95;
            this.label7.Text = "Total Parts";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.button3.Location = new System.Drawing.Point(488, 310);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(97, 30);
            this.button3.TabIndex = 94;
            this.button3.Text = "Garment List";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(646, 320);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 23);
            this.label5.TabIndex = 93;
            this.label5.Text = "All Parts Detail";
            this.label5.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // button_LefttoRight
            // 
            this.button_LefttoRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.button_LefttoRight.Location = new System.Drawing.Point(605, 415);
            this.button_LefttoRight.Name = "button_LefttoRight";
            this.button_LefttoRight.Size = new System.Drawing.Size(38, 30);
            this.button_LefttoRight.TabIndex = 92;
            this.button_LefttoRight.Text = ">";
            this.button_LefttoRight.UseVisualStyleBackColor = true;
            this.button_LefttoRight.Click += new System.EventHandler(this.button_LefttoRight_Click);
            // 
            // button_RighttoLeft
            // 
            this.button_RighttoLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.button_RighttoLeft.Location = new System.Drawing.Point(605, 484);
            this.button_RighttoLeft.Name = "button_RighttoLeft";
            this.button_RighttoLeft.Size = new System.Drawing.Size(38, 30);
            this.button_RighttoLeft.TabIndex = 91;
            this.button_RighttoLeft.Text = "<";
            this.button_RighttoLeft.UseVisualStyleBackColor = true;
            this.button_RighttoLeft.Click += new System.EventHandler(this.button_RighttoLeft_Click);
            // 
            // numericBox_noofbundle
            // 
            this.numericBox_noofbundle.BackColor = System.Drawing.Color.White;
            this.numericBox_noofbundle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.numericBox_noofbundle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox_noofbundle.Location = new System.Drawing.Point(101, 312);
            this.numericBox_noofbundle.Name = "numericBox_noofbundle";
            this.numericBox_noofbundle.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox_noofbundle.Size = new System.Drawing.Size(35, 21);
            this.numericBox_noofbundle.TabIndex = 90;
            this.numericBox_noofbundle.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox_noofbundle.Validated += new System.EventHandler(this.numericBox_noofbundle_Validated);
            // 
            // label_TotalCutOutput
            // 
            this.label_TotalCutOutput.BackColor = System.Drawing.Color.Transparent;
            this.label_TotalCutOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label_TotalCutOutput.Lines = 0;
            this.label_TotalCutOutput.Location = new System.Drawing.Point(250, 312);
            this.label_TotalCutOutput.Name = "label_TotalCutOutput";
            this.label_TotalCutOutput.Size = new System.Drawing.Size(54, 23);
            this.label_TotalCutOutput.TabIndex = 89;
            this.label_TotalCutOutput.Text = "0";
            this.label_TotalCutOutput.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label_TotalCutOutput.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(8, 311);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 23);
            this.label1.TabIndex = 88;
            this.label1.Text = "No of Bundle";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(9, 585);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 23);
            this.label6.TabIndex = 87;
            this.label6.Text = "Total Qty";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(139, 312);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 23);
            this.label4.TabIndex = 86;
            this.label4.Text = "Total Cut Output:";
            this.label4.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // CutDate_dateBox
            // 
            this.CutDate_dateBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.CutDate_dateBox.Location = new System.Drawing.Point(249, 9);
            this.CutDate_dateBox.Name = "CutDate_dateBox";
            this.CutDate_dateBox.Size = new System.Drawing.Size(130, 21);
            this.CutDate_dateBox.TabIndex = 1;
            // 
            // POID_TextBox
            // 
            this.POID_TextBox.BackColor = System.Drawing.Color.White;
            this.POID_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.POID_TextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.POID_TextBox.Location = new System.Drawing.Point(479, 9);
            this.POID_TextBox.Name = "POID_TextBox";
            this.POID_TextBox.Size = new System.Drawing.Size(108, 21);
            this.POID_TextBox.TabIndex = 2;
            // 
            // SP_label
            // 
            this.SP_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.SP_label.Lines = 0;
            this.SP_label.Location = new System.Drawing.Point(401, 8);
            this.SP_label.Name = "SP_label";
            this.SP_label.Size = new System.Drawing.Size(75, 23);
            this.SP_label.TabIndex = 83;
            this.SP_label.Text = "PO ID";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(162, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 23);
            this.label2.TabIndex = 82;
            this.label2.Text = "Est.CutDate";
            // 
            // Cutref_textBox
            // 
            this.Cutref_textBox.BackColor = System.Drawing.Color.White;
            this.Cutref_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Cutref_textBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Cutref_textBox.Location = new System.Drawing.Point(86, 9);
            this.Cutref_textBox.Name = "Cutref_textBox";
            this.Cutref_textBox.Size = new System.Drawing.Size(70, 21);
            this.Cutref_textBox.TabIndex = 0;
            // 
            // Cutref_label
            // 
            this.Cutref_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Cutref_label.Lines = 0;
            this.Cutref_label.Location = new System.Drawing.Point(9, 8);
            this.Cutref_label.Name = "Cutref_label";
            this.Cutref_label.Size = new System.Drawing.Size(75, 23);
            this.Cutref_label.TabIndex = 80;
            this.Cutref_label.Text = "Cutref#";
            // 
            // Cutpart_grid
            // 
            this.Cutpart_grid.AllowUserToAddRows = false;
            this.Cutpart_grid.AllowUserToDeleteRows = false;
            this.Cutpart_grid.AllowUserToResizeRows = false;
            this.Cutpart_grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.Cutpart_grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.Cutpart_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Cutpart_grid.ContextMenuStrip = this.art_contextMenuStrip;
            this.Cutpart_grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.Cutpart_grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.Cutpart_grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.Cutpart_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.Cutpart_grid.Location = new System.Drawing.Point(160, 346);
            this.Cutpart_grid.Name = "Cutpart_grid";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Cutpart_grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.Cutpart_grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.Cutpart_grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.Cutpart_grid.RowTemplate.Height = 24;
            this.Cutpart_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Cutpart_grid.Size = new System.Drawing.Size(439, 230);
            this.Cutpart_grid.TabIndex = 79;
            this.Cutpart_grid.TabStop = false;
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
            // ArticleSize_grid
            // 
            this.ArticleSize_grid.AllowUserToAddRows = false;
            this.ArticleSize_grid.AllowUserToDeleteRows = false;
            this.ArticleSize_grid.AllowUserToResizeRows = false;
            this.ArticleSize_grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.ArticleSize_grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.ArticleSize_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ArticleSize_grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.ArticleSize_grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.ArticleSize_grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ArticleSize_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.ArticleSize_grid.Location = new System.Drawing.Point(417, 38);
            this.ArticleSize_grid.Name = "ArticleSize_grid";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ArticleSize_grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ArticleSize_grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.ArticleSize_grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.ArticleSize_grid.RowTemplate.Height = 24;
            this.ArticleSize_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ArticleSize_grid.Size = new System.Drawing.Size(580, 267);
            this.ArticleSize_grid.TabIndex = 78;
            this.ArticleSize_grid.TabStop = false;
            this.ArticleSize_grid.SelectionChanged += new System.EventHandler(this.ArticleSize_grid_SelectionChanged);
            // 
            // AllPart_grid
            // 
            this.AllPart_grid.AllowUserToAddRows = false;
            this.AllPart_grid.AllowUserToDeleteRows = false;
            this.AllPart_grid.AllowUserToResizeRows = false;
            this.AllPart_grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.AllPart_grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.AllPart_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AllPart_grid.ContextMenuStrip = this.allpart_contextMenuStrip;
            this.AllPart_grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.AllPart_grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.AllPart_grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.AllPart_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.AllPart_grid.Location = new System.Drawing.Point(646, 346);
            this.AllPart_grid.Name = "AllPart_grid";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.AllPart_grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.AllPart_grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.AllPart_grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.AllPart_grid.RowTemplate.Height = 24;
            this.AllPart_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.AllPart_grid.Size = new System.Drawing.Size(353, 230);
            this.AllPart_grid.TabIndex = 76;
            this.AllPart_grid.TabStop = false;
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
            // Qty_grid
            // 
            this.Qty_grid.AllowUserToAddRows = false;
            this.Qty_grid.AllowUserToDeleteRows = false;
            this.Qty_grid.AllowUserToResizeRows = false;
            this.Qty_grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.Qty_grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.Qty_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Qty_grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.Qty_grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.Qty_grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.Qty_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.Qty_grid.Location = new System.Drawing.Point(8, 346);
            this.Qty_grid.Name = "Qty_grid";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Qty_grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.Qty_grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.Qty_grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.Qty_grid.RowTemplate.Height = 24;
            this.Qty_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Qty_grid.Size = new System.Drawing.Size(148, 230);
            this.Qty_grid.TabIndex = 77;
            this.Qty_grid.TabStop = false;
            // 
            // CutRef_grid
            // 
            this.CutRef_grid.AllowUserToAddRows = false;
            this.CutRef_grid.AllowUserToDeleteRows = false;
            this.CutRef_grid.AllowUserToResizeRows = false;
            this.CutRef_grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.CutRef_grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.CutRef_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CutRef_grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.CutRef_grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.CutRef_grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.CutRef_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.CutRef_grid.Location = new System.Drawing.Point(8, 38);
            this.CutRef_grid.Name = "CutRef_grid";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CutRef_grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.CutRef_grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.CutRef_grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.CutRef_grid.RowTemplate.Height = 24;
            this.CutRef_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CutRef_grid.Size = new System.Drawing.Size(399, 267);
            this.CutRef_grid.TabIndex = 75;
            this.CutRef_grid.TabStop = false;
            this.CutRef_grid.SelectionChanged += new System.EventHandler(this.CutRef_grid_SelectionChanged);
            // 
            // Query_button
            // 
            this.Query_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Query_button.Location = new System.Drawing.Point(593, 4);
            this.Query_button.Name = "Query_button";
            this.Query_button.Size = new System.Drawing.Size(80, 30);
            this.Query_button.TabIndex = 3;
            this.Query_button.Text = "Query";
            this.Query_button.UseVisualStyleBackColor = true;
            this.Query_button.Click += new System.EventHandler(this.Query_button_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(657, 591);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 23);
            this.label3.TabIndex = 104;
            this.label3.Text = "Group";
            this.label3.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label3.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // P11
            // 
            this.ClientSize = new System.Drawing.Size(1008, 630);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Query_button);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.Close_Button);
            this.Controls.Add(this.BatchCreate_Button);
            this.Controls.Add(this.label_TotalQty);
            this.Controls.Add(this.copy_to_same_Cutref);
            this.Controls.Add(this.copy_to_other_Cutref);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.totalpart_numericBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_LefttoRight);
            this.Controls.Add(this.button_RighttoLeft);
            this.Controls.Add(this.numericBox_noofbundle);
            this.Controls.Add(this.label_TotalCutOutput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CutDate_dateBox);
            this.Controls.Add(this.POID_TextBox);
            this.Controls.Add(this.SP_label);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Cutref_textBox);
            this.Controls.Add(this.Cutref_label);
            this.Controls.Add(this.Cutpart_grid);
            this.Controls.Add(this.ArticleSize_grid);
            this.Controls.Add(this.AllPart_grid);
            this.Controls.Add(this.Qty_grid);
            this.Controls.Add(this.CutRef_grid);
            this.EditMode = true;
            this.Name = "P11";
            this.Text = "P11.Batch Create Bundle Card";
            this.Controls.SetChildIndex(this.CutRef_grid, 0);
            this.Controls.SetChildIndex(this.Qty_grid, 0);
            this.Controls.SetChildIndex(this.AllPart_grid, 0);
            this.Controls.SetChildIndex(this.ArticleSize_grid, 0);
            this.Controls.SetChildIndex(this.Cutpart_grid, 0);
            this.Controls.SetChildIndex(this.Cutref_label, 0);
            this.Controls.SetChildIndex(this.Cutref_textBox, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.SP_label, 0);
            this.Controls.SetChildIndex(this.POID_TextBox, 0);
            this.Controls.SetChildIndex(this.CutDate_dateBox, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label_TotalCutOutput, 0);
            this.Controls.SetChildIndex(this.numericBox_noofbundle, 0);
            this.Controls.SetChildIndex(this.button_RighttoLeft, 0);
            this.Controls.SetChildIndex(this.button_LefttoRight, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.button3, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.totalpart_numericBox, 0);
            this.Controls.SetChildIndex(this.button1, 0);
            this.Controls.SetChildIndex(this.copy_to_other_Cutref, 0);
            this.Controls.SetChildIndex(this.copy_to_same_Cutref, 0);
            this.Controls.SetChildIndex(this.label_TotalQty, 0);
            this.Controls.SetChildIndex(this.BatchCreate_Button, 0);
            this.Controls.SetChildIndex(this.Close_Button, 0);
            this.Controls.SetChildIndex(this.radioGroup1, 0);
            this.Controls.SetChildIndex(this.Query_button, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Cutpart_grid)).EndInit();
            this.art_contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ArticleSize_grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AllPart_grid)).EndInit();
            this.allpart_contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Qty_grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CutRef_grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioButton withcuto;
        private Win.UI.RadioGroup radioGroup1;
        private Win.UI.RadioButton begin1;
        private Win.UI.Button Close_Button;
        private Win.UI.Button BatchCreate_Button;
        private Win.UI.Label label_TotalQty;
        private Win.UI.Button copy_to_same_Cutref;
        private Win.UI.Button copy_to_other_Cutref;
        private Win.UI.Button button1;
        private Win.UI.NumericBox totalpart_numericBox;
        private Win.UI.Label label7;
        private Win.UI.Button button3;
        private Win.UI.Label label5;
        private Win.UI.Button button_LefttoRight;
        private Win.UI.Button button_RighttoLeft;
        private Win.UI.NumericBox numericBox_noofbundle;
        private Win.UI.Label label_TotalCutOutput;
        private Win.UI.Label label1;
        private Win.UI.Label label6;
        private Win.UI.Label label4;
        private Win.UI.DateBox CutDate_dateBox;
        private Win.UI.TextBox POID_TextBox;
        private Win.UI.Label SP_label;
        private Win.UI.Label label2;
        private Win.UI.TextBox Cutref_textBox;
        private Win.UI.Label Cutref_label;
        private Win.UI.Grid Cutpart_grid;
        private Win.UI.Grid ArticleSize_grid;
        private Win.UI.Grid AllPart_grid;
        private Win.UI.Grid Qty_grid;
        private Win.UI.Grid CutRef_grid;
        private Win.UI.Button Query_button;
        private Win.UI.ContextMenuStrip allpart_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem allpart_insert;
        private System.Windows.Forms.ToolStripMenuItem allpart_delete;
        private Win.UI.ContextMenuStrip art_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem insertIntoRecordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRecordToolStripMenuItem;
        private Win.UI.Label label3;
    }
}
