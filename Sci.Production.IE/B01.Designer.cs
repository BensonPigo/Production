namespace Sci.Production.IE
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
            this.labelCode = new Sci.Win.UI.Label();
            this.labelActivities = new Sci.Win.UI.Label();
            this.labelDaysbefore = new Sci.Win.UI.Label();
            this.labelBaseOn = new Sci.Win.UI.Label();
            this.labelNewRepeatAll = new Sci.Win.UI.Label();
            this.txtCode = new Sci.Win.UI.TextBox();
            this.txtActivities = new Sci.Win.UI.TextBox();
            this.txtDaysbefore = new Sci.Win.UI.TextBox();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioSCIDelivery = new Sci.Win.UI.RadioButton();
            this.radioChangeOver = new Sci.Win.UI.RadioButton();
            this.comboNewRepeatAll = new Sci.Win.UI.ComboBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.labelBrand = new Sci.Win.UI.Label();
            this.txtBrand = new Sci.Win.UI.TextBox();
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
            this.detail.Size = new System.Drawing.Size(832, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtBrand);
            this.detailcont.Controls.Add(this.labelBrand);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.comboNewRepeatAll);
            this.detailcont.Controls.Add(this.radioPanel1);
            this.detailcont.Controls.Add(this.txtDaysbefore);
            this.detailcont.Controls.Add(this.txtActivities);
            this.detailcont.Controls.Add(this.txtCode);
            this.detailcont.Controls.Add(this.labelNewRepeatAll);
            this.detailcont.Controls.Add(this.labelBaseOn);
            this.detailcont.Controls.Add(this.labelDaysbefore);
            this.detailcont.Controls.Add(this.labelActivities);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Size = new System.Drawing.Size(832, 357);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(832, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(832, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(840, 424);
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
            // labelCode
            // 
            this.labelCode.Lines = 0;
            this.labelCode.Location = new System.Drawing.Point(42, 28);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(103, 23);
            this.labelCode.TabIndex = 0;
            this.labelCode.Text = "Code";
            // 
            // labelActivities
            // 
            this.labelActivities.Lines = 0;
            this.labelActivities.Location = new System.Drawing.Point(42, 68);
            this.labelActivities.Name = "labelActivities";
            this.labelActivities.Size = new System.Drawing.Size(103, 23);
            this.labelActivities.TabIndex = 1;
            this.labelActivities.Text = "Activities";
            // 
            // labelDaysbefore
            // 
            this.labelDaysbefore.Lines = 0;
            this.labelDaysbefore.Location = new System.Drawing.Point(42, 108);
            this.labelDaysbefore.Name = "labelDaysbefore";
            this.labelDaysbefore.Size = new System.Drawing.Size(103, 23);
            this.labelDaysbefore.TabIndex = 2;
            this.labelDaysbefore.Text = "Days before";
            // 
            // labelBaseOn
            // 
            this.labelBaseOn.Lines = 0;
            this.labelBaseOn.Location = new System.Drawing.Point(222, 108);
            this.labelBaseOn.Name = "labelBaseOn";
            this.labelBaseOn.Size = new System.Drawing.Size(58, 23);
            this.labelBaseOn.TabIndex = 3;
            this.labelBaseOn.Text = "Base On";
            // 
            // labelNewRepeatAll
            // 
            this.labelNewRepeatAll.Lines = 0;
            this.labelNewRepeatAll.Location = new System.Drawing.Point(42, 148);
            this.labelNewRepeatAll.Name = "labelNewRepeatAll";
            this.labelNewRepeatAll.Size = new System.Drawing.Size(103, 23);
            this.labelNewRepeatAll.TabIndex = 4;
            this.labelNewRepeatAll.Text = "New/Repeat/All";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Code", true));
            this.txtCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCode.Location = new System.Drawing.Point(149, 28);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(50, 23);
            this.txtCode.TabIndex = 0;
            // 
            // txtActivities
            // 
            this.txtActivities.BackColor = System.Drawing.Color.White;
            this.txtActivities.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtActivities.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.txtActivities.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtActivities.Location = new System.Drawing.Point(149, 68);
            this.txtActivities.Name = "txtActivities";
            this.txtActivities.Size = new System.Drawing.Size(404, 23);
            this.txtActivities.TabIndex = 1;
            // 
            // txtDaysbefore
            // 
            this.txtDaysbefore.BackColor = System.Drawing.Color.White;
            this.txtDaysbefore.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DaysBefore", true));
            this.txtDaysbefore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDaysbefore.Location = new System.Drawing.Point(149, 108);
            this.txtDaysbefore.Name = "txtDaysbefore";
            this.txtDaysbefore.Size = new System.Drawing.Size(38, 23);
            this.txtDaysbefore.TabIndex = 2;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioSCIDelivery);
            this.radioPanel1.Controls.Add(this.radioChangeOver);
            this.radioPanel1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "BaseOn", true));
            this.radioPanel1.Location = new System.Drawing.Point(283, 108);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(270, 23);
            this.radioPanel1.TabIndex = 8;
            // 
            // radioSCIDelivery
            // 
            this.radioSCIDelivery.AutoSize = true;
            this.radioSCIDelivery.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSCIDelivery.Location = new System.Drawing.Point(128, 1);
            this.radioSCIDelivery.Name = "radioSCIDelivery";
            this.radioSCIDelivery.Size = new System.Drawing.Size(102, 21);
            this.radioSCIDelivery.TabIndex = 1;
            this.radioSCIDelivery.TabStop = true;
            this.radioSCIDelivery.Text = "SCI Delivery";
            this.radioSCIDelivery.UseVisualStyleBackColor = true;
            this.radioSCIDelivery.Value = "2";
            // 
            // radioChangeOver
            // 
            this.radioChangeOver.AutoSize = true;
            this.radioChangeOver.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioChangeOver.Location = new System.Drawing.Point(3, 1);
            this.radioChangeOver.Name = "radioChangeOver";
            this.radioChangeOver.Size = new System.Drawing.Size(106, 21);
            this.radioChangeOver.TabIndex = 0;
            this.radioChangeOver.TabStop = true;
            this.radioChangeOver.Text = "ChangeOver";
            this.radioChangeOver.UseVisualStyleBackColor = true;
            this.radioChangeOver.Value = "1";
            // 
            // comboNewRepeatAll
            // 
            this.comboNewRepeatAll.BackColor = System.Drawing.Color.White;
            this.comboNewRepeatAll.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "UseFor", true));
            this.comboNewRepeatAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboNewRepeatAll.FormattingEnabled = true;
            this.comboNewRepeatAll.IsSupportUnselect = true;
            this.comboNewRepeatAll.Location = new System.Drawing.Point(149, 148);
            this.comboNewRepeatAll.Name = "comboNewRepeatAll";
            this.comboNewRepeatAll.Size = new System.Drawing.Size(92, 24);
            this.comboNewRepeatAll.TabIndex = 3;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(449, 30);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 5;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(238, 28);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(42, 23);
            this.labelBrand.TabIndex = 11;
            this.labelBrand.Text = "Brand";
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(283, 28);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(66, 23);
            this.txtBrand.TabIndex = 4;
            this.txtBrand.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtBrand_PopUp);
            this.txtBrand.Validating += new System.ComponentModel.CancelEventHandler(this.TxtBrand_Validating);
            // 
            // B01
            // 
            this.ClientSize = new System.Drawing.Size(840, 457);
            this.DefaultControl = "txtCode";
            this.DefaultControlForEdit = "txtActivities";
            this.DefaultOrder = "BrandID,Code";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B01";
            this.Text = "B01. Changeover Check List Activities index";
            this.UniqueExpress = "CODE,BRANDID";
            this.WorkAlias = "ChgOverCheckList";
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

        private Win.UI.CheckBox checkJunk;
        private Win.UI.ComboBox comboNewRepeatAll;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioSCIDelivery;
        private Win.UI.RadioButton radioChangeOver;
        private Win.UI.TextBox txtDaysbefore;
        private Win.UI.TextBox txtActivities;
        private Win.UI.TextBox txtCode;
        private Win.UI.Label labelNewRepeatAll;
        private Win.UI.Label labelBaseOn;
        private Win.UI.Label labelDaysbefore;
        private Win.UI.Label labelActivities;
        private Win.UI.Label labelCode;
        private Win.UI.Label labelBrand;
        private Win.UI.TextBox txtBrand;
    }
}
