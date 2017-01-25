namespace Sci.Production.Cutting
{
    partial class P11_copytocutref
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
            this.label1 = new Sci.Win.UI.Label();
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.Copy = new Sci.Win.UI.Button();
            this.button2 = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(35, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "CutRef#";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(114, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 23);
            this.textBox1.TabIndex = 0;
            // 
            // Copy
            // 
            this.Copy.Location = new System.Drawing.Point(35, 64);
            this.Copy.Name = "Copy";
            this.Copy.Size = new System.Drawing.Size(80, 30);
            this.Copy.TabIndex = 1;
            this.Copy.Text = "Copy";
            this.Copy.UseVisualStyleBackColor = true;
            this.Copy.Click += new System.EventHandler(this.Copy_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(134, 64);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 30);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // P11_copytocutref
            // 
            this.ClientSize = new System.Drawing.Size(254, 110);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Copy);
            this.DefaultControl = "textBox1";
            this.Name = "P11_copytocutref";
            this.Text = "Copy Cutpart/Artwork to other CutRef#";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.TextBox textBox1;
        private Win.UI.Button Copy;
        private Win.UI.Button button2;
    }
}
