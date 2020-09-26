namespace Sci.Production.Subcon
{
    partial class P30_BatchApprove
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnApprove = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnToExcel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gridHead = new Sci.Win.UI.Grid();
            this.listControlBindingSource_Master = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.gridBody = new Sci.Win.UI.Grid();
            this.listControlBindingSource_Detail = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridHead)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource_Master)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBody)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource_Detail)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnApprove);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnToExcel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 595);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(891, 44);
            this.panel1.TabIndex = 0;
            // 
            // btnApprove
            // 
            this.btnApprove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApprove.Location = new System.Drawing.Point(701, 7);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(86, 30);
            this.btnApprove.TabIndex = 3;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.BtnApprove_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(793, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(86, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(158, 7);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(86, 30);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(10, 7);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(142, 30);
            this.btnToExcel.TabIndex = 0;
            this.btnToExcel.Text = "Lock List To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridHead);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(891, 298);
            this.panel2.TabIndex = 1;
            // 
            // gridHead
            // 
            this.gridHead.AllowUserToAddRows = false;
            this.gridHead.AllowUserToDeleteRows = false;
            this.gridHead.AllowUserToResizeRows = false;
            this.gridHead.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridHead.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridHead.DataSource = this.listControlBindingSource_Master;
            this.gridHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridHead.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridHead.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridHead.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridHead.Location = new System.Drawing.Point(0, 0);
            this.gridHead.Name = "gridHead";
            this.gridHead.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridHead.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridHead.RowTemplate.Height = 24;
            this.gridHead.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridHead.ShowCellToolTips = false;
            this.gridHead.Size = new System.Drawing.Size(891, 298);
            this.gridHead.TabIndex = 1;
            this.gridHead.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gridBody);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 304);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(891, 291);
            this.panel3.TabIndex = 2;
            // 
            // gridBody
            // 
            this.gridBody.AllowUserToAddRows = false;
            this.gridBody.AllowUserToDeleteRows = false;
            this.gridBody.AllowUserToResizeRows = false;
            this.gridBody.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBody.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBody.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBody.DataSource = this.listControlBindingSource_Detail;
            this.gridBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBody.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBody.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBody.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBody.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBody.Location = new System.Drawing.Point(0, 0);
            this.gridBody.Name = "gridBody";
            this.gridBody.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBody.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBody.RowTemplate.Height = 24;
            this.gridBody.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBody.ShowCellToolTips = false;
            this.gridBody.Size = new System.Drawing.Size(891, 291);
            this.gridBody.TabIndex = 1;
            this.gridBody.TabStop = false;
            // 
            // P30_BatchApprove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 639);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P30_BatchApprove";
            this.Text = "Subcon P30 Batch Approve";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridHead)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource_Master)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBody)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource_Detail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnToExcel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private Win.UI.Grid gridHead;
        private Win.UI.Grid gridBody;
        private Win.UI.ListControlBindingSource listControlBindingSource_Master;
        private Win.UI.ListControlBindingSource listControlBindingSource_Detail;
    }
}