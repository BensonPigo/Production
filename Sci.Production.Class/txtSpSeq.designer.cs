namespace Sci.Production.Class
{
    partial class TxtSpSeq
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
            this.TextBoxSP = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.TextBoxSeq = new Sci.Win.UI.TextBox();
            this.SuspendLayout();
            // 
            // txtSp
            // 
            this.TextBoxSP.Location = new System.Drawing.Point(106, 2);
            this.TextBoxSP.Name = "txtSp";
            this.TextBoxSP.Size = new System.Drawing.Size(117, 22);
            this.TextBoxSP.TabIndex = 2;
            this.TextBoxSP.Leave += new System.EventHandler(this.TxtSp_Leave);
            this.TextBoxSP.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSp_Validating);
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
            this.TextBoxSeq.Location = new System.Drawing.Point(229, 2);
            this.TextBoxSeq.Name = "txtSeq";
            this.TextBoxSeq.Size = new System.Drawing.Size(30, 22);
            this.TextBoxSeq.TabIndex = 3;
            this.TextBoxSeq.Leave += new System.EventHandler(this.TxtSeq_Leave);
            this.TextBoxSeq.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSeq_Validating);
            // 
            // txtSpSeq
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.TextBoxSeq);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBoxSP);
            this.Name = "txtSpSeq";
            this.Size = new System.Drawing.Size(263, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.Label label1;
    }
}
