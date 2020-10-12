namespace Sci.Production.Shipping
{
    partial class P62
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.labID = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.dateDeclarationDate = new Sci.Win.UI.DateBox();
            this.labShipper = new Sci.Win.UI.Label();
            this.labBuyer = new Sci.Win.UI.Label();
            this.labDestination = new Sci.Win.UI.Label();
            this.labCustCD = new Sci.Win.UI.Label();
            this.labShipmode = new Sci.Win.UI.Label();
            this.labDeclaration = new Sci.Win.UI.Label();
            this.labForwarder = new Sci.Win.UI.Label();
            this.labETD = new Sci.Win.UI.Label();
            this.labLoading = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.labDecNWGW = new Sci.Win.UI.Label();
            this.labDecCTN = new Sci.Win.UI.Label();
            this.labDecQty = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.numTtlDeclGW = new Sci.Win.UI.NumericBox();
            this.numTtlDeclNW = new Sci.Win.UI.NumericBox();
            this.labelNotApprove = new Sci.Win.UI.Label();
            this.comboShipper = new Sci.Win.UI.ComboBox();
            this.txtbuyer = new Sci.Production.Class.Txtbuyer();
            this.txtshipmode = new Sci.Production.Class.Txtshipmode();
            this.txtcustcd = new Sci.Production.Class.Txtcustcd();
            this.txtcountry = new Sci.Production.Class.Txtcountry();
            this.txtDeclaration = new Sci.Win.UI.TextBox();
            this.dateETD = new Sci.Win.UI.DateBox();
            this.txtForwarder = new Sci.Production.Class.TxtsubconNoConfirm();
            this.txtLoadingPort = new Sci.Win.UI.TextBox();
            this.numDecQty = new Sci.Win.UI.NumericBox();
            this.numDecCTN = new Sci.Win.UI.NumericBox();
            this.numDecAmount = new Sci.Win.UI.NumericBox();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.btnBatchImport = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.btnBatchImport);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.numDecAmount);
            this.masterpanel.Controls.Add(this.numDecCTN);
            this.masterpanel.Controls.Add(this.numDecQty);
            this.masterpanel.Controls.Add(this.txtLoadingPort);
            this.masterpanel.Controls.Add(this.txtForwarder);
            this.masterpanel.Controls.Add(this.txtDeclaration);
            this.masterpanel.Controls.Add(this.txtcountry);
            this.masterpanel.Controls.Add(this.txtcustcd);
            this.masterpanel.Controls.Add(this.txtshipmode);
            this.masterpanel.Controls.Add(this.txtbuyer);
            this.masterpanel.Controls.Add(this.dateETD);
            this.masterpanel.Controls.Add(this.comboShipper);
            this.masterpanel.Controls.Add(this.labelNotApprove);
            this.masterpanel.Controls.Add(this.label4);
            this.masterpanel.Controls.Add(this.numTtlDeclGW);
            this.masterpanel.Controls.Add(this.numTtlDeclNW);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Controls.Add(this.labDecNWGW);
            this.masterpanel.Controls.Add(this.labDecCTN);
            this.masterpanel.Controls.Add(this.labDecQty);
            this.masterpanel.Controls.Add(this.labLoading);
            this.masterpanel.Controls.Add(this.labForwarder);
            this.masterpanel.Controls.Add(this.labETD);
            this.masterpanel.Controls.Add(this.labDeclaration);
            this.masterpanel.Controls.Add(this.labDestination);
            this.masterpanel.Controls.Add(this.labCustCD);
            this.masterpanel.Controls.Add(this.labShipmode);
            this.masterpanel.Controls.Add(this.labBuyer);
            this.masterpanel.Controls.Add(this.labShipper);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.labID);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.dateDeclarationDate);
            this.masterpanel.Size = new System.Drawing.Size(1000, 212);
            this.masterpanel.Controls.SetChildIndex(this.dateDeclarationDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labID, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labShipper, 0);
            this.masterpanel.Controls.SetChildIndex(this.labBuyer, 0);
            this.masterpanel.Controls.SetChildIndex(this.labShipmode, 0);
            this.masterpanel.Controls.SetChildIndex(this.labCustCD, 0);
            this.masterpanel.Controls.SetChildIndex(this.labDestination, 0);
            this.masterpanel.Controls.SetChildIndex(this.labDeclaration, 0);
            this.masterpanel.Controls.SetChildIndex(this.labETD, 0);
            this.masterpanel.Controls.SetChildIndex(this.labForwarder, 0);
            this.masterpanel.Controls.SetChildIndex(this.labLoading, 0);
            this.masterpanel.Controls.SetChildIndex(this.labDecQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.labDecCTN, 0);
            this.masterpanel.Controls.SetChildIndex(this.labDecNWGW, 0);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTtlDeclNW, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTtlDeclGW, 0);
            this.masterpanel.Controls.SetChildIndex(this.label4, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNotApprove, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboShipper, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateETD, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtbuyer, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtshipmode, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtcustcd, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtcountry, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtDeclaration, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtForwarder, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLoadingPort, 0);
            this.masterpanel.Controls.SetChildIndex(this.numDecQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.numDecCTN, 0);
            this.masterpanel.Controls.SetChildIndex(this.numDecAmount, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnBatchImport, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 212);
            this.detailpanel.Size = new System.Drawing.Size(1000, 238);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(892, 174);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1000, 238);
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
            this.detail.Size = new System.Drawing.Size(1000, 488);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1000, 450);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 450);
            this.detailbtm.Size = new System.Drawing.Size(1000, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1000, 488);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1008, 517);
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(140, 11);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(144, 23);
            this.displayID.TabIndex = 4;
            // 
            // labID
            // 
            this.labID.Location = new System.Drawing.Point(30, 11);
            this.labID.Name = "labID";
            this.labID.Size = new System.Drawing.Size(107, 23);
            this.labID.TabIndex = 5;
            this.labID.Text = "ID";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(30, 177);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 23);
            this.label1.TabIndex = 56;
            this.label1.Text = "Remark";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(310, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "Declaration Date";
            // 
            // dateDeclarationDate
            // 
            this.dateDeclarationDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CDate", true));
            this.dateDeclarationDate.Location = new System.Drawing.Point(420, 11);
            this.dateDeclarationDate.Name = "dateDeclarationDate";
            this.dateDeclarationDate.Size = new System.Drawing.Size(110, 23);
            this.dateDeclarationDate.TabIndex = 5;
            // 
            // labShipper
            // 
            this.labShipper.Location = new System.Drawing.Point(30, 38);
            this.labShipper.Name = "labShipper";
            this.labShipper.Size = new System.Drawing.Size(107, 23);
            this.labShipper.TabIndex = 57;
            this.labShipper.Text = "Shipper";
            // 
            // labBuyer
            // 
            this.labBuyer.Location = new System.Drawing.Point(30, 65);
            this.labBuyer.Name = "labBuyer";
            this.labBuyer.Size = new System.Drawing.Size(107, 23);
            this.labBuyer.TabIndex = 58;
            this.labBuyer.Text = "Buyer";
            // 
            // labDestination
            // 
            this.labDestination.Location = new System.Drawing.Point(30, 149);
            this.labDestination.Name = "labDestination";
            this.labDestination.Size = new System.Drawing.Size(107, 23);
            this.labDestination.TabIndex = 61;
            this.labDestination.Text = "Destination";
            // 
            // labCustCD
            // 
            this.labCustCD.Location = new System.Drawing.Point(30, 121);
            this.labCustCD.Name = "labCustCD";
            this.labCustCD.Size = new System.Drawing.Size(107, 23);
            this.labCustCD.TabIndex = 60;
            this.labCustCD.Text = "CustCD";
            // 
            // labShipmode
            // 
            this.labShipmode.Location = new System.Drawing.Point(30, 93);
            this.labShipmode.Name = "labShipmode";
            this.labShipmode.Size = new System.Drawing.Size(107, 23);
            this.labShipmode.TabIndex = 59;
            this.labShipmode.Text = "ShipMode";
            // 
            // labDeclaration
            // 
            this.labDeclaration.Location = new System.Drawing.Point(310, 38);
            this.labDeclaration.Name = "labDeclaration";
            this.labDeclaration.Size = new System.Drawing.Size(107, 23);
            this.labDeclaration.TabIndex = 62;
            this.labDeclaration.Text = "Declaration#";
            // 
            // labForwarder
            // 
            this.labForwarder.Location = new System.Drawing.Point(310, 93);
            this.labForwarder.Name = "labForwarder";
            this.labForwarder.Size = new System.Drawing.Size(107, 23);
            this.labForwarder.TabIndex = 64;
            this.labForwarder.Text = "Forwarder";
            // 
            // labETD
            // 
            this.labETD.Location = new System.Drawing.Point(310, 65);
            this.labETD.Name = "labETD";
            this.labETD.Size = new System.Drawing.Size(107, 23);
            this.labETD.TabIndex = 63;
            this.labETD.Text = "ETD";
            // 
            // labLoading
            // 
            this.labLoading.Location = new System.Drawing.Point(310, 121);
            this.labLoading.Name = "labLoading";
            this.labLoading.Size = new System.Drawing.Size(107, 23);
            this.labLoading.TabIndex = 65;
            this.labLoading.Text = "Loading (Port)";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(596, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(166, 23);
            this.label3.TabIndex = 69;
            this.label3.Text = "Declaration Amount(USD)";
            // 
            // labDecNWGW
            // 
            this.labDecNWGW.Location = new System.Drawing.Point(596, 65);
            this.labDecNWGW.Name = "labDecNWGW";
            this.labDecNWGW.Size = new System.Drawing.Size(166, 23);
            this.labDecNWGW.TabIndex = 68;
            this.labDecNWGW.Text = "Declaration N.W. / G.W.";
            // 
            // labDecCTN
            // 
            this.labDecCTN.Location = new System.Drawing.Point(596, 38);
            this.labDecCTN.Name = "labDecCTN";
            this.labDecCTN.Size = new System.Drawing.Size(166, 23);
            this.labDecCTN.TabIndex = 67;
            this.labDecCTN.Text = "Declaration CTN";
            // 
            // labDecQty
            // 
            this.labDecQty.Location = new System.Drawing.Point(596, 11);
            this.labDecQty.Name = "labDecQty";
            this.labDecQty.Size = new System.Drawing.Size(166, 23);
            this.labDecQty.TabIndex = 66;
            this.labDecQty.Text = "Declaration Qty";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(856, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 23);
            this.label4.TabIndex = 72;
            this.label4.Text = "/";
            this.label4.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // numTtlDeclGW
            // 
            this.numTtlDeclGW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTtlDeclGW.DecimalPlaces = 3;
            this.numTtlDeclGW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTtlDeclGW.IsSupportEditMode = false;
            this.numTtlDeclGW.Location = new System.Drawing.Point(881, 65);
            this.numTtlDeclGW.Name = "numTtlDeclGW";
            this.numTtlDeclGW.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTtlDeclGW.ReadOnly = true;
            this.numTtlDeclGW.Size = new System.Drawing.Size(90, 23);
            this.numTtlDeclGW.TabIndex = 71;
            this.numTtlDeclGW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numTtlDeclNW
            // 
            this.numTtlDeclNW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTtlDeclNW.DecimalPlaces = 3;
            this.numTtlDeclNW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTtlDeclNW.IsSupportEditMode = false;
            this.numTtlDeclNW.Location = new System.Drawing.Point(765, 65);
            this.numTtlDeclNW.Name = "numTtlDeclNW";
            this.numTtlDeclNW.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTtlDeclNW.ReadOnly = true;
            this.numTtlDeclNW.Size = new System.Drawing.Size(90, 23);
            this.numTtlDeclNW.TabIndex = 70;
            this.numTtlDeclNW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelNotApprove
            // 
            this.labelNotApprove.BackColor = System.Drawing.Color.Transparent;
            this.labelNotApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.labelNotApprove.Location = new System.Drawing.Point(866, 11);
            this.labelNotApprove.Name = "labelNotApprove";
            this.labelNotApprove.Size = new System.Drawing.Size(115, 23);
            this.labelNotApprove.TabIndex = 73;
            this.labelNotApprove.Text = "Not Approve";
            this.labelNotApprove.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // comboShipper
            // 
            this.comboShipper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboShipper.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Shipper", true));
            this.comboShipper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboShipper.FormattingEnabled = true;
            this.comboShipper.IsSupportUnselect = true;
            this.comboShipper.Items.AddRange(new object[] {
            "SPS",
            "SPR"});
            this.comboShipper.Location = new System.Drawing.Point(140, 37);
            this.comboShipper.Name = "comboShipper";
            this.comboShipper.OldText = "";
            this.comboShipper.ReadOnly = true;
            this.comboShipper.Size = new System.Drawing.Size(105, 24);
            this.comboShipper.TabIndex = 0;
            // 
            // txtbuyer
            // 
            this.txtbuyer.BackColor = System.Drawing.Color.White;
            this.txtbuyer.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Buyer", true));
            this.txtbuyer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbuyer.Location = new System.Drawing.Point(140, 65);
            this.txtbuyer.Name = "txtbuyer";
            this.txtbuyer.Size = new System.Drawing.Size(105, 23);
            this.txtbuyer.TabIndex = 1;
            // 
            // txtshipmode
            // 
            this.txtshipmode.BackColor = System.Drawing.Color.White;
            this.txtshipmode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "shipModeID", true));
            this.txtshipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshipmode.FormattingEnabled = true;
            this.txtshipmode.IsSupportUnselect = true;
            this.txtshipmode.Location = new System.Drawing.Point(140, 92);
            this.txtshipmode.Name = "txtshipmode";
            this.txtshipmode.OldText = "";
            this.txtshipmode.Size = new System.Drawing.Size(105, 24);
            this.txtshipmode.TabIndex = 2;
            this.txtshipmode.UseFunction = null;
            // 
            // txtcustcd
            // 
            this.txtcustcd.BackColor = System.Drawing.Color.White;
            this.txtcustcd.BrandObjectName = null;
            this.txtcustcd.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "custCDID", true));
            this.txtcustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd.Location = new System.Drawing.Point(140, 122);
            this.txtcustcd.Name = "txtcustcd";
            this.txtcustcd.Size = new System.Drawing.Size(105, 23);
            this.txtcustcd.TabIndex = 77;
            // 
            // txtcountry
            // 
            this.txtcountry.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Dest", true));
            this.txtcountry.DisplayBox1Binding = "";
            this.txtcountry.Location = new System.Drawing.Point(140, 150);
            this.txtcountry.Name = "txtcountry";
            this.txtcountry.Size = new System.Drawing.Size(232, 22);
            this.txtcountry.TabIndex = 3;
            this.txtcountry.TextBox1Binding = "";
            // 
            // txtDeclaration
            // 
            this.txtDeclaration.BackColor = System.Drawing.Color.White;
            this.txtDeclaration.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DeclareNo", true));
            this.txtDeclaration.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDeclaration.Location = new System.Drawing.Point(420, 38);
            this.txtDeclaration.Name = "txtDeclaration";
            this.txtDeclaration.Size = new System.Drawing.Size(114, 23);
            this.txtDeclaration.TabIndex = 6;
            // 
            // dateETD
            // 
            this.dateETD.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ETD", true));
            this.dateETD.Location = new System.Drawing.Point(420, 65);
            this.dateETD.Name = "dateETD";
            this.dateETD.Size = new System.Drawing.Size(110, 23);
            this.dateETD.TabIndex = 7;
            // 
            // txtForwarder
            // 
            this.txtForwarder.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Forwarder", true));
            this.txtForwarder.DisplayBox1Binding = "";
            this.txtForwarder.IsIncludeJunk = false;
            this.txtForwarder.IsMisc = false;
            this.txtForwarder.IsShipping = false;
            this.txtForwarder.IsSubcon = false;
            this.txtForwarder.Location = new System.Drawing.Point(420, 94);
            this.txtForwarder.Name = "txtForwarder";
            this.txtForwarder.Size = new System.Drawing.Size(170, 23);
            this.txtForwarder.TabIndex = 8;
            this.txtForwarder.TextBox1Binding = "";
            // 
            // txtLoadingPort
            // 
            this.txtLoadingPort.BackColor = System.Drawing.Color.White;
            this.txtLoadingPort.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ExportPort", true));
            this.txtLoadingPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLoadingPort.Location = new System.Drawing.Point(420, 122);
            this.txtLoadingPort.Name = "txtLoadingPort";
            this.txtLoadingPort.Size = new System.Drawing.Size(114, 23);
            this.txtLoadingPort.TabIndex = 9;
            this.txtLoadingPort.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtLoadingPort_PopUp);
            this.txtLoadingPort.Validating += new System.ComponentModel.CancelEventHandler(this.TxtLoadingPort_Validating);
            // 
            // numDecQty
            // 
            this.numDecQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numDecQty.DecimalPlaces = 3;
            this.numDecQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numDecQty.IsSupportEditMode = false;
            this.numDecQty.Location = new System.Drawing.Point(765, 11);
            this.numDecQty.Name = "numDecQty";
            this.numDecQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDecQty.ReadOnly = true;
            this.numDecQty.Size = new System.Drawing.Size(90, 23);
            this.numDecQty.TabIndex = 83;
            this.numDecQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numDecCTN
            // 
            this.numDecCTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numDecCTN.DecimalPlaces = 3;
            this.numDecCTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numDecCTN.IsSupportEditMode = false;
            this.numDecCTN.Location = new System.Drawing.Point(765, 37);
            this.numDecCTN.Name = "numDecCTN";
            this.numDecCTN.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDecCTN.ReadOnly = true;
            this.numDecCTN.Size = new System.Drawing.Size(90, 23);
            this.numDecCTN.TabIndex = 84;
            this.numDecCTN.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numDecAmount
            // 
            this.numDecAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numDecAmount.DecimalPlaces = 3;
            this.numDecAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numDecAmount.IsSupportEditMode = false;
            this.numDecAmount.Location = new System.Drawing.Point(765, 94);
            this.numDecAmount.Name = "numDecAmount";
            this.numDecAmount.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDecAmount.ReadOnly = true;
            this.numDecAmount.Size = new System.Drawing.Size(90, 23);
            this.numDecAmount.TabIndex = 85;
            this.numDecAmount.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.editRemark.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(140, 177);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(450, 23);
            this.editRemark.TabIndex = 4;
            // 
            // btnBatchImport
            // 
            this.btnBatchImport.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnBatchImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBatchImport.Location = new System.Drawing.Point(876, 124);
            this.btnBatchImport.Name = "btnBatchImport";
            this.btnBatchImport.Size = new System.Drawing.Size(116, 31);
            this.btnBatchImport.TabIndex = 87;
            this.btnBatchImport.Text = "Batch Import";
            this.btnBatchImport.UseVisualStyleBackColor = true;
            this.btnBatchImport.Click += new System.EventHandler(this.BtnBatchImport_Click);
            // 
            // P62
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1008, 550);
            this.Grid2New = 0;
            this.GridAlias = "KHExportDeclaration_Detail";
            this.GridNew = 0;
            this.GridUniqueKey = "ID,InvNo,OrderID";
            this.IsSupportConfirm = true;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P62";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P62. KH Export Declaration";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "ID";
            this.WorkAlias = "KHExportDeclaration";
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

        private Win.UI.DisplayBox displayID;
        private Win.UI.Label labID;
        private Win.UI.Label label1;
        private Win.UI.Label label3;
        private Win.UI.Label labDecNWGW;
        private Win.UI.Label labDecCTN;
        private Win.UI.Label labDecQty;
        private Win.UI.Label labLoading;
        private Win.UI.Label labForwarder;
        private Win.UI.Label labETD;
        private Win.UI.Label labDeclaration;
        private Win.UI.Label labDestination;
        private Win.UI.Label labCustCD;
        private Win.UI.Label labShipmode;
        private Win.UI.Label labBuyer;
        private Win.UI.Label labShipper;
        private Win.UI.Label label2;
        private Win.UI.DateBox dateDeclarationDate;
        private Win.UI.Label label4;
        private Win.UI.NumericBox numTtlDeclGW;
        private Win.UI.NumericBox numTtlDeclNW;
        private Win.UI.Label labelNotApprove;
        private Class.Txtcountry txtcountry;
        private Class.Txtcustcd txtcustcd;
        private Class.Txtshipmode txtshipmode;
        private Class.Txtbuyer txtbuyer;
        private Win.UI.ComboBox comboShipper;
        private Win.UI.TextBox txtDeclaration;
        private Win.UI.TextBox txtLoadingPort;
        private Class.TxtsubconNoConfirm txtForwarder;
        private Win.UI.DateBox dateETD;
        private Win.UI.NumericBox numDecAmount;
        private Win.UI.NumericBox numDecCTN;
        private Win.UI.NumericBox numDecQty;
        private Win.UI.EditBox editRemark;
        private Win.UI.Button btnBatchImport;
    }
}
