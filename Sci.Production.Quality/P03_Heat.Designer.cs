namespace Sci.Production.Quality
{
    partial class P03_Heat
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
            this.AQtytext = new Sci.Win.UI.TextBox();
            this.Colortext = new Sci.Win.UI.TextBox();
            this.BRnotext = new Sci.Win.UI.TextBox();
            this.SRnotext = new Sci.Win.UI.TextBox();
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
            this.Supptext = new Sci.Production.Class.txtsupplier();
            this.Arrdate = new Sci.Win.UI.DateBox();
            this.LIDate = new Sci.Win.UI.DateBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.ToExcel);
            this.btmcont.Location = new System.Drawing.Point(0, 538);
            this.btmcont.Size = new System.Drawing.Size(1056, 40);
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
            this.gridcont.Size = new System.Drawing.Size(1032, 385);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(966, 5);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(886, 5);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkBox1.IsSupportEditMode = false;
            this.checkBox1.Location = new System.Drawing.Point(925, 69);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.ReadOnly = true;
            this.checkBox1.Size = new System.Drawing.Size(50, 21);
            this.checkBox1.TabIndex = 181;
            this.checkBox1.Text = "N/A";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // encode_button
            // 
            this.encode_button.Location = new System.Drawing.Point(925, 107);
            this.encode_button.Name = "encode_button";
            this.encode_button.Size = new System.Drawing.Size(80, 30);
            this.encode_button.TabIndex = 180;
            this.encode_button.Text = "Encode";
            this.encode_button.UseVisualStyleBackColor = true;
            this.encode_button.Click += new System.EventHandler(this.encode_button_Click);
            // 
            // ResultText
            // 
            this.ResultText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.ResultText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.ResultText.IsSupportEditMode = false;
            this.ResultText.Location = new System.Drawing.Point(806, 66);
            this.ResultText.Name = "ResultText";
            this.ResultText.ReadOnly = true;
            this.ResultText.Size = new System.Drawing.Size(100, 23);
            this.ResultText.TabIndex = 179;
            // 
            // AQtytext
            // 
            this.AQtytext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.AQtytext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.AQtytext.IsSupportEditMode = false;
            this.AQtytext.Location = new System.Drawing.Point(585, 38);
            this.AQtytext.Name = "AQtytext";
            this.AQtytext.ReadOnly = true;
            this.AQtytext.Size = new System.Drawing.Size(134, 23);
            this.AQtytext.TabIndex = 176;
            // 
            // Colortext
            // 
            this.Colortext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Colortext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Colortext.IsSupportEditMode = false;
            this.Colortext.Location = new System.Drawing.Point(585, 9);
            this.Colortext.Name = "Colortext";
            this.Colortext.ReadOnly = true;
            this.Colortext.Size = new System.Drawing.Size(134, 23);
            this.Colortext.TabIndex = 175;
            // 
            // BRnotext
            // 
            this.BRnotext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.BRnotext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.BRnotext.IsSupportEditMode = false;
            this.BRnotext.Location = new System.Drawing.Point(345, 96);
            this.BRnotext.Name = "BRnotext";
            this.BRnotext.ReadOnly = true;
            this.BRnotext.Size = new System.Drawing.Size(147, 23);
            this.BRnotext.TabIndex = 174;
            // 
            // SRnotext
            // 
            this.SRnotext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.SRnotext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.SRnotext.IsSupportEditMode = false;
            this.SRnotext.Location = new System.Drawing.Point(345, 67);
            this.SRnotext.Name = "SRnotext";
            this.SRnotext.ReadOnly = true;
            this.SRnotext.Size = new System.Drawing.Size(147, 23);
            this.SRnotext.TabIndex = 173;
            // 
            // SEQtext
            // 
            this.SEQtext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.SEQtext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.SEQtext.IsSupportEditMode = false;
            this.SEQtext.Location = new System.Drawing.Point(345, 9);
            this.SEQtext.Name = "SEQtext";
            this.SEQtext.ReadOnly = true;
            this.SEQtext.Size = new System.Drawing.Size(147, 23);
            this.SEQtext.TabIndex = 171;
            // 
            // Brandtext
            // 
            this.Brandtext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Brandtext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Brandtext.IsSupportEditMode = false;
            this.Brandtext.Location = new System.Drawing.Point(88, 96);
            this.Brandtext.Name = "Brandtext";
            this.Brandtext.ReadOnly = true;
            this.Brandtext.Size = new System.Drawing.Size(141, 23);
            this.Brandtext.TabIndex = 170;
            // 
            // Styletext
            // 
            this.Styletext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Styletext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Styletext.IsSupportEditMode = false;
            this.Styletext.Location = new System.Drawing.Point(88, 67);
            this.Styletext.Name = "Styletext";
            this.Styletext.ReadOnly = true;
            this.Styletext.Size = new System.Drawing.Size(141, 23);
            this.Styletext.TabIndex = 169;
            // 
            // Wknotext
            // 
            this.Wknotext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Wknotext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Wknotext.IsSupportEditMode = false;
            this.Wknotext.Location = new System.Drawing.Point(88, 38);
            this.Wknotext.Name = "Wknotext";
            this.Wknotext.ReadOnly = true;
            this.Wknotext.Size = new System.Drawing.Size(141, 23);
            this.Wknotext.TabIndex = 168;
            // 
            // sptext
            // 
            this.sptext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.sptext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.sptext.IsSupportEditMode = false;
            this.sptext.Location = new System.Drawing.Point(88, 9);
            this.sptext.Name = "sptext";
            this.sptext.ReadOnly = true;
            this.sptext.Size = new System.Drawing.Size(141, 23);
            this.sptext.TabIndex = 167;
            // 
            // label13
            // 
            this.label13.Lines = 0;
            this.label13.Location = new System.Drawing.Point(728, 68);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(75, 23);
            this.label13.TabIndex = 166;
            this.label13.Text = "Result";
            // 
            // label12
            // 
            this.label12.Lines = 0;
            this.label12.Location = new System.Drawing.Point(728, 39);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(144, 23);
            this.label12.TabIndex = 165;
            this.label12.Text = "Last Inspection Date :";
            // 
            // label11
            // 
            this.label11.Lines = 0;
            this.label11.Location = new System.Drawing.Point(728, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(144, 23);
            this.label11.TabIndex = 164;
            this.label11.Text = "Arrive W/H Date:";
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(507, 39);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 23);
            this.label10.TabIndex = 163;
            this.label10.Text = "Arrive Qty:";
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(507, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 23);
            this.label9.TabIndex = 162;
            this.label9.Text = "Color:";
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(245, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 26);
            this.label8.TabIndex = 161;
            this.label8.Text = "Brand Refno:";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(245, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 23);
            this.label7.TabIndex = 160;
            this.label7.Text = "SCI Refno:";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(245, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 23);
            this.label6.TabIndex = 159;
            this.label6.Text = "Supp:";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(245, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 23);
            this.label5.TabIndex = 158;
            this.label5.Text = "SEQ#:";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(9, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 157;
            this.label4.Text = "Brand:";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(9, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 156;
            this.label3.Text = "Style#:";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(9, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 155;
            this.label2.Text = "Wkno:";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 154;
            this.label1.Text = "SP#:";
            // 
            // ToExcel
            // 
            this.ToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.ToExcel.Location = new System.Drawing.Point(759, 5);
            this.ToExcel.Name = "ToExcel";
            this.ToExcel.Size = new System.Drawing.Size(80, 30);
            this.ToExcel.TabIndex = 95;
            this.ToExcel.Text = "ToExcel";
            this.ToExcel.UseVisualStyleBackColor = true;
            this.ToExcel.Click += new System.EventHandler(this.ToExcel_Click);
            // 
            // Supptext
            // 
            this.Supptext.DisplayBox1Binding = "";
            this.Supptext.Location = new System.Drawing.Point(345, 39);
            this.Supptext.Name = "Supptext";
            this.Supptext.Size = new System.Drawing.Size(147, 23);
            this.Supptext.TabIndex = 182;
            this.Supptext.TextBox1Binding = "";
            // 
            // Arrdate
            // 
            this.Arrdate.IsSupportEditMode = false;
            this.Arrdate.Location = new System.Drawing.Point(875, 9);
            this.Arrdate.Name = "Arrdate";
            this.Arrdate.ReadOnly = true;
            this.Arrdate.Size = new System.Drawing.Size(130, 23);
            this.Arrdate.TabIndex = 183;
            // 
            // LIDate
            // 
            this.LIDate.IsSupportEditMode = false;
            this.LIDate.Location = new System.Drawing.Point(875, 40);
            this.LIDate.Name = "LIDate";
            this.LIDate.ReadOnly = true;
            this.LIDate.Size = new System.Drawing.Size(130, 23);
            this.LIDate.TabIndex = 184;
            // 
            // P03_Heat
            // 
            this.ClientSize = new System.Drawing.Size(1056, 578);
            this.Controls.Add(this.LIDate);
            this.Controls.Add(this.Arrdate);
            this.Controls.Add(this.Supptext);
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
            this.KeyField1 = "ID";
            this.Name = "P03_Heat";
            this.Text = "Heat Test";
            this.WorkAlias = "FIR_Laboratory_Heat";
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
            this.Controls.SetChildIndex(this.SRnotext, 0);
            this.Controls.SetChildIndex(this.BRnotext, 0);
            this.Controls.SetChildIndex(this.Colortext, 0);
            this.Controls.SetChildIndex(this.AQtytext, 0);
            this.Controls.SetChildIndex(this.ResultText, 0);
            this.Controls.SetChildIndex(this.encode_button, 0);
            this.Controls.SetChildIndex(this.checkBox1, 0);
            this.Controls.SetChildIndex(this.Supptext, 0);
            this.Controls.SetChildIndex(this.Arrdate, 0);
            this.Controls.SetChildIndex(this.LIDate, 0);
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
        private Win.UI.TextBox AQtytext;
        private Win.UI.TextBox Colortext;
        private Win.UI.TextBox BRnotext;
        private Win.UI.TextBox SRnotext;
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
        private Class.txtsupplier Supptext;
        private Win.UI.DateBox Arrdate;
        private Win.UI.DateBox LIDate;

    }
}
