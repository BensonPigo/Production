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
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelRefNo = new Sci.Win.UI.Label();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.labelLocation = new Sci.Win.UI.Label();
            this.txtLocation = new Sci.Win.UI.TextBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.checkBalanceQty = new Sci.Win.UI.CheckBox();
            this.txtStyle = new Sci.Win.UI.TextBox();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelColor = new Sci.Win.UI.Label();
            this.txtSizeCode = new Sci.Win.UI.TextBox();
            this.labelSizeCode = new Sci.Win.UI.Label();
            this.txtColor = new Sci.Win.UI.TextBox();
            this.labelStyle = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(484, 5);
            this.print.TabIndex = 9;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(484, 41);
            this.toexcel.TabIndex = 10;
            this.toexcel.Click += new System.EventHandler(this.toexcel_Click);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(484, 77);
            this.close.TabIndex = 11;
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(102, 12);
            this.txtSPNo.MaxLength = 13;
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(118, 23);
            this.txtSPNo.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(10, 12);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(89, 23);
            this.labelSPNo.TabIndex = 96;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelRefNo
            // 
            this.labelRefNo.BackColor = System.Drawing.Color.SkyBlue;
            this.labelRefNo.Lines = 0;
            this.labelRefNo.Location = new System.Drawing.Point(10, 48);
            this.labelRefNo.Name = "labelRefNo";
            this.labelRefNo.Size = new System.Drawing.Size(89, 23);
            this.labelRefNo.TabIndex = 97;
            this.labelRefNo.Text = "Ref#";
            this.labelRefNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(101, 48);
            this.txtRefno.MaxLength = 26;
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(118, 23);
            this.txtRefno.TabIndex = 2;
            // 
            // labelLocation
            // 
            this.labelLocation.Lines = 0;
            this.labelLocation.Location = new System.Drawing.Point(248, 48);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.RectStyle.Color = System.Drawing.Color.LightSkyBlue;
            this.labelLocation.Size = new System.Drawing.Size(89, 23);
            this.labelLocation.TabIndex = 100;
            this.labelLocation.Text = "Location";
            this.labelLocation.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtLocation
            // 
            this.txtLocation.BackColor = System.Drawing.Color.White;
            this.txtLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocation.Location = new System.Drawing.Point(339, 48);
            this.txtLocation.MaxLength = 10;
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(118, 23);
            this.txtLocation.TabIndex = 3;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(10, 152);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(89, 23);
            this.labelFactory.TabIndex = 104;
            this.labelFactory.Text = "Factory";
            // 
            // checkBalanceQty
            // 
            this.checkBalanceQty.AutoSize = true;
            this.checkBalanceQty.Checked = true;
            this.checkBalanceQty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBalanceQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBalanceQty.Location = new System.Drawing.Point(257, 191);
            this.checkBalanceQty.Name = "checkBalanceQty";
            this.checkBalanceQty.Size = new System.Drawing.Size(128, 21);
            this.checkBalanceQty.TabIndex = 8;
            this.checkBalanceQty.Text = "Balance Qty > 0";
            this.checkBalanceQty.UseVisualStyleBackColor = true;
            // 
            // txtStyle
            // 
            this.txtStyle.BackColor = System.Drawing.Color.White;
            this.txtStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStyle.Location = new System.Drawing.Point(340, 12);
            this.txtStyle.MaxLength = 20;
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.Size = new System.Drawing.Size(118, 23);
            this.txtStyle.TabIndex = 1;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Items.AddRange(new object[] {
            "Bulk+Sample",
            "Bulk",
            "Sample",
            "Material"});
            this.comboCategory.Location = new System.Drawing.Point(102, 188);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.Size = new System.Drawing.Size(121, 24);
            this.comboCategory.TabIndex = 7;
            // 
            // labelCategory
            // 
            this.labelCategory.Lines = 0;
            this.labelCategory.Location = new System.Drawing.Point(10, 188);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(89, 23);
            this.labelCategory.TabIndex = 107;
            this.labelCategory.Text = "Category";
            // 
            // labelColor
            // 
            this.labelColor.Lines = 0;
            this.labelColor.Location = new System.Drawing.Point(10, 84);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(89, 23);
            this.labelColor.TabIndex = 108;
            this.labelColor.Text = "Color";
            // 
            // txtSizeCode
            // 
            this.txtSizeCode.BackColor = System.Drawing.Color.White;
            this.txtSizeCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSizeCode.Location = new System.Drawing.Point(102, 120);
            this.txtSizeCode.MaxLength = 10;
            this.txtSizeCode.Name = "txtSizeCode";
            this.txtSizeCode.Size = new System.Drawing.Size(121, 23);
            this.txtSizeCode.TabIndex = 5;
            // 
            // labelSizeCode
            // 
            this.labelSizeCode.Lines = 0;
            this.labelSizeCode.Location = new System.Drawing.Point(10, 120);
            this.labelSizeCode.Name = "labelSizeCode";
            this.labelSizeCode.Size = new System.Drawing.Size(89, 23);
            this.labelSizeCode.TabIndex = 110;
            this.labelSizeCode.Text = "SizeCode";
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.White;
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtColor.Location = new System.Drawing.Point(102, 84);
            this.txtColor.MaxLength = 10;
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(121, 23);
            this.txtColor.TabIndex = 4;
            // 
            // labelStyle
            // 
            this.labelStyle.Lines = 0;
            this.labelStyle.Location = new System.Drawing.Point(248, 12);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(89, 23);
            this.labelStyle.TabIndex = 114;
            this.labelStyle.Text = "Style";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.Location = new System.Drawing.Point(102, 152);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 115;
            // 
            // R18
            // 
            this.ClientSize = new System.Drawing.Size(576, 254);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.txtSizeCode);
            this.Controls.Add(this.labelSizeCode);
            this.Controls.Add(this.labelColor);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.txtStyle);
            this.Controls.Add(this.checkBalanceQty);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.labelLocation);
            this.Controls.Add(this.txtRefno);
            this.Controls.Add(this.labelRefNo);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.txtSPNo);
            this.Name = "R18";
            this.Text = "R18. Material Tracking";
            this.Load += new System.EventHandler(this.R18_Load);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelRefNo, 0);
            this.Controls.SetChildIndex(this.txtRefno, 0);
            this.Controls.SetChildIndex(this.labelLocation, 0);
            this.Controls.SetChildIndex(this.txtLocation, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.checkBalanceQty, 0);
            this.Controls.SetChildIndex(this.txtStyle, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.Controls.SetChildIndex(this.labelColor, 0);
            this.Controls.SetChildIndex(this.labelSizeCode, 0);
            this.Controls.SetChildIndex(this.txtSizeCode, 0);
            this.Controls.SetChildIndex(this.txtColor, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelRefNo;
        private Win.UI.TextBox txtRefno;
        private Win.UI.Label labelLocation;
        private Win.UI.TextBox txtLocation;
        private Win.UI.Label labelFactory;
        private Win.UI.CheckBox checkBalanceQty;
        private Win.UI.TextBox txtStyle;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelColor;
        private Win.UI.TextBox txtSizeCode;
        private Win.UI.Label labelSizeCode;
        private Win.UI.TextBox txtColor;
        private Win.UI.Label labelStyle;
        private Class.txtfactory txtfactory;
    }
}
