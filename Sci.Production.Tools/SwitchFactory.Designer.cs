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
            this.SuspendLayout();
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(121, 86);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(131, 24);
            this.comboFactory.TabIndex = 97;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(9, 87);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(109, 23);
            this.labelFactory.TabIndex = 100;
            this.labelFactory.Text = "Factory";
            this.labelFactory.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnExit.Location = new System.Drawing.Point(260, 123);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 30);
            this.btnExit.TabIndex = 99;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnLogin.Location = new System.Drawing.Point(174, 123);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(80, 30);
            this.btnLogin.TabIndex = 98;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPassword.Location = new System.Drawing.Point(121, 54);
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
            this.txtAccount.Location = new System.Drawing.Point(121, 22);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.ShortcutsEnabled = false;
            this.txtAccount.Size = new System.Drawing.Size(131, 23);
            this.txtAccount.TabIndex = 93;
            // 
            // labelPassword
            // 
            this.labelPassword.Lines = 0;
            this.labelPassword.Location = new System.Drawing.Point(9, 54);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(109, 23);
            this.labelPassword.TabIndex = 94;
            this.labelPassword.Text = "Password";
            this.labelPassword.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAccount
            // 
            this.labelAccount.Lines = 0;
            this.labelAccount.Location = new System.Drawing.Point(9, 22);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(109, 23);
            this.labelAccount.TabIndex = 95;
            this.labelAccount.Text = "Account";
            this.labelAccount.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SwitchFactory
            // 
            this.ClientSize = new System.Drawing.Size(352, 165);
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
    }
}
