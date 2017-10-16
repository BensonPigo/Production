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
            this.btn_LefttoRight = new Sci.Win.UI.Button();
            this.btn_RighttoLeft = new Sci.Win.UI.Button();
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
            this.gridCutpart = new Sci.Win.UI.Grid();
            this.art_contextMenuStrip = new Sci.Win.UI.ContextMenuStrip();
            this.insertIntoRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridArticleSize = new Sci.Win.UI.Grid();
            this.gridAllPart = new Sci.Win.UI.Grid();
            this.allpart_contextMenuStrip = new Sci.Win.UI.ContextMenuStrip();
            this.allpart_insert = new System.Windows.Forms.ToolStripMenuItem();
            this.allpart_delete = new System.Windows.Forms.ToolStripMenuItem();
            this.gridQty = new Sci.Win.UI.Grid();
            this.gridCutRef = new Sci.Win.UI.Grid();
            this.btnQuery = new Sci.Win.UI.Button();
            this.labelGroup = new Sci.Win.UI.Label();
            this.txtfactoryByM = new Sci.Production.Class.txtfactory();
            this.label1 = new Sci.Win.UI.Label();
            this.radioGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCutpart)).BeginInit();
            this.art_contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridArticleSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAllPart)).BeginInit();
            this.allpart_contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCutRef)).BeginInit();
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
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
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
            this.btnBatchCreate.Click += new System.EventHandler(this.btnBatchCreate_Click);
            // 
            // label_TotalQty
            // 
            this.label_TotalQty.BackColor = System.Drawing.Color.Transparent;
            this.label_TotalQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
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
            this.btnCopy_to_same_Cutref.Click += new System.EventHandler(this.btnCopy_to_same_Cutref_Click);
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
            this.btnCopy_to_other_Cutref.Click += new System.EventHandler(this.btnCopy_to_other_Cutref_Click);
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
            this.btnColorComb.Click += new System.EventHandler(this.btnColorComb_Click);
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
            this.btnGarmentList.Click += new System.EventHandler(this.btnGarmentList_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Location = new System.Drawing.Point(646, 320);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 23);
            this.label5.TabIndex = 93;
            this.label5.Text = "All Parts Detail";
            this.label5.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // btn_LefttoRight
            // 
            this.btn_LefttoRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.btn_LefttoRight.Location = new System.Drawing.Point(605, 415);
            this.btn_LefttoRight.Name = "btn_LefttoRight";
            this.btn_LefttoRight.Size = new System.Drawing.Size(38, 30);
            this.btn_LefttoRight.TabIndex = 9;
            this.btn_LefttoRight.Text = ">";
            this.btn_LefttoRight.UseVisualStyleBackColor = true;
            this.btn_LefttoRight.Click += new System.EventHandler(this.btn_LefttoRight_Click);
            // 
            // btn_RighttoLeft
            // 
            this.btn_RighttoLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.btn_RighttoLeft.Location = new System.Drawing.Point(605, 484);
            this.btn_RighttoLeft.Name = "btn_RighttoLeft";
            this.btn_RighttoLeft.Size = new System.Drawing.Size(38, 30);
            this.btn_RighttoLeft.TabIndex = 10;
            this.btn_RighttoLeft.Text = "<";
            this.btn_RighttoLeft.UseVisualStyleBackColor = true;
            this.btn_RighttoLeft.Click += new System.EventHandler(this.btn_RighttoLeft_Click);
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
            this.numNoOfBundle.Validated += new System.EventHandler(this.numNoOfBundle_Validated);
            // 
            // label_TotalCutOutput
            // 
            this.label_TotalCutOutput.BackColor = System.Drawing.Color.Transparent;
            this.label_TotalCutOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
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
            this.labelNoOfBundle.Location = new System.Drawing.Point(8, 311);
            this.labelNoOfBundle.Name = "labelNoOfBundle";
            this.labelNoOfBundle.Size = new System.Drawing.Size(90, 23);
            this.labelNoOfBundle.TabIndex = 88;
            this.labelNoOfBundle.Text = "No of Bundle";
            // 
            // labelTotalQty
            // 
            this.labelTotalQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
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
            this.txtPOID.Location = new System.Drawing.Point(577, 9);
            this.txtPOID.Name = "txtPOID";
            this.txtPOID.Size = new System.Drawing.Size(108, 21);
            this.txtPOID.TabIndex = 2;
            // 
            // labelPOID
            // 
            this.labelPOID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelPOID.Location = new System.Drawing.Point(499, 8);
            this.labelPOID.Name = "labelPOID";
            this.labelPOID.Size = new System.Drawing.Size(75, 23);
            this.labelPOID.TabIndex = 83;
            this.labelPOID.Text = "PO ID";
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
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
            this.labelCutref.Location = new System.Drawing.Point(9, 8);
            this.labelCutref.Name = "labelCutref";
            this.labelCutref.Size = new System.Drawing.Size(75, 23);
            this.labelCutref.TabIndex = 80;
            this.labelCutref.Text = "Cutref#";
            // 
            // gridCutpart
            // 
            this.gridCutpart.AllowUserToAddRows = false;
            this.gridCutpart.AllowUserToDeleteRows = false;
            this.gridCutpart.AllowUserToResizeRows = false;
            this.gridCutpart.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCutpart.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCutpart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCutpart.ContextMenuStrip = this.art_contextMenuStrip;
            this.gridCutpart.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCutpart.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCutpart.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCutpart.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCutpart.Location = new System.Drawing.Point(160, 346);
            this.gridCutpart.Name = "gridCutpart";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridCutpart.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridCutpart.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCutpart.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCutpart.RowTemplate.Height = 24;
            this.gridCutpart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCutpart.Size = new System.Drawing.Size(439, 230);
            this.gridCutpart.TabIndex = 79;
            this.gridCutpart.TabStop = false;
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
            // gridArticleSize
            // 
            this.gridArticleSize.AllowUserToAddRows = false;
            this.gridArticleSize.AllowUserToDeleteRows = false;
            this.gridArticleSize.AllowUserToResizeRows = false;
            this.gridArticleSize.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridArticleSize.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridArticleSize.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridArticleSize.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridArticleSize.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridArticleSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridArticleSize.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridArticleSize.Location = new System.Drawing.Point(460, 38);
            this.gridArticleSize.Name = "gridArticleSize";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridArticleSize.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridArticleSize.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridArticleSize.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridArticleSize.RowTemplate.Height = 24;
            this.gridArticleSize.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridArticleSize.Size = new System.Drawing.Size(537, 267);
            this.gridArticleSize.TabIndex = 20;
            this.gridArticleSize.TabStop = false;
            this.gridArticleSize.SelectionChanged += new System.EventHandler(this.gridArticleSize_SelectionChanged);
            // 
            // gridAllPart
            // 
            this.gridAllPart.AllowUserToAddRows = false;
            this.gridAllPart.AllowUserToDeleteRows = false;
            this.gridAllPart.AllowUserToResizeRows = false;
            this.gridAllPart.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridAllPart.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridAllPart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAllPart.ContextMenuStrip = this.allpart_contextMenuStrip;
            this.gridAllPart.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridAllPart.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridAllPart.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridAllPart.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridAllPart.Location = new System.Drawing.Point(646, 346);
            this.gridAllPart.Name = "gridAllPart";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridAllPart.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridAllPart.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridAllPart.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridAllPart.RowTemplate.Height = 24;
            this.gridAllPart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAllPart.Size = new System.Drawing.Size(353, 230);
            this.gridAllPart.TabIndex = 76;
            this.gridAllPart.TabStop = false;
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
            // gridQty
            // 
            this.gridQty.AllowUserToAddRows = false;
            this.gridQty.AllowUserToDeleteRows = false;
            this.gridQty.AllowUserToResizeRows = false;
            this.gridQty.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridQty.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridQty.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridQty.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridQty.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridQty.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridQty.Location = new System.Drawing.Point(8, 346);
            this.gridQty.Name = "gridQty";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridQty.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridQty.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridQty.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridQty.RowTemplate.Height = 24;
            this.gridQty.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridQty.Size = new System.Drawing.Size(148, 230);
            this.gridQty.TabIndex = 77;
            this.gridQty.TabStop = false;
            // 
            // gridCutRef
            // 
            this.gridCutRef.AllowUserToAddRows = false;
            this.gridCutRef.AllowUserToDeleteRows = false;
            this.gridCutRef.AllowUserToResizeRows = false;
            this.gridCutRef.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCutRef.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCutRef.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCutRef.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCutRef.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCutRef.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCutRef.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCutRef.Location = new System.Drawing.Point(8, 38);
            this.gridCutRef.Name = "gridCutRef";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridCutRef.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.gridCutRef.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCutRef.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCutRef.RowTemplate.Height = 24;
            this.gridCutRef.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCutRef.Size = new System.Drawing.Size(446, 267);
            this.gridCutRef.TabIndex = 22;
            this.gridCutRef.TabStop = false;
            this.gridCutRef.SelectionChanged += new System.EventHandler(this.gridCutRef_SelectionChanged);
            // 
            // btnQuery
            // 
            this.btnQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnQuery.Location = new System.Drawing.Point(695, 4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 3;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // labelGroup
            // 
            this.labelGroup.BackColor = System.Drawing.Color.Transparent;
            this.labelGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.labelGroup.Location = new System.Drawing.Point(657, 591);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(35, 23);
            this.labelGroup.TabIndex = 17;
            this.labelGroup.Text = "Group";
            this.labelGroup.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.labelGroup.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // txtfactoryByM
            // 
            this.txtfactoryByM.BackColor = System.Drawing.Color.White;
            this.txtfactoryByM.FilteMDivision = true;
            this.txtfactoryByM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactoryByM.IssupportJunk = false;
            this.txtfactoryByM.Location = new System.Drawing.Point(430, 8);
            this.txtfactoryByM.Name = "txtfactoryByM";
            this.txtfactoryByM.Size = new System.Drawing.Size(66, 23);
            this.txtfactoryByM.TabIndex = 96;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label1.Location = new System.Drawing.Point(382, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 23);
            this.label1.TabIndex = 97;
            this.label1.Text = "Factory";
            // 
            // P11
            // 
            this.ClientSize = new System.Drawing.Size(1008, 630);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtfactoryByM);
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
            this.Controls.Add(this.btn_LefttoRight);
            this.Controls.Add(this.btn_RighttoLeft);
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
            this.Controls.Add(this.gridCutpart);
            this.Controls.Add(this.gridArticleSize);
            this.Controls.Add(this.gridAllPart);
            this.Controls.Add(this.gridQty);
            this.Controls.Add(this.gridCutRef);
            this.DefaultControl = "txtCutref";
            this.DefaultControlForEdit = "txtCutref";
            this.EditMode = true;
            this.Name = "P11";
            this.Text = "P11.Batch Create Bundle Card";
            this.Controls.SetChildIndex(this.gridCutRef, 0);
            this.Controls.SetChildIndex(this.gridQty, 0);
            this.Controls.SetChildIndex(this.gridAllPart, 0);
            this.Controls.SetChildIndex(this.gridArticleSize, 0);
            this.Controls.SetChildIndex(this.gridCutpart, 0);
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
            this.Controls.SetChildIndex(this.btn_RighttoLeft, 0);
            this.Controls.SetChildIndex(this.btn_LefttoRight, 0);
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
            this.Controls.SetChildIndex(this.txtfactoryByM, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCutpart)).EndInit();
            this.art_contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridArticleSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAllPart)).EndInit();
            this.allpart_contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCutRef)).EndInit();
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
        private Win.UI.Button btn_LefttoRight;
        private Win.UI.Button btn_RighttoLeft;
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
        private Win.UI.Grid gridCutpart;
        private Win.UI.Grid gridArticleSize;
        private Win.UI.Grid gridAllPart;
        private Win.UI.Grid gridQty;
        private Win.UI.Grid gridCutRef;
        private Win.UI.Button btnQuery;
        private Win.UI.ContextMenuStrip allpart_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem allpart_insert;
        private System.Windows.Forms.ToolStripMenuItem allpart_delete;
        private Win.UI.ContextMenuStrip art_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem insertIntoRecordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRecordToolStripMenuItem;
        private Win.UI.Label labelGroup;
        private Class.txtfactory txtfactoryByM;
        private Win.UI.Label label1;
    }
}
