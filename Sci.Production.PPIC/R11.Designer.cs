namespace Sci.Production.PPIC
{
    partial class R11
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
            this.dateRangeReadyDate = new Sci.Win.UI.DateRange();
            this.labReadyDate = new Sci.Win.UI.Label();
            this.labM = new Sci.Win.UI.Label();
            this.labFactory = new Sci.Win.UI.Label();
            this.labDateGap = new Sci.Win.UI.Label();
            this.numDateGap = new Sci.Win.UI.NumericBox();
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.txtMdivision = new Sci.Production.Class.txtMdivision();
            this.labBrand = new Sci.Win.UI.Label();
            this.txtbrand = new Sci.Production.Class.txtbrand();
            this.chkHoliday = new Sci.Win.UI.CheckBox();
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(450, 88);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(450, 16);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(450, 52);
            // 
            // dateRangeReadyDate
            // 
            // 
            // 
            // 
            this.dateRangeReadyDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeReadyDate.DateBox1.Name = "";
            this.dateRangeReadyDate.DateBox1.Size = new System.Drawing.Size(136, 23);
            this.dateRangeReadyDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeReadyDate.DateBox2.Location = new System.Drawing.Point(158, 0);
            this.dateRangeReadyDate.DateBox2.Name = "";
            this.dateRangeReadyDate.DateBox2.Size = new System.Drawing.Size(136, 23);
            this.dateRangeReadyDate.DateBox2.TabIndex = 1;
            this.dateRangeReadyDate.Location = new System.Drawing.Point(107, 23);
            this.dateRangeReadyDate.Name = "dateRangeReadyDate";
            this.dateRangeReadyDate.Size = new System.Drawing.Size(295, 23);
            this.dateRangeReadyDate.TabIndex = 94;
            // 
            // labReadyDate
            // 
            this.labReadyDate.Location = new System.Drawing.Point(24, 23);
            this.labReadyDate.Name = "labReadyDate";
            this.labReadyDate.Size = new System.Drawing.Size(80, 23);
            this.labReadyDate.TabIndex = 95;
            this.labReadyDate.Text = "Ready Date";
            // 
            // labM
            // 
            this.labM.Location = new System.Drawing.Point(24, 59);
            this.labM.Name = "labM";
            this.labM.Size = new System.Drawing.Size(31, 23);
            this.labM.TabIndex = 96;
            this.labM.Text = "M";
            // 
            // labFactory
            // 
            this.labFactory.Location = new System.Drawing.Point(141, 59);
            this.labFactory.Name = "labFactory";
            this.labFactory.Size = new System.Drawing.Size(58, 23);
            this.labFactory.TabIndex = 97;
            this.labFactory.Text = "Factory";
            // 
            // labDateGap
            // 
            this.labDateGap.Location = new System.Drawing.Point(24, 95);
            this.labDateGap.Name = "labDateGap";
            this.labDateGap.Size = new System.Drawing.Size(305, 23);
            this.labDateGap.TabIndex = 98;
            this.labDateGap.Text = "Ready Date and SewingOutput Date gap(Days)";
            // 
            // numDateGap
            // 
            this.numDateGap.BackColor = System.Drawing.Color.White;
            this.numDateGap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numDateGap.Location = new System.Drawing.Point(332, 95);
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
            this.numDateGap.TabIndex = 99;
            this.numDateGap.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.boolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(202, 59);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 100;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(58, 59);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(71, 23);
            this.txtMdivision.TabIndex = 101;
            // 
            // labBrand
            // 
            this.labBrand.Location = new System.Drawing.Point(286, 59);
            this.labBrand.Name = "labBrand";
            this.labBrand.Size = new System.Drawing.Size(43, 23);
            this.labBrand.TabIndex = 102;
            this.labBrand.Text = "Brand";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(332, 59);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 103;
            // 
            // chkHoliday
            // 
            this.chkHoliday.AutoSize = true;
            this.chkHoliday.Checked = true;
            this.chkHoliday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHoliday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkHoliday.Location = new System.Drawing.Point(24, 125);
            this.chkHoliday.Name = "chkHoliday";
            this.chkHoliday.Size = new System.Drawing.Size(127, 21);
            this.chkHoliday.TabIndex = 104;
            this.chkHoliday.Text = "Exclude Holiday";
            this.chkHoliday.UseVisualStyleBackColor = true;
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(24, 150);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 161;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // R11
            // 
            this.ClientSize = new System.Drawing.Size(542, 196);
            this.Controls.Add(this.chkIncludeCancelOrder);
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
            this.Name = "R11";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R11. Sewing Ready Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
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
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.chkIncludeCancelOrder, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateRangeReadyDate;
        private Win.UI.Label labReadyDate;
        private Win.UI.Label labM;
        private Win.UI.Label labFactory;
        private Win.UI.Label labDateGap;
        private Win.UI.NumericBox numDateGap;
        private Class.txtfactory txtfactory;
        private Class.txtMdivision txtMdivision;
        private Win.UI.Label labBrand;
        private Class.txtbrand txtbrand;
        private Win.UI.CheckBox chkHoliday;
        private Win.UI.CheckBox chkIncludeCancelOrder;
    }
}
