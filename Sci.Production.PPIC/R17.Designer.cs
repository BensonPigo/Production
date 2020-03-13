namespace Sci.Production.PPIC
{
    partial class R17
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
            this.lbSP = new Sci.Win.UI.Label();
            this.lbFactory = new Sci.Win.UI.Label();
            this.lbMdivision = new Sci.Win.UI.Label();
            this.lbBuyerDelivery = new Sci.Win.UI.Label();
            this.dateRangeByerDev = new Sci.Win.UI.DateRange();
            this.txtSP = new Sci.Production.Class.txtbrand();
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.txtMdivision = new Sci.Production.Class.txtMdivision();
            this.dateRangeCDate = new Sci.Win.UI.DateRange();
            this.lbCreateDate = new Sci.Win.UI.Label();
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
            // lbSP
            // 
            this.lbSP.Location = new System.Drawing.Point(31, 91);
            this.lbSP.Name = "lbSP";
            this.lbSP.Size = new System.Drawing.Size(100, 23);
            this.lbSP.TabIndex = 550;
            this.lbSP.Text = "SP#";
            // 
            // lbFactory
            // 
            this.lbFactory.Location = new System.Drawing.Point(31, 163);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(100, 23);
            this.lbFactory.TabIndex = 549;
            this.lbFactory.Text = "Factory";
            // 
            // lbMdivision
            // 
            this.lbMdivision.Location = new System.Drawing.Point(31, 127);
            this.lbMdivision.Name = "lbMdivision";
            this.lbMdivision.Size = new System.Drawing.Size(100, 23);
            this.lbMdivision.TabIndex = 548;
            this.lbMdivision.Text = "M";
            // 
            // lbBuyerDelivery
            // 
            this.lbBuyerDelivery.Location = new System.Drawing.Point(31, 55);
            this.lbBuyerDelivery.Name = "lbBuyerDelivery";
            this.lbBuyerDelivery.Size = new System.Drawing.Size(100, 23);
            this.lbBuyerDelivery.TabIndex = 547;
            this.lbBuyerDelivery.Text = "Buyer Delivery";
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
            this.dateRangeByerDev.Location = new System.Drawing.Point(147, 55);
            this.dateRangeByerDev.Name = "dateRangeByerDev";
            this.dateRangeByerDev.Size = new System.Drawing.Size(280, 23);
            this.dateRangeByerDev.TabIndex = 2;
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(147, 91);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(130, 23);
            this.txtSP.TabIndex = 3;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.boolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(147, 163);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 5;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(147, 127);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 4;
            // 
            // dateRangeCDate
            // 
            // 
            // 
            // 
            this.dateRangeCDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeCDate.DateBox1.Name = "";
            this.dateRangeCDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeCDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeCDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeCDate.DateBox2.Name = "";
            this.dateRangeCDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeCDate.DateBox2.TabIndex = 1;
            this.dateRangeCDate.IsRequired = false;
            this.dateRangeCDate.Location = new System.Drawing.Point(147, 19);
            this.dateRangeCDate.Name = "dateRangeCDate";
            this.dateRangeCDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeCDate.TabIndex = 1;
            // 
            // lbCreateDate
            // 
            this.lbCreateDate.Location = new System.Drawing.Point(31, 19);
            this.lbCreateDate.Name = "lbCreateDate";
            this.lbCreateDate.Size = new System.Drawing.Size(100, 23);
            this.lbCreateDate.TabIndex = 1;
            this.lbCreateDate.Text = "Create Date";
            // 
            // R17
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 224);
            this.Controls.Add(this.lbCreateDate);
            this.Controls.Add(this.dateRangeCDate);
            this.Controls.Add(this.dateRangeByerDev);
            this.Controls.Add(this.txtSP);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.lbSP);
            this.Controls.Add(this.lbFactory);
            this.Controls.Add(this.lbMdivision);
            this.Controls.Add(this.lbBuyerDelivery);
            this.IsSupportToPrint = false;
            this.Name = "R17";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R17. Buy Back List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.lbBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.lbMdivision, 0);
            this.Controls.SetChildIndex(this.lbFactory, 0);
            this.Controls.SetChildIndex(this.lbSP, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtSP, 0);
            this.Controls.SetChildIndex(this.dateRangeByerDev, 0);
            this.Controls.SetChildIndex(this.dateRangeCDate, 0);
            this.Controls.SetChildIndex(this.lbCreateDate, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbSP;
        private Win.UI.Label lbFactory;
        private Win.UI.Label lbMdivision;
        private Win.UI.Label lbBuyerDelivery;
        private Class.txtMdivision txtMdivision;
        private Class.txtfactory txtfactory;
        private Class.txtbrand txtSP;
        private Win.UI.DateRange dateRangeByerDev;
        private Win.UI.DateRange dateRangeCDate;
        private Win.UI.Label lbCreateDate;
    }
}