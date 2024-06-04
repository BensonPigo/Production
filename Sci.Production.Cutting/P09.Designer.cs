namespace Sci.Production.Cutting
{
    partial class P09
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnImportFromWorkOrderForPlanning = new Sci.Win.UI.Button();
            this.btnExcludeSetting = new Sci.Win.UI.Button();
            this.displayBoxStyle = new Sci.Win.UI.DisplayBox();
            this.label1 = new Sci.Win.UI.Label();
            this.displayBoxSP = new Sci.Win.UI.DisplayBox();
            this.labelCutplanID = new Sci.Win.UI.Label();
            this.labelMSG = new System.Windows.Forms.Label();
            this.btnBatchAssign = new Sci.Win.UI.Button();
            this.btnImportMarker = new Sci.Win.UI.Button();
            this.btnDownload = new Sci.Win.UI.Button();
            this.btnImportMarkerLectra = new Sci.Win.UI.Button();
            this.btnEdit = new Sci.Win.UI.Button();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.displayBoxFabricTypeRefno = new Sci.Win.UI.DisplayBox();
            this.displayBoxDescription = new Sci.Win.UI.DisplayBox();
            this.label4 = new Sci.Win.UI.Label();
            this.txtUnitCons1 = new Sci.Win.UI.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUnitCons2 = new Sci.Win.UI.TextBox();
            this.displayBoxTotalCutQty = new Sci.Win.UI.DisplayBox();
            this.label6 = new Sci.Win.UI.Label();
            this.displayBoxTtlDistQty = new Sci.Win.UI.DisplayBox();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.gridSizeRatio = new Sci.Win.UI.Grid();
            this.label9 = new System.Windows.Forms.Label();
            this.gridSpreadingFabric = new Sci.Win.UI.Grid();
            this.label10 = new System.Windows.Forms.Label();
            this.gridDistributeToSP = new Sci.Win.UI.Grid();
            this.label11 = new System.Windows.Forms.Label();
            this.gridQtyBreakDown = new Sci.Win.UI.Grid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnAutoRef = new Sci.Win.UI.Button();
            this.btnAutoCut = new Sci.Win.UI.Button();
            this.btnAllSPDistribute = new Sci.Win.UI.Button();
            this.btnDistributeThisCutRef = new Sci.Win.UI.Button();
            this.btnCutPartsCheck = new Sci.Win.UI.Button();
            this.btnCutPartsCheckSummary = new Sci.Win.UI.Button();
            this.btnHistory = new Sci.Win.UI.Button();
            this.btnQtyBreakdown = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
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
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSizeRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSpreadingFabric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDistributeToSP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridQtyBreakDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.btnEdit);
            this.masterpanel.Controls.Add(this.btnImportMarkerLectra);
            this.masterpanel.Controls.Add(this.btnDownload);
            this.masterpanel.Controls.Add(this.btnImportMarker);
            this.masterpanel.Controls.Add(this.btnBatchAssign);
            this.masterpanel.Controls.Add(this.labelMSG);
            this.masterpanel.Controls.Add(this.panel2);
            this.masterpanel.Size = new System.Drawing.Size(936, 65);
            this.masterpanel.Controls.SetChildIndex(this.panel2, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelMSG, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnBatchAssign, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportMarker, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnDownload, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportMarkerLectra, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnEdit, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 65);
            this.detailpanel.Size = new System.Drawing.Size(936, 549);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.gridicon.Dock = System.Windows.Forms.DockStyle.Right;
            this.gridicon.Location = new System.Drawing.Point(836, 35);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(1197, 3);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(936, 549);
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
            this.detailcont.Size = new System.Drawing.Size(936, 614);
            // 
            // detailbtm
            // 
            this.detailbtm.Controls.Add(this.btnToExcel);
            this.detailbtm.Controls.Add(this.btnQtyBreakdown);
            this.detailbtm.Controls.Add(this.btnHistory);
            this.detailbtm.Controls.Add(this.btnCutPartsCheckSummary);
            this.detailbtm.Controls.Add(this.btnCutPartsCheck);
            this.detailbtm.Controls.Add(this.btnDistributeThisCutRef);
            this.detailbtm.Controls.Add(this.btnAllSPDistribute);
            this.detailbtm.Controls.Add(this.btnAutoCut);
            this.detailbtm.Controls.Add(this.btnAutoRef);
            this.detailbtm.Location = new System.Drawing.Point(0, 614);
            this.detailbtm.Size = new System.Drawing.Size(1280, 41);
            this.detailbtm.Controls.SetChildIndex(this.lbleditby, 0);
            this.detailbtm.Controls.SetChildIndex(this.lblcreateby, 0);
            this.detailbtm.Controls.SetChildIndex(this.editby, 0);
            this.detailbtm.Controls.SetChildIndex(this.createby, 0);
            this.detailbtm.Controls.SetChildIndex(this.refresh, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnAutoRef, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnAutoCut, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnAllSPDistribute, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnDistributeThisCutRef, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnCutPartsCheck, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnCutPartsCheckSummary, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnHistory, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnQtyBreakdown, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnToExcel, 0);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1184, 603);
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
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Controls.Add(this.gridSpreadingFabric);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.gridSizeRatio);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.displayBoxTtlDistQty);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.displayBoxTotalCutQty);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtUnitCons2);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtUnitCons1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.displayBoxDescription);
            this.panel1.Controls.Add(this.displayBoxFabricTypeRefno);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(936, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(344, 614);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnImportFromWorkOrderForPlanning);
            this.panel2.Controls.Add(this.btnExcludeSetting);
            this.panel2.Controls.Add(this.displayBoxStyle);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.displayBoxSP);
            this.panel2.Controls.Add(this.labelCutplanID);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(936, 35);
            this.panel2.TabIndex = 12;
            // 
            // btnImportFromWorkOrderForPlanning
            // 
            this.btnImportFromWorkOrderForPlanning.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportFromWorkOrderForPlanning.Location = new System.Drawing.Point(646, 3);
            this.btnImportFromWorkOrderForPlanning.Name = "btnImportFromWorkOrderForPlanning";
            this.btnImportFromWorkOrderForPlanning.Size = new System.Drawing.Size(290, 30);
            this.btnImportFromWorkOrderForPlanning.TabIndex = 1;
            this.btnImportFromWorkOrderForPlanning.Text = "Import From Work Order For Planning";
            this.btnImportFromWorkOrderForPlanning.UseVisualStyleBackColor = true;
            // 
            // btnExcludeSetting
            // 
            this.btnExcludeSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnExcludeSetting.Location = new System.Drawing.Point(505, 3);
            this.btnExcludeSetting.Name = "btnExcludeSetting";
            this.btnExcludeSetting.Size = new System.Drawing.Size(137, 30);
            this.btnExcludeSetting.TabIndex = 0;
            this.btnExcludeSetting.Text = "Exclude Setting";
            this.btnExcludeSetting.UseVisualStyleBackColor = true;
            // 
            // displayBoxStyle
            // 
            this.displayBoxStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxStyle.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
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
            this.displayBoxSP.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
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
            // labelMSG
            // 
            this.labelMSG.AutoSize = true;
            this.labelMSG.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.labelMSG.ForeColor = System.Drawing.Color.Red;
            this.labelMSG.Location = new System.Drawing.Point(3, 38);
            this.labelMSG.Name = "labelMSG";
            this.labelMSG.Size = new System.Drawing.Size(318, 22);
            this.labelMSG.TabIndex = 13;
            this.labelMSG.Text = "Marker/EachCons Marker Different";
            // 
            // btnBatchAssign
            // 
            this.btnBatchAssign.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBatchAssign.Location = new System.Drawing.Point(327, 34);
            this.btnBatchAssign.Name = "btnBatchAssign";
            this.btnBatchAssign.Size = new System.Drawing.Size(113, 30);
            this.btnBatchAssign.TabIndex = 2;
            this.btnBatchAssign.Text = "Batch Assign";
            this.btnBatchAssign.UseVisualStyleBackColor = true;
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
            // 
            // btnDownload
            // 
            this.btnDownload.Image = global::Sci.Production.Cutting.Properties.Resources.download;
            this.btnDownload.Location = new System.Drawing.Point(564, 37);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(25, 25);
            this.btnDownload.TabIndex = 4;
            this.btnDownload.UseVisualStyleBackColor = true;
            // 
            // btnImportMarkerLectra
            // 
            this.btnImportMarkerLectra.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportMarkerLectra.Location = new System.Drawing.Point(595, 34);
            this.btnImportMarkerLectra.Name = "btnImportMarkerLectra";
            this.btnImportMarkerLectra.Size = new System.Drawing.Size(175, 30);
            this.btnImportMarkerLectra.TabIndex = 14;
            this.btnImportMarkerLectra.Text = "Import Marker(Lectra)";
            this.btnImportMarkerLectra.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnEdit.Location = new System.Drawing.Point(772, 34);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(61, 30);
            this.btnEdit.TabIndex = 15;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
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
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(3, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Description";
            // 
            // displayBoxFabricTypeRefno
            // 
            this.displayBoxFabricTypeRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxFabricTypeRefno.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayBoxFabricTypeRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxFabricTypeRefno.Location = new System.Drawing.Point(129, 3);
            this.displayBoxFabricTypeRefno.Name = "displayBoxFabricTypeRefno";
            this.displayBoxFabricTypeRefno.Size = new System.Drawing.Size(212, 23);
            this.displayBoxFabricTypeRefno.TabIndex = 13;
            // 
            // displayBoxDescription
            // 
            this.displayBoxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxDescription.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayBoxDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxDescription.Location = new System.Drawing.Point(129, 29);
            this.displayBoxDescription.Multiline = true;
            this.displayBoxDescription.Name = "displayBoxDescription";
            this.displayBoxDescription.Size = new System.Drawing.Size(212, 70);
            this.displayBoxDescription.TabIndex = 14;
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
            // txtUnitCons1
            // 
            this.txtUnitCons1.BackColor = System.Drawing.Color.White;
            this.txtUnitCons1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtUnitCons1.Location = new System.Drawing.Point(129, 102);
            this.txtUnitCons1.Name = "txtUnitCons1";
            this.txtUnitCons1.Size = new System.Drawing.Size(96, 23);
            this.txtUnitCons1.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(229, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 17);
            this.label5.TabIndex = 17;
            this.label5.Text = "/";
            // 
            // txtUnitCons2
            // 
            this.txtUnitCons2.BackColor = System.Drawing.Color.White;
            this.txtUnitCons2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtUnitCons2.Location = new System.Drawing.Point(245, 102);
            this.txtUnitCons2.Name = "txtUnitCons2";
            this.txtUnitCons2.Size = new System.Drawing.Size(96, 23);
            this.txtUnitCons2.TabIndex = 18;
            // 
            // displayBoxTotalCutQty
            // 
            this.displayBoxTotalCutQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxTotalCutQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayBoxTotalCutQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxTotalCutQty.Location = new System.Drawing.Point(129, 128);
            this.displayBoxTotalCutQty.Name = "displayBoxTotalCutQty";
            this.displayBoxTotalCutQty.Size = new System.Drawing.Size(212, 23);
            this.displayBoxTotalCutQty.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(3, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 23);
            this.label6.TabIndex = 19;
            this.label6.Text = "Total Cut Qty";
            // 
            // displayBoxTtlDistQty
            // 
            this.displayBoxTtlDistQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxTtlDistQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayBoxTtlDistQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxTtlDistQty.Location = new System.Drawing.Point(129, 154);
            this.displayBoxTtlDistQty.Name = "displayBoxTtlDistQty";
            this.displayBoxTtlDistQty.Size = new System.Drawing.Size(212, 23);
            this.displayBoxTtlDistQty.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(3, 154);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 23);
            this.label7.TabIndex = 21;
            this.label7.Text = "Ttl. Dist. Qty.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1, 185);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 17);
            this.label8.TabIndex = 23;
            this.label8.Text = "Size Ratio";
            // 
            // gridSizeRatio
            // 
            this.gridSizeRatio.AllowUserToAddRows = false;
            this.gridSizeRatio.AllowUserToDeleteRows = false;
            this.gridSizeRatio.AllowUserToResizeRows = false;
            this.gridSizeRatio.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSizeRatio.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridSizeRatio.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridSizeRatio.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridSizeRatio.DefaultCellStyle = dataGridViewCellStyle5;
            this.gridSizeRatio.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSizeRatio.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSizeRatio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSizeRatio.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSizeRatio.Location = new System.Drawing.Point(3, 205);
            this.gridSizeRatio.Name = "gridSizeRatio";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridSizeRatio.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.gridSizeRatio.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSizeRatio.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSizeRatio.RowTemplate.Height = 24;
            this.gridSizeRatio.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSizeRatio.ShowCellToolTips = false;
            this.gridSizeRatio.Size = new System.Drawing.Size(123, 95);
            this.gridSizeRatio.TabIndex = 24;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(126, 185);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(116, 17);
            this.label9.TabIndex = 25;
            this.label9.Text = "Spreading Fabric";
            // 
            // gridSpreadingFabric
            // 
            this.gridSpreadingFabric.AllowUserToAddRows = false;
            this.gridSpreadingFabric.AllowUserToDeleteRows = false;
            this.gridSpreadingFabric.AllowUserToResizeRows = false;
            this.gridSpreadingFabric.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSpreadingFabric.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSpreadingFabric.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSpreadingFabric.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSpreadingFabric.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSpreadingFabric.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSpreadingFabric.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSpreadingFabric.Location = new System.Drawing.Point(129, 205);
            this.gridSpreadingFabric.Name = "gridSpreadingFabric";
            this.gridSpreadingFabric.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSpreadingFabric.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSpreadingFabric.RowTemplate.Height = 24;
            this.gridSpreadingFabric.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSpreadingFabric.ShowCellToolTips = false;
            this.gridSpreadingFabric.Size = new System.Drawing.Size(212, 95);
            this.gridSpreadingFabric.TabIndex = 26;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(-1, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(119, 17);
            this.label10.TabIndex = 27;
            this.label10.Text = "Distribute To SP#";
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
            this.gridDistributeToSP.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDistributeToSP.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDistributeToSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDistributeToSP.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDistributeToSP.Location = new System.Drawing.Point(1, 20);
            this.gridDistributeToSP.Name = "gridDistributeToSP";
            this.gridDistributeToSP.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDistributeToSP.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDistributeToSP.RowTemplate.Height = 24;
            this.gridDistributeToSP.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDistributeToSP.ShowCellToolTips = false;
            this.gridDistributeToSP.Size = new System.Drawing.Size(338, 130);
            this.gridDistributeToSP.TabIndex = 28;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(0, 1);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(110, 17);
            this.label11.TabIndex = 29;
            this.label11.Text = "Qty Break Down";
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
            this.gridQtyBreakDown.Size = new System.Drawing.Size(338, 136);
            this.gridQtyBreakDown.TabIndex = 30;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 304);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label10);
            this.splitContainer1.Panel1.Controls.Add(this.gridDistributeToSP);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label11);
            this.splitContainer1.Panel2.Controls.Add(this.gridQtyBreakDown);
            this.splitContainer1.Size = new System.Drawing.Size(340, 310);
            this.splitContainer1.SplitterDistance = 149;
            this.splitContainer1.TabIndex = 31;
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
            // 
            // btnAutoCut
            // 
            this.btnAutoCut.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnAutoCut.Location = new System.Drawing.Point(95, 6);
            this.btnAutoCut.Name = "btnAutoCut";
            this.btnAutoCut.Size = new System.Drawing.Size(92, 30);
            this.btnAutoCut.TabIndex = 17;
            this.btnAutoCut.Text = "Auto Cut#";
            this.btnAutoCut.UseVisualStyleBackColor = true;
            // 
            // btnAllSPDistribute
            // 
            this.btnAllSPDistribute.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnAllSPDistribute.Location = new System.Drawing.Point(243, 6);
            this.btnAllSPDistribute.Name = "btnAllSPDistribute";
            this.btnAllSPDistribute.Size = new System.Drawing.Size(145, 30);
            this.btnAllSPDistribute.TabIndex = 18;
            this.btnAllSPDistribute.Text = "All SP# Distribute";
            this.btnAllSPDistribute.UseVisualStyleBackColor = true;
            // 
            // btnDistributeThisCutRef
            // 
            this.btnDistributeThisCutRef.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnDistributeThisCutRef.Location = new System.Drawing.Point(388, 6);
            this.btnDistributeThisCutRef.Name = "btnDistributeThisCutRef";
            this.btnDistributeThisCutRef.Size = new System.Drawing.Size(179, 30);
            this.btnDistributeThisCutRef.TabIndex = 19;
            this.btnDistributeThisCutRef.Text = "Distribute This CutRef";
            this.btnDistributeThisCutRef.UseVisualStyleBackColor = true;
            // 
            // btnCutPartsCheck
            // 
            this.btnCutPartsCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCutPartsCheck.Location = new System.Drawing.Point(568, 6);
            this.btnCutPartsCheck.Name = "btnCutPartsCheck";
            this.btnCutPartsCheck.Size = new System.Drawing.Size(133, 30);
            this.btnCutPartsCheck.TabIndex = 20;
            this.btnCutPartsCheck.Text = "Cut Parts Check";
            this.btnCutPartsCheck.UseVisualStyleBackColor = true;
            // 
            // btnCutPartsCheckSummary
            // 
            this.btnCutPartsCheckSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCutPartsCheckSummary.Location = new System.Drawing.Point(702, 6);
            this.btnCutPartsCheckSummary.Name = "btnCutPartsCheckSummary";
            this.btnCutPartsCheckSummary.Size = new System.Drawing.Size(204, 30);
            this.btnCutPartsCheckSummary.TabIndex = 21;
            this.btnCutPartsCheckSummary.Text = "Cut Parts Check Summary";
            this.btnCutPartsCheckSummary.UseVisualStyleBackColor = true;
            // 
            // btnHistory
            // 
            this.btnHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnHistory.Location = new System.Drawing.Point(907, 7);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(70, 28);
            this.btnHistory.TabIndex = 22;
            this.btnHistory.Text = "History";
            this.btnHistory.UseVisualStyleBackColor = true;
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
            // 
            // P09
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1288, 717);
            this.DefaultOrder = "ID";
            this.ExpressQuery = true;
            this.GridAlias = "WorkOrderForOutput";
            this.GridNew = 0;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.KeyField1 = "ID";
            this.Name = "P09";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P09. WorkOrder For Output";
            this.WorkAlias = "Cutting";
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
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSizeRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSpreadingFabric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDistributeToSP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridQtyBreakDown)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
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
        private Win.UI.Button btnExcludeSetting;
        private Win.UI.Button btnImportFromWorkOrderForPlanning;
        private System.Windows.Forms.Label labelMSG;
        private Win.UI.Button btnBatchAssign;
        private Win.UI.Button btnImportMarker;
        private Win.UI.Button btnDownload;
        private Win.UI.Button btnImportMarkerLectra;
        private Win.UI.Button btnEdit;
        private Win.UI.DisplayBox displayBoxDescription;
        private Win.UI.DisplayBox displayBoxFabricTypeRefno;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtUnitCons2;
        private System.Windows.Forms.Label label5;
        private Win.UI.TextBox txtUnitCons1;
        private Win.UI.Label label4;
        private Win.UI.Grid gridSpreadingFabric;
        private System.Windows.Forms.Label label9;
        private Win.UI.Grid gridSizeRatio;
        private System.Windows.Forms.Label label8;
        private Win.UI.DisplayBox displayBoxTtlDistQty;
        private Win.UI.Label label7;
        private Win.UI.DisplayBox displayBoxTotalCutQty;
        private Win.UI.Label label6;
        private System.Windows.Forms.Label label10;
        private Win.UI.Grid gridQtyBreakDown;
        private System.Windows.Forms.Label label11;
        private Win.UI.Grid gridDistributeToSP;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Button btnAutoRef;
        private Win.UI.Button btnAutoCut;
        private Win.UI.Button btnToExcel;
        private Win.UI.Button btnQtyBreakdown;
        private Win.UI.Button btnHistory;
        private Win.UI.Button btnCutPartsCheckSummary;
        private Win.UI.Button btnCutPartsCheck;
        private Win.UI.Button btnDistributeThisCutRef;
        private Win.UI.Button btnAllSPDistribute;
    }
}
