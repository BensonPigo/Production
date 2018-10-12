namespace Sci.Production.Thread
{
    partial class P04
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
            this.labelNo = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelDate = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.displayNo = new Sci.Win.UI.DisplayBox();
            this.dateDate = new Sci.Win.UI.DateBox();
            this.displayM = new Sci.Win.UI.DisplayBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.label7 = new Sci.Win.UI.Label();
            this.displayRequestID = new Sci.Win.UI.DisplayBox();
            this.RequestID = new Sci.Win.UI.Label();
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
            this.masterpanel.Controls.Add(this.displayRequestID);
            this.masterpanel.Controls.Add(this.RequestID);
            this.masterpanel.Controls.Add(this.label7);
            this.masterpanel.Controls.Add(this.txtRemark);
            this.masterpanel.Controls.Add(this.displayM);
            this.masterpanel.Controls.Add(this.displayNo);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.labelDate);
            this.masterpanel.Controls.Add(this.labelM);
            this.masterpanel.Controls.Add(this.labelNo);
            this.masterpanel.Controls.Add(this.dateDate);
            this.masterpanel.Size = new System.Drawing.Size(918, 104);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelM, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayM, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.label7, 0);
            this.masterpanel.Controls.SetChildIndex(this.RequestID, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayRequestID, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 104);
            this.detailpanel.Size = new System.Drawing.Size(918, 210);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(642, 66);
            this.gridicon.TabIndex = 2;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(827, 1);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(918, 210);
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
            this.detail.Size = new System.Drawing.Size(918, 352);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(918, 314);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 314);
            this.detailbtm.Size = new System.Drawing.Size(918, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(918, 352);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(926, 381);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelNo
            // 
            this.labelNo.Location = new System.Drawing.Point(37, 22);
            this.labelNo.Name = "labelNo";
            this.labelNo.Size = new System.Drawing.Size(75, 23);
            this.labelNo.TabIndex = 1;
            this.labelNo.Text = "No.";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(274, 22);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(75, 23);
            this.labelM.TabIndex = 2;
            this.labelM.Text = "M";
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(37, 71);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(75, 23);
            this.labelDate.TabIndex = 3;
            this.labelDate.Text = "Date";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(274, 71);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 4;
            this.labelRemark.Text = "Remark";
            // 
            // displayNo
            // 
            this.displayNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayNo.Location = new System.Drawing.Point(115, 22);
            this.displayNo.Name = "displayNo";
            this.displayNo.Size = new System.Drawing.Size(108, 23);
            this.displayNo.TabIndex = 33;
            // 
            // dateDate
            // 
            this.dateDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "cdate", true));
            this.dateDate.Location = new System.Drawing.Point(115, 71);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(130, 23);
            this.dateDate.TabIndex = 0;
            // 
            // displayM
            // 
            this.displayM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayM.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MDivisionid", true));
            this.displayM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayM.Location = new System.Drawing.Point(352, 22);
            this.displayM.Name = "displayM";
            this.displayM.Size = new System.Drawing.Size(66, 23);
            this.displayM.TabIndex = 36;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(352, 71);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(256, 23);
            this.txtRemark.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label7.Location = new System.Drawing.Point(620, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(130, 23);
            this.label7.TabIndex = 38;
            this.label7.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // displayRequestID
            // 
            this.displayRequestID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRequestID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "RequestID", true));
            this.displayRequestID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRequestID.Location = new System.Drawing.Point(513, 22);
            this.displayRequestID.Name = "displayRequestID";
            this.displayRequestID.Size = new System.Drawing.Size(95, 23);
            this.displayRequestID.TabIndex = 40;
            // 
            // RequestID
            // 
            this.RequestID.Location = new System.Drawing.Point(435, 22);
            this.RequestID.Name = "RequestID";
            this.RequestID.Size = new System.Drawing.Size(75, 23);
            this.RequestID.TabIndex = 39;
            this.RequestID.Text = "RequestID";
            // 
            // P04
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(926, 414);
            this.DefaultControl = "dateDate";
            this.DefaultControlForEdit = "txtRemark";
            this.DefaultOrder = "ID";
            this.GridAlias = "ThreadIssue_Detail";
            this.GridNew = 0;
            this.GridUniqueKey = "Refno,Threadcolorid,threadlocationid";
            this.IsSupportConfirm = true;
            this.IsSupportPrint = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P04";
            this.Text = "P04.Thread Issue";
            this.UnApvChkValue = "Confirmed";
            this.WorkAlias = "ThreadIssue";
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

        private Win.UI.Label labelNo;
        private Win.UI.TextBox txtRemark;
        private Win.UI.DisplayBox displayM;
        private Win.UI.DateBox dateDate;
        private Win.UI.DisplayBox displayNo;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelDate;
        private Win.UI.Label labelM;
        private Win.UI.Label label7;
        private Win.UI.DisplayBox displayRequestID;
        private Win.UI.Label RequestID;
    }
}
