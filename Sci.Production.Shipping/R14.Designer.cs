namespace Sci.Production.Shipping
{
    partial class R14
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
            this.labelReportContent = new Sci.Win.UI.Label();
            this.labelPulloutDate = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelShipMode = new Sci.Win.UI.Label();
            this.datePulloutDate = new Sci.Win.UI.DateRange();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtshipmode = new Sci.Production.Class.Txtshipmode();
            this.dateETD = new Sci.Win.UI.DateRange();
            this.labelOnBoardDate = new Sci.Win.UI.Label();
            this.dateInvoice = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.cmbStatus = new Sci.Win.UI.ComboBox();
            this.txtShipPlanID1 = new Sci.Win.UI.TextBox();
            this.label7 = new Sci.Win.UI.Label();
            this.txtShipPlanID2 = new Sci.Win.UI.TextBox();
            this.rdbtnDetail = new Sci.Win.UI.RadioButton();
            this.rdbtnSummary = new Sci.Win.UI.RadioButton();
            this.rdbtnContainer = new Sci.Win.UI.RadioButton();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(438, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(438, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(438, 84);
            // 
            // labelReportContent
            // 
            this.labelReportContent.Location = new System.Drawing.Point(13, 12);
            this.labelReportContent.Name = "labelReportContent";
            this.labelReportContent.Size = new System.Drawing.Size(101, 23);
            this.labelReportContent.TabIndex = 94;
            this.labelReportContent.Text = "Ship Plan ID";
            // 
            // labelPulloutDate
            // 
            this.labelPulloutDate.Location = new System.Drawing.Point(13, 111);
            this.labelPulloutDate.Name = "labelPulloutDate";
            this.labelPulloutDate.Size = new System.Drawing.Size(101, 23);
            this.labelPulloutDate.TabIndex = 95;
            this.labelPulloutDate.Text = "Pullout Date";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 45);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(101, 23);
            this.labelBrand.TabIndex = 96;
            this.labelBrand.Text = "Brand";
            // 
            // labelShipMode
            // 
            this.labelShipMode.Location = new System.Drawing.Point(13, 178);
            this.labelShipMode.Name = "labelShipMode";
            this.labelShipMode.Size = new System.Drawing.Size(101, 23);
            this.labelShipMode.TabIndex = 99;
            this.labelShipMode.Text = "Ship Mode";
            // 
            // datePulloutDate
            // 
            // 
            // 
            // 
            this.datePulloutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.datePulloutDate.DateBox1.Name = "";
            this.datePulloutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.datePulloutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.datePulloutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.datePulloutDate.DateBox2.Name = "";
            this.datePulloutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.datePulloutDate.DateBox2.TabIndex = 1;
            this.datePulloutDate.IsRequired = false;
            this.datePulloutDate.Location = new System.Drawing.Point(122, 111);
            this.datePulloutDate.Name = "datePulloutDate";
            this.datePulloutDate.Size = new System.Drawing.Size(280, 23);
            this.datePulloutDate.TabIndex = 102;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(122, 45);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 103;
            // 
            // txtshipmode
            // 
            this.txtshipmode.BackColor = System.Drawing.Color.White;
            this.txtshipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshipmode.FormattingEnabled = true;
            this.txtshipmode.IsSupportUnselect = true;
            this.txtshipmode.Location = new System.Drawing.Point(122, 177);
            this.txtshipmode.Name = "txtshipmode";
            this.txtshipmode.OldText = "";
            this.txtshipmode.Size = new System.Drawing.Size(80, 24);
            this.txtshipmode.TabIndex = 106;
            this.txtshipmode.UseFunction = null;
            // 
            // dateETD
            // 
            // 
            // 
            // 
            this.dateETD.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateETD.DateBox1.Name = "";
            this.dateETD.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateETD.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateETD.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateETD.DateBox2.Name = "";
            this.dateETD.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateETD.DateBox2.TabIndex = 1;
            this.dateETD.IsRequired = false;
            this.dateETD.Location = new System.Drawing.Point(122, 144);
            this.dateETD.Name = "dateETD";
            this.dateETD.Size = new System.Drawing.Size(280, 23);
            this.dateETD.TabIndex = 115;
            // 
            // labelOnBoardDate
            // 
            this.labelOnBoardDate.Location = new System.Drawing.Point(13, 144);
            this.labelOnBoardDate.Name = "labelOnBoardDate";
            this.labelOnBoardDate.Size = new System.Drawing.Size(101, 23);
            this.labelOnBoardDate.TabIndex = 114;
            this.labelOnBoardDate.Text = "ETD";
            // 
            // dateInvoice
            // 
            // 
            // 
            // 
            this.dateInvoice.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInvoice.DateBox1.Name = "";
            this.dateInvoice.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateInvoice.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInvoice.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateInvoice.DateBox2.Name = "";
            this.dateInvoice.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateInvoice.DateBox2.TabIndex = 1;
            this.dateInvoice.IsRequired = false;
            this.dateInvoice.Location = new System.Drawing.Point(122, 78);
            this.dateInvoice.Name = "dateInvoice";
            this.dateInvoice.Size = new System.Drawing.Size(280, 23);
            this.dateInvoice.TabIndex = 116;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 23);
            this.label1.TabIndex = 117;
            this.label1.Text = "Invoice Date";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 212);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 23);
            this.label2.TabIndex = 118;
            this.label2.Text = "Status";
            // 
            // cmbStatus
            // 
            this.cmbStatus.BackColor = System.Drawing.Color.White;
            this.cmbStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.IsSupportUnselect = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "All",
            "New",
            "Checked",
            "Confirmed"});
            this.cmbStatus.Location = new System.Drawing.Point(122, 211);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.OldText = "";
            this.cmbStatus.Size = new System.Drawing.Size(122, 24);
            this.cmbStatus.TabIndex = 119;
            // 
            // txtShipPlanID1
            // 
            this.txtShipPlanID1.BackColor = System.Drawing.Color.White;
            this.txtShipPlanID1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShipPlanID1.Location = new System.Drawing.Point(122, 12);
            this.txtShipPlanID1.Name = "txtShipPlanID1";
            this.txtShipPlanID1.Size = new System.Drawing.Size(123, 23);
            this.txtShipPlanID1.TabIndex = 120;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(248, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 23);
            this.label7.TabIndex = 121;
            this.label7.Text = "～";
            this.label7.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            this.label7.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtShipPlanID2
            // 
            this.txtShipPlanID2.BackColor = System.Drawing.Color.White;
            this.txtShipPlanID2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShipPlanID2.Location = new System.Drawing.Point(273, 12);
            this.txtShipPlanID2.Name = "txtShipPlanID2";
            this.txtShipPlanID2.Size = new System.Drawing.Size(123, 23);
            this.txtShipPlanID2.TabIndex = 122;
            // 
            // rdbtnDetail
            // 
            this.rdbtnDetail.AutoSize = true;
            this.rdbtnDetail.Checked = true;
            this.rdbtnDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtnDetail.Location = new System.Drawing.Point(122, 241);
            this.rdbtnDetail.Name = "rdbtnDetail";
            this.rdbtnDetail.Size = new System.Drawing.Size(62, 21);
            this.rdbtnDetail.TabIndex = 123;
            this.rdbtnDetail.TabStop = true;
            this.rdbtnDetail.Text = "Detail";
            this.rdbtnDetail.UseVisualStyleBackColor = true;
            // 
            // rdbtnSummary
            // 
            this.rdbtnSummary.AutoSize = true;
            this.rdbtnSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtnSummary.Location = new System.Drawing.Point(190, 241);
            this.rdbtnSummary.Name = "rdbtnSummary";
            this.rdbtnSummary.Size = new System.Drawing.Size(153, 21);
            this.rdbtnSummary.TabIndex = 124;
            this.rdbtnSummary.Text = "Summary(By Brand)";
            this.rdbtnSummary.UseVisualStyleBackColor = true;
            // 
            // rdbtnContainer
            // 
            this.rdbtnContainer.AutoSize = true;
            this.rdbtnContainer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtnContainer.Location = new System.Drawing.Point(349, 241);
            this.rdbtnContainer.Name = "rdbtnContainer";
            this.rdbtnContainer.Size = new System.Drawing.Size(127, 21);
            this.rdbtnContainer.TabIndex = 125;
            this.rdbtnContainer.Text = "Container Detail";
            this.rdbtnContainer.UseVisualStyleBackColor = true;
            // 
            // R14
            // 
            this.ClientSize = new System.Drawing.Size(530, 305);
            this.Controls.Add(this.rdbtnContainer);
            this.Controls.Add(this.rdbtnSummary);
            this.Controls.Add(this.rdbtnDetail);
            this.Controls.Add(this.txtShipPlanID2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtShipPlanID1);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateInvoice);
            this.Controls.Add(this.dateETD);
            this.Controls.Add(this.labelOnBoardDate);
            this.Controls.Add(this.txtshipmode);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.datePulloutDate);
            this.Controls.Add(this.labelShipMode);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelPulloutDate);
            this.Controls.Add(this.labelReportContent);
            this.IsSupportToPrint = false;
            this.Name = "R14";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R14. Ship Plan Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelReportContent, 0);
            this.Controls.SetChildIndex(this.labelPulloutDate, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelShipMode, 0);
            this.Controls.SetChildIndex(this.datePulloutDate, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtshipmode, 0);
            this.Controls.SetChildIndex(this.labelOnBoardDate, 0);
            this.Controls.SetChildIndex(this.dateETD, 0);
            this.Controls.SetChildIndex(this.dateInvoice, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbStatus, 0);
            this.Controls.SetChildIndex(this.txtShipPlanID1, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.txtShipPlanID2, 0);
            this.Controls.SetChildIndex(this.rdbtnDetail, 0);
            this.Controls.SetChildIndex(this.rdbtnSummary, 0);
            this.Controls.SetChildIndex(this.rdbtnContainer, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelReportContent;
        private Win.UI.Label labelPulloutDate;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelShipMode;
        private Win.UI.DateRange datePulloutDate;
        private Class.Txtbrand txtbrand;
        private Class.Txtshipmode txtshipmode;
        private Win.UI.DateRange dateETD;
        private Win.UI.Label labelOnBoardDate;
        private Win.UI.DateRange dateInvoice;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.ComboBox cmbStatus;
        private Win.UI.TextBox txtShipPlanID1;
        private Win.UI.Label label7;
        private Win.UI.TextBox txtShipPlanID2;
        private Win.UI.RadioButton rdbtnDetail;
        private Win.UI.RadioButton rdbtnSummary;
        private Win.UI.RadioButton rdbtnContainer;
    }
}
