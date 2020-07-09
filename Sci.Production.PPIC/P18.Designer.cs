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
            this.labRemark = new Sci.Win.UI.Label();
            this.labStatus = new Sci.Win.UI.Label();
            this.btnSendEMail = new Sci.Win.UI.Button();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.labPPICSupApv = new Sci.Win.UI.Label();
            this.labEditBy = new Sci.Win.UI.Label();
            this.labWHSupApv = new Sci.Win.UI.Label();
            this.labCreate = new Sci.Win.UI.Label();
            this.txtAddName = new Sci.Win.UI.TextBox();
            this.txtPPICSupApv = new Sci.Win.UI.TextBox();
            this.txtEditBy = new Sci.Win.UI.TextBox();
            this.txtWHSupApv = new Sci.Win.UI.TextBox();
            this.txtProdMgrApv = new Sci.Win.UI.TextBox();
            this.txtPPDMgrApv = new Sci.Win.UI.TextBox();
            this.labProdMgrApv = new Sci.Win.UI.Label();
            this.labPPDMgrApv = new Sci.Win.UI.Label();
            this.txtuser = new Sci.Production.Class.Txtuser();
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
            this.masterpanel.Controls.Add(this.txtRemark);
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
            this.masterpanel.Size = new System.Drawing.Size(974, 92);
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
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 92);
            this.detailpanel.Size = new System.Drawing.Size(974, 331);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.gridicon.Location = new System.Drawing.Point(866, 48);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(884, 13);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(974, 331);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(974, 525);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(974, 423);
            // 
            // detailbtm
            // 
            this.detailbtm.Controls.Add(this.txtProdMgrApv);
            this.detailbtm.Controls.Add(this.txtPPDMgrApv);
            this.detailbtm.Controls.Add(this.labProdMgrApv);
            this.detailbtm.Controls.Add(this.labPPDMgrApv);
            this.detailbtm.Controls.Add(this.txtWHSupApv);
            this.detailbtm.Controls.Add(this.txtEditBy);
            this.detailbtm.Controls.Add(this.txtPPICSupApv);
            this.detailbtm.Controls.Add(this.txtAddName);
            this.detailbtm.Controls.Add(this.labCreate);
            this.detailbtm.Controls.Add(this.labWHSupApv);
            this.detailbtm.Controls.Add(this.labEditBy);
            this.detailbtm.Controls.Add(this.labPPICSupApv);
            this.detailbtm.Location = new System.Drawing.Point(0, 423);
            this.detailbtm.Size = new System.Drawing.Size(974, 102);
            this.detailbtm.Controls.SetChildIndex(this.createby, 0);
            this.detailbtm.Controls.SetChildIndex(this.lbleditby, 0);
            this.detailbtm.Controls.SetChildIndex(this.editby, 0);
            this.detailbtm.Controls.SetChildIndex(this.lblcreateby, 0);
            this.detailbtm.Controls.SetChildIndex(this.refresh, 0);
            this.detailbtm.Controls.SetChildIndex(this.labPPICSupApv, 0);
            this.detailbtm.Controls.SetChildIndex(this.labEditBy, 0);
            this.detailbtm.Controls.SetChildIndex(this.labWHSupApv, 0);
            this.detailbtm.Controls.SetChildIndex(this.labCreate, 0);
            this.detailbtm.Controls.SetChildIndex(this.txtAddName, 0);
            this.detailbtm.Controls.SetChildIndex(this.txtPPICSupApv, 0);
            this.detailbtm.Controls.SetChildIndex(this.txtEditBy, 0);
            this.detailbtm.Controls.SetChildIndex(this.txtWHSupApv, 0);
            this.detailbtm.Controls.SetChildIndex(this.labPPDMgrApv, 0);
            this.detailbtm.Controls.SetChildIndex(this.labProdMgrApv, 0);
            this.detailbtm.Controls.SetChildIndex(this.txtPPDMgrApv, 0);
            this.detailbtm.Controls.SetChildIndex(this.txtProdMgrApv, 0);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(974, 525);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(982, 554);
            // 
            // createby
            // 
            this.createby.Location = new System.Drawing.Point(581, 72);
            this.createby.Visible = false;
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(10, 71);
            this.editby.Visible = false;
            // 
            // lblcreateby
            // 
            this.lblcreateby.Location = new System.Drawing.Point(136, 71);
            this.lblcreateby.Size = new System.Drawing.Size(91, 23);
            this.lblcreateby.Visible = false;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(386, 71);
            this.lbleditby.Visible = false;
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(88, 16);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(120, 23);
            this.displayID.TabIndex = 2;
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(10, 16);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 3;
            this.labelID.Text = "ID";
            // 
            // labIssueDate
            // 
            this.labIssueDate.Location = new System.Drawing.Point(10, 52);
            this.labIssueDate.Name = "labIssueDate";
            this.labIssueDate.Size = new System.Drawing.Size(75, 23);
            this.labIssueDate.TabIndex = 11;
            this.labIssueDate.Text = "Issue Date";
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "cDate", true));
            this.dateIssueDate.Location = new System.Drawing.Point(89, 52);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(119, 23);
            this.dateIssueDate.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(221, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "M";
            // 
            // labHandle
            // 
            this.labHandle.Location = new System.Drawing.Point(402, 16);
            this.labHandle.Name = "labHandle";
            this.labHandle.Size = new System.Drawing.Size(72, 23);
            this.labHandle.TabIndex = 13;
            this.labHandle.Text = "Handle";
            // 
            // txtM
            // 
            this.txtM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtM.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Mdivisionid", true));
            this.txtM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtM.IsSupportEditMode = false;
            this.txtM.Location = new System.Drawing.Point(299, 16);
            this.txtM.Name = "txtM";
            this.txtM.ReadOnly = true;
            this.txtM.Size = new System.Drawing.Size(100, 23);
            this.txtM.TabIndex = 15;
            // 
            // labRemark
            // 
            this.labRemark.Location = new System.Drawing.Point(221, 52);
            this.labRemark.Name = "labRemark";
            this.labRemark.Size = new System.Drawing.Size(178, 23);
            this.labRemark.TabIndex = 17;
            this.labRemark.Text = "Special Instruction(Logistic)";
            // 
            // labStatus
            // 
            this.labStatus.BackColor = System.Drawing.Color.Transparent;
            this.labStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.labStatus.Location = new System.Drawing.Point(775, 16);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(170, 23);
            this.labStatus.TabIndex = 44;
            this.labStatus.Text = "Not Approve";
            this.labStatus.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // btnSendEMail
            // 
            this.btnSendEMail.Location = new System.Drawing.Point(755, 48);
            this.btnSendEMail.Name = "btnSendEMail";
            this.btnSendEMail.Size = new System.Drawing.Size(95, 30);
            this.btnSendEMail.TabIndex = 45;
            this.btnSendEMail.Text = "Send Email";
            this.btnSendEMail.UseVisualStyleBackColor = true;
            this.btnSendEMail.Click += new System.EventHandler(this.btnSendEMail_Click);
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(402, 52);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(326, 23);
            this.txtRemark.TabIndex = 46;
            // 
            // labPPICSupApv
            // 
            this.labPPICSupApv.Location = new System.Drawing.Point(5, 43);
            this.labPPICSupApv.Name = "labPPICSupApv";
            this.labPPICSupApv.Size = new System.Drawing.Size(95, 23);
            this.labPPICSupApv.TabIndex = 14;
            this.labPPICSupApv.Text = "PPIC Sup Apv";
            // 
            // labEditBy
            // 
            this.labEditBy.Location = new System.Drawing.Point(452, 13);
            this.labEditBy.Name = "labEditBy";
            this.labEditBy.Size = new System.Drawing.Size(91, 23);
            this.labEditBy.TabIndex = 15;
            this.labEditBy.Text = "EditBy";
            // 
            // labWHSupApv
            // 
            this.labWHSupApv.Location = new System.Drawing.Point(452, 43);
            this.labWHSupApv.Name = "labWHSupApv";
            this.labWHSupApv.Size = new System.Drawing.Size(91, 23);
            this.labWHSupApv.TabIndex = 16;
            this.labWHSupApv.Text = "WH Sup Apv";
            // 
            // labCreate
            // 
            this.labCreate.Location = new System.Drawing.Point(5, 13);
            this.labCreate.Name = "labCreate";
            this.labCreate.Size = new System.Drawing.Size(95, 23);
            this.labCreate.TabIndex = 17;
            this.labCreate.Text = "Create By";
            // 
            // txtAddName
            // 
            this.txtAddName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtAddName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtAddName.IsSupportEditMode = false;
            this.txtAddName.Location = new System.Drawing.Point(103, 13);
            this.txtAddName.Name = "txtAddName";
            this.txtAddName.ReadOnly = true;
            this.txtAddName.Size = new System.Drawing.Size(332, 23);
            this.txtAddName.TabIndex = 18;
            // 
            // txtPPICSupApv
            // 
            this.txtPPICSupApv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtPPICSupApv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtPPICSupApv.IsSupportEditMode = false;
            this.txtPPICSupApv.Location = new System.Drawing.Point(103, 42);
            this.txtPPICSupApv.Name = "txtPPICSupApv";
            this.txtPPICSupApv.ReadOnly = true;
            this.txtPPICSupApv.Size = new System.Drawing.Size(332, 23);
            this.txtPPICSupApv.TabIndex = 19;
            // 
            // txtEditBy
            // 
            this.txtEditBy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtEditBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtEditBy.IsSupportEditMode = false;
            this.txtEditBy.Location = new System.Drawing.Point(546, 13);
            this.txtEditBy.Name = "txtEditBy";
            this.txtEditBy.ReadOnly = true;
            this.txtEditBy.Size = new System.Drawing.Size(332, 23);
            this.txtEditBy.TabIndex = 20;
            // 
            // txtWHSupApv
            // 
            this.txtWHSupApv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtWHSupApv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtWHSupApv.IsSupportEditMode = false;
            this.txtWHSupApv.Location = new System.Drawing.Point(546, 43);
            this.txtWHSupApv.Name = "txtWHSupApv";
            this.txtWHSupApv.ReadOnly = true;
            this.txtWHSupApv.Size = new System.Drawing.Size(332, 23);
            this.txtWHSupApv.TabIndex = 21;
            // 
            // txtProdMgrApv
            // 
            this.txtProdMgrApv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtProdMgrApv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtProdMgrApv.IsSupportEditMode = false;
            this.txtProdMgrApv.Location = new System.Drawing.Point(546, 72);
            this.txtProdMgrApv.Name = "txtProdMgrApv";
            this.txtProdMgrApv.ReadOnly = true;
            this.txtProdMgrApv.Size = new System.Drawing.Size(332, 23);
            this.txtProdMgrApv.TabIndex = 25;
            // 
            // txtPPDMgrApv
            // 
            this.txtPPDMgrApv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtPPDMgrApv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtPPDMgrApv.IsSupportEditMode = false;
            this.txtPPDMgrApv.Location = new System.Drawing.Point(103, 71);
            this.txtPPDMgrApv.Name = "txtPPDMgrApv";
            this.txtPPDMgrApv.ReadOnly = true;
            this.txtPPDMgrApv.Size = new System.Drawing.Size(332, 23);
            this.txtPPDMgrApv.TabIndex = 24;
            // 
            // labProdMgrApv
            // 
            this.labProdMgrApv.Location = new System.Drawing.Point(452, 72);
            this.labProdMgrApv.Name = "labProdMgrApv";
            this.labProdMgrApv.Size = new System.Drawing.Size(91, 23);
            this.labProdMgrApv.TabIndex = 23;
            this.labProdMgrApv.Text = "Prod Mgr Apv";
            // 
            // labPPDMgrApv
            // 
            this.labPPDMgrApv.Location = new System.Drawing.Point(5, 72);
            this.labPPDMgrApv.Name = "labPPDMgrApv";
            this.labPPDMgrApv.Size = new System.Drawing.Size(95, 23);
            this.labPPDMgrApv.TabIndex = 22;
            this.labPPDMgrApv.Text = "PPD Mgr Apv";
            // 
            // txtuser
            // 
            this.txtuser.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Handle", true));
            this.txtuser.DisplayBox1Binding = "";
            this.txtuser.Location = new System.Drawing.Point(477, 16);
            this.txtuser.Name = "txtuser";
            this.txtuser.Size = new System.Drawing.Size(251, 23);
            this.txtuser.TabIndex = 16;
            this.txtuser.TextBox1Binding = "";
            // 
            // P18
            // 
            this.ApvChkValue = "Received";
            this.CheckChkValue = "Sent";
            this.ClientSize = new System.Drawing.Size(982, 587);
            this.DefaultOrder = "ID";
            this.GridAlias = "AVO_Detail";
            this.IsSupportCheck = true;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportRecall = true;
            this.IsSupportReceive = true;
            this.IsSupportReturn = true;
            this.IsSupportSend = true;
            this.IsSupportUncheck = true;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "id";
            this.Name = "P18";
            this.OnLineHelpID = "Sci.Win.Tems.Input8";
            this.RecallChkValue = "Sent";
            this.ReceiveChkValue = "Checked";
            this.ReturnChkValue = "Received";
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
        private Class.Txtuser txtuser;
        private Win.UI.TextBox txtM;
        private Win.UI.Label labHandle;
        private Win.UI.Label label1;
        private Win.UI.Label labIssueDate;
        private Win.UI.DateBox dateIssueDate;
        private Win.UI.Button btnSendEMail;
        private Win.UI.Label labStatus;
        private Win.UI.TextBox txtRemark;
        private Win.UI.Label labPPICSupApv;
        private Win.UI.Label labWHSupApv;
        private Win.UI.Label labEditBy;
        private Win.UI.TextBox txtWHSupApv;
        private Win.UI.TextBox txtEditBy;
        private Win.UI.TextBox txtPPICSupApv;
        private Win.UI.TextBox txtAddName;
        private Win.UI.Label labCreate;
        private Win.UI.TextBox txtProdMgrApv;
        private Win.UI.TextBox txtPPDMgrApv;
        private Win.UI.Label labProdMgrApv;
        private Win.UI.Label labPPDMgrApv;
    }
}
