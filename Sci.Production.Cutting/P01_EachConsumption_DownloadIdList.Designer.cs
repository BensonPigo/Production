namespace Sci.Production.Cutting
{
    partial class P01_EachConsumption_DownloadIdList
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
            this.btmcont.Location = new System.Drawing.Point(0, 301);
            this.btmcont.Size = new System.Drawing.Size(464, 44);
            this.btmcont.TabIndex = 0;
            // 
            // gridcont
            // 
            this.gridcont.Size = new System.Drawing.Size(440, 283);
            // 
            // append
            // 
            this.append.TabIndex = 0;
            this.append.Visible = false;
            // 
            // revise
            // 
            this.revise.TabIndex = 1;
            this.revise.Visible = false;
            // 
            // delete
            // 
            this.delete.TabIndex = 2;
            this.delete.Visible = false;
            // 
            // undo
            // 
            this.undo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.undo.Location = new System.Drawing.Point(362, 8);
            this.undo.TabIndex = 4;
            // 
            // save
            // 
            this.save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.save.Location = new System.Drawing.Point(276, 8);
            this.save.TabIndex = 3;
            // 
            // P01_EachConsumption_DownloadIdList
            // 
            this.ClientSize = new System.Drawing.Size(464, 345);
            this.Name = "P01_EachConsumption_DownloadIdList";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Marker Download ID";
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
