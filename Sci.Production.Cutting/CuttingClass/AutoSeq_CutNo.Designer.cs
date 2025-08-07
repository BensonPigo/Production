namespace Sci.Production.Cutting
{
    partial class AutoSeq_CutNo
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
            this.ui_btnClose = new Sci.Win.UI.Button();
            this.btnAutoSeq = new Sci.Win.UI.Button();
            this.ui_grid = new Sci.Win.UI.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.ui_grid)).BeginInit();
            this.SuspendLayout();
            // 
            // ui_btnClose
            // 
            this.ui_btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_btnClose.Location = new System.Drawing.Point(376, 40);
            this.ui_btnClose.Name = "ui_btnClose";
            this.ui_btnClose.Size = new System.Drawing.Size(97, 30);
            this.ui_btnClose.TabIndex = 17;
            this.ui_btnClose.Text = "Close";
            this.ui_btnClose.UseVisualStyleBackColor = true;
            this.ui_btnClose.Click += new System.EventHandler(this.Ui_btnClose_Click);
            // 
            // btnAutoSeq
            // 
            this.btnAutoSeq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoSeq.Location = new System.Drawing.Point(376, 4);
            this.btnAutoSeq.Name = "btnAutoSeq";
            this.btnAutoSeq.Size = new System.Drawing.Size(97, 30);
            this.btnAutoSeq.TabIndex = 16;
            this.btnAutoSeq.Text = "Auto Seq";
            this.btnAutoSeq.UseVisualStyleBackColor = true;
            this.btnAutoSeq.Click += new System.EventHandler(this.BtnAutoSeq_Click);
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
            this.ui_grid.Location = new System.Drawing.Point(12, 4);
            this.ui_grid.Name = "ui_grid";
            this.ui_grid.RowHeadersVisible = false;
            this.ui_grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.ui_grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.ui_grid.RowTemplate.Height = 24;
            this.ui_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_grid.ShowCellToolTips = false;
            this.ui_grid.Size = new System.Drawing.Size(358, 152);
            this.ui_grid.TabIndex = 14;
            this.ui_grid.TabStop = false;
            // 
            // AutoSeq_CutNo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 168);
            this.Controls.Add(this.ui_btnClose);
            this.Controls.Add(this.btnAutoSeq);
            this.Controls.Add(this.ui_grid);
            this.Name = "AutoSeq_CutNo";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "() ";
            this.Controls.SetChildIndex(this.ui_grid, 0);
            this.Controls.SetChildIndex(this.btnAutoSeq, 0);
            this.Controls.SetChildIndex(this.ui_btnClose, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ui_grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.Button ui_btnClose;
        private Win.UI.Button btnAutoSeq;
        private Win.UI.Grid ui_grid;
    }
}