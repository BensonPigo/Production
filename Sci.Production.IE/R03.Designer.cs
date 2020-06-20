namespace Sci.Production.IE
{
    partial class R03
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
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.lbToolType = new Sci.Win.UI.Label();
            this.comboToolType = new Sci.Win.UI.ComboBox();
            this.dateInlineDate = new Sci.Win.UI.DateRange();
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.txtseason = new Sci.Production.Class.txtseason();
            this.txtstyle = new Sci.Production.Class.txtstyle();
            this.comboVersion = new Sci.Win.UI.ComboBox();
            this.lbVersion = new Sci.Win.UI.Label();
            this.lbInlineDate = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(449, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(449, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(449, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(403, 122);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(417, 128);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(429, 131);
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(9, 48);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(75, 23);
            this.labelFactory.TabIndex = 94;
            this.labelFactory.Text = "Factory";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(9, 83);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(75, 23);
            this.labelStyle.TabIndex = 95;
            this.labelStyle.Text = "Style#";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(9, 119);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(75, 23);
            this.labelSeason.TabIndex = 96;
            this.labelSeason.Text = "Season";
            // 
            // lbToolType
            // 
            this.lbToolType.Location = new System.Drawing.Point(9, 155);
            this.lbToolType.Name = "lbToolType";
            this.lbToolType.Size = new System.Drawing.Size(75, 23);
            this.lbToolType.TabIndex = 97;
            this.lbToolType.Text = "Tool Type";
            // 
            // comboToolType
            // 
            this.comboToolType.BackColor = System.Drawing.Color.White;
            this.comboToolType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboToolType.FormattingEnabled = true;
            this.comboToolType.IsSupportUnselect = true;
            this.comboToolType.Location = new System.Drawing.Point(87, 155);
            this.comboToolType.Name = "comboToolType";
            this.comboToolType.OldText = "";
            this.comboToolType.Size = new System.Drawing.Size(130, 24);
            this.comboToolType.TabIndex = 5;
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
            this.dateInlineDate.Location = new System.Drawing.Point(87, 12);
            this.dateInlineDate.Name = "dateInlineDate";
            this.dateInlineDate.Size = new System.Drawing.Size(280, 23);
            this.dateInlineDate.TabIndex = 1;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.boolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(87, 48);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(80, 23);
            this.txtfactory.TabIndex = 2;
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(87, 119);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(80, 23);
            this.txtseason.TabIndex = 4;
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(87, 83);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 3;
            this.txtstyle.tarBrand = null;
            this.txtstyle.tarSeason = null;
            // 
            // comboVersion
            // 
            this.comboVersion.BackColor = System.Drawing.Color.White;
            this.comboVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboVersion.FormattingEnabled = true;
            this.comboVersion.IsSupportUnselect = true;
            this.comboVersion.Location = new System.Drawing.Point(87, 193);
            this.comboVersion.Name = "comboVersion";
            this.comboVersion.OldText = "";
            this.comboVersion.Size = new System.Drawing.Size(130, 24);
            this.comboVersion.TabIndex = 6;
            // 
            // lbVersion
            // 
            this.lbVersion.Location = new System.Drawing.Point(9, 193);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(75, 23);
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
            this.lbInlineDate.Size = new System.Drawing.Size(75, 23);
            this.lbInlineDate.TabIndex = 105;
            this.lbInlineDate.Text = "Inline Date";
            this.lbInlineDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lbInlineDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // R03
            // 
            this.ClientSize = new System.Drawing.Size(541, 255);
            this.Controls.Add(this.lbInlineDate);
            this.Controls.Add(this.comboVersion);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.txtstyle);
            this.Controls.Add(this.txtseason);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.dateInlineDate);
            this.Controls.Add(this.comboToolType);
            this.Controls.Add(this.lbToolType);
            this.Controls.Add(this.labelSeason);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.labelFactory);
            this.DefaultControl = "txtFactory";
            this.DefaultControlForEdit = "txtFactory";
            this.IsSupportToPrint = false;
            this.Name = "R03";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R03. Tool Usage Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.labelSeason, 0);
            this.Controls.SetChildIndex(this.lbToolType, 0);
            this.Controls.SetChildIndex(this.comboToolType, 0);
            this.Controls.SetChildIndex(this.dateInlineDate, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtseason, 0);
            this.Controls.SetChildIndex(this.txtstyle, 0);
            this.Controls.SetChildIndex(this.lbVersion, 0);
            this.Controls.SetChildIndex(this.comboVersion, 0);
            this.Controls.SetChildIndex(this.lbInlineDate, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelFactory;
        private Win.UI.Label labelStyle;
        private Win.UI.Label labelSeason;
        private Win.UI.Label lbToolType;
        private Win.UI.ComboBox comboToolType;
        private Win.UI.DateRange dateInlineDate;
        private Class.txtfactory txtfactory;
        private Class.txtseason txtseason;
        private Class.txtstyle txtstyle;
        private Win.UI.ComboBox comboVersion;
        private Win.UI.Label lbVersion;
        private Win.UI.Label lbInlineDate;
    }
}
