namespace Sci.Production.IE
{
    partial class R04
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
            this.labelFactory = new Sci.Win.UI.Label();
            this.lbBrand = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.dateInlineDate = new Sci.Win.UI.DateRange();
            this.lbVersion = new Sci.Win.UI.Label();
            this.lbInlineDate = new Sci.Win.UI.Label();
            this.lbOperationCode = new Sci.Win.UI.Label();
            this.chkLatestVersion = new Sci.Win.UI.CheckBox();
            this.txtmultiSeason1 = new Sci.Production.Class.TxtmultiSeason();
            this.txtbrand1 = new Sci.Production.Class.Txtbrand();
            this.txtfactory1 = new Sci.Production.Class.Txtfactory();
            this.txtmulitOperation1 = new Sci.Production.Class.TxtmulitOperation();
            this.txtmulitMachineType1 = new Sci.Production.Class.TxtmulitMachineType();
            this.lbMC = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(419, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(419, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(419, 84);
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
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(9, 120);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(104, 23);
            this.labelFactory.TabIndex = 94;
            this.labelFactory.Text = "Factory";
            // 
            // lbBrand
            // 
            this.lbBrand.Location = new System.Drawing.Point(9, 156);
            this.lbBrand.Name = "lbBrand";
            this.lbBrand.Size = new System.Drawing.Size(104, 23);
            this.lbBrand.TabIndex = 95;
            this.lbBrand.Text = "Brand";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(9, 192);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(104, 23);
            this.labelSeason.TabIndex = 96;
            this.labelSeason.Text = "Season";
            // 
            // dateInlineDate
            // 
            // 
            // 
            // 
            this.dateInlineDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInlineDate.DateBox1.Name = "";
            this.dateInlineDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateInlineDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInlineDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateInlineDate.DateBox2.Name = "";
            this.dateInlineDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateInlineDate.DateBox2.TabIndex = 1;
            this.dateInlineDate.IsRequired = false;
            this.dateInlineDate.Location = new System.Drawing.Point(116, 12);
            this.dateInlineDate.Name = "dateInlineDate";
            this.dateInlineDate.Size = new System.Drawing.Size(280, 23);
            this.dateInlineDate.TabIndex = 1;
            // 
            // lbVersion
            // 
            this.lbVersion.Location = new System.Drawing.Point(9, 228);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(104, 23);
            this.lbVersion.TabIndex = 104;
            this.lbVersion.Text = "Version";
            // 
            // lbInlineDate
            // 
            this.lbInlineDate.Location = new System.Drawing.Point(9, 12);
            this.lbInlineDate.Name = "lbInlineDate";
            this.lbInlineDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lbInlineDate.RectStyle.BorderWidth = 1F;
            this.lbInlineDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbInlineDate.RectStyle.ExtBorderWidth = 1F;
            this.lbInlineDate.Size = new System.Drawing.Size(104, 23);
            this.lbInlineDate.TabIndex = 105;
            this.lbInlineDate.Text = "Inline Date";
            this.lbInlineDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lbInlineDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lbOperationCode
            // 
            this.lbOperationCode.Location = new System.Drawing.Point(9, 84);
            this.lbOperationCode.Name = "lbOperationCode";
            this.lbOperationCode.Size = new System.Drawing.Size(104, 23);
            this.lbOperationCode.TabIndex = 107;
            this.lbOperationCode.Text = "Operation code";
            // 
            // chkLatestVersion
            // 
            this.chkLatestVersion.AutoSize = true;
            this.chkLatestVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkLatestVersion.Location = new System.Drawing.Point(116, 230);
            this.chkLatestVersion.Name = "chkLatestVersion";
            this.chkLatestVersion.Size = new System.Drawing.Size(118, 21);
            this.chkLatestVersion.TabIndex = 165;
            this.chkLatestVersion.Text = "Latest Version";
            this.chkLatestVersion.UseVisualStyleBackColor = true;
            // 
            // txtmultiSeason1
            // 
            this.txtmultiSeason1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtmultiSeason1.BrandObjectName = this.txtbrand1;
            this.txtmultiSeason1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtmultiSeason1.IsSupportEditMode = false;
            this.txtmultiSeason1.Location = new System.Drawing.Point(116, 192);
            this.txtmultiSeason1.Name = "txtmultiSeason1";
            this.txtmultiSeason1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtmultiSeason1.ReadOnly = true;
            this.txtmultiSeason1.Size = new System.Drawing.Size(280, 23);
            this.txtmultiSeason1.TabIndex = 112;
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(116, 155);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(91, 23);
            this.txtbrand1.TabIndex = 108;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.BoolFtyGroupList = true;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IsProduceFty = false;
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(116, 119);
            this.txtfactory1.MDivision = null;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(91, 23);
            this.txtfactory1.TabIndex = 111;
            // 
            // txtmulitOperation1
            // 
            this.txtmulitOperation1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtmulitOperation1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtmulitOperation1.IsJunk = false;
            this.txtmulitOperation1.IsSupportEditMode = false;
            this.txtmulitOperation1.Location = new System.Drawing.Point(116, 84);
            this.txtmulitOperation1.Name = "txtmulitOperation1";
            this.txtmulitOperation1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtmulitOperation1.ReadOnly = true;
            this.txtmulitOperation1.Size = new System.Drawing.Size(280, 23);
            this.txtmulitOperation1.TabIndex = 110;
            // 
            // txtmulitMachineType1
            // 
            this.txtmulitMachineType1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtmulitMachineType1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtmulitMachineType1.IsJunk = false;
            this.txtmulitMachineType1.IsSupportEditMode = false;
            this.txtmulitMachineType1.Location = new System.Drawing.Point(116, 48);
            this.txtmulitMachineType1.Name = "txtmulitMachineType1";
            this.txtmulitMachineType1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtmulitMachineType1.ReadOnly = true;
            this.txtmulitMachineType1.Size = new System.Drawing.Size(280, 23);
            this.txtmulitMachineType1.TabIndex = 109;
            // 
            // lbMC
            // 
            this.lbMC.Location = new System.Drawing.Point(9, 48);
            this.lbMC.Name = "lbMC";
            this.lbMC.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lbMC.RectStyle.BorderWidth = 1F;
            this.lbMC.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbMC.RectStyle.ExtBorderWidth = 1F;
            this.lbMC.Size = new System.Drawing.Size(104, 23);
            this.lbMC.TabIndex = 166;
            this.lbMC.Text = "M/C";
            this.lbMC.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lbMC.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // R04
            // 
            this.ClientSize = new System.Drawing.Size(511, 293);
            this.Controls.Add(this.lbMC);
            this.Controls.Add(this.chkLatestVersion);
            this.Controls.Add(this.txtmultiSeason1);
            this.Controls.Add(this.txtfactory1);
            this.Controls.Add(this.txtmulitOperation1);
            this.Controls.Add(this.txtmulitMachineType1);
            this.Controls.Add(this.txtbrand1);
            this.Controls.Add(this.lbOperationCode);
            this.Controls.Add(this.lbInlineDate);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.dateInlineDate);
            this.Controls.Add(this.labelSeason);
            this.Controls.Add(this.lbBrand);
            this.Controls.Add(this.labelFactory);
            this.DefaultControl = "txtFactory";
            this.DefaultControlForEdit = "txtFactory";
            this.IsSupportToPrint = false;
            this.Name = "R04";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R04. Operation GSD Time vs. CT Time Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.lbBrand, 0);
            this.Controls.SetChildIndex(this.labelSeason, 0);
            this.Controls.SetChildIndex(this.dateInlineDate, 0);
            this.Controls.SetChildIndex(this.lbVersion, 0);
            this.Controls.SetChildIndex(this.lbInlineDate, 0);
            this.Controls.SetChildIndex(this.lbOperationCode, 0);
            this.Controls.SetChildIndex(this.txtbrand1, 0);
            this.Controls.SetChildIndex(this.txtmulitMachineType1, 0);
            this.Controls.SetChildIndex(this.txtmulitOperation1, 0);
            this.Controls.SetChildIndex(this.txtfactory1, 0);
            this.Controls.SetChildIndex(this.txtmultiSeason1, 0);
            this.Controls.SetChildIndex(this.chkLatestVersion, 0);
            this.Controls.SetChildIndex(this.lbMC, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelFactory;
        private Win.UI.Label lbBrand;
        private Win.UI.Label labelSeason;
        private Win.UI.DateRange dateInlineDate;
        private Win.UI.Label lbVersion;
        private Win.UI.Label lbInlineDate;
        private Win.UI.Label lbOperationCode;
        private Class.Txtbrand txtbrand1;
        private Class.TxtmulitMachineType txtmulitMachineType1;
        private Class.TxtmulitOperation txtmulitOperation1;
        private Class.Txtfactory txtfactory1;
        private Class.TxtmultiSeason txtmultiSeason1;
        private Win.UI.CheckBox chkLatestVersion;
        private Win.UI.Label lbMC;
    }
}
