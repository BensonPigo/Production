namespace Sci.Production.IE
{
    partial class R02
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
            this.lbFactory = new Sci.Win.UI.Label();
            this.lbYearMonth = new Sci.Win.UI.Label();
            this.lbSewingLine = new Sci.Win.UI.Label();
            this.txtSewingLine = new Sci.Win.UI.TextBox();
            this.dateTPmonth = new System.Windows.Forms.DateTimePicker();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(299, 6);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(299, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(299, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(29, 12);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(161, 12);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(267, 10);
            // 
            // lbFactory
            // 
            this.lbFactory.Location = new System.Drawing.Point(40, 91);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(85, 23);
            this.lbFactory.TabIndex = 97;
            this.lbFactory.Text = "Factory";
            // 
            // lbYearMonth
            // 
            this.lbYearMonth.Location = new System.Drawing.Point(40, 48);
            this.lbYearMonth.Name = "lbYearMonth";
            this.lbYearMonth.Size = new System.Drawing.Size(85, 23);
            this.lbYearMonth.TabIndex = 98;
            this.lbYearMonth.Text = "Year / Month";
            // 
            // lbSewingLine
            // 
            this.lbSewingLine.Location = new System.Drawing.Point(40, 133);
            this.lbSewingLine.Name = "lbSewingLine";
            this.lbSewingLine.Size = new System.Drawing.Size(85, 23);
            this.lbSewingLine.TabIndex = 99;
            this.lbSewingLine.Text = "Sewing Line";
            // 
            // txtSewingLine
            // 
            this.txtSewingLine.BackColor = System.Drawing.Color.White;
            this.txtSewingLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSewingLine.Location = new System.Drawing.Point(128, 133);
            this.txtSewingLine.Name = "txtSewingLine";
            this.txtSewingLine.Size = new System.Drawing.Size(41, 23);
            this.txtSewingLine.TabIndex = 2;
            this.txtSewingLine.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSewingLineStart_PopUp);
            // 
            // dateTPmonth
            // 
            this.dateTPmonth.CalendarTitleBackColor = System.Drawing.SystemColors.ControlText;
            this.dateTPmonth.CalendarTitleForeColor = System.Drawing.Color.Red;
            this.dateTPmonth.CustomFormat = "yyyy - MM";
            this.dateTPmonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTPmonth.Location = new System.Drawing.Point(128, 48);
            this.dateTPmonth.Name = "dateTPmonth";
            this.dateTPmonth.ShowUpDown = true;
            this.dateTPmonth.Size = new System.Drawing.Size(85, 23);
            this.dateTPmonth.TabIndex = 0;
            this.dateTPmonth.Value = new System.DateTime(2019, 1, 1, 0, 0, 0, 0);
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(128, 91);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(65, 24);
            this.comboFactory.TabIndex = 1;
            // 
            // R02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 221);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.dateTPmonth);
            this.Controls.Add(this.txtSewingLine);
            this.Controls.Add(this.lbSewingLine);
            this.Controls.Add(this.lbYearMonth);
            this.Controls.Add(this.lbFactory);
            this.Name = "R02";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R02. Style Changeover Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.lbFactory, 0);
            this.Controls.SetChildIndex(this.lbYearMonth, 0);
            this.Controls.SetChildIndex(this.lbSewingLine, 0);
            this.Controls.SetChildIndex(this.txtSewingLine, 0);
            this.Controls.SetChildIndex(this.dateTPmonth, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbFactory;
        private Win.UI.Label lbYearMonth;
        private Win.UI.Label lbSewingLine;
        private Win.UI.TextBox txtSewingLine;
        private System.Windows.Forms.DateTimePicker dateTPmonth;
        private Win.UI.ComboBox comboFactory;
    }
}