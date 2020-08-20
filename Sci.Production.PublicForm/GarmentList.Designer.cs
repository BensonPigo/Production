namespace Sci.Production.PublicForm
{
    partial class GarmentList
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
            this.gridGarment = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnArticleForFCode = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridGarment)).BeginInit();
            this.SuspendLayout();
            // 
            // gridGarment
            // 
            this.gridGarment.AllowUserToAddRows = false;
            this.gridGarment.AllowUserToDeleteRows = false;
            this.gridGarment.AllowUserToResizeRows = false;
            this.gridGarment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridGarment.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridGarment.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridGarment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridGarment.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridGarment.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridGarment.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridGarment.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridGarment.Location = new System.Drawing.Point(12, 12);
            this.gridGarment.Name = "gridGarment";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridGarment.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridGarment.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridGarment.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridGarment.RowTemplate.Height = 24;
            this.gridGarment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridGarment.Size = new System.Drawing.Size(1086, 459);
            this.gridGarment.TabIndex = 0;
            this.gridGarment.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1018, 477);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnArticleForFCode
            // 
            this.btnArticleForFCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnArticleForFCode.Location = new System.Drawing.Point(12, 477);
            this.btnArticleForFCode.Name = "btnArticleForFCode";
            this.btnArticleForFCode.Size = new System.Drawing.Size(150, 30);
            this.btnArticleForFCode.TabIndex = 2;
            this.btnArticleForFCode.Text = "Article for F_Code";
            this.btnArticleForFCode.UseVisualStyleBackColor = true;
            this.btnArticleForFCode.Click += new System.EventHandler(this.BtnArticleForFCode_Click);
            // 
            // GarmentList
            // 
            this.ClientSize = new System.Drawing.Size(1110, 511);
            this.Controls.Add(this.btnArticleForFCode);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridGarment);
            this.Name = "GarmentList";
            this.Text = "Garment List";
            ((System.ComponentModel.ISupportInitialize)(this.gridGarment)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridGarment;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnArticleForFCode;
    }
}
