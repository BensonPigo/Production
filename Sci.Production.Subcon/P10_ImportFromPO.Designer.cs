namespace Sci.Production.Subcon
{
    partial class P10_ImportFromPO
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
            this.txtArtworkPOIDEnd = new Sci.Win.UI.TextBox();
            this.txtArtworkPOIDStart = new Sci.Win.UI.TextBox();
            this.labelArtworkPOID = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridImportFromPO = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImportFromPO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(908, 15);
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
            this.btnImport.Location = new System.Drawing.Point(812, 16);
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
            this.btnFindNow.Location = new System.Drawing.Point(897, 15);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 4;
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
            this.txtSPNoStart.Size = new System.Drawing.Size(122, 23);
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
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.displayBox1);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 477);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1004, 53);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtArtworkPOIDEnd);
            this.groupBox1.Controls.Add(this.txtArtworkPOIDStart);
            this.groupBox1.Controls.Add(this.labelArtworkPOID);
            this.groupBox1.Controls.Add(this.txtSPNoEnd);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtSPNoStart);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1004, 52);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(632, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 17);
            this.label1.TabIndex = 15;
            this.label1.Text = "～";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(235, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 17);
            this.label8.TabIndex = 14;
            this.label8.Text = "～";
            // 
            // txtArtworkPOIDEnd
            // 
            this.txtArtworkPOIDEnd.BackColor = System.Drawing.Color.White;
            this.txtArtworkPOIDEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtArtworkPOIDEnd.Location = new System.Drawing.Point(660, 15);
            this.txtArtworkPOIDEnd.Name = "txtArtworkPOIDEnd";
            this.txtArtworkPOIDEnd.Size = new System.Drawing.Size(122, 23);
            this.txtArtworkPOIDEnd.TabIndex = 3;
            // 
            // txtArtworkPOIDStart
            // 
            this.txtArtworkPOIDStart.BackColor = System.Drawing.Color.White;
            this.txtArtworkPOIDStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtArtworkPOIDStart.Location = new System.Drawing.Point(504, 15);
            this.txtArtworkPOIDStart.Name = "txtArtworkPOIDStart";
            this.txtArtworkPOIDStart.Size = new System.Drawing.Size(122, 23);
            this.txtArtworkPOIDStart.TabIndex = 2;
            // 
            // labelArtworkPOID
            // 
            this.labelArtworkPOID.Location = new System.Drawing.Point(406, 15);
            this.labelArtworkPOID.Name = "labelArtworkPOID";
            this.labelArtworkPOID.Size = new System.Drawing.Size(95, 23);
            this.labelArtworkPOID.TabIndex = 8;
            this.labelArtworkPOID.Text = "Artwork POID";
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(263, 15);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(122, 23);
            this.txtSPNoEnd.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridImportFromPO);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 52);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1004, 425);
            this.panel1.TabIndex = 20;
            // 
            // gridImportFromPO
            // 
            this.gridImportFromPO.AllowUserToAddRows = false;
            this.gridImportFromPO.AllowUserToDeleteRows = false;
            this.gridImportFromPO.AllowUserToResizeRows = false;
            this.gridImportFromPO.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridImportFromPO.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridImportFromPO.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridImportFromPO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImportFromPO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImportFromPO.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImportFromPO.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImportFromPO.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImportFromPO.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImportFromPO.Location = new System.Drawing.Point(0, 0);
            this.gridImportFromPO.Name = "gridImportFromPO";
            this.gridImportFromPO.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImportFromPO.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImportFromPO.RowTemplate.Height = 24;
            this.gridImportFromPO.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImportFromPO.ShowCellToolTips = false;
            this.gridImportFromPO.Size = new System.Drawing.Size(1004, 425);
            this.gridImportFromPO.TabIndex = 0;
            this.gridImportFromPO.TabStop = false;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(41, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(256, 15);
            this.label7.TabIndex = 15;
            this.label7.Text = "Please note that. RFID data from two supplier.";
            // 
            // displayBox1
            // 
            this.displayBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(12, 22);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(23, 23);
            this.displayBox1.TabIndex = 14;
            // 
            // P10_ImportFromPO
            // 
            this.ClientSize = new System.Drawing.Size(1004, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.DefaultControl = "txtSPNoStart";
            this.Name = "P10_ImportFromPO";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Import From P/O#";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImportFromPO)).EndInit();
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
        private Win.UI.Grid gridImportFromPO;
        private Win.UI.Label labelArtworkPOID;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.TextBox txtArtworkPOIDEnd;
        private Win.UI.TextBox txtArtworkPOIDStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private Win.UI.DisplayBox displayBox1;
    }
}
