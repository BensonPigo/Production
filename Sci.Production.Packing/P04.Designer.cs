namespace Sci.Production.Packing
{
    partial class P04
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
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelCustCD = new Sci.Win.UI.Label();
            this.labelDestination = new Sci.Win.UI.Label();
            this.labelTtlCtn = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.displayNo = new Sci.Win.UI.DisplayBox();
            this.txtbrand = new Sci.Production.Class.txtbrand();
            this.txtcustcd = new Sci.Production.Class.txtcustcd();
            this.txtcountry = new Sci.Production.Class.txtcountry();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelShipMode = new Sci.Win.UI.Label();
            this.labelCartonEstBooking = new Sci.Win.UI.Label();
            this.labelCartonEstArrived = new Sci.Win.UI.Label();
            this.labelPurchaseCTN = new Sci.Win.UI.Label();
            this.txtshipmode = new Sci.Production.Class.txtshipmode();
            this.dateCartonEstBooking = new Sci.Win.UI.DateBox();
            this.dateCartonEstArrived = new Sci.Win.UI.DateBox();
            this.labelPullOutDate = new Sci.Win.UI.Label();
            this.labelInvoiceNo = new Sci.Win.UI.Label();
            this.labelShipPlanNo = new Sci.Win.UI.Label();
            this.labelShipQty = new Sci.Win.UI.Label();
            this.labelTtlCBM = new Sci.Win.UI.Label();
            this.datePullOutDate = new Sci.Win.UI.DateBox();
            this.displayInvoiceNo = new Sci.Win.UI.DisplayBox();
            this.displayShipPlanNo = new Sci.Win.UI.DisplayBox();
            this.numShipQty = new Sci.Win.UI.NumericBox();
            this.numTtlCBM = new Sci.Win.UI.NumericBox();
            this.btnCartonSummary = new Sci.Win.UI.Button();
            this.btnBatchImport = new Sci.Win.UI.Button();
            this.btnImportFromExcel = new Sci.Win.UI.Button();
            this.btnDownloadExcelFormat = new Sci.Win.UI.Button();
            this.btnRecalculateWeight = new Sci.Win.UI.Button();
            this.numTtlCtn = new Sci.Win.UI.NumericBox();
            this.displayPurchaseCTN = new Sci.Win.UI.DisplayBox();
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.labelConfirmed = new Sci.Win.UI.Label();
            this.numTtlGW = new Sci.Win.UI.NumericBox();
            this.labelTtlGW = new Sci.Win.UI.Label();
            this.labelPullOutNo = new Sci.Win.UI.Label();
            this.displayPullOutNo = new Sci.Win.UI.DisplayBox();
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
            this.masterpanel.Controls.Add(this.numTtlGW);
            this.masterpanel.Controls.Add(this.labelTtlGW);
            this.masterpanel.Controls.Add(this.labelConfirmed);
            this.masterpanel.Controls.Add(this.txtfactory);
            this.masterpanel.Controls.Add(this.displayPurchaseCTN);
            this.masterpanel.Controls.Add(this.btnRecalculateWeight);
            this.masterpanel.Controls.Add(this.btnDownloadExcelFormat);
            this.masterpanel.Controls.Add(this.btnImportFromExcel);
            this.masterpanel.Controls.Add(this.btnBatchImport);
            this.masterpanel.Controls.Add(this.btnCartonSummary);
            this.masterpanel.Controls.Add(this.numTtlCBM);
            this.masterpanel.Controls.Add(this.numShipQty);
            this.masterpanel.Controls.Add(this.displayShipPlanNo);
            this.masterpanel.Controls.Add(this.displayPullOutNo);
            this.masterpanel.Controls.Add(this.displayInvoiceNo);
            this.masterpanel.Controls.Add(this.datePullOutDate);
            this.masterpanel.Controls.Add(this.labelTtlCBM);
            this.masterpanel.Controls.Add(this.labelShipQty);
            this.masterpanel.Controls.Add(this.labelShipPlanNo);
            this.masterpanel.Controls.Add(this.labelInvoiceNo);
            this.masterpanel.Controls.Add(this.labelPullOutNo);
            this.masterpanel.Controls.Add(this.labelPullOutDate);
            this.masterpanel.Controls.Add(this.dateCartonEstArrived);
            this.masterpanel.Controls.Add(this.dateCartonEstBooking);
            this.masterpanel.Controls.Add(this.txtshipmode);
            this.masterpanel.Controls.Add(this.labelPurchaseCTN);
            this.masterpanel.Controls.Add(this.labelCartonEstArrived);
            this.masterpanel.Controls.Add(this.labelCartonEstBooking);
            this.masterpanel.Controls.Add(this.labelShipMode);
            this.masterpanel.Controls.Add(this.labelFactory);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.numTtlCtn);
            this.masterpanel.Controls.Add(this.txtcountry);
            this.masterpanel.Controls.Add(this.txtcustcd);
            this.masterpanel.Controls.Add(this.txtbrand);
            this.masterpanel.Controls.Add(this.displayNo);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.labelTtlCtn);
            this.masterpanel.Controls.Add(this.labelDestination);
            this.masterpanel.Controls.Add(this.labelCustCD);
            this.masterpanel.Controls.Add(this.labelBrand);
            this.masterpanel.Controls.Add(this.labelNo);
            this.masterpanel.Size = new System.Drawing.Size(992, 230);
            this.masterpanel.Controls.SetChildIndex(this.labelNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelBrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCustCD, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDestination, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTtlCtn, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtbrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtcustcd, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtcountry, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTtlCtn, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShipMode, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCartonEstBooking, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCartonEstArrived, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPurchaseCTN, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtshipmode, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateCartonEstBooking, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateCartonEstArrived, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPullOutDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPullOutNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelInvoiceNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShipPlanNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShipQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTtlCBM, 0);
            this.masterpanel.Controls.SetChildIndex(this.datePullOutDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayInvoiceNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayPullOutNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayShipPlanNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.numShipQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTtlCBM, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnCartonSummary, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnBatchImport, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportFromExcel, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnDownloadExcelFormat, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnRecalculateWeight, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayPurchaseCTN, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtfactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelConfirmed, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTtlGW, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTtlGW, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 230);
            this.detailpanel.Size = new System.Drawing.Size(992, 288);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(739, 192);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(912, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(992, 288);
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
            this.detail.Size = new System.Drawing.Size(992, 556);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(992, 518);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 518);
            this.detailbtm.Size = new System.Drawing.Size(992, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(992, 556);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1000, 585);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(472, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(424, 13);
            // 
            // labelNo
            // 
            this.labelNo.Location = new System.Drawing.Point(3, 4);
            this.labelNo.Name = "labelNo";
            this.labelNo.Size = new System.Drawing.Size(75, 23);
            this.labelNo.TabIndex = 8;
            this.labelNo.Text = "No.";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(3, 31);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(75, 23);
            this.labelBrand.TabIndex = 9;
            this.labelBrand.Text = "Brand";
            // 
            // labelCustCD
            // 
            this.labelCustCD.Location = new System.Drawing.Point(3, 58);
            this.labelCustCD.Name = "labelCustCD";
            this.labelCustCD.Size = new System.Drawing.Size(75, 23);
            this.labelCustCD.TabIndex = 10;
            this.labelCustCD.Text = "CustCD";
            // 
            // labelDestination
            // 
            this.labelDestination.Location = new System.Drawing.Point(3, 85);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(75, 23);
            this.labelDestination.TabIndex = 11;
            this.labelDestination.Text = "Destination";
            // 
            // labelTtlCtn
            // 
            this.labelTtlCtn.Location = new System.Drawing.Point(3, 112);
            this.labelTtlCtn.Name = "labelTtlCtn";
            this.labelTtlCtn.Size = new System.Drawing.Size(75, 23);
            this.labelTtlCtn.TabIndex = 12;
            this.labelTtlCtn.Text = "Ttl Ctn";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(3, 139);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 13;
            this.labelRemark.Text = "Remark";
            // 
            // displayNo
            // 
            this.displayNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayNo.Location = new System.Drawing.Point(81, 4);
            this.displayNo.Name = "displayNo";
            this.displayNo.Size = new System.Drawing.Size(125, 23);
            this.displayNo.TabIndex = 7;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(81, 31);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 1;
            // 
            // txtcustcd
            // 
            this.txtcustcd.BackColor = System.Drawing.Color.White;
            this.txtcustcd.BrandObjectName = this.txtbrand;
            this.txtcustcd.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CustCDID", true));
            this.txtcustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd.Location = new System.Drawing.Point(81, 58);
            this.txtcustcd.Name = "txtcustcd";
            this.txtcustcd.Size = new System.Drawing.Size(125, 23);
            this.txtcustcd.TabIndex = 3;
            this.txtcustcd.Validated += new System.EventHandler(this.Txtcustcd_Validated);
            // 
            // txtcountry
            // 
            this.txtcountry.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Dest", true));
            this.txtcountry.DisplayBox1Binding = "";
            this.txtcountry.Location = new System.Drawing.Point(81, 85);
            this.txtcountry.Name = "txtcountry";
            this.txtcountry.Size = new System.Drawing.Size(209, 22);
            this.txtcountry.TabIndex = 4;
            this.txtcountry.TextBox1Binding = "";
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(81, 139);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(434, 85);
            this.editRemark.TabIndex = 7;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(305, 4);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(95, 23);
            this.labelFactory.TabIndex = 13;
            this.labelFactory.Text = "Factory";
            // 
            // labelShipMode
            // 
            this.labelShipMode.Location = new System.Drawing.Point(305, 31);
            this.labelShipMode.Name = "labelShipMode";
            this.labelShipMode.Size = new System.Drawing.Size(95, 23);
            this.labelShipMode.TabIndex = 14;
            this.labelShipMode.Text = "Ship Mode";
            // 
            // labelCartonEstBooking
            // 
            this.labelCartonEstBooking.Location = new System.Drawing.Point(536, 112);
            this.labelCartonEstBooking.Name = "labelCartonEstBooking";
            this.labelCartonEstBooking.Size = new System.Drawing.Size(127, 23);
            this.labelCartonEstBooking.TabIndex = 15;
            this.labelCartonEstBooking.Text = "Carton Est. Booking";
            // 
            // labelCartonEstArrived
            // 
            this.labelCartonEstArrived.Location = new System.Drawing.Point(536, 139);
            this.labelCartonEstArrived.Name = "labelCartonEstArrived";
            this.labelCartonEstArrived.Size = new System.Drawing.Size(127, 23);
            this.labelCartonEstArrived.TabIndex = 16;
            this.labelCartonEstArrived.Text = "Carton Est. Arrived";
            // 
            // labelPurchaseCTN
            // 
            this.labelPurchaseCTN.Location = new System.Drawing.Point(536, 166);
            this.labelPurchaseCTN.Name = "labelPurchaseCTN";
            this.labelPurchaseCTN.Size = new System.Drawing.Size(127, 23);
            this.labelPurchaseCTN.TabIndex = 17;
            this.labelPurchaseCTN.Text = "Purchase CTN";
            // 
            // txtshipmode
            // 
            this.txtshipmode.BackColor = System.Drawing.Color.White;
            this.txtshipmode.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "ShipModeID", true));
            this.txtshipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshipmode.FormattingEnabled = true;
            this.txtshipmode.IsSupportUnselect = true;
            this.txtshipmode.Location = new System.Drawing.Point(403, 29);
            this.txtshipmode.Name = "txtshipmode";
            this.txtshipmode.Size = new System.Drawing.Size(121, 24);
            this.txtshipmode.TabIndex = 2;
            this.txtshipmode.UseFunction = null;
            // 
            // dateCartonEstBooking
            // 
            this.dateCartonEstBooking.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "EstCTNBooking", true));
            this.dateCartonEstBooking.Location = new System.Drawing.Point(666, 112);
            this.dateCartonEstBooking.Name = "dateCartonEstBooking";
            this.dateCartonEstBooking.Size = new System.Drawing.Size(130, 23);
            this.dateCartonEstBooking.TabIndex = 5;
            // 
            // dateCartonEstArrived
            // 
            this.dateCartonEstArrived.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "EstCTNArrive", true));
            this.dateCartonEstArrived.Location = new System.Drawing.Point(666, 139);
            this.dateCartonEstArrived.Name = "dateCartonEstArrived";
            this.dateCartonEstArrived.Size = new System.Drawing.Size(130, 23);
            this.dateCartonEstArrived.TabIndex = 6;
            // 
            // labelPullOutDate
            // 
            this.labelPullOutDate.Location = new System.Drawing.Point(536, 4);
            this.labelPullOutDate.Name = "labelPullOutDate";
            this.labelPullOutDate.Size = new System.Drawing.Size(127, 23);
            this.labelPullOutDate.TabIndex = 23;
            this.labelPullOutDate.Text = "Pull-out Date";
            // 
            // labelInvoiceNo
            // 
            this.labelInvoiceNo.Location = new System.Drawing.Point(536, 58);
            this.labelInvoiceNo.Name = "labelInvoiceNo";
            this.labelInvoiceNo.Size = new System.Drawing.Size(127, 23);
            this.labelInvoiceNo.TabIndex = 24;
            this.labelInvoiceNo.Text = "Invoice No.";
            // 
            // labelShipPlanNo
            // 
            this.labelShipPlanNo.Location = new System.Drawing.Point(536, 85);
            this.labelShipPlanNo.Name = "labelShipPlanNo";
            this.labelShipPlanNo.Size = new System.Drawing.Size(127, 23);
            this.labelShipPlanNo.TabIndex = 25;
            this.labelShipPlanNo.Text = "Ship Plan No.";
            // 
            // labelShipQty
            // 
            this.labelShipQty.Location = new System.Drawing.Point(305, 58);
            this.labelShipQty.Name = "labelShipQty";
            this.labelShipQty.Size = new System.Drawing.Size(95, 23);
            this.labelShipQty.TabIndex = 26;
            this.labelShipQty.Text = "Ship Qty";
            // 
            // labelTtlCBM
            // 
            this.labelTtlCBM.Location = new System.Drawing.Point(305, 85);
            this.labelTtlCBM.Name = "labelTtlCBM";
            this.labelTtlCBM.Size = new System.Drawing.Size(95, 23);
            this.labelTtlCBM.TabIndex = 27;
            this.labelTtlCBM.Text = "Ttl CBM";
            // 
            // datePullOutDate
            // 
            this.datePullOutDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PulloutDate", true));
            this.datePullOutDate.IsSupportEditMode = false;
            this.datePullOutDate.Location = new System.Drawing.Point(666, 4);
            this.datePullOutDate.Name = "datePullOutDate";
            this.datePullOutDate.ReadOnly = true;
            this.datePullOutDate.Size = new System.Drawing.Size(130, 23);
            this.datePullOutDate.TabIndex = 28;
            // 
            // displayInvoiceNo
            // 
            this.displayInvoiceNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayInvoiceNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "INVNo", true));
            this.displayInvoiceNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayInvoiceNo.Location = new System.Drawing.Point(666, 58);
            this.displayInvoiceNo.Name = "displayInvoiceNo";
            this.displayInvoiceNo.Size = new System.Drawing.Size(182, 23);
            this.displayInvoiceNo.TabIndex = 29;
            // 
            // displayShipPlanNo
            // 
            this.displayShipPlanNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayShipPlanNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ShipPlanID", true));
            this.displayShipPlanNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayShipPlanNo.Location = new System.Drawing.Point(666, 85);
            this.displayShipPlanNo.Name = "displayShipPlanNo";
            this.displayShipPlanNo.Size = new System.Drawing.Size(120, 23);
            this.displayShipPlanNo.TabIndex = 30;
            // 
            // numShipQty
            // 
            this.numShipQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numShipQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ShipQty", true));
            this.numShipQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numShipQty.IsSupportEditMode = false;
            this.numShipQty.Location = new System.Drawing.Point(403, 58);
            this.numShipQty.Name = "numShipQty";
            this.numShipQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numShipQty.ReadOnly = true;
            this.numShipQty.Size = new System.Drawing.Size(59, 23);
            this.numShipQty.TabIndex = 31;
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
            this.numTtlCBM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTtlCBM.IsSupportEditMode = false;
            this.numTtlCBM.Location = new System.Drawing.Point(403, 84);
            this.numTtlCBM.Name = "numTtlCBM";
            this.numTtlCBM.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTtlCBM.ReadOnly = true;
            this.numTtlCBM.Size = new System.Drawing.Size(75, 23);
            this.numTtlCBM.TabIndex = 32;
            this.numTtlCBM.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // btnCartonSummary
            // 
            this.btnCartonSummary.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnCartonSummary.Location = new System.Drawing.Point(847, 71);
            this.btnCartonSummary.Name = "btnCartonSummary";
            this.btnCartonSummary.Size = new System.Drawing.Size(145, 30);
            this.btnCartonSummary.TabIndex = 49;
            this.btnCartonSummary.Text = "Carton Summary";
            this.btnCartonSummary.UseVisualStyleBackColor = true;
            this.btnCartonSummary.Click += new System.EventHandler(this.BtnCartonSummary_Click);
            // 
            // btnBatchImport
            // 
            this.btnBatchImport.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnBatchImport.Location = new System.Drawing.Point(847, 104);
            this.btnBatchImport.Name = "btnBatchImport";
            this.btnBatchImport.Size = new System.Drawing.Size(145, 30);
            this.btnBatchImport.TabIndex = 50;
            this.btnBatchImport.Text = "Batch Import";
            this.btnBatchImport.UseVisualStyleBackColor = true;
            this.btnBatchImport.Click += new System.EventHandler(this.BtnBatchImport_Click);
            // 
            // btnImportFromExcel
            // 
            this.btnImportFromExcel.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImportFromExcel.Location = new System.Drawing.Point(847, 137);
            this.btnImportFromExcel.Name = "btnImportFromExcel";
            this.btnImportFromExcel.Size = new System.Drawing.Size(145, 30);
            this.btnImportFromExcel.TabIndex = 51;
            this.btnImportFromExcel.Text = "Import from excel";
            this.btnImportFromExcel.UseVisualStyleBackColor = true;
            this.btnImportFromExcel.Click += new System.EventHandler(this.BtnImportFromExcel_Click);
            // 
            // btnDownloadExcelFormat
            // 
            this.btnDownloadExcelFormat.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnDownloadExcelFormat.Location = new System.Drawing.Point(847, 169);
            this.btnDownloadExcelFormat.Name = "btnDownloadExcelFormat";
            this.btnDownloadExcelFormat.Size = new System.Drawing.Size(145, 55);
            this.btnDownloadExcelFormat.TabIndex = 52;
            this.btnDownloadExcelFormat.Text = "Download excel format";
            this.btnDownloadExcelFormat.UseVisualStyleBackColor = true;
            this.btnDownloadExcelFormat.Click += new System.EventHandler(this.BtnDownloadExcelFormat_Click);
            // 
            // btnRecalculateWeight
            // 
            this.btnRecalculateWeight.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnRecalculateWeight.Location = new System.Drawing.Point(536, 196);
            this.btnRecalculateWeight.Name = "btnRecalculateWeight";
            this.btnRecalculateWeight.Size = new System.Drawing.Size(148, 30);
            this.btnRecalculateWeight.TabIndex = 53;
            this.btnRecalculateWeight.Text = "Recalculate Weight";
            this.btnRecalculateWeight.UseVisualStyleBackColor = true;
            this.btnRecalculateWeight.Click += new System.EventHandler(this.BtnRecalculateWeight_Click);
            // 
            // numTtlCtn
            // 
            this.numTtlCtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTtlCtn.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CTNQty", true));
            this.numTtlCtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTtlCtn.IsSupportEditMode = false;
            this.numTtlCtn.Location = new System.Drawing.Point(81, 112);
            this.numTtlCtn.Name = "numTtlCtn";
            this.numTtlCtn.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTtlCtn.ReadOnly = true;
            this.numTtlCtn.Size = new System.Drawing.Size(59, 23);
            this.numTtlCtn.TabIndex = 11;
            this.numTtlCtn.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // displayPurchaseCTN
            // 
            this.displayPurchaseCTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPurchaseCTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPurchaseCTN.Location = new System.Drawing.Point(666, 166);
            this.displayPurchaseCTN.Name = "displayPurchaseCTN";
            this.displayPurchaseCTN.Size = new System.Drawing.Size(27, 23);
            this.displayPurchaseCTN.TabIndex = 54;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FactoryID", true));
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(403, 4);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 0;
            // 
            // labelConfirmed
            // 
            this.labelConfirmed.BackColor = System.Drawing.Color.Transparent;
            this.labelConfirmed.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.labelConfirmed.Location = new System.Drawing.Point(839, 28);
            this.labelConfirmed.Name = "labelConfirmed";
            this.labelConfirmed.Size = new System.Drawing.Size(148, 25);
            this.labelConfirmed.TabIndex = 55;
            this.labelConfirmed.Text = "status";
            this.labelConfirmed.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelConfirmed.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.labelConfirmed.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // numTtlGW
            // 
            this.numTtlGW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTtlGW.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "gw", true));
            this.numTtlGW.DecimalPlaces = 3;
            this.numTtlGW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTtlGW.IsSupportEditMode = false;
            this.numTtlGW.Location = new System.Drawing.Point(403, 114);
            this.numTtlGW.Name = "numTtlGW";
            this.numTtlGW.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTtlGW.ReadOnly = true;
            this.numTtlGW.Size = new System.Drawing.Size(75, 23);
            this.numTtlGW.TabIndex = 64;
            this.numTtlGW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelTtlGW
            // 
            this.labelTtlGW.Location = new System.Drawing.Point(305, 114);
            this.labelTtlGW.Name = "labelTtlGW";
            this.labelTtlGW.Size = new System.Drawing.Size(95, 22);
            this.labelTtlGW.TabIndex = 63;
            this.labelTtlGW.Text = "Ttl GW";
            // 
            // labelPullOutNo
            // 
            this.labelPullOutNo.Location = new System.Drawing.Point(536, 30);
            this.labelPullOutNo.Name = "labelPullOutNo";
            this.labelPullOutNo.Size = new System.Drawing.Size(127, 23);
            this.labelPullOutNo.TabIndex = 23;
            this.labelPullOutNo.Text = "Pull-out No.";
            // 
            // displayPullOutNo
            // 
            this.displayPullOutNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPullOutNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PulloutID", true));
            this.displayPullOutNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPullOutNo.Location = new System.Drawing.Point(666, 30);
            this.displayPullOutNo.Name = "displayPullOutNo";
            this.displayPullOutNo.Size = new System.Drawing.Size(182, 23);
            this.displayPullOutNo.TabIndex = 29;
            // 
            // P04
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1000, 618);
            this.DefaultControl = "txtfactory";
            this.DefaultControlForEdit = "txtfactory";
            this.DefaultDetailOrder = "Seq";
            this.DefaultOrder = "ID";
            this.GridAlias = "PackingList_Detail";
            this.GridNew = 0;
            this.GridUniqueKey = "ID,OrderID,OrderShipmodeSeq,CTNStartNo,Article,SizeCode";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P04";
            this.Text = "P04. Packing List Weight & Summary(Sample)";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "ID";
            this.WorkAlias = "PackingList";
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

        private Win.UI.Label labelRemark;
        private Win.UI.Label labelTtlCtn;
        private Win.UI.Label labelDestination;
        private Win.UI.Label labelCustCD;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelNo;
        private Class.txtcountry txtcountry;
        private Class.txtcustcd txtcustcd;
        private Class.txtbrand txtbrand;
        private Win.UI.DisplayBox displayNo;
        private Win.UI.Label labelPurchaseCTN;
        private Win.UI.Label labelCartonEstArrived;
        private Win.UI.Label labelCartonEstBooking;
        private Win.UI.Label labelShipMode;
        private Win.UI.Label labelFactory;
        private Win.UI.EditBox editRemark;
        private Win.UI.NumericBox numTtlCBM;
        private Win.UI.NumericBox numShipQty;
        private Win.UI.DisplayBox displayShipPlanNo;
        private Win.UI.DisplayBox displayInvoiceNo;
        private Win.UI.DateBox datePullOutDate;
        private Win.UI.Label labelTtlCBM;
        private Win.UI.Label labelShipQty;
        private Win.UI.Label labelShipPlanNo;
        private Win.UI.Label labelInvoiceNo;
        private Win.UI.Label labelPullOutDate;
        private Win.UI.DateBox dateCartonEstArrived;
        private Win.UI.DateBox dateCartonEstBooking;
        private Class.txtshipmode txtshipmode;
        private Win.UI.Button btnRecalculateWeight;
        private Win.UI.Button btnDownloadExcelFormat;
        private Win.UI.Button btnImportFromExcel;
        private Win.UI.Button btnBatchImport;
        private Win.UI.Button btnCartonSummary;
        private Win.UI.DisplayBox displayPurchaseCTN;
        private Win.UI.NumericBox numTtlCtn;
        private Class.txtfactory txtfactory;
        private Win.UI.Label labelConfirmed;
        private Win.UI.NumericBox numTtlGW;
        private Win.UI.Label labelTtlGW;
        private Win.UI.DisplayBox displayPullOutNo;
        private Win.UI.Label labelPullOutNo;
    }
}
