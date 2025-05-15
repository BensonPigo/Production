namespace Sci.Production.Cutting
{
    partial class R08
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
            this.numSpeed = new Sci.Win.UI.NumericBox();
            this.label7 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.dateActCutDate = new Sci.Win.UI.DateRange();
            this.txtfactory1 = new Sci.Production.Class.Txtfactory();
            this.txtMdivision1 = new Sci.Production.Class.TxtMdivision();
            this.txtCuttingSp = new Sci.Win.UI.TextBox();
            this.label3 = new Sci.Win.UI.Label();
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
            this.print.TabIndex = 0;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(442, 48);
            this.toexcel.TabIndex = 1;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(442, 84);
            this.close.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.numWorkHourDay);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.numSpeed);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.dateActCutDate);
            this.panel1.Controls.Add(this.txtfactory1);
            this.panel1.Controls.Add(this.txtMdivision1);
            this.panel1.Controls.Add(this.txtCuttingSp);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelM);
            this.panel1.Controls.Add(this.labelEstCutDate);
            this.panel1.Controls.Add(this.dateEstCutDate);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 215);
            this.panel1.TabIndex = 94;
            // 
            // numWorkHourDay
            // 
            this.numWorkHourDay.BackColor = System.Drawing.Color.White;
            this.numWorkHourDay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numWorkHourDay.Location = new System.Drawing.Point(184, 186);
            this.numWorkHourDay.Name = "numWorkHourDay";
            this.numWorkHourDay.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWorkHourDay.Size = new System.Drawing.Size(53, 23);
            this.numWorkHourDay.TabIndex = 10;
            this.numWorkHourDay.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(12, 186);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(166, 23);
            this.label8.TabIndex = 121;
            this.label8.Text = "Work Hour/Day";
            // 
            // numSpeed
            // 
            this.numSpeed.BackColor = System.Drawing.Color.White;
            this.numSpeed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numSpeed.Location = new System.Drawing.Point(184, 157);
            this.numSpeed.Name = "numSpeed";
            this.numSpeed.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSpeed.Size = new System.Drawing.Size(53, 23);
            this.numSpeed.TabIndex = 9;
            this.numSpeed.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(166, 23);
            this.label7.TabIndex = 119;
            this.label7.Text = "Factory Max speed m/sec";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 23);
            this.label6.TabIndex = 118;
            this.label6.Text = "Act. Cut Date ";
            // 
            // dateActCutDate
            // 
            // 
            // 
            // 
            this.dateActCutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateActCutDate.DateBox1.Name = "";
            this.dateActCutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateActCutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateActCutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateActCutDate.DateBox2.Name = "";
            this.dateActCutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateActCutDate.DateBox2.TabIndex = 1;
            this.dateActCutDate.Location = new System.Drawing.Point(112, 99);
            this.dateActCutDate.Name = "dateActCutDate";
            this.dateActCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateActCutDate.TabIndex = 3;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.BoolFtyGroupList = true;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IsMultiselect = false;
            this.txtfactory1.IsProduceFty = false;
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(112, 41);
            this.txtfactory1.MDivision = null;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 1;
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(112, 12);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 0;
            // 
            // txtCuttingSp
            // 
            this.txtCuttingSp.BackColor = System.Drawing.Color.White;
            this.txtCuttingSp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCuttingSp.Location = new System.Drawing.Point(111, 128);
            this.txtCuttingSp.Name = "txtCuttingSp";
            this.txtCuttingSp.Size = new System.Drawing.Size(126, 23);
            this.txtCuttingSp.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 23);
            this.label3.TabIndex = 101;
            this.label3.Text = "Cutting SP#";
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
            this.dateEstCutDate.TabIndex = 2;
            // 
            // R08
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 258);
            this.Controls.Add(this.panel1);
            this.IsSupportPrint = false;
            this.Name = "R08";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R08.Actual Cutting Output Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
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
        private Win.UI.TextBox txtCuttingSp;
        private Win.UI.Label label3;
        private Class.Txtfactory txtfactory1;
        private Class.TxtMdivision txtMdivision1;
        private Win.UI.Label label6;
        private Win.UI.DateRange dateActCutDate;
        private Win.UI.NumericBox numSpeed;
        private Win.UI.Label label7;
        private Win.UI.NumericBox numWorkHourDay;
        private Win.UI.Label label8;
    }
}