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
            this.labelDate = new Sci.Win.UI.Label();
            this.labelArtworkType = new Sci.Win.UI.Label();
            this.labelFtySupp = new Sci.Win.UI.Label();
            this.labelCapacity = new Sci.Win.UI.Label();
            this.labelHeads = new Sci.Win.UI.Label();
            this.dateDate = new Sci.Win.UI.DateBox();
            this.comboArtworkType = new Sci.Win.UI.ComboBox();
            this.numCapacity = new Sci.Win.UI.NumericBox();
            this.numHeads = new Sci.Win.UI.NumericBox();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radiobyMonth = new Sci.Win.UI.RadioButton();
            this.radiobyDay = new Sci.Win.UI.RadioButton();
            this.label25 = new Sci.Win.UI.Label();
            this.txtsubconFtySupp = new Sci.Production.Class.TxtsubconNoConfirm();
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
            this.detail.Size = new System.Drawing.Size(831, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.label25);
            this.detailcont.Controls.Add(this.radioPanel1);
            this.detailcont.Controls.Add(this.numHeads);
            this.detailcont.Controls.Add(this.numCapacity);
            this.detailcont.Controls.Add(this.txtsubconFtySupp);
            this.detailcont.Controls.Add(this.comboArtworkType);
            this.detailcont.Controls.Add(this.dateDate);
            this.detailcont.Controls.Add(this.labelHeads);
            this.detailcont.Controls.Add(this.labelDate);
            this.detailcont.Controls.Add(this.labelArtworkType);
            this.detailcont.Controls.Add(this.labelFtySupp);
            this.detailcont.Controls.Add(this.labelCapacity);
            this.detailcont.Size = new System.Drawing.Size(831, 357);
            this.detailcont.TabIndex = 1;
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(831, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(831, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(839, 424);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelDate
            // 
            this.labelDate.Lines = 0;
            this.labelDate.Location = new System.Drawing.Point(16, 16);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(107, 23);
            this.labelDate.TabIndex = 4;
            this.labelDate.Text = "Date";
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.Lines = 0;
            this.labelArtworkType.Location = new System.Drawing.Point(16, 48);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.Size = new System.Drawing.Size(107, 23);
            this.labelArtworkType.TabIndex = 5;
            this.labelArtworkType.Text = "ArtworkType";
            // 
            // labelFtySupp
            // 
            this.labelFtySupp.Lines = 0;
            this.labelFtySupp.Location = new System.Drawing.Point(16, 80);
            this.labelFtySupp.Name = "labelFtySupp";
            this.labelFtySupp.Size = new System.Drawing.Size(107, 23);
            this.labelFtySupp.TabIndex = 6;
            this.labelFtySupp.Text = "Fty / Supp";
            // 
            // labelCapacity
            // 
            this.labelCapacity.Lines = 0;
            this.labelCapacity.Location = new System.Drawing.Point(16, 112);
            this.labelCapacity.Name = "labelCapacity";
            this.labelCapacity.Size = new System.Drawing.Size(107, 23);
            this.labelCapacity.TabIndex = 7;
            this.labelCapacity.Text = "Capacity (unit)";
            // 
            // labelHeads
            // 
            this.labelHeads.Lines = 0;
            this.labelHeads.Location = new System.Drawing.Point(16, 144);
            this.labelHeads.Name = "labelHeads";
            this.labelHeads.Size = new System.Drawing.Size(107, 23);
            this.labelHeads.TabIndex = 8;
            this.labelHeads.Text = "# of Heads";
            // 
            // dateDate
            // 
            this.dateDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "issuedate", true));
            this.dateDate.Location = new System.Drawing.Point(126, 16);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(130, 23);
            this.dateDate.TabIndex = 0;
            this.dateDate.Validated += new System.EventHandler(this.DateDate_Validated);
            // 
            // comboArtworkType
            // 
            this.comboArtworkType.BackColor = System.Drawing.Color.White;
            this.comboArtworkType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "artworktypeid", true));
            this.comboArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboArtworkType.FormattingEnabled = true;
            this.comboArtworkType.IsSupportUnselect = true;
            this.comboArtworkType.Location = new System.Drawing.Point(126, 48);
            this.comboArtworkType.Name = "comboArtworkType";
            this.comboArtworkType.Size = new System.Drawing.Size(159, 24);
            this.comboArtworkType.TabIndex = 1;
            this.comboArtworkType.SelectedIndexChanged += new System.EventHandler(this.ComboArtworkType_SelectedIndexChanged);
            // 
            // numCapacity
            // 
            this.numCapacity.BackColor = System.Drawing.Color.White;
            this.numCapacity.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "capacity", true));
            this.numCapacity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numCapacity.Location = new System.Drawing.Point(126, 112);
            this.numCapacity.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numCapacity.MaxLength = 7;
            this.numCapacity.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCapacity.Name = "numCapacity";
            this.numCapacity.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCapacity.Size = new System.Drawing.Size(100, 23);
            this.numCapacity.TabIndex = 3;
            this.numCapacity.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numHeads
            // 
            this.numHeads.BackColor = System.Drawing.Color.White;
            this.numHeads.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "heads", true));
            this.numHeads.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numHeads.Location = new System.Drawing.Point(126, 144);
            this.numHeads.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numHeads.MaxLength = 2;
            this.numHeads.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numHeads.Name = "numHeads";
            this.numHeads.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numHeads.Size = new System.Drawing.Size(100, 23);
            this.numHeads.TabIndex = 4;
            this.numHeads.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radiobyMonth);
            this.radioPanel1.Controls.Add(this.radiobyDay);
            this.radioPanel1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "unit", true));
            this.radioPanel1.Location = new System.Drawing.Point(247, 112);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(229, 31);
            this.radioPanel1.TabIndex = 5;
            this.radioPanel1.Validated += new System.EventHandler(this.RadioPanel1_Validated);
            // 
            // radiobyMonth
            // 
            this.radiobyMonth.AutoSize = true;
            this.radiobyMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobyMonth.Location = new System.Drawing.Point(77, 3);
            this.radiobyMonth.Name = "radiobyMonth";
            this.radiobyMonth.Size = new System.Drawing.Size(84, 21);
            this.radiobyMonth.TabIndex = 1;
            this.radiobyMonth.TabStop = true;
            this.radiobyMonth.Text = "by month";
            this.radiobyMonth.UseVisualStyleBackColor = true;
            this.radiobyMonth.Value = "2";
            // 
            // radiobyDay
            // 
            this.radiobyDay.AutoSize = true;
            this.radiobyDay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobyDay.Location = new System.Drawing.Point(3, 2);
            this.radiobyDay.Name = "radiobyDay";
            this.radiobyDay.Size = new System.Drawing.Size(68, 21);
            this.radiobyDay.TabIndex = 0;
            this.radiobyDay.TabStop = true;
            this.radiobyDay.Text = "by day";
            this.radiobyDay.UseVisualStyleBackColor = true;
            this.radiobyDay.Value = "1";
            // 
            // label25
            // 
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Lines = 0;
            this.label25.Location = new System.Drawing.Point(313, 49);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(326, 23);
            this.label25.TabIndex = 5;
            this.label25.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // txtsubconFtySupp
            // 
            this.txtsubconFtySupp.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "ftysupp", true));
            this.txtsubconFtySupp.DisplayBox1Binding = "";
            this.txtsubconFtySupp.IsIncludeJunk = false;
            this.txtsubconFtySupp.Location = new System.Drawing.Point(126, 80);
            this.txtsubconFtySupp.Name = "txtsubconFtySupp";
            this.txtsubconFtySupp.Size = new System.Drawing.Size(159, 23);
            this.txtsubconFtySupp.TabIndex = 2;
            this.txtsubconFtySupp.TextBox1Binding = "";
            // 
            // B01
            // 
            this.ClientSize = new System.Drawing.Size(839, 457);
            this.DefaultControl = "dateDate";
            this.DefaultControlForEdit = "dateDate";
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
        private Win.UI.RadioButton radiobyMonth;
        private Win.UI.RadioButton radiobyDay;
        private Win.UI.NumericBox numHeads;
        private Win.UI.NumericBox numCapacity;
        private Class.TxtsubconNoConfirm txtsubconFtySupp;
        private Win.UI.ComboBox comboArtworkType;
        private Win.UI.DateBox dateDate;
        private Win.UI.Label labelHeads;
        private Win.UI.Label labelDate;
        private Win.UI.Label labelArtworkType;
        private Win.UI.Label labelFtySupp;
        private Win.UI.Label labelCapacity;
        private Win.UI.Label label25;
    }
}
