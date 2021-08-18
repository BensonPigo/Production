namespace Sci.Production.Quality
{
    partial class B09
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
            this.label1 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.btnAttach = new Sci.Win.UI.Button();
            this.btnDelete = new Sci.Win.UI.Button();
            this.pictureBoxSignature = new Sci.Win.UI.PictureBox();
            this.chkJunk = new Sci.Win.UI.CheckBox();
            this.chkSampleGarment = new Sci.Win.UI.CheckBox();
            this.chkSampleCrocking = new Sci.Win.UI.CheckBox();
            this.chkSampleOven = new Sci.Win.UI.CheckBox();
            this.chkP13 = new Sci.Win.UI.CheckBox();
            this.txtID = new Sci.Production.Class.Txtuser();
            this.chkBulkGarmentTest = new Sci.Win.UI.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkSampleWash = new Sci.Win.UI.CheckBox();
            this.chkBulkWash = new Sci.Win.UI.CheckBox();
            this.chkBulkCrocking = new Sci.Win.UI.CheckBox();
            this.chkBulkOven = new Sci.Win.UI.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSignature)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(735, 408);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.groupBox2);
            this.detailcont.Controls.Add(this.groupBox1);
            this.detailcont.Controls.Add(this.chkJunk);
            this.detailcont.Controls.Add(this.pictureBoxSignature);
            this.detailcont.Controls.Add(this.btnDelete);
            this.detailcont.Controls.Add(this.btnAttach);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(735, 370);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 370);
            this.detailbtm.Size = new System.Drawing.Size(735, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(735, 303);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(743, 437);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(30, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Technician ID";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(30, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Signature";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(460, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(209, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Authorized Certified Technician";
            // 
            // btnAttach
            // 
            this.btnAttach.Location = new System.Drawing.Point(151, 55);
            this.btnAttach.Name = "btnAttach";
            this.btnAttach.Size = new System.Drawing.Size(80, 30);
            this.btnAttach.TabIndex = 6;
            this.btnAttach.Text = "Attach";
            this.btnAttach.UseVisualStyleBackColor = true;
            this.btnAttach.Click += new System.EventHandler(this.BtnAttach_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(237, 55);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 30);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // pictureBoxSignature
            // 
            this.pictureBoxSignature.Image = null;
            this.pictureBoxSignature.Location = new System.Drawing.Point(151, 92);
            this.pictureBoxSignature.Name = "pictureBoxSignature";
            this.pictureBoxSignature.Size = new System.Drawing.Size(267, 93);
            this.pictureBoxSignature.TabIndex = 8;
            this.pictureBoxSignature.TabStop = false;
            this.pictureBoxSignature.WaitOnLoad = true;
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkJunk.Location = new System.Drawing.Point(460, 22);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(57, 21);
            this.chkJunk.TabIndex = 9;
            this.chkJunk.Text = "Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // chkSampleGarment
            // 
            this.chkSampleGarment.AutoSize = true;
            this.chkSampleGarment.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SampleGarmentWash", true));
            this.chkSampleGarment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSampleGarment.Location = new System.Drawing.Point(10, 22);
            this.chkSampleGarment.Name = "chkSampleGarment";
            this.chkSampleGarment.Size = new System.Drawing.Size(114, 21);
            this.chkSampleGarment.TabIndex = 10;
            this.chkSampleGarment.Text = "Garment Test";
            this.chkSampleGarment.UseVisualStyleBackColor = true;
            // 
            // chkSampleCrocking
            // 
            this.chkSampleCrocking.AutoSize = true;
            this.chkSampleCrocking.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MockupCrocking", true));
            this.chkSampleCrocking.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSampleCrocking.Location = new System.Drawing.Point(10, 49);
            this.chkSampleCrocking.Name = "chkSampleCrocking";
            this.chkSampleCrocking.Size = new System.Drawing.Size(167, 21);
            this.chkSampleCrocking.TabIndex = 11;
            this.chkSampleCrocking.Text = "Mockup Crocking Test";
            this.chkSampleCrocking.UseVisualStyleBackColor = true;
            // 
            // chkSampleOven
            // 
            this.chkSampleOven.AutoSize = true;
            this.chkSampleOven.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MockupOven", true));
            this.chkSampleOven.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSampleOven.Location = new System.Drawing.Point(10, 76);
            this.chkSampleOven.Name = "chkSampleOven";
            this.chkSampleOven.Size = new System.Drawing.Size(146, 21);
            this.chkSampleOven.TabIndex = 12;
            this.chkSampleOven.Text = "Mockup Oven Test";
            this.chkSampleOven.UseVisualStyleBackColor = true;
            // 
            // chkP13
            // 
            this.chkP13.AutoSize = true;
            this.chkP13.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MockupWash", true));
            this.chkP13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkP13.Location = new System.Drawing.Point(6, 130);
            this.chkP13.Name = "chkP13";
            this.chkP13.Size = new System.Drawing.Size(190, 21);
            this.chkP13.TabIndex = 13;
            this.chkP13.Text = "P13. Mockup - Wash Test";
            this.chkP13.UseVisualStyleBackColor = true;
            // 
            // txtID
            // 
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "ID", true));
            this.txtID.DisplayBox1Binding = "";
            this.txtID.Location = new System.Drawing.Point(151, 22);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(300, 23);
            this.txtID.TabIndex = 5;
            this.txtID.TextBox1Binding = "";
            // 
            // chkBulkGarmentTest
            // 
            this.chkBulkGarmentTest.AutoSize = true;
            this.chkBulkGarmentTest.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "GarmentTest", true));
            this.chkBulkGarmentTest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkBulkGarmentTest.Location = new System.Drawing.Point(6, 22);
            this.chkBulkGarmentTest.Name = "chkBulkGarmentTest";
            this.chkBulkGarmentTest.Size = new System.Drawing.Size(114, 21);
            this.chkBulkGarmentTest.TabIndex = 14;
            this.chkBulkGarmentTest.Text = "Garment Test";
            this.chkBulkGarmentTest.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkBulkWash);
            this.groupBox1.Controls.Add(this.chkBulkCrocking);
            this.groupBox1.Controls.Add(this.chkBulkOven);
            this.groupBox1.Controls.Add(this.chkBulkGarmentTest);
            this.groupBox1.Controls.Add(this.chkP13);
            this.groupBox1.Location = new System.Drawing.Point(460, 92);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(209, 127);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bulk Stage";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkSampleWash);
            this.groupBox2.Controls.Add(this.chkSampleGarment);
            this.groupBox2.Controls.Add(this.chkSampleCrocking);
            this.groupBox2.Controls.Add(this.chkSampleOven);
            this.groupBox2.Location = new System.Drawing.Point(460, 225);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(209, 129);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sample Stage";
            // 
            // chkSampleWash
            // 
            this.chkSampleWash.AutoSize = true;
            this.chkSampleWash.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MockupWash", true));
            this.chkSampleWash.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSampleWash.Location = new System.Drawing.Point(10, 103);
            this.chkSampleWash.Name = "chkSampleWash";
            this.chkSampleWash.Size = new System.Drawing.Size(148, 21);
            this.chkSampleWash.TabIndex = 13;
            this.chkSampleWash.Text = "Mockup Wash Test";
            this.chkSampleWash.UseVisualStyleBackColor = true;
            // 
            // chkBulkWash
            // 
            this.chkBulkWash.AutoSize = true;
            this.chkBulkWash.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "BulkMockWash", true));
            this.chkBulkWash.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkBulkWash.Location = new System.Drawing.Point(6, 103);
            this.chkBulkWash.Name = "chkBulkWash";
            this.chkBulkWash.Size = new System.Drawing.Size(148, 21);
            this.chkBulkWash.TabIndex = 17;
            this.chkBulkWash.Text = "Mockup Wash Test";
            this.chkBulkWash.UseVisualStyleBackColor = true;
            // 
            // chkBulkCrocking
            // 
            this.chkBulkCrocking.AutoSize = true;
            this.chkBulkCrocking.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "BulkMockupCrocking", true));
            this.chkBulkCrocking.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkBulkCrocking.Location = new System.Drawing.Point(6, 49);
            this.chkBulkCrocking.Name = "chkBulkCrocking";
            this.chkBulkCrocking.Size = new System.Drawing.Size(167, 21);
            this.chkBulkCrocking.TabIndex = 15;
            this.chkBulkCrocking.Text = "Mockup Crocking Test";
            this.chkBulkCrocking.UseVisualStyleBackColor = true;
            // 
            // chkBulkOven
            // 
            this.chkBulkOven.AutoSize = true;
            this.chkBulkOven.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "BulkMockupOven", true));
            this.chkBulkOven.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkBulkOven.Location = new System.Drawing.Point(6, 76);
            this.chkBulkOven.Name = "chkBulkOven";
            this.chkBulkOven.Size = new System.Drawing.Size(146, 21);
            this.chkBulkOven.TabIndex = 16;
            this.chkBulkOven.Text = "Mockup Oven Test";
            this.chkBulkOven.UseVisualStyleBackColor = true;
            // 
            // B09
            // 
            this.ClientSize = new System.Drawing.Size(743, 470);
            this.Name = "B09";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B09. Technician List";
            this.WorkAlias = "Technician";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSignature)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label1;
        private Win.UI.CheckBox chkP13;
        private Win.UI.CheckBox chkSampleOven;
        private Win.UI.CheckBox chkSampleCrocking;
        private Win.UI.CheckBox chkSampleGarment;
        private Win.UI.CheckBox chkJunk;
        private Win.UI.PictureBox pictureBoxSignature;
        private Win.UI.Button btnDelete;
        private Win.UI.Button btnAttach;
        private Class.Txtuser txtID;
        private Win.UI.CheckBox chkBulkGarmentTest;
        private System.Windows.Forms.GroupBox groupBox2;
        private Win.UI.CheckBox chkSampleWash;
        private System.Windows.Forms.GroupBox groupBox1;
        private Win.UI.CheckBox chkBulkWash;
        private Win.UI.CheckBox chkBulkCrocking;
        private Win.UI.CheckBox chkBulkOven;
    }
}
