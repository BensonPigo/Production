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
            this.components = new System.ComponentModel.Container();
            this.labelNo = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelCustCD = new Sci.Win.UI.Label();
            this.labelDestination = new Sci.Win.UI.Label();
            this.labelTtlCtn = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.displayNo = new Sci.Win.UI.DisplayBox();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelShipMode = new Sci.Win.UI.Label();
            this.labelCartonEstBooking = new Sci.Win.UI.Label();
            this.labelCartonEstArrived = new Sci.Win.UI.Label();
            this.labelPurchaseCTN = new Sci.Win.UI.Label();
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
            this.labelConfirmed = new Sci.Win.UI.Label();
            this.numTtlGW = new Sci.Win.UI.NumericBox();
            this.labelTtlGW = new Sci.Win.UI.Label();
            this.labelPullOutNo = new Sci.Win.UI.Label();
            this.displayPullOutNo = new Sci.Win.UI.DisplayBox();
            this.numAppEstAmtVW = new Sci.Win.UI.NumericBox();
            this.labAppEstAmtVW = new Sci.Win.UI.Label();
            this.numAppBookingVW = new Sci.Win.UI.NumericBox();
            this.labAppBookingVW = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.displayHC = new Sci.Win.UI.DisplayBox();
            this.label2 = new Sci.Win.UI.Label();
            this.dateBox1 = new Sci.Win.UI.DateBox();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtshipmode = new Sci.Production.Class.Txtshipmode();
            this.txtcountry = new Sci.Production.Class.Txtcountry();
            this.txtcustcd = new Sci.Production.Class.Txtcustcd();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.comboCompany1 = new Sci.Production.Class.ComboCompany(this.components);
            this.label5 = new Sci.Win.UI.Label();
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
            this.masterpanel.Controls.Add(this.comboCompany1);
            this.masterpanel.Controls.Add(this.label5);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.displayHC);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Controls.Add(this.numAppEstAmtVW);
            this.masterpanel.Controls.Add(this.labAppEstAmtVW);
            this.masterpanel.Controls.Add(this.numAppBookingVW);
            this.masterpanel.Controls.Add(this.labAppBookingVW);
            this.masterpanel.Controls.Add(this.numTtlGW);
            this.masterpanel.Controls.Add(this.labelTtlGW);
            this.masterpanel.Controls.Add(this.labelConfirmed);
            this.masterpanel.Controls.Add(this.txtfactory);
            this.masterpanel.Controls.Add(this.displayPurchaseCTN);
            this.masterpanel.Controls.Add(this.dateBox1);
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
            this.masterpanel.Size = new System.Drawing.Size(1084, 274);
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
            this.masterpanel.Controls.SetChildIndex(this.dateBox1, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayPurchaseCTN, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtfactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelConfirmed, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTtlGW, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTtlGW, 0);
            this.masterpanel.Controls.SetChildIndex(this.labAppBookingVW, 0);
            this.masterpanel.Controls.SetChildIndex(this.numAppBookingVW, 0);
            this.masterpanel.Controls.SetChildIndex(this.labAppEstAmtVW, 0);
            this.masterpanel.Controls.SetChildIndex(this.numAppEstAmtVW, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayHC, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.label5, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboCompany1, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 274);
            this.detailpanel.Size = new System.Drawing.Size(1084, 244);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(970, 234);
            this.gridicon.TabIndex = 24;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(912, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1084, 244);
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
            this.detail.Size = new System.Drawing.Size(1084, 556);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1084, 518);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 518);
            this.detailbtm.Size = new System.Drawing.Size(1084, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1084, 556);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1092, 585);
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
            this.labelNo.Size = new System.Drawing.Size(141, 23);
            this.labelNo.TabIndex = 8;
            this.labelNo.Text = "No.";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(3, 54);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(141, 23);
            this.labelBrand.TabIndex = 9;
            this.labelBrand.Text = "Brand";
            // 
            // labelCustCD
            // 
            this.labelCustCD.Location = new System.Drawing.Point(3, 79);
            this.labelCustCD.Name = "labelCustCD";
            this.labelCustCD.Size = new System.Drawing.Size(141, 23);
            this.labelCustCD.TabIndex = 10;
            this.labelCustCD.Text = "CustCD";
            // 
            // labelDestination
            // 
            this.labelDestination.Location = new System.Drawing.Point(3, 104);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(141, 23);
            this.labelDestination.TabIndex = 11;
            this.labelDestination.Text = "Destination";
            // 
            // labelTtlCtn
            // 
            this.labelTtlCtn.Location = new System.Drawing.Point(3, 129);
            this.labelTtlCtn.Name = "labelTtlCtn";
            this.labelTtlCtn.Size = new System.Drawing.Size(141, 23);
            this.labelTtlCtn.TabIndex = 12;
            this.labelTtlCtn.Text = "Ttl Ctn";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(3, 179);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(141, 23);
            this.labelRemark.TabIndex = 13;
            this.labelRemark.Text = "Remark";
            // 
            // displayNo
            // 
            this.displayNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayNo.Location = new System.Drawing.Point(147, 4);
            this.displayNo.Name = "displayNo";
            this.displayNo.Size = new System.Drawing.Size(125, 23);
            this.displayNo.TabIndex = 1;
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(147, 179);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(462, 85);
            this.editRemark.TabIndex = 7;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(363, 4);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(95, 23);
            this.labelFactory.TabIndex = 13;
            this.labelFactory.Text = "Factory";
            // 
            // labelShipMode
            // 
            this.labelShipMode.Location = new System.Drawing.Point(363, 29);
            this.labelShipMode.Name = "labelShipMode";
            this.labelShipMode.Size = new System.Drawing.Size(95, 23);
            this.labelShipMode.TabIndex = 14;
            this.labelShipMode.Text = "Ship Mode";
            // 
            // labelCartonEstBooking
            // 
            this.labelCartonEstBooking.Location = new System.Drawing.Point(615, 129);
            this.labelCartonEstBooking.Name = "labelCartonEstBooking";
            this.labelCartonEstBooking.Size = new System.Drawing.Size(127, 23);
            this.labelCartonEstBooking.TabIndex = 15;
            this.labelCartonEstBooking.Text = "Carton Est. Booking";
            // 
            // labelCartonEstArrived
            // 
            this.labelCartonEstArrived.Location = new System.Drawing.Point(615, 154);
            this.labelCartonEstArrived.Name = "labelCartonEstArrived";
            this.labelCartonEstArrived.Size = new System.Drawing.Size(127, 23);
            this.labelCartonEstArrived.TabIndex = 16;
            this.labelCartonEstArrived.Text = "Carton Est. Arrived";
            // 
            // labelPurchaseCTN
            // 
            this.labelPurchaseCTN.Location = new System.Drawing.Point(615, 179);
            this.labelPurchaseCTN.Name = "labelPurchaseCTN";
            this.labelPurchaseCTN.Size = new System.Drawing.Size(127, 23);
            this.labelPurchaseCTN.TabIndex = 17;
            this.labelPurchaseCTN.Text = "Purchase CTN";
            // 
            // dateCartonEstBooking
            // 
            this.dateCartonEstBooking.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "EstCTNBooking", true));
            this.dateCartonEstBooking.Location = new System.Drawing.Point(745, 129);
            this.dateCartonEstBooking.Name = "dateCartonEstBooking";
            this.dateCartonEstBooking.Size = new System.Drawing.Size(130, 23);
            this.dateCartonEstBooking.TabIndex = 19;
            // 
            // dateCartonEstArrived
            // 
            this.dateCartonEstArrived.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "EstCTNArrive", true));
            this.dateCartonEstArrived.Location = new System.Drawing.Point(745, 154);
            this.dateCartonEstArrived.Name = "dateCartonEstArrived";
            this.dateCartonEstArrived.Size = new System.Drawing.Size(130, 23);
            this.dateCartonEstArrived.TabIndex = 20;
            // 
            // labelPullOutDate
            // 
            this.labelPullOutDate.Location = new System.Drawing.Point(615, 4);
            this.labelPullOutDate.Name = "labelPullOutDate";
            this.labelPullOutDate.Size = new System.Drawing.Size(127, 23);
            this.labelPullOutDate.TabIndex = 23;
            this.labelPullOutDate.Text = "Pull-out Date";
            // 
            // labelInvoiceNo
            // 
            this.labelInvoiceNo.Location = new System.Drawing.Point(615, 54);
            this.labelInvoiceNo.Name = "labelInvoiceNo";
            this.labelInvoiceNo.Size = new System.Drawing.Size(127, 23);
            this.labelInvoiceNo.TabIndex = 24;
            this.labelInvoiceNo.Text = "Invoice No.";
            // 
            // labelShipPlanNo
            // 
            this.labelShipPlanNo.Location = new System.Drawing.Point(615, 79);
            this.labelShipPlanNo.Name = "labelShipPlanNo";
            this.labelShipPlanNo.Size = new System.Drawing.Size(127, 23);
            this.labelShipPlanNo.TabIndex = 25;
            this.labelShipPlanNo.Text = "Ship Plan No.";
            // 
            // labelShipQty
            // 
            this.labelShipQty.Location = new System.Drawing.Point(363, 54);
            this.labelShipQty.Name = "labelShipQty";
            this.labelShipQty.Size = new System.Drawing.Size(95, 23);
            this.labelShipQty.TabIndex = 26;
            this.labelShipQty.Text = "Ship Qty";
            // 
            // labelTtlCBM
            // 
            this.labelTtlCBM.Location = new System.Drawing.Point(363, 79);
            this.labelTtlCBM.Name = "labelTtlCBM";
            this.labelTtlCBM.Size = new System.Drawing.Size(95, 23);
            this.labelTtlCBM.TabIndex = 27;
            this.labelTtlCBM.Text = "Ttl CBM";
            // 
            // datePullOutDate
            // 
            this.datePullOutDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PulloutDate", true));
            this.datePullOutDate.IsSupportEditMode = false;
            this.datePullOutDate.Location = new System.Drawing.Point(745, 4);
            this.datePullOutDate.Name = "datePullOutDate";
            this.datePullOutDate.ReadOnly = true;
            this.datePullOutDate.Size = new System.Drawing.Size(130, 23);
            this.datePullOutDate.TabIndex = 14;
            // 
            // displayInvoiceNo
            // 
            this.displayInvoiceNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayInvoiceNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "INVNo", true));
            this.displayInvoiceNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayInvoiceNo.Location = new System.Drawing.Point(745, 54);
            this.displayInvoiceNo.Name = "displayInvoiceNo";
            this.displayInvoiceNo.Size = new System.Drawing.Size(182, 23);
            this.displayInvoiceNo.TabIndex = 16;
            // 
            // displayShipPlanNo
            // 
            this.displayShipPlanNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayShipPlanNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ShipPlanID", true));
            this.displayShipPlanNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayShipPlanNo.Location = new System.Drawing.Point(745, 79);
            this.displayShipPlanNo.Name = "displayShipPlanNo";
            this.displayShipPlanNo.Size = new System.Drawing.Size(120, 23);
            this.displayShipPlanNo.TabIndex = 17;
            // 
            // numShipQty
            // 
            this.numShipQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numShipQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ShipQty", true));
            this.numShipQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numShipQty.IsSupportEditMode = false;
            this.numShipQty.Location = new System.Drawing.Point(461, 54);
            this.numShipQty.Name = "numShipQty";
            this.numShipQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numShipQty.ReadOnly = true;
            this.numShipQty.Size = new System.Drawing.Size(59, 23);
            this.numShipQty.TabIndex = 10;
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
            this.numTtlCBM.Location = new System.Drawing.Point(461, 79);
            this.numTtlCBM.Name = "numTtlCBM";
            this.numTtlCBM.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTtlCBM.ReadOnly = true;
            this.numTtlCBM.Size = new System.Drawing.Size(75, 23);
            this.numTtlCBM.TabIndex = 11;
            this.numTtlCBM.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // btnCartonSummary
            // 
            this.btnCartonSummary.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnCartonSummary.Location = new System.Drawing.Point(927, 29);
            this.btnCartonSummary.Name = "btnCartonSummary";
            this.btnCartonSummary.Size = new System.Drawing.Size(145, 25);
            this.btnCartonSummary.TabIndex = 23;
            this.btnCartonSummary.Text = "Carton Summary";
            this.btnCartonSummary.UseVisualStyleBackColor = true;
            this.btnCartonSummary.Click += new System.EventHandler(this.BtnCartonSummary_Click);
            // 
            // btnBatchImport
            // 
            this.btnBatchImport.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnBatchImport.Location = new System.Drawing.Point(927, 59);
            this.btnBatchImport.Name = "btnBatchImport";
            this.btnBatchImport.Size = new System.Drawing.Size(145, 25);
            this.btnBatchImport.TabIndex = 24;
            this.btnBatchImport.Text = "Batch Import";
            this.btnBatchImport.UseVisualStyleBackColor = true;
            this.btnBatchImport.Click += new System.EventHandler(this.BtnBatchImport_Click);
            // 
            // btnImportFromExcel
            // 
            this.btnImportFromExcel.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImportFromExcel.Location = new System.Drawing.Point(927, 88);
            this.btnImportFromExcel.Name = "btnImportFromExcel";
            this.btnImportFromExcel.Size = new System.Drawing.Size(145, 25);
            this.btnImportFromExcel.TabIndex = 25;
            this.btnImportFromExcel.Text = "Import from excel";
            this.btnImportFromExcel.UseVisualStyleBackColor = true;
            this.btnImportFromExcel.Click += new System.EventHandler(this.BtnImportFromExcel_Click);
            // 
            // btnDownloadExcelFormat
            // 
            this.btnDownloadExcelFormat.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnDownloadExcelFormat.Location = new System.Drawing.Point(927, 117);
            this.btnDownloadExcelFormat.Name = "btnDownloadExcelFormat";
            this.btnDownloadExcelFormat.Size = new System.Drawing.Size(145, 45);
            this.btnDownloadExcelFormat.TabIndex = 26;
            this.btnDownloadExcelFormat.Text = "Download excel format";
            this.btnDownloadExcelFormat.UseVisualStyleBackColor = true;
            this.btnDownloadExcelFormat.Click += new System.EventHandler(this.BtnDownloadExcelFormat_Click);
            // 
            // btnRecalculateWeight
            // 
            this.btnRecalculateWeight.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnRecalculateWeight.Location = new System.Drawing.Point(927, 166);
            this.btnRecalculateWeight.Name = "btnRecalculateWeight";
            this.btnRecalculateWeight.Size = new System.Drawing.Size(145, 25);
            this.btnRecalculateWeight.TabIndex = 22;
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
            this.numTtlCtn.Location = new System.Drawing.Point(147, 129);
            this.numTtlCtn.Name = "numTtlCtn";
            this.numTtlCtn.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTtlCtn.ReadOnly = true;
            this.numTtlCtn.Size = new System.Drawing.Size(59, 23);
            this.numTtlCtn.TabIndex = 5;
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
            this.displayPurchaseCTN.Location = new System.Drawing.Point(745, 179);
            this.displayPurchaseCTN.Name = "displayPurchaseCTN";
            this.displayPurchaseCTN.Size = new System.Drawing.Size(27, 23);
            this.displayPurchaseCTN.TabIndex = 21;
            // 
            // labelConfirmed
            // 
            this.labelConfirmed.BackColor = System.Drawing.Color.Transparent;
            this.labelConfirmed.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.labelConfirmed.Location = new System.Drawing.Point(924, 4);
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
            this.numTtlGW.Location = new System.Drawing.Point(461, 104);
            this.numTtlGW.Name = "numTtlGW";
            this.numTtlGW.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTtlGW.ReadOnly = true;
            this.numTtlGW.Size = new System.Drawing.Size(75, 23);
            this.numTtlGW.TabIndex = 12;
            this.numTtlGW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelTtlGW
            // 
            this.labelTtlGW.Location = new System.Drawing.Point(363, 104);
            this.labelTtlGW.Name = "labelTtlGW";
            this.labelTtlGW.Size = new System.Drawing.Size(95, 22);
            this.labelTtlGW.TabIndex = 63;
            this.labelTtlGW.Text = "Ttl GW";
            // 
            // labelPullOutNo
            // 
            this.labelPullOutNo.Location = new System.Drawing.Point(615, 29);
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
            this.displayPullOutNo.Location = new System.Drawing.Point(745, 29);
            this.displayPullOutNo.Name = "displayPullOutNo";
            this.displayPullOutNo.Size = new System.Drawing.Size(182, 23);
            this.displayPullOutNo.TabIndex = 15;
            // 
            // numAppEstAmtVW
            // 
            this.numAppEstAmtVW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numAppEstAmtVW.DecimalPlaces = 3;
            this.numAppEstAmtVW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numAppEstAmtVW.IsSupportEditMode = false;
            this.numAppEstAmtVW.Location = new System.Drawing.Point(504, 129);
            this.numAppEstAmtVW.Name = "numAppEstAmtVW";
            this.numAppEstAmtVW.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAppEstAmtVW.ReadOnly = true;
            this.numAppEstAmtVW.Size = new System.Drawing.Size(109, 23);
            this.numAppEstAmtVW.TabIndex = 13;
            this.numAppEstAmtVW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labAppEstAmtVW
            // 
            this.labAppEstAmtVW.Location = new System.Drawing.Point(363, 129);
            this.labAppEstAmtVW.Name = "labAppEstAmtVW";
            this.labAppEstAmtVW.Size = new System.Drawing.Size(138, 22);
            this.labAppEstAmtVW.TabIndex = 79;
            this.labAppEstAmtVW.Text = "V.M. for APP est. Amt";
            // 
            // numAppBookingVW
            // 
            this.numAppBookingVW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numAppBookingVW.DecimalPlaces = 3;
            this.numAppBookingVW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numAppBookingVW.IsSupportEditMode = false;
            this.numAppBookingVW.Location = new System.Drawing.Point(147, 154);
            this.numAppBookingVW.Name = "numAppBookingVW";
            this.numAppBookingVW.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAppBookingVW.ReadOnly = true;
            this.numAppBookingVW.Size = new System.Drawing.Size(120, 23);
            this.numAppBookingVW.TabIndex = 6;
            this.numAppBookingVW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labAppBookingVW
            // 
            this.labAppBookingVW.Location = new System.Drawing.Point(3, 154);
            this.labAppBookingVW.Name = "labAppBookingVW";
            this.labAppBookingVW.Size = new System.Drawing.Size(141, 22);
            this.labAppBookingVW.TabIndex = 77;
            this.labAppBookingVW.Text = "V.W. for APP booking";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(615, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 23);
            this.label1.TabIndex = 81;
            this.label1.Text = "HC No.";
            // 
            // displayHC
            // 
            this.displayHC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayHC.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ExpressID", true));
            this.displayHC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayHC.Location = new System.Drawing.Point(745, 104);
            this.displayHC.Name = "displayHC";
            this.displayHC.Size = new System.Drawing.Size(120, 23);
            this.displayHC.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(615, 204);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 23);
            this.label2.TabIndex = 82;
            this.label2.Text = "Apv. to Purchase";
            // 
            // dateBox1
            // 
            this.dateBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ApvToPurchaseDate", true));
            this.dateBox1.IsSupportEditMode = false;
            this.dateBox1.Location = new System.Drawing.Point(745, 204);
            this.dateBox1.Name = "dateBox1";
            this.dateBox1.ReadOnly = true;
            this.dateBox1.Size = new System.Drawing.Size(130, 23);
            this.dateBox1.TabIndex = 83;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FactoryID", true));
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsMultiselect = false;
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(461, 4);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 8;
            // 
            // txtshipmode
            // 
            this.txtshipmode.BackColor = System.Drawing.Color.White;
            this.txtshipmode.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "ShipModeID", true));
            this.txtshipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshipmode.FormattingEnabled = true;
            this.txtshipmode.IsSupportUnselect = true;
            this.txtshipmode.Location = new System.Drawing.Point(461, 28);
            this.txtshipmode.Name = "txtshipmode";
            this.txtshipmode.OldText = "";
            this.txtshipmode.Size = new System.Drawing.Size(121, 24);
            this.txtshipmode.TabIndex = 9;
            this.txtshipmode.UseFunction = null;
            // 
            // txtcountry
            // 
            this.txtcountry.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Dest", true));
            this.txtcountry.DisplayBox1Binding = "";
            this.txtcountry.Location = new System.Drawing.Point(147, 104);
            this.txtcountry.Name = "txtcountry";
            this.txtcountry.Size = new System.Drawing.Size(209, 22);
            this.txtcountry.TabIndex = 4;
            this.txtcountry.TextBox1Binding = "";
            // 
            // txtcustcd
            // 
            this.txtcustcd.BackColor = System.Drawing.Color.White;
            this.txtcustcd.BrandObjectName = this.txtbrand;
            this.txtcustcd.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CustCDID", true));
            this.txtcustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd.Location = new System.Drawing.Point(147, 79);
            this.txtcustcd.Name = "txtcustcd";
            this.txtcustcd.Size = new System.Drawing.Size(125, 23);
            this.txtcustcd.TabIndex = 3;
            this.txtcustcd.Validated += new System.EventHandler(this.Txtcustcd_Validated);
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(147, 54);
            this.txtbrand.MyDocumentdName = null;
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 2;
            // 
            // comboCompany1
            // 
            this.comboCompany1.BackColor = System.Drawing.Color.White;
            this.comboCompany1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "OrderCompanyID", true));
            this.comboCompany1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCompany1.FormattingEnabled = true;
            this.comboCompany1.IsOrderCompany = null;
            this.comboCompany1.IsSupportUnselect = true;
            this.comboCompany1.Junk = null;
            this.comboCompany1.Location = new System.Drawing.Point(147, 28);
            this.comboCompany1.Name = "comboCompany1";
            this.comboCompany1.OldText = "";
            this.comboCompany1.Size = new System.Drawing.Size(214, 24);
            this.comboCompany1.TabIndex = 87;
            this.comboCompany1.SelectedIndexChanged += new System.EventHandler(this.ComboCompany1_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(2, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(142, 22);
            this.label5.TabIndex = 86;
            this.label5.Text = "Order Company";
            // 
            // P04
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1092, 618);
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
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
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
        private Class.Txtcountry txtcountry;
        private Class.Txtcustcd txtcustcd;
        private Class.Txtbrand txtbrand;
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
        private Class.Txtshipmode txtshipmode;
        private Win.UI.Button btnRecalculateWeight;
        private Win.UI.Button btnDownloadExcelFormat;
        private Win.UI.Button btnImportFromExcel;
        private Win.UI.Button btnBatchImport;
        private Win.UI.Button btnCartonSummary;
        private Win.UI.DisplayBox displayPurchaseCTN;
        private Win.UI.NumericBox numTtlCtn;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label labelConfirmed;
        private Win.UI.NumericBox numTtlGW;
        private Win.UI.Label labelTtlGW;
        private Win.UI.DisplayBox displayPullOutNo;
        private Win.UI.Label labelPullOutNo;
        private Win.UI.NumericBox numAppEstAmtVW;
        private Win.UI.Label labAppEstAmtVW;
        private Win.UI.NumericBox numAppBookingVW;
        private Win.UI.Label labAppBookingVW;
        private Win.UI.DisplayBox displayHC;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.DateBox dateBox1;
        private Class.ComboCompany comboCompany1;
        private Win.UI.Label label5;
    }
}
