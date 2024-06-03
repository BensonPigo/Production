namespace Sci.Production.Centralized
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
            this.lbOutputDate = new Sci.Win.UI.Label();
            this.lbFormat = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.dtInlineDate = new Sci.Win.UI.DateRange();
            this.dtOutputDate = new Sci.Win.UI.DateRange();
            this.dtSewingDate = new Sci.Win.UI.DateRange();
            this.comboM = new Sci.Production.Class.ComboCentralizedM(this.components);
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.comboPhase = new Sci.Win.UI.ComboBox();
            this.lbPhase = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.label4 = new Sci.Win.UI.Label();
            this.txtsewingline = new Sci.Production.Class.Txtsewingline();
            this.comboReportType = new Sci.Win.UI.ComboBox();
            this.label5 = new Sci.Win.UI.Label();
            this.chkLatestVersion = new Sci.Win.UI.CheckBox();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.comboFty = new Sci.Production.Class.ComboCentralizedFactory(this.components);
            this.txtSeason = new Sci.Production.Class.Txtseason();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(463, 12);
            this.print.TabIndex = 4;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(463, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(463, 84);
            this.close.TabIndex = 6;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(338, 313);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(364, 322);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(338, 333);
            // 
            // lbOutputDate
            // 
            this.lbOutputDate.Location = new System.Drawing.Point(13, 12);
            this.lbOutputDate.Name = "lbOutputDate";
            this.lbOutputDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lbOutputDate.RectStyle.BorderWidth = 1F;
            this.lbOutputDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbOutputDate.RectStyle.ExtBorderWidth = 1F;
            this.lbOutputDate.Size = new System.Drawing.Size(134, 23);
            this.lbOutputDate.TabIndex = 96;
            this.lbOutputDate.Text = "Sewing Output Date";
            this.lbOutputDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lbOutputDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lbFormat
            // 
            this.lbFormat.Location = new System.Drawing.Point(13, 356);
            this.lbFormat.Name = "lbFormat";
            this.lbFormat.Size = new System.Drawing.Size(134, 23);
            this.lbFormat.TabIndex = 143;
            this.lbFormat.Text = "Report Type";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 48);
            this.label1.Name = "label1";
            this.label1.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.RectStyle.BorderWidth = 1F;
            this.label1.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label1.RectStyle.ExtBorderWidth = 1F;
            this.label1.Size = new System.Drawing.Size(134, 23);
            this.label1.TabIndex = 146;
            this.label1.Text = "Inline Date";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 84);
            this.label2.Name = "label2";
            this.label2.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.RectStyle.BorderWidth = 1F;
            this.label2.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label2.RectStyle.ExtBorderWidth = 1F;
            this.label2.Size = new System.Drawing.Size(134, 23);
            this.label2.TabIndex = 147;
            this.label2.Text = "Sewing Date";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dtInlineDate
            // 
            // 
            // 
            // 
            this.dtInlineDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dtInlineDate.DateBox1.Name = "";
            this.dtInlineDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dtInlineDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dtInlineDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dtInlineDate.DateBox2.Name = "";
            this.dtInlineDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dtInlineDate.DateBox2.TabIndex = 1;
            this.dtInlineDate.Location = new System.Drawing.Point(150, 48);
            this.dtInlineDate.Name = "dtInlineDate";
            this.dtInlineDate.Size = new System.Drawing.Size(280, 23);
            this.dtInlineDate.TabIndex = 149;
            // 
            // dtOutputDate
            // 
            // 
            // 
            // 
            this.dtOutputDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dtOutputDate.DateBox1.Name = "";
            this.dtOutputDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dtOutputDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dtOutputDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dtOutputDate.DateBox2.Name = "";
            this.dtOutputDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dtOutputDate.DateBox2.TabIndex = 1;
            this.dtOutputDate.IsRequired = false;
            this.dtOutputDate.Location = new System.Drawing.Point(150, 12);
            this.dtOutputDate.Name = "dtOutputDate";
            this.dtOutputDate.Size = new System.Drawing.Size(280, 23);
            this.dtOutputDate.TabIndex = 148;
            // 
            // dtSewingDate
            // 
            // 
            // 
            // 
            this.dtSewingDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dtSewingDate.DateBox1.Name = "";
            this.dtSewingDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dtSewingDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dtSewingDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dtSewingDate.DateBox2.Name = "";
            this.dtSewingDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dtSewingDate.DateBox2.TabIndex = 1;
            this.dtSewingDate.Location = new System.Drawing.Point(150, 84);
            this.dtSewingDate.Name = "dtSewingDate";
            this.dtSewingDate.Size = new System.Drawing.Size(280, 23);
            this.dtSewingDate.TabIndex = 150;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(151, 118);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(80, 24);
            this.comboM.TabIndex = 152;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 118);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(135, 23);
            this.labelM.TabIndex = 151;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 152);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(134, 23);
            this.labelFactory.TabIndex = 153;
            this.labelFactory.Text = "Factory";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(13, 218);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(134, 23);
            this.labelSeason.TabIndex = 158;
            this.labelSeason.Text = "Season";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(13, 185);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(134, 23);
            this.labelStyle.TabIndex = 157;
            this.labelStyle.Text = "Style";
            // 
            // comboPhase
            // 
            this.comboPhase.BackColor = System.Drawing.Color.White;
            this.comboPhase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboPhase.FormattingEnabled = true;
            this.comboPhase.IsSupportUnselect = true;
            this.comboPhase.Location = new System.Drawing.Point(150, 285);
            this.comboPhase.Name = "comboPhase";
            this.comboPhase.OldText = "";
            this.comboPhase.Size = new System.Drawing.Size(88, 24);
            this.comboPhase.TabIndex = 245;
            // 
            // lbPhase
            // 
            this.lbPhase.Location = new System.Drawing.Point(13, 285);
            this.lbPhase.Name = "lbPhase";
            this.lbPhase.Size = new System.Drawing.Size(134, 23);
            this.lbPhase.TabIndex = 244;
            this.lbPhase.Text = "Phase";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 252);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 23);
            this.label3.TabIndex = 246;
            this.label3.Text = "Brand";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(150, 252);
            this.txtbrand.MyDocumentdName = null;
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(300, 23);
            this.txtbrand.TabIndex = 247;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(13, 320);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 23);
            this.label4.TabIndex = 248;
            this.label4.Text = "Line";
            // 
            // txtsewingline
            // 
            this.txtsewingline.BackColor = System.Drawing.Color.White;
            this.txtsewingline.FactoryobjectName = null;
            this.txtsewingline.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtsewingline.Location = new System.Drawing.Point(151, 320);
            this.txtsewingline.Name = "txtsewingline";
            this.txtsewingline.Size = new System.Drawing.Size(60, 23);
            this.txtsewingline.TabIndex = 249;
            // 
            // comboReportType
            // 
            this.comboReportType.BackColor = System.Drawing.Color.White;
            this.comboReportType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboReportType.FormattingEnabled = true;
            this.comboReportType.IsSupportUnselect = true;
            this.comboReportType.Location = new System.Drawing.Point(151, 356);
            this.comboReportType.Name = "comboReportType";
            this.comboReportType.OldText = "";
            this.comboReportType.Size = new System.Drawing.Size(118, 24);
            this.comboReportType.TabIndex = 250;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(13, 391);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 23);
            this.label5.TabIndex = 251;
            this.label5.Text = "Version";
            // 
            // chkLatestVersion
            // 
            this.chkLatestVersion.AutoSize = true;
            this.chkLatestVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkLatestVersion.Location = new System.Drawing.Point(151, 393);
            this.chkLatestVersion.Name = "chkLatestVersion";
            this.chkLatestVersion.Size = new System.Drawing.Size(118, 21);
            this.chkLatestVersion.TabIndex = 252;
            this.chkLatestVersion.Text = "Latest Version";
            this.chkLatestVersion.UseVisualStyleBackColor = true;
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = this.txtbrand;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(150, 185);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.SeasonObjectName = null;
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 253;
            this.txtstyle.TarBrand = null;
            this.txtstyle.TarSeason = null;
            // 
            // comboFty
            // 
            this.comboFty.BackColor = System.Drawing.Color.White;
            this.comboFty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFty.FormattingEnabled = true;
            this.comboFty.IsSupportUnselect = true;
            this.comboFty.Location = new System.Drawing.Point(151, 151);
            this.comboFty.Name = "comboFty";
            this.comboFty.OldText = "";
            this.comboFty.Size = new System.Drawing.Size(80, 24);
            this.comboFty.TabIndex = 254;
            // 
            // txtSeason
            // 
            this.txtSeason.BackColor = System.Drawing.Color.White;
            this.txtSeason.BrandObjectName = null;
            this.txtSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeason.Location = new System.Drawing.Point(150, 218);
            this.txtSeason.Name = "txtSeason";
            this.txtSeason.Size = new System.Drawing.Size(226, 23);
            this.txtSeason.TabIndex = 255;
            // 
            // R08
            // 
            this.ClientSize = new System.Drawing.Size(572, 462);
            this.Controls.Add(this.txtSeason);
            this.Controls.Add(this.comboFty);
            this.Controls.Add(this.txtstyle);
            this.Controls.Add(this.chkLatestVersion);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboReportType);
            this.Controls.Add(this.txtsewingline);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboPhase);
            this.Controls.Add(this.lbPhase);
            this.Controls.Add(this.labelSeason);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.dtSewingDate);
            this.Controls.Add(this.dtInlineDate);
            this.Controls.Add(this.dtOutputDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbFormat);
            this.Controls.Add(this.lbOutputDate);
            this.DefaultControl = "dateOutputDate";
            this.DefaultControlForEdit = "dateOutputDate";
            this.IsSupportToPrint = false;
            this.Name = "R08";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R08. Centralized Line Mapping List";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.lbOutputDate, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.lbFormat, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.dtOutputDate, 0);
            this.Controls.SetChildIndex(this.dtInlineDate, 0);
            this.Controls.SetChildIndex(this.dtSewingDate, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.labelSeason, 0);
            this.Controls.SetChildIndex(this.lbPhase, 0);
            this.Controls.SetChildIndex(this.comboPhase, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txtsewingline, 0);
            this.Controls.SetChildIndex(this.comboReportType, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.chkLatestVersion, 0);
            this.Controls.SetChildIndex(this.txtstyle, 0);
            this.Controls.SetChildIndex(this.comboFty, 0);
            this.Controls.SetChildIndex(this.txtSeason, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbOutputDate;
        private Win.UI.Label lbFormat;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.DateRange dtInlineDate;
        private Win.UI.DateRange dtOutputDate;
        private Win.UI.DateRange dtSewingDate;
        private Class.ComboCentralizedM comboM;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelStyle;
        private Win.UI.ComboBox comboPhase;
        private Win.UI.Label lbPhase;
        private Win.UI.Label label3;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label label4;
        private Class.Txtsewingline txtsewingline;
        private Win.UI.ComboBox comboReportType;
        private Win.UI.Label label5;
        private Win.UI.CheckBox chkLatestVersion;
        private Class.Txtstyle txtstyle;
        private Class.ComboCentralizedFactory comboFty;
        private Class.Txtseason txtSeason;
    }
}
