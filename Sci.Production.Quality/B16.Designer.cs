namespace Sci.Production.Quality
{
    partial class B16
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
            this.txtSubProcessID = new Sci.Win.UI.TextBox();
            this.lbSubprocess = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txtAssignColumn = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txtDisplayName = new Sci.Win.UI.TextBox();
            this.txtLocalDisplayName = new Sci.Win.UI.TextBox();
            this.label3 = new Sci.Win.UI.Label();
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
            this.masterpanel.Controls.Add(this.txtLocalDisplayName);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Controls.Add(this.txtDisplayName);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.txtAssignColumn);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Controls.Add(this.txtSubProcessID);
            this.masterpanel.Controls.Add(this.lbSubprocess);
            this.masterpanel.Size = new System.Drawing.Size(938, 78);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbSubprocess, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSubProcessID, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtAssignColumn, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtDisplayName, 0);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLocalDisplayName, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 78);
            this.detailpanel.Size = new System.Drawing.Size(938, 241);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gridicon.Location = new System.Drawing.Point(832, 41);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(827, 1);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(938, 241);
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
            this.detail.Size = new System.Drawing.Size(938, 357);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(938, 319);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 319);
            this.detailbtm.Size = new System.Drawing.Size(938, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(938, 357);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(946, 386);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            this.createby.Visible = false;
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            this.editby.Visible = false;
            // 
            // lblcreateby
            // 
            this.lblcreateby.Visible = false;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            this.lbleditby.Visible = false;
            // 
            // txtSubProcessID
            // 
            this.txtSubProcessID.BackColor = System.Drawing.Color.White;
            this.txtSubProcessID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SubProcessID", true));
            this.txtSubProcessID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSubProcessID.Location = new System.Drawing.Point(133, 12);
            this.txtSubProcessID.Name = "txtSubProcessID";
            this.txtSubProcessID.Size = new System.Drawing.Size(151, 23);
            this.txtSubProcessID.TabIndex = 21;
            this.txtSubProcessID.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSubProcessID_PopUp);
            this.txtSubProcessID.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSubProcessID_Validating);
            // 
            // lbSubprocess
            // 
            this.lbSubprocess.Location = new System.Drawing.Point(22, 13);
            this.lbSubprocess.Name = "lbSubprocess";
            this.lbSubprocess.Size = new System.Drawing.Size(108, 23);
            this.lbSubprocess.TabIndex = 20;
            this.lbSubprocess.Text = "SubProcess";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(22, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 23);
            this.label1.TabIndex = 22;
            this.label1.Text = "Assign Column";
            // 
            // txtAssignColumn
            // 
            this.txtAssignColumn.BackColor = System.Drawing.Color.White;
            this.txtAssignColumn.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "AssignColumn", true));
            this.txtAssignColumn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtAssignColumn.Location = new System.Drawing.Point(133, 41);
            this.txtAssignColumn.Name = "txtAssignColumn";
            this.txtAssignColumn.Size = new System.Drawing.Size(151, 23);
            this.txtAssignColumn.TabIndex = 23;
            this.txtAssignColumn.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtAssignColumn_PopUp);
            this.txtAssignColumn.Validating += new System.ComponentModel.CancelEventHandler(this.TxtAssignColumn_Validating);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(287, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 23);
            this.label2.TabIndex = 24;
            this.label2.Text = "Display Name";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.BackColor = System.Drawing.Color.White;
            this.txtDisplayName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DisplayName", true));
            this.txtDisplayName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDisplayName.Location = new System.Drawing.Point(398, 41);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(151, 23);
            this.txtDisplayName.TabIndex = 25;
            // 
            // txtLocalDisplayName
            // 
            this.txtLocalDisplayName.BackColor = System.Drawing.Color.White;
            this.txtLocalDisplayName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "LocalDisplayName", true));
            this.txtLocalDisplayName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocalDisplayName.Location = new System.Drawing.Point(686, 41);
            this.txtLocalDisplayName.Name = "txtLocalDisplayName";
            this.txtLocalDisplayName.Size = new System.Drawing.Size(126, 23);
            this.txtLocalDisplayName.TabIndex = 27;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(552, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 23);
            this.label3.TabIndex = 26;
            this.label3.Text = "Local Display Name";
            // 
            // B16
            // 
            this.ClientSize = new System.Drawing.Size(946, 419);
            this.EnableGridJunkColor = true;
            this.GridAlias = "SubProCustomColumn_Detail";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "SubProcessID,AssignColumn";
            this.KeyField2 = "SubProcessID,AssignColumn";
            this.Name = "B16";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B16. Sub-Process Custom Column";
            this.WorkAlias = "SubProCustomColumn";
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

        private Win.UI.TextBox txtSubProcessID;
        private Win.UI.Label lbSubprocess;
        private Win.UI.TextBox txtAssignColumn;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtDisplayName;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtLocalDisplayName;
        private Win.UI.Label label3;
    }
}
