namespace Sci.Production.PPIC
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
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelBuyerDeliveryDate = new Sci.Win.UI.Label();
            this.labelSPnum = new Sci.Win.UI.Label();
            this.labelMDivision = new Sci.Win.UI.Label();
            this.labelSewingOutputDate = new Sci.Win.UI.Label();
            this.dateRangeBuyerDelivery = new Sci.Win.UI.DateRange();
            this.textSPStart = new Sci.Win.UI.TextBox();
            this.textSPEnd = new Sci.Win.UI.TextBox();
            this.labelSign = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.txtMdivision = new Sci.Production.Class.txtMdivision();
            this.label1 = new Sci.Win.UI.Label();
            this.dateBoxSewingOutput = new Sci.Win.UI.DateBox();
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(437, -100);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(437, 12);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(437, 48);
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(9, 99);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(131, 23);
            this.labelFactory.TabIndex = 94;
            this.labelFactory.Text = "Factory";
            // 
            // labelBuyerDeliveryDate
            // 
            this.labelBuyerDeliveryDate.Location = new System.Drawing.Point(9, 9);
            this.labelBuyerDeliveryDate.Name = "labelBuyerDeliveryDate";
            this.labelBuyerDeliveryDate.Size = new System.Drawing.Size(131, 23);
            this.labelBuyerDeliveryDate.TabIndex = 94;
            this.labelBuyerDeliveryDate.Text = "Buyer Delivery Date";
            // 
            // labelSPnum
            // 
            this.labelSPnum.Location = new System.Drawing.Point(9, 39);
            this.labelSPnum.Name = "labelSPnum";
            this.labelSPnum.Size = new System.Drawing.Size(131, 23);
            this.labelSPnum.TabIndex = 94;
            this.labelSPnum.Text = "SP#";
            // 
            // labelMDivision
            // 
            this.labelMDivision.Location = new System.Drawing.Point(9, 69);
            this.labelMDivision.Name = "labelMDivision";
            this.labelMDivision.Size = new System.Drawing.Size(131, 23);
            this.labelMDivision.TabIndex = 94;
            this.labelMDivision.Text = "M";
            // 
            // labelSewingOutputDate
            // 
            this.labelSewingOutputDate.Location = new System.Drawing.Point(9, 129);
            this.labelSewingOutputDate.Name = "labelSewingOutputDate";
            this.labelSewingOutputDate.Size = new System.Drawing.Size(131, 23);
            this.labelSewingOutputDate.TabIndex = 94;
            this.labelSewingOutputDate.Text = "Sewing Output Date";
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
            this.dateRangeBuyerDelivery.IsRequired = false;
            this.dateRangeBuyerDelivery.Location = new System.Drawing.Point(144, 9);
            this.dateRangeBuyerDelivery.Name = "dateRangeBuyerDelivery";
            this.dateRangeBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateRangeBuyerDelivery.TabIndex = 95;
            // 
            // textSPStart
            // 
            this.textSPStart.BackColor = System.Drawing.Color.White;
            this.textSPStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textSPStart.Location = new System.Drawing.Point(144, 39);
            this.textSPStart.Name = "textSPStart";
            this.textSPStart.Size = new System.Drawing.Size(100, 23);
            this.textSPStart.TabIndex = 97;
            // 
            // textSPEnd
            // 
            this.textSPEnd.BackColor = System.Drawing.Color.White;
            this.textSPEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textSPEnd.Location = new System.Drawing.Point(277, 39);
            this.textSPEnd.Name = "textSPEnd";
            this.textSPEnd.Size = new System.Drawing.Size(100, 23);
            this.textSPEnd.TabIndex = 98;
            // 
            // labelSign
            // 
            this.labelSign.BackColor = System.Drawing.Color.Transparent;
            this.labelSign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.labelSign.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSign.Location = new System.Drawing.Point(247, 39);
            this.labelSign.Name = "labelSign";
            this.labelSign.Size = new System.Drawing.Size(27, 23);
            this.labelSign.TabIndex = 99;
            this.labelSign.Text = "~";
            this.labelSign.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelSign.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSign.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.boolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(144, 99);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 100;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(144, 69);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 101;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(323, 23);
            this.label1.TabIndex = 102;
            this.label1.Text = "** The sewing output date is base date of calculation";
            this.label1.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // dateBoxSewingOutput
            // 
            this.dateBoxSewingOutput.Location = new System.Drawing.Point(144, 129);
            this.dateBoxSewingOutput.Name = "dateBoxSewingOutput";
            this.dateBoxSewingOutput.Size = new System.Drawing.Size(130, 23);
            this.dateBoxSewingOutput.TabIndex = 103;
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(9, 156);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 160;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // R10
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 223);
            this.Controls.Add(this.chkIncludeCancelOrder);
            this.Controls.Add(this.dateBoxSewingOutput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelSign);
            this.Controls.Add(this.textSPEnd);
            this.Controls.Add(this.textSPStart);
            this.Controls.Add(this.dateRangeBuyerDelivery);
            this.Controls.Add(this.labelMDivision);
            this.Controls.Add(this.labelSPnum);
            this.Controls.Add(this.labelBuyerDeliveryDate);
            this.Controls.Add(this.labelSewingOutputDate);
            this.Controls.Add(this.labelFactory);
            this.Name = "R10";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R10. Potential Delay Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelSewingOutputDate, 0);
            this.Controls.SetChildIndex(this.labelBuyerDeliveryDate, 0);
            this.Controls.SetChildIndex(this.labelSPnum, 0);
            this.Controls.SetChildIndex(this.labelMDivision, 0);
            this.Controls.SetChildIndex(this.dateRangeBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.textSPStart, 0);
            this.Controls.SetChildIndex(this.textSPEnd, 0);
            this.Controls.SetChildIndex(this.labelSign, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.dateBoxSewingOutput, 0);
            this.Controls.SetChildIndex(this.chkIncludeCancelOrder, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelFactory;
        private Win.UI.Label labelBuyerDeliveryDate;
        private Win.UI.Label labelSPnum;
        private Win.UI.Label labelMDivision;
        private Win.UI.Label labelSewingOutputDate;
        private Win.UI.DateRange dateRangeBuyerDelivery;
        private Win.UI.TextBox textSPStart;
        private Win.UI.TextBox textSPEnd;
        private Win.UI.Label labelSign;
        private Class.txtfactory txtfactory;
        private Class.txtMdivision txtMdivision;
        private Win.UI.Label label1;
        private Win.UI.DateBox dateBoxSewingOutput;
        private Win.UI.CheckBox chkIncludeCancelOrder;
    }
}