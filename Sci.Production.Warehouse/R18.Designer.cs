namespace Sci.Production.Warehouse
{
    partial class R18
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
            this.tbxSP = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.tbxRefno = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.tbxLocation = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.tbxStyle = new Sci.Win.UI.TextBox();
            this.cbxCategory = new Sci.Win.UI.ComboBox();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.tbxSizeCode = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.txtmfactory1 = new Sci.Production.Class.txtmfactory();
            this.tbxColor = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(484, 5);
            this.print.TabIndex = 7;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(484, 41);
            this.toexcel.TabIndex = 8;
            this.toexcel.Click += new System.EventHandler(this.toexcel_Click);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(484, 77);
            this.close.TabIndex = 9;
            // 
            // tbxSP
            // 
            this.tbxSP.BackColor = System.Drawing.Color.White;
            this.tbxSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tbxSP.Location = new System.Drawing.Point(102, 12);
            this.tbxSP.MaxLength = 13;
            this.tbxSP.Name = "tbxSP";
            this.tbxSP.Size = new System.Drawing.Size(118, 23);
            this.tbxSP.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(10, 12);
            this.label2.Name = "label2";
            this.label2.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label2.Size = new System.Drawing.Size(89, 23);
            this.label2.TabIndex = 96;
            this.label2.Text = "SP#";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.SkyBlue;
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(10, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 23);
            this.label3.TabIndex = 97;
            this.label3.Text = "Ref#";
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // tbxRefno
            // 
            this.tbxRefno.BackColor = System.Drawing.Color.White;
            this.tbxRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tbxRefno.Location = new System.Drawing.Point(101, 48);
            this.tbxRefno.MaxLength = 26;
            this.tbxRefno.Name = "tbxRefno";
            this.tbxRefno.Size = new System.Drawing.Size(118, 23);
            this.tbxRefno.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(248, 48);
            this.label4.Name = "label4";
            this.label4.RectStyle.Color = System.Drawing.Color.LightSkyBlue;
            this.label4.Size = new System.Drawing.Size(89, 23);
            this.label4.TabIndex = 100;
            this.label4.Text = "Location";
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // tbxLocation
            // 
            this.tbxLocation.BackColor = System.Drawing.Color.White;
            this.tbxLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tbxLocation.Location = new System.Drawing.Point(339, 48);
            this.tbxLocation.MaxLength = 10;
            this.tbxLocation.Name = "tbxLocation";
            this.tbxLocation.Size = new System.Drawing.Size(118, 23);
            this.tbxLocation.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(10, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 23);
            this.label6.TabIndex = 104;
            this.label6.Text = "Factory";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(257, 191);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(128, 21);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Balance Qty > 0";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // tbxStyle
            // 
            this.tbxStyle.BackColor = System.Drawing.Color.White;
            this.tbxStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tbxStyle.Location = new System.Drawing.Point(340, 12);
            this.tbxStyle.MaxLength = 20;
            this.tbxStyle.Name = "tbxStyle";
            this.tbxStyle.Size = new System.Drawing.Size(118, 23);
            this.tbxStyle.TabIndex = 105;
            // 
            // cbxCategory
            // 
            this.cbxCategory.BackColor = System.Drawing.Color.White;
            this.cbxCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbxCategory.FormattingEnabled = true;
            this.cbxCategory.IsSupportUnselect = true;
            this.cbxCategory.Items.AddRange(new object[] {
            "Bulk+Sample",
            "Bulk",
            "Sample",
            "Material"});
            this.cbxCategory.Location = new System.Drawing.Point(102, 188);
            this.cbxCategory.Name = "cbxCategory";
            this.cbxCategory.Size = new System.Drawing.Size(121, 24);
            this.cbxCategory.TabIndex = 106;
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(10, 188);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 23);
            this.label7.TabIndex = 107;
            this.label7.Text = "Category";
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(10, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 23);
            this.label8.TabIndex = 108;
            this.label8.Text = "Color";
            // 
            // tbxSizeCode
            // 
            this.tbxSizeCode.BackColor = System.Drawing.Color.White;
            this.tbxSizeCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tbxSizeCode.Location = new System.Drawing.Point(102, 120);
            this.tbxSizeCode.MaxLength = 10;
            this.tbxSizeCode.Name = "tbxSizeCode";
            this.tbxSizeCode.Size = new System.Drawing.Size(121, 23);
            this.tbxSizeCode.TabIndex = 111;
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(10, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 23);
            this.label9.TabIndex = 110;
            this.label9.Text = "SizeCode";
            // 
            // txtmfactory1
            // 
            this.txtmfactory1.BackColor = System.Drawing.Color.White;
            this.txtmfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtmfactory1.Location = new System.Drawing.Point(102, 152);
            this.txtmfactory1.MaxLength = 8;
            this.txtmfactory1.Name = "txtmfactory1";
            this.txtmfactory1.Size = new System.Drawing.Size(121, 23);
            this.txtmfactory1.TabIndex = 112;
            // 
            // tbxColor
            // 
            this.tbxColor.BackColor = System.Drawing.Color.White;
            this.tbxColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tbxColor.Location = new System.Drawing.Point(102, 84);
            this.tbxColor.MaxLength = 10;
            this.tbxColor.Name = "tbxColor";
            this.tbxColor.Size = new System.Drawing.Size(121, 23);
            this.tbxColor.TabIndex = 113;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(248, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 23);
            this.label1.TabIndex = 114;
            this.label1.Text = "Style";
            // 
            // R18
            // 
            this.ClientSize = new System.Drawing.Size(576, 254);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxColor);
            this.Controls.Add(this.txtmfactory1);
            this.Controls.Add(this.tbxSizeCode);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbxCategory);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbxStyle);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbxLocation);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbxRefno);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxSP);
            this.Name = "R18";
            this.Text = "R18. Material Tracking";
            this.Load += new System.EventHandler(this.R18_Load);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.tbxSP, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbxRefno, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.tbxLocation, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.checkBox1, 0);
            this.Controls.SetChildIndex(this.tbxStyle, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.cbxCategory, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.tbxSizeCode, 0);
            this.Controls.SetChildIndex(this.txtmfactory1, 0);
            this.Controls.SetChildIndex(this.tbxColor, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox tbxSP;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.TextBox tbxRefno;
        private Win.UI.Label label4;
        private Win.UI.TextBox tbxLocation;
        private Win.UI.Label label6;
        private Win.UI.CheckBox checkBox1;
        private Win.UI.TextBox tbxStyle;
        private Win.UI.ComboBox cbxCategory;
        private Win.UI.Label label7;
        private Win.UI.Label label8;
        private Win.UI.TextBox tbxSizeCode;
        private Win.UI.Label label9;
        private Class.txtmfactory txtmfactory1;
        private Win.UI.TextBox tbxColor;
        private Win.UI.Label label1;
    }
}
