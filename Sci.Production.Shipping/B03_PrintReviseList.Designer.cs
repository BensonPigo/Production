namespace Sci.Production.Shipping
{
    partial class B03_PrintReviseList
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
            this.labelReviseDate = new Sci.Win.UI.Label();
            this.dateReviseDate = new Sci.Win.UI.DateRange();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(398, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(398, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(398, 84);
            // 
            // labelReviseDate
            // 
            this.labelReviseDate.Lines = 0;
            this.labelReviseDate.Location = new System.Drawing.Point(13, 48);
            this.labelReviseDate.Name = "labelReviseDate";
            this.labelReviseDate.Size = new System.Drawing.Size(82, 23);
            this.labelReviseDate.TabIndex = 94;
            this.labelReviseDate.Text = "Revise Date";
            // 
            // dateReviseDate
            // 
            this.dateReviseDate.IsRequired = false;
            this.dateReviseDate.Location = new System.Drawing.Point(99, 48);
            this.dateReviseDate.Name = "dateReviseDate";
            this.dateReviseDate.Size = new System.Drawing.Size(280, 23);
            this.dateReviseDate.TabIndex = 95;
            // 
            // B03_PrintReviseList
            // 
            this.ClientSize = new System.Drawing.Size(490, 159);
            this.Controls.Add(this.dateReviseDate);
            this.Controls.Add(this.labelReviseDate);
            this.IsSupportToPrint = false;
            this.Name = "B03_PrintReviseList";
            this.Text = "Print Revise List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelReviseDate, 0);
            this.Controls.SetChildIndex(this.dateReviseDate, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelReviseDate;
        private Win.UI.DateRange dateReviseDate;
    }
}
