namespace Sci.Production.PPIC
{
    partial class R16
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
            this.lbBrand = new Sci.Win.UI.Label();
            this.lbSP = new Sci.Win.UI.Label();
            this.lbSCIDelivery = new Sci.Win.UI.Label();
            this.lbReport = new Sci.Win.UI.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateRangeByerDev = new Sci.Win.UI.DateRange();
            this.chkOutstanding = new Sci.Win.UI.CheckBox();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.chkExcludeSis = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(460, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(460, 48);
            this.toexcel.TabIndex = 7;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(460, 84);
            this.close.TabIndex = 8;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(386, 138);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(412, 138);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(430, 136);
            // 
            // lbBrand
            // 
            this.lbBrand.Location = new System.Drawing.Point(31, 136);
            this.lbBrand.Name = "lbBrand";
            this.lbBrand.Size = new System.Drawing.Size(100, 23);
            this.lbBrand.TabIndex = 550;
            this.lbBrand.Text = "Brand";
            // 
            // lbSP
            // 
            this.lbSP.Location = new System.Drawing.Point(31, 96);
            this.lbSP.Name = "lbSP";
            this.lbSP.Size = new System.Drawing.Size(100, 23);
            this.lbSP.TabIndex = 549;
            this.lbSP.Text = "Factory";
            // 
            // lbSCIDelivery
            // 
            this.lbSCIDelivery.Location = new System.Drawing.Point(31, 56);
            this.lbSCIDelivery.Name = "lbSCIDelivery";
            this.lbSCIDelivery.Size = new System.Drawing.Size(100, 23);
            this.lbSCIDelivery.TabIndex = 548;
            this.lbSCIDelivery.Text = "M";
            // 
            // lbReport
            // 
            this.lbReport.Location = new System.Drawing.Point(31, 19);
            this.lbReport.Name = "lbReport";
            this.lbReport.Size = new System.Drawing.Size(100, 23);
            this.lbReport.TabIndex = 547;
            this.lbReport.Text = "Buyer Delivery";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(28, 189);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(364, 17);
            this.label1.TabIndex = 555;
            this.label1.Text = "This report include Bulk and Garment order only.";
            // 
            // dateRangeByerDev
            // 
            // 
            // 
            // 
            this.dateRangeByerDev.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeByerDev.DateBox1.Name = "";
            this.dateRangeByerDev.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeByerDev.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeByerDev.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeByerDev.DateBox2.Name = "";
            this.dateRangeByerDev.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeByerDev.DateBox2.TabIndex = 1;
            this.dateRangeByerDev.Location = new System.Drawing.Point(147, 19);
            this.dateRangeByerDev.Name = "dateRangeByerDev";
            this.dateRangeByerDev.Size = new System.Drawing.Size(280, 23);
            this.dateRangeByerDev.TabIndex = 1;
            // 
            // chkOutstanding
            // 
            this.chkOutstanding.AutoSize = true;
            this.chkOutstanding.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkOutstanding.Location = new System.Drawing.Point(257, 56);
            this.chkOutstanding.Name = "chkOutstanding";
            this.chkOutstanding.Size = new System.Drawing.Size(128, 21);
            this.chkOutstanding.TabIndex = 5;
            this.chkOutstanding.Text = "Outstanding PO";
            this.chkOutstanding.UseVisualStyleBackColor = true;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(147, 136);
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
            this.txtfactory.Location = new System.Drawing.Point(147, 96);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 3;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(147, 56);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 2;
            // 
            // chkExcludeSis
            // 
            this.chkExcludeSis.AutoSize = true;
            this.chkExcludeSis.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkExcludeSis.Location = new System.Drawing.Point(257, 84);
            this.chkExcludeSis.Name = "chkExcludeSis";
            this.chkExcludeSis.Size = new System.Drawing.Size(191, 21);
            this.chkExcludeSis.TabIndex = 6;
            this.chkExcludeSis.Text = "Exclude sister transfer out";
            this.chkExcludeSis.UseVisualStyleBackColor = true;
            // 
            // R16
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 254);
            this.Controls.Add(this.chkExcludeSis);
            this.Controls.Add(this.chkOutstanding);
            this.Controls.Add(this.dateRangeByerDev);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.lbBrand);
            this.Controls.Add(this.lbSP);
            this.Controls.Add(this.lbSCIDelivery);
            this.Controls.Add(this.lbReport);
            this.IsSupportToPrint = false;
            this.Name = "R16";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R16. Outstanding PO Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.lbReport, 0);
            this.Controls.SetChildIndex(this.lbSCIDelivery, 0);
            this.Controls.SetChildIndex(this.lbSP, 0);
            this.Controls.SetChildIndex(this.lbBrand, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.dateRangeByerDev, 0);
            this.Controls.SetChildIndex(this.chkOutstanding, 0);
            this.Controls.SetChildIndex(this.chkExcludeSis, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbBrand;
        private Win.UI.Label lbSP;
        private Win.UI.Label lbSCIDelivery;
        private Win.UI.Label lbReport;
        private Class.TxtMdivision txtMdivision;
        private Class.Txtfactory txtfactory;
        private Class.Txtbrand txtbrand;
        private System.Windows.Forms.Label label1;
        private Win.UI.DateRange dateRangeByerDev;
        private Win.UI.CheckBox chkOutstanding;
        private Win.UI.CheckBox chkExcludeSis;
    }
}