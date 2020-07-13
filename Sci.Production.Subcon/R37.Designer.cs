namespace Sci.Production.Subcon
{
    partial class R37
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
            this.comboReportType = new Sci.Win.UI.ComboBox();
            this.comboPaymentSettled = new Sci.Win.UI.ComboBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.txttpeuser_caneditSMR = new Sci.Production.Class.Txttpeuser_canedit();
            this.txttpeuser_caneditHandle = new Sci.Production.Class.Txttpeuser_canedit();
            this.dateSettledDate = new Sci.Win.UI.DateRange();
            this.dateConfirmDate = new Sci.Win.UI.DateRange();
            this.dateDebitDate = new Sci.Win.UI.DateRange();
            this.txtDebitNoEnd = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.txtDebitNoStart = new Sci.Win.UI.TextBox();
            this.labelReportType = new Sci.Win.UI.Label();
            this.labelPaymentSettled = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelSMR = new Sci.Win.UI.Label();
            this.labelHandle = new Sci.Win.UI.Label();
            this.labelDebitNo = new Sci.Win.UI.Label();
            this.labelSettledDate = new Sci.Win.UI.Label();
            this.labelConfirmDate = new Sci.Win.UI.Label();
            this.labelDebitDate = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.TabIndex = 1;
            // 
            // toexcel
            // 
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboReportType);
            this.panel1.Controls.Add(this.comboPaymentSettled);
            this.panel1.Controls.Add(this.comboFactory);
            this.panel1.Controls.Add(this.txttpeuser_caneditSMR);
            this.panel1.Controls.Add(this.txttpeuser_caneditHandle);
            this.panel1.Controls.Add(this.dateSettledDate);
            this.panel1.Controls.Add(this.dateConfirmDate);
            this.panel1.Controls.Add(this.dateDebitDate);
            this.panel1.Controls.Add(this.txtDebitNoEnd);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtDebitNoStart);
            this.panel1.Controls.Add(this.labelReportType);
            this.panel1.Controls.Add(this.labelPaymentSettled);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelSMR);
            this.panel1.Controls.Add(this.labelHandle);
            this.panel1.Controls.Add(this.labelDebitNo);
            this.panel1.Controls.Add(this.labelSettledDate);
            this.panel1.Controls.Add(this.labelConfirmDate);
            this.panel1.Controls.Add(this.labelDebitDate);
            this.panel1.Location = new System.Drawing.Point(18, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(511, 305);
            this.panel1.TabIndex = 0;
            // 
            // comboReportType
            // 
            this.comboReportType.BackColor = System.Drawing.Color.White;
            this.comboReportType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboReportType.FormattingEnabled = true;
            this.comboReportType.IsSupportUnselect = true;
            this.comboReportType.Items.AddRange(new object[] {
            "List",
            "Detail List"});
            this.comboReportType.Location = new System.Drawing.Point(139, 270);
            this.comboReportType.Name = "comboReportType";
            this.comboReportType.OldText = "";
            this.comboReportType.Size = new System.Drawing.Size(121, 24);
            this.comboReportType.TabIndex = 9;
            // 
            // comboPaymentSettled
            // 
            this.comboPaymentSettled.BackColor = System.Drawing.Color.White;
            this.comboPaymentSettled.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboPaymentSettled.FormattingEnabled = true;
            this.comboPaymentSettled.IsSupportUnselect = true;
            this.comboPaymentSettled.Items.AddRange(new object[] {
            " ",
            "Settled",
            "Not Settled"});
            this.comboPaymentSettled.Location = new System.Drawing.Point(139, 235);
            this.comboPaymentSettled.Name = "comboPaymentSettled";
            this.comboPaymentSettled.OldText = "";
            this.comboPaymentSettled.Size = new System.Drawing.Size(121, 24);
            this.comboPaymentSettled.TabIndex = 8;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(139, 199);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 7;
            // 
            // txttpeuser_caneditSMR
            // 
            this.txttpeuser_caneditSMR.DisplayBox1Binding = "";
            this.txttpeuser_caneditSMR.Location = new System.Drawing.Point(139, 167);
            this.txttpeuser_caneditSMR.Name = "txttpeuser_caneditSMR";
            this.txttpeuser_caneditSMR.Size = new System.Drawing.Size(300, 23);
            this.txttpeuser_caneditSMR.TabIndex = 6;
            this.txttpeuser_caneditSMR.TextBox1Binding = "";
            // 
            // txttpeuser_caneditHandle
            // 
            this.txttpeuser_caneditHandle.DisplayBox1Binding = "";
            this.txttpeuser_caneditHandle.Location = new System.Drawing.Point(139, 134);
            this.txttpeuser_caneditHandle.Name = "txttpeuser_caneditHandle";
            this.txttpeuser_caneditHandle.Size = new System.Drawing.Size(300, 23);
            this.txttpeuser_caneditHandle.TabIndex = 5;
            this.txttpeuser_caneditHandle.TextBox1Binding = "";
            // 
            // dateSettledDate
            // 
            // 
            // 
            // 
            this.dateSettledDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSettledDate.DateBox1.Name = "";
            this.dateSettledDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSettledDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSettledDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSettledDate.DateBox2.Name = "";
            this.dateSettledDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSettledDate.DateBox2.TabIndex = 1;
            this.dateSettledDate.IsRequired = false;
            this.dateSettledDate.Location = new System.Drawing.Point(139, 74);
            this.dateSettledDate.Name = "dateSettledDate";
            this.dateSettledDate.Size = new System.Drawing.Size(280, 23);
            this.dateSettledDate.TabIndex = 2;
            // 
            // dateConfirmDate
            // 
            // 
            // 
            // 
            this.dateConfirmDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateConfirmDate.DateBox1.Name = "";
            this.dateConfirmDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateConfirmDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateConfirmDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateConfirmDate.DateBox2.Name = "";
            this.dateConfirmDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateConfirmDate.DateBox2.TabIndex = 1;
            this.dateConfirmDate.IsRequired = false;
            this.dateConfirmDate.Location = new System.Drawing.Point(139, 45);
            this.dateConfirmDate.Name = "dateConfirmDate";
            this.dateConfirmDate.Size = new System.Drawing.Size(280, 23);
            this.dateConfirmDate.TabIndex = 1;
            // 
            // dateDebitDate
            // 
            // 
            // 
            // 
            this.dateDebitDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateDebitDate.DateBox1.Name = "";
            this.dateDebitDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateDebitDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateDebitDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateDebitDate.DateBox2.Name = "";
            this.dateDebitDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateDebitDate.DateBox2.TabIndex = 1;
            this.dateDebitDate.IsRequired = false;
            this.dateDebitDate.Location = new System.Drawing.Point(139, 16);
            this.dateDebitDate.Name = "dateDebitDate";
            this.dateDebitDate.Size = new System.Drawing.Size(280, 23);
            this.dateDebitDate.TabIndex = 0;
            // 
            // txtDebitNoEnd
            // 
            this.txtDebitNoEnd.BackColor = System.Drawing.Color.White;
            this.txtDebitNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDebitNoEnd.Location = new System.Drawing.Point(322, 104);
            this.txtDebitNoEnd.Name = "txtDebitNoEnd";
            this.txtDebitNoEnd.Size = new System.Drawing.Size(164, 23);
            this.txtDebitNoEnd.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.SystemColors.Control;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label10.Location = new System.Drawing.Point(301, 111);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(21, 16);
            this.label10.TabIndex = 10;
            this.label10.Text = "~";
            this.label10.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtDebitNoStart
            // 
            this.txtDebitNoStart.BackColor = System.Drawing.Color.White;
            this.txtDebitNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDebitNoStart.Location = new System.Drawing.Point(139, 104);
            this.txtDebitNoStart.Name = "txtDebitNoStart";
            this.txtDebitNoStart.Size = new System.Drawing.Size(159, 23);
            this.txtDebitNoStart.TabIndex = 3;
            // 
            // labelReportType
            // 
            this.labelReportType.Location = new System.Drawing.Point(21, 268);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(111, 23);
            this.labelReportType.TabIndex = 8;
            this.labelReportType.Text = "Report Type";
            // 
            // labelPaymentSettled
            // 
            this.labelPaymentSettled.Location = new System.Drawing.Point(22, 233);
            this.labelPaymentSettled.Name = "labelPaymentSettled";
            this.labelPaymentSettled.Size = new System.Drawing.Size(110, 23);
            this.labelPaymentSettled.TabIndex = 7;
            this.labelPaymentSettled.Text = "Payment Settled";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(22, 200);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(110, 23);
            this.labelFactory.TabIndex = 6;
            this.labelFactory.Text = "Factory";
            // 
            // labelSMR
            // 
            this.labelSMR.Location = new System.Drawing.Point(22, 168);
            this.labelSMR.Name = "labelSMR";
            this.labelSMR.Size = new System.Drawing.Size(110, 23);
            this.labelSMR.TabIndex = 5;
            this.labelSMR.Text = "SMR";
            // 
            // labelHandle
            // 
            this.labelHandle.Location = new System.Drawing.Point(22, 135);
            this.labelHandle.Name = "labelHandle";
            this.labelHandle.Size = new System.Drawing.Size(110, 23);
            this.labelHandle.TabIndex = 4;
            this.labelHandle.Text = "Handle";
            // 
            // labelDebitNo
            // 
            this.labelDebitNo.Location = new System.Drawing.Point(22, 104);
            this.labelDebitNo.Name = "labelDebitNo";
            this.labelDebitNo.Size = new System.Drawing.Size(110, 23);
            this.labelDebitNo.TabIndex = 3;
            this.labelDebitNo.Text = "Debit No";
            // 
            // labelSettledDate
            // 
            this.labelSettledDate.Location = new System.Drawing.Point(22, 75);
            this.labelSettledDate.Name = "labelSettledDate";
            this.labelSettledDate.Size = new System.Drawing.Size(110, 23);
            this.labelSettledDate.TabIndex = 2;
            this.labelSettledDate.Text = "Settled Date";
            // 
            // labelConfirmDate
            // 
            this.labelConfirmDate.Location = new System.Drawing.Point(22, 45);
            this.labelConfirmDate.Name = "labelConfirmDate";
            this.labelConfirmDate.Size = new System.Drawing.Size(110, 23);
            this.labelConfirmDate.TabIndex = 1;
            this.labelConfirmDate.Text = "Confirm Date";
            // 
            // labelDebitDate
            // 
            this.labelDebitDate.Location = new System.Drawing.Point(22, 16);
            this.labelDebitDate.Name = "labelDebitDate";
            this.labelDebitDate.Size = new System.Drawing.Size(110, 23);
            this.labelDebitDate.TabIndex = 0;
            this.labelDebitDate.Text = "Debit Date";
            // 
            // R37
            // 
            this.ClientSize = new System.Drawing.Size(627, 348);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "dateDebitDate";
            this.DefaultControlForEdit = "dateDebitDate";
            this.Name = "R37";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R37. Debit Memo List(Taipei)";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Label labelConfirmDate;
        private Win.UI.Label labelDebitDate;
        private Win.UI.Label label10;
        private Win.UI.TextBox txtDebitNoStart;
        private Win.UI.Label labelReportType;
        private Win.UI.Label labelPaymentSettled;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelSMR;
        private Win.UI.Label labelHandle;
        private Win.UI.Label labelDebitNo;
        private Win.UI.Label labelSettledDate;
        private Win.UI.DateRange dateSettledDate;
        private Win.UI.DateRange dateConfirmDate;
        private Win.UI.DateRange dateDebitDate;
        private Win.UI.TextBox txtDebitNoEnd;
        private Win.UI.ComboBox comboFactory;
        private Class.Txttpeuser_canedit txttpeuser_caneditSMR;
        private Class.Txttpeuser_canedit txttpeuser_caneditHandle;
        private Win.UI.ComboBox comboReportType;
        private Win.UI.ComboBox comboPaymentSettled;

    }
}
