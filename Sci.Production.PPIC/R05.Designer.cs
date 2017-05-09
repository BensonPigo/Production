namespace Sci.Production.PPIC
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelReportType = new Sci.Win.UI.Label();
            this.labelApvDate = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.comboReportType = new Sci.Win.UI.ComboBox();
            this.dateApvDate = new Sci.Win.UI.DateRange();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(395, 12);
            this.print.TabIndex = 4;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(395, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(395, 84);
            this.close.TabIndex = 6;
            // 
            // labelReportType
            // 
            this.labelReportType.Lines = 0;
            this.labelReportType.Location = new System.Drawing.Point(13, 12);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(82, 23);
            this.labelReportType.TabIndex = 94;
            this.labelReportType.Text = "Report Type";
            // 
            // labelApvDate
            // 
            this.labelApvDate.Lines = 0;
            this.labelApvDate.Location = new System.Drawing.Point(13, 48);
            this.labelApvDate.Name = "labelApvDate";
            this.labelApvDate.Size = new System.Drawing.Size(82, 23);
            this.labelApvDate.TabIndex = 95;
            this.labelApvDate.Text = "Apv. Date";
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(13, 84);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(82, 23);
            this.labelM.TabIndex = 96;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(13, 120);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(82, 23);
            this.labelFactory.TabIndex = 97;
            this.labelFactory.Text = "Factory";
            // 
            // comboReportType
            // 
            this.comboReportType.BackColor = System.Drawing.Color.White;
            this.comboReportType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboReportType.FormattingEnabled = true;
            this.comboReportType.IsSupportUnselect = true;
            this.comboReportType.Location = new System.Drawing.Point(99, 12);
            this.comboReportType.Name = "comboReportType";
            this.comboReportType.Size = new System.Drawing.Size(114, 24);
            this.comboReportType.TabIndex = 0;
            // 
            // dateApvDate
            // 
            this.dateApvDate.IsRequired = false;
            this.dateApvDate.Location = new System.Drawing.Point(99, 48);
            this.dateApvDate.Name = "dateApvDate";
            this.dateApvDate.Size = new System.Drawing.Size(280, 23);
            this.dateApvDate.TabIndex = 1;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(99, 84);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(72, 24);
            this.comboM.TabIndex = 2;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(99, 120);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(72, 24);
            this.comboFactory.TabIndex = 3;
            // 
            // R05
            // 
            this.ClientSize = new System.Drawing.Size(487, 179);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.dateApvDate);
            this.Controls.Add(this.comboReportType);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelApvDate);
            this.Controls.Add(this.labelReportType);
            this.DefaultControl = "comboReportType";
            this.DefaultControlForEdit = "comboReportType";
            this.IsSupportToPrint = false;
            this.Name = "R05";
            this.Text = "R05. Allowance Consumption Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelReportType, 0);
            this.Controls.SetChildIndex(this.labelApvDate, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.comboReportType, 0);
            this.Controls.SetChildIndex(this.dateApvDate, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelReportType;
        private Win.UI.Label labelApvDate;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.ComboBox comboReportType;
        private Win.UI.DateRange dateApvDate;
        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboFactory;
    }
}
