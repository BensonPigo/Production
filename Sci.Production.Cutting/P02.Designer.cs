using System.Windows.Forms;

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
            this.label13 = new Sci.Win.UI.Label();
            this.numTtlDistQty = new Sci.Win.UI.NumericBox();
            this.txtMarkerLength = new Sci.Production.Class.TxtMarkerLength();
            this.txtPatternNo = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.numCons = new Sci.Win.UI.NumericBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label6 = new System.Windows.Forms.Label();
            this.gridSizeRatio = new Sci.Win.UI.Grid();
            this.cmsSizeRatio = new Sci.Win.UI.ContextMenuStrip();
            this.insertSizeRatioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSizeRatioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sizeRatiobs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label11 = new System.Windows.Forms.Label();
            this.gridDistributeToSP = new Sci.Win.UI.Grid();
            this.cmsDistribute = new Sci.Win.UI.ContextMenuStrip();
            this.MenuItemInsertDistribute = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDeleteDistribute = new System.Windows.Forms.ToolStripMenuItem();
            this.distributebs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label12 = new System.Windows.Forms.Label();
            this.gridQtyBreakDown = new Sci.Win.UI.Grid();
            this.qtybreakds = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.numUnitCons = new Sci.Win.UI.NumericBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.displayBoxDescription = new Sci.Win.UI.DisplayBox();
            this.displayBoxFabricTypeRefno = new Sci.Win.UI.DisplayBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label17 = new Sci.Win.UI.Label();
            this.comboSort = new Sci.Win.UI.ComboBox();
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
            this.btnAutoSeq = new Sci.Win.UI.Button();
            this.btnPackingMethod = new Sci.Win.UI.Button();
            this.btnExcludeSetting = new Sci.Win.UI.Button();
            this.btnAllSPDistribute = new Sci.Win.UI.Button();
            this.btnDistributeThisCutRef = new Sci.Win.UI.Button();
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
            this.cmsSizeRatio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeRatiobs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDistributeToSP)).BeginInit();
            this.cmsDistribute.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.distributebs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridQtyBreakDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qtybreakds)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qtybreakBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.btnExcludeSetting);
            this.masterpanel.Controls.Add(this.btnEdit);
            this.masterpanel.Controls.Add(this.btnImportMarkerLectra);
            this.masterpanel.Controls.Add(this.btnDownload);
            this.masterpanel.Controls.Add(this.btnImportMarker);
            this.masterpanel.Controls.Add(this.btnBatchAssign);
            this.masterpanel.Controls.Add(this.panel2);
            this.masterpanel.Size = new System.Drawing.Size(858, 65);
            this.masterpanel.Controls.SetChildIndex(this.panel2, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnBatchAssign, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportMarker, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnDownload, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportMarkerLectra, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnEdit, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnExcludeSetting, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 65);
            this.detailpanel.Size = new System.Drawing.Size(858, 552);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.gridicon.Dock = System.Windows.Forms.DockStyle.Right;
            this.gridicon.Location = new System.Drawing.Point(758, 35);
            // 
            // refresh
            // 
            this.refresh.Dock = System.Windows.Forms.DockStyle.Right;
            this.refresh.Location = new System.Drawing.Point(1200, 0);
            this.refresh.Size = new System.Drawing.Size(80, 38);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(858, 552);
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
            this.detail.Size = new System.Drawing.Size(1280, 655);
            this.detail.Controls.SetChildIndex(this.detailbtm, 0);
            this.detail.Controls.SetChildIndex(this.panel1, 0);
            this.detail.Controls.SetChildIndex(this.detailcont, 0);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(858, 617);
            // 
            // detailbtm
            // 
            this.detailbtm.Controls.Add(this.btnDistributeThisCutRef);
            this.detailbtm.Controls.Add(this.btnAllSPDistribute);
            this.detailbtm.Controls.Add(this.btnPackingMethod);
            this.detailbtm.Controls.Add(this.btnAutoSeq);
            this.detailbtm.Controls.Add(this.btnToExcel);
            this.detailbtm.Controls.Add(this.btnQtyBreakdown);
            this.detailbtm.Controls.Add(this.btnCutPartsCheckSummary);
            this.detailbtm.Controls.Add(this.btnCutPartsCheck);
            this.detailbtm.Controls.Add(this.btnAutoRef);
            this.detailbtm.Location = new System.Drawing.Point(0, 617);
            this.detailbtm.Size = new System.Drawing.Size(1280, 38);
            this.detailbtm.Controls.SetChildIndex(this.lblcreateby, 0);
            this.detailbtm.Controls.SetChildIndex(this.lbleditby, 0);
            this.detailbtm.Controls.SetChildIndex(this.editby, 0);
            this.detailbtm.Controls.SetChildIndex(this.createby, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnAutoRef, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnCutPartsCheck, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnCutPartsCheckSummary, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnQtyBreakdown, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnToExcel, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnAutoSeq, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnPackingMethod, 0);
            this.detailbtm.Controls.SetChildIndex(this.refresh, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnAllSPDistribute, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnDistributeThisCutRef, 0);
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
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.numTtlDistQty);
            this.panel1.Controls.Add(this.txtMarkerLength);
            this.panel1.Controls.Add(this.txtPatternNo);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.numCons);
            this.panel1.Controls.Add(this.splitContainer2);
            this.panel1.Controls.Add(this.numUnitCons);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.displayBoxDescription);
            this.panel1.Controls.Add(this.displayBoxFabricTypeRefno);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(858, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(422, 617);
            this.panel1.TabIndex = 4;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label13.Location = new System.Drawing.Point(269, 109);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 23);
            this.label13.TabIndex = 41;
            this.label13.Text = "Ttl. Dist. Qty";
            // 
            // numTtlDistQty
            // 
            this.numTtlDistQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTtlDistQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTtlDistQty.IsSupportEditMode = false;
            this.numTtlDistQty.Location = new System.Drawing.Point(358, 109);
            this.numTtlDistQty.Name = "numTtlDistQty";
            this.numTtlDistQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTtlDistQty.ReadOnly = true;
            this.numTtlDistQty.Size = new System.Drawing.Size(56, 23);
            this.numTtlDistQty.TabIndex = 40;
            this.numTtlDistQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtMarkerLength
            // 
            this.txtMarkerLength.BackColor = System.Drawing.Color.White;
            this.txtMarkerLength.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMarkerLength.Location = new System.Drawing.Point(326, 82);
            this.txtMarkerLength.Mask = "00Y00-0/0+0\"";
            this.txtMarkerLength.Name = "txtMarkerLength";
            this.txtMarkerLength.Size = new System.Drawing.Size(88, 23);
            this.txtMarkerLength.TabIndex = 39;
            this.txtMarkerLength.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            this.txtMarkerLength.Validating += new System.ComponentModel.CancelEventHandler(this.TxtMarkerLength_Validating);
            // 
            // txtPatternNo
            // 
            this.txtPatternNo.BackColor = System.Drawing.Color.White;
            this.txtPatternNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPatternNo.Location = new System.Drawing.Point(129, 82);
            this.txtPatternNo.Name = "txtPatternNo";
            this.txtPatternNo.Size = new System.Drawing.Size(88, 23);
            this.txtPatternNo.TabIndex = 38;
            this.txtPatternNo.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtPatternNo_PopUp);
            this.txtPatternNo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtPatternNo_Validating);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(220, 82);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(103, 23);
            this.label10.TabIndex = 37;
            this.label10.Text = "Marker Length";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(3, 82);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(123, 23);
            this.label9.TabIndex = 36;
            this.label9.Text = "Pattern No.";
            // 
            // numCons
            // 
            this.numCons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numCons.DecimalPlaces = 4;
            this.numCons.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numCons.IsSupportEditMode = false;
            this.numCons.Location = new System.Drawing.Point(192, 109);
            this.numCons.Name = "numCons";
            this.numCons.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCons.ReadOnly = true;
            this.numCons.Size = new System.Drawing.Size(74, 23);
            this.numCons.TabIndex = 35;
            this.numCons.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(6, 138);
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
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(413, 470);
            this.splitContainer2.SplitterDistance = 135;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 34;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 3);
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
            this.gridSizeRatio.ContextMenuStrip = this.cmsSizeRatio;
            this.gridSizeRatio.DataSource = this.sizeRatiobs;
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
            this.gridSizeRatio.Size = new System.Drawing.Size(412, 108);
            this.gridSizeRatio.TabIndex = 34;
            this.gridSizeRatio.EditingKeyProcessing += new System.EventHandler<Ict.Win.UI.DataGridViewEditingKeyProcessingEventArgs>(this.GridSizeRatio_EditingKeyProcessing);
            // 
            // cmsSizeRatio
            // 
            this.cmsSizeRatio.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertSizeRatioToolStripMenuItem,
            this.deleteSizeRatioToolStripMenuItem});
            this.cmsSizeRatio.Name = "sizeratioMenuStrip";
            this.cmsSizeRatio.Size = new System.Drawing.Size(164, 48);
            // 
            // insertSizeRatioToolStripMenuItem
            // 
            this.insertSizeRatioToolStripMenuItem.Name = "insertSizeRatioToolStripMenuItem";
            this.insertSizeRatioToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.insertSizeRatioToolStripMenuItem.Text = "Insert Size Ratio";
            this.insertSizeRatioToolStripMenuItem.Click += new System.EventHandler(this.MenuItemInsertSizeRatio_Click);
            // 
            // deleteSizeRatioToolStripMenuItem
            // 
            this.deleteSizeRatioToolStripMenuItem.Name = "deleteSizeRatioToolStripMenuItem";
            this.deleteSizeRatioToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.deleteSizeRatioToolStripMenuItem.Text = "Delete Record";
            this.deleteSizeRatioToolStripMenuItem.Click += new System.EventHandler(this.MenuItemDeleteSizeRatio_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label11);
            this.splitContainer1.Panel1.Controls.Add(this.gridDistributeToSP);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label12);
            this.splitContainer1.Panel2.Controls.Add(this.gridQtyBreakDown);
            this.splitContainer1.Size = new System.Drawing.Size(413, 330);
            this.splitContainer1.SplitterDistance = 149;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 32;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 2);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(119, 17);
            this.label11.TabIndex = 27;
            this.label11.Text = "Distribute To SP#";
            // 
            // gridDistributeToSP
            // 
            this.gridDistributeToSP.AllowUserToAddRows = false;
            this.gridDistributeToSP.AllowUserToDeleteRows = false;
            this.gridDistributeToSP.AllowUserToResizeRows = false;
            this.gridDistributeToSP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDistributeToSP.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDistributeToSP.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDistributeToSP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDistributeToSP.ContextMenuStrip = this.cmsDistribute;
            this.gridDistributeToSP.DataSource = this.distributebs;
            this.gridDistributeToSP.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDistributeToSP.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDistributeToSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDistributeToSP.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDistributeToSP.Location = new System.Drawing.Point(1, 22);
            this.gridDistributeToSP.Name = "gridDistributeToSP";
            this.gridDistributeToSP.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDistributeToSP.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDistributeToSP.RowTemplate.Height = 24;
            this.gridDistributeToSP.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDistributeToSP.ShowCellToolTips = false;
            this.gridDistributeToSP.Size = new System.Drawing.Size(411, 127);
            this.gridDistributeToSP.TabIndex = 28;
            this.gridDistributeToSP.SelectionChanged += new System.EventHandler(this.GridDistributeToSP_SelectionChanged);
            // 
            // cmsDistribute
            // 
            this.cmsDistribute.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsDistribute.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemInsertDistribute,
            this.MenuItemDeleteDistribute});
            this.cmsDistribute.Name = "contextMenuStrip1";
            this.cmsDistribute.Size = new System.Drawing.Size(162, 48);
            // 
            // MenuItemInsertDistribute
            // 
            this.MenuItemInsertDistribute.Name = "MenuItemInsertDistribute";
            this.MenuItemInsertDistribute.Size = new System.Drawing.Size(161, 22);
            this.MenuItemInsertDistribute.Text = "Insert Distribute";
            this.MenuItemInsertDistribute.Click += new System.EventHandler(this.MenuItemInsertDistribute_Click);
            // 
            // MenuItemDeleteDistribute
            // 
            this.MenuItemDeleteDistribute.Name = "MenuItemDeleteDistribute";
            this.MenuItemDeleteDistribute.Size = new System.Drawing.Size(161, 22);
            this.MenuItemDeleteDistribute.Text = "Delete Record";
            this.MenuItemDeleteDistribute.Click += new System.EventHandler(this.MenuItemDeleteDistribute_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 1);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(110, 17);
            this.label12.TabIndex = 29;
            this.label12.Text = "Qty Break Down";
            // 
            // gridQtyBreakDown
            // 
            this.gridQtyBreakDown.AllowUserToAddRows = false;
            this.gridQtyBreakDown.AllowUserToDeleteRows = false;
            this.gridQtyBreakDown.AllowUserToResizeRows = false;
            this.gridQtyBreakDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridQtyBreakDown.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridQtyBreakDown.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridQtyBreakDown.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridQtyBreakDown.DataSource = this.qtybreakds;
            this.gridQtyBreakDown.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridQtyBreakDown.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridQtyBreakDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridQtyBreakDown.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridQtyBreakDown.Location = new System.Drawing.Point(1, 21);
            this.gridQtyBreakDown.Name = "gridQtyBreakDown";
            this.gridQtyBreakDown.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridQtyBreakDown.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridQtyBreakDown.RowTemplate.Height = 24;
            this.gridQtyBreakDown.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridQtyBreakDown.ShowCellToolTips = false;
            this.gridQtyBreakDown.Size = new System.Drawing.Size(411, 134);
            this.gridQtyBreakDown.TabIndex = 30;
            // 
            // numUnitCons
            // 
            this.numUnitCons.BackColor = System.Drawing.Color.White;
            this.numUnitCons.DecimalPlaces = 4;
            this.numUnitCons.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numUnitCons.Location = new System.Drawing.Point(110, 108);
            this.numUnitCons.Name = "numUnitCons";
            this.numUnitCons.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numUnitCons.Size = new System.Drawing.Size(72, 23);
            this.numUnitCons.TabIndex = 33;
            this.numUnitCons.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numUnitCons.Validated += new System.EventHandler(this.NumUnitCons_Validated);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(181, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 17);
            this.label5.TabIndex = 17;
            this.label5.Text = "/";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(3, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 23);
            this.label4.TabIndex = 15;
            this.label4.Text = "Unit Cons/Cons";
            // 
            // displayBoxDescription
            // 
            this.displayBoxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxDescription.Location = new System.Drawing.Point(129, 29);
            this.displayBoxDescription.Multiline = true;
            this.displayBoxDescription.Name = "displayBoxDescription";
            this.displayBoxDescription.Size = new System.Drawing.Size(285, 50);
            this.displayBoxDescription.TabIndex = 14;
            // 
            // displayBoxFabricTypeRefno
            // 
            this.displayBoxFabricTypeRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxFabricTypeRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxFabricTypeRefno.Location = new System.Drawing.Point(129, 3);
            this.displayBoxFabricTypeRefno.Name = "displayBoxFabricTypeRefno";
            this.displayBoxFabricTypeRefno.Size = new System.Drawing.Size(285, 23);
            this.displayBoxFabricTypeRefno.TabIndex = 13;
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
            this.panel2.Controls.Add(this.label17);
            this.panel2.Controls.Add(this.comboSort);
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
            this.panel2.Size = new System.Drawing.Size(858, 35);
            this.panel2.TabIndex = 12;
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(694, 5);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(33, 23);
            this.label17.TabIndex = 45;
            this.label17.Text = "Sort";
            // 
            // comboSort
            // 
            this.comboSort.BackColor = System.Drawing.Color.White;
            this.comboSort.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboSort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSort.FormattingEnabled = true;
            this.comboSort.IsSupportUnselect = true;
            this.comboSort.Items.AddRange(new object[] {
            "",
            "SP",
            "Cut#",
            "Ref#",
            "Cutplan#",
            "MarkerName"});
            this.comboSort.Location = new System.Drawing.Point(730, 4);
            this.comboSort.Name = "comboSort";
            this.comboSort.OldText = "";
            this.comboSort.Size = new System.Drawing.Size(122, 24);
            this.comboSort.TabIndex = 44;
            this.comboSort.SelectedIndexChanged += new System.EventHandler(this.ComboSort_SelectedIndexChanged);
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
            this.btnBatchAssign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBatchAssign.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnBatchAssign.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBatchAssign.Location = new System.Drawing.Point(249, 34);
            this.btnBatchAssign.Name = "btnBatchAssign";
            this.btnBatchAssign.Size = new System.Drawing.Size(113, 32);
            this.btnBatchAssign.TabIndex = 2;
            this.btnBatchAssign.Text = "Batch Assign";
            this.btnBatchAssign.UseVisualStyleBackColor = true;
            this.btnBatchAssign.Click += new System.EventHandler(this.BtnBatchAssign_Click);
            // 
            // btnImportMarker
            // 
            this.btnImportMarker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportMarker.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnImportMarker.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportMarker.Location = new System.Drawing.Point(363, 34);
            this.btnImportMarker.Name = "btnImportMarker";
            this.btnImportMarker.Size = new System.Drawing.Size(124, 32);
            this.btnImportMarker.TabIndex = 3;
            this.btnImportMarker.Text = "Import Marker";
            this.btnImportMarker.UseVisualStyleBackColor = true;
            this.btnImportMarker.Click += new System.EventHandler(this.BtnImportMarker_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.Image = global::Sci.Production.Cutting.Properties.Resources.download;
            this.btnDownload.Location = new System.Drawing.Point(487, 37);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(25, 25);
            this.btnDownload.TabIndex = 4;
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.BtnDownload_Click);
            // 
            // btnImportMarkerLectra
            // 
            this.btnImportMarkerLectra.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportMarkerLectra.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImportMarkerLectra.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportMarkerLectra.Location = new System.Drawing.Point(517, 34);
            this.btnImportMarkerLectra.Name = "btnImportMarkerLectra";
            this.btnImportMarkerLectra.Size = new System.Drawing.Size(175, 32);
            this.btnImportMarkerLectra.TabIndex = 14;
            this.btnImportMarkerLectra.Text = "Import Marker(Lectra)";
            this.btnImportMarkerLectra.UseVisualStyleBackColor = true;
            this.btnImportMarkerLectra.Click += new System.EventHandler(this.BtnImportMarkerLectra_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnEdit.Location = new System.Drawing.Point(694, 34);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(61, 32);
            this.btnEdit.TabIndex = 15;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // btnAutoRef
            // 
            this.btnAutoRef.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnAutoRef.Location = new System.Drawing.Point(3, 3);
            this.btnAutoRef.Name = "btnAutoRef";
            this.btnAutoRef.Size = new System.Drawing.Size(92, 32);
            this.btnAutoRef.TabIndex = 16;
            this.btnAutoRef.Text = "Auto Ref#";
            this.btnAutoRef.UseVisualStyleBackColor = true;
            this.btnAutoRef.Click += new System.EventHandler(this.BtnAutoRef_Click);
            // 
            // btnCutPartsCheck
            // 
            this.btnCutPartsCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCutPartsCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCutPartsCheck.ForeColor = System.Drawing.Color.Blue;
            this.btnCutPartsCheck.Location = new System.Drawing.Point(493, 3);
            this.btnCutPartsCheck.Name = "btnCutPartsCheck";
            this.btnCutPartsCheck.Size = new System.Drawing.Size(133, 32);
            this.btnCutPartsCheck.TabIndex = 20;
            this.btnCutPartsCheck.Text = "Cut Parts Check";
            this.btnCutPartsCheck.UseVisualStyleBackColor = true;
            this.btnCutPartsCheck.Click += new System.EventHandler(this.BtnCutPartsCheck_Click);
            // 
            // btnCutPartsCheckSummary
            // 
            this.btnCutPartsCheckSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCutPartsCheckSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCutPartsCheckSummary.ForeColor = System.Drawing.Color.Blue;
            this.btnCutPartsCheckSummary.Location = new System.Drawing.Point(627, 3);
            this.btnCutPartsCheckSummary.Name = "btnCutPartsCheckSummary";
            this.btnCutPartsCheckSummary.Size = new System.Drawing.Size(204, 32);
            this.btnCutPartsCheckSummary.TabIndex = 21;
            this.btnCutPartsCheckSummary.Text = "Cut Parts Check Summary";
            this.btnCutPartsCheckSummary.UseVisualStyleBackColor = true;
            this.btnCutPartsCheckSummary.Click += new System.EventHandler(this.BtnCutPartsCheckSummary_Click);
            // 
            // btnQtyBreakdown
            // 
            this.btnQtyBreakdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQtyBreakdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnQtyBreakdown.Location = new System.Drawing.Point(978, 3);
            this.btnQtyBreakdown.Name = "btnQtyBreakdown";
            this.btnQtyBreakdown.Size = new System.Drawing.Size(125, 32);
            this.btnQtyBreakdown.TabIndex = 23;
            this.btnQtyBreakdown.Text = "Qty Breakdown";
            this.btnQtyBreakdown.UseVisualStyleBackColor = true;
            this.btnQtyBreakdown.Click += new System.EventHandler(this.BtnQtyBreakdown_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnToExcel.Location = new System.Drawing.Point(1104, 3);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(92, 32);
            this.btnToExcel.TabIndex = 24;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // btnAutoSeq
            // 
            this.btnAutoSeq.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnAutoSeq.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnAutoSeq.Location = new System.Drawing.Point(101, 3);
            this.btnAutoSeq.Name = "btnAutoSeq";
            this.btnAutoSeq.Size = new System.Drawing.Size(92, 32);
            this.btnAutoSeq.TabIndex = 25;
            this.btnAutoSeq.Text = "Auto Seq";
            this.btnAutoSeq.UseVisualStyleBackColor = true;
            this.btnAutoSeq.Click += new System.EventHandler(this.BtnAutoSeq_Click);
            // 
            // btnPackingMethod
            // 
            this.btnPackingMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPackingMethod.Location = new System.Drawing.Point(832, 3);
            this.btnPackingMethod.Name = "btnPackingMethod";
            this.btnPackingMethod.Size = new System.Drawing.Size(145, 32);
            this.btnPackingMethod.TabIndex = 94;
            this.btnPackingMethod.Text = "Packing Method";
            this.btnPackingMethod.UseVisualStyleBackColor = true;
            this.btnPackingMethod.Click += new System.EventHandler(this.BtnPackingMethod_Click);
            // 
            // btnExcludeSetting
            // 
            this.btnExcludeSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcludeSetting.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnExcludeSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnExcludeSetting.Location = new System.Drawing.Point(136, 34);
            this.btnExcludeSetting.Name = "btnExcludeSetting";
            this.btnExcludeSetting.Size = new System.Drawing.Size(113, 32);
            this.btnExcludeSetting.TabIndex = 16;
            this.btnExcludeSetting.Text = "Exclude Setting";
            this.btnExcludeSetting.UseVisualStyleBackColor = true;
            this.btnExcludeSetting.Click += new System.EventHandler(this.BtnExcludeSetting_Click);
            // 
            // btnAllSPDistribute
            // 
            this.btnAllSPDistribute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAllSPDistribute.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnAllSPDistribute.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnAllSPDistribute.Location = new System.Drawing.Point(201, 3);
            this.btnAllSPDistribute.Name = "btnAllSPDistribute";
            this.btnAllSPDistribute.Size = new System.Drawing.Size(145, 32);
            this.btnAllSPDistribute.TabIndex = 95;
            this.btnAllSPDistribute.Text = "All SP# Distribute";
            this.btnAllSPDistribute.UseVisualStyleBackColor = true;
            this.btnAllSPDistribute.Click += new System.EventHandler(this.BtnAllSPDistribute_Click);
            // 
            // btnDistributeThisCutRef
            // 
            this.btnDistributeThisCutRef.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDistributeThisCutRef.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnDistributeThisCutRef.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnDistributeThisCutRef.Location = new System.Drawing.Point(348, 3);
            this.btnDistributeThisCutRef.Name = "btnDistributeThisCutRef";
            this.btnDistributeThisCutRef.Size = new System.Drawing.Size(145, 32);
            this.btnDistributeThisCutRef.TabIndex = 96;
            this.btnDistributeThisCutRef.Text = "Distribute This CutRef";
            this.btnDistributeThisCutRef.UseVisualStyleBackColor = true;
            this.btnDistributeThisCutRef.Click += new System.EventHandler(this.BtnDistributeThisCutRef_Click);
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
            this.SizeChanged += new System.EventHandler(this.P02_SizeChanged);
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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSizeRatio)).EndInit();
            this.cmsSizeRatio.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sizeRatiobs)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDistributeToSP)).EndInit();
            this.cmsDistribute.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.distributebs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridQtyBreakDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qtybreakds)).EndInit();
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
        private Win.UI.DisplayBox displayBoxDescription;
        private Win.UI.DisplayBox displayBoxFabricTypeRefno;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private System.Windows.Forms.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Button btnAutoRef;
        private Win.UI.Button btnToExcel;
        private Win.UI.Button btnQtyBreakdown;
        private Win.UI.Button btnCutPartsCheckSummary;
        private Win.UI.Button btnCutPartsCheck;
        private Win.UI.ListControlBindingSource sizeRatiobs;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label6;
        private Win.UI.Grid gridSizeRatio;
        private System.Windows.Forms.Label label7;
        private Win.UI.Label label8;
        private Win.UI.NumericBox numTotalLayer;
        private Win.UI.NumericBox numBalanceLayer;
        private Win.UI.NumericBox numUnitCons;
        private Win.UI.NumericBox numCons;
        private System.Windows.Forms.ToolStripMenuItem insertSizeRatioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteSizeRatioToolStripMenuItem;
        private Win.UI.ContextMenuStrip cmsSizeRatio;
        private Win.UI.ListControlBindingSource qtybreakBindingSource;
        private Win.UI.Button btnAutoSeq;
        private Win.UI.Button btnPackingMethod;
        private Win.UI.Label label9;
        private Win.UI.Label label10;
        private Win.UI.TextBox txtPatternNo;
        private Win.UI.Button btnExcludeSetting;
        private SplitContainer splitContainer1;
        private Label label11;
        private Win.UI.Grid gridDistributeToSP;
        private Label label12;
        private Win.UI.Grid gridQtyBreakDown;
        private Win.UI.ListControlBindingSource distributebs;
        private Win.UI.ListControlBindingSource qtybreakds;
        private Win.UI.ContextMenuStrip cmsDistribute;
        private ToolStripMenuItem MenuItemInsertDistribute;
        private ToolStripMenuItem MenuItemDeleteDistribute;
        private Win.UI.Button btnDistributeThisCutRef;
        private Win.UI.Button btnAllSPDistribute;
        private Class.TxtMarkerLength txtMarkerLength;
        private Win.UI.NumericBox numTtlDistQty;
        private Win.UI.Label label13;
        private Win.UI.ComboBox comboSort;
        private Win.UI.Label label17;
    }
}
