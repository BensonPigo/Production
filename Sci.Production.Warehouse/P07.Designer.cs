namespace Sci.Production.Warehouse
{
    partial class P07
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
            this.labelInvoiceNo = new Sci.Win.UI.Label();
            this.labelWKNO = new Sci.Win.UI.Label();
            this.labelETA = new Sci.Win.UI.Label();
            this.labelArriveWHDate = new Sci.Win.UI.Label();
            this.labelSendDate = new Sci.Win.UI.Label();
            this.labelPLRcvDate = new Sci.Win.UI.Label();
            this.labelDoxRcvDate = new Sci.Win.UI.Label();
            this.labelNotApprove = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.dateETA = new Sci.Win.UI.DateBox();
            this.txtInvoiceNo = new Sci.Win.UI.TextBox();
            this.displayWKNO = new Sci.Win.UI.DisplayBox();
            this.labelArrivePortDate = new Sci.Win.UI.Label();
            this.dateArrivePortDate = new Sci.Win.UI.DateBox();
            this.dateArriveWHDate = new Sci.Win.UI.DateBox();
            this.datePLRcvDate = new Sci.Win.UI.DateBox();
            this.dateDoxRcvDate = new Sci.Win.UI.DateBox();
            this.dateSendDate = new Sci.Win.UI.DateBox();
            this.btnModifyRollDyelot = new Sci.Win.UI.Button();
            this.btnDownloadSample = new Sci.Win.UI.Button();
            this.btnPrintSticker = new Sci.Win.UI.Button();
            this.btnAccumulatedQty = new Sci.Win.UI.Button();
            this.btnImportFromExcel = new Sci.Win.UI.Button();
            this.btnUpdateWeight = new Sci.Win.UI.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lbWhenWKNo = new Sci.Win.UI.Label();
            this.btnFind = new Sci.Win.UI.Button();
            this.txtLocateForSP = new Sci.Win.UI.TextBox();
            this.labelLocateForSP = new Sci.Win.UI.Label();
            this.labelTypeFilter = new Sci.Win.UI.Label();
            this.comboTypeFilter = new Sci.Win.UI.ComboBox();
            this.btnDeleteAll = new Sci.Win.UI.Button();
            this.check3rdCountry = new Sci.Win.UI.CheckBox();
            this.txtSeq1 = new Sci.Production.Class.TxtSeq();
            this.labSortBy = new Sci.Win.UI.Label();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioEncodeSeq = new Sci.Win.UI.RadioButton();
            this.radiobySP = new Sci.Win.UI.RadioButton();
            this.btnCallP99 = new Sci.Win.UI.Button();
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
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.btnCallP99);
            this.masterpanel.Controls.Add(this.radioPanel1);
            this.masterpanel.Controls.Add(this.labSortBy);
            this.masterpanel.Controls.Add(this.txtSeq1);
            this.masterpanel.Controls.Add(this.check3rdCountry);
            this.masterpanel.Controls.Add(this.btnDeleteAll);
            this.masterpanel.Controls.Add(this.labelTypeFilter);
            this.masterpanel.Controls.Add(this.comboTypeFilter);
            this.masterpanel.Controls.Add(this.btnFind);
            this.masterpanel.Controls.Add(this.txtLocateForSP);
            this.masterpanel.Controls.Add(this.labelLocateForSP);
            this.masterpanel.Controls.Add(this.lbWhenWKNo);
            this.masterpanel.Controls.Add(this.btnUpdateWeight);
            this.masterpanel.Controls.Add(this.btnImportFromExcel);
            this.masterpanel.Controls.Add(this.btnAccumulatedQty);
            this.masterpanel.Controls.Add(this.btnPrintSticker);
            this.masterpanel.Controls.Add(this.btnDownloadSample);
            this.masterpanel.Controls.Add(this.btnModifyRollDyelot);
            this.masterpanel.Controls.Add(this.dateSendDate);
            this.masterpanel.Controls.Add(this.dateDoxRcvDate);
            this.masterpanel.Controls.Add(this.datePLRcvDate);
            this.masterpanel.Controls.Add(this.dateArriveWHDate);
            this.masterpanel.Controls.Add(this.dateArrivePortDate);
            this.masterpanel.Controls.Add(this.labelArrivePortDate);
            this.masterpanel.Controls.Add(this.displayWKNO);
            this.masterpanel.Controls.Add(this.txtInvoiceNo);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.labelNotApprove);
            this.masterpanel.Controls.Add(this.labelDoxRcvDate);
            this.masterpanel.Controls.Add(this.labelPLRcvDate);
            this.masterpanel.Controls.Add(this.labelSendDate);
            this.masterpanel.Controls.Add(this.labelArriveWHDate);
            this.masterpanel.Controls.Add(this.labelETA);
            this.masterpanel.Controls.Add(this.labelWKNO);
            this.masterpanel.Controls.Add(this.labelInvoiceNo);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Controls.Add(this.dateETA);
            this.masterpanel.Controls.Add(this.shapeContainer1);
            this.masterpanel.Size = new System.Drawing.Size(1007, 198);
            this.masterpanel.Controls.SetChildIndex(this.shapeContainer1, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateETA, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelInvoiceNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelWKNO, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelETA, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelArriveWHDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSendDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPLRcvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDoxRcvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNotApprove, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtInvoiceNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayWKNO, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelArrivePortDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateArrivePortDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateArriveWHDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.datePLRcvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateDoxRcvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateSendDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnModifyRollDyelot, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnDownloadSample, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnPrintSticker, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnAccumulatedQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportFromExcel, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnUpdateWeight, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbWhenWKNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelLocateForSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLocateForSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnFind, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboTypeFilter, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTypeFilter, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnDeleteAll, 0);
            this.masterpanel.Controls.SetChildIndex(this.check3rdCountry, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSeq1, 0);
            this.masterpanel.Controls.SetChildIndex(this.labSortBy, 0);
            this.masterpanel.Controls.SetChildIndex(this.radioPanel1, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnCallP99, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 198);
            this.detailpanel.Size = new System.Drawing.Size(1007, 279);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.gridicon.Location = new System.Drawing.Point(902, 159);
            this.gridicon.TabIndex = 20;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(913, 0);
            this.refresh.TabIndex = 0;
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1007, 279);
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
            this.detail.Size = new System.Drawing.Size(1007, 515);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1007, 477);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 477);
            this.detailbtm.Size = new System.Drawing.Size(1007, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1007, 515);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1015, 544);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(484, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(436, 13);
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(3, 13);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 1;
            this.labelID.Text = "ID";
            // 
            // labelInvoiceNo
            // 
            this.labelInvoiceNo.Location = new System.Drawing.Point(3, 40);
            this.labelInvoiceNo.Name = "labelInvoiceNo";
            this.labelInvoiceNo.Size = new System.Drawing.Size(75, 23);
            this.labelInvoiceNo.TabIndex = 6;
            this.labelInvoiceNo.Text = "Invoice#";
            // 
            // labelWKNO
            // 
            this.labelWKNO.Location = new System.Drawing.Point(3, 67);
            this.labelWKNO.Name = "labelWKNO";
            this.labelWKNO.Size = new System.Drawing.Size(75, 23);
            this.labelWKNO.TabIndex = 7;
            this.labelWKNO.Text = "WK NO";
            // 
            // labelETA
            // 
            this.labelETA.Location = new System.Drawing.Point(233, 40);
            this.labelETA.Name = "labelETA";
            this.labelETA.Size = new System.Drawing.Size(108, 23);
            this.labelETA.TabIndex = 9;
            this.labelETA.Text = "ETA";
            // 
            // labelArriveWHDate
            // 
            this.labelArriveWHDate.Location = new System.Drawing.Point(233, 67);
            this.labelArriveWHDate.Name = "labelArriveWHDate";
            this.labelArriveWHDate.Size = new System.Drawing.Size(108, 23);
            this.labelArriveWHDate.TabIndex = 11;
            this.labelArriveWHDate.Text = "Arrive W/H Date";
            // 
            // labelSendDate
            // 
            this.labelSendDate.Location = new System.Drawing.Point(460, 67);
            this.labelSendDate.Name = "labelSendDate";
            this.labelSendDate.Size = new System.Drawing.Size(95, 23);
            this.labelSendDate.TabIndex = 14;
            this.labelSendDate.Text = "Send Date";
            // 
            // labelPLRcvDate
            // 
            this.labelPLRcvDate.Location = new System.Drawing.Point(460, 40);
            this.labelPLRcvDate.Name = "labelPLRcvDate";
            this.labelPLRcvDate.Size = new System.Drawing.Size(95, 23);
            this.labelPLRcvDate.TabIndex = 15;
            this.labelPLRcvDate.Text = "P/L Rcv Date";
            // 
            // labelDoxRcvDate
            // 
            this.labelDoxRcvDate.Location = new System.Drawing.Point(233, 94);
            this.labelDoxRcvDate.Name = "labelDoxRcvDate";
            this.labelDoxRcvDate.Size = new System.Drawing.Size(108, 23);
            this.labelDoxRcvDate.TabIndex = 18;
            this.labelDoxRcvDate.Text = "Dox Rcv Date";
            // 
            // labelNotApprove
            // 
            this.labelNotApprove.BackColor = System.Drawing.Color.Transparent;
            this.labelNotApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.labelNotApprove.Location = new System.Drawing.Point(692, 15);
            this.labelNotApprove.Name = "labelNotApprove";
            this.labelNotApprove.Size = new System.Drawing.Size(115, 23);
            this.labelNotApprove.TabIndex = 43;
            this.labelNotApprove.Text = "Not Approve";
            this.labelNotApprove.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(81, 13);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(120, 23);
            this.displayID.TabIndex = 0;
            // 
            // dateETA
            // 
            this.dateETA.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "eta", true));
            this.dateETA.Location = new System.Drawing.Point(345, 40);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(110, 23);
            this.dateETA.TabIndex = 3;
            // 
            // txtInvoiceNo
            // 
            this.txtInvoiceNo.BackColor = System.Drawing.Color.White;
            this.txtInvoiceNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "InvNo", true));
            this.txtInvoiceNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvoiceNo.Location = new System.Drawing.Point(81, 40);
            this.txtInvoiceNo.Name = "txtInvoiceNo";
            this.txtInvoiceNo.Size = new System.Drawing.Size(120, 23);
            this.txtInvoiceNo.TabIndex = 0;
            this.txtInvoiceNo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtInvoiceNo_Validating);
            // 
            // displayWKNO
            // 
            this.displayWKNO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayWKNO.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "exportid", true));
            this.displayWKNO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayWKNO.Location = new System.Drawing.Point(81, 67);
            this.displayWKNO.Name = "displayWKNO";
            this.displayWKNO.Size = new System.Drawing.Size(120, 23);
            this.displayWKNO.TabIndex = 1;
            // 
            // labelArrivePortDate
            // 
            this.labelArrivePortDate.Location = new System.Drawing.Point(3, 94);
            this.labelArrivePortDate.Name = "labelArrivePortDate";
            this.labelArrivePortDate.Size = new System.Drawing.Size(109, 23);
            this.labelArrivePortDate.TabIndex = 46;
            this.labelArrivePortDate.Text = "Arrive Port Date";
            // 
            // dateArrivePortDate
            // 
            this.dateArrivePortDate.IsSupportCalendar = false;
            this.dateArrivePortDate.IsSupportEditMode = false;
            this.dateArrivePortDate.Location = new System.Drawing.Point(116, 94);
            this.dateArrivePortDate.Name = "dateArrivePortDate";
            this.dateArrivePortDate.ReadOnly = true;
            this.dateArrivePortDate.Size = new System.Drawing.Size(110, 23);
            this.dateArrivePortDate.TabIndex = 2;
            // 
            // dateArriveWHDate
            // 
            this.dateArriveWHDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "whseArrival", true));
            this.dateArriveWHDate.Location = new System.Drawing.Point(345, 67);
            this.dateArriveWHDate.Name = "dateArriveWHDate";
            this.dateArriveWHDate.Size = new System.Drawing.Size(110, 23);
            this.dateArriveWHDate.TabIndex = 4;
            // 
            // datePLRcvDate
            // 
            this.datePLRcvDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PackingReceive", true));
            this.datePLRcvDate.Location = new System.Drawing.Point(560, 40);
            this.datePLRcvDate.Name = "datePLRcvDate";
            this.datePLRcvDate.Size = new System.Drawing.Size(110, 23);
            this.datePLRcvDate.TabIndex = 6;
            // 
            // dateDoxRcvDate
            // 
            this.dateDoxRcvDate.IsSupportCalendar = false;
            this.dateDoxRcvDate.IsSupportEditMode = false;
            this.dateDoxRcvDate.Location = new System.Drawing.Point(345, 94);
            this.dateDoxRcvDate.Name = "dateDoxRcvDate";
            this.dateDoxRcvDate.ReadOnly = true;
            this.dateDoxRcvDate.Size = new System.Drawing.Size(110, 23);
            this.dateDoxRcvDate.TabIndex = 5;
            // 
            // dateSendDate
            // 
            this.dateSendDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Transfer2Taipei", true));
            this.dateSendDate.IsSupportCalendar = false;
            this.dateSendDate.IsSupportEditMode = false;
            this.dateSendDate.Location = new System.Drawing.Point(560, 67);
            this.dateSendDate.Name = "dateSendDate";
            this.dateSendDate.ReadOnly = true;
            this.dateSendDate.Size = new System.Drawing.Size(110, 23);
            this.dateSendDate.TabIndex = 7;
            // 
            // btnModifyRollDyelot
            // 
            this.btnModifyRollDyelot.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnModifyRollDyelot.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnModifyRollDyelot.Location = new System.Drawing.Point(689, 50);
            this.btnModifyRollDyelot.Name = "btnModifyRollDyelot";
            this.btnModifyRollDyelot.Size = new System.Drawing.Size(122, 43);
            this.btnModifyRollDyelot.TabIndex = 9;
            this.btnModifyRollDyelot.Text = "Modify Roll & Dyelot";
            this.btnModifyRollDyelot.UseVisualStyleBackColor = true;
            this.btnModifyRollDyelot.Click += new System.EventHandler(this.BtModifyRollDyelot_Click);
            // 
            // btnDownloadSample
            // 
            this.btnDownloadSample.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnDownloadSample.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnDownloadSample.Location = new System.Drawing.Point(820, 100);
            this.btnDownloadSample.Name = "btnDownloadSample";
            this.btnDownloadSample.Size = new System.Drawing.Size(178, 30);
            this.btnDownloadSample.TabIndex = 12;
            this.btnDownloadSample.Text = "Download Sample File";
            this.btnDownloadSample.UseVisualStyleBackColor = true;
            this.btnDownloadSample.Click += new System.EventHandler(this.BtDownloadSample_Click);
            // 
            // btnPrintSticker
            // 
            this.btnPrintSticker.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnPrintSticker.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnPrintSticker.Location = new System.Drawing.Point(820, 50);
            this.btnPrintSticker.Name = "btnPrintSticker";
            this.btnPrintSticker.Size = new System.Drawing.Size(178, 43);
            this.btnPrintSticker.TabIndex = 10;
            this.btnPrintSticker.Text = "Print Fabric Sticker for SMS";
            this.btnPrintSticker.UseVisualStyleBackColor = true;
            this.btnPrintSticker.Click += new System.EventHandler(this.BtPrintSticker_Click);
            // 
            // btnAccumulatedQty
            // 
            this.btnAccumulatedQty.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnAccumulatedQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnAccumulatedQty.Location = new System.Drawing.Point(820, 12);
            this.btnAccumulatedQty.Name = "btnAccumulatedQty";
            this.btnAccumulatedQty.Size = new System.Drawing.Size(178, 31);
            this.btnAccumulatedQty.TabIndex = 8;
            this.btnAccumulatedQty.Text = "Accumulated Qty";
            this.btnAccumulatedQty.UseVisualStyleBackColor = true;
            this.btnAccumulatedQty.Click += new System.EventHandler(this.BtAccumulated_Click);
            // 
            // btnImportFromExcel
            // 
            this.btnImportFromExcel.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImportFromExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportFromExcel.Location = new System.Drawing.Point(746, 161);
            this.btnImportFromExcel.Name = "btnImportFromExcel";
            this.btnImportFromExcel.Size = new System.Drawing.Size(155, 30);
            this.btnImportFromExcel.TabIndex = 18;
            this.btnImportFromExcel.Text = "Import From Excel";
            this.btnImportFromExcel.UseVisualStyleBackColor = true;
            this.btnImportFromExcel.Click += new System.EventHandler(this.BtImportFromExcel_Click);
            // 
            // btnUpdateWeight
            // 
            this.btnUpdateWeight.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnUpdateWeight.Location = new System.Drawing.Point(689, 100);
            this.btnUpdateWeight.Name = "btnUpdateWeight";
            this.btnUpdateWeight.Size = new System.Drawing.Size(122, 30);
            this.btnUpdateWeight.TabIndex = 11;
            this.btnUpdateWeight.Text = "Update Weight";
            this.btnUpdateWeight.UseVisualStyleBackColor = true;
            this.btnUpdateWeight.Click += new System.EventHandler(this.BtUpdateWeight_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(1007, 198);
            this.shapeContainer1.TabIndex = 54;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 2;
            this.lineShape1.X2 = 1003;
            this.lineShape1.Y1 = 155;
            this.lineShape1.Y2 = 155;
            // 
            // lbWhenWKNo
            // 
            this.lbWhenWKNo.BackColor = System.Drawing.Color.Transparent;
            this.lbWhenWKNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbWhenWKNo.Location = new System.Drawing.Point(334, 13);
            this.lbWhenWKNo.Name = "lbWhenWKNo";
            this.lbWhenWKNo.Size = new System.Drawing.Size(338, 23);
            this.lbWhenWKNo.TabIndex = 55;
            this.lbWhenWKNo.Text = "(When \"WK No\" is empty. 3rd country will  be checked)";
            this.lbWhenWKNo.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(320, 161);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(81, 30);
            this.btnFind.TabIndex = 15;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtFind_Click);
            // 
            // txtLocateForSP
            // 
            this.txtLocateForSP.BackColor = System.Drawing.Color.White;
            this.txtLocateForSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateForSP.IsSupportEditMode = false;
            this.txtLocateForSP.Location = new System.Drawing.Point(115, 165);
            this.txtLocateForSP.Name = "txtLocateForSP";
            this.txtLocateForSP.Size = new System.Drawing.Size(135, 23);
            this.txtLocateForSP.TabIndex = 13;
            // 
            // labelLocateForSP
            // 
            this.labelLocateForSP.Location = new System.Drawing.Point(3, 165);
            this.labelLocateForSP.Name = "labelLocateForSP";
            this.labelLocateForSP.Size = new System.Drawing.Size(109, 23);
            this.labelLocateForSP.TabIndex = 58;
            this.labelLocateForSP.Text = "Locate for SP#";
            // 
            // labelTypeFilter
            // 
            this.labelTypeFilter.Location = new System.Drawing.Point(404, 165);
            this.labelTypeFilter.Name = "labelTypeFilter";
            this.labelTypeFilter.Size = new System.Drawing.Size(75, 23);
            this.labelTypeFilter.TabIndex = 16;
            this.labelTypeFilter.Text = "Type Filter";
            // 
            // comboTypeFilter
            // 
            this.comboTypeFilter.BackColor = System.Drawing.Color.White;
            this.comboTypeFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboTypeFilter.FormattingEnabled = true;
            this.comboTypeFilter.IsSupportUnselect = true;
            this.comboTypeFilter.Location = new System.Drawing.Point(481, 164);
            this.comboTypeFilter.Name = "comboTypeFilter";
            this.comboTypeFilter.OldText = "";
            this.comboTypeFilter.Size = new System.Drawing.Size(100, 24);
            this.comboTypeFilter.TabIndex = 16;
            this.comboTypeFilter.SelectedIndexChanged += new System.EventHandler(this.ComboTypeFilter_SelectedIndexChanged);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnDeleteAll.Location = new System.Drawing.Point(586, 161);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(156, 30);
            this.btnDeleteAll.TabIndex = 17;
            this.btnDeleteAll.Text = "Delete all fabric";
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.BtDeleteAllDetail_Click);
            // 
            // check3rdCountry
            // 
            this.check3rdCountry.AutoSize = true;
            this.check3rdCountry.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "third", true));
            this.check3rdCountry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.check3rdCountry.IsSupportEditMode = false;
            this.check3rdCountry.Location = new System.Drawing.Point(233, 15);
            this.check3rdCountry.Name = "check3rdCountry";
            this.check3rdCountry.ReadOnly = true;
            this.check3rdCountry.Size = new System.Drawing.Size(101, 21);
            this.check3rdCountry.TabIndex = 62;
            this.check3rdCountry.Text = "3rd Country";
            this.check3rdCountry.UseVisualStyleBackColor = true;
            // 
            // txtSeq1
            // 
            this.txtSeq1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq1.Location = new System.Drawing.Point(254, 165);
            this.txtSeq1.Name = "txtSeq1";
            this.txtSeq1.Seq1 = "";
            this.txtSeq1.Seq2 = "";
            this.txtSeq1.Size = new System.Drawing.Size(61, 23);
            this.txtSeq1.TabIndex = 14;
            // 
            // labSortBy
            // 
            this.labSortBy.Location = new System.Drawing.Point(3, 124);
            this.labSortBy.Name = "labSortBy";
            this.labSortBy.Size = new System.Drawing.Size(110, 23);
            this.labSortBy.TabIndex = 63;
            this.labSortBy.Text = "Sort by";
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioEncodeSeq);
            this.radioPanel1.Controls.Add(this.radiobySP);
            this.radioPanel1.IsSupportEditMode = false;
            this.radioPanel1.Location = new System.Drawing.Point(116, 120);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(339, 31);
            this.radioPanel1.TabIndex = 64;
            this.radioPanel1.Value = "1";
            this.radioPanel1.ValueChanged += new System.EventHandler(this.RadioPanel1_ValueChanged);
            // 
            // radioEncodeSeq
            // 
            this.radioEncodeSeq.AutoSize = true;
            this.radioEncodeSeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioEncodeSeq.Location = new System.Drawing.Point(182, 5);
            this.radioEncodeSeq.Name = "radioEncodeSeq";
            this.radioEncodeSeq.Size = new System.Drawing.Size(103, 21);
            this.radioEncodeSeq.TabIndex = 1;
            this.radioEncodeSeq.Text = "Encode Seq";
            this.radioEncodeSeq.UseVisualStyleBackColor = true;
            this.radioEncodeSeq.Value = "2";
            // 
            // radiobySP
            // 
            this.radiobySP.AutoSize = true;
            this.radiobySP.Checked = true;
            this.radiobySP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobySP.Location = new System.Drawing.Point(9, 5);
            this.radiobySP.Name = "radiobySP";
            this.radiobySP.Size = new System.Drawing.Size(165, 21);
            this.radiobySP.TabIndex = 0;
            this.radiobySP.TabStop = true;
            this.radiobySP.Text = "SP#, Seq, Roll, Dyelot";
            this.radiobySP.UseVisualStyleBackColor = true;
            this.radiobySP.Value = "1";
            // 
            // btnCallP99
            // 
            this.btnCallP99.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnCallP99.Location = new System.Drawing.Point(521, 117);
            this.btnCallP99.Name = "btnCallP99";
            this.btnCallP99.Size = new System.Drawing.Size(149, 30);
            this.btnCallP99.TabIndex = 65;
            this.btnCallP99.Text = "Revise Detail data";
            this.btnCallP99.UseVisualStyleBackColor = true;
            this.btnCallP99.Click += new System.EventHandler(this.BtnCallP99_Click);
            // 
            // P07
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1015, 577);
            this.DefaultControl = "txtInvoiceNo";
            this.DefaultControlForEdit = "txtInvoiceNo";
            this.DefaultDetailOrder = "poid,seq1,seq2,dyelot,roll";
            this.DefaultOrder = "ID";
            this.Grid2New = 0;
            this.GridAlias = "Receiving_detail";
            this.GridNew = 0;
            this.GridUniqueKey = "poid,seq1,seq2,roll,Dyelot";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P07";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P07. Material Receiving";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "id";
            this.WorkAlias = "Receiving";
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
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelDoxRcvDate;
        private Win.UI.Label labelPLRcvDate;
        private Win.UI.Label labelSendDate;
        private Win.UI.Label labelArriveWHDate;
        private Win.UI.Label labelETA;
        private Win.UI.Label labelWKNO;
        private Win.UI.Label labelInvoiceNo;
        private Win.UI.Label labelID;
        private Win.UI.DateBox dateETA;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label labelNotApprove;
        private Win.UI.Label lbWhenWKNo;
        private Win.UI.Button btnUpdateWeight;
        private Win.UI.Button btnImportFromExcel;
        private Win.UI.Button btnAccumulatedQty;
        private Win.UI.Button btnPrintSticker;
        private Win.UI.Button btnDownloadSample;
        private Win.UI.Button btnModifyRollDyelot;
        private Win.UI.DateBox dateSendDate;
        private Win.UI.DateBox dateDoxRcvDate;
        private Win.UI.DateBox datePLRcvDate;
        private Win.UI.DateBox dateArriveWHDate;
        private Win.UI.DateBox dateArrivePortDate;
        private Win.UI.Label labelArrivePortDate;
        private Win.UI.DisplayBox displayWKNO;
        private Win.UI.TextBox txtInvoiceNo;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.Button btnFind;
        private Win.UI.TextBox txtLocateForSP;
        private Win.UI.Label labelLocateForSP;
        private Win.UI.CheckBox check3rdCountry;
        private Win.UI.Button btnDeleteAll;
        private Win.UI.Label labelTypeFilter;
        private Win.UI.ComboBox comboTypeFilter;
        private Class.TxtSeq txtSeq1;
        private Win.UI.Label labSortBy;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioEncodeSeq;
        private Win.UI.RadioButton radiobySP;
        private Win.UI.Button btnCallP99;
    }
}
