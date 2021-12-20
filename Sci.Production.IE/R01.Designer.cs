namespace Sci.Production.IE
{
    partial class R01
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
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelTeam = new Sci.Win.UI.Label();
            this.txtFactory = new Sci.Win.UI.TextBox();
            this.txtStyle = new Sci.Win.UI.TextBox();
            this.txtSeason = new Sci.Win.UI.TextBox();
            this.dateInlineDate = new Sci.Win.UI.DateRange();
            this.lbReportType = new Sci.Win.UI.Label();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioSummary = new Sci.Win.UI.RadioButton();
            this.radioDetail = new Sci.Win.UI.RadioButton();
            this.chkBalancing = new Sci.Win.UI.CheckBox();
            this.comboSewingTeam1 = new Sci.Production.Class.ComboSewingTeam(this.components);
            this.dateSewingDate = new Sci.Win.UI.DateRange();
            this.lbInlineDate = new Sci.Win.UI.Label();
            this.labelInlineDate = new Sci.Win.UI.Label();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(446, 13);
            this.print.TabIndex = 8;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(446, 49);
            this.toexcel.TabIndex = 9;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(446, 85);
            this.close.TabIndex = 10;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 13);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(86, 23);
            this.labelFactory.TabIndex = 94;
            this.labelFactory.Text = "Factory";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(13, 48);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(86, 23);
            this.labelStyle.TabIndex = 95;
            this.labelStyle.Text = "Style#";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(13, 84);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(86, 23);
            this.labelSeason.TabIndex = 96;
            this.labelSeason.Text = "Season";
            // 
            // labelTeam
            // 
            this.labelTeam.Location = new System.Drawing.Point(13, 120);
            this.labelTeam.Name = "labelTeam";
            this.labelTeam.Size = new System.Drawing.Size(86, 23);
            this.labelTeam.TabIndex = 97;
            this.labelTeam.Text = "Team";
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.Location = new System.Drawing.Point(102, 13);
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(70, 23);
            this.txtFactory.TabIndex = 0;
            this.txtFactory.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtFactory_PopUp);
            this.txtFactory.Validating += new System.ComponentModel.CancelEventHandler(this.TxtFactory_Validating);
            // 
            // txtStyle
            // 
            this.txtStyle.BackColor = System.Drawing.Color.White;
            this.txtStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStyle.Location = new System.Drawing.Point(102, 48);
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.Size = new System.Drawing.Size(150, 23);
            this.txtStyle.TabIndex = 1;
            this.txtStyle.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtStyle_PopUp);
            this.txtStyle.Validating += new System.ComponentModel.CancelEventHandler(this.TxtStyle_Validating);
            // 
            // txtSeason
            // 
            this.txtSeason.BackColor = System.Drawing.Color.White;
            this.txtSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeason.Location = new System.Drawing.Point(102, 84);
            this.txtSeason.Name = "txtSeason";
            this.txtSeason.Size = new System.Drawing.Size(80, 23);
            this.txtSeason.TabIndex = 2;
            this.txtSeason.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSeason_PopUp);
            this.txtSeason.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSeason_Validating);
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
            this.dateInlineDate.Location = new System.Drawing.Point(102, 150);
            this.dateInlineDate.Name = "dateInlineDate";
            this.dateInlineDate.Size = new System.Drawing.Size(280, 23);
            this.dateInlineDate.TabIndex = 4;
            // 
            // lbReportType
            // 
            this.lbReportType.Location = new System.Drawing.Point(14, 220);
            this.lbReportType.Name = "lbReportType";
            this.lbReportType.Size = new System.Drawing.Size(85, 23);
            this.lbReportType.TabIndex = 233;
            this.lbReportType.Text = "Report Type";
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioSummary);
            this.radioPanel1.Controls.Add(this.radioDetail);
            this.radioPanel1.Location = new System.Drawing.Point(102, 220);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(192, 25);
            this.radioPanel1.TabIndex = 6;
            // 
            // radioSummary
            // 
            this.radioSummary.AutoSize = true;
            this.radioSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSummary.Location = new System.Drawing.Point(94, 3);
            this.radioSummary.Name = "radioSummary";
            this.radioSummary.Size = new System.Drawing.Size(85, 21);
            this.radioSummary.TabIndex = 7;
            this.radioSummary.Text = "Summary";
            this.radioSummary.UseVisualStyleBackColor = true;
            // 
            // radioDetail
            // 
            this.radioDetail.AutoSize = true;
            this.radioDetail.Checked = true;
            this.radioDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail.Location = new System.Drawing.Point(3, 3);
            this.radioDetail.Name = "radioDetail";
            this.radioDetail.Size = new System.Drawing.Size(62, 21);
            this.radioDetail.TabIndex = 6;
            this.radioDetail.TabStop = true;
            this.radioDetail.Text = "Detail";
            this.radioDetail.UseVisualStyleBackColor = true;
            // 
            // chkBalancing
            // 
            this.chkBalancing.AutoSize = true;
            this.chkBalancing.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkBalancing.Location = new System.Drawing.Point(102, 254);
            this.chkBalancing.Name = "chkBalancing";
            this.chkBalancing.Size = new System.Drawing.Size(297, 21);
            this.chkBalancing.TabIndex = 7;
            this.chkBalancing.Text = "Only show not hit line balancing target item";
            this.chkBalancing.UseVisualStyleBackColor = true;
            // 
            // comboSewingTeam1
            // 
            this.comboSewingTeam1.BackColor = System.Drawing.Color.White;
            this.comboSewingTeam1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSewingTeam1.FormattingEnabled = true;
            this.comboSewingTeam1.IssupportEmptyitem = true;
            this.comboSewingTeam1.IssupportJunk = true;
            this.comboSewingTeam1.IsSupportUnselect = true;
            this.comboSewingTeam1.Location = new System.Drawing.Point(102, 119);
            this.comboSewingTeam1.Name = "comboSewingTeam1";
            this.comboSewingTeam1.OldText = "";
            this.comboSewingTeam1.Size = new System.Drawing.Size(88, 24);
            this.comboSewingTeam1.TabIndex = 3;
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
            this.dateSewingDate.Location = new System.Drawing.Point(102, 182);
            this.dateSewingDate.Name = "dateSewingDate";
            this.dateSewingDate.Size = new System.Drawing.Size(280, 23);
            this.dateSewingDate.TabIndex = 5;
            // 
            // lbInlineDate
            // 
            this.lbInlineDate.Location = new System.Drawing.Point(14, 182);
            this.lbInlineDate.Name = "lbInlineDate";
            this.lbInlineDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lbInlineDate.RectStyle.BorderWidth = 1F;
            this.lbInlineDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbInlineDate.RectStyle.ExtBorderWidth = 1F;
            this.lbInlineDate.Size = new System.Drawing.Size(85, 23);
            this.lbInlineDate.TabIndex = 238;
            this.lbInlineDate.Text = "Sewing Date";
            this.lbInlineDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lbInlineDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelInlineDate
            // 
            this.labelInlineDate.Location = new System.Drawing.Point(13, 150);
            this.labelInlineDate.Name = "labelInlineDate";
            this.labelInlineDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelInlineDate.RectStyle.BorderWidth = 1F;
            this.labelInlineDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelInlineDate.RectStyle.ExtBorderWidth = 1F;
            this.labelInlineDate.Size = new System.Drawing.Size(86, 23);
            this.labelInlineDate.TabIndex = 239;
            this.labelInlineDate.Text = "Inline Date";
            this.labelInlineDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelInlineDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // R01
            // 
            this.ClientSize = new System.Drawing.Size(531, 320);
            this.Controls.Add(this.labelInlineDate);
            this.Controls.Add(this.lbInlineDate);
            this.Controls.Add(this.dateSewingDate);
            this.Controls.Add(this.comboSewingTeam1);
            this.Controls.Add(this.chkBalancing);
            this.Controls.Add(this.lbReportType);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.dateInlineDate);
            this.Controls.Add(this.txtSeason);
            this.Controls.Add(this.txtStyle);
            this.Controls.Add(this.txtFactory);
            this.Controls.Add(this.labelTeam);
            this.Controls.Add(this.labelSeason);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.labelFactory);
            this.DefaultControl = "txtFactory";
            this.DefaultControlForEdit = "txtFactory";
            this.IsSupportToPrint = false;
            this.Name = "R01";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R01. Line Mapping List";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.labelSeason, 0);
            this.Controls.SetChildIndex(this.labelTeam, 0);
            this.Controls.SetChildIndex(this.txtFactory, 0);
            this.Controls.SetChildIndex(this.txtStyle, 0);
            this.Controls.SetChildIndex(this.txtSeason, 0);
            this.Controls.SetChildIndex(this.dateInlineDate, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.lbReportType, 0);
            this.Controls.SetChildIndex(this.chkBalancing, 0);
            this.Controls.SetChildIndex(this.comboSewingTeam1, 0);
            this.Controls.SetChildIndex(this.dateSewingDate, 0);
            this.Controls.SetChildIndex(this.lbInlineDate, 0);
            this.Controls.SetChildIndex(this.labelInlineDate, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelFactory;
        private Win.UI.Label labelStyle;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelTeam;
        private Win.UI.TextBox txtFactory;
        private Win.UI.TextBox txtStyle;
        private Win.UI.TextBox txtSeason;
        private Win.UI.DateRange dateInlineDate;
        private Win.UI.Label lbReportType;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioSummary;
        private Win.UI.RadioButton radioDetail;
        private Win.UI.CheckBox chkBalancing;
        private Class.ComboSewingTeam comboSewingTeam1;
        private Win.UI.DateRange dateSewingDate;
        private Win.UI.Label lbInlineDate;
        private Win.UI.Label labelInlineDate;
    }
}
