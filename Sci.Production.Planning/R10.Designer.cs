namespace Sci.Production.Planning
{
    partial class R10
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
            this.radioGroup2 = new Sci.Win.UI.RadioGroup();
            this.radioProductionStatus = new Sci.Win.UI.RadioButton();
            this.txtFactory = new Sci.Production.Class.txtfactory();
            this.txtM = new Sci.Production.Class.txtMdivision();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.checkFty = new Sci.Win.UI.CheckBox();
            this.checkForecast = new Sci.Win.UI.CheckBox();
            this.checkOrder = new Sci.Win.UI.CheckBox();
            this.comboReport = new Sci.Win.UI.ComboBox();
            this.comboDate = new Sci.Win.UI.ComboBox();
            this.txtBrand = new Sci.Production.Class.txtbrand();
            this.numMonth = new System.Windows.Forms.NumericUpDown();
            this.numYear = new System.Windows.Forms.NumericUpDown();
            this.labelMonth = new Sci.Win.UI.Label();
            this.labelSource = new Sci.Win.UI.Label();
            this.labelReport = new Sci.Win.UI.Label();
            this.labelDate = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelYear = new Sci.Win.UI.Label();
            this.radioSemimonthlyReport = new System.Windows.Forms.RadioButton();
            this.radioMonthlyReport = new System.Windows.Forms.RadioButton();
            this.label1 = new Sci.Win.UI.Label();
            this.radioGroup2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).BeginInit();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(371, 24);
            this.print.TabIndex = 0;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(371, 60);
            this.toexcel.TabIndex = 1;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(371, 96);
            this.close.TabIndex = 2;
            // 
            // radioGroup2
            // 
            this.radioGroup2.Controls.Add(this.radioProductionStatus);
            this.radioGroup2.Controls.Add(this.txtFactory);
            this.radioGroup2.Controls.Add(this.txtM);
            this.radioGroup2.Controls.Add(this.labelFactory);
            this.radioGroup2.Controls.Add(this.labelM);
            this.radioGroup2.Controls.Add(this.checkFty);
            this.radioGroup2.Controls.Add(this.checkForecast);
            this.radioGroup2.Controls.Add(this.checkOrder);
            this.radioGroup2.Controls.Add(this.comboReport);
            this.radioGroup2.Controls.Add(this.comboDate);
            this.radioGroup2.Controls.Add(this.txtBrand);
            this.radioGroup2.Controls.Add(this.numMonth);
            this.radioGroup2.Controls.Add(this.numYear);
            this.radioGroup2.Controls.Add(this.labelMonth);
            this.radioGroup2.Controls.Add(this.labelSource);
            this.radioGroup2.Controls.Add(this.labelReport);
            this.radioGroup2.Controls.Add(this.labelDate);
            this.radioGroup2.Controls.Add(this.labelBrand);
            this.radioGroup2.Controls.Add(this.labelYear);
            this.radioGroup2.Controls.Add(this.radioSemimonthlyReport);
            this.radioGroup2.Controls.Add(this.radioMonthlyReport);
            this.radioGroup2.Location = new System.Drawing.Point(12, 14);
            this.radioGroup2.Name = "radioGroup2";
            this.radioGroup2.Size = new System.Drawing.Size(348, 385);
            this.radioGroup2.TabIndex = 0;
            this.radioGroup2.TabStop = false;
            // 
            // radioProductionStatus
            // 
            this.radioProductionStatus.AutoSize = true;
            this.radioProductionStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioProductionStatus.Location = new System.Drawing.Point(17, 76);
            this.radioProductionStatus.Name = "radioProductionStatus";
            this.radioProductionStatus.Size = new System.Drawing.Size(130, 21);
            this.radioProductionStatus.TabIndex = 18;
            this.radioProductionStatus.TabStop = true;
            this.radioProductionStatus.Text = "Prouction Status";
            this.radioProductionStatus.UseVisualStyleBackColor = true;
            this.radioProductionStatus.CheckedChanged += new System.EventHandler(this.RadioProductionStatus_CheckedChanged);
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.Location = new System.Drawing.Point(150, 207);
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(94, 23);
            this.txtFactory.TabIndex = 6;
            this.txtFactory.IssupportJunk = true;
            // 
            // txtM
            // 
            this.txtM.BackColor = System.Drawing.Color.White;
            this.txtM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtM.Location = new System.Drawing.Point(150, 172);
            this.txtM.Name = "txtM";
            this.txtM.Size = new System.Drawing.Size(94, 23);
            this.txtM.TabIndex = 5;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(64, 207);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(83, 23);
            this.labelFactory.TabIndex = 17;
            this.labelFactory.Text = "Factory";
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(64, 172);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(83, 23);
            this.labelM.TabIndex = 16;
            this.labelM.Text = "M";
            // 
            // checkFty
            // 
            this.checkFty.AutoSize = true;
            this.checkFty.Checked = true;
            this.checkFty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkFty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkFty.Location = new System.Drawing.Point(150, 355);
            this.checkFty.Name = "checkFty";
            this.checkFty.Size = new System.Drawing.Size(125, 21);
            this.checkFty.TabIndex = 11;
            this.checkFty.Text = "Fty Local Order";
            this.checkFty.UseVisualStyleBackColor = true;
            // 
            // checkForecast
            // 
            this.checkForecast.AutoSize = true;
            this.checkForecast.Checked = true;
            this.checkForecast.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkForecast.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkForecast.Location = new System.Drawing.Point(150, 328);
            this.checkForecast.Name = "checkForecast";
            this.checkForecast.Size = new System.Drawing.Size(82, 21);
            this.checkForecast.TabIndex = 10;
            this.checkForecast.Text = "Forecast";
            this.checkForecast.UseVisualStyleBackColor = true;
            // 
            // checkOrder
            // 
            this.checkOrder.AutoSize = true;
            this.checkOrder.Checked = true;
            this.checkOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkOrder.Location = new System.Drawing.Point(150, 304);
            this.checkOrder.Name = "checkOrder";
            this.checkOrder.Size = new System.Drawing.Size(64, 21);
            this.checkOrder.TabIndex = 9;
            this.checkOrder.Text = "Order";
            this.checkOrder.UseVisualStyleBackColor = true;
            // 
            // comboReport
            // 
            this.comboReport.BackColor = System.Drawing.Color.White;
            this.comboReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboReport.FormattingEnabled = true;
            this.comboReport.IsSupportUnselect = true;
            this.comboReport.Location = new System.Drawing.Point(150, 270);
            this.comboReport.Name = "comboReport";
            this.comboReport.Size = new System.Drawing.Size(190, 24);
            this.comboReport.TabIndex = 8;
            // 
            // comboDate
            // 
            this.comboDate.BackColor = System.Drawing.Color.White;
            this.comboDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDate.FormattingEnabled = true;
            this.comboDate.IsSupportUnselect = true;
            this.comboDate.Location = new System.Drawing.Point(150, 239);
            this.comboDate.Name = "comboDate";
            this.comboDate.Size = new System.Drawing.Size(190, 24);
            this.comboDate.TabIndex = 7;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(150, 138);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(94, 23);
            this.txtBrand.TabIndex = 4;
            // 
            // numMonth
            // 
            this.numMonth.Location = new System.Drawing.Point(290, 105);
            this.numMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numMonth.Name = "numMonth";
            this.numMonth.Size = new System.Drawing.Size(50, 23);
            this.numMonth.TabIndex = 3;
            this.numMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numYear
            // 
            this.numYear.Location = new System.Drawing.Point(150, 105);
            this.numYear.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.numYear.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numYear.Name = "numYear";
            this.numYear.Size = new System.Drawing.Size(72, 23);
            this.numYear.TabIndex = 2;
            this.numYear.Value = new decimal(new int[] {
            2015,
            0,
            0,
            0});
            // 
            // labelMonth
            // 
            this.labelMonth.Lines = 0;
            this.labelMonth.Location = new System.Drawing.Point(241, 105);
            this.labelMonth.Name = "labelMonth";
            this.labelMonth.Size = new System.Drawing.Size(46, 23);
            this.labelMonth.TabIndex = 7;
            this.labelMonth.Text = "Month";
            // 
            // labelSource
            // 
            this.labelSource.Lines = 0;
            this.labelSource.Location = new System.Drawing.Point(64, 302);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(83, 23);
            this.labelSource.TabIndex = 6;
            this.labelSource.Text = "Source";
            // 
            // labelReport
            // 
            this.labelReport.Lines = 0;
            this.labelReport.Location = new System.Drawing.Point(64, 270);
            this.labelReport.Name = "labelReport";
            this.labelReport.Size = new System.Drawing.Size(83, 23);
            this.labelReport.TabIndex = 5;
            this.labelReport.Text = "Report";
            // 
            // labelDate
            // 
            this.labelDate.Lines = 0;
            this.labelDate.Location = new System.Drawing.Point(64, 239);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(83, 23);
            this.labelDate.TabIndex = 4;
            this.labelDate.Text = "Date";
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(64, 138);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(83, 23);
            this.labelBrand.TabIndex = 3;
            this.labelBrand.Text = "Brand";
            // 
            // labelYear
            // 
            this.labelYear.Lines = 0;
            this.labelYear.Location = new System.Drawing.Point(64, 105);
            this.labelYear.Name = "labelYear";
            this.labelYear.Size = new System.Drawing.Size(83, 23);
            this.labelYear.TabIndex = 2;
            this.labelYear.Text = "Year";
            // 
            // radioSemimonthlyReport
            // 
            this.radioSemimonthlyReport.AutoSize = true;
            this.radioSemimonthlyReport.ForeColor = System.Drawing.Color.Red;
            this.radioSemimonthlyReport.Location = new System.Drawing.Point(17, 49);
            this.radioSemimonthlyReport.Name = "radioSemimonthlyReport";
            this.radioSemimonthlyReport.Size = new System.Drawing.Size(153, 21);
            this.radioSemimonthlyReport.TabIndex = 1;
            this.radioSemimonthlyReport.TabStop = true;
            this.radioSemimonthlyReport.Text = "Semimonthly Report";
            this.radioSemimonthlyReport.UseVisualStyleBackColor = true;
            this.radioSemimonthlyReport.CheckedChanged += new System.EventHandler(this.RadioSemimonthlyReport_CheckedChanged);
            // 
            // radioMonthlyReport
            // 
            this.radioMonthlyReport.AutoSize = true;
            this.radioMonthlyReport.Checked = true;
            this.radioMonthlyReport.ForeColor = System.Drawing.Color.Red;
            this.radioMonthlyReport.Location = new System.Drawing.Point(17, 22);
            this.radioMonthlyReport.Name = "radioMonthlyReport";
            this.radioMonthlyReport.Size = new System.Drawing.Size(122, 21);
            this.radioMonthlyReport.TabIndex = 0;
            this.radioMonthlyReport.TabStop = true;
            this.radioMonthlyReport.Text = "Monthly Report";
            this.radioMonthlyReport.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(363, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 23);
            this.label1.TabIndex = 95;
            this.label1.Text = "Paper Size A4";
            this.label1.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // R10
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(467, 430);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioGroup2);
            this.DefaultControl = "numYear";
            this.DefaultControlForEdit = "numYear";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "R10";
            this.Text = "R10. Factory Capacity by Month Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioGroup2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.radioGroup2.ResumeLayout(false);
            this.radioGroup2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioGroup radioGroup2;
        private System.Windows.Forms.RadioButton radioMonthlyReport;
        private System.Windows.Forms.NumericUpDown numYear;
        private Win.UI.Label labelMonth;
        private Win.UI.Label labelSource;
        private Win.UI.Label labelReport;
        private Win.UI.Label labelDate;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelYear;
        private System.Windows.Forms.RadioButton radioSemimonthlyReport;
        private Class.txtbrand txtBrand;
        private System.Windows.Forms.NumericUpDown numMonth;
        private Win.UI.CheckBox checkFty;
        private Win.UI.CheckBox checkForecast;
        private Win.UI.CheckBox checkOrder;
        private Win.UI.ComboBox comboReport;
        private Win.UI.ComboBox comboDate;
        private Win.UI.Label label1;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelM;
        private Class.txtfactory txtFactory;
        private Class.txtMdivision txtM;
        private Win.UI.RadioButton radioProductionStatus;
    }
}
