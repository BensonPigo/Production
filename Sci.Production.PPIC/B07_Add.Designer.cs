namespace Sci.Production.PPIC
{
    partial class B07_Add
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
            this.label1 = new Sci.Win.UI.Label();
            this.dateRange1 = new Sci.Win.UI.DateRange();
            this.label2 = new Sci.Win.UI.Label();
            this.numericBox1 = new Sci.Win.UI.NumericBox();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.label3 = new Sci.Win.UI.Label();
            this.button1 = new Sci.Win.UI.Button();
            this.button2 = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(18, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Date";
            // 
            // dateRange1
            // 
            this.dateRange1.Location = new System.Drawing.Point(69, 14);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 23);
            this.dateRange1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(18, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Hours";
            // 
            // numericBox1
            // 
            this.numericBox1.BackColor = System.Drawing.Color.White;
            this.numericBox1.DecimalPlaces = 1;
            this.numericBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox1.Location = new System.Drawing.Point(69, 50);
            this.numericBox1.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            65536});
            this.numericBox1.MaxLength = 4;
            this.numericBox1.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.Size = new System.Drawing.Size(46, 23);
            this.numericBox1.TabIndex = 3;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(18, 85);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(133, 21);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Include Saturday";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(18, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(190, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Sunday will be non-office day";
            this.label3.TextStyle.BorderColor = System.Drawing.Color.Blue;
            this.label3.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button1.Location = new System.Drawing.Point(178, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 6;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button2.Location = new System.Drawing.Point(264, 144);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 30);
            this.button2.TabIndex = 7;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // B07_Add
            // 
            this.ClientSize = new System.Drawing.Size(360, 187);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.numericBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateRange1);
            this.Controls.Add(this.label1);
            this.Name = "B07_Add";
            this.Text = "Add";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.DateRange dateRange1;
        private Win.UI.Label label2;
        private Win.UI.NumericBox numericBox1;
        private Win.UI.CheckBox checkBox1;
        private Win.UI.Label label3;
        private Win.UI.Button button1;
        private Win.UI.Button button2;
    }
}
