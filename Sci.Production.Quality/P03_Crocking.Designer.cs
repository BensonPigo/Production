namespace Sci.Production.Quality
{
    partial class P03_Crocking
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
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.label11 = new Sci.Win.UI.Label();
            this.label12 = new Sci.Win.UI.Label();
            this.label13 = new Sci.Win.UI.Label();
            this.sptext = new Sci.Win.UI.TextBox();
            this.Wknotext = new Sci.Win.UI.TextBox();
            this.Styletext = new Sci.Win.UI.TextBox();
            this.Brandtext = new Sci.Win.UI.TextBox();
            this.SEQtext = new Sci.Win.UI.TextBox();
            this.SRnotext = new Sci.Win.UI.TextBox();
            this.BRnotext = new Sci.Win.UI.TextBox();
            this.Colortext = new Sci.Win.UI.TextBox();
            this.AQtytext = new Sci.Win.UI.TextBox();
            this.ResultText = new Sci.Win.UI.TextBox();
            this.encode_button = new Sci.Win.UI.Button();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.ToExcelBtn = new Sci.Win.UI.Button();
            this.Arrdate = new Sci.Win.UI.DateBox();
            this.LIDate = new Sci.Win.UI.DateBox();
            this.Supptext = new Sci.Production.Class.txtsupplier();
            this.label14 = new Sci.Win.UI.Label();
            this.Description_box = new Sci.Win.UI.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.ToExcelBtn);
            this.btmcont.Location = new System.Drawing.Point(0, 545);
            this.btmcont.Size = new System.Drawing.Size(974, 40);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.ToExcelBtn, 0);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 127);
            this.gridcont.Size = new System.Drawing.Size(950, 408);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(884, 5);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(804, 5);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 23);
            this.label1.TabIndex = 98;
            this.label1.Text = "SP#";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(9, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 23);
            this.label2.TabIndex = 99;
            this.label2.Text = "Wkno";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(9, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 23);
            this.label3.TabIndex = 100;
            this.label3.Text = "Style#";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(9, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 23);
            this.label4.TabIndex = 101;
            this.label4.Text = "Brand";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(184, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 23);
            this.label5.TabIndex = 102;
            this.label5.Text = "SEQ#";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(184, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 23);
            this.label6.TabIndex = 103;
            this.label6.Text = "Supp";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(184, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 23);
            this.label7.TabIndex = 104;
            this.label7.Text = "SCI Refno";
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(184, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 25);
            this.label8.TabIndex = 105;
            this.label8.Text = "Brand Refno";
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(309, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 23);
            this.label9.TabIndex = 106;
            this.label9.Text = "Color";
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(472, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 23);
            this.label10.TabIndex = 107;
            this.label10.Text = "Arrive Qty";
            // 
            // label11
            // 
            this.label11.Lines = 0;
            this.label11.Location = new System.Drawing.Point(675, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(144, 23);
            this.label11.TabIndex = 108;
            this.label11.Text = "Arrive W/H Date";
            // 
            // label12
            // 
            this.label12.Lines = 0;
            this.label12.Location = new System.Drawing.Point(675, 38);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(144, 23);
            this.label12.TabIndex = 109;
            this.label12.Text = "Last Inspection Date ";
            // 
            // label13
            // 
            this.label13.Lines = 0;
            this.label13.Location = new System.Drawing.Point(472, 38);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(75, 23);
            this.label13.TabIndex = 110;
            this.label13.Text = "Result";
            // 
            // sptext
            // 
            this.sptext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.sptext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.sptext.IsSupportEditMode = false;
            this.sptext.Location = new System.Drawing.Point(58, 9);
            this.sptext.Name = "sptext";
            this.sptext.ReadOnly = true;
            this.sptext.Size = new System.Drawing.Size(115, 23);
            this.sptext.TabIndex = 111;
            // 
            // Wknotext
            // 
            this.Wknotext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Wknotext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Wknotext.IsSupportEditMode = false;
            this.Wknotext.Location = new System.Drawing.Point(58, 38);
            this.Wknotext.Name = "Wknotext";
            this.Wknotext.ReadOnly = true;
            this.Wknotext.Size = new System.Drawing.Size(115, 23);
            this.Wknotext.TabIndex = 112;
            // 
            // Styletext
            // 
            this.Styletext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Styletext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Styletext.IsSupportEditMode = false;
            this.Styletext.Location = new System.Drawing.Point(58, 67);
            this.Styletext.Name = "Styletext";
            this.Styletext.ReadOnly = true;
            this.Styletext.Size = new System.Drawing.Size(115, 23);
            this.Styletext.TabIndex = 113;
            // 
            // Brandtext
            // 
            this.Brandtext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Brandtext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Brandtext.IsSupportEditMode = false;
            this.Brandtext.Location = new System.Drawing.Point(58, 96);
            this.Brandtext.Name = "Brandtext";
            this.Brandtext.ReadOnly = true;
            this.Brandtext.Size = new System.Drawing.Size(115, 23);
            this.Brandtext.TabIndex = 114;
            // 
            // SEQtext
            // 
            this.SEQtext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.SEQtext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.SEQtext.IsSupportEditMode = false;
            this.SEQtext.Location = new System.Drawing.Point(238, 9);
            this.SEQtext.Name = "SEQtext";
            this.SEQtext.ReadOnly = true;
            this.SEQtext.Size = new System.Drawing.Size(60, 23);
            this.SEQtext.TabIndex = 115;
            // 
            // SRnotext
            // 
            this.SRnotext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.SRnotext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.SRnotext.IsSupportEditMode = false;
            this.SRnotext.Location = new System.Drawing.Point(273, 67);
            this.SRnotext.Name = "SRnotext";
            this.SRnotext.ReadOnly = true;
            this.SRnotext.Size = new System.Drawing.Size(188, 23);
            this.SRnotext.TabIndex = 117;
            // 
            // BRnotext
            // 
            this.BRnotext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.BRnotext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.BRnotext.IsSupportEditMode = false;
            this.BRnotext.Location = new System.Drawing.Point(274, 96);
            this.BRnotext.Name = "BRnotext";
            this.BRnotext.ReadOnly = true;
            this.BRnotext.Size = new System.Drawing.Size(187, 23);
            this.BRnotext.TabIndex = 118;
            // 
            // Colortext
            // 
            this.Colortext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Colortext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Colortext.IsSupportEditMode = false;
            this.Colortext.Location = new System.Drawing.Point(359, 9);
            this.Colortext.Name = "Colortext";
            this.Colortext.ReadOnly = true;
            this.Colortext.Size = new System.Drawing.Size(102, 23);
            this.Colortext.TabIndex = 119;
            // 
            // AQtytext
            // 
            this.AQtytext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.AQtytext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.AQtytext.IsSupportEditMode = false;
            this.AQtytext.Location = new System.Drawing.Point(549, 9);
            this.AQtytext.Name = "AQtytext";
            this.AQtytext.ReadOnly = true;
            this.AQtytext.Size = new System.Drawing.Size(111, 23);
            this.AQtytext.TabIndex = 120;
            // 
            // ResultText
            // 
            this.ResultText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.ResultText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.ResultText.IsSupportEditMode = false;
            this.ResultText.Location = new System.Drawing.Point(550, 38);
            this.ResultText.Name = "ResultText";
            this.ResultText.ReadOnly = true;
            this.ResultText.Size = new System.Drawing.Size(110, 23);
            this.ResultText.TabIndex = 123;
            // 
            // encode_button
            // 
            this.encode_button.Location = new System.Drawing.Point(883, 89);
            this.encode_button.Name = "encode_button";
            this.encode_button.Size = new System.Drawing.Size(80, 30);
            this.encode_button.TabIndex = 124;
            this.encode_button.Text = "Encode";
            this.encode_button.UseVisualStyleBackColor = true;
            this.encode_button.Click += new System.EventHandler(this.encode_button_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkBox1.IsSupportEditMode = false;
            this.checkBox1.Location = new System.Drawing.Point(913, 67);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.ReadOnly = true;
            this.checkBox1.Size = new System.Drawing.Size(50, 21);
            this.checkBox1.TabIndex = 153;
            this.checkBox1.Text = "N/A";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // ToExcelBtn
            // 
            this.ToExcelBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.ToExcelBtn.Location = new System.Drawing.Point(718, 5);
            this.ToExcelBtn.Name = "ToExcelBtn";
            this.ToExcelBtn.Size = new System.Drawing.Size(80, 30);
            this.ToExcelBtn.TabIndex = 95;
            this.ToExcelBtn.Text = "To Excel";
            this.ToExcelBtn.UseVisualStyleBackColor = true;
            this.ToExcelBtn.Click += new System.EventHandler(this.ToExcelBtn_Click);
            // 
            // Arrdate
            // 
            this.Arrdate.IsSupportEditMode = false;
            this.Arrdate.Location = new System.Drawing.Point(822, 9);
            this.Arrdate.Name = "Arrdate";
            this.Arrdate.ReadOnly = true;
            this.Arrdate.Size = new System.Drawing.Size(130, 23);
            this.Arrdate.TabIndex = 154;
            // 
            // LIDate
            // 
            this.LIDate.IsSupportEditMode = false;
            this.LIDate.Location = new System.Drawing.Point(822, 38);
            this.LIDate.Name = "LIDate";
            this.LIDate.ReadOnly = true;
            this.LIDate.Size = new System.Drawing.Size(130, 23);
            this.LIDate.TabIndex = 155;
            // 
            // Supptext
            // 
            this.Supptext.DisplayBox1Binding = "";
            this.Supptext.Location = new System.Drawing.Point(273, 38);
            this.Supptext.Name = "Supptext";
            this.Supptext.Size = new System.Drawing.Size(145, 23);
            this.Supptext.TabIndex = 156;
            this.Supptext.TextBox1Binding = "";
            // 
            // label14
            // 
            this.label14.Lines = 0;
            this.label14.Location = new System.Drawing.Point(472, 67);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 25);
            this.label14.TabIndex = 157;
            this.label14.Text = "Description";
            // 
            // Description_box
            // 
            this.Description_box.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Description_box.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Description_box.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Description_box.Location = new System.Drawing.Point(550, 67);
            this.Description_box.Multiline = true;
            this.Description_box.Name = "Description_box";
            this.Description_box.ReadOnly = true;
            this.Description_box.Size = new System.Drawing.Size(320, 54);
            this.Description_box.TabIndex = 158;
            // 
            // P03_Crocking
            // 
            this.ClientSize = new System.Drawing.Size(974, 585);
            this.Controls.Add(this.Description_box);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.Supptext);
            this.Controls.Add(this.LIDate);
            this.Controls.Add(this.Arrdate);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.encode_button);
            this.Controls.Add(this.ResultText);
            this.Controls.Add(this.AQtytext);
            this.Controls.Add(this.Colortext);
            this.Controls.Add(this.BRnotext);
            this.Controls.Add(this.SRnotext);
            this.Controls.Add(this.SEQtext);
            this.Controls.Add(this.Brandtext);
            this.Controls.Add(this.Styletext);
            this.Controls.Add(this.Wknotext);
            this.Controls.Add(this.sptext);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.GridPopUp = false;
            this.KeyField1 = "id";
            this.Name = "P03_Crocking";
            this.Text = "Crocking Test";
            this.WorkAlias = "FIR_Laboratory_Crocking";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.sptext, 0);
            this.Controls.SetChildIndex(this.Wknotext, 0);
            this.Controls.SetChildIndex(this.Styletext, 0);
            this.Controls.SetChildIndex(this.Brandtext, 0);
            this.Controls.SetChildIndex(this.SEQtext, 0);
            this.Controls.SetChildIndex(this.SRnotext, 0);
            this.Controls.SetChildIndex(this.BRnotext, 0);
            this.Controls.SetChildIndex(this.Colortext, 0);
            this.Controls.SetChildIndex(this.AQtytext, 0);
            this.Controls.SetChildIndex(this.ResultText, 0);
            this.Controls.SetChildIndex(this.encode_button, 0);
            this.Controls.SetChildIndex(this.checkBox1, 0);
            this.Controls.SetChildIndex(this.Arrdate, 0);
            this.Controls.SetChildIndex(this.LIDate, 0);
            this.Controls.SetChildIndex(this.Supptext, 0);
            this.Controls.SetChildIndex(this.label14, 0);
            this.Controls.SetChildIndex(this.Description_box, 0);
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.Label label8;
        private Win.UI.Label label9;
        private Win.UI.Label label10;
        private Win.UI.Label label11;
        private Win.UI.Label label12;
        private Win.UI.Label label13;
        private Win.UI.TextBox sptext;
        private Win.UI.TextBox Wknotext;
        private Win.UI.TextBox Styletext;
        private Win.UI.TextBox Brandtext;
        private Win.UI.TextBox SEQtext;
        private Win.UI.TextBox SRnotext;
        private Win.UI.TextBox BRnotext;
        private Win.UI.TextBox Colortext;
        private Win.UI.TextBox AQtytext;
        private Win.UI.TextBox ResultText;
        private Win.UI.Button encode_button;
        private Win.UI.CheckBox checkBox1;
        private Win.UI.Button ToExcelBtn;
        private Win.UI.DateBox Arrdate;
        private Win.UI.DateBox LIDate;
        private Class.txtsupplier Supptext;
        private Win.UI.Label label14;
        private Win.UI.EditBox Description_box;
    }
}
