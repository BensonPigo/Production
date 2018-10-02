namespace Sci.Production.Thread
{
    partial class B05
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grid1 = new Sci.Win.UI.Grid();
            this.labelRefno = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelThreadType = new Sci.Win.UI.Label();
            this.labelThreadTex = new Sci.Win.UI.Label();
            this.checkjunk = new Sci.Win.UI.CheckBox();
            this.displayRefno = new Sci.Win.UI.DisplayBox();
            this.displayDescription = new Sci.Win.UI.DisplayBox();
            this.displayCategory = new Sci.Win.UI.DisplayBox();
            this.displayThreadType = new Sci.Win.UI.DisplayBox();
            this.numThreadTex = new Sci.Win.UI.NumericBox();
            this.comboThreadColorLocation = new Sci.Win.UI.ComboBox();
            this.txtThreadColorLocation = new Sci.Win.UI.TextBox();
            this.btnFilter = new Sci.Win.UI.Button();
            this.btnRecalculateStockQty = new Sci.Win.UI.Button();
            this.labelTransactionDate = new Sci.Win.UI.Label();
            this.dateTransactionDate = new Sci.Win.UI.DateRange();
            this.btnQuery = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).BeginInit();
            this.masterpanel.SuspendLayout();
            this.detailpanel.SuspendLayout();
            this.detail2.SuspendLayout();
            this.detailpanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.btnRecalculateStockQty);
            this.masterpanel.Controls.Add(this.btnFilter);
            this.masterpanel.Controls.Add(this.txtThreadColorLocation);
            this.masterpanel.Controls.Add(this.comboThreadColorLocation);
            this.masterpanel.Controls.Add(this.numThreadTex);
            this.masterpanel.Controls.Add(this.displayThreadType);
            this.masterpanel.Controls.Add(this.displayCategory);
            this.masterpanel.Controls.Add(this.displayDescription);
            this.masterpanel.Controls.Add(this.displayRefno);
            this.masterpanel.Controls.Add(this.checkjunk);
            this.masterpanel.Controls.Add(this.labelThreadTex);
            this.masterpanel.Controls.Add(this.labelThreadType);
            this.masterpanel.Controls.Add(this.labelCategory);
            this.masterpanel.Controls.Add(this.labelDescription);
            this.masterpanel.Controls.Add(this.labelRefno);
            this.masterpanel.Size = new System.Drawing.Size(911, 108);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRefno, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDescription, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCategory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelThreadType, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelThreadTex, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkjunk, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayRefno, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayDescription, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayCategory, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayThreadType, 0);
            this.masterpanel.Controls.SetChildIndex(this.numThreadTex, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboThreadColorLocation, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtThreadColorLocation, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnFilter, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnRecalculateStockQty, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 108);
            this.detailpanel.Size = new System.Drawing.Size(911, 172);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(796, 70);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(828, 217);
            this.refresh.Size = new System.Drawing.Size(80, 28);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(911, 172);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(892, 387);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(886, 341);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(886, 40);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(911, 528);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(911, 280);
            // 
            // detailbtm
            // 
            this.detailbtm.Controls.Add(this.btnQuery);
            this.detailbtm.Controls.Add(this.dateTransactionDate);
            this.detailbtm.Controls.Add(this.labelTransactionDate);
            this.detailbtm.Controls.Add(this.grid1);
            this.detailbtm.Location = new System.Drawing.Point(0, 280);
            this.detailbtm.Size = new System.Drawing.Size(911, 248);
            this.detailbtm.Controls.SetChildIndex(this.lbleditby, 0);
            this.detailbtm.Controls.SetChildIndex(this.lblcreateby, 0);
            this.detailbtm.Controls.SetChildIndex(this.editby, 0);
            this.detailbtm.Controls.SetChildIndex(this.createby, 0);
            this.detailbtm.Controls.SetChildIndex(this.refresh, 0);
            this.detailbtm.Controls.SetChildIndex(this.grid1, 0);
            this.detailbtm.Controls.SetChildIndex(this.labelTransactionDate, 0);
            this.detailbtm.Controls.SetChildIndex(this.dateTransactionDate, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnQuery, 0);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(911, 528);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(919, 557);
            // 
            // createby
            // 
            this.createby.Location = new System.Drawing.Point(70, 217);
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 217);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lblcreateby
            // 
            this.lblcreateby.Location = new System.Drawing.Point(5, 223);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 223);
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 36);
            this.grid1.Name = "grid1";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(911, 179);
            this.grid1.TabIndex = 3;
            this.grid1.TabStop = false;
            // 
            // labelRefno
            // 
            this.labelRefno.Location = new System.Drawing.Point(28, 13);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(75, 23);
            this.labelRefno.TabIndex = 1;
            this.labelRefno.Text = "Refno";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(28, 48);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = "Description";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(293, 13);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(75, 23);
            this.labelCategory.TabIndex = 3;
            this.labelCategory.Text = "Category";
            // 
            // labelThreadType
            // 
            this.labelThreadType.Location = new System.Drawing.Point(506, 13);
            this.labelThreadType.Name = "labelThreadType";
            this.labelThreadType.Size = new System.Drawing.Size(88, 23);
            this.labelThreadType.TabIndex = 4;
            this.labelThreadType.Text = "Thread Type";
            // 
            // labelThreadTex
            // 
            this.labelThreadTex.Location = new System.Drawing.Point(506, 48);
            this.labelThreadTex.Name = "labelThreadTex";
            this.labelThreadTex.Size = new System.Drawing.Size(88, 23);
            this.labelThreadTex.TabIndex = 5;
            this.labelThreadTex.Text = "Thread Tex";
            // 
            // checkjunk
            // 
            this.checkjunk.AutoSize = true;
            this.checkjunk.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "junk", true));
            this.checkjunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkjunk.Location = new System.Drawing.Point(697, 50);
            this.checkjunk.Name = "checkjunk";
            this.checkjunk.Size = new System.Drawing.Size(53, 21);
            this.checkjunk.TabIndex = 6;
            this.checkjunk.Text = "junk";
            this.checkjunk.UseVisualStyleBackColor = true;
            // 
            // displayRefno
            // 
            this.displayRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRefno.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "refno", true));
            this.displayRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRefno.Location = new System.Drawing.Point(106, 13);
            this.displayRefno.Name = "displayRefno";
            this.displayRefno.Size = new System.Drawing.Size(184, 23);
            this.displayRefno.TabIndex = 7;
            // 
            // displayDescription
            // 
            this.displayDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDescription.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "description", true));
            this.displayDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDescription.Location = new System.Drawing.Point(106, 48);
            this.displayDescription.Name = "displayDescription";
            this.displayDescription.Size = new System.Drawing.Size(314, 23);
            this.displayDescription.TabIndex = 8;
            // 
            // displayCategory
            // 
            this.displayCategory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCategory.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "category", true));
            this.displayCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCategory.Location = new System.Drawing.Point(371, 13);
            this.displayCategory.Name = "displayCategory";
            this.displayCategory.Size = new System.Drawing.Size(132, 23);
            this.displayCategory.TabIndex = 9;
            // 
            // displayThreadType
            // 
            this.displayThreadType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayThreadType.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "threadtypeid", true));
            this.displayThreadType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayThreadType.Location = new System.Drawing.Point(597, 13);
            this.displayThreadType.Name = "displayThreadType";
            this.displayThreadType.Size = new System.Drawing.Size(153, 23);
            this.displayThreadType.TabIndex = 10;
            // 
            // numThreadTex
            // 
            this.numThreadTex.BackColor = System.Drawing.Color.White;
            this.numThreadTex.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "threadtex", true));
            this.numThreadTex.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numThreadTex.Location = new System.Drawing.Point(597, 48);
            this.numThreadTex.Name = "numThreadTex";
            this.numThreadTex.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numThreadTex.Size = new System.Drawing.Size(61, 23);
            this.numThreadTex.TabIndex = 11;
            this.numThreadTex.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // comboThreadColorLocation
            // 
            this.comboThreadColorLocation.BackColor = System.Drawing.Color.White;
            this.comboThreadColorLocation.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboThreadColorLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboThreadColorLocation.FormattingEnabled = true;
            this.comboThreadColorLocation.IsSupportUnselect = true;
            this.comboThreadColorLocation.Location = new System.Drawing.Point(28, 79);
            this.comboThreadColorLocation.Name = "comboThreadColorLocation";
            this.comboThreadColorLocation.OldText = "";
            this.comboThreadColorLocation.Size = new System.Drawing.Size(135, 24);
            this.comboThreadColorLocation.TabIndex = 12;
            // 
            // txtThreadColorLocation
            // 
            this.txtThreadColorLocation.BackColor = System.Drawing.Color.White;
            this.txtThreadColorLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtThreadColorLocation.IsSupportEditMode = false;
            this.txtThreadColorLocation.Location = new System.Drawing.Point(173, 80);
            this.txtThreadColorLocation.Name = "txtThreadColorLocation";
            this.txtThreadColorLocation.Size = new System.Drawing.Size(117, 23);
            this.txtThreadColorLocation.TabIndex = 13;
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(296, 76);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(80, 30);
            this.btnFilter.TabIndex = 14;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.BtnFilter_Click);
            // 
            // btnRecalculateStockQty
            // 
            this.btnRecalculateStockQty.Location = new System.Drawing.Point(732, 72);
            this.btnRecalculateStockQty.Name = "btnRecalculateStockQty";
            this.btnRecalculateStockQty.Size = new System.Drawing.Size(171, 30);
            this.btnRecalculateStockQty.TabIndex = 15;
            this.btnRecalculateStockQty.Text = "Re-calculate stock qty";
            this.btnRecalculateStockQty.UseVisualStyleBackColor = true;
            this.btnRecalculateStockQty.Click += new System.EventHandler(this.BtnRecalculateStockQty_Click);
            // 
            // labelTransactionDate
            // 
            this.labelTransactionDate.Location = new System.Drawing.Point(28, 4);
            this.labelTransactionDate.Name = "labelTransactionDate";
            this.labelTransactionDate.Size = new System.Drawing.Size(112, 23);
            this.labelTransactionDate.TabIndex = 16;
            this.labelTransactionDate.Text = "Transaction Date";
            // 
            // dateTransactionDate
            // 
            // 
            // 
            // 
            this.dateTransactionDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateTransactionDate.DateBox1.Name = "";
            this.dateTransactionDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateTransactionDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateTransactionDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateTransactionDate.DateBox2.Name = "";
            this.dateTransactionDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateTransactionDate.DateBox2.TabIndex = 1;
            this.dateTransactionDate.IsSupportEditMode = false;
            this.dateTransactionDate.Location = new System.Drawing.Point(143, 4);
            this.dateTransactionDate.Name = "dateTransactionDate";
            this.dateTransactionDate.Size = new System.Drawing.Size(280, 23);
            this.dateTransactionDate.TabIndex = 17;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(433, 3);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 16;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // B05
            // 
            this.ClientSize = new System.Drawing.Size(919, 590);
            this.DefaultDetailOrder = "ThreadColorid,ThreadLocationid";
            this.DefaultOrder = "Refno";
            this.GridAlias = "ThreadStock";
            this.GridUniqueKey = "threadLocationid,ThreadColorid";
            this.IsGridIconVisible = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "refno";
            this.Name = "B05";
            this.Text = "B05.Thread Stock";
            this.WorkAlias = "Localitem";
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).EndInit();
            this.masterpanel.ResumeLayout(false);
            this.masterpanel.PerformLayout();
            this.detailpanel.ResumeLayout(false);
            this.detail2.ResumeLayout(false);
            this.detailpanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button btnRecalculateStockQty;
        private Win.UI.Button btnFilter;
        private Win.UI.TextBox txtThreadColorLocation;
        private Win.UI.ComboBox comboThreadColorLocation;
        private Win.UI.NumericBox numThreadTex;
        private Win.UI.DisplayBox displayThreadType;
        private Win.UI.DisplayBox displayCategory;
        private Win.UI.DisplayBox displayDescription;
        private Win.UI.DisplayBox displayRefno;
        private Win.UI.CheckBox checkjunk;
        private Win.UI.Label labelThreadTex;
        private Win.UI.Label labelThreadType;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelRefno;
        private Win.UI.Button btnQuery;
        private Win.UI.DateRange dateTransactionDate;
        private Win.UI.Label labelTransactionDate;
        private Win.UI.Grid grid1;
    }
}
