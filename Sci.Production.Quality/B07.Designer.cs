namespace Sci.Production.Quality
{
    partial class B07
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
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.radioOption1 = new Sci.Win.UI.RadioButton();
            this.radioOption2 = new Sci.Win.UI.RadioButton();
            this.txtFormula = new Sci.Win.UI.TextBox();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(650, 360);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.radioPanel1);
            this.detailcont.Controls.Add(this.pictureBox1);
            this.detailcont.Controls.Add(this.txtFormula);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.txtbrand);
            this.detailcont.Size = new System.Drawing.Size(650, 322);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 322);
            this.detailbtm.Size = new System.Drawing.Size(650, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(650, 360);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(658, 389);
            this.tabs.TabIndex = 0;
            // 
            // editby
            // 
            this.editby.Size = new System.Drawing.Size(227, 23);
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtbrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "brandid", true));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtbrand.IsSupportEditMode = false;
            this.txtbrand.Location = new System.Drawing.Point(173, 31);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.ReadOnly = true;
            this.txtbrand.Size = new System.Drawing.Size(119, 23);
            this.txtbrand.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(93, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Brand";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(93, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "Option";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(93, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 23);
            this.label3.TabIndex = 6;
            this.label3.Text = "Formula";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(93, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 23);
            this.label4.TabIndex = 7;
            this.label4.Text = "Example";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(298, 33);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 2;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
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
            this.radioOption1.CheckedChanged += new System.EventHandler(this.RadioOption1_CheckedChanged);
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
            this.txtFormula.Location = new System.Drawing.Point(173, 112);
            this.txtFormula.Name = "txtFormula";
            this.txtFormula.ReadOnly = true;
            this.txtFormula.Size = new System.Drawing.Size(246, 23);
            this.txtFormula.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(182, 153);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(157, 133);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioOption1);
            this.radioPanel1.Controls.Add(this.radioOption2);
            this.radioPanel1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.radioPanel1.Location = new System.Drawing.Point(173, 58);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.ReadOnly = true;
            this.radioPanel1.Size = new System.Drawing.Size(189, 48);
            this.radioPanel1.TabIndex = 1;
            // 
            // B07
            // 
            this.ClientSize = new System.Drawing.Size(658, 422);
            this.DefaultControl = "txtbrand";
            this.DefaultControlForEdit = "txtbrand";
            this.IsDeleteOnBrowse = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B07";
            this.Text = "B07.Skewness Option";
            this.WorkAlias = "SkewnessOption";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Class.Txtbrand txtbrand;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.RadioButton radioOption1;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.TextBox txtFormula;
        private Win.UI.RadioButton radioOption2;
        private Win.UI.RadioPanel radioPanel1;
    }
}
