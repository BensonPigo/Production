namespace Sci.Production.Packing
{
    partial class P28_UA
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.BindingSourcePackingFile = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel5 = new Sci.Win.UI.Panel();
            this.btnRemovePackingFile = new Sci.Win.UI.Button();
            this.btnAddPackingFile = new Sci.Win.UI.Button();
            this.panel15 = new Sci.Win.UI.Panel();
            this.gridItemFile = new Sci.Win.UI.Grid();
            this.BindingSourceItemFile = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel13 = new Sci.Win.UI.Panel();
            this.btnRemoveItemFile = new Sci.Win.UI.Button();
            this.btnAddItemFile = new Sci.Win.UI.Button();
            this.panel10 = new Sci.Win.UI.Panel();
            this.gridMatch = new Sci.Win.UI.Grid();
            this.BindingSourceMatch = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel7 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnConfirm = new Sci.Win.UI.Button();
            this.panel6 = new Sci.Win.UI.Panel();
            this.gridErrorMsg = new Sci.Win.UI.Grid();
            this.BindingSourceMsgGrid = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnMapping = new Sci.Win.UI.Button();
            this.openFileDialogPackingList = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogItemFile = new System.Windows.Forms.OpenFileDialog();
            this.gridPackingFile = new Sci.Win.UI.Grid();
            this.panel9 = new Sci.Win.UI.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourcePackingFile)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridItemFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourceItemFile)).BeginInit();
            this.panel13.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMatch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourceMatch)).BeginInit();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridErrorMsg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourceMsgGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPackingFile)).BeginInit();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel10);
            this.splitContainer1.Panel2.Controls.Add(this.panel7);
            this.splitContainer1.Panel2.Controls.Add(this.panel6);
            this.splitContainer1.Size = new System.Drawing.Size(1084, 603);
            this.splitContainer1.SplitterDistance = 492;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel9);
            this.splitContainer2.Panel1.Controls.Add(this.panel5);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel15);
            this.splitContainer2.Panel2.Controls.Add(this.panel13);
            this.splitContainer2.Size = new System.Drawing.Size(492, 603);
            this.splitContainer2.SplitterDistance = 297;
            this.splitContainer2.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label1);
            this.panel5.Controls.Add(this.btnRemovePackingFile);
            this.panel5.Controls.Add(this.btnAddPackingFile);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(488, 39);
            this.panel5.TabIndex = 3;
            // 
            // btnRemovePackingFile
            // 
            this.btnRemovePackingFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemovePackingFile.Location = new System.Drawing.Point(357, 4);
            this.btnRemovePackingFile.Name = "btnRemovePackingFile";
            this.btnRemovePackingFile.Size = new System.Drawing.Size(128, 30);
            this.btnRemovePackingFile.TabIndex = 2;
            this.btnRemovePackingFile.Text = "Remove File";
            this.btnRemovePackingFile.UseVisualStyleBackColor = true;
            this.btnRemovePackingFile.Click += new System.EventHandler(this.BtnRemovePackingFile_Click);
            // 
            // btnAddPackingFile
            // 
            this.btnAddPackingFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPackingFile.Location = new System.Drawing.Point(266, 4);
            this.btnAddPackingFile.Name = "btnAddPackingFile";
            this.btnAddPackingFile.Size = new System.Drawing.Size(85, 30);
            this.btnAddPackingFile.TabIndex = 1;
            this.btnAddPackingFile.Text = "Add File";
            this.btnAddPackingFile.UseVisualStyleBackColor = true;
            this.btnAddPackingFile.Click += new System.EventHandler(this.BtnAddPackingFile_Click);
            // 
            // panel15
            // 
            this.panel15.Controls.Add(this.gridItemFile);
            this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel15.Location = new System.Drawing.Point(0, 41);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(488, 257);
            this.panel15.TabIndex = 6;
            // 
            // gridItemFile
            // 
            this.gridItemFile.AllowUserToAddRows = false;
            this.gridItemFile.AllowUserToDeleteRows = false;
            this.gridItemFile.AllowUserToResizeRows = false;
            this.gridItemFile.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridItemFile.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridItemFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridItemFile.DataSource = this.BindingSourceItemFile;
            this.gridItemFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridItemFile.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridItemFile.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridItemFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridItemFile.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridItemFile.Location = new System.Drawing.Point(0, 0);
            this.gridItemFile.Name = "gridItemFile";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridItemFile.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.gridItemFile.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridItemFile.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridItemFile.RowTemplate.Height = 24;
            this.gridItemFile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridItemFile.ShowCellToolTips = false;
            this.gridItemFile.Size = new System.Drawing.Size(488, 257);
            this.gridItemFile.TabIndex = 0;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.label4);
            this.panel13.Controls.Add(this.btnRemoveItemFile);
            this.panel13.Controls.Add(this.btnAddItemFile);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel13.Location = new System.Drawing.Point(0, 0);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(488, 41);
            this.panel13.TabIndex = 4;
            // 
            // btnRemoveItemFile
            // 
            this.btnRemoveItemFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveItemFile.Location = new System.Drawing.Point(357, 5);
            this.btnRemoveItemFile.Name = "btnRemoveItemFile";
            this.btnRemoveItemFile.Size = new System.Drawing.Size(128, 30);
            this.btnRemoveItemFile.TabIndex = 2;
            this.btnRemoveItemFile.Text = "Remove File";
            this.btnRemoveItemFile.UseVisualStyleBackColor = true;
            this.btnRemoveItemFile.Click += new System.EventHandler(this.BtnRemoveItemFile_Click);
            // 
            // btnAddItemFile
            // 
            this.btnAddItemFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddItemFile.Location = new System.Drawing.Point(266, 5);
            this.btnAddItemFile.Name = "btnAddItemFile";
            this.btnAddItemFile.Size = new System.Drawing.Size(85, 30);
            this.btnAddItemFile.TabIndex = 1;
            this.btnAddItemFile.Text = "Add File";
            this.btnAddItemFile.UseVisualStyleBackColor = true;
            this.btnAddItemFile.Click += new System.EventHandler(this.BtnAddItemFile_Click);
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.gridMatch);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(0, 39);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(584, 519);
            this.panel10.TabIndex = 5;
            // 
            // gridMatch
            // 
            this.gridMatch.AllowUserToAddRows = false;
            this.gridMatch.AllowUserToDeleteRows = false;
            this.gridMatch.AllowUserToResizeRows = false;
            this.gridMatch.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridMatch.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridMatch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMatch.DataSource = this.BindingSourceMatch;
            this.gridMatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMatch.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridMatch.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridMatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridMatch.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridMatch.Location = new System.Drawing.Point(0, 0);
            this.gridMatch.Name = "gridMatch";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridMatch.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.gridMatch.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridMatch.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridMatch.RowTemplate.Height = 24;
            this.gridMatch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMatch.ShowCellToolTips = false;
            this.gridMatch.Size = new System.Drawing.Size(584, 519);
            this.gridMatch.TabIndex = 1;
            this.gridMatch.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridMatch_ColumnHeaderMouseClick);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnClose);
            this.panel7.Controls.Add(this.btnConfirm);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(0, 558);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(584, 41);
            this.panel7.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(499, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(82, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(408, 5);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(85, 30);
            this.btnConfirm.TabIndex = 3;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label2);
            this.panel6.Controls.Add(this.gridErrorMsg);
            this.panel6.Controls.Add(this.btnMapping);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(584, 39);
            this.panel6.TabIndex = 3;
            // 
            // gridErrorMsg
            // 
            this.gridErrorMsg.AllowUserToAddRows = false;
            this.gridErrorMsg.AllowUserToDeleteRows = false;
            this.gridErrorMsg.AllowUserToResizeRows = false;
            this.gridErrorMsg.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridErrorMsg.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridErrorMsg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridErrorMsg.DataSource = this.BindingSourceMsgGrid;
            this.gridErrorMsg.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridErrorMsg.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridErrorMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridErrorMsg.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridErrorMsg.Location = new System.Drawing.Point(501, 16);
            this.gridErrorMsg.Name = "gridErrorMsg";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridErrorMsg.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.gridErrorMsg.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridErrorMsg.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridErrorMsg.RowTemplate.Height = 24;
            this.gridErrorMsg.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridErrorMsg.ShowCellToolTips = false;
            this.gridErrorMsg.Size = new System.Drawing.Size(49, 16);
            this.gridErrorMsg.TabIndex = 12;
            this.gridErrorMsg.Visible = false;
            // 
            // btnMapping
            // 
            this.btnMapping.Location = new System.Drawing.Point(3, 3);
            this.btnMapping.Name = "btnMapping";
            this.btnMapping.Size = new System.Drawing.Size(118, 30);
            this.btnMapping.TabIndex = 3;
            this.btnMapping.Text = "Mapping PL";
            this.btnMapping.UseVisualStyleBackColor = true;
            this.btnMapping.Click += new System.EventHandler(this.BtnMapping_Click);
            // 
            // openFileDialogPackingList
            // 
            this.openFileDialogPackingList.FileName = "openFileDialog1";
            this.openFileDialogPackingList.Multiselect = true;
            // 
            // openFileDialogItemFile
            // 
            this.openFileDialogItemFile.FileName = "openFileDialog1";
            this.openFileDialogItemFile.Multiselect = true;
            // 
            // gridPackingFile
            // 
            this.gridPackingFile.AllowUserToAddRows = false;
            this.gridPackingFile.AllowUserToDeleteRows = false;
            this.gridPackingFile.AllowUserToResizeRows = false;
            this.gridPackingFile.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridPackingFile.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridPackingFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPackingFile.DataSource = this.BindingSourcePackingFile;
            this.gridPackingFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPackingFile.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridPackingFile.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridPackingFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridPackingFile.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridPackingFile.Location = new System.Drawing.Point(0, 0);
            this.gridPackingFile.Name = "gridPackingFile";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridPackingFile.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.gridPackingFile.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridPackingFile.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridPackingFile.RowTemplate.Height = 24;
            this.gridPackingFile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPackingFile.ShowCellToolTips = false;
            this.gridPackingFile.Size = new System.Drawing.Size(488, 254);
            this.gridPackingFile.TabIndex = 0;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.gridPackingFile);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(0, 39);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(488, 254);
            this.panel9.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 22);
            this.label1.TabIndex = 4;
            this.label1.Text = "Packing List  Report";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.label2.Location = new System.Drawing.Point(263, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 22);
            this.label2.TabIndex = 14;
            this.label2.Text = "Match";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.label4.Location = new System.Drawing.Point(3, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 22);
            this.label4.TabIndex = 16;
            this.label4.Text = "Item Size Report";
            // 
            // P28_UA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 603);
            this.Controls.Add(this.splitContainer1);
            this.Name = "P28_UA";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P28. Upload Packing List - Cust CTN# (U.ARMOUR)";
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourcePackingFile)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel15.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridItemFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourceItemFile)).EndInit();
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.panel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMatch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourceMatch)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridErrorMsg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourceMsgGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPackingFile)).EndInit();
            this.panel9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Panel panel10;
        private Win.UI.Grid gridMatch;
        private Win.UI.ListControlBindingSource BindingSourceMatch;
        private Win.UI.Panel panel7;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnConfirm;
        private Win.UI.Panel panel6;
        private Win.UI.Button btnMapping;
        private Win.UI.ListControlBindingSource BindingSourcePackingFile;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Win.UI.Panel panel5;
        private Win.UI.Button btnRemovePackingFile;
        private Win.UI.Button btnAddPackingFile;
        private Win.UI.Panel panel15;
        private Win.UI.Grid gridItemFile;
        private Win.UI.Panel panel13;
        private Win.UI.Button btnRemoveItemFile;
        private Win.UI.Button btnAddItemFile;
        private Win.UI.ListControlBindingSource BindingSourceItemFile;
        private System.Windows.Forms.OpenFileDialog openFileDialogPackingList;
        private System.Windows.Forms.OpenFileDialog openFileDialogItemFile;
        private Win.UI.ListControlBindingSource BindingSourceMsgGrid;
        private Win.UI.Grid gridErrorMsg;
        private Win.UI.Panel panel9;
        private Win.UI.Grid gridPackingFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
    }
}