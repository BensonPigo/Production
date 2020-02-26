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
            this.lblInvoiceSer = new Sci.Win.UI.Label();
            this.lblShipper = new Sci.Win.UI.Label();
            this.dateInvoice = new Sci.Win.UI.DateRange();
            this.txtInvSerFrom = new Sci.Win.UI.TextBox();
            this.txtInvSerTo = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtShipper = new Sci.Win.UI.TextBox();
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
            // lblInvoiceSer
            // 
            this.lblInvoiceSer.Location = new System.Drawing.Point(24, 101);
            this.lblInvoiceSer.Name = "lblInvoiceSer";
            this.lblInvoiceSer.Size = new System.Drawing.Size(100, 23);
            this.lblInvoiceSer.TabIndex = 104;
            this.lblInvoiceSer.Text = "Invoice Serial";
            // 
            // lblShipper
            // 
            this.lblShipper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lblShipper.Location = new System.Drawing.Point(24, 131);
            this.lblShipper.Name = "lblShipper";
            this.lblShipper.Size = new System.Drawing.Size(100, 23);
            this.lblShipper.TabIndex = 105;
            this.lblShipper.Text = "Shipper";
            this.lblShipper.TextStyle.Color = System.Drawing.Color.Black;
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
            // txtInvSerFrom
            // 
            this.txtInvSerFrom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtInvSerFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtInvSerFrom.Location = new System.Drawing.Point(127, 101);
            this.txtInvSerFrom.Name = "txtInvSerFrom";
            this.txtInvSerFrom.ReadOnly = true;
            this.txtInvSerFrom.Size = new System.Drawing.Size(129, 23);
            this.txtInvSerFrom.TabIndex = 107;
            // 
            // txtInvSerTo
            // 
            this.txtInvSerTo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtInvSerTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtInvSerTo.Location = new System.Drawing.Point(278, 101);
            this.txtInvSerTo.Name = "txtInvSerTo";
            this.txtInvSerTo.ReadOnly = true;
            this.txtInvSerTo.Size = new System.Drawing.Size(129, 23);
            this.txtInvSerTo.TabIndex = 108;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(256, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 23);
            this.label1.TabIndex = 109;
            this.label1.Text = "～";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtShipper
            // 
            this.txtShipper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtShipper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtShipper.Location = new System.Drawing.Point(127, 131);
            this.txtShipper.Name = "txtShipper";
            this.txtShipper.ReadOnly = true;
            this.txtShipper.Size = new System.Drawing.Size(129, 23);
            this.txtShipper.TabIndex = 110;
            // 
            // P11_Print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 201);
            this.Controls.Add(this.txtShipper);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInvSerTo);
            this.Controls.Add(this.txtInvSerFrom);
            this.Controls.Add(this.dateInvoice);
            this.Controls.Add(this.lblShipper);
            this.Controls.Add(this.lblInvoiceSer);
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
            this.Controls.SetChildIndex(this.lblInvoiceSer, 0);
            this.Controls.SetChildIndex(this.lblShipper, 0);
            this.Controls.SetChildIndex(this.dateInvoice, 0);
            this.Controls.SetChildIndex(this.txtInvSerFrom, 0);
            this.Controls.SetChildIndex(this.txtInvSerTo, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtShipper, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioButton radioBIRSalesReport;
        private Win.UI.RadioButton radioBIRSalesInv;
        private Win.UI.Label lblInvDate;
        private Win.UI.Label lblInvoiceSer;
        private Win.UI.Label lblShipper;
        private Win.UI.DateRange dateInvoice;
        private Win.UI.TextBox txtInvSerFrom;
        private Win.UI.TextBox txtInvSerTo;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtShipper;
    }
}