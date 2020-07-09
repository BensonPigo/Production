namespace Sci.Production.Cutting
{
    partial class R07
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.numWorkHourDay = new Sci.Win.UI.NumericBox();
            this.label8 = new Sci.Win.UI.Label();
            this.txtfactory1 = new Sci.Production.Class.Txtfactory();
            this.txtMdivision1 = new Sci.Production.Class.TxtMdivision();
            this.txtSpreadingNo2 = new Sci.Production.Class.TxtSpreadingNo();
            this.txtSpreadingNo1 = new Sci.Production.Class.TxtSpreadingNo();
            this.txtCell2 = new Sci.Production.Class.TxtCell();
            this.txtCell1 = new Sci.Production.Class.TxtCell();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCuttingSp = new Sci.Win.UI.TextBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelEstCutDate = new Sci.Win.UI.Label();
            this.dateEstCutDate = new Sci.Win.UI.DateRange();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(442, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(442, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(442, 84);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.numWorkHourDay);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtfactory1);
            this.panel1.Controls.Add(this.txtMdivision1);
            this.panel1.Controls.Add(this.txtSpreadingNo2);
            this.panel1.Controls.Add(this.txtSpreadingNo1);
            this.panel1.Controls.Add(this.txtCell2);
            this.panel1.Controls.Add(this.txtCell1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtCuttingSp);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelM);
            this.panel1.Controls.Add(this.labelEstCutDate);
            this.panel1.Controls.Add(this.dateEstCutDate);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 226);
            this.panel1.TabIndex = 94;
            // 
            // numWorkHourDay
            // 
            this.numWorkHourDay.BackColor = System.Drawing.Color.White;
            this.numWorkHourDay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numWorkHourDay.Location = new System.Drawing.Point(185, 186);
            this.numWorkHourDay.Name = "numWorkHourDay";
            this.numWorkHourDay.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWorkHourDay.Size = new System.Drawing.Size(53, 23);
            this.numWorkHourDay.TabIndex = 122;
            this.numWorkHourDay.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(13, 186);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(166, 23);
            this.label8.TabIndex = 121;
            this.label8.Text = "Work Hour/Day";
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(112, 41);
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 116;
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(112, 12);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 115;
            // 
            // txtSpreadingNo2
            // 
            this.txtSpreadingNo2.BackColor = System.Drawing.Color.White;
            this.txtSpreadingNo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpreadingNo2.Location = new System.Drawing.Point(185, 99);
            this.txtSpreadingNo2.MDivision = "";
            this.txtSpreadingNo2.Name = "txtSpreadingNo2";
            this.txtSpreadingNo2.Size = new System.Drawing.Size(45, 23);
            this.txtSpreadingNo2.TabIndex = 114;
            // 
            // txtSpreadingNo1
            // 
            this.txtSpreadingNo1.BackColor = System.Drawing.Color.White;
            this.txtSpreadingNo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpreadingNo1.Location = new System.Drawing.Point(112, 99);
            this.txtSpreadingNo1.MDivision = "";
            this.txtSpreadingNo1.Name = "txtSpreadingNo1";
            this.txtSpreadingNo1.Size = new System.Drawing.Size(45, 23);
            this.txtSpreadingNo1.TabIndex = 113;
            // 
            // txtCell2
            // 
            this.txtCell2.BackColor = System.Drawing.Color.White;
            this.txtCell2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCell2.Location = new System.Drawing.Point(162, 128);
            this.txtCell2.MDivisionID = "";
            this.txtCell2.Name = "txtCell2";
            this.txtCell2.Size = new System.Drawing.Size(30, 23);
            this.txtCell2.TabIndex = 112;
            // 
            // txtCell1
            // 
            this.txtCell1.BackColor = System.Drawing.Color.White;
            this.txtCell1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCell1.Location = new System.Drawing.Point(112, 128);
            this.txtCell1.MDivisionID = "";
            this.txtCell1.Name = "txtCell1";
            this.txtCell1.Size = new System.Drawing.Size(30, 23);
            this.txtCell1.TabIndex = 111;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(145, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 17);
            this.label5.TabIndex = 108;
            this.label5.Text = "~";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(163, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 17);
            this.label4.TabIndex = 107;
            this.label4.Text = "~";
            // 
            // txtCuttingSp
            // 
            this.txtCuttingSp.BackColor = System.Drawing.Color.White;
            this.txtCuttingSp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCuttingSp.Location = new System.Drawing.Point(112, 157);
            this.txtCuttingSp.Name = "txtCuttingSp";
            this.txtCuttingSp.Size = new System.Drawing.Size(126, 23);
            this.txtCuttingSp.TabIndex = 106;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 23);
            this.label3.TabIndex = 101;
            this.label3.Text = "Cutting SP#";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 23);
            this.label2.TabIndex = 100;
            this.label2.Text = "Cut Cell";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 23);
            this.label1.TabIndex = 99;
            this.label1.Text = "Spreading No";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(12, 41);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(95, 23);
            this.labelFactory.TabIndex = 98;
            this.labelFactory.Text = "Factory";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(12, 12);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(94, 23);
            this.labelM.TabIndex = 97;
            this.labelM.Text = "M";
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Location = new System.Drawing.Point(12, 70);
            this.labelEstCutDate.Name = "labelEstCutDate";
            this.labelEstCutDate.Size = new System.Drawing.Size(94, 23);
            this.labelEstCutDate.TabIndex = 3;
            this.labelEstCutDate.Text = "Est. Cut Date";
            // 
            // dateEstCutDate
            // 
            // 
            // 
            // 
            this.dateEstCutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEstCutDate.DateBox1.Name = "";
            this.dateEstCutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateEstCutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEstCutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateEstCutDate.DateBox2.Name = "";
            this.dateEstCutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateEstCutDate.DateBox2.TabIndex = 1;
            this.dateEstCutDate.Location = new System.Drawing.Point(112, 70);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstCutDate.TabIndex = 0;
            // 
            // R07
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 274);
            this.Controls.Add(this.panel1);
            this.IsSupportPrint = false;
            this.Name = "R07";
            this.Text = "R07.Spreading & Cutting Forecast Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelM;
        private Win.UI.Label labelEstCutDate;
        private Win.UI.DateRange dateEstCutDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private Win.UI.TextBox txtCuttingSp;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Class.TxtSpreadingNo txtSpreadingNo2;
        private Class.TxtSpreadingNo txtSpreadingNo1;
        private Class.TxtCell txtCell2;
        private Class.TxtCell txtCell1;
        private Class.Txtfactory txtfactory1;
        private Class.TxtMdivision txtMdivision1;
        private Win.UI.NumericBox numWorkHourDay;
        private Win.UI.Label label8;
    }
}