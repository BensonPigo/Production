﻿namespace Sci.Production.Warehouse
{
    partial class R03
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
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.dateSuppDelivery = new Sci.Win.UI.DateRange();
            this.labelSuppDelivery = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.txtRefnoEnd = new Sci.Win.UI.TextBox();
            this.txtRefnoStart = new Sci.Win.UI.TextBox();
            this.label12 = new Sci.Win.UI.Label();
            this.dateFinalETA = new Sci.Win.UI.DateRange();
            this.labelFinalETA = new Sci.Win.UI.Label();
            this.dateETA = new Sci.Win.UI.DateRange();
            this.labelETA = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelFabricType = new Sci.Win.UI.Label();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.labelM = new Sci.Win.UI.Label();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.comboFabricType = new System.Windows.Forms.ComboBox();
            this.labelCountry = new Sci.Win.UI.Label();
            this.txtcountry = new Sci.Production.Class.Txtcountry();
            this.labelRefno = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.txtsupplier = new Sci.Production.Class.Txtsupplier();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.comboOrderBy = new Sci.Win.UI.ComboBox();
            this.labelOrderBy = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.label1 = new Sci.Win.UI.Label();
            this.txtWKNo2 = new Sci.Win.UI.TextBox();
            this.txtWKNo1 = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.chkWhseClose = new Sci.Win.UI.CheckBox();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.labelBrand = new Sci.Win.UI.Label();
            this.chkIncludeJunk = new Sci.Win.UI.CheckBox();
            this.chkExcludeMaterial = new Sci.Win.UI.CheckBox();
            this.chkSeparateByWK = new Sci.Win.UI.CheckBox();
            this.comboDurable = new Sci.Win.UI.ComboBox();
            this.labelDurable = new Sci.Win.UI.Label();
            this.chkBulk = new Sci.Win.UI.CheckBox();
            this.chkSample = new Sci.Win.UI.CheckBox();
            this.chkMaterial = new Sci.Win.UI.CheckBox();
            this.chkGarment = new Sci.Win.UI.CheckBox();
            this.chkSMTL = new Sci.Win.UI.CheckBox();
            this.label3 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(436, 12);
            this.print.TabIndex = 29;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(436, 48);
            this.toexcel.TabIndex = 30;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(436, 84);
            this.close.TabIndex = 31;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(12, 12);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.RectStyle.BorderWidth = 1F;
            this.labelSCIDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSCIDelivery.RectStyle.ExtBorderWidth = 1F;
            this.labelSCIDelivery.Size = new System.Drawing.Size(98, 23);
            this.labelSCIDelivery.TabIndex = 96;
            this.labelSCIDelivery.Text = "SCI Delivery";
            this.labelSCIDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(115, 12);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 0;
            // 
            // dateSuppDelivery
            // 
            // 
            // 
            // 
            this.dateSuppDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSuppDelivery.DateBox1.Name = "";
            this.dateSuppDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSuppDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSuppDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSuppDelivery.DateBox2.Name = "";
            this.dateSuppDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSuppDelivery.DateBox2.TabIndex = 1;
            this.dateSuppDelivery.IsRequired = false;
            this.dateSuppDelivery.Location = new System.Drawing.Point(115, 41);
            this.dateSuppDelivery.Name = "dateSuppDelivery";
            this.dateSuppDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSuppDelivery.TabIndex = 1;
            // 
            // labelSuppDelivery
            // 
            this.labelSuppDelivery.Location = new System.Drawing.Point(12, 42);
            this.labelSuppDelivery.Name = "labelSuppDelivery";
            this.labelSuppDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSuppDelivery.RectStyle.BorderWidth = 1F;
            this.labelSuppDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSuppDelivery.RectStyle.ExtBorderWidth = 1F;
            this.labelSuppDelivery.Size = new System.Drawing.Size(98, 23);
            this.labelSuppDelivery.TabIndex = 105;
            this.labelSuppDelivery.Text = "Supp Delivery";
            this.labelSuppDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSuppDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(304, 128);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(164, 23);
            this.txtSPNoEnd.TabIndex = 5;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(115, 132);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(164, 23);
            this.txtSPNoStart.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(282, 128);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 23);
            this.label10.TabIndex = 115;
            this.label10.Text = "～";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(12, 132);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(98, 23);
            this.labelSPNo.TabIndex = 114;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtRefnoEnd
            // 
            this.txtRefnoEnd.BackColor = System.Drawing.Color.White;
            this.txtRefnoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefnoEnd.Location = new System.Drawing.Point(304, 163);
            this.txtRefnoEnd.Name = "txtRefnoEnd";
            this.txtRefnoEnd.Size = new System.Drawing.Size(164, 23);
            this.txtRefnoEnd.TabIndex = 7;
            // 
            // txtRefnoStart
            // 
            this.txtRefnoStart.BackColor = System.Drawing.Color.White;
            this.txtRefnoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefnoStart.Location = new System.Drawing.Point(115, 163);
            this.txtRefnoStart.Name = "txtRefnoStart";
            this.txtRefnoStart.Size = new System.Drawing.Size(164, 23);
            this.txtRefnoStart.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(282, 163);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(18, 23);
            this.label12.TabIndex = 123;
            this.label12.Text = "～";
            // 
            // dateFinalETA
            // 
            // 
            // 
            // 
            this.dateFinalETA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateFinalETA.DateBox1.Name = "";
            this.dateFinalETA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateFinalETA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateFinalETA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateFinalETA.DateBox2.Name = "";
            this.dateFinalETA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateFinalETA.DateBox2.TabIndex = 1;
            this.dateFinalETA.IsRequired = false;
            this.dateFinalETA.Location = new System.Drawing.Point(114, 99);
            this.dateFinalETA.Name = "dateFinalETA";
            this.dateFinalETA.Size = new System.Drawing.Size(280, 23);
            this.dateFinalETA.TabIndex = 3;
            // 
            // labelFinalETA
            // 
            this.labelFinalETA.Location = new System.Drawing.Point(12, 102);
            this.labelFinalETA.Name = "labelFinalETA";
            this.labelFinalETA.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelFinalETA.RectStyle.BorderWidth = 1F;
            this.labelFinalETA.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelFinalETA.RectStyle.ExtBorderWidth = 1F;
            this.labelFinalETA.Size = new System.Drawing.Size(98, 23);
            this.labelFinalETA.TabIndex = 128;
            this.labelFinalETA.Text = "Final ETA";
            this.labelFinalETA.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelFinalETA.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateETA
            // 
            // 
            // 
            // 
            this.dateETA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateETA.DateBox1.Name = "";
            this.dateETA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateETA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateETA.DateBox2.Name = "";
            this.dateETA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox2.TabIndex = 1;
            this.dateETA.IsRequired = false;
            this.dateETA.Location = new System.Drawing.Point(115, 72);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(280, 23);
            this.dateETA.TabIndex = 2;
            // 
            // labelETA
            // 
            this.labelETA.Location = new System.Drawing.Point(12, 72);
            this.labelETA.Name = "labelETA";
            this.labelETA.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelETA.RectStyle.BorderWidth = 1F;
            this.labelETA.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelETA.RectStyle.ExtBorderWidth = 1F;
            this.labelETA.Size = new System.Drawing.Size(98, 23);
            this.labelETA.TabIndex = 127;
            this.labelETA.Text = "ETA";
            this.labelETA.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelETA.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(12, 252);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(98, 23);
            this.labelSeason.TabIndex = 95;
            this.labelSeason.Text = "Season";
            // 
            // labelFabricType
            // 
            this.labelFabricType.Location = new System.Drawing.Point(12, 462);
            this.labelFabricType.Name = "labelFabricType";
            this.labelFabricType.Size = new System.Drawing.Size(98, 23);
            this.labelFabricType.TabIndex = 98;
            this.labelFabricType.Text = "Material Type";
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.DefaultValue = false;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(115, 372);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.NeedInitialMdivision = false;
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 15;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(12, 372);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 103;
            this.labelM.Text = "M";
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(115, 252);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(80, 23);
            this.txtseason.TabIndex = 11;
            // 
            // comboFabricType
            // 
            this.comboFabricType.FormattingEnabled = true;
            this.comboFabricType.Location = new System.Drawing.Point(115, 461);
            this.comboFabricType.Name = "comboFabricType";
            this.comboFabricType.Size = new System.Drawing.Size(121, 24);
            this.comboFabricType.TabIndex = 22;
            // 
            // labelCountry
            // 
            this.labelCountry.Location = new System.Drawing.Point(12, 312);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(98, 23);
            this.labelCountry.TabIndex = 113;
            this.labelCountry.Text = "Country";
            // 
            // txtcountry
            // 
            this.txtcountry.DisplayBox1Binding = "";
            this.txtcountry.Location = new System.Drawing.Point(115, 312);
            this.txtcountry.Name = "txtcountry";
            this.txtcountry.Size = new System.Drawing.Size(232, 30);
            this.txtcountry.TabIndex = 13;
            this.txtcountry.TextBox1Binding = "";
            // 
            // labelRefno
            // 
            this.labelRefno.Location = new System.Drawing.Point(12, 162);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelRefno.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelRefno.Size = new System.Drawing.Size(98, 23);
            this.labelRefno.TabIndex = 130;
            this.labelRefno.Text = "Refno";
            this.labelRefno.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelRefno.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(12, 222);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(98, 23);
            this.labelStyle.TabIndex = 132;
            this.labelStyle.Text = "Style";
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(115, 222);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.SeasonObjectName = null;
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 10;
            this.txtstyle.TarBrand = null;
            this.txtstyle.TarSeason = null;
            // 
            // txtsupplier
            // 
            this.txtsupplier.DisplayBox1Binding = "";
            this.txtsupplier.Location = new System.Drawing.Point(115, 342);
            this.txtsupplier.Name = "txtsupplier";
            this.txtsupplier.Size = new System.Drawing.Size(147, 23);
            this.txtsupplier.TabIndex = 14;
            this.txtsupplier.TextBox1Binding = "";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(12, 342);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(98, 23);
            this.labelSupplier.TabIndex = 135;
            this.labelSupplier.Text = "Supplier";
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
            this.comboOrderBy.Location = new System.Drawing.Point(114, 492);
            this.comboOrderBy.Name = "comboOrderBy";
            this.comboOrderBy.OldText = "";
            this.comboOrderBy.Size = new System.Drawing.Size(121, 24);
            this.comboOrderBy.TabIndex = 23;
            // 
            // labelOrderBy
            // 
            this.labelOrderBy.Location = new System.Drawing.Point(12, 492);
            this.labelOrderBy.Name = "labelOrderBy";
            this.labelOrderBy.Size = new System.Drawing.Size(98, 23);
            this.labelOrderBy.TabIndex = 137;
            this.labelOrderBy.Text = "Order By";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(12, 432);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 138;
            this.labelFactory.Text = "Factory";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsIE = false;
            this.txtfactory.IsMultiselect = false;
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(115, 432);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.NeedInitialFactory = false;
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 192);
            this.label1.Name = "label1";
            this.label1.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label1.Size = new System.Drawing.Size(98, 23);
            this.label1.TabIndex = 144;
            this.label1.Text = "WK#";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtWKNo2
            // 
            this.txtWKNo2.BackColor = System.Drawing.Color.White;
            this.txtWKNo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWKNo2.Location = new System.Drawing.Point(303, 192);
            this.txtWKNo2.Name = "txtWKNo2";
            this.txtWKNo2.Size = new System.Drawing.Size(164, 23);
            this.txtWKNo2.TabIndex = 9;
            // 
            // txtWKNo1
            // 
            this.txtWKNo1.BackColor = System.Drawing.Color.White;
            this.txtWKNo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWKNo1.Location = new System.Drawing.Point(114, 192);
            this.txtWKNo1.Name = "txtWKNo1";
            this.txtWKNo1.Size = new System.Drawing.Size(164, 23);
            this.txtWKNo1.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(281, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 23);
            this.label2.TabIndex = 143;
            this.label2.Text = "～";
            // 
            // chkWhseClose
            // 
            this.chkWhseClose.AutoSize = true;
            this.chkWhseClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkWhseClose.Location = new System.Drawing.Point(13, 556);
            this.chkWhseClose.Name = "chkWhseClose";
            this.chkWhseClose.Size = new System.Drawing.Size(142, 21);
            this.chkWhseClose.TabIndex = 25;
            this.chkWhseClose.Text = "exclude closed SP";
            this.chkWhseClose.UseVisualStyleBackColor = true;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(115, 283);
            this.txtbrand.MyDocumentdName = null;
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(98, 23);
            this.txtbrand.TabIndex = 12;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(12, 282);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(98, 23);
            this.labelBrand.TabIndex = 147;
            this.labelBrand.Text = "Brand";
            // 
            // chkIncludeJunk
            // 
            this.chkIncludeJunk.AutoSize = true;
            this.chkIncludeJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeJunk.Location = new System.Drawing.Point(13, 583);
            this.chkIncludeJunk.Name = "chkIncludeJunk";
            this.chkIncludeJunk.Size = new System.Drawing.Size(160, 21);
            this.chkIncludeJunk.TabIndex = 26;
            this.chkIncludeJunk.Text = "Include Junk Material";
            this.chkIncludeJunk.UseVisualStyleBackColor = true;
            // 
            // chkExcludeMaterial
            // 
            this.chkExcludeMaterial.AutoSize = true;
            this.chkExcludeMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkExcludeMaterial.Location = new System.Drawing.Point(13, 610);
            this.chkExcludeMaterial.Name = "chkExcludeMaterial";
            this.chkExcludeMaterial.Size = new System.Drawing.Size(130, 21);
            this.chkExcludeMaterial.TabIndex = 27;
            this.chkExcludeMaterial.Text = "Exclude Material";
            this.chkExcludeMaterial.UseVisualStyleBackColor = true;
            // 
            // chkSeparateByWK
            // 
            this.chkSeparateByWK.AutoSize = true;
            this.chkSeparateByWK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSeparateByWK.Location = new System.Drawing.Point(12, 637);
            this.chkSeparateByWK.Name = "chkSeparateByWK";
            this.chkSeparateByWK.Size = new System.Drawing.Size(138, 21);
            this.chkSeparateByWK.TabIndex = 28;
            this.chkSeparateByWK.Text = "Separate by WK#";
            this.chkSeparateByWK.UseVisualStyleBackColor = true;
            // 
            // comboDurable
            // 
            this.comboDurable.BackColor = System.Drawing.Color.White;
            this.comboDurable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDurable.FormattingEnabled = true;
            this.comboDurable.IsSupportUnselect = true;
            this.comboDurable.Items.AddRange(new object[] {
            "Issue Date",
            "Supplier"});
            this.comboDurable.Location = new System.Drawing.Point(178, 521);
            this.comboDurable.Name = "comboDurable";
            this.comboDurable.OldText = "";
            this.comboDurable.Size = new System.Drawing.Size(100, 24);
            this.comboDurable.TabIndex = 24;
            // 
            // labelDurable
            // 
            this.labelDurable.Location = new System.Drawing.Point(12, 522);
            this.labelDurable.Name = "labelDurable";
            this.labelDurable.Size = new System.Drawing.Size(160, 23);
            this.labelDurable.TabIndex = 149;
            this.labelDurable.Text = "Durable Water Repellent";
            // 
            // chkBulk
            // 
            this.chkBulk.AutoSize = true;
            this.chkBulk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkBulk.Location = new System.Drawing.Point(115, 402);
            this.chkBulk.Name = "chkBulk";
            this.chkBulk.Size = new System.Drawing.Size(54, 21);
            this.chkBulk.TabIndex = 16;
            this.chkBulk.Text = "Bulk";
            this.chkBulk.UseVisualStyleBackColor = true;
            // 
            // chkSample
            // 
            this.chkSample.AutoSize = true;
            this.chkSample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSample.Location = new System.Drawing.Point(175, 402);
            this.chkSample.Name = "chkSample";
            this.chkSample.Size = new System.Drawing.Size(74, 21);
            this.chkSample.TabIndex = 17;
            this.chkSample.Text = "Sample";
            this.chkSample.UseVisualStyleBackColor = true;
            // 
            // chkMaterial
            // 
            this.chkMaterial.AutoSize = true;
            this.chkMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkMaterial.Location = new System.Drawing.Point(255, 402);
            this.chkMaterial.Name = "chkMaterial";
            this.chkMaterial.Size = new System.Drawing.Size(77, 21);
            this.chkMaterial.TabIndex = 18;
            this.chkMaterial.Text = "Material";
            this.chkMaterial.UseVisualStyleBackColor = true;
            // 
            // chkGarment
            // 
            this.chkGarment.AutoSize = true;
            this.chkGarment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkGarment.Location = new System.Drawing.Point(408, 402);
            this.chkGarment.Name = "chkGarment";
            this.chkGarment.Size = new System.Drawing.Size(82, 21);
            this.chkGarment.TabIndex = 20;
            this.chkGarment.Text = "Garment";
            this.chkGarment.UseVisualStyleBackColor = true;
            // 
            // chkSMTL
            // 
            this.chkSMTL.AutoSize = true;
            this.chkSMTL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSMTL.Location = new System.Drawing.Point(338, 402);
            this.chkSMTL.Name = "chkSMTL";
            this.chkSMTL.Size = new System.Drawing.Size(64, 21);
            this.chkSMTL.TabIndex = 19;
            this.chkSMTL.Text = "SMTL";
            this.chkSMTL.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 402);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 23);
            this.label3.TabIndex = 155;
            this.label3.Text = "Category";
            // 
            // R03
            // 
            this.ClientSize = new System.Drawing.Size(528, 691);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkSMTL);
            this.Controls.Add(this.chkGarment);
            this.Controls.Add(this.chkMaterial);
            this.Controls.Add(this.chkSample);
            this.Controls.Add(this.chkBulk);
            this.Controls.Add(this.comboDurable);
            this.Controls.Add(this.labelDurable);
            this.Controls.Add(this.chkSeparateByWK);
            this.Controls.Add(this.chkExcludeMaterial);
            this.Controls.Add(this.chkIncludeJunk);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.chkWhseClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtWKNo2);
            this.Controls.Add(this.txtWKNo1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.comboOrderBy);
            this.Controls.Add(this.labelOrderBy);
            this.Controls.Add(this.labelSupplier);
            this.Controls.Add(this.txtsupplier);
            this.Controls.Add(this.txtstyle);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.labelRefno);
            this.Controls.Add(this.txtcountry);
            this.Controls.Add(this.dateFinalETA);
            this.Controls.Add(this.labelFinalETA);
            this.Controls.Add(this.dateETA);
            this.Controls.Add(this.labelETA);
            this.Controls.Add(this.comboFabricType);
            this.Controls.Add(this.txtRefnoEnd);
            this.Controls.Add(this.txtRefnoStart);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtseason);
            this.Controls.Add(this.txtSPNoEnd);
            this.Controls.Add(this.txtSPNoStart);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.labelCountry);
            this.Controls.Add(this.dateSuppDelivery);
            this.Controls.Add(this.labelSuppDelivery);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.labelFabricType);
            this.Controls.Add(this.labelSCIDelivery);
            this.Controls.Add(this.labelSeason);
            this.IsSupportToPrint = false;
            this.Name = "R03";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R03. Material Delivery Report";
            this.Controls.SetChildIndex(this.labelSeason, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.labelFabricType, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelSuppDelivery, 0);
            this.Controls.SetChildIndex(this.dateSuppDelivery, 0);
            this.Controls.SetChildIndex(this.labelCountry, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.txtSPNoStart, 0);
            this.Controls.SetChildIndex(this.txtSPNoEnd, 0);
            this.Controls.SetChildIndex(this.txtseason, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.txtRefnoStart, 0);
            this.Controls.SetChildIndex(this.txtRefnoEnd, 0);
            this.Controls.SetChildIndex(this.comboFabricType, 0);
            this.Controls.SetChildIndex(this.labelETA, 0);
            this.Controls.SetChildIndex(this.dateETA, 0);
            this.Controls.SetChildIndex(this.labelFinalETA, 0);
            this.Controls.SetChildIndex(this.dateFinalETA, 0);
            this.Controls.SetChildIndex(this.txtcountry, 0);
            this.Controls.SetChildIndex(this.labelRefno, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.txtstyle, 0);
            this.Controls.SetChildIndex(this.txtsupplier, 0);
            this.Controls.SetChildIndex(this.labelSupplier, 0);
            this.Controls.SetChildIndex(this.labelOrderBy, 0);
            this.Controls.SetChildIndex(this.comboOrderBy, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtWKNo1, 0);
            this.Controls.SetChildIndex(this.txtWKNo2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.chkWhseClose, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.chkIncludeJunk, 0);
            this.Controls.SetChildIndex(this.chkExcludeMaterial, 0);
            this.Controls.SetChildIndex(this.chkSeparateByWK, 0);
            this.Controls.SetChildIndex(this.labelDurable, 0);
            this.Controls.SetChildIndex(this.comboDurable, 0);
            this.Controls.SetChildIndex(this.chkBulk, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.chkSample, 0);
            this.Controls.SetChildIndex(this.chkMaterial, 0);
            this.Controls.SetChildIndex(this.chkGarment, 0);
            this.Controls.SetChildIndex(this.chkSMTL, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSCIDelivery;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.DateRange dateSuppDelivery;
        private Win.UI.Label labelSuppDelivery;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.Label label10;
        private Win.UI.Label labelSPNo;
        private Win.UI.TextBox txtRefnoEnd;
        private Win.UI.TextBox txtRefnoStart;
        private Win.UI.Label label12;
        private Win.UI.DateRange dateFinalETA;
        private Win.UI.Label labelFinalETA;
        private Win.UI.DateRange dateETA;
        private Win.UI.Label labelETA;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelFabricType;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.Label labelM;
        private Class.Txtseason txtseason;
        private System.Windows.Forms.ComboBox comboFabricType;
        private Win.UI.Label labelCountry;
        private Class.Txtcountry txtcountry;
        private Win.UI.Label labelRefno;
        private Win.UI.Label labelStyle;
        private Class.Txtstyle txtstyle;
        private Class.Txtsupplier txtsupplier;
        private Win.UI.Label labelSupplier;
        private Win.UI.ComboBox comboOrderBy;
        private Win.UI.Label labelOrderBy;
        private Win.UI.Label labelFactory;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtWKNo2;
        private Win.UI.TextBox txtWKNo1;
        private Win.UI.Label label2;
        private Win.UI.CheckBox chkWhseClose;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label labelBrand;
        private Win.UI.CheckBox chkIncludeJunk;
        private Win.UI.CheckBox chkExcludeMaterial;
        private Win.UI.CheckBox chkSeparateByWK;
		private Win.UI.ComboBox comboDurable;
		private Win.UI.Label labelDurable;
        private Win.UI.CheckBox chkBulk;
        private Win.UI.CheckBox chkSample;
        private Win.UI.CheckBox chkMaterial;
        private Win.UI.CheckBox chkGarment;
        private Win.UI.CheckBox chkSMTL;
        private Win.UI.Label label3;
    }
}
