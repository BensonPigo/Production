namespace Sci.Production.Subcon
{
    partial class P36
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
            this.labelSDNo = new Sci.Win.UI.Label();
            this.lblTaipeiDebitNote = new Sci.Win.UI.Label();
            this.lblStatus = new Sci.Win.UI.Label();
            this.displaySDNo = new Sci.Win.UI.DisplayBox();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
            this.labelHandle = new Sci.Win.UI.Label();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.labelSMR = new Sci.Win.UI.Label();
            this.datePrintDate = new Sci.Win.UI.DateBox();
            this.labelPrintDate = new Sci.Win.UI.Label();
            this.dateStatusUpdate = new Sci.Win.UI.DateBox();
            this.labelStatusUpdate = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.displayFactory = new Sci.Win.UI.DisplayBox();
            this.btnDebitSchedule = new Sci.Win.UI.Button();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelAmount = new Sci.Win.UI.Label();
            this.numAmount = new Sci.Win.UI.NumericBox();
            this.numTax = new Sci.Win.UI.NumericBox();
            this.labelTax = new Sci.Win.UI.Label();
            this.labelTotalAmt = new Sci.Win.UI.Label();
            this.numTotalAmt = new Sci.Win.UI.NumericBox();
            this.numtaxrate = new Sci.Win.UI.NumericBox();
            this.numExchange = new Sci.Win.UI.NumericBox();
            this.labelExchange = new Sci.Win.UI.Label();
            this.labelAmtReceived = new Sci.Win.UI.Label();
            this.labelReceived = new Sci.Win.UI.Label();
            this.labelConfirmed = new Sci.Win.UI.Label();
            this.labelAccountNo = new Sci.Win.UI.Label();
            this.labelSettleVoucher = new Sci.Win.UI.Label();
            this.displaySettleVoucher = new Sci.Win.UI.DisplayBox();
            this.btnStatusHistory = new Sci.Win.UI.Button();
            this.displaycurrencyid = new Sci.Win.UI.DisplayBox();
            this.displayAmtReceived = new Sci.Win.UI.DisplayBox();
            this.displayConfirmed = new Sci.Win.UI.DisplayBox();
            this.displaySettleDate = new Sci.Win.UI.DisplayBox();
            this.dateReceiveDate = new Sci.Win.UI.DateBox();
            this.txtuserAmtReceived = new Sci.Production.Class.txtuser();
            this.txtsubconSupplier = new Sci.Production.Class.txtsubcon();
            this.txtuserSMR = new Sci.Production.Class.txtuser();
            this.txtuserHandle = new Sci.Production.Class.txtuser();
            this.txtAccountNo = new Sci.Production.Class.txtAccountNo();
            this.txtuserConfirmed = new Sci.Production.Class.txtuser();
            this.txtuserReceived = new Sci.Production.Class.txtuser();
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
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.displayAmtReceived);
            this.masterpanel.Controls.Add(this.displaycurrencyid);
            this.masterpanel.Controls.Add(this.btnStatusHistory);
            this.masterpanel.Controls.Add(this.txtuserAmtReceived);
            this.masterpanel.Controls.Add(this.labelAmtReceived);
            this.masterpanel.Controls.Add(this.numExchange);
            this.masterpanel.Controls.Add(this.labelExchange);
            this.masterpanel.Controls.Add(this.numtaxrate);
            this.masterpanel.Controls.Add(this.numTotalAmt);
            this.masterpanel.Controls.Add(this.labelTotalAmt);
            this.masterpanel.Controls.Add(this.numTax);
            this.masterpanel.Controls.Add(this.labelTax);
            this.masterpanel.Controls.Add(this.numAmount);
            this.masterpanel.Controls.Add(this.labelAmount);
            this.masterpanel.Controls.Add(this.labelSupplier);
            this.masterpanel.Controls.Add(this.txtsubconSupplier);
            this.masterpanel.Controls.Add(this.btnDebitSchedule);
            this.masterpanel.Controls.Add(this.displayFactory);
            this.masterpanel.Controls.Add(this.labelFactory);
            this.masterpanel.Controls.Add(this.dateStatusUpdate);
            this.masterpanel.Controls.Add(this.labelStatusUpdate);
            this.masterpanel.Controls.Add(this.datePrintDate);
            this.masterpanel.Controls.Add(this.labelPrintDate);
            this.masterpanel.Controls.Add(this.txtuserSMR);
            this.masterpanel.Controls.Add(this.labelSMR);
            this.masterpanel.Controls.Add(this.txtuserHandle);
            this.masterpanel.Controls.Add(this.labelHandle);
            this.masterpanel.Controls.Add(this.dateIssueDate);
            this.masterpanel.Controls.Add(this.labelIssueDate);
            this.masterpanel.Controls.Add(this.displaySDNo);
            this.masterpanel.Controls.Add(this.lblTaipeiDebitNote);
            this.masterpanel.Controls.Add(this.lblStatus);
            this.masterpanel.Controls.Add(this.labelSDNo);
            this.masterpanel.Size = new System.Drawing.Size(875, 225);
            this.masterpanel.Controls.SetChildIndex(this.labelSDNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.lblStatus, 0);
            this.masterpanel.Controls.SetChildIndex(this.lblTaipeiDebitNote, 0);
            this.masterpanel.Controls.SetChildIndex(this.displaySDNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSMR, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserSMR, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPrintDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.datePrintDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelStatusUpdate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateStatusUpdate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnDebitSchedule, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtsubconSupplier, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSupplier, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelAmount, 0);
            this.masterpanel.Controls.SetChildIndex(this.numAmount, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTax, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTax, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTotalAmt, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTotalAmt, 0);
            this.masterpanel.Controls.SetChildIndex(this.numtaxrate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelExchange, 0);
            this.masterpanel.Controls.SetChildIndex(this.numExchange, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelAmtReceived, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserAmtReceived, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnStatusHistory, 0);
            this.masterpanel.Controls.SetChildIndex(this.displaycurrencyid, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayAmtReceived, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 225);
            this.detailpanel.Size = new System.Drawing.Size(875, 0);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gridicon.Location = new System.Drawing.Point(5017, 184);
            // 
            // refresh
            // 
            this.refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.refresh.Location = new System.Drawing.Point(900, 167);
            this.refresh.Size = new System.Drawing.Size(92, 35);
            this.refresh.TabIndex = 4;
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(875, 0);
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
            this.detail.Size = new System.Drawing.Size(892, 387);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(892, 177);
            // 
            // detailbtm
            // 
            this.detailbtm.Controls.Add(this.dateReceiveDate);
            this.detailbtm.Controls.Add(this.displaySettleDate);
            this.detailbtm.Controls.Add(this.displayConfirmed);
            this.detailbtm.Controls.Add(this.displaySettleVoucher);
            this.detailbtm.Controls.Add(this.labelSettleVoucher);
            this.detailbtm.Controls.Add(this.labelAccountNo);
            this.detailbtm.Controls.Add(this.txtAccountNo);
            this.detailbtm.Controls.Add(this.txtuserConfirmed);
            this.detailbtm.Controls.Add(this.labelConfirmed);
            this.detailbtm.Controls.Add(this.txtuserReceived);
            this.detailbtm.Controls.Add(this.labelReceived);
            this.detailbtm.Controls.Add(this.editDescription);
            this.detailbtm.Location = new System.Drawing.Point(0, 177);
            this.detailbtm.Size = new System.Drawing.Size(892, 210);
            this.detailbtm.TabIndex = 0;
            this.detailbtm.Controls.SetChildIndex(this.editDescription, 0);
            this.detailbtm.Controls.SetChildIndex(this.labelReceived, 0);
            this.detailbtm.Controls.SetChildIndex(this.txtuserReceived, 0);
            this.detailbtm.Controls.SetChildIndex(this.labelConfirmed, 0);
            this.detailbtm.Controls.SetChildIndex(this.txtuserConfirmed, 0);
            this.detailbtm.Controls.SetChildIndex(this.txtAccountNo, 0);
            this.detailbtm.Controls.SetChildIndex(this.labelAccountNo, 0);
            this.detailbtm.Controls.SetChildIndex(this.labelSettleVoucher, 0);
            this.detailbtm.Controls.SetChildIndex(this.displaySettleVoucher, 0);
            this.detailbtm.Controls.SetChildIndex(this.displayConfirmed, 0);
            this.detailbtm.Controls.SetChildIndex(this.displaySettleDate, 0);
            this.detailbtm.Controls.SetChildIndex(this.lbleditby, 0);
            this.detailbtm.Controls.SetChildIndex(this.lblcreateby, 0);
            this.detailbtm.Controls.SetChildIndex(this.editby, 0);
            this.detailbtm.Controls.SetChildIndex(this.createby, 0);
            this.detailbtm.Controls.SetChildIndex(this.refresh, 0);
            this.detailbtm.Controls.SetChildIndex(this.dateReceiveDate, 0);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1000, 599);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1008, 628);
            this.tabs.TabIndex = 0;
            // 
            // createby
            // 
            this.createby.Location = new System.Drawing.Point(70, 179);
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(479, 179);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lblcreateby
            // 
            this.lblcreateby.Location = new System.Drawing.Point(5, 185);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(431, 185);
            // 
            // labelSDNo
            // 
            this.labelSDNo.Lines = 0;
            this.labelSDNo.Location = new System.Drawing.Point(17, 9);
            this.labelSDNo.Name = "labelSDNo";
            this.labelSDNo.Size = new System.Drawing.Size(75, 23);
            this.labelSDNo.TabIndex = 1;
            this.labelSDNo.Text = "SD No. ";
            // 
            // lblTaipeiDebitNote
            // 
            this.lblTaipeiDebitNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTaipeiDebitNote.BackColor = System.Drawing.Color.Transparent;
            this.lblTaipeiDebitNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblTaipeiDebitNote.Lines = 0;
            this.lblTaipeiDebitNote.Location = new System.Drawing.Point(710, 9);
            this.lblTaipeiDebitNote.Name = "lblTaipeiDebitNote";
            this.lblTaipeiDebitNote.Size = new System.Drawing.Size(157, 23);
            this.lblTaipeiDebitNote.TabIndex = 46;
            this.lblTaipeiDebitNote.Text = "Taipei Debit Note";
            this.lblTaipeiDebitNote.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblStatus.Lines = 0;
            this.lblStatus.Location = new System.Drawing.Point(752, 41);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(115, 23);
            this.lblStatus.TabIndex = 45;
            this.lblStatus.Text = "Not Approve";
            this.lblStatus.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // displaySDNo
            // 
            this.displaySDNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySDNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displaySDNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySDNo.Location = new System.Drawing.Point(95, 9);
            this.displaySDNo.Name = "displaySDNo";
            this.displaySDNo.Size = new System.Drawing.Size(128, 23);
            this.displaySDNo.TabIndex = 0;
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Lines = 0;
            this.labelIssueDate.Location = new System.Drawing.Point(17, 41);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.Size = new System.Drawing.Size(75, 23);
            this.labelIssueDate.TabIndex = 48;
            this.labelIssueDate.Text = "Issue Date";
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "issuedate", true));
            this.dateIssueDate.IsSupportEditMode = false;
            this.dateIssueDate.Location = new System.Drawing.Point(95, 41);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.ReadOnly = true;
            this.dateIssueDate.Size = new System.Drawing.Size(130, 23);
            this.dateIssueDate.TabIndex = 2;
            this.dateIssueDate.TabStop = false;
            // 
            // labelHandle
            // 
            this.labelHandle.Lines = 0;
            this.labelHandle.Location = new System.Drawing.Point(279, 9);
            this.labelHandle.Name = "labelHandle";
            this.labelHandle.Size = new System.Drawing.Size(75, 23);
            this.labelHandle.TabIndex = 50;
            this.labelHandle.Text = "Handle";
            // 
            // labelSMR
            // 
            this.labelSMR.Lines = 0;
            this.labelSMR.Location = new System.Drawing.Point(279, 41);
            this.labelSMR.Name = "labelSMR";
            this.labelSMR.Size = new System.Drawing.Size(75, 23);
            this.labelSMR.TabIndex = 52;
            this.labelSMR.Text = "SMR";
            // 
            // datePrintDate
            // 
            this.datePrintDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "printdate", true));
            this.datePrintDate.IsSupportEditMode = false;
            this.datePrintDate.Location = new System.Drawing.Point(357, 76);
            this.datePrintDate.Name = "datePrintDate";
            this.datePrintDate.ReadOnly = true;
            this.datePrintDate.Size = new System.Drawing.Size(130, 23);
            this.datePrintDate.TabIndex = 5;
            this.datePrintDate.TabStop = false;
            // 
            // labelPrintDate
            // 
            this.labelPrintDate.Lines = 0;
            this.labelPrintDate.Location = new System.Drawing.Point(279, 76);
            this.labelPrintDate.Name = "labelPrintDate";
            this.labelPrintDate.Size = new System.Drawing.Size(75, 23);
            this.labelPrintDate.TabIndex = 54;
            this.labelPrintDate.Text = "Print Date";
            // 
            // dateStatusUpdate
            // 
            this.dateStatusUpdate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "statuseditdate", true));
            this.dateStatusUpdate.IsSupportEditMode = false;
            this.dateStatusUpdate.Location = new System.Drawing.Point(626, 76);
            this.dateStatusUpdate.Name = "dateStatusUpdate";
            this.dateStatusUpdate.ReadOnly = true;
            this.dateStatusUpdate.Size = new System.Drawing.Size(130, 23);
            this.dateStatusUpdate.TabIndex = 6;
            this.dateStatusUpdate.TabStop = false;
            // 
            // labelStatusUpdate
            // 
            this.labelStatusUpdate.Lines = 0;
            this.labelStatusUpdate.Location = new System.Drawing.Point(526, 76);
            this.labelStatusUpdate.Name = "labelStatusUpdate";
            this.labelStatusUpdate.Size = new System.Drawing.Size(97, 23);
            this.labelStatusUpdate.TabIndex = 56;
            this.labelStatusUpdate.Text = "Status Update";
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(17, 76);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(75, 23);
            this.labelFactory.TabIndex = 58;
            this.labelFactory.Text = "Factory";
            // 
            // displayFactory
            // 
            this.displayFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFactory.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "factoryid", true));
            this.displayFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFactory.Location = new System.Drawing.Point(95, 76);
            this.displayFactory.Name = "displayFactory";
            this.displayFactory.Size = new System.Drawing.Size(100, 23);
            this.displayFactory.TabIndex = 4;
            // 
            // btnDebitSchedule
            // 
            this.btnDebitSchedule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDebitSchedule.Location = new System.Drawing.Point(740, 76);
            this.btnDebitSchedule.Name = "btnDebitSchedule";
            this.btnDebitSchedule.Size = new System.Drawing.Size(127, 30);
            this.btnDebitSchedule.TabIndex = 8;
            this.btnDebitSchedule.Text = "Debit Schedule";
            this.btnDebitSchedule.UseVisualStyleBackColor = true;
            this.btnDebitSchedule.Click += new System.EventHandler(this.btnDebitSchedule_Click);
            // 
            // editDescription
            // 
            this.editDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(8, 6);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(380, 167);
            this.editDescription.TabIndex = 0;
            // 
            // labelSupplier
            // 
            this.labelSupplier.Lines = 0;
            this.labelSupplier.Location = new System.Drawing.Point(17, 110);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(75, 23);
            this.labelSupplier.TabIndex = 63;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelAmount
            // 
            this.labelAmount.Lines = 0;
            this.labelAmount.Location = new System.Drawing.Point(17, 145);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(75, 23);
            this.labelAmount.TabIndex = 64;
            this.labelAmount.Text = "Amount";
            // 
            // numAmount
            // 
            this.numAmount.BackColor = System.Drawing.Color.White;
            this.numAmount.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "amount", true));
            this.numAmount.DecimalPlaces = 2;
            this.numAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numAmount.Location = new System.Drawing.Point(95, 145);
            this.numAmount.MaxBytes = 12;
            this.numAmount.Name = "numAmount";
            this.numAmount.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAmount.Size = new System.Drawing.Size(100, 23);
            this.numAmount.TabIndex = 3;
            this.numAmount.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numTax
            // 
            this.numTax.BackColor = System.Drawing.Color.White;
            this.numTax.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "tax", true));
            this.numTax.DecimalPlaces = 2;
            this.numTax.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numTax.Location = new System.Drawing.Point(357, 145);
            this.numTax.MaxBytes = 11;
            this.numTax.Name = "numTax";
            this.numTax.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTax.Size = new System.Drawing.Size(100, 23);
            this.numTax.TabIndex = 5;
            this.numTax.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelTax
            // 
            this.labelTax.Lines = 0;
            this.labelTax.Location = new System.Drawing.Point(279, 145);
            this.labelTax.Name = "labelTax";
            this.labelTax.Size = new System.Drawing.Size(75, 23);
            this.labelTax.TabIndex = 66;
            this.labelTax.Text = "Tax";
            // 
            // labelTotalAmt
            // 
            this.labelTotalAmt.Lines = 0;
            this.labelTotalAmt.Location = new System.Drawing.Point(526, 145);
            this.labelTotalAmt.Name = "labelTotalAmt";
            this.labelTotalAmt.Size = new System.Drawing.Size(75, 23);
            this.labelTotalAmt.TabIndex = 69;
            this.labelTotalAmt.Text = "Total Amt.";
            // 
            // numTotalAmt
            // 
            this.numTotalAmt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalAmt.DecimalPlaces = 2;
            this.numTotalAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalAmt.IsSupportEditMode = false;
            this.numTotalAmt.Location = new System.Drawing.Point(604, 145);
            this.numTotalAmt.MaxBytes = 12;
            this.numTotalAmt.Name = "numTotalAmt";
            this.numTotalAmt.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalAmt.ReadOnly = true;
            this.numTotalAmt.Size = new System.Drawing.Size(103, 23);
            this.numTotalAmt.TabIndex = 12;
            this.numTotalAmt.TabStop = false;
            this.numTotalAmt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numtaxrate
            // 
            this.numtaxrate.BackColor = System.Drawing.Color.White;
            this.numtaxrate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "taxrate", true));
            this.numtaxrate.DecimalPlaces = 2;
            this.numtaxrate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numtaxrate.Location = new System.Drawing.Point(463, 145);
            this.numtaxrate.MaxBytes = 5;
            this.numtaxrate.Name = "numtaxrate";
            this.numtaxrate.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numtaxrate.Size = new System.Drawing.Size(60, 23);
            this.numtaxrate.TabIndex = 6;
            this.numtaxrate.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numExchange
            // 
            this.numExchange.BackColor = System.Drawing.Color.White;
            this.numExchange.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "exchange", true));
            this.numExchange.DecimalPlaces = 3;
            this.numExchange.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numExchange.Location = new System.Drawing.Point(95, 180);
            this.numExchange.MaxBytes = 8;
            this.numExchange.Name = "numExchange";
            this.numExchange.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numExchange.Size = new System.Drawing.Size(100, 23);
            this.numExchange.TabIndex = 4;
            this.numExchange.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelExchange
            // 
            this.labelExchange.Lines = 0;
            this.labelExchange.Location = new System.Drawing.Point(17, 180);
            this.labelExchange.Name = "labelExchange";
            this.labelExchange.Size = new System.Drawing.Size(75, 23);
            this.labelExchange.TabIndex = 72;
            this.labelExchange.Text = "Exchange";
            // 
            // labelAmtReceived
            // 
            this.labelAmtReceived.Lines = 0;
            this.labelAmtReceived.Location = new System.Drawing.Point(279, 180);
            this.labelAmtReceived.Name = "labelAmtReceived";
            this.labelAmtReceived.Size = new System.Drawing.Size(102, 23);
            this.labelAmtReceived.TabIndex = 74;
            this.labelAmtReceived.Text = "Amt. Received";
            // 
            // labelReceived
            // 
            this.labelReceived.Lines = 0;
            this.labelReceived.Location = new System.Drawing.Point(416, 16);
            this.labelReceived.Name = "labelReceived";
            this.labelReceived.Size = new System.Drawing.Size(75, 23);
            this.labelReceived.TabIndex = 62;
            this.labelReceived.Text = "Received";
            // 
            // labelConfirmed
            // 
            this.labelConfirmed.Lines = 0;
            this.labelConfirmed.Location = new System.Drawing.Point(416, 45);
            this.labelConfirmed.Name = "labelConfirmed";
            this.labelConfirmed.Size = new System.Drawing.Size(75, 23);
            this.labelConfirmed.TabIndex = 78;
            this.labelConfirmed.Text = "Confirmed";
            // 
            // labelAccountNo
            // 
            this.labelAccountNo.Lines = 0;
            this.labelAccountNo.Location = new System.Drawing.Point(416, 74);
            this.labelAccountNo.Name = "labelAccountNo";
            this.labelAccountNo.Size = new System.Drawing.Size(75, 23);
            this.labelAccountNo.TabIndex = 81;
            this.labelAccountNo.Text = "Account No";
            // 
            // labelSettleVoucher
            // 
            this.labelSettleVoucher.Lines = 0;
            this.labelSettleVoucher.Location = new System.Drawing.Point(416, 106);
            this.labelSettleVoucher.Name = "labelSettleVoucher";
            this.labelSettleVoucher.Size = new System.Drawing.Size(104, 23);
            this.labelSettleVoucher.TabIndex = 82;
            this.labelSettleVoucher.Text = "Settle Voucher";
            // 
            // displaySettleVoucher
            // 
            this.displaySettleVoucher.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySettleVoucher.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySettleVoucher.Location = new System.Drawing.Point(523, 106);
            this.displaySettleVoucher.Name = "displaySettleVoucher";
            this.displaySettleVoucher.Size = new System.Drawing.Size(174, 23);
            this.displaySettleVoucher.TabIndex = 6;
            // 
            // btnStatusHistory
            // 
            this.btnStatusHistory.Location = new System.Drawing.Point(762, 72);
            this.btnStatusHistory.Name = "btnStatusHistory";
            this.btnStatusHistory.Size = new System.Drawing.Size(22, 30);
            this.btnStatusHistory.TabIndex = 7;
            this.btnStatusHistory.TabStop = false;
            this.btnStatusHistory.Text = "H";
            this.btnStatusHistory.UseVisualStyleBackColor = true;
            this.btnStatusHistory.Click += new System.EventHandler(this.btnStatusHistory_Click);
            // 
            // displaycurrencyid
            // 
            this.displaycurrencyid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaycurrencyid.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "currencyid", true));
            this.displaycurrencyid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaycurrencyid.Location = new System.Drawing.Point(713, 145);
            this.displaycurrencyid.Name = "displaycurrencyid";
            this.displaycurrencyid.Size = new System.Drawing.Size(43, 23);
            this.displaycurrencyid.TabIndex = 13;
            // 
            // displayAmtReceived
            // 
            this.displayAmtReceived.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayAmtReceived.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayAmtReceived.Location = new System.Drawing.Point(690, 180);
            this.displayAmtReceived.Name = "displayAmtReceived";
            this.displayAmtReceived.Size = new System.Drawing.Size(169, 23);
            this.displayAmtReceived.TabIndex = 16;
            // 
            // displayConfirmed
            // 
            this.displayConfirmed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayConfirmed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayConfirmed.Location = new System.Drawing.Point(800, 45);
            this.displayConfirmed.Name = "displayConfirmed";
            this.displayConfirmed.Size = new System.Drawing.Size(107, 23);
            this.displayConfirmed.TabIndex = 4;
            // 
            // displaySettleDate
            // 
            this.displaySettleDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySettleDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySettleDate.Location = new System.Drawing.Point(703, 106);
            this.displaySettleDate.Name = "displaySettleDate";
            this.displaySettleDate.Size = new System.Drawing.Size(91, 23);
            this.displaySettleDate.TabIndex = 7;
            // 
            // dateReceiveDate
            // 
            this.dateReceiveDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "receiveDate", true));
            this.dateReceiveDate.IsSupportEditMode = false;
            this.dateReceiveDate.Location = new System.Drawing.Point(800, 16);
            this.dateReceiveDate.Name = "dateReceiveDate";
            this.dateReceiveDate.ReadOnly = true;
            this.dateReceiveDate.Size = new System.Drawing.Size(130, 23);
            this.dateReceiveDate.TabIndex = 83;
            // 
            // txtuserAmtReceived
            // 
            this.txtuserAmtReceived.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "amtrevisename", true));
            this.txtuserAmtReceived.DisplayBox1Binding = "";
            this.txtuserAmtReceived.Location = new System.Drawing.Point(384, 180);
            this.txtuserAmtReceived.Name = "txtuserAmtReceived";
            this.txtuserAmtReceived.Size = new System.Drawing.Size(300, 23);
            this.txtuserAmtReceived.TabIndex = 7;
            this.txtuserAmtReceived.TextBox1Binding = "";
            // 
            // txtsubconSupplier
            // 
            this.txtsubconSupplier.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "localsuppid", true));
            this.txtsubconSupplier.DisplayBox1Binding = "";
            this.txtsubconSupplier.IsIncludeJunk = false;
            this.txtsubconSupplier.Location = new System.Drawing.Point(95, 110);
            this.txtsubconSupplier.Name = "txtsubconSupplier";
            this.txtsubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtsubconSupplier.TabIndex = 2;
            this.txtsubconSupplier.TextBox1Binding = "";
            this.txtsubconSupplier.Validated += new System.EventHandler(this.txtsubconSupplier_Validated);
            // 
            // txtuserSMR
            // 
            this.txtuserSMR.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "SMR", true));
            this.txtuserSMR.DisplayBox1Binding = "";
            this.txtuserSMR.Location = new System.Drawing.Point(357, 41);
            this.txtuserSMR.Name = "txtuserSMR";
            this.txtuserSMR.Size = new System.Drawing.Size(300, 23);
            this.txtuserSMR.TabIndex = 1;
            this.txtuserSMR.TextBox1Binding = "";
            // 
            // txtuserHandle
            // 
            this.txtuserHandle.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "handle", true));
            this.txtuserHandle.DisplayBox1Binding = "";
            this.txtuserHandle.Location = new System.Drawing.Point(359, 9);
            this.txtuserHandle.Name = "txtuserHandle";
            this.txtuserHandle.Size = new System.Drawing.Size(300, 23);
            this.txtuserHandle.TabIndex = 0;
            this.txtuserHandle.TextBox1Binding = "";
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "AccountID", true));
            this.txtAccountNo.DisplayBox1Binding = "";
            this.txtAccountNo.Location = new System.Drawing.Point(494, 74);
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.Size = new System.Drawing.Size(300, 23);
            this.txtAccountNo.TabIndex = 3;
            this.txtAccountNo.TextBox1Binding = "";
            // 
            // txtuserConfirmed
            // 
            this.txtuserConfirmed.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "cfmName", true));
            this.txtuserConfirmed.DisplayBox1Binding = "";
            this.txtuserConfirmed.Location = new System.Drawing.Point(494, 45);
            this.txtuserConfirmed.Name = "txtuserConfirmed";
            this.txtuserConfirmed.Size = new System.Drawing.Size(300, 23);
            this.txtuserConfirmed.TabIndex = 2;
            this.txtuserConfirmed.TextBox1Binding = "";
            // 
            // txtuserReceived
            // 
            this.txtuserReceived.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "receiveName", true));
            this.txtuserReceived.DisplayBox1Binding = "";
            this.txtuserReceived.Location = new System.Drawing.Point(494, 16);
            this.txtuserReceived.Name = "txtuserReceived";
            this.txtuserReceived.Size = new System.Drawing.Size(300, 23);
            this.txtuserReceived.TabIndex = 1;
            this.txtuserReceived.TextBox1Binding = "";
            // 
            // P36
            // 
            this.ApvChkValue = "Received";
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.DefaultControl = "txtuserHandle";
            this.DefaultControlForEdit = "txtuserHandle";
            this.DefaultDetailOrder = "orderid";
            this.DefaultOrder = "IssueDate,ID";
            this.GridAlias = "LocalDebit_Detail";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportJunk = true;
            this.IsSupportRecall = true;
            this.IsSupportReceive = true;
            this.IsSupportReturn = true;
            this.IsSupportSend = true;
            this.IsSupportUnconfirm = true;
            this.JunkChkValue = "New";
            this.KeyField1 = "ID";
            this.Name = "P36";
            this.RecallChkValue = "Sent";
            this.ReceiveChkValue = "Sent";
            this.ReturnChkValue = "Received";
            this.SendChkValue = "New";
            this.Text = "P36.  Local Supplier Debit Note ";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "id";
            this.WorkAlias = "LocalDebit";
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
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSDNo;
        private Class.txtuser txtuserAmtReceived;
        private Win.UI.Label labelAmtReceived;
        private Win.UI.NumericBox numExchange;
        private Win.UI.Label labelExchange;
        private Win.UI.NumericBox numtaxrate;
        private Win.UI.NumericBox numTotalAmt;
        private Win.UI.Label labelTotalAmt;
        private Win.UI.NumericBox numTax;
        private Win.UI.Label labelTax;
        private Win.UI.NumericBox numAmount;
        private Win.UI.Label labelAmount;
        private Win.UI.Label labelSupplier;
        private Class.txtsubcon txtsubconSupplier;
        private Win.UI.Button btnDebitSchedule;
        private Win.UI.DisplayBox displayFactory;
        private Win.UI.Label labelFactory;
        private Win.UI.DateBox dateStatusUpdate;
        private Win.UI.Label labelStatusUpdate;
        private Win.UI.DateBox datePrintDate;
        private Win.UI.Label labelPrintDate;
        private Class.txtuser txtuserSMR;
        private Win.UI.Label labelSMR;
        private Class.txtuser txtuserHandle;
        private Win.UI.Label labelHandle;
        private Win.UI.DateBox dateIssueDate;
        private Win.UI.Label labelIssueDate;
        private Win.UI.DisplayBox displaySDNo;
        private Win.UI.Label lblTaipeiDebitNote;
        private Win.UI.Label lblStatus;
        private Class.txtuser txtuserConfirmed;
        private Win.UI.Label labelConfirmed;
        private Class.txtuser txtuserReceived;
        private Win.UI.Label labelReceived;
        private Win.UI.EditBox editDescription;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Class.txtAccountNo txtAccountNo;
        private Win.UI.DisplayBox displaySettleVoucher;
        private Win.UI.Label labelSettleVoucher;
        private Win.UI.Label labelAccountNo;
        private Win.UI.Button btnStatusHistory;
        private Win.UI.DisplayBox displayAmtReceived;
        private Win.UI.DisplayBox displaycurrencyid;
        private Win.UI.DisplayBox displaySettleDate;
        private Win.UI.DisplayBox displayConfirmed;
        private Win.UI.DateBox dateReceiveDate;
    }
}
