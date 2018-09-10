namespace Sci.Production.PPIC
{
    partial class R13
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dateRangeReadyDate = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.checkHoliday = new Sci.Win.UI.CheckBox();
            this.label6 = new Sci.Win.UI.Label();
            this.txtFactory = new Sci.Production.Class.txtfactory();
            this.txtBrand = new Sci.Production.Class.txtbrand();
            this.txtMdivision = new Sci.Production.Class.txtMdivision();
            this.txtTime = new Sci.Win.UI.TextBox();
            this.numericBoxDateGap = new Sci.Win.UI.NumericBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(529, 160);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(529, 12);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(529, 48);
            // 
            // dateRangeReadyDate
            // 
            // 
            // 
            // 
            this.dateRangeReadyDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeReadyDate.DateBox1.Name = "";
            this.dateRangeReadyDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeReadyDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeReadyDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeReadyDate.DateBox2.Name = "";
            this.dateRangeReadyDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeReadyDate.DateBox2.TabIndex = 1;
            this.dateRangeReadyDate.Location = new System.Drawing.Point(111, 19);
            this.dateRangeReadyDate.Name = "dateRangeReadyDate";
            this.dateRangeReadyDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeReadyDate.TabIndex = 94;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 23);
            this.label1.TabIndex = 95;
            this.label1.Text = "Ready Date";
            this.label1.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 23);
            this.label2.TabIndex = 96;
            this.label2.Text = "M ";
            this.label2.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 23);
            this.label3.TabIndex = 97;
            this.label3.Text = "Factory ";
            this.label3.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(24, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 23);
            this.label4.TabIndex = 98;
            this.label4.Text = "Brand ";
            this.label4.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(24, 164);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(297, 23);
            this.label5.TabIndex = 99;
            this.label5.Text = "Ready Date and Sewing Offline Date gap(Days)";
            this.label5.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkHoliday
            // 
            this.checkHoliday.AutoSize = true;
            this.checkHoliday.Checked = true;
            this.checkHoliday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkHoliday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkHoliday.Location = new System.Drawing.Point(402, 21);
            this.checkHoliday.Name = "checkHoliday";
            this.checkHoliday.Size = new System.Drawing.Size(127, 21);
            this.checkHoliday.TabIndex = 100;
            this.checkHoliday.Text = "Exclude Holiday";
            this.checkHoliday.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(364, 164);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 23);
            this.label6.TabIndex = 101;
            this.label6.Text = "Time ";
            this.label6.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.FilteMDivision = false;
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.IssupportJunk = false;
            this.txtFactory.Location = new System.Drawing.Point(111, 92);
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(66, 23);
            this.txtFactory.TabIndex = 102;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(111, 131);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(66, 23);
            this.txtBrand.TabIndex = 103;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(111, 55);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 104;
            // 
            // txtTime
            // 
            this.txtTime.BackColor = System.Drawing.Color.White;
            this.txtTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTime.Location = new System.Drawing.Point(412, 164);
            this.txtTime.Mask = "90:00";
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(48, 23);
            this.txtTime.TabIndex = 105;
            this.txtTime.Text = "1000";
            this.txtTime.ValidatingType = typeof(System.DateTime);
            // 
            // numericBoxDateGap
            // 
            this.numericBoxDateGap.BackColor = System.Drawing.Color.White;
            this.numericBoxDateGap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBoxDateGap.Location = new System.Drawing.Point(324, 164);
            this.numericBoxDateGap.MaxLength = 1;
            this.numericBoxDateGap.Name = "numericBoxDateGap";
            this.numericBoxDateGap.NullValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericBoxDateGap.Size = new System.Drawing.Size(26, 23);
            this.numericBoxDateGap.TabIndex = 106;
            this.numericBoxDateGap.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // R13
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 222);
            this.Controls.Add(this.numericBoxDateGap);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.txtFactory);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkHoliday);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateRangeReadyDate);
            this.Name = "R13";
            this.Text = "R13. SubProcess Ready Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.dateRangeReadyDate, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.checkHoliday, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtFactory, 0);
            this.Controls.SetChildIndex(this.txtBrand, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtTime, 0);
            this.Controls.SetChildIndex(this.numericBoxDateGap, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateRangeReadyDate;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.CheckBox checkHoliday;
        private Win.UI.Label label6;
        private Class.txtfactory txtFactory;
        private Class.txtbrand txtBrand;
        private Class.txtMdivision txtMdivision;
        private Win.UI.TextBox txtTime;
        private Win.UI.NumericBox numericBoxDateGap;
    }
}