namespace Sci.Production.Sewing
{
    partial class P04_SaveComplete
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
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnClose = new Sci.Win.UI.Button();
            this.gridSaveComplete = new Sci.Win.UI.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSaveComplete)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(320, 249);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // gridSaveComplete
            // 
            this.gridSaveComplete.AllowUserToAddRows = false;
            this.gridSaveComplete.AllowUserToDeleteRows = false;
            this.gridSaveComplete.AllowUserToResizeRows = false;
            this.gridSaveComplete.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSaveComplete.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSaveComplete.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSaveComplete.DataSource = this.listControlBindingSource1;
            this.gridSaveComplete.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSaveComplete.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSaveComplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSaveComplete.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSaveComplete.Location = new System.Drawing.Point(6, 5);
            this.gridSaveComplete.Name = "gridSaveComplete";
            this.gridSaveComplete.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSaveComplete.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSaveComplete.RowTemplate.Height = 24;
            this.gridSaveComplete.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSaveComplete.Size = new System.Drawing.Size(398, 239);
            this.gridSaveComplete.TabIndex = 2;
            // 
            // P04_SaveComplete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 284);
            this.Controls.Add(this.gridSaveComplete);
            this.Controls.Add(this.btnClose);
            this.Name = "P04_SaveComplete";
            this.Text = "P04_SaveComplete";
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.gridSaveComplete, 0);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSaveComplete)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnClose;
        private Win.UI.Grid gridSaveComplete;
    }
}