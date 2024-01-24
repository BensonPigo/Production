namespace Sci.Production.IE
{
    partial class P01_AT_Summary
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel5 = new Sci.Win.UI.Panel();
            this.splitContainer2 = new Sci.Win.UI.SplitContainer();
            this.gridIETMS = new Sci.Win.UI.Grid();
            this.gridAT = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnCancel = new Sci.Win.UI.Button();
            this.panel4 = new Sci.Win.UI.Panel();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridIETMS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 558);
            this.panel1.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(846, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 558);
            this.panel2.TabIndex = 11;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.splitContainer2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(504, 558);
            this.panel5.TabIndex = 14;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.gridIETMS);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gridAT);
            this.splitContainer2.Size = new System.Drawing.Size(504, 558);
            this.splitContainer2.SplitterDistance = 336;
            this.splitContainer2.TabIndex = 7;
            this.splitContainer2.TabStop = false;
            // 
            // gridIETMS
            // 
            this.gridIETMS.AllowUserToAddRows = false;
            this.gridIETMS.AllowUserToDeleteRows = false;
            this.gridIETMS.AllowUserToResizeRows = false;
            this.gridIETMS.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridIETMS.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridIETMS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridIETMS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridIETMS.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridIETMS.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridIETMS.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridIETMS.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridIETMS.Location = new System.Drawing.Point(0, 0);
            this.gridIETMS.MultiSelect = false;
            this.gridIETMS.Name = "gridIETMS";
            this.gridIETMS.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridIETMS.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridIETMS.RowTemplate.Height = 24;
            this.gridIETMS.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridIETMS.ShowCellToolTips = false;
            this.gridIETMS.Size = new System.Drawing.Size(504, 336);
            this.gridIETMS.TabIndex = 2;
            this.gridIETMS.TabStop = false;
            // 
            // gridAT
            // 
            this.gridAT.AllowUserToAddRows = false;
            this.gridAT.AllowUserToDeleteRows = false;
            this.gridAT.AllowUserToResizeRows = false;
            this.gridAT.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridAT.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridAT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridAT.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridAT.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridAT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridAT.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridAT.Location = new System.Drawing.Point(0, 0);
            this.gridAT.MultiSelect = false;
            this.gridAT.Name = "gridAT";
            this.gridAT.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridAT.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridAT.RowTemplate.Height = 24;
            this.gridAT.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAT.ShowCellToolTips = false;
            this.gridAT.Size = new System.Drawing.Size(504, 218);
            this.gridAT.TabIndex = 1;
            this.gridAT.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(416, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnCancel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 513);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(504, 45);
            this.panel4.TabIndex = 13;
            // 
            // P01_AT_Summary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 558);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P01_AT_Summary";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "AT Summary";
            this.panel5.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridIETMS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel5;
        private Win.UI.SplitContainer splitContainer2;
        private Win.UI.Grid gridIETMS;
        private Win.UI.Grid gridAT;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnCancel;
        private Win.UI.Panel panel4;
    }
}