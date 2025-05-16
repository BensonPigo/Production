namespace Sci.Production.Cutting
{
    partial class R13
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
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.comboMDivision = new Sci.Production.Class.ComboMDivision(this.components);
            this.comboFactory = new Sci.Production.Class.ComboFactory(this.components);
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.lbCuttingSP = new Sci.Win.UI.Label();
            this.lbActCuttingDate = new Sci.Win.UI.Label();
            this.lbEstCutDate = new Sci.Win.UI.Label();
            this.dateActCuttingDate = new Sci.Win.UI.DateRange();
            this.txtCuttingSPEnd = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.txtCuttingSPStart = new Sci.Win.UI.TextBox();
            this.dateEstCutDate = new Sci.Win.UI.DateRange();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(455, 93);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.toexcel.Location = new System.Drawing.Point(455, 12);
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.close.Location = new System.Drawing.Point(455, 57);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(270, 151);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(402, 153);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(402, 155);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 23);
            this.label2.TabIndex = 157;
            this.label2.Text = "Factory";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 23);
            this.label1.TabIndex = 156;
            this.label1.Text = "M";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(10, 151);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(146, 23);
            this.label6.TabIndex = 154;
            this.label6.Text = "Style";
            // 
            // comboMDivision
            // 
            this.comboMDivision.BackColor = System.Drawing.Color.White;
            this.comboMDivision.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboMDivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMDivision.FormattingEnabled = true;
            this.comboMDivision.IsSupportUnselect = true;
            this.comboMDivision.Location = new System.Drawing.Point(159, 9);
            this.comboMDivision.Name = "comboMDivision";
            this.comboMDivision.OldText = "";
            this.comboMDivision.Size = new System.Drawing.Size(95, 24);
            this.comboMDivision.TabIndex = 153;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboFactory.FilteMDivision = false;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IssupportJunk = false;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(159, 37);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(95, 24);
            this.comboFactory.TabIndex = 152;
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(159, 151);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.SeasonObjectName = null;
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 150;
            this.txtstyle.TarBrand = null;
            this.txtstyle.TarSeason = null;
            // 
            // lbCuttingSP
            // 
            this.lbCuttingSP.Location = new System.Drawing.Point(9, 122);
            this.lbCuttingSP.Name = "lbCuttingSP";
            this.lbCuttingSP.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbCuttingSP.Size = new System.Drawing.Size(147, 23);
            this.lbCuttingSP.TabIndex = 165;
            this.lbCuttingSP.Text = "Cutting SP#";
            this.lbCuttingSP.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lbActCuttingDate
            // 
            this.lbActCuttingDate.Location = new System.Drawing.Point(9, 93);
            this.lbActCuttingDate.Name = "lbActCuttingDate";
            this.lbActCuttingDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbActCuttingDate.Size = new System.Drawing.Size(147, 23);
            this.lbActCuttingDate.TabIndex = 164;
            this.lbActCuttingDate.Text = "Act. Cutting Date";
            this.lbActCuttingDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lbEstCutDate
            // 
            this.lbEstCutDate.Location = new System.Drawing.Point(9, 64);
            this.lbEstCutDate.Name = "lbEstCutDate";
            this.lbEstCutDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbEstCutDate.Size = new System.Drawing.Size(147, 23);
            this.lbEstCutDate.TabIndex = 163;
            this.lbEstCutDate.Text = "Est. Cut Date";
            this.lbEstCutDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateActCuttingDate
            // 
            // 
            // 
            // 
            this.dateActCuttingDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateActCuttingDate.DateBox1.Name = "";
            this.dateActCuttingDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateActCuttingDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateActCuttingDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateActCuttingDate.DateBox2.Name = "";
            this.dateActCuttingDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateActCuttingDate.DateBox2.TabIndex = 1;
            this.dateActCuttingDate.IsRequired = false;
            this.dateActCuttingDate.Location = new System.Drawing.Point(159, 93);
            this.dateActCuttingDate.Name = "dateActCuttingDate";
            this.dateActCuttingDate.Size = new System.Drawing.Size(280, 23);
            this.dateActCuttingDate.TabIndex = 159;
            // 
            // txtCuttingSPEnd
            // 
            this.txtCuttingSPEnd.BackColor = System.Drawing.Color.White;
            this.txtCuttingSPEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCuttingSPEnd.Location = new System.Drawing.Point(311, 122);
            this.txtCuttingSPEnd.MaxLength = 13;
            this.txtCuttingSPEnd.Name = "txtCuttingSPEnd";
            this.txtCuttingSPEnd.Size = new System.Drawing.Size(126, 23);
            this.txtCuttingSPEnd.TabIndex = 161;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(288, 122);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 23);
            this.label9.TabIndex = 162;
            this.label9.Text = "～";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtCuttingSPStart
            // 
            this.txtCuttingSPStart.BackColor = System.Drawing.Color.White;
            this.txtCuttingSPStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCuttingSPStart.Location = new System.Drawing.Point(159, 122);
            this.txtCuttingSPStart.MaxLength = 13;
            this.txtCuttingSPStart.Name = "txtCuttingSPStart";
            this.txtCuttingSPStart.Size = new System.Drawing.Size(126, 23);
            this.txtCuttingSPStart.TabIndex = 160;
            // 
            // dateEstCutDate
            // 
            // 
            // 
            // 
            this.dateEstCutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEstCutDate.DateBox1.Name = "";
            this.dateEstCutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateEstCutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEstCutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateEstCutDate.DateBox2.Name = "";
            this.dateEstCutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateEstCutDate.DateBox2.TabIndex = 1;
            this.dateEstCutDate.IsRequired = false;
            this.dateEstCutDate.Location = new System.Drawing.Point(159, 64);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstCutDate.TabIndex = 158;
            // 
            // R13
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 212);
            this.Controls.Add(this.lbCuttingSP);
            this.Controls.Add(this.lbActCuttingDate);
            this.Controls.Add(this.lbEstCutDate);
            this.Controls.Add(this.dateActCuttingDate);
            this.Controls.Add(this.txtCuttingSPEnd);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtCuttingSPStart);
            this.Controls.Add(this.dateEstCutDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboMDivision);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.txtstyle);
            this.Name = "R13";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R13. Cutting schedule output list";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.txtstyle, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.comboMDivision, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.dateEstCutDate, 0);
            this.Controls.SetChildIndex(this.txtCuttingSPStart, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.txtCuttingSPEnd, 0);
            this.Controls.SetChildIndex(this.dateActCuttingDate, 0);
            this.Controls.SetChildIndex(this.lbEstCutDate, 0);
            this.Controls.SetChildIndex(this.lbActCuttingDate, 0);
            this.Controls.SetChildIndex(this.lbCuttingSP, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Label label6;
        private Class.ComboMDivision comboMDivision;
        private Class.ComboFactory comboFactory;
        private Class.Txtstyle txtstyle;
        private Win.UI.Label lbCuttingSP;
        private Win.UI.Label lbActCuttingDate;
        private Win.UI.Label lbEstCutDate;
        private Win.UI.DateRange dateActCuttingDate;
        private Win.UI.TextBox txtCuttingSPEnd;
        private Win.UI.Label label9;
        private Win.UI.TextBox txtCuttingSPStart;
        private Win.UI.DateRange dateEstCutDate;
    }
}