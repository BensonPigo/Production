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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new Sci.Win.UI.TabControl();
            this.Artwork = new System.Windows.Forms.TabPage();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.ArtworkGrid = new Sci.Win.UI.Grid();
            this.ArtworkSource = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.CombBySP = new System.Windows.Forms.TabPage();
            this.panel2 = new Sci.Win.UI.Panel();
            this.CombBySPgrid = new Sci.Win.UI.Grid();
            this.CombByArtworkType = new System.Windows.Forms.TabPage();
            this.panel3 = new Sci.Win.UI.Panel();
            this.CombByArtworkGrid = new Sci.Win.UI.Grid();
            this.CombBySPSource = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.CombByArtworkTypeSource = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel5 = new Sci.Win.UI.Panel();
            this.button1 = new Sci.Win.UI.Button();
            this.panel6 = new Sci.Win.UI.Panel();
            this.button2 = new Sci.Win.UI.Button();
            this.tabControl1.SuspendLayout();
            this.Artwork.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ArtworkGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArtworkSource)).BeginInit();
            this.CombBySP.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CombBySPgrid)).BeginInit();
            this.CombByArtworkType.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CombByArtworkGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CombBySPSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CombByArtworkTypeSource)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
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
            this.tabControl1.Size = new System.Drawing.Size(831, 524);
            this.tabControl1.TabIndex = 0;
            // 
            // Artwork
            // 
            this.Artwork.Controls.Add(this.panel1);
            this.Artwork.Location = new System.Drawing.Point(4, 25);
            this.Artwork.Name = "Artwork";
            this.Artwork.Padding = new System.Windows.Forms.Padding(3);
            this.Artwork.Size = new System.Drawing.Size(823, 495);
            this.Artwork.TabIndex = 0;
            this.Artwork.Text = "Artwork";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.ArtworkGrid);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(817, 489);
            this.panel1.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 451);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(817, 38);
            this.panel4.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(732, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
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
            this.ArtworkGrid.Size = new System.Drawing.Size(817, 489);
            this.ArtworkGrid.TabIndex = 0;
            // 
            // CombBySP
            // 
            this.CombBySP.Controls.Add(this.panel2);
            this.CombBySP.Location = new System.Drawing.Point(4, 25);
            this.CombBySP.Name = "CombBySP";
            this.CombBySP.Padding = new System.Windows.Forms.Padding(3);
            this.CombBySP.Size = new System.Drawing.Size(823, 495);
            this.CombBySP.TabIndex = 1;
            this.CombBySP.Text = "Comb by SP#";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.CombBySPgrid);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(817, 489);
            this.panel2.TabIndex = 0;
            // 
            // CombBySPgrid
            // 
            this.CombBySPgrid.AllowUserToAddRows = false;
            this.CombBySPgrid.AllowUserToDeleteRows = false;
            this.CombBySPgrid.AllowUserToResizeRows = false;
            this.CombBySPgrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.CombBySPgrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.CombBySPgrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CombBySPgrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CombBySPgrid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.CombBySPgrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.CombBySPgrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.CombBySPgrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.CombBySPgrid.Location = new System.Drawing.Point(0, 0);
            this.CombBySPgrid.Name = "CombBySPgrid";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CombBySPgrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.CombBySPgrid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.CombBySPgrid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.CombBySPgrid.RowTemplate.Height = 24;
            this.CombBySPgrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CombBySPgrid.ShowCellToolTips = false;
            this.CombBySPgrid.Size = new System.Drawing.Size(817, 489);
            this.CombBySPgrid.TabIndex = 0;
            // 
            // CombByArtworkType
            // 
            this.CombByArtworkType.Controls.Add(this.panel3);
            this.CombByArtworkType.Location = new System.Drawing.Point(4, 25);
            this.CombByArtworkType.Name = "CombByArtworkType";
            this.CombByArtworkType.Padding = new System.Windows.Forms.Padding(3);
            this.CombByArtworkType.Size = new System.Drawing.Size(823, 495);
            this.CombByArtworkType.TabIndex = 2;
            this.CombByArtworkType.Text = "Comb by Artwork Type";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.CombByArtworkGrid);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(817, 489);
            this.panel3.TabIndex = 0;
            // 
            // CombByArtworkGrid
            // 
            this.CombByArtworkGrid.AllowUserToAddRows = false;
            this.CombByArtworkGrid.AllowUserToDeleteRows = false;
            this.CombByArtworkGrid.AllowUserToResizeRows = false;
            this.CombByArtworkGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.CombByArtworkGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.CombByArtworkGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CombByArtworkGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CombByArtworkGrid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.CombByArtworkGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.CombByArtworkGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.CombByArtworkGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.CombByArtworkGrid.Location = new System.Drawing.Point(0, 0);
            this.CombByArtworkGrid.Name = "CombByArtworkGrid";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CombByArtworkGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.CombByArtworkGrid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.CombByArtworkGrid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.CombByArtworkGrid.RowTemplate.Height = 24;
            this.CombByArtworkGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CombByArtworkGrid.ShowCellToolTips = false;
            this.CombByArtworkGrid.Size = new System.Drawing.Size(817, 489);
            this.CombByArtworkGrid.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.button1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 451);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(817, 38);
            this.panel5.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(732, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.button2);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 451);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(817, 38);
            this.panel6.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(732, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 30);
            this.button2.TabIndex = 0;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // P01_Artwork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 524);
            this.Controls.Add(this.tabControl1);
            this.Name = "P01_Artwork";
            this.Text = "P01_ArtwrokNew";
            this.WorkAlias = "Order_Artwork";
            this.tabControl1.ResumeLayout(false);
            this.Artwork.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ArtworkGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArtworkSource)).EndInit();
            this.CombBySP.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CombBySPgrid)).EndInit();
            this.CombByArtworkType.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CombByArtworkGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CombBySPSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CombByArtworkTypeSource)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
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
        private Win.UI.Panel panel4;
        private Win.UI.Button btnClose;
        private Win.UI.Panel panel5;
        private Win.UI.Button button1;
        private Win.UI.Panel panel6;
        private Win.UI.Button button2;
    }
}