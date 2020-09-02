namespace Sci.Production.Warehouse
{
    partial class P39_Import
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
            this.txtSP = new Sci.Win.UI.TextBox();
            this.labelSP = new Sci.Win.UI.Label();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.btnUpdateAll = new Sci.Win.UI.Button();
            this.labelReason = new Sci.Win.UI.Label();
            this.comboReason = new Sci.Win.UI.ComboBox();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.txtLocation = new Sci.Win.UI.TextBox();
            this.labelLocation = new Sci.Win.UI.Label();
            this.txtRef = new Sci.Win.UI.TextBox();
            this.labelRef = new Sci.Win.UI.Label();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridImport = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.txtColor = new Sci.Win.UI.TextBox();
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
            this.btnFindNow.Location = new System.Drawing.Point(890, 15);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 5;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(81, 19);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(122, 23);
            this.txtSP.TabIndex = 0;
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(9, 19);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(69, 23);
            this.labelSP.TabIndex = 0;
            this.labelSP.Text = "SP#";
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
            this.groupBox2.Size = new System.Drawing.Size(1008, 53);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // btnUpdateAll
            // 
            this.btnUpdateAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnUpdateAll.Location = new System.Drawing.Point(675, 16);
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
            this.labelReason.Location = new System.Drawing.Point(160, 19);
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
            this.comboReason.Location = new System.Drawing.Point(232, 19);
            this.comboReason.Name = "comboReason";
            this.comboReason.Size = new System.Drawing.Size(437, 24);
            this.comboReason.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtColor);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtLocation);
            this.groupBox1.Controls.Add(this.labelLocation);
            this.groupBox1.Controls.Add(this.txtRef);
            this.groupBox1.Controls.Add(this.labelRef);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtSP);
            this.groupBox1.Controls.Add(this.labelSP);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 58);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // txtLocation
            // 
            this.txtLocation.BackColor = System.Drawing.Color.White;
            this.txtLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocation.Location = new System.Drawing.Point(649, 19);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(106, 23);
            this.txtLocation.TabIndex = 3;
            this.txtLocation.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtLocation_PopUp);
            this.txtLocation.Validating += new System.ComponentModel.CancelEventHandler(this.TxtLocation_Validating);
            // 
            // labelLocation
            // 
            this.labelLocation.Location = new System.Drawing.Point(577, 19);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(69, 23);
            this.labelLocation.TabIndex = 7;
            this.labelLocation.Text = "Location";
            // 
            // txtRef
            // 
            this.txtRef.BackColor = System.Drawing.Color.White;
            this.txtRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRef.Location = new System.Drawing.Point(281, 19);
            this.txtRef.Name = "txtRef";
            this.txtRef.Size = new System.Drawing.Size(106, 23);
            this.txtRef.TabIndex = 1;
            // 
            // labelRef
            // 
            this.labelRef.Location = new System.Drawing.Point(209, 19);
            this.labelRef.Name = "labelRef";
            this.labelRef.Size = new System.Drawing.Size(69, 23);
            this.labelRef.TabIndex = 4;
            this.labelRef.Text = "Ref#";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 58);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 419);
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
            this.gridImport.Size = new System.Drawing.Size(1008, 419);
            this.gridImport.TabIndex = 0;
            this.gridImport.TabStop = false;
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.White;
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtColor.Location = new System.Drawing.Point(465, 19);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(106, 23);
            this.txtColor.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(393, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 23);
            this.label1.TabIndex = 9;
            this.label1.Text = "Color";
            // 
            // P39_Import
            // 
            this.ClientSize = new System.Drawing.Size(1008, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P39_Import";
            this.Text = "P39. Import Detail";
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
        private Win.UI.TextBox txtSP;
        private Win.UI.Label labelSP;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridImport;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.TextBox txtRef;
        private Win.UI.Label labelRef;
        private Win.UI.Label labelLocation;
        private Win.UI.Button btnUpdateAll;
        private Win.UI.Label labelReason;
        private Win.UI.ComboBox comboReason;
        private Win.UI.TextBox txtLocation;
        private Win.UI.TextBox txtColor;
        private Win.UI.Label label1;
    }
}
