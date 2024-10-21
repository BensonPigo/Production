﻿namespace Sci.Production.Class.Controls
{
    partial class CalendarGrid
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

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridCalendar = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelday4 = new System.Windows.Forms.Label();
            this.labelday5 = new System.Windows.Forms.Label();
            this.labelday6 = new System.Windows.Forms.Label();
            this.labelday3 = new System.Windows.Forms.Label();
            this.labelday0 = new System.Windows.Forms.Label();
            this.labelday1 = new System.Windows.Forms.Label();
            this.labelday2 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.Start1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.End1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Start2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.End2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Start3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.End3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Start4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.End4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Start5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.End5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Start6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.End6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Start7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.End7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridCalendar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridCalendar
            // 
            this.gridCalendar.AllowUserToAddRows = false;
            this.gridCalendar.AllowUserToDeleteRows = false;
            this.gridCalendar.AllowUserToResizeRows = false;
            this.gridCalendar.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCalendar.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridCalendar.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridCalendar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCalendar.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Start1,
            this.End1,
            this.Start2,
            this.End2,
            this.Start3,
            this.End3,
            this.Start4,
            this.End4,
            this.Start5,
            this.End5,
            this.Start6,
            this.End6,
            this.Start7,
            this.End7});
            this.gridCalendar.DataSource = this.listControlBindingSource1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridCalendar.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridCalendar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCalendar.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCalendar.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCalendar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCalendar.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCalendar.Location = new System.Drawing.Point(0, 24);
            this.gridCalendar.Margin = new System.Windows.Forms.Padding(0);
            this.gridCalendar.Name = "gridCalendar";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridCalendar.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridCalendar.RowHeadersVisible = false;
            this.gridCalendar.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCalendar.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCalendar.RowTemplate.Height = 24;
            this.gridCalendar.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridCalendar.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridCalendar.ShowCellToolTips = false;
            this.gridCalendar.Size = new System.Drawing.Size(668, 298);
            this.gridCalendar.TabIndex = 58;
            this.gridCalendar.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.GridCalendar_CellFormatting);
            this.gridCalendar.SizeChanged += new System.EventHandler(this.GridCalendar_SizeChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.Controls.Add(this.labelday4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelday5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelday6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelday3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelday0, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelday1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelday2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(668, 24);
            this.tableLayoutPanel1.TabIndex = 59;
            // 
            // labelday4
            // 
            this.labelday4.AutoSize = true;
            this.labelday4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelday4.Location = new System.Drawing.Point(380, 0);
            this.labelday4.Margin = new System.Windows.Forms.Padding(0);
            this.labelday4.Name = "labelday4";
            this.labelday4.Size = new System.Drawing.Size(95, 24);
            this.labelday4.TabIndex = 60;
            this.labelday4.Text = "Thursday";
            this.labelday4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelday5
            // 
            this.labelday5.AutoSize = true;
            this.labelday5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelday5.Location = new System.Drawing.Point(475, 0);
            this.labelday5.Margin = new System.Windows.Forms.Padding(0);
            this.labelday5.Name = "labelday5";
            this.labelday5.Size = new System.Drawing.Size(95, 24);
            this.labelday5.TabIndex = 59;
            this.labelday5.Text = "Friday";
            this.labelday5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelday6
            // 
            this.labelday6.AutoSize = true;
            this.labelday6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelday6.Location = new System.Drawing.Point(570, 0);
            this.labelday6.Margin = new System.Windows.Forms.Padding(0);
            this.labelday6.Name = "labelday6";
            this.labelday6.Size = new System.Drawing.Size(98, 24);
            this.labelday6.TabIndex = 58;
            this.labelday6.Text = "Saturday";
            this.labelday6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelday3
            // 
            this.labelday3.AutoSize = true;
            this.labelday3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelday3.Location = new System.Drawing.Point(285, 0);
            this.labelday3.Margin = new System.Windows.Forms.Padding(0);
            this.labelday3.Name = "labelday3";
            this.labelday3.Size = new System.Drawing.Size(95, 24);
            this.labelday3.TabIndex = 57;
            this.labelday3.Text = "Wednesday";
            this.labelday3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelday0
            // 
            this.labelday0.AutoSize = true;
            this.labelday0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelday0.Location = new System.Drawing.Point(0, 0);
            this.labelday0.Margin = new System.Windows.Forms.Padding(0);
            this.labelday0.Name = "labelday0";
            this.labelday0.Size = new System.Drawing.Size(95, 24);
            this.labelday0.TabIndex = 56;
            this.labelday0.Text = "Sunday";
            this.labelday0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelday1
            // 
            this.labelday1.AutoSize = true;
            this.labelday1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelday1.Location = new System.Drawing.Point(95, 0);
            this.labelday1.Margin = new System.Windows.Forms.Padding(0);
            this.labelday1.Name = "labelday1";
            this.labelday1.Size = new System.Drawing.Size(95, 24);
            this.labelday1.TabIndex = 55;
            this.labelday1.Text = "Monday";
            this.labelday1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelday2
            // 
            this.labelday2.AutoSize = true;
            this.labelday2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelday2.Location = new System.Drawing.Point(190, 0);
            this.labelday2.Margin = new System.Windows.Forms.Padding(0);
            this.labelday2.Name = "labelday2";
            this.labelday2.Size = new System.Drawing.Size(95, 24);
            this.labelday2.TabIndex = 54;
            this.labelday2.Text = "Tuesday";
            this.labelday2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Start1
            // 
            this.Start1.DataPropertyName = "Start1";
            this.Start1.HeaderText = "Start";
            this.Start1.Name = "Start1";
            this.Start1.ReadOnly = true;
            this.Start1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Start1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Start1.Width = 45;
            // 
            // End1
            // 
            this.End1.DataPropertyName = "End1";
            this.End1.HeaderText = "End";
            this.End1.Name = "End1";
            this.End1.ReadOnly = true;
            this.End1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.End1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.End1.Width = 45;
            // 
            // Start2
            // 
            this.Start2.DataPropertyName = "Start2";
            this.Start2.HeaderText = "Start";
            this.Start2.Name = "Start2";
            this.Start2.ReadOnly = true;
            this.Start2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Start2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Start2.Width = 45;
            // 
            // End2
            // 
            this.End2.DataPropertyName = "End2";
            this.End2.HeaderText = "End";
            this.End2.Name = "End2";
            this.End2.ReadOnly = true;
            this.End2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.End2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.End2.Width = 45;
            // 
            // Start3
            // 
            this.Start3.DataPropertyName = "Start3";
            this.Start3.HeaderText = "Start";
            this.Start3.Name = "Start3";
            this.Start3.ReadOnly = true;
            this.Start3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Start3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Start3.Width = 45;
            // 
            // End3
            // 
            this.End3.DataPropertyName = "End3";
            this.End3.HeaderText = "End";
            this.End3.Name = "End3";
            this.End3.ReadOnly = true;
            this.End3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.End3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.End3.Width = 45;
            // 
            // Start4
            // 
            this.Start4.DataPropertyName = "Start4";
            this.Start4.HeaderText = "Start";
            this.Start4.Name = "Start4";
            this.Start4.ReadOnly = true;
            this.Start4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Start4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Start4.Width = 45;
            // 
            // End4
            // 
            this.End4.DataPropertyName = "End4";
            this.End4.HeaderText = "End";
            this.End4.Name = "End4";
            this.End4.ReadOnly = true;
            this.End4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.End4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.End4.Width = 45;
            // 
            // Start5
            // 
            this.Start5.DataPropertyName = "Start5";
            this.Start5.HeaderText = "Start";
            this.Start5.Name = "Start5";
            this.Start5.ReadOnly = true;
            this.Start5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Start5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Start5.Width = 45;
            // 
            // End5
            // 
            this.End5.DataPropertyName = "End5";
            this.End5.HeaderText = "End";
            this.End5.Name = "End5";
            this.End5.ReadOnly = true;
            this.End5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.End5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.End5.Width = 45;
            // 
            // Start6
            // 
            this.Start6.DataPropertyName = "Start6";
            this.Start6.HeaderText = "Start";
            this.Start6.Name = "Start6";
            this.Start6.ReadOnly = true;
            this.Start6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Start6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Start6.Width = 45;
            // 
            // End6
            // 
            this.End6.DataPropertyName = "End6";
            this.End6.HeaderText = "End";
            this.End6.Name = "End6";
            this.End6.ReadOnly = true;
            this.End6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.End6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.End6.Width = 45;
            // 
            // Start7
            // 
            this.Start7.DataPropertyName = "Start7";
            this.Start7.HeaderText = "Start";
            this.Start7.Name = "Start7";
            this.Start7.ReadOnly = true;
            this.Start7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Start7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Start7.Width = 45;
            // 
            // End7
            // 
            this.End7.DataPropertyName = "End7";
            this.End7.HeaderText = "End";
            this.End7.Name = "End7";
            this.End7.ReadOnly = true;
            this.End7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.End7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.End7.Width = 45;
            // 
            // CalendarGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridCalendar);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CalendarGrid";
            this.Size = new System.Drawing.Size(668, 322);
            ((System.ComponentModel.ISupportInitialize)(this.gridCalendar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public Win.UI.Grid gridCalendar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelday4;
        private System.Windows.Forms.Label labelday5;
        private System.Windows.Forms.Label labelday6;
        private System.Windows.Forms.Label labelday3;
        private System.Windows.Forms.Label labelday0;
        private System.Windows.Forms.Label labelday1;
        private System.Windows.Forms.Label labelday2;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Start1;
        private System.Windows.Forms.DataGridViewTextBoxColumn End1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Start2;
        private System.Windows.Forms.DataGridViewTextBoxColumn End2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Start3;
        private System.Windows.Forms.DataGridViewTextBoxColumn End3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Start4;
        private System.Windows.Forms.DataGridViewTextBoxColumn End4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Start5;
        private System.Windows.Forms.DataGridViewTextBoxColumn End5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Start6;
        private System.Windows.Forms.DataGridViewTextBoxColumn End6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Start7;
        private System.Windows.Forms.DataGridViewTextBoxColumn End7;
    }
}
