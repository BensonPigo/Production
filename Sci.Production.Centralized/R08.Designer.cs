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
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.comboFty = new Sci.Production.Class.ComboCentralizedFactory(this.components);
            this.txtStyle = new Sci.Win.UI.TextBox();
            this.txtSeason = new Sci.Win.UI.TextBox();
            this.txtBrand = new Sci.Win.UI.TextBox();
            this.txtLine = new Sci.Win.UI.TextBox();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.labelCategory = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(478, 12);
            this.print.TabIndex = 4;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(478, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(478, 84);
            this.close.TabIndex = 6;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(368, 145);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(394, 154);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(368, 165);
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
            this.comboM.Location = new System.Drawing.Point(151, 151);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(80, 24);
            this.comboM.TabIndex = 152;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 151);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(135, 23);
            this.labelM.TabIndex = 151;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 185);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(134, 23);
            this.labelFactory.TabIndex = 153;
            this.labelFactory.Text = "Factory";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(13, 251);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(134, 23);
            this.labelSeason.TabIndex = 158;
            this.labelSeason.Text = "Season";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(13, 284);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(134, 23);
            this.labelStyle.TabIndex = 157;
            this.labelStyle.Text = "Style";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 217);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 23);
            this.label3.TabIndex = 246;
            this.label3.Text = "Brand";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(13, 321);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 23);
            this.label4.TabIndex = 248;
            this.label4.Text = "Line";
            // 
            // comboFty
            // 
            this.comboFty.BackColor = System.Drawing.Color.White;
            this.comboFty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFty.FormattingEnabled = true;
            this.comboFty.IsSupportUnselect = true;
            this.comboFty.Location = new System.Drawing.Point(151, 184);
            this.comboFty.Name = "comboFty";
            this.comboFty.OldText = "";
            this.comboFty.Size = new System.Drawing.Size(80, 24);
            this.comboFty.TabIndex = 254;
            this.comboFty.SelectedValueChanged += new System.EventHandler(this.ComboFty_SelectedValueChanged);
            // 
            // txtStyle
            // 
            this.txtStyle.BackColor = System.Drawing.Color.White;
            this.txtStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStyle.Location = new System.Drawing.Point(150, 284);
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.Size = new System.Drawing.Size(100, 23);
            this.txtStyle.TabIndex = 256;
            this.txtStyle.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtStyle_PopUp);
            this.txtStyle.Validating += new System.ComponentModel.CancelEventHandler(this.TxtStyle_Validating);
            // 
            // txtSeason
            // 
            this.txtSeason.BackColor = System.Drawing.Color.White;
            this.txtSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeason.Location = new System.Drawing.Point(151, 251);
            this.txtSeason.Name = "txtSeason";
            this.txtSeason.Size = new System.Drawing.Size(100, 23);
            this.txtSeason.TabIndex = 257;
            this.txtSeason.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSeason_PopUp);
            this.txtSeason.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSeason_Validating);
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(151, 217);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(100, 23);
            this.txtBrand.TabIndex = 258;
            this.txtBrand.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtBrand_PopUp);
            this.txtBrand.Validating += new System.ComponentModel.CancelEventHandler(this.TxtBrand_Validating);
            // 
            // txtLine
            // 
            this.txtLine.BackColor = System.Drawing.Color.White;
            this.txtLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLine.Location = new System.Drawing.Point(151, 321);
            this.txtLine.Name = "txtLine";
            this.txtLine.Size = new System.Drawing.Size(100, 23);
            this.txtLine.TabIndex = 259;
            this.txtLine.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtLine_PopUp);
            this.txtLine.Validating += new System.ComponentModel.CancelEventHandler(this.TxtLine_Validating);
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(150, 116);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(170, 24);
            this.comboCategory.TabIndex = 260;
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(13, 117);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(135, 23);
            this.labelCategory.TabIndex = 261;
            this.labelCategory.Text = "Category";
            // 
            // R08
            // 
            this.ClientSize = new System.Drawing.Size(570, 380);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.txtLine);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.txtSeason);
            this.Controls.Add(this.txtStyle);
            this.Controls.Add(this.comboFty);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
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
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.comboFty, 0);
            this.Controls.SetChildIndex(this.txtStyle, 0);
            this.Controls.SetChildIndex(this.txtSeason, 0);
            this.Controls.SetChildIndex(this.txtBrand, 0);
            this.Controls.SetChildIndex(this.txtLine, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbOutputDate;
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
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Class.ComboCentralizedFactory comboFty;
        private Win.UI.TextBox txtStyle;
        private Win.UI.TextBox txtSeason;
        private Win.UI.TextBox txtBrand;
        private Win.UI.TextBox txtLine;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.Label labelCategory;
    }
}
