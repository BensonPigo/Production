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
            this.chkP10 = new Sci.Win.UI.CheckBox();
            this.chkP11 = new Sci.Win.UI.CheckBox();
            this.chkP12 = new Sci.Win.UI.CheckBox();
            this.chkP13 = new Sci.Win.UI.CheckBox();
            this.txtID = new Sci.Production.Class.Txtuser();
            this.chkGarmentTest = new Sci.Win.UI.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSignature)).BeginInit();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(735, 303);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.chkGarmentTest);
            this.detailcont.Controls.Add(this.chkP13);
            this.detailcont.Controls.Add(this.chkP12);
            this.detailcont.Controls.Add(this.chkP11);
            this.detailcont.Controls.Add(this.chkP10);
            this.detailcont.Controls.Add(this.chkJunk);
            this.detailcont.Controls.Add(this.pictureBoxSignature);
            this.detailcont.Controls.Add(this.btnDelete);
            this.detailcont.Controls.Add(this.btnAttach);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(735, 265);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 265);
            this.detailbtm.Size = new System.Drawing.Size(735, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(735, 303);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(743, 332);
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
            this.btnAttach.Click += new System.EventHandler(this.btnAttach_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(237, 55);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 30);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
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
            // chkP10
            // 
            this.chkP10.AutoSize = true;
            this.chkP10.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SampleGarmentWash", true));
            this.chkP10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkP10.Location = new System.Drawing.Point(460, 108);
            this.chkP10.Name = "chkP10";
            this.chkP10.Size = new System.Drawing.Size(247, 21);
            this.chkP10.TabIndex = 10;
            this.chkP10.Text = "P10. Sample Order Garment Wash";
            this.chkP10.UseVisualStyleBackColor = true;
            // 
            // chkP11
            // 
            this.chkP11.AutoSize = true;
            this.chkP11.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MockupCrocking", true));
            this.chkP11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkP11.Location = new System.Drawing.Point(460, 135);
            this.chkP11.Name = "chkP11";
            this.chkP11.Size = new System.Drawing.Size(209, 21);
            this.chkP11.TabIndex = 11;
            this.chkP11.Text = "P11. Mockup - Crocking Test";
            this.chkP11.UseVisualStyleBackColor = true;
            // 
            // chkP12
            // 
            this.chkP12.AutoSize = true;
            this.chkP12.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MockupOven", true));
            this.chkP12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkP12.Location = new System.Drawing.Point(460, 162);
            this.chkP12.Name = "chkP12";
            this.chkP12.Size = new System.Drawing.Size(188, 21);
            this.chkP12.TabIndex = 12;
            this.chkP12.Text = "P12. Mockup - Oven Test";
            this.chkP12.UseVisualStyleBackColor = true;
            // 
            // chkP13
            // 
            this.chkP13.AutoSize = true;
            this.chkP13.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MockupWash", true));
            this.chkP13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkP13.Location = new System.Drawing.Point(460, 189);
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
            // chkGarmentTest
            // 
            this.chkGarmentTest.AutoSize = true;
            this.chkGarmentTest.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "GarmentTest", true));
            this.chkGarmentTest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkGarmentTest.Location = new System.Drawing.Point(460, 81);
            this.chkGarmentTest.Name = "chkGarmentTest";
            this.chkGarmentTest.Size = new System.Drawing.Size(221, 21);
            this.chkGarmentTest.TabIndex = 14;
            this.chkGarmentTest.Text = "P04. Laboratory-Garment Test";
            this.chkGarmentTest.UseVisualStyleBackColor = true;
            // 
            // B09
            // 
            this.ClientSize = new System.Drawing.Size(743, 365);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label1;
        private Win.UI.CheckBox chkP13;
        private Win.UI.CheckBox chkP12;
        private Win.UI.CheckBox chkP11;
        private Win.UI.CheckBox chkP10;
        private Win.UI.CheckBox chkJunk;
        private Win.UI.PictureBox pictureBoxSignature;
        private Win.UI.Button btnDelete;
        private Win.UI.Button btnAttach;
        private Class.Txtuser txtID;
        private Win.UI.CheckBox chkGarmentTest;
    }
}
