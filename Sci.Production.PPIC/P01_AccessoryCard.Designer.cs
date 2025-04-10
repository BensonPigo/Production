namespace Sci.Production.PPIC
{
    partial class P01_AccessoryCard
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
            this.RdbPanel = new Sci.Win.UI.RadioPanel();
            this.btnOK = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel = new Sci.Win.UI.Panel();
            this.grid = new Sci.Win.UI.Grid();
            this.gridBS = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.RdbNF = new System.Windows.Forms.RadioButton();
            this.RdbSCI = new System.Windows.Forms.RadioButton();
            this.RdbPanel.SuspendLayout();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBS)).BeginInit();
            this.SuspendLayout();
            // 
            // RdbPanel
            // 
            this.RdbPanel.Controls.Add(this.RdbSCI);
            this.RdbPanel.Controls.Add(this.RdbNF);
            this.RdbPanel.Location = new System.Drawing.Point(12, 333);
            this.RdbPanel.Name = "RdbPanel";
            this.RdbPanel.Size = new System.Drawing.Size(148, 33);
            this.RdbPanel.TabIndex = 8;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(166, 336);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 30);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(252, 336);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Controls.Add(this.grid);
            this.panel.Location = new System.Drawing.Point(12, 12);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(320, 314);
            this.panel.TabIndex = 5;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(320, 314);
            this.grid.TabIndex = 0;
            this.grid.TabStop = false;
            // 
            // RdbNF
            // 
            this.RdbNF.AutoSize = true;
            this.RdbNF.ForeColor = System.Drawing.Color.Red;
            this.RdbNF.Location = new System.Drawing.Point(56, 3);
            this.RdbNF.Name = "RdbNF";
            this.RdbNF.Size = new System.Drawing.Size(75, 21);
            this.RdbNF.TabIndex = 2;
            this.RdbNF.TabStop = true;
            this.RdbNF.Text = "N.FACE";
            this.RdbNF.UseVisualStyleBackColor = true;
            // 
            // RdbSCI
            // 
            this.RdbSCI.AutoSize = true;
            this.RdbSCI.ForeColor = System.Drawing.Color.Red;
            this.RdbSCI.Location = new System.Drawing.Point(3, 3);
            this.RdbSCI.Name = "RdbSCI";
            this.RdbSCI.Size = new System.Drawing.Size(47, 21);
            this.RdbSCI.TabIndex = 1;
            this.RdbSCI.TabStop = true;
            this.RdbSCI.Text = "SCI";
            this.RdbSCI.UseVisualStyleBackColor = true;
            // 
            // P01_AccessoryCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 374);
            this.Controls.Add(this.RdbPanel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panel);
            this.Name = "P01_AccessoryCard";
            this.OnLineHelpID = "Win.Tems.QueryForm";
            this.Text = "Select Article";
            this.Controls.SetChildIndex(this.panel, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.RdbPanel, 0);
            this.RdbPanel.ResumeLayout(false);
            this.RdbPanel.PerformLayout();
            this.panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.RadioPanel RdbPanel;
        private Win.UI.Button btnOK;
        private Win.UI.Button btnClose;
        private Win.UI.Panel panel;
        private Win.UI.Grid grid;
        private Win.UI.ListControlBindingSource gridBS;
        private System.Windows.Forms.RadioButton RdbNF;
        private System.Windows.Forms.RadioButton RdbSCI;
    }
}