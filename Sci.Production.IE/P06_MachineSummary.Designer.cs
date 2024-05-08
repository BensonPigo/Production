namespace Sci.Production.IE
{
    partial class P06_MachineSummary
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
            this.txtTemplate = new Sci.Win.UI.NumericBox();
            this.txtAttachment = new Sci.Win.UI.NumericBox();
            this.txtMachine = new Sci.Win.UI.NumericBox();
            this.lblTemplate = new Sci.Win.UI.Label();
            this.lblAttachment = new Sci.Win.UI.Label();
            this.lblMachine = new Sci.Win.UI.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.grid1 = new Sci.Win.UI.Grid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid = new Sci.Win.UI.Grid();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTemplate
            // 
            this.txtTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtTemplate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtTemplate.Location = new System.Drawing.Point(627, 7);
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtTemplate.ReadOnly = true;
            this.txtTemplate.Size = new System.Drawing.Size(143, 23);
            this.txtTemplate.TabIndex = 32;
            this.txtTemplate.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtAttachment
            // 
            this.txtAttachment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtAttachment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtAttachment.Location = new System.Drawing.Point(386, 6);
            this.txtAttachment.Name = "txtAttachment";
            this.txtAttachment.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtAttachment.ReadOnly = true;
            this.txtAttachment.Size = new System.Drawing.Size(143, 23);
            this.txtAttachment.TabIndex = 31;
            this.txtAttachment.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtMachine
            // 
            this.txtMachine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtMachine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtMachine.Location = new System.Drawing.Point(85, 6);
            this.txtMachine.Name = "txtMachine";
            this.txtMachine.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtMachine.ReadOnly = true;
            this.txtMachine.Size = new System.Drawing.Size(143, 23);
            this.txtMachine.TabIndex = 30;
            this.txtMachine.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // lblTemplate
            // 
            this.lblTemplate.Location = new System.Drawing.Point(532, 7);
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size(92, 23);
            this.lblTemplate.TabIndex = 29;
            this.lblTemplate.Text = "Template";
            // 
            // lblAttachment
            // 
            this.lblAttachment.Location = new System.Drawing.Point(291, 6);
            this.lblAttachment.Name = "lblAttachment";
            this.lblAttachment.Size = new System.Drawing.Size(92, 23);
            this.lblAttachment.TabIndex = 28;
            this.lblAttachment.Text = "Attachment";
            // 
            // lblMachine
            // 
            this.lblMachine.Location = new System.Drawing.Point(9, 6);
            this.lblMachine.Name = "lblMachine";
            this.lblMachine.Size = new System.Drawing.Size(73, 23);
            this.lblMachine.TabIndex = 27;
            this.lblMachine.Text = "Machine";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(7, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(775, 473);
            this.panel1.TabIndex = 33;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel3.Controls.Add(this.grid1);
            this.panel3.Location = new System.Drawing.Point(278, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(493, 463);
            this.panel3.TabIndex = 1;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(3, 3);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(487, 457);
            this.grid1.TabIndex = 3;
            this.grid1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.Controls.Add(this.grid);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(269, 463);
            this.panel2.TabIndex = 0;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(3, 3);
            this.grid.Name = "grid";
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(263, 457);
            this.grid.TabIndex = 3;
            this.grid.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Location = new System.Drawing.Point(7, 510);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(775, 32);
            this.panel4.TabIndex = 34;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnClose.Location = new System.Drawing.Point(682, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(93, 32);
            this.btnClose.TabIndex = 54;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P06_MachineSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 546);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtTemplate);
            this.Controls.Add(this.txtAttachment);
            this.Controls.Add(this.txtMachine);
            this.Controls.Add(this.lblTemplate);
            this.Controls.Add(this.lblAttachment);
            this.Controls.Add(this.lblMachine);
            this.Name = "P06_MachineSummary";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P06_MachineSummary";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.NumericBox txtTemplate;
        private Win.UI.NumericBox txtAttachment;
        private Win.UI.NumericBox txtMachine;
        private Win.UI.Label lblTemplate;
        private Win.UI.Label lblAttachment;
        private Win.UI.Label lblMachine;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private Win.UI.Grid grid1;
        private System.Windows.Forms.Panel panel2;
        private Win.UI.Grid grid;
        private System.Windows.Forms.Panel panel4;
        private Win.UI.Button btnClose;
    }
}