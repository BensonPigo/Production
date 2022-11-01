namespace Sci.Production.Shipping
{
    partial class R64
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
            this.dateETA = new Sci.Win.UI.DateRange();
            this.dateArrivedWHDate = new Sci.Win.UI.DateRange();
            this.labArrivWHDate = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(498, 80);
            this.print.TabIndex = 8;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(498, 9);
            this.toexcel.TabIndex = 6;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(498, 45);
            this.close.TabIndex = 7;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(452, 114);
            this.buttonCustomized.TabIndex = 13;
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(457, 123);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(457, 123);
            this.txtVersion.TabIndex = 14;
            // 
            // dateETA
            // 
            // 
            // 
            // 
            this.dateETA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateETA.DateBox1.Name = "";
            this.dateETA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateETA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateETA.DateBox2.Name = "";
            this.dateETA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox2.TabIndex = 1;
            this.dateETA.Location = new System.Drawing.Point(133, 17);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(280, 23);
            this.dateETA.TabIndex = 0;
            // 
            // dateArrivedWHDate
            // 
            // 
            // 
            // 
            this.dateArrivedWHDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateArrivedWHDate.DateBox1.Name = "";
            this.dateArrivedWHDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateArrivedWHDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateArrivedWHDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateArrivedWHDate.DateBox2.Name = "";
            this.dateArrivedWHDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateArrivedWHDate.DateBox2.TabIndex = 1;
            this.dateArrivedWHDate.Location = new System.Drawing.Point(133, 46);
            this.dateArrivedWHDate.Name = "dateArrivedWHDate";
            this.dateArrivedWHDate.Size = new System.Drawing.Size(280, 23);
            this.dateArrivedWHDate.TabIndex = 98;
            // 
            // labArrivWHDate
            // 
            this.labArrivWHDate.Location = new System.Drawing.Point(6, 46);
            this.labArrivWHDate.Name = "labArrivWHDate";
            this.labArrivWHDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labArrivWHDate.RectStyle.BorderWidth = 1F;
            this.labArrivWHDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labArrivWHDate.RectStyle.ExtBorderWidth = 1F;
            this.labArrivWHDate.Size = new System.Drawing.Size(124, 23);
            this.labArrivWHDate.TabIndex = 102;
            this.labArrivWHDate.Text = "Arrived WH Date";
            this.labArrivWHDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labArrivWHDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 17);
            this.label2.Name = "label2";
            this.label2.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.RectStyle.BorderWidth = 1F;
            this.label2.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label2.RectStyle.ExtBorderWidth = 1F;
            this.label2.Size = new System.Drawing.Size(124, 23);
            this.label2.TabIndex = 101;
            this.label2.Text = "ETA";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // R64
            // 
            this.ClientSize = new System.Drawing.Size(590, 173);
            this.Controls.Add(this.labArrivWHDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateArrivedWHDate);
            this.Controls.Add(this.dateETA);
            this.Name = "R64";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R64. KH CMT Invoice Report";
            this.Controls.SetChildIndex(this.dateETA, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.dateArrivedWHDate, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.labArrivWHDate, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.DateRange dateETA;
        private Win.UI.DateRange dateArrivedWHDate;
        private Win.UI.Label labArrivWHDate;
        private Win.UI.Label label2;
    }
}
