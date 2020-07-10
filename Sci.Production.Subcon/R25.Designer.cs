namespace Sci.Production.Subcon
{
    partial class R25
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
            this.txtartworktype_ftyCategory = new Sci.Production.Class.Txtartworktype_fty();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.dateReceiveDate = new Sci.Win.UI.DateRange();
            this.txtsubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelRefno = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelReceiveDate = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(429, 27);
            this.print.TabIndex = 1;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(429, 63);
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(429, 99);
            this.close.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtartworktype_ftyCategory);
            this.panel1.Controls.Add(this.txtRefno);
            this.panel1.Controls.Add(this.txtSPNo);
            this.panel1.Controls.Add(this.comboFactory);
            this.panel1.Controls.Add(this.dateReceiveDate);
            this.panel1.Controls.Add(this.txtsubconSupplier);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelSupplier);
            this.panel1.Controls.Add(this.labelCategory);
            this.panel1.Controls.Add(this.labelRefno);
            this.panel1.Controls.Add(this.labelSPNo);
            this.panel1.Controls.Add(this.labelReceiveDate);
            this.panel1.Location = new System.Drawing.Point(9, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(414, 244);
            this.panel1.TabIndex = 0;
            // 
            // txtartworktype_ftyCategory
            // 
            this.txtartworktype_ftyCategory.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftyCategory.CClassify = "\'p\'";
            this.txtartworktype_ftyCategory.CSubprocess = "";
            this.txtartworktype_ftyCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftyCategory.Location = new System.Drawing.Point(118, 131);
            this.txtartworktype_ftyCategory.Name = "txtartworktype_ftyCategory";
            this.txtartworktype_ftyCategory.Size = new System.Drawing.Size(100, 23);
            this.txtartworktype_ftyCategory.TabIndex = 3;
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(118, 92);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(100, 23);
            this.txtRefno.TabIndex = 2;
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(118, 51);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(100, 23);
            this.txtSPNo.TabIndex = 1;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(118, 208);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 5;
            // 
            // dateReceiveDate
            // 
            this.dateReceiveDate.IsRequired = false;
            this.dateReceiveDate.Location = new System.Drawing.Point(118, 14);
            this.dateReceiveDate.Name = "dateReceiveDate";
            this.dateReceiveDate.Size = new System.Drawing.Size(280, 23);
            this.dateReceiveDate.TabIndex = 0;
            // 
            // txtsubconSupplier
            // 
            this.txtsubconSupplier.DisplayBox1Binding = "";
            this.txtsubconSupplier.IsIncludeJunk = true;
            this.txtsubconSupplier.Location = new System.Drawing.Point(118, 169);
            this.txtsubconSupplier.Name = "txtsubconSupplier";
            this.txtsubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtsubconSupplier.TabIndex = 4;
            this.txtsubconSupplier.TextBox1Binding = "";
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(13, 209);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(102, 23);
            this.labelFactory.TabIndex = 5;
            this.labelFactory.Text = "Factory";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Lines = 0;
            this.labelSupplier.Location = new System.Drawing.Point(13, 169);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(102, 23);
            this.labelSupplier.TabIndex = 4;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelCategory
            // 
            this.labelCategory.Lines = 0;
            this.labelCategory.Location = new System.Drawing.Point(13, 131);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(102, 23);
            this.labelCategory.TabIndex = 3;
            this.labelCategory.Text = "Category";
            // 
            // labelRefno
            // 
            this.labelRefno.Lines = 0;
            this.labelRefno.Location = new System.Drawing.Point(13, 92);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(102, 23);
            this.labelRefno.TabIndex = 2;
            this.labelRefno.Text = "Refno";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(13, 51);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(102, 23);
            this.labelSPNo.TabIndex = 1;
            this.labelSPNo.Text = "SP#";
            // 
            // labelReceiveDate
            // 
            this.labelReceiveDate.Lines = 0;
            this.labelReceiveDate.Location = new System.Drawing.Point(13, 14);
            this.labelReceiveDate.Name = "labelReceiveDate";
            this.labelReceiveDate.Size = new System.Drawing.Size(102, 23);
            this.labelReceiveDate.TabIndex = 0;
            this.labelReceiveDate.Text = "Receive Date";
            // 
            // R25
            // 
            this.ClientSize = new System.Drawing.Size(517, 294);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "dateReceiveDate";
            this.DefaultControlForEdit = "dateReceiveDate";
            this.Name = "R25";
            this.Text = "R25. Local Purchase Receiving List";
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
        private Win.UI.Label labelReceiveDate;
        private Class.TxtsubconNoConfirm txtsubconSupplier;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelRefno;
        private Win.UI.Label labelSPNo;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.DateRange dateReceiveDate;
        private Win.UI.TextBox txtRefno;
        private Class.Txtartworktype_fty txtartworktype_ftyCategory;
    }
}
