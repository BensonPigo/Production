namespace Sci.Production.Planning
{
    partial class R07
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
            this.components = new System.ComponentModel.Container();
            this.lbOutputDate = new Sci.Win.UI.Label();
            this.dateOutputDate = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.labelM = new Sci.Win.UI.Label();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.lbCDcode = new Sci.Win.UI.Label();
            this.lbShift = new Sci.Win.UI.Label();
            this.txtCDCode = new Sci.Win.UI.TextBox();
            this.comboShift = new Sci.Win.UI.ComboBox();
            this.lbFormat = new Sci.Win.UI.Label();
            this.radioSintexEffReportCompare = new Sci.Win.UI.RadioButton();
            this.radioDetail = new Sci.Win.UI.RadioButton();
            this.numYear = new System.Windows.Forms.NumericUpDown();
            this.lbBrand = new Sci.Win.UI.Label();
            this.txtbrand1 = new Sci.Production.Class.Txtbrand();
            this.lbConstruction = new Sci.Win.UI.Label();
            this.lbGender = new Sci.Win.UI.Label();
            this.lbLining = new Sci.Win.UI.Label();
            this.lbFabricType = new Sci.Win.UI.Label();
            this.lbProductType = new Sci.Win.UI.Label();
            this.comboGender1 = new Sci.Production.Class.ComboGender(this.components);
            this.comboFabricType1 = new Sci.Production.Class.ComboFabricType(this.components);
            this.comboLining1 = new Sci.Production.Class.ComboLining(this.components);
            this.comboProductType1 = new Sci.Production.Class.ComboProductType(this.components);
            this.comboConstruction1 = new Sci.Production.Class.ComboConstruction(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).BeginInit();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(437, 12);
            this.print.TabIndex = 4;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(437, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(437, 84);
            this.close.TabIndex = 6;
            // 
            // lbOutputDate
            // 
            this.lbOutputDate.Location = new System.Drawing.Point(13, 12);
            this.lbOutputDate.Name = "lbOutputDate";
            this.lbOutputDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lbOutputDate.RectStyle.BorderWidth = 1F;
            this.lbOutputDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbOutputDate.RectStyle.ExtBorderWidth = 1F;
            this.lbOutputDate.Size = new System.Drawing.Size(98, 23);
            this.lbOutputDate.TabIndex = 96;
            this.lbOutputDate.Text = "OutputDate";
            this.lbOutputDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lbOutputDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateOutputDate
            // 
            // 
            // 
            // 
            this.dateOutputDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateOutputDate.DateBox1.Name = "";
            this.dateOutputDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateOutputDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateOutputDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateOutputDate.DateBox2.Name = "";
            this.dateOutputDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateOutputDate.DateBox2.TabIndex = 1;
            this.dateOutputDate.IsRequired = false;
            this.dateOutputDate.Location = new System.Drawing.Point(115, 12);
            this.dateOutputDate.Name = "dateOutputDate";
            this.dateOutputDate.Size = new System.Drawing.Size(280, 23);
            this.dateOutputDate.TabIndex = 0;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 83);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 132;
            this.labelFactory.Text = "Factory";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = true;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(115, 83);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 2;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 48);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 134;
            this.labelM.Text = "M";
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(115, 49);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 1;
            this.txtMdivision.DefaultValue = true;
            // 
            // lbCDcode
            // 
            this.lbCDcode.Location = new System.Drawing.Point(13, 160);
            this.lbCDcode.Name = "lbCDcode";
            this.lbCDcode.Size = new System.Drawing.Size(98, 23);
            this.lbCDcode.TabIndex = 136;
            this.lbCDcode.Text = "CD Code";
            // 
            // lbShift
            // 
            this.lbShift.Location = new System.Drawing.Point(13, 364);
            this.lbShift.Name = "lbShift";
            this.lbShift.Size = new System.Drawing.Size(98, 23);
            this.lbShift.TabIndex = 137;
            this.lbShift.Text = "Shift";
            // 
            // txtCDCode
            // 
            this.txtCDCode.BackColor = System.Drawing.Color.White;
            this.txtCDCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCDCode.Location = new System.Drawing.Point(115, 160);
            this.txtCDCode.Name = "txtCDCode";
            this.txtCDCode.Size = new System.Drawing.Size(100, 23);
            this.txtCDCode.TabIndex = 138;
            // 
            // comboShift
            // 
            this.comboShift.BackColor = System.Drawing.Color.White;
            this.comboShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShift.FormattingEnabled = true;
            this.comboShift.IsSupportUnselect = true;
            this.comboShift.Location = new System.Drawing.Point(115, 364);
            this.comboShift.Name = "comboShift";
            this.comboShift.OldText = "";
            this.comboShift.Size = new System.Drawing.Size(150, 24);
            this.comboShift.TabIndex = 139;
            // 
            // lbFormat
            // 
            this.lbFormat.Location = new System.Drawing.Point(13, 399);
            this.lbFormat.Name = "lbFormat";
            this.lbFormat.Size = new System.Drawing.Size(98, 23);
            this.lbFormat.TabIndex = 140;
            this.lbFormat.Text = "Format";
            // 
            // radioSintexEffReportCompare
            // 
            this.radioSintexEffReportCompare.AutoSize = true;
            this.radioSintexEffReportCompare.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSintexEffReportCompare.Location = new System.Drawing.Point(115, 427);
            this.radioSintexEffReportCompare.Name = "radioSintexEffReportCompare";
            this.radioSintexEffReportCompare.Size = new System.Drawing.Size(193, 21);
            this.radioSintexEffReportCompare.TabIndex = 142;
            this.radioSintexEffReportCompare.Text = "Sintex Eff Report Compare";
            this.radioSintexEffReportCompare.UseVisualStyleBackColor = true;
            this.radioSintexEffReportCompare.CheckedChanged += new System.EventHandler(this.RadioSintexEffReportCompare_CheckedChanged);
            // 
            // radioDetail
            // 
            this.radioDetail.AutoSize = true;
            this.radioDetail.Checked = true;
            this.radioDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail.Location = new System.Drawing.Point(115, 399);
            this.radioDetail.Name = "radioDetail";
            this.radioDetail.Size = new System.Drawing.Size(82, 21);
            this.radioDetail.TabIndex = 141;
            this.radioDetail.TabStop = true;
            this.radioDetail.Text = "By Detail";
            this.radioDetail.UseVisualStyleBackColor = true;
            this.radioDetail.CheckedChanged += new System.EventHandler(this.RadioDetail_CheckedChanged);
            // 
            // numYear
            // 
            this.numYear.Location = new System.Drawing.Point(115, 12);
            this.numYear.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.numYear.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numYear.Name = "numYear";
            this.numYear.Size = new System.Drawing.Size(72, 23);
            this.numYear.TabIndex = 143;
            this.numYear.Value = new decimal(new int[] {
            2015,
            0,
            0,
            0});
            this.numYear.Visible = false;
            // 
            // lbBrand
            // 
            this.lbBrand.Location = new System.Drawing.Point(13, 120);
            this.lbBrand.Name = "lbBrand";
            this.lbBrand.Size = new System.Drawing.Size(98, 23);
            this.lbBrand.TabIndex = 144;
            this.lbBrand.Text = "Brand";
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(115, 120);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(300, 23);
            this.txtbrand1.TabIndex = 145;
            // 
            // lbConstruction
            // 
            this.lbConstruction.Location = new System.Drawing.Point(13, 298);
            this.lbConstruction.Name = "lbConstruction";
            this.lbConstruction.Size = new System.Drawing.Size(98, 23);
            this.lbConstruction.TabIndex = 247;
            this.lbConstruction.Text = "Construction";
            // 
            // lbGender
            // 
            this.lbGender.Location = new System.Drawing.Point(13, 331);
            this.lbGender.Name = "lbGender";
            this.lbGender.Size = new System.Drawing.Size(98, 23);
            this.lbGender.TabIndex = 246;
            this.lbGender.Text = "Gender";
            // 
            // lbLining
            // 
            this.lbLining.Location = new System.Drawing.Point(13, 263);
            this.lbLining.Name = "lbLining";
            this.lbLining.Size = new System.Drawing.Size(98, 23);
            this.lbLining.TabIndex = 245;
            this.lbLining.Text = "Lining";
            // 
            // lbFabricType
            // 
            this.lbFabricType.Location = new System.Drawing.Point(13, 228);
            this.lbFabricType.Name = "lbFabricType";
            this.lbFabricType.Size = new System.Drawing.Size(98, 23);
            this.lbFabricType.TabIndex = 244;
            this.lbFabricType.Text = "Fabric Type";
            // 
            // lbProductType
            // 
            this.lbProductType.Location = new System.Drawing.Point(13, 194);
            this.lbProductType.Name = "lbProductType";
            this.lbProductType.Size = new System.Drawing.Size(98, 23);
            this.lbProductType.TabIndex = 243;
            this.lbProductType.Text = "Product Type";
            // 
            // comboGender1
            // 
            this.comboGender1.BackColor = System.Drawing.Color.White;
            this.comboGender1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboGender1.FormattingEnabled = true;
            this.comboGender1.IsSupportUnselect = true;
            this.comboGender1.Location = new System.Drawing.Point(114, 330);
            this.comboGender1.Name = "comboGender1";
            this.comboGender1.OldText = "";
            this.comboGender1.Size = new System.Drawing.Size(129, 24);
            this.comboGender1.StyleGender = "";
            this.comboGender1.TabIndex = 252;
            // 
            // comboFabricType1
            // 
            this.comboFabricType1.BackColor = System.Drawing.Color.White;
            this.comboFabricType1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFabricType1.FormattingEnabled = true;
            this.comboFabricType1.IsJunk = false;
            this.comboFabricType1.IsSupportUnselect = true;
            this.comboFabricType1.Location = new System.Drawing.Point(114, 227);
            this.comboFabricType1.Name = "comboFabricType1";
            this.comboFabricType1.OldText = "";
            this.comboFabricType1.Size = new System.Drawing.Size(129, 24);
            this.comboFabricType1.StyleFabricType = "";
            this.comboFabricType1.TabIndex = 251;
            // 
            // comboLining1
            // 
            this.comboLining1.BackColor = System.Drawing.Color.White;
            this.comboLining1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboLining1.FormattingEnabled = true;
            this.comboLining1.IsSupportUnselect = true;
            this.comboLining1.Location = new System.Drawing.Point(114, 262);
            this.comboLining1.Name = "comboLining1";
            this.comboLining1.OldText = "";
            this.comboLining1.Size = new System.Drawing.Size(129, 24);
            this.comboLining1.StyleLining = "";
            this.comboLining1.TabIndex = 250;
            // 
            // comboProductType1
            // 
            this.comboProductType1.BackColor = System.Drawing.Color.White;
            this.comboProductType1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboProductType1.FormattingEnabled = true;
            this.comboProductType1.IsJunk = false;
            this.comboProductType1.IsSupportUnselect = true;
            this.comboProductType1.Location = new System.Drawing.Point(114, 193);
            this.comboProductType1.Name = "comboProductType1";
            this.comboProductType1.OldText = "";
            this.comboProductType1.Size = new System.Drawing.Size(129, 24);
            this.comboProductType1.StyleApparelType = "";
            this.comboProductType1.TabIndex = 249;
            // 
            // comboConstruction1
            // 
            this.comboConstruction1.BackColor = System.Drawing.Color.White;
            this.comboConstruction1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboConstruction1.FormattingEnabled = true;
            this.comboConstruction1.IsSupportUnselect = true;
            this.comboConstruction1.Location = new System.Drawing.Point(114, 297);
            this.comboConstruction1.Name = "comboConstruction1";
            this.comboConstruction1.OldText = "";
            this.comboConstruction1.Size = new System.Drawing.Size(129, 24);
            this.comboConstruction1.StyleGender = "";
            this.comboConstruction1.TabIndex = 248;
            // 
            // R07
            // 
            this.ClientSize = new System.Drawing.Size(529, 500);
            this.Controls.Add(this.lbConstruction);
            this.Controls.Add(this.lbGender);
            this.Controls.Add(this.lbLining);
            this.Controls.Add(this.lbFabricType);
            this.Controls.Add(this.lbProductType);
            this.Controls.Add(this.comboGender1);
            this.Controls.Add(this.comboFabricType1);
            this.Controls.Add(this.comboLining1);
            this.Controls.Add(this.comboProductType1);
            this.Controls.Add(this.comboConstruction1);
            this.Controls.Add(this.txtbrand1);
            this.Controls.Add(this.lbBrand);
            this.Controls.Add(this.numYear);
            this.Controls.Add(this.radioSintexEffReportCompare);
            this.Controls.Add(this.radioDetail);
            this.Controls.Add(this.lbFormat);
            this.Controls.Add(this.comboShift);
            this.Controls.Add(this.txtCDCode);
            this.Controls.Add(this.lbShift);
            this.Controls.Add(this.lbCDcode);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.dateOutputDate);
            this.Controls.Add(this.lbOutputDate);
            this.DefaultControl = "dateOutputDate";
            this.DefaultControlForEdit = "dateOutputDate";
            this.IsSupportToPrint = false;
            this.Name = "R07";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R07. Adidas Efficiency Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.lbOutputDate, 0);
            this.Controls.SetChildIndex(this.dateOutputDate, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.lbCDcode, 0);
            this.Controls.SetChildIndex(this.lbShift, 0);
            this.Controls.SetChildIndex(this.txtCDCode, 0);
            this.Controls.SetChildIndex(this.comboShift, 0);
            this.Controls.SetChildIndex(this.lbFormat, 0);
            this.Controls.SetChildIndex(this.radioDetail, 0);
            this.Controls.SetChildIndex(this.radioSintexEffReportCompare, 0);
            this.Controls.SetChildIndex(this.numYear, 0);
            this.Controls.SetChildIndex(this.lbBrand, 0);
            this.Controls.SetChildIndex(this.txtbrand1, 0);
            this.Controls.SetChildIndex(this.comboConstruction1, 0);
            this.Controls.SetChildIndex(this.comboProductType1, 0);
            this.Controls.SetChildIndex(this.comboLining1, 0);
            this.Controls.SetChildIndex(this.comboFabricType1, 0);
            this.Controls.SetChildIndex(this.comboGender1, 0);
            this.Controls.SetChildIndex(this.lbProductType, 0);
            this.Controls.SetChildIndex(this.lbFabricType, 0);
            this.Controls.SetChildIndex(this.lbLining, 0);
            this.Controls.SetChildIndex(this.lbGender, 0);
            this.Controls.SetChildIndex(this.lbConstruction, 0);
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbOutputDate;
        private Win.UI.DateRange dateOutputDate;
        private Win.UI.Label labelFactory;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label labelM;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.Label lbCDcode;
        private Win.UI.Label lbShift;
        private Win.UI.TextBox txtCDCode;
        private Win.UI.ComboBox comboShift;
        private Win.UI.Label lbFormat;
        private Win.UI.RadioButton radioSintexEffReportCompare;
        private Win.UI.RadioButton radioDetail;
        private System.Windows.Forms.NumericUpDown numYear;
        private Win.UI.Label lbBrand;
        private Class.Txtbrand txtbrand1;
        private Win.UI.Label lbConstruction;
        private Win.UI.Label lbGender;
        private Win.UI.Label lbLining;
        private Win.UI.Label lbFabricType;
        private Win.UI.Label lbProductType;
        private Class.ComboGender comboGender1;
        private Class.ComboFabricType comboFabricType1;
        private Class.ComboLining comboLining1;
        private Class.ComboProductType comboProductType1;
        private Class.ComboConstruction comboConstruction1;
    }
}
