namespace Sci.Production.PPIC
{
    partial class P07
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
            this.label1 = new Sci.Win.UI.Label();
            this.btnDownload = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.labShowDateRange = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 38);
            this.label1.TabIndex = 1;
            this.label1.Text = "Download Data";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Blue;
            this.label1.TextStyle.Color = System.Drawing.Color.Blue;
            this.label1.TextStyle.ExtBorderColor = System.Drawing.Color.Blue;
            this.label1.TextStyle.GradientColor = System.Drawing.Color.Blue;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(74, 97);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(97, 30);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.BtnDownload_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(177, 97);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(97, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // labShowDateRange
            // 
            this.labShowDateRange.BackColor = System.Drawing.Color.Transparent;
            this.labShowDateRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.labShowDateRange.Location = new System.Drawing.Point(16, 65);
            this.labShowDateRange.Name = "labShowDateRange";
            this.labShowDateRange.Size = new System.Drawing.Size(302, 23);
            this.labShowDateRange.TabIndex = 44;
            this.labShowDateRange.Text = "download...";
            this.labShowDateRange.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // P07
            // 
            this.ClientSize = new System.Drawing.Size(327, 137);
            this.Controls.Add(this.labShowDateRange);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "P07";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P07. Download APS Data";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnDownload, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.labShowDateRange, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Button btnDownload;
        private Win.UI.Button btnClose;
        private Win.UI.Label labShowDateRange;
    }
}
