namespace Sci.Production.Planning
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelNewStyleBaseOn = new Sci.Win.UI.Label();
            this.labelmonth = new Sci.Win.UI.Label();
            this.numNewStyleBaseOn = new Sci.Win.UI.NumericUpDown();
            this.txtMdivision = new Sci.Production.Class.txtMdivision();
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.comboCategory = new Sci.Production.Class.comboDropDownList(this.components);
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numNewStyleBaseOn)).BeginInit();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(480, 12);
            this.print.TabIndex = 5;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(480, 48);
            this.toexcel.TabIndex = 6;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(480, 84);
            this.close.TabIndex = 7;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(13, 12);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.RectStyle.BorderWidth = 1F;
            this.labelSCIDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSCIDelivery.RectStyle.ExtBorderWidth = 1F;
            this.labelSCIDelivery.Size = new System.Drawing.Size(98, 23);
            this.labelSCIDelivery.TabIndex = 96;
            this.labelSCIDelivery.Text = "SCI Delivery";
            this.labelSCIDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
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
            this.dateSCIDelivery.Location = new System.Drawing.Point(115, 12);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 0;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 83);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 132;
            this.labelFactory.Text = "Factory";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 48);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 134;
            this.labelM.Text = "M";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(13, 121);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(98, 23);
            this.labelCategory.TabIndex = 136;
            this.labelCategory.Text = "Category";
            // 
            // labelNewStyleBaseOn
            // 
            this.labelNewStyleBaseOn.Location = new System.Drawing.Point(13, 161);
            this.labelNewStyleBaseOn.Name = "labelNewStyleBaseOn";
            this.labelNewStyleBaseOn.Size = new System.Drawing.Size(126, 23);
            this.labelNewStyleBaseOn.TabIndex = 138;
            this.labelNewStyleBaseOn.Text = "New Style Base on";
            // 
            // labelmonth
            // 
            this.labelmonth.Location = new System.Drawing.Point(198, 161);
            this.labelmonth.Name = "labelmonth";
            this.labelmonth.Size = new System.Drawing.Size(61, 23);
            this.labelmonth.TabIndex = 139;
            this.labelmonth.Text = "month(s)";
            // 
            // numNewStyleBaseOn
            // 
            this.numNewStyleBaseOn.BackColor = System.Drawing.Color.White;
            this.numNewStyleBaseOn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numNewStyleBaseOn.Location = new System.Drawing.Point(142, 161);
            this.numNewStyleBaseOn.Name = "numNewStyleBaseOn";
            this.numNewStyleBaseOn.Size = new System.Drawing.Size(53, 23);
            this.numNewStyleBaseOn.TabIndex = 4;
            this.numNewStyleBaseOn.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(115, 49);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 1;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.boolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(115, 83);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 2;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(115, 120);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(184, 24);
            this.comboCategory.TabIndex = 142;
            this.comboCategory.Type = "Pms_ReportForProdEff";
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(188, 51);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 143;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // R11
            // 
            this.ClientSize = new System.Drawing.Size(568, 219);
            this.Controls.Add(this.chkIncludeCancelOrder);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.numNewStyleBaseOn);
            this.Controls.Add(this.labelmonth);
            this.Controls.Add(this.labelNewStyleBaseOn);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.labelSCIDelivery);
            this.DefaultControl = "dateSCIDelivery";
            this.DefaultControlForEdit = "dateSCIDelivery";
            this.IsSupportToPrint = false;
            this.Name = "R11";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R11. Prod. Efficiency record by  Style";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.labelNewStyleBaseOn, 0);
            this.Controls.SetChildIndex(this.labelmonth, 0);
            this.Controls.SetChildIndex(this.numNewStyleBaseOn, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.Controls.SetChildIndex(this.chkIncludeCancelOrder, 0);
            ((System.ComponentModel.ISupportInitialize)(this.numNewStyleBaseOn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSCIDelivery;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.Label labelFactory;
        private Class.txtfactory txtfactory;
        private Win.UI.Label labelM;
        private Class.txtMdivision txtMdivision;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelNewStyleBaseOn;
        private Win.UI.Label labelmonth;
        private Win.UI.NumericUpDown numNewStyleBaseOn;
        private Class.comboDropDownList comboCategory;
        private Win.UI.CheckBox chkIncludeCancelOrder;
    }
}
