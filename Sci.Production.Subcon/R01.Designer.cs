namespace Sci.Production.Subcon
{
    partial class R01
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.cbbFactory = new Sci.Win.UI.ComboBox();
            this.dateRange1 = new Sci.Win.UI.DateRange();
            this.cbbOrderBy = new Sci.Win.UI.ComboBox();
            this.txtSPNO = new Sci.Win.UI.TextBox();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.txtsubcon1 = new Sci.Production.Class.txtsubcon();
            this.txtartworktype_fty1 = new Sci.Production.Class.txtartworktype_fty();
            this.txtstyle1 = new Sci.Production.Class.txtstyle();
            this.label4 = new Sci.Win.UI.Label();
            this.txtMdivision1 = new Sci.Production.Class.txtMdivision();
            this.label9 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(430, 12);
            this.print.TabIndex = 9;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(430, 48);
            this.toexcel.TabIndex = 10;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(430, 84);
            this.close.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(13, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Factory";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(13, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 23);
            this.label2.TabIndex = 95;
            this.label2.Text = "Artwork Type";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(13, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 23);
            this.label5.TabIndex = 98;
            this.label5.Text = "Supplier";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(13, 193);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 23);
            this.label6.TabIndex = 99;
            this.label6.Text = "SP#";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(13, 229);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 23);
            this.label7.TabIndex = 100;
            this.label7.Text = "Style";
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(13, 265);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 23);
            this.label8.TabIndex = 101;
            this.label8.Text = "Order By";
            // 
            // cbbFactory
            // 
            this.cbbFactory.BackColor = System.Drawing.Color.White;
            this.cbbFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbbFactory.FormattingEnabled = true;
            this.cbbFactory.IsSupportUnselect = true;
            this.cbbFactory.Location = new System.Drawing.Point(114, 119);
            this.cbbFactory.Name = "cbbFactory";
            this.cbbFactory.Size = new System.Drawing.Size(121, 24);
            this.cbbFactory.TabIndex = 3;
            // 
            // dateRange1
            // 
            this.dateRange1.Location = new System.Drawing.Point(115, 12);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 23);
            this.dateRange1.TabIndex = 0;
            // 
            // cbbOrderBy
            // 
            this.cbbOrderBy.BackColor = System.Drawing.Color.White;
            this.cbbOrderBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbbOrderBy.FormattingEnabled = true;
            this.cbbOrderBy.IsSupportUnselect = true;
            this.cbbOrderBy.Items.AddRange(new object[] {
            "Issue Date",
            "Supplier"});
            this.cbbOrderBy.Location = new System.Drawing.Point(114, 264);
            this.cbbOrderBy.Name = "cbbOrderBy";
            this.cbbOrderBy.Size = new System.Drawing.Size(121, 24);
            this.cbbOrderBy.TabIndex = 7;
            // 
            // txtSPNO
            // 
            this.txtSPNO.BackColor = System.Drawing.Color.White;
            this.txtSPNO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNO.Location = new System.Drawing.Point(115, 193);
            this.txtSPNO.MaxLength = 13;
            this.txtSPNO.Name = "txtSPNO";
            this.txtSPNO.Size = new System.Drawing.Size(146, 23);
            this.txtSPNO.TabIndex = 5;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(13, 302);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(250, 21);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Outstanding (without payment only)";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // txtsubcon1
            // 
            this.txtsubcon1.DisplayBox1Binding = "";
            this.txtsubcon1.IsIncludeJunk = false;
            this.txtsubcon1.Location = new System.Drawing.Point(115, 157);
            this.txtsubcon1.Name = "txtsubcon1";
            this.txtsubcon1.Size = new System.Drawing.Size(170, 23);
            this.txtsubcon1.TabIndex = 4;
            this.txtsubcon1.TextBox1Binding = "";
            // 
            // txtartworktype_fty1
            // 
            this.txtartworktype_fty1.BackColor = System.Drawing.Color.White;
            this.txtartworktype_fty1.cClassify = "";
            this.txtartworktype_fty1.cSubprocess = "Y";
            this.txtartworktype_fty1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_fty1.Location = new System.Drawing.Point(114, 48);
            this.txtartworktype_fty1.Name = "txtartworktype_fty1";
            this.txtartworktype_fty1.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_fty1.TabIndex = 1;
            // 
            // txtstyle1
            // 
            this.txtstyle1.BackColor = System.Drawing.Color.White;
            this.txtstyle1.BrandObjectName = null;
            this.txtstyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle1.Location = new System.Drawing.Point(115, 229);
            this.txtstyle1.Name = "txtstyle1";
            this.txtstyle1.Size = new System.Drawing.Size(130, 23);
            this.txtstyle1.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(13, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 23);
            this.label4.TabIndex = 105;
            this.label4.Text = "M";
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(114, 84);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(13, 12);
            this.label9.Name = "label9";
            this.label9.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.RectStyle.BorderWidth = 1F;
            this.label9.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label9.RectStyle.ExtBorderWidth = 1F;
            this.label9.Size = new System.Drawing.Size(98, 23);
            this.label9.TabIndex = 106;
            this.label9.Text = "Issue Date";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // R01
            // 
            this.ClientSize = new System.Drawing.Size(522, 360);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMdivision1);
            this.Controls.Add(this.txtstyle1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.txtsubcon1);
            this.Controls.Add(this.txtartworktype_fty1);
            this.Controls.Add(this.txtSPNO);
            this.Controls.Add(this.cbbOrderBy);
            this.Controls.Add(this.dateRange1);
            this.Controls.Add(this.cbbFactory);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.DefaultControl = "dateRange1";
            this.DefaultControlForEdit = "dateRange1";
            this.IsSupportToPrint = false;
            this.Name = "R01";
            this.Text = "R01. Subcon PO List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.cbbFactory, 0);
            this.Controls.SetChildIndex(this.dateRange1, 0);
            this.Controls.SetChildIndex(this.cbbOrderBy, 0);
            this.Controls.SetChildIndex(this.txtSPNO, 0);
            this.Controls.SetChildIndex(this.txtartworktype_fty1, 0);
            this.Controls.SetChildIndex(this.txtsubcon1, 0);
            this.Controls.SetChildIndex(this.checkBox1, 0);
            this.Controls.SetChildIndex(this.txtstyle1, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.Label label8;
        private Win.UI.ComboBox cbbFactory;
        private Win.UI.DateRange dateRange1;
        private Win.UI.ComboBox cbbOrderBy;
        private Win.UI.TextBox txtSPNO;
        private Class.txtartworktype_fty txtartworktype_fty1;
        private Class.txtsubcon txtsubcon1;
        private Win.UI.CheckBox checkBox1;
        private Class.txtstyle txtstyle1;
        private Win.UI.Label label4;
        private Class.txtMdivision txtMdivision1;
        private Win.UI.Label label9;
    }
}
