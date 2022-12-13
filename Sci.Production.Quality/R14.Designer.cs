namespace Sci.Production.Quality
{
    partial class R14
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dateLastInsDate = new Sci.Win.UI.DateRange();
            this.lbLastInsDate = new Sci.Win.UI.Label();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioSummary = new Sci.Win.UI.RadioButton();
            this.radioDetail = new Sci.Win.UI.RadioButton();
            this.labelReportType = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtWorkOrderCuRef = new Sci.Win.UI.TextBox();
            this.txtInsCutRef = new Sci.Win.UI.TextBox();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.comboShift = new Sci.Production.Class.ComboShift();
            this.comboFactory = new Sci.Production.Class.ComboFactory(this.components);
            this.comboMDivision = new Sci.Production.Class.ComboMDivision(this.components);
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(304, 151);
            this.print.TabStop = false;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(449, 9);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(449, 45);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(304, 234);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(304, 187);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(304, 205);
            // 
            // dateLastInsDate
            // 
            // 
            // 
            // 
            this.dateLastInsDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateLastInsDate.DateBox1.Name = "";
            this.dateLastInsDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateLastInsDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateLastInsDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateLastInsDate.DateBox2.Name = "";
            this.dateLastInsDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateLastInsDate.DateBox2.TabIndex = 1;
            this.dateLastInsDate.Location = new System.Drawing.Point(150, 9);
            this.dateLastInsDate.Name = "dateLastInsDate";
            this.dateLastInsDate.Size = new System.Drawing.Size(280, 23);
            this.dateLastInsDate.TabIndex = 98;
            // 
            // lbLastInsDate
            // 
            this.lbLastInsDate.Location = new System.Drawing.Point(9, 9);
            this.lbLastInsDate.Name = "lbLastInsDate";
            this.lbLastInsDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lbLastInsDate.RectStyle.BorderWidth = 1F;
            this.lbLastInsDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbLastInsDate.RectStyle.ExtBorderWidth = 1F;
            this.lbLastInsDate.Size = new System.Drawing.Size(138, 23);
            this.lbLastInsDate.TabIndex = 99;
            this.lbLastInsDate.Text = "Last Ins. Date";
            this.lbLastInsDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lbLastInsDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioSummary);
            this.radioPanel1.Controls.Add(this.radioDetail);
            this.radioPanel1.Location = new System.Drawing.Point(150, 215);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(113, 59);
            this.radioPanel1.TabIndex = 109;
            // 
            // radioSummary
            // 
            this.radioSummary.AutoSize = true;
            this.radioSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSummary.Location = new System.Drawing.Point(3, 3);
            this.radioSummary.Name = "radioSummary";
            this.radioSummary.Size = new System.Drawing.Size(85, 21);
            this.radioSummary.TabIndex = 11;
            this.radioSummary.TabStop = true;
            this.radioSummary.Text = "Summary";
            this.radioSummary.UseVisualStyleBackColor = true;
            // 
            // radioDetail
            // 
            this.radioDetail.AutoSize = true;
            this.radioDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail.Location = new System.Drawing.Point(3, 30);
            this.radioDetail.Name = "radioDetail";
            this.radioDetail.Size = new System.Drawing.Size(82, 21);
            this.radioDetail.TabIndex = 10;
            this.radioDetail.TabStop = true;
            this.radioDetail.Text = "By Detail";
            this.radioDetail.UseVisualStyleBackColor = true;
            // 
            // labelReportType
            // 
            this.labelReportType.Location = new System.Drawing.Point(9, 216);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(138, 23);
            this.labelReportType.TabIndex = 110;
            this.labelReportType.Text = "Format";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(9, 125);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(138, 23);
            this.labelM.TabIndex = 112;
            this.labelM.Text = "M";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 23);
            this.label1.TabIndex = 114;
            this.label1.Text = "Factory";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 23);
            this.label2.TabIndex = 116;
            this.label2.Text = "Shift";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 38);
            this.label3.Name = "label3";
            this.label3.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label3.RectStyle.BorderWidth = 1F;
            this.label3.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label3.RectStyle.ExtBorderWidth = 1F;
            this.label3.Size = new System.Drawing.Size(138, 23);
            this.label3.TabIndex = 118;
            this.label3.Text = "Work Order CutRef#";
            this.label3.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtWorkOrderCuRef
            // 
            this.txtWorkOrderCuRef.BackColor = System.Drawing.Color.White;
            this.txtWorkOrderCuRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWorkOrderCuRef.Location = new System.Drawing.Point(150, 38);
            this.txtWorkOrderCuRef.Name = "txtWorkOrderCuRef";
            this.txtWorkOrderCuRef.Size = new System.Drawing.Size(110, 23);
            this.txtWorkOrderCuRef.TabIndex = 119;
            // 
            // txtInsCutRef
            // 
            this.txtInsCutRef.BackColor = System.Drawing.Color.White;
            this.txtInsCutRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInsCutRef.Location = new System.Drawing.Point(150, 67);
            this.txtInsCutRef.Name = "txtInsCutRef";
            this.txtInsCutRef.Size = new System.Drawing.Size(110, 23);
            this.txtInsCutRef.TabIndex = 120;
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(150, 96);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(110, 23);
            this.txtSP.TabIndex = 121;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 67);
            this.label4.Name = "label4";
            this.label4.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label4.RectStyle.BorderWidth = 1F;
            this.label4.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label4.RectStyle.ExtBorderWidth = 1F;
            this.label4.Size = new System.Drawing.Size(138, 23);
            this.label4.TabIndex = 122;
            this.label4.Text = "Ins. CutRef#";
            this.label4.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 96);
            this.label5.Name = "label5";
            this.label5.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label5.RectStyle.BorderWidth = 1F;
            this.label5.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label5.RectStyle.ExtBorderWidth = 1F;
            this.label5.Size = new System.Drawing.Size(138, 23);
            this.label5.TabIndex = 123;
            this.label5.Text = "SP#";
            this.label5.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label5.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(436, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 23);
            this.label6.TabIndex = 124;
            this.label6.Text = "Paper Size A4";
            // 
            // comboShift
            // 
            this.comboShift.BackColor = System.Drawing.Color.White;
            this.comboShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShift.FormattingEnabled = true;
            this.comboShift.IsSupportUnselect = true;
            this.comboShift.Location = new System.Drawing.Point(150, 185);
            this.comboShift.Name = "comboShift";
            this.comboShift.OldText = "";
            this.comboShift.Size = new System.Drawing.Size(113, 24);
            this.comboShift.TabIndex = 125;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.FilteMDivision = false;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IssupportJunk = false;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(150, 155);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(113, 24);
            this.comboFactory.TabIndex = 115;
            this.comboFactory.SelectedIndexChanged += new System.EventHandler(this.ComboFactory_SelectedIndexChanged);
            // 
            // comboMDivision
            // 
            this.comboMDivision.BackColor = System.Drawing.Color.White;
            this.comboMDivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMDivision.FormattingEnabled = true;
            this.comboMDivision.IsSupportUnselect = true;
            this.comboMDivision.Location = new System.Drawing.Point(150, 125);
            this.comboMDivision.Name = "comboMDivision";
            this.comboMDivision.OldText = "";
            this.comboMDivision.Size = new System.Drawing.Size(113, 24);
            this.comboMDivision.TabIndex = 113;
            // 
            // R14
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 300);
            this.Controls.Add(this.comboShift);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSP);
            this.Controls.Add(this.txtInsCutRef);
            this.Controls.Add(this.txtWorkOrderCuRef);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboMDivision);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.labelReportType);
            this.Controls.Add(this.dateLastInsDate);
            this.Controls.Add(this.lbLastInsDate);
            this.Name = "R14";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R14. Spreading Inspection Repor";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.lbLastInsDate, 0);
            this.Controls.SetChildIndex(this.dateLastInsDate, 0);
            this.Controls.SetChildIndex(this.labelReportType, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.comboMDivision, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtWorkOrderCuRef, 0);
            this.Controls.SetChildIndex(this.txtInsCutRef, 0);
            this.Controls.SetChildIndex(this.txtSP, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.comboShift, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateLastInsDate;
        private Win.UI.Label lbLastInsDate;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioSummary;
        private Win.UI.RadioButton radioDetail;
        private Win.UI.Label labelReportType;
        private Win.UI.Label labelM;
        private Class.ComboMDivision comboMDivision;
        private Win.UI.Label label1;
        private Class.ComboFactory comboFactory;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtWorkOrderCuRef;
        private Win.UI.TextBox txtInsCutRef;
        private Win.UI.TextBox txtSP;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Class.ComboShift comboShift;
    }
}