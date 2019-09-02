namespace Sci.Production.Shipping
{
    partial class P14
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtSupp = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.txtPayInv = new Sci.Win.UI.TextBox();
            this.labelPackID = new Sci.Win.UI.Label();
            this.dateETA = new Sci.Win.UI.DateRange();
            this.labelReceiveDate = new Sci.Win.UI.Label();
            this.gridCertOfOrigin = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.gridExport = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnSave = new Sci.Win.UI.Button();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCertOfOrigin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.txtSupp);
            this.panel3.Controls.Add(this.labelSPNo);
            this.panel3.Controls.Add(this.txtPayInv);
            this.panel3.Controls.Add(this.labelPackID);
            this.panel3.Controls.Add(this.dateETA);
            this.panel3.Controls.Add(this.labelReceiveDate);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(971, 55);
            this.panel3.TabIndex = 4;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(870, 13);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 3;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtSupp
            // 
            this.txtSupp.BackColor = System.Drawing.Color.White;
            this.txtSupp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSupp.Location = new System.Drawing.Point(355, 17);
            this.txtSupp.Name = "txtSupp";
            this.txtSupp.Size = new System.Drawing.Size(77, 23);
            this.txtSupp.TabIndex = 1;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(286, 17);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(66, 23);
            this.labelSPNo.TabIndex = 4;
            this.labelSPNo.Text = "Supplier";
            // 
            // txtPayInv
            // 
            this.txtPayInv.BackColor = System.Drawing.Color.White;
            this.txtPayInv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPayInv.Location = new System.Drawing.Point(140, 17);
            this.txtPayInv.Name = "txtPayInv";
            this.txtPayInv.Size = new System.Drawing.Size(130, 23);
            this.txtPayInv.TabIndex = 0;
            // 
            // labelPackID
            // 
            this.labelPackID.Location = new System.Drawing.Point(19, 17);
            this.labelPackID.Name = "labelPackID";
            this.labelPackID.Size = new System.Drawing.Size(118, 23);
            this.labelPackID.TabIndex = 2;
            this.labelPackID.Text = "Payment Invoice#";
            // 
            // dateETA
            // 
            this.dateETA.BackColor = System.Drawing.SystemColors.Menu;
            this.dateETA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            // 
            // 
            // 
            this.dateETA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateETA.DateBox1.Name = "";
            this.dateETA.DateBox1.Size = new System.Drawing.Size(124, 23);
            this.dateETA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateETA.DateBox2.Location = new System.Drawing.Point(146, 0);
            this.dateETA.DateBox2.Name = "";
            this.dateETA.DateBox2.Size = new System.Drawing.Size(124, 23);
            this.dateETA.DateBox2.TabIndex = 1;
            this.dateETA.Location = new System.Drawing.Point(519, 17);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(270, 23);
            this.dateETA.TabIndex = 2;
            // 
            // labelReceiveDate
            // 
            this.labelReceiveDate.Location = new System.Drawing.Point(463, 17);
            this.labelReceiveDate.Name = "labelReceiveDate";
            this.labelReceiveDate.Size = new System.Drawing.Size(53, 23);
            this.labelReceiveDate.TabIndex = 0;
            this.labelReceiveDate.Text = "ETA";
            // 
            // gridCertOfOrigin
            // 
            this.gridCertOfOrigin.AllowUserToAddRows = false;
            this.gridCertOfOrigin.AllowUserToDeleteRows = false;
            this.gridCertOfOrigin.AllowUserToResizeRows = false;
            this.gridCertOfOrigin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCertOfOrigin.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCertOfOrigin.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCertOfOrigin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCertOfOrigin.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCertOfOrigin.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCertOfOrigin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCertOfOrigin.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCertOfOrigin.Location = new System.Drawing.Point(12, 61);
            this.gridCertOfOrigin.Name = "gridCertOfOrigin";
            this.gridCertOfOrigin.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCertOfOrigin.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCertOfOrigin.RowTemplate.Height = 24;
            this.gridCertOfOrigin.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCertOfOrigin.ShowCellToolTips = false;
            this.gridCertOfOrigin.Size = new System.Drawing.Size(947, 259);
            this.gridCertOfOrigin.TabIndex = 5;
            this.gridCertOfOrigin.TabStop = false;
            // 
            // gridExport
            // 
            this.gridExport.AllowUserToAddRows = false;
            this.gridExport.AllowUserToDeleteRows = false;
            this.gridExport.AllowUserToResizeRows = false;
            this.gridExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridExport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridExport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridExport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridExport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridExport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridExport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridExport.Location = new System.Drawing.Point(12, 364);
            this.gridExport.Name = "gridExport";
            this.gridExport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridExport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridExport.RowTemplate.Height = 24;
            this.gridExport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridExport.ShowCellToolTips = false;
            this.gridExport.Size = new System.Drawing.Size(947, 164);
            this.gridExport.TabIndex = 6;
            this.gridExport.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(870, 326);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // P14
            // 
            this.ClientSize = new System.Drawing.Size(971, 541);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gridExport);
            this.Controls.Add(this.gridCertOfOrigin);
            this.Controls.Add(this.panel3);
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "P14";
            this.OnLineHelpID = "Sci.Win.Forms.Base";
            this.Text = "P14. Certificate of Origin";
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.gridCertOfOrigin, 0);
            this.Controls.SetChildIndex(this.gridExport, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCertOfOrigin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel3;
        private Win.UI.Button btnQuery;
        private Win.UI.TextBox txtSupp;
        private Win.UI.Label labelSPNo;
        private Win.UI.TextBox txtPayInv;
        private Win.UI.Label labelPackID;
        private Win.UI.DateRange dateETA;
        private Win.UI.Label labelReceiveDate;
        private Win.UI.Grid gridCertOfOrigin;
        private Win.UI.Grid gridExport;
        private Win.UI.Button btnSave;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
    }
}
