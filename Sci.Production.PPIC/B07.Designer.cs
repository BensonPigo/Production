namespace Sci.Production.PPIC
{
    partial class B07
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
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelDate = new Sci.Win.UI.Label();
            this.labelDay = new Sci.Win.UI.Label();
            this.labelSewingLine = new Sci.Win.UI.Label();
            this.labelWorkingHours = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.displayFactory = new Sci.Win.UI.DisplayBox();
            this.dateDate = new Sci.Win.UI.DateBox();
            this.displayDay = new Sci.Win.UI.DisplayBox();
            this.numWorkingHours = new Sci.Win.UI.NumericBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.checkHoliday = new Sci.Win.UI.CheckBox();
            this.btnBatchEdit = new Sci.Win.UI.Button();
            this.txtSewingLine = new Sci.Production.Class.Txtsewingline();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(827, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.btnBatchEdit);
            this.detailcont.Controls.Add(this.checkHoliday);
            this.detailcont.Controls.Add(this.txtRemark);
            this.detailcont.Controls.Add(this.numWorkingHours);
            this.detailcont.Controls.Add(this.txtSewingLine);
            this.detailcont.Controls.Add(this.displayDay);
            this.detailcont.Controls.Add(this.dateDate);
            this.detailcont.Controls.Add(this.displayFactory);
            this.detailcont.Controls.Add(this.labelRemark);
            this.detailcont.Controls.Add(this.labelWorkingHours);
            this.detailcont.Controls.Add(this.labelSewingLine);
            this.detailcont.Controls.Add(this.labelDay);
            this.detailcont.Controls.Add(this.labelDate);
            this.detailcont.Controls.Add(this.labelFactory);
            this.detailcont.Size = new System.Drawing.Size(827, 357);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(827, 38);
            this.detailbtm.TabIndex = 1;
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(827, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(835, 424);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(30, 30);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(96, 23);
            this.labelFactory.TabIndex = 0;
            this.labelFactory.Text = "Factory";
            // 
            // labelDate
            // 
            this.labelDate.Lines = 0;
            this.labelDate.Location = new System.Drawing.Point(30, 65);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(96, 23);
            this.labelDate.TabIndex = 1;
            this.labelDate.Text = "Date";
            // 
            // labelDay
            // 
            this.labelDay.Lines = 0;
            this.labelDay.Location = new System.Drawing.Point(30, 100);
            this.labelDay.Name = "labelDay";
            this.labelDay.Size = new System.Drawing.Size(96, 23);
            this.labelDay.TabIndex = 2;
            this.labelDay.Text = "Day";
            // 
            // labelSewingLine
            // 
            this.labelSewingLine.Lines = 0;
            this.labelSewingLine.Location = new System.Drawing.Point(30, 135);
            this.labelSewingLine.Name = "labelSewingLine";
            this.labelSewingLine.Size = new System.Drawing.Size(96, 23);
            this.labelSewingLine.TabIndex = 3;
            this.labelSewingLine.Text = "Sewing Line";
            // 
            // labelWorkingHours
            // 
            this.labelWorkingHours.Lines = 0;
            this.labelWorkingHours.Location = new System.Drawing.Point(30, 170);
            this.labelWorkingHours.Name = "labelWorkingHours";
            this.labelWorkingHours.Size = new System.Drawing.Size(96, 23);
            this.labelWorkingHours.TabIndex = 4;
            this.labelWorkingHours.Text = "Working hours";
            // 
            // labelRemark
            // 
            this.labelRemark.Lines = 0;
            this.labelRemark.Location = new System.Drawing.Point(30, 205);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(96, 23);
            this.labelRemark.TabIndex = 5;
            this.labelRemark.Text = "Remark";
            // 
            // displayFactory
            // 
            this.displayFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFactory.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FactoryID", true));
            this.displayFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFactory.Location = new System.Drawing.Point(130, 30);
            this.displayFactory.Name = "displayFactory";
            this.displayFactory.Size = new System.Drawing.Size(60, 23);
            this.displayFactory.TabIndex = 0;
            // 
            // dateDate
            // 
            this.dateDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Date", true));
            this.dateDate.Location = new System.Drawing.Point(130, 65);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(130, 23);
            this.dateDate.TabIndex = 1;
            // 
            // displayDay
            // 
            this.displayDay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDay.Location = new System.Drawing.Point(130, 100);
            this.displayDay.Name = "displayDay";
            this.displayDay.Size = new System.Drawing.Size(94, 23);
            this.displayDay.TabIndex = 2;
            // 
            // numWorkingHours
            // 
            this.numWorkingHours.BackColor = System.Drawing.Color.White;
            this.numWorkingHours.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Hours", true));
            this.numWorkingHours.DecimalPlaces = 1;
            this.numWorkingHours.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numWorkingHours.Location = new System.Drawing.Point(130, 170);
            this.numWorkingHours.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            65536});
            this.numWorkingHours.MaxLength = 4;
            this.numWorkingHours.Name = "numWorkingHours";
            this.numWorkingHours.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWorkingHours.Size = new System.Drawing.Size(45, 23);
            this.numWorkingHours.TabIndex = 4;
            this.numWorkingHours.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(130, 205);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(318, 23);
            this.txtRemark.TabIndex = 5;
            // 
            // checkHoliday
            // 
            this.checkHoliday.AutoSize = true;
            this.checkHoliday.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Holiday", true));
            this.checkHoliday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkHoliday.Location = new System.Drawing.Point(302, 65);
            this.checkHoliday.Name = "checkHoliday";
            this.checkHoliday.Size = new System.Drawing.Size(74, 21);
            this.checkHoliday.TabIndex = 6;
            this.checkHoliday.Text = "Holiday";
            this.checkHoliday.UseVisualStyleBackColor = true;
            // 
            // btnBatchEdit
            // 
            this.btnBatchEdit.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnBatchEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBatchEdit.Location = new System.Drawing.Point(534, 26);
            this.btnBatchEdit.Name = "btnBatchEdit";
            this.btnBatchEdit.Size = new System.Drawing.Size(96, 30);
            this.btnBatchEdit.TabIndex = 7;
            this.btnBatchEdit.Text = "Batch Edit";
            this.btnBatchEdit.UseVisualStyleBackColor = true;
            this.btnBatchEdit.Click += new System.EventHandler(this.BtnBatchEdit_Click);
            // 
            // txtSewingLine
            // 
            this.txtSewingLine.BackColor = System.Drawing.Color.White;
            this.txtSewingLine.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SewingLineID", true));
            this.txtSewingLine.FactoryobjectName = this.displayFactory;
            this.txtSewingLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSewingLine.Location = new System.Drawing.Point(130, 135);
            this.txtSewingLine.Name = "txtSewingLine";
            this.txtSewingLine.Size = new System.Drawing.Size(60, 23);
            this.txtSewingLine.TabIndex = 3;
            // 
            // B07
            // 
            this.ClientSize = new System.Drawing.Size(835, 457);
            this.DefaultOrder = "Date,SewingLineID";
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.Name = "B07";
            this.Text = "B07. Work hours by sewing line ";
            this.UniqueExpress = "FactoryID,SewingLineID,Date";
            this.WorkAlias = "WorkHour";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.CheckBox checkHoliday;
        private Win.UI.TextBox txtRemark;
        private Win.UI.NumericBox numWorkingHours;
        private Class.Txtsewingline txtSewingLine;
        private Win.UI.DisplayBox displayDay;
        private Win.UI.DateBox dateDate;
        private Win.UI.DisplayBox displayFactory;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelWorkingHours;
        private Win.UI.Label labelSewingLine;
        private Win.UI.Label labelDay;
        private Win.UI.Label labelDate;
        private Win.UI.Label labelFactory;
        private Win.UI.Button btnBatchEdit;
    }
}
