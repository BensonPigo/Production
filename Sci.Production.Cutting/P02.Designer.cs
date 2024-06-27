namespace Sci.Production.Cutting
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new Sci.Win.UI.Panel();
            this.numCons = new Sci.Win.UI.NumericBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label6 = new System.Windows.Forms.Label();
            this.gridSizeRatio = new Sci.Win.UI.Grid();
            this.sizeratioMenuStrip = new Sci.Win.UI.ContextMenuStrip();
            this.insertSizeRatioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSizeRatioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sizeRatioBindingSource = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label11 = new System.Windows.Forms.Label();
            this.gridOrderList = new Sci.Win.UI.Grid();
            this.orderListBindingSource = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.numUnitCons = new Sci.Win.UI.NumericBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.disDescription = new Sci.Win.UI.DisplayBox();
            this.disFabricTypeRefno = new Sci.Win.UI.DisplayBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numBalanceLayer = new Sci.Win.UI.NumericBox();
            this.numTotalLayer = new Sci.Win.UI.NumericBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.displayBoxStyle = new Sci.Win.UI.DisplayBox();
            this.label1 = new Sci.Win.UI.Label();
            this.displayBoxSP = new Sci.Win.UI.DisplayBox();
            this.labelCutplanID = new Sci.Win.UI.Label();
            this.btnBatchAssign = new Sci.Win.UI.Button();
            this.btnImportMarker = new Sci.Win.UI.Button();
            this.btnDownload = new Sci.Win.UI.Button();
            this.btnImportMarkerLectra = new Sci.Win.UI.Button();
            this.btnEdit = new Sci.Win.UI.Button();
            this.btnAutoRef = new Sci.Win.UI.Button();
            this.btnCutPartsCheck = new Sci.Win.UI.Button();
            this.btnCutPartsCheckSummary = new Sci.Win.UI.Button();
            this.btnQtyBreakdown = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.qtybreakBindingSource = new Sci.Win.UI.ListControlBindingSource(this.components);
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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSizeRatio)).BeginInit();
            this.sizeratioMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeRatioBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridOrderList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderListBindingSource)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qtybreakBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.btnEdit);
            this.masterpanel.Controls.Add(this.btnImportMarkerLectra);
            this.masterpanel.Controls.Add(this.btnDownload);
            this.masterpanel.Controls.Add(this.btnImportMarker);
            this.masterpanel.Controls.Add(this.btnBatchAssign);
            this.masterpanel.Controls.Add(this.panel2);
            this.masterpanel.Size = new System.Drawing.Size(548, 65);
            this.masterpanel.Controls.SetChildIndex(this.panel2, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnBatchAssign, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportMarker, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnDownload, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportMarkerLectra, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnEdit, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 65);
            this.detailpanel.Size = new System.Drawing.Size(548, 281);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.gridicon.Dock = System.Windows.Forms.DockStyle.Right;
            this.gridicon.Location = new System.Drawing.Point(448, 35);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(1197, 3);
            this.refresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(548, 281);
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
            this.detail.Controls.Add(this.panel1);
            this.detail.Controls.SetChildIndex(this.detailbtm, 0);
            this.detail.Controls.SetChildIndex(this.panel1, 0);
            this.detail.Controls.SetChildIndex(this.detailcont, 0);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(548, 346);
            // 
            // detailbtm
            // 
            this.detailbtm.Controls.Add(this.btnToExcel);
            this.detailbtm.Controls.Add(this.btnQtyBreakdown);
            this.detailbtm.Controls.Add(this.btnCutPartsCheckSummary);
            this.detailbtm.Controls.Add(this.btnCutPartsCheck);
            this.detailbtm.Controls.Add(this.btnAutoRef);
            this.detailbtm.Location = new System.Drawing.Point(0, 346);
            this.detailbtm.Size = new System.Drawing.Size(892, 41);
            this.detailbtm.Controls.SetChildIndex(this.lbleditby, 0);
            this.detailbtm.Controls.SetChildIndex(this.lblcreateby, 0);
            this.detailbtm.Controls.SetChildIndex(this.editby, 0);
            this.detailbtm.Controls.SetChildIndex(this.createby, 0);
            this.detailbtm.Controls.SetChildIndex(this.refresh, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnAutoRef, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnCutPartsCheck, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnCutPartsCheckSummary, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnQtyBreakdown, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnToExcel, 0);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1280, 655);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1288, 684);
            // 
            // createby
            // 
            this.createby.Location = new System.Drawing.Point(70, 10);
            this.createby.Size = new System.Drawing.Size(350, 23);
            this.createby.Visible = false;
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 10);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.Visible = false;
            // 
            // lblcreateby
            // 
            this.lblcreateby.Location = new System.Drawing.Point(5, 16);
            this.lblcreateby.Visible = false;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 16);
            this.lbleditby.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.numCons);
            this.panel1.Controls.Add(this.splitContainer2);
            this.panel1.Controls.Add(this.numUnitCons);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.disDescription);
            this.panel1.Controls.Add(this.disFabricTypeRefno);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(548, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(344, 346);
            this.panel1.TabIndex = 4;
            // 
            // numCons
            // 
            this.numCons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numCons.DecimalPlaces = 4;
            this.numCons.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numCons.IsSupportEditMode = false;
            this.numCons.Location = new System.Drawing.Point(240, 103);
            this.numCons.Name = "numCons";
            this.numCons.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCons.ReadOnly = true;
            this.numCons.Size = new System.Drawing.Size(98, 23);
            this.numCons.TabIndex = 35;
            this.numCons.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // splitContainer2
            // 
            this.splitContainer2.Location = new System.Drawing.Point(6, 146);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label6);
            this.splitContainer2.Panel1.Controls.Add(this.gridSizeRatio);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.label11);
            this.splitContainer2.Panel2.Controls.Add(this.gridOrderList);
            this.splitContainer2.Size = new System.Drawing.Size(335, 462);
            this.splitContainer2.SplitterDistance = 181;
            this.splitContainer2.TabIndex = 34;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 17);
            this.label6.TabIndex = 33;
            this.label6.Text = "Size Ratio";
            // 
            // gridSizeRatio
            // 
            this.gridSizeRatio.AllowUserToAddRows = false;
            this.gridSizeRatio.AllowUserToDeleteRows = false;
            this.gridSizeRatio.AllowUserToResizeRows = false;
            this.gridSizeRatio.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSizeRatio.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSizeRatio.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSizeRatio.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSizeRatio.ContextMenuStrip = this.sizeratioMenuStrip;
            this.gridSizeRatio.DataSource = this.sizeRatioBindingSource;
            this.gridSizeRatio.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSizeRatio.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSizeRatio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSizeRatio.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSizeRatio.Location = new System.Drawing.Point(1, 26);
            this.gridSizeRatio.Name = "gridSizeRatio";
            this.gridSizeRatio.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSizeRatio.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSizeRatio.RowTemplate.Height = 24;
            this.gridSizeRatio.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSizeRatio.ShowCellToolTips = false;
            this.gridSizeRatio.Size = new System.Drawing.Size(332, 148);
            this.gridSizeRatio.TabIndex = 34;
            // 
            // sizeratioMenuStrip
            // 
            this.sizeratioMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertSizeRatioToolStripMenuItem,
            this.deleteSizeRatioToolStripMenuItem});
            this.sizeratioMenuStrip.Name = "sizeratioMenuStrip";
            this.sizeratioMenuStrip.Size = new System.Drawing.Size(164, 48);
            // 
            // insertSizeRatioToolStripMenuItem
            // 
            this.insertSizeRatioToolStripMenuItem.Name = "insertSizeRatioToolStripMenuItem";
            this.insertSizeRatioToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.insertSizeRatioToolStripMenuItem.Text = "Insert Size Ratio";
            this.insertSizeRatioToolStripMenuItem.Click += new System.EventHandler(this.InsertSizeRatioToolStripMenuItem_Click);
            // 
            // deleteSizeRatioToolStripMenuItem
            // 
            this.deleteSizeRatioToolStripMenuItem.Name = "deleteSizeRatioToolStripMenuItem";
            this.deleteSizeRatioToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.deleteSizeRatioToolStripMenuItem.Text = "Delete Record";
            this.deleteSizeRatioToolStripMenuItem.Click += new System.EventHandler(this.DeleteSizeRatioToolStripMenuItem_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 6);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 17);
            this.label11.TabIndex = 31;
            this.label11.Text = "Order List";
            // 
            // gridOrderList
            // 
            this.gridOrderList.AllowUserToAddRows = false;
            this.gridOrderList.AllowUserToDeleteRows = false;
            this.gridOrderList.AllowUserToResizeRows = false;
            this.gridOrderList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridOrderList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridOrderList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridOrderList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridOrderList.DataSource = this.orderListBindingSource;
            this.gridOrderList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridOrderList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridOrderList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridOrderList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridOrderList.Location = new System.Drawing.Point(0, 26);
            this.gridOrderList.Name = "gridOrderList";
            this.gridOrderList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridOrderList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridOrderList.RowTemplate.Height = 24;
            this.gridOrderList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridOrderList.ShowCellToolTips = false;
            this.gridOrderList.Size = new System.Drawing.Size(332, 248);
            this.gridOrderList.TabIndex = 32;
            // 
            // numUnitCons
            // 
            this.numUnitCons.BackColor = System.Drawing.Color.White;
            this.numUnitCons.DecimalPlaces = 4;
            this.numUnitCons.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numUnitCons.Location = new System.Drawing.Point(129, 102);
            this.numUnitCons.Name = "numUnitCons";
            this.numUnitCons.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numUnitCons.Size = new System.Drawing.Size(94, 23);
            this.numUnitCons.TabIndex = 33;
            this.numUnitCons.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(227, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 17);
            this.label5.TabIndex = 17;
            this.label5.Text = "/";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(3, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 23);
            this.label4.TabIndex = 15;
            this.label4.Text = "Unit Cons/Cons";
            // 
            // disDescription
            // 
            this.disDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disDescription.Location = new System.Drawing.Point(129, 29);
            this.disDescription.Multiline = true;
            this.disDescription.Name = "disDescription";
            this.disDescription.Size = new System.Drawing.Size(212, 70);
            this.disDescription.TabIndex = 14;
            // 
            // disFabricTypeRefno
            // 
            this.disFabricTypeRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disFabricTypeRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disFabricTypeRefno.Location = new System.Drawing.Point(129, 3);
            this.disFabricTypeRefno.Name = "disFabricTypeRefno";
            this.disFabricTypeRefno.Size = new System.Drawing.Size(212, 23);
            this.disFabricTypeRefno.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(3, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Description";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Fabric Type/Refno";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.numBalanceLayer);
            this.panel2.Controls.Add(this.numTotalLayer);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.displayBoxStyle);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.displayBoxSP);
            this.panel2.Controls.Add(this.labelCutplanID);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(548, 35);
            this.panel2.TabIndex = 12;
            // 
            // numBalanceLayer
            // 
            this.numBalanceLayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numBalanceLayer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numBalanceLayer.IsSupportEditMode = false;
            this.numBalanceLayer.Location = new System.Drawing.Point(595, 5);
            this.numBalanceLayer.Name = "numBalanceLayer";
            this.numBalanceLayer.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBalanceLayer.ReadOnly = true;
            this.numBalanceLayer.Size = new System.Drawing.Size(95, 23);
            this.numBalanceLayer.TabIndex = 38;
            this.numBalanceLayer.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numTotalLayer
            // 
            this.numTotalLayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalLayer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalLayer.IsSupportEditMode = false;
            this.numTotalLayer.Location = new System.Drawing.Point(480, 5);
            this.numTotalLayer.Name = "numTotalLayer";
            this.numTotalLayer.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalLayer.ReadOnly = true;
            this.numTotalLayer.Size = new System.Drawing.Size(95, 23);
            this.numTotalLayer.TabIndex = 37;
            this.numTotalLayer.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(580, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(12, 17);
            this.label7.TabIndex = 35;
            this.label7.Text = "/";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(376, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(101, 23);
            this.label8.TabIndex = 34;
            this.label8.Text = "Ttl./Bal. Layer";
            // 
            // displayBoxStyle
            // 
            this.displayBoxStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxStyle.Location = new System.Drawing.Point(261, 5);
            this.displayBoxStyle.Name = "displayBoxStyle";
            this.displayBoxStyle.Size = new System.Drawing.Size(111, 23);
            this.displayBoxStyle.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(190, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 23);
            this.label1.TabIndex = 13;
            this.label1.Text = "Style";
            // 
            // displayBoxSP
            // 
            this.displayBoxSP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxSP.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayBoxSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxSP.Location = new System.Drawing.Point(76, 5);
            this.displayBoxSP.Name = "displayBoxSP";
            this.displayBoxSP.Size = new System.Drawing.Size(111, 23);
            this.displayBoxSP.TabIndex = 12;
            // 
            // labelCutplanID
            // 
            this.labelCutplanID.Location = new System.Drawing.Point(5, 5);
            this.labelCutplanID.Name = "labelCutplanID";
            this.labelCutplanID.Size = new System.Drawing.Size(68, 23);
            this.labelCutplanID.TabIndex = 2;
            this.labelCutplanID.Text = "SP";
            // 
            // btnBatchAssign
            // 
            this.btnBatchAssign.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnBatchAssign.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBatchAssign.Location = new System.Drawing.Point(327, 34);
            this.btnBatchAssign.Name = "btnBatchAssign";
            this.btnBatchAssign.Size = new System.Drawing.Size(113, 30);
            this.btnBatchAssign.TabIndex = 2;
            this.btnBatchAssign.Text = "Batch Assign";
            this.btnBatchAssign.UseVisualStyleBackColor = true;
            this.btnBatchAssign.Click += new System.EventHandler(this.BtnBatchAssign_Click);
            // 
            // btnImportMarker
            // 
            this.btnImportMarker.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportMarker.Location = new System.Drawing.Point(441, 34);
            this.btnImportMarker.Name = "btnImportMarker";
            this.btnImportMarker.Size = new System.Drawing.Size(124, 30);
            this.btnImportMarker.TabIndex = 3;
            this.btnImportMarker.Text = "Import Marker";
            this.btnImportMarker.UseVisualStyleBackColor = true;
            this.btnImportMarker.Click += new System.EventHandler(this.BtnImportMarker_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Image = global::Sci.Production.Cutting.Properties.Resources.download;
            this.btnDownload.Location = new System.Drawing.Point(565, 37);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(25, 25);
            this.btnDownload.TabIndex = 4;
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.BtnDownload_Click);
            // 
            // btnImportMarkerLectra
            // 
            this.btnImportMarkerLectra.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImportMarkerLectra.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportMarkerLectra.Location = new System.Drawing.Point(595, 34);
            this.btnImportMarkerLectra.Name = "btnImportMarkerLectra";
            this.btnImportMarkerLectra.Size = new System.Drawing.Size(175, 30);
            this.btnImportMarkerLectra.TabIndex = 14;
            this.btnImportMarkerLectra.Text = "Import Marker(Lectra)";
            this.btnImportMarkerLectra.UseVisualStyleBackColor = true;
            this.btnImportMarkerLectra.Click += new System.EventHandler(this.BtnImportMarkerLectra_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnEdit.Location = new System.Drawing.Point(772, 34);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(61, 30);
            this.btnEdit.TabIndex = 15;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // btnAutoRef
            // 
            this.btnAutoRef.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnAutoRef.Location = new System.Drawing.Point(3, 6);
            this.btnAutoRef.Name = "btnAutoRef";
            this.btnAutoRef.Size = new System.Drawing.Size(92, 30);
            this.btnAutoRef.TabIndex = 16;
            this.btnAutoRef.Text = "Auto Ref#";
            this.btnAutoRef.UseVisualStyleBackColor = true;
            this.btnAutoRef.Click += new System.EventHandler(this.BtnAutoRef_Click);
            // 
            // btnCutPartsCheck
            // 
            this.btnCutPartsCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCutPartsCheck.Location = new System.Drawing.Point(640, 6);
            this.btnCutPartsCheck.Name = "btnCutPartsCheck";
            this.btnCutPartsCheck.Size = new System.Drawing.Size(133, 30);
            this.btnCutPartsCheck.TabIndex = 20;
            this.btnCutPartsCheck.Text = "Cut Parts Check";
            this.btnCutPartsCheck.UseVisualStyleBackColor = true;
            this.btnCutPartsCheck.Click += new System.EventHandler(this.BtnCutPartsCheck_Click);
            // 
            // btnCutPartsCheckSummary
            // 
            this.btnCutPartsCheckSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCutPartsCheckSummary.Location = new System.Drawing.Point(774, 6);
            this.btnCutPartsCheckSummary.Name = "btnCutPartsCheckSummary";
            this.btnCutPartsCheckSummary.Size = new System.Drawing.Size(204, 30);
            this.btnCutPartsCheckSummary.TabIndex = 21;
            this.btnCutPartsCheckSummary.Text = "Cut Parts Check Summary";
            this.btnCutPartsCheckSummary.UseVisualStyleBackColor = true;
            this.btnCutPartsCheckSummary.Click += new System.EventHandler(this.BtnCutPartsCheckSummary_Click);
            // 
            // btnQtyBreakdown
            // 
            this.btnQtyBreakdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnQtyBreakdown.Location = new System.Drawing.Point(978, 7);
            this.btnQtyBreakdown.Name = "btnQtyBreakdown";
            this.btnQtyBreakdown.Size = new System.Drawing.Size(125, 28);
            this.btnQtyBreakdown.TabIndex = 23;
            this.btnQtyBreakdown.Text = "Qty Breakdown";
            this.btnQtyBreakdown.UseVisualStyleBackColor = true;
            this.btnQtyBreakdown.Click += new System.EventHandler(this.BtnQtyBreakdown_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnToExcel.Location = new System.Drawing.Point(1104, 7);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(92, 28);
            this.btnToExcel.TabIndex = 24;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // P02
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1288, 717);
            this.DefaultOrder = "ID";
            this.ExpressQuery = true;
            this.GridAlias = "WorkOrderForPlanning";
            this.GridNew = 0;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.KeyField1 = "ID";
            this.Name = "P02";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P02. WorkOrder For Planning";
            this.WorkAlias = "Cutting";
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).EndInit();
            this.masterpanel.ResumeLayout(false);
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
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSizeRatio)).EndInit();
            this.sizeratioMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sizeRatioBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridOrderList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderListBindingSource)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qtybreakBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Win.UI.Label labelCutplanID;
        private Win.UI.DisplayBox displayBoxStyle;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox displayBoxSP;
        private Win.UI.Button btnBatchAssign;
        private Win.UI.Button btnImportMarker;
        private Win.UI.Button btnDownload;
        private Win.UI.Button btnImportMarkerLectra;
        private Win.UI.Button btnEdit;
        private Win.UI.DisplayBox disDescription;
        private Win.UI.DisplayBox disFabricTypeRefno;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private System.Windows.Forms.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Button btnAutoRef;
        private Win.UI.Button btnToExcel;
        private Win.UI.Button btnQtyBreakdown;
        private Win.UI.Button btnCutPartsCheckSummary;
        private Win.UI.Button btnCutPartsCheck;
        private Win.UI.ListControlBindingSource sizeRatioBindingSource;
        private Win.UI.ListControlBindingSource orderListBindingSource;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label6;
        private Win.UI.Grid gridSizeRatio;
        private System.Windows.Forms.Label label11;
        private Win.UI.Grid gridOrderList;
        private System.Windows.Forms.Label label7;
        private Win.UI.Label label8;
        private Win.UI.NumericBox numTotalLayer;
        private Win.UI.NumericBox numBalanceLayer;
        private Win.UI.NumericBox numUnitCons;
        private Win.UI.NumericBox numCons;
        private System.Windows.Forms.ToolStripMenuItem insertSizeRatioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteSizeRatioToolStripMenuItem;
        private Win.UI.ContextMenuStrip sizeratioMenuStrip;
        private Win.UI.ListControlBindingSource qtybreakBindingSource;
    }
}
