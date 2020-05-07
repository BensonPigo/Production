namespace Sci.Production.Quality
{
    partial class P31
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.CoverPanel = new Sci.Win.UI.Panel();
            this.tabControl = new Sci.Win.UI.TabControl();
            this.tab_QtyBreakdown = new System.Windows.Forms.TabPage();
            this.tab_CartonSummary = new System.Windows.Forms.TabPage();
            this.gridQtyBreakdown = new Sci.Win.UI.Grid();
            this.gridCartonSummary = new Sci.Win.UI.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).BeginInit();
            this.masterpanel.SuspendLayout();
            this.detailpanel.SuspendLayout();
            this.detailgridcont.SuspendLayout();
            this.detail2.SuspendLayout();
            this.detailpanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.CoverPanel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tab_QtyBreakdown.SuspendLayout();
            this.tab_CartonSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridQtyBreakdown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCartonSummary)).BeginInit();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Size = new System.Drawing.Size(792, 100);
            // 
            // detailpanel
            // 
            this.detailpanel.Size = new System.Drawing.Size(792, 250);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Controls.Add(this.CoverPanel);
            this.detailgridcont.Size = new System.Drawing.Size(792, 250);
            this.detailgridcont.Controls.SetChildIndex(this.CoverPanel, 0);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(892, 387);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(886, 341);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(886, 40);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(792, 388);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(792, 350);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 350);
            this.detailbtm.Size = new System.Drawing.Size(792, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(792, 388);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(800, 417);
            // 
            // CoverPanel
            // 
            this.CoverPanel.Controls.Add(this.tabControl);
            this.CoverPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CoverPanel.Location = new System.Drawing.Point(0, 0);
            this.CoverPanel.Name = "CoverPanel";
            this.CoverPanel.Size = new System.Drawing.Size(792, 250);
            this.CoverPanel.TabIndex = 2;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tab_QtyBreakdown);
            this.tabControl.Controls.Add(this.tab_CartonSummary);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(792, 250);
            this.tabControl.TabIndex = 0;
            // 
            // tab_QtyBreakdown
            // 
            this.tab_QtyBreakdown.Controls.Add(this.gridQtyBreakdown);
            this.tab_QtyBreakdown.Location = new System.Drawing.Point(4, 25);
            this.tab_QtyBreakdown.Name = "tab_QtyBreakdown";
            this.tab_QtyBreakdown.Padding = new System.Windows.Forms.Padding(3);
            this.tab_QtyBreakdown.Size = new System.Drawing.Size(784, 221);
            this.tab_QtyBreakdown.TabIndex = 0;
            this.tab_QtyBreakdown.Text = "Qty breakdown";
            // 
            // tab_CartonSummary
            // 
            this.tab_CartonSummary.Controls.Add(this.gridCartonSummary);
            this.tab_CartonSummary.Location = new System.Drawing.Point(4, 25);
            this.tab_CartonSummary.Name = "tab_CartonSummary";
            this.tab_CartonSummary.Padding = new System.Windows.Forms.Padding(3);
            this.tab_CartonSummary.Size = new System.Drawing.Size(784, 221);
            this.tab_CartonSummary.TabIndex = 1;
            this.tab_CartonSummary.Text = "By Carton Summary";
            // 
            // gridQtyBreakdown
            // 
            this.gridQtyBreakdown.AllowUserToAddRows = false;
            this.gridQtyBreakdown.AllowUserToDeleteRows = false;
            this.gridQtyBreakdown.AllowUserToResizeRows = false;
            this.gridQtyBreakdown.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridQtyBreakdown.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridQtyBreakdown.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.gridQtyBreakdown.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridQtyBreakdown.DefaultCellStyle = dataGridViewCellStyle8;
            this.gridQtyBreakdown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridQtyBreakdown.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridQtyBreakdown.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridQtyBreakdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridQtyBreakdown.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridQtyBreakdown.Location = new System.Drawing.Point(3, 3);
            this.gridQtyBreakdown.Name = "gridQtyBreakdown";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridQtyBreakdown.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.gridQtyBreakdown.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridQtyBreakdown.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridQtyBreakdown.RowTemplate.Height = 24;
            this.gridQtyBreakdown.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridQtyBreakdown.ShowCellToolTips = false;
            this.gridQtyBreakdown.Size = new System.Drawing.Size(778, 215);
            this.gridQtyBreakdown.TabIndex = 0;
            // 
            // gridCartonSummary
            // 
            this.gridCartonSummary.AllowUserToAddRows = false;
            this.gridCartonSummary.AllowUserToDeleteRows = false;
            this.gridCartonSummary.AllowUserToResizeRows = false;
            this.gridCartonSummary.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCartonSummary.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridCartonSummary.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.gridCartonSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridCartonSummary.DefaultCellStyle = dataGridViewCellStyle11;
            this.gridCartonSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCartonSummary.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCartonSummary.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCartonSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCartonSummary.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCartonSummary.Location = new System.Drawing.Point(3, 3);
            this.gridCartonSummary.Name = "gridCartonSummary";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridCartonSummary.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.gridCartonSummary.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCartonSummary.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCartonSummary.RowTemplate.Height = 24;
            this.gridCartonSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCartonSummary.ShowCellToolTips = false;
            this.gridCartonSummary.Size = new System.Drawing.Size(778, 215);
            this.gridCartonSummary.TabIndex = 0;
            // 
            // P31
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "P31";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P31. CFA Master List ";
            this.WorkAlias = "Order_QtyShip";
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).EndInit();
            this.masterpanel.ResumeLayout(false);
            this.detailpanel.ResumeLayout(false);
            this.detailgridcont.ResumeLayout(false);
            this.detail2.ResumeLayout(false);
            this.detailpanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.CoverPanel.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tab_QtyBreakdown.ResumeLayout(false);
            this.tab_CartonSummary.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridQtyBreakdown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCartonSummary)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel CoverPanel;
        private Win.UI.TabControl tabControl;
        private System.Windows.Forms.TabPage tab_QtyBreakdown;
        private Win.UI.Grid gridQtyBreakdown;
        private System.Windows.Forms.TabPage tab_CartonSummary;
        private Win.UI.Grid gridCartonSummary;
    }
}