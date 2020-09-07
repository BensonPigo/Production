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
            this.txtCell = new Sci.Production.Class.TxtCell();
            this.txtLine = new Sci.Production.Class.Txtsewingline();
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.txtDefectType = new Sci.Win.UI.TextBox();
            this.txtDefectCode = new Sci.Win.UI.TextBox();
            this.radioSummybyDateandStyle = new Sci.Win.UI.RadioButton();
            this.radioSummybyStyle = new Sci.Win.UI.RadioButton();
            this.radioSummybySP = new Sci.Win.UI.RadioButton();
            this.radioDetail = new Sci.Win.UI.RadioButton();
            this.radioAllData = new Sci.Win.UI.RadioButton();
            this.radioPerCell = new Sci.Win.UI.RadioButton();
            this.radioPerLine = new Sci.Win.UI.RadioButton();
            this.ComboFactory = new Sci.Win.UI.ComboBox();
            this.datePeriod = new Sci.Win.UI.DateRange();
            this.labelDefectType = new Sci.Win.UI.Label();
            this.labelDefectCode = new Sci.Win.UI.Label();
            this.labelFormal = new Sci.Win.UI.Label();
            this.labelCell = new Sci.Win.UI.Label();
            this.labelLine = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelPeriod = new Sci.Win.UI.Label();
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
            this.panel1.Controls.Add(this.txtCell);
            this.panel1.Controls.Add(this.txtLine);
            this.panel1.Controls.Add(this.txtBrand);
            this.panel1.Controls.Add(this.txtDefectType);
            this.panel1.Controls.Add(this.txtDefectCode);
            this.panel1.Controls.Add(this.radioSummybyDateandStyle);
            this.panel1.Controls.Add(this.radioSummybyStyle);
            this.panel1.Controls.Add(this.radioSummybySP);
            this.panel1.Controls.Add(this.radioDetail);
            this.panel1.Controls.Add(this.radioAllData);
            this.panel1.Controls.Add(this.radioPerCell);
            this.panel1.Controls.Add(this.radioPerLine);
            this.panel1.Controls.Add(this.ComboFactory);
            this.panel1.Controls.Add(this.datePeriod);
            this.panel1.Controls.Add(this.labelDefectType);
            this.panel1.Controls.Add(this.labelDefectCode);
            this.panel1.Controls.Add(this.labelFormal);
            this.panel1.Controls.Add(this.labelCell);
            this.panel1.Controls.Add(this.labelLine);
            this.panel1.Controls.Add(this.labelBrand);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelPeriod);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(391, 407);
            this.panel1.TabIndex = 94;
            // 
            // txtCell
            // 
            this.txtCell.BackColor = System.Drawing.Color.White;
            this.txtCell.MDivisionID = Sci.Env.User.Keyword;
            this.txtCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCell.Location = new System.Drawing.Point(100, 123);
            this.txtCell.Name = "txtCell";
            this.txtCell.Size = new System.Drawing.Size(30, 23);
            this.txtCell.TabIndex = 24;
            // 
            // txtLine
            // 
            this.txtLine.BackColor = System.Drawing.Color.White;
            this.txtLine.FactoryobjectName = this.ComboFactory;
            this.txtLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLine.Location = new System.Drawing.Point(100, 94);
            this.txtLine.Name = "txtLine";
            this.txtLine.Size = new System.Drawing.Size(60, 23);
            this.txtLine.TabIndex = 23;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(100, 65);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(66, 23);
            this.txtBrand.TabIndex = 22;
            // 
            // txtDefectType
            // 
            this.txtDefectType.BackColor = System.Drawing.Color.White;
            this.txtDefectType.Enabled = false;
            this.txtDefectType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDefectType.Location = new System.Drawing.Point(100, 371);
            this.txtDefectType.MaxLength = 1;
            this.txtDefectType.Name = "txtDefectType";
            this.txtDefectType.Size = new System.Drawing.Size(36, 23);
            this.txtDefectType.TabIndex = 21;
            // 
            // txtDefectCode
            // 
            this.txtDefectCode.BackColor = System.Drawing.Color.White;
            this.txtDefectCode.Enabled = false;
            this.txtDefectCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDefectCode.Location = new System.Drawing.Point(100, 342);
            this.txtDefectCode.MaxLength = 3;
            this.txtDefectCode.Name = "txtDefectCode";
            this.txtDefectCode.Size = new System.Drawing.Size(100, 23);
            this.txtDefectCode.TabIndex = 20;
            // 
            // radioSummybyDateandStyle
            // 
            this.radioSummybyDateandStyle.AutoSize = true;
            this.radioSummybyDateandStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSummybyDateandStyle.Location = new System.Drawing.Point(100, 315);
            this.radioSummybyDateandStyle.Name = "radioSummybyDateandStyle";
            this.radioSummybyDateandStyle.Size = new System.Drawing.Size(164, 21);
            this.radioSummybyDateandStyle.TabIndex = 19;
            this.radioSummybyDateandStyle.Text = "Summy by Date & Style";
            this.radioSummybyDateandStyle.UseVisualStyleBackColor = true;
            this.radioSummybyDateandStyle.CheckedChanged += new System.EventHandler(this.RadioSummybyDateandStyle_CheckedChanged);
            // 
            // radioSummybyStyle
            // 
            this.radioSummybyStyle.AutoSize = true;
            this.radioSummybyStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSummybyStyle.Location = new System.Drawing.Point(100, 288);
            this.radioSummybyStyle.Name = "radioSummybyStyle";
            this.radioSummybyStyle.Size = new System.Drawing.Size(126, 21);
            this.radioSummybyStyle.TabIndex = 18;
            this.radioSummybyStyle.Text = "Summy by Style";
            this.radioSummybyStyle.UseVisualStyleBackColor = true;
            this.radioSummybyStyle.CheckedChanged += new System.EventHandler(this.RadioSummybyStyle_CheckedChanged);
            // 
            // radioSummybySP
            // 
            this.radioSummybySP.AutoSize = true;
            this.radioSummybySP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSummybySP.Location = new System.Drawing.Point(100, 261);
            this.radioSummybySP.Name = "radioSummybySP";
            this.radioSummybySP.Size = new System.Drawing.Size(121, 21);
            this.radioSummybySP.TabIndex = 17;
            this.radioSummybySP.Text = "Summy by SP#";
            this.radioSummybySP.UseVisualStyleBackColor = true;
            this.radioSummybySP.CheckedChanged += new System.EventHandler(this.RadioSummybySP_CheckedChanged);
            // 
            // radioDetail
            // 
            this.radioDetail.AutoSize = true;
            this.radioDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail.Location = new System.Drawing.Point(100, 234);
            this.radioDetail.Name = "radioDetail";
            this.radioDetail.Size = new System.Drawing.Size(62, 21);
            this.radioDetail.TabIndex = 16;
            this.radioDetail.Text = "Detail";
            this.radioDetail.UseVisualStyleBackColor = true;
            this.radioDetail.CheckedChanged += new System.EventHandler(this.RadioDetail_CheckedChanged);
            // 
            // radioAllData
            // 
            this.radioAllData.AutoSize = true;
            this.radioAllData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioAllData.Location = new System.Drawing.Point(100, 207);
            this.radioAllData.Name = "radioAllData";
            this.radioAllData.Size = new System.Drawing.Size(75, 21);
            this.radioAllData.TabIndex = 15;
            this.radioAllData.Text = "All Data";
            this.radioAllData.UseVisualStyleBackColor = true;
            this.radioAllData.CheckedChanged += new System.EventHandler(this.RadioAllData_CheckedChanged);
            // 
            // radioPerCell
            // 
            this.radioPerCell.AutoSize = true;
            this.radioPerCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioPerCell.Location = new System.Drawing.Point(100, 180);
            this.radioPerCell.Name = "radioPerCell";
            this.radioPerCell.Size = new System.Drawing.Size(71, 21);
            this.radioPerCell.TabIndex = 14;
            this.radioPerCell.Text = "PerCell";
            this.radioPerCell.UseVisualStyleBackColor = true;
            this.radioPerCell.CheckedChanged += new System.EventHandler(this.RadioPerCell_CheckedChanged);
            // 
            // radioPerLine
            // 
            this.radioPerLine.AutoSize = true;
            this.radioPerLine.Checked = true;
            this.radioPerLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioPerLine.Location = new System.Drawing.Point(100, 153);
            this.radioPerLine.Name = "radioPerLine";
            this.radioPerLine.Size = new System.Drawing.Size(79, 21);
            this.radioPerLine.TabIndex = 13;
            this.radioPerLine.TabStop = true;
            this.radioPerLine.Text = "Per Line";
            this.radioPerLine.UseVisualStyleBackColor = true;
            this.radioPerLine.CheckedChanged += new System.EventHandler(this.RadioPerLine_CheckedChanged);
            // 
            // ComboFactory
            // 
            this.ComboFactory.BackColor = System.Drawing.Color.White;
            this.ComboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ComboFactory.FormattingEnabled = true;
            this.ComboFactory.IsSupportUnselect = true;
            this.ComboFactory.Location = new System.Drawing.Point(100, 36);
            this.ComboFactory.Name = "ComboFactory";
            this.ComboFactory.Size = new System.Drawing.Size(121, 24);
            this.ComboFactory.TabIndex = 9;
            this.ComboFactory.TextChanged += new System.EventHandler(this.ComboFactory_TextChanged);
            // 
            // datePeriod
            // 
            this.datePeriod.IsRequired = false;
            this.datePeriod.Location = new System.Drawing.Point(100, 7);
            this.datePeriod.Name = "datePeriod";
            this.datePeriod.Size = new System.Drawing.Size(280, 23);
            this.datePeriod.TabIndex = 8;
            // 
            // labelDefectType
            // 
            this.labelDefectType.Location = new System.Drawing.Point(9, 371);
            this.labelDefectType.Name = "labelDefectType";
            this.labelDefectType.Size = new System.Drawing.Size(88, 23);
            this.labelDefectType.TabIndex = 7;
            this.labelDefectType.Text = "Defect Type:";
            // 
            // labelDefectCode
            // 
            this.labelDefectCode.Location = new System.Drawing.Point(9, 342);
            this.labelDefectCode.Name = "labelDefectCode";
            this.labelDefectCode.Size = new System.Drawing.Size(88, 23);
            this.labelDefectCode.TabIndex = 6;
            this.labelDefectCode.Text = "Defect Code:";
            // 
            // labelFormal
            // 
            this.labelFormal.Location = new System.Drawing.Point(9, 153);
            this.labelFormal.Name = "labelFormal";
            this.labelFormal.Size = new System.Drawing.Size(88, 23);
            this.labelFormal.TabIndex = 5;
            this.labelFormal.Text = "Formal";
            // 
            // labelCell
            // 
            this.labelCell.Location = new System.Drawing.Point(9, 123);
            this.labelCell.Name = "labelCell";
            this.labelCell.Size = new System.Drawing.Size(88, 23);
            this.labelCell.TabIndex = 4;
            this.labelCell.Text = "Cell";
            // 
            // labelLine
            // 
            this.labelLine.Location = new System.Drawing.Point(9, 94);
            this.labelLine.Name = "labelLine";
            this.labelLine.Size = new System.Drawing.Size(88, 23);
            this.labelLine.TabIndex = 3;
            this.labelLine.Text = "Line";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(9, 65);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(88, 23);
            this.labelBrand.TabIndex = 2;
            this.labelBrand.Text = "Brand";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(9, 36);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(88, 23);
            this.labelFactory.TabIndex = 1;
            this.labelFactory.Text = "Factory";
            // 
            // labelPeriod
            // 
            this.labelPeriod.Location = new System.Drawing.Point(9, 7);
            this.labelPeriod.Name = "labelPeriod";
            this.labelPeriod.Size = new System.Drawing.Size(88, 23);
            this.labelPeriod.TabIndex = 0;
            this.labelPeriod.Text = "Period";
            // 
            // label9
            // 
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
            this.Text = "R20.Right First Time Daily Report   ";
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
        private Win.UI.TextBox txtDefectType;
        private Win.UI.TextBox txtDefectCode;
        private Win.UI.RadioButton radioSummybyDateandStyle;
        private Win.UI.RadioButton radioSummybyStyle;
        private Win.UI.RadioButton radioSummybySP;
        private Win.UI.RadioButton radioDetail;
        private Win.UI.RadioButton radioAllData;
        private Win.UI.RadioButton radioPerCell;
        private Win.UI.RadioButton radioPerLine;
        private Win.UI.ComboBox ComboFactory;
        private Win.UI.DateRange datePeriod;
        private Win.UI.Label labelDefectType;
        private Win.UI.Label labelDefectCode;
        private Win.UI.Label labelFormal;
        private Win.UI.Label labelCell;
        private Win.UI.Label labelLine;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelPeriod;
        private Win.UI.Label label9;
        private Class.TxtCell txtCell;
        private Class.Txtsewingline txtLine;
        private Class.Txtbrand txtBrand;
    }
}
