namespace Sci.Production.Cutting
{
    partial class P12_Print
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
            this.radioBundleCardRF = new System.Windows.Forms.RadioButton();
            this.radioBundleErase = new System.Windows.Forms.RadioButton();
            this.linkLabelRFCardEraseBeforePrinting1 = new Sci.Production.Class.LinkLabelRFCardEraseBeforePrinting();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(267, 12);
            this.print.TabIndex = 0;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(267, 48);
            this.toexcel.TabIndex = 1;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(267, 84);
            this.close.TabIndex = 2;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(221, 120);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(247, 126);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(247, 124);
            // 
            // radioBundleCardRF
            // 
            this.radioBundleCardRF.AutoSize = true;
            this.radioBundleCardRF.Checked = true;
            this.radioBundleCardRF.Location = new System.Drawing.Point(12, 17);
            this.radioBundleCardRF.Name = "radioBundleCardRF";
            this.radioBundleCardRF.Size = new System.Drawing.Size(83, 21);
            this.radioBundleCardRF.TabIndex = 100;
            this.radioBundleCardRF.TabStop = true;
            this.radioBundleCardRF.Text = "Print(RF)";
            this.radioBundleCardRF.UseVisualStyleBackColor = true;
            // 
            // radioBundleErase
            // 
            this.radioBundleErase.AutoSize = true;
            this.radioBundleErase.Location = new System.Drawing.Point(13, 53);
            this.radioBundleErase.Name = "radioBundleErase";
            this.radioBundleErase.Size = new System.Drawing.Size(91, 21);
            this.radioBundleErase.TabIndex = 104;
            this.radioBundleErase.Text = "Erase(RF)";
            this.radioBundleErase.UseVisualStyleBackColor = true;
            // 
            // linkLabelRFCardEraseBeforePrinting1
            // 
            this.linkLabelRFCardEraseBeforePrinting1.AutoSize = true;
            this.linkLabelRFCardEraseBeforePrinting1.Location = new System.Drawing.Point(12, 91);
            this.linkLabelRFCardEraseBeforePrinting1.Name = "linkLabelRFCardEraseBeforePrinting1";
            this.linkLabelRFCardEraseBeforePrinting1.Size = new System.Drawing.Size(247, 17);
            this.linkLabelRFCardEraseBeforePrinting1.TabIndex = 105;
            this.linkLabelRFCardEraseBeforePrinting1.TabStop = true;
            this.linkLabelRFCardEraseBeforePrinting1.Text = "linkLabelRFCardEraseBeforePrinting1";
            // 
            // P12_Print
            // 
            this.ClientSize = new System.Drawing.Size(359, 178);
            this.Controls.Add(this.linkLabelRFCardEraseBeforePrinting1);
            this.Controls.Add(this.radioBundleErase);
            this.Controls.Add(this.radioBundleCardRF);
            this.DefaultControl = "radioBundleCard";
            this.Name = "P12_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "P12. Print";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.P12_Print_FormClosed);
            this.Controls.SetChildIndex(this.radioBundleCardRF, 0);
            this.Controls.SetChildIndex(this.radioBundleErase, 0);
            this.Controls.SetChildIndex(this.linkLabelRFCardEraseBeforePrinting1, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton radioBundleCardRF;
        private System.Windows.Forms.RadioButton radioBundleErase;
        private Class.LinkLabelRFCardEraseBeforePrinting linkLabelRFCardEraseBeforePrinting1;
    }
}
