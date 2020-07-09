namespace Sci.Production.Packing
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.displayFactory = new Sci.Win.UI.DisplayBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.displayPONo = new Sci.Win.UI.DisplayBox();
            this.displayOrder = new Sci.Win.UI.DisplayBox();
            this.labelPONo = new Sci.Win.UI.Label();
            this.labelOrder = new Sci.Win.UI.Label();
            this.displayM = new Sci.Win.UI.DisplayBox();
            this.labelM = new Sci.Win.UI.Label();
            this.numOrderQty = new Sci.Win.UI.NumericBox();
            this.txtcountryDestination = new Sci.Production.Class.Txtcountry();
            this.labelOrderQty = new Sci.Win.UI.Label();
            this.labelDestination = new Sci.Win.UI.Label();
            this.displayBuyer = new Sci.Win.UI.DisplayBox();
            this.displayBrand = new Sci.Win.UI.DisplayBox();
            this.labelBuyer = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.displayStyle = new Sci.Win.UI.DisplayBox();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.panel7 = new Sci.Win.UI.Panel();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel6 = new Sci.Win.UI.Panel();
            this.btnStartToScan = new Sci.Win.UI.Button();
            this.btnUncomplete = new Sci.Win.UI.Button();
            this.txtQuickSelectCTN = new Sci.Win.UI.TextBox();
            this.labelQuickSelectCTN = new Sci.Win.UI.Label();
            this.comboPackingNoFilter = new Sci.Win.UI.ComboBox();
            this.labelPackingNoFilter = new Sci.Win.UI.Label();
            this.checkOnlyNotYetScanComplete = new Sci.Win.UI.CheckBox();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 546);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(960, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 546);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.displayFactory);
            this.panel3.Controls.Add(this.labelFactory);
            this.panel3.Controls.Add(this.displayPONo);
            this.panel3.Controls.Add(this.displayOrder);
            this.panel3.Controls.Add(this.labelPONo);
            this.panel3.Controls.Add(this.labelOrder);
            this.panel3.Controls.Add(this.displayM);
            this.panel3.Controls.Add(this.labelM);
            this.panel3.Controls.Add(this.numOrderQty);
            this.panel3.Controls.Add(this.txtcountryDestination);
            this.panel3.Controls.Add(this.labelOrderQty);
            this.panel3.Controls.Add(this.labelDestination);
            this.panel3.Controls.Add(this.displayBuyer);
            this.panel3.Controls.Add(this.displayBrand);
            this.panel3.Controls.Add(this.labelBuyer);
            this.panel3.Controls.Add(this.labelBrand);
            this.panel3.Controls.Add(this.displaySeason);
            this.panel3.Controls.Add(this.displayStyle);
            this.panel3.Controls.Add(this.txtSP);
            this.panel3.Controls.Add(this.labelSeason);
            this.panel3.Controls.Add(this.labelStyle);
            this.panel3.Controls.Add(this.labelSP);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(950, 90);
            this.panel3.TabIndex = 3;
            // 
            // displayFactory
            // 
            this.displayFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFactory.Location = new System.Drawing.Point(436, 9);
            this.displayFactory.Name = "displayFactory";
            this.displayFactory.Size = new System.Drawing.Size(64, 23);
            this.displayFactory.TabIndex = 21;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(358, 9);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(75, 23);
            this.labelFactory.TabIndex = 20;
            this.labelFactory.Text = "Factory";
            // 
            // displayPONo
            // 
            this.displayPONo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPONo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPONo.Location = new System.Drawing.Point(746, 63);
            this.displayPONo.Name = "displayPONo";
            this.displayPONo.Size = new System.Drawing.Size(188, 23);
            this.displayPONo.TabIndex = 19;
            // 
            // displayOrder
            // 
            this.displayOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayOrder.Location = new System.Drawing.Point(746, 36);
            this.displayOrder.Name = "displayOrder";
            this.displayOrder.Size = new System.Drawing.Size(139, 23);
            this.displayOrder.TabIndex = 18;
            // 
            // labelPONo
            // 
            this.labelPONo.Lines = 0;
            this.labelPONo.Location = new System.Drawing.Point(685, 63);
            this.labelPONo.Name = "labelPONo";
            this.labelPONo.Size = new System.Drawing.Size(57, 23);
            this.labelPONo.TabIndex = 17;
            this.labelPONo.Text = "P.O. No.";
            // 
            // labelOrder
            // 
            this.labelOrder.Lines = 0;
            this.labelOrder.Location = new System.Drawing.Point(685, 36);
            this.labelOrder.Name = "labelOrder";
            this.labelOrder.Size = new System.Drawing.Size(57, 23);
            this.labelOrder.TabIndex = 16;
            this.labelOrder.Text = "Order#";
            // 
            // displayM
            // 
            this.displayM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayM.Location = new System.Drawing.Point(257, 9);
            this.displayM.Name = "displayM";
            this.displayM.Size = new System.Drawing.Size(64, 23);
            this.displayM.TabIndex = 15;
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(211, 9);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(42, 23);
            this.labelM.TabIndex = 14;
            this.labelM.Text = "M";
            // 
            // numOrderQty
            // 
            this.numOrderQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numOrderQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numOrderQty.IsSupportEditMode = false;
            this.numOrderQty.Location = new System.Drawing.Point(437, 63);
            this.numOrderQty.Name = "numOrderQty";
            this.numOrderQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numOrderQty.ReadOnly = true;
            this.numOrderQty.Size = new System.Drawing.Size(64, 23);
            this.numOrderQty.TabIndex = 13;
            this.numOrderQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtcountryDestination
            // 
            this.txtcountryDestination.DisplayBox1Binding = "";
            this.txtcountryDestination.Location = new System.Drawing.Point(437, 36);
            this.txtcountryDestination.Name = "txtcountryDestination";
            this.txtcountryDestination.Size = new System.Drawing.Size(232, 22);
            this.txtcountryDestination.TabIndex = 12;
            this.txtcountryDestination.TextBox1Binding = "";
            // 
            // labelOrderQty
            // 
            this.labelOrderQty.Lines = 0;
            this.labelOrderQty.Location = new System.Drawing.Point(358, 63);
            this.labelOrderQty.Name = "labelOrderQty";
            this.labelOrderQty.Size = new System.Drawing.Size(75, 23);
            this.labelOrderQty.TabIndex = 11;
            this.labelOrderQty.Text = "Order Qty";
            // 
            // labelDestination
            // 
            this.labelDestination.Lines = 0;
            this.labelDestination.Location = new System.Drawing.Point(358, 36);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(75, 23);
            this.labelDestination.TabIndex = 10;
            this.labelDestination.Text = "Destination";
            // 
            // displayBuyer
            // 
            this.displayBuyer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBuyer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBuyer.Location = new System.Drawing.Point(257, 63);
            this.displayBuyer.Name = "displayBuyer";
            this.displayBuyer.Size = new System.Drawing.Size(80, 23);
            this.displayBuyer.TabIndex = 9;
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(257, 36);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(80, 23);
            this.displayBrand.TabIndex = 8;
            // 
            // labelBuyer
            // 
            this.labelBuyer.Lines = 0;
            this.labelBuyer.Location = new System.Drawing.Point(211, 63);
            this.labelBuyer.Name = "labelBuyer";
            this.labelBuyer.Size = new System.Drawing.Size(42, 23);
            this.labelBuyer.TabIndex = 7;
            this.labelBuyer.Text = "Buyer";
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(211, 36);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(42, 23);
            this.labelBrand.TabIndex = 6;
            this.labelBrand.Text = "Brand";
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(59, 63);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(74, 23);
            this.displaySeason.TabIndex = 5;
            // 
            // displayStyle
            // 
            this.displayStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyle.Location = new System.Drawing.Point(59, 36);
            this.displayStyle.Name = "displayStyle";
            this.displayStyle.Size = new System.Drawing.Size(133, 23);
            this.displayStyle.TabIndex = 4;
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(59, 9);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(130, 23);
            this.txtSP.TabIndex = 3;
            this.txtSP.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSP_Validating);
            this.txtSP.Validated += new System.EventHandler(this.TxtSP_Validated);
            // 
            // labelSeason
            // 
            this.labelSeason.Lines = 0;
            this.labelSeason.Location = new System.Drawing.Point(3, 63);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(52, 23);
            this.labelSeason.TabIndex = 2;
            this.labelSeason.Text = "Season";
            // 
            // labelStyle
            // 
            this.labelStyle.Lines = 0;
            this.labelStyle.Location = new System.Drawing.Point(3, 36);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(52, 23);
            this.labelStyle.TabIndex = 1;
            this.labelStyle.Text = "Style";
            // 
            // labelSP
            // 
            this.labelSP.Lines = 0;
            this.labelSP.Location = new System.Drawing.Point(3, 9);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(52, 23);
            this.labelSP.TabIndex = 0;
            this.labelSP.Text = "SP#";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 504);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(950, 42);
            this.panel4.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(866, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 90);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(950, 414);
            this.panel5.TabIndex = 5;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.gridDetail);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 42);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(950, 372);
            this.panel7.TabIndex = 1;
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridDetail.RowHeadersVisible = false;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.Size = new System.Drawing.Size(950, 372);
            this.gridDetail.TabIndex = 0;
            this.gridDetail.TabStop = false;
            this.gridDetail.RowSelecting += new System.EventHandler<Ict.Win.UI.DataGridViewRowSelectingEventArgs>(this.Grid1_RowSelecting);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnStartToScan);
            this.panel6.Controls.Add(this.btnUncomplete);
            this.panel6.Controls.Add(this.txtQuickSelectCTN);
            this.panel6.Controls.Add(this.labelQuickSelectCTN);
            this.panel6.Controls.Add(this.comboPackingNoFilter);
            this.panel6.Controls.Add(this.labelPackingNoFilter);
            this.panel6.Controls.Add(this.checkOnlyNotYetScanComplete);
            this.panel6.Controls.Add(this.shapeContainer1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(950, 42);
            this.panel6.TabIndex = 0;
            // 
            // btnStartToScan
            // 
            this.btnStartToScan.Location = new System.Drawing.Point(831, 8);
            this.btnStartToScan.Name = "btnStartToScan";
            this.btnStartToScan.Size = new System.Drawing.Size(113, 30);
            this.btnStartToScan.TabIndex = 6;
            this.btnStartToScan.Text = "Start to Scan";
            this.btnStartToScan.UseVisualStyleBackColor = true;
            this.btnStartToScan.Click += new System.EventHandler(this.BtnStartToScan_Click);
            // 
            // btnUncomplete
            // 
            this.btnUncomplete.Location = new System.Drawing.Point(720, 8);
            this.btnUncomplete.Name = "btnUncomplete";
            this.btnUncomplete.Size = new System.Drawing.Size(104, 30);
            this.btnUncomplete.TabIndex = 5;
            this.btnUncomplete.Text = "Uncomplete";
            this.btnUncomplete.UseVisualStyleBackColor = true;
            this.btnUncomplete.Click += new System.EventHandler(this.BtnUncomplete_Click);
            // 
            // txtQuickSelectCTN
            // 
            this.txtQuickSelectCTN.BackColor = System.Drawing.Color.White;
            this.txtQuickSelectCTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtQuickSelectCTN.Location = new System.Drawing.Point(611, 11);
            this.txtQuickSelectCTN.Name = "txtQuickSelectCTN";
            this.txtQuickSelectCTN.Size = new System.Drawing.Size(65, 23);
            this.txtQuickSelectCTN.TabIndex = 4;
            this.txtQuickSelectCTN.TextChanged += new System.EventHandler(this.TxtQuickSelectCTN_TextChanged);
            // 
            // labelQuickSelectCTN
            // 
            this.labelQuickSelectCTN.Lines = 0;
            this.labelQuickSelectCTN.Location = new System.Drawing.Point(484, 11);
            this.labelQuickSelectCTN.Name = "labelQuickSelectCTN";
            this.labelQuickSelectCTN.Size = new System.Drawing.Size(123, 23);
            this.labelQuickSelectCTN.TabIndex = 3;
            this.labelQuickSelectCTN.Text = "Quick Select CTN#";
            // 
            // comboPackingNoFilter
            // 
            this.comboPackingNoFilter.BackColor = System.Drawing.Color.White;
            this.comboPackingNoFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboPackingNoFilter.FormattingEnabled = true;
            this.comboPackingNoFilter.IsSupportUnselect = true;
            this.comboPackingNoFilter.Location = new System.Drawing.Point(323, 11);
            this.comboPackingNoFilter.Name = "comboPackingNoFilter";
            this.comboPackingNoFilter.Size = new System.Drawing.Size(136, 24);
            this.comboPackingNoFilter.TabIndex = 2;
            this.comboPackingNoFilter.SelectedIndexChanged += new System.EventHandler(this.ComboPackingNoFilter_SelectedIndexChanged);
            // 
            // labelPackingNoFilter
            // 
            this.labelPackingNoFilter.Lines = 0;
            this.labelPackingNoFilter.Location = new System.Drawing.Point(211, 11);
            this.labelPackingNoFilter.Name = "labelPackingNoFilter";
            this.labelPackingNoFilter.Size = new System.Drawing.Size(108, 23);
            this.labelPackingNoFilter.TabIndex = 1;
            this.labelPackingNoFilter.Text = "Packing No Filter";
            // 
            // checkOnlyNotYetScanComplete
            // 
            this.checkOnlyNotYetScanComplete.AutoSize = true;
            this.checkOnlyNotYetScanComplete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkOnlyNotYetScanComplete.Location = new System.Drawing.Point(3, 11);
            this.checkOnlyNotYetScanComplete.Name = "checkOnlyNotYetScanComplete";
            this.checkOnlyNotYetScanComplete.Size = new System.Drawing.Size(198, 21);
            this.checkOnlyNotYetScanComplete.TabIndex = 0;
            this.checkOnlyNotYetScanComplete.Text = "Only not yet scan complete";
            this.checkOnlyNotYetScanComplete.UseVisualStyleBackColor = true;
            this.checkOnlyNotYetScanComplete.CheckedChanged += new System.EventHandler(this.CheckOnlyNotYetScanComplete_CheckedChanged);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(950, 42);
            this.shapeContainer1.TabIndex = 7;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 0;
            this.lineShape1.X2 = 950;
            this.lineShape1.Y1 = 2;
            this.lineShape1.Y2 = 2;
            // 
            // P09
            // 
            this.ClientSize = new System.Drawing.Size(970, 546);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P09";
            this.Text = "P09. Scan & Pack";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.DisplayBox displayFactory;
        private Win.UI.Label labelFactory;
        private Win.UI.DisplayBox displayPONo;
        private Win.UI.DisplayBox displayOrder;
        private Win.UI.Label labelPONo;
        private Win.UI.Label labelOrder;
        private Win.UI.DisplayBox displayM;
        private Win.UI.Label labelM;
        private Win.UI.NumericBox numOrderQty;
        private Class.Txtcountry txtcountryDestination;
        private Win.UI.Label labelOrderQty;
        private Win.UI.Label labelDestination;
        private Win.UI.DisplayBox displayBuyer;
        private Win.UI.DisplayBox displayBrand;
        private Win.UI.Label labelBuyer;
        private Win.UI.Label labelBrand;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.DisplayBox displayStyle;
        private Win.UI.TextBox txtSP;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelStyle;
        private Win.UI.Label labelSP;
        private Win.UI.Panel panel4;
        private Win.UI.Button btnClose;
        private Win.UI.Panel panel5;
        private Win.UI.Panel panel7;
        private Win.UI.Grid gridDetail;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel6;
        private Win.UI.Button btnStartToScan;
        private Win.UI.Button btnUncomplete;
        private Win.UI.TextBox txtQuickSelectCTN;
        private Win.UI.Label labelQuickSelectCTN;
        private Win.UI.ComboBox comboPackingNoFilter;
        private Win.UI.Label labelPackingNoFilter;
        private Win.UI.CheckBox checkOnlyNotYetScanComplete;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
    }
}
