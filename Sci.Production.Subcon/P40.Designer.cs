namespace Sci.Production.Subcon
{
    partial class P40
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.bgWorkerUpdateInfo = new System.ComponentModel.BackgroundWorker();
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtSize = new Sci.Win.UI.TextBox();
            this.txtArticle = new Sci.Win.UI.TextBox();
            this.txtCombo = new Sci.Win.UI.TextBox();
            this.displaySubProcess = new Sci.Win.UI.DisplayBox();
            this.txtsewingline = new Sci.Production.Class.Txtsewingline();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.labSize = new Sci.Win.UI.Label();
            this.labArticle = new Sci.Win.UI.Label();
            this.labCombo = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.labinline = new Sci.Win.UI.Label();
            this.labFty = new Sci.Win.UI.Label();
            this.txtLocation = new Sci.Win.UI.TextBox();
            this.dateRangeInlineDate = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.txtSp2 = new Sci.Win.UI.TextBox();
            this.txtSp1 = new Sci.Win.UI.TextBox();
            this.labLocation = new Sci.Win.UI.Label();
            this.labInlineDate = new Sci.Win.UI.Label();
            this.labSpno = new Sci.Win.UI.Label();
            this.grid = new Sci.Win.UI.Grid();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnToExcel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 509);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 53);
            this.panel1.TabIndex = 14;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(899, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(97, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(790, 11);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(103, 30);
            this.btnToExcel.TabIndex = 0;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // bgWorkerUpdateInfo
            // 
            this.bgWorkerUpdateInfo.WorkerReportsProgress = true;
            this.bgWorkerUpdateInfo.WorkerSupportsCancellation = true;
            this.bgWorkerUpdateInfo.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BgWorkerUpdateInfo_DoWork);
            this.bgWorkerUpdateInfo.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BgWorkerUpdateInfo_RunWorkerCompleted);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(912, 12);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 41;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtSize
            // 
            this.txtSize.BackColor = System.Drawing.Color.White;
            this.txtSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSize.Location = new System.Drawing.Point(681, 67);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(135, 23);
            this.txtSize.TabIndex = 40;
            // 
            // txtArticle
            // 
            this.txtArticle.BackColor = System.Drawing.Color.White;
            this.txtArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtArticle.Location = new System.Drawing.Point(681, 38);
            this.txtArticle.Name = "txtArticle";
            this.txtArticle.Size = new System.Drawing.Size(135, 23);
            this.txtArticle.TabIndex = 39;
            // 
            // txtCombo
            // 
            this.txtCombo.BackColor = System.Drawing.Color.White;
            this.txtCombo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCombo.Location = new System.Drawing.Point(681, 9);
            this.txtCombo.Name = "txtCombo";
            this.txtCombo.Size = new System.Drawing.Size(135, 23);
            this.txtCombo.TabIndex = 38;
            // 
            // displaySubProcess
            // 
            this.displaySubProcess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySubProcess.Location = new System.Drawing.Point(479, 67);
            this.displaySubProcess.Name = "displaySubProcess";
            this.displaySubProcess.Size = new System.Drawing.Size(134, 23);
            this.displaySubProcess.TabIndex = 37;
            // 
            // txtsewingline
            // 
            this.txtsewingline.BackColor = System.Drawing.Color.White;
            this.txtsewingline.FactoryobjectName = this.txtfactory;
            this.txtsewingline.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtsewingline.Location = new System.Drawing.Point(479, 38);
            this.txtsewingline.Name = "txtsewingline";
            this.txtsewingline.Size = new System.Drawing.Size(134, 23);
            this.txtsewingline.TabIndex = 36;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.FilteMDivision = true;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(479, 9);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(134, 23);
            this.txtfactory.TabIndex = 35;
            // 
            // labSize
            // 
            this.labSize.Location = new System.Drawing.Point(616, 66);
            this.labSize.Name = "labSize";
            this.labSize.Size = new System.Drawing.Size(62, 23);
            this.labSize.TabIndex = 34;
            this.labSize.Text = "Size";
            // 
            // labArticle
            // 
            this.labArticle.Location = new System.Drawing.Point(616, 38);
            this.labArticle.Name = "labArticle";
            this.labArticle.Size = new System.Drawing.Size(62, 23);
            this.labArticle.TabIndex = 33;
            this.labArticle.Text = "Article";
            // 
            // labCombo
            // 
            this.labCombo.Location = new System.Drawing.Point(616, 9);
            this.labCombo.Name = "labCombo";
            this.labCombo.Size = new System.Drawing.Size(62, 23);
            this.labCombo.TabIndex = 32;
            this.labCombo.Text = "Combo";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(392, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 23);
            this.label2.TabIndex = 31;
            this.label2.Text = "Sub Process";
            // 
            // labinline
            // 
            this.labinline.Location = new System.Drawing.Point(392, 38);
            this.labinline.Name = "labinline";
            this.labinline.Size = new System.Drawing.Size(84, 23);
            this.labinline.TabIndex = 30;
            this.labinline.Text = "Inline Line#";
            // 
            // labFty
            // 
            this.labFty.Location = new System.Drawing.Point(392, 9);
            this.labFty.Name = "labFty";
            this.labFty.Size = new System.Drawing.Size(84, 23);
            this.labFty.TabIndex = 29;
            this.labFty.Text = "Factory";
            // 
            // txtLocation
            // 
            this.txtLocation.BackColor = System.Drawing.Color.White;
            this.txtLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocation.Location = new System.Drawing.Point(83, 66);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(127, 23);
            this.txtLocation.TabIndex = 28;
            // 
            // dateRangeInlineDate
            // 
            // 
            // 
            // 
            this.dateRangeInlineDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeInlineDate.DateBox1.Name = "";
            this.dateRangeInlineDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeInlineDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeInlineDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeInlineDate.DateBox2.Name = "";
            this.dateRangeInlineDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeInlineDate.DateBox2.TabIndex = 1;
            this.dateRangeInlineDate.Location = new System.Drawing.Point(83, 38);
            this.dateRangeInlineDate.Name = "dateRangeInlineDate";
            this.dateRangeInlineDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeInlineDate.TabIndex = 27;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(215, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 23);
            this.label1.TabIndex = 26;
            this.label1.Text = "~";
            this.label1.TextStyle.Alignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // txtSp2
            // 
            this.txtSp2.BackColor = System.Drawing.Color.White;
            this.txtSp2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSp2.Location = new System.Drawing.Point(236, 9);
            this.txtSp2.Name = "txtSp2";
            this.txtSp2.Size = new System.Drawing.Size(127, 23);
            this.txtSp2.TabIndex = 25;
            // 
            // txtSp1
            // 
            this.txtSp1.BackColor = System.Drawing.Color.White;
            this.txtSp1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSp1.Location = new System.Drawing.Point(83, 9);
            this.txtSp1.Name = "txtSp1";
            this.txtSp1.Size = new System.Drawing.Size(127, 23);
            this.txtSp1.TabIndex = 24;
            // 
            // labLocation
            // 
            this.labLocation.Location = new System.Drawing.Point(5, 66);
            this.labLocation.Name = "labLocation";
            this.labLocation.Size = new System.Drawing.Size(75, 23);
            this.labLocation.TabIndex = 23;
            this.labLocation.Text = "Location";
            // 
            // labInlineDate
            // 
            this.labInlineDate.Location = new System.Drawing.Point(5, 38);
            this.labInlineDate.Name = "labInlineDate";
            this.labInlineDate.Size = new System.Drawing.Size(75, 23);
            this.labInlineDate.TabIndex = 22;
            this.labInlineDate.Text = "Inline Date";
            // 
            // labSpno
            // 
            this.labSpno.Location = new System.Drawing.Point(5, 9);
            this.labSpno.Name = "labSpno";
            this.labSpno.Size = new System.Drawing.Size(75, 23);
            this.labSpno.TabIndex = 21;
            this.labSpno.Text = "SP#";
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
            this.grid.DataSource = this.listControlBindingSource1;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(5, 95);
            this.grid.Name = "grid";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(1003, 408);
            this.grid.TabIndex = 42;
            // 
            // P40
            // 
            this.ClientSize = new System.Drawing.Size(1008, 562);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.txtArticle);
            this.Controls.Add(this.txtCombo);
            this.Controls.Add(this.displaySubProcess);
            this.Controls.Add(this.txtsewingline);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labSize);
            this.Controls.Add(this.labArticle);
            this.Controls.Add(this.labCombo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labinline);
            this.Controls.Add(this.labFty);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.dateRangeInlineDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSp2);
            this.Controls.Add(this.txtSp1);
            this.Controls.Add(this.labLocation);
            this.Controls.Add(this.labInlineDate);
            this.Controls.Add(this.labSpno);
            this.Controls.Add(this.panel1);
            this.Name = "P40";
            this.Text = "P40. Search Location (RFID)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.P40_FormClosing);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.labSpno, 0);
            this.Controls.SetChildIndex(this.labInlineDate, 0);
            this.Controls.SetChildIndex(this.labLocation, 0);
            this.Controls.SetChildIndex(this.txtSp1, 0);
            this.Controls.SetChildIndex(this.txtSp2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.dateRangeInlineDate, 0);
            this.Controls.SetChildIndex(this.txtLocation, 0);
            this.Controls.SetChildIndex(this.labFty, 0);
            this.Controls.SetChildIndex(this.labinline, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.labCombo, 0);
            this.Controls.SetChildIndex(this.labArticle, 0);
            this.Controls.SetChildIndex(this.labSize, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtsewingline, 0);
            this.Controls.SetChildIndex(this.displaySubProcess, 0);
            this.Controls.SetChildIndex(this.txtCombo, 0);
            this.Controls.SetChildIndex(this.txtArticle, 0);
            this.Controls.SetChildIndex(this.txtSize, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnToExcel;
        private System.ComponentModel.BackgroundWorker bgWorkerUpdateInfo;
        private Win.UI.Button btnQuery;
        private Win.UI.TextBox txtSize;
        private Win.UI.TextBox txtArticle;
        private Win.UI.TextBox txtCombo;
        private Win.UI.DisplayBox displaySubProcess;
        private Class.Txtsewingline txtsewingline;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label labSize;
        private Win.UI.Label labArticle;
        private Win.UI.Label labCombo;
        private Win.UI.Label label2;
        private Win.UI.Label labinline;
        private Win.UI.Label labFty;
        private Win.UI.TextBox txtLocation;
        private Win.UI.DateRange dateRangeInlineDate;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtSp2;
        private Win.UI.TextBox txtSp1;
        private Win.UI.Label labLocation;
        private Win.UI.Label labInlineDate;
        private Win.UI.Label labSpno;
        private Win.UI.Grid grid;
    }
}
