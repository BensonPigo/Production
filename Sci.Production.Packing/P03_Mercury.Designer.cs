namespace Sci.Production.Packing
{
    partial class P03_Mercury
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
            this.btnUploadPL = new Sci.Win.UI.Button();
            this.btnDownloadStickerFile = new Sci.Win.UI.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gridDownloadStickerQueue = new Sci.Win.UI.Grid();
            this.panelUploadProgress = new Sci.Win.UI.Panel();
            this.lblUploadProgressStatus = new Sci.Win.UI.Label();
            this.progressBarUploadPL = new System.Windows.Forms.ProgressBar();
            this.timerRefreshDownloadStickerQueue = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDownloadStickerQueue)).BeginInit();
            this.panelUploadProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUploadPL
            // 
            this.btnUploadPL.Location = new System.Drawing.Point(12, 12);
            this.btnUploadPL.Name = "btnUploadPL";
            this.btnUploadPL.Size = new System.Drawing.Size(187, 30);
            this.btnUploadPL.TabIndex = 1;
            this.btnUploadPL.Text = "Upload PL";
            this.btnUploadPL.UseVisualStyleBackColor = true;
            this.btnUploadPL.Click += new System.EventHandler(this.BtnUploadPL_Click);
            // 
            // btnDownloadStickerFile
            // 
            this.btnDownloadStickerFile.Location = new System.Drawing.Point(12, 48);
            this.btnDownloadStickerFile.Name = "btnDownloadStickerFile";
            this.btnDownloadStickerFile.Size = new System.Drawing.Size(187, 30);
            this.btnDownloadStickerFile.TabIndex = 2;
            this.btnDownloadStickerFile.Text = "Download Sticker File";
            this.btnDownloadStickerFile.UseVisualStyleBackColor = true;
            this.btnDownloadStickerFile.Click += new System.EventHandler(this.BtnDownloadStickerFile_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gridDownloadStickerQueue);
            this.groupBox1.Location = new System.Drawing.Point(205, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(817, 473);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Download Sticker Queue";
            // 
            // gridDownloadStickerQueue
            // 
            this.gridDownloadStickerQueue.AllowUserToAddRows = false;
            this.gridDownloadStickerQueue.AllowUserToDeleteRows = false;
            this.gridDownloadStickerQueue.AllowUserToResizeRows = false;
            this.gridDownloadStickerQueue.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDownloadStickerQueue.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDownloadStickerQueue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDownloadStickerQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDownloadStickerQueue.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDownloadStickerQueue.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDownloadStickerQueue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDownloadStickerQueue.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDownloadStickerQueue.Location = new System.Drawing.Point(5, 21);
            this.gridDownloadStickerQueue.Name = "gridDownloadStickerQueue";
            this.gridDownloadStickerQueue.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDownloadStickerQueue.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDownloadStickerQueue.RowTemplate.Height = 24;
            this.gridDownloadStickerQueue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDownloadStickerQueue.ShowCellToolTips = false;
            this.gridDownloadStickerQueue.Size = new System.Drawing.Size(807, 447);
            this.gridDownloadStickerQueue.TabIndex = 0;
            // 
            // panelUploadProgress
            // 
            this.panelUploadProgress.Controls.Add(this.lblUploadProgressStatus);
            this.panelUploadProgress.Controls.Add(this.progressBarUploadPL);
            this.panelUploadProgress.Location = new System.Drawing.Point(193, 177);
            this.panelUploadProgress.Name = "panelUploadProgress";
            this.panelUploadProgress.Size = new System.Drawing.Size(390, 90);
            this.panelUploadProgress.TabIndex = 4;
            this.panelUploadProgress.Visible = false;
            // 
            // lblUploadProgressStatus
            // 
            this.lblUploadProgressStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUploadProgressStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblUploadProgressStatus.Location = new System.Drawing.Point(16, 11);
            this.lblUploadProgressStatus.Name = "lblUploadProgressStatus";
            this.lblUploadProgressStatus.Size = new System.Drawing.Size(354, 21);
            this.lblUploadProgressStatus.TabIndex = 1;
            this.lblUploadProgressStatus.Text = "Uploading Carton X of Y";
            this.lblUploadProgressStatus.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // progressBarUploadPL
            // 
            this.progressBarUploadPL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarUploadPL.Location = new System.Drawing.Point(16, 45);
            this.progressBarUploadPL.Name = "progressBarUploadPL";
            this.progressBarUploadPL.Size = new System.Drawing.Size(354, 23);
            this.progressBarUploadPL.TabIndex = 0;
            // 
            // timerRefreshDownloadStickerQueue
            // 
            this.timerRefreshDownloadStickerQueue.Tick += new System.EventHandler(this.TimerRefreshDownloadStickerQueue_Tick);
            // 
            // P03_Mercury
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 497);
            this.Controls.Add(this.panelUploadProgress);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDownloadStickerFile);
            this.Controls.Add(this.btnUploadPL);
            this.Name = "P03_Mercury";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Nike Mercury";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.P03_Mercury_FormClosing);
            this.Resize += new System.EventHandler(this.P03_Mercury_Resize);
            this.Controls.SetChildIndex(this.btnUploadPL, 0);
            this.Controls.SetChildIndex(this.btnDownloadStickerFile, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.panelUploadProgress, 0);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDownloadStickerQueue)).EndInit();
            this.panelUploadProgress.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnUploadPL;
        private Win.UI.Button btnDownloadStickerFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private Win.UI.Grid gridDownloadStickerQueue;
        private Win.UI.Panel panelUploadProgress;
        private Win.UI.Label lblUploadProgressStatus;
        private System.Windows.Forms.ProgressBar progressBarUploadPL;
        private System.Windows.Forms.Timer timerRefreshDownloadStickerQueue;
    }
}