namespace Sci.Production.Warehouse
{
    partial class P10_Batch_MIND_Releaser
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
            this.btnMIND = new Sci.Win.UI.Button();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label1 = new Sci.Win.UI.Label();
            this.dateIssueDate = new Sci.Win.UI.DateRange();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMIND
            // 
            this.btnMIND.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMIND.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnMIND.Location = new System.Drawing.Point(855, 5);
            this.btnMIND.Name = "btnMIND";
            this.btnMIND.Size = new System.Drawing.Size(135, 30);
            this.btnMIND.TabIndex = 4;
            this.btnMIND.Text = "MIND Releaser";
            this.btnMIND.UseVisualStyleBackColor = true;
            this.btnMIND.Click += new System.EventHandler(this.BtnMIND_Click);
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
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(12, 42);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(981, 534);
            this.grid1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SkyBlue;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 23);
            this.label1.TabIndex = 134;
            this.label1.Text = "Issue Date";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateIssueDate
            // 
            // 
            // 
            // 
            this.dateIssueDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateIssueDate.DateBox1.Name = "";
            this.dateIssueDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateIssueDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateIssueDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateIssueDate.DateBox2.Name = "";
            this.dateIssueDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateIssueDate.DateBox2.TabIndex = 1;
            this.dateIssueDate.IsRequired = false;
            this.dateIssueDate.Location = new System.Drawing.Point(113, 9);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(280, 23);
            this.dateIssueDate.TabIndex = 133;
            this.dateIssueDate.Validating += new System.ComponentModel.CancelEventHandler(this.DateIssueDate_Validating);
            // 
            // P10_Batch_MIND_Releaser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 588);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateIssueDate);
            this.Controls.Add(this.btnMIND);
            this.Controls.Add(this.grid1);
            this.Name = "P10_Batch_MIND_Releaser";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P10. Batch MIND Releaser";
            this.Controls.SetChildIndex(this.grid1, 0);
            this.Controls.SetChildIndex(this.btnMIND, 0);
            this.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnMIND;
        private Win.UI.Grid grid1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label label1;
        private Win.UI.DateRange dateIssueDate;
    }
}