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
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtBundleNo = new Sci.Win.UI.TextBox();
            this.Date = new Sci.Win.UI.DateRange();
            this.labelBundleNo = new Sci.Win.UI.Label();
            this.labelDate = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(444, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(444, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(444, 84);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtfactory);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.txtBundleNo);
            this.panel1.Controls.Add(this.Date);
            this.panel1.Controls.Add(this.labelBundleNo);
            this.panel1.Controls.Add(this.labelDate);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(413, 113);
            this.panel1.TabIndex = 94;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(12, 79);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(99, 23);
            this.labelFactory.TabIndex = 110;
            this.labelFactory.Text = "Factory";
            // 
            // txtBundleNo
            // 
            this.txtBundleNo.BackColor = System.Drawing.Color.White;
            this.txtBundleNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBundleNo.Location = new System.Drawing.Point(123, 43);
            this.txtBundleNo.MaxLength = 20;
            this.txtBundleNo.Name = "txtBundleNo";
            this.txtBundleNo.Size = new System.Drawing.Size(132, 23);
            this.txtBundleNo.TabIndex = 1;
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
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(430, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 22);
            this.label4.TabIndex = 112;
            this.label4.Text = "Paper Size A4";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(123, 79);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 112;
            // 
            // R50
            // 
            this.ClientSize = new System.Drawing.Size(536, 170);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.Name = "R50";
            this.Text = "R50. Production Bundle Transfer";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Label labelFactory;
        private Win.UI.TextBox txtBundleNo;
        private Win.UI.DateRange Date;
        private Win.UI.Label labelBundleNo;
        private Win.UI.Label labelDate;
        private Class.txtfactory txtfactory;
        private Win.UI.Label label4;
    }
}
