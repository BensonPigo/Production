namespace Sci.Production.Warehouse
{
    partial class P03_RollTransaction
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
            this.panel2 = new Sci.Win.UI.Panel();
            this.btnPrint = new Sci.Win.UI.Button();
            this.labelStockType = new Sci.Win.UI.Label();
            this.comboStockType = new Sci.Win.UI.ComboBox();
            this.btnClose = new Sci.Win.UI.Button();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.panel3 = new Sci.Win.UI.Panel();
            this.numBalQtyBySeq = new Sci.Win.UI.NumericBox();
            this.numReleasedQtyBySeq = new Sci.Win.UI.NumericBox();
            this.numArrivedQtyBySeq = new Sci.Win.UI.NumericBox();
            this.labelBalQtyBySeq = new Sci.Win.UI.Label();
            this.labelReleasedQtyBySeq = new Sci.Win.UI.Label();
            this.labelArrivedQtyBySeq = new Sci.Win.UI.Label();
            this.displayDescription = new Sci.Win.UI.DisplayBox();
            this.labelDescription = new Sci.Win.UI.Label();
            this.displaySeqNo = new Sci.Win.UI.DisplayBox();
            this.labelSeqNo = new Sci.Win.UI.Label();
            this.bindingSource2 = new Sci.Win.UI.BindingSource(this.components);
            this.bindingSource3 = new Sci.Win.UI.BindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridFtyinventory = new Sci.Win.UI.Grid();
            this.gridTrans = new Sci.Win.UI.Grid();
            this.gridSummary = new Sci.Win.UI.Grid();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFtyinventory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTrans)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSummary)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnPrint);
            this.panel2.Controls.Add(this.labelStockType);
            this.panel2.Controls.Add(this.comboStockType);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 513);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 48);
            this.panel2.TabIndex = 0;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(816, 11);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(80, 30);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // labelStockType
            // 
            this.labelStockType.Location = new System.Drawing.Point(9, 11);
            this.labelStockType.Name = "labelStockType";
            this.labelStockType.Size = new System.Drawing.Size(75, 23);
            this.labelStockType.TabIndex = 4;
            this.labelStockType.Text = "Stock Type";
            // 
            // comboStockType
            // 
            this.comboStockType.BackColor = System.Drawing.Color.White;
            this.comboStockType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStockType.FormattingEnabled = true;
            this.comboStockType.IsSupportUnselect = true;
            this.comboStockType.Items.AddRange(new object[] {
            "ALL",
            "Bulk",
            "Inventory"});
            this.comboStockType.Location = new System.Drawing.Point(87, 11);
            this.comboStockType.Name = "comboStockType";
            this.comboStockType.OldText = "";
            this.comboStockType.Size = new System.Drawing.Size(100, 24);
            this.comboStockType.TabIndex = 3;
            this.comboStockType.SelectedIndexChanged += new System.EventHandler(this.ComboStockType_SelectedIndexChanged);
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
            // bindingSource1
            // 
            this.bindingSource1.PositionChanged += new System.EventHandler(this.BindingSource1_PositionChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.numBalQtyBySeq);
            this.panel3.Controls.Add(this.numReleasedQtyBySeq);
            this.panel3.Controls.Add(this.numArrivedQtyBySeq);
            this.panel3.Controls.Add(this.labelBalQtyBySeq);
            this.panel3.Controls.Add(this.labelReleasedQtyBySeq);
            this.panel3.Controls.Add(this.labelArrivedQtyBySeq);
            this.panel3.Controls.Add(this.displayDescription);
            this.panel3.Controls.Add(this.labelDescription);
            this.panel3.Controls.Add(this.displaySeqNo);
            this.panel3.Controls.Add(this.labelSeqNo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1008, 73);
            this.panel3.TabIndex = 1;
            // 
            // numBalQtyBySeq
            // 
            this.numBalQtyBySeq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numBalQtyBySeq.DecimalPlaces = 2;
            this.numBalQtyBySeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numBalQtyBySeq.IsSupportEditMode = false;
            this.numBalQtyBySeq.Location = new System.Drawing.Point(672, 39);
            this.numBalQtyBySeq.Name = "numBalQtyBySeq";
            this.numBalQtyBySeq.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBalQtyBySeq.ReadOnly = true;
            this.numBalQtyBySeq.Size = new System.Drawing.Size(101, 23);
            this.numBalQtyBySeq.TabIndex = 11;
            this.numBalQtyBySeq.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numReleasedQtyBySeq
            // 
            this.numReleasedQtyBySeq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numReleasedQtyBySeq.DecimalPlaces = 2;
            this.numReleasedQtyBySeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numReleasedQtyBySeq.IsSupportEditMode = false;
            this.numReleasedQtyBySeq.Location = new System.Drawing.Point(419, 39);
            this.numReleasedQtyBySeq.Name = "numReleasedQtyBySeq";
            this.numReleasedQtyBySeq.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numReleasedQtyBySeq.ReadOnly = true;
            this.numReleasedQtyBySeq.Size = new System.Drawing.Size(101, 23);
            this.numReleasedQtyBySeq.TabIndex = 10;
            this.numReleasedQtyBySeq.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numArrivedQtyBySeq
            // 
            this.numArrivedQtyBySeq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numArrivedQtyBySeq.DecimalPlaces = 2;
            this.numArrivedQtyBySeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numArrivedQtyBySeq.IsSupportEditMode = false;
            this.numArrivedQtyBySeq.Location = new System.Drawing.Point(141, 39);
            this.numArrivedQtyBySeq.Name = "numArrivedQtyBySeq";
            this.numArrivedQtyBySeq.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numArrivedQtyBySeq.ReadOnly = true;
            this.numArrivedQtyBySeq.Size = new System.Drawing.Size(101, 23);
            this.numArrivedQtyBySeq.TabIndex = 9;
            this.numArrivedQtyBySeq.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelBalQtyBySeq
            // 
            this.labelBalQtyBySeq.Location = new System.Drawing.Point(556, 39);
            this.labelBalQtyBySeq.Name = "labelBalQtyBySeq";
            this.labelBalQtyBySeq.Size = new System.Drawing.Size(113, 23);
            this.labelBalQtyBySeq.TabIndex = 8;
            this.labelBalQtyBySeq.Text = "Bal. Qty By Seq";
            // 
            // labelReleasedQtyBySeq
            // 
            this.labelReleasedQtyBySeq.Location = new System.Drawing.Point(276, 39);
            this.labelReleasedQtyBySeq.Name = "labelReleasedQtyBySeq";
            this.labelReleasedQtyBySeq.Size = new System.Drawing.Size(140, 23);
            this.labelReleasedQtyBySeq.TabIndex = 6;
            this.labelReleasedQtyBySeq.Text = "Released Qty By Seq";
            // 
            // labelArrivedQtyBySeq
            // 
            this.labelArrivedQtyBySeq.Location = new System.Drawing.Point(9, 39);
            this.labelArrivedQtyBySeq.Name = "labelArrivedQtyBySeq";
            this.labelArrivedQtyBySeq.Size = new System.Drawing.Size(129, 23);
            this.labelArrivedQtyBySeq.TabIndex = 4;
            this.labelArrivedQtyBySeq.Text = "Arrived Qty By Seq";
            // 
            // displayDescription
            // 
            this.displayDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDescription.Location = new System.Drawing.Point(297, 9);
            this.displayDescription.Name = "displayDescription";
            this.displayDescription.Size = new System.Drawing.Size(699, 23);
            this.displayDescription.TabIndex = 3;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(199, 9);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(95, 23);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = "Description";
            // 
            // displaySeqNo
            // 
            this.displaySeqNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeqNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeqNo.Location = new System.Drawing.Point(87, 9);
            this.displaySeqNo.Name = "displaySeqNo";
            this.displaySeqNo.Size = new System.Drawing.Size(81, 23);
            this.displaySeqNo.TabIndex = 1;
            // 
            // labelSeqNo
            // 
            this.labelSeqNo.Location = new System.Drawing.Point(9, 9);
            this.labelSeqNo.Name = "labelSeqNo";
            this.labelSeqNo.Size = new System.Drawing.Size(75, 23);
            this.labelSeqNo.TabIndex = 0;
            this.labelSeqNo.Text = "Seq#";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 73);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridFtyinventory);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridTrans);
            this.splitContainer1.Panel2.Controls.Add(this.gridSummary);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 440);
            this.splitContainer1.SplitterDistance = 430;
            this.splitContainer1.TabIndex = 4;
            // 
            // gridFtyinventory
            // 
            this.gridFtyinventory.AllowUserToAddRows = false;
            this.gridFtyinventory.AllowUserToDeleteRows = false;
            this.gridFtyinventory.AllowUserToResizeRows = false;
            this.gridFtyinventory.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridFtyinventory.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridFtyinventory.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridFtyinventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFtyinventory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFtyinventory.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridFtyinventory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridFtyinventory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridFtyinventory.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridFtyinventory.Location = new System.Drawing.Point(0, 0);
            this.gridFtyinventory.Name = "gridFtyinventory";
            this.gridFtyinventory.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridFtyinventory.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridFtyinventory.RowTemplate.Height = 24;
            this.gridFtyinventory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFtyinventory.ShowCellToolTips = false;
            this.gridFtyinventory.Size = new System.Drawing.Size(430, 440);
            this.gridFtyinventory.TabIndex = 1;
            this.gridFtyinventory.TabStop = false;
            this.gridFtyinventory.SelectionChanged += new System.EventHandler(this.GridFtyinventory_SelectionChanged);
            // 
            // gridTrans
            // 
            this.gridTrans.AllowUserToAddRows = false;
            this.gridTrans.AllowUserToDeleteRows = false;
            this.gridTrans.AllowUserToResizeRows = false;
            this.gridTrans.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridTrans.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridTrans.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridTrans.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridTrans.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTrans.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridTrans.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridTrans.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridTrans.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridTrans.Location = new System.Drawing.Point(3, 3);
            this.gridTrans.Name = "gridTrans";
            this.gridTrans.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridTrans.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridTrans.RowTemplate.Height = 24;
            this.gridTrans.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTrans.ShowCellToolTips = false;
            this.gridTrans.Size = new System.Drawing.Size(568, 245);
            this.gridTrans.TabIndex = 3;
            this.gridTrans.TabStop = false;
            // 
            // gridSummary
            // 
            this.gridSummary.AllowUserToAddRows = false;
            this.gridSummary.AllowUserToDeleteRows = false;
            this.gridSummary.AllowUserToResizeRows = false;
            this.gridSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSummary.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSummary.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridSummary.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSummary.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSummary.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSummary.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSummary.Location = new System.Drawing.Point(3, 254);
            this.gridSummary.Name = "gridSummary";
            this.gridSummary.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSummary.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSummary.RowTemplate.Height = 24;
            this.gridSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSummary.ShowCellToolTips = false;
            this.gridSummary.Size = new System.Drawing.Size(568, 183);
            this.gridSummary.TabIndex = 5;
            this.gridSummary.TabStop = false;
            // 
            // P03_RollTransaction
            // 
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Name = "P03_RollTransaction";
            this.Text = "Transaction Detail by Roll# Dyelot";
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource3)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFtyinventory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTrans)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSummary)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Panel panel3;
        private Win.UI.Label labelBalQtyBySeq;
        private Win.UI.Label labelReleasedQtyBySeq;
        private Win.UI.Label labelArrivedQtyBySeq;
        private Win.UI.DisplayBox displayDescription;
        private Win.UI.Label labelDescription;
        private Win.UI.DisplayBox displaySeqNo;
        private Win.UI.Label labelSeqNo;
        private Win.UI.Label labelStockType;
        private Win.UI.ComboBox comboStockType;
        private Win.UI.BindingSource bindingSource2;
        private Win.UI.BindingSource bindingSource3;
        private Win.UI.NumericBox numBalQtyBySeq;
        private Win.UI.NumericBox numReleasedQtyBySeq;
        private Win.UI.NumericBox numArrivedQtyBySeq;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Grid gridFtyinventory;
        private Win.UI.Grid gridTrans;
        private Win.UI.Grid gridSummary;
        private Win.UI.Button btnPrint;
    }
}
