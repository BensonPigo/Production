namespace Sci.Production.Warehouse
{
    partial class P19_TransferWKImport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.txtTransferExportID = new Sci.Win.UI.TextBox();
            this.comboFabricType = new Sci.Win.UI.ComboBox();
            this.splitGrid = new System.Windows.Forms.SplitContainer();
            this.gridExport = new Sci.Win.UI.Grid();
            this.gridStock = new Sci.Win.UI.Grid();
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.bindingGridStock = new Sci.Win.UI.BindingSource(this.components);
            this.label3 = new Sci.Win.UI.Label();
            this.displayTotal = new Sci.Win.UI.DisplayBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitGrid)).BeginInit();
            this.splitGrid.Panel1.SuspendLayout();
            this.splitGrid.Panel2.SuspendLayout();
            this.splitGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingGridStock)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Transfer WK#";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(279, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Type Filter";
            // 
            // txtTransferExportID
            // 
            this.txtTransferExportID.BackColor = System.Drawing.Color.White;
            this.txtTransferExportID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTransferExportID.Location = new System.Drawing.Point(105, 9);
            this.txtTransferExportID.Name = "txtTransferExportID";
            this.txtTransferExportID.Size = new System.Drawing.Size(143, 23);
            this.txtTransferExportID.TabIndex = 2;
            // 
            // comboFabricType
            // 
            this.comboFabricType.BackColor = System.Drawing.Color.White;
            this.comboFabricType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFabricType.FormattingEnabled = true;
            this.comboFabricType.IsSupportUnselect = true;
            this.comboFabricType.Location = new System.Drawing.Point(357, 9);
            this.comboFabricType.Name = "comboFabricType";
            this.comboFabricType.OldText = "";
            this.comboFabricType.Size = new System.Drawing.Size(121, 24);
            this.comboFabricType.TabIndex = 3;
            // 
            // splitGrid
            // 
            this.splitGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitGrid.Location = new System.Drawing.Point(9, 38);
            this.splitGrid.Name = "splitGrid";
            // 
            // splitGrid.Panel1
            // 
            this.splitGrid.Panel1.Controls.Add(this.gridExport);
            // 
            // splitGrid.Panel2
            // 
            this.splitGrid.Panel2.Controls.Add(this.gridStock);
            this.splitGrid.Size = new System.Drawing.Size(1044, 456);
            this.splitGrid.SplitterDistance = 511;
            this.splitGrid.TabIndex = 4;
            // 
            // gridExport
            // 
            this.gridExport.AllowUserToAddRows = false;
            this.gridExport.AllowUserToDeleteRows = false;
            this.gridExport.AllowUserToResizeRows = false;
            this.gridExport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridExport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridExport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridExport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridExport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridExport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridExport.Location = new System.Drawing.Point(0, 0);
            this.gridExport.Name = "gridExport";
            this.gridExport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridExport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridExport.RowTemplate.Height = 24;
            this.gridExport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridExport.ShowCellToolTips = false;
            this.gridExport.Size = new System.Drawing.Size(511, 456);
            this.gridExport.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridExport.TabIndex = 0;
            this.gridExport.SelectionChanged += new System.EventHandler(this.GridExport_SelectionChanged);
            // 
            // gridStock
            // 
            this.gridStock.AllowUserToAddRows = false;
            this.gridStock.AllowUserToDeleteRows = false;
            this.gridStock.AllowUserToResizeRows = false;
            this.gridStock.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridStock.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridStock.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridStock.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridStock.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridStock.Location = new System.Drawing.Point(0, 0);
            this.gridStock.Name = "gridStock";
            this.gridStock.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridStock.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridStock.RowTemplate.Height = 24;
            this.gridStock.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridStock.ShowCellToolTips = false;
            this.gridStock.Size = new System.Drawing.Size(529, 456);
            this.gridStock.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridStock.TabIndex = 0;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(973, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 5;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(890, 503);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(976, 503);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.Location = new System.Drawing.Point(264, 510);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "Total Export Qty ";
            // 
            // displayTotal
            // 
            this.displayTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.displayTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTotal.Location = new System.Drawing.Point(375, 510);
            this.displayTotal.Name = "displayTotal";
            this.displayTotal.Size = new System.Drawing.Size(103, 23);
            this.displayTotal.TabIndex = 9;
            // 
            // P19_TransferWKImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1065, 546);
            this.Controls.Add(this.displayTotal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.splitGrid);
            this.Controls.Add(this.comboFabricType);
            this.Controls.Add(this.txtTransferExportID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "P19_TransferWKImport";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P19. Transfer WK# Import";
            this.splitGrid.Panel1.ResumeLayout(false);
            this.splitGrid.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitGrid)).EndInit();
            this.splitGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingGridStock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtTransferExportID;
        private Win.UI.ComboBox comboFabricType;
        private System.Windows.Forms.SplitContainer splitGrid;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnClose;
        private Win.UI.Grid gridExport;
        private Win.UI.Grid gridStock;
        private Win.UI.BindingSource bindingGridStock;
        private Win.UI.Label label3;
        private Win.UI.DisplayBox displayTotal;
    }
}