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
            this.components = new System.ComponentModel.Container();
            this.tabControlCapacityWorkday = new Sci.Win.UI.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel7 = new Sci.Win.UI.Panel();
            this.grid1 = new Sci.Win.UI.Grid();
            this.panel6 = new Sci.Win.UI.Panel();
            this.labelCapacityYear = new Sci.Win.UI.Label();
            this.comboCapacityYear = new Sci.Win.UI.ComboBox();
            this.comboArtwork = new Sci.Win.UI.ComboBox();
            this.labelCapacityArtwork = new Sci.Win.UI.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel9 = new Sci.Win.UI.Panel();
            this.grid2 = new Sci.Win.UI.Grid();
            this.panel8 = new Sci.Win.UI.Panel();
            this.labelWorkdayYear = new Sci.Win.UI.Label();
            this.comboWorkdayYear = new Sci.Win.UI.ComboBox();
            this.btnClose = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.panel5 = new Sci.Win.UI.Panel();
            this.tabControlCapacityWorkday.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.panel6.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).BeginInit();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlCapacityWorkday
            // 
            this.tabControlCapacityWorkday.Controls.Add(this.tabPage1);
            this.tabControlCapacityWorkday.Controls.Add(this.tabPage2);
            this.tabControlCapacityWorkday.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCapacityWorkday.Location = new System.Drawing.Point(0, 0);
            this.tabControlCapacityWorkday.Name = "tabControlCapacityWorkday";
            this.tabControlCapacityWorkday.SelectedIndex = 0;
            this.tabControlCapacityWorkday.Size = new System.Drawing.Size(528, 390);
            this.tabControlCapacityWorkday.TabIndex = 0;
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
            // panel7
            // 
            this.panel7.Controls.Add(this.grid1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 36);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(514, 322);
            this.panel7.TabIndex = 6;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
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
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(514, 322);
            this.grid1.TabIndex = 4;
            this.grid1.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.labelCapacityYear);
            this.panel6.Controls.Add(this.comboCapacityYear);
            this.panel6.Controls.Add(this.comboArtwork);
            this.panel6.Controls.Add(this.labelCapacityArtwork);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(514, 33);
            this.panel6.TabIndex = 5;
            // 
            // labelCapacityYear
            // 
            this.labelCapacityYear.Lines = 0;
            this.labelCapacityYear.Location = new System.Drawing.Point(5, 5);
            this.labelCapacityYear.Name = "labelCapacityYear";
            this.labelCapacityYear.Size = new System.Drawing.Size(39, 23);
            this.labelCapacityYear.TabIndex = 0;
            this.labelCapacityYear.Text = "Year";
            // 
            // comboCapacityYear
            // 
            this.comboCapacityYear.BackColor = System.Drawing.Color.White;
            this.comboCapacityYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCapacityYear.FormattingEnabled = true;
            this.comboCapacityYear.IsSupportUnselect = true;
            this.comboCapacityYear.Location = new System.Drawing.Point(48, 5);
            this.comboCapacityYear.Name = "comboCapacityYear";
            this.comboCapacityYear.Size = new System.Drawing.Size(75, 24);
            this.comboCapacityYear.TabIndex = 1;
            this.comboCapacityYear.SelectedIndexChanged += new System.EventHandler(this.ComboCapacityYear_SelectedIndexChanged);
            // 
            // comboArtwork
            // 
            this.comboArtwork.BackColor = System.Drawing.Color.White;
            this.comboArtwork.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboArtwork.FormattingEnabled = true;
            this.comboArtwork.IsSupportUnselect = true;
            this.comboArtwork.Location = new System.Drawing.Point(252, 5);
            this.comboArtwork.Name = "comboArtwork";
            this.comboArtwork.Size = new System.Drawing.Size(154, 24);
            this.comboArtwork.TabIndex = 3;
            this.comboArtwork.SelectedIndexChanged += new System.EventHandler(this.ComboArtwork_SelectedIndexChanged);
            // 
            // labelCapacityArtwork
            // 
            this.labelCapacityArtwork.Lines = 0;
            this.labelCapacityArtwork.Location = new System.Drawing.Point(191, 5);
            this.labelCapacityArtwork.Name = "labelCapacityArtwork";
            this.labelCapacityArtwork.Size = new System.Drawing.Size(57, 23);
            this.labelCapacityArtwork.TabIndex = 2;
            this.labelCapacityArtwork.Text = "Artwork";
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
            // panel9
            // 
            this.panel9.Controls.Add(this.grid2);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(3, 36);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(514, 322);
            this.panel9.TabIndex = 6;
            // 
            // grid2
            // 
            this.grid2.AllowUserToAddRows = false;
            this.grid2.AllowUserToDeleteRows = false;
            this.grid2.AllowUserToResizeRows = false;
            this.grid2.BackgroundColor = System.Drawing.SystemColors.Control;
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
            this.grid2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid2.RowTemplate.Height = 24;
            this.grid2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid2.Size = new System.Drawing.Size(514, 322);
            this.grid2.TabIndex = 4;
            this.grid2.TabStop = false;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.labelWorkdayYear);
            this.panel8.Controls.Add(this.comboWorkdayYear);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(3, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(514, 33);
            this.panel8.TabIndex = 5;
            // 
            // labelWorkdayYear
            // 
            this.labelWorkdayYear.Lines = 0;
            this.labelWorkdayYear.Location = new System.Drawing.Point(5, 5);
            this.labelWorkdayYear.Name = "labelWorkdayYear";
            this.labelWorkdayYear.Size = new System.Drawing.Size(39, 23);
            this.labelWorkdayYear.TabIndex = 2;
            this.labelWorkdayYear.Text = "Year";
            // 
            // comboWorkdayYear
            // 
            this.comboWorkdayYear.BackColor = System.Drawing.Color.White;
            this.comboWorkdayYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboWorkdayYear.FormattingEnabled = true;
            this.comboWorkdayYear.IsSupportUnselect = true;
            this.comboWorkdayYear.Location = new System.Drawing.Point(48, 5);
            this.comboWorkdayYear.Name = "comboWorkdayYear";
            this.comboWorkdayYear.Size = new System.Drawing.Size(75, 24);
            this.comboWorkdayYear.TabIndex = 3;
            this.comboWorkdayYear.SelectedIndexChanged += new System.EventHandler(this.ComboWorkdayYear_SelectedIndexChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(434, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
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
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 395);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(528, 35);
            this.panel4.TabIndex = 8;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.tabControlCapacityWorkday);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(528, 390);
            this.panel5.TabIndex = 9;
            // 
            // B01_CapacityWorkDay
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(538, 430);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "B01_CapacityWorkDay";
            this.Text = "Capacity / Work day";
            this.tabControlCapacityWorkday.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.panel6.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).EndInit();
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.TabControl tabControlCapacityWorkday;
        private System.Windows.Forms.TabPage tabPage1;
        private Win.UI.Grid grid1;
        private Win.UI.ComboBox comboArtwork;
        private Win.UI.Label labelCapacityArtwork;
        private Win.UI.ComboBox comboCapacityYear;
        private Win.UI.Label labelCapacityYear;
        private System.Windows.Forms.TabPage tabPage2;
        private Win.UI.Grid grid2;
        private Win.UI.ComboBox comboWorkdayYear;
        private Win.UI.Label labelWorkdayYear;
        private Win.UI.Button btnClose;
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
