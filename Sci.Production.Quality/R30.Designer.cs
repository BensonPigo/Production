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
            this.textBrand = new Sci.Win.UI.TextBox();
            this.textSeason = new Sci.Win.UI.TextBox();
            this.textStyle = new Sci.Win.UI.TextBox();
            this.dateInspectionDate = new Sci.Win.UI.DateRange();
            this.dateBuyerdelivery = new Sci.Win.UI.DateRange();
            this.label9 = new Sci.Win.UI.Label();
            this.textSP2 = new Sci.Win.UI.TextBox();
            this.textSP1 = new Sci.Win.UI.TextBox();
            this.label8 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
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
            this.panel1.Controls.Add(this.textBrand);
            this.panel1.Controls.Add(this.textSeason);
            this.panel1.Controls.Add(this.textStyle);
            this.panel1.Controls.Add(this.dateInspectionDate);
            this.panel1.Controls.Add(this.dateBuyerdelivery);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.textSP2);
            this.panel1.Controls.Add(this.textSP1);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
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
            // textBrand
            // 
            this.textBrand.BackColor = System.Drawing.Color.White;
            this.textBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBrand.Location = new System.Drawing.Point(120, 157);
            this.textBrand.MaxLength = 10;
            this.textBrand.Name = "textBrand";
            this.textBrand.Size = new System.Drawing.Size(100, 23);
            this.textBrand.TabIndex = 115;
            // 
            // textSeason
            // 
            this.textSeason.BackColor = System.Drawing.Color.White;
            this.textSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textSeason.Location = new System.Drawing.Point(120, 129);
            this.textSeason.MaxLength = 10;
            this.textSeason.Name = "textSeason";
            this.textSeason.Size = new System.Drawing.Size(100, 23);
            this.textSeason.TabIndex = 114;
            // 
            // textStyle
            // 
            this.textStyle.BackColor = System.Drawing.Color.White;
            this.textStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textStyle.Location = new System.Drawing.Point(120, 101);
            this.textStyle.MaxLength = 20;
            this.textStyle.Name = "textStyle";
            this.textStyle.Size = new System.Drawing.Size(178, 23);
            this.textStyle.TabIndex = 113;
            // 
            // dateInspectionDate
            // 
            this.dateInspectionDate.Location = new System.Drawing.Point(120, 73);
            this.dateInspectionDate.Name = "dateInspectionDate";
            this.dateInspectionDate.Size = new System.Drawing.Size(280, 23);
            this.dateInspectionDate.TabIndex = 112;
            // 
            // dateBuyerdelivery
            // 
            this.dateBuyerdelivery.Location = new System.Drawing.Point(120, 45);
            this.dateBuyerdelivery.Name = "dateBuyerdelivery";
            this.dateBuyerdelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerdelivery.TabIndex = 111;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Lines = 0;
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
            // textSP2
            // 
            this.textSP2.BackColor = System.Drawing.Color.White;
            this.textSP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textSP2.Location = new System.Drawing.Point(262, 17);
            this.textSP2.MaxLength = 13;
            this.textSP2.Name = "textSP2";
            this.textSP2.Size = new System.Drawing.Size(116, 23);
            this.textSP2.TabIndex = 9;
            // 
            // textSP1
            // 
            this.textSP1.BackColor = System.Drawing.Color.White;
            this.textSP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textSP1.Location = new System.Drawing.Point(120, 17);
            this.textSP1.MaxLength = 13;
            this.textSP1.Name = "textSP1";
            this.textSP1.Size = new System.Drawing.Size(116, 23);
            this.textSP1.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(12, 213);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 23);
            this.label8.TabIndex = 7;
            this.label8.Text = "Inspected";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(12, 185);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 23);
            this.label7.TabIndex = 6;
            this.label7.Text = "Factory";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(12, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "Brand";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(12, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Season";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(12, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Style";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Inspection Date";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Buyer delivery";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "SP#";
            // 
            // label10
            // 
            this.label10.Lines = 0;
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
            this.Text = "R30.Material Detection List";
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
        private Win.UI.Label label8;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.TextBox textSP2;
        private Win.UI.TextBox textSP1;
        private Win.UI.ComboBox comboInspected;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.TextBox textBrand;
        private Win.UI.TextBox textSeason;
        private Win.UI.TextBox textStyle;
        private Win.UI.DateRange dateInspectionDate;
        private Win.UI.DateRange dateBuyerdelivery;
        private Win.UI.Label label9;
        private Win.UI.Label label10;
    }
}
