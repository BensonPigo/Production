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
            this.comboYear = new Sci.Win.UI.ComboBox();
            this.radioPrintDetail = new Sci.Win.UI.RadioButton();
            this.radioPillAndSnaggDetail = new Sci.Win.UI.RadioButton();
            this.comboBrand = new Sci.Win.UI.ComboBox();
            this.labelReportType = new Sci.Win.UI.Label();
            this.labelYear = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toexcel
            // 
            this.toexcel.TabIndex = 0;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.comboYear);
            this.radioPanel1.Controls.Add(this.radioPrintDetail);
            this.radioPanel1.Controls.Add(this.radioPillAndSnaggDetail);
            this.radioPanel1.Controls.Add(this.comboBrand);
            this.radioPanel1.Controls.Add(this.labelReportType);
            this.radioPanel1.Controls.Add(this.labelYear);
            this.radioPanel1.Controls.Add(this.labelBrand);
            this.radioPanel1.Location = new System.Drawing.Point(28, 29);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(459, 207);
            this.radioPanel1.TabIndex = 94;
            this.radioPanel1.Value = "Pilling and Snagging Detail ";
            // 
            // comboYear
            // 
            this.comboYear.BackColor = System.Drawing.Color.White;
            this.comboYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboYear.FormattingEnabled = true;
            this.comboYear.IsSupportUnselect = true;
            this.comboYear.Location = new System.Drawing.Point(81, 75);
            this.comboYear.Name = "comboYear";
            this.comboYear.Size = new System.Drawing.Size(121, 24);
            this.comboYear.TabIndex = 1;
            // 
            // radioPrintDetail
            // 
            this.radioPrintDetail.AutoSize = true;
            this.radioPrintDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioPrintDetail.Location = new System.Drawing.Point(124, 174);
            this.radioPrintDetail.Name = "radioPrintDetail";
            this.radioPrintDetail.Size = new System.Drawing.Size(114, 21);
            this.radioPrintDetail.TabIndex = 3;
            this.radioPrintDetail.TabStop = true;
            this.radioPrintDetail.Text = "Printing Detail";
            this.radioPrintDetail.UseVisualStyleBackColor = true;
            // 
            // radioPillAndSnaggDetail
            // 
            this.radioPillAndSnaggDetail.AutoSize = true;
            this.radioPillAndSnaggDetail.Checked = true;
            this.radioPillAndSnaggDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioPillAndSnaggDetail.Location = new System.Drawing.Point(124, 136);
            this.radioPillAndSnaggDetail.Name = "radioPillAndSnaggDetail";
            this.radioPillAndSnaggDetail.Size = new System.Drawing.Size(199, 21);
            this.radioPillAndSnaggDetail.TabIndex = 2;
            this.radioPillAndSnaggDetail.TabStop = true;
            this.radioPillAndSnaggDetail.Text = "Pilling and Snagging Detail ";
            this.radioPillAndSnaggDetail.UseVisualStyleBackColor = true;
            this.radioPillAndSnaggDetail.Value = "Pilling and Snagging Detail ";
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
            this.comboBrand.Location = new System.Drawing.Point(81, 26);
            this.comboBrand.Name = "comboBrand";
            this.comboBrand.Size = new System.Drawing.Size(121, 24);
            this.comboBrand.TabIndex = 0;
            // 
            // labelReportType
            // 
            this.labelReportType.Lines = 0;
            this.labelReportType.Location = new System.Drawing.Point(28, 136);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(90, 23);
            this.labelReportType.TabIndex = 2;
            this.labelReportType.Text = "Report Type";
            // 
            // labelYear
            // 
            this.labelYear.Lines = 0;
            this.labelYear.Location = new System.Drawing.Point(28, 75);
            this.labelYear.Name = "labelYear";
            this.labelYear.Size = new System.Drawing.Size(50, 23);
            this.labelYear.TabIndex = 1;
            this.labelYear.Text = "Year";
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(28, 26);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(50, 23);
            this.labelBrand.TabIndex = 0;
            this.labelBrand.Text = "Brand";
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
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelReportType;
        private Win.UI.Label labelYear;
        private Win.UI.ComboBox comboBrand;
        private Win.UI.RadioButton radioPillAndSnaggDetail;
        private Win.UI.ComboBox comboYear;
        private Win.UI.RadioButton radioPrintDetail;
    }
}