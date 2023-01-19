namespace Sci.Production.Warehouse
{
    partial class P19_TKSeparateHistory
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
            this.gridTransferWK = new Sci.Win.UI.Grid();
            this.panel2 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.comboTK = new Sci.Win.UI.ComboBox();
            this.lblTransferWK = new System.Windows.Forms.Label();
            this.lblNewTK = new Sci.Win.UI.Label();
            this.lblPackingList = new System.Windows.Forms.Label();
            this.gridPacking = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridTransferWK)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPacking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridTransferWK
            // 
            this.gridTransferWK.AllowUserToAddRows = false;
            this.gridTransferWK.AllowUserToDeleteRows = false;
            this.gridTransferWK.AllowUserToResizeRows = false;
            this.gridTransferWK.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridTransferWK.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridTransferWK.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridTransferWK.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTransferWK.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridTransferWK.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridTransferWK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridTransferWK.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridTransferWK.Location = new System.Drawing.Point(3, 57);
            this.gridTransferWK.Name = "gridTransferWK";
            this.gridTransferWK.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridTransferWK.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridTransferWK.RowTemplate.Height = 24;
            this.gridTransferWK.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTransferWK.ShowCellToolTips = false;
            this.gridTransferWK.Size = new System.Drawing.Size(1016, 264);
            this.gridTransferWK.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridTransferWK.TabIndex = 0;
            this.gridTransferWK.TabStop = false;
            this.gridTransferWK.SelectionChanged += new System.EventHandler(this.GridTransferWK_SelectionChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 609);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1016, 48);
            this.panel2.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(924, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.comboTK);
            this.splitContainer1.Panel1.Controls.Add(this.lblTransferWK);
            this.splitContainer1.Panel1.Controls.Add(this.lblNewTK);
            this.splitContainer1.Panel1.Controls.Add(this.gridTransferWK);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblPackingList);
            this.splitContainer1.Panel2.Controls.Add(this.gridPacking);
            this.splitContainer1.Size = new System.Drawing.Size(1016, 657);
            this.splitContainer1.SplitterDistance = 324;
            this.splitContainer1.TabIndex = 3;
            // 
            // comboTK
            // 
            this.comboTK.BackColor = System.Drawing.Color.White;
            this.comboTK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboTK.FormattingEnabled = true;
            this.comboTK.IsSupportUnselect = true;
            this.comboTK.Location = new System.Drawing.Point(106, 9);
            this.comboTK.Name = "comboTK";
            this.comboTK.OldText = "";
            this.comboTK.Size = new System.Drawing.Size(121, 24);
            this.comboTK.TabIndex = 3;
            this.comboTK.SelectedValueChanged += new System.EventHandler(this.ComboTK_SelectedValueChanged);
            // 
            // lblTransferWK
            // 
            this.lblTransferWK.AutoSize = true;
            this.lblTransferWK.Location = new System.Drawing.Point(6, 37);
            this.lblTransferWK.Name = "lblTransferWK";
            this.lblTransferWK.Size = new System.Drawing.Size(128, 17);
            this.lblTransferWK.TabIndex = 2;
            this.lblTransferWK.Text = "Transfer WK List：";
            // 
            // lblNewTK
            // 
            this.lblNewTK.Location = new System.Drawing.Point(9, 9);
            this.lblNewTK.Name = "lblNewTK";
            this.lblNewTK.Size = new System.Drawing.Size(94, 23);
            this.lblNewTK.TabIndex = 1;
            this.lblNewTK.Text = "New TK Filter";
            // 
            // lblPackingList
            // 
            this.lblPackingList.AutoSize = true;
            this.lblPackingList.Location = new System.Drawing.Point(5, 4);
            this.lblPackingList.Name = "lblPackingList";
            this.lblPackingList.Size = new System.Drawing.Size(98, 17);
            this.lblPackingList.TabIndex = 1;
            this.lblPackingList.Text = "Packing List：";
            // 
            // gridPacking
            // 
            this.gridPacking.AllowUserToAddRows = false;
            this.gridPacking.AllowUserToDeleteRows = false;
            this.gridPacking.AllowUserToResizeRows = false;
            this.gridPacking.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridPacking.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridPacking.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridPacking.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPacking.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridPacking.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridPacking.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridPacking.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridPacking.Location = new System.Drawing.Point(3, 24);
            this.gridPacking.Name = "gridPacking";
            this.gridPacking.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridPacking.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridPacking.RowTemplate.Height = 24;
            this.gridPacking.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPacking.ShowCellToolTips = false;
            this.gridPacking.Size = new System.Drawing.Size(1010, 251);
            this.gridPacking.TabIndex = 0;
            this.gridPacking.TabStop = false;
            // 
            // P19_TKSeparateHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 657);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitContainer1);
            this.Name = "P19_TKSeparateHistory";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P19_TKSeparateHistory";
            ((System.ComponentModel.ISupportInitialize)(this.gridTransferWK)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridPacking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridTransferWK;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.BindingSource bindingSource1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Grid gridPacking;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private System.Windows.Forms.Label lblPackingList;
        private Win.UI.ComboBox comboTK;
        private System.Windows.Forms.Label lblTransferWK;
        private Win.UI.Label lblNewTK;
    }
}