namespace Sci.Production.Shipping
{
    partial class P14_BatchUpdate
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(P14_BatchUpdate));
            this.lblSupplier = new Sci.Win.UI.Label();
            this.lblInvNo = new Sci.Win.UI.Label();
            this.txtSupplier = new Sci.Win.UI.TextBox();
            this.txtInvNo = new Sci.Win.UI.TextBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.gridBatchUpdate = new Sci.Win.UI.Grid();
            this.dateFtyReceive = new Sci.Win.UI.DateBox();
            this.btnUpdateGridFtyRecieveDate = new Sci.Win.UI.PictureBox();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchUpdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdateGridFtyRecieveDate)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSupplier
            // 
            this.lblSupplier.Location = new System.Drawing.Point(9, 9);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(75, 23);
            this.lblSupplier.TabIndex = 0;
            this.lblSupplier.Text = "Supplier";
            // 
            // lblInvNo
            // 
            this.lblInvNo.Location = new System.Drawing.Point(212, 9);
            this.lblInvNo.Name = "lblInvNo";
            this.lblInvNo.Size = new System.Drawing.Size(75, 23);
            this.lblInvNo.TabIndex = 1;
            this.lblInvNo.Text = "Invoice#";
            // 
            // txtSupplier
            // 
            this.txtSupplier.BackColor = System.Drawing.Color.White;
            this.txtSupplier.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSupplier.Location = new System.Drawing.Point(87, 9);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Size = new System.Drawing.Size(100, 23);
            this.txtSupplier.TabIndex = 2;
            // 
            // txtInvNo
            // 
            this.txtInvNo.BackColor = System.Drawing.Color.White;
            this.txtInvNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvNo.Location = new System.Drawing.Point(290, 9);
            this.txtInvNo.Name = "txtInvNo";
            this.txtInvNo.Size = new System.Drawing.Size(138, 23);
            this.txtInvNo.TabIndex = 3;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(705, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 4;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // gridBatchUpdate
            // 
            this.gridBatchUpdate.AllowUserToAddRows = false;
            this.gridBatchUpdate.AllowUserToDeleteRows = false;
            this.gridBatchUpdate.AllowUserToResizeRows = false;
            this.gridBatchUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridBatchUpdate.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBatchUpdate.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBatchUpdate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBatchUpdate.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBatchUpdate.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBatchUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBatchUpdate.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBatchUpdate.Location = new System.Drawing.Point(6, 41);
            this.gridBatchUpdate.Name = "gridBatchUpdate";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridBatchUpdate.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridBatchUpdate.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBatchUpdate.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBatchUpdate.RowTemplate.Height = 24;
            this.gridBatchUpdate.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBatchUpdate.ShowCellToolTips = false;
            this.gridBatchUpdate.Size = new System.Drawing.Size(796, 364);
            this.gridBatchUpdate.TabIndex = 5;
            // 
            // dateFtyReceive
            // 
            this.dateFtyReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dateFtyReceive.Location = new System.Drawing.Point(468, 415);
            this.dateFtyReceive.Name = "dateFtyReceive";
            this.dateFtyReceive.Size = new System.Drawing.Size(130, 23);
            this.dateFtyReceive.TabIndex = 6;
            // 
            // btnUpdateGridFtyRecieveDate
            // 
            this.btnUpdateGridFtyRecieveDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateGridFtyRecieveDate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdateGridFtyRecieveDate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdateGridFtyRecieveDate.Image")));
            this.btnUpdateGridFtyRecieveDate.Location = new System.Drawing.Point(603, 412);
            this.btnUpdateGridFtyRecieveDate.Name = "btnUpdateGridFtyRecieveDate";
            this.btnUpdateGridFtyRecieveDate.Size = new System.Drawing.Size(24, 30);
            this.btnUpdateGridFtyRecieveDate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnUpdateGridFtyRecieveDate.TabIndex = 7;
            this.btnUpdateGridFtyRecieveDate.TabStop = false;
            this.btnUpdateGridFtyRecieveDate.WaitOnLoad = true;
            this.btnUpdateGridFtyRecieveDate.Click += new System.EventHandler(this.BtnUpdateGridFtyRecieveDate_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(630, 412);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(717, 412);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P14_BatchUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 450);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnUpdateGridFtyRecieveDate);
            this.Controls.Add(this.dateFtyReceive);
            this.Controls.Add(this.gridBatchUpdate);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.txtInvNo);
            this.Controls.Add(this.txtSupplier);
            this.Controls.Add(this.lblInvNo);
            this.Controls.Add(this.lblSupplier);
            this.Name = "P14_BatchUpdate";
            this.OnLineHelpID = "Sci.Win.Forms.Base";
            this.Text = "Batch Update";
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchUpdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdateGridFtyRecieveDate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lblSupplier;
        private Win.UI.Label lblInvNo;
        private Win.UI.TextBox txtSupplier;
        private Win.UI.TextBox txtInvNo;
        private Win.UI.Button btnQuery;
        private Win.UI.Grid gridBatchUpdate;
        private Win.UI.DateBox dateFtyReceive;
        private Win.UI.PictureBox btnUpdateGridFtyRecieveDate;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnClose;
    }
}