namespace Sci.Production.Warehouse
{
    partial class P63_Transaction
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
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.labSeq = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.displaySeq = new Sci.Win.UI.DisplayBox();
            this.displayInQty = new Sci.Win.UI.DisplayBox();
            this.displayOutQty = new Sci.Win.UI.DisplayBox();
            this.displayBalQty = new Sci.Win.UI.DisplayBox();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.gridLeft = new Sci.Win.UI.Grid();
            this.gridRight = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            this.displayDesc = new Sci.Win.UI.EditBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dispTotalBalance = new Sci.Win.UI.DisplayBox();
            this.dispTotalAdjustQty = new Sci.Win.UI.DisplayBox();
            this.dispTotalReleasedQty = new Sci.Win.UI.DisplayBox();
            this.dispTotalArrivedQty = new Sci.Win.UI.DisplayBox();
            this.label1 = new Sci.Win.UI.Label();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnReCalculate = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRight)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labSeq
            // 
            this.labSeq.Location = new System.Drawing.Point(9, 9);
            this.labSeq.Name = "labSeq";
            this.labSeq.Size = new System.Drawing.Size(75, 23);
            this.labSeq.TabIndex = 0;
            this.labSeq.Text = "Seq";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "In Qty";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(201, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Out Qty";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(201, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Description";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(435, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Bal. Qty";
            // 
            // displaySeq
            // 
            this.displaySeq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeq.Location = new System.Drawing.Point(87, 8);
            this.displaySeq.Name = "displaySeq";
            this.displaySeq.Size = new System.Drawing.Size(111, 23);
            this.displaySeq.TabIndex = 5;
            // 
            // displayInQty
            // 
            this.displayInQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayInQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayInQty.Location = new System.Drawing.Point(87, 68);
            this.displayInQty.Name = "displayInQty";
            this.displayInQty.Size = new System.Drawing.Size(111, 23);
            this.displayInQty.TabIndex = 6;
            // 
            // displayOutQty
            // 
            this.displayOutQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayOutQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayOutQty.Location = new System.Drawing.Point(293, 68);
            this.displayOutQty.Name = "displayOutQty";
            this.displayOutQty.Size = new System.Drawing.Size(139, 23);
            this.displayOutQty.TabIndex = 8;
            // 
            // displayBalQty
            // 
            this.displayBalQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBalQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBalQty.Location = new System.Drawing.Point(527, 68);
            this.displayBalQty.Name = "displayBalQty";
            this.displayBalQty.Size = new System.Drawing.Size(139, 23);
            this.displayBalQty.TabIndex = 9;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(3, 3);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.gridLeft);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.gridRight);
            this.splitContainer.Size = new System.Drawing.Size(979, 462);
            this.splitContainer.SplitterDistance = 428;
            this.splitContainer.TabIndex = 10;
            // 
            // gridLeft
            // 
            this.gridLeft.AllowUserToAddRows = false;
            this.gridLeft.AllowUserToDeleteRows = false;
            this.gridLeft.AllowUserToResizeRows = false;
            this.gridLeft.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridLeft.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridLeft.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridLeft.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridLeft.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridLeft.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridLeft.Location = new System.Drawing.Point(0, 0);
            this.gridLeft.Name = "gridLeft";
            this.gridLeft.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridLeft.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridLeft.RowTemplate.Height = 25;
            this.gridLeft.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLeft.ShowCellToolTips = false;
            this.gridLeft.Size = new System.Drawing.Size(428, 462);
            this.gridLeft.TabIndex = 0;
            this.gridLeft.SelectionChanged += new System.EventHandler(this.GridLeft_SelectionChanged);
            // 
            // gridRight
            // 
            this.gridRight.AllowUserToAddRows = false;
            this.gridRight.AllowUserToDeleteRows = false;
            this.gridRight.AllowUserToResizeRows = false;
            this.gridRight.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridRight.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridRight.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRight.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridRight.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridRight.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridRight.Location = new System.Drawing.Point(0, 0);
            this.gridRight.Name = "gridRight";
            this.gridRight.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridRight.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridRight.RowTemplate.Height = 25;
            this.gridRight.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRight.ShowCellToolTips = false;
            this.gridRight.Size = new System.Drawing.Size(547, 462);
            this.gridRight.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(922, 600);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // displayDesc
            // 
            this.displayDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDesc.IsSupportEditMode = false;
            this.displayDesc.Location = new System.Drawing.Point(293, 8);
            this.displayDesc.Multiline = true;
            this.displayDesc.Name = "displayDesc";
            this.displayDesc.ReadOnly = true;
            this.displayDesc.Size = new System.Drawing.Size(709, 54);
            this.displayDesc.TabIndex = 12;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(9, 97);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(993, 497);
            this.tabControl1.TabIndex = 13;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dispTotalBalance);
            this.tabPage1.Controls.Add(this.dispTotalAdjustQty);
            this.tabPage1.Controls.Add(this.dispTotalReleasedQty);
            this.tabPage1.Controls.Add(this.dispTotalArrivedQty);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.gridDetail);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(985, 468);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Detail";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dispTotalBalance
            // 
            this.dispTotalBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dispTotalBalance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispTotalBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispTotalBalance.Location = new System.Drawing.Point(761, 439);
            this.dispTotalBalance.Name = "dispTotalBalance";
            this.dispTotalBalance.Size = new System.Drawing.Size(68, 23);
            this.dispTotalBalance.TabIndex = 14;
            // 
            // dispTotalAdjustQty
            // 
            this.dispTotalAdjustQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dispTotalAdjustQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispTotalAdjustQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispTotalAdjustQty.Location = new System.Drawing.Point(670, 439);
            this.dispTotalAdjustQty.Name = "dispTotalAdjustQty";
            this.dispTotalAdjustQty.Size = new System.Drawing.Size(87, 23);
            this.dispTotalAdjustQty.TabIndex = 13;
            // 
            // dispTotalReleasedQty
            // 
            this.dispTotalReleasedQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dispTotalReleasedQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispTotalReleasedQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispTotalReleasedQty.Location = new System.Drawing.Point(577, 439);
            this.dispTotalReleasedQty.Name = "dispTotalReleasedQty";
            this.dispTotalReleasedQty.Size = new System.Drawing.Size(87, 23);
            this.dispTotalReleasedQty.TabIndex = 12;
            // 
            // dispTotalArrivedQty
            // 
            this.dispTotalArrivedQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dispTotalArrivedQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispTotalArrivedQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispTotalArrivedQty.Location = new System.Drawing.Point(485, 439);
            this.dispTotalArrivedQty.Name = "dispTotalArrivedQty";
            this.dispTotalArrivedQty.Size = new System.Drawing.Size(87, 23);
            this.dispTotalArrivedQty.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(392, 439);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "Total";
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(3, 0);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 25;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(979, 435);
            this.gridDetail.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(985, 468);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "by Roll# Dylot";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnReCalculate
            // 
            this.btnReCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReCalculate.Location = new System.Drawing.Point(13, 600);
            this.btnReCalculate.Name = "btnReCalculate";
            this.btnReCalculate.Size = new System.Drawing.Size(156, 30);
            this.btnReCalculate.TabIndex = 14;
            this.btnReCalculate.Text = "Re-Calculate";
            this.btnReCalculate.UseVisualStyleBackColor = true;
            this.btnReCalculate.Click += new System.EventHandler(this.BtnReCalculate_Click);
            // 
            // P63_Transaction
            // 
            this.ClientSize = new System.Drawing.Size(1014, 639);
            this.Controls.Add(this.btnReCalculate);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.displayDesc);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.displayBalQty);
            this.Controls.Add(this.displayOutQty);
            this.Controls.Add(this.displayInQty);
            this.Controls.Add(this.displaySeq);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labSeq);
            this.Name = "P63_Transaction";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Transaction Detail";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRight)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Label labSeq;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.DisplayBox displaySeq;
        private Win.UI.DisplayBox displayInQty;
        private Win.UI.DisplayBox displayOutQty;
        private Win.UI.DisplayBox displayBalQty;
        private System.Windows.Forms.SplitContainer splitContainer;
        private Win.UI.Grid gridLeft;
        private Win.UI.Grid gridRight;
        private Win.UI.Button btnClose;
        private Win.UI.EditBox displayDesc;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Win.UI.Grid gridDetail;
        private System.Windows.Forms.TabPage tabPage2;
        private Win.UI.DisplayBox dispTotalAdjustQty;
        private Win.UI.DisplayBox dispTotalReleasedQty;
        private Win.UI.DisplayBox dispTotalArrivedQty;
        private Win.UI.DisplayBox dispTotalBalance;
        private Win.UI.Label label1;
        private Win.UI.Button btnReCalculate;
    }
}
