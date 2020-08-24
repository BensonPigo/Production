namespace Sci.Production.PublicForm
{
    partial class StdGSDList
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.comboTypeFilter = new Sci.Win.UI.ComboBox();
            this.labelTypeFilter = new Sci.Win.UI.Label();
            this.numTotalSMV = new Sci.Win.UI.NumericBox();
            this.labelTotalSMV = new Sci.Win.UI.Label();
            this.numTotalCPUTMS = new Sci.Win.UI.NumericBox();
            this.labelTotalCPUTMS = new Sci.Win.UI.Label();
            this.numTotalGSD = new Sci.Win.UI.NumericBox();
            this.labelTotalGSD = new Sci.Win.UI.Label();
            this.dateRequireFinish = new Sci.Win.UI.DateBox();
            this.labelRequireFinish = new Sci.Win.UI.Label();
            this.displayVersion = new Sci.Win.UI.DisplayBox();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.labelVersion = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.displayApplyNo = new Sci.Win.UI.DisplayBox();
            this.displayStyleNo = new Sci.Win.UI.DisplayBox();
            this.labelApplyNo = new Sci.Win.UI.Label();
            this.labelStyleNo = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.panel5 = new Sci.Win.UI.Panel();
            this.tabControl1 = new Sci.Win.UI.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gridStdGSD = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gridSummaryByArtwork = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.gridSummaryByMachine = new Sci.Win.UI.Grid();
            this.listControlBindingSource3 = new Sci.Win.UI.ListControlBindingSource();
            this.btnCIPF = new Sci.Win.UI.Button();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridStdGSD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSummaryByArtwork)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSummaryByMachine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource3)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 562);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(872, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 562);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnCIPF);
            this.panel3.Controls.Add(this.btnToExcel);
            this.panel3.Controls.Add(this.comboTypeFilter);
            this.panel3.Controls.Add(this.labelTypeFilter);
            this.panel3.Controls.Add(this.numTotalSMV);
            this.panel3.Controls.Add(this.labelTotalSMV);
            this.panel3.Controls.Add(this.numTotalCPUTMS);
            this.panel3.Controls.Add(this.labelTotalCPUTMS);
            this.panel3.Controls.Add(this.numTotalGSD);
            this.panel3.Controls.Add(this.labelTotalGSD);
            this.panel3.Controls.Add(this.dateRequireFinish);
            this.panel3.Controls.Add(this.labelRequireFinish);
            this.panel3.Controls.Add(this.displayVersion);
            this.panel3.Controls.Add(this.displaySeason);
            this.panel3.Controls.Add(this.labelVersion);
            this.panel3.Controls.Add(this.labelSeason);
            this.panel3.Controls.Add(this.displayApplyNo);
            this.panel3.Controls.Add(this.displayStyleNo);
            this.panel3.Controls.Add(this.labelApplyNo);
            this.panel3.Controls.Add(this.labelStyleNo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(862, 130);
            this.panel3.TabIndex = 2;
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(222, 96);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 18;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // comboTypeFilter
            // 
            this.comboTypeFilter.BackColor = System.Drawing.Color.White;
            this.comboTypeFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboTypeFilter.FormattingEnabled = true;
            this.comboTypeFilter.IsSupportUnselect = true;
            this.comboTypeFilter.Location = new System.Drawing.Point(79, 100);
            this.comboTypeFilter.Name = "comboTypeFilter";
            this.comboTypeFilter.OldText = "";
            this.comboTypeFilter.Size = new System.Drawing.Size(121, 24);
            this.comboTypeFilter.TabIndex = 17;
            this.comboTypeFilter.SelectedIndexChanged += new System.EventHandler(this.ComboTypeFilter_SelectedIndexChanged);
            // 
            // labelTypeFilter
            // 
            this.labelTypeFilter.Location = new System.Drawing.Point(0, 101);
            this.labelTypeFilter.Name = "labelTypeFilter";
            this.labelTypeFilter.Size = new System.Drawing.Size(75, 23);
            this.labelTypeFilter.TabIndex = 16;
            this.labelTypeFilter.Text = "Type Filter";
            // 
            // numTotalSMV
            // 
            this.numTotalSMV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalSMV.DecimalPlaces = 4;
            this.numTotalSMV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalSMV.IsSupportEditMode = false;
            this.numTotalSMV.Location = new System.Drawing.Point(615, 61);
            this.numTotalSMV.Name = "numTotalSMV";
            this.numTotalSMV.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalSMV.ReadOnly = true;
            this.numTotalSMV.Size = new System.Drawing.Size(70, 23);
            this.numTotalSMV.TabIndex = 15;
            this.numTotalSMV.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelTotalSMV
            // 
            this.labelTotalSMV.Location = new System.Drawing.Point(511, 61);
            this.labelTotalSMV.Name = "labelTotalSMV";
            this.labelTotalSMV.Size = new System.Drawing.Size(100, 23);
            this.labelTotalSMV.TabIndex = 14;
            this.labelTotalSMV.Text = "Total SMV(min)";
            // 
            // numTotalCPUTMS
            // 
            this.numTotalCPUTMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalCPUTMS.DecimalPlaces = 2;
            this.numTotalCPUTMS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalCPUTMS.IsSupportEditMode = false;
            this.numTotalCPUTMS.Location = new System.Drawing.Point(385, 61);
            this.numTotalCPUTMS.Name = "numTotalCPUTMS";
            this.numTotalCPUTMS.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalCPUTMS.ReadOnly = true;
            this.numTotalCPUTMS.Size = new System.Drawing.Size(70, 23);
            this.numTotalCPUTMS.TabIndex = 13;
            this.numTotalCPUTMS.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelTotalCPUTMS
            // 
            this.labelTotalCPUTMS.Location = new System.Drawing.Point(250, 61);
            this.labelTotalCPUTMS.Name = "labelTotalCPUTMS";
            this.labelTotalCPUTMS.Size = new System.Drawing.Size(131, 23);
            this.labelTotalCPUTMS.TabIndex = 12;
            this.labelTotalCPUTMS.Text = "Total CPU TMS(sec)";
            // 
            // numTotalGSD
            // 
            this.numTotalGSD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalGSD.DecimalPlaces = 2;
            this.numTotalGSD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalGSD.IsSupportEditMode = false;
            this.numTotalGSD.Location = new System.Drawing.Point(104, 61);
            this.numTotalGSD.Name = "numTotalGSD";
            this.numTotalGSD.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalGSD.ReadOnly = true;
            this.numTotalGSD.Size = new System.Drawing.Size(70, 23);
            this.numTotalGSD.TabIndex = 11;
            this.numTotalGSD.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelTotalGSD
            // 
            this.labelTotalGSD.Location = new System.Drawing.Point(1, 61);
            this.labelTotalGSD.Name = "labelTotalGSD";
            this.labelTotalGSD.Size = new System.Drawing.Size(99, 23);
            this.labelTotalGSD.TabIndex = 10;
            this.labelTotalGSD.Text = "Total GSD(sec)";
            // 
            // dateRequireFinish
            // 
            this.dateRequireFinish.IsSupportEditMode = false;
            this.dateRequireFinish.Location = new System.Drawing.Point(610, 7);
            this.dateRequireFinish.Name = "dateRequireFinish";
            this.dateRequireFinish.ReadOnly = true;
            this.dateRequireFinish.Size = new System.Drawing.Size(110, 23);
            this.dateRequireFinish.TabIndex = 9;
            // 
            // labelRequireFinish
            // 
            this.labelRequireFinish.Location = new System.Drawing.Point(511, 7);
            this.labelRequireFinish.Name = "labelRequireFinish";
            this.labelRequireFinish.Size = new System.Drawing.Size(95, 23);
            this.labelRequireFinish.TabIndex = 8;
            this.labelRequireFinish.Text = "Require Finish";
            // 
            // displayVersion
            // 
            this.displayVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayVersion.Location = new System.Drawing.Point(306, 34);
            this.displayVersion.Name = "displayVersion";
            this.displayVersion.Size = new System.Drawing.Size(27, 23);
            this.displayVersion.TabIndex = 7;
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(306, 7);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(80, 23);
            this.displaySeason.TabIndex = 6;
            // 
            // labelVersion
            // 
            this.labelVersion.Location = new System.Drawing.Point(250, 34);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(52, 23);
            this.labelVersion.TabIndex = 5;
            this.labelVersion.Text = "Version";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(250, 7);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(52, 23);
            this.labelSeason.TabIndex = 4;
            this.labelSeason.Text = "Season";
            // 
            // displayApplyNo
            // 
            this.displayApplyNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayApplyNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayApplyNo.Location = new System.Drawing.Point(71, 34);
            this.displayApplyNo.Name = "displayApplyNo";
            this.displayApplyNo.Size = new System.Drawing.Size(100, 23);
            this.displayApplyNo.TabIndex = 3;
            // 
            // displayStyleNo
            // 
            this.displayStyleNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyleNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyleNo.Location = new System.Drawing.Point(71, 7);
            this.displayStyleNo.Name = "displayStyleNo";
            this.displayStyleNo.Size = new System.Drawing.Size(130, 23);
            this.displayStyleNo.TabIndex = 2;
            // 
            // labelApplyNo
            // 
            this.labelApplyNo.Location = new System.Drawing.Point(1, 34);
            this.labelApplyNo.Name = "labelApplyNo";
            this.labelApplyNo.Size = new System.Drawing.Size(66, 23);
            this.labelApplyNo.TabIndex = 1;
            this.labelApplyNo.Text = "Apply No.";
            // 
            // labelStyleNo
            // 
            this.labelStyleNo.Location = new System.Drawing.Point(1, 7);
            this.labelStyleNo.Name = "labelStyleNo";
            this.labelStyleNo.Size = new System.Drawing.Size(66, 23);
            this.labelStyleNo.TabIndex = 0;
            this.labelStyleNo.Text = "Style#";
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 552);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(862, 10);
            this.panel4.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.tabControl1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 130);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(862, 422);
            this.panel5.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(862, 422);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gridStdGSD);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(854, 393);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Std. GSD";
            // 
            // gridStdGSD
            // 
            this.gridStdGSD.AllowUserToAddRows = false;
            this.gridStdGSD.AllowUserToDeleteRows = false;
            this.gridStdGSD.AllowUserToResizeRows = false;
            this.gridStdGSD.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridStdGSD.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridStdGSD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridStdGSD.DataSource = this.listControlBindingSource1;
            this.gridStdGSD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridStdGSD.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridStdGSD.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridStdGSD.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridStdGSD.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridStdGSD.Location = new System.Drawing.Point(3, 3);
            this.gridStdGSD.Name = "gridStdGSD";
            this.gridStdGSD.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridStdGSD.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridStdGSD.RowTemplate.Height = 24;
            this.gridStdGSD.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridStdGSD.ShowCellToolTips = false;
            this.gridStdGSD.Size = new System.Drawing.Size(848, 387);
            this.gridStdGSD.TabIndex = 0;
            this.gridStdGSD.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gridSummaryByArtwork);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(854, 393);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Summary by artwork";
            // 
            // gridSummaryByArtwork
            // 
            this.gridSummaryByArtwork.AllowUserToAddRows = false;
            this.gridSummaryByArtwork.AllowUserToDeleteRows = false;
            this.gridSummaryByArtwork.AllowUserToResizeRows = false;
            this.gridSummaryByArtwork.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSummaryByArtwork.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSummaryByArtwork.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSummaryByArtwork.DataSource = this.listControlBindingSource2;
            this.gridSummaryByArtwork.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSummaryByArtwork.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSummaryByArtwork.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSummaryByArtwork.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSummaryByArtwork.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSummaryByArtwork.Location = new System.Drawing.Point(3, 3);
            this.gridSummaryByArtwork.Name = "gridSummaryByArtwork";
            this.gridSummaryByArtwork.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSummaryByArtwork.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSummaryByArtwork.RowTemplate.Height = 24;
            this.gridSummaryByArtwork.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSummaryByArtwork.ShowCellToolTips = false;
            this.gridSummaryByArtwork.Size = new System.Drawing.Size(848, 387);
            this.gridSummaryByArtwork.TabIndex = 0;
            this.gridSummaryByArtwork.TabStop = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.gridSummaryByMachine);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(854, 396);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Summary by machine";
            // 
            // gridSummaryByMachine
            // 
            this.gridSummaryByMachine.AllowUserToAddRows = false;
            this.gridSummaryByMachine.AllowUserToDeleteRows = false;
            this.gridSummaryByMachine.AllowUserToResizeRows = false;
            this.gridSummaryByMachine.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSummaryByMachine.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSummaryByMachine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSummaryByMachine.DataSource = this.listControlBindingSource3;
            this.gridSummaryByMachine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSummaryByMachine.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSummaryByMachine.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSummaryByMachine.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSummaryByMachine.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSummaryByMachine.Location = new System.Drawing.Point(3, 3);
            this.gridSummaryByMachine.Name = "gridSummaryByMachine";
            this.gridSummaryByMachine.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSummaryByMachine.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSummaryByMachine.RowTemplate.Height = 24;
            this.gridSummaryByMachine.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSummaryByMachine.ShowCellToolTips = false;
            this.gridSummaryByMachine.Size = new System.Drawing.Size(848, 390);
            this.gridSummaryByMachine.TabIndex = 0;
            this.gridSummaryByMachine.TabStop = false;
            // 
            // btnCIPF
            // 
            this.btnCIPF.Location = new System.Drawing.Point(481, 96);
            this.btnCIPF.Name = "btnCIPF";
            this.btnCIPF.Size = new System.Drawing.Size(60, 30);
            this.btnCIPF.TabIndex = 19;
            this.btnCIPF.Text = "CIPF";
            this.btnCIPF.UseVisualStyleBackColor = true;
            this.btnCIPF.Click += new System.EventHandler(this.BtnCIPF_Click);
            // 
            // StdGSDList
            // 
            this.ClientSize = new System.Drawing.Size(882, 562);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "StdGSDList";
            this.Text = "Std. GSD List";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridStdGSD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSummaryByArtwork)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSummaryByMachine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Win.UI.Grid gridStdGSD;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private System.Windows.Forms.TabPage tabPage2;
        private Win.UI.Grid gridSummaryByArtwork;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private System.Windows.Forms.TabPage tabPage3;
        private Win.UI.Grid gridSummaryByMachine;
        private Win.UI.ListControlBindingSource listControlBindingSource3;
        private Win.UI.Label labelApplyNo;
        private Win.UI.Label labelStyleNo;
        private Win.UI.DisplayBox displayApplyNo;
        private Win.UI.DisplayBox displayStyleNo;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.Label labelVersion;
        private Win.UI.Label labelSeason;
        private Win.UI.DisplayBox displayVersion;
        private Win.UI.NumericBox numTotalGSD;
        private Win.UI.Label labelTotalGSD;
        private Win.UI.DateBox dateRequireFinish;
        private Win.UI.Label labelRequireFinish;
        private Win.UI.ComboBox comboTypeFilter;
        private Win.UI.Label labelTypeFilter;
        private Win.UI.NumericBox numTotalSMV;
        private Win.UI.Label labelTotalSMV;
        private Win.UI.NumericBox numTotalCPUTMS;
        private Win.UI.Label labelTotalCPUTMS;
        private Win.UI.Button btnToExcel;
        private Win.UI.Button btnCIPF;
    }
}
