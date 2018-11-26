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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labsubpro = new System.Windows.Forms.Panel();
            this.grid = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.labSpno = new Sci.Win.UI.Label();
            this.labInlineDate = new Sci.Win.UI.Label();
            this.labLocation = new Sci.Win.UI.Label();
            this.txtSp1 = new Sci.Win.UI.TextBox();
            this.txtSp2 = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.dateRangeInlineDate = new Sci.Win.UI.DateRange();
            this.txtLocation = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.labinline = new Sci.Win.UI.Label();
            this.labFty = new Sci.Win.UI.Label();
            this.labSize = new Sci.Win.UI.Label();
            this.labArticle = new Sci.Win.UI.Label();
            this.labCombo = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.txtsewingline = new Sci.Production.Class.txtsewingline();
            this.displaySubProcess = new Sci.Win.UI.DisplayBox();
            this.txtCombo = new Sci.Win.UI.TextBox();
            this.txtArticle = new Sci.Win.UI.TextBox();
            this.txtSize = new Sci.Win.UI.TextBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.bgWorkerUpdateInfo = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.labsubpro.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
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
            // panel2
            // 
            this.panel2.Controls.Add(this.grid);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 106);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 403);
            this.panel2.TabIndex = 15;
            // 
            // labsubpro
            // 
            this.labsubpro.Controls.Add(this.btnQuery);
            this.labsubpro.Controls.Add(this.txtSize);
            this.labsubpro.Controls.Add(this.txtArticle);
            this.labsubpro.Controls.Add(this.txtCombo);
            this.labsubpro.Controls.Add(this.displaySubProcess);
            this.labsubpro.Controls.Add(this.txtsewingline);
            this.labsubpro.Controls.Add(this.txtfactory);
            this.labsubpro.Controls.Add(this.labSize);
            this.labsubpro.Controls.Add(this.labArticle);
            this.labsubpro.Controls.Add(this.labCombo);
            this.labsubpro.Controls.Add(this.label2);
            this.labsubpro.Controls.Add(this.labinline);
            this.labsubpro.Controls.Add(this.labFty);
            this.labsubpro.Controls.Add(this.txtLocation);
            this.labsubpro.Controls.Add(this.dateRangeInlineDate);
            this.labsubpro.Controls.Add(this.label1);
            this.labsubpro.Controls.Add(this.txtSp2);
            this.labsubpro.Controls.Add(this.txtSp1);
            this.labsubpro.Controls.Add(this.labLocation);
            this.labsubpro.Controls.Add(this.labInlineDate);
            this.labsubpro.Controls.Add(this.labSpno);
            this.labsubpro.Dock = System.Windows.Forms.DockStyle.Top;
            this.labsubpro.Location = new System.Drawing.Point(0, 0);
            this.labsubpro.Name = "labsubpro";
            this.labsubpro.Size = new System.Drawing.Size(1008, 100);
            this.labsubpro.TabIndex = 16;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(1008, 403);
            this.grid.TabIndex = 13;
            // 
            // labSpno
            // 
            this.labSpno.Location = new System.Drawing.Point(9, 9);
            this.labSpno.Name = "labSpno";
            this.labSpno.Size = new System.Drawing.Size(75, 23);
            this.labSpno.TabIndex = 0;
            this.labSpno.Text = "SP#";
            // 
            // labInlineDate
            // 
            this.labInlineDate.Location = new System.Drawing.Point(9, 38);
            this.labInlineDate.Name = "labInlineDate";
            this.labInlineDate.Size = new System.Drawing.Size(75, 23);
            this.labInlineDate.TabIndex = 1;
            this.labInlineDate.Text = "Inline Date";
            // 
            // labLocation
            // 
            this.labLocation.Location = new System.Drawing.Point(9, 66);
            this.labLocation.Name = "labLocation";
            this.labLocation.Size = new System.Drawing.Size(75, 23);
            this.labLocation.TabIndex = 2;
            this.labLocation.Text = "Location";
            // 
            // txtSp1
            // 
            this.txtSp1.BackColor = System.Drawing.Color.White;
            this.txtSp1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSp1.Location = new System.Drawing.Point(87, 9);
            this.txtSp1.Name = "txtSp1";
            this.txtSp1.Size = new System.Drawing.Size(127, 23);
            this.txtSp1.TabIndex = 3;
            // 
            // txtSp2
            // 
            this.txtSp2.BackColor = System.Drawing.Color.White;
            this.txtSp2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSp2.Location = new System.Drawing.Point(240, 9);
            this.txtSp2.Name = "txtSp2";
            this.txtSp2.Size = new System.Drawing.Size(127, 23);
            this.txtSp2.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(219, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 23);
            this.label1.TabIndex = 5;
            this.label1.Text = "~";
            this.label1.TextStyle.Alignment = System.Drawing.ContentAlignment.BottomCenter;
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
            this.dateRangeInlineDate.Location = new System.Drawing.Point(87, 38);
            this.dateRangeInlineDate.Name = "dateRangeInlineDate";
            this.dateRangeInlineDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeInlineDate.TabIndex = 6;
            // 
            // txtLocation
            // 
            this.txtLocation.BackColor = System.Drawing.Color.White;
            this.txtLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocation.Location = new System.Drawing.Point(87, 66);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(127, 23);
            this.txtLocation.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(396, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 23);
            this.label2.TabIndex = 10;
            this.label2.Text = "Sub Process";
            // 
            // labinline
            // 
            this.labinline.Location = new System.Drawing.Point(396, 38);
            this.labinline.Name = "labinline";
            this.labinline.Size = new System.Drawing.Size(84, 23);
            this.labinline.TabIndex = 9;
            this.labinline.Text = "Inline Line#";
            // 
            // labFty
            // 
            this.labFty.Location = new System.Drawing.Point(396, 9);
            this.labFty.Name = "labFty";
            this.labFty.Size = new System.Drawing.Size(84, 23);
            this.labFty.TabIndex = 8;
            this.labFty.Text = "Factory";
            // 
            // labSize
            // 
            this.labSize.Location = new System.Drawing.Point(620, 66);
            this.labSize.Name = "labSize";
            this.labSize.Size = new System.Drawing.Size(62, 23);
            this.labSize.TabIndex = 13;
            this.labSize.Text = "Size";
            // 
            // labArticle
            // 
            this.labArticle.Location = new System.Drawing.Point(620, 38);
            this.labArticle.Name = "labArticle";
            this.labArticle.Size = new System.Drawing.Size(62, 23);
            this.labArticle.TabIndex = 12;
            this.labArticle.Text = "Article";
            // 
            // labCombo
            // 
            this.labCombo.Location = new System.Drawing.Point(620, 9);
            this.labCombo.Name = "labCombo";
            this.labCombo.Size = new System.Drawing.Size(62, 23);
            this.labCombo.TabIndex = 11;
            this.labCombo.Text = "Combo";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.FilteMDivision = true;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(483, 9);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(134, 23);
            this.txtfactory.TabIndex = 14;
            // 
            // txtsewingline
            // 
            this.txtsewingline.BackColor = System.Drawing.Color.White;
            this.txtsewingline.factoryobjectName = null;
            this.txtsewingline.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtsewingline.Location = new System.Drawing.Point(483, 38);
            this.txtsewingline.Name = "txtsewingline";
            this.txtsewingline.Size = new System.Drawing.Size(134, 23);
            this.txtsewingline.TabIndex = 15;
            // 
            // displaySubProcess
            // 
            this.displaySubProcess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySubProcess.Location = new System.Drawing.Point(483, 67);
            this.displaySubProcess.Name = "displaySubProcess";
            this.displaySubProcess.Size = new System.Drawing.Size(134, 23);
            this.displaySubProcess.TabIndex = 16;
            // 
            // txtCombo
            // 
            this.txtCombo.BackColor = System.Drawing.Color.White;
            this.txtCombo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCombo.Location = new System.Drawing.Point(685, 9);
            this.txtCombo.Name = "txtCombo";
            this.txtCombo.Size = new System.Drawing.Size(135, 23);
            this.txtCombo.TabIndex = 17;
            // 
            // txtArticle
            // 
            this.txtArticle.BackColor = System.Drawing.Color.White;
            this.txtArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtArticle.Location = new System.Drawing.Point(685, 38);
            this.txtArticle.Name = "txtArticle";
            this.txtArticle.Size = new System.Drawing.Size(135, 23);
            this.txtArticle.TabIndex = 18;
            // 
            // txtSize
            // 
            this.txtSize.BackColor = System.Drawing.Color.White;
            this.txtSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSize.Location = new System.Drawing.Point(685, 67);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(135, 23);
            this.txtSize.TabIndex = 19;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(916, 12);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 20;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(790, 11);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(103, 30);
            this.btnToExcel.TabIndex = 0;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.btnToExcel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(899, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(97, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // bgWorkerUpdateInfo
            // 
            this.bgWorkerUpdateInfo.WorkerReportsProgress = true;
            this.bgWorkerUpdateInfo.WorkerSupportsCancellation = true;
            this.bgWorkerUpdateInfo.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerUpdateInfo_DoWork);
            this.bgWorkerUpdateInfo.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerUpdateInfo_RunWorkerCompleted);
            // 
            // P40
            // 
            this.ClientSize = new System.Drawing.Size(1008, 562);
            this.Controls.Add(this.labsubpro);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P40";
            this.Text = "P40. Search Location (RFID)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.P40_FormClosing);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.labsubpro, 0);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.labsubpro.ResumeLayout(false);
            this.labsubpro.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Win.UI.Grid grid;
        private System.Windows.Forms.Panel labsubpro;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtSp2;
        private Win.UI.TextBox txtSp1;
        private Win.UI.Label labLocation;
        private Win.UI.Label labInlineDate;
        private Win.UI.Label labSpno;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label label2;
        private Win.UI.Label labinline;
        private Win.UI.Label labFty;
        private Win.UI.TextBox txtLocation;
        private Win.UI.DateRange dateRangeInlineDate;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnToExcel;
        private Win.UI.Button btnQuery;
        private Win.UI.TextBox txtSize;
        private Win.UI.TextBox txtArticle;
        private Win.UI.TextBox txtCombo;
        private Win.UI.DisplayBox displaySubProcess;
        private Class.txtsewingline txtsewingline;
        private Class.txtfactory txtfactory;
        private Win.UI.Label labSize;
        private Win.UI.Label labArticle;
        private Win.UI.Label labCombo;
        private System.ComponentModel.BackgroundWorker bgWorkerUpdateInfo;
    }
}
