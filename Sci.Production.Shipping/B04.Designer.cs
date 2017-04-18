namespace Sci.Production.Shipping
{
    partial class B04
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
            this.labelAccountNo = new Sci.Win.UI.Label();
            this.labelAccountName = new Sci.Win.UI.Label();
            this.labelExpenseReason = new Sci.Win.UI.Label();
            this.labelShippingMode = new Sci.Win.UI.Label();
            this.labelSharebase = new Sci.Win.UI.Label();
            this.displayAccountName = new Sci.Win.UI.DisplayBox();
            this.displayExpenseReason = new Sci.Win.UI.DisplayBox();
            this.displayShippingMode = new Sci.Win.UI.DisplayBox();
            this.comboSharebase = new Sci.Win.UI.ComboBox();
            this.txtAccountNo = new Sci.Win.UI.TextBox();
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
            this.detail.Size = new System.Drawing.Size(823, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtAccountNo);
            this.detailcont.Controls.Add(this.comboSharebase);
            this.detailcont.Controls.Add(this.displayShippingMode);
            this.detailcont.Controls.Add(this.displayExpenseReason);
            this.detailcont.Controls.Add(this.displayAccountName);
            this.detailcont.Controls.Add(this.labelSharebase);
            this.detailcont.Controls.Add(this.labelShippingMode);
            this.detailcont.Controls.Add(this.labelExpenseReason);
            this.detailcont.Controls.Add(this.labelAccountName);
            this.detailcont.Controls.Add(this.labelAccountNo);
            this.detailcont.Size = new System.Drawing.Size(823, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(823, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(823, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(831, 424);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelAccountNo
            // 
            this.labelAccountNo.Lines = 0;
            this.labelAccountNo.Location = new System.Drawing.Point(33, 30);
            this.labelAccountNo.Name = "labelAccountNo";
            this.labelAccountNo.Size = new System.Drawing.Size(115, 23);
            this.labelAccountNo.TabIndex = 0;
            this.labelAccountNo.Text = "Account No.";
            // 
            // labelAccountName
            // 
            this.labelAccountName.Lines = 0;
            this.labelAccountName.Location = new System.Drawing.Point(33, 70);
            this.labelAccountName.Name = "labelAccountName";
            this.labelAccountName.Size = new System.Drawing.Size(115, 23);
            this.labelAccountName.TabIndex = 1;
            this.labelAccountName.Text = "Account Name";
            // 
            // labelExpenseReason
            // 
            this.labelExpenseReason.Lines = 0;
            this.labelExpenseReason.Location = new System.Drawing.Point(33, 110);
            this.labelExpenseReason.Name = "labelExpenseReason";
            this.labelExpenseReason.Size = new System.Drawing.Size(115, 23);
            this.labelExpenseReason.TabIndex = 2;
            this.labelExpenseReason.Text = "Expense Reason";
            // 
            // labelShippingMode
            // 
            this.labelShippingMode.Lines = 0;
            this.labelShippingMode.Location = new System.Drawing.Point(33, 150);
            this.labelShippingMode.Name = "labelShippingMode";
            this.labelShippingMode.Size = new System.Drawing.Size(115, 23);
            this.labelShippingMode.TabIndex = 3;
            this.labelShippingMode.Text = "Shipping Mode";
            // 
            // labelSharebase
            // 
            this.labelSharebase.Lines = 0;
            this.labelSharebase.Location = new System.Drawing.Point(33, 190);
            this.labelSharebase.Name = "labelSharebase";
            this.labelSharebase.Size = new System.Drawing.Size(115, 23);
            this.labelSharebase.TabIndex = 4;
            this.labelSharebase.Text = "Share base";
            // 
            // displayAccountName
            // 
            this.displayAccountName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayAccountName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayAccountName.Location = new System.Drawing.Point(151, 70);
            this.displayAccountName.Name = "displayAccountName";
            this.displayAccountName.Size = new System.Drawing.Size(200, 23);
            this.displayAccountName.TabIndex = 6;
            // 
            // displayExpenseReason
            // 
            this.displayExpenseReason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayExpenseReason.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ExpenseReason", true));
            this.displayExpenseReason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayExpenseReason.Location = new System.Drawing.Point(151, 110);
            this.displayExpenseReason.Name = "displayExpenseReason";
            this.displayExpenseReason.Size = new System.Drawing.Size(200, 23);
            this.displayExpenseReason.TabIndex = 7;
            // 
            // displayShippingMode
            // 
            this.displayShippingMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayShippingMode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ShipModeID", true));
            this.displayShippingMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayShippingMode.Location = new System.Drawing.Point(151, 150);
            this.displayShippingMode.Name = "displayShippingMode";
            this.displayShippingMode.Size = new System.Drawing.Size(455, 23);
            this.displayShippingMode.TabIndex = 8;
            // 
            // comboSharebase
            // 
            this.comboSharebase.BackColor = System.Drawing.Color.White;
            this.comboSharebase.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "ShareBase", true));
            this.comboSharebase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSharebase.FormattingEnabled = true;
            this.comboSharebase.IsSupportUnselect = true;
            this.comboSharebase.Location = new System.Drawing.Point(151, 190);
            this.comboSharebase.Name = "comboSharebase";
            this.comboSharebase.Size = new System.Drawing.Size(178, 24);
            this.comboSharebase.TabIndex = 9;
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.BackColor = System.Drawing.Color.White;
            this.txtAccountNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "AccountID", true));
            this.txtAccountNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtAccountNo.Location = new System.Drawing.Point(151, 30);
            this.txtAccountNo.Mask = "CCCC-CCCC";
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.Size = new System.Drawing.Size(70, 23);
            this.txtAccountNo.TabIndex = 10;
            // 
            // B04
            // 
            this.ClientSize = new System.Drawing.Size(831, 457);
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B04";
            this.Text = "B04. Share base of Share Expense";
            this.WorkAlias = "ShareRule";
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

        private Win.UI.ComboBox comboSharebase;
        private Win.UI.DisplayBox displayShippingMode;
        private Win.UI.DisplayBox displayExpenseReason;
        private Win.UI.DisplayBox displayAccountName;
        private Win.UI.Label labelSharebase;
        private Win.UI.Label labelShippingMode;
        private Win.UI.Label labelExpenseReason;
        private Win.UI.Label labelAccountName;
        private Win.UI.Label labelAccountNo;
        private Win.UI.TextBox txtAccountNo;
    }
}
