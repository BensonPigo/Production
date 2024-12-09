namespace Sci.Production.Shipping
{
    partial class P44
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
            this.panel3 = new Sci.Win.UI.Panel();
            this.dateCreateDate = new Sci.Win.UI.DateBox();
            this.radioPanel = new Sci.Win.UI.RadioPanel();
            this.radio_ClogR06 = new Sci.Win.UI.RadioButton();
            this.radio_SewingR13 = new Sci.Win.UI.RadioButton();
            this.radio_WHR29 = new Sci.Win.UI.RadioButton();
            this.radio_CuttingR15 = new Sci.Win.UI.RadioButton();
            this.radio_CuttingR14 = new Sci.Win.UI.RadioButton();
            this.radio_WHR30 = new Sci.Win.UI.RadioButton();
            this.radio_WHR28 = new Sci.Win.UI.RadioButton();
            this.gridIcon1 = new Sci.Win.UI.GridIcon();
            this.dateQueryCreateDate = new Sci.Win.UI.DateRange();
            this.labSCIDlv = new Sci.Win.UI.Label();
            this.labSPNo = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnImportFile = new Sci.Win.UI.Button();
            this.grid = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.radioPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dateCreateDate);
            this.panel3.Controls.Add(this.radioPanel);
            this.panel3.Controls.Add(this.gridIcon1);
            this.panel3.Controls.Add(this.dateQueryCreateDate);
            this.panel3.Controls.Add(this.labSCIDlv);
            this.panel3.Controls.Add(this.labSPNo);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.btnImportFile);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(756, 280);
            this.panel3.TabIndex = 20;
            // 
            // dateCreateDate
            // 
            this.dateCreateDate.Location = new System.Drawing.Point(118, 12);
            this.dateCreateDate.Name = "dateCreateDate";
            this.dateCreateDate.Size = new System.Drawing.Size(128, 23);
            this.dateCreateDate.TabIndex = 0;
            // 
            // radioPanel
            // 
            this.radioPanel.Controls.Add(this.radio_ClogR06);
            this.radioPanel.Controls.Add(this.radio_SewingR13);
            this.radioPanel.Controls.Add(this.radio_WHR29);
            this.radioPanel.Controls.Add(this.radio_CuttingR15);
            this.radioPanel.Controls.Add(this.radio_CuttingR14);
            this.radioPanel.Controls.Add(this.radio_WHR30);
            this.radioPanel.Controls.Add(this.radio_WHR28);
            this.radioPanel.Location = new System.Drawing.Point(18, 44);
            this.radioPanel.Name = "radioPanel";
            this.radioPanel.Size = new System.Drawing.Size(544, 197);
            this.radioPanel.TabIndex = 56;
            // 
            // radio_ClogR06
            // 
            this.radio_ClogR06.AutoSize = true;
            this.radio_ClogR06.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radio_ClogR06.Location = new System.Drawing.Point(3, 163);
            this.radio_ClogR06.Name = "radio_ClogR06";
            this.radio_ClogR06.Size = new System.Drawing.Size(220, 21);
            this.radio_ClogR06.TabIndex = 6;
            this.radio_ClogR06.TabStop = true;
            this.radio_ClogR06.Text = "Clog R06. FG Inventory Report";
            this.radio_ClogR06.UseVisualStyleBackColor = true;
            this.radio_ClogR06.Value = "ClogR06";
            // 
            // radio_SewingR13
            // 
            this.radio_SewingR13.AutoSize = true;
            this.radio_SewingR13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radio_SewingR13.Location = new System.Drawing.Point(3, 136);
            this.radio_SewingR13.Name = "radio_SewingR13";
            this.radio_SewingR13.Size = new System.Drawing.Size(259, 21);
            this.radio_SewingR13.TabIndex = 5;
            this.radio_SewingR13.TabStop = true;
            this.radio_SewingR13.Text = "Sewing R13. PPIC Master List Report";
            this.radio_SewingR13.UseVisualStyleBackColor = true;
            this.radio_SewingR13.Value = "SewingR13";
            // 
            // radio_WHR29
            // 
            this.radio_WHR29.AutoSize = true;
            this.radio_WHR29.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radio_WHR29.Location = new System.Drawing.Point(3, 28);
            this.radio_WHR29.Name = "radio_WHR29";
            this.radio_WHR29.Size = new System.Drawing.Size(286, 21);
            this.radio_WHR29.TabIndex = 1;
            this.radio_WHR29.TabStop = true;
            this.radio_WHR29.Text = "Warehouse R29. Replacement Rsleasing";
            this.radio_WHR29.UseVisualStyleBackColor = true;
            this.radio_WHR29.Value = "WarehouseR29";
            // 
            // radio_CuttingR15
            // 
            this.radio_CuttingR15.AutoSize = true;
            this.radio_CuttingR15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radio_CuttingR15.Location = new System.Drawing.Point(3, 109);
            this.radio_CuttingR15.Name = "radio_CuttingR15";
            this.radio_CuttingR15.Size = new System.Drawing.Size(201, 21);
            this.radio_CuttingR15.TabIndex = 4;
            this.radio_CuttingR15.TabStop = true;
            this.radio_CuttingR15.Text = "Cutting R15. Manual Report";
            this.radio_CuttingR15.UseVisualStyleBackColor = true;
            this.radio_CuttingR15.Value = "CuttingR15";
            // 
            // radio_CuttingR14
            // 
            this.radio_CuttingR14.AutoSize = true;
            this.radio_CuttingR14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radio_CuttingR14.Location = new System.Drawing.Point(3, 82);
            this.radio_CuttingR14.Name = "radio_CuttingR14";
            this.radio_CuttingR14.Size = new System.Drawing.Size(336, 21);
            this.radio_CuttingR14.TabIndex = 3;
            this.radio_CuttingR14.TabStop = true;
            this.radio_CuttingR14.Text = "Cutting R14. Loading-Subprocess Manual Report";
            this.radio_CuttingR14.UseVisualStyleBackColor = true;
            this.radio_CuttingR14.Value = "CuttingR14";
            // 
            // radio_WHR30
            // 
            this.radio_WHR30.AutoSize = true;
            this.radio_WHR30.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radio_WHR30.Location = new System.Drawing.Point(3, 54);
            this.radio_WHR30.Name = "radio_WHR30";
            this.radio_WHR30.Size = new System.Drawing.Size(242, 21);
            this.radio_WHR30.TabIndex = 2;
            this.radio_WHR30.TabStop = true;
            this.radio_WHR30.Text = "Warehouse R30. Inventory Report";
            this.radio_WHR30.UseVisualStyleBackColor = true;
            this.radio_WHR30.Value = "WarehouseR30";
            // 
            // radio_WHR28
            // 
            this.radio_WHR28.AutoSize = true;
            this.radio_WHR28.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radio_WHR28.Location = new System.Drawing.Point(3, 1);
            this.radio_WHR28.Name = "radio_WHR28";
            this.radio_WHR28.Size = new System.Drawing.Size(209, 21);
            this.radio_WHR28.TabIndex = 0;
            this.radio_WHR28.TabStop = true;
            this.radio_WHR28.Text = "Warehouse R28. Subcon out";
            this.radio_WHR28.UseVisualStyleBackColor = true;
            this.radio_WHR28.Value = "WarehouseR28";
            // 
            // gridIcon1
            // 
            this.gridIcon1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridIcon1.Location = new System.Drawing.Point(645, 238);
            this.gridIcon1.Name = "gridIcon1";
            this.gridIcon1.Size = new System.Drawing.Size(100, 32);
            this.gridIcon1.TabIndex = 5;
            this.gridIcon1.Text = "gridIcon1";
            this.gridIcon1.RemoveClick += new System.EventHandler(this.GridIcon1_RemoveClick);
            // 
            // dateQueryCreateDate
            // 
            // 
            // 
            // 
            this.dateQueryCreateDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateQueryCreateDate.DateBox1.Name = "";
            this.dateQueryCreateDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateQueryCreateDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateQueryCreateDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateQueryCreateDate.DateBox2.Name = "";
            this.dateQueryCreateDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateQueryCreateDate.DateBox2.TabIndex = 1;
            this.dateQueryCreateDate.IsRequired = false;
            this.dateQueryCreateDate.IsSupportEditMode = false;
            this.dateQueryCreateDate.Location = new System.Drawing.Point(118, 247);
            this.dateQueryCreateDate.Name = "dateQueryCreateDate";
            this.dateQueryCreateDate.Size = new System.Drawing.Size(280, 23);
            this.dateQueryCreateDate.TabIndex = 2;
            // 
            // labSCIDlv
            // 
            this.labSCIDlv.BackColor = System.Drawing.Color.Transparent;
            this.labSCIDlv.Location = new System.Drawing.Point(18, 247);
            this.labSCIDlv.Name = "labSCIDlv";
            this.labSCIDlv.Size = new System.Drawing.Size(97, 23);
            this.labSCIDlv.TabIndex = 54;
            this.labSCIDlv.Text = "Create Date";
            this.labSCIDlv.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labSPNo
            // 
            this.labSPNo.Location = new System.Drawing.Point(18, 12);
            this.labSPNo.Name = "labSPNo";
            this.labSPNo.Size = new System.Drawing.Size(97, 23);
            this.labSPNo.TabIndex = 46;
            this.labSPNo.Text = "Create Date";
            // 
            // btnQuery
            // 
            this.btnQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnQuery.Location = new System.Drawing.Point(461, 242);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(97, 30);
            this.btnQuery.TabIndex = 4;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // btnImportFile
            // 
            this.btnImportFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportFile.Location = new System.Drawing.Point(582, 21);
            this.btnImportFile.Name = "btnImportFile";
            this.btnImportFile.Size = new System.Drawing.Size(132, 30);
            this.btnImportFile.TabIndex = 3;
            this.btnImportFile.Text = "Import File";
            this.btnImportFile.UseVisualStyleBackColor = true;
            this.btnImportFile.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(0, 280);
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(756, 314);
            this.grid.TabIndex = 21;
            this.grid.TabStop = false;
            // 
            // P44
            // 
            this.ClientSize = new System.Drawing.Size(756, 594);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.panel3);
            this.DefaultControl = "dateCreateDate";
            this.Name = "P44";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P44. Statement Report";
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.panel3.ResumeLayout(false);
            this.radioPanel.ResumeLayout(false);
            this.radioPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel3;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnImportFile;
        private Win.UI.Label labSCIDlv;
        private Win.UI.Label labSPNo;
        private Win.UI.Grid grid;
        private Win.UI.DateRange dateQueryCreateDate;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.GridIcon gridIcon1;
        private Win.UI.RadioPanel radioPanel;
        private Win.UI.RadioButton radio_WHR29;
        private Win.UI.RadioButton radio_CuttingR15;
        private Win.UI.RadioButton radio_CuttingR14;
        private Win.UI.RadioButton radio_WHR30;
        private Win.UI.RadioButton radio_WHR28;
        private Win.UI.RadioButton radio_ClogR06;
        private Win.UI.RadioButton radio_SewingR13;
        private Win.UI.DateBox dateCreateDate;
    }
}
