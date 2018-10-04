namespace Sci.Production.Shipping
{
    partial class B03_CanvassRecord
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
            this.labelPrice4 = new Sci.Win.UI.Label();
            this.numPrice4 = new Sci.Win.UI.NumericBox();
            this.numPrice3 = new Sci.Win.UI.NumericBox();
            this.numPrice2 = new Sci.Win.UI.NumericBox();
            this.numPrice1 = new Sci.Win.UI.NumericBox();
            this.labelPrice3 = new Sci.Win.UI.Label();
            this.labelPrice2 = new Sci.Win.UI.Label();
            this.labelPrice1 = new Sci.Win.UI.Label();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioSupplier4 = new Sci.Win.UI.RadioButton();
            this.radioSupplier3 = new Sci.Win.UI.RadioButton();
            this.radioSupplier2 = new Sci.Win.UI.RadioButton();
            this.radioSupplier1 = new Sci.Win.UI.RadioButton();
            this.displayCode = new Sci.Win.UI.DisplayBox();
            this.labelCode = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txtCurrency4 = new Sci.Production.Class.txtcurrency();
            this.txtCurrency3 = new Sci.Production.Class.txtcurrency();
            this.txtCurrency2 = new Sci.Production.Class.txtcurrency();
            this.txtCurrency1 = new Sci.Production.Class.txtcurrency();
            this.txtsubconSupplier4 = new Sci.Production.Class.txtsubcon();
            this.txtsubconSupplier3 = new Sci.Production.Class.txtsubcon();
            this.txtsubconSupplier2 = new Sci.Production.Class.txtsubcon();
            this.txtsubconSupplier1 = new Sci.Production.Class.txtsubcon();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(826, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.labelPrice4);
            this.detailcont.Controls.Add(this.numPrice4);
            this.detailcont.Controls.Add(this.numPrice3);
            this.detailcont.Controls.Add(this.numPrice2);
            this.detailcont.Controls.Add(this.numPrice1);
            this.detailcont.Controls.Add(this.labelPrice3);
            this.detailcont.Controls.Add(this.labelPrice2);
            this.detailcont.Controls.Add(this.labelPrice1);
            this.detailcont.Controls.Add(this.radioPanel1);
            this.detailcont.Controls.Add(this.displayCode);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Controls.Add(this.txtCurrency4);
            this.detailcont.Controls.Add(this.txtCurrency3);
            this.detailcont.Controls.Add(this.txtCurrency2);
            this.detailcont.Controls.Add(this.txtCurrency1);
            this.detailcont.Controls.Add(this.txtsubconSupplier4);
            this.detailcont.Controls.Add(this.txtsubconSupplier3);
            this.detailcont.Controls.Add(this.txtsubconSupplier2);
            this.detailcont.Controls.Add(this.txtsubconSupplier1);
            this.detailcont.Size = new System.Drawing.Size(826, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(826, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(826, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(834, 424);
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
            // labelPrice4
            // 
            this.labelPrice4.Location = new System.Drawing.Point(332, 137);
            this.labelPrice4.Name = "labelPrice4";
            this.labelPrice4.Size = new System.Drawing.Size(44, 23);
            this.labelPrice4.TabIndex = 41;
            this.labelPrice4.Text = "Price";
            // 
            // numPrice4
            // 
            this.numPrice4.BackColor = System.Drawing.Color.White;
            this.numPrice4.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "price4", true));
            this.numPrice4.DecimalPlaces = 4;
            this.numPrice4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numPrice4.Location = new System.Drawing.Point(425, 137);
            this.numPrice4.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            262144});
            this.numPrice4.MaxLength = 13;
            this.numPrice4.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.numPrice4.Name = "numPrice4";
            this.numPrice4.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPrice4.Size = new System.Drawing.Size(100, 23);
            this.numPrice4.TabIndex = 40;
            this.numPrice4.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numPrice3
            // 
            this.numPrice3.BackColor = System.Drawing.Color.White;
            this.numPrice3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "price3", true));
            this.numPrice3.DecimalPlaces = 4;
            this.numPrice3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numPrice3.Location = new System.Drawing.Point(425, 107);
            this.numPrice3.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            262144});
            this.numPrice3.MaxLength = 13;
            this.numPrice3.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.numPrice3.Name = "numPrice3";
            this.numPrice3.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPrice3.Size = new System.Drawing.Size(100, 23);
            this.numPrice3.TabIndex = 39;
            this.numPrice3.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numPrice2
            // 
            this.numPrice2.BackColor = System.Drawing.Color.White;
            this.numPrice2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "price2", true));
            this.numPrice2.DecimalPlaces = 4;
            this.numPrice2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numPrice2.Location = new System.Drawing.Point(425, 78);
            this.numPrice2.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            262144});
            this.numPrice2.MaxLength = 13;
            this.numPrice2.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.numPrice2.Name = "numPrice2";
            this.numPrice2.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPrice2.Size = new System.Drawing.Size(100, 23);
            this.numPrice2.TabIndex = 38;
            this.numPrice2.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numPrice1
            // 
            this.numPrice1.BackColor = System.Drawing.Color.White;
            this.numPrice1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "price1", true));
            this.numPrice1.DecimalPlaces = 4;
            this.numPrice1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numPrice1.Location = new System.Drawing.Point(425, 47);
            this.numPrice1.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            262144});
            this.numPrice1.MaxLength = 13;
            this.numPrice1.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.numPrice1.Name = "numPrice1";
            this.numPrice1.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPrice1.Size = new System.Drawing.Size(100, 23);
            this.numPrice1.TabIndex = 37;
            this.numPrice1.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelPrice3
            // 
            this.labelPrice3.Location = new System.Drawing.Point(332, 108);
            this.labelPrice3.Name = "labelPrice3";
            this.labelPrice3.Size = new System.Drawing.Size(44, 23);
            this.labelPrice3.TabIndex = 36;
            this.labelPrice3.Text = "Price";
            // 
            // labelPrice2
            // 
            this.labelPrice2.Location = new System.Drawing.Point(332, 79);
            this.labelPrice2.Name = "labelPrice2";
            this.labelPrice2.Size = new System.Drawing.Size(44, 23);
            this.labelPrice2.TabIndex = 35;
            this.labelPrice2.Text = "Price";
            // 
            // labelPrice1
            // 
            this.labelPrice1.Location = new System.Drawing.Point(332, 48);
            this.labelPrice1.Name = "labelPrice1";
            this.labelPrice1.Size = new System.Drawing.Size(44, 23);
            this.labelPrice1.TabIndex = 34;
            this.labelPrice1.Text = "Price";
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioSupplier4);
            this.radioPanel1.Controls.Add(this.radioSupplier3);
            this.radioPanel1.Controls.Add(this.radioSupplier2);
            this.radioPanel1.Controls.Add(this.radioSupplier1);
            this.radioPanel1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ChooseSupp", true));
            this.radioPanel1.Location = new System.Drawing.Point(32, 44);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(95, 123);
            this.radioPanel1.TabIndex = 32;
            this.radioPanel1.Value = "1";
            // 
            // radioSupplier4
            // 
            this.radioSupplier4.AutoSize = true;
            this.radioSupplier4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSupplier4.Location = new System.Drawing.Point(3, 93);
            this.radioSupplier4.Name = "radioSupplier4";
            this.radioSupplier4.Size = new System.Drawing.Size(90, 21);
            this.radioSupplier4.TabIndex = 3;
            this.radioSupplier4.Text = "Supplier 4";
            this.radioSupplier4.UseVisualStyleBackColor = true;
            this.radioSupplier4.Value = "4";
            // 
            // radioSupplier3
            // 
            this.radioSupplier3.AutoSize = true;
            this.radioSupplier3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSupplier3.Location = new System.Drawing.Point(3, 63);
            this.radioSupplier3.Name = "radioSupplier3";
            this.radioSupplier3.Size = new System.Drawing.Size(90, 21);
            this.radioSupplier3.TabIndex = 2;
            this.radioSupplier3.Text = "Supplier 3";
            this.radioSupplier3.UseVisualStyleBackColor = true;
            this.radioSupplier3.Value = "3";
            // 
            // radioSupplier2
            // 
            this.radioSupplier2.AutoSize = true;
            this.radioSupplier2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSupplier2.Location = new System.Drawing.Point(3, 34);
            this.radioSupplier2.Name = "radioSupplier2";
            this.radioSupplier2.Size = new System.Drawing.Size(90, 21);
            this.radioSupplier2.TabIndex = 1;
            this.radioSupplier2.Text = "Supplier 2";
            this.radioSupplier2.UseVisualStyleBackColor = true;
            this.radioSupplier2.Value = "2";
            // 
            // radioSupplier1
            // 
            this.radioSupplier1.AutoSize = true;
            this.radioSupplier1.Checked = true;
            this.radioSupplier1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSupplier1.Location = new System.Drawing.Point(3, 5);
            this.radioSupplier1.Name = "radioSupplier1";
            this.radioSupplier1.Size = new System.Drawing.Size(90, 21);
            this.radioSupplier1.TabIndex = 0;
            this.radioSupplier1.TabStop = true;
            this.radioSupplier1.Text = "Supplier 1";
            this.radioSupplier1.UseVisualStyleBackColor = true;
            this.radioSupplier1.Value = "1";
            // 
            // displayCode
            // 
            this.displayCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCode.Location = new System.Drawing.Point(78, 18);
            this.displayCode.Name = "displayCode";
            this.displayCode.Size = new System.Drawing.Size(140, 23);
            this.displayCode.TabIndex = 31;
            // 
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(32, 18);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(42, 23);
            this.labelCode.TabIndex = 30;
            this.labelCode.Text = "Code";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label1.Location = new System.Drawing.Point(557, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 23);
            this.label1.TabIndex = 42;
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.label1.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // txtCurrency4
            // 
            this.txtCurrency4.BackColor = System.Drawing.Color.White;
            this.txtCurrency4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CurrencyID4", true));
            this.txtCurrency4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCurrency4.IsSupportSytsemContextMenu = false;
            this.txtCurrency4.Location = new System.Drawing.Point(379, 137);
            this.txtCurrency4.Name = "txtCurrency4";
            this.txtCurrency4.Size = new System.Drawing.Size(40, 23);
            this.txtCurrency4.TabIndex = 29;
            // 
            // txtCurrency3
            // 
            this.txtCurrency3.BackColor = System.Drawing.Color.White;
            this.txtCurrency3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CurrencyID3", true));
            this.txtCurrency3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCurrency3.IsSupportSytsemContextMenu = false;
            this.txtCurrency3.Location = new System.Drawing.Point(379, 107);
            this.txtCurrency3.Name = "txtCurrency3";
            this.txtCurrency3.Size = new System.Drawing.Size(40, 23);
            this.txtCurrency3.TabIndex = 28;
            // 
            // txtCurrency2
            // 
            this.txtCurrency2.BackColor = System.Drawing.Color.White;
            this.txtCurrency2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CurrencyID2", true));
            this.txtCurrency2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCurrency2.IsSupportSytsemContextMenu = false;
            this.txtCurrency2.Location = new System.Drawing.Point(379, 78);
            this.txtCurrency2.Name = "txtCurrency2";
            this.txtCurrency2.Size = new System.Drawing.Size(40, 23);
            this.txtCurrency2.TabIndex = 27;
            // 
            // txtCurrency1
            // 
            this.txtCurrency1.BackColor = System.Drawing.Color.White;
            this.txtCurrency1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CurrencyID1", true));
            this.txtCurrency1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCurrency1.IsSupportSytsemContextMenu = false;
            this.txtCurrency1.Location = new System.Drawing.Point(379, 47);
            this.txtCurrency1.Name = "txtCurrency1";
            this.txtCurrency1.Size = new System.Drawing.Size(40, 23);
            this.txtCurrency1.TabIndex = 26;
            // 
            // txtsubconSupplier4
            // 
            this.txtsubconSupplier4.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LocalSuppID4", true));
            this.txtsubconSupplier4.DisplayBox1Binding = "";
            this.txtsubconSupplier4.IsIncludeJunk = false;
            this.txtsubconSupplier4.Location = new System.Drawing.Point(133, 138);
            this.txtsubconSupplier4.Name = "txtsubconSupplier4";
            this.txtsubconSupplier4.Size = new System.Drawing.Size(159, 23);
            this.txtsubconSupplier4.TabIndex = 25;
            this.txtsubconSupplier4.TextBox1Binding = "";
            // 
            // txtsubconSupplier3
            // 
            this.txtsubconSupplier3.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LocalSuppID3", true));
            this.txtsubconSupplier3.DisplayBox1Binding = "";
            this.txtsubconSupplier3.IsIncludeJunk = false;
            this.txtsubconSupplier3.Location = new System.Drawing.Point(133, 108);
            this.txtsubconSupplier3.Name = "txtsubconSupplier3";
            this.txtsubconSupplier3.Size = new System.Drawing.Size(159, 23);
            this.txtsubconSupplier3.TabIndex = 24;
            this.txtsubconSupplier3.TextBox1Binding = "";
            // 
            // txtsubconSupplier2
            // 
            this.txtsubconSupplier2.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LocalSuppID2", true));
            this.txtsubconSupplier2.DisplayBox1Binding = "";
            this.txtsubconSupplier2.IsIncludeJunk = false;
            this.txtsubconSupplier2.Location = new System.Drawing.Point(133, 79);
            this.txtsubconSupplier2.Name = "txtsubconSupplier2";
            this.txtsubconSupplier2.Size = new System.Drawing.Size(159, 23);
            this.txtsubconSupplier2.TabIndex = 23;
            this.txtsubconSupplier2.TextBox1Binding = "";
            // 
            // txtsubconSupplier1
            // 
            this.txtsubconSupplier1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LocalSuppID1", true));
            this.txtsubconSupplier1.DisplayBox1Binding = "";
            this.txtsubconSupplier1.IsIncludeJunk = false;
            this.txtsubconSupplier1.Location = new System.Drawing.Point(133, 48);
            this.txtsubconSupplier1.Name = "txtsubconSupplier1";
            this.txtsubconSupplier1.Size = new System.Drawing.Size(159, 23);
            this.txtsubconSupplier1.TabIndex = 22;
            this.txtsubconSupplier1.TextBox1Binding = "";
            // 
            // B03_CanvassRecord
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(834, 457);
            this.DefaultOrder = "ID,AddDate";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.Name = "B03_CanvassRecord";
            this.Text = "Canvass record";
            this.WorkAlias = "ShipExpense_CanVass";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelPrice4;
        private Win.UI.NumericBox numPrice4;
        private Win.UI.NumericBox numPrice3;
        private Win.UI.NumericBox numPrice2;
        private Win.UI.NumericBox numPrice1;
        private Win.UI.Label labelPrice3;
        private Win.UI.Label labelPrice2;
        private Win.UI.Label labelPrice1;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioSupplier4;
        private Win.UI.RadioButton radioSupplier3;
        private Win.UI.RadioButton radioSupplier2;
        private Win.UI.RadioButton radioSupplier1;
        private Win.UI.DisplayBox displayCode;
        private Win.UI.Label labelCode;
        private Class.txtcurrency txtCurrency4;
        private Class.txtcurrency txtCurrency3;
        private Class.txtcurrency txtCurrency2;
        private Class.txtcurrency txtCurrency1;
        private Class.txtsubcon txtsubconSupplier4;
        private Class.txtsubcon txtsubconSupplier3;
        private Class.txtsubcon txtsubconSupplier2;
        private Class.txtsubcon txtsubconSupplier1;
        private Win.UI.Label label1;
    }
}
