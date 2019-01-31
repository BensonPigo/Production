namespace Sci.Production.Shipping
{
    partial class B48
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labID = new Sci.Win.UI.Label();
            this.labDesc = new Sci.Win.UI.Label();
            this.dispID = new Sci.Win.UI.DisplayBox();
            this.dispDesc = new Sci.Win.UI.DisplayBox();
            this.chkJunk = new Sci.Win.UI.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(713, 353);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.chkJunk);
            this.detailcont.Controls.Add(this.dispDesc);
            this.detailcont.Controls.Add(this.dispID);
            this.detailcont.Controls.Add(this.labDesc);
            this.detailcont.Controls.Add(this.labID);
            this.detailcont.Size = new System.Drawing.Size(713, 315);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 315);
            this.detailbtm.Size = new System.Drawing.Size(713, 38);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(721, 382);
            // 
            // labID
            // 
            this.labID.Location = new System.Drawing.Point(70, 61);
            this.labID.Name = "labID";
            this.labID.Size = new System.Drawing.Size(99, 23);
            this.labID.TabIndex = 0;
            this.labID.Text = "ID";
            // 
            // labDesc
            // 
            this.labDesc.Location = new System.Drawing.Point(70, 110);
            this.labDesc.Name = "labDesc";
            this.labDesc.Size = new System.Drawing.Size(99, 23);
            this.labDesc.TabIndex = 1;
            this.labDesc.Text = "Description";
            // 
            // dispID
            // 
            this.dispID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.dispID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispID.Location = new System.Drawing.Point(172, 61);
            this.dispID.Name = "dispID";
            this.dispID.Size = new System.Drawing.Size(69, 23);
            this.dispID.TabIndex = 2;
            // 
            // dispDesc
            // 
            this.dispDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispDesc.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Description", true));
            this.dispDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispDesc.Location = new System.Drawing.Point(172, 110);
            this.dispDesc.Name = "dispDesc";
            this.dispDesc.Size = new System.Drawing.Size(476, 23);
            this.dispDesc.TabIndex = 3;
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "junk", true));
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkJunk.Location = new System.Drawing.Point(591, 63);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(57, 21);
            this.chkJunk.TabIndex = 4;
            this.chkJunk.Text = "Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // B48
            // 
            this.ClientSize = new System.Drawing.Size(721, 415);
            this.IsDeleteOnBrowse = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B48";
            this.Text = "B48. Adjust Customs Contract Qty Reason";
            this.WorkAlias = "ShippingReason";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labDesc;
        private Win.UI.Label labID;
        private Win.UI.DisplayBox dispDesc;
        private Win.UI.DisplayBox dispID;
        private Win.UI.CheckBox chkJunk;
    }
}
