namespace Sci.Production.PPIC
{
    partial class B07_Add
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
            this.labelDate = new Sci.Win.UI.Label();
            this.dateDate = new Sci.Win.UI.DateRange();
            this.labelHours = new Sci.Win.UI.Label();
            this.numHours = new Sci.Win.UI.NumericBox();
            this.checkIncludeSaturday = new Sci.Win.UI.CheckBox();
            this.label3 = new Sci.Win.UI.Label();
            this.btnOK = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // labelDate
            // 
            this.labelDate.Lines = 0;
            this.labelDate.Location = new System.Drawing.Point(18, 14);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(48, 23);
            this.labelDate.TabIndex = 5;
            this.labelDate.Text = "Date";
            // 
            // dateDate
            // 
            this.dateDate.IsRequired = false;
            this.dateDate.Location = new System.Drawing.Point(69, 14);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(280, 23);
            this.dateDate.TabIndex = 0;
            // 
            // labelHours
            // 
            this.labelHours.Lines = 0;
            this.labelHours.Location = new System.Drawing.Point(18, 50);
            this.labelHours.Name = "labelHours";
            this.labelHours.Size = new System.Drawing.Size(48, 23);
            this.labelHours.TabIndex = 6;
            this.labelHours.Text = "Hours";
            // 
            // numHours
            // 
            this.numHours.BackColor = System.Drawing.Color.White;
            this.numHours.DecimalPlaces = 1;
            this.numHours.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numHours.Location = new System.Drawing.Point(69, 50);
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
            this.numHours.TabIndex = 1;
            this.numHours.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // checkIncludeSaturday
            // 
            this.checkIncludeSaturday.AutoSize = true;
            this.checkIncludeSaturday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIncludeSaturday.Location = new System.Drawing.Point(18, 85);
            this.checkIncludeSaturday.Name = "checkIncludeSaturday";
            this.checkIncludeSaturday.Size = new System.Drawing.Size(133, 21);
            this.checkIncludeSaturday.TabIndex = 2;
            this.checkIncludeSaturday.Text = "Include Saturday";
            this.checkIncludeSaturday.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(18, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(190, 23);
            this.label3.TabIndex = 7;
            this.label3.Text = "Sunday will be non-office day";
            this.label3.TextStyle.BorderColor = System.Drawing.Color.Blue;
            this.label3.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnOK.Location = new System.Drawing.Point(178, 144);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 30);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(264, 144);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // B07_Add
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(360, 187);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkIncludeSaturday);
            this.Controls.Add(this.numHours);
            this.Controls.Add(this.labelHours);
            this.Controls.Add(this.dateDate);
            this.Controls.Add(this.labelDate);
            this.DefaultControl = "dateDate";
            this.Name = "B07_Add";
            this.Text = "Add";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelDate;
        private Win.UI.DateRange dateDate;
        private Win.UI.Label labelHours;
        private Win.UI.NumericBox numHours;
        private Win.UI.CheckBox checkIncludeSaturday;
        private Win.UI.Label label3;
        private Win.UI.Button btnOK;
        private Win.UI.Button btnCancel;
    }
}
