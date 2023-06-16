namespace Sci.Production.PublicForm
{
    partial class ClipGASA01
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
            this.close = new Sci.Win.UI.Button();
            this.save = new Sci.Win.UI.Button();
            this.btm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            this.SuspendLayout();
            // 
            // top
            // 
            this.top.Size = new System.Drawing.Size(876, 10);
            // 
            // btm
            // 
            this.btm.Controls.Add(this.save);
            this.btm.Controls.Add(this.close);
            this.btm.Location = new System.Drawing.Point(10, 337);
            this.btm.Size = new System.Drawing.Size(876, 43);
            // 
            // left
            // 
            this.left.Size = new System.Drawing.Size(10, 380);
            // 
            // right
            // 
            this.right.Location = new System.Drawing.Point(886, 0);
            this.right.Size = new System.Drawing.Size(10, 380);
            // 
            // cont
            // 
            this.cont.Size = new System.Drawing.Size(876, 327);
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.close.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.close.Location = new System.Drawing.Point(790, 6);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(80, 30);
            this.close.TabIndex = 1;
            this.close.TabStop = false;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // save
            // 
            this.save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.save.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.save.Location = new System.Drawing.Point(704, 6);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(80, 30);
            this.save.TabIndex = 1;
            this.save.TabStop = false;
            this.save.Text = "Save";
            this.save.UseVisualStyleBackColor = true;
            // 
            // Clip01
            // 
            this.ClientSize = new System.Drawing.Size(896, 380);
            this.Name = "Clip01";
            this.OnLineHelpID = "Sci.Win.Tools.BaseGrid";
            this.btm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button save;
        private Win.UI.Button close;
    }
}
