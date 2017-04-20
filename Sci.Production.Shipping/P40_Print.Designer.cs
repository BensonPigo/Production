namespace Sci.Production.Shipping
{
    partial class P40_Print
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
            this.radioFormForCustomSystem = new Sci.Win.UI.RadioButton();
            this.radioCommercialInvoice = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(256, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(256, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(256, 84);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioFormForCustomSystem);
            this.radioPanel1.Controls.Add(this.radioCommercialInvoice);
            this.radioPanel1.Location = new System.Drawing.Point(7, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(212, 102);
            this.radioPanel1.TabIndex = 94;
            // 
            // radioFormForCustomSystem
            // 
            this.radioFormForCustomSystem.AutoSize = true;
            this.radioFormForCustomSystem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioFormForCustomSystem.Location = new System.Drawing.Point(13, 49);
            this.radioFormForCustomSystem.Name = "radioFormForCustomSystem";
            this.radioFormForCustomSystem.Size = new System.Drawing.Size(176, 21);
            this.radioFormForCustomSystem.TabIndex = 1;
            this.radioFormForCustomSystem.TabStop = true;
            this.radioFormForCustomSystem.Text = "Form for custom system";
            this.radioFormForCustomSystem.UseVisualStyleBackColor = true;
            // 
            // radioCommercialInvoice
            // 
            this.radioCommercialInvoice.AutoSize = true;
            this.radioCommercialInvoice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioCommercialInvoice.Location = new System.Drawing.Point(13, 13);
            this.radioCommercialInvoice.Name = "radioCommercialInvoice";
            this.radioCommercialInvoice.Size = new System.Drawing.Size(147, 21);
            this.radioCommercialInvoice.TabIndex = 0;
            this.radioCommercialInvoice.TabStop = true;
            this.radioCommercialInvoice.Text = "Commercial Invoice";
            this.radioCommercialInvoice.UseVisualStyleBackColor = true;
            // 
            // P40_Print
            // 
            this.ClientSize = new System.Drawing.Size(348, 148);
            this.Controls.Add(this.radioPanel1);
            this.IsSupportToPrint = false;
            this.Name = "P40_Print";
            this.Text = "Print";
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
        private Win.UI.RadioButton radioFormForCustomSystem;
        private Win.UI.RadioButton radioCommercialInvoice;
    }
}
