namespace Sci.Production.Sewing
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
            this.labelDate = new Sci.Win.UI.Label();
            this.labelSewingLine = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelOrderBy = new Sci.Win.UI.Label();
            this.dateDateStart = new Sci.Win.UI.DateBox();
            this.dateDateEnd = new Sci.Win.UI.DateBox();
            this.label8 = new Sci.Win.UI.Label();
            this.txtSewingLineStart = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.txtSewingLineEnd = new Sci.Win.UI.TextBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.comboOrderBy = new Sci.Win.UI.ComboBox();
            this.label10 = new Sci.Win.UI.Label();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.labelM = new Sci.Win.UI.Label();
            this.checkSampleFty = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(355, 12);
            this.print.TabIndex = 10;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(355, 48);
            this.toexcel.TabIndex = 11;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(355, 84);
            this.close.TabIndex = 12;
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(13, 12);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(80, 23);
            this.labelDate.TabIndex = 94;
            this.labelDate.Text = "Date";
            // 
            // labelSewingLine
            // 
            this.labelSewingLine.Location = new System.Drawing.Point(13, 48);
            this.labelSewingLine.Name = "labelSewingLine";
            this.labelSewingLine.Size = new System.Drawing.Size(80, 23);
            this.labelSewingLine.TabIndex = 95;
            this.labelSewingLine.Text = "Sewing Line";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 120);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(80, 23);
            this.labelFactory.TabIndex = 99;
            this.labelFactory.Text = "Factory";
            // 
            // labelOrderBy
            // 
            this.labelOrderBy.Location = new System.Drawing.Point(13, 158);
            this.labelOrderBy.Name = "labelOrderBy";
            this.labelOrderBy.Size = new System.Drawing.Size(80, 23);
            this.labelOrderBy.TabIndex = 100;
            this.labelOrderBy.Text = "Order By";
            // 
            // dateDateStart
            // 
            this.dateDateStart.Location = new System.Drawing.Point(97, 12);
            this.dateDateStart.Name = "dateDateStart";
            this.dateDateStart.Size = new System.Drawing.Size(98, 23);
            this.dateDateStart.TabIndex = 0;
            // 
            // dateDateEnd
            // 
            this.dateDateEnd.IsSupportEditMode = false;
            this.dateDateEnd.Location = new System.Drawing.Point(222, 12);
            this.dateDateEnd.Name = "dateDateEnd";
            this.dateDateEnd.Size = new System.Drawing.Size(98, 23);
            this.dateDateEnd.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(198, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(21, 23);
            this.label8.TabIndex = 103;
            this.label8.Text = "～";
            this.label8.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label8.TextStyle.Color = System.Drawing.Color.Black;
            this.label8.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label8.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtSewingLineStart
            // 
            this.txtSewingLineStart.BackColor = System.Drawing.Color.White;
            this.txtSewingLineStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSewingLineStart.Location = new System.Drawing.Point(97, 48);
            this.txtSewingLineStart.Name = "txtSewingLineStart";
            this.txtSewingLineStart.Size = new System.Drawing.Size(41, 23);
            this.txtSewingLineStart.TabIndex = 2;
            this.txtSewingLineStart.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSewingLineStart_PopUp);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(141, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 23);
            this.label9.TabIndex = 105;
            this.label9.Text = "～";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtSewingLineEnd
            // 
            this.txtSewingLineEnd.BackColor = System.Drawing.Color.White;
            this.txtSewingLineEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSewingLineEnd.Location = new System.Drawing.Point(165, 48);
            this.txtSewingLineEnd.Name = "txtSewingLineEnd";
            this.txtSewingLineEnd.Size = new System.Drawing.Size(41, 23);
            this.txtSewingLineEnd.TabIndex = 3;
            this.txtSewingLineEnd.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSewingLineEnd_PopUp);
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(97, 120);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(65, 24);
            this.comboFactory.TabIndex = 7;
            // 
            // comboOrderBy
            // 
            this.comboOrderBy.BackColor = System.Drawing.Color.White;
            this.comboOrderBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboOrderBy.FormattingEnabled = true;
            this.comboOrderBy.IsSupportUnselect = true;
            this.comboOrderBy.Location = new System.Drawing.Point(97, 158);
            this.comboOrderBy.Name = "comboOrderBy";
            this.comboOrderBy.OldText = "";
            this.comboOrderBy.Size = new System.Drawing.Size(121, 24);
            this.comboOrderBy.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(13, 185);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(341, 47);
            this.label10.TabIndex = 9;
            this.label10.Text = "** The value in this report are all excluded subcon-out, unless the column with \"" +
    "included subcon-out\".";
            this.label10.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.label10.TextStyle.Color = System.Drawing.Color.Red;
            this.label10.TextStyle.ExtBorderColor = System.Drawing.Color.Red;
            this.label10.TextStyle.GradientColor = System.Drawing.Color.Red;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(96, 84);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(66, 24);
            this.comboM.TabIndex = 106;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 84);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(80, 23);
            this.labelM.TabIndex = 107;
            this.labelM.Text = "M";
            // 
            // checkSampleFty
            // 
            this.checkSampleFty.AutoSize = true;
            this.checkSampleFty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkSampleFty.Location = new System.Drawing.Point(241, 158);
            this.checkSampleFty.Name = "checkSampleFty";
            this.checkSampleFty.Size = new System.Drawing.Size(178, 21);
            this.checkSampleFty.TabIndex = 108;
            this.checkSampleFty.Text = "Exclude Sample Factory";
            this.checkSampleFty.UseVisualStyleBackColor = true;
            // 
            // R08
            // 
            this.ClientSize = new System.Drawing.Size(475, 270);
            this.Controls.Add(this.checkSampleFty);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.comboOrderBy);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.txtSewingLineEnd);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtSewingLineStart);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.dateDateEnd);
            this.Controls.Add(this.dateDateStart);
            this.Controls.Add(this.labelOrderBy);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelSewingLine);
            this.Controls.Add(this.labelDate);
            this.DefaultControl = "dateDateStart";
            this.DefaultControlForEdit = "dateDateStart";
            this.IsSupportToPrint = false;
            this.Name = "R08";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R08. Production output Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labelDate, 0);
            this.Controls.SetChildIndex(this.labelSewingLine, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelOrderBy, 0);
            this.Controls.SetChildIndex(this.dateDateStart, 0);
            this.Controls.SetChildIndex(this.dateDateEnd, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.txtSewingLineStart, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.txtSewingLineEnd, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.comboOrderBy, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.checkSampleFty, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelDate;
        private Win.UI.Label labelSewingLine;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelOrderBy;
        private Win.UI.DateBox dateDateStart;
        private Win.UI.DateBox dateDateEnd;
        private Win.UI.Label label8;
        private Win.UI.TextBox txtSewingLineStart;
        private Win.UI.Label label9;
        private Win.UI.TextBox txtSewingLineEnd;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.ComboBox comboOrderBy;
        private Win.UI.Label label10;
        private Win.UI.ComboBox comboM;
        private Win.UI.Label labelM;
        private Win.UI.CheckBox checkSampleFty;
    }
}
