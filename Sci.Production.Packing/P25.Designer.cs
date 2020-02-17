namespace Sci.Production.Packing
{
    partial class P25
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
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnToExcel = new System.Windows.Forms.Button();
            this.panel4 = new Sci.Win.UI.Panel();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.Button();
            this.txtPOno = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtSP_e = new Sci.Win.UI.TextBox();
            this.txtSP_s = new Sci.Win.UI.TextBox();
            this.labelSewingInlineDate = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.panel5 = new Sci.Win.UI.Panel();
            this.grid = new Sci.Win.UI.Grid();
            this.listControlBindingSource = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(12, 662);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(941, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(12, 662);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Controls.Add(this.btnToExcel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(12, 618);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(929, 44);
            this.panel3.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(831, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(81, 32);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToExcel.Location = new System.Drawing.Point(733, 6);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(81, 32);
            this.btnToExcel.TabIndex = 7;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(12, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(929, 98);
            this.panel4.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Controls.Add(this.txtPOno);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSP_e);
            this.groupBox1.Controls.Add(this.txtSP_s);
            this.groupBox1.Controls.Add(this.labelSewingInlineDate);
            this.groupBox1.Controls.Add(this.labelSCIDelivery);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(929, 98);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(16, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(681, 17);
            this.label2.TabIndex = 27;
            this.label2.Text = "※ If Packing List include more than one SP# + Ship mode seq, system cannot suppor" +
    "t print Packing Guide.";
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(831, 19);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(81, 32);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtPOno
            // 
            this.txtPOno.BackColor = System.Drawing.Color.White;
            this.txtPOno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPOno.Location = new System.Drawing.Point(140, 44);
            this.txtPOno.Name = "txtPOno";
            this.txtPOno.Size = new System.Drawing.Size(156, 23);
            this.txtPOno.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(299, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 23);
            this.label1.TabIndex = 26;
            this.label1.Text = "～";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            this.label1.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtSP_e
            // 
            this.txtSP_e.BackColor = System.Drawing.Color.White;
            this.txtSP_e.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP_e.Location = new System.Drawing.Point(324, 19);
            this.txtSP_e.Name = "txtSP_e";
            this.txtSP_e.Size = new System.Drawing.Size(156, 23);
            this.txtSP_e.TabIndex = 4;
            // 
            // txtSP_s
            // 
            this.txtSP_s.BackColor = System.Drawing.Color.White;
            this.txtSP_s.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP_s.Location = new System.Drawing.Point(140, 19);
            this.txtSP_s.Name = "txtSP_s";
            this.txtSP_s.Size = new System.Drawing.Size(156, 23);
            this.txtSP_s.TabIndex = 3;
            // 
            // labelSewingInlineDate
            // 
            this.labelSewingInlineDate.Location = new System.Drawing.Point(19, 44);
            this.labelSewingInlineDate.Name = "labelSewingInlineDate";
            this.labelSewingInlineDate.Size = new System.Drawing.Size(118, 23);
            this.labelSewingInlineDate.TabIndex = 20;
            this.labelSewingInlineDate.Text = "P.O. No.";
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(19, 19);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(118, 23);
            this.labelSCIDelivery.TabIndex = 19;
            this.labelSCIDelivery.Text = "SP#";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.grid);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(12, 98);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(929, 520);
            this.panel5.TabIndex = 6;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.DataSource = this.listControlBindingSource;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(929, 520);
            this.grid.TabIndex = 0;
            // 
            // P25
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 662);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P25";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P25. Batch Print Packing Guide (Bulk)";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Label labelSewingInlineDate;
        private Win.UI.Label labelSCIDelivery;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnToExcel;
        private System.Windows.Forms.Button btnQuery;
        private Win.UI.TextBox txtPOno;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtSP_e;
        private Win.UI.TextBox txtSP_s;
        private Win.UI.Grid grid;
        private Win.UI.ListControlBindingSource listControlBindingSource;
        private System.Windows.Forms.Label label2;
    }
}