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
            this.comboMonth = new Sci.Win.UI.ComboBox();
            this.comboyear = new Sci.Win.UI.ComboBox();
            this.combobrand = new Sci.Win.UI.ComboBox();
            this.labelMonth = new Sci.Win.UI.Label();
            this.labelYear = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
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
            this.radioPanel1.Controls.Add(this.comboMonth);
            this.radioPanel1.Controls.Add(this.comboyear);
            this.radioPanel1.Controls.Add(this.combobrand);
            this.radioPanel1.Controls.Add(this.labelMonth);
            this.radioPanel1.Controls.Add(this.labelYear);
            this.radioPanel1.Controls.Add(this.labelBrand);
            this.radioPanel1.Location = new System.Drawing.Point(26, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(289, 202);
            this.radioPanel1.TabIndex = 94;
            // 
            // comboMonth
            // 
            this.comboMonth.BackColor = System.Drawing.Color.White;
            this.comboMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMonth.FormattingEnabled = true;
            this.comboMonth.IsSupportUnselect = true;
            this.comboMonth.Location = new System.Drawing.Point(88, 144);
            this.comboMonth.Name = "comboMonth";
            this.comboMonth.Size = new System.Drawing.Size(121, 24);
            this.comboMonth.TabIndex = 2;
            // 
            // comboyear
            // 
            this.comboyear.BackColor = System.Drawing.Color.White;
            this.comboyear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboyear.FormattingEnabled = true;
            this.comboyear.IsSupportUnselect = true;
            this.comboyear.Location = new System.Drawing.Point(88, 87);
            this.comboyear.Name = "comboyear";
            this.comboyear.Size = new System.Drawing.Size(121, 24);
            this.comboyear.TabIndex = 1;
            // 
            // combobrand
            // 
            this.combobrand.BackColor = System.Drawing.Color.White;
            this.combobrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.combobrand.FormattingEnabled = true;
            this.combobrand.IsSupportUnselect = true;
            this.combobrand.Items.AddRange(new object[] {
            "ADIDAS",
            "REEBOK"});
            this.combobrand.Location = new System.Drawing.Point(88, 32);
            this.combobrand.Name = "combobrand";
            this.combobrand.Size = new System.Drawing.Size(121, 24);
            this.combobrand.TabIndex = 0;
            // 
            // labelMonth
            // 
            this.labelMonth.Lines = 0;
            this.labelMonth.Location = new System.Drawing.Point(22, 145);
            this.labelMonth.Name = "labelMonth";
            this.labelMonth.Size = new System.Drawing.Size(49, 23);
            this.labelMonth.TabIndex = 2;
            this.labelMonth.Text = "Month";
            // 
            // labelYear
            // 
            this.labelYear.Lines = 0;
            this.labelYear.Location = new System.Drawing.Point(22, 88);
            this.labelYear.Name = "labelYear";
            this.labelYear.Size = new System.Drawing.Size(49, 23);
            this.labelYear.TabIndex = 1;
            this.labelYear.Text = "Year";
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(22, 33);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(49, 23);
            this.labelBrand.TabIndex = 0;
            this.labelBrand.Text = "Brand";
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
        private Win.UI.ComboBox comboMonth;
        private Win.UI.ComboBox comboyear;
        private Win.UI.ComboBox combobrand;
        private Win.UI.Label labelMonth;
        private Win.UI.Label labelYear;
        private Win.UI.Label labelBrand;
    }
}
