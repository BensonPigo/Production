namespace Sci.Production.Subcon
{
    partial class P26
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
            this.labelTransferID = new Sci.Win.UI.Label();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.labelSubproc = new Sci.Win.UI.Label();
            this.txtTransfer = new Sci.Win.UI.TextBox();
            this.txSubproc = new Sci.Win.UI.TextBox();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).BeginInit();
            this.masterpanel.SuspendLayout();
            this.detailpanel.SuspendLayout();
            this.detail2.SuspendLayout();
            this.detailpanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.txSubproc);
            this.masterpanel.Controls.Add(this.txtTransfer);
            this.masterpanel.Controls.Add(this.labelSubproc);
            this.masterpanel.Controls.Add(this.labelIssueDate);
            this.masterpanel.Controls.Add(this.labelTransferID);
            this.masterpanel.Controls.Add(this.dateIssueDate);
            this.masterpanel.Size = new System.Drawing.Size(892, 41);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTransferID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSubproc, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtTransfer, 0);
            this.masterpanel.Controls.SetChildIndex(this.txSubproc, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 41);
            this.detailpanel.Size = new System.Drawing.Size(892, 308);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(789, 6);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(892, 308);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(892, 387);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(886, 341);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(886, 40);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(892, 387);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(892, 349);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 349);
            this.detailbtm.Size = new System.Drawing.Size(892, 38);
            // 
            // labelTransferID
            // 
            this.labelTransferID.Location = new System.Drawing.Point(5, 9);
            this.labelTransferID.Name = "labelTransferID";
            this.labelTransferID.Size = new System.Drawing.Size(75, 23);
            this.labelTransferID.TabIndex = 2;
            this.labelTransferID.Text = "Transfer ID";
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(256, 9);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.Size = new System.Drawing.Size(75, 23);
            this.labelIssueDate.TabIndex = 3;
            this.labelIssueDate.Text = "IssueDate";
            // 
            // labelSubproc
            // 
            this.labelSubproc.Location = new System.Drawing.Point(504, 9);
            this.labelSubproc.Name = "labelSubproc";
            this.labelSubproc.Size = new System.Drawing.Size(78, 23);
            this.labelSubproc.TabIndex = 4;
            this.labelSubproc.Text = "Subprocess";
            // 
            // txtTransfer
            // 
            this.txtTransfer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtTransfer.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtTransfer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtTransfer.IsSupportEditMode = false;
            this.txtTransfer.Location = new System.Drawing.Point(83, 9);
            this.txtTransfer.Name = "txtTransfer";
            this.txtTransfer.ReadOnly = true;
            this.txtTransfer.Size = new System.Drawing.Size(123, 23);
            this.txtTransfer.TabIndex = 8;
            // 
            // txSubproc
            // 
            this.txSubproc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txSubproc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "StartProcess", true));
            this.txSubproc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txSubproc.IsSupportEditMode = false;
            this.txSubproc.Location = new System.Drawing.Point(585, 9);
            this.txSubproc.Name = "txSubproc";
            this.txSubproc.ReadOnly = true;
            this.txSubproc.Size = new System.Drawing.Size(123, 23);
            this.txSubproc.TabIndex = 10;
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IssueDate", true));
            this.dateIssueDate.IsSupportEditMode = false;
            this.dateIssueDate.Location = new System.Drawing.Point(334, 9);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.ReadOnly = true;
            this.dateIssueDate.Size = new System.Drawing.Size(100, 23);
            this.dateIssueDate.TabIndex = 11;
            // 
            // P26
            // 
            this.ClientSize = new System.Drawing.Size(900, 449);
            this.DefaultControl = "txtTransfer";
            this.DefaultControlForEdit = "txtTransfer";
            this.DefaultFilter = "Id LIKE \'TC%\'";
            this.DefaultOrder = "ID";
            this.ExpressQuery = true;
            this.GridAlias = "BundleTrack_detail";
            this.GridUniqueKey = "ID,BundleNo";
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportLocate = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "ID";
            this.Name = "P26";
            this.Text = "P26. Farm In(Barcode)";
            this.UniqueExpress = "ID";
            this.WorkAlias = "BundleTrack";
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).EndInit();
            this.masterpanel.ResumeLayout(false);
            this.masterpanel.PerformLayout();
            this.detailpanel.ResumeLayout(false);
            this.detail2.ResumeLayout(false);
            this.detailpanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSubproc;
        private Win.UI.Label labelIssueDate;
        private Win.UI.Label labelTransferID;
        private Win.UI.TextBox txSubproc;
        private Win.UI.TextBox txtTransfer;
        private Win.UI.DateBox dateIssueDate;
    }
}
