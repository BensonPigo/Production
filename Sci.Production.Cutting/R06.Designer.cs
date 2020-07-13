namespace Sci.Production.Cutting
{
    partial class R06
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.dateReady = new Sci.Win.UI.DateRange();
            this.label5 = new Sci.Win.UI.Label();
            this.numDateOfflineGap = new Sci.Win.UI.NumericBox();
            this.numDateDeliveryGap = new Sci.Win.UI.NumericBox();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.chkHoliday = new Sci.Win.UI.CheckBox();
            this.numDateCutGapDay = new Sci.Win.UI.NumericBox();
            this.txtDateCutGapTime = new Sci.Win.UI.TextBox();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
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
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Ready Date";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 23);
            this.label2.TabIndex = 95;
            this.label2.Text = "M";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 23);
            this.label3.TabIndex = 96;
            this.label3.Text = "Factory";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 23);
            this.label4.TabIndex = 97;
            this.label4.Text = "Brand";
            // 
            // dateReady
            // 
            // 
            // 
            // 
            this.dateReady.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateReady.DateBox1.Name = "";
            this.dateReady.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateReady.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateReady.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateReady.DateBox2.Name = "";
            this.dateReady.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateReady.DateBox2.TabIndex = 1;
            this.dateReady.Location = new System.Drawing.Point(102, 12);
            this.dateReady.Name = "dateReady";
            this.dateReady.Size = new System.Drawing.Size(280, 23);
            this.dateReady.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(299, 23);
            this.label5.TabIndex = 102;
            this.label5.Text = "Ready Date and Sewing Offline gap(Days)";
            // 
            // numDateOfflineGap
            // 
            this.numDateOfflineGap.BackColor = System.Drawing.Color.White;
            this.numDateOfflineGap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numDateOfflineGap.Location = new System.Drawing.Point(311, 144);
            this.numDateOfflineGap.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numDateOfflineGap.Name = "numDateOfflineGap";
            this.numDateOfflineGap.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDateOfflineGap.Size = new System.Drawing.Size(29, 23);
            this.numDateOfflineGap.TabIndex = 5;
            this.numDateOfflineGap.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // numDateDeliveryGap
            // 
            this.numDateDeliveryGap.BackColor = System.Drawing.Color.White;
            this.numDateDeliveryGap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numDateDeliveryGap.Location = new System.Drawing.Point(311, 177);
            this.numDateDeliveryGap.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numDateDeliveryGap.Name = "numDateDeliveryGap";
            this.numDateDeliveryGap.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDateDeliveryGap.Size = new System.Drawing.Size(29, 23);
            this.numDateDeliveryGap.TabIndex = 6;
            this.numDateDeliveryGap.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(9, 177);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(299, 23);
            this.label6.TabIndex = 107;
            this.label6.Text = "Ready Date and Buyer Delivery Date gap(Days)";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(9, 210);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(299, 23);
            this.label7.TabIndex = 109;
            this.label7.Text = "Ready Date and Cut Qty gap";
            // 
            // chkHoliday
            // 
            this.chkHoliday.AutoSize = true;
            this.chkHoliday.Checked = true;
            this.chkHoliday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHoliday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkHoliday.Location = new System.Drawing.Point(9, 243);
            this.chkHoliday.Name = "chkHoliday";
            this.chkHoliday.Size = new System.Drawing.Size(127, 21);
            this.chkHoliday.TabIndex = 9;
            this.chkHoliday.Text = "Exclude Holiday";
            this.chkHoliday.UseVisualStyleBackColor = true;
            // 
            // numDateCutGapDay
            // 
            this.numDateCutGapDay.BackColor = System.Drawing.Color.White;
            this.numDateCutGapDay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numDateCutGapDay.Location = new System.Drawing.Point(311, 210);
            this.numDateCutGapDay.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numDateCutGapDay.Name = "numDateCutGapDay";
            this.numDateCutGapDay.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDateCutGapDay.Size = new System.Drawing.Size(29, 23);
            this.numDateCutGapDay.TabIndex = 7;
            this.numDateCutGapDay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtDateCutGapTime
            // 
            this.txtDateCutGapTime.BackColor = System.Drawing.Color.White;
            this.txtDateCutGapTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDateCutGapTime.Location = new System.Drawing.Point(346, 210);
            this.txtDateCutGapTime.Mask = "00:00";
            this.txtDateCutGapTime.Name = "txtDateCutGapTime";
            this.txtDateCutGapTime.Size = new System.Drawing.Size(55, 23);
            this.txtDateCutGapTime.TabIndex = 8;
            this.txtDateCutGapTime.Text = "1000";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(102, 111);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 4;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(102, 78);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 3;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(102, 45);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(71, 23);
            this.txtMdivision.TabIndex = 2;
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(9, 266);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 164;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // R06
            // 
            this.ClientSize = new System.Drawing.Size(591, 316);
            this.Controls.Add(this.chkIncludeCancelOrder);
            this.Controls.Add(this.txtDateCutGapTime);
            this.Controls.Add(this.numDateCutGapDay);
            this.Controls.Add(this.chkHoliday);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numDateDeliveryGap);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numDateOfflineGap);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateReady);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R06";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R06. Cutting Ready Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.dateReady, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.numDateOfflineGap, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.numDateDeliveryGap, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.chkHoliday, 0);
            this.Controls.SetChildIndex(this.numDateCutGapDay, 0);
            this.Controls.SetChildIndex(this.txtDateCutGapTime, 0);
            this.Controls.SetChildIndex(this.chkIncludeCancelOrder, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.DateRange dateReady;
        private Win.UI.Label label5;
        private Class.TxtMdivision txtMdivision;
        private Class.Txtfactory txtfactory;
        private Class.Txtbrand txtbrand;
        private Win.UI.NumericBox numDateOfflineGap;
        private Win.UI.NumericBox numDateDeliveryGap;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.CheckBox chkHoliday;
        private Win.UI.NumericBox numDateCutGapDay;
        private Win.UI.TextBox txtDateCutGapTime;
        private Win.UI.CheckBox chkIncludeCancelOrder;
    }
}
