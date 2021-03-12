namespace Sci.Production.Warehouse
{
    partial class P30
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel2 = new Sci.Win.UI.Panel();
            this.checkOnly = new Sci.Win.UI.CheckBox();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnCreate = new Sci.Win.UI.Button();
            this.btnExcel = new Sci.Win.UI.Button();
            this.labelSP = new Sci.Win.UI.Label();
            this.labCfmDate = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnAutoPick = new Sci.Win.UI.Button();
            this.txtSP_s = new Sci.Win.UI.TextBox();
            this.dateCfmDate = new Sci.Win.UI.DateRange();
            this.labelFabricType = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.panel1 = new Sci.Win.UI.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridComplete = new Sci.Win.UI.Grid();
            this.gridRel = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.txtSP_e = new Sci.Win.UI.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboCategory = new Sci.Production.Class.ComboDropDownList(this.components);
            this.comboFabricType = new Sci.Production.Class.ComboDropDownList(this.components);
            this.txtmfactory = new Sci.Production.Class.Txtfactory();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridComplete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.SuspendLayout();
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
            this.panel2.TabIndex = 6;
            // 
            // checkOnly
            // 
            this.checkOnly.AutoSize = true;
            this.checkOnly.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkOnly.Location = new System.Drawing.Point(12, 17);
            this.checkOnly.Name = "checkOnly";
            this.checkOnly.Size = new System.Drawing.Size(316, 21);
            this.checkOnly.TabIndex = 12;
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
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.Location = new System.Drawing.Point(763, 11);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(147, 30);
            this.btnCreate.TabIndex = 10;
            this.btnCreate.Text = "Create && Confirm";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Location = new System.Drawing.Point(677, 11);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(80, 30);
            this.btnExcel.TabIndex = 9;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.BtnExcel_Click);
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(13, 15);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(123, 23);
            this.labelSP.TabIndex = 9;
            this.labelSP.Text = "SP#";
            // 
            // labCfmDate
            // 
            this.labCfmDate.Location = new System.Drawing.Point(13, 48);
            this.labCfmDate.Name = "labCfmDate";
            this.labCfmDate.Size = new System.Drawing.Size(123, 23);
            this.labCfmDate.TabIndex = 13;
            this.labCfmDate.Text = "Inventory Cfm date";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(475, 15);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(75, 23);
            this.labelFactory.TabIndex = 11;
            this.labelFactory.Text = "Factory";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(903, 15);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 7;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // btnAutoPick
            // 
            this.btnAutoPick.Location = new System.Drawing.Point(903, 48);
            this.btnAutoPick.Name = "btnAutoPick";
            this.btnAutoPick.Size = new System.Drawing.Size(80, 30);
            this.btnAutoPick.TabIndex = 8;
            this.btnAutoPick.Text = "AutoPick";
            this.btnAutoPick.UseVisualStyleBackColor = true;
            this.btnAutoPick.Click += new System.EventHandler(this.BtnAutoPick_Click);
            // 
            // txtSP_s
            // 
            this.txtSP_s.BackColor = System.Drawing.Color.White;
            this.txtSP_s.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP_s.Location = new System.Drawing.Point(139, 15);
            this.txtSP_s.Name = "txtSP_s";
            this.txtSP_s.Size = new System.Drawing.Size(117, 23);
            this.txtSP_s.TabIndex = 1;
            // 
            // dateCfmDate
            // 
            // 
            // 
            // 
            this.dateCfmDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCfmDate.DateBox1.Name = "";
            this.dateCfmDate.DateBox1.Size = new System.Drawing.Size(152, 23);
            this.dateCfmDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCfmDate.DateBox2.Location = new System.Drawing.Point(174, 0);
            this.dateCfmDate.DateBox2.Name = "";
            this.dateCfmDate.DateBox2.Size = new System.Drawing.Size(152, 23);
            this.dateCfmDate.DateBox2.TabIndex = 1;
            this.dateCfmDate.Location = new System.Drawing.Point(139, 48);
            this.dateCfmDate.Name = "dateCfmDate";
            this.dateCfmDate.Size = new System.Drawing.Size(326, 23);
            this.dateCfmDate.TabIndex = 3;
            // 
            // labelFabricType
            // 
            this.labelFabricType.Location = new System.Drawing.Point(685, 15);
            this.labelFabricType.Name = "labelFabricType";
            this.labelFabricType.Size = new System.Drawing.Size(89, 23);
            this.labelFabricType.TabIndex = 14;
            this.labelFabricType.Text = "Material Type";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(475, 48);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(75, 23);
            this.labelCategory.TabIndex = 15;
            this.labelCategory.Text = "Category";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtSP_e);
            this.panel1.Controls.Add(this.comboCategory);
            this.panel1.Controls.Add(this.comboFabricType);
            this.panel1.Controls.Add(this.txtmfactory);
            this.panel1.Controls.Add(this.labelCategory);
            this.panel1.Controls.Add(this.labelFabricType);
            this.panel1.Controls.Add(this.dateCfmDate);
            this.panel1.Controls.Add(this.txtSP_s);
            this.panel1.Controls.Add(this.btnAutoPick);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labCfmDate);
            this.panel1.Controls.Add(this.labelSP);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 90);
            this.panel1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 90);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridComplete);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridRel);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 458);
            this.splitContainer1.SplitterDistance = 571;
            this.splitContainer1.TabIndex = 7;
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
            this.gridComplete.Size = new System.Drawing.Size(571, 458);
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
            this.gridRel.Size = new System.Drawing.Size(433, 458);
            this.gridRel.TabIndex = 0;
            this.gridRel.TabStop = false;
            // 
            // txtSP_e
            // 
            this.txtSP_e.BackColor = System.Drawing.Color.White;
            this.txtSP_e.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP_e.Location = new System.Drawing.Point(283, 15);
            this.txtSP_e.Name = "txtSP_e";
            this.txtSP_e.Size = new System.Drawing.Size(117, 23);
            this.txtSP_e.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(261, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 17);
            this.label1.TabIndex = 19;
            this.label1.Text = "~";
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(553, 47);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(121, 24);
            this.comboCategory.TabIndex = 5;
            this.comboCategory.Type = "Pms_MtlCategory";
            // 
            // comboFabricType
            // 
            this.comboFabricType.BackColor = System.Drawing.Color.White;
            this.comboFabricType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFabricType.FormattingEnabled = true;
            this.comboFabricType.IsSupportUnselect = true;
            this.comboFabricType.Location = new System.Drawing.Point(777, 15);
            this.comboFabricType.Name = "comboFabricType";
            this.comboFabricType.OldText = "";
            this.comboFabricType.Size = new System.Drawing.Size(121, 24);
            this.comboFabricType.TabIndex = 6;
            this.comboFabricType.Type = "Pms_FabricType";
            // 
            // txtmfactory
            // 
            this.txtmfactory.BackColor = System.Drawing.Color.White;
            this.txtmfactory.BoolFtyGroupList = true;
            this.txtmfactory.FilteMDivision = true;
            this.txtmfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtmfactory.IsProduceFty = false;
            this.txtmfactory.IssupportJunk = false;
            this.txtmfactory.Location = new System.Drawing.Point(553, 15);
            this.txtmfactory.MDivision = null;
            this.txtmfactory.Name = "txtmfactory";
            this.txtmfactory.Size = new System.Drawing.Size(121, 23);
            this.txtmfactory.TabIndex = 4;
            // 
            // P30
            // 
            this.ClientSize = new System.Drawing.Size(1008, 601);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "txtSP";
            this.DefaultControlForEdit = "txtSP";
            this.EditMode = true;
            this.Name = "P30";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P30.Batch Create Inventory to Scarp";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridComplete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.Panel panel2;
        private Win.UI.CheckBox checkOnly;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnCreate;
        private Win.UI.Button btnExcel;
        private Win.UI.Label labelSP;
        private Win.UI.Label labCfmDate;
        private Win.UI.Label labelFactory;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnAutoPick;
        private Win.UI.TextBox txtSP_s;
        private Win.UI.DateRange dateCfmDate;
        private Win.UI.Label labelFabricType;
        private Win.UI.Label labelCategory;
        private Class.Txtfactory txtmfactory;
        private Win.UI.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Grid gridComplete;
        private Win.UI.Grid gridRel;
        private Class.ComboDropDownList comboFabricType;
        private Class.ComboDropDownList comboCategory;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private System.Windows.Forms.Label label1;
        private Win.UI.TextBox txtSP_e;
    }
}
