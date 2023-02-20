namespace Sci.Production.PublicForm
{
    partial class P19_SeparateHistory
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
            this.label8 = new Sci.Win.UI.Label();
            this.comboNewID = new Sci.Win.UI.ComboBox();
            this.label9 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.gridTransferWKList = new Sci.Win.UI.Grid();
            this.gridPackingList = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridTransferWKList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPackingList)).BeginInit();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(9, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(140, 23);
            this.label8.TabIndex = 8;
            this.label8.Text = "New TK Filter";
            // 
            // comboNewID
            // 
            this.comboNewID.BackColor = System.Drawing.Color.White;
            this.comboNewID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboNewID.FormattingEnabled = true;
            this.comboNewID.IsSupportUnselect = true;
            this.comboNewID.Location = new System.Drawing.Point(152, 8);
            this.comboNewID.Name = "comboNewID";
            this.comboNewID.OldText = "";
            this.comboNewID.Size = new System.Drawing.Size(121, 24);
            this.comboNewID.TabIndex = 10;
            this.comboNewID.SelectedIndexChanged += new System.EventHandler(this.ComboNewID_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(9, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(125, 23);
            this.label9.TabIndex = 15;
            this.label9.Text = "Transfer WK List";
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(9, 320);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(125, 23);
            this.label10.TabIndex = 16;
            this.label10.Text = "Packing List";
            this.label10.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // gridTransferWKList
            // 
            this.gridTransferWKList.AllowUserToAddRows = false;
            this.gridTransferWKList.AllowUserToDeleteRows = false;
            this.gridTransferWKList.AllowUserToResizeRows = false;
            this.gridTransferWKList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridTransferWKList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridTransferWKList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridTransferWKList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTransferWKList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridTransferWKList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridTransferWKList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridTransferWKList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridTransferWKList.Location = new System.Drawing.Point(9, 68);
            this.gridTransferWKList.Name = "gridTransferWKList";
            this.gridTransferWKList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridTransferWKList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridTransferWKList.RowTemplate.Height = 24;
            this.gridTransferWKList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTransferWKList.ShowCellToolTips = false;
            this.gridTransferWKList.Size = new System.Drawing.Size(1088, 236);
            this.gridTransferWKList.TabIndex = 17;
            this.gridTransferWKList.SelectionChanged += new System.EventHandler(this.GridTransferWKList_SelectionChanged);
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
            this.gridPackingList.Location = new System.Drawing.Point(9, 346);
            this.gridPackingList.Name = "gridPackingList";
            this.gridPackingList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridPackingList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridPackingList.RowTemplate.Height = 24;
            this.gridPackingList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPackingList.ShowCellToolTips = false;
            this.gridPackingList.Size = new System.Drawing.Size(1088, 209);
            this.gridPackingList.TabIndex = 18;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1002, 561);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(92, 30);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P19_SeparateHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 603);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridPackingList);
            this.Controls.Add(this.gridTransferWKList);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboNewID);
            this.Controls.Add(this.label8);
            this.Name = "P19_SeparateHistory";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P19.TK Separate History";
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.comboNewID, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.gridTransferWKList, 0);
            this.Controls.SetChildIndex(this.gridPackingList, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridTransferWKList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPackingList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.Label label8;
        private Win.UI.ComboBox comboNewID;
        private Win.UI.Label label9;
        private Win.UI.Label label10;
        private Win.UI.Grid gridTransferWKList;
        private Win.UI.Grid gridPackingList;
        private Win.UI.Button btnClose;
    }
}