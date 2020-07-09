namespace Sci.Production.Subcon
{
    partial class P10
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
            this.labelArtworkType = new Sci.Win.UI.Label();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.labelAmount = new Sci.Win.UI.Label();
            this.labelAccountant = new Sci.Win.UI.Label();
            this.labelVat = new Sci.Win.UI.Label();
            this.labelApprovedDate = new Sci.Win.UI.Label();
            this.labelCurrency = new Sci.Win.UI.Label();
            this.labelTotal = new Sci.Win.UI.Label();
            this.labelVatRate = new Sci.Win.UI.Label();
            this.labelHandle = new Sci.Win.UI.Label();
            this.label25 = new Sci.Win.UI.Label();
            this.btnImportFromPO = new Sci.Win.UI.Button();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
            this.displayCurrency = new Sci.Win.UI.DisplayBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.numVatRate = new Sci.Win.UI.NumericBox();
            this.numVat = new Sci.Win.UI.NumericBox();
            this.numAmount = new Sci.Win.UI.NumericBox();
            this.numTotal = new Sci.Win.UI.NumericBox();
            this.labelTerms = new Sci.Win.UI.Label();
            this.txtInvoiceNo = new Sci.Win.UI.TextBox();
            this.labelInvoiceNo = new Sci.Win.UI.Label();
            this.displayTransferAC = new Sci.Win.UI.DisplayBox();
            this.labelTransferAC = new Sci.Win.UI.Label();
            this.dateApprovedDate = new Sci.Win.UI.DateBox();
            this.txtmfactory = new Sci.Production.Class.Txtfactory();
            this.txtpayterm_ftyTerms = new Sci.Production.Class.Txtpayterm_fty();
            this.txtuserAccountant = new Sci.Production.Class.Txtuser();
            this.txtuserHandle = new Sci.Production.Class.Txtuser();
            this.txtartworktype_ftyArtworkType = new Sci.Production.Class.Txtartworktype_fty();
            this.txtsubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.lbExVoucherID = new Sci.Win.UI.Label();
            this.disExVoucherID = new Sci.Win.UI.DisplayBox();
            this.label7 = new System.Windows.Forms.Label();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.BtnRemoveQty0 = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.txtmfactory);
            this.masterpanel.Controls.Add(this.displayTransferAC);
            this.masterpanel.Controls.Add(this.labelTransferAC);
            this.masterpanel.Controls.Add(this.txtInvoiceNo);
            this.masterpanel.Controls.Add(this.labelInvoiceNo);
            this.masterpanel.Controls.Add(this.txtpayterm_ftyTerms);
            this.masterpanel.Controls.Add(this.labelTerms);
            this.masterpanel.Controls.Add(this.numTotal);
            this.masterpanel.Controls.Add(this.numAmount);
            this.masterpanel.Controls.Add(this.numVat);
            this.masterpanel.Controls.Add(this.txtuserAccountant);
            this.masterpanel.Controls.Add(this.txtuserHandle);
            this.masterpanel.Controls.Add(this.numVatRate);
            this.masterpanel.Controls.Add(this.txtRemark);
            this.masterpanel.Controls.Add(this.txtartworktype_ftyArtworkType);
            this.masterpanel.Controls.Add(this.displayCurrency);
            this.masterpanel.Controls.Add(this.txtsubconSupplier);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.BtnRemoveQty0);
            this.masterpanel.Controls.Add(this.btnImportFromPO);
            this.masterpanel.Controls.Add(this.label25);
            this.masterpanel.Controls.Add(this.labelHandle);
            this.masterpanel.Controls.Add(this.labelVatRate);
            this.masterpanel.Controls.Add(this.labelTotal);
            this.masterpanel.Controls.Add(this.labelCurrency);
            this.masterpanel.Controls.Add(this.labelApprovedDate);
            this.masterpanel.Controls.Add(this.labelVat);
            this.masterpanel.Controls.Add(this.labelAccountant);
            this.masterpanel.Controls.Add(this.labelAmount);
            this.masterpanel.Controls.Add(this.labelIssueDate);
            this.masterpanel.Controls.Add(this.labelArtworkType);
            this.masterpanel.Controls.Add(this.labelSupplier);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.labelFactory);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Controls.Add(this.dateApprovedDate);
            this.masterpanel.Controls.Add(this.dateIssueDate);
            this.masterpanel.Size = new System.Drawing.Size(1000, 258);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateApprovedDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSupplier, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelArtworkType, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelAmount, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelAccountant, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelVat, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelApprovedDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCurrency, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTotal, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelVatRate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.label25, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportFromPO, 0);
            this.masterpanel.Controls.SetChildIndex(this.BtnRemoveQty0, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtsubconSupplier, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayCurrency, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtartworktype_ftyArtworkType, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.numVatRate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserAccountant, 0);
            this.masterpanel.Controls.SetChildIndex(this.numVat, 0);
            this.masterpanel.Controls.SetChildIndex(this.numAmount, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTotal, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTerms, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtpayterm_ftyTerms, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelInvoiceNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtInvoiceNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTransferAC, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayTransferAC, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtmfactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbExVoucherID, 0);
            this.masterpanel.Controls.SetChildIndex(this.disExVoucherID, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 258);
            this.detailpanel.Size = new System.Drawing.Size(1000, 250);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.gridicon.Location = new System.Drawing.Point(894, 210);
            this.gridicon.TabIndex = 10;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(914, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1000, 250);
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
            this.detail.Size = new System.Drawing.Size(1000, 570);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1000, 508);
            // 
            // detailbtm
            // 
            this.detailbtm.Controls.Add(this.label7);
            this.detailbtm.Controls.Add(this.displayBox1);
            this.detailbtm.Location = new System.Drawing.Point(0, 508);
            this.detailbtm.Size = new System.Drawing.Size(1000, 62);
            this.detailbtm.TabIndex = 0;
            this.detailbtm.Controls.SetChildIndex(this.lbleditby, 0);
            this.detailbtm.Controls.SetChildIndex(this.lblcreateby, 0);
            this.detailbtm.Controls.SetChildIndex(this.editby, 0);
            this.detailbtm.Controls.SetChildIndex(this.createby, 0);
            this.detailbtm.Controls.SetChildIndex(this.refresh, 0);
            this.detailbtm.Controls.SetChildIndex(this.displayBox1, 0);
            this.detailbtm.Controls.SetChildIndex(this.label7, 0);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1000, 570);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1008, 599);
            // 
            // createby
            // 
            this.createby.Location = new System.Drawing.Point(70, 31);
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(475, 31);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lblcreateby
            // 
            this.lblcreateby.Location = new System.Drawing.Point(5, 37);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(427, 37);
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(5, 15);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 1;
            this.labelID.Text = "ID";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(253, 49);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(75, 23);
            this.labelFactory.TabIndex = 4;
            this.labelFactory.Text = "Factory";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(5, 154);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 5;
            this.labelRemark.Text = "Remark";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(5, 84);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(75, 23);
            this.labelSupplier.TabIndex = 6;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.Location = new System.Drawing.Point(5, 49);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.Size = new System.Drawing.Size(92, 23);
            this.labelArtworkType.TabIndex = 7;
            this.labelArtworkType.Text = "ArtworkType";
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(253, 15);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.Size = new System.Drawing.Size(75, 23);
            this.labelIssueDate.TabIndex = 9;
            this.labelIssueDate.Text = "Issue Date";
            // 
            // labelAmount
            // 
            this.labelAmount.Location = new System.Drawing.Point(677, 15);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(59, 23);
            this.labelAmount.TabIndex = 10;
            this.labelAmount.Text = "Amount";
            // 
            // labelAccountant
            // 
            this.labelAccountant.Location = new System.Drawing.Point(471, 154);
            this.labelAccountant.Name = "labelAccountant";
            this.labelAccountant.Size = new System.Drawing.Size(95, 23);
            this.labelAccountant.TabIndex = 11;
            this.labelAccountant.Text = "Accountant";
            // 
            // labelVat
            // 
            this.labelVat.Location = new System.Drawing.Point(677, 49);
            this.labelVat.Name = "labelVat";
            this.labelVat.Size = new System.Drawing.Size(59, 23);
            this.labelVat.TabIndex = 13;
            this.labelVat.Text = "Vat";
            // 
            // labelApprovedDate
            // 
            this.labelApprovedDate.Location = new System.Drawing.Point(471, 189);
            this.labelApprovedDate.Name = "labelApprovedDate";
            this.labelApprovedDate.Size = new System.Drawing.Size(95, 23);
            this.labelApprovedDate.TabIndex = 14;
            this.labelApprovedDate.Text = "ApprovedDate";
            // 
            // labelCurrency
            // 
            this.labelCurrency.Location = new System.Drawing.Point(471, 15);
            this.labelCurrency.Name = "labelCurrency";
            this.labelCurrency.Size = new System.Drawing.Size(95, 23);
            this.labelCurrency.TabIndex = 15;
            this.labelCurrency.Text = "Currency";
            // 
            // labelTotal
            // 
            this.labelTotal.Location = new System.Drawing.Point(677, 84);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(59, 23);
            this.labelTotal.TabIndex = 16;
            this.labelTotal.Text = "Total";
            // 
            // labelVatRate
            // 
            this.labelVatRate.Location = new System.Drawing.Point(471, 49);
            this.labelVatRate.Name = "labelVatRate";
            this.labelVatRate.Size = new System.Drawing.Size(95, 23);
            this.labelVatRate.TabIndex = 18;
            this.labelVatRate.Text = "Vat Rate (%)";
            // 
            // labelHandle
            // 
            this.labelHandle.Location = new System.Drawing.Point(471, 119);
            this.labelHandle.Name = "labelHandle";
            this.labelHandle.Size = new System.Drawing.Size(95, 23);
            this.labelHandle.TabIndex = 19;
            this.labelHandle.Text = "Handle";
            // 
            // label25
            // 
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label25.Location = new System.Drawing.Point(876, 14);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(115, 23);
            this.label25.TabIndex = 43;
            this.label25.Text = "Not Approve";
            this.label25.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // btnImportFromPO
            // 
            this.btnImportFromPO.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportFromPO.Location = new System.Drawing.Point(844, 45);
            this.btnImportFromPO.Name = "btnImportFromPO";
            this.btnImportFromPO.Size = new System.Drawing.Size(152, 30);
            this.btnImportFromPO.TabIndex = 9;
            this.btnImportFromPO.Text = "Import From PO#";
            this.btnImportFromPO.UseVisualStyleBackColor = true;
            this.btnImportFromPO.Click += new System.EventHandler(this.btnImportFromPO_Click);
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(83, 15);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(120, 23);
            this.displayID.TabIndex = 0;
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "issuedate", true));
            this.dateIssueDate.Location = new System.Drawing.Point(331, 15);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(130, 23);
            this.dateIssueDate.TabIndex = 4;
            // 
            // displayCurrency
            // 
            this.displayCurrency.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCurrency.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CurrencyID", true));
            this.displayCurrency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCurrency.Location = new System.Drawing.Point(569, 15);
            this.displayCurrency.Name = "displayCurrency";
            this.displayCurrency.Size = new System.Drawing.Size(100, 23);
            this.displayCurrency.TabIndex = 2;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(83, 154);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(345, 23);
            this.txtRemark.TabIndex = 2;
            // 
            // numVatRate
            // 
            this.numVatRate.BackColor = System.Drawing.Color.White;
            this.numVatRate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "vatrate", true));
            this.numVatRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numVatRate.Location = new System.Drawing.Point(569, 49);
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
            this.numVatRate.TabIndex = 6;
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
            this.numVat.Location = new System.Drawing.Point(737, 49);
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
            this.numAmount.Location = new System.Drawing.Point(737, 15);
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
            this.numTotal.Location = new System.Drawing.Point(737, 84);
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
            this.labelTerms.Location = new System.Drawing.Point(5, 119);
            this.labelTerms.Name = "labelTerms";
            this.labelTerms.Size = new System.Drawing.Size(75, 23);
            this.labelTerms.TabIndex = 46;
            this.labelTerms.Text = "Terms";
            // 
            // txtInvoiceNo
            // 
            this.txtInvoiceNo.BackColor = System.Drawing.Color.White;
            this.txtInvoiceNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "invno", true));
            this.txtInvoiceNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvoiceNo.Location = new System.Drawing.Point(83, 189);
            this.txtInvoiceNo.Name = "txtInvoiceNo";
            this.txtInvoiceNo.Size = new System.Drawing.Size(345, 23);
            this.txtInvoiceNo.TabIndex = 3;
            // 
            // labelInvoiceNo
            // 
            this.labelInvoiceNo.Location = new System.Drawing.Point(5, 189);
            this.labelInvoiceNo.Name = "labelInvoiceNo";
            this.labelInvoiceNo.Size = new System.Drawing.Size(75, 23);
            this.labelInvoiceNo.TabIndex = 48;
            this.labelInvoiceNo.Text = "Invoice#";
            // 
            // displayTransferAC
            // 
            this.displayTransferAC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTransferAC.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "VoucherID", true));
            this.displayTransferAC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTransferAC.Location = new System.Drawing.Point(569, 224);
            this.displayTransferAC.Name = "displayTransferAC";
            this.displayTransferAC.Size = new System.Drawing.Size(120, 23);
            this.displayTransferAC.TabIndex = 16;
            // 
            // labelTransferAC
            // 
            this.labelTransferAC.Location = new System.Drawing.Point(471, 224);
            this.labelTransferAC.Name = "labelTransferAC";
            this.labelTransferAC.Size = new System.Drawing.Size(95, 23);
            this.labelTransferAC.TabIndex = 51;
            this.labelTransferAC.Text = "Transfer A/C";
            // 
            // dateApprovedDate
            // 
            this.dateApprovedDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "apvdate", true));
            this.dateApprovedDate.Location = new System.Drawing.Point(569, 189);
            this.dateApprovedDate.Name = "dateApprovedDate";
            this.dateApprovedDate.ReadOnly = true;
            this.dateApprovedDate.Size = new System.Drawing.Size(130, 23);
            this.dateApprovedDate.TabIndex = 15;
            this.dateApprovedDate.TabStop = false;
            // 
            // txtmfactory
            // 
            this.txtmfactory.BackColor = System.Drawing.Color.White;
            this.txtmfactory.BoolFtyGroupList = true;
            this.txtmfactory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "factoryid", true));
            this.txtmfactory.FilteMDivision = true;
            this.txtmfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtmfactory.IsProduceFty = false;
            this.txtmfactory.IssupportJunk = false;
            this.txtmfactory.Location = new System.Drawing.Point(331, 49);
            this.txtmfactory.MDivision = null;
            this.txtmfactory.Name = "txtmfactory";
            this.txtmfactory.Size = new System.Drawing.Size(66, 23);
            this.txtmfactory.TabIndex = 5;
            // 
            // txtpayterm_ftyTerms
            // 
            this.txtpayterm_ftyTerms.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "paytermid", true));
            this.txtpayterm_ftyTerms.DisplayBox1Binding = "";
            this.txtpayterm_ftyTerms.Location = new System.Drawing.Point(83, 119);
            this.txtpayterm_ftyTerms.Name = "txtpayterm_ftyTerms";
            this.txtpayterm_ftyTerms.Size = new System.Drawing.Size(348, 23);
            this.txtpayterm_ftyTerms.TabIndex = 2;
            this.txtpayterm_ftyTerms.TextBox1Binding = "";
            // 
            // txtuserAccountant
            // 
            this.txtuserAccountant.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "apvname", true));
            this.txtuserAccountant.DisplayBox1Binding = "";
            this.txtuserAccountant.Location = new System.Drawing.Point(569, 154);
            this.txtuserAccountant.Name = "txtuserAccountant";
            this.txtuserAccountant.Size = new System.Drawing.Size(300, 23);
            this.txtuserAccountant.TabIndex = 8;
            this.txtuserAccountant.TextBox1Binding = "";
            // 
            // txtuserHandle
            // 
            this.txtuserHandle.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "handle", true));
            this.txtuserHandle.DisplayBox1Binding = "";
            this.txtuserHandle.Location = new System.Drawing.Point(569, 119);
            this.txtuserHandle.Name = "txtuserHandle";
            this.txtuserHandle.Size = new System.Drawing.Size(300, 23);
            this.txtuserHandle.TabIndex = 7;
            this.txtuserHandle.TextBox1Binding = "";
            // 
            // txtartworktype_ftyArtworkType
            // 
            this.txtartworktype_ftyArtworkType.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftyArtworkType.CClassify = "";
            this.txtartworktype_ftyArtworkType.CSubprocess = "Y";
            this.txtartworktype_ftyArtworkType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "artworktypeid", true));
            this.txtartworktype_ftyArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftyArtworkType.Location = new System.Drawing.Point(100, 49);
            this.txtartworktype_ftyArtworkType.Name = "txtartworktype_ftyArtworkType";
            this.txtartworktype_ftyArtworkType.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_ftyArtworkType.TabIndex = 0;
            this.txtartworktype_ftyArtworkType.Validated += new System.EventHandler(this.txtartworktype_ftyArtworkType_Validated);
            // 
            // txtsubconSupplier
            // 
            this.txtsubconSupplier.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "localsuppid", true));
            this.txtsubconSupplier.DisplayBox1Binding = "";
            this.txtsubconSupplier.IsIncludeJunk = false;
            this.txtsubconSupplier.IsMisc = false;
            this.txtsubconSupplier.IsShipping = false;
            this.txtsubconSupplier.IsSubcon = false;
            this.txtsubconSupplier.Location = new System.Drawing.Point(83, 84);
            this.txtsubconSupplier.Name = "txtsubconSupplier";
            this.txtsubconSupplier.Size = new System.Drawing.Size(159, 23);
            this.txtsubconSupplier.TabIndex = 1;
            this.txtsubconSupplier.TextBox1Binding = "";
            // 
            // lbExVoucherID
            // 
            this.lbExVoucherID.Location = new System.Drawing.Point(5, 224);
            this.lbExVoucherID.Name = "lbExVoucherID";
            this.lbExVoucherID.Size = new System.Drawing.Size(107, 23);
            this.lbExVoucherID.TabIndex = 52;
            this.lbExVoucherID.Text = "Ex Voucher No.";
            // 
            // disExVoucherID
            // 
            this.disExVoucherID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disExVoucherID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disExVoucherID.Location = new System.Drawing.Point(115, 224);
            this.disExVoucherID.Name = "disExVoucherID";
            this.disExVoucherID.Size = new System.Drawing.Size(313, 23);
            this.disExVoucherID.TabIndex = 53;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(37, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(256, 15);
            this.label7.TabIndex = 13;
            this.label7.Text = "Please note that. RFID data from two supplier.";
            // 
            // displayBox1
            // 
            this.displayBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(8, 5);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(23, 23);
            this.displayBox1.TabIndex = 12;
            // 
            // BtnRemoveQty0
            // 
            this.BtnRemoveQty0.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.BtnRemoveQty0.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.BtnRemoveQty0.Location = new System.Drawing.Point(791, 210);
            this.BtnRemoveQty0.Name = "BtnRemoveQty0";
            this.BtnRemoveQty0.Size = new System.Drawing.Size(168, 30);
            this.BtnRemoveQty0.TabIndex = 9;
            this.BtnRemoveQty0.Text = "Remove qty = 0 data";
            this.BtnRemoveQty0.UseVisualStyleBackColor = true;
            this.BtnRemoveQty0.Click += new System.EventHandler(this.BtnRemoveQty0_Click);
            // 
            // P10
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1008, 632);
            this.DefaultControl = "txtpayterm_ftyTerms";
            this.DefaultControlForEdit = "txtRemark";
            this.DefaultOrder = "issuedate,id";
            this.GridAlias = "ArtworkAP_detail";
            this.GridUniqueKey = "artworkpo_detailukey";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P10";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P10. Sub-con Accounts Payable";
            this.UnApvChkValue = "Approved";
            this.UniqueExpress = "id";
            this.WorkAlias = "ArtworkAP";
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
        private Win.UI.Label labelApprovedDate;
        private Win.UI.Label labelVat;
        private Win.UI.Label labelAccountant;
        private Win.UI.Label labelAmount;
        private Win.UI.Label labelIssueDate;
        private Win.UI.Label labelArtworkType;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelID;
        private Win.UI.NumericBox numVatRate;
        private Win.UI.TextBox txtRemark;
        private Class.Txtartworktype_fty txtartworktype_ftyArtworkType;
        private Win.UI.DisplayBox displayCurrency;
        private Win.UI.DateBox dateIssueDate;
        private Class.TxtsubconNoConfirm txtsubconSupplier;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Button btnImportFromPO;
        private Win.UI.Label label25;
        private Class.Txtuser txtuserAccountant;
        private Class.Txtuser txtuserHandle;
        private Win.UI.NumericBox numTotal;
        private Win.UI.NumericBox numAmount;
        private Win.UI.NumericBox numVat;
        private Win.UI.DisplayBox displayTransferAC;
        private Win.UI.Label labelTransferAC;
        private Win.UI.TextBox txtInvoiceNo;
        private Win.UI.Label labelInvoiceNo;
        private Class.Txtpayterm_fty txtpayterm_ftyTerms;
        private Win.UI.Label labelTerms;
        private Class.Txtfactory txtmfactory;
        private Win.UI.DateBox dateApprovedDate;
        private Win.UI.DisplayBox disExVoucherID;
        private Win.UI.Label lbExVoucherID;
        private System.Windows.Forms.Label label7;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.Button BtnRemoveQty0;
    }
}
