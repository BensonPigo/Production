namespace Sci.Production.Cutting
{
    partial class R04
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.radioBtnByDetail = new Sci.Win.UI.RadioButton();
            this.radioBtnByCutCell = new Sci.Win.UI.RadioButton();
            this.radiobtnByM = new Sci.Win.UI.RadioButton();
            this.cmbM = new Sci.Win.UI.ComboBox();
            this.textBox2 = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.dateRangeEstCutDate = new Sci.Win.UI.DateRange();
            this.label5 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.radioBtnByDetail);
            this.panel1.Controls.Add(this.radioBtnByCutCell);
            this.panel1.Controls.Add(this.radiobtnByM);
            this.panel1.Controls.Add(this.cmbM);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.dateRangeEstCutDate);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(483, 239);
            this.panel1.TabIndex = 95;
            // 
            // radioBtnByDetail
            // 
            this.radioBtnByDetail.AutoSize = true;
            this.radioBtnByDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBtnByDetail.Location = new System.Drawing.Point(136, 203);
            this.radioBtnByDetail.Name = "radioBtnByDetail";
            this.radioBtnByDetail.Size = new System.Drawing.Size(82, 21);
            this.radioBtnByDetail.TabIndex = 114;
            this.radioBtnByDetail.Text = "By Detail";
            this.radioBtnByDetail.UseVisualStyleBackColor = true;
            this.radioBtnByDetail.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioBtnByCutCell
            // 
            this.radioBtnByCutCell.AutoSize = true;
            this.radioBtnByCutCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBtnByCutCell.Location = new System.Drawing.Point(136, 176);
            this.radioBtnByCutCell.Name = "radioBtnByCutCell";
            this.radioBtnByCutCell.Size = new System.Drawing.Size(94, 21);
            this.radioBtnByCutCell.TabIndex = 113;
            this.radioBtnByCutCell.Text = "By Cut Cell";
            this.radioBtnByCutCell.UseVisualStyleBackColor = true;
            this.radioBtnByCutCell.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radiobtnByM
            // 
            this.radiobtnByM.AutoSize = true;
            this.radiobtnByM.Checked = true;
            this.radiobtnByM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobtnByM.Location = new System.Drawing.Point(136, 149);
            this.radiobtnByM.Name = "radiobtnByM";
            this.radiobtnByM.Size = new System.Drawing.Size(57, 21);
            this.radiobtnByM.TabIndex = 112;
            this.radiobtnByM.TabStop = true;
            this.radiobtnByM.Text = "By M";
            this.radiobtnByM.UseVisualStyleBackColor = true;
            this.radiobtnByM.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // cmbM
            // 
            this.cmbM.BackColor = System.Drawing.Color.White;
            this.cmbM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbM.FormattingEnabled = true;
            this.cmbM.IsSupportUnselect = true;
            this.cmbM.Location = new System.Drawing.Point(136, 13);
            this.cmbM.Name = "cmbM";
            this.cmbM.Size = new System.Drawing.Size(121, 24);
            this.cmbM.TabIndex = 111;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.Enabled = false;
            this.textBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox2.Location = new System.Drawing.Point(227, 104);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(65, 23);
            this.textBox2.TabIndex = 110;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(204, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 23);
            this.label9.TabIndex = 109;
            this.label9.Text = "～";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Enabled = false;
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(136, 104);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(65, 23);
            this.textBox1.TabIndex = 108;
            // 
            // dateRangeEstCutDate
            // 
            this.dateRangeEstCutDate.Location = new System.Drawing.Point(136, 60);
            this.dateRangeEstCutDate.Name = "dateRangeEstCutDate";
            this.dateRangeEstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeEstCutDate.TabIndex = 100;
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(11, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 23);
            this.label5.TabIndex = 99;
            this.label5.Text = "Report Type";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(11, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 23);
            this.label3.TabIndex = 98;
            this.label3.Text = "Cut Cell";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(11, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 23);
            this.label1.TabIndex = 96;
            this.label1.Text = "M";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(11, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 23);
            this.label2.TabIndex = 115;
            this.label2.Text = "Est. Cut Date";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(523, 229);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 22);
            this.label4.TabIndex = 96;
            this.label4.Text = "Paper Size A4";
            // 
            // R04
            // 
            this.ClientSize = new System.Drawing.Size(627, 314);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.Name = "R04";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.RadioButton radioBtnByDetail;
        private Win.UI.RadioButton radioBtnByCutCell;
        private Win.UI.RadioButton radiobtnByM;
        private Win.UI.ComboBox cmbM;
        private Win.UI.TextBox textBox2;
        private Win.UI.Label label9;
        private Win.UI.TextBox textBox1;
        private Win.UI.DateRange dateRangeEstCutDate;
        private Win.UI.Label label5;
        private Win.UI.Label label3;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label4;

    }
}
