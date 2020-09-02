namespace Sci.Production.Warehouse
{
    partial class P02
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
            this.labelWKNo = new Sci.Win.UI.Label();
            this.labelETA = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelConsignee = new Sci.Win.UI.Label();
            this.labelPackages = new Sci.Win.UI.Label();
            this.labelContainerType = new Sci.Win.UI.Label();
            this.labelShipModeID = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.displayWKNo = new Sci.Win.UI.DisplayBox();
            this.dateETA = new Sci.Win.UI.DateBox();
            this.displayFactory = new Sci.Win.UI.DisplayBox();
            this.displayConsignee = new Sci.Win.UI.DisplayBox();
            this.numPackages = new Sci.Win.UI.NumericBox();
            this.displayContainerType = new Sci.Win.UI.DisplayBox();
            this.displayShipModeID = new Sci.Win.UI.DisplayBox();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.labelInvoiceNo = new Sci.Win.UI.Label();
            this.labelPayer = new Sci.Win.UI.Label();
            this.labelBLNo = new Sci.Win.UI.Label();
            this.labelVesselName = new Sci.Win.UI.Label();
            this.labelNWGW = new Sci.Win.UI.Label();
            this.labelCBM = new Sci.Win.UI.Label();
            this.displayInvoiceNo = new Sci.Win.UI.DisplayBox();
            this.displayPayer = new Sci.Win.UI.DisplayBox();
            this.displayBLNo = new Sci.Win.UI.DisplayBox();
            this.displayVesselName = new Sci.Win.UI.DisplayBox();
            this.numNetKg = new Sci.Win.UI.NumericBox();
            this.label15 = new Sci.Win.UI.Label();
            this.numWeightKg = new Sci.Win.UI.NumericBox();
            this.numCBM = new Sci.Win.UI.NumericBox();
            this.labelHandle = new Sci.Win.UI.Label();
            this.labelPLRcvDate = new Sci.Win.UI.Label();
            this.labelArrivePortDate = new Sci.Win.UI.Label();
            this.labelArriveWHDate = new Sci.Win.UI.Label();
            this.labelDoxRcvDate = new Sci.Win.UI.Label();
            this.datePLRcvDate = new Sci.Win.UI.DateBox();
            this.dateArrivePortDate = new Sci.Win.UI.DateBox();
            this.dateArriveWHDate = new Sci.Win.UI.DateBox();
            this.dateDoxRcvDate = new Sci.Win.UI.DateBox();
            this.label21 = new Sci.Win.UI.Label();
            this.btnShippingMark = new Sci.Win.UI.Button();
            this.labelLocateSP = new Sci.Win.UI.Label();
            this.txtLocateSP = new Sci.Win.UI.TextBox();
            this.label23 = new Sci.Win.UI.Label();
            this.btnFind = new Sci.Win.UI.Button();
            this.txttpeuserHandle = new Sci.Production.Class.Txttpeuser();
            this.txtSeq1 = new Sci.Production.Class.TxtSeq();
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
            this.masterpanel.Controls.Add(this.txtSeq1);
            this.masterpanel.Controls.Add(this.btnFind);
            this.masterpanel.Controls.Add(this.label23);
            this.masterpanel.Controls.Add(this.txtLocateSP);
            this.masterpanel.Controls.Add(this.labelLocateSP);
            this.masterpanel.Controls.Add(this.btnShippingMark);
            this.masterpanel.Controls.Add(this.label21);
            this.masterpanel.Controls.Add(this.txttpeuserHandle);
            this.masterpanel.Controls.Add(this.labelDoxRcvDate);
            this.masterpanel.Controls.Add(this.labelArriveWHDate);
            this.masterpanel.Controls.Add(this.labelArrivePortDate);
            this.masterpanel.Controls.Add(this.labelPLRcvDate);
            this.masterpanel.Controls.Add(this.labelHandle);
            this.masterpanel.Controls.Add(this.numCBM);
            this.masterpanel.Controls.Add(this.numWeightKg);
            this.masterpanel.Controls.Add(this.label15);
            this.masterpanel.Controls.Add(this.numNetKg);
            this.masterpanel.Controls.Add(this.displayVesselName);
            this.masterpanel.Controls.Add(this.displayBLNo);
            this.masterpanel.Controls.Add(this.displayPayer);
            this.masterpanel.Controls.Add(this.displayInvoiceNo);
            this.masterpanel.Controls.Add(this.labelCBM);
            this.masterpanel.Controls.Add(this.labelNWGW);
            this.masterpanel.Controls.Add(this.labelVesselName);
            this.masterpanel.Controls.Add(this.labelBLNo);
            this.masterpanel.Controls.Add(this.labelPayer);
            this.masterpanel.Controls.Add(this.labelInvoiceNo);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.displayShipModeID);
            this.masterpanel.Controls.Add(this.displayContainerType);
            this.masterpanel.Controls.Add(this.numPackages);
            this.masterpanel.Controls.Add(this.displayConsignee);
            this.masterpanel.Controls.Add(this.displayFactory);
            this.masterpanel.Controls.Add(this.displayWKNo);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.labelShipModeID);
            this.masterpanel.Controls.Add(this.labelContainerType);
            this.masterpanel.Controls.Add(this.labelPackages);
            this.masterpanel.Controls.Add(this.labelConsignee);
            this.masterpanel.Controls.Add(this.labelFactory);
            this.masterpanel.Controls.Add(this.labelETA);
            this.masterpanel.Controls.Add(this.labelWKNo);
            this.masterpanel.Controls.Add(this.dateDoxRcvDate);
            this.masterpanel.Controls.Add(this.dateArriveWHDate);
            this.masterpanel.Controls.Add(this.dateArrivePortDate);
            this.masterpanel.Controls.Add(this.datePLRcvDate);
            this.masterpanel.Controls.Add(this.dateETA);
            this.masterpanel.Size = new System.Drawing.Size(990, 285);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateETA, 0);
            this.masterpanel.Controls.SetChildIndex(this.datePLRcvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateArrivePortDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateArriveWHDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateDoxRcvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelWKNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelETA, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelConsignee, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPackages, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelContainerType, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShipModeID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayWKNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayConsignee, 0);
            this.masterpanel.Controls.SetChildIndex(this.numPackages, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayContainerType, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayShipModeID, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelInvoiceNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPayer, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelBLNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelVesselName, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNWGW, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCBM, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayInvoiceNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayPayer, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBLNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayVesselName, 0);
            this.masterpanel.Controls.SetChildIndex(this.numNetKg, 0);
            this.masterpanel.Controls.SetChildIndex(this.label15, 0);
            this.masterpanel.Controls.SetChildIndex(this.numWeightKg, 0);
            this.masterpanel.Controls.SetChildIndex(this.numCBM, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPLRcvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelArrivePortDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelArriveWHDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDoxRcvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txttpeuserHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.label21, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnShippingMark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelLocateSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLocateSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.label23, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnFind, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSeq1, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 285);
            this.detailpanel.Size = new System.Drawing.Size(990, 277);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(882, 213);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(910, 0);
            this.refresh.TabIndex = 0;
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(990, 277);
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
            this.editby.Location = new System.Drawing.Point(492, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(444, 13);
            // 
            // labelWKNo
            // 
            this.labelWKNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelWKNo.Location = new System.Drawing.Point(5, 6);
            this.labelWKNo.Name = "labelWKNo";
            this.labelWKNo.Size = new System.Drawing.Size(89, 23);
            this.labelWKNo.TabIndex = 1;
            this.labelWKNo.Text = "WK No.";
            // 
            // labelETA
            // 
            this.labelETA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelETA.Location = new System.Drawing.Point(5, 33);
            this.labelETA.Name = "labelETA";
            this.labelETA.Size = new System.Drawing.Size(89, 23);
            this.labelETA.TabIndex = 2;
            this.labelETA.Text = "ETA";
            // 
            // labelFactory
            // 
            this.labelFactory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelFactory.Location = new System.Drawing.Point(4, 60);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(90, 23);
            this.labelFactory.TabIndex = 3;
            this.labelFactory.Text = "Factory";
            // 
            // labelConsignee
            // 
            this.labelConsignee.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelConsignee.Location = new System.Drawing.Point(4, 87);
            this.labelConsignee.Name = "labelConsignee";
            this.labelConsignee.Size = new System.Drawing.Size(90, 23);
            this.labelConsignee.TabIndex = 4;
            this.labelConsignee.Text = "Consignee";
            // 
            // labelPackages
            // 
            this.labelPackages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelPackages.Location = new System.Drawing.Point(4, 114);
            this.labelPackages.Name = "labelPackages";
            this.labelPackages.Size = new System.Drawing.Size(90, 23);
            this.labelPackages.TabIndex = 5;
            this.labelPackages.Text = "Packages";
            // 
            // labelContainerType
            // 
            this.labelContainerType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelContainerType.Location = new System.Drawing.Point(4, 141);
            this.labelContainerType.Name = "labelContainerType";
            this.labelContainerType.Size = new System.Drawing.Size(90, 23);
            this.labelContainerType.TabIndex = 6;
            this.labelContainerType.Text = "CY/CFS";
            // 
            // labelShipModeID
            // 
            this.labelShipModeID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelShipModeID.Location = new System.Drawing.Point(4, 168);
            this.labelShipModeID.Name = "labelShipModeID";
            this.labelShipModeID.Size = new System.Drawing.Size(90, 23);
            this.labelShipModeID.TabIndex = 7;
            this.labelShipModeID.Text = "ShipModeID";
            // 
            // labelRemark
            // 
            this.labelRemark.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelRemark.Location = new System.Drawing.Point(4, 195);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(90, 23);
            this.labelRemark.TabIndex = 8;
            this.labelRemark.Text = "Remark";
            // 
            // displayWKNo
            // 
            this.displayWKNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayWKNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayWKNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayWKNo.Location = new System.Drawing.Point(98, 6);
            this.displayWKNo.Name = "displayWKNo";
            this.displayWKNo.Size = new System.Drawing.Size(120, 23);
            this.displayWKNo.TabIndex = 9;
            // 
            // dateETA
            // 
            this.dateETA.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Eta", true));
            this.dateETA.IsSupportEditMode = false;
            this.dateETA.Location = new System.Drawing.Point(98, 33);
            this.dateETA.Name = "dateETA";
            this.dateETA.ReadOnly = true;
            this.dateETA.Size = new System.Drawing.Size(130, 23);
            this.dateETA.TabIndex = 10;
            // 
            // displayFactory
            // 
            this.displayFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFactory.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FactoryID", true));
            this.displayFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFactory.Location = new System.Drawing.Point(98, 60);
            this.displayFactory.Name = "displayFactory";
            this.displayFactory.Size = new System.Drawing.Size(75, 23);
            this.displayFactory.TabIndex = 11;
            // 
            // displayConsignee
            // 
            this.displayConsignee.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayConsignee.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Consignee", true));
            this.displayConsignee.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayConsignee.Location = new System.Drawing.Point(98, 87);
            this.displayConsignee.Name = "displayConsignee";
            this.displayConsignee.Size = new System.Drawing.Size(75, 23);
            this.displayConsignee.TabIndex = 12;
            // 
            // numPackages
            // 
            this.numPackages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numPackages.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Packages", true));
            this.numPackages.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numPackages.IsSupportEditMode = false;
            this.numPackages.Location = new System.Drawing.Point(98, 114);
            this.numPackages.Name = "numPackages";
            this.numPackages.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPackages.ReadOnly = true;
            this.numPackages.Size = new System.Drawing.Size(50, 23);
            this.numPackages.TabIndex = 13;
            this.numPackages.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // displayContainerType
            // 
            this.displayContainerType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayContainerType.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MainCYCFS", true));
            this.displayContainerType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayContainerType.Location = new System.Drawing.Point(98, 141);
            this.displayContainerType.Name = "displayContainerType";
            this.displayContainerType.Size = new System.Drawing.Size(30, 23);
            this.displayContainerType.TabIndex = 14;
            // 
            // displayShipModeID
            // 
            this.displayShipModeID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayShipModeID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ShipModeID", true));
            this.displayShipModeID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayShipModeID.Location = new System.Drawing.Point(98, 168);
            this.displayShipModeID.Name = "displayShipModeID";
            this.displayShipModeID.Size = new System.Drawing.Size(120, 23);
            this.displayShipModeID.TabIndex = 15;
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editRemark.IsSupportEditMode = false;
            this.editRemark.Location = new System.Drawing.Point(98, 195);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.ReadOnly = true;
            this.editRemark.Size = new System.Drawing.Size(444, 50);
            this.editRemark.TabIndex = 16;
            // 
            // labelInvoiceNo
            // 
            this.labelInvoiceNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelInvoiceNo.Location = new System.Drawing.Point(250, 33);
            this.labelInvoiceNo.Name = "labelInvoiceNo";
            this.labelInvoiceNo.Size = new System.Drawing.Size(79, 23);
            this.labelInvoiceNo.TabIndex = 17;
            this.labelInvoiceNo.Text = "Invoice#";
            // 
            // labelPayer
            // 
            this.labelPayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelPayer.Location = new System.Drawing.Point(250, 60);
            this.labelPayer.Name = "labelPayer";
            this.labelPayer.Size = new System.Drawing.Size(79, 23);
            this.labelPayer.TabIndex = 18;
            this.labelPayer.Text = "Payer";
            // 
            // labelBLNo
            // 
            this.labelBLNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelBLNo.Location = new System.Drawing.Point(250, 87);
            this.labelBLNo.Name = "labelBLNo";
            this.labelBLNo.Size = new System.Drawing.Size(79, 23);
            this.labelBLNo.TabIndex = 19;
            this.labelBLNo.Text = "B/L No.";
            // 
            // labelVesselName
            // 
            this.labelVesselName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelVesselName.Location = new System.Drawing.Point(250, 114);
            this.labelVesselName.Name = "labelVesselName";
            this.labelVesselName.Size = new System.Drawing.Size(79, 23);
            this.labelVesselName.TabIndex = 20;
            this.labelVesselName.Text = "Vessel Name";
            // 
            // labelNWGW
            // 
            this.labelNWGW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelNWGW.Location = new System.Drawing.Point(250, 141);
            this.labelNWGW.Name = "labelNWGW";
            this.labelNWGW.Size = new System.Drawing.Size(79, 23);
            this.labelNWGW.TabIndex = 21;
            this.labelNWGW.Text = "N.W./G.W.";
            // 
            // labelCBM
            // 
            this.labelCBM.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelCBM.Location = new System.Drawing.Point(250, 168);
            this.labelCBM.Name = "labelCBM";
            this.labelCBM.Size = new System.Drawing.Size(79, 23);
            this.labelCBM.TabIndex = 22;
            this.labelCBM.Text = "CBM";
            // 
            // displayInvoiceNo
            // 
            this.displayInvoiceNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayInvoiceNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "InvNo", true));
            this.displayInvoiceNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayInvoiceNo.Location = new System.Drawing.Point(332, 33);
            this.displayInvoiceNo.Name = "displayInvoiceNo";
            this.displayInvoiceNo.Size = new System.Drawing.Size(210, 23);
            this.displayInvoiceNo.TabIndex = 23;
            // 
            // displayPayer
            // 
            this.displayPayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPayer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPayer.Location = new System.Drawing.Point(332, 60);
            this.displayPayer.Name = "displayPayer";
            this.displayPayer.Size = new System.Drawing.Size(210, 23);
            this.displayPayer.TabIndex = 24;
            // 
            // displayBLNo
            // 
            this.displayBLNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBLNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Blno", true));
            this.displayBLNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBLNo.Location = new System.Drawing.Point(332, 87);
            this.displayBLNo.Name = "displayBLNo";
            this.displayBLNo.Size = new System.Drawing.Size(175, 23);
            this.displayBLNo.TabIndex = 25;
            // 
            // displayVesselName
            // 
            this.displayVesselName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayVesselName.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Vessel", true));
            this.displayVesselName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayVesselName.Location = new System.Drawing.Point(332, 114);
            this.displayVesselName.Name = "displayVesselName";
            this.displayVesselName.Size = new System.Drawing.Size(226, 23);
            this.displayVesselName.TabIndex = 26;
            // 
            // numNetKg
            // 
            this.numNetKg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numNetKg.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NetKg", true));
            this.numNetKg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numNetKg.IsSupportEditMode = false;
            this.numNetKg.Location = new System.Drawing.Point(332, 141);
            this.numNetKg.Name = "numNetKg";
            this.numNetKg.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numNetKg.ReadOnly = true;
            this.numNetKg.Size = new System.Drawing.Size(65, 23);
            this.numNetKg.TabIndex = 27;
            this.numNetKg.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Location = new System.Drawing.Point(398, 141);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(10, 23);
            this.label15.TabIndex = 28;
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
            this.numWeightKg.Location = new System.Drawing.Point(407, 141);
            this.numWeightKg.Name = "numWeightKg";
            this.numWeightKg.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWeightKg.ReadOnly = true;
            this.numWeightKg.Size = new System.Drawing.Size(65, 23);
            this.numWeightKg.TabIndex = 29;
            this.numWeightKg.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numCBM
            // 
            this.numCBM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numCBM.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Cbm", true));
            this.numCBM.DecimalPlaces = 3;
            this.numCBM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numCBM.IsSupportEditMode = false;
            this.numCBM.Location = new System.Drawing.Point(332, 168);
            this.numCBM.Name = "numCBM";
            this.numCBM.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCBM.ReadOnly = true;
            this.numCBM.Size = new System.Drawing.Size(90, 23);
            this.numCBM.TabIndex = 30;
            this.numCBM.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelHandle
            // 
            this.labelHandle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelHandle.Location = new System.Drawing.Point(577, 33);
            this.labelHandle.Name = "labelHandle";
            this.labelHandle.Size = new System.Drawing.Size(92, 23);
            this.labelHandle.TabIndex = 31;
            this.labelHandle.Text = "Handle";
            // 
            // labelPLRcvDate
            // 
            this.labelPLRcvDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelPLRcvDate.Location = new System.Drawing.Point(577, 60);
            this.labelPLRcvDate.Name = "labelPLRcvDate";
            this.labelPLRcvDate.Size = new System.Drawing.Size(92, 23);
            this.labelPLRcvDate.TabIndex = 32;
            this.labelPLRcvDate.Text = "P/L Rcv Date";
            // 
            // labelArrivePortDate
            // 
            this.labelArrivePortDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelArrivePortDate.Location = new System.Drawing.Point(577, 88);
            this.labelArrivePortDate.Name = "labelArrivePortDate";
            this.labelArrivePortDate.Size = new System.Drawing.Size(92, 23);
            this.labelArrivePortDate.TabIndex = 33;
            this.labelArrivePortDate.Text = "Arrive Port Date";
            // 
            // labelArriveWHDate
            // 
            this.labelArriveWHDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelArriveWHDate.Location = new System.Drawing.Point(577, 115);
            this.labelArriveWHDate.Name = "labelArriveWHDate";
            this.labelArriveWHDate.Size = new System.Drawing.Size(92, 23);
            this.labelArriveWHDate.TabIndex = 34;
            this.labelArriveWHDate.Text = "Arrive W/H Date";
            // 
            // labelDoxRcvDate
            // 
            this.labelDoxRcvDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelDoxRcvDate.Location = new System.Drawing.Point(577, 142);
            this.labelDoxRcvDate.Name = "labelDoxRcvDate";
            this.labelDoxRcvDate.Size = new System.Drawing.Size(92, 23);
            this.labelDoxRcvDate.TabIndex = 35;
            this.labelDoxRcvDate.Text = "Dox Rcv Date";
            // 
            // datePLRcvDate
            // 
            this.datePLRcvDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PackingArrival", true));
            this.datePLRcvDate.Location = new System.Drawing.Point(673, 61);
            this.datePLRcvDate.Name = "datePLRcvDate";
            this.datePLRcvDate.Size = new System.Drawing.Size(130, 23);
            this.datePLRcvDate.TabIndex = 37;
            // 
            // dateArrivePortDate
            // 
            this.dateArrivePortDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PortArrival", true));
            this.dateArrivePortDate.IsSupportEditMode = false;
            this.dateArrivePortDate.Location = new System.Drawing.Point(673, 88);
            this.dateArrivePortDate.Name = "dateArrivePortDate";
            this.dateArrivePortDate.ReadOnly = true;
            this.dateArrivePortDate.Size = new System.Drawing.Size(130, 23);
            this.dateArrivePortDate.TabIndex = 38;
            // 
            // dateArriveWHDate
            // 
            this.dateArriveWHDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "WhseArrival", true));
            this.dateArriveWHDate.Location = new System.Drawing.Point(673, 115);
            this.dateArriveWHDate.Name = "dateArriveWHDate";
            this.dateArriveWHDate.Size = new System.Drawing.Size(130, 23);
            this.dateArriveWHDate.TabIndex = 39;
            // 
            // dateDoxRcvDate
            // 
            this.dateDoxRcvDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "DocArrival", true));
            this.dateDoxRcvDate.IsSupportEditMode = false;
            this.dateDoxRcvDate.Location = new System.Drawing.Point(673, 142);
            this.dateDoxRcvDate.Name = "dateDoxRcvDate";
            this.dateDoxRcvDate.ReadOnly = true;
            this.dateDoxRcvDate.Size = new System.Drawing.Size(130, 23);
            this.dateDoxRcvDate.TabIndex = 40;
            // 
            // label21
            // 
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label21.Location = new System.Drawing.Point(250, 6);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(75, 23);
            this.label21.TabIndex = 41;
            this.label21.Text = "Junk";
            this.label21.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.label21.TextStyle.Color = System.Drawing.Color.Red;
            this.label21.TextStyle.ExtBorderColor = System.Drawing.Color.Red;
            this.label21.TextStyle.GradientColor = System.Drawing.Color.Red;
            // 
            // btnShippingMark
            // 
            this.btnShippingMark.Location = new System.Drawing.Point(859, 97);
            this.btnShippingMark.Name = "btnShippingMark";
            this.btnShippingMark.Size = new System.Drawing.Size(123, 30);
            this.btnShippingMark.TabIndex = 3;
            this.btnShippingMark.Text = "Shipping Mark";
            this.btnShippingMark.UseVisualStyleBackColor = true;
            this.btnShippingMark.Click += new System.EventHandler(this.BtnShippingMark_Click);
            // 
            // labelLocateSP
            // 
            this.labelLocateSP.Location = new System.Drawing.Point(5, 252);
            this.labelLocateSP.Name = "labelLocateSP";
            this.labelLocateSP.Size = new System.Drawing.Size(89, 23);
            this.labelLocateSP.TabIndex = 44;
            this.labelLocateSP.Text = "Locate SP#";
            // 
            // txtLocateSP
            // 
            this.txtLocateSP.BackColor = System.Drawing.Color.White;
            this.txtLocateSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateSP.Location = new System.Drawing.Point(98, 252);
            this.txtLocateSP.Name = "txtLocateSP";
            this.txtLocateSP.Size = new System.Drawing.Size(100, 23);
            this.txtLocateSP.TabIndex = 0;
            // 
            // label23
            // 
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Location = new System.Drawing.Point(200, 252);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(10, 23);
            this.label23.TabIndex = 46;
            this.label23.Text = "-";
            this.label23.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label23.TextStyle.Color = System.Drawing.Color.Black;
            this.label23.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label23.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(280, 248);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(58, 30);
            this.btnFind.TabIndex = 2;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // txttpeuserHandle
            // 
            this.txttpeuserHandle.DataBindings.Add(new System.Windows.Forms.Binding("DisplayBox1Binding", this.mtbs, "Handle", true));
            this.txttpeuserHandle.DisplayBox1Binding = "";
            this.txttpeuserHandle.DisplayBox2Binding = "";
            this.txttpeuserHandle.Location = new System.Drawing.Point(672, 33);
            this.txttpeuserHandle.Name = "txttpeuserHandle";
            this.txttpeuserHandle.Size = new System.Drawing.Size(302, 23);
            this.txttpeuserHandle.TabIndex = 36;
            // 
            // txtSeq1
            // 
            this.txtSeq1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq1.Location = new System.Drawing.Point(213, 252);
            this.txtSeq1.Name = "txtSeq1";
            this.txtSeq1.Seq1 = "";
            this.txtSeq1.Seq2 = "";
            this.txtSeq1.Size = new System.Drawing.Size(61, 23);
            this.txtSeq1.TabIndex = 1;
            // 
            // P02
            // 
            this.ClientSize = new System.Drawing.Size(998, 662);
            this.DefaultDetailOrder = "PoID,Seq1,Seq2";
            this.DefaultOrder = "ID";
            this.GridAlias = "Export_Detail";
            this.GridEdit = false;
            this.GridPopUp = false;
            this.GridUniqueKey = "Ukey";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.KeyField1 = "ID";
            this.Name = "P02";
            this.OnLineHelpID = "Sci.Win.Tems.Input2";
            this.Text = "P02. Import schedule";
            this.UniqueExpress = "ID";
            this.WorkAlias = "Export";
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

        private Win.UI.Button btnFind;
        private Win.UI.Label label23;
        private Win.UI.TextBox txtLocateSP;
        private Win.UI.Label labelLocateSP;
        private Win.UI.Button btnShippingMark;
        private Win.UI.Label label21;
        private Win.UI.DateBox dateDoxRcvDate;
        private Win.UI.DateBox dateArriveWHDate;
        private Win.UI.DateBox dateArrivePortDate;
        private Win.UI.DateBox datePLRcvDate;
        private Class.Txttpeuser txttpeuserHandle;
        private Win.UI.Label labelDoxRcvDate;
        private Win.UI.Label labelArriveWHDate;
        private Win.UI.Label labelArrivePortDate;
        private Win.UI.Label labelPLRcvDate;
        private Win.UI.Label labelHandle;
        private Win.UI.NumericBox numCBM;
        private Win.UI.NumericBox numWeightKg;
        private Win.UI.Label label15;
        private Win.UI.NumericBox numNetKg;
        private Win.UI.DisplayBox displayVesselName;
        private Win.UI.DisplayBox displayBLNo;
        private Win.UI.DisplayBox displayPayer;
        private Win.UI.DisplayBox displayInvoiceNo;
        private Win.UI.Label labelCBM;
        private Win.UI.Label labelNWGW;
        private Win.UI.Label labelVesselName;
        private Win.UI.Label labelBLNo;
        private Win.UI.Label labelPayer;
        private Win.UI.Label labelInvoiceNo;
        private Win.UI.EditBox editRemark;
        private Win.UI.DisplayBox displayShipModeID;
        private Win.UI.DisplayBox displayContainerType;
        private Win.UI.NumericBox numPackages;
        private Win.UI.DisplayBox displayConsignee;
        private Win.UI.DisplayBox displayFactory;
        private Win.UI.DateBox dateETA;
        private Win.UI.DisplayBox displayWKNo;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelShipModeID;
        private Win.UI.Label labelContainerType;
        private Win.UI.Label labelPackages;
        private Win.UI.Label labelConsignee;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelETA;
        private Win.UI.Label labelWKNo;
        private Class.TxtSeq txtSeq1;
    }
}
