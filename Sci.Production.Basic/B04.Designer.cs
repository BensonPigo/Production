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
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.checkUseSBTS = new Sci.Win.UI.CheckBox();
            this.checkIsFactory = new Sci.Win.UI.CheckBox();
            this.btnAccountingChartNo = new Sci.Win.UI.Button();
            this.btnBankDetail = new Sci.Win.UI.Button();
            this.txtpayterm_ftyPaymentTerm = new Sci.Production.Class.txtpayterm_fty();
            this.txtCurrency = new Sci.Production.Class.txtcurrency();
            this.txtCountryNationality = new Sci.Production.Class.txtcountry();
            this.numWHoldingTerm = new Sci.Win.UI.NumericBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(824, 395);
            // 
            // detailcont
            //
            this.detailcont.Controls.Add(this.numWHoldingTerm);
            this.detailcont.Controls.Add(this.btnBankDetail);
            this.detailcont.Controls.Add(this.btnAccountingChartNo);
            this.detailcont.Controls.Add(this.checkIsFactory);
            this.detailcont.Controls.Add(this.checkUseSBTS);
            this.detailcont.Controls.Add(this.checkJunk);
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
            this.detailcont.Size = new System.Drawing.Size(824, 357);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(824, 38);
            //  
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(824, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(832, 424);
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
            this.labelCode.Lines = 0;
            this.labelCode.Location = new System.Drawing.Point(28, 15);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(125, 23);
            this.labelCode.TabIndex = 0;
            this.labelCode.Text = "Code";
            // 
            // labelAbbreviation
            // 
            this.labelAbbreviation.Lines = 0;
            this.labelAbbreviation.Location = new System.Drawing.Point(28, 42);
            this.labelAbbreviation.Name = "labelAbbreviation";
            this.labelAbbreviation.Size = new System.Drawing.Size(125, 23);
            this.labelAbbreviation.TabIndex = 1;
            this.labelAbbreviation.Text = "Abbreviation";
            // 
            // labelNationality
            // 
            this.labelNationality.Lines = 0;
            this.labelNationality.Location = new System.Drawing.Point(28, 69);
            this.labelNationality.Name = "labelNationality";
            this.labelNationality.Size = new System.Drawing.Size(125, 23);
            this.labelNationality.TabIndex = 2;
            this.labelNationality.Text = "Nationality";
            // 
            // labelCompany
            // 
            this.labelCompany.Lines = 0;
            this.labelCompany.Location = new System.Drawing.Point(28, 96);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(125, 23);
            this.labelCompany.TabIndex = 3;
            this.labelCompany.Text = "Company";
            // 
            // labelTel
            // 
            this.labelTel.Lines = 0;
            this.labelTel.Location = new System.Drawing.Point(28, 123);
            this.labelTel.Name = "labelTel";
            this.labelTel.Size = new System.Drawing.Size(125, 23);
            this.labelTel.TabIndex = 4;
            this.labelTel.Text = "Tel";
            // 
            // labelFax
            // 
            this.labelFax.Lines = 0;
            this.labelFax.Location = new System.Drawing.Point(28, 150);
            this.labelFax.Name = "labelFax";
            this.labelFax.Size = new System.Drawing.Size(125, 23);
            this.labelFax.TabIndex = 5;
            this.labelFax.Text = "Fax";
            // 
            // labelAddress
            // 
            this.labelAddress.Lines = 0;
            this.labelAddress.Location = new System.Drawing.Point(28, 177);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(125, 23);
            this.labelAddress.TabIndex = 6;
            this.labelAddress.Text = "Address";
            // 
            // labelCurrency
            // 
            this.labelCurrency.Lines = 0;
            this.labelCurrency.Location = new System.Drawing.Point(28, 263);
            this.labelCurrency.Name = "labelCurrency";
            this.labelCurrency.Size = new System.Drawing.Size(125, 23);
            this.labelCurrency.TabIndex = 7;
            this.labelCurrency.Text = "Currency";
            // 
            // labelPaymentTerm
            // 
            this.labelPaymentTerm.Lines = 0;
            this.labelPaymentTerm.Location = new System.Drawing.Point(28, 290);
            this.labelPaymentTerm.Name = "labelPaymentTerm";
            this.labelPaymentTerm.Size = new System.Drawing.Size(125, 23);
            this.labelPaymentTerm.TabIndex = 8;
            this.labelPaymentTerm.Text = "Payment Term";
            // 
            // labelWHoldingRate
            // 
            this.labelWHoldingRate.Lines = 0;
            this.labelWHoldingRate.Location = new System.Drawing.Point(28, 317);
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
            this.txtAbbreviation.TabIndex = 2;
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
            this.txtCompany.TabIndex = 6;
            // 
            // txtTel
            // 
            this.txtTel.BackColor = System.Drawing.Color.White;
            this.txtTel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Tel", true));
            this.txtTel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTel.Location = new System.Drawing.Point(156, 123);
            this.txtTel.Name = "txtTel";
            this.txtTel.Size = new System.Drawing.Size(220, 23);
            this.txtTel.TabIndex = 7;
            // 
            // txtFax
            // 
            this.txtFax.BackColor = System.Drawing.Color.White;
            this.txtFax.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Fax", true));
            this.txtFax.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFax.Location = new System.Drawing.Point(156, 150);
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(220, 23);
            this.txtFax.TabIndex = 8;
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
            this.editAddress.TabIndex = 9;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(478, 15);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 1;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // checkUseSBTS
            // 
            this.checkUseSBTS.AutoSize = true;
            this.checkUseSBTS.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "UseSBTS", true));
            this.checkUseSBTS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkUseSBTS.Location = new System.Drawing.Point(478, 42);
            this.checkUseSBTS.Name = "checkUseSBTS";
            this.checkUseSBTS.Size = new System.Drawing.Size(92, 21);
            this.checkUseSBTS.TabIndex = 3;
            this.checkUseSBTS.Text = "Use SBTS";
            this.checkUseSBTS.UseVisualStyleBackColor = true;
            // 
            // checkIsFactory
            // 
            this.checkIsFactory.AutoSize = true;
            this.checkIsFactory.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsFactory", true));
            this.checkIsFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIsFactory.Location = new System.Drawing.Point(478, 69);
            this.checkIsFactory.Name = "checkIsFactory";
            this.checkIsFactory.Size = new System.Drawing.Size(88, 21);
            this.checkIsFactory.TabIndex = 5;
            this.checkIsFactory.Text = "Is Factory";
            this.checkIsFactory.UseVisualStyleBackColor = true;
            // 
            // btnAccountingChartNo
            // 
            this.btnAccountingChartNo.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnAccountingChartNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnAccountingChartNo.Location = new System.Drawing.Point(589, 15);
            this.btnAccountingChartNo.Name = "btnAccountingChartNo";
            this.btnAccountingChartNo.Size = new System.Drawing.Size(163, 30);
            this.btnAccountingChartNo.TabIndex = 13;
            this.btnAccountingChartNo.Text = "Accounting chart no";
            this.btnAccountingChartNo.UseVisualStyleBackColor = true;
            this.btnAccountingChartNo.Click += new System.EventHandler(this.BtnAccountingChartNo_Click);
            // 
            // btnBankDetail
            // 
            this.btnBankDetail.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnBankDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBankDetail.Location = new System.Drawing.Point(589, 52);
            this.btnBankDetail.Name = "btnBankDetail";
            this.btnBankDetail.Size = new System.Drawing.Size(163, 30);
            this.btnBankDetail.TabIndex = 14;
            this.btnBankDetail.Text = "Bank detail";
            this.btnBankDetail.UseVisualStyleBackColor = true;
            this.btnBankDetail.Click += new System.EventHandler(this.BtnBankDetail_Click);
            // 
            // txtpayterm_ftyPaymentTerm
            // 
            this.txtpayterm_ftyPaymentTerm.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "PayTermID", true));
            this.txtpayterm_ftyPaymentTerm.DisplayBox1Binding = "";
            this.txtpayterm_ftyPaymentTerm.Location = new System.Drawing.Point(156, 290);
            this.txtpayterm_ftyPaymentTerm.Name = "txtpayterm_ftyPaymentTerm";
            this.txtpayterm_ftyPaymentTerm.Size = new System.Drawing.Size(384, 23);
            this.txtpayterm_ftyPaymentTerm.TabIndex = 11;
            this.txtpayterm_ftyPaymentTerm.TextBox1Binding = "";
            // 
            // txtCurrency
            // 
            this.txtCurrency.BackColor = System.Drawing.Color.White;
            this.txtCurrency.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CurrencyID", true));
            this.txtCurrency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCurrency.IsSupportSytsemContextMenu = false;
            this.txtCurrency.Location = new System.Drawing.Point(156, 263);
            this.txtCurrency.Name = "txtCurrency";
            this.txtCurrency.Size = new System.Drawing.Size(40, 23);
            this.txtCurrency.TabIndex = 10;
            // 
            // txtCountryNationality
            // 
            this.txtCountryNationality.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "CountryID", true));
            this.txtCountryNationality.DisplayBox1Binding = "";
            this.txtCountryNationality.Location = new System.Drawing.Point(156, 69);
            this.txtCountryNationality.Name = "txtCountryNationality";
            this.txtCountryNationality.Size = new System.Drawing.Size(232, 22);
            this.txtCountryNationality.TabIndex = 4;
            this.txtCountryNationality.TextBox1Binding = "";
            // 
            // numWHoldingTerm
            // 
            this.numWHoldingTerm.BackColor = System.Drawing.Color.White;
            this.numWHoldingTerm.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "WithHoldingRate", true));
            this.numWHoldingTerm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numWHoldingTerm.Location = new System.Drawing.Point(156, 317);
            this.numWHoldingTerm.Name = "numWHoldingTerm";
            this.numWHoldingTerm.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWHoldingTerm.Size = new System.Drawing.Size(40, 23);
            this.numWHoldingTerm.TabIndex = 12;
            this.numWHoldingTerm.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // B04
            // 
            this.ClientSize = new System.Drawing.Size(832, 457);
            this.DefaultControl = "txtCode";
            this.DefaultControlForEdit = "txtTel";
            this.DefaultOrder = "ID";
            this.EnableGridJunkColor = true;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B04";
            this.Text = "B04. Supplier/Sub Con (Local)";
            this.UniqueExpress = "ID";
            this.WorkAlias = "LocalSupp";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelCode;
        private Class.txtpayterm_fty txtpayterm_ftyPaymentTerm;
        private Class.txtcurrency txtCurrency;
        private Class.txtcountry txtCountryNationality;
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
        private Win.UI.CheckBox checkJunk;
        private Win.UI.NumericBox numWHoldingTerm;
    }
}
