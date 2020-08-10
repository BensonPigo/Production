namespace Sci.Production.Warehouse
{
    partial class R05
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.dateIssue = new Sci.Win.UI.DateRange();
            this.txtSP1 = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.txtSP2 = new Sci.Win.UI.TextBox();
            this.txtMdivision1 = new Sci.Production.Class.TxtMdivision();
            this.txtfactory1 = new Sci.Production.Class.Txtfactory();
            this.radioTransferIn = new Sci.Win.UI.RadioButton();
            this.radioTransferOut = new Sci.Win.UI.RadioButton();
            this.lbReportType = new Sci.Win.UI.Label();
            this.lbMaterialType = new Sci.Win.UI.Label();
            this.comboMaterialType = new Sci.Win.UI.ComboBox();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioSummary = new Sci.Win.UI.RadioButton();
            this.radioDetail = new Sci.Win.UI.RadioButton();
            this.radioPanel2 = new Sci.Win.UI.RadioPanel();
            this.radioPanel1.SuspendLayout();
            this.radioPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(435, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(435, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(435, 84);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SkyBlue;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Issue Date";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 23);
            this.label2.TabIndex = 95;
            this.label2.Text = "SP#";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 23);
            this.label3.TabIndex = 96;
            this.label3.Text = "M";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 23);
            this.label4.TabIndex = 97;
            this.label4.Text = "Factory";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 23);
            this.label5.TabIndex = 98;
            this.label5.Text = "Transfer Type";
            // 
            // dateIssue
            // 
            // 
            // 
            // 
            this.dateIssue.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateIssue.DateBox1.Name = "";
            this.dateIssue.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateIssue.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateIssue.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateIssue.DateBox2.Name = "";
            this.dateIssue.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateIssue.DateBox2.TabIndex = 1;
            this.dateIssue.Location = new System.Drawing.Point(105, 9);
            this.dateIssue.Name = "dateIssue";
            this.dateIssue.Size = new System.Drawing.Size(280, 23);
            this.dateIssue.TabIndex = 99;
            // 
            // txtSP1
            // 
            this.txtSP1.BackColor = System.Drawing.Color.White;
            this.txtSP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP1.Location = new System.Drawing.Point(105, 44);
            this.txtSP1.Name = "txtSP1";
            this.txtSP1.Size = new System.Drawing.Size(100, 23);
            this.txtSP1.TabIndex = 100;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(208, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 23);
            this.label6.TabIndex = 101;
            this.label6.Text = "～";
            // 
            // txtSP2
            // 
            this.txtSP2.BackColor = System.Drawing.Color.White;
            this.txtSP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP2.Location = new System.Drawing.Point(234, 44);
            this.txtSP2.Name = "txtSP2";
            this.txtSP2.Size = new System.Drawing.Size(100, 23);
            this.txtSP2.TabIndex = 102;
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(105, 79);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 103;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.BoolFtyGroupList = true;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IsProduceFty = false;
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(105, 114);
            this.txtfactory1.MDivision = null;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 104;
            // 
            // radioTransferIn
            // 
            this.radioTransferIn.AutoSize = true;
            this.radioTransferIn.Checked = true;
            this.radioTransferIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioTransferIn.Location = new System.Drawing.Point(10, 5);
            this.radioTransferIn.Name = "radioTransferIn";
            this.radioTransferIn.Size = new System.Drawing.Size(95, 21);
            this.radioTransferIn.TabIndex = 105;
            this.radioTransferIn.TabStop = true;
            this.radioTransferIn.Text = "Transfer In";
            this.radioTransferIn.UseVisualStyleBackColor = true;
            // 
            // radioTransferOut
            // 
            this.radioTransferOut.AutoSize = true;
            this.radioTransferOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioTransferOut.Location = new System.Drawing.Point(111, 5);
            this.radioTransferOut.Name = "radioTransferOut";
            this.radioTransferOut.Size = new System.Drawing.Size(107, 21);
            this.radioTransferOut.TabIndex = 106;
            this.radioTransferOut.Text = "Transfer Out";
            this.radioTransferOut.UseVisualStyleBackColor = true;
            // 
            // lbReportType
            // 
            this.lbReportType.Location = new System.Drawing.Point(9, 184);
            this.lbReportType.Name = "lbReportType";
            this.lbReportType.Size = new System.Drawing.Size(93, 23);
            this.lbReportType.TabIndex = 107;
            this.lbReportType.Text = "Report Type";
            // 
            // lbMaterialType
            // 
            this.lbMaterialType.Location = new System.Drawing.Point(9, 219);
            this.lbMaterialType.Name = "lbMaterialType";
            this.lbMaterialType.Size = new System.Drawing.Size(93, 23);
            this.lbMaterialType.TabIndex = 108;
            this.lbMaterialType.Text = "Material Type";
            // 
            // comboMaterialType
            // 
            this.comboMaterialType.BackColor = System.Drawing.Color.White;
            this.comboMaterialType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMaterialType.FormattingEnabled = true;
            this.comboMaterialType.IsSupportUnselect = true;
            this.comboMaterialType.Location = new System.Drawing.Point(105, 219);
            this.comboMaterialType.Name = "comboMaterialType";
            this.comboMaterialType.OldText = "";
            this.comboMaterialType.Size = new System.Drawing.Size(121, 24);
            this.comboMaterialType.TabIndex = 111;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioSummary);
            this.radioPanel1.Controls.Add(this.radioDetail);
            this.radioPanel1.Location = new System.Drawing.Point(105, 184);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(175, 28);
            this.radioPanel1.TabIndex = 112;
            // 
            // radioSummary
            // 
            this.radioSummary.AutoSize = true;
            this.radioSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSummary.Location = new System.Drawing.Point(78, 3);
            this.radioSummary.Name = "radioSummary";
            this.radioSummary.Size = new System.Drawing.Size(85, 21);
            this.radioSummary.TabIndex = 1;
            this.radioSummary.Text = "Summary";
            this.radioSummary.UseVisualStyleBackColor = true;
            // 
            // radioDetail
            // 
            this.radioDetail.AutoSize = true;
            this.radioDetail.Checked = true;
            this.radioDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail.Location = new System.Drawing.Point(10, 3);
            this.radioDetail.Name = "radioDetail";
            this.radioDetail.Size = new System.Drawing.Size(62, 21);
            this.radioDetail.TabIndex = 0;
            this.radioDetail.TabStop = true;
            this.radioDetail.Text = "Detail";
            this.radioDetail.UseVisualStyleBackColor = true;
            // 
            // radioPanel2
            // 
            this.radioPanel2.Controls.Add(this.radioTransferIn);
            this.radioPanel2.Controls.Add(this.radioTransferOut);
            this.radioPanel2.Location = new System.Drawing.Point(105, 149);
            this.radioPanel2.Name = "radioPanel2";
            this.radioPanel2.Size = new System.Drawing.Size(229, 29);
            this.radioPanel2.TabIndex = 113;
            // 
            // R05
            // 
            this.ClientSize = new System.Drawing.Size(527, 275);
            this.Controls.Add(this.radioPanel2);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.comboMaterialType);
            this.Controls.Add(this.lbMaterialType);
            this.Controls.Add(this.lbReportType);
            this.Controls.Add(this.txtfactory1);
            this.Controls.Add(this.txtMdivision1);
            this.Controls.Add(this.txtSP2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSP1);
            this.Controls.Add(this.dateIssue);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R05";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R05. Material Transfer In / Out Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.dateIssue, 0);
            this.Controls.SetChildIndex(this.txtSP1, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtSP2, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.txtfactory1, 0);
            this.Controls.SetChildIndex(this.lbReportType, 0);
            this.Controls.SetChildIndex(this.lbMaterialType, 0);
            this.Controls.SetChildIndex(this.comboMaterialType, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.radioPanel2, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.radioPanel2.ResumeLayout(false);
            this.radioPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.DateRange dateIssue;
        private Win.UI.TextBox txtSP1;
        private Win.UI.Label label6;
        private Win.UI.TextBox txtSP2;
        private Class.TxtMdivision txtMdivision1;
        private Class.Txtfactory txtfactory1;
        private Win.UI.RadioButton radioTransferIn;
        private Win.UI.RadioButton radioTransferOut;
        private Win.UI.Label lbReportType;
        private Win.UI.Label lbMaterialType;
        private Win.UI.ComboBox comboMaterialType;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioSummary;
        private Win.UI.RadioButton radioDetail;
        private Win.UI.RadioPanel radioPanel2;
    }
}
