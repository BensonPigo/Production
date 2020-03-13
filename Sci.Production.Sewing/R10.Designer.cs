namespace Sci.Production.Sewing
{
    partial class R10
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
            this.lbSCIDelivery = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.dateRangeByerDev = new Sci.Win.UI.DateRange();
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.txtMdivision = new Sci.Production.Class.txtMdivision();
            this.lbFactory = new Sci.Win.UI.Label();
            this.lbMdivision = new Sci.Win.UI.Label();
            this.lbBuyerDelivery = new Sci.Win.UI.Label();
            this.lbSewingOutputDate = new Sci.Win.UI.Label();
            this.dateRangeSewingOutputDate = new Sci.Win.UI.DateRange();
            this.chkOutstanding = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(470, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(470, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(470, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(209, 1);
            // 
            // lbSCIDelivery
            // 
            this.lbSCIDelivery.Location = new System.Drawing.Point(28, 48);
            this.lbSCIDelivery.Name = "lbSCIDelivery";
            this.lbSCIDelivery.Size = new System.Drawing.Size(134, 23);
            this.lbSCIDelivery.TabIndex = 551;
            this.lbSCIDelivery.Text = "SCI Delivery";
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(165, 48);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 2;
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
            this.dateRangeByerDev.IsRequired = false;
            this.dateRangeByerDev.Location = new System.Drawing.Point(165, 12);
            this.dateRangeByerDev.Name = "dateRangeByerDev";
            this.dateRangeByerDev.Size = new System.Drawing.Size(280, 23);
            this.dateRangeByerDev.TabIndex = 1;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.boolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(165, 156);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 5;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(165, 120);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 4;
            // 
            // lbFactory
            // 
            this.lbFactory.Location = new System.Drawing.Point(28, 156);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(134, 23);
            this.lbFactory.TabIndex = 559;
            this.lbFactory.Text = "Factory";
            // 
            // lbMdivision
            // 
            this.lbMdivision.Location = new System.Drawing.Point(28, 120);
            this.lbMdivision.Name = "lbMdivision";
            this.lbMdivision.Size = new System.Drawing.Size(134, 23);
            this.lbMdivision.TabIndex = 558;
            this.lbMdivision.Text = "M";
            // 
            // lbBuyerDelivery
            // 
            this.lbBuyerDelivery.Location = new System.Drawing.Point(28, 12);
            this.lbBuyerDelivery.Name = "lbBuyerDelivery";
            this.lbBuyerDelivery.Size = new System.Drawing.Size(134, 23);
            this.lbBuyerDelivery.TabIndex = 557;
            this.lbBuyerDelivery.Text = "Buyer Delivery";
            // 
            // lbSewingOutputDate
            // 
            this.lbSewingOutputDate.Location = new System.Drawing.Point(28, 84);
            this.lbSewingOutputDate.Name = "lbSewingOutputDate";
            this.lbSewingOutputDate.Size = new System.Drawing.Size(134, 23);
            this.lbSewingOutputDate.TabIndex = 560;
            this.lbSewingOutputDate.Text = "Sewing Output Date";
            // 
            // dateRangeSewingOutputDate
            // 
            // 
            // 
            // 
            this.dateRangeSewingOutputDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeSewingOutputDate.DateBox1.Name = "";
            this.dateRangeSewingOutputDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeSewingOutputDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeSewingOutputDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeSewingOutputDate.DateBox2.Name = "";
            this.dateRangeSewingOutputDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeSewingOutputDate.DateBox2.TabIndex = 1;
            this.dateRangeSewingOutputDate.IsRequired = false;
            this.dateRangeSewingOutputDate.Location = new System.Drawing.Point(165, 84);
            this.dateRangeSewingOutputDate.Name = "dateRangeSewingOutputDate";
            this.dateRangeSewingOutputDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeSewingOutputDate.TabIndex = 3;
            // 
            // chkOutstanding
            // 
            this.chkOutstanding.AutoSize = true;
            this.chkOutstanding.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkOutstanding.Location = new System.Drawing.Point(28, 196);
            this.chkOutstanding.Name = "chkOutstanding";
            this.chkOutstanding.Size = new System.Drawing.Size(104, 21);
            this.chkOutstanding.TabIndex = 561;
            this.chkOutstanding.Text = "Outstanding";
            this.chkOutstanding.UseVisualStyleBackColor = true;
            // 
            // R10
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 244);
            this.Controls.Add(this.chkOutstanding);
            this.Controls.Add(this.dateRangeSewingOutputDate);
            this.Controls.Add(this.lbSewingOutputDate);
            this.Controls.Add(this.lbSCIDelivery);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.dateRangeByerDev);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.lbFactory);
            this.Controls.Add(this.lbMdivision);
            this.Controls.Add(this.lbBuyerDelivery);
            this.Name = "R10";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R10. Order Qty v.s. Sewing Output Qty";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.lbBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.lbMdivision, 0);
            this.Controls.SetChildIndex(this.lbFactory, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.dateRangeByerDev, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.lbSCIDelivery, 0);
            this.Controls.SetChildIndex(this.lbSewingOutputDate, 0);
            this.Controls.SetChildIndex(this.dateRangeSewingOutputDate, 0);
            this.Controls.SetChildIndex(this.chkOutstanding, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbSCIDelivery;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.DateRange dateRangeByerDev;
        private Class.txtfactory txtfactory;
        private Class.txtMdivision txtMdivision;
        private Win.UI.Label lbFactory;
        private Win.UI.Label lbMdivision;
        private Win.UI.Label lbBuyerDelivery;
        private Win.UI.Label lbSewingOutputDate;
        private Win.UI.DateRange dateRangeSewingOutputDate;
        private Win.UI.CheckBox chkOutstanding;
    }
}