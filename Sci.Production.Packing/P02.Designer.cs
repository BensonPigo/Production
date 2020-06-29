namespace Sci.Production.Packing
{
    partial class P02
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
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelOrderQty = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.displayStyle = new Sci.Win.UI.DisplayBox();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.numOrderQty = new Sci.Win.UI.NumericBox();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.labelSeq = new Sci.Win.UI.Label();
            this.txtSeq = new Sci.Win.UI.TextBox();
            this.labelPONo = new Sci.Win.UI.Label();
            this.labelShippingMode = new Sci.Win.UI.Label();
            this.labelPackingMethod = new Sci.Win.UI.Label();
            this.labelStartCtn = new Sci.Win.UI.Label();
            this.labelTotalCartons = new Sci.Win.UI.Label();
            this.displayPONo = new Sci.Win.UI.DisplayBox();
            this.comboPackingMethod = new Sci.Win.UI.ComboBox();
            this.numStartCtn = new Sci.Win.UI.NumericBox();
            this.numTotalCartons = new Sci.Win.UI.NumericBox();
            this.labelTotalShipQty = new Sci.Win.UI.Label();
            this.numTotalShipQty = new Sci.Win.UI.NumericBox();
            this.btnSpecialInstruction = new Sci.Win.UI.Button();
            this.btnCartonDimension = new Sci.Win.UI.Button();
            this.btnSwitchToPackingList = new Sci.Win.UI.Button();
            this.txtshipmode = new Sci.Production.Class.txtshipmode();
            this.labelFactory = new Sci.Win.UI.Label();
            this.displayFactory = new Sci.Win.UI.DisplayBox();
            this.btnSwitchToPLByArticle = new Sci.Win.UI.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUpdateBalance = new Sci.Win.UI.Button();
            this.txtCartonRefBalance = new Sci.Win.UI.TextBox();
            this.lbUpdateRefNoforBalance = new Sci.Win.UI.Label();
            this.btnUpdate = new Sci.Win.UI.Button();
            this.txtCartonRef = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
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
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.panel1);
            this.masterpanel.Controls.Add(this.btnSwitchToPLByArticle);
            this.masterpanel.Controls.Add(this.displayFactory);
            this.masterpanel.Controls.Add(this.labelFactory);
            this.masterpanel.Controls.Add(this.txtshipmode);
            this.masterpanel.Controls.Add(this.btnSwitchToPackingList);
            this.masterpanel.Controls.Add(this.btnCartonDimension);
            this.masterpanel.Controls.Add(this.btnSpecialInstruction);
            this.masterpanel.Controls.Add(this.numTotalShipQty);
            this.masterpanel.Controls.Add(this.labelTotalShipQty);
            this.masterpanel.Controls.Add(this.numTotalCartons);
            this.masterpanel.Controls.Add(this.numStartCtn);
            this.masterpanel.Controls.Add(this.comboPackingMethod);
            this.masterpanel.Controls.Add(this.displayPONo);
            this.masterpanel.Controls.Add(this.labelTotalCartons);
            this.masterpanel.Controls.Add(this.labelStartCtn);
            this.masterpanel.Controls.Add(this.labelPackingMethod);
            this.masterpanel.Controls.Add(this.labelShippingMode);
            this.masterpanel.Controls.Add(this.labelPONo);
            this.masterpanel.Controls.Add(this.txtSeq);
            this.masterpanel.Controls.Add(this.labelSeq);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.numOrderQty);
            this.masterpanel.Controls.Add(this.displaySeason);
            this.masterpanel.Controls.Add(this.displayStyle);
            this.masterpanel.Controls.Add(this.txtSPNo);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.labelOrderQty);
            this.masterpanel.Controls.Add(this.labelSeason);
            this.masterpanel.Controls.Add(this.labelStyle);
            this.masterpanel.Controls.Add(this.labelSPNo);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Size = new System.Drawing.Size(912, 248);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSPNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelStyle, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSeason, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelOrderQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSPNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayStyle, 0);
            this.masterpanel.Controls.SetChildIndex(this.displaySeason, 0);
            this.masterpanel.Controls.SetChildIndex(this.numOrderQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSeq, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSeq, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPONo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShippingMode, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPackingMethod, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelStartCtn, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTotalCartons, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayPONo, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboPackingMethod, 0);
            this.masterpanel.Controls.SetChildIndex(this.numStartCtn, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTotalCartons, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTotalShipQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTotalShipQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnSpecialInstruction, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnCartonDimension, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnSwitchToPackingList, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtshipmode, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnSwitchToPLByArticle, 0);
            this.masterpanel.Controls.SetChildIndex(this.panel1, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 248);
            this.detailpanel.Size = new System.Drawing.Size(912, 202);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(777, 213);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(827, 1);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(912, 202);
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
            this.detail.Size = new System.Drawing.Size(912, 488);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(912, 450);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 450);
            this.detailbtm.Size = new System.Drawing.Size(912, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(912, 488);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(920, 517);
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
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(5, 12);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 1;
            this.labelID.Text = "ID";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(5, 39);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(75, 23);
            this.labelSPNo.TabIndex = 2;
            this.labelSPNo.Text = "SP No.";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(5, 66);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(75, 23);
            this.labelStyle.TabIndex = 3;
            this.labelStyle.Text = "Style";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(5, 93);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(75, 23);
            this.labelSeason.TabIndex = 4;
            this.labelSeason.Text = "Season";
            // 
            // labelOrderQty
            // 
            this.labelOrderQty.Location = new System.Drawing.Point(5, 120);
            this.labelOrderQty.Name = "labelOrderQty";
            this.labelOrderQty.Size = new System.Drawing.Size(75, 23);
            this.labelOrderQty.TabIndex = 5;
            this.labelOrderQty.Text = "Order Q\'ty";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(5, 147);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 6;
            this.labelRemark.Text = "Remark";
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(84, 12);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(120, 23);
            this.displayID.TabIndex = 7;
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OrderID", true));
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(84, 39);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(120, 23);
            this.txtSPNo.TabIndex = 0;
            this.txtSPNo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSPNo_Validating);
            this.txtSPNo.Validated += new System.EventHandler(this.TxtSPNo_Validated);
            // 
            // displayStyle
            // 
            this.displayStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyle.Location = new System.Drawing.Point(84, 66);
            this.displayStyle.Name = "displayStyle";
            this.displayStyle.Size = new System.Drawing.Size(135, 23);
            this.displayStyle.TabIndex = 9;
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(84, 93);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(80, 23);
            this.displaySeason.TabIndex = 10;
            // 
            // numOrderQty
            // 
            this.numOrderQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numOrderQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numOrderQty.IsSupportEditMode = false;
            this.numOrderQty.Location = new System.Drawing.Point(84, 120);
            this.numOrderQty.Name = "numOrderQty";
            this.numOrderQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numOrderQty.ReadOnly = true;
            this.numOrderQty.Size = new System.Drawing.Size(65, 23);
            this.numOrderQty.TabIndex = 11;
            this.numOrderQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(84, 147);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(624, 50);
            this.editRemark.TabIndex = 4;
            // 
            // labelSeq
            // 
            this.labelSeq.Location = new System.Drawing.Point(221, 38);
            this.labelSeq.Name = "labelSeq";
            this.labelSeq.Size = new System.Drawing.Size(33, 24);
            this.labelSeq.TabIndex = 13;
            this.labelSeq.Text = "Seq";
            // 
            // txtSeq
            // 
            this.txtSeq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSeq.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OrderShipmodeSeq", true));
            this.txtSeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSeq.IsSupportEditMode = false;
            this.txtSeq.Location = new System.Drawing.Point(258, 39);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtSeq.ReadOnly = true;
            this.txtSeq.Size = new System.Drawing.Size(28, 23);
            this.txtSeq.TabIndex = 1;
            this.txtSeq.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSeq_PopUp);
            // 
            // labelPONo
            // 
            this.labelPONo.Location = new System.Drawing.Point(372, 11);
            this.labelPONo.Name = "labelPONo";
            this.labelPONo.Size = new System.Drawing.Size(107, 23);
            this.labelPONo.TabIndex = 15;
            this.labelPONo.Text = "P.O. No.";
            // 
            // labelShippingMode
            // 
            this.labelShippingMode.Location = new System.Drawing.Point(372, 38);
            this.labelShippingMode.Name = "labelShippingMode";
            this.labelShippingMode.Size = new System.Drawing.Size(107, 23);
            this.labelShippingMode.TabIndex = 16;
            this.labelShippingMode.Text = "Shipping Mode";
            // 
            // labelPackingMethod
            // 
            this.labelPackingMethod.Location = new System.Drawing.Point(372, 65);
            this.labelPackingMethod.Name = "labelPackingMethod";
            this.labelPackingMethod.Size = new System.Drawing.Size(107, 23);
            this.labelPackingMethod.TabIndex = 17;
            this.labelPackingMethod.Text = "Packing Method";
            // 
            // labelStartCtn
            // 
            this.labelStartCtn.Location = new System.Drawing.Point(372, 92);
            this.labelStartCtn.Name = "labelStartCtn";
            this.labelStartCtn.Size = new System.Drawing.Size(107, 23);
            this.labelStartCtn.TabIndex = 18;
            this.labelStartCtn.Text = "Start Ctn#";
            // 
            // labelTotalCartons
            // 
            this.labelTotalCartons.Location = new System.Drawing.Point(372, 119);
            this.labelTotalCartons.Name = "labelTotalCartons";
            this.labelTotalCartons.Size = new System.Drawing.Size(107, 23);
            this.labelTotalCartons.TabIndex = 19;
            this.labelTotalCartons.Text = "Total Cartons";
            // 
            // displayPONo
            // 
            this.displayPONo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPONo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPONo.Location = new System.Drawing.Point(483, 11);
            this.displayPONo.Name = "displayPONo";
            this.displayPONo.Size = new System.Drawing.Size(200, 23);
            this.displayPONo.TabIndex = 20;
            // 
            // comboPackingMethod
            // 
            this.comboPackingMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboPackingMethod.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboPackingMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboPackingMethod.FormattingEnabled = true;
            this.comboPackingMethod.IsSupportUnselect = true;
            this.comboPackingMethod.Location = new System.Drawing.Point(483, 64);
            this.comboPackingMethod.Name = "comboPackingMethod";
            this.comboPackingMethod.OldText = "";
            this.comboPackingMethod.ReadOnly = true;
            this.comboPackingMethod.Size = new System.Drawing.Size(216, 24);
            this.comboPackingMethod.TabIndex = 22;
            this.comboPackingMethod.SelectionChangeCommitted += new System.EventHandler(this.ComboPackingMethod_SelectionChangeCommitted);
            // 
            // numStartCtn
            // 
            this.numStartCtn.BackColor = System.Drawing.Color.White;
            this.numStartCtn.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CTNStartNo", true));
            this.numStartCtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numStartCtn.Location = new System.Drawing.Point(483, 93);
            this.numStartCtn.Name = "numStartCtn";
            this.numStartCtn.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numStartCtn.Size = new System.Drawing.Size(65, 23);
            this.numStartCtn.TabIndex = 3;
            this.numStartCtn.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numTotalCartons
            // 
            this.numTotalCartons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalCartons.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CTNQty", true));
            this.numTotalCartons.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalCartons.IsSupportEditMode = false;
            this.numTotalCartons.Location = new System.Drawing.Point(483, 120);
            this.numTotalCartons.Name = "numTotalCartons";
            this.numTotalCartons.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalCartons.ReadOnly = true;
            this.numTotalCartons.Size = new System.Drawing.Size(65, 23);
            this.numTotalCartons.TabIndex = 24;
            this.numTotalCartons.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelTotalShipQty
            // 
            this.labelTotalShipQty.Location = new System.Drawing.Point(561, 93);
            this.labelTotalShipQty.Name = "labelTotalShipQty";
            this.labelTotalShipQty.Size = new System.Drawing.Size(96, 23);
            this.labelTotalShipQty.TabIndex = 25;
            this.labelTotalShipQty.Text = "Total Ship Q\'ty";
            // 
            // numTotalShipQty
            // 
            this.numTotalShipQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalShipQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalShipQty.IsSupportEditMode = false;
            this.numTotalShipQty.Location = new System.Drawing.Point(661, 93);
            this.numTotalShipQty.Name = "numTotalShipQty";
            this.numTotalShipQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalShipQty.ReadOnly = true;
            this.numTotalShipQty.Size = new System.Drawing.Size(65, 23);
            this.numTotalShipQty.TabIndex = 26;
            this.numTotalShipQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // btnSpecialInstruction
            // 
            this.btnSpecialInstruction.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnSpecialInstruction.Location = new System.Drawing.Point(742, 11);
            this.btnSpecialInstruction.Name = "btnSpecialInstruction";
            this.btnSpecialInstruction.Size = new System.Drawing.Size(143, 30);
            this.btnSpecialInstruction.TabIndex = 5;
            this.btnSpecialInstruction.Text = "Special Instruction";
            this.btnSpecialInstruction.UseVisualStyleBackColor = true;
            this.btnSpecialInstruction.Click += new System.EventHandler(this.BtnSpecialInstruction_Click);
            // 
            // btnCartonDimension
            // 
            this.btnCartonDimension.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnCartonDimension.Location = new System.Drawing.Point(742, 47);
            this.btnCartonDimension.Name = "btnCartonDimension";
            this.btnCartonDimension.Size = new System.Drawing.Size(143, 30);
            this.btnCartonDimension.TabIndex = 6;
            this.btnCartonDimension.Text = "Carton Dimension";
            this.btnCartonDimension.UseVisualStyleBackColor = true;
            this.btnCartonDimension.Click += new System.EventHandler(this.BtnCartonDimension_Click);
            // 
            // btnSwitchToPackingList
            // 
            this.btnSwitchToPackingList.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnSwitchToPackingList.Location = new System.Drawing.Point(726, 141);
            this.btnSwitchToPackingList.Name = "btnSwitchToPackingList";
            this.btnSwitchToPackingList.Size = new System.Drawing.Size(160, 30);
            this.btnSwitchToPackingList.TabIndex = 7;
            this.btnSwitchToPackingList.Text = "Switch to Packing list";
            this.btnSwitchToPackingList.UseVisualStyleBackColor = true;
            this.btnSwitchToPackingList.Click += new System.EventHandler(this.BtnSwitchToPackingList_Click);
            // 
            // txtshipmode
            // 
            this.txtshipmode.BackColor = System.Drawing.Color.White;
            this.txtshipmode.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "ShipModeID", true));
            this.txtshipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshipmode.FormattingEnabled = true;
            this.txtshipmode.IsSupportUnselect = true;
            this.txtshipmode.Location = new System.Drawing.Point(483, 37);
            this.txtshipmode.Name = "txtshipmode";
            this.txtshipmode.OldText = "";
            this.txtshipmode.Size = new System.Drawing.Size(121, 24);
            this.txtshipmode.TabIndex = 2;
            this.txtshipmode.UseFunction = "ORDER";
            this.txtshipmode.SelectionChangeCommitted += new System.EventHandler(this.Txtshipmode_SelectionChangeCommitted);
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(221, 11);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(51, 23);
            this.labelFactory.TabIndex = 27;
            this.labelFactory.Text = "Factory";
            // 
            // displayFactory
            // 
            this.displayFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFactory.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FactoryID", true));
            this.displayFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFactory.Location = new System.Drawing.Point(276, 11);
            this.displayFactory.Name = "displayFactory";
            this.displayFactory.Size = new System.Drawing.Size(63, 23);
            this.displayFactory.TabIndex = 28;
            // 
            // btnSwitchToPLByArticle
            // 
            this.btnSwitchToPLByArticle.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnSwitchToPLByArticle.Location = new System.Drawing.Point(726, 174);
            this.btnSwitchToPLByArticle.Name = "btnSwitchToPLByArticle";
            this.btnSwitchToPLByArticle.Size = new System.Drawing.Size(160, 30);
            this.btnSwitchToPLByArticle.TabIndex = 29;
            this.btnSwitchToPLByArticle.Text = "Switch to PL by Article";
            this.btnSwitchToPLByArticle.UseVisualStyleBackColor = true;
            this.btnSwitchToPLByArticle.Click += new System.EventHandler(this.BtnSwitchToPackingList_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnUpdateBalance);
            this.panel1.Controls.Add(this.txtCartonRefBalance);
            this.panel1.Controls.Add(this.lbUpdateRefNoforBalance);
            this.panel1.Controls.Add(this.btnUpdate);
            this.panel1.Controls.Add(this.txtCartonRef);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 210);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(912, 38);
            this.panel1.TabIndex = 30;
            // 
            // btnUpdateBalance
            // 
            this.btnUpdateBalance.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnUpdateBalance.Location = new System.Drawing.Point(744, 3);
            this.btnUpdateBalance.Name = "btnUpdateBalance";
            this.btnUpdateBalance.Size = new System.Drawing.Size(80, 30);
            this.btnUpdateBalance.TabIndex = 36;
            this.btnUpdateBalance.Text = "Update";
            this.btnUpdateBalance.UseVisualStyleBackColor = true;
            this.btnUpdateBalance.Click += new System.EventHandler(this.BtnUpdateBalance_Click);
            // 
            // txtCartonRefBalance
            // 
            this.txtCartonRefBalance.BackColor = System.Drawing.Color.White;
            this.txtCartonRefBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCartonRefBalance.Location = new System.Drawing.Point(561, 7);
            this.txtCartonRefBalance.Name = "txtCartonRefBalance";
            this.txtCartonRefBalance.Size = new System.Drawing.Size(177, 23);
            this.txtCartonRefBalance.TabIndex = 35;
            this.txtCartonRefBalance.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtCartonRefBalance_PopUp);
            this.txtCartonRefBalance.Validating += new System.ComponentModel.CancelEventHandler(this.TxtCartonRefBalance_Validating);
            // 
            // lbUpdateRefNoforBalance
            // 
            this.lbUpdateRefNoforBalance.Location = new System.Drawing.Point(383, 7);
            this.lbUpdateRefNoforBalance.Name = "lbUpdateRefNoforBalance";
            this.lbUpdateRefNoforBalance.Size = new System.Drawing.Size(175, 23);
            this.lbUpdateRefNoforBalance.TabIndex = 34;
            this.lbUpdateRefNoforBalance.Text = "Update Ref No. for Balance";
            // 
            // btnUpdate
            // 
            this.btnUpdate.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnUpdate.Location = new System.Drawing.Point(291, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(80, 30);
            this.btnUpdate.TabIndex = 33;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // txtCartonRef
            // 
            this.txtCartonRef.BackColor = System.Drawing.Color.White;
            this.txtCartonRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCartonRef.Location = new System.Drawing.Point(108, 7);
            this.txtCartonRef.Name = "txtCartonRef";
            this.txtCartonRef.Size = new System.Drawing.Size(177, 23);
            this.txtCartonRef.TabIndex = 32;
            this.txtCartonRef.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtCartonRef_PopUp);
            this.txtCartonRef.Validating += new System.ComponentModel.CancelEventHandler(this.TxtCartonRef_Validating);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 23);
            this.label1.TabIndex = 31;
            this.label1.Text = "Carton Ref No.";
            // 
            // P02
            // 
            this.ClientSize = new System.Drawing.Size(920, 550);
            this.DefaultControl = "txtSPNo";
            this.DefaultControlForEdit = "txtSPNo";
            this.DefaultOrder = "OrderID";
            this.GridAlias = "PackingGuide_Detail";
            this.GridNew = 0;
            this.GridUniqueKey = "ID,Article,SizeCode";
            this.IsGridIconVisible = false;
            this.IsSupportCopy = false;
            this.KeyField1 = "ID";
            this.Name = "P02";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P02. Packing Guide";
            this.UniqueExpress = "ID";
            this.WorkAlias = "PackingGuide";
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Class.txtshipmode txtshipmode;
        private Win.UI.Button btnSwitchToPackingList;
        private Win.UI.Button btnCartonDimension;
        private Win.UI.Button btnSpecialInstruction;
        private Win.UI.NumericBox numTotalShipQty;
        private Win.UI.Label labelTotalShipQty;
        private Win.UI.NumericBox numTotalCartons;
        private Win.UI.NumericBox numStartCtn;
        private Win.UI.ComboBox comboPackingMethod;
        private Win.UI.DisplayBox displayPONo;
        private Win.UI.Label labelTotalCartons;
        private Win.UI.Label labelStartCtn;
        private Win.UI.Label labelPackingMethod;
        private Win.UI.Label labelShippingMode;
        private Win.UI.Label labelPONo;
        private Win.UI.TextBox txtSeq;
        private Win.UI.Label labelSeq;
        private Win.UI.EditBox editRemark;
        private Win.UI.NumericBox numOrderQty;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.DisplayBox displayStyle;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelOrderQty;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelStyle;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelID;
        private Win.UI.DisplayBox displayFactory;
        private Win.UI.Label labelFactory;
        private Win.UI.Button btnSwitchToPLByArticle;
        private System.Windows.Forms.Panel panel1;
        private Win.UI.Button btnUpdate;
        private Win.UI.TextBox txtCartonRef;
        private Win.UI.Label label1;
        private Win.UI.Button btnUpdateBalance;
        private Win.UI.TextBox txtCartonRefBalance;
        private Win.UI.Label lbUpdateRefNoforBalance;
    }
}
