namespace Sci.Production.Quality
{
    partial class P23
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dateTransferDate = new Sci.Win.UI.DateRange();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.btnFind = new Sci.Win.UI.Button();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.labelPackID = new Sci.Win.UI.Label();
            this.labelPONo = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.txtPONo = new Sci.Win.UI.TextBox();
            this.labTransDate = new Sci.Win.UI.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.numTTLCTNQty = new Sci.Win.UI.NumericBox();
            this.label2 = new Sci.Win.UI.Label();
            this.numSelectQty = new Sci.Win.UI.NumericBox();
            this.label1 = new Sci.Win.UI.Label();
            this.btnImportFromBarcode = new Sci.Win.UI.Button();
            this.grid = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnSave = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.dateTransferDate);
            this.panel6.Controls.Add(this.labelSPNo);
            this.panel6.Controls.Add(this.btnFind);
            this.panel6.Controls.Add(this.txtPackID);
            this.panel6.Controls.Add(this.labelPackID);
            this.panel6.Controls.Add(this.labelPONo);
            this.panel6.Controls.Add(this.txtSPNo);
            this.panel6.Controls.Add(this.txtPONo);
            this.panel6.Controls.Add(this.labTransDate);
            this.panel6.Location = new System.Drawing.Point(0, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(864, 63);
            this.panel6.TabIndex = 40;
            // 
            // dateTransferDate
            // 
            // 
            // 
            // 
            this.dateTransferDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateTransferDate.DateBox1.Name = "";
            this.dateTransferDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateTransferDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateTransferDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateTransferDate.DateBox2.Name = "";
            this.dateTransferDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateTransferDate.DateBox2.TabIndex = 1;
            this.dateTransferDate.IsSupportEditMode = false;
            this.dateTransferDate.Location = new System.Drawing.Point(103, 34);
            this.dateTransferDate.Name = "dateTransferDate";
            this.dateTransferDate.Size = new System.Drawing.Size(280, 23);
            this.dateTransferDate.TabIndex = 119;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(8, 5);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(92, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP#";
            // 
            // btnFind
            // 
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFind.Location = new System.Drawing.Point(773, 2);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 6;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.IsSupportEditMode = false;
            this.txtPackID.Location = new System.Drawing.Point(588, 5);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(145, 23);
            this.txtPackID.TabIndex = 5;
            // 
            // labelPackID
            // 
            this.labelPackID.Location = new System.Drawing.Point(516, 5);
            this.labelPackID.Name = "labelPackID";
            this.labelPackID.Size = new System.Drawing.Size(69, 23);
            this.labelPackID.TabIndex = 4;
            this.labelPackID.Text = "PackID";
            // 
            // labelPONo
            // 
            this.labelPONo.Location = new System.Drawing.Point(275, 5);
            this.labelPONo.Name = "labelPONo";
            this.labelPONo.Size = new System.Drawing.Size(49, 23);
            this.labelPONo.TabIndex = 2;
            this.labelPONo.Text = "PO#";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.IsSupportEditMode = false;
            this.txtSPNo.Location = new System.Drawing.Point(103, 5);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(146, 23);
            this.txtSPNo.TabIndex = 1;
            // 
            // txtPONo
            // 
            this.txtPONo.BackColor = System.Drawing.Color.White;
            this.txtPONo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPONo.IsSupportEditMode = false;
            this.txtPONo.Location = new System.Drawing.Point(327, 5);
            this.txtPONo.Name = "txtPONo";
            this.txtPONo.Size = new System.Drawing.Size(153, 23);
            this.txtPONo.TabIndex = 3;
            // 
            // labTransDate
            // 
            this.labTransDate.Location = new System.Drawing.Point(8, 34);
            this.labTransDate.Name = "labTransDate";
            this.labTransDate.Size = new System.Drawing.Size(92, 23);
            this.labTransDate.TabIndex = 18;
            this.labTransDate.Text = "Transfer Date";
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.numTTLCTNQty);
            this.panel7.Controls.Add(this.label2);
            this.panel7.Controls.Add(this.numSelectQty);
            this.panel7.Controls.Add(this.label1);
            this.panel7.Controls.Add(this.btnImportFromBarcode);
            this.panel7.Location = new System.Drawing.Point(0, 64);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(864, 42);
            this.panel7.TabIndex = 41;
            // 
            // numTTLCTNQty
            // 
            this.numTTLCTNQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTTLCTNQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTTLCTNQty.IsSupportEditMode = false;
            this.numTTLCTNQty.Location = new System.Drawing.Point(272, 10);
            this.numTTLCTNQty.Name = "numTTLCTNQty";
            this.numTTLCTNQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTTLCTNQty.ReadOnly = true;
            this.numTTLCTNQty.Size = new System.Drawing.Size(40, 23);
            this.numTTLCTNQty.TabIndex = 34;
            this.numTTLCTNQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(169, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 33;
            this.label2.Text = "Total CTN Qty:";
            // 
            // numSelectQty
            // 
            this.numSelectQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numSelectQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numSelectQty.IsSupportEditMode = false;
            this.numSelectQty.Location = new System.Drawing.Point(126, 10);
            this.numSelectQty.Name = "numSelectQty";
            this.numSelectQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSelectQty.ReadOnly = true;
            this.numSelectQty.Size = new System.Drawing.Size(40, 23);
            this.numSelectQty.TabIndex = 32;
            this.numSelectQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 23);
            this.label1.TabIndex = 31;
            this.label1.Text = "Selected CTN Qty";
            // 
            // btnImportFromBarcode
            // 
            this.btnImportFromBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportFromBarcode.Location = new System.Drawing.Point(331, 3);
            this.btnImportFromBarcode.Name = "btnImportFromBarcode";
            this.btnImportFromBarcode.Size = new System.Drawing.Size(176, 30);
            this.btnImportFromBarcode.TabIndex = 7;
            this.btnImportFromBarcode.Text = "Import From Barcode";
            this.btnImportFromBarcode.UseVisualStyleBackColor = true;
            this.btnImportFromBarcode.Click += new System.EventHandler(this.BtnImportFromBarcode_Click);
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.DataSource = this.listControlBindingSource1;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(0, 148);
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(867, 402);
            this.grid.TabIndex = 42;
            this.grid.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(678, 112);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 43;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(774, 112);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 44;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Size = new System.Drawing.Size(928, 152);
            this.shapeContainer2.TabIndex = 0;
            this.shapeContainer2.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // P23
            // 
            this.ClientSize = new System.Drawing.Size(867, 550);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel6);
            this.Name = "P23";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P23. CFA Receive Carton Input";
            this.Controls.SetChildIndex(this.panel6, 0);
            this.Controls.SetChildIndex(this.panel7, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel6;
        private Win.UI.Label labelSPNo;
        private Win.UI.Button btnFind;
        private Win.UI.TextBox txtPackID;
        private Win.UI.Label labelPackID;
        private Win.UI.Label labelPONo;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.TextBox txtPONo;
        private Win.UI.Label labTransDate;
        private System.Windows.Forms.Panel panel7;
        private Win.UI.Button btnImportFromBarcode;
        private Win.UI.Grid grid;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnClose;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.DateRange dateTransferDate;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Win.UI.NumericBox numTTLCTNQty;
        private Win.UI.Label label2;
        private Win.UI.NumericBox numSelectQty;
        private Win.UI.Label label1;
    }
}
