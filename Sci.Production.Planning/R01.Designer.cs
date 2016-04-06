namespace Sci.Production.Planning
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
            this.label3 = new Sci.Win.UI.Label();
            this.dateRange1 = new Sci.Win.UI.DateRange();
            this.label14 = new Sci.Win.UI.Label();
            this.txtfactory1 = new Sci.Production.Class.txtfactory();
            this.label4 = new Sci.Win.UI.Label();
            this.txtMdivision1 = new Sci.Production.Class.txtMdivision();
            this.cbxCategory = new Sci.Win.UI.ComboBox();
            this.label7 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(430, 12);
            this.print.TabIndex = 11;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(430, 48);
            this.toexcel.TabIndex = 12;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(430, 84);
            this.close.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(13, 12);
            this.label3.Name = "label3";
            this.label3.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label3.RectStyle.BorderWidth = 1F;
            this.label3.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label3.RectStyle.ExtBorderWidth = 1F;
            this.label3.Size = new System.Drawing.Size(98, 23);
            this.label3.TabIndex = 96;
            this.label3.Text = "SCI Delivery";
            this.label3.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateRange1
            // 
            this.dateRange1.Location = new System.Drawing.Point(115, 12);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 23);
            this.dateRange1.TabIndex = 0;
            // 
            // label14
            // 
            this.label14.Lines = 0;
            this.label14.Location = new System.Drawing.Point(13, 83);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(98, 23);
            this.label14.TabIndex = 132;
            this.label14.Text = "Factory";
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.Location = new System.Drawing.Point(115, 83);
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(13, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 23);
            this.label4.TabIndex = 134;
            this.label4.Text = "M";
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(115, 49);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 133;
            // 
            // cbxCategory
            // 
            this.cbxCategory.BackColor = System.Drawing.Color.White;
            this.cbxCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbxCategory.FormattingEnabled = true;
            this.cbxCategory.IsSupportUnselect = true;
            this.cbxCategory.Items.AddRange(new object[] {
            "Bulk+Sample",
            "Bulk",
            "Sample",
            "Material"});
            this.cbxCategory.Location = new System.Drawing.Point(115, 121);
            this.cbxCategory.Name = "cbxCategory";
            this.cbxCategory.Size = new System.Drawing.Size(121, 24);
            this.cbxCategory.TabIndex = 135;
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(13, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 23);
            this.label7.TabIndex = 136;
            this.label7.Text = "Category";
            // 
            // R01
            // 
            this.ClientSize = new System.Drawing.Size(522, 190);
            this.Controls.Add(this.cbxCategory);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMdivision1);
            this.Controls.Add(this.txtfactory1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.dateRange1);
            this.Controls.Add(this.label3);
            this.IsSupportToPrint = false;
            this.Name = "R01";
            this.Text = "R01. Sub-process Monthly Report";
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.dateRange1, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label14, 0);
            this.Controls.SetChildIndex(this.txtfactory1, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.cbxCategory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label3;
        private Win.UI.DateRange dateRange1;
        private Win.UI.Label label14;
        private Class.txtfactory txtfactory1;
        private Win.UI.Label label4;
        private Class.txtMdivision txtMdivision1;
        private Win.UI.ComboBox cbxCategory;
        private Win.UI.Label label7;
    }
}
