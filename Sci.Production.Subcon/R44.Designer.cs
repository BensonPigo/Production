namespace Sci.Production.Subcon
{
    partial class R44
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
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.comboDropDownList1 = new Sci.Production.Class.ComboDropDownList(this.components);
            this.label1 = new Sci.Win.UI.Label();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.dateSewingDate = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.labelReportType = new Sci.Win.UI.Label();
            this.labelSewingDate = new Sci.Win.UI.Label();
            this.radioBySP = new Sci.Win.UI.RadioButton();
            this.radioByFactory = new Sci.Win.UI.RadioButton();
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(440, 12);
            this.print.TabIndex = 1;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(440, 48);
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(440, 84);
            this.close.TabIndex = 3;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.chkIncludeCancelOrder);
            this.radioPanel1.Controls.Add(this.comboDropDownList1);
            this.radioPanel1.Controls.Add(this.label1);
            this.radioPanel1.Controls.Add(this.txtSP);
            this.radioPanel1.Controls.Add(this.comboFactory);
            this.radioPanel1.Controls.Add(this.dateSewingDate);
            this.radioPanel1.Controls.Add(this.labelFactory);
            this.radioPanel1.Controls.Add(this.labelSP);
            this.radioPanel1.Controls.Add(this.labelReportType);
            this.radioPanel1.Controls.Add(this.labelSewingDate);
            this.radioPanel1.Controls.Add(this.radioBySP);
            this.radioPanel1.Controls.Add(this.radioByFactory);
            this.radioPanel1.Location = new System.Drawing.Point(12, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(398, 214);
            this.radioPanel1.TabIndex = 0;
            this.radioPanel1.Value = "P/L Rcv Report";
            // 
            // comboDropDownList1
            // 
            this.comboDropDownList1.BackColor = System.Drawing.Color.White;
            this.comboDropDownList1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownList1.FormattingEnabled = true;
            this.comboDropDownList1.IsSupportUnselect = true;
            this.comboDropDownList1.Location = new System.Drawing.Point(105, 159);
            this.comboDropDownList1.Name = "comboDropDownList1";
            this.comboDropDownList1.OldText = "";
            this.comboDropDownList1.Size = new System.Drawing.Size(121, 24);
            this.comboDropDownList1.TabIndex = 13;
            this.comboDropDownList1.Type = "Pms_ReportForProduct";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 159);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "Category";
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(105, 130);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(121, 23);
            this.txtSP.TabIndex = 4;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(105, 7);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 0;
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
            this.dateSewingDate.Location = new System.Drawing.Point(105, 40);
            this.dateSewingDate.Name = "dateSewingDate";
            this.dateSewingDate.Size = new System.Drawing.Size(280, 23);
            this.dateSewingDate.TabIndex = 1;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(8, 7);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(94, 23);
            this.labelFactory.TabIndex = 11;
            this.labelFactory.Text = "Factory";
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(8, 130);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(94, 23);
            this.labelSP.TabIndex = 11;
            this.labelSP.Text = "SP#";
            // 
            // labelReportType
            // 
            this.labelReportType.Location = new System.Drawing.Point(8, 72);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(94, 23);
            this.labelReportType.TabIndex = 11;
            this.labelReportType.Text = "Report Type";
            // 
            // labelSewingDate
            // 
            this.labelSewingDate.Location = new System.Drawing.Point(8, 40);
            this.labelSewingDate.Name = "labelSewingDate";
            this.labelSewingDate.Size = new System.Drawing.Size(94, 23);
            this.labelSewingDate.TabIndex = 11;
            this.labelSewingDate.Text = "Sewing Date";
            // 
            // radioBySP
            // 
            this.radioBySP.AutoSize = true;
            this.radioBySP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBySP.Location = new System.Drawing.Point(105, 99);
            this.radioBySP.Name = "radioBySP";
            this.radioBySP.Size = new System.Drawing.Size(72, 21);
            this.radioBySP.TabIndex = 3;
            this.radioBySP.TabStop = true;
            this.radioBySP.Text = "By SP#";
            this.radioBySP.UseVisualStyleBackColor = true;
            this.radioBySP.Value = "Arrive W/H Report";
            // 
            // radioByFactory
            // 
            this.radioByFactory.AutoSize = true;
            this.radioByFactory.Checked = true;
            this.radioByFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByFactory.Location = new System.Drawing.Point(105, 72);
            this.radioByFactory.Name = "radioByFactory";
            this.radioByFactory.Size = new System.Drawing.Size(93, 21);
            this.radioByFactory.TabIndex = 2;
            this.radioByFactory.TabStop = true;
            this.radioByFactory.Text = "By Factory";
            this.radioByFactory.UseVisualStyleBackColor = true;
            this.radioByFactory.Value = "P/L Rcv Report";
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(8, 187);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 128;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // R44
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 255);
            this.Controls.Add(this.radioPanel1);
            this.Name = "R44";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R44. Loading BCS Base on Std.Q(RFID)";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioBySP;
        private Win.UI.RadioButton radioByFactory;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelSP;
        private Win.UI.Label labelReportType;
        private Win.UI.Label labelSewingDate;
        private Win.UI.DateRange dateSewingDate;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.TextBox txtSP;
        private Class.ComboDropDownList comboDropDownList1;
        private Win.UI.Label label1;
        private Win.UI.CheckBox chkIncludeCancelOrder;
    }
}