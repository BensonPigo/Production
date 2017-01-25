namespace Sci.Production.Cutting
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.checkBox2 = new Sci.Win.UI.CheckBox();
            this.checkBox3 = new Sci.Win.UI.CheckBox();
            this.checkBox4 = new Sci.Win.UI.CheckBox();
            this.label3 = new Sci.Win.UI.Label();
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.textBox3 = new Sci.Win.UI.TextBox();
            this.textBox5 = new Sci.Win.UI.TextBox();
            this.label5 = new Sci.Win.UI.Label();
            this.txtartworktype_fty1 = new Sci.Production.Class.txtartworktype_fty();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(676, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtartworktype_fty1);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.textBox5);
            this.detailcont.Controls.Add(this.textBox3);
            this.detailcont.Controls.Add(this.textBox1);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.checkBox4);
            this.detailcont.Controls.Add(this.checkBox3);
            this.detailcont.Controls.Add(this.checkBox2);
            this.detailcont.Controls.Add(this.checkBox1);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(676, 357);
            this.detailcont.TabIndex = 0;
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
            // editby
            // 
            this.editby.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(70, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(70, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Show Seq";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "junk", true));
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(315, 59);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(57, 21);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Junk";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "IsRFIDDefault", true));
            this.checkBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox2.Location = new System.Drawing.Point(412, 112);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(135, 21);
            this.checkBox2.TabIndex = 7;
            this.checkBox2.Text = "Display on report";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "isSelection", true));
            this.checkBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox3.Location = new System.Drawing.Point(315, 112);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(82, 21);
            this.checkBox3.TabIndex = 6;
            this.checkBox3.Text = "Selected";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "IsRFIDProcess", true));
            this.checkBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox4.Location = new System.Drawing.Point(412, 57);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(78, 21);
            this.checkBox4.TabIndex = 5;
            this.checkBox4.Text = "Process";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(70, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "Artwork Type";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "id", true));
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.textBox1.IsSupportEditMode = false;
            this.textBox1.Location = new System.Drawing.Point(162, 55);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(90, 23);
            this.textBox1.TabIndex = 0;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.White;
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "showseq", true));
            this.textBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox3.Location = new System.Drawing.Point(162, 167);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(53, 23);
            this.textBox3.TabIndex = 2;
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.Color.White;
            this.textBox5.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "bcsdate", true));
            this.textBox5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox5.Location = new System.Drawing.Point(162, 221);
            this.textBox5.MaxLength = 2;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(66, 23);
            this.textBox5.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(70, 221);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 23);
            this.label5.TabIndex = 16;
            this.label5.Text = "BcsDate";
            // 
            // txtartworktype_fty1
            // 
            this.txtartworktype_fty1.BackColor = System.Drawing.Color.White;
            this.txtartworktype_fty1.cClassify = "";
            this.txtartworktype_fty1.cSubprocess = "t";
            this.txtartworktype_fty1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "artworktypeid", true));
            this.txtartworktype_fty1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_fty1.Location = new System.Drawing.Point(162, 112);
            this.txtartworktype_fty1.Name = "txtartworktype_fty1";
            this.txtartworktype_fty1.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_fty1.TabIndex = 1;
            // 
            // B01
            // 
            this.ClientSize = new System.Drawing.Size(684, 457);
            this.DefaultControl = "textBox1";
            this.DefaultControlForEdit = "txtartworktype_fty1";
            this.DefaultOrder = "id";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B01";
            this.Text = "B01.SubProcess Data";
            this.WorkAlias = "Subprocess";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label3;
        private Win.UI.CheckBox checkBox4;
        private Win.UI.CheckBox checkBox3;
        private Win.UI.CheckBox checkBox2;
        private Win.UI.CheckBox checkBox1;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.TextBox textBox3;
        private Win.UI.TextBox textBox1;
        private Win.UI.Label label5;
        private Win.UI.TextBox textBox5;
        private Class.txtartworktype_fty txtartworktype_fty1;
    }
}
