namespace Sci.Production.PPIC
{
    partial class P10
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
            this.labelDate = new Sci.Win.UI.Label();
            this.labelType = new Sci.Win.UI.Label();
            this.labelShift = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.displayNo = new Sci.Win.UI.DisplayBox();
            this.dateDate = new Sci.Win.UI.DateBox();
            this.comboType = new Sci.Win.UI.ComboBox();
            this.comboShift = new Sci.Win.UI.ComboBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.labelMasterSP = new Sci.Win.UI.Label();
            this.labelSewingLine = new Sci.Win.UI.Label();
            this.displayFactory = new Sci.Win.UI.DisplayBox();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.displayMasterSP = new Sci.Win.UI.DisplayBox();
            this.labelHandle = new Sci.Win.UI.Label();
            this.labelApprove = new Sci.Win.UI.Label();
            this.labelApvDate = new Sci.Win.UI.Label();
            this.labelIssueNo = new Sci.Win.UI.Label();
            this.dateApvDate = new Sci.Win.UI.DateBox();
            this.displayIssueNo = new Sci.Win.UI.DisplayBox();
            this.lbStatus = new Sci.Win.UI.Label();
            this.labelIssueLackDate = new Sci.Win.UI.Label();
            this.displayIssueLackDate = new Sci.Win.UI.DisplayBox();
            this.txtuserApprove = new Sci.Production.Class.Txtuser();
            this.txtuserHandle = new Sci.Production.Class.Txtuser();
            this.txtsewingline = new Sci.Production.Class.Txtsewingline();
            this.btnImport = new Sci.Win.UI.Button();
            this.labelSubconName = new Sci.Win.UI.Label();
            this.btnAutoOutputQuery = new Sci.Win.UI.Button();
            this.txtLocalSupp1 = new Sci.Production.Class.TxtLocalSupp();
            this.lblDept = new Sci.Win.UI.Label();
            this.txtDept = new Sci.Win.UI.TextBox();
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
            this.masterpanel.Controls.Add(this.txtDept);
            this.masterpanel.Controls.Add(this.lblDept);
            this.masterpanel.Controls.Add(this.txtLocalSupp1);
            this.masterpanel.Controls.Add(this.labelSubconName);
            this.masterpanel.Controls.Add(this.btnImport);
            this.masterpanel.Controls.Add(this.displayIssueLackDate);
            this.masterpanel.Controls.Add(this.labelIssueLackDate);
            this.masterpanel.Controls.Add(this.lbStatus);
            this.masterpanel.Controls.Add(this.displayIssueNo);
            this.masterpanel.Controls.Add(this.txtuserApprove);
            this.masterpanel.Controls.Add(this.txtuserHandle);
            this.masterpanel.Controls.Add(this.labelIssueNo);
            this.masterpanel.Controls.Add(this.labelApvDate);
            this.masterpanel.Controls.Add(this.labelApprove);
            this.masterpanel.Controls.Add(this.labelHandle);
            this.masterpanel.Controls.Add(this.txtsewingline);
            this.masterpanel.Controls.Add(this.displayMasterSP);
            this.masterpanel.Controls.Add(this.txtSP);
            this.masterpanel.Controls.Add(this.displayFactory);
            this.masterpanel.Controls.Add(this.labelSewingLine);
            this.masterpanel.Controls.Add(this.labelMasterSP);
            this.masterpanel.Controls.Add(this.labelSP);
            this.masterpanel.Controls.Add(this.labelFactory);
            this.masterpanel.Controls.Add(this.txtRemark);
            this.masterpanel.Controls.Add(this.comboShift);
            this.masterpanel.Controls.Add(this.comboType);
            this.masterpanel.Controls.Add(this.displayNo);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.labelShift);
            this.masterpanel.Controls.Add(this.labelType);
            this.masterpanel.Controls.Add(this.labelDate);
            this.masterpanel.Controls.Add(this.labelNo);
            this.masterpanel.Controls.Add(this.dateApvDate);
            this.masterpanel.Controls.Add(this.dateDate);
            this.masterpanel.Size = new System.Drawing.Size(1068, 172);
            this.masterpanel.Controls.SetChildIndex(this.dateDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateApvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelType, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShift, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboType, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboShift, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelMasterSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSewingLine, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayMasterSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtsewingline, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelApprove, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelApvDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelIssueNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserApprove, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayIssueNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbStatus, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelIssueLackDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayIssueLackDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImport, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSubconName, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLocalSupp1, 0);
            this.masterpanel.Controls.SetChildIndex(this.lblDept, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtDept, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 172);
            this.detailpanel.Size = new System.Drawing.Size(1068, 277);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(810, 137);
            this.gridicon.TabIndex = 6;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(827, 3);
            this.refresh.TabIndex = 3;
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1068, 277);
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
            this.detail.Size = new System.Drawing.Size(1068, 487);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1068, 449);
            // 
            // detailbtm
            // 
            this.detailbtm.Controls.Add(this.btnAutoOutputQuery);
            this.detailbtm.Location = new System.Drawing.Point(0, 449);
            this.detailbtm.Size = new System.Drawing.Size(1068, 38);
            this.detailbtm.TabIndex = 0;
            this.detailbtm.Controls.SetChildIndex(this.lbleditby, 0);
            this.detailbtm.Controls.SetChildIndex(this.lblcreateby, 0);
            this.detailbtm.Controls.SetChildIndex(this.editby, 0);
            this.detailbtm.Controls.SetChildIndex(this.createby, 0);
            this.detailbtm.Controls.SetChildIndex(this.refresh, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnAutoOutputQuery, 0);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1068, 487);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1076, 516);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            this.createby.TabIndex = 1;
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 2;
            // 
            // lblcreateby
            // 
            this.lblcreateby.TabIndex = 0;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelNo
            // 
            this.labelNo.Location = new System.Drawing.Point(5, 4);
            this.labelNo.Name = "labelNo";
            this.labelNo.Size = new System.Drawing.Size(92, 23);
            this.labelNo.TabIndex = 0;
            this.labelNo.Text = "No.";
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(5, 31);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(92, 23);
            this.labelDate.TabIndex = 2;
            this.labelDate.Text = "Date";
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(5, 58);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(92, 23);
            this.labelType.TabIndex = 3;
            this.labelType.Text = "Type";
            // 
            // labelShift
            // 
            this.labelShift.Location = new System.Drawing.Point(5, 85);
            this.labelShift.Name = "labelShift";
            this.labelShift.Size = new System.Drawing.Size(92, 23);
            this.labelShift.TabIndex = 4;
            this.labelShift.Text = "Shift";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(5, 139);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(92, 23);
            this.labelRemark.TabIndex = 5;
            this.labelRemark.Text = "Remark";
            // 
            // displayNo
            // 
            this.displayNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayNo.Location = new System.Drawing.Point(103, 4);
            this.displayNo.Name = "displayNo";
            this.displayNo.Size = new System.Drawing.Size(120, 23);
            this.displayNo.TabIndex = 0;
            // 
            // dateDate
            // 
            this.dateDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IssueDate", true));
            this.dateDate.IsSupportEditMode = false;
            this.dateDate.Location = new System.Drawing.Point(103, 31);
            this.dateDate.Name = "dateDate";
            this.dateDate.ReadOnly = true;
            this.dateDate.Size = new System.Drawing.Size(110, 23);
            this.dateDate.TabIndex = 3;
            this.dateDate.TabStop = false;
            // 
            // comboType
            // 
            this.comboType.BackColor = System.Drawing.Color.White;
            this.comboType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Type", true));
            this.comboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType.FormattingEnabled = true;
            this.comboType.IsSupportUnselect = true;
            this.comboType.Location = new System.Drawing.Point(103, 57);
            this.comboType.Name = "comboType";
            this.comboType.OldText = "";
            this.comboType.Size = new System.Drawing.Size(121, 24);
            this.comboType.TabIndex = 0;
            this.comboType.Validated += new System.EventHandler(this.ComboType_Validated);
            // 
            // comboShift
            // 
            this.comboShift.BackColor = System.Drawing.Color.White;
            this.comboShift.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Shift", true));
            this.comboShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShift.FormattingEnabled = true;
            this.comboShift.IsSupportUnselect = true;
            this.comboShift.Location = new System.Drawing.Point(103, 84);
            this.comboShift.Name = "comboShift";
            this.comboShift.OldText = "";
            this.comboShift.Size = new System.Drawing.Size(110, 24);
            this.comboShift.TabIndex = 1;
            this.comboShift.SelectedIndexChanged += new System.EventHandler(this.ComboShift_SelectedIndexChanged);
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(103, 139);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(577, 23);
            this.txtRemark.TabIndex = 2;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(239, 4);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(80, 23);
            this.labelFactory.TabIndex = 11;
            this.labelFactory.Text = "Factory";
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(239, 31);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(80, 23);
            this.labelSP.TabIndex = 12;
            this.labelSP.Text = "SP#";
            // 
            // labelMasterSP
            // 
            this.labelMasterSP.Location = new System.Drawing.Point(239, 58);
            this.labelMasterSP.Name = "labelMasterSP";
            this.labelMasterSP.Size = new System.Drawing.Size(80, 23);
            this.labelMasterSP.TabIndex = 13;
            this.labelMasterSP.Text = "Master SP#";
            // 
            // labelSewingLine
            // 
            this.labelSewingLine.Location = new System.Drawing.Point(239, 85);
            this.labelSewingLine.Name = "labelSewingLine";
            this.labelSewingLine.Size = new System.Drawing.Size(80, 23);
            this.labelSewingLine.TabIndex = 14;
            this.labelSewingLine.Text = "Sewing Line";
            // 
            // displayFactory
            // 
            this.displayFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFactory.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FactoryID", true));
            this.displayFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFactory.Location = new System.Drawing.Point(323, 4);
            this.displayFactory.Name = "displayFactory";
            this.displayFactory.Size = new System.Drawing.Size(60, 23);
            this.displayFactory.TabIndex = 3;
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OrderID", true));
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(323, 31);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(120, 23);
            this.txtSP.TabIndex = 3;
            this.txtSP.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSP_Validating);
            // 
            // displayMasterSP
            // 
            this.displayMasterSP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayMasterSP.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "POID", true));
            this.displayMasterSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayMasterSP.Location = new System.Drawing.Point(323, 57);
            this.displayMasterSP.Name = "displayMasterSP";
            this.displayMasterSP.Size = new System.Drawing.Size(120, 23);
            this.displayMasterSP.TabIndex = 7;
            // 
            // labelHandle
            // 
            this.labelHandle.Location = new System.Drawing.Point(492, 4);
            this.labelHandle.Name = "labelHandle";
            this.labelHandle.Size = new System.Drawing.Size(64, 23);
            this.labelHandle.TabIndex = 19;
            this.labelHandle.Text = "Handle";
            // 
            // labelApprove
            // 
            this.labelApprove.Location = new System.Drawing.Point(492, 31);
            this.labelApprove.Name = "labelApprove";
            this.labelApprove.Size = new System.Drawing.Size(64, 23);
            this.labelApprove.TabIndex = 20;
            this.labelApprove.Text = "Approve";
            // 
            // labelApvDate
            // 
            this.labelApvDate.Location = new System.Drawing.Point(492, 57);
            this.labelApvDate.Name = "labelApvDate";
            this.labelApvDate.Size = new System.Drawing.Size(64, 23);
            this.labelApvDate.TabIndex = 21;
            this.labelApvDate.Text = "Apv Date";
            // 
            // labelIssueNo
            // 
            this.labelIssueNo.Location = new System.Drawing.Point(393, 84);
            this.labelIssueNo.Name = "labelIssueNo";
            this.labelIssueNo.Size = new System.Drawing.Size(78, 23);
            this.labelIssueNo.TabIndex = 22;
            this.labelIssueNo.Text = "Issue No.";
            // 
            // dateApvDate
            // 
            this.dateApvDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ApvDate", true));
            this.dateApvDate.IsSupportEditMode = false;
            this.dateApvDate.Location = new System.Drawing.Point(560, 58);
            this.dateApvDate.Name = "dateApvDate";
            this.dateApvDate.ReadOnly = true;
            this.dateApvDate.Size = new System.Drawing.Size(110, 23);
            this.dateApvDate.TabIndex = 8;
            this.dateApvDate.TabStop = false;
            // 
            // displayIssueNo
            // 
            this.displayIssueNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayIssueNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IssueLackId", true));
            this.displayIssueNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayIssueNo.Location = new System.Drawing.Point(474, 84);
            this.displayIssueNo.Name = "displayIssueNo";
            this.displayIssueNo.Size = new System.Drawing.Size(120, 23);
            this.displayIssueNo.TabIndex = 11;
            // 
            // lbStatus
            // 
            this.lbStatus.BackColor = System.Drawing.Color.Transparent;
            this.lbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.lbStatus.Location = new System.Drawing.Point(700, 55);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(120, 31);
            this.lbStatus.TabIndex = 36;
            this.lbStatus.Text = "Confirmed";
            this.lbStatus.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.lbStatus.TextStyle.Color = System.Drawing.Color.Red;
            this.lbStatus.TextStyle.ExtBorderColor = System.Drawing.Color.Red;
            this.lbStatus.TextStyle.GradientColor = System.Drawing.Color.Red;
            // 
            // labelIssueLackDate
            // 
            this.labelIssueLackDate.Location = new System.Drawing.Point(597, 84);
            this.labelIssueLackDate.Name = "labelIssueLackDate";
            this.labelIssueLackDate.Size = new System.Drawing.Size(108, 23);
            this.labelIssueLackDate.TabIndex = 37;
            this.labelIssueLackDate.Text = "Issue Lack Date";
            // 
            // displayIssueLackDate
            // 
            this.displayIssueLackDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayIssueLackDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayIssueLackDate.Location = new System.Drawing.Point(708, 84);
            this.displayIssueLackDate.Name = "displayIssueLackDate";
            this.displayIssueLackDate.Size = new System.Drawing.Size(154, 23);
            this.displayIssueLackDate.TabIndex = 38;
            // 
            // txtuserApprove
            // 
            this.txtuserApprove.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "ApvName", true));
            this.txtuserApprove.DisplayBox1Binding = "";
            this.txtuserApprove.Location = new System.Drawing.Point(560, 31);
            this.txtuserApprove.Name = "txtuserApprove";
            this.txtuserApprove.Size = new System.Drawing.Size(302, 23);
            this.txtuserApprove.TabIndex = 6;
            this.txtuserApprove.TabStop = false;
            this.txtuserApprove.TextBox1Binding = "";
            // 
            // txtuserHandle
            // 
            this.txtuserHandle.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "ApplyName", true));
            this.txtuserHandle.DisplayBox1Binding = "";
            this.txtuserHandle.Location = new System.Drawing.Point(560, 4);
            this.txtuserHandle.Name = "txtuserHandle";
            this.txtuserHandle.Size = new System.Drawing.Size(315, 23);
            this.txtuserHandle.TabIndex = 5;
            this.txtuserHandle.TextBox1Binding = "";
            // 
            // txtsewingline
            // 
            this.txtsewingline.BackColor = System.Drawing.Color.White;
            this.txtsewingline.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SewingLineID", true));
            this.txtsewingline.FactoryobjectName = this.displayFactory;
            this.txtsewingline.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtsewingline.Location = new System.Drawing.Point(323, 84);
            this.txtsewingline.Name = "txtsewingline";
            this.txtsewingline.Size = new System.Drawing.Size(60, 23);
            this.txtsewingline.TabIndex = 4;
            // 
            // btnImport
            // 
            this.btnImport.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(686, 135);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(118, 31);
            this.btnImport.TabIndex = 39;
            this.btnImport.Text = "Batch Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // labelSubconName
            // 
            this.labelSubconName.Location = new System.Drawing.Point(5, 111);
            this.labelSubconName.Name = "labelSubconName";
            this.labelSubconName.Size = new System.Drawing.Size(92, 23);
            this.labelSubconName.TabIndex = 40;
            this.labelSubconName.Text = "SubconName";
            // 
            // btnAutoOutputQuery
            // 
            this.btnAutoOutputQuery.Location = new System.Drawing.Point(913, 5);
            this.btnAutoOutputQuery.Name = "btnAutoOutputQuery";
            this.btnAutoOutputQuery.Size = new System.Drawing.Size(145, 30);
            this.btnAutoOutputQuery.TabIndex = 4;
            this.btnAutoOutputQuery.Text = "Auto Output Query";
            this.btnAutoOutputQuery.UseVisualStyleBackColor = true;
            this.btnAutoOutputQuery.Click += new System.EventHandler(this.BtnAutoOutputQuery_Click);
            // 
            // txtLocalSupp1
            // 
            this.txtLocalSupp1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "SubconName", true));
            this.txtLocalSupp1.DisplayBox1Binding = "";
            this.txtLocalSupp1.Location = new System.Drawing.Point(103, 111);
            this.txtLocalSupp1.Name = "txtLocalSupp1";
            this.txtLocalSupp1.Size = new System.Drawing.Size(252, 23);
            this.txtLocalSupp1.TabIndex = 44;
            this.txtLocalSupp1.TextBox1Binding = "";
            // 
            // lblDept
            // 
            this.lblDept.Location = new System.Drawing.Point(393, 111);
            this.lblDept.Name = "lblDept";
            this.lblDept.Size = new System.Drawing.Size(78, 23);
            this.lblDept.TabIndex = 45;
            this.lblDept.Text = "Department";
            // 
            // txtDept
            // 
            this.txtDept.BackColor = System.Drawing.Color.White;
            this.txtDept.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Dept", true));
            this.txtDept.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDept.Location = new System.Drawing.Point(474, 111);
            this.txtDept.Name = "txtDept";
            this.txtDept.Size = new System.Drawing.Size(120, 23);
            this.txtDept.TabIndex = 46;
            // 
            // P10
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1076, 549);
            this.DefaultControl = "txtSP";
            this.DefaultControlForEdit = "txtSP";
            this.DefaultDetailOrder = "Seq";
            this.DefaultOrder = "ID";
            this.ExpressQuery = true;
            this.GridAlias = "Lack_Detail";
            this.GridUniqueKey = "Seq";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportReceive = true;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P10";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.ReceiveChkValue = "Confirmed";
            this.Text = "P10. Fabric Lacking & Replacement";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "ID";
            this.WorkAlias = "Lack";
            this.FormLoaded += new System.EventHandler(this.P10_FormLoaded);
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

        private Win.UI.DisplayBox displayIssueNo;
        private Win.UI.DateBox dateApvDate;
        private Class.Txtuser txtuserApprove;
        private Class.Txtuser txtuserHandle;
        private Win.UI.Label labelIssueNo;
        private Win.UI.Label labelApvDate;
        private Win.UI.Label labelApprove;
        private Win.UI.Label labelHandle;
        private Class.Txtsewingline txtsewingline;
        private Win.UI.DisplayBox displayMasterSP;
        private Win.UI.TextBox txtSP;
        private Win.UI.DisplayBox displayFactory;
        private Win.UI.Label labelSewingLine;
        private Win.UI.Label labelMasterSP;
        private Win.UI.Label labelSP;
        private Win.UI.Label labelFactory;
        private Win.UI.TextBox txtRemark;
        private Win.UI.ComboBox comboShift;
        private Win.UI.ComboBox comboType;
        private Win.UI.DateBox dateDate;
        private Win.UI.DisplayBox displayNo;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelShift;
        private Win.UI.Label labelType;
        private Win.UI.Label labelDate;
        private Win.UI.Label labelNo;
        private Win.UI.Label lbStatus;
        private Win.UI.DisplayBox displayIssueLackDate;
        private Win.UI.Label labelIssueLackDate;
        private Win.UI.Button btnImport;
        private Win.UI.Label labelSubconName;
        private Win.UI.Button btnAutoOutputQuery;
        private Class.TxtLocalSupp txtLocalSupp1;
        private Win.UI.TextBox txtDept;
        private Win.UI.Label lblDept;
    }
}
