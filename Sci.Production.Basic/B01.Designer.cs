namespace Sci.Production.Basic
{
    partial class B01
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
            this.labelCode = new Sci.Win.UI.Label();
            this.labelName = new Sci.Win.UI.Label();
            this.labelAddress = new Sci.Win.UI.Label();
            this.labelTel = new Sci.Win.UI.Label();
            this.labelTINNo = new Sci.Win.UI.Label();
            this.labelManager = new Sci.Win.UI.Label();
            this.labelFtyGroup = new Sci.Win.UI.Label();
            this.labelRegionCode = new Sci.Win.UI.Label();
            this.labelForPAPACCNo = new Sci.Win.UI.Label();
            this.labelVAT = new Sci.Win.UI.Label();
            this.labelWHolding = new Sci.Win.UI.Label();
            this.labelCreditbank = new Sci.Win.UI.Label();
            this.txtCode = new Sci.Win.UI.TextBox();
            this.txtName = new Sci.Win.UI.TextBox();
            this.txtTel = new Sci.Win.UI.TextBox();
            this.editAddress = new Sci.Win.UI.EditBox();
            this.txtTINNo = new Sci.Win.UI.TextBox();
            this.txtFtyGroup = new Sci.Win.UI.TextBox();
            this.txtRegionCode = new Sci.Win.UI.TextBox();
            this.txtVAT = new Sci.Win.UI.TextBox();
            this.txtWHolding = new Sci.Win.UI.TextBox();
            this.txtCreditbank = new Sci.Win.UI.TextBox();
            this.labelKeyWord = new Sci.Win.UI.Label();
            this.txtKeyWord = new Sci.Win.UI.TextBox();
            this.btnCapacityWorkday = new Sci.Win.UI.Button();
            this.checkUseSBTS = new Sci.Win.UI.CheckBox();
            this.labelM = new Sci.Win.UI.Label();
            this.displayM = new Sci.Win.UI.DisplayBox();
            this.txtUserManager = new Sci.Production.Class.Txtuser();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
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
            this.detail.Size = new System.Drawing.Size(830, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.displayM);
            this.detailcont.Controls.Add(this.labelM);
            this.detailcont.Controls.Add(this.checkUseSBTS);
            this.detailcont.Controls.Add(this.btnCapacityWorkday);
            this.detailcont.Controls.Add(this.txtKeyWord);
            this.detailcont.Controls.Add(this.labelKeyWord);
            this.detailcont.Controls.Add(this.txtCreditbank);
            this.detailcont.Controls.Add(this.txtWHolding);
            this.detailcont.Controls.Add(this.txtVAT);
            this.detailcont.Controls.Add(this.txtRegionCode);
            this.detailcont.Controls.Add(this.txtFtyGroup);
            this.detailcont.Controls.Add(this.txtTINNo);
            this.detailcont.Controls.Add(this.editAddress);
            this.detailcont.Controls.Add(this.txtTel);
            this.detailcont.Controls.Add(this.txtName);
            this.detailcont.Controls.Add(this.txtCode);
            this.detailcont.Controls.Add(this.labelCreditbank);
            this.detailcont.Controls.Add(this.labelWHolding);
            this.detailcont.Controls.Add(this.labelVAT);
            this.detailcont.Controls.Add(this.labelForPAPACCNo);
            this.detailcont.Controls.Add(this.labelRegionCode);
            this.detailcont.Controls.Add(this.labelFtyGroup);
            this.detailcont.Controls.Add(this.labelManager);
            this.detailcont.Controls.Add(this.labelTINNo);
            this.detailcont.Controls.Add(this.labelTel);
            this.detailcont.Controls.Add(this.labelAddress);
            this.detailcont.Controls.Add(this.labelName);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Controls.Add(this.txtUserManager);
            this.detailcont.Size = new System.Drawing.Size(830, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(830, 38);
            this.detailbtm.TabIndex = 0;
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(830, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(838, 424);
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
            this.labelCode.Location = new System.Drawing.Point(35, 14);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(85, 23);
            this.labelCode.TabIndex = 0;
            this.labelCode.Text = "Code";
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(35, 41);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(85, 23);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "Name";
            // 
            // labelAddress
            // 
            this.labelAddress.Location = new System.Drawing.Point(35, 68);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(85, 23);
            this.labelAddress.TabIndex = 2;
            this.labelAddress.Text = "Address";
            // 
            // labelTel
            // 
            this.labelTel.Location = new System.Drawing.Point(35, 142);
            this.labelTel.Name = "labelTel";
            this.labelTel.Size = new System.Drawing.Size(85, 23);
            this.labelTel.TabIndex = 3;
            this.labelTel.Text = "Tel";
            // 
            // labelTINNo
            // 
            this.labelTINNo.Location = new System.Drawing.Point(35, 169);
            this.labelTINNo.Name = "labelTINNo";
            this.labelTINNo.Size = new System.Drawing.Size(85, 23);
            this.labelTINNo.TabIndex = 4;
            this.labelTINNo.Text = "TIN No.";
            // 
            // labelManager
            // 
            this.labelManager.Location = new System.Drawing.Point(35, 196);
            this.labelManager.Name = "labelManager";
            this.labelManager.Size = new System.Drawing.Size(85, 23);
            this.labelManager.TabIndex = 5;
            this.labelManager.Text = "Manager";
            // 
            // labelFtyGroup
            // 
            this.labelFtyGroup.Location = new System.Drawing.Point(35, 223);
            this.labelFtyGroup.Name = "labelFtyGroup";
            this.labelFtyGroup.Size = new System.Drawing.Size(85, 23);
            this.labelFtyGroup.TabIndex = 6;
            this.labelFtyGroup.Text = "Fty Group";
            // 
            // labelRegionCode
            // 
            this.labelRegionCode.Location = new System.Drawing.Point(35, 250);
            this.labelRegionCode.Name = "labelRegionCode";
            this.labelRegionCode.Size = new System.Drawing.Size(85, 23);
            this.labelRegionCode.TabIndex = 7;
            this.labelRegionCode.Text = "Region Code";
            // 
            // labelForPAPACCNo
            // 
            this.labelForPAPACCNo.Location = new System.Drawing.Point(35, 277);
            this.labelForPAPACCNo.Name = "labelForPAPACCNo";
            this.labelForPAPACCNo.Size = new System.Drawing.Size(115, 23);
            this.labelForPAPACCNo.TabIndex = 8;
            this.labelForPAPACCNo.Text = "For PAP ACC No.";
            // 
            // labelVAT
            // 
            this.labelVAT.Location = new System.Drawing.Point(70, 304);
            this.labelVAT.Name = "labelVAT";
            this.labelVAT.Size = new System.Drawing.Size(33, 23);
            this.labelVAT.TabIndex = 9;
            this.labelVAT.Text = "VAT";
            // 
            // labelWHolding
            // 
            this.labelWHolding.Location = new System.Drawing.Point(221, 304);
            this.labelWHolding.Name = "labelWHolding";
            this.labelWHolding.Size = new System.Drawing.Size(70, 23);
            this.labelWHolding.TabIndex = 10;
            this.labelWHolding.Text = "W/Holding";
            // 
            // labelCreditbank
            // 
            this.labelCreditbank.Location = new System.Drawing.Point(408, 304);
            this.labelCreditbank.Name = "labelCreditbank";
            this.labelCreditbank.Size = new System.Drawing.Size(75, 23);
            this.labelCreditbank.TabIndex = 11;
            this.labelCreditbank.Text = "Credit bank";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCode.Location = new System.Drawing.Point(124, 14);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(66, 23);
            this.txtCode.TabIndex = 0;
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "NameEN", true));
            this.txtName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtName.Location = new System.Drawing.Point(124, 41);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(317, 23);
            this.txtName.TabIndex = 3;
            // 
            // txtTel
            // 
            this.txtTel.BackColor = System.Drawing.Color.White;
            this.txtTel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Tel", true));
            this.txtTel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTel.Location = new System.Drawing.Point(124, 142);
            this.txtTel.Name = "txtTel";
            this.txtTel.Size = new System.Drawing.Size(200, 23);
            this.txtTel.TabIndex = 5;
            // 
            // editAddress
            // 
            this.editAddress.BackColor = System.Drawing.Color.White;
            this.editAddress.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "AddressEN", true));
            this.editAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editAddress.Location = new System.Drawing.Point(124, 68);
            this.editAddress.Multiline = true;
            this.editAddress.Name = "editAddress";
            this.editAddress.Size = new System.Drawing.Size(317, 69);
            this.editAddress.TabIndex = 4;
            // 
            // txtTINNo
            // 
            this.txtTINNo.BackColor = System.Drawing.Color.White;
            this.txtTINNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "TINNo", true));
            this.txtTINNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTINNo.Location = new System.Drawing.Point(124, 169);
            this.txtTINNo.Name = "txtTINNo";
            this.txtTINNo.Size = new System.Drawing.Size(110, 23);
            this.txtTINNo.TabIndex = 6;
            // 
            // txtFtyGroup
            // 
            this.txtFtyGroup.BackColor = System.Drawing.Color.White;
            this.txtFtyGroup.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FTYGroup", true));
            this.txtFtyGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFtyGroup.Location = new System.Drawing.Point(124, 223);
            this.txtFtyGroup.Name = "txtFtyGroup";
            this.txtFtyGroup.Size = new System.Drawing.Size(66, 23);
            this.txtFtyGroup.TabIndex = 8;
            // 
            // txtRegionCode
            // 
            this.txtRegionCode.BackColor = System.Drawing.Color.White;
            this.txtRegionCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "NegoRegion", true));
            this.txtRegionCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRegionCode.Location = new System.Drawing.Point(124, 250);
            this.txtRegionCode.Name = "txtRegionCode";
            this.txtRegionCode.Size = new System.Drawing.Size(36, 23);
            this.txtRegionCode.TabIndex = 9;
            // 
            // txtVAT
            // 
            this.txtVAT.BackColor = System.Drawing.Color.White;
            this.txtVAT.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "VATAccNo", true));
            this.txtVAT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtVAT.Location = new System.Drawing.Point(108, 304);
            this.txtVAT.Mask = "9999-9999";
            this.txtVAT.Name = "txtVAT";
            this.txtVAT.Size = new System.Drawing.Size(70, 23);
            this.txtVAT.TabIndex = 10;
            this.txtVAT.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // txtWHolding
            // 
            this.txtWHolding.BackColor = System.Drawing.Color.White;
            this.txtWHolding.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "WithholdingRateAccNo", true));
            this.txtWHolding.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWHolding.Location = new System.Drawing.Point(296, 304);
            this.txtWHolding.Mask = "9999-9999";
            this.txtWHolding.Name = "txtWHolding";
            this.txtWHolding.Size = new System.Drawing.Size(70, 23);
            this.txtWHolding.TabIndex = 11;
            this.txtWHolding.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // txtCreditbank
            // 
            this.txtCreditbank.BackColor = System.Drawing.Color.White;
            this.txtCreditbank.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CreditBankAccNo", true));
            this.txtCreditbank.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCreditbank.Location = new System.Drawing.Point(487, 304);
            this.txtCreditbank.Mask = "9999-9999";
            this.txtCreditbank.Name = "txtCreditbank";
            this.txtCreditbank.Size = new System.Drawing.Size(70, 23);
            this.txtCreditbank.TabIndex = 12;
            this.txtCreditbank.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // labelKeyWord
            // 
            this.labelKeyWord.Location = new System.Drawing.Point(227, 14);
            this.labelKeyWord.Name = "labelKeyWord";
            this.labelKeyWord.Size = new System.Drawing.Size(70, 23);
            this.labelKeyWord.TabIndex = 22;
            this.labelKeyWord.Text = "Key Word";
            // 
            // txtKeyWord
            // 
            this.txtKeyWord.BackColor = System.Drawing.Color.White;
            this.txtKeyWord.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "KeyWord", true));
            this.txtKeyWord.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtKeyWord.Location = new System.Drawing.Point(301, 14);
            this.txtKeyWord.Name = "txtKeyWord";
            this.txtKeyWord.Size = new System.Drawing.Size(36, 23);
            this.txtKeyWord.TabIndex = 1;
            // 
            // btnCapacityWorkday
            // 
            this.btnCapacityWorkday.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnCapacityWorkday.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCapacityWorkday.Location = new System.Drawing.Point(500, 14);
            this.btnCapacityWorkday.Name = "btnCapacityWorkday";
            this.btnCapacityWorkday.Size = new System.Drawing.Size(162, 30);
            this.btnCapacityWorkday.TabIndex = 14;
            this.btnCapacityWorkday.Text = "Capacity / Work day";
            this.btnCapacityWorkday.UseVisualStyleBackColor = true;
            this.btnCapacityWorkday.Click += new System.EventHandler(this.BtnCapacityWorkday_Click);
            // 
            // checkUseSBTS
            // 
            this.checkUseSBTS.AutoSize = true;
            this.checkUseSBTS.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "UseSBTS", true));
            this.checkUseSBTS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkUseSBTS.Location = new System.Drawing.Point(500, 74);
            this.checkUseSBTS.Name = "checkUseSBTS";
            this.checkUseSBTS.Size = new System.Drawing.Size(92, 21);
            this.checkUseSBTS.TabIndex = 13;
            this.checkUseSBTS.Text = "Use SBTS";
            this.checkUseSBTS.UseVisualStyleBackColor = true;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(376, 14);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(19, 23);
            this.labelM.TabIndex = 27;
            this.labelM.Text = "M";
            // 
            // displayM
            // 
            this.displayM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayM.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MDivisionID", true));
            this.displayM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayM.Location = new System.Drawing.Point(398, 14);
            this.displayM.Name = "displayM";
            this.displayM.Size = new System.Drawing.Size(41, 23);
            this.displayM.TabIndex = 2;
            // 
            // txtUserManager
            // 
            this.txtUserManager.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Manager", true));
            this.txtUserManager.DisplayBox1Binding = "";
            this.txtUserManager.Location = new System.Drawing.Point(124, 196);
            this.txtUserManager.Name = "txtUserManager";
            this.txtUserManager.Size = new System.Drawing.Size(300, 23);
            this.txtUserManager.TabIndex = 7;
            this.txtUserManager.TextBox1Binding = "";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // B01
            // 
            this.ClientSize = new System.Drawing.Size(838, 457);
            this.DefaultControl = "txtCode";
            this.DefaultControlForEdit = "txtKeyWord";
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B01";
            this.Text = "B01. Company Profile";
            this.UniqueExpress = "ID";
            this.WorkAlias = "Factory";
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

        private Win.UI.Label labelKeyWord;
        private Win.UI.TextBox txtCreditbank;
        private Win.UI.TextBox txtWHolding;
        private Win.UI.TextBox txtVAT;
        private Win.UI.TextBox txtRegionCode;
        private Win.UI.TextBox txtFtyGroup;
        private Win.UI.TextBox txtTINNo;
        private Win.UI.EditBox editAddress;
        private Win.UI.TextBox txtTel;
        private Win.UI.TextBox txtName;
        private Win.UI.TextBox txtCode;
        private Win.UI.Label labelCreditbank;
        private Win.UI.Label labelWHolding;
        private Win.UI.Label labelVAT;
        private Win.UI.Label labelForPAPACCNo;
        private Win.UI.Label labelRegionCode;
        private Win.UI.Label labelFtyGroup;
        private Win.UI.Label labelManager;
        private Win.UI.Label labelTINNo;
        private Win.UI.Label labelTel;
        private Win.UI.Label labelAddress;
        private Win.UI.Label labelName;
        private Win.UI.Label labelCode;
        private Win.UI.TextBox txtKeyWord;
        private Class.Txtuser txtUserManager;
        private Win.UI.CheckBox checkUseSBTS;
        private Win.UI.Button btnCapacityWorkday;
        private Win.UI.DisplayBox displayM;
        private Win.UI.Label labelM;
        private System.Windows.Forms.ImageList imageList1;
    }
}
