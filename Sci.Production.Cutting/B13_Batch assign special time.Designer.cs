namespace Sci.Production.Cutting
{
    partial class B13_BatchAssignSpecialTime
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
            this.btnDelete = new Sci.Win.UI.Button();
            this.btnAdd = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnReset = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.grid1 = new Sci.Win.UI.Grid();
            this.grid1bs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.gridMachine = new Sci.Win.UI.Grid();
            this.gridMachinebs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label1 = new Sci.Win.UI.Label();
            this.comboDropDownList1 = new Sci.Production.Class.ComboDropDownList(this.components);
            this.label4 = new Sci.Win.UI.Label();
            this.dateWorking = new Sci.Win.UI.DateBox();
            this.checkBoxIsSpecialTime = new Sci.Win.UI.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMachine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMachinebs)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(499, 104);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(35, 35);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "-";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Enabled = false;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.btnAdd.Location = new System.Drawing.Point(499, 63);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(35, 35);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "+";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnClose.Location = new System.Drawing.Point(499, 407);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(99, 30);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnReset.Location = new System.Drawing.Point(499, 371);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(99, 30);
            this.btnReset.TabIndex = 8;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnSave.Location = new System.Drawing.Point(499, 335);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(99, 30);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.grid1bs;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(222, 63);
            this.grid1.Name = "grid1";
            this.grid1.RowHeadersVisible = false;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(271, 374);
            this.grid1.TabIndex = 4;
            // 
            // gridMachine
            // 
            this.gridMachine.AllowUserToAddRows = false;
            this.gridMachine.AllowUserToDeleteRows = false;
            this.gridMachine.AllowUserToResizeRows = false;
            this.gridMachine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gridMachine.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridMachine.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridMachine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMachine.DataSource = this.gridMachinebs;
            this.gridMachine.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridMachine.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridMachine.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridMachine.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridMachine.Location = new System.Drawing.Point(17, 63);
            this.gridMachine.Name = "gridMachine";
            this.gridMachine.RowHeadersVisible = false;
            this.gridMachine.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridMachine.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridMachine.RowTemplate.Height = 24;
            this.gridMachine.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMachine.ShowCellToolTips = false;
            this.gridMachine.Size = new System.Drawing.Size(199, 374);
            this.gridMachine.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(17, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 23);
            this.label1.TabIndex = 30;
            this.label1.Text = "Machine Type";
            // 
            // comboDropDownList1
            // 
            this.comboDropDownList1.AddAllItem = false;
            this.comboDropDownList1.BackColor = System.Drawing.Color.White;
            this.comboDropDownList1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownList1.FormattingEnabled = true;
            this.comboDropDownList1.IsSupportUnselect = true;
            this.comboDropDownList1.Location = new System.Drawing.Point(112, 8);
            this.comboDropDownList1.Name = "comboDropDownList1";
            this.comboDropDownList1.OldText = "";
            this.comboDropDownList1.Size = new System.Drawing.Size(121, 24);
            this.comboDropDownList1.TabIndex = 0;
            this.comboDropDownList1.Type = "PMS_CutMachIoTType";
            this.comboDropDownList1.SelectedIndexChanged += new System.EventHandler(this.ComboDropDownList1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(17, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 23);
            this.label4.TabIndex = 60;
            this.label4.Text = "Working Date";
            // 
            // dateWorking
            // 
            this.dateWorking.Location = new System.Drawing.Point(112, 35);
            this.dateWorking.Name = "dateWorking";
            this.dateWorking.Size = new System.Drawing.Size(130, 23);
            this.dateWorking.TabIndex = 1;
            // 
            // checkBoxIsSpecialTime
            // 
            this.checkBoxIsSpecialTime.AutoSize = true;
            this.checkBoxIsSpecialTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxIsSpecialTime.Location = new System.Drawing.Point(257, 8);
            this.checkBoxIsSpecialTime.Name = "checkBoxIsSpecialTime";
            this.checkBoxIsSpecialTime.Size = new System.Drawing.Size(122, 21);
            this.checkBoxIsSpecialTime.TabIndex = 2;
            this.checkBoxIsSpecialTime.Text = "Is Special Time";
            this.checkBoxIsSpecialTime.UseVisualStyleBackColor = true;
            this.checkBoxIsSpecialTime.CheckedChanged += new System.EventHandler(this.CheckBoxIsSpecialTime_CheckedChanged);
            // 
            // B13_BatchAssignSpecialTime
            // 
            this.ClientSize = new System.Drawing.Size(611, 449);
            this.Controls.Add(this.checkBoxIsSpecialTime);
            this.Controls.Add(this.dateWorking);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboDropDownList1);
            this.Controls.Add(this.gridMachine);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grid1);
            this.DefaultControl = "txtDescription";
            this.EditMode = true;
            this.Name = "B13_BatchAssignSpecialTime";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "B13_Batch assign special time";
            this.Controls.SetChildIndex(this.grid1, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnReset, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnAdd, 0);
            this.Controls.SetChildIndex(this.btnDelete, 0);
            this.Controls.SetChildIndex(this.gridMachine, 0);
            this.Controls.SetChildIndex(this.comboDropDownList1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.dateWorking, 0);
            this.Controls.SetChildIndex(this.checkBoxIsSpecialTime, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMachine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMachinebs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button btnDelete;
        private Win.UI.Button btnAdd;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnReset;
        private Win.UI.Button btnSave;
        private Win.UI.Grid grid1;
        private Win.UI.ListControlBindingSource grid1bs;
        private Win.UI.Grid gridMachine;
        private Win.UI.Label label1;
        private Class.ComboDropDownList comboDropDownList1;
        private Win.UI.Label label4;
        private Win.UI.DateBox dateWorking;
        private Win.UI.CheckBox checkBoxIsSpecialTime;
        private Win.UI.ListControlBindingSource gridMachinebs;
    }
}
