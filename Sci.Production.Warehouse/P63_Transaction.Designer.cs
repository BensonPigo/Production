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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.displayRefno = new Sci.Win.UI.DisplayBox();
            this.displayInQty = new Sci.Win.UI.DisplayBox();
            this.displayDesc = new Sci.Win.UI.DisplayBox();
            this.displayOutQty = new Sci.Win.UI.DisplayBox();
            this.displayBalQty = new Sci.Win.UI.DisplayBox();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.gridLeft = new Sci.Win.UI.Grid();
            this.gridRight = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRight)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Refno";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "In Qty";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(201, 42);
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
            this.label5.Location = new System.Drawing.Point(435, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Bal. Qty";
            // 
            // displayRefno
            // 
            this.displayRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRefno.Location = new System.Drawing.Point(87, 8);
            this.displayRefno.Name = "displayRefno";
            this.displayRefno.Size = new System.Drawing.Size(111, 24);
            this.displayRefno.TabIndex = 5;
            // 
            // displayInQty
            // 
            this.displayInQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayInQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayInQty.Location = new System.Drawing.Point(87, 41);
            this.displayInQty.Name = "displayInQty";
            this.displayInQty.Size = new System.Drawing.Size(111, 24);
            this.displayInQty.TabIndex = 6;
            // 
            // displayDesc
            // 
            this.displayDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDesc.Location = new System.Drawing.Point(293, 8);
            this.displayDesc.Name = "displayDesc";
            this.displayDesc.Size = new System.Drawing.Size(703, 24);
            this.displayDesc.TabIndex = 7;
            // 
            // displayOutQty
            // 
            this.displayOutQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayOutQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayOutQty.Location = new System.Drawing.Point(293, 41);
            this.displayOutQty.Name = "displayOutQty";
            this.displayOutQty.Size = new System.Drawing.Size(139, 24);
            this.displayOutQty.TabIndex = 8;
            // 
            // displayBalQty
            // 
            this.displayBalQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBalQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBalQty.Location = new System.Drawing.Point(527, 41);
            this.displayBalQty.Name = "displayBalQty";
            this.displayBalQty.Size = new System.Drawing.Size(139, 24);
            this.displayBalQty.TabIndex = 9;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(9, 71);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.gridLeft);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.gridRight);
            this.splitContainer.Size = new System.Drawing.Size(993, 394);
            this.splitContainer.SplitterDistance = 435;
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridLeft.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridLeft.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridLeft.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridLeft.RowTemplate.Height = 25;
            this.gridLeft.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLeft.ShowCellToolTips = false;
            this.gridLeft.Size = new System.Drawing.Size(435, 394);
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
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridRight.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridRight.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridRight.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridRight.RowTemplate.Height = 25;
            this.gridRight.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRight.ShowCellToolTips = false;
            this.gridRight.Size = new System.Drawing.Size(554, 394);
            this.gridRight.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(922, 471);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P63_Transaction
            // 
            this.ClientSize = new System.Drawing.Size(1014, 510);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.displayBalQty);
            this.Controls.Add(this.displayOutQty);
            this.Controls.Add(this.displayDesc);
            this.Controls.Add(this.displayInQty);
            this.Controls.Add(this.displayRefno);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.DisplayBox displayRefno;
        private Win.UI.DisplayBox displayInQty;
        private Win.UI.DisplayBox displayDesc;
        private Win.UI.DisplayBox displayOutQty;
        private Win.UI.DisplayBox displayBalQty;
        private System.Windows.Forms.SplitContainer splitContainer;
        private Win.UI.Grid gridLeft;
        private Win.UI.Grid gridRight;
        private Win.UI.Button btnClose;
    }
}
