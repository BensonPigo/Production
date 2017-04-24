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
            this.comboYear = new Sci.Win.UI.ComboBox();
            this.comboBrand = new Sci.Win.UI.ComboBox();
            this.labelYear = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.comboYear);
            this.radioPanel1.Controls.Add(this.comboBrand);
            this.radioPanel1.Controls.Add(this.labelYear);
            this.radioPanel1.Controls.Add(this.labelBrand);
            this.radioPanel1.Location = new System.Drawing.Point(38, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(399, 203);
            this.radioPanel1.TabIndex = 94;
            // 
            // comboYear
            // 
            this.comboYear.BackColor = System.Drawing.Color.White;
            this.comboYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboYear.FormattingEnabled = true;
            this.comboYear.IsSupportUnselect = true;
            this.comboYear.Location = new System.Drawing.Point(88, 118);
            this.comboYear.Name = "comboYear";
            this.comboYear.Size = new System.Drawing.Size(121, 24);
            this.comboYear.TabIndex = 3;
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
            this.comboBrand.Location = new System.Drawing.Point(88, 40);
            this.comboBrand.Name = "comboBrand";
            this.comboBrand.Size = new System.Drawing.Size(121, 24);
            this.comboBrand.TabIndex = 2;
            // 
            // labelYear
            // 
            this.labelYear.Lines = 0;
            this.labelYear.Location = new System.Drawing.Point(34, 119);
            this.labelYear.Name = "labelYear";
            this.labelYear.Size = new System.Drawing.Size(51, 23);
            this.labelYear.TabIndex = 1;
            this.labelYear.Text = "Year";
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(34, 43);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(51, 23);
            this.labelBrand.TabIndex = 0;
            this.labelBrand.Text = "Brand";
            // 
            // R41
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 314);
            this.Controls.Add(this.radioPanel1);
            this.Name = "R41";
            this.Text = "R41.AdiComp Defect  Analysis";
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
        private Win.UI.ComboBox comboYear;
        private Win.UI.ComboBox comboBrand;
        private Win.UI.Label labelYear;
        private Win.UI.Label labelBrand;
    }
}