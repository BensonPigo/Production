namespace Sci.Production.Warehouse
{
    partial class P03
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.chk_includeJunk = new Sci.Win.UI.CheckBox();
            this.btnNewSearch = new Sci.Win.UI.Button();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.panel3 = new Sci.Win.UI.Panel();
            this.labelJunk = new Sci.Win.UI.Label();
            this.displayJunk = new Sci.Win.UI.DisplayBox();
            this.labelCalSize = new Sci.Win.UI.Label();
            this.displayCalSize = new Sci.Win.UI.DisplayBox();
            this.labelFtySupp = new Sci.Win.UI.Label();
            this.displayFtySupp = new Sci.Win.UI.DisplayBox();
            this.labeluseStock = new Sci.Win.UI.Label();
            this.displayUseStock = new Sci.Win.UI.DisplayBox();
            this.labelSortBy = new Sci.Win.UI.Label();
            this.comboSortBy = new Sci.Win.UI.ComboBox();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel2 = new Sci.Win.UI.Panel();
            this.gridMaterialStatus = new Sci.Win.UI.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMaterialStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chk_includeJunk);
            this.panel1.Controls.Add(this.btnNewSearch);
            this.panel1.Controls.Add(this.txtSPNo);
            this.panel1.Controls.Add(this.labelSPNo);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 88);
            this.panel1.TabIndex = 2;
            // 
            // chk_includeJunk
            // 
            this.chk_includeJunk.AutoSize = true;
            this.chk_includeJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chk_includeJunk.Location = new System.Drawing.Point(483, 14);
            this.chk_includeJunk.Name = "chk_includeJunk";
            this.chk_includeJunk.Size = new System.Drawing.Size(160, 21);
            this.chk_includeJunk.TabIndex = 19;
            this.chk_includeJunk.Text = "Include Junk Material";
            this.chk_includeJunk.UseVisualStyleBackColor = true;
            this.chk_includeJunk.CheckedChanged += new System.EventHandler(this.Chk_includeJunk_CheckedChanged);
            // 
            // btnNewSearch
            // 
            this.btnNewSearch.Location = new System.Drawing.Point(272, 8);
            this.btnNewSearch.Name = "btnNewSearch";
            this.btnNewSearch.Size = new System.Drawing.Size(104, 30);
            this.btnNewSearch.TabIndex = 5;
            this.btnNewSearch.Text = "New Search";
            this.btnNewSearch.UseVisualStyleBackColor = true;
            this.btnNewSearch.Click += new System.EventHandler(this.BtnNewSearch_Click);
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(121, 12);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(145, 23);
            this.txtSPNo.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(9, 12);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(109, 23);
            this.labelSPNo.TabIndex = 18;
            this.labelSPNo.Text = "SP#";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(391, 8);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 1;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelJunk);
            this.panel3.Controls.Add(this.displayJunk);
            this.panel3.Controls.Add(this.labelCalSize);
            this.panel3.Controls.Add(this.displayCalSize);
            this.panel3.Controls.Add(this.labelFtySupp);
            this.panel3.Controls.Add(this.displayFtySupp);
            this.panel3.Controls.Add(this.labeluseStock);
            this.panel3.Controls.Add(this.displayUseStock);
            this.panel3.Controls.Add(this.labelSortBy);
            this.panel3.Controls.Add(this.comboSortBy);
            this.panel3.Controls.Add(this.btnToExcel);
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 601);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1008, 60);
            this.panel3.TabIndex = 3;
            // 
            // labelJunk
            // 
            this.labelJunk.BackColor = System.Drawing.Color.Transparent;
            this.labelJunk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.labelJunk.Location = new System.Drawing.Point(671, 16);
            this.labelJunk.Name = "labelJunk";
            this.labelJunk.Size = new System.Drawing.Size(29, 23);
            this.labelJunk.TabIndex = 47;
            this.labelJunk.Text = "Junk";
            this.labelJunk.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            this.labelJunk.TextStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // 
            // displayJunk
            // 
            this.displayJunk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayJunk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayJunk.Location = new System.Drawing.Point(700, 17);
            this.displayJunk.Name = "displayJunk";
            this.displayJunk.Size = new System.Drawing.Size(20, 21);
            this.displayJunk.TabIndex = 46;
            // 
            // labelCalSize
            // 
            this.labelCalSize.BackColor = System.Drawing.Color.Transparent;
            this.labelCalSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.labelCalSize.Location = new System.Drawing.Point(526, 16);
            this.labelCalSize.Name = "labelCalSize";
            this.labelCalSize.Size = new System.Drawing.Size(111, 23);
            this.labelCalSize.TabIndex = 45;
            this.labelCalSize.Text = "Calculate by SizeItem";
            this.labelCalSize.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            this.labelCalSize.TextStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // 
            // displayCalSize
            // 
            this.displayCalSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCalSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayCalSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCalSize.Location = new System.Drawing.Point(637, 17);
            this.displayCalSize.Name = "displayCalSize";
            this.displayCalSize.Size = new System.Drawing.Size(20, 21);
            this.displayCalSize.TabIndex = 44;
            // 
            // labelFtySupp
            // 
            this.labelFtySupp.BackColor = System.Drawing.Color.Transparent;
            this.labelFtySupp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.labelFtySupp.Location = new System.Drawing.Point(415, 16);
            this.labelFtySupp.Name = "labelFtySupp";
            this.labelFtySupp.Size = new System.Drawing.Size(77, 23);
            this.labelFtySupp.TabIndex = 43;
            this.labelFtySupp.Text = "Factory Supply";
            this.labelFtySupp.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            this.labelFtySupp.TextStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // 
            // displayFtySupp
            // 
            this.displayFtySupp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFtySupp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayFtySupp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFtySupp.Location = new System.Drawing.Point(492, 17);
            this.displayFtySupp.Name = "displayFtySupp";
            this.displayFtySupp.Size = new System.Drawing.Size(20, 21);
            this.displayFtySupp.TabIndex = 42;
            // 
            // labeluseStock
            // 
            this.labeluseStock.BackColor = System.Drawing.Color.Transparent;
            this.labeluseStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.labeluseStock.Location = new System.Drawing.Point(325, 16);
            this.labeluseStock.Name = "labeluseStock";
            this.labeluseStock.Size = new System.Drawing.Size(56, 23);
            this.labeluseStock.TabIndex = 41;
            this.labeluseStock.Text = "Use Stock";
            this.labeluseStock.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            this.labeluseStock.TextStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // 
            // displayUseStock
            // 
            this.displayUseStock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayUseStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayUseStock.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayUseStock.Location = new System.Drawing.Point(384, 17);
            this.displayUseStock.Name = "displayUseStock";
            this.displayUseStock.Size = new System.Drawing.Size(20, 21);
            this.displayUseStock.TabIndex = 40;
            // 
            // labelSortBy
            // 
            this.labelSortBy.Location = new System.Drawing.Point(9, 15);
            this.labelSortBy.Name = "labelSortBy";
            this.labelSortBy.Size = new System.Drawing.Size(109, 23);
            this.labelSortBy.TabIndex = 34;
            this.labelSortBy.Text = "Sort By";
            // 
            // comboSortBy
            // 
            this.comboSortBy.BackColor = System.Drawing.Color.White;
            this.comboSortBy.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboSortBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSortBy.FormattingEnabled = true;
            this.comboSortBy.IsSupportUnselect = true;
            this.comboSortBy.Items.AddRange(new object[] {
            "Refno",
            "Seq"});
            this.comboSortBy.Location = new System.Drawing.Point(121, 15);
            this.comboSortBy.Name = "comboSortBy";
            this.comboSortBy.OldText = "";
            this.comboSortBy.Size = new System.Drawing.Size(121, 24);
            this.comboSortBy.TabIndex = 0;
            this.comboSortBy.SelectedIndexChanged += new System.EventHandler(this.ComboSortBy_SelectedIndexChanged);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnToExcel.Location = new System.Drawing.Point(826, 15);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(90, 30);
            this.btnToExcel.TabIndex = 1;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(922, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridMaterialStatus);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 88);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 513);
            this.panel2.TabIndex = 4;
            // 
            // gridMaterialStatus
            // 
            this.gridMaterialStatus.AllowUserToAddRows = false;
            this.gridMaterialStatus.AllowUserToDeleteRows = false;
            this.gridMaterialStatus.AllowUserToResizeRows = false;
            this.gridMaterialStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMaterialStatus.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridMaterialStatus.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridMaterialStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMaterialStatus.DataSource = this.listControlBindingSource1;
            this.gridMaterialStatus.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridMaterialStatus.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridMaterialStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridMaterialStatus.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridMaterialStatus.Location = new System.Drawing.Point(3, 4);
            this.gridMaterialStatus.Name = "gridMaterialStatus";
            this.gridMaterialStatus.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridMaterialStatus.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridMaterialStatus.RowTemplate.Height = 24;
            this.gridMaterialStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMaterialStatus.ShowCellToolTips = false;
            this.gridMaterialStatus.Size = new System.Drawing.Size(1002, 504);
            this.gridMaterialStatus.TabIndex = 0;
            this.gridMaterialStatus.TabStop = false;
            this.gridMaterialStatus.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.GridMaterialStatus_DataError);
            this.gridMaterialStatus.Sorted += new System.EventHandler(this.GridMaterialStatus_Sorted);
            // 
            // P03
            // 
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "P03";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P03. Material Status (With Local Material)";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMaterialStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel1;
        private Win.UI.Button btnQuery;
        private Win.UI.Panel panel3;
        private Win.UI.Button btnClose;
        private Win.UI.Panel panel2;
        private Win.UI.Grid gridMaterialStatus;
        private Win.UI.Button btnToExcel;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelSortBy;
        private Win.UI.ComboBox comboSortBy;
        private Win.UI.Button btnNewSearch;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.DisplayBox displayUseStock;
        private Win.UI.Label labeluseStock;
        private Win.UI.CheckBox chk_includeJunk;
        private Win.UI.Label labelJunk;
        private Win.UI.DisplayBox displayJunk;
        private Win.UI.Label labelCalSize;
        private Win.UI.DisplayBox displayCalSize;
        private Win.UI.Label labelFtySupp;
        private Win.UI.DisplayBox displayFtySupp;
    }
}
