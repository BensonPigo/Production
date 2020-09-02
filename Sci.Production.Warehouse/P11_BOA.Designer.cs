namespace Sci.Production.Warehouse
{
    partial class P11_BOA
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
            this.panel2 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.gridBOA = new Sci.Win.UI.Grid();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBOA)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 503);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 48);
            this.panel2.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(916, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 503);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // gridBOA
            // 
            this.gridBOA.AllowUserToAddRows = false;
            this.gridBOA.AllowUserToDeleteRows = false;
            this.gridBOA.AllowUserToResizeRows = false;
            this.gridBOA.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBOA.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBOA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBOA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBOA.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBOA.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBOA.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBOA.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBOA.Location = new System.Drawing.Point(3, 0);
            this.gridBOA.Name = "gridBOA";
            this.gridBOA.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBOA.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBOA.RowTemplate.Height = 24;
            this.gridBOA.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBOA.Size = new System.Drawing.Size(1005, 503);
            this.gridBOA.TabIndex = 2;
            this.gridBOA.TabStop = false;
            // 
            // P11_BOA
            // 
            this.ClientSize = new System.Drawing.Size(1008, 551);
            this.Controls.Add(this.gridBOA);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel2);
            this.Name = "P11_BOA";
            this.Text = "P11. BOA";
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBOA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private System.Windows.Forms.Splitter splitter1;
        private Win.UI.Grid gridBOA;
    }
}
