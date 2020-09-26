namespace Sci.Production.Subcon
{
    partial class P35_Import
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
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelDelivery = new Sci.Win.UI.Label();
            this.labelPOIssueDate = new Sci.Win.UI.Label();
            this.dateDelivery = new Sci.Win.UI.DateRange();
            this.datePOIssueDate = new Sci.Win.UI.DateRange();
            this.txtPOIDEnd = new Sci.Win.UI.TextBox();
            this.txtPOIDStart = new Sci.Win.UI.TextBox();
            this.labelPOID = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridImport = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.txtReceiving = new Sci.Win.UI.TextBox();
            this.labReceiving = new Sci.Win.UI.Label();
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
            this.btnCancel.Location = new System.Drawing.Point(892, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(796, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 1;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindNow.Location = new System.Drawing.Point(880, 11);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 6;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(107, 15);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(110, 23);
            this.txtSPNoStart.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(9, 15);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(95, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP#";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 500);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(988, 53);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labReceiving);
            this.groupBox1.Controls.Add(this.txtReceiving);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.labelDelivery);
            this.groupBox1.Controls.Add(this.labelPOIssueDate);
            this.groupBox1.Controls.Add(this.dateDelivery);
            this.groupBox1.Controls.Add(this.datePOIssueDate);
            this.groupBox1.Controls.Add(this.txtPOIDEnd);
            this.groupBox1.Controls.Add(this.txtPOIDStart);
            this.groupBox1.Controls.Add(this.labelPOID);
            this.groupBox1.Controls.Add(this.txtSPNoEnd);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtSPNoStart);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(988, 86);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(564, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 17);
            this.label1.TabIndex = 15;
            this.label1.Text = "～";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(224, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 17);
            this.label8.TabIndex = 14;
            this.label8.Text = "～";
            // 
            // labelDelivery
            // 
            this.labelDelivery.Location = new System.Drawing.Point(368, 48);
            this.labelDelivery.Name = "labelDelivery";
            this.labelDelivery.Size = new System.Drawing.Size(77, 23);
            this.labelDelivery.TabIndex = 12;
            this.labelDelivery.Text = "Delivery";
            // 
            // labelPOIssueDate
            // 
            this.labelPOIssueDate.Location = new System.Drawing.Point(9, 48);
            this.labelPOIssueDate.Name = "labelPOIssueDate";
            this.labelPOIssueDate.Size = new System.Drawing.Size(95, 23);
            this.labelPOIssueDate.TabIndex = 11;
            this.labelPOIssueDate.Text = "PO Issue Date";
            // 
            // dateDelivery
            // 
            // 
            // 
            // 
            this.dateDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateDelivery.DateBox1.Name = "";
            this.dateDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateDelivery.DateBox2.Name = "";
            this.dateDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateDelivery.DateBox2.TabIndex = 1;
            this.dateDelivery.IsRequired = false;
            this.dateDelivery.Location = new System.Drawing.Point(448, 48);
            this.dateDelivery.Name = "dateDelivery";
            this.dateDelivery.Size = new System.Drawing.Size(254, 23);
            this.dateDelivery.TabIndex = 5;
            // 
            // datePOIssueDate
            // 
            // 
            // 
            // 
            this.datePOIssueDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.datePOIssueDate.DateBox1.Name = "";
            this.datePOIssueDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.datePOIssueDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.datePOIssueDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.datePOIssueDate.DateBox2.Name = "";
            this.datePOIssueDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.datePOIssueDate.DateBox2.TabIndex = 1;
            this.datePOIssueDate.IsRequired = false;
            this.datePOIssueDate.Location = new System.Drawing.Point(107, 48);
            this.datePOIssueDate.Name = "datePOIssueDate";
            this.datePOIssueDate.Size = new System.Drawing.Size(254, 23);
            this.datePOIssueDate.TabIndex = 4;
            // 
            // txtPOIDEnd
            // 
            this.txtPOIDEnd.BackColor = System.Drawing.Color.White;
            this.txtPOIDEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPOIDEnd.Location = new System.Drawing.Point(592, 15);
            this.txtPOIDEnd.Name = "txtPOIDEnd";
            this.txtPOIDEnd.Size = new System.Drawing.Size(110, 23);
            this.txtPOIDEnd.TabIndex = 3;
            // 
            // txtPOIDStart
            // 
            this.txtPOIDStart.BackColor = System.Drawing.Color.White;
            this.txtPOIDStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPOIDStart.Location = new System.Drawing.Point(448, 15);
            this.txtPOIDStart.Name = "txtPOIDStart";
            this.txtPOIDStart.Size = new System.Drawing.Size(110, 23);
            this.txtPOIDStart.TabIndex = 2;
            // 
            // labelPOID
            // 
            this.labelPOID.Location = new System.Drawing.Point(368, 15);
            this.labelPOID.Name = "labelPOID";
            this.labelPOID.Size = new System.Drawing.Size(77, 23);
            this.labelPOID.TabIndex = 8;
            this.labelPOID.Text = "PO ID";
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(250, 15);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(110, 23);
            this.txtSPNoEnd.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 86);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(988, 414);
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
            this.gridImport.Size = new System.Drawing.Size(988, 414);
            this.gridImport.TabIndex = 0;
            this.gridImport.TabStop = false;
            // 
            // txtReceiving
            // 
            this.txtReceiving.BackColor = System.Drawing.Color.White;
            this.txtReceiving.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReceiving.Location = new System.Drawing.Point(859, 48);
            this.txtReceiving.Name = "txtReceiving";
            this.txtReceiving.Size = new System.Drawing.Size(122, 23);
            this.txtReceiving.TabIndex = 16;
            // 
            // labReceiving
            // 
            this.labReceiving.Location = new System.Drawing.Point(721, 48);
            this.labReceiving.Name = "labReceiving";
            this.labReceiving.Size = new System.Drawing.Size(135, 23);
            this.labReceiving.TabIndex = 17;
            this.labReceiving.Text = "Receiving Incoming#";
            // 
            // P35_Import
            // 
            this.ClientSize = new System.Drawing.Size(988, 553);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.DefaultControl = "txtSPNoStart";
            this.Name = "P35_Import";
            this.Text = "Import From P/O#";
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
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.Label labelSPNo;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridImport;
        private Win.UI.Label labelPOID;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.TextBox txtPOIDEnd;
        private Win.UI.TextBox txtPOIDStart;
        private Win.UI.Label labelDelivery;
        private Win.UI.Label labelPOIssueDate;
        private Win.UI.DateRange dateDelivery;
        private Win.UI.DateRange datePOIssueDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private Win.UI.TextBox txtReceiving;
        private Win.UI.Label labReceiving;
    }
}
