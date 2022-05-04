namespace Sci.Production.Shipping
{
    partial class P11_Print
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
            this.radioBIRSalesReport = new Sci.Win.UI.RadioButton();
            this.radioBIRSalesInv = new Sci.Win.UI.RadioButton();
            this.lblInvDate = new Sci.Win.UI.Label();
            this.dateInvoice = new Sci.Win.UI.DateRange();
            this.txtShipper = new Sci.Win.UI.TextBox();
            this.txtGBNo = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.chkOutStanding = new Sci.Win.UI.CheckBox();
            this.labBrand = new Sci.Win.UI.Label();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(218, 3);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(462, 8);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(462, 44);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(426, 1);
            // 
            // radioBIRSalesReport
            // 
            this.radioBIRSalesReport.AutoSize = true;
            this.radioBIRSalesReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBIRSalesReport.Location = new System.Drawing.Point(12, 35);
            this.radioBIRSalesReport.Name = "radioBIRSalesReport";
            this.radioBIRSalesReport.Size = new System.Drawing.Size(134, 21);
            this.radioBIRSalesReport.TabIndex = 102;
            this.radioBIRSalesReport.Text = "BIR Sales Report";
            this.radioBIRSalesReport.UseVisualStyleBackColor = true;
            this.radioBIRSalesReport.CheckedChanged += new System.EventHandler(this.RadioBIRSalesReport_CheckedChanged);
            // 
            // radioBIRSalesInv
            // 
            this.radioBIRSalesInv.AutoSize = true;
            this.radioBIRSalesInv.Checked = true;
            this.radioBIRSalesInv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBIRSalesInv.Location = new System.Drawing.Point(12, 8);
            this.radioBIRSalesInv.Name = "radioBIRSalesInv";
            this.radioBIRSalesInv.Size = new System.Drawing.Size(135, 21);
            this.radioBIRSalesInv.TabIndex = 101;
            this.radioBIRSalesInv.TabStop = true;
            this.radioBIRSalesInv.Text = "BIR Sales Invoice";
            this.radioBIRSalesInv.UseVisualStyleBackColor = true;
            this.radioBIRSalesInv.CheckedChanged += new System.EventHandler(this.RadioBIRSalesReport_CheckedChanged);
            // 
            // lblInvDate
            // 
            this.lblInvDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lblInvDate.Location = new System.Drawing.Point(24, 71);
            this.lblInvDate.Name = "lblInvDate";
            this.lblInvDate.Size = new System.Drawing.Size(100, 23);
            this.lblInvDate.TabIndex = 103;
            this.lblInvDate.Text = "Invoice Date";
            this.lblInvDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateInvoice
            // 
            // 
            // 
            // 
            this.dateInvoice.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInvoice.DateBox1.Name = "";
            this.dateInvoice.DateBox1.ReadOnly = true;
            this.dateInvoice.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateInvoice.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInvoice.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateInvoice.DateBox2.Name = "";
            this.dateInvoice.DateBox2.ReadOnly = true;
            this.dateInvoice.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateInvoice.DateBox2.TabIndex = 1;
            this.dateInvoice.Location = new System.Drawing.Point(127, 71);
            this.dateInvoice.Name = "dateInvoice";
            this.dateInvoice.ReadOnly = true;
            this.dateInvoice.Size = new System.Drawing.Size(280, 23);
            this.dateInvoice.TabIndex = 106;
            // 
            // txtShipper
            // 
            this.txtShipper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtShipper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtShipper.Location = new System.Drawing.Point(127, 102);
            this.txtShipper.Name = "txtShipper";
            this.txtShipper.ReadOnly = true;
            this.txtShipper.Size = new System.Drawing.Size(129, 23);
            this.txtShipper.TabIndex = 110;
            // 
            // txtGBNo
            // 
            this.txtGBNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtGBNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtGBNo.Location = new System.Drawing.Point(127, 168);
            this.txtGBNo.Name = "txtGBNo";
            this.txtGBNo.ReadOnly = true;
            this.txtGBNo.Size = new System.Drawing.Size(129, 23);
            this.txtGBNo.TabIndex = 112;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label2.Location = new System.Drawing.Point(24, 168);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 111;
            this.label2.Text = "GB#";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 113;
            this.label3.Text = "Shipper";
            // 
            // chkOutStanding
            // 
            this.chkOutStanding.AutoSize = true;
            this.chkOutStanding.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkOutStanding.Location = new System.Drawing.Point(436, 101);
            this.chkOutStanding.Name = "chkOutStanding";
            this.chkOutStanding.Size = new System.Drawing.Size(106, 21);
            this.chkOutStanding.TabIndex = 114;
            this.chkOutStanding.Text = "OutStanding";
            this.chkOutStanding.UseVisualStyleBackColor = true;
            this.chkOutStanding.CheckedChanged += new System.EventHandler(this.ChkOutStanding_CheckedChanged);
            // 
            // labBrand
            // 
            this.labBrand.Location = new System.Drawing.Point(24, 135);
            this.labBrand.Name = "labBrand";
            this.labBrand.Size = new System.Drawing.Size(100, 23);
            this.labBrand.TabIndex = 116;
            this.labBrand.Text = "Brand";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtbrand.IsSupportEditMode = false;
            this.txtbrand.Location = new System.Drawing.Point(127, 135);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.ReadOnly = true;
            this.txtbrand.Size = new System.Drawing.Size(129, 23);
            this.txtbrand.TabIndex = 117;
            // 
            // P11_Print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 238);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.labBrand);
            this.Controls.Add(this.chkOutStanding);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtGBNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtShipper);
            this.Controls.Add(this.dateInvoice);
            this.Controls.Add(this.lblInvDate);
            this.Controls.Add(this.radioBIRSalesReport);
            this.Controls.Add(this.radioBIRSalesInv);
            this.Name = "P11_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "P11_Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.radioBIRSalesInv, 0);
            this.Controls.SetChildIndex(this.radioBIRSalesReport, 0);
            this.Controls.SetChildIndex(this.lblInvDate, 0);
            this.Controls.SetChildIndex(this.dateInvoice, 0);
            this.Controls.SetChildIndex(this.txtShipper, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtGBNo, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.chkOutStanding, 0);
            this.Controls.SetChildIndex(this.labBrand, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioButton radioBIRSalesReport;
        private Win.UI.RadioButton radioBIRSalesInv;
        private Win.UI.Label lblInvDate;
        private Win.UI.DateRange dateInvoice;
        private Win.UI.TextBox txtShipper;
        private Win.UI.TextBox txtGBNo;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.CheckBox chkOutStanding;
        private Win.UI.Label labBrand;
        private Class.Txtbrand txtbrand;
    }
}