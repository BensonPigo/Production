namespace Sci.Production.Quality
{
    partial class R42
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.combo_Brand = new Sci.Win.UI.ComboBox();
            this.radiobtn_pill_snagg_detail = new Sci.Win.UI.RadioButton();
            this.radiobtn_print_detail = new Sci.Win.UI.RadioButton();
            this.combo_Year = new Sci.Win.UI.ComboBox();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toexcel
            // 
            this.toexcel.TabIndex = 0;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.combo_Year);
            this.radioPanel1.Controls.Add(this.radiobtn_print_detail);
            this.radioPanel1.Controls.Add(this.radiobtn_pill_snagg_detail);
            this.radioPanel1.Controls.Add(this.combo_Brand);
            this.radioPanel1.Controls.Add(this.label3);
            this.radioPanel1.Controls.Add(this.label1);
            this.radioPanel1.Controls.Add(this.label2);
            this.radioPanel1.Location = new System.Drawing.Point(28, 29);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(459, 207);
            this.radioPanel1.TabIndex = 94;
            this.radioPanel1.Value = "Pilling and Snagging Detail ";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(28, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Brand:";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(28, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Year:";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(28, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Report Type:";
            // 
            // combo_Brand
            // 
            this.combo_Brand.BackColor = System.Drawing.Color.White;
            this.combo_Brand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.combo_Brand.FormattingEnabled = true;
            this.combo_Brand.IsSupportUnselect = true;
            this.combo_Brand.Items.AddRange(new object[] {
            "ADIDAS",
            "REEBOK"});
            this.combo_Brand.Location = new System.Drawing.Point(81, 26);
            this.combo_Brand.Name = "combo_Brand";
            this.combo_Brand.Size = new System.Drawing.Size(121, 24);
            this.combo_Brand.TabIndex = 0;
            // 
            // radiobtn_pill_snagg_detail
            // 
            this.radiobtn_pill_snagg_detail.AutoSize = true;
            this.radiobtn_pill_snagg_detail.Checked = true;
            this.radiobtn_pill_snagg_detail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobtn_pill_snagg_detail.Location = new System.Drawing.Point(124, 136);
            this.radiobtn_pill_snagg_detail.Name = "radiobtn_pill_snagg_detail";
            this.radiobtn_pill_snagg_detail.Size = new System.Drawing.Size(199, 21);
            this.radiobtn_pill_snagg_detail.TabIndex = 2;
            this.radiobtn_pill_snagg_detail.TabStop = true;
            this.radiobtn_pill_snagg_detail.Text = "Pilling and Snagging Detail ";
            this.radiobtn_pill_snagg_detail.UseVisualStyleBackColor = true;
            this.radiobtn_pill_snagg_detail.Value = "Pilling and Snagging Detail ";
            // 
            // radiobtn_print_detail
            // 
            this.radiobtn_print_detail.AutoSize = true;
            this.radiobtn_print_detail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobtn_print_detail.Location = new System.Drawing.Point(124, 174);
            this.radiobtn_print_detail.Name = "radiobtn_print_detail";
            this.radiobtn_print_detail.Size = new System.Drawing.Size(114, 21);
            this.radiobtn_print_detail.TabIndex = 3;
            this.radiobtn_print_detail.TabStop = true;
            this.radiobtn_print_detail.Text = "Printing Detail";
            this.radiobtn_print_detail.UseVisualStyleBackColor = true;
            // 
            // combo_Year
            // 
            this.combo_Year.BackColor = System.Drawing.Color.White;
            this.combo_Year.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.combo_Year.FormattingEnabled = true;
            this.combo_Year.IsSupportUnselect = true;
            this.combo_Year.Location = new System.Drawing.Point(81, 75);
            this.combo_Year.Name = "combo_Year";
            this.combo_Year.Size = new System.Drawing.Size(121, 24);
            this.combo_Year.TabIndex = 1;
            // 
            // R42
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 314);
            this.Controls.Add(this.radioPanel1);
            this.Name = "R42";
            this.Text = "R42.Supplier Claim";
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
        private Win.UI.Label label3;
        private Win.UI.Label label1;
        private Win.UI.ComboBox combo_Brand;
        private Win.UI.RadioButton radiobtn_pill_snagg_detail;
        private Win.UI.ComboBox combo_Year;
        private Win.UI.RadioButton radiobtn_print_detail;
    }
}