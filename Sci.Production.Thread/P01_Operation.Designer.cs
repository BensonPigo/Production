namespace Sci.Production.Thread
{
    partial class P01_Operation
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
            // append
            // 
            this.append.Visible = false;
            // 
            // save
            // 
            this.save.Visible = false;
            // 
            // revise
            // 
            this.revise.Visible = false;
            // 
            // delete
            // 
            this.delete.Visible = false;
            // 
            // P01_Operation
            // 
            this.ClientSize = new System.Drawing.Size(677, 395);
            this.Name = "P01_Operation";
            this.Text = "ThreadColorComb_Operation";
            this.WorkAlias = "ThreadColorComb_Operation";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
