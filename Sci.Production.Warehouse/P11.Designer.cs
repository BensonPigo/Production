namespace Sci.Production.Warehouse
{
    partial class P11
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
            this.labelRequest = new Sci.Win.UI.Label();
            this.txtRequest = new Sci.Win.UI.TextBox();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
            this.displayLineNo = new Sci.Win.UI.DisplayBox();
            this.labelLineNo = new Sci.Win.UI.Label();
            this.displayCutCell = new Sci.Win.UI.DisplayBox();
            this.labelCutCell = new Sci.Win.UI.Label();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.labelRemark = new Sci.Win.UI.Label();
            this.labelCutNo = new Sci.Win.UI.Label();
            this.editCutNo = new Sci.Win.UI.EditBox();
            this.btnAutoPick = new Sci.Win.UI.Button();
            this.label25 = new Sci.Win.UI.Label();
            this.btnBreakDown = new Sci.Win.UI.Button();
            this.editArticle = new Sci.Win.UI.EditBox();
            this.labelArticle = new Sci.Win.UI.Label();
            this.displayPOID = new Sci.Win.UI.DisplayBox();
            this.labelPOID = new Sci.Win.UI.Label();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnBOA = new Sci.Win.UI.Button();
            this.btnClear = new Sci.Win.UI.Button();
            this.txtOrderID = new Sci.Win.UI.TextBox();
            this.labelOrderID = new Sci.Win.UI.Label();
            this.gridIssueBreakDown = new Sci.Win.UI.Grid();
            this.gridIssueBreakDownBS = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.checkByCombo = new Sci.Win.UI.CheckBox();
            this.lbCustCD = new Sci.Win.UI.Label();
            this.displayCustCD = new Sci.Win.UI.DisplayBox();
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
            this.masterpanel.Controls.Add(this.displayCustCD);
            this.masterpanel.Controls.Add(this.lbCustCD);
            this.masterpanel.Controls.Add(this.checkByCombo);
            this.masterpanel.Controls.Add(this.txtOrderID);
            this.masterpanel.Controls.Add(this.labelOrderID);
            this.masterpanel.Controls.Add(this.btnClear);
            this.masterpanel.Controls.Add(this.btnBOA);
            this.masterpanel.Controls.Add(this.displayPOID);
            this.masterpanel.Controls.Add(this.labelPOID);
            this.masterpanel.Controls.Add(this.editArticle);
            this.masterpanel.Controls.Add(this.labelArticle);
            this.masterpanel.Controls.Add(this.btnBreakDown);
            this.masterpanel.Controls.Add(this.label25);
            this.masterpanel.Controls.Add(this.btnAutoPick);
            this.masterpanel.Controls.Add(this.editCutNo);
            this.masterpanel.Controls.Add(this.labelCutNo);
            this.masterpanel.Controls.Add(this.txtRemark);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.displayCutCell);
            this.masterpanel.Controls.Add(this.labelCutCell);
            this.masterpanel.Controls.Add(this.displayLineNo);
            this.masterpanel.Controls.Add(this.labelLineNo);
            this.masterpanel.Controls.Add(this.labelIssueDate);
            this.masterpanel.Controls.Add(this.txtRequest);
            this.masterpanel.Controls.Add(this.labelRequest);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Controls.Add(this.dateIssueDate);
            this.masterpanel.Size = new System.Drawing.Size(892, 164);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRequest, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRequest, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelLineNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayLineNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCutCell, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayCutCell, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCutNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.editCutNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnAutoPick, 0);
            this.masterpanel.Controls.SetChildIndex(this.label25, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnBreakDown, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelArticle, 0);
            this.masterpanel.Controls.SetChildIndex(this.editArticle, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPOID, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayPOID, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnBOA, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnClear, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelOrderID, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtOrderID, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkByCombo, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbCustCD, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayCustCD, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 164);
            this.detailpanel.Size = new System.Drawing.Size(892, 49);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gridicon.Location = new System.Drawing.Point(2305, 128);
            // 
            // refresh
            // 
            this.refresh.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.refresh.Location = new System.Drawing.Point(911, 138);
            this.refresh.Size = new System.Drawing.Size(80, 36);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(892, 49);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(892, 213);
            // 
            // detailbtm
            // 
            this.detailbtm.Controls.Add(this.gridIssueBreakDown);
            this.detailbtm.Location = new System.Drawing.Point(0, 213);
            this.detailbtm.Size = new System.Drawing.Size(892, 174);
            this.detailbtm.Controls.SetChildIndex(this.lbleditby, 0);
            this.detailbtm.Controls.SetChildIndex(this.lblcreateby, 0);
            this.detailbtm.Controls.SetChildIndex(this.editby, 0);
            this.detailbtm.Controls.SetChildIndex(this.createby, 0);
            this.detailbtm.Controls.SetChildIndex(this.refresh, 0);
            this.detailbtm.Controls.SetChildIndex(this.gridIssueBreakDown, 0);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1035, 530);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1043, 559);
            // 
            // createby
            // 
            this.createby.Location = new System.Drawing.Point(70, 143);
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(469, 143);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lblcreateby
            // 
            this.lblcreateby.Location = new System.Drawing.Point(5, 149);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(421, 149);
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
            this.displayID.TabIndex = 2;
            // 
            // labelRequest
            // 
            this.labelRequest.Location = new System.Drawing.Point(9, 40);
            this.labelRequest.Name = "labelRequest";
            this.labelRequest.Size = new System.Drawing.Size(75, 23);
            this.labelRequest.TabIndex = 3;
            this.labelRequest.Text = "Request";
            // 
            // txtRequest
            // 
            this.txtRequest.BackColor = System.Drawing.Color.White;
            this.txtRequest.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "cutplanID", true));
            this.txtRequest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRequest.Location = new System.Drawing.Point(87, 40);
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.Size = new System.Drawing.Size(107, 23);
            this.txtRequest.TabIndex = 1;
            this.txtRequest.Validating += new System.ComponentModel.CancelEventHandler(this.TxtRequest_Validating);
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
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "issuedate", true));
            this.dateIssueDate.Location = new System.Drawing.Point(302, 9);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(130, 23);
            this.dateIssueDate.TabIndex = 0;
            // 
            // displayLineNo
            // 
            this.displayLineNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayLineNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayLineNo.Location = new System.Drawing.Point(87, 102);
            this.displayLineNo.Name = "displayLineNo";
            this.displayLineNo.Size = new System.Drawing.Size(211, 23);
            this.displayLineNo.TabIndex = 8;
            // 
            // labelLineNo
            // 
            this.labelLineNo.Location = new System.Drawing.Point(9, 102);
            this.labelLineNo.Name = "labelLineNo";
            this.labelLineNo.Size = new System.Drawing.Size(75, 23);
            this.labelLineNo.TabIndex = 7;
            this.labelLineNo.Text = "Line#";
            // 
            // displayCutCell
            // 
            this.displayCutCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCutCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCutCell.Location = new System.Drawing.Point(380, 102);
            this.displayCutCell.Name = "displayCutCell";
            this.displayCutCell.Size = new System.Drawing.Size(100, 23);
            this.displayCutCell.TabIndex = 10;
            // 
            // labelCutCell
            // 
            this.labelCutCell.Location = new System.Drawing.Point(302, 102);
            this.labelCutCell.Name = "labelCutCell";
            this.labelCutCell.Size = new System.Drawing.Size(75, 23);
            this.labelCutCell.TabIndex = 9;
            this.labelCutCell.Text = "Cut Cell";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(87, 133);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(340, 23);
            this.txtRemark.TabIndex = 4;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(9, 133);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 11;
            this.labelRemark.Text = "Remark";
            // 
            // labelCutNo
            // 
            this.labelCutNo.Location = new System.Drawing.Point(441, 9);
            this.labelCutNo.Name = "labelCutNo";
            this.labelCutNo.Size = new System.Drawing.Size(75, 23);
            this.labelCutNo.TabIndex = 13;
            this.labelCutNo.Text = "Cut#";
            // 
            // editCutNo
            // 
            this.editCutNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editCutNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editCutNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editCutNo.IsSupportEditMode = false;
            this.editCutNo.Location = new System.Drawing.Point(519, 8);
            this.editCutNo.Multiline = true;
            this.editCutNo.Name = "editCutNo";
            this.editCutNo.ReadOnly = true;
            this.editCutNo.Size = new System.Drawing.Size(248, 55);
            this.editCutNo.TabIndex = 14;
            // 
            // btnAutoPick
            // 
            this.btnAutoPick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoPick.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnAutoPick.Location = new System.Drawing.Point(804, 31);
            this.btnAutoPick.Name = "btnAutoPick";
            this.btnAutoPick.Size = new System.Drawing.Size(80, 30);
            this.btnAutoPick.TabIndex = 5;
            this.btnAutoPick.Text = "AutoPick";
            this.btnAutoPick.UseVisualStyleBackColor = true;
            this.btnAutoPick.Click += new System.EventHandler(this.BtnAutoPick_Click);
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label25.Location = new System.Drawing.Point(772, 8);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(115, 23);
            this.label25.TabIndex = 44;
            this.label25.Text = "Not Approve";
            this.label25.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // btnBreakDown
            // 
            this.btnBreakDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBreakDown.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnBreakDown.Enabled = false;
            this.btnBreakDown.Location = new System.Drawing.Point(769, 63);
            this.btnBreakDown.Name = "btnBreakDown";
            this.btnBreakDown.Size = new System.Drawing.Size(120, 30);
            this.btnBreakDown.TabIndex = 6;
            this.btnBreakDown.Text = "Issue B\'down";
            this.btnBreakDown.UseVisualStyleBackColor = true;
            this.btnBreakDown.Click += new System.EventHandler(this.BtnBreakDown_Click);
            // 
            // editArticle
            // 
            this.editArticle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editArticle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editArticle.IsSupportEditMode = false;
            this.editArticle.Location = new System.Drawing.Point(519, 70);
            this.editArticle.Multiline = true;
            this.editArticle.Name = "editArticle";
            this.editArticle.ReadOnly = true;
            this.editArticle.Size = new System.Drawing.Size(248, 55);
            this.editArticle.TabIndex = 47;
            // 
            // labelArticle
            // 
            this.labelArticle.Location = new System.Drawing.Point(441, 71);
            this.labelArticle.Name = "labelArticle";
            this.labelArticle.Size = new System.Drawing.Size(75, 23);
            this.labelArticle.TabIndex = 46;
            this.labelArticle.Text = "Article";
            // 
            // displayPOID
            // 
            this.displayPOID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPOID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPOID.Location = new System.Drawing.Point(302, 71);
            this.displayPOID.Name = "displayPOID";
            this.displayPOID.Size = new System.Drawing.Size(107, 23);
            this.displayPOID.TabIndex = 49;
            // 
            // labelPOID
            // 
            this.labelPOID.Location = new System.Drawing.Point(224, 71);
            this.labelPOID.Name = "labelPOID";
            this.labelPOID.Size = new System.Drawing.Size(75, 23);
            this.labelPOID.TabIndex = 48;
            this.labelPOID.Text = "PO ID";
            // 
            // btnBOA
            // 
            this.btnBOA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBOA.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnBOA.Location = new System.Drawing.Point(804, 95);
            this.btnBOA.Name = "btnBOA";
            this.btnBOA.Size = new System.Drawing.Size(80, 30);
            this.btnBOA.TabIndex = 7;
            this.btnBOA.Text = "BOA";
            this.btnBOA.UseVisualStyleBackColor = true;
            this.btnBOA.Click += new System.EventHandler(this.BtnBOA_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnClear.Location = new System.Drawing.Point(670, 128);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(108, 30);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear Qty=0";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // txtOrderID
            // 
            this.txtOrderID.BackColor = System.Drawing.Color.White;
            this.txtOrderID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OrderId", true));
            this.txtOrderID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtOrderID.Location = new System.Drawing.Point(87, 71);
            this.txtOrderID.Name = "txtOrderID";
            this.txtOrderID.Size = new System.Drawing.Size(107, 23);
            this.txtOrderID.TabIndex = 3;
            this.txtOrderID.Validating += new System.ComponentModel.CancelEventHandler(this.TxtOrderID_Validating);
            this.txtOrderID.Validated += new System.EventHandler(this.TxtOrderID_Validated);
            // 
            // labelOrderID
            // 
            this.labelOrderID.Location = new System.Drawing.Point(9, 70);
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
            this.gridIssueBreakDown.Size = new System.Drawing.Size(889, 129);
            this.gridIssueBreakDown.TabIndex = 3;
            this.gridIssueBreakDown.TabStop = false;
            // 
            // checkByCombo
            // 
            this.checkByCombo.AutoSize = true;
            this.checkByCombo.Checked = true;
            this.checkByCombo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkByCombo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "combo", true));
            this.checkByCombo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkByCombo.Location = new System.Drawing.Point(224, 40);
            this.checkByCombo.Name = "checkByCombo";
            this.checkByCombo.Size = new System.Drawing.Size(91, 21);
            this.checkByCombo.TabIndex = 2;
            this.checkByCombo.Text = "By Combo";
            this.checkByCombo.UseVisualStyleBackColor = true;
            this.checkByCombo.CheckedChanged += new System.EventHandler(this.CheckByCombo_CheckedChanged);
            // 
            // lbCustCD
            // 
            this.lbCustCD.Location = new System.Drawing.Point(441, 133);
            this.lbCustCD.Name = "lbCustCD";
            this.lbCustCD.Size = new System.Drawing.Size(75, 23);
            this.lbCustCD.TabIndex = 53;
            this.lbCustCD.Text = "CustCD";
            // 
            // displayCustCD
            // 
            this.displayCustCD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCustCD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCustCD.Location = new System.Drawing.Point(519, 132);
            this.displayCustCD.Name = "displayCustCD";
            this.displayCustCD.Size = new System.Drawing.Size(100, 23);
            this.displayCustCD.TabIndex = 54;
            // 
            // P11
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1043, 592);
            this.DefaultControl = "txtRequest";
            this.DefaultControlForEdit = "checkByCombo";
            this.DefaultDetailOrder = "poid,seq1,seq2,dyelot,roll";
            this.DefaultOrder = "issuedate,id";
            this.ExpressQuery = true;
            this.GridAlias = "Issue_Detail";
            this.GridNew = 0;
            this.GridUniqueKey = "POID,Seq1,Seq2,MDivisionID";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "id";
            this.Name = "P11";
            this.OnLineHelpID = "Sci.Win.Tems.Input8";
            this.SubGridAlias = "Issue_size";
            this.Text = "P11. Issue Sewing Material by Transfer Guide";
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
        private Win.UI.EditBox editCutNo;
        private Win.UI.Label labelCutNo;
        private Win.UI.TextBox txtRemark;
        private Win.UI.Label labelRemark;
        private Win.UI.DisplayBox displayCutCell;
        private Win.UI.Label labelCutCell;
        private Win.UI.DisplayBox displayLineNo;
        private Win.UI.Label labelLineNo;
        private Win.UI.DateBox dateIssueDate;
        private Win.UI.Label labelIssueDate;
        private Win.UI.TextBox txtRequest;
        private Win.UI.Label labelRequest;
        private Win.UI.Button btnAutoPick;
        private Win.UI.Label label25;
        private Win.UI.Button btnBreakDown;
        private Win.UI.DisplayBox displayPOID;
        private Win.UI.Label labelPOID;
        private Win.UI.EditBox editArticle;
        private Win.UI.Label labelArticle;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnBOA;
        private Win.UI.Button btnClear;
        private Win.UI.TextBox txtOrderID;
        private Win.UI.Label labelOrderID;
        private Win.UI.Grid gridIssueBreakDown;
        private Win.UI.ListControlBindingSource gridIssueBreakDownBS;
        private Win.UI.CheckBox checkByCombo;
        private Win.UI.DisplayBox displayCustCD;
        private Win.UI.Label lbCustCD;
    }
}
