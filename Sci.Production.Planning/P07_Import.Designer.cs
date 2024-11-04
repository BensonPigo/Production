namespace Sci.Production.Planning
{
    partial class P07_Import
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnSave = new Sci.Win.UI.Button();
            this.gridBatchImport = new Sci.Win.UI.Grid();
            this.panel1 = new Sci.Win.UI.Panel();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtMonth = new Sci.Win.UI.TextBox();
            this.txtYear = new Sci.Win.UI.TextBox();
            this.btnDownloadTemplate = new Sci.Win.UI.Button();
            this.btnImportExcel = new Sci.Win.UI.Button();
            this.lblRemainCPU = new Sci.Win.UI.Label();
            this.txtLoadOnCpu = new Sci.Win.UI.DisplayBox();
            this.txtDeductLoadingFromMonth = new Sci.Win.UI.DisplayBox();
            this.txtAddPullFromMonth = new Sci.Win.UI.DisplayBox();
            this.txtFromSisterFactory = new Sci.Win.UI.DisplayBox();
            this.txtToSisterFactory = new Sci.Win.UI.DisplayBox();
            this.lblLoadOnCpu = new Sci.Win.UI.Label();
            this.lblDeductLoadingFromMonth = new Sci.Win.UI.Label();
            this.lblAddPullFromMonth = new Sci.Win.UI.Label();
            this.lblFromSisterFactory = new Sci.Win.UI.Label();
            this.lblToSisterFactory = new Sci.Win.UI.Label();
            this.lblFactory = new Sci.Win.UI.Label();
            this.lblMonth = new Sci.Win.UI.Label();
            this.txtLocalInCpu = new Sci.Win.UI.DisplayBox();
            this.txtCanceled = new Sci.Win.UI.DisplayBox();
            this.txtLastMonth = new Sci.Win.UI.DisplayBox();
            this.txtUnLastMonth = new Sci.Win.UI.DisplayBox();
            this.txtLoaded = new Sci.Win.UI.DisplayBox();
            this.lblLocalInCpu = new Sci.Win.UI.Label();
            this.lblCanceled = new Sci.Win.UI.Label();
            this.lblLastMonth = new Sci.Win.UI.Label();
            this.lblUnLastMonth = new Sci.Win.UI.Label();
            this.lblTTLCPULoaded = new Sci.Win.UI.Label();
            this.lblYear = new Sci.Win.UI.Label();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtRemainCPU = new Sci.Win.UI.DisplayBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchImport)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(954, 15);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // gridBatchImport
            // 
            this.gridBatchImport.AllowUserToAddRows = false;
            this.gridBatchImport.AllowUserToDeleteRows = false;
            this.gridBatchImport.AllowUserToResizeRows = false;
            this.gridBatchImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBatchImport.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridBatchImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBatchImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBatchImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBatchImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBatchImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBatchImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBatchImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBatchImport.Location = new System.Drawing.Point(0, 0);
            this.gridBatchImport.Name = "gridBatchImport";
            this.gridBatchImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBatchImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBatchImport.RowTemplate.Height = 24;
            this.gridBatchImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBatchImport.ShowCellToolTips = false;
            this.gridBatchImport.Size = new System.Drawing.Size(1050, 255);
            this.gridBatchImport.TabIndex = 0;
            this.gridBatchImport.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridBatchImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 191);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1050, 255);
            this.panel1.TabIndex = 26;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtfactory);
            this.groupBox1.Controls.Add(this.txtMonth);
            this.groupBox1.Controls.Add(this.txtYear);
            this.groupBox1.Controls.Add(this.btnDownloadTemplate);
            this.groupBox1.Controls.Add(this.btnImportExcel);
            this.groupBox1.Controls.Add(this.txtRemainCPU);
            this.groupBox1.Controls.Add(this.lblRemainCPU);
            this.groupBox1.Controls.Add(this.txtLoadOnCpu);
            this.groupBox1.Controls.Add(this.txtDeductLoadingFromMonth);
            this.groupBox1.Controls.Add(this.txtAddPullFromMonth);
            this.groupBox1.Controls.Add(this.txtFromSisterFactory);
            this.groupBox1.Controls.Add(this.txtToSisterFactory);
            this.groupBox1.Controls.Add(this.lblLoadOnCpu);
            this.groupBox1.Controls.Add(this.lblDeductLoadingFromMonth);
            this.groupBox1.Controls.Add(this.lblAddPullFromMonth);
            this.groupBox1.Controls.Add(this.lblFromSisterFactory);
            this.groupBox1.Controls.Add(this.lblToSisterFactory);
            this.groupBox1.Controls.Add(this.lblFactory);
            this.groupBox1.Controls.Add(this.lblMonth);
            this.groupBox1.Controls.Add(this.txtLocalInCpu);
            this.groupBox1.Controls.Add(this.txtCanceled);
            this.groupBox1.Controls.Add(this.txtLastMonth);
            this.groupBox1.Controls.Add(this.txtUnLastMonth);
            this.groupBox1.Controls.Add(this.txtLoaded);
            this.groupBox1.Controls.Add(this.lblLocalInCpu);
            this.groupBox1.Controls.Add(this.lblCanceled);
            this.groupBox1.Controls.Add(this.lblLastMonth);
            this.groupBox1.Controls.Add(this.lblUnLastMonth);
            this.groupBox1.Controls.Add(this.lblTTLCPULoaded);
            this.groupBox1.Controls.Add(this.lblYear);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1050, 191);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsMultiselect = false;
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(507, 21);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(99, 23);
            this.txtfactory.TabIndex = 52;
            // 
            // txtMonth
            // 
            this.txtMonth.BackColor = System.Drawing.Color.White;
            this.txtMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMonth.Location = new System.Drawing.Point(277, 20);
            this.txtMonth.Name = "txtMonth";
            this.txtMonth.Size = new System.Drawing.Size(137, 23);
            this.txtMonth.TabIndex = 51;
            this.txtMonth.TextChanged += new System.EventHandler(this.TxtMonth_TextChanged);
            this.txtMonth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtMonth_KeyPress);
            // 
            // txtYear
            // 
            this.txtYear.BackColor = System.Drawing.Color.White;
            this.txtYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtYear.Location = new System.Drawing.Point(72, 21);
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(137, 23);
            this.txtYear.TabIndex = 50;
            this.txtYear.TextChanged += new System.EventHandler(this.TxtYear_TextChanged);
            this.txtYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtYear_KeyPress);
            // 
            // btnDownloadTemplate
            // 
            this.btnDownloadTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownloadTemplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnDownloadTemplate.Location = new System.Drawing.Point(860, 58);
            this.btnDownloadTemplate.Name = "btnDownloadTemplate";
            this.btnDownloadTemplate.Size = new System.Drawing.Size(178, 30);
            this.btnDownloadTemplate.TabIndex = 49;
            this.btnDownloadTemplate.Text = "Download Template";
            this.btnDownloadTemplate.UseVisualStyleBackColor = true;
            this.btnDownloadTemplate.Click += new System.EventHandler(this.BtnDownloadTemplate_Click);
            // 
            // btnImportExcel
            // 
            this.btnImportExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportExcel.Location = new System.Drawing.Point(860, 22);
            this.btnImportExcel.Name = "btnImportExcel";
            this.btnImportExcel.Size = new System.Drawing.Size(178, 30);
            this.btnImportExcel.TabIndex = 48;
            this.btnImportExcel.Text = "Import Excel";
            this.btnImportExcel.UseVisualStyleBackColor = true;
            this.btnImportExcel.Click += new System.EventHandler(this.BtnImportExcel_Click);
            // 
            // lblRemainCPU
            // 
            this.lblRemainCPU.Location = new System.Drawing.Point(737, 159);
            this.lblRemainCPU.Name = "lblRemainCPU";
            this.lblRemainCPU.Size = new System.Drawing.Size(125, 23);
            this.lblRemainCPU.TabIndex = 46;
            this.lblRemainCPU.Text = "Remain CPU in M";
            // 
            // txtLoadOnCpu
            // 
            this.txtLoadOnCpu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtLoadOnCpu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtLoadOnCpu.Location = new System.Drawing.Point(538, 159);
            this.txtLoadOnCpu.Name = "txtLoadOnCpu";
            this.txtLoadOnCpu.Size = new System.Drawing.Size(178, 23);
            this.txtLoadOnCpu.TabIndex = 41;
            // 
            // txtDeductLoadingFromMonth
            // 
            this.txtDeductLoadingFromMonth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtDeductLoadingFromMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtDeductLoadingFromMonth.Location = new System.Drawing.Point(538, 132);
            this.txtDeductLoadingFromMonth.Name = "txtDeductLoadingFromMonth";
            this.txtDeductLoadingFromMonth.Size = new System.Drawing.Size(178, 23);
            this.txtDeductLoadingFromMonth.TabIndex = 42;
            // 
            // txtAddPullFromMonth
            // 
            this.txtAddPullFromMonth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtAddPullFromMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtAddPullFromMonth.Location = new System.Drawing.Point(538, 105);
            this.txtAddPullFromMonth.Name = "txtAddPullFromMonth";
            this.txtAddPullFromMonth.Size = new System.Drawing.Size(178, 23);
            this.txtAddPullFromMonth.TabIndex = 43;
            // 
            // txtFromSisterFactory
            // 
            this.txtFromSisterFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtFromSisterFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtFromSisterFactory.Location = new System.Drawing.Point(538, 77);
            this.txtFromSisterFactory.Name = "txtFromSisterFactory";
            this.txtFromSisterFactory.Size = new System.Drawing.Size(178, 23);
            this.txtFromSisterFactory.TabIndex = 44;
            // 
            // txtToSisterFactory
            // 
            this.txtToSisterFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtToSisterFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtToSisterFactory.Location = new System.Drawing.Point(538, 49);
            this.txtToSisterFactory.Name = "txtToSisterFactory";
            this.txtToSisterFactory.Size = new System.Drawing.Size(178, 23);
            this.txtToSisterFactory.TabIndex = 45;
            // 
            // lblLoadOnCpu
            // 
            this.lblLoadOnCpu.Location = new System.Drawing.Point(379, 159);
            this.lblLoadOnCpu.Name = "lblLoadOnCpu";
            this.lblLoadOnCpu.Size = new System.Drawing.Size(156, 23);
            this.lblLoadOnCpu.TabIndex = 40;
            this.lblLoadOnCpu.Text = "Local Sub Out CPU(-)";
            // 
            // lblDeductLoadingFromMonth
            // 
            this.lblDeductLoadingFromMonth.Location = new System.Drawing.Point(379, 132);
            this.lblDeductLoadingFromMonth.Name = "lblDeductLoadingFromMonth";
            this.lblDeductLoadingFromMonth.Size = new System.Drawing.Size(156, 23);
            this.lblDeductLoadingFromMonth.TabIndex = 39;
            this.lblDeductLoadingFromMonth.Text = "Deduct Delay From M";
            // 
            // lblAddPullFromMonth
            // 
            this.lblAddPullFromMonth.Location = new System.Drawing.Point(379, 105);
            this.lblAddPullFromMonth.Name = "lblAddPullFromMonth";
            this.lblAddPullFromMonth.Size = new System.Drawing.Size(156, 23);
            this.lblAddPullFromMonth.TabIndex = 38;
            this.lblAddPullFromMonth.Text = "Add Pull From Next M";
            // 
            // lblFromSisterFactory
            // 
            this.lblFromSisterFactory.Location = new System.Drawing.Point(379, 77);
            this.lblFromSisterFactory.Name = "lblFromSisterFactory";
            this.lblFromSisterFactory.Size = new System.Drawing.Size(156, 23);
            this.lblFromSisterFactory.TabIndex = 37;
            this.lblFromSisterFactory.Text = "Subcon From Sis (+)";
            // 
            // lblToSisterFactory
            // 
            this.lblToSisterFactory.Location = new System.Drawing.Point(379, 49);
            this.lblToSisterFactory.Name = "lblToSisterFactory";
            this.lblToSisterFactory.Size = new System.Drawing.Size(156, 23);
            this.lblToSisterFactory.TabIndex = 36;
            this.lblToSisterFactory.Text = "Subcon To Sis (-)";
            // 
            // lblFactory
            // 
            this.lblFactory.Location = new System.Drawing.Point(442, 21);
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.Size = new System.Drawing.Size(62, 23);
            this.lblFactory.TabIndex = 35;
            this.lblFactory.Text = "Factory";
            // 
            // lblMonth
            // 
            this.lblMonth.Location = new System.Drawing.Point(212, 21);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(62, 23);
            this.lblMonth.TabIndex = 33;
            this.lblMonth.Text = "Month";
            // 
            // txtLocalInCpu
            // 
            this.txtLocalInCpu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtLocalInCpu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtLocalInCpu.Location = new System.Drawing.Point(179, 158);
            this.txtLocalInCpu.Name = "txtLocalInCpu";
            this.txtLocalInCpu.Size = new System.Drawing.Size(178, 23);
            this.txtLocalInCpu.TabIndex = 25;
            // 
            // txtCanceled
            // 
            this.txtCanceled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtCanceled.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtCanceled.Location = new System.Drawing.Point(179, 131);
            this.txtCanceled.Name = "txtCanceled";
            this.txtCanceled.Size = new System.Drawing.Size(178, 23);
            this.txtCanceled.TabIndex = 24;
            // 
            // txtLastMonth
            // 
            this.txtLastMonth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtLastMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtLastMonth.Location = new System.Drawing.Point(179, 104);
            this.txtLastMonth.Name = "txtLastMonth";
            this.txtLastMonth.Size = new System.Drawing.Size(178, 23);
            this.txtLastMonth.TabIndex = 23;
            // 
            // txtUnLastMonth
            // 
            this.txtUnLastMonth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtUnLastMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtUnLastMonth.Location = new System.Drawing.Point(179, 76);
            this.txtUnLastMonth.Name = "txtUnLastMonth";
            this.txtUnLastMonth.Size = new System.Drawing.Size(178, 23);
            this.txtUnLastMonth.TabIndex = 22;
            // 
            // txtLoaded
            // 
            this.txtLoaded.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtLoaded.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtLoaded.Location = new System.Drawing.Point(179, 48);
            this.txtLoaded.Name = "txtLoaded";
            this.txtLoaded.Size = new System.Drawing.Size(178, 23);
            this.txtLoaded.TabIndex = 21;
            // 
            // lblLocalInCpu
            // 
            this.lblLocalInCpu.Location = new System.Drawing.Point(7, 159);
            this.lblLocalInCpu.Name = "lblLocalInCpu";
            this.lblLocalInCpu.Size = new System.Drawing.Size(169, 23);
            this.lblLocalInCpu.TabIndex = 30;
            this.lblLocalInCpu.Text = "Local Sub In CPU(+)";
            // 
            // lblCanceled
            // 
            this.lblCanceled.Location = new System.Drawing.Point(7, 132);
            this.lblCanceled.Name = "lblCanceled";
            this.lblCanceled.Size = new System.Drawing.Size(169, 23);
            this.lblCanceled.TabIndex = 29;
            this.lblCanceled.Text = "Canceled(Still need Prod)";
            // 
            // lblLastMonth
            // 
            this.lblLastMonth.Location = new System.Drawing.Point(7, 105);
            this.lblLastMonth.Name = "lblLastMonth";
            this.lblLastMonth.Size = new System.Drawing.Size(169, 23);
            this.lblLastMonth.TabIndex = 28;
            this.lblLastMonth.Text = "Finished Last Month (-)";
            // 
            // lblUnLastMonth
            // 
            this.lblUnLastMonth.Location = new System.Drawing.Point(7, 77);
            this.lblUnLastMonth.Name = "lblUnLastMonth";
            this.lblUnLastMonth.Size = new System.Drawing.Size(169, 23);
            this.lblUnLastMonth.TabIndex = 27;
            this.lblUnLastMonth.Text = "Unfinished Last Month (+)";
            // 
            // lblTTLCPULoaded
            // 
            this.lblTTLCPULoaded.Location = new System.Drawing.Point(7, 49);
            this.lblTTLCPULoaded.Name = "lblTTLCPULoaded";
            this.lblTTLCPULoaded.Size = new System.Drawing.Size(169, 23);
            this.lblTTLCPULoaded.TabIndex = 31;
            this.lblTTLCPULoaded.Text = "TTL. CPU loaded";
            // 
            // lblYear
            // 
            this.lblYear.Location = new System.Drawing.Point(7, 21);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(62, 23);
            this.lblYear.TabIndex = 26;
            this.lblYear.Text = "Year";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 446);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1050, 53);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtRemainCPU
            // 
            this.txtRemainCPU.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtRemainCPU.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtRemainCPU.Location = new System.Drawing.Point(865, 159);
            this.txtRemainCPU.Name = "txtRemainCPU";
            this.txtRemainCPU.Size = new System.Drawing.Size(178, 23);
            this.txtRemainCPU.TabIndex = 47;
            // 
            // P07_Import
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 499);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P07_Import";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Import Daily Accumulated CPU Loading Report";
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchImport)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.Button btnSave;
        private Win.UI.Grid gridBatchImport;
        private Win.UI.Panel panel1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.Label lblRemainCPU;
        private Win.UI.DisplayBox txtLoadOnCpu;
        private Win.UI.DisplayBox txtDeductLoadingFromMonth;
        private Win.UI.DisplayBox txtAddPullFromMonth;
        private Win.UI.DisplayBox txtFromSisterFactory;
        private Win.UI.DisplayBox txtToSisterFactory;
        private Win.UI.Label lblLoadOnCpu;
        private Win.UI.Label lblDeductLoadingFromMonth;
        private Win.UI.Label lblAddPullFromMonth;
        private Win.UI.Label lblFromSisterFactory;
        private Win.UI.Label lblToSisterFactory;
        private Win.UI.Label lblFactory;
        private Win.UI.Label lblMonth;
        private Win.UI.DisplayBox txtLocalInCpu;
        private Win.UI.DisplayBox txtCanceled;
        private Win.UI.DisplayBox txtLastMonth;
        private Win.UI.DisplayBox txtUnLastMonth;
        private Win.UI.DisplayBox txtLoaded;
        private Win.UI.Label lblLocalInCpu;
        private Win.UI.Label lblCanceled;
        private Win.UI.Label lblLastMonth;
        private Win.UI.Label lblUnLastMonth;
        private Win.UI.Label lblTTLCPULoaded;
        private Win.UI.Label lblYear;
        private Win.UI.Button btnDownloadTemplate;
        private Win.UI.Button btnImportExcel;
        private Win.UI.TextBox txtMonth;
        private Win.UI.TextBox txtYear;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Class.Txtfactory txtfactory;
        private Win.UI.DisplayBox txtRemainCPU;
    }
}