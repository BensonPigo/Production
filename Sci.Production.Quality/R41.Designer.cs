namespace Sci.Production.Quality
{
    partial class R41
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.combo_Brand = new Sci.Win.UI.ComboBox();
            this.combo_Year = new Sci.Win.UI.ComboBox();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.combo_Year);
            this.radioPanel1.Controls.Add(this.combo_Brand);
            this.radioPanel1.Controls.Add(this.label2);
            this.radioPanel1.Controls.Add(this.label1);
            this.radioPanel1.Location = new System.Drawing.Point(38, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(399, 203);
            this.radioPanel1.TabIndex = 94;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(34, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Brand:";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(34, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Year:";
            // 
            // combo_Brand
            // 
            this.combo_Brand.BackColor = System.Drawing.Color.White;
            this.combo_Brand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.combo_Brand.FormattingEnabled = true;
            this.combo_Brand.IsSupportUnselect = true;
            this.combo_Brand.Location = new System.Drawing.Point(88, 40);
            this.combo_Brand.Name = "combo_Brand";
            this.combo_Brand.Size = new System.Drawing.Size(121, 24);
            this.combo_Brand.TabIndex = 2;
            // 
            // combo_Year
            // 
            this.combo_Year.BackColor = System.Drawing.Color.White;
            this.combo_Year.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.combo_Year.FormattingEnabled = true;
            this.combo_Year.IsSupportUnselect = true;
            this.combo_Year.Location = new System.Drawing.Point(88, 118);
            this.combo_Year.Name = "combo_Year";
            this.combo_Year.Size = new System.Drawing.Size(121, 24);
            this.combo_Year.TabIndex = 3;
            // 
            // R41
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 314);
            this.Controls.Add(this.radioPanel1);
            this.Name = "R41";
            this.Text = "R41";
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
        private Win.UI.ComboBox combo_Year;
        private Win.UI.ComboBox combo_Brand;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
    }
}