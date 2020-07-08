using Sci.Win;
namespace Sci.Production.Win
{
    partial class Login
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
            this.act = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.pwd = new Sci.Win.UI.TextBox();
            this.ok = new Sci.Win.UI.Button();
            this.exit = new Sci.Win.UI.Button();
            this.label3 = new Sci.Win.UI.Label();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.comboBox2 = new Sci.Win.UI.ComboBox();
            this.label4 = new Sci.Win.UI.Label();
            this.checkBoxTestEnvironment = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // act
            // 
            this.act.BackColor = System.Drawing.Color.White;
            this.act.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.act.Location = new System.Drawing.Point(127, 40);
            this.act.Name = "act";
            this.act.ShortcutsEnabled = false;
            this.act.Size = new System.Drawing.Size(131, 23);
            this.act.TabIndex = 0;
            this.act.Validating += new System.ComponentModel.CancelEventHandler(this.Act_Validated);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Account";
            this.label1.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Password";
            this.label2.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pwd
            // 
            this.pwd.BackColor = System.Drawing.Color.White;
            this.pwd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pwd.Location = new System.Drawing.Point(127, 72);
            this.pwd.Name = "pwd";
            this.pwd.PasswordChar = '*';
            this.pwd.ShortcutsEnabled = false;
            this.pwd.Size = new System.Drawing.Size(131, 23);
            this.pwd.TabIndex = 1;
            // 
            // ok
            // 
            this.ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ok.Location = new System.Drawing.Point(164, 137);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(80, 30);
            this.ok.TabIndex = 3;
            this.ok.Text = "Login";
            this.ok.UseVisualStyleBackColor = true;
            // 
            // exit
            // 
            this.exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.exit.Location = new System.Drawing.Point(250, 137);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(80, 30);
            this.exit.TabIndex = 4;
            this.exit.Text = "Exit";
            this.exit.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 23);
            this.label3.TabIndex = 92;
            this.label3.Text = "Factory";
            this.label3.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(127, 104);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(131, 24);
            this.comboBox1.TabIndex = 2;
            // 
            // comboBox2
            // 
            this.comboBox2.BackColor = System.Drawing.Color.White;
            this.comboBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.IsSupportUnselect = true;
            this.comboBox2.Location = new System.Drawing.Point(127, 9);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(131, 24);
            this.comboBox2.TabIndex = 97;
            this.comboBox2.Visible = false;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.ComboBox2_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 23);
            this.label4.TabIndex = 96;
            this.label4.Text = "DataBase";
            this.label4.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Visible = false;
            // 
            // checkBoxTestEnvironment
            // 
            this.checkBoxTestEnvironment.AutoSize = true;
            this.checkBoxTestEnvironment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxTestEnvironment.Location = new System.Drawing.Point(9, 143);
            this.checkBoxTestEnvironment.Name = "checkBoxTestEnvironment";
            this.checkBoxTestEnvironment.Size = new System.Drawing.Size(138, 21);
            this.checkBoxTestEnvironment.TabIndex = 98;
            this.checkBoxTestEnvironment.Text = "Test Environment";
            this.checkBoxTestEnvironment.UseVisualStyleBackColor = true;
            this.checkBoxTestEnvironment.Visible = false;
            this.checkBoxTestEnvironment.CheckedChanged += new System.EventHandler(this.CheckBoxTestEnvironment_CheckedChanged);
            // 
            // Login
            // 
            this.ClientSize = new System.Drawing.Size(342, 178);
            this.Controls.Add(this.checkBoxTestEnvironment);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.pwd);
            this.Controls.Add(this.act);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Login";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sci.Win.UI.TextBox act;
        private Sci.Win.UI.Label label1;
        private Sci.Win.UI.Label label2;
        private Sci.Win.UI.TextBox pwd;
        private Sci.Win.UI.Button ok;
        private Sci.Win.UI.Button exit;
        private Sci.Win.UI.Label label3;
        private Sci.Win.UI.ComboBox comboBox1;
        private Sci.Win.UI.ComboBox comboBox2;
        private Sci.Win.UI.Label label4;
        private Sci.Win.UI.CheckBox checkBoxTestEnvironment;
    }
}
