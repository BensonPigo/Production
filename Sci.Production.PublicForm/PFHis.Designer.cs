namespace Sci.Production.PublicForm
{
    partial class PFHis
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
            this.labLockKPILETA = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.labLockKPILETA);
            this.btmcont.Location = new System.Drawing.Point(0, 453);
            this.btmcont.Size = new System.Drawing.Size(831, 44);
            this.btmcont.TabIndex = 0;
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.labLockKPILETA, 0);
            // 
            // append
            // 
            this.append.Size = new System.Drawing.Size(80, 34);
            this.append.TabIndex = 0;
            this.append.Visible = false;
            // 
            // revise
            // 
            this.revise.Size = new System.Drawing.Size(80, 34);
            this.revise.TabIndex = 1;
            this.revise.Visible = false;
            // 
            // delete
            // 
            this.delete.Size = new System.Drawing.Size(80, 34);
            this.delete.TabIndex = 2;
            this.delete.Visible = false;
            // 
            // undo
            // 
            this.undo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.undo.Dock = System.Windows.Forms.DockStyle.None;
            this.undo.Location = new System.Drawing.Point(739, 7);
            this.undo.TabIndex = 4;
            // 
            // save
            // 
            this.save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.save.Dock = System.Windows.Forms.DockStyle.None;
            this.save.Location = new System.Drawing.Point(653, 7);
            this.save.TabIndex = 3;
            // 
            // labLockKPILETA
            // 
            this.labLockKPILETA.AutoSize = true;
            this.labLockKPILETA.BackColor = System.Drawing.Color.Transparent;
            this.labLockKPILETA.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labLockKPILETA.Location = new System.Drawing.Point(11, 6);
            this.labLockKPILETA.MaximumSize = new System.Drawing.Size(65, 32);
            this.labLockKPILETA.MinimumSize = new System.Drawing.Size(65, 32);
            this.labLockKPILETA.Name = "labLockKPILETA";
            this.labLockKPILETA.Size = new System.Drawing.Size(65, 32);
            this.labLockKPILETA.TabIndex = 127;
            this.labLockKPILETA.Text = "Lock";
            this.labLockKPILETA.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.labLockKPILETA.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // PFHis
            // 
            this.ClientSize = new System.Drawing.Size(831, 497);
            this.KeyField1 = "ID";
            this.Name = "PFHis";
            this.Text = "Pull Forward History";
            this.WorkAlias = "Order_PFHis";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.btmcont.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label labLockKPILETA;
    }
}
