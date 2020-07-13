namespace Sci.Production.Subcon
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
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.dateIssueDate = new Sci.Win.UI.DateRange();
            this.labelM = new Sci.Win.UI.Label();
            this.txtMdivisionM = new Sci.Production.Class.TxtMdivision();
            this.txtsubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.txtartworktype_ftyCategory = new Sci.Production.Class.Txtartworktype_fty();
            this.dateApproveDate = new Sci.Win.UI.DateRange();
            this.labelApproveDate = new Sci.Win.UI.Label();
            this.checkSummary = new Sci.Win.UI.CheckBox();
            this.labIOrderID = new Sci.Win.UI.Label();
            this.txtSPNo2 = new Sci.Win.UI.TextBox();
            this.txtSPNo1 = new Sci.Win.UI.TextBox();
            this.label13 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(430, 12);
            this.print.TabIndex = 7;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(430, 48);
            this.toexcel.TabIndex = 8;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(430, 84);
            this.close.TabIndex = 9;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 188);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 94;
            this.labelFactory.Text = "Factory";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(13, 117);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(98, 23);
            this.labelCategory.TabIndex = 95;
            this.labelCategory.Text = "Category";
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(13, 12);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelIssueDate.RectStyle.BorderWidth = 1F;
            this.labelIssueDate.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelIssueDate.RectStyle.ExtBorderWidth = 1F;
            this.labelIssueDate.Size = new System.Drawing.Size(98, 23);
            this.labelIssueDate.TabIndex = 96;
            this.labelIssueDate.Text = "Issue Date";
            this.labelIssueDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelIssueDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(13, 225);
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
            this.comboFactory.Location = new System.Drawing.Point(114, 187);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 4;
            // 
            // dateIssueDate
            // 
            // 
            // 
            // 
            this.dateIssueDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateIssueDate.DateBox1.Name = "";
            this.dateIssueDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateIssueDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateIssueDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateIssueDate.DateBox2.Name = "";
            this.dateIssueDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateIssueDate.DateBox2.TabIndex = 1;
            this.dateIssueDate.IsRequired = false;
            this.dateIssueDate.Location = new System.Drawing.Point(115, 12);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(280, 23);
            this.dateIssueDate.TabIndex = 0;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 153);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 103;
            this.labelM.Text = "M";
            // 
            // txtMdivisionM
            // 
            this.txtMdivisionM.BackColor = System.Drawing.Color.White;
            this.txtMdivisionM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivisionM.Location = new System.Drawing.Point(114, 153);
            this.txtMdivisionM.Name = "txtMdivisionM";
            this.txtMdivisionM.Size = new System.Drawing.Size(66, 23);
            this.txtMdivisionM.TabIndex = 3;
            // 
            // txtsubconSupplier
            // 
            this.txtsubconSupplier.DisplayBox1Binding = "";
            this.txtsubconSupplier.IsIncludeJunk = true;
            this.txtsubconSupplier.Location = new System.Drawing.Point(115, 225);
            this.txtsubconSupplier.Name = "txtsubconSupplier";
            this.txtsubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtsubconSupplier.TabIndex = 5;
            this.txtsubconSupplier.TextBox1Binding = "";
            // 
            // txtartworktype_ftyCategory
            // 
            this.txtartworktype_ftyCategory.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftyCategory.CClassify = "\'P\'";
            this.txtartworktype_ftyCategory.CSubprocess = "";
            this.txtartworktype_ftyCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftyCategory.Location = new System.Drawing.Point(114, 117);
            this.txtartworktype_ftyCategory.Name = "txtartworktype_ftyCategory";
            this.txtartworktype_ftyCategory.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_ftyCategory.TabIndex = 2;
            // 
            // dateApproveDate
            // 
            // 
            // 
            // 
            this.dateApproveDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateApproveDate.DateBox1.Name = "";
            this.dateApproveDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateApproveDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateApproveDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateApproveDate.DateBox2.Name = "";
            this.dateApproveDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateApproveDate.DateBox2.TabIndex = 1;
            this.dateApproveDate.IsRequired = false;
            this.dateApproveDate.Location = new System.Drawing.Point(115, 48);
            this.dateApproveDate.Name = "dateApproveDate";
            this.dateApproveDate.Size = new System.Drawing.Size(280, 23);
            this.dateApproveDate.TabIndex = 1;
            // 
            // labelApproveDate
            // 
            this.labelApproveDate.Location = new System.Drawing.Point(13, 48);
            this.labelApproveDate.Name = "labelApproveDate";
            this.labelApproveDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelApproveDate.RectStyle.BorderWidth = 1F;
            this.labelApproveDate.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelApproveDate.RectStyle.ExtBorderWidth = 1F;
            this.labelApproveDate.Size = new System.Drawing.Size(98, 23);
            this.labelApproveDate.TabIndex = 105;
            this.labelApproveDate.Text = "Approve Date";
            this.labelApproveDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelApproveDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // checkSummary
            // 
            this.checkSummary.AutoSize = true;
            this.checkSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkSummary.Location = new System.Drawing.Point(300, 227);
            this.checkSummary.Name = "checkSummary";
            this.checkSummary.Size = new System.Drawing.Size(86, 21);
            this.checkSummary.TabIndex = 6;
            this.checkSummary.Text = "Summary";
            this.checkSummary.UseVisualStyleBackColor = true;
            // 
            // labIOrderID
            // 
            this.labIOrderID.Location = new System.Drawing.Point(13, 84);
            this.labIOrderID.Name = "labIOrderID";
            this.labIOrderID.Size = new System.Drawing.Size(98, 23);
            this.labIOrderID.TabIndex = 106;
            this.labIOrderID.Text = "SP#";
            // 
            // txtSPNo2
            // 
            this.txtSPNo2.BackColor = System.Drawing.Color.White;
            this.txtSPNo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo2.Location = new System.Drawing.Point(269, 84);
            this.txtSPNo2.Name = "txtSPNo2";
            this.txtSPNo2.Size = new System.Drawing.Size(128, 23);
            this.txtSPNo2.TabIndex = 124;
            // 
            // txtSPNo1
            // 
            this.txtSPNo1.BackColor = System.Drawing.Color.White;
            this.txtSPNo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo1.Location = new System.Drawing.Point(115, 84);
            this.txtSPNo1.Name = "txtSPNo1";
            this.txtSPNo1.Size = new System.Drawing.Size(128, 23);
            this.txtSPNo1.TabIndex = 123;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label13.Location = new System.Drawing.Point(244, 84);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(22, 23);
            this.label13.TabIndex = 126;
            this.label13.Text = "～";
            this.label13.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // R22
            // 
            this.ClientSize = new System.Drawing.Size(522, 273);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtSPNo2);
            this.Controls.Add(this.txtSPNo1);
            this.Controls.Add(this.labIOrderID);
            this.Controls.Add(this.checkSummary);
            this.Controls.Add(this.dateApproveDate);
            this.Controls.Add(this.labelApproveDate);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivisionM);
            this.Controls.Add(this.txtsubconSupplier);
            this.Controls.Add(this.txtartworktype_ftyCategory);
            this.Controls.Add(this.dateIssueDate);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.labelSupplier);
            this.Controls.Add(this.labelIssueDate);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelFactory);
            this.DefaultControl = "dateIssueDate";
            this.DefaultControlForEdit = "dateIssueDate";
            this.IsSupportToPrint = false;
            this.Name = "R22";
            this.Text = "R22. Local Payment List or Summary";
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.Controls.SetChildIndex(this.labelSupplier, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.Controls.SetChildIndex(this.txtartworktype_ftyCategory, 0);
            this.Controls.SetChildIndex(this.txtsubconSupplier, 0);
            this.Controls.SetChildIndex(this.txtMdivisionM, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelApproveDate, 0);
            this.Controls.SetChildIndex(this.dateApproveDate, 0);
            this.Controls.SetChildIndex(this.checkSummary, 0);
            this.Controls.SetChildIndex(this.labIOrderID, 0);
            this.Controls.SetChildIndex(this.txtSPNo1, 0);
            this.Controls.SetChildIndex(this.txtSPNo2, 0);
            this.Controls.SetChildIndex(this.label13, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelFactory;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelIssueDate;
        private Win.UI.Label labelSupplier;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.DateRange dateIssueDate;
        private Class.Txtartworktype_fty txtartworktype_ftyCategory;
        private Class.TxtsubconNoConfirm txtsubconSupplier;
        private Class.TxtMdivision txtMdivisionM;
        private Win.UI.Label labelM;
        private Win.UI.DateRange dateApproveDate;
        private Win.UI.Label labelApproveDate;
        private Win.UI.CheckBox checkSummary;
        private Win.UI.Label labIOrderID;
        private Win.UI.TextBox txtSPNo2;
        private Win.UI.TextBox txtSPNo1;
        private Win.UI.Label label13;
    }
}
