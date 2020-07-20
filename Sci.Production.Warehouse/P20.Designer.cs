namespace Sci.Production.Warehouse
{
    partial class P20
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
            this.btnClose = new Sci.Win.UI.Button();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.panel3 = new Sci.Win.UI.Panel();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtColorID = new Sci.Win.UI.TextBox();
            this.txtRefNo = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txtSeq = new Sci.Production.Class.TxtSeq();
            this.checkQty = new Sci.Win.UI.CheckBox();
            this.labelSeq = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.bindingSource2 = new Sci.Win.UI.BindingSource(this.components);
            this.bindingSource3 = new Sci.Win.UI.BindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridStockList = new Sci.Win.UI.Grid();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.gridTransactionID = new Sci.Win.UI.Grid();
            this.gridRoll = new Sci.Win.UI.Grid();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridStockList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTransactionID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRoll)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 613);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1004, 48);
            this.panel2.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(912, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // bindingSource1
            // 
            this.bindingSource1.PositionChanged += new System.EventHandler(this.bindingSource1_PositionChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.comboFactory);
            this.panel3.Controls.Add(this.labelFactory);
            this.panel3.Controls.Add(this.txtColorID);
            this.panel3.Controls.Add(this.txtRefNo);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.txtSeq);
            this.panel3.Controls.Add(this.checkQty);
            this.panel3.Controls.Add(this.labelSeq);
            this.panel3.Controls.Add(this.txtSPNo);
            this.panel3.Controls.Add(this.labelSPNo);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1004, 41);
            this.panel3.TabIndex = 1;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(736, 8);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(83, 24);
            this.comboFactory.TabIndex = 113;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(672, 9);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(61, 23);
            this.labelFactory.TabIndex = 112;
            this.labelFactory.Text = "Factory";
            // 
            // txtColorID
            // 
            this.txtColorID.BackColor = System.Drawing.Color.White;
            this.txtColorID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtColorID.IsSupportEditMode = false;
            this.txtColorID.IsSupportSytsemContextMenu = false;
            this.txtColorID.Location = new System.Drawing.Point(611, 9);
            this.txtColorID.MaxLength = 13;
            this.txtColorID.Name = "txtColorID";
            this.txtColorID.Size = new System.Drawing.Size(58, 23);
            this.txtColorID.TabIndex = 3;
            // 
            // txtRefNo
            // 
            this.txtRefNo.BackColor = System.Drawing.Color.White;
            this.txtRefNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefNo.IsSupportEditMode = false;
            this.txtRefNo.IsSupportSytsemContextMenu = false;
            this.txtRefNo.Location = new System.Drawing.Point(434, 9);
            this.txtRefNo.MaxLength = 20;
            this.txtRefNo.Name = "txtRefNo";
            this.txtRefNo.Size = new System.Drawing.Size(112, 23);
            this.txtRefNo.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(549, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 23);
            this.label2.TabIndex = 19;
            this.label2.Text = "Color ID";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(353, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 18;
            this.label1.Text = "RefNo";
            // 
            // txtSeq
            // 
            this.txtSeq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq.Location = new System.Drawing.Point(284, 9);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Seq1 = "";
            this.txtSeq.Seq2 = "";
            this.txtSeq.Size = new System.Drawing.Size(61, 23);
            this.txtSeq.TabIndex = 1;
            // 
            // checkQty
            // 
            this.checkQty.AutoSize = true;
            this.checkQty.Checked = true;
            this.checkQty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkQty.IsSupportEditMode = false;
            this.checkQty.Location = new System.Drawing.Point(929, 10);
            this.checkQty.Name = "checkQty";
            this.checkQty.Size = new System.Drawing.Size(73, 21);
            this.checkQty.TabIndex = 5;
            this.checkQty.Text = "Qty > 0";
            this.checkQty.UseVisualStyleBackColor = true;
            this.checkQty.CheckedChanged += new System.EventHandler(this.checkQty_CheckedChanged);
            // 
            // labelSeq
            // 
            this.labelSeq.Location = new System.Drawing.Point(222, 9);
            this.labelSeq.Name = "labelSeq";
            this.labelSeq.Size = new System.Drawing.Size(59, 23);
            this.labelSeq.TabIndex = 17;
            this.labelSeq.Text = "Seq";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.IsSupportEditMode = false;
            this.txtSPNo.IsSupportSytsemContextMenu = false;
            this.txtSPNo.Location = new System.Drawing.Point(85, 9);
            this.txtSPNo.MaxLength = 13;
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(134, 23);
            this.txtSPNo.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(7, 9);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(75, 23);
            this.labelSPNo.TabIndex = 13;
            this.labelSPNo.Text = "SP#";
            // 
            // btnQuery
            // 
            this.btnQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnQuery.Location = new System.Drawing.Point(825, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 4;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 41);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridStockList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1004, 572);
            this.splitContainer1.SplitterDistance = 175;
            this.splitContainer1.TabIndex = 4;
            // 
            // gridStockList
            // 
            this.gridStockList.AllowUserToAddRows = false;
            this.gridStockList.AllowUserToDeleteRows = false;
            this.gridStockList.AllowUserToResizeRows = false;
            this.gridStockList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridStockList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridStockList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridStockList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridStockList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridStockList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridStockList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridStockList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridStockList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridStockList.Location = new System.Drawing.Point(0, 0);
            this.gridStockList.Name = "gridStockList";
            this.gridStockList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridStockList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridStockList.RowTemplate.Height = 24;
            this.gridStockList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridStockList.ShowCellToolTips = false;
            this.gridStockList.Size = new System.Drawing.Size(1004, 175);
            this.gridStockList.TabIndex = 1;
            this.gridStockList.TabStop = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.gridTransactionID);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gridRoll);
            this.splitContainer2.Size = new System.Drawing.Size(1004, 393);
            this.splitContainer2.SplitterDistance = 244;
            this.splitContainer2.TabIndex = 0;
            // 
            // gridTransactionID
            // 
            this.gridTransactionID.AllowUserToAddRows = false;
            this.gridTransactionID.AllowUserToDeleteRows = false;
            this.gridTransactionID.AllowUserToResizeRows = false;
            this.gridTransactionID.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridTransactionID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridTransactionID.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridTransactionID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTransactionID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTransactionID.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridTransactionID.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridTransactionID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridTransactionID.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridTransactionID.Location = new System.Drawing.Point(0, 0);
            this.gridTransactionID.Name = "gridTransactionID";
            this.gridTransactionID.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridTransactionID.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridTransactionID.RowTemplate.Height = 24;
            this.gridTransactionID.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTransactionID.ShowCellToolTips = false;
            this.gridTransactionID.Size = new System.Drawing.Size(1004, 244);
            this.gridTransactionID.TabIndex = 6;
            this.gridTransactionID.TabStop = false;
            // 
            // gridRoll
            // 
            this.gridRoll.AllowUserToAddRows = false;
            this.gridRoll.AllowUserToDeleteRows = false;
            this.gridRoll.AllowUserToResizeRows = false;
            this.gridRoll.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridRoll.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridRoll.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridRoll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRoll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRoll.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridRoll.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridRoll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridRoll.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridRoll.Location = new System.Drawing.Point(0, 0);
            this.gridRoll.Name = "gridRoll";
            this.gridRoll.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridRoll.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridRoll.RowTemplate.Height = 24;
            this.gridRoll.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRoll.ShowCellToolTips = false;
            this.gridRoll.Size = new System.Drawing.Size(1004, 145);
            this.gridRoll.TabIndex = 8;
            this.gridRoll.TabStop = false;
            // 
            // P20
            // 
            this.ClientSize = new System.Drawing.Size(1004, 661);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Name = "P20";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P20. Stock List";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
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
            ((System.ComponentModel.ISupportInitialize)(this.gridStockList)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTransactionID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRoll)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Panel panel3;
        private Win.UI.BindingSource bindingSource2;
        private Win.UI.BindingSource bindingSource3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Grid gridStockList;
        private Win.UI.Label labelSeq;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelSPNo;
        private Win.UI.Button btnQuery;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Win.UI.Grid gridTransactionID;
        private Win.UI.Grid gridRoll;
        private Win.UI.CheckBox checkQty;
        private Class.TxtSeq txtSeq;
        private Win.UI.TextBox txtColorID;
        private Win.UI.TextBox txtRefNo;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.Label labelFactory;
    }
}
