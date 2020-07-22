namespace Sci.Production.Centralized
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
            this.radioMonthly = new Sci.Win.UI.RadioButton();
            this.radioSemiMonthly = new Sci.Win.UI.RadioButton();
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.comboFtyZone = new Sci.Production.Class.ComboFtyZone(this.components);
            this.comboCentralizedFactory1 = new Sci.Production.Class.ComboCentralizedFactory(this.components);
            this.comboCentralizedM1 = new Sci.Production.Class.ComboCentralizedM(this.components);
            this.txtbrand1 = new Sci.Production.Class.Txtbrand();
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
            this.numYear.Size = new System.Drawing.Size(80, 23);
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
            this.cmbDate.Size = new System.Drawing.Size(267, 24);
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
            this.chkOrder.Size = new System.Drawing.Size(68, 21);
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
            this.chkForecast.Size = new System.Drawing.Size(86, 21);
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
            this.chkFtyLocalOrder.Size = new System.Drawing.Size(125, 21);
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
            this.chkExcludeSampleFactory.Location = new System.Drawing.Point(12, 321);
            this.chkExcludeSampleFactory.Name = "chkExcludeSampleFactory";
            this.chkExcludeSampleFactory.Size = new System.Drawing.Size(178, 21);
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
            // radioMonthly
            // 
            this.radioMonthly.AutoSize = true;
            this.radioMonthly.Checked = true;
            this.radioMonthly.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioMonthly.Location = new System.Drawing.Point(9, 2);
            this.radioMonthly.Name = "radioMonthly";
            this.radioMonthly.Size = new System.Drawing.Size(122, 21);
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
            this.radioSemiMonthly.Size = new System.Drawing.Size(158, 21);
            this.radioSemiMonthly.TabIndex = 116;
            this.radioSemiMonthly.TabStop = true;
            this.radioSemiMonthly.Text = "Semi-monthly Report";
            this.radioSemiMonthly.UseVisualStyleBackColor = true;
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.Checked = true;
            this.chkIncludeCancelOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(13, 348);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 117;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
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
            this.comboFtyZone.Size = new System.Drawing.Size(80, 24);
            this.comboFtyZone.TabIndex = 114;
            // 
            // comboCentralizedFactory1
            // 
            this.comboCentralizedFactory1.BackColor = System.Drawing.Color.White;
            this.comboCentralizedFactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCentralizedFactory1.FormattingEnabled = true;
            this.comboCentralizedFactory1.IsSupportUnselect = true;
            this.comboCentralizedFactory1.Location = new System.Drawing.Point(123, 172);
            this.comboCentralizedFactory1.Name = "comboCentralizedFactory1";
            this.comboCentralizedFactory1.OldText = "";
            this.comboCentralizedFactory1.Size = new System.Drawing.Size(80, 24);
            this.comboCentralizedFactory1.TabIndex = 101;
            // 
            // comboCentralizedM1
            // 
            this.comboCentralizedM1.BackColor = System.Drawing.Color.White;
            this.comboCentralizedM1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCentralizedM1.FormattingEnabled = true;
            this.comboCentralizedM1.IsSupportUnselect = true;
            this.comboCentralizedM1.Location = new System.Drawing.Point(123, 112);
            this.comboCentralizedM1.Name = "comboCentralizedM1";
            this.comboCentralizedM1.OldText = "";
            this.comboCentralizedM1.Size = new System.Drawing.Size(80, 24);
            this.comboCentralizedM1.TabIndex = 99;
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(123, 83);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(80, 23);
            this.txtbrand1.TabIndex = 98;
            // 
            // R05
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 400);
            this.Controls.Add(this.chkIncludeCancelOrder);
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
            this.Controls.Add(this.comboCentralizedFactory1);
            this.Controls.Add(this.comboCentralizedM1);
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
            this.Controls.SetChildIndex(this.comboCentralizedM1, 0);
            this.Controls.SetChildIndex(this.comboCentralizedFactory1, 0);
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
            this.Controls.SetChildIndex(this.chkIncludeCancelOrder, 0);
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.NumericUpDown numYear;
        private Class.Txtbrand txtbrand1;
        private Class.ComboCentralizedM comboCentralizedM1;
        private Class.ComboCentralizedFactory comboCentralizedFactory1;
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
        private Win.UI.CheckBox chkIncludeCancelOrder;
    }
}