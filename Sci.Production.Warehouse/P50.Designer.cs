namespace Sci.Production.Warehouse
{
    partial class P50
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
            this.labelID = new Sci.Win.UI.Label();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.label25 = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
            this.btngenerate = new Sci.Win.UI.Button();
            this.labelRemark = new Sci.Win.UI.Label();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.displayAdjustID = new Sci.Win.UI.DisplayBox();
            this.labelAdjustID = new Sci.Win.UI.Label();
            this.labelStockType = new Sci.Win.UI.Label();
            this.comboStockType = new Sci.Win.UI.ComboBox();
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
            this.masterpanel.Controls.Add(this.comboStockType);
            this.masterpanel.Controls.Add(this.labelStockType);
            this.masterpanel.Controls.Add(this.displayAdjustID);
            this.masterpanel.Controls.Add(this.labelAdjustID);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.btngenerate);
            this.masterpanel.Controls.Add(this.dateIssueDate);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.label25);
            this.masterpanel.Controls.Add(this.labelIssueDate);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Size = new System.Drawing.Size(978, 139);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.label25, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.btngenerate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelAdjustID, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayAdjustID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelStockType, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboStockType, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 139);
            this.detailpanel.Size = new System.Drawing.Size(978, 338);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.gridicon.Location = new System.Drawing.Point(848, 97);
            this.gridicon.TabIndex = 4;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(868, 1);
            this.refresh.TabIndex = 0;
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(978, 338);
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
            this.detail.Size = new System.Drawing.Size(978, 515);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(978, 477);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 477);
            this.detailbtm.Size = new System.Drawing.Size(978, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(978, 515);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(986, 544);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(466, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(418, 13);
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(16, 13);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 1;
            this.labelID.Text = "ID";
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(241, 13);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.Size = new System.Drawing.Size(108, 23);
            this.labelIssueDate.TabIndex = 11;
            this.labelIssueDate.Text = "Issue Date";
            // 
            // label25
            // 
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label25.Location = new System.Drawing.Point(833, 13);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(115, 23);
            this.label25.TabIndex = 43;
            this.label25.Text = "Not Approve";
            this.label25.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(98, 13);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(120, 23);
            this.displayID.TabIndex = 0;
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IssueDate", true));
            this.dateIssueDate.Location = new System.Drawing.Point(352, 13);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(130, 23);
            this.dateIssueDate.TabIndex = 0;
            // 
            // btngenerate
            // 
            this.btngenerate.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btngenerate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btngenerate.Location = new System.Drawing.Point(859, 42);
            this.btngenerate.Name = "btngenerate";
            this.btngenerate.Size = new System.Drawing.Size(89, 31);
            this.btngenerate.TabIndex = 3;
            this.btngenerate.Text = "generate";
            this.btngenerate.UseVisualStyleBackColor = true;
            this.btngenerate.Click += new System.EventHandler(this.Btngenerate_Click);
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(16, 78);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 59;
            this.labelRemark.Text = "Remark";
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(97, 78);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(658, 51);
            this.editRemark.TabIndex = 2;
            // 
            // displayAdjustID
            // 
            this.displayAdjustID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayAdjustID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "adjustid", true));
            this.displayAdjustID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayAdjustID.Location = new System.Drawing.Point(597, 13);
            this.displayAdjustID.Name = "displayAdjustID";
            this.displayAdjustID.Size = new System.Drawing.Size(120, 23);
            this.displayAdjustID.TabIndex = 63;
            // 
            // labelAdjustID
            // 
            this.labelAdjustID.Location = new System.Drawing.Point(515, 13);
            this.labelAdjustID.Name = "labelAdjustID";
            this.labelAdjustID.Size = new System.Drawing.Size(75, 23);
            this.labelAdjustID.TabIndex = 64;
            this.labelAdjustID.Text = "Adjust ID";
            // 
            // labelStockType
            // 
            this.labelStockType.Location = new System.Drawing.Point(515, 47);
            this.labelStockType.Name = "labelStockType";
            this.labelStockType.Size = new System.Drawing.Size(75, 23);
            this.labelStockType.TabIndex = 65;
            this.labelStockType.Text = "Stock Type";
            // 
            // comboStockType
            // 
            this.comboStockType.BackColor = System.Drawing.Color.White;
            this.comboStockType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "StockType", true));
            this.comboStockType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStockType.FormattingEnabled = true;
            this.comboStockType.IsSupportUnselect = true;
            this.comboStockType.Location = new System.Drawing.Point(597, 46);
            this.comboStockType.Name = "comboStockType";
            this.comboStockType.Size = new System.Drawing.Size(121, 24);
            this.comboStockType.TabIndex = 1;
            this.comboStockType.Validating += new System.ComponentModel.CancelEventHandler(this.ComboStockType_Validating);
            // 
            // P50
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(986, 577);
            this.DefaultControl = "dateIssueDate";
            this.DefaultControlForEdit = "dateIssueDate";
            this.DefaultOrder = "issuedate,ID";
            this.Grid2New = 0;
            this.GridAlias = "StockTaking_detail";
            this.GridNew = 0;
            this.GridUniqueKey = "mdivisionid,poid,seq1,seq2,roll,Dyelot";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.KeyField1 = "ID";
            this.Name = "P50";
            this.Text = "P50. Warehouse Forward Stocktaking";
            this.UniqueExpress = "id";
            this.WorkAlias = "StockTaking";
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

        private Win.UI.Label labelIssueDate;
        private Win.UI.Label labelID;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label label25;
        private Win.UI.Button btngenerate;
        private Win.UI.DateBox dateIssueDate;
        private Win.UI.EditBox editRemark;
        private Win.UI.Label labelRemark;
        private Win.UI.ComboBox comboStockType;
        private Win.UI.Label labelStockType;
        private Win.UI.DisplayBox displayAdjustID;
        private Win.UI.Label labelAdjustID;
    }
}
