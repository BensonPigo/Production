namespace Sci.Production.Warehouse
{
    partial class P07_PrintAccessorySticker
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
            this.btnPrint = new Sci.Win.UI.Button();
            this.gridSticker = new Sci.Win.UI.Grid();
            this.btnCleanStickerQty = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridSticker)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(543, 575);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(80, 30);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // gridSticker
            // 
            this.gridSticker.AllowUserToAddRows = false;
            this.gridSticker.AllowUserToDeleteRows = false;
            this.gridSticker.AllowUserToResizeRows = false;
            this.gridSticker.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSticker.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSticker.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSticker.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSticker.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSticker.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSticker.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSticker.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSticker.Location = new System.Drawing.Point(12, 12);
            this.gridSticker.Name = "gridSticker";
            this.gridSticker.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSticker.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSticker.RowTemplate.Height = 24;
            this.gridSticker.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSticker.ShowCellToolTips = false;
            this.gridSticker.Size = new System.Drawing.Size(611, 557);
            this.gridSticker.TabIndex = 2;
            this.gridSticker.TabStop = false;
            // 
            // btnCleanStickerQty
            // 
            this.btnCleanStickerQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCleanStickerQty.Location = new System.Drawing.Point(398, 575);
            this.btnCleanStickerQty.Name = "btnCleanStickerQty";
            this.btnCleanStickerQty.Size = new System.Drawing.Size(139, 30);
            this.btnCleanStickerQty.TabIndex = 3;
            this.btnCleanStickerQty.Text = "Clean Sticker Qty";
            this.btnCleanStickerQty.UseVisualStyleBackColor = true;
            this.btnCleanStickerQty.Click += new System.EventHandler(this.BtnCleanStickerQty_Click);
            // 
            // P07_PrintAccessorySticker
            // 
            this.ClientSize = new System.Drawing.Size(635, 611);
            this.Controls.Add(this.btnCleanStickerQty);
            this.Controls.Add(this.gridSticker);
            this.Controls.Add(this.btnPrint);
            this.Name = "P07_PrintAccessorySticker";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Print Accessory Sticker";
            this.Controls.SetChildIndex(this.btnPrint, 0);
            this.Controls.SetChildIndex(this.gridSticker, 0);
            this.Controls.SetChildIndex(this.btnCleanStickerQty, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridSticker)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnPrint;
        private Win.UI.Grid gridSticker;
        private Win.UI.Button btnCleanStickerQty;
    }
}
