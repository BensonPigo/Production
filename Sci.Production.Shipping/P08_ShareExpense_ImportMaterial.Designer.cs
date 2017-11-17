namespace Sci.Production.Shipping
{
    partial class P08_ShareExpense_ImportMaterial
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
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.labelWKNo = new Sci.Win.UI.Label();
            this.txtWKNo = new Sci.Win.UI.TextBox();
            this.labelBLNo = new Sci.Win.UI.Label();
            this.txtBLNo = new Sci.Win.UI.TextBox();
            this.labelInvoiceNo = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtInvoiceNo = new Sci.Win.UI.TextBox();
            this.dateArrivePortDate = new Sci.Win.UI.DateRange();
            this.labelArrivePortDate = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridImport = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 515);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(760, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 515);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelWKNo);
            this.panel3.Controls.Add(this.txtWKNo);
            this.panel3.Controls.Add(this.labelBLNo);
            this.panel3.Controls.Add(this.txtBLNo);
            this.panel3.Controls.Add(this.labelInvoiceNo);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.txtInvoiceNo);
            this.panel3.Controls.Add(this.dateArrivePortDate);
            this.panel3.Controls.Add(this.labelArrivePortDate);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(755, 73);
            this.panel3.TabIndex = 2;
            // 
            // labelWKNo
            // 
            this.labelWKNo.Lines = 0;
            this.labelWKNo.Location = new System.Drawing.Point(293, 43);
            this.labelWKNo.Name = "labelWKNo";
            this.labelWKNo.Size = new System.Drawing.Size(52, 23);
            this.labelWKNo.TabIndex = 21;
            this.labelWKNo.Text = "WK No.";
            // 
            // txtWKNo
            // 
            this.txtWKNo.BackColor = System.Drawing.Color.White;
            this.txtWKNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWKNo.Location = new System.Drawing.Point(348, 43);
            this.txtWKNo.Name = "txtWKNo";
            this.txtWKNo.Size = new System.Drawing.Size(120, 23);
            this.txtWKNo.TabIndex = 20;
            // 
            // labelBLNo
            // 
            this.labelBLNo.Lines = 0;
            this.labelBLNo.Location = new System.Drawing.Point(4, 43);
            this.labelBLNo.Name = "labelBLNo";
            this.labelBLNo.Size = new System.Drawing.Size(52, 23);
            this.labelBLNo.TabIndex = 19;
            this.labelBLNo.Text = "B/L No.";
            // 
            // txtBLNo
            // 
            this.txtBLNo.BackColor = System.Drawing.Color.White;
            this.txtBLNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBLNo.Location = new System.Drawing.Point(59, 43);
            this.txtBLNo.Name = "txtBLNo";
            this.txtBLNo.Size = new System.Drawing.Size(185, 23);
            this.txtBLNo.TabIndex = 18;
            // 
            // labelInvoiceNo
            // 
            this.labelInvoiceNo.Lines = 0;
            this.labelInvoiceNo.Location = new System.Drawing.Point(416, 10);
            this.labelInvoiceNo.Name = "labelInvoiceNo";
            this.labelInvoiceNo.Size = new System.Drawing.Size(75, 23);
            this.labelInvoiceNo.TabIndex = 17;
            this.labelInvoiceNo.Text = "Invoice No.";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(663, 37);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtInvoiceNo
            // 
            this.txtInvoiceNo.BackColor = System.Drawing.Color.White;
            this.txtInvoiceNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvoiceNo.Location = new System.Drawing.Point(494, 10);
            this.txtInvoiceNo.Name = "txtInvoiceNo";
            this.txtInvoiceNo.Size = new System.Drawing.Size(230, 23);
            this.txtInvoiceNo.TabIndex = 1;
            // 
            // dateArrivePortDate
            // 
            this.dateArrivePortDate.IsRequired = false;
            this.dateArrivePortDate.Location = new System.Drawing.Point(112, 10);
            this.dateArrivePortDate.Name = "dateArrivePortDate";
            this.dateArrivePortDate.Size = new System.Drawing.Size(280, 23);
            this.dateArrivePortDate.TabIndex = 0;
            // 
            // labelArrivePortDate
            // 
            this.labelArrivePortDate.Lines = 0;
            this.labelArrivePortDate.Location = new System.Drawing.Point(4, 10);
            this.labelArrivePortDate.Name = "labelArrivePortDate";
            this.labelArrivePortDate.Size = new System.Drawing.Size(105, 23);
            this.labelArrivePortDate.TabIndex = 0;
            this.labelArrivePortDate.Text = "Arrive Port Date";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.btnImport);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 476);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(755, 39);
            this.panel4.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(644, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(549, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridImport);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 73);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(755, 403);
            this.panel5.TabIndex = 4;
            // 
            // gridImport
            // 
            this.gridImport.AllowUserToAddRows = false;
            this.gridImport.AllowUserToDeleteRows = false;
            this.gridImport.AllowUserToResizeRows = false;
            this.gridImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.gridImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImport.Location = new System.Drawing.Point(0, 0);
            this.gridImport.Name = "gridImport";
            this.gridImport.RowHeadersVisible = false;
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 24;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.Size = new System.Drawing.Size(755, 403);
            this.gridImport.TabIndex = 0;
            this.gridImport.TabStop = false;
            // 
            // P08_ShareExpense_ImportMaterial
            // 
            this.AcceptButton = this.btnImport;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(765, 515);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "P08_ShareExpense_ImportMaterial";
            this.Text = "Import - Material";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Button btnQuery;
        private Win.UI.TextBox txtInvoiceNo;
        private Win.UI.DateRange dateArrivePortDate;
        private Win.UI.Label labelArrivePortDate;
        private Win.UI.Panel panel4;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnImport;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridImport;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label labelInvoiceNo;
        private Win.UI.Label labelWKNo;
        private Win.UI.TextBox txtWKNo;
        private Win.UI.Label labelBLNo;
        private Win.UI.TextBox txtBLNo;
    }
}
