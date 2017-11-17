namespace Sci.Production.Shipping
{
    partial class P06
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
            this.displayNo = new Sci.Win.UI.DisplayBox();
            this.displayM = new Sci.Win.UI.DisplayBox();
            this.labelPulloutdate = new Sci.Win.UI.Label();
            this.datePulloutdate = new Sci.Win.UI.DateBox();
            this.dateSendtoSCI = new Sci.Win.UI.DateBox();
            this.btnRevise = new Sci.Win.UI.Button();
            this.label6 = new Sci.Win.UI.Label();
            this.btnHistory = new Sci.Win.UI.Button();
            this.btnRevisedHistory = new Sci.Win.UI.Button();
            this.labelSendtoSCI = new Sci.Win.UI.Label();
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
            this.masterpanel.Controls.Add(this.btnHistory);
            this.masterpanel.Controls.Add(this.label6);
            this.masterpanel.Controls.Add(this.btnRevise);
            this.masterpanel.Controls.Add(this.labelSendtoSCI);
            this.masterpanel.Controls.Add(this.labelPulloutdate);
            this.masterpanel.Controls.Add(this.displayM);
            this.masterpanel.Controls.Add(this.displayNo);
            this.masterpanel.Controls.Add(this.labelM);
            this.masterpanel.Controls.Add(this.labelNo);
            this.masterpanel.Controls.Add(this.dateSendtoSCI);
            this.masterpanel.Controls.Add(this.datePulloutdate);
            this.masterpanel.Size = new System.Drawing.Size(1000, 71);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.datePulloutdate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateSendtoSCI, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelM, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayM, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPulloutdate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSendtoSCI, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnRevise, 0);
            this.masterpanel.Controls.SetChildIndex(this.label6, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnHistory, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 71);
            this.detailpanel.Size = new System.Drawing.Size(1000, 439);
            // 
            // gridicon
            // 
            this.gridicon.Enabled = false;
            this.gridicon.Location = new System.Drawing.Point(791, 36);
            this.gridicon.Visible = false;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(778, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1000, 439);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(1000, 548);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1000, 510);
            // 
            // detailbtm
            // 
            this.detailbtm.Controls.Add(this.btnRevisedHistory);
            this.detailbtm.Location = new System.Drawing.Point(0, 510);
            this.detailbtm.Size = new System.Drawing.Size(1000, 38);
            this.detailbtm.Controls.SetChildIndex(this.lbleditby, 0);
            this.detailbtm.Controls.SetChildIndex(this.lblcreateby, 0);
            this.detailbtm.Controls.SetChildIndex(this.editby, 0);
            this.detailbtm.Controls.SetChildIndex(this.createby, 0);
            this.detailbtm.Controls.SetChildIndex(this.refresh, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnRevisedHistory, 0);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1000, 548);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1008, 577);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(327, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(448, 7);
            this.editby.Size = new System.Drawing.Size(324, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(400, 13);
            // 
            // labelNo
            // 
            this.labelNo.Location = new System.Drawing.Point(5, 5);
            this.labelNo.Name = "labelNo";
            this.labelNo.Size = new System.Drawing.Size(28, 23);
            this.labelNo.TabIndex = 1;
            this.labelNo.Text = "No.";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(5, 32);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(28, 23);
            this.labelM.TabIndex = 2;
            this.labelM.Text = "M";
            // 
            // displayNo
            // 
            this.displayNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayNo.Location = new System.Drawing.Point(37, 5);
            this.displayNo.Name = "displayNo";
            this.displayNo.Size = new System.Drawing.Size(130, 23);
            this.displayNo.TabIndex = 3;
            // 
            // displayM
            // 
            this.displayM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayM.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MDivisionID", true));
            this.displayM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayM.Location = new System.Drawing.Point(37, 32);
            this.displayM.Name = "displayM";
            this.displayM.Size = new System.Drawing.Size(68, 23);
            this.displayM.TabIndex = 4;
            // 
            // labelPulloutdate
            // 
            this.labelPulloutdate.Location = new System.Drawing.Point(196, 5);
            this.labelPulloutdate.Name = "labelPulloutdate";
            this.labelPulloutdate.Size = new System.Drawing.Size(83, 23);
            this.labelPulloutdate.TabIndex = 5;
            this.labelPulloutdate.Text = "Pull-out date";
            // 
            // datePulloutdate
            // 
            this.datePulloutdate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PulloutDate", true));
            this.datePulloutdate.IsSupportEditMode = false;
            this.datePulloutdate.Location = new System.Drawing.Point(283, 5);
            this.datePulloutdate.Name = "datePulloutdate";
            this.datePulloutdate.ReadOnly = true;
            this.datePulloutdate.Size = new System.Drawing.Size(100, 23);
            this.datePulloutdate.TabIndex = 7;
            // 
            // dateSendtoSCI
            // 
            this.dateSendtoSCI.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SendToTPE", true));
            this.dateSendtoSCI.IsSupportEditMode = false;
            this.dateSendtoSCI.Location = new System.Drawing.Point(506, 5);
            this.dateSendtoSCI.Name = "dateSendtoSCI";
            this.dateSendtoSCI.ReadOnly = true;
            this.dateSendtoSCI.Size = new System.Drawing.Size(100, 23);
            this.dateSendtoSCI.TabIndex = 10;
            // 
            // btnRevise
            // 
            this.btnRevise.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnRevise.Location = new System.Drawing.Point(612, 5);
            this.btnRevise.Name = "btnRevise";
            this.btnRevise.Size = new System.Drawing.Size(173, 63);
            this.btnRevise.TabIndex = 11;
            this.btnRevise.Text = "Revise from ship plan and FOC/LO packing list";
            this.btnRevise.UseVisualStyleBackColor = true;
            this.btnRevise.Click += new System.EventHandler(this.BtnRevise_Click);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(791, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 28);
            this.label6.TabIndex = 12;
            this.label6.Text = "Lock";
            this.label6.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.label6.TextStyle.Color = System.Drawing.Color.Red;
            this.label6.TextStyle.ExtBorderColor = System.Drawing.Color.Red;
            this.label6.TextStyle.GradientColor = System.Drawing.Color.Red;
            // 
            // btnHistory
            // 
            this.btnHistory.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnHistory.Location = new System.Drawing.Point(852, 5);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(80, 30);
            this.btnHistory.TabIndex = 13;
            this.btnHistory.Text = "History";
            this.btnHistory.UseVisualStyleBackColor = true;
            this.btnHistory.Click += new System.EventHandler(this.BtnHistory_Click);
            // 
            // btnRevisedHistory
            // 
            this.btnRevisedHistory.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnRevisedHistory.Location = new System.Drawing.Point(864, 2);
            this.btnRevisedHistory.Name = "btnRevisedHistory";
            this.btnRevisedHistory.Size = new System.Drawing.Size(130, 30);
            this.btnRevisedHistory.TabIndex = 3;
            this.btnRevisedHistory.Text = "Revised History";
            this.btnRevisedHistory.UseVisualStyleBackColor = true;
            this.btnRevisedHistory.Click += new System.EventHandler(this.BtnRevisedHistory_Click);
            // 
            // labelSendtoSCI
            // 
            this.labelSendtoSCI.Location = new System.Drawing.Point(422, 5);
            this.labelSendtoSCI.Name = "labelSendtoSCI";
            this.labelSendtoSCI.Size = new System.Drawing.Size(80, 23);
            this.labelSendtoSCI.TabIndex = 9;
            this.labelSendtoSCI.Text = "Send to SCI";
            // 
            // P06
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1008, 610);
            this.DefaultDetailOrder = "OrderID";
            this.DefaultOrder = "ID";
            this.GridAlias = "Pullout_Detail";
            this.GridNew = 0;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P06";
            this.SubDetailKeyField1 = "UKey";
            this.SubDetailKeyField2 = "Pullout_DetailUKey";
            this.SubGridAlias = "Pullout_Detail_Detail";
            this.SubKeyField1 = "UKey";
            this.Text = "P06. Pullout Report";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "ID";
            this.WorkAlias = "Pullout";
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

        private Win.UI.Button btnHistory;
        private Win.UI.Label label6;
        private Win.UI.Button btnRevise;
        private Win.UI.DateBox dateSendtoSCI;
        private Win.UI.Label labelSendtoSCI;
        private Win.UI.DateBox datePulloutdate;
        private Win.UI.Label labelPulloutdate;
        private Win.UI.DisplayBox displayM;
        private Win.UI.DisplayBox displayNo;
        private Win.UI.Label labelM;
        private Win.UI.Label labelNo;
        private Win.UI.Button btnRevisedHistory;
    }
}
