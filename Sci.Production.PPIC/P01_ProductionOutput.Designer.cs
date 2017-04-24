﻿namespace Sci.Production.PPIC
{
    partial class P01_ProductionOutput
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.label1 = new Sci.Win.UI.Label();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.tabControl1 = new Sci.Win.UI.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel7 = new Sci.Win.UI.Panel();
            this.gridSewingOutput = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel6 = new Sci.Win.UI.Panel();
            this.numSewingQty = new Sci.Win.UI.NumericBox();
            this.numSewingOrderQty = new Sci.Win.UI.NumericBox();
            this.label5 = new Sci.Win.UI.Label();
            this.dateLastSewingOutputDate = new Sci.Win.UI.DateBox();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel9 = new Sci.Win.UI.Panel();
            this.gridCutting = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel8 = new Sci.Win.UI.Panel();
            this.numCuttingQty = new Sci.Win.UI.NumericBox();
            this.numOrderQty = new Sci.Win.UI.NumericBox();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSewingOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel6.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCutting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 429);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(614, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 429);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(609, 5);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 389);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(609, 40);
            this.panel4.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(8, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Dbl Click on < Q\'ty > for Details";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Blue;
            this.label1.TextStyle.Color = System.Drawing.Color.Blue;
            this.label1.TextStyle.ExtBorderColor = System.Drawing.Color.Blue;
            this.label1.TextStyle.GradientColor = System.Drawing.Color.Blue;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(514, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.tabControl1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(609, 384);
            this.panel5.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(609, 384);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel7);
            this.tabPage1.Controls.Add(this.panel6);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(601, 355);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Sewing output";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.gridSewingOutput);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(595, 290);
            this.panel7.TabIndex = 1;
            // 
            // gridSewingOutput
            // 
            this.gridSewingOutput.AllowUserToAddRows = false;
            this.gridSewingOutput.AllowUserToDeleteRows = false;
            this.gridSewingOutput.AllowUserToResizeRows = false;
            this.gridSewingOutput.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSewingOutput.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSewingOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSewingOutput.DataSource = this.listControlBindingSource1;
            this.gridSewingOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSewingOutput.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSewingOutput.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSewingOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSewingOutput.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSewingOutput.Location = new System.Drawing.Point(0, 0);
            this.gridSewingOutput.Name = "gridSewingOutput";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridSewingOutput.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridSewingOutput.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSewingOutput.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSewingOutput.RowTemplate.Height = 24;
            this.gridSewingOutput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSewingOutput.Size = new System.Drawing.Size(595, 290);
            this.gridSewingOutput.TabIndex = 0;
            this.gridSewingOutput.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.numSewingQty);
            this.panel6.Controls.Add(this.numSewingOrderQty);
            this.panel6.Controls.Add(this.label5);
            this.panel6.Controls.Add(this.dateLastSewingOutputDate);
            this.panel6.Controls.Add(this.label4);
            this.panel6.Controls.Add(this.label3);
            this.panel6.Controls.Add(this.label2);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(3, 293);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(595, 59);
            this.panel6.TabIndex = 0;
            // 
            // numSewingQty
            // 
            this.numSewingQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numSewingQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numSewingQty.IsSupportEditMode = false;
            this.numSewingQty.Location = new System.Drawing.Point(274, 29);
            this.numSewingQty.Name = "numSewingQty";
            this.numSewingQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSewingQty.ReadOnly = true;
            this.numSewingQty.Size = new System.Drawing.Size(75, 23);
            this.numSewingQty.TabIndex = 6;
            this.numSewingQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSewingQty.DoubleClick += new System.EventHandler(this.numericBox2_DoubleClick);
            // 
            // numSewingOrderQty
            // 
            this.numSewingOrderQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numSewingOrderQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numSewingOrderQty.IsSupportEditMode = false;
            this.numSewingOrderQty.Location = new System.Drawing.Point(184, 29);
            this.numSewingOrderQty.Name = "numSewingOrderQty";
            this.numSewingOrderQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSewingOrderQty.ReadOnly = true;
            this.numSewingOrderQty.Size = new System.Drawing.Size(75, 23);
            this.numSewingOrderQty.TabIndex = 5;
            this.numSewingOrderQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(133, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Total";
            this.label5.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.label5.TextStyle.Color = System.Drawing.Color.Red;
            this.label5.TextStyle.ExtBorderColor = System.Drawing.Color.Red;
            this.label5.TextStyle.GradientColor = System.Drawing.Color.Red;
            // 
            // dateLastSewingOutputDate
            // 
            this.dateLastSewingOutputDate.IsSupportEditMode = false;
            this.dateLastSewingOutputDate.Location = new System.Drawing.Point(12, 29);
            this.dateLastSewingOutputDate.Name = "dateLastSewingOutputDate";
            this.dateLastSewingOutputDate.ReadOnly = true;
            this.dateLastSewingOutputDate.Size = new System.Drawing.Size(100, 23);
            this.dateLastSewingOutputDate.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(271, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 23);
            this.label4.TabIndex = 2;
            this.label4.Text = "Sewing Q\'ty";
            this.label4.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            this.label4.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label4.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(184, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "Order Q\'ty";
            this.label3.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            this.label3.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label3.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Last Sewing Output Date";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            this.label2.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel9);
            this.tabPage2.Controls.Add(this.panel8);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(601, 355);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "-(Cutting(Comb";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.gridCutting);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(3, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(595, 290);
            this.panel9.TabIndex = 1;
            // 
            // gridCutting
            // 
            this.gridCutting.AllowUserToAddRows = false;
            this.gridCutting.AllowUserToDeleteRows = false;
            this.gridCutting.AllowUserToResizeRows = false;
            this.gridCutting.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCutting.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCutting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCutting.DataSource = this.listControlBindingSource2;
            this.gridCutting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCutting.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCutting.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCutting.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCutting.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCutting.Location = new System.Drawing.Point(0, 0);
            this.gridCutting.Name = "gridCutting";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridCutting.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridCutting.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCutting.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCutting.RowTemplate.Height = 24;
            this.gridCutting.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCutting.Size = new System.Drawing.Size(595, 290);
            this.gridCutting.TabIndex = 0;
            this.gridCutting.TabStop = false;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.numCuttingQty);
            this.panel8.Controls.Add(this.numOrderQty);
            this.panel8.Controls.Add(this.label6);
            this.panel8.Controls.Add(this.label7);
            this.panel8.Controls.Add(this.label8);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(3, 293);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(595, 59);
            this.panel8.TabIndex = 0;
            // 
            // numCuttingQty
            // 
            this.numCuttingQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numCuttingQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numCuttingQty.IsSupportEditMode = false;
            this.numCuttingQty.Location = new System.Drawing.Point(271, 29);
            this.numCuttingQty.Name = "numCuttingQty";
            this.numCuttingQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCuttingQty.ReadOnly = true;
            this.numCuttingQty.Size = new System.Drawing.Size(75, 23);
            this.numCuttingQty.TabIndex = 13;
            this.numCuttingQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCuttingQty.DoubleClick += new System.EventHandler(this.numericBox3_DoubleClick);
            // 
            // numOrderQty
            // 
            this.numOrderQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numOrderQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numOrderQty.IsSupportEditMode = false;
            this.numOrderQty.Location = new System.Drawing.Point(184, 29);
            this.numOrderQty.Name = "numOrderQty";
            this.numOrderQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numOrderQty.ReadOnly = true;
            this.numOrderQty.Size = new System.Drawing.Size(75, 23);
            this.numOrderQty.TabIndex = 12;
            this.numOrderQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(133, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 23);
            this.label6.TabIndex = 11;
            this.label6.Text = "Total";
            this.label6.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.label6.TextStyle.Color = System.Drawing.Color.Red;
            this.label6.TextStyle.ExtBorderColor = System.Drawing.Color.Red;
            this.label6.TextStyle.GradientColor = System.Drawing.Color.Red;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(271, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 23);
            this.label7.TabIndex = 9;
            this.label7.Text = "Cutting Q\'ty";
            this.label7.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            this.label7.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(184, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 23);
            this.label8.TabIndex = 8;
            this.label8.Text = "Order Q\'ty";
            this.label8.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label8.TextStyle.Color = System.Drawing.Color.Black;
            this.label8.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label8.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // P01_ProductionOutput
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(619, 429);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P01_ProductionOutput";
            this.Text = "Production output - ";
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSewingOutput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCutting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
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
        private System.Windows.Forms.TabPage tabPage2;
        private Win.UI.Label label1;
        private Win.UI.Button btnClose;
        private Win.UI.Panel panel7;
        private Win.UI.Grid gridSewingOutput;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel6;
        private Win.UI.NumericBox numSewingQty;
        private Win.UI.NumericBox numSewingOrderQty;
        private Win.UI.Label label5;
        private Win.UI.DateBox dateLastSewingOutputDate;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Panel panel9;
        private Win.UI.Grid gridCutting;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Panel panel8;
        private Win.UI.NumericBox numCuttingQty;
        private Win.UI.NumericBox numOrderQty;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.Label label8;
    }
}
