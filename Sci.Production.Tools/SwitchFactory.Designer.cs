namespace Sci.Production.Tools
{
    partial class SwitchFactory
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
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.btnExit = new Sci.Win.UI.Button();
            this.btnLogin = new Sci.Win.UI.Button();
            this.txtPassword = new Sci.Win.UI.TextBox();
            this.txtAccount = new Sci.Win.UI.TextBox();
            this.labelPassword = new Sci.Win.UI.Label();
            this.labelAccount = new Sci.Win.UI.Label();
            this.comboBox2 = new Sci.Win.UI.ComboBox();
            this.label4 = new Sci.Win.UI.Label();
            this.checkBoxTestEnvironment = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(121, 103);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(131, 24);
            this.comboFactory.TabIndex = 97;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(9, 104);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(109, 23);
            this.labelFactory.TabIndex = 100;
            this.labelFactory.Text = "Factory";
            this.labelFactory.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnExit.Location = new System.Drawing.Point(241, 136);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 30);
            this.btnExit.TabIndex = 99;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnLogin.Location = new System.Drawing.Point(155, 136);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(80, 30);
            this.btnLogin.TabIndex = 98;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPassword.Location = new System.Drawing.Point(121, 71);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.ShortcutsEnabled = false;
            this.txtPassword.Size = new System.Drawing.Size(131, 23);
            this.txtPassword.TabIndex = 96;
            // 
            // txtAccount
            // 
            this.txtAccount.BackColor = System.Drawing.Color.White;
            this.txtAccount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtAccount.Location = new System.Drawing.Point(121, 39);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.ShortcutsEnabled = false;
            this.txtAccount.Size = new System.Drawing.Size(131, 23);
            this.txtAccount.TabIndex = 93;
            // 
            // labelPassword
            // 
            this.labelPassword.Location = new System.Drawing.Point(9, 71);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(109, 23);
            this.labelPassword.TabIndex = 94;
            this.labelPassword.Text = "Password";
            this.labelPassword.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAccount
            // 
            this.labelAccount.Location = new System.Drawing.Point(9, 39);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(109, 23);
            this.labelAccount.TabIndex = 95;
            this.labelAccount.Text = "Account";
            this.labelAccount.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox2
            // 
            this.comboBox2.BackColor = System.Drawing.Color.White;
            this.comboBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.IsSupportUnselect = true;
            this.comboBox2.Location = new System.Drawing.Point(121, 9);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(131, 24);
            this.comboBox2.TabIndex = 104;
            this.comboBox2.Visible = false;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.ComboBox2_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 23);
            this.label4.TabIndex = 103;
            this.label4.Text = "DataBase";
            this.label4.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Visible = false;
            // 
            // checkBoxTestEnvironment
            // 
            this.checkBoxTestEnvironment.AutoSize = true;
            this.checkBoxTestEnvironment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxTestEnvironment.Location = new System.Drawing.Point(9, 142);
            this.checkBoxTestEnvironment.Name = "checkBoxTestEnvironment";
            this.checkBoxTestEnvironment.Size = new System.Drawing.Size(138, 21);
            this.checkBoxTestEnvironment.TabIndex = 105;
            this.checkBoxTestEnvironment.Text = "Test Environment";
            this.checkBoxTestEnvironment.UseVisualStyleBackColor = true;
            this.checkBoxTestEnvironment.Visible = false;
            this.checkBoxTestEnvironment.CheckedChanged += new System.EventHandler(this.CheckBoxTestEnvironment_CheckedChanged);
            // 
            // SwitchFactory
            // 
            this.ClientSize = new System.Drawing.Size(335, 174);
            this.Controls.Add(this.checkBoxTestEnvironment);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtAccount);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelAccount);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SwitchFactory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Switch Factory";
            this.Controls.SetChildIndex(this.labelAccount, 0);
            this.Controls.SetChildIndex(this.labelPassword, 0);
            this.Controls.SetChildIndex(this.txtAccount, 0);
            this.Controls.SetChildIndex(this.txtPassword, 0);
            this.Controls.SetChildIndex(this.btnLogin, 0);
            this.Controls.SetChildIndex(this.btnExit, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.comboBox2, 0);
            this.Controls.SetChildIndex(this.checkBoxTestEnvironment, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.ComboBox comboFactory;
        private Win.UI.Label labelFactory;
        private Win.UI.Button btnExit;
        private Win.UI.Button btnLogin;
        private Win.UI.TextBox txtPassword;
        private Win.UI.TextBox txtAccount;
        private Win.UI.Label labelPassword;
        private Win.UI.Label labelAccount;
        private Win.UI.ComboBox comboBox2;
        private Win.UI.Label label4;
        private Win.UI.CheckBox checkBoxTestEnvironment;
    }
}
