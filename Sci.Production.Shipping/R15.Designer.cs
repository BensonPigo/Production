namespace Sci.Production.Shipping
{
    partial class R15
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
            this.chkJunk = new Sci.Win.UI.CheckBox();
            this.comboStatus = new Sci.Win.UI.ComboBox();
            this.labelStatus = new Sci.Win.UI.Label();
            this.labBrand = new Sci.Win.UI.Label();
            this.dateApproveDate = new Sci.Win.UI.DateRange();
            this.labApproveDate = new Sci.Win.UI.Label();
            this.dateAddDate = new Sci.Win.UI.DateRange();
            this.labAddDate = new Sci.Win.UI.Label();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(461, 120);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(461, 12);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(461, 48);
            this.close.TabIndex = 6;
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkJunk.Location = new System.Drawing.Point(39, 158);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(106, 21);
            this.chkJunk.TabIndex = 4;
            this.chkJunk.Text = "Include Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // comboStatus
            // 
            this.comboStatus.BackColor = System.Drawing.Color.White;
            this.comboStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.IsSupportUnselect = true;
            this.comboStatus.Location = new System.Drawing.Point(141, 120);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.OldText = "";
            this.comboStatus.Size = new System.Drawing.Size(102, 24);
            this.comboStatus.TabIndex = 3;
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(39, 120);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(96, 23);
            this.labelStatus.TabIndex = 110;
            this.labelStatus.Text = "Status";
            // 
            // labBrand
            // 
            this.labBrand.Location = new System.Drawing.Point(39, 85);
            this.labBrand.Name = "labBrand";
            this.labBrand.Size = new System.Drawing.Size(96, 23);
            this.labBrand.TabIndex = 109;
            this.labBrand.Text = "Brand";
            // 
            // dateApproveDate
            // 
            // 
            // 
            // 
            this.dateApproveDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateApproveDate.DateBox1.Name = "";
            this.dateApproveDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateApproveDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateApproveDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateApproveDate.DateBox2.Name = "";
            this.dateApproveDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateApproveDate.DateBox2.TabIndex = 1;
            this.dateApproveDate.IsRequired = false;
            this.dateApproveDate.Location = new System.Drawing.Point(141, 50);
            this.dateApproveDate.Name = "dateApproveDate";
            this.dateApproveDate.Size = new System.Drawing.Size(280, 23);
            this.dateApproveDate.TabIndex = 1;
            // 
            // labApproveDate
            // 
            this.labApproveDate.Location = new System.Drawing.Point(39, 50);
            this.labApproveDate.Name = "labApproveDate";
            this.labApproveDate.Size = new System.Drawing.Size(96, 23);
            this.labApproveDate.TabIndex = 108;
            this.labApproveDate.Text = "Approve Date";
            // 
            // dateAddDate
            // 
            // 
            // 
            // 
            this.dateAddDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateAddDate.DateBox1.Name = "";
            this.dateAddDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateAddDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateAddDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateAddDate.DateBox2.Name = "";
            this.dateAddDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateAddDate.DateBox2.TabIndex = 1;
            this.dateAddDate.IsRequired = false;
            this.dateAddDate.Location = new System.Drawing.Point(141, 16);
            this.dateAddDate.Name = "dateAddDate";
            this.dateAddDate.Size = new System.Drawing.Size(280, 23);
            this.dateAddDate.TabIndex = 0;
            // 
            // labAddDate
            // 
            this.labAddDate.Location = new System.Drawing.Point(39, 16);
            this.labAddDate.Name = "labAddDate";
            this.labAddDate.Size = new System.Drawing.Size(96, 23);
            this.labAddDate.TabIndex = 107;
            this.labAddDate.Text = "Add Date";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(141, 85);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(102, 23);
            this.txtbrand.TabIndex = 2;
            // 
            // R15
            // 
            this.ClientSize = new System.Drawing.Size(553, 224);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.chkJunk);
            this.Controls.Add(this.comboStatus);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labBrand);
            this.Controls.Add(this.dateApproveDate);
            this.Controls.Add(this.labApproveDate);
            this.Controls.Add(this.dateAddDate);
            this.Controls.Add(this.labAddDate);
            this.Name = "R15";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R15. Shipping Expense Quotation List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labAddDate, 0);
            this.Controls.SetChildIndex(this.dateAddDate, 0);
            this.Controls.SetChildIndex(this.labApproveDate, 0);
            this.Controls.SetChildIndex(this.dateApproveDate, 0);
            this.Controls.SetChildIndex(this.labBrand, 0);
            this.Controls.SetChildIndex(this.labelStatus, 0);
            this.Controls.SetChildIndex(this.comboStatus, 0);
            this.Controls.SetChildIndex(this.chkJunk, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.CheckBox chkJunk;
        private Win.UI.ComboBox comboStatus;
        private Win.UI.Label labelStatus;
        private Win.UI.Label labBrand;
        private Win.UI.DateRange dateApproveDate;
        private Win.UI.Label labApproveDate;
        private Win.UI.DateRange dateAddDate;
        private Win.UI.Label labAddDate;
        private Class.Txtbrand txtbrand;
    }
}
