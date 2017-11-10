namespace Sci.Production.IE
{
    partial class P01_History
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnSummary = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.label7 = new Sci.Win.UI.Label();
            this.numNumOfSewer = new Sci.Win.UI.NumericBox();
            this.numTotalSewingTimePc = new Sci.Win.UI.NumericBox();
            this.comboStatus = new Sci.Win.UI.ComboBox();
            this.labelNumOfSewer = new Sci.Win.UI.Label();
            this.labelTotalSewingTimePc = new Sci.Win.UI.Label();
            this.labelStatus = new Sci.Win.UI.Label();
            this.displayStyle1 = new Sci.Win.UI.DisplayBox();
            this.displayCD = new Sci.Win.UI.DisplayBox();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.displayStyle = new Sci.Win.UI.DisplayBox();
            this.labelCD = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 504);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(811, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 504);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSummary);
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Controls.Add(this.btnToExcel);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.numNumOfSewer);
            this.panel3.Controls.Add(this.numTotalSewingTimePc);
            this.panel3.Controls.Add(this.comboStatus);
            this.panel3.Controls.Add(this.labelNumOfSewer);
            this.panel3.Controls.Add(this.labelTotalSewingTimePc);
            this.panel3.Controls.Add(this.labelStatus);
            this.panel3.Controls.Add(this.displayStyle1);
            this.panel3.Controls.Add(this.displayCD);
            this.panel3.Controls.Add(this.displaySeason);
            this.panel3.Controls.Add(this.displayStyle);
            this.panel3.Controls.Add(this.labelCD);
            this.panel3.Controls.Add(this.labelSeason);
            this.panel3.Controls.Add(this.labelStyle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(801, 89);
            this.panel3.TabIndex = 2;
            // 
            // btnSummary
            // 
            this.btnSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSummary.Location = new System.Drawing.Point(604, 49);
            this.btnSummary.Name = "btnSummary";
            this.btnSummary.Size = new System.Drawing.Size(94, 30);
            this.btnSummary.TabIndex = 8;
            this.btnSummary.Text = "Summary";
            this.btnSummary.UseVisualStyleBackColor = true;
            this.btnSummary.Click += new System.EventHandler(this.BtnSummary_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(714, 49);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnToExcel
            // 
            this.btnToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToExcel.Location = new System.Drawing.Point(714, 12);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 9;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(470, 34);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 23);
            this.label7.TabIndex = 6;
            this.label7.Text = "sec";
            this.label7.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            this.label7.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // numNumOfSewer
            // 
            this.numNumOfSewer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numNumOfSewer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numNumOfSewer.IsSupportEditMode = false;
            this.numNumOfSewer.Location = new System.Drawing.Point(400, 61);
            this.numNumOfSewer.Name = "numNumOfSewer";
            this.numNumOfSewer.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numNumOfSewer.ReadOnly = true;
            this.numNumOfSewer.Size = new System.Drawing.Size(45, 23);
            this.numNumOfSewer.TabIndex = 7;
            this.numNumOfSewer.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numTotalSewingTimePc
            // 
            this.numTotalSewingTimePc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalSewingTimePc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalSewingTimePc.IsSupportEditMode = false;
            this.numTotalSewingTimePc.Location = new System.Drawing.Point(400, 34);
            this.numTotalSewingTimePc.Name = "numTotalSewingTimePc";
            this.numTotalSewingTimePc.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalSewingTimePc.ReadOnly = true;
            this.numTotalSewingTimePc.Size = new System.Drawing.Size(66, 23);
            this.numTotalSewingTimePc.TabIndex = 5;
            this.numTotalSewingTimePc.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // comboStatus
            // 
            this.comboStatus.BackColor = System.Drawing.Color.White;
            this.comboStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.IsSupportUnselect = true;
            this.comboStatus.Location = new System.Drawing.Point(400, 5);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.Size = new System.Drawing.Size(149, 24);
            this.comboStatus.TabIndex = 4;
            this.comboStatus.SelectedIndexChanged += new System.EventHandler(this.ComboStatus_SelectedIndexChanged);
            // 
            // labelNumOfSewer
            // 
            this.labelNumOfSewer.Lines = 0;
            this.labelNumOfSewer.Location = new System.Drawing.Point(303, 61);
            this.labelNumOfSewer.Name = "labelNumOfSewer";
            this.labelNumOfSewer.Size = new System.Drawing.Size(93, 23);
            this.labelNumOfSewer.TabIndex = 9;
            this.labelNumOfSewer.Text = "Num of Sewer";
            // 
            // labelTotalSewingTimePc
            // 
            this.labelTotalSewingTimePc.Lines = 0;
            this.labelTotalSewingTimePc.Location = new System.Drawing.Point(262, 34);
            this.labelTotalSewingTimePc.Name = "labelTotalSewingTimePc";
            this.labelTotalSewingTimePc.Size = new System.Drawing.Size(134, 23);
            this.labelTotalSewingTimePc.TabIndex = 8;
            this.labelTotalSewingTimePc.Text = "Total Sewing time/Pc";
            // 
            // labelStatus
            // 
            this.labelStatus.Lines = 0;
            this.labelStatus.Location = new System.Drawing.Point(350, 7);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(46, 23);
            this.labelStatus.TabIndex = 7;
            this.labelStatus.Text = "Status";
            // 
            // displayStyle1
            // 
            this.displayStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyle1.Location = new System.Drawing.Point(200, 7);
            this.displayStyle1.Name = "displayStyle1";
            this.displayStyle1.Size = new System.Drawing.Size(25, 23);
            this.displayStyle1.TabIndex = 1;
            // 
            // displayCD
            // 
            this.displayCD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCD.Location = new System.Drawing.Point(60, 61);
            this.displayCD.Name = "displayCD";
            this.displayCD.Size = new System.Drawing.Size(55, 23);
            this.displayCD.TabIndex = 3;
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(60, 34);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(92, 23);
            this.displaySeason.TabIndex = 2;
            // 
            // displayStyle
            // 
            this.displayStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyle.Location = new System.Drawing.Point(60, 7);
            this.displayStyle.Name = "displayStyle";
            this.displayStyle.Size = new System.Drawing.Size(138, 23);
            this.displayStyle.TabIndex = 0;
            // 
            // labelCD
            // 
            this.labelCD.Lines = 0;
            this.labelCD.Location = new System.Drawing.Point(4, 61);
            this.labelCD.Name = "labelCD";
            this.labelCD.Size = new System.Drawing.Size(52, 23);
            this.labelCD.TabIndex = 2;
            this.labelCD.Text = "CD#";
            // 
            // labelSeason
            // 
            this.labelSeason.Lines = 0;
            this.labelSeason.Location = new System.Drawing.Point(4, 34);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(52, 23);
            this.labelSeason.TabIndex = 1;
            this.labelSeason.Text = "Season";
            // 
            // labelStyle
            // 
            this.labelStyle.Lines = 0;
            this.labelStyle.Location = new System.Drawing.Point(4, 7);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(52, 23);
            this.labelStyle.TabIndex = 0;
            this.labelStyle.Text = "Style";
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 494);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(801, 10);
            this.panel4.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridDetail);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 89);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(801, 405);
            this.panel5.TabIndex = 4;
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.DataSource = this.listControlBindingSource1;
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(0, 0);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.Size = new System.Drawing.Size(801, 405);
            this.gridDetail.TabIndex = 0;
            this.gridDetail.TabStop = false;
            // 
            // P01_History
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(821, 504);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "P01_History";
            this.Text = "History --";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridDetail;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnSummary;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnToExcel;
        private Win.UI.Label label7;
        private Win.UI.NumericBox numNumOfSewer;
        private Win.UI.NumericBox numTotalSewingTimePc;
        private Win.UI.ComboBox comboStatus;
        private Win.UI.Label labelNumOfSewer;
        private Win.UI.Label labelTotalSewingTimePc;
        private Win.UI.Label labelStatus;
        private Win.UI.DisplayBox displayStyle1;
        private Win.UI.DisplayBox displayCD;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.DisplayBox displayStyle;
        private Win.UI.Label labelCD;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelStyle;
    }
}
