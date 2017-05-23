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
            this.radioBySummary = new Sci.Win.UI.RadioButton();
            this.radioByOneDayDetial = new Sci.Win.UI.RadioButton();
            this.radioByDetail = new Sci.Win.UI.RadioButton();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.txtCutCellEnd = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.txtCutCellStart = new Sci.Win.UI.TextBox();
            this.dateCuttingDate = new Sci.Win.UI.DateRange();
            this.labelType = new Sci.Win.UI.Label();
            this.labelCutCell = new Sci.Win.UI.Label();
            this.labelCuttingDate = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.btnSendMail = new Sci.Win.UI.Button();
            this.labelFactory = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(446, 12);
            this.print.Size = new System.Drawing.Size(87, 30);
            this.print.TabIndex = 1;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(446, 48);
            this.toexcel.Size = new System.Drawing.Size(87, 30);
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(446, 120);
            this.close.Size = new System.Drawing.Size(87, 30);
            this.close.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboFactory);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.radioBySummary);
            this.panel1.Controls.Add(this.radioByOneDayDetial);
            this.panel1.Controls.Add(this.radioByDetail);
            this.panel1.Controls.Add(this.comboM);
            this.panel1.Controls.Add(this.txtCutCellEnd);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtCutCellStart);
            this.panel1.Controls.Add(this.dateCuttingDate);
            this.panel1.Controls.Add(this.labelType);
            this.panel1.Controls.Add(this.labelCutCell);
            this.panel1.Controls.Add(this.labelCuttingDate);
            this.panel1.Controls.Add(this.labelM);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(413, 240);
            this.panel1.TabIndex = 0;
            // 
            // radioBySummary
            // 
            this.radioBySummary.AutoSize = true;
            this.radioBySummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBySummary.Location = new System.Drawing.Point(123, 208);
            this.radioBySummary.Name = "radioBySummary";
            this.radioBySummary.Size = new System.Drawing.Size(105, 21);
            this.radioBySummary.TabIndex = 6;
            this.radioBySummary.TabStop = true;
            this.radioBySummary.Text = "By Summary";
            this.radioBySummary.UseVisualStyleBackColor = true;
            // 
            // radioByOneDayDetial
            // 
            this.radioByOneDayDetial.AutoSize = true;
            this.radioByOneDayDetial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByOneDayDetial.Location = new System.Drawing.Point(123, 181);
            this.radioByOneDayDetial.Name = "radioByOneDayDetial";
            this.radioByOneDayDetial.Size = new System.Drawing.Size(135, 21);
            this.radioByOneDayDetial.TabIndex = 5;
            this.radioByOneDayDetial.TabStop = true;
            this.radioByOneDayDetial.Text = "By one day detial";
            this.radioByOneDayDetial.UseVisualStyleBackColor = true;
            this.radioByOneDayDetial.CheckedChanged += new System.EventHandler(this.radioByOneDayDetial_CheckedChanged);
            // 
            // radioByDetail
            // 
            this.radioByDetail.AutoSize = true;
            this.radioByDetail.Checked = true;
            this.radioByDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByDetail.Location = new System.Drawing.Point(123, 154);
            this.radioByDetail.Name = "radioByDetail";
            this.radioByDetail.Size = new System.Drawing.Size(82, 21);
            this.radioByDetail.TabIndex = 4;
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
            this.comboM.Location = new System.Drawing.Point(123, 79);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 3;
            // 
            // txtCutCellEnd
            // 
            this.txtCutCellEnd.BackColor = System.Drawing.Color.White;
            this.txtCutCellEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutCellEnd.Location = new System.Drawing.Point(214, 43);
            this.txtCutCellEnd.MaxLength = 2;
            this.txtCutCellEnd.Name = "txtCutCellEnd";
            this.txtCutCellEnd.Size = new System.Drawing.Size(65, 23);
            this.txtCutCellEnd.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(191, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 23);
            this.label9.TabIndex = 109;
            this.label9.Text = "～";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtCutCellStart
            // 
            this.txtCutCellStart.BackColor = System.Drawing.Color.White;
            this.txtCutCellStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutCellStart.Location = new System.Drawing.Point(123, 43);
            this.txtCutCellStart.MaxLength = 2;
            this.txtCutCellStart.Name = "txtCutCellStart";
            this.txtCutCellStart.Size = new System.Drawing.Size(65, 23);
            this.txtCutCellStart.TabIndex = 1;
            // 
            // dateCuttingDate
            // 
            this.dateCuttingDate.IsRequired = false;
            this.dateCuttingDate.Location = new System.Drawing.Point(123, 7);
            this.dateCuttingDate.Name = "dateCuttingDate";
            this.dateCuttingDate.Size = new System.Drawing.Size(280, 23);
            this.dateCuttingDate.TabIndex = 0;
            this.dateCuttingDate.Leave += new System.EventHandler(this.dateCuttingDate_Leave);
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(12, 152);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(99, 23);
            this.labelType.TabIndex = 99;
            this.labelType.Text = "Type";
            // 
            // labelCutCell
            // 
            this.labelCutCell.Location = new System.Drawing.Point(12, 43);
            this.labelCutCell.Name = "labelCutCell";
            this.labelCutCell.Size = new System.Drawing.Size(99, 23);
            this.labelCutCell.TabIndex = 98;
            this.labelCutCell.Text = "Cut Cell";
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
            this.labelM.Location = new System.Drawing.Point(12, 79);
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
            this.btnSendMail.Location = new System.Drawing.Point(446, 85);
            this.btnSendMail.Name = "btnSendMail";
            this.btnSendMail.Size = new System.Drawing.Size(87, 30);
            this.btnSendMail.TabIndex = 3;
            this.btnSendMail.Text = "Send Mail";
            this.btnSendMail.UseVisualStyleBackColor = true;
            this.btnSendMail.Click += new System.EventHandler(this.btnSendMail_Click);
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(12, 115);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(99, 23);
            this.labelFactory.TabIndex = 110;
            this.labelFactory.Text = "Factory";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(123, 115);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 111;
            // 
            // R02
            // 
            this.ClientSize = new System.Drawing.Size(545, 287);
            this.Controls.Add(this.btnSendMail);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "dateCuttingDate";
            this.DefaultControlForEdit = "dateCuttingDate";
            this.Name = "R02";
            this.Text = "R02.Cutting Daily Plan Summary Report";
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
        private Win.UI.Label labelCutCell;
        private Win.UI.Label labelCuttingDate;
        private Win.UI.Label labelM;
        private Win.UI.DateRange dateCuttingDate;
        private Win.UI.RadioButton radioBySummary;
        private Win.UI.RadioButton radioByOneDayDetial;
        private Win.UI.RadioButton radioByDetail;
        private Win.UI.ComboBox comboM;
        private Win.UI.TextBox txtCutCellEnd;
        private Win.UI.Label label9;
        private Win.UI.TextBox txtCutCellStart;
        private Win.UI.Button btnSendMail;
        private Win.UI.Label labelFactory;
        private Win.UI.ComboBox comboFactory;

    }
}
