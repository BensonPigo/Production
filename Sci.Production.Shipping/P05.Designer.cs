namespace Sci.Production.Shipping
{
    partial class P05
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
            this.labelInvoiceNo = new Sci.Win.UI.Label();
            this.labelInvSerial = new Sci.Win.UI.Label();
            this.labelInvDate = new Sci.Win.UI.Label();
            this.labelShipper = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelFCRDate = new Sci.Win.UI.Label();
            this.labelCustCD = new Sci.Win.UI.Label();
            this.labelDestination = new Sci.Win.UI.Label();
            this.labelPaymentTerm = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.displayInvoiceNo = new Sci.Win.UI.DisplayBox();
            this.txtInvSerial = new Sci.Win.UI.TextBox();
            this.dateInvDate = new Sci.Win.UI.DateBox();
            this.dateFCRDate = new Sci.Win.UI.DateBox();
            this.displayDescription = new Sci.Win.UI.DisplayBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.labelShippingMode = new Sci.Win.UI.Label();
            this.labelShipmentTerm = new Sci.Win.UI.Label();
            this.labelttlQty = new Sci.Win.UI.Label();
            this.labelttlCarton = new Sci.Win.UI.Label();
            this.labelttlGW = new Sci.Win.UI.Label();
            this.labelttlMeas = new Sci.Win.UI.Label();
            this.labelttlNW = new Sci.Win.UI.Label();
            this.labellttlNNW = new Sci.Win.UI.Label();
            this.numttlQty = new Sci.Win.UI.NumericBox();
            this.numttlCarton = new Sci.Win.UI.NumericBox();
            this.numttlGW = new Sci.Win.UI.NumericBox();
            this.numttlMeas = new Sci.Win.UI.NumericBox();
            this.numttlNW = new Sci.Win.UI.NumericBox();
            this.numttlNNW = new Sci.Win.UI.NumericBox();
            this.labelHandle = new Sci.Win.UI.Label();
            this.labelForwarder = new Sci.Win.UI.Label();
            this.labelContainerType = new Sci.Win.UI.Label();
            this.labelSONo = new Sci.Win.UI.Label();
            this.labelTerminalWhse = new Sci.Win.UI.Label();
            this.labelCutoffDate = new Sci.Win.UI.Label();
            this.labelSOCfmDate = new Sci.Win.UI.Label();
            this.labelVslvoyFltNo = new Sci.Win.UI.Label();
            this.labelShipPlanNo = new Sci.Win.UI.Label();
            this.labelETDETA = new Sci.Win.UI.Label();
            this.labelSendtoTPE = new Sci.Win.UI.Label();
            this.txtSONo = new Sci.Win.UI.TextBox();
            this.dateSOCfmDate = new Sci.Win.UI.DateBox();
            this.txtVslvoyFltNo = new Sci.Win.UI.TextBox();
            this.dateETD = new Sci.Win.UI.DateBox();
            this.dateETA = new Sci.Win.UI.DateBox();
            this.dateSendtoTPE = new Sci.Win.UI.DateBox();
            this.label31 = new Sci.Win.UI.Label();
            this.btnAirPPList = new Sci.Win.UI.Button();
            this.btnExpenseData = new Sci.Win.UI.Button();
            this.btnContainerTruck = new Sci.Win.UI.Button();
            this.btnCFM = new Sci.Win.UI.Button();
            this.btnH = new Sci.Win.UI.Button();
            this.btnImportfrompackinglist = new Sci.Win.UI.Button();
            this.displayShipPlanNo = new Sci.Win.UI.DisplayBox();
            this.txtCustCD = new Sci.Win.UI.TextBox();
            this.comboContainerType = new Sci.Win.UI.ComboBox();
            this.txtTerminalWhse = new Sci.Win.UI.TextBox();
            this.txtCutoffDate = new Sci.Win.UI.TextBox();
            this.txtSubconForwarder = new Sci.Production.Class.txtsubcon();
            this.txtUserHandle = new Sci.Production.Class.txtuser();
            this.txtShiptermShipmentTerm = new Sci.Production.Class.txtshipterm();
            this.txtShipmodeShippingMode = new Sci.Production.Class.txtshipmode();
            this.txtpaytermarPaymentTerm = new Sci.Production.Class.txtpaytermar();
            this.txtCountryDestination = new Sci.Production.Class.txtcountry();
            this.txtbrand = new Sci.Production.Class.txtbrand();
            this.txtfactoryShipper = new Sci.Win.UI.TextBox();
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
            this.masterpanel.Controls.Add(this.btnImportfrompackinglist);
            this.masterpanel.Controls.Add(this.txtfactoryShipper);
            this.masterpanel.Controls.Add(this.txtCutoffDate);
            this.masterpanel.Controls.Add(this.txtTerminalWhse);
            this.masterpanel.Controls.Add(this.comboContainerType);
            this.masterpanel.Controls.Add(this.txtCustCD);
            this.masterpanel.Controls.Add(this.displayShipPlanNo);
            this.masterpanel.Controls.Add(this.btnH);
            this.masterpanel.Controls.Add(this.btnCFM);
            this.masterpanel.Controls.Add(this.btnContainerTruck);
            this.masterpanel.Controls.Add(this.btnExpenseData);
            this.masterpanel.Controls.Add(this.btnAirPPList);
            this.masterpanel.Controls.Add(this.label31);
            this.masterpanel.Controls.Add(this.txtVslvoyFltNo);
            this.masterpanel.Controls.Add(this.txtSONo);
            this.masterpanel.Controls.Add(this.txtSubconForwarder);
            this.masterpanel.Controls.Add(this.txtUserHandle);
            this.masterpanel.Controls.Add(this.labelSendtoTPE);
            this.masterpanel.Controls.Add(this.labelETDETA);
            this.masterpanel.Controls.Add(this.labelShipPlanNo);
            this.masterpanel.Controls.Add(this.labelVslvoyFltNo);
            this.masterpanel.Controls.Add(this.labelSOCfmDate);
            this.masterpanel.Controls.Add(this.labelCutoffDate);
            this.masterpanel.Controls.Add(this.labelTerminalWhse);
            this.masterpanel.Controls.Add(this.labelSONo);
            this.masterpanel.Controls.Add(this.labelContainerType);
            this.masterpanel.Controls.Add(this.labelForwarder);
            this.masterpanel.Controls.Add(this.labelHandle);
            this.masterpanel.Controls.Add(this.numttlNNW);
            this.masterpanel.Controls.Add(this.numttlNW);
            this.masterpanel.Controls.Add(this.numttlMeas);
            this.masterpanel.Controls.Add(this.numttlGW);
            this.masterpanel.Controls.Add(this.numttlCarton);
            this.masterpanel.Controls.Add(this.numttlQty);
            this.masterpanel.Controls.Add(this.txtShiptermShipmentTerm);
            this.masterpanel.Controls.Add(this.txtShipmodeShippingMode);
            this.masterpanel.Controls.Add(this.labellttlNNW);
            this.masterpanel.Controls.Add(this.labelttlNW);
            this.masterpanel.Controls.Add(this.labelttlMeas);
            this.masterpanel.Controls.Add(this.labelttlGW);
            this.masterpanel.Controls.Add(this.labelttlCarton);
            this.masterpanel.Controls.Add(this.labelttlQty);
            this.masterpanel.Controls.Add(this.labelShipmentTerm);
            this.masterpanel.Controls.Add(this.labelShippingMode);
            this.masterpanel.Controls.Add(this.txtRemark);
            this.masterpanel.Controls.Add(this.displayDescription);
            this.masterpanel.Controls.Add(this.txtpaytermarPaymentTerm);
            this.masterpanel.Controls.Add(this.txtCountryDestination);
            this.masterpanel.Controls.Add(this.txtbrand);
            this.masterpanel.Controls.Add(this.txtInvSerial);
            this.masterpanel.Controls.Add(this.displayInvoiceNo);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.labelDescription);
            this.masterpanel.Controls.Add(this.labelPaymentTerm);
            this.masterpanel.Controls.Add(this.labelDestination);
            this.masterpanel.Controls.Add(this.labelCustCD);
            this.masterpanel.Controls.Add(this.labelFCRDate);
            this.masterpanel.Controls.Add(this.labelBrand);
            this.masterpanel.Controls.Add(this.labelShipper);
            this.masterpanel.Controls.Add(this.labelInvDate);
            this.masterpanel.Controls.Add(this.labelInvSerial);
            this.masterpanel.Controls.Add(this.labelInvoiceNo);
            this.masterpanel.Controls.Add(this.dateETA);
            this.masterpanel.Controls.Add(this.dateSOCfmDate);
            this.masterpanel.Controls.Add(this.dateETD);
            this.masterpanel.Controls.Add(this.dateSendtoTPE);
            this.masterpanel.Controls.Add(this.dateFCRDate);
            this.masterpanel.Controls.Add(this.dateInvDate);
            this.masterpanel.Size = new System.Drawing.Size(990, 311);
            this.masterpanel.Controls.SetChildIndex(this.dateInvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateFCRDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateSendtoTPE, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateETD, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateSOCfmDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateETA, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelInvoiceNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelInvSerial, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelInvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShipper, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelBrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFCRDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCustCD, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDestination, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPaymentTerm, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDescription, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayInvoiceNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtInvSerial, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtbrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtCountryDestination, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtpaytermarPaymentTerm, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayDescription, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShippingMode, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShipmentTerm, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelttlQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelttlCarton, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelttlGW, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelttlMeas, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelttlNW, 0);
            this.masterpanel.Controls.SetChildIndex(this.labellttlNNW, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtShipmodeShippingMode, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtShiptermShipmentTerm, 0);
            this.masterpanel.Controls.SetChildIndex(this.numttlQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.numttlCarton, 0);
            this.masterpanel.Controls.SetChildIndex(this.numttlGW, 0);
            this.masterpanel.Controls.SetChildIndex(this.numttlMeas, 0);
            this.masterpanel.Controls.SetChildIndex(this.numttlNW, 0);
            this.masterpanel.Controls.SetChildIndex(this.numttlNNW, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelForwarder, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelContainerType, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSONo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTerminalWhse, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCutoffDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSOCfmDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelVslvoyFltNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShipPlanNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelETDETA, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSendtoTPE, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtUserHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSubconForwarder, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSONo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtVslvoyFltNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.label31, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnAirPPList, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnExpenseData, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnContainerTruck, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnCFM, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnH, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayShipPlanNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtCustCD, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboContainerType, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtTerminalWhse, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtCutoffDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtfactoryShipper, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportfrompackinglist, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 311);
            this.detailpanel.Size = new System.Drawing.Size(990, 251);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(882, 273);
            this.gridicon.TabIndex = 20;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(910, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(990, 251);
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
            this.detail.Size = new System.Drawing.Size(990, 600);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(990, 562);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 562);
            this.detailbtm.Size = new System.Drawing.Size(990, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(990, 600);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(998, 629);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(473, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(425, 13);
            // 
            // labelInvoiceNo
            // 
            this.labelInvoiceNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelInvoiceNo.Location = new System.Drawing.Point(4, 4);
            this.labelInvoiceNo.Name = "labelInvoiceNo";
            this.labelInvoiceNo.Size = new System.Drawing.Size(66, 23);
            this.labelInvoiceNo.TabIndex = 21;
            this.labelInvoiceNo.Text = "Invoice No.";
            // 
            // labelInvSerial
            // 
            this.labelInvSerial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelInvSerial.Location = new System.Drawing.Point(4, 31);
            this.labelInvSerial.Name = "labelInvSerial";
            this.labelInvSerial.Size = new System.Drawing.Size(66, 23);
            this.labelInvSerial.TabIndex = 22;
            this.labelInvSerial.Text = "Inv. Serial";
            // 
            // labelInvDate
            // 
            this.labelInvDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelInvDate.Location = new System.Drawing.Point(4, 58);
            this.labelInvDate.Name = "labelInvDate";
            this.labelInvDate.Size = new System.Drawing.Size(66, 23);
            this.labelInvDate.TabIndex = 23;
            this.labelInvDate.Text = "Inv. Date";
            // 
            // labelShipper
            // 
            this.labelShipper.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelShipper.Location = new System.Drawing.Point(4, 85);
            this.labelShipper.Name = "labelShipper";
            this.labelShipper.Size = new System.Drawing.Size(66, 23);
            this.labelShipper.TabIndex = 24;
            this.labelShipper.Text = "Shipper";
            // 
            // labelBrand
            // 
            this.labelBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelBrand.Location = new System.Drawing.Point(4, 112);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(66, 23);
            this.labelBrand.TabIndex = 25;
            this.labelBrand.Text = "Brand";
            // 
            // labelFCRDate
            // 
            this.labelFCRDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelFCRDate.Location = new System.Drawing.Point(4, 139);
            this.labelFCRDate.Name = "labelFCRDate";
            this.labelFCRDate.Size = new System.Drawing.Size(66, 23);
            this.labelFCRDate.TabIndex = 26;
            this.labelFCRDate.Text = "FCR Date";
            // 
            // labelCustCD
            // 
            this.labelCustCD.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelCustCD.Location = new System.Drawing.Point(4, 166);
            this.labelCustCD.Name = "labelCustCD";
            this.labelCustCD.Size = new System.Drawing.Size(66, 23);
            this.labelCustCD.TabIndex = 27;
            this.labelCustCD.Text = "CustCD";
            // 
            // labelDestination
            // 
            this.labelDestination.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelDestination.Location = new System.Drawing.Point(4, 193);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(66, 23);
            this.labelDestination.TabIndex = 28;
            this.labelDestination.Text = "Destination";
            // 
            // labelPaymentTerm
            // 
            this.labelPaymentTerm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelPaymentTerm.Location = new System.Drawing.Point(4, 220);
            this.labelPaymentTerm.Name = "labelPaymentTerm";
            this.labelPaymentTerm.Size = new System.Drawing.Size(84, 23);
            this.labelPaymentTerm.TabIndex = 29;
            this.labelPaymentTerm.Text = "Payment Term";
            // 
            // labelDescription
            // 
            this.labelDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelDescription.Location = new System.Drawing.Point(4, 247);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(66, 23);
            this.labelDescription.TabIndex = 30;
            this.labelDescription.Text = "Description";
            // 
            // labelRemark
            // 
            this.labelRemark.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelRemark.Location = new System.Drawing.Point(4, 274);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(66, 23);
            this.labelRemark.TabIndex = 31;
            this.labelRemark.Text = "Remark";
            // 
            // displayInvoiceNo
            // 
            this.displayInvoiceNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayInvoiceNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayInvoiceNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayInvoiceNo.Location = new System.Drawing.Point(73, 4);
            this.displayInvoiceNo.Name = "displayInvoiceNo";
            this.displayInvoiceNo.Size = new System.Drawing.Size(186, 23);
            this.displayInvoiceNo.TabIndex = 12;
            // 
            // txtInvSerial
            // 
            this.txtInvSerial.BackColor = System.Drawing.Color.White;
            this.txtInvSerial.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "InvSerial", true));
            this.txtInvSerial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvSerial.Location = new System.Drawing.Point(73, 31);
            this.txtInvSerial.Name = "txtInvSerial";
            this.txtInvSerial.Size = new System.Drawing.Size(100, 23);
            this.txtInvSerial.TabIndex = 1;
            this.txtInvSerial.Validated += new System.EventHandler(this.TxtInvSerial_Validated);
            // 
            // dateInvDate
            // 
            this.dateInvDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "InvDate", true));
            this.dateInvDate.Location = new System.Drawing.Point(73, 58);
            this.dateInvDate.Name = "dateInvDate";
            this.dateInvDate.Size = new System.Drawing.Size(130, 23);
            this.dateInvDate.TabIndex = 2;
            this.dateInvDate.Validating += new System.ComponentModel.CancelEventHandler(this.DateInvDate_Validating);
            // 
            // dateFCRDate
            // 
            this.dateFCRDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FCRDate", true));
            this.dateFCRDate.Location = new System.Drawing.Point(73, 139);
            this.dateFCRDate.Name = "dateFCRDate";
            this.dateFCRDate.Size = new System.Drawing.Size(130, 23);
            this.dateFCRDate.TabIndex = 4;
            this.dateFCRDate.Validating += new System.ComponentModel.CancelEventHandler(this.DateFCRDate_Validating);
            // 
            // displayDescription
            // 
            this.displayDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDescription.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Description", true));
            this.displayDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDescription.Location = new System.Drawing.Point(73, 247);
            this.displayDescription.Name = "displayDescription";
            this.displayDescription.Size = new System.Drawing.Size(331, 23);
            this.displayDescription.TabIndex = 7;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(73, 274);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(469, 23);
            this.txtRemark.TabIndex = 8;
            // 
            // labelShippingMode
            // 
            this.labelShippingMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelShippingMode.Location = new System.Drawing.Point(327, 4);
            this.labelShippingMode.Name = "labelShippingMode";
            this.labelShippingMode.Size = new System.Drawing.Size(90, 23);
            this.labelShippingMode.TabIndex = 32;
            this.labelShippingMode.Text = "Shipping Mode";
            // 
            // labelShipmentTerm
            // 
            this.labelShipmentTerm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelShipmentTerm.Location = new System.Drawing.Point(327, 31);
            this.labelShipmentTerm.Name = "labelShipmentTerm";
            this.labelShipmentTerm.Size = new System.Drawing.Size(90, 23);
            this.labelShipmentTerm.TabIndex = 33;
            this.labelShipmentTerm.Text = "Shipment Term";
            // 
            // labelttlQty
            // 
            this.labelttlQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelttlQty.Location = new System.Drawing.Point(327, 58);
            this.labelttlQty.Name = "labelttlQty";
            this.labelttlQty.Size = new System.Drawing.Size(90, 23);
            this.labelttlQty.TabIndex = 25;
            this.labelttlQty.Text = "ttl Q\'ty";
            // 
            // labelttlCarton
            // 
            this.labelttlCarton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelttlCarton.Location = new System.Drawing.Point(327, 85);
            this.labelttlCarton.Name = "labelttlCarton";
            this.labelttlCarton.Size = new System.Drawing.Size(90, 23);
            this.labelttlCarton.TabIndex = 26;
            this.labelttlCarton.Text = "ttl Carton";
            // 
            // labelttlGW
            // 
            this.labelttlGW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelttlGW.Location = new System.Drawing.Point(327, 112);
            this.labelttlGW.Name = "labelttlGW";
            this.labelttlGW.Size = new System.Drawing.Size(90, 23);
            this.labelttlGW.TabIndex = 27;
            this.labelttlGW.Text = "ttl G.W.";
            // 
            // labelttlMeas
            // 
            this.labelttlMeas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelttlMeas.Location = new System.Drawing.Point(327, 139);
            this.labelttlMeas.Name = "labelttlMeas";
            this.labelttlMeas.Size = new System.Drawing.Size(90, 23);
            this.labelttlMeas.TabIndex = 28;
            this.labelttlMeas.Text = "ttl Meas.";
            // 
            // labelttlNW
            // 
            this.labelttlNW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelttlNW.Location = new System.Drawing.Point(327, 166);
            this.labelttlNW.Name = "labelttlNW";
            this.labelttlNW.Size = new System.Drawing.Size(90, 23);
            this.labelttlNW.TabIndex = 29;
            this.labelttlNW.Text = "ttl N.W.";
            // 
            // labellttlNNW
            // 
            this.labellttlNNW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labellttlNNW.Location = new System.Drawing.Point(327, 193);
            this.labellttlNNW.Name = "labellttlNNW";
            this.labellttlNNW.Size = new System.Drawing.Size(90, 23);
            this.labellttlNNW.TabIndex = 30;
            this.labellttlNNW.Text = "ttl N.N.W.";
            // 
            // numttlQty
            // 
            this.numttlQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numttlQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "TotalShipQty", true));
            this.numttlQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numttlQty.IsSupportEditMode = false;
            this.numttlQty.Location = new System.Drawing.Point(420, 58);
            this.numttlQty.Name = "numttlQty";
            this.numttlQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numttlQty.ReadOnly = true;
            this.numttlQty.Size = new System.Drawing.Size(100, 23);
            this.numttlQty.TabIndex = 33;
            this.numttlQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numttlCarton
            // 
            this.numttlCarton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numttlCarton.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "TotalCTNQty", true));
            this.numttlCarton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numttlCarton.IsSupportEditMode = false;
            this.numttlCarton.Location = new System.Drawing.Point(420, 85);
            this.numttlCarton.Name = "numttlCarton";
            this.numttlCarton.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numttlCarton.ReadOnly = true;
            this.numttlCarton.Size = new System.Drawing.Size(100, 23);
            this.numttlCarton.TabIndex = 34;
            this.numttlCarton.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numttlGW
            // 
            this.numttlGW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numttlGW.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "TotalGW", true));
            this.numttlGW.DecimalPlaces = 2;
            this.numttlGW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numttlGW.IsSupportEditMode = false;
            this.numttlGW.Location = new System.Drawing.Point(420, 112);
            this.numttlGW.Name = "numttlGW";
            this.numttlGW.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numttlGW.ReadOnly = true;
            this.numttlGW.Size = new System.Drawing.Size(100, 23);
            this.numttlGW.TabIndex = 35;
            this.numttlGW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numttlMeas
            // 
            this.numttlMeas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numttlMeas.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "TotalCBM", true));
            this.numttlMeas.DecimalPlaces = 2;
            this.numttlMeas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numttlMeas.IsSupportEditMode = false;
            this.numttlMeas.Location = new System.Drawing.Point(420, 139);
            this.numttlMeas.Name = "numttlMeas";
            this.numttlMeas.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numttlMeas.ReadOnly = true;
            this.numttlMeas.Size = new System.Drawing.Size(100, 23);
            this.numttlMeas.TabIndex = 36;
            this.numttlMeas.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numttlNW
            // 
            this.numttlNW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numttlNW.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "TotalNW", true));
            this.numttlNW.DecimalPlaces = 2;
            this.numttlNW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numttlNW.IsSupportEditMode = false;
            this.numttlNW.Location = new System.Drawing.Point(420, 166);
            this.numttlNW.Name = "numttlNW";
            this.numttlNW.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numttlNW.ReadOnly = true;
            this.numttlNW.Size = new System.Drawing.Size(100, 23);
            this.numttlNW.TabIndex = 37;
            this.numttlNW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numttlNNW
            // 
            this.numttlNNW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numttlNNW.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "TotalNNW", true));
            this.numttlNNW.DecimalPlaces = 2;
            this.numttlNNW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numttlNNW.IsSupportEditMode = false;
            this.numttlNNW.Location = new System.Drawing.Point(420, 193);
            this.numttlNNW.Name = "numttlNNW";
            this.numttlNNW.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numttlNNW.ReadOnly = true;
            this.numttlNNW.Size = new System.Drawing.Size(100, 23);
            this.numttlNNW.TabIndex = 38;
            this.numttlNNW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelHandle
            // 
            this.labelHandle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelHandle.Location = new System.Drawing.Point(560, 4);
            this.labelHandle.Name = "labelHandle";
            this.labelHandle.Size = new System.Drawing.Size(94, 23);
            this.labelHandle.TabIndex = 39;
            this.labelHandle.Text = "Handle";
            // 
            // labelForwarder
            // 
            this.labelForwarder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelForwarder.Location = new System.Drawing.Point(560, 31);
            this.labelForwarder.Name = "labelForwarder";
            this.labelForwarder.Size = new System.Drawing.Size(94, 23);
            this.labelForwarder.TabIndex = 40;
            this.labelForwarder.Text = "Forwarder";
            // 
            // labelContainerType
            // 
            this.labelContainerType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelContainerType.Location = new System.Drawing.Point(560, 58);
            this.labelContainerType.Name = "labelContainerType";
            this.labelContainerType.Size = new System.Drawing.Size(94, 23);
            this.labelContainerType.TabIndex = 41;
            this.labelContainerType.Text = "Container Type";
            // 
            // labelSONo
            // 
            this.labelSONo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelSONo.Location = new System.Drawing.Point(560, 85);
            this.labelSONo.Name = "labelSONo";
            this.labelSONo.Size = new System.Drawing.Size(94, 23);
            this.labelSONo.TabIndex = 42;
            this.labelSONo.Text = "S/O #";
            // 
            // labelTerminalWhse
            // 
            this.labelTerminalWhse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelTerminalWhse.Location = new System.Drawing.Point(560, 112);
            this.labelTerminalWhse.Name = "labelTerminalWhse";
            this.labelTerminalWhse.Size = new System.Drawing.Size(94, 23);
            this.labelTerminalWhse.TabIndex = 43;
            this.labelTerminalWhse.Text = "Terminal/Whse#";
            // 
            // labelCutoffDate
            // 
            this.labelCutoffDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelCutoffDate.Location = new System.Drawing.Point(560, 139);
            this.labelCutoffDate.Name = "labelCutoffDate";
            this.labelCutoffDate.Size = new System.Drawing.Size(94, 23);
            this.labelCutoffDate.TabIndex = 44;
            this.labelCutoffDate.Text = "Cut-off Date";
            // 
            // labelSOCfmDate
            // 
            this.labelSOCfmDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelSOCfmDate.Location = new System.Drawing.Point(560, 166);
            this.labelSOCfmDate.Name = "labelSOCfmDate";
            this.labelSOCfmDate.Size = new System.Drawing.Size(94, 23);
            this.labelSOCfmDate.TabIndex = 45;
            this.labelSOCfmDate.Text = "S/O Cfm Date";
            // 
            // labelVslvoyFltNo
            // 
            this.labelVslvoyFltNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelVslvoyFltNo.Location = new System.Drawing.Point(560, 193);
            this.labelVslvoyFltNo.Name = "labelVslvoyFltNo";
            this.labelVslvoyFltNo.Size = new System.Drawing.Size(94, 23);
            this.labelVslvoyFltNo.TabIndex = 46;
            this.labelVslvoyFltNo.Text = "Vsl voy/Flt No.";
            // 
            // labelShipPlanNo
            // 
            this.labelShipPlanNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelShipPlanNo.Location = new System.Drawing.Point(560, 220);
            this.labelShipPlanNo.Name = "labelShipPlanNo";
            this.labelShipPlanNo.Size = new System.Drawing.Size(94, 23);
            this.labelShipPlanNo.TabIndex = 47;
            this.labelShipPlanNo.Text = "Ship Plan No.";
            // 
            // labelETDETA
            // 
            this.labelETDETA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelETDETA.Location = new System.Drawing.Point(560, 247);
            this.labelETDETA.Name = "labelETDETA";
            this.labelETDETA.Size = new System.Drawing.Size(74, 23);
            this.labelETDETA.TabIndex = 48;
            this.labelETDETA.Text = "ETD/ETA";
            // 
            // labelSendtoTPE
            // 
            this.labelSendtoTPE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelSendtoTPE.Location = new System.Drawing.Point(560, 274);
            this.labelSendtoTPE.Name = "labelSendtoTPE";
            this.labelSendtoTPE.Size = new System.Drawing.Size(74, 23);
            this.labelSendtoTPE.TabIndex = 49;
            this.labelSendtoTPE.Text = "Send to TPE";
            // 
            // txtSONo
            // 
            this.txtSONo.BackColor = System.Drawing.Color.White;
            this.txtSONo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SONo", true));
            this.txtSONo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSONo.Location = new System.Drawing.Point(658, 85);
            this.txtSONo.Name = "txtSONo";
            this.txtSONo.Size = new System.Drawing.Size(140, 23);
            this.txtSONo.TabIndex = 13;
            // 
            // dateSOCfmDate
            // 
            this.dateSOCfmDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SOCFMDate", true));
            this.dateSOCfmDate.IsSupportEditMode = false;
            this.dateSOCfmDate.Location = new System.Drawing.Point(658, 166);
            this.dateSOCfmDate.Name = "dateSOCfmDate";
            this.dateSOCfmDate.ReadOnly = true;
            this.dateSOCfmDate.Size = new System.Drawing.Size(130, 23);
            this.dateSOCfmDate.TabIndex = 56;
            // 
            // txtVslvoyFltNo
            // 
            this.txtVslvoyFltNo.BackColor = System.Drawing.Color.White;
            this.txtVslvoyFltNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Vessel", true));
            this.txtVslvoyFltNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtVslvoyFltNo.Location = new System.Drawing.Point(658, 193);
            this.txtVslvoyFltNo.Name = "txtVslvoyFltNo";
            this.txtVslvoyFltNo.Size = new System.Drawing.Size(188, 23);
            this.txtVslvoyFltNo.TabIndex = 16;
            // 
            // dateETD
            // 
            this.dateETD.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ETD", true));
            this.dateETD.Location = new System.Drawing.Point(636, 247);
            this.dateETD.Name = "dateETD";
            this.dateETD.Size = new System.Drawing.Size(130, 23);
            this.dateETD.TabIndex = 17;
            this.dateETD.Validating += new System.ComponentModel.CancelEventHandler(this.DateETD_Validating);
            // 
            // dateETA
            // 
            this.dateETA.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ETA", true));
            this.dateETA.Location = new System.Drawing.Point(784, 247);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(130, 23);
            this.dateETA.TabIndex = 18;
            this.dateETA.Validating += new System.ComponentModel.CancelEventHandler(this.DateETA_Validating);
            // 
            // dateSendtoTPE
            // 
            this.dateSendtoTPE.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SendToTPE", true));
            this.dateSendtoTPE.IsSupportEditMode = false;
            this.dateSendtoTPE.Location = new System.Drawing.Point(636, 274);
            this.dateSendtoTPE.Name = "dateSendtoTPE";
            this.dateSendtoTPE.ReadOnly = true;
            this.dateSendtoTPE.Size = new System.Drawing.Size(130, 23);
            this.dateSendtoTPE.TabIndex = 61;
            // 
            // label31
            // 
            this.label31.Location = new System.Drawing.Point(770, 247);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(10, 23);
            this.label31.TabIndex = 19;
            this.label31.Text = "/";
            // 
            // btnAirPPList
            // 
            this.btnAirPPList.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnAirPPList.Location = new System.Drawing.Point(849, 34);
            this.btnAirPPList.Name = "btnAirPPList";
            this.btnAirPPList.Size = new System.Drawing.Size(132, 30);
            this.btnAirPPList.TabIndex = 64;
            this.btnAirPPList.Text = "AirPP List";
            this.btnAirPPList.UseVisualStyleBackColor = true;
            this.btnAirPPList.Click += new System.EventHandler(this.BtnAirPPList_Click);
            this.btnAirPPList.Visible = false;
            // 
            // btnExpenseData
            // 
            this.btnExpenseData.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnExpenseData.Location = new System.Drawing.Point(849, 71);
            this.btnExpenseData.Name = "btnExpenseData";
            this.btnExpenseData.Size = new System.Drawing.Size(132, 30);
            this.btnExpenseData.TabIndex = 65;
            this.btnExpenseData.Text = "Expense Data";
            this.btnExpenseData.UseVisualStyleBackColor = true;
            this.btnExpenseData.Click += new System.EventHandler(this.BtnExpenseData_Click);
            // 
            // btnContainerTruck
            // 
            this.btnContainerTruck.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnContainerTruck.Location = new System.Drawing.Point(849, 108);
            this.btnContainerTruck.Name = "btnContainerTruck";
            this.btnContainerTruck.Size = new System.Drawing.Size(132, 30);
            this.btnContainerTruck.TabIndex = 66;
            this.btnContainerTruck.Text = "Container/Truck";
            this.btnContainerTruck.UseVisualStyleBackColor = true;
            this.btnContainerTruck.Click += new System.EventHandler(this.BtnContainerTruck_Click);
            // 
            // btnCFM
            // 
            this.btnCFM.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnCFM.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnCFM.Location = new System.Drawing.Point(765, 165);
            this.btnCFM.Name = "btnCFM";
            this.btnCFM.Size = new System.Drawing.Size(65, 25);
            this.btnCFM.TabIndex = 67;
            this.btnCFM.Text = "CFM";
            this.btnCFM.UseVisualStyleBackColor = true;
            this.btnCFM.Click += new System.EventHandler(this.BtnCFM_Click);
            // 
            // btnH
            // 
            this.btnH.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnH.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnH.Location = new System.Drawing.Point(834, 165);
            this.btnH.Name = "btnH";
            this.btnH.Size = new System.Drawing.Size(20, 25);
            this.btnH.TabIndex = 68;
            this.btnH.Text = "H";
            this.btnH.UseVisualStyleBackColor = true;
            this.btnH.Click += new System.EventHandler(this.BtnH_Click);
            // 
            // btnImportfrompackinglist
            // 
            this.btnImportfrompackinglist.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImportfrompackinglist.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnImportfrompackinglist.Location = new System.Drawing.Point(773, 274);
            this.btnImportfrompackinglist.Name = "btnImportfrompackinglist";
            this.btnImportfrompackinglist.Size = new System.Drawing.Size(165, 30);
            this.btnImportfrompackinglist.TabIndex = 19;
            this.btnImportfrompackinglist.Text = "Import from packing list";
            this.btnImportfrompackinglist.UseVisualStyleBackColor = true;
            this.btnImportfrompackinglist.Click += new System.EventHandler(this.BtnImportfrompackinglist_Click);
            // 
            // displayShipPlanNo
            // 
            this.displayShipPlanNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayShipPlanNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ShipPlanID", true));
            this.displayShipPlanNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayShipPlanNo.Location = new System.Drawing.Point(658, 220);
            this.displayShipPlanNo.Name = "displayShipPlanNo";
            this.displayShipPlanNo.Size = new System.Drawing.Size(120, 23);
            this.displayShipPlanNo.TabIndex = 70;
            // 
            // txtCustCD
            // 
            this.txtCustCD.BackColor = System.Drawing.Color.White;
            this.txtCustCD.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CustCDID", true));
            this.txtCustCD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCustCD.Location = new System.Drawing.Point(73, 165);
            this.txtCustCD.Name = "txtCustCD";
            this.txtCustCD.Size = new System.Drawing.Size(125, 23);
            this.txtCustCD.TabIndex = 5;
            this.txtCustCD.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtCustCD_PopUp);
            this.txtCustCD.Validating += new System.ComponentModel.CancelEventHandler(this.TxtCustCD_Validating);
            this.txtCustCD.Validated += new System.EventHandler(this.TxtCustCD_Validated);
            // 
            // comboContainerType
            // 
            this.comboContainerType.BackColor = System.Drawing.Color.White;
            this.comboContainerType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CYCFS", true));
            this.comboContainerType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboContainerType.FormattingEnabled = true;
            this.comboContainerType.IsSupportUnselect = true;
            this.comboContainerType.Location = new System.Drawing.Point(658, 57);
            this.comboContainerType.Name = "comboContainerType";
            this.comboContainerType.Size = new System.Drawing.Size(121, 24);
            this.comboContainerType.TabIndex = 12;
            // 
            // txtTerminalWhse
            // 
            this.txtTerminalWhse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtTerminalWhse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtTerminalWhse.Location = new System.Drawing.Point(658, 112);
            this.txtTerminalWhse.Name = "txtTerminalWhse";
            this.txtTerminalWhse.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.NonReadOnly;
            this.txtTerminalWhse.ReadOnly = true;
            this.txtTerminalWhse.Size = new System.Drawing.Size(164, 23);
            this.txtTerminalWhse.TabIndex = 14;
            this.txtTerminalWhse.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtTerminalWhse_PopUp);
            this.txtTerminalWhse.Validating += new System.ComponentModel.CancelEventHandler(this.TxtTerminalWhse_Validating);
            // 
            // txtCutoffDate
            // 
            this.txtCutoffDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtCutoffDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtCutoffDate.Location = new System.Drawing.Point(658, 139);
            this.txtCutoffDate.Name = "txtCutoffDate";
            this.txtCutoffDate.ReadOnly = true;
            this.txtCutoffDate.Size = new System.Drawing.Size(153, 23);
            this.txtCutoffDate.TabIndex = 15;
            this.txtCutoffDate.Validating += new System.ComponentModel.CancelEventHandler(this.TxtCutoffDate_Validating);
            // 
            // txtSubconForwarder
            // 
            this.txtSubconForwarder.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Forwarder", true));
            this.txtSubconForwarder.DisplayBox1Binding = "";
            this.txtSubconForwarder.IsIncludeJunk = false;
            this.txtSubconForwarder.Location = new System.Drawing.Point(658, 31);
            this.txtSubconForwarder.Name = "txtSubconForwarder";
            this.txtSubconForwarder.Size = new System.Drawing.Size(170, 23);
            this.txtSubconForwarder.TabIndex = 11;
            this.txtSubconForwarder.TextBox1Binding = "";
            // 
            // txtUserHandle
            // 
            this.txtUserHandle.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Handle", true));
            this.txtUserHandle.DisplayBox1Binding = "";
            this.txtUserHandle.Location = new System.Drawing.Point(658, 4);
            this.txtUserHandle.Name = "txtUserHandle";
            this.txtUserHandle.Size = new System.Drawing.Size(302, 23);
            this.txtUserHandle.TabIndex = 10;
            this.txtUserHandle.TextBox1Binding = "";
            // 
            // txtShiptermShipmentTerm
            // 
            this.txtShiptermShipmentTerm.BackColor = System.Drawing.Color.White;
            this.txtShiptermShipmentTerm.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ShipTermID", true));
            this.txtShiptermShipmentTerm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShiptermShipmentTerm.Location = new System.Drawing.Point(420, 31);
            this.txtShiptermShipmentTerm.Name = "txtShiptermShipmentTerm";
            this.txtShiptermShipmentTerm.Size = new System.Drawing.Size(50, 23);
            this.txtShiptermShipmentTerm.TabIndex = 9;
            // 
            // txtShipmodeShippingMode
            // 
            this.txtShipmodeShippingMode.BackColor = System.Drawing.Color.White;
            this.txtShipmodeShippingMode.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "ShipModeID", true));
            this.txtShipmodeShippingMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShipmodeShippingMode.FormattingEnabled = true;
            this.txtShipmodeShippingMode.IsSupportUnselect = true;
            this.txtShipmodeShippingMode.Location = new System.Drawing.Point(420, 4);
            this.txtShipmodeShippingMode.Name = "txtShipmodeShippingMode";
            this.txtShipmodeShippingMode.Size = new System.Drawing.Size(121, 24);
            this.txtShipmodeShippingMode.TabIndex = 8;
            this.txtShipmodeShippingMode.UseFunction = "ORDER";
            // 
            // txtpaytermarPaymentTerm
            // 
            this.txtpaytermarPaymentTerm.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "PayTermARID", true));
            this.txtpaytermarPaymentTerm.DisplayBox1Binding = "";
            this.txtpaytermarPaymentTerm.Location = new System.Drawing.Point(92, 220);
            this.txtpaytermarPaymentTerm.Name = "txtpaytermarPaymentTerm";
            this.txtpaytermarPaymentTerm.Size = new System.Drawing.Size(311, 23);
            this.txtpaytermarPaymentTerm.TabIndex = 7;
            this.txtpaytermarPaymentTerm.TextBox1Binding = "";
            // 
            // txtCountryDestination
            // 
            this.txtCountryDestination.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Dest", true));
            this.txtCountryDestination.DisplayBox1Binding = "";
            this.txtCountryDestination.Location = new System.Drawing.Point(73, 193);
            this.txtCountryDestination.Name = "txtCountryDestination";
            this.txtCountryDestination.Size = new System.Drawing.Size(232, 22);
            this.txtCountryDestination.TabIndex = 6;
            this.txtCountryDestination.TextBox1Binding = "";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(73, 112);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(84, 23);
            this.txtbrand.TabIndex = 3;
            this.txtbrand.Validated += new System.EventHandler(this.Txtbrand_Validated);
            // 
            // txtfactoryShipper
            // 
            this.txtfactoryShipper.BackColor = System.Drawing.Color.White;
            this.txtfactoryShipper.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Shipper", true));
            this.txtfactoryShipper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactoryShipper.Location = new System.Drawing.Point(73, 85);
            this.txtfactoryShipper.Name = "txtfactoryShipper";
            this.txtfactoryShipper.Size = new System.Drawing.Size(64, 23);
            this.txtfactoryShipper.TabIndex = 0;
            this.txtfactoryShipper.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtfactoryShipper_PopUp);
            this.txtfactoryShipper.Validating += new System.ComponentModel.CancelEventHandler(this.TxtfactoryShipper_Validating);
            // 
            // P05
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(998, 662);
            this.DefaultControl = "txtfactoryShipper";
            this.DefaultControlForEdit = "txtfactoryShipper";
            this.DefaultOrder = "ID";
            this.GridAlias = "PackingList";
            this.GridNew = 0;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.KeyField2 = "INVNo";
            this.Name = "P05";
            this.Text = "P05. Garment Booking";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "ID";
            this.WorkAlias = "GMTBooking";
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

        private Win.UI.Label labellttlNNW;
        private Win.UI.Label labelttlNW;
        private Win.UI.Label labelttlMeas;
        private Win.UI.Label labelttlGW;
        private Win.UI.Label labelttlCarton;
        private Win.UI.Label labelttlQty;
        private Win.UI.Label labelShipmentTerm;
        private Win.UI.Label labelShippingMode;
        private Win.UI.TextBox txtRemark;
        private Win.UI.DisplayBox displayDescription;
        private Class.txtpaytermar txtpaytermarPaymentTerm;
        private Class.txtcountry txtCountryDestination;
        private Win.UI.DateBox dateFCRDate;
        private Class.txtbrand txtbrand;
        private Win.UI.DateBox dateInvDate;
        private Win.UI.TextBox txtInvSerial;
        private Win.UI.DisplayBox displayInvoiceNo;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelPaymentTerm;
        private Win.UI.Label labelDestination;
        private Win.UI.Label labelCustCD;
        private Win.UI.Label labelFCRDate;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelShipper;
        private Win.UI.Label labelInvDate;
        private Win.UI.Label labelInvSerial;
        private Win.UI.Label labelInvoiceNo;
        private Class.txtsubcon txtSubconForwarder;
        private Class.txtuser txtUserHandle;
        private Win.UI.Label labelSendtoTPE;
        private Win.UI.Label labelETDETA;
        private Win.UI.Label labelShipPlanNo;
        private Win.UI.Label labelVslvoyFltNo;
        private Win.UI.Label labelSOCfmDate;
        private Win.UI.Label labelCutoffDate;
        private Win.UI.Label labelTerminalWhse;
        private Win.UI.Label labelSONo;
        private Win.UI.Label labelContainerType;
        private Win.UI.Label labelForwarder;
        private Win.UI.Label labelHandle;
        private Win.UI.NumericBox numttlNNW;
        private Win.UI.NumericBox numttlNW;
        private Win.UI.NumericBox numttlMeas;
        private Win.UI.NumericBox numttlGW;
        private Win.UI.NumericBox numttlCarton;
        private Win.UI.NumericBox numttlQty;
        private Class.txtshipterm txtShiptermShipmentTerm;
        private Class.txtshipmode txtShipmodeShippingMode;
        private Win.UI.TextBox txtCustCD;
        private Win.UI.DisplayBox displayShipPlanNo;
        private Win.UI.Button btnImportfrompackinglist;
        private Win.UI.Button btnH;
        private Win.UI.Button btnCFM;
        private Win.UI.Button btnContainerTruck;
        private Win.UI.Button btnExpenseData;
        private Win.UI.Button btnAirPPList;
        private Win.UI.Label label31;
        private Win.UI.DateBox dateSendtoTPE;
        private Win.UI.DateBox dateETA;
        private Win.UI.DateBox dateETD;
        private Win.UI.TextBox txtVslvoyFltNo;
        private Win.UI.DateBox dateSOCfmDate;
        private Win.UI.TextBox txtSONo;
        private Win.UI.ComboBox comboContainerType;
        private Win.UI.TextBox txtTerminalWhse;
        private Win.UI.TextBox txtCutoffDate;
        private Win.UI.TextBox txtfactoryShipper;
    }
}
