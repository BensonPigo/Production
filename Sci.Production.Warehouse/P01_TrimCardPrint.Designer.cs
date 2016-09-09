namespace Sci.Production.Warehouse
{
    partial class P01_TrimCardPrint
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
            this.radioFabric = new Sci.Win.UI.RadioButton();
            this.radioAccessory = new Sci.Win.UI.RadioButton();
            this.radioOther = new Sci.Win.UI.RadioButton();
            this.radioThread = new Sci.Win.UI.RadioButton();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(242, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(242, 48);
            this.toexcel.Text = "To Word";
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(242, 84);
            // 
            // radioFabric
            // 
            this.radioFabric.AutoSize = true;
            this.radioFabric.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioFabric.Location = new System.Drawing.Point(12, 17);
            this.radioFabric.Name = "radioFabric";
            this.radioFabric.Size = new System.Drawing.Size(65, 21);
            this.radioFabric.TabIndex = 201;
            this.radioFabric.TabStop = true;
            this.radioFabric.Text = "Fabric";
            this.radioFabric.UseVisualStyleBackColor = true;
            // 
            // radioAccessory
            // 
            this.radioAccessory.AutoSize = true;
            this.radioAccessory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioAccessory.Location = new System.Drawing.Point(12, 44);
            this.radioAccessory.Name = "radioAccessory";
            this.radioAccessory.Size = new System.Drawing.Size(91, 21);
            this.radioAccessory.TabIndex = 202;
            this.radioAccessory.TabStop = true;
            this.radioAccessory.Text = "Accessory";
            this.radioAccessory.UseVisualStyleBackColor = true;
            // 
            // radioOther
            // 
            this.radioOther.AutoSize = true;
            this.radioOther.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioOther.Location = new System.Drawing.Point(12, 71);
            this.radioOther.Name = "radioOther";
            this.radioOther.Size = new System.Drawing.Size(62, 21);
            this.radioOther.TabIndex = 203;
            this.radioOther.TabStop = true;
            this.radioOther.Text = "Other";
            this.radioOther.UseVisualStyleBackColor = true;
            // 
            // radioThread
            // 
            this.radioThread.AutoSize = true;
            this.radioThread.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioThread.Location = new System.Drawing.Point(12, 98);
            this.radioThread.Name = "radioThread";
            this.radioThread.Size = new System.Drawing.Size(72, 21);
            this.radioThread.TabIndex = 204;
            this.radioThread.TabStop = true;
            this.radioThread.Text = "Thread";
            this.radioThread.UseVisualStyleBackColor = true;
            // 
            // P01_TrimCardPrint
            // 
            this.ClientSize = new System.Drawing.Size(334, 162);
            this.Controls.Add(this.radioThread);
            this.Controls.Add(this.radioOther);
            this.Controls.Add(this.radioAccessory);
            this.Controls.Add(this.radioFabric);
            this.Name = "P01_TrimCardPrint";
            this.Text = "Trim Card Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioFabric, 0);
            this.Controls.SetChildIndex(this.radioAccessory, 0);
            this.Controls.SetChildIndex(this.radioOther, 0);
            this.Controls.SetChildIndex(this.radioThread, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioButton radioFabric;
        private Win.UI.RadioButton radioAccessory;
        private Win.UI.RadioButton radioOther;
        private Win.UI.RadioButton radioThread;
    }
}
