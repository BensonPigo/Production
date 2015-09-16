namespace Sci.Production.Miscellaneous
{
    partial class B03
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
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.textBox2 = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.txtmiscbrand1 = new Sci.Machine.Class.txtmiscbrand();
            this.txtsubcon1 = new Sci.Production.Class.txtsubcon();
            this.txtmmsunit1 = new Sci.Machine.Class.txtmmsunit();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.editBox1 = new Sci.Win.UI.EditBox();
            this.checkBox2 = new Sci.Win.UI.CheckBox();
            this.checkBox3 = new Sci.Win.UI.CheckBox();
            this.checkBox4 = new Sci.Win.UI.CheckBox();
            this.textBox3 = new Sci.Win.UI.TextBox();
            this.label11 = new Sci.Win.UI.Label();
            this.numericBox2 = new Sci.Win.UI.NumericBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.label27 = new Sci.Win.UI.Label();
            this.button9 = new Sci.Win.UI.Button();
            this.button10 = new Sci.Win.UI.Button();
            this.numericBox1 = new Sci.Win.UI.NumericBox();
            this.label10 = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(795, 371);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.numericBox2);
            this.detailcont.Controls.Add(this.label11);
            this.detailcont.Controls.Add(this.numericBox1);
            this.detailcont.Controls.Add(this.label10);
            this.detailcont.Controls.Add(this.textBox3);
            this.detailcont.Controls.Add(this.checkBox4);
            this.detailcont.Controls.Add(this.checkBox3);
            this.detailcont.Controls.Add(this.checkBox2);
            this.detailcont.Controls.Add(this.editBox1);
            this.detailcont.Controls.Add(this.displayBox1);
            this.detailcont.Controls.Add(this.txtmmsunit1);
            this.detailcont.Controls.Add(this.txtmiscbrand1);
            this.detailcont.Controls.Add(this.txtsubcon1);
            this.detailcont.Controls.Add(this.label9);
            this.detailcont.Controls.Add(this.label8);
            this.detailcont.Controls.Add(this.label7);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.textBox2);
            this.detailcont.Controls.Add(this.textBox1);
            this.detailcont.Controls.Add(this.checkBox1);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Size = new System.Drawing.Size(795, 333);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 333);
            this.detailbtm.Size = new System.Drawing.Size(795, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(795, 371);
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabPage1);
            this.tabs.Size = new System.Drawing.Size(803, 400);
            this.tabs.Controls.SetChildIndex(this.tabPage1, 0);
            this.tabs.Controls.SetChildIndex(this.detail, 0);
            this.tabs.Controls.SetChildIndex(this.browse, 0);
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(39, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "Misc ID";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(39, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "Model";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "junk", true));
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkBox1.IsSupportEditMode = false;
            this.checkBox1.Location = new System.Drawing.Point(340, 23);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.ReadOnly = true;
            this.checkBox1.Size = new System.Drawing.Size(57, 21);
            this.checkBox1.TabIndex = 15;
            this.checkBox1.Text = "Junk";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.textBox1.IsSupportEditMode = false;
            this.textBox1.Location = new System.Drawing.Point(117, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(168, 23);
            this.textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "model", true));
            this.textBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.textBox2.IsSupportEditMode = false;
            this.textBox2.Location = new System.Drawing.Point(117, 61);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(330, 23);
            this.textBox2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(39, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "Brand";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(39, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 9;
            this.label5.Text = "Supplier";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(39, 250);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 23);
            this.label6.TabIndex = 10;
            this.label6.Text = "Purchase Unit";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(39, 174);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 23);
            this.label7.TabIndex = 11;
            this.label7.Text = "Currency";
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(307, 175);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 23);
            this.label8.TabIndex = 12;
            this.label8.Text = "Description";
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(307, 289);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 23);
            this.label9.TabIndex = 13;
            this.label9.Text = "Remark";
            // 
            // txtmiscbrand1
            // 
            this.txtmiscbrand1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "miscbrandid", true));
            // 
            // 
            // 
            this.txtmiscbrand1.DisplayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtmiscbrand1.DisplayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtmiscbrand1.DisplayBox1.Location = new System.Drawing.Point(97, 3);
            this.txtmiscbrand1.DisplayBox1.Name = "displayBox1";
            this.txtmiscbrand1.DisplayBox1.Size = new System.Drawing.Size(191, 23);
            this.txtmiscbrand1.DisplayBox1.TabIndex = 1;
            this.txtmiscbrand1.Location = new System.Drawing.Point(112, 94);
            this.txtmiscbrand1.Name = "txtmiscbrand1";
            this.txtmiscbrand1.Size = new System.Drawing.Size(298, 28);
            this.txtmiscbrand1.TabIndex = 3;
            // 
            // 
            // 
            this.txtmiscbrand1.TextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtmiscbrand1.TextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtmiscbrand1.TextBox1.IsSupportEditMode = false;
            this.txtmiscbrand1.TextBox1.Location = new System.Drawing.Point(3, 3);
            this.txtmiscbrand1.TextBox1.Name = "textBox1";
            this.txtmiscbrand1.TextBox1.ReadOnly = true;
            this.txtmiscbrand1.TextBox1.Size = new System.Drawing.Size(90, 23);
            this.txtmiscbrand1.TextBox1.TabIndex = 0;
            // 
            // txtsubcon1
            // 
            this.txtsubcon1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "suppid", true));
            this.txtsubcon1.DisplayBox1Binding = "";
            this.txtsubcon1.IsIncludeJunk = false;
            this.txtsubcon1.Location = new System.Drawing.Point(117, 135);
            this.txtsubcon1.Name = "txtsubcon1";
            this.txtsubcon1.Size = new System.Drawing.Size(170, 23);
            this.txtsubcon1.TabIndex = 4;
            this.txtsubcon1.TextBox1Binding = "";
            // 
            // txtmmsunit1
            // 
            this.txtmmsunit1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtmmsunit1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "unitid", true));
            this.txtmmsunit1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtmmsunit1.IsSupportEditMode = false;
            this.txtmmsunit1.Location = new System.Drawing.Point(138, 250);
            this.txtmmsunit1.Name = "txtmmsunit1";
            this.txtmmsunit1.ReadOnly = true;
            this.txtmmsunit1.Size = new System.Drawing.Size(78, 23);
            this.txtmmsunit1.TabIndex = 7;
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "currencyid", true));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(117, 174);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(48, 23);
            this.displayBox1.TabIndex = 5;
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editBox1.IsSupportEditMode = false;
            this.editBox1.Location = new System.Drawing.Point(385, 175);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.ReadOnly = true;
            this.editBox1.Size = new System.Drawing.Size(391, 91);
            this.editBox1.TabIndex = 10;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "Inspect", true));
            this.checkBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox2.Location = new System.Drawing.Point(543, 23);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(126, 21);
            this.checkBox2.TabIndex = 12;
            this.checkBox2.Text = "Need to Inspect";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "ismachine", true));
            this.checkBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkBox3.IsSupportEditMode = false;
            this.checkBox3.Location = new System.Drawing.Point(543, 50);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.ReadOnly = true;
            this.checkBox3.Size = new System.Drawing.Size(90, 21);
            this.checkBox3.TabIndex = 13;
            this.checkBox3.Text = "IsMachine";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "isasset", true));
            this.checkBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkBox4.IsSupportEditMode = false;
            this.checkBox4.Location = new System.Drawing.Point(543, 77);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.ReadOnly = true;
            this.checkBox4.Size = new System.Drawing.Size(72, 21);
            this.checkBox4.TabIndex = 14;
            this.checkBox4.Text = "IsAsset";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.textBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.textBox3.IsSupportEditMode = false;
            this.textBox3.Location = new System.Drawing.Point(385, 289);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(391, 23);
            this.textBox3.TabIndex = 11;
            // 
            // label11
            // 
            this.label11.Lines = 0;
            this.label11.Location = new System.Drawing.Point(39, 212);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 23);
            this.label11.TabIndex = 26;
            this.label11.Text = "Price";
            // 
            // numericBox2
            // 
            this.numericBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numericBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "price", true));
            this.numericBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numericBox2.IsSupportEditMode = false;
            this.numericBox2.Location = new System.Drawing.Point(117, 212);
            this.numericBox2.Name = "numericBox2";
            this.numericBox2.ReadOnly = true;
            this.numericBox2.Size = new System.Drawing.Size(84, 23);
            this.numericBox2.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.label27);
            this.tabPage1.Controls.Add(this.button9);
            this.tabPage1.Controls.Add(this.button10);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(897, 395);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Picture";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(6, 44);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(824, 319);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // label27
            // 
            this.label27.Lines = 0;
            this.label27.Location = new System.Drawing.Point(8, 15);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(75, 23);
            this.label27.TabIndex = 18;
            this.label27.Text = "Picture1";
            // 
            // button9
            // 
            this.button9.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.button9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button9.Location = new System.Drawing.Point(184, 8);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(80, 30);
            this.button9.TabIndex = 17;
            this.button9.Text = "Delete";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button10.Location = new System.Drawing.Point(98, 8);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(80, 30);
            this.button10.TabIndex = 16;
            this.button10.Text = "Change";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // numericBox1
            // 
            this.numericBox1.BackColor = System.Drawing.Color.White;
            this.numericBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox1.Location = new System.Drawing.Point(162, 289);
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.Size = new System.Drawing.Size(49, 23);
            this.numericBox1.TabIndex = 8;
            this.numericBox1.Validated += new System.EventHandler(this.numericBox1_Validated);
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(39, 289);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(120, 23);
            this.label10.TabIndex = 24;
            this.label10.Text = "Inspect Lead Time";
            // 
            // B03
            // 
            this.ClientSize = new System.Drawing.Size(803, 433);
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B03";
            this.Text = "B02.Miscellaneous Data Maintain(Local)";
            this.WorkAlias = "Misc";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.CheckBox checkBox1;
        private Win.UI.TextBox textBox2;
        private Win.UI.TextBox textBox1;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label2;
        private Win.UI.Label label9;
        private Win.UI.Label label8;
        private Class.txtsubcon txtsubcon1;
        private Machine.Class.txtmiscbrand txtmiscbrand1;
        private Win.UI.CheckBox checkBox4;
        private Win.UI.CheckBox checkBox3;
        private Win.UI.CheckBox checkBox2;
        private Win.UI.EditBox editBox1;
        private Win.UI.DisplayBox displayBox1;
        private Machine.Class.txtmmsunit txtmmsunit1;
        private Win.UI.NumericBox numericBox2;
        private Win.UI.Label label11;
        private Win.UI.TextBox textBox3;
        private System.Windows.Forms.TabPage tabPage1;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.Label label27;
        private Win.UI.Button button9;
        private Win.UI.Button button10;
        private Win.UI.NumericBox numericBox1;
        private Win.UI.Label label10;
    }
}
