namespace Sci.Production.Packing
{
    partial class P17
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
            this.panel3 = new Sci.Win.UI.Panel();
            this.comboBrand = new Sci.Win.UI.ComboBox();
            this.label1 = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.Btnexit = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.panel7 = new Sci.Win.UI.Panel();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel6 = new Sci.Win.UI.Panel();
            this.panel10 = new Sci.Win.UI.Panel();
            this.panel12 = new Sci.Win.UI.Panel();
            this.gridAttachFile = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel11 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Panel();
            this.button1 = new Sci.Win.UI.Button();
            this.btnCheckImport = new Sci.Win.UI.Button();
            this.btnRemoveExcel = new Sci.Win.UI.Button();
            this.btnAddExcel = new Sci.Win.UI.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttachFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.btnClose.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.comboBrand);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(929, 33);
            this.panel3.TabIndex = 2;
            // 
            // comboBrand
            // 
            this.comboBrand.BackColor = System.Drawing.Color.White;
            this.comboBrand.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBrand.FormattingEnabled = true;
            this.comboBrand.IsSupportUnselect = true;
            this.comboBrand.Items.AddRange(new object[] {
            "N.FACE",
            "DOME",
            "LLL"});
            this.comboBrand.Location = new System.Drawing.Point(70, 6);
            this.comboBrand.Name = "comboBrand";
            this.comboBrand.OldText = "";
            this.comboBrand.Size = new System.Drawing.Size(121, 24);
            this.comboBrand.TabIndex = 2;
            this.comboBrand.SelectedIndexChanged += new System.EventHandler(this.ComboBrand_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Brand";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.Btnexit);
            this.panel4.Controls.Add(this.btnImport);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 486);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(929, 42);
            this.panel4.TabIndex = 3;
            // 
            // Btnexit
            // 
            this.Btnexit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Btnexit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Btnexit.Location = new System.Drawing.Point(843, 6);
            this.Btnexit.Name = "Btnexit";
            this.Btnexit.Size = new System.Drawing.Size(80, 30);
            this.Btnexit.TabIndex = 3;
            this.Btnexit.Text = "Exit";
            this.Btnexit.UseVisualStyleBackColor = true;
            this.Btnexit.Click += new System.EventHandler(this.Btnclose_click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(757, 6);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnWriteIn_Click_1);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 33);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(929, 453);
            this.panel5.TabIndex = 4;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.gridDetail);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 169);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(929, 284);
            this.panel7.TabIndex = 1;
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.DataSource = this.listControlBindingSource2;
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(0, 0);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowHeadersVisible = false;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(929, 284);
            this.gridDetail.TabIndex = 0;
            this.gridDetail.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel10);
            this.panel6.Controls.Add(this.btnClose);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(929, 169);
            this.panel6.TabIndex = 0;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.panel12);
            this.panel10.Controls.Add(this.panel11);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(0, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(769, 169);
            this.panel10.TabIndex = 2;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.gridAttachFile);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel12.Location = new System.Drawing.Point(0, 0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(769, 159);
            this.panel12.TabIndex = 1;
            // 
            // gridAttachFile
            // 
            this.gridAttachFile.AllowUserToAddRows = false;
            this.gridAttachFile.AllowUserToDeleteRows = false;
            this.gridAttachFile.AllowUserToResizeRows = false;
            this.gridAttachFile.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridAttachFile.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridAttachFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAttachFile.DataSource = this.listControlBindingSource1;
            this.gridAttachFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridAttachFile.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridAttachFile.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridAttachFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridAttachFile.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridAttachFile.Location = new System.Drawing.Point(0, 0);
            this.gridAttachFile.Name = "gridAttachFile";
            this.gridAttachFile.RowHeadersVisible = false;
            this.gridAttachFile.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridAttachFile.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridAttachFile.RowTemplate.Height = 24;
            this.gridAttachFile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAttachFile.ShowCellToolTips = false;
            this.gridAttachFile.Size = new System.Drawing.Size(769, 159);
            this.gridAttachFile.TabIndex = 0;
            this.gridAttachFile.TabStop = false;
            // 
            // panel11
            // 
            this.panel11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel11.Location = new System.Drawing.Point(0, 159);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(769, 10);
            this.panel11.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Controls.Add(this.button1);
            this.btnClose.Controls.Add(this.btnCheckImport);
            this.btnClose.Controls.Add(this.btnRemoveExcel);
            this.btnClose.Controls.Add(this.btnAddExcel);
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(769, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(160, 169);
            this.btnClose.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 115);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 44);
            this.button1.TabIndex = 3;
            this.button1.Text = "Download Other Template";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.Btndowload_click);
            // 
            // btnCheckImport
            // 
            this.btnCheckImport.Location = new System.Drawing.Point(7, 79);
            this.btnCheckImport.Name = "btnCheckImport";
            this.btnCheckImport.Size = new System.Drawing.Size(146, 30);
            this.btnCheckImport.TabIndex = 2;
            this.btnCheckImport.Text = "Check File";
            this.btnCheckImport.UseVisualStyleBackColor = true;
            this.btnCheckImport.Click += new System.EventHandler(this.BtnCheckImport_Click);
            // 
            // btnRemoveExcel
            // 
            this.btnRemoveExcel.Location = new System.Drawing.Point(6, 43);
            this.btnRemoveExcel.Name = "btnRemoveExcel";
            this.btnRemoveExcel.Size = new System.Drawing.Size(146, 30);
            this.btnRemoveExcel.TabIndex = 1;
            this.btnRemoveExcel.Text = "Remove File";
            this.btnRemoveExcel.UseVisualStyleBackColor = true;
            this.btnRemoveExcel.Click += new System.EventHandler(this.BtnRemoveExcel_Click);
            // 
            // btnAddExcel
            // 
            this.btnAddExcel.Location = new System.Drawing.Point(6, 6);
            this.btnAddExcel.Name = "btnAddExcel";
            this.btnAddExcel.Size = new System.Drawing.Size(146, 30);
            this.btnAddExcel.TabIndex = 0;
            this.btnAddExcel.Text = "Add File";
            this.btnAddExcel.UseVisualStyleBackColor = true;
            this.btnAddExcel.Click += new System.EventHandler(this.BtnAddExcel_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 528);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(939, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 528);
            this.panel2.TabIndex = 1;
            // 
            // P17
            // 
            this.ClientSize = new System.Drawing.Size(949, 528);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P17";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P17. Import Scan & Pack Barcode";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridAttachFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.btnClose.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Panel panel7;
        private Win.UI.Grid gridDetail;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Panel panel6;
        private Win.UI.Panel panel10;
        private Win.UI.Grid gridAttachFile;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel btnClose;
        private Win.UI.Button btnCheckImport;
        private Win.UI.Button btnRemoveExcel;
        private Win.UI.Button btnAddExcel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Win.UI.Panel panel12;
        private Win.UI.Panel panel11;
        private Win.UI.ComboBox comboBrand;
        private Win.UI.Label label1;
        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Button Btnexit;
        private Win.UI.Button btnImport;
        private Win.UI.Button button1;
    }
}
