namespace Sci.Production.Warehouse
{
    partial class P50_Import
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
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.labelSortby = new Sci.Win.UI.Label();
            this.comboSortby = new Sci.Win.UI.ComboBox();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.label9 = new Sci.Win.UI.Label();
            this.labelFabricType = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.numRandom = new Sci.Win.UI.NumericBox();
            this.labelCountOfRandom = new Sci.Win.UI.Label();
            this.txtLocation = new Sci.Win.UI.TextBox();
            this.labelLocation = new Sci.Win.UI.Label();
            this.comboFabricType = new Sci.Win.UI.ComboBox();
            this.numPrice2 = new Sci.Win.UI.NumericBox();
            this.numPrice1 = new Sci.Win.UI.NumericBox();
            this.labelUnitPrice = new Sci.Win.UI.Label();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridImport = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label1 = new Sci.Win.UI.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(912, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(816, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindNow.Location = new System.Drawing.Point(895, 54);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 8;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(402, 58);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(122, 23);
            this.txtSPNoStart.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelSortby);
            this.groupBox2.Controls.Add(this.comboSortby);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 477);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1008, 53);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // labelSortby
            // 
            this.labelSortby.Location = new System.Drawing.Point(9, 19);
            this.labelSortby.Name = "labelSortby";
            this.labelSortby.Size = new System.Drawing.Size(75, 23);
            this.labelSortby.TabIndex = 6;
            this.labelSortby.Text = "Sort by";
            // 
            // comboSortby
            // 
            this.comboSortby.BackColor = System.Drawing.Color.White;
            this.comboSortby.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSortby.FormattingEnabled = true;
            this.comboSortby.IsSupportUnselect = true;
            this.comboSortby.Items.AddRange(new object[] {
            "ALL",
            "Bulk",
            "Inventory"});
            this.comboSortby.Location = new System.Drawing.Point(87, 19);
            this.comboSortby.Name = "comboSortby";
            this.comboSortby.Size = new System.Drawing.Size(119, 24);
            this.comboSortby.TabIndex = 5;
            this.comboSortby.SelectedIndexChanged += new System.EventHandler(this.ComboSortby_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.labelFabricType);
            this.groupBox1.Controls.Add(this.labelCategory);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Controls.Add(this.numRandom);
            this.groupBox1.Controls.Add(this.labelCountOfRandom);
            this.groupBox1.Controls.Add(this.txtLocation);
            this.groupBox1.Controls.Add(this.labelLocation);
            this.groupBox1.Controls.Add(this.comboFabricType);
            this.groupBox1.Controls.Add(this.numPrice2);
            this.groupBox1.Controls.Add(this.numPrice1);
            this.groupBox1.Controls.Add(this.labelUnitPrice);
            this.groupBox1.Controls.Add(this.comboCategory);
            this.groupBox1.Controls.Add(this.txtSPNoEnd);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtSPNoStart);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 101);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.Control;
            this.label9.Location = new System.Drawing.Point(530, 60);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 23);
            this.label9.TabIndex = 107;
            this.label9.Text = "~";
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelFabricType
            // 
            this.labelFabricType.Location = new System.Drawing.Point(9, 57);
            this.labelFabricType.Name = "labelFabricType";
            this.labelFabricType.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelFabricType.Size = new System.Drawing.Size(112, 23);
            this.labelFabricType.TabIndex = 99;
            this.labelFabricType.Text = "Material Type";
            this.labelFabricType.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(9, 23);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelCategory.Size = new System.Drawing.Size(112, 23);
            this.labelCategory.TabIndex = 98;
            this.labelCategory.Text = "Category";
            this.labelCategory.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(282, 57);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(117, 23);
            this.labelSPNo.TabIndex = 97;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // numRandom
            // 
            this.numRandom.BackColor = System.Drawing.Color.White;
            this.numRandom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numRandom.Location = new System.Drawing.Point(804, 58);
            this.numRandom.Name = "numRandom";
            this.numRandom.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numRandom.Size = new System.Drawing.Size(63, 23);
            this.numRandom.TabIndex = 7;
            this.numRandom.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numRandom.TextChanged += new System.EventHandler(this.NumRandom_TextChanged);
            // 
            // labelCountOfRandom
            // 
            this.labelCountOfRandom.Location = new System.Drawing.Point(681, 57);
            this.labelCountOfRandom.Name = "labelCountOfRandom";
            this.labelCountOfRandom.Size = new System.Drawing.Size(120, 23);
            this.labelCountOfRandom.TabIndex = 12;
            this.labelCountOfRandom.Text = "Count of Random";
            // 
            // txtLocation
            // 
            this.txtLocation.BackColor = System.Drawing.Color.White;
            this.txtLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocation.Location = new System.Drawing.Point(779, 23);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(100, 23);
            this.txtLocation.TabIndex = 3;
            this.txtLocation.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtLocation_PopUp);
            // 
            // labelLocation
            // 
            this.labelLocation.Location = new System.Drawing.Point(681, 23);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(95, 23);
            this.labelLocation.TabIndex = 10;
            this.labelLocation.Text = "Location";
            // 
            // comboFabricType
            // 
            this.comboFabricType.BackColor = System.Drawing.Color.White;
            this.comboFabricType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFabricType.FormattingEnabled = true;
            this.comboFabricType.IsSupportUnselect = true;
            this.comboFabricType.Items.AddRange(new object[] {
            "ALL",
            "Fabric",
            "Accessory"});
            this.comboFabricType.Location = new System.Drawing.Point(124, 57);
            this.comboFabricType.Name = "comboFabricType";
            this.comboFabricType.Size = new System.Drawing.Size(121, 24);
            this.comboFabricType.TabIndex = 4;
            // 
            // numPrice2
            // 
            this.numPrice2.BackColor = System.Drawing.Color.White;
            this.numPrice2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numPrice2.Location = new System.Drawing.Point(551, 23);
            this.numPrice2.Name = "numPrice2";
            this.numPrice2.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPrice2.Size = new System.Drawing.Size(116, 23);
            this.numPrice2.TabIndex = 2;
            this.numPrice2.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numPrice1
            // 
            this.numPrice1.BackColor = System.Drawing.Color.White;
            this.numPrice1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numPrice1.Location = new System.Drawing.Point(402, 23);
            this.numPrice1.Name = "numPrice1";
            this.numPrice1.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPrice1.Size = new System.Drawing.Size(122, 23);
            this.numPrice1.TabIndex = 1;
            this.numPrice1.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelUnitPrice
            // 
            this.labelUnitPrice.Location = new System.Drawing.Point(282, 21);
            this.labelUnitPrice.Name = "labelUnitPrice";
            this.labelUnitPrice.Size = new System.Drawing.Size(117, 23);
            this.labelUnitPrice.TabIndex = 5;
            this.labelUnitPrice.Text = "Unit Price (US$)";
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Items.AddRange(new object[] {
            "ALL",
            "Bulk",
            "Sample",
            "Material"});
            this.comboCategory.Location = new System.Drawing.Point(124, 22);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.Size = new System.Drawing.Size(121, 24);
            this.comboCategory.TabIndex = 0;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(551, 58);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(116, 23);
            this.txtSPNoEnd.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 101);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 376);
            this.panel1.TabIndex = 20;
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
            this.gridImport.Size = new System.Drawing.Size(1008, 376);
            this.gridImport.TabIndex = 0;
            this.gridImport.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(529, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 23);
            this.label1.TabIndex = 108;
            this.label1.Text = "~";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // P50_Import
            // 
            this.ClientSize = new System.Drawing.Size(1008, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P50_Import";
            this.Text = "P50. Import Detail";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridImport;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.NumericBox numRandom;
        private Win.UI.Label labelCountOfRandom;
        private Win.UI.TextBox txtLocation;
        private Win.UI.Label labelLocation;
        private Win.UI.ComboBox comboFabricType;
        private Win.UI.NumericBox numPrice2;
        private Win.UI.NumericBox numPrice1;
        private Win.UI.Label labelUnitPrice;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.Label labelFabricType;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelSortby;
        private Win.UI.ComboBox comboSortby;
        private Win.UI.Label label9;
        private Win.UI.Label label1;
    }
}
