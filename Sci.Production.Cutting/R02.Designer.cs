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
            this.radiobtn_BySummary = new Sci.Win.UI.RadioButton();
            this.radioBtn_Byonedaydetial = new Sci.Win.UI.RadioButton();
            this.radiobtn_Bydetail = new Sci.Win.UI.RadioButton();
            this.cmb_MDivisionID = new Sci.Win.UI.ComboBox();
            this.txt_CutCell2 = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.txt_CutCell1 = new Sci.Win.UI.TextBox();
            this.dateR_CuttingDate = new Sci.Win.UI.DateRange();
            this.label5 = new Sci.Win.UI.Label();
            this.label_CutCell = new Sci.Win.UI.Label();
            this.labelCuttingDate = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.btn_sendmail = new Sci.Win.UI.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(528, 12);
            this.print.Size = new System.Drawing.Size(87, 30);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(528, 48);
            this.toexcel.Size = new System.Drawing.Size(87, 30);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(528, 120);
            this.close.Size = new System.Drawing.Size(87, 30);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radiobtn_BySummary);
            this.panel1.Controls.Add(this.radioBtn_Byonedaydetial);
            this.panel1.Controls.Add(this.radiobtn_Bydetail);
            this.panel1.Controls.Add(this.cmb_MDivisionID);
            this.panel1.Controls.Add(this.txt_CutCell2);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txt_CutCell1);
            this.panel1.Controls.Add(this.dateR_CuttingDate);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label_CutCell);
            this.panel1.Controls.Add(this.labelCuttingDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(492, 239);
            this.panel1.TabIndex = 94;
            // 
            // radiobtn_BySummary
            // 
            this.radiobtn_BySummary.AutoSize = true;
            this.radiobtn_BySummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobtn_BySummary.Location = new System.Drawing.Point(123, 197);
            this.radiobtn_BySummary.Name = "radiobtn_BySummary";
            this.radiobtn_BySummary.Size = new System.Drawing.Size(105, 21);
            this.radiobtn_BySummary.TabIndex = 114;
            this.radiobtn_BySummary.TabStop = true;
            this.radiobtn_BySummary.Text = "By Summary";
            this.radiobtn_BySummary.UseVisualStyleBackColor = true;
            this.radiobtn_BySummary.CheckedChanged += new System.EventHandler(this.radiobtn_BySummary_CheckedChanged);
            // 
            // radioBtn_Byonedaydetial
            // 
            this.radioBtn_Byonedaydetial.AutoSize = true;
            this.radioBtn_Byonedaydetial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBtn_Byonedaydetial.Location = new System.Drawing.Point(123, 170);
            this.radioBtn_Byonedaydetial.Name = "radioBtn_Byonedaydetial";
            this.radioBtn_Byonedaydetial.Size = new System.Drawing.Size(135, 21);
            this.radioBtn_Byonedaydetial.TabIndex = 113;
            this.radioBtn_Byonedaydetial.TabStop = true;
            this.radioBtn_Byonedaydetial.Text = "By one day detial";
            this.radioBtn_Byonedaydetial.UseVisualStyleBackColor = true;
            this.radioBtn_Byonedaydetial.CheckedChanged += new System.EventHandler(this.radioBtn_Byonedaydetial_CheckedChanged);
            // 
            // radiobtn_Bydetail
            // 
            this.radiobtn_Bydetail.AutoSize = true;
            this.radiobtn_Bydetail.Checked = true;
            this.radiobtn_Bydetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobtn_Bydetail.Location = new System.Drawing.Point(123, 143);
            this.radiobtn_Bydetail.Name = "radiobtn_Bydetail";
            this.radiobtn_Bydetail.Size = new System.Drawing.Size(80, 21);
            this.radiobtn_Bydetail.TabIndex = 112;
            this.radiobtn_Bydetail.TabStop = true;
            this.radiobtn_Bydetail.Text = "By detail";
            this.radiobtn_Bydetail.UseVisualStyleBackColor = true;
            this.radiobtn_Bydetail.CheckedChanged += new System.EventHandler(this.radiobtn_Bydetail_CheckedChanged);
            // 
            // cmb_MDivisionID
            // 
            this.cmb_MDivisionID.BackColor = System.Drawing.Color.White;
            this.cmb_MDivisionID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmb_MDivisionID.FormattingEnabled = true;
            this.cmb_MDivisionID.IsSupportUnselect = true;
            this.cmb_MDivisionID.Location = new System.Drawing.Point(123, 100);
            this.cmb_MDivisionID.Name = "cmb_MDivisionID";
            this.cmb_MDivisionID.Size = new System.Drawing.Size(121, 24);
            this.cmb_MDivisionID.TabIndex = 111;
            // 
            // txt_CutCell2
            // 
            this.txt_CutCell2.BackColor = System.Drawing.Color.White;
            this.txt_CutCell2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txt_CutCell2.Location = new System.Drawing.Point(214, 59);
            this.txt_CutCell2.Name = "txt_CutCell2";
            this.txt_CutCell2.Size = new System.Drawing.Size(65, 23);
            this.txt_CutCell2.TabIndex = 110;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(191, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 23);
            this.label9.TabIndex = 109;
            this.label9.Text = "～";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txt_CutCell1
            // 
            this.txt_CutCell1.BackColor = System.Drawing.Color.White;
            this.txt_CutCell1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txt_CutCell1.Location = new System.Drawing.Point(123, 59);
            this.txt_CutCell1.Name = "txt_CutCell1";
            this.txt_CutCell1.Size = new System.Drawing.Size(65, 23);
            this.txt_CutCell1.TabIndex = 108;
            // 
            // dateR_CuttingDate
            // 
            this.dateR_CuttingDate.IsRequired = false;
            this.dateR_CuttingDate.Location = new System.Drawing.Point(123, 18);
            this.dateR_CuttingDate.Name = "dateR_CuttingDate";
            this.dateR_CuttingDate.Size = new System.Drawing.Size(280, 23);
            this.dateR_CuttingDate.TabIndex = 100;
            this.dateR_CuttingDate.Leave += new System.EventHandler(this.Leave_CuttingDate);
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(12, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 23);
            this.label5.TabIndex = 99;
            this.label5.Text = "Type";
            // 
            // label_CutCell
            // 
            this.label_CutCell.Lines = 0;
            this.label_CutCell.Location = new System.Drawing.Point(12, 59);
            this.label_CutCell.Name = "label_CutCell";
            this.label_CutCell.Size = new System.Drawing.Size(99, 23);
            this.label_CutCell.TabIndex = 98;
            this.label_CutCell.Text = "Cut Cell";
            // 
            // labelCuttingDate
            // 
            this.labelCuttingDate.Lines = 0;
            this.labelCuttingDate.Location = new System.Drawing.Point(12, 18);
            this.labelCuttingDate.Name = "labelCuttingDate";
            this.labelCuttingDate.Size = new System.Drawing.Size(99, 23);
            this.labelCuttingDate.TabIndex = 97;
            this.labelCuttingDate.Text = "Cutting Date";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(12, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 23);
            this.label1.TabIndex = 96;
            this.label1.Text = "M";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(518, 229);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 22);
            this.label4.TabIndex = 96;
            this.label4.Text = "Paper Size A4";
            // 
            // btn_sendmail
            // 
            this.btn_sendmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btn_sendmail.Location = new System.Drawing.Point(528, 84);
            this.btn_sendmail.Name = "btn_sendmail";
            this.btn_sendmail.Size = new System.Drawing.Size(87, 30);
            this.btn_sendmail.TabIndex = 97;
            this.btn_sendmail.Text = "Send Mail";
            this.btn_sendmail.UseVisualStyleBackColor = true;
            this.btn_sendmail.Click += new System.EventHandler(this.btn_sendmail_Click);
            // 
            // R02
            // 
            this.ClientSize = new System.Drawing.Size(627, 293);
            this.Controls.Add(this.btn_sendmail);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.Name = "R02";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.btn_sendmail, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label_CutCell;
        private Win.UI.Label labelCuttingDate;
        private Win.UI.Label label1;
        private Win.UI.DateRange dateR_CuttingDate;
        private Win.UI.RadioButton radiobtn_BySummary;
        private Win.UI.RadioButton radioBtn_Byonedaydetial;
        private Win.UI.RadioButton radiobtn_Bydetail;
        private Win.UI.ComboBox cmb_MDivisionID;
        private Win.UI.TextBox txt_CutCell2;
        private Win.UI.Label label9;
        private Win.UI.TextBox txt_CutCell1;
        private Win.UI.Button btn_sendmail;

    }
}
