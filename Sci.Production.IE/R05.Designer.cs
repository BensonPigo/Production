namespace Sci.Production.IE
{
    partial class R05
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
            this.dateDate = new Sci.Win.UI.DateRange();
            this.lbVersion = new Sci.Win.UI.Label();
            this.lbDate = new Sci.Win.UI.Label();
            this.chkLatestVersion = new Sci.Win.UI.CheckBox();
            this.txtmultiSeason1 = new Sci.Production.Class.TxtmultiSeason();
            this.txtbrand1 = new Sci.Production.Class.Txtbrand();
            this.txtfactory1 = new Sci.Production.Class.Txtfactory();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioIsHide = new Sci.Win.UI.RadioButton();
            this.radioISPPA = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(486, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(486, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(486, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(451, 148);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(477, 119);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(463, 121);
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(9, 155);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(116, 23);
            this.labelFactory.TabIndex = 94;
            this.labelFactory.Text = "Factory";
            // 
            // lbBrand
            // 
            this.lbBrand.Location = new System.Drawing.Point(9, 84);
            this.lbBrand.Name = "lbBrand";
            this.lbBrand.Size = new System.Drawing.Size(116, 23);
            this.lbBrand.TabIndex = 95;
            this.lbBrand.Text = "Brand";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(9, 119);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(116, 23);
            this.labelSeason.TabIndex = 96;
            this.labelSeason.Text = "Season";
            // 
            // dateDate
            // 
            // 
            // 
            // 
            this.dateDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateDate.DateBox1.Name = "";
            this.dateDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateDate.DateBox2.Name = "";
            this.dateDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateDate.DateBox2.TabIndex = 1;
            this.dateDate.IsRequired = false;
            this.dateDate.Location = new System.Drawing.Point(128, 48);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(280, 23);
            this.dateDate.TabIndex = 1;
            // 
            // lbVersion
            // 
            this.lbVersion.Location = new System.Drawing.Point(9, 192);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(116, 23);
            this.lbVersion.TabIndex = 104;
            this.lbVersion.Text = "Version";
            // 
            // lbDate
            // 
            this.lbDate.Location = new System.Drawing.Point(9, 48);
            this.lbDate.Name = "lbDate";
            this.lbDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lbDate.RectStyle.BorderWidth = 1F;
            this.lbDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbDate.RectStyle.ExtBorderWidth = 1F;
            this.lbDate.Size = new System.Drawing.Size(116, 23);
            this.lbDate.TabIndex = 105;
            this.lbDate.Text = "Create/ Edit date";
            this.lbDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lbDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // chkLatestVersion
            // 
            this.chkLatestVersion.AutoSize = true;
            this.chkLatestVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkLatestVersion.Location = new System.Drawing.Point(128, 192);
            this.chkLatestVersion.Name = "chkLatestVersion";
            this.chkLatestVersion.Size = new System.Drawing.Size(118, 21);
            this.chkLatestVersion.TabIndex = 5;
            this.chkLatestVersion.Text = "Latest Version";
            this.chkLatestVersion.UseVisualStyleBackColor = true;
            // 
            // txtmultiSeason1
            // 
            this.txtmultiSeason1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtmultiSeason1.BrandObjectName = this.txtbrand1;
            this.txtmultiSeason1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtmultiSeason1.IsSupportEditMode = false;
            this.txtmultiSeason1.Location = new System.Drawing.Point(128, 119);
            this.txtmultiSeason1.Name = "txtmultiSeason1";
            this.txtmultiSeason1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtmultiSeason1.ReadOnly = true;
            this.txtmultiSeason1.Size = new System.Drawing.Size(280, 23);
            this.txtmultiSeason1.TabIndex = 3;
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(128, 84);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(91, 23);
            this.txtbrand1.TabIndex = 2;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.BoolFtyGroupList = true;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IsProduceFty = false;
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(128, 152);
            this.txtfactory1.MDivision = null;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(91, 23);
            this.txtfactory1.TabIndex = 4;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioIsHide);
            this.radioPanel1.Controls.Add(this.radioISPPA);
            this.radioPanel1.Location = new System.Drawing.Point(12, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(192, 25);
            this.radioPanel1.TabIndex = 228;
            // 
            // radioIsHide
            // 
            this.radioIsHide.AutoSize = true;
            this.radioIsHide.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioIsHide.Location = new System.Drawing.Point(94, 3);
            this.radioIsHide.Name = "radioIsHide";
            this.radioIsHide.Size = new System.Drawing.Size(69, 21);
            this.radioIsHide.TabIndex = 7;
            this.radioIsHide.Text = "Is Hide";
            this.radioIsHide.UseVisualStyleBackColor = true;
            // 
            // radioISPPA
            // 
            this.radioISPPA.AutoSize = true;
            this.radioISPPA.Checked = true;
            this.radioISPPA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioISPPA.Location = new System.Drawing.Point(3, 3);
            this.radioISPPA.Name = "radioISPPA";
            this.radioISPPA.Size = new System.Drawing.Size(67, 21);
            this.radioISPPA.TabIndex = 6;
            this.radioISPPA.TabStop = true;
            this.radioISPPA.Text = "Is PPA";
            this.radioISPPA.UseVisualStyleBackColor = true;
            // 
            // R05
            // 
            this.ClientSize = new System.Drawing.Size(578, 250);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.chkLatestVersion);
            this.Controls.Add(this.txtmultiSeason1);
            this.Controls.Add(this.txtfactory1);
            this.Controls.Add(this.txtbrand1);
            this.Controls.Add(this.lbDate);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.dateDate);
            this.Controls.Add(this.labelSeason);
            this.Controls.Add(this.lbBrand);
            this.Controls.Add(this.labelFactory);
            this.DefaultControl = "txtFactory";
            this.DefaultControlForEdit = "txtFactory";
            this.IsSupportToPrint = false;
            this.Name = "R05";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R05. Non-sewing Line Operation Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.lbBrand, 0);
            this.Controls.SetChildIndex(this.labelSeason, 0);
            this.Controls.SetChildIndex(this.dateDate, 0);
            this.Controls.SetChildIndex(this.lbVersion, 0);
            this.Controls.SetChildIndex(this.lbDate, 0);
            this.Controls.SetChildIndex(this.txtbrand1, 0);
            this.Controls.SetChildIndex(this.txtfactory1, 0);
            this.Controls.SetChildIndex(this.txtmultiSeason1, 0);
            this.Controls.SetChildIndex(this.chkLatestVersion, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelFactory;
        private Win.UI.Label lbBrand;
        private Win.UI.Label labelSeason;
        private Win.UI.DateRange dateDate;
        private Win.UI.Label lbVersion;
        private Win.UI.Label lbDate;
        private Class.Txtbrand txtbrand1;
        private Class.Txtfactory txtfactory1;
        private Class.TxtmultiSeason txtmultiSeason1;
        private Win.UI.CheckBox chkLatestVersion;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioIsHide;
        private Win.UI.RadioButton radioISPPA;
    }
}
