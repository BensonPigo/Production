namespace Sci.Production.Subcon
{
    partial class P01_FarmOutList
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
            this.btnClose = new Sci.Win.UI.Button();
            this.gridFarmOutList = new Sci.Win.UI.Grid();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gridFarmOutList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(637, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // gridFarmOutList
            // 
            this.gridFarmOutList.AllowUserToAddRows = false;
            this.gridFarmOutList.AllowUserToDeleteRows = false;
            this.gridFarmOutList.AllowUserToResizeRows = false;
            this.gridFarmOutList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridFarmOutList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridFarmOutList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFarmOutList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFarmOutList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridFarmOutList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridFarmOutList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridFarmOutList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridFarmOutList.Location = new System.Drawing.Point(0, 0);
            this.gridFarmOutList.Name = "gridFarmOutList";
            this.gridFarmOutList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridFarmOutList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridFarmOutList.RowTemplate.Height = 24;
            this.gridFarmOutList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFarmOutList.Size = new System.Drawing.Size(720, 437);
            this.gridFarmOutList.TabIndex = 1;
            this.gridFarmOutList.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridFarmOutList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(720, 437);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 394);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(720, 43);
            this.panel2.TabIndex = 2;
            // 
            // P01_FarmOutList
            // 
            this.ClientSize = new System.Drawing.Size(720, 437);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P01_FarmOutList";
            this.Text = "Farm Out List";
            ((System.ComponentModel.ISupportInitialize)(this.gridFarmOutList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnClose;
        private Win.UI.Grid gridFarmOutList;
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
    }
}
