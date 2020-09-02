namespace Sci.Production.Quality
{
    partial class P24
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
            this.btnSave = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnImportFromBarcode = new Sci.Win.UI.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dateReceiveDate = new Sci.Win.UI.DateRange();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.btnFind = new Sci.Win.UI.Button();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.labelPackID = new Sci.Win.UI.Label();
            this.labelPONo = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.txtPONo = new Sci.Win.UI.TextBox();
            this.labReceiveDate = new Sci.Win.UI.Label();
            this.grid = new Sci.Win.UI.Grid();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label1 = new Sci.Win.UI.Label();
            this.comboDropDownList1 = new Sci.Production.Class.ComboDropDownList(this.components);
            this.btnUpdateAll = new Sci.Win.UI.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(678, 112);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 47;
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
            this.btnClose.TabIndex = 48;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.btnImportFromBarcode);
            this.panel7.Location = new System.Drawing.Point(0, 64);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(864, 42);
            this.panel7.TabIndex = 46;
            // 
            // btnImportFromBarcode
            // 
            this.btnImportFromBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportFromBarcode.Location = new System.Drawing.Point(677, 5);
            this.btnImportFromBarcode.Name = "btnImportFromBarcode";
            this.btnImportFromBarcode.Size = new System.Drawing.Size(176, 30);
            this.btnImportFromBarcode.TabIndex = 7;
            this.btnImportFromBarcode.Text = "Import From Barcode";
            this.btnImportFromBarcode.UseVisualStyleBackColor = true;
            this.btnImportFromBarcode.Click += new System.EventHandler(this.BtnImportFromBarcode_Click);
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.dateReceiveDate);
            this.panel6.Controls.Add(this.labelSPNo);
            this.panel6.Controls.Add(this.btnFind);
            this.panel6.Controls.Add(this.txtPackID);
            this.panel6.Controls.Add(this.labelPackID);
            this.panel6.Controls.Add(this.labelPONo);
            this.panel6.Controls.Add(this.txtSPNo);
            this.panel6.Controls.Add(this.txtPONo);
            this.panel6.Controls.Add(this.labReceiveDate);
            this.panel6.Location = new System.Drawing.Point(0, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(864, 63);
            this.panel6.TabIndex = 45;
            // 
            // dateReceiveDate
            // 
            // 
            // 
            // 
            this.dateReceiveDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateReceiveDate.DateBox1.Name = "";
            this.dateReceiveDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateReceiveDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateReceiveDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateReceiveDate.DateBox2.Name = "";
            this.dateReceiveDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateReceiveDate.DateBox2.TabIndex = 1;
            this.dateReceiveDate.IsSupportEditMode = false;
            this.dateReceiveDate.Location = new System.Drawing.Point(103, 34);
            this.dateReceiveDate.Name = "dateReceiveDate";
            this.dateReceiveDate.Size = new System.Drawing.Size(280, 23);
            this.dateReceiveDate.TabIndex = 119;
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
            this.labelPackID.Text = "Pack ID";
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
            // labReceiveDate
            // 
            this.labReceiveDate.Location = new System.Drawing.Point(8, 34);
            this.labReceiveDate.Name = "labReceiveDate";
            this.labReceiveDate.Size = new System.Drawing.Size(92, 23);
            this.labReceiveDate.TabIndex = 18;
            this.labReceiveDate.Text = "Receive Date";
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.DataSource = this.listControlBindingSource1;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(0, 155);
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(870, 404);
            this.grid.TabIndex = 49;
            this.grid.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 23);
            this.label1.TabIndex = 50;
            this.label1.Text = "Return to";
            // 
            // comboDropDownList1
            // 
            this.comboDropDownList1.BackColor = System.Drawing.Color.White;
            this.comboDropDownList1.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboDropDownList1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownList1.FormattingEnabled = true;
            this.comboDropDownList1.IsSupportUnselect = true;
            this.comboDropDownList1.Location = new System.Drawing.Point(104, 119);
            this.comboDropDownList1.Name = "comboDropDownList1";
            this.comboDropDownList1.OldText = "";
            this.comboDropDownList1.Size = new System.Drawing.Size(121, 24);
            this.comboDropDownList1.TabIndex = 51;
            this.comboDropDownList1.Type = "Pms_CFAReturnReason";
            // 
            // btnUpdateAll
            // 
            this.btnUpdateAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnUpdateAll.Location = new System.Drawing.Point(231, 115);
            this.btnUpdateAll.Name = "btnUpdateAll";
            this.btnUpdateAll.Size = new System.Drawing.Size(131, 30);
            this.btnUpdateAll.TabIndex = 52;
            this.btnUpdateAll.Text = "Update All Return";
            this.btnUpdateAll.UseVisualStyleBackColor = true;
            this.btnUpdateAll.Click += new System.EventHandler(this.BtnUpdateAll_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Controls.Add(this.btnUpdateAll);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.comboDropDownList1);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(870, 155);
            this.panel1.TabIndex = 53;
            // 
            // P24
            // 
            this.ClientSize = new System.Drawing.Size(870, 559);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.panel1);
            this.Name = "P24";
            this.Text = "P24. CFA Carton return Input";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnSave;
        private Win.UI.Button btnClose;
        private System.Windows.Forms.Panel panel7;
        private Win.UI.Button btnImportFromBarcode;
        private System.Windows.Forms.Panel panel6;
        private Win.UI.DateRange dateReceiveDate;
        private Win.UI.Label labelSPNo;
        private Win.UI.Button btnFind;
        private Win.UI.TextBox txtPackID;
        private Win.UI.Label labelPackID;
        private Win.UI.Label labelPONo;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.TextBox txtPONo;
        private Win.UI.Label labReceiveDate;
        private Win.UI.Grid grid;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label label1;
        private Class.ComboDropDownList comboDropDownList1;
        private Win.UI.Button btnUpdateAll;
        private System.Windows.Forms.Panel panel1;
    }
}
