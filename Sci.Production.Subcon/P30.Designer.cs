namespace Sci.Production.Subcon
{
    partial class P30
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
            this.labelInternalRemark = new Sci.Win.UI.Label();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.labelAmount = new Sci.Win.UI.Label();
            this.labelApprove = new Sci.Win.UI.Label();
            this.labelVat = new Sci.Win.UI.Label();
            this.labelApvDate = new Sci.Win.UI.Label();
            this.labelCurrency = new Sci.Win.UI.Label();
            this.labelTotal = new Sci.Win.UI.Label();
            this.labelVatRate = new Sci.Win.UI.Label();
            this.label25 = new Sci.Win.UI.Label();
            this.btnImportThread = new Sci.Win.UI.Button();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
            this.displayCurrency = new Sci.Win.UI.DisplayBox();
            this.displayApvDate = new Sci.Win.UI.DisplayBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.numVatRate = new Sci.Win.UI.NumericBox();
            this.txtInternalRemark = new Sci.Win.UI.TextBox();
            this.numVat = new Sci.Win.UI.NumericBox();
            this.numAmount = new Sci.Win.UI.NumericBox();
            this.numTotal = new Sci.Win.UI.NumericBox();
            this.dateDeliveryDate = new Sci.Win.UI.DateBox();
            this.label1 = new Sci.Win.UI.Label();
            this.btnBatchUpdateDellivery = new Sci.Win.UI.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBuyer = new Sci.Win.UI.TextBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.numttlqty = new Sci.Win.UI.NumericBox();
            this.btnIrrPriceReason = new Sci.Win.UI.Button();
            this.displayLockDate = new Sci.Win.UI.DisplayBox();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.displayCloseDate = new Sci.Win.UI.DisplayBox();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.btnBatchApprove = new System.Windows.Forms.Button();
            this.txtLocalPurchaseItem = new Sci.Production.Class.TxtLocalPurchaseItem();
            this.txtuserClose = new Sci.Production.Class.Txtuser();
            this.txtuserLock = new Sci.Production.Class.Txtuser();
            this.txtmfactory = new Sci.Production.Class.Txtfactory();
            this.txtuserApprove = new Sci.Production.Class.Txtuser();
            this.txtsubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
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
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.txtLocalPurchaseItem);
            this.masterpanel.Controls.Add(this.txtuserClose);
            this.masterpanel.Controls.Add(this.displayCloseDate);
            this.masterpanel.Controls.Add(this.label6);
            this.masterpanel.Controls.Add(this.label7);
            this.masterpanel.Controls.Add(this.txtuserLock);
            this.masterpanel.Controls.Add(this.displayLockDate);
            this.masterpanel.Controls.Add(this.label4);
            this.masterpanel.Controls.Add(this.label5);
            this.masterpanel.Controls.Add(this.btnIrrPriceReason);
            this.masterpanel.Controls.Add(this.numttlqty);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.groupBox1);
            this.masterpanel.Controls.Add(this.txtmfactory);
            this.masterpanel.Controls.Add(this.numTotal);
            this.masterpanel.Controls.Add(this.numAmount);
            this.masterpanel.Controls.Add(this.numVat);
            this.masterpanel.Controls.Add(this.txtuserApprove);
            this.masterpanel.Controls.Add(this.txtInternalRemark);
            this.masterpanel.Controls.Add(this.numVatRate);
            this.masterpanel.Controls.Add(this.txtRemark);
            this.masterpanel.Controls.Add(this.displayApvDate);
            this.masterpanel.Controls.Add(this.displayCurrency);
            this.masterpanel.Controls.Add(this.txtsubconSupplier);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.btnImportThread);
            this.masterpanel.Controls.Add(this.label25);
            this.masterpanel.Controls.Add(this.labelVatRate);
            this.masterpanel.Controls.Add(this.labelTotal);
            this.masterpanel.Controls.Add(this.labelCurrency);
            this.masterpanel.Controls.Add(this.labelApvDate);
            this.masterpanel.Controls.Add(this.labelVat);
            this.masterpanel.Controls.Add(this.labelApprove);
            this.masterpanel.Controls.Add(this.labelAmount);
            this.masterpanel.Controls.Add(this.labelIssueDate);
            this.masterpanel.Controls.Add(this.labelInternalRemark);
            this.masterpanel.Controls.Add(this.labelCategory);
            this.masterpanel.Controls.Add(this.labelSupplier);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.labelFactory);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Controls.Add(this.dateIssueDate);
            this.masterpanel.Size = new System.Drawing.Size(1022, 281);
            this.masterpanel.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSupplier, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCategory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelInternalRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelAmount, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelApprove, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelVat, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelApvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCurrency, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTotal, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelVatRate, 0);
            this.masterpanel.Controls.SetChildIndex(this.label25, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportThread, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtsubconSupplier, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayCurrency, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayApvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.numVatRate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtInternalRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserApprove, 0);
            this.masterpanel.Controls.SetChildIndex(this.numVat, 0);
            this.masterpanel.Controls.SetChildIndex(this.numAmount, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTotal, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtmfactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.groupBox1, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.numttlqty, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnIrrPriceReason, 0);
            this.masterpanel.Controls.SetChildIndex(this.label5, 0);
            this.masterpanel.Controls.SetChildIndex(this.label4, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayLockDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserLock, 0);
            this.masterpanel.Controls.SetChildIndex(this.label7, 0);
            this.masterpanel.Controls.SetChildIndex(this.label6, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayCloseDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserClose, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLocalPurchaseItem, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 281);
            this.detailpanel.Size = new System.Drawing.Size(1022, 274);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.gridicon.Location = new System.Drawing.Point(907, 240);
            this.gridicon.TabIndex = 9;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(913, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1022, 274);
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
            this.detail.Size = new System.Drawing.Size(1022, 593);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1022, 555);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 555);
            this.detailbtm.Size = new System.Drawing.Size(1022, 38);
            this.detailbtm.TabIndex = 0;
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1022, 593);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1030, 622);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(475, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(427, 13);
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(8, 13);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(105, 23);
            this.labelID.TabIndex = 1;
            this.labelID.Text = "ID";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(284, 13);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(70, 23);
            this.labelFactory.TabIndex = 4;
            this.labelFactory.Text = "Factory";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(8, 95);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(105, 23);
            this.labelRemark.TabIndex = 5;
            this.labelRemark.Text = "Remark";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(9, 39);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(105, 23);
            this.labelSupplier.TabIndex = 6;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(8, 66);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(105, 23);
            this.labelCategory.TabIndex = 7;
            this.labelCategory.Text = "Category";
            // 
            // labelInternalRemark
            // 
            this.labelInternalRemark.Location = new System.Drawing.Point(8, 124);
            this.labelInternalRemark.Name = "labelInternalRemark";
            this.labelInternalRemark.Size = new System.Drawing.Size(106, 23);
            this.labelInternalRemark.TabIndex = 8;
            this.labelInternalRemark.Text = "Internal Remark";
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(284, 39);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.Size = new System.Drawing.Size(70, 23);
            this.labelIssueDate.TabIndex = 9;
            this.labelIssueDate.Text = "Issue Date";
            // 
            // labelAmount
            // 
            this.labelAmount.Location = new System.Drawing.Point(685, 13);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(56, 23);
            this.labelAmount.TabIndex = 10;
            this.labelAmount.Text = "Amount";
            // 
            // labelApprove
            // 
            this.labelApprove.Location = new System.Drawing.Point(8, 183);
            this.labelApprove.Name = "labelApprove";
            this.labelApprove.Size = new System.Drawing.Size(105, 23);
            this.labelApprove.TabIndex = 11;
            this.labelApprove.Text = "Approve";
            // 
            // labelVat
            // 
            this.labelVat.Location = new System.Drawing.Point(685, 39);
            this.labelVat.Name = "labelVat";
            this.labelVat.Size = new System.Drawing.Size(56, 23);
            this.labelVat.TabIndex = 13;
            this.labelVat.Text = "Vat";
            // 
            // labelApvDate
            // 
            this.labelApvDate.Location = new System.Drawing.Point(491, 183);
            this.labelApvDate.Name = "labelApvDate";
            this.labelApvDate.Size = new System.Drawing.Size(75, 23);
            this.labelApvDate.TabIndex = 14;
            this.labelApvDate.Text = "Apv. Date";
            // 
            // labelCurrency
            // 
            this.labelCurrency.Location = new System.Drawing.Point(491, 13);
            this.labelCurrency.Name = "labelCurrency";
            this.labelCurrency.Size = new System.Drawing.Size(86, 23);
            this.labelCurrency.TabIndex = 15;
            this.labelCurrency.Text = "Currency";
            // 
            // labelTotal
            // 
            this.labelTotal.Location = new System.Drawing.Point(685, 66);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(56, 23);
            this.labelTotal.TabIndex = 16;
            this.labelTotal.Text = "Total";
            // 
            // labelVatRate
            // 
            this.labelVatRate.Location = new System.Drawing.Point(491, 39);
            this.labelVatRate.Name = "labelVatRate";
            this.labelVatRate.Size = new System.Drawing.Size(86, 23);
            this.labelVatRate.TabIndex = 18;
            this.labelVatRate.Text = "Vat Rate (%)";
            // 
            // label25
            // 
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label25.Location = new System.Drawing.Point(890, 14);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(127, 23);
            this.label25.TabIndex = 43;
            this.label25.Text = "Not Approve";
            this.label25.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // btnImportThread
            // 
            this.btnImportThread.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportThread.Location = new System.Drawing.Point(850, 40);
            this.btnImportThread.Name = "btnImportThread";
            this.btnImportThread.Size = new System.Drawing.Size(160, 49);
            this.btnImportThread.TabIndex = 8;
            this.btnImportThread.Text = "Import Thread, Carton request";
            this.btnImportThread.UseVisualStyleBackColor = true;
            this.btnImportThread.Click += new System.EventHandler(this.btnImportThread_Click);
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(116, 13);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(120, 23);
            this.displayID.TabIndex = 0;
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "issuedate", true));
            this.dateIssueDate.Location = new System.Drawing.Point(357, 39);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(130, 23);
            this.dateIssueDate.TabIndex = 5;
            // 
            // displayCurrency
            // 
            this.displayCurrency.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCurrency.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CurrencyID", true));
            this.displayCurrency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCurrency.Location = new System.Drawing.Point(580, 14);
            this.displayCurrency.Name = "displayCurrency";
            this.displayCurrency.Size = new System.Drawing.Size(100, 23);
            this.displayCurrency.TabIndex = 2;
            // 
            // displayApvDate
            // 
            this.displayApvDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayApvDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayApvDate.Location = new System.Drawing.Point(569, 183);
            this.displayApvDate.Name = "displayApvDate";
            this.displayApvDate.Size = new System.Drawing.Size(100, 23);
            this.displayApvDate.TabIndex = 11;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(116, 95);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(728, 23);
            this.txtRemark.TabIndex = 2;
            // 
            // numVatRate
            // 
            this.numVatRate.BackColor = System.Drawing.Color.White;
            this.numVatRate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "vatrate", true));
            this.numVatRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numVatRate.Location = new System.Drawing.Point(580, 39);
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
            // txtInternalRemark
            // 
            this.txtInternalRemark.BackColor = System.Drawing.Color.White;
            this.txtInternalRemark.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtInternalRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "internalremark", true));
            this.txtInternalRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInternalRemark.Location = new System.Drawing.Point(116, 124);
            this.txtInternalRemark.Name = "txtInternalRemark";
            this.txtInternalRemark.Size = new System.Drawing.Size(728, 23);
            this.txtInternalRemark.TabIndex = 3;
            // 
            // numVat
            // 
            this.numVat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numVat.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "vat", true));
            this.numVat.DecimalPlaces = 2;
            this.numVat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numVat.IsSupportEditMode = false;
            this.numVat.Location = new System.Drawing.Point(744, 39);
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
            this.numAmount.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "amount", true));
            this.numAmount.DecimalPlaces = 2;
            this.numAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numAmount.IsSupportEditMode = false;
            this.numAmount.Location = new System.Drawing.Point(744, 13);
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
            this.numTotal.Location = new System.Drawing.Point(744, 66);
            this.numTotal.Name = "numTotal";
            this.numTotal.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotal.ReadOnly = true;
            this.numTotal.Size = new System.Drawing.Size(100, 23);
            this.numTotal.TabIndex = 10;
            this.numTotal.TabStop = false;
            this.numTotal.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // dateDeliveryDate
            // 
            this.dateDeliveryDate.Location = new System.Drawing.Point(70, 12);
            this.dateDeliveryDate.Name = "dateDeliveryDate";
            this.dateDeliveryDate.Size = new System.Drawing.Size(130, 23);
            this.dateDeliveryDate.TabIndex = 6;
            this.dateDeliveryDate.Validating += new System.ComponentModel.CancelEventHandler(this.dateDeliveryDate_Validating);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 23);
            this.label1.TabIndex = 44;
            this.label1.Text = "Delivery";
            // 
            // btnBatchUpdateDellivery
            // 
            this.btnBatchUpdateDellivery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBatchUpdateDellivery.Location = new System.Drawing.Point(338, 8);
            this.btnBatchUpdateDellivery.Name = "btnBatchUpdateDellivery";
            this.btnBatchUpdateDellivery.Size = new System.Drawing.Size(215, 29);
            this.btnBatchUpdateDellivery.TabIndex = 45;
            this.btnBatchUpdateDellivery.Text = "Batch update Delivery,Buyer";
            this.btnBatchUpdateDellivery.UseVisualStyleBackColor = true;
            this.btnBatchUpdateDellivery.Click += new System.EventHandler(this.btnBatchUpdateDellivery_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtBuyer);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnBatchUpdateDellivery);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dateDeliveryDate);
            this.groupBox1.Location = new System.Drawing.Point(5, 233);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(558, 39);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            // 
            // txtBuyer
            // 
            this.txtBuyer.BackColor = System.Drawing.Color.White;
            this.txtBuyer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBuyer.Location = new System.Drawing.Point(254, 12);
            this.txtBuyer.Name = "txtBuyer";
            this.txtBuyer.Size = new System.Drawing.Size(78, 23);
            this.txtBuyer.TabIndex = 47;
            this.txtBuyer.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtBuyer_PopUp);
            this.txtBuyer.Validating += new System.ComponentModel.CancelEventHandler(this.txtBuyer_Validating);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(206, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 23);
            this.label3.TabIndex = 46;
            this.label3.Text = "Buyer";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(284, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 23);
            this.label2.TabIndex = 47;
            this.label2.Text = "Total Qty";
            // 
            // numttlqty
            // 
            this.numttlqty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numttlqty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numttlqty.IsSupportEditMode = false;
            this.numttlqty.Location = new System.Drawing.Point(357, 66);
            this.numttlqty.Name = "numttlqty";
            this.numttlqty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numttlqty.ReadOnly = true;
            this.numttlqty.Size = new System.Drawing.Size(100, 23);
            this.numttlqty.TabIndex = 48;
            this.numttlqty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // btnIrrPriceReason
            // 
            this.btnIrrPriceReason.Location = new System.Drawing.Point(850, 95);
            this.btnIrrPriceReason.Name = "btnIrrPriceReason";
            this.btnIrrPriceReason.Size = new System.Drawing.Size(160, 46);
            this.btnIrrPriceReason.TabIndex = 49;
            this.btnIrrPriceReason.Text = "Irregular Price Reason";
            this.btnIrrPriceReason.UseVisualStyleBackColor = true;
            this.btnIrrPriceReason.Click += new System.EventHandler(this.btnIrrPriceReason_Click);
            // 
            // displayLockDate
            // 
            this.displayLockDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayLockDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayLockDate.Location = new System.Drawing.Point(569, 153);
            this.displayLockDate.Name = "displayLockDate";
            this.displayLockDate.Size = new System.Drawing.Size(100, 23);
            this.displayLockDate.TabIndex = 51;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(491, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 53;
            this.label4.Text = "Lock Date";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 23);
            this.label5.TabIndex = 52;
            this.label5.Text = "Lock";
            // 
            // displayCloseDate
            // 
            this.displayCloseDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCloseDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCloseDate.Location = new System.Drawing.Point(570, 213);
            this.displayCloseDate.Name = "displayCloseDate";
            this.displayCloseDate.Size = new System.Drawing.Size(100, 23);
            this.displayCloseDate.TabIndex = 55;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(492, 213);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 23);
            this.label6.TabIndex = 57;
            this.label6.Text = "Close Date";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(9, 213);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 23);
            this.label7.TabIndex = 56;
            this.label7.Text = "Close";
            // 
            // btnBatchApprove
            // 
            this.btnBatchApprove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBatchApprove.Location = new System.Drawing.Point(875, 12);
            this.btnBatchApprove.Name = "btnBatchApprove";
            this.btnBatchApprove.Size = new System.Drawing.Size(143, 30);
            this.btnBatchApprove.TabIndex = 4;
            this.btnBatchApprove.Text = "Batch Approve";
            this.btnBatchApprove.UseVisualStyleBackColor = true;
            this.btnBatchApprove.Click += new System.EventHandler(this.btnBatchApprove_Click);
            // 
            // txtLocalPurchaseItem
            // 
            this.txtLocalPurchaseItem.BackColor = System.Drawing.Color.White;
            this.txtLocalPurchaseItem.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "category", true));
            this.txtLocalPurchaseItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocalPurchaseItem.Location = new System.Drawing.Point(117, 68);
            this.txtLocalPurchaseItem.Name = "txtLocalPurchaseItem";
            this.txtLocalPurchaseItem.Size = new System.Drawing.Size(164, 23);
            this.txtLocalPurchaseItem.TabIndex = 58;
            this.txtLocalPurchaseItem.TextChanged += new System.EventHandler(this.txtLocalPurchaseItem_TextChanged);
            this.txtLocalPurchaseItem.Validated += new System.EventHandler(this.txtLocalPurchaseItem_Validated);
            // 
            // txtuserClose
            // 
            this.txtuserClose.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "CloseName", true));
            this.txtuserClose.DisplayBox1Binding = "";
            this.txtuserClose.Enabled = false;
            this.txtuserClose.Location = new System.Drawing.Point(117, 213);
            this.txtuserClose.Name = "txtuserClose";
            this.txtuserClose.Size = new System.Drawing.Size(300, 23);
            this.txtuserClose.TabIndex = 54;
            this.txtuserClose.TextBox1Binding = "";
            // 
            // txtuserLock
            // 
            this.txtuserLock.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LockName", true));
            this.txtuserLock.DisplayBox1Binding = "";
            this.txtuserLock.Enabled = false;
            this.txtuserLock.Location = new System.Drawing.Point(116, 153);
            this.txtuserLock.Name = "txtuserLock";
            this.txtuserLock.Size = new System.Drawing.Size(300, 23);
            this.txtuserLock.TabIndex = 50;
            this.txtuserLock.TextBox1Binding = "";
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
            this.txtmfactory.Location = new System.Drawing.Point(357, 13);
            this.txtmfactory.MDivision = null;
            this.txtmfactory.Name = "txtmfactory";
            this.txtmfactory.Size = new System.Drawing.Size(66, 23);
            this.txtmfactory.TabIndex = 4;
            this.txtmfactory.TabStop = false;
            // 
            // txtuserApprove
            // 
            this.txtuserApprove.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "apvname", true));
            this.txtuserApprove.DisplayBox1Binding = "";
            this.txtuserApprove.Enabled = false;
            this.txtuserApprove.Location = new System.Drawing.Point(116, 183);
            this.txtuserApprove.Name = "txtuserApprove";
            this.txtuserApprove.Size = new System.Drawing.Size(300, 23);
            this.txtuserApprove.TabIndex = 6;
            this.txtuserApprove.TextBox1Binding = "";
            // 
            // txtsubconSupplier
            // 
            this.txtsubconSupplier.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "localsuppid", true));
            this.txtsubconSupplier.DisplayBox1Binding = "";
            this.txtsubconSupplier.IsIncludeJunk = false;
            this.txtsubconSupplier.IsMisc = false;
            this.txtsubconSupplier.IsShipping = false;
            this.txtsubconSupplier.IsSubcon = true;
            this.txtsubconSupplier.Location = new System.Drawing.Point(117, 39);
            this.txtsubconSupplier.Name = "txtsubconSupplier";
            this.txtsubconSupplier.Size = new System.Drawing.Size(167, 23);
            this.txtsubconSupplier.TabIndex = 0;
            this.txtsubconSupplier.TextBox1Binding = "";
            // 
            // P30
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1030, 655);
            this.Controls.Add(this.btnBatchApprove);
            this.DefaultControl = "txtsubconSupplier";
            this.DefaultControlForEdit = "dateIssueDate";
            this.DefaultDetailOrder = "orderid,refno,threadcolorid";
            this.DefaultOrder = "issuedate,id";
            this.Grid2New = 0;
            this.GridAlias = "localPO_detail";
            this.GridNew = 0;
            this.GridUniqueKey = "orderid,refno,threadcolorid";
            this.IsSupportCheck = true;
            this.IsSupportClose = true;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUncheck = true;
            this.IsSupportUnclose = true;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P30";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P30. Local Purchase Order";
            this.UnApvChkValue = "Approved";
            this.UniqueExpress = "id";
            this.WorkAlias = "LocalPO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.P30_FormClosing);
            this.Controls.SetChildIndex(this.tabs, 0);
            this.Controls.SetChildIndex(this.btnBatchApprove, 0);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelVatRate;
        private Win.UI.Label labelTotal;
        private Win.UI.Label labelCurrency;
        private Win.UI.Label labelApvDate;
        private Win.UI.Label labelVat;
        private Win.UI.Label labelApprove;
        private Win.UI.Label labelAmount;
        private Win.UI.Label labelIssueDate;
        private Win.UI.Label labelInternalRemark;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelID;
        private Win.UI.TextBox txtInternalRemark;
        private Win.UI.NumericBox numVatRate;
        private Win.UI.TextBox txtRemark;
        private Win.UI.DisplayBox displayApvDate;
        private Win.UI.DisplayBox displayCurrency;
        private Win.UI.DateBox dateIssueDate;
        private Class.TxtsubconNoConfirm txtsubconSupplier;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Button btnImportThread;
        private Win.UI.Label label25;
        private Class.Txtuser txtuserApprove;
        private Win.UI.NumericBox numTotal;
        private Win.UI.NumericBox numAmount;
        private Win.UI.NumericBox numVat;
        private Class.Txtfactory txtmfactory;
        private Win.UI.Button btnBatchUpdateDellivery;
        private Win.UI.Label label1;
        private Win.UI.DateBox dateDeliveryDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private Win.UI.NumericBox numttlqty;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtBuyer;
        private Win.UI.Label label3;
        private Win.UI.Button btnIrrPriceReason;
        private Class.Txtuser txtuserClose;
        private Win.UI.DisplayBox displayCloseDate;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Class.Txtuser txtuserLock;
        private Win.UI.DisplayBox displayLockDate;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private System.Windows.Forms.Button btnBatchApprove;
        private Class.TxtLocalPurchaseItem txtLocalPurchaseItem;
    }
}
