namespace Sci.Production.Packing
{
    partial class P15
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel3 = new Sci.Win.UI.Panel();
            this.txtTransferSlipNo = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnFind = new Sci.Win.UI.Button();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.labelSP = new Sci.Win.UI.Label();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.labelPackID = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.gridDetail = new Sci.Win.UI.Grid();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtTransferSlipNo);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.btnFind);
            this.panel3.Controls.Add(this.txtSP);
            this.panel3.Controls.Add(this.labelSP);
            this.panel3.Controls.Add(this.txtPackID);
            this.panel3.Controls.Add(this.labelPackID);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(739, 80);
            this.panel3.TabIndex = 4;
            // 
            // txtTransferSlipNo
            // 
            this.txtTransferSlipNo.BackColor = System.Drawing.Color.White;
            this.txtTransferSlipNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTransferSlipNo.Location = new System.Drawing.Point(485, 12);
            this.txtTransferSlipNo.Name = "txtTransferSlipNo";
            this.txtTransferSlipNo.Size = new System.Drawing.Size(130, 23);
            this.txtTransferSlipNo.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(382, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 29;
            this.label1.Text = "TransferSlipNo";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(626, 44);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(626, 7);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 6;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(52, 11);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(130, 23);
            this.txtSP.TabIndex = 0;
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(13, 11);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(35, 23);
            this.labelSP.TabIndex = 4;
            this.labelSP.Text = "SP#";
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.Location = new System.Drawing.Point(249, 11);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(130, 23);
            this.txtPackID.TabIndex = 1;
            // 
            // labelPackID
            // 
            this.labelPackID.Location = new System.Drawing.Point(189, 11);
            this.labelPackID.Name = "labelPackID";
            this.labelPackID.Size = new System.Drawing.Size(56, 23);
            this.labelPackID.TabIndex = 2;
            this.labelPackID.Text = "Pack ID";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 407);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(739, 47);
            this.panel4.TabIndex = 6;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(647, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.DataSource = this.listControlBindingSource1;
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(0, 80);
            this.gridDetail.Name = "gridDetail";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.Size = new System.Drawing.Size(739, 327);
            this.gridDetail.TabIndex = 7;
            this.gridDetail.TabStop = false;
            // 
            // P15
            // 
            this.ClientSize = new System.Drawing.Size(739, 454);
            this.Controls.Add(this.gridDetail);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.EditMode = true;
            this.Name = "P15";
            this.Text = "P15.Remove Transfer Slip No";
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.gridDetail, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel3;
        private Win.UI.TextBox txtTransferSlipNo;
        private Win.UI.Label label1;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnFind;
        private Win.UI.TextBox txtSP;
        private Win.UI.Label labelSP;
        private Win.UI.TextBox txtPackID;
        private Win.UI.Label labelPackID;
        private Win.UI.Panel panel4;
        private Win.UI.Button btnClose;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Grid gridDetail;
    }
}
