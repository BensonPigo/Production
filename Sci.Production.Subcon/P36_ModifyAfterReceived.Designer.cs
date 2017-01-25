namespace Sci.Production.Subcon
{
    partial class P36_ModifyAfterReceived
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
            this.button1 = new Sci.Win.UI.Button();
            this.button2 = new Sci.Win.UI.Button();
            this.label1 = new Sci.Win.UI.Label();
            this.editBox1 = new Sci.Win.UI.EditBox();
            this.mtbs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label16 = new Sci.Win.UI.Label();
            this.txtAccountNo1 = new Sci.Production.Class.txtAccountNo();
            this.txtuser4 = new Sci.Production.Class.txtuser();
            this.label14 = new Sci.Win.UI.Label();
            this.dateBox1 = new Sci.Win.UI.DateBox();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button1.Location = new System.Drawing.Point(472, 302);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 5;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button2.Location = new System.Drawing.Point(386, 302);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 30);
            this.button2.TabIndex = 4;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Description";
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.White;
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "description", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox1.Location = new System.Drawing.Point(87, 9);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(459, 155);
            this.editBox1.TabIndex = 0;
            // 
            // label16
            // 
            this.label16.Lines = 0;
            this.label16.Location = new System.Drawing.Point(9, 242);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(75, 23);
            this.label16.TabIndex = 90;
            this.label16.Text = "Account No";
            // 
            // txtAccountNo1
            // 
            this.txtAccountNo1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "AccountID", true));
            this.txtAccountNo1.DisplayBox1Binding = "";
            this.txtAccountNo1.Location = new System.Drawing.Point(87, 242);
            this.txtAccountNo1.Name = "txtAccountNo1";
            this.txtAccountNo1.Size = new System.Drawing.Size(300, 23);
            this.txtAccountNo1.TabIndex = 3;
            this.txtAccountNo1.TextBox1Binding = "";
            // 
            // txtuser4
            // 
            this.txtuser4.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "receiveName", true));
            this.txtuser4.DisplayBox1Binding = "";
            this.txtuser4.Location = new System.Drawing.Point(87, 178);
            this.txtuser4.Name = "txtuser4";
            this.txtuser4.Size = new System.Drawing.Size(300, 23);
            this.txtuser4.TabIndex = 1;
            this.txtuser4.TextBox1Binding = "";
            // 
            // label14
            // 
            this.label14.Lines = 0;
            this.label14.Location = new System.Drawing.Point(9, 178);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 23);
            this.label14.TabIndex = 91;
            this.label14.Text = "Received";
            // 
            // dateBox1
            // 
            this.dateBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "receiveDate", true));
            this.dateBox1.Location = new System.Drawing.Point(87, 207);
            this.dateBox1.Name = "dateBox1";
            this.dateBox1.Size = new System.Drawing.Size(130, 23);
            this.dateBox1.TabIndex = 2;
            // 
            // P36_ModifyAfterReceived
            // 
            this.ClientSize = new System.Drawing.Size(565, 344);
            this.Controls.Add(this.dateBox1);
            this.Controls.Add(this.txtuser4);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtAccountNo1);
            this.Controls.Add(this.editBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.DefaultControl = "editBox1";
            this.Name = "P36_ModifyAfterReceived";
            this.Text = "P36. Modify After Received";
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button button1;
        private Win.UI.Button button2;
        private Win.UI.Label label1;
        private Win.UI.EditBox editBox1;
        private Win.UI.Label label16;
        private Class.txtAccountNo txtAccountNo1;
        private Class.txtuser txtuser4;
        private Win.UI.Label label14;
        private Win.UI.DateBox dateBox1;
        private Win.UI.ListControlBindingSource mtbs;
    }
}
