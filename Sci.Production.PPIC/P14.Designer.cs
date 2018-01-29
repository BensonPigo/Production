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
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detailcont
            // 
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
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(726, 351);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(734, 380);
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
            // P14
            // 
            this.ClientSize = new System.Drawing.Size(734, 413);
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
    }
}
