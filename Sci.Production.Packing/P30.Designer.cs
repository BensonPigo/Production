namespace Sci.Production.Packing
{
    partial class P30
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
            this.displayNike = new Sci.Win.UI.DisplayBox();
            this.gridDownloadStickerQueue = new Sci.Win.UI.Grid();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridDownloadStickerQueue)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Brand";
            // 
            // displayNike
            // 
            this.displayNike.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayNike.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayNike.Location = new System.Drawing.Point(87, 9);
            this.displayNike.Name = "displayNike";
            this.displayNike.Size = new System.Drawing.Size(100, 23);
            this.displayNike.TabIndex = 2;
            // 
            // gridDownloadStickerQueue
            // 
            this.gridDownloadStickerQueue.AllowUserToAddRows = false;
            this.gridDownloadStickerQueue.AllowUserToDeleteRows = false;
            this.gridDownloadStickerQueue.AllowUserToResizeRows = false;
            this.gridDownloadStickerQueue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDownloadStickerQueue.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDownloadStickerQueue.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDownloadStickerQueue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDownloadStickerQueue.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDownloadStickerQueue.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDownloadStickerQueue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDownloadStickerQueue.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDownloadStickerQueue.Location = new System.Drawing.Point(9, 38);
            this.gridDownloadStickerQueue.Name = "gridDownloadStickerQueue";
            this.gridDownloadStickerQueue.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDownloadStickerQueue.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDownloadStickerQueue.RowTemplate.Height = 24;
            this.gridDownloadStickerQueue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDownloadStickerQueue.ShowCellToolTips = false;
            this.gridDownloadStickerQueue.Size = new System.Drawing.Size(787, 400);
            this.gridDownloadStickerQueue.TabIndex = 3;
            // 
            // timerRefresh
            // 
            this.timerRefresh.Tick += new System.EventHandler(this.TimerRefresh_Tick);
            // 
            // P30
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 450);
            this.Controls.Add(this.gridDownloadStickerQueue);
            this.Controls.Add(this.displayNike);
            this.Controls.Add(this.label1);
            this.Name = "P30";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P30. Download Sticker Queue";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.displayNike, 0);
            this.Controls.SetChildIndex(this.gridDownloadStickerQueue, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridDownloadStickerQueue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.DisplayBox displayNike;
        private Win.UI.Grid gridDownloadStickerQueue;
        private System.Windows.Forms.Timer timerRefresh;
    }
}