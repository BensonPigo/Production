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
            this.label1 = new Sci.Win.UI.Label();
            this.dateEarliestBuyerDelivery = new Sci.Win.UI.DateRange();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(519, 12);
            this.print.TabIndex = 1;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(519, 48);
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(519, 84);
            this.close.TabIndex = 3;
            // 
            // panel1
            // 
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
            this.panel1.Size = new System.Drawing.Size(472, 268);
            this.panel1.TabIndex = 0;
            // 
            // txtCuttingSPEnd
            // 
            this.txtCuttingSPEnd.BackColor = System.Drawing.Color.White;
            this.txtCuttingSPEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCuttingSPEnd.Location = new System.Drawing.Point(318, 121);
            this.txtCuttingSPEnd.MaxLength = 13;
            this.txtCuttingSPEnd.Name = "txtCuttingSPEnd";
            this.txtCuttingSPEnd.Size = new System.Drawing.Size(126, 23);
            this.txtCuttingSPEnd.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(295, 121);
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
            this.labelFactory.Location = new System.Drawing.Point(15, 51);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(148, 23);
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
            this.txtCuttingSPStart.Location = new System.Drawing.Point(166, 121);
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
            this.comboFactory.Location = new System.Drawing.Point(166, 51);
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
            this.dateEarliestSewingInline.Location = new System.Drawing.Point(166, 226);
            this.dateEarliestSewingInline.Name = "dateEarliestSewingInline";
            this.dateEarliestSewingInline.Size = new System.Drawing.Size(280, 23);
            this.dateEarliestSewingInline.TabIndex = 7;
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
            this.dateEarliestSCIDelivery.Location = new System.Drawing.Point(166, 191);
            this.dateEarliestSCIDelivery.Name = "dateEarliestSCIDelivery";
            this.dateEarliestSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateEarliestSCIDelivery.TabIndex = 6;
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
            this.dateEstCutDate.Location = new System.Drawing.Point(166, 86);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstCutDate.TabIndex = 2;
            // 
            // labelEarliestSewingInline
            // 
            this.labelEarliestSewingInline.Location = new System.Drawing.Point(14, 226);
            this.labelEarliestSewingInline.Name = "labelEarliestSewingInline";
            this.labelEarliestSewingInline.Size = new System.Drawing.Size(147, 23);
            this.labelEarliestSewingInline.TabIndex = 5;
            this.labelEarliestSewingInline.Text = "Earliest Sewing Inline";
            // 
            // labelEarliestSCIDelivery
            // 
            this.labelEarliestSCIDelivery.Location = new System.Drawing.Point(14, 191);
            this.labelEarliestSCIDelivery.Name = "labelEarliestSCIDelivery";
            this.labelEarliestSCIDelivery.Size = new System.Drawing.Size(147, 23);
            this.labelEarliestSCIDelivery.TabIndex = 4;
            this.labelEarliestSCIDelivery.Text = "Earliest SCI Delivery";
            // 
            // labelCuttingSP
            // 
            this.labelCuttingSP.Location = new System.Drawing.Point(16, 121);
            this.labelCuttingSP.Name = "labelCuttingSP";
            this.labelCuttingSP.Size = new System.Drawing.Size(147, 23);
            this.labelCuttingSP.TabIndex = 3;
            this.labelCuttingSP.Text = "Cutting SP#";
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Location = new System.Drawing.Point(16, 86);
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
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 23);
            this.label1.TabIndex = 97;
            this.label1.Text = "Earliest Buyer Delivery ";
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
            this.dateEarliestBuyerDelivery.Location = new System.Drawing.Point(166, 156);
            this.dateEarliestBuyerDelivery.Name = "dateEarliestBuyerDelivery";
            this.dateEarliestBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateEarliestBuyerDelivery.TabIndex = 5;
            // 
            // R03
            // 
            this.ClientSize = new System.Drawing.Size(611, 361);
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
    }
}
