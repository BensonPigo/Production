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
            this.btn_Cancel = new Sci.Win.UI.Button();
            this.checkBox2 = new Sci.Win.UI.CheckBox();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.btn_Import = new Sci.Win.UI.Button();
            this.button1 = new Sci.Win.UI.Button();
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.btn_updateLoc = new Sci.Win.UI.Button();
            this.textBox3 = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.cb_return = new Sci.Win.UI.CheckBox();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.grid_TaipeiInput = new Sci.Win.UI.Grid();
            this.panel1 = new Sci.Win.UI.Panel();
            this.grid_ftyDetail = new Sci.Win.UI.Grid();
            this.TaipeiInputBS = new Sci.Win.UI.ListControlBindingSource();
            this.FtyDetailBS = new Sci.Win.UI.ListControlBindingSource();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_TaipeiInput)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_ftyDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaipeiInputBS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FtyDetailBS)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btn_Cancel.Location = new System.Drawing.Point(912, 15);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(90, 30);
            this.btn_Cancel.TabIndex = 3;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox2.AutoSize = true;
            this.checkBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkBox2.IsSupportEditMode = false;
            this.checkBox2.Location = new System.Drawing.Point(25, 23);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.ReadOnly = true;
            this.checkBox2.Size = new System.Drawing.Size(15, 14);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkBox1.IsSupportEditMode = false;
            this.checkBox1.Location = new System.Drawing.Point(6, 23);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.ReadOnly = true;
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // btn_Import
            // 
            this.btn_Import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Import.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btn_Import.Location = new System.Drawing.Point(816, 15);
            this.btn_Import.Name = "btn_Import";
            this.btn_Import.Size = new System.Drawing.Size(90, 30);
            this.btn_Import.TabIndex = 2;
            this.btn_Import.Text = "Import";
            this.btn_Import.UseVisualStyleBackColor = true;
            this.btn_Import.Click += new System.EventHandler(this.btn_Import_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button1.Location = new System.Drawing.Point(235, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 30);
            this.button1.TabIndex = 3;
            this.button1.Text = "Find Now";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(107, 19);
            this.textBox1.MaxLength = 13;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(122, 23);
            this.textBox1.TabIndex = 0;
            this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "SP# (Poid)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_updateLoc);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cb_return);
            this.groupBox2.Controls.Add(this.btn_Cancel);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.btn_Import);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 548);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1008, 53);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // btn_updateLoc
            // 
            this.btn_updateLoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_updateLoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btn_updateLoc.Location = new System.Drawing.Point(625, 15);
            this.btn_updateLoc.Name = "btn_updateLoc";
            this.btn_updateLoc.Size = new System.Drawing.Size(142, 30);
            this.btn_updateLoc.TabIndex = 7;
            this.btn_updateLoc.Text = "Update All Location";
            this.btn_updateLoc.UseVisualStyleBackColor = true;
            this.btn_updateLoc.Click += new System.EventHandler(this.btn_updateLoc_Click);
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.textBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.textBox3.IsSupportEditMode = false;
            this.textBox3.Location = new System.Drawing.Point(400, 19);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(222, 23);
            this.textBox3.TabIndex = 6;
            this.textBox3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox3_MouseDown);
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(327, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "Location";
            // 
            // cb_return
            // 
            this.cb_return.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_return.AutoSize = true;
            this.cb_return.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cb_return.IsSupportEditMode = false;
            this.cb_return.Location = new System.Drawing.Point(70, 20);
            this.cb_return.Name = "cb_return";
            this.cb_return.Size = new System.Drawing.Size(189, 21);
            this.cb_return.TabIndex = 5;
            this.cb_return.Text = "Return Transfer Qty Back";
            this.cb_return.UseVisualStyleBackColor = true;
            this.cb_return.Click += new System.EventHandler(this.cb_return_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grid_TaipeiInput);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 287);
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
            this.grid_TaipeiInput.RowHeadersVisible = false;
            this.grid_TaipeiInput.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_TaipeiInput.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_TaipeiInput.RowTemplate.Height = 24;
            this.grid_TaipeiInput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_TaipeiInput.Size = new System.Drawing.Size(1008, 217);
            this.grid_TaipeiInput.TabIndex = 4;
            this.grid_TaipeiInput.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grid_ftyDetail);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 287);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 261);
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
            this.grid_ftyDetail.RowHeadersVisible = false;
            this.grid_ftyDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_ftyDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_ftyDetail.RowTemplate.Height = 24;
            this.grid_ftyDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_ftyDetail.Size = new System.Drawing.Size(1008, 261);
            this.grid_ftyDetail.TabIndex = 0;
            this.grid_ftyDetail.TabStop = false;
            // 
            // P22_Import
            // 
            this.ClientSize = new System.Drawing.Size(1008, 601);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P22_Import";
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

        private Win.UI.Button btn_Cancel;
        private Win.UI.CheckBox checkBox2;
        private Win.UI.CheckBox checkBox1;
        private Win.UI.Button btn_Import;
        private Win.UI.Button button1;
        private Win.UI.TextBox textBox1;
        private Win.UI.Label label1;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid grid_ftyDetail;
        private Win.UI.ListControlBindingSource TaipeiInputBS;
        private Win.UI.Grid grid_TaipeiInput;
        private Win.UI.ListControlBindingSource FtyDetailBS;
        private Win.UI.CheckBox cb_return;
        private Win.UI.Button btn_updateLoc;
        private Win.UI.TextBox textBox3;
        private Win.UI.Label label2;
    }
}
