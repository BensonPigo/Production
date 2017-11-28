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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.radioGroup1 = new Sci.Win.UI.RadioGroup();
            this.lbRemark = new Sci.Win.UI.Label();
            this.panelSourceType = new Sci.Win.UI.Panel();
            this.radioAll = new System.Windows.Forms.RadioButton();
            this.radioReebok = new System.Windows.Forms.RadioButton();
            this.radioAdidas = new System.Windows.Forms.RadioButton();
            this.panelByType = new Sci.Win.UI.Panel();
            this.radioByAll = new System.Windows.Forms.RadioButton();
            this.radioByFactory = new System.Windows.Forms.RadioButton();
            this.radioByAGC = new System.Windows.Forms.RadioButton();
            this.checkDetail = new Sci.Win.UI.CheckBox();
            this.panelReportType = new Sci.Win.UI.Panel();
            this.radioMDP = new System.Windows.Forms.RadioButton();
            this.radioFactory = new System.Windows.Forms.RadioButton();
            this.radioAGC = new System.Windows.Forms.RadioButton();
            this.numMonth = new System.Windows.Forms.NumericUpDown();
            this.numYear = new System.Windows.Forms.NumericUpDown();
            this.lbSource = new Sci.Win.UI.Label();
            this.lbReport = new Sci.Win.UI.Label();
            this.lbMonth = new Sci.Win.UI.Label();
            this.lbYear = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.gridAdidasKPIReport = new Sci.Win.UI.Grid();
            this.btnEdit = new Sci.Win.UI.Button();
            this.btnUndo = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.bindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.radioGroup1.SuspendLayout();
            this.panelSourceType.SuspendLayout();
            this.panelByType.SuspendLayout();
            this.panelReportType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAdidasKPIReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(340, 17);
            this.print.TabIndex = 5;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(340, 53);
            this.toexcel.TabIndex = 6;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(340, 89);
            this.close.TabIndex = 7;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.lbRemark);
            this.radioGroup1.Controls.Add(this.panelSourceType);
            this.radioGroup1.Controls.Add(this.panelByType);
            this.radioGroup1.Controls.Add(this.checkDetail);
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
            this.radioGroup1.TabIndex = 1;
            this.radioGroup1.TabStop = false;
            // 
            // lbRemark
            // 
            this.lbRemark.BackColor = System.Drawing.SystemColors.Control;
            this.lbRemark.Location = new System.Drawing.Point(3, 248);
            this.lbRemark.Name = "lbRemark";
            this.lbRemark.Size = new System.Drawing.Size(241, 23);
            this.lbRemark.TabIndex = 10;
            this.lbRemark.Text = "PS:Sample Order has been Excluded";
            this.lbRemark.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // panelSourceType
            // 
            this.panelSourceType.Controls.Add(this.radioAll);
            this.panelSourceType.Controls.Add(this.radioReebok);
            this.panelSourceType.Controls.Add(this.radioAdidas);
            this.panelSourceType.Location = new System.Drawing.Point(58, 207);
            this.panelSourceType.Name = "panelSourceType";
            this.panelSourceType.Size = new System.Drawing.Size(209, 26);
            this.panelSourceType.TabIndex = 9;
            // 
            // radioAll
            // 
            this.radioAll.AutoSize = true;
            this.radioAll.ForeColor = System.Drawing.Color.Red;
            this.radioAll.Location = new System.Drawing.Point(160, 2);
            this.radioAll.Name = "radioAll";
            this.radioAll.Size = new System.Drawing.Size(41, 21);
            this.radioAll.TabIndex = 2;
            this.radioAll.TabStop = true;
            this.radioAll.Text = "All";
            this.radioAll.UseVisualStyleBackColor = true;
            // 
            // radioReebok
            // 
            this.radioReebok.AutoSize = true;
            this.radioReebok.ForeColor = System.Drawing.Color.Red;
            this.radioReebok.Location = new System.Drawing.Point(78, 2);
            this.radioReebok.Name = "radioReebok";
            this.radioReebok.Size = new System.Drawing.Size(75, 21);
            this.radioReebok.TabIndex = 1;
            this.radioReebok.TabStop = true;
            this.radioReebok.Text = "Reebok";
            this.radioReebok.UseVisualStyleBackColor = true;
            // 
            // radioAdidas
            // 
            this.radioAdidas.AutoSize = true;
            this.radioAdidas.Checked = true;
            this.radioAdidas.ForeColor = System.Drawing.Color.Red;
            this.radioAdidas.Location = new System.Drawing.Point(3, 2);
            this.radioAdidas.Name = "radioAdidas";
            this.radioAdidas.Size = new System.Drawing.Size(69, 21);
            this.radioAdidas.TabIndex = 0;
            this.radioAdidas.TabStop = true;
            this.radioAdidas.Text = "Adidas";
            this.radioAdidas.UseVisualStyleBackColor = true;
            // 
            // panelByType
            // 
            this.panelByType.Controls.Add(this.radioByAll);
            this.panelByType.Controls.Add(this.radioByFactory);
            this.panelByType.Controls.Add(this.radioByAGC);
            this.panelByType.Location = new System.Drawing.Point(191, 123);
            this.panelByType.Name = "panelByType";
            this.panelByType.Size = new System.Drawing.Size(111, 76);
            this.panelByType.TabIndex = 8;
            this.panelByType.Visible = false;
            // 
            // radioByAll
            // 
            this.radioByAll.AutoSize = true;
            this.radioByAll.ForeColor = System.Drawing.Color.Red;
            this.radioByAll.Location = new System.Drawing.Point(2, 54);
            this.radioByAll.Name = "radioByAll";
            this.radioByAll.Size = new System.Drawing.Size(60, 21);
            this.radioByAll.TabIndex = 2;
            this.radioByAll.Text = "by All";
            this.radioByAll.UseVisualStyleBackColor = true;
            // 
            // radioByFactory
            // 
            this.radioByFactory.AutoSize = true;
            this.radioByFactory.ForeColor = System.Drawing.Color.Red;
            this.radioByFactory.Location = new System.Drawing.Point(2, 31);
            this.radioByFactory.Name = "radioByFactory";
            this.radioByFactory.Size = new System.Drawing.Size(92, 21);
            this.radioByFactory.TabIndex = 1;
            this.radioByFactory.Text = "by Factory";
            this.radioByFactory.UseVisualStyleBackColor = true;
            // 
            // radioByAGC
            // 
            this.radioByAGC.AutoSize = true;
            this.radioByAGC.Checked = true;
            this.radioByAGC.ForeColor = System.Drawing.Color.Red;
            this.radioByAGC.Location = new System.Drawing.Point(2, 6);
            this.radioByAGC.Name = "radioByAGC";
            this.radioByAGC.Size = new System.Drawing.Size(74, 21);
            this.radioByAGC.TabIndex = 0;
            this.radioByAGC.TabStop = true;
            this.radioByAGC.Text = "by AGC";
            this.radioByAGC.UseVisualStyleBackColor = true;
            // 
            // checkDetail
            // 
            this.checkDetail.AutoSize = true;
            this.checkDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkDetail.Location = new System.Drawing.Point(191, 72);
            this.checkDetail.Name = "checkDetail";
            this.checkDetail.Size = new System.Drawing.Size(123, 21);
            this.checkDetail.TabIndex = 2;
            this.checkDetail.Text = "List Detail Data";
            this.checkDetail.UseVisualStyleBackColor = true;
            // 
            // panelReportType
            // 
            this.panelReportType.Controls.Add(this.radioMDP);
            this.panelReportType.Controls.Add(this.radioFactory);
            this.panelReportType.Controls.Add(this.radioAGC);
            this.panelReportType.Location = new System.Drawing.Point(58, 70);
            this.panelReportType.Name = "panelReportType";
            this.panelReportType.Size = new System.Drawing.Size(124, 80);
            this.panelReportType.TabIndex = 2;
            // 
            // radioMDP
            // 
            this.radioMDP.AutoSize = true;
            this.radioMDP.ForeColor = System.Drawing.Color.Red;
            this.radioMDP.Location = new System.Drawing.Point(2, 53);
            this.radioMDP.Name = "radioMDP";
            this.radioMDP.Size = new System.Drawing.Size(103, 21);
            this.radioMDP.TabIndex = 2;
            this.radioMDP.Text = "MDP Report";
            this.radioMDP.UseVisualStyleBackColor = true;
            // 
            // radioFactory
            // 
            this.radioFactory.AutoSize = true;
            this.radioFactory.ForeColor = System.Drawing.Color.Red;
            this.radioFactory.Location = new System.Drawing.Point(1, 29);
            this.radioFactory.Name = "radioFactory";
            this.radioFactory.Size = new System.Drawing.Size(120, 21);
            this.radioFactory.TabIndex = 1;
            this.radioFactory.Text = "Factory Report";
            this.radioFactory.UseVisualStyleBackColor = true;
            // 
            // radioAGC
            // 
            this.radioAGC.AutoSize = true;
            this.radioAGC.Checked = true;
            this.radioAGC.ForeColor = System.Drawing.Color.Red;
            this.radioAGC.Location = new System.Drawing.Point(1, 5);
            this.radioAGC.Name = "radioAGC";
            this.radioAGC.Size = new System.Drawing.Size(102, 21);
            this.radioAGC.TabIndex = 0;
            this.radioAGC.TabStop = true;
            this.radioAGC.Text = "AGC Report";
            this.radioAGC.UseVisualStyleBackColor = true;
            // 
            // numMonth
            // 
            this.numMonth.Location = new System.Drawing.Point(185, 19);
            this.numMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMonth.Name = "numMonth";
            this.numMonth.Size = new System.Drawing.Size(59, 23);
            this.numMonth.TabIndex = 1;
            this.numMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numYear
            // 
            this.numYear.Location = new System.Drawing.Point(46, 19);
            this.numYear.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.numYear.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numYear.Name = "numYear";
            this.numYear.Size = new System.Drawing.Size(68, 23);
            this.numYear.TabIndex = 0;
            this.numYear.Value = new decimal(new int[] {
            2015,
            0,
            0,
            0});
            // 
            // lbSource
            // 
            this.lbSource.Location = new System.Drawing.Point(3, 207);
            this.lbSource.Name = "lbSource";
            this.lbSource.Size = new System.Drawing.Size(53, 23);
            this.lbSource.TabIndex = 3;
            this.lbSource.Text = "Source";
            // 
            // lbReport
            // 
            this.lbReport.Location = new System.Drawing.Point(3, 70);
            this.lbReport.Name = "lbReport";
            this.lbReport.Size = new System.Drawing.Size(52, 23);
            this.lbReport.TabIndex = 2;
            this.lbReport.Text = "Report";
            // 
            // lbMonth
            // 
            this.lbMonth.Location = new System.Drawing.Point(133, 19);
            this.lbMonth.Name = "lbMonth";
            this.lbMonth.Size = new System.Drawing.Size(49, 23);
            this.lbMonth.TabIndex = 1;
            this.lbMonth.Text = "Month";
            // 
            // lbYear
            // 
            this.lbYear.Location = new System.Drawing.Point(3, 19);
            this.lbYear.Name = "lbYear";
            this.lbYear.Size = new System.Drawing.Size(40, 23);
            this.lbYear.TabIndex = 0;
            this.lbYear.Text = "Year";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(334, 224);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 23);
            this.label1.TabIndex = 95;
            this.label1.Text = "Paper Size A4";
            this.label1.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // gridAdidasKPIReport
            // 
            this.gridAdidasKPIReport.AllowUserToAddRows = false;
            this.gridAdidasKPIReport.AllowUserToDeleteRows = false;
            this.gridAdidasKPIReport.AllowUserToResizeRows = false;
            this.gridAdidasKPIReport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridAdidasKPIReport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridAdidasKPIReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAdidasKPIReport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridAdidasKPIReport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridAdidasKPIReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridAdidasKPIReport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridAdidasKPIReport.Location = new System.Drawing.Point(431, 12);
            this.gridAdidasKPIReport.Name = "gridAdidasKPIReport";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridAdidasKPIReport.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridAdidasKPIReport.RowHeadersVisible = false;
            this.gridAdidasKPIReport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridAdidasKPIReport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridAdidasKPIReport.RowTemplate.Height = 24;
            this.gridAdidasKPIReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAdidasKPIReport.Size = new System.Drawing.Size(351, 273);
            this.gridAdidasKPIReport.TabIndex = 96;
            this.gridAdidasKPIReport.TabStop = false;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(431, 285);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(90, 41);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Location = new System.Drawing.Point(619, 285);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(79, 41);
            this.btnUndo.TabIndex = 3;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.BtnUndo_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(704, 285);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 41);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // R13
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 351);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.gridAdidasKPIReport);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioGroup1);
            this.DefaultControl = "numYear";
            this.DefaultControlForEdit = "numYear";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "R13";
            this.Text = "R13. Adidas KPI Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioGroup1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.gridAdidasKPIReport, 0);
            this.Controls.SetChildIndex(this.btnEdit, 0);
            this.Controls.SetChildIndex(this.btnUndo, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            this.panelSourceType.ResumeLayout(false);
            this.panelSourceType.PerformLayout();
            this.panelByType.ResumeLayout(false);
            this.panelByType.PerformLayout();
            this.panelReportType.ResumeLayout(false);
            this.panelReportType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAdidasKPIReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioGroup radioGroup1;
        private Win.UI.Panel panelReportType;
        private System.Windows.Forms.RadioButton radioMDP;
        private System.Windows.Forms.RadioButton radioFactory;
        private System.Windows.Forms.RadioButton radioAGC;
        private System.Windows.Forms.NumericUpDown numMonth;
        private System.Windows.Forms.NumericUpDown numYear;
        private Win.UI.Label lbSource;
        private Win.UI.Label lbReport;
        private Win.UI.Label lbMonth;
        private Win.UI.Label lbYear;
        private Win.UI.Panel panelByType;
        private System.Windows.Forms.RadioButton radioByAll;
        private System.Windows.Forms.RadioButton radioByFactory;
        private System.Windows.Forms.RadioButton radioByAGC;
        private Win.UI.CheckBox checkDetail;
        private Win.UI.Panel panelSourceType;
        private Win.UI.Label lbRemark;
        private System.Windows.Forms.RadioButton radioAll;
        private System.Windows.Forms.RadioButton radioReebok;
        private System.Windows.Forms.RadioButton radioAdidas;
        private Win.UI.Label label1;
        private Win.UI.Grid gridAdidasKPIReport;
        private Win.UI.Button btnEdit;
        private Win.UI.Button btnUndo;
        private Win.UI.Button btnSave;
        private Win.UI.ListControlBindingSource bindingSource1;
    }
}
