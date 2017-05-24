namespace Sci.Production.Thread
{
    partial class P02_Detail
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
            this.txtTotalLength = new Sci.Win.UI.TextBox();
            this.labelTotalLength = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridcont
            // 
            this.gridcont.Size = new System.Drawing.Size(672, 333);
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.labelTotalLength);
            this.btmcont.Controls.Add(this.txtTotalLength);
            this.btmcont.Size = new System.Drawing.Size(696, 40);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.next, 0);
            this.btmcont.Controls.SetChildIndex(this.prev, 0);
            this.btmcont.Controls.SetChildIndex(this.txtTotalLength, 0);
            this.btmcont.Controls.SetChildIndex(this.labelTotalLength, 0);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(526, 5);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(606, 5);
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(471, 5);
            // 
            // prev
            // 
            this.prev.Location = new System.Drawing.Point(416, 5);
            // 
            // txtTotalLength
            // 
            this.txtTotalLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalLength.BackColor = System.Drawing.Color.White;
            this.txtTotalLength.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTotalLength.Location = new System.Drawing.Point(338, 8);
            this.txtTotalLength.Name = "txtTotalLength";
            this.txtTotalLength.Size = new System.Drawing.Size(72, 23);
            this.txtTotalLength.TabIndex = 97;
            // 
            // labelTotalLength
            // 
            this.labelTotalLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTotalLength.Location = new System.Drawing.Point(253, 8);
            this.labelTotalLength.Name = "labelTotalLength";
            this.labelTotalLength.Size = new System.Drawing.Size(82, 23);
            this.labelTotalLength.TabIndex = 98;
            this.labelTotalLength.Text = "TotalLength";
            // 
            // P02_Detail
            // 
            this.ClientSize = new System.Drawing.Size(696, 395);
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportUpdate = false;
            this.KeyField1 = "ThreadRequisition_DetailUkey";
            this.Name = "P02_Detail";
            this.Text = "Thread Consumption Detail";
            this.WorkAlias = "ThreadRequisition_Detail_Cons";
            this.Load += new System.EventHandler(this.P02_Detail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.btmcont.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label labelTotalLength;
        private Win.UI.TextBox txtTotalLength;
    }
}
