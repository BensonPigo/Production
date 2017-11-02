namespace Sci.Production.PPIC
{
    partial class B07_BatchAdd
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
            this.labelLine = new Sci.Win.UI.Label();
            this.labelDate = new Sci.Win.UI.Label();
            this.labelWeekDay = new Sci.Win.UI.Label();
            this.labelHours = new Sci.Win.UI.Label();
            this.dateDate = new Sci.Win.UI.DateRange();
            this.comboWeekDay = new Sci.Win.UI.ComboBox();
            this.numHours = new Sci.Win.UI.NumericBox();
            this.checkItsAHoliday = new Sci.Win.UI.CheckBox();
            this.btnOK = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            this.label5 = new Sci.Win.UI.Label();
            this.txtLineNoStart = new Sci.Win.UI.TextBox();
            this.txtLineNoEnd = new Sci.Win.UI.TextBox();
            this.SuspendLayout();
            // 
            // labelLine
            // 
            this.labelLine.Lines = 0;
            this.labelLine.Location = new System.Drawing.Point(13, 13);
            this.labelLine.Name = "labelLine";
            this.labelLine.Size = new System.Drawing.Size(70, 23);
            this.labelLine.TabIndex = 0;
            this.labelLine.Text = "Line#";
            // 
            // labelDate
            // 
            this.labelDate.Lines = 0;
            this.labelDate.Location = new System.Drawing.Point(13, 45);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(70, 23);
            this.labelDate.TabIndex = 1;
            this.labelDate.Text = "Date";
            // 
            // labelWeekDay
            // 
            this.labelWeekDay.Lines = 0;
            this.labelWeekDay.Location = new System.Drawing.Point(13, 78);
            this.labelWeekDay.Name = "labelWeekDay";
            this.labelWeekDay.Size = new System.Drawing.Size(70, 23);
            this.labelWeekDay.TabIndex = 2;
            this.labelWeekDay.Text = "Week day";
            // 
            // labelHours
            // 
            this.labelHours.Lines = 0;
            this.labelHours.Location = new System.Drawing.Point(13, 111);
            this.labelHours.Name = "labelHours";
            this.labelHours.Size = new System.Drawing.Size(70, 23);
            this.labelHours.TabIndex = 3;
            this.labelHours.Text = "Hours";
            // 
            // dateDate
            // 
            this.dateDate.IsRequired = false;
            this.dateDate.Location = new System.Drawing.Point(87, 45);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(280, 23);
            this.dateDate.TabIndex = 2;
            // 
            // comboWeekDay
            // 
            this.comboWeekDay.BackColor = System.Drawing.Color.White;
            this.comboWeekDay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboWeekDay.FormattingEnabled = true;
            this.comboWeekDay.IsSupportUnselect = true;
            this.comboWeekDay.Location = new System.Drawing.Point(87, 78);
            this.comboWeekDay.Name = "comboWeekDay";
            this.comboWeekDay.Size = new System.Drawing.Size(144, 24);
            this.comboWeekDay.TabIndex = 3;
            // 
            // numHours
            // 
            this.numHours.BackColor = System.Drawing.Color.White;
            this.numHours.DecimalPlaces = 1;
            this.numHours.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numHours.Location = new System.Drawing.Point(87, 111);
            this.numHours.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            65536});
            this.numHours.MaxLength = 4;
            this.numHours.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numHours.Name = "numHours";
            this.numHours.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numHours.Size = new System.Drawing.Size(46, 23);
            this.numHours.TabIndex = 4;
            this.numHours.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // checkItsAHoliday
            // 
            this.checkItsAHoliday.AutoSize = true;
            this.checkItsAHoliday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkItsAHoliday.IsSupportEditMode = false;
            this.checkItsAHoliday.Location = new System.Drawing.Point(13, 147);
            this.checkItsAHoliday.Name = "checkItsAHoliday";
            this.checkItsAHoliday.Size = new System.Drawing.Size(105, 21);
            this.checkItsAHoliday.TabIndex = 5;
            this.checkItsAHoliday.Text = "It\'s a holiday";
            this.checkItsAHoliday.UseVisualStyleBackColor = true;
            this.checkItsAHoliday.CheckedChanged += new System.EventHandler(this.CheckItsAHoliday_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnOK.Location = new System.Drawing.Point(196, 164);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 30);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(283, 164);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(130, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 23);
            this.label5.TabIndex = 12;
            this.label5.Text = "~";
            this.label5.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label5.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtLineNoStart
            // 
            this.txtLineNoStart.BackColor = System.Drawing.Color.White;
            this.txtLineNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLineNoStart.Location = new System.Drawing.Point(87, 13);
            this.txtLineNoStart.MaxLength = 2;
            this.txtLineNoStart.Name = "txtLineNoStart";
            this.txtLineNoStart.Size = new System.Drawing.Size(40, 23);
            this.txtLineNoStart.TabIndex = 0;
            this.txtLineNoStart.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtLineNoStart_PopUp);
            this.txtLineNoStart.Validating += new System.ComponentModel.CancelEventHandler(this.TxtLineNoStart_Validating);
            // 
            // txtLineNoEnd
            // 
            this.txtLineNoEnd.BackColor = System.Drawing.Color.White;
            this.txtLineNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLineNoEnd.Location = new System.Drawing.Point(148, 13);
            this.txtLineNoEnd.MaxLength = 2;
            this.txtLineNoEnd.Name = "txtLineNoEnd";
            this.txtLineNoEnd.Size = new System.Drawing.Size(40, 23);
            this.txtLineNoEnd.TabIndex = 1;
            this.txtLineNoEnd.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtLineNoStart_PopUp);
            this.txtLineNoEnd.Validating += new System.ComponentModel.CancelEventHandler(this.TxtLineNoStart_Validating);
            // 
            // B07_BatchAdd
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(377, 204);
            this.Controls.Add(this.txtLineNoEnd);
            this.Controls.Add(this.txtLineNoStart);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.checkItsAHoliday);
            this.Controls.Add(this.numHours);
            this.Controls.Add(this.comboWeekDay);
            this.Controls.Add(this.dateDate);
            this.Controls.Add(this.labelHours);
            this.Controls.Add(this.labelWeekDay);
            this.Controls.Add(this.labelDate);
            this.Controls.Add(this.labelLine);
            this.DefaultControl = "txtLineNoStart";
            this.Name = "B07_BatchAdd";
            this.Text = " Batch Edit/Add";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelLine;
        private Win.UI.Label labelDate;
        private Win.UI.Label labelWeekDay;
        private Win.UI.Label labelHours;
        private Win.UI.DateRange dateDate;
        private Win.UI.ComboBox comboWeekDay;
        private Win.UI.NumericBox numHours;
        private Win.UI.CheckBox checkItsAHoliday;
        private Win.UI.Button btnOK;
        private Win.UI.Button btnCancel;
        private Win.UI.Label label5;
        private Win.UI.TextBox txtLineNoStart;
        private Win.UI.TextBox txtLineNoEnd;
    }
}
