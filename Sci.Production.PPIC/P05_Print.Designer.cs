namespace Sci.Production.PPIC
{
    partial class P05_Print
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
            this.dateRange1 = new Sci.Win.UI.DateRange();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(401, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(401, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(401, 84);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(23, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Ready date";
            // 
            // dateRange1
            // 
            this.dateRange1.IsRequired = false;
            this.dateRange1.IsSupportEditMode = false;
            this.dateRange1.Location = new System.Drawing.Point(102, 48);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 23);
            this.dateRange1.TabIndex = 95;
            // 
            // P05_Print
            // 
            this.ClientSize = new System.Drawing.Size(493, 149);
            this.Controls.Add(this.dateRange1);
            this.Controls.Add(this.label1);
            this.IsSupportToPrint = false;
            this.Name = "P05_Print";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.dateRange1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.DateRange dateRange1;
    }
}
