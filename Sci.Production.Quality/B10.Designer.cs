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
            this.detail.Size = new System.Drawing.Size(712, 269);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.radioPanel1);
            this.detailcont.Controls.Add(this.txtFormula);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.txtbrand);
            this.detailcont.Size = new System.Drawing.Size(712, 231);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 231);
            this.detailbtm.Size = new System.Drawing.Size(712, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(712, 269);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(720, 298);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioOption1);
            this.radioPanel1.Controls.Add(this.radioOption2);
            this.radioPanel1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PointRateOption", true));
            this.radioPanel1.Location = new System.Drawing.Point(173, 70);
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
            this.txtFormula.Location = new System.Drawing.Point(173, 124);
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
            this.checkJunk.Location = new System.Drawing.Point(298, 45);
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
            this.label3.Size = new System.Drawing.Size(74, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Formula";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(93, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "Point Rate";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(93, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Brand";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtbrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "brandid", true));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtbrand.IsSupportEditMode = false;
            this.txtbrand.Location = new System.Drawing.Point(173, 43);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.ReadOnly = true;
            this.txtbrand.Size = new System.Drawing.Size(119, 23);
            this.txtbrand.TabIndex = 0;
            // 
            // B10
            // 
            this.ClientSize = new System.Drawing.Size(720, 331);
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
    }
}
