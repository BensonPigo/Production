namespace Sci.Production.PPIC
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new Sci.Win.UI.Label();
            this.labelApvDate = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.comboReportType = new Sci.Win.UI.ComboBox();
            this.dateApvDate = new Sci.Win.UI.DateRange();
            this.lbLeadtime = new Sci.Win.UI.Label();
            this.comboLeadtime = new Sci.Win.UI.ComboBox();
            this.comboFactory = new Sci.Production.Class.ComboFactory(this.components);
            this.comboM = new Sci.Production.Class.ComboMDivision(this.components);
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(389, 1);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(389, 37);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(389, 73);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(343, 3);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Report Type";
            // 
            // labelApvDate
            // 
            this.labelApvDate.Location = new System.Drawing.Point(13, 48);
            this.labelApvDate.Name = "labelApvDate";
            this.labelApvDate.Size = new System.Drawing.Size(82, 23);
            this.labelApvDate.TabIndex = 95;
            this.labelApvDate.Text = "Apv. Date";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 84);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(82, 23);
            this.labelM.TabIndex = 96;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
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
            this.comboReportType.Location = new System.Drawing.Point(99, 11);
            this.comboReportType.Name = "comboReportType";
            this.comboReportType.OldText = "";
            this.comboReportType.Size = new System.Drawing.Size(121, 24);
            this.comboReportType.TabIndex = 0;
            // 
            // dateApvDate
            // 
            // 
            // 
            // 
            this.dateApvDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateApvDate.DateBox1.Name = "";
            this.dateApvDate.DateBox1.Size = new System.Drawing.Size(103, 23);
            this.dateApvDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateApvDate.DateBox2.Location = new System.Drawing.Point(125, 0);
            this.dateApvDate.DateBox2.Name = "";
            this.dateApvDate.DateBox2.Size = new System.Drawing.Size(103, 23);
            this.dateApvDate.DateBox2.TabIndex = 1;
            this.dateApvDate.IsRequired = false;
            this.dateApvDate.Location = new System.Drawing.Point(99, 48);
            this.dateApvDate.Name = "dateApvDate";
            this.dateApvDate.Size = new System.Drawing.Size(229, 23);
            this.dateApvDate.TabIndex = 1;
            // 
            // lbLeadtime
            // 
            this.lbLeadtime.Location = new System.Drawing.Point(188, 119);
            this.lbLeadtime.Name = "lbLeadtime";
            this.lbLeadtime.Size = new System.Drawing.Size(82, 23);
            this.lbLeadtime.TabIndex = 98;
            this.lbLeadtime.Text = "Leadtime";
            // 
            // comboLeadtime
            // 
            this.comboLeadtime.BackColor = System.Drawing.Color.White;
            this.comboLeadtime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboLeadtime.FormattingEnabled = true;
            this.comboLeadtime.IsSupportUnselect = true;
            this.comboLeadtime.Location = new System.Drawing.Point(273, 120);
            this.comboLeadtime.Name = "comboLeadtime";
            this.comboLeadtime.OldText = "";
            this.comboLeadtime.Size = new System.Drawing.Size(90, 24);
            this.comboLeadtime.TabIndex = 4;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.FilteMDivision = true;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IssupportJunk = false;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(98, 119);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(82, 24);
            this.comboFactory.TabIndex = 565;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(99, 84);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(81, 24);
            this.comboM.TabIndex = 564;
            // 
            // R04
            // 
            this.ClientSize = new System.Drawing.Size(475, 192);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.comboLeadtime);
            this.Controls.Add(this.lbLeadtime);
            this.Controls.Add(this.dateApvDate);
            this.Controls.Add(this.comboReportType);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelApvDate);
            this.Controls.Add(this.label1);
            this.DefaultControl = "comboReportType";
            this.DefaultControlForEdit = "comboReportType";
            this.IsSupportToPrint = false;
            this.Name = "R04";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R04. Lacking & Replacement BCS Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.labelApvDate, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.comboReportType, 0);
            this.Controls.SetChildIndex(this.dateApvDate, 0);
            this.Controls.SetChildIndex(this.lbLeadtime, 0);
            this.Controls.SetChildIndex(this.comboLeadtime, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label labelApvDate;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.ComboBox comboReportType;
        private Win.UI.DateRange dateApvDate;
        private Win.UI.Label lbLeadtime;
        private Win.UI.ComboBox comboLeadtime;
        private Class.ComboFactory comboFactory;
        private Class.ComboMDivision comboM;
    }
}
