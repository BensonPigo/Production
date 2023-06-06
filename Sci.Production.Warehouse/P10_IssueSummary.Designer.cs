namespace Sci.Production.Warehouse
{
    partial class P10_IssueSummary
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
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel4 = new Sci.Win.UI.Panel();
            this.gridArtworkSummary = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridArtworkSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(366, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 328);
            this.panel2.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 328);
            this.panel1.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(356, 10);
            this.panel3.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(267, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 286);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(356, 42);
            this.panel4.TabIndex = 5;
            // 
            // gridArtworkSummary
            // 
            this.gridArtworkSummary.AllowUserToAddRows = false;
            this.gridArtworkSummary.AllowUserToDeleteRows = false;
            this.gridArtworkSummary.AllowUserToResizeRows = false;
            this.gridArtworkSummary.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridArtworkSummary.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridArtworkSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridArtworkSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridArtworkSummary.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridArtworkSummary.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridArtworkSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridArtworkSummary.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridArtworkSummary.Location = new System.Drawing.Point(10, 10);
            this.gridArtworkSummary.Name = "gridArtworkSummary";
            this.gridArtworkSummary.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridArtworkSummary.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridArtworkSummary.RowTemplate.Height = 24;
            this.gridArtworkSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridArtworkSummary.ShowCellToolTips = false;
            this.gridArtworkSummary.Size = new System.Drawing.Size(356, 276);
            this.gridArtworkSummary.TabIndex = 6;
            this.gridArtworkSummary.TabStop = false;
            // 
            // P10_IssueSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 328);
            this.Controls.Add(this.gridArtworkSummary);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P10_IssueSummary";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Issue Summary";
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridArtworkSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel2;
        private Win.UI.Panel panel1;
        private Win.UI.Panel panel3;
        private Win.UI.Button btnClose;
        private Win.UI.Panel panel4;
        private Win.UI.Grid gridArtworkSummary;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}