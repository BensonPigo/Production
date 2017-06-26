namespace Sci.Production.Subcon
{
    partial class R50
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtToBundleNo = new Sci.Win.UI.TextBox();
            this.txtFromBundleNo = new Sci.Win.UI.TextBox();
            this.Date = new Sci.Win.UI.DateRange();
            this.labelBundleNo = new Sci.Win.UI.Label();
            this.labelDate = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(700, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(444, 12);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(444, 48);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtToBundleNo);
            this.panel1.Controls.Add(this.txtFromBundleNo);
            this.panel1.Controls.Add(this.Date);
            this.panel1.Controls.Add(this.labelBundleNo);
            this.panel1.Controls.Add(this.labelDate);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(413, 113);
            this.panel1.TabIndex = 94;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(254, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 17);
            this.label6.TabIndex = 107;
            this.label6.Text = "～";
            // 
            // txtToBundleNo
            // 
            this.txtToBundleNo.BackColor = System.Drawing.Color.White;
            this.txtToBundleNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtToBundleNo.Location = new System.Drawing.Point(278, 43);
            this.txtToBundleNo.MaxLength = 20;
            this.txtToBundleNo.Name = "txtToBundleNo";
            this.txtToBundleNo.Size = new System.Drawing.Size(125, 23);
            this.txtToBundleNo.TabIndex = 99;
            // 
            // txtFromBundleNo
            // 
            this.txtFromBundleNo.BackColor = System.Drawing.Color.White;
            this.txtFromBundleNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFromBundleNo.Location = new System.Drawing.Point(123, 43);
            this.txtFromBundleNo.MaxLength = 20;
            this.txtFromBundleNo.Name = "txtFromBundleNo";
            this.txtFromBundleNo.Size = new System.Drawing.Size(125, 23);
            this.txtFromBundleNo.TabIndex = 1;
            // 
            // Date
            // 
            this.Date.IsRequired = false;
            this.Date.Location = new System.Drawing.Point(123, 7);
            this.Date.Name = "Date";
            this.Date.Size = new System.Drawing.Size(280, 23);
            this.Date.TabIndex = 0;
            // 
            // labelBundleNo
            // 
            this.labelBundleNo.Location = new System.Drawing.Point(12, 43);
            this.labelBundleNo.Name = "labelBundleNo";
            this.labelBundleNo.Size = new System.Drawing.Size(99, 23);
            this.labelBundleNo.TabIndex = 98;
            this.labelBundleNo.Text = "Bundle No";
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(12, 7);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(99, 23);
            this.labelDate.TabIndex = 97;
            this.labelDate.Text = "Date";
            // 
            // R50
            // 
            this.ClientSize = new System.Drawing.Size(536, 170);
            this.Controls.Add(this.panel1);
            this.Name = "R50";
            this.Text = "R50. Transfer Bundle Data To Printing System";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.TextBox txtFromBundleNo;
        private Win.UI.DateRange Date;
        private Win.UI.Label labelBundleNo;
        private Win.UI.Label labelDate;
        private Win.UI.TextBox txtToBundleNo;
        private System.Windows.Forms.Label label6;
    }
}
