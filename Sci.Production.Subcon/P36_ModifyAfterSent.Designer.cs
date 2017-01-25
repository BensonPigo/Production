namespace Sci.Production.Subcon
{
    partial class P36_ModifyAfterSent
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
            this.numExchange = new Sci.Win.UI.NumericBox();
            this.label12 = new Sci.Win.UI.Label();
            this.numericBox4 = new Sci.Win.UI.NumericBox();
            this.numericBox2 = new Sci.Win.UI.NumericBox();
            this.label10 = new Sci.Win.UI.Label();
            this.numericBox1 = new Sci.Win.UI.NumericBox();
            this.label9 = new Sci.Win.UI.Label();
            this.label16 = new Sci.Win.UI.Label();
            this.txtAccountNo1 = new Sci.Production.Class.txtAccountNo();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button1.Location = new System.Drawing.Point(472, 302);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 7;
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
            this.button2.TabIndex = 6;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(16, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Description";
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.White;
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "description", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox1.Location = new System.Drawing.Point(94, 90);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(459, 155);
            this.editBox1.TabIndex = 4;
            // 
            // numExchange
            // 
            this.numExchange.BackColor = System.Drawing.Color.White;
            this.numExchange.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "exchange", true));
            this.numExchange.DecimalPlaces = 3;
            this.numExchange.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numExchange.Location = new System.Drawing.Point(94, 54);
            this.numExchange.MaxBytes = 8;
            this.numExchange.Name = "numExchange";
            this.numExchange.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numExchange.Size = new System.Drawing.Size(100, 23);
            this.numExchange.TabIndex = 3;
            this.numExchange.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.Lines = 0;
            this.label12.Location = new System.Drawing.Point(16, 54);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(75, 23);
            this.label12.TabIndex = 87;
            this.label12.Text = "Exchange";
            // 
            // numericBox4
            // 
            this.numericBox4.BackColor = System.Drawing.Color.White;
            this.numericBox4.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "taxrate", true));
            this.numericBox4.DecimalPlaces = 2;
            this.numericBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox4.Location = new System.Drawing.Point(462, 19);
            this.numericBox4.MaxBytes = 5;
            this.numericBox4.Name = "numericBox4";
            this.numericBox4.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox4.Size = new System.Drawing.Size(60, 23);
            this.numericBox4.TabIndex = 2;
            this.numericBox4.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numericBox2
            // 
            this.numericBox2.BackColor = System.Drawing.Color.White;
            this.numericBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "tax", true));
            this.numericBox2.DecimalPlaces = 2;
            this.numericBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox2.Location = new System.Drawing.Point(356, 19);
            this.numericBox2.MaxBytes = 11;
            this.numericBox2.Name = "numericBox2";
            this.numericBox2.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox2.Size = new System.Drawing.Size(100, 23);
            this.numericBox2.TabIndex = 1;
            this.numericBox2.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(278, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 23);
            this.label10.TabIndex = 84;
            this.label10.Text = "Tax";
            // 
            // numericBox1
            // 
            this.numericBox1.BackColor = System.Drawing.Color.White;
            this.numericBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "amount", true));
            this.numericBox1.DecimalPlaces = 2;
            this.numericBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox1.Location = new System.Drawing.Point(94, 19);
            this.numericBox1.MaxBytes = 12;
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox1.Size = new System.Drawing.Size(100, 23);
            this.numericBox1.TabIndex = 0;
            this.numericBox1.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(16, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 23);
            this.label9.TabIndex = 82;
            this.label9.Text = "Amount";
            // 
            // label16
            // 
            this.label16.Lines = 0;
            this.label16.Location = new System.Drawing.Point(16, 258);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(75, 23);
            this.label16.TabIndex = 90;
            this.label16.Text = "Account No";
            // 
            // txtAccountNo1
            // 
            this.txtAccountNo1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "AccountID", true));
            this.txtAccountNo1.DisplayBox1Binding = "";
            this.txtAccountNo1.Location = new System.Drawing.Point(94, 258);
            this.txtAccountNo1.Name = "txtAccountNo1";
            this.txtAccountNo1.Size = new System.Drawing.Size(300, 23);
            this.txtAccountNo1.TabIndex = 5;
            this.txtAccountNo1.TextBox1Binding = "";
            // 
            // P36_ModifyAfterSent
            // 
            this.ClientSize = new System.Drawing.Size(565, 344);
            this.Controls.Add(this.numExchange);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.numericBox4);
            this.Controls.Add(this.numericBox2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.numericBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtAccountNo1);
            this.Controls.Add(this.editBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.DefaultControl = "numericBox1";
            this.Name = "P36_ModifyAfterSent";
            this.Text = "P36. Modify After Sent";
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button button1;
        private Win.UI.Button button2;
        private Win.UI.Label label1;
        private Win.UI.EditBox editBox1;
        private Win.UI.NumericBox numExchange;
        private Win.UI.Label label12;
        private Win.UI.NumericBox numericBox4;
        private Win.UI.NumericBox numericBox2;
        private Win.UI.Label label10;
        private Win.UI.NumericBox numericBox1;
        private Win.UI.Label label9;
        private Win.UI.Label label16;
        private Class.txtAccountNo txtAccountNo1;
        private Win.UI.ListControlBindingSource mtbs;
    }
}
