namespace Sci.Production.PPIC
{
    partial class R01_LastAPSdownloadTime
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnClose = new Sci.Win.UI.Button();
            this.gridLastDownloadAPSDate = new Sci.Win.UI.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.gridLastDownloadAPSDate)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(209, 333);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // gridLastDownloadAPSDate
            // 
            this.gridLastDownloadAPSDate.AllowUserToAddRows = false;
            this.gridLastDownloadAPSDate.AllowUserToDeleteRows = false;
            this.gridLastDownloadAPSDate.AllowUserToResizeRows = false;
            this.gridLastDownloadAPSDate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridLastDownloadAPSDate.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridLastDownloadAPSDate.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridLastDownloadAPSDate.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridLastDownloadAPSDate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridLastDownloadAPSDate.DefaultCellStyle = dataGridViewCellStyle5;
            this.gridLastDownloadAPSDate.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridLastDownloadAPSDate.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridLastDownloadAPSDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridLastDownloadAPSDate.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridLastDownloadAPSDate.Location = new System.Drawing.Point(12, 12);
            this.gridLastDownloadAPSDate.Name = "gridLastDownloadAPSDate";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridLastDownloadAPSDate.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.gridLastDownloadAPSDate.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridLastDownloadAPSDate.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridLastDownloadAPSDate.RowTemplate.Height = 24;
            this.gridLastDownloadAPSDate.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLastDownloadAPSDate.ShowCellToolTips = false;
            this.gridLastDownloadAPSDate.Size = new System.Drawing.Size(277, 315);
            this.gridLastDownloadAPSDate.TabIndex = 1;
            // 
            // R01_LastAPSdownloadTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 375);
            this.Controls.Add(this.gridLastDownloadAPSDate);
            this.Controls.Add(this.btnClose);
            this.Name = "R01_LastAPSdownloadTime";
            this.OnLineHelpID = "Sci.Win.Forms.Base";
            this.Text = "Last APS Download Time";
            ((System.ComponentModel.ISupportInitialize)(this.gridLastDownloadAPSDate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnClose;
        private Win.UI.Grid gridLastDownloadAPSDate;
    }
}