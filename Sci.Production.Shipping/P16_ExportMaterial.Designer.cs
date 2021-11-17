namespace Sci.Production.Shipping
{
    partial class P16_ExportMaterial
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dispFromSP = new Sci.Win.UI.DisplayBox();
            this.labFromSP = new Sci.Win.UI.Label();
            this.dispFromSeq = new Sci.Win.UI.DisplayBox();
            this.labFromSeq = new Sci.Win.UI.Label();
            this.dispRefno = new Sci.Win.UI.DisplayBox();
            this.labRefno = new Sci.Win.UI.Label();
            this.dispColor = new Sci.Win.UI.DisplayBox();
            this.labColorID = new Sci.Win.UI.Label();
            this.dispExportQty = new Sci.Win.UI.DisplayBox();
            this.labExportQty = new Sci.Win.UI.Label();
            this.dispToSeq = new Sci.Win.UI.DisplayBox();
            this.labToSeq = new Sci.Win.UI.Label();
            this.dispToSP = new Sci.Win.UI.DisplayBox();
            this.labToSp = new Sci.Win.UI.Label();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.grid3 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 571);
            this.btmcont.Size = new System.Drawing.Size(734, 10);
            this.btmcont.Visible = false;
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(644, 5);
            this.undo.Size = new System.Drawing.Size(80, 0);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(564, 5);
            this.save.Size = new System.Drawing.Size(80, 0);
            // 
            // left
            // 
            this.left.Size = new System.Drawing.Size(80, 0);
            // 
            // right
            // 
            this.right.Size = new System.Drawing.Size(80, 0);
            // 
            // dispFromSP
            // 
            this.dispFromSP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispFromSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispFromSP.Location = new System.Drawing.Point(86, 12);
            this.dispFromSP.Name = "dispFromSP";
            this.dispFromSP.Size = new System.Drawing.Size(118, 23);
            this.dispFromSP.TabIndex = 111;
            // 
            // labFromSP
            // 
            this.labFromSP.Location = new System.Drawing.Point(8, 12);
            this.labFromSP.Name = "labFromSP";
            this.labFromSP.Size = new System.Drawing.Size(75, 23);
            this.labFromSP.TabIndex = 107;
            this.labFromSP.Text = "From SP#";
            // 
            // dispFromSeq
            // 
            this.dispFromSeq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispFromSeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispFromSeq.Location = new System.Drawing.Point(287, 11);
            this.dispFromSeq.Name = "dispFromSeq";
            this.dispFromSeq.Size = new System.Drawing.Size(82, 23);
            this.dispFromSeq.TabIndex = 113;
            // 
            // labFromSeq
            // 
            this.labFromSeq.Location = new System.Drawing.Point(209, 11);
            this.labFromSeq.Name = "labFromSeq";
            this.labFromSeq.Size = new System.Drawing.Size(75, 23);
            this.labFromSeq.TabIndex = 112;
            this.labFromSeq.Text = "From Seq";
            // 
            // dispRefno
            // 
            this.dispRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispRefno.Location = new System.Drawing.Point(452, 11);
            this.dispRefno.Name = "dispRefno";
            this.dispRefno.Size = new System.Drawing.Size(118, 23);
            this.dispRefno.TabIndex = 115;
            // 
            // labRefno
            // 
            this.labRefno.Location = new System.Drawing.Point(374, 11);
            this.labRefno.Name = "labRefno";
            this.labRefno.Size = new System.Drawing.Size(75, 23);
            this.labRefno.TabIndex = 114;
            this.labRefno.Text = "Refno";
            // 
            // dispColor
            // 
            this.dispColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispColor.Location = new System.Drawing.Point(653, 11);
            this.dispColor.Name = "dispColor";
            this.dispColor.Size = new System.Drawing.Size(72, 23);
            this.dispColor.TabIndex = 117;
            // 
            // labColorID
            // 
            this.labColorID.Location = new System.Drawing.Point(575, 11);
            this.labColorID.Name = "labColorID";
            this.labColorID.Size = new System.Drawing.Size(75, 23);
            this.labColorID.TabIndex = 116;
            this.labColorID.Text = "ColorID";
            // 
            // dispExportQty
            // 
            this.dispExportQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispExportQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispExportQty.Location = new System.Drawing.Point(452, 38);
            this.dispExportQty.Name = "dispExportQty";
            this.dispExportQty.Size = new System.Drawing.Size(118, 23);
            this.dispExportQty.TabIndex = 123;
            // 
            // labExportQty
            // 
            this.labExportQty.Location = new System.Drawing.Point(374, 38);
            this.labExportQty.Name = "labExportQty";
            this.labExportQty.Size = new System.Drawing.Size(75, 23);
            this.labExportQty.TabIndex = 122;
            this.labExportQty.Text = "Export Qty";
            // 
            // dispToSeq
            // 
            this.dispToSeq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispToSeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispToSeq.Location = new System.Drawing.Point(287, 38);
            this.dispToSeq.Name = "dispToSeq";
            this.dispToSeq.Size = new System.Drawing.Size(82, 23);
            this.dispToSeq.TabIndex = 121;
            // 
            // labToSeq
            // 
            this.labToSeq.Location = new System.Drawing.Point(209, 38);
            this.labToSeq.Name = "labToSeq";
            this.labToSeq.Size = new System.Drawing.Size(75, 23);
            this.labToSeq.TabIndex = 120;
            this.labToSeq.Text = "To Seq";
            // 
            // dispToSP
            // 
            this.dispToSP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispToSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispToSP.Location = new System.Drawing.Point(86, 38);
            this.dispToSP.Name = "dispToSP";
            this.dispToSP.Size = new System.Drawing.Size(118, 23);
            this.dispToSP.TabIndex = 119;
            // 
            // labToSp
            // 
            this.labToSp.Location = new System.Drawing.Point(9, 39);
            this.labToSp.Name = "labToSp";
            this.labToSp.Size = new System.Drawing.Size(75, 23);
            this.labToSp.TabIndex = 118;
            this.labToSp.Text = "To SP#";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dispFromSP);
            this.panel3.Controls.Add(this.labFromSP);
            this.panel3.Controls.Add(this.dispExportQty);
            this.panel3.Controls.Add(this.labFromSeq);
            this.panel3.Controls.Add(this.labExportQty);
            this.panel3.Controls.Add(this.dispFromSeq);
            this.panel3.Controls.Add(this.dispToSeq);
            this.panel3.Controls.Add(this.labRefno);
            this.panel3.Controls.Add(this.labToSeq);
            this.panel3.Controls.Add(this.dispRefno);
            this.panel3.Controls.Add(this.dispToSP);
            this.panel3.Controls.Add(this.labColorID);
            this.panel3.Controls.Add(this.labToSp);
            this.panel3.Controls.Add(this.dispColor);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(734, 74);
            this.panel3.TabIndex = 124;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 524);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(734, 47);
            this.panel4.TabIndex = 125;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(642, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // grid3
            // 
            this.grid3.AllowUserToAddRows = false;
            this.grid3.AllowUserToDeleteRows = false;
            this.grid3.AllowUserToResizeRows = false;
            this.grid3.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid3.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid3.DataSource = this.listControlBindingSource1;
            this.grid3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid3.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid3.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid3.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid3.Location = new System.Drawing.Point(0, 74);
            this.grid3.Name = "grid3";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid3.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.grid3.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid3.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid3.RowTemplate.Height = 24;
            this.grid3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid3.ShowCellToolTips = false;
            this.grid3.Size = new System.Drawing.Size(734, 450);
            this.grid3.TabIndex = 126;
            this.grid3.TabStop = false;
            // 
            // P16_ExportMaterial
            // 
            this.ClientSize = new System.Drawing.Size(734, 581);
            this.Controls.Add(this.grid3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Name = "P16_ExportMaterial";
            this.OnLineHelpID = "Sci.Win.Subs.Input6A";
            this.Text = "Export Material";
            this.WorkAlias = "TransferExport_Detail";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.grid3, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.DisplayBox dispFromSP;
        private Win.UI.Label labFromSP;
        private Win.UI.DisplayBox dispFromSeq;
        private Win.UI.Label labFromSeq;
        private Win.UI.DisplayBox dispRefno;
        private Win.UI.Label labRefno;
        private Win.UI.DisplayBox dispColor;
        private Win.UI.Label labColorID;
        private Win.UI.DisplayBox dispExportQty;
        private Win.UI.Label labExportQty;
        private Win.UI.DisplayBox dispToSeq;
        private Win.UI.Label labToSeq;
        private Win.UI.DisplayBox dispToSP;
        private Win.UI.Label labToSp;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Button btnClose;
        private Win.UI.Grid grid3;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}
