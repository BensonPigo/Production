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
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.LbAdditional = new Sci.Win.UI.Label();
            this.chkByBrand = new Sci.Win.UI.CheckBox();
            this.chkByCPU = new Sci.Win.UI.CheckBox();
            this.chkHideFoundry = new Sci.Win.UI.CheckBox();
            this.TxtZone = new Sci.Win.UI.TextBox();
            this.labZone = new Sci.Win.UI.Label();
            this.radioProductionStatus = new Sci.Win.UI.RadioButton();
            this.txtFactory = new Sci.Production.Class.txtfactory();
            this.txtMDivision = new Sci.Production.Class.txtMdivision();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.chkFty = new Sci.Win.UI.CheckBox();
            this.chkForecast = new Sci.Win.UI.CheckBox();
            this.chkOrder = new Sci.Win.UI.CheckBox();
            this.cbReportType = new Sci.Win.UI.ComboBox();
            this.cbDateType = new Sci.Win.UI.ComboBox();
            this.txtBrand1 = new Sci.Production.Class.txtbrand();
            this.numMonth = new System.Windows.Forms.NumericUpDown();
            this.numYear = new System.Windows.Forms.NumericUpDown();
            this.lbMonth = new Sci.Win.UI.Label();
            this.labelSource = new Sci.Win.UI.Label();
            this.labelReport = new Sci.Win.UI.Label();
            this.labelDate = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelYear = new Sci.Win.UI.Label();
            this.rdHalfMonth = new System.Windows.Forms.RadioButton();
            this.rdMonth = new System.Windows.Forms.RadioButton();
            this.label1 = new Sci.Win.UI.Label();
            this.radioGroup2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).BeginInit();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(438, 27);
            this.print.TabIndex = 0;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(438, 63);
            this.toexcel.TabIndex = 1;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(438, 99);
            this.close.TabIndex = 2;
            // 
            // radioGroup2
            // 
            this.radioGroup2.Controls.Add(this.chkIncludeCancelOrder);
            this.radioGroup2.Controls.Add(this.LbAdditional);
            this.radioGroup2.Controls.Add(this.chkByBrand);
            this.radioGroup2.Controls.Add(this.chkByCPU);
            this.radioGroup2.Controls.Add(this.chkHideFoundry);
            this.radioGroup2.Controls.Add(this.TxtZone);
            this.radioGroup2.Controls.Add(this.labZone);
            this.radioGroup2.Controls.Add(this.radioProductionStatus);
            this.radioGroup2.Controls.Add(this.txtFactory);
            this.radioGroup2.Controls.Add(this.txtMDivision);
            this.radioGroup2.Controls.Add(this.labelFactory);
            this.radioGroup2.Controls.Add(this.labelM);
            this.radioGroup2.Controls.Add(this.chkFty);
            this.radioGroup2.Controls.Add(this.chkForecast);
            this.radioGroup2.Controls.Add(this.chkOrder);
            this.radioGroup2.Controls.Add(this.cbReportType);
            this.radioGroup2.Controls.Add(this.cbDateType);
            this.radioGroup2.Controls.Add(this.txtBrand1);
            this.radioGroup2.Controls.Add(this.numMonth);
            this.radioGroup2.Controls.Add(this.numYear);
            this.radioGroup2.Controls.Add(this.lbMonth);
            this.radioGroup2.Controls.Add(this.labelSource);
            this.radioGroup2.Controls.Add(this.labelReport);
            this.radioGroup2.Controls.Add(this.labelDate);
            this.radioGroup2.Controls.Add(this.labelBrand);
            this.radioGroup2.Controls.Add(this.labelYear);
            this.radioGroup2.Controls.Add(this.rdHalfMonth);
            this.radioGroup2.Controls.Add(this.rdMonth);
            this.radioGroup2.Location = new System.Drawing.Point(12, 14);
            this.radioGroup2.Name = "radioGroup2";
            this.radioGroup2.Size = new System.Drawing.Size(381, 516);
            this.radioGroup2.TabIndex = 0;
            this.radioGroup2.TabStop = false;
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.Checked = true;
            this.chkIncludeCancelOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(150, 487);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(160, 21);
            this.chkIncludeCancelOrder.TabIndex = 238;
            this.chkIncludeCancelOrder.Text = "Include Cancel Order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // LbAdditional
            // 
            this.LbAdditional.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbAdditional.Location = new System.Drawing.Point(64, 404);
            this.LbAdditional.Name = "LbAdditional";
            this.LbAdditional.Size = new System.Drawing.Size(83, 23);
            this.LbAdditional.TabIndex = 237;
            this.LbAdditional.Text = "Additional";
            // 
            // chkByBrand
            // 
            this.chkByBrand.AutoSize = true;
            this.chkByBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkByBrand.Location = new System.Drawing.Point(150, 460);
            this.chkByBrand.Name = "chkByBrand";
            this.chkByBrand.Size = new System.Drawing.Size(85, 21);
            this.chkByBrand.TabIndex = 236;
            this.chkByBrand.Text = "By Brand";
            this.chkByBrand.UseVisualStyleBackColor = true;
            // 
            // chkByCPU
            // 
            this.chkByCPU.AutoSize = true;
            this.chkByCPU.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkByCPU.Location = new System.Drawing.Point(150, 433);
            this.chkByCPU.Name = "chkByCPU";
            this.chkByCPU.Size = new System.Drawing.Size(75, 21);
            this.chkByCPU.TabIndex = 235;
            this.chkByCPU.Text = "By CPU";
            this.chkByCPU.UseVisualStyleBackColor = true;
            // 
            // chkHideFoundry
            // 
            this.chkHideFoundry.AutoSize = true;
            this.chkHideFoundry.Checked = true;
            this.chkHideFoundry.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHideFoundry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkHideFoundry.Location = new System.Drawing.Point(150, 406);
            this.chkHideFoundry.Name = "chkHideFoundry";
            this.chkHideFoundry.Size = new System.Drawing.Size(112, 21);
            this.chkHideFoundry.TabIndex = 234;
            this.chkHideFoundry.Text = "Hide Foundry";
            this.chkHideFoundry.UseVisualStyleBackColor = true;
            // 
            // TxtZone
            // 
            this.TxtZone.BackColor = System.Drawing.Color.White;
            this.TxtZone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.TxtZone.Location = new System.Drawing.Point(150, 201);
            this.TxtZone.Name = "TxtZone";
            this.TxtZone.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.TxtZone.Size = new System.Drawing.Size(94, 23);
            this.TxtZone.TabIndex = 21;
            this.TxtZone.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtZone_PopUp);
            this.TxtZone.Validating += new System.ComponentModel.CancelEventHandler(this.TxtZone_Validating);
            // 
            // labZone
            // 
            this.labZone.Location = new System.Drawing.Point(64, 200);
            this.labZone.Name = "labZone";
            this.labZone.Size = new System.Drawing.Size(83, 23);
            this.labZone.TabIndex = 20;
            this.labZone.Text = "Zone";
            // 
            // radioProductionStatus
            // 
            this.radioProductionStatus.AutoSize = true;
            this.radioProductionStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioProductionStatus.Location = new System.Drawing.Point(17, 76);
            this.radioProductionStatus.Name = "radioProductionStatus";
            this.radioProductionStatus.Size = new System.Drawing.Size(138, 21);
            this.radioProductionStatus.TabIndex = 18;
            this.radioProductionStatus.TabStop = true;
            this.radioProductionStatus.Text = "Production Status";
            this.radioProductionStatus.UseVisualStyleBackColor = true;
            this.radioProductionStatus.CheckedChanged += new System.EventHandler(this.RadioProductionStatus_CheckedChanged);
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.boolFtyGroupList = false;
            this.txtFactory.FilteMDivision = false;
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.IsProduceFty = true;
            this.txtFactory.IssupportJunk = true;
            this.txtFactory.Location = new System.Drawing.Point(150, 232);
            this.txtFactory.MDivision = null;
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(94, 23);
            this.txtFactory.TabIndex = 6;
            // 
            // txtMDivision
            // 
            this.txtMDivision.BackColor = System.Drawing.Color.White;
            this.txtMDivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMDivision.Location = new System.Drawing.Point(150, 169);
            this.txtMDivision.Name = "txtMDivision";
            this.txtMDivision.Size = new System.Drawing.Size(94, 23);
            this.txtMDivision.TabIndex = 5;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(64, 232);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(83, 23);
            this.labelFactory.TabIndex = 17;
            this.labelFactory.Text = "Factory";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(64, 169);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(83, 23);
            this.labelM.TabIndex = 16;
            this.labelM.Text = "M";
            // 
            // chkFty
            // 
            this.chkFty.AutoSize = true;
            this.chkFty.Checked = true;
            this.chkFty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkFty.Location = new System.Drawing.Point(150, 380);
            this.chkFty.Name = "chkFty";
            this.chkFty.Size = new System.Drawing.Size(125, 21);
            this.chkFty.TabIndex = 11;
            this.chkFty.Text = "Fty Local Order";
            this.chkFty.UseVisualStyleBackColor = true;
            // 
            // chkForecast
            // 
            this.chkForecast.AutoSize = true;
            this.chkForecast.Checked = true;
            this.chkForecast.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkForecast.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkForecast.Location = new System.Drawing.Point(150, 353);
            this.chkForecast.Name = "chkForecast";
            this.chkForecast.Size = new System.Drawing.Size(82, 21);
            this.chkForecast.TabIndex = 10;
            this.chkForecast.Text = "Forecast";
            this.chkForecast.UseVisualStyleBackColor = true;
            // 
            // chkOrder
            // 
            this.chkOrder.AutoSize = true;
            this.chkOrder.Checked = true;
            this.chkOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkOrder.Location = new System.Drawing.Point(150, 329);
            this.chkOrder.Name = "chkOrder";
            this.chkOrder.Size = new System.Drawing.Size(64, 21);
            this.chkOrder.TabIndex = 9;
            this.chkOrder.Text = "Order";
            this.chkOrder.UseVisualStyleBackColor = true;
            // 
            // cbReportType
            // 
            this.cbReportType.BackColor = System.Drawing.Color.White;
            this.cbReportType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbReportType.FormattingEnabled = true;
            this.cbReportType.IsSupportUnselect = true;
            this.cbReportType.Location = new System.Drawing.Point(150, 295);
            this.cbReportType.Name = "cbReportType";
            this.cbReportType.OldText = "";
            this.cbReportType.Size = new System.Drawing.Size(211, 24);
            this.cbReportType.TabIndex = 8;
            // 
            // cbDateType
            // 
            this.cbDateType.BackColor = System.Drawing.Color.White;
            this.cbDateType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbDateType.FormattingEnabled = true;
            this.cbDateType.IsSupportUnselect = true;
            this.cbDateType.Location = new System.Drawing.Point(150, 264);
            this.cbDateType.Name = "cbDateType";
            this.cbDateType.OldText = "";
            this.cbDateType.Size = new System.Drawing.Size(190, 24);
            this.cbDateType.TabIndex = 7;
            // 
            // txtBrand1
            // 
            this.txtBrand1.BackColor = System.Drawing.Color.White;
            this.txtBrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand1.Location = new System.Drawing.Point(150, 137);
            this.txtBrand1.Name = "txtBrand1";
            this.txtBrand1.Size = new System.Drawing.Size(94, 23);
            this.txtBrand1.TabIndex = 4;
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
            // lbMonth
            // 
            this.lbMonth.Location = new System.Drawing.Point(241, 105);
            this.lbMonth.Name = "lbMonth";
            this.lbMonth.Size = new System.Drawing.Size(46, 23);
            this.lbMonth.TabIndex = 7;
            this.lbMonth.Text = "Month";
            // 
            // labelSource
            // 
            this.labelSource.Location = new System.Drawing.Point(64, 327);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(83, 23);
            this.labelSource.TabIndex = 6;
            this.labelSource.Text = "Source";
            // 
            // labelReport
            // 
            this.labelReport.Location = new System.Drawing.Point(64, 295);
            this.labelReport.Name = "labelReport";
            this.labelReport.Size = new System.Drawing.Size(83, 23);
            this.labelReport.TabIndex = 5;
            this.labelReport.Text = "Report";
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(64, 264);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(83, 23);
            this.labelDate.TabIndex = 4;
            this.labelDate.Text = "Date";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(64, 137);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(83, 23);
            this.labelBrand.TabIndex = 3;
            this.labelBrand.Text = "Brand";
            // 
            // labelYear
            // 
            this.labelYear.Location = new System.Drawing.Point(64, 105);
            this.labelYear.Name = "labelYear";
            this.labelYear.Size = new System.Drawing.Size(83, 23);
            this.labelYear.TabIndex = 2;
            this.labelYear.Text = "Year";
            // 
            // rdHalfMonth
            // 
            this.rdHalfMonth.AutoSize = true;
            this.rdHalfMonth.ForeColor = System.Drawing.Color.Red;
            this.rdHalfMonth.Location = new System.Drawing.Point(17, 49);
            this.rdHalfMonth.Name = "rdHalfMonth";
            this.rdHalfMonth.Size = new System.Drawing.Size(158, 21);
            this.rdHalfMonth.TabIndex = 1;
            this.rdHalfMonth.TabStop = true;
            this.rdHalfMonth.Text = "Semi-monthly Report";
            this.rdHalfMonth.UseVisualStyleBackColor = true;
            this.rdHalfMonth.CheckedChanged += new System.EventHandler(this.RadioSemimonthlyReport_CheckedChanged);
            // 
            // rdMonth
            // 
            this.rdMonth.AutoSize = true;
            this.rdMonth.Checked = true;
            this.rdMonth.ForeColor = System.Drawing.Color.Red;
            this.rdMonth.Location = new System.Drawing.Point(17, 22);
            this.rdMonth.Name = "rdMonth";
            this.rdMonth.Size = new System.Drawing.Size(122, 21);
            this.rdMonth.TabIndex = 0;
            this.rdMonth.TabStop = true;
            this.rdMonth.Text = "Monthly Report";
            this.rdMonth.UseVisualStyleBackColor = true;
            this.rdMonth.CheckedChanged += new System.EventHandler(this.RdMonth_CheckedChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(416, 132);
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
            this.ClientSize = new System.Drawing.Size(531, 555);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioGroup2);
            this.DefaultControl = "numYear";
            this.DefaultControlForEdit = "numYear";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "R10";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R10. Factory Capacity by Month Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
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
        private System.Windows.Forms.RadioButton rdMonth;
        private System.Windows.Forms.NumericUpDown numYear;
        private Win.UI.Label lbMonth;
        private Win.UI.Label labelSource;
        private Win.UI.Label labelReport;
        private Win.UI.Label labelDate;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelYear;
        private System.Windows.Forms.RadioButton rdHalfMonth;
        private Class.txtbrand txtBrand1;
        private System.Windows.Forms.NumericUpDown numMonth;
        private Win.UI.CheckBox chkFty;
        private Win.UI.CheckBox chkForecast;
        private Win.UI.CheckBox chkOrder;
        private Win.UI.ComboBox cbReportType;
        private Win.UI.ComboBox cbDateType;
        private Win.UI.Label label1;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelM;
        private Class.txtfactory txtFactory;
        private Class.txtMdivision txtMDivision;
        private Win.UI.RadioButton radioProductionStatus;
        private Win.UI.Label labZone;
        private Win.UI.TextBox TxtZone;
        private Win.UI.Label LbAdditional;
        private Win.UI.CheckBox chkByBrand;
        private Win.UI.CheckBox chkByCPU;
        private Win.UI.CheckBox chkHideFoundry;
        private Win.UI.CheckBox chkIncludeCancelOrder;
    }
}
