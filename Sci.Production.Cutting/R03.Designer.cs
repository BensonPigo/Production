namespace Sci.Production.Cutting
{
    partial class R03
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
            this.dateEarliestBuyerDelivery = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.txtCuttingSPEnd = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.txtCuttingSPStart = new Sci.Win.UI.TextBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.dateEarliestSewingInline = new Sci.Win.UI.DateRange();
            this.dateEarliestSCIDelivery = new Sci.Win.UI.DateRange();
            this.dateEstCutDate = new Sci.Win.UI.DateRange();
            this.labelEarliestSewingInline = new Sci.Win.UI.Label();
            this.labelEarliestSCIDelivery = new Sci.Win.UI.Label();
            this.labelCuttingSP = new Sci.Win.UI.Label();
            this.labelEstCutDate = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.txtstyle1 = new Sci.Production.Class.txtstyle();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.dateSewingInline = new Sci.Win.UI.DateRange();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(519, 12);
            this.print.TabIndex = 12;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(519, 48);
            this.toexcel.TabIndex = 13;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(519, 84);
            this.close.TabIndex = 14;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dateSewingInline);
            this.panel1.Controls.Add(this.dateSCIDelivery);
            this.panel1.Controls.Add(this.dateBuyerDelivery);
            this.panel1.Controls.Add(this.txtstyle1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dateEarliestBuyerDelivery);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtCuttingSPEnd);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelM);
            this.panel1.Controls.Add(this.txtCuttingSPStart);
            this.panel1.Controls.Add(this.comboFactory);
            this.panel1.Controls.Add(this.comboM);
            this.panel1.Controls.Add(this.dateEarliestSewingInline);
            this.panel1.Controls.Add(this.dateEarliestSCIDelivery);
            this.panel1.Controls.Add(this.dateEstCutDate);
            this.panel1.Controls.Add(this.labelEarliestSewingInline);
            this.panel1.Controls.Add(this.labelEarliestSCIDelivery);
            this.panel1.Controls.Add(this.labelCuttingSP);
            this.panel1.Controls.Add(this.labelEstCutDate);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(472, 351);
            this.panel1.TabIndex = 0;
            // 
            // dateEarliestBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateEarliestBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEarliestBuyerDelivery.DateBox1.Name = "";
            this.dateEarliestBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateEarliestBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEarliestBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateEarliestBuyerDelivery.DateBox2.Name = "";
            this.dateEarliestBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateEarliestBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateEarliestBuyerDelivery.IsRequired = false;
            this.dateEarliestBuyerDelivery.Location = new System.Drawing.Point(166, 250);
            this.dateEarliestBuyerDelivery.Name = "dateEarliestBuyerDelivery";
            this.dateEarliestBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateEarliestBuyerDelivery.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 250);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 23);
            this.label1.TabIndex = 97;
            this.label1.Text = "Earliest Buyer Delivery ";
            // 
            // txtCuttingSPEnd
            // 
            this.txtCuttingSPEnd.BackColor = System.Drawing.Color.White;
            this.txtCuttingSPEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCuttingSPEnd.Location = new System.Drawing.Point(318, 105);
            this.txtCuttingSPEnd.MaxLength = 13;
            this.txtCuttingSPEnd.Name = "txtCuttingSPEnd";
            this.txtCuttingSPEnd.Size = new System.Drawing.Size(126, 23);
            this.txtCuttingSPEnd.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(295, 105);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 23);
            this.label9.TabIndex = 7;
            this.label9.Text = "～";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(16, 46);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(147, 23);
            this.labelFactory.TabIndex = 96;
            this.labelFactory.Text = "Factory";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(16, 16);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(147, 23);
            this.labelM.TabIndex = 95;
            this.labelM.Text = "M";
            // 
            // txtCuttingSPStart
            // 
            this.txtCuttingSPStart.BackColor = System.Drawing.Color.White;
            this.txtCuttingSPStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCuttingSPStart.Location = new System.Drawing.Point(166, 105);
            this.txtCuttingSPStart.MaxLength = 13;
            this.txtCuttingSPStart.Name = "txtCuttingSPStart";
            this.txtCuttingSPStart.Size = new System.Drawing.Size(126, 23);
            this.txtCuttingSPStart.TabIndex = 3;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(166, 46);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 1;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(166, 16);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 0;
            // 
            // dateEarliestSewingInline
            // 
            // 
            // 
            // 
            this.dateEarliestSewingInline.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEarliestSewingInline.DateBox1.Name = "";
            this.dateEarliestSewingInline.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateEarliestSewingInline.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEarliestSewingInline.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateEarliestSewingInline.DateBox2.Name = "";
            this.dateEarliestSewingInline.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateEarliestSewingInline.DateBox2.TabIndex = 1;
            this.dateEarliestSewingInline.IsRequired = false;
            this.dateEarliestSewingInline.Location = new System.Drawing.Point(166, 308);
            this.dateEarliestSewingInline.Name = "dateEarliestSewingInline";
            this.dateEarliestSewingInline.Size = new System.Drawing.Size(280, 23);
            this.dateEarliestSewingInline.TabIndex = 11;
            // 
            // dateEarliestSCIDelivery
            // 
            // 
            // 
            // 
            this.dateEarliestSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEarliestSCIDelivery.DateBox1.Name = "";
            this.dateEarliestSCIDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateEarliestSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEarliestSCIDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateEarliestSCIDelivery.DateBox2.Name = "";
            this.dateEarliestSCIDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateEarliestSCIDelivery.DateBox2.TabIndex = 1;
            this.dateEarliestSCIDelivery.IsRequired = false;
            this.dateEarliestSCIDelivery.Location = new System.Drawing.Point(166, 279);
            this.dateEarliestSCIDelivery.Name = "dateEarliestSCIDelivery";
            this.dateEarliestSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateEarliestSCIDelivery.TabIndex = 10;
            // 
            // dateEstCutDate
            // 
            // 
            // 
            // 
            this.dateEstCutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEstCutDate.DateBox1.Name = "";
            this.dateEstCutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateEstCutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEstCutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateEstCutDate.DateBox2.Name = "";
            this.dateEstCutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateEstCutDate.DateBox2.TabIndex = 1;
            this.dateEstCutDate.IsRequired = false;
            this.dateEstCutDate.Location = new System.Drawing.Point(166, 76);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstCutDate.TabIndex = 2;
            // 
            // labelEarliestSewingInline
            // 
            this.labelEarliestSewingInline.Location = new System.Drawing.Point(16, 308);
            this.labelEarliestSewingInline.Name = "labelEarliestSewingInline";
            this.labelEarliestSewingInline.Size = new System.Drawing.Size(147, 23);
            this.labelEarliestSewingInline.TabIndex = 5;
            this.labelEarliestSewingInline.Text = "Earliest Sewing Inline";
            // 
            // labelEarliestSCIDelivery
            // 
            this.labelEarliestSCIDelivery.Location = new System.Drawing.Point(16, 279);
            this.labelEarliestSCIDelivery.Name = "labelEarliestSCIDelivery";
            this.labelEarliestSCIDelivery.Size = new System.Drawing.Size(147, 23);
            this.labelEarliestSCIDelivery.TabIndex = 4;
            this.labelEarliestSCIDelivery.Text = "Earliest SCI Delivery";
            // 
            // labelCuttingSP
            // 
            this.labelCuttingSP.Location = new System.Drawing.Point(16, 105);
            this.labelCuttingSP.Name = "labelCuttingSP";
            this.labelCuttingSP.Size = new System.Drawing.Size(147, 23);
            this.labelCuttingSP.TabIndex = 3;
            this.labelCuttingSP.Text = "Cutting SP#";
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Location = new System.Drawing.Point(16, 76);
            this.labelEstCutDate.Name = "labelEstCutDate";
            this.labelEstCutDate.Size = new System.Drawing.Size(147, 23);
            this.labelEstCutDate.TabIndex = 2;
            this.labelEstCutDate.Text = "Est. Cut Date";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(502, 221);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 22);
            this.label7.TabIndex = 97;
            this.label7.Text = "Paper Size A4";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 221);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 23);
            this.label2.TabIndex = 98;
            this.label2.Text = "Style";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 23);
            this.label3.TabIndex = 99;
            this.label3.Text = "BuyerDelivery";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(147, 23);
            this.label4.TabIndex = 100;
            this.label4.Text = "SCI Delivery";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 192);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(147, 23);
            this.label5.TabIndex = 101;
            this.label5.Text = "Sewing Inline";
            // 
            // txtstyle1
            // 
            this.txtstyle1.BackColor = System.Drawing.Color.White;
            this.txtstyle1.BrandObjectName = null;
            this.txtstyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle1.Location = new System.Drawing.Point(166, 221);
            this.txtstyle1.Name = "txtstyle1";
            this.txtstyle1.Size = new System.Drawing.Size(130, 23);
            this.txtstyle1.TabIndex = 8;
            this.txtstyle1.tarBrand = null;
            this.txtstyle1.tarSeason = null;
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(166, 134);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 5;
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(166, 163);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 6;
            // 
            // dateSewingInline
            // 
            // 
            // 
            // 
            this.dateSewingInline.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSewingInline.DateBox1.Name = "";
            this.dateSewingInline.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSewingInline.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSewingInline.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSewingInline.DateBox2.Name = "";
            this.dateSewingInline.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSewingInline.DateBox2.TabIndex = 1;
            this.dateSewingInline.IsRequired = false;
            this.dateSewingInline.Location = new System.Drawing.Point(166, 192);
            this.dateSewingInline.Name = "dateSewingInline";
            this.dateSewingInline.Size = new System.Drawing.Size(280, 23);
            this.dateSewingInline.TabIndex = 7;
            // 
            // R03
            // 
            this.ClientSize = new System.Drawing.Size(611, 396);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "comboM";
            this.DefaultControlForEdit = "comboM";
            this.IsSupportToPrint = false;
            this.Name = "R03";
            this.Text = "R03.Cutting Schedule List    ";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.TextBox txtCuttingSPStart;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.ComboBox comboM;
        private Win.UI.DateRange dateEarliestSewingInline;
        private Win.UI.DateRange dateEarliestSCIDelivery;
        private Win.UI.DateRange dateEstCutDate;
        private Win.UI.Label labelEarliestSewingInline;
        private Win.UI.Label labelEarliestSCIDelivery;
        private Win.UI.Label labelCuttingSP;
        private Win.UI.Label labelEstCutDate;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.TextBox txtCuttingSPEnd;
        private Win.UI.Label label9;
        private Win.UI.Label label7;
        private Win.UI.DateRange dateEarliestBuyerDelivery;
        private Win.UI.Label label1;
        private Win.UI.DateRange dateSewingInline;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.DateRange dateBuyerDelivery;
        private Class.txtstyle txtstyle1;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
    }
}
