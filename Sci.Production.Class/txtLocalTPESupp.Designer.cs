namespace Sci.Production.Class
{
    partial class txtLocalTPESupp
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
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(66, 22);
            this.textBox1.TabIndex = 0;
            this.textBox1.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.textBox1_PopUp);
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // displayBox1
            // 
            this.displayBox1.Location = new System.Drawing.Point(67, 0);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(100, 22);
            this.displayBox1.TabIndex = 1;
            // 
            // txtsubcon
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.displayBox1);
            this.Controls.Add(this.textBox1);
            this.Name = "txtsubcon";
            this.Size = new System.Drawing.Size(170, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox textBox1;
        private Win.UI.DisplayBox displayBox1;
    }
}
