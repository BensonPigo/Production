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
            this.components = new System.ComponentModel.Container();
            this.labelReportType = new Sci.Win.UI.Label();
            this.labelApvDate = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.comboReportType = new Sci.Win.UI.ComboBox();
            this.dateApvDate = new Sci.Win.UI.DateRange();
            this.SewingDate = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.BuyerDelivery = new Sci.Win.UI.DateRange();
            this.label2 = new Sci.Win.UI.Label();
            this.SCIDelivery = new Sci.Win.UI.DateRange();
            this.label3 = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Production.Class.ComboFactory(this.components);
            this.comboM = new Sci.Production.Class.ComboMDivision(this.components);
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(438, 12);
            this.print.TabIndex = 4;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(438, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(438, 84);
            this.close.TabIndex = 6;
            // 
            // labelReportType
            // 
            this.labelReportType.Location = new System.Drawing.Point(13, 12);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(98, 23);
            this.labelReportType.TabIndex = 94;
            this.labelReportType.Text = "Report Type";
            // 
            // labelApvDate
            // 
            this.labelApvDate.Location = new System.Drawing.Point(13, 48);
            this.labelApvDate.Name = "labelApvDate";
            this.labelApvDate.Size = new System.Drawing.Size(98, 23);
            this.labelApvDate.TabIndex = 95;
            this.labelApvDate.Text = "Apv. Date";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 200);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 96;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 236);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 97;
            this.labelFactory.Text = "Factory";
            // 
            // comboReportType
            // 
            this.comboReportType.BackColor = System.Drawing.Color.White;
            this.comboReportType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboReportType.FormattingEnabled = true;
            this.comboReportType.IsSupportUnselect = true;
            this.comboReportType.Location = new System.Drawing.Point(114, 12);
            this.comboReportType.Name = "comboReportType";
            this.comboReportType.OldText = "";
            this.comboReportType.Size = new System.Drawing.Size(114, 24);
            this.comboReportType.TabIndex = 0;
            // 
            // dateApvDate
            // 
            // 
            // 
            // 
            this.dateApvDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateApvDate.DateBox1.Name = "";
            this.dateApvDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateApvDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateApvDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateApvDate.DateBox2.Name = "";
            this.dateApvDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateApvDate.DateBox2.TabIndex = 1;
            this.dateApvDate.IsRequired = false;
            this.dateApvDate.Location = new System.Drawing.Point(114, 48);
            this.dateApvDate.Name = "dateApvDate";
            this.dateApvDate.Size = new System.Drawing.Size(280, 23);
            this.dateApvDate.TabIndex = 1;
            // 
            // SewingDate
            // 
            // 
            // 
            // 
            this.SewingDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.SewingDate.DateBox1.Name = "";
            this.SewingDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.SewingDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.SewingDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.SewingDate.DateBox2.Name = "";
            this.SewingDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.SewingDate.DateBox2.TabIndex = 1;
            this.SewingDate.IsRequired = false;
            this.SewingDate.Location = new System.Drawing.Point(114, 85);
            this.SewingDate.Name = "SewingDate";
            this.SewingDate.Size = new System.Drawing.Size(280, 23);
            this.SewingDate.TabIndex = 98;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 23);
            this.label1.TabIndex = 99;
            this.label1.Text = "Sewing Date";
            // 
            // BuyerDelivery
            // 
            // 
            // 
            // 
            this.BuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.BuyerDelivery.DateBox1.Name = "";
            this.BuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.BuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.BuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.BuyerDelivery.DateBox2.Name = "";
            this.BuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.BuyerDelivery.DateBox2.TabIndex = 1;
            this.BuyerDelivery.IsRequired = false;
            this.BuyerDelivery.Location = new System.Drawing.Point(114, 125);
            this.BuyerDelivery.Name = "BuyerDelivery";
            this.BuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.BuyerDelivery.TabIndex = 100;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 23);
            this.label2.TabIndex = 101;
            this.label2.Text = "Buyer Delivery";
            // 
            // SCIDelivery
            // 
            // 
            // 
            // 
            this.SCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.SCIDelivery.DateBox1.Name = "";
            this.SCIDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.SCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.SCIDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.SCIDelivery.DateBox2.Name = "";
            this.SCIDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.SCIDelivery.DateBox2.TabIndex = 1;
            this.SCIDelivery.IsRequired = false;
            this.SCIDelivery.Location = new System.Drawing.Point(114, 164);
            this.SCIDelivery.Name = "SCIDelivery";
            this.SCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.SCIDelivery.TabIndex = 102;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 23);
            this.label3.TabIndex = 103;
            this.label3.Text = "SCI Delivery";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.FilteMDivision = true;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IssupportJunk = false;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(114, 236);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(72, 24);
            this.comboFactory.TabIndex = 565;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(114, 199);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(72, 24);
            this.comboM.TabIndex = 564;
            // 
            // R05
            // 
            this.ClientSize = new System.Drawing.Size(530, 296);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.SCIDelivery);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BuyerDelivery);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SewingDate);
            this.Controls.Add(this.label1);
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
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R05. Allowance Consumption Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelReportType, 0);
            this.Controls.SetChildIndex(this.labelApvDate, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.comboReportType, 0);
            this.Controls.SetChildIndex(this.dateApvDate, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.SewingDate, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.BuyerDelivery, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.SCIDelivery, 0);
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
        private Win.UI.DateRange SewingDate;
        private Win.UI.Label label1;
        private Win.UI.DateRange BuyerDelivery;
        private Win.UI.Label label2;
        private Win.UI.DateRange SCIDelivery;
        private Win.UI.Label label3;
        private Class.ComboFactory comboFactory;
        private Class.ComboMDivision comboM;
    }
}
