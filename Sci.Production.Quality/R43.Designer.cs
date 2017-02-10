namespace Sci.Production.Quality
{
    partial class R43
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
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.comboBox_month = new Sci.Win.UI.ComboBox();
            this.comboBox_year = new Sci.Win.UI.ComboBox();
            this.comboBox_brand = new Sci.Win.UI.ComboBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(343, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(343, 48);
            this.toexcel.TabIndex = 0;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(343, 84);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.comboBox_month);
            this.radioPanel1.Controls.Add(this.comboBox_year);
            this.radioPanel1.Controls.Add(this.comboBox_brand);
            this.radioPanel1.Controls.Add(this.label3);
            this.radioPanel1.Controls.Add(this.label2);
            this.radioPanel1.Controls.Add(this.label1);
            this.radioPanel1.Location = new System.Drawing.Point(26, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(289, 202);
            this.radioPanel1.TabIndex = 94;
            // 
            // comboBox_month
            // 
            this.comboBox_month.BackColor = System.Drawing.Color.White;
            this.comboBox_month.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox_month.FormattingEnabled = true;
            this.comboBox_month.IsSupportUnselect = true;
            this.comboBox_month.Location = new System.Drawing.Point(88, 144);
            this.comboBox_month.Name = "comboBox_month";
            this.comboBox_month.Size = new System.Drawing.Size(121, 24);
            this.comboBox_month.TabIndex = 2;
            // 
            // comboBox_year
            // 
            this.comboBox_year.BackColor = System.Drawing.Color.White;
            this.comboBox_year.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox_year.FormattingEnabled = true;
            this.comboBox_year.IsSupportUnselect = true;
            this.comboBox_year.Location = new System.Drawing.Point(88, 87);
            this.comboBox_year.Name = "comboBox_year";
            this.comboBox_year.Size = new System.Drawing.Size(121, 24);
            this.comboBox_year.TabIndex = 1;
            // 
            // comboBox_brand
            // 
            this.comboBox_brand.BackColor = System.Drawing.Color.White;
            this.comboBox_brand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox_brand.FormattingEnabled = true;
            this.comboBox_brand.IsSupportUnselect = true;
            this.comboBox_brand.Items.AddRange(new object[] {
            "ADIDAS",
            "REEBOK"});
            this.comboBox_brand.Location = new System.Drawing.Point(88, 32);
            this.comboBox_brand.Name = "comboBox_brand";
            this.comboBox_brand.Size = new System.Drawing.Size(121, 24);
            this.comboBox_brand.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(22, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Month";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(22, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Year";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(22, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Brand";
            // 
            // R43
            // 
            this.ClientSize = new System.Drawing.Size(435, 253);
            this.Controls.Add(this.radioPanel1);
            this.Name = "R43";
            this.Text = "R43.AdiComp Detail Form";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.radioPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.ComboBox comboBox_month;
        private Win.UI.ComboBox comboBox_year;
        private Win.UI.ComboBox comboBox_brand;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
    }
}
