namespace Sci.Production.Subcon
{
    partial class P01_Purchase
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
            this.bindingSource2 = new Sci.Win.UI.BindingSource(this.components);
            this.bindingSource3 = new Sci.Win.UI.BindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridSPList = new Sci.Win.UI.Grid();
            this.gridQuantity = new Sci.Win.UI.Grid();
            this.gridAccumulated = new Sci.Win.UI.Grid();
            this.lbSP = new System.Windows.Forms.Label();
            this.lbQuantity = new System.Windows.Forms.Label();
            this.lbAccumulated = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSPList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAccumulated)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 513);
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
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lbSP);
            this.splitContainer1.Panel1.Controls.Add(this.gridSPList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lbAccumulated);
            this.splitContainer1.Panel2.Controls.Add(this.lbQuantity);
            this.splitContainer1.Panel2.Controls.Add(this.gridQuantity);
            this.splitContainer1.Panel2.Controls.Add(this.gridAccumulated);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 513);
            this.splitContainer1.SplitterDistance = 430;
            this.splitContainer1.TabIndex = 4;
            // 
            // gridSPList
            // 
            this.gridSPList.AllowUserToAddRows = false;
            this.gridSPList.AllowUserToDeleteRows = false;
            this.gridSPList.AllowUserToResizeRows = false;
            this.gridSPList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSPList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSPList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridSPList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSPList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSPList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSPList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSPList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSPList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSPList.Location = new System.Drawing.Point(0, 24);
            this.gridSPList.Name = "gridSPList";
            this.gridSPList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSPList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSPList.RowTemplate.Height = 24;
            this.gridSPList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSPList.ShowCellToolTips = false;
            this.gridSPList.Size = new System.Drawing.Size(431, 489);
            this.gridSPList.TabIndex = 1;
            this.gridSPList.TabStop = false;
            // 
            // gridQuantity
            // 
            this.gridQuantity.AllowUserToAddRows = false;
            this.gridQuantity.AllowUserToDeleteRows = false;
            this.gridQuantity.AllowUserToResizeRows = false;
            this.gridQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridQuantity.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridQuantity.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridQuantity.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridQuantity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridQuantity.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridQuantity.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridQuantity.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridQuantity.Location = new System.Drawing.Point(3, 24);
            this.gridQuantity.Name = "gridQuantity";
            this.gridQuantity.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridQuantity.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridQuantity.RowTemplate.Height = 24;
            this.gridQuantity.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridQuantity.ShowCellToolTips = false;
            this.gridQuantity.Size = new System.Drawing.Size(568, 224);
            this.gridQuantity.TabIndex = 3;
            this.gridQuantity.TabStop = false;
            // 
            // gridAccumulated
            // 
            this.gridAccumulated.AllowUserToAddRows = false;
            this.gridAccumulated.AllowUserToDeleteRows = false;
            this.gridAccumulated.AllowUserToResizeRows = false;
            this.gridAccumulated.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridAccumulated.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridAccumulated.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridAccumulated.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridAccumulated.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAccumulated.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridAccumulated.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridAccumulated.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridAccumulated.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridAccumulated.Location = new System.Drawing.Point(3, 276);
            this.gridAccumulated.Name = "gridAccumulated";
            this.gridAccumulated.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridAccumulated.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridAccumulated.RowTemplate.Height = 24;
            this.gridAccumulated.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAccumulated.ShowCellToolTips = false;
            this.gridAccumulated.Size = new System.Drawing.Size(568, 234);
            this.gridAccumulated.TabIndex = 5;
            this.gridAccumulated.TabStop = false;
            // 
            // lbSP
            // 
            this.lbSP.AutoSize = true;
            this.lbSP.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lbSP.Location = new System.Drawing.Point(3, 4);
            this.lbSP.Name = "lbSP";
            this.lbSP.Size = new System.Drawing.Size(60, 17);
            this.lbSP.TabIndex = 6;
            this.lbSP.Text = "SP# List";
            // 
            // lbQuantity
            // 
            this.lbQuantity.AutoSize = true;
            this.lbQuantity.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lbQuantity.Location = new System.Drawing.Point(3, 4);
            this.lbQuantity.Name = "lbQuantity";
            this.lbQuantity.Size = new System.Drawing.Size(135, 17);
            this.lbQuantity.TabIndex = 7;
            this.lbQuantity.Text = "Quantity Breakdown";
            // 
            // lbAccumulated
            // 
            this.lbAccumulated.AutoSize = true;
            this.lbAccumulated.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lbAccumulated.Location = new System.Drawing.Point(3, 256);
            this.lbAccumulated.Name = "lbAccumulated";
            this.lbAccumulated.Size = new System.Drawing.Size(139, 17);
            this.lbAccumulated.TabIndex = 8;
            this.lbAccumulated.Text = "Accumulated PO List";
            // 
            // P01_Purchase
            // 
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Name = "P01_Purchase";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P01. Purchase Order QTY Breakdown";
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource3)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSPList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAccumulated)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.BindingSource bindingSource2;
        private Win.UI.BindingSource bindingSource3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Grid gridSPList;
        private Win.UI.Grid gridQuantity;
        private Win.UI.Grid gridAccumulated;
        private System.Windows.Forms.Label lbSP;
        private System.Windows.Forms.Label lbAccumulated;
        private System.Windows.Forms.Label lbQuantity;
    }
}
