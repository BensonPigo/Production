namespace Sci.Production.Shipping
{
    partial class P02_Print
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
            this.radioDetailPackingList = new Sci.Win.UI.RadioButton();
            this.radioPackingList = new Sci.Win.UI.RadioButton();
            this.radioDetailList = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(320, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(320, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(320, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(271, 120);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(297, 156);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(297, 183);
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
            this.radioPanel1.Controls.Add(this.radioDetailPackingList);
            this.radioPanel1.Controls.Add(this.radioPackingList);
            this.radioPanel1.Controls.Add(this.radioDetailList);
            this.radioPanel1.Location = new System.Drawing.Point(42, 39);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(200, 147);
            this.radioPanel1.TabIndex = 95;
            // 
            // radioDetailPackingList
            // 
            this.radioDetailPackingList.AutoSize = true;
            this.radioDetailPackingList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetailPackingList.Location = new System.Drawing.Point(16, 63);
            this.radioDetailPackingList.Name = "radioDetailPackingList";
            this.radioDetailPackingList.Size = new System.Drawing.Size(162, 21);
            this.radioDetailPackingList.TabIndex = 2;
            this.radioDetailPackingList.TabStop = true;
            this.radioDetailPackingList.Text = "Invoice + Packing List";
            this.radioDetailPackingList.UseVisualStyleBackColor = true;
            // 
            // radioPackingList
            // 
            this.radioPackingList.AutoSize = true;
            this.radioPackingList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioPackingList.Location = new System.Drawing.Point(16, 35);
            this.radioPackingList.Name = "radioPackingList";
            this.radioPackingList.Size = new System.Drawing.Size(102, 21);
            this.radioPackingList.TabIndex = 1;
            this.radioPackingList.TabStop = true;
            this.radioPackingList.Text = "Packing List";
            this.radioPackingList.UseVisualStyleBackColor = true;
            // 
            // radioDetailList
            // 
            this.radioDetailList.AutoSize = true;
            this.radioDetailList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetailList.Location = new System.Drawing.Point(16, 7);
            this.radioDetailList.Name = "radioDetailList";
            this.radioDetailList.Size = new System.Drawing.Size(88, 21);
            this.radioDetailList.TabIndex = 0;
            this.radioDetailList.TabStop = true;
            this.radioDetailList.Text = "Detail List";
            this.radioDetailList.UseVisualStyleBackColor = true;
            // 
            // P02_Print
            // 
            this.ClientSize = new System.Drawing.Size(412, 238);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.labeReportType);
            this.IsSupportToPrint = false;
            this.Name = "P02_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "Print";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.P02_Print_FormClosed);
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
        private Win.UI.RadioButton radioDetailPackingList;
        private Win.UI.RadioButton radioPackingList;
        private Win.UI.RadioButton radioDetailList;
    }
}
