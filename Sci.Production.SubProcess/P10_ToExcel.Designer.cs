namespace Sci.Production.SubProcess
{
    partial class P10_ToExcel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.labelReport = new Sci.Win.UI.Label();
            this.labelDate = new Sci.Win.UI.Label();
            this.dateRange = new Sci.Win.UI.DateRange();
            this.radioBtnCapacityFillRate = new Sci.Win.UI.RadioButton();
            this.radioBtnGroupFillRate = new Sci.Win.UI.RadioButton();
            this.listControlBindingSourceGroup = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.grid = new Sci.Win.UI.Grid();
            this.listControlBindingSourceCapacity = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.radioPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSourceGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSourceCapacity)).BeginInit();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(393, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(393, 48);
            this.toexcel.TabIndex = 0;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(393, 84);
            this.close.TabIndex = 1;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.labelReport);
            this.radioPanel1.Controls.Add(this.labelDate);
            this.radioPanel1.Controls.Add(this.dateRange);
            this.radioPanel1.Controls.Add(this.radioBtnCapacityFillRate);
            this.radioPanel1.Controls.Add(this.radioBtnGroupFillRate);
            this.radioPanel1.Location = new System.Drawing.Point(12, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(375, 277);
            this.radioPanel1.TabIndex = 94;
            // 
            // labelReport
            // 
            this.labelReport.Location = new System.Drawing.Point(4, 36);
            this.labelReport.Name = "labelReport";
            this.labelReport.Size = new System.Drawing.Size(75, 23);
            this.labelReport.TabIndex = 3;
            this.labelReport.Text = "Report";
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(4, 7);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(75, 23);
            this.labelDate.TabIndex = 3;
            this.labelDate.Text = "Date";
            // 
            // dateRange
            // 
            // 
            // 
            // 
            this.dateRange.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRange.DateBox1.Name = "";
            this.dateRange.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRange.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRange.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRange.DateBox2.Name = "";
            this.dateRange.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRange.DateBox2.TabIndex = 1;
            this.dateRange.Location = new System.Drawing.Point(82, 7);
            this.dateRange.Name = "dateRange";
            this.dateRange.Size = new System.Drawing.Size(280, 23);
            this.dateRange.TabIndex = 0;
            // 
            // radioBtnCapacityFillRate
            // 
            this.radioBtnCapacityFillRate.AutoSize = true;
            this.radioBtnCapacityFillRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBtnCapacityFillRate.Location = new System.Drawing.Point(82, 64);
            this.radioBtnCapacityFillRate.Name = "radioBtnCapacityFillRate";
            this.radioBtnCapacityFillRate.Size = new System.Drawing.Size(163, 21);
            this.radioBtnCapacityFillRate.TabIndex = 2;
            this.radioBtnCapacityFillRate.Text = "Capacity and Fill Rate";
            this.radioBtnCapacityFillRate.UseVisualStyleBackColor = true;
            this.radioBtnCapacityFillRate.CheckedChanged += new System.EventHandler(this.RadioBtnCapacityFillRate_CheckedChanged);
            // 
            // radioBtnGroupFillRate
            // 
            this.radioBtnGroupFillRate.AutoSize = true;
            this.radioBtnGroupFillRate.Checked = true;
            this.radioBtnGroupFillRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBtnGroupFillRate.Location = new System.Drawing.Point(82, 36);
            this.radioBtnGroupFillRate.Name = "radioBtnGroupFillRate";
            this.radioBtnGroupFillRate.Size = new System.Drawing.Size(121, 21);
            this.radioBtnGroupFillRate.TabIndex = 1;
            this.radioBtnGroupFillRate.TabStop = true;
            this.radioBtnGroupFillRate.Text = "Group Fill Rate";
            this.radioBtnGroupFillRate.UseVisualStyleBackColor = true;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.DataSource = this.listControlBindingSourceGroup;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(480, 12);
            this.grid.Name = "grid";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(286, 277);
            this.grid.TabIndex = 2;
            // 
            // P10_ToExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 314);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.radioPanel1);
            this.Name = "P10_ToExcel";
            this.Text = "P10_toExcel. PPA Schedule Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSourceGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSourceCapacity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.Label labelReport;
        private Win.UI.Label labelDate;
        private Win.UI.DateRange dateRange;
        private Win.UI.RadioButton radioBtnCapacityFillRate;
        private Win.UI.RadioButton radioBtnGroupFillRate;
        private Win.UI.ListControlBindingSource listControlBindingSourceGroup;
        private Win.UI.Grid grid;
        private Win.UI.ListControlBindingSource listControlBindingSourceCapacity;
    }
}