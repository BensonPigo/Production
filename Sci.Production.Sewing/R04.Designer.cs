namespace Sci.Production.Sewing
{
    partial class R04
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
            this.labelOoutputDate = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelCDCode = new Sci.Win.UI.Label();
            this.dateOoutputDate = new Sci.Win.UI.DateRange();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.txtbrand = new Sci.Production.Class.txtbrand();
            this.txtCDCode = new Sci.Production.Class.txtcdcode();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(433, 12);
            this.print.TabIndex = 6;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(433, 48);
            this.toexcel.TabIndex = 7;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(433, 84);
            this.close.TabIndex = 8;
            // 
            // labelOoutputDate
            // 
            this.labelOoutputDate.Location = new System.Drawing.Point(23, 12);
            this.labelOoutputDate.Name = "labelOoutputDate";
            this.labelOoutputDate.Size = new System.Drawing.Size(88, 23);
            this.labelOoutputDate.TabIndex = 94;
            this.labelOoutputDate.Text = "Output Date";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(23, 48);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(88, 23);
            this.labelCategory.TabIndex = 95;
            this.labelCategory.Text = "Category";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(23, 84);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(88, 23);
            this.labelM.TabIndex = 96;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(23, 120);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(88, 23);
            this.labelFactory.TabIndex = 97;
            this.labelFactory.Text = "Factory";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(23, 158);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(88, 23);
            this.labelBrand.TabIndex = 98;
            this.labelBrand.Text = "Brand";
            // 
            // labelCDCode
            // 
            this.labelCDCode.Location = new System.Drawing.Point(23, 194);
            this.labelCDCode.Name = "labelCDCode";
            this.labelCDCode.Size = new System.Drawing.Size(88, 23);
            this.labelCDCode.TabIndex = 99;
            this.labelCDCode.Text = "CD Code";
            // 
            // dateOoutputDate
            // 
            this.dateOoutputDate.IsRequired = false;
            this.dateOoutputDate.Location = new System.Drawing.Point(115, 12);
            this.dateOoutputDate.Name = "dateOoutputDate";
            this.dateOoutputDate.Size = new System.Drawing.Size(280, 23);
            this.dateOoutputDate.TabIndex = 0;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(115, 48);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.Size = new System.Drawing.Size(170, 24);
            this.comboCategory.TabIndex = 1;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(115, 84);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(73, 24);
            this.comboM.TabIndex = 2;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(115, 120);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(73, 24);
            this.comboFactory.TabIndex = 3;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(115, 158);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(90, 23);
            this.txtbrand.TabIndex = 4;
            // 
            // txtCDCode
            // 
            this.txtCDCode.BackColor = System.Drawing.Color.White;
            this.txtCDCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCDCode.Location = new System.Drawing.Point(115, 194);
            this.txtCDCode.Name = "txtCDCode";
            this.txtCDCode.Size = new System.Drawing.Size(54, 23);
            this.txtCDCode.TabIndex = 5;
            // 
            // R04
            // 
            this.ClientSize = new System.Drawing.Size(525, 255);
            this.Controls.Add(this.txtCDCode);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.dateOoutputDate);
            this.Controls.Add(this.labelCDCode);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelOoutputDate);
            this.DefaultControl = "dateOoutputDate";
            this.DefaultControlForEdit = "dateOoutputDate";
            this.IsSupportToPrint = false;
            this.Name = "R04";
            this.Text = "R04. Sewing daily output list";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelOoutputDate, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelCDCode, 0);
            this.Controls.SetChildIndex(this.dateOoutputDate, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtCDCode, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelOoutputDate;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelCDCode;
        private Win.UI.DateRange dateOoutputDate;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboFactory;
        private Class.txtbrand txtbrand;
        private Class.txtcdcode txtCDCode;
    }
}
