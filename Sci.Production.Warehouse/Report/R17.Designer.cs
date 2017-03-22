namespace Sci.Production.Warehouse
{
    partial class R17
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
            this.dateRange1 = new Sci.Win.UI.DateRange();
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.textBox3 = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.txtSeq1 = new Sci.Production.Class.txtSeq();
            this.label8 = new Sci.Win.UI.Label();
            this.txtfactory1 = new Sci.Production.Class.txtfactory();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(440, 12);
            this.print.TabIndex = 7;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(440, 48);
            this.toexcel.TabIndex = 8;
            this.toexcel.Click += new System.EventHandler(this.toexcel_Click);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(440, 84);
            this.close.TabIndex = 9;
            // 
            // dateRange1
            // 
            this.dateRange1.IsRequired = false;
            this.dateRange1.Location = new System.Drawing.Point(100, 44);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 23);
            this.dateRange1.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(100, 12);
            this.textBox1.MaxLength = 13;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(118, 23);
            this.textBox1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(8, 12);
            this.label2.Name = "label2";
            this.label2.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label2.Size = new System.Drawing.Size(89, 23);
            this.label2.TabIndex = 96;
            this.label2.Text = "SP#";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(251, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 23);
            this.label3.TabIndex = 97;
            this.label3.Text = "Seq#";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.PaleGreen;
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(8, 44);
            this.label1.Name = "label1";
            this.label1.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label1.Size = new System.Drawing.Size(89, 23);
            this.label1.TabIndex = 99;
            this.label1.Text = "SCI Delivery";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.PaleGreen;
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(8, 76);
            this.label4.Name = "label4";
            this.label4.RectStyle.Color = System.Drawing.Color.LightSkyBlue;
            this.label4.Size = new System.Drawing.Size(89, 23);
            this.label4.TabIndex = 100;
            this.label4.Text = "Location";
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.White;
            this.textBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox3.Location = new System.Drawing.Point(100, 76);
            this.textBox3.MaxLength = 10;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(106, 23);
            this.textBox3.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(8, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 23);
            this.label6.TabIndex = 104;
            this.label6.Text = "Stock Type";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(234, 143);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(128, 21);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Balance Qty > 0";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Items.AddRange(new object[] {
            "ALL",
            "Bulk",
            "Inventory"});
            this.comboBox1.Location = new System.Drawing.Point(99, 141);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 5;
            // 
            // txtSeq1
            // 
            this.txtSeq1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq1.Location = new System.Drawing.Point(302, 12);
            this.txtSeq1.Name = "txtSeq1";
            this.txtSeq1.seq1 = "";
            this.txtSeq1.seq2 = "";
            this.txtSeq1.Size = new System.Drawing.Size(61, 23);
            this.txtSeq1.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(8, 109);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 23);
            this.label8.TabIndex = 106;
            this.label8.Text = "Factory";
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.Location = new System.Drawing.Point(100, 109);
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 107;
            // 
            // R17
            // 
            this.ClientSize = new System.Drawing.Size(559, 237);
            this.Controls.Add(this.txtfactory1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtSeq1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dateRange1);
            this.Name = "R17";
            this.Text = "R17. Material Location Query";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.dateRange1, 0);
            this.Controls.SetChildIndex(this.textBox1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.textBox3, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.checkBox1, 0);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            this.Controls.SetChildIndex(this.txtSeq1, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.txtfactory1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateRange1;
        private Win.UI.TextBox textBox1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label1;
        private Win.UI.Label label4;
        private Win.UI.TextBox textBox3;
        private Win.UI.Label label6;
        private Win.UI.CheckBox checkBox1;
        private Win.UI.ComboBox comboBox1;
        private Class.txtSeq txtSeq1;
        private Win.UI.Label label8;
        private Class.txtfactory txtfactory1;
    }
}
