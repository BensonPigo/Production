namespace Sci.Production.PPIC
{
    partial class R13
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
            this.dateRangeReadyDate = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.checkHoliday = new Sci.Win.UI.CheckBox();
            this.label6 = new Sci.Win.UI.Label();
            this.txtFactory = new Sci.Production.Class.Txtfactory();
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.txtTime = new Sci.Win.UI.TextBox();
            this.numericBoxDateGap = new Sci.Win.UI.NumericBox();
            this.labBuyerDelivery = new Sci.Win.UI.Label();
            this.dateRangeBuyerDelivery = new Sci.Win.UI.DateRange();
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(543, 160);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(543, 12);
            this.toexcel.TabIndex = 7;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(543, 48);
            this.close.TabIndex = 8;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(202, 1);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(420, 1);
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
            this.dateRangeReadyDate.Location = new System.Drawing.Point(125, 19);
            this.dateRangeReadyDate.Name = "dateRangeReadyDate";
            this.dateRangeReadyDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeReadyDate.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 23);
            this.label1.TabIndex = 95;
            this.label1.Text = "Ready Date";
            this.label1.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(19, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 23);
            this.label2.TabIndex = 96;
            this.label2.Text = "M ";
            this.label2.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(19, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 23);
            this.label3.TabIndex = 97;
            this.label3.Text = "Factory ";
            this.label3.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(19, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 23);
            this.label4.TabIndex = 98;
            this.label4.Text = "Brand ";
            this.label4.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(19, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(316, 23);
            this.label5.TabIndex = 99;
            this.label5.Text = "Ready Date and Sewing Offline Date gap(Days)";
            this.label5.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkHoliday
            // 
            this.checkHoliday.AutoSize = true;
            this.checkHoliday.Checked = true;
            this.checkHoliday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkHoliday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkHoliday.Location = new System.Drawing.Point(416, 21);
            this.checkHoliday.Name = "checkHoliday";
            this.checkHoliday.Size = new System.Drawing.Size(127, 21);
            this.checkHoliday.TabIndex = 100;
            this.checkHoliday.Text = "Exclude Holiday";
            this.checkHoliday.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(378, 197);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 23);
            this.label6.TabIndex = 101;
            this.label6.Text = "Time ";
            this.label6.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.BoolFtyGroupList = true;
            this.txtFactory.FilteMDivision = false;
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.IsProduceFty = true;
            this.txtFactory.IssupportJunk = false;
            this.txtFactory.Location = new System.Drawing.Point(125, 125);
            this.txtFactory.MDivision = null;
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(66, 23);
            this.txtFactory.TabIndex = 3;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(125, 164);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(66, 23);
            this.txtBrand.TabIndex = 4;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(125, 88);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 2;
            // 
            // txtTime
            // 
            this.txtTime.BackColor = System.Drawing.Color.White;
            this.txtTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTime.Location = new System.Drawing.Point(426, 197);
            this.txtTime.Mask = "90:00";
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(48, 23);
            this.txtTime.TabIndex = 6;
            this.txtTime.Text = "1000";
            this.txtTime.ValidatingType = typeof(System.DateTime);
            // 
            // numericBoxDateGap
            // 
            this.numericBoxDateGap.BackColor = System.Drawing.Color.White;
            this.numericBoxDateGap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBoxDateGap.Location = new System.Drawing.Point(338, 197);
            this.numericBoxDateGap.MaxLength = 1;
            this.numericBoxDateGap.Name = "numericBoxDateGap";
            this.numericBoxDateGap.NullValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericBoxDateGap.Size = new System.Drawing.Size(26, 23);
            this.numericBoxDateGap.TabIndex = 5;
            this.numericBoxDateGap.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labBuyerDelivery
            // 
            this.labBuyerDelivery.Location = new System.Drawing.Point(19, 55);
            this.labBuyerDelivery.Name = "labBuyerDelivery";
            this.labBuyerDelivery.Size = new System.Drawing.Size(103, 23);
            this.labBuyerDelivery.TabIndex = 108;
            this.labBuyerDelivery.Text = "Buyer Delivery";
            this.labBuyerDelivery.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateRangeBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateRangeBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeBuyerDelivery.DateBox1.Name = "";
            this.dateRangeBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeBuyerDelivery.DateBox2.Name = "";
            this.dateRangeBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateRangeBuyerDelivery.Location = new System.Drawing.Point(125, 55);
            this.dateRangeBuyerDelivery.Name = "dateRangeBuyerDelivery";
            this.dateRangeBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateRangeBuyerDelivery.TabIndex = 1;
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(19, 223);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 163;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // R13
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 273);
            this.Controls.Add(this.chkIncludeCancelOrder);
            this.Controls.Add(this.labBuyerDelivery);
            this.Controls.Add(this.dateRangeBuyerDelivery);
            this.Controls.Add(this.numericBoxDateGap);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.txtFactory);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkHoliday);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateRangeReadyDate);
            this.Name = "R13";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R13. SubProcess Ready Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.dateRangeReadyDate, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.checkHoliday, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtFactory, 0);
            this.Controls.SetChildIndex(this.txtBrand, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtTime, 0);
            this.Controls.SetChildIndex(this.numericBoxDateGap, 0);
            this.Controls.SetChildIndex(this.dateRangeBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.chkIncludeCancelOrder, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateRangeReadyDate;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.CheckBox checkHoliday;
        private Win.UI.Label label6;
        private Class.Txtfactory txtFactory;
        private Class.Txtbrand txtBrand;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.TextBox txtTime;
        private Win.UI.NumericBox numericBoxDateGap;
        private Win.UI.Label labBuyerDelivery;
        private Win.UI.DateRange dateRangeBuyerDelivery;
        private Win.UI.CheckBox chkIncludeCancelOrder;
    }
}