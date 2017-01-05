namespace Sci.Production.Cutting
{
    partial class R04
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
            this.radioBtn_ByDetail = new Sci.Win.UI.RadioButton();
            this.radioBtn_ByCutCell = new Sci.Win.UI.RadioButton();
            this.radiobtn_ByM = new Sci.Win.UI.RadioButton();
            this.cmb_M = new Sci.Win.UI.ComboBox();
            this.txt_CutCell2 = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.txt_CutCell1 = new Sci.Win.UI.TextBox();
            this.dateR_EstCutDate = new Sci.Win.UI.DateRange();
            this.label5 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(441, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(441, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(441, 84);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.radioBtn_ByDetail);
            this.panel1.Controls.Add(this.radioBtn_ByCutCell);
            this.panel1.Controls.Add(this.radiobtn_ByM);
            this.panel1.Controls.Add(this.cmb_M);
            this.panel1.Controls.Add(this.txt_CutCell2);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txt_CutCell1);
            this.panel1.Controls.Add(this.dateR_EstCutDate);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(412, 191);
            this.panel1.TabIndex = 95;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(11, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 23);
            this.label2.TabIndex = 115;
            this.label2.Text = "Est. Cut Date";
            // 
            // radioBtn_ByDetail
            // 
            this.radioBtn_ByDetail.AutoSize = true;
            this.radioBtn_ByDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBtn_ByDetail.Location = new System.Drawing.Point(109, 157);
            this.radioBtn_ByDetail.Name = "radioBtn_ByDetail";
            this.radioBtn_ByDetail.Size = new System.Drawing.Size(82, 21);
            this.radioBtn_ByDetail.TabIndex = 114;
            this.radioBtn_ByDetail.Text = "By Detail";
            this.radioBtn_ByDetail.UseVisualStyleBackColor = true;
            this.radioBtn_ByDetail.CheckedChanged += new System.EventHandler(this.radioBtn3_ByDetail_CheckedChanged);
            // 
            // radioBtn_ByCutCell
            // 
            this.radioBtn_ByCutCell.AutoSize = true;
            this.radioBtn_ByCutCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBtn_ByCutCell.Location = new System.Drawing.Point(109, 130);
            this.radioBtn_ByCutCell.Name = "radioBtn_ByCutCell";
            this.radioBtn_ByCutCell.Size = new System.Drawing.Size(94, 21);
            this.radioBtn_ByCutCell.TabIndex = 113;
            this.radioBtn_ByCutCell.Text = "By Cut Cell";
            this.radioBtn_ByCutCell.UseVisualStyleBackColor = true;
            this.radioBtn_ByCutCell.CheckedChanged += new System.EventHandler(this.radioBtn2_ByCutCell_CheckedChanged);
            // 
            // radiobtn_ByM
            // 
            this.radiobtn_ByM.AutoSize = true;
            this.radiobtn_ByM.Checked = true;
            this.radiobtn_ByM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobtn_ByM.Location = new System.Drawing.Point(109, 103);
            this.radiobtn_ByM.Name = "radiobtn_ByM";
            this.radiobtn_ByM.Size = new System.Drawing.Size(57, 21);
            this.radiobtn_ByM.TabIndex = 112;
            this.radiobtn_ByM.TabStop = true;
            this.radiobtn_ByM.Text = "By M";
            this.radiobtn_ByM.UseVisualStyleBackColor = true;
            this.radiobtn_ByM.CheckedChanged += new System.EventHandler(this.radiobtn1_ByM_CheckedChanged);
            // 
            // cmb_M
            // 
            this.cmb_M.BackColor = System.Drawing.Color.White;
            this.cmb_M.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmb_M.FormattingEnabled = true;
            this.cmb_M.IsSupportUnselect = true;
            this.cmb_M.Location = new System.Drawing.Point(109, 12);
            this.cmb_M.Name = "cmb_M";
            this.cmb_M.Size = new System.Drawing.Size(121, 24);
            this.cmb_M.TabIndex = 111;
            // 
            // txt_CutCell2
            // 
            this.txt_CutCell2.BackColor = System.Drawing.Color.White;
            this.txt_CutCell2.Enabled = false;
            this.txt_CutCell2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txt_CutCell2.Location = new System.Drawing.Point(194, 73);
            this.txt_CutCell2.MaxLength = 2;
            this.txt_CutCell2.Name = "txt_CutCell2";
            this.txt_CutCell2.Size = new System.Drawing.Size(65, 23);
            this.txt_CutCell2.TabIndex = 110;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(171, 73);
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
            this.txt_CutCell1.Enabled = false;
            this.txt_CutCell1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txt_CutCell1.Location = new System.Drawing.Point(109, 73);
            this.txt_CutCell1.MaxLength = 2;
            this.txt_CutCell1.Name = "txt_CutCell1";
            this.txt_CutCell1.Size = new System.Drawing.Size(65, 23);
            this.txt_CutCell1.TabIndex = 108;
            // 
            // dateR_EstCutDate
            // 
            this.dateR_EstCutDate.Location = new System.Drawing.Point(109, 43);
            this.dateR_EstCutDate.Name = "dateR_EstCutDate";
            this.dateR_EstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateR_EstCutDate.TabIndex = 100;
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(11, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 23);
            this.label5.TabIndex = 99;
            this.label5.Text = "Report Type";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(11, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 23);
            this.label3.TabIndex = 98;
            this.label3.Text = "Cut Cell";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(11, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 23);
            this.label1.TabIndex = 96;
            this.label1.Text = "M";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(429, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 22);
            this.label4.TabIndex = 96;
            this.label4.Text = "Paper Size A4";
            // 
            // R04
            // 
            this.ClientSize = new System.Drawing.Size(533, 242);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.Name = "R04";
            this.Text = "Cutting_R04_CuttingBCSReport";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.RadioButton radioBtn_ByDetail;
        private Win.UI.RadioButton radioBtn_ByCutCell;
        private Win.UI.RadioButton radiobtn_ByM;
        private Win.UI.ComboBox cmb_M;
        private Win.UI.TextBox txt_CutCell2;
        private Win.UI.Label label9;
        private Win.UI.TextBox txt_CutCell1;
        private Win.UI.DateRange dateR_EstCutDate;
        private Win.UI.Label label5;
        private Win.UI.Label label3;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label4;

    }
}
