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
            this.labelReadydate = new Sci.Win.UI.Label();
            this.dateReadydate = new Sci.Win.UI.DateRange();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(401, 12);
            this.print.TabIndex = 1;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(401, 48);
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(401, 84);
            this.close.TabIndex = 3;
            // 
            // labelReadydate
            // 
            this.labelReadydate.Lines = 0;
            this.labelReadydate.Location = new System.Drawing.Point(23, 48);
            this.labelReadydate.Name = "labelReadydate";
            this.labelReadydate.Size = new System.Drawing.Size(75, 23);
            this.labelReadydate.TabIndex = 94;
            this.labelReadydate.Text = "Ready date";
            // 
            // dateReadydate
            // 
            this.dateReadydate.IsRequired = false;
            this.dateReadydate.IsSupportEditMode = false;
            this.dateReadydate.Location = new System.Drawing.Point(102, 48);
            this.dateReadydate.Name = "dateReadydate";
            this.dateReadydate.Size = new System.Drawing.Size(280, 23);
            this.dateReadydate.TabIndex = 0;
            // 
            // P05_Print
            // 
            this.ClientSize = new System.Drawing.Size(493, 149);
            this.Controls.Add(this.dateReadydate);
            this.Controls.Add(this.labelReadydate);
            this.DefaultControl = "dateReadydate";
            this.DefaultControlForEdit = "dateReadydate";
            this.IsSupportToPrint = false;
            this.Name = "P05_Print";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelReadydate, 0);
            this.Controls.SetChildIndex(this.dateReadydate, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelReadydate;
        private Win.UI.DateRange dateReadydate;
    }
}
