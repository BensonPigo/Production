namespace Sci.Production.Packing
{
    partial class P03
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
            this.labelNo = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.labelSortby = new Sci.Win.UI.Label();
            this.displayNo = new Sci.Win.UI.DisplayBox();
            this.comboSortby = new Sci.Win.UI.ComboBox();
            this.labelShipMode = new Sci.Win.UI.Label();
            this.labelStartCtn = new Sci.Win.UI.Label();
            this.labelTtlCtns = new Sci.Win.UI.Label();
            this.labelShipQty = new Sci.Win.UI.Label();
            this.labelTtlCBM = new Sci.Win.UI.Label();
            this.labelPurchaseCtn = new Sci.Win.UI.Label();
            this.displayStartCtn = new Sci.Win.UI.DisplayBox();
            this.numTtlCtns = new Sci.Win.UI.NumericBox();
            this.numShipQty = new Sci.Win.UI.NumericBox();
            this.numTtlCBM = new Sci.Win.UI.NumericBox();
            this.displayPurchaseCtn = new Sci.Win.UI.DisplayBox();
            this.labelPullOutDate = new Sci.Win.UI.Label();
            this.labelInvoiceNo = new Sci.Win.UI.Label();
            this.labelShipPlanNo = new Sci.Win.UI.Label();
            this.labelCartonEstBooking = new Sci.Win.UI.Label();
            this.labelCartonEstArrived = new Sci.Win.UI.Label();
            this.datePullOutDate = new Sci.Win.UI.DateBox();
            this.displayInvoiceNo = new Sci.Win.UI.DisplayBox();
            this.displayShipPlanNo = new Sci.Win.UI.DisplayBox();
            this.dateCartonEstBooking = new Sci.Win.UI.DateBox();
            this.dateCartonEstArrived = new Sci.Win.UI.DateBox();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.btnCartonSummary = new Sci.Win.UI.Button();
            this.btnRecalculateWeight = new Sci.Win.UI.Button();
            this.btnUnConfirmHistory = new Sci.Win.UI.Button();
            this.labelLocateforTransferClog = new Sci.Win.UI.Label();
            this.dateLocateforTransferClog = new Sci.Win.UI.DateBox();
            this.txtLocateforTransferClog = new Sci.Win.UI.TextBox();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelCustCD = new Sci.Win.UI.Label();
            this.labelDestination = new Sci.Win.UI.Label();
            this.labelCofirmed = new Sci.Win.UI.Label();
            this.btnDownloadSampleFile = new Sci.Win.UI.Button();
            this.btnImportFromExcel = new Sci.Win.UI.Button();
            this.txtcountry = new Sci.Production.Class.txtcountry();
            this.txtcustcd = new Sci.Production.Class.txtcustcd();
            this.txtbrand = new Sci.Production.Class.txtbrand();
            this.txtshipmode = new Sci.Production.Class.txtshipmode();
            this.numTtlGW = new Sci.Win.UI.NumericBox();
            this.labelTtlGW = new Sci.Win.UI.Label();
            this.labelPullOutNo = new Sci.Win.UI.Label();
            this.displayBoxPullOutNo = new Sci.Win.UI.DisplayBox();
            this.btnUpdateBarcode = new Sci.Win.UI.Button();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.datesciDelivery = new Sci.Win.UI.DateBox();
            this.datekpileta = new Sci.Win.UI.DateBox();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).BeginInit();
            this.masterpanel.SuspendLayout();
            this.detailpanel.SuspendLayout();
            this.detail2.SuspendLayout();
            this.detailpanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Controls.Add(this.btnUpdateBarcode);
            this.masterpanel.Controls.Add(this.datekpileta);
            this.masterpanel.Controls.Add(this.datesciDelivery);
            this.masterpanel.Controls.Add(this.numTtlGW);
            this.masterpanel.Controls.Add(this.labelTtlGW);
            this.masterpanel.Controls.Add(this.btnImportFromExcel);
            this.masterpanel.Controls.Add(this.btnDownloadSampleFile);
            this.masterpanel.Controls.Add(this.labelCofirmed);
            this.masterpanel.Controls.Add(this.txtcountry);
            this.masterpanel.Controls.Add(this.txtcustcd);
            this.masterpanel.Controls.Add(this.txtbrand);
            this.masterpanel.Controls.Add(this.labelDestination);
            this.masterpanel.Controls.Add(this.labelCustCD);
            this.masterpanel.Controls.Add(this.labelBrand);
            this.masterpanel.Controls.Add(this.btnFindNow);
            this.masterpanel.Controls.Add(this.txtLocateforTransferClog);
            this.masterpanel.Controls.Add(this.labelLocateforTransferClog);
            this.masterpanel.Controls.Add(this.btnUnConfirmHistory);
            this.masterpanel.Controls.Add(this.btnRecalculateWeight);
            this.masterpanel.Controls.Add(this.btnCartonSummary);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.displayShipPlanNo);
            this.masterpanel.Controls.Add(this.displayBoxPullOutNo);
            this.masterpanel.Controls.Add(this.displayInvoiceNo);
            this.masterpanel.Controls.Add(this.labelCartonEstArrived);
            this.masterpanel.Controls.Add(this.labelCartonEstBooking);
            this.masterpanel.Controls.Add(this.labelShipPlanNo);
            this.masterpanel.Controls.Add(this.labelInvoiceNo);
            this.masterpanel.Controls.Add(this.labelPullOutNo);
            this.masterpanel.Controls.Add(this.labelPullOutDate);
            this.masterpanel.Controls.Add(this.displayPurchaseCtn);
            this.masterpanel.Controls.Add(this.numTtlCBM);
            this.masterpanel.Controls.Add(this.numShipQty);
            this.masterpanel.Controls.Add(this.numTtlCtns);
            this.masterpanel.Controls.Add(this.displayStartCtn);
            this.masterpanel.Controls.Add(this.txtshipmode);
            this.masterpanel.Controls.Add(this.labelPurchaseCtn);
            this.masterpanel.Controls.Add(this.labelTtlCBM);
            this.masterpanel.Controls.Add(this.labelShipQty);
            this.masterpanel.Controls.Add(this.labelTtlCtns);
            this.masterpanel.Controls.Add(this.labelStartCtn);
            this.masterpanel.Controls.Add(this.labelShipMode);
            this.masterpanel.Controls.Add(this.comboSortby);
            this.masterpanel.Controls.Add(this.displayNo);
            this.masterpanel.Controls.Add(this.labelSortby);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.labelNo);
            this.masterpanel.Controls.Add(this.dateLocateforTransferClog);
            this.masterpanel.Controls.Add(this.dateCartonEstArrived);
            this.masterpanel.Controls.Add(this.dateCartonEstBooking);
            this.masterpanel.Controls.Add(this.datePullOutDate);
            this.masterpanel.Size = new System.Drawing.Size(1000, 287);
            this.masterpanel.Controls.SetChildIndex(this.datePullOutDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateCartonEstBooking, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateCartonEstArrived, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateLocateforTransferClog, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSortby, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboSortby, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShipMode, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelStartCtn, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTtlCtns, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShipQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTtlCBM, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPurchaseCtn, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtshipmode, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayStartCtn, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTtlCtns, 0);
            this.masterpanel.Controls.SetChildIndex(this.numShipQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTtlCBM, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayPurchaseCtn, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPullOutDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPullOutNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelInvoiceNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShipPlanNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCartonEstBooking, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCartonEstArrived, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayInvoiceNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBoxPullOutNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayShipPlanNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnCartonSummary, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnRecalculateWeight, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnUnConfirmHistory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelLocateforTransferClog, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLocateforTransferClog, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnFindNow, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelBrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCustCD, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDestination, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtbrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtcustcd, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtcountry, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCofirmed, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnDownloadSampleFile, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportFromExcel, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTtlGW, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTtlGW, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.datesciDelivery, 0);
            this.masterpanel.Controls.SetChildIndex(this.datekpileta, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnUpdateBarcode, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 287);
            this.detailpanel.Size = new System.Drawing.Size(1000, 231);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(894, 252);
            this.gridicon.TabIndex = 8;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(912, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1000, 231);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(892, 387);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(886, 341);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(886, 40);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(1000, 556);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1000, 518);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 518);
            this.detailbtm.Size = new System.Drawing.Size(1000, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1000, 556);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1008, 585);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(481, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(433, 13);
            // 
            // labelNo
            // 
            this.labelNo.Location = new System.Drawing.Point(4, 4);
            this.labelNo.Name = "labelNo";
            this.labelNo.Size = new System.Drawing.Size(75, 22);
            this.labelNo.TabIndex = 7;
            this.labelNo.Text = "No.";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(4, 140);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 7;
            this.labelRemark.Text = "Remark";
            // 
            // labelSortby
            // 
            this.labelSortby.Location = new System.Drawing.Point(5, 261);
            this.labelSortby.Name = "labelSortby";
            this.labelSortby.Size = new System.Drawing.Size(66, 23);
            this.labelSortby.TabIndex = 8;
            this.labelSortby.Text = "Sort by";
            // 
            // displayNo
            // 
            this.displayNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayNo.Location = new System.Drawing.Point(83, 4);
            this.displayNo.Name = "displayNo";
            this.displayNo.Size = new System.Drawing.Size(120, 23);
            this.displayNo.TabIndex = 9;
            // 
            // comboSortby
            // 
            this.comboSortby.BackColor = System.Drawing.Color.White;
            this.comboSortby.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.comboSortby.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSortby.FormattingEnabled = true;
            this.comboSortby.IsSupportUnselect = true;
            this.comboSortby.Location = new System.Drawing.Point(75, 260);
            this.comboSortby.Name = "comboSortby";
            this.comboSortby.OldText = "";
            this.comboSortby.Size = new System.Drawing.Size(121, 24);
            this.comboSortby.TabIndex = 18;
            this.comboSortby.SelectedIndexChanged += new System.EventHandler(this.ComboSortby_SelectedIndexChanged);
            // 
            // labelShipMode
            // 
            this.labelShipMode.Location = new System.Drawing.Point(4, 112);
            this.labelShipMode.Name = "labelShipMode";
            this.labelShipMode.Size = new System.Drawing.Size(75, 22);
            this.labelShipMode.TabIndex = 19;
            this.labelShipMode.Text = "Ship Mode";
            // 
            // labelStartCtn
            // 
            this.labelStartCtn.Location = new System.Drawing.Point(332, 4);
            this.labelStartCtn.Name = "labelStartCtn";
            this.labelStartCtn.Size = new System.Drawing.Size(90, 22);
            this.labelStartCtn.TabIndex = 20;
            this.labelStartCtn.Text = "Start Ctn#";
            // 
            // labelTtlCtns
            // 
            this.labelTtlCtns.Location = new System.Drawing.Point(332, 31);
            this.labelTtlCtns.Name = "labelTtlCtns";
            this.labelTtlCtns.Size = new System.Drawing.Size(90, 22);
            this.labelTtlCtns.TabIndex = 21;
            this.labelTtlCtns.Text = "Ttl Ctns";
            // 
            // labelShipQty
            // 
            this.labelShipQty.Location = new System.Drawing.Point(332, 57);
            this.labelShipQty.Name = "labelShipQty";
            this.labelShipQty.Size = new System.Drawing.Size(90, 22);
            this.labelShipQty.TabIndex = 22;
            this.labelShipQty.Text = "Ship Qty";
            // 
            // labelTtlCBM
            // 
            this.labelTtlCBM.Location = new System.Drawing.Point(332, 83);
            this.labelTtlCBM.Name = "labelTtlCBM";
            this.labelTtlCBM.Size = new System.Drawing.Size(90, 22);
            this.labelTtlCBM.TabIndex = 23;
            this.labelTtlCBM.Text = "Ttl CBM";
            // 
            // labelPurchaseCtn
            // 
            this.labelPurchaseCtn.Location = new System.Drawing.Point(521, 227);
            this.labelPurchaseCtn.Name = "labelPurchaseCtn";
            this.labelPurchaseCtn.Size = new System.Drawing.Size(126, 22);
            this.labelPurchaseCtn.TabIndex = 24;
            this.labelPurchaseCtn.Text = "Purchase Ctn";
            // 
            // displayStartCtn
            // 
            this.displayStartCtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStartCtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStartCtn.Location = new System.Drawing.Point(426, 4);
            this.displayStartCtn.Name = "displayStartCtn";
            this.displayStartCtn.Size = new System.Drawing.Size(52, 23);
            this.displayStartCtn.TabIndex = 26;
            // 
            // numTtlCtns
            // 
            this.numTtlCtns.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTtlCtns.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CTNQty", true));
            this.numTtlCtns.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTtlCtns.IsSupportEditMode = false;
            this.numTtlCtns.Location = new System.Drawing.Point(426, 31);
            this.numTtlCtns.Name = "numTtlCtns";
            this.numTtlCtns.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTtlCtns.ReadOnly = true;
            this.numTtlCtns.Size = new System.Drawing.Size(59, 23);
            this.numTtlCtns.TabIndex = 27;
            this.numTtlCtns.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numShipQty
            // 
            this.numShipQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numShipQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ShipQty", true));
            this.numShipQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numShipQty.IsSupportEditMode = false;
            this.numShipQty.Location = new System.Drawing.Point(426, 57);
            this.numShipQty.Name = "numShipQty";
            this.numShipQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numShipQty.ReadOnly = true;
            this.numShipQty.Size = new System.Drawing.Size(59, 23);
            this.numShipQty.TabIndex = 28;
            this.numShipQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numTtlCBM
            // 
            this.numTtlCBM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTtlCBM.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CBM", true));
            this.numTtlCBM.DecimalPlaces = 3;
            this.numTtlCBM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTtlCBM.IsSupportEditMode = false;
            this.numTtlCBM.Location = new System.Drawing.Point(426, 83);
            this.numTtlCBM.Name = "numTtlCBM";
            this.numTtlCBM.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTtlCBM.ReadOnly = true;
            this.numTtlCBM.Size = new System.Drawing.Size(75, 23);
            this.numTtlCBM.TabIndex = 29;
            this.numTtlCBM.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // displayPurchaseCtn
            // 
            this.displayPurchaseCtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPurchaseCtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPurchaseCtn.Location = new System.Drawing.Point(652, 228);
            this.displayPurchaseCtn.Name = "displayPurchaseCtn";
            this.displayPurchaseCtn.Size = new System.Drawing.Size(27, 23);
            this.displayPurchaseCtn.TabIndex = 30;
            // 
            // labelPullOutDate
            // 
            this.labelPullOutDate.Location = new System.Drawing.Point(519, 6);
            this.labelPullOutDate.Name = "labelPullOutDate";
            this.labelPullOutDate.Size = new System.Drawing.Size(127, 22);
            this.labelPullOutDate.TabIndex = 32;
            this.labelPullOutDate.Text = "Pull-Out Date";
            // 
            // labelInvoiceNo
            // 
            this.labelInvoiceNo.Location = new System.Drawing.Point(519, 57);
            this.labelInvoiceNo.Name = "labelInvoiceNo";
            this.labelInvoiceNo.Size = new System.Drawing.Size(127, 22);
            this.labelInvoiceNo.TabIndex = 33;
            this.labelInvoiceNo.Text = "Invoice No.";
            // 
            // labelShipPlanNo
            // 
            this.labelShipPlanNo.Location = new System.Drawing.Point(520, 83);
            this.labelShipPlanNo.Name = "labelShipPlanNo";
            this.labelShipPlanNo.Size = new System.Drawing.Size(127, 22);
            this.labelShipPlanNo.TabIndex = 34;
            this.labelShipPlanNo.Text = "Ship Plan No.";
            // 
            // labelCartonEstBooking
            // 
            this.labelCartonEstBooking.Location = new System.Drawing.Point(520, 170);
            this.labelCartonEstBooking.Name = "labelCartonEstBooking";
            this.labelCartonEstBooking.Size = new System.Drawing.Size(127, 22);
            this.labelCartonEstBooking.TabIndex = 35;
            this.labelCartonEstBooking.Text = "Carton Est. Booking";
            // 
            // labelCartonEstArrived
            // 
            this.labelCartonEstArrived.Location = new System.Drawing.Point(520, 199);
            this.labelCartonEstArrived.Name = "labelCartonEstArrived";
            this.labelCartonEstArrived.Size = new System.Drawing.Size(127, 22);
            this.labelCartonEstArrived.TabIndex = 36;
            this.labelCartonEstArrived.Text = "Carton Est. Arrived";
            // 
            // datePullOutDate
            // 
            this.datePullOutDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PulloutDate", true));
            this.datePullOutDate.IsSupportEditMode = false;
            this.datePullOutDate.Location = new System.Drawing.Point(650, 6);
            this.datePullOutDate.Name = "datePullOutDate";
            this.datePullOutDate.ReadOnly = true;
            this.datePullOutDate.Size = new System.Drawing.Size(130, 23);
            this.datePullOutDate.TabIndex = 38;
            // 
            // displayInvoiceNo
            // 
            this.displayInvoiceNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayInvoiceNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "INVNo", true));
            this.displayInvoiceNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayInvoiceNo.Location = new System.Drawing.Point(650, 57);
            this.displayInvoiceNo.Name = "displayInvoiceNo";
            this.displayInvoiceNo.Size = new System.Drawing.Size(160, 23);
            this.displayInvoiceNo.TabIndex = 39;
            // 
            // displayShipPlanNo
            // 
            this.displayShipPlanNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayShipPlanNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ShipPlanID", true));
            this.displayShipPlanNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayShipPlanNo.Location = new System.Drawing.Point(651, 83);
            this.displayShipPlanNo.Name = "displayShipPlanNo";
            this.displayShipPlanNo.Size = new System.Drawing.Size(120, 23);
            this.displayShipPlanNo.TabIndex = 40;
            // 
            // dateCartonEstBooking
            // 
            this.dateCartonEstBooking.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "EstCTNBooking", true));
            this.dateCartonEstBooking.Location = new System.Drawing.Point(651, 170);
            this.dateCartonEstBooking.Name = "dateCartonEstBooking";
            this.dateCartonEstBooking.Size = new System.Drawing.Size(130, 23);
            this.dateCartonEstBooking.TabIndex = 6;
            // 
            // dateCartonEstArrived
            // 
            this.dateCartonEstArrived.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "EstCTNArrive", true));
            this.dateCartonEstArrived.Location = new System.Drawing.Point(651, 199);
            this.dateCartonEstArrived.Name = "dateCartonEstArrived";
            this.dateCartonEstArrived.Size = new System.Drawing.Size(130, 23);
            this.dateCartonEstArrived.TabIndex = 7;
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(83, 140);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(429, 96);
            this.editRemark.TabIndex = 4;
            // 
            // btnCartonSummary
            // 
            this.btnCartonSummary.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnCartonSummary.Location = new System.Drawing.Point(815, 28);
            this.btnCartonSummary.Name = "btnCartonSummary";
            this.btnCartonSummary.Size = new System.Drawing.Size(178, 30);
            this.btnCartonSummary.TabIndex = 48;
            this.btnCartonSummary.Text = "Carton Summary";
            this.btnCartonSummary.UseVisualStyleBackColor = true;
            this.btnCartonSummary.Click += new System.EventHandler(this.BtnCartonSummary_Click);
            // 
            // btnRecalculateWeight
            // 
            this.btnRecalculateWeight.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnRecalculateWeight.Location = new System.Drawing.Point(815, 60);
            this.btnRecalculateWeight.Name = "btnRecalculateWeight";
            this.btnRecalculateWeight.Size = new System.Drawing.Size(179, 30);
            this.btnRecalculateWeight.TabIndex = 49;
            this.btnRecalculateWeight.Text = "Recalculate Weight";
            this.btnRecalculateWeight.UseVisualStyleBackColor = true;
            this.btnRecalculateWeight.Click += new System.EventHandler(this.BtnRecalculateWeight_Click);
            // 
            // btnUnConfirmHistory
            // 
            this.btnUnConfirmHistory.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnUnConfirmHistory.Location = new System.Drawing.Point(815, 92);
            this.btnUnConfirmHistory.Name = "btnUnConfirmHistory";
            this.btnUnConfirmHistory.Size = new System.Drawing.Size(179, 30);
            this.btnUnConfirmHistory.TabIndex = 50;
            this.btnUnConfirmHistory.Text = "UnConfirm History";
            this.btnUnConfirmHistory.UseVisualStyleBackColor = true;
            this.btnUnConfirmHistory.Click += new System.EventHandler(this.BtnUnConfirmHistory_Click);
            // 
            // labelLocateforTransferClog
            // 
            this.labelLocateforTransferClog.Location = new System.Drawing.Point(289, 261);
            this.labelLocateforTransferClog.Name = "labelLocateforTransferClog";
            this.labelLocateforTransferClog.Size = new System.Drawing.Size(156, 23);
            this.labelLocateforTransferClog.TabIndex = 51;
            this.labelLocateforTransferClog.Text = "Locate for Transfer Clog:";
            // 
            // dateLocateforTransferClog
            // 
            this.dateLocateforTransferClog.IsSupportEditMode = false;
            this.dateLocateforTransferClog.Location = new System.Drawing.Point(450, 261);
            this.dateLocateforTransferClog.Name = "dateLocateforTransferClog";
            this.dateLocateforTransferClog.Size = new System.Drawing.Size(130, 23);
            this.dateLocateforTransferClog.TabIndex = 52;
            // 
            // txtLocateforTransferClog
            // 
            this.txtLocateforTransferClog.BackColor = System.Drawing.Color.White;
            this.txtLocateforTransferClog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateforTransferClog.IsSupportEditMode = false;
            this.txtLocateforTransferClog.Location = new System.Drawing.Point(450, 261);
            this.txtLocateforTransferClog.Name = "txtLocateforTransferClog";
            this.txtLocateforTransferClog.Size = new System.Drawing.Size(80, 23);
            this.txtLocateforTransferClog.TabIndex = 53;
            // 
            // btnFindNow
            // 
            this.btnFindNow.Location = new System.Drawing.Point(592, 256);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(80, 30);
            this.btnFindNow.TabIndex = 54;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(4, 31);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(75, 22);
            this.labelBrand.TabIndex = 55;
            this.labelBrand.Text = "Brand";
            // 
            // labelCustCD
            // 
            this.labelCustCD.Location = new System.Drawing.Point(5, 57);
            this.labelCustCD.Name = "labelCustCD";
            this.labelCustCD.Size = new System.Drawing.Size(75, 22);
            this.labelCustCD.TabIndex = 56;
            this.labelCustCD.Text = "CustCD";
            // 
            // labelDestination
            // 
            this.labelDestination.Location = new System.Drawing.Point(4, 83);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(75, 22);
            this.labelDestination.TabIndex = 57;
            this.labelDestination.Text = "Destination";
            // 
            // labelCofirmed
            // 
            this.labelCofirmed.BackColor = System.Drawing.Color.Transparent;
            this.labelCofirmed.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.labelCofirmed.Location = new System.Drawing.Point(839, 0);
            this.labelCofirmed.Name = "labelCofirmed";
            this.labelCofirmed.Size = new System.Drawing.Size(161, 26);
            this.labelCofirmed.TabIndex = 58;
            this.labelCofirmed.Text = "Shipping Lock";
            this.labelCofirmed.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelCofirmed.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.labelCofirmed.TextStyle.Color = System.Drawing.Color.Red;
            this.labelCofirmed.Visible = false;
            // 
            // btnDownloadSampleFile
            // 
            this.btnDownloadSampleFile.Location = new System.Drawing.Point(815, 156);
            this.btnDownloadSampleFile.Name = "btnDownloadSampleFile";
            this.btnDownloadSampleFile.Size = new System.Drawing.Size(179, 30);
            this.btnDownloadSampleFile.TabIndex = 59;
            this.btnDownloadSampleFile.Text = "Download Sample File";
            this.btnDownloadSampleFile.UseVisualStyleBackColor = true;
            this.btnDownloadSampleFile.Visible = false;
            this.btnDownloadSampleFile.Click += new System.EventHandler(this.BtnDownloadSample_Click);
            // 
            // btnImportFromExcel
            // 
            this.btnImportFromExcel.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImportFromExcel.Location = new System.Drawing.Point(815, 188);
            this.btnImportFromExcel.Name = "btnImportFromExcel";
            this.btnImportFromExcel.Size = new System.Drawing.Size(179, 30);
            this.btnImportFromExcel.TabIndex = 60;
            this.btnImportFromExcel.Text = "Import From Excel";
            this.btnImportFromExcel.UseVisualStyleBackColor = true;
            this.btnImportFromExcel.Visible = false;
            this.btnImportFromExcel.Click += new System.EventHandler(this.BtnImportFromExcel_Click);
            // 
            // txtcountry
            // 
            this.txtcountry.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Dest", true));
            this.txtcountry.DisplayBox1Binding = "";
            this.txtcountry.Location = new System.Drawing.Point(83, 83);
            this.txtcountry.Name = "txtcountry";
            this.txtcountry.Size = new System.Drawing.Size(232, 22);
            this.txtcountry.TabIndex = 2;
            this.txtcountry.TextBox1Binding = "";
            // 
            // txtcustcd
            // 
            this.txtcustcd.BackColor = System.Drawing.Color.White;
            this.txtcustcd.BrandObjectName = this.txtbrand;
            this.txtcustcd.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CustCDID", true));
            this.txtcustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd.Location = new System.Drawing.Point(84, 57);
            this.txtcustcd.Name = "txtcustcd";
            this.txtcustcd.Size = new System.Drawing.Size(125, 23);
            this.txtcustcd.TabIndex = 1;
            this.txtcustcd.Validated += new System.EventHandler(this.Txtcustcd_Validated);
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(83, 31);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(78, 23);
            this.txtbrand.TabIndex = 0;
            this.txtbrand.Validated += new System.EventHandler(this.Txtbrand_Validated);
            // 
            // txtshipmode
            // 
            this.txtshipmode.BackColor = System.Drawing.Color.White;
            this.txtshipmode.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "ShipModeID", true));
            this.txtshipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshipmode.FormattingEnabled = true;
            this.txtshipmode.IsSupportUnselect = true;
            this.txtshipmode.Location = new System.Drawing.Point(83, 111);
            this.txtshipmode.Name = "txtshipmode";
            this.txtshipmode.OldText = "";
            this.txtshipmode.Size = new System.Drawing.Size(121, 24);
            this.txtshipmode.TabIndex = 3;
            this.txtshipmode.UseFunction = "ORDER";
            this.txtshipmode.Validated += new System.EventHandler(this.Txtshipmode_Validated);
            // 
            // numTtlGW
            // 
            this.numTtlGW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTtlGW.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "gw", true));
            this.numTtlGW.DecimalPlaces = 3;
            this.numTtlGW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTtlGW.IsSupportEditMode = false;
            this.numTtlGW.Location = new System.Drawing.Point(426, 111);
            this.numTtlGW.Name = "numTtlGW";
            this.numTtlGW.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTtlGW.ReadOnly = true;
            this.numTtlGW.Size = new System.Drawing.Size(75, 23);
            this.numTtlGW.TabIndex = 62;
            this.numTtlGW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelTtlGW
            // 
            this.labelTtlGW.Location = new System.Drawing.Point(332, 111);
            this.labelTtlGW.Name = "labelTtlGW";
            this.labelTtlGW.Size = new System.Drawing.Size(90, 22);
            this.labelTtlGW.TabIndex = 61;
            this.labelTtlGW.Text = "Ttl GW";
            // 
            // labelPullOutNo
            // 
            this.labelPullOutNo.Location = new System.Drawing.Point(519, 31);
            this.labelPullOutNo.Name = "labelPullOutNo";
            this.labelPullOutNo.Size = new System.Drawing.Size(127, 22);
            this.labelPullOutNo.TabIndex = 32;
            this.labelPullOutNo.Text = "Pull-out No.";
            // 
            // displayBoxPullOutNo
            // 
            this.displayBoxPullOutNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxPullOutNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PulloutID", true));
            this.displayBoxPullOutNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxPullOutNo.Location = new System.Drawing.Point(650, 30);
            this.displayBoxPullOutNo.Name = "displayBoxPullOutNo";
            this.displayBoxPullOutNo.Size = new System.Drawing.Size(160, 23);
            this.displayBoxPullOutNo.TabIndex = 39;
            // 
            // btnUpdateBarcode
            // 
            this.btnUpdateBarcode.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnUpdateBarcode.Location = new System.Drawing.Point(815, 124);
            this.btnUpdateBarcode.Name = "btnUpdateBarcode";
            this.btnUpdateBarcode.Size = new System.Drawing.Size(179, 30);
            this.btnUpdateBarcode.TabIndex = 63;
            this.btnUpdateBarcode.Text = "Update Barcode";
            this.btnUpdateBarcode.UseVisualStyleBackColor = true;
            this.btnUpdateBarcode.Click += new System.EventHandler(this.BtnUpdateBarcode_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(519, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 22);
            this.label1.TabIndex = 64;
            this.label1.Text = "SCI Dlv.";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(520, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 22);
            this.label2.TabIndex = 65;
            this.label2.Text = "KPI L/ETA";
            // 
            // datesciDelivery
            // 
            this.datesciDelivery.IsSupportEditMode = false;
            this.datesciDelivery.Location = new System.Drawing.Point(652, 110);
            this.datesciDelivery.Name = "datesciDelivery";
            this.datesciDelivery.ReadOnly = true;
            this.datesciDelivery.Size = new System.Drawing.Size(130, 23);
            this.datesciDelivery.TabIndex = 66;
            // 
            // datekpileta
            // 
            this.datekpileta.IsSupportEditMode = false;
            this.datekpileta.Location = new System.Drawing.Point(652, 139);
            this.datekpileta.Name = "datekpileta";
            this.datekpileta.ReadOnly = true;
            this.datekpileta.Size = new System.Drawing.Size(130, 23);
            this.datekpileta.TabIndex = 7;
            // 
            // P03
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1008, 618);
            this.DefaultControl = "txtbrand";
            this.DefaultControlForEdit = "txtbrand";
            this.DefaultDetailOrder = "Seq";
            this.DefaultOrder = "ID";
            this.ExpressQuery = true;
            this.GridAlias = "PackingList_Detail";
            this.GridNew = 0;
            this.GridUniqueKey = "ID,OrderID,OrderShipmodeSeq,CTNStartNo,Article,SizeCode";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P03";
            this.Text = "P03. Packing List Weight & Summary(Bulk)";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "ID";
            this.WorkAlias = "PackingList";
            this.WorkView = "View_PackingP03";
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).EndInit();
            this.masterpanel.ResumeLayout(false);
            this.masterpanel.PerformLayout();
            this.detailpanel.ResumeLayout(false);
            this.detail2.ResumeLayout(false);
            this.detailpanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DisplayBox displayNo;
        private Win.UI.Label labelSortby;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelNo;
        private Win.UI.ComboBox comboSortby;
        private Class.txtshipmode txtshipmode;
        private Win.UI.Label labelPurchaseCtn;
        private Win.UI.Label labelTtlCBM;
        private Win.UI.Label labelShipQty;
        private Win.UI.Label labelTtlCtns;
        private Win.UI.Label labelStartCtn;
        private Win.UI.Label labelShipMode;
        private Win.UI.DateBox dateCartonEstArrived;
        private Win.UI.DateBox dateCartonEstBooking;
        private Win.UI.DisplayBox displayShipPlanNo;
        private Win.UI.DisplayBox displayInvoiceNo;
        private Win.UI.DateBox datePullOutDate;
        private Win.UI.Label labelCartonEstArrived;
        private Win.UI.Label labelCartonEstBooking;
        private Win.UI.Label labelShipPlanNo;
        private Win.UI.Label labelInvoiceNo;
        private Win.UI.Label labelPullOutDate;
        private Win.UI.DisplayBox displayPurchaseCtn;
        private Win.UI.NumericBox numTtlCBM;
        private Win.UI.NumericBox numShipQty;
        private Win.UI.NumericBox numTtlCtns;
        private Win.UI.DisplayBox displayStartCtn;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtLocateforTransferClog;
        private Win.UI.DateBox dateLocateforTransferClog;
        private Win.UI.Label labelLocateforTransferClog;
        private Win.UI.Button btnUnConfirmHistory;
        private Win.UI.Button btnRecalculateWeight;
        private Win.UI.Button btnCartonSummary;
        private Win.UI.EditBox editRemark;
        private Class.txtcountry txtcountry;
        private Class.txtcustcd txtcustcd;
        private Class.txtbrand txtbrand;
        private Win.UI.Label labelDestination;
        private Win.UI.Label labelCustCD;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelCofirmed;
        private Win.UI.Button btnImportFromExcel;
        private Win.UI.Button btnDownloadSampleFile;
        private Win.UI.NumericBox numTtlGW;
        private Win.UI.Label labelTtlGW;
        private Win.UI.DisplayBox displayBoxPullOutNo;
        private Win.UI.Label labelPullOutNo;
        private Win.UI.Button btnUpdateBarcode;
        private Win.UI.DateBox datekpileta;
        private Win.UI.DateBox datesciDelivery;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
    }
}
