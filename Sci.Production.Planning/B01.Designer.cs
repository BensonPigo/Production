namespace Sci.Production.Planning
{
    partial class B01
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
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.dateBox1 = new Sci.Win.UI.DateBox();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.txtsubcon1 = new Sci.Production.Class.txtsubcon();
            this.numericBox1 = new Sci.Win.UI.NumericBox();
            this.numericBox2 = new Sci.Win.UI.NumericBox();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioButton2 = new Sci.Win.UI.RadioButton();
            this.radioButton1 = new Sci.Win.UI.RadioButton();
            this.label25 = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(900, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.label25);
            this.detailcont.Controls.Add(this.radioPanel1);
            this.detailcont.Controls.Add(this.numericBox2);
            this.detailcont.Controls.Add(this.numericBox1);
            this.detailcont.Controls.Add(this.txtsubcon1);
            this.detailcont.Controls.Add(this.comboBox1);
            this.detailcont.Controls.Add(this.dateBox1);
            this.detailcont.Controls.Add(this.label7);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Size = new System.Drawing.Size(900, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(900, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(900, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(908, 424);
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(16, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Date";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(16, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "ArtworkType";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(16, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 23);
            this.label5.TabIndex = 6;
            this.label5.Text = "Fty / Supp";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(16, 112);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 23);
            this.label6.TabIndex = 7;
            this.label6.Text = "Capacity (unit)";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(16, 144);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 23);
            this.label7.TabIndex = 8;
            this.label7.Text = "# of Heads";
            // 
            // dateBox1
            // 
            this.dateBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "issuedate", true));
            this.dateBox1.Location = new System.Drawing.Point(126, 16);
            this.dateBox1.Name = "dateBox1";
            this.dateBox1.Size = new System.Drawing.Size(130, 23);
            this.dateBox1.TabIndex = 1;
            this.dateBox1.Validated += new System.EventHandler(this.dateBox1_Validated);
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "artworktypeid", true));
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(126, 48);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(159, 24);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // txtsubcon1
            // 
            this.txtsubcon1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "ftysupp", true));
            this.txtsubcon1.DisplayBox1Binding = "";
            this.txtsubcon1.IsIncludeJunk = false;
            this.txtsubcon1.Location = new System.Drawing.Point(126, 80);
            this.txtsubcon1.Name = "txtsubcon1";
            this.txtsubcon1.Size = new System.Drawing.Size(159, 23);
            this.txtsubcon1.TabIndex = 3;
            this.txtsubcon1.TextBox1Binding = "";
            // 
            // numericBox1
            // 
            this.numericBox1.BackColor = System.Drawing.Color.White;
            this.numericBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "capacity", true));
            this.numericBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox1.Location = new System.Drawing.Point(126, 112);
            this.numericBox1.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericBox1.MaxLength = 7;
            this.numericBox1.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox1.Size = new System.Drawing.Size(100, 23);
            this.numericBox1.TabIndex = 4;
            this.numericBox1.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numericBox2
            // 
            this.numericBox2.BackColor = System.Drawing.Color.White;
            this.numericBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "heads", true));
            this.numericBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox2.Location = new System.Drawing.Point(126, 144);
            this.numericBox2.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericBox2.MaxLength = 2;
            this.numericBox2.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox2.Name = "numericBox2";
            this.numericBox2.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox2.Size = new System.Drawing.Size(100, 23);
            this.numericBox2.TabIndex = 8;
            this.numericBox2.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioButton2);
            this.radioPanel1.Controls.Add(this.radioButton1);
            this.radioPanel1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "unit", true));
            this.radioPanel1.Location = new System.Drawing.Point(247, 112);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(229, 31);
            this.radioPanel1.TabIndex = 5;
            this.radioPanel1.Validated += new System.EventHandler(this.radioPanel1_Validated);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioButton2.Location = new System.Drawing.Point(77, 3);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(84, 21);
            this.radioButton2.TabIndex = 7;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "by month";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Value = "2";
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioButton1.Location = new System.Drawing.Point(3, 2);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(68, 21);
            this.radioButton1.TabIndex = 6;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "by day";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Value = "1";
            // 
            // label25
            // 
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Lines = 0;
            this.label25.Location = new System.Drawing.Point(313, 49);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(326, 23);
            this.label25.TabIndex = 43;
            this.label25.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // B01
            // 
            this.ClientSize = new System.Drawing.Size(908, 457);
            this.DefaultControl = "datebox1";
            this.DefaultControlForEdit = "datebox1";
            this.DefaultOrder = "ISSUEDATE,ARTWORKTYPEID,FTYSUPP";
            this.IsSupportPrint = false;
            this.Name = "B01";
            this.Text = "B01. Sub-process capacity";
            this.UniqueExpress = "ISSUEDATE,ARTWORKTYPEID,FTYSUPP";
            this.WorkAlias = "capacity";
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

        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioButton2;
        private Win.UI.RadioButton radioButton1;
        private Win.UI.NumericBox numericBox2;
        private Win.UI.NumericBox numericBox1;
        private Class.txtsubcon txtsubcon1;
        private Win.UI.ComboBox comboBox1;
        private Win.UI.DateBox dateBox1;
        private Win.UI.Label label7;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.Label label25;
    }
}
