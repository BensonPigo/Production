namespace Sci.Production.Class
{
    partial class Txttpeuser
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
            this.DisplayBox1 = new Sci.Win.UI.DisplayBox();
            this.DisplayBox2 = new Sci.Win.UI.DisplayBox();
            this.SuspendLayout();
            // 
            // displayBox1
            // 
            this.DisplayBox1.Location = new System.Drawing.Point(0, 0);
            this.DisplayBox1.Name = "displayBox1";
            this.DisplayBox1.Size = new System.Drawing.Size(80, 22);
            this.DisplayBox1.TabIndex = 0;
            this.DisplayBox1.TextChanged += new System.EventHandler(this.DisplayBox1_TextChanged);
            this.DisplayBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DisplayBox1_MouseDoubleClick);
            // 
            // displayBox2
            // 
            this.DisplayBox2.Location = new System.Drawing.Point(82, 0);
            this.DisplayBox2.Name = "displayBox2";
            this.DisplayBox2.Size = new System.Drawing.Size(220, 22);
            this.DisplayBox2.TabIndex = 1;
            // 
            // txttpeuser
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.DisplayBox2);
            this.Controls.Add(this.DisplayBox1);
            this.Name = "txttpeuser";
            this.Size = new System.Drawing.Size(302, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

    }
}
