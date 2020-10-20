namespace Sci.Production.Warehouse
{
    partial class P03_Scrap
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.label6 = new Sci.Win.UI.Label();
            this.grid_detail = new Sci.Win.UI.Grid();
            this.gridScrapList = new Sci.Win.UI.Grid();
            this.panel2 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.bindingSource2 = new Sci.Win.UI.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_detail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridScrapList)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.grid_detail);
            this.panel1.Controls.Add(this.gridScrapList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(996, 444);
            this.panel1.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label6.Location = new System.Drawing.Point(3, 175);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(163, 23);
            this.label6.TabIndex = 65;
            this.label6.Text = "Detail :";
            this.label6.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // grid_detail
            // 
            this.grid_detail.AllowUserToAddRows = false;
            this.grid_detail.AllowUserToDeleteRows = false;
            this.grid_detail.AllowUserToResizeRows = false;
            this.grid_detail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grid_detail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid_detail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid_detail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_detail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid_detail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid_detail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid_detail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid_detail.Location = new System.Drawing.Point(3, 201);
            this.grid_detail.Name = "grid_detail";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid_detail.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid_detail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_detail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_detail.RowTemplate.Height = 24;
            this.grid_detail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_detail.Size = new System.Drawing.Size(981, 208);
            this.grid_detail.TabIndex = 2;
            this.grid_detail.TabStop = false;
            // 
            // gridScrapList
            // 
            this.gridScrapList.AllowUserToAddRows = false;
            this.gridScrapList.AllowUserToDeleteRows = false;
            this.gridScrapList.AllowUserToResizeRows = false;
            this.gridScrapList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridScrapList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridScrapList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridScrapList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridScrapList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridScrapList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridScrapList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridScrapList.Location = new System.Drawing.Point(3, 3);
            this.gridScrapList.Name = "gridScrapList";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridScrapList.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridScrapList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridScrapList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridScrapList.RowTemplate.Height = 24;
            this.gridScrapList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridScrapList.Size = new System.Drawing.Size(981, 163);
            this.gridScrapList.TabIndex = 1;
            this.gridScrapList.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 444);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(996, 48);
            this.panel2.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(904, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P03_Scrap
            // 
            this.ClientSize = new System.Drawing.Size(996, 492);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "P03_Scrap";
            this.Text = "Scrap Tran";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_detail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridScrapList)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Grid gridScrapList;
        private Win.UI.Grid grid_detail;
        private Win.UI.Label label6;
        private Win.UI.BindingSource bindingSource2;
    }
}
