namespace Sci.Production.IE
{
    partial class P01_SelectOperationCode
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
            this.btnFind = new Sci.Win.UI.Button();
            this.txtDescription = new Sci.Win.UI.TextBox();
            this.numSeamLength = new Sci.Win.UI.NumericBox();
            this.labelSeamLength = new Sci.Win.UI.Label();
            this.txtMachineCode = new Sci.Win.UI.TextBox();
            this.labelMachineCode = new Sci.Win.UI.Label();
            this.numSMV = new Sci.Win.UI.NumericBox();
            this.labelSMV = new Sci.Win.UI.Label();
            this.txtID = new Sci.Win.UI.TextBox();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelID = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.numCount = new Sci.Win.UI.NumericBox();
            this.labelCount = new Sci.Win.UI.Label();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnSelect = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label1 = new Sci.Win.UI.Label();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 570);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(808, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 570);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.btnFind);
            this.panel3.Controls.Add(this.txtDescription);
            this.panel3.Controls.Add(this.numSeamLength);
            this.panel3.Controls.Add(this.labelSeamLength);
            this.panel3.Controls.Add(this.txtMachineCode);
            this.panel3.Controls.Add(this.labelMachineCode);
            this.panel3.Controls.Add(this.numSMV);
            this.panel3.Controls.Add(this.labelSMV);
            this.panel3.Controls.Add(this.txtID);
            this.panel3.Controls.Add(this.labelDescription);
            this.panel3.Controls.Add(this.labelID);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(798, 79);
            this.panel3.TabIndex = 2;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(695, 37);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 5;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDescription.Location = new System.Drawing.Point(81, 41);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(339, 23);
            this.txtDescription.TabIndex = 1;
            // 
            // numSeamLength
            // 
            this.numSeamLength.BackColor = System.Drawing.Color.White;
            this.numSeamLength.DecimalPlaces = 2;
            this.numSeamLength.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numSeamLength.Location = new System.Drawing.Point(687, 8);
            this.numSeamLength.Name = "numSeamLength";
            this.numSeamLength.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSeamLength.Size = new System.Drawing.Size(88, 23);
            this.numSeamLength.TabIndex = 4;
            this.numSeamLength.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelSeamLength
            // 
            this.labelSeamLength.Location = new System.Drawing.Point(595, 8);
            this.labelSeamLength.Name = "labelSeamLength";
            this.labelSeamLength.Size = new System.Drawing.Size(88, 23);
            this.labelSeamLength.TabIndex = 7;
            this.labelSeamLength.Text = "Seam Length";
            // 
            // txtMachineCode
            // 
            this.txtMachineCode.BackColor = System.Drawing.Color.White;
            this.txtMachineCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMachineCode.Location = new System.Drawing.Point(472, 9);
            this.txtMachineCode.Name = "txtMachineCode";
            this.txtMachineCode.Size = new System.Drawing.Size(92, 23);
            this.txtMachineCode.TabIndex = 3;
            // 
            // labelMachineCode
            // 
            this.labelMachineCode.Location = new System.Drawing.Point(374, 9);
            this.labelMachineCode.Name = "labelMachineCode";
            this.labelMachineCode.Size = new System.Drawing.Size(94, 23);
            this.labelMachineCode.TabIndex = 5;
            this.labelMachineCode.Text = "Machine Code";
            // 
            // numSMV
            // 
            this.numSMV.BackColor = System.Drawing.Color.White;
            this.numSMV.DecimalPlaces = 4;
            this.numSMV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numSMV.Location = new System.Drawing.Point(276, 9);
            this.numSMV.Name = "numSMV";
            this.numSMV.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSMV.Size = new System.Drawing.Size(68, 23);
            this.numSMV.TabIndex = 2;
            this.numSMV.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelSMV
            // 
            this.labelSMV.Location = new System.Drawing.Point(236, 9);
            this.labelSMV.Name = "labelSMV";
            this.labelSMV.Size = new System.Drawing.Size(36, 23);
            this.labelSMV.TabIndex = 3;
            this.labelSMV.Text = "SMV";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtID.Location = new System.Drawing.Point(34, 9);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(180, 23);
            this.txtID.TabIndex = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(3, 41);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(74, 23);
            this.labelDescription.TabIndex = 1;
            this.labelDescription.Text = "Description";
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(4, 9);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(26, 23);
            this.labelID.TabIndex = 0;
            this.labelID.Text = "ID";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.numCount);
            this.panel4.Controls.Add(this.labelCount);
            this.panel4.Controls.Add(this.btnCancel);
            this.panel4.Controls.Add(this.btnSelect);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 525);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(798, 45);
            this.panel4.TabIndex = 3;
            // 
            // numCount
            // 
            this.numCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numCount.IsSupportEditMode = false;
            this.numCount.Location = new System.Drawing.Point(165, 9);
            this.numCount.Name = "numCount";
            this.numCount.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCount.ReadOnly = true;
            this.numCount.Size = new System.Drawing.Size(81, 23);
            this.numCount.TabIndex = 0;
            this.numCount.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelCount
            // 
            this.labelCount.Location = new System.Drawing.Point(117, 9);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(44, 23);
            this.labelCount.TabIndex = 2;
            this.labelCount.Text = "Count";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(687, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(585, 8);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(80, 30);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridDetail);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 79);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(798, 446);
            this.panel5.TabIndex = 4;
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.DataSource = this.listControlBindingSource1;
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(0, 0);
            this.gridDetail.MultiSelect = false;
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(798, 446);
            this.gridDetail.TabIndex = 0;
            this.gridDetail.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(423, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(269, 23);
            this.label1.TabIndex = 9;
            this.label1.Text = "Use \';\' to separate desc keyword";
            // 
            // P01_SelectOperationCode
            // 
            this.AcceptButton = this.btnSelect;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(818, 570);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "txtID";
            this.Name = "P01_SelectOperationCode";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Select Operation Code";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelID;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Button btnFind;
        private Win.UI.TextBox txtDescription;
        private Win.UI.NumericBox numSeamLength;
        private Win.UI.Label labelSeamLength;
        private Win.UI.TextBox txtMachineCode;
        private Win.UI.Label labelMachineCode;
        private Win.UI.NumericBox numSMV;
        private Win.UI.Label labelSMV;
        private Win.UI.TextBox txtID;
        private Win.UI.NumericBox numCount;
        private Win.UI.Label labelCount;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnSelect;
        private Win.UI.Grid gridDetail;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label label1;
    }
}
