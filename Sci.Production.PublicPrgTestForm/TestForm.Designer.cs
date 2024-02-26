namespace Sci.Production.PublicPrgTestForm
{
    partial class TestForm
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
            this.btnTestBI = new Ict.Win.UI.Button();
            this.SuspendLayout();
            // 
            // btnTestBI
            // 
            this.btnTestBI.Location = new System.Drawing.Point(290, 133);
            this.btnTestBI.Name = "btnTestBI";
            this.btnTestBI.Size = new System.Drawing.Size(75, 23);
            this.btnTestBI.TabIndex = 0;
            this.btnTestBI.Text = "Test BI";
            this.btnTestBI.UseVisualStyleBackColor = true;
            this.btnTestBI.Click += new System.EventHandler(this.btnTestBI_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnTestBI);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Ict.Win.UI.Button btnTestBI;
    }
}

