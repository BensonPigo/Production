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
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.label3 = new Sci.Win.UI.Label();
            this.exit = new Sci.Win.UI.Button();
            this.ok = new Sci.Win.UI.Button();
            this.pwd = new Sci.Win.UI.TextBox();
            this.act = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(121, 86);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(131, 24);
            this.comboBox1.TabIndex = 97;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(9, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 23);
            this.label3.TabIndex = 100;
            this.label3.Text = "Factory";
            this.label3.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // exit
            // 
            this.exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.exit.Location = new System.Drawing.Point(260, 123);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(80, 30);
            this.exit.TabIndex = 99;
            this.exit.Text = "Exit";
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // ok
            // 
            this.ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ok.Location = new System.Drawing.Point(174, 123);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(80, 30);
            this.ok.TabIndex = 98;
            this.ok.Text = "Login";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // pwd
            // 
            this.pwd.BackColor = System.Drawing.Color.White;
            this.pwd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pwd.Location = new System.Drawing.Point(121, 54);
            this.pwd.Name = "pwd";
            this.pwd.PasswordChar = '*';
            this.pwd.ShortcutsEnabled = false;
            this.pwd.Size = new System.Drawing.Size(131, 23);
            this.pwd.TabIndex = 96;
            // 
            // act
            // 
            this.act.BackColor = System.Drawing.Color.White;
            this.act.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.act.Location = new System.Drawing.Point(121, 22);
            this.act.Name = "act";
            this.act.ShortcutsEnabled = false;
            this.act.Size = new System.Drawing.Size(131, 23);
            this.act.TabIndex = 93;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(9, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 23);
            this.label2.TabIndex = 94;
            this.label2.Text = "Password";
            this.label2.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 23);
            this.label1.TabIndex = 95;
            this.label1.Text = "Account";
            this.label1.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SwitchFactory
            // 
            this.ClientSize = new System.Drawing.Size(352, 165);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.pwd);
            this.Controls.Add(this.act);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SwitchFactory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Switch Factory";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.act, 0);
            this.Controls.SetChildIndex(this.pwd, 0);
            this.Controls.SetChildIndex(this.ok, 0);
            this.Controls.SetChildIndex(this.exit, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.ComboBox comboBox1;
        private Win.UI.Label label3;
        private Win.UI.Button exit;
        private Win.UI.Button ok;
        private Win.UI.TextBox pwd;
        private Win.UI.TextBox act;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
    }
}
