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
            this.panel1.Size = new System.Drawing.Size(472, 231);
            this.panel1.TabIndex = 0;
            // 
            // txtCuttingSPEnd
            // 
            this.txtCuttingSPEnd.BackColor = System.Drawing.Color.White;
            this.txtCuttingSPEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCuttingSPEnd.Location = new System.Drawing.Point(312, 128);
            this.txtCuttingSPEnd.MaxLength = 13;
            this.txtCuttingSPEnd.Name = "txtCuttingSPEnd";
            this.txtCuttingSPEnd.Size = new System.Drawing.Size(126, 23);
            this.txtCuttingSPEnd.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(289, 128);
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
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(15, 55);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(142, 23);
            this.labelFactory.TabIndex = 96;
            this.labelFactory.Text = "Factory";
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(16, 16);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(141, 23);
            this.labelM.TabIndex = 95;
            this.labelM.Text = "M";
            // 
            // txtCuttingSPStart
            // 
            this.txtCuttingSPStart.BackColor = System.Drawing.Color.White;
            this.txtCuttingSPStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCuttingSPStart.Location = new System.Drawing.Point(160, 128);
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
            this.comboFactory.Location = new System.Drawing.Point(160, 54);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 1;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(160, 16);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 0;
            // 
            // dateEarliestSewingInline
            // 
            this.dateEarliestSewingInline.IsRequired = false;
            this.dateEarliestSewingInline.Location = new System.Drawing.Point(160, 195);
            this.dateEarliestSewingInline.Name = "dateEarliestSewingInline";
            this.dateEarliestSewingInline.Size = new System.Drawing.Size(280, 23);
            this.dateEarliestSewingInline.TabIndex = 6;
            // 
            // dateEarliestSCIDelivery
            // 
            this.dateEarliestSCIDelivery.IsRequired = false;
            this.dateEarliestSCIDelivery.Location = new System.Drawing.Point(160, 161);
            this.dateEarliestSCIDelivery.Name = "dateEarliestSCIDelivery";
            this.dateEarliestSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateEarliestSCIDelivery.TabIndex = 5;
            // 
            // dateEstCutDate
            // 
            this.dateEstCutDate.IsRequired = false;
            this.dateEstCutDate.Location = new System.Drawing.Point(160, 90);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstCutDate.TabIndex = 2;
            // 
            // labelEarliestSewingInline
            // 
            this.labelEarliestSewingInline.Lines = 0;
            this.labelEarliestSewingInline.Location = new System.Drawing.Point(16, 195);
            this.labelEarliestSewingInline.Name = "labelEarliestSewingInline";
            this.labelEarliestSewingInline.Size = new System.Drawing.Size(141, 23);
            this.labelEarliestSewingInline.TabIndex = 5;
            this.labelEarliestSewingInline.Text = "Earliest Sewing Inline";
            // 
            // labelEarliestSCIDelivery
            // 
            this.labelEarliestSCIDelivery.Lines = 0;
            this.labelEarliestSCIDelivery.Location = new System.Drawing.Point(16, 161);
            this.labelEarliestSCIDelivery.Name = "labelEarliestSCIDelivery";
            this.labelEarliestSCIDelivery.Size = new System.Drawing.Size(141, 23);
            this.labelEarliestSCIDelivery.TabIndex = 4;
            this.labelEarliestSCIDelivery.Text = "Earliest SCI Delivery";
            // 
            // labelCuttingSP
            // 
            this.labelCuttingSP.Lines = 0;
            this.labelCuttingSP.Location = new System.Drawing.Point(16, 128);
            this.labelCuttingSP.Name = "labelCuttingSP";
            this.labelCuttingSP.Size = new System.Drawing.Size(141, 23);
            this.labelCuttingSP.TabIndex = 3;
            this.labelCuttingSP.Text = "Cutting SP#";
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Lines = 0;
            this.labelEstCutDate.Location = new System.Drawing.Point(16, 90);
            this.labelEstCutDate.Name = "labelEstCutDate";
            this.labelEstCutDate.Size = new System.Drawing.Size(141, 23);
            this.labelEstCutDate.TabIndex = 2;
            this.labelEstCutDate.Text = "Est. Cut Date";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(502, 221);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 22);
            this.label7.TabIndex = 97;
            this.label7.Text = "Paper Size A4";
            // 
            // R03
            // 
            this.ClientSize = new System.Drawing.Size(611, 287);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "cmb_M";
            this.DefaultControlForEdit = "cmb_M";
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
    }
}
