namespace Sci.Production.Warehouse
{
    partial class P03_InventoryStatus
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
            this.gridInventoryStatus = new Sci.Win.UI.Grid();
            this.panel2 = new Sci.Win.UI.Panel();
            this.labelTip = new Sci.Win.UI.Label();
            this.btnClose = new Sci.Win.UI.Button();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridInventoryStatus)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.gridInventoryStatus);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 438);
            this.panel1.TabIndex = 0;
            // 
            // gridInventoryStatus
            // 
            this.gridInventoryStatus.AllowUserToAddRows = false;
            this.gridInventoryStatus.AllowUserToDeleteRows = false;
            this.gridInventoryStatus.AllowUserToResizeRows = false;
            this.gridInventoryStatus.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridInventoryStatus.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridInventoryStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridInventoryStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridInventoryStatus.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridInventoryStatus.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridInventoryStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridInventoryStatus.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridInventoryStatus.Location = new System.Drawing.Point(0, 0);
            this.gridInventoryStatus.Name = "gridInventoryStatus";
            this.gridInventoryStatus.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridInventoryStatus.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridInventoryStatus.RowTemplate.Height = 24;
            this.gridInventoryStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridInventoryStatus.Size = new System.Drawing.Size(1008, 438);
            this.gridInventoryStatus.TabIndex = 0;
            this.gridInventoryStatus.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelTip);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 444);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 48);
            this.panel2.TabIndex = 0;
            // 
            // labelTip
            // 
            this.labelTip.Lines = 0;
            this.labelTip.Location = new System.Drawing.Point(9, 11);
            this.labelTip.Name = "labelTip";
            this.labelTip.Size = new System.Drawing.Size(567, 23);
            this.labelTip.TabIndex = 1;
            this.labelTip.Text = "Tip: A means warehouse Bulk, B means warehouse Inventory, C means warehouse Scrap" +
    ".";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(916, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P03_InventoryStatus
            // 
            this.ClientSize = new System.Drawing.Size(1008, 492);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P03_InventoryStatus";
            this.Text = "Inventory Status";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridInventoryStatus)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Grid gridInventoryStatus;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Label labelTip;
    }
}
