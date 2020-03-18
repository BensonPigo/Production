namespace Sci.Production.Shipping
{
    partial class B03_Quotation
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
            this.numPrice1 = new Sci.Win.UI.NumericBox();
            this.labelPrice1 = new Sci.Win.UI.Label();
            this.displayCode = new Sci.Win.UI.DisplayBox();
            this.labelCode = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txtCurrency1 = new Sci.Production.Class.txtcurrency();
            this.txtsubconSupplier1 = new Sci.Production.Class.txtsubconNoConfirm();
            this.label2 = new Sci.Win.UI.Label();
            this.dateQuotation1 = new Sci.Win.UI.DateBox();
            this.label6 = new Sci.Win.UI.Label();
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
            this.detail.Location = new System.Drawing.Point(4, 29);
            this.detail.Size = new System.Drawing.Size(826, 391);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.numPrice1);
            this.detailcont.Controls.Add(this.labelPrice1);
            this.detailcont.Controls.Add(this.dateQuotation1);
            this.detailcont.Controls.Add(this.displayCode);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Controls.Add(this.txtCurrency1);
            this.detailcont.Controls.Add(this.txtsubconSupplier1);
            this.detailcont.Size = new System.Drawing.Size(826, 353);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 353);
            this.detailbtm.Size = new System.Drawing.Size(826, 38);
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(4, 29);
            this.browse.Size = new System.Drawing.Size(826, 391);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(834, 424);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 26);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 26);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // numPrice1
            // 
            this.numPrice1.BackColor = System.Drawing.Color.White;
            this.numPrice1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "price1", true));
            this.numPrice1.DecimalPlaces = 5;
            this.numPrice1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numPrice1.Location = new System.Drawing.Point(179, 89);
            this.numPrice1.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            262144});
            this.numPrice1.MaxLength = 13;
            this.numPrice1.Minimum = new decimal(new int[] {
            99999999,
            0,
            0,
            -2147483648});
            this.numPrice1.Name = "numPrice1";
            this.numPrice1.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPrice1.Size = new System.Drawing.Size(118, 26);
            this.numPrice1.TabIndex = 37;
            this.numPrice1.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelPrice1
            // 
            this.labelPrice1.Location = new System.Drawing.Point(32, 89);
            this.labelPrice1.Name = "labelPrice1";
            this.labelPrice1.Size = new System.Drawing.Size(102, 23);
            this.labelPrice1.TabIndex = 34;
            this.labelPrice1.Text = "Price";
            // 
            // displayCode
            // 
            this.displayCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCode.Location = new System.Drawing.Point(137, 18);
            this.displayCode.Name = "displayCode";
            this.displayCode.Size = new System.Drawing.Size(140, 26);
            this.displayCode.TabIndex = 31;
            // 
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(32, 18);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(102, 23);
            this.labelCode.TabIndex = 30;
            this.labelCode.Text = "Code";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label1.Location = new System.Drawing.Point(375, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 23);
            this.label1.TabIndex = 42;
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.label1.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // txtCurrency1
            // 
            this.txtCurrency1.BackColor = System.Drawing.Color.White;
            this.txtCurrency1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CurrencyID1", true));
            this.txtCurrency1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCurrency1.IsSupportSytsemContextMenu = false;
            this.txtCurrency1.Location = new System.Drawing.Point(137, 89);
            this.txtCurrency1.Name = "txtCurrency1";
            this.txtCurrency1.Size = new System.Drawing.Size(40, 26);
            this.txtCurrency1.TabIndex = 26;
            // 
            // txtsubconSupplier1
            // 
            this.txtsubconSupplier1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LocalSuppID1", true));
            this.txtsubconSupplier1.DisplayBox1Binding = "";
            this.txtsubconSupplier1.IsIncludeJunk = false;
            this.txtsubconSupplier1.isMisc = false;
            this.txtsubconSupplier1.isShipping = false;
            this.txtsubconSupplier1.isSubcon = false;
            this.txtsubconSupplier1.Location = new System.Drawing.Point(137, 52);
            this.txtsubconSupplier1.Name = "txtsubconSupplier1";
            this.txtsubconSupplier1.Size = new System.Drawing.Size(159, 23);
            this.txtsubconSupplier1.TabIndex = 22;
            this.txtsubconSupplier1.TextBox1Binding = "";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(32, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 23);
            this.label2.TabIndex = 52;
            this.label2.Text = "Quotation Date";
            // 
            // dateQuotation1
            // 
            this.dateQuotation1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "QuotDate1", true));
            this.dateQuotation1.Location = new System.Drawing.Point(137, 126);
            this.dateQuotation1.Name = "dateQuotation1";
            this.dateQuotation1.Size = new System.Drawing.Size(130, 26);
            this.dateQuotation1.TabIndex = 48;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(32, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 23);
            this.label6.TabIndex = 56;
            this.label6.Text = "Supplier";
            // 
            // B03_Quotation
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(834, 457);
            this.DefaultOrder = "ID,AddDate";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.Name = "B03_Quotation";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "Quotation Record";
            this.WorkAlias = "ShipExpense_CanVass";
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
        private Win.UI.NumericBox numPrice1;
        private Win.UI.Label labelPrice1;
        private Win.UI.DisplayBox displayCode;
        private Win.UI.Label labelCode;
        private Class.txtcurrency txtCurrency1;
        private Class.txtsubconNoConfirm txtsubconSupplier1;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.DateBox dateQuotation1;
        private Win.UI.Label label6;
    }
}
