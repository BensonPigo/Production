namespace Sci.Production.Quality
{
    partial class B10
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioOption1 = new Sci.Win.UI.RadioButton();
            this.radioOption2 = new Sci.Win.UI.RadioButton();
            this.txtFormula = new Sci.Win.UI.TextBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.radioPanel2 = new Sci.Win.UI.RadioPanel();
            this.radioForWetDry = new Sci.Win.UI.RadioButton();
            this.radioForWeftWarp = new Sci.Win.UI.RadioButton();
            this.label4 = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.radioPanel1.SuspendLayout();
            this.radioPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(712, 285);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.radioPanel2);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.radioPanel1);
            this.detailcont.Controls.Add(this.txtFormula);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.txtbrand);
            this.detailcont.Size = new System.Drawing.Size(712, 247);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 247);
            this.detailbtm.Size = new System.Drawing.Size(712, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(712, 269);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(720, 314);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioOption1);
            this.radioPanel1.Controls.Add(this.radioOption2);
            this.radioPanel1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PointRateOption", true));
            this.radioPanel1.Location = new System.Drawing.Point(185, 69);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.ReadOnly = true;
            this.radioPanel1.Size = new System.Drawing.Size(189, 48);
            this.radioPanel1.TabIndex = 8;
            this.radioPanel1.ValueChanged += new System.EventHandler(this.RadioPanel1_ValueChanged);
            // 
            // radioOption1
            // 
            this.radioOption1.AutoCheck = false;
            this.radioOption1.AutoSize = true;
            this.radioOption1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.radioOption1.Location = new System.Drawing.Point(9, 13);
            this.radioOption1.Name = "radioOption1";
            this.radioOption1.Size = new System.Drawing.Size(76, 21);
            this.radioOption1.TabIndex = 0;
            this.radioOption1.Text = "Option1";
            this.radioOption1.UseVisualStyleBackColor = true;
            this.radioOption1.Value = "1";
            // 
            // radioOption2
            // 
            this.radioOption2.AutoCheck = false;
            this.radioOption2.AutoSize = true;
            this.radioOption2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.radioOption2.Location = new System.Drawing.Point(91, 13);
            this.radioOption2.Name = "radioOption2";
            this.radioOption2.Size = new System.Drawing.Size(76, 21);
            this.radioOption2.TabIndex = 1;
            this.radioOption2.Text = "Option2";
            this.radioOption2.UseVisualStyleBackColor = true;
            this.radioOption2.Value = "2";
            // 
            // txtFormula
            // 
            this.txtFormula.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtFormula.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtFormula.IsSupportEditMode = false;
            this.txtFormula.Location = new System.Drawing.Point(185, 123);
            this.txtFormula.Name = "txtFormula";
            this.txtFormula.ReadOnly = true;
            this.txtFormula.Size = new System.Drawing.Size(355, 23);
            this.txtFormula.TabIndex = 1;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(310, 44);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 2;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(93, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Formula";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(93, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "Point Rate";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(93, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Brand";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtbrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "brandid", true));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtbrand.IsSupportEditMode = false;
            this.txtbrand.Location = new System.Drawing.Point(185, 42);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.ReadOnly = true;
            this.txtbrand.Size = new System.Drawing.Size(119, 23);
            this.txtbrand.TabIndex = 0;
            // 
            // radioPanel2
            // 
            this.radioPanel2.Controls.Add(this.radioForWetDry);
            this.radioPanel2.Controls.Add(this.radioForWeftWarp);
            this.radioPanel2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CrockingTestOption", true));
            this.radioPanel2.Location = new System.Drawing.Point(185, 166);
            this.radioPanel2.Name = "radioPanel2";
            this.radioPanel2.ReadOnly = true;
            this.radioPanel2.Size = new System.Drawing.Size(373, 66);
            this.radioPanel2.TabIndex = 10;
            // 
            // radioForWetDry
            // 
            this.radioForWetDry.AutoCheck = false;
            this.radioForWetDry.AutoSize = true;
            this.radioForWetDry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.radioForWetDry.Location = new System.Drawing.Point(9, 3);
            this.radioForWetDry.Name = "radioForWetDry";
            this.radioForWetDry.Size = new System.Drawing.Size(171, 21);
            this.radioForWetDry.TabIndex = 0;
            this.radioForWetDry.Text = "Only 1 for Wet and Dry";
            this.radioForWetDry.UseVisualStyleBackColor = true;
            this.radioForWetDry.Value = "0";
            // 
            // radioForWeftWarp
            // 
            this.radioForWeftWarp.AutoCheck = false;
            this.radioForWeftWarp.AutoSize = true;
            this.radioForWeftWarp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.radioForWeftWarp.Location = new System.Drawing.Point(9, 30);
            this.radioForWeftWarp.Name = "radioForWeftWarp";
            this.radioForWeftWarp.Size = new System.Drawing.Size(356, 21);
            this.radioForWeftWarp.TabIndex = 1;
            this.radioForWeftWarp.Text = "2 kind (WEFT and WARP) of testing for Wet and Dry";
            this.radioForWeftWarp.UseVisualStyleBackColor = true;
            this.radioForWeftWarp.Value = "1";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(93, 166);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 23);
            this.label4.TabIndex = 9;
            this.label4.Text = "Crocking Test";
            // 
            // B10
            // 
            this.ClientSize = new System.Drawing.Size(720, 347);
            this.DefaultControl = "txtbrand";
            this.DefaultControlForEdit = "txtbrand";
            this.IsDeleteOnBrowse = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B10";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B10. Brand";
            this.WorkAlias = "QABrandSetting";
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
            this.radioPanel2.ResumeLayout(false);
            this.radioPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioOption1;
        private Win.UI.RadioButton radioOption2;
        private Win.UI.TextBox txtFormula;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Class.Txtbrand txtbrand;
        private Win.UI.RadioPanel radioPanel2;
        private Win.UI.RadioButton radioForWetDry;
        private Win.UI.RadioButton radioForWeftWarp;
        private Win.UI.Label label4;
    }
}
