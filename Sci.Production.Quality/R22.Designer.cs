namespace Sci.Production.Quality
{
    partial class R22
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
            this.radioPerDateBrand = new Sci.Win.UI.RadioButton();
            this.radioPerDateFactory = new Sci.Win.UI.RadioButton();
            this.radioPerBrand = new Sci.Win.UI.RadioButton();
            this.dateAuditDate = new Sci.Win.UI.DateRange();
            this.labelFormat = new Sci.Win.UI.Label();
            this.labelAuditDate = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(442, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(442, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(442, 84);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioPerDateBrand);
            this.panel1.Controls.Add(this.radioPerDateFactory);
            this.panel1.Controls.Add(this.radioPerBrand);
            this.panel1.Controls.Add(this.dateAuditDate);
            this.panel1.Controls.Add(this.labelFormat);
            this.panel1.Controls.Add(this.labelAuditDate);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 148);
            this.panel1.TabIndex = 94;
            // 
            // radioPerDateBrand
            // 
            this.radioPerDateBrand.AutoSize = true;
            this.radioPerDateBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioPerDateBrand.Location = new System.Drawing.Point(95, 111);
            this.radioPerDateBrand.Name = "radioPerDateBrand";
            this.radioPerDateBrand.Size = new System.Drawing.Size(134, 21);
            this.radioPerDateBrand.TabIndex = 5;
            this.radioPerDateBrand.Text = "Per Date (Brand)";
            this.radioPerDateBrand.UseVisualStyleBackColor = true;
            this.radioPerDateBrand.CheckedChanged += new System.EventHandler(this.RadioPerDateBrand_CheckedChanged);
            // 
            // radioPerDateFactory
            // 
            this.radioPerDateFactory.AutoSize = true;
            this.radioPerDateFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioPerDateFactory.Location = new System.Drawing.Point(95, 83);
            this.radioPerDateFactory.Name = "radioPerDateFactory";
            this.radioPerDateFactory.Size = new System.Drawing.Size(143, 21);
            this.radioPerDateFactory.TabIndex = 4;
            this.radioPerDateFactory.Text = "Per Date (Factory)";
            this.radioPerDateFactory.UseVisualStyleBackColor = true;
            this.radioPerDateFactory.CheckedChanged += new System.EventHandler(this.RadioPerDateFactory_CheckedChanged);
            // 
            // radioPerBrand
            // 
            this.radioPerBrand.AutoSize = true;
            this.radioPerBrand.Checked = true;
            this.radioPerBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioPerBrand.Location = new System.Drawing.Point(95, 55);
            this.radioPerBrand.Name = "radioPerBrand";
            this.radioPerBrand.Size = new System.Drawing.Size(90, 21);
            this.radioPerBrand.TabIndex = 3;
            this.radioPerBrand.TabStop = true;
            this.radioPerBrand.Text = "Per Brand";
            this.radioPerBrand.UseVisualStyleBackColor = true;
            this.radioPerBrand.CheckedChanged += new System.EventHandler(this.RadioPerBrand_CheckedChanged);
            // 
            // dateAuditDate
            // 
            this.dateAuditDate.IsRequired = false;
            this.dateAuditDate.Location = new System.Drawing.Point(95, 17);
            this.dateAuditDate.Name = "dateAuditDate";
            this.dateAuditDate.Size = new System.Drawing.Size(280, 23);
            this.dateAuditDate.TabIndex = 2;
            // 
            // labelFormat
            // 
            this.labelFormat.Lines = 0;
            this.labelFormat.Location = new System.Drawing.Point(17, 59);
            this.labelFormat.Name = "labelFormat";
            this.labelFormat.Size = new System.Drawing.Size(75, 23);
            this.labelFormat.TabIndex = 1;
            this.labelFormat.Text = "Format";
            // 
            // labelAuditDate
            // 
            this.labelAuditDate.Lines = 0;
            this.labelAuditDate.Location = new System.Drawing.Point(17, 17);
            this.labelAuditDate.Name = "labelAuditDate";
            this.labelAuditDate.Size = new System.Drawing.Size(75, 23);
            this.labelAuditDate.TabIndex = 0;
            this.labelAuditDate.Text = "Audit Date";
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(425, 138);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 22);
            this.label9.TabIndex = 98;
            this.label9.Text = "Paper Size A4";
            // 
            // R22
            // 
            this.ClientSize = new System.Drawing.Size(534, 198);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel1);
            this.Name = "R22";
            this.Text = "R22.Pass rate report";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.RadioButton radioPerDateBrand;
        private Win.UI.RadioButton radioPerDateFactory;
        private Win.UI.RadioButton radioPerBrand;
        private Win.UI.DateRange dateAuditDate;
        private Win.UI.Label labelFormat;
        private Win.UI.Label labelAuditDate;
        private Win.UI.Label label9;
    }
}
