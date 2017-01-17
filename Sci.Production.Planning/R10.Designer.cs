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
            this.chkFty = new Sci.Win.UI.CheckBox();
            this.chkForecast = new Sci.Win.UI.CheckBox();
            this.chkOrder = new Sci.Win.UI.CheckBox();
            this.cbReportType = new Sci.Win.UI.ComboBox();
            this.cbDateType = new Sci.Win.UI.ComboBox();
            this.numMonth = new System.Windows.Forms.NumericUpDown();
            this.numYear1 = new System.Windows.Forms.NumericUpDown();
            this.lbMonth = new Sci.Win.UI.Label();
            this.lbSource = new Sci.Win.UI.Label();
            this.lbReportType = new Sci.Win.UI.Label();
            this.lbDateType = new Sci.Win.UI.Label();
            this.lbBrand = new Sci.Win.UI.Label();
            this.lbYear = new Sci.Win.UI.Label();
            this.rdHalfMonth = new System.Windows.Forms.RadioButton();
            this.rdMonth = new System.Windows.Forms.RadioButton();
            this.label1 = new Sci.Win.UI.Label();
            this.txtBrand1 = new Sci.Production.Class.txtbrand();
            this.radioGroup2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYear1)).BeginInit();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(374, 24);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(374, 60);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(374, 96);
            // 
            // radioGroup2
            // 
            this.radioGroup2.Controls.Add(this.chkFty);
            this.radioGroup2.Controls.Add(this.chkForecast);
            this.radioGroup2.Controls.Add(this.chkOrder);
            this.radioGroup2.Controls.Add(this.cbReportType);
            this.radioGroup2.Controls.Add(this.cbDateType);
            this.radioGroup2.Controls.Add(this.txtBrand1);
            this.radioGroup2.Controls.Add(this.numMonth);
            this.radioGroup2.Controls.Add(this.numYear1);
            this.radioGroup2.Controls.Add(this.lbMonth);
            this.radioGroup2.Controls.Add(this.lbSource);
            this.radioGroup2.Controls.Add(this.lbReportType);
            this.radioGroup2.Controls.Add(this.lbDateType);
            this.radioGroup2.Controls.Add(this.lbBrand);
            this.radioGroup2.Controls.Add(this.lbYear);
            this.radioGroup2.Controls.Add(this.rdHalfMonth);
            this.radioGroup2.Controls.Add(this.rdMonth);
            this.radioGroup2.Location = new System.Drawing.Point(12, 14);
            this.radioGroup2.Name = "radioGroup2";
            this.radioGroup2.Size = new System.Drawing.Size(348, 319);
            this.radioGroup2.TabIndex = 94;
            this.radioGroup2.TabStop = false;
            // 
            // chkFty
            // 
            this.chkFty.AutoSize = true;
            this.chkFty.Checked = true;
            this.chkFty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkFty.Location = new System.Drawing.Point(150, 272);
            this.chkFty.Name = "chkFty";
            this.chkFty.Size = new System.Drawing.Size(125, 21);
            this.chkFty.TabIndex = 15;
            this.chkFty.Text = "Fty Local Order";
            this.chkFty.UseVisualStyleBackColor = true;
            // 
            // chkForecast
            // 
            this.chkForecast.AutoSize = true;
            this.chkForecast.Checked = true;
            this.chkForecast.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkForecast.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkForecast.Location = new System.Drawing.Point(150, 245);
            this.chkForecast.Name = "chkForecast";
            this.chkForecast.Size = new System.Drawing.Size(82, 21);
            this.chkForecast.TabIndex = 14;
            this.chkForecast.Text = "Forecast";
            this.chkForecast.UseVisualStyleBackColor = true;
            // 
            // chkOrder
            // 
            this.chkOrder.AutoSize = true;
            this.chkOrder.Checked = true;
            this.chkOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkOrder.Location = new System.Drawing.Point(150, 221);
            this.chkOrder.Name = "chkOrder";
            this.chkOrder.Size = new System.Drawing.Size(64, 21);
            this.chkOrder.TabIndex = 13;
            this.chkOrder.Text = "Order";
            this.chkOrder.UseVisualStyleBackColor = true;
            // 
            // cbReportType
            // 
            this.cbReportType.BackColor = System.Drawing.Color.White;
            this.cbReportType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbReportType.FormattingEnabled = true;
            this.cbReportType.IsSupportUnselect = true;
            this.cbReportType.Location = new System.Drawing.Point(150, 187);
            this.cbReportType.Name = "cbReportType";
            this.cbReportType.Size = new System.Drawing.Size(190, 24);
            this.cbReportType.TabIndex = 12;
            // 
            // cbDateType
            // 
            this.cbDateType.BackColor = System.Drawing.Color.White;
            this.cbDateType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbDateType.FormattingEnabled = true;
            this.cbDateType.IsSupportUnselect = true;
            this.cbDateType.Location = new System.Drawing.Point(150, 156);
            this.cbDateType.Name = "cbDateType";
            this.cbDateType.Size = new System.Drawing.Size(190, 24);
            this.cbDateType.TabIndex = 11;
            // 
            // numMonth
            // 
            this.numMonth.Location = new System.Drawing.Point(290, 85);
            this.numMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numMonth.Name = "numMonth";
            this.numMonth.Size = new System.Drawing.Size(50, 23);
            this.numMonth.TabIndex = 9;
            this.numMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numYear1
            // 
            this.numYear1.Location = new System.Drawing.Point(150, 85);
            this.numYear1.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.numYear1.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numYear1.Name = "numYear1";
            this.numYear1.Size = new System.Drawing.Size(72, 23);
            this.numYear1.TabIndex = 8;
            this.numYear1.Value = new decimal(new int[] {
            2015,
            0,
            0,
            0});
            // 
            // lbMonth
            // 
            this.lbMonth.Lines = 0;
            this.lbMonth.Location = new System.Drawing.Point(241, 85);
            this.lbMonth.Name = "lbMonth";
            this.lbMonth.Size = new System.Drawing.Size(46, 23);
            this.lbMonth.TabIndex = 7;
            this.lbMonth.Text = "Month";
            // 
            // lbSource
            // 
            this.lbSource.Lines = 0;
            this.lbSource.Location = new System.Drawing.Point(64, 219);
            this.lbSource.Name = "lbSource";
            this.lbSource.Size = new System.Drawing.Size(83, 23);
            this.lbSource.TabIndex = 6;
            this.lbSource.Text = "Source";
            // 
            // lbReportType
            // 
            this.lbReportType.Lines = 0;
            this.lbReportType.Location = new System.Drawing.Point(64, 187);
            this.lbReportType.Name = "lbReportType";
            this.lbReportType.Size = new System.Drawing.Size(83, 23);
            this.lbReportType.TabIndex = 5;
            this.lbReportType.Text = "Report";
            // 
            // lbDateType
            // 
            this.lbDateType.Lines = 0;
            this.lbDateType.Location = new System.Drawing.Point(64, 156);
            this.lbDateType.Name = "lbDateType";
            this.lbDateType.Size = new System.Drawing.Size(83, 23);
            this.lbDateType.TabIndex = 4;
            this.lbDateType.Text = "Date";
            // 
            // lbBrand
            // 
            this.lbBrand.Lines = 0;
            this.lbBrand.Location = new System.Drawing.Point(64, 118);
            this.lbBrand.Name = "lbBrand";
            this.lbBrand.Size = new System.Drawing.Size(83, 23);
            this.lbBrand.TabIndex = 3;
            this.lbBrand.Text = "Brand";
            // 
            // lbYear
            // 
            this.lbYear.Lines = 0;
            this.lbYear.Location = new System.Drawing.Point(64, 85);
            this.lbYear.Name = "lbYear";
            this.lbYear.Size = new System.Drawing.Size(83, 23);
            this.lbYear.TabIndex = 2;
            this.lbYear.Text = "Year";
            // 
            // rdHalfMonth
            // 
            this.rdHalfMonth.AutoSize = true;
            this.rdHalfMonth.ForeColor = System.Drawing.Color.Red;
            this.rdHalfMonth.Location = new System.Drawing.Point(17, 49);
            this.rdHalfMonth.Name = "rdHalfMonth";
            this.rdHalfMonth.Size = new System.Drawing.Size(153, 21);
            this.rdHalfMonth.TabIndex = 1;
            this.rdHalfMonth.TabStop = true;
            this.rdHalfMonth.Text = "Semimonthly Report";
            this.rdHalfMonth.UseVisualStyleBackColor = true;
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
            // txtBrand1
            // 
            this.txtBrand1.BackColor = System.Drawing.Color.White;
            this.txtBrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand1.Location = new System.Drawing.Point(150, 118);
            this.txtBrand1.Name = "txtBrand1";
            this.txtBrand1.Size = new System.Drawing.Size(94, 23);
            this.txtBrand1.TabIndex = 10;
            // 
            // R10
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(470, 357);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioGroup2);
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
            ((System.ComponentModel.ISupportInitialize)(this.numYear1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioGroup radioGroup2;
        private System.Windows.Forms.RadioButton rdMonth;
        private System.Windows.Forms.NumericUpDown numYear1;
        private Win.UI.Label lbMonth;
        private Win.UI.Label lbSource;
        private Win.UI.Label lbReportType;
        private Win.UI.Label lbDateType;
        private Win.UI.Label lbBrand;
        private Win.UI.Label lbYear;
        private System.Windows.Forms.RadioButton rdHalfMonth;
        private Class.txtbrand txtBrand1;
        private System.Windows.Forms.NumericUpDown numMonth;
        private Win.UI.CheckBox chkFty;
        private Win.UI.CheckBox chkForecast;
        private Win.UI.CheckBox chkOrder;
        private Win.UI.ComboBox cbReportType;
        private Win.UI.ComboBox cbDateType;
        private Win.UI.Label label1;
    }
}
