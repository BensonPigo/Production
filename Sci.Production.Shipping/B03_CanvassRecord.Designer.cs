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
            this.label8 = new Sci.Win.UI.Label();
            this.numericBox4 = new Sci.Win.UI.NumericBox();
            this.numericBox3 = new Sci.Win.UI.NumericBox();
            this.numericBox2 = new Sci.Win.UI.NumericBox();
            this.numericBox1 = new Sci.Win.UI.NumericBox();
            this.label7 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioButton4 = new Sci.Win.UI.RadioButton();
            this.radioButton3 = new Sci.Win.UI.RadioButton();
            this.radioButton2 = new Sci.Win.UI.RadioButton();
            this.radioButton1 = new Sci.Win.UI.RadioButton();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txtcurrency4 = new Sci.Production.Class.txtcurrency();
            this.txtcurrency3 = new Sci.Production.Class.txtcurrency();
            this.txtcurrency2 = new Sci.Production.Class.txtcurrency();
            this.txtcurrency1 = new Sci.Production.Class.txtcurrency();
            this.txtsubcon4 = new Sci.Production.Class.txtsubcon();
            this.txtsubcon3 = new Sci.Production.Class.txtsubcon();
            this.txtsubcon2 = new Sci.Production.Class.txtsubcon();
            this.txtsubcon1 = new Sci.Production.Class.txtsubcon();
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
            this.detail.Size = new System.Drawing.Size(676, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.label8);
            this.detailcont.Controls.Add(this.numericBox4);
            this.detailcont.Controls.Add(this.numericBox3);
            this.detailcont.Controls.Add(this.numericBox2);
            this.detailcont.Controls.Add(this.numericBox1);
            this.detailcont.Controls.Add(this.label7);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.radioPanel1);
            this.detailcont.Controls.Add(this.displayBox1);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.txtcurrency4);
            this.detailcont.Controls.Add(this.txtcurrency3);
            this.detailcont.Controls.Add(this.txtcurrency2);
            this.detailcont.Controls.Add(this.txtcurrency1);
            this.detailcont.Controls.Add(this.txtsubcon4);
            this.detailcont.Controls.Add(this.txtsubcon3);
            this.detailcont.Controls.Add(this.txtsubcon2);
            this.detailcont.Controls.Add(this.txtsubcon1);
            this.detailcont.Size = new System.Drawing.Size(676, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(676, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(676, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(684, 424);
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(332, 137);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 23);
            this.label8.TabIndex = 41;
            this.label8.Text = "Price";
            // 
            // numericBox4
            // 
            this.numericBox4.BackColor = System.Drawing.Color.White;
            this.numericBox4.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "price4", true));
            this.numericBox4.DecimalPlaces = 4;
            this.numericBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox4.Location = new System.Drawing.Point(425, 137);
            this.numericBox4.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            262144});
            this.numericBox4.MaxLength = 13;
            this.numericBox4.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.numericBox4.Name = "numericBox4";
            this.numericBox4.Size = new System.Drawing.Size(100, 23);
            this.numericBox4.TabIndex = 40;
            // 
            // numericBox3
            // 
            this.numericBox3.BackColor = System.Drawing.Color.White;
            this.numericBox3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "price3", true));
            this.numericBox3.DecimalPlaces = 4;
            this.numericBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox3.Location = new System.Drawing.Point(425, 107);
            this.numericBox3.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            262144});
            this.numericBox3.MaxLength = 13;
            this.numericBox3.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.numericBox3.Name = "numericBox3";
            this.numericBox3.Size = new System.Drawing.Size(100, 23);
            this.numericBox3.TabIndex = 39;
            // 
            // numericBox2
            // 
            this.numericBox2.BackColor = System.Drawing.Color.White;
            this.numericBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "price2", true));
            this.numericBox2.DecimalPlaces = 4;
            this.numericBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox2.Location = new System.Drawing.Point(425, 78);
            this.numericBox2.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            262144});
            this.numericBox2.MaxLength = 13;
            this.numericBox2.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.numericBox2.Name = "numericBox2";
            this.numericBox2.Size = new System.Drawing.Size(100, 23);
            this.numericBox2.TabIndex = 38;
            // 
            // numericBox1
            // 
            this.numericBox1.BackColor = System.Drawing.Color.White;
            this.numericBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "price1", true));
            this.numericBox1.DecimalPlaces = 4;
            this.numericBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox1.Location = new System.Drawing.Point(425, 47);
            this.numericBox1.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            262144});
            this.numericBox1.MaxLength = 13;
            this.numericBox1.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.Size = new System.Drawing.Size(100, 23);
            this.numericBox1.TabIndex = 37;
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(332, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 23);
            this.label7.TabIndex = 36;
            this.label7.Text = "Price";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(332, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 23);
            this.label6.TabIndex = 35;
            this.label6.Text = "Price";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(332, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 23);
            this.label5.TabIndex = 34;
            this.label5.Text = "Price";
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioButton4);
            this.radioPanel1.Controls.Add(this.radioButton3);
            this.radioPanel1.Controls.Add(this.radioButton2);
            this.radioPanel1.Controls.Add(this.radioButton1);
            this.radioPanel1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ChooseSupp", true));
            this.radioPanel1.Location = new System.Drawing.Point(32, 44);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(95, 123);
            this.radioPanel1.TabIndex = 32;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioButton4.Location = new System.Drawing.Point(3, 93);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(90, 21);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Supplier 4";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.Value = "4";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioButton3.Location = new System.Drawing.Point(3, 63);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(90, 21);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Supplier 3";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.Value = "3";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioButton2.Location = new System.Drawing.Point(3, 34);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(90, 21);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Supplier 2";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Value = "2";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioButton1.Location = new System.Drawing.Point(3, 5);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(90, 21);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Supplier 1";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Value = "1";
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(78, 18);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(140, 23);
            this.displayBox1.TabIndex = 31;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(32, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 23);
            this.label3.TabIndex = 30;
            this.label3.Text = "Code";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(557, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 23);
            this.label1.TabIndex = 42;
            this.label1.Text = "label1";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.label1.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // txtcurrency4
            // 
            this.txtcurrency4.BackColor = System.Drawing.Color.White;
            this.txtcurrency4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CurrencyID4", true));
            this.txtcurrency4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcurrency4.IsSupportSytsemContextMenu = false;
            this.txtcurrency4.Location = new System.Drawing.Point(379, 137);
            this.txtcurrency4.Name = "txtcurrency4";
            this.txtcurrency4.Size = new System.Drawing.Size(40, 23);
            this.txtcurrency4.TabIndex = 29;
            // 
            // txtcurrency3
            // 
            this.txtcurrency3.BackColor = System.Drawing.Color.White;
            this.txtcurrency3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CurrencyID3", true));
            this.txtcurrency3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcurrency3.IsSupportSytsemContextMenu = false;
            this.txtcurrency3.Location = new System.Drawing.Point(379, 107);
            this.txtcurrency3.Name = "txtcurrency3";
            this.txtcurrency3.Size = new System.Drawing.Size(40, 23);
            this.txtcurrency3.TabIndex = 28;
            // 
            // txtcurrency2
            // 
            this.txtcurrency2.BackColor = System.Drawing.Color.White;
            this.txtcurrency2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CurrencyID2", true));
            this.txtcurrency2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcurrency2.IsSupportSytsemContextMenu = false;
            this.txtcurrency2.Location = new System.Drawing.Point(379, 78);
            this.txtcurrency2.Name = "txtcurrency2";
            this.txtcurrency2.Size = new System.Drawing.Size(40, 23);
            this.txtcurrency2.TabIndex = 27;
            // 
            // txtcurrency1
            // 
            this.txtcurrency1.BackColor = System.Drawing.Color.White;
            this.txtcurrency1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CurrencyID1", true));
            this.txtcurrency1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcurrency1.IsSupportSytsemContextMenu = false;
            this.txtcurrency1.Location = new System.Drawing.Point(379, 47);
            this.txtcurrency1.Name = "txtcurrency1";
            this.txtcurrency1.Size = new System.Drawing.Size(40, 23);
            this.txtcurrency1.TabIndex = 26;
            // 
            // txtsubcon4
            // 
            this.txtsubcon4.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LocalSuppID4", true));
            this.txtsubcon4.DisplayBox1Binding = "";
            this.txtsubcon4.IsIncludeJunk = false;
            this.txtsubcon4.Location = new System.Drawing.Point(133, 138);
            this.txtsubcon4.Name = "txtsubcon4";
            this.txtsubcon4.Size = new System.Drawing.Size(159, 23);
            this.txtsubcon4.TabIndex = 25;
            this.txtsubcon4.TextBox1Binding = "";
            // 
            // txtsubcon3
            // 
            this.txtsubcon3.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LocalSuppID3", true));
            this.txtsubcon3.DisplayBox1Binding = "";
            this.txtsubcon3.IsIncludeJunk = false;
            this.txtsubcon3.Location = new System.Drawing.Point(133, 108);
            this.txtsubcon3.Name = "txtsubcon3";
            this.txtsubcon3.Size = new System.Drawing.Size(159, 23);
            this.txtsubcon3.TabIndex = 24;
            this.txtsubcon3.TextBox1Binding = "";
            // 
            // txtsubcon2
            // 
            this.txtsubcon2.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LocalSuppID2", true));
            this.txtsubcon2.DisplayBox1Binding = "";
            this.txtsubcon2.IsIncludeJunk = false;
            this.txtsubcon2.Location = new System.Drawing.Point(133, 79);
            this.txtsubcon2.Name = "txtsubcon2";
            this.txtsubcon2.Size = new System.Drawing.Size(159, 23);
            this.txtsubcon2.TabIndex = 23;
            this.txtsubcon2.TextBox1Binding = "";
            // 
            // txtsubcon1
            // 
            this.txtsubcon1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LocalSuppID1", true));
            this.txtsubcon1.DisplayBox1Binding = "";
            this.txtsubcon1.IsIncludeJunk = false;
            this.txtsubcon1.Location = new System.Drawing.Point(133, 48);
            this.txtsubcon1.Name = "txtsubcon1";
            this.txtsubcon1.Size = new System.Drawing.Size(159, 23);
            this.txtsubcon1.TabIndex = 22;
            this.txtsubcon1.TextBox1Binding = "";
            // 
            // B03_CanvassRecord
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(684, 457);
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

        private Win.UI.Label label8;
        private Win.UI.NumericBox numericBox4;
        private Win.UI.NumericBox numericBox3;
        private Win.UI.NumericBox numericBox2;
        private Win.UI.NumericBox numericBox1;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioButton4;
        private Win.UI.RadioButton radioButton3;
        private Win.UI.RadioButton radioButton2;
        private Win.UI.RadioButton radioButton1;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.Label label3;
        private Class.txtcurrency txtcurrency4;
        private Class.txtcurrency txtcurrency3;
        private Class.txtcurrency txtcurrency2;
        private Class.txtcurrency txtcurrency1;
        private Class.txtsubcon txtsubcon4;
        private Class.txtsubcon txtsubcon3;
        private Class.txtsubcon txtsubcon2;
        private Class.txtsubcon txtsubcon1;
        private Win.UI.Label label1;
    }
}
