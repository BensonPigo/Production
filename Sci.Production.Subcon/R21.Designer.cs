namespace Sci.Production.Subcon
{
    partial class R21
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
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelAPDate = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.dateAPDate = new Sci.Win.UI.DateRange();
            this.labelM = new Sci.Win.UI.Label();
            this.txtMdivisionM = new Sci.Production.Class.TxtMdivision();
            this.txtsubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.txtartworktype_ftyCategory = new Sci.Production.Class.Txtartworktype_fty();
            this.comboOrderBy = new Sci.Win.UI.ComboBox();
            this.labelOrderBy = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(430, 12);
            this.print.TabIndex = 6;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(430, 48);
            this.toexcel.TabIndex = 7;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(430, 84);
            this.close.TabIndex = 8;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(13, 119);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 94;
            this.labelFactory.Text = "Factory";
            // 
            // labelCategory
            // 
            this.labelCategory.Lines = 0;
            this.labelCategory.Location = new System.Drawing.Point(13, 48);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(98, 23);
            this.labelCategory.TabIndex = 95;
            this.labelCategory.Text = "Category";
            // 
            // labelAPDate
            // 
            this.labelAPDate.Lines = 0;
            this.labelAPDate.Location = new System.Drawing.Point(13, 12);
            this.labelAPDate.Name = "labelAPDate";
            this.labelAPDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelAPDate.RectStyle.BorderWidth = 1F;
            this.labelAPDate.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelAPDate.RectStyle.ExtBorderWidth = 1F;
            this.labelAPDate.Size = new System.Drawing.Size(98, 23);
            this.labelAPDate.TabIndex = 96;
            this.labelAPDate.Text = "A/P Date";
            this.labelAPDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelAPDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSupplier
            // 
            this.labelSupplier.Lines = 0;
            this.labelSupplier.Location = new System.Drawing.Point(13, 156);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(98, 23);
            this.labelSupplier.TabIndex = 98;
            this.labelSupplier.Text = "Supplier";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(114, 118);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 3;
            // 
            // dateAPDate
            // 
            this.dateAPDate.IsRequired = false;
            this.dateAPDate.Location = new System.Drawing.Point(115, 12);
            this.dateAPDate.Name = "dateAPDate";
            this.dateAPDate.Size = new System.Drawing.Size(280, 23);
            this.dateAPDate.TabIndex = 0;
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(13, 84);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 103;
            this.labelM.Text = "M";
            // 
            // txtMdivisionM
            // 
            this.txtMdivisionM.BackColor = System.Drawing.Color.White;
            this.txtMdivisionM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivisionM.Location = new System.Drawing.Point(114, 84);
            this.txtMdivisionM.Name = "txtMdivisionM";
            this.txtMdivisionM.Size = new System.Drawing.Size(66, 23);
            this.txtMdivisionM.TabIndex = 2;
            // 
            // txtsubconSupplier
            // 
            this.txtsubconSupplier.DisplayBox1Binding = "";
            this.txtsubconSupplier.IsIncludeJunk = true;
            this.txtsubconSupplier.Location = new System.Drawing.Point(115, 156);
            this.txtsubconSupplier.Name = "txtsubconSupplier";
            this.txtsubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtsubconSupplier.TabIndex = 4;
            this.txtsubconSupplier.TextBox1Binding = "";
            // 
            // txtartworktype_ftyCategory
            // 
            this.txtartworktype_ftyCategory.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftyCategory.CClassify = "\'P\'";
            this.txtartworktype_ftyCategory.CSubprocess = "";
            this.txtartworktype_ftyCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftyCategory.Location = new System.Drawing.Point(114, 48);
            this.txtartworktype_ftyCategory.Name = "txtartworktype_ftyCategory";
            this.txtartworktype_ftyCategory.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_ftyCategory.TabIndex = 1;
            // 
            // comboOrderBy
            // 
            this.comboOrderBy.BackColor = System.Drawing.Color.White;
            this.comboOrderBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboOrderBy.FormattingEnabled = true;
            this.comboOrderBy.IsSupportUnselect = true;
            this.comboOrderBy.Items.AddRange(new object[] {
            "Issue Date",
            "Supplier"});
            this.comboOrderBy.Location = new System.Drawing.Point(114, 190);
            this.comboOrderBy.Name = "comboOrderBy";
            this.comboOrderBy.Size = new System.Drawing.Size(121, 24);
            this.comboOrderBy.TabIndex = 5;
            // 
            // labelOrderBy
            // 
            this.labelOrderBy.Lines = 0;
            this.labelOrderBy.Location = new System.Drawing.Point(13, 191);
            this.labelOrderBy.Name = "labelOrderBy";
            this.labelOrderBy.Size = new System.Drawing.Size(98, 23);
            this.labelOrderBy.TabIndex = 105;
            this.labelOrderBy.Text = "Order By";
            // 
            // R21
            // 
            this.ClientSize = new System.Drawing.Size(522, 249);
            this.Controls.Add(this.comboOrderBy);
            this.Controls.Add(this.labelOrderBy);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivisionM);
            this.Controls.Add(this.txtsubconSupplier);
            this.Controls.Add(this.txtartworktype_ftyCategory);
            this.Controls.Add(this.dateAPDate);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.labelSupplier);
            this.Controls.Add(this.labelAPDate);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelFactory);
            this.DefaultControl = "dateAPDate";
            this.DefaultControlForEdit = "dateAPDate";
            this.IsSupportToPrint = false;
            this.Name = "R21";
            this.Text = "R21. Outstanding List of Local Payment";
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.labelAPDate, 0);
            this.Controls.SetChildIndex(this.labelSupplier, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.dateAPDate, 0);
            this.Controls.SetChildIndex(this.txtartworktype_ftyCategory, 0);
            this.Controls.SetChildIndex(this.txtsubconSupplier, 0);
            this.Controls.SetChildIndex(this.txtMdivisionM, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelOrderBy, 0);
            this.Controls.SetChildIndex(this.comboOrderBy, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelFactory;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelAPDate;
        private Win.UI.Label labelSupplier;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.DateRange dateAPDate;
        private Class.Txtartworktype_fty txtartworktype_ftyCategory;
        private Class.TxtsubconNoConfirm txtsubconSupplier;
        private Class.TxtMdivision txtMdivisionM;
        private Win.UI.Label labelM;
        private Win.UI.ComboBox comboOrderBy;
        private Win.UI.Label labelOrderBy;
    }
}
