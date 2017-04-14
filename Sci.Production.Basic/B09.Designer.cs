namespace Sci.Production.Basic
{
    partial class B09
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
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.displayCode = new Sci.Win.UI.DisplayBox();
            this.displayAbbrvChinese = new Sci.Win.UI.DisplayBox();
            this.displayCompanyChinese = new Sci.Win.UI.DisplayBox();
            this.displayCompanyEnglish = new Sci.Win.UI.DisplayBox();
            this.displayAddressChinese = new Sci.Win.UI.DisplayBox();
            this.label10 = new Sci.Win.UI.Label();
            this.displayAbbrvEnglish = new Sci.Win.UI.DisplayBox();
            this.label11 = new Sci.Win.UI.Label();
            this.displayZipCode = new Sci.Win.UI.DisplayBox();
            this.label12 = new Sci.Win.UI.Label();
            this.label13 = new Sci.Win.UI.Label();
            this.displayTel = new Sci.Win.UI.DisplayBox();
            this.displayFax = new Sci.Win.UI.DisplayBox();
            this.label14 = new Sci.Win.UI.Label();
            this.label15 = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.checkThirdCountry = new Sci.Win.UI.CheckBox();
            this.editAddressEnglish = new Sci.Win.UI.EditBox();
            this.editLockDate = new Sci.Win.UI.EditBox();
            this.editDelay = new Sci.Win.UI.EditBox();
            this.dateLockDate = new Sci.Win.UI.DateBox();
            this.dateDelay = new Sci.Win.UI.DateBox();
            this.txtCountry = new Sci.Production.Class.txtcountry();
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
            this.detail.Size = new System.Drawing.Size(830, 478);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtCountry);
            this.detailcont.Controls.Add(this.dateDelay);
            this.detailcont.Controls.Add(this.dateLockDate);
            this.detailcont.Controls.Add(this.editDelay);
            this.detailcont.Controls.Add(this.editLockDate);
            this.detailcont.Controls.Add(this.editAddressEnglish);
            this.detailcont.Controls.Add(this.checkThirdCountry);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.label15);
            this.detailcont.Controls.Add(this.label14);
            this.detailcont.Controls.Add(this.displayFax);
            this.detailcont.Controls.Add(this.displayTel);
            this.detailcont.Controls.Add(this.label13);
            this.detailcont.Controls.Add(this.label12);
            this.detailcont.Controls.Add(this.displayZipCode);
            this.detailcont.Controls.Add(this.label11);
            this.detailcont.Controls.Add(this.displayAbbrvEnglish);
            this.detailcont.Controls.Add(this.label10);
            this.detailcont.Controls.Add(this.displayAddressChinese);
            this.detailcont.Controls.Add(this.displayCompanyEnglish);
            this.detailcont.Controls.Add(this.displayCompanyChinese);
            this.detailcont.Controls.Add(this.displayAbbrvChinese);
            this.detailcont.Controls.Add(this.displayCode);
            this.detailcont.Controls.Add(this.label9);
            this.detailcont.Controls.Add(this.label8);
            this.detailcont.Controls.Add(this.label7);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Size = new System.Drawing.Size(830, 440);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 440);
            this.detailbtm.Size = new System.Drawing.Size(830, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(830, 478);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(838, 507);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(27, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "Code";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(27, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "Abbrv (Chinese)";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(27, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 23);
            this.label5.TabIndex = 2;
            this.label5.Text = "Nationality";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(27, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 23);
            this.label6.TabIndex = 3;
            this.label6.Text = "Company(Chinese)";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(27, 144);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 23);
            this.label7.TabIndex = 4;
            this.label7.Text = "Company(English)";
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(27, 176);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(122, 23);
            this.label8.TabIndex = 5;
            this.label8.Text = "Address(Chinese)";
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(27, 208);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(122, 23);
            this.label9.TabIndex = 6;
            this.label9.Text = "Address(English)";
            // 
            // displayCode
            // 
            this.displayCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCode.Location = new System.Drawing.Point(153, 14);
            this.displayCode.Name = "displayCode";
            this.displayCode.Size = new System.Drawing.Size(60, 23);
            this.displayCode.TabIndex = 7;
            // 
            // displayAbbrvChinese
            // 
            this.displayAbbrvChinese.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayAbbrvChinese.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "AbbCH", true));
            this.displayAbbrvChinese.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayAbbrvChinese.Location = new System.Drawing.Point(153, 47);
            this.displayAbbrvChinese.Name = "displayAbbrvChinese";
            this.displayAbbrvChinese.Size = new System.Drawing.Size(100, 23);
            this.displayAbbrvChinese.TabIndex = 8;
            // 
            // displayCompanyChinese
            // 
            this.displayCompanyChinese.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCompanyChinese.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NameCH", true));
            this.displayCompanyChinese.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCompanyChinese.Location = new System.Drawing.Point(153, 111);
            this.displayCompanyChinese.Name = "displayCompanyChinese";
            this.displayCompanyChinese.Size = new System.Drawing.Size(369, 23);
            this.displayCompanyChinese.TabIndex = 9;
            // 
            // displayCompanyEnglish
            // 
            this.displayCompanyEnglish.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCompanyEnglish.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NameEN", true));
            this.displayCompanyEnglish.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCompanyEnglish.Location = new System.Drawing.Point(153, 144);
            this.displayCompanyEnglish.Name = "displayCompanyEnglish";
            this.displayCompanyEnglish.Size = new System.Drawing.Size(369, 23);
            this.displayCompanyEnglish.TabIndex = 10;
            // 
            // displayAddressChinese
            // 
            this.displayAddressChinese.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayAddressChinese.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "AddressCH", true));
            this.displayAddressChinese.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayAddressChinese.Location = new System.Drawing.Point(153, 176);
            this.displayAddressChinese.Name = "displayAddressChinese";
            this.displayAddressChinese.Size = new System.Drawing.Size(369, 23);
            this.displayAddressChinese.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(382, 47);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 23);
            this.label10.TabIndex = 14;
            this.label10.Text = "(English)";
            // 
            // displayAbbrvEnglish
            // 
            this.displayAbbrvEnglish.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayAbbrvEnglish.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "AbbEN", true));
            this.displayAbbrvEnglish.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayAbbrvEnglish.Location = new System.Drawing.Point(449, 47);
            this.displayAbbrvEnglish.Name = "displayAbbrvEnglish";
            this.displayAbbrvEnglish.Size = new System.Drawing.Size(100, 23);
            this.displayAbbrvEnglish.TabIndex = 15;
            // 
            // label11
            // 
            this.label11.Lines = 0;
            this.label11.Location = new System.Drawing.Point(382, 79);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 23);
            this.label11.TabIndex = 16;
            this.label11.Text = "Zip Code";
            // 
            // displayZipCode
            // 
            this.displayZipCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayZipCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ZipCode", true));
            this.displayZipCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayZipCode.Location = new System.Drawing.Point(449, 79);
            this.displayZipCode.Name = "displayZipCode";
            this.displayZipCode.Size = new System.Drawing.Size(73, 23);
            this.displayZipCode.TabIndex = 17;
            // 
            // label12
            // 
            this.label12.Lines = 0;
            this.label12.Location = new System.Drawing.Point(27, 271);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(122, 23);
            this.label12.TabIndex = 18;
            this.label12.Text = "Tel";
            // 
            // label13
            // 
            this.label13.Lines = 0;
            this.label13.Location = new System.Drawing.Point(27, 304);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(122, 23);
            this.label13.TabIndex = 19;
            this.label13.Text = "Fax";
            // 
            // displayTel
            // 
            this.displayTel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTel.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Tel", true));
            this.displayTel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTel.Location = new System.Drawing.Point(153, 271);
            this.displayTel.Name = "displayTel";
            this.displayTel.Size = new System.Drawing.Size(250, 23);
            this.displayTel.TabIndex = 20;
            // 
            // displayFax
            // 
            this.displayFax.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFax.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Fax", true));
            this.displayFax.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFax.Location = new System.Drawing.Point(153, 304);
            this.displayFax.Name = "displayFax";
            this.displayFax.Size = new System.Drawing.Size(250, 23);
            this.displayFax.TabIndex = 21;
            // 
            // label14
            // 
            this.label14.Lines = 0;
            this.label14.Location = new System.Drawing.Point(27, 336);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(122, 23);
            this.label14.TabIndex = 22;
            this.label14.Text = "Lock Date";
            // 
            // label15
            // 
            this.label15.Lines = 0;
            this.label15.Location = new System.Drawing.Point(404, 336);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(64, 23);
            this.label15.TabIndex = 25;
            this.label15.Text = "Delay";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(609, 14);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 28;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // checkThirdCountry
            // 
            this.checkThirdCountry.AutoSize = true;
            this.checkThirdCountry.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ThirdCountry", true));
            this.checkThirdCountry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkThirdCountry.Location = new System.Drawing.Point(609, 47);
            this.checkThirdCountry.Name = "checkThirdCountry";
            this.checkThirdCountry.Size = new System.Drawing.Size(113, 21);
            this.checkThirdCountry.TabIndex = 29;
            this.checkThirdCountry.Text = "Third Country";
            this.checkThirdCountry.UseVisualStyleBackColor = true;
            // 
            // editAddressEnglish
            // 
            this.editAddressEnglish.BackColor = System.Drawing.Color.White;
            this.editAddressEnglish.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "AddressEN", true));
            this.editAddressEnglish.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editAddressEnglish.Location = new System.Drawing.Point(153, 208);
            this.editAddressEnglish.Multiline = true;
            this.editAddressEnglish.Name = "editAddressEnglish";
            this.editAddressEnglish.Size = new System.Drawing.Size(369, 52);
            this.editAddressEnglish.TabIndex = 30;
            // 
            // editLockDate
            // 
            this.editLockDate.BackColor = System.Drawing.Color.White;
            this.editLockDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "LockMemo", true));
            this.editLockDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editLockDate.Location = new System.Drawing.Point(153, 366);
            this.editLockDate.Multiline = true;
            this.editLockDate.Name = "editLockDate";
            this.editLockDate.Size = new System.Drawing.Size(221, 68);
            this.editLockDate.TabIndex = 31;
            // 
            // editDelay
            // 
            this.editDelay.BackColor = System.Drawing.Color.White;
            this.editDelay.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DelayMemo", true));
            this.editDelay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDelay.Location = new System.Drawing.Point(471, 366);
            this.editDelay.Multiline = true;
            this.editDelay.Name = "editDelay";
            this.editDelay.Size = new System.Drawing.Size(221, 68);
            this.editDelay.TabIndex = 32;
            // 
            // dateLockDate
            // 
            this.dateLockDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "LockDate", true));
            this.dateLockDate.Location = new System.Drawing.Point(153, 336);
            this.dateLockDate.Name = "dateLockDate";
            this.dateLockDate.Size = new System.Drawing.Size(130, 23);
            this.dateLockDate.TabIndex = 33;
            // 
            // dateDelay
            // 
            this.dateDelay.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Delay", true));
            this.dateDelay.Location = new System.Drawing.Point(471, 336);
            this.dateDelay.Name = "dateDelay";
            this.dateDelay.Size = new System.Drawing.Size(130, 23);
            this.dateDelay.TabIndex = 34;
            // 
            // txtCountry
            // 
            this.txtCountry.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "CountryID", true));
            this.txtCountry.DisplayBox1Binding = "";
            this.txtCountry.Location = new System.Drawing.Point(153, 80);
            this.txtCountry.Name = "txtCountry";
            this.txtCountry.Size = new System.Drawing.Size(221, 22);
            this.txtCountry.TabIndex = 35;
            this.txtCountry.TextBox1Binding = "";
            // 
            // B09
            // 
            this.ClientSize = new System.Drawing.Size(838, 540);
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B09";
            this.Text = "B09. Supplier (Taiwan)";
            this.UniqueExpress = "ID";
            this.WorkAlias = "Supp";
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

        private Win.UI.CheckBox checkThirdCountry;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label label15;
        private Win.UI.Label label14;
        private Win.UI.DisplayBox displayFax;
        private Win.UI.DisplayBox displayTel;
        private Win.UI.Label label13;
        private Win.UI.Label label12;
        private Win.UI.DisplayBox displayZipCode;
        private Win.UI.Label label11;
        private Win.UI.DisplayBox displayAbbrvEnglish;
        private Win.UI.Label label10;
        private Win.UI.DisplayBox displayAddressChinese;
        private Win.UI.DisplayBox displayCompanyEnglish;
        private Win.UI.DisplayBox displayCompanyChinese;
        private Win.UI.DisplayBox displayAbbrvChinese;
        private Win.UI.DisplayBox displayCode;
        private Win.UI.Label label9;
        private Win.UI.Label label8;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.EditBox editDelay;
        private Win.UI.EditBox editLockDate;
        private Win.UI.EditBox editAddressEnglish;
        private Win.UI.DateBox dateDelay;
        private Win.UI.DateBox dateLockDate;
        private Class.txtcountry txtCountry;
    }
}
