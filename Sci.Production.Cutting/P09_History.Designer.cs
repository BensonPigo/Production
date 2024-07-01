namespace Sci.Production.Cutting
{
    partial class P09_History
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
            this.originalCutRefbs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.currentCutRefbs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.removeListbs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.labelSPNo = new Sci.Win.UI.Label();
            this.cmbOriginalCutRef = new Sci.Win.UI.ComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.gridOriginalCutRef = new Sci.Win.UI.Grid();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.gridCurrentCutRef = new Sci.Win.UI.Grid();
            this.label4 = new System.Windows.Forms.Label();
            this.gridRemoveList = new Sci.Win.UI.Grid();
            this.cmbCurrentCutRef = new Sci.Win.UI.ComboBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.displayBox2 = new Sci.Win.UI.DisplayBox();
            this.displayBox3 = new Sci.Win.UI.DisplayBox();
            this.btnClean = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.originalCutRefbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentCutRefbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.removeListbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridOriginalCutRef)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCurrentCutRef)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRemoveList)).BeginInit();
            this.SuspendLayout();
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(9, 9);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(104, 23);
            this.labelSPNo.TabIndex = 3;
            this.labelSPNo.Text = "Original CutRef";
            // 
            // cmbOriCutRef
            // 
            this.cmbOriginalCutRef.BackColor = System.Drawing.Color.White;
            this.cmbOriginalCutRef.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.cmbOriginalCutRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbOriginalCutRef.FormattingEnabled = true;
            this.cmbOriginalCutRef.IsSupportUnselect = true;
            this.cmbOriginalCutRef.Location = new System.Drawing.Point(116, 8);
            this.cmbOriginalCutRef.Name = "cmbOriCutRef";
            this.cmbOriginalCutRef.OldText = "";
            this.cmbOriginalCutRef.Size = new System.Drawing.Size(121, 24);
            this.cmbOriginalCutRef.TabIndex = 4;
            this.cmbOriginalCutRef.SelectedIndexChanged += new System.EventHandler(this.CmbOriCutRef_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(9, 38);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.gridOriginalCutRef);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(882, 512);
            this.splitContainer1.SplitterDistance = 350;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Original CutRef List";
            // 
            // gridOriCutRef
            // 
            this.gridOriginalCutRef.AllowUserToAddRows = false;
            this.gridOriginalCutRef.AllowUserToDeleteRows = false;
            this.gridOriginalCutRef.AllowUserToResizeRows = false;
            this.gridOriginalCutRef.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridOriginalCutRef.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridOriginalCutRef.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridOriginalCutRef.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridOriginalCutRef.DataSource = this.originalCutRefbs;
            this.gridOriginalCutRef.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridOriginalCutRef.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridOriginalCutRef.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridOriginalCutRef.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridOriginalCutRef.Location = new System.Drawing.Point(0, 23);
            this.gridOriginalCutRef.Name = "gridOriCutRef";
            this.gridOriginalCutRef.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridOriginalCutRef.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridOriginalCutRef.RowTemplate.Height = 24;
            this.gridOriginalCutRef.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridOriginalCutRef.ShowCellToolTips = false;
            this.gridOriginalCutRef.Size = new System.Drawing.Size(350, 489);
            this.gridOriginalCutRef.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.gridCurrentCutRef);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.label4);
            this.splitContainer2.Panel2.Controls.Add(this.gridRemoveList);
            this.splitContainer2.Size = new System.Drawing.Size(526, 512);
            this.splitContainer2.SplitterDistance = 252;
            this.splitContainer2.SplitterWidth = 6;
            this.splitContainer2.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Current CutRef List";
            // 
            // gridCurrentCutRef
            // 
            this.gridCurrentCutRef.AllowUserToAddRows = false;
            this.gridCurrentCutRef.AllowUserToDeleteRows = false;
            this.gridCurrentCutRef.AllowUserToResizeRows = false;
            this.gridCurrentCutRef.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCurrentCutRef.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCurrentCutRef.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCurrentCutRef.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCurrentCutRef.DataSource = this.currentCutRefbs;
            this.gridCurrentCutRef.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCurrentCutRef.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCurrentCutRef.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCurrentCutRef.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCurrentCutRef.Location = new System.Drawing.Point(0, 23);
            this.gridCurrentCutRef.Name = "gridCurrentCutRef";
            this.gridCurrentCutRef.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCurrentCutRef.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCurrentCutRef.RowTemplate.Height = 24;
            this.gridCurrentCutRef.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCurrentCutRef.ShowCellToolTips = false;
            this.gridCurrentCutRef.Size = new System.Drawing.Size(526, 229);
            this.gridCurrentCutRef.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Remove List";
            // 
            // gridRemoveList
            // 
            this.gridRemoveList.AllowUserToAddRows = false;
            this.gridRemoveList.AllowUserToDeleteRows = false;
            this.gridRemoveList.AllowUserToResizeRows = false;
            this.gridRemoveList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridRemoveList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridRemoveList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridRemoveList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRemoveList.DataSource = this.removeListbs;
            this.gridRemoveList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridRemoveList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridRemoveList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridRemoveList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridRemoveList.Location = new System.Drawing.Point(0, 23);
            this.gridRemoveList.Name = "gridRemoveList";
            this.gridRemoveList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridRemoveList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridRemoveList.RowTemplate.Height = 24;
            this.gridRemoveList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRemoveList.ShowCellToolTips = false;
            this.gridRemoveList.Size = new System.Drawing.Size(526, 231);
            this.gridRemoveList.TabIndex = 2;
            // 
            // cmbCurrentCutRef
            // 
            this.cmbCurrentCutRef.BackColor = System.Drawing.Color.White;
            this.cmbCurrentCutRef.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.cmbCurrentCutRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbCurrentCutRef.FormattingEnabled = true;
            this.cmbCurrentCutRef.IsSupportUnselect = true;
            this.cmbCurrentCutRef.Location = new System.Drawing.Point(347, 7);
            this.cmbCurrentCutRef.Name = "cmbCurrentCutRef";
            this.cmbCurrentCutRef.OldText = "";
            this.cmbCurrentCutRef.Size = new System.Drawing.Size(121, 24);
            this.cmbCurrentCutRef.TabIndex = 7;
            this.cmbCurrentCutRef.SelectedIndexChanged += new System.EventHandler(this.CmbCurrentCutRef_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(240, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Current CutRef";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 553);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Original Layer";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(119, 553);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Current Layer";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(225, 553);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 17);
            this.label7.TabIndex = 10;
            this.label7.Text = "Remove Layer";
            // 
            // displayBox1
            // 
            this.displayBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(9, 573);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(100, 23);
            this.displayBox1.TabIndex = 11;
            // 
            // displayBox2
            // 
            this.displayBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.displayBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox2.Location = new System.Drawing.Point(116, 573);
            this.displayBox2.Name = "displayBox2";
            this.displayBox2.Size = new System.Drawing.Size(100, 23);
            this.displayBox2.TabIndex = 12;
            // 
            // displayBox3
            // 
            this.displayBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.displayBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox3.Location = new System.Drawing.Point(222, 573);
            this.displayBox3.Name = "displayBox3";
            this.displayBox3.Size = new System.Drawing.Size(100, 23);
            this.displayBox3.TabIndex = 13;
            // 
            // btnClean
            // 
            this.btnClean.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClean.Location = new System.Drawing.Point(769, 3);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(122, 30);
            this.btnClean.TabIndex = 14;
            this.btnClean.Text = "Clean Filter";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.BtnClean_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(795, 569);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 30);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P09_History
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 608);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnClean);
            this.Controls.Add(this.displayBox3);
            this.Controls.Add(this.displayBox2);
            this.Controls.Add(this.displayBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbCurrentCutRef);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.cmbOriginalCutRef);
            this.Controls.Add(this.labelSPNo);
            this.Name = "P09_History";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P09.History";
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.cmbOriginalCutRef, 0);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbCurrentCutRef, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.displayBox1, 0);
            this.Controls.SetChildIndex(this.displayBox2, 0);
            this.Controls.SetChildIndex(this.displayBox3, 0);
            this.Controls.SetChildIndex(this.btnClean, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            ((System.ComponentModel.ISupportInitialize)(this.originalCutRefbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentCutRefbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.removeListbs)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridOriginalCutRef)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCurrentCutRef)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRemoveList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.ListControlBindingSource originalCutRefbs;
        private Win.UI.ListControlBindingSource currentCutRefbs;
        private Win.UI.ListControlBindingSource removeListbs;
        private Win.UI.Label labelSPNo;
        private Win.UI.ComboBox cmbOriginalCutRef;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Win.UI.Grid gridOriginalCutRef;
        private Win.UI.Grid gridCurrentCutRef;
        private Win.UI.Grid gridRemoveList;
        private Win.UI.ComboBox cmbCurrentCutRef;
        private Win.UI.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.DisplayBox displayBox2;
        private Win.UI.DisplayBox displayBox3;
        private Win.UI.Button btnClean;
        private Win.UI.Button btnClose;
    }
}