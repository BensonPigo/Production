namespace Sci.Production.Warehouse
{
    partial class P03_ReturnQty
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
            this.panel2 = new Sci.Win.UI.Panel();
            this.labelTotal = new Sci.Win.UI.Label();
            this.numTotal = new Sci.Win.UI.NumericBox();
            this.btnClose = new Sci.Win.UI.Button();
            this.gridReturn = new Sci.Win.UI.Grid();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridReturn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelTotal);
            this.panel2.Controls.Add(this.numTotal);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 392);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(924, 48);
            this.panel2.TabIndex = 1;
            // 
            // labelTotal
            // 
            this.labelTotal.Location = new System.Drawing.Point(651, 13);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(75, 23);
            this.labelTotal.TabIndex = 6;
            this.labelTotal.Text = "Total";
            // 
            // numTotal
            // 
            this.numTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotal.DecimalPlaces = 2;
            this.numTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotal.IsSupportEditMode = false;
            this.numTotal.Location = new System.Drawing.Point(729, 13);
            this.numTotal.Name = "numTotal";
            this.numTotal.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotal.ReadOnly = true;
            this.numTotal.Size = new System.Drawing.Size(100, 23);
            this.numTotal.TabIndex = 5;
            this.numTotal.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(832, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // gridReturn
            // 
            this.gridReturn.AllowUserToAddRows = false;
            this.gridReturn.AllowUserToDeleteRows = false;
            this.gridReturn.AllowUserToResizeRows = false;
            this.gridReturn.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridReturn.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridReturn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridReturn.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridReturn.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridReturn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridReturn.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridReturn.Location = new System.Drawing.Point(0, 0);
            this.gridReturn.Name = "gridReturn";
            this.gridReturn.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridReturn.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridReturn.RowTemplate.Height = 24;
            this.gridReturn.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridReturn.ShowCellToolTips = false;
            this.gridReturn.Size = new System.Drawing.Size(924, 392);
            this.gridReturn.TabIndex = 2;
            this.gridReturn.TabStop = false;
            // 
            // P03_ReturnQty
            // 
            this.ClientSize = new System.Drawing.Size(924, 440);
            this.Controls.Add(this.gridReturn);
            this.Controls.Add(this.panel2);
            this.Name = "P03_ReturnQty";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Return Transaction Detail";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridReturn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.Grid gridReturn;
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Label labelTotal;
        private Win.UI.NumericBox numTotal;
    }
}
