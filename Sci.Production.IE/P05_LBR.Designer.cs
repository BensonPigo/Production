namespace Sci.Production.IE
{
    partial class P05_LBR
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.numSewerManpower = new Sci.Win.UI.NumericBox();
            this.numLBR = new Sci.Win.UI.NumericBox();
            this.numOPLoading = new Sci.Win.UI.NumericBox();
            this.tabNoOfOperator = new Sci.Win.UI.TabControl();
            this.tabSewerMinus2 = new System.Windows.Forms.TabPage();
            this.splitViewOperator = new System.Windows.Forms.SplitContainer();
            this.gridMain = new Sci.Win.UI.Grid();
            this.gridSub = new Sci.Win.UI.Grid();
            this.tabSewerMinus1 = new System.Windows.Forms.TabPage();
            this.tabSewerManualInput = new System.Windows.Forms.TabPage();
            this.tabSewerPlus1 = new System.Windows.Forms.TabPage();
            this.tabSewerPlus2 = new System.Windows.Forms.TabPage();
            this.gridMainBs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnReset = new Sci.Win.UI.Button();
            this.btnReload = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.tabNoOfOperator.SuspendLayout();
            this.tabSewerMinus2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitViewOperator)).BeginInit();
            this.splitViewOperator.Panel1.SuspendLayout();
            this.splitViewOperator.Panel2.SuspendLayout();
            this.splitViewOperator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMainBs)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "No. of Sewer";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(164, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "LBR(%)";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(286, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(189, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Highest Operator Loading(%)";
            // 
            // numSewerManpower
            // 
            this.numSewerManpower.BackColor = System.Drawing.Color.White;
            this.numSewerManpower.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numSewerManpower.Location = new System.Drawing.Point(103, 9);
            this.numSewerManpower.Name = "numSewerManpower";
            this.numSewerManpower.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSewerManpower.Size = new System.Drawing.Size(48, 23);
            this.numSewerManpower.TabIndex = 4;
            this.numSewerManpower.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numLBR
            // 
            this.numLBR.BackColor = System.Drawing.Color.White;
            this.numLBR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numLBR.Location = new System.Drawing.Point(225, 9);
            this.numLBR.Name = "numLBR";
            this.numLBR.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numLBR.Size = new System.Drawing.Size(48, 23);
            this.numLBR.TabIndex = 5;
            this.numLBR.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numOPLoading
            // 
            this.numOPLoading.BackColor = System.Drawing.Color.White;
            this.numOPLoading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numOPLoading.Location = new System.Drawing.Point(478, 9);
            this.numOPLoading.Name = "numOPLoading";
            this.numOPLoading.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numOPLoading.Size = new System.Drawing.Size(48, 23);
            this.numOPLoading.TabIndex = 6;
            this.numOPLoading.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // tabNoOfOperator
            // 
            this.tabNoOfOperator.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabNoOfOperator.Controls.Add(this.tabSewerMinus2);
            this.tabNoOfOperator.Controls.Add(this.tabSewerMinus1);
            this.tabNoOfOperator.Controls.Add(this.tabSewerManualInput);
            this.tabNoOfOperator.Controls.Add(this.tabSewerPlus1);
            this.tabNoOfOperator.Controls.Add(this.tabSewerPlus2);
            this.tabNoOfOperator.Location = new System.Drawing.Point(9, 38);
            this.tabNoOfOperator.Name = "tabNoOfOperator";
            this.tabNoOfOperator.SelectedIndex = 0;
            this.tabNoOfOperator.Size = new System.Drawing.Size(908, 507);
            this.tabNoOfOperator.TabIndex = 7;
            // 
            // tabSewerMinus2
            // 
            this.tabSewerMinus2.Controls.Add(this.splitViewOperator);
            this.tabSewerMinus2.Location = new System.Drawing.Point(4, 25);
            this.tabSewerMinus2.Name = "tabSewerMinus2";
            this.tabSewerMinus2.Padding = new System.Windows.Forms.Padding(3);
            this.tabSewerMinus2.Size = new System.Drawing.Size(900, 478);
            this.tabSewerMinus2.TabIndex = 0;
            this.tabSewerMinus2.Text = "-2 Sewer";
            // 
            // splitViewOperator
            // 
            this.splitViewOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitViewOperator.Location = new System.Drawing.Point(3, 3);
            this.splitViewOperator.Name = "splitViewOperator";
            // 
            // splitViewOperator.Panel1
            // 
            this.splitViewOperator.Panel1.Controls.Add(this.gridMain);
            // 
            // splitViewOperator.Panel2
            // 
            this.splitViewOperator.Panel2.Controls.Add(this.gridSub);
            this.splitViewOperator.Size = new System.Drawing.Size(894, 472);
            this.splitViewOperator.SplitterDistance = 632;
            this.splitViewOperator.TabIndex = 0;
            // 
            // gridMain
            // 
            this.gridMain.AllowUserToAddRows = false;
            this.gridMain.AllowUserToDeleteRows = false;
            this.gridMain.AllowUserToResizeRows = false;
            this.gridMain.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridMain.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMain.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridMain.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridMain.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridMain.Location = new System.Drawing.Point(0, 0);
            this.gridMain.Name = "gridMain";
            this.gridMain.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridMain.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridMain.RowTemplate.Height = 24;
            this.gridMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMain.ShowCellToolTips = false;
            this.gridMain.Size = new System.Drawing.Size(632, 472);
            this.gridMain.TabIndex = 0;
            // 
            // gridSub
            // 
            this.gridSub.AllowUserToAddRows = false;
            this.gridSub.AllowUserToDeleteRows = false;
            this.gridSub.AllowUserToResizeRows = false;
            this.gridSub.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSub.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSub.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSub.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSub.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSub.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSub.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSub.Location = new System.Drawing.Point(0, 0);
            this.gridSub.Name = "gridSub";
            this.gridSub.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSub.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSub.RowTemplate.Height = 24;
            this.gridSub.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSub.ShowCellToolTips = false;
            this.gridSub.Size = new System.Drawing.Size(258, 472);
            this.gridSub.TabIndex = 1;
            // 
            // tabSewerMinus1
            // 
            this.tabSewerMinus1.Location = new System.Drawing.Point(4, 25);
            this.tabSewerMinus1.Name = "tabSewerMinus1";
            this.tabSewerMinus1.Padding = new System.Windows.Forms.Padding(3);
            this.tabSewerMinus1.Size = new System.Drawing.Size(900, 478);
            this.tabSewerMinus1.TabIndex = 1;
            this.tabSewerMinus1.Text = "-1 Sewer";
            // 
            // tabSewerManualInput
            // 
            this.tabSewerManualInput.Location = new System.Drawing.Point(4, 25);
            this.tabSewerManualInput.Name = "tabSewerManualInput";
            this.tabSewerManualInput.Size = new System.Drawing.Size(900, 478);
            this.tabSewerManualInput.TabIndex = 2;
            this.tabSewerManualInput.Text = "Manual Input";
            // 
            // tabSewerPlus1
            // 
            this.tabSewerPlus1.Location = new System.Drawing.Point(4, 25);
            this.tabSewerPlus1.Name = "tabSewerPlus1";
            this.tabSewerPlus1.Size = new System.Drawing.Size(900, 478);
            this.tabSewerPlus1.TabIndex = 3;
            this.tabSewerPlus1.Text = "+1 Sewer";
            // 
            // tabSewerPlus2
            // 
            this.tabSewerPlus2.Location = new System.Drawing.Point(4, 25);
            this.tabSewerPlus2.Name = "tabSewerPlus2";
            this.tabSewerPlus2.Size = new System.Drawing.Size(900, 478);
            this.tabSewerPlus2.TabIndex = 4;
            this.tabSewerPlus2.Text = "+2 Sewer";
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(652, 552);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(80, 30);
            this.btnReset.TabIndex = 8;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // btnReload
            // 
            this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReload.Location = new System.Drawing.Point(738, 552);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(80, 30);
            this.btnReload.TabIndex = 9;
            this.btnReload.Text = "Reload";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.BtnReload_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(824, 552);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P05_LBR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 592);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.tabNoOfOperator);
            this.Controls.Add(this.numOPLoading);
            this.Controls.Add(this.numLBR);
            this.Controls.Add(this.numSewerManpower);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "P05_LBR";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P05. View No of Operator";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.numSewerManpower, 0);
            this.Controls.SetChildIndex(this.numLBR, 0);
            this.Controls.SetChildIndex(this.numOPLoading, 0);
            this.Controls.SetChildIndex(this.tabNoOfOperator, 0);
            this.Controls.SetChildIndex(this.btnReset, 0);
            this.Controls.SetChildIndex(this.btnReload, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.tabNoOfOperator.ResumeLayout(false);
            this.tabSewerMinus2.ResumeLayout(false);
            this.splitViewOperator.Panel1.ResumeLayout(false);
            this.splitViewOperator.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitViewOperator)).EndInit();
            this.splitViewOperator.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMainBs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.NumericBox numSewerManpower;
        private Win.UI.NumericBox numLBR;
        private Win.UI.NumericBox numOPLoading;
        private Win.UI.TabControl tabNoOfOperator;
        private System.Windows.Forms.TabPage tabSewerMinus2;
        private System.Windows.Forms.SplitContainer splitViewOperator;
        private System.Windows.Forms.TabPage tabSewerMinus1;
        private System.Windows.Forms.TabPage tabSewerManualInput;
        private System.Windows.Forms.TabPage tabSewerPlus1;
        private System.Windows.Forms.TabPage tabSewerPlus2;
        private Win.UI.Grid gridMain;
        private Win.UI.Grid gridSub;
        private Win.UI.ListControlBindingSource gridMainBs;
        private Win.UI.Button btnReset;
        private Win.UI.Button btnReload;
        private Win.UI.Button btnClose;
    }
}