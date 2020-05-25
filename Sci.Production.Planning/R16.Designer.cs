namespace Sci.Production.Planning
{
    partial class R16
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dateSewingDate = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.labelM = new Sci.Win.UI.Label();
            this.txtMdivision = new Sci.Production.Class.txtMdivision();
            this.dateSciDelivery = new Sci.Win.UI.DateRange();
            this.labelSciDelivery = new Sci.Win.UI.Label();
            this.labelSewingDate = new Sci.Win.UI.Label();
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(434, 12);
            this.print.TabIndex = 4;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(434, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(434, 84);
            this.close.TabIndex = 6;
            // 
            // dateSewingDate
            // 
            // 
            // 
            // 
            this.dateSewingDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSewingDate.DateBox1.Name = "";
            this.dateSewingDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSewingDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSewingDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSewingDate.DateBox2.Name = "";
            this.dateSewingDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSewingDate.DateBox2.TabIndex = 1;
            this.dateSewingDate.IsRequired = false;
            this.dateSewingDate.Location = new System.Drawing.Point(111, 48);
            this.dateSewingDate.Name = "dateSewingDate";
            this.dateSewingDate.Size = new System.Drawing.Size(280, 23);
            this.dateSewingDate.TabIndex = 1;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(9, 114);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 132;
            this.labelFactory.Text = "Factory";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.boolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(111, 114);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 3;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(9, 81);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 134;
            this.labelM.Text = "M";
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(111, 81);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 2;
            // 
            // dateSciDelivery
            // 
            // 
            // 
            // 
            this.dateSciDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSciDelivery.DateBox1.Name = "";
            this.dateSciDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSciDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSciDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSciDelivery.DateBox2.Name = "";
            this.dateSciDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSciDelivery.DateBox2.TabIndex = 1;
            this.dateSciDelivery.IsRequired = false;
            this.dateSciDelivery.Location = new System.Drawing.Point(111, 12);
            this.dateSciDelivery.Name = "dateSciDelivery";
            this.dateSciDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSciDelivery.TabIndex = 0;
            // 
            // labelSciDelivery
            // 
            this.labelSciDelivery.Location = new System.Drawing.Point(9, 12);
            this.labelSciDelivery.Name = "labelSciDelivery";
            this.labelSciDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSciDelivery.RectStyle.BorderWidth = 1F;
            this.labelSciDelivery.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelSciDelivery.RectStyle.ExtBorderWidth = 1F;
            this.labelSciDelivery.Size = new System.Drawing.Size(98, 23);
            this.labelSciDelivery.TabIndex = 136;
            this.labelSciDelivery.Text = "Sci Delivery";
            this.labelSciDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSciDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSewingDate
            // 
            this.labelSewingDate.Location = new System.Drawing.Point(9, 48);
            this.labelSewingDate.Name = "labelSewingDate";
            this.labelSewingDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSewingDate.RectStyle.BorderWidth = 1F;
            this.labelSewingDate.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelSewingDate.RectStyle.ExtBorderWidth = 1F;
            this.labelSewingDate.Size = new System.Drawing.Size(98, 23);
            this.labelSewingDate.TabIndex = 137;
            this.labelSewingDate.Text = "Sewing Date";
            this.labelSewingDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSewingDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(9, 143);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 159;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // R16
            // 
            this.ClientSize = new System.Drawing.Size(522, 230);
            this.Controls.Add(this.chkIncludeCancelOrder);
            this.Controls.Add(this.labelSewingDate);
            this.Controls.Add(this.labelSciDelivery);
            this.Controls.Add(this.dateSciDelivery);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.dateSewingDate);
            this.DefaultControl = "dateSciDelivery";
            this.DefaultControlForEdit = "dateSciDelivery";
            this.IsSupportToPrint = false;
            this.Name = "R16";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R16. Critical Activity Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.dateSewingDate, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.dateSciDelivery, 0);
            this.Controls.SetChildIndex(this.labelSciDelivery, 0);
            this.Controls.SetChildIndex(this.labelSewingDate, 0);
            this.Controls.SetChildIndex(this.chkIncludeCancelOrder, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateSewingDate;
        private Win.UI.Label labelFactory;
        private Class.txtfactory txtfactory;
        private Win.UI.Label labelM;
        private Class.txtMdivision txtMdivision;
        private Win.UI.DateRange dateSciDelivery;
        private Win.UI.Label labelSciDelivery;
        private Win.UI.Label labelSewingDate;
        private Win.UI.CheckBox chkIncludeCancelOrder;
    }
}
