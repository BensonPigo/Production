namespace Sci.Production.Logistic
{
    partial class P08
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkUpdateOriLocation = new System.Windows.Forms.CheckBox();
            this.txtcloglocationLocationNo = new Sci.Production.Class.Txtcloglocation();
            this.labelLocationNo = new Sci.Win.UI.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dateTransferDate = new Sci.Win.UI.DateRange();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.btnFind = new Sci.Win.UI.Button();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.labelPackID = new Sci.Win.UI.Label();
            this.labelPONo = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.txtPONo = new Sci.Win.UI.TextBox();
            this.labReceiveDate = new Sci.Win.UI.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnImportFromBarcode = new Sci.Win.UI.Button();
            this.btnUpdateAll = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.grid = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkUpdateOriLocation);
            this.panel1.Controls.Add(this.txtcloglocationLocationNo);
            this.panel1.Controls.Add(this.labelLocationNo);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Controls.Add(this.btnUpdateAll);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(839, 158);
            this.panel1.TabIndex = 54;
            // 
            // chkUpdateOriLocation
            // 
            this.chkUpdateOriLocation.AutoSize = true;
            this.chkUpdateOriLocation.ForeColor = System.Drawing.Color.Red;
            this.chkUpdateOriLocation.Location = new System.Drawing.Point(417, 122);
            this.chkUpdateOriLocation.Name = "chkUpdateOriLocation";
            this.chkUpdateOriLocation.Size = new System.Drawing.Size(205, 21);
            this.chkUpdateOriLocation.TabIndex = 139;
            this.chkUpdateOriLocation.Text = "Update to original locationID";
            this.chkUpdateOriLocation.UseVisualStyleBackColor = true;
            this.chkUpdateOriLocation.CheckedChanged += new System.EventHandler(this.ChkUpdateOriLocation_CheckedChanged);
            // 
            // txtcloglocationLocationNo
            // 
            this.txtcloglocationLocationNo.BackColor = System.Drawing.Color.White;
            this.txtcloglocationLocationNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcloglocationLocationNo.IsSupportSytsemContextMenu = false;
            this.txtcloglocationLocationNo.Location = new System.Drawing.Point(104, 122);
            this.txtcloglocationLocationNo.MDivisionObjectName = null;
            this.txtcloglocationLocationNo.Name = "txtcloglocationLocationNo";
            this.txtcloglocationLocationNo.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.NonReadOnly;
            this.txtcloglocationLocationNo.Size = new System.Drawing.Size(121, 23);
            this.txtcloglocationLocationNo.TabIndex = 54;
            // 
            // labelLocationNo
            // 
            this.labelLocationNo.Location = new System.Drawing.Point(9, 122);
            this.labelLocationNo.Name = "labelLocationNo";
            this.labelLocationNo.Size = new System.Drawing.Size(91, 23);
            this.labelLocationNo.TabIndex = 53;
            this.labelLocationNo.Text = "Location No";
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
            this.panel6.Controls.Add(this.labReceiveDate);
            this.panel6.Location = new System.Drawing.Point(0, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(831, 63);
            this.panel6.TabIndex = 45;
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
            this.btnFind.Location = new System.Drawing.Point(739, 5);
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
            this.labReceiveDate.Text = "Transfer Date";
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.btnImportFromBarcode);
            this.panel7.Location = new System.Drawing.Point(0, 64);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(831, 42);
            this.panel7.TabIndex = 46;
            // 
            // btnImportFromBarcode
            // 
            this.btnImportFromBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportFromBarcode.Location = new System.Drawing.Point(643, 5);
            this.btnImportFromBarcode.Name = "btnImportFromBarcode";
            this.btnImportFromBarcode.Size = new System.Drawing.Size(176, 30);
            this.btnImportFromBarcode.TabIndex = 7;
            this.btnImportFromBarcode.Text = "Import From Barcode";
            this.btnImportFromBarcode.UseVisualStyleBackColor = true;
            this.btnImportFromBarcode.Click += new System.EventHandler(this.BtnImportFromBarcode_Click);
            // 
            // btnUpdateAll
            // 
            this.btnUpdateAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnUpdateAll.Location = new System.Drawing.Point(231, 119);
            this.btnUpdateAll.Name = "btnUpdateAll";
            this.btnUpdateAll.Size = new System.Drawing.Size(160, 30);
            this.btnUpdateAll.TabIndex = 52;
            this.btnUpdateAll.Text = "Update All Location";
            this.btnUpdateAll.UseVisualStyleBackColor = true;
            this.btnUpdateAll.Click += new System.EventHandler(this.BtnUpdateAll_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(740, 115);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 48;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(644, 115);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 47;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
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
            this.grid.Location = new System.Drawing.Point(0, 158);
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(839, 392);
            this.grid.TabIndex = 55;
            this.grid.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Size = new System.Drawing.Size(877, 152);
            this.shapeContainer2.TabIndex = 0;
            this.shapeContainer2.TabStop = false;
            // 
            // P08
            // 
            this.ClientSize = new System.Drawing.Size(839, 550);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P08";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P08.Clog Receive CFA Carton Input";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel6;
        private Win.UI.DateRange dateTransferDate;
        private Win.UI.Label labelSPNo;
        private Win.UI.Button btnFind;
        private Win.UI.TextBox txtPackID;
        private Win.UI.Label labelPackID;
        private Win.UI.Label labelPONo;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.TextBox txtPONo;
        private Win.UI.Label labReceiveDate;
        private System.Windows.Forms.Panel panel7;
        private Win.UI.Button btnImportFromBarcode;
        private Win.UI.Button btnUpdateAll;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave;
        private Win.UI.Grid grid;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Class.Txtcloglocation txtcloglocationLocationNo;
        private Win.UI.Label labelLocationNo;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private System.Windows.Forms.CheckBox chkUpdateOriLocation;
    }
}
