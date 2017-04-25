namespace Sci.Production.Warehouse
{
    partial class P03_Print
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
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioPurchaseList = new Sci.Win.UI.RadioButton();
            this.radioMaterialStatus = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioPurchaseList);
            this.radioPanel1.Controls.Add(this.radioMaterialStatus);
            this.radioPanel1.Location = new System.Drawing.Point(35, 25);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(387, 204);
            this.radioPanel1.TabIndex = 94;
            this.radioPanel1.Value = "Material Status";
            // 
            // radioPurchaseList
            // 
            this.radioPurchaseList.AutoSize = true;
            this.radioPurchaseList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioPurchaseList.Location = new System.Drawing.Point(26, 114);
            this.radioPurchaseList.Name = "radioPurchaseList";
            this.radioPurchaseList.Size = new System.Drawing.Size(112, 21);
            this.radioPurchaseList.TabIndex = 1;
            this.radioPurchaseList.Text = "Purchase List";
            this.radioPurchaseList.UseVisualStyleBackColor = true;
            this.radioPurchaseList.Value = "Purchase List";
            // 
            // radioMaterialStatus
            // 
            this.radioMaterialStatus.AutoSize = true;
            this.radioMaterialStatus.Checked = true;
            this.radioMaterialStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioMaterialStatus.Location = new System.Drawing.Point(26, 32);
            this.radioMaterialStatus.Name = "radioMaterialStatus";
            this.radioMaterialStatus.Size = new System.Drawing.Size(120, 21);
            this.radioMaterialStatus.TabIndex = 0;
            this.radioMaterialStatus.TabStop = true;
            this.radioMaterialStatus.Text = "Material Status";
            this.radioMaterialStatus.UseVisualStyleBackColor = true;
            this.radioMaterialStatus.Value = "Material Status";
            // 
            // P03_Print
            // 
            this.ClientSize = new System.Drawing.Size(627, 314);
            this.Controls.Add(this.radioPanel1);
            this.Name = "P03_Print";
            this.Text = "() ";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioPurchaseList;
        private Win.UI.RadioButton radioMaterialStatus;


    }
}
