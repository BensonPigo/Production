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
            this.txtConsignee = new Sci.Win.UI.TextBox();
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
            this.txtUserHandle = new Sci.Production.Class.txtuser();
            this.txtdropdownlistContainerType = new Sci.Production.Class.txtdropdownlist();
            this.txtShipmodeShippingMode = new Sci.Production.Class.txtshipmode();
            this.txtSubconForwarder = new Sci.Production.Class.txtsubcon();
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
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
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
            this.masterpanel.Controls.Add(this.txtShipmodeShippingMode);
            this.masterpanel.Controls.Add(this.labelNWGW);
            this.masterpanel.Controls.Add(this.labelCBM);
            this.masterpanel.Controls.Add(this.labelPackages);
            this.masterpanel.Controls.Add(this.labelContainerType);
            this.masterpanel.Controls.Add(this.labelShippingMode);
            this.masterpanel.Controls.Add(this.dateDoxRcvDate);
            this.masterpanel.Controls.Add(this.dateArriveWHDate);
            this.masterpanel.Controls.Add(this.dateArrivePortDate);
            this.masterpanel.Controls.Add(this.radioPanel1);
            this.masterpanel.Controls.Add(this.displayPortofDischarge);
            this.masterpanel.Controls.Add(this.displayPortofLoading);
            this.masterpanel.Controls.Add(this.txtPortofDischarge);
            this.masterpanel.Controls.Add(this.txtPortofLoading);
            this.masterpanel.Controls.Add(this.txtConsignee);
            this.masterpanel.Controls.Add(this.txtInvoiceNo);
            this.masterpanel.Controls.Add(this.txtSubconForwarder);
            this.masterpanel.Controls.Add(this.displayFtyWKNo);
            this.masterpanel.Controls.Add(this.labelPortofDischarge);
            this.masterpanel.Controls.Add(this.labelPortofLoading);
            this.masterpanel.Controls.Add(this.labelConsignee);
            this.masterpanel.Controls.Add(this.labelInvoiceNo);
            this.masterpanel.Controls.Add(this.labelForwarder);
            this.masterpanel.Controls.Add(this.labelFtyWKNo);
            this.masterpanel.Size = new System.Drawing.Size(990, 176);
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
            this.masterpanel.Controls.SetChildIndex(this.txtConsignee, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtPortofLoading, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtPortofDischarge, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayPortofLoading, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayPortofDischarge, 0);
            this.masterpanel.Controls.SetChildIndex(this.radioPanel1, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateArrivePortDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateArriveWHDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateDoxRcvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShippingMode, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelContainerType, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPackages, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCBM, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNWGW, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtShipmodeShippingMode, 0);
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
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 176);
            this.detailpanel.Size = new System.Drawing.Size(990, 280);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(887, 140);
            this.gridicon.TabIndex = 13;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(910, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(990, 280);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(990, 494);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(984, 448);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(984, 40);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(990, 494);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(990, 456);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 456);
            this.detailbtm.Size = new System.Drawing.Size(990, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(990, 494);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(998, 523);
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
            this.labelFtyWKNo.Lines = 0;
            this.labelFtyWKNo.Location = new System.Drawing.Point(5, 6);
            this.labelFtyWKNo.Name = "labelFtyWKNo";
            this.labelFtyWKNo.Size = new System.Drawing.Size(67, 23);
            this.labelFtyWKNo.TabIndex = 12;
            this.labelFtyWKNo.Text = "Fty WK No.";
            // 
            // labelForwarder
            // 
            this.labelForwarder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelForwarder.Lines = 0;
            this.labelForwarder.Location = new System.Drawing.Point(5, 34);
            this.labelForwarder.Name = "labelForwarder";
            this.labelForwarder.Size = new System.Drawing.Size(67, 23);
            this.labelForwarder.TabIndex = 13;
            this.labelForwarder.Text = "Forwarder";
            // 
            // labelInvoiceNo
            // 
            this.labelInvoiceNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelInvoiceNo.Lines = 0;
            this.labelInvoiceNo.Location = new System.Drawing.Point(5, 62);
            this.labelInvoiceNo.Name = "labelInvoiceNo";
            this.labelInvoiceNo.Size = new System.Drawing.Size(67, 23);
            this.labelInvoiceNo.TabIndex = 14;
            this.labelInvoiceNo.Text = "Invoice No.";
            // 
            // labelConsignee
            // 
            this.labelConsignee.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelConsignee.Lines = 0;
            this.labelConsignee.Location = new System.Drawing.Point(5, 90);
            this.labelConsignee.Name = "labelConsignee";
            this.labelConsignee.Size = new System.Drawing.Size(67, 23);
            this.labelConsignee.TabIndex = 15;
            this.labelConsignee.Text = "Consignee";
            // 
            // labelPortofLoading
            // 
            this.labelPortofLoading.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelPortofLoading.Lines = 0;
            this.labelPortofLoading.Location = new System.Drawing.Point(5, 118);
            this.labelPortofLoading.Name = "labelPortofLoading";
            this.labelPortofLoading.Size = new System.Drawing.Size(100, 23);
            this.labelPortofLoading.TabIndex = 16;
            this.labelPortofLoading.Text = "Port of Loading";
            // 
            // labelPortofDischarge
            // 
            this.labelPortofDischarge.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelPortofDischarge.Lines = 0;
            this.labelPortofDischarge.Location = new System.Drawing.Point(5, 146);
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
            this.displayFtyWKNo.Location = new System.Drawing.Point(75, 6);
            this.displayFtyWKNo.Name = "displayFtyWKNo";
            this.displayFtyWKNo.Size = new System.Drawing.Size(120, 23);
            this.displayFtyWKNo.TabIndex = 18;
            // 
            // txtInvoiceNo
            // 
            this.txtInvoiceNo.BackColor = System.Drawing.Color.White;
            this.txtInvoiceNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "INVNo", true));
            this.txtInvoiceNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvoiceNo.Location = new System.Drawing.Point(75, 62);
            this.txtInvoiceNo.Name = "txtInvoiceNo";
            this.txtInvoiceNo.Size = new System.Drawing.Size(230, 23);
            this.txtInvoiceNo.TabIndex = 1;
            // 
            // txtConsignee
            // 
            this.txtConsignee.BackColor = System.Drawing.Color.White;
            this.txtConsignee.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Consignee", true));
            this.txtConsignee.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtConsignee.Location = new System.Drawing.Point(75, 90);
            this.txtConsignee.Name = "txtConsignee";
            this.txtConsignee.Size = new System.Drawing.Size(75, 23);
            this.txtConsignee.TabIndex = 2;
            // 
            // txtPortofLoading
            // 
            this.txtPortofLoading.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtPortofLoading.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ExportPort", true));
            this.txtPortofLoading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtPortofLoading.IsSupportEditMode = false;
            this.txtPortofLoading.Location = new System.Drawing.Point(109, 118);
            this.txtPortofLoading.Name = "txtPortofLoading";
            this.txtPortofLoading.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtPortofLoading.ReadOnly = true;
            this.txtPortofLoading.Size = new System.Drawing.Size(185, 23);
            this.txtPortofLoading.TabIndex = 14;
            this.txtPortofLoading.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtPortofLoading_PopUp);
            // 
            // txtPortofDischarge
            // 
            this.txtPortofDischarge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtPortofDischarge.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ImportPort", true));
            this.txtPortofDischarge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtPortofDischarge.IsSupportEditMode = false;
            this.txtPortofDischarge.Location = new System.Drawing.Point(109, 146);
            this.txtPortofDischarge.Name = "txtPortofDischarge";
            this.txtPortofDischarge.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtPortofDischarge.ReadOnly = true;
            this.txtPortofDischarge.Size = new System.Drawing.Size(185, 23);
            this.txtPortofDischarge.TabIndex = 15;
            this.txtPortofDischarge.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtPortofDischarge_PopUp);
            // 
            // displayPortofLoading
            // 
            this.displayPortofLoading.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPortofLoading.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ExportCountry", true));
            this.displayPortofLoading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPortofLoading.Location = new System.Drawing.Point(296, 118);
            this.displayPortofLoading.Name = "displayPortofLoading";
            this.displayPortofLoading.Size = new System.Drawing.Size(32, 23);
            this.displayPortofLoading.TabIndex = 21;
            // 
            // displayPortofDischarge
            // 
            this.displayPortofDischarge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPortofDischarge.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ImportCountry", true));
            this.displayPortofDischarge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPortofDischarge.Location = new System.Drawing.Point(296, 146);
            this.displayPortofDischarge.Name = "displayPortofDischarge";
            this.displayPortofDischarge.Size = new System.Drawing.Size(32, 23);
            this.displayPortofDischarge.TabIndex = 22;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioLocalPurchase);
            this.radioPanel1.Controls.Add(this.radioTransferOut);
            this.radioPanel1.Controls.Add(this.radioTransferIn);
            this.radioPanel1.Controls.Add(this.radio3rdCountry);
            this.radioPanel1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Type", true));
            this.radioPanel1.Location = new System.Drawing.Point(199, 4);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(393, 25);
            this.radioPanel1.TabIndex = 0;
            this.radioPanel1.ValueChanged += new System.EventHandler(this.RadioPanel1_ValueChanged);
            // 
            // radioLocalPurchase
            // 
            this.radioLocalPurchase.AutoSize = true;
            this.radioLocalPurchase.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.radioLocalPurchase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioLocalPurchase.Location = new System.Drawing.Point(281, 2);
            this.radioLocalPurchase.Name = "radioLocalPurchase";
            this.radioLocalPurchase.Size = new System.Drawing.Size(110, 19);
            this.radioLocalPurchase.TabIndex = 3;
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
            this.radioTransferOut.Location = new System.Drawing.Point(183, 2);
            this.radioTransferOut.Name = "radioTransferOut";
            this.radioTransferOut.Size = new System.Drawing.Size(92, 19);
            this.radioTransferOut.TabIndex = 2;
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
            this.radioTransferIn.Location = new System.Drawing.Point(96, 2);
            this.radioTransferIn.Name = "radioTransferIn";
            this.radioTransferIn.Size = new System.Drawing.Size(83, 19);
            this.radioTransferIn.TabIndex = 1;
            this.radioTransferIn.TabStop = true;
            this.radioTransferIn.Text = "Transfer In";
            this.radioTransferIn.UseVisualStyleBackColor = true;
            this.radioTransferIn.Value = "2";
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
            this.labelShippingMode.Lines = 0;
            this.labelShippingMode.Location = new System.Drawing.Point(352, 33);
            this.labelShippingMode.Name = "labelShippingMode";
            this.labelShippingMode.Size = new System.Drawing.Size(90, 23);
            this.labelShippingMode.TabIndex = 16;
            this.labelShippingMode.Text = "Shipping Mode";
            // 
            // labelContainerType
            // 
            this.labelContainerType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelContainerType.Lines = 0;
            this.labelContainerType.Location = new System.Drawing.Point(352, 62);
            this.labelContainerType.Name = "labelContainerType";
            this.labelContainerType.Size = new System.Drawing.Size(90, 23);
            this.labelContainerType.TabIndex = 17;
            this.labelContainerType.Text = "Container Type";
            // 
            // labelPackages
            // 
            this.labelPackages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelPackages.Lines = 0;
            this.labelPackages.Location = new System.Drawing.Point(352, 90);
            this.labelPackages.Name = "labelPackages";
            this.labelPackages.Size = new System.Drawing.Size(90, 23);
            this.labelPackages.TabIndex = 18;
            this.labelPackages.Text = "Packages";
            // 
            // labelCBM
            // 
            this.labelCBM.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelCBM.Lines = 0;
            this.labelCBM.Location = new System.Drawing.Point(352, 118);
            this.labelCBM.Name = "labelCBM";
            this.labelCBM.Size = new System.Drawing.Size(90, 23);
            this.labelCBM.TabIndex = 19;
            this.labelCBM.Text = "CBM";
            // 
            // labelNWGW
            // 
            this.labelNWGW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelNWGW.Lines = 0;
            this.labelNWGW.Location = new System.Drawing.Point(352, 146);
            this.labelNWGW.Name = "labelNWGW";
            this.labelNWGW.Size = new System.Drawing.Size(90, 23);
            this.labelNWGW.TabIndex = 20;
            this.labelNWGW.Text = "N.W./G.W.";
            // 
            // numPackages
            // 
            this.numPackages.BackColor = System.Drawing.Color.White;
            this.numPackages.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Packages", true));
            this.numPackages.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numPackages.Location = new System.Drawing.Point(445, 90);
            this.numPackages.Name = "numPackages";
            this.numPackages.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPackages.Size = new System.Drawing.Size(67, 23);
            this.numPackages.TabIndex = 5;
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
            this.numCBM.Location = new System.Drawing.Point(445, 118);
            this.numCBM.Name = "numCBM";
            this.numCBM.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCBM.Size = new System.Drawing.Size(67, 23);
            this.numCBM.TabIndex = 6;
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
            this.numNetKg.Location = new System.Drawing.Point(444, 146);
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
            this.label15.Lines = 0;
            this.label15.Location = new System.Drawing.Point(510, 146);
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
            this.numWeightKg.Location = new System.Drawing.Point(521, 146);
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
            this.labelHandle.Lines = 0;
            this.labelHandle.Location = new System.Drawing.Point(615, 6);
            this.labelHandle.Name = "labelHandle";
            this.labelHandle.Size = new System.Drawing.Size(46, 23);
            this.labelHandle.TabIndex = 31;
            this.labelHandle.Text = "Handle";
            // 
            // labelBLAWBNo
            // 
            this.labelBLAWBNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelBLAWBNo.Lines = 0;
            this.labelBLAWBNo.Location = new System.Drawing.Point(615, 34);
            this.labelBLAWBNo.Name = "labelBLAWBNo";
            this.labelBLAWBNo.Size = new System.Drawing.Size(93, 23);
            this.labelBLAWBNo.TabIndex = 32;
            this.labelBLAWBNo.Text = "B/L(AWB) No.";
            // 
            // labelVslvoyFltNo
            // 
            this.labelVslvoyFltNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelVslvoyFltNo.Lines = 0;
            this.labelVslvoyFltNo.Location = new System.Drawing.Point(615, 62);
            this.labelVslvoyFltNo.Name = "labelVslvoyFltNo";
            this.labelVslvoyFltNo.Size = new System.Drawing.Size(93, 23);
            this.labelVslvoyFltNo.TabIndex = 33;
            this.labelVslvoyFltNo.Text = "Vsl voy/Flt No.";
            // 
            // labelArrivePortDate
            // 
            this.labelArrivePortDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelArrivePortDate.Lines = 0;
            this.labelArrivePortDate.Location = new System.Drawing.Point(615, 90);
            this.labelArrivePortDate.Name = "labelArrivePortDate";
            this.labelArrivePortDate.Size = new System.Drawing.Size(93, 23);
            this.labelArrivePortDate.TabIndex = 34;
            this.labelArrivePortDate.Text = "Arrive Port Date";
            // 
            // labelArriveWHDate
            // 
            this.labelArriveWHDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelArriveWHDate.Lines = 0;
            this.labelArriveWHDate.Location = new System.Drawing.Point(615, 118);
            this.labelArriveWHDate.Name = "labelArriveWHDate";
            this.labelArriveWHDate.Size = new System.Drawing.Size(93, 23);
            this.labelArriveWHDate.TabIndex = 35;
            this.labelArriveWHDate.Text = "Arrive W/H Date";
            // 
            // labelDoxRcvDate
            // 
            this.labelDoxRcvDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelDoxRcvDate.Lines = 0;
            this.labelDoxRcvDate.Location = new System.Drawing.Point(615, 146);
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
            this.txtBLAWBNo.Location = new System.Drawing.Point(712, 34);
            this.txtBLAWBNo.Name = "txtBLAWBNo";
            this.txtBLAWBNo.Size = new System.Drawing.Size(185, 23);
            this.txtBLAWBNo.TabIndex = 8;
            // 
            // txtVslvoyFltNo
            // 
            this.txtVslvoyFltNo.BackColor = System.Drawing.Color.White;
            this.txtVslvoyFltNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Vessel", true));
            this.txtVslvoyFltNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtVslvoyFltNo.Location = new System.Drawing.Point(712, 62);
            this.txtVslvoyFltNo.Name = "txtVslvoyFltNo";
            this.txtVslvoyFltNo.Size = new System.Drawing.Size(254, 23);
            this.txtVslvoyFltNo.TabIndex = 9;
            // 
            // dateArrivePortDate
            // 
            this.dateArrivePortDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PortArrival", true));
            this.dateArrivePortDate.Location = new System.Drawing.Point(712, 90);
            this.dateArrivePortDate.Name = "dateArrivePortDate";
            this.dateArrivePortDate.Size = new System.Drawing.Size(130, 23);
            this.dateArrivePortDate.TabIndex = 10;
            // 
            // dateArriveWHDate
            // 
            this.dateArriveWHDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "WhseArrival", true));
            this.dateArriveWHDate.IsSupportEditMode = false;
            this.dateArriveWHDate.Location = new System.Drawing.Point(712, 118);
            this.dateArriveWHDate.Name = "dateArriveWHDate";
            this.dateArriveWHDate.ReadOnly = true;
            this.dateArriveWHDate.Size = new System.Drawing.Size(130, 23);
            this.dateArriveWHDate.TabIndex = 15;
            // 
            // dateDoxRcvDate
            // 
            this.dateDoxRcvDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "DocArrival", true));
            this.dateDoxRcvDate.Location = new System.Drawing.Point(712, 146);
            this.dateDoxRcvDate.Name = "dateDoxRcvDate";
            this.dateDoxRcvDate.Size = new System.Drawing.Size(130, 23);
            this.dateDoxRcvDate.TabIndex = 11;
            // 
            // btnExpenseData
            // 
            this.btnExpenseData.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnExpenseData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnExpenseData.Location = new System.Drawing.Point(875, 92);
            this.btnExpenseData.Name = "btnExpenseData";
            this.btnExpenseData.Size = new System.Drawing.Size(106, 30);
            this.btnExpenseData.TabIndex = 43;
            this.btnExpenseData.Text = "Expense Data";
            this.btnExpenseData.UseVisualStyleBackColor = true;
            this.btnExpenseData.Click += new System.EventHandler(this.BtnExpenseData_Click);
            // 
            // btnImportData
            // 
            this.btnImportData.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImportData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnImportData.Location = new System.Drawing.Point(861, 141);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(93, 30);
            this.btnImportData.TabIndex = 12;
            this.btnImportData.Text = "Import Data";
            this.btnImportData.UseVisualStyleBackColor = true;
            this.btnImportData.Click += new System.EventHandler(this.BtnImportData_Click);
            // 
            // txtUserHandle
            // 
            this.txtUserHandle.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Handle", true));
            this.txtUserHandle.DisplayBox1Binding = "";
            this.txtUserHandle.Location = new System.Drawing.Point(664, 6);
            this.txtUserHandle.Name = "txtUserHandle";
            this.txtUserHandle.Size = new System.Drawing.Size(302, 23);
            this.txtUserHandle.TabIndex = 7;
            this.txtUserHandle.TextBox1Binding = "";
            // 
            // txtdropdownlistContainerType
            // 
            this.txtdropdownlistContainerType.BackColor = System.Drawing.Color.White;
            this.txtdropdownlistContainerType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "CYCFS", true));
            this.txtdropdownlistContainerType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlistContainerType.FormattingEnabled = true;
            this.txtdropdownlistContainerType.IsSupportUnselect = true;
            this.txtdropdownlistContainerType.Location = new System.Drawing.Point(445, 60);
            this.txtdropdownlistContainerType.Name = "txtdropdownlistContainerType";
            this.txtdropdownlistContainerType.Size = new System.Drawing.Size(121, 24);
            this.txtdropdownlistContainerType.TabIndex = 4;
            this.txtdropdownlistContainerType.Type = "ContainerCY";
            // 
            // txtShipmodeShippingMode
            // 
            this.txtShipmodeShippingMode.BackColor = System.Drawing.Color.White;
            this.txtShipmodeShippingMode.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "ShipModeID", true));
            this.txtShipmodeShippingMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShipmodeShippingMode.FormattingEnabled = true;
            this.txtShipmodeShippingMode.IsSupportUnselect = true;
            this.txtShipmodeShippingMode.Location = new System.Drawing.Point(445, 31);
            this.txtShipmodeShippingMode.Name = "txtShipmodeShippingMode";
            this.txtShipmodeShippingMode.Size = new System.Drawing.Size(80, 24);
            this.txtShipmodeShippingMode.TabIndex = 3;
            this.txtShipmodeShippingMode.UseFunction = "WK";
            // 
            // txtSubconForwarder
            // 
            this.txtSubconForwarder.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Forwarder", true));
            this.txtSubconForwarder.DisplayBox1Binding = "";
            this.txtSubconForwarder.IsIncludeJunk = false;
            this.txtSubconForwarder.Location = new System.Drawing.Point(75, 34);
            this.txtSubconForwarder.Name = "txtSubconForwarder";
            this.txtSubconForwarder.Size = new System.Drawing.Size(170, 23);
            this.txtSubconForwarder.TabIndex = 0;
            this.txtSubconForwarder.TextBox1Binding = "";
            // 
            // P04
            // 
            this.ClientSize = new System.Drawing.Size(998, 556);
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
            this.tabs.ResumeLayout(false);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.NumericBox numCBM;
        private Win.UI.NumericBox numPackages;
        private Class.txtdropdownlist txtdropdownlistContainerType;
        private Class.txtshipmode txtShipmodeShippingMode;
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
        private Win.UI.TextBox txtConsignee;
        private Win.UI.TextBox txtInvoiceNo;
        private Class.txtsubcon txtSubconForwarder;
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
        private Class.txtuser txtUserHandle;
        private Win.UI.Button btnImportData;
        private Win.UI.Button btnExpenseData;
    }
}
