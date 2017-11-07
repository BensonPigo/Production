namespace Sci.Production.PPIC
{
    partial class P01_CTNStatus
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnRecompute = new Sci.Win.UI.Button();
            this.comboSortby = new Sci.Win.UI.ComboBox();
            this.labelSortby = new Sci.Win.UI.Label();
            this.comboCTN = new Sci.Win.UI.ComboBox();
            this.labelCTN = new Sci.Win.UI.Label();
            this.comboPackingListID = new Sci.Win.UI.ComboBox();
            this.labelPackingListID = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.panel5 = new Sci.Win.UI.Panel();
            this.tabControl1 = new Sci.Win.UI.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gridTransactionDetali = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gridLastStatus = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTransactionDetali)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLastStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 465);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(790, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 465);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnRecompute);
            this.panel3.Controls.Add(this.comboSortby);
            this.panel3.Controls.Add(this.labelSortby);
            this.panel3.Controls.Add(this.comboCTN);
            this.panel3.Controls.Add(this.labelCTN);
            this.panel3.Controls.Add(this.comboPackingListID);
            this.panel3.Controls.Add(this.labelPackingListID);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(780, 54);
            this.panel3.TabIndex = 0;
            // 
            // btnRecompute
            // 
            this.btnRecompute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecompute.Location = new System.Drawing.Point(645, 10);
            this.btnRecompute.Name = "btnRecompute";
            this.btnRecompute.Size = new System.Drawing.Size(133, 30);
            this.btnRecompute.TabIndex = 3;
            this.btnRecompute.Text = "Recompute";
            this.btnRecompute.UseVisualStyleBackColor = true;
            this.btnRecompute.Click += new System.EventHandler(this.BtnRecompute_Click);
            // 
            // comboSortby
            // 
            this.comboSortby.BackColor = System.Drawing.Color.White;
            this.comboSortby.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSortby.FormattingEnabled = true;
            this.comboSortby.IsSupportUnselect = true;
            this.comboSortby.Location = new System.Drawing.Point(495, 13);
            this.comboSortby.Name = "comboSortby";
            this.comboSortby.Size = new System.Drawing.Size(118, 24);
            this.comboSortby.TabIndex = 2;
            this.comboSortby.SelectedIndexChanged += new System.EventHandler(this.ComboSortby_SelectedIndexChanged);
            // 
            // labelSortby
            // 
            this.labelSortby.Lines = 0;
            this.labelSortby.Location = new System.Drawing.Point(441, 13);
            this.labelSortby.Name = "labelSortby";
            this.labelSortby.Size = new System.Drawing.Size(50, 23);
            this.labelSortby.TabIndex = 4;
            this.labelSortby.Text = "Sort by";
            // 
            // comboCTN
            // 
            this.comboCTN.BackColor = System.Drawing.Color.White;
            this.comboCTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCTN.FormattingEnabled = true;
            this.comboCTN.IsSupportUnselect = true;
            this.comboCTN.Location = new System.Drawing.Point(327, 13);
            this.comboCTN.Name = "comboCTN";
            this.comboCTN.Size = new System.Drawing.Size(69, 24);
            this.comboCTN.TabIndex = 1;
            this.comboCTN.SelectedIndexChanged += new System.EventHandler(this.ComboCTN_SelectedIndexChanged);
            // 
            // labelCTN
            // 
            this.labelCTN.Lines = 0;
            this.labelCTN.Location = new System.Drawing.Point(281, 13);
            this.labelCTN.Name = "labelCTN";
            this.labelCTN.Size = new System.Drawing.Size(42, 23);
            this.labelCTN.TabIndex = 2;
            this.labelCTN.Text = "CTN#";
            // 
            // comboPackingListID
            // 
            this.comboPackingListID.BackColor = System.Drawing.Color.White;
            this.comboPackingListID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboPackingListID.FormattingEnabled = true;
            this.comboPackingListID.IsSupportUnselect = true;
            this.comboPackingListID.Location = new System.Drawing.Point(108, 13);
            this.comboPackingListID.Name = "comboPackingListID";
            this.comboPackingListID.Size = new System.Drawing.Size(139, 24);
            this.comboPackingListID.TabIndex = 0;
            this.comboPackingListID.SelectedIndexChanged += new System.EventHandler(this.ComboPackingListID_SelectedIndexChanged);
            // 
            // labelPackingListID
            // 
            this.labelPackingListID.Lines = 0;
            this.labelPackingListID.Location = new System.Drawing.Point(7, 13);
            this.labelPackingListID.Name = "labelPackingListID";
            this.labelPackingListID.Size = new System.Drawing.Size(97, 23);
            this.labelPackingListID.TabIndex = 0;
            this.labelPackingListID.Text = "Packing List ID";
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 455);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(780, 10);
            this.panel4.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.tabControl1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 54);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(780, 401);
            this.panel5.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(780, 401);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.TabControl1_Selecting);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gridTransactionDetali);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(772, 372);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Transaction Detail";
            // 
            // gridTransactionDetali
            // 
            this.gridTransactionDetali.AllowUserToAddRows = false;
            this.gridTransactionDetali.AllowUserToDeleteRows = false;
            this.gridTransactionDetali.AllowUserToResizeRows = false;
            this.gridTransactionDetali.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridTransactionDetali.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridTransactionDetali.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTransactionDetali.DataSource = this.listControlBindingSource1;
            this.gridTransactionDetali.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTransactionDetali.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridTransactionDetali.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridTransactionDetali.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridTransactionDetali.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridTransactionDetali.Location = new System.Drawing.Point(3, 3);
            this.gridTransactionDetali.Name = "gridTransactionDetali";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridTransactionDetali.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridTransactionDetali.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridTransactionDetali.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridTransactionDetali.RowTemplate.Height = 24;
            this.gridTransactionDetali.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTransactionDetali.Size = new System.Drawing.Size(766, 366);
            this.gridTransactionDetali.TabIndex = 0;
            this.gridTransactionDetali.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gridLastStatus);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(772, 372);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "CTN# Last Status";
            // 
            // gridLastStatus
            // 
            this.gridLastStatus.AllowUserToAddRows = false;
            this.gridLastStatus.AllowUserToDeleteRows = false;
            this.gridLastStatus.AllowUserToResizeRows = false;
            this.gridLastStatus.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridLastStatus.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridLastStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLastStatus.DataSource = this.listControlBindingSource2;
            this.gridLastStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridLastStatus.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridLastStatus.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridLastStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridLastStatus.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridLastStatus.Location = new System.Drawing.Point(3, 3);
            this.gridLastStatus.Name = "gridLastStatus";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridLastStatus.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridLastStatus.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridLastStatus.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridLastStatus.RowTemplate.Height = 24;
            this.gridLastStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLastStatus.Size = new System.Drawing.Size(766, 366);
            this.gridLastStatus.TabIndex = 0;
            this.gridLastStatus.TabStop = false;
            // 
            // P01_CTNStatus
            // 
            this.ClientSize = new System.Drawing.Size(800, 465);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "comboPackingListID";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "P01_CTNStatus";
            this.Text = "Carton Status";
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTransactionDetali)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridLastStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Button btnRecompute;
        private Win.UI.ComboBox comboSortby;
        private Win.UI.Label labelSortby;
        private Win.UI.ComboBox comboCTN;
        private Win.UI.Label labelCTN;
        private Win.UI.ComboBox comboPackingListID;
        private Win.UI.Label labelPackingListID;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Win.UI.Grid gridTransactionDetali;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private System.Windows.Forms.TabPage tabPage2;
        private Win.UI.Grid gridLastStatus;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
    }
}
