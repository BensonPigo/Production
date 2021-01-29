namespace Sci.Production.Subcon
{
    partial class P01
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
            this.labelPONo = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelArtworkType = new Sci.Win.UI.Label();
            this.labelInternalRemark = new Sci.Win.UI.Label();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.labelAmount = new Sci.Win.UI.Label();
            this.labelApproveName = new Sci.Win.UI.Label();
            this.labelVat = new Sci.Win.UI.Label();
            this.labelApproveDate = new Sci.Win.UI.Label();
            this.labelCurrency = new Sci.Win.UI.Label();
            this.labelTotal = new Sci.Win.UI.Label();
            this.labelVatRate = new Sci.Win.UI.Label();
            this.labelHandle = new Sci.Win.UI.Label();
            this.label25 = new Sci.Win.UI.Label();
            this.label17 = new Sci.Win.UI.Label();
            this.btnBatchImport = new Sci.Win.UI.Button();
            this.displayPONo = new Sci.Win.UI.DisplayBox();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
            this.displayCurrency = new Sci.Win.UI.DisplayBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.numVatRate = new Sci.Win.UI.NumericBox();
            this.txtInternalRemark = new Sci.Win.UI.TextBox();
            this.numVat = new Sci.Win.UI.NumericBox();
            this.numAmount = new Sci.Win.UI.NumericBox();
            this.numTotal = new Sci.Win.UI.NumericBox();
            this.labelDeliveryDate = new Sci.Win.UI.Label();
            this.dateDeliveryDate = new Sci.Win.UI.DateBox();
            this.btnBatchCreate = new Sci.Win.UI.Button();
            this.dateApproveDate = new Sci.Win.UI.DateBox();
            this.txtmfactory = new Sci.Production.Class.Txtfactory();
            this.txtuserApproveName = new Sci.Production.Class.Txtuser();
            this.txtuserHandle = new Sci.Production.Class.Txtuser();
            this.txtartworktype_ftyArtworkType = new Sci.Production.Class.Txtartworktype_fty();
            this.txtsubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.labelTotalPoQty = new Sci.Win.UI.Label();
            this.numTotalPOQty = new Sci.Win.UI.NumericBox();
            this.btnIrrPriceReason = new Sci.Win.UI.Button();
            this.btnBatchApprove = new Sci.Win.UI.Button();
            this.lbLockName = new Sci.Win.UI.Label();
            this.lbCloseName = new Sci.Win.UI.Label();
            this.lbLockDate = new Sci.Win.UI.Label();
            this.lbCloseDate = new Sci.Win.UI.Label();
            this.txtuserLockName = new Sci.Production.Class.Txtuser();
            this.txtuserCloseName = new Sci.Production.Class.Txtuser();
            this.dateLockDate = new Sci.Win.UI.DateBox();
            this.dateCloseDate = new Sci.Win.UI.DateBox();
            this.btnPurchase = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.btnPurchase);
            this.masterpanel.Controls.Add(this.txtuserCloseName);
            this.masterpanel.Controls.Add(this.txtuserLockName);
            this.masterpanel.Controls.Add(this.lbCloseDate);
            this.masterpanel.Controls.Add(this.lbLockDate);
            this.masterpanel.Controls.Add(this.lbCloseName);
            this.masterpanel.Controls.Add(this.lbLockName);
            this.masterpanel.Controls.Add(this.btnIrrPriceReason);
            this.masterpanel.Controls.Add(this.numTotalPOQty);
            this.masterpanel.Controls.Add(this.labelTotalPoQty);
            this.masterpanel.Controls.Add(this.txtmfactory);
            this.masterpanel.Controls.Add(this.labelDeliveryDate);
            this.masterpanel.Controls.Add(this.numTotal);
            this.masterpanel.Controls.Add(this.numAmount);
            this.masterpanel.Controls.Add(this.numVat);
            this.masterpanel.Controls.Add(this.txtuserApproveName);
            this.masterpanel.Controls.Add(this.txtuserHandle);
            this.masterpanel.Controls.Add(this.txtInternalRemark);
            this.masterpanel.Controls.Add(this.numVatRate);
            this.masterpanel.Controls.Add(this.txtRemark);
            this.masterpanel.Controls.Add(this.txtartworktype_ftyArtworkType);
            this.masterpanel.Controls.Add(this.displayCurrency);
            this.masterpanel.Controls.Add(this.txtsubconSupplier);
            this.masterpanel.Controls.Add(this.displayPONo);
            this.masterpanel.Controls.Add(this.btnBatchImport);
            this.masterpanel.Controls.Add(this.label17);
            this.masterpanel.Controls.Add(this.label25);
            this.masterpanel.Controls.Add(this.labelHandle);
            this.masterpanel.Controls.Add(this.labelVatRate);
            this.masterpanel.Controls.Add(this.labelTotal);
            this.masterpanel.Controls.Add(this.labelCurrency);
            this.masterpanel.Controls.Add(this.labelApproveDate);
            this.masterpanel.Controls.Add(this.labelVat);
            this.masterpanel.Controls.Add(this.labelApproveName);
            this.masterpanel.Controls.Add(this.labelAmount);
            this.masterpanel.Controls.Add(this.labelIssueDate);
            this.masterpanel.Controls.Add(this.labelInternalRemark);
            this.masterpanel.Controls.Add(this.labelArtworkType);
            this.masterpanel.Controls.Add(this.labelSupplier);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.labelFactory);
            this.masterpanel.Controls.Add(this.labelPONo);
            this.masterpanel.Controls.Add(this.dateCloseDate);
            this.masterpanel.Controls.Add(this.dateLockDate);
            this.masterpanel.Controls.Add(this.dateApproveDate);
            this.masterpanel.Controls.Add(this.dateDeliveryDate);
            this.masterpanel.Controls.Add(this.dateIssueDate);
            this.masterpanel.Size = new System.Drawing.Size(1070, 226);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateDeliveryDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateApproveDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateLockDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateCloseDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPONo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSupplier, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelArtworkType, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelInternalRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelAmount, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelApproveName, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelVat, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelApproveDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCurrency, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTotal, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelVatRate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.label25, 0);
            this.masterpanel.Controls.SetChildIndex(this.label17, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnBatchImport, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayPONo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtsubconSupplier, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayCurrency, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtartworktype_ftyArtworkType, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.numVatRate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtInternalRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserApproveName, 0);
            this.masterpanel.Controls.SetChildIndex(this.numVat, 0);
            this.masterpanel.Controls.SetChildIndex(this.numAmount, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTotal, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDeliveryDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtmfactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTotalPoQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTotalPOQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnIrrPriceReason, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbLockName, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbCloseName, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbLockDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbCloseDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserLockName, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserCloseName, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnPurchase, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 226);
            this.detailpanel.Size = new System.Drawing.Size(1070, 230);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.gridicon.Location = new System.Drawing.Point(953, 182);
            this.gridicon.TabIndex = 11;
            this.gridicon.TabStop = false;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(914, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1070, 230);
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
            this.detail.Size = new System.Drawing.Size(1070, 494);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1070, 456);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 456);
            this.detailbtm.Size = new System.Drawing.Size(1070, 38);
            this.detailbtm.TabIndex = 1;
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1070, 494);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1078, 523);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(486, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(438, 13);
            // 
            // labelPONo
            // 
            this.labelPONo.Location = new System.Drawing.Point(6, 14);
            this.labelPONo.Name = "labelPONo";
            this.labelPONo.Size = new System.Drawing.Size(97, 23);
            this.labelPONo.TabIndex = 1;
            this.labelPONo.Text = "PO #";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(285, 14);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(84, 23);
            this.labelFactory.TabIndex = 4;
            this.labelFactory.Text = "Factory";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(6, 118);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(96, 23);
            this.labelRemark.TabIndex = 5;
            this.labelRemark.Text = "Remark";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(6, 40);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelSupplier.Size = new System.Drawing.Size(97, 23);
            this.labelSupplier.TabIndex = 6;
            this.labelSupplier.Text = "Supplier";
            this.labelSupplier.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.Location = new System.Drawing.Point(6, 66);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelArtworkType.Size = new System.Drawing.Size(96, 23);
            this.labelArtworkType.TabIndex = 7;
            this.labelArtworkType.Text = "ArtworkType";
            this.labelArtworkType.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelInternalRemark
            // 
            this.labelInternalRemark.Location = new System.Drawing.Point(476, 118);
            this.labelInternalRemark.Name = "labelInternalRemark";
            this.labelInternalRemark.Size = new System.Drawing.Size(109, 23);
            this.labelInternalRemark.TabIndex = 8;
            this.labelInternalRemark.Text = "Internal Remark";
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(285, 40);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.Size = new System.Drawing.Size(84, 23);
            this.labelIssueDate.TabIndex = 9;
            this.labelIssueDate.Text = "Issue Date";
            // 
            // labelAmount
            // 
            this.labelAmount.Location = new System.Drawing.Point(689, 14);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(75, 23);
            this.labelAmount.TabIndex = 10;
            this.labelAmount.Text = "Amount";
            // 
            // labelApproveName
            // 
            this.labelApproveName.Location = new System.Drawing.Point(6, 170);
            this.labelApproveName.Name = "labelApproveName";
            this.labelApproveName.Size = new System.Drawing.Size(96, 23);
            this.labelApproveName.TabIndex = 11;
            this.labelApproveName.Text = "ApproveName";
            // 
            // labelVat
            // 
            this.labelVat.Location = new System.Drawing.Point(689, 40);
            this.labelVat.Name = "labelVat";
            this.labelVat.Size = new System.Drawing.Size(75, 23);
            this.labelVat.TabIndex = 13;
            this.labelVat.Text = "Vat";
            // 
            // labelApproveDate
            // 
            this.labelApproveDate.Location = new System.Drawing.Point(476, 170);
            this.labelApproveDate.Name = "labelApproveDate";
            this.labelApproveDate.Size = new System.Drawing.Size(109, 23);
            this.labelApproveDate.TabIndex = 14;
            this.labelApproveDate.Text = "Approved Date";
            // 
            // labelCurrency
            // 
            this.labelCurrency.Location = new System.Drawing.Point(476, 14);
            this.labelCurrency.Name = "labelCurrency";
            this.labelCurrency.Size = new System.Drawing.Size(109, 23);
            this.labelCurrency.TabIndex = 15;
            this.labelCurrency.Text = "Currency";
            // 
            // labelTotal
            // 
            this.labelTotal.Location = new System.Drawing.Point(689, 66);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(75, 23);
            this.labelTotal.TabIndex = 16;
            this.labelTotal.Text = "Total";
            // 
            // labelVatRate
            // 
            this.labelVatRate.Location = new System.Drawing.Point(476, 40);
            this.labelVatRate.Name = "labelVatRate";
            this.labelVatRate.Size = new System.Drawing.Size(109, 23);
            this.labelVatRate.TabIndex = 18;
            this.labelVatRate.Text = "Vat Rate (%)";
            // 
            // labelHandle
            // 
            this.labelHandle.Location = new System.Drawing.Point(6, 92);
            this.labelHandle.Name = "labelHandle";
            this.labelHandle.Size = new System.Drawing.Size(96, 23);
            this.labelHandle.TabIndex = 19;
            this.labelHandle.Text = "Handle";
            // 
            // label25
            // 
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label25.Location = new System.Drawing.Point(870, 14);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(126, 23);
            this.label25.TabIndex = 16;
            this.label25.Text = "Not Approve";
            this.label25.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label17.Location = new System.Drawing.Point(929, 150);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(113, 23);
            this.label17.TabIndex = 44;
            this.label17.Text = "Exceed Qty";
            this.label17.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // btnBatchImport
            // 
            this.btnBatchImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBatchImport.Location = new System.Drawing.Point(870, 44);
            this.btnBatchImport.Name = "btnBatchImport";
            this.btnBatchImport.Size = new System.Drawing.Size(188, 30);
            this.btnBatchImport.TabIndex = 9;
            this.btnBatchImport.TabStop = false;
            this.btnBatchImport.Text = "Batch Import";
            this.btnBatchImport.UseVisualStyleBackColor = true;
            this.btnBatchImport.Click += new System.EventHandler(this.BtnBatchImport_Click);
            // 
            // displayPONo
            // 
            this.displayPONo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPONo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayPONo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPONo.Location = new System.Drawing.Point(107, 14);
            this.displayPONo.Name = "displayPONo";
            this.displayPONo.Size = new System.Drawing.Size(165, 23);
            this.displayPONo.TabIndex = 13;
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "issuedate", true));
            this.dateIssueDate.Location = new System.Drawing.Point(373, 40);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(100, 23);
            this.dateIssueDate.TabIndex = 2;
            // 
            // displayCurrency
            // 
            this.displayCurrency.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCurrency.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CurrencyID", true));
            this.displayCurrency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCurrency.Location = new System.Drawing.Point(586, 14);
            this.displayCurrency.Name = "displayCurrency";
            this.displayCurrency.Size = new System.Drawing.Size(100, 23);
            this.displayCurrency.TabIndex = 15;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(107, 118);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(366, 23);
            this.txtRemark.TabIndex = 7;
            // 
            // numVatRate
            // 
            this.numVatRate.BackColor = System.Drawing.Color.White;
            this.numVatRate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "vatrate", true));
            this.numVatRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numVatRate.Location = new System.Drawing.Point(586, 40);
            this.numVatRate.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numVatRate.MaxLength = 3;
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
            this.numVatRate.TabIndex = 3;
            this.numVatRate.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtInternalRemark
            // 
            this.txtInternalRemark.BackColor = System.Drawing.Color.White;
            this.txtInternalRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "internalremark", true));
            this.txtInternalRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInternalRemark.Location = new System.Drawing.Point(588, 118);
            this.txtInternalRemark.Name = "txtInternalRemark";
            this.txtInternalRemark.Size = new System.Drawing.Size(239, 23);
            this.txtInternalRemark.TabIndex = 8;
            // 
            // numVat
            // 
            this.numVat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numVat.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "vat", true));
            this.numVat.DecimalPlaces = 2;
            this.numVat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numVat.IsSupportEditMode = false;
            this.numVat.Location = new System.Drawing.Point(767, 40);
            this.numVat.Name = "numVat";
            this.numVat.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numVat.ReadOnly = true;
            this.numVat.Size = new System.Drawing.Size(100, 23);
            this.numVat.TabIndex = 20;
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
            this.numAmount.Location = new System.Drawing.Point(767, 14);
            this.numAmount.Name = "numAmount";
            this.numAmount.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAmount.ReadOnly = true;
            this.numAmount.Size = new System.Drawing.Size(100, 23);
            this.numAmount.TabIndex = 17;
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
            this.numTotal.Location = new System.Drawing.Point(767, 66);
            this.numTotal.Name = "numTotal";
            this.numTotal.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotal.ReadOnly = true;
            this.numTotal.Size = new System.Drawing.Size(100, 23);
            this.numTotal.TabIndex = 22;
            this.numTotal.TabStop = false;
            this.numTotal.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelDeliveryDate
            // 
            this.labelDeliveryDate.Location = new System.Drawing.Point(285, 66);
            this.labelDeliveryDate.Name = "labelDeliveryDate";
            this.labelDeliveryDate.Size = new System.Drawing.Size(84, 23);
            this.labelDeliveryDate.TabIndex = 69;
            this.labelDeliveryDate.Text = "DeliveryDate";
            // 
            // dateDeliveryDate
            // 
            this.dateDeliveryDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "delivery", true));
            this.dateDeliveryDate.Location = new System.Drawing.Point(373, 66);
            this.dateDeliveryDate.Name = "dateDeliveryDate";
            this.dateDeliveryDate.Size = new System.Drawing.Size(100, 23);
            this.dateDeliveryDate.TabIndex = 5;
            // 
            // btnBatchCreate
            // 
            this.btnBatchCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBatchCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBatchCreate.Location = new System.Drawing.Point(837, 12);
            this.btnBatchCreate.Name = "btnBatchCreate";
            this.btnBatchCreate.Size = new System.Drawing.Size(115, 30);
            this.btnBatchCreate.TabIndex = 0;
            this.btnBatchCreate.Text = "Batch Create";
            this.btnBatchCreate.UseVisualStyleBackColor = true;
            this.btnBatchCreate.Click += new System.EventHandler(this.BtnBatchCreate_Click);
            // 
            // dateApproveDate
            // 
            this.dateApproveDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "apvdate", true));
            this.dateApproveDate.IsSupportEditMode = false;
            this.dateApproveDate.Location = new System.Drawing.Point(588, 170);
            this.dateApproveDate.Name = "dateApproveDate";
            this.dateApproveDate.Size = new System.Drawing.Size(130, 23);
            this.dateApproveDate.TabIndex = 12;
            this.dateApproveDate.TabStop = false;
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
            this.txtmfactory.Location = new System.Drawing.Point(373, 14);
            this.txtmfactory.MDivision = null;
            this.txtmfactory.Name = "txtmfactory";
            this.txtmfactory.Size = new System.Drawing.Size(100, 23);
            this.txtmfactory.TabIndex = 0;
            // 
            // txtuserApproveName
            // 
            this.txtuserApproveName.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "apvname", true));
            this.txtuserApproveName.DisplayBox1Binding = "";
            this.txtuserApproveName.Location = new System.Drawing.Point(107, 170);
            this.txtuserApproveName.Name = "txtuserApproveName";
            this.txtuserApproveName.Size = new System.Drawing.Size(300, 23);
            this.txtuserApproveName.TabIndex = 10;
            this.txtuserApproveName.TextBox1Binding = "";
            // 
            // txtuserHandle
            // 
            this.txtuserHandle.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "handle", true));
            this.txtuserHandle.DisplayBox1Binding = "";
            this.txtuserHandle.Location = new System.Drawing.Point(107, 92);
            this.txtuserHandle.Name = "txtuserHandle";
            this.txtuserHandle.Size = new System.Drawing.Size(300, 23);
            this.txtuserHandle.TabIndex = 6;
            this.txtuserHandle.TextBox1Binding = "";
            // 
            // txtartworktype_ftyArtworkType
            // 
            this.txtartworktype_ftyArtworkType.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftyArtworkType.CClassify = "";
            this.txtartworktype_ftyArtworkType.CSubprocess = "Y";
            this.txtartworktype_ftyArtworkType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "artworktypeid", true));
            this.txtartworktype_ftyArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftyArtworkType.Location = new System.Drawing.Point(107, 66);
            this.txtartworktype_ftyArtworkType.Name = "txtartworktype_ftyArtworkType";
            this.txtartworktype_ftyArtworkType.Size = new System.Drawing.Size(165, 23);
            this.txtartworktype_ftyArtworkType.TabIndex = 4;
            this.txtartworktype_ftyArtworkType.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.Txtartworktype_ftyArtworkType_PopUp);
            this.txtartworktype_ftyArtworkType.Validating += new System.ComponentModel.CancelEventHandler(this.Txtartworktype_ftyArtworkType_Validating);
            // 
            // txtsubconSupplier
            // 
            this.txtsubconSupplier.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "localsuppid", true));
            this.txtsubconSupplier.DisplayBox1Binding = "";
            this.txtsubconSupplier.IsIncludeJunk = false;
            this.txtsubconSupplier.IsMisc = false;
            this.txtsubconSupplier.IsShipping = false;
            this.txtsubconSupplier.IsSubcon = false;
            this.txtsubconSupplier.Location = new System.Drawing.Point(107, 40);
            this.txtsubconSupplier.Name = "txtsubconSupplier";
            this.txtsubconSupplier.Size = new System.Drawing.Size(165, 23);
            this.txtsubconSupplier.TabIndex = 1;
            this.txtsubconSupplier.TextBox1Binding = "";
            this.txtsubconSupplier.Validating += new System.ComponentModel.CancelEventHandler(this.TxtsubconSupplier_Validating);
            // 
            // labelTotalPoQty
            // 
            this.labelTotalPoQty.Location = new System.Drawing.Point(476, 66);
            this.labelTotalPoQty.Name = "labelTotalPoQty";
            this.labelTotalPoQty.Size = new System.Drawing.Size(109, 23);
            this.labelTotalPoQty.TabIndex = 70;
            this.labelTotalPoQty.Text = "Total PO Qty";
            // 
            // numTotalPOQty
            // 
            this.numTotalPOQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalPOQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalPOQty.IsSupportEditMode = false;
            this.numTotalPOQty.Location = new System.Drawing.Point(586, 66);
            this.numTotalPOQty.Name = "numTotalPOQty";
            this.numTotalPOQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalPOQty.ReadOnly = true;
            this.numTotalPOQty.Size = new System.Drawing.Size(100, 23);
            this.numTotalPOQty.TabIndex = 71;
            this.numTotalPOQty.TabStop = false;
            this.numTotalPOQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // btnIrrPriceReason
            // 
            this.btnIrrPriceReason.Location = new System.Drawing.Point(870, 85);
            this.btnIrrPriceReason.Name = "btnIrrPriceReason";
            this.btnIrrPriceReason.Size = new System.Drawing.Size(188, 30);
            this.btnIrrPriceReason.TabIndex = 72;
            this.btnIrrPriceReason.TabStop = false;
            this.btnIrrPriceReason.Text = "Irregular Price Reason";
            this.btnIrrPriceReason.UseVisualStyleBackColor = true;
            this.btnIrrPriceReason.Click += new System.EventHandler(this.BtnIrrPriceReason_Click);
            // 
            // btnBatchApprove
            // 
            this.btnBatchApprove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBatchApprove.Location = new System.Drawing.Point(958, 12);
            this.btnBatchApprove.Name = "btnBatchApprove";
            this.btnBatchApprove.Size = new System.Drawing.Size(115, 30);
            this.btnBatchApprove.TabIndex = 4;
            this.btnBatchApprove.Text = "Batch Approve";
            this.btnBatchApprove.UseVisualStyleBackColor = true;
            this.btnBatchApprove.Click += new System.EventHandler(this.BtnBatchApprove_Click);
            // 
            // lbLockName
            // 
            this.lbLockName.Location = new System.Drawing.Point(6, 144);
            this.lbLockName.Name = "lbLockName";
            this.lbLockName.Size = new System.Drawing.Size(96, 23);
            this.lbLockName.TabIndex = 73;
            this.lbLockName.Text = "Lock Name";
            // 
            // lbCloseName
            // 
            this.lbCloseName.Location = new System.Drawing.Point(6, 196);
            this.lbCloseName.Name = "lbCloseName";
            this.lbCloseName.Size = new System.Drawing.Size(96, 23);
            this.lbCloseName.TabIndex = 74;
            this.lbCloseName.Text = "Close Name";
            // 
            // lbLockDate
            // 
            this.lbLockDate.Location = new System.Drawing.Point(476, 144);
            this.lbLockDate.Name = "lbLockDate";
            this.lbLockDate.Size = new System.Drawing.Size(109, 23);
            this.lbLockDate.TabIndex = 75;
            this.lbLockDate.Text = "Locked Date";
            // 
            // lbCloseDate
            // 
            this.lbCloseDate.Location = new System.Drawing.Point(476, 196);
            this.lbCloseDate.Name = "lbCloseDate";
            this.lbCloseDate.Size = new System.Drawing.Size(109, 23);
            this.lbCloseDate.TabIndex = 76;
            this.lbCloseDate.Text = "Closed Date";
            // 
            // txtuserLockName
            // 
            this.txtuserLockName.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LockName", true));
            this.txtuserLockName.DisplayBox1Binding = "";
            this.txtuserLockName.Location = new System.Drawing.Point(107, 144);
            this.txtuserLockName.Name = "txtuserLockName";
            this.txtuserLockName.Size = new System.Drawing.Size(300, 23);
            this.txtuserLockName.TabIndex = 9;
            this.txtuserLockName.TextBox1Binding = "";
            // 
            // txtuserCloseName
            // 
            this.txtuserCloseName.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "CloseName", true));
            this.txtuserCloseName.DisplayBox1Binding = "";
            this.txtuserCloseName.Location = new System.Drawing.Point(107, 196);
            this.txtuserCloseName.Name = "txtuserCloseName";
            this.txtuserCloseName.Size = new System.Drawing.Size(300, 23);
            this.txtuserCloseName.TabIndex = 11;
            this.txtuserCloseName.TextBox1Binding = "";
            // 
            // dateLockDate
            // 
            this.dateLockDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "LockDate", true));
            this.dateLockDate.IsSupportEditMode = false;
            this.dateLockDate.Location = new System.Drawing.Point(588, 144);
            this.dateLockDate.Name = "dateLockDate";
            this.dateLockDate.Size = new System.Drawing.Size(130, 23);
            this.dateLockDate.TabIndex = 10;
            this.dateLockDate.TabStop = false;
            // 
            // dateCloseDate
            // 
            this.dateCloseDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CloseDate", true));
            this.dateCloseDate.IsSupportEditMode = false;
            this.dateCloseDate.Location = new System.Drawing.Point(588, 196);
            this.dateCloseDate.Name = "dateCloseDate";
            this.dateCloseDate.Size = new System.Drawing.Size(130, 23);
            this.dateCloseDate.TabIndex = 14;
            this.dateCloseDate.TabStop = false;
            // 
            // btnPurchase
            // 
            this.btnPurchase.Location = new System.Drawing.Point(833, 121);
            this.btnPurchase.Name = "btnPurchase";
            this.btnPurchase.Size = new System.Drawing.Size(234, 30);
            this.btnPurchase.TabIndex = 77;
            this.btnPurchase.TabStop = false;
            this.btnPurchase.Text = "Purchase Order QTY Breakdown";
            this.btnPurchase.UseVisualStyleBackColor = true;
            this.btnPurchase.Click += new System.EventHandler(this.BtnPurchase_Click);
            // 
            // P01
            // 
            this.ApvChkValue = "Locked";
            this.CheckChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1078, 556);
            this.CloseChkValue = "Approved";
            this.Controls.Add(this.btnBatchApprove);
            this.Controls.Add(this.btnBatchCreate);
            this.DefaultControl = "txtsubconSupplier";
            this.DefaultControlForEdit = "dateIssueDate";
            this.DefaultFilter = "potype=\'O\'";
            this.DefaultOrder = "issuedate,id";
            this.GridAlias = "ArtworkPO_detail";
            this.IsSupportCheck = true;
            this.IsSupportClose = true;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUncheck = true;
            this.IsSupportUnclose = true;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P01";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P01. Sub-con Purchase Order";
            this.UnApvChkValue = "Approved";
            this.UncheckChkValue = "Locked";
            this.UncloseChkValue = "Closed";
            this.UniqueExpress = "ID";
            this.WorkAlias = "ArtworkPO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.P01_FormClosing);
            this.Controls.SetChildIndex(this.tabs, 0);
            this.Controls.SetChildIndex(this.btnBatchCreate, 0);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelHandle;
        private Win.UI.Label labelVatRate;
        private Win.UI.Label labelTotal;
        private Win.UI.Label labelCurrency;
        private Win.UI.Label labelApproveDate;
        private Win.UI.Label labelVat;
        private Win.UI.Label labelApproveName;
        private Win.UI.Label labelAmount;
        private Win.UI.Label labelIssueDate;
        private Win.UI.Label labelInternalRemark;
        private Win.UI.Label labelArtworkType;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelPONo;
        private Win.UI.TextBox txtInternalRemark;
        private Win.UI.NumericBox numVatRate;
        private Win.UI.TextBox txtRemark;
        private Class.Txtartworktype_fty txtartworktype_ftyArtworkType;
        private Win.UI.DisplayBox displayCurrency;
        private Win.UI.DateBox dateIssueDate;
        private Class.TxtsubconNoConfirm txtsubconSupplier;
        private Win.UI.DisplayBox displayPONo;
        private Win.UI.Button btnBatchImport;
        private Win.UI.Label label17;
        private Win.UI.Label label25;
        private Class.Txtuser txtuserApproveName;
        private Class.Txtuser txtuserHandle;
        private Win.UI.NumericBox numTotal;
        private Win.UI.NumericBox numAmount;
        private Win.UI.NumericBox numVat;
        private Win.UI.Label labelDeliveryDate;
        private Win.UI.DateBox dateDeliveryDate;
        private Win.UI.Button btnBatchCreate;
        private Class.Txtfactory txtmfactory;
        private Win.UI.DateBox dateApproveDate;
        private Win.UI.NumericBox numTotalPOQty;
        private Win.UI.Label labelTotalPoQty;
        private Win.UI.Button btnIrrPriceReason;
        private Win.UI.Button btnBatchApprove;
        private Class.Txtuser txtuserLockName;
        private Win.UI.Label lbCloseDate;
        private Win.UI.Label lbLockDate;
        private Win.UI.Label lbCloseName;
        private Win.UI.Label lbLockName;
        private Class.Txtuser txtuserCloseName;
        private Win.UI.DateBox dateCloseDate;
        private Win.UI.DateBox dateLockDate;
        private Win.UI.Button btnPurchase;
    }
}
