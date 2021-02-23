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
            this.btnMoistureStandardList = new Sci.Win.UI.Button();
            this.radioPanel2 = new Sci.Win.UI.RadioPanel();
            this.radioForWetDry = new Sci.Win.UI.RadioButton();
            this.radioForWeftWarp = new Sci.Win.UI.RadioButton();
            this.label4 = new Sci.Win.UI.Label();
            this.radioPanel3 = new Sci.Win.UI.RadioPanel();
            this.SkewnessOption3 = new Sci.Win.UI.RadioButton();
            this.SkewnessOption1 = new Sci.Win.UI.RadioButton();
            this.SkewnessOption2 = new Sci.Win.UI.RadioButton();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.txtSkewnessFormula = new Sci.Win.UI.TextBox();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.radioPanel1.SuspendLayout();
            this.radioPanel2.SuspendLayout();
            this.radioPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(712, 464);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.radioPanel3);
            this.detailcont.Controls.Add(this.radioPanel2);
            this.detailcont.Controls.Add(this.radioPanel1);
            this.detailcont.Controls.Add(this.pictureBox1);
            this.detailcont.Controls.Add(this.txtSkewnessFormula);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label7);
            this.detailcont.Controls.Add(this.btnMoistureStandardList);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.txtFormula);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.txtbrand);
            this.detailcont.Size = new System.Drawing.Size(712, 426);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 426);
            this.detailbtm.Size = new System.Drawing.Size(712, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(712, 464);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(720, 493);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioOption1);
            this.radioPanel1.Controls.Add(this.radioOption2);
            this.radioPanel1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PointRateOption", true));
            this.radioPanel1.Location = new System.Drawing.Point(164, 63);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.ReadOnly = true;
            this.radioPanel1.Size = new System.Drawing.Size(189, 28);
            this.radioPanel1.TabIndex = 8;
            this.radioPanel1.ValueChanged += new System.EventHandler(this.RadioPanel1_ValueChanged);
            // 
            // radioOption1
            // 
            this.radioOption1.AutoCheck = false;
            this.radioOption1.AutoSize = true;
            this.radioOption1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.radioOption1.Location = new System.Drawing.Point(9, 3);
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
            this.radioOption2.Location = new System.Drawing.Point(91, 3);
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
            this.txtFormula.Location = new System.Drawing.Point(164, 97);
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
            this.checkJunk.Location = new System.Drawing.Point(289, 37);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 2;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(30, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Point Rate Formula";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(30, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "Point Rate";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(30, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Brand";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtbrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "brandid", true));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtbrand.IsSupportEditMode = false;
            this.txtbrand.Location = new System.Drawing.Point(164, 35);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.ReadOnly = true;
            this.txtbrand.Size = new System.Drawing.Size(119, 23);
            this.txtbrand.TabIndex = 0;
            // 
            // btnMoistureStandardList
            // 
            this.btnMoistureStandardList.Location = new System.Drawing.Point(510, 35);
            this.btnMoistureStandardList.Name = "btnMoistureStandardList";
            this.btnMoistureStandardList.Size = new System.Drawing.Size(194, 30);
            this.btnMoistureStandardList.TabIndex = 9;
            this.btnMoistureStandardList.Text = "Moisture Standard List";
            this.btnMoistureStandardList.UseVisualStyleBackColor = true;
            this.btnMoistureStandardList.Click += new System.EventHandler(this.BtnMoistureStandardList_Click);
            // 
            // radioPanel2
            // 
            this.radioPanel2.Controls.Add(this.radioForWetDry);
            this.radioPanel2.Controls.Add(this.radioForWeftWarp);
            this.radioPanel2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CrockingTestOption", true));
            this.radioPanel2.Location = new System.Drawing.Point(164, 127);
            this.radioPanel2.Name = "radioPanel2";
            this.radioPanel2.ReadOnly = true;
            this.radioPanel2.Size = new System.Drawing.Size(373, 56);
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
            this.label4.Location = new System.Drawing.Point(30, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 23);
            this.label4.TabIndex = 9;
            this.label4.Text = "Crocking Test";
            // 
            // radioPanel3
            // 
            this.radioPanel3.Controls.Add(this.SkewnessOption3);
            this.radioPanel3.Controls.Add(this.SkewnessOption1);
            this.radioPanel3.Controls.Add(this.SkewnessOption2);
            this.radioPanel3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SkewnessOption", true));
            this.radioPanel3.Location = new System.Drawing.Point(164, 185);
            this.radioPanel3.Name = "radioPanel3";
            this.radioPanel3.ReadOnly = true;
            this.radioPanel3.Size = new System.Drawing.Size(279, 28);
            this.radioPanel3.TabIndex = 11;
            this.radioPanel3.ValueChanged += new System.EventHandler(this.RadioPanel3_ValueChanged);
            // 
            // SkewnessOption3
            // 
            this.SkewnessOption3.AutoCheck = false;
            this.SkewnessOption3.AutoSize = true;
            this.SkewnessOption3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.SkewnessOption3.Location = new System.Drawing.Point(173, 2);
            this.SkewnessOption3.Name = "SkewnessOption3";
            this.SkewnessOption3.Size = new System.Drawing.Size(76, 21);
            this.SkewnessOption3.TabIndex = 2;
            this.SkewnessOption3.Text = "Option3";
            this.SkewnessOption3.UseVisualStyleBackColor = true;
            this.SkewnessOption3.Value = "3";
            // 
            // SkewnessOption1
            // 
            this.SkewnessOption1.AutoCheck = false;
            this.SkewnessOption1.AutoSize = true;
            this.SkewnessOption1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.SkewnessOption1.Location = new System.Drawing.Point(9, 2);
            this.SkewnessOption1.Name = "SkewnessOption1";
            this.SkewnessOption1.Size = new System.Drawing.Size(76, 21);
            this.SkewnessOption1.TabIndex = 0;
            this.SkewnessOption1.Text = "Option1";
            this.SkewnessOption1.UseVisualStyleBackColor = true;
            this.SkewnessOption1.Value = "1";
            // 
            // SkewnessOption2
            // 
            this.SkewnessOption2.AutoCheck = false;
            this.SkewnessOption2.AutoSize = true;
            this.SkewnessOption2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.SkewnessOption2.Location = new System.Drawing.Point(91, 2);
            this.SkewnessOption2.Name = "SkewnessOption2";
            this.SkewnessOption2.Size = new System.Drawing.Size(76, 21);
            this.SkewnessOption2.TabIndex = 1;
            this.SkewnessOption2.Text = "Option2";
            this.SkewnessOption2.UseVisualStyleBackColor = true;
            this.SkewnessOption2.Value = "2";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = null;
            this.pictureBox1.Location = new System.Drawing.Point(164, 251);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(157, 133);
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            // 
            // txtSkewnessFormula
            // 
            this.txtSkewnessFormula.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSkewnessFormula.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSkewnessFormula.IsSupportEditMode = false;
            this.txtSkewnessFormula.Location = new System.Drawing.Point(164, 219);
            this.txtSkewnessFormula.Name = "txtSkewnessFormula";
            this.txtSkewnessFormula.ReadOnly = true;
            this.txtSkewnessFormula.Size = new System.Drawing.Size(246, 23);
            this.txtSkewnessFormula.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(30, 251);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 23);
            this.label5.TabIndex = 15;
            this.label5.Text = "Skewness  Example";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(30, 219);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 23);
            this.label6.TabIndex = 14;
            this.label6.Text = "Skewness Formula";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(30, 186);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 23);
            this.label7.TabIndex = 13;
            this.label7.Text = "Skewness Option";
            // 
            // B10
            // 
            this.ClientSize = new System.Drawing.Size(720, 526);
            this.DefaultControl = "txtbrand";
            this.DefaultControlForEdit = "txtbrand";
            this.IsDeleteOnBrowse = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B10";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B10. QA Brand Setting";
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
            this.radioPanel3.ResumeLayout(false);
            this.radioPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private Win.UI.Button btnMoistureStandardList;
        private Win.UI.RadioPanel radioPanel2;
        private Win.UI.RadioButton radioForWetDry;
        private Win.UI.RadioButton radioForWeftWarp;
        private Win.UI.Label label4;
        private Win.UI.RadioPanel radioPanel3;
        private Win.UI.RadioButton SkewnessOption1;
        private Win.UI.RadioButton SkewnessOption2;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.TextBox txtSkewnessFormula;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.RadioButton SkewnessOption3;
    }
}
