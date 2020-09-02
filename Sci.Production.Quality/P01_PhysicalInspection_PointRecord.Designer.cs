namespace Sci.Production.Quality
{
    partial class P01_PhysicalInspection_PointRecord
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
            this.gridPhysicalInspection = new Sci.Win.UI.Grid();
            this.btnOK = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridPhysicalInspection)).BeginInit();
            this.SuspendLayout();
            // 
            // gridPhysicalInspection
            // 
            this.gridPhysicalInspection.AllowUserToAddRows = false;
            this.gridPhysicalInspection.AllowUserToDeleteRows = false;
            this.gridPhysicalInspection.AllowUserToResizeRows = false;
            this.gridPhysicalInspection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gridPhysicalInspection.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridPhysicalInspection.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridPhysicalInspection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPhysicalInspection.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridPhysicalInspection.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridPhysicalInspection.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridPhysicalInspection.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridPhysicalInspection.Location = new System.Drawing.Point(6, 12);
            this.gridPhysicalInspection.Name = "gridPhysicalInspection";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridPhysicalInspection.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridPhysicalInspection.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridPhysicalInspection.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridPhysicalInspection.RowTemplate.Height = 24;
            this.gridPhysicalInspection.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPhysicalInspection.Size = new System.Drawing.Size(600, 428);
            this.gridPhysicalInspection.TabIndex = 0;
            this.gridPhysicalInspection.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(526, 446);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // P01_PhysicalInspection_PointRecord
            // 
            this.ClientSize = new System.Drawing.Size(614, 488);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gridPhysicalInspection);
            this.Name = "P01_PhysicalInspection_PointRecord";
            ((System.ComponentModel.ISupportInitialize)(this.gridPhysicalInspection)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridPhysicalInspection;
        private Win.UI.Button btnOK;
    }
}
