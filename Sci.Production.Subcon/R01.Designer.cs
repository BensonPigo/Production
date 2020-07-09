namespace Sci.Production.Subcon
{
    partial class R01
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
            this.label5 = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelOrderBy = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.dateIssueDate = new Sci.Win.UI.DateRange();
            this.comboOrderBy = new Sci.Win.UI.ComboBox();
            this.txtSPNO = new Sci.Win.UI.TextBox();
            this.checkOutstanding = new Sci.Win.UI.CheckBox();
            this.txtsubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.txtartworktype_ftyArtworkType = new Sci.Production.Class.Txtartworktype_fty();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.labelM = new Sci.Win.UI.Label();
            this.txtMdivisionM = new Sci.Production.Class.TxtMdivision();
            this.label9 = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(441, 12);
            this.print.TabIndex = 10;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(441, 48);
            this.toexcel.TabIndex = 11;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(441, 84);
            this.close.TabIndex = 12;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 117);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 94;
            this.labelFactory.Text = "Factory";
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.Location = new System.Drawing.Point(13, 47);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.Size = new System.Drawing.Size(98, 23);
            this.labelArtworkType.TabIndex = 95;
            this.labelArtworkType.Text = "Artwork Type";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(13, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 23);
            this.label5.TabIndex = 98;
            this.label5.Text = "Supplier";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(13, 187);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(98, 23);
            this.labelSPNo.TabIndex = 99;
            this.labelSPNo.Text = "SP#";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(13, 257);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(98, 23);
            this.labelStyle.TabIndex = 100;
            this.labelStyle.Text = "Style";
            // 
            // labelOrderBy
            // 
            this.labelOrderBy.Location = new System.Drawing.Point(13, 292);
            this.labelOrderBy.Name = "labelOrderBy";
            this.labelOrderBy.Size = new System.Drawing.Size(98, 23);
            this.labelOrderBy.TabIndex = 101;
            this.labelOrderBy.Text = "Order By";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(114, 116);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 3;
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
            // comboOrderBy
            // 
            this.comboOrderBy.BackColor = System.Drawing.Color.White;
            this.comboOrderBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboOrderBy.FormattingEnabled = true;
            this.comboOrderBy.IsSupportUnselect = true;
            this.comboOrderBy.Items.AddRange(new object[] {
            "Issue Date",
            "Supplier"});
            this.comboOrderBy.Location = new System.Drawing.Point(114, 291);
            this.comboOrderBy.Name = "comboOrderBy";
            this.comboOrderBy.OldText = "";
            this.comboOrderBy.Size = new System.Drawing.Size(121, 24);
            this.comboOrderBy.TabIndex = 8;
            // 
            // txtSPNO
            // 
            this.txtSPNO.BackColor = System.Drawing.Color.White;
            this.txtSPNO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNO.Location = new System.Drawing.Point(115, 187);
            this.txtSPNO.MaxLength = 13;
            this.txtSPNO.Name = "txtSPNO";
            this.txtSPNO.Size = new System.Drawing.Size(146, 23);
            this.txtSPNO.TabIndex = 5;
            // 
            // checkOutstanding
            // 
            this.checkOutstanding.AutoSize = true;
            this.checkOutstanding.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkOutstanding.Location = new System.Drawing.Point(13, 329);
            this.checkOutstanding.Name = "checkOutstanding";
            this.checkOutstanding.Size = new System.Drawing.Size(407, 21);
            this.checkOutstanding.TabIndex = 9;
            this.checkOutstanding.Text = "Outstanding (Without payment only && Exclude status closed)";
            this.checkOutstanding.UseVisualStyleBackColor = true;
            // 
            // txtsubconSupplier
            // 
            this.txtsubconSupplier.DisplayBox1Binding = "";
            this.txtsubconSupplier.IsIncludeJunk = true;
            this.txtsubconSupplier.IsMisc = false;
            this.txtsubconSupplier.IsShipping = false;
            this.txtsubconSupplier.IsSubcon = false;
            this.txtsubconSupplier.Location = new System.Drawing.Point(115, 152);
            this.txtsubconSupplier.Name = "txtsubconSupplier";
            this.txtsubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtsubconSupplier.TabIndex = 4;
            this.txtsubconSupplier.TextBox1Binding = "";
            // 
            // txtartworktype_ftyArtworkType
            // 
            this.txtartworktype_ftyArtworkType.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftyArtworkType.CClassify = "";
            this.txtartworktype_ftyArtworkType.CSubprocess = "Y";
            this.txtartworktype_ftyArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftyArtworkType.Location = new System.Drawing.Point(114, 47);
            this.txtartworktype_ftyArtworkType.Name = "txtartworktype_ftyArtworkType";
            this.txtartworktype_ftyArtworkType.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_ftyArtworkType.TabIndex = 1;
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(115, 257);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 7;
            this.txtstyle.TarBrand = null;
            this.txtstyle.TarSeason = null;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 82);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 105;
            this.labelM.Text = "M";
            // 
            // txtMdivisionM
            // 
            this.txtMdivisionM.BackColor = System.Drawing.Color.White;
            this.txtMdivisionM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivisionM.Location = new System.Drawing.Point(114, 82);
            this.txtMdivisionM.Name = "txtMdivisionM";
            this.txtMdivisionM.Size = new System.Drawing.Size(66, 23);
            this.txtMdivisionM.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(13, 12);
            this.label9.Name = "label9";
            this.label9.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.RectStyle.BorderWidth = 1F;
            this.label9.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label9.RectStyle.ExtBorderWidth = 1F;
            this.label9.Size = new System.Drawing.Size(98, 23);
            this.label9.TabIndex = 106;
            this.label9.Text = "Issue Date";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 222);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(98, 23);
            this.labelBrand.TabIndex = 107;
            this.labelBrand.Text = "Brand";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(115, 222);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(83, 23);
            this.txtbrand.TabIndex = 6;
            // 
            // R01
            // 
            this.ClientSize = new System.Drawing.Size(533, 381);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivisionM);
            this.Controls.Add(this.txtstyle);
            this.Controls.Add(this.checkOutstanding);
            this.Controls.Add(this.txtsubconSupplier);
            this.Controls.Add(this.txtartworktype_ftyArtworkType);
            this.Controls.Add(this.txtSPNO);
            this.Controls.Add(this.comboOrderBy);
            this.Controls.Add(this.dateIssueDate);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.labelOrderBy);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labelArtworkType);
            this.Controls.Add(this.labelFactory);
            this.DefaultControl = "dateIssueDate";
            this.DefaultControlForEdit = "dateIssueDate";
            this.IsSupportToPrint = false;
            this.Name = "R01";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R01. Subcon PO List";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelArtworkType, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.labelOrderBy, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.Controls.SetChildIndex(this.comboOrderBy, 0);
            this.Controls.SetChildIndex(this.txtSPNO, 0);
            this.Controls.SetChildIndex(this.txtartworktype_ftyArtworkType, 0);
            this.Controls.SetChildIndex(this.txtsubconSupplier, 0);
            this.Controls.SetChildIndex(this.checkOutstanding, 0);
            this.Controls.SetChildIndex(this.txtstyle, 0);
            this.Controls.SetChildIndex(this.txtMdivisionM, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelFactory;
        private Win.UI.Label labelArtworkType;
        private Win.UI.Label label5;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelStyle;
        private Win.UI.Label labelOrderBy;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.DateRange dateIssueDate;
        private Win.UI.ComboBox comboOrderBy;
        private Win.UI.TextBox txtSPNO;
        private Class.Txtartworktype_fty txtartworktype_ftyArtworkType;
        private Class.TxtsubconNoConfirm txtsubconSupplier;
        private Win.UI.CheckBox checkOutstanding;
        private Class.Txtstyle txtstyle;
        private Win.UI.Label labelM;
        private Class.TxtMdivision txtMdivisionM;
        private Win.UI.Label label9;
        private Win.UI.Label labelBrand;
        private Class.Txtbrand txtbrand;
    }
}
