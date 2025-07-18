namespace Sci.Production.Warehouse
{
    partial class P08_Print
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
            this.radioTapingInformationSheet = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(447, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(447, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(447, 84);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioTapingInformationSheet);
            this.radioPanel1.Location = new System.Drawing.Point(26, 37);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(394, 128);
            this.radioPanel1.TabIndex = 97;
            // 
            // radioTapingInformationSheet
            // 
            this.radioTapingInformationSheet.AutoSize = true;
            this.radioTapingInformationSheet.Checked = true;
            this.radioTapingInformationSheet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioTapingInformationSheet.Location = new System.Drawing.Point(32, 24);
            this.radioTapingInformationSheet.Name = "radioTapingInformationSheet";
            this.radioTapingInformationSheet.Size = new System.Drawing.Size(185, 21);
            this.radioTapingInformationSheet.TabIndex = 0;
            this.radioTapingInformationSheet.TabStop = true;
            this.radioTapingInformationSheet.Text = "Taping Information Sheet";
            this.radioTapingInformationSheet.UseVisualStyleBackColor = true;
            // 
            // P08_Print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 212);
            this.Controls.Add(this.radioPanel1);
            this.Name = "P08_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "P08_Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioTapingInformationSheet;
    }
}