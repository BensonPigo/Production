namespace Sci.Production.PPIC
{
    partial class P18
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
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.labelID = new Sci.Win.UI.Label();
            this.labIssueDate = new Sci.Win.UI.Label();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
            this.label1 = new Sci.Win.UI.Label();
            this.labHandle = new Sci.Win.UI.Label();
            this.txtM = new Sci.Win.UI.TextBox();
            this.txtuser = new Sci.Production.Class.txtuser();
            this.labRemark = new Sci.Win.UI.Label();
            this.labStatus = new Sci.Win.UI.Label();
            this.btnSendEMail = new Sci.Win.UI.Button();
            this.textBox1 = new Sci.Win.UI.TextBox();
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
            this.masterpanel.Controls.Add(this.textBox1);
            this.masterpanel.Controls.Add(this.btnSendEMail);
            this.masterpanel.Controls.Add(this.labStatus);
            this.masterpanel.Controls.Add(this.labRemark);
            this.masterpanel.Controls.Add(this.txtuser);
            this.masterpanel.Controls.Add(this.txtM);
            this.masterpanel.Controls.Add(this.labHandle);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Controls.Add(this.labIssueDate);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Controls.Add(this.dateIssueDate);
            this.masterpanel.Size = new System.Drawing.Size(952, 92);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.labHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtM, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuser, 0);
            this.masterpanel.Controls.SetChildIndex(this.labRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labStatus, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnSendEMail, 0);
            this.masterpanel.Controls.SetChildIndex(this.textBox1, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 92);
            this.detailpanel.Size = new System.Drawing.Size(952, 309);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gridicon.Location = new System.Drawing.Point(844, 50);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(952, 309);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(952, 439);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(952, 401);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 401);
            this.detailbtm.Size = new System.Drawing.Size(952, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(925, 439);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(960, 468);
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(109, 16);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(120, 23);
            this.displayID.TabIndex = 2;
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(31, 16);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 3;
            this.labelID.Text = "ID";
            // 
            // labIssueDate
            // 
            this.labIssueDate.Location = new System.Drawing.Point(31, 52);
            this.labIssueDate.Name = "labIssueDate";
            this.labIssueDate.Size = new System.Drawing.Size(75, 23);
            this.labIssueDate.TabIndex = 11;
            this.labIssueDate.Text = "Issue Date";
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "cDate", true));
            this.dateIssueDate.Location = new System.Drawing.Point(110, 52);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(119, 23);
            this.dateIssueDate.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(242, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "M";
            // 
            // labHandle
            // 
            this.labHandle.Location = new System.Drawing.Point(401, 16);
            this.labHandle.Name = "labHandle";
            this.labHandle.Size = new System.Drawing.Size(75, 23);
            this.labHandle.TabIndex = 13;
            this.labHandle.Text = "Handle";
            // 
            // txtM
            // 
            this.txtM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtM.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Mdivisionid", true));
            this.txtM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtM.IsSupportEditMode = false;
            this.txtM.Location = new System.Drawing.Point(320, 16);
            this.txtM.Name = "txtM";
            this.txtM.ReadOnly = true;
            this.txtM.Size = new System.Drawing.Size(77, 23);
            this.txtM.TabIndex = 15;
            // 
            // txtuser
            // 
            this.txtuser.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Handle", true));
            this.txtuser.DisplayBox1Binding = "";
            this.txtuser.Location = new System.Drawing.Point(479, 16);
            this.txtuser.Name = "txtuser";
            this.txtuser.Size = new System.Drawing.Size(251, 23);
            this.txtuser.TabIndex = 16;
            this.txtuser.TextBox1Binding = "";
            // 
            // labRemark
            // 
            this.labRemark.Location = new System.Drawing.Point(242, 52);
            this.labRemark.Name = "labRemark";
            this.labRemark.Size = new System.Drawing.Size(75, 23);
            this.labRemark.TabIndex = 17;
            this.labRemark.Text = "Remark";
            // 
            // labStatus
            // 
            this.labStatus.BackColor = System.Drawing.Color.Transparent;
            this.labStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.labStatus.Location = new System.Drawing.Point(756, 16);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(170, 23);
            this.labStatus.TabIndex = 44;
            this.labStatus.Text = "Not Approve";
            this.labStatus.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // btnSendEMail
            // 
            this.btnSendEMail.Location = new System.Drawing.Point(736, 52);
            this.btnSendEMail.Name = "btnSendEMail";
            this.btnSendEMail.Size = new System.Drawing.Size(95, 30);
            this.btnSendEMail.TabIndex = 45;
            this.btnSendEMail.Text = "Send Email";
            this.btnSendEMail.UseVisualStyleBackColor = true;
            this.btnSendEMail.Click += new System.EventHandler(this.btnSendEMail_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(320, 52);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(410, 23);
            this.textBox1.TabIndex = 46;
            // 
            // P18
            // 
            this.ApvChkValue = "Checked";
            this.CheckChkValue = "Sent";
            this.ClientSize = new System.Drawing.Size(960, 501);
            this.DefaultOrder = "ID";
            this.GridAlias = "AVO_Detail";
            this.IsSupportCheck = true;
            this.IsSupportConfirm = true;
            this.IsSupportRecall = true;
            this.IsSupportSend = true;
            this.IsSupportUncheck = true;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "id";
            this.Name = "P18";
            this.RecallChkValue = "Sent";
            this.SendChkValue = "New";
            this.SubDetailKeyField1 = "ukey";
            this.SubDetailKeyField2 = "AVO_DetailUkey";
            this.SubGridAlias = "AVO_Detail_RefNo";
            this.SubKeyField1 = "Ukey";
            this.Text = "P18. Avoid Verbal Orders";
            this.UnApvChkValue = "Confirmed";
            this.UncheckChkValue = "Checked";
            this.UniqueExpress = "id";
            this.WorkAlias = "AVO";
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

        private Win.UI.DisplayBox displayID;
        private Win.UI.Label labelID;
        private Win.UI.Label labRemark;
        private Class.txtuser txtuser;
        private Win.UI.TextBox txtM;
        private Win.UI.Label labHandle;
        private Win.UI.Label label1;
        private Win.UI.Label labIssueDate;
        private Win.UI.DateBox dateIssueDate;
        private Win.UI.Button btnSendEMail;
        private Win.UI.Label labStatus;
        private Win.UI.TextBox textBox1;
    }
}
