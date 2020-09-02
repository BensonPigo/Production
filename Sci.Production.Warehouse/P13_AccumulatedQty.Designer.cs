namespace Sci.Production.Warehouse
{
    partial class P13_AccumulatedQty
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
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.gridAccumlatedQty = new Sci.Win.UI.Grid();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAccumlatedQty)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(701, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // gridAccumlatedQty
            // 
            this.gridAccumlatedQty.AllowUserToAddRows = false;
            this.gridAccumlatedQty.AllowUserToDeleteRows = false;
            this.gridAccumlatedQty.AllowUserToResizeRows = false;
            this.gridAccumlatedQty.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridAccumlatedQty.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridAccumlatedQty.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAccumlatedQty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridAccumlatedQty.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridAccumlatedQty.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridAccumlatedQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridAccumlatedQty.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridAccumlatedQty.Location = new System.Drawing.Point(0, 0);
            this.gridAccumlatedQty.Name = "gridAccumlatedQty";
            this.gridAccumlatedQty.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridAccumlatedQty.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridAccumlatedQty.RowTemplate.Height = 24;
            this.gridAccumlatedQty.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAccumlatedQty.Size = new System.Drawing.Size(784, 389);
            this.gridAccumlatedQty.TabIndex = 1;
            this.gridAccumlatedQty.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridAccumlatedQty);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 389);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 389);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(784, 48);
            this.panel2.TabIndex = 0;
            // 
            // P13_AccumulatedQty
            // 
            this.ClientSize = new System.Drawing.Size(784, 437);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "P13_AccumulatedQty";
            this.Text = "P13. Accumulated Qty";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAccumlatedQty)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnClose;
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Grid gridAccumlatedQty;
        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
    }
}
