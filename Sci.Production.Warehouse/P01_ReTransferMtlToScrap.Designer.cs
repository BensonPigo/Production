namespace Sci.Production.Warehouse
{
    partial class P01_ReTransferMtlToScrap
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblScrapHistory = new Sci.Win.UI.Label();
            this.lblBulk = new Sci.Win.UI.Label();
            this.gridScrapHistory = new Sci.Win.UI.Grid();
            this.gridBulk = new Sci.Win.UI.Grid();
            this.btnRetransferToScrap = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.lblFtyInventoryLockHint = new Sci.Win.UI.Label();
            this.labelNoQuoteHintColor = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridScrapHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBulk)).BeginInit();
            this.SuspendLayout();
            // 
            // lblScrapHistory
            // 
            this.lblScrapHistory.Location = new System.Drawing.Point(22, 9);
            this.lblScrapHistory.Name = "lblScrapHistory";
            this.lblScrapHistory.Size = new System.Drawing.Size(90, 23);
            this.lblScrapHistory.TabIndex = 1;
            this.lblScrapHistory.Text = "Scrap History";
            // 
            // lblBulk
            // 
            this.lblBulk.Location = new System.Drawing.Point(22, 230);
            this.lblBulk.Name = "lblBulk";
            this.lblBulk.Size = new System.Drawing.Size(90, 23);
            this.lblBulk.TabIndex = 2;
            this.lblBulk.Text = "Bulk";
            // 
            // gridScrapHistory
            // 
            this.gridScrapHistory.AllowUserToAddRows = false;
            this.gridScrapHistory.AllowUserToDeleteRows = false;
            this.gridScrapHistory.AllowUserToResizeRows = false;
            this.gridScrapHistory.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridScrapHistory.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridScrapHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridScrapHistory.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridScrapHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridScrapHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridScrapHistory.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridScrapHistory.Location = new System.Drawing.Point(22, 35);
            this.gridScrapHistory.Name = "gridScrapHistory";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridScrapHistory.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridScrapHistory.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridScrapHistory.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridScrapHistory.RowTemplate.Height = 24;
            this.gridScrapHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridScrapHistory.ShowCellToolTips = false;
            this.gridScrapHistory.Size = new System.Drawing.Size(824, 183);
            this.gridScrapHistory.TabIndex = 3;
            // 
            // gridBulk
            // 
            this.gridBulk.AllowUserToAddRows = false;
            this.gridBulk.AllowUserToDeleteRows = false;
            this.gridBulk.AllowUserToResizeRows = false;
            this.gridBulk.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBulk.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBulk.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBulk.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBulk.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBulk.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBulk.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBulk.Location = new System.Drawing.Point(22, 256);
            this.gridBulk.Name = "gridBulk";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridBulk.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridBulk.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBulk.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBulk.RowTemplate.Height = 24;
            this.gridBulk.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBulk.ShowCellToolTips = false;
            this.gridBulk.Size = new System.Drawing.Size(824, 183);
            this.gridBulk.TabIndex = 4;
            this.gridBulk.Sorted += new System.EventHandler(this.GridBulk_Sorted);
            // 
            // btnRetransferToScrap
            // 
            this.btnRetransferToScrap.Location = new System.Drawing.Point(582, 445);
            this.btnRetransferToScrap.Name = "btnRetransferToScrap";
            this.btnRetransferToScrap.Size = new System.Drawing.Size(185, 30);
            this.btnRetransferToScrap.TabIndex = 5;
            this.btnRetransferToScrap.Text = "Re-Transfer to Scrap";
            this.btnRetransferToScrap.UseVisualStyleBackColor = true;
            this.btnRetransferToScrap.Click += new System.EventHandler(this.BtnRetransferToScrap_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(773, 445);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(73, 30);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // lblFtyInventoryLockHint
            // 
            this.lblFtyInventoryLockHint.BackColor = System.Drawing.Color.Transparent;
            this.lblFtyInventoryLockHint.Location = new System.Drawing.Point(41, 449);
            this.lblFtyInventoryLockHint.Name = "lblFtyInventoryLockHint";
            this.lblFtyInventoryLockHint.Size = new System.Drawing.Size(259, 23);
            this.lblFtyInventoryLockHint.TabIndex = 42;
            this.lblFtyInventoryLockHint.Text = "Still lock material. can not do re-transfer.";
            this.lblFtyInventoryLockHint.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelNoQuoteHintColor
            // 
            this.labelNoQuoteHintColor.BackColor = System.Drawing.Color.Silver;
            this.labelNoQuoteHintColor.Location = new System.Drawing.Point(22, 450);
            this.labelNoQuoteHintColor.Name = "labelNoQuoteHintColor";
            this.labelNoQuoteHintColor.Size = new System.Drawing.Size(19, 20);
            this.labelNoQuoteHintColor.TabIndex = 43;
            // 
            // P01_ReTransferMtlToScrap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 484);
            this.Controls.Add(this.labelNoQuoteHintColor);
            this.Controls.Add(this.lblFtyInventoryLockHint);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRetransferToScrap);
            this.Controls.Add(this.gridBulk);
            this.Controls.Add(this.gridScrapHistory);
            this.Controls.Add(this.lblBulk);
            this.Controls.Add(this.lblScrapHistory);
            this.Name = "P01_ReTransferMtlToScrap";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P01_ReTransferMtlToScrap";
            this.Controls.SetChildIndex(this.lblScrapHistory, 0);
            this.Controls.SetChildIndex(this.lblBulk, 0);
            this.Controls.SetChildIndex(this.gridScrapHistory, 0);
            this.Controls.SetChildIndex(this.gridBulk, 0);
            this.Controls.SetChildIndex(this.btnRetransferToScrap, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.lblFtyInventoryLockHint, 0);
            this.Controls.SetChildIndex(this.labelNoQuoteHintColor, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridScrapHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBulk)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label lblScrapHistory;
        private Win.UI.Label lblBulk;
        private Win.UI.Grid gridScrapHistory;
        private Win.UI.Grid gridBulk;
        private Win.UI.Button btnRetransferToScrap;
        private Win.UI.Button btnClose;
        private Win.UI.Label lblFtyInventoryLockHint;
        private Win.UI.Label labelNoQuoteHintColor;
    }
}