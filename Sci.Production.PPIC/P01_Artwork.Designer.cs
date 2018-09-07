namespace Sci.Production.PPIC
{
    partial class P01_Artwork
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new Sci.Win.UI.TabControl();
            this.Artwork = new System.Windows.Forms.TabPage();
            this.panel1 = new Sci.Win.UI.Panel();
            this.ArtworkGrid = new Sci.Win.UI.Grid();
            this.ArtworkSource = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.CombBySP = new System.Windows.Forms.TabPage();
            this.CombByArtworkType = new System.Windows.Forms.TabPage();
            this.CombBySPSource = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.CombByArtworkTypeSource = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3 = new Sci.Win.UI.Panel();
            this.CombByArtworkGrid = new Sci.Win.UI.Grid();
            this.panel2 = new Sci.Win.UI.Panel();
            this.CombBySPgrid = new Sci.Win.UI.Grid();
            this.tabControl1.SuspendLayout();
            this.Artwork.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ArtworkGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArtworkSource)).BeginInit();
            this.CombBySP.SuspendLayout();
            this.CombByArtworkType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CombBySPSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CombByArtworkTypeSource)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CombByArtworkGrid)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CombBySPgrid)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Artwork);
            this.tabControl1.Controls.Add(this.CombBySP);
            this.tabControl1.Controls.Add(this.CombByArtworkType);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(831, 497);
            this.tabControl1.TabIndex = 0;
            // 
            // Artwork
            // 
            this.Artwork.Controls.Add(this.panel1);
            this.Artwork.Location = new System.Drawing.Point(4, 25);
            this.Artwork.Name = "Artwork";
            this.Artwork.Padding = new System.Windows.Forms.Padding(3);
            this.Artwork.Size = new System.Drawing.Size(823, 468);
            this.Artwork.TabIndex = 0;
            this.Artwork.Text = "Artwork";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ArtworkGrid);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(817, 462);
            this.panel1.TabIndex = 1;
            // 
            // ArtworkGrid
            // 
            this.ArtworkGrid.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.ArtworkGrid.AllowUserToAddRows = false;
            this.ArtworkGrid.AllowUserToDeleteRows = false;
            this.ArtworkGrid.AllowUserToResizeRows = false;
            this.ArtworkGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.ArtworkGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.ArtworkGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ArtworkGrid.DataSource = this.ArtworkSource;
            this.ArtworkGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ArtworkGrid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.ArtworkGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.ArtworkGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ArtworkGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.ArtworkGrid.Location = new System.Drawing.Point(0, 0);
            this.ArtworkGrid.Name = "ArtworkGrid";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ArtworkGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ArtworkGrid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.ArtworkGrid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.ArtworkGrid.RowTemplate.Height = 24;
            this.ArtworkGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ArtworkGrid.ShowCellToolTips = false;
            this.ArtworkGrid.Size = new System.Drawing.Size(817, 462);
            this.ArtworkGrid.TabIndex = 0;
            // 
            // CombBySP
            // 
            this.CombBySP.Controls.Add(this.panel2);
            this.CombBySP.Location = new System.Drawing.Point(4, 25);
            this.CombBySP.Name = "CombBySP";
            this.CombBySP.Padding = new System.Windows.Forms.Padding(3);
            this.CombBySP.Size = new System.Drawing.Size(823, 468);
            this.CombBySP.TabIndex = 1;
            this.CombBySP.Text = "Comb by SP#";
            // 
            // CombByArtworkType
            // 
            this.CombByArtworkType.Controls.Add(this.panel3);
            this.CombByArtworkType.Location = new System.Drawing.Point(4, 25);
            this.CombByArtworkType.Name = "CombByArtworkType";
            this.CombByArtworkType.Padding = new System.Windows.Forms.Padding(3);
            this.CombByArtworkType.Size = new System.Drawing.Size(823, 468);
            this.CombByArtworkType.TabIndex = 2;
            this.CombByArtworkType.Text = "Comb by Artwork Type";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.CombByArtworkGrid);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(817, 462);
            this.panel3.TabIndex = 0;
            // 
            // CombByArtworkGrid
            // 
            this.CombByArtworkGrid.AllowUserToAddRows = false;
            this.CombByArtworkGrid.AllowUserToDeleteRows = false;
            this.CombByArtworkGrid.AllowUserToResizeRows = false;
            this.CombByArtworkGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.CombByArtworkGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CombByArtworkGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.CombByArtworkGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.CombByArtworkGrid.DefaultCellStyle = dataGridViewCellStyle6;
            this.CombByArtworkGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CombByArtworkGrid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.CombByArtworkGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.CombByArtworkGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.CombByArtworkGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.CombByArtworkGrid.Location = new System.Drawing.Point(0, 0);
            this.CombByArtworkGrid.Name = "CombByArtworkGrid";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CombByArtworkGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.CombByArtworkGrid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.CombByArtworkGrid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.CombByArtworkGrid.RowTemplate.Height = 24;
            this.CombByArtworkGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CombByArtworkGrid.ShowCellToolTips = false;
            this.CombByArtworkGrid.Size = new System.Drawing.Size(817, 462);
            this.CombByArtworkGrid.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.CombBySPgrid);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(817, 462);
            this.panel2.TabIndex = 0;
            // 
            // CombBySPgrid
            // 
            this.CombBySPgrid.AllowUserToAddRows = false;
            this.CombBySPgrid.AllowUserToDeleteRows = false;
            this.CombBySPgrid.AllowUserToResizeRows = false;
            this.CombBySPgrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.CombBySPgrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CombBySPgrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.CombBySPgrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.CombBySPgrid.DefaultCellStyle = dataGridViewCellStyle3;
            this.CombBySPgrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CombBySPgrid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.CombBySPgrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.CombBySPgrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.CombBySPgrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.CombBySPgrid.Location = new System.Drawing.Point(0, 0);
            this.CombBySPgrid.Name = "CombBySPgrid";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CombBySPgrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.CombBySPgrid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.CombBySPgrid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.CombBySPgrid.RowTemplate.Height = 24;
            this.CombBySPgrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CombBySPgrid.ShowCellToolTips = false;
            this.CombBySPgrid.Size = new System.Drawing.Size(817, 462);
            this.CombBySPgrid.TabIndex = 0;
            // 
            // P01_ArtwrokNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 497);
            this.Controls.Add(this.tabControl1);
            this.Name = "P01_ArtwrokNew";
            this.Text = "P01_ArtwrokNew";
            this.WorkAlias = "Order_Artwork";
            this.tabControl1.ResumeLayout(false);
            this.Artwork.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ArtworkGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArtworkSource)).EndInit();
            this.CombBySP.ResumeLayout(false);
            this.CombByArtworkType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CombBySPSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CombByArtworkTypeSource)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CombByArtworkGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CombBySPgrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.TabControl tabControl1;
        private System.Windows.Forms.TabPage Artwork;
        private System.Windows.Forms.TabPage CombBySP;
        private System.Windows.Forms.TabPage CombByArtworkType;
        private Win.UI.ListControlBindingSource ArtworkSource;
        private Win.UI.Grid ArtworkGrid;
        private Win.UI.ListControlBindingSource CombBySPSource;
        private Win.UI.ListControlBindingSource CombByArtworkTypeSource;
        private Win.UI.Panel panel1;
        private Win.UI.Panel panel3;
        private Win.UI.Grid CombByArtworkGrid;
        private Win.UI.Panel panel2;
        private Win.UI.Grid CombBySPgrid;
    }
}