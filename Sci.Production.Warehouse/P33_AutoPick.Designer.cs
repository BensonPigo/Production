namespace Sci.Production.Warehouse
{
    partial class P33_AutoPick
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
            this.gridAutoPick = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnPick = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.label1 = new Sci.Win.UI.Label();
            this.btnAutoCacu = new Sci.Win.UI.Button();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.chkQuiting = new Sci.Win.UI.CheckBox();
            this.groupBox3 = new Sci.Win.UI.GroupBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txtMachineType = new Sci.Win.UI.TextBox();
            this.chkSewingType = new Sci.Win.UI.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridAutoPick)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridAutoPick);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(13, 119);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(982, 439);
            this.panel1.TabIndex = 20;
            // 
            // gridAutoPick
            // 
            this.gridAutoPick.AllowUserToAddRows = false;
            this.gridAutoPick.AllowUserToDeleteRows = false;
            this.gridAutoPick.AllowUserToResizeRows = false;
            this.gridAutoPick.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridAutoPick.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridAutoPick.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAutoPick.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridAutoPick.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridAutoPick.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridAutoPick.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridAutoPick.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridAutoPick.Location = new System.Drawing.Point(0, 0);
            this.gridAutoPick.Name = "gridAutoPick";
            this.gridAutoPick.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridAutoPick.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridAutoPick.RowTemplate.Height = 24;
            this.gridAutoPick.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAutoPick.ShowCellToolTips = false;
            this.gridAutoPick.Size = new System.Drawing.Size(982, 439);
            this.gridAutoPick.TabIndex = 1;
            this.gridAutoPick.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(13, 613);
            this.panel2.TabIndex = 21;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(995, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(13, 613);
            this.panel3.TabIndex = 22;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnCancel);
            this.panel4.Controls.Add(this.btnPick);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(13, 558);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(982, 55);
            this.panel4.TabIndex = 23;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(885, 13);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnPick
            // 
            this.btnPick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPick.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnPick.Location = new System.Drawing.Point(773, 13);
            this.btnPick.Name = "btnPick";
            this.btnPick.Size = new System.Drawing.Size(80, 30);
            this.btnPick.TabIndex = 15;
            this.btnPick.Text = "Pick";
            this.btnPick.UseVisualStyleBackColor = true;
            this.btnPick.Click += new System.EventHandler(this.BtnPick_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.groupBox1);
            this.panel5.Controls.Add(this.btnAutoCacu);
            this.panel5.Controls.Add(this.groupBox2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(13, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(982, 119);
            this.panel5.TabIndex = 24;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(16, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(152, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label1.Location = new System.Drawing.Point(17, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "Issue Type";
            // 
            // btnAutoCacu
            // 
            this.btnAutoCacu.Location = new System.Drawing.Point(829, 18);
            this.btnAutoCacu.Name = "btnAutoCacu";
            this.btnAutoCacu.Size = new System.Drawing.Size(136, 30);
            this.btnAutoCacu.TabIndex = 2;
            this.btnAutoCacu.Text = "Auto Calculate";
            this.btnAutoCacu.UseVisualStyleBackColor = true;
            this.btnAutoCacu.Click += new System.EventHandler(this.BtnAutoCacu_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkQuiting);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(158, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(665, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // chkQuiting
            // 
            this.chkQuiting.AutoSize = true;
            this.chkQuiting.Checked = true;
            this.chkQuiting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkQuiting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkQuiting.Location = new System.Drawing.Point(16, 65);
            this.chkQuiting.Name = "chkQuiting";
            this.chkQuiting.Size = new System.Drawing.Size(72, 21);
            this.chkQuiting.TabIndex = 1;
            this.chkQuiting.Text = "Quiting";
            this.chkQuiting.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtMachineType);
            this.groupBox3.Controls.Add(this.chkSewingType);
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(665, 57);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(134, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Machine Type";
            // 
            // txtMachineType
            // 
            this.txtMachineType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtMachineType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtMachineType.Location = new System.Drawing.Point(235, 22);
            this.txtMachineType.Name = "txtMachineType";
            this.txtMachineType.ReadOnly = true;
            this.txtMachineType.Size = new System.Drawing.Size(424, 23);
            this.txtMachineType.TabIndex = 1;
            this.txtMachineType.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtMachineType_PopUp);
            this.txtMachineType.Validating += new System.ComponentModel.CancelEventHandler(this.TxtMachineType_Validating);
            // 
            // chkSewingType
            // 
            this.chkSewingType.AutoSize = true;
            this.chkSewingType.Checked = true;
            this.chkSewingType.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSewingType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSewingType.Location = new System.Drawing.Point(16, 22);
            this.chkSewingType.Name = "chkSewingType";
            this.chkSewingType.Size = new System.Drawing.Size(108, 21);
            this.chkSewingType.TabIndex = 0;
            this.chkSewingType.Text = "Sewing Type";
            this.chkSewingType.UseVisualStyleBackColor = true;
            this.chkSewingType.CheckedChanged += new System.EventHandler(this.ChkSewingType_CheckedChanged);
            // 
            // P33_AutoPick
            // 
            this.ClientSize = new System.Drawing.Size(1008, 613);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Name = "P33_AutoPick";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P33. Auto Pick";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridAutoPick)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.Panel panel1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Grid gridAutoPick;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnPick;
        private Win.UI.Panel panel5;
        private Win.UI.Button btnAutoCacu;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.CheckBox chkQuiting;
        private Win.UI.GroupBox groupBox3;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtMachineType;
        private Win.UI.CheckBox chkSewingType;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Label label1;
    }
}
