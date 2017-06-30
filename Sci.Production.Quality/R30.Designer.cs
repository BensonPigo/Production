namespace Sci.Production.Quality
{
    partial class R30
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
            this.comboInspected = new Sci.Win.UI.ComboBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.txtBrand = new Sci.Win.UI.TextBox();
            this.txtSeason = new Sci.Win.UI.TextBox();
            this.txtStyle = new Sci.Win.UI.TextBox();
            this.dateInspectionDate = new Sci.Win.UI.DateRange();
            this.dateBuyerdelivery = new Sci.Win.UI.DateRange();
            this.label9 = new Sci.Win.UI.Label();
            this.txtSPEnd = new Sci.Win.UI.TextBox();
            this.txtSPStart = new Sci.Win.UI.TextBox();
            this.labelInspected = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelInspectionDate = new Sci.Win.UI.Label();
            this.labelBuyerdelivery = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(458, 12);
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
            this.panel1.Controls.Add(this.comboInspected);
            this.panel1.Controls.Add(this.comboFactory);
            this.panel1.Controls.Add(this.txtBrand);
            this.panel1.Controls.Add(this.txtSeason);
            this.panel1.Controls.Add(this.txtStyle);
            this.panel1.Controls.Add(this.dateInspectionDate);
            this.panel1.Controls.Add(this.dateBuyerdelivery);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtSPEnd);
            this.panel1.Controls.Add(this.txtSPStart);
            this.panel1.Controls.Add(this.labelInspected);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelBrand);
            this.panel1.Controls.Add(this.labelSeason);
            this.panel1.Controls.Add(this.labelStyle);
            this.panel1.Controls.Add(this.labelInspectionDate);
            this.panel1.Controls.Add(this.labelBuyerdelivery);
            this.panel1.Controls.Add(this.labelSP);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(417, 260);
            this.panel1.TabIndex = 94;
            // 
            // comboInspected
            // 
            this.comboInspected.BackColor = System.Drawing.Color.White;
            this.comboInspected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboInspected.FormattingEnabled = true;
            this.comboInspected.IsSupportUnselect = true;
            this.comboInspected.Items.AddRange(new object[] {
            "With Inspection Date",
            "Without Inspection Date",
            "ALL"});
            this.comboInspected.Location = new System.Drawing.Point(120, 213);
            this.comboInspected.Name = "comboInspected";
            this.comboInspected.Size = new System.Drawing.Size(178, 24);
            this.comboInspected.TabIndex = 117;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(120, 185);
            this.comboFactory.MaxLength = 8;
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(116, 24);
            this.comboFactory.TabIndex = 116;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(120, 157);
            this.txtBrand.MaxLength = 10;
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(100, 23);
            this.txtBrand.TabIndex = 115;
            // 
            // txtSeason
            // 
            this.txtSeason.BackColor = System.Drawing.Color.White;
            this.txtSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeason.Location = new System.Drawing.Point(120, 129);
            this.txtSeason.MaxLength = 10;
            this.txtSeason.Name = "txtSeason";
            this.txtSeason.Size = new System.Drawing.Size(100, 23);
            this.txtSeason.TabIndex = 114;
            // 
            // txtStyle
            // 
            this.txtStyle.BackColor = System.Drawing.Color.White;
            this.txtStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStyle.Location = new System.Drawing.Point(120, 101);
            this.txtStyle.MaxLength = 20;
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.Size = new System.Drawing.Size(178, 23);
            this.txtStyle.TabIndex = 113;
            // 
            // dateInspectionDate
            // 
            this.dateInspectionDate.IsRequired = false;
            this.dateInspectionDate.Location = new System.Drawing.Point(120, 73);
            this.dateInspectionDate.Name = "dateInspectionDate";
            this.dateInspectionDate.Size = new System.Drawing.Size(280, 23);
            this.dateInspectionDate.TabIndex = 112;
            // 
            // dateBuyerdelivery
            // 
            this.dateBuyerdelivery.IsRequired = false;
            this.dateBuyerdelivery.Location = new System.Drawing.Point(120, 45);
            this.dateBuyerdelivery.Name = "dateBuyerdelivery";
            this.dateBuyerdelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerdelivery.TabIndex = 111;
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
            // txtSPEnd
            // 
            this.txtSPEnd.BackColor = System.Drawing.Color.White;
            this.txtSPEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPEnd.Location = new System.Drawing.Point(262, 17);
            this.txtSPEnd.MaxLength = 13;
            this.txtSPEnd.Name = "txtSPEnd";
            this.txtSPEnd.Size = new System.Drawing.Size(116, 23);
            this.txtSPEnd.TabIndex = 9;
            // 
            // txtSPStart
            // 
            this.txtSPStart.BackColor = System.Drawing.Color.White;
            this.txtSPStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPStart.Location = new System.Drawing.Point(120, 17);
            this.txtSPStart.MaxLength = 13;
            this.txtSPStart.Name = "txtSPStart";
            this.txtSPStart.Size = new System.Drawing.Size(116, 23);
            this.txtSPStart.TabIndex = 8;
            // 
            // labelInspected
            // 
            this.labelInspected.Location = new System.Drawing.Point(12, 213);
            this.labelInspected.Name = "labelInspected";
            this.labelInspected.Size = new System.Drawing.Size(105, 23);
            this.labelInspected.TabIndex = 7;
            this.labelInspected.Text = "Inspected";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(12, 185);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(105, 23);
            this.labelFactory.TabIndex = 6;
            this.labelFactory.Text = "Factory";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(12, 157);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(105, 23);
            this.labelBrand.TabIndex = 5;
            this.labelBrand.Text = "Brand";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(12, 129);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(105, 23);
            this.labelSeason.TabIndex = 4;
            this.labelSeason.Text = "Season";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(12, 101);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(105, 23);
            this.labelStyle.TabIndex = 3;
            this.labelStyle.Text = "Style";
            // 
            // labelInspectionDate
            // 
            this.labelInspectionDate.Location = new System.Drawing.Point(12, 73);
            this.labelInspectionDate.Name = "labelInspectionDate";
            this.labelInspectionDate.Size = new System.Drawing.Size(105, 23);
            this.labelInspectionDate.TabIndex = 2;
            this.labelInspectionDate.Text = "Inspection Date";
            // 
            // labelBuyerdelivery
            // 
            this.labelBuyerdelivery.Location = new System.Drawing.Point(12, 45);
            this.labelBuyerdelivery.Name = "labelBuyerdelivery";
            this.labelBuyerdelivery.Size = new System.Drawing.Size(105, 23);
            this.labelBuyerdelivery.TabIndex = 1;
            this.labelBuyerdelivery.Text = "Buyer delivery";
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(12, 17);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(105, 23);
            this.labelSP.TabIndex = 0;
            this.labelSP.Text = "SP#";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(444, 126);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 22);
            this.label10.TabIndex = 97;
            this.label10.Text = "Paper Size A4";
            // 
            // R30
            // 
            this.ClientSize = new System.Drawing.Size(550, 306);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panel1);
            this.Name = "R30";
            this.Text = "R30.Metal Detection List";
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
        private Win.UI.Label labelInspectionDate;
        private Win.UI.Label labelBuyerdelivery;
        private Win.UI.Label labelSP;
        private Win.UI.TextBox txtSPEnd;
        private Win.UI.TextBox txtSPStart;
        private Win.UI.ComboBox comboInspected;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.TextBox txtBrand;
        private Win.UI.TextBox txtSeason;
        private Win.UI.TextBox txtStyle;
        private Win.UI.DateRange dateInspectionDate;
        private Win.UI.DateRange dateBuyerdelivery;
        private Win.UI.Label label9;
        private Win.UI.Label label10;
    }
}
