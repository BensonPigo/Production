namespace Sci.Production.Warehouse
{
    partial class R19
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
            this.tbxSP = new Sci.Win.UI.TextBox();
            this.cbxCategory = new Sci.Win.UI.ComboBox();
            this.label7 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.dateRange_ReturnDate = new Sci.Win.UI.DateRange();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtSeq1 = new Sci.Production.Class.txtSeq();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(484, 5);
            this.print.TabIndex = 7;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(484, 41);
            this.toexcel.TabIndex = 8;
            this.toexcel.Click += new System.EventHandler(this.toexcel_Click);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(484, 77);
            this.close.TabIndex = 9;
            // 
            // tbxSP
            // 
            this.tbxSP.BackColor = System.Drawing.Color.White;
            this.tbxSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tbxSP.Location = new System.Drawing.Point(127, 48);
            this.tbxSP.MaxLength = 13;
            this.tbxSP.Name = "tbxSP";
            this.tbxSP.Size = new System.Drawing.Size(118, 23);
            this.tbxSP.TabIndex = 1;
            // 
            // cbxCategory
            // 
            this.cbxCategory.BackColor = System.Drawing.Color.White;
            this.cbxCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbxCategory.FormattingEnabled = true;
            this.cbxCategory.IsSupportUnselect = true;
            this.cbxCategory.Items.AddRange(new object[] {
            "Not Yet",
            "Yet",
            "All"});
            this.cbxCategory.Location = new System.Drawing.Point(127, 121);
            this.cbxCategory.Name = "cbxCategory";
            this.cbxCategory.Size = new System.Drawing.Size(121, 24);
            this.cbxCategory.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(10, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 23);
            this.label7.TabIndex = 107;
            this.label7.Text = "Already Return";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(10, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 23);
            this.label1.TabIndex = 114;
            this.label1.Text = "Borrow Seq#";
            // 
            // dateRange_ReturnDate
            // 
            this.dateRange_ReturnDate.Location = new System.Drawing.Point(127, 12);
            this.dateRange_ReturnDate.Name = "dateRange_ReturnDate";
            this.dateRange_ReturnDate.Size = new System.Drawing.Size(280, 23);
            this.dateRange_ReturnDate.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(10, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 23);
            this.label2.TabIndex = 116;
            this.label2.Text = "Borrow SP#";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(10, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 23);
            this.label3.TabIndex = 117;
            this.label3.Text = "Est. Return Date";
            // 
            // txtSeq1
            // 
            this.txtSeq1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq1.Location = new System.Drawing.Point(127, 84);
            this.txtSeq1.Name = "txtSeq1";
            this.txtSeq1.seq1 = "";
            this.txtSeq1.seq2 = "";
            this.txtSeq1.Size = new System.Drawing.Size(61, 23);
            this.txtSeq1.TabIndex = 118;
            // 
            // R19
            // 
            this.ClientSize = new System.Drawing.Size(576, 240);
            this.Controls.Add(this.txtSeq1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateRange_ReturnDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbxCategory);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbxSP);
            this.Name = "R19";
            this.Text = "R19. Material Borrowing Query";
            this.Load += new System.EventHandler(this.R18_Load);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.tbxSP, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.cbxCategory, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.dateRange_ReturnDate, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtSeq1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox tbxSP;
        private Win.UI.ComboBox cbxCategory;
        private Win.UI.Label label7;
        private Win.UI.Label label1;
        private Win.UI.DateRange dateRange_ReturnDate;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Class.txtSeq txtSeq1;
    }
}
