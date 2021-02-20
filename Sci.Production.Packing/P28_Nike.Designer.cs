namespace Sci.Production.Packing
{
    partial class P28_Nike
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel9 = new Sci.Win.UI.Panel();
            this.gridFile = new Sci.Win.UI.Grid();
            this.BindingSourceFile = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel8 = new Sci.Win.UI.Panel();
            this.panel5 = new Sci.Win.UI.Panel();
            this.btnRemoveFile = new Sci.Win.UI.Button();
            this.btnAddFile = new Sci.Win.UI.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel10 = new Sci.Win.UI.Panel();
            this.gridMatch = new Sci.Win.UI.Grid();
            this.BindingSourceMatch = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel7 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnConfirm = new Sci.Win.UI.Button();
            this.panel6 = new Sci.Win.UI.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMapping = new Sci.Win.UI.Button();
            this.panel4 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourceFile)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMatch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourceMatch)).BeginInit();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel9);
            this.splitContainer1.Panel1.Controls.Add(this.panel8);
            this.splitContainer1.Panel1.Controls.Add(this.panel5);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel10);
            this.splitContainer1.Panel2.Controls.Add(this.panel7);
            this.splitContainer1.Panel2.Controls.Add(this.panel6);
            this.splitContainer1.Panel2.Controls.Add(this.panel4);
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            this.splitContainer1.Size = new System.Drawing.Size(1084, 603);
            this.splitContainer1.SplitterDistance = 492;
            this.splitContainer1.TabIndex = 1;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.gridFile);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(10, 47);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(470, 544);
            this.panel9.TabIndex = 4;
            // 
            // gridFile
            // 
            this.gridFile.AllowUserToAddRows = false;
            this.gridFile.AllowUserToDeleteRows = false;
            this.gridFile.AllowUserToResizeRows = false;
            this.gridFile.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridFile.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFile.DataSource = this.BindingSourceFile;
            this.gridFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFile.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridFile.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridFile.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridFile.Location = new System.Drawing.Point(0, 0);
            this.gridFile.Name = "gridFile";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridFile.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridFile.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridFile.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridFile.RowTemplate.Height = 24;
            this.gridFile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFile.ShowCellToolTips = false;
            this.gridFile.Size = new System.Drawing.Size(470, 544);
            this.gridFile.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(10, 591);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(470, 10);
            this.panel8.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnRemoveFile);
            this.panel5.Controls.Add(this.btnAddFile);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(10, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(470, 47);
            this.panel5.TabIndex = 2;
            // 
            // btnRemoveFile
            // 
            this.btnRemoveFile.Location = new System.Drawing.Point(338, 9);
            this.btnRemoveFile.Name = "btnRemoveFile";
            this.btnRemoveFile.Size = new System.Drawing.Size(128, 30);
            this.btnRemoveFile.TabIndex = 2;
            this.btnRemoveFile.Text = "Remove File";
            this.btnRemoveFile.UseVisualStyleBackColor = true;
            this.btnRemoveFile.Click += new System.EventHandler(this.BtnRemoveFile_Click);
            // 
            // btnAddFile
            // 
            this.btnAddFile.Location = new System.Drawing.Point(247, 9);
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new System.Drawing.Size(85, 30);
            this.btnAddFile.TabIndex = 1;
            this.btnAddFile.Text = "Add File";
            this.btnAddFile.UseVisualStyleBackColor = true;
            this.btnAddFile.Click += new System.EventHandler(this.BtnAddFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Packing List  Report";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(480, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 601);
            this.panel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 601);
            this.panel1.TabIndex = 0;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.gridMatch);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(10, 47);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(566, 507);
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
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridMatch.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridMatch.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridMatch.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridMatch.RowTemplate.Height = 24;
            this.gridMatch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMatch.ShowCellToolTips = false;
            this.gridMatch.Size = new System.Drawing.Size(566, 507);
            this.gridMatch.TabIndex = 1;
            this.gridMatch.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridMatch_ColumnHeaderMouseClick);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnClose);
            this.panel7.Controls.Add(this.btnConfirm);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(10, 554);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(566, 47);
            this.panel7.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(478, 9);
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
            this.btnConfirm.Location = new System.Drawing.Point(387, 9);
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
            this.panel6.Controls.Add(this.btnMapping);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(10, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(566, 47);
            this.panel6.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(302, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Match";
            // 
            // btnMapping
            // 
            this.btnMapping.Location = new System.Drawing.Point(6, 9);
            this.btnMapping.Name = "btnMapping";
            this.btnMapping.Size = new System.Drawing.Size(118, 30);
            this.btnMapping.TabIndex = 3;
            this.btnMapping.Text = "Mapping PL";
            this.btnMapping.UseVisualStyleBackColor = true;
            this.btnMapping.Click += new System.EventHandler(this.BtnMapping_Click);
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(576, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(10, 601);
            this.panel4.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(10, 601);
            this.panel3.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            // 
            // P28_Nike
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 603);
            this.Controls.Add(this.splitContainer1);
            this.Name = "P28_Nike";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P28. Upload Packing List - Cust CTN# (Nike)";
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourceFile)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMatch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSourceMatch)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Panel panel9;
        private Win.UI.Grid gridFile;
        private Win.UI.ListControlBindingSource BindingSourceFile;
        private Win.UI.Panel panel8;
        private Win.UI.Panel panel5;
        private Win.UI.Button btnRemoveFile;
        private Win.UI.Button btnAddFile;
        private System.Windows.Forms.Label label1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel1;
        private Win.UI.Panel panel10;
        private Win.UI.Grid gridMatch;
        private Win.UI.ListControlBindingSource BindingSourceMatch;
        private Win.UI.Panel panel7;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnConfirm;
        private Win.UI.Panel panel6;
        private System.Windows.Forms.Label label2;
        private Win.UI.Button btnMapping;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}