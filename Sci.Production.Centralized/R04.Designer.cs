namespace Sci.Production.Centralized
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
            this.chk_Accumulate_output = new Sci.Win.UI.CheckBox();
            this.chk_Include_Artwork = new Sci.Win.UI.CheckBox();
            this.chkSewingReasonID = new Sci.Win.UI.CheckBox();
            this.chkType = new Sci.Win.UI.CheckBox();
            this.comboFactory = new Sci.Production.Class.ComboCentralizedFactory(this.components);
            this.comboM = new Sci.Production.Class.ComboCentralizedM(this.components);
            this.txtCDCode = new Sci.Production.Class.Txtcdcode();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.comboCategory = new Sci.Production.Class.ComboDropDownList(this.components);
            this.comboShift = new Sci.Win.UI.ComboBox();
            this.lbShift = new Sci.Win.UI.Label();
            this.chkOnlyCancelOrder = new Sci.Win.UI.CheckBox();
            this.chkExcludeNonRevenue = new Sci.Win.UI.CheckBox();
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
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(476, 12);
            this.print.TabIndex = 6;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(476, 48);
            this.toexcel.TabIndex = 7;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(476, 84);
            this.close.TabIndex = 8;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(197, 1);
            // 
            // labelOoutputDate
            // 
            this.labelOoutputDate.Location = new System.Drawing.Point(23, 12);
            this.labelOoutputDate.Name = "labelOoutputDate";
            this.labelOoutputDate.Size = new System.Drawing.Size(98, 23);
            this.labelOoutputDate.TabIndex = 94;
            this.labelOoutputDate.Text = "Output Date";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(23, 48);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(98, 23);
            this.labelCategory.TabIndex = 95;
            this.labelCategory.Text = "Category";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(23, 83);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 96;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(23, 119);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 97;
            this.labelFactory.Text = "Factory";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(23, 155);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(98, 23);
            this.labelBrand.TabIndex = 98;
            this.labelBrand.Text = "Brand";
            // 
            // labelCDCode
            // 
            this.labelCDCode.Location = new System.Drawing.Point(23, 190);
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
            this.dateOoutputDate.Location = new System.Drawing.Point(124, 12);
            this.dateOoutputDate.Name = "dateOoutputDate";
            this.dateOoutputDate.Size = new System.Drawing.Size(280, 23);
            this.dateOoutputDate.TabIndex = 0;
            // 
            // chk_Accumulate_output
            // 
            this.chk_Accumulate_output.AutoSize = true;
            this.chk_Accumulate_output.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chk_Accumulate_output.Location = new System.Drawing.Point(291, 173);
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
            this.chk_Include_Artwork.Location = new System.Drawing.Point(291, 146);
            this.chk_Include_Artwork.Name = "chk_Include_Artwork";
            this.chk_Include_Artwork.Size = new System.Drawing.Size(155, 21);
            this.chk_Include_Artwork.TabIndex = 101;
            this.chk_Include_Artwork.Text = "Include Artwork data";
            this.chk_Include_Artwork.UseVisualStyleBackColor = true;
            // 
            // chkSewingReasonID
            // 
            this.chkSewingReasonID.AutoSize = true;
            this.chkSewingReasonID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSewingReasonID.Location = new System.Drawing.Point(291, 227);
            this.chkSewingReasonID.Name = "chkSewingReasonID";
            this.chkSewingReasonID.Size = new System.Drawing.Size(267, 21);
            this.chkSewingReasonID.TabIndex = 102;
            this.chkSewingReasonID.Text = "Only show sewing reason is not empty";
            this.chkSewingReasonID.UseVisualStyleBackColor = true;
            // 
            // chkType
            // 
            this.chkType.AutoSize = true;
            this.chkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkType.Location = new System.Drawing.Point(291, 119);
            this.chkType.Name = "chkType";
            this.chkType.Size = new System.Drawing.Size(172, 21);
            this.chkType.TabIndex = 227;
            this.chkType.Text = "Exclude sample factory";
            this.chkType.UseVisualStyleBackColor = true;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(124, 119);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(80, 24);
            this.comboFactory.TabIndex = 104;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(124, 83);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(80, 24);
            this.comboM.TabIndex = 103;
            // 
            // txtCDCode
            // 
            this.txtCDCode.BackColor = System.Drawing.Color.White;
            this.txtCDCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCDCode.Location = new System.Drawing.Point(124, 190);
            this.txtCDCode.Name = "txtCDCode";
            this.txtCDCode.Size = new System.Drawing.Size(80, 23);
            this.txtCDCode.TabIndex = 5;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(124, 155);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(80, 23);
            this.txtbrand.TabIndex = 4;
            // 
            // comboCategory
            // 
            this.comboCategory.AddAllItem = false;
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(124, 47);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(160, 24);
            this.comboCategory.TabIndex = 228;
            this.comboCategory.Type = "Pms_GMT_Simple";
            // 
            // comboShift
            // 
            this.comboShift.BackColor = System.Drawing.Color.White;
            this.comboShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShift.FormattingEnabled = true;
            this.comboShift.IsSupportUnselect = true;
            this.comboShift.Location = new System.Drawing.Point(124, 397);
            this.comboShift.Name = "comboShift";
            this.comboShift.OldText = "";
            this.comboShift.Size = new System.Drawing.Size(170, 24);
            this.comboShift.TabIndex = 230;
            // 
            // lbShift
            // 
            this.lbShift.Location = new System.Drawing.Point(23, 397);
            this.lbShift.Name = "lbShift";
            this.lbShift.Size = new System.Drawing.Size(98, 23);
            this.lbShift.TabIndex = 229;
            this.lbShift.Text = "Shift";
            // 
            // chkOnlyCancelOrder
            // 
            this.chkOnlyCancelOrder.AutoSize = true;
            this.chkOnlyCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkOnlyCancelOrder.Location = new System.Drawing.Point(291, 200);
            this.chkOnlyCancelOrder.Name = "chkOnlyCancelOrder";
            this.chkOnlyCancelOrder.Size = new System.Drawing.Size(144, 21);
            this.chkOnlyCancelOrder.TabIndex = 231;
            this.chkOnlyCancelOrder.Text = "Only Cancel Order";
            this.chkOnlyCancelOrder.UseVisualStyleBackColor = true;
            // 
            // chkExcludeNonRevenue
            // 
            this.chkExcludeNonRevenue.AutoSize = true;
            this.chkExcludeNonRevenue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkExcludeNonRevenue.Location = new System.Drawing.Point(291, 254);
            this.chkExcludeNonRevenue.Name = "chkExcludeNonRevenue";
            this.chkExcludeNonRevenue.Size = new System.Drawing.Size(167, 21);
            this.chkExcludeNonRevenue.TabIndex = 232;
            this.chkExcludeNonRevenue.Text = "Exclude Non Revenue";
            this.chkExcludeNonRevenue.UseVisualStyleBackColor = true;
            // 
            // lbConstruction
            // 
            this.lbConstruction.Location = new System.Drawing.Point(23, 331);
            this.lbConstruction.Name = "lbConstruction";
            this.lbConstruction.Size = new System.Drawing.Size(98, 23);
            this.lbConstruction.TabIndex = 237;
            this.lbConstruction.Text = "Construction";
            // 
            // lbGender
            // 
            this.lbGender.Location = new System.Drawing.Point(23, 364);
            this.lbGender.Name = "lbGender";
            this.lbGender.Size = new System.Drawing.Size(98, 23);
            this.lbGender.TabIndex = 236;
            this.lbGender.Text = "Gender";
            // 
            // lbLining
            // 
            this.lbLining.Location = new System.Drawing.Point(23, 296);
            this.lbLining.Name = "lbLining";
            this.lbLining.Size = new System.Drawing.Size(98, 23);
            this.lbLining.TabIndex = 235;
            this.lbLining.Text = "Lining";
            // 
            // lbFabricType
            // 
            this.lbFabricType.Location = new System.Drawing.Point(23, 261);
            this.lbFabricType.Name = "lbFabricType";
            this.lbFabricType.Size = new System.Drawing.Size(98, 23);
            this.lbFabricType.TabIndex = 234;
            this.lbFabricType.Text = "Fabric Type";
            // 
            // lbProductType
            // 
            this.lbProductType.Location = new System.Drawing.Point(23, 227);
            this.lbProductType.Name = "lbProductType";
            this.lbProductType.Size = new System.Drawing.Size(98, 23);
            this.lbProductType.TabIndex = 233;
            this.lbProductType.Text = "Product Type";
            // 
            // comboGender1
            // 
            this.comboGender1.BackColor = System.Drawing.Color.White;
            this.comboGender1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboGender1.FormattingEnabled = true;
            this.comboGender1.IsSupportUnselect = true;
            this.comboGender1.Location = new System.Drawing.Point(124, 363);
            this.comboGender1.Name = "comboGender1";
            this.comboGender1.OldText = "";
            this.comboGender1.Size = new System.Drawing.Size(129, 24);
            this.comboGender1.StyleGender = "";
            this.comboGender1.TabIndex = 242;
            // 
            // comboFabricType1
            // 
            this.comboFabricType1.BackColor = System.Drawing.Color.White;
            this.comboFabricType1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFabricType1.FormattingEnabled = true;
            this.comboFabricType1.IsJunk = false;
            this.comboFabricType1.IsSupportUnselect = true;
            this.comboFabricType1.Location = new System.Drawing.Point(124, 260);
            this.comboFabricType1.Name = "comboFabricType1";
            this.comboFabricType1.OldText = "";
            this.comboFabricType1.Size = new System.Drawing.Size(129, 24);
            this.comboFabricType1.StyleFabricType = "";
            this.comboFabricType1.TabIndex = 241;
            // 
            // comboLining1
            // 
            this.comboLining1.BackColor = System.Drawing.Color.White;
            this.comboLining1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboLining1.FormattingEnabled = true;
            this.comboLining1.IsSupportUnselect = true;
            this.comboLining1.Location = new System.Drawing.Point(124, 295);
            this.comboLining1.Name = "comboLining1";
            this.comboLining1.OldText = "";
            this.comboLining1.Size = new System.Drawing.Size(129, 24);
            this.comboLining1.StyleLining = "";
            this.comboLining1.TabIndex = 240;
            // 
            // comboProductType1
            // 
            this.comboProductType1.BackColor = System.Drawing.Color.White;
            this.comboProductType1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboProductType1.FormattingEnabled = true;
            this.comboProductType1.IsJunk = false;
            this.comboProductType1.IsSupportUnselect = true;
            this.comboProductType1.Location = new System.Drawing.Point(124, 226);
            this.comboProductType1.Name = "comboProductType1";
            this.comboProductType1.OldText = "";
            this.comboProductType1.Size = new System.Drawing.Size(129, 24);
            this.comboProductType1.StyleApparelType = "";
            this.comboProductType1.TabIndex = 239;
            // 
            // comboConstruction1
            // 
            this.comboConstruction1.BackColor = System.Drawing.Color.White;
            this.comboConstruction1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboConstruction1.FormattingEnabled = true;
            this.comboConstruction1.IsSupportUnselect = true;
            this.comboConstruction1.Location = new System.Drawing.Point(124, 330);
            this.comboConstruction1.Name = "comboConstruction1";
            this.comboConstruction1.OldText = "";
            this.comboConstruction1.Size = new System.Drawing.Size(129, 24);
            this.comboConstruction1.StyleGender = "";
            this.comboConstruction1.TabIndex = 238;
            // 
            // R04
            // 
            this.ClientSize = new System.Drawing.Size(568, 533);
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
            this.Controls.Add(this.chkExcludeNonRevenue);
            this.Controls.Add(this.chkOnlyCancelOrder);
            this.Controls.Add(this.comboShift);
            this.Controls.Add(this.lbShift);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.chkType);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.chkSewingReasonID);
            this.Controls.Add(this.chk_Include_Artwork);
            this.Controls.Add(this.chk_Accumulate_output);
            this.Controls.Add(this.txtCDCode);
            this.Controls.Add(this.txtbrand);
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
            this.Text = "R04. Centralized daily output list";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labelOoutputDate, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelCDCode, 0);
            this.Controls.SetChildIndex(this.dateOoutputDate, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtCDCode, 0);
            this.Controls.SetChildIndex(this.chk_Accumulate_output, 0);
            this.Controls.SetChildIndex(this.chk_Include_Artwork, 0);
            this.Controls.SetChildIndex(this.chkSewingReasonID, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.chkType, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.Controls.SetChildIndex(this.lbShift, 0);
            this.Controls.SetChildIndex(this.comboShift, 0);
            this.Controls.SetChildIndex(this.chkOnlyCancelOrder, 0);
            this.Controls.SetChildIndex(this.chkExcludeNonRevenue, 0);
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
        private Class.Txtbrand txtbrand;
        private Class.Txtcdcode txtCDCode;
        private Win.UI.CheckBox chk_Accumulate_output;
        private Win.UI.CheckBox chk_Include_Artwork;
        private Win.UI.CheckBox chkSewingReasonID;
        private Class.ComboCentralizedM comboM;
        private Class.ComboCentralizedFactory comboFactory;
        private Win.UI.CheckBox chkType;
        private Class.ComboDropDownList comboCategory;
        private Win.UI.ComboBox comboShift;
        private Win.UI.Label lbShift;
        private Win.UI.CheckBox chkOnlyCancelOrder;
        private Win.UI.CheckBox chkExcludeNonRevenue;
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
