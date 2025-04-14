namespace Sci.Production.IE
{
    partial class P02_NewCheckList
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
            this.btmcont.Size = new System.Drawing.Size(1225, 40);
            // 
            // gridcont
            // 
            this.gridcont.Size = new System.Drawing.Size(1201, 435);
            // 
            // append
            // 
            this.append.Visible = false;
            // 
            // revise
            // 
            this.revise.Visible = false;
            // 
            // delete
            // 
            this.delete.Visible = false;
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(1135, 5);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(1055, 5);
            this.save.Click += new System.EventHandler(this.Save_Click);
            // 
            // P02_NewCheckList
            // 
            this.ClientSize = new System.Drawing.Size(1225, 497);
            this.GridPopUp = false;
            this.KeyField1 = "ID";
            this.Name = "P02_NewCheckList";
            this.OnLineHelpID = "Sci.Win.Subs.Input4";
            this.Text = "Check List";
            this.WorkAlias = "ChgOver_Check";
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
