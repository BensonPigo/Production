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
            this.txt_CuttingSP2 = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txt_CuttingSP1 = new Sci.Win.UI.TextBox();
            this.cmd_Factory = new Sci.Win.UI.ComboBox();
            this.cmb_M = new Sci.Win.UI.ComboBox();
            this.dateR_EarliestSewingInline = new Sci.Win.UI.DateRange();
            this.dateR_EarliestSCIDelivery = new Sci.Win.UI.DateRange();
            this.dateR_EstCutDate = new Sci.Win.UI.DateRange();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
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
            this.panel1.Controls.Add(this.txt_CuttingSP2);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txt_CuttingSP1);
            this.panel1.Controls.Add(this.cmd_Factory);
            this.panel1.Controls.Add(this.cmb_M);
            this.panel1.Controls.Add(this.dateR_EarliestSewingInline);
            this.panel1.Controls.Add(this.dateR_EarliestSCIDelivery);
            this.panel1.Controls.Add(this.dateR_EstCutDate);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(472, 231);
            this.panel1.TabIndex = 0;
            // 
            // txt_CuttingSP2
            // 
            this.txt_CuttingSP2.BackColor = System.Drawing.Color.White;
            this.txt_CuttingSP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txt_CuttingSP2.Location = new System.Drawing.Point(312, 128);
            this.txt_CuttingSP2.MaxLength = 13;
            this.txt_CuttingSP2.Name = "txt_CuttingSP2";
            this.txt_CuttingSP2.Size = new System.Drawing.Size(126, 23);
            this.txt_CuttingSP2.TabIndex = 4;
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
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(15, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 23);
            this.label2.TabIndex = 96;
            this.label2.Text = "Factory";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 23);
            this.label1.TabIndex = 95;
            this.label1.Text = "M";
            // 
            // txt_CuttingSP1
            // 
            this.txt_CuttingSP1.BackColor = System.Drawing.Color.White;
            this.txt_CuttingSP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txt_CuttingSP1.Location = new System.Drawing.Point(160, 128);
            this.txt_CuttingSP1.MaxLength = 13;
            this.txt_CuttingSP1.Name = "txt_CuttingSP1";
            this.txt_CuttingSP1.Size = new System.Drawing.Size(126, 23);
            this.txt_CuttingSP1.TabIndex = 3;
            // 
            // cmd_Factory
            // 
            this.cmd_Factory.BackColor = System.Drawing.Color.White;
            this.cmd_Factory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmd_Factory.FormattingEnabled = true;
            this.cmd_Factory.IsSupportUnselect = true;
            this.cmd_Factory.Location = new System.Drawing.Point(160, 54);
            this.cmd_Factory.Name = "cmd_Factory";
            this.cmd_Factory.Size = new System.Drawing.Size(121, 24);
            this.cmd_Factory.TabIndex = 1;
            // 
            // cmb_M
            // 
            this.cmb_M.BackColor = System.Drawing.Color.White;
            this.cmb_M.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmb_M.FormattingEnabled = true;
            this.cmb_M.IsSupportUnselect = true;
            this.cmb_M.Location = new System.Drawing.Point(160, 16);
            this.cmb_M.Name = "cmb_M";
            this.cmb_M.Size = new System.Drawing.Size(121, 24);
            this.cmb_M.TabIndex = 0;
            // 
            // dateR_EarliestSewingInline
            // 
            this.dateR_EarliestSewingInline.Location = new System.Drawing.Point(160, 195);
            this.dateR_EarliestSewingInline.Name = "dateR_EarliestSewingInline";
            this.dateR_EarliestSewingInline.Size = new System.Drawing.Size(280, 23);
            this.dateR_EarliestSewingInline.TabIndex = 6;
            // 
            // dateR_EarliestSCIDelivery
            // 
            this.dateR_EarliestSCIDelivery.Location = new System.Drawing.Point(160, 161);
            this.dateR_EarliestSCIDelivery.Name = "dateR_EarliestSCIDelivery";
            this.dateR_EarliestSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateR_EarliestSCIDelivery.TabIndex = 5;
            // 
            // dateR_EstCutDate
            // 
            this.dateR_EstCutDate.Location = new System.Drawing.Point(160, 90);
            this.dateR_EstCutDate.Name = "dateR_EstCutDate";
            this.dateR_EstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateR_EstCutDate.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(16, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(141, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "Earliest Sewing Inline";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(16, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(141, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Earliest SCI Delivery";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(16, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Cutting SP#";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(16, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Est. Cut Date";
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
        private Win.UI.TextBox txt_CuttingSP1;
        private Win.UI.ComboBox cmd_Factory;
        private Win.UI.ComboBox cmb_M;
        private Win.UI.DateRange dateR_EarliestSewingInline;
        private Win.UI.DateRange dateR_EarliestSCIDelivery;
        private Win.UI.DateRange dateR_EstCutDate;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.TextBox txt_CuttingSP2;
        private Win.UI.Label label9;
        private Win.UI.Label label7;
    }
}
