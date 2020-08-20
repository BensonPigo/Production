namespace Sci.Production.Cutting
{
    partial class P01_Cutpartcheck
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
            this.gridCutpartcheck = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridCutpartcheck)).BeginInit();
            this.SuspendLayout();
            // 
            // gridCutpartcheck
            // 
            this.gridCutpartcheck.AllowUserToAddRows = false;
            this.gridCutpartcheck.AllowUserToDeleteRows = false;
            this.gridCutpartcheck.AllowUserToResizeRows = false;
            this.gridCutpartcheck.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCutpartcheck.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCutpartcheck.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCutpartcheck.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCutpartcheck.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCutpartcheck.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCutpartcheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCutpartcheck.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCutpartcheck.Location = new System.Drawing.Point(12, 12);
            this.gridCutpartcheck.Name = "gridCutpartcheck";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridCutpartcheck.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridCutpartcheck.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCutpartcheck.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCutpartcheck.RowTemplate.Height = 24;
            this.gridCutpartcheck.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCutpartcheck.Size = new System.Drawing.Size(898, 416);
            this.gridCutpartcheck.TabIndex = 0;
            this.gridCutpartcheck.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(830, 434);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P01_Cutpartcheck
            // 
            this.ClientSize = new System.Drawing.Size(922, 468);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridCutpartcheck);
            this.Name = "P01_Cutpartcheck";
            ((System.ComponentModel.ISupportInitialize)(this.gridCutpartcheck)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridCutpartcheck;
        private Win.UI.Button btnClose;
    }
}
