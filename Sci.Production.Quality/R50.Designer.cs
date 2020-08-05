namespace Sci.Production.Quality
{
    partial class R50
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
            this.components = new System.ComponentModel.Container();
            this.dateInspectionDate = new Sci.Win.UI.DateRange();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.comboShift = new Sci.Win.UI.ComboBox();
            this.comboFactory1 = new Sci.Production.Class.ComboFactory(this.components);
            this.comboMDivision1 = new Sci.Production.Class.ComboMDivision(this.components);
            this.txtstyle1 = new Sci.Production.Class.Txtstyle();
            this.txtShiftTime1 = new Sci.Win.UI.TextBox();
            this.txtShiftTime2 = new Sci.Win.UI.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(430, 106);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(430, 12);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(430, 48);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(387, 142);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(413, 178);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(410, 205);
            // 
            // dateInspectionDate
            // 
            // 
            // 
            // 
            this.dateInspectionDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInspectionDate.DateBox1.Name = "";
            this.dateInspectionDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateInspectionDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInspectionDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateInspectionDate.DateBox2.Name = "";
            this.dateInspectionDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateInspectionDate.DateBox2.TabIndex = 1;
            this.dateInspectionDate.IsRequired = false;
            this.dateInspectionDate.Location = new System.Drawing.Point(126, 12);
            this.dateInspectionDate.Name = "dateInspectionDate";
            this.dateInspectionDate.Size = new System.Drawing.Size(280, 23);
            this.dateInspectionDate.TabIndex = 0;
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(126, 41);
            this.txtSP.MaxLength = 13;
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(116, 23);
            this.txtSP.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.Location = new System.Drawing.Point(418, 81);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 22);
            this.label10.TabIndex = 97;
            this.label10.Text = "Paper Size A4";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(18, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 23);
            this.label2.TabIndex = 120;
            this.label2.Text = "Style";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 23);
            this.label1.TabIndex = 122;
            this.label1.Text = "M";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(18, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 23);
            this.label3.TabIndex = 124;
            this.label3.Text = "Factory";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(18, 41);
            this.label4.Name = "label4";
            this.label4.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label4.Size = new System.Drawing.Size(105, 23);
            this.label4.TabIndex = 125;
            this.label4.Text = "SP#";
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(18, 12);
            this.label5.Name = "label5";
            this.label5.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label5.Size = new System.Drawing.Size(105, 23);
            this.label5.TabIndex = 126;
            this.label5.Text = "Inspection Date";
            this.label5.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(18, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 23);
            this.label6.TabIndex = 127;
            this.label6.Text = "Shift";
            // 
            // comboShift
            // 
            this.comboShift.BackColor = System.Drawing.Color.White;
            this.comboShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShift.FormattingEnabled = true;
            this.comboShift.IsSupportUnselect = true;
            this.comboShift.Location = new System.Drawing.Point(126, 160);
            this.comboShift.Name = "comboShift";
            this.comboShift.OldText = "";
            this.comboShift.Size = new System.Drawing.Size(80, 24);
            this.comboShift.TabIndex = 128;
            this.comboShift.SelectedIndexChanged += new System.EventHandler(this.ComboShift_SelectedIndexChanged);
            // 
            // comboFactory1
            // 
            this.comboFactory1.BackColor = System.Drawing.Color.White;
            this.comboFactory1.FilteMDivision = false;
            this.comboFactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory1.FormattingEnabled = true;
            this.comboFactory1.IssupportJunk = false;
            this.comboFactory1.IsSupportUnselect = true;
            this.comboFactory1.Location = new System.Drawing.Point(126, 129);
            this.comboFactory1.Name = "comboFactory1";
            this.comboFactory1.OldText = "";
            this.comboFactory1.Size = new System.Drawing.Size(80, 24);
            this.comboFactory1.TabIndex = 4;
            // 
            // comboMDivision1
            // 
            this.comboMDivision1.BackColor = System.Drawing.Color.White;
            this.comboMDivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMDivision1.FormattingEnabled = true;
            this.comboMDivision1.IsSupportUnselect = true;
            this.comboMDivision1.Location = new System.Drawing.Point(126, 99);
            this.comboMDivision1.Name = "comboMDivision1";
            this.comboMDivision1.OldText = "";
            this.comboMDivision1.Size = new System.Drawing.Size(80, 24);
            this.comboMDivision1.TabIndex = 3;
            // 
            // txtstyle1
            // 
            this.txtstyle1.BackColor = System.Drawing.Color.White;
            this.txtstyle1.BrandObjectName = null;
            this.txtstyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle1.Location = new System.Drawing.Point(126, 70);
            this.txtstyle1.Name = "txtstyle1";
            this.txtstyle1.Size = new System.Drawing.Size(130, 23);
            this.txtstyle1.TabIndex = 2;
            this.txtstyle1.TarBrand = null;
            this.txtstyle1.TarSeason = null;
            // 
            // txtShiftTime1
            // 
            this.txtShiftTime1.BackColor = System.Drawing.Color.White;
            this.txtShiftTime1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShiftTime1.Location = new System.Drawing.Point(212, 160);
            this.txtShiftTime1.Mask = "90:00";
            this.txtShiftTime1.Name = "txtShiftTime1";
            this.txtShiftTime1.Size = new System.Drawing.Size(68, 23);
            this.txtShiftTime1.TabIndex = 129;
            this.txtShiftTime1.ValidatingType = typeof(System.DateTime);
            this.txtShiftTime1.Validating += new System.ComponentModel.CancelEventHandler(this.TxtShiftTime_Validating);
            // 
            // txtShiftTime2
            // 
            this.txtShiftTime2.BackColor = System.Drawing.Color.White;
            this.txtShiftTime2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShiftTime2.Location = new System.Drawing.Point(306, 161);
            this.txtShiftTime2.Mask = "90:00";
            this.txtShiftTime2.Name = "txtShiftTime2";
            this.txtShiftTime2.Size = new System.Drawing.Size(68, 23);
            this.txtShiftTime2.TabIndex = 130;
            this.txtShiftTime2.ValidatingType = typeof(System.DateTime);
            this.txtShiftTime2.Validating += new System.ComponentModel.CancelEventHandler(this.TxtShiftTime_Validating);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(282, 164);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 17);
            this.label7.TabIndex = 131;
            this.label7.Text = "～";
            // 
            // R50
            // 
            this.ClientSize = new System.Drawing.Size(522, 260);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtShiftTime2);
            this.Controls.Add(this.txtShiftTime1);
            this.Controls.Add(this.comboShift);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.comboFactory1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboMDivision1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtstyle1);
            this.Controls.Add(this.dateInspectionDate);
            this.Controls.Add(this.txtSP);
            this.Name = "R50";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R50. Cutting Inspection Report";
            this.Controls.SetChildIndex(this.txtSP, 0);
            this.Controls.SetChildIndex(this.dateInspectionDate, 0);
            this.Controls.SetChildIndex(this.txtstyle1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.comboMDivision1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.comboFactory1, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.comboShift, 0);
            this.Controls.SetChildIndex(this.txtShiftTime1, 0);
            this.Controls.SetChildIndex(this.txtShiftTime2, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.TextBox txtSP;
        private Win.UI.DateRange dateInspectionDate;
        private Win.UI.Label label10;
        private Win.UI.Label label3;
        private Class.ComboFactory comboFactory1;
        private Win.UI.Label label1;
        private Class.ComboMDivision comboMDivision1;
        private Win.UI.Label label2;
        private Class.Txtstyle txtstyle1;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.ComboBox comboShift;
        private Win.UI.TextBox txtShiftTime1;
        private Win.UI.TextBox txtShiftTime2;
        private System.Windows.Forms.Label label7;
    }
}
