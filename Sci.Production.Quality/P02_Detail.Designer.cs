namespace Sci.Production.Quality
{
    partial class P02_Detail
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
            this.Encode = new Sci.Win.UI.Button();
            this.inspQty_text = new Sci.Win.UI.TextBox();
            this.unit_text = new Sci.Win.UI.TextBox();
            this.color_text = new Sci.Win.UI.TextBox();
            this.size_text = new Sci.Win.UI.TextBox();
            this.Arrive_qty_text = new Sci.Win.UI.TextBox();
            this.wkno_text = new Sci.Win.UI.TextBox();
            this.brand_text = new Sci.Win.UI.TextBox();
            this.ref_text = new Sci.Win.UI.TextBox();
            this.seq_text = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.lable_color = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.label11 = new Sci.Win.UI.Label();
            this.RejQty_text = new Sci.Win.UI.TextBox();
            this.label8 = new Sci.Win.UI.Label();
            this.label12 = new Sci.Win.UI.Label();
            this.Remark_text = new Sci.Win.UI.TextBox();
            this.label13 = new Sci.Win.UI.Label();
            this.Instor_text = new Sci.Win.UI.TextBox();
            this.label14 = new Sci.Win.UI.Label();
            this.label15 = new Sci.Win.UI.Label();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.textID = new Sci.Win.UI.TextBox();
            this.editBox1 = new Sci.Win.UI.EditBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.InsDate_text = new Sci.Win.UI.DateBox();
            this.txtsupplier1 = new Sci.Production.Class.txtsupplier();
            this.btnClose = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.btnClose);
            this.btmcont.Location = new System.Drawing.Point(0, 416);
            this.btmcont.Size = new System.Drawing.Size(669, 40);
            this.btmcont.Controls.SetChildIndex(this.left, 0);
            this.btmcont.Controls.SetChildIndex(this.right, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.btnClose, 0);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(579, 5);
            this.undo.Click += new System.EventHandler(this.undo_Click);
            // 
            // save
            // 
            this.save.Enabled = true;
            this.save.Location = new System.Drawing.Point(499, 5);
            this.save.Text = "Edit";
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // left
            // 
            this.left.Enabled = true;
            this.left.Visible = false;
            // 
            // right
            // 
            this.right.Enabled = true;
            this.right.Visible = false;
            // 
            // Encode
            // 
            this.Encode.Location = new System.Drawing.Point(499, 30);
            this.Encode.Name = "Encode";
            this.Encode.Size = new System.Drawing.Size(91, 30);
            this.Encode.TabIndex = 117;
            this.Encode.Text = "Amend";
            this.Encode.UseVisualStyleBackColor = true;
            this.Encode.Click += new System.EventHandler(this.Encode_Click);
            // 
            // inspQty_text
            // 
            this.inspQty_text.BackColor = System.Drawing.Color.White;
            this.inspQty_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.inspQty_text.Location = new System.Drawing.Point(112, 188);
            this.inspQty_text.Name = "inspQty_text";
            this.inspQty_text.Size = new System.Drawing.Size(124, 23);
            this.inspQty_text.TabIndex = 116;
            // 
            // unit_text
            // 
            this.unit_text.BackColor = System.Drawing.Color.White;
            this.unit_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.unit_text.Location = new System.Drawing.Point(95, 125);
            this.unit_text.Name = "unit_text";
            this.unit_text.Size = new System.Drawing.Size(144, 23);
            this.unit_text.TabIndex = 115;
            // 
            // color_text
            // 
            this.color_text.BackColor = System.Drawing.Color.White;
            this.color_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.color_text.Location = new System.Drawing.Point(328, 125);
            this.color_text.Name = "color_text";
            this.color_text.Size = new System.Drawing.Size(125, 23);
            this.color_text.TabIndex = 114;
            // 
            // size_text
            // 
            this.size_text.BackColor = System.Drawing.Color.White;
            this.size_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.size_text.Location = new System.Drawing.Point(534, 125);
            this.size_text.Name = "size_text";
            this.size_text.Size = new System.Drawing.Size(119, 23);
            this.size_text.TabIndex = 113;
            // 
            // Arrive_qty_text
            // 
            this.Arrive_qty_text.BackColor = System.Drawing.Color.White;
            this.Arrive_qty_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Arrive_qty_text.Location = new System.Drawing.Point(95, 94);
            this.Arrive_qty_text.Name = "Arrive_qty_text";
            this.Arrive_qty_text.Size = new System.Drawing.Size(144, 23);
            this.Arrive_qty_text.TabIndex = 112;
            // 
            // wkno_text
            // 
            this.wkno_text.BackColor = System.Drawing.Color.White;
            this.wkno_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.wkno_text.Location = new System.Drawing.Point(95, 60);
            this.wkno_text.Name = "wkno_text";
            this.wkno_text.Size = new System.Drawing.Size(144, 23);
            this.wkno_text.TabIndex = 109;
            // 
            // brand_text
            // 
            this.brand_text.BackColor = System.Drawing.Color.White;
            this.brand_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.brand_text.Location = new System.Drawing.Point(328, 60);
            this.brand_text.Name = "brand_text";
            this.brand_text.Size = new System.Drawing.Size(147, 23);
            this.brand_text.TabIndex = 108;
            // 
            // ref_text
            // 
            this.ref_text.BackColor = System.Drawing.Color.White;
            this.ref_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ref_text.Location = new System.Drawing.Point(328, 30);
            this.ref_text.Name = "ref_text";
            this.ref_text.Size = new System.Drawing.Size(147, 23);
            this.ref_text.TabIndex = 107;
            // 
            // seq_text
            // 
            this.seq_text.BackColor = System.Drawing.Color.White;
            this.seq_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.seq_text.Location = new System.Drawing.Point(95, 30);
            this.seq_text.Name = "seq_text";
            this.seq_text.Size = new System.Drawing.Size(144, 23);
            this.seq_text.TabIndex = 106;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(15, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 95;
            this.label1.Text = "SEQ#";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(15, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 96;
            this.label2.Text = "WKNO";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(248, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 97;
            this.label3.Text = "SCI Refno";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(247, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 98;
            this.label4.Text = "Refno";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(248, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 99;
            this.label5.Text = "Supplier";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(15, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 23);
            this.label6.TabIndex = 100;
            this.label6.Text = "Arrive Qty";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(456, 125);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 23);
            this.label7.TabIndex = 101;
            this.label7.Text = "Size";
            // 
            // lable_color
            // 
            this.lable_color.Lines = 0;
            this.lable_color.Location = new System.Drawing.Point(248, 125);
            this.lable_color.Name = "lable_color";
            this.lable_color.Size = new System.Drawing.Size(75, 23);
            this.lable_color.TabIndex = 102;
            this.lable_color.Text = "Color";
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(15, 125);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 23);
            this.label9.TabIndex = 103;
            this.label9.Text = "Unit";
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(236, 32);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(91, 23);
            this.label10.TabIndex = 104;
            this.label10.Text = "Defect";
            // 
            // label11
            // 
            this.label11.Lines = 0;
            this.label11.Location = new System.Drawing.Point(18, 188);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(91, 23);
            this.label11.TabIndex = 105;
            this.label11.Text = "Inspected Qty";
            // 
            // RejQty_text
            // 
            this.RejQty_text.BackColor = System.Drawing.Color.White;
            this.RejQty_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.RejQty_text.Location = new System.Drawing.Point(113, 223);
            this.RejQty_text.Name = "RejQty_text";
            this.RejQty_text.Size = new System.Drawing.Size(121, 23);
            this.RejQty_text.TabIndex = 120;
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(18, 223);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 23);
            this.label8.TabIndex = 119;
            this.label8.Text = "Rejected Qty";
            // 
            // label12
            // 
            this.label12.Lines = 0;
            this.label12.Location = new System.Drawing.Point(18, 257);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 23);
            this.label12.TabIndex = 121;
            this.label12.Text = "Inspect Date";
            // 
            // Remark_text
            // 
            this.Remark_text.BackColor = System.Drawing.Color.White;
            this.Remark_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Remark_text.Location = new System.Drawing.Point(340, 160);
            this.Remark_text.Name = "Remark_text";
            this.Remark_text.Size = new System.Drawing.Size(266, 23);
            this.Remark_text.TabIndex = 124;
            // 
            // label13
            // 
            this.label13.Lines = 0;
            this.label13.Location = new System.Drawing.Point(236, 160);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(91, 23);
            this.label13.TabIndex = 123;
            this.label13.Text = "Remark";
            // 
            // Instor_text
            // 
            this.Instor_text.BackColor = System.Drawing.Color.White;
            this.Instor_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Instor_text.Location = new System.Drawing.Point(115, 288);
            this.Instor_text.Name = "Instor_text";
            this.Instor_text.Size = new System.Drawing.Size(121, 23);
            this.Instor_text.TabIndex = 126;
            // 
            // label14
            // 
            this.label14.Lines = 0;
            this.label14.Location = new System.Drawing.Point(17, 288);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(91, 23);
            this.label14.TabIndex = 125;
            this.label14.Text = "Inspector";
            // 
            // label15
            // 
            this.label15.Lines = 0;
            this.label15.Location = new System.Drawing.Point(18, 318);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(91, 23);
            this.label15.TabIndex = 127;
            this.label15.Text = "Result";
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "result", true));
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(114, 316);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 129;
            // 
            // textID
            // 
            this.textID.BackColor = System.Drawing.Color.White;
            this.textID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textID.Location = new System.Drawing.Point(573, 80);
            this.textID.Name = "textID";
            this.textID.Size = new System.Drawing.Size(80, 23);
            this.textID.TabIndex = 130;
            this.textID.Visible = false;
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.White;
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox1.Location = new System.Drawing.Point(340, 32);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(266, 105);
            this.editBox1.TabIndex = 132;
            this.editBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.editBox1_MouseDown);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.panel1.Controls.Add(this.InsDate_text);
            this.panel1.Controls.Add(this.editBox1);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.Remark_text);
            this.panel1.Location = new System.Drawing.Point(10, 156);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(639, 234);
            this.panel1.TabIndex = 133;
            // 
            // InsDate_text
            // 
            this.InsDate_text.Location = new System.Drawing.Point(103, 101);
            this.InsDate_text.Name = "InsDate_text";
            this.InsDate_text.Size = new System.Drawing.Size(145, 23);
            this.InsDate_text.TabIndex = 135;
            // 
            // txtsupplier1
            // 
            this.txtsupplier1.DisplayBox1Binding = "";
            this.txtsupplier1.Location = new System.Drawing.Point(328, 94);
            this.txtsupplier1.Name = "txtsupplier1";
            this.txtsupplier1.Size = new System.Drawing.Size(214, 23);
            this.txtsupplier1.TabIndex = 134;
            this.txtsupplier1.TextBox1Binding = "";
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(350, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 135;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // P02_Detail
            // 
            this.ClientSize = new System.Drawing.Size(669, 456);
            this.Controls.Add(this.txtsupplier1);
            this.Controls.Add(this.textID);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.Instor_text);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.RejQty_text);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Encode);
            this.Controls.Add(this.inspQty_text);
            this.Controls.Add(this.unit_text);
            this.Controls.Add(this.color_text);
            this.Controls.Add(this.size_text);
            this.Controls.Add(this.Arrive_qty_text);
            this.Controls.Add(this.wkno_text);
            this.Controls.Add(this.brand_text);
            this.Controls.Add(this.ref_text);
            this.Controls.Add(this.seq_text);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lable_color);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.panel1);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "defect", true));
            this.Name = "P02_Detail";
            this.Text = "Accessory Inspection- SP+SEQ+Detail";
            this.WorkAlias = "AIR";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.lable_color, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.seq_text, 0);
            this.Controls.SetChildIndex(this.ref_text, 0);
            this.Controls.SetChildIndex(this.brand_text, 0);
            this.Controls.SetChildIndex(this.wkno_text, 0);
            this.Controls.SetChildIndex(this.Arrive_qty_text, 0);
            this.Controls.SetChildIndex(this.size_text, 0);
            this.Controls.SetChildIndex(this.color_text, 0);
            this.Controls.SetChildIndex(this.unit_text, 0);
            this.Controls.SetChildIndex(this.inspQty_text, 0);
            this.Controls.SetChildIndex(this.Encode, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.RejQty_text, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.label14, 0);
            this.Controls.SetChildIndex(this.Instor_text, 0);
            this.Controls.SetChildIndex(this.label15, 0);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            this.Controls.SetChildIndex(this.textID, 0);
            this.Controls.SetChildIndex(this.txtsupplier1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button Encode;
        private Win.UI.TextBox inspQty_text;
        private Win.UI.TextBox unit_text;
        private Win.UI.TextBox color_text;
        private Win.UI.TextBox size_text;
        private Win.UI.TextBox Arrive_qty_text;
        private Win.UI.TextBox wkno_text;
        private Win.UI.TextBox brand_text;
        private Win.UI.TextBox ref_text;
        private Win.UI.TextBox seq_text;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.Label lable_color;
        private Win.UI.Label label9;
        private Win.UI.Label label10;
        private Win.UI.Label label11;
        private Win.UI.TextBox RejQty_text;
        private Win.UI.Label label8;
        private Win.UI.Label label12;
        private Win.UI.TextBox Remark_text;
        private Win.UI.Label label13;
        private Win.UI.TextBox Instor_text;
        private Win.UI.Label label14;
        private Win.UI.Label label15;
        private Win.UI.ComboBox comboBox1;
        private Win.UI.TextBox textID;
        private Win.UI.EditBox editBox1;
        private Win.UI.Panel panel1;
        private Class.txtsupplier txtsupplier1;
        private Win.UI.DateBox InsDate_text;
        private Win.UI.Button btnClose;

    }
}
