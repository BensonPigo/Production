namespace Sci.Production.Warehouse
{
    partial class P43_Import
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
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.btnUpdateAll = new Sci.Win.UI.Button();
            this.labelReason = new Sci.Win.UI.Label();
            this.comboReason = new Sci.Win.UI.ComboBox();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.txtLocation = new Sci.Win.UI.TextBox();
            this.labelLocation = new Sci.Win.UI.Label();
            this.txtRef = new Sci.Win.UI.TextBox();
            this.labelRef = new Sci.Win.UI.Label();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.gridImport = new Sci.Win.UI.Grid();
            this.txtSeq1 = new Sci.Win.UI.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSeq2 = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.cmbFabric = new Sci.Win.UI.ComboBox();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnUpdateAll);
            this.groupBox2.Controls.Add(this.labelReason);
            this.groupBox2.Controls.Add(this.comboReason);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 477);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1122, 53);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            // 
            // btnUpdateAll
            // 
            this.btnUpdateAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnUpdateAll.Location = new System.Drawing.Point(789, 16);
            this.btnUpdateAll.Name = "btnUpdateAll";
            this.btnUpdateAll.Size = new System.Drawing.Size(90, 30);
            this.btnUpdateAll.TabIndex = 1;
            this.btnUpdateAll.Text = "Update All";
            this.btnUpdateAll.UseVisualStyleBackColor = true;
            this.btnUpdateAll.Click += new System.EventHandler(this.BtnUpdateAll_Click);
            // 
            // labelReason
            // 
            this.labelReason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelReason.Location = new System.Drawing.Point(274, 19);
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
            this.comboReason.Location = new System.Drawing.Point(346, 19);
            this.comboReason.Name = "comboReason";
            this.comboReason.Size = new System.Drawing.Size(437, 24);
            this.comboReason.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(1026, 15);
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
            this.btnImport.Location = new System.Drawing.Point(930, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbFabric);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSeq2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtSeq1);
            this.groupBox1.Controls.Add(this.txtLocation);
            this.groupBox1.Controls.Add(this.labelLocation);
            this.groupBox1.Controls.Add(this.txtRef);
            this.groupBox1.Controls.Add(this.labelRef);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtSPNo);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1122, 58);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // txtLocation
            // 
            this.txtLocation.BackColor = System.Drawing.Color.White;
            this.txtLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocation.Location = new System.Drawing.Point(599, 19);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(106, 23);
            this.txtLocation.TabIndex = 3;
            // 
            // labelLocation
            // 
            this.labelLocation.Location = new System.Drawing.Point(527, 19);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(69, 23);
            this.labelLocation.TabIndex = 7;
            this.labelLocation.Text = "Location";
            // 
            // txtRef
            // 
            this.txtRef.BackColor = System.Drawing.Color.White;
            this.txtRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRef.Location = new System.Drawing.Point(401, 19);
            this.txtRef.Name = "txtRef";
            this.txtRef.Size = new System.Drawing.Size(106, 23);
            this.txtRef.TabIndex = 2;
            // 
            // labelRef
            // 
            this.labelRef.Location = new System.Drawing.Point(329, 19);
            this.labelRef.Name = "labelRef";
            this.labelRef.Size = new System.Drawing.Size(69, 23);
            this.labelRef.TabIndex = 4;
            this.labelRef.Text = "Ref#";
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindNow.Location = new System.Drawing.Point(1004, 15);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 5;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(91, 19);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(122, 23);
            this.txtSPNo.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(19, 19);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(69, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP#";
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
            this.gridImport.Location = new System.Drawing.Point(0, 58);
            this.gridImport.Name = "gridImport";
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 24;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.Size = new System.Drawing.Size(1122, 419);
            this.gridImport.TabIndex = 28;
            this.gridImport.TabStop = false;
            // 
            // txtSeq1
            // 
            this.txtSeq1.BackColor = System.Drawing.Color.White;
            this.txtSeq1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq1.Location = new System.Drawing.Point(218, 19);
            this.txtSeq1.Name = "txtSeq1";
            this.txtSeq1.Size = new System.Drawing.Size(49, 23);
            this.txtSeq1.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(267, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "-";
            // 
            // txtSeq2
            // 
            this.txtSeq2.BackColor = System.Drawing.Color.White;
            this.txtSeq2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq2.Location = new System.Drawing.Point(278, 19);
            this.txtSeq2.Name = "txtSeq2";
            this.txtSeq2.Size = new System.Drawing.Size(33, 23);
            this.txtSeq2.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(723, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 23);
            this.label1.TabIndex = 13;
            this.label1.Text = "Material Type";
            // 
            // cmbFabric
            // 
            this.cmbFabric.BackColor = System.Drawing.Color.White;
            this.cmbFabric.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbFabric.FormattingEnabled = true;
            this.cmbFabric.IsSupportUnselect = true;
            this.cmbFabric.Location = new System.Drawing.Point(812, 18);
            this.cmbFabric.Name = "cmbFabric";
            this.cmbFabric.Size = new System.Drawing.Size(121, 24);
            this.cmbFabric.TabIndex = 14;
            // 
            // P43_Import
            // 
            this.ClientSize = new System.Drawing.Size(1122, 530);
            this.Controls.Add(this.gridImport);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P43_Import";
            this.Text = "P43. Import Detail";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.GroupBox groupBox2;
        private Win.UI.Button btnUpdateAll;
        private Win.UI.Label labelReason;
        private Win.UI.ComboBox comboReason;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtSeq2;
        private System.Windows.Forms.Label label2;
        private Win.UI.TextBox txtSeq1;
        private Win.UI.TextBox txtLocation;
        private Win.UI.Label labelLocation;
        private Win.UI.TextBox txtRef;
        private Win.UI.Label labelRef;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelSPNo;
        private Win.UI.Grid gridImport;
        private Win.UI.ComboBox cmbFabric;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}
