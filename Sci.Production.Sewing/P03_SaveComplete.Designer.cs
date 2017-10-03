namespace Sci.Production.Sewing
{
    partial class P03_SaveComplete
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
            this.tabControl1 = new Sci.Win.UI.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gridSaveComplete = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gridCheckPacking = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnClose = new Sci.Win.UI.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSaveComplete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCheckPacking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(764, 484);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gridSaveComplete);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(756, 455);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "SaveCompelet";
            // 
            // gridSaveComplete
            // 
            this.gridSaveComplete.AllowUserToAddRows = false;
            this.gridSaveComplete.AllowUserToDeleteRows = false;
            this.gridSaveComplete.AllowUserToResizeRows = false;
            this.gridSaveComplete.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSaveComplete.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSaveComplete.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSaveComplete.DataSource = this.listControlBindingSource1;
            this.gridSaveComplete.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSaveComplete.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSaveComplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSaveComplete.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSaveComplete.Location = new System.Drawing.Point(3, 3);
            this.gridSaveComplete.Name = "gridSaveComplete";
            this.gridSaveComplete.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSaveComplete.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSaveComplete.RowTemplate.Height = 24;
            this.gridSaveComplete.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSaveComplete.Size = new System.Drawing.Size(750, 452);
            this.gridSaveComplete.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gridCheckPacking);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(756, 455);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "CheckPaking";
            // 
            // gridCheckPacking
            // 
            this.gridCheckPacking.AllowUserToAddRows = false;
            this.gridCheckPacking.AllowUserToDeleteRows = false;
            this.gridCheckPacking.AllowUserToResizeRows = false;
            this.gridCheckPacking.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCheckPacking.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCheckPacking.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCheckPacking.DataSource = this.listControlBindingSource2;
            this.gridCheckPacking.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCheckPacking.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCheckPacking.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCheckPacking.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCheckPacking.Location = new System.Drawing.Point(3, 3);
            this.gridCheckPacking.Name = "gridCheckPacking";
            this.gridCheckPacking.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCheckPacking.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCheckPacking.RowTemplate.Height = 24;
            this.gridCheckPacking.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCheckPacking.Size = new System.Drawing.Size(750, 452);
            this.gridCheckPacking.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(681, 489);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // P03_SaveComplete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 526);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.Name = "P03_SaveComplete";
            this.Text = "P03_SaveComplete";
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSaveComplete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCheckPacking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Win.UI.Grid gridSaveComplete;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Grid gridCheckPacking;
        private Win.UI.Button btnClose;
    }
}