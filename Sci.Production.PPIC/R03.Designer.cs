namespace Sci.Production.PPIC
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
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.labelCutOffDate = new Sci.Win.UI.Label();
            this.labelCustRQSDate = new Sci.Win.UI.Label();
            this.labelPlanDate = new Sci.Win.UI.Label();
            this.labelOrderCfmDate = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelCustCD = new Sci.Win.UI.Label();
            this.labelZone = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelSubProcess = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.dateCutOffDate = new Sci.Win.UI.DateRange();
            this.dateCustRQSDate = new Sci.Win.UI.DateRange();
            this.datePlanDate = new Sci.Win.UI.DateRange();
            this.dateOrderCfmDate = new Sci.Win.UI.DateRange();
            this.comboZone = new Sci.Win.UI.ComboBox();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.comboSubProcess = new Sci.Win.UI.ComboBox();
            this.checkIncludeHistoryOrder = new Sci.Win.UI.CheckBox();
            this.checkIncludeArtworkdata = new Sci.Win.UI.CheckBox();
            this.checkIncludeArtworkdataKindIsPAP = new Sci.Win.UI.CheckBox();
            this.checkQtyBDownByShipmode = new Sci.Win.UI.CheckBox();
            this.checkListPOCombo = new Sci.Win.UI.CheckBox();
            this.checkBulk = new Sci.Win.UI.CheckBox();
            this.checkSample = new Sci.Win.UI.CheckBox();
            this.checkMaterial = new Sci.Win.UI.CheckBox();
            this.checkForecast = new Sci.Win.UI.CheckBox();
            this.txtstyle = new Sci.Production.Class.txtstyle();
            this.txtseason = new Sci.Production.Class.txtseason();
            this.txtcustcd = new Sci.Production.Class.txtcustcd();
            this.txtbrand = new Sci.Production.Class.txtbrand();
            this.checkGarment = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(428, 12);
            this.print.TabIndex = 24;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(428, 48);
            this.toexcel.TabIndex = 25;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(428, 84);
            this.close.TabIndex = 26;
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(13, 11);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(102, 23);
            this.labelBuyerDelivery.TabIndex = 94;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(13, 39);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(102, 23);
            this.labelSCIDelivery.TabIndex = 95;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // labelCutOffDate
            // 
            this.labelCutOffDate.Location = new System.Drawing.Point(13, 67);
            this.labelCutOffDate.Name = "labelCutOffDate";
            this.labelCutOffDate.Size = new System.Drawing.Size(102, 23);
            this.labelCutOffDate.TabIndex = 96;
            this.labelCutOffDate.Text = "Cut-Off Date";
            // 
            // labelCustRQSDate
            // 
            this.labelCustRQSDate.Location = new System.Drawing.Point(13, 95);
            this.labelCustRQSDate.Name = "labelCustRQSDate";
            this.labelCustRQSDate.Size = new System.Drawing.Size(102, 23);
            this.labelCustRQSDate.TabIndex = 97;
            this.labelCustRQSDate.Text = "Cust RQS Date";
            // 
            // labelPlanDate
            // 
            this.labelPlanDate.Location = new System.Drawing.Point(13, 123);
            this.labelPlanDate.Name = "labelPlanDate";
            this.labelPlanDate.Size = new System.Drawing.Size(102, 23);
            this.labelPlanDate.TabIndex = 98;
            this.labelPlanDate.Text = "Plan Date";
            // 
            // labelOrderCfmDate
            // 
            this.labelOrderCfmDate.Location = new System.Drawing.Point(13, 151);
            this.labelOrderCfmDate.Name = "labelOrderCfmDate";
            this.labelOrderCfmDate.Size = new System.Drawing.Size(102, 23);
            this.labelOrderCfmDate.TabIndex = 99;
            this.labelOrderCfmDate.Text = "Order Cfm Date";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(13, 179);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(102, 23);
            this.labelStyle.TabIndex = 100;
            this.labelStyle.Text = "Style";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(13, 207);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(102, 23);
            this.labelSeason.TabIndex = 101;
            this.labelSeason.Text = "Season";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 235);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(102, 23);
            this.labelBrand.TabIndex = 102;
            this.labelBrand.Text = "Brand";
            // 
            // labelCustCD
            // 
            this.labelCustCD.Location = new System.Drawing.Point(13, 263);
            this.labelCustCD.Name = "labelCustCD";
            this.labelCustCD.Size = new System.Drawing.Size(102, 23);
            this.labelCustCD.TabIndex = 103;
            this.labelCustCD.Text = "Cust CD";
            // 
            // labelZone
            // 
            this.labelZone.Location = new System.Drawing.Point(13, 291);
            this.labelZone.Name = "labelZone";
            this.labelZone.Size = new System.Drawing.Size(102, 23);
            this.labelZone.TabIndex = 104;
            this.labelZone.Text = "Zone";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 319);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(102, 23);
            this.labelM.TabIndex = 105;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 347);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(102, 23);
            this.labelFactory.TabIndex = 106;
            this.labelFactory.Text = "Factory";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(13, 375);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(102, 23);
            this.labelCategory.TabIndex = 107;
            this.labelCategory.Text = "Category";
            // 
            // labelSubProcess
            // 
            this.labelSubProcess.Location = new System.Drawing.Point(13, 403);
            this.labelSubProcess.Name = "labelSubProcess";
            this.labelSubProcess.Size = new System.Drawing.Size(102, 23);
            this.labelSubProcess.TabIndex = 108;
            this.labelSubProcess.Text = "Sub Process";
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(102, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(124, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(102, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(119, 11);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(227, 23);
            this.dateBuyerDelivery.TabIndex = 1;
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(102, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(124, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(102, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(119, 39);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(227, 23);
            this.dateSCIDelivery.TabIndex = 2;
            // 
            // dateCutOffDate
            // 
            // 
            // 
            // 
            this.dateCutOffDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCutOffDate.DateBox1.Name = "";
            this.dateCutOffDate.DateBox1.Size = new System.Drawing.Size(102, 23);
            this.dateCutOffDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCutOffDate.DateBox2.Location = new System.Drawing.Point(124, 0);
            this.dateCutOffDate.DateBox2.Name = "";
            this.dateCutOffDate.DateBox2.Size = new System.Drawing.Size(102, 23);
            this.dateCutOffDate.DateBox2.TabIndex = 1;
            this.dateCutOffDate.IsRequired = false;
            this.dateCutOffDate.Location = new System.Drawing.Point(119, 67);
            this.dateCutOffDate.Name = "dateCutOffDate";
            this.dateCutOffDate.Size = new System.Drawing.Size(227, 23);
            this.dateCutOffDate.TabIndex = 3;
            // 
            // dateCustRQSDate
            // 
            // 
            // 
            // 
            this.dateCustRQSDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCustRQSDate.DateBox1.Name = "";
            this.dateCustRQSDate.DateBox1.Size = new System.Drawing.Size(102, 23);
            this.dateCustRQSDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCustRQSDate.DateBox2.Location = new System.Drawing.Point(124, 0);
            this.dateCustRQSDate.DateBox2.Name = "";
            this.dateCustRQSDate.DateBox2.Size = new System.Drawing.Size(102, 23);
            this.dateCustRQSDate.DateBox2.TabIndex = 1;
            this.dateCustRQSDate.IsRequired = false;
            this.dateCustRQSDate.Location = new System.Drawing.Point(119, 95);
            this.dateCustRQSDate.Name = "dateCustRQSDate";
            this.dateCustRQSDate.Size = new System.Drawing.Size(227, 23);
            this.dateCustRQSDate.TabIndex = 4;
            // 
            // datePlanDate
            // 
            // 
            // 
            // 
            this.datePlanDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.datePlanDate.DateBox1.Name = "";
            this.datePlanDate.DateBox1.Size = new System.Drawing.Size(102, 23);
            this.datePlanDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.datePlanDate.DateBox2.Location = new System.Drawing.Point(124, 0);
            this.datePlanDate.DateBox2.Name = "";
            this.datePlanDate.DateBox2.Size = new System.Drawing.Size(102, 23);
            this.datePlanDate.DateBox2.TabIndex = 1;
            this.datePlanDate.IsRequired = false;
            this.datePlanDate.Location = new System.Drawing.Point(119, 123);
            this.datePlanDate.Name = "datePlanDate";
            this.datePlanDate.Size = new System.Drawing.Size(227, 23);
            this.datePlanDate.TabIndex = 5;
            // 
            // dateOrderCfmDate
            // 
            // 
            // 
            // 
            this.dateOrderCfmDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateOrderCfmDate.DateBox1.Name = "";
            this.dateOrderCfmDate.DateBox1.Size = new System.Drawing.Size(102, 23);
            this.dateOrderCfmDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateOrderCfmDate.DateBox2.Location = new System.Drawing.Point(124, 0);
            this.dateOrderCfmDate.DateBox2.Name = "";
            this.dateOrderCfmDate.DateBox2.Size = new System.Drawing.Size(102, 23);
            this.dateOrderCfmDate.DateBox2.TabIndex = 1;
            this.dateOrderCfmDate.IsRequired = false;
            this.dateOrderCfmDate.Location = new System.Drawing.Point(119, 151);
            this.dateOrderCfmDate.Name = "dateOrderCfmDate";
            this.dateOrderCfmDate.Size = new System.Drawing.Size(227, 23);
            this.dateOrderCfmDate.TabIndex = 6;
            // 
            // comboZone
            // 
            this.comboZone.BackColor = System.Drawing.Color.White;
            this.comboZone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboZone.FormattingEnabled = true;
            this.comboZone.IsSupportUnselect = true;
            this.comboZone.Location = new System.Drawing.Point(119, 290);
            this.comboZone.Name = "comboZone";
            this.comboZone.Size = new System.Drawing.Size(227, 24);
            this.comboZone.TabIndex = 11;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(119, 318);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(66, 24);
            this.comboM.TabIndex = 12;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(119, 346);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(66, 24);
            this.comboFactory.TabIndex = 13;
            // 
            // comboSubProcess
            // 
            this.comboSubProcess.BackColor = System.Drawing.Color.White;
            this.comboSubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSubProcess.FormattingEnabled = true;
            this.comboSubProcess.IsSupportUnselect = true;
            this.comboSubProcess.Location = new System.Drawing.Point(119, 402);
            this.comboSubProcess.Name = "comboSubProcess";
            this.comboSubProcess.Size = new System.Drawing.Size(165, 24);
            this.comboSubProcess.TabIndex = 18;
            // 
            // checkIncludeHistoryOrder
            // 
            this.checkIncludeHistoryOrder.AutoSize = true;
            this.checkIncludeHistoryOrder.Checked = true;
            this.checkIncludeHistoryOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkIncludeHistoryOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIncludeHistoryOrder.Location = new System.Drawing.Point(13, 430);
            this.checkIncludeHistoryOrder.Name = "checkIncludeHistoryOrder";
            this.checkIncludeHistoryOrder.Size = new System.Drawing.Size(161, 21);
            this.checkIncludeHistoryOrder.TabIndex = 19;
            this.checkIncludeHistoryOrder.Text = "Include History Order";
            this.checkIncludeHistoryOrder.UseVisualStyleBackColor = true;
            // 
            // checkIncludeArtworkdata
            // 
            this.checkIncludeArtworkdata.AutoSize = true;
            this.checkIncludeArtworkdata.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIncludeArtworkdata.Location = new System.Drawing.Point(13, 458);
            this.checkIncludeArtworkdata.Name = "checkIncludeArtworkdata";
            this.checkIncludeArtworkdata.Size = new System.Drawing.Size(155, 21);
            this.checkIncludeArtworkdata.TabIndex = 20;
            this.checkIncludeArtworkdata.Text = "Include Artwork data";
            this.checkIncludeArtworkdata.UseVisualStyleBackColor = true;
            // 
            // checkIncludeArtworkdataKindIsPAP
            // 
            this.checkIncludeArtworkdataKindIsPAP.AutoSize = true;
            this.checkIncludeArtworkdataKindIsPAP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIncludeArtworkdataKindIsPAP.Location = new System.Drawing.Point(13, 486);
            this.checkIncludeArtworkdataKindIsPAP.Name = "checkIncludeArtworkdataKindIsPAP";
            this.checkIncludeArtworkdataKindIsPAP.Size = new System.Drawing.Size(252, 21);
            this.checkIncludeArtworkdataKindIsPAP.TabIndex = 21;
            this.checkIncludeArtworkdataKindIsPAP.Text = "Include Artwork data -- Kind is \'PAP\'";
            this.checkIncludeArtworkdataKindIsPAP.UseVisualStyleBackColor = true;
            // 
            // checkQtyBDownByShipmode
            // 
            this.checkQtyBDownByShipmode.AutoSize = true;
            this.checkQtyBDownByShipmode.Checked = true;
            this.checkQtyBDownByShipmode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkQtyBDownByShipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkQtyBDownByShipmode.Location = new System.Drawing.Point(13, 514);
            this.checkQtyBDownByShipmode.Name = "checkQtyBDownByShipmode";
            this.checkQtyBDownByShipmode.Size = new System.Drawing.Size(286, 21);
            this.checkQtyBDownByShipmode.TabIndex = 22;
            this.checkQtyBDownByShipmode.Text = "Seperate by < Qty b\'down by shipmode >";
            this.checkQtyBDownByShipmode.UseVisualStyleBackColor = true;
            // 
            // checkListPOCombo
            // 
            this.checkListPOCombo.AutoSize = true;
            this.checkListPOCombo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkListPOCombo.Location = new System.Drawing.Point(13, 542);
            this.checkListPOCombo.Name = "checkListPOCombo";
            this.checkListPOCombo.Size = new System.Drawing.Size(121, 21);
            this.checkListPOCombo.TabIndex = 23;
            this.checkListPOCombo.Text = "List PO Combo";
            this.checkListPOCombo.UseVisualStyleBackColor = true;
            // 
            // checkBulk
            // 
            this.checkBulk.AutoSize = true;
            this.checkBulk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBulk.Location = new System.Drawing.Point(119, 375);
            this.checkBulk.Name = "checkBulk";
            this.checkBulk.Size = new System.Drawing.Size(54, 21);
            this.checkBulk.TabIndex = 14;
            this.checkBulk.Text = "Bulk";
            this.checkBulk.UseVisualStyleBackColor = true;
            // 
            // checkSample
            // 
            this.checkSample.AutoSize = true;
            this.checkSample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkSample.Location = new System.Drawing.Point(180, 375);
            this.checkSample.Name = "checkSample";
            this.checkSample.Size = new System.Drawing.Size(74, 21);
            this.checkSample.TabIndex = 15;
            this.checkSample.Text = "Sample";
            this.checkSample.UseVisualStyleBackColor = true;
            // 
            // checkMaterial
            // 
            this.checkMaterial.AutoSize = true;
            this.checkMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkMaterial.Location = new System.Drawing.Point(260, 375);
            this.checkMaterial.Name = "checkMaterial";
            this.checkMaterial.Size = new System.Drawing.Size(77, 21);
            this.checkMaterial.TabIndex = 16;
            this.checkMaterial.Text = "Material";
            this.checkMaterial.UseVisualStyleBackColor = true;
            // 
            // checkForecast
            // 
            this.checkForecast.AutoSize = true;
            this.checkForecast.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkForecast.Location = new System.Drawing.Point(344, 375);
            this.checkForecast.Name = "checkForecast";
            this.checkForecast.Size = new System.Drawing.Size(82, 21);
            this.checkForecast.TabIndex = 17;
            this.checkForecast.Text = "Forecast";
            this.checkForecast.UseVisualStyleBackColor = true;
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(119, 178);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.Size = new System.Drawing.Size(186, 23);
            this.txtstyle.TabIndex = 7;
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(119, 207);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(98, 23);
            this.txtseason.TabIndex = 8;
            // 
            // txtcustcd
            // 
            this.txtcustcd.BackColor = System.Drawing.Color.White;
            this.txtcustcd.BrandObjectName = null;
            this.txtcustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd.Location = new System.Drawing.Point(119, 261);
            this.txtcustcd.Name = "txtcustcd";
            this.txtcustcd.Size = new System.Drawing.Size(186, 23);
            this.txtcustcd.TabIndex = 10;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(119, 235);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(98, 23);
            this.txtbrand.TabIndex = 9;
            // 
            // checkGarment
            // 
            this.checkGarment.AutoSize = true;
            this.checkGarment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkGarment.Location = new System.Drawing.Point(432, 375);
            this.checkGarment.Name = "checkGarment";
            this.checkGarment.Size = new System.Drawing.Size(82, 21);
            this.checkGarment.TabIndex = 109;
            this.checkGarment.Text = "Garment";
            this.checkGarment.UseVisualStyleBackColor = true;
            // 
            // R03
            // 
            this.ClientSize = new System.Drawing.Size(520, 590);
            this.Controls.Add(this.checkGarment);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.txtcustcd);
            this.Controls.Add(this.txtseason);
            this.Controls.Add(this.txtstyle);
            this.Controls.Add(this.checkForecast);
            this.Controls.Add(this.checkMaterial);
            this.Controls.Add(this.checkSample);
            this.Controls.Add(this.checkBulk);
            this.Controls.Add(this.checkListPOCombo);
            this.Controls.Add(this.checkQtyBDownByShipmode);
            this.Controls.Add(this.checkIncludeArtworkdataKindIsPAP);
            this.Controls.Add(this.checkIncludeArtworkdata);
            this.Controls.Add(this.checkIncludeHistoryOrder);
            this.Controls.Add(this.comboSubProcess);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.comboZone);
            this.Controls.Add(this.dateOrderCfmDate);
            this.Controls.Add(this.datePlanDate);
            this.Controls.Add(this.dateCustRQSDate);
            this.Controls.Add(this.dateCutOffDate);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.labelSubProcess);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelZone);
            this.Controls.Add(this.labelCustCD);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelSeason);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.labelOrderCfmDate);
            this.Controls.Add(this.labelPlanDate);
            this.Controls.Add(this.labelCustRQSDate);
            this.Controls.Add(this.labelCutOffDate);
            this.Controls.Add(this.labelSCIDelivery);
            this.Controls.Add(this.labelBuyerDelivery);
            this.DefaultControl = "dateBuyerDelivery";
            this.DefaultControlForEdit = "dateBuyerDelivery";
            this.IsSupportToPrint = false;
            this.Name = "R03";
            this.Text = "R03. PPIC master list report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.labelCutOffDate, 0);
            this.Controls.SetChildIndex(this.labelCustRQSDate, 0);
            this.Controls.SetChildIndex(this.labelPlanDate, 0);
            this.Controls.SetChildIndex(this.labelOrderCfmDate, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.labelSeason, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelCustCD, 0);
            this.Controls.SetChildIndex(this.labelZone, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.labelSubProcess, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.dateCutOffDate, 0);
            this.Controls.SetChildIndex(this.dateCustRQSDate, 0);
            this.Controls.SetChildIndex(this.datePlanDate, 0);
            this.Controls.SetChildIndex(this.dateOrderCfmDate, 0);
            this.Controls.SetChildIndex(this.comboZone, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.comboSubProcess, 0);
            this.Controls.SetChildIndex(this.checkIncludeHistoryOrder, 0);
            this.Controls.SetChildIndex(this.checkIncludeArtworkdata, 0);
            this.Controls.SetChildIndex(this.checkIncludeArtworkdataKindIsPAP, 0);
            this.Controls.SetChildIndex(this.checkQtyBDownByShipmode, 0);
            this.Controls.SetChildIndex(this.checkListPOCombo, 0);
            this.Controls.SetChildIndex(this.checkBulk, 0);
            this.Controls.SetChildIndex(this.checkSample, 0);
            this.Controls.SetChildIndex(this.checkMaterial, 0);
            this.Controls.SetChildIndex(this.checkForecast, 0);
            this.Controls.SetChildIndex(this.txtstyle, 0);
            this.Controls.SetChildIndex(this.txtseason, 0);
            this.Controls.SetChildIndex(this.txtcustcd, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.checkGarment, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label labelCutOffDate;
        private Win.UI.Label labelCustRQSDate;
        private Win.UI.Label labelPlanDate;
        private Win.UI.Label labelOrderCfmDate;
        private Win.UI.Label labelStyle;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelCustCD;
        private Win.UI.Label labelZone;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelSubProcess;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.DateRange dateCutOffDate;
        private Win.UI.DateRange dateCustRQSDate;
        private Win.UI.DateRange datePlanDate;
        private Win.UI.DateRange dateOrderCfmDate;
        private Win.UI.ComboBox comboZone;
        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.ComboBox comboSubProcess;
        private Win.UI.CheckBox checkIncludeHistoryOrder;
        private Win.UI.CheckBox checkIncludeArtworkdata;
        private Win.UI.CheckBox checkIncludeArtworkdataKindIsPAP;
        private Win.UI.CheckBox checkQtyBDownByShipmode;
        private Win.UI.CheckBox checkListPOCombo;
        private Win.UI.CheckBox checkBulk;
        private Win.UI.CheckBox checkSample;
        private Win.UI.CheckBox checkMaterial;
        private Win.UI.CheckBox checkForecast;
        private Class.txtstyle txtstyle;
        private Class.txtseason txtseason;
        private Class.txtcustcd txtcustcd;
        private Class.txtbrand txtbrand;
        private Win.UI.CheckBox checkGarment;
    }
}
