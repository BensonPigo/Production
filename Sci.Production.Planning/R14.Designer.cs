namespace Sci.Production.Planning
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.txtInvNo_From = new Sci.Win.UI.TextBox();
            this.txtInvNo_To = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.dateRangePulloutDate = new Sci.Win.UI.DateRange();
            this.dateRangeFCRDate = new Sci.Win.UI.DateRange();
            this.dateRangeETD = new Sci.Win.UI.DateRange();
            this.dateRangeETA = new Sci.Win.UI.DateRange();
            this.labelFactoryKPIDate = new Sci.Win.UI.Label();
            this.label11 = new Sci.Win.UI.Label();
            this.label12 = new Sci.Win.UI.Label();
            this.label13 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.comboCategory = new Sci.Production.Class.ComboDropDownList(this.components);
            this.comboInvStatus = new Sci.Production.Class.ComboDropDownList(this.components);
            this.txtcustcd = new Sci.Production.Class.Txtcustcd();
            this.txtDest = new Sci.Production.Class.Txtcountry();
            this.txtExportOrigin = new Sci.Production.Class.Txtcountry();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(420, 12);
            this.print.TabIndex = 4;
            this.print.TabStop = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(420, 48);
            this.toexcel.TabIndex = 5;
            this.toexcel.TabStop = false;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(420, 84);
            this.close.TabIndex = 6;
            this.close.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 23);
            this.label1.TabIndex = 137;
            this.label1.Text = "Brand";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(19, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 23);
            this.label2.TabIndex = 138;
            this.label2.Text = "Factory";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(19, 234);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 23);
            this.label3.TabIndex = 139;
            this.label3.Text = "Export Origin";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(19, 266);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 23);
            this.label4.TabIndex = 140;
            this.label4.Text = "Destination";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(19, 298);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 23);
            this.label5.TabIndex = 141;
            this.label5.Text = "CustCD";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(19, 362);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 23);
            this.label8.TabIndex = 144;
            this.label8.Text = "Category";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(19, 330);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 23);
            this.label9.TabIndex = 145;
            this.label9.Text = "Invoice Status";
            // 
            // txtInvNo_From
            // 
            this.txtInvNo_From.BackColor = System.Drawing.Color.White;
            this.txtInvNo_From.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvNo_From.Location = new System.Drawing.Point(120, 9);
            this.txtInvNo_From.Name = "txtInvNo_From";
            this.txtInvNo_From.Size = new System.Drawing.Size(121, 23);
            this.txtInvNo_From.TabIndex = 1;
            // 
            // txtInvNo_To
            // 
            this.txtInvNo_To.BackColor = System.Drawing.Color.White;
            this.txtInvNo_To.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvNo_To.Location = new System.Drawing.Point(274, 9);
            this.txtInvNo_To.Name = "txtInvNo_To";
            this.txtInvNo_To.Size = new System.Drawing.Size(126, 23);
            this.txtInvNo_To.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(245, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(26, 23);
            this.label10.TabIndex = 157;
            this.label10.Text = " ～";
            this.label10.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateRangePulloutDate
            // 
            // 
            // 
            // 
            this.dateRangePulloutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangePulloutDate.DateBox1.Name = "";
            this.dateRangePulloutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangePulloutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangePulloutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangePulloutDate.DateBox2.Name = "";
            this.dateRangePulloutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangePulloutDate.DateBox2.TabIndex = 1;
            this.dateRangePulloutDate.IsRequired = false;
            this.dateRangePulloutDate.Location = new System.Drawing.Point(120, 41);
            this.dateRangePulloutDate.Name = "dateRangePulloutDate";
            this.dateRangePulloutDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangePulloutDate.TabIndex = 3;
            // 
            // dateRangeFCRDate
            // 
            // 
            // 
            // 
            this.dateRangeFCRDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeFCRDate.DateBox1.Name = "";
            this.dateRangeFCRDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeFCRDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeFCRDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeFCRDate.DateBox2.Name = "";
            this.dateRangeFCRDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeFCRDate.DateBox2.TabIndex = 1;
            this.dateRangeFCRDate.IsRequired = false;
            this.dateRangeFCRDate.Location = new System.Drawing.Point(120, 73);
            this.dateRangeFCRDate.Name = "dateRangeFCRDate";
            this.dateRangeFCRDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeFCRDate.TabIndex = 4;
            // 
            // dateRangeETD
            // 
            // 
            // 
            // 
            this.dateRangeETD.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeETD.DateBox1.Name = "";
            this.dateRangeETD.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeETD.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeETD.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeETD.DateBox2.Name = "";
            this.dateRangeETD.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeETD.DateBox2.TabIndex = 1;
            this.dateRangeETD.IsRequired = false;
            this.dateRangeETD.Location = new System.Drawing.Point(120, 105);
            this.dateRangeETD.Name = "dateRangeETD";
            this.dateRangeETD.Size = new System.Drawing.Size(280, 23);
            this.dateRangeETD.TabIndex = 5;
            // 
            // dateRangeETA
            // 
            // 
            // 
            // 
            this.dateRangeETA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeETA.DateBox1.Name = "";
            this.dateRangeETA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeETA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeETA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeETA.DateBox2.Name = "";
            this.dateRangeETA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeETA.DateBox2.TabIndex = 1;
            this.dateRangeETA.IsRequired = false;
            this.dateRangeETA.Location = new System.Drawing.Point(120, 137);
            this.dateRangeETA.Name = "dateRangeETA";
            this.dateRangeETA.Size = new System.Drawing.Size(280, 23);
            this.dateRangeETA.TabIndex = 6;
            // 
            // labelFactoryKPIDate
            // 
            this.labelFactoryKPIDate.Location = new System.Drawing.Point(19, 41);
            this.labelFactoryKPIDate.Name = "labelFactoryKPIDate";
            this.labelFactoryKPIDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelFactoryKPIDate.RectStyle.BorderWidth = 1F;
            this.labelFactoryKPIDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelFactoryKPIDate.RectStyle.ExtBorderWidth = 1F;
            this.labelFactoryKPIDate.Size = new System.Drawing.Size(99, 23);
            this.labelFactoryKPIDate.TabIndex = 162;
            this.labelFactoryKPIDate.Text = "Pullout Date";
            this.labelFactoryKPIDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelFactoryKPIDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(19, 137);
            this.label11.Name = "label11";
            this.label11.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label11.RectStyle.BorderWidth = 1F;
            this.label11.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label11.RectStyle.ExtBorderWidth = 1F;
            this.label11.Size = new System.Drawing.Size(99, 23);
            this.label11.TabIndex = 163;
            this.label11.Text = "ETA";
            this.label11.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label11.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(19, 105);
            this.label12.Name = "label12";
            this.label12.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label12.RectStyle.BorderWidth = 1F;
            this.label12.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label12.RectStyle.ExtBorderWidth = 1F;
            this.label12.Size = new System.Drawing.Size(99, 23);
            this.label12.TabIndex = 164;
            this.label12.Text = "ETD";
            this.label12.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label12.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(19, 73);
            this.label13.Name = "label13";
            this.label13.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label13.RectStyle.BorderWidth = 1F;
            this.label13.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label13.RectStyle.ExtBorderWidth = 1F;
            this.label13.Size = new System.Drawing.Size(99, 23);
            this.label13.TabIndex = 165;
            this.label13.Text = "FCR Date";
            this.label13.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label13.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(19, 9);
            this.label6.Name = "label6";
            this.label6.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label6.RectStyle.BorderWidth = 1F;
            this.label6.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label6.RectStyle.ExtBorderWidth = 1F;
            this.label6.Size = new System.Drawing.Size(99, 23);
            this.label6.TabIndex = 166;
            this.label6.Text = "Invoice #";
            this.label6.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(120, 362);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(121, 24);
            this.comboCategory.TabIndex = 13;
            this.comboCategory.Type = "Pms_ReportCategory";
            // 
            // comboInvStatus
            // 
            this.comboInvStatus.BackColor = System.Drawing.Color.White;
            this.comboInvStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboInvStatus.FormattingEnabled = true;
            this.comboInvStatus.IsSupportUnselect = true;
            this.comboInvStatus.Location = new System.Drawing.Point(120, 330);
            this.comboInvStatus.Name = "comboInvStatus";
            this.comboInvStatus.OldText = "";
            this.comboInvStatus.Size = new System.Drawing.Size(121, 24);
            this.comboInvStatus.TabIndex = 12;
            this.comboInvStatus.Type = "Pms_InvoiceStatus";
            // 
            // txtcustcd
            // 
            this.txtcustcd.BackColor = System.Drawing.Color.White;
            this.txtcustcd.BrandObjectName = null;
            this.txtcustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd.Location = new System.Drawing.Point(120, 298);
            this.txtcustcd.Name = "txtcustcd";
            this.txtcustcd.Size = new System.Drawing.Size(125, 23);
            this.txtcustcd.TabIndex = 11;
            // 
            // txtDest
            // 
            this.txtDest.DisplayBox1Binding = "";
            this.txtDest.Location = new System.Drawing.Point(120, 267);
            this.txtDest.Name = "txtDest";
            this.txtDest.Size = new System.Drawing.Size(232, 22);
            this.txtDest.TabIndex = 10;
            this.txtDest.TextBox1Binding = "";
            // 
            // txtExportOrigin
            // 
            this.txtExportOrigin.DisplayBox1Binding = "";
            this.txtExportOrigin.Location = new System.Drawing.Point(120, 234);
            this.txtExportOrigin.Name = "txtExportOrigin";
            this.txtExportOrigin.Size = new System.Drawing.Size(232, 22);
            this.txtExportOrigin.TabIndex = 9;
            this.txtExportOrigin.TextBox1Binding = "";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(120, 202);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 8;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(120, 170);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 7;
            // 
            // R14
            // 
            this.ClientSize = new System.Drawing.Size(512, 417);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.labelFactoryKPIDate);
            this.Controls.Add(this.dateRangeETA);
            this.Controls.Add(this.dateRangeETD);
            this.Controls.Add(this.dateRangeFCRDate);
            this.Controls.Add(this.dateRangePulloutDate);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtInvNo_To);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.comboInvStatus);
            this.Controls.Add(this.txtcustcd);
            this.Controls.Add(this.txtDest);
            this.Controls.Add(this.txtExportOrigin);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.txtInvNo_From);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.DefaultControl = "dateSewingDate";
            this.DefaultControlForEdit = "dateSewingDate";
            this.IsSupportToPrint = false;
            this.Name = "R14";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R14. Adidas Specific Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.txtInvNo_From, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtExportOrigin, 0);
            this.Controls.SetChildIndex(this.txtDest, 0);
            this.Controls.SetChildIndex(this.txtcustcd, 0);
            this.Controls.SetChildIndex(this.comboInvStatus, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.Controls.SetChildIndex(this.txtInvNo_To, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.dateRangePulloutDate, 0);
            this.Controls.SetChildIndex(this.dateRangeFCRDate, 0);
            this.Controls.SetChildIndex(this.dateRangeETD, 0);
            this.Controls.SetChildIndex(this.dateRangeETA, 0);
            this.Controls.SetChildIndex(this.labelFactoryKPIDate, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label8;
        private Win.UI.Label label9;
        private Win.UI.TextBox txtInvNo_From;
        private Class.Txtbrand txtbrand;
        private Class.Txtfactory txtfactory;
        private Class.Txtcountry txtExportOrigin;
        private Class.Txtcountry txtDest;
        private Class.Txtcustcd txtcustcd;
        private Class.ComboDropDownList comboInvStatus;
        private Class.ComboDropDownList comboCategory;
        private Win.UI.TextBox txtInvNo_To;
        private Win.UI.Label label10;
        private Win.UI.DateRange dateRangePulloutDate;
        private Win.UI.DateRange dateRangeFCRDate;
        private Win.UI.DateRange dateRangeETD;
        private Win.UI.DateRange dateRangeETA;
        private Win.UI.Label labelFactoryKPIDate;
        private Win.UI.Label label11;
        private Win.UI.Label label12;
        private Win.UI.Label label13;
        private Win.UI.Label label6;
    }
}
