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
            this.labLocation = new Sci.Win.UI.Label();
            this.txtMtlLocation = new Sci.Production.Class.TxtMtlLocation(this.components);
            this.dateATA = new Sci.Win.UI.DateRange();
            this.comboDropDownList1 = new Sci.Production.Class.ComboDropDownList(this.components);
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtReceivingid = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txtwkno = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.labelSEQ = new Sci.Win.UI.Label();
            this.txtSeq = new Sci.Production.Class.TxtSeq();
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
            this.panel1.Controls.Add(this.labLocation);
            this.panel1.Controls.Add(this.txtMtlLocation);
            this.panel1.Controls.Add(this.dateATA);
            this.panel1.Controls.Add(this.comboDropDownList1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtReceivingid);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtwkno);
            this.panel1.Controls.Add(this.label1);
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
            this.panel1.Size = new System.Drawing.Size(1008, 80);
            this.panel1.TabIndex = 0;
            // 
            // labLocation
            // 
            this.labLocation.Location = new System.Drawing.Point(765, 9);
            this.labLocation.Name = "labLocation";
            this.labLocation.Size = new System.Drawing.Size(65, 23);
            this.labLocation.TabIndex = 14;
            this.labLocation.Text = "Location";
            // 
            // txtMtlLocation
            // 
            this.txtMtlLocation.BackColor = System.Drawing.Color.White;
            this.txtMtlLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMtlLocation.Location = new System.Drawing.Point(831, 10);
            this.txtMtlLocation.Name = "txtMtlLocation";
            this.txtMtlLocation.Size = new System.Drawing.Size(76, 23);
            this.txtMtlLocation.StockTypeFilte = "B,I";
            this.txtMtlLocation.TabIndex = 4;
            // 
            // dateATA
            // 
            // 
            // 
            // 
            this.dateATA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateATA.DateBox1.Name = "";
            this.dateATA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateATA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateATA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateATA.DateBox2.Name = "";
            this.dateATA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateATA.DateBox2.TabIndex = 1;
            this.dateATA.Location = new System.Drawing.Point(499, 43);
            this.dateATA.Name = "dateATA";
            this.dateATA.Size = new System.Drawing.Size(280, 23);
            this.dateATA.TabIndex = 7;
            // 
            // comboDropDownList1
            // 
            this.comboDropDownList1.BackColor = System.Drawing.Color.White;
            this.comboDropDownList1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownList1.FormattingEnabled = true;
            this.comboDropDownList1.IsSupportUnselect = true;
            this.comboDropDownList1.Location = new System.Drawing.Point(876, 42);
            this.comboDropDownList1.Name = "comboDropDownList1";
            this.comboDropDownList1.OldText = "";
            this.comboDropDownList1.Size = new System.Drawing.Size(121, 24);
            this.comboDropDownList1.TabIndex = 8;
            this.comboDropDownList1.Type = "Pms_FabricType";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(782, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 23);
            this.label4.TabIndex = 18;
            this.label4.Text = "Material Type";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(410, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 23);
            this.label3.TabIndex = 17;
            this.label3.Text = "Material ATA ";
            // 
            // txtReceivingid
            // 
            this.txtReceivingid.BackColor = System.Drawing.Color.White;
            this.txtReceivingid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReceivingid.Location = new System.Drawing.Point(285, 43);
            this.txtReceivingid.MaxLength = 13;
            this.txtReceivingid.Name = "txtReceivingid";
            this.txtReceivingid.Size = new System.Drawing.Size(122, 23);
            this.txtReceivingid.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(196, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 23);
            this.label2.TabIndex = 16;
            this.label2.Text = "Receiving ID";
            // 
            // txtwkno
            // 
            this.txtwkno.BackColor = System.Drawing.Color.White;
            this.txtwkno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtwkno.Location = new System.Drawing.Point(71, 43);
            this.txtwkno.MaxLength = 13;
            this.txtwkno.Name = "txtwkno";
            this.txtwkno.Size = new System.Drawing.Size(122, 23);
            this.txtwkno.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 23);
            this.label1.TabIndex = 15;
            this.label1.Text = "WK NO";
            // 
            // labelSEQ
            // 
            this.labelSEQ.Location = new System.Drawing.Point(196, 10);
            this.labelSEQ.Name = "labelSEQ";
            this.labelSEQ.Size = new System.Drawing.Size(59, 23);
            this.labelSEQ.TabIndex = 11;
            this.labelSEQ.Text = "SEQ";
            // 
            // txtSeq
            // 
            this.txtSeq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq.Location = new System.Drawing.Point(258, 10);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Seq1 = "";
            this.txtSeq.Seq2 = "";
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
            this.comboStockType.Location = new System.Drawing.Point(648, 9);
            this.comboStockType.Name = "comboStockType";
            this.comboStockType.OldText = "";
            this.comboStockType.Size = new System.Drawing.Size(100, 24);
            this.comboStockType.TabIndex = 3;
            // 
            // labelStockType
            // 
            this.labelStockType.Location = new System.Drawing.Point(550, 9);
            this.labelStockType.Name = "labelStockType";
            this.labelStockType.Size = new System.Drawing.Size(95, 23);
            this.labelStockType.TabIndex = 13;
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
            this.labelSP.Location = new System.Drawing.Point(9, 10);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(59, 23);
            this.labelSP.TabIndex = 10;
            this.labelSP.Text = "SP#";
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(333, 9);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(74, 23);
            this.labelStatus.TabIndex = 12;
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
            this.comboStatus.Location = new System.Drawing.Point(410, 9);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.OldText = "";
            this.comboStatus.Size = new System.Drawing.Size(121, 24);
            this.comboStatus.TabIndex = 2;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(916, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 9;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridMaterialLock);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 80);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 581);
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
            this.gridMaterialLock.ShowCellToolTips = false;
            this.gridMaterialLock.Size = new System.Drawing.Size(1002, 512);
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
            this.btnExcel.TabIndex = 0;
            this.btnExcel.Text = "To Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.BtnExcel_Click);
            // 
            // btnLock
            // 
            this.btnLock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLock.Location = new System.Drawing.Point(750, 15);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(80, 30);
            this.btnLock.TabIndex = 1;
            this.btnLock.Text = "Lock";
            this.btnLock.UseVisualStyleBackColor = true;
            this.btnLock.Click += new System.EventHandler(this.BtnLock_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(922, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnUnlock
            // 
            this.btnUnlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnlock.Location = new System.Drawing.Point(836, 15);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(80, 30);
            this.btnUnlock.TabIndex = 2;
            this.btnUnlock.Text = "Unlock";
            this.btnUnlock.UseVisualStyleBackColor = true;
            this.btnUnlock.Click += new System.EventHandler(this.BtnUnlock_Click);
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
        private Class.TxtSeq txtSeq;
        private Win.UI.Label labelSEQ;
        private Win.UI.TextBox txtReceivingid;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtwkno;
        private Win.UI.Label label1;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Class.ComboDropDownList comboDropDownList1;
        private Win.UI.DateRange dateATA;
        private Win.UI.Label labLocation;
        private Class.TxtMtlLocation txtMtlLocation;
    }
}
