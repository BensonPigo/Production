using Sci.Win;
namespace Sci.Production.Win
{
    partial class OTPVerification
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
            this.labelAccount = new Sci.Win.UI.Label();
            this.displayBoxAccount = new Sci.Win.UI.DisplayBox();
            this.txtOTP = new Sci.Win.UI.TextBox();
            this.btnLogin = new Sci.Win.UI.Button();
            this.btnSendOtp = new Sci.Win.UI.Button();
            this.labelOTP = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // labelAccount
            // 
            this.labelAccount.Location = new System.Drawing.Point(9, 9);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(131, 23);
            this.labelAccount.TabIndex = 2;
            this.labelAccount.Text = "Account";
            // 
            // displayBoxAccount
            // 
            this.displayBoxAccount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxAccount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxAccount.Location = new System.Drawing.Point(143, 9);
            this.displayBoxAccount.Name = "displayBoxAccount";
            this.displayBoxAccount.Size = new System.Drawing.Size(287, 23);
            this.displayBoxAccount.TabIndex = 56;
            // 
            // txtOTP
            // 
            this.txtOTP.BackColor = System.Drawing.Color.White;
            this.txtOTP.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtOTP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtOTP.Location = new System.Drawing.Point(143, 38);
            this.txtOTP.Mask = "000000";
            this.txtOTP.MaxLength = 6;
            this.txtOTP.Name = "txtOTP";
            this.txtOTP.Size = new System.Drawing.Size(287, 23);
            this.txtOTP.TabIndex = 58;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(350, 67);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(80, 30);
            this.btnLogin.TabIndex = 59;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // btnSendOtp
            // 
            this.btnSendOtp.Enabled = false;
            this.btnSendOtp.Location = new System.Drawing.Point(436, 67);
            this.btnSendOtp.Name = "btnSendOtp";
            this.btnSendOtp.Size = new System.Drawing.Size(109, 30);
            this.btnSendOtp.TabIndex = 60;
            this.btnSendOtp.Text = "Send Again";
            this.btnSendOtp.UseVisualStyleBackColor = true;
            this.btnSendOtp.Click += new System.EventHandler(this.BtnSendOtp_Click);
            // 
            // labelOTP
            // 
            this.labelOTP.Location = new System.Drawing.Point(9, 38);
            this.labelOTP.Name = "labelOTP";
            this.labelOTP.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelOTP.RectStyle.BorderWidth = 1F;
            this.labelOTP.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelOTP.RectStyle.ExtBorderWidth = 1F;
            this.labelOTP.Size = new System.Drawing.Size(131, 23);
            this.labelOTP.TabIndex = 126;
            this.labelOTP.Text = "OTP";
            this.labelOTP.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelOTP.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // OTPVerification
            // 
            this.ClientSize = new System.Drawing.Size(557, 110);
            this.Controls.Add(this.labelOTP);
            this.Controls.Add(this.btnSendOtp);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtOTP);
            this.Controls.Add(this.displayBoxAccount);
            this.Controls.Add(this.labelAccount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OTPVerification";
            this.OnLineHelpID = "Sci.Win.Tools.Base";
            this.Text = "OTP Verification";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sci.Win.UI.Label labelAccount;
        private Sci.Win.UI.DisplayBox displayBoxAccount;
        private Sci.Win.UI.TextBox txtOTP;
        private Sci.Win.UI.Button btnLogin;
        private Sci.Win.UI.Button btnSendOtp;
        private Sci.Win.UI.Label labelOTP;
    }
}
