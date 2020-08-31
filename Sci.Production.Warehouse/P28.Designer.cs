namespace Sci.Production.Warehouse
{
    partial class P28
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
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1 = new Sci.Win.UI.Panel();
            this.Category = new Sci.Production.Class.ComboDropDownList(this.components);
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.lbFactory = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.comboFabricType = new Sci.Win.UI.ComboBox();
            this.labelFabricType = new Sci.Win.UI.Label();
            this.dateInputDate = new Sci.Win.UI.DateRange();
            this.dateMaterialATA = new Sci.Win.UI.DateRange();
            this.txtIssueSP = new Sci.Win.UI.TextBox();
            this.btnAutoPick = new Sci.Win.UI.Button();
            this.btnQuery = new Sci.Win.UI.Button();
            this.labelInputDate = new Sci.Win.UI.Label();
            this.labelMaterialATA = new Sci.Win.UI.Label();
            this.labelIssueSP = new Sci.Win.UI.Label();
            this.panel2 = new Sci.Win.UI.Panel();
            this.checkOnly = new Sci.Win.UI.CheckBox();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnCreate = new Sci.Win.UI.Button();
            this.btnExcel = new Sci.Win.UI.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridComplete = new Sci.Win.UI.Grid();
            this.gridRel = new Sci.Win.UI.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridComplete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRel)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Category);
            this.panel1.Controls.Add(this.txtfactory);
            this.panel1.Controls.Add(this.lbFactory);
            this.panel1.Controls.Add(this.labelCategory);
            this.panel1.Controls.Add(this.comboFabricType);
            this.panel1.Controls.Add(this.labelFabricType);
            this.panel1.Controls.Add(this.dateInputDate);
            this.panel1.Controls.Add(this.dateMaterialATA);
            this.panel1.Controls.Add(this.txtIssueSP);
            this.panel1.Controls.Add(this.btnAutoPick);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.labelInputDate);
            this.panel1.Controls.Add(this.labelMaterialATA);
            this.panel1.Controls.Add(this.labelIssueSP);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 107);
            this.panel1.TabIndex = 1;
            // 
            // Category
            // 
            this.Category.BackColor = System.Drawing.Color.White;
            this.Category.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Category.FormattingEnabled = true;
            this.Category.IsSupportUnselect = true;
            this.Category.Location = new System.Drawing.Point(729, 43);
            this.Category.Name = "Category";
            this.Category.OldText = "";
            this.Category.Size = new System.Drawing.Size(121, 24);
            this.Category.TabIndex = 119;
            this.Category.Type = "Pms_MtlCategory";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(87, 42);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(117, 23);
            this.txtfactory.TabIndex = 118;
            // 
            // lbFactory
            // 
            this.lbFactory.Location = new System.Drawing.Point(9, 42);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(75, 23);
            this.lbFactory.TabIndex = 117;
            this.lbFactory.Text = "Factory";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(637, 42);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(89, 23);
            this.labelCategory.TabIndex = 115;
            this.labelCategory.Text = "Category";
            // 
            // comboFabricType
            // 
            this.comboFabricType.BackColor = System.Drawing.Color.White;
            this.comboFabricType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFabricType.FormattingEnabled = true;
            this.comboFabricType.IsSupportUnselect = true;
            this.comboFabricType.Items.AddRange(new object[] {
            "Fabric",
            "Accessory",
            "All"});
            this.comboFabricType.Location = new System.Drawing.Point(729, 9);
            this.comboFabricType.Name = "comboFabricType";
            this.comboFabricType.OldText = "";
            this.comboFabricType.Size = new System.Drawing.Size(121, 24);
            this.comboFabricType.TabIndex = 4;
            // 
            // labelFabricType
            // 
            this.labelFabricType.Location = new System.Drawing.Point(637, 9);
            this.labelFabricType.Name = "labelFabricType";
            this.labelFabricType.Size = new System.Drawing.Size(89, 23);
            this.labelFabricType.TabIndex = 113;
            this.labelFabricType.Text = "Fabric Type";
            // 
            // dateInputDate
            // 
            // 
            // 
            // 
            this.dateInputDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInputDate.DateBox1.Name = "";
            this.dateInputDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateInputDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInputDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateInputDate.DateBox2.Name = "";
            this.dateInputDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateInputDate.DateBox2.TabIndex = 1;
            this.dateInputDate.Location = new System.Drawing.Point(334, 42);
            this.dateInputDate.Name = "dateInputDate";
            this.dateInputDate.Size = new System.Drawing.Size(280, 23);
            this.dateInputDate.TabIndex = 3;
            // 
            // dateMaterialATA
            // 
            // 
            // 
            // 
            this.dateMaterialATA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateMaterialATA.DateBox1.Name = "";
            this.dateMaterialATA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateMaterialATA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateMaterialATA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateMaterialATA.DateBox2.Name = "";
            this.dateMaterialATA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateMaterialATA.DateBox2.TabIndex = 1;
            this.dateMaterialATA.Location = new System.Drawing.Point(334, 9);
            this.dateMaterialATA.Name = "dateMaterialATA";
            this.dateMaterialATA.Size = new System.Drawing.Size(280, 23);
            this.dateMaterialATA.TabIndex = 1;
            // 
            // txtIssueSP
            // 
            this.txtIssueSP.BackColor = System.Drawing.Color.White;
            this.txtIssueSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtIssueSP.Location = new System.Drawing.Point(87, 9);
            this.txtIssueSP.Name = "txtIssueSP";
            this.txtIssueSP.Size = new System.Drawing.Size(117, 23);
            this.txtIssueSP.TabIndex = 0;
            // 
            // btnAutoPick
            // 
            this.btnAutoPick.Location = new System.Drawing.Point(916, 45);
            this.btnAutoPick.Name = "btnAutoPick";
            this.btnAutoPick.Size = new System.Drawing.Size(80, 30);
            this.btnAutoPick.TabIndex = 7;
            this.btnAutoPick.Text = "AutoPick";
            this.btnAutoPick.UseVisualStyleBackColor = true;
            this.btnAutoPick.Click += new System.EventHandler(this.BtnAutoPick_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(916, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // labelInputDate
            // 
            this.labelInputDate.Location = new System.Drawing.Point(218, 42);
            this.labelInputDate.Name = "labelInputDate";
            this.labelInputDate.Size = new System.Drawing.Size(113, 23);
            this.labelInputDate.TabIndex = 3;
            this.labelInputDate.Text = "Input Date";
            // 
            // labelMaterialATA
            // 
            this.labelMaterialATA.Location = new System.Drawing.Point(218, 9);
            this.labelMaterialATA.Name = "labelMaterialATA";
            this.labelMaterialATA.Size = new System.Drawing.Size(113, 23);
            this.labelMaterialATA.TabIndex = 2;
            this.labelMaterialATA.Text = "Material ATA";
            // 
            // labelIssueSP
            // 
            this.labelIssueSP.Location = new System.Drawing.Point(9, 9);
            this.labelIssueSP.Name = "labelIssueSP";
            this.labelIssueSP.Size = new System.Drawing.Size(75, 23);
            this.labelIssueSP.TabIndex = 7;
            this.labelIssueSP.Text = "Issue SP#";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkOnly);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.btnCreate);
            this.panel2.Controls.Add(this.btnExcel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 548);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 53);
            this.panel2.TabIndex = 3;
            // 
            // checkOnly
            // 
            this.checkOnly.AutoSize = true;
            this.checkOnly.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkOnly.Location = new System.Drawing.Point(12, 17);
            this.checkOnly.Name = "checkOnly";
            this.checkOnly.Size = new System.Drawing.Size(316, 21);
            this.checkOnly.TabIndex = 3;
            this.checkOnly.Text = "Only show data of complete inventory location";
            this.checkOnly.UseVisualStyleBackColor = true;
            this.checkOnly.CheckedChanged += new System.EventHandler(this.CheckOnly_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(916, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.Location = new System.Drawing.Point(764, 11);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(146, 30);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.Text = "Create && Confirm";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Location = new System.Drawing.Point(678, 11);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(80, 30);
            this.btnExcel.TabIndex = 0;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.BtnExcel_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 107);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridComplete);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridRel);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 441);
            this.splitContainer1.SplitterDistance = 571;
            this.splitContainer1.TabIndex = 4;
            // 
            // gridComplete
            // 
            this.gridComplete.AllowUserToAddRows = false;
            this.gridComplete.AllowUserToDeleteRows = false;
            this.gridComplete.AllowUserToResizeRows = false;
            this.gridComplete.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridComplete.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridComplete.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridComplete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridComplete.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridComplete.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridComplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridComplete.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridComplete.Location = new System.Drawing.Point(0, 0);
            this.gridComplete.Name = "gridComplete";
            this.gridComplete.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridComplete.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridComplete.RowTemplate.Height = 24;
            this.gridComplete.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridComplete.ShowCellToolTips = false;
            this.gridComplete.Size = new System.Drawing.Size(571, 441);
            this.gridComplete.TabIndex = 0;
            this.gridComplete.TabStop = false;
            this.gridComplete.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridComplete_RowEnter);
            // 
            // gridRel
            // 
            this.gridRel.AllowUserToAddRows = false;
            this.gridRel.AllowUserToDeleteRows = false;
            this.gridRel.AllowUserToResizeRows = false;
            this.gridRel.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridRel.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridRel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRel.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridRel.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridRel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridRel.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridRel.Location = new System.Drawing.Point(0, 0);
            this.gridRel.Name = "gridRel";
            this.gridRel.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridRel.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridRel.RowTemplate.Height = 24;
            this.gridRel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRel.ShowCellToolTips = false;
            this.gridRel.Size = new System.Drawing.Size(433, 441);
            this.gridRel.TabIndex = 0;
            this.gridRel.TabStop = false;
            this.gridRel.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.GridRel_CellFormatting);
            // 
            // P28
            // 
            this.ClientSize = new System.Drawing.Size(1008, 601);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "txtIssueSP";
            this.DefaultControlForEdit = "txtIssueSP";
            this.EditMode = true;
            this.Name = "P28";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P28. Batch Create Bulk to Inventory";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridComplete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Panel panel1;
        private Win.UI.Button btnAutoPick;
        private Win.UI.Button btnQuery;
        private Win.UI.Label labelInputDate;
        private Win.UI.Label labelMaterialATA;
        private Win.UI.Label labelIssueSP;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnCreate;
        private Win.UI.Button btnExcel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.DateRange dateInputDate;
        private Win.UI.DateRange dateMaterialATA;
        private Win.UI.TextBox txtIssueSP;
        private Win.UI.Label labelCategory;
        private Win.UI.ComboBox comboFabricType;
        private Win.UI.Label labelFabricType;
        private Win.UI.Grid gridComplete;
        private Win.UI.Grid gridRel;
        private Win.UI.CheckBox checkOnly;
        private Win.UI.Label lbFactory;
        private Class.Txtfactory txtfactory;
        private Class.ComboDropDownList Category;
    }
}
