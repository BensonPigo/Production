namespace Sci.Production.Warehouse
{
    partial class P03_Transaction
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridTransactionDetail = new Sci.Win.UI.Grid();
            this.panel2 = new Sci.Win.UI.Panel();
            this.numTotal4 = new Sci.Win.UI.NumericBox();
            this.btnPrint = new Sci.Win.UI.Button();
            this.labelTotal = new Sci.Win.UI.Label();
            this.numTotal3 = new Sci.Win.UI.NumericBox();
            this.numTotal2 = new Sci.Win.UI.NumericBox();
            this.numTotal1 = new Sci.Win.UI.NumericBox();
            this.btnReCalculate = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.numTotal5 = new Sci.Win.UI.NumericBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTransactionDetail)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.gridTransactionDetail);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 507);
            this.panel1.TabIndex = 0;
            // 
            // gridTransactionDetail
            // 
            this.gridTransactionDetail.AllowUserToAddRows = false;
            this.gridTransactionDetail.AllowUserToDeleteRows = false;
            this.gridTransactionDetail.AllowUserToResizeRows = false;
            this.gridTransactionDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridTransactionDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridTransactionDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTransactionDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTransactionDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridTransactionDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridTransactionDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridTransactionDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridTransactionDetail.Location = new System.Drawing.Point(0, 0);
            this.gridTransactionDetail.Name = "gridTransactionDetail";
            this.gridTransactionDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridTransactionDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridTransactionDetail.RowTemplate.Height = 24;
            this.gridTransactionDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTransactionDetail.ShowCellToolTips = false;
            this.gridTransactionDetail.Size = new System.Drawing.Size(1008, 507);
            this.gridTransactionDetail.TabIndex = 0;
            this.gridTransactionDetail.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.numTotal5);
            this.panel2.Controls.Add(this.numTotal4);
            this.panel2.Controls.Add(this.btnPrint);
            this.panel2.Controls.Add(this.labelTotal);
            this.panel2.Controls.Add(this.numTotal3);
            this.panel2.Controls.Add(this.numTotal2);
            this.panel2.Controls.Add(this.numTotal1);
            this.panel2.Controls.Add(this.btnReCalculate);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 513);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 48);
            this.panel2.TabIndex = 0;
            // 
            // numTotal4
            // 
            this.numTotal4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotal4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotal4.IsSupportEditMode = false;
            this.numTotal4.Location = new System.Drawing.Point(622, 15);
            this.numTotal4.Name = "numTotal4";
            this.numTotal4.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotal4.ReadOnly = true;
            this.numTotal4.Size = new System.Drawing.Size(100, 23);
            this.numTotal4.TabIndex = 5;
            this.numTotal4.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(834, 11);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(80, 30);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // labelTotal
            // 
            this.labelTotal.Location = new System.Drawing.Point(226, 15);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(75, 23);
            this.labelTotal.TabIndex = 4;
            this.labelTotal.Text = "Total";
            // 
            // numTotal3
            // 
            this.numTotal3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotal3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotal3.IsSupportEditMode = false;
            this.numTotal3.Location = new System.Drawing.Point(516, 15);
            this.numTotal3.Name = "numTotal3";
            this.numTotal3.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotal3.ReadOnly = true;
            this.numTotal3.Size = new System.Drawing.Size(100, 23);
            this.numTotal3.TabIndex = 3;
            this.numTotal3.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numTotal2
            // 
            this.numTotal2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotal2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotal2.IsSupportEditMode = false;
            this.numTotal2.Location = new System.Drawing.Point(410, 15);
            this.numTotal2.Name = "numTotal2";
            this.numTotal2.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotal2.ReadOnly = true;
            this.numTotal2.Size = new System.Drawing.Size(100, 23);
            this.numTotal2.TabIndex = 2;
            this.numTotal2.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numTotal1
            // 
            this.numTotal1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotal1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotal1.IsSupportEditMode = false;
            this.numTotal1.Location = new System.Drawing.Point(304, 15);
            this.numTotal1.Name = "numTotal1";
            this.numTotal1.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotal1.ReadOnly = true;
            this.numTotal1.Size = new System.Drawing.Size(100, 23);
            this.numTotal1.TabIndex = 1;
            this.numTotal1.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // btnReCalculate
            // 
            this.btnReCalculate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnReCalculate.Location = new System.Drawing.Point(12, 11);
            this.btnReCalculate.Name = "btnReCalculate";
            this.btnReCalculate.Size = new System.Drawing.Size(113, 30);
            this.btnReCalculate.TabIndex = 0;
            this.btnReCalculate.Text = "Re-Calculate";
            this.btnReCalculate.UseVisualStyleBackColor = true;
            this.btnReCalculate.Click += new System.EventHandler(this.BtnReCalculate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(916, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // numTotal5
            // 
            this.numTotal5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotal5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotal5.IsSupportEditMode = false;
            this.numTotal5.Location = new System.Drawing.Point(728, 15);
            this.numTotal5.Name = "numTotal5";
            this.numTotal5.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotal5.ReadOnly = true;
            this.numTotal5.Size = new System.Drawing.Size(100, 23);
            this.numTotal5.TabIndex = 6;
            this.numTotal5.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // P03_Transaction
            // 
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P03_Transaction";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Transaction Detail";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTransactionDetail)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Grid gridTransactionDetail;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Button btnReCalculate;
        private Win.UI.Label labelTotal;
        private Win.UI.NumericBox numTotal3;
        private Win.UI.NumericBox numTotal2;
        private Win.UI.NumericBox numTotal1;
        private Win.UI.Button btnPrint;
        private Win.UI.NumericBox numTotal4;
        private Win.UI.NumericBox numTotal5;
    }
}
