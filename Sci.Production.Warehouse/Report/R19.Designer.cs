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
            this.labBorrowDate = new Sci.Win.UI.Label();
            this.dateRangeBorrowDate = new Sci.Win.UI.DateRange();
            this.labBuyerDlv = new Sci.Win.UI.Label();
            this.dateRangeBuyerDlv = new Sci.Win.UI.DateRange();
            this.labFactory = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtSeq = new Sci.Production.Class.TxtSeq();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(484, 5);
            this.print.TabIndex = 70;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(484, 41);
            this.toexcel.TabIndex = 80;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(484, 77);
            this.close.TabIndex = 90;
            // 
            // txtBorrowSPNo
            // 
            this.txtBorrowSPNo.BackColor = System.Drawing.Color.White;
            this.txtBorrowSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBorrowSPNo.Location = new System.Drawing.Point(127, 132);
            this.txtBorrowSPNo.MaxLength = 13;
            this.txtBorrowSPNo.Name = "txtBorrowSPNo";
            this.txtBorrowSPNo.Size = new System.Drawing.Size(118, 23);
            this.txtBorrowSPNo.TabIndex = 40;
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
            this.comboCategoryAlreadyReturn.Location = new System.Drawing.Point(127, 190);
            this.comboCategoryAlreadyReturn.Name = "comboCategoryAlreadyReturn";
            this.comboCategoryAlreadyReturn.OldText = "";
            this.comboCategoryAlreadyReturn.Size = new System.Drawing.Size(121, 24);
            this.comboCategoryAlreadyReturn.TabIndex = 60;
            // 
            // labelAlreadyReturn
            // 
            this.labelAlreadyReturn.Location = new System.Drawing.Point(10, 190);
            this.labelAlreadyReturn.Name = "labelAlreadyReturn";
            this.labelAlreadyReturn.Size = new System.Drawing.Size(114, 23);
            this.labelAlreadyReturn.TabIndex = 107;
            this.labelAlreadyReturn.Text = "Already Return";
            // 
            // labelBorrowSeqNo
            // 
            this.labelBorrowSeqNo.Location = new System.Drawing.Point(10, 161);
            this.labelBorrowSeqNo.Name = "labelBorrowSeqNo";
            this.labelBorrowSeqNo.Size = new System.Drawing.Size(114, 23);
            this.labelBorrowSeqNo.TabIndex = 114;
            this.labelBorrowSeqNo.Text = "Borrow Seq#";
            // 
            // dateEstReturnDate
            // 
            // 
            // 
            // 
            this.dateEstReturnDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEstReturnDate.DateBox1.Name = "";
            this.dateEstReturnDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateEstReturnDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEstReturnDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateEstReturnDate.DateBox2.Name = "";
            this.dateEstReturnDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateEstReturnDate.DateBox2.TabIndex = 1;
            this.dateEstReturnDate.IsRequired = false;
            this.dateEstReturnDate.Location = new System.Drawing.Point(127, 12);
            this.dateEstReturnDate.Name = "dateEstReturnDate";
            this.dateEstReturnDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstReturnDate.TabIndex = 0;
            // 
            // labelBorrowSPNo
            // 
            this.labelBorrowSPNo.Location = new System.Drawing.Point(10, 132);
            this.labelBorrowSPNo.Name = "labelBorrowSPNo";
            this.labelBorrowSPNo.Size = new System.Drawing.Size(114, 23);
            this.labelBorrowSPNo.TabIndex = 116;
            this.labelBorrowSPNo.Text = "Borrow SP#";
            // 
            // labelEstReturnDate
            // 
            this.labelEstReturnDate.Location = new System.Drawing.Point(10, 12);
            this.labelEstReturnDate.Name = "labelEstReturnDate";
            this.labelEstReturnDate.Size = new System.Drawing.Size(114, 23);
            this.labelEstReturnDate.TabIndex = 117;
            this.labelEstReturnDate.Text = "Est. Return Date";
            // 
            // labBorrowDate
            // 
            this.labBorrowDate.Location = new System.Drawing.Point(10, 42);
            this.labBorrowDate.Name = "labBorrowDate";
            this.labBorrowDate.Size = new System.Drawing.Size(114, 23);
            this.labBorrowDate.TabIndex = 119;
            this.labBorrowDate.Text = "Borrow Date";
            // 
            // dateRangeBorrowDate
            // 
            // 
            // 
            // 
            this.dateRangeBorrowDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeBorrowDate.DateBox1.Name = "";
            this.dateRangeBorrowDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeBorrowDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeBorrowDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeBorrowDate.DateBox2.Name = "";
            this.dateRangeBorrowDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeBorrowDate.DateBox2.TabIndex = 1;
            this.dateRangeBorrowDate.IsRequired = false;
            this.dateRangeBorrowDate.Location = new System.Drawing.Point(127, 42);
            this.dateRangeBorrowDate.Name = "dateRangeBorrowDate";
            this.dateRangeBorrowDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeBorrowDate.TabIndex = 10;
            // 
            // labBuyerDlv
            // 
            this.labBuyerDlv.Location = new System.Drawing.Point(10, 72);
            this.labBuyerDlv.Name = "labBuyerDlv";
            this.labBuyerDlv.Size = new System.Drawing.Size(114, 23);
            this.labBuyerDlv.TabIndex = 121;
            this.labBuyerDlv.Text = "Buyer Dlv.";
            // 
            // dateRangeBuyerDlv
            // 
            // 
            // 
            // 
            this.dateRangeBuyerDlv.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeBuyerDlv.DateBox1.Name = "";
            this.dateRangeBuyerDlv.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeBuyerDlv.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeBuyerDlv.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeBuyerDlv.DateBox2.Name = "";
            this.dateRangeBuyerDlv.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeBuyerDlv.DateBox2.TabIndex = 1;
            this.dateRangeBuyerDlv.IsRequired = false;
            this.dateRangeBuyerDlv.Location = new System.Drawing.Point(127, 72);
            this.dateRangeBuyerDlv.Name = "dateRangeBuyerDlv";
            this.dateRangeBuyerDlv.Size = new System.Drawing.Size(280, 23);
            this.dateRangeBuyerDlv.TabIndex = 20;
            // 
            // labFactory
            // 
            this.labFactory.Location = new System.Drawing.Point(10, 102);
            this.labFactory.Name = "labFactory";
            this.labFactory.Size = new System.Drawing.Size(114, 23);
            this.labFactory.TabIndex = 122;
            this.labFactory.Text = "Factory";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(127, 102);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 30;
            // 
            // txtSeq
            // 
            this.txtSeq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq.Location = new System.Drawing.Point(127, 161);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Seq1 = "";
            this.txtSeq.Seq2 = "";
            this.txtSeq.Size = new System.Drawing.Size(61, 23);
            this.txtSeq.TabIndex = 50;
            // 
            // R19
            // 
            this.ClientSize = new System.Drawing.Size(576, 255);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labFactory);
            this.Controls.Add(this.labBuyerDlv);
            this.Controls.Add(this.dateRangeBuyerDlv);
            this.Controls.Add(this.labBorrowDate);
            this.Controls.Add(this.dateRangeBorrowDate);
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
            this.Controls.SetChildIndex(this.dateRangeBorrowDate, 0);
            this.Controls.SetChildIndex(this.labBorrowDate, 0);
            this.Controls.SetChildIndex(this.dateRangeBuyerDlv, 0);
            this.Controls.SetChildIndex(this.labBuyerDlv, 0);
            this.Controls.SetChildIndex(this.labFactory, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
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
        private Class.TxtSeq txtSeq;
        private Win.UI.Label labBorrowDate;
        private Win.UI.DateRange dateRangeBorrowDate;
        private Win.UI.Label labBuyerDlv;
        private Win.UI.DateRange dateRangeBuyerDlv;
        private Win.UI.Label labFactory;
        private Class.Txtfactory txtfactory;
    }
}
