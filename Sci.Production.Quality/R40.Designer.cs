namespace Sci.Production.Quality
{
    partial class R40
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
            this.label2 = new Sci.Win.UI.Label();
            this.radiobtn_byfactory = new Sci.Win.UI.RadioButton();
            this.radiobtn_byYear = new Sci.Win.UI.RadioButton();
            this.comboBox_brand = new Sci.Win.UI.ComboBox();
            this.label1 = new Sci.Win.UI.Label();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.label2);
            this.radioPanel1.Controls.Add(this.radiobtn_byfactory);
            this.radioPanel1.Controls.Add(this.radiobtn_byYear);
            this.radioPanel1.Controls.Add(this.comboBox_brand);
            this.radioPanel1.Controls.Add(this.label1);
            this.radioPanel1.Location = new System.Drawing.Point(25, 25);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(406, 217);
            this.radioPanel1.TabIndex = 94;
            this.radioPanel1.Value = "By Year";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(21, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(363, 31);
            this.label2.TabIndex = 4;
            this.label2.Text = "*This report always include 3 years data (include this year)";
            // 
            // radiobtn_byfactory
            // 
            this.radiobtn_byfactory.AutoSize = true;
            this.radiobtn_byfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobtn_byfactory.Location = new System.Drawing.Point(22, 123);
            this.radiobtn_byfactory.Name = "radiobtn_byfactory";
            this.radiobtn_byfactory.Size = new System.Drawing.Size(93, 21);
            this.radiobtn_byfactory.TabIndex = 3;
            this.radiobtn_byfactory.TabStop = true;
            this.radiobtn_byfactory.Text = "By Factory";
            this.radiobtn_byfactory.UseVisualStyleBackColor = true;
            // 
            // radiobtn_byYear
            // 
            this.radiobtn_byYear.AutoSize = true;
            this.radiobtn_byYear.Checked = true;
            this.radiobtn_byYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobtn_byYear.Location = new System.Drawing.Point(22, 79);
            this.radiobtn_byYear.Name = "radiobtn_byYear";
            this.radiobtn_byYear.Size = new System.Drawing.Size(76, 21);
            this.radiobtn_byYear.TabIndex = 2;
            this.radiobtn_byYear.TabStop = true;
            this.radiobtn_byYear.Text = "By Year";
            this.radiobtn_byYear.UseVisualStyleBackColor = true;
            this.radiobtn_byYear.Value = "By Year";
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
            this.comboBox_brand.Location = new System.Drawing.Point(81, 19);
            this.comboBox_brand.Name = "comboBox_brand";
            this.comboBox_brand.Size = new System.Drawing.Size(121, 24);
            this.comboBox_brand.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(22, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Brand:";
            // 
            // R40
            // 
            this.ClientSize = new System.Drawing.Size(627, 314);
            this.Controls.Add(this.radioPanel1);
            this.Name = "R40";
            this.Text = "R40. AdiComp Summary Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.Label label2;
        private Win.UI.RadioButton radiobtn_byfactory;
        private Win.UI.RadioButton radiobtn_byYear;
        private Win.UI.ComboBox comboBox_brand;
        private Win.UI.Label label1;
    }
}
