namespace Sci.Production.Subcon
{
    partial class R14
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
            this.labelStyle = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.dateAPDate = new Sci.Win.UI.DateRange();
            this.labelM = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.labelOrderType = new Sci.Win.UI.Label();
            this.labelRateType = new Sci.Win.UI.Label();
            this.comboStatus = new Sci.Win.UI.ComboBox();
            this.labelStatus = new Sci.Win.UI.Label();
            this.txtSpnoStart = new Sci.Win.UI.TextBox();
            this.txtSpnoEnd = new Sci.Win.UI.TextBox();
            this.labelAPDate = new Sci.Win.UI.Label();
            this.txtMdivisionM = new Sci.Production.Class.TxtMdivision();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.txtartworktype_ftyArtworkType = new Sci.Production.Class.Txtartworktype_fty();
            this.txtFinanceEnReasonRateType = new Sci.Production.Class.TxtFinanceEnReason();
            this.txtdropdownlistOrderType = new Sci.Production.Class.Txtdropdownlist();
            this.dateGLDate = new Sci.Win.UI.DateRange();
            this.labelGLDate = new Sci.Win.UI.Label();
            this.chk_IrregularPriceReason = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(430, 12);
            this.print.TabIndex = 11;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(430, 48);
            this.toexcel.TabIndex = 12;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(430, 84);
            this.close.TabIndex = 13;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(14, 78);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 94;
            this.labelFactory.Text = "Factory";
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.Location = new System.Drawing.Point(13, 111);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.Size = new System.Drawing.Size(98, 23);
            this.labelArtworkType.TabIndex = 95;
            this.labelArtworkType.Text = "ArtworkType";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(14, 210);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(98, 23);
            this.labelStyle.TabIndex = 100;
            this.labelStyle.Text = "Style";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(115, 77);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 2;
            // 
            // dateAPDate
            // 
            // 
            // 
            // 
            this.dateAPDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateAPDate.DateBox1.Name = "";
            this.dateAPDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateAPDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateAPDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateAPDate.DateBox2.Name = "";
            this.dateAPDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateAPDate.DateBox2.TabIndex = 1;
            this.dateAPDate.IsRequired = false;
            this.dateAPDate.Location = new System.Drawing.Point(115, 12);
            this.dateAPDate.Name = "dateAPDate";
            this.dateAPDate.Size = new System.Drawing.Size(280, 23);
            this.dateAPDate.TabIndex = 0;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(14, 45);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 103;
            this.labelM.Text = "M";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(14, 177);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(98, 23);
            this.labelSPNo.TabIndex = 106;
            this.labelSPNo.Text = "SP#";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(246, 178);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 23);
            this.label10.TabIndex = 109;
            this.label10.Text = "～";
            // 
            // labelOrderType
            // 
            this.labelOrderType.Location = new System.Drawing.Point(14, 243);
            this.labelOrderType.Name = "labelOrderType";
            this.labelOrderType.Size = new System.Drawing.Size(98, 23);
            this.labelOrderType.TabIndex = 112;
            this.labelOrderType.Text = "Order Type";
            // 
            // labelRateType
            // 
            this.labelRateType.Location = new System.Drawing.Point(14, 276);
            this.labelRateType.Name = "labelRateType";
            this.labelRateType.Size = new System.Drawing.Size(98, 23);
            this.labelRateType.TabIndex = 114;
            this.labelRateType.Text = "Rate Type";
            // 
            // comboStatus
            // 
            this.comboStatus.BackColor = System.Drawing.Color.White;
            this.comboStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.IsSupportUnselect = true;
            this.comboStatus.Location = new System.Drawing.Point(114, 309);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.OldText = "";
            this.comboStatus.Size = new System.Drawing.Size(158, 24);
            this.comboStatus.TabIndex = 10;
            this.comboStatus.SelectedIndexChanged += new System.EventHandler(this.comboStatus_SelectedIndexChanged);
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(14, 309);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(98, 23);
            this.labelStatus.TabIndex = 116;
            this.labelStatus.Text = "Status";
            // 
            // txtSpnoStart
            // 
            this.txtSpnoStart.BackColor = System.Drawing.Color.White;
            this.txtSpnoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpnoStart.Location = new System.Drawing.Point(115, 178);
            this.txtSpnoStart.Name = "txtSpnoStart";
            this.txtSpnoStart.Size = new System.Drawing.Size(128, 23);
            this.txtSpnoStart.TabIndex = 5;
            // 
            // txtSpnoEnd
            // 
            this.txtSpnoEnd.BackColor = System.Drawing.Color.White;
            this.txtSpnoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpnoEnd.Location = new System.Drawing.Point(268, 178);
            this.txtSpnoEnd.Name = "txtSpnoEnd";
            this.txtSpnoEnd.Size = new System.Drawing.Size(128, 23);
            this.txtSpnoEnd.TabIndex = 6;
            // 
            // labelAPDate
            // 
            this.labelAPDate.Location = new System.Drawing.Point(14, 12);
            this.labelAPDate.Name = "labelAPDate";
            this.labelAPDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelAPDate.RectStyle.BorderWidth = 1F;
            this.labelAPDate.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelAPDate.RectStyle.ExtBorderWidth = 1F;
            this.labelAPDate.Size = new System.Drawing.Size(98, 23);
            this.labelAPDate.TabIndex = 120;
            this.labelAPDate.Text = "AP Date";
            this.labelAPDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelAPDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtMdivisionM
            // 
            this.txtMdivisionM.BackColor = System.Drawing.Color.White;
            this.txtMdivisionM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivisionM.Location = new System.Drawing.Point(115, 46);
            this.txtMdivisionM.Name = "txtMdivisionM";
            this.txtMdivisionM.Size = new System.Drawing.Size(66, 23);
            this.txtMdivisionM.TabIndex = 1;
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(115, 210);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.Size = new System.Drawing.Size(170, 23);
            this.txtstyle.TabIndex = 7;
            this.txtstyle.TarBrand = null;
            this.txtstyle.TarSeason = null;
            // 
            // txtartworktype_ftyArtworkType
            // 
            this.txtartworktype_ftyArtworkType.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftyArtworkType.CClassify = "";
            this.txtartworktype_ftyArtworkType.CSubprocess = "Y";
            this.txtartworktype_ftyArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftyArtworkType.Location = new System.Drawing.Point(114, 111);
            this.txtartworktype_ftyArtworkType.Name = "txtartworktype_ftyArtworkType";
            this.txtartworktype_ftyArtworkType.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_ftyArtworkType.TabIndex = 3;
            // 
            // txtFinanceEnReasonRateType
            // 
            this.txtFinanceEnReasonRateType.BackColor = System.Drawing.Color.White;
            this.txtFinanceEnReasonRateType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFinanceEnReasonRateType.FormattingEnabled = true;
            this.txtFinanceEnReasonRateType.IsSupportUnselect = true;
            this.txtFinanceEnReasonRateType.Location = new System.Drawing.Point(114, 275);
            this.txtFinanceEnReasonRateType.Name = "txtFinanceEnReasonRateType";
            this.txtFinanceEnReasonRateType.OldText = "";
            this.txtFinanceEnReasonRateType.ReasonTypeID = "RateType";
            this.txtFinanceEnReasonRateType.Size = new System.Drawing.Size(261, 24);
            this.txtFinanceEnReasonRateType.TabIndex = 9;
            // 
            // txtdropdownlistOrderType
            // 
            this.txtdropdownlistOrderType.BackColor = System.Drawing.Color.White;
            this.txtdropdownlistOrderType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlistOrderType.FormattingEnabled = true;
            this.txtdropdownlistOrderType.IsSupportUnselect = true;
            this.txtdropdownlistOrderType.Location = new System.Drawing.Point(114, 243);
            this.txtdropdownlistOrderType.Name = "txtdropdownlistOrderType";
            this.txtdropdownlistOrderType.OldText = "";
            this.txtdropdownlistOrderType.Size = new System.Drawing.Size(261, 24);
            this.txtdropdownlistOrderType.TabIndex = 8;
            this.txtdropdownlistOrderType.Type = "orderType";
            // 
            // dateGLDate
            // 
            // 
            // 
            // 
            this.dateGLDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateGLDate.DateBox1.Name = "";
            this.dateGLDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateGLDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateGLDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateGLDate.DateBox2.Name = "";
            this.dateGLDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateGLDate.DateBox2.TabIndex = 1;
            this.dateGLDate.IsRequired = false;
            this.dateGLDate.Location = new System.Drawing.Point(114, 144);
            this.dateGLDate.Name = "dateGLDate";
            this.dateGLDate.Size = new System.Drawing.Size(280, 23);
            this.dateGLDate.TabIndex = 4;
            // 
            // labelGLDate
            // 
            this.labelGLDate.Location = new System.Drawing.Point(14, 144);
            this.labelGLDate.Name = "labelGLDate";
            this.labelGLDate.Size = new System.Drawing.Size(98, 23);
            this.labelGLDate.TabIndex = 122;
            this.labelGLDate.Text = "GL Date";
            // 
            // chk_IrregularPriceReason
            // 
            this.chk_IrregularPriceReason.AutoSize = true;
            this.chk_IrregularPriceReason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chk_IrregularPriceReason.Location = new System.Drawing.Point(14, 335);
            this.chk_IrregularPriceReason.Name = "chk_IrregularPriceReason";
            this.chk_IrregularPriceReason.Size = new System.Drawing.Size(444, 21);
            this.chk_IrregularPriceReason.TabIndex = 123;
            this.chk_IrregularPriceReason.Text = "Only show as item which have irregular price but not enter reason.";
            this.chk_IrregularPriceReason.UseVisualStyleBackColor = true;
            // 
            // R14
            // 
            this.ClientSize = new System.Drawing.Size(522, 388);
            this.Controls.Add(this.chk_IrregularPriceReason);
            this.Controls.Add(this.labelGLDate);
            this.Controls.Add(this.dateGLDate);
            this.Controls.Add(this.txtdropdownlistOrderType);
            this.Controls.Add(this.txtFinanceEnReasonRateType);
            this.Controls.Add(this.labelAPDate);
            this.Controls.Add(this.txtSpnoEnd);
            this.Controls.Add(this.txtSpnoStart);
            this.Controls.Add(this.comboStatus);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelRateType);
            this.Controls.Add(this.labelOrderType);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivisionM);
            this.Controls.Add(this.txtstyle);
            this.Controls.Add(this.txtartworktype_ftyArtworkType);
            this.Controls.Add(this.dateAPDate);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.labelArtworkType);
            this.Controls.Add(this.labelFactory);
            this.DefaultControl = "dateAPDate";
            this.DefaultControlForEdit = "dateAPDate";
            this.IsSupportToPrint = false;
            this.Name = "R14";
            this.Text = "R14. Sub-con Payment summary by SP#";
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelArtworkType, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.dateAPDate, 0);
            this.Controls.SetChildIndex(this.txtartworktype_ftyArtworkType, 0);
            this.Controls.SetChildIndex(this.txtstyle, 0);
            this.Controls.SetChildIndex(this.txtMdivisionM, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.labelOrderType, 0);
            this.Controls.SetChildIndex(this.labelRateType, 0);
            this.Controls.SetChildIndex(this.labelStatus, 0);
            this.Controls.SetChildIndex(this.comboStatus, 0);
            this.Controls.SetChildIndex(this.txtSpnoStart, 0);
            this.Controls.SetChildIndex(this.txtSpnoEnd, 0);
            this.Controls.SetChildIndex(this.labelAPDate, 0);
            this.Controls.SetChildIndex(this.txtFinanceEnReasonRateType, 0);
            this.Controls.SetChildIndex(this.txtdropdownlistOrderType, 0);
            this.Controls.SetChildIndex(this.dateGLDate, 0);
            this.Controls.SetChildIndex(this.labelGLDate, 0);
            this.Controls.SetChildIndex(this.chk_IrregularPriceReason, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelFactory;
        private Win.UI.Label labelArtworkType;
        private Win.UI.Label labelStyle;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.DateRange dateAPDate;
        private Class.Txtartworktype_fty txtartworktype_ftyArtworkType;
        private Class.Txtstyle txtstyle;
        private Class.TxtMdivision txtMdivisionM;
        private Win.UI.Label labelM;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label label10;
        private Win.UI.Label labelOrderType;
        private Win.UI.Label labelRateType;
        private Win.UI.ComboBox comboStatus;
        private Win.UI.Label labelStatus;
        private Win.UI.TextBox txtSpnoStart;
        private Win.UI.TextBox txtSpnoEnd;
        private Win.UI.Label labelAPDate;
        private Class.TxtFinanceEnReason txtFinanceEnReasonRateType;
        private Class.Txtdropdownlist txtdropdownlistOrderType;
        private Win.UI.DateRange dateGLDate;
        private Win.UI.Label labelGLDate;
        private Win.UI.CheckBox chk_IrregularPriceReason;
    }
}
