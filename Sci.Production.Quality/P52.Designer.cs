namespace Sci.Production.Quality
{
    partial class P52
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.bs_Material = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.bs_Document = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.bs_Report = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.tabControl1 = new Sci.Win.UI.TabControl();
            this.MaterialDocumentRecord = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grid_Material = new Sci.Win.UI.Grid();
            this.grid_Document = new Sci.Win.UI.Grid();
            this.grid_Report = new Sci.Win.UI.Grid();
            this.comboMaterialType = new System.Windows.Forms.ComboBox();
            this.txtColor = new Sci.Win.UI.TextBox();
            this.label11 = new Sci.Win.UI.Label();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.label12 = new Sci.Win.UI.Label();
            this.label13 = new Sci.Win.UI.Label();
            this.dateATA1 = new Sci.Win.UI.DateRange();
            this.label14 = new Sci.Win.UI.Label();
            this.txtSupplier = new Sci.Win.UI.TextBox();
            this.label15 = new Sci.Win.UI.Label();
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.label16 = new Sci.Win.UI.Label();
            this.label17 = new Sci.Win.UI.Label();
            this.txtSeason = new Sci.Production.Class.Txtseason();
            this.txtseq2 = new Sci.Win.UI.TextBox();
            this.txtSeq1 = new Sci.Win.UI.TextBox();
            this.label18 = new Sci.Win.UI.Label();
            this.btnClose1 = new Sci.Win.UI.Button();
            this.button3 = new Sci.Win.UI.Button();
            this.btnQuery1 = new Sci.Win.UI.Button();
            this.dateETA = new Sci.Win.UI.DateRange();
            this.label19 = new Sci.Win.UI.Label();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.tabInspectionReport = new System.Windows.Forms.TabPage();
            this.dateATA = new Sci.Win.UI.DateRange();
            this.label8 = new Sci.Win.UI.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnSave2 = new Sci.Win.UI.Button();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnQuery2 = new Sci.Win.UI.Button();
            this.txtpo = new Sci.Win.UI.TextBox();
            this.txtsp2 = new Sci.Win.UI.TextBox();
            this.txtSeq = new Sci.Production.Class.TxtSeq();
            this.dateRangeETA = new Sci.Win.UI.DateRange();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.btn_ToExcel = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bs_Material)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_Document)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_Report)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.MaterialDocumentRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Material)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Document)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Report)).BeginInit();
            this.tabInspectionReport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // bs_Material
            // 
            this.bs_Material.PositionChanged += new System.EventHandler(this.Bs_Material_PositionChanged);
            // 
            // bs_Document
            // 
            this.bs_Document.PositionChanged += new System.EventHandler(this.Bs_Document_PositionChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.MaterialDocumentRecord);
            this.tabControl1.Controls.Add(this.tabInspectionReport);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1191, 603);
            this.tabControl1.TabIndex = 1;
            // 
            // MaterialDocumentRecord
            // 
            this.MaterialDocumentRecord.Controls.Add(this.btn_ToExcel);
            this.MaterialDocumentRecord.Controls.Add(this.splitContainer1);
            this.MaterialDocumentRecord.Controls.Add(this.grid_Report);
            this.MaterialDocumentRecord.Controls.Add(this.comboMaterialType);
            this.MaterialDocumentRecord.Controls.Add(this.txtColor);
            this.MaterialDocumentRecord.Controls.Add(this.label11);
            this.MaterialDocumentRecord.Controls.Add(this.txtRefno);
            this.MaterialDocumentRecord.Controls.Add(this.label12);
            this.MaterialDocumentRecord.Controls.Add(this.label13);
            this.MaterialDocumentRecord.Controls.Add(this.dateATA1);
            this.MaterialDocumentRecord.Controls.Add(this.label14);
            this.MaterialDocumentRecord.Controls.Add(this.txtSupplier);
            this.MaterialDocumentRecord.Controls.Add(this.label15);
            this.MaterialDocumentRecord.Controls.Add(this.txtBrand);
            this.MaterialDocumentRecord.Controls.Add(this.label16);
            this.MaterialDocumentRecord.Controls.Add(this.label17);
            this.MaterialDocumentRecord.Controls.Add(this.txtSeason);
            this.MaterialDocumentRecord.Controls.Add(this.txtseq2);
            this.MaterialDocumentRecord.Controls.Add(this.txtSeq1);
            this.MaterialDocumentRecord.Controls.Add(this.label18);
            this.MaterialDocumentRecord.Controls.Add(this.btnClose1);
            this.MaterialDocumentRecord.Controls.Add(this.button3);
            this.MaterialDocumentRecord.Controls.Add(this.btnQuery1);
            this.MaterialDocumentRecord.Controls.Add(this.dateETA);
            this.MaterialDocumentRecord.Controls.Add(this.label19);
            this.MaterialDocumentRecord.Controls.Add(this.txtSP);
            this.MaterialDocumentRecord.Controls.Add(this.labelSPNo);
            this.MaterialDocumentRecord.Location = new System.Drawing.Point(4, 25);
            this.MaterialDocumentRecord.Name = "MaterialDocumentRecord";
            this.MaterialDocumentRecord.Padding = new System.Windows.Forms.Padding(3);
            this.MaterialDocumentRecord.Size = new System.Drawing.Size(1183, 574);
            this.MaterialDocumentRecord.TabIndex = 2;
            this.MaterialDocumentRecord.Text = "Material Document Record";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(5, 117);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grid_Material);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grid_Document);
            this.splitContainer1.Size = new System.Drawing.Size(1170, 292);
            this.splitContainer1.SplitterDistance = 907;
            this.splitContainer1.TabIndex = 1207;
            // 
            // grid_Material
            // 
            this.grid_Material.AllowUserToAddRows = false;
            this.grid_Material.AllowUserToDeleteRows = false;
            this.grid_Material.AllowUserToResizeRows = false;
            this.grid_Material.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid_Material.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid_Material.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Material.DataSource = this.bs_Material;
            this.grid_Material.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_Material.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid_Material.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid_Material.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid_Material.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid_Material.Location = new System.Drawing.Point(0, 0);
            this.grid_Material.Name = "grid_Material";
            this.grid_Material.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_Material.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_Material.RowTemplate.Height = 24;
            this.grid_Material.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_Material.ShowCellToolTips = false;
            this.grid_Material.Size = new System.Drawing.Size(907, 292);
            this.grid_Material.TabIndex = 117;
            // 
            // grid_Document
            // 
            this.grid_Document.AllowUserToAddRows = false;
            this.grid_Document.AllowUserToDeleteRows = false;
            this.grid_Document.AllowUserToResizeRows = false;
            this.grid_Document.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid_Document.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid_Document.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Document.DataSource = this.bs_Document;
            this.grid_Document.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_Document.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid_Document.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid_Document.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid_Document.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid_Document.Location = new System.Drawing.Point(0, 0);
            this.grid_Document.Name = "grid_Document";
            this.grid_Document.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_Document.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_Document.RowTemplate.Height = 24;
            this.grid_Document.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_Document.ShowCellToolTips = false;
            this.grid_Document.Size = new System.Drawing.Size(259, 292);
            this.grid_Document.TabIndex = 171;
            // 
            // grid_Report
            // 
            this.grid_Report.AllowUserToAddRows = false;
            this.grid_Report.AllowUserToDeleteRows = false;
            this.grid_Report.AllowUserToResizeRows = false;
            this.grid_Report.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid_Report.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid_Report.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid_Report.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Report.DataSource = this.bs_Report;
            this.grid_Report.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid_Report.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid_Report.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid_Report.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid_Report.Location = new System.Drawing.Point(5, 415);
            this.grid_Report.Name = "grid_Report";
            this.grid_Report.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_Report.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_Report.RowTemplate.Height = 24;
            this.grid_Report.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_Report.ShowCellToolTips = false;
            this.grid_Report.Size = new System.Drawing.Size(1170, 114);
            this.grid_Report.TabIndex = 1206;
            // 
            // comboMaterialType
            // 
            this.comboMaterialType.FormattingEnabled = true;
            this.comboMaterialType.Location = new System.Drawing.Point(104, 87);
            this.comboMaterialType.Name = "comboMaterialType";
            this.comboMaterialType.Size = new System.Drawing.Size(121, 24);
            this.comboMaterialType.TabIndex = 1192;
            this.comboMaterialType.SelectedIndexChanged += new System.EventHandler(this.ComboMaterialType_SelectedIndexChanged);
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.White;
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtColor.Location = new System.Drawing.Point(606, 31);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(132, 23);
            this.txtColor.TabIndex = 1189;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(513, 31);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(90, 23);
            this.label11.TabIndex = 1200;
            this.label11.Text = "Color";
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(307, 31);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(200, 23);
            this.txtRefno.TabIndex = 1188;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(243, 31);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 23);
            this.label12.TabIndex = 1199;
            this.label12.Text = "Refno";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(5, 87);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(96, 23);
            this.label13.TabIndex = 1203;
            this.label13.Text = "Material Filter";
            // 
            // dateATA1
            // 
            // 
            // 
            // 
            this.dateATA1.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateATA1.DateBox1.Name = "";
            this.dateATA1.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateATA1.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateATA1.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateATA1.DateBox2.Name = "";
            this.dateATA1.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateATA1.DateBox2.TabIndex = 1;
            this.dateATA1.Location = new System.Drawing.Point(513, 60);
            this.dateATA1.Name = "dateATA1";
            this.dateATA1.Size = new System.Drawing.Size(280, 23);
            this.dateATA1.TabIndex = 1191;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(442, 60);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 23);
            this.label14.TabIndex = 1202;
            this.label14.Text = "ATA";
            // 
            // txtSupplier
            // 
            this.txtSupplier.BackColor = System.Drawing.Color.White;
            this.txtSupplier.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSupplier.Location = new System.Drawing.Point(104, 31);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Size = new System.Drawing.Size(130, 23);
            this.txtSupplier.TabIndex = 1187;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(5, 31);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(96, 23);
            this.label15.TabIndex = 1198;
            this.label15.Text = "Supplier";
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(684, 3);
            this.txtBrand.MyDocumentdName = null;
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(95, 23);
            this.txtBrand.TabIndex = 1186;
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(606, 3);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(75, 23);
            this.label16.TabIndex = 1197;
            this.label16.Text = "Brand";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(442, 3);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 23);
            this.label17.TabIndex = 1196;
            this.label17.Text = "Season";
            // 
            // txtSeason
            // 
            this.txtSeason.BackColor = System.Drawing.Color.White;
            this.txtSeason.BrandObjectName = null;
            this.txtSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeason.Location = new System.Drawing.Point(513, 3);
            this.txtSeason.Name = "txtSeason";
            this.txtSeason.Size = new System.Drawing.Size(90, 23);
            this.txtSeason.TabIndex = 1185;
            // 
            // txtseq2
            // 
            this.txtseq2.BackColor = System.Drawing.Color.White;
            this.txtseq2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseq2.Location = new System.Drawing.Point(381, 3);
            this.txtseq2.MaxLength = 2;
            this.txtseq2.Name = "txtseq2";
            this.txtseq2.Size = new System.Drawing.Size(52, 23);
            this.txtseq2.TabIndex = 1184;
            // 
            // txtSeq1
            // 
            this.txtSeq1.BackColor = System.Drawing.Color.White;
            this.txtSeq1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq1.Location = new System.Drawing.Point(307, 3);
            this.txtSeq1.MaxLength = 3;
            this.txtSeq1.Name = "txtSeq1";
            this.txtSeq1.Size = new System.Drawing.Size(71, 23);
            this.txtSeq1.TabIndex = 1183;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(243, 3);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(59, 23);
            this.label18.TabIndex = 1195;
            this.label18.Text = "SEQ";
            // 
            // btnClose1
            // 
            this.btnClose1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose1.Location = new System.Drawing.Point(1080, 535);
            this.btnClose1.Name = "btnClose1";
            this.btnClose1.Size = new System.Drawing.Size(80, 30);
            this.btnClose1.TabIndex = 1205;
            this.btnClose1.Text = "Close";
            this.btnClose1.UseVisualStyleBackColor = true;
            this.btnClose1.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(822, 535);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(80, 30);
            this.button3.TabIndex = 1204;
            this.button3.Text = "Save";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            // 
            // btnQuery1
            // 
            this.btnQuery1.Location = new System.Drawing.Point(831, 6);
            this.btnQuery1.Name = "btnQuery1";
            this.btnQuery1.Size = new System.Drawing.Size(95, 30);
            this.btnQuery1.TabIndex = 1193;
            this.btnQuery1.Text = "Query";
            this.btnQuery1.UseVisualStyleBackColor = true;
            this.btnQuery1.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // dateETA
            // 
            // 
            // 
            // 
            this.dateETA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateETA.DateBox1.Name = "";
            this.dateETA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateETA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateETA.DateBox2.Name = "";
            this.dateETA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox2.TabIndex = 1;
            this.dateETA.Location = new System.Drawing.Point(104, 59);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(280, 23);
            this.dateETA.TabIndex = 1190;
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(5, 59);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(96, 23);
            this.label19.TabIndex = 1201;
            this.label19.Text = "ETA";
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(104, 3);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(130, 23);
            this.txtSP.TabIndex = 1182;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(5, 3);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(96, 23);
            this.labelSPNo.TabIndex = 1194;
            this.labelSPNo.Text = "SP#";
            // 
            // tabInspectionReport
            // 
            this.tabInspectionReport.Controls.Add(this.dateATA);
            this.tabInspectionReport.Controls.Add(this.label8);
            this.tabInspectionReport.Controls.Add(this.label7);
            this.tabInspectionReport.Controls.Add(this.displayBox1);
            this.tabInspectionReport.Controls.Add(this.btnClose);
            this.tabInspectionReport.Controls.Add(this.btnSave2);
            this.tabInspectionReport.Controls.Add(this.grid1);
            this.tabInspectionReport.Controls.Add(this.btnQuery2);
            this.tabInspectionReport.Controls.Add(this.txtpo);
            this.tabInspectionReport.Controls.Add(this.txtsp2);
            this.tabInspectionReport.Controls.Add(this.txtSeq);
            this.tabInspectionReport.Controls.Add(this.dateRangeETA);
            this.tabInspectionReport.Controls.Add(this.label3);
            this.tabInspectionReport.Controls.Add(this.label2);
            this.tabInspectionReport.Controls.Add(this.label1);
            this.tabInspectionReport.Location = new System.Drawing.Point(4, 25);
            this.tabInspectionReport.Name = "tabInspectionReport";
            this.tabInspectionReport.Padding = new System.Windows.Forms.Padding(3);
            this.tabInspectionReport.Size = new System.Drawing.Size(1183, 574);
            this.tabInspectionReport.TabIndex = 0;
            this.tabInspectionReport.Text = "Inspection Report";
            // 
            // dateATA
            // 
            // 
            // 
            // 
            this.dateATA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateATA.DateBox1.Name = "";
            this.dateATA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateATA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateATA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateATA.DateBox2.Name = "";
            this.dateATA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateATA.DateBox2.TabIndex = 1;
            this.dateATA.Location = new System.Drawing.Point(75, 6);
            this.dateATA.Name = "dateATA";
            this.dateATA.Size = new System.Drawing.Size(280, 23);
            this.dateATA.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 23);
            this.label8.TabIndex = 13;
            this.label8.Text = "ATA";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(36, 545);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(689, 15);
            this.label7.TabIndex = 11;
            this.label7.Text = "Fabric with clima function items, need to check received report included (6.04 Wa" +
    "ter Absorbency + 6.07 Drying Time) by dyelot.";
            // 
            // displayBox1
            // 
            this.displayBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(7, 542);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(23, 23);
            this.displayBox1.TabIndex = 10;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1092, 538);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSave2
            // 
            this.btnSave2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave2.Location = new System.Drawing.Point(1006, 538);
            this.btnSave2.Name = "btnSave2";
            this.btnSave2.Size = new System.Drawing.Size(80, 30);
            this.btnSave2.TabIndex = 8;
            this.btnSave2.Text = "Save";
            this.btnSave2.UseVisualStyleBackColor = true;
            this.btnSave2.Click += new System.EventHandler(this.BtnSave2_Click);
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 42);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(1172, 490);
            this.grid1.TabIndex = 8;
            // 
            // btnQuery2
            // 
            this.btnQuery2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery2.Location = new System.Drawing.Point(1092, 5);
            this.btnQuery2.Name = "btnQuery2";
            this.btnQuery2.Size = new System.Drawing.Size(80, 30);
            this.btnQuery2.TabIndex = 6;
            this.btnQuery2.Text = "Query";
            this.btnQuery2.UseVisualStyleBackColor = true;
            this.btnQuery2.Click += new System.EventHandler(this.BtnQuery2_Click);
            // 
            // txtpo
            // 
            this.txtpo.BackColor = System.Drawing.Color.White;
            this.txtpo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtpo.Location = new System.Drawing.Point(984, 6);
            this.txtpo.Name = "txtpo";
            this.txtpo.Size = new System.Drawing.Size(100, 23);
            this.txtpo.TabIndex = 5;
            // 
            // txtsp2
            // 
            this.txtsp2.BackColor = System.Drawing.Color.White;
            this.txtsp2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtsp2.Location = new System.Drawing.Point(768, 6);
            this.txtsp2.Name = "txtsp2";
            this.txtsp2.Size = new System.Drawing.Size(100, 23);
            this.txtsp2.TabIndex = 3;
            // 
            // txtSeq
            // 
            this.txtSeq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq.Location = new System.Drawing.Point(874, 6);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Seq1 = "";
            this.txtSeq.Seq2 = "";
            this.txtSeq.Size = new System.Drawing.Size(71, 23);
            this.txtSeq.TabIndex = 4;
            // 
            // dateRangeETA
            // 
            // 
            // 
            // 
            this.dateRangeETA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeETA.DateBox1.Name = "";
            this.dateRangeETA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeETA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeETA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeETA.DateBox2.Name = "";
            this.dateRangeETA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeETA.DateBox2.TabIndex = 1;
            this.dateRangeETA.Location = new System.Drawing.Point(434, 6);
            this.dateRangeETA.Name = "dateRangeETA";
            this.dateRangeETA.Size = new System.Drawing.Size(280, 23);
            this.dateRangeETA.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(948, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "PO";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(728, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "SP#";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(367, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "ETA";
            // 
            // btn_ToExcel
            // 
            this.btn_ToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_ToExcel.Location = new System.Drawing.Point(994, 535);
            this.btn_ToExcel.Name = "btn_ToExcel";
            this.btn_ToExcel.Size = new System.Drawing.Size(80, 30);
            this.btn_ToExcel.TabIndex = 1210;
            this.btn_ToExcel.Text = "To Excel";
            this.btn_ToExcel.UseVisualStyleBackColor = true;
            this.btn_ToExcel.Click += new System.EventHandler(this.Btn_ToExcel_Click);
            // 
            // P52
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1191, 603);
            this.Controls.Add(this.tabControl1);
            this.DefaultControl = "txtSP";
            this.DefaultControlForEdit = "txtSP";
            this.Name = "P52";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P52. Material Document Record";
            this.Controls.SetChildIndex(this.tabControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs_Material)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_Document)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_Report)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.MaterialDocumentRecord.ResumeLayout(false);
            this.MaterialDocumentRecord.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_Material)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Document)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Report)).EndInit();
            this.tabInspectionReport.ResumeLayout(false);
            this.tabInspectionReport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.ListControlBindingSource bs_Material;
        private Win.UI.ListControlBindingSource bs_Document;
        private Win.UI.ListControlBindingSource bs_Report;
        private Win.UI.TabControl tabControl1;
        private System.Windows.Forms.TabPage MaterialDocumentRecord;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Grid grid_Material;
        private Win.UI.Grid grid_Document;
        private Win.UI.Grid grid_Report;
        private System.Windows.Forms.ComboBox comboMaterialType;
        private Win.UI.TextBox txtColor;
        private Win.UI.Label label11;
        private Win.UI.TextBox txtRefno;
        private Win.UI.Label label12;
        private Win.UI.Label label13;
        private Win.UI.DateRange dateATA1;
        private Win.UI.Label label14;
        private Win.UI.TextBox txtSupplier;
        private Win.UI.Label label15;
        private Class.Txtbrand txtBrand;
        private Win.UI.Label label16;
        private Win.UI.Label label17;
        private Class.Txtseason txtSeason;
        private Win.UI.TextBox txtseq2;
        private Win.UI.TextBox txtSeq1;
        private Win.UI.Label label18;
        private Win.UI.Button btnClose1;
        private Win.UI.Button button3;
        private Win.UI.Button btnQuery1;
        private Win.UI.DateRange dateETA;
        private Win.UI.Label label19;
        private Win.UI.TextBox txtSP;
        private Win.UI.Label labelSPNo;
        private System.Windows.Forms.TabPage tabInspectionReport;
        private Win.UI.DateRange dateATA;
        private Win.UI.Label label8;
        private System.Windows.Forms.Label label7;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave2;
        private Win.UI.Grid grid1;
        private Win.UI.Button btnQuery2;
        private Win.UI.TextBox txtpo;
        private Win.UI.TextBox txtsp2;
        private Class.TxtSeq txtSeq;
        private Win.UI.DateRange dateRangeETA;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btn_ToExcel;
    }
}