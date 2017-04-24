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
            this.radiobyfactory = new Sci.Win.UI.RadioButton();
            this.radiobyYear = new Sci.Win.UI.RadioButton();
            this.comboBrand = new Sci.Win.UI.ComboBox();
            this.labelBrand = new Sci.Win.UI.Label();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.label2);
            this.radioPanel1.Controls.Add(this.radiobyfactory);
            this.radioPanel1.Controls.Add(this.radiobyYear);
            this.radioPanel1.Controls.Add(this.comboBrand);
            this.radioPanel1.Controls.Add(this.labelBrand);
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
            // radiobyfactory
            // 
            this.radiobyfactory.AutoSize = true;
            this.radiobyfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobyfactory.Location = new System.Drawing.Point(22, 123);
            this.radiobyfactory.Name = "radiobyfactory";
            this.radiobyfactory.Size = new System.Drawing.Size(93, 21);
            this.radiobyfactory.TabIndex = 3;
            this.radiobyfactory.TabStop = true;
            this.radiobyfactory.Text = "By Factory";
            this.radiobyfactory.UseVisualStyleBackColor = true;
            // 
            // radiobyYear
            // 
            this.radiobyYear.AutoSize = true;
            this.radiobyYear.Checked = true;
            this.radiobyYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobyYear.Location = new System.Drawing.Point(22, 79);
            this.radiobyYear.Name = "radiobyYear";
            this.radiobyYear.Size = new System.Drawing.Size(76, 21);
            this.radiobyYear.TabIndex = 2;
            this.radiobyYear.TabStop = true;
            this.radiobyYear.Text = "By Year";
            this.radiobyYear.UseVisualStyleBackColor = true;
            this.radiobyYear.Value = "By Year";
            // 
            // comboBrand
            // 
            this.comboBrand.BackColor = System.Drawing.Color.White;
            this.comboBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBrand.FormattingEnabled = true;
            this.comboBrand.IsSupportUnselect = true;
            this.comboBrand.Items.AddRange(new object[] {
            "ADIDAS",
            "REEBOK"});
            this.comboBrand.Location = new System.Drawing.Point(81, 19);
            this.comboBrand.Name = "comboBrand";
            this.comboBrand.Size = new System.Drawing.Size(121, 24);
            this.comboBrand.TabIndex = 1;
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(22, 19);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(56, 23);
            this.labelBrand.TabIndex = 0;
            this.labelBrand.Text = "Brand:";
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
        private Win.UI.RadioButton radiobyfactory;
        private Win.UI.RadioButton radiobyYear;
        private Win.UI.ComboBox comboBrand;
        private Win.UI.Label labelBrand;
    }
}
