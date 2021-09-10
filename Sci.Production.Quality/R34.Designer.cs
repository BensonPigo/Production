namespace Sci.Production.Quality
{
    partial class R34
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.txtbrand1 = new Sci.Production.Class.Txtbrand();
            this.txtstyle1 = new Sci.Production.Class.Txtstyle();
            this.comboMDivision1 = new Sci.Production.Class.ComboMDivision(this.components);
            this.comboDropDownList1 = new Sci.Production.Class.ComboDropDownList(this.components);
            this.label1 = new Sci.Win.UI.Label();
            this.comboReportType = new Sci.Win.UI.ComboBox();
            this.txtSeason = new Sci.Win.UI.TextBox();
            this.dateBuyerdelivery = new Sci.Win.UI.DateRange();
            this.label9 = new Sci.Win.UI.Label();
            this.txtSP2 = new Sci.Win.UI.TextBox();
            this.txtSP1 = new Sci.Win.UI.TextBox();
            this.labelInspected = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(458, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(458, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(458, 84);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtbrand1);
            this.panel1.Controls.Add(this.txtstyle1);
            this.panel1.Controls.Add(this.comboMDivision1);
            this.panel1.Controls.Add(this.comboDropDownList1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.comboReportType);
            this.panel1.Controls.Add(this.txtSeason);
            this.panel1.Controls.Add(this.dateBuyerdelivery);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtSP2);
            this.panel1.Controls.Add(this.txtSP1);
            this.panel1.Controls.Add(this.labelInspected);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelBrand);
            this.panel1.Controls.Add(this.labelSeason);
            this.panel1.Controls.Add(this.labelStyle);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(417, 256);
            this.panel1.TabIndex = 94;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 46);
            this.label5.Name = "label5";
            this.label5.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label5.Size = new System.Drawing.Size(105, 23);
            this.label5.TabIndex = 127;
            this.label5.Text = "Buyer delivery";
            this.label5.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 17);
            this.label4.Name = "label4";
            this.label4.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label4.Size = new System.Drawing.Size(105, 23);
            this.label4.TabIndex = 126;
            this.label4.Text = "SP#";
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(120, 104);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(100, 23);
            this.txtbrand1.TabIndex = 4;
            // 
            // txtstyle1
            // 
            this.txtstyle1.BackColor = System.Drawing.Color.White;
            this.txtstyle1.BrandObjectName = null;
            this.txtstyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle1.Location = new System.Drawing.Point(120, 75);
            this.txtstyle1.Name = "txtstyle1";
            this.txtstyle1.Size = new System.Drawing.Size(130, 23);
            this.txtstyle1.TabIndex = 3;
            this.txtstyle1.TarBrand = null;
            this.txtstyle1.TarSeason = null;
            // 
            // comboMDivision1
            // 
            this.comboMDivision1.BackColor = System.Drawing.Color.White;
            this.comboMDivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMDivision1.FormattingEnabled = true;
            this.comboMDivision1.IsSupportUnselect = true;
            this.comboMDivision1.Location = new System.Drawing.Point(120, 189);
            this.comboMDivision1.Name = "comboMDivision1";
            this.comboMDivision1.OldText = "";
            this.comboMDivision1.Size = new System.Drawing.Size(80, 24);
            this.comboMDivision1.TabIndex = 7;
            // 
            // comboDropDownList1
            // 
            this.comboDropDownList1.AddAllItem = false;
            this.comboDropDownList1.BackColor = System.Drawing.Color.White;
            this.comboDropDownList1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownList1.FormattingEnabled = true;
            this.comboDropDownList1.IsSupportUnselect = true;
            this.comboDropDownList1.Location = new System.Drawing.Point(120, 159);
            this.comboDropDownList1.Name = "comboDropDownList1";
            this.comboDropDownList1.OldText = "";
            this.comboDropDownList1.Size = new System.Drawing.Size(161, 24);
            this.comboDropDownList1.TabIndex = 6;
            this.comboDropDownList1.Type = "Pms_ReportForProduct";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 23);
            this.label1.TabIndex = 118;
            this.label1.Text = "Category";
            // 
            // comboReportType
            // 
            this.comboReportType.BackColor = System.Drawing.Color.White;
            this.comboReportType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboReportType.FormattingEnabled = true;
            this.comboReportType.IsSupportUnselect = true;
            this.comboReportType.Items.AddRange(new object[] {
            "SP#, Seq",
            "Style"});
            this.comboReportType.Location = new System.Drawing.Point(120, 219);
            this.comboReportType.Name = "comboReportType";
            this.comboReportType.OldText = "";
            this.comboReportType.Size = new System.Drawing.Size(178, 24);
            this.comboReportType.TabIndex = 8;
            // 
            // txtSeason
            // 
            this.txtSeason.BackColor = System.Drawing.Color.White;
            this.txtSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeason.Location = new System.Drawing.Point(120, 130);
            this.txtSeason.MaxLength = 10;
            this.txtSeason.Name = "txtSeason";
            this.txtSeason.Size = new System.Drawing.Size(100, 23);
            this.txtSeason.TabIndex = 5;
            // 
            // dateBuyerdelivery
            // 
            // 
            // 
            // 
            this.dateBuyerdelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerdelivery.DateBox1.Name = "";
            this.dateBuyerdelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerdelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerdelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerdelivery.DateBox2.Name = "";
            this.dateBuyerdelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerdelivery.DateBox2.TabIndex = 1;
            this.dateBuyerdelivery.Location = new System.Drawing.Point(120, 46);
            this.dateBuyerdelivery.Name = "dateBuyerdelivery";
            this.dateBuyerdelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerdelivery.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(239, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 23);
            this.label9.TabIndex = 110;
            this.label9.Text = "～";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtSP2
            // 
            this.txtSP2.BackColor = System.Drawing.Color.White;
            this.txtSP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP2.Location = new System.Drawing.Point(262, 17);
            this.txtSP2.MaxLength = 13;
            this.txtSP2.Name = "txtSP2";
            this.txtSP2.Size = new System.Drawing.Size(116, 23);
            this.txtSP2.TabIndex = 1;
            // 
            // txtSP1
            // 
            this.txtSP1.BackColor = System.Drawing.Color.White;
            this.txtSP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP1.Location = new System.Drawing.Point(120, 17);
            this.txtSP1.MaxLength = 13;
            this.txtSP1.Name = "txtSP1";
            this.txtSP1.Size = new System.Drawing.Size(116, 23);
            this.txtSP1.TabIndex = 0;
            // 
            // labelInspected
            // 
            this.labelInspected.Location = new System.Drawing.Point(12, 219);
            this.labelInspected.Name = "labelInspected";
            this.labelInspected.Size = new System.Drawing.Size(105, 23);
            this.labelInspected.TabIndex = 7;
            this.labelInspected.Text = "Report Type";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(12, 190);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(105, 23);
            this.labelFactory.TabIndex = 6;
            this.labelFactory.Text = "Mdivision";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(12, 104);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(105, 23);
            this.labelBrand.TabIndex = 5;
            this.labelBrand.Text = "Brand";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(12, 130);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(105, 23);
            this.labelSeason.TabIndex = 4;
            this.labelSeason.Text = "Season";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(12, 75);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(105, 23);
            this.labelStyle.TabIndex = 3;
            this.labelStyle.Text = "Style";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(444, 126);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 22);
            this.label10.TabIndex = 97;
            this.label10.Text = "Paper Size A4";
            // 
            // R34
            // 
            this.ClientSize = new System.Drawing.Size(550, 299);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panel1);
            this.Name = "R34";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R34. MD Pass Rate";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Label labelInspected;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelStyle;
        private Win.UI.TextBox txtSP2;
        private Win.UI.TextBox txtSP1;
        private Win.UI.ComboBox comboReportType;
        private Win.UI.TextBox txtSeason;
        private Win.UI.DateRange dateBuyerdelivery;
        private Win.UI.Label label9;
        private Win.UI.Label label10;
        private Class.ComboMDivision comboMDivision1;
        private Class.ComboDropDownList comboDropDownList1;
        private Win.UI.Label label1;
        private Class.Txtbrand txtbrand1;
        private Class.Txtstyle txtstyle1;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
    }
}
