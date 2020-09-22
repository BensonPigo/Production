namespace Sci.Production.Subcon
{
    partial class P36_ModifyAfterSent
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
            this.components = new System.ComponentModel.Container();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.labelDescription = new Sci.Win.UI.Label();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.mtbs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.numExchange = new Sci.Win.UI.NumericBox();
            this.labelExchange = new Sci.Win.UI.Label();
            this.numtaxrate = new Sci.Win.UI.NumericBox();
            this.numTax = new Sci.Win.UI.NumericBox();
            this.labelTax = new Sci.Win.UI.Label();
            this.numAmount = new Sci.Win.UI.NumericBox();
            this.labelAmount = new Sci.Win.UI.Label();
            this.labelAccountNo = new Sci.Win.UI.Label();
            this.txtAccountNo = new Sci.Production.Class.TxtAccountNo();
            this.displayBoxTPECurrency = new Sci.Win.UI.DisplayBox();
            this.numericTaipeiAMT = new Sci.Win.UI.NumericBox();
            this.labOriDebit = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.displayBoxCurrency = new Sci.Win.UI.DisplayBox();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(465, 271);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(379, 271);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(16, 73);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(92, 23);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = "Description";
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(112, 73);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(433, 155);
            this.editDescription.TabIndex = 1;
            // 
            // numExchange
            // 
            this.numExchange.BackColor = System.Drawing.Color.White;
            this.numExchange.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "exchange", true));
            this.numExchange.DecimalPlaces = 3;
            this.numExchange.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numExchange.Location = new System.Drawing.Point(112, 46);
            this.numExchange.MaxBytes = 8;
            this.numExchange.Name = "numExchange";
            this.numExchange.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numExchange.Size = new System.Drawing.Size(103, 23);
            this.numExchange.TabIndex = 0;
            this.numExchange.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numExchange.Validating += new System.ComponentModel.CancelEventHandler(this.NumExchange_Validating);
            // 
            // labelExchange
            // 
            this.labelExchange.Location = new System.Drawing.Point(16, 46);
            this.labelExchange.Name = "labelExchange";
            this.labelExchange.Size = new System.Drawing.Size(92, 23);
            this.labelExchange.TabIndex = 87;
            this.labelExchange.Text = "Exchange";
            // 
            // numtaxrate
            // 
            this.numtaxrate.BackColor = System.Drawing.Color.White;
            this.numtaxrate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "taxrate", true));
            this.numtaxrate.DecimalPlaces = 2;
            this.numtaxrate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numtaxrate.Location = new System.Drawing.Point(464, 46);
            this.numtaxrate.MaxBytes = 5;
            this.numtaxrate.Name = "numtaxrate";
            this.numtaxrate.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numtaxrate.Size = new System.Drawing.Size(60, 23);
            this.numtaxrate.TabIndex = 4;
            this.numtaxrate.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numtaxrate.Validating += new System.ComponentModel.CancelEventHandler(this.Numtaxrate_Validating);
            // 
            // numTax
            // 
            this.numTax.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTax.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "tax", true));
            this.numTax.DecimalPlaces = 2;
            this.numTax.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTax.IsSupportEditMode = false;
            this.numTax.Location = new System.Drawing.Point(358, 46);
            this.numTax.MaxBytes = 11;
            this.numTax.Name = "numTax";
            this.numTax.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTax.ReadOnly = true;
            this.numTax.Size = new System.Drawing.Size(100, 23);
            this.numTax.TabIndex = 5;
            this.numTax.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelTax
            // 
            this.labelTax.Location = new System.Drawing.Point(280, 46);
            this.labelTax.Name = "labelTax";
            this.labelTax.Size = new System.Drawing.Size(75, 23);
            this.labelTax.TabIndex = 84;
            this.labelTax.Text = "Tax";
            // 
            // numAmount
            // 
            this.numAmount.BackColor = System.Drawing.Color.White;
            this.numAmount.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "amount", true));
            this.numAmount.DecimalPlaces = 2;
            this.numAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numAmount.Location = new System.Drawing.Point(358, 19);
            this.numAmount.MaxBytes = 12;
            this.numAmount.Name = "numAmount";
            this.numAmount.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAmount.Size = new System.Drawing.Size(100, 23);
            this.numAmount.TabIndex = 3;
            this.numAmount.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAmount.Validating += new System.ComponentModel.CancelEventHandler(this.NumAmount_Validating);
            // 
            // labelAmount
            // 
            this.labelAmount.Location = new System.Drawing.Point(280, 19);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(75, 23);
            this.labelAmount.TabIndex = 82;
            this.labelAmount.Text = "Amount";
            // 
            // labelAccountNo
            // 
            this.labelAccountNo.Location = new System.Drawing.Point(16, 241);
            this.labelAccountNo.Name = "labelAccountNo";
            this.labelAccountNo.Size = new System.Drawing.Size(92, 23);
            this.labelAccountNo.TabIndex = 90;
            this.labelAccountNo.Text = "Account No";
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "AccountID", true));
            this.txtAccountNo.DisplayBox1Binding = "";
            this.txtAccountNo.Location = new System.Drawing.Point(112, 241);
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.Size = new System.Drawing.Size(300, 23);
            this.txtAccountNo.TabIndex = 2;
            this.txtAccountNo.TextBox1Binding = "";
            // 
            // displayBoxTPECurrency
            // 
            this.displayBoxTPECurrency.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxTPECurrency.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "TaipeiCurrencyID", true));
            this.displayBoxTPECurrency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxTPECurrency.Location = new System.Drawing.Point(217, 19);
            this.displayBoxTPECurrency.Name = "displayBoxTPECurrency";
            this.displayBoxTPECurrency.Size = new System.Drawing.Size(43, 23);
            this.displayBoxTPECurrency.TabIndex = 92;
            // 
            // numericTaipeiAMT
            // 
            this.numericTaipeiAMT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numericTaipeiAMT.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "TaipeiAMT", true));
            this.numericTaipeiAMT.DecimalPlaces = 2;
            this.numericTaipeiAMT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numericTaipeiAMT.IsSupportEditMode = false;
            this.numericTaipeiAMT.Location = new System.Drawing.Point(112, 19);
            this.numericTaipeiAMT.MaxBytes = 12;
            this.numericTaipeiAMT.Name = "numericTaipeiAMT";
            this.numericTaipeiAMT.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericTaipeiAMT.ReadOnly = true;
            this.numericTaipeiAMT.Size = new System.Drawing.Size(103, 23);
            this.numericTaipeiAMT.TabIndex = 91;
            this.numericTaipeiAMT.TabStop = false;
            this.numericTaipeiAMT.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labOriDebit
            // 
            this.labOriDebit.Location = new System.Drawing.Point(16, 19);
            this.labOriDebit.Name = "labOriDebit";
            this.labOriDebit.Size = new System.Drawing.Size(92, 23);
            this.labOriDebit.TabIndex = 93;
            this.labOriDebit.Text = "Ori. Debit Amt.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(527, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "%";
            // 
            // displayBoxCurrency
            // 
            this.displayBoxCurrency.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxCurrency.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CurrencyID", true));
            this.displayBoxCurrency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxCurrency.Location = new System.Drawing.Point(464, 19);
            this.displayBoxCurrency.Name = "displayBoxCurrency";
            this.displayBoxCurrency.Size = new System.Drawing.Size(43, 23);
            this.displayBoxCurrency.TabIndex = 95;
            // 
            // P36_ModifyAfterSent
            // 
            this.ClientSize = new System.Drawing.Size(565, 313);
            this.Controls.Add(this.displayBoxCurrency);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.displayBoxTPECurrency);
            this.Controls.Add(this.numericTaipeiAMT);
            this.Controls.Add(this.labOriDebit);
            this.Controls.Add(this.numExchange);
            this.Controls.Add(this.labelExchange);
            this.Controls.Add(this.numtaxrate);
            this.Controls.Add(this.numTax);
            this.Controls.Add(this.labelTax);
            this.Controls.Add(this.numAmount);
            this.Controls.Add(this.labelAmount);
            this.Controls.Add(this.labelAccountNo);
            this.Controls.Add(this.txtAccountNo);
            this.Controls.Add(this.editDescription);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Name = "P36_ModifyAfterSent";
            this.Text = "P36. Modify After Sent";
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave;
        private Win.UI.Label labelDescription;
        private Win.UI.EditBox editDescription;
        private Win.UI.NumericBox numExchange;
        private Win.UI.Label labelExchange;
        private Win.UI.NumericBox numtaxrate;
        private Win.UI.NumericBox numTax;
        private Win.UI.Label labelTax;
        private Win.UI.NumericBox numAmount;
        private Win.UI.Label labelAmount;
        private Win.UI.Label labelAccountNo;
        private Class.TxtAccountNo txtAccountNo;
        private Win.UI.ListControlBindingSource mtbs;
        private Win.UI.DisplayBox displayBoxTPECurrency;
        private Win.UI.NumericBox numericTaipeiAMT;
        private Win.UI.Label labOriDebit;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox displayBoxCurrency;
    }
}
