namespace Sci.Production.Logistic
{
    partial class P02_ImportFromBarCode
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
            this.gridFileList = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnImportData = new Sci.Win.UI.Button();
            this.btnDelete = new Sci.Win.UI.Button();
            this.btnAppend = new Sci.Win.UI.Button();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel2 = new Sci.Win.UI.Panel();
            this.gridLocationNo = new Sci.Win.UI.Grid();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.labelTotalCartonScanned = new Sci.Win.UI.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.numTotalCartonScanned = new Sci.Win.UI.NumericBox();
            this.labelFileList = new Sci.Win.UI.Label();
            this.dateReceiveDate = new Sci.Win.UI.DateBox();
            this.labelReceiveDate = new Sci.Win.UI.Label();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.panel5 = new Sci.Win.UI.Panel();
            this.panel6 = new Sci.Win.UI.Panel();
            this.panel7 = new Sci.Win.UI.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFileList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLocationNo)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridFileList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(7, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(698, 101);
            this.panel1.TabIndex = 0;
            // 
            // gridFileList
            // 
            this.gridFileList.AllowUserToAddRows = false;
            this.gridFileList.AllowUserToDeleteRows = false;
            this.gridFileList.AllowUserToResizeRows = false;
            this.gridFileList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridFileList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridFileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFileList.DataSource = this.listControlBindingSource1;
            this.gridFileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFileList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridFileList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridFileList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridFileList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridFileList.Location = new System.Drawing.Point(0, 0);
            this.gridFileList.Name = "gridFileList";
            this.gridFileList.RowHeadersVisible = false;
            this.gridFileList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridFileList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridFileList.RowTemplate.Height = 24;
            this.gridFileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFileList.Size = new System.Drawing.Size(698, 101);
            this.gridFileList.TabIndex = 0;
            this.gridFileList.TabStop = false;
            // 
            // btnImportData
            // 
            this.btnImportData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportData.Location = new System.Drawing.Point(585, 4);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(105, 30);
            this.btnImportData.TabIndex = 3;
            this.btnImportData.Text = "Import Data";
            this.btnImportData.UseVisualStyleBackColor = true;
            this.btnImportData.Click += new System.EventHandler(this.BtnImportData_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnDelete.Location = new System.Drawing.Point(94, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 30);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnAppend
            // 
            this.btnAppend.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnAppend.Location = new System.Drawing.Point(7, 4);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(80, 30);
            this.btnAppend.TabIndex = 1;
            this.btnAppend.Text = "Append";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.BtnAppend_Click);
            // 
            // lineShape2
            // 
            this.lineShape2.Name = "lineShape2";
            this.lineShape2.X1 = 1;
            this.lineShape2.X2 = 704;
            this.lineShape2.Y1 = 37;
            this.lineShape2.Y2 = 37;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridLocationNo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(7, 204);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(698, 236);
            this.panel2.TabIndex = 1;
            // 
            // gridLocationNo
            // 
            this.gridLocationNo.AllowUserToAddRows = false;
            this.gridLocationNo.AllowUserToDeleteRows = false;
            this.gridLocationNo.AllowUserToResizeRows = false;
            this.gridLocationNo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridLocationNo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridLocationNo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLocationNo.DataSource = this.listControlBindingSource2;
            this.gridLocationNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridLocationNo.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridLocationNo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridLocationNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridLocationNo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridLocationNo.Location = new System.Drawing.Point(0, 0);
            this.gridLocationNo.Name = "gridLocationNo";
            this.gridLocationNo.RowHeadersVisible = false;
            this.gridLocationNo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridLocationNo.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridLocationNo.RowTemplate.Height = 24;
            this.gridLocationNo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLocationNo.Size = new System.Drawing.Size(698, 236);
            this.gridLocationNo.TabIndex = 2;
            this.gridLocationNo.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(613, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(519, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnToExcel.Location = new System.Drawing.Point(350, 3);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 5;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // labelTotalCartonScanned
            // 
            this.labelTotalCartonScanned.Lines = 0;
            this.labelTotalCartonScanned.Location = new System.Drawing.Point(7, 7);
            this.labelTotalCartonScanned.Name = "labelTotalCartonScanned";
            this.labelTotalCartonScanned.Size = new System.Drawing.Size(137, 23);
            this.labelTotalCartonScanned.TabIndex = 3;
            this.labelTotalCartonScanned.Text = "Total Carton Scanned";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // numTotalCartonScanned
            // 
            this.numTotalCartonScanned.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalCartonScanned.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalCartonScanned.IsSupportEditMode = false;
            this.numTotalCartonScanned.Location = new System.Drawing.Point(148, 7);
            this.numTotalCartonScanned.Name = "numTotalCartonScanned";
            this.numTotalCartonScanned.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalCartonScanned.ReadOnly = true;
            this.numTotalCartonScanned.Size = new System.Drawing.Size(100, 23);
            this.numTotalCartonScanned.TabIndex = 8;
            this.numTotalCartonScanned.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelFileList
            // 
            this.labelFileList.Lines = 0;
            this.labelFileList.Location = new System.Drawing.Point(4, 4);
            this.labelFileList.Name = "labelFileList";
            this.labelFileList.Size = new System.Drawing.Size(56, 23);
            this.labelFileList.TabIndex = 5;
            this.labelFileList.Text = "File List";
            // 
            // dateReceiveDate
            // 
            this.dateReceiveDate.Location = new System.Drawing.Point(99, 43);
            this.dateReceiveDate.Name = "dateReceiveDate";
            this.dateReceiveDate.Size = new System.Drawing.Size(130, 23);
            this.dateReceiveDate.TabIndex = 3;
            // 
            // labelReceiveDate
            // 
            this.labelReceiveDate.Lines = 0;
            this.labelReceiveDate.Location = new System.Drawing.Point(7, 43);
            this.labelReceiveDate.Name = "labelReceiveDate";
            this.labelReceiveDate.Size = new System.Drawing.Size(89, 23);
            this.labelReceiveDate.TabIndex = 2;
            this.labelReceiveDate.Text = "Receive Date";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelTotalCartonScanned);
            this.panel3.Controls.Add(this.numTotalCartonScanned);
            this.panel3.Controls.Add(this.btnToExcel);
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(7, 440);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(698, 39);
            this.panel3.TabIndex = 9;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.labelReceiveDate);
            this.panel4.Controls.Add(this.dateReceiveDate);
            this.panel4.Controls.Add(this.btnAppend);
            this.panel4.Controls.Add(this.btnDelete);
            this.panel4.Controls.Add(this.btnImportData);
            this.panel4.Controls.Add(this.shapeContainer1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(7, 131);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(698, 73);
            this.panel4.TabIndex = 10;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape2});
            this.shapeContainer1.Size = new System.Drawing.Size(698, 73);
            this.shapeContainer1.TabIndex = 4;
            this.shapeContainer1.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.labelFileList);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(7, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(698, 30);
            this.panel5.TabIndex = 11;
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(7, 479);
            this.panel6.TabIndex = 6;
            // 
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(705, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(7, 479);
            this.panel7.TabIndex = 12;
            // 
            // P02_ImportFromBarCode
            // 
            this.ClientSize = new System.Drawing.Size(712, 479);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel7);
            this.Name = "P02_ImportFromBarCode";
            this.Text = "Import From Bar Code";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFileList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridLocationNo)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Button btnImportData;
        private Win.UI.Button btnDelete;
        private Win.UI.Button btnAppend;
        private Win.UI.Grid gridFileList;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnToExcel;
        private Win.UI.Label labelTotalCartonScanned;
        private Win.UI.Grid gridLocationNo;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Win.UI.NumericBox numTotalCartonScanned;
        private Win.UI.Label labelFileList;
        private Win.UI.DateBox dateReceiveDate;
        private Win.UI.Label labelReceiveDate;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Win.UI.Panel panel5;
        private Win.UI.Panel panel6;
        private Win.UI.Panel panel7;
    }
}
