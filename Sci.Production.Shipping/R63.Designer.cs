namespace Sci.Production.Shipping
{
    partial class R63
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
            this.txtGBTo = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtGBFrom = new Sci.Win.UI.TextBox();
            this.labDecNo = new Sci.Win.UI.Label();
            this.dateCMTInvDate = new Sci.Win.UI.DateRange();
            this.labCMTInv = new Sci.Win.UI.Label();
            this.labGB = new Sci.Win.UI.Label();
            this.chkOutstanding = new Sci.Win.UI.CheckBox();
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(498, 80);
            this.print.TabIndex = 8;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(498, 9);
            this.toexcel.TabIndex = 6;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(498, 45);
            this.close.TabIndex = 7;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(452, 114);
            this.buttonCustomized.TabIndex = 13;
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(457, 123);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(457, 123);
            this.txtVersion.TabIndex = 14;
            // 
            // txtGBTo
            // 
            this.txtGBTo.BackColor = System.Drawing.Color.White;
            this.txtGBTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGBTo.Location = new System.Drawing.Point(284, 46);
            this.txtGBTo.Name = "txtGBTo";
            this.txtGBTo.Size = new System.Drawing.Size(125, 23);
            this.txtGBTo.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("標楷體", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(262, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "~";
            this.label1.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtGBFrom
            // 
            this.txtGBFrom.BackColor = System.Drawing.Color.White;
            this.txtGBFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGBFrom.Location = new System.Drawing.Point(133, 46);
            this.txtGBFrom.Name = "txtGBFrom";
            this.txtGBFrom.Size = new System.Drawing.Size(125, 23);
            this.txtGBFrom.TabIndex = 2;
            // 
            // labDecNo
            // 
            this.labDecNo.Location = new System.Drawing.Point(13, 75);
            this.labDecNo.Name = "labDecNo";
            this.labDecNo.Size = new System.Drawing.Size(117, 23);
            this.labDecNo.TabIndex = 11;
            this.labDecNo.Text = "Brand";
            // 
            // dateCMTInvDate
            // 
            // 
            // 
            // 
            this.dateCMTInvDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCMTInvDate.DateBox1.Name = "";
            this.dateCMTInvDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCMTInvDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCMTInvDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCMTInvDate.DateBox2.Name = "";
            this.dateCMTInvDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCMTInvDate.DateBox2.TabIndex = 1;
            this.dateCMTInvDate.Location = new System.Drawing.Point(133, 17);
            this.dateCMTInvDate.Name = "dateCMTInvDate";
            this.dateCMTInvDate.Size = new System.Drawing.Size(280, 23);
            this.dateCMTInvDate.TabIndex = 0;
            // 
            // labCMTInv
            // 
            this.labCMTInv.Location = new System.Drawing.Point(13, 17);
            this.labCMTInv.Name = "labCMTInv";
            this.labCMTInv.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labCMTInv.RectStyle.BorderWidth = 1F;
            this.labCMTInv.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labCMTInv.RectStyle.ExtBorderWidth = 1F;
            this.labCMTInv.Size = new System.Drawing.Size(117, 23);
            this.labCMTInv.TabIndex = 97;
            this.labCMTInv.Text = "CMT Invoice Date";
            this.labCMTInv.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labCMTInv.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labGB
            // 
            this.labGB.Location = new System.Drawing.Point(13, 46);
            this.labGB.Name = "labGB";
            this.labGB.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labGB.RectStyle.BorderWidth = 1F;
            this.labGB.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labGB.RectStyle.ExtBorderWidth = 1F;
            this.labGB.Size = new System.Drawing.Size(117, 23);
            this.labGB.TabIndex = 98;
            this.labGB.Text = "GB#";
            this.labGB.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labGB.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // chkOutstanding
            // 
            this.chkOutstanding.AutoSize = true;
            this.chkOutstanding.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkOutstanding.Location = new System.Drawing.Point(133, 104);
            this.chkOutstanding.Name = "chkOutstanding";
            this.chkOutstanding.Size = new System.Drawing.Size(104, 21);
            this.chkOutstanding.TabIndex = 99;
            this.chkOutstanding.Text = "Outstanding";
            this.chkOutstanding.UseVisualStyleBackColor = true;
            this.chkOutstanding.CheckedChanged += new System.EventHandler(this.ChkOutstanding_CheckedChanged);
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(133, 75);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(182, 23);
            this.txtBrand.TabIndex = 100;
            // 
            // R63
            // 
            this.ClientSize = new System.Drawing.Size(590, 173);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.chkOutstanding);
            this.Controls.Add(this.labGB);
            this.Controls.Add(this.labCMTInv);
            this.Controls.Add(this.txtGBTo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtGBFrom);
            this.Controls.Add(this.labDecNo);
            this.Controls.Add(this.dateCMTInvDate);
            this.Name = "R63";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R63. KH CMT Invoice Report";
            this.Controls.SetChildIndex(this.dateCMTInvDate, 0);
            this.Controls.SetChildIndex(this.labDecNo, 0);
            this.Controls.SetChildIndex(this.txtGBFrom, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtGBTo, 0);
            this.Controls.SetChildIndex(this.labCMTInv, 0);
            this.Controls.SetChildIndex(this.labGB, 0);
            this.Controls.SetChildIndex(this.chkOutstanding, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.txtBrand, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox txtGBTo;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtGBFrom;
        private Win.UI.Label labDecNo;
        private Win.UI.DateRange dateCMTInvDate;
        private Win.UI.Label labCMTInv;
        private Win.UI.Label labGB;
        private Win.UI.CheckBox chkOutstanding;
        private Class.Txtbrand txtBrand;
    }
}
