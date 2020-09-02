namespace Sci.Production.Warehouse
{
    partial class P03_TaipeiInventory
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
            this.gridTaipeiInventoryList = new Sci.Win.UI.Grid();
            this.panel2 = new Sci.Win.UI.Panel();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.labelRemark = new Sci.Win.UI.Label();
            this.labelSortBy = new Sci.Win.UI.Label();
            this.comboSortBy = new Sci.Win.UI.ComboBox();
            this.btnClose = new Sci.Win.UI.Button();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTaipeiInventoryList)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.gridTaipeiInventoryList);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 393);
            this.panel1.TabIndex = 0;
            // 
            // gridTaipeiInventoryList
            // 
            this.gridTaipeiInventoryList.AllowUserToAddRows = false;
            this.gridTaipeiInventoryList.AllowUserToDeleteRows = false;
            this.gridTaipeiInventoryList.AllowUserToResizeRows = false;
            this.gridTaipeiInventoryList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridTaipeiInventoryList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridTaipeiInventoryList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTaipeiInventoryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTaipeiInventoryList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridTaipeiInventoryList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridTaipeiInventoryList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridTaipeiInventoryList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridTaipeiInventoryList.Location = new System.Drawing.Point(0, 0);
            this.gridTaipeiInventoryList.Name = "gridTaipeiInventoryList";
            this.gridTaipeiInventoryList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridTaipeiInventoryList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridTaipeiInventoryList.RowTemplate.Height = 24;
            this.gridTaipeiInventoryList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTaipeiInventoryList.Size = new System.Drawing.Size(1008, 393);
            this.gridTaipeiInventoryList.TabIndex = 2;
            this.gridTaipeiInventoryList.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.editRemark);
            this.panel2.Controls.Add(this.labelRemark);
            this.panel2.Controls.Add(this.labelSortBy);
            this.panel2.Controls.Add(this.comboSortBy);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 399);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 74);
            this.panel2.TabIndex = 0;
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editRemark.IsSupportEditMode = false;
            this.editRemark.Location = new System.Drawing.Point(283, 11);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.ReadOnly = true;
            this.editRemark.Size = new System.Drawing.Size(615, 56);
            this.editRemark.TabIndex = 4;
            // 
            // labelRemark
            // 
            this.labelRemark.Lines = 0;
            this.labelRemark.Location = new System.Drawing.Point(205, 12);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 3;
            this.labelRemark.Text = "Remark";
            // 
            // labelSortBy
            // 
            this.labelSortBy.Lines = 0;
            this.labelSortBy.Location = new System.Drawing.Point(13, 11);
            this.labelSortBy.Name = "labelSortBy";
            this.labelSortBy.Size = new System.Drawing.Size(75, 23);
            this.labelSortBy.TabIndex = 2;
            this.labelSortBy.Text = "Sort By";
            // 
            // comboSortBy
            // 
            this.comboSortBy.BackColor = System.Drawing.Color.White;
            this.comboSortBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSortBy.FormattingEnabled = true;
            this.comboSortBy.IsSupportUnselect = true;
            this.comboSortBy.Items.AddRange(new object[] {
            "Date",
            "Type"});
            this.comboSortBy.Location = new System.Drawing.Point(91, 11);
            this.comboSortBy.Name = "comboSortBy";
            this.comboSortBy.Size = new System.Drawing.Size(100, 24);
            this.comboSortBy.TabIndex = 1;
            this.comboSortBy.SelectedIndexChanged += new System.EventHandler(this.ComboSortBy_SelectedIndexChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(916, 37);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P03_TaipeiInventory
            // 
            this.ClientSize = new System.Drawing.Size(1008, 473);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P03_TaipeiInventory";
            this.Text = "Taipei Inventory List";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTaipeiInventoryList)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Grid gridTaipeiInventoryList;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelSortBy;
        private Win.UI.ComboBox comboSortBy;
        private Win.UI.EditBox editRemark;
    }
}
