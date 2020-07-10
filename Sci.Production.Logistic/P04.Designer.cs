namespace Sci.Production.Logistic
{
    partial class P04
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
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.numTTLCTNQty = new Sci.Win.UI.NumericBox();
            this.label2 = new Sci.Win.UI.Label();
            this.numSelectQty = new Sci.Win.UI.NumericBox();
            this.label1 = new Sci.Win.UI.Label();
            this.btnImportFromBarcode = new Sci.Win.UI.Button();
            this.comboFilter2 = new Sci.Win.UI.ComboBox();
            this.comboFilter = new Sci.Win.UI.ComboBox();
            this.labelFilter = new Sci.Win.UI.Label();
            this.btnUpdateAllLocation = new Sci.Win.UI.Button();
            this.txtcloglocationLocationNo = new Sci.Production.Class.Txtcloglocation();
            this.labelLocationNo = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.txtPONoStart = new Sci.Win.UI.TextBox();
            this.txtPONoEnd = new Sci.Win.UI.TextBox();
            this.label5 = new Sci.Win.UI.Label();
            this.txtPackIDEnd = new Sci.Win.UI.TextBox();
            this.textTransferSlipNo = new Sci.Win.UI.TextBox();
            this.txtPackIDStart = new Sci.Win.UI.TextBox();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.labelPONo = new Sci.Win.UI.Label();
            this.labelTransferSlipNo = new Sci.Win.UI.Label();
            this.labelPackID = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnPrintMoveTicket = new Sci.Win.UI.Button();
            this.comboRequestby = new Sci.Win.UI.ComboBox();
            this.labelRequestby = new Sci.Win.UI.Label();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridPackID = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPackID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 563);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(907, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 563);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.numTTLCTNQty);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.numSelectQty);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.btnImportFromBarcode);
            this.panel3.Controls.Add(this.comboFilter2);
            this.panel3.Controls.Add(this.comboFilter);
            this.panel3.Controls.Add(this.labelFilter);
            this.panel3.Controls.Add(this.btnUpdateAllLocation);
            this.panel3.Controls.Add(this.txtcloglocationLocationNo);
            this.panel3.Controls.Add(this.labelLocationNo);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.txtPONoStart);
            this.panel3.Controls.Add(this.txtPONoEnd);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.txtPackIDEnd);
            this.panel3.Controls.Add(this.textTransferSlipNo);
            this.panel3.Controls.Add(this.txtPackIDStart);
            this.panel3.Controls.Add(this.txtSPNoEnd);
            this.panel3.Controls.Add(this.txtSPNoStart);
            this.panel3.Controls.Add(this.labelPONo);
            this.panel3.Controls.Add(this.labelTransferSlipNo);
            this.panel3.Controls.Add(this.labelPackID);
            this.panel3.Controls.Add(this.labelSPNo);
            this.panel3.Controls.Add(this.shapeContainer1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(902, 106);
            this.panel3.TabIndex = 3;
            // 
            // numTTLCTNQty
            // 
            this.numTTLCTNQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTTLCTNQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTTLCTNQty.IsSupportEditMode = false;
            this.numTTLCTNQty.Location = new System.Drawing.Point(858, 35);
            this.numTTLCTNQty.Name = "numTTLCTNQty";
            this.numTTLCTNQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTTLCTNQty.ReadOnly = true;
            this.numTTLCTNQty.Size = new System.Drawing.Size(40, 23);
            this.numTTLCTNQty.TabIndex = 26;
            this.numTTLCTNQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(755, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 25;
            this.label2.Text = "Total CTN Qty:";
            // 
            // numSelectQty
            // 
            this.numSelectQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numSelectQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numSelectQty.IsSupportEditMode = false;
            this.numSelectQty.Location = new System.Drawing.Point(712, 35);
            this.numSelectQty.Name = "numSelectQty";
            this.numSelectQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSelectQty.ReadOnly = true;
            this.numSelectQty.Size = new System.Drawing.Size(40, 23);
            this.numSelectQty.TabIndex = 24;
            this.numSelectQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(593, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 23);
            this.label1.TabIndex = 23;
            this.label1.Text = "Selected CTN Qty";
            // 
            // btnImportFromBarcode
            // 
            this.btnImportFromBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportFromBarcode.Location = new System.Drawing.Point(715, 69);
            this.btnImportFromBarcode.Name = "btnImportFromBarcode";
            this.btnImportFromBarcode.Size = new System.Drawing.Size(176, 30);
            this.btnImportFromBarcode.TabIndex = 22;
            this.btnImportFromBarcode.Text = "Import From Barcode";
            this.btnImportFromBarcode.UseVisualStyleBackColor = true;
            this.btnImportFromBarcode.Click += new System.EventHandler(this.BtnImportFromBarcode_Click);
            // 
            // comboFilter2
            // 
            this.comboFilter2.BackColor = System.Drawing.Color.White;
            this.comboFilter2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFilter2.FormattingEnabled = true;
            this.comboFilter2.IsSupportUnselect = true;
            this.comboFilter2.Location = new System.Drawing.Point(543, 73);
            this.comboFilter2.Name = "comboFilter2";
            this.comboFilter2.OldText = "";
            this.comboFilter2.Size = new System.Drawing.Size(166, 24);
            this.comboFilter2.TabIndex = 12;
            this.comboFilter2.SelectedIndexChanged += new System.EventHandler(this.ComboFilter2_SelectedIndexChanged);
            // 
            // comboFilter
            // 
            this.comboFilter.BackColor = System.Drawing.Color.White;
            this.comboFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFilter.FormattingEnabled = true;
            this.comboFilter.IsSupportUnselect = true;
            this.comboFilter.Location = new System.Drawing.Point(453, 73);
            this.comboFilter.Name = "comboFilter";
            this.comboFilter.OldText = "";
            this.comboFilter.Size = new System.Drawing.Size(85, 24);
            this.comboFilter.TabIndex = 11;
            this.comboFilter.SelectedIndexChanged += new System.EventHandler(this.ComboFilter_SelectedIndexChanged);
            // 
            // labelFilter
            // 
            this.labelFilter.Location = new System.Drawing.Point(409, 74);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(41, 23);
            this.labelFilter.TabIndex = 21;
            this.labelFilter.Text = "Filter";
            // 
            // btnUpdateAllLocation
            // 
            this.btnUpdateAllLocation.Location = new System.Drawing.Point(179, 70);
            this.btnUpdateAllLocation.Name = "btnUpdateAllLocation";
            this.btnUpdateAllLocation.Size = new System.Drawing.Size(159, 30);
            this.btnUpdateAllLocation.TabIndex = 10;
            this.btnUpdateAllLocation.Text = "Update All Location";
            this.btnUpdateAllLocation.UseVisualStyleBackColor = true;
            this.btnUpdateAllLocation.Click += new System.EventHandler(this.BtnUpdateAllLocation_Click);
            // 
            // txtcloglocationLocationNo
            // 
            this.txtcloglocationLocationNo.BackColor = System.Drawing.Color.White;
            this.txtcloglocationLocationNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcloglocationLocationNo.IsSupportSytsemContextMenu = false;
            this.txtcloglocationLocationNo.Location = new System.Drawing.Point(92, 74);
            this.txtcloglocationLocationNo.MDivisionObjectName = null;
            this.txtcloglocationLocationNo.Name = "txtcloglocationLocationNo";
            this.txtcloglocationLocationNo.Size = new System.Drawing.Size(80, 23);
            this.txtcloglocationLocationNo.TabIndex = 9;
            // 
            // labelLocationNo
            // 
            this.labelLocationNo.Location = new System.Drawing.Point(4, 74);
            this.labelLocationNo.Name = "labelLocationNo";
            this.labelLocationNo.Size = new System.Drawing.Size(84, 23);
            this.labelLocationNo.TabIndex = 18;
            this.labelLocationNo.Text = "Location No.";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(689, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 8;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(185, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 23);
            this.label7.TabIndex = 15;
            this.label7.Text = "~";
            this.label7.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(543, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 23);
            this.label8.TabIndex = 14;
            this.label8.Text = "~";
            this.label8.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label8.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtPONoStart
            // 
            this.txtPONoStart.BackColor = System.Drawing.Color.White;
            this.txtPONoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPONoStart.Location = new System.Drawing.Point(62, 36);
            this.txtPONoStart.Name = "txtPONoStart";
            this.txtPONoStart.Size = new System.Drawing.Size(120, 23);
            this.txtPONoStart.TabIndex = 6;
            // 
            // txtPONoEnd
            // 
            this.txtPONoEnd.BackColor = System.Drawing.Color.White;
            this.txtPONoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPONoEnd.Location = new System.Drawing.Point(205, 36);
            this.txtPONoEnd.Name = "txtPONoEnd";
            this.txtPONoEnd.Size = new System.Drawing.Size(120, 23);
            this.txtPONoEnd.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(185, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 23);
            this.label5.TabIndex = 8;
            this.label5.Text = "~";
            this.label5.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label5.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtPackIDEnd
            // 
            this.txtPackIDEnd.BackColor = System.Drawing.Color.White;
            this.txtPackIDEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackIDEnd.Location = new System.Drawing.Point(563, 9);
            this.txtPackIDEnd.Name = "txtPackIDEnd";
            this.txtPackIDEnd.Size = new System.Drawing.Size(120, 23);
            this.txtPackIDEnd.TabIndex = 5;
            // 
            // textTransferSlipNo
            // 
            this.textTransferSlipNo.BackColor = System.Drawing.Color.White;
            this.textTransferSlipNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textTransferSlipNo.Location = new System.Drawing.Point(470, 35);
            this.textTransferSlipNo.Name = "textTransferSlipNo";
            this.textTransferSlipNo.Size = new System.Drawing.Size(120, 23);
            this.textTransferSlipNo.TabIndex = 4;
            // 
            // txtPackIDStart
            // 
            this.txtPackIDStart.BackColor = System.Drawing.Color.White;
            this.txtPackIDStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackIDStart.Location = new System.Drawing.Point(420, 9);
            this.txtPackIDStart.Name = "txtPackIDStart";
            this.txtPackIDStart.Size = new System.Drawing.Size(120, 23);
            this.txtPackIDStart.TabIndex = 4;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(205, 9);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(120, 23);
            this.txtSPNoEnd.TabIndex = 1;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(62, 9);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(120, 23);
            this.txtSPNoStart.TabIndex = 0;
            // 
            // labelPONo
            // 
            this.labelPONo.Location = new System.Drawing.Point(4, 36);
            this.labelPONo.Name = "labelPONo";
            this.labelPONo.Size = new System.Drawing.Size(55, 23);
            this.labelPONo.TabIndex = 3;
            this.labelPONo.Text = "PO#";
            // 
            // labelTransferSlipNo
            // 
            this.labelTransferSlipNo.Location = new System.Drawing.Point(362, 36);
            this.labelTransferSlipNo.Name = "labelTransferSlipNo";
            this.labelTransferSlipNo.Size = new System.Drawing.Size(105, 23);
            this.labelTransferSlipNo.TabIndex = 2;
            this.labelTransferSlipNo.Text = "TransferSlipNo";
            // 
            // labelPackID
            // 
            this.labelPackID.Location = new System.Drawing.Point(362, 9);
            this.labelPackID.Name = "labelPackID";
            this.labelPackID.Size = new System.Drawing.Size(55, 23);
            this.labelPackID.TabIndex = 2;
            this.labelPackID.Text = "Pack ID";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(4, 9);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(55, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP#";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(902, 106);
            this.shapeContainer1.TabIndex = 17;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 2;
            this.lineShape1.X2 = 847;
            this.lineShape1.Y1 = 64;
            this.lineShape1.Y2 = 64;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnCancel);
            this.panel4.Controls.Add(this.btnSave);
            this.panel4.Controls.Add(this.btnPrintMoveTicket);
            this.panel4.Controls.Add(this.comboRequestby);
            this.panel4.Controls.Add(this.labelRequestby);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 514);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(902, 49);
            this.panel4.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(809, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(723, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnPrintMoveTicket
            // 
            this.btnPrintMoveTicket.Location = new System.Drawing.Point(412, 8);
            this.btnPrintMoveTicket.Name = "btnPrintMoveTicket";
            this.btnPrintMoveTicket.Size = new System.Drawing.Size(135, 30);
            this.btnPrintMoveTicket.TabIndex = 3;
            this.btnPrintMoveTicket.Text = "Print Move Ticket";
            this.btnPrintMoveTicket.UseVisualStyleBackColor = true;
            this.btnPrintMoveTicket.Click += new System.EventHandler(this.BtnPrintMoveTicket_Click);
            // 
            // comboRequestby
            // 
            this.comboRequestby.BackColor = System.Drawing.Color.White;
            this.comboRequestby.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboRequestby.FormattingEnabled = true;
            this.comboRequestby.IsSupportUnselect = true;
            this.comboRequestby.Location = new System.Drawing.Point(302, 12);
            this.comboRequestby.Name = "comboRequestby";
            this.comboRequestby.OldText = "";
            this.comboRequestby.Size = new System.Drawing.Size(104, 24);
            this.comboRequestby.TabIndex = 2;
            // 
            // labelRequestby
            // 
            this.labelRequestby.Location = new System.Drawing.Point(224, 13);
            this.labelRequestby.Name = "labelRequestby";
            this.labelRequestby.Size = new System.Drawing.Size(75, 23);
            this.labelRequestby.TabIndex = 0;
            this.labelRequestby.Text = "Request by";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridPackID);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 106);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(902, 408);
            this.panel5.TabIndex = 5;
            // 
            // gridPackID
            // 
            this.gridPackID.AllowUserToAddRows = false;
            this.gridPackID.AllowUserToDeleteRows = false;
            this.gridPackID.AllowUserToResizeRows = false;
            this.gridPackID.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridPackID.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridPackID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPackID.DataSource = this.listControlBindingSource1;
            this.gridPackID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPackID.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridPackID.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridPackID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridPackID.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridPackID.Location = new System.Drawing.Point(0, 0);
            this.gridPackID.Name = "gridPackID";
            this.gridPackID.RowHeadersVisible = false;
            this.gridPackID.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridPackID.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridPackID.RowTemplate.Height = 24;
            this.gridPackID.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPackID.ShowCellToolTips = false;
            this.gridPackID.Size = new System.Drawing.Size(902, 408);
            this.gridPackID.TabIndex = 0;
            this.gridPackID.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // P04
            // 
            this.ClientSize = new System.Drawing.Size(912, 563);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P04";
            this.Text = "P04. Update Location";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridPackID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.ComboBox comboFilter2;
        private Win.UI.ComboBox comboFilter;
        private Win.UI.Label labelFilter;
        private Win.UI.Button btnUpdateAllLocation;
        private Class.Txtcloglocation txtcloglocationLocationNo;
        private Win.UI.Label labelLocationNo;
        private Win.UI.Button btnQuery;
        private Win.UI.Label label7;
        private Win.UI.Label label8;
        private Win.UI.TextBox txtPONoStart;
        private Win.UI.TextBox txtPONoEnd;
        private Win.UI.Label label5;
        private Win.UI.TextBox txtPackIDEnd;
        private Win.UI.TextBox txtPackIDStart;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.Label labelPONo;
        private Win.UI.Label labelPackID;
        private Win.UI.Label labelSPNo;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.Panel panel4;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnPrintMoveTicket;
        private Win.UI.ComboBox comboRequestby;
        private Win.UI.Label labelRequestby;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridPackID;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.TextBox textTransferSlipNo;
        private Win.UI.Label labelTransferSlipNo;
        private Win.UI.Button btnImportFromBarcode;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Win.UI.NumericBox numTTLCTNQty;
        private Win.UI.Label label2;
        private Win.UI.NumericBox numSelectQty;
        private Win.UI.Label label1;
    }
}
