namespace Sci.Production.Warehouse
{
    partial class P20
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new Sci.Win.UI.Panel();
            this.button1 = new Sci.Win.UI.Button();
            this.bindingSource1 = new Sci.Win.UI.BindingSource();
            this.panel3 = new Sci.Win.UI.Panel();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.label3 = new Sci.Win.UI.Label();
            this.textBox3 = new Sci.Win.UI.TextBox();
            this.textBox2 = new Sci.Win.UI.TextBox();
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.button2 = new Sci.Win.UI.Button();
            this.label1 = new Sci.Win.UI.Label();
            this.bindingSource2 = new Sci.Win.UI.BindingSource();
            this.bindingSource3 = new Sci.Win.UI.BindingSource();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grid1 = new Sci.Win.UI.Grid();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.grid2 = new Sci.Win.UI.Grid();
            this.grid3 = new Sci.Win.UI.Grid();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid3)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 613);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 48);
            this.panel2.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button1.Location = new System.Drawing.Point(916, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bindingSource1
            // 
            this.bindingSource1.PositionChanged += new System.EventHandler(this.bindingSource1_PositionChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.checkBox1);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.textBox3);
            this.panel3.Controls.Add(this.textBox2);
            this.panel3.Controls.Add(this.textBox1);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.button2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1008, 43);
            this.panel3.TabIndex = 1;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.IsSupportEditMode = false;
            this.checkBox1.Location = new System.Drawing.Point(560, 11);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(73, 21);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Qty > 0";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(224, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 23);
            this.label3.TabIndex = 17;
            this.label3.Text = "Seq1";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.White;
            this.textBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox3.IsSupportEditMode = false;
            this.textBox3.Location = new System.Drawing.Point(390, 9);
            this.textBox3.MaxLength = 2;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(64, 23);
            this.textBox3.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox2.IsSupportEditMode = false;
            this.textBox2.Location = new System.Drawing.Point(272, 9);
            this.textBox2.MaxLength = 3;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(64, 23);
            this.textBox2.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.IsSupportEditMode = false;
            this.textBox1.Location = new System.Drawing.Point(87, 9);
            this.textBox1.MaxLength = 13;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(134, 23);
            this.textBox1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 13;
            this.label2.Text = "Seq#";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button2.Location = new System.Drawing.Point(460, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 30);
            this.button2.TabIndex = 3;
            this.button2.Text = "Query";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(342, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Seq2";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 43);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grid1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 570);
            this.splitContainer1.SplitterDistance = 175;
            this.splitContainer1.TabIndex = 4;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 0);
            this.grid1.Name = "grid1";
            this.grid1.RowHeadersVisible = false;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(1008, 175);
            this.grid1.TabIndex = 1;
            this.grid1.TabStop = false;
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
            this.splitContainer2.Panel1.Controls.Add(this.grid2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.grid3);
            this.splitContainer2.Size = new System.Drawing.Size(1008, 391);
            this.splitContainer2.SplitterDistance = 244;
            this.splitContainer2.TabIndex = 0;
            // 
            // grid2
            // 
            this.grid2.AllowUserToAddRows = false;
            this.grid2.AllowUserToDeleteRows = false;
            this.grid2.AllowUserToResizeRows = false;
            this.grid2.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grid2.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid2.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.grid2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid2.Location = new System.Drawing.Point(0, 0);
            this.grid2.Name = "grid2";
            this.grid2.RowHeadersVisible = false;
            this.grid2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid2.RowTemplate.Height = 24;
            this.grid2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid2.Size = new System.Drawing.Size(1008, 244);
            this.grid2.TabIndex = 6;
            this.grid2.TabStop = false;
            // 
            // grid3
            // 
            this.grid3.AllowUserToAddRows = false;
            this.grid3.AllowUserToDeleteRows = false;
            this.grid3.AllowUserToResizeRows = false;
            this.grid3.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grid3.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid3.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid3.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.grid3.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid3.Location = new System.Drawing.Point(0, 0);
            this.grid3.Name = "grid3";
            this.grid3.RowHeadersVisible = false;
            this.grid3.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid3.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid3.RowTemplate.Height = 24;
            this.grid3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid3.Size = new System.Drawing.Size(1008, 143);
            this.grid3.TabIndex = 8;
            this.grid3.TabStop = false;
            // 
            // P20
            // 
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Name = "P20";
            this.Text = "P20. Stock List";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource3)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel2;
        private Win.UI.Button button1;
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Panel panel3;
        private Win.UI.Label label1;
        private Win.UI.BindingSource bindingSource2;
        private Win.UI.BindingSource bindingSource3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Grid grid1;
        private Win.UI.Label label3;
        private Win.UI.TextBox textBox3;
        private Win.UI.TextBox textBox2;
        private Win.UI.TextBox textBox1;
        private Win.UI.Label label2;
        private Win.UI.Button button2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Win.UI.Grid grid2;
        private Win.UI.Grid grid3;
        private Win.UI.CheckBox checkBox1;
    }
}
