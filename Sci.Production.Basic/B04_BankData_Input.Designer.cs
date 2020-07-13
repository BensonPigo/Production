namespace Sci.Production.Basic
{
    partial class B04_BankData_Input
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
            this.labelAccountNo = new Sci.Win.UI.Label();
            this.labelSWIFTCode = new Sci.Win.UI.Label();
            this.labelAccountName = new Sci.Win.UI.Label();
            this.labelBankName = new Sci.Win.UI.Label();
            this.labelCountry = new Sci.Win.UI.Label();
            this.labelCity = new Sci.Win.UI.Label();
            this.labelIntermediaryBank = new Sci.Win.UI.Label();
            this.labelIntermediaryBankSWIFTCode = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.labelCreateby = new Sci.Win.UI.Label();
            this.labelEditby = new Sci.Win.UI.Label();
            this.txtAccountNo = new Sci.Win.UI.TextBox();
            this.checkDefault = new Sci.Win.UI.CheckBox();
            this.txtSWIFTCode = new Sci.Win.UI.TextBox();
            this.txtAccountName = new Sci.Win.UI.TextBox();
            this.txtBankName = new Sci.Win.UI.TextBox();
            this.txtCity = new Sci.Win.UI.TextBox();
            this.txtIntermediaryBank = new Sci.Win.UI.TextBox();
            this.txtIntermediaryBankSWIFTCode = new Sci.Win.UI.TextBox();
            this.displayCreateBy = new Sci.Win.UI.DisplayBox();
            this.displayEditBy = new Sci.Win.UI.DisplayBox();
            this.txtCountry = new Sci.Production.Class.Txtcountry();
            this.editRemark = new Sci.Win.UI.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 354);
            // 
            // undo
            // 
            this.undo.TabIndex = 3;
            // 
            // save
            // 
            this.save.TabIndex = 2;
            // 
            // left
            // 
            this.left.TabIndex = 0;
            // 
            // right
            // 
            this.right.TabIndex = 1;
            // 
            // labelAccountNo
            // 
            this.labelAccountNo.Location = new System.Drawing.Point(13, 13);
            this.labelAccountNo.Name = "labelAccountNo";
            this.labelAccountNo.Size = new System.Drawing.Size(120, 23);
            this.labelAccountNo.TabIndex = 95;
            this.labelAccountNo.Text = "Account No.";
            // 
            // labelSWIFTCode
            // 
            this.labelSWIFTCode.Location = new System.Drawing.Point(13, 40);
            this.labelSWIFTCode.Name = "labelSWIFTCode";
            this.labelSWIFTCode.Size = new System.Drawing.Size(120, 23);
            this.labelSWIFTCode.TabIndex = 96;
            this.labelSWIFTCode.Text = "SWIFT Code";
            // 
            // labelAccountName
            // 
            this.labelAccountName.Location = new System.Drawing.Point(13, 67);
            this.labelAccountName.Name = "labelAccountName";
            this.labelAccountName.Size = new System.Drawing.Size(120, 23);
            this.labelAccountName.TabIndex = 97;
            this.labelAccountName.Text = "Account Name";
            // 
            // labelBankName
            // 
            this.labelBankName.Location = new System.Drawing.Point(13, 94);
            this.labelBankName.Name = "labelBankName";
            this.labelBankName.Size = new System.Drawing.Size(120, 23);
            this.labelBankName.TabIndex = 98;
            this.labelBankName.Text = "Bank Name";
            // 
            // labelCountry
            // 
            this.labelCountry.Location = new System.Drawing.Point(13, 121);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(120, 23);
            this.labelCountry.TabIndex = 99;
            this.labelCountry.Text = "Country";
            // 
            // labelCity
            // 
            this.labelCity.Location = new System.Drawing.Point(13, 148);
            this.labelCity.Name = "labelCity";
            this.labelCity.Size = new System.Drawing.Size(120, 23);
            this.labelCity.TabIndex = 100;
            this.labelCity.Text = "City";
            // 
            // labelIntermediaryBank
            // 
            this.labelIntermediaryBank.Location = new System.Drawing.Point(13, 175);
            this.labelIntermediaryBank.Name = "labelIntermediaryBank";
            this.labelIntermediaryBank.Size = new System.Drawing.Size(120, 23);
            this.labelIntermediaryBank.TabIndex = 101;
            this.labelIntermediaryBank.Text = "Intermediary Bank";
            // 
            // labelIntermediaryBankSWIFTCode
            // 
            this.labelIntermediaryBankSWIFTCode.Location = new System.Drawing.Point(13, 202);
            this.labelIntermediaryBankSWIFTCode.Name = "labelIntermediaryBankSWIFTCode";
            this.labelIntermediaryBankSWIFTCode.Size = new System.Drawing.Size(200, 23);
            this.labelIntermediaryBankSWIFTCode.TabIndex = 102;
            this.labelIntermediaryBankSWIFTCode.Text = "Intermediary Bank-SWIFT Code";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(13, 229);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(120, 23);
            this.labelRemark.TabIndex = 103;
            this.labelRemark.Text = "Remark";
            // 
            // labelCreateby
            // 
            this.labelCreateby.Location = new System.Drawing.Point(13, 293);
            this.labelCreateby.Name = "labelCreateby";
            this.labelCreateby.Size = new System.Drawing.Size(120, 23);
            this.labelCreateby.TabIndex = 104;
            this.labelCreateby.Text = "Create by";
            // 
            // labelEditby
            // 
            this.labelEditby.Location = new System.Drawing.Point(13, 320);
            this.labelEditby.Name = "labelEditby";
            this.labelEditby.Size = new System.Drawing.Size(120, 23);
            this.labelEditby.TabIndex = 105;
            this.labelEditby.Text = "Edit by";
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.BackColor = System.Drawing.Color.White;
            this.txtAccountNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "AccountNo", true));
            this.txtAccountNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtAccountNo.Location = new System.Drawing.Point(136, 13);
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.Size = new System.Drawing.Size(198, 23);
            this.txtAccountNo.TabIndex = 0;
            // 
            // checkDefault
            // 
            this.checkDefault.AutoSize = true;
            this.checkDefault.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsDefault", true));
            this.checkDefault.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkDefault.Location = new System.Drawing.Point(377, 12);
            this.checkDefault.Name = "checkDefault";
            this.checkDefault.Size = new System.Drawing.Size(72, 21);
            this.checkDefault.TabIndex = 9;
            this.checkDefault.Text = "Default";
            this.checkDefault.UseVisualStyleBackColor = true;
            // 
            // txtSWIFTCode
            // 
            this.txtSWIFTCode.BackColor = System.Drawing.Color.White;
            this.txtSWIFTCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SWIFTCode", true));
            this.txtSWIFTCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSWIFTCode.Location = new System.Drawing.Point(136, 40);
            this.txtSWIFTCode.Name = "txtSWIFTCode";
            this.txtSWIFTCode.Size = new System.Drawing.Size(85, 23);
            this.txtSWIFTCode.TabIndex = 1;
            // 
            // txtAccountName
            // 
            this.txtAccountName.BackColor = System.Drawing.Color.White;
            this.txtAccountName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtAccountName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "AccountName", true));
            this.txtAccountName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtAccountName.Location = new System.Drawing.Point(136, 67);
            this.txtAccountName.Name = "txtAccountName";
            this.txtAccountName.Size = new System.Drawing.Size(378, 23);
            this.txtAccountName.TabIndex = 2;
            // 
            // txtBankName
            // 
            this.txtBankName.BackColor = System.Drawing.Color.White;
            this.txtBankName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtBankName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BankName", true));
            this.txtBankName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBankName.Location = new System.Drawing.Point(136, 94);
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.Size = new System.Drawing.Size(438, 23);
            this.txtBankName.TabIndex = 3;
            // 
            // txtCity
            // 
            this.txtCity.BackColor = System.Drawing.Color.White;
            this.txtCity.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "City", true));
            this.txtCity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCity.Location = new System.Drawing.Point(136, 148);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(140, 23);
            this.txtCity.TabIndex = 5;
            // 
            // txtIntermediaryBank
            // 
            this.txtIntermediaryBank.BackColor = System.Drawing.Color.White;
            this.txtIntermediaryBank.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MidBankName", true));
            this.txtIntermediaryBank.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtIntermediaryBank.Location = new System.Drawing.Point(136, 175);
            this.txtIntermediaryBank.Name = "txtIntermediaryBank";
            this.txtIntermediaryBank.Size = new System.Drawing.Size(438, 23);
            this.txtIntermediaryBank.TabIndex = 6;
            // 
            // txtIntermediaryBankSWIFTCode
            // 
            this.txtIntermediaryBankSWIFTCode.BackColor = System.Drawing.Color.White;
            this.txtIntermediaryBankSWIFTCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MidSWIFTCode", true));
            this.txtIntermediaryBankSWIFTCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtIntermediaryBankSWIFTCode.Location = new System.Drawing.Point(217, 202);
            this.txtIntermediaryBankSWIFTCode.Name = "txtIntermediaryBankSWIFTCode";
            this.txtIntermediaryBankSWIFTCode.Size = new System.Drawing.Size(84, 23);
            this.txtIntermediaryBankSWIFTCode.TabIndex = 7;
            // 
            // displayCreateBy
            // 
            this.displayCreateBy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCreateBy.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CreateBy", true));
            this.displayCreateBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCreateBy.Location = new System.Drawing.Point(136, 293);
            this.displayCreateBy.Name = "displayCreateBy";
            this.displayCreateBy.Size = new System.Drawing.Size(350, 23);
            this.displayCreateBy.TabIndex = 10;
            // 
            // displayEditBy
            // 
            this.displayEditBy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayEditBy.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "EditBy", true));
            this.displayEditBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayEditBy.Location = new System.Drawing.Point(136, 320);
            this.displayEditBy.Name = "displayEditBy";
            this.displayEditBy.Size = new System.Drawing.Size(350, 23);
            this.displayEditBy.TabIndex = 11;
            // 
            // txtCountry
            // 
            this.txtCountry.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "CountryID", true));
            this.txtCountry.DisplayBox1Binding = "";
            this.txtCountry.Location = new System.Drawing.Point(136, 121);
            this.txtCountry.Name = "txtCountry";
            this.txtCountry.Size = new System.Drawing.Size(232, 22);
            this.txtCountry.TabIndex = 4;
            this.txtCountry.TextBox1Binding = "";
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(137, 229);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(435, 60);
            this.editRemark.TabIndex = 8;
            // 
            // B04_BankData_Input
            // 
            this.ClientSize = new System.Drawing.Size(584, 394);
            this.Controls.Add(this.editRemark);
            this.Controls.Add(this.txtCountry);
            this.Controls.Add(this.displayEditBy);
            this.Controls.Add(this.displayCreateBy);
            this.Controls.Add(this.txtIntermediaryBankSWIFTCode);
            this.Controls.Add(this.txtIntermediaryBank);
            this.Controls.Add(this.txtCity);
            this.Controls.Add(this.txtBankName);
            this.Controls.Add(this.txtAccountName);
            this.Controls.Add(this.txtSWIFTCode);
            this.Controls.Add(this.checkDefault);
            this.Controls.Add(this.txtAccountNo);
            this.Controls.Add(this.labelEditby);
            this.Controls.Add(this.labelCreateby);
            this.Controls.Add(this.labelRemark);
            this.Controls.Add(this.labelIntermediaryBankSWIFTCode);
            this.Controls.Add(this.labelIntermediaryBank);
            this.Controls.Add(this.labelCity);
            this.Controls.Add(this.labelCountry);
            this.Controls.Add(this.labelBankName);
            this.Controls.Add(this.labelAccountName);
            this.Controls.Add(this.labelSWIFTCode);
            this.Controls.Add(this.labelAccountNo);
            this.EditMode = true;
            this.Name = "B04_BankData_Input";
            this.OnLineHelpID = "Sci.Win.Subs.Input6A";
            this.Text = "Data maintain";
            this.WorkAlias = "LocalSupp_Bank_Detail";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.labelAccountNo, 0);
            this.Controls.SetChildIndex(this.labelSWIFTCode, 0);
            this.Controls.SetChildIndex(this.labelAccountName, 0);
            this.Controls.SetChildIndex(this.labelBankName, 0);
            this.Controls.SetChildIndex(this.labelCountry, 0);
            this.Controls.SetChildIndex(this.labelCity, 0);
            this.Controls.SetChildIndex(this.labelIntermediaryBank, 0);
            this.Controls.SetChildIndex(this.labelIntermediaryBankSWIFTCode, 0);
            this.Controls.SetChildIndex(this.labelRemark, 0);
            this.Controls.SetChildIndex(this.labelCreateby, 0);
            this.Controls.SetChildIndex(this.labelEditby, 0);
            this.Controls.SetChildIndex(this.txtAccountNo, 0);
            this.Controls.SetChildIndex(this.checkDefault, 0);
            this.Controls.SetChildIndex(this.txtSWIFTCode, 0);
            this.Controls.SetChildIndex(this.txtAccountName, 0);
            this.Controls.SetChildIndex(this.txtBankName, 0);
            this.Controls.SetChildIndex(this.txtCity, 0);
            this.Controls.SetChildIndex(this.txtIntermediaryBank, 0);
            this.Controls.SetChildIndex(this.txtIntermediaryBankSWIFTCode, 0);
            this.Controls.SetChildIndex(this.displayCreateBy, 0);
            this.Controls.SetChildIndex(this.displayEditBy, 0);
            this.Controls.SetChildIndex(this.txtCountry, 0);
            this.Controls.SetChildIndex(this.editRemark, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelAccountNo;
        private Win.UI.Label labelSWIFTCode;
        private Win.UI.Label labelAccountName;
        private Win.UI.Label labelBankName;
        private Win.UI.Label labelCountry;
        private Win.UI.Label labelCity;
        private Win.UI.Label labelIntermediaryBank;
        private Win.UI.Label labelIntermediaryBankSWIFTCode;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelCreateby;
        private Win.UI.Label labelEditby;
        private Win.UI.TextBox txtAccountNo;
        private Win.UI.CheckBox checkDefault;
        private Win.UI.TextBox txtSWIFTCode;
        private Win.UI.TextBox txtAccountName;
        private Win.UI.TextBox txtBankName;
        private Win.UI.TextBox txtCity;
        private Win.UI.TextBox txtIntermediaryBank;
        private Win.UI.TextBox txtIntermediaryBankSWIFTCode;
        private Win.UI.DisplayBox displayCreateBy;
        private Win.UI.DisplayBox displayEditBy;
        private Class.Txtcountry txtCountry;
        private Win.UI.EditBox editRemark;
    }
}
