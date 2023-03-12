namespace Sci.Production.Shipping
{
    partial class P16_Separate_TK_Group
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
            this.gridCartonList = new Sci.Win.UI.Grid();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.gridPackingList = new Sci.Win.UI.Grid();
            this.label3 = new Sci.Win.UI.Label();
            this.gridGroupSummaryInfo = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnEditSave = new Sci.Win.UI.Button();
            this.bindingSourcePackingList = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.bindingSourceGroupSummary = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.bindingSourceCartonList = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnRequestSeparate = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridCartonList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPackingList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridGroupSummaryInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePackingList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceGroupSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCartonList)).BeginInit();
            this.SuspendLayout();
            // 
            // gridCartonList
            // 
            this.gridCartonList.AllowUserToAddRows = false;
            this.gridCartonList.AllowUserToDeleteRows = false;
            this.gridCartonList.AllowUserToResizeRows = false;
            this.gridCartonList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCartonList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCartonList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCartonList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCartonList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCartonList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCartonList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCartonList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCartonList.Location = new System.Drawing.Point(12, 39);
            this.gridCartonList.Name = "gridCartonList";
            this.gridCartonList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCartonList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCartonList.RowTemplate.Height = 24;
            this.gridCartonList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCartonList.ShowCellToolTips = false;
            this.gridCartonList.Size = new System.Drawing.Size(655, 255);
            this.gridCartonList.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridCartonList.TabIndex = 1;
            this.gridCartonList.SelectionChanged += new System.EventHandler(this.GridCartonList_SelectionChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Carton List";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 317);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Packing List";
            // 
            // gridPackingList
            // 
            this.gridPackingList.AllowUserToAddRows = false;
            this.gridPackingList.AllowUserToDeleteRows = false;
            this.gridPackingList.AllowUserToResizeRows = false;
            this.gridPackingList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridPackingList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridPackingList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridPackingList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPackingList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridPackingList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridPackingList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridPackingList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridPackingList.Location = new System.Drawing.Point(12, 343);
            this.gridPackingList.Name = "gridPackingList";
            this.gridPackingList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridPackingList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridPackingList.RowTemplate.Height = 24;
            this.gridPackingList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPackingList.ShowCellToolTips = false;
            this.gridPackingList.Size = new System.Drawing.Size(655, 336);
            this.gridPackingList.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridPackingList.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(673, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Group Summary Info";
            // 
            // gridGroupSummaryInfo
            // 
            this.gridGroupSummaryInfo.AllowUserToAddRows = false;
            this.gridGroupSummaryInfo.AllowUserToDeleteRows = false;
            this.gridGroupSummaryInfo.AllowUserToResizeRows = false;
            this.gridGroupSummaryInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridGroupSummaryInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridGroupSummaryInfo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridGroupSummaryInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridGroupSummaryInfo.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridGroupSummaryInfo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridGroupSummaryInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridGroupSummaryInfo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridGroupSummaryInfo.Location = new System.Drawing.Point(673, 39);
            this.gridGroupSummaryInfo.Name = "gridGroupSummaryInfo";
            this.gridGroupSummaryInfo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridGroupSummaryInfo.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridGroupSummaryInfo.RowTemplate.Height = 24;
            this.gridGroupSummaryInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridGroupSummaryInfo.ShowCellToolTips = false;
            this.gridGroupSummaryInfo.Size = new System.Drawing.Size(323, 640);
            this.gridGroupSummaryInfo.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridGroupSummaryInfo.TabIndex = 6;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(916, 687);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnEditSave
            // 
            this.btnEditSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditSave.Location = new System.Drawing.Point(830, 687);
            this.btnEditSave.Name = "btnEditSave";
            this.btnEditSave.Size = new System.Drawing.Size(80, 30);
            this.btnEditSave.TabIndex = 8;
            this.btnEditSave.Text = "Edit";
            this.btnEditSave.UseVisualStyleBackColor = true;
            this.btnEditSave.Click += new System.EventHandler(this.BtnEditSave_Click);
            // 
            // btnRequestSeparate
            // 
            this.btnRequestSeparate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRequestSeparate.Location = new System.Drawing.Point(663, 687);
            this.btnRequestSeparate.Name = "btnRequestSeparate";
            this.btnRequestSeparate.Size = new System.Drawing.Size(161, 30);
            this.btnRequestSeparate.TabIndex = 9;
            this.btnRequestSeparate.Text = "Request Separate";
            this.btnRequestSeparate.UseVisualStyleBackColor = true;
            this.btnRequestSeparate.Click += new System.EventHandler(this.BtnRequestSeparate_Click);
            // 
            // P16_Separate_TK_Group
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.btnRequestSeparate);
            this.Controls.Add(this.btnEditSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridGroupSummaryInfo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.gridPackingList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridCartonList);
            this.Name = "P16_Separate_TK_Group";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Separate TK Group";
            this.Controls.SetChildIndex(this.gridCartonList, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.gridPackingList, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.gridGroupSummaryInfo, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnEditSave, 0);
            this.Controls.SetChildIndex(this.btnRequestSeparate, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridCartonList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPackingList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridGroupSummaryInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePackingList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceGroupSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCartonList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridCartonList;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Grid gridPackingList;
        private Win.UI.Label label3;
        private Win.UI.Grid gridGroupSummaryInfo;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnEditSave;
        private Win.UI.ListControlBindingSource bindingSourcePackingList;
        private Win.UI.ListControlBindingSource bindingSourceGroupSummary;
        private Win.UI.ListControlBindingSource bindingSourceCartonList;
        private Win.UI.Button btnRequestSeparate;
    }
}