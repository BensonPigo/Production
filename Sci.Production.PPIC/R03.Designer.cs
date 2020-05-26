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
            this.checkGarment = new Sci.Win.UI.CheckBox();
            this.labelArticle = new Sci.Win.UI.Label();
            this.txtArticle = new Sci.Win.UI.TextBox();
            this.checkSMTL = new Sci.Win.UI.CheckBox();
            this.txtSp1 = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSp2 = new Sci.Win.UI.TextBox();
            this.txtbrand = new Sci.Production.Class.txtbrand();
            this.txtcustcd = new Sci.Production.Class.txtcustcd();
            this.txtseason = new Sci.Production.Class.txtseason();
            this.txtstyle = new Sci.Production.Class.txtstyle();
            this.checkByCPU = new Sci.Win.UI.CheckBox();
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(502, 12);
            this.print.TabIndex = 29;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(502, 48);
            this.toexcel.TabIndex = 30;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(502, 84);
            this.close.TabIndex = 31;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(456, 120);
            this.buttonCustomized.Visible = true;
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(475, 156);
            this.checkUseCustomized.Visible = true;
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(475, 183);
            this.txtVersion.Visible = true;
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
            this.labelStyle.Location = new System.Drawing.Point(13, 210);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(102, 23);
            this.labelStyle.TabIndex = 100;
            this.labelStyle.Text = "Style";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(13, 265);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(102, 23);
            this.labelSeason.TabIndex = 101;
            this.labelSeason.Text = "Season";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 293);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(102, 23);
            this.labelBrand.TabIndex = 102;
            this.labelBrand.Text = "Brand";
            // 
            // labelCustCD
            // 
            this.labelCustCD.Location = new System.Drawing.Point(13, 321);
            this.labelCustCD.Name = "labelCustCD";
            this.labelCustCD.Size = new System.Drawing.Size(102, 23);
            this.labelCustCD.TabIndex = 103;
            this.labelCustCD.Text = "Cust CD";
            // 
            // labelZone
            // 
            this.labelZone.Location = new System.Drawing.Point(13, 349);
            this.labelZone.Name = "labelZone";
            this.labelZone.Size = new System.Drawing.Size(102, 23);
            this.labelZone.TabIndex = 104;
            this.labelZone.Text = "Zone";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 377);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(102, 23);
            this.labelM.TabIndex = 105;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 405);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(102, 23);
            this.labelFactory.TabIndex = 106;
            this.labelFactory.Text = "Factory";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(13, 433);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(102, 23);
            this.labelCategory.TabIndex = 107;
            this.labelCategory.Text = "Category";
            // 
            // labelSubProcess
            // 
            this.labelSubProcess.Location = new System.Drawing.Point(13, 461);
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
            this.comboZone.Location = new System.Drawing.Point(119, 348);
            this.comboZone.Name = "comboZone";
            this.comboZone.OldText = "";
            this.comboZone.Size = new System.Drawing.Size(227, 24);
            this.comboZone.TabIndex = 14;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(119, 376);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(66, 24);
            this.comboM.TabIndex = 15;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(119, 404);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(66, 24);
            this.comboFactory.TabIndex = 16;
            // 
            // comboSubProcess
            // 
            this.comboSubProcess.BackColor = System.Drawing.Color.White;
            this.comboSubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSubProcess.FormattingEnabled = true;
            this.comboSubProcess.IsSupportUnselect = true;
            this.comboSubProcess.Location = new System.Drawing.Point(119, 460);
            this.comboSubProcess.Name = "comboSubProcess";
            this.comboSubProcess.OldText = "";
            this.comboSubProcess.Size = new System.Drawing.Size(165, 24);
            this.comboSubProcess.TabIndex = 23;
            // 
            // checkIncludeHistoryOrder
            // 
            this.checkIncludeHistoryOrder.AutoSize = true;
            this.checkIncludeHistoryOrder.Checked = true;
            this.checkIncludeHistoryOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkIncludeHistoryOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIncludeHistoryOrder.Location = new System.Drawing.Point(13, 488);
            this.checkIncludeHistoryOrder.Name = "checkIncludeHistoryOrder";
            this.checkIncludeHistoryOrder.Size = new System.Drawing.Size(161, 21);
            this.checkIncludeHistoryOrder.TabIndex = 24;
            this.checkIncludeHistoryOrder.Text = "Include History Order";
            this.checkIncludeHistoryOrder.UseVisualStyleBackColor = true;
            // 
            // checkIncludeArtworkdata
            // 
            this.checkIncludeArtworkdata.AutoSize = true;
            this.checkIncludeArtworkdata.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIncludeArtworkdata.Location = new System.Drawing.Point(13, 516);
            this.checkIncludeArtworkdata.Name = "checkIncludeArtworkdata";
            this.checkIncludeArtworkdata.Size = new System.Drawing.Size(155, 21);
            this.checkIncludeArtworkdata.TabIndex = 25;
            this.checkIncludeArtworkdata.Text = "Include Artwork data";
            this.checkIncludeArtworkdata.UseVisualStyleBackColor = true;
            // 
            // checkIncludeArtworkdataKindIsPAP
            // 
            this.checkIncludeArtworkdataKindIsPAP.AutoSize = true;
            this.checkIncludeArtworkdataKindIsPAP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIncludeArtworkdataKindIsPAP.Location = new System.Drawing.Point(13, 544);
            this.checkIncludeArtworkdataKindIsPAP.Name = "checkIncludeArtworkdataKindIsPAP";
            this.checkIncludeArtworkdataKindIsPAP.Size = new System.Drawing.Size(252, 21);
            this.checkIncludeArtworkdataKindIsPAP.TabIndex = 26;
            this.checkIncludeArtworkdataKindIsPAP.Text = "Include Artwork data -- Kind is \'PAP\'";
            this.checkIncludeArtworkdataKindIsPAP.UseVisualStyleBackColor = true;
            // 
            // checkQtyBDownByShipmode
            // 
            this.checkQtyBDownByShipmode.AutoSize = true;
            this.checkQtyBDownByShipmode.Checked = true;
            this.checkQtyBDownByShipmode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkQtyBDownByShipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkQtyBDownByShipmode.Location = new System.Drawing.Point(13, 571);
            this.checkQtyBDownByShipmode.Name = "checkQtyBDownByShipmode";
            this.checkQtyBDownByShipmode.Size = new System.Drawing.Size(286, 21);
            this.checkQtyBDownByShipmode.TabIndex = 27;
            this.checkQtyBDownByShipmode.Text = "Seperate by < Qty b\'down by shipmode >";
            this.checkQtyBDownByShipmode.UseVisualStyleBackColor = true;
            // 
            // checkListPOCombo
            // 
            this.checkListPOCombo.AutoSize = true;
            this.checkListPOCombo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkListPOCombo.Location = new System.Drawing.Point(13, 599);
            this.checkListPOCombo.Name = "checkListPOCombo";
            this.checkListPOCombo.Size = new System.Drawing.Size(121, 21);
            this.checkListPOCombo.TabIndex = 28;
            this.checkListPOCombo.Text = "List PO Combo";
            this.checkListPOCombo.UseVisualStyleBackColor = true;
            // 
            // checkBulk
            // 
            this.checkBulk.AutoSize = true;
            this.checkBulk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBulk.Location = new System.Drawing.Point(119, 433);
            this.checkBulk.Name = "checkBulk";
            this.checkBulk.Size = new System.Drawing.Size(54, 21);
            this.checkBulk.TabIndex = 17;
            this.checkBulk.Text = "Bulk";
            this.checkBulk.UseVisualStyleBackColor = true;
            // 
            // checkSample
            // 
            this.checkSample.AutoSize = true;
            this.checkSample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkSample.Location = new System.Drawing.Point(180, 433);
            this.checkSample.Name = "checkSample";
            this.checkSample.Size = new System.Drawing.Size(74, 21);
            this.checkSample.TabIndex = 18;
            this.checkSample.Text = "Sample";
            this.checkSample.UseVisualStyleBackColor = true;
            // 
            // checkMaterial
            // 
            this.checkMaterial.AutoSize = true;
            this.checkMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkMaterial.Location = new System.Drawing.Point(260, 433);
            this.checkMaterial.Name = "checkMaterial";
            this.checkMaterial.Size = new System.Drawing.Size(77, 21);
            this.checkMaterial.TabIndex = 19;
            this.checkMaterial.Text = "Material";
            this.checkMaterial.UseVisualStyleBackColor = true;
            // 
            // checkForecast
            // 
            this.checkForecast.AutoSize = true;
            this.checkForecast.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkForecast.Location = new System.Drawing.Point(344, 433);
            this.checkForecast.Name = "checkForecast";
            this.checkForecast.Size = new System.Drawing.Size(82, 21);
            this.checkForecast.TabIndex = 20;
            this.checkForecast.Text = "Forecast";
            this.checkForecast.UseVisualStyleBackColor = true;
            // 
            // checkGarment
            // 
            this.checkGarment.AutoSize = true;
            this.checkGarment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkGarment.Location = new System.Drawing.Point(432, 433);
            this.checkGarment.Name = "checkGarment";
            this.checkGarment.Size = new System.Drawing.Size(82, 21);
            this.checkGarment.TabIndex = 21;
            this.checkGarment.Text = "Garment";
            this.checkGarment.UseVisualStyleBackColor = true;
            // 
            // labelArticle
            // 
            this.labelArticle.Location = new System.Drawing.Point(13, 237);
            this.labelArticle.Name = "labelArticle";
            this.labelArticle.Size = new System.Drawing.Size(102, 23);
            this.labelArticle.TabIndex = 111;
            this.labelArticle.Text = "Article";
            // 
            // txtArticle
            // 
            this.txtArticle.BackColor = System.Drawing.Color.White;
            this.txtArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtArticle.Location = new System.Drawing.Point(119, 237);
            this.txtArticle.Name = "txtArticle";
            this.txtArticle.Size = new System.Drawing.Size(100, 23);
            this.txtArticle.TabIndex = 10;
            // 
            // checkSMTL
            // 
            this.checkSMTL.AutoSize = true;
            this.checkSMTL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkSMTL.Location = new System.Drawing.Point(520, 433);
            this.checkSMTL.Name = "checkSMTL";
            this.checkSMTL.Size = new System.Drawing.Size(64, 21);
            this.checkSMTL.TabIndex = 22;
            this.checkSMTL.Text = "SMTL";
            this.checkSMTL.UseVisualStyleBackColor = true;
            // 
            // txtSp1
            // 
            this.txtSp1.BackColor = System.Drawing.Color.White;
            this.txtSp1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSp1.Location = new System.Drawing.Point(119, 180);
            this.txtSp1.Name = "txtSp1";
            this.txtSp1.Size = new System.Drawing.Size(122, 23);
            this.txtSp1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 23);
            this.label1.TabIndex = 114;
            this.label1.Text = "SP#";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(243, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 17);
            this.label2.TabIndex = 115;
            this.label2.Text = "～";
            // 
            // txtSp2
            // 
            this.txtSp2.BackColor = System.Drawing.Color.White;
            this.txtSp2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSp2.Location = new System.Drawing.Point(271, 180);
            this.txtSp2.Name = "txtSp2";
            this.txtSp2.Size = new System.Drawing.Size(122, 23);
            this.txtSp2.TabIndex = 8;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(119, 293);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(98, 23);
            this.txtbrand.TabIndex = 12;
            // 
            // txtcustcd
            // 
            this.txtcustcd.BackColor = System.Drawing.Color.White;
            this.txtcustcd.BrandObjectName = null;
            this.txtcustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd.Location = new System.Drawing.Point(119, 319);
            this.txtcustcd.Name = "txtcustcd";
            this.txtcustcd.Size = new System.Drawing.Size(186, 23);
            this.txtcustcd.TabIndex = 13;
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(119, 265);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(98, 23);
            this.txtseason.TabIndex = 11;
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(119, 209);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.Size = new System.Drawing.Size(186, 23);
            this.txtstyle.TabIndex = 9;
            this.txtstyle.tarBrand = null;
            this.txtstyle.tarSeason = null;
            // 
            // checkByCPU
            // 
            this.checkByCPU.AutoSize = true;
            this.checkByCPU.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkByCPU.Location = new System.Drawing.Point(271, 516);
            this.checkByCPU.Name = "checkByCPU";
            this.checkByCPU.Size = new System.Drawing.Size(75, 21);
            this.checkByCPU.TabIndex = 116;
            this.checkByCPU.Text = "By CPU";
            this.checkByCPU.UseVisualStyleBackColor = true;
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(13, 626);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 129;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // R03
            // 
            this.ClientSize = new System.Drawing.Size(587, 682);
            this.Controls.Add(this.chkIncludeCancelOrder);
            this.Controls.Add(this.checkByCPU);
            this.Controls.Add(this.txtSp2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSp1);
            this.Controls.Add(this.checkSMTL);
            this.Controls.Add(this.txtArticle);
            this.Controls.Add(this.labelArticle);
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
            this.IsSupportCustomized = true;
            this.IsSupportToPrint = false;
            this.Name = "R03";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R03. PPIC master list report";
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
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
            this.Controls.SetChildIndex(this.labelArticle, 0);
            this.Controls.SetChildIndex(this.txtArticle, 0);
            this.Controls.SetChildIndex(this.checkSMTL, 0);
            this.Controls.SetChildIndex(this.txtSp1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtSp2, 0);
            this.Controls.SetChildIndex(this.checkByCPU, 0);
            this.Controls.SetChildIndex(this.chkIncludeCancelOrder, 0);
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
        private Win.UI.Label labelArticle;
        private Win.UI.TextBox txtArticle;
        private Win.UI.CheckBox checkSMTL;
        private Win.UI.TextBox txtSp1;
        private Win.UI.Label label1;
        private System.Windows.Forms.Label label2;
        private Win.UI.TextBox txtSp2;
        private Win.UI.CheckBox checkByCPU;
        private Win.UI.CheckBox chkIncludeCancelOrder;
    }
}
