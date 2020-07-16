namespace Sci.Production.Shipping
{
    partial class B03
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
            this.labelCode = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelPrice = new Sci.Win.UI.Label();
            this.labelCanvassDate = new Sci.Win.UI.Label();
            this.labelAccountNo = new Sci.Win.UI.Label();
            this.txtCode = new Sci.Win.UI.TextBox();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.displayPrice = new Sci.Win.UI.DisplayBox();
            this.numPrice = new Sci.Win.UI.NumericBox();
            this.dateCanvassDate = new Sci.Win.UI.DateBox();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.btnQuotationRecord = new Sci.Win.UI.Button();
            this.btnPaymentHistory = new Sci.Win.UI.Button();
            this.labelUnit = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.btnBatchApprove = new Sci.Win.UI.Button();
            this.txtUnit = new Sci.Production.Class.Txtunit_local();
            this.txtsubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtAccountNo = new Sci.Production.Class.TxtAccountNo();
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
            this.detail.Size = new System.Drawing.Size(882, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtAccountNo);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.labelUnit);
            this.detailcont.Controls.Add(this.btnPaymentHistory);
            this.detailcont.Controls.Add(this.btnQuotationRecord);
            this.detailcont.Controls.Add(this.checkBox1);
            this.detailcont.Controls.Add(this.dateCanvassDate);
            this.detailcont.Controls.Add(this.numPrice);
            this.detailcont.Controls.Add(this.displayPrice);
            this.detailcont.Controls.Add(this.editDescription);
            this.detailcont.Controls.Add(this.txtCode);
            this.detailcont.Controls.Add(this.labelAccountNo);
            this.detailcont.Controls.Add(this.labelCanvassDate);
            this.detailcont.Controls.Add(this.labelPrice);
            this.detailcont.Controls.Add(this.labelSupplier);
            this.detailcont.Controls.Add(this.labelBrand);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Controls.Add(this.txtUnit);
            this.detailcont.Controls.Add(this.txtsubconSupplier);
            this.detailcont.Controls.Add(this.txtbrand);
            this.detailcont.Size = new System.Drawing.Size(882, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(882, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(882, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(890, 424);
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
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(27, 22);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(90, 23);
            this.labelCode.TabIndex = 4;
            this.labelCode.Text = "Code";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(27, 57);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(90, 23);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "Description";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(27, 120);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(90, 23);
            this.labelBrand.TabIndex = 6;
            this.labelBrand.Text = "Brand";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(27, 187);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(90, 23);
            this.labelSupplier.TabIndex = 7;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelPrice
            // 
            this.labelPrice.Location = new System.Drawing.Point(27, 222);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.Size = new System.Drawing.Size(90, 23);
            this.labelPrice.TabIndex = 8;
            this.labelPrice.Text = "Price";
            // 
            // labelCanvassDate
            // 
            this.labelCanvassDate.Location = new System.Drawing.Point(27, 257);
            this.labelCanvassDate.Name = "labelCanvassDate";
            this.labelCanvassDate.Size = new System.Drawing.Size(90, 23);
            this.labelCanvassDate.TabIndex = 9;
            this.labelCanvassDate.Text = "Quot. Date";
            // 
            // labelAccountNo
            // 
            this.labelAccountNo.Location = new System.Drawing.Point(27, 292);
            this.labelAccountNo.Name = "labelAccountNo";
            this.labelAccountNo.Size = new System.Drawing.Size(90, 23);
            this.labelAccountNo.TabIndex = 10;
            this.labelAccountNo.Text = "Account No";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCode.Location = new System.Drawing.Point(120, 22);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(140, 23);
            this.txtCode.TabIndex = 7;
            this.txtCode.Validating += new System.ComponentModel.CancelEventHandler(this.TxtCode_Validating);
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(120, 57);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(391, 50);
            this.editDescription.TabIndex = 0;
            // 
            // displayPrice
            // 
            this.displayPrice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPrice.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CurrencyID", true));
            this.displayPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPrice.Location = new System.Drawing.Point(120, 222);
            this.displayPrice.Name = "displayPrice";
            this.displayPrice.Size = new System.Drawing.Size(40, 23);
            this.displayPrice.TabIndex = 11;
            // 
            // numPrice
            // 
            this.numPrice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numPrice.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Price", true));
            this.numPrice.DecimalPlaces = 5;
            this.numPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numPrice.IsSupportEditMode = false;
            this.numPrice.Location = new System.Drawing.Point(161, 222);
            this.numPrice.Name = "numPrice";
            this.numPrice.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPrice.ReadOnly = true;
            this.numPrice.Size = new System.Drawing.Size(118, 23);
            this.numPrice.TabIndex = 12;
            this.numPrice.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // dateCanvassDate
            // 
            this.dateCanvassDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CanvassDate", true));
            this.dateCanvassDate.IsSupportEditMode = false;
            this.dateCanvassDate.Location = new System.Drawing.Point(120, 257);
            this.dateCanvassDate.Name = "dateCanvassDate";
            this.dateCanvassDate.ReadOnly = true;
            this.dateCanvassDate.Size = new System.Drawing.Size(130, 23);
            this.dateCanvassDate.TabIndex = 13;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkBox1.IsSupportEditMode = false;
            this.checkBox1.Location = new System.Drawing.Point(316, 22);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.ReadOnly = true;
            this.checkBox1.Size = new System.Drawing.Size(57, 21);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Junk";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // btnQuotationRecord
            // 
            this.btnQuotationRecord.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnQuotationRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnQuotationRecord.Location = new System.Drawing.Point(683, 57);
            this.btnQuotationRecord.Name = "btnQuotationRecord";
            this.btnQuotationRecord.Size = new System.Drawing.Size(145, 30);
            this.btnQuotationRecord.TabIndex = 16;
            this.btnQuotationRecord.Text = "Quotation Record";
            this.btnQuotationRecord.UseVisualStyleBackColor = true;
            this.btnQuotationRecord.Click += new System.EventHandler(this.BtnCanvassRecord_Click);
            // 
            // btnPaymentHistory
            // 
            this.btnPaymentHistory.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnPaymentHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnPaymentHistory.Location = new System.Drawing.Point(683, 94);
            this.btnPaymentHistory.Name = "btnPaymentHistory";
            this.btnPaymentHistory.Size = new System.Drawing.Size(145, 30);
            this.btnPaymentHistory.TabIndex = 17;
            this.btnPaymentHistory.Text = "Payment History";
            this.btnPaymentHistory.UseVisualStyleBackColor = true;
            this.btnPaymentHistory.Click += new System.EventHandler(this.BtnPaymentHistory_Click);
            // 
            // labelUnit
            // 
            this.labelUnit.Location = new System.Drawing.Point(27, 154);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(90, 23);
            this.labelUnit.TabIndex = 19;
            this.labelUnit.Text = "Unit";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Status", true));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.label2.Location = new System.Drawing.Point(683, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 23);
            this.label2.TabIndex = 23;
            this.label2.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // btnBatchApprove
            // 
            this.btnBatchApprove.Location = new System.Drawing.Point(678, 3);
            this.btnBatchApprove.Name = "btnBatchApprove";
            this.btnBatchApprove.Size = new System.Drawing.Size(203, 30);
            this.btnBatchApprove.TabIndex = 2;
            this.btnBatchApprove.Text = "Quotation Batch Approve";
            this.btnBatchApprove.UseVisualStyleBackColor = true;
            this.btnBatchApprove.Click += new System.EventHandler(this.BtnBatchApprove_Click);
            // 
            // txtUnit
            // 
            this.txtUnit.BackColor = System.Drawing.Color.White;
            this.txtUnit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "UnitID", true));
            this.txtUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtUnit.Location = new System.Drawing.Point(120, 154);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(66, 23);
            this.txtUnit.TabIndex = 20;
            // 
            // txtsubconSupplier
            // 
            this.txtsubconSupplier.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LocalSuppID", true));
            this.txtsubconSupplier.DisplayBox1Binding = "";
            this.txtsubconSupplier.IsIncludeJunk = false;
            this.txtsubconSupplier.IsMisc = false;
            this.txtsubconSupplier.IsShipping = false;
            this.txtsubconSupplier.IsSubcon = false;
            this.txtsubconSupplier.Location = new System.Drawing.Point(120, 187);
            this.txtsubconSupplier.Name = "txtsubconSupplier";
            this.txtsubconSupplier.Size = new System.Drawing.Size(159, 23);
            this.txtsubconSupplier.TabIndex = 10;
            this.txtsubconSupplier.TextBox1Binding = "";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(120, 118);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 1;
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "AccountID", true));
            this.txtAccountNo.DisplayBox1Binding = "";
            this.txtAccountNo.IsUnselectableShipB03 = true;
            this.txtAccountNo.Location = new System.Drawing.Point(120, 292);
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.Size = new System.Drawing.Size(308, 23);
            this.txtAccountNo.TabIndex = 24;
            this.txtAccountNo.TextBox1Binding = "";
            // 
            // B03
            // 
            this.ClientSize = new System.Drawing.Size(890, 457);
            this.CloseChkValue = "New";
            this.Controls.Add(this.btnBatchApprove);
            this.DefaultControl = "txtCode";
            this.DefaultControlForEdit = "editDescription";
            this.DefaultOrder = "ID";
            this.EnableGridJunkColor = true;
            this.ExpressQuery = true;
            this.IsSupportClose = true;
            this.IsSupportDelete = false;
            this.IsSupportJunk = true;
            this.IsSupportUnclose = true;
            this.IsSupportUnJunk = true;
            this.JunkChkValue = "New";
            this.Name = "B03";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B03. Shipping Expense";
            this.UncloseChkValue = "Locked";
            this.UniqueExpress = "ID";
            this.UnjunkChkValue = "Junked";
            this.WorkAlias = "ShipExpense";
            this.FormLoaded += new System.EventHandler(this.B03_FormLoaded);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.B03_FormClosing);
            this.Controls.SetChildIndex(this.tabs, 0);
            this.Controls.SetChildIndex(this.btnBatchApprove, 0);
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
        private Win.UI.DateBox dateCanvassDate;
        private Win.UI.NumericBox numPrice;
        private Win.UI.DisplayBox displayPrice;
        private Class.TxtsubconNoConfirm txtsubconSupplier;
        private Class.Txtbrand txtbrand;
        private Win.UI.EditBox editDescription;
        private Win.UI.TextBox txtCode;
        private Win.UI.Label labelAccountNo;
        private Win.UI.Label labelCanvassDate;
        private Win.UI.Label labelPrice;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelCode;
        private Win.UI.Button btnPaymentHistory;
        private Win.UI.Button btnQuotationRecord;
        private Win.UI.CheckBox checkBox1;
        private Win.UI.Label labelUnit;
        private Class.Txtunit_local txtUnit;
        private Win.UI.Label label2;
        private Win.UI.Button btnBatchApprove;
        private Class.TxtAccountNo txtAccountNo;
    }
}
