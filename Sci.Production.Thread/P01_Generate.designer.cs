namespace Sci.Production.Thread
{
    partial class P01_Generate
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
            this.txtMachineType = new Sci.Win.UI.TextBox();
            this.btnFilter = new Sci.Win.UI.Button();
            this.panel2 = new Sci.Win.UI.Panel();
            this.displayBoxEdit = new Sci.Win.UI.DisplayBox();
            this.label1 = new Sci.Win.UI.Label();
            this.btnBatchUpdate = new Sci.Win.UI.Button();
            this.labelThreadCombination = new Sci.Win.UI.Label();
            this.txtthreadcomb = new Sci.Production.Class.Txtthreadcomb();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnGenerate = new Sci.Win.UI.Button();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1 = new Sci.Win.UI.Panel();
            this.cmbDescription = new Sci.Win.UI.ComboBox();
            this.checkOnlyShowNotYetAssignCombination = new Sci.Win.UI.CheckBox();
            this.labelMachineType = new Sci.Win.UI.Label();
            this.lblanguage = new Sci.Win.UI.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMachineType
            // 
            this.txtMachineType.BackColor = System.Drawing.Color.White;
            this.txtMachineType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMachineType.Location = new System.Drawing.Point(165, 17);
            this.txtMachineType.Name = "txtMachineType";
            this.txtMachineType.Size = new System.Drawing.Size(150, 23);
            this.txtMachineType.TabIndex = 18;
            this.txtMachineType.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtMachineType_PopUp);
            this.txtMachineType.Validating += new System.ComponentModel.CancelEventHandler(this.TxtMachineType_Validating);
            // 
            // btnFilter
            // 
            this.btnFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilter.Location = new System.Drawing.Point(856, 13);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(80, 30);
            this.btnFilter.TabIndex = 2;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.BtnFilter_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.displayBoxEdit);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnBatchUpdate);
            this.panel2.Controls.Add(this.labelThreadCombination);
            this.panel2.Controls.Add(this.txtthreadcomb);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.btnGenerate);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 430);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(946, 77);
            this.panel2.TabIndex = 11;
            // 
            // displayBoxEdit
            // 
            this.displayBoxEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxEdit.Location = new System.Drawing.Point(81, 40);
            this.displayBoxEdit.Name = "displayBoxEdit";
            this.displayBoxEdit.Size = new System.Drawing.Size(309, 23);
            this.displayBoxEdit.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 23);
            this.label1.TabIndex = 20;
            this.label1.Text = "Edit By:";
            // 
            // btnBatchUpdate
            // 
            this.btnBatchUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBatchUpdate.Location = new System.Drawing.Point(856, 6);
            this.btnBatchUpdate.Name = "btnBatchUpdate";
            this.btnBatchUpdate.Size = new System.Drawing.Size(80, 30);
            this.btnBatchUpdate.TabIndex = 16;
            this.btnBatchUpdate.Text = "Batch update";
            this.btnBatchUpdate.UseVisualStyleBackColor = true;
            this.btnBatchUpdate.Click += new System.EventHandler(this.BtnBatchUpdate_Click);
            // 
            // labelThreadCombination
            // 
            this.labelThreadCombination.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelThreadCombination.Location = new System.Drawing.Point(624, 12);
            this.labelThreadCombination.Name = "labelThreadCombination";
            this.labelThreadCombination.Size = new System.Drawing.Size(133, 23);
            this.labelThreadCombination.TabIndex = 15;
            this.labelThreadCombination.Text = "Thread Combination";
            // 
            // txtthreadcomb
            // 
            this.txtthreadcomb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtthreadcomb.BackColor = System.Drawing.Color.White;
            this.txtthreadcomb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtthreadcomb.IsSupportSytsemContextMenu = false;
            this.txtthreadcomb.Location = new System.Drawing.Point(760, 10);
            this.txtthreadcomb.Name = "txtthreadcomb";
            this.txtthreadcomb.Size = new System.Drawing.Size(90, 23);
            this.txtthreadcomb.TabIndex = 9;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(856, 40);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(770, 40);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(80, 30);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
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
            this.gridDetail.Location = new System.Drawing.Point(0, 58);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(946, 372);
            this.gridDetail.TabIndex = 0;
            this.gridDetail.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblanguage);
            this.panel1.Controls.Add(this.cmbDescription);
            this.panel1.Controls.Add(this.txtMachineType);
            this.panel1.Controls.Add(this.checkOnlyShowNotYetAssignCombination);
            this.panel1.Controls.Add(this.labelMachineType);
            this.panel1.Controls.Add(this.btnFilter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(946, 58);
            this.panel1.TabIndex = 10;
            // 
            // cmbDescription
            // 
            this.cmbDescription.BackColor = System.Drawing.Color.White;
            this.cmbDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbDescription.FormattingEnabled = true;
            this.cmbDescription.IsSupportUnselect = true;
            this.cmbDescription.Items.AddRange(new object[] {
            "English",
            "Chinese",
            "Vietnamese",
            "Cambodian"});
            this.cmbDescription.Location = new System.Drawing.Point(695, 16);
            this.cmbDescription.Name = "cmbDescription";
            this.cmbDescription.OldText = "";
            this.cmbDescription.Size = new System.Drawing.Size(121, 24);
            this.cmbDescription.TabIndex = 19;
            this.cmbDescription.SelectedIndexChanged += new System.EventHandler(this.cmbDescription_SelectedIndexChanged);
            // 
            // checkOnlyShowNotYetAssignCombination
            // 
            this.checkOnlyShowNotYetAssignCombination.AutoSize = true;
            this.checkOnlyShowNotYetAssignCombination.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkOnlyShowNotYetAssignCombination.Location = new System.Drawing.Point(356, 19);
            this.checkOnlyShowNotYetAssignCombination.Name = "checkOnlyShowNotYetAssignCombination";
            this.checkOnlyShowNotYetAssignCombination.Size = new System.Drawing.Size(264, 21);
            this.checkOnlyShowNotYetAssignCombination.TabIndex = 14;
            this.checkOnlyShowNotYetAssignCombination.Text = "Only show not yet assign combination";
            this.checkOnlyShowNotYetAssignCombination.UseVisualStyleBackColor = true;
            // 
            // labelMachineType
            // 
            this.labelMachineType.Location = new System.Drawing.Point(57, 17);
            this.labelMachineType.Name = "labelMachineType";
            this.labelMachineType.Size = new System.Drawing.Size(95, 23);
            this.labelMachineType.TabIndex = 13;
            this.labelMachineType.Text = "Machine Type";
            // 
            // lblanguage
            // 
            this.lblanguage.Location = new System.Drawing.Point(617, 16);
            this.lblanguage.Name = "lblanguage";
            this.lblanguage.Size = new System.Drawing.Size(75, 23);
            this.lblanguage.TabIndex = 20;
            this.lblanguage.Text = "language";
            // 
            // P01_Generate
            // 
            this.ClientSize = new System.Drawing.Size(946, 507);
            this.Controls.Add(this.gridDetail);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "P01_Generate";
            this.Text = "Generate  Thread Combination";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnFilter;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnGenerate;
        private Win.UI.Grid gridDetail;
        private Win.UI.Panel panel1;
        private Win.UI.CheckBox checkOnlyShowNotYetAssignCombination;
        private Win.UI.Label labelMachineType;
        private Win.UI.Label labelThreadCombination;
        private Class.Txtthreadcomb txtthreadcomb;
        private Win.UI.Button btnBatchUpdate;
        private Win.UI.TextBox txtMachineType;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.DisplayBox displayBoxEdit;
        private Win.UI.Label label1;
        private Win.UI.ComboBox cmbDescription;
        private Win.UI.Label lblanguage;
    }
}
