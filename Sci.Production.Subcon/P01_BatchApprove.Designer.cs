namespace Sci.Production.Subcon
{
    partial class P01_BatchApprove
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
            this.gridArtworkPO = new Sci.Win.UI.Grid();
            this.gridArtworkPODetail = new Sci.Win.UI.Grid();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.btnRefresh = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnApprove = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridArtworkPO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridArtworkPODetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // gridArtworkPO
            // 
            this.gridArtworkPO.AllowUserToAddRows = false;
            this.gridArtworkPO.AllowUserToDeleteRows = false;
            this.gridArtworkPO.AllowUserToResizeRows = false;
            this.gridArtworkPO.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridArtworkPO.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridArtworkPO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridArtworkPO.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridArtworkPO.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridArtworkPO.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridArtworkPO.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridArtworkPO.Location = new System.Drawing.Point(-4, -2);
            this.gridArtworkPO.Name = "gridArtworkPO";
            this.gridArtworkPO.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridArtworkPO.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridArtworkPO.RowTemplate.Height = 24;
            this.gridArtworkPO.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridArtworkPO.ShowCellToolTips = false;
            this.gridArtworkPO.Size = new System.Drawing.Size(887, 247);
            this.gridArtworkPO.TabIndex = 0;
            // 
            // gridArtworkPODetail
            // 
            this.gridArtworkPODetail.AllowUserToAddRows = false;
            this.gridArtworkPODetail.AllowUserToDeleteRows = false;
            this.gridArtworkPODetail.AllowUserToResizeRows = false;
            this.gridArtworkPODetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridArtworkPODetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridArtworkPODetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridArtworkPODetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridArtworkPODetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridArtworkPODetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridArtworkPODetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridArtworkPODetail.Location = new System.Drawing.Point(-4, 251);
            this.gridArtworkPODetail.Name = "gridArtworkPODetail";
            this.gridArtworkPODetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridArtworkPODetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridArtworkPODetail.RowTemplate.Height = 24;
            this.gridArtworkPODetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridArtworkPODetail.ShowCellToolTips = false;
            this.gridArtworkPODetail.Size = new System.Drawing.Size(887, 295);
            this.gridArtworkPODetail.TabIndex = 1;
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(12, 552);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(155, 30);
            this.btnToExcel.TabIndex = 2;
            this.btnToExcel.Text = "Lock List To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.btnToExcel_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(173, 552);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 30);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(791, 552);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(700, 551);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(85, 30);
            this.btnApprove.TabIndex = 5;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // P01_BatchApprove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 586);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnToExcel);
            this.Controls.Add(this.gridArtworkPODetail);
            this.Controls.Add(this.gridArtworkPO);
            this.Name = "P01_BatchApprove";
            this.Text = "Subcon P01 Batch Approve";
            ((System.ComponentModel.ISupportInitialize)(this.gridArtworkPO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridArtworkPODetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridArtworkPO;
        private Win.UI.Grid gridArtworkPODetail;
        private Win.UI.Button btnToExcel;
        private Win.UI.Button btnRefresh;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnApprove;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
    }
}