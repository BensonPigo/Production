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
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridScrapList = new Sci.Win.UI.Grid();
            this.panel2 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridScrapList)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridScrapList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(718, 444);
            this.panel1.TabIndex = 0;
            // 
            // gridScrapList
            // 
            this.gridScrapList.AllowUserToAddRows = false;
            this.gridScrapList.AllowUserToDeleteRows = false;
            this.gridScrapList.AllowUserToResizeRows = false;
            this.gridScrapList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridScrapList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridScrapList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridScrapList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridScrapList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridScrapList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridScrapList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridScrapList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridScrapList.Location = new System.Drawing.Point(0, 0);
            this.gridScrapList.Name = "gridScrapList";
            this.gridScrapList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridScrapList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridScrapList.RowTemplate.Height = 24;
            this.gridScrapList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridScrapList.Size = new System.Drawing.Size(718, 444);
            this.gridScrapList.TabIndex = 0;
            this.gridScrapList.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 444);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(718, 48);
            this.panel2.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(626, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // P03_Scrap
            // 
            this.ClientSize = new System.Drawing.Size(718, 492);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "P03_Scrap";
            this.Text = "Scrap List";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridScrapList)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Grid gridScrapList;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.BindingSource bindingSource1;
    }
}
