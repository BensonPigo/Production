namespace Sci.Production.Shipping
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
            this.components = new System.ComponentModel.Container();
            this.labelHCNo = new Sci.Win.UI.Label();
            this.labelFrom = new Sci.Win.UI.Label();
            this.labelTO = new Sci.Win.UI.Label();
            this.displayHCNo = new Sci.Win.UI.DisplayBox();
            this.comboFrom = new Sci.Win.UI.ComboBox();
            this.comboTO = new Sci.Win.UI.ComboBox();
            this.txtFrom = new Sci.Win.UI.TextBox();
            this.displayFrom = new Sci.Win.UI.DisplayBox();
            this.txtTO = new Sci.Win.UI.TextBox();
            this.displayTO = new Sci.Win.UI.DisplayBox();
            this.labelShipMark = new Sci.Win.UI.Label();
            this.txtShipMark = new Sci.Win.UI.TextBox();
            this.labelHandle = new Sci.Win.UI.Label();
            this.labelManager = new Sci.Win.UI.Label();
            this.labelDestination = new Sci.Win.UI.Label();
            this.labelPort = new Sci.Win.UI.Label();
            this.txtPort = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.btnMailto = new Sci.Win.UI.Button();
            this.btnCartonDimensionWeight = new Sci.Win.UI.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape3 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.labelShipDate = new Sci.Win.UI.Label();
            this.dateShipDate = new Sci.Win.UI.DateBox();
            this.labelETD = new Sci.Win.UI.Label();
            this.dateETD = new Sci.Win.UI.DateBox();
            this.dateETA = new Sci.Win.UI.DateBox();
            this.labelETA = new Sci.Win.UI.Label();
            this.labelCTNQty = new Sci.Win.UI.Label();
            this.numCTNQty = new Sci.Win.UI.NumericBox();
            this.labelttlNW = new Sci.Win.UI.Label();
            this.numttlNW = new Sci.Win.UI.NumericBox();
            this.labelttlCartonWeight = new Sci.Win.UI.Label();
            this.numttlCartonWeight = new Sci.Win.UI.NumericBox();
            this.labelttlGW = new Sci.Win.UI.Label();
            this.numericBoxttlGW = new Sci.Win.UI.NumericBox();
            this.labelVolumnWeight = new Sci.Win.UI.Label();
            this.numVolumnWeight = new Sci.Win.UI.NumericBox();
            this.labelCarrier = new Sci.Win.UI.Label();
            this.labelExpressAC = new Sci.Win.UI.Label();
            this.labelBLNo = new Sci.Win.UI.Label();
            this.labelInvoiceNo = new Sci.Win.UI.Label();
            this.txtCarrier = new Sci.Win.UI.TextBox();
            this.displayCarrier = new Sci.Win.UI.DisplayBox();
            this.displayExpressAC = new Sci.Win.UI.DisplayBox();
            this.txtBLNo = new Sci.Win.UI.TextBox();
            this.txtInvoiceNo = new Sci.Win.UI.TextBox();
            this.labelRemark = new Sci.Win.UI.Label();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.labelStatupdate = new Sci.Win.UI.Label();
            this.displayStatupdate = new Sci.Win.UI.DisplayBox();
            this.labelSendtoSCI = new Sci.Win.UI.Label();
            this.displaySendtoSCI = new Sci.Win.UI.DisplayBox();
            this.labelDescription = new Sci.Win.UI.Label();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.txtCountryDestination = new Sci.Production.Class.Txtcountry();
            this.txtUserManager = new Sci.Production.Class.Txtuser();
            this.txtUserHandle = new Sci.Production.Class.Txtuser();
            this.cmbPayer = new Sci.Win.UI.ComboBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtCarrierbyCustomer = new Sci.Win.UI.TextBox();
            this.txtCarrierbyFty = new Sci.Win.UI.TextBox();
            this.dispCarrierbyCustomer = new Sci.Win.UI.DisplayBox();
            this.disSuppabb = new Sci.Win.UI.DisplayBox();
            this.chkSpecialSending = new Sci.Win.UI.CheckBox();
            this.btnPaymentDetail = new Sci.Win.UI.Button();
            this.btnHistory = new Sci.Win.UI.Button();
            this.checkBoxDoc = new Sci.Win.UI.CheckBox();
            this.checkTestingCenter = new Sci.Win.UI.CheckBox();
            this.label4 = new Sci.Win.UI.Label();
            this.comboCompany = new Sci.Production.Class.ComboCompany(this.components);
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
            this.masterpanel.Controls.Add(this.comboCompany);
            this.masterpanel.Controls.Add(this.label4);
            this.masterpanel.Controls.Add(this.editDescription);
            this.masterpanel.Controls.Add(this.checkTestingCenter);
            this.masterpanel.Controls.Add(this.labelDescription);
            this.masterpanel.Controls.Add(this.checkBoxDoc);
            this.masterpanel.Controls.Add(this.btnHistory);
            this.masterpanel.Controls.Add(this.labelStatupdate);
            this.masterpanel.Controls.Add(this.displayStatupdate);
            this.masterpanel.Controls.Add(this.btnPaymentDetail);
            this.masterpanel.Controls.Add(this.chkSpecialSending);
            this.masterpanel.Controls.Add(this.disSuppabb);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.dispCarrierbyCustomer);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.txtCarrierbyFty);
            this.masterpanel.Controls.Add(this.txtCarrierbyCustomer);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Controls.Add(this.cmbPayer);
            this.masterpanel.Controls.Add(this.displaySendtoSCI);
            this.masterpanel.Controls.Add(this.labelSendtoSCI);
            this.masterpanel.Controls.Add(this.txtInvoiceNo);
            this.masterpanel.Controls.Add(this.txtBLNo);
            this.masterpanel.Controls.Add(this.displayExpressAC);
            this.masterpanel.Controls.Add(this.displayCarrier);
            this.masterpanel.Controls.Add(this.txtCarrier);
            this.masterpanel.Controls.Add(this.labelInvoiceNo);
            this.masterpanel.Controls.Add(this.labelBLNo);
            this.masterpanel.Controls.Add(this.labelExpressAC);
            this.masterpanel.Controls.Add(this.labelCarrier);
            this.masterpanel.Controls.Add(this.numVolumnWeight);
            this.masterpanel.Controls.Add(this.labelVolumnWeight);
            this.masterpanel.Controls.Add(this.numericBoxttlGW);
            this.masterpanel.Controls.Add(this.labelttlGW);
            this.masterpanel.Controls.Add(this.numttlCartonWeight);
            this.masterpanel.Controls.Add(this.labelttlCartonWeight);
            this.masterpanel.Controls.Add(this.numttlNW);
            this.masterpanel.Controls.Add(this.labelttlNW);
            this.masterpanel.Controls.Add(this.numCTNQty);
            this.masterpanel.Controls.Add(this.labelCTNQty);
            this.masterpanel.Controls.Add(this.labelETA);
            this.masterpanel.Controls.Add(this.labelETD);
            this.masterpanel.Controls.Add(this.labelShipDate);
            this.masterpanel.Controls.Add(this.btnCartonDimensionWeight);
            this.masterpanel.Controls.Add(this.btnMailto);
            this.masterpanel.Controls.Add(this.label9);
            this.masterpanel.Controls.Add(this.txtPort);
            this.masterpanel.Controls.Add(this.txtCountryDestination);
            this.masterpanel.Controls.Add(this.txtUserManager);
            this.masterpanel.Controls.Add(this.txtUserHandle);
            this.masterpanel.Controls.Add(this.labelPort);
            this.masterpanel.Controls.Add(this.labelDestination);
            this.masterpanel.Controls.Add(this.labelManager);
            this.masterpanel.Controls.Add(this.labelHandle);
            this.masterpanel.Controls.Add(this.txtShipMark);
            this.masterpanel.Controls.Add(this.labelShipMark);
            this.masterpanel.Controls.Add(this.displayTO);
            this.masterpanel.Controls.Add(this.txtTO);
            this.masterpanel.Controls.Add(this.displayFrom);
            this.masterpanel.Controls.Add(this.txtFrom);
            this.masterpanel.Controls.Add(this.comboTO);
            this.masterpanel.Controls.Add(this.comboFrom);
            this.masterpanel.Controls.Add(this.displayHCNo);
            this.masterpanel.Controls.Add(this.labelTO);
            this.masterpanel.Controls.Add(this.labelFrom);
            this.masterpanel.Controls.Add(this.labelHCNo);
            this.masterpanel.Controls.Add(this.dateShipDate);
            this.masterpanel.Controls.Add(this.dateETD);
            this.masterpanel.Controls.Add(this.dateETA);
            this.masterpanel.Controls.Add(this.shapeContainer1);
            this.masterpanel.Size = new System.Drawing.Size(1053, 381);
            this.masterpanel.Controls.SetChildIndex(this.shapeContainer1, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateETA, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateETD, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateShipDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelHCNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFrom, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTO, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayHCNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboFrom, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboTO, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtFrom, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayFrom, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtTO, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayTO, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShipMark, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtShipMark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelManager, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDestination, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPort, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtUserHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtUserManager, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtCountryDestination, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtPort, 0);
            this.masterpanel.Controls.SetChildIndex(this.label9, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnMailto, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnCartonDimensionWeight, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShipDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelETD, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelETA, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCTNQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.numCTNQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelttlNW, 0);
            this.masterpanel.Controls.SetChildIndex(this.numttlNW, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelttlCartonWeight, 0);
            this.masterpanel.Controls.SetChildIndex(this.numttlCartonWeight, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelttlGW, 0);
            this.masterpanel.Controls.SetChildIndex(this.numericBoxttlGW, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelVolumnWeight, 0);
            this.masterpanel.Controls.SetChildIndex(this.numVolumnWeight, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCarrier, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelExpressAC, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelBLNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelInvoiceNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtCarrier, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayCarrier, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayExpressAC, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtBLNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtInvoiceNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSendtoSCI, 0);
            this.masterpanel.Controls.SetChildIndex(this.displaySendtoSCI, 0);
            this.masterpanel.Controls.SetChildIndex(this.cmbPayer, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtCarrierbyCustomer, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtCarrierbyFty, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.dispCarrierbyCustomer, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.disSuppabb, 0);
            this.masterpanel.Controls.SetChildIndex(this.chkSpecialSending, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnPaymentDetail, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayStatupdate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelStatupdate, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnHistory, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkBoxDoc, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDescription, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkTestingCenter, 0);
            this.masterpanel.Controls.SetChildIndex(this.editDescription, 0);
            this.masterpanel.Controls.SetChildIndex(this.label4, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboCompany, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 381);
            this.detailpanel.Size = new System.Drawing.Size(1053, 174);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(883, 346);
            // 
            // refresh
            // 
            this.refresh.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.refresh.Location = new System.Drawing.Point(8887, 8);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1053, 174);
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
            this.detail.Size = new System.Drawing.Size(1053, 595);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1053, 555);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 555);
            this.detailbtm.Size = new System.Drawing.Size(1053, 40);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1053, 595);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1061, 624);
            // 
            // createby
            // 
            this.createby.Location = new System.Drawing.Point(71, 11);
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(555, 15);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lblcreateby
            // 
            this.lblcreateby.Location = new System.Drawing.Point(6, 15);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(491, 18);
            this.lbleditby.Size = new System.Drawing.Size(62, 17);
            // 
            // labelHCNo
            // 
            this.labelHCNo.Location = new System.Drawing.Point(7, 4);
            this.labelHCNo.Name = "labelHCNo";
            this.labelHCNo.Size = new System.Drawing.Size(49, 23);
            this.labelHCNo.TabIndex = 18;
            this.labelHCNo.Text = "HC No.";
            // 
            // labelFrom
            // 
            this.labelFrom.Location = new System.Drawing.Point(6, 60);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(49, 23);
            this.labelFrom.TabIndex = 23;
            this.labelFrom.Text = "From";
            // 
            // labelTO
            // 
            this.labelTO.Location = new System.Drawing.Point(6, 87);
            this.labelTO.Name = "labelTO";
            this.labelTO.Size = new System.Drawing.Size(49, 23);
            this.labelTO.TabIndex = 25;
            this.labelTO.Text = "To";
            // 
            // displayHCNo
            // 
            this.displayHCNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayHCNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayHCNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayHCNo.Location = new System.Drawing.Point(59, 4);
            this.displayHCNo.Name = "displayHCNo";
            this.displayHCNo.Size = new System.Drawing.Size(160, 23);
            this.displayHCNo.TabIndex = 19;
            // 
            // comboFrom
            // 
            this.comboFrom.BackColor = System.Drawing.Color.White;
            this.comboFrom.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "FromTag", true));
            this.comboFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFrom.FormattingEnabled = true;
            this.comboFrom.IsSupportUnselect = true;
            this.comboFrom.Location = new System.Drawing.Point(59, 59);
            this.comboFrom.Name = "comboFrom";
            this.comboFrom.OldText = "";
            this.comboFrom.Size = new System.Drawing.Size(80, 24);
            this.comboFrom.TabIndex = 2;
            this.comboFrom.SelectionChangeCommitted += new System.EventHandler(this.ComboFrom_SelectionChangeCommitted);
            // 
            // comboTO
            // 
            this.comboTO.BackColor = System.Drawing.Color.White;
            this.comboTO.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "ToTag", true));
            this.comboTO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboTO.FormattingEnabled = true;
            this.comboTO.IsSupportUnselect = true;
            this.comboTO.Location = new System.Drawing.Point(59, 86);
            this.comboTO.Name = "comboTO";
            this.comboTO.OldText = "";
            this.comboTO.Size = new System.Drawing.Size(80, 24);
            this.comboTO.TabIndex = 4;
            this.comboTO.SelectionChangeCommitted += new System.EventHandler(this.ComboTO_SelectionChangeCommitted);
            // 
            // txtFrom
            // 
            this.txtFrom.BackColor = System.Drawing.Color.White;
            this.txtFrom.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FromSite", true));
            this.txtFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFrom.Location = new System.Drawing.Point(146, 60);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(73, 23);
            this.txtFrom.TabIndex = 3;
            this.txtFrom.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtFrom_PopUp);
            this.txtFrom.Validating += new System.ComponentModel.CancelEventHandler(this.TxtFrom_Validating);
            // 
            // displayFrom
            // 
            this.displayFrom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFrom.Location = new System.Drawing.Point(221, 60);
            this.displayFrom.Name = "displayFrom";
            this.displayFrom.Size = new System.Drawing.Size(195, 23);
            this.displayFrom.TabIndex = 24;
            // 
            // txtTO
            // 
            this.txtTO.BackColor = System.Drawing.Color.White;
            this.txtTO.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ToSite", true));
            this.txtTO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTO.Location = new System.Drawing.Point(146, 87);
            this.txtTO.Name = "txtTO";
            this.txtTO.Size = new System.Drawing.Size(73, 23);
            this.txtTO.TabIndex = 5;
            this.txtTO.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtTO_PopUp);
            this.txtTO.Validating += new System.ComponentModel.CancelEventHandler(this.TxtTO_Validating);
            // 
            // displayTO
            // 
            this.displayTO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTO.Location = new System.Drawing.Point(221, 87);
            this.displayTO.Name = "displayTO";
            this.displayTO.Size = new System.Drawing.Size(195, 23);
            this.displayTO.TabIndex = 26;
            // 
            // labelShipMark
            // 
            this.labelShipMark.Location = new System.Drawing.Point(225, 5);
            this.labelShipMark.Name = "labelShipMark";
            this.labelShipMark.Size = new System.Drawing.Size(93, 23);
            this.labelShipMark.TabIndex = 20;
            this.labelShipMark.Text = "Ship Mark";
            // 
            // txtShipMark
            // 
            this.txtShipMark.BackColor = System.Drawing.Color.White;
            this.txtShipMark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ShipMark", true));
            this.txtShipMark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShipMark.Location = new System.Drawing.Point(322, 5);
            this.txtShipMark.Name = "txtShipMark";
            this.txtShipMark.Size = new System.Drawing.Size(92, 23);
            this.txtShipMark.TabIndex = 0;
            // 
            // labelHandle
            // 
            this.labelHandle.Location = new System.Drawing.Point(431, 4);
            this.labelHandle.Name = "labelHandle";
            this.labelHandle.Size = new System.Drawing.Size(92, 23);
            this.labelHandle.TabIndex = 43;
            this.labelHandle.Text = "Handle";
            // 
            // labelManager
            // 
            this.labelManager.Location = new System.Drawing.Point(431, 31);
            this.labelManager.Name = "labelManager";
            this.labelManager.Size = new System.Drawing.Size(92, 23);
            this.labelManager.TabIndex = 44;
            this.labelManager.Text = "Manager";
            // 
            // labelDestination
            // 
            this.labelDestination.Location = new System.Drawing.Point(431, 58);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(92, 23);
            this.labelDestination.TabIndex = 45;
            this.labelDestination.Text = "Destination";
            // 
            // labelPort
            // 
            this.labelPort.Location = new System.Drawing.Point(714, 58);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(54, 23);
            this.labelPort.TabIndex = 46;
            this.labelPort.Text = "Port";
            // 
            // txtPort
            // 
            this.txtPort.BackColor = System.Drawing.Color.White;
            this.txtPort.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "PortAir", true));
            this.txtPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPort.Location = new System.Drawing.Point(771, 58);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(127, 23);
            this.txtPort.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Status", true));
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label9.Location = new System.Drawing.Point(901, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(109, 23);
            this.label9.TabIndex = 67;
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Fuchsia;
            this.label9.TextStyle.Color = System.Drawing.Color.Fuchsia;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Fuchsia;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Fuchsia;
            // 
            // btnMailto
            // 
            this.btnMailto.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnMailto.Location = new System.Drawing.Point(905, 32);
            this.btnMailto.Name = "btnMailto";
            this.btnMailto.Size = new System.Drawing.Size(140, 30);
            this.btnMailto.TabIndex = 68;
            this.btnMailto.Text = "Mail to";
            this.btnMailto.UseVisualStyleBackColor = true;
            this.btnMailto.Click += new System.EventHandler(this.BtnMailto_Click);
            // 
            // btnCartonDimensionWeight
            // 
            this.btnCartonDimensionWeight.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnCartonDimensionWeight.Location = new System.Drawing.Point(905, 69);
            this.btnCartonDimensionWeight.Name = "btnCartonDimensionWeight";
            this.btnCartonDimensionWeight.Size = new System.Drawing.Size(140, 51);
            this.btnCartonDimensionWeight.TabIndex = 69;
            this.btnCartonDimensionWeight.Text = "Carton Dimension && Weight";
            this.btnCartonDimensionWeight.UseVisualStyleBackColor = true;
            this.btnCartonDimensionWeight.Click += new System.EventHandler(this.BtnCartonDimensionWeight_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape3,
            this.lineShape2,
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(1053, 381);
            this.shapeContainer1.TabIndex = 24;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape3
            // 
            this.lineShape3.BorderColor = System.Drawing.SystemColors.AppWorkspace;
            this.lineShape3.Name = "lineShape3";
            this.lineShape3.X1 = 4;
            this.lineShape3.X2 = 900;
            this.lineShape3.Y1 = 175;
            this.lineShape3.Y2 = 175;
            // 
            // lineShape2
            // 
            this.lineShape2.BorderColor = System.Drawing.SystemColors.AppWorkspace;
            this.lineShape2.Name = "lineShape2";
            this.lineShape2.X1 = 4;
            this.lineShape2.X2 = 900;
            this.lineShape2.Y1 = 143;
            this.lineShape2.Y2 = 143;
            // 
            // lineShape1
            // 
            this.lineShape1.BorderColor = System.Drawing.SystemColors.AppWorkspace;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 4;
            this.lineShape1.X2 = 900;
            this.lineShape1.Y1 = 111;
            this.lineShape1.Y2 = 111;
            // 
            // labelShipDate
            // 
            this.labelShipDate.Location = new System.Drawing.Point(7, 120);
            this.labelShipDate.Name = "labelShipDate";
            this.labelShipDate.Size = new System.Drawing.Size(80, 23);
            this.labelShipDate.TabIndex = 27;
            this.labelShipDate.Text = "Ship. Date";
            // 
            // dateShipDate
            // 
            this.dateShipDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ShipDate", true));
            this.dateShipDate.Location = new System.Drawing.Point(90, 120);
            this.dateShipDate.Name = "dateShipDate";
            this.dateShipDate.Size = new System.Drawing.Size(128, 23);
            this.dateShipDate.TabIndex = 6;
            // 
            // labelETD
            // 
            this.labelETD.Location = new System.Drawing.Point(221, 120);
            this.labelETD.Name = "labelETD";
            this.labelETD.Size = new System.Drawing.Size(61, 23);
            this.labelETD.TabIndex = 28;
            this.labelETD.Text = "ETD";
            // 
            // dateETD
            // 
            this.dateETD.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ETD", true));
            this.dateETD.Location = new System.Drawing.Point(286, 120);
            this.dateETD.Name = "dateETD";
            this.dateETD.Size = new System.Drawing.Size(128, 23);
            this.dateETD.TabIndex = 7;
            this.dateETD.Validating += new System.ComponentModel.CancelEventHandler(this.DateETD_Validating);
            // 
            // dateETA
            // 
            this.dateETA.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ETA", true));
            this.dateETA.Location = new System.Drawing.Point(286, 146);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(128, 23);
            this.dateETA.TabIndex = 8;
            this.dateETA.Validating += new System.ComponentModel.CancelEventHandler(this.DateETA_Validating);
            // 
            // labelETA
            // 
            this.labelETA.Location = new System.Drawing.Point(221, 146);
            this.labelETA.Name = "labelETA";
            this.labelETA.Size = new System.Drawing.Size(61, 23);
            this.labelETA.TabIndex = 31;
            this.labelETA.Text = "ETA";
            // 
            // labelCTNQty
            // 
            this.labelCTNQty.Location = new System.Drawing.Point(755, 120);
            this.labelCTNQty.Name = "labelCTNQty";
            this.labelCTNQty.Size = new System.Drawing.Size(65, 23);
            this.labelCTNQty.TabIndex = 53;
            this.labelCTNQty.Text = "CTN Q\'ty";
            // 
            // numCTNQty
            // 
            this.numCTNQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numCTNQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CTNQty", true));
            this.numCTNQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numCTNQty.IsSupportEditMode = false;
            this.numCTNQty.Location = new System.Drawing.Point(823, 120);
            this.numCTNQty.Name = "numCTNQty";
            this.numCTNQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCTNQty.ReadOnly = true;
            this.numCTNQty.Size = new System.Drawing.Size(78, 23);
            this.numCTNQty.TabIndex = 54;
            this.numCTNQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelttlNW
            // 
            this.labelttlNW.Location = new System.Drawing.Point(431, 120);
            this.labelttlNW.Name = "labelttlNW";
            this.labelttlNW.Size = new System.Drawing.Size(54, 23);
            this.labelttlNW.TabIndex = 49;
            this.labelttlNW.Text = "ttl N.W.";
            // 
            // numttlNW
            // 
            this.numttlNW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numttlNW.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NW", true));
            this.numttlNW.DecimalPlaces = 3;
            this.numttlNW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numttlNW.IsSupportEditMode = false;
            this.numttlNW.Location = new System.Drawing.Point(488, 120);
            this.numttlNW.Name = "numttlNW";
            this.numttlNW.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numttlNW.ReadOnly = true;
            this.numttlNW.Size = new System.Drawing.Size(72, 23);
            this.numttlNW.TabIndex = 50;
            this.numttlNW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelttlCartonWeight
            // 
            this.labelttlCartonWeight.Location = new System.Drawing.Point(563, 148);
            this.labelttlCartonWeight.Name = "labelttlCartonWeight";
            this.labelttlCartonWeight.Size = new System.Drawing.Size(106, 23);
            this.labelttlCartonWeight.TabIndex = 57;
            this.labelttlCartonWeight.Text = "ttl Carton Weight";
            // 
            // numttlCartonWeight
            // 
            this.numttlCartonWeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numttlCartonWeight.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CTNNW", true));
            this.numttlCartonWeight.DecimalPlaces = 2;
            this.numttlCartonWeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numttlCartonWeight.IsSupportEditMode = false;
            this.numttlCartonWeight.Location = new System.Drawing.Point(672, 149);
            this.numttlCartonWeight.Name = "numttlCartonWeight";
            this.numttlCartonWeight.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numttlCartonWeight.ReadOnly = true;
            this.numttlCartonWeight.Size = new System.Drawing.Size(77, 23);
            this.numttlCartonWeight.TabIndex = 58;
            this.numttlCartonWeight.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelttlGW
            // 
            this.labelttlGW.Location = new System.Drawing.Point(431, 150);
            this.labelttlGW.Name = "labelttlGW";
            this.labelttlGW.Size = new System.Drawing.Size(54, 23);
            this.labelttlGW.TabIndex = 55;
            this.labelttlGW.Text = "ttl G.W.";
            // 
            // numericBoxttlGW
            // 
            this.numericBoxttlGW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numericBoxttlGW.DecimalPlaces = 3;
            this.numericBoxttlGW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numericBoxttlGW.IsSupportEditMode = false;
            this.numericBoxttlGW.Location = new System.Drawing.Point(488, 148);
            this.numericBoxttlGW.Name = "numericBoxttlGW";
            this.numericBoxttlGW.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBoxttlGW.ReadOnly = true;
            this.numericBoxttlGW.Size = new System.Drawing.Size(72, 23);
            this.numericBoxttlGW.TabIndex = 56;
            this.numericBoxttlGW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelVolumnWeight
            // 
            this.labelVolumnWeight.Location = new System.Drawing.Point(563, 120);
            this.labelVolumnWeight.Name = "labelVolumnWeight";
            this.labelVolumnWeight.Size = new System.Drawing.Size(106, 23);
            this.labelVolumnWeight.TabIndex = 51;
            this.labelVolumnWeight.Text = "Volumn Weight";
            // 
            // numVolumnWeight
            // 
            this.numVolumnWeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numVolumnWeight.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "VW", true));
            this.numVolumnWeight.DecimalPlaces = 2;
            this.numVolumnWeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numVolumnWeight.IsSupportEditMode = false;
            this.numVolumnWeight.Location = new System.Drawing.Point(672, 120);
            this.numVolumnWeight.Name = "numVolumnWeight";
            this.numVolumnWeight.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numVolumnWeight.ReadOnly = true;
            this.numVolumnWeight.Size = new System.Drawing.Size(77, 23);
            this.numVolumnWeight.TabIndex = 52;
            this.numVolumnWeight.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelCarrier
            // 
            this.labelCarrier.Location = new System.Drawing.Point(7, 207);
            this.labelCarrier.Name = "labelCarrier";
            this.labelCarrier.Size = new System.Drawing.Size(133, 23);
            this.labelCarrier.TabIndex = 33;
            this.labelCarrier.Text = "Carrier by 3rd Party";
            // 
            // labelExpressAC
            // 
            this.labelExpressAC.Location = new System.Drawing.Point(431, 207);
            this.labelExpressAC.Name = "labelExpressAC";
            this.labelExpressAC.Size = new System.Drawing.Size(92, 23);
            this.labelExpressAC.TabIndex = 60;
            this.labelExpressAC.Text = "Express A/C#";
            // 
            // labelBLNo
            // 
            this.labelBLNo.Location = new System.Drawing.Point(431, 234);
            this.labelBLNo.Name = "labelBLNo";
            this.labelBLNo.Size = new System.Drawing.Size(92, 23);
            this.labelBLNo.TabIndex = 62;
            this.labelBLNo.Text = "B/L No.";
            // 
            // labelInvoiceNo
            // 
            this.labelInvoiceNo.Location = new System.Drawing.Point(431, 179);
            this.labelInvoiceNo.Name = "labelInvoiceNo";
            this.labelInvoiceNo.Size = new System.Drawing.Size(92, 23);
            this.labelInvoiceNo.TabIndex = 59;
            this.labelInvoiceNo.Text = "Invoice No.";
            // 
            // txtCarrier
            // 
            this.txtCarrier.BackColor = System.Drawing.Color.White;
            this.txtCarrier.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CarrierID", true));
            this.txtCarrier.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCarrier.Location = new System.Drawing.Point(142, 207);
            this.txtCarrier.Name = "txtCarrier";
            this.txtCarrier.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditMode;
            this.txtCarrier.Size = new System.Drawing.Size(40, 23);
            this.txtCarrier.TabIndex = 34;
            this.txtCarrier.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtCarrier_PopUp);
            this.txtCarrier.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtCarrier_KeyDown);
            // 
            // displayCarrier
            // 
            this.displayCarrier.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCarrier.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCarrier.Location = new System.Drawing.Point(186, 207);
            this.displayCarrier.Name = "displayCarrier";
            this.displayCarrier.Size = new System.Drawing.Size(233, 23);
            this.displayCarrier.TabIndex = 35;
            // 
            // displayExpressAC
            // 
            this.displayExpressAC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayExpressAC.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ExpressACNo", true));
            this.displayExpressAC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayExpressAC.Location = new System.Drawing.Point(526, 208);
            this.displayExpressAC.Name = "displayExpressAC";
            this.displayExpressAC.Size = new System.Drawing.Size(223, 23);
            this.displayExpressAC.TabIndex = 61;
            // 
            // txtBLNo
            // 
            this.txtBLNo.BackColor = System.Drawing.Color.White;
            this.txtBLNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BLNo", true));
            this.txtBLNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBLNo.Location = new System.Drawing.Point(526, 234);
            this.txtBLNo.Name = "txtBLNo";
            this.txtBLNo.Size = new System.Drawing.Size(223, 23);
            this.txtBLNo.TabIndex = 17;
            this.txtBLNo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtBLNo_Validating);
            // 
            // txtInvoiceNo
            // 
            this.txtInvoiceNo.BackColor = System.Drawing.Color.White;
            this.txtInvoiceNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FtyInvNo", true));
            this.txtInvoiceNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvoiceNo.Location = new System.Drawing.Point(526, 180);
            this.txtInvoiceNo.Name = "txtInvoiceNo";
            this.txtInvoiceNo.Size = new System.Drawing.Size(223, 23);
            this.txtInvoiceNo.TabIndex = 16;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(6, 290);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(133, 23);
            this.labelRemark.TabIndex = 42;
            this.labelRemark.Text = "Remark";
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(142, 290);
            this.editRemark.MaxLength = 100;
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(279, 79);
            this.editRemark.TabIndex = 10;
            // 
            // labelStatupdate
            // 
            this.labelStatupdate.Location = new System.Drawing.Point(7, 147);
            this.labelStatupdate.Name = "labelStatupdate";
            this.labelStatupdate.Size = new System.Drawing.Size(80, 23);
            this.labelStatupdate.TabIndex = 29;
            this.labelStatupdate.Text = "Stat. update";
            // 
            // displayStatupdate
            // 
            this.displayStatupdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStatupdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStatupdate.Location = new System.Drawing.Point(91, 147);
            this.displayStatupdate.Name = "displayStatupdate";
            this.displayStatupdate.Size = new System.Drawing.Size(127, 23);
            this.displayStatupdate.TabIndex = 30;
            // 
            // labelSendtoSCI
            // 
            this.labelSendtoSCI.Location = new System.Drawing.Point(431, 261);
            this.labelSendtoSCI.Name = "labelSendtoSCI";
            this.labelSendtoSCI.Size = new System.Drawing.Size(92, 23);
            this.labelSendtoSCI.TabIndex = 63;
            this.labelSendtoSCI.Text = "Send to SCI";
            // 
            // displaySendtoSCI
            // 
            this.displaySendtoSCI.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySendtoSCI.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySendtoSCI.Location = new System.Drawing.Point(526, 261);
            this.displaySendtoSCI.Name = "displaySendtoSCI";
            this.displaySendtoSCI.Size = new System.Drawing.Size(223, 23);
            this.displaySendtoSCI.TabIndex = 64;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(431, 290);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(92, 23);
            this.labelDescription.TabIndex = 65;
            this.labelDescription.Text = "Description";
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editDescription.IsSupportEditMode = false;
            this.editDescription.Location = new System.Drawing.Point(526, 290);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.ReadOnly = true;
            this.editDescription.Size = new System.Drawing.Size(372, 79);
            this.editDescription.TabIndex = 66;
            // 
            // txtCountryDestination
            // 
            this.txtCountryDestination.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Dest", true));
            this.txtCountryDestination.DisplayBox1Binding = "";
            this.txtCountryDestination.Location = new System.Drawing.Point(526, 59);
            this.txtCountryDestination.Name = "txtCountryDestination";
            this.txtCountryDestination.Size = new System.Drawing.Size(185, 23);
            this.txtCountryDestination.TabIndex = 13;
            this.txtCountryDestination.TextBox1Binding = "";
            // 
            // txtUserManager
            // 
            this.txtUserManager.AllowSelectResign = false;
            this.txtUserManager.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Manager", true));
            this.txtUserManager.DisplayBox1Binding = "";
            this.txtUserManager.Location = new System.Drawing.Point(526, 31);
            this.txtUserManager.Name = "txtUserManager";
            this.txtUserManager.Size = new System.Drawing.Size(372, 23);
            this.txtUserManager.TabIndex = 12;
            this.txtUserManager.TextBox1Binding = "";
            // 
            // txtUserHandle
            // 
            this.txtUserHandle.AllowSelectResign = false;
            this.txtUserHandle.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Handle", true));
            this.txtUserHandle.DisplayBox1Binding = "";
            this.txtUserHandle.Location = new System.Drawing.Point(526, 5);
            this.txtUserHandle.Name = "txtUserHandle";
            this.txtUserHandle.Size = new System.Drawing.Size(372, 23);
            this.txtUserHandle.TabIndex = 11;
            this.txtUserHandle.TextBox1Binding = "";
            // 
            // cmbPayer
            // 
            this.cmbPayer.BackColor = System.Drawing.Color.White;
            this.cmbPayer.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "FreightBy", true));
            this.cmbPayer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbPayer.FormattingEnabled = true;
            this.cmbPayer.IsSupportUnselect = true;
            this.cmbPayer.Location = new System.Drawing.Point(142, 179);
            this.cmbPayer.Name = "cmbPayer";
            this.cmbPayer.OldText = "";
            this.cmbPayer.Size = new System.Drawing.Size(274, 24);
            this.cmbPayer.TabIndex = 9;
            this.cmbPayer.SelectedIndexChanged += new System.EventHandler(this.CmbPayer_SelectedIndexChanged);
            this.cmbPayer.SelectionChangeCommitted += new System.EventHandler(this.CmbPayer_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 23);
            this.label1.TabIndex = 32;
            this.label1.Text = "Payer";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 234);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 23);
            this.label2.TabIndex = 36;
            this.label2.Text = "Carrier by Customer";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(7, 261);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 23);
            this.label3.TabIndex = 39;
            this.label3.Text = "Carrier by Fty";
            // 
            // txtCarrierbyCustomer
            // 
            this.txtCarrierbyCustomer.BackColor = System.Drawing.Color.White;
            this.txtCarrierbyCustomer.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ByCustomerCarrier", true));
            this.txtCarrierbyCustomer.Enabled = false;
            this.txtCarrierbyCustomer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCarrierbyCustomer.Location = new System.Drawing.Point(142, 234);
            this.txtCarrierbyCustomer.Name = "txtCarrierbyCustomer";
            this.txtCarrierbyCustomer.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditMode;
            this.txtCarrierbyCustomer.Size = new System.Drawing.Size(77, 23);
            this.txtCarrierbyCustomer.TabIndex = 37;
            this.txtCarrierbyCustomer.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtCarrierbyCustomer_PopUp);
            this.txtCarrierbyCustomer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtCarrierbyCustomer_KeyDown);
            // 
            // txtCarrierbyFty
            // 
            this.txtCarrierbyFty.BackColor = System.Drawing.Color.White;
            this.txtCarrierbyFty.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ByFtyCarrier", true));
            this.txtCarrierbyFty.Enabled = false;
            this.txtCarrierbyFty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCarrierbyFty.Location = new System.Drawing.Point(142, 261);
            this.txtCarrierbyFty.Name = "txtCarrierbyFty";
            this.txtCarrierbyFty.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditMode;
            this.txtCarrierbyFty.Size = new System.Drawing.Size(77, 23);
            this.txtCarrierbyFty.TabIndex = 40;
            this.txtCarrierbyFty.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtCarrierbyFty_PopUp);
            this.txtCarrierbyFty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtCarrierbyFty_KeyDown);
            // 
            // dispCarrierbyCustomer
            // 
            this.dispCarrierbyCustomer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispCarrierbyCustomer.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ByCustomerAccountID", true));
            this.dispCarrierbyCustomer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispCarrierbyCustomer.Location = new System.Drawing.Point(225, 235);
            this.dispCarrierbyCustomer.Name = "dispCarrierbyCustomer";
            this.dispCarrierbyCustomer.Size = new System.Drawing.Size(194, 23);
            this.dispCarrierbyCustomer.TabIndex = 38;
            // 
            // disSuppabb
            // 
            this.disSuppabb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disSuppabb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disSuppabb.Location = new System.Drawing.Point(225, 261);
            this.disSuppabb.Name = "disSuppabb";
            this.disSuppabb.Size = new System.Drawing.Size(194, 23);
            this.disSuppabb.TabIndex = 41;
            // 
            // chkSpecialSending
            // 
            this.chkSpecialSending.AutoSize = true;
            this.chkSpecialSending.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsSpecialSending", true));
            this.chkSpecialSending.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.chkSpecialSending.IsSupportEditMode = false;
            this.chkSpecialSending.Location = new System.Drawing.Point(431, 87);
            this.chkSpecialSending.Name = "chkSpecialSending";
            this.chkSpecialSending.ReadOnly = true;
            this.chkSpecialSending.Size = new System.Drawing.Size(129, 21);
            this.chkSpecialSending.TabIndex = 47;
            this.chkSpecialSending.Text = "Special Sending";
            this.chkSpecialSending.UseVisualStyleBackColor = true;
            // 
            // btnPaymentDetail
            // 
            this.btnPaymentDetail.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnPaymentDetail.Location = new System.Drawing.Point(905, 126);
            this.btnPaymentDetail.Name = "btnPaymentDetail";
            this.btnPaymentDetail.Size = new System.Drawing.Size(140, 38);
            this.btnPaymentDetail.TabIndex = 70;
            this.btnPaymentDetail.Text = "Payment Detail";
            this.btnPaymentDetail.UseVisualStyleBackColor = true;
            this.btnPaymentDetail.Click += new System.EventHandler(this.BtnPaymentDetail_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnHistory.Location = new System.Drawing.Point(905, 170);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(140, 38);
            this.btnHistory.TabIndex = 71;
            this.btnHistory.Text = "History";
            this.btnHistory.UseVisualStyleBackColor = true;
            this.btnHistory.Click += new System.EventHandler(this.BtnHistory_Click);
            // 
            // checkBoxDoc
            // 
            this.checkBoxDoc.AutoSize = true;
            this.checkBoxDoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkBoxDoc.IsSupportEditMode = false;
            this.checkBoxDoc.Location = new System.Drawing.Point(558, 87);
            this.checkBoxDoc.Name = "checkBoxDoc";
            this.checkBoxDoc.ReadOnly = true;
            this.checkBoxDoc.Size = new System.Drawing.Size(168, 21);
            this.checkBoxDoc.TabIndex = 48;
            this.checkBoxDoc.Text = "DOC(Envelope ONLY)";
            this.checkBoxDoc.UseVisualStyleBackColor = true;
            // 
            // checkTestingCenter
            // 
            this.checkTestingCenter.AutoSize = true;
            this.checkTestingCenter.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Testing_Center", true));
            this.checkTestingCenter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkTestingCenter.Location = new System.Drawing.Point(725, 87);
            this.checkTestingCenter.Name = "checkTestingCenter";
            this.checkTestingCenter.ReadOnly = true;
            this.checkTestingCenter.Size = new System.Drawing.Size(120, 21);
            this.checkTestingCenter.TabIndex = 15;
            this.checkTestingCenter.Text = "Testing Center";
            this.checkTestingCenter.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(7, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 23);
            this.label4.TabIndex = 22;
            this.label4.Text = "Order Company";
            // 
            // comboCompany
            // 
            this.comboCompany.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboCompany.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "OrderCompanyID", true));
            this.comboCompany.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboCompany.FormattingEnabled = true;
            this.comboCompany.IsOrderCompany = false;
            this.comboCompany.IsSupportUnselect = true;
            this.comboCompany.Junk = false;
            this.comboCompany.Location = new System.Drawing.Point(146, 32);
            this.comboCompany.Name = "comboCompany";
            this.comboCompany.OldText = "";
            this.comboCompany.ReadOnly = true;
            this.comboCompany.Size = new System.Drawing.Size(270, 24);
            this.comboCompany.TabIndex = 1;
            this.comboCompany.SelectedValueChanged += new System.EventHandler(this.ComboCompany_SelectedValueChanged);
            // 
            // P02
            // 
            this.ApvChkValue = "Sent";
            this.ClientSize = new System.Drawing.Size(1061, 657);
            this.DefaultControl = "comboFrom";
            this.DefaultControlForEdit = "comboFrom";
            this.DefaultDetailOrder = "CTNNo,Seq1";
            this.DefaultOrder = "ShipDate,ID";
            this.GridAlias = "Express_Detail";
            this.GridNew = 0;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportJunk = true;
            this.IsSupportRecall = true;
            this.IsSupportSend = true;
            this.IsSupportUnconfirm = true;
            this.JunkChkValue = "New";
            this.KeyField1 = "ID";
            this.Name = "P02";
            this.OnLineHelpID = "Sci.Win.Tems.Input2";
            this.RecallChkValue = "Sent";
            this.SendChkValue = "New";
            this.Text = "P02. International Express";
            this.UnApvChkValue = "Approved";
            this.UniqueExpress = "ID";
            this.WorkAlias = "Express";
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

        private Win.UI.NumericBox numVolumnWeight;
        private Win.UI.Label labelVolumnWeight;
        private Win.UI.NumericBox numericBoxttlGW;
        private Win.UI.Label labelttlGW;
        private Win.UI.NumericBox numttlCartonWeight;
        private Win.UI.Label labelttlCartonWeight;
        private Win.UI.NumericBox numttlNW;
        private Win.UI.Label labelttlNW;
        private Win.UI.NumericBox numCTNQty;
        private Win.UI.Label labelCTNQty;
        private Win.UI.DateBox dateETA;
        private Win.UI.Label labelETA;
        private Win.UI.DateBox dateETD;
        private Win.UI.Label labelETD;
        private Win.UI.DateBox dateShipDate;
        private Win.UI.Label labelShipDate;
        private Win.UI.Button btnCartonDimensionWeight;
        private Win.UI.Button btnMailto;
        private Win.UI.Label label9;
        private Win.UI.TextBox txtPort;
        private Class.Txtcountry txtCountryDestination;
        private Class.Txtuser txtUserManager;
        private Class.Txtuser txtUserHandle;
        private Win.UI.Label labelPort;
        private Win.UI.Label labelDestination;
        private Win.UI.Label labelManager;
        private Win.UI.Label labelHandle;
        private Win.UI.TextBox txtShipMark;
        private Win.UI.Label labelShipMark;
        private Win.UI.DisplayBox displayTO;
        private Win.UI.TextBox txtTO;
        private Win.UI.DisplayBox displayFrom;
        private Win.UI.TextBox txtFrom;
        private Win.UI.ComboBox comboTO;
        private Win.UI.ComboBox comboFrom;
        private Win.UI.DisplayBox displayHCNo;
        private Win.UI.Label labelTO;
        private Win.UI.Label labelFrom;
        private Win.UI.Label labelHCNo;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape3;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.DisplayBox displaySendtoSCI;
        private Win.UI.Label labelSendtoSCI;
        private Win.UI.DisplayBox displayStatupdate;
        private Win.UI.Label labelStatupdate;
        private Win.UI.EditBox editRemark;
        private Win.UI.Label labelRemark;
        private Win.UI.TextBox txtInvoiceNo;
        private Win.UI.TextBox txtBLNo;
        private Win.UI.DisplayBox displayExpressAC;
        private Win.UI.DisplayBox displayCarrier;
        private Win.UI.TextBox txtCarrier;
        private Win.UI.Label labelInvoiceNo;
        private Win.UI.Label labelBLNo;
        private Win.UI.Label labelExpressAC;
        private Win.UI.Label labelCarrier;
        private Win.UI.EditBox editDescription;
        private Win.UI.Label labelDescription;
        private Win.UI.ComboBox cmbPayer;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtCarrierbyFty;
        private Win.UI.TextBox txtCarrierbyCustomer;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.DisplayBox dispCarrierbyCustomer;
        private Win.UI.DisplayBox disSuppabb;
        private Win.UI.CheckBox chkSpecialSending;
        private Win.UI.Button btnHistory;
        private Win.UI.Button btnPaymentDetail;
        private Win.UI.CheckBox checkTestingCenter;
        private Win.UI.CheckBox checkBoxDoc;
        private Class.ComboCompany comboCompany;
        private Win.UI.Label label4;
    }
}
