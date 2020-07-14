namespace Sci.Production.Subcon
{
    partial class P05
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
            this.labelPONo = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelArtworkType = new Sci.Win.UI.Label();
            this.labelReqDate = new Sci.Win.UI.Label();
            this.labelHandle = new Sci.Win.UI.Label();
            this.labStatus = new Sci.Win.UI.Label();
            this.labExceed = new Sci.Win.UI.Label();
            this.btnBatchImport = new Sci.Win.UI.Button();
            this.displayReqID = new Sci.Win.UI.DisplayBox();
            this.dateReqDate = new Sci.Win.UI.DateBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.btnBatchCreate = new Sci.Win.UI.Button();
            this.btnIrrQtyReason = new Sci.Win.UI.Button();
            this.btnBatchApprove = new Sci.Win.UI.Button();
            this.labDeptMgrApv = new Sci.Win.UI.Label();
            this.dispDeptApv = new Sci.Win.UI.DisplayBox();
            this.dispMgrApv = new Sci.Win.UI.DisplayBox();
            this.labMGMgrApv = new Sci.Win.UI.Label();
            this.dispClosed = new Sci.Win.UI.DisplayBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtmfactory = new Sci.Production.Class.txtfactory();
            this.txtuserHandle = new Sci.Production.Class.txtuser();
            this.txtartworktype_ftyArtworkType = new Sci.Production.Class.txtartworktype_fty();
            this.txtsubconSupplier = new Sci.Production.Class.txtsubconNoConfirm();
            this.btnSpecialRecord = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.btnSpecialRecord);
            this.masterpanel.Controls.Add(this.btnIrrQtyReason);
            this.masterpanel.Controls.Add(this.txtmfactory);
            this.masterpanel.Controls.Add(this.txtuserHandle);
            this.masterpanel.Controls.Add(this.txtRemark);
            this.masterpanel.Controls.Add(this.txtartworktype_ftyArtworkType);
            this.masterpanel.Controls.Add(this.txtsubconSupplier);
            this.masterpanel.Controls.Add(this.displayReqID);
            this.masterpanel.Controls.Add(this.btnBatchImport);
            this.masterpanel.Controls.Add(this.labExceed);
            this.masterpanel.Controls.Add(this.labStatus);
            this.masterpanel.Controls.Add(this.labelHandle);
            this.masterpanel.Controls.Add(this.labelReqDate);
            this.masterpanel.Controls.Add(this.labelArtworkType);
            this.masterpanel.Controls.Add(this.labelSupplier);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.labelFactory);
            this.masterpanel.Controls.Add(this.labelPONo);
            this.masterpanel.Controls.Add(this.dateReqDate);
            this.masterpanel.Size = new System.Drawing.Size(892, 149);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateReqDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPONo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSupplier, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelArtworkType, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelReqDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.labStatus, 0);
            this.masterpanel.Controls.SetChildIndex(this.labExceed, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnBatchImport, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayReqID, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtsubconSupplier, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtartworktype_ftyArtworkType, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserHandle, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtmfactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnIrrQtyReason, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnSpecialRecord, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 149);
            this.detailpanel.Size = new System.Drawing.Size(892, 136);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.gridicon.Location = new System.Drawing.Point(946, 113);
            this.gridicon.TabIndex = 10;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(880, 61);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(892, 136);
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
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(892, 285);
            // 
            // detailbtm
            // 
            this.detailbtm.Controls.Add(this.dispClosed);
            this.detailbtm.Controls.Add(this.label1);
            this.detailbtm.Controls.Add(this.dispMgrApv);
            this.detailbtm.Controls.Add(this.labMGMgrApv);
            this.detailbtm.Controls.Add(this.dispDeptApv);
            this.detailbtm.Controls.Add(this.labDeptMgrApv);
            this.detailbtm.Location = new System.Drawing.Point(0, 285);
            this.detailbtm.Size = new System.Drawing.Size(892, 102);
            this.detailbtm.TabIndex = 1;
            this.detailbtm.Controls.SetChildIndex(this.labDeptMgrApv, 0);
            this.detailbtm.Controls.SetChildIndex(this.lbleditby, 0);
            this.detailbtm.Controls.SetChildIndex(this.dispDeptApv, 0);
            this.detailbtm.Controls.SetChildIndex(this.lblcreateby, 0);
            this.detailbtm.Controls.SetChildIndex(this.editby, 0);
            this.detailbtm.Controls.SetChildIndex(this.labMGMgrApv, 0);
            this.detailbtm.Controls.SetChildIndex(this.dispMgrApv, 0);
            this.detailbtm.Controls.SetChildIndex(this.createby, 0);
            this.detailbtm.Controls.SetChildIndex(this.refresh, 0);
            this.detailbtm.Controls.SetChildIndex(this.label1, 0);
            this.detailbtm.Controls.SetChildIndex(this.dispClosed, 0);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1061, 556);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1069, 585);
            // 
            // createby
            // 
            this.createby.Location = new System.Drawing.Point(145, 8);
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(675, 8);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lblcreateby
            // 
            this.lblcreateby.Location = new System.Drawing.Point(6, 8);
            this.lblcreateby.Size = new System.Drawing.Size(136, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(558, 8);
            this.lbleditby.Size = new System.Drawing.Size(114, 23);
            // 
            // labelPONo
            // 
            this.labelPONo.Location = new System.Drawing.Point(6, 17);
            this.labelPONo.Name = "labelPONo";
            this.labelPONo.Size = new System.Drawing.Size(97, 23);
            this.labelPONo.TabIndex = 1;
            this.labelPONo.Text = "Requisition #";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(285, 49);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(84, 23);
            this.labelFactory.TabIndex = 4;
            this.labelFactory.Text = "Factory";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(6, 115);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(97, 23);
            this.labelRemark.TabIndex = 5;
            this.labelRemark.Text = "Remark";
            // 
            // labelSupplier
            // 
            this.labelSupplier.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelSupplier.Location = new System.Drawing.Point(6, 49);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(97, 23);
            this.labelSupplier.TabIndex = 6;
            this.labelSupplier.Text = "Supplier";
            this.labelSupplier.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelArtworkType.Location = new System.Drawing.Point(6, 83);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.Size = new System.Drawing.Size(97, 23);
            this.labelArtworkType.TabIndex = 7;
            this.labelArtworkType.Text = "ArtworkType";
            this.labelArtworkType.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelReqDate
            // 
            this.labelReqDate.Location = new System.Drawing.Point(285, 17);
            this.labelReqDate.Name = "labelReqDate";
            this.labelReqDate.Size = new System.Drawing.Size(84, 23);
            this.labelReqDate.TabIndex = 9;
            this.labelReqDate.Text = "Req. Date";
            // 
            // labelHandle
            // 
            this.labelHandle.Location = new System.Drawing.Point(285, 83);
            this.labelHandle.Name = "labelHandle";
            this.labelHandle.Size = new System.Drawing.Size(84, 23);
            this.labelHandle.TabIndex = 19;
            this.labelHandle.Text = "Handle";
            // 
            // labStatus
            // 
            this.labStatus.BackColor = System.Drawing.Color.Transparent;
            this.labStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.labStatus.Location = new System.Drawing.Point(729, 14);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(126, 23);
            this.labStatus.TabIndex = 6;
            this.labStatus.Text = "Not Approve";
            this.labStatus.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // labExceed
            // 
            this.labExceed.BackColor = System.Drawing.Color.Transparent;
            this.labExceed.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.labExceed.Location = new System.Drawing.Point(729, 50);
            this.labExceed.Name = "labExceed";
            this.labExceed.Size = new System.Drawing.Size(113, 23);
            this.labExceed.TabIndex = 7;
            this.labExceed.Text = "Exceed Qty";
            this.labExceed.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // btnBatchImport
            // 
            this.btnBatchImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBatchImport.Location = new System.Drawing.Point(858, 7);
            this.btnBatchImport.Name = "btnBatchImport";
            this.btnBatchImport.Size = new System.Drawing.Size(188, 30);
            this.btnBatchImport.TabIndex = 8;
            this.btnBatchImport.Text = "Batch Import";
            this.btnBatchImport.UseVisualStyleBackColor = true;
            this.btnBatchImport.Click += new System.EventHandler(this.btnBatchImport_Click);
            // 
            // displayReqID
            // 
            this.displayReqID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayReqID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayReqID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayReqID.Location = new System.Drawing.Point(107, 17);
            this.displayReqID.Name = "displayReqID";
            this.displayReqID.Size = new System.Drawing.Size(165, 23);
            this.displayReqID.TabIndex = 13;
            // 
            // dateReqDate
            // 
            this.dateReqDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ReqDate", true));
            this.dateReqDate.Location = new System.Drawing.Point(373, 17);
            this.dateReqDate.Name = "dateReqDate";
            this.dateReqDate.Size = new System.Drawing.Size(100, 23);
            this.dateReqDate.TabIndex = 3;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(107, 115);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(566, 23);
            this.txtRemark.TabIndex = 2;
            // 
            // btnBatchCreate
            // 
            this.btnBatchCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBatchCreate.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnBatchCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBatchCreate.Location = new System.Drawing.Point(816, 12);
            this.btnBatchCreate.Name = "btnBatchCreate";
            this.btnBatchCreate.Size = new System.Drawing.Size(115, 30);
            this.btnBatchCreate.TabIndex = 0;
            this.btnBatchCreate.Text = "Batch Create";
            this.btnBatchCreate.UseVisualStyleBackColor = true;
            this.btnBatchCreate.Click += new System.EventHandler(this.btnBatchCreate_Click);
            // 
            // btnIrrQtyReason
            // 
            this.btnIrrQtyReason.Enabled = false;
            this.btnIrrQtyReason.Location = new System.Drawing.Point(858, 76);
            this.btnIrrQtyReason.Name = "btnIrrQtyReason";
            this.btnIrrQtyReason.Size = new System.Drawing.Size(188, 30);
            this.btnIrrQtyReason.TabIndex = 9;
            this.btnIrrQtyReason.Text = "Irregular Qty Reason";
            this.btnIrrQtyReason.UseVisualStyleBackColor = true;
            this.btnIrrQtyReason.Click += new System.EventHandler(this.btnIrrPriceReason_Click);
            // 
            // btnBatchApprove
            // 
            this.btnBatchApprove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBatchApprove.Location = new System.Drawing.Point(937, 12);
            this.btnBatchApprove.Name = "btnBatchApprove";
            this.btnBatchApprove.Size = new System.Drawing.Size(127, 30);
            this.btnBatchApprove.TabIndex = 4;
            this.btnBatchApprove.Text = "Batch Approve";
            this.btnBatchApprove.UseVisualStyleBackColor = true;
            this.btnBatchApprove.Click += new System.EventHandler(this.btnBatchApprove_Click);
            // 
            // labDeptMgrApv
            // 
            this.labDeptMgrApv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labDeptMgrApv.Location = new System.Drawing.Point(6, 37);
            this.labDeptMgrApv.Name = "labDeptMgrApv";
            this.labDeptMgrApv.Size = new System.Drawing.Size(136, 23);
            this.labDeptMgrApv.TabIndex = 73;
            this.labDeptMgrApv.Text = "Dept. Mgr Apv";
            // 
            // dispDeptApv
            // 
            this.dispDeptApv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dispDeptApv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispDeptApv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispDeptApv.Location = new System.Drawing.Point(145, 37);
            this.dispDeptApv.Name = "dispDeptApv";
            this.dispDeptApv.Size = new System.Drawing.Size(350, 23);
            this.dispDeptApv.TabIndex = 74;
            // 
            // dispMgrApv
            // 
            this.dispMgrApv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dispMgrApv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispMgrApv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispMgrApv.Location = new System.Drawing.Point(675, 37);
            this.dispMgrApv.Name = "dispMgrApv";
            this.dispMgrApv.Size = new System.Drawing.Size(350, 23);
            this.dispMgrApv.TabIndex = 76;
            // 
            // labMGMgrApv
            // 
            this.labMGMgrApv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labMGMgrApv.Location = new System.Drawing.Point(558, 37);
            this.labMGMgrApv.Name = "labMGMgrApv";
            this.labMGMgrApv.Size = new System.Drawing.Size(114, 23);
            this.labMGMgrApv.TabIndex = 75;
            this.labMGMgrApv.Text = "Mg Mgr Apv";
            // 
            // dispClosed
            // 
            this.dispClosed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dispClosed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispClosed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispClosed.Location = new System.Drawing.Point(145, 66);
            this.dispClosed.Name = "dispClosed";
            this.dispClosed.Size = new System.Drawing.Size(350, 23);
            this.dispClosed.TabIndex = 78;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(6, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 23);
            this.label1.TabIndex = 77;
            this.label1.Text = "Closed/UnClosed By";
            // 
            // txtmfactory
            // 
            this.txtmfactory.BackColor = System.Drawing.Color.White;
            this.txtmfactory.boolFtyGroupList = true;
            this.txtmfactory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "factoryid", true));
            this.txtmfactory.FilteMDivision = true;
            this.txtmfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtmfactory.IsProduceFty = false;
            this.txtmfactory.IssupportJunk = false;
            this.txtmfactory.Location = new System.Drawing.Point(373, 49);
            this.txtmfactory.MDivision = null;
            this.txtmfactory.Name = "txtmfactory";
            this.txtmfactory.Size = new System.Drawing.Size(100, 23);
            this.txtmfactory.TabIndex = 4;
            // 
            // txtuserHandle
            // 
            this.txtuserHandle.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "handle", true));
            this.txtuserHandle.DisplayBox1Binding = "";
            this.txtuserHandle.Location = new System.Drawing.Point(373, 80);
            this.txtuserHandle.Name = "txtuserHandle";
            this.txtuserHandle.Size = new System.Drawing.Size(300, 23);
            this.txtuserHandle.TabIndex = 5;
            this.txtuserHandle.TextBox1Binding = "";
            // 
            // txtartworktype_ftyArtworkType
            // 
            this.txtartworktype_ftyArtworkType.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftyArtworkType.cClassify = "";
            this.txtartworktype_ftyArtworkType.cSubprocess = "Y";
            this.txtartworktype_ftyArtworkType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "artworktypeid", true));
            this.txtartworktype_ftyArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftyArtworkType.Location = new System.Drawing.Point(108, 83);
            this.txtartworktype_ftyArtworkType.Name = "txtartworktype_ftyArtworkType";
            this.txtartworktype_ftyArtworkType.Size = new System.Drawing.Size(165, 23);
            this.txtartworktype_ftyArtworkType.TabIndex = 1;
            this.txtartworktype_ftyArtworkType.Validating += new System.ComponentModel.CancelEventHandler(this.txtartworktype_ftyArtworkType_Validating);
            // 
            // txtsubconSupplier
            // 
            this.txtsubconSupplier.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "localsuppid", true));
            this.txtsubconSupplier.DisplayBox1Binding = "";
            this.txtsubconSupplier.IsIncludeJunk = false;
            this.txtsubconSupplier.isMisc = false;
            this.txtsubconSupplier.isShipping = false;
            this.txtsubconSupplier.isSubcon = false;
            this.txtsubconSupplier.Location = new System.Drawing.Point(108, 49);
            this.txtsubconSupplier.Name = "txtsubconSupplier";
            this.txtsubconSupplier.Size = new System.Drawing.Size(165, 23);
            this.txtsubconSupplier.TabIndex = 0;
            this.txtsubconSupplier.TextBox1Binding = "";
            this.txtsubconSupplier.Validating += new System.ComponentModel.CancelEventHandler(this.txtsubconSupplier_Validating);
            // 
            // btnSpecialRecord
            // 
            this.btnSpecialRecord.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnSpecialRecord.Location = new System.Drawing.Point(858, 42);
            this.btnSpecialRecord.Name = "btnSpecialRecord";
            this.btnSpecialRecord.Size = new System.Drawing.Size(188, 30);
            this.btnSpecialRecord.TabIndex = 20;
            this.btnSpecialRecord.Text = "Special Record";
            this.btnSpecialRecord.UseVisualStyleBackColor = true;
            this.btnSpecialRecord.Click += new System.EventHandler(this.btnSpecialRecord_Click);
            // 
            // P05
            // 
            this.ApvChkValue = "Locked";
            this.CheckChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1069, 618);
            this.Controls.Add(this.btnBatchApprove);
            this.Controls.Add(this.btnBatchCreate);
            this.DefaultControl = "txtsubconSupplier";
            this.DefaultControlForEdit = "txtsubconSupplier";
            this.ExpressQuery = true;
            this.GridAlias = "ArtworkReq_Detail";
            this.GridUniqueKey = "id,artworkid,patterncode,PatternDesc,OrderId,Article,SizeCode";
            this.IsSupportCheck = true;
            this.IsSupportClose = true;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUncheck = true;
            this.IsSupportUnclose = true;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P05";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P05. Sub-con Requisition";
            this.UnApvChkValue = "Approved";
            this.UncheckChkValue = "Locked";
            this.UncloseChkValue = "Closed";
            this.UniqueExpress = "ID";
            this.WorkAlias = "ArtworkReq";
            this.FormLoaded += new System.EventHandler(this.P05_FormLoaded);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.P05_FormClosing);
            this.Controls.SetChildIndex(this.tabs, 0);
            this.Controls.SetChildIndex(this.btnBatchCreate, 0);
            this.Controls.SetChildIndex(this.btnBatchApprove, 0);
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

        private Win.UI.Label labelHandle;
        private Win.UI.Label labelReqDate;
        private Win.UI.Label labelArtworkType;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelPONo;
        private Win.UI.TextBox txtRemark;
        private Class.txtartworktype_fty txtartworktype_ftyArtworkType;
        private Win.UI.DateBox dateReqDate;
        private Class.txtsubconNoConfirm txtsubconSupplier;
        private Win.UI.DisplayBox displayReqID;
        private Win.UI.Button btnBatchImport;
        private Win.UI.Label labExceed;
        private Win.UI.Label labStatus;
        private Class.txtuser txtuserHandle;
        private Win.UI.Button btnBatchCreate;
        private Class.txtfactory txtmfactory;
        private Win.UI.Button btnIrrQtyReason;
        private Win.UI.Button btnBatchApprove;
        private Win.UI.DisplayBox dispClosed;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox dispMgrApv;
        private Win.UI.Label labMGMgrApv;
        private Win.UI.DisplayBox dispDeptApv;
        private Win.UI.Label labDeptMgrApv;
        private Win.UI.Button btnSpecialRecord;
    }
}
