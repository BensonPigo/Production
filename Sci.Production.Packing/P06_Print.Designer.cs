namespace Sci.Production.Packing
{
    partial class P06_Print
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
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.label1 = new Sci.Win.UI.Label();
            this.radioButton2 = new Sci.Win.UI.RadioButton();
            this.radioButton1 = new Sci.Win.UI.RadioButton();
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.textBox2 = new Sci.Win.UI.TextBox();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(288, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(288, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(288, 84);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioButton2);
            this.radioPanel1.Controls.Add(this.radioButton1);
            this.radioPanel1.Location = new System.Drawing.Point(20, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(198, 70);
            this.radioPanel1.TabIndex = 94;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(55, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "CTN#";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioButton2.Location = new System.Drawing.Point(13, 47);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(112, 21);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Barcode Print";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioButton1.Location = new System.Drawing.Point(13, 11);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(165, 21);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Packing Guide Report";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(102, 86);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(56, 23);
            this.textBox1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(162, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "～";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            this.label2.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox2.Location = new System.Drawing.Point(185, 86);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(56, 23);
            this.textBox2.TabIndex = 5;
            // 
            // P06_Print
            // 
            this.ClientSize = new System.Drawing.Size(380, 159);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.EditMode = true;
            this.Name = "P06_Print";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.textBox1, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.textBox2, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.Label label1;
        private Win.UI.RadioButton radioButton2;
        private Win.UI.RadioButton radioButton1;
        private Win.UI.TextBox textBox1;
        private Win.UI.Label label2;
        private Win.UI.TextBox textBox2;
    }
}
