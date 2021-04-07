namespace Sci.Production.Warehouse
{
    partial class P63
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridSemiFinishedInventory = new Sci.Win.UI.Grid();
            this.label1 = new Sci.Win.UI.Label();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.btnNewSearch = new Sci.Win.UI.Button();
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridSemiFinishedInventory)).BeginInit();
            this.SuspendLayout();
            // 
            // gridSemiFinishedInventory
            // 
            this.gridSemiFinishedInventory.AllowUserToAddRows = false;
            this.gridSemiFinishedInventory.AllowUserToDeleteRows = false;
            this.gridSemiFinishedInventory.AllowUserToResizeRows = false;
            this.gridSemiFinishedInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSemiFinishedInventory.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSemiFinishedInventory.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSemiFinishedInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSemiFinishedInventory.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSemiFinishedInventory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSemiFinishedInventory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSemiFinishedInventory.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSemiFinishedInventory.Location = new System.Drawing.Point(6, 39);
            this.gridSemiFinishedInventory.Name = "gridSemiFinishedInventory";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridSemiFinishedInventory.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridSemiFinishedInventory.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSemiFinishedInventory.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSemiFinishedInventory.RowTemplate.Height = 25;
            this.gridSemiFinishedInventory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSemiFinishedInventory.ShowCellToolTips = false;
            this.gridSemiFinishedInventory.Size = new System.Drawing.Size(847, 429);
            this.gridSemiFinishedInventory.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "SP#";
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(55, 8);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(133, 24);
            this.txtSP.TabIndex = 3;
            // 
            // btnNewSearch
            // 
            this.btnNewSearch.Location = new System.Drawing.Point(194, 5);
            this.btnNewSearch.Name = "btnNewSearch";
            this.btnNewSearch.Size = new System.Drawing.Size(111, 30);
            this.btnNewSearch.TabIndex = 4;
            this.btnNewSearch.Text = "New Search";
            this.btnNewSearch.UseVisualStyleBackColor = true;
            this.btnNewSearch.Click += new System.EventHandler(this.BtnNewSearch_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(311, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(111, 30);
            this.btnQuery.TabIndex = 5;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(742, 474);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(111, 30);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P63
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 511);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.btnNewSearch);
            this.Controls.Add(this.txtSP);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridSemiFinishedInventory);
            this.Name = "P63";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P63. Material Status (Semi-finished)";
            this.Controls.SetChildIndex(this.gridSemiFinishedInventory, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtSP, 0);
            this.Controls.SetChildIndex(this.btnNewSearch, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridSemiFinishedInventory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid gridSemiFinishedInventory;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtSP;
        private Win.UI.Button btnNewSearch;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnClose;
    }
}