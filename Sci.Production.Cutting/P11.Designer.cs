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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.radioWithcuto = new Sci.Win.UI.RadioButton();
            this.radioGroup1 = new Sci.Win.UI.RadioGroup();
            this.radiobegin1 = new Sci.Win.UI.RadioButton();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnBatchCreate = new Sci.Win.UI.Button();
            this.label_TotalQty = new Sci.Win.UI.Label();
            this.btnCopy_to_same_Cutref = new Sci.Win.UI.Button();
            this.btnCopy_to_other_Cutref = new Sci.Win.UI.Button();
            this.btnColorComb = new Sci.Win.UI.Button();
            this.numTotalPart = new Sci.Win.UI.NumericBox();
            this.label7 = new Sci.Win.UI.Label();
            this.btnGarmentList = new Sci.Win.UI.Button();
            this.label5 = new Sci.Win.UI.Label();
            this.button_LefttoRight = new Sci.Win.UI.Button();
            this.button_RighttoLeft = new Sci.Win.UI.Button();
            this.numNoOfBundle = new Sci.Win.UI.NumericBox();
            this.label_TotalCutOutput = new Sci.Win.UI.Label();
            this.labelNoOfBundle = new Sci.Win.UI.Label();
            this.labelTotalQty = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.dateEstCutDate = new Sci.Win.UI.DateBox();
            this.txtPOID = new Sci.Win.UI.TextBox();
            this.labelPOID = new Sci.Win.UI.Label();
            this.labelEstCutDate = new Sci.Win.UI.Label();
            this.txtCutref = new Sci.Win.UI.TextBox();
            this.labelCutref = new Sci.Win.UI.Label();
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
            this.btnQuery = new Sci.Win.UI.Button();
            this.labelGroup = new Sci.Win.UI.Label();
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
            // radioWithcuto
            // 
            this.radioWithcuto.AutoSize = true;
            this.radioWithcuto.Checked = true;
            this.radioWithcuto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioWithcuto.Location = new System.Drawing.Point(10, 2);
            this.radioWithcuto.Name = "radioWithcuto";
            this.radioWithcuto.Size = new System.Drawing.Size(74, 19);
            this.radioWithcuto.TabIndex = 0;
            this.radioWithcuto.TabStop = true;
            this.radioWithcuto.Text = "with auto";
            this.radioWithcuto.UseVisualStyleBackColor = true;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.radiobegin1);
            this.radioGroup1.Controls.Add(this.radioWithcuto);
            this.radioGroup1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.radioGroup1.IsSupportEditMode = false;
            this.radioGroup1.Location = new System.Drawing.Point(685, 582);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(114, 43);
            this.radioGroup1.TabIndex = 14;
            this.radioGroup1.TabStop = false;
            // 
            // radiobegin1
            // 
            this.radiobegin1.AutoSize = true;
            this.radiobegin1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobegin1.Location = new System.Drawing.Point(10, 21);
            this.radiobegin1.Name = "radiobegin1";
            this.radiobegin1.Size = new System.Drawing.Size(91, 19);
            this.radiobegin1.TabIndex = 1;
            this.radiobegin1.Text = "begin with 1";
            this.radiobegin1.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnClose.Location = new System.Drawing.Point(917, 584);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.Close_Button_Click);
            // 
            // btnBatchCreate
            // 
            this.btnBatchCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnBatchCreate.Location = new System.Drawing.Point(811, 584);
            this.btnBatchCreate.Name = "btnBatchCreate";
            this.btnBatchCreate.Size = new System.Drawing.Size(100, 30);
            this.btnBatchCreate.TabIndex = 15;
            this.btnBatchCreate.Text = "Batch Create";
            this.btnBatchCreate.UseVisualStyleBackColor = true;
            this.btnBatchCreate.Click += new System.EventHandler(this.BatchCreate_Button_Click);
            // 
            // label_TotalQty
            // 
            this.label_TotalQty.BackColor = System.Drawing.Color.Transparent;
            this.label_TotalQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label_TotalQty.Lines = 0;
            this.label_TotalQty.Location = new System.Drawing.Point(87, 585);
            this.label_TotalQty.Name = "label_TotalQty";
            this.label_TotalQty.Size = new System.Drawing.Size(54, 23);
            this.label_TotalQty.TabIndex = 11;
            this.label_TotalQty.Text = "0";
            this.label_TotalQty.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label_TotalQty.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // btnCopy_to_same_Cutref
            // 
            this.btnCopy_to_same_Cutref.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnCopy_to_same_Cutref.Location = new System.Drawing.Point(158, 582);
            this.btnCopy_to_same_Cutref.Name = "btnCopy_to_same_Cutref";
            this.btnCopy_to_same_Cutref.Size = new System.Drawing.Size(224, 30);
            this.btnCopy_to_same_Cutref.TabIndex = 12;
            this.btnCopy_to_same_Cutref.Text = "Copy Cutpart/Artwork to same CutRef#";
            this.btnCopy_to_same_Cutref.UseVisualStyleBackColor = true;
            this.btnCopy_to_same_Cutref.Click += new System.EventHandler(this.copy_to_same_Cutref_Click);
            // 
            // btnCopy_to_other_Cutref
            // 
            this.btnCopy_to_other_Cutref.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnCopy_to_other_Cutref.Location = new System.Drawing.Point(383, 582);
            this.btnCopy_to_other_Cutref.Name = "btnCopy_to_other_Cutref";
            this.btnCopy_to_other_Cutref.Size = new System.Drawing.Size(219, 30);
            this.btnCopy_to_other_Cutref.TabIndex = 13;
            this.btnCopy_to_other_Cutref.Text = "Copy Cutpart/Artwork to other CutRef#";
            this.btnCopy_to_other_Cutref.UseVisualStyleBackColor = true;
            this.btnCopy_to_other_Cutref.Click += new System.EventHandler(this.copy_to_other_Cutref_Click);
            // 
            // btnColorComb
            // 
            this.btnColorComb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnColorComb.Location = new System.Drawing.Point(902, 310);
            this.btnColorComb.Name = "btnColorComb";
            this.btnColorComb.Size = new System.Drawing.Size(97, 30);
            this.btnColorComb.TabIndex = 8;
            this.btnColorComb.Text = "Color Comb";
            this.btnColorComb.UseVisualStyleBackColor = true;
            this.btnColorComb.Click += new System.EventHandler(this.button1_Click);
            // 
            // numTotalPart
            // 
            this.numTotalPart.BackColor = System.Drawing.Color.White;
            this.numTotalPart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.numTotalPart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numTotalPart.IsSupportEditMode = false;
            this.numTotalPart.Location = new System.Drawing.Point(446, 312);
            this.numTotalPart.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalPart.Name = "numTotalPart";
            this.numTotalPart.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalPart.Size = new System.Drawing.Size(37, 21);
            this.numTotalPart.TabIndex = 6;
            this.numTotalPart.Value = new decimal(new int[] {
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
            // btnGarmentList
            // 
            this.btnGarmentList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnGarmentList.Location = new System.Drawing.Point(488, 310);
            this.btnGarmentList.Name = "btnGarmentList";
            this.btnGarmentList.Size = new System.Drawing.Size(97, 30);
            this.btnGarmentList.TabIndex = 7;
            this.btnGarmentList.Text = "Garment List";
            this.btnGarmentList.UseVisualStyleBackColor = true;
            this.btnGarmentList.Click += new System.EventHandler(this.button3_Click);
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
            this.button_LefttoRight.TabIndex = 9;
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
            this.button_RighttoLeft.TabIndex = 10;
            this.button_RighttoLeft.Text = "<";
            this.button_RighttoLeft.UseVisualStyleBackColor = true;
            this.button_RighttoLeft.Click += new System.EventHandler(this.button_RighttoLeft_Click);
            // 
            // numNoOfBundle
            // 
            this.numNoOfBundle.BackColor = System.Drawing.Color.White;
            this.numNoOfBundle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.numNoOfBundle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numNoOfBundle.Location = new System.Drawing.Point(101, 312);
            this.numNoOfBundle.Name = "numNoOfBundle";
            this.numNoOfBundle.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numNoOfBundle.Size = new System.Drawing.Size(35, 21);
            this.numNoOfBundle.TabIndex = 4;
            this.numNoOfBundle.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numNoOfBundle.Validated += new System.EventHandler(this.numericBox_noofbundle_Validated);
            // 
            // label_TotalCutOutput
            // 
            this.label_TotalCutOutput.BackColor = System.Drawing.Color.Transparent;
            this.label_TotalCutOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label_TotalCutOutput.Lines = 0;
            this.label_TotalCutOutput.Location = new System.Drawing.Point(250, 312);
            this.label_TotalCutOutput.Name = "label_TotalCutOutput";
            this.label_TotalCutOutput.Size = new System.Drawing.Size(54, 23);
            this.label_TotalCutOutput.TabIndex = 5;
            this.label_TotalCutOutput.Text = "0";
            this.label_TotalCutOutput.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label_TotalCutOutput.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelNoOfBundle
            // 
            this.labelNoOfBundle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelNoOfBundle.Lines = 0;
            this.labelNoOfBundle.Location = new System.Drawing.Point(8, 311);
            this.labelNoOfBundle.Name = "labelNoOfBundle";
            this.labelNoOfBundle.Size = new System.Drawing.Size(90, 23);
            this.labelNoOfBundle.TabIndex = 88;
            this.labelNoOfBundle.Text = "No of Bundle";
            // 
            // labelTotalQty
            // 
            this.labelTotalQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelTotalQty.Lines = 0;
            this.labelTotalQty.Location = new System.Drawing.Point(9, 585);
            this.labelTotalQty.Name = "labelTotalQty";
            this.labelTotalQty.Size = new System.Drawing.Size(75, 23);
            this.labelTotalQty.TabIndex = 87;
            this.labelTotalQty.Text = "Total Qty";
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
            // dateEstCutDate
            // 
            this.dateEstCutDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.dateEstCutDate.Location = new System.Drawing.Point(249, 9);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(130, 21);
            this.dateEstCutDate.TabIndex = 1;
            // 
            // txtPOID
            // 
            this.txtPOID.BackColor = System.Drawing.Color.White;
            this.txtPOID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtPOID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPOID.Location = new System.Drawing.Point(479, 9);
            this.txtPOID.Name = "txtPOID";
            this.txtPOID.Size = new System.Drawing.Size(108, 21);
            this.txtPOID.TabIndex = 2;
            // 
            // labelPOID
            // 
            this.labelPOID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelPOID.Lines = 0;
            this.labelPOID.Location = new System.Drawing.Point(401, 8);
            this.labelPOID.Name = "labelPOID";
            this.labelPOID.Size = new System.Drawing.Size(75, 23);
            this.labelPOID.TabIndex = 83;
            this.labelPOID.Text = "PO ID";
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelEstCutDate.Lines = 0;
            this.labelEstCutDate.Location = new System.Drawing.Point(162, 8);
            this.labelEstCutDate.Name = "labelEstCutDate";
            this.labelEstCutDate.Size = new System.Drawing.Size(83, 23);
            this.labelEstCutDate.TabIndex = 82;
            this.labelEstCutDate.Text = "Est.CutDate";
            // 
            // txtCutref
            // 
            this.txtCutref.BackColor = System.Drawing.Color.White;
            this.txtCutref.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtCutref.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutref.Location = new System.Drawing.Point(86, 9);
            this.txtCutref.Name = "txtCutref";
            this.txtCutref.Size = new System.Drawing.Size(70, 21);
            this.txtCutref.TabIndex = 0;
            // 
            // labelCutref
            // 
            this.labelCutref.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelCutref.Lines = 0;
            this.labelCutref.Location = new System.Drawing.Point(9, 8);
            this.labelCutref.Name = "labelCutref";
            this.labelCutref.Size = new System.Drawing.Size(75, 23);
            this.labelCutref.TabIndex = 80;
            this.labelCutref.Text = "Cutref#";
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Cutpart_grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
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
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ArticleSize_grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ArticleSize_grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.ArticleSize_grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.ArticleSize_grid.RowTemplate.Height = 24;
            this.ArticleSize_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ArticleSize_grid.Size = new System.Drawing.Size(580, 267);
            this.ArticleSize_grid.TabIndex = 20;
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
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.AllPart_grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
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
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Qty_grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
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
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CutRef_grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.CutRef_grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.CutRef_grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.CutRef_grid.RowTemplate.Height = 24;
            this.CutRef_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CutRef_grid.Size = new System.Drawing.Size(399, 267);
            this.CutRef_grid.TabIndex = 22;
            this.CutRef_grid.TabStop = false;
            this.CutRef_grid.SelectionChanged += new System.EventHandler(this.CutRef_grid_SelectionChanged);
            // 
            // btnQuery
            // 
            this.btnQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnQuery.Location = new System.Drawing.Point(593, 4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 3;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.Query_button_Click);
            // 
            // labelGroup
            // 
            this.labelGroup.BackColor = System.Drawing.Color.Transparent;
            this.labelGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.labelGroup.Lines = 0;
            this.labelGroup.Location = new System.Drawing.Point(657, 591);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(35, 23);
            this.labelGroup.TabIndex = 17;
            this.labelGroup.Text = "Group";
            this.labelGroup.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.labelGroup.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // P11
            // 
            this.ClientSize = new System.Drawing.Size(1008, 630);
            this.Controls.Add(this.labelGroup);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnBatchCreate);
            this.Controls.Add(this.label_TotalQty);
            this.Controls.Add(this.btnCopy_to_same_Cutref);
            this.Controls.Add(this.btnCopy_to_other_Cutref);
            this.Controls.Add(this.btnColorComb);
            this.Controls.Add(this.numTotalPart);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnGarmentList);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_LefttoRight);
            this.Controls.Add(this.button_RighttoLeft);
            this.Controls.Add(this.numNoOfBundle);
            this.Controls.Add(this.label_TotalCutOutput);
            this.Controls.Add(this.labelNoOfBundle);
            this.Controls.Add(this.labelTotalQty);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateEstCutDate);
            this.Controls.Add(this.txtPOID);
            this.Controls.Add(this.labelPOID);
            this.Controls.Add(this.labelEstCutDate);
            this.Controls.Add(this.txtCutref);
            this.Controls.Add(this.labelCutref);
            this.Controls.Add(this.Cutpart_grid);
            this.Controls.Add(this.ArticleSize_grid);
            this.Controls.Add(this.AllPart_grid);
            this.Controls.Add(this.Qty_grid);
            this.Controls.Add(this.CutRef_grid);
            this.DefaultControl = "Cutref_textBox";
            this.DefaultControlForEdit = "Cutref_textBox";
            this.EditMode = true;
            this.Name = "P11";
            this.Text = "P11.Batch Create Bundle Card";
            this.Controls.SetChildIndex(this.CutRef_grid, 0);
            this.Controls.SetChildIndex(this.Qty_grid, 0);
            this.Controls.SetChildIndex(this.AllPart_grid, 0);
            this.Controls.SetChildIndex(this.ArticleSize_grid, 0);
            this.Controls.SetChildIndex(this.Cutpart_grid, 0);
            this.Controls.SetChildIndex(this.labelCutref, 0);
            this.Controls.SetChildIndex(this.txtCutref, 0);
            this.Controls.SetChildIndex(this.labelEstCutDate, 0);
            this.Controls.SetChildIndex(this.labelPOID, 0);
            this.Controls.SetChildIndex(this.txtPOID, 0);
            this.Controls.SetChildIndex(this.dateEstCutDate, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.labelTotalQty, 0);
            this.Controls.SetChildIndex(this.labelNoOfBundle, 0);
            this.Controls.SetChildIndex(this.label_TotalCutOutput, 0);
            this.Controls.SetChildIndex(this.numNoOfBundle, 0);
            this.Controls.SetChildIndex(this.button_RighttoLeft, 0);
            this.Controls.SetChildIndex(this.button_LefttoRight, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.btnGarmentList, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.numTotalPart, 0);
            this.Controls.SetChildIndex(this.btnColorComb, 0);
            this.Controls.SetChildIndex(this.btnCopy_to_other_Cutref, 0);
            this.Controls.SetChildIndex(this.btnCopy_to_same_Cutref, 0);
            this.Controls.SetChildIndex(this.label_TotalQty, 0);
            this.Controls.SetChildIndex(this.btnBatchCreate, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.radioGroup1, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.labelGroup, 0);
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

        private Win.UI.RadioButton radioWithcuto;
        private Win.UI.RadioGroup radioGroup1;
        private Win.UI.RadioButton radiobegin1;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnBatchCreate;
        private Win.UI.Label label_TotalQty;
        private Win.UI.Button btnCopy_to_same_Cutref;
        private Win.UI.Button btnCopy_to_other_Cutref;
        private Win.UI.Button btnColorComb;
        private Win.UI.NumericBox numTotalPart;
        private Win.UI.Label label7;
        private Win.UI.Button btnGarmentList;
        private Win.UI.Label label5;
        private Win.UI.Button button_LefttoRight;
        private Win.UI.Button button_RighttoLeft;
        private Win.UI.NumericBox numNoOfBundle;
        private Win.UI.Label label_TotalCutOutput;
        private Win.UI.Label labelNoOfBundle;
        private Win.UI.Label labelTotalQty;
        private Win.UI.Label label4;
        private Win.UI.DateBox dateEstCutDate;
        private Win.UI.TextBox txtPOID;
        private Win.UI.Label labelPOID;
        private Win.UI.Label labelEstCutDate;
        private Win.UI.TextBox txtCutref;
        private Win.UI.Label labelCutref;
        private Win.UI.Grid Cutpart_grid;
        private Win.UI.Grid ArticleSize_grid;
        private Win.UI.Grid AllPart_grid;
        private Win.UI.Grid Qty_grid;
        private Win.UI.Grid CutRef_grid;
        private Win.UI.Button btnQuery;
        private Win.UI.ContextMenuStrip allpart_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem allpart_insert;
        private System.Windows.Forms.ToolStripMenuItem allpart_delete;
        private Win.UI.ContextMenuStrip art_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem insertIntoRecordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRecordToolStripMenuItem;
        private Win.UI.Label labelGroup;
    }
}
