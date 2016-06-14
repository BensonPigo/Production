namespace Sci.Production.Cutting
{
    partial class P02_Print
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
            this.radioGroup1 = new Sci.Win.UI.RadioGroup();
            this.label2 = new Sci.Win.UI.Label();
            this.textBox2 = new Sci.Win.UI.TextBox();
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.Requ_ra = new Sci.Win.UI.RadioButton();
            this.Cutref_ra = new Sci.Win.UI.RadioButton();
            this.radioGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(439, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(439, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(439, 84);
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.label2);
            this.radioGroup1.Controls.Add(this.textBox2);
            this.radioGroup1.Controls.Add(this.textBox1);
            this.radioGroup1.Controls.Add(this.label1);
            this.radioGroup1.Controls.Add(this.Requ_ra);
            this.radioGroup1.Controls.Add(this.Cutref_ra);
            this.radioGroup1.Location = new System.Drawing.Point(12, 12);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(412, 166);
            this.radioGroup1.TabIndex = 94;
            this.radioGroup1.TabStop = false;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(233, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "~";
            this.label2.TextStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox2.Location = new System.Drawing.Point(255, 93);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(108, 23);
            this.textBox2.TabIndex = 4;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(122, 93);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(108, 23);
            this.textBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(44, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Cut RefNo";
            // 
            // Requ_ra
            // 
            this.Requ_ra.AutoSize = true;
            this.Requ_ra.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Requ_ra.Location = new System.Drawing.Point(44, 49);
            this.Requ_ra.Name = "Requ_ra";
            this.Requ_ra.Size = new System.Drawing.Size(109, 21);
            this.Requ_ra.TabIndex = 1;
            this.Requ_ra.TabStop = true;
            this.Requ_ra.Text = "By Cutplan Id";
            this.Requ_ra.UseVisualStyleBackColor = true;
            this.Requ_ra.CheckedChanged += new System.EventHandler(this.Requ_ra_CheckedChanged);
            // 
            // Cutref_ra
            // 
            this.Cutref_ra.AutoSize = true;
            this.Cutref_ra.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Cutref_ra.Location = new System.Drawing.Point(44, 22);
            this.Cutref_ra.Name = "Cutref_ra";
            this.Cutref_ra.Size = new System.Drawing.Size(111, 21);
            this.Cutref_ra.TabIndex = 0;
            this.Cutref_ra.TabStop = true;
            this.Cutref_ra.Text = "By Cut RefNo";
            this.Cutref_ra.UseVisualStyleBackColor = true;
            // 
            // P02_Print
            // 
            this.ClientSize = new System.Drawing.Size(531, 220);
            this.Controls.Add(this.radioGroup1);
            this.IsSupportToPrint = false;
            this.Name = "P02_Print";
            this.Text = "Spreading Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioGroup1, 0);
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioGroup radioGroup1;
        private Win.UI.Label label2;
        private Win.UI.TextBox textBox2;
        private Win.UI.TextBox textBox1;
        private Win.UI.Label label1;
        private Win.UI.RadioButton Requ_ra;
        private Win.UI.RadioButton Cutref_ra;
    }
}
