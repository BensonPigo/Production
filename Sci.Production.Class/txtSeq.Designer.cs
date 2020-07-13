namespace Sci.Production.Class
{
    partial class TxtSeq
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

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textSeq2 = new Sci.Win.UI.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textSeq1 = new Sci.Win.UI.TextBox();
            this.SuspendLayout();
            // 
            // textSeq2
            // 
            this.textSeq2.IsSupportEditMode = false;
            this.textSeq2.Location = new System.Drawing.Point(38, 0);
            this.textSeq2.MaxLength = 2;
            this.textSeq2.Name = "textSeq2";
            this.textSeq2.Size = new System.Drawing.Size(24, 22);
            this.textSeq2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(9, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "-";
            // 
            // textSeq1
            // 
            this.textSeq1.IsSupportEditMode = false;
            this.textSeq1.Location = new System.Drawing.Point(0, 0);
            this.textSeq1.MaxLength = 3;
            this.textSeq1.Name = "textSeq1";
            this.textSeq1.Size = new System.Drawing.Size(32, 22);
            this.textSeq1.TabIndex = 1;
            // 
            // txtSeq
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.textSeq1);
            this.Controls.Add(this.textSeq2);
            this.Controls.Add(this.label1);
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Name = "txtSeq";
            this.Size = new System.Drawing.Size(61, 23);
            this.Leave += new System.EventHandler(this.TxtSeq_Leave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox textSeq2;
        private System.Windows.Forms.Label label1;
        private Win.UI.TextBox textSeq1;
    }
}
