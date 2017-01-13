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
            this.radioGroup1 = new Sci.Win.UI.RadioGroup();
            this.numYear = new Sci.Win.UI.NumericUpDown();
            this.lbSource = new Sci.Win.UI.Label();
            this.lbReportType = new Sci.Win.UI.Label();
            this.lbDateType = new Sci.Win.UI.Label();
            this.lbBrand = new Sci.Win.UI.Label();
            this.rdHalfMonth = new Sci.Win.UI.RadioButton();
            this.rdMonth = new Sci.Win.UI.RadioButton();
            this.lbYear1 = new Sci.Win.UI.Label();
            this.lbMonth = new Sci.Win.UI.Label();
            this.numMonth = new Sci.Win.UI.NumericUpDown();
            this.txtBrand1 = new Sci.Trade.Class.TxtBrand();
            this.cbDateType = new Sci.Win.UI.ComboBox();
            this.cbReportType = new Sci.Win.UI.ComboBox();
            this.chkOrder = new Sci.Win.UI.CheckBox();
            this.chkForecast = new Sci.Win.UI.CheckBox();
            this.chkFty = new Sci.Win.UI.CheckBox();
            this.label1 = new Sci.Win.UI.Label();
            this.radioGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMonth)).BeginInit();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(351, 20);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(351, 56);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(351, 92);
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.chkFty);
            this.radioGroup1.Controls.Add(this.chkForecast);
            this.radioGroup1.Controls.Add(this.chkOrder);
            this.radioGroup1.Controls.Add(this.cbReportType);
            this.radioGroup1.Controls.Add(this.cbDateType);
            this.radioGroup1.Controls.Add(this.txtBrand1);
            this.radioGroup1.Controls.Add(this.numMonth);
            this.radioGroup1.Controls.Add(this.lbMonth);
            this.radioGroup1.Controls.Add(this.numYear);
            this.radioGroup1.Controls.Add(this.lbSource);
            this.radioGroup1.Controls.Add(this.lbReportType);
            this.radioGroup1.Controls.Add(this.lbDateType);
            this.radioGroup1.Controls.Add(this.lbBrand);
            this.radioGroup1.Controls.Add(this.lbYear1);
            this.radioGroup1.Controls.Add(this.rdHalfMonth);
            this.radioGroup1.Controls.Add(this.rdMonth);
            this.radioGroup1.Location = new System.Drawing.Point(12, 12);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(333, 274);
            this.radioGroup1.TabIndex = 94;
            this.radioGroup1.TabStop = false;
            // 
            // numYear
            // 
            this.numYear.BackColor = System.Drawing.Color.White;
            this.numYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numYear.Location = new System.Drawing.Point(151, 69);
            this.numYear.Name = "numYear";
            this.numYear.Size = new System.Drawing.Size(74, 23);
            this.numYear.TabIndex = 7;
            this.numYear.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // lbSource
            // 
            this.lbSource.Lines = 0;
            this.lbSource.Location = new System.Drawing.Point(72, 194);
            this.lbSource.Name = "lbSource";
            this.lbSource.Size = new System.Drawing.Size(76, 23);
            this.lbSource.TabIndex = 6;
            this.lbSource.Text = "Source";
            // 
            // lbReportType
            // 
            this.lbReportType.Lines = 0;
            this.lbReportType.Location = new System.Drawing.Point(72, 160);
            this.lbReportType.Name = "lbReportType";
            this.lbReportType.Size = new System.Drawing.Size(76, 23);
            this.lbReportType.TabIndex = 5;
            this.lbReportType.Text = "Report";
            // 
            // lbDateType
            // 
            this.lbDateType.Lines = 0;
            this.lbDateType.Location = new System.Drawing.Point(72, 128);
            this.lbDateType.Name = "lbDateType";
            this.lbDateType.Size = new System.Drawing.Size(76, 23);
            this.lbDateType.TabIndex = 4;
            this.lbDateType.Text = "Date";
            // 
            // lbBrand
            // 
            this.lbBrand.Lines = 0;
            this.lbBrand.Location = new System.Drawing.Point(72, 98);
            this.lbBrand.Name = "lbBrand";
            this.lbBrand.Size = new System.Drawing.Size(76, 23);
            this.lbBrand.TabIndex = 3;
            this.lbBrand.Text = "Brand";
            // 
            // rdHalfMonth
            // 
            this.rdHalfMonth.AutoSize = true;
            this.rdHalfMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdHalfMonth.Location = new System.Drawing.Point(6, 44);
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
            this.rdMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdMonth.Location = new System.Drawing.Point(6, 18);
            this.rdMonth.Name = "rdMonth";
            this.rdMonth.Size = new System.Drawing.Size(122, 21);
            this.rdMonth.TabIndex = 0;
            this.rdMonth.TabStop = true;
            this.rdMonth.Text = "Monthly Report";
            this.rdMonth.UseVisualStyleBackColor = true;
            // 
            // lbYear1
            // 
            this.lbYear1.Lines = 0;
            this.lbYear1.Location = new System.Drawing.Point(72, 69);
            this.lbYear1.Name = "lbYear1";
            this.lbYear1.Size = new System.Drawing.Size(76, 23);
            this.lbYear1.TabIndex = 2;
            this.lbYear1.Text = "Year";
            // 
            // lbMonth
            // 
            this.lbMonth.Lines = 0;
            this.lbMonth.Location = new System.Drawing.Point(228, 70);
            this.lbMonth.Name = "lbMonth";
            this.lbMonth.Size = new System.Drawing.Size(52, 23);
            this.lbMonth.TabIndex = 8;
            this.lbMonth.Text = "Month";
            // 
            // numMonth
            // 
            this.numMonth.BackColor = System.Drawing.Color.White;
            this.numMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numMonth.Location = new System.Drawing.Point(283, 70);
            this.numMonth.Name = "numMonth";
            this.numMonth.Size = new System.Drawing.Size(45, 23);
            this.numMonth.TabIndex = 9;
            this.numMonth.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtBrand1
            // 
            this.txtBrand1.BackColor = System.Drawing.Color.White;
            this.txtBrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand1.Location = new System.Drawing.Point(151, 98);
            this.txtBrand1.Name = "txtBrand1";
            this.txtBrand1.Size = new System.Drawing.Size(82, 23);
            this.txtBrand1.TabIndex = 10;
            // 
            // cbDateType
            // 
            this.cbDateType.BackColor = System.Drawing.Color.White;
            this.cbDateType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbDateType.FormattingEnabled = true;
            this.cbDateType.IsSupportUnselect = true;
            this.cbDateType.Location = new System.Drawing.Point(151, 128);
            this.cbDateType.Name = "cbDateType";
            this.cbDateType.Size = new System.Drawing.Size(121, 24);
            this.cbDateType.TabIndex = 11;
            // 
            // cbReportType
            // 
            this.cbReportType.BackColor = System.Drawing.Color.White;
            this.cbReportType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbReportType.FormattingEnabled = true;
            this.cbReportType.IsSupportUnselect = true;
            this.cbReportType.Location = new System.Drawing.Point(151, 160);
            this.cbReportType.Name = "cbReportType";
            this.cbReportType.Size = new System.Drawing.Size(121, 24);
            this.cbReportType.TabIndex = 12;
            // 
            // chkOrder
            // 
            this.chkOrder.AutoSize = true;
            this.chkOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkOrder.Location = new System.Drawing.Point(151, 196);
            this.chkOrder.Name = "chkOrder";
            this.chkOrder.Size = new System.Drawing.Size(64, 21);
            this.chkOrder.TabIndex = 13;
            this.chkOrder.Text = "Order";
            this.chkOrder.UseVisualStyleBackColor = true;
            // 
            // chkForecast
            // 
            this.chkForecast.AutoSize = true;
            this.chkForecast.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkForecast.Location = new System.Drawing.Point(151, 223);
            this.chkForecast.Name = "chkForecast";
            this.chkForecast.Size = new System.Drawing.Size(82, 21);
            this.chkForecast.TabIndex = 14;
            this.chkForecast.Text = "Forecast";
            this.chkForecast.UseVisualStyleBackColor = true;
            // 
            // chkFty
            // 
            this.chkFty.AutoSize = true;
            this.chkFty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkFty.Location = new System.Drawing.Point(151, 250);
            this.chkFty.Name = "chkFty";
            this.chkFty.Size = new System.Drawing.Size(125, 21);
            this.chkFty.TabIndex = 15;
            this.chkFty.Text = "Fty Local Order";
            this.chkFty.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Menu;
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(348, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 23);
            this.label1.TabIndex = 95;
            this.label1.Text = "Paper Size A4";
            this.label1.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // R10
            // 
            this.ClientSize = new System.Drawing.Size(448, 314);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioGroup1);
            this.Name = "R10";
            this.Text = "R10.Factory Capacity by Month Report";
            this.Controls.SetChildIndex(this.radioGroup1, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMonth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioGroup radioGroup1;
        private Win.UI.RadioButton rdMonth;
        private Win.UI.RadioButton rdHalfMonth;
        private Win.UI.NumericUpDown numYear;
        private Win.UI.Label lbSource;
        private Win.UI.Label lbReportType;
        private Win.UI.Label lbDateType;
        private Win.UI.Label lbBrand;
        private Win.UI.NumericUpDown numMonth;
        private Win.UI.Label lbMonth;
        private Win.UI.Label lbYear1;
        private Trade.Class.TxtBrand txtBrand1;
        private Win.UI.CheckBox chkOrder;
        private Win.UI.ComboBox cbReportType;
        private Win.UI.ComboBox cbDateType;
        private Win.UI.CheckBox chkFty;
        private Win.UI.CheckBox chkForecast;
        private Win.UI.Label label1;

    }
}
