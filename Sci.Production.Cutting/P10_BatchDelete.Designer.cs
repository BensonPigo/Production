namespace Sci.Production.Cutting
{
    partial class P10_BatchDelete
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
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.label10 = new Sci.Win.UI.Label();
            this.txtSPNo1 = new Sci.Win.UI.TextBox();
            this.dateEstCutDate = new Sci.Win.UI.DateBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtCutRef = new Sci.Win.UI.TextBox();
            this.labAddDate = new Sci.Win.UI.Label();
            this.labSpNo = new Sci.Win.UI.Label();
            this.labCutRef = new Sci.Win.UI.Label();
            this.dateAddDate = new Sci.Win.UI.DateRange();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnDelete = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.gridBatchDelete = new Sci.Win.UI.Grid();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchDelete)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtSPNo1);
            this.groupBox1.Controls.Add(this.dateEstCutDate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtCutRef);
            this.groupBox1.Controls.Add(this.labAddDate);
            this.groupBox1.Controls.Add(this.labSpNo);
            this.groupBox1.Controls.Add(this.labCutRef);
            this.groupBox1.Controls.Add(this.dateAddDate);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtSPNo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(893, 87);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(272, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 23);
            this.label10.TabIndex = 116;
            this.label10.Text = "～";
            // 
            // txtSPNo1
            // 
            this.txtSPNo1.BackColor = System.Drawing.Color.White;
            this.txtSPNo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo1.Location = new System.Drawing.Point(301, 22);
            this.txtSPNo1.Name = "txtSPNo1";
            this.txtSPNo1.Size = new System.Drawing.Size(107, 23);
            this.txtSPNo1.TabIndex = 1;
            // 
            // dateEstCutDate
            // 
            this.dateEstCutDate.Location = new System.Drawing.Point(532, 22);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(130, 23);
            this.dateEstCutDate.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(438, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "Est. Cut Date";
            // 
            // txtCutRef
            // 
            this.txtCutRef.BackColor = System.Drawing.Color.White;
            this.txtCutRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRef.Location = new System.Drawing.Point(532, 51);
            this.txtCutRef.Name = "txtCutRef";
            this.txtCutRef.Size = new System.Drawing.Size(108, 23);
            this.txtCutRef.TabIndex = 4;
            // 
            // labAddDate
            // 
            this.labAddDate.Location = new System.Drawing.Point(9, 51);
            this.labAddDate.Name = "labAddDate";
            this.labAddDate.Size = new System.Drawing.Size(137, 23);
            this.labAddDate.TabIndex = 6;
            this.labAddDate.Text = "Bundle Created Date";
            // 
            // labSpNo
            // 
            this.labSpNo.Location = new System.Drawing.Point(9, 22);
            this.labSpNo.Name = "labSpNo";
            this.labSpNo.Size = new System.Drawing.Size(137, 23);
            this.labSpNo.TabIndex = 4;
            this.labSpNo.Text = "SP#";
            // 
            // labCutRef
            // 
            this.labCutRef.Location = new System.Drawing.Point(438, 51);
            this.labCutRef.Name = "labCutRef";
            this.labCutRef.Size = new System.Drawing.Size(91, 23);
            this.labCutRef.TabIndex = 5;
            this.labCutRef.Text = "CutRef";
            // 
            // dateAddDate
            // 
            // 
            // 
            // 
            this.dateAddDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateAddDate.DateBox1.Name = "";
            this.dateAddDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateAddDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateAddDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateAddDate.DateBox2.Name = "";
            this.dateAddDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateAddDate.DateBox2.TabIndex = 1;
            this.dateAddDate.Location = new System.Drawing.Point(149, 51);
            this.dateAddDate.Name = "dateAddDate";
            this.dateAddDate.Size = new System.Drawing.Size(280, 23);
            this.dateAddDate.TabIndex = 3;
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindNow.Location = new System.Drawing.Point(780, 22);
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
            this.txtSPNo.Location = new System.Drawing.Point(149, 22);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(107, 23);
            this.txtSPNo.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 372);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(893, 53);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(797, 17);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnDelete.Location = new System.Drawing.Point(684, 17);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(107, 30);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // gridBatchDelete
            // 
            this.gridBatchDelete.AllowUserToAddRows = false;
            this.gridBatchDelete.AllowUserToDeleteRows = false;
            this.gridBatchDelete.AllowUserToResizeRows = false;
            this.gridBatchDelete.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBatchDelete.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridBatchDelete.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBatchDelete.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBatchDelete.DataSource = this.listControlBindingSource1;
            this.gridBatchDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBatchDelete.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBatchDelete.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBatchDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBatchDelete.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBatchDelete.Location = new System.Drawing.Point(0, 87);
            this.gridBatchDelete.Name = "gridBatchDelete";
            this.gridBatchDelete.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBatchDelete.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBatchDelete.RowTemplate.Height = 24;
            this.gridBatchDelete.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBatchDelete.ShowCellToolTips = false;
            this.gridBatchDelete.Size = new System.Drawing.Size(893, 285);
            this.gridBatchDelete.TabIndex = 21;
            this.gridBatchDelete.TabStop = false;
            // 
            // P10_BatchDelete
            // 
            this.ClientSize = new System.Drawing.Size(893, 425);
            this.Controls.Add(this.gridBatchDelete);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "P10_BatchDelete";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P10. Batch Delete";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchDelete)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.GroupBox groupBox1;
        private Win.UI.Label labCutRef;
        private Win.UI.DateRange dateAddDate;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnDelete;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label labAddDate;
        private Win.UI.Label labSpNo;
        private Win.UI.Grid gridBatchDelete;
        private Win.UI.TextBox txtCutRef;
        private Win.UI.Label label1;
        private Win.UI.DateBox dateEstCutDate;
        private Win.UI.TextBox txtSPNo1;
        private Win.UI.Label label10;
    }
}
