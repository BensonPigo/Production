namespace Sci.Production.PublicForm
{
    partial class TK_StatusHistory
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
            this.gridTransferExport_StatusHistory = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridTransferExport_StatusHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // gridTransferExport_StatusHistory
            // 
            this.gridTransferExport_StatusHistory.AllowUserToAddRows = false;
            this.gridTransferExport_StatusHistory.AllowUserToDeleteRows = false;
            this.gridTransferExport_StatusHistory.AllowUserToResizeRows = false;
            this.gridTransferExport_StatusHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridTransferExport_StatusHistory.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridTransferExport_StatusHistory.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridTransferExport_StatusHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTransferExport_StatusHistory.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridTransferExport_StatusHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridTransferExport_StatusHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridTransferExport_StatusHistory.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridTransferExport_StatusHistory.Location = new System.Drawing.Point(12, 12);
            this.gridTransferExport_StatusHistory.Name = "gridTransferExport_StatusHistory";
            this.gridTransferExport_StatusHistory.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridTransferExport_StatusHistory.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridTransferExport_StatusHistory.RowTemplate.Height = 24;
            this.gridTransferExport_StatusHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTransferExport_StatusHistory.ShowCellToolTips = false;
            this.gridTransferExport_StatusHistory.Size = new System.Drawing.Size(984, 390);
            this.gridTransferExport_StatusHistory.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(916, 408);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // TK_StatusHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 450);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridTransferExport_StatusHistory);
            this.Name = "TK_StatusHistory";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Status History";
            this.Controls.SetChildIndex(this.gridTransferExport_StatusHistory, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridTransferExport_StatusHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridTransferExport_StatusHistory;
        private Win.UI.Button btnClose;
    }
}