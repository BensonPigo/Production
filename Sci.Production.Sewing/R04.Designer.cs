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
            this.components = new System.ComponentModel.Container();
            this.labelOoutputDate = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelCDCode = new Sci.Win.UI.Label();
            this.dateOoutputDate = new Sci.Win.UI.DateRange();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.chk_Accumulate_output = new Sci.Win.UI.CheckBox();
            this.chk_Include_Artwork = new Sci.Win.UI.CheckBox();
            this.chkExcludeSampleFty = new Sci.Win.UI.CheckBox();
            this.lbShift = new Sci.Win.UI.Label();
            this.comboShift = new Sci.Win.UI.ComboBox();
            this.chkOnlyCancelOrder = new Sci.Win.UI.CheckBox();
            this.chkExcludeNonRevenue = new Sci.Win.UI.CheckBox();
            this.chkSubconOut = new Sci.Win.UI.CheckBox();
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
            this.txtCDCode = new Sci.Production.Class.Txtcdcode();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.comboFactory = new Sci.Production.Class.ComboFactory(this.components);
            this.comboM = new Sci.Production.Class.ComboMDivision(this.components);
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(441, 12);
            this.print.TabIndex = 6;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(441, 48);
            this.toexcel.TabIndex = 7;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(441, 84);
            this.close.TabIndex = 8;
            // 
            // labelOoutputDate
            // 
            this.labelOoutputDate.Location = new System.Drawing.Point(24, 12);
            this.labelOoutputDate.Name = "labelOoutputDate";
            this.labelOoutputDate.Size = new System.Drawing.Size(98, 23);
            this.labelOoutputDate.TabIndex = 94;
            this.labelOoutputDate.Text = "Output Date";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(24, 47);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(98, 23);
            this.labelCategory.TabIndex = 95;
            this.labelCategory.Text = "Category";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(24, 83);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 96;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(24, 119);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 97;
            this.labelFactory.Text = "Factory";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(24, 155);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(98, 23);
            this.labelBrand.TabIndex = 98;
            this.labelBrand.Text = "Brand";
            // 
            // labelCDCode
            // 
            this.labelCDCode.Location = new System.Drawing.Point(24, 190);
            this.labelCDCode.Name = "labelCDCode";
            this.labelCDCode.Size = new System.Drawing.Size(98, 23);
            this.labelCDCode.TabIndex = 99;
            this.labelCDCode.Text = "CD Code";
            // 
            // dateOoutputDate
            // 
            // 
            // 
            // 
            this.dateOoutputDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateOoutputDate.DateBox1.Name = "";
            this.dateOoutputDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateOoutputDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateOoutputDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateOoutputDate.DateBox2.Name = "";
            this.dateOoutputDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateOoutputDate.DateBox2.TabIndex = 1;
            this.dateOoutputDate.IsRequired = false;
            this.dateOoutputDate.Location = new System.Drawing.Point(125, 12);
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
            this.comboCategory.Location = new System.Drawing.Point(125, 47);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(170, 24);
            this.comboCategory.TabIndex = 1;
            // 
            // chk_Accumulate_output
            // 
            this.chk_Accumulate_output.AutoSize = true;
            this.chk_Accumulate_output.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chk_Accumulate_output.Location = new System.Drawing.Point(291, 146);
            this.chk_Accumulate_output.Name = "chk_Accumulate_output";
            this.chk_Accumulate_output.Size = new System.Drawing.Size(193, 21);
            this.chk_Accumulate_output.TabIndex = 100;
            this.chk_Accumulate_output.Text = "Include Accumulate output";
            this.chk_Accumulate_output.UseVisualStyleBackColor = true;
            // 
            // chk_Include_Artwork
            // 
            this.chk_Include_Artwork.AutoSize = true;
            this.chk_Include_Artwork.Checked = true;
            this.chk_Include_Artwork.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_Include_Artwork.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chk_Include_Artwork.Location = new System.Drawing.Point(291, 119);
            this.chk_Include_Artwork.Name = "chk_Include_Artwork";
            this.chk_Include_Artwork.Size = new System.Drawing.Size(155, 21);
            this.chk_Include_Artwork.TabIndex = 101;
            this.chk_Include_Artwork.Text = "Include Artwork data";
            this.chk_Include_Artwork.UseVisualStyleBackColor = true;
            // 
            // chkExcludeSampleFty
            // 
            this.chkExcludeSampleFty.AutoSize = true;
            this.chkExcludeSampleFty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkExcludeSampleFty.Location = new System.Drawing.Point(291, 173);
            this.chkExcludeSampleFty.Name = "chkExcludeSampleFty";
            this.chkExcludeSampleFty.Size = new System.Drawing.Size(178, 21);
            this.chkExcludeSampleFty.TabIndex = 102;
            this.chkExcludeSampleFty.Text = "Exclude Sample Factory";
            this.chkExcludeSampleFty.UseVisualStyleBackColor = true;
            // 
            // lbShift
            // 
            this.lbShift.Location = new System.Drawing.Point(24, 394);
            this.lbShift.Name = "lbShift";
            this.lbShift.Size = new System.Drawing.Size(98, 23);
            this.lbShift.TabIndex = 103;
            this.lbShift.Text = "Shift";
            // 
            // comboShift
            // 
            this.comboShift.BackColor = System.Drawing.Color.White;
            this.comboShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShift.FormattingEnabled = true;
            this.comboShift.IsSupportUnselect = true;
            this.comboShift.Location = new System.Drawing.Point(125, 393);
            this.comboShift.Name = "comboShift";
            this.comboShift.OldText = "";
            this.comboShift.Size = new System.Drawing.Size(170, 24);
            this.comboShift.TabIndex = 104;
            // 
            // chkOnlyCancelOrder
            // 
            this.chkOnlyCancelOrder.AutoSize = true;
            this.chkOnlyCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkOnlyCancelOrder.Location = new System.Drawing.Point(291, 200);
            this.chkOnlyCancelOrder.Name = "chkOnlyCancelOrder";
            this.chkOnlyCancelOrder.Size = new System.Drawing.Size(144, 21);
            this.chkOnlyCancelOrder.TabIndex = 105;
            this.chkOnlyCancelOrder.Text = "Only Cancel Order";
            this.chkOnlyCancelOrder.UseVisualStyleBackColor = true;
            // 
            // chkExcludeNonRevenue
            // 
            this.chkExcludeNonRevenue.AutoSize = true;
            this.chkExcludeNonRevenue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkExcludeNonRevenue.Location = new System.Drawing.Point(291, 228);
            this.chkExcludeNonRevenue.Name = "chkExcludeNonRevenue";
            this.chkExcludeNonRevenue.Size = new System.Drawing.Size(167, 21);
            this.chkExcludeNonRevenue.TabIndex = 110;
            this.chkExcludeNonRevenue.Text = "Exclude Non Revenue";
            this.chkExcludeNonRevenue.UseVisualStyleBackColor = true;
            // 
            // chkSubconOut
            // 
            this.chkSubconOut.AutoSize = true;
            this.chkSubconOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSubconOut.Location = new System.Drawing.Point(291, 252);
            this.chkSubconOut.Name = "chkSubconOut";
            this.chkSubconOut.Size = new System.Drawing.Size(135, 21);
            this.chkSubconOut.TabIndex = 111;
            this.chkSubconOut.Text = "Only Subcon Out";
            this.chkSubconOut.UseVisualStyleBackColor = true;
            // 
            // lbConstruction
            // 
            this.lbConstruction.Location = new System.Drawing.Point(24, 328);
            this.lbConstruction.Name = "lbConstruction";
            this.lbConstruction.Size = new System.Drawing.Size(98, 23);
            this.lbConstruction.TabIndex = 116;
            this.lbConstruction.Text = "Construction";
            // 
            // lbGender
            // 
            this.lbGender.Location = new System.Drawing.Point(24, 361);
            this.lbGender.Name = "lbGender";
            this.lbGender.Size = new System.Drawing.Size(98, 23);
            this.lbGender.TabIndex = 115;
            this.lbGender.Text = "Gender";
            // 
            // lbLining
            // 
            this.lbLining.Location = new System.Drawing.Point(24, 293);
            this.lbLining.Name = "lbLining";
            this.lbLining.Size = new System.Drawing.Size(98, 23);
            this.lbLining.TabIndex = 114;
            this.lbLining.Text = "Lining";
            // 
            // lbFabricType
            // 
            this.lbFabricType.Location = new System.Drawing.Point(24, 258);
            this.lbFabricType.Name = "lbFabricType";
            this.lbFabricType.Size = new System.Drawing.Size(98, 23);
            this.lbFabricType.TabIndex = 113;
            this.lbFabricType.Text = "Fabric Type";
            // 
            // lbProductType
            // 
            this.lbProductType.Location = new System.Drawing.Point(24, 224);
            this.lbProductType.Name = "lbProductType";
            this.lbProductType.Size = new System.Drawing.Size(98, 23);
            this.lbProductType.TabIndex = 112;
            this.lbProductType.Text = "Product Type";
            // 
            // comboGender1
            // 
            this.comboGender1.BackColor = System.Drawing.Color.White;
            this.comboGender1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboGender1.FormattingEnabled = true;
            this.comboGender1.IsSupportUnselect = true;
            this.comboGender1.Location = new System.Drawing.Point(125, 360);
            this.comboGender1.Name = "comboGender1";
            this.comboGender1.OldText = "";
            this.comboGender1.Size = new System.Drawing.Size(129, 24);
            this.comboGender1.StyleGender = "";
            this.comboGender1.TabIndex = 121;
            // 
            // comboFabricType1
            // 
            this.comboFabricType1.BackColor = System.Drawing.Color.White;
            this.comboFabricType1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFabricType1.FormattingEnabled = true;
            this.comboFabricType1.IsJunk = false;
            this.comboFabricType1.IsSupportUnselect = true;
            this.comboFabricType1.Location = new System.Drawing.Point(125, 257);
            this.comboFabricType1.Name = "comboFabricType1";
            this.comboFabricType1.OldText = "";
            this.comboFabricType1.Size = new System.Drawing.Size(129, 24);
            this.comboFabricType1.StyleFabricType = "";
            this.comboFabricType1.TabIndex = 120;
            // 
            // comboLining1
            // 
            this.comboLining1.BackColor = System.Drawing.Color.White;
            this.comboLining1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboLining1.FormattingEnabled = true;
            this.comboLining1.IsSupportUnselect = true;
            this.comboLining1.Location = new System.Drawing.Point(125, 292);
            this.comboLining1.Name = "comboLining1";
            this.comboLining1.OldText = "";
            this.comboLining1.Size = new System.Drawing.Size(129, 24);
            this.comboLining1.StyleLining = "";
            this.comboLining1.TabIndex = 119;
            // 
            // comboProductType1
            // 
            this.comboProductType1.BackColor = System.Drawing.Color.White;
            this.comboProductType1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboProductType1.FormattingEnabled = true;
            this.comboProductType1.IsJunk = false;
            this.comboProductType1.IsSupportUnselect = true;
            this.comboProductType1.Location = new System.Drawing.Point(125, 223);
            this.comboProductType1.Name = "comboProductType1";
            this.comboProductType1.OldText = "";
            this.comboProductType1.Size = new System.Drawing.Size(129, 24);
            this.comboProductType1.StyleApparelType = "";
            this.comboProductType1.TabIndex = 118;
            // 
            // comboConstruction1
            // 
            this.comboConstruction1.BackColor = System.Drawing.Color.White;
            this.comboConstruction1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboConstruction1.FormattingEnabled = true;
            this.comboConstruction1.IsSupportUnselect = true;
            this.comboConstruction1.Location = new System.Drawing.Point(125, 327);
            this.comboConstruction1.Name = "comboConstruction1";
            this.comboConstruction1.OldText = "";
            this.comboConstruction1.Size = new System.Drawing.Size(129, 24);
            this.comboConstruction1.StyleGender = "";
            this.comboConstruction1.TabIndex = 117;
            // 
            // txtCDCode
            // 
            this.txtCDCode.BackColor = System.Drawing.Color.White;
            this.txtCDCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCDCode.Location = new System.Drawing.Point(125, 190);
            this.txtCDCode.Name = "txtCDCode";
            this.txtCDCode.Size = new System.Drawing.Size(54, 23);
            this.txtCDCode.TabIndex = 5;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(125, 155);
            this.txtbrand.MyDocumentdName = null;
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(90, 23);
            this.txtbrand.TabIndex = 4;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.FilteMDivision = true;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IssupportJunk = false;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(125, 119);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(73, 24);
            this.comboFactory.TabIndex = 567;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(125, 83);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(73, 24);
            this.comboM.TabIndex = 566;
            // 
            // R04
            // 
            this.ClientSize = new System.Drawing.Size(531, 457);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
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
            this.Controls.Add(this.chkSubconOut);
            this.Controls.Add(this.chkExcludeNonRevenue);
            this.Controls.Add(this.chkOnlyCancelOrder);
            this.Controls.Add(this.comboShift);
            this.Controls.Add(this.lbShift);
            this.Controls.Add(this.chkExcludeSampleFty);
            this.Controls.Add(this.chk_Include_Artwork);
            this.Controls.Add(this.chk_Accumulate_output);
            this.Controls.Add(this.txtCDCode);
            this.Controls.Add(this.txtbrand);
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
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R04. Sewing daily output list";
            this.Controls.SetChildIndex(this.labelOoutputDate, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelCDCode, 0);
            this.Controls.SetChildIndex(this.dateOoutputDate, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtCDCode, 0);
            this.Controls.SetChildIndex(this.chk_Accumulate_output, 0);
            this.Controls.SetChildIndex(this.chk_Include_Artwork, 0);
            this.Controls.SetChildIndex(this.chkExcludeSampleFty, 0);
            this.Controls.SetChildIndex(this.lbShift, 0);
            this.Controls.SetChildIndex(this.comboShift, 0);
            this.Controls.SetChildIndex(this.chkOnlyCancelOrder, 0);
            this.Controls.SetChildIndex(this.chkExcludeNonRevenue, 0);
            this.Controls.SetChildIndex(this.chkSubconOut, 0);
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
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
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
        private Class.Txtbrand txtbrand;
        private Class.Txtcdcode txtCDCode;
        private Win.UI.CheckBox chk_Accumulate_output;
        private Win.UI.CheckBox chk_Include_Artwork;
        private Win.UI.CheckBox chkExcludeSampleFty;
        private Win.UI.Label lbShift;
        private Win.UI.ComboBox comboShift;
        private Win.UI.CheckBox chkOnlyCancelOrder;
        private Win.UI.CheckBox chkExcludeNonRevenue;
        private Win.UI.CheckBox chkSubconOut;
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
        private Class.ComboFactory comboFactory;
        private Class.ComboMDivision comboM;
    }
}
