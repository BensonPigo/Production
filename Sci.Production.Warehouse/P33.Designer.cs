namespace Sci.Production.Warehouse
{
    partial class P33
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
            this.components = new System.ComponentModel.Container();
            this.labelID = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
            this.displayLineNo = new Sci.Win.UI.DisplayBox();
            this.labelLineNo = new Sci.Win.UI.Label();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.labelRemark = new Sci.Win.UI.Label();
            this.btnAutoPick = new Sci.Win.UI.Button();
            this.btnBreakDown = new Sci.Win.UI.Button();
            this.displayPOID = new Sci.Win.UI.DisplayBox();
            this.labelPOID = new Sci.Win.UI.Label();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.txtOrderID = new Sci.Win.UI.TextBox();
            this.labelOrderID = new Sci.Win.UI.Label();
            this.gridIssueBreakDown = new Sci.Win.UI.Grid();
            this.gridIssueBreakDownBS = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.checkByCombo = new Sci.Win.UI.CheckBox();
            this.labelConfirmed = new Sci.Win.UI.Label();
            this.displayStyle = new Sci.Win.UI.DisplayBox();
            this.lblStyle = new Sci.Win.UI.Label();
            this.btnCallP99 = new Sci.Win.UI.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridIssueBreakDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridIssueBreakDownBS)).BeginInit();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.displayStyle);
            this.masterpanel.Controls.Add(this.lblStyle);
            this.masterpanel.Controls.Add(this.labelConfirmed);
            this.masterpanel.Controls.Add(this.checkByCombo);
            this.masterpanel.Controls.Add(this.txtOrderID);
            this.masterpanel.Controls.Add(this.labelOrderID);
            this.masterpanel.Controls.Add(this.displayPOID);
            this.masterpanel.Controls.Add(this.labelPOID);
            this.masterpanel.Controls.Add(this.btnBreakDown);
            this.masterpanel.Controls.Add(this.btnAutoPick);
            this.masterpanel.Controls.Add(this.txtRemark);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.displayLineNo);
            this.masterpanel.Controls.Add(this.labelLineNo);
            this.masterpanel.Controls.Add(this.labelIssueDate);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Controls.Add(this.dateIssueDate);
            this.masterpanel.Size = new System.Drawing.Size(999, 131);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelLineNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayLineNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnAutoPick, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnBreakDown, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPOID, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayPOID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelOrderID, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtOrderID, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkByCombo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelConfirmed, 0);
            this.masterpanel.Controls.SetChildIndex(this.lblStyle, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayStyle, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 131);
            this.detailpanel.Size = new System.Drawing.Size(999, 248);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gridicon.Location = new System.Drawing.Point(3388, 111);
            // 
            // refresh
            // 
            this.refresh.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.refresh.Location = new System.Drawing.Point(787, 136);
            this.refresh.Size = new System.Drawing.Size(80, 36);
            this.refresh.TabIndex = 10;
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(999, 248);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(999, 553);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(999, 379);
            // 
            // detailbtm
            // 
            this.detailbtm.Controls.Add(this.btnCallP99);
            this.detailbtm.Controls.Add(this.gridIssueBreakDown);
            this.detailbtm.Location = new System.Drawing.Point(0, 379);
            this.detailbtm.Size = new System.Drawing.Size(999, 174);
            this.detailbtm.Controls.SetChildIndex(this.lbleditby, 0);
            this.detailbtm.Controls.SetChildIndex(this.lblcreateby, 0);
            this.detailbtm.Controls.SetChildIndex(this.editby, 0);
            this.detailbtm.Controls.SetChildIndex(this.createby, 0);
            this.detailbtm.Controls.SetChildIndex(this.refresh, 0);
            this.detailbtm.Controls.SetChildIndex(this.gridIssueBreakDown, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnCallP99, 0);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(999, 553);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1007, 582);
            // 
            // createby
            // 
            this.createby.Location = new System.Drawing.Point(70, 143);
            this.createby.Size = new System.Drawing.Size(322, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(445, 143);
            this.editby.Size = new System.Drawing.Size(336, 23);
            // 
            // lblcreateby
            // 
            this.lblcreateby.Location = new System.Drawing.Point(5, 149);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(397, 149);
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(9, 9);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 1;
            this.labelID.Text = "ID";
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(87, 9);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(107, 23);
            this.displayID.TabIndex = 1;
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(224, 9);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.Size = new System.Drawing.Size(75, 23);
            this.labelIssueDate.TabIndex = 5;
            this.labelIssueDate.Text = "Issue Date";
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IssueDate", true));
            this.dateIssueDate.Location = new System.Drawing.Point(302, 9);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(130, 23);
            this.dateIssueDate.TabIndex = 5;
            // 
            // displayLineNo
            // 
            this.displayLineNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayLineNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayLineNo.Location = new System.Drawing.Point(87, 70);
            this.displayLineNo.Name = "displayLineNo";
            this.displayLineNo.Size = new System.Drawing.Size(211, 23);
            this.displayLineNo.TabIndex = 3;
            // 
            // labelLineNo
            // 
            this.labelLineNo.Location = new System.Drawing.Point(9, 70);
            this.labelLineNo.Name = "labelLineNo";
            this.labelLineNo.Size = new System.Drawing.Size(75, 23);
            this.labelLineNo.TabIndex = 7;
            this.labelLineNo.Text = "Line#";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(87, 99);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(549, 23);
            this.txtRemark.TabIndex = 4;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(9, 99);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 11;
            this.labelRemark.Text = "Remark";
            // 
            // btnAutoPick
            // 
            this.btnAutoPick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoPick.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnAutoPick.Location = new System.Drawing.Point(911, 31);
            this.btnAutoPick.Name = "btnAutoPick";
            this.btnAutoPick.Size = new System.Drawing.Size(80, 30);
            this.btnAutoPick.TabIndex = 8;
            this.btnAutoPick.Text = "AutoPick";
            this.btnAutoPick.UseVisualStyleBackColor = true;
            this.btnAutoPick.Click += new System.EventHandler(this.BtnAutoPick_Click);
            // 
            // btnBreakDown
            // 
            this.btnBreakDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBreakDown.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnBreakDown.Enabled = false;
            this.btnBreakDown.Location = new System.Drawing.Point(876, 63);
            this.btnBreakDown.Name = "btnBreakDown";
            this.btnBreakDown.Size = new System.Drawing.Size(120, 30);
            this.btnBreakDown.TabIndex = 9;
            this.btnBreakDown.Text = "Issue B\'down";
            this.btnBreakDown.UseVisualStyleBackColor = true;
            this.btnBreakDown.Click += new System.EventHandler(this.BtnBreakDown_Click);
            // 
            // displayPOID
            // 
            this.displayPOID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPOID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPOID.Location = new System.Drawing.Point(301, 41);
            this.displayPOID.Name = "displayPOID";
            this.displayPOID.Size = new System.Drawing.Size(107, 23);
            this.displayPOID.TabIndex = 6;
            // 
            // labelPOID
            // 
            this.labelPOID.Location = new System.Drawing.Point(223, 41);
            this.labelPOID.Name = "labelPOID";
            this.labelPOID.Size = new System.Drawing.Size(75, 23);
            this.labelPOID.TabIndex = 48;
            this.labelPOID.Text = "PO#";
            // 
            // txtOrderID
            // 
            this.txtOrderID.BackColor = System.Drawing.Color.White;
            this.txtOrderID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OrderId", true));
            this.txtOrderID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtOrderID.Location = new System.Drawing.Point(87, 41);
            this.txtOrderID.Name = "txtOrderID";
            this.txtOrderID.Size = new System.Drawing.Size(107, 23);
            this.txtOrderID.TabIndex = 2;
            this.txtOrderID.Validating += new System.ComponentModel.CancelEventHandler(this.TxtOrderID_Validating);
            this.txtOrderID.Validated += new System.EventHandler(this.TxtOrderID_Validated);
            // 
            // labelOrderID
            // 
            this.labelOrderID.Location = new System.Drawing.Point(9, 40);
            this.labelOrderID.Name = "labelOrderID";
            this.labelOrderID.Size = new System.Drawing.Size(75, 23);
            this.labelOrderID.TabIndex = 52;
            this.labelOrderID.Text = "Order ID";
            // 
            // gridIssueBreakDown
            // 
            this.gridIssueBreakDown.AllowUserToAddRows = false;
            this.gridIssueBreakDown.AllowUserToDeleteRows = false;
            this.gridIssueBreakDown.AllowUserToResizeRows = false;
            this.gridIssueBreakDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridIssueBreakDown.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridIssueBreakDown.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridIssueBreakDown.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridIssueBreakDown.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridIssueBreakDown.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridIssueBreakDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridIssueBreakDown.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridIssueBreakDown.Location = new System.Drawing.Point(0, 2);
            this.gridIssueBreakDown.Name = "gridIssueBreakDown";
            this.gridIssueBreakDown.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridIssueBreakDown.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridIssueBreakDown.RowTemplate.Height = 24;
            this.gridIssueBreakDown.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridIssueBreakDown.ShowCellToolTips = false;
            this.gridIssueBreakDown.Size = new System.Drawing.Size(996, 129);
            this.gridIssueBreakDown.TabIndex = 3;
            this.gridIssueBreakDown.TabStop = false;
            // 
            // checkByCombo
            // 
            this.checkByCombo.AutoSize = true;
            this.checkByCombo.Checked = true;
            this.checkByCombo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkByCombo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Combo", true));
            this.checkByCombo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkByCombo.Location = new System.Drawing.Point(301, 72);
            this.checkByCombo.Name = "checkByCombo";
            this.checkByCombo.Size = new System.Drawing.Size(91, 21);
            this.checkByCombo.TabIndex = 7;
            this.checkByCombo.Text = "By Combo";
            this.checkByCombo.UseVisualStyleBackColor = true;
            this.checkByCombo.CheckedChanged += new System.EventHandler(this.CheckByCombo_CheckedChanged);
            // 
            // labelConfirmed
            // 
            this.labelConfirmed.BackColor = System.Drawing.Color.Transparent;
            this.labelConfirmed.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.labelConfirmed.Location = new System.Drawing.Point(843, 3);
            this.labelConfirmed.Name = "labelConfirmed";
            this.labelConfirmed.Size = new System.Drawing.Size(148, 25);
            this.labelConfirmed.TabIndex = 59;
            this.labelConfirmed.Text = "status";
            this.labelConfirmed.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelConfirmed.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.labelConfirmed.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // displayStyle
            // 
            this.displayStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyle.Location = new System.Drawing.Point(529, 41);
            this.displayStyle.Name = "displayStyle";
            this.displayStyle.Size = new System.Drawing.Size(107, 23);
            this.displayStyle.TabIndex = 60;
            // 
            // lblStyle
            // 
            this.lblStyle.Location = new System.Drawing.Point(451, 41);
            this.lblStyle.Name = "lblStyle";
            this.lblStyle.Size = new System.Drawing.Size(75, 23);
            this.lblStyle.TabIndex = 61;
            this.lblStyle.Text = "Style";
            // 
            // btnCallP99
            // 
            this.btnCallP99.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnCallP99.Location = new System.Drawing.Point(876, 136);
            this.btnCallP99.Name = "btnCallP99";
            this.btnCallP99.Size = new System.Drawing.Size(100, 35);
            this.btnCallP99.TabIndex = 71;
            this.btnCallP99.Text = "Link To P99";
            this.btnCallP99.UseVisualStyleBackColor = true;
            this.btnCallP99.Click += new System.EventHandler(this.BtnCallP99_Click);
            // 
            // P33
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1007, 615);
            this.DefaultControlForEdit = "checkByCombo";
            this.DefaultOrder = "issuedate,id";
            this.ExpressQuery = true;
            this.GridAlias = "Issue_Summary";
            this.GridNew = 0;
            this.GridUniqueKey = "SCIRefno,ColorID";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "id";
            this.Name = "P33";
            this.OnLineHelpID = "Sci.Win.Tems.Input8";
            this.SubDetailKeyField1 = "id";
            this.SubDetailKeyField2 = "Issue_SummaryUkey";
            this.SubGridAlias = "Issue_Detail";
            this.Text = "P33. Issue Thread";
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
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridIssueBreakDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridIssueBreakDownBS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DisplayBox displayID;
        private Win.UI.Label labelID;
        private Win.UI.TextBox txtRemark;
        private Win.UI.Label labelRemark;
        private Win.UI.DisplayBox displayLineNo;
        private Win.UI.Label labelLineNo;
        private Win.UI.DateBox dateIssueDate;
        private Win.UI.Label labelIssueDate;
        private Win.UI.Button btnAutoPick;
        private Win.UI.Button btnBreakDown;
        private Win.UI.DisplayBox displayPOID;
        private Win.UI.Label labelPOID;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.TextBox txtOrderID;
        private Win.UI.Label labelOrderID;
        private Win.UI.Grid gridIssueBreakDown;
        private Win.UI.ListControlBindingSource gridIssueBreakDownBS;
        private Win.UI.CheckBox checkByCombo;
        private Win.UI.Label labelConfirmed;
        private Win.UI.DisplayBox displayStyle;
        private Win.UI.Label lblStyle;
        private Win.UI.Button btnCallP99;
    }
}
