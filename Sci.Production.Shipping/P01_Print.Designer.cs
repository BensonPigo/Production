namespace Sci.Production.Shipping
{
    partial class P01_Print
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
            this.labeReportType = new Sci.Win.UI.Label();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioDetail = new Sci.Win.UI.RadioButton();
            this.radioRequest = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(348, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(348, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(348, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(299, 120);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(325, 156);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(325, 183);
            // 
            // labeReportType
            // 
            this.labeReportType.Location = new System.Drawing.Point(25, 13);
            this.labeReportType.Name = "labeReportType";
            this.labeReportType.Size = new System.Drawing.Size(85, 23);
            this.labeReportType.TabIndex = 94;
            this.labeReportType.Text = "Report Type:";
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioDetail);
            this.radioPanel1.Controls.Add(this.radioRequest);
            this.radioPanel1.Location = new System.Drawing.Point(42, 39);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(246, 75);
            this.radioPanel1.TabIndex = 95;
            // 
            // radioDetail
            // 
            this.radioDetail.AutoSize = true;
            this.radioDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail.Location = new System.Drawing.Point(16, 35);
            this.radioDetail.Name = "radioDetail";
            this.radioDetail.Size = new System.Drawing.Size(196, 21);
            this.radioDetail.TabIndex = 1;
            this.radioDetail.TabStop = true;
            this.radioDetail.Text = "Air pre-paid payment detail";
            this.radioDetail.UseVisualStyleBackColor = true;
            // 
            // radioRequest
            // 
            this.radioRequest.AutoSize = true;
            this.radioRequest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioRequest.Location = new System.Drawing.Point(16, 7);
            this.radioRequest.Name = "radioRequest";
            this.radioRequest.Size = new System.Drawing.Size(211, 21);
            this.radioRequest.TabIndex = 0;
            this.radioRequest.TabStop = true;
            this.radioRequest.Text = "Air pre-paid request approval";
            this.radioRequest.UseVisualStyleBackColor = true;
            // 
            // P01_Print
            // 
            this.ClientSize = new System.Drawing.Size(440, 241);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.labeReportType);
            this.IsSupportToPrint = false;
            this.Name = "P01_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "Print";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.P01_Print_FormClosed);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labeReportType, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labeReportType;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioDetail;
        private Win.UI.RadioButton radioRequest;
    }
}
