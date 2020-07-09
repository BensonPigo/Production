namespace Sci.Production.Subcon
{
    partial class R13
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
            this.labelArtworkType = new Sci.Win.UI.Label();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.dateIssueDate = new Sci.Win.UI.DateRange();
            this.labelM = new Sci.Win.UI.Label();
            this.txtMdivisionM = new Sci.Production.Class.TxtMdivision();
            this.txtsubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.txtartworktype_ftyArtworkType = new Sci.Production.Class.Txtartworktype_fty();
            this.dateApproveDate = new Sci.Win.UI.DateRange();
            this.labelApproveDate = new Sci.Win.UI.Label();
            this.checkSummary = new Sci.Win.UI.CheckBox();
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
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(13, 155);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 94;
            this.labelFactory.Text = "Factory";
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.Lines = 0;
            this.labelArtworkType.Location = new System.Drawing.Point(13, 84);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.Size = new System.Drawing.Size(98, 23);
            this.labelArtworkType.TabIndex = 95;
            this.labelArtworkType.Text = "Artwork Type";
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Lines = 0;
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
            this.labelSupplier.Lines = 0;
            this.labelSupplier.Location = new System.Drawing.Point(13, 192);
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
            this.comboFactory.Location = new System.Drawing.Point(114, 154);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 4;
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.IsRequired = false;
            this.dateIssueDate.Location = new System.Drawing.Point(115, 12);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(280, 23);
            this.dateIssueDate.TabIndex = 0;
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(13, 120);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 103;
            this.labelM.Text = "M";
            // 
            // txtMdivisionM
            // 
            this.txtMdivisionM.BackColor = System.Drawing.Color.White;
            this.txtMdivisionM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivisionM.Location = new System.Drawing.Point(114, 120);
            this.txtMdivisionM.Name = "txtMdivisionM";
            this.txtMdivisionM.Size = new System.Drawing.Size(66, 23);
            this.txtMdivisionM.TabIndex = 3;
            // 
            // txtsubconSupplier
            // 
            this.txtsubconSupplier.DisplayBox1Binding = "";
            this.txtsubconSupplier.IsIncludeJunk = true;
            this.txtsubconSupplier.Location = new System.Drawing.Point(115, 192);
            this.txtsubconSupplier.Name = "txtsubconSupplier";
            this.txtsubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtsubconSupplier.TabIndex = 5;
            this.txtsubconSupplier.TextBox1Binding = "";
            // 
            // txtartworktype_ftyArtworkType
            // 
            this.txtartworktype_ftyArtworkType.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftyArtworkType.CClassify = "";
            this.txtartworktype_ftyArtworkType.CSubprocess = "Y";
            this.txtartworktype_ftyArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftyArtworkType.Location = new System.Drawing.Point(114, 84);
            this.txtartworktype_ftyArtworkType.Name = "txtartworktype_ftyArtworkType";
            this.txtartworktype_ftyArtworkType.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_ftyArtworkType.TabIndex = 2;
            // 
            // dateApproveDate
            // 
            this.dateApproveDate.IsRequired = false;
            this.dateApproveDate.Location = new System.Drawing.Point(115, 48);
            this.dateApproveDate.Name = "dateApproveDate";
            this.dateApproveDate.Size = new System.Drawing.Size(280, 23);
            this.dateApproveDate.TabIndex = 1;
            // 
            // labelApproveDate
            // 
            this.labelApproveDate.Lines = 0;
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
            this.checkSummary.Location = new System.Drawing.Point(300, 194);
            this.checkSummary.Name = "checkSummary";
            this.checkSummary.Size = new System.Drawing.Size(86, 21);
            this.checkSummary.TabIndex = 6;
            this.checkSummary.Text = "Summary";
            this.checkSummary.UseVisualStyleBackColor = true;
            // 
            // R13
            // 
            this.ClientSize = new System.Drawing.Size(522, 249);
            this.Controls.Add(this.checkSummary);
            this.Controls.Add(this.dateApproveDate);
            this.Controls.Add(this.labelApproveDate);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivisionM);
            this.Controls.Add(this.txtsubconSupplier);
            this.Controls.Add(this.txtartworktype_ftyArtworkType);
            this.Controls.Add(this.dateIssueDate);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.labelSupplier);
            this.Controls.Add(this.labelIssueDate);
            this.Controls.Add(this.labelArtworkType);
            this.Controls.Add(this.labelFactory);
            this.DefaultControl = "dateIssueDate";
            this.DefaultControlForEdit = "dateIssueDate";
            this.IsSupportToPrint = false;
            this.Name = "R13";
            this.RightToLeftLayout = true;
            this.Text = "R13. Sub-con Payment List & Summary";
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelArtworkType, 0);
            this.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.Controls.SetChildIndex(this.labelSupplier, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.Controls.SetChildIndex(this.txtartworktype_ftyArtworkType, 0);
            this.Controls.SetChildIndex(this.txtsubconSupplier, 0);
            this.Controls.SetChildIndex(this.txtMdivisionM, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelApproveDate, 0);
            this.Controls.SetChildIndex(this.dateApproveDate, 0);
            this.Controls.SetChildIndex(this.checkSummary, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelFactory;
        private Win.UI.Label labelArtworkType;
        private Win.UI.Label labelIssueDate;
        private Win.UI.Label labelSupplier;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.DateRange dateIssueDate;
        private Class.Txtartworktype_fty txtartworktype_ftyArtworkType;
        private Class.TxtsubconNoConfirm txtsubconSupplier;
        private Class.TxtMdivision txtMdivisionM;
        private Win.UI.Label labelM;
        private Win.UI.DateRange dateApproveDate;
        private Win.UI.Label labelApproveDate;
        private Win.UI.CheckBox checkSummary;
    }
}
