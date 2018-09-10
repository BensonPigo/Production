namespace Sci.Production.Warehouse
{
    partial class P26_Import
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
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.BalanceQty = new Sci.Win.UI.CheckBox();
            this.btnUpdateAllLocation = new Sci.Win.UI.Button();
            this.txtLocation2 = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.comboStockType = new Sci.Production.Class.comboDropDownList(this.components);
            this.label1 = new Sci.Win.UI.Label();
            this.cmbMaterialType = new Sci.Win.UI.ComboBox();
            this.labelMaterialType = new Sci.Win.UI.Label();
            this.txtSeq = new Sci.Production.Class.txtSeq();
            this.txtDyelot = new Sci.Win.UI.TextBox();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioSPNo = new Sci.Win.UI.RadioButton();
            this.radioTransactionID = new Sci.Win.UI.RadioButton();
            this.labelDyelot = new Sci.Win.UI.Label();
            this.labelRef = new Sci.Win.UI.Label();
            this.txtLocation = new Sci.Win.UI.TextBox();
            this.txtTransactionID = new Sci.Win.UI.TextBox();
            this.txtRef = new Sci.Win.UI.TextBox();
            this.labelLocation = new Sci.Win.UI.Label();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridImport = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.radioPanel1.SuspendLayout();
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
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(816, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 4;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnQuery.Location = new System.Drawing.Point(918, 25);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(84, 30);
            this.btnQuery.TabIndex = 7;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.IsSupportEditMode = false;
            this.txtSPNo.Location = new System.Drawing.Point(138, 29);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(122, 23);
            this.txtSPNo.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.BalanceQty);
            this.groupBox2.Controls.Add(this.btnUpdateAllLocation);
            this.groupBox2.Controls.Add(this.txtLocation2);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 477);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1008, 53);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // BalanceQty
            // 
            this.BalanceQty.AutoSize = true;
            this.BalanceQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BalanceQty.Location = new System.Drawing.Point(12, 21);
            this.BalanceQty.Name = "BalanceQty";
            this.BalanceQty.Size = new System.Drawing.Size(128, 21);
            this.BalanceQty.TabIndex = 6;
            this.BalanceQty.Text = "Balance Qty > 0";
            this.BalanceQty.UseVisualStyleBackColor = true;
            this.BalanceQty.CheckedChanged += new System.EventHandler(this.BalanceQty_CheckedChanged);
            // 
            // btnUpdateAllLocation
            // 
            this.btnUpdateAllLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateAllLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnUpdateAllLocation.Location = new System.Drawing.Point(635, 16);
            this.btnUpdateAllLocation.Name = "btnUpdateAllLocation";
            this.btnUpdateAllLocation.Size = new System.Drawing.Size(156, 30);
            this.btnUpdateAllLocation.TabIndex = 3;
            this.btnUpdateAllLocation.Text = "Update All Location";
            this.btnUpdateAllLocation.UseVisualStyleBackColor = true;
            this.btnUpdateAllLocation.Click += new System.EventHandler(this.btnUpdateAllLocation_Click);
            // 
            // txtLocation2
            // 
            this.txtLocation2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtLocation2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtLocation2.IsSupportEditMode = false;
            this.txtLocation2.Location = new System.Drawing.Point(220, 20);
            this.txtLocation2.Name = "txtLocation2";
            this.txtLocation2.ReadOnly = true;
            this.txtLocation2.Size = new System.Drawing.Size(400, 23);
            this.txtLocation2.TabIndex = 2;
            this.txtLocation2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtLocation2_MouseDown);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(146, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Location";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboStockType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbMaterialType);
            this.groupBox1.Controls.Add(this.labelMaterialType);
            this.groupBox1.Controls.Add(this.txtSeq);
            this.groupBox1.Controls.Add(this.txtDyelot);
            this.groupBox1.Controls.Add(this.radioPanel1);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Controls.Add(this.labelDyelot);
            this.groupBox1.Controls.Add(this.txtSPNo);
            this.groupBox1.Controls.Add(this.labelRef);
            this.groupBox1.Controls.Add(this.txtLocation);
            this.groupBox1.Controls.Add(this.txtTransactionID);
            this.groupBox1.Controls.Add(this.txtRef);
            this.groupBox1.Controls.Add(this.labelLocation);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 107);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // comboStockType
            // 
            this.comboStockType.BackColor = System.Drawing.Color.White;
            this.comboStockType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStockType.FormattingEnabled = true;
            this.comboStockType.IsSupportUnselect = true;
            this.comboStockType.Location = new System.Drawing.Point(821, 69);
            this.comboStockType.Name = "comboStockType";
            this.comboStockType.OldText = "";
            this.comboStockType.Size = new System.Drawing.Size(91, 24);
            this.comboStockType.TabIndex = 13;
            this.comboStockType.Type = "Pms_StockType";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(730, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "Stock Type";
            // 
            // cmbMaterialType
            // 
            this.cmbMaterialType.BackColor = System.Drawing.Color.White;
            this.cmbMaterialType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbMaterialType.FormattingEnabled = true;
            this.cmbMaterialType.IsSupportUnselect = true;
            this.cmbMaterialType.Location = new System.Drawing.Point(821, 28);
            this.cmbMaterialType.Name = "cmbMaterialType";
            this.cmbMaterialType.OldText = "";
            this.cmbMaterialType.Size = new System.Drawing.Size(91, 24);
            this.cmbMaterialType.TabIndex = 11;
            // 
            // labelMaterialType
            // 
            this.labelMaterialType.Location = new System.Drawing.Point(730, 29);
            this.labelMaterialType.Name = "labelMaterialType";
            this.labelMaterialType.Size = new System.Drawing.Size(88, 23);
            this.labelMaterialType.TabIndex = 10;
            this.labelMaterialType.Text = "Material Type";
            // 
            // txtSeq
            // 
            this.txtSeq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq.Location = new System.Drawing.Point(267, 29);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.seq1 = "";
            this.txtSeq.seq2 = "";
            this.txtSeq.Size = new System.Drawing.Size(61, 23);
            this.txtSeq.TabIndex = 2;
            // 
            // txtDyelot
            // 
            this.txtDyelot.BackColor = System.Drawing.Color.White;
            this.txtDyelot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDyelot.IsSupportEditMode = false;
            this.txtDyelot.Location = new System.Drawing.Point(667, 29);
            this.txtDyelot.Name = "txtDyelot";
            this.txtDyelot.Size = new System.Drawing.Size(60, 23);
            this.txtDyelot.TabIndex = 6;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioSPNo);
            this.radioPanel1.Controls.Add(this.radioTransactionID);
            this.radioPanel1.Location = new System.Drawing.Point(6, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(126, 88);
            this.radioPanel1.TabIndex = 0;
            this.radioPanel1.Value = "1";
            this.radioPanel1.ValueChanged += new System.EventHandler(this.radioPanel1_ValueChanged);
            // 
            // radioSPNo
            // 
            this.radioSPNo.AutoSize = true;
            this.radioSPNo.Checked = true;
            this.radioSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSPNo.Location = new System.Drawing.Point(6, 19);
            this.radioSPNo.Name = "radioSPNo";
            this.radioSPNo.Size = new System.Drawing.Size(52, 21);
            this.radioSPNo.TabIndex = 0;
            this.radioSPNo.TabStop = true;
            this.radioSPNo.Text = "SP#";
            this.radioSPNo.UseVisualStyleBackColor = true;
            this.radioSPNo.Value = "1";
            // 
            // radioTransactionID
            // 
            this.radioTransactionID.AutoSize = true;
            this.radioTransactionID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioTransactionID.Location = new System.Drawing.Point(6, 58);
            this.radioTransactionID.Name = "radioTransactionID";
            this.radioTransactionID.Size = new System.Drawing.Size(118, 21);
            this.radioTransactionID.TabIndex = 1;
            this.radioTransactionID.Text = "Transaction ID";
            this.radioTransactionID.UseVisualStyleBackColor = true;
            this.radioTransactionID.Value = "2";
            // 
            // labelDyelot
            // 
            this.labelDyelot.Location = new System.Drawing.Point(613, 29);
            this.labelDyelot.Name = "labelDyelot";
            this.labelDyelot.Size = new System.Drawing.Size(51, 23);
            this.labelDyelot.TabIndex = 9;
            this.labelDyelot.Text = "Dyelot";
            // 
            // labelRef
            // 
            this.labelRef.Location = new System.Drawing.Point(331, 29);
            this.labelRef.Name = "labelRef";
            this.labelRef.Size = new System.Drawing.Size(40, 23);
            this.labelRef.TabIndex = 3;
            this.labelRef.Text = "Ref#";
            // 
            // txtLocation
            // 
            this.txtLocation.BackColor = System.Drawing.Color.White;
            this.txtLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocation.IsSupportEditMode = false;
            this.txtLocation.Location = new System.Drawing.Point(542, 29);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(68, 23);
            this.txtLocation.TabIndex = 5;
            // 
            // txtTransactionID
            // 
            this.txtTransactionID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtTransactionID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtTransactionID.IsSupportEditMode = false;
            this.txtTransactionID.Location = new System.Drawing.Point(138, 68);
            this.txtTransactionID.Name = "txtTransactionID";
            this.txtTransactionID.ReadOnly = true;
            this.txtTransactionID.Size = new System.Drawing.Size(122, 23);
            this.txtTransactionID.TabIndex = 8;
            // 
            // txtRef
            // 
            this.txtRef.BackColor = System.Drawing.Color.White;
            this.txtRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRef.IsSupportEditMode = false;
            this.txtRef.Location = new System.Drawing.Point(374, 29);
            this.txtRef.Name = "txtRef";
            this.txtRef.Size = new System.Drawing.Size(87, 23);
            this.txtRef.TabIndex = 4;
            // 
            // labelLocation
            // 
            this.labelLocation.Location = new System.Drawing.Point(464, 29);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(75, 23);
            this.labelLocation.TabIndex = 7;
            this.labelLocation.Text = "Location";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 107);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 370);
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
            this.gridImport.ShowCellToolTips = false;
            this.gridImport.Size = new System.Drawing.Size(1008, 370);
            this.gridImport.TabIndex = 0;
            this.gridImport.TabStop = false;
            // 
            // P26_Import
            // 
            this.ClientSize = new System.Drawing.Size(1008, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.DefaultControl = "txtSPNo";
            this.Name = "P26_Import";
            this.Text = "P26. Import Detail";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnQuery;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridImport;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label label2;
        private Win.UI.Button btnUpdateAllLocation;
        private Win.UI.TextBox txtLocation2;
        private Win.UI.TextBox txtDyelot;
        private Win.UI.Label labelDyelot;
        private Win.UI.TextBox txtLocation;
        private Win.UI.Label labelLocation;
        private Win.UI.TextBox txtRef;
        private Win.UI.Label labelRef;
        private Win.UI.TextBox txtTransactionID;
        private Win.UI.RadioButton radioTransactionID;
        private Win.UI.RadioButton radioSPNo;
        private Win.UI.RadioPanel radioPanel1;
        private Class.txtSeq txtSeq;
        private Win.UI.Label labelMaterialType;
        private Win.UI.ComboBox cmbMaterialType;
        private Class.comboDropDownList comboStockType;
        private Win.UI.Label label1;
        private Win.UI.CheckBox BalanceQty;
    }
}
