namespace Sci.Production.Class
{
    partial class TxtAccountNo
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
            this.TextBox1 = new Sci.Win.UI.TextBox();
            this.DisplayBox1 = new Sci.Win.UI.DisplayBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.TextBox1TextBox1.Location = new System.Drawing.Point(0, 0);
            this.TextBox1.Name = "textBox1";
            this.TextBox1.Size = new System.Drawing.Size(95, 22);
            this.TextBox1.TabIndex = 0;
            this.TextBox1.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.textBox1_PopUp);
            this.TextBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.TextBox1.Leave += new System.EventHandler(this.TextBox1_Leave);
            this.TextBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // displayBox1
            // 
            this.DisplayBox1.Location = new System.Drawing.Point(97, 0);
            this.DisplayBox1.Name = "displayBox1";
            this.DisplayBox1.Size = new System.Drawing.Size(208, 22);
            this.DisplayBox1.TabIndex = 1;
            // 
            // txtAccountNo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.DisplayBox1);
            this.Controls.Add(this.TextBox1);
            this.Name = "txtAccountNo";
            this.Size = new System.Drawing.Size(308, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
