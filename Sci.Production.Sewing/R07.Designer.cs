namespace Sci.Production.Sewing
{
	partial class R07
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
			if(disposing && (components != null))
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
            this.labelM = new Sci.Win.UI.Label();
            this.txtMdivisionM = new Sci.Production.Class.TxtMdivision();
            this.txtSPNO = new Sci.Win.UI.TextBox();
            this.comboStatus = new Sci.Win.UI.ComboBox();
            this.dateIssueDate = new Sci.Win.UI.DateRange();
            this.labStatus = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.dateRangeApvDate = new Sci.Win.UI.DateRange();
            this.labSubcon = new Sci.Win.UI.Label();
            this.labContract = new Sci.Win.UI.Label();
            this.txtSubCon = new Sci.Win.UI.TextBox();
            this.txtContract = new Sci.Win.UI.TextBox();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.labApvDate = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(440, 120);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(440, 12);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(440, 48);
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(21, 185);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(109, 23);
            this.labelM.TabIndex = 119;
            this.labelM.Text = "M";
            // 
            // txtMdivisionM
            // 
            this.txtMdivisionM.BackColor = System.Drawing.Color.White;
            this.txtMdivisionM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivisionM.Location = new System.Drawing.Point(133, 185);
            this.txtMdivisionM.Name = "txtMdivisionM";
            this.txtMdivisionM.Size = new System.Drawing.Size(66, 23);
            this.txtMdivisionM.TabIndex = 107;
            // 
            // txtSPNO
            // 
            this.txtSPNO.BackColor = System.Drawing.Color.White;
            this.txtSPNO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNO.Location = new System.Drawing.Point(135, 150);
            this.txtSPNO.MaxLength = 13;
            this.txtSPNO.Name = "txtSPNO";
            this.txtSPNO.Size = new System.Drawing.Size(146, 23);
            this.txtSPNO.TabIndex = 110;
            // 
            // comboStatus
            // 
            this.comboStatus.BackColor = System.Drawing.Color.White;
            this.comboStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.IsSupportUnselect = true;
            this.comboStatus.Items.AddRange(new object[] {
            "Issue Date",
            "Supplier"});
            this.comboStatus.Location = new System.Drawing.Point(133, 256);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.OldText = "";
            this.comboStatus.Size = new System.Drawing.Size(121, 24);
            this.comboStatus.TabIndex = 112;
            // 
            // dateIssueDate
            // 
            // 
            // 
            // 
            this.dateIssueDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateIssueDate.DateBox1.Name = "";
            this.dateIssueDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateIssueDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateIssueDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateIssueDate.DateBox2.Name = "";
            this.dateIssueDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateIssueDate.DateBox2.TabIndex = 1;
            this.dateIssueDate.IsRequired = false;
            this.dateIssueDate.Location = new System.Drawing.Point(134, 14);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(280, 23);
            this.dateIssueDate.TabIndex = 105;
            // 
            // labStatus
            // 
            this.labStatus.Location = new System.Drawing.Point(21, 257);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(109, 23);
            this.labStatus.TabIndex = 118;
            this.labStatus.Text = "Status";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(22, 150);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(109, 23);
            this.labelSPNo.TabIndex = 116;
            this.labelSPNo.Text = "SP#";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(21, 220);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(109, 23);
            this.labelFactory.TabIndex = 113;
            this.labelFactory.Text = "Factory";
            // 
            // dateRangeApvDate
            // 
            // 
            // 
            // 
            this.dateRangeApvDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeApvDate.DateBox1.Name = "";
            this.dateRangeApvDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeApvDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeApvDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeApvDate.DateBox2.Name = "";
            this.dateRangeApvDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeApvDate.DateBox2.TabIndex = 1;
            this.dateRangeApvDate.IsRequired = false;
            this.dateRangeApvDate.Location = new System.Drawing.Point(134, 48);
            this.dateRangeApvDate.Name = "dateRangeApvDate";
            this.dateRangeApvDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeApvDate.TabIndex = 121;
            // 
            // labSubcon
            // 
            this.labSubcon.Location = new System.Drawing.Point(20, 84);
            this.labSubcon.Name = "labSubcon";
            this.labSubcon.Size = new System.Drawing.Size(110, 23);
            this.labSubcon.TabIndex = 123;
            this.labSubcon.Text = "SubCon-Out-Fty";
            // 
            // labContract
            // 
            this.labContract.Location = new System.Drawing.Point(21, 116);
            this.labContract.Name = "labContract";
            this.labContract.Size = new System.Drawing.Size(110, 23);
            this.labContract.TabIndex = 124;
            this.labContract.Text = "Contract number";
            // 
            // txtSubCon
            // 
            this.txtSubCon.BackColor = System.Drawing.Color.White;
            this.txtSubCon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSubCon.Location = new System.Drawing.Point(135, 84);
            this.txtSubCon.Name = "txtSubCon";
            this.txtSubCon.Size = new System.Drawing.Size(146, 23);
            this.txtSubCon.TabIndex = 125;
            // 
            // txtContract
            // 
            this.txtContract.BackColor = System.Drawing.Color.White;
            this.txtContract.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtContract.Location = new System.Drawing.Point(135, 116);
            this.txtContract.Name = "txtContract";
            this.txtContract.Size = new System.Drawing.Size(146, 23);
            this.txtContract.TabIndex = 126;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(133, 220);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 127;
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(20, 14);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.Size = new System.Drawing.Size(110, 23);
            this.labelIssueDate.TabIndex = 128;
            this.labelIssueDate.Text = "Issue Date";
            // 
            // labApvDate
            // 
            this.labApvDate.Location = new System.Drawing.Point(20, 48);
            this.labApvDate.Name = "labApvDate";
            this.labApvDate.Size = new System.Drawing.Size(110, 23);
            this.labApvDate.TabIndex = 129;
            this.labApvDate.Text = "Apv Date";
            // 
            // R07
            // 
            this.ClientSize = new System.Drawing.Size(532, 314);
            this.Controls.Add(this.labApvDate);
            this.Controls.Add(this.labelIssueDate);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtContract);
            this.Controls.Add(this.txtSubCon);
            this.Controls.Add(this.labContract);
            this.Controls.Add(this.labSubcon);
            this.Controls.Add(this.dateRangeApvDate);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivisionM);
            this.Controls.Add(this.txtSPNO);
            this.Controls.Add(this.comboStatus);
            this.Controls.Add(this.dateIssueDate);
            this.Controls.Add(this.labStatus);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.labelFactory);
            this.Name = "R07";
            this.Text = "P07. Subcon out list Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labStatus, 0);
            this.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.Controls.SetChildIndex(this.comboStatus, 0);
            this.Controls.SetChildIndex(this.txtSPNO, 0);
            this.Controls.SetChildIndex(this.txtMdivisionM, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.dateRangeApvDate, 0);
            this.Controls.SetChildIndex(this.labSubcon, 0);
            this.Controls.SetChildIndex(this.labContract, 0);
            this.Controls.SetChildIndex(this.txtSubCon, 0);
            this.Controls.SetChildIndex(this.txtContract, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.Controls.SetChildIndex(this.labApvDate, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion
        private Win.UI.Label labelM;
        private Class.TxtMdivision txtMdivisionM;
        private Win.UI.TextBox txtSPNO;
        private Win.UI.ComboBox comboStatus;
        private Win.UI.DateRange dateIssueDate;
        private Win.UI.Label labStatus;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelFactory;
        private Win.UI.DateRange dateRangeApvDate;
        private Win.UI.Label labSubcon;
        private Win.UI.Label labContract;
        private Win.UI.TextBox txtSubCon;
        private Win.UI.TextBox txtContract;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label labelIssueDate;
        private Win.UI.Label labApvDate;
    }
}
