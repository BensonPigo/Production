namespace Sci.Production.Subcon
{
    partial class R12
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
            this.label3 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.cbbFactory = new Sci.Win.UI.ComboBox();
            this.dateRange1 = new Sci.Win.UI.DateRange();
            this.label4 = new Sci.Win.UI.Label();
            this.txtMdivision1 = new Sci.Production.Class.txtMdivision();
            this.txtsubcon1 = new Sci.Production.Class.txtsubcon();
            this.txtartworktype_fty1 = new Sci.Production.Class.txtartworktype_fty();
            this.cbbOrderBy = new Sci.Win.UI.ComboBox();
            this.label8 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(430, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(430, 48);
            this.toexcel.TabIndex = 9;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(430, 84);
            this.close.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(13, 119);
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
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(13, 12);
            this.label3.Name = "label3";
            this.label3.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label3.RectStyle.BorderWidth = 1F;
            this.label3.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label3.RectStyle.ExtBorderWidth = 1F;
            this.label3.Size = new System.Drawing.Size(98, 23);
            this.label3.TabIndex = 96;
            this.label3.Text = "A/P Date";
            this.label3.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(13, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 23);
            this.label5.TabIndex = 98;
            this.label5.Text = "Supplier";
            // 
            // cbbFactory
            // 
            this.cbbFactory.BackColor = System.Drawing.Color.White;
            this.cbbFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbbFactory.FormattingEnabled = true;
            this.cbbFactory.IsSupportUnselect = true;
            this.cbbFactory.Location = new System.Drawing.Point(114, 118);
            this.cbbFactory.Name = "cbbFactory";
            this.cbbFactory.Size = new System.Drawing.Size(121, 24);
            this.cbbFactory.TabIndex = 5;
            // 
            // dateRange1
            // 
            this.dateRange1.Location = new System.Drawing.Point(115, 12);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 23);
            this.dateRange1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(13, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 23);
            this.label4.TabIndex = 103;
            this.label4.Text = "M";
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(114, 84);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 4;
            // 
            // txtsubcon1
            // 
            this.txtsubcon1.DisplayBox1Binding = "";
            this.txtsubcon1.IsIncludeJunk = false;
            this.txtsubcon1.Location = new System.Drawing.Point(115, 156);
            this.txtsubcon1.Name = "txtsubcon1";
            this.txtsubcon1.Size = new System.Drawing.Size(170, 23);
            this.txtsubcon1.TabIndex = 6;
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
            this.txtartworktype_fty1.TabIndex = 3;
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
            this.cbbOrderBy.Location = new System.Drawing.Point(114, 190);
            this.cbbOrderBy.Name = "cbbOrderBy";
            this.cbbOrderBy.Size = new System.Drawing.Size(121, 24);
            this.cbbOrderBy.TabIndex = 104;
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(13, 191);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 23);
            this.label8.TabIndex = 105;
            this.label8.Text = "Order By";
            // 
            // R12
            // 
            this.ClientSize = new System.Drawing.Size(522, 249);
            this.Controls.Add(this.cbbOrderBy);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMdivision1);
            this.Controls.Add(this.txtsubcon1);
            this.Controls.Add(this.txtartworktype_fty1);
            this.Controls.Add(this.dateRange1);
            this.Controls.Add(this.cbbFactory);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.IsSupportToPrint = false;
            this.Name = "R12";
            this.Text = "R12. Outstanding List of Subcon Payment";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.cbbFactory, 0);
            this.Controls.SetChildIndex(this.dateRange1, 0);
            this.Controls.SetChildIndex(this.txtartworktype_fty1, 0);
            this.Controls.SetChildIndex(this.txtsubcon1, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.cbbOrderBy, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label5;
        private Win.UI.ComboBox cbbFactory;
        private Win.UI.DateRange dateRange1;
        private Class.txtartworktype_fty txtartworktype_fty1;
        private Class.txtsubcon txtsubcon1;
        private Class.txtMdivision txtMdivision1;
        private Win.UI.Label label4;
        private Win.UI.ComboBox cbbOrderBy;
        private Win.UI.Label label8;
    }
}
