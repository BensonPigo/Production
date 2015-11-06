namespace Sci.Production.Basic
{
    partial class B01_CapacityWorkDay
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
            this.tabControl1 = new Sci.Win.UI.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grid1 = new Sci.Win.UI.Grid();
            this.comboBox2 = new Sci.Win.UI.ComboBox();
            this.label2 = new Sci.Win.UI.Label();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.label1 = new Sci.Win.UI.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.grid2 = new Sci.Win.UI.Grid();
            this.comboBox3 = new Sci.Win.UI.ComboBox();
            this.label3 = new Sci.Win.UI.Label();
            this.button1 = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.panel5 = new Sci.Win.UI.Panel();
            this.panel6 = new Sci.Win.UI.Panel();
            this.panel7 = new Sci.Win.UI.Panel();
            this.panel8 = new Sci.Win.UI.Panel();
            this.panel9 = new Sci.Win.UI.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(528, 390);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel7);
            this.tabPage1.Controls.Add(this.panel6);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(520, 361);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Capacity";
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 0);
            this.grid1.Name = "grid1";
            this.grid1.ReadOnly = true;
            this.grid1.RowHeadersVisible = false;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(514, 322);
            this.grid1.TabIndex = 4;
            this.grid1.TabStop = false;
            // 
            // comboBox2
            // 
            this.comboBox2.BackColor = System.Drawing.Color.White;
            this.comboBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.IsSupportUnselect = true;
            this.comboBox2.Location = new System.Drawing.Point(252, 5);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(154, 24);
            this.comboBox2.TabIndex = 3;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(191, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Artwork";
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(48, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(75, 24);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Year";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel9);
            this.tabPage2.Controls.Add(this.panel8);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(520, 361);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Work day";
            // 
            // grid2
            // 
            this.grid2.AllowUserToAddRows = false;
            this.grid2.AllowUserToDeleteRows = false;
            this.grid2.AllowUserToResizeRows = false;
            this.grid2.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grid2.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid2.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid2.Location = new System.Drawing.Point(0, 0);
            this.grid2.Name = "grid2";
            this.grid2.ReadOnly = true;
            this.grid2.RowHeadersVisible = false;
            this.grid2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid2.RowTemplate.Height = 24;
            this.grid2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid2.Size = new System.Drawing.Size(514, 322);
            this.grid2.TabIndex = 4;
            this.grid2.TabStop = false;
            // 
            // comboBox3
            // 
            this.comboBox3.BackColor = System.Drawing.Color.White;
            this.comboBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.IsSupportUnselect = true;
            this.comboBox3.Location = new System.Drawing.Point(48, 5);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(75, 24);
            this.comboBox3.TabIndex = 3;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(5, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Year";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button1.Location = new System.Drawing.Point(434, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 430);
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(533, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 430);
            this.panel2.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(528, 5);
            this.panel3.TabIndex = 7;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.button1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 395);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(528, 35);
            this.panel4.TabIndex = 8;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.tabControl1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(528, 390);
            this.panel5.TabIndex = 9;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label1);
            this.panel6.Controls.Add(this.comboBox1);
            this.panel6.Controls.Add(this.comboBox2);
            this.panel6.Controls.Add(this.label2);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(514, 33);
            this.panel6.TabIndex = 5;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.grid1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 36);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(514, 322);
            this.panel7.TabIndex = 6;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label3);
            this.panel8.Controls.Add(this.comboBox3);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(3, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(514, 33);
            this.panel8.TabIndex = 5;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.grid2);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(3, 36);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(514, 322);
            this.panel9.TabIndex = 6;
            // 
            // B01_CapacityWorkDay
            // 
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(538, 430);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "B01_CapacityWorkDay";
            this.Text = "Capacity / Work day";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Win.UI.Grid grid1;
        private Win.UI.ComboBox comboBox2;
        private Win.UI.Label label2;
        private Win.UI.ComboBox comboBox1;
        private Win.UI.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private Win.UI.Grid grid2;
        private Win.UI.ComboBox comboBox3;
        private Win.UI.Label label3;
        private Win.UI.Button button1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Panel panel7;
        private Win.UI.Panel panel6;
        private Win.UI.Panel panel9;
        private Win.UI.Panel panel8;
    }
}
