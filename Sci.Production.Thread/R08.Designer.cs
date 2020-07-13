namespace Sci.Production.Thread
{
    partial class R08
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.comboMDivision = new Sci.Production.Class.ComboMDivision(this.components);
            this.labelM = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.txtLocationEnd = new Sci.Win.UI.TextBox();
            this.txtLocationStart = new Sci.Win.UI.TextBox();
            this.txtThreadItem = new Sci.Win.UI.TextBox();
            this.txtType = new Sci.Win.UI.TextBox();
            this.txtShade = new Sci.Win.UI.TextBox();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioSummary = new Sci.Win.UI.RadioButton();
            this.radioDetail = new Sci.Win.UI.RadioButton();
            this.txtRefNoEnd = new Sci.Win.UI.TextBox();
            this.txtRefNoStart = new Sci.Win.UI.TextBox();
            this.dateDate = new Sci.Win.UI.DateRange();
            this.labelReportType = new Sci.Win.UI.Label();
            this.labelLocation = new Sci.Win.UI.Label();
            this.labelThreadItem = new Sci.Win.UI.Label();
            this.labelType = new Sci.Win.UI.Label();
            this.labelShade = new Sci.Win.UI.Label();
            this.labelRefNo = new Sci.Win.UI.Label();
            this.labelDate = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(430, 12);
            this.print.TabIndex = 0;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(430, 48);
            this.toexcel.TabIndex = 1;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(430, 84);
            this.close.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboMDivision);
            this.panel1.Controls.Add(this.labelM);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtLocationEnd);
            this.panel1.Controls.Add(this.txtLocationStart);
            this.panel1.Controls.Add(this.txtThreadItem);
            this.panel1.Controls.Add(this.txtType);
            this.panel1.Controls.Add(this.txtShade);
            this.panel1.Controls.Add(this.radioPanel1);
            this.panel1.Controls.Add(this.txtRefNoEnd);
            this.panel1.Controls.Add(this.txtRefNoStart);
            this.panel1.Controls.Add(this.dateDate);
            this.panel1.Controls.Add(this.labelReportType);
            this.panel1.Controls.Add(this.labelLocation);
            this.panel1.Controls.Add(this.labelThreadItem);
            this.panel1.Controls.Add(this.labelType);
            this.panel1.Controls.Add(this.labelShade);
            this.panel1.Controls.Add(this.labelRefNo);
            this.panel1.Controls.Add(this.labelDate);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(409, 292);
            this.panel1.TabIndex = 0;
            // 
            // comboMDivision
            // 
            this.comboMDivision.BackColor = System.Drawing.Color.White;
            this.comboMDivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMDivision.FormattingEnabled = true;
            this.comboMDivision.IsSupportUnselect = true;
            this.comboMDivision.Location = new System.Drawing.Point(117, 226);
            this.comboMDivision.Name = "comboMDivision";
            this.comboMDivision.Size = new System.Drawing.Size(80, 24);
            this.comboMDivision.TabIndex = 8;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(19, 226);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(90, 23);
            this.labelM.TabIndex = 27;
            this.labelM.Text = "M";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.Control;
            this.label8.Location = new System.Drawing.Point(219, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 30);
            this.label8.TabIndex = 24;
            this.label8.Text = "~";
            this.label8.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.Control;
            this.label9.Location = new System.Drawing.Point(236, 200);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 23);
            this.label9.TabIndex = 23;
            this.label9.Text = "~";
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtLocationEnd
            // 
            this.txtLocationEnd.BackColor = System.Drawing.Color.White;
            this.txtLocationEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocationEnd.Location = new System.Drawing.Point(256, 193);
            this.txtLocationEnd.Name = "txtLocationEnd";
            this.txtLocationEnd.Size = new System.Drawing.Size(113, 23);
            this.txtLocationEnd.TabIndex = 7;
            this.txtLocationEnd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TxtLocationEnd_MouseDown);
            // 
            // txtLocationStart
            // 
            this.txtLocationStart.BackColor = System.Drawing.Color.White;
            this.txtLocationStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocationStart.Location = new System.Drawing.Point(117, 193);
            this.txtLocationStart.Name = "txtLocationStart";
            this.txtLocationStart.Size = new System.Drawing.Size(113, 23);
            this.txtLocationStart.TabIndex = 6;
            this.txtLocationStart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TxtLocationStart_MouseDown);
            // 
            // txtThreadItem
            // 
            this.txtThreadItem.BackColor = System.Drawing.Color.White;
            this.txtThreadItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtThreadItem.Location = new System.Drawing.Point(116, 159);
            this.txtThreadItem.Name = "txtThreadItem";
            this.txtThreadItem.Size = new System.Drawing.Size(127, 23);
            this.txtThreadItem.TabIndex = 5;
            this.txtThreadItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TxtThreadItem_MouseDown);
            // 
            // txtType
            // 
            this.txtType.BackColor = System.Drawing.Color.White;
            this.txtType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtType.Location = new System.Drawing.Point(117, 123);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(135, 23);
            this.txtType.TabIndex = 4;
            this.txtType.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TxtType_MouseDown);
            // 
            // txtShade
            // 
            this.txtShade.BackColor = System.Drawing.Color.White;
            this.txtShade.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShade.Location = new System.Drawing.Point(117, 86);
            this.txtShade.Name = "txtShade";
            this.txtShade.Size = new System.Drawing.Size(122, 23);
            this.txtShade.TabIndex = 3;
            this.txtShade.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TxtShade_MouseDown);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioSummary);
            this.radioPanel1.Controls.Add(this.radioDetail);
            this.radioPanel1.Location = new System.Drawing.Point(116, 256);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(175, 26);
            this.radioPanel1.TabIndex = 9;
            // 
            // radioSummary
            // 
            this.radioSummary.AutoSize = true;
            this.radioSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSummary.Location = new System.Drawing.Point(78, 3);
            this.radioSummary.Name = "radioSummary";
            this.radioSummary.Size = new System.Drawing.Size(85, 21);
            this.radioSummary.TabIndex = 1;
            this.radioSummary.Text = "Summary";
            this.radioSummary.UseVisualStyleBackColor = true;
            // 
            // radioDetail
            // 
            this.radioDetail.AutoSize = true;
            this.radioDetail.Checked = true;
            this.radioDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail.Location = new System.Drawing.Point(10, 3);
            this.radioDetail.Name = "radioDetail";
            this.radioDetail.Size = new System.Drawing.Size(62, 21);
            this.radioDetail.TabIndex = 0;
            this.radioDetail.TabStop = true;
            this.radioDetail.Text = "Detail";
            this.radioDetail.UseVisualStyleBackColor = true;
            // 
            // txtRefNoEnd
            // 
            this.txtRefNoEnd.BackColor = System.Drawing.Color.White;
            this.txtRefNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefNoEnd.Location = new System.Drawing.Point(240, 52);
            this.txtRefNoEnd.Name = "txtRefNoEnd";
            this.txtRefNoEnd.Size = new System.Drawing.Size(100, 23);
            this.txtRefNoEnd.TabIndex = 2;
            this.txtRefNoEnd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TxtRefNoEnd_MouseDown);
            // 
            // txtRefNoStart
            // 
            this.txtRefNoStart.BackColor = System.Drawing.Color.White;
            this.txtRefNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefNoStart.Location = new System.Drawing.Point(116, 52);
            this.txtRefNoStart.Name = "txtRefNoStart";
            this.txtRefNoStart.Size = new System.Drawing.Size(100, 23);
            this.txtRefNoStart.TabIndex = 1;
            this.txtRefNoStart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TxtRefNoStart_MouseDown);
            // 
            // dateDate
            // 
            this.dateDate.IsRequired = false;
            this.dateDate.Location = new System.Drawing.Point(116, 17);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(280, 23);
            this.dateDate.TabIndex = 0;
            // 
            // labelReportType
            // 
            this.labelReportType.Location = new System.Drawing.Point(19, 259);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(90, 23);
            this.labelReportType.TabIndex = 6;
            this.labelReportType.Text = "Report Type";
            // 
            // labelLocation
            // 
            this.labelLocation.Location = new System.Drawing.Point(19, 193);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(90, 23);
            this.labelLocation.TabIndex = 5;
            this.labelLocation.Text = "Location";
            // 
            // labelThreadItem
            // 
            this.labelThreadItem.Location = new System.Drawing.Point(19, 159);
            this.labelThreadItem.Name = "labelThreadItem";
            this.labelThreadItem.Size = new System.Drawing.Size(90, 23);
            this.labelThreadItem.TabIndex = 4;
            this.labelThreadItem.Text = "Thread Item";
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(19, 123);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(90, 23);
            this.labelType.TabIndex = 3;
            this.labelType.Text = "Type";
            // 
            // labelShade
            // 
            this.labelShade.Location = new System.Drawing.Point(19, 86);
            this.labelShade.Name = "labelShade";
            this.labelShade.Size = new System.Drawing.Size(90, 23);
            this.labelShade.TabIndex = 2;
            this.labelShade.Text = "Shade";
            // 
            // labelRefNo
            // 
            this.labelRefNo.Location = new System.Drawing.Point(19, 52);
            this.labelRefNo.Name = "labelRefNo";
            this.labelRefNo.Size = new System.Drawing.Size(90, 23);
            this.labelRefNo.TabIndex = 1;
            this.labelRefNo.Text = "RefNo";
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(19, 17);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(90, 23);
            this.labelDate.TabIndex = 0;
            this.labelDate.Text = "Date";
            // 
            // R08
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 335);
            this.Controls.Add(this.panel1);
            this.Name = "R08";
            this.Text = "R08. Thread Issue List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Label label8;
        private Win.UI.Label label9;
        private Win.UI.TextBox txtLocationEnd;
        private Win.UI.TextBox txtLocationStart;
        private Win.UI.TextBox txtThreadItem;
        private Win.UI.TextBox txtType;
        private Win.UI.TextBox txtShade;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioSummary;
        private Win.UI.RadioButton radioDetail;
        private Win.UI.TextBox txtRefNoEnd;
        private Win.UI.TextBox txtRefNoStart;
        private Win.UI.DateRange dateDate;
        private Win.UI.Label labelReportType;
        private Win.UI.Label labelLocation;
        private Win.UI.Label labelThreadItem;
        private Win.UI.Label labelType;
        private Win.UI.Label labelShade;
        private Win.UI.Label labelRefNo;
        private Win.UI.Label labelDate;
        private Win.UI.Label labelM;
        private Class.ComboMDivision comboMDivision;
    }
}