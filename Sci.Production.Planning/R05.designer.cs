namespace Sci.Production.Planning
{
    partial class R05
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
            this.components = new System.ComponentModel.Container();
            this.numYear = new Sci.Win.UI.NumericUpDown();
            this.cmbDate = new Sci.Win.UI.ComboBox();
            this.chkOrder = new Sci.Win.UI.CheckBox();
            this.chkForecast = new Sci.Win.UI.CheckBox();
            this.chkFtyLocalOrder = new Sci.Win.UI.CheckBox();
            this.chkExcludeSampleFactory = new Sci.Win.UI.CheckBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.txtbrand1 = new Sci.Production.Class.Txtbrand();
            this.comboFtyZone = new Sci.Production.Class.ComboFtyZone(this.components);
            this.radioMonthly = new Sci.Win.UI.RadioButton();
            this.radioSemiMonthly = new Sci.Win.UI.RadioButton();
            this.comboMDivision = new Sci.Production.Class.ComboMDivision(this.components);
            this.comboFactory = new Sci.Production.Class.ComboFactory(this.components);
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.chkCMPLockDate = new Sci.Win.UI.CheckBox();
            this.dateOutputDate = new Sci.Win.UI.DateBox();
            this.label8 = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).BeginInit();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(499, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(499, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(499, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(453, 120);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(479, 156);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(479, 183);
            // 
            // numYear
            // 
            this.numYear.BackColor = System.Drawing.Color.White;
            this.numYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numYear.Location = new System.Drawing.Point(123, 54);
            this.numYear.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numYear.Minimum = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.numYear.Name = "numYear";
            this.numYear.Size = new System.Drawing.Size(80, 24);
            this.numYear.TabIndex = 97;
            this.numYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numYear.Value = new decimal(new int[] {
            2019,
            0,
            0,
            0});
            // 
            // cmbDate
            // 
            this.cmbDate.BackColor = System.Drawing.Color.White;
            this.cmbDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbDate.FormattingEnabled = true;
            this.cmbDate.IsSupportUnselect = true;
            this.cmbDate.Location = new System.Drawing.Point(123, 202);
            this.cmbDate.Name = "cmbDate";
            this.cmbDate.OldText = "";
            this.cmbDate.Size = new System.Drawing.Size(267, 26);
            this.cmbDate.TabIndex = 102;
            // 
            // chkOrder
            // 
            this.chkOrder.AutoSize = true;
            this.chkOrder.Checked = true;
            this.chkOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkOrder.Location = new System.Drawing.Point(123, 232);
            this.chkOrder.Name = "chkOrder";
            this.chkOrder.Size = new System.Drawing.Size(69, 22);
            this.chkOrder.TabIndex = 103;
            this.chkOrder.Text = "Order ";
            this.chkOrder.UseVisualStyleBackColor = true;
            // 
            // chkForecast
            // 
            this.chkForecast.AutoSize = true;
            this.chkForecast.Checked = true;
            this.chkForecast.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkForecast.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkForecast.Location = new System.Drawing.Point(123, 259);
            this.chkForecast.Name = "chkForecast";
            this.chkForecast.Size = new System.Drawing.Size(90, 22);
            this.chkForecast.TabIndex = 104;
            this.chkForecast.Text = "Forecast ";
            this.chkForecast.UseVisualStyleBackColor = true;
            // 
            // chkFtyLocalOrder
            // 
            this.chkFtyLocalOrder.AutoSize = true;
            this.chkFtyLocalOrder.Checked = true;
            this.chkFtyLocalOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFtyLocalOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkFtyLocalOrder.Location = new System.Drawing.Point(123, 286);
            this.chkFtyLocalOrder.Name = "chkFtyLocalOrder";
            this.chkFtyLocalOrder.Size = new System.Drawing.Size(129, 22);
            this.chkFtyLocalOrder.TabIndex = 105;
            this.chkFtyLocalOrder.Text = "Fty Local Order";
            this.chkFtyLocalOrder.UseVisualStyleBackColor = true;
            // 
            // chkExcludeSampleFactory
            // 
            this.chkExcludeSampleFactory.AutoSize = true;
            this.chkExcludeSampleFactory.Checked = true;
            this.chkExcludeSampleFactory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExcludeSampleFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkExcludeSampleFactory.Location = new System.Drawing.Point(9, 372);
            this.chkExcludeSampleFactory.Name = "chkExcludeSampleFactory";
            this.chkExcludeSampleFactory.Size = new System.Drawing.Size(187, 22);
            this.chkExcludeSampleFactory.TabIndex = 106;
            this.chkExcludeSampleFactory.Text = "Exclude Sample Factory";
            this.chkExcludeSampleFactory.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 23);
            this.label1.TabIndex = 107;
            this.label1.Text = "Year";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 23);
            this.label2.TabIndex = 108;
            this.label2.Text = "Brand";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 23);
            this.label3.TabIndex = 109;
            this.label3.Text = "M";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 23);
            this.label4.TabIndex = 110;
            this.label4.Text = "FtyZone";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 173);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 23);
            this.label5.TabIndex = 111;
            this.label5.Text = "Factory";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(9, 203);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 23);
            this.label6.TabIndex = 112;
            this.label6.Text = "Date";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(9, 232);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(111, 23);
            this.label7.TabIndex = 113;
            this.label7.Text = "Source";
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(123, 83);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(80, 24);
            this.txtbrand1.TabIndex = 98;
            // 
            // comboFtyZone
            // 
            this.comboFtyZone.BackColor = System.Drawing.Color.White;
            this.comboFtyZone.FilteMDivision = false;
            this.comboFtyZone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFtyZone.FormattingEnabled = true;
            this.comboFtyZone.IsIncludeSampleRoom = false;
            this.comboFtyZone.IsProduceFty = true;
            this.comboFtyZone.IssupportJunk = false;
            this.comboFtyZone.IsSupportUnselect = true;
            this.comboFtyZone.Location = new System.Drawing.Point(123, 141);
            this.comboFtyZone.Name = "comboFtyZone";
            this.comboFtyZone.OldText = "";
            this.comboFtyZone.SelectTable = "Factory";
            this.comboFtyZone.Size = new System.Drawing.Size(80, 26);
            this.comboFtyZone.TabIndex = 114;
            // 
            // radioMonthly
            // 
            this.radioMonthly.AutoSize = true;
            this.radioMonthly.Checked = true;
            this.radioMonthly.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioMonthly.Location = new System.Drawing.Point(9, 2);
            this.radioMonthly.Name = "radioMonthly";
            this.radioMonthly.Size = new System.Drawing.Size(127, 22);
            this.radioMonthly.TabIndex = 115;
            this.radioMonthly.TabStop = true;
            this.radioMonthly.Text = "Monthly Report";
            this.radioMonthly.UseVisualStyleBackColor = true;
            // 
            // radioSemiMonthly
            // 
            this.radioSemiMonthly.AutoSize = true;
            this.radioSemiMonthly.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSemiMonthly.Location = new System.Drawing.Point(9, 27);
            this.radioSemiMonthly.Name = "radioSemiMonthly";
            this.radioSemiMonthly.Size = new System.Drawing.Size(166, 22);
            this.radioSemiMonthly.TabIndex = 116;
            this.radioSemiMonthly.TabStop = true;
            this.radioSemiMonthly.Text = "Semi-monthly Report";
            this.radioSemiMonthly.UseVisualStyleBackColor = true;
            // 
            // comboMDivision
            // 
            this.comboMDivision.BackColor = System.Drawing.Color.White;
            this.comboMDivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMDivision.FormattingEnabled = true;
            this.comboMDivision.IsSupportUnselect = true;
            this.comboMDivision.Location = new System.Drawing.Point(123, 111);
            this.comboMDivision.Name = "comboMDivision";
            this.comboMDivision.OldText = "";
            this.comboMDivision.Size = new System.Drawing.Size(80, 26);
            this.comboMDivision.TabIndex = 117;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.FilteMDivision = false;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IssupportJunk = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(123, 173);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(80, 26);
            this.comboFactory.TabIndex = 118;
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.Checked = true;
            this.chkIncludeCancelOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(9, 399);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(162, 22);
            this.chkIncludeCancelOrder.TabIndex = 119;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // chkCMPLockDate
            // 
            this.chkCMPLockDate.AutoSize = true;
            this.chkCMPLockDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkCMPLockDate.Location = new System.Drawing.Point(9, 345);
            this.chkCMPLockDate.Name = "chkCMPLockDate";
            this.chkCMPLockDate.Size = new System.Drawing.Size(210, 22);
            this.chkCMPLockDate.TabIndex = 120;
            this.chkCMPLockDate.Text = "By CMP Monthly Lock Date";
            this.chkCMPLockDate.UseVisualStyleBackColor = true;
            // 
            // dateOutputDate
            // 
            this.dateOutputDate.Location = new System.Drawing.Point(123, 314);
            this.dateOutputDate.Name = "dateOutputDate";
            this.dateOutputDate.Size = new System.Drawing.Size(130, 24);
            this.dateOutputDate.TabIndex = 125;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(9, 314);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 23);
            this.label8.TabIndex = 124;
            this.label8.Text = "Output Date";
            // 
            // R05
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 458);
            this.Controls.Add(this.dateOutputDate);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkCMPLockDate);
            this.Controls.Add(this.chkIncludeCancelOrder);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboMDivision);
            this.Controls.Add(this.radioSemiMonthly);
            this.Controls.Add(this.radioMonthly);
            this.Controls.Add(this.comboFtyZone);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkExcludeSampleFactory);
            this.Controls.Add(this.chkFtyLocalOrder);
            this.Controls.Add(this.chkForecast);
            this.Controls.Add(this.chkOrder);
            this.Controls.Add(this.cmbDate);
            this.Controls.Add(this.txtbrand1);
            this.Controls.Add(this.numYear);
            this.Name = "R05";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R05. Loading & Production output Summary";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.numYear, 0);
            this.Controls.SetChildIndex(this.txtbrand1, 0);
            this.Controls.SetChildIndex(this.cmbDate, 0);
            this.Controls.SetChildIndex(this.chkOrder, 0);
            this.Controls.SetChildIndex(this.chkForecast, 0);
            this.Controls.SetChildIndex(this.chkFtyLocalOrder, 0);
            this.Controls.SetChildIndex(this.chkExcludeSampleFactory, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.comboFtyZone, 0);
            this.Controls.SetChildIndex(this.radioMonthly, 0);
            this.Controls.SetChildIndex(this.radioSemiMonthly, 0);
            this.Controls.SetChildIndex(this.comboMDivision, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.chkIncludeCancelOrder, 0);
            this.Controls.SetChildIndex(this.chkCMPLockDate, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.dateOutputDate, 0);
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.NumericUpDown numYear;
        private Class.Txtbrand txtbrand1;
        private Win.UI.ComboBox cmbDate;
        private Win.UI.CheckBox chkOrder;
        private Win.UI.CheckBox chkForecast;
        private Win.UI.CheckBox chkFtyLocalOrder;
        private Win.UI.CheckBox chkExcludeSampleFactory;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Class.ComboFtyZone comboFtyZone;
        private Win.UI.RadioButton radioMonthly;
        private Win.UI.RadioButton radioSemiMonthly;
        private Class.ComboMDivision comboMDivision;
        private Class.ComboFactory comboFactory;
        private Win.UI.CheckBox chkIncludeCancelOrder;
        private Win.UI.CheckBox chkCMPLockDate;
        private Win.UI.DateBox dateOutputDate;
        private Win.UI.Label label8;
    }
}