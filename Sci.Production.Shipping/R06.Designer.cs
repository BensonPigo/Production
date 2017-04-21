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
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(380, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(380, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(380, 84);
            // 
            // labelDate
            // 
            this.labelDate.Lines = 0;
            this.labelDate.Location = new System.Drawing.Point(13, 12);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(65, 23);
            this.labelDate.TabIndex = 94;
            this.labelDate.Text = "Date";
            // 
            // labelApvDate
            // 
            this.labelApvDate.Lines = 0;
            this.labelApvDate.Location = new System.Drawing.Point(13, 48);
            this.labelApvDate.Name = "labelApvDate";
            this.labelApvDate.Size = new System.Drawing.Size(65, 23);
            this.labelApvDate.TabIndex = 95;
            this.labelApvDate.Text = "Apv. Date";
            // 
            // labelBLNo
            // 
            this.labelBLNo.Lines = 0;
            this.labelBLNo.Location = new System.Drawing.Point(13, 84);
            this.labelBLNo.Name = "labelBLNo";
            this.labelBLNo.Size = new System.Drawing.Size(65, 23);
            this.labelBLNo.TabIndex = 96;
            this.labelBLNo.Text = "BL No.";
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(13, 148);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(65, 23);
            this.labelM.TabIndex = 97;
            this.labelM.Text = "M";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Lines = 0;
            this.labelSupplier.Location = new System.Drawing.Point(13, 184);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(65, 23);
            this.labelSupplier.TabIndex = 98;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelOrderby
            // 
            this.labelOrderby.Lines = 0;
            this.labelOrderby.Location = new System.Drawing.Point(13, 221);
            this.labelOrderby.Name = "labelOrderby";
            this.labelOrderby.Size = new System.Drawing.Size(65, 23);
            this.labelOrderby.TabIndex = 99;
            this.labelOrderby.Text = "Order by";
            // 
            // dateDate
            // 
            this.dateDate.IsRequired = false;
            this.dateDate.Location = new System.Drawing.Point(82, 12);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(280, 23);
            this.dateDate.TabIndex = 100;
            // 
            // dateApvDate
            // 
            this.dateApvDate.IsRequired = false;
            this.dateApvDate.Location = new System.Drawing.Point(82, 48);
            this.dateApvDate.Name = "dateApvDate";
            this.dateApvDate.Size = new System.Drawing.Size(280, 23);
            this.dateApvDate.TabIndex = 101;
            // 
            // txtBLNoStart
            // 
            this.txtBLNoStart.BackColor = System.Drawing.Color.White;
            this.txtBLNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBLNoStart.Location = new System.Drawing.Point(82, 84);
            this.txtBLNoStart.Name = "txtBLNoStart";
            this.txtBLNoStart.Size = new System.Drawing.Size(190, 23);
            this.txtBLNoStart.TabIndex = 102;
            // 
            // txtBLNoEnd
            // 
            this.txtBLNoEnd.BackColor = System.Drawing.Color.White;
            this.txtBLNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBLNoEnd.Location = new System.Drawing.Point(82, 114);
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
            this.comboM.Location = new System.Drawing.Point(82, 148);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(73, 24);
            this.comboM.TabIndex = 104;
            // 
            // txtSubconSupplier
            // 
            this.txtSubconSupplier.DisplayBox1Binding = "";
            this.txtSubconSupplier.IsIncludeJunk = true;
            this.txtSubconSupplier.Location = new System.Drawing.Point(82, 184);
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
            this.comboOrderby.Location = new System.Drawing.Point(82, 221);
            this.comboOrderby.Name = "comboOrderby";
            this.comboOrderby.Size = new System.Drawing.Size(101, 24);
            this.comboOrderby.TabIndex = 106;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(279, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 23);
            this.label7.TabIndex = 107;
            this.label7.Text = "～";
            this.label7.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            this.label7.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // R06
            // 
            this.ClientSize = new System.Drawing.Size(472, 281);
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
    }
}
