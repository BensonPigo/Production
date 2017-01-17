namespace Sci.Production.Planning
{
    partial class R13
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.radioGroup1 = new Sci.Win.UI.RadioGroup();
            this.panelReportType = new Sci.Win.UI.Panel();
            this.rdMDP = new System.Windows.Forms.RadioButton();
            this.rdFactory = new System.Windows.Forms.RadioButton();
            this.rdAGC = new System.Windows.Forms.RadioButton();
            this.numMonth = new System.Windows.Forms.NumericUpDown();
            this.numYear = new System.Windows.Forms.NumericUpDown();
            this.lbSource = new Sci.Win.UI.Label();
            this.lbReport = new Sci.Win.UI.Label();
            this.lbMonth = new Sci.Win.UI.Label();
            this.lbYear = new Sci.Win.UI.Label();
            this.chkDetail = new Sci.Win.UI.CheckBox();
            this.panelByType = new Sci.Win.UI.Panel();
            this.rdByAGC = new System.Windows.Forms.RadioButton();
            this.rdByFactory = new System.Windows.Forms.RadioButton();
            this.rdByAll = new System.Windows.Forms.RadioButton();
            this.panelSourceType = new Sci.Win.UI.Panel();
            this.rdAdidas = new System.Windows.Forms.RadioButton();
            this.rdReebok = new System.Windows.Forms.RadioButton();
            this.rdAll = new System.Windows.Forms.RadioButton();
            this.lbRemark = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.grid1 = new Sci.Win.UI.Grid();
            this.btnEdit = new Sci.Win.UI.Button();
            this.btnUndo = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.radioGroup1.SuspendLayout();
            this.panelReportType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).BeginInit();
            this.panelByType.SuspendLayout();
            this.panelSourceType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(340, 17);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(340, 53);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(340, 89);
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.lbRemark);
            this.radioGroup1.Controls.Add(this.panelSourceType);
            this.radioGroup1.Controls.Add(this.panelByType);
            this.radioGroup1.Controls.Add(this.chkDetail);
            this.radioGroup1.Controls.Add(this.panelReportType);
            this.radioGroup1.Controls.Add(this.numMonth);
            this.radioGroup1.Controls.Add(this.numYear);
            this.radioGroup1.Controls.Add(this.lbSource);
            this.radioGroup1.Controls.Add(this.lbReport);
            this.radioGroup1.Controls.Add(this.lbMonth);
            this.radioGroup1.Controls.Add(this.lbYear);
            this.radioGroup1.Location = new System.Drawing.Point(12, 3);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(314, 282);
            this.radioGroup1.TabIndex = 94;
            this.radioGroup1.TabStop = false;
            // 
            // panelReportType
            // 
            this.panelReportType.Controls.Add(this.rdMDP);
            this.panelReportType.Controls.Add(this.rdFactory);
            this.panelReportType.Controls.Add(this.rdAGC);
            this.panelReportType.Location = new System.Drawing.Point(58, 70);
            this.panelReportType.Name = "panelReportType";
            this.panelReportType.Size = new System.Drawing.Size(124, 80);
            this.panelReportType.TabIndex = 6;
            // 
            // rdMDP
            // 
            this.rdMDP.AutoSize = true;
            this.rdMDP.ForeColor = System.Drawing.Color.Red;
            this.rdMDP.Location = new System.Drawing.Point(2, 53);
            this.rdMDP.Name = "rdMDP";
            this.rdMDP.Size = new System.Drawing.Size(103, 21);
            this.rdMDP.TabIndex = 9;
            this.rdMDP.Text = "MDP Report";
            this.rdMDP.UseVisualStyleBackColor = true;
            // 
            // rdFactory
            // 
            this.rdFactory.AutoSize = true;
            this.rdFactory.ForeColor = System.Drawing.Color.Red;
            this.rdFactory.Location = new System.Drawing.Point(1, 29);
            this.rdFactory.Name = "rdFactory";
            this.rdFactory.Size = new System.Drawing.Size(120, 21);
            this.rdFactory.TabIndex = 8;
            this.rdFactory.Text = "Factory Report";
            this.rdFactory.UseVisualStyleBackColor = true;
            // 
            // rdAGC
            // 
            this.rdAGC.AutoSize = true;
            this.rdAGC.Checked = true;
            this.rdAGC.ForeColor = System.Drawing.Color.Red;
            this.rdAGC.Location = new System.Drawing.Point(1, 5);
            this.rdAGC.Name = "rdAGC";
            this.rdAGC.Size = new System.Drawing.Size(102, 21);
            this.rdAGC.TabIndex = 7;
            this.rdAGC.TabStop = true;
            this.rdAGC.Text = "AGC Report";
            this.rdAGC.UseVisualStyleBackColor = true;
            // 
            // numMonth
            // 
            this.numMonth.Location = new System.Drawing.Point(185, 19);
            this.numMonth.Name = "numMonth";
            this.numMonth.Size = new System.Drawing.Size(59, 23);
            this.numMonth.TabIndex = 5;
            // 
            // numYear
            // 
            this.numYear.Location = new System.Drawing.Point(46, 19);
            this.numYear.Name = "numYear";
            this.numYear.Size = new System.Drawing.Size(68, 23);
            this.numYear.TabIndex = 4;
            // 
            // lbSource
            // 
            this.lbSource.Lines = 0;
            this.lbSource.Location = new System.Drawing.Point(3, 207);
            this.lbSource.Name = "lbSource";
            this.lbSource.Size = new System.Drawing.Size(53, 23);
            this.lbSource.TabIndex = 3;
            this.lbSource.Text = "Source";
            // 
            // lbReport
            // 
            this.lbReport.Lines = 0;
            this.lbReport.Location = new System.Drawing.Point(3, 70);
            this.lbReport.Name = "lbReport";
            this.lbReport.Size = new System.Drawing.Size(52, 23);
            this.lbReport.TabIndex = 2;
            this.lbReport.Text = "Report";
            // 
            // lbMonth
            // 
            this.lbMonth.Lines = 0;
            this.lbMonth.Location = new System.Drawing.Point(133, 19);
            this.lbMonth.Name = "lbMonth";
            this.lbMonth.Size = new System.Drawing.Size(49, 23);
            this.lbMonth.TabIndex = 1;
            this.lbMonth.Text = "Month";
            // 
            // lbYear
            // 
            this.lbYear.Lines = 0;
            this.lbYear.Location = new System.Drawing.Point(3, 19);
            this.lbYear.Name = "lbYear";
            this.lbYear.Size = new System.Drawing.Size(40, 23);
            this.lbYear.TabIndex = 0;
            this.lbYear.Text = "Year";
            // 
            // chkDetail
            // 
            this.chkDetail.AutoSize = true;
            this.chkDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkDetail.Location = new System.Drawing.Point(191, 72);
            this.chkDetail.Name = "chkDetail";
            this.chkDetail.Size = new System.Drawing.Size(123, 21);
            this.chkDetail.TabIndex = 7;
            this.chkDetail.Text = "List Detail Data";
            this.chkDetail.UseVisualStyleBackColor = true;
            // 
            // panelByType
            // 
            this.panelByType.Controls.Add(this.rdByAll);
            this.panelByType.Controls.Add(this.rdByFactory);
            this.panelByType.Controls.Add(this.rdByAGC);
            this.panelByType.Location = new System.Drawing.Point(191, 123);
            this.panelByType.Name = "panelByType";
            this.panelByType.Size = new System.Drawing.Size(111, 76);
            this.panelByType.TabIndex = 8;
            // 
            // rdByAGC
            // 
            this.rdByAGC.AutoSize = true;
            this.rdByAGC.Checked = true;
            this.rdByAGC.ForeColor = System.Drawing.Color.Red;
            this.rdByAGC.Location = new System.Drawing.Point(2, 6);
            this.rdByAGC.Name = "rdByAGC";
            this.rdByAGC.Size = new System.Drawing.Size(74, 21);
            this.rdByAGC.TabIndex = 0;
            this.rdByAGC.TabStop = true;
            this.rdByAGC.Text = "by AGC";
            this.rdByAGC.UseVisualStyleBackColor = true;
            // 
            // rdByFactory
            // 
            this.rdByFactory.AutoSize = true;
            this.rdByFactory.ForeColor = System.Drawing.Color.Red;
            this.rdByFactory.Location = new System.Drawing.Point(2, 31);
            this.rdByFactory.Name = "rdByFactory";
            this.rdByFactory.Size = new System.Drawing.Size(92, 21);
            this.rdByFactory.TabIndex = 1;
            this.rdByFactory.Text = "by Factory";
            this.rdByFactory.UseVisualStyleBackColor = true;
            // 
            // rdByAll
            // 
            this.rdByAll.AutoSize = true;
            this.rdByAll.ForeColor = System.Drawing.Color.Red;
            this.rdByAll.Location = new System.Drawing.Point(2, 54);
            this.rdByAll.Name = "rdByAll";
            this.rdByAll.Size = new System.Drawing.Size(60, 21);
            this.rdByAll.TabIndex = 2;
            this.rdByAll.Text = "by All";
            this.rdByAll.UseVisualStyleBackColor = true;
            // 
            // panelSourceType
            // 
            this.panelSourceType.Controls.Add(this.rdAll);
            this.panelSourceType.Controls.Add(this.rdReebok);
            this.panelSourceType.Controls.Add(this.rdAdidas);
            this.panelSourceType.Location = new System.Drawing.Point(58, 207);
            this.panelSourceType.Name = "panelSourceType";
            this.panelSourceType.Size = new System.Drawing.Size(209, 26);
            this.panelSourceType.TabIndex = 9;
            // 
            // rdAdidas
            // 
            this.rdAdidas.AutoSize = true;
            this.rdAdidas.Checked = true;
            this.rdAdidas.ForeColor = System.Drawing.Color.Red;
            this.rdAdidas.Location = new System.Drawing.Point(3, 2);
            this.rdAdidas.Name = "rdAdidas";
            this.rdAdidas.Size = new System.Drawing.Size(69, 21);
            this.rdAdidas.TabIndex = 0;
            this.rdAdidas.TabStop = true;
            this.rdAdidas.Text = "Adidas";
            this.rdAdidas.UseVisualStyleBackColor = true;
            // 
            // rdReebok
            // 
            this.rdReebok.AutoSize = true;
            this.rdReebok.ForeColor = System.Drawing.Color.Red;
            this.rdReebok.Location = new System.Drawing.Point(78, 2);
            this.rdReebok.Name = "rdReebok";
            this.rdReebok.Size = new System.Drawing.Size(75, 21);
            this.rdReebok.TabIndex = 1;
            this.rdReebok.TabStop = true;
            this.rdReebok.Text = "Reebok";
            this.rdReebok.UseVisualStyleBackColor = true;
            // 
            // rdAll
            // 
            this.rdAll.AutoSize = true;
            this.rdAll.ForeColor = System.Drawing.Color.Red;
            this.rdAll.Location = new System.Drawing.Point(160, 2);
            this.rdAll.Name = "rdAll";
            this.rdAll.Size = new System.Drawing.Size(41, 21);
            this.rdAll.TabIndex = 2;
            this.rdAll.TabStop = true;
            this.rdAll.Text = "All";
            this.rdAll.UseVisualStyleBackColor = true;
            // 
            // lbRemark
            // 
            this.lbRemark.BackColor = System.Drawing.SystemColors.Control;
            this.lbRemark.Lines = 0;
            this.lbRemark.Location = new System.Drawing.Point(3, 248);
            this.lbRemark.Name = "lbRemark";
            this.lbRemark.Size = new System.Drawing.Size(311, 23);
            this.lbRemark.TabIndex = 10;
            this.lbRemark.Text = "PS:Sample Order has been Excluded";
            this.lbRemark.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(334, 224);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 23);
            this.label1.TabIndex = 95;
            this.label1.Text = "Paper Size A4";
            this.label1.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid1.DefaultCellStyle = dataGridViewCellStyle11;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(431, 12);
            this.grid1.Name = "grid1";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(351, 273);
            this.grid1.TabIndex = 96;
            this.grid1.TabStop = false;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(431, 285);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(90, 36);
            this.btnEdit.TabIndex = 97;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            // 
            // btnUndo
            // 
            this.btnUndo.Location = new System.Drawing.Point(619, 285);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(79, 36);
            this.btnUndo.TabIndex = 98;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(704, 285);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 36);
            this.btnSave.TabIndex = 99;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // R13
            // 
            this.ClientSize = new System.Drawing.Size(789, 351);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioGroup1);
            this.Name = "R13";
            this.Text = "R13. Adidas KPI Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioGroup1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.grid1, 0);
            this.Controls.SetChildIndex(this.btnEdit, 0);
            this.Controls.SetChildIndex(this.btnUndo, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            this.panelReportType.ResumeLayout(false);
            this.panelReportType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).EndInit();
            this.panelByType.ResumeLayout(false);
            this.panelByType.PerformLayout();
            this.panelSourceType.ResumeLayout(false);
            this.panelSourceType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioGroup radioGroup1;
        private Win.UI.Panel panelReportType;
        private System.Windows.Forms.RadioButton rdMDP;
        private System.Windows.Forms.RadioButton rdFactory;
        private System.Windows.Forms.RadioButton rdAGC;
        private System.Windows.Forms.NumericUpDown numMonth;
        private System.Windows.Forms.NumericUpDown numYear;
        private Win.UI.Label lbSource;
        private Win.UI.Label lbReport;
        private Win.UI.Label lbMonth;
        private Win.UI.Label lbYear;
        private Win.UI.Panel panelByType;
        private System.Windows.Forms.RadioButton rdByAll;
        private System.Windows.Forms.RadioButton rdByFactory;
        private System.Windows.Forms.RadioButton rdByAGC;
        private Win.UI.CheckBox chkDetail;
        private Win.UI.Panel panelSourceType;
        private Win.UI.Label lbRemark;
        private System.Windows.Forms.RadioButton rdAll;
        private System.Windows.Forms.RadioButton rdReebok;
        private System.Windows.Forms.RadioButton rdAdidas;
        private Win.UI.Label label1;
        private Win.UI.Grid grid1;
        private Win.UI.Button btnEdit;
        private Win.UI.Button btnUndo;
        private Win.UI.Button btnSave;
    }
}
