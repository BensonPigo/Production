﻿namespace Sci.Production.Quality
{
    partial class R51
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
            this.dateInspectionDate = new Sci.Win.UI.DateRange();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.comboShift = new Sci.Win.UI.ComboBox();
            this.comboFactory1 = new Sci.Production.Class.ComboFactory(this.components);
            this.comboMDivision1 = new Sci.Production.Class.ComboMDivision(this.components);
            this.txtstyle1 = new Sci.Production.Class.Txtstyle();
            this.radioDetail_DefectType = new Sci.Win.UI.RadioButton();
            this.radioSummary = new Sci.Win.UI.RadioButton();
            this.label9 = new Sci.Win.UI.Label();
            this.radioDetail_Responseteam = new Sci.Win.UI.RadioButton();
            this.txtsubprocess = new Sci.Production.Class.Txtsubprocess();
            this.label7 = new Sci.Win.UI.Label();
            this.radioDetail_Operator = new Sci.Win.UI.RadioButton();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(404, 106);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(404, 12);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(404, 48);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(361, 142);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(387, 178);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(384, 205);
            // 
            // dateInspectionDate
            // 
            // 
            // 
            // 
            this.dateInspectionDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInspectionDate.DateBox1.Name = "";
            this.dateInspectionDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateInspectionDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInspectionDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateInspectionDate.DateBox2.Name = "";
            this.dateInspectionDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateInspectionDate.DateBox2.TabIndex = 1;
            this.dateInspectionDate.IsRequired = false;
            this.dateInspectionDate.Location = new System.Drawing.Point(126, 12);
            this.dateInspectionDate.Name = "dateInspectionDate";
            this.dateInspectionDate.Size = new System.Drawing.Size(280, 23);
            this.dateInspectionDate.TabIndex = 0;
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(126, 41);
            this.txtSP.MaxLength = 13;
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(116, 23);
            this.txtSP.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.Location = new System.Drawing.Point(392, 81);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 22);
            this.label10.TabIndex = 97;
            this.label10.Text = "Paper Size A4";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(18, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 23);
            this.label2.TabIndex = 120;
            this.label2.Text = "Style";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 23);
            this.label1.TabIndex = 122;
            this.label1.Text = "M";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(18, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 23);
            this.label3.TabIndex = 124;
            this.label3.Text = "Factory";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(18, 41);
            this.label4.Name = "label4";
            this.label4.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label4.Size = new System.Drawing.Size(105, 23);
            this.label4.TabIndex = 125;
            this.label4.Text = "SP#";
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(18, 12);
            this.label5.Name = "label5";
            this.label5.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label5.Size = new System.Drawing.Size(105, 23);
            this.label5.TabIndex = 126;
            this.label5.Text = "Inspection Date";
            this.label5.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(18, 189);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 23);
            this.label6.TabIndex = 127;
            this.label6.Text = "Shift";
            // 
            // comboShift
            // 
            this.comboShift.BackColor = System.Drawing.Color.White;
            this.comboShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShift.FormattingEnabled = true;
            this.comboShift.IsSupportUnselect = true;
            this.comboShift.Location = new System.Drawing.Point(126, 189);
            this.comboShift.Name = "comboShift";
            this.comboShift.OldText = "";
            this.comboShift.Size = new System.Drawing.Size(80, 24);
            this.comboShift.TabIndex = 6;
            // 
            // comboFactory1
            // 
            this.comboFactory1.BackColor = System.Drawing.Color.White;
            this.comboFactory1.FilteMDivision = false;
            this.comboFactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory1.FormattingEnabled = true;
            this.comboFactory1.IssupportJunk = false;
            this.comboFactory1.IsSupportUnselect = true;
            this.comboFactory1.Location = new System.Drawing.Point(126, 158);
            this.comboFactory1.Name = "comboFactory1";
            this.comboFactory1.OldText = "";
            this.comboFactory1.Size = new System.Drawing.Size(80, 24);
            this.comboFactory1.TabIndex = 5;
            // 
            // comboMDivision1
            // 
            this.comboMDivision1.BackColor = System.Drawing.Color.White;
            this.comboMDivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMDivision1.FormattingEnabled = true;
            this.comboMDivision1.IsSupportUnselect = true;
            this.comboMDivision1.Location = new System.Drawing.Point(126, 128);
            this.comboMDivision1.Name = "comboMDivision1";
            this.comboMDivision1.OldText = "";
            this.comboMDivision1.Size = new System.Drawing.Size(80, 24);
            this.comboMDivision1.TabIndex = 4;
            // 
            // txtstyle1
            // 
            this.txtstyle1.BackColor = System.Drawing.Color.White;
            this.txtstyle1.BrandObjectName = null;
            this.txtstyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle1.Location = new System.Drawing.Point(126, 99);
            this.txtstyle1.Name = "txtstyle1";
            this.txtstyle1.Size = new System.Drawing.Size(130, 23);
            this.txtstyle1.TabIndex = 3;
            this.txtstyle1.TarBrand = null;
            this.txtstyle1.TarSeason = null;
            // 
            // radioDetail_DefectType
            // 
            this.radioDetail_DefectType.AutoSize = true;
            this.radioDetail_DefectType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail_DefectType.Location = new System.Drawing.Point(126, 249);
            this.radioDetail_DefectType.Name = "radioDetail_DefectType";
            this.radioDetail_DefectType.Size = new System.Drawing.Size(167, 21);
            this.radioDetail_DefectType.TabIndex = 137;
            this.radioDetail_DefectType.Text = "By Detail(Defect Type)";
            this.radioDetail_DefectType.UseVisualStyleBackColor = true;
            // 
            // radioSummary
            // 
            this.radioSummary.AutoSize = true;
            this.radioSummary.Checked = true;
            this.radioSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSummary.Location = new System.Drawing.Point(126, 222);
            this.radioSummary.Name = "radioSummary";
            this.radioSummary.Size = new System.Drawing.Size(85, 21);
            this.radioSummary.TabIndex = 136;
            this.radioSummary.TabStop = true;
            this.radioSummary.Text = "Summary";
            this.radioSummary.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(18, 220);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(105, 23);
            this.label9.TabIndex = 135;
            this.label9.Text = "Format";
            // 
            // radioDetail_Responseteam
            // 
            this.radioDetail_Responseteam.AutoSize = true;
            this.radioDetail_Responseteam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail_Responseteam.Location = new System.Drawing.Point(126, 276);
            this.radioDetail_Responseteam.Name = "radioDetail_Responseteam";
            this.radioDetail_Responseteam.Size = new System.Drawing.Size(191, 21);
            this.radioDetail_Responseteam.TabIndex = 138;
            this.radioDetail_Responseteam.Text = "By Detail(Response team)";
            this.radioDetail_Responseteam.UseVisualStyleBackColor = true;
            // 
            // txtsubprocess
            // 
            this.txtsubprocess.BackColor = System.Drawing.Color.White;
            this.txtsubprocess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtsubprocess.IsRFIDProcess = true;
            this.txtsubprocess.Location = new System.Drawing.Point(126, 70);
            this.txtsubprocess.MultiSelect = true;
            this.txtsubprocess.Name = "txtsubprocess";
            this.txtsubprocess.Size = new System.Drawing.Size(130, 23);
            this.txtsubprocess.TabIndex = 140;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(18, 70);
            this.label7.Name = "label7";
            this.label7.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label7.Size = new System.Drawing.Size(105, 23);
            this.label7.TabIndex = 141;
            this.label7.Text = "SubProcess";
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // radioDetail_Operator
            // 
            this.radioDetail_Operator.AutoSize = true;
            this.radioDetail_Operator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail_Operator.Location = new System.Drawing.Point(126, 303);
            this.radioDetail_Operator.Name = "radioDetail_Operator";
            this.radioDetail_Operator.Size = new System.Drawing.Size(149, 21);
            this.radioDetail_Operator.TabIndex = 142;
            this.radioDetail_Operator.Text = "By Detail(Operator)";
            this.radioDetail_Operator.UseVisualStyleBackColor = true;
            // 
            // R51
            // 
            this.ClientSize = new System.Drawing.Size(496, 363);
            this.Controls.Add(this.radioDetail_Operator);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtsubprocess);
            this.Controls.Add(this.radioDetail_Responseteam);
            this.Controls.Add(this.radioDetail_DefectType);
            this.Controls.Add(this.radioSummary);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboShift);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.comboFactory1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboMDivision1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtstyle1);
            this.Controls.Add(this.dateInspectionDate);
            this.Controls.Add(this.txtSP);
            this.Name = "R51";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R51. Sub-Process Inspection Report";
            this.Controls.SetChildIndex(this.txtSP, 0);
            this.Controls.SetChildIndex(this.dateInspectionDate, 0);
            this.Controls.SetChildIndex(this.txtstyle1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.comboMDivision1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.comboFactory1, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.comboShift, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.radioSummary, 0);
            this.Controls.SetChildIndex(this.radioDetail_DefectType, 0);
            this.Controls.SetChildIndex(this.radioDetail_Responseteam, 0);
            this.Controls.SetChildIndex(this.txtsubprocess, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.radioDetail_Operator, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.TextBox txtSP;
        private Win.UI.DateRange dateInspectionDate;
        private Win.UI.Label label10;
        private Win.UI.Label label3;
        private Class.ComboFactory comboFactory1;
        private Win.UI.Label label1;
        private Class.ComboMDivision comboMDivision1;
        private Win.UI.Label label2;
        private Class.Txtstyle txtstyle1;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.ComboBox comboShift;
        private Win.UI.RadioButton radioDetail_DefectType;
        private Win.UI.RadioButton radioSummary;
        private Win.UI.Label label9;
        private Win.UI.RadioButton radioDetail_Responseteam;
        private Class.Txtsubprocess txtsubprocess;
        private Win.UI.Label label7;
        private Win.UI.RadioButton radioDetail_Operator;
    }
}
