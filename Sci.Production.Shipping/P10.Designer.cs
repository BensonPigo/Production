namespace Sci.Production.Shipping
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
            this.components = new System.ComponentModel.Container();
            this.labelID = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.btnUpdatePulloutDate = new Sci.Win.UI.Button();
            this.btnImportData = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.gridDetail = new Sci.Win.UI.Grid();
            this.labelDetail = new Sci.Win.UI.Label();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.labelTTLQty = new Sci.Win.UI.Label();
            this.labelTTLCTN = new Sci.Win.UI.Label();
            this.numericBoxTTLQTY = new Sci.Win.UI.NumericBox();
            this.numericBoxTTLCTN = new Sci.Win.UI.NumericBox();
            this.btnContainerTruck = new Sci.Win.UI.Button();
            this.displayTTLContainer = new Sci.Win.UI.DisplayBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.displayCFSCBM = new Sci.Win.UI.DisplayBox();
            this.displayAIRGW = new Sci.Win.UI.DisplayBox();
            this.displayTTLGW = new Sci.Win.UI.DisplayBox();
            this.displayTTLCBM = new Sci.Win.UI.DisplayBox();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // detailgridbs
            // 
            this.detailgridbs.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.Detailgridbs_ListChanged);
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.displayTTLGW);
            this.masterpanel.Controls.Add(this.displayTTLCBM);
            this.masterpanel.Controls.Add(this.label4);
            this.masterpanel.Controls.Add(this.label5);
            this.masterpanel.Controls.Add(this.displayAIRGW);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Controls.Add(this.displayCFSCBM);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Controls.Add(this.displayTTLContainer);
            this.masterpanel.Controls.Add(this.btnContainerTruck);
            this.masterpanel.Controls.Add(this.btnImportData);
            this.masterpanel.Controls.Add(this.numericBoxTTLCTN);
            this.masterpanel.Controls.Add(this.numericBoxTTLQTY);
            this.masterpanel.Controls.Add(this.labelTTLCTN);
            this.masterpanel.Controls.Add(this.labelTTLQty);
            this.masterpanel.Controls.Add(this.btnUpdatePulloutDate);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Size = new System.Drawing.Size(888, 177);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnUpdatePulloutDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTTLQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTTLCTN, 0);
            this.masterpanel.Controls.SetChildIndex(this.numericBoxTTLQTY, 0);
            this.masterpanel.Controls.SetChildIndex(this.numericBoxTTLCTN, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportData, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnContainerTruck, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayTTLContainer, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayCFSCBM, 0);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayAIRGW, 0);
            this.masterpanel.Controls.SetChildIndex(this.label5, 0);
            this.masterpanel.Controls.SetChildIndex(this.label4, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayTTLCBM, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayTTLGW, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Controls.Add(this.panel4);
            this.detailpanel.Controls.Add(this.panel1);
            this.detailpanel.Location = new System.Drawing.Point(0, 177);
            this.detailpanel.Size = new System.Drawing.Size(888, 309);
            this.detailpanel.Controls.SetChildIndex(this.panel1, 0);
            this.detailpanel.Controls.SetChildIndex(this.panel4, 0);
            this.detailpanel.Controls.SetChildIndex(this.detailgridcont, 0);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(767, 142);
            this.gridicon.TabIndex = 1;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(827, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(888, 132);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(913, 524);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(907, 478);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(907, 40);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(888, 524);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(888, 486);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 486);
            this.detailbtm.Size = new System.Drawing.Size(888, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(913, 524);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(896, 553);
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
            this.labelID.Location = new System.Drawing.Point(4, 4);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(55, 23);
            this.labelID.TabIndex = 1;
            this.labelID.Text = "ID";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(4, 31);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(55, 23);
            this.labelRemark.TabIndex = 2;
            this.labelRemark.Text = "Remark";
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(63, 4);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(120, 23);
            this.displayID.TabIndex = 3;
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(63, 31);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(445, 50);
            this.editRemark.TabIndex = 2;
            // 
            // btnUpdatePulloutDate
            // 
            this.btnUpdatePulloutDate.Enabled = false;
            this.btnUpdatePulloutDate.Location = new System.Drawing.Point(707, 4);
            this.btnUpdatePulloutDate.Name = "btnUpdatePulloutDate";
            this.btnUpdatePulloutDate.Size = new System.Drawing.Size(160, 30);
            this.btnUpdatePulloutDate.TabIndex = 3;
            this.btnUpdatePulloutDate.Text = "Update Pullout Date";
            this.btnUpdatePulloutDate.UseVisualStyleBackColor = true;
            this.btnUpdatePulloutDate.Click += new System.EventHandler(this.BtnUpdatePulloutDate_Click);
            // 
            // btnImportData
            // 
            this.btnImportData.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImportData.Location = new System.Drawing.Point(707, 74);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(116, 30);
            this.btnImportData.TabIndex = 0;
            this.btnImportData.Text = "Import Data";
            this.btnImportData.UseVisualStyleBackColor = true;
            this.btnImportData.Click += new System.EventHandler(this.BtnImportData_Click);
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.DataSource = this.listControlBindingSource1;
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(0, 0);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowHeadersVisible = false;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(888, 145);
            this.gridDetail.TabIndex = 2;
            this.gridDetail.TabStop = false;
            // 
            // labelDetail
            // 
            this.labelDetail.Location = new System.Drawing.Point(4, 5);
            this.labelDetail.Name = "labelDetail";
            this.labelDetail.Size = new System.Drawing.Size(42, 23);
            this.labelDetail.TabIndex = 3;
            this.labelDetail.Text = "Detail";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 132);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(888, 177);
            this.panel1.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gridDetail);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 32);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(888, 145);
            this.panel3.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelDetail);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(888, 32);
            this.panel2.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(888, 132);
            this.panel4.TabIndex = 5;
            // 
            // labelTTLQty
            // 
            this.labelTTLQty.Location = new System.Drawing.Point(516, 4);
            this.labelTTLQty.Name = "labelTTLQty";
            this.labelTTLQty.Size = new System.Drawing.Size(73, 23);
            this.labelTTLQty.TabIndex = 4;
            this.labelTTLQty.Text = "TTL Qty";
            // 
            // labelTTLCTN
            // 
            this.labelTTLCTN.Location = new System.Drawing.Point(516, 31);
            this.labelTTLCTN.Name = "labelTTLCTN";
            this.labelTTLCTN.Size = new System.Drawing.Size(73, 23);
            this.labelTTLCTN.TabIndex = 5;
            this.labelTTLCTN.Text = "TTL CTN";
            // 
            // numericBoxTTLQTY
            // 
            this.numericBoxTTLQTY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numericBoxTTLQTY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numericBoxTTLQTY.IsSupportEditMode = false;
            this.numericBoxTTLQTY.Location = new System.Drawing.Point(592, 4);
            this.numericBoxTTLQTY.Name = "numericBoxTTLQTY";
            this.numericBoxTTLQTY.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBoxTTLQTY.ReadOnly = true;
            this.numericBoxTTLQTY.Size = new System.Drawing.Size(109, 23);
            this.numericBoxTTLQTY.TabIndex = 6;
            this.numericBoxTTLQTY.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numericBoxTTLCTN
            // 
            this.numericBoxTTLCTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numericBoxTTLCTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numericBoxTTLCTN.IsSupportEditMode = false;
            this.numericBoxTTLCTN.Location = new System.Drawing.Point(592, 31);
            this.numericBoxTTLCTN.Name = "numericBoxTTLCTN";
            this.numericBoxTTLCTN.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBoxTTLCTN.ReadOnly = true;
            this.numericBoxTTLCTN.Size = new System.Drawing.Size(109, 23);
            this.numericBoxTTLCTN.TabIndex = 7;
            this.numericBoxTTLCTN.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // btnContainerTruck
            // 
            this.btnContainerTruck.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnContainerTruck.Location = new System.Drawing.Point(707, 39);
            this.btnContainerTruck.Name = "btnContainerTruck";
            this.btnContainerTruck.Size = new System.Drawing.Size(131, 30);
            this.btnContainerTruck.TabIndex = 67;
            this.btnContainerTruck.Text = "Container/Truck";
            this.btnContainerTruck.UseVisualStyleBackColor = true;
            this.btnContainerTruck.Click += new System.EventHandler(this.BtnContainerTruck_Click);
            // 
            // displayTTLContainer
            // 
            this.displayTTLContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTTLContainer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTTLContainer.Location = new System.Drawing.Point(109, 89);
            this.displayTTLContainer.Name = "displayTTLContainer";
            this.displayTTLContainer.Size = new System.Drawing.Size(399, 23);
            this.displayTTLContainer.TabIndex = 70;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(516, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 23);
            this.label1.TabIndex = 71;
            this.label1.Text = "CFS CBM";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(516, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 23);
            this.label2.TabIndex = 72;
            this.label2.Text = "AIR G.W.";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(5, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 23);
            this.label3.TabIndex = 73;
            this.label3.Text = "Total Container";
            // 
            // displayCFSCBM
            // 
            this.displayCFSCBM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCFSCBM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCFSCBM.Location = new System.Drawing.Point(592, 60);
            this.displayCFSCBM.Name = "displayCFSCBM";
            this.displayCFSCBM.Size = new System.Drawing.Size(109, 23);
            this.displayCFSCBM.TabIndex = 1;
            // 
            // displayAIRGW
            // 
            this.displayAIRGW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayAIRGW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayAIRGW.Location = new System.Drawing.Point(592, 89);
            this.displayAIRGW.Name = "displayAIRGW";
            this.displayAIRGW.Size = new System.Drawing.Size(109, 23);
            this.displayAIRGW.TabIndex = 2;
            // 
            // displayTTLGW
            // 
            this.displayTTLGW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTTLGW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTTLGW.Location = new System.Drawing.Point(592, 147);
            this.displayTTLGW.Name = "displayTTLGW";
            this.displayTTLGW.Size = new System.Drawing.Size(109, 23);
            this.displayTTLGW.TabIndex = 75;
            // 
            // displayTTLCBM
            // 
            this.displayTTLCBM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTTLCBM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTTLCBM.Location = new System.Drawing.Point(592, 118);
            this.displayTTLCBM.Name = "displayTTLCBM";
            this.displayTTLCBM.Size = new System.Drawing.Size(109, 23);
            this.displayTTLCBM.TabIndex = 74;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(516, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 23);
            this.label4.TabIndex = 77;
            this.label4.Text = "TTL G.W.";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(516, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 23);
            this.label5.TabIndex = 76;
            this.label5.Text = "TTL CBM";
            // 
            // P10
            // 
            this.ApvChkValue = "Checked";
            this.CheckChkValue = "New";
            this.ClientSize = new System.Drawing.Size(896, 586);
            this.DefaultOrder = "ID";
            this.GridAlias = "GMTBooking";
            this.GridNew = 0;
            this.IsSupportCheck = true;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUncheck = true;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.KeyField2 = "ShipPlanID";
            this.Name = "P10";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P10. Ship Plan";
            this.UnApvChkValue = "Confirmed";
            this.UncheckChkValue = "Checked";
            this.UniqueExpress = "ID";
            this.WorkAlias = "ShipPlan";
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
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button btnImportData;
        private Win.UI.Button btnUpdatePulloutDate;
        private Win.UI.EditBox editRemark;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelID;
        private Win.UI.Label labelDetail;
        private Win.UI.Grid gridDetail;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel1;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel2;
        private Win.UI.Label labelTTLCTN;
        private Win.UI.Label labelTTLQty;
        private Win.UI.NumericBox numericBoxTTLCTN;
        private Win.UI.NumericBox numericBoxTTLQTY;
        private Win.UI.Button btnContainerTruck;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox displayTTLContainer;
        private Win.UI.DisplayBox displayAIRGW;
        private Win.UI.DisplayBox displayCFSCBM;
        private Win.UI.DisplayBox displayTTLGW;
        private Win.UI.DisplayBox displayTTLCBM;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
    }
}
