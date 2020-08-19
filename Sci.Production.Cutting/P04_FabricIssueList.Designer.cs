namespace Sci.Production.Cutting
{
    partial class P04_FabricIssueList
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
            this.gridFabricIssueList = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridFabricIssueList)).BeginInit();
            this.SuspendLayout();
            // 
            // gridFabricIssueList
            // 
            this.gridFabricIssueList.AllowUserToAddRows = false;
            this.gridFabricIssueList.AllowUserToDeleteRows = false;
            this.gridFabricIssueList.AllowUserToResizeRows = false;
            this.gridFabricIssueList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridFabricIssueList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridFabricIssueList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFabricIssueList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridFabricIssueList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridFabricIssueList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridFabricIssueList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridFabricIssueList.Location = new System.Drawing.Point(12, 12);
            this.gridFabricIssueList.Name = "gridFabricIssueList";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridFabricIssueList.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridFabricIssueList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridFabricIssueList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridFabricIssueList.RowTemplate.Height = 24;
            this.gridFabricIssueList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFabricIssueList.Size = new System.Drawing.Size(342, 335);
            this.gridFabricIssueList.TabIndex = 0;
            this.gridFabricIssueList.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(274, 353);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P04_FabricIssueList
            // 
            this.ClientSize = new System.Drawing.Size(363, 389);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridFabricIssueList);
            this.Name = "P04_FabricIssueList";
            this.Text = "Fabric Issue List";
            ((System.ComponentModel.ISupportInitialize)(this.gridFabricIssueList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridFabricIssueList;
        private Win.UI.Button btnClose;
    }
}
