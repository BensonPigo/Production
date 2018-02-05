namespace Sci.Production.PPIC
{
    partial class P14
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
            this.txtRequest = new Sci.Win.UI.TextBox();
            this.labelM = new Sci.Win.UI.Label();
            this.txtOrderID = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.dateNewKPI = new Sci.Win.UI.DateBox();
            this.dateOldKPI = new Sci.Win.UI.DateBox();
            this.label4 = new Sci.Win.UI.Label();
            this.editFtyRemark = new Sci.Win.UI.EditBox();
            this.labelStatus = new Sci.Win.UI.Label();
            this.labelKPILETA = new Sci.Win.UI.Label();
            this.dateConfirmedNewKPILETA = new Sci.Win.UI.DateBox();
            this.label5 = new Sci.Win.UI.Label();
            this.txtApproveName = new Sci.Production.Class.txttpeuser();
            this.txtConfirmName = new Sci.Production.Class.txttpeuser();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.editBoxTPERemark = new Sci.Win.UI.EditBox();
            this.label10 = new Sci.Win.UI.Label();
            this.txtApproveDate = new Sci.Win.UI.TextBox();
            this.txtConfirmDate = new Sci.Win.UI.TextBox();
            this.txtTPEEditName = new Sci.Win.UI.TextBox();
            this.label11 = new Sci.Win.UI.Label();
            this.txtTPEEditDate = new Sci.Win.UI.TextBox();
            this.btnmail = new Sci.Win.UI.Button();
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
            this.detail.Size = new System.Drawing.Size(726, 499);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.btnmail);
            this.detailcont.Controls.Add(this.txtTPEEditDate);
            this.detailcont.Controls.Add(this.label11);
            this.detailcont.Controls.Add(this.txtTPEEditName);
            this.detailcont.Controls.Add(this.txtConfirmDate);
            this.detailcont.Controls.Add(this.txtApproveDate);
            this.detailcont.Controls.Add(this.label10);
            this.detailcont.Controls.Add(this.editBoxTPERemark);
            this.detailcont.Controls.Add(this.label9);
            this.detailcont.Controls.Add(this.label8);
            this.detailcont.Controls.Add(this.label7);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.txtConfirmName);
            this.detailcont.Controls.Add(this.txtApproveName);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.dateConfirmedNewKPILETA);
            this.detailcont.Controls.Add(this.labelKPILETA);
            this.detailcont.Controls.Add(this.labelStatus);
            this.detailcont.Controls.Add(this.editFtyRemark);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.dateOldKPI);
            this.detailcont.Controls.Add(this.dateNewKPI);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.txtOrderID);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.txtRequest);
            this.detailcont.Controls.Add(this.labelM);
            this.detailcont.Size = new System.Drawing.Size(726, 461);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 461);
            this.detailbtm.Size = new System.Drawing.Size(726, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(726, 499);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(734, 528);
            // 
            // txtRequest
            // 
            this.txtRequest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtRequest.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtRequest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtRequest.IsSupportEditMode = false;
            this.txtRequest.Location = new System.Drawing.Point(169, 32);
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.ReadOnly = true;
            this.txtRequest.Size = new System.Drawing.Size(108, 23);
            this.txtRequest.TabIndex = 6;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(59, 32);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(107, 23);
            this.labelM.TabIndex = 4;
            this.labelM.Text = "Request ID";
            // 
            // txtOrderID
            // 
            this.txtOrderID.BackColor = System.Drawing.Color.White;
            this.txtOrderID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OrderID", true));
            this.txtOrderID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtOrderID.Location = new System.Drawing.Point(169, 62);
            this.txtOrderID.Name = "txtOrderID";
            this.txtOrderID.Size = new System.Drawing.Size(108, 23);
            this.txtOrderID.TabIndex = 8;
            this.txtOrderID.Validating += new System.ComponentModel.CancelEventHandler(this.TxtOrderID_Validating);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(59, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "OrderID";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(59, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "Old KPILETA";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(366, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 23);
            this.label3.TabIndex = 11;
            this.label3.Text = "New KPILETA";
            // 
            // dateNewKPI
            // 
            this.dateNewKPI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "NewKPILETA", true));
            this.dateNewKPI.Location = new System.Drawing.Point(476, 91);
            this.dateNewKPI.Name = "dateNewKPI";
            this.dateNewKPI.Size = new System.Drawing.Size(104, 23);
            this.dateNewKPI.TabIndex = 12;
            // 
            // dateOldKPI
            // 
            this.dateOldKPI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OldKPILETA", true));
            this.dateOldKPI.IsSupportEditMode = false;
            this.dateOldKPI.Location = new System.Drawing.Point(169, 91);
            this.dateOldKPI.Name = "dateOldKPI";
            this.dateOldKPI.ReadOnly = true;
            this.dateOldKPI.Size = new System.Drawing.Size(104, 23);
            this.dateOldKPI.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(59, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 23);
            this.label4.TabIndex = 14;
            this.label4.Text = "Factory Remark";
            // 
            // editFtyRemark
            // 
            this.editFtyRemark.BackColor = System.Drawing.Color.White;
            this.editFtyRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FactoryRemark", true));
            this.editFtyRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editFtyRemark.Location = new System.Drawing.Point(169, 120);
            this.editFtyRemark.Multiline = true;
            this.editFtyRemark.Name = "editFtyRemark";
            this.editFtyRemark.Size = new System.Drawing.Size(411, 79);
            this.editFtyRemark.TabIndex = 15;
            // 
            // labelStatus
            // 
            this.labelStatus.BackColor = System.Drawing.Color.Transparent;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.labelStatus.Location = new System.Drawing.Point(377, 18);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(203, 54);
            this.labelStatus.TabIndex = 56;
            this.labelStatus.Text = "status";
            this.labelStatus.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelStatus.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.labelStatus.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // labelKPILETA
            // 
            this.labelKPILETA.Location = new System.Drawing.Point(59, 211);
            this.labelKPILETA.Name = "labelKPILETA";
            this.labelKPILETA.Size = new System.Drawing.Size(160, 23);
            this.labelKPILETA.TabIndex = 57;
            this.labelKPILETA.Text = "Confirmed New KPILETA";
            // 
            // dateConfirmedNewKPILETA
            // 
            this.dateConfirmedNewKPILETA.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ConfirmedNewKPILETA", true));
            this.dateConfirmedNewKPILETA.IsSupportEditMode = false;
            this.dateConfirmedNewKPILETA.Location = new System.Drawing.Point(222, 211);
            this.dateConfirmedNewKPILETA.Name = "dateConfirmedNewKPILETA";
            this.dateConfirmedNewKPILETA.ReadOnly = true;
            this.dateConfirmedNewKPILETA.Size = new System.Drawing.Size(104, 23);
            this.dateConfirmedNewKPILETA.TabIndex = 58;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(59, 240);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 23);
            this.label5.TabIndex = 59;
            this.label5.Text = "Approve Name";
            // 
            // txtApproveName
            // 
            this.txtApproveName.DataBindings.Add(new System.Windows.Forms.Binding("DisplayBox1Binding", this.mtbs, "ApproveName", true));
            this.txtApproveName.DisplayBox1Binding = "";
            this.txtApproveName.DisplayBox2Binding = "";
            this.txtApproveName.Location = new System.Drawing.Point(169, 240);
            this.txtApproveName.Name = "txtApproveName";
            this.txtApproveName.Size = new System.Drawing.Size(302, 23);
            this.txtApproveName.TabIndex = 60;
            // 
            // txtConfirmName
            // 
            this.txtConfirmName.DataBindings.Add(new System.Windows.Forms.Binding("DisplayBox1Binding", this.mtbs, "ConfirmName", true));
            this.txtConfirmName.DisplayBox1Binding = "";
            this.txtConfirmName.DisplayBox2Binding = "";
            this.txtConfirmName.Location = new System.Drawing.Point(169, 269);
            this.txtConfirmName.Name = "txtConfirmName";
            this.txtConfirmName.Size = new System.Drawing.Size(302, 23);
            this.txtConfirmName.TabIndex = 61;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(59, 269);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 23);
            this.label6.TabIndex = 62;
            this.label6.Text = "Confirm Name";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(476, 240);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 23);
            this.label7.TabIndex = 63;
            this.label7.Text = "Approve Date";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(476, 269);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 23);
            this.label8.TabIndex = 64;
            this.label8.Text = "ConfirmDate";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(59, 298);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(107, 23);
            this.label9.TabIndex = 65;
            this.label9.Text = "Taipei Remark";
            // 
            // editBoxTPERemark
            // 
            this.editBoxTPERemark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editBoxTPERemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "TPERemark", true));
            this.editBoxTPERemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editBoxTPERemark.IsSupportEditMode = false;
            this.editBoxTPERemark.Location = new System.Drawing.Point(169, 298);
            this.editBoxTPERemark.Multiline = true;
            this.editBoxTPERemark.Name = "editBoxTPERemark";
            this.editBoxTPERemark.ReadOnly = true;
            this.editBoxTPERemark.Size = new System.Drawing.Size(411, 79);
            this.editBoxTPERemark.TabIndex = 66;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(59, 383);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(113, 23);
            this.label10.TabIndex = 67;
            this.label10.Text = "Taipei Edit Name";
            // 
            // txtApproveDate
            // 
            this.txtApproveDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtApproveDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtApproveDate.IsSupportEditMode = false;
            this.txtApproveDate.Location = new System.Drawing.Point(586, 240);
            this.txtApproveDate.Name = "txtApproveDate";
            this.txtApproveDate.ReadOnly = true;
            this.txtApproveDate.Size = new System.Drawing.Size(137, 23);
            this.txtApproveDate.TabIndex = 68;
            this.txtApproveDate.ValidatingType = typeof(System.DateTime);
            // 
            // txtConfirmDate
            // 
            this.txtConfirmDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtConfirmDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtConfirmDate.IsSupportEditMode = false;
            this.txtConfirmDate.Location = new System.Drawing.Point(586, 269);
            this.txtConfirmDate.Name = "txtConfirmDate";
            this.txtConfirmDate.ReadOnly = true;
            this.txtConfirmDate.Size = new System.Drawing.Size(137, 23);
            this.txtConfirmDate.TabIndex = 69;
            this.txtConfirmDate.ValidatingType = typeof(System.DateTime);
            // 
            // txtTPEEditName
            // 
            this.txtTPEEditName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtTPEEditName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "TPEEditName", true));
            this.txtTPEEditName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtTPEEditName.IsSupportEditMode = false;
            this.txtTPEEditName.Location = new System.Drawing.Point(175, 383);
            this.txtTPEEditName.Name = "txtTPEEditName";
            this.txtTPEEditName.ReadOnly = true;
            this.txtTPEEditName.Size = new System.Drawing.Size(137, 23);
            this.txtTPEEditName.TabIndex = 70;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(315, 383);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(106, 23);
            this.label11.TabIndex = 71;
            this.label11.Text = "Taipei Edit Date";
            // 
            // txtTPEEditDate
            // 
            this.txtTPEEditDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtTPEEditDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtTPEEditDate.IsSupportEditMode = false;
            this.txtTPEEditDate.Location = new System.Drawing.Point(424, 383);
            this.txtTPEEditDate.Name = "txtTPEEditDate";
            this.txtTPEEditDate.ReadOnly = true;
            this.txtTPEEditDate.Size = new System.Drawing.Size(137, 23);
            this.txtTPEEditDate.TabIndex = 72;
            this.txtTPEEditDate.ValidatingType = typeof(System.DateTime);
            // 
            // btnmail
            // 
            this.btnmail.Location = new System.Drawing.Point(583, 28);
            this.btnmail.Name = "btnmail";
            this.btnmail.Size = new System.Drawing.Size(80, 30);
            this.btnmail.TabIndex = 73;
            this.btnmail.Text = "Send Mail";
            this.btnmail.UseVisualStyleBackColor = true;
            this.btnmail.Click += new System.EventHandler(this.Btnmail_Click);
            // 
            // P14
            // 
            this.ClientSize = new System.Drawing.Size(734, 561);
            this.DefaultOrder = "ID";
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.IsSupportSend = true;
            this.Name = "P14";
            this.RecallChkValue = "Sent";
            this.SendChkValue = "New";
            this.Text = "Change KPILETA Request";
            this.UniqueExpress = "id";
            this.WorkAlias = "ChangeKPILETARequest";
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

        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtOrderID;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtRequest;
        private Win.UI.Label labelM;
        private Win.UI.Label label4;
        private Win.UI.DateBox dateOldKPI;
        private Win.UI.DateBox dateNewKPI;
        private Win.UI.EditBox editFtyRemark;
        private Win.UI.Label labelStatus;
        private Win.UI.Label label10;
        private Win.UI.EditBox editBoxTPERemark;
        private Win.UI.Label label9;
        private Win.UI.Label label8;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Class.txttpeuser txtConfirmName;
        private Class.txttpeuser txtApproveName;
        private Win.UI.Label label5;
        private Win.UI.DateBox dateConfirmedNewKPILETA;
        private Win.UI.Label labelKPILETA;
        private Win.UI.TextBox txtTPEEditDate;
        private Win.UI.Label label11;
        private Win.UI.TextBox txtTPEEditName;
        private Win.UI.TextBox txtConfirmDate;
        private Win.UI.TextBox txtApproveDate;
        private Win.UI.Button btnmail;
    }
}
