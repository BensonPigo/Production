namespace Sci.Production.Warehouse
{
    partial class R45
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
            this.radioSummary = new Sci.Win.UI.RadioButton();
            this.radioDetail = new Sci.Win.UI.RadioButton();
            this.lblReportType = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.lblSubConReturn = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.txtSP2 = new Sci.Win.UI.TextBox();
            this.txtSP1 = new Sci.Win.UI.TextBox();
            this.dateSubConReturnDate = new Sci.Win.UI.DateRange();
            this.label4 = new Sci.Win.UI.Label();
            this.dateTransfertoSubconDate = new Sci.Win.UI.DateRange();
            this.lblSP = new Sci.Win.UI.Label();
            this.lblMdivision = new Sci.Win.UI.Label();
            this.lbFactory = new Sci.Win.UI.Label();
            this.lblRefno = new Sci.Win.UI.Label();
            this.lblSeq = new Sci.Win.UI.Label();
            this.txtSeq = new Sci.Production.Class.TxtSeq();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(362, 202);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(468, 9);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(468, 45);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(422, 172);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(448, 208);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(448, 143);
            // 
            // radioSummary
            // 
            this.radioSummary.AutoSize = true;
            this.radioSummary.Checked = true;
            this.radioSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSummary.Location = new System.Drawing.Point(120, 215);
            this.radioSummary.Name = "radioSummary";
            this.radioSummary.Size = new System.Drawing.Size(85, 21);
            this.radioSummary.TabIndex = 176;
            this.radioSummary.TabStop = true;
            this.radioSummary.Text = "Summary";
            this.radioSummary.UseVisualStyleBackColor = true;
            // 
            // radioDetail
            // 
            this.radioDetail.AutoSize = true;
            this.radioDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail.Location = new System.Drawing.Point(211, 215);
            this.radioDetail.Name = "radioDetail";
            this.radioDetail.Size = new System.Drawing.Size(62, 21);
            this.radioDetail.TabIndex = 175;
            this.radioDetail.Text = "Detail";
            this.radioDetail.UseVisualStyleBackColor = true;
            // 
            // lblReportType
            // 
            this.lblReportType.Location = new System.Drawing.Point(9, 208);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(108, 23);
            this.lblReportType.TabIndex = 183;
            this.lblReportType.Text = "Report Type";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(120, 180);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 174;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(120, 151);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 173;
            // 
            // lblSubConReturn
            // 
            this.lblSubConReturn.Location = new System.Drawing.Point(9, 67);
            this.lblSubConReturn.Name = "lblSubConReturn";
            this.lblSubConReturn.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lblSubConReturn.Size = new System.Drawing.Size(161, 23);
            this.lblSubConReturn.TabIndex = 182;
            this.lblSubConReturn.Text = "Sub con Return Date";
            this.lblSubConReturn.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Location = new System.Drawing.Point(306, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 23);
            this.label6.TabIndex = 181;
            this.label6.Text = "～";
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSP2
            // 
            this.txtSP2.BackColor = System.Drawing.Color.White;
            this.txtSP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP2.Location = new System.Drawing.Point(330, 9);
            this.txtSP2.Name = "txtSP2";
            this.txtSP2.Size = new System.Drawing.Size(123, 23);
            this.txtSP2.TabIndex = 172;
            // 
            // txtSP1
            // 
            this.txtSP1.BackColor = System.Drawing.Color.White;
            this.txtSP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP1.Location = new System.Drawing.Point(173, 9);
            this.txtSP1.Name = "txtSP1";
            this.txtSP1.Size = new System.Drawing.Size(130, 23);
            this.txtSP1.TabIndex = 171;
            // 
            // dateSubConReturnDate
            // 
            // 
            // 
            // 
            this.dateSubConReturnDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSubConReturnDate.DateBox1.Name = "";
            this.dateSubConReturnDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSubConReturnDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSubConReturnDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSubConReturnDate.DateBox2.Name = "";
            this.dateSubConReturnDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSubConReturnDate.DateBox2.TabIndex = 1;
            this.dateSubConReturnDate.Location = new System.Drawing.Point(173, 67);
            this.dateSubConReturnDate.Name = "dateSubConReturnDate";
            this.dateSubConReturnDate.Size = new System.Drawing.Size(280, 23);
            this.dateSubConReturnDate.TabIndex = 170;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 38);
            this.label4.Name = "label4";
            this.label4.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label4.Size = new System.Drawing.Size(161, 23);
            this.label4.TabIndex = 180;
            this.label4.Text = "Transfer to Sub con Date";
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateTransfertoSubconDate
            // 
            // 
            // 
            // 
            this.dateTransfertoSubconDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateTransfertoSubconDate.DateBox1.Name = "";
            this.dateTransfertoSubconDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateTransfertoSubconDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateTransfertoSubconDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateTransfertoSubconDate.DateBox2.Name = "";
            this.dateTransfertoSubconDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateTransfertoSubconDate.DateBox2.TabIndex = 1;
            this.dateTransfertoSubconDate.Location = new System.Drawing.Point(173, 38);
            this.dateTransfertoSubconDate.Name = "dateTransfertoSubconDate";
            this.dateTransfertoSubconDate.Size = new System.Drawing.Size(280, 23);
            this.dateTransfertoSubconDate.TabIndex = 169;
            // 
            // lblSP
            // 
            this.lblSP.Location = new System.Drawing.Point(9, 9);
            this.lblSP.Name = "lblSP";
            this.lblSP.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lblSP.Size = new System.Drawing.Size(161, 23);
            this.lblSP.TabIndex = 179;
            this.lblSP.Text = "SP#";
            this.lblSP.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lblMdivision
            // 
            this.lblMdivision.Location = new System.Drawing.Point(9, 151);
            this.lblMdivision.Name = "lblMdivision";
            this.lblMdivision.Size = new System.Drawing.Size(108, 23);
            this.lblMdivision.TabIndex = 178;
            this.lblMdivision.Text = "M";
            // 
            // lbFactory
            // 
            this.lbFactory.Location = new System.Drawing.Point(9, 180);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(108, 23);
            this.lbFactory.TabIndex = 177;
            this.lbFactory.Text = "Factory";
            // 
            // lblRefno
            // 
            this.lblRefno.Location = new System.Drawing.Point(9, 123);
            this.lblRefno.Name = "lblRefno";
            this.lblRefno.Size = new System.Drawing.Size(108, 23);
            this.lblRefno.TabIndex = 184;
            this.lblRefno.Text = "Refno";
            // 
            // lblSeq
            // 
            this.lblSeq.Location = new System.Drawing.Point(9, 95);
            this.lblSeq.Name = "lblSeq";
            this.lblSeq.Size = new System.Drawing.Size(108, 23);
            this.lblSeq.TabIndex = 185;
            this.lblSeq.Text = "Seq";
            // 
            // txtSeq
            // 
            this.txtSeq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq.Location = new System.Drawing.Point(121, 96);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Seq1 = "";
            this.txtSeq.Seq2 = "";
            this.txtSeq.Size = new System.Drawing.Size(65, 23);
            this.txtSeq.TabIndex = 186;
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(120, 122);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(130, 23);
            this.txtRefno.TabIndex = 187;
            // 
            // R45
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 261);
            this.Controls.Add(this.txtRefno);
            this.Controls.Add(this.txtSeq);
            this.Controls.Add(this.lblSeq);
            this.Controls.Add(this.lblRefno);
            this.Controls.Add(this.radioSummary);
            this.Controls.Add(this.radioDetail);
            this.Controls.Add(this.lblReportType);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.lblSubConReturn);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSP2);
            this.Controls.Add(this.txtSP1);
            this.Controls.Add(this.dateSubConReturnDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateTransfertoSubconDate);
            this.Controls.Add(this.lblSP);
            this.Controls.Add(this.lblMdivision);
            this.Controls.Add(this.lbFactory);
            this.Name = "R45";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R45.  Transfer to Sub con report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.lbFactory, 0);
            this.Controls.SetChildIndex(this.lblMdivision, 0);
            this.Controls.SetChildIndex(this.lblSP, 0);
            this.Controls.SetChildIndex(this.dateTransfertoSubconDate, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.dateSubConReturnDate, 0);
            this.Controls.SetChildIndex(this.txtSP1, 0);
            this.Controls.SetChildIndex(this.txtSP2, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.lblSubConReturn, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.lblReportType, 0);
            this.Controls.SetChildIndex(this.radioDetail, 0);
            this.Controls.SetChildIndex(this.radioSummary, 0);
            this.Controls.SetChildIndex(this.lblRefno, 0);
            this.Controls.SetChildIndex(this.lblSeq, 0);
            this.Controls.SetChildIndex(this.txtSeq, 0);
            this.Controls.SetChildIndex(this.txtRefno, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioButton radioSummary;
        private Win.UI.RadioButton radioDetail;
        private Win.UI.Label lblReportType;
        private Class.Txtfactory txtfactory;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.Label lblSubConReturn;
        private Win.UI.Label label6;
        private Win.UI.TextBox txtSP2;
        private Win.UI.TextBox txtSP1;
        private Win.UI.DateRange dateSubConReturnDate;
        private Win.UI.Label label4;
        private Win.UI.DateRange dateTransfertoSubconDate;
        private Win.UI.Label lblSP;
        private Win.UI.Label lblMdivision;
        private Win.UI.Label lbFactory;
        private Win.UI.Label lblRefno;
        private Win.UI.Label lblSeq;
        private Class.TxtSeq txtSeq;
        private Win.UI.TextBox txtRefno;
    }
}