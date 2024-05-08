namespace Sci.Production.IE
{
    partial class P05_MachineSummary
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.grid1 = new Sci.Win.UI.Grid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid = new Sci.Win.UI.Grid();
            this.lblMachine = new Sci.Win.UI.Label();
            this.lblAttachment = new Sci.Win.UI.Label();
            this.lblTemplate = new Sci.Win.UI.Label();
            this.txtMachine = new Sci.Win.UI.NumericBox();
            this.txtAttachment = new Sci.Win.UI.NumericBox();
            this.txtTemplate = new Sci.Win.UI.NumericBox();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(12, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(774, 492);
            this.panel1.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.grid1);
            this.panel3.Location = new System.Drawing.Point(278, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(493, 486);
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
            this.grid1.Size = new System.Drawing.Size(487, 480);
            this.grid1.TabIndex = 3;
            this.grid1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grid);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(269, 486);
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
            this.grid.Size = new System.Drawing.Size(263, 480);
            this.grid.TabIndex = 3;
            this.grid.TabStop = false;
            // 
            // lblMachine
            // 
            this.lblMachine.Location = new System.Drawing.Point(15, 9);
            this.lblMachine.Name = "lblMachine";
            this.lblMachine.Size = new System.Drawing.Size(73, 23);
            this.lblMachine.TabIndex = 17;
            this.lblMachine.Text = "Machine";
            // 
            // lblAttachment
            // 
            this.lblAttachment.Location = new System.Drawing.Point(293, 8);
            this.lblAttachment.Name = "lblAttachment";
            this.lblAttachment.Size = new System.Drawing.Size(92, 23);
            this.lblAttachment.TabIndex = 19;
            this.lblAttachment.Text = "Attachment";
            // 
            // lblTemplate
            // 
            this.lblTemplate.Location = new System.Drawing.Point(534, 9);
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size(92, 23);
            this.lblTemplate.TabIndex = 21;
            this.lblTemplate.Text = "Template";
            // 
            // txtMachine
            // 
            this.txtMachine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtMachine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtMachine.Location = new System.Drawing.Point(91, 9);
            this.txtMachine.Name = "txtMachine";
            this.txtMachine.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtMachine.ReadOnly = true;
            this.txtMachine.Size = new System.Drawing.Size(143, 23);
            this.txtMachine.TabIndex = 23;
            this.txtMachine.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtAttachment
            // 
            this.txtAttachment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtAttachment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtAttachment.Location = new System.Drawing.Point(388, 8);
            this.txtAttachment.Name = "txtAttachment";
            this.txtAttachment.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtAttachment.ReadOnly = true;
            this.txtAttachment.Size = new System.Drawing.Size(143, 23);
            this.txtAttachment.TabIndex = 24;
            this.txtAttachment.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtTemplate
            // 
            this.txtTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtTemplate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtTemplate.Location = new System.Drawing.Point(629, 9);
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtTemplate.ReadOnly = true;
            this.txtTemplate.Size = new System.Drawing.Size(143, 23);
            this.txtTemplate.TabIndex = 25;
            this.txtTemplate.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // P05_MachineSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 543);
            this.Controls.Add(this.txtTemplate);
            this.Controls.Add(this.txtAttachment);
            this.Controls.Add(this.txtMachine);
            this.Controls.Add(this.lblTemplate);
            this.Controls.Add(this.lblAttachment);
            this.Controls.Add(this.lblMachine);
            this.Controls.Add(this.panel1);
            this.Name = "P05_MachineSummary";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P05_MachineSummary";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private Win.UI.Grid grid1;
        private Win.UI.Grid grid;
        private Win.UI.Label lblMachine;
        private Win.UI.Label lblAttachment;
        private Win.UI.Label lblTemplate;
        private Win.UI.NumericBox txtMachine;
        private Win.UI.NumericBox txtAttachment;
        private Win.UI.NumericBox txtTemplate;
    }
}