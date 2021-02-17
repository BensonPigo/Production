namespace Sci.Production.Warehouse
{
    partial class P48
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridImport = new Sci.Win.UI.Grid();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.txtMtlLocation2 = new Sci.Production.Class.TxtMtlLocation(this.components);
            this.txtMtlLocation1 = new Sci.Production.Class.TxtMtlLocation(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.lbSeason = new Sci.Win.UI.Label();
            this.lbBrand = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.label2 = new Sci.Win.UI.Label();
            this.txtdropdownlistFabricType = new Sci.Production.Class.ComboDropDownList(this.components);
            this.comboCategory = new Sci.Production.Class.ComboDropDownList(this.components);
            this.labCategory = new Sci.Win.UI.Label();
            this.txtSPNo2 = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.labelLocation = new Sci.Win.UI.Label();
            this.labelFabricType = new Sci.Win.UI.Label();
            this.txtRef = new Sci.Win.UI.TextBox();
            this.labelRef = new Sci.Win.UI.Label();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.txtSPNo1 = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.btnUpdateAll = new Sci.Win.UI.Button();
            this.labelReason = new Sci.Win.UI.Label();
            this.comboReason = new Sci.Win.UI.ComboBox();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 116);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1096, 304);
            this.panel1.TabIndex = 23;
            // 
            // gridImport
            // 
            this.gridImport.AllowUserToAddRows = false;
            this.gridImport.AllowUserToDeleteRows = false;
            this.gridImport.AllowUserToResizeRows = false;
            this.gridImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridImport.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImport.Location = new System.Drawing.Point(0, 0);
            this.gridImport.Name = "gridImport";
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 24;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.ShowCellToolTips = false;
            this.gridImport.Size = new System.Drawing.Size(1096, 304);
            this.gridImport.TabIndex = 0;
            this.gridImport.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMtlLocation2);
            this.groupBox1.Controls.Add(this.txtMtlLocation1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtseason);
            this.groupBox1.Controls.Add(this.txtbrand);
            this.groupBox1.Controls.Add(this.lbSeason);
            this.groupBox1.Controls.Add(this.lbBrand);
            this.groupBox1.Controls.Add(this.txtfactory);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtdropdownlistFabricType);
            this.groupBox1.Controls.Add(this.comboCategory);
            this.groupBox1.Controls.Add(this.labCategory);
            this.groupBox1.Controls.Add(this.txtSPNo2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.labelLocation);
            this.groupBox1.Controls.Add(this.labelFabricType);
            this.groupBox1.Controls.Add(this.txtRef);
            this.groupBox1.Controls.Add(this.labelRef);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtSPNo1);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1096, 116);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // txtMtlLocation2
            // 
            this.txtMtlLocation2.BackColor = System.Drawing.Color.White;
            this.txtMtlLocation2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMtlLocation2.Location = new System.Drawing.Point(763, 50);
            this.txtMtlLocation2.Name = "txtMtlLocation2";
            this.txtMtlLocation2.Size = new System.Drawing.Size(132, 23);
            this.txtMtlLocation2.StockTypeFilte = "O";
            this.txtMtlLocation2.TabIndex = 120;
            // 
            // txtMtlLocation1
            // 
            this.txtMtlLocation1.BackColor = System.Drawing.Color.White;
            this.txtMtlLocation1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMtlLocation1.Location = new System.Drawing.Point(601, 51);
            this.txtMtlLocation1.Name = "txtMtlLocation1";
            this.txtMtlLocation1.Size = new System.Drawing.Size(132, 23);
            this.txtMtlLocation1.StockTypeFilte = "O";
            this.txtMtlLocation1.TabIndex = 119;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(741, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 17);
            this.label3.TabIndex = 118;
            this.label3.Text = "~";
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(601, 20);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(132, 23);
            this.txtseason.TabIndex = 116;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(431, 20);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(92, 23);
            this.txtbrand.TabIndex = 115;
            // 
            // lbSeason
            // 
            this.lbSeason.Location = new System.Drawing.Point(529, 20);
            this.lbSeason.Name = "lbSeason";
            this.lbSeason.Size = new System.Drawing.Size(69, 23);
            this.lbSeason.TabIndex = 114;
            this.lbSeason.Text = "Season";
            // 
            // lbBrand
            // 
            this.lbBrand.Location = new System.Drawing.Point(359, 19);
            this.lbBrand.Name = "lbBrand";
            this.lbBrand.Size = new System.Drawing.Size(69, 23);
            this.lbBrand.TabIndex = 113;
            this.lbBrand.Text = "Brand";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(431, 51);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(92, 23);
            this.txtfactory.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(359, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 23);
            this.label2.TabIndex = 112;
            this.label2.Text = "Factory";
            // 
            // txtdropdownlistFabricType
            // 
            this.txtdropdownlistFabricType.AddAllItem = false;
            this.txtdropdownlistFabricType.BackColor = System.Drawing.Color.White;
            this.txtdropdownlistFabricType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlistFabricType.FormattingEnabled = true;
            this.txtdropdownlistFabricType.IsSupportUnselect = true;
            this.txtdropdownlistFabricType.Location = new System.Drawing.Point(827, 20);
            this.txtdropdownlistFabricType.Name = "txtdropdownlistFabricType";
            this.txtdropdownlistFabricType.OldText = "";
            this.txtdropdownlistFabricType.Size = new System.Drawing.Size(143, 24);
            this.txtdropdownlistFabricType.TabIndex = 5;
            this.txtdropdownlistFabricType.Type = "FabricType_Condition";
            // 
            // comboCategory
            // 
            this.comboCategory.AddAllItem = false;
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(81, 81);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(143, 24);
            this.comboCategory.TabIndex = 6;
            this.comboCategory.Type = "Pms_MtlCategory";
            // 
            // labCategory
            // 
            this.labCategory.Location = new System.Drawing.Point(9, 81);
            this.labCategory.Name = "labCategory";
            this.labCategory.Size = new System.Drawing.Size(69, 23);
            this.labCategory.TabIndex = 111;
            this.labCategory.Text = "Category";
            // 
            // txtSPNo2
            // 
            this.txtSPNo2.BackColor = System.Drawing.Color.White;
            this.txtSPNo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo2.Location = new System.Drawing.Point(219, 20);
            this.txtSPNo2.Name = "txtSPNo2";
            this.txtSPNo2.Size = new System.Drawing.Size(122, 23);
            this.txtSPNo2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(206, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 23);
            this.label1.TabIndex = 109;
            this.label1.Text = "~";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelLocation
            // 
            this.labelLocation.Location = new System.Drawing.Point(529, 50);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(69, 23);
            this.labelLocation.TabIndex = 7;
            this.labelLocation.Text = "Location";
            // 
            // labelFabricType
            // 
            this.labelFabricType.Location = new System.Drawing.Point(739, 19);
            this.labelFabricType.Name = "labelFabricType";
            this.labelFabricType.Size = new System.Drawing.Size(85, 23);
            this.labelFabricType.TabIndex = 6;
            this.labelFabricType.Text = "Fabric Type";
            // 
            // txtRef
            // 
            this.txtRef.BackColor = System.Drawing.Color.White;
            this.txtRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRef.Location = new System.Drawing.Point(81, 50);
            this.txtRef.Name = "txtRef";
            this.txtRef.Size = new System.Drawing.Size(260, 23);
            this.txtRef.TabIndex = 2;
            // 
            // labelRef
            // 
            this.labelRef.Location = new System.Drawing.Point(9, 50);
            this.labelRef.Name = "labelRef";
            this.labelRef.Size = new System.Drawing.Size(69, 23);
            this.labelRef.TabIndex = 4;
            this.labelRef.Text = "Ref#";
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindNow.Location = new System.Drawing.Point(978, 15);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 7;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // txtSPNo1
            // 
            this.txtSPNo1.BackColor = System.Drawing.Color.White;
            this.txtSPNo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo1.Location = new System.Drawing.Point(81, 19);
            this.txtSPNo1.Name = "txtSPNo1";
            this.txtSPNo1.Size = new System.Drawing.Size(122, 23);
            this.txtSPNo1.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(9, 19);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(69, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP#";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnUpdateAll);
            this.groupBox2.Controls.Add(this.labelReason);
            this.groupBox2.Controls.Add(this.comboReason);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 420);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1096, 53);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            // 
            // btnUpdateAll
            // 
            this.btnUpdateAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnUpdateAll.Location = new System.Drawing.Point(763, 16);
            this.btnUpdateAll.Name = "btnUpdateAll";
            this.btnUpdateAll.Size = new System.Drawing.Size(90, 30);
            this.btnUpdateAll.TabIndex = 9;
            this.btnUpdateAll.Text = "Update All";
            this.btnUpdateAll.UseVisualStyleBackColor = true;
            this.btnUpdateAll.Click += new System.EventHandler(this.BtnUpdateAll_Click);
            // 
            // labelReason
            // 
            this.labelReason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelReason.Location = new System.Drawing.Point(248, 19);
            this.labelReason.Name = "labelReason";
            this.labelReason.Size = new System.Drawing.Size(69, 23);
            this.labelReason.TabIndex = 8;
            this.labelReason.Text = "Reason";
            // 
            // comboReason
            // 
            this.comboReason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboReason.BackColor = System.Drawing.Color.White;
            this.comboReason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboReason.FormattingEnabled = true;
            this.comboReason.IsSupportUnselect = true;
            this.comboReason.Items.AddRange(new object[] {
            "ALL",
            "Bulk",
            "Inventory"});
            this.comboReason.Location = new System.Drawing.Point(320, 19);
            this.comboReason.Name = "comboReason";
            this.comboReason.OldText = "";
            this.comboReason.Size = new System.Drawing.Size(437, 24);
            this.comboReason.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(1000, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(904, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 10;
            this.btnImport.Text = "Create";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // P48
            // 
            this.ClientSize = new System.Drawing.Size(1096, 473);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.EditMode = true;
            this.Name = "P48";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P48 Batch Create Remove From Scrap Whse";
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Grid gridImport;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Label labelLocation;
        private Win.UI.Label labelFabricType;
        private Win.UI.TextBox txtRef;
        private Win.UI.Label labelRef;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtSPNo1;
        private Win.UI.Label labelSPNo;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.Button btnUpdateAll;
        private Win.UI.Label labelReason;
        private Win.UI.ComboBox comboReason;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.TextBox txtSPNo2;
        private Win.UI.Label label1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label labCategory;
        private Class.ComboDropDownList comboCategory;
        private Class.ComboDropDownList txtdropdownlistFabricType;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label label2;
        private Class.Txtseason txtseason;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label lbSeason;
        private Win.UI.Label lbBrand;
        private System.Windows.Forms.Label label3;
        private Class.TxtMtlLocation txtMtlLocation1;
        private Class.TxtMtlLocation txtMtlLocation2;
    }
}
