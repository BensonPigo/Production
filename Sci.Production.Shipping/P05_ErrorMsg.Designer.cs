namespace Sci.Production.Shipping
{
    partial class P05_ErrorMsg
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelErrMsg = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelErrMsg
            // 
            this.labelErrMsg.AutoSize = true;
            this.labelErrMsg.BackColor = System.Drawing.Color.White;
            this.labelErrMsg.Location = new System.Drawing.Point(2, 2);
            this.labelErrMsg.Name = "labelErrMsg";
            this.labelErrMsg.Size = new System.Drawing.Size(0, 17);
            this.labelErrMsg.TabIndex = 73;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(432, 356);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(78, 27);
            this.buttonOK.TabIndex = 74;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // P05_ErrorMsg
            // 
            this.ClientSize = new System.Drawing.Size(522, 395);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelErrMsg);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "P05_ErrorMsg";
            this.Text = "Warning";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelErrMsg;
        private System.Windows.Forms.Button buttonOK;
    }
}
