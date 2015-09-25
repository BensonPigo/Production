namespace Sci.Production.Shipping
{
    partial class P02_EditFromPO
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
            this.editBox2 = new Sci.Win.UI.EditBox();
            this.displayBox2 = new Sci.Win.UI.DisplayBox();
            this.label13 = new Sci.Win.UI.Label();
            this.textBox3 = new Sci.Win.UI.TextBox();
            this.label11 = new Sci.Win.UI.Label();
            this.numericBox3 = new Sci.Win.UI.NumericBox();
            this.label10 = new Sci.Win.UI.Label();
            this.numericBox2 = new Sci.Win.UI.NumericBox();
            this.textBox2 = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.numericBox1 = new Sci.Win.UI.NumericBox();
            this.editBox1 = new Sci.Win.UI.EditBox();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.label8 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.displayBox3 = new Sci.Win.UI.DisplayBox();
            this.displayBox4 = new Sci.Win.UI.DisplayBox();
            this.label12 = new Sci.Win.UI.Label();
            this.txtsupplier1 = new Sci.Production.Class.txtsupplier();
            this.txttpeuser1 = new Sci.Production.Class.txttpeuser();
            this.txtunit_fty1 = new Sci.Production.Class.txtunit_fty();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 319);
            this.btmcont.Size = new System.Drawing.Size(689, 40);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(605, 7);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(519, 7);
            // 
            // editBox2
            // 
            this.editBox2.BackColor = System.Drawing.Color.White;
            this.editBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox2.Location = new System.Drawing.Point(100, 257);
            this.editBox2.Multiline = true;
            this.editBox2.Name = "editBox2";
            this.editBox2.Size = new System.Drawing.Size(572, 56);
            this.editBox2.TabIndex = 137;
            // 
            // displayBox2
            // 
            this.displayBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "BrandID", true));
            this.displayBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox2.Location = new System.Drawing.Point(487, 229);
            this.displayBox2.Name = "displayBox2";
            this.displayBox2.Size = new System.Drawing.Size(100, 23);
            this.displayBox2.TabIndex = 153;
            // 
            // label13
            // 
            this.label13.Lines = 0;
            this.label13.Location = new System.Drawing.Point(413, 229);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 23);
            this.label13.TabIndex = 152;
            this.label13.Text = "Brand";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.White;
            this.textBox3.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Receiver", true));
            this.textBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox3.Location = new System.Drawing.Point(487, 176);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(185, 23);
            this.textBox3.TabIndex = 134;
            // 
            // label11
            // 
            this.label11.Lines = 0;
            this.label11.Location = new System.Drawing.Point(413, 176);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 23);
            this.label11.TabIndex = 149;
            this.label11.Text = "Receiver";
            // 
            // numericBox3
            // 
            this.numericBox3.BackColor = System.Drawing.Color.White;
            this.numericBox3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NW", true));
            this.numericBox3.DecimalPlaces = 2;
            this.numericBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox3.Location = new System.Drawing.Point(100, 203);
            this.numericBox3.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
            this.numericBox3.Name = "numericBox3";
            this.numericBox3.Size = new System.Drawing.Size(65, 23);
            this.numericBox3.TabIndex = 133;
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(413, 149);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 23);
            this.label10.TabIndex = 148;
            this.label10.Text = "Unit";
            // 
            // numericBox2
            // 
            this.numericBox2.BackColor = System.Drawing.Color.White;
            this.numericBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Qty", true));
            this.numericBox2.DecimalPlaces = 2;
            this.numericBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox2.Location = new System.Drawing.Point(100, 176);
            this.numericBox2.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            131072});
            this.numericBox2.Name = "numericBox2";
            this.numericBox2.Size = new System.Drawing.Size(74, 23);
            this.numericBox2.TabIndex = 131;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CTNNo", true));
            this.textBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox2.Location = new System.Drawing.Point(487, 122);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(90, 23);
            this.textBox2.TabIndex = 130;
            this.textBox2.Validated += new System.EventHandler(this.textBox2_Validated);
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(413, 122);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 23);
            this.label9.TabIndex = 147;
            this.label9.Text = "CTN No.";
            // 
            // numericBox1
            // 
            this.numericBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numericBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Price", true));
            this.numericBox1.DecimalPlaces = 4;
            this.numericBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numericBox1.IsSupportEditMode = false;
            this.numericBox1.Location = new System.Drawing.Point(100, 149);
            this.numericBox1.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            262144});
            this.numericBox1.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.ReadOnly = true;
            this.numericBox1.Size = new System.Drawing.Size(90, 23);
            this.numericBox1.TabIndex = 129;
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MtlDesc", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editBox1.IsSupportEditMode = false;
            this.editBox1.Location = new System.Drawing.Point(100, 36);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.ReadOnly = true;
            this.editBox1.Size = new System.Drawing.Size(572, 82);
            this.editBox1.TabIndex = 128;
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Seq1", true));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(225, 9);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(30, 23);
            this.displayBox1.TabIndex = 146;
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(9, 257);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 23);
            this.label8.TabIndex = 145;
            this.label8.Text = "Remark";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(9, 230);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 23);
            this.label7.TabIndex = 144;
            this.label7.Text = "Team Leader";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(413, 203);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 23);
            this.label6.TabIndex = 143;
            this.label6.Text = "Category";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(9, 203);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 23);
            this.label5.TabIndex = 142;
            this.label5.Text = "N.W. (kg)";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(9, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 23);
            this.label4.TabIndex = 141;
            this.label4.Text = "Q\'ty";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(9, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 23);
            this.label3.TabIndex = 140;
            this.label3.Text = "Price";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(9, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 23);
            this.label2.TabIndex = 139;
            this.label2.Text = "Description";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 23);
            this.label1.TabIndex = 138;
            this.label1.Text = "SP#";
            // 
            // displayBox3
            // 
            this.displayBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "OrderID", true));
            this.displayBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox3.Location = new System.Drawing.Point(100, 9);
            this.displayBox3.Name = "displayBox3";
            this.displayBox3.Size = new System.Drawing.Size(120, 23);
            this.displayBox3.TabIndex = 154;
            // 
            // displayBox4
            // 
            this.displayBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox4.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Seq2", true));
            this.displayBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox4.Location = new System.Drawing.Point(257, 9);
            this.displayBox4.Name = "displayBox4";
            this.displayBox4.Size = new System.Drawing.Size(25, 23);
            this.displayBox4.TabIndex = 155;
            // 
            // label12
            // 
            this.label12.Lines = 0;
            this.label12.Location = new System.Drawing.Point(9, 122);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(87, 23);
            this.label12.TabIndex = 157;
            this.label12.Text = "Supp ID";
            // 
            // txtsupplier1
            // 
            this.txtsupplier1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "SuppID", true));
            this.txtsupplier1.DisplayBox1Binding = "";
            this.txtsupplier1.Location = new System.Drawing.Point(100, 122);
            this.txtsupplier1.Name = "txtsupplier1";
            this.txtsupplier1.Size = new System.Drawing.Size(147, 23);
            this.txtsupplier1.TabIndex = 158;
            this.txtsupplier1.TextBox1Binding = "";
            // 
            // txttpeuser1
            // 
            this.txttpeuser1.DataBindings.Add(new System.Windows.Forms.Binding("DisplayBox1Binding", this.mtbs, "Leader", true));
            this.txttpeuser1.DisplayBox1Binding = "";
            this.txttpeuser1.DisplayBox2Binding = "";
            this.txttpeuser1.Location = new System.Drawing.Point(100, 230);
            this.txttpeuser1.Name = "txttpeuser1";
            this.txttpeuser1.Size = new System.Drawing.Size(302, 23);
            this.txttpeuser1.TabIndex = 151;
            // 
            // txtunit_fty1
            // 
            this.txtunit_fty1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtunit_fty1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "UnitID", true));
            this.txtunit_fty1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtunit_fty1.IsSupportEditMode = false;
            this.txtunit_fty1.Location = new System.Drawing.Point(487, 149);
            this.txtunit_fty1.Name = "txtunit_fty1";
            this.txtunit_fty1.ReadOnly = true;
            this.txtunit_fty1.Size = new System.Drawing.Size(66, 23);
            this.txtunit_fty1.TabIndex = 132;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Category", true));
            this.comboBox1.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(487, 202);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.ReadOnly = true;
            this.comboBox1.Size = new System.Drawing.Size(90, 24);
            this.comboBox1.TabIndex = 159;
            // 
            // P02_EditFromPO
            // 
            this.ClientSize = new System.Drawing.Size(689, 359);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.txtsupplier1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.displayBox4);
            this.Controls.Add(this.displayBox3);
            this.Controls.Add(this.editBox2);
            this.Controls.Add(this.displayBox2);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txttpeuser1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.numericBox3);
            this.Controls.Add(this.txtunit_fty1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.numericBox2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.numericBox1);
            this.Controls.Add(this.editBox1);
            this.Controls.Add(this.displayBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "P02_EditFromPO";
            this.Text = "International Air/Express - Import From Purchase";
            this.WorkAlias = "Express_Detail";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.displayBox1, 0);
            this.Controls.SetChildIndex(this.editBox1, 0);
            this.Controls.SetChildIndex(this.numericBox1, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.textBox2, 0);
            this.Controls.SetChildIndex(this.numericBox2, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.txtunit_fty1, 0);
            this.Controls.SetChildIndex(this.numericBox3, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.textBox3, 0);
            this.Controls.SetChildIndex(this.txttpeuser1, 0);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.displayBox2, 0);
            this.Controls.SetChildIndex(this.editBox2, 0);
            this.Controls.SetChildIndex(this.displayBox3, 0);
            this.Controls.SetChildIndex(this.displayBox4, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.txtsupplier1, 0);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.EditBox editBox2;
        private Win.UI.DisplayBox displayBox2;
        private Win.UI.Label label13;
        private Class.txttpeuser txttpeuser1;
        private Win.UI.TextBox textBox3;
        private Win.UI.Label label11;
        private Win.UI.NumericBox numericBox3;
        private Class.txtunit_fty txtunit_fty1;
        private Win.UI.Label label10;
        private Win.UI.NumericBox numericBox2;
        private Win.UI.TextBox textBox2;
        private Win.UI.Label label9;
        private Win.UI.NumericBox numericBox1;
        private Win.UI.EditBox editBox1;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.Label label8;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox displayBox3;
        private Win.UI.DisplayBox displayBox4;
        private Win.UI.Label label12;
        private Class.txtsupplier txtsupplier1;
        private Win.UI.ComboBox comboBox1;
    }
}
