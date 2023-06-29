namespace Sci.Production.IE
{
    partial class P05_NotHitTargetReason
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
            this.gridNotHitTargetReason = new Sci.Win.UI.Grid();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridNotHitTargetReason)).BeginInit();
            this.SuspendLayout();
            // 
            // gridNotHitTargetReason
            // 
            this.gridNotHitTargetReason.AllowUserToAddRows = false;
            this.gridNotHitTargetReason.AllowUserToDeleteRows = false;
            this.gridNotHitTargetReason.AllowUserToResizeRows = false;
            this.gridNotHitTargetReason.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridNotHitTargetReason.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridNotHitTargetReason.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridNotHitTargetReason.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridNotHitTargetReason.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridNotHitTargetReason.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridNotHitTargetReason.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridNotHitTargetReason.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridNotHitTargetReason.Location = new System.Drawing.Point(3, 3);
            this.gridNotHitTargetReason.Name = "gridNotHitTargetReason";
            this.gridNotHitTargetReason.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridNotHitTargetReason.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridNotHitTargetReason.RowTemplate.Height = 24;
            this.gridNotHitTargetReason.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridNotHitTargetReason.ShowCellToolTips = false;
            this.gridNotHitTargetReason.Size = new System.Drawing.Size(898, 391);
            this.gridNotHitTargetReason.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridNotHitTargetReason.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(725, 404);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(811, 404);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // P05_NotHitTargetReason
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 446);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gridNotHitTargetReason);
            this.Name = "P05_NotHitTargetReason";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P05. Not Hit Target Reason";
            this.Controls.SetChildIndex(this.gridNotHitTargetReason, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridNotHitTargetReason)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridNotHitTargetReason;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnCancel;
    }
}