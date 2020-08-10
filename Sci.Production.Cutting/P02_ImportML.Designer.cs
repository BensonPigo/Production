namespace Sci.Production.Cutting
{
    partial class P02_ImportML
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
            this.ui_chkCuttingPiece = new Sci.Win.UI.CheckBox();
            this.ui_btnImportFiles = new Sci.Win.UI.Button();
            this.ui_btnCancel = new Sci.Win.UI.Button();
            this.ui_btnConfirm = new Sci.Win.UI.Button();
            this.ui_btnDelete = new Sci.Win.UI.Button();
            this.ui_grid = new Sci.Win.UI.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.ui_grid)).BeginInit();
            this.SuspendLayout();
            // 
            // ui_chkCuttingPiece
            // 
            this.ui_chkCuttingPiece.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_chkCuttingPiece.AutoSize = true;
            this.ui_chkCuttingPiece.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ui_chkCuttingPiece.Location = new System.Drawing.Point(20, 232);
            this.ui_chkCuttingPiece.Name = "ui_chkCuttingPiece";
            this.ui_chkCuttingPiece.Size = new System.Drawing.Size(110, 21);
            this.ui_chkCuttingPiece.TabIndex = 19;
            this.ui_chkCuttingPiece.Text = "Cutting Piece";
            this.ui_chkCuttingPiece.UseVisualStyleBackColor = true;
            this.ui_chkCuttingPiece.Visible = false;
            // 
            // ui_btnImportFiles
            // 
            this.ui_btnImportFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_btnImportFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ui_btnImportFiles.Location = new System.Drawing.Point(132, 226);
            this.ui_btnImportFiles.Name = "ui_btnImportFiles";
            this.ui_btnImportFiles.Size = new System.Drawing.Size(97, 30);
            this.ui_btnImportFiles.TabIndex = 18;
            this.ui_btnImportFiles.Text = "Import Files";
            this.ui_btnImportFiles.UseVisualStyleBackColor = true;
            this.ui_btnImportFiles.Click += new System.EventHandler(this.Ui_btnImportFiles_Click);
            // 
            // ui_btnCancel
            // 
            this.ui_btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_btnCancel.Location = new System.Drawing.Point(423, 226);
            this.ui_btnCancel.Name = "ui_btnCancel";
            this.ui_btnCancel.Size = new System.Drawing.Size(97, 30);
            this.ui_btnCancel.TabIndex = 17;
            this.ui_btnCancel.Text = "Cancel";
            this.ui_btnCancel.UseVisualStyleBackColor = true;
            this.ui_btnCancel.Click += new System.EventHandler(this.Ui_btnCancel_Click);
            // 
            // ui_btnConfirm
            // 
            this.ui_btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_btnConfirm.Location = new System.Drawing.Point(326, 226);
            this.ui_btnConfirm.Name = "ui_btnConfirm";
            this.ui_btnConfirm.Size = new System.Drawing.Size(97, 30);
            this.ui_btnConfirm.TabIndex = 16;
            this.ui_btnConfirm.Text = "Confirm";
            this.ui_btnConfirm.UseVisualStyleBackColor = true;
            this.ui_btnConfirm.Click += new System.EventHandler(this.Ui_btnConfirm_Click);
            // 
            // ui_btnDelete
            // 
            this.ui_btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_btnDelete.Location = new System.Drawing.Point(229, 226);
            this.ui_btnDelete.Name = "ui_btnDelete";
            this.ui_btnDelete.Size = new System.Drawing.Size(97, 30);
            this.ui_btnDelete.TabIndex = 15;
            this.ui_btnDelete.Text = "Delete";
            this.ui_btnDelete.UseVisualStyleBackColor = true;
            this.ui_btnDelete.Click += new System.EventHandler(this.Ui_btnDelete_Click);
            // 
            // ui_grid
            // 
            this.ui_grid.AllowUserToAddRows = false;
            this.ui_grid.AllowUserToDeleteRows = false;
            this.ui_grid.AllowUserToResizeRows = false;
            this.ui_grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.ui_grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.ui_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.ui_grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.ui_grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ui_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.ui_grid.Location = new System.Drawing.Point(12, 12);
            this.ui_grid.Name = "ui_grid";
            this.ui_grid.RowHeadersVisible = false;
            this.ui_grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.ui_grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.ui_grid.RowTemplate.Height = 24;
            this.ui_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_grid.ShowCellToolTips = false;
            this.ui_grid.Size = new System.Drawing.Size(508, 211);
            this.ui_grid.TabIndex = 14;
            this.ui_grid.TabStop = false;
            // 
            // P02_ImportML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 268);
            this.Controls.Add(this.ui_chkCuttingPiece);
            this.Controls.Add(this.ui_btnImportFiles);
            this.Controls.Add(this.ui_btnCancel);
            this.Controls.Add(this.ui_btnConfirm);
            this.Controls.Add(this.ui_btnDelete);
            this.Controls.Add(this.ui_grid);
            this.Name = "P02_ImportML";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "ImportML";
            this.Controls.SetChildIndex(this.ui_grid, 0);
            this.Controls.SetChildIndex(this.ui_btnDelete, 0);
            this.Controls.SetChildIndex(this.ui_btnConfirm, 0);
            this.Controls.SetChildIndex(this.ui_btnCancel, 0);
            this.Controls.SetChildIndex(this.ui_btnImportFiles, 0);
            this.Controls.SetChildIndex(this.ui_chkCuttingPiece, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ui_grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.CheckBox ui_chkCuttingPiece;
        private Win.UI.Button ui_btnImportFiles;
        private Win.UI.Button ui_btnCancel;
        private Win.UI.Button ui_btnConfirm;
        private Win.UI.Button ui_btnDelete;
        private Win.UI.Grid ui_grid;
    }
}