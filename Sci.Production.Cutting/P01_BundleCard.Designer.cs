namespace Sci.Production.Cutting
{
    partial class P01_BundleCard
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridBundleCard = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridBundleCard)).BeginInit();
            this.SuspendLayout();
            // 
            // gridBundleCard
            // 
            this.gridBundleCard.AllowUserToAddRows = false;
            this.gridBundleCard.AllowUserToDeleteRows = false;
            this.gridBundleCard.AllowUserToResizeRows = false;
            this.gridBundleCard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridBundleCard.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBundleCard.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBundleCard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBundleCard.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBundleCard.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBundleCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBundleCard.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBundleCard.Location = new System.Drawing.Point(12, 12);
            this.gridBundleCard.Name = "gridBundleCard";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridBundleCard.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridBundleCard.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBundleCard.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBundleCard.RowTemplate.Height = 24;
            this.gridBundleCard.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBundleCard.ShowCellToolTips = false;
            this.gridBundleCard.Size = new System.Drawing.Size(925, 416);
            this.gridBundleCard.TabIndex = 0;
            this.gridBundleCard.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(857, 434);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P01_BundleCard
            // 
            this.ClientSize = new System.Drawing.Size(949, 468);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridBundleCard);
            this.Name = "P01_BundleCard";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Bundle Card";
            this.Controls.SetChildIndex(this.gridBundleCard, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridBundleCard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridBundleCard;
        private Win.UI.Button btnClose;
    }
}
