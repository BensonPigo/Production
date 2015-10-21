namespace Sci.Production.PPIC
{
    partial class P04_ComboType
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
            this.btmcont.Location = new System.Drawing.Point(0, 286);
            this.btmcont.Size = new System.Drawing.Size(544, 44);
            // 
            // gridcont
            // 
            this.gridcont.Size = new System.Drawing.Size(520, 268);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(460, 8);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(374, 8);
            // 
            // P04_ComboType
            // 
            this.ClientSize = new System.Drawing.Size(544, 330);
            this.GridPopUp = false;
            this.GridUniqueKey = "StyleUkey,Location";
            this.KeyField1 = "StyleUkey";
            this.Name = "P04_ComboType";
            this.Text = "Combo Type";
            this.WorkAlias = "Style_Location";
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
