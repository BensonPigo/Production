namespace Sci.Production.Warehouse
{
    partial class P33_ThreadColorCombination
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridLeft = new Sci.Win.UI.Grid();
            this.gridRight = new Sci.Win.UI.Grid();
            this.Left_Datasource = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.Right_DataSource = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Left_Datasource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Right_DataSource)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridLeft);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridRight);
            this.splitContainer1.Size = new System.Drawing.Size(1023, 517);
            this.splitContainer1.SplitterDistance = 615;
            this.splitContainer1.TabIndex = 0;
            // 
            // gridLeft
            // 
            this.gridLeft.AllowUserToAddRows = false;
            this.gridLeft.AllowUserToDeleteRows = false;
            this.gridLeft.AllowUserToResizeRows = false;
            this.gridLeft.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridLeft.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridLeft.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLeft.DataSource = this.Left_Datasource;
            this.gridLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridLeft.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridLeft.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridLeft.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridLeft.Location = new System.Drawing.Point(0, 0);
            this.gridLeft.Name = "gridLeft";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridLeft.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridLeft.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridLeft.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridLeft.RowTemplate.Height = 24;
            this.gridLeft.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLeft.ShowCellToolTips = false;
            this.gridLeft.Size = new System.Drawing.Size(615, 517);
            this.gridLeft.TabIndex = 0;
            // 
            // gridRight
            // 
            this.gridRight.AllowUserToAddRows = false;
            this.gridRight.AllowUserToDeleteRows = false;
            this.gridRight.AllowUserToResizeRows = false;
            this.gridRight.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridRight.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridRight.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridRight.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRight.DataSource = this.Right_DataSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridRight.DefaultCellStyle = dataGridViewCellStyle3;
            this.gridRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRight.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridRight.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridRight.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridRight.Location = new System.Drawing.Point(0, 0);
            this.gridRight.Name = "gridRight";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridRight.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridRight.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridRight.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridRight.RowTemplate.Height = 24;
            this.gridRight.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRight.ShowCellToolTips = false;
            this.gridRight.Size = new System.Drawing.Size(404, 517);
            this.gridRight.TabIndex = 0;
            // 
            // P33_ThreadColorCombination
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1023, 517);
            this.Controls.Add(this.splitContainer1);
            this.Name = "P33_ThreadColorCombination";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P33. Thread Color Combination";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Left_Datasource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Right_DataSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Grid gridLeft;
        private Win.UI.Grid gridRight;
        private Win.UI.ListControlBindingSource Left_Datasource;
        private Win.UI.ListControlBindingSource Right_DataSource;
    }
}