namespace Sci.Production.Shipping
{
    partial class R06
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
            this.labelApvDate = new Sci.Win.UI.Label();
            this.labelBLNo = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelOrderby = new Sci.Win.UI.Label();
            this.dateDate = new Sci.Win.UI.DateRange();
            this.dateApvDate = new Sci.Win.UI.DateRange();
            this.txtBLNoStart = new Sci.Win.UI.TextBox();
            this.txtBLNoEnd = new Sci.Win.UI.TextBox();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.txtSubconSupplier = new Sci.Production.Class.txtsubcon();
            this.comboOrderby = new Sci.Win.UI.ComboBox();
            this.label7 = new Sci.Win.UI.Label();
            this.labelReportType = new Sci.Win.UI.Label();
            this.radioSummary = new Sci.Win.UI.RadioButton();
            this.radioDetail = new Sci.Win.UI.RadioButton();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(392, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(392, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(392, 84);
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(13, 12);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(86, 23);
            this.labelDate.TabIndex = 94;
            this.labelDate.Text = "Date";
            // 
            // labelApvDate
            // 
            this.labelApvDate.Location = new System.Drawing.Point(13, 48);
            this.labelApvDate.Name = "labelApvDate";
            this.labelApvDate.Size = new System.Drawing.Size(86, 23);
            this.labelApvDate.TabIndex = 95;
            this.labelApvDate.Text = "Apv. Date";
            // 
            // labelBLNo
            // 
            this.labelBLNo.Location = new System.Drawing.Point(13, 84);
            this.labelBLNo.Name = "labelBLNo";
            this.labelBLNo.Size = new System.Drawing.Size(86, 23);
            this.labelBLNo.TabIndex = 96;
            this.labelBLNo.Text = "BL No.";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 148);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(86, 23);
            this.labelM.TabIndex = 97;
            this.labelM.Text = "M";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(13, 184);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(86, 23);
            this.labelSupplier.TabIndex = 98;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelOrderby
            // 
            this.labelOrderby.Location = new System.Drawing.Point(13, 221);
            this.labelOrderby.Name = "labelOrderby";
            this.labelOrderby.Size = new System.Drawing.Size(86, 23);
            this.labelOrderby.TabIndex = 99;
            this.labelOrderby.Text = "Order by";
            // 
            // dateDate
            // 
            // 
            // 
            // 
            this.dateDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateDate.DateBox1.Name = "";
            this.dateDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateDate.DateBox2.Name = "";
            this.dateDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateDate.DateBox2.TabIndex = 1;
            this.dateDate.IsRequired = false;
            this.dateDate.Location = new System.Drawing.Point(105, 12);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(280, 23);
            this.dateDate.TabIndex = 100;
            // 
            // dateApvDate
            // 
            // 
            // 
            // 
            this.dateApvDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateApvDate.DateBox1.Name = "";
            this.dateApvDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateApvDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateApvDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateApvDate.DateBox2.Name = "";
            this.dateApvDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateApvDate.DateBox2.TabIndex = 1;
            this.dateApvDate.IsRequired = false;
            this.dateApvDate.Location = new System.Drawing.Point(105, 48);
            this.dateApvDate.Name = "dateApvDate";
            this.dateApvDate.Size = new System.Drawing.Size(280, 23);
            this.dateApvDate.TabIndex = 101;
            // 
            // txtBLNoStart
            // 
            this.txtBLNoStart.BackColor = System.Drawing.Color.White;
            this.txtBLNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBLNoStart.Location = new System.Drawing.Point(105, 84);
            this.txtBLNoStart.Name = "txtBLNoStart";
            this.txtBLNoStart.Size = new System.Drawing.Size(190, 23);
            this.txtBLNoStart.TabIndex = 102;
            // 
            // txtBLNoEnd
            // 
            this.txtBLNoEnd.BackColor = System.Drawing.Color.White;
            this.txtBLNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBLNoEnd.Location = new System.Drawing.Point(105, 114);
            this.txtBLNoEnd.Name = "txtBLNoEnd";
            this.txtBLNoEnd.Size = new System.Drawing.Size(190, 23);
            this.txtBLNoEnd.TabIndex = 103;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(105, 148);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(73, 24);
            this.comboM.TabIndex = 104;
            // 
            // txtSubconSupplier
            // 
            this.txtSubconSupplier.DisplayBox1Binding = "";
            this.txtSubconSupplier.IsIncludeJunk = true;
            this.txtSubconSupplier.Location = new System.Drawing.Point(105, 184);
            this.txtSubconSupplier.Name = "txtSubconSupplier";
            this.txtSubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtSubconSupplier.TabIndex = 105;
            this.txtSubconSupplier.TextBox1Binding = "";
            // 
            // comboOrderby
            // 
            this.comboOrderby.BackColor = System.Drawing.Color.White;
            this.comboOrderby.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboOrderby.FormattingEnabled = true;
            this.comboOrderby.IsSupportUnselect = true;
            this.comboOrderby.Location = new System.Drawing.Point(105, 221);
            this.comboOrderby.Name = "comboOrderby";
            this.comboOrderby.Size = new System.Drawing.Size(101, 24);
            this.comboOrderby.TabIndex = 106;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(302, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 23);
            this.label7.TabIndex = 107;
            this.label7.Text = "～";
            this.label7.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            this.label7.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // labelReportType
            // 
            this.labelReportType.Location = new System.Drawing.Point(13, 258);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(86, 23);
            this.labelReportType.TabIndex = 108;
            this.labelReportType.Text = "Report Type";
            // 
            // radioSummary
            // 
            this.radioSummary.AutoSize = true;
            this.radioSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSummary.Location = new System.Drawing.Point(83, 1);
            this.radioSummary.Name = "radioSummary";
            this.radioSummary.Size = new System.Drawing.Size(85, 21);
            this.radioSummary.TabIndex = 1;
            this.radioSummary.TabStop = true;
            this.radioSummary.Text = "Summary";
            this.radioSummary.UseVisualStyleBackColor = true;
            // 
            // radioDetail
            // 
            this.radioDetail.AutoSize = true;
            this.radioDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail.Location = new System.Drawing.Point(1, 1);
            this.radioDetail.Name = "radioDetail";
            this.radioDetail.Size = new System.Drawing.Size(62, 21);
            this.radioDetail.TabIndex = 0;
            this.radioDetail.TabStop = true;
            this.radioDetail.Text = "Detail";
            this.radioDetail.UseVisualStyleBackColor = true;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioSummary);
            this.radioPanel1.Controls.Add(this.radioDetail);
            this.radioPanel1.Location = new System.Drawing.Point(105, 257);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(219, 29);
            this.radioPanel1.TabIndex = 109;
            // 
            // R06
            // 
            this.ClientSize = new System.Drawing.Size(484, 311);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.labelReportType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboOrderby);
            this.Controls.Add(this.txtSubconSupplier);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.txtBLNoEnd);
            this.Controls.Add(this.txtBLNoStart);
            this.Controls.Add(this.dateApvDate);
            this.Controls.Add(this.dateDate);
            this.Controls.Add(this.labelOrderby);
            this.Controls.Add(this.labelSupplier);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelBLNo);
            this.Controls.Add(this.labelApvDate);
            this.Controls.Add(this.labelDate);
            this.IsSupportToPrint = false;
            this.Name = "R06";
            this.Text = "R06. Payment List - Shipping";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelDate, 0);
            this.Controls.SetChildIndex(this.labelApvDate, 0);
            this.Controls.SetChildIndex(this.labelBLNo, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelSupplier, 0);
            this.Controls.SetChildIndex(this.labelOrderby, 0);
            this.Controls.SetChildIndex(this.dateDate, 0);
            this.Controls.SetChildIndex(this.dateApvDate, 0);
            this.Controls.SetChildIndex(this.txtBLNoStart, 0);
            this.Controls.SetChildIndex(this.txtBLNoEnd, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.txtSubconSupplier, 0);
            this.Controls.SetChildIndex(this.comboOrderby, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.labelReportType, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelDate;
        private Win.UI.Label labelApvDate;
        private Win.UI.Label labelBLNo;
        private Win.UI.Label labelM;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelOrderby;
        private Win.UI.DateRange dateDate;
        private Win.UI.DateRange dateApvDate;
        private Win.UI.TextBox txtBLNoStart;
        private Win.UI.TextBox txtBLNoEnd;
        private Win.UI.ComboBox comboM;
        private Class.txtsubcon txtSubconSupplier;
        private Win.UI.ComboBox comboOrderby;
        private Win.UI.Label label7;
        private Win.UI.Label labelReportType;
        private Win.UI.RadioButton radioSummary;
        private Win.UI.RadioButton radioDetail;
        private Win.UI.RadioPanel radioPanel1;
    }
}
