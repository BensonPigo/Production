namespace Sci.Production.Cutting
{
    partial class R02
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
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.radioBySummary = new Sci.Win.UI.RadioButton();
            this.radioByOneDayDetial = new Sci.Win.UI.RadioButton();
            this.radioByDetail = new Sci.Win.UI.RadioButton();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.dateCuttingDate = new Sci.Win.UI.DateRange();
            this.labelType = new Sci.Win.UI.Label();
            this.labelCuttingDate = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.btnSendMail = new Sci.Win.UI.Button();
            this.txtCellTo = new Sci.Production.Class.TxtCell();
            this.txtCellFrom = new Sci.Production.Class.TxtCell();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(462, 12);
            this.print.Size = new System.Drawing.Size(87, 30);
            this.print.TabIndex = 100;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(462, 48);
            this.toexcel.Size = new System.Drawing.Size(87, 30);
            this.toexcel.TabIndex = 101;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(462, 120);
            this.close.Size = new System.Drawing.Size(87, 30);
            this.close.TabIndex = 103;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtCellTo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtCellFrom);
            this.panel1.Controls.Add(this.comboFactory);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.radioBySummary);
            this.panel1.Controls.Add(this.radioByOneDayDetial);
            this.panel1.Controls.Add(this.radioByDetail);
            this.panel1.Controls.Add(this.comboM);
            this.panel1.Controls.Add(this.dateCuttingDate);
            this.panel1.Controls.Add(this.labelType);
            this.panel1.Controls.Add(this.labelCuttingDate);
            this.panel1.Controls.Add(this.labelM);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(413, 218);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(247, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 23);
            this.label2.TabIndex = 114;
            this.label2.Text = "～";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 23);
            this.label1.TabIndex = 113;
            this.label1.Text = "Cut Cell";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(123, 89);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 4;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(12, 89);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(99, 23);
            this.labelFactory.TabIndex = 110;
            this.labelFactory.Text = "Factory";
            // 
            // radioBySummary
            // 
            this.radioBySummary.AutoSize = true;
            this.radioBySummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBySummary.Location = new System.Drawing.Point(123, 173);
            this.radioBySummary.Name = "radioBySummary";
            this.radioBySummary.Size = new System.Drawing.Size(105, 21);
            this.radioBySummary.TabIndex = 7;
            this.radioBySummary.Text = "By Summary";
            this.radioBySummary.UseVisualStyleBackColor = true;
            // 
            // radioByOneDayDetial
            // 
            this.radioByOneDayDetial.AutoSize = true;
            this.radioByOneDayDetial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByOneDayDetial.Location = new System.Drawing.Point(123, 146);
            this.radioByOneDayDetial.Name = "radioByOneDayDetial";
            this.radioByOneDayDetial.Size = new System.Drawing.Size(142, 21);
            this.radioByOneDayDetial.TabIndex = 6;
            this.radioByOneDayDetial.Text = "By One Day Detail";
            this.radioByOneDayDetial.UseVisualStyleBackColor = true;
            this.radioByOneDayDetial.CheckedChanged += new System.EventHandler(this.RadioByOneDayDetial_CheckedChanged);
            // 
            // radioByDetail
            // 
            this.radioByDetail.AutoSize = true;
            this.radioByDetail.Checked = true;
            this.radioByDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByDetail.Location = new System.Drawing.Point(123, 119);
            this.radioByDetail.Name = "radioByDetail";
            this.radioByDetail.Size = new System.Drawing.Size(82, 21);
            this.radioByDetail.TabIndex = 5;
            this.radioByDetail.TabStop = true;
            this.radioByDetail.Text = "By Detail";
            this.radioByDetail.UseVisualStyleBackColor = true;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(123, 62);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 3;
            this.comboM.TextChanged += new System.EventHandler(this.ComboM_TextChanged);
            // 
            // dateCuttingDate
            // 
            // 
            // 
            // 
            this.dateCuttingDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCuttingDate.DateBox1.Name = "";
            this.dateCuttingDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCuttingDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCuttingDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCuttingDate.DateBox2.Name = "";
            this.dateCuttingDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCuttingDate.DateBox2.TabIndex = 1;
            this.dateCuttingDate.IsRequired = false;
            this.dateCuttingDate.Location = new System.Drawing.Point(123, 7);
            this.dateCuttingDate.Name = "dateCuttingDate";
            this.dateCuttingDate.Size = new System.Drawing.Size(280, 23);
            this.dateCuttingDate.TabIndex = 0;
            this.dateCuttingDate.Leave += new System.EventHandler(this.DateCuttingDate_Leave);
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(12, 117);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(99, 23);
            this.labelType.TabIndex = 99;
            this.labelType.Text = "Type";
            // 
            // labelCuttingDate
            // 
            this.labelCuttingDate.Location = new System.Drawing.Point(12, 7);
            this.labelCuttingDate.Name = "labelCuttingDate";
            this.labelCuttingDate.Size = new System.Drawing.Size(99, 23);
            this.labelCuttingDate.TabIndex = 97;
            this.labelCuttingDate.Text = "Cutting Date";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(12, 62);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(99, 23);
            this.labelM.TabIndex = 96;
            this.labelM.Text = "M";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(442, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 22);
            this.label4.TabIndex = 96;
            this.label4.Text = "Paper Size A4";
            // 
            // btnSendMail
            // 
            this.btnSendMail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendMail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnSendMail.Location = new System.Drawing.Point(462, 85);
            this.btnSendMail.Name = "btnSendMail";
            this.btnSendMail.Size = new System.Drawing.Size(87, 30);
            this.btnSendMail.TabIndex = 102;
            this.btnSendMail.Text = "Send Mail";
            this.btnSendMail.UseVisualStyleBackColor = true;
            this.btnSendMail.Click += new System.EventHandler(this.BtnSendMail_Click);
            // 
            // txtCellTo
            // 
            this.txtCellTo.BackColor = System.Drawing.Color.White;
            this.txtCellTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCellTo.Location = new System.Drawing.Point(271, 35);
            this.txtCellTo.MDivisionID = "";
            this.txtCellTo.Name = "txtCellTo";
            this.txtCellTo.Size = new System.Drawing.Size(121, 23);
            this.txtCellTo.TabIndex = 2;
            // 
            // txtCellFrom
            // 
            this.txtCellFrom.BackColor = System.Drawing.Color.White;
            this.txtCellFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCellFrom.Location = new System.Drawing.Point(123, 35);
            this.txtCellFrom.MDivisionID = "";
            this.txtCellFrom.Name = "txtCellFrom";
            this.txtCellFrom.Size = new System.Drawing.Size(121, 23);
            this.txtCellFrom.TabIndex = 1;
            // 
            // R02
            // 
            this.ClientSize = new System.Drawing.Size(561, 271);
            this.Controls.Add(this.btnSendMail);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "dateCuttingDate";
            this.DefaultControlForEdit = "dateCuttingDate";
            this.Name = "R02";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R02.Cutting Daily Plan Summary Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.btnSendMail, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Label label4;
        private Win.UI.Label labelType;
        private Win.UI.Label labelCuttingDate;
        private Win.UI.Label labelM;
        private Win.UI.DateRange dateCuttingDate;
        private Win.UI.RadioButton radioBySummary;
        private Win.UI.RadioButton radioByOneDayDetial;
        private Win.UI.RadioButton radioByDetail;
        private Win.UI.ComboBox comboM;
        private Win.UI.Button btnSendMail;
        private Win.UI.Label labelFactory;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.Label label1;
        private Class.TxtCell txtCellFrom;
        private Class.TxtCell txtCellTo;
        private Win.UI.Label label2;
    }
}
