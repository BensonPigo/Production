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
            this.displayCurrency = new Sci.Win.UI.DisplayBox();
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
            this.comboFactory1 = new Sci.Production.Class.comboFactory(this.components);
            this.txtSserAccountant = new Sci.Production.Class.txtuser();
            this.txtUserHandle = new Sci.Production.Class.txtuser();
            this.txtpayterm_ftyTerms = new Sci.Production.Class.txtpayterm_fty();
            this.txtSubconSupplier = new Sci.Production.Class.txtsubcon();
            this.lbExVoucherID = new Sci.Win.UI.Label();
            this.disExVoucherID = new Sci.Win.UI.DisplayBox();
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
            this.masterpanel.Controls.Add(this.disExVoucherID);
            this.masterpanel.Controls.Add(this.lbExVoucherID);
            this.masterpanel.Controls.Add(this.comboFactory1);
            this.masterpanel.Controls.Add(this.label1);
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
            this.masterpanel.Controls.Add(this.displayCurrency);
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
            this.masterpanel.Size = new System.Drawing.Size(1006, 183);
            this.masterpanel.Paint += new System.Windows.Forms.PaintEventHandler(this.Masterpanel_Paint);
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
            this.masterpanel.Controls.SetChildIndex(this.displayCurrency, 0);
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
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboFactory1, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbExVoucherID, 0);
            this.masterpanel.Controls.SetChildIndex(this.disExVoucherID, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 183);
            this.detailpanel.Size = new System.Drawing.Size(1006, 246);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(770, 148);
            this.gridicon.TabIndex = 11;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(910, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1006, 246);
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
            this.detail.Size = new System.Drawing.Size(1006, 467);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1006, 429);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 429);
            this.detailbtm.Size = new System.Drawing.Size(1006, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(990, 467);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1014, 496);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(502, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(454, 13);
            // 
            // labelNo
            // 
            this.labelNo.Location = new System.Drawing.Point(4, 6);
            this.labelNo.Name = "labelNo";
            this.labelNo.Size = new System.Drawing.Size(58, 23);
            this.labelNo.TabIndex = 12;
            this.labelNo.Text = "No.";
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(314, 6);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(52, 23);
            this.labelDate.TabIndex = 18;
            this.labelDate.Text = "Date";
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(5, 35);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(57, 23);
            this.labelType.TabIndex = 13;
            this.labelType.Text = "Type";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(5, 64);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(57, 23);
            this.labelSupplier.TabIndex = 14;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelTerms
            // 
            this.labelTerms.Location = new System.Drawing.Point(5, 93);
            this.labelTerms.Name = "labelTerms";
            this.labelTerms.Size = new System.Drawing.Size(57, 23);
            this.labelTerms.TabIndex = 15;
            this.labelTerms.Text = "Terms";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(5, 122);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(57, 23);
            this.labelRemark.TabIndex = 16;
            this.labelRemark.Text = "Remark";
            // 
            // labelInvoice
            // 
            this.labelInvoice.Location = new System.Drawing.Point(5, 151);
            this.labelInvoice.Name = "labelInvoice";
            this.labelInvoice.Size = new System.Drawing.Size(57, 23);
            this.labelInvoice.TabIndex = 17;
            this.labelInvoice.Text = "Invoice#";
            // 
            // displayNo
            // 
            this.displayNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayNo.Location = new System.Drawing.Point(64, 6);
            this.displayNo.Name = "displayNo";
            this.displayNo.Size = new System.Drawing.Size(112, 23);
            this.displayNo.TabIndex = 20;
            // 
            // dateDate
            // 
            this.dateDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CDate", true));
            this.dateDate.Location = new System.Drawing.Point(368, 6);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(99, 23);
            this.dateDate.TabIndex = 6;
            // 
            // comboType
            // 
            this.comboType.BackColor = System.Drawing.Color.White;
            this.comboType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue2", this.mtbs, "Type", true));
            this.comboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType.FormattingEnabled = true;
            this.comboType.IsSupportUnselect = true;
            this.comboType.Location = new System.Drawing.Point(64, 33);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(100, 24);
            this.comboType.TabIndex = 0;
            this.comboType.SelectedIndexChanged += new System.EventHandler(this.ComboType_SelectedIndexChanged);
            // 
            // comboType2
            // 
            this.comboType2.BackColor = System.Drawing.Color.White;
            this.comboType2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType2.FormattingEnabled = true;
            this.comboType2.IsSupportUnselect = true;
            this.comboType2.Location = new System.Drawing.Point(169, 33);
            this.comboType2.Name = "comboType2";
            this.comboType2.Size = new System.Drawing.Size(203, 24);
            this.comboType2.TabIndex = 1;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(64, 122);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(311, 23);
            this.txtRemark.TabIndex = 4;
            // 
            // txtInvoice
            // 
            this.txtInvoice.BackColor = System.Drawing.Color.White;
            this.txtInvoice.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "InvNo", true));
            this.txtInvoice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvoice.Location = new System.Drawing.Point(64, 150);
            this.txtInvoice.Name = "txtInvoice";
            this.txtInvoice.Size = new System.Drawing.Size(185, 23);
            this.txtInvoice.TabIndex = 5;
            // 
            // labelCurrency
            // 
            this.labelCurrency.Location = new System.Drawing.Point(471, 6);
            this.labelCurrency.Name = "labelCurrency";
            this.labelCurrency.Size = new System.Drawing.Size(60, 23);
            this.labelCurrency.TabIndex = 18;
            this.labelCurrency.Text = "Currency";
            // 
            // labelAmount
            // 
            this.labelAmount.Location = new System.Drawing.Point(471, 35);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(60, 23);
            this.labelAmount.TabIndex = 19;
            this.labelAmount.Text = "Amount";
            // 
            // labelVATRate
            // 
            this.labelVATRate.Location = new System.Drawing.Point(471, 64);
            this.labelVATRate.Name = "labelVATRate";
            this.labelVATRate.Size = new System.Drawing.Size(87, 23);
            this.labelVATRate.TabIndex = 20;
            this.labelVATRate.Text = "VAT Rate(%)";
            // 
            // labelVAT
            // 
            this.labelVAT.Location = new System.Drawing.Point(471, 93);
            this.labelVAT.Name = "labelVAT";
            this.labelVAT.Size = new System.Drawing.Size(60, 23);
            this.labelVAT.TabIndex = 21;
            this.labelVAT.Text = "VAT";
            // 
            // labelTotal
            // 
            this.labelTotal.Location = new System.Drawing.Point(471, 122);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(60, 23);
            this.labelTotal.TabIndex = 22;
            this.labelTotal.Text = "Total";
            // 
            // labelBLNo
            // 
            this.labelBLNo.Location = new System.Drawing.Point(471, 151);
            this.labelBLNo.Name = "labelBLNo";
            this.labelBLNo.Size = new System.Drawing.Size(60, 23);
            this.labelBLNo.TabIndex = 23;
            this.labelBLNo.Text = "B/L No.";
            // 
            // displayCurrency
            // 
            this.displayCurrency.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCurrency.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CurrencyID", true));
            this.displayCurrency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCurrency.Location = new System.Drawing.Point(532, 6);
            this.displayCurrency.Name = "displayCurrency";
            this.displayCurrency.Size = new System.Drawing.Size(50, 23);
            this.displayCurrency.TabIndex = 24;
            // 
            // numAmount
            // 
            this.numAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numAmount.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Amount", true));
            this.numAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numAmount.IsSupportEditMode = false;
            this.numAmount.Location = new System.Drawing.Point(532, 33);
            this.numAmount.Name = "numAmount";
            this.numAmount.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAmount.ReadOnly = true;
            this.numAmount.Size = new System.Drawing.Size(76, 23);
            this.numAmount.TabIndex = 25;
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
            this.numVATRate.Location = new System.Drawing.Point(561, 64);
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
            this.numVATRate.Size = new System.Drawing.Size(47, 23);
            this.numVATRate.TabIndex = 7;
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
            this.numVAT.Location = new System.Drawing.Point(532, 93);
            this.numVAT.Name = "numVAT";
            this.numVAT.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numVAT.ReadOnly = true;
            this.numVAT.Size = new System.Drawing.Size(76, 23);
            this.numVAT.TabIndex = 27;
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
            this.numTotal.Location = new System.Drawing.Point(532, 122);
            this.numTotal.Name = "numTotal";
            this.numTotal.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotal.ReadOnly = true;
            this.numTotal.Size = new System.Drawing.Size(75, 23);
            this.numTotal.TabIndex = 28;
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
            this.txtBLNo.Location = new System.Drawing.Point(532, 151);
            this.txtBLNo.Name = "txtBLNo";
            this.txtBLNo.Size = new System.Drawing.Size(185, 23);
            this.txtBLNo.TabIndex = 8;
            // 
            // labelHandle
            // 
            this.labelHandle.Location = new System.Drawing.Point(635, 6);
            this.labelHandle.Name = "labelHandle";
            this.labelHandle.Size = new System.Drawing.Size(80, 23);
            this.labelHandle.TabIndex = 30;
            this.labelHandle.Text = "Handle";
            // 
            // labelAccountant
            // 
            this.labelAccountant.Location = new System.Drawing.Point(635, 35);
            this.labelAccountant.Name = "labelAccountant";
            this.labelAccountant.Size = new System.Drawing.Size(80, 23);
            this.labelAccountant.TabIndex = 31;
            this.labelAccountant.Text = "Accountant";
            // 
            // labelApvDate
            // 
            this.labelApvDate.Location = new System.Drawing.Point(635, 64);
            this.labelApvDate.Name = "labelApvDate";
            this.labelApvDate.Size = new System.Drawing.Size(80, 23);
            this.labelApvDate.TabIndex = 32;
            this.labelApvDate.Text = "Apv Date";
            // 
            // labelVoucherNo
            // 
            this.labelVoucherNo.Location = new System.Drawing.Point(635, 93);
            this.labelVoucherNo.Name = "labelVoucherNo";
            this.labelVoucherNo.Size = new System.Drawing.Size(80, 23);
            this.labelVoucherNo.TabIndex = 33;
            this.labelVoucherNo.Text = "Voucher No.";
            // 
            // dateApvDate
            // 
            this.dateApvDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ApvDate", true));
            this.dateApvDate.IsSupportEditMode = false;
            this.dateApvDate.Location = new System.Drawing.Point(717, 64);
            this.dateApvDate.Name = "dateApvDate";
            this.dateApvDate.ReadOnly = true;
            this.dateApvDate.Size = new System.Drawing.Size(130, 23);
            this.dateApvDate.TabIndex = 36;
            // 
            // displayVoucherNo
            // 
            this.displayVoucherNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayVoucherNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "VoucherID", true));
            this.displayVoucherNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayVoucherNo.Location = new System.Drawing.Point(717, 93);
            this.displayVoucherNo.Name = "displayVoucherNo";
            this.displayVoucherNo.Size = new System.Drawing.Size(148, 23);
            this.displayVoucherNo.TabIndex = 37;
            // 
            // btnShareExpense
            // 
            this.btnShareExpense.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnShareExpense.Location = new System.Drawing.Point(876, 116);
            this.btnShareExpense.Name = "btnShareExpense";
            this.btnShareExpense.Size = new System.Drawing.Size(128, 30);
            this.btnShareExpense.TabIndex = 38;
            this.btnShareExpense.Text = "Share Expense";
            this.btnShareExpense.UseVisualStyleBackColor = true;
            this.btnShareExpense.Click += new System.EventHandler(this.BtnShareExpense_Click);
            // 
            // btnAcctApprove
            // 
            this.btnAcctApprove.Location = new System.Drawing.Point(876, 149);
            this.btnAcctApprove.Name = "btnAcctApprove";
            this.btnAcctApprove.Size = new System.Drawing.Size(128, 30);
            this.btnAcctApprove.TabIndex = 39;
            this.btnAcctApprove.Text = "Acct. Approve";
            this.btnAcctApprove.UseVisualStyleBackColor = true;
            this.btnAcctApprove.Click += new System.EventHandler(this.BtnAcctApprove_Click);
            // 
            // displayM
            // 
            this.displayM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayM.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MDivisionID", true));
            this.displayM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayM.Location = new System.Drawing.Point(273, 64);
            this.displayM.Name = "displayM";
            this.displayM.Size = new System.Drawing.Size(72, 23);
            this.displayM.TabIndex = 17;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(249, 64);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(21, 23);
            this.labelM.TabIndex = 19;
            this.labelM.Text = "M";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(178, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 23);
            this.label1.TabIndex = 40;
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
            this.comboFactory1.Location = new System.Drawing.Point(242, 6);
            this.comboFactory1.Name = "comboFactory1";
            this.comboFactory1.Size = new System.Drawing.Size(66, 24);
            this.comboFactory1.TabIndex = 41;
            // 
            // txtSserAccountant
            // 
            this.txtSserAccountant.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Accountant", true));
            this.txtSserAccountant.DisplayBox1Binding = "";
            this.txtSserAccountant.Location = new System.Drawing.Point(717, 35);
            this.txtSserAccountant.Name = "txtSserAccountant";
            this.txtSserAccountant.Size = new System.Drawing.Size(285, 23);
            this.txtSserAccountant.TabIndex = 10;
            this.txtSserAccountant.TextBox1Binding = "";
            // 
            // txtUserHandle
            // 
            this.txtUserHandle.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Handle", true));
            this.txtUserHandle.DisplayBox1Binding = "";
            this.txtUserHandle.Location = new System.Drawing.Point(717, 6);
            this.txtUserHandle.Name = "txtUserHandle";
            this.txtUserHandle.Size = new System.Drawing.Size(285, 23);
            this.txtUserHandle.TabIndex = 9;
            this.txtUserHandle.TextBox1Binding = "";
            // 
            // txtpayterm_ftyTerms
            // 
            this.txtpayterm_ftyTerms.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "PayTermID", true));
            this.txtpayterm_ftyTerms.DisplayBox1Binding = "";
            this.txtpayterm_ftyTerms.Location = new System.Drawing.Point(64, 93);
            this.txtpayterm_ftyTerms.Name = "txtpayterm_ftyTerms";
            this.txtpayterm_ftyTerms.Size = new System.Drawing.Size(310, 23);
            this.txtpayterm_ftyTerms.TabIndex = 3;
            this.txtpayterm_ftyTerms.TextBox1Binding = "";
            // 
            // txtSubconSupplier
            // 
            this.txtSubconSupplier.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LocalSuppID", true));
            this.txtSubconSupplier.DisplayBox1Binding = "";
            this.txtSubconSupplier.IsIncludeJunk = false;
            this.txtSubconSupplier.Location = new System.Drawing.Point(64, 64);
            this.txtSubconSupplier.Name = "txtSubconSupplier";
            this.txtSubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtSubconSupplier.TabIndex = 2;
            this.txtSubconSupplier.TextBox1Binding = "";
            // 
            // lbExVoucherID
            // 
            this.lbExVoucherID.Location = new System.Drawing.Point(615, 122);
            this.lbExVoucherID.Name = "lbExVoucherID";
            this.lbExVoucherID.Size = new System.Drawing.Size(102, 23);
            this.lbExVoucherID.TabIndex = 42;
            this.lbExVoucherID.Text = "Ex Voucher No.";
            // 
            // disExVoucherID
            // 
            this.disExVoucherID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disExVoucherID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disExVoucherID.Location = new System.Drawing.Point(717, 122);
            this.disExVoucherID.Name = "disExVoucherID";
            this.disExVoucherID.Size = new System.Drawing.Size(148, 23);
            this.disExVoucherID.TabIndex = 43;
            // 
            // P08
            // 
            this.ClientSize = new System.Drawing.Size(1014, 529);
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
        private Class.txtuser txtSserAccountant;
        private Class.txtuser txtUserHandle;
        private Win.UI.Label labelVoucherNo;
        private Win.UI.Label labelApvDate;
        private Win.UI.Label labelAccountant;
        private Win.UI.Label labelHandle;
        private Win.UI.TextBox txtBLNo;
        private Win.UI.NumericBox numTotal;
        private Win.UI.NumericBox numVAT;
        private Win.UI.NumericBox numVATRate;
        private Win.UI.NumericBox numAmount;
        private Win.UI.DisplayBox displayCurrency;
        private Win.UI.Label labelBLNo;
        private Win.UI.Label labelTotal;
        private Win.UI.Label labelVAT;
        private Win.UI.Label labelVATRate;
        private Win.UI.Label labelAmount;
        private Win.UI.Label labelCurrency;
        private Win.UI.TextBox txtInvoice;
        private Win.UI.TextBox txtRemark;
        private Class.txtpayterm_fty txtpayterm_ftyTerms;
        private Class.txtsubcon txtSubconSupplier;
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
        private Class.comboFactory comboFactory1;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox disExVoucherID;
        private Win.UI.Label lbExVoucherID;
    }
}
