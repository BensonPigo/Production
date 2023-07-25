namespace Sci.Production.IE
{
    partial class P06_Default
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
            this.splitViewOperator = new System.Windows.Forms.SplitContainer();
            this.gridMain = new Sci.Win.UI.Grid();
            this.gridSub = new Sci.Win.UI.Grid();
            this.numHighestGSD = new Sci.Win.UI.NumericBox();
            this.numLBR = new Sci.Win.UI.NumericBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.gridMainBs = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitViewOperator)).BeginInit();
            this.splitViewOperator.Panel1.SuspendLayout();
            this.splitViewOperator.Panel2.SuspendLayout();
            this.splitViewOperator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMainBs)).BeginInit();
            this.SuspendLayout();
            // 
            // splitViewOperator
            // 
            this.splitViewOperator.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitViewOperator.Location = new System.Drawing.Point(12, 37);
            this.splitViewOperator.Name = "splitViewOperator";
            // 
            // splitViewOperator.Panel1
            // 
            this.splitViewOperator.Panel1.Controls.Add(this.gridMain);
            // 
            // splitViewOperator.Panel2
            // 
            this.splitViewOperator.Panel2.Controls.Add(this.gridSub);
            this.splitViewOperator.Size = new System.Drawing.Size(876, 455);
            this.splitViewOperator.SplitterDistance = 619;
            this.splitViewOperator.TabIndex = 1;
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
            this.gridMain.Size = new System.Drawing.Size(619, 455);
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
            this.gridSub.Size = new System.Drawing.Size(253, 455);
            this.gridSub.TabIndex = 1;
            // 
            // numHighestGSD
            // 
            this.numHighestGSD.BackColor = System.Drawing.Color.White;
            this.numHighestGSD.DecimalPlaces = 2;
            this.numHighestGSD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numHighestGSD.Location = new System.Drawing.Point(344, 9);
            this.numHighestGSD.Name = "numHighestGSD";
            this.numHighestGSD.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numHighestGSD.Size = new System.Drawing.Size(48, 23);
            this.numHighestGSD.TabIndex = 10;
            this.numHighestGSD.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numLBR
            // 
            this.numLBR.BackColor = System.Drawing.Color.White;
            this.numLBR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numLBR.Location = new System.Drawing.Point(110, 9);
            this.numLBR.Name = "numLBR";
            this.numLBR.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numLBR.Size = new System.Drawing.Size(48, 23);
            this.numLBR.TabIndex = 9;
            this.numLBR.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(183, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "Highest Cycle time (Auto)";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "LBR(Auto)(%)";
            // 
            // P06_Default
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 504);
            this.Controls.Add(this.numHighestGSD);
            this.Controls.Add(this.numLBR);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.splitViewOperator);
            this.Name = "P06_Default";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P06. Line Mapping & Balancing (Default)";
            this.Controls.SetChildIndex(this.splitViewOperator, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.numLBR, 0);
            this.Controls.SetChildIndex(this.numHighestGSD, 0);
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

        private System.Windows.Forms.SplitContainer splitViewOperator;
        private Win.UI.Grid gridMain;
        private Win.UI.Grid gridSub;
        private Win.UI.NumericBox numHighestGSD;
        private Win.UI.NumericBox numLBR;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.ListControlBindingSource gridMainBs;
    }
}