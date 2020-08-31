namespace Sci.Production.Warehouse
{
    partial class P22_Import
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
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.btnUpdateAllLocation = new Sci.Win.UI.Button();
            this.txtLocation = new Sci.Win.UI.TextBox();
            this.labelLocation = new Sci.Win.UI.Label();
            this.checkReturn = new Sci.Win.UI.CheckBox();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.grid_TaipeiInput = new Sci.Win.UI.Grid();
            this.panel1 = new Sci.Win.UI.Panel();
            this.grid_ftyDetail = new Sci.Win.UI.Grid();
            this.TaipeiInputBS = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.FtyDetailBS = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_TaipeiInput)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_ftyDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaipeiInputBS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FtyDetailBS)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(1034, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(938, 15);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindNow.Location = new System.Drawing.Point(357, 15);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 1;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(107, 19);
            this.txtSPNo.MaxLength = 13;
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(122, 23);
            this.txtSPNo.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(9, 19);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(95, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP# (Poid)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnUpdateAllLocation);
            this.groupBox2.Controls.Add(this.txtLocation);
            this.groupBox2.Controls.Add(this.labelLocation);
            this.groupBox2.Controls.Add(this.checkReturn);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 548);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1130, 53);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // btnUpdateAllLocation
            // 
            this.btnUpdateAllLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateAllLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnUpdateAllLocation.Location = new System.Drawing.Point(747, 15);
            this.btnUpdateAllLocation.Name = "btnUpdateAllLocation";
            this.btnUpdateAllLocation.Size = new System.Drawing.Size(142, 30);
            this.btnUpdateAllLocation.TabIndex = 7;
            this.btnUpdateAllLocation.Text = "Update All Location";
            this.btnUpdateAllLocation.UseVisualStyleBackColor = true;
            this.btnUpdateAllLocation.Click += new System.EventHandler(this.BtnUpdateAllLocation_Click);
            // 
            // txtLocation
            // 
            this.txtLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtLocation.IsSupportEditMode = false;
            this.txtLocation.Location = new System.Drawing.Point(519, 19);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.ReadOnly = true;
            this.txtLocation.Size = new System.Drawing.Size(222, 23);
            this.txtLocation.TabIndex = 6;
            this.txtLocation.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TxtLocation_MouseDown);
            // 
            // labelLocation
            // 
            this.labelLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLocation.Location = new System.Drawing.Point(446, 19);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(69, 23);
            this.labelLocation.TabIndex = 8;
            this.labelLocation.Text = "Location";
            // 
            // checkReturn
            // 
            this.checkReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkReturn.AutoSize = true;
            this.checkReturn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkReturn.IsSupportEditMode = false;
            this.checkReturn.Location = new System.Drawing.Point(9, 19);
            this.checkReturn.Name = "checkReturn";
            this.checkReturn.Size = new System.Drawing.Size(189, 21);
            this.checkReturn.TabIndex = 5;
            this.checkReturn.Text = "Return Transfer Qty Back";
            this.checkReturn.UseVisualStyleBackColor = true;
            this.checkReturn.Click += new System.EventHandler(this.CheckReturn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grid_TaipeiInput);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtSPNo);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1130, 287);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // grid_TaipeiInput
            // 
            this.grid_TaipeiInput.AllowUserToAddRows = false;
            this.grid_TaipeiInput.AllowUserToDeleteRows = false;
            this.grid_TaipeiInput.AllowUserToResizeRows = false;
            this.grid_TaipeiInput.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid_TaipeiInput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grid_TaipeiInput.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid_TaipeiInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_TaipeiInput.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid_TaipeiInput.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid_TaipeiInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid_TaipeiInput.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid_TaipeiInput.Location = new System.Drawing.Point(3, 55);
            this.grid_TaipeiInput.Name = "grid_TaipeiInput";
            this.grid_TaipeiInput.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_TaipeiInput.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_TaipeiInput.RowTemplate.Height = 24;
            this.grid_TaipeiInput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_TaipeiInput.ShowCellToolTips = false;
            this.grid_TaipeiInput.Size = new System.Drawing.Size(1122, 217);
            this.grid_TaipeiInput.TabIndex = 4;
            this.grid_TaipeiInput.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grid_ftyDetail);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 287);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1130, 261);
            this.panel1.TabIndex = 20;
            // 
            // grid_ftyDetail
            // 
            this.grid_ftyDetail.AllowUserToAddRows = false;
            this.grid_ftyDetail.AllowUserToDeleteRows = false;
            this.grid_ftyDetail.AllowUserToResizeRows = false;
            this.grid_ftyDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid_ftyDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grid_ftyDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid_ftyDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_ftyDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_ftyDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid_ftyDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid_ftyDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid_ftyDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid_ftyDetail.Location = new System.Drawing.Point(0, 0);
            this.grid_ftyDetail.Name = "grid_ftyDetail";
            this.grid_ftyDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_ftyDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_ftyDetail.RowTemplate.Height = 24;
            this.grid_ftyDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_ftyDetail.ShowCellToolTips = false;
            this.grid_ftyDetail.Size = new System.Drawing.Size(1130, 261);
            this.grid_ftyDetail.TabIndex = 0;
            this.grid_ftyDetail.TabStop = false;
            this.grid_ftyDetail.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.Grid_ftyDetail_CellFormatting);
            // 
            // P22_Import
            // 
            this.ClientSize = new System.Drawing.Size(1130, 601);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P22_Import";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P22. Import Detail";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_TaipeiInput)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_ftyDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaipeiInputBS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FtyDetailBS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelSPNo;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid grid_ftyDetail;
        private Win.UI.ListControlBindingSource TaipeiInputBS;
        private Win.UI.Grid grid_TaipeiInput;
        private Win.UI.ListControlBindingSource FtyDetailBS;
        private Win.UI.CheckBox checkReturn;
        private Win.UI.Button btnUpdateAllLocation;
        private Win.UI.TextBox txtLocation;
        private Win.UI.Label labelLocation;
    }
}
