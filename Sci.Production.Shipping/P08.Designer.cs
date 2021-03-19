namespace Sci.Production.Shipping
{
    partial class P08
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
            this.labelDate = new Sci.Win.UI.Label();
            this.labelType = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelTerms = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.labelInvoice = new Sci.Win.UI.Label();
            this.displayNo = new Sci.Win.UI.DisplayBox();
            this.dateDate = new Sci.Win.UI.DateBox();
            this.comboType = new Sci.Win.UI.ComboBox();
            this.comboType2 = new Sci.Win.UI.ComboBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.txtInvoice = new Sci.Win.UI.TextBox();
            this.labelCurrency = new Sci.Win.UI.Label();
            this.labelAmount = new Sci.Win.UI.Label();
            this.labelVATRate = new Sci.Win.UI.Label();
            this.labelVAT = new Sci.Win.UI.Label();
            this.labelTotal = new Sci.Win.UI.Label();
            this.labelBLNo = new Sci.Win.UI.Label();
            this.numAmount = new Sci.Win.UI.NumericBox();
            this.numVATRate = new Sci.Win.UI.NumericBox();
            this.numVAT = new Sci.Win.UI.NumericBox();
            this.numTotal = new Sci.Win.UI.NumericBox();
            this.txtBLNo = new Sci.Win.UI.TextBox();
            this.labelHandle = new Sci.Win.UI.Label();
            this.labelAccountant = new Sci.Win.UI.Label();
            this.labelApvDate = new Sci.Win.UI.Label();
            this.labelVoucherNo = new Sci.Win.UI.Label();
            this.dateApvDate = new Sci.Win.UI.DateBox();
            this.displayVoucherNo = new Sci.Win.UI.DisplayBox();
            this.btnShareExpense = new Sci.Win.UI.Button();
            this.btnAcctApprove = new Sci.Win.UI.Button();
            this.displayM = new Sci.Win.UI.DisplayBox();
            this.labelM = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.comboFactory1 = new Sci.Production.Class.ComboFactory(this.components);
            this.txtSserAccountant = new Sci.Production.Class.Txtuser();
            this.txtUserHandle = new Sci.Production.Class.Txtuser();
            this.txtpayterm_ftyTerms = new Sci.Production.Class.Txtpayterm_fty();
            this.txtSubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.lbExVoucherID = new Sci.Win.UI.Label();
            this.disExVoucherID = new Sci.Win.UI.DisplayBox();
            this.labReason = new Sci.Win.UI.Label();
            this.txtReason = new Sci.Win.UI.TextBox();
            this.txtReasonDesc = new Sci.Win.UI.TextBox();
            this.lbVesselName = new Sci.Win.UI.Label();
            this.disVesselName = new Sci.Win.UI.DisplayBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txtVoucherDate = new Sci.Win.UI.DateBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.numericBoxShareAmtFactory = new Sci.Win.UI.NumericBox();
            this.numericBoxShareAmt = new Sci.Win.UI.NumericBox();
            this.label5 = new Sci.Win.UI.Label();
            this.numericBox1 = new Sci.Win.UI.NumericBox();
            this.chkIncludeFoundry = new Sci.Win.UI.CheckBox();
            this.lbSisFtyAPID = new Sci.Win.UI.Label();
            this.txtSisFtyAPID = new Sci.Win.UI.TextBox();
            this.txtcurrency = new Sci.Production.Class.Txtcurrency();
            this.btnIncludeFoundryRatio = new Sci.Win.UI.Button();
            this.checkIsFreightForwarder = new Sci.Win.UI.CheckBox();
            this.btnImport = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.btnImport);
            this.masterpanel.Controls.Add(this.checkIsFreightForwarder);
            this.masterpanel.Controls.Add(this.btnIncludeFoundryRatio);
            this.masterpanel.Controls.Add(this.txtcurrency);
            this.masterpanel.Controls.Add(this.txtSisFtyAPID);
            this.masterpanel.Controls.Add(this.lbSisFtyAPID);
            this.masterpanel.Controls.Add(this.chkIncludeFoundry);
            this.masterpanel.Controls.Add(this.numericBox1);
            this.masterpanel.Controls.Add(this.label5);
            this.masterpanel.Controls.Add(this.numericBoxShareAmt);
            this.masterpanel.Controls.Add(this.numericBoxShareAmtFactory);
            this.masterpanel.Controls.Add(this.label4);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.disVesselName);
            this.masterpanel.Controls.Add(this.lbVesselName);
            this.masterpanel.Controls.Add(this.txtReasonDesc);
            this.masterpanel.Controls.Add(this.txtReason);
            this.masterpanel.Controls.Add(this.labReason);
            this.masterpanel.Controls.Add(this.disExVoucherID);
            this.masterpanel.Controls.Add(this.lbExVoucherID);
            this.masterpanel.Controls.Add(this.comboFactory1);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Controls.Add(this.txtVoucherDate);
            this.masterpanel.Controls.Add(this.btnAcctApprove);
            this.masterpanel.Controls.Add(this.btnShareExpense);
            this.masterpanel.Controls.Add(this.displayVoucherNo);
            this.masterpanel.Controls.Add(this.txtSserAccountant);
            this.masterpanel.Controls.Add(this.txtUserHandle);
            this.masterpanel.Controls.Add(this.labelVoucherNo);
            this.masterpanel.Controls.Add(this.labelApvDate);
            this.masterpanel.Controls.Add(this.labelAccountant);
            this.masterpanel.Controls.Add(this.labelHandle);
            this.masterpanel.Controls.Add(this.txtBLNo);
            this.masterpanel.Controls.Add(this.numTotal);
            this.masterpanel.Controls.Add(this.numVAT);
            this.masterpanel.Controls.Add(this.numVATRate);
            this.masterpanel.Controls.Add(this.numAmount);
            this.masterpanel.Controls.Add(this.labelBLNo);
            this.masterpanel.Controls.Add(this.labelTotal);
            this.masterpanel.Controls.Add(this.labelVAT);
            this.masterpanel.Controls.Add(this.labelVATRate);
            this.masterpanel.Controls.Add(this.labelAmount);
            this.masterpanel.Controls.Add(this.labelCurrency);
            this.masterpanel.Controls.Add(this.displayM);
            this.masterpanel.Controls.Add(this.labelM);
            this.masterpanel.Controls.Add(this.txtInvoice);
            this.masterpanel.Controls.Add(this.txtRemark);
            this.masterpanel.Controls.Add(this.txtpayterm_ftyTerms);
            this.masterpanel.Controls.Add(this.txtSubconSupplier);
            this.masterpanel.Controls.Add(this.comboType2);
            this.masterpanel.Controls.Add(this.comboType);
            this.masterpanel.Controls.Add(this.displayNo);
            this.masterpanel.Controls.Add(this.labelInvoice);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.labelTerms);
            this.masterpanel.Controls.Add(this.labelSupplier);
            this.masterpanel.Controls.Add(this.labelType);
            this.masterpanel.Controls.Add(this.labelDate);
            this.masterpanel.Controls.Add(this.labelNo);
            this.masterpanel.Controls.Add(this.dateApvDate);
            this.masterpanel.Controls.Add(this.dateDate);
            this.masterpanel.Size = new System.Drawing.Size(1000, 241);
            this.masterpanel.TabIndex = 28;
            this.masterpanel.Controls.SetChildIndex(this.dateDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateApvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelType, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSupplier, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTerms, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelInvoice, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboType, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboType2, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSubconSupplier, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtpayterm_ftyTerms, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtInvoice, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelM, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayM, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCurrency, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelAmount, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelVATRate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelVAT, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTotal, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelBLNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.numAmount, 0);
            this.masterpanel.Controls.SetChildIndex(this.numVATRate, 0);
            this.masterpanel.Controls.SetChildIndex(this.numVAT, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTotal, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtBLNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelAccountant, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelApvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelVoucherNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtUserHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSserAccountant, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayVoucherNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnShareExpense, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnAcctApprove, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtVoucherDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboFactory1, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbExVoucherID, 0);
            this.masterpanel.Controls.SetChildIndex(this.disExVoucherID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labReason, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtReason, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtReasonDesc, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbVesselName, 0);
            this.masterpanel.Controls.SetChildIndex(this.disVesselName, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.label4, 0);
            this.masterpanel.Controls.SetChildIndex(this.numericBoxShareAmtFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.numericBoxShareAmt, 0);
            this.masterpanel.Controls.SetChildIndex(this.label5, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.numericBox1, 0);
            this.masterpanel.Controls.SetChildIndex(this.chkIncludeFoundry, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbSisFtyAPID, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSisFtyAPID, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtcurrency, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnIncludeFoundryRatio, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkIsFreightForwarder, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImport, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 241);
            this.detailpanel.Size = new System.Drawing.Size(1000, 285);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(890, 203);
            this.gridicon.TabIndex = 28;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(910, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1000, 285);
            // 
            // detail2
            // 
            this.detail2.Location = new System.Drawing.Point(4, 27);
            this.detail2.Size = new System.Drawing.Size(892, 385);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(886, 339);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(886, 40);
            // 
            // detail
            // 
            this.detail.Location = new System.Drawing.Point(4, 27);
            this.detail.Size = new System.Drawing.Size(1000, 564);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1000, 526);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 526);
            this.detailbtm.Size = new System.Drawing.Size(1000, 38);
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(4, 27);
            this.browse.Size = new System.Drawing.Size(1000, 564);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1008, 595);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 24);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(502, 7);
            this.editby.Size = new System.Drawing.Size(350, 24);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(454, 13);
            // 
            // labelNo
            // 
            this.labelNo.Location = new System.Drawing.Point(4, 5);
            this.labelNo.Name = "labelNo";
            this.labelNo.Size = new System.Drawing.Size(90, 23);
            this.labelNo.TabIndex = 99;
            this.labelNo.Text = "No.";
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(407, 5);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(83, 23);
            this.labelDate.TabIndex = 99;
            this.labelDate.Text = "Date";
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(4, 31);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(90, 23);
            this.labelType.TabIndex = 99;
            this.labelType.Text = "Type";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(4, 56);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(90, 23);
            this.labelSupplier.TabIndex = 99;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelTerms
            // 
            this.labelTerms.Location = new System.Drawing.Point(4, 81);
            this.labelTerms.Name = "labelTerms";
            this.labelTerms.Size = new System.Drawing.Size(90, 23);
            this.labelTerms.TabIndex = 99;
            this.labelTerms.Text = "Terms";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(4, 206);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(90, 23);
            this.labelRemark.TabIndex = 99;
            this.labelRemark.Text = "Remark";
            // 
            // labelInvoice
            // 
            this.labelInvoice.Location = new System.Drawing.Point(4, 106);
            this.labelInvoice.Name = "labelInvoice";
            this.labelInvoice.Size = new System.Drawing.Size(90, 23);
            this.labelInvoice.TabIndex = 99;
            this.labelInvoice.Text = "Invoice#";
            // 
            // displayNo
            // 
            this.displayNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayNo.Location = new System.Drawing.Point(95, 5);
            this.displayNo.Name = "displayNo";
            this.displayNo.Size = new System.Drawing.Size(112, 24);
            this.displayNo.TabIndex = 0;
            // 
            // dateDate
            // 
            this.dateDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CDate", true));
            this.dateDate.Location = new System.Drawing.Point(491, 5);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(99, 24);
            this.dateDate.TabIndex = 13;
            // 
            // comboType
            // 
            this.comboType.BackColor = System.Drawing.Color.White;
            this.comboType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue2", this.mtbs, "Type", true));
            this.comboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType.FormattingEnabled = true;
            this.comboType.IsSupportUnselect = true;
            this.comboType.Location = new System.Drawing.Point(95, 30);
            this.comboType.Name = "comboType";
            this.comboType.OldText = "";
            this.comboType.Size = new System.Drawing.Size(100, 26);
            this.comboType.TabIndex = 2;
            this.comboType.SelectedIndexChanged += new System.EventHandler(this.ComboType_SelectedIndexChanged);
            // 
            // comboType2
            // 
            this.comboType2.BackColor = System.Drawing.Color.White;
            this.comboType2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType2.FormattingEnabled = true;
            this.comboType2.IsSupportUnselect = true;
            this.comboType2.Location = new System.Drawing.Point(198, 30);
            this.comboType2.Name = "comboType2";
            this.comboType2.OldText = "";
            this.comboType2.Size = new System.Drawing.Size(203, 26);
            this.comboType2.TabIndex = 3;
            this.comboType2.SelectedIndexChanged += new System.EventHandler(this.ComboType2_SelectedIndexChanged);
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(95, 206);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(308, 24);
            this.txtRemark.TabIndex = 12;
            // 
            // txtInvoice
            // 
            this.txtInvoice.BackColor = System.Drawing.Color.White;
            this.txtInvoice.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "InvNo", true));
            this.txtInvoice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvoice.Location = new System.Drawing.Point(95, 106);
            this.txtInvoice.Name = "txtInvoice";
            this.txtInvoice.Size = new System.Drawing.Size(185, 24);
            this.txtInvoice.TabIndex = 7;
            // 
            // labelCurrency
            // 
            this.labelCurrency.Location = new System.Drawing.Point(407, 30);
            this.labelCurrency.Name = "labelCurrency";
            this.labelCurrency.Size = new System.Drawing.Size(83, 23);
            this.labelCurrency.TabIndex = 99;
            this.labelCurrency.Text = "Currency";
            // 
            // labelAmount
            // 
            this.labelAmount.Location = new System.Drawing.Point(407, 56);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(83, 23);
            this.labelAmount.TabIndex = 99;
            this.labelAmount.Text = "Amount";
            // 
            // labelVATRate
            // 
            this.labelVATRate.Location = new System.Drawing.Point(407, 81);
            this.labelVATRate.Name = "labelVATRate";
            this.labelVATRate.Size = new System.Drawing.Size(83, 23);
            this.labelVATRate.TabIndex = 99;
            this.labelVATRate.Text = "VAT Rate(%)";
            // 
            // labelVAT
            // 
            this.labelVAT.Location = new System.Drawing.Point(407, 106);
            this.labelVAT.Name = "labelVAT";
            this.labelVAT.Size = new System.Drawing.Size(83, 23);
            this.labelVAT.TabIndex = 99;
            this.labelVAT.Text = "VAT";
            // 
            // labelTotal
            // 
            this.labelTotal.Location = new System.Drawing.Point(407, 131);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(83, 23);
            this.labelTotal.TabIndex = 99;
            this.labelTotal.Text = "Total";
            // 
            // labelBLNo
            // 
            this.labelBLNo.Location = new System.Drawing.Point(4, 131);
            this.labelBLNo.Name = "labelBLNo";
            this.labelBLNo.Size = new System.Drawing.Size(90, 23);
            this.labelBLNo.TabIndex = 99;
            this.labelBLNo.Text = "B/L No.";
            // 
            // numAmount
            // 
            this.numAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numAmount.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Amount", true));
            this.numAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numAmount.IsSupportEditMode = false;
            this.numAmount.Location = new System.Drawing.Point(491, 56);
            this.numAmount.Name = "numAmount";
            this.numAmount.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAmount.ReadOnly = true;
            this.numAmount.Size = new System.Drawing.Size(76, 24);
            this.numAmount.TabIndex = 15;
            this.numAmount.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numVATRate
            // 
            this.numVATRate.BackColor = System.Drawing.Color.White;
            this.numVATRate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "VATRate", true));
            this.numVATRate.DecimalPlaces = 1;
            this.numVATRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numVATRate.Location = new System.Drawing.Point(491, 81);
            this.numVATRate.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            65536});
            this.numVATRate.MaxLength = 5;
            this.numVATRate.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.numVATRate.Name = "numVATRate";
            this.numVATRate.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numVATRate.Size = new System.Drawing.Size(47, 24);
            this.numVATRate.TabIndex = 16;
            this.numVATRate.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numVAT
            // 
            this.numVAT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numVAT.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "VAT", true));
            this.numVAT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numVAT.IsSupportEditMode = false;
            this.numVAT.Location = new System.Drawing.Point(491, 106);
            this.numVAT.Name = "numVAT";
            this.numVAT.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numVAT.ReadOnly = true;
            this.numVAT.Size = new System.Drawing.Size(76, 24);
            this.numVAT.TabIndex = 17;
            this.numVAT.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numTotal
            // 
            this.numTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotal.IsSupportEditMode = false;
            this.numTotal.Location = new System.Drawing.Point(491, 131);
            this.numTotal.Name = "numTotal";
            this.numTotal.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotal.ReadOnly = true;
            this.numTotal.Size = new System.Drawing.Size(75, 24);
            this.numTotal.TabIndex = 18;
            this.numTotal.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtBLNo
            // 
            this.txtBLNo.BackColor = System.Drawing.Color.White;
            this.txtBLNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BLNo", true));
            this.txtBLNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBLNo.Location = new System.Drawing.Point(95, 131);
            this.txtBLNo.Name = "txtBLNo";
            this.txtBLNo.Size = new System.Drawing.Size(174, 24);
            this.txtBLNo.TabIndex = 8;
            this.txtBLNo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtBLNo_Validating);
            // 
            // labelHandle
            // 
            this.labelHandle.Location = new System.Drawing.Point(591, 106);
            this.labelHandle.Name = "labelHandle";
            this.labelHandle.Size = new System.Drawing.Size(106, 23);
            this.labelHandle.TabIndex = 99;
            this.labelHandle.Text = "Handle";
            // 
            // labelAccountant
            // 
            this.labelAccountant.Location = new System.Drawing.Point(591, 131);
            this.labelAccountant.Name = "labelAccountant";
            this.labelAccountant.Size = new System.Drawing.Size(106, 23);
            this.labelAccountant.TabIndex = 99;
            this.labelAccountant.Text = "Accountant";
            // 
            // labelApvDate
            // 
            this.labelApvDate.Location = new System.Drawing.Point(591, 5);
            this.labelApvDate.Name = "labelApvDate";
            this.labelApvDate.Size = new System.Drawing.Size(106, 23);
            this.labelApvDate.TabIndex = 99;
            this.labelApvDate.Text = "Apv Date";
            // 
            // labelVoucherNo
            // 
            this.labelVoucherNo.Location = new System.Drawing.Point(591, 31);
            this.labelVoucherNo.Name = "labelVoucherNo";
            this.labelVoucherNo.Size = new System.Drawing.Size(106, 23);
            this.labelVoucherNo.TabIndex = 99;
            this.labelVoucherNo.Text = "Voucher No.";
            // 
            // dateApvDate
            // 
            this.dateApvDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ApvDate", true));
            this.dateApvDate.IsSupportEditMode = false;
            this.dateApvDate.Location = new System.Drawing.Point(699, 5);
            this.dateApvDate.Name = "dateApvDate";
            this.dateApvDate.ReadOnly = true;
            this.dateApvDate.Size = new System.Drawing.Size(128, 24);
            this.dateApvDate.TabIndex = 22;
            // 
            // displayVoucherNo
            // 
            this.displayVoucherNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayVoucherNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "VoucherID", true));
            this.displayVoucherNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayVoucherNo.Location = new System.Drawing.Point(698, 31);
            this.displayVoucherNo.Name = "displayVoucherNo";
            this.displayVoucherNo.Size = new System.Drawing.Size(168, 24);
            this.displayVoucherNo.TabIndex = 23;
            // 
            // btnShareExpense
            // 
            this.btnShareExpense.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnShareExpense.Location = new System.Drawing.Point(866, 5);
            this.btnShareExpense.Name = "btnShareExpense";
            this.btnShareExpense.Size = new System.Drawing.Size(128, 30);
            this.btnShareExpense.TabIndex = 26;
            this.btnShareExpense.Text = "Share Expense";
            this.btnShareExpense.UseVisualStyleBackColor = true;
            this.btnShareExpense.Click += new System.EventHandler(this.BtnShareExpense_Click);
            // 
            // btnAcctApprove
            // 
            this.btnAcctApprove.Location = new System.Drawing.Point(866, 40);
            this.btnAcctApprove.Name = "btnAcctApprove";
            this.btnAcctApprove.Size = new System.Drawing.Size(128, 30);
            this.btnAcctApprove.TabIndex = 27;
            this.btnAcctApprove.Text = "Acct. Approve";
            this.btnAcctApprove.UseVisualStyleBackColor = true;
            this.btnAcctApprove.Click += new System.EventHandler(this.BtnAcctApprove_Click);
            // 
            // displayM
            // 
            this.displayM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayM.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MDivisionID", true));
            this.displayM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayM.Location = new System.Drawing.Point(227, 4);
            this.displayM.Name = "displayM";
            this.displayM.Size = new System.Drawing.Size(50, 24);
            this.displayM.TabIndex = 5;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(208, 5);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(18, 23);
            this.labelM.TabIndex = 99;
            this.labelM.Text = "M";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(279, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 23);
            this.label1.TabIndex = 99;
            this.label1.Text = "Factory";
            // 
            // comboFactory1
            // 
            this.comboFactory1.BackColor = System.Drawing.Color.White;
            this.comboFactory1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "FactoryID", true));
            this.comboFactory1.FilteMDivision = false;
            this.comboFactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory1.FormattingEnabled = true;
            this.comboFactory1.IssupportJunk = false;
            this.comboFactory1.IsSupportUnselect = true;
            this.comboFactory1.Location = new System.Drawing.Point(333, 5);
            this.comboFactory1.Name = "comboFactory1";
            this.comboFactory1.OldText = "";
            this.comboFactory1.Size = new System.Drawing.Size(66, 26);
            this.comboFactory1.TabIndex = 1;
            // 
            // txtSserAccountant
            // 
            this.txtSserAccountant.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Accountant", true));
            this.txtSserAccountant.DisplayBox1Binding = "";
            this.txtSserAccountant.Location = new System.Drawing.Point(698, 131);
            this.txtSserAccountant.Name = "txtSserAccountant";
            this.txtSserAccountant.Size = new System.Drawing.Size(295, 23);
            this.txtSserAccountant.TabIndex = 27;
            this.txtSserAccountant.TextBox1Binding = "";
            // 
            // txtUserHandle
            // 
            this.txtUserHandle.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Handle", true));
            this.txtUserHandle.DisplayBox1Binding = "";
            this.txtUserHandle.Location = new System.Drawing.Point(698, 106);
            this.txtUserHandle.Name = "txtUserHandle";
            this.txtUserHandle.Size = new System.Drawing.Size(295, 23);
            this.txtUserHandle.TabIndex = 26;
            this.txtUserHandle.TextBox1Binding = "";
            // 
            // txtpayterm_ftyTerms
            // 
            this.txtpayterm_ftyTerms.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "PayTermID", true));
            this.txtpayterm_ftyTerms.DisplayBox1Binding = "";
            this.txtpayterm_ftyTerms.Location = new System.Drawing.Point(95, 81);
            this.txtpayterm_ftyTerms.Name = "txtpayterm_ftyTerms";
            this.txtpayterm_ftyTerms.Size = new System.Drawing.Size(310, 23);
            this.txtpayterm_ftyTerms.TabIndex = 6;
            this.txtpayterm_ftyTerms.TextBox1Binding = "";
            // 
            // txtSubconSupplier
            // 
            this.txtSubconSupplier.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LocalSuppID", true));
            this.txtSubconSupplier.DisplayBox1Binding = "";
            this.txtSubconSupplier.IsIncludeJunk = false;
            this.txtSubconSupplier.IsMisc = false;
            this.txtSubconSupplier.IsShipping = true;
            this.txtSubconSupplier.IsSubcon = false;
            this.txtSubconSupplier.Location = new System.Drawing.Point(95, 56);
            this.txtSubconSupplier.Name = "txtSubconSupplier";
            this.txtSubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtSubconSupplier.TabIndex = 4;
            this.txtSubconSupplier.TextBox1Binding = "";
            this.txtSubconSupplier.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSubconSupplier_Validating);
            // 
            // lbExVoucherID
            // 
            this.lbExVoucherID.Location = new System.Drawing.Point(591, 81);
            this.lbExVoucherID.Name = "lbExVoucherID";
            this.lbExVoucherID.Size = new System.Drawing.Size(106, 23);
            this.lbExVoucherID.TabIndex = 99;
            this.lbExVoucherID.Text = "Ex Voucher No.";
            // 
            // disExVoucherID
            // 
            this.disExVoucherID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disExVoucherID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disExVoucherID.Location = new System.Drawing.Point(698, 81);
            this.disExVoucherID.Name = "disExVoucherID";
            this.disExVoucherID.Size = new System.Drawing.Size(148, 24);
            this.disExVoucherID.TabIndex = 25;
            // 
            // labReason
            // 
            this.labReason.Location = new System.Drawing.Point(4, 181);
            this.labReason.Name = "labReason";
            this.labReason.Size = new System.Drawing.Size(90, 23);
            this.labReason.TabIndex = 99;
            this.labReason.Text = "Reason";
            // 
            // txtReason
            // 
            this.txtReason.BackColor = System.Drawing.Color.White;
            this.txtReason.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Reason", true));
            this.txtReason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReason.Location = new System.Drawing.Point(95, 181);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(75, 24);
            this.txtReason.TabIndex = 10;
            this.txtReason.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtReason_PopUp);
            this.txtReason.Validating += new System.ComponentModel.CancelEventHandler(this.TxtReason_Validating);
            // 
            // txtReasonDesc
            // 
            this.txtReasonDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtReasonDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtReasonDesc.IsSupportEditMode = false;
            this.txtReasonDesc.Location = new System.Drawing.Point(173, 181);
            this.txtReasonDesc.Name = "txtReasonDesc";
            this.txtReasonDesc.ReadOnly = true;
            this.txtReasonDesc.Size = new System.Drawing.Size(228, 24);
            this.txtReasonDesc.TabIndex = 11;
            // 
            // lbVesselName
            // 
            this.lbVesselName.Location = new System.Drawing.Point(4, 156);
            this.lbVesselName.Name = "lbVesselName";
            this.lbVesselName.Size = new System.Drawing.Size(90, 23);
            this.lbVesselName.TabIndex = 99;
            this.lbVesselName.Text = "Vessel Name";
            // 
            // disVesselName
            // 
            this.disVesselName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disVesselName.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "VoucherID", true));
            this.disVesselName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disVesselName.Location = new System.Drawing.Point(95, 156);
            this.disVesselName.Name = "disVesselName";
            this.disVesselName.Size = new System.Drawing.Size(174, 24);
            this.disVesselName.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(591, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 23);
            this.label2.TabIndex = 99;
            this.label2.Text = "Voucher Date";
            // 
            // txtVoucherDate
            // 
            this.txtVoucherDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "VoucherDate", true));
            this.txtVoucherDate.IsSupportEditMode = false;
            this.txtVoucherDate.Location = new System.Drawing.Point(699, 56);
            this.txtVoucherDate.Name = "txtVoucherDate";
            this.txtVoucherDate.ReadOnly = true;
            this.txtVoucherDate.Size = new System.Drawing.Size(130, 24);
            this.txtVoucherDate.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(407, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 23);
            this.label3.TabIndex = 100;
            this.label3.Text = "Shared Amt - Factory";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(407, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 23);
            this.label4.TabIndex = 101;
            this.label4.Text = "Shared Amt - Other";
            // 
            // numericBoxShareAmtFactory
            // 
            this.numericBoxShareAmtFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numericBoxShareAmtFactory.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SharedAmtFactory", true));
            this.numericBoxShareAmtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numericBoxShareAmtFactory.IsSupportEditMode = false;
            this.numericBoxShareAmtFactory.Location = new System.Drawing.Point(544, 156);
            this.numericBoxShareAmtFactory.Name = "numericBoxShareAmtFactory";
            this.numericBoxShareAmtFactory.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBoxShareAmtFactory.ReadOnly = true;
            this.numericBoxShareAmtFactory.Size = new System.Drawing.Size(92, 24);
            this.numericBoxShareAmtFactory.TabIndex = 19;
            this.numericBoxShareAmtFactory.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numericBoxShareAmt
            // 
            this.numericBoxShareAmt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numericBoxShareAmt.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SharedAmtOther", true));
            this.numericBoxShareAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numericBoxShareAmt.IsSupportEditMode = false;
            this.numericBoxShareAmt.Location = new System.Drawing.Point(544, 181);
            this.numericBoxShareAmt.Name = "numericBoxShareAmt";
            this.numericBoxShareAmt.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBoxShareAmt.ReadOnly = true;
            this.numericBoxShareAmt.Size = new System.Drawing.Size(92, 24);
            this.numericBoxShareAmt.TabIndex = 20;
            this.numericBoxShareAmt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(407, 206);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 23);
            this.label5.TabIndex = 102;
            this.label5.Text = "APP Exchage Rate";
            // 
            // numericBox1
            // 
            this.numericBox1.BackColor = System.Drawing.Color.White;
            this.numericBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "APPExchageRate", true));
            this.numericBox1.DecimalPlaces = 6;
            this.numericBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox1.Location = new System.Drawing.Point(544, 206);
            this.numericBox1.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            393216});
            this.numericBox1.MaxLength = 11;
            this.numericBox1.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox1.Size = new System.Drawing.Size(92, 24);
            this.numericBox1.TabIndex = 21;
            this.numericBox1.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // chkIncludeFoundry
            // 
            this.chkIncludeFoundry.AutoSize = true;
            this.chkIncludeFoundry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.chkIncludeFoundry.IsSupportEditMode = false;
            this.chkIncludeFoundry.Location = new System.Drawing.Point(642, 160);
            this.chkIncludeFoundry.Name = "chkIncludeFoundry";
            this.chkIncludeFoundry.ReadOnly = true;
            this.chkIncludeFoundry.Size = new System.Drawing.Size(131, 22);
            this.chkIncludeFoundry.TabIndex = 103;
            this.chkIncludeFoundry.Text = "Include Foundry";
            this.chkIncludeFoundry.UseVisualStyleBackColor = true;
            // 
            // lbSisFtyAPID
            // 
            this.lbSisFtyAPID.Location = new System.Drawing.Point(642, 181);
            this.lbSisFtyAPID.Name = "lbSisFtyAPID";
            this.lbSisFtyAPID.Size = new System.Drawing.Size(83, 23);
            this.lbSisFtyAPID.TabIndex = 104;
            this.lbSisFtyAPID.Text = "Sis. Fty A/P#";
            // 
            // txtSisFtyAPID
            // 
            this.txtSisFtyAPID.BackColor = System.Drawing.Color.White;
            this.txtSisFtyAPID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SisFtyAPID", true));
            this.txtSisFtyAPID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSisFtyAPID.Location = new System.Drawing.Point(728, 181);
            this.txtSisFtyAPID.Name = "txtSisFtyAPID";
            this.txtSisFtyAPID.Size = new System.Drawing.Size(100, 24);
            this.txtSisFtyAPID.TabIndex = 105;
            // 
            // txtcurrency
            // 
            this.txtcurrency.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtcurrency.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CurrencyID", true));
            this.txtcurrency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtcurrency.IsSupportEditMode = false;
            this.txtcurrency.Location = new System.Drawing.Point(493, 30);
            this.txtcurrency.Name = "txtcurrency";
            this.txtcurrency.ReadOnly = true;
            this.txtcurrency.Size = new System.Drawing.Size(48, 24);
            this.txtcurrency.TabIndex = 14;
            // 
            // btnIncludeFoundryRatio
            // 
            this.btnIncludeFoundryRatio.Location = new System.Drawing.Point(776, 154);
            this.btnIncludeFoundryRatio.Name = "btnIncludeFoundryRatio";
            this.btnIncludeFoundryRatio.Size = new System.Drawing.Size(29, 26);
            this.btnIncludeFoundryRatio.TabIndex = 106;
            this.btnIncludeFoundryRatio.Text = "...";
            this.btnIncludeFoundryRatio.UseVisualStyleBackColor = true;
            this.btnIncludeFoundryRatio.Click += new System.EventHandler(this.BtnIncludeFoundryRatio_Click);
            // 
            // checkIsFreightForwarder
            // 
            this.checkIsFreightForwarder.AutoSize = true;
            this.checkIsFreightForwarder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkIsFreightForwarder.IsSupportEditMode = false;
            this.checkIsFreightForwarder.Location = new System.Drawing.Point(266, 56);
            this.checkIsFreightForwarder.Name = "checkIsFreightForwarder";
            this.checkIsFreightForwarder.ReadOnly = true;
            this.checkIsFreightForwarder.Size = new System.Drawing.Size(119, 22);
            this.checkIsFreightForwarder.TabIndex = 107;
            this.checkIsFreightForwarder.Text = "Is Freight Fwd";
            this.checkIsFreightForwarder.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(890, 167);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 108;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // P08
            // 
            this.ClientSize = new System.Drawing.Size(1008, 628);
            this.DefaultControl = "comboType";
            this.DefaultControlForEdit = "comboType";
            this.DefaultDetailOrder = "ID,ShipExpenseID";
            this.DefaultOrder = "CDate,ID";
            this.GridAlias = "ShippingAP_Detail";
            this.GridNew = 0;
            this.GridUniqueKey = "ID,ShipExpenseID,Price";
            this.IsSupportCopy = false;
            this.KeyField1 = "ID";
            this.Name = "P08";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P08. Account Payment - Shipping";
            this.UniqueExpress = "ID";
            this.WorkAlias = "ShippingAP";
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

        private Win.UI.Button btnAcctApprove;
        private Win.UI.Button btnShareExpense;
        private Win.UI.DisplayBox displayVoucherNo;
        private Win.UI.DateBox dateApvDate;
        private Class.Txtuser txtSserAccountant;
        private Class.Txtuser txtUserHandle;
        private Win.UI.Label labelVoucherNo;
        private Win.UI.Label labelApvDate;
        private Win.UI.Label labelAccountant;
        private Win.UI.Label labelHandle;
        private Win.UI.TextBox txtBLNo;
        private Win.UI.NumericBox numTotal;
        private Win.UI.NumericBox numVAT;
        private Win.UI.NumericBox numVATRate;
        private Win.UI.NumericBox numAmount;
        private Win.UI.Label labelBLNo;
        private Win.UI.Label labelTotal;
        private Win.UI.Label labelVAT;
        private Win.UI.Label labelVATRate;
        private Win.UI.Label labelAmount;
        private Win.UI.Label labelCurrency;
        private Win.UI.TextBox txtInvoice;
        private Win.UI.TextBox txtRemark;
        private Class.Txtpayterm_fty txtpayterm_ftyTerms;
        private Class.TxtsubconNoConfirm txtSubconSupplier;
        private Win.UI.ComboBox comboType2;
        private Win.UI.ComboBox comboType;
        private Win.UI.DateBox dateDate;
        private Win.UI.DisplayBox displayNo;
        private Win.UI.Label labelInvoice;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelTerms;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelType;
        private Win.UI.Label labelDate;
        private Win.UI.Label labelNo;
        private Win.UI.DisplayBox displayM;
        private Win.UI.Label labelM;
        private Class.ComboFactory comboFactory1;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox disExVoucherID;
        private Win.UI.Label lbExVoucherID;
        private Win.UI.Label labReason;
        private Win.UI.TextBox txtReasonDesc;
        private Win.UI.TextBox txtReason;
        private Win.UI.DisplayBox disVesselName;
        private Win.UI.Label lbVesselName;
        private Win.UI.DateBox txtVoucherDate;
        private Win.UI.Label label2;
        private Win.UI.NumericBox numericBoxShareAmt;
        private Win.UI.NumericBox numericBoxShareAmtFactory;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.NumericBox numericBox1;
        private Win.UI.Label label5;
        private Win.UI.CheckBox chkIncludeFoundry;
        private Win.UI.TextBox txtSisFtyAPID;
        private Win.UI.Label lbSisFtyAPID;
        private Class.Txtcurrency txtcurrency;
        private Win.UI.Button btnIncludeFoundryRatio;
        private Win.UI.CheckBox checkIsFreightForwarder;
        private Win.UI.Button btnImport;
    }
}
