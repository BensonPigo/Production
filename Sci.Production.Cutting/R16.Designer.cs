namespace Sci.Production.Cutting
{
    partial class R16
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
            this.comboMDivision = new Sci.Production.Class.ComboMDivision(this.components);
            this.comboFactory = new Sci.Production.Class.ComboFactory(this.components);
            this.lbCuttingSP = new Sci.Win.UI.Label();
            this.lbEstCutDate = new Sci.Win.UI.Label();
            this.txtCuttingSPEnd = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.txtCuttingSPStart = new Sci.Win.UI.TextBox();
            this.dateEstCutDate = new Sci.Win.UI.DateRange();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(455, 92);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.toexcel.Location = new System.Drawing.Point(455, 11);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.close.Location = new System.Drawing.Point(455, 56);
            this.close.TabIndex = 6;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(298, 11);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(349, 12);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(324, 11);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 39);
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
            this.comboMDivision.Size = new System.Drawing.Size(106, 24);
            this.comboMDivision.TabIndex = 0;
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
            this.comboFactory.Location = new System.Drawing.Point(159, 39);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(106, 24);
            this.comboFactory.TabIndex = 1;
            // 
            // lbCuttingSP
            // 
            this.lbCuttingSP.Location = new System.Drawing.Point(9, 101);
            this.lbCuttingSP.Name = "lbCuttingSP";
            this.lbCuttingSP.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbCuttingSP.Size = new System.Drawing.Size(147, 23);
            this.lbCuttingSP.TabIndex = 165;
            this.lbCuttingSP.Text = "Cutting SP#";
            this.lbCuttingSP.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lbEstCutDate
            // 
            this.lbEstCutDate.Location = new System.Drawing.Point(9, 70);
            this.lbEstCutDate.Name = "lbEstCutDate";
            this.lbEstCutDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbEstCutDate.Size = new System.Drawing.Size(147, 23);
            this.lbEstCutDate.TabIndex = 163;
            this.lbEstCutDate.Text = "Est. Cut Date";
            this.lbEstCutDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtCuttingSPEnd
            // 
            this.txtCuttingSPEnd.BackColor = System.Drawing.Color.White;
            this.txtCuttingSPEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCuttingSPEnd.Location = new System.Drawing.Point(311, 101);
            this.txtCuttingSPEnd.MaxLength = 13;
            this.txtCuttingSPEnd.Name = "txtCuttingSPEnd";
            this.txtCuttingSPEnd.Size = new System.Drawing.Size(126, 23);
            this.txtCuttingSPEnd.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(288, 101);
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
            this.txtCuttingSPStart.Location = new System.Drawing.Point(159, 101);
            this.txtCuttingSPStart.MaxLength = 13;
            this.txtCuttingSPStart.Name = "txtCuttingSPStart";
            this.txtCuttingSPStart.Size = new System.Drawing.Size(126, 23);
            this.txtCuttingSPStart.TabIndex = 3;
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
            this.dateEstCutDate.Location = new System.Drawing.Point(159, 70);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstCutDate.TabIndex = 2;
            // 
            // R16
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 162);
            this.Controls.Add(this.lbCuttingSP);
            this.Controls.Add(this.lbEstCutDate);
            this.Controls.Add(this.txtCuttingSPEnd);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtCuttingSPStart);
            this.Controls.Add(this.dateEstCutDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboMDivision);
            this.Controls.Add(this.comboFactory);
            this.DefaultControl = "dateEstCutDate";
            this.Name = "R16";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R16. Cutting schedule output list";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.comboMDivision, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.dateEstCutDate, 0);
            this.Controls.SetChildIndex(this.txtCuttingSPStart, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.txtCuttingSPEnd, 0);
            this.Controls.SetChildIndex(this.lbEstCutDate, 0);
            this.Controls.SetChildIndex(this.lbCuttingSP, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Class.ComboMDivision comboMDivision;
        private Class.ComboFactory comboFactory;
        private Win.UI.Label lbCuttingSP;
        private Win.UI.Label lbEstCutDate;
        private Win.UI.TextBox txtCuttingSPEnd;
        private Win.UI.Label label9;
        private Win.UI.TextBox txtCuttingSPStart;
        private Win.UI.DateRange dateEstCutDate;
    }
}