namespace Sci.Production.IE
{
    partial class P05_Compare
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
            this.tabCompare = new Sci.Win.UI.TabControl();
            this.tabSewerMinus2 = new System.Windows.Forms.TabPage();
            this.panelDisplay = new Sci.Win.UI.Panel();
            this.splitBottom = new System.Windows.Forms.SplitContainer();
            this.gridManualNoDetail = new Sci.Win.UI.Grid();
            this.label3 = new Sci.Win.UI.Label();
            this.gridAutoNoDetail = new Sci.Win.UI.Grid();
            this.label4 = new Sci.Win.UI.Label();
            this.splitTop = new System.Windows.Forms.SplitContainer();
            this.gridManualMain = new Sci.Win.UI.Grid();
            this.numManualOPLoading = new Sci.Win.UI.NumericBox();
            this.numManualLBR = new Sci.Win.UI.NumericBox();
            this.numManualSewerManpower = new Sci.Win.UI.NumericBox();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.gridAutoMain = new Sci.Win.UI.Grid();
            this.numAutoOPLoading = new Sci.Win.UI.NumericBox();
            this.numAutoLBR = new Sci.Win.UI.NumericBox();
            this.numAutoSewerManpower = new Sci.Win.UI.NumericBox();
            this.label8 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.tabSewerMinus1 = new System.Windows.Forms.TabPage();
            this.tabSewerManualInput = new System.Windows.Forms.TabPage();
            this.tabSewerPlus1 = new System.Windows.Forms.TabPage();
            this.tabSewerPlus2 = new System.Windows.Forms.TabPage();
            this.gridAutoDetailBs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.gridManualDetailBs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.tabCompare.SuspendLayout();
            this.tabSewerMinus2.SuspendLayout();
            this.panelDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitBottom)).BeginInit();
            this.splitBottom.Panel1.SuspendLayout();
            this.splitBottom.Panel2.SuspendLayout();
            this.splitBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridManualNoDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAutoNoDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitTop)).BeginInit();
            this.splitTop.Panel1.SuspendLayout();
            this.splitTop.Panel2.SuspendLayout();
            this.splitTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridManualMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAutoMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAutoDetailBs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridManualDetailBs)).BeginInit();
            this.SuspendLayout();
            // 
            // tabCompare
            // 
            this.tabCompare.Controls.Add(this.tabSewerMinus2);
            this.tabCompare.Controls.Add(this.tabSewerMinus1);
            this.tabCompare.Controls.Add(this.tabSewerManualInput);
            this.tabCompare.Controls.Add(this.tabSewerPlus1);
            this.tabCompare.Controls.Add(this.tabSewerPlus2);
            this.tabCompare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCompare.Location = new System.Drawing.Point(0, 0);
            this.tabCompare.Name = "tabCompare";
            this.tabCompare.SelectedIndex = 0;
            this.tabCompare.Size = new System.Drawing.Size(1157, 687);
            this.tabCompare.TabIndex = 8;
            // 
            // tabSewerMinus2
            // 
            this.tabSewerMinus2.Controls.Add(this.panelDisplay);
            this.tabSewerMinus2.Location = new System.Drawing.Point(4, 25);
            this.tabSewerMinus2.Name = "tabSewerMinus2";
            this.tabSewerMinus2.Padding = new System.Windows.Forms.Padding(3);
            this.tabSewerMinus2.Size = new System.Drawing.Size(1149, 658);
            this.tabSewerMinus2.TabIndex = 0;
            this.tabSewerMinus2.Text = "-2 Sewer";
            // 
            // panelDisplay
            // 
            this.panelDisplay.Controls.Add(this.splitBottom);
            this.panelDisplay.Controls.Add(this.splitTop);
            this.panelDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDisplay.Location = new System.Drawing.Point(3, 3);
            this.panelDisplay.Name = "panelDisplay";
            this.panelDisplay.Size = new System.Drawing.Size(1143, 652);
            this.panelDisplay.TabIndex = 0;
            // 
            // splitBottom
            // 
            this.splitBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitBottom.Location = new System.Drawing.Point(0, 414);
            this.splitBottom.Name = "splitBottom";
            this.splitBottom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitBottom.Panel1
            // 
            this.splitBottom.Panel1.Controls.Add(this.gridManualNoDetail);
            this.splitBottom.Panel1.Controls.Add(this.label3);
            // 
            // splitBottom.Panel2
            // 
            this.splitBottom.Panel2.Controls.Add(this.gridAutoNoDetail);
            this.splitBottom.Panel2.Controls.Add(this.label4);
            this.splitBottom.Size = new System.Drawing.Size(1143, 238);
            this.splitBottom.SplitterDistance = 119;
            this.splitBottom.TabIndex = 1;
            // 
            // gridManualNoDetail
            // 
            this.gridManualNoDetail.AllowUserToAddRows = false;
            this.gridManualNoDetail.AllowUserToDeleteRows = false;
            this.gridManualNoDetail.AllowUserToResizeRows = false;
            this.gridManualNoDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridManualNoDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridManualNoDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridManualNoDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridManualNoDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridManualNoDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridManualNoDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridManualNoDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridManualNoDetail.Location = new System.Drawing.Point(80, 3);
            this.gridManualNoDetail.Name = "gridManualNoDetail";
            this.gridManualNoDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridManualNoDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridManualNoDetail.RowTemplate.Height = 24;
            this.gridManualNoDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridManualNoDetail.ShowCellToolTips = false;
            this.gridManualNoDetail.Size = new System.Drawing.Size(1058, 113);
            this.gridManualNoDetail.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "Manual";
            this.label3.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // gridAutoNoDetail
            // 
            this.gridAutoNoDetail.AllowUserToAddRows = false;
            this.gridAutoNoDetail.AllowUserToDeleteRows = false;
            this.gridAutoNoDetail.AllowUserToResizeRows = false;
            this.gridAutoNoDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridAutoNoDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridAutoNoDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridAutoNoDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAutoNoDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridAutoNoDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridAutoNoDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridAutoNoDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridAutoNoDetail.Location = new System.Drawing.Point(80, -1);
            this.gridAutoNoDetail.Name = "gridAutoNoDetail";
            this.gridAutoNoDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridAutoNoDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridAutoNoDetail.RowTemplate.Height = 24;
            this.gridAutoNoDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAutoNoDetail.ShowCellToolTips = false;
            this.gridAutoNoDetail.Size = new System.Drawing.Size(1058, 113);
            this.gridAutoNoDetail.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 2;
            this.label4.Text = "Auto";
            this.label4.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // splitTop
            // 
            this.splitTop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitTop.Location = new System.Drawing.Point(5, 3);
            this.splitTop.Name = "splitTop";
            // 
            // splitTop.Panel1
            // 
            this.splitTop.Panel1.Controls.Add(this.gridManualMain);
            this.splitTop.Panel1.Controls.Add(this.numManualOPLoading);
            this.splitTop.Panel1.Controls.Add(this.numManualLBR);
            this.splitTop.Panel1.Controls.Add(this.numManualSewerManpower);
            this.splitTop.Panel1.Controls.Add(this.label5);
            this.splitTop.Panel1.Controls.Add(this.label6);
            this.splitTop.Panel1.Controls.Add(this.label7);
            this.splitTop.Panel1.Controls.Add(this.label1);
            // 
            // splitTop.Panel2
            // 
            this.splitTop.Panel2.Controls.Add(this.gridAutoMain);
            this.splitTop.Panel2.Controls.Add(this.numAutoOPLoading);
            this.splitTop.Panel2.Controls.Add(this.numAutoLBR);
            this.splitTop.Panel2.Controls.Add(this.numAutoSewerManpower);
            this.splitTop.Panel2.Controls.Add(this.label8);
            this.splitTop.Panel2.Controls.Add(this.label9);
            this.splitTop.Panel2.Controls.Add(this.label10);
            this.splitTop.Panel2.Controls.Add(this.label2);
            this.splitTop.Size = new System.Drawing.Size(1133, 405);
            this.splitTop.SplitterDistance = 566;
            this.splitTop.TabIndex = 0;
            // 
            // gridManualMain
            // 
            this.gridManualMain.AllowUserToAddRows = false;
            this.gridManualMain.AllowUserToDeleteRows = false;
            this.gridManualMain.AllowUserToResizeRows = false;
            this.gridManualMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridManualMain.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridManualMain.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridManualMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridManualMain.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridManualMain.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridManualMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridManualMain.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridManualMain.Location = new System.Drawing.Point(3, 26);
            this.gridManualMain.Name = "gridManualMain";
            this.gridManualMain.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridManualMain.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridManualMain.RowTemplate.Height = 24;
            this.gridManualMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridManualMain.ShowCellToolTips = false;
            this.gridManualMain.Size = new System.Drawing.Size(560, 376);
            this.gridManualMain.TabIndex = 13;
            // 
            // numManualOPLoading
            // 
            this.numManualOPLoading.BackColor = System.Drawing.Color.White;
            this.numManualOPLoading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numManualOPLoading.Location = new System.Drawing.Point(474, 0);
            this.numManualOPLoading.Name = "numManualOPLoading";
            this.numManualOPLoading.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numManualOPLoading.Size = new System.Drawing.Size(48, 23);
            this.numManualOPLoading.TabIndex = 12;
            this.numManualOPLoading.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numManualLBR
            // 
            this.numManualLBR.BackColor = System.Drawing.Color.White;
            this.numManualLBR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numManualLBR.Location = new System.Drawing.Point(256, 0);
            this.numManualLBR.Name = "numManualLBR";
            this.numManualLBR.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numManualLBR.Size = new System.Drawing.Size(48, 23);
            this.numManualLBR.TabIndex = 11;
            this.numManualLBR.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numManualSewerManpower
            // 
            this.numManualSewerManpower.BackColor = System.Drawing.Color.White;
            this.numManualSewerManpower.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numManualSewerManpower.Location = new System.Drawing.Point(157, 0);
            this.numManualSewerManpower.Name = "numManualSewerManpower";
            this.numManualSewerManpower.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numManualSewerManpower.Size = new System.Drawing.Size(48, 23);
            this.numManualSewerManpower.TabIndex = 10;
            this.numManualSewerManpower.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label5.Location = new System.Drawing.Point(306, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(168, 23);
            this.label5.TabIndex = 9;
            this.label5.Text = "Highest Operator Loading(%)";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label6.Location = new System.Drawing.Point(207, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 23);
            this.label6.TabIndex = 8;
            this.label6.Text = "LBR(%)";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label7.Location = new System.Drawing.Point(75, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 23);
            this.label7.TabIndex = 7;
            this.label7.Text = "No. of Sewer";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Manual";
            this.label1.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // gridAutoMain
            // 
            this.gridAutoMain.AllowUserToAddRows = false;
            this.gridAutoMain.AllowUserToDeleteRows = false;
            this.gridAutoMain.AllowUserToResizeRows = false;
            this.gridAutoMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridAutoMain.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridAutoMain.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridAutoMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAutoMain.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridAutoMain.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridAutoMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridAutoMain.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridAutoMain.Location = new System.Drawing.Point(3, 26);
            this.gridAutoMain.Name = "gridAutoMain";
            this.gridAutoMain.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridAutoMain.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridAutoMain.RowTemplate.Height = 24;
            this.gridAutoMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAutoMain.ShowCellToolTips = false;
            this.gridAutoMain.Size = new System.Drawing.Size(557, 376);
            this.gridAutoMain.TabIndex = 14;
            // 
            // numAutoOPLoading
            // 
            this.numAutoOPLoading.BackColor = System.Drawing.Color.White;
            this.numAutoOPLoading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numAutoOPLoading.Location = new System.Drawing.Point(474, 0);
            this.numAutoOPLoading.Name = "numAutoOPLoading";
            this.numAutoOPLoading.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAutoOPLoading.Size = new System.Drawing.Size(48, 23);
            this.numAutoOPLoading.TabIndex = 18;
            this.numAutoOPLoading.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numAutoLBR
            // 
            this.numAutoLBR.BackColor = System.Drawing.Color.White;
            this.numAutoLBR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numAutoLBR.Location = new System.Drawing.Point(256, 0);
            this.numAutoLBR.Name = "numAutoLBR";
            this.numAutoLBR.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAutoLBR.Size = new System.Drawing.Size(48, 23);
            this.numAutoLBR.TabIndex = 17;
            this.numAutoLBR.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numAutoSewerManpower
            // 
            this.numAutoSewerManpower.BackColor = System.Drawing.Color.White;
            this.numAutoSewerManpower.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numAutoSewerManpower.Location = new System.Drawing.Point(157, 0);
            this.numAutoSewerManpower.Name = "numAutoSewerManpower";
            this.numAutoSewerManpower.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAutoSewerManpower.Size = new System.Drawing.Size(48, 23);
            this.numAutoSewerManpower.TabIndex = 16;
            this.numAutoSewerManpower.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label8.Location = new System.Drawing.Point(306, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(168, 23);
            this.label8.TabIndex = 15;
            this.label8.Text = "Highest Operator Loading(%)";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label9.Location = new System.Drawing.Point(207, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 23);
            this.label9.TabIndex = 14;
            this.label9.Text = "LBR(%)";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label10.Location = new System.Drawing.Point(75, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(82, 23);
            this.label10.TabIndex = 13;
            this.label10.Text = "No. of Sewer";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Auto";
            this.label2.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // tabSewerMinus1
            // 
            this.tabSewerMinus1.Location = new System.Drawing.Point(4, 25);
            this.tabSewerMinus1.Name = "tabSewerMinus1";
            this.tabSewerMinus1.Padding = new System.Windows.Forms.Padding(3);
            this.tabSewerMinus1.Size = new System.Drawing.Size(1149, 658);
            this.tabSewerMinus1.TabIndex = 1;
            this.tabSewerMinus1.Text = "-1 Sewer";
            // 
            // tabSewerManualInput
            // 
            this.tabSewerManualInput.Location = new System.Drawing.Point(4, 25);
            this.tabSewerManualInput.Name = "tabSewerManualInput";
            this.tabSewerManualInput.Size = new System.Drawing.Size(1149, 658);
            this.tabSewerManualInput.TabIndex = 2;
            this.tabSewerManualInput.Text = "Manual Input";
            // 
            // tabSewerPlus1
            // 
            this.tabSewerPlus1.Location = new System.Drawing.Point(4, 25);
            this.tabSewerPlus1.Name = "tabSewerPlus1";
            this.tabSewerPlus1.Size = new System.Drawing.Size(1149, 658);
            this.tabSewerPlus1.TabIndex = 3;
            this.tabSewerPlus1.Text = "+1 Sewer";
            // 
            // tabSewerPlus2
            // 
            this.tabSewerPlus2.Location = new System.Drawing.Point(4, 25);
            this.tabSewerPlus2.Name = "tabSewerPlus2";
            this.tabSewerPlus2.Size = new System.Drawing.Size(1149, 658);
            this.tabSewerPlus2.TabIndex = 4;
            this.tabSewerPlus2.Text = "+2 Sewer";
            // 
            // P05_Compare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1157, 687);
            this.Controls.Add(this.tabCompare);
            this.Name = "P05_Compare";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P05. Compare Manual and Auto Line Mapping";
            this.Controls.SetChildIndex(this.tabCompare, 0);
            this.tabCompare.ResumeLayout(false);
            this.tabSewerMinus2.ResumeLayout(false);
            this.panelDisplay.ResumeLayout(false);
            this.splitBottom.Panel1.ResumeLayout(false);
            this.splitBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBottom)).EndInit();
            this.splitBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridManualNoDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAutoNoDetail)).EndInit();
            this.splitTop.Panel1.ResumeLayout(false);
            this.splitTop.Panel1.PerformLayout();
            this.splitTop.Panel2.ResumeLayout(false);
            this.splitTop.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitTop)).EndInit();
            this.splitTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridManualMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAutoMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAutoDetailBs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridManualDetailBs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.TabControl tabCompare;
        private System.Windows.Forms.TabPage tabSewerMinus2;
        private System.Windows.Forms.TabPage tabSewerMinus1;
        private System.Windows.Forms.TabPage tabSewerManualInput;
        private System.Windows.Forms.TabPage tabSewerPlus1;
        private System.Windows.Forms.TabPage tabSewerPlus2;
        private Win.UI.Panel panelDisplay;
        private System.Windows.Forms.SplitContainer splitBottom;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private System.Windows.Forms.SplitContainer splitTop;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.NumericBox numManualOPLoading;
        private Win.UI.NumericBox numManualLBR;
        private Win.UI.NumericBox numManualSewerManpower;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.NumericBox numAutoOPLoading;
        private Win.UI.NumericBox numAutoLBR;
        private Win.UI.NumericBox numAutoSewerManpower;
        private Win.UI.Label label8;
        private Win.UI.Label label9;
        private Win.UI.Label label10;
        private Win.UI.Grid gridManualNoDetail;
        private Win.UI.Grid gridAutoNoDetail;
        private Win.UI.Grid gridManualMain;
        private Win.UI.Grid gridAutoMain;
        private Win.UI.ListControlBindingSource gridAutoDetailBs;
        private Win.UI.ListControlBindingSource gridManualDetailBs;
    }
}