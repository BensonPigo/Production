namespace Sci.Production.Warehouse
{
    partial class R48
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
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioSummary = new Sci.Win.UI.RadioButton();
            this.radioDetail = new Sci.Win.UI.RadioButton();
            this.labeReportType = new Sci.Win.UI.Label();
            this.txtSPNo2 = new Sci.Win.UI.TextBox();
            this.txtSPNo1 = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.checkQty = new Sci.Win.UI.CheckBox();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(501, 6);
            this.print.TabIndex = 4;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(501, 42);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(501, 78);
            this.close.TabIndex = 6;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(197, 148);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(329, 148);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(435, 148);
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(8, 69);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(85, 23);
            this.labelFactory.TabIndex = 9;
            this.labelFactory.Text = "Factory";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(8, 39);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(85, 23);
            this.labelM.TabIndex = 8;
            this.labelM.Text = "M";
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioSummary);
            this.radioPanel1.Controls.Add(this.radioDetail);
            this.radioPanel1.Location = new System.Drawing.Point(96, 98);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(323, 42);
            this.radioPanel1.TabIndex = 146;
            // 
            // radioSummary
            // 
            this.radioSummary.AutoSize = true;
            this.radioSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSummary.Location = new System.Drawing.Point(84, 3);
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
            this.radioDetail.Location = new System.Drawing.Point(16, 3);
            this.radioDetail.Name = "radioDetail";
            this.radioDetail.Size = new System.Drawing.Size(62, 21);
            this.radioDetail.TabIndex = 0;
            this.radioDetail.TabStop = true;
            this.radioDetail.Text = "Detail";
            this.radioDetail.UseVisualStyleBackColor = true;
            // 
            // labeReportType
            // 
            this.labeReportType.Location = new System.Drawing.Point(8, 100);
            this.labeReportType.Name = "labeReportType";
            this.labeReportType.Size = new System.Drawing.Size(85, 23);
            this.labeReportType.TabIndex = 10;
            this.labeReportType.Text = "Report Type:";
            // 
            // txtSPNo2
            // 
            this.txtSPNo2.BackColor = System.Drawing.Color.White;
            this.txtSPNo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo2.Location = new System.Drawing.Point(284, 10);
            this.txtSPNo2.Name = "txtSPNo2";
            this.txtSPNo2.Size = new System.Drawing.Size(164, 23);
            this.txtSPNo2.TabIndex = 1;
            // 
            // txtSPNo1
            // 
            this.txtSPNo1.BackColor = System.Drawing.Color.White;
            this.txtSPNo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo1.Location = new System.Drawing.Point(96, 10);
            this.txtSPNo1.Name = "txtSPNo1";
            this.txtSPNo1.Size = new System.Drawing.Size(164, 23);
            this.txtSPNo1.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(263, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 23);
            this.label10.TabIndex = 12;
            this.label10.Text = "～";
            this.label10.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label10.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "SP#";
            // 
            // checkQty
            // 
            this.checkQty.AutoSize = true;
            this.checkQty.Checked = true;
            this.checkQty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkQty.Location = new System.Drawing.Point(12, 136);
            this.checkQty.Name = "checkQty";
            this.checkQty.Size = new System.Drawing.Size(73, 21);
            this.checkQty.TabIndex = 11;
            this.checkQty.Text = "Qty > 0";
            this.checkQty.UseVisualStyleBackColor = true;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(96, 69);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 3;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(96, 39);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 2;
            // 
            // R48
            // 
            this.ClientSize = new System.Drawing.Size(593, 205);
            this.Controls.Add(this.checkQty);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSPNo2);
            this.Controls.Add(this.txtSPNo1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.labeReportType);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivision);
            this.Name = "R48";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R48. Bulk To Scrap Transaction List";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.labeReportType, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.txtSPNo1, 0);
            this.Controls.SetChildIndex(this.txtSPNo2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.checkQty, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Class.Txtfactory txtfactory;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelM;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioSummary;
        private Win.UI.RadioButton radioDetail;
        private Win.UI.Label labeReportType;
        private Win.UI.TextBox txtSPNo2;
        private Win.UI.TextBox txtSPNo1;
        private Win.UI.Label label10;
        private Win.UI.Label label1;
        private Win.UI.CheckBox checkQty;
    }
}
