namespace Sci.Production.Warehouse
{
    partial class P62
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
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.labelID = new Sci.Win.UI.Label();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
            this.txtRequest = new Sci.Win.UI.TextBox();
            this.labelRequest = new Sci.Win.UI.Label();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.labelRemark = new Sci.Win.UI.Label();
            this.labelNotApprove = new Sci.Win.UI.Label();
            this.btnAutoPick = new Sci.Win.UI.Button();
            this.btnCutRefNo = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.btnCutRefNo);
            this.masterpanel.Controls.Add(this.btnAutoPick);
            this.masterpanel.Controls.Add(this.labelNotApprove);
            this.masterpanel.Controls.Add(this.txtRemark);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.txtRequest);
            this.masterpanel.Controls.Add(this.labelRequest);
            this.masterpanel.Controls.Add(this.labelIssueDate);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Controls.Add(this.dateIssueDate);
            this.masterpanel.Size = new System.Drawing.Size(1009, 102);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRequest, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRequest, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNotApprove, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnAutoPick, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnCutRefNo, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 102);
            this.detailpanel.Size = new System.Drawing.Size(1009, 341);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(5, 67);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1009, 341);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(1009, 481);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1009, 443);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 443);
            this.detailbtm.Size = new System.Drawing.Size(1009, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1009, 481);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1017, 510);
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(83, 9);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(107, 23);
            this.displayID.TabIndex = 4;
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(5, 9);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 3;
            this.labelID.Text = "ID";
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(5, 38);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.Size = new System.Drawing.Size(75, 23);
            this.labelIssueDate.TabIndex = 7;
            this.labelIssueDate.Text = "Issue Date";
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "issuedate", true));
            this.dateIssueDate.Location = new System.Drawing.Point(83, 38);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(130, 23);
            this.dateIssueDate.TabIndex = 8;
            // 
            // txtRequest
            // 
            this.txtRequest.BackColor = System.Drawing.Color.White;
            this.txtRequest.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "cutplanID", true));
            this.txtRequest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRequest.Location = new System.Drawing.Point(305, 9);
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.Size = new System.Drawing.Size(107, 23);
            this.txtRequest.TabIndex = 10;
            this.txtRequest.Validating += new System.ComponentModel.CancelEventHandler(this.TxtRequest_Validating);
            // 
            // labelRequest
            // 
            this.labelRequest.Location = new System.Drawing.Point(227, 9);
            this.labelRequest.Name = "labelRequest";
            this.labelRequest.Size = new System.Drawing.Size(75, 23);
            this.labelRequest.TabIndex = 9;
            this.labelRequest.Text = "Request";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(305, 38);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(217, 23);
            this.txtRemark.TabIndex = 14;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(227, 38);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 13;
            this.labelRemark.Text = "Remark";
            // 
            // labelNotApprove
            // 
            this.labelNotApprove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNotApprove.BackColor = System.Drawing.Color.Transparent;
            this.labelNotApprove.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "status", true));
            this.labelNotApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.labelNotApprove.Location = new System.Drawing.Point(866, 9);
            this.labelNotApprove.Name = "labelNotApprove";
            this.labelNotApprove.Size = new System.Drawing.Size(115, 23);
            this.labelNotApprove.TabIndex = 45;
            this.labelNotApprove.Text = "Not Approve";
            this.labelNotApprove.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // btnAutoPick
            // 
            this.btnAutoPick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoPick.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnAutoPick.Location = new System.Drawing.Point(866, 34);
            this.btnAutoPick.Name = "btnAutoPick";
            this.btnAutoPick.Size = new System.Drawing.Size(80, 30);
            this.btnAutoPick.TabIndex = 46;
            this.btnAutoPick.Text = "AutoPick";
            this.btnAutoPick.UseVisualStyleBackColor = true;
            this.btnAutoPick.Click += new System.EventHandler(this.BtnAutoPick_Click);
            // 
            // btnCutRefNo
            // 
            this.btnCutRefNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCutRefNo.Location = new System.Drawing.Point(753, 34);
            this.btnCutRefNo.Name = "btnCutRefNo";
            this.btnCutRefNo.Size = new System.Drawing.Size(93, 30);
            this.btnCutRefNo.TabIndex = 47;
            this.btnCutRefNo.Text = "Cutting Ref";
            this.btnCutRefNo.UseVisualStyleBackColor = true;
            this.btnCutRefNo.Click += new System.EventHandler(this.BtnCutRefNo_Click);
            // 
            // P62
            // 
            this.ApvChkValue = "New";
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1017, 543);
            this.DefaultDetailOrder = "poid,scirefno,colorid,sizespec";
            this.DefaultOrder = "issuedate,id";
            this.DefaultSubDetailOrder = "poid,seq1,seq2,dyelot,roll";
            this.GridAlias = "Issue_Summary";
            this.GridUniqueKey = "poid,scirefno,colorid";
            this.IsSupportClip = false;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "id";
            this.KeyField2 = "id";
            this.Name = "P62";
            this.OnLineHelpID = "Sci.Win.Tems.Input8";
            this.SubDetailKeyField1 = "id,Ukey";
            this.SubDetailKeyField2 = "id,Issue_SummaryUkey";
            this.SubGridAlias = "Issue_Detail";
            this.SubKeyField1 = "id";
            this.SubKeyField2 = "Ukey";
            this.Text = "P62. Issue Fabric for Cutting Tape";
            this.UnApvChkValue = "Confirmed";
            this.WorkAlias = "Issue";
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

        private Win.UI.TextBox txtRemark;
        private Win.UI.Label labelRemark;
        private Win.UI.TextBox txtRequest;
        private Win.UI.Label labelRequest;
        private Win.UI.Label labelIssueDate;
        private Win.UI.DateBox dateIssueDate;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label labelID;
        private Win.UI.Label labelNotApprove;
        private Win.UI.Button btnAutoPick;
        private Win.UI.Button btnCutRefNo;
    }
}