namespace Sci.Production.Subcon
{
    partial class P23
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txSubproc = new Sci.Win.UI.TextBox();
            this.txtTransfer = new Sci.Win.UI.TextBox();
            this.labelSubproc = new Sci.Win.UI.Label();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.labelTransferID = new Sci.Win.UI.Label();
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
            this.masterpanel.Size = new System.Drawing.Size(892, 45);
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
            this.detailpanel.Location = new System.Drawing.Point(0, 45);
            this.detailpanel.Size = new System.Drawing.Size(892, 304);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(787, 7);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(892, 304);
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
            // txSubproc
            // 
            this.txSubproc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txSubproc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "StartProcess", true));
            this.txSubproc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txSubproc.IsSupportEditMode = false;
            this.txSubproc.Location = new System.Drawing.Point(588, 12);
            this.txSubproc.Name = "txSubproc";
            this.txSubproc.ReadOnly = true;
            this.txSubproc.Size = new System.Drawing.Size(123, 23);
            this.txSubproc.TabIndex = 16;
            // 
            // txtTransfer
            // 
            this.txtTransfer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtTransfer.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtTransfer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtTransfer.IsSupportEditMode = false;
            this.txtTransfer.Location = new System.Drawing.Point(86, 12);
            this.txtTransfer.Name = "txtTransfer";
            this.txtTransfer.ReadOnly = true;
            this.txtTransfer.Size = new System.Drawing.Size(123, 23);
            this.txtTransfer.TabIndex = 15;
            // 
            // labelSubproc
            // 
            this.labelSubproc.Location = new System.Drawing.Point(507, 12);
            this.labelSubproc.Name = "labelSubproc";
            this.labelSubproc.Size = new System.Drawing.Size(78, 23);
            this.labelSubproc.TabIndex = 14;
            this.labelSubproc.Text = "Subprocess";
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(259, 12);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.Size = new System.Drawing.Size(75, 23);
            this.labelIssueDate.TabIndex = 13;
            this.labelIssueDate.Text = "IssueDate";
            // 
            // labelTransferID
            // 
            this.labelTransferID.Location = new System.Drawing.Point(8, 12);
            this.labelTransferID.Name = "labelTransferID";
            this.labelTransferID.Size = new System.Drawing.Size(75, 23);
            this.labelTransferID.TabIndex = 12;
            this.labelTransferID.Text = "Transfer ID";
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IssueDate", true));
            this.dateIssueDate.IsSupportEditMode = false;
            this.dateIssueDate.Location = new System.Drawing.Point(337, 12);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.ReadOnly = true;
            this.dateIssueDate.Size = new System.Drawing.Size(100, 23);
            this.dateIssueDate.TabIndex = 17;
            // 
            // P23
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 449);
            this.KeyField1 = "ID";
            this.Name = "P23";
            this.Text = "P23.Farm Out(Barcode)";
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

        private Win.UI.TextBox txSubproc;
        private Win.UI.TextBox txtTransfer;
        private Win.UI.Label labelSubproc;
        private Win.UI.Label labelIssueDate;
        private Win.UI.Label labelTransferID;
        private Win.UI.DateBox dateIssueDate;
    }
}