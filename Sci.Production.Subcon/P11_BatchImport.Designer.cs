namespace Sci.Production.Subcon
{
    partial class P11_BatchImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(P11_BatchImport));
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.btnFind = new Sci.Win.UI.Button();
            this.txtSPNo1 = new Sci.Win.UI.TextBox();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.numValue = new Sci.Win.UI.NumericBox();
            this.btnUpdate = new Sci.Win.UI.PictureBox();
            this.txtBatchUpdate = new Sci.Win.UI.TextBox();
            this.comboColumnName = new Sci.Win.UI.ComboBox();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.label7 = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.label5 = new Sci.Win.UI.Label();
            this.txtPO2 = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.txtPO1 = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.txtStyle2 = new Sci.Win.UI.TextBox();
            this.txtStyle1 = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtSPNo2 = new Sci.Win.UI.TextBox();
            this.label3 = new Sci.Win.UI.Label();
            this.labelLocation = new Sci.Win.UI.Label();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridImport = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).BeginInit();
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
            this.btnCancel.Location = new System.Drawing.Point(811, 17);
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
            this.btnImport.Location = new System.Drawing.Point(715, 18);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFind.Location = new System.Drawing.Point(784, 18);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(84, 30);
            this.btnFind.TabIndex = 7;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtSPNo1
            // 
            this.txtSPNo1.BackColor = System.Drawing.Color.White;
            this.txtSPNo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo1.IsSupportEditMode = false;
            this.txtSPNo1.Location = new System.Drawing.Point(73, 22);
            this.txtSPNo1.Name = "txtSPNo1";
            this.txtSPNo1.Size = new System.Drawing.Size(122, 23);
            this.txtSPNo1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numValue);
            this.groupBox2.Controls.Add(this.btnUpdate);
            this.groupBox2.Controls.Add(this.txtBatchUpdate);
            this.groupBox2.Controls.Add(this.comboColumnName);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 477);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(913, 53);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // numValue
            // 
            this.numValue.BackColor = System.Drawing.Color.White;
            this.numValue.DecimalPlaces = 4;
            this.numValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numValue.Location = new System.Drawing.Point(312, 19);
            this.numValue.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numValue.MaxLength = 10;
            this.numValue.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numValue.Name = "numValue";
            this.numValue.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numValue.Size = new System.Drawing.Size(174, 23);
            this.numValue.TabIndex = 12;
            this.numValue.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // btnUpdate
            // 
            this.btnUpdate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.Location = new System.Drawing.Point(492, 15);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(27, 32);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.TabStop = false;
            this.btnUpdate.WaitOnLoad = true;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // txtBatchUpdate
            // 
            this.txtBatchUpdate.BackColor = System.Drawing.Color.White;
            this.txtBatchUpdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBatchUpdate.IsSupportEditMode = false;
            this.txtBatchUpdate.Location = new System.Drawing.Point(312, 19);
            this.txtBatchUpdate.MaxLength = 3;
            this.txtBatchUpdate.Name = "txtBatchUpdate";
            this.txtBatchUpdate.Size = new System.Drawing.Size(174, 23);
            this.txtBatchUpdate.TabIndex = 1;
            // 
            // comboColumnName
            // 
            this.comboColumnName.BackColor = System.Drawing.Color.White;
            this.comboColumnName.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboColumnName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboColumnName.FormattingEnabled = true;
            this.comboColumnName.IsSupportUnselect = true;
            this.comboColumnName.Items.AddRange(new object[] {
            "Price(Unit)",
            "Currency",
            "U/P Exclude VAT(Local currency)",
            "VAT(Local currency)",
            "Kpi Rate"});
            this.comboColumnName.Location = new System.Drawing.Point(49, 18);
            this.comboColumnName.Name = "comboColumnName";
            this.comboColumnName.OldText = "";
            this.comboColumnName.Size = new System.Drawing.Size(254, 24);
            this.comboColumnName.TabIndex = 0;
            this.comboColumnName.SelectedValueChanged += new System.EventHandler(this.ComboColumnName_SelectedValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.dateBuyerDelivery);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtPO2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtPO1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtStyle2);
            this.groupBox1.Controls.Add(this.txtStyle1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSPNo2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnFind);
            this.groupBox1.Controls.Add(this.txtSPNo1);
            this.groupBox1.Controls.Add(this.labelLocation);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(913, 86);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(359, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 23);
            this.label7.TabIndex = 11;
            this.label7.Text = "Buyer Delivery";
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(462, 51);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(585, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 23);
            this.label5.TabIndex = 27;
            this.label5.Text = "~";
            this.label5.TextStyle.Alignment = System.Drawing.ContentAlignment.BottomCenter;
            this.label5.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label5.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtPO2
            // 
            this.txtPO2.BackColor = System.Drawing.Color.White;
            this.txtPO2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPO2.IsSupportEditMode = false;
            this.txtPO2.Location = new System.Drawing.Point(606, 23);
            this.txtPO2.Name = "txtPO2";
            this.txtPO2.Size = new System.Drawing.Size(122, 23);
            this.txtPO2.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(359, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 9;
            this.label6.Text = "PO#";
            // 
            // txtPO1
            // 
            this.txtPO1.BackColor = System.Drawing.Color.White;
            this.txtPO1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPO1.IsSupportEditMode = false;
            this.txtPO1.Location = new System.Drawing.Point(462, 22);
            this.txtPO1.Name = "txtPO1";
            this.txtPO1.Size = new System.Drawing.Size(122, 23);
            this.txtPO1.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(196, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 23);
            this.label4.TabIndex = 23;
            this.label4.Text = "~";
            this.label4.TextStyle.Alignment = System.Drawing.ContentAlignment.BottomCenter;
            this.label4.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtStyle2
            // 
            this.txtStyle2.BackColor = System.Drawing.Color.White;
            this.txtStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStyle2.IsSupportEditMode = false;
            this.txtStyle2.Location = new System.Drawing.Point(217, 53);
            this.txtStyle2.Name = "txtStyle2";
            this.txtStyle2.Size = new System.Drawing.Size(122, 23);
            this.txtStyle2.TabIndex = 5;
            // 
            // txtStyle1
            // 
            this.txtStyle1.BackColor = System.Drawing.Color.White;
            this.txtStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStyle1.IsSupportEditMode = false;
            this.txtStyle1.Location = new System.Drawing.Point(73, 52);
            this.txtStyle1.Name = "txtStyle1";
            this.txtStyle1.Size = new System.Drawing.Size(122, 23);
            this.txtStyle1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(196, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 23);
            this.label1.TabIndex = 20;
            this.label1.Text = "~";
            this.label1.TextStyle.Alignment = System.Drawing.ContentAlignment.BottomCenter;
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPNo2
            // 
            this.txtSPNo2.BackColor = System.Drawing.Color.White;
            this.txtSPNo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo2.IsSupportEditMode = false;
            this.txtSPNo2.Location = new System.Drawing.Point(217, 23);
            this.txtSPNo2.Name = "txtSPNo2";
            this.txtSPNo2.Size = new System.Drawing.Size(122, 23);
            this.txtSPNo2.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "SP#";
            // 
            // labelLocation
            // 
            this.labelLocation.Location = new System.Drawing.Point(14, 52);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(56, 23);
            this.labelLocation.TabIndex = 10;
            this.labelLocation.Text = "Style#";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 86);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(913, 391);
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
            this.gridImport.Size = new System.Drawing.Size(913, 391);
            this.gridImport.TabIndex = 0;
            this.gridImport.TabStop = false;
            // 
            // P11_BatchImport
            // 
            this.ClientSize = new System.Drawing.Size(913, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.DefaultControl = "txtSPNo1";
            this.Name = "P11_BatchImport";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Batch Import";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).EndInit();
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
        private Win.UI.Button btnFind;
        private Win.UI.TextBox txtSPNo1;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridImport;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label labelLocation;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtSPNo2;
        private Win.UI.Label label5;
        private Win.UI.TextBox txtPO2;
        private Win.UI.Label label6;
        private Win.UI.TextBox txtPO1;
        private Win.UI.Label label4;
        private Win.UI.TextBox txtStyle2;
        private Win.UI.TextBox txtStyle1;
        private Win.UI.Label label1;
        private Win.UI.Label label7;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.TextBox txtBatchUpdate;
        private Win.UI.ComboBox comboColumnName;
        private Win.UI.PictureBox btnUpdate;
        private Win.UI.NumericBox numValue;
    }
}
