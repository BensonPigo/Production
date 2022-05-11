namespace Sci.Production.IE
{
    partial class R08
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
            this.lbBrand = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.lbBuyerDelivery = new Sci.Win.UI.Label();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtmulitOperation1 = new Sci.Production.Class.TxtmulitOperation();
            this.lbOperation = new Sci.Win.UI.Label();
            this.comboMDivision = new Sci.Production.Class.ComboMDivision(this.components);
            this.lbM = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.lbFactory = new Sci.Win.UI.Label();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.lbSeason = new Sci.Win.UI.Label();
            this.chkSample = new Sci.Win.UI.CheckBox();
            this.chkBulk = new Sci.Win.UI.CheckBox();
            this.chkForecast = new Sci.Win.UI.CheckBox();
            this.lbCategory = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(419, 12);
            this.print.TabIndex = 8;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(419, 48);
            this.toexcel.TabIndex = 9;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(419, 84);
            this.close.TabIndex = 10;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(373, 148);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(399, 121);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(399, 121);
            // 
            // lbBrand
            // 
            this.lbBrand.Location = new System.Drawing.Point(9, 119);
            this.lbBrand.Name = "lbBrand";
            this.lbBrand.Size = new System.Drawing.Size(104, 23);
            this.lbBrand.TabIndex = 16;
            this.lbBrand.Text = "Brand";
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(116, 12);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 0;
            // 
            // lbBuyerDelivery
            // 
            this.lbBuyerDelivery.Location = new System.Drawing.Point(9, 12);
            this.lbBuyerDelivery.Name = "lbBuyerDelivery";
            this.lbBuyerDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lbBuyerDelivery.RectStyle.BorderWidth = 1F;
            this.lbBuyerDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbBuyerDelivery.RectStyle.ExtBorderWidth = 1F;
            this.lbBuyerDelivery.Size = new System.Drawing.Size(104, 23);
            this.lbBuyerDelivery.TabIndex = 11;
            this.lbBuyerDelivery.Text = "Buyer Delivery";
            this.lbBuyerDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lbBuyerDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(116, 119);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(91, 23);
            this.txtbrand.TabIndex = 5;
            // 
            // txtmulitOperation1
            // 
            this.txtmulitOperation1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtmulitOperation1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtmulitOperation1.IsJunk = false;
            this.txtmulitOperation1.IsSupportEditMode = false;
            this.txtmulitOperation1.Location = new System.Drawing.Point(116, 48);
            this.txtmulitOperation1.Name = "txtmulitOperation1";
            this.txtmulitOperation1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtmulitOperation1.ReadOnly = true;
            this.txtmulitOperation1.Size = new System.Drawing.Size(280, 23);
            this.txtmulitOperation1.TabIndex = 3;
            // 
            // lbOperation
            // 
            this.lbOperation.Location = new System.Drawing.Point(9, 48);
            this.lbOperation.Name = "lbOperation";
            this.lbOperation.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lbOperation.RectStyle.BorderWidth = 1F;
            this.lbOperation.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbOperation.RectStyle.ExtBorderWidth = 1F;
            this.lbOperation.Size = new System.Drawing.Size(104, 23);
            this.lbOperation.TabIndex = 12;
            this.lbOperation.Text = "Operation";
            this.lbOperation.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lbOperation.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // comboMDivision
            // 
            this.comboMDivision.BackColor = System.Drawing.Color.White;
            this.comboMDivision.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboMDivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMDivision.FormattingEnabled = true;
            this.comboMDivision.IsSupportUnselect = true;
            this.comboMDivision.Location = new System.Drawing.Point(116, 154);
            this.comboMDivision.Name = "comboMDivision";
            this.comboMDivision.OldText = "";
            this.comboMDivision.Size = new System.Drawing.Size(91, 24);
            this.comboMDivision.TabIndex = 145;
            // 
            // lbM
            // 
            this.lbM.Location = new System.Drawing.Point(9, 155);
            this.lbM.Name = "lbM";
            this.lbM.Size = new System.Drawing.Size(104, 23);
            this.lbM.TabIndex = 146;
            this.lbM.Text = "M";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(116, 190);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(91, 24);
            this.comboFactory.TabIndex = 147;
            // 
            // lbFactory
            // 
            this.lbFactory.Location = new System.Drawing.Point(9, 191);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(104, 23);
            this.lbFactory.TabIndex = 148;
            this.lbFactory.Text = "Factory";
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(116, 84);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(91, 23);
            this.txtseason.TabIndex = 149;
            // 
            // lbSeason
            // 
            this.lbSeason.Location = new System.Drawing.Point(9, 84);
            this.lbSeason.Name = "lbSeason";
            this.lbSeason.Size = new System.Drawing.Size(104, 23);
            this.lbSeason.TabIndex = 150;
            this.lbSeason.Text = "Season";
            // 
            // chkSample
            // 
            this.chkSample.AutoSize = true;
            this.chkSample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSample.IsSupportEditMode = false;
            this.chkSample.Location = new System.Drawing.Point(176, 231);
            this.chkSample.Name = "chkSample";
            this.chkSample.Size = new System.Drawing.Size(74, 21);
            this.chkSample.TabIndex = 152;
            this.chkSample.Text = "Sample";
            this.chkSample.UseVisualStyleBackColor = true;
            // 
            // chkBulk
            // 
            this.chkBulk.AutoSize = true;
            this.chkBulk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkBulk.IsSupportEditMode = false;
            this.chkBulk.Location = new System.Drawing.Point(116, 231);
            this.chkBulk.Name = "chkBulk";
            this.chkBulk.Size = new System.Drawing.Size(54, 21);
            this.chkBulk.TabIndex = 151;
            this.chkBulk.Text = "Bulk";
            this.chkBulk.UseVisualStyleBackColor = true;
            // 
            // chkForecast
            // 
            this.chkForecast.AutoSize = true;
            this.chkForecast.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkForecast.IsSupportEditMode = false;
            this.chkForecast.Location = new System.Drawing.Point(256, 231);
            this.chkForecast.Name = "chkForecast";
            this.chkForecast.Size = new System.Drawing.Size(82, 21);
            this.chkForecast.TabIndex = 153;
            this.chkForecast.Text = "Forecast";
            this.chkForecast.UseVisualStyleBackColor = true;
            // 
            // lbCategory
            // 
            this.lbCategory.Location = new System.Drawing.Point(9, 229);
            this.lbCategory.Name = "lbCategory";
            this.lbCategory.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lbCategory.RectStyle.BorderWidth = 1F;
            this.lbCategory.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbCategory.RectStyle.ExtBorderWidth = 1F;
            this.lbCategory.Size = new System.Drawing.Size(104, 23);
            this.lbCategory.TabIndex = 154;
            this.lbCategory.Text = "Category";
            this.lbCategory.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lbCategory.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // R08
            // 
            this.ClientSize = new System.Drawing.Size(511, 290);
            this.Controls.Add(this.lbCategory);
            this.Controls.Add(this.chkForecast);
            this.Controls.Add(this.chkSample);
            this.Controls.Add(this.chkBulk);
            this.Controls.Add(this.txtseason);
            this.Controls.Add(this.lbSeason);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.lbFactory);
            this.Controls.Add(this.comboMDivision);
            this.Controls.Add(this.lbM);
            this.Controls.Add(this.lbOperation);
            this.Controls.Add(this.txtmulitOperation1);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.lbBuyerDelivery);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.lbBrand);
            this.DefaultControl = "txtFactory";
            this.DefaultControlForEdit = "txtFactory";
            this.IsSupportToPrint = false;
            this.Name = "R08";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R08. Order Analysis Report";
            this.Controls.SetChildIndex(this.lbBrand, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.lbBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtmulitOperation1, 0);
            this.Controls.SetChildIndex(this.lbOperation, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.lbM, 0);
            this.Controls.SetChildIndex(this.comboMDivision, 0);
            this.Controls.SetChildIndex(this.lbFactory, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.lbSeason, 0);
            this.Controls.SetChildIndex(this.txtseason, 0);
            this.Controls.SetChildIndex(this.chkBulk, 0);
            this.Controls.SetChildIndex(this.chkSample, 0);
            this.Controls.SetChildIndex(this.chkForecast, 0);
            this.Controls.SetChildIndex(this.lbCategory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.Label lbBrand;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Label lbBuyerDelivery;
        private Class.Txtbrand txtbrand;
        private Class.TxtmulitOperation txtmulitOperation1;
        private Win.UI.Label lbOperation;
        private Class.ComboMDivision comboMDivision;
        private Win.UI.Label lbM;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.Label lbFactory;
        private Class.Txtseason txtseason;
        private Win.UI.Label lbSeason;
        private Win.UI.CheckBox chkSample;
        private Win.UI.CheckBox chkBulk;
        private Win.UI.CheckBox chkForecast;
        private Win.UI.Label lbCategory;
    }
}
