namespace Sci.Production.Centralized
{
    partial class R07
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
            this.dateOutputDate = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.lbCDcode = new Sci.Win.UI.Label();
            this.lbShift = new Sci.Win.UI.Label();
            this.txtCDCode = new Sci.Win.UI.TextBox();
            this.comboShift = new Sci.Win.UI.ComboBox();
            this.comboM = new Sci.Production.Class.ComboCentralizedM(this.components);
            this.comboFactory = new Sci.Production.Class.ComboCentralizedFactory(this.components);
            this.radioSintexEffReportCompare = new Sci.Win.UI.RadioButton();
            this.radioDetail = new Sci.Win.UI.RadioButton();
            this.lbFormat = new Sci.Win.UI.Label();
            this.numYear = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).BeginInit();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(430, 12);
            this.print.TabIndex = 4;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(430, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(430, 84);
            this.close.TabIndex = 6;
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(410, 1);
            // 
            // lbOutputDate
            // 
            this.lbOutputDate.Location = new System.Drawing.Point(13, 12);
            this.lbOutputDate.Name = "lbOutputDate";
            this.lbOutputDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lbOutputDate.RectStyle.BorderWidth = 1F;
            this.lbOutputDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbOutputDate.RectStyle.ExtBorderWidth = 1F;
            this.lbOutputDate.Size = new System.Drawing.Size(98, 23);
            this.lbOutputDate.TabIndex = 96;
            this.lbOutputDate.Text = "OutputDate";
            this.lbOutputDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lbOutputDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateOutputDate
            // 
            // 
            // 
            // 
            this.dateOutputDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateOutputDate.DateBox1.Name = "";
            this.dateOutputDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateOutputDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateOutputDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateOutputDate.DateBox2.Name = "";
            this.dateOutputDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateOutputDate.DateBox2.TabIndex = 1;
            this.dateOutputDate.IsRequired = false;
            this.dateOutputDate.Location = new System.Drawing.Point(115, 12);
            this.dateOutputDate.Name = "dateOutputDate";
            this.dateOutputDate.Size = new System.Drawing.Size(280, 23);
            this.dateOutputDate.TabIndex = 0;
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
            // lbCDcode
            // 
            this.lbCDcode.Location = new System.Drawing.Point(13, 121);
            this.lbCDcode.Name = "lbCDcode";
            this.lbCDcode.Size = new System.Drawing.Size(98, 23);
            this.lbCDcode.TabIndex = 136;
            this.lbCDcode.Text = "CD Code";
            // 
            // lbShift
            // 
            this.lbShift.Location = new System.Drawing.Point(13, 157);
            this.lbShift.Name = "lbShift";
            this.lbShift.Size = new System.Drawing.Size(98, 23);
            this.lbShift.TabIndex = 137;
            this.lbShift.Text = "Shift";
            // 
            // txtCDCode
            // 
            this.txtCDCode.BackColor = System.Drawing.Color.White;
            this.txtCDCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCDCode.Location = new System.Drawing.Point(115, 121);
            this.txtCDCode.Name = "txtCDCode";
            this.txtCDCode.Size = new System.Drawing.Size(100, 23);
            this.txtCDCode.TabIndex = 138;
            // 
            // comboShift
            // 
            this.comboShift.BackColor = System.Drawing.Color.White;
            this.comboShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShift.FormattingEnabled = true;
            this.comboShift.IsSupportUnselect = true;
            this.comboShift.Location = new System.Drawing.Point(115, 157);
            this.comboShift.Name = "comboShift";
            this.comboShift.OldText = "";
            this.comboShift.Size = new System.Drawing.Size(66, 24);
            this.comboShift.TabIndex = 139;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(115, 47);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(80, 24);
            this.comboM.TabIndex = 140;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(115, 83);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(80, 24);
            this.comboFactory.TabIndex = 141;
            // 
            // radioSintexEffReportCompare
            // 
            this.radioSintexEffReportCompare.AutoSize = true;
            this.radioSintexEffReportCompare.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSintexEffReportCompare.Location = new System.Drawing.Point(115, 224);
            this.radioSintexEffReportCompare.Name = "radioSintexEffReportCompare";
            this.radioSintexEffReportCompare.Size = new System.Drawing.Size(193, 21);
            this.radioSintexEffReportCompare.TabIndex = 145;
            this.radioSintexEffReportCompare.Text = "Sintex Eff Report Compare";
            this.radioSintexEffReportCompare.UseVisualStyleBackColor = true;
            this.radioSintexEffReportCompare.CheckedChanged += new System.EventHandler(this.RadioSintexEffReportCompare_CheckedChanged);
            // 
            // radioDetail
            // 
            this.radioDetail.AutoSize = true;
            this.radioDetail.Checked = true;
            this.radioDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail.Location = new System.Drawing.Point(115, 196);
            this.radioDetail.Name = "radioDetail";
            this.radioDetail.Size = new System.Drawing.Size(82, 21);
            this.radioDetail.TabIndex = 144;
            this.radioDetail.TabStop = true;
            this.radioDetail.Text = "By Detail";
            this.radioDetail.UseVisualStyleBackColor = true;
            this.radioDetail.CheckedChanged += new System.EventHandler(this.RadioDetail_CheckedChanged);
            // 
            // lbFormat
            // 
            this.lbFormat.Location = new System.Drawing.Point(13, 196);
            this.lbFormat.Name = "lbFormat";
            this.lbFormat.Size = new System.Drawing.Size(98, 23);
            this.lbFormat.TabIndex = 143;
            this.lbFormat.Text = "Format";
            // 
            // numYear
            // 
            this.numYear.Location = new System.Drawing.Point(115, 12);
            this.numYear.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.numYear.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numYear.Name = "numYear";
            this.numYear.Size = new System.Drawing.Size(72, 23);
            this.numYear.TabIndex = 146;
            this.numYear.Value = new decimal(new int[] {
            2015,
            0,
            0,
            0});
            this.numYear.Visible = false;
            // 
            // R07
            // 
            this.ClientSize = new System.Drawing.Size(522, 295);
            this.Controls.Add(this.numYear);
            this.Controls.Add(this.radioSintexEffReportCompare);
            this.Controls.Add(this.radioDetail);
            this.Controls.Add(this.lbFormat);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.comboShift);
            this.Controls.Add(this.txtCDCode);
            this.Controls.Add(this.lbShift);
            this.Controls.Add(this.lbCDcode);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.dateOutputDate);
            this.Controls.Add(this.lbOutputDate);
            this.DefaultControl = "dateOutputDate";
            this.DefaultControlForEdit = "dateOutputDate";
            this.IsSupportToPrint = false;
            this.Name = "R07";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R07. Adidas Efficiency Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.lbOutputDate, 0);
            this.Controls.SetChildIndex(this.dateOutputDate, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.lbCDcode, 0);
            this.Controls.SetChildIndex(this.lbShift, 0);
            this.Controls.SetChildIndex(this.txtCDCode, 0);
            this.Controls.SetChildIndex(this.comboShift, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.lbFormat, 0);
            this.Controls.SetChildIndex(this.radioDetail, 0);
            this.Controls.SetChildIndex(this.radioSintexEffReportCompare, 0);
            this.Controls.SetChildIndex(this.numYear, 0);
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbOutputDate;
        private Win.UI.DateRange dateOutputDate;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelM;
        private Win.UI.Label lbCDcode;
        private Win.UI.Label lbShift;
        private Win.UI.TextBox txtCDCode;
        private Win.UI.ComboBox comboShift;
        private Class.ComboCentralizedM comboM;
        private Class.ComboCentralizedFactory comboFactory;
        private Win.UI.RadioButton radioSintexEffReportCompare;
        private Win.UI.RadioButton radioDetail;
        private Win.UI.Label lbFormat;
        private System.Windows.Forms.NumericUpDown numYear;
    }
}
