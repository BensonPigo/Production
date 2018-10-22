namespace Sci.Production.Warehouse
{
    partial class P29
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
            this.txtmfactory = new Sci.Production.Class.txtfactory();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.labelCategory = new Sci.Win.UI.Label();
            this.comboFabricType = new Sci.Win.UI.ComboBox();
            this.labelFabricType = new Sci.Win.UI.Label();
            this.dateOrderCfmDate = new Sci.Win.UI.DateRange();
            this.dateCuttingInline = new Sci.Win.UI.DateRange();
            this.txtProjectID = new Sci.Win.UI.TextBox();
            this.txtIssueSP = new Sci.Win.UI.TextBox();
            this.btnAutoPick = new Sci.Win.UI.Button();
            this.btnQuery = new Sci.Win.UI.Button();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelOrderCfmDate = new Sci.Win.UI.Label();
            this.labelCuttingInline = new Sci.Win.UI.Label();
            this.labelProjectID = new Sci.Win.UI.Label();
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
            this.panel1.Controls.Add(this.txtmfactory);
            this.panel1.Controls.Add(this.comboCategory);
            this.panel1.Controls.Add(this.labelCategory);
            this.panel1.Controls.Add(this.comboFabricType);
            this.panel1.Controls.Add(this.labelFabricType);
            this.panel1.Controls.Add(this.dateOrderCfmDate);
            this.panel1.Controls.Add(this.dateCuttingInline);
            this.panel1.Controls.Add(this.txtProjectID);
            this.panel1.Controls.Add(this.txtIssueSP);
            this.panel1.Controls.Add(this.btnAutoPick);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelOrderCfmDate);
            this.panel1.Controls.Add(this.labelCuttingInline);
            this.panel1.Controls.Add(this.labelProjectID);
            this.panel1.Controls.Add(this.labelIssueSP);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 107);
            this.panel1.TabIndex = 1;
            // 
            // txtmfactory
            // 
            this.txtmfactory.BackColor = System.Drawing.Color.White;
            this.txtmfactory.FilteMDivision = true;
            this.txtmfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtmfactory.IssupportJunk = false;
            this.txtmfactory.Location = new System.Drawing.Point(87, 75);
            this.txtmfactory.Name = "txtmfactory";
            this.txtmfactory.Size = new System.Drawing.Size(117, 23);
            this.txtmfactory.TabIndex = 2;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Items.AddRange(new object[] {
            "Bulk",
            "Sample",
            "Material",
            "All"});
            this.comboCategory.Location = new System.Drawing.Point(729, 42);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(121, 24);
            this.comboCategory.TabIndex = 6;
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(637, 42);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(89, 23);
            this.labelCategory.TabIndex = 15;
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
            this.comboFabricType.TabIndex = 5;
            // 
            // labelFabricType
            // 
            this.labelFabricType.Location = new System.Drawing.Point(637, 9);
            this.labelFabricType.Name = "labelFabricType";
            this.labelFabricType.Size = new System.Drawing.Size(89, 23);
            this.labelFabricType.TabIndex = 14;
            this.labelFabricType.Text = "Fabric Type";
            // 
            // dateOrderCfmDate
            // 
            // 
            // 
            // 
            this.dateOrderCfmDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateOrderCfmDate.DateBox1.Name = "";
            this.dateOrderCfmDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateOrderCfmDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateOrderCfmDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateOrderCfmDate.DateBox2.Name = "";
            this.dateOrderCfmDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateOrderCfmDate.DateBox2.TabIndex = 1;
            this.dateOrderCfmDate.Location = new System.Drawing.Point(334, 42);
            this.dateOrderCfmDate.Name = "dateOrderCfmDate";
            this.dateOrderCfmDate.Size = new System.Drawing.Size(280, 23);
            this.dateOrderCfmDate.TabIndex = 4;
            // 
            // dateCuttingInline
            // 
            // 
            // 
            // 
            this.dateCuttingInline.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCuttingInline.DateBox1.Name = "";
            this.dateCuttingInline.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCuttingInline.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCuttingInline.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCuttingInline.DateBox2.Name = "";
            this.dateCuttingInline.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCuttingInline.DateBox2.TabIndex = 1;
            this.dateCuttingInline.Location = new System.Drawing.Point(334, 9);
            this.dateCuttingInline.Name = "dateCuttingInline";
            this.dateCuttingInline.Size = new System.Drawing.Size(280, 23);
            this.dateCuttingInline.TabIndex = 3;
            // 
            // txtProjectID
            // 
            this.txtProjectID.BackColor = System.Drawing.Color.White;
            this.txtProjectID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtProjectID.Location = new System.Drawing.Point(87, 42);
            this.txtProjectID.Name = "txtProjectID";
            this.txtProjectID.Size = new System.Drawing.Size(117, 23);
            this.txtProjectID.TabIndex = 1;
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
            this.btnAutoPick.Location = new System.Drawing.Point(916, 42);
            this.btnAutoPick.Name = "btnAutoPick";
            this.btnAutoPick.Size = new System.Drawing.Size(80, 30);
            this.btnAutoPick.TabIndex = 8;
            this.btnAutoPick.Text = "AutoPick";
            this.btnAutoPick.UseVisualStyleBackColor = true;
            this.btnAutoPick.Click += new System.EventHandler(this.btnAutoPick_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(916, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 7;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(9, 75);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(75, 23);
            this.labelFactory.TabIndex = 11;
            this.labelFactory.Text = "Factory";
            // 
            // labelOrderCfmDate
            // 
            this.labelOrderCfmDate.Location = new System.Drawing.Point(218, 42);
            this.labelOrderCfmDate.Name = "labelOrderCfmDate";
            this.labelOrderCfmDate.Size = new System.Drawing.Size(113, 23);
            this.labelOrderCfmDate.TabIndex = 13;
            this.labelOrderCfmDate.Text = "Order Cfm Date";
            // 
            // labelCuttingInline
            // 
            this.labelCuttingInline.Location = new System.Drawing.Point(218, 9);
            this.labelCuttingInline.Name = "labelCuttingInline";
            this.labelCuttingInline.Size = new System.Drawing.Size(113, 23);
            this.labelCuttingInline.TabIndex = 12;
            this.labelCuttingInline.Text = "Cutting Inline";
            // 
            // labelProjectID
            // 
            this.labelProjectID.Location = new System.Drawing.Point(9, 42);
            this.labelProjectID.Name = "labelProjectID";
            this.labelProjectID.Size = new System.Drawing.Size(75, 23);
            this.labelProjectID.TabIndex = 10;
            this.labelProjectID.Text = "Project ID";
            // 
            // labelIssueSP
            // 
            this.labelIssueSP.Location = new System.Drawing.Point(9, 9);
            this.labelIssueSP.Name = "labelIssueSP";
            this.labelIssueSP.Size = new System.Drawing.Size(75, 23);
            this.labelIssueSP.TabIndex = 9;
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
            this.checkOnly.CheckedChanged += new System.EventHandler(this.checkOnly_CheckedChanged);
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
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.Location = new System.Drawing.Point(763, 11);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(147, 30);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.Text = "Create && Confirm";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Location = new System.Drawing.Point(677, 11);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(80, 30);
            this.btnExcel.TabIndex = 0;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
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
            this.gridComplete.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridComplete_RowEnter);
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
            // 
            // P29
            // 
            this.ClientSize = new System.Drawing.Size(1008, 601);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "txtIssueSP";
            this.DefaultControlForEdit = "txtIssueSP";
            this.EditMode = true;
            this.Name = "P29";
            this.Text = "P29. Batch Create Inventory to Bulk";
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
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelOrderCfmDate;
        private Win.UI.Label labelCuttingInline;
        private Win.UI.Label labelProjectID;
        private Win.UI.Label labelIssueSP;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnCreate;
        private Win.UI.Button btnExcel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.DateRange dateOrderCfmDate;
        private Win.UI.DateRange dateCuttingInline;
        private Win.UI.TextBox txtProjectID;
        private Win.UI.TextBox txtIssueSP;
        private Class.txtfactory txtmfactory;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.Label labelCategory;
        private Win.UI.ComboBox comboFabricType;
        private Win.UI.Label labelFabricType;
        private Win.UI.Grid gridComplete;
        private Win.UI.Grid gridRel;
        private Win.UI.CheckBox checkOnly;
    }
}
