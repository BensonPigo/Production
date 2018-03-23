namespace Sci.Production.PPIC
{
    partial class P03
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
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnViewDetail = new Sci.Win.UI.Button();
            this.btnBatchUpdate = new Sci.Win.UI.Button();
            this.dateFactoryReceiveDate = new Sci.Win.UI.DateBox();
            this.labelFactoryReceiveDate = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.dateSendDate = new Sci.Win.UI.DateBox();
            this.labelSenddate = new Sci.Win.UI.Label();
            this.txtSeason = new Sci.Win.UI.TextBox();
            this.labelSeason = new Sci.Win.UI.Label();
            this.txtStyleNo = new Sci.Win.UI.TextBox();
            this.labelStyleNo = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridProductionKitsConfirm = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductionKitsConfirm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 484);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(929, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 484);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnViewDetail);
            this.panel3.Controls.Add(this.btnBatchUpdate);
            this.panel3.Controls.Add(this.dateFactoryReceiveDate);
            this.panel3.Controls.Add(this.labelFactoryReceiveDate);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.dateSendDate);
            this.panel3.Controls.Add(this.labelSenddate);
            this.panel3.Controls.Add(this.txtSeason);
            this.panel3.Controls.Add(this.labelSeason);
            this.panel3.Controls.Add(this.txtStyleNo);
            this.panel3.Controls.Add(this.labelStyleNo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(924, 88);
            this.panel3.TabIndex = 3;
            // 
            // btnViewDetail
            // 
            this.btnViewDetail.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnViewDetail.Location = new System.Drawing.Point(808, 51);
            this.btnViewDetail.Name = "btnViewDetail";
            this.btnViewDetail.Size = new System.Drawing.Size(95, 30);
            this.btnViewDetail.TabIndex = 8;
            this.btnViewDetail.Text = "View Detail";
            this.btnViewDetail.UseVisualStyleBackColor = true;
            this.btnViewDetail.Click += new System.EventHandler(this.BtnViewDetail_Click);
            // 
            // btnBatchUpdate
            // 
            this.btnBatchUpdate.Location = new System.Drawing.Point(282, 51);
            this.btnBatchUpdate.Name = "btnBatchUpdate";
            this.btnBatchUpdate.Size = new System.Drawing.Size(131, 30);
            this.btnBatchUpdate.TabIndex = 7;
            this.btnBatchUpdate.Text = "Batch update";
            this.btnBatchUpdate.UseVisualStyleBackColor = true;
            this.btnBatchUpdate.Click += new System.EventHandler(this.BtnBatchUpdate_Click);
            // 
            // dateFactoryReceiveDate
            // 
            this.dateFactoryReceiveDate.IsSupportEditMode = false;
            this.dateFactoryReceiveDate.Location = new System.Drawing.Point(141, 55);
            this.dateFactoryReceiveDate.Name = "dateFactoryReceiveDate";
            this.dateFactoryReceiveDate.Size = new System.Drawing.Size(110, 23);
            this.dateFactoryReceiveDate.TabIndex = 6;
            // 
            // labelFactoryReceiveDate
            // 
            this.labelFactoryReceiveDate.Location = new System.Drawing.Point(8, 55);
            this.labelFactoryReceiveDate.Name = "labelFactoryReceiveDate";
            this.labelFactoryReceiveDate.Size = new System.Drawing.Size(129, 23);
            this.labelFactoryReceiveDate.TabIndex = 7;
            this.labelFactoryReceiveDate.Text = "Factory receive date";
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(823, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 5;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // dateSendDate
            // 
            this.dateSendDate.IsSupportEditMode = false;
            this.dateSendDate.Location = new System.Drawing.Point(575, 16);
            this.dateSendDate.Name = "dateSendDate";
            this.dateSendDate.Size = new System.Drawing.Size(110, 23);
            this.dateSendDate.TabIndex = 4;
            this.dateSendDate.ValueChanged += new System.EventHandler(this.DateSendDate_ValueChanged);
            // 
            // labelSenddate
            // 
            this.labelSenddate.Location = new System.Drawing.Point(494, 16);
            this.labelSenddate.Name = "labelSenddate";
            this.labelSenddate.Size = new System.Drawing.Size(78, 23);
            this.labelSenddate.TabIndex = 4;
            this.labelSenddate.Text = "Send date";
            // 
            // txtSeason
            // 
            this.txtSeason.BackColor = System.Drawing.Color.White;
            this.txtSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeason.IsSupportEditMode = false;
            this.txtSeason.Location = new System.Drawing.Point(360, 16);
            this.txtSeason.Name = "txtSeason";
            this.txtSeason.Size = new System.Drawing.Size(92, 23);
            this.txtSeason.TabIndex = 3;
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(282, 16);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(75, 23);
            this.labelSeason.TabIndex = 2;
            this.labelSeason.Text = "Season";
            // 
            // txtStyleNo
            // 
            this.txtStyleNo.BackColor = System.Drawing.Color.White;
            this.txtStyleNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStyleNo.IsSupportEditMode = false;
            this.txtStyleNo.Location = new System.Drawing.Point(73, 16);
            this.txtStyleNo.Name = "txtStyleNo";
            this.txtStyleNo.Size = new System.Drawing.Size(178, 23);
            this.txtStyleNo.TabIndex = 2;
            // 
            // labelStyleNo
            // 
            this.labelStyleNo.Location = new System.Drawing.Point(8, 16);
            this.labelStyleNo.Name = "labelStyleNo";
            this.labelStyleNo.Size = new System.Drawing.Size(61, 23);
            this.labelStyleNo.TabIndex = 8;
            this.labelStyleNo.Text = "Style No.";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.btnSave);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 440);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(924, 44);
            this.panel4.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(823, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(724, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridProductionKitsConfirm);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 88);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(924, 352);
            this.panel5.TabIndex = 5;
            // 
            // gridProductionKitsConfirm
            // 
            this.gridProductionKitsConfirm.AllowUserToAddRows = false;
            this.gridProductionKitsConfirm.AllowUserToDeleteRows = false;
            this.gridProductionKitsConfirm.AllowUserToResizeRows = false;
            this.gridProductionKitsConfirm.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridProductionKitsConfirm.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridProductionKitsConfirm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridProductionKitsConfirm.DataSource = this.listControlBindingSource1;
            this.gridProductionKitsConfirm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridProductionKitsConfirm.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridProductionKitsConfirm.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridProductionKitsConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridProductionKitsConfirm.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridProductionKitsConfirm.Location = new System.Drawing.Point(0, 0);
            this.gridProductionKitsConfirm.Name = "gridProductionKitsConfirm";
            this.gridProductionKitsConfirm.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridProductionKitsConfirm.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridProductionKitsConfirm.RowTemplate.Height = 24;
            this.gridProductionKitsConfirm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridProductionKitsConfirm.ShowCellToolTips = false;
            this.gridProductionKitsConfirm.Size = new System.Drawing.Size(924, 352);
            this.gridProductionKitsConfirm.TabIndex = 0;
            this.gridProductionKitsConfirm.TabStop = false;
            // 
            // P03
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(934, 484);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "txtStyleNo";
            this.DefaultControlForEdit = "txtStyleNo";
            this.Name = "P03";
            this.Text = "P03. Production Kits confirm";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridProductionKitsConfirm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Button btnBatchUpdate;
        private Win.UI.DateBox dateFactoryReceiveDate;
        private Win.UI.Label labelFactoryReceiveDate;
        private Win.UI.Button btnQuery;
        private Win.UI.DateBox dateSendDate;
        private Win.UI.Label labelSenddate;
        private Win.UI.TextBox txtSeason;
        private Win.UI.Label labelSeason;
        private Win.UI.TextBox txtStyleNo;
        private Win.UI.Label labelStyleNo;
        private Win.UI.Panel panel4;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridProductionKitsConfirm;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnViewDetail;
    }
}
