namespace Sci.Production.Quality
{
    partial class P03_Wash
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
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.encode_button = new Sci.Win.UI.Button();
            this.ResultText = new Sci.Win.UI.TextBox();
            this.LIDate = new Sci.Win.UI.TextBox();
            this.Arrdate = new Sci.Win.UI.TextBox();
            this.AQtytext = new Sci.Win.UI.TextBox();
            this.Colortext = new Sci.Win.UI.TextBox();
            this.BRnotext = new Sci.Win.UI.TextBox();
            this.SRnotext = new Sci.Win.UI.TextBox();
            this.Supptext = new Sci.Win.UI.TextBox();
            this.SEQtext = new Sci.Win.UI.TextBox();
            this.Brandtext = new Sci.Win.UI.TextBox();
            this.Styletext = new Sci.Win.UI.TextBox();
            this.Wknotext = new Sci.Win.UI.TextBox();
            this.sptext = new Sci.Win.UI.TextBox();
            this.label13 = new Sci.Win.UI.Label();
            this.label12 = new Sci.Win.UI.Label();
            this.label11 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.ToExcel = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.ToExcel);
            this.btmcont.Size = new System.Drawing.Size(1036, 40);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.ToExcel, 0);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 143);
            this.gridcont.Size = new System.Drawing.Size(1012, 304);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(946, 5);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(866, 5);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(884, 72);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(50, 21);
            this.checkBox1.TabIndex = 209;
            this.checkBox1.Text = "N/A";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // encode_button
            // 
            this.encode_button.Location = new System.Drawing.Point(788, 110);
            this.encode_button.Name = "encode_button";
            this.encode_button.Size = new System.Drawing.Size(80, 30);
            this.encode_button.TabIndex = 208;
            this.encode_button.Text = "Encode";
            this.encode_button.UseVisualStyleBackColor = true;
            this.encode_button.Click += new System.EventHandler(this.encode_button_Click);
            // 
            // ResultText
            // 
            this.ResultText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.ResultText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.ResultText.Location = new System.Drawing.Point(768, 71);
            this.ResultText.Name = "ResultText";
            this.ResultText.ReadOnly = true;
            this.ResultText.Size = new System.Drawing.Size(100, 23);
            this.ResultText.TabIndex = 207;
            // 
            // LIDate
            // 
            this.LIDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.LIDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.LIDate.Location = new System.Drawing.Point(834, 42);
            this.LIDate.Name = "LIDate";
            this.LIDate.ReadOnly = true;
            this.LIDate.Size = new System.Drawing.Size(100, 23);
            this.LIDate.TabIndex = 206;
            // 
            // Arrdate
            // 
            this.Arrdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Arrdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Arrdate.Location = new System.Drawing.Point(834, 12);
            this.Arrdate.Name = "Arrdate";
            this.Arrdate.ReadOnly = true;
            this.Arrdate.Size = new System.Drawing.Size(100, 23);
            this.Arrdate.TabIndex = 205;
            // 
            // AQtytext
            // 
            this.AQtytext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.AQtytext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.AQtytext.Location = new System.Drawing.Point(550, 41);
            this.AQtytext.Name = "AQtytext";
            this.AQtytext.ReadOnly = true;
            this.AQtytext.Size = new System.Drawing.Size(100, 23);
            this.AQtytext.TabIndex = 204;
            // 
            // Colortext
            // 
            this.Colortext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Colortext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Colortext.Location = new System.Drawing.Point(550, 12);
            this.Colortext.Name = "Colortext";
            this.Colortext.ReadOnly = true;
            this.Colortext.Size = new System.Drawing.Size(100, 23);
            this.Colortext.TabIndex = 203;
            // 
            // BRnotext
            // 
            this.BRnotext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.BRnotext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.BRnotext.Location = new System.Drawing.Point(339, 99);
            this.BRnotext.Name = "BRnotext";
            this.BRnotext.ReadOnly = true;
            this.BRnotext.Size = new System.Drawing.Size(100, 23);
            this.BRnotext.TabIndex = 202;
            // 
            // SRnotext
            // 
            this.SRnotext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.SRnotext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.SRnotext.Location = new System.Drawing.Point(325, 70);
            this.SRnotext.Name = "SRnotext";
            this.SRnotext.ReadOnly = true;
            this.SRnotext.Size = new System.Drawing.Size(100, 23);
            this.SRnotext.TabIndex = 201;
            // 
            // Supptext
            // 
            this.Supptext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Supptext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Supptext.Location = new System.Drawing.Point(325, 41);
            this.Supptext.Name = "Supptext";
            this.Supptext.ReadOnly = true;
            this.Supptext.Size = new System.Drawing.Size(100, 23);
            this.Supptext.TabIndex = 200;
            // 
            // SEQtext
            // 
            this.SEQtext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.SEQtext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.SEQtext.Location = new System.Drawing.Point(325, 12);
            this.SEQtext.Name = "SEQtext";
            this.SEQtext.ReadOnly = true;
            this.SEQtext.Size = new System.Drawing.Size(100, 23);
            this.SEQtext.TabIndex = 199;
            // 
            // Brandtext
            // 
            this.Brandtext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Brandtext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Brandtext.Location = new System.Drawing.Point(111, 99);
            this.Brandtext.Name = "Brandtext";
            this.Brandtext.ReadOnly = true;
            this.Brandtext.Size = new System.Drawing.Size(100, 23);
            this.Brandtext.TabIndex = 198;
            // 
            // Styletext
            // 
            this.Styletext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Styletext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Styletext.Location = new System.Drawing.Point(111, 70);
            this.Styletext.Name = "Styletext";
            this.Styletext.ReadOnly = true;
            this.Styletext.Size = new System.Drawing.Size(100, 23);
            this.Styletext.TabIndex = 197;
            // 
            // Wknotext
            // 
            this.Wknotext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Wknotext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Wknotext.Location = new System.Drawing.Point(111, 41);
            this.Wknotext.Name = "Wknotext";
            this.Wknotext.ReadOnly = true;
            this.Wknotext.Size = new System.Drawing.Size(100, 23);
            this.Wknotext.TabIndex = 196;
            // 
            // sptext
            // 
            this.sptext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.sptext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.sptext.Location = new System.Drawing.Point(111, 12);
            this.sptext.Name = "sptext";
            this.sptext.ReadOnly = true;
            this.sptext.Size = new System.Drawing.Size(100, 23);
            this.sptext.TabIndex = 195;
            // 
            // label13
            // 
            this.label13.Lines = 0;
            this.label13.Location = new System.Drawing.Point(678, 71);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(75, 23);
            this.label13.TabIndex = 194;
            this.label13.Text = "Result";
            // 
            // label12
            // 
            this.label12.Lines = 0;
            this.label12.Location = new System.Drawing.Point(678, 42);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(144, 23);
            this.label12.TabIndex = 193;
            this.label12.Text = "Last Inspection Date :";
            // 
            // label11
            // 
            this.label11.Lines = 0;
            this.label11.Location = new System.Drawing.Point(678, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(110, 23);
            this.label11.TabIndex = 192;
            this.label11.Text = "Arrive W/H Date:";
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(459, 42);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 23);
            this.label10.TabIndex = 191;
            this.label10.Text = "Arrive Qty:";
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(459, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 23);
            this.label9.TabIndex = 190;
            this.label9.Text = "Color:";
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(230, 99);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 26);
            this.label8.TabIndex = 189;
            this.label8.Text = "Brand Refno:";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(230, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 23);
            this.label7.TabIndex = 188;
            this.label7.Text = "SCI Refno:";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(230, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 23);
            this.label6.TabIndex = 187;
            this.label6.Text = "Supp:";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(230, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 186;
            this.label5.Text = "SEQ#:";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(18, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 185;
            this.label4.Text = "Brand:";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(18, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 184;
            this.label3.Text = "Style#:";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(18, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 183;
            this.label2.Text = "Wkno:";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(18, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 182;
            this.label1.Text = "SP#:";
            // 
            // ToExcel
            // 
            this.ToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.ToExcel.Location = new System.Drawing.Point(768, 5);
            this.ToExcel.Name = "ToExcel";
            this.ToExcel.Size = new System.Drawing.Size(80, 30);
            this.ToExcel.TabIndex = 95;
            this.ToExcel.Text = "ToExcel";
            this.ToExcel.UseVisualStyleBackColor = true;
            this.ToExcel.Click += new System.EventHandler(this.ToExcel_Click);
            // 
            // P03_Wash
            // 
            this.ClientSize = new System.Drawing.Size(1036, 497);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.encode_button);
            this.Controls.Add(this.ResultText);
            this.Controls.Add(this.LIDate);
            this.Controls.Add(this.Arrdate);
            this.Controls.Add(this.AQtytext);
            this.Controls.Add(this.Colortext);
            this.Controls.Add(this.BRnotext);
            this.Controls.Add(this.SRnotext);
            this.Controls.Add(this.Supptext);
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
            this.KeyField1 = "ID";
            this.Name = "P03_Wash";
            this.Text = "P03_Wash";
            this.WorkAlias = "FIR_Laboratory_Wash";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
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
            this.Controls.SetChildIndex(this.Supptext, 0);
            this.Controls.SetChildIndex(this.SRnotext, 0);
            this.Controls.SetChildIndex(this.BRnotext, 0);
            this.Controls.SetChildIndex(this.Colortext, 0);
            this.Controls.SetChildIndex(this.AQtytext, 0);
            this.Controls.SetChildIndex(this.Arrdate, 0);
            this.Controls.SetChildIndex(this.LIDate, 0);
            this.Controls.SetChildIndex(this.ResultText, 0);
            this.Controls.SetChildIndex(this.encode_button, 0);
            this.Controls.SetChildIndex(this.checkBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.CheckBox checkBox1;
        private Win.UI.Button encode_button;
        private Win.UI.TextBox ResultText;
        private Win.UI.TextBox LIDate;
        private Win.UI.TextBox Arrdate;
        private Win.UI.TextBox AQtytext;
        private Win.UI.TextBox Colortext;
        private Win.UI.TextBox BRnotext;
        private Win.UI.TextBox SRnotext;
        private Win.UI.TextBox Supptext;
        private Win.UI.TextBox SEQtext;
        private Win.UI.TextBox Brandtext;
        private Win.UI.TextBox Styletext;
        private Win.UI.TextBox Wknotext;
        private Win.UI.TextBox sptext;
        private Win.UI.Label label13;
        private Win.UI.Label label12;
        private Win.UI.Label label11;
        private Win.UI.Label label10;
        private Win.UI.Label label9;
        private Win.UI.Label label8;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Button ToExcel;

    }
}
