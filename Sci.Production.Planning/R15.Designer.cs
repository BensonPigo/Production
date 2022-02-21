﻿namespace Sci.Production.Planning
{
    partial class R15
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
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.dateCustRQSDate = new Sci.Win.UI.DateRange();
            this.labelCustRQSDate = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.dateCutOffDate = new Sci.Win.UI.DateRange();
            this.labelCutOffDate = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.checkIncludeArtowkData = new Sci.Win.UI.CheckBox();
            this.datePlanDate = new Sci.Win.UI.DateRange();
            this.labelPlanDate = new Sci.Win.UI.Label();
            this.labelOrderBy = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelCustCD = new Sci.Win.UI.Label();
            this.comboOrderBy = new Sci.Win.UI.ComboBox();
            this.label1 = new Sci.Win.UI.Label();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.lbSewingInline = new Sci.Win.UI.Label();
            this.dateSewingInline = new Sci.Win.UI.DateRange();
            this.label2 = new Sci.Win.UI.Label();
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.dateLastSewDate = new Sci.Win.UI.DateRange();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.txtsubprocess1 = new Sci.Production.Class.Txtsubprocess();
            this.txtStyle = new Sci.Production.Class.Txtstyle();
            this.comboCategory = new Sci.Production.Class.ComboDropDownList(this.components);
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtCustCD = new Sci.Production.Class.Txtcustcd();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.comboRFIDProcessLocation1 = new Sci.Production.Class.ComboRFIDProcessLocation();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(460, 12);
            this.print.TabIndex = 17;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(460, 48);
            this.toexcel.TabIndex = 18;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(460, 84);
            this.close.TabIndex = 19;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(414, 120);
            this.buttonCustomized.TabIndex = 20;
            this.buttonCustomized.Visible = true;
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(440, 156);
            this.checkUseCustomized.TabIndex = 21;
            this.checkUseCustomized.Visible = true;
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(440, 183);
            this.txtVersion.TabIndex = 22;
            this.txtVersion.Visible = true;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(13, 12);
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
            this.dateSCIDelivery.Location = new System.Drawing.Point(114, 12);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 0;
            // 
            // dateCustRQSDate
            // 
            // 
            // 
            // 
            this.dateCustRQSDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCustRQSDate.DateBox1.Name = "";
            this.dateCustRQSDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCustRQSDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCustRQSDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCustRQSDate.DateBox2.Name = "";
            this.dateCustRQSDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCustRQSDate.DateBox2.TabIndex = 1;
            this.dateCustRQSDate.IsRequired = false;
            this.dateCustRQSDate.Location = new System.Drawing.Point(114, 152);
            this.dateCustRQSDate.Name = "dateCustRQSDate";
            this.dateCustRQSDate.Size = new System.Drawing.Size(280, 23);
            this.dateCustRQSDate.TabIndex = 4;
            // 
            // labelCustRQSDate
            // 
            this.labelCustRQSDate.Location = new System.Drawing.Point(13, 152);
            this.labelCustRQSDate.Name = "labelCustRQSDate";
            this.labelCustRQSDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelCustRQSDate.RectStyle.BorderWidth = 1F;
            this.labelCustRQSDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelCustRQSDate.RectStyle.ExtBorderWidth = 1F;
            this.labelCustRQSDate.Size = new System.Drawing.Size(98, 23);
            this.labelCustRQSDate.TabIndex = 105;
            this.labelCustRQSDate.Text = "Cust RQS Date";
            this.labelCustRQSDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelCustRQSDate.TextStyle.Color = System.Drawing.Color.Black;
            this.tooltip.SetToolTip(this.labelCustRQSDate, "CRDDate");
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(267, 222);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(128, 23);
            this.txtSPNoEnd.TabIndex = 7;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(114, 222);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(128, 23);
            this.txtSPNoStart.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(245, 222);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 23);
            this.label10.TabIndex = 115;
            this.label10.Text = "～";
            this.label10.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(13, 222);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(98, 23);
            this.labelSPNo.TabIndex = 114;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateCutOffDate
            // 
            // 
            // 
            // 
            this.dateCutOffDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCutOffDate.DateBox1.Name = "";
            this.dateCutOffDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCutOffDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCutOffDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCutOffDate.DateBox2.Name = "";
            this.dateCutOffDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCutOffDate.DateBox2.TabIndex = 1;
            this.dateCutOffDate.IsRequired = false;
            this.dateCutOffDate.Location = new System.Drawing.Point(114, 117);
            this.dateCutOffDate.Name = "dateCutOffDate";
            this.dateCutOffDate.Size = new System.Drawing.Size(280, 23);
            this.dateCutOffDate.TabIndex = 3;
            // 
            // labelCutOffDate
            // 
            this.labelCutOffDate.Location = new System.Drawing.Point(13, 117);
            this.labelCutOffDate.Name = "labelCutOffDate";
            this.labelCutOffDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelCutOffDate.RectStyle.BorderWidth = 1F;
            this.labelCutOffDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelCutOffDate.RectStyle.ExtBorderWidth = 1F;
            this.labelCutOffDate.Size = new System.Drawing.Size(98, 23);
            this.labelCutOffDate.TabIndex = 128;
            this.labelCutOffDate.Text = "Cut Off Date";
            this.labelCutOffDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelCutOffDate.TextStyle.Color = System.Drawing.Color.Black;
            this.tooltip.SetToolTip(this.labelCutOffDate, "SDPDate");
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(114, 47);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 1;
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(13, 47);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelBuyerDelivery.RectStyle.BorderWidth = 1F;
            this.labelBuyerDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelBuyerDelivery.RectStyle.ExtBorderWidth = 1F;
            this.labelBuyerDelivery.Size = new System.Drawing.Size(98, 23);
            this.labelBuyerDelivery.TabIndex = 127;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            this.labelBuyerDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelBuyerDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 397);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 103;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 432);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 132;
            this.labelFactory.Text = "Factory";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(13, 467);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(98, 23);
            this.labelCategory.TabIndex = 141;
            this.labelCategory.Text = "Category";
            // 
            // checkIncludeArtowkData
            // 
            this.checkIncludeArtowkData.AutoSize = true;
            this.checkIncludeArtowkData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIncludeArtowkData.Location = new System.Drawing.Point(13, 576);
            this.checkIncludeArtowkData.Name = "checkIncludeArtowkData";
            this.checkIncludeArtowkData.Size = new System.Drawing.Size(157, 21);
            this.checkIncludeArtowkData.TabIndex = 16;
            this.checkIncludeArtowkData.Text = "Include Artwork Data";
            this.checkIncludeArtowkData.UseVisualStyleBackColor = true;
            // 
            // datePlanDate
            // 
            // 
            // 
            // 
            this.datePlanDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.datePlanDate.DateBox1.Name = "";
            this.datePlanDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.datePlanDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.datePlanDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.datePlanDate.DateBox2.Name = "";
            this.datePlanDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.datePlanDate.DateBox2.TabIndex = 1;
            this.datePlanDate.IsRequired = false;
            this.datePlanDate.Location = new System.Drawing.Point(114, 187);
            this.datePlanDate.Name = "datePlanDate";
            this.datePlanDate.Size = new System.Drawing.Size(280, 23);
            this.datePlanDate.TabIndex = 5;
            // 
            // labelPlanDate
            // 
            this.labelPlanDate.Location = new System.Drawing.Point(13, 187);
            this.labelPlanDate.Name = "labelPlanDate";
            this.labelPlanDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelPlanDate.RectStyle.BorderWidth = 1F;
            this.labelPlanDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelPlanDate.RectStyle.ExtBorderWidth = 1F;
            this.labelPlanDate.Size = new System.Drawing.Size(98, 23);
            this.labelPlanDate.TabIndex = 145;
            this.labelPlanDate.Text = "Plan Date";
            this.labelPlanDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelPlanDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelOrderBy
            // 
            this.labelOrderBy.Location = new System.Drawing.Point(13, 502);
            this.labelOrderBy.Name = "labelOrderBy";
            this.labelOrderBy.Size = new System.Drawing.Size(98, 23);
            this.labelOrderBy.TabIndex = 147;
            this.labelOrderBy.Text = "Order By";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 327);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(98, 23);
            this.labelBrand.TabIndex = 150;
            this.labelBrand.Text = "Brand";
            // 
            // labelCustCD
            // 
            this.labelCustCD.Location = new System.Drawing.Point(13, 362);
            this.labelCustCD.Name = "labelCustCD";
            this.labelCustCD.Size = new System.Drawing.Size(98, 23);
            this.labelCustCD.TabIndex = 151;
            this.labelCustCD.Text = "Cust CD";
            // 
            // comboOrderBy
            // 
            this.comboOrderBy.BackColor = System.Drawing.Color.White;
            this.comboOrderBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboOrderBy.FormattingEnabled = true;
            this.comboOrderBy.IsSupportUnselect = true;
            this.comboOrderBy.Location = new System.Drawing.Point(114, 502);
            this.comboOrderBy.Name = "comboOrderBy";
            this.comboOrderBy.OldText = "";
            this.comboOrderBy.Size = new System.Drawing.Size(121, 24);
            this.comboOrderBy.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 537);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 23);
            this.label1.TabIndex = 153;
            this.label1.Text = "SummaryBy";
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(114, 536);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.OldText = "";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 15;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // lbSewingInline
            // 
            this.lbSewingInline.Location = new System.Drawing.Point(13, 82);
            this.lbSewingInline.Name = "lbSewingInline";
            this.lbSewingInline.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lbSewingInline.RectStyle.BorderWidth = 1F;
            this.lbSewingInline.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbSewingInline.RectStyle.ExtBorderWidth = 1F;
            this.lbSewingInline.Size = new System.Drawing.Size(98, 23);
            this.lbSewingInline.TabIndex = 155;
            this.lbSewingInline.Text = "Sewing Inline";
            this.lbSewingInline.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lbSewingInline.TextStyle.Color = System.Drawing.Color.Black;
            this.tooltip.SetToolTip(this.lbSewingInline, "SDPDate");
            // 
            // dateSewingInline
            // 
            // 
            // 
            // 
            this.dateSewingInline.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSewingInline.DateBox1.Name = "";
            this.dateSewingInline.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSewingInline.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSewingInline.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSewingInline.DateBox2.Name = "";
            this.dateSewingInline.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSewingInline.DateBox2.TabIndex = 1;
            this.dateSewingInline.IsRequired = false;
            this.dateSewingInline.Location = new System.Drawing.Point(114, 82);
            this.dateSewingInline.Name = "dateSewingInline";
            this.dateSewingInline.Size = new System.Drawing.Size(280, 23);
            this.dateSewingInline.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 292);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 23);
            this.label2.TabIndex = 157;
            this.label2.Text = "Style";
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(13, 595);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 158;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // dateLastSewDate
            // 
            // 
            // 
            // 
            this.dateLastSewDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateLastSewDate.DateBox1.Name = "";
            this.dateLastSewDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateLastSewDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateLastSewDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateLastSewDate.DateBox2.Name = "";
            this.dateLastSewDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateLastSewDate.DateBox2.TabIndex = 1;
            this.dateLastSewDate.IsSupportEditMode = false;
            this.dateLastSewDate.Location = new System.Drawing.Point(114, 257);
            this.dateLastSewDate.Name = "dateLastSewDate";
            this.dateLastSewDate.Size = new System.Drawing.Size(280, 23);
            this.dateLastSewDate.TabIndex = 160;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 257);
            this.label3.Name = "label3";
            this.label3.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label3.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label3.Size = new System.Drawing.Size(98, 23);
            this.label3.TabIndex = 161;
            this.label3.Text = "Last Sew. Date";
            this.label3.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label4.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label4.Size = new System.Drawing.Size(109, 23);
            this.label4.TabIndex = 162;
            this.label4.Text = "Subprocess";
            this.label4.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(0, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 23);
            this.label5.TabIndex = 163;
            this.label5.Text = "Process Location";
            // 
            // txtsubprocess1
            // 
            this.txtsubprocess1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtsubprocess1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtsubprocess1.IsSupportEditMode = false;
            this.txtsubprocess1.Location = new System.Drawing.Point(112, 0);
            this.txtsubprocess1.MultiSelect = true;
            this.txtsubprocess1.Name = "txtsubprocess1";
            this.txtsubprocess1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtsubprocess1.ReadOnly = true;
            this.txtsubprocess1.Size = new System.Drawing.Size(146, 23);
            this.txtsubprocess1.TabIndex = 164;
            // 
            // txtStyle
            // 
            this.txtStyle.BackColor = System.Drawing.Color.White;
            this.txtStyle.BrandObjectName = null;
            this.txtStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStyle.Location = new System.Drawing.Point(115, 292);
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.Size = new System.Drawing.Size(186, 23);
            this.txtStyle.TabIndex = 8;
            this.txtStyle.TarBrand = null;
            this.txtStyle.TarSeason = null;
            // 
            // comboCategory
            // 
            this.comboCategory.AddAllItem = false;
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(114, 467);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(280, 24);
            this.comboCategory.TabIndex = 13;
            this.comboCategory.Type = "Pms_ReportForProduct";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(115, 327);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(122, 23);
            this.txtbrand.TabIndex = 9;
            // 
            // txtCustCD
            // 
            this.txtCustCD.BackColor = System.Drawing.Color.White;
            this.txtCustCD.BrandObjectName = null;
            this.txtCustCD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCustCD.Location = new System.Drawing.Point(114, 362);
            this.txtCustCD.Name = "txtCustCD";
            this.txtCustCD.Size = new System.Drawing.Size(125, 23);
            this.txtCustCD.TabIndex = 10;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(114, 432);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 12;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(115, 397);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 11;
            // 
            // comboRFIDProcessLocation1
            // 
            this.comboRFIDProcessLocation1.BackColor = System.Drawing.Color.White;
            this.comboRFIDProcessLocation1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboRFIDProcessLocation1.FormattingEnabled = true;
            this.comboRFIDProcessLocation1.IncludeJunk = false;
            this.comboRFIDProcessLocation1.IsSupportUnselect = true;
            this.comboRFIDProcessLocation1.Location = new System.Drawing.Point(112, 34);
            this.comboRFIDProcessLocation1.Name = "comboRFIDProcessLocation1";
            this.comboRFIDProcessLocation1.OldText = "";
            this.comboRFIDProcessLocation1.Size = new System.Drawing.Size(121, 24);
            this.comboRFIDProcessLocation1.TabIndex = 165;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.comboRFIDProcessLocation1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtsubprocess1);
            this.panel1.Location = new System.Drawing.Point(245, 502);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(268, 63);
            this.panel1.TabIndex = 166;
            // 
            // R15
            // 
            this.ClientSize = new System.Drawing.Size(552, 651);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateLastSewDate);
            this.Controls.Add(this.chkIncludeCancelOrder);
            this.Controls.Add(this.txtStyle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateSewingInline);
            this.Controls.Add(this.lbSewingInline);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.comboOrderBy);
            this.Controls.Add(this.labelCustCD);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.txtCustCD);
            this.Controls.Add(this.labelOrderBy);
            this.Controls.Add(this.datePlanDate);
            this.Controls.Add(this.labelPlanDate);
            this.Controls.Add(this.checkIncludeArtowkData);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.dateCutOffDate);
            this.Controls.Add(this.labelCutOffDate);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.labelBuyerDelivery);
            this.Controls.Add(this.txtSPNoEnd);
            this.Controls.Add(this.txtSPNoStart);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.dateCustRQSDate);
            this.Controls.Add(this.labelCustRQSDate);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.labelSCIDelivery);
            this.DefaultControl = "dateBuyerDelivery";
            this.DefaultControlForEdit = "dateBuyerDelivery";
            this.IsSupportCustomized = true;
            this.IsSupportToPrint = false;
            this.Name = "R15";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R15. WIP";
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelCustRQSDate, 0);
            this.Controls.SetChildIndex(this.dateCustRQSDate, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.txtSPNoStart, 0);
            this.Controls.SetChildIndex(this.txtSPNoEnd, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labelCutOffDate, 0);
            this.Controls.SetChildIndex(this.dateCutOffDate, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.checkIncludeArtowkData, 0);
            this.Controls.SetChildIndex(this.labelPlanDate, 0);
            this.Controls.SetChildIndex(this.datePlanDate, 0);
            this.Controls.SetChildIndex(this.labelOrderBy, 0);
            this.Controls.SetChildIndex(this.txtCustCD, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelCustCD, 0);
            this.Controls.SetChildIndex(this.comboOrderBy, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            this.Controls.SetChildIndex(this.lbSewingInline, 0);
            this.Controls.SetChildIndex(this.dateSewingInline, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.txtStyle, 0);
            this.Controls.SetChildIndex(this.chkIncludeCancelOrder, 0);
            this.Controls.SetChildIndex(this.dateLastSewDate, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSCIDelivery;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.DateRange dateCustRQSDate;
        private Win.UI.Label labelCustRQSDate;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.Label label10;
        private Win.UI.Label labelSPNo;
        private Win.UI.DateRange dateCutOffDate;
        private Win.UI.Label labelCutOffDate;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Label labelBuyerDelivery;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label labelCategory;
        private Win.UI.CheckBox checkIncludeArtowkData;
        private Win.UI.DateRange datePlanDate;
        private Win.UI.Label labelPlanDate;
        private Win.UI.Label labelOrderBy;
        private Class.Txtcustcd txtCustCD;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelCustCD;
        private Win.UI.ComboBox comboOrderBy;
        private Class.ComboDropDownList comboCategory;
        private Win.UI.Label label1;
        private Win.UI.ComboBox comboBox1;
        private Win.UI.Label lbSewingInline;
        private Win.UI.DateRange dateSewingInline;
        private Win.UI.Label label2;
        private Class.Txtstyle txtStyle;
        private Win.UI.CheckBox chkIncludeCancelOrder;
        private Win.UI.DateRange dateLastSewDate;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Class.Txtsubprocess txtsubprocess1;
        private Class.ComboRFIDProcessLocation comboRFIDProcessLocation1;
        private Win.UI.Panel panel1;
    }
}
