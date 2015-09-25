namespace Sci.Production.Shipping
{
    partial class P02_CTNDimensionAndWeight
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
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 252);
            this.btmcont.Size = new System.Drawing.Size(524, 44);
            // 
            // gridcont
            // 
            this.gridcont.Size = new System.Drawing.Size(500, 234);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(440, 8);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(354, 8);
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // P02_CTNDimensionAndWeight
            // 
            this.ClientSize = new System.Drawing.Size(524, 296);
            this.KeyField1 = "ID";
            this.Name = "P02_CTNDimensionAndWeight";
            this.Text = "Carton Dimension & Weight";
            this.WorkAlias = "Express_CTNData";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
