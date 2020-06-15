namespace Sci.Production.Cutting
{
    partial class P08
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
            this.labelCutplanID = new Sci.Win.UI.Label();
            this.labelCuttingDate = new Sci.Win.UI.Label();
            this.labelCuttingSPNo = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.displayCuttingSPNo = new Sci.Win.UI.DisplayBox();
            this.dateCuttingDate = new Sci.Win.UI.DateBox();
            this.btnSendMail = new Sci.Win.UI.Button();
            this.label1 = new Sci.Win.UI.Label();
            this.displayfactory = new Sci.Win.UI.DisplayBox();
            this.btnSetDefaultMail = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.btnSetDefaultMail);
            this.masterpanel.Controls.Add(this.displayfactory);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Controls.Add(this.btnSendMail);
            this.masterpanel.Controls.Add(this.dateCuttingDate);
            this.masterpanel.Controls.Add(this.displayCuttingSPNo);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.labelCuttingSPNo);
            this.masterpanel.Controls.Add(this.labelCuttingDate);
            this.masterpanel.Controls.Add(this.labelCutplanID);
            this.masterpanel.Size = new System.Drawing.Size(1000, 77);
            this.masterpanel.Controls.SetChildIndex(this.labelCutplanID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCuttingDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCuttingSPNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayCuttingSPNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateCuttingDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnSendMail, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayfactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnSetDefaultMail, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 77);
            this.detailpanel.Size = new System.Drawing.Size(1000, 341);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(828, 42);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(827, 1);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1000, 341);
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
            this.detail.Size = new System.Drawing.Size(1000, 456);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1000, 418);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 418);
            this.detailbtm.Size = new System.Drawing.Size(1000, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1000, 456);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1008, 485);
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
            // labelCutplanID
            // 
            this.labelCutplanID.Location = new System.Drawing.Point(18, 13);
            this.labelCutplanID.Name = "labelCutplanID";
            this.labelCutplanID.Size = new System.Drawing.Size(112, 23);
            this.labelCutplanID.TabIndex = 1;
            this.labelCutplanID.Text = "Cut tape plan ID";
            // 
            // labelCuttingDate
            // 
            this.labelCuttingDate.Location = new System.Drawing.Point(18, 42);
            this.labelCuttingDate.Name = "labelCuttingDate";
            this.labelCuttingDate.Size = new System.Drawing.Size(112, 23);
            this.labelCuttingDate.TabIndex = 3;
            this.labelCuttingDate.Text = "Est. Cutting Date";
            // 
            // labelCuttingSPNo
            // 
            this.labelCuttingSPNo.Location = new System.Drawing.Point(274, 13);
            this.labelCuttingSPNo.Name = "labelCuttingSPNo";
            this.labelCuttingSPNo.Size = new System.Drawing.Size(91, 23);
            this.labelCuttingSPNo.TabIndex = 9;
            this.labelCuttingSPNo.Text = "Cutting SP#";
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(133, 13);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(108, 23);
            this.displayID.TabIndex = 11;
            // 
            // displayCuttingSPNo
            // 
            this.displayCuttingSPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCuttingSPNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "cuttingid", true));
            this.displayCuttingSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCuttingSPNo.Location = new System.Drawing.Point(368, 13);
            this.displayCuttingSPNo.Name = "displayCuttingSPNo";
            this.displayCuttingSPNo.Size = new System.Drawing.Size(108, 23);
            this.displayCuttingSPNo.TabIndex = 13;
            // 
            // dateCuttingDate
            // 
            this.dateCuttingDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "estcutdate", true));
            this.dateCuttingDate.Location = new System.Drawing.Point(133, 42);
            this.dateCuttingDate.Name = "dateCuttingDate";
            this.dateCuttingDate.Size = new System.Drawing.Size(130, 23);
            this.dateCuttingDate.TabIndex = 15;
            // 
            // btnSendMail
            // 
            this.btnSendMail.Location = new System.Drawing.Point(681, 9);
            this.btnSendMail.Name = "btnSendMail";
            this.btnSendMail.Size = new System.Drawing.Size(95, 30);
            this.btnSendMail.TabIndex = 20;
            this.btnSendMail.Text = "Send Mail";
            this.btnSendMail.UseVisualStyleBackColor = true;
            this.btnSendMail.Click += new System.EventHandler(this.BtnSendMail_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(274, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 23);
            this.label1.TabIndex = 21;
            this.label1.Text = "Factory";
            // 
            // displayfactory
            // 
            this.displayfactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayfactory.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FactoryID", true));
            this.displayfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayfactory.Location = new System.Drawing.Point(368, 42);
            this.displayfactory.Name = "displayfactory";
            this.displayfactory.Size = new System.Drawing.Size(75, 23);
            this.displayfactory.TabIndex = 22;
            // 
            // btnSetDefaultMail
            // 
            this.btnSetDefaultMail.Location = new System.Drawing.Point(782, 9);
            this.btnSetDefaultMail.Name = "btnSetDefaultMail";
            this.btnSetDefaultMail.Size = new System.Drawing.Size(146, 30);
            this.btnSetDefaultMail.TabIndex = 23;
            this.btnSetDefaultMail.Text = "Set Default Mail";
            this.btnSetDefaultMail.UseVisualStyleBackColor = true;
            this.btnSetDefaultMail.Click += new System.EventHandler(this.BtnSetDefaultMail_Click);
            // 
            // P08
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1008, 518);
            this.DefaultOrder = "id";
            this.ExpressQuery = true;
            this.GridAlias = "CutTapePlan_Detail";
            this.GridNew = 0;
            this.GridUniqueKey = "FabricCombo,MarkerName,ColorID,Dyelot";
            this.IsSupportClip = false;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "id";
            this.Name = "P08";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P08. Cutting Tape Plan";
            this.UnApvChkValue = "Confirmed";
            this.WorkAlias = "CutTapePlan";
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
        private Win.UI.Label labelCuttingDate;
        private Win.UI.Label labelCutplanID;
        private Win.UI.Button btnSendMail;
        private Win.UI.DateBox dateCuttingDate;
        private Win.UI.DisplayBox displayCuttingSPNo;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label labelCuttingSPNo;
        private Win.UI.DisplayBox displayfactory;
        private Win.UI.Label label1;
        private Win.UI.Button btnSetDefaultMail;
    }
}
