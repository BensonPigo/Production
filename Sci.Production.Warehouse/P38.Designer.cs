namespace Sci.Production.Warehouse
{
    partial class P38
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
            this.labelSEQ = new Sci.Win.UI.Label();
            this.txtSeq = new Sci.Production.Class.txtSeq();
            this.comboStockType = new Sci.Win.UI.ComboBox();
            this.labelStockType = new Sci.Win.UI.Label();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.labelSP = new Sci.Win.UI.Label();
            this.labelStatus = new Sci.Win.UI.Label();
            this.comboStatus = new Sci.Win.UI.ComboBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.panel2 = new Sci.Win.UI.Panel();
            this.gridMaterialLock = new Sci.Win.UI.Grid();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnExcel = new Sci.Win.UI.Button();
            this.btnLock = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnUnlock = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMaterialLock)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelSEQ);
            this.panel1.Controls.Add(this.txtSeq);
            this.panel1.Controls.Add(this.comboStockType);
            this.panel1.Controls.Add(this.labelStockType);
            this.panel1.Controls.Add(this.txtSP);
            this.panel1.Controls.Add(this.labelSP);
            this.panel1.Controls.Add(this.labelStatus);
            this.panel1.Controls.Add(this.comboStatus);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 45);
            this.panel1.TabIndex = 0;
            // 
            // labelSEQ
            // 
            this.labelSEQ.Lines = 0;
            this.labelSEQ.Location = new System.Drawing.Point(234, 10);
            this.labelSEQ.Name = "labelSEQ";
            this.labelSEQ.Size = new System.Drawing.Size(59, 23);
            this.labelSEQ.TabIndex = 40;
            this.labelSEQ.Text = "SEQ";
            // 
            // txtSeq
            // 
            this.txtSeq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq.Location = new System.Drawing.Point(296, 10);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.seq1 = "";
            this.txtSeq.seq2 = "";
            this.txtSeq.Size = new System.Drawing.Size(61, 23);
            this.txtSeq.TabIndex = 1;
            // 
            // comboStockType
            // 
            this.comboStockType.BackColor = System.Drawing.Color.White;
            this.comboStockType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStockType.FormattingEnabled = true;
            this.comboStockType.IsSupportUnselect = true;
            this.comboStockType.Items.AddRange(new object[] {
            "ALL",
            "Bulk",
            "Inventory"});
            this.comboStockType.Location = new System.Drawing.Point(766, 10);
            this.comboStockType.Name = "comboStockType";
            this.comboStockType.Size = new System.Drawing.Size(100, 24);
            this.comboStockType.TabIndex = 4;
            // 
            // labelStockType
            // 
            this.labelStockType.Lines = 0;
            this.labelStockType.Location = new System.Drawing.Point(668, 10);
            this.labelStockType.Name = "labelStockType";
            this.labelStockType.Size = new System.Drawing.Size(95, 23);
            this.labelStockType.TabIndex = 38;
            this.labelStockType.Text = "Stock Type";
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(71, 10);
            this.txtSP.MaxLength = 13;
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(122, 23);
            this.txtSP.TabIndex = 0;
            // 
            // labelSP
            // 
            this.labelSP.Lines = 0;
            this.labelSP.Location = new System.Drawing.Point(9, 10);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(59, 23);
            this.labelSP.TabIndex = 35;
            this.labelSP.Text = "SP#";
            // 
            // labelStatus
            // 
            this.labelStatus.Lines = 0;
            this.labelStatus.Location = new System.Drawing.Point(415, 10);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(74, 23);
            this.labelStatus.TabIndex = 33;
            this.labelStatus.Text = "Status";
            // 
            // comboStatus
            // 
            this.comboStatus.BackColor = System.Drawing.Color.White;
            this.comboStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.IsSupportUnselect = true;
            this.comboStatus.Items.AddRange(new object[] {
            "All",
            "Locked",
            "Unlocked"});
            this.comboStatus.Location = new System.Drawing.Point(492, 10);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.Size = new System.Drawing.Size(121, 24);
            this.comboStatus.TabIndex = 3;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(916, 6);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 5;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridMaterialLock);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 45);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 616);
            this.panel2.TabIndex = 0;
            // 
            // gridMaterialLock
            // 
            this.gridMaterialLock.AllowUserToAddRows = false;
            this.gridMaterialLock.AllowUserToDeleteRows = false;
            this.gridMaterialLock.AllowUserToResizeRows = false;
            this.gridMaterialLock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMaterialLock.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridMaterialLock.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridMaterialLock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMaterialLock.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridMaterialLock.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridMaterialLock.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridMaterialLock.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridMaterialLock.Location = new System.Drawing.Point(3, 3);
            this.gridMaterialLock.Name = "gridMaterialLock";
            this.gridMaterialLock.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridMaterialLock.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridMaterialLock.RowTemplate.Height = 24;
            this.gridMaterialLock.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMaterialLock.Size = new System.Drawing.Size(1002, 547);
            this.gridMaterialLock.TabIndex = 0;
            this.gridMaterialLock.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnExcel);
            this.panel3.Controls.Add(this.btnLock);
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Controls.Add(this.btnUnlock);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 601);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1008, 60);
            this.panel3.TabIndex = 1;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Location = new System.Drawing.Point(664, 15);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(80, 30);
            this.btnExcel.TabIndex = 3;
            this.btnExcel.Text = "To Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnLock
            // 
            this.btnLock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLock.Location = new System.Drawing.Point(750, 15);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(80, 30);
            this.btnLock.TabIndex = 0;
            this.btnLock.Text = "Lock";
            this.btnLock.UseVisualStyleBackColor = true;
            this.btnLock.Click += new System.EventHandler(this.btnLock_Click);
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
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUnlock
            // 
            this.btnUnlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnlock.Location = new System.Drawing.Point(836, 15);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(80, 30);
            this.btnUnlock.TabIndex = 1;
            this.btnUnlock.Text = "Unlock";
            this.btnUnlock.UseVisualStyleBackColor = true;
            this.btnUnlock.Click += new System.EventHandler(this.btnUnlock_Click);
            // 
            // P38
            // 
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "txtSP";
            this.DefaultControlForEdit = "txtSP";
            this.Name = "P38";
            this.Text = "P38. Material Lock/Unlock for All Transaction";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMaterialLock)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Button btnQuery;
        private Win.UI.Grid gridMaterialLock;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnUnlock;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label labelStatus;
        private Win.UI.ComboBox comboStatus;
        private Win.UI.TextBox txtSP;
        private Win.UI.Label labelSP;
        private Win.UI.Label labelStockType;
        private Win.UI.ComboBox comboStockType;
        private Win.UI.Button btnLock;
        private Win.UI.Button btnExcel;
        private Class.txtSeq txtSeq;
        private Win.UI.Label labelSEQ;
    }
}
