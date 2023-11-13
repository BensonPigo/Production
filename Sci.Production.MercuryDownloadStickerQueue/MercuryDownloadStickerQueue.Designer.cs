namespace Sci.Production.MercuryDownloadStickerQueue
{
    partial class MercuryDownloadStickerQueue
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gridDownloadStickerQueue = new Sci.Win.UI.Grid();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.backgroundDownloadSticker = new System.ComponentModel.BackgroundWorker();
            this.editErrorMsg = new Sci.Win.UI.EditBox();
            this.label1 = new Sci.Win.UI.Label();
            this.checkCartonBarcode = new Sci.Win.UI.CheckBox();
            this.btnRun = new Sci.Win.UI.Button();
            this.progressBarProcessing = new System.Windows.Forms.ProgressBar();
            this.label2 = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridDownloadStickerQueue)).BeginInit();
            this.SuspendLayout();
            // 
            // gridDownloadStickerQueue
            // 
            this.gridDownloadStickerQueue.AllowUserToAddRows = false;
            this.gridDownloadStickerQueue.AllowUserToDeleteRows = false;
            this.gridDownloadStickerQueue.AllowUserToResizeRows = false;
            this.gridDownloadStickerQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDownloadStickerQueue.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDownloadStickerQueue.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDownloadStickerQueue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDownloadStickerQueue.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDownloadStickerQueue.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDownloadStickerQueue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDownloadStickerQueue.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDownloadStickerQueue.Location = new System.Drawing.Point(12, 37);
            this.gridDownloadStickerQueue.Name = "gridDownloadStickerQueue";
            this.gridDownloadStickerQueue.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDownloadStickerQueue.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDownloadStickerQueue.RowTemplate.Height = 24;
            this.gridDownloadStickerQueue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDownloadStickerQueue.ShowCellToolTips = false;
            this.gridDownloadStickerQueue.Size = new System.Drawing.Size(869, 459);
            this.gridDownloadStickerQueue.TabIndex = 1;
            // 
            // timerRefresh
            // 
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // backgroundDownloadSticker
            // 
            this.backgroundDownloadSticker.WorkerReportsProgress = true;
            this.backgroundDownloadSticker.WorkerSupportsCancellation = true;
            this.backgroundDownloadSticker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundDownloadSticker_DoWork);
            this.backgroundDownloadSticker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundDownloadSticker_ProgressChanged);
            // 
            // editErrorMsg
            // 
            this.editErrorMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editErrorMsg.BackColor = System.Drawing.Color.White;
            this.editErrorMsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editErrorMsg.Location = new System.Drawing.Point(124, 502);
            this.editErrorMsg.Multiline = true;
            this.editErrorMsg.Name = "editErrorMsg";
            this.editErrorMsg.Size = new System.Drawing.Size(757, 91);
            this.editErrorMsg.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(12, 502);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Error Message";
            // 
            // checkCartonBarcode
            // 
            this.checkCartonBarcode.AutoSize = true;
            this.checkCartonBarcode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkCartonBarcode.Location = new System.Drawing.Point(12, 10);
            this.checkCartonBarcode.Name = "checkCartonBarcode";
            this.checkCartonBarcode.Size = new System.Drawing.Size(166, 21);
            this.checkCartonBarcode.TabIndex = 4;
            this.checkCartonBarcode.Text = "Carton barcode check";
            this.checkCartonBarcode.UseVisualStyleBackColor = true;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRun.Location = new System.Drawing.Point(13, 543);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(80, 30);
            this.btnRun.TabIndex = 5;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // progressBarProcessing
            // 
            this.progressBarProcessing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarProcessing.Location = new System.Drawing.Point(690, 12);
            this.progressBarProcessing.Name = "progressBarProcessing";
            this.progressBarProcessing.Size = new System.Drawing.Size(191, 23);
            this.progressBarProcessing.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(554, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "Processing progress";
            // 
            // MercuryDownloadStickerQueue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 596);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressBarProcessing);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.checkCartonBarcode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.editErrorMsg);
            this.Controls.Add(this.gridDownloadStickerQueue);
            this.Name = "MercuryDownloadStickerQueue";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Mercury Download Sticker Queue";
            this.Controls.SetChildIndex(this.gridDownloadStickerQueue, 0);
            this.Controls.SetChildIndex(this.editErrorMsg, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.checkCartonBarcode, 0);
            this.Controls.SetChildIndex(this.btnRun, 0);
            this.Controls.SetChildIndex(this.progressBarProcessing, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridDownloadStickerQueue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid gridDownloadStickerQueue;
        private System.Windows.Forms.Timer timerRefresh;
        private System.ComponentModel.BackgroundWorker backgroundDownloadSticker;
        private Win.UI.EditBox editErrorMsg;
        private Win.UI.Label label1;
        private Win.UI.CheckBox checkCartonBarcode;
        private Win.UI.Button btnRun;
        private System.Windows.Forms.ProgressBar progressBarProcessing;
        private Win.UI.Label label2;
    }
}

