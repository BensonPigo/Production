namespace Sci.Production.PPIC
{
    partial class R12
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
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.checkBulk = new Sci.Win.UI.CheckBox();
            this.checkSample = new Sci.Win.UI.CheckBox();
            this.checkMaterial = new Sci.Win.UI.CheckBox();
            this.checkForecast = new Sci.Win.UI.CheckBox();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.checkGarment = new Sci.Win.UI.CheckBox();
            this.checkSMTL = new Sci.Win.UI.CheckBox();
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(502, 12);
            this.print.TabIndex = 24;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(502, 48);
            this.toexcel.TabIndex = 25;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(502, 84);
            this.close.TabIndex = 26;
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(13, 11);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(102, 23);
            this.labelBuyerDelivery.TabIndex = 94;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(13, 39);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(102, 23);
            this.labelSCIDelivery.TabIndex = 95;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(13, 68);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(102, 23);
            this.labelSeason.TabIndex = 101;
            this.labelSeason.Text = "Season";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 97);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(102, 23);
            this.labelBrand.TabIndex = 102;
            this.labelBrand.Text = "Brand";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 127);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(102, 23);
            this.labelM.TabIndex = 105;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 155);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(102, 23);
            this.labelFactory.TabIndex = 106;
            this.labelFactory.Text = "Factory";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(13, 184);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(102, 23);
            this.labelCategory.TabIndex = 107;
            this.labelCategory.Text = "Category";
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(102, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(124, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(102, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(119, 11);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(227, 23);
            this.dateBuyerDelivery.TabIndex = 1;
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(102, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(124, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(102, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(119, 39);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(227, 23);
            this.dateSCIDelivery.TabIndex = 2;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(119, 126);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(66, 24);
            this.comboM.TabIndex = 13;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(119, 154);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(66, 24);
            this.comboFactory.TabIndex = 14;
            // 
            // checkBulk
            // 
            this.checkBulk.AutoSize = true;
            this.checkBulk.Checked = true;
            this.checkBulk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBulk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBulk.Location = new System.Drawing.Point(119, 184);
            this.checkBulk.Name = "checkBulk";
            this.checkBulk.Size = new System.Drawing.Size(54, 21);
            this.checkBulk.TabIndex = 15;
            this.checkBulk.Text = "Bulk";
            this.checkBulk.UseVisualStyleBackColor = true;
            // 
            // checkSample
            // 
            this.checkSample.AutoSize = true;
            this.checkSample.Checked = true;
            this.checkSample.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkSample.Location = new System.Drawing.Point(180, 184);
            this.checkSample.Name = "checkSample";
            this.checkSample.Size = new System.Drawing.Size(74, 21);
            this.checkSample.TabIndex = 16;
            this.checkSample.Text = "Sample";
            this.checkSample.UseVisualStyleBackColor = true;
            // 
            // checkMaterial
            // 
            this.checkMaterial.AutoSize = true;
            this.checkMaterial.Checked = true;
            this.checkMaterial.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkMaterial.Location = new System.Drawing.Point(260, 184);
            this.checkMaterial.Name = "checkMaterial";
            this.checkMaterial.Size = new System.Drawing.Size(77, 21);
            this.checkMaterial.TabIndex = 17;
            this.checkMaterial.Text = "Material";
            this.checkMaterial.UseVisualStyleBackColor = true;
            // 
            // checkForecast
            // 
            this.checkForecast.AutoSize = true;
            this.checkForecast.Checked = true;
            this.checkForecast.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkForecast.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkForecast.Location = new System.Drawing.Point(344, 184);
            this.checkForecast.Name = "checkForecast";
            this.checkForecast.Size = new System.Drawing.Size(82, 21);
            this.checkForecast.TabIndex = 18;
            this.checkForecast.Text = "Forecast";
            this.checkForecast.UseVisualStyleBackColor = true;
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(119, 68);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(98, 23);
            this.txtseason.TabIndex = 9;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(119, 97);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(98, 23);
            this.txtbrand.TabIndex = 10;
            // 
            // checkGarment
            // 
            this.checkGarment.AutoSize = true;
            this.checkGarment.Checked = true;
            this.checkGarment.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkGarment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkGarment.Location = new System.Drawing.Point(432, 184);
            this.checkGarment.Name = "checkGarment";
            this.checkGarment.Size = new System.Drawing.Size(82, 21);
            this.checkGarment.TabIndex = 19;
            this.checkGarment.Text = "Garment";
            this.checkGarment.UseVisualStyleBackColor = true;
            // 
            // checkSMTL
            // 
            this.checkSMTL.AutoSize = true;
            this.checkSMTL.Checked = true;
            this.checkSMTL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSMTL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkSMTL.Location = new System.Drawing.Point(520, 184);
            this.checkSMTL.Name = "checkSMTL";
            this.checkSMTL.Size = new System.Drawing.Size(64, 21);
            this.checkSMTL.TabIndex = 112;
            this.checkSMTL.Text = "SMTL";
            this.checkSMTL.UseVisualStyleBackColor = true;
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(13, 211);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 162;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // R12
            // 
            this.ClientSize = new System.Drawing.Size(587, 266);
            this.Controls.Add(this.chkIncludeCancelOrder);
            this.Controls.Add(this.checkSMTL);
            this.Controls.Add(this.checkGarment);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.txtseason);
            this.Controls.Add(this.checkForecast);
            this.Controls.Add(this.checkMaterial);
            this.Controls.Add(this.checkSample);
            this.Controls.Add(this.checkBulk);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelSeason);
            this.Controls.Add(this.labelSCIDelivery);
            this.Controls.Add(this.labelBuyerDelivery);
            this.DefaultControl = "dateBuyerDelivery";
            this.DefaultControlForEdit = "dateBuyerDelivery";
            this.IsSupportToPrint = false;
            this.Name = "R12";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R12. PAD Print Ink forecast usage amount";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.labelSeason, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.checkBulk, 0);
            this.Controls.SetChildIndex(this.checkSample, 0);
            this.Controls.SetChildIndex(this.checkMaterial, 0);
            this.Controls.SetChildIndex(this.checkForecast, 0);
            this.Controls.SetChildIndex(this.txtseason, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.checkGarment, 0);
            this.Controls.SetChildIndex(this.checkSMTL, 0);
            this.Controls.SetChildIndex(this.chkIncludeCancelOrder, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelCategory;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.CheckBox checkBulk;
        private Win.UI.CheckBox checkSample;
        private Win.UI.CheckBox checkMaterial;
        private Win.UI.CheckBox checkForecast;
        private Class.Txtseason txtseason;
        private Class.Txtbrand txtbrand;
        private Win.UI.CheckBox checkGarment;
        private Win.UI.CheckBox checkSMTL;
        private Win.UI.CheckBox chkIncludeCancelOrder;
    }
}
