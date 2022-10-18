namespace Sci.Production.PPIC
{
    partial class P24_Critical_Operations
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridCritical_Operations = new Sci.Win.UI.Grid();
            this.panel3 = new Sci.Win.UI.Panel();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnDownload = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridCritical_Operations)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridCritical_Operations
            // 
            this.gridCritical_Operations.AllowUserToAddRows = false;
            this.gridCritical_Operations.AllowUserToDeleteRows = false;
            this.gridCritical_Operations.AllowUserToResizeRows = false;
            this.gridCritical_Operations.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCritical_Operations.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCritical_Operations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCritical_Operations.DataSource = this.listControlBindingSource1;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridCritical_Operations.DefaultCellStyle = dataGridViewCellStyle1;
            this.gridCritical_Operations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCritical_Operations.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCritical_Operations.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCritical_Operations.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCritical_Operations.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCritical_Operations.Location = new System.Drawing.Point(0, 46);
            this.gridCritical_Operations.Name = "gridCritical_Operations";
            this.gridCritical_Operations.RowHeadersVisible = false;
            this.gridCritical_Operations.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCritical_Operations.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCritical_Operations.RowTemplate.Height = 24;
            this.gridCritical_Operations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCritical_Operations.ShowCellToolTips = false;
            this.gridCritical_Operations.Size = new System.Drawing.Size(493, 392);
            this.gridCritical_Operations.TabIndex = 5;
            this.gridCritical_Operations.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnDownload);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(493, 46);
            this.panel3.TabIndex = 6;
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.Gray;
            this.btnDownload.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDownload.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.ForeColor = System.Drawing.Color.White;
            this.btnDownload.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDownload.Location = new System.Drawing.Point(336, 9);
            this.btnDownload.Margin = new System.Windows.Forms.Padding(0);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(132, 32);
            this.btnDownload.TabIndex = 21;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // P24_Critical_Operations
            // 
            this.ClientSize = new System.Drawing.Size(493, 438);
            this.Controls.Add(this.gridCritical_Operations);
            this.Controls.Add(this.panel3);
            this.Name = "P24_Critical_Operations";
            this.OnLineHelpID = "Sci.Win.Forms.Base";
            this.Text = "Critical Operations";
            ((System.ComponentModel.ISupportInitialize)(this.gridCritical_Operations)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.Grid gridCritical_Operations;
        private Win.UI.Panel panel3;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnDownload;
    }
}
