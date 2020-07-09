namespace Sci.Production.Quality
{
    partial class R23
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.numDateGap = new Sci.Win.UI.NumericBox();
            this.labDateGap = new Sci.Win.UI.Label();
            this.labFactory = new Sci.Win.UI.Label();
            this.labM = new Sci.Win.UI.Label();
            this.labReadyDate = new Sci.Win.UI.Label();
            this.dateRangeReadyDate = new Sci.Win.UI.DateRange();
            this.labBrand = new Sci.Win.UI.Label();
            this.chkHoliday = new Sci.Win.UI.CheckBox();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtTime = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.numInsGap = new Sci.Win.UI.NumericBox();
            this.label1 = new Sci.Win.UI.Label();
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(444, 87);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(444, 15);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(444, 51);
            // 
            // numDateGap
            // 
            this.numDateGap.BackColor = System.Drawing.Color.White;
            this.numDateGap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numDateGap.Location = new System.Drawing.Point(317, 91);
            this.numDateGap.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numDateGap.Name = "numDateGap";
            this.numDateGap.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDateGap.Size = new System.Drawing.Size(66, 23);
            this.numDateGap.TabIndex = 107;
            this.numDateGap.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labDateGap
            // 
            this.labDateGap.Location = new System.Drawing.Point(20, 91);
            this.labDateGap.Name = "labDateGap";
            this.labDateGap.Size = new System.Drawing.Size(294, 23);
            this.labDateGap.TabIndex = 106;
            this.labDateGap.Text = "Ready Date and SewingOutput Date gap(Days)";
            // 
            // labFactory
            // 
            this.labFactory.Location = new System.Drawing.Point(136, 55);
            this.labFactory.Name = "labFactory";
            this.labFactory.Size = new System.Drawing.Size(55, 23);
            this.labFactory.TabIndex = 105;
            this.labFactory.Text = "Factory";
            // 
            // labM
            // 
            this.labM.Location = new System.Drawing.Point(20, 55);
            this.labM.Name = "labM";
            this.labM.Size = new System.Drawing.Size(28, 23);
            this.labM.TabIndex = 104;
            this.labM.Text = "M";
            // 
            // labReadyDate
            // 
            this.labReadyDate.Location = new System.Drawing.Point(20, 19);
            this.labReadyDate.Name = "labReadyDate";
            this.labReadyDate.Size = new System.Drawing.Size(80, 23);
            this.labReadyDate.TabIndex = 103;
            this.labReadyDate.Text = "Ready Date";
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
            this.dateRangeReadyDate.Location = new System.Drawing.Point(103, 19);
            this.dateRangeReadyDate.Name = "dateRangeReadyDate";
            this.dateRangeReadyDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeReadyDate.TabIndex = 102;
            // 
            // labBrand
            // 
            this.labBrand.Location = new System.Drawing.Point(263, 55);
            this.labBrand.Name = "labBrand";
            this.labBrand.Size = new System.Drawing.Size(51, 23);
            this.labBrand.TabIndex = 110;
            this.labBrand.Text = "Brand";
            // 
            // chkHoliday
            // 
            this.chkHoliday.AutoSize = true;
            this.chkHoliday.Checked = true;
            this.chkHoliday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHoliday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkHoliday.Location = new System.Drawing.Point(20, 164);
            this.chkHoliday.Name = "chkHoliday";
            this.chkHoliday.Size = new System.Drawing.Size(127, 21);
            this.chkHoliday.TabIndex = 112;
            this.chkHoliday.Text = "Exclude Holiday";
            this.chkHoliday.UseVisualStyleBackColor = true;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(317, 55);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 111;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(51, 55);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(82, 23);
            this.txtMdivision.TabIndex = 109;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(194, 55);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 108;
            // 
            // txtTime
            // 
            this.txtTime.BackColor = System.Drawing.Color.White;
            this.txtTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTime.Location = new System.Drawing.Point(317, 162);
            this.txtTime.Mask = "90:00";
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(48, 23);
            this.txtTime.TabIndex = 114;
            this.txtTime.Text = "1200";
            this.txtTime.ValidatingType = typeof(System.DateTime);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(269, 162);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 23);
            this.label6.TabIndex = 113;
            this.label6.Text = "Time ";
            this.label6.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numInsGap
            // 
            this.numInsGap.BackColor = System.Drawing.Color.White;
            this.numInsGap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numInsGap.Location = new System.Drawing.Point(317, 128);
            this.numInsGap.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numInsGap.Name = "numInsGap";
            this.numInsGap.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numInsGap.Size = new System.Drawing.Size(66, 23);
            this.numInsGap.TabIndex = 116;
            this.numInsGap.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(294, 23);
            this.label1.TabIndex = 115;
            this.label1.Text = "Ready Date and Inspection Date gap(Days)";
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(20, 191);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 169;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // R23
            // 
            this.ClientSize = new System.Drawing.Size(536, 264);
            this.Controls.Add(this.chkIncludeCancelOrder);
            this.Controls.Add(this.numInsGap);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkHoliday);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.labBrand);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.numDateGap);
            this.Controls.Add(this.labDateGap);
            this.Controls.Add(this.labFactory);
            this.Controls.Add(this.labM);
            this.Controls.Add(this.labReadyDate);
            this.Controls.Add(this.dateRangeReadyDate);
            this.Name = "R23";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R23 Inspection Ready Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.dateRangeReadyDate, 0);
            this.Controls.SetChildIndex(this.labReadyDate, 0);
            this.Controls.SetChildIndex(this.labM, 0);
            this.Controls.SetChildIndex(this.labFactory, 0);
            this.Controls.SetChildIndex(this.labDateGap, 0);
            this.Controls.SetChildIndex(this.numDateGap, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labBrand, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.chkHoliday, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtTime, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.numInsGap, 0);
            this.Controls.SetChildIndex(this.chkIncludeCancelOrder, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Class.TxtMdivision txtMdivision;
        private Class.Txtfactory txtfactory;
        private Win.UI.NumericBox numDateGap;
        private Win.UI.Label labDateGap;
        private Win.UI.Label labFactory;
        private Win.UI.Label labM;
        private Win.UI.Label labReadyDate;
        private Win.UI.DateRange dateRangeReadyDate;
        private Win.UI.Label labBrand;
        private Class.Txtbrand txtbrand;
        private Win.UI.CheckBox chkHoliday;
        private Win.UI.TextBox txtTime;
        private Win.UI.Label label6;
        private Win.UI.NumericBox numInsGap;
        private Win.UI.Label label1;
        private Win.UI.CheckBox chkIncludeCancelOrder;
    }
}
