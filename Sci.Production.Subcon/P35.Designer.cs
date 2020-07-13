namespace Sci.Production.Subcon
{
    partial class P35
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
            this.labelID = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.labelAmount = new Sci.Win.UI.Label();
            this.labelAccountant = new Sci.Win.UI.Label();
            this.labelVat = new Sci.Win.UI.Label();
            this.labelApvDate = new Sci.Win.UI.Label();
            this.labelCurrency = new Sci.Win.UI.Label();
            this.labelTotal = new Sci.Win.UI.Label();
            this.labelVatRate = new Sci.Win.UI.Label();
            this.labelHandle = new Sci.Win.UI.Label();
            this.label25 = new Sci.Win.UI.Label();
            this.btnBatchImport = new Sci.Win.UI.Button();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
            this.displayCurrency = new Sci.Win.UI.DisplayBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.numVatRate = new Sci.Win.UI.NumericBox();
            this.numVat = new Sci.Win.UI.NumericBox();
            this.numAmount = new Sci.Win.UI.NumericBox();
            this.numTotal = new Sci.Win.UI.NumericBox();
            this.labelTerms = new Sci.Win.UI.Label();
            this.txtInvoice = new Sci.Win.UI.TextBox();
            this.labelInvoice = new Sci.Win.UI.Label();
            this.displayVoucher = new Sci.Win.UI.DisplayBox();
            this.labelVoucher = new Sci.Win.UI.Label();
            this.dateApvDate = new Sci.Win.UI.DateBox();
            this.txtmfactory = new Sci.Production.Class.Txtfactory();
            this.txtpayterm_ftyTerms = new Sci.Production.Class.Txtpayterm_fty();
            this.txtuserAccountant = new Sci.Production.Class.Txtuser();
            this.txtuserHandle = new Sci.Production.Class.Txtuser();
            this.txtartworktype_ftyCategory = new Sci.Production.Class.Txtartworktype_fty();
            this.txtsubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.labelTotalQty = new Sci.Win.UI.Label();
            this.numTotalqty = new Sci.Win.UI.NumericBox();
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
            this.masterpanel.Controls.Add(this.numTotalqty);
            this.masterpanel.Controls.Add(this.labelTotalQty);
            this.masterpanel.Controls.Add(this.txtmfactory);
            this.masterpanel.Controls.Add(this.displayVoucher);
            this.masterpanel.Controls.Add(this.labelVoucher);
            this.masterpanel.Controls.Add(this.txtInvoice);
            this.masterpanel.Controls.Add(this.labelInvoice);
            this.masterpanel.Controls.Add(this.txtpayterm_ftyTerms);
            this.masterpanel.Controls.Add(this.labelTerms);
            this.masterpanel.Controls.Add(this.numTotal);
            this.masterpanel.Controls.Add(this.numAmount);
            this.masterpanel.Controls.Add(this.numVat);
            this.masterpanel.Controls.Add(this.txtuserAccountant);
            this.masterpanel.Controls.Add(this.txtuserHandle);
            this.masterpanel.Controls.Add(this.numVatRate);
            this.masterpanel.Controls.Add(this.txtRemark);
            this.masterpanel.Controls.Add(this.txtartworktype_ftyCategory);
            this.masterpanel.Controls.Add(this.displayCurrency);
            this.masterpanel.Controls.Add(this.txtsubconSupplier);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.btnBatchImport);
            this.masterpanel.Controls.Add(this.label25);
            this.masterpanel.Controls.Add(this.labelHandle);
            this.masterpanel.Controls.Add(this.labelVatRate);
            this.masterpanel.Controls.Add(this.labelTotal);
            this.masterpanel.Controls.Add(this.labelCurrency);
            this.masterpanel.Controls.Add(this.labelApvDate);
            this.masterpanel.Controls.Add(this.labelVat);
            this.masterpanel.Controls.Add(this.labelAccountant);
            this.masterpanel.Controls.Add(this.labelAmount);
            this.masterpanel.Controls.Add(this.labelIssueDate);
            this.masterpanel.Controls.Add(this.labelCategory);
            this.masterpanel.Controls.Add(this.labelSupplier);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.labelFactory);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Controls.Add(this.dateApvDate);
            this.masterpanel.Controls.Add(this.dateIssueDate);
            this.masterpanel.Size = new System.Drawing.Size(1000, 258);
            this.masterpanel.TabIndex = 1;
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateApvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSupplier, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCategory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelAmount, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelAccountant, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelVat, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelApvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCurrency, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTotal, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelVatRate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.label25, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnBatchImport, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtsubconSupplier, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayCurrency, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtartworktype_ftyCategory, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.numVatRate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserAccountant, 0);
            this.masterpanel.Controls.SetChildIndex(this.numVat, 0);
            this.masterpanel.Controls.SetChildIndex(this.numAmount, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTotal, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTerms, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtpayterm_ftyTerms, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelInvoice, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtInvoice, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelVoucher, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayVoucher, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtmfactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTotalQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTotalqty, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbExVoucherID, 0);
            this.masterpanel.Controls.SetChildIndex(this.disExVoucherID, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 258);
            this.detailpanel.Size = new System.Drawing.Size(1000, 219);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.gridicon.Location = new System.Drawing.Point(896, 210);
            this.gridicon.TabIndex = 11;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(913, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1000, 219);
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
            this.detail.Size = new System.Drawing.Size(1000, 515);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1000, 477);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 477);
            this.detailbtm.Size = new System.Drawing.Size(1000, 38);
            this.detailbtm.TabIndex = 0;
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1000, 515);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1008, 544);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(485, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(437, 13);
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(8, 15);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 1;
            this.labelID.Text = "ID";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(256, 49);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(75, 23);
            this.labelFactory.TabIndex = 4;
            this.labelFactory.Text = "Factory";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(8, 154);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 5;
            this.labelRemark.Text = "Remark";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(8, 84);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(75, 23);
            this.labelSupplier.TabIndex = 6;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(8, 49);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(75, 23);
            this.labelCategory.TabIndex = 7;
            this.labelCategory.Text = "Category";
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(256, 15);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.Size = new System.Drawing.Size(75, 23);
            this.labelIssueDate.TabIndex = 9;
            this.labelIssueDate.Text = "Issue Date";
            // 
            // labelAmount
            // 
            this.labelAmount.Location = new System.Drawing.Point(695, 15);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(75, 23);
            this.labelAmount.TabIndex = 10;
            this.labelAmount.Text = "Amount";
            // 
            // labelAccountant
            // 
            this.labelAccountant.Location = new System.Drawing.Point(483, 154);
            this.labelAccountant.Name = "labelAccountant";
            this.labelAccountant.Size = new System.Drawing.Size(95, 23);
            this.labelAccountant.TabIndex = 11;
            this.labelAccountant.Text = "Accountant";
            // 
            // labelVat
            // 
            this.labelVat.Location = new System.Drawing.Point(695, 49);
            this.labelVat.Name = "labelVat";
            this.labelVat.Size = new System.Drawing.Size(75, 23);
            this.labelVat.TabIndex = 13;
            this.labelVat.Text = "Vat";
            // 
            // labelApvDate
            // 
            this.labelApvDate.Location = new System.Drawing.Point(483, 189);
            this.labelApvDate.Name = "labelApvDate";
            this.labelApvDate.Size = new System.Drawing.Size(95, 23);
            this.labelApvDate.TabIndex = 14;
            this.labelApvDate.Text = "Apv. Date";
            // 
            // labelCurrency
            // 
            this.labelCurrency.Location = new System.Drawing.Point(483, 15);
            this.labelCurrency.Name = "labelCurrency";
            this.labelCurrency.Size = new System.Drawing.Size(95, 23);
            this.labelCurrency.TabIndex = 15;
            this.labelCurrency.Text = "Currency";
            // 
            // labelTotal
            // 
            this.labelTotal.Location = new System.Drawing.Point(695, 84);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(75, 23);
            this.labelTotal.TabIndex = 16;
            this.labelTotal.Text = "Total";
            // 
            // labelVatRate
            // 
            this.labelVatRate.Location = new System.Drawing.Point(483, 49);
            this.labelVatRate.Name = "labelVatRate";
            this.labelVatRate.Size = new System.Drawing.Size(95, 23);
            this.labelVatRate.TabIndex = 18;
            this.labelVatRate.Text = "Vat Rate (%)";
            // 
            // labelHandle
            // 
            this.labelHandle.Location = new System.Drawing.Point(483, 119);
            this.labelHandle.Name = "labelHandle";
            this.labelHandle.Size = new System.Drawing.Size(95, 23);
            this.labelHandle.TabIndex = 19;
            this.labelHandle.Text = "Handle";
            // 
            // label25
            // 
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label25.Location = new System.Drawing.Point(880, 14);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(115, 23);
            this.label25.TabIndex = 43;
            this.label25.Text = "Not Approve";
            this.label25.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // btnBatchImport
            // 
            this.btnBatchImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBatchImport.Location = new System.Drawing.Point(876, 45);
            this.btnBatchImport.Name = "btnBatchImport";
            this.btnBatchImport.Size = new System.Drawing.Size(120, 30);
            this.btnBatchImport.TabIndex = 10;
            this.btnBatchImport.Text = "Batch Import";
            this.btnBatchImport.UseVisualStyleBackColor = true;
            this.btnBatchImport.Click += new System.EventHandler(this.btnBatchImport_Click);
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(86, 15);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(120, 23);
            this.displayID.TabIndex = 0;
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "issuedate", true));
            this.dateIssueDate.Location = new System.Drawing.Point(334, 15);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(130, 23);
            this.dateIssueDate.TabIndex = 5;
            // 
            // displayCurrency
            // 
            this.displayCurrency.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCurrency.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CurrencyID", true));
            this.displayCurrency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCurrency.Location = new System.Drawing.Point(581, 15);
            this.displayCurrency.Name = "displayCurrency";
            this.displayCurrency.Size = new System.Drawing.Size(100, 23);
            this.displayCurrency.TabIndex = 2;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(86, 154);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(345, 23);
            this.txtRemark.TabIndex = 3;
            // 
            // numVatRate
            // 
            this.numVatRate.BackColor = System.Drawing.Color.White;
            this.numVatRate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "vatrate", true));
            this.numVatRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numVatRate.Location = new System.Drawing.Point(581, 49);
            this.numVatRate.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numVatRate.MaxLength = 5;
            this.numVatRate.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numVatRate.Name = "numVatRate";
            this.numVatRate.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numVatRate.Size = new System.Drawing.Size(100, 23);
            this.numVatRate.TabIndex = 7;
            this.numVatRate.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numVat
            // 
            this.numVat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numVat.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "vat", true));
            this.numVat.DecimalPlaces = 2;
            this.numVat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numVat.IsSupportEditMode = false;
            this.numVat.Location = new System.Drawing.Point(773, 49);
            this.numVat.Name = "numVat";
            this.numVat.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numVat.ReadOnly = true;
            this.numVat.Size = new System.Drawing.Size(100, 23);
            this.numVat.TabIndex = 7;
            this.numVat.TabStop = false;
            this.numVat.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numAmount
            // 
            this.numAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numAmount.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "amount", true));
            this.numAmount.DecimalPlaces = 2;
            this.numAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numAmount.IsSupportEditMode = false;
            this.numAmount.Location = new System.Drawing.Point(773, 15);
            this.numAmount.Name = "numAmount";
            this.numAmount.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAmount.ReadOnly = true;
            this.numAmount.Size = new System.Drawing.Size(100, 23);
            this.numAmount.TabIndex = 3;
            this.numAmount.TabStop = false;
            this.numAmount.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numTotal
            // 
            this.numTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotal.DecimalPlaces = 2;
            this.numTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotal.IsSupportEditMode = false;
            this.numTotal.Location = new System.Drawing.Point(773, 84);
            this.numTotal.Name = "numTotal";
            this.numTotal.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotal.ReadOnly = true;
            this.numTotal.Size = new System.Drawing.Size(100, 23);
            this.numTotal.TabIndex = 9;
            this.numTotal.TabStop = false;
            this.numTotal.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelTerms
            // 
            this.labelTerms.Location = new System.Drawing.Point(8, 119);
            this.labelTerms.Name = "labelTerms";
            this.labelTerms.Size = new System.Drawing.Size(75, 23);
            this.labelTerms.TabIndex = 46;
            this.labelTerms.Text = "Terms";
            // 
            // txtInvoice
            // 
            this.txtInvoice.BackColor = System.Drawing.Color.White;
            this.txtInvoice.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "invno", true));
            this.txtInvoice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvoice.Location = new System.Drawing.Point(86, 189);
            this.txtInvoice.Name = "txtInvoice";
            this.txtInvoice.Size = new System.Drawing.Size(345, 23);
            this.txtInvoice.TabIndex = 4;
            // 
            // labelInvoice
            // 
            this.labelInvoice.Location = new System.Drawing.Point(8, 189);
            this.labelInvoice.Name = "labelInvoice";
            this.labelInvoice.Size = new System.Drawing.Size(75, 23);
            this.labelInvoice.TabIndex = 48;
            this.labelInvoice.Text = "Invoice#";
            // 
            // displayVoucher
            // 
            this.displayVoucher.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayVoucher.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "VoucherID", true));
            this.displayVoucher.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayVoucher.Location = new System.Drawing.Point(581, 224);
            this.displayVoucher.Name = "displayVoucher";
            this.displayVoucher.Size = new System.Drawing.Size(120, 23);
            this.displayVoucher.TabIndex = 16;
            // 
            // labelVoucher
            // 
            this.labelVoucher.Location = new System.Drawing.Point(483, 224);
            this.labelVoucher.Name = "labelVoucher";
            this.labelVoucher.Size = new System.Drawing.Size(95, 23);
            this.labelVoucher.TabIndex = 51;
            this.labelVoucher.Text = "Voucher#";
            // 
            // dateApvDate
            // 
            this.dateApvDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "apvdate", true));
            this.dateApvDate.IsSupportEditMode = false;
            this.dateApvDate.Location = new System.Drawing.Point(581, 189);
            this.dateApvDate.Name = "dateApvDate";
            this.dateApvDate.ReadOnly = true;
            this.dateApvDate.Size = new System.Drawing.Size(130, 23);
            this.dateApvDate.TabIndex = 15;
            this.dateApvDate.TabStop = false;
            // 
            // txtmfactory
            // 
            this.txtmfactory.BackColor = System.Drawing.Color.White;
            this.txtmfactory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "factoryid", true));
            this.txtmfactory.FilteMDivision = true;
            this.txtmfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtmfactory.IssupportJunk = false;
            this.txtmfactory.Location = new System.Drawing.Point(334, 49);
            this.txtmfactory.Name = "txtmfactory";
            this.txtmfactory.Size = new System.Drawing.Size(66, 23);
            this.txtmfactory.TabIndex = 6;
            // 
            // txtpayterm_ftyTerms
            // 
            this.txtpayterm_ftyTerms.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "paytermid", true));
            this.txtpayterm_ftyTerms.DisplayBox1Binding = "";
            this.txtpayterm_ftyTerms.Location = new System.Drawing.Point(86, 119);
            this.txtpayterm_ftyTerms.Name = "txtpayterm_ftyTerms";
            this.txtpayterm_ftyTerms.Size = new System.Drawing.Size(348, 23);
            this.txtpayterm_ftyTerms.TabIndex = 2;
            this.txtpayterm_ftyTerms.TextBox1Binding = "";
            // 
            // txtuserAccountant
            // 
            this.txtuserAccountant.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "apvname", true));
            this.txtuserAccountant.DisplayBox1Binding = "";
            this.txtuserAccountant.Enabled = false;
            this.txtuserAccountant.Location = new System.Drawing.Point(581, 154);
            this.txtuserAccountant.Name = "txtuserAccountant";
            this.txtuserAccountant.Size = new System.Drawing.Size(300, 23);
            this.txtuserAccountant.TabIndex = 9;
            this.txtuserAccountant.TextBox1Binding = "";
            // 
            // txtuserHandle
            // 
            this.txtuserHandle.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "handle", true));
            this.txtuserHandle.DisplayBox1Binding = "";
            this.txtuserHandle.Location = new System.Drawing.Point(581, 119);
            this.txtuserHandle.Name = "txtuserHandle";
            this.txtuserHandle.Size = new System.Drawing.Size(300, 23);
            this.txtuserHandle.TabIndex = 8;
            this.txtuserHandle.TextBox1Binding = "";
            // 
            // txtartworktype_ftyCategory
            // 
            this.txtartworktype_ftyCategory.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftyCategory.CClassify = "\'P\'";
            this.txtartworktype_ftyCategory.CSubprocess = "";
            this.txtartworktype_ftyCategory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "category", true));
            this.txtartworktype_ftyCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftyCategory.Location = new System.Drawing.Point(86, 49);
            this.txtartworktype_ftyCategory.Name = "txtartworktype_ftyCategory";
            this.txtartworktype_ftyCategory.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_ftyCategory.TabIndex = 0;
            this.txtartworktype_ftyCategory.Validated += new System.EventHandler(this.txtartworktype_ftyCategory_Validated);
            // 
            // txtsubconSupplier
            // 
            this.txtsubconSupplier.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "localsuppid", true));
            this.txtsubconSupplier.DisplayBox1Binding = "";
            this.txtsubconSupplier.IsIncludeJunk = false;
            this.txtsubconSupplier.Location = new System.Drawing.Point(86, 84);
            this.txtsubconSupplier.Name = "txtsubconSupplier";
            this.txtsubconSupplier.Size = new System.Drawing.Size(159, 23);
            this.txtsubconSupplier.TabIndex = 1;
            this.txtsubconSupplier.TextBox1Binding = "";
            // 
            // labelTotalQty
            // 
            this.labelTotalQty.Location = new System.Drawing.Point(714, 189);
            this.labelTotalQty.Name = "labelTotalQty";
            this.labelTotalQty.Size = new System.Drawing.Size(67, 23);
            this.labelTotalQty.TabIndex = 52;
            this.labelTotalQty.Text = "Total Qty";
            // 
            // numTotalqty
            // 
            this.numTotalqty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalqty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalqty.IsSupportEditMode = false;
            this.numTotalqty.Location = new System.Drawing.Point(784, 189);
            this.numTotalqty.Name = "numTotalqty";
            this.numTotalqty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalqty.ReadOnly = true;
            this.numTotalqty.Size = new System.Drawing.Size(89, 23);
            this.numTotalqty.TabIndex = 53;
            this.numTotalqty.TabStop = false;
            this.numTotalqty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // lbExVoucherID
            // 
            this.lbExVoucherID.Location = new System.Drawing.Point(8, 224);
            this.lbExVoucherID.Name = "lbExVoucherID";
            this.lbExVoucherID.Size = new System.Drawing.Size(103, 23);
            this.lbExVoucherID.TabIndex = 54;
            this.lbExVoucherID.Text = "Ex Voucher No.";
            // 
            // disExVoucherID
            // 
            this.disExVoucherID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disExVoucherID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disExVoucherID.Location = new System.Drawing.Point(114, 224);
            this.disExVoucherID.Name = "disExVoucherID";
            this.disExVoucherID.Size = new System.Drawing.Size(317, 23);
            this.disExVoucherID.TabIndex = 55;
            // 
            // P35
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1008, 577);
            this.DefaultControl = "txtartworktype_ftyCategory";
            this.DefaultControlForEdit = "dateIssueDate";
            this.DefaultDetailOrder = "Localpoid+orderid+refno+threadcolorid";
            this.DefaultOrder = "issuedate,id";
            this.Grid2New = 0;
            this.GridAlias = "LocalAP_detail";
            this.GridNew = 0;
            this.GridUniqueKey = "localpo_detailukey";
            this.IsSupportConfirm = true;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P35";
            this.Text = "P35. Local Purchase Accounts Payable";
            this.UnApvChkValue = "Approved";
            this.UniqueExpress = "id";
            this.WorkAlias = "LocalAP";
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

        private Win.UI.Label labelHandle;
        private Win.UI.Label labelVatRate;
        private Win.UI.Label labelTotal;
        private Win.UI.Label labelCurrency;
        private Win.UI.Label labelApvDate;
        private Win.UI.Label labelVat;
        private Win.UI.Label labelAccountant;
        private Win.UI.Label labelAmount;
        private Win.UI.Label labelIssueDate;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelID;
        private Win.UI.NumericBox numVatRate;
        private Win.UI.TextBox txtRemark;
        private Class.Txtartworktype_fty txtartworktype_ftyCategory;
        private Win.UI.DisplayBox displayCurrency;
        private Win.UI.DateBox dateIssueDate;
        private Class.TxtsubconNoConfirm txtsubconSupplier;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Button btnBatchImport;
        private Win.UI.Label label25;
        private Class.Txtuser txtuserAccountant;
        private Class.Txtuser txtuserHandle;
        private Win.UI.NumericBox numTotal;
        private Win.UI.NumericBox numAmount;
        private Win.UI.NumericBox numVat;
        private Win.UI.DisplayBox displayVoucher;
        private Win.UI.Label labelVoucher;
        private Win.UI.TextBox txtInvoice;
        private Win.UI.Label labelInvoice;
        private Class.Txtpayterm_fty txtpayterm_ftyTerms;
        private Win.UI.Label labelTerms;
        private Class.Txtfactory txtmfactory;
        private Win.UI.DateBox dateApvDate;
        private Win.UI.NumericBox numTotalqty;
        private Win.UI.Label labelTotalQty;
        private Win.UI.DisplayBox disExVoucherID;
        private Win.UI.Label lbExVoucherID;
    }
}
