using System.Drawing;

namespace Sci.Production.Basic
{
    partial class B04
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelCode = new Sci.Win.UI.Label();
            this.labelAbbreviation = new Sci.Win.UI.Label();
            this.labelNationality = new Sci.Win.UI.Label();
            this.labelCompany = new Sci.Win.UI.Label();
            this.labelTel = new Sci.Win.UI.Label();
            this.labelFax = new Sci.Win.UI.Label();
            this.labelAddress = new Sci.Win.UI.Label();
            this.labelCurrency = new Sci.Win.UI.Label();
            this.labelPaymentTerm = new Sci.Win.UI.Label();
            this.labelWHoldingRate = new Sci.Win.UI.Label();
            this.txtCode = new Sci.Win.UI.TextBox();
            this.txtAbbreviation = new Sci.Win.UI.TextBox();
            this.txtCompany = new Sci.Win.UI.TextBox();
            this.txtTel = new Sci.Win.UI.TextBox();
            this.txtFax = new Sci.Win.UI.TextBox();
            this.editAddress = new Sci.Win.UI.EditBox();
            this.checkUseSBTS = new Sci.Win.UI.CheckBox();
            this.checkIsFactory = new Sci.Win.UI.CheckBox();
            this.btnAccountingChartNo = new Sci.Win.UI.Button();
            this.btnBankDetail = new Sci.Win.UI.Button();
            this.txtpayterm_ftyPaymentTerm = new Sci.Production.Class.Txtpayterm_fty();
            this.txtCurrency = new Sci.Production.Class.Txtcurrency();
            this.txtCountryNationality = new Sci.Production.Class.Txtcountry();
            this.numWHoldingTerm = new Sci.Win.UI.NumericBox();
            this.chkIsShipping = new Sci.Win.UI.CheckBox();
            this.chkisSubcon = new Sci.Win.UI.CheckBox();
            this.chkisMisc = new Sci.Win.UI.CheckBox();
            this.chkIsSintexSubcon = new Sci.Win.UI.CheckBox();
            this.label1 = new Sci.Win.UI.Label();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.btnBatchApprove = new Sci.Win.UI.Button();
            this.gridBankDetail = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label2 = new Sci.Win.UI.Label();
            this.chkPayByChk = new Sci.Win.UI.CheckBox();
            this.label3 = new Sci.Win.UI.Label();
            this.txtEmail = new Sci.Win.UI.TextBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.chkIsFreightForwarder = new Sci.Win.UI.CheckBox();
            this.chkJunk = new Sci.Win.UI.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBankDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(945, 708);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.chkJunk);
            this.detailcont.Controls.Add(this.chkIsFreightForwarder);
            this.detailcont.Controls.Add(this.panel1);
            this.detailcont.Controls.Add(this.txtEmail);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.chkPayByChk);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.editRemark);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.chkIsSintexSubcon);
            this.detailcont.Controls.Add(this.chkisMisc);
            this.detailcont.Controls.Add(this.chkisSubcon);
            this.detailcont.Controls.Add(this.chkIsShipping);
            this.detailcont.Controls.Add(this.numWHoldingTerm);
            this.detailcont.Controls.Add(this.btnBankDetail);
            this.detailcont.Controls.Add(this.btnAccountingChartNo);
            this.detailcont.Controls.Add(this.checkIsFactory);
            this.detailcont.Controls.Add(this.checkUseSBTS);
            this.detailcont.Controls.Add(this.txtpayterm_ftyPaymentTerm);
            this.detailcont.Controls.Add(this.txtCurrency);
            this.detailcont.Controls.Add(this.txtCountryNationality);
            this.detailcont.Controls.Add(this.editAddress);
            this.detailcont.Controls.Add(this.txtFax);
            this.detailcont.Controls.Add(this.txtTel);
            this.detailcont.Controls.Add(this.txtCompany);
            this.detailcont.Controls.Add(this.txtAbbreviation);
            this.detailcont.Controls.Add(this.txtCode);
            this.detailcont.Controls.Add(this.labelWHoldingRate);
            this.detailcont.Controls.Add(this.labelPaymentTerm);
            this.detailcont.Controls.Add(this.labelCurrency);
            this.detailcont.Controls.Add(this.labelAddress);
            this.detailcont.Controls.Add(this.labelFax);
            this.detailcont.Controls.Add(this.labelTel);
            this.detailcont.Controls.Add(this.labelCompany);
            this.detailcont.Controls.Add(this.labelNationality);
            this.detailcont.Controls.Add(this.labelAbbreviation);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Size = new System.Drawing.Size(945, 670);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 670);
            this.detailbtm.Size = new System.Drawing.Size(945, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(945, 708);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(953, 737);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(28, 15);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(125, 23);
            this.labelCode.TabIndex = 0;
            this.labelCode.Text = "Code";
            // 
            // labelAbbreviation
            // 
            this.labelAbbreviation.Location = new System.Drawing.Point(28, 42);
            this.labelAbbreviation.Name = "labelAbbreviation";
            this.labelAbbreviation.Size = new System.Drawing.Size(125, 23);
            this.labelAbbreviation.TabIndex = 1;
            this.labelAbbreviation.Text = "Abbreviation";
            // 
            // labelNationality
            // 
            this.labelNationality.Location = new System.Drawing.Point(28, 69);
            this.labelNationality.Name = "labelNationality";
            this.labelNationality.Size = new System.Drawing.Size(125, 23);
            this.labelNationality.TabIndex = 2;
            this.labelNationality.Text = "Nationality";
            // 
            // labelCompany
            // 
            this.labelCompany.Location = new System.Drawing.Point(28, 96);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(125, 23);
            this.labelCompany.TabIndex = 3;
            this.labelCompany.Text = "Company";
            // 
            // labelTel
            // 
            this.labelTel.Location = new System.Drawing.Point(28, 123);
            this.labelTel.Name = "labelTel";
            this.labelTel.Size = new System.Drawing.Size(125, 23);
            this.labelTel.TabIndex = 4;
            this.labelTel.Text = "Tel";
            // 
            // labelFax
            // 
            this.labelFax.Location = new System.Drawing.Point(28, 150);
            this.labelFax.Name = "labelFax";
            this.labelFax.Size = new System.Drawing.Size(125, 23);
            this.labelFax.TabIndex = 5;
            this.labelFax.Text = "Fax";
            // 
            // labelAddress
            // 
            this.labelAddress.Location = new System.Drawing.Point(28, 177);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(125, 23);
            this.labelAddress.TabIndex = 6;
            this.labelAddress.Text = "Address";
            // 
            // labelCurrency
            // 
            this.labelCurrency.Location = new System.Drawing.Point(28, 293);
            this.labelCurrency.Name = "labelCurrency";
            this.labelCurrency.Size = new System.Drawing.Size(125, 23);
            this.labelCurrency.TabIndex = 7;
            this.labelCurrency.Text = "Currency";
            // 
            // labelPaymentTerm
            // 
            this.labelPaymentTerm.Location = new System.Drawing.Point(28, 320);
            this.labelPaymentTerm.Name = "labelPaymentTerm";
            this.labelPaymentTerm.Size = new System.Drawing.Size(125, 23);
            this.labelPaymentTerm.TabIndex = 8;
            this.labelPaymentTerm.Text = "Payment Term";
            // 
            // labelWHoldingRate
            // 
            this.labelWHoldingRate.Location = new System.Drawing.Point(28, 347);
            this.labelWHoldingRate.Name = "labelWHoldingRate";
            this.labelWHoldingRate.Size = new System.Drawing.Size(125, 23);
            this.labelWHoldingRate.TabIndex = 9;
            this.labelWHoldingRate.Text = "W/Holding Rate(%)";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCode.Location = new System.Drawing.Point(156, 15);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(66, 23);
            this.txtCode.TabIndex = 0;
            // 
            // txtAbbreviation
            // 
            this.txtAbbreviation.BackColor = System.Drawing.Color.White;
            this.txtAbbreviation.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Abb", true));
            this.txtAbbreviation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtAbbreviation.Location = new System.Drawing.Point(156, 42);
            this.txtAbbreviation.Name = "txtAbbreviation";
            this.txtAbbreviation.Size = new System.Drawing.Size(120, 23);
            this.txtAbbreviation.TabIndex = 1;
            // 
            // txtCompany
            // 
            this.txtCompany.BackColor = System.Drawing.Color.White;
            this.txtCompany.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtCompany.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Name", true));
            this.txtCompany.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCompany.Location = new System.Drawing.Point(156, 96);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(317, 23);
            this.txtCompany.TabIndex = 3;
            // 
            // txtTel
            // 
            this.txtTel.BackColor = System.Drawing.Color.White;
            this.txtTel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Tel", true));
            this.txtTel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTel.Location = new System.Drawing.Point(156, 123);
            this.txtTel.Name = "txtTel";
            this.txtTel.Size = new System.Drawing.Size(220, 23);
            this.txtTel.TabIndex = 4;
            // 
            // txtFax
            // 
            this.txtFax.BackColor = System.Drawing.Color.White;
            this.txtFax.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Fax", true));
            this.txtFax.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFax.Location = new System.Drawing.Point(156, 150);
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(220, 23);
            this.txtFax.TabIndex = 5;
            // 
            // editAddress
            // 
            this.editAddress.BackColor = System.Drawing.Color.White;
            this.editAddress.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Address", true));
            this.editAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editAddress.Location = new System.Drawing.Point(156, 177);
            this.editAddress.Multiline = true;
            this.editAddress.Name = "editAddress";
            this.editAddress.Size = new System.Drawing.Size(400, 82);
            this.editAddress.TabIndex = 6;
            // 
            // checkUseSBTS
            // 
            this.checkUseSBTS.AutoSize = true;
            this.checkUseSBTS.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "UseSBTS", true));
            this.checkUseSBTS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkUseSBTS.Location = new System.Drawing.Point(585, 21);
            this.checkUseSBTS.Name = "checkUseSBTS";
            this.checkUseSBTS.Size = new System.Drawing.Size(92, 21);
            this.checkUseSBTS.TabIndex = 12;
            this.checkUseSBTS.Text = "Use SBTS";
            this.checkUseSBTS.UseVisualStyleBackColor = true;
            // 
            // checkIsFactory
            // 
            this.checkIsFactory.AutoSize = true;
            this.checkIsFactory.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsFactory", true));
            this.checkIsFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIsFactory.Location = new System.Drawing.Point(585, 48);
            this.checkIsFactory.Name = "checkIsFactory";
            this.checkIsFactory.Size = new System.Drawing.Size(88, 21);
            this.checkIsFactory.TabIndex = 13;
            this.checkIsFactory.Text = "Is Factory";
            this.checkIsFactory.UseVisualStyleBackColor = true;
            // 
            // btnAccountingChartNo
            // 
            this.btnAccountingChartNo.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnAccountingChartNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnAccountingChartNo.Location = new System.Drawing.Point(761, 15);
            this.btnAccountingChartNo.Name = "btnAccountingChartNo";
            this.btnAccountingChartNo.Size = new System.Drawing.Size(163, 30);
            this.btnAccountingChartNo.TabIndex = 18;
            this.btnAccountingChartNo.Text = "Accounting chart no";
            this.btnAccountingChartNo.UseVisualStyleBackColor = true;
            this.btnAccountingChartNo.Click += new System.EventHandler(this.BtnAccountingChartNo_Click);
            // 
            // btnBankDetail
            // 
            this.btnBankDetail.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnBankDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBankDetail.Location = new System.Drawing.Point(761, 52);
            this.btnBankDetail.Name = "btnBankDetail";
            this.btnBankDetail.Size = new System.Drawing.Size(163, 30);
            this.btnBankDetail.TabIndex = 19;
            this.btnBankDetail.Text = "Bank detail";
            this.btnBankDetail.UseVisualStyleBackColor = true;
            this.btnBankDetail.Click += new System.EventHandler(this.BtnBankDetail_Click);
            // 
            // txtpayterm_ftyPaymentTerm
            // 
            this.txtpayterm_ftyPaymentTerm.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "PayTermID", true));
            this.txtpayterm_ftyPaymentTerm.DisplayBox1Binding = "";
            this.txtpayterm_ftyPaymentTerm.Location = new System.Drawing.Point(156, 320);
            this.txtpayterm_ftyPaymentTerm.Name = "txtpayterm_ftyPaymentTerm";
            this.txtpayterm_ftyPaymentTerm.Size = new System.Drawing.Size(384, 23);
            this.txtpayterm_ftyPaymentTerm.TabIndex = 9;
            this.txtpayterm_ftyPaymentTerm.TextBox1Binding = "";
            // 
            // txtCurrency
            // 
            this.txtCurrency.BackColor = System.Drawing.Color.White;
            this.txtCurrency.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CurrencyID", true));
            this.txtCurrency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCurrency.IsSupportSytsemContextMenu = false;
            this.txtCurrency.Location = new System.Drawing.Point(156, 293);
            this.txtCurrency.Name = "txtCurrency";
            this.txtCurrency.Size = new System.Drawing.Size(40, 23);
            this.txtCurrency.TabIndex = 8;
            // 
            // txtCountryNationality
            // 
            this.txtCountryNationality.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "CountryID", true));
            this.txtCountryNationality.DisplayBox1Binding = "";
            this.txtCountryNationality.Location = new System.Drawing.Point(156, 69);
            this.txtCountryNationality.Name = "txtCountryNationality";
            this.txtCountryNationality.Size = new System.Drawing.Size(232, 22);
            this.txtCountryNationality.TabIndex = 2;
            this.txtCountryNationality.TextBox1Binding = "";
            // 
            // numWHoldingTerm
            // 
            this.numWHoldingTerm.BackColor = System.Drawing.Color.White;
            this.numWHoldingTerm.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "WithHoldingRate", true));
            this.numWHoldingTerm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numWHoldingTerm.Location = new System.Drawing.Point(156, 347);
            this.numWHoldingTerm.Name = "numWHoldingTerm";
            this.numWHoldingTerm.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWHoldingTerm.Size = new System.Drawing.Size(40, 23);
            this.numWHoldingTerm.TabIndex = 10;
            this.numWHoldingTerm.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // chkIsShipping
            // 
            this.chkIsShipping.AutoSize = true;
            this.chkIsShipping.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsShipping", true));
            this.chkIsShipping.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsShipping.Location = new System.Drawing.Point(585, 75);
            this.chkIsShipping.Name = "chkIsShipping";
            this.chkIsShipping.Size = new System.Drawing.Size(152, 21);
            this.chkIsShipping.TabIndex = 14;
            this.chkIsShipping.Text = "Is Shipping Supplier";
            this.chkIsShipping.UseVisualStyleBackColor = true;
            // 
            // chkisSubcon
            // 
            this.chkisSubcon.AutoSize = true;
            this.chkisSubcon.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsSubcon", true));
            this.chkisSubcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkisSubcon.Location = new System.Drawing.Point(585, 102);
            this.chkisSubcon.Name = "chkisSubcon";
            this.chkisSubcon.Size = new System.Drawing.Size(145, 21);
            this.chkisSubcon.TabIndex = 15;
            this.chkisSubcon.Text = "Is Subcon Supplier";
            this.chkisSubcon.UseVisualStyleBackColor = true;
            // 
            // chkisMisc
            // 
            this.chkisMisc.AutoSize = true;
            this.chkisMisc.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsMisc", true));
            this.chkisMisc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkisMisc.Location = new System.Drawing.Point(585, 129);
            this.chkisMisc.Name = "chkisMisc";
            this.chkisMisc.Size = new System.Drawing.Size(186, 21);
            this.chkisMisc.TabIndex = 16;
            this.chkisMisc.Text = "Is Miscellaneous Supplier";
            this.chkisMisc.UseVisualStyleBackColor = true;
            // 
            // chkIsSintexSubcon
            // 
            this.chkIsSintexSubcon.AutoSize = true;
            this.chkIsSintexSubcon.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsSintexSubcon", true));
            this.chkIsSintexSubcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsSintexSubcon.Location = new System.Drawing.Point(585, 156);
            this.chkIsSintexSubcon.Name = "chkIsSintexSubcon";
            this.chkIsSintexSubcon.Size = new System.Drawing.Size(131, 21);
            this.chkIsSintexSubcon.TabIndex = 17;
            this.chkIsSintexSubcon.Text = "Is Sintex Subcon";
            this.chkIsSintexSubcon.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(28, 373);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 23);
            this.label1.TabIndex = 19;
            this.label1.Text = "Remark";
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(156, 376);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(400, 82);
            this.editRemark.TabIndex = 11;
            // 
            // btnBatchApprove
            // 
            this.btnBatchApprove.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnBatchApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBatchApprove.Location = new System.Drawing.Point(824, 3);
            this.btnBatchApprove.Name = "btnBatchApprove";
            this.btnBatchApprove.Size = new System.Drawing.Size(122, 30);
            this.btnBatchApprove.TabIndex = 21;
            this.btnBatchApprove.Text = "Batch Approve";
            this.btnBatchApprove.UseVisualStyleBackColor = true;
            this.btnBatchApprove.Click += new System.EventHandler(this.btnBatchApprove_Click);
            // 
            // gridBankDetail
            // 
            this.gridBankDetail.AllowUserToAddRows = false;
            this.gridBankDetail.AllowUserToDeleteRows = false;
            this.gridBankDetail.AllowUserToResizeRows = false;
            this.gridBankDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBankDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBankDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBankDetail.DataSource = this.listControlBindingSource1;
            this.gridBankDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBankDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBankDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBankDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBankDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBankDetail.Location = new System.Drawing.Point(0, 0);
            this.gridBankDetail.Name = "gridBankDetail";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridBankDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridBankDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBankDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBankDetail.RowTemplate.Height = 24;
            this.gridBankDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBankDetail.ShowCellToolTips = false;
            this.gridBankDetail.Size = new System.Drawing.Size(896, 146);
            this.gridBankDetail.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(28, 469);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 23);
            this.label2.TabIndex = 210;
            this.label2.Text = "Bank Detail";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // chkPayByChk
            // 
            this.chkPayByChk.AutoSize = true;
            this.chkPayByChk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsSintexSubcon", true));
            this.chkPayByChk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.chkPayByChk.IsSupportEditMode = false;
            this.chkPayByChk.Location = new System.Drawing.Point(156, 469);
            this.chkPayByChk.Name = "chkPayByChk";
            this.chkPayByChk.ReadOnly = true;
            this.chkPayByChk.Size = new System.Drawing.Size(114, 21);
            this.chkPayByChk.TabIndex = 99;
            this.chkPayByChk.Text = "Pay By Check";
            this.chkPayByChk.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(28, 263);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 23);
            this.label3.TabIndex = 211;
            this.label3.Text = "E-mail";
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.White;
            this.txtEmail.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtEmail.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Email", true));
            this.txtEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtEmail.Location = new System.Drawing.Point(156, 264);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(317, 23);
            this.txtEmail.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.gridBankDetail);
            this.panel1.Location = new System.Drawing.Point(28, 509);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(896, 146);
            this.panel1.TabIndex = 212;
            // 
            // chkIsFreightForwarder
            // 
            this.chkIsFreightForwarder.AutoSize = true;
            this.chkIsFreightForwarder.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsFreightForwarder", true));
            this.chkIsFreightForwarder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsFreightForwarder.Location = new System.Drawing.Point(585, 183);
            this.chkIsFreightForwarder.Name = "chkIsFreightForwarder";
            this.chkIsFreightForwarder.Size = new System.Drawing.Size(153, 21);
            this.chkIsFreightForwarder.TabIndex = 18;
            this.chkIsFreightForwarder.Text = "Is Freight Forwarder";
            this.chkIsFreightForwarder.UseVisualStyleBackColor = true;
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.chkJunk.IsSupportEditMode = false;
            this.chkJunk.Location = new System.Drawing.Point(325, 17);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.ReadOnly = true;
            this.chkJunk.Size = new System.Drawing.Size(57, 21);
            this.chkJunk.TabIndex = 213;
            this.chkJunk.Text = "Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // B04
            // 
            this.ClientSize = new System.Drawing.Size(953, 770);
            this.Controls.Add(this.btnBatchApprove);
            this.DefaultControl = "txtCode";
            this.DefaultControlForEdit = "txtTel";
            this.DefaultOrder = "ID";
            this.EnableGridJunkColor = true;
            this.ExpressQuery = true;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportJunk = true;
            this.IsSupportPrint = false;
            this.IsSupportUnJunk = true;
            this.JunkChkValue = "New";
            this.Name = "B04";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B04. Supplier/Sub Con (Local)";
            this.UniqueExpress = "ID";
            this.UnjunkChkValue = "Junked";
            this.WorkAlias = "LocalSupp";
            this.Load += new System.EventHandler(this.B04_FormLoaded);
            this.Controls.SetChildIndex(this.btnBatchApprove, 0);
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBankDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelCode;
        private Class.Txtpayterm_fty txtpayterm_ftyPaymentTerm;
        private Class.Txtcurrency txtCurrency;
        private Class.Txtcountry txtCountryNationality;
        private Win.UI.EditBox editAddress;
        private Win.UI.TextBox txtFax;
        private Win.UI.TextBox txtTel;
        private Win.UI.TextBox txtCompany;
        private Win.UI.TextBox txtAbbreviation;
        private Win.UI.TextBox txtCode;
        private Win.UI.Label labelWHoldingRate;
        private Win.UI.Label labelPaymentTerm;
        private Win.UI.Label labelCurrency;
        private Win.UI.Label labelAddress;
        private Win.UI.Label labelFax;
        private Win.UI.Label labelTel;
        private Win.UI.Label labelCompany;
        private Win.UI.Label labelNationality;
        private Win.UI.Label labelAbbreviation;
        private Win.UI.Button btnBankDetail;
        private Win.UI.Button btnAccountingChartNo;
        private Win.UI.CheckBox checkIsFactory;
        private Win.UI.CheckBox checkUseSBTS;
        private Win.UI.NumericBox numWHoldingTerm;
        private Win.UI.CheckBox chkisMisc;
        private Win.UI.CheckBox chkisSubcon;
        private Win.UI.CheckBox chkIsShipping;
        private Win.UI.CheckBox chkIsSintexSubcon;
        private Win.UI.EditBox editRemark;
        private Win.UI.Label label1;
        private Win.UI.Button btnBatchApprove;
        private Win.UI.Grid gridBankDetail;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label label2;
        private Win.UI.CheckBox chkPayByChk;
        private Win.UI.TextBox txtEmail;
        private Win.UI.Label label3;
        private Win.UI.Panel panel1;
        private Win.UI.CheckBox chkIsFreightForwarder;
        private Win.UI.CheckBox chkJunk;
    }
}
