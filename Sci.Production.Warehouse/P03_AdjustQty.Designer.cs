namespace Sci.Production.Warehouse
{
    partial class P03_AdjustQty
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
            this.gridAdjust = new Sci.Win.UI.Grid();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridAdjust)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelTotal);
            this.panel2.Controls.Add(this.numTotal);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 380);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(939, 48);
            this.panel2.TabIndex = 2;
            // 
            // labelTotal
            // 
            this.labelTotal.Location = new System.Drawing.Point(657, 14);
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
            this.numTotal.Location = new System.Drawing.Point(735, 14);
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
            this.btnClose.Location = new System.Drawing.Point(847, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // gridAdjust
            // 
            this.gridAdjust.AllowUserToAddRows = false;
            this.gridAdjust.AllowUserToDeleteRows = false;
            this.gridAdjust.AllowUserToResizeRows = false;
            this.gridAdjust.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridAdjust.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridAdjust.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAdjust.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridAdjust.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridAdjust.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridAdjust.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridAdjust.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridAdjust.Location = new System.Drawing.Point(0, 0);
            this.gridAdjust.Name = "gridAdjust";
            this.gridAdjust.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridAdjust.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridAdjust.RowTemplate.Height = 24;
            this.gridAdjust.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAdjust.ShowCellToolTips = false;
            this.gridAdjust.Size = new System.Drawing.Size(939, 380);
            this.gridAdjust.TabIndex = 3;
            this.gridAdjust.TabStop = false;
            // 
            // P03_AdjustQty
            // 
            this.ClientSize = new System.Drawing.Size(939, 428);
            this.Controls.Add(this.gridAdjust);
            this.Controls.Add(this.panel2);
            this.Name = "P03_AdjustQty";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Adjust Transaction Detail";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridAdjust)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel2;
        private Win.UI.Label labelTotal;
        private Win.UI.NumericBox numTotal;
        private Win.UI.Button btnClose;
        private Win.UI.Grid gridAdjust;
        private Win.UI.BindingSource bindingSource1;
    }
}
