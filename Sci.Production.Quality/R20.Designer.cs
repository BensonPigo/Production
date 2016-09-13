namespace Sci.Production.Quality
{
    partial class R20
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.txt_DefectType = new Sci.Win.UI.TextBox();
            this.txt_DefectCode = new Sci.Win.UI.TextBox();
            this.radiobtn_SummybyDateandStyle = new Sci.Win.UI.RadioButton();
            this.radiobtn_SummybyStyle = new Sci.Win.UI.RadioButton();
            this.radioBtn_SummybySP = new Sci.Win.UI.RadioButton();
            this.radioBtn_Detail = new Sci.Win.UI.RadioButton();
            this.radiobtn_AllData = new Sci.Win.UI.RadioButton();
            this.radiobtn_PerCell = new Sci.Win.UI.RadioButton();
            this.radiobtn_PerLine = new Sci.Win.UI.RadioButton();
            this.txt_Cell = new Sci.Win.UI.TextBox();
            this.txt_Line = new Sci.Win.UI.TextBox();
            this.txt_Brand = new Sci.Win.UI.TextBox();
            this.cmb_Factory = new Sci.Win.UI.ComboBox();
            this.datePeriod = new Sci.Win.UI.DateRange();
            this.label8 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(427, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(427, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(427, 84);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txt_DefectType);
            this.panel1.Controls.Add(this.txt_DefectCode);
            this.panel1.Controls.Add(this.radiobtn_SummybyDateandStyle);
            this.panel1.Controls.Add(this.radiobtn_SummybyStyle);
            this.panel1.Controls.Add(this.radioBtn_SummybySP);
            this.panel1.Controls.Add(this.radioBtn_Detail);
            this.panel1.Controls.Add(this.radiobtn_AllData);
            this.panel1.Controls.Add(this.radiobtn_PerCell);
            this.panel1.Controls.Add(this.radiobtn_PerLine);
            this.panel1.Controls.Add(this.txt_Cell);
            this.panel1.Controls.Add(this.txt_Line);
            this.panel1.Controls.Add(this.txt_Brand);
            this.panel1.Controls.Add(this.cmb_Factory);
            this.panel1.Controls.Add(this.datePeriod);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(391, 407);
            this.panel1.TabIndex = 94;
            // 
            // txt_DefectType
            // 
            this.txt_DefectType.BackColor = System.Drawing.Color.White;
            this.txt_DefectType.Enabled = false;
            this.txt_DefectType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txt_DefectType.Location = new System.Drawing.Point(100, 371);
            this.txt_DefectType.Name = "txt_DefectType";
            this.txt_DefectType.Size = new System.Drawing.Size(100, 23);
            this.txt_DefectType.TabIndex = 21;
            // 
            // txt_DefectCode
            // 
            this.txt_DefectCode.BackColor = System.Drawing.Color.White;
            this.txt_DefectCode.Enabled = false;
            this.txt_DefectCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txt_DefectCode.Location = new System.Drawing.Point(100, 342);
            this.txt_DefectCode.Name = "txt_DefectCode";
            this.txt_DefectCode.Size = new System.Drawing.Size(100, 23);
            this.txt_DefectCode.TabIndex = 20;
            // 
            // radiobtn_SummybyDateandStyle
            // 
            this.radiobtn_SummybyDateandStyle.AutoSize = true;
            this.radiobtn_SummybyDateandStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobtn_SummybyDateandStyle.Location = new System.Drawing.Point(100, 315);
            this.radiobtn_SummybyDateandStyle.Name = "radiobtn_SummybyDateandStyle";
            this.radiobtn_SummybyDateandStyle.Size = new System.Drawing.Size(164, 21);
            this.radiobtn_SummybyDateandStyle.TabIndex = 19;
            this.radiobtn_SummybyDateandStyle.Text = "Summy by Date & Style";
            this.radiobtn_SummybyDateandStyle.UseVisualStyleBackColor = true;
            this.radiobtn_SummybyDateandStyle.CheckedChanged += new System.EventHandler(this.radiobtn_SummybyDateandStyle_CheckedChanged);
            // 
            // radiobtn_SummybyStyle
            // 
            this.radiobtn_SummybyStyle.AutoSize = true;
            this.radiobtn_SummybyStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobtn_SummybyStyle.Location = new System.Drawing.Point(100, 288);
            this.radiobtn_SummybyStyle.Name = "radiobtn_SummybyStyle";
            this.radiobtn_SummybyStyle.Size = new System.Drawing.Size(126, 21);
            this.radiobtn_SummybyStyle.TabIndex = 18;
            this.radiobtn_SummybyStyle.Text = "Summy by Style";
            this.radiobtn_SummybyStyle.UseVisualStyleBackColor = true;
            this.radiobtn_SummybyStyle.CheckedChanged += new System.EventHandler(this.radiobtn_SummybyStyle_CheckedChanged);
            // 
            // radioBtn_SummybySP
            // 
            this.radioBtn_SummybySP.AutoSize = true;
            this.radioBtn_SummybySP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBtn_SummybySP.Location = new System.Drawing.Point(100, 261);
            this.radioBtn_SummybySP.Name = "radioBtn_SummybySP";
            this.radioBtn_SummybySP.Size = new System.Drawing.Size(121, 21);
            this.radioBtn_SummybySP.TabIndex = 17;
            this.radioBtn_SummybySP.Text = "Summy by SP#";
            this.radioBtn_SummybySP.UseVisualStyleBackColor = true;
            this.radioBtn_SummybySP.CheckedChanged += new System.EventHandler(this.radioBtn_SummybySP_CheckedChanged);
            // 
            // radioBtn_Detail
            // 
            this.radioBtn_Detail.AutoSize = true;
            this.radioBtn_Detail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBtn_Detail.Location = new System.Drawing.Point(100, 234);
            this.radioBtn_Detail.Name = "radioBtn_Detail";
            this.radioBtn_Detail.Size = new System.Drawing.Size(62, 21);
            this.radioBtn_Detail.TabIndex = 16;
            this.radioBtn_Detail.Text = "Detail";
            this.radioBtn_Detail.UseVisualStyleBackColor = true;
            this.radioBtn_Detail.CheckedChanged += new System.EventHandler(this.radioBtn_Detail_CheckedChanged);
            // 
            // radiobtn_AllData
            // 
            this.radiobtn_AllData.AutoSize = true;
            this.radiobtn_AllData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobtn_AllData.Location = new System.Drawing.Point(100, 207);
            this.radiobtn_AllData.Name = "radiobtn_AllData";
            this.radiobtn_AllData.Size = new System.Drawing.Size(75, 21);
            this.radiobtn_AllData.TabIndex = 15;
            this.radiobtn_AllData.Text = "All Data";
            this.radiobtn_AllData.UseVisualStyleBackColor = true;
            this.radiobtn_AllData.CheckedChanged += new System.EventHandler(this.radiobtn_AllData_CheckedChanged);
            // 
            // radiobtn_PerCell
            // 
            this.radiobtn_PerCell.AutoSize = true;
            this.radiobtn_PerCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobtn_PerCell.Location = new System.Drawing.Point(100, 180);
            this.radiobtn_PerCell.Name = "radiobtn_PerCell";
            this.radiobtn_PerCell.Size = new System.Drawing.Size(71, 21);
            this.radiobtn_PerCell.TabIndex = 14;
            this.radiobtn_PerCell.Text = "PerCell";
            this.radiobtn_PerCell.UseVisualStyleBackColor = true;
            this.radiobtn_PerCell.CheckedChanged += new System.EventHandler(this.radiobtn_PerCell_CheckedChanged);
            // 
            // radiobtn_PerLine
            // 
            this.radiobtn_PerLine.AutoSize = true;
            this.radiobtn_PerLine.Checked = true;
            this.radiobtn_PerLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobtn_PerLine.Location = new System.Drawing.Point(100, 153);
            this.radiobtn_PerLine.Name = "radiobtn_PerLine";
            this.radiobtn_PerLine.Size = new System.Drawing.Size(79, 21);
            this.radiobtn_PerLine.TabIndex = 13;
            this.radiobtn_PerLine.TabStop = true;
            this.radiobtn_PerLine.Text = "Per Line";
            this.radiobtn_PerLine.UseVisualStyleBackColor = true;
            this.radiobtn_PerLine.CheckedChanged += new System.EventHandler(this.radiobtn_PerLine_CheckedChanged);
            // 
            // txt_Cell
            // 
            this.txt_Cell.BackColor = System.Drawing.Color.White;
            this.txt_Cell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txt_Cell.Location = new System.Drawing.Point(100, 124);
            this.txt_Cell.MaxLength = 1;
            this.txt_Cell.Name = "txt_Cell";
            this.txt_Cell.Size = new System.Drawing.Size(36, 23);
            this.txt_Cell.TabIndex = 12;
            // 
            // txt_Line
            // 
            this.txt_Line.BackColor = System.Drawing.Color.White;
            this.txt_Line.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txt_Line.Location = new System.Drawing.Point(100, 95);
            this.txt_Line.MaxLength = 2;
            this.txt_Line.Name = "txt_Line";
            this.txt_Line.Size = new System.Drawing.Size(36, 23);
            this.txt_Line.TabIndex = 11;
            // 
            // txt_Brand
            // 
            this.txt_Brand.BackColor = System.Drawing.Color.White;
            this.txt_Brand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txt_Brand.Location = new System.Drawing.Point(100, 66);
            this.txt_Brand.MaxLength = 10;
            this.txt_Brand.Name = "txt_Brand";
            this.txt_Brand.Size = new System.Drawing.Size(100, 23);
            this.txt_Brand.TabIndex = 10;
            // 
            // cmb_Factory
            // 
            this.cmb_Factory.BackColor = System.Drawing.Color.White;
            this.cmb_Factory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmb_Factory.FormattingEnabled = true;
            this.cmb_Factory.IsSupportUnselect = true;
            this.cmb_Factory.Location = new System.Drawing.Point(100, 36);
            this.cmb_Factory.Name = "cmb_Factory";
            this.cmb_Factory.Size = new System.Drawing.Size(121, 24);
            this.cmb_Factory.TabIndex = 9;
            // 
            // datePeriod
            // 
            this.datePeriod.Location = new System.Drawing.Point(100, 7);
            this.datePeriod.Name = "datePeriod";
            this.datePeriod.Size = new System.Drawing.Size(280, 23);
            this.datePeriod.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(9, 371);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 23);
            this.label8.TabIndex = 7;
            this.label8.Text = "Defect Type:";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(9, 342);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 23);
            this.label7.TabIndex = 6;
            this.label7.Text = "Defect Code:";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(9, 153);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "Formal";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(9, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Cell";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(9, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Line";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(9, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Brand";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(9, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Factory";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Period";
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(410, 117);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 22);
            this.label9.TabIndex = 97;
            this.label9.Text = "Paper Size A4";
            // 
            // R20
            // 
            this.ClientSize = new System.Drawing.Size(519, 452);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel1);
            this.Name = "R20";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.TextBox txt_DefectType;
        private Win.UI.TextBox txt_DefectCode;
        private Win.UI.RadioButton radiobtn_SummybyDateandStyle;
        private Win.UI.RadioButton radiobtn_SummybyStyle;
        private Win.UI.RadioButton radioBtn_SummybySP;
        private Win.UI.RadioButton radioBtn_Detail;
        private Win.UI.RadioButton radiobtn_AllData;
        private Win.UI.RadioButton radiobtn_PerCell;
        private Win.UI.RadioButton radiobtn_PerLine;
        private Win.UI.TextBox txt_Cell;
        private Win.UI.TextBox txt_Line;
        private Win.UI.TextBox txt_Brand;
        private Win.UI.ComboBox cmb_Factory;
        private Win.UI.DateRange datePeriod;
        private Win.UI.Label label8;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Label label9;
    }
}
