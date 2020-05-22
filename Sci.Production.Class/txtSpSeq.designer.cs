namespace Sci.Production.Class
{
    partial class txtSpSeq
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
            this.txtSp = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtSeq = new Sci.Win.UI.TextBox();
            this.SuspendLayout();
            // 
            // txtSp
            // 
            this.txtSp.Location = new System.Drawing.Point(106, 2);
            this.txtSp.Name = "txtSp";
            this.txtSp.Size = new System.Drawing.Size(117, 22);
            this.txtSp.TabIndex = 2;
            this.txtSp.Leave += new System.EventHandler(this.txtSp_Leave);
            this.txtSp.Validating += new System.ComponentModel.CancelEventHandler(this.txtSp_Validating);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "SP# & SEQ";
            // 
            // txtSeq
            // 
            this.txtSeq.Location = new System.Drawing.Point(229, 2);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Size = new System.Drawing.Size(30, 22);
            this.txtSeq.TabIndex = 3;
            this.txtSeq.Leave += new System.EventHandler(this.txtSeq_Leave);
            this.txtSeq.Validating += new System.ComponentModel.CancelEventHandler(this.txtSeq_Validating);
            // 
            // txtSpSeq
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.txtSeq);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSp);
            this.Name = "txtSpSeq";
            this.Size = new System.Drawing.Size(263, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.TextBox txtSp;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtSeq;
    }
}
