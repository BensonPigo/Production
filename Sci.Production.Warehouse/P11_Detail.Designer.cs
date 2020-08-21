namespace Sci.Production.Warehouse
{
    partial class P11_Detail
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
            this.labelSeqNo = new Sci.Win.UI.Label();
            this.labelUnit = new Sci.Win.UI.Label();
            this.displySeqNo = new Sci.Win.UI.DisplayBox();
            this.displyUnit = new Sci.Win.UI.DisplayBox();
            this.displyColorid = new Sci.Win.UI.DisplayBox();
            this.labelColor = new Sci.Win.UI.Label();
            this.displySpecial = new Sci.Win.UI.DisplayBox();
            this.labelSpecial = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.eb_desc = new Sci.Win.UI.EditBox();
            this.editOrderList = new Sci.Win.UI.EditBox();
            this.labelQty = new Sci.Win.UI.Label();
            this.labelSize = new Sci.Win.UI.Label();
            this.displyQty = new Sci.Win.UI.DisplayBox();
            this.displySize = new Sci.Win.UI.DisplayBox();
            this.labelOrderList = new Sci.Win.UI.Label();
            this.gridBreakDown = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.labelTotalIssueQty = new Sci.Win.UI.Label();
            this.displayTotalIssueQty = new Sci.Win.UI.DisplayBox();
            this.label1 = new Sci.Win.UI.Label();
            this.displayDiffqty = new Sci.Win.UI.DisplayBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBreakDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(560, 223);
            this.gridcont.Size = new System.Drawing.Size(436, 284);
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.label1);
            this.btmcont.Controls.Add(this.displayDiffqty);
            this.btmcont.Controls.Add(this.labelTotalIssueQty);
            this.btmcont.Controls.Add(this.displayTotalIssueQty);
            this.btmcont.Location = new System.Drawing.Point(0, 517);
            this.btmcont.Size = new System.Drawing.Size(1008, 40);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.next, 0);
            this.btmcont.Controls.SetChildIndex(this.prev, 0);
            this.btmcont.Controls.SetChildIndex(this.displayTotalIssueQty, 0);
            this.btmcont.Controls.SetChildIndex(this.labelTotalIssueQty, 0);
            this.btmcont.Controls.SetChildIndex(this.displayDiffqty, 0);
            this.btmcont.Controls.SetChildIndex(this.label1, 0);
            // 
            // append
            // 
            this.append.Enabled = false;
            this.append.Visible = false;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(838, 5);
            this.save.Click += new System.EventHandler(this.Save_Click);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(918, 5);
            this.undo.Click += new System.EventHandler(this.Undo_Click);
            // 
            // revise
            // 
            this.revise.Enabled = false;
            this.revise.Visible = false;
            // 
            // delete
            // 
            this.delete.Enabled = false;
            this.delete.Visible = false;
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(783, 5);
            // 
            // prev
            // 
            this.prev.Location = new System.Drawing.Point(728, 5);
            // 
            // labelSeqNo
            // 
            this.labelSeqNo.Location = new System.Drawing.Point(9, 22);
            this.labelSeqNo.Name = "labelSeqNo";
            this.labelSeqNo.Size = new System.Drawing.Size(75, 23);
            this.labelSeqNo.TabIndex = 99;
            this.labelSeqNo.Text = "Seq#";
            // 
            // labelUnit
            // 
            this.labelUnit.Location = new System.Drawing.Point(252, 22);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(75, 23);
            this.labelUnit.TabIndex = 100;
            this.labelUnit.Text = "Unit";
            // 
            // displySeqNo
            // 
            this.displySeqNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displySeqNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displySeqNo.Location = new System.Drawing.Point(87, 22);
            this.displySeqNo.Name = "displySeqNo";
            this.displySeqNo.Size = new System.Drawing.Size(124, 23);
            this.displySeqNo.TabIndex = 101;
            // 
            // displyUnit
            // 
            this.displyUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displyUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displyUnit.Location = new System.Drawing.Point(330, 22);
            this.displyUnit.Name = "displyUnit";
            this.displyUnit.Size = new System.Drawing.Size(124, 23);
            this.displyUnit.TabIndex = 102;
            // 
            // displyColorid
            // 
            this.displyColorid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displyColorid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displyColorid.Location = new System.Drawing.Point(87, 92);
            this.displyColorid.Name = "displyColorid";
            this.displyColorid.Size = new System.Drawing.Size(124, 23);
            this.displyColorid.TabIndex = 104;
            // 
            // labelColor
            // 
            this.labelColor.Location = new System.Drawing.Point(9, 92);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(75, 23);
            this.labelColor.TabIndex = 103;
            this.labelColor.Text = "Color";
            // 
            // displySpecial
            // 
            this.displySpecial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displySpecial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displySpecial.Location = new System.Drawing.Point(87, 127);
            this.displySpecial.Name = "displySpecial";
            this.displySpecial.Size = new System.Drawing.Size(124, 23);
            this.displySpecial.TabIndex = 106;
            // 
            // labelSpecial
            // 
            this.labelSpecial.Location = new System.Drawing.Point(9, 127);
            this.labelSpecial.Name = "labelSpecial";
            this.labelSpecial.Size = new System.Drawing.Size(75, 23);
            this.labelSpecial.TabIndex = 105;
            this.labelSpecial.Text = "Special";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 220);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 107;
            this.label5.Text = "Desc";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.eb_desc);
            this.groupBox1.Controls.Add(this.editOrderList);
            this.groupBox1.Controls.Add(this.labelQty);
            this.groupBox1.Controls.Add(this.labelSize);
            this.groupBox1.Controls.Add(this.displyQty);
            this.groupBox1.Controls.Add(this.displySize);
            this.groupBox1.Controls.Add(this.labelOrderList);
            this.groupBox1.Controls.Add(this.labelSeqNo);
            this.groupBox1.Controls.Add(this.displySpecial);
            this.groupBox1.Controls.Add(this.labelSpecial);
            this.groupBox1.Controls.Add(this.labelUnit);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.displySeqNo);
            this.groupBox1.Controls.Add(this.displyUnit);
            this.groupBox1.Controls.Add(this.labelColor);
            this.groupBox1.Controls.Add(this.displyColorid);
            this.groupBox1.Location = new System.Drawing.Point(12, 223);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(542, 284);
            this.groupBox1.TabIndex = 110;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Issue Item";
            // 
            // eb_desc
            // 
            this.eb_desc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.eb_desc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.eb_desc.IsSupportEditMode = false;
            this.eb_desc.Location = new System.Drawing.Point(87, 220);
            this.eb_desc.Multiline = true;
            this.eb_desc.Name = "eb_desc";
            this.eb_desc.ReadOnly = true;
            this.eb_desc.Size = new System.Drawing.Size(449, 50);
            this.eb_desc.TabIndex = 116;
            // 
            // editOrderList
            // 
            this.editOrderList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editOrderList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editOrderList.IsSupportEditMode = false;
            this.editOrderList.Location = new System.Drawing.Point(87, 162);
            this.editOrderList.Multiline = true;
            this.editOrderList.Name = "editOrderList";
            this.editOrderList.ReadOnly = true;
            this.editOrderList.Size = new System.Drawing.Size(449, 50);
            this.editOrderList.TabIndex = 115;
            // 
            // labelQty
            // 
            this.labelQty.Location = new System.Drawing.Point(9, 57);
            this.labelQty.Name = "labelQty";
            this.labelQty.Size = new System.Drawing.Size(75, 23);
            this.labelQty.TabIndex = 111;
            this.labelQty.Text = "@Qty";
            // 
            // labelSize
            // 
            this.labelSize.Location = new System.Drawing.Point(252, 57);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(75, 23);
            this.labelSize.TabIndex = 112;
            this.labelSize.Text = "Size";
            // 
            // displyQty
            // 
            this.displyQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displyQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displyQty.Location = new System.Drawing.Point(87, 57);
            this.displyQty.Name = "displyQty";
            this.displyQty.Size = new System.Drawing.Size(124, 23);
            this.displyQty.TabIndex = 113;
            // 
            // displySize
            // 
            this.displySize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displySize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displySize.Location = new System.Drawing.Point(330, 57);
            this.displySize.Name = "displySize";
            this.displySize.Size = new System.Drawing.Size(124, 23);
            this.displySize.TabIndex = 114;
            // 
            // labelOrderList
            // 
            this.labelOrderList.Location = new System.Drawing.Point(9, 162);
            this.labelOrderList.Name = "labelOrderList";
            this.labelOrderList.Size = new System.Drawing.Size(75, 23);
            this.labelOrderList.TabIndex = 109;
            this.labelOrderList.Text = "Order List";
            // 
            // gridBreakDown
            // 
            this.gridBreakDown.AllowUserToAddRows = false;
            this.gridBreakDown.AllowUserToDeleteRows = false;
            this.gridBreakDown.AllowUserToResizeRows = false;
            this.gridBreakDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridBreakDown.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBreakDown.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBreakDown.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBreakDown.DataSource = this.listControlBindingSource1;
            this.gridBreakDown.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBreakDown.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBreakDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBreakDown.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBreakDown.Location = new System.Drawing.Point(10, 12);
            this.gridBreakDown.Name = "gridBreakDown";
            this.gridBreakDown.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBreakDown.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBreakDown.RowTemplate.Height = 24;
            this.gridBreakDown.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBreakDown.ShowCellToolTips = false;
            this.gridBreakDown.Size = new System.Drawing.Size(986, 205);
            this.gridBreakDown.TabIndex = 111;
            this.gridBreakDown.TabStop = false;
            // 
            // labelTotalIssueQty
            // 
            this.labelTotalIssueQty.Location = new System.Drawing.Point(499, 8);
            this.labelTotalIssueQty.Name = "labelTotalIssueQty";
            this.labelTotalIssueQty.Size = new System.Drawing.Size(99, 23);
            this.labelTotalIssueQty.TabIndex = 118;
            this.labelTotalIssueQty.Text = "Total Issue Qty";
            // 
            // displayTotalIssueQty
            // 
            this.displayTotalIssueQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTotalIssueQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTotalIssueQty.Location = new System.Drawing.Point(601, 8);
            this.displayTotalIssueQty.Name = "displayTotalIssueQty";
            this.displayTotalIssueQty.Size = new System.Drawing.Size(124, 23);
            this.displayTotalIssueQty.TabIndex = 117;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(263, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 23);
            this.label1.TabIndex = 120;
            this.label1.Text = "Diff with bal. qty";
            // 
            // displayDiffqty
            // 
            this.displayDiffqty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDiffqty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDiffqty.Location = new System.Drawing.Point(372, 8);
            this.displayDiffqty.Name = "displayDiffqty";
            this.displayDiffqty.Size = new System.Drawing.Size(124, 23);
            this.displayDiffqty.TabIndex = 119;
            // 
            // P11_Detail
            // 
            this.ClientSize = new System.Drawing.Size(1008, 557);
            this.Controls.Add(this.gridBreakDown);
            this.Controls.Add(this.groupBox1);
            this.Name = "P11_Detail";
            this.OnLineHelpID = "Sci.Win.Subs.Input8A";
            this.Text = "P11. Output Detail";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.gridBreakDown, 0);
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.btmcont.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBreakDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label labelSeqNo;
        private Win.UI.Label labelUnit;
        private Win.UI.DisplayBox displySeqNo;
        private Win.UI.DisplayBox displyUnit;
        private Win.UI.DisplayBox displyColorid;
        private Win.UI.Label labelColor;
        private Win.UI.DisplayBox displySpecial;
        private Win.UI.Label labelSpecial;
        private Win.UI.Label label5;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Label labelOrderList;
        private Win.UI.EditBox eb_desc;
        private Win.UI.EditBox editOrderList;
        private Win.UI.Label labelQty;
        private Win.UI.Label labelSize;
        private Win.UI.DisplayBox displyQty;
        private Win.UI.DisplayBox displySize;
        private Win.UI.Grid gridBreakDown;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label labelTotalIssueQty;
        private Win.UI.DisplayBox displayTotalIssueQty;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox displayDiffqty;
    }
}
