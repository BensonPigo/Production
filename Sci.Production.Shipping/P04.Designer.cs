namespace Sci.Production.Shipping
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
            this.labelFtyWKNo = new Sci.Win.UI.Label();
            this.labelForwarder = new Sci.Win.UI.Label();
            this.labelInvoiceNo = new Sci.Win.UI.Label();
            this.labelConsignee = new Sci.Win.UI.Label();
            this.labelPortofLoading = new Sci.Win.UI.Label();
            this.labelPortofDischarge = new Sci.Win.UI.Label();
            this.displayFtyWKNo = new Sci.Win.UI.DisplayBox();
            this.txtInvoiceNo = new Sci.Win.UI.TextBox();
            this.txtPortofLoading = new Sci.Win.UI.TextBox();
            this.txtPortofDischarge = new Sci.Win.UI.TextBox();
            this.displayPortofLoading = new Sci.Win.UI.DisplayBox();
            this.displayPortofDischarge = new Sci.Win.UI.DisplayBox();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioLocalPurchase = new Sci.Win.UI.RadioButton();
            this.radioTransferOut = new Sci.Win.UI.RadioButton();
            this.radioTransferIn = new Sci.Win.UI.RadioButton();
            this.radio3rdCountry = new Sci.Win.UI.RadioButton();
            this.labelShippingMode = new Sci.Win.UI.Label();
            this.labelContainerType = new Sci.Win.UI.Label();
            this.labelPackages = new Sci.Win.UI.Label();
            this.labelCBM = new Sci.Win.UI.Label();
            this.labelNWGW = new Sci.Win.UI.Label();
            this.numPackages = new Sci.Win.UI.NumericBox();
            this.numCBM = new Sci.Win.UI.NumericBox();
            this.numNetKg = new Sci.Win.UI.NumericBox();
            this.label15 = new Sci.Win.UI.Label();
            this.numWeightKg = new Sci.Win.UI.NumericBox();
            this.labelHandle = new Sci.Win.UI.Label();
            this.labelBLAWBNo = new Sci.Win.UI.Label();
            this.labelVslvoyFltNo = new Sci.Win.UI.Label();
            this.labelArrivePortDate = new Sci.Win.UI.Label();
            this.labelArriveWHDate = new Sci.Win.UI.Label();
            this.labelDoxRcvDate = new Sci.Win.UI.Label();
            this.txtBLAWBNo = new Sci.Win.UI.TextBox();
            this.txtVslvoyFltNo = new Sci.Win.UI.TextBox();
            this.dateArrivePortDate = new Sci.Win.UI.DateBox();
            this.dateArriveWHDate = new Sci.Win.UI.DateBox();
            this.dateDoxRcvDate = new Sci.Win.UI.DateBox();
            this.btnExpenseData = new Sci.Win.UI.Button();
            this.btnImportData = new Sci.Win.UI.Button();
            this.txtUserHandle = new Sci.Production.Class.Txtuser();
            this.txtdropdownlistContainerType = new Sci.Production.Class.Txtdropdownlist();
            this.txtSubconForwarder = new Sci.Production.Class.TxtsubconNoConfirm();
            this.labSisFtyWk = new Sci.Win.UI.Label();
            this.txtSisFtyWK = new Sci.Win.UI.TextBox();
            this.labShipper = new Sci.Win.UI.Label();
            this.txtLocalSupp = new Sci.Production.Class.TxtLocalSupp();
            this.labOnBoard = new Sci.Win.UI.Label();
            this.dateOnBoardDate = new Sci.Win.UI.DateBox();
            this.comboShippMode = new Sci.Win.UI.ComboBox();
            this.chkNoCharge = new Sci.Win.UI.CheckBox();
            this.label6 = new Sci.Win.UI.Label();
            this.displayCustomsDeclareNo = new Sci.Win.UI.DisplayBox();
            this.lbDeclareation = new Sci.Win.UI.Label();
            this.displayDeclarationID = new Sci.Win.UI.DisplayBox();
            this.chkNonDeclare = new Sci.Win.UI.CheckBox();
            this.txtLocalSupp1 = new Sci.Production.Class.TxtLocalSupp();
            this.lbShipDate = new Sci.Win.UI.Label();
            this.dateShipDate = new Sci.Win.UI.DateBox();
            this.btnConsigneeMail = new Sci.Win.UI.Button();
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
            this.browse.SuspendLayout();
            this.tabs.SuspendLayout();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.lbShipDate);
            this.masterpanel.Controls.Add(this.txtLocalSupp1);
            this.masterpanel.Controls.Add(this.chkNonDeclare);
            this.masterpanel.Controls.Add(this.label6);
            this.masterpanel.Controls.Add(this.displayCustomsDeclareNo);
            this.masterpanel.Controls.Add(this.lbDeclareation);
            this.masterpanel.Controls.Add(this.displayDeclarationID);
            this.masterpanel.Controls.Add(this.chkNoCharge);
            this.masterpanel.Controls.Add(this.comboShippMode);
            this.masterpanel.Controls.Add(this.labOnBoard);
            this.masterpanel.Controls.Add(this.txtLocalSupp);
            this.masterpanel.Controls.Add(this.labShipper);
            this.masterpanel.Controls.Add(this.txtSisFtyWK);
            this.masterpanel.Controls.Add(this.labSisFtyWk);
            this.masterpanel.Controls.Add(this.btnImportData);
            this.masterpanel.Controls.Add(this.btnExpenseData);
            this.masterpanel.Controls.Add(this.txtVslvoyFltNo);
            this.masterpanel.Controls.Add(this.txtBLAWBNo);
            this.masterpanel.Controls.Add(this.txtUserHandle);
            this.masterpanel.Controls.Add(this.labelDoxRcvDate);
            this.masterpanel.Controls.Add(this.labelArriveWHDate);
            this.masterpanel.Controls.Add(this.labelArrivePortDate);
            this.masterpanel.Controls.Add(this.labelVslvoyFltNo);
            this.masterpanel.Controls.Add(this.labelBLAWBNo);
            this.masterpanel.Controls.Add(this.labelHandle);
            this.masterpanel.Controls.Add(this.numWeightKg);
            this.masterpanel.Controls.Add(this.label15);
            this.masterpanel.Controls.Add(this.numNetKg);
            this.masterpanel.Controls.Add(this.numCBM);
            this.masterpanel.Controls.Add(this.numPackages);
            this.masterpanel.Controls.Add(this.txtdropdownlistContainerType);
            this.masterpanel.Controls.Add(this.labelNWGW);
            this.masterpanel.Controls.Add(this.labelCBM);
            this.masterpanel.Controls.Add(this.labelPackages);
            this.masterpanel.Controls.Add(this.labelContainerType);
            this.masterpanel.Controls.Add(this.labelShippingMode);
            this.masterpanel.Controls.Add(this.dateShipDate);
            this.masterpanel.Controls.Add(this.dateOnBoardDate);
            this.masterpanel.Controls.Add(this.dateDoxRcvDate);
            this.masterpanel.Controls.Add(this.dateArriveWHDate);
            this.masterpanel.Controls.Add(this.dateArrivePortDate);
            this.masterpanel.Controls.Add(this.radioPanel1);
            this.masterpanel.Controls.Add(this.displayPortofDischarge);
            this.masterpanel.Controls.Add(this.displayPortofLoading);
            this.masterpanel.Controls.Add(this.txtPortofDischarge);
            this.masterpanel.Controls.Add(this.txtPortofLoading);
            this.masterpanel.Controls.Add(this.txtInvoiceNo);
            this.masterpanel.Controls.Add(this.txtSubconForwarder);
            this.masterpanel.Controls.Add(this.displayFtyWKNo);
            this.masterpanel.Controls.Add(this.labelPortofDischarge);
            this.masterpanel.Controls.Add(this.labelPortofLoading);
            this.masterpanel.Controls.Add(this.labelConsignee);
            this.masterpanel.Controls.Add(this.labelInvoiceNo);
            this.masterpanel.Controls.Add(this.labelForwarder);
            this.masterpanel.Controls.Add(this.labelFtyWKNo);
            this.masterpanel.Size = new System.Drawing.Size(1024, 254);
            this.masterpanel.TabIndex = 2;
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFtyWKNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelForwarder, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelInvoiceNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelConsignee, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPortofLoading, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPortofDischarge, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayFtyWKNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSubconForwarder, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtInvoiceNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtPortofLoading, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtPortofDischarge, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayPortofLoading, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayPortofDischarge, 0);
            this.masterpanel.Controls.SetChildIndex(this.radioPanel1, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateArrivePortDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateArriveWHDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateDoxRcvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateOnBoardDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateShipDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShippingMode, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelContainerType, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPackages, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCBM, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNWGW, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtdropdownlistContainerType, 0);
            this.masterpanel.Controls.SetChildIndex(this.numPackages, 0);
            this.masterpanel.Controls.SetChildIndex(this.numCBM, 0);
            this.masterpanel.Controls.SetChildIndex(this.numNetKg, 0);
            this.masterpanel.Controls.SetChildIndex(this.label15, 0);
            this.masterpanel.Controls.SetChildIndex(this.numWeightKg, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelBLAWBNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelVslvoyFltNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelArrivePortDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelArriveWHDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDoxRcvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtUserHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtBLAWBNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtVslvoyFltNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnExpenseData, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportData, 0);
            this.masterpanel.Controls.SetChildIndex(this.labSisFtyWk, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSisFtyWK, 0);
            this.masterpanel.Controls.SetChildIndex(this.labShipper, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLocalSupp, 0);
            this.masterpanel.Controls.SetChildIndex(this.labOnBoard, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboShippMode, 0);
            this.masterpanel.Controls.SetChildIndex(this.chkNoCharge, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayDeclarationID, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbDeclareation, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayCustomsDeclareNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.label6, 0);
            this.masterpanel.Controls.SetChildIndex(this.chkNonDeclare, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLocalSupp1, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbShipDate, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 254);
            this.detailpanel.Size = new System.Drawing.Size(1024, 202);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(887, 218);
            this.gridicon.TabIndex = 80;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(910, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1024, 202);
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
            this.detail.Size = new System.Drawing.Size(1024, 494);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1024, 456);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 456);
            this.detailbtm.Size = new System.Drawing.Size(1024, 38);
            // 
            // browse
            // 
            this.browse.Controls.Add(this.btnConsigneeMail);
            this.browse.Size = new System.Drawing.Size(1024, 494);
            this.browse.Controls.SetChildIndex(this.btnConsigneeMail, 0);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1032, 523);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelFtyWKNo
            // 
            this.labelFtyWKNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelFtyWKNo.Location = new System.Drawing.Point(5, 6);
            this.labelFtyWKNo.Name = "labelFtyWKNo";
            this.labelFtyWKNo.Size = new System.Drawing.Size(100, 23);
            this.labelFtyWKNo.TabIndex = 12;
            this.labelFtyWKNo.Text = "Fty WK No.";
            // 
            // labelForwarder
            // 
            this.labelForwarder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelForwarder.Location = new System.Drawing.Point(5, 61);
            this.labelForwarder.Name = "labelForwarder";
            this.labelForwarder.Size = new System.Drawing.Size(100, 23);
            this.labelForwarder.TabIndex = 13;
            this.labelForwarder.Text = "Forwarder";
            // 
            // labelInvoiceNo
            // 
            this.labelInvoiceNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelInvoiceNo.Location = new System.Drawing.Point(5, 89);
            this.labelInvoiceNo.Name = "labelInvoiceNo";
            this.labelInvoiceNo.Size = new System.Drawing.Size(100, 23);
            this.labelInvoiceNo.TabIndex = 14;
            this.labelInvoiceNo.Text = "Invoice No.";
            // 
            // labelConsignee
            // 
            this.labelConsignee.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelConsignee.Location = new System.Drawing.Point(5, 146);
            this.labelConsignee.Name = "labelConsignee";
            this.labelConsignee.Size = new System.Drawing.Size(100, 23);
            this.labelConsignee.TabIndex = 15;
            this.labelConsignee.Text = "Consignee";
            // 
            // labelPortofLoading
            // 
            this.labelPortofLoading.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelPortofLoading.Location = new System.Drawing.Point(5, 174);
            this.labelPortofLoading.Name = "labelPortofLoading";
            this.labelPortofLoading.Size = new System.Drawing.Size(100, 23);
            this.labelPortofLoading.TabIndex = 16;
            this.labelPortofLoading.Text = "Port of Loading";
            // 
            // labelPortofDischarge
            // 
            this.labelPortofDischarge.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelPortofDischarge.Location = new System.Drawing.Point(5, 202);
            this.labelPortofDischarge.Name = "labelPortofDischarge";
            this.labelPortofDischarge.Size = new System.Drawing.Size(100, 23);
            this.labelPortofDischarge.TabIndex = 17;
            this.labelPortofDischarge.Text = "Port of Discharge";
            // 
            // displayFtyWKNo
            // 
            this.displayFtyWKNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFtyWKNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayFtyWKNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFtyWKNo.Location = new System.Drawing.Point(109, 6);
            this.displayFtyWKNo.Name = "displayFtyWKNo";
            this.displayFtyWKNo.Size = new System.Drawing.Size(120, 23);
            this.displayFtyWKNo.TabIndex = 1;
            // 
            // txtInvoiceNo
            // 
            this.txtInvoiceNo.BackColor = System.Drawing.Color.White;
            this.txtInvoiceNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "INVNo", true));
            this.txtInvoiceNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvoiceNo.Location = new System.Drawing.Point(109, 89);
            this.txtInvoiceNo.Name = "txtInvoiceNo";
            this.txtInvoiceNo.Size = new System.Drawing.Size(230, 23);
            this.txtInvoiceNo.TabIndex = 5;
            // 
            // txtPortofLoading
            // 
            this.txtPortofLoading.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtPortofLoading.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ExportPort", true));
            this.txtPortofLoading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtPortofLoading.IsSupportEditMode = false;
            this.txtPortofLoading.Location = new System.Drawing.Point(109, 174);
            this.txtPortofLoading.Name = "txtPortofLoading";
            this.txtPortofLoading.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtPortofLoading.ReadOnly = true;
            this.txtPortofLoading.Size = new System.Drawing.Size(185, 23);
            this.txtPortofLoading.TabIndex = 8;
            this.txtPortofLoading.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtPortofLoading_PopUp);
            // 
            // txtPortofDischarge
            // 
            this.txtPortofDischarge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtPortofDischarge.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ImportPort", true));
            this.txtPortofDischarge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtPortofDischarge.IsSupportEditMode = false;
            this.txtPortofDischarge.Location = new System.Drawing.Point(109, 202);
            this.txtPortofDischarge.Name = "txtPortofDischarge";
            this.txtPortofDischarge.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtPortofDischarge.ReadOnly = true;
            this.txtPortofDischarge.Size = new System.Drawing.Size(185, 23);
            this.txtPortofDischarge.TabIndex = 10;
            this.txtPortofDischarge.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtPortofDischarge_PopUp);
            // 
            // displayPortofLoading
            // 
            this.displayPortofLoading.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPortofLoading.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ExportCountry", true));
            this.displayPortofLoading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPortofLoading.Location = new System.Drawing.Point(296, 174);
            this.displayPortofLoading.Name = "displayPortofLoading";
            this.displayPortofLoading.Size = new System.Drawing.Size(32, 23);
            this.displayPortofLoading.TabIndex = 9;
            // 
            // displayPortofDischarge
            // 
            this.displayPortofDischarge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPortofDischarge.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ImportCountry", true));
            this.displayPortofDischarge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPortofDischarge.Location = new System.Drawing.Point(296, 202);
            this.displayPortofDischarge.Name = "displayPortofDischarge";
            this.displayPortofDischarge.Size = new System.Drawing.Size(32, 23);
            this.displayPortofDischarge.TabIndex = 11;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioLocalPurchase);
            this.radioPanel1.Controls.Add(this.radioTransferOut);
            this.radioPanel1.Controls.Add(this.radioTransferIn);
            this.radioPanel1.Controls.Add(this.radio3rdCountry);
            this.radioPanel1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Type", true));
            this.radioPanel1.Location = new System.Drawing.Point(228, 4);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(376, 25);
            this.radioPanel1.TabIndex = 0;
            this.radioPanel1.ValueChanged += new System.EventHandler(this.RadioPanel1_ValueChanged);
            // 
            // radioLocalPurchase
            // 
            this.radioLocalPurchase.AutoSize = true;
            this.radioLocalPurchase.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.radioLocalPurchase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioLocalPurchase.Location = new System.Drawing.Point(266, 2);
            this.radioLocalPurchase.Name = "radioLocalPurchase";
            this.radioLocalPurchase.Size = new System.Drawing.Size(110, 19);
            this.radioLocalPurchase.TabIndex = 5;
            this.radioLocalPurchase.TabStop = true;
            this.radioLocalPurchase.Text = "Local Purchase";
            this.radioLocalPurchase.UseVisualStyleBackColor = true;
            this.radioLocalPurchase.Value = "4";
            // 
            // radioTransferOut
            // 
            this.radioTransferOut.AutoSize = true;
            this.radioTransferOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.radioTransferOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioTransferOut.Location = new System.Drawing.Point(172, 2);
            this.radioTransferOut.Name = "radioTransferOut";
            this.radioTransferOut.Size = new System.Drawing.Size(92, 19);
            this.radioTransferOut.TabIndex = 4;
            this.radioTransferOut.TabStop = true;
            this.radioTransferOut.Text = "Transfer Out";
            this.radioTransferOut.UseVisualStyleBackColor = true;
            this.radioTransferOut.Value = "3";
            // 
            // radioTransferIn
            // 
            this.radioTransferIn.AutoSize = true;
            this.radioTransferIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.radioTransferIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioTransferIn.Location = new System.Drawing.Point(91, 2);
            this.radioTransferIn.Name = "radioTransferIn";
            this.radioTransferIn.Size = new System.Drawing.Size(83, 19);
            this.radioTransferIn.TabIndex = 3;
            this.radioTransferIn.TabStop = true;
            this.radioTransferIn.Text = "Transfer In";
            this.radioTransferIn.UseVisualStyleBackColor = true;
            this.radioTransferIn.Value = "2";
            this.radioTransferIn.CheckedChanged += new System.EventHandler(this.RadioTransferIn_CheckedChanged);
            // 
            // radio3rdCountry
            // 
            this.radio3rdCountry.AutoSize = true;
            this.radio3rdCountry.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.radio3rdCountry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radio3rdCountry.Location = new System.Drawing.Point(4, 2);
            this.radio3rdCountry.Name = "radio3rdCountry";
            this.radio3rdCountry.Size = new System.Drawing.Size(87, 19);
            this.radio3rdCountry.TabIndex = 0;
            this.radio3rdCountry.TabStop = true;
            this.radio3rdCountry.Text = "3rd Country";
            this.radio3rdCountry.UseVisualStyleBackColor = true;
            this.radio3rdCountry.Value = "1";
            // 
            // labelShippingMode
            // 
            this.labelShippingMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelShippingMode.Location = new System.Drawing.Point(340, 32);
            this.labelShippingMode.Name = "labelShippingMode";
            this.labelShippingMode.Size = new System.Drawing.Size(123, 23);
            this.labelShippingMode.TabIndex = 16;
            this.labelShippingMode.Text = "Shipping Mode";
            // 
            // labelContainerType
            // 
            this.labelContainerType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelContainerType.Location = new System.Drawing.Point(340, 61);
            this.labelContainerType.Name = "labelContainerType";
            this.labelContainerType.Size = new System.Drawing.Size(123, 23);
            this.labelContainerType.TabIndex = 17;
            this.labelContainerType.Text = "Container Type";
            // 
            // labelPackages
            // 
            this.labelPackages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelPackages.Location = new System.Drawing.Point(340, 89);
            this.labelPackages.Name = "labelPackages";
            this.labelPackages.Size = new System.Drawing.Size(123, 23);
            this.labelPackages.TabIndex = 18;
            this.labelPackages.Text = "Packages";
            // 
            // labelCBM
            // 
            this.labelCBM.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelCBM.Location = new System.Drawing.Point(340, 117);
            this.labelCBM.Name = "labelCBM";
            this.labelCBM.Size = new System.Drawing.Size(123, 23);
            this.labelCBM.TabIndex = 19;
            this.labelCBM.Text = "CBM";
            // 
            // labelNWGW
            // 
            this.labelNWGW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelNWGW.Location = new System.Drawing.Point(340, 145);
            this.labelNWGW.Name = "labelNWGW";
            this.labelNWGW.Size = new System.Drawing.Size(123, 23);
            this.labelNWGW.TabIndex = 20;
            this.labelNWGW.Text = "N.W./G.W.";
            // 
            // numPackages
            // 
            this.numPackages.BackColor = System.Drawing.Color.White;
            this.numPackages.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Packages", true));
            this.numPackages.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numPackages.Location = new System.Drawing.Point(467, 89);
            this.numPackages.Name = "numPackages";
            this.numPackages.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPackages.Size = new System.Drawing.Size(67, 23);
            this.numPackages.TabIndex = 14;
            this.numPackages.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numCBM
            // 
            this.numCBM.BackColor = System.Drawing.Color.White;
            this.numCBM.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Cbm", true));
            this.numCBM.DecimalPlaces = 3;
            this.numCBM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numCBM.Location = new System.Drawing.Point(467, 117);
            this.numCBM.Name = "numCBM";
            this.numCBM.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCBM.Size = new System.Drawing.Size(67, 23);
            this.numCBM.TabIndex = 15;
            this.numCBM.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numNetKg
            // 
            this.numNetKg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numNetKg.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NetKg", true));
            this.numNetKg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numNetKg.IsSupportEditMode = false;
            this.numNetKg.Location = new System.Drawing.Point(466, 145);
            this.numNetKg.Name = "numNetKg";
            this.numNetKg.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numNetKg.ReadOnly = true;
            this.numNetKg.Size = new System.Drawing.Size(65, 23);
            this.numNetKg.TabIndex = 16;
            this.numNetKg.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Location = new System.Drawing.Point(531, 145);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(10, 23);
            this.label15.TabIndex = 29;
            this.label15.Text = "/";
            this.label15.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label15.TextStyle.Color = System.Drawing.Color.Black;
            this.label15.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label15.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // numWeightKg
            // 
            this.numWeightKg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numWeightKg.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "WeightKg", true));
            this.numWeightKg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numWeightKg.IsSupportEditMode = false;
            this.numWeightKg.Location = new System.Drawing.Point(539, 146);
            this.numWeightKg.Name = "numWeightKg";
            this.numWeightKg.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWeightKg.ReadOnly = true;
            this.numWeightKg.Size = new System.Drawing.Size(65, 23);
            this.numWeightKg.TabIndex = 17;
            this.numWeightKg.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelHandle
            // 
            this.labelHandle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelHandle.Location = new System.Drawing.Point(606, 6);
            this.labelHandle.Name = "labelHandle";
            this.labelHandle.Size = new System.Drawing.Size(93, 23);
            this.labelHandle.TabIndex = 31;
            this.labelHandle.Text = "Handle";
            // 
            // labelBLAWBNo
            // 
            this.labelBLAWBNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelBLAWBNo.Location = new System.Drawing.Point(606, 34);
            this.labelBLAWBNo.Name = "labelBLAWBNo";
            this.labelBLAWBNo.Size = new System.Drawing.Size(93, 23);
            this.labelBLAWBNo.TabIndex = 32;
            this.labelBLAWBNo.Text = "B/L(AWB) No.";
            // 
            // labelVslvoyFltNo
            // 
            this.labelVslvoyFltNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelVslvoyFltNo.Location = new System.Drawing.Point(606, 62);
            this.labelVslvoyFltNo.Name = "labelVslvoyFltNo";
            this.labelVslvoyFltNo.Size = new System.Drawing.Size(93, 23);
            this.labelVslvoyFltNo.TabIndex = 33;
            this.labelVslvoyFltNo.Text = "Vsl voy/Flt No.";
            // 
            // labelArrivePortDate
            // 
            this.labelArrivePortDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelArrivePortDate.Location = new System.Drawing.Point(606, 145);
            this.labelArrivePortDate.Name = "labelArrivePortDate";
            this.labelArrivePortDate.Size = new System.Drawing.Size(93, 23);
            this.labelArrivePortDate.TabIndex = 34;
            this.labelArrivePortDate.Text = "Arrive Port Date";
            // 
            // labelArriveWHDate
            // 
            this.labelArriveWHDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelArriveWHDate.Location = new System.Drawing.Point(606, 173);
            this.labelArriveWHDate.Name = "labelArriveWHDate";
            this.labelArriveWHDate.Size = new System.Drawing.Size(93, 23);
            this.labelArriveWHDate.TabIndex = 35;
            this.labelArriveWHDate.Text = "Arrive W/H Date";
            // 
            // labelDoxRcvDate
            // 
            this.labelDoxRcvDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelDoxRcvDate.Location = new System.Drawing.Point(606, 201);
            this.labelDoxRcvDate.Name = "labelDoxRcvDate";
            this.labelDoxRcvDate.Size = new System.Drawing.Size(93, 23);
            this.labelDoxRcvDate.TabIndex = 36;
            this.labelDoxRcvDate.Text = "Dox Rcv Date";
            // 
            // txtBLAWBNo
            // 
            this.txtBLAWBNo.BackColor = System.Drawing.Color.White;
            this.txtBLAWBNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Blno", true));
            this.txtBLAWBNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBLAWBNo.Location = new System.Drawing.Point(702, 34);
            this.txtBLAWBNo.Name = "txtBLAWBNo";
            this.txtBLAWBNo.Size = new System.Drawing.Size(185, 23);
            this.txtBLAWBNo.TabIndex = 21;
            // 
            // txtVslvoyFltNo
            // 
            this.txtVslvoyFltNo.BackColor = System.Drawing.Color.White;
            this.txtVslvoyFltNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Vessel", true));
            this.txtVslvoyFltNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtVslvoyFltNo.Location = new System.Drawing.Point(702, 62);
            this.txtVslvoyFltNo.Name = "txtVslvoyFltNo";
            this.txtVslvoyFltNo.Size = new System.Drawing.Size(254, 23);
            this.txtVslvoyFltNo.TabIndex = 22;
            // 
            // dateArrivePortDate
            // 
            this.dateArrivePortDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PortArrival", true));
            this.dateArrivePortDate.Location = new System.Drawing.Point(702, 145);
            this.dateArrivePortDate.Name = "dateArrivePortDate";
            this.dateArrivePortDate.Size = new System.Drawing.Size(130, 23);
            this.dateArrivePortDate.TabIndex = 25;
            // 
            // dateArriveWHDate
            // 
            this.dateArriveWHDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "WhseArrival", true));
            this.dateArriveWHDate.IsSupportEditMode = false;
            this.dateArriveWHDate.Location = new System.Drawing.Point(702, 173);
            this.dateArriveWHDate.Name = "dateArriveWHDate";
            this.dateArriveWHDate.ReadOnly = true;
            this.dateArriveWHDate.Size = new System.Drawing.Size(130, 23);
            this.dateArriveWHDate.TabIndex = 26;
            // 
            // dateDoxRcvDate
            // 
            this.dateDoxRcvDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "DocArrival", true));
            this.dateDoxRcvDate.Location = new System.Drawing.Point(702, 201);
            this.dateDoxRcvDate.Name = "dateDoxRcvDate";
            this.dateDoxRcvDate.Size = new System.Drawing.Size(130, 23);
            this.dateDoxRcvDate.TabIndex = 27;
            // 
            // btnExpenseData
            // 
            this.btnExpenseData.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnExpenseData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnExpenseData.Location = new System.Drawing.Point(886, 91);
            this.btnExpenseData.Name = "btnExpenseData";
            this.btnExpenseData.Size = new System.Drawing.Size(106, 30);
            this.btnExpenseData.TabIndex = 90;
            this.btnExpenseData.Text = "Expense Data";
            this.btnExpenseData.UseVisualStyleBackColor = true;
            this.btnExpenseData.Click += new System.EventHandler(this.BtnExpenseData_Click);
            // 
            // btnImportData
            // 
            this.btnImportData.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImportData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnImportData.Location = new System.Drawing.Point(899, 127);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(93, 30);
            this.btnImportData.TabIndex = 91;
            this.btnImportData.Text = "Import Data";
            this.btnImportData.UseVisualStyleBackColor = true;
            this.btnImportData.Click += new System.EventHandler(this.BtnImportData_Click);
            // 
            // txtUserHandle
            // 
            this.txtUserHandle.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Handle", true));
            this.txtUserHandle.DisplayBox1Binding = "";
            this.txtUserHandle.Location = new System.Drawing.Point(702, 5);
            this.txtUserHandle.Name = "txtUserHandle";
            this.txtUserHandle.Size = new System.Drawing.Size(302, 23);
            this.txtUserHandle.TabIndex = 20;
            this.txtUserHandle.TextBox1Binding = "";
            // 
            // txtdropdownlistContainerType
            // 
            this.txtdropdownlistContainerType.BackColor = System.Drawing.Color.White;
            this.txtdropdownlistContainerType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "CYCFS", true));
            this.txtdropdownlistContainerType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlistContainerType.FormattingEnabled = true;
            this.txtdropdownlistContainerType.IsSupportUnselect = true;
            this.txtdropdownlistContainerType.Location = new System.Drawing.Point(467, 60);
            this.txtdropdownlistContainerType.Name = "txtdropdownlistContainerType";
            this.txtdropdownlistContainerType.OldText = "";
            this.txtdropdownlistContainerType.Size = new System.Drawing.Size(121, 24);
            this.txtdropdownlistContainerType.TabIndex = 13;
            this.txtdropdownlistContainerType.Type = "ContainerCY";
            // 
            // txtSubconForwarder
            // 
            this.txtSubconForwarder.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Forwarder", true));
            this.txtSubconForwarder.DisplayBox1Binding = "";
            this.txtSubconForwarder.IsIncludeJunk = false;
            this.txtSubconForwarder.IsMisc = false;
            this.txtSubconForwarder.IsShipping = false;
            this.txtSubconForwarder.IsSubcon = false;
            this.txtSubconForwarder.Location = new System.Drawing.Point(109, 61);
            this.txtSubconForwarder.Name = "txtSubconForwarder";
            this.txtSubconForwarder.Size = new System.Drawing.Size(170, 23);
            this.txtSubconForwarder.TabIndex = 4;
            this.txtSubconForwarder.TextBox1Binding = "";
            // 
            // labSisFtyWk
            // 
            this.labSisFtyWk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labSisFtyWk.Location = new System.Drawing.Point(5, 34);
            this.labSisFtyWk.Name = "labSisFtyWk";
            this.labSisFtyWk.Size = new System.Drawing.Size(100, 23);
            this.labSisFtyWk.TabIndex = 44;
            this.labSisFtyWk.Text = "Sis Fty WK#";
            // 
            // txtSisFtyWK
            // 
            this.txtSisFtyWK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSisFtyWK.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SisFtyID", true));
            this.txtSisFtyWK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSisFtyWK.IsSupportEditMode = false;
            this.txtSisFtyWK.Location = new System.Drawing.Point(109, 32);
            this.txtSisFtyWK.Name = "txtSisFtyWK";
            this.txtSisFtyWK.ReadOnly = true;
            this.txtSisFtyWK.Size = new System.Drawing.Size(120, 23);
            this.txtSisFtyWK.TabIndex = 3;
            // 
            // labShipper
            // 
            this.labShipper.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labShipper.Location = new System.Drawing.Point(5, 118);
            this.labShipper.Name = "labShipper";
            this.labShipper.Size = new System.Drawing.Size(100, 23);
            this.labShipper.TabIndex = 46;
            this.labShipper.Text = "Shipper";
            // 
            // txtLocalSupp
            // 
            this.txtLocalSupp.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Shipper", true));
            this.txtLocalSupp.DisplayBox1Binding = "";
            this.txtLocalSupp.IsFactory = false;
            this.txtLocalSupp.Location = new System.Drawing.Point(109, 117);
            this.txtLocalSupp.Name = "txtLocalSupp";
            this.txtLocalSupp.Size = new System.Drawing.Size(230, 23);
            this.txtLocalSupp.TabIndex = 6;
            this.txtLocalSupp.TextBox1Binding = "";
            // 
            // labOnBoard
            // 
            this.labOnBoard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labOnBoard.Location = new System.Drawing.Point(606, 117);
            this.labOnBoard.Name = "labOnBoard";
            this.labOnBoard.Size = new System.Drawing.Size(93, 23);
            this.labOnBoard.TabIndex = 49;
            this.labOnBoard.Text = "On Board Date";
            // 
            // dateOnBoardDate
            // 
            this.dateOnBoardDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "OnBoard", true));
            this.dateOnBoardDate.Location = new System.Drawing.Point(702, 117);
            this.dateOnBoardDate.Name = "dateOnBoardDate";
            this.dateOnBoardDate.Size = new System.Drawing.Size(130, 23);
            this.dateOnBoardDate.TabIndex = 24;
            // 
            // comboShippMode
            // 
            this.comboShippMode.BackColor = System.Drawing.Color.White;
            this.comboShippMode.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "ShipModeID", true));
            this.comboShippMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShippMode.FormattingEnabled = true;
            this.comboShippMode.IsSupportUnselect = true;
            this.comboShippMode.Location = new System.Drawing.Point(467, 32);
            this.comboShippMode.Name = "comboShippMode";
            this.comboShippMode.OldText = "";
            this.comboShippMode.Size = new System.Drawing.Size(121, 24);
            this.comboShippMode.TabIndex = 12;
            // 
            // chkNoCharge
            // 
            this.chkNoCharge.AutoSize = true;
            this.chkNoCharge.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NoCharges", true));
            this.chkNoCharge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkNoCharge.Location = new System.Drawing.Point(838, 186);
            this.chkNoCharge.Name = "chkNoCharge";
            this.chkNoCharge.Size = new System.Drawing.Size(182, 21);
            this.chkNoCharge.TabIndex = 29;
            this.chkNoCharge.Text = "No Import/Export Charge";
            this.chkNoCharge.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label6.Location = new System.Drawing.Point(340, 201);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 23);
            this.label6.TabIndex = 68;
            this.label6.Text = "Customs Declare No";
            // 
            // displayCustomsDeclareNo
            // 
            this.displayCustomsDeclareNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCustomsDeclareNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCustomsDeclareNo.Location = new System.Drawing.Point(467, 203);
            this.displayCustomsDeclareNo.Name = "displayCustomsDeclareNo";
            this.displayCustomsDeclareNo.Size = new System.Drawing.Size(137, 23);
            this.displayCustomsDeclareNo.TabIndex = 19;
            // 
            // lbDeclareation
            // 
            this.lbDeclareation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lbDeclareation.Location = new System.Drawing.Point(340, 173);
            this.lbDeclareation.Name = "lbDeclareation";
            this.lbDeclareation.Size = new System.Drawing.Size(124, 23);
            this.lbDeclareation.TabIndex = 66;
            this.lbDeclareation.Text = "Import Declaration ID";
            // 
            // displayDeclarationID
            // 
            this.displayDeclarationID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDeclarationID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDeclarationID.Location = new System.Drawing.Point(467, 174);
            this.displayDeclarationID.Name = "displayDeclarationID";
            this.displayDeclarationID.Size = new System.Drawing.Size(137, 23);
            this.displayDeclarationID.TabIndex = 18;
            // 
            // chkNonDeclare
            // 
            this.chkNonDeclare.AutoSize = true;
            this.chkNonDeclare.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NonDeclare", true));
            this.chkNonDeclare.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkNonDeclare.Location = new System.Drawing.Point(838, 163);
            this.chkNonDeclare.Name = "chkNonDeclare";
            this.chkNonDeclare.Size = new System.Drawing.Size(106, 21);
            this.chkNonDeclare.TabIndex = 28;
            this.chkNonDeclare.Text = "Non Declare";
            this.chkNonDeclare.UseVisualStyleBackColor = true;
            // 
            // txtLocalSupp1
            // 
            this.txtLocalSupp1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Consignee", true));
            this.txtLocalSupp1.DisplayBox1Binding = "";
            this.txtLocalSupp1.IsFactory = true;
            this.txtLocalSupp1.Location = new System.Drawing.Point(109, 146);
            this.txtLocalSupp1.Name = "txtLocalSupp1";
            this.txtLocalSupp1.Size = new System.Drawing.Size(228, 23);
            this.txtLocalSupp1.TabIndex = 7;
            this.txtLocalSupp1.TextBox1Binding = "";
            // 
            // lbShipDate
            // 
            this.lbShipDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lbShipDate.Location = new System.Drawing.Point(606, 89);
            this.lbShipDate.Name = "lbShipDate";
            this.lbShipDate.Size = new System.Drawing.Size(93, 23);
            this.lbShipDate.TabIndex = 71;
            this.lbShipDate.Text = "Ship Date";
            // 
            // dateShipDate
            // 
            this.dateShipDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ShipDate", true));
            this.dateShipDate.Location = new System.Drawing.Point(702, 89);
            this.dateShipDate.Name = "dateShipDate";
            this.dateShipDate.Size = new System.Drawing.Size(130, 23);
            this.dateShipDate.TabIndex = 23;
            // 
            // btnConsigneeMail
            // 
            this.btnConsigneeMail.Location = new System.Drawing.Point(627, 6);
            this.btnConsigneeMail.Name = "btnConsigneeMail";
            this.btnConsigneeMail.Size = new System.Drawing.Size(135, 29);
            this.btnConsigneeMail.TabIndex = 4;
            this.btnConsigneeMail.Text = "Consignee Mail";
            this.btnConsigneeMail.UseVisualStyleBackColor = true;
            this.btnConsigneeMail.Click += new System.EventHandler(this.BtnConsigneeMail_Click);
            // 
            // P04
            // 
            this.ClientSize = new System.Drawing.Size(1032, 556);
            this.DefaultControl = "txtSubconForwarder";
            this.DefaultControlForEdit = "txtSubconForwarder";
            this.DefaultDetailOrder = "POID,Seq1,Seq2";
            this.DefaultOrder = "ID";
            this.GridAlias = "FtyExport_Detail";
            this.GridNew = 0;
            this.GridUniqueKey = "ID,POID,Seq1,Seq2,SCIRefNo,RefNo,LocalPOID";
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "ID";
            this.Name = "P04";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P04. Raw Material Shipment Data Maintain";
            this.UniqueExpress = "ID";
            this.WorkAlias = "FtyExport";
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
            this.browse.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.NumericBox numCBM;
        private Win.UI.NumericBox numPackages;
        private Class.Txtdropdownlist txtdropdownlistContainerType;
        private Win.UI.Label labelNWGW;
        private Win.UI.Label labelCBM;
        private Win.UI.Label labelPackages;
        private Win.UI.Label labelContainerType;
        private Win.UI.Label labelShippingMode;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioLocalPurchase;
        private Win.UI.RadioButton radioTransferOut;
        private Win.UI.RadioButton radioTransferIn;
        private Win.UI.RadioButton radio3rdCountry;
        private Win.UI.DisplayBox displayPortofDischarge;
        private Win.UI.DisplayBox displayPortofLoading;
        private Win.UI.TextBox txtPortofDischarge;
        private Win.UI.TextBox txtPortofLoading;
        private Win.UI.TextBox txtInvoiceNo;
        private Class.TxtsubconNoConfirm txtSubconForwarder;
        private Win.UI.DisplayBox displayFtyWKNo;
        private Win.UI.Label labelPortofDischarge;
        private Win.UI.Label labelPortofLoading;
        private Win.UI.Label labelConsignee;
        private Win.UI.Label labelInvoiceNo;
        private Win.UI.Label labelForwarder;
        private Win.UI.Label labelFtyWKNo;
        private Win.UI.NumericBox numNetKg;
        private Win.UI.Label labelDoxRcvDate;
        private Win.UI.Label labelArriveWHDate;
        private Win.UI.Label labelArrivePortDate;
        private Win.UI.Label labelVslvoyFltNo;
        private Win.UI.Label labelBLAWBNo;
        private Win.UI.Label labelHandle;
        private Win.UI.NumericBox numWeightKg;
        private Win.UI.Label label15;
        private Win.UI.DateBox dateDoxRcvDate;
        private Win.UI.DateBox dateArriveWHDate;
        private Win.UI.DateBox dateArrivePortDate;
        private Win.UI.TextBox txtVslvoyFltNo;
        private Win.UI.TextBox txtBLAWBNo;
        private Class.Txtuser txtUserHandle;
        private Win.UI.Button btnImportData;
        private Win.UI.Button btnExpenseData;
        private Win.UI.TextBox txtSisFtyWK;
        private Win.UI.Label labSisFtyWk;
        private Class.TxtLocalSupp txtLocalSupp;
        private Win.UI.Label labShipper;
        private Win.UI.Label labOnBoard;
        private Win.UI.DateBox dateOnBoardDate;
        private Win.UI.ComboBox comboShippMode;
        private Win.UI.CheckBox chkNoCharge;
        private Win.UI.Label label6;
        private Win.UI.DisplayBox displayCustomsDeclareNo;
        private Win.UI.Label lbDeclareation;
        private Win.UI.DisplayBox displayDeclarationID;
        private Win.UI.CheckBox chkNonDeclare;
        private Class.TxtLocalSupp txtLocalSupp1;
        private Win.UI.DateBox dateShipDate;
        private Win.UI.Label lbShipDate;
        private Win.UI.Button btnConsigneeMail;
    }
}
