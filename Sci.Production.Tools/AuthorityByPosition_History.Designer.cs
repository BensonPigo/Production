namespace Sci.Production.Tools
{
    partial class AuthorityByPosition_History
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
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 347);
            // 
            // gridcont
            // 
            this.gridcont.Size = new System.Drawing.Size(807, 329);
            // 
            // append
            // 
            this.append.EditMode = Sci.Win.UI.AdvEditModes.None;
            // 
            // revise
            // 
            this.revise.EditMode = Sci.Win.UI.AdvEditModes.None;
            // 
            // delete
            // 
            this.delete.EditMode = Sci.Win.UI.AdvEditModes.None;
            // 
            // undo
            // 
            this.undo.EditMode = Sci.Win.UI.AdvEditModes.None;
            // 
            // save
            // 
            this.save.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // AuthorityByPosition_History
            // 
            this.ClientSize = new System.Drawing.Size(831, 391);
            this.GridPopUp = false;
            this.KeyField1 = "ID";
            this.Name = "AuthorityByPosition_History";
            this.Text = "Modify History";
            this.WorkAlias = "PassEdit_History";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
