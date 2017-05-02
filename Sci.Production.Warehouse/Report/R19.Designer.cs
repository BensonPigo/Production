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
            this.txtBorrowSPNo = new Sci.Win.UI.TextBox();
            this.comboCategoryAlreadyReturn = new Sci.Win.UI.ComboBox();
            this.labelAlreadyReturn = new Sci.Win.UI.Label();
            this.labelBorrowSeqNo = new Sci.Win.UI.Label();
            this.dateEstReturnDate = new Sci.Win.UI.DateRange();
            this.labelBorrowSPNo = new Sci.Win.UI.Label();
            this.labelEstReturnDate = new Sci.Win.UI.Label();
            this.txtSeq = new Sci.Production.Class.txtSeq();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(484, 5);
            this.print.TabIndex = 4;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(484, 41);
            this.toexcel.TabIndex = 5;
            this.toexcel.Click += new System.EventHandler(this.toexcel_Click);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(484, 77);
            this.close.TabIndex = 6;
            // 
            // txtBorrowSPNo
            // 
            this.txtBorrowSPNo.BackColor = System.Drawing.Color.White;
            this.txtBorrowSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBorrowSPNo.Location = new System.Drawing.Point(127, 48);
            this.txtBorrowSPNo.MaxLength = 13;
            this.txtBorrowSPNo.Name = "txtBorrowSPNo";
            this.txtBorrowSPNo.Size = new System.Drawing.Size(118, 23);
            this.txtBorrowSPNo.TabIndex = 1;
            // 
            // comboCategoryAlreadyReturn
            // 
            this.comboCategoryAlreadyReturn.BackColor = System.Drawing.Color.White;
            this.comboCategoryAlreadyReturn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategoryAlreadyReturn.FormattingEnabled = true;
            this.comboCategoryAlreadyReturn.IsSupportUnselect = true;
            this.comboCategoryAlreadyReturn.Items.AddRange(new object[] {
            "Not Yet",
            "Yet",
            "All"});
            this.comboCategoryAlreadyReturn.Location = new System.Drawing.Point(127, 121);
            this.comboCategoryAlreadyReturn.Name = "comboCategoryAlreadyReturn";
            this.comboCategoryAlreadyReturn.Size = new System.Drawing.Size(121, 24);
            this.comboCategoryAlreadyReturn.TabIndex = 3;
            // 
            // labelAlreadyReturn
            // 
            this.labelAlreadyReturn.Lines = 0;
            this.labelAlreadyReturn.Location = new System.Drawing.Point(10, 121);
            this.labelAlreadyReturn.Name = "labelAlreadyReturn";
            this.labelAlreadyReturn.Size = new System.Drawing.Size(114, 23);
            this.labelAlreadyReturn.TabIndex = 107;
            this.labelAlreadyReturn.Text = "Already Return";
            // 
            // labelBorrowSeqNo
            // 
            this.labelBorrowSeqNo.Lines = 0;
            this.labelBorrowSeqNo.Location = new System.Drawing.Point(10, 84);
            this.labelBorrowSeqNo.Name = "labelBorrowSeqNo";
            this.labelBorrowSeqNo.Size = new System.Drawing.Size(114, 23);
            this.labelBorrowSeqNo.TabIndex = 114;
            this.labelBorrowSeqNo.Text = "Borrow Seq#";
            // 
            // dateEstReturnDate
            // 
            this.dateEstReturnDate.IsRequired = false;
            this.dateEstReturnDate.Location = new System.Drawing.Point(127, 12);
            this.dateEstReturnDate.Name = "dateEstReturnDate";
            this.dateEstReturnDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstReturnDate.TabIndex = 0;
            // 
            // labelBorrowSPNo
            // 
            this.labelBorrowSPNo.Lines = 0;
            this.labelBorrowSPNo.Location = new System.Drawing.Point(10, 48);
            this.labelBorrowSPNo.Name = "labelBorrowSPNo";
            this.labelBorrowSPNo.Size = new System.Drawing.Size(114, 23);
            this.labelBorrowSPNo.TabIndex = 116;
            this.labelBorrowSPNo.Text = "Borrow SP#";
            // 
            // labelEstReturnDate
            // 
            this.labelEstReturnDate.Lines = 0;
            this.labelEstReturnDate.Location = new System.Drawing.Point(10, 12);
            this.labelEstReturnDate.Name = "labelEstReturnDate";
            this.labelEstReturnDate.Size = new System.Drawing.Size(114, 23);
            this.labelEstReturnDate.TabIndex = 117;
            this.labelEstReturnDate.Text = "Est. Return Date";
            // 
            // txtSeq
            // 
            this.txtSeq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq.Location = new System.Drawing.Point(127, 84);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.seq1 = "";
            this.txtSeq.seq2 = "";
            this.txtSeq.Size = new System.Drawing.Size(61, 23);
            this.txtSeq.TabIndex = 2;
            // 
            // R19
            // 
            this.ClientSize = new System.Drawing.Size(576, 240);
            this.Controls.Add(this.txtSeq);
            this.Controls.Add(this.labelEstReturnDate);
            this.Controls.Add(this.labelBorrowSPNo);
            this.Controls.Add(this.dateEstReturnDate);
            this.Controls.Add(this.labelBorrowSeqNo);
            this.Controls.Add(this.comboCategoryAlreadyReturn);
            this.Controls.Add(this.labelAlreadyReturn);
            this.Controls.Add(this.txtBorrowSPNo);
            this.Name = "R19";
            this.Text = "R19. Material Borrowing Query";
            this.Load += new System.EventHandler(this.R18_Load);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.txtBorrowSPNo, 0);
            this.Controls.SetChildIndex(this.labelAlreadyReturn, 0);
            this.Controls.SetChildIndex(this.comboCategoryAlreadyReturn, 0);
            this.Controls.SetChildIndex(this.labelBorrowSeqNo, 0);
            this.Controls.SetChildIndex(this.dateEstReturnDate, 0);
            this.Controls.SetChildIndex(this.labelBorrowSPNo, 0);
            this.Controls.SetChildIndex(this.labelEstReturnDate, 0);
            this.Controls.SetChildIndex(this.txtSeq, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox txtBorrowSPNo;
        private Win.UI.ComboBox comboCategoryAlreadyReturn;
        private Win.UI.Label labelAlreadyReturn;
        private Win.UI.Label labelBorrowSeqNo;
        private Win.UI.DateRange dateEstReturnDate;
        private Win.UI.Label labelBorrowSPNo;
        private Win.UI.Label labelEstReturnDate;
        private Class.txtSeq txtSeq;
    }
}
