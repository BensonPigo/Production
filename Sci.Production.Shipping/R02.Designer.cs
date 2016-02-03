namespace Sci.Production.Shipping
{
    partial class R02
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
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.dateRange1 = new Sci.Win.UI.DateRange();
            this.dateRange2 = new Sci.Win.UI.DateRange();
            this.dateRange3 = new Sci.Win.UI.DateRange();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.textBox2 = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(413, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(413, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(413, 84);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Pullout Date";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(13, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 23);
            this.label2.TabIndex = 95;
            this.label2.Text = "SP No";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(13, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 23);
            this.label3.TabIndex = 96;
            this.label3.Text = "SDP Date";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(13, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 23);
            this.label4.TabIndex = 97;
            this.label4.Text = "SCI Delivery";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(13, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 23);
            this.label5.TabIndex = 98;
            this.label5.Text = "M";
            // 
            // dateRange1
            // 
            this.dateRange1.Location = new System.Drawing.Point(97, 12);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 23);
            this.dateRange1.TabIndex = 99;
            // 
            // dateRange2
            // 
            this.dateRange2.Location = new System.Drawing.Point(96, 84);
            this.dateRange2.Name = "dateRange2";
            this.dateRange2.Size = new System.Drawing.Size(280, 23);
            this.dateRange2.TabIndex = 100;
            // 
            // dateRange3
            // 
            this.dateRange3.Location = new System.Drawing.Point(97, 120);
            this.dateRange3.Name = "dateRange3";
            this.dateRange3.Size = new System.Drawing.Size(280, 23);
            this.dateRange3.TabIndex = 101;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(97, 156);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(81, 24);
            this.comboBox1.TabIndex = 102;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(97, 48);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(130, 23);
            this.textBox1.TabIndex = 103;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox2.Location = new System.Drawing.Point(252, 48);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(130, 23);
            this.textBox2.TabIndex = 104;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(228, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 23);
            this.label6.TabIndex = 105;
            this.label6.Text = "～";
            this.label6.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            this.label6.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label6.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // R02
            // 
            this.ClientSize = new System.Drawing.Size(505, 217);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dateRange3);
            this.Controls.Add(this.dateRange2);
            this.Controls.Add(this.dateRange1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.EditMode = true;
            this.IsSupportToPrint = false;
            this.Name = "R02";
            this.Text = "R02. Pullout Report List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.dateRange1, 0);
            this.Controls.SetChildIndex(this.dateRange2, 0);
            this.Controls.SetChildIndex(this.dateRange3, 0);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            this.Controls.SetChildIndex(this.textBox1, 0);
            this.Controls.SetChildIndex(this.textBox2, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.DateRange dateRange1;
        private Win.UI.DateRange dateRange2;
        private Win.UI.DateRange dateRange3;
        private Win.UI.ComboBox comboBox1;
        private Win.UI.TextBox textBox1;
        private Win.UI.TextBox textBox2;
        private Win.UI.Label label6;
    }
}
