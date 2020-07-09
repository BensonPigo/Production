namespace Sci.Production.PPIC
{
    partial class R14
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labIssueDate = new Sci.Win.UI.Label();
            this.dateIssueDate = new Sci.Win.UI.DateRange();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.txtSpNo1 = new Sci.Win.UI.TextBox();
            this.labSPNo = new Sci.Win.UI.Label();
            this.labBrand = new Sci.Win.UI.Label();
            this.labBuyerDelivery = new Sci.Win.UI.Label();
            this.labStatus = new Sci.Win.UI.Label();
            this.txtSpNo2 = new Sci.Win.UI.TextBox();
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.comboDropDownList = new Sci.Production.Class.ComboDropDownList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.labM = new Sci.Win.UI.Label();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(432, 137);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(432, 17);
            this.toexcel.TabIndex = 7;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(432, 53);
            this.close.TabIndex = 8;
            // 
            // labIssueDate
            // 
            this.labIssueDate.Location = new System.Drawing.Point(28, 17);
            this.labIssueDate.Name = "labIssueDate";
            this.labIssueDate.Size = new System.Drawing.Size(100, 23);
            this.labIssueDate.TabIndex = 94;
            this.labIssueDate.Text = "Issue Date";
            // 
            // dateIssueDate
            // 
            // 
            // 
            // 
            this.dateIssueDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateIssueDate.DateBox1.Name = "";
            this.dateIssueDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateIssueDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateIssueDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateIssueDate.DateBox2.Name = "";
            this.dateIssueDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateIssueDate.DateBox2.TabIndex = 1;
            this.dateIssueDate.Location = new System.Drawing.Point(131, 17);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(280, 23);
            this.dateIssueDate.TabIndex = 0;
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(131, 107);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 4;
            // 
            // txtSpNo1
            // 
            this.txtSpNo1.BackColor = System.Drawing.Color.White;
            this.txtSpNo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpNo1.Location = new System.Drawing.Point(131, 48);
            this.txtSpNo1.Name = "txtSpNo1";
            this.txtSpNo1.Size = new System.Drawing.Size(130, 23);
            this.txtSpNo1.TabIndex = 1;
            // 
            // labSPNo
            // 
            this.labSPNo.Location = new System.Drawing.Point(28, 48);
            this.labSPNo.Name = "labSPNo";
            this.labSPNo.Size = new System.Drawing.Size(100, 23);
            this.labSPNo.TabIndex = 98;
            this.labSPNo.Text = "SP#";
            // 
            // labBrand
            // 
            this.labBrand.Location = new System.Drawing.Point(28, 78);
            this.labBrand.Name = "labBrand";
            this.labBrand.Size = new System.Drawing.Size(100, 23);
            this.labBrand.TabIndex = 99;
            this.labBrand.Text = "Brand";
            // 
            // labBuyerDelivery
            // 
            this.labBuyerDelivery.Location = new System.Drawing.Point(28, 107);
            this.labBuyerDelivery.Name = "labBuyerDelivery";
            this.labBuyerDelivery.Size = new System.Drawing.Size(100, 23);
            this.labBuyerDelivery.TabIndex = 100;
            this.labBuyerDelivery.Text = "Buyer Delivery";
            // 
            // labStatus
            // 
            this.labStatus.Location = new System.Drawing.Point(28, 162);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(100, 23);
            this.labStatus.TabIndex = 101;
            this.labStatus.Text = "Status";
            // 
            // txtSpNo2
            // 
            this.txtSpNo2.BackColor = System.Drawing.Color.White;
            this.txtSpNo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpNo2.Location = new System.Drawing.Point(283, 48);
            this.txtSpNo2.Name = "txtSpNo2";
            this.txtSpNo2.Size = new System.Drawing.Size(130, 23);
            this.txtSpNo2.TabIndex = 2;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(131, 78);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(130, 23);
            this.txtBrand.TabIndex = 3;
            // 
            // comboDropDownList
            // 
            this.comboDropDownList.BackColor = System.Drawing.Color.White;
            this.comboDropDownList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownList.FormattingEnabled = true;
            this.comboDropDownList.IsSupportUnselect = true;
            this.comboDropDownList.Location = new System.Drawing.Point(131, 162);
            this.comboDropDownList.Name = "comboDropDownList";
            this.comboDropDownList.OldText = "";
            this.comboDropDownList.Size = new System.Drawing.Size(130, 24);
            this.comboDropDownList.TabIndex = 6;
            this.comboDropDownList.Type = "Pms_AVOStatus";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(262, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 17);
            this.label1.TabIndex = 105;
            this.label1.Text = "～";
            // 
            // labM
            // 
            this.labM.Location = new System.Drawing.Point(28, 134);
            this.labM.Name = "labM";
            this.labM.Size = new System.Drawing.Size(100, 23);
            this.labM.TabIndex = 106;
            this.labM.Text = "M";
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(131, 134);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(130, 23);
            this.txtMdivision.TabIndex = 5;
            // 
            // R14
            // 
            this.ClientSize = new System.Drawing.Size(524, 222);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.labM);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboDropDownList);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.txtSpNo2);
            this.Controls.Add(this.labStatus);
            this.Controls.Add(this.labBuyerDelivery);
            this.Controls.Add(this.labBrand);
            this.Controls.Add(this.labSPNo);
            this.Controls.Add(this.txtSpNo1);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.dateIssueDate);
            this.Controls.Add(this.labIssueDate);
            this.Name = "R14";
            this.Text = "R14. Avoid Verbal Orders Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labIssueDate, 0);
            this.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.txtSpNo1, 0);
            this.Controls.SetChildIndex(this.labSPNo, 0);
            this.Controls.SetChildIndex(this.labBrand, 0);
            this.Controls.SetChildIndex(this.labBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labStatus, 0);
            this.Controls.SetChildIndex(this.txtSpNo2, 0);
            this.Controls.SetChildIndex(this.txtBrand, 0);
            this.Controls.SetChildIndex(this.comboDropDownList, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.labM, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labIssueDate;
        private Win.UI.DateRange dateIssueDate;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.TextBox txtSpNo1;
        private Win.UI.Label labSPNo;
        private Win.UI.Label labBrand;
        private Win.UI.Label labBuyerDelivery;
        private Win.UI.Label labStatus;
        private Win.UI.TextBox txtSpNo2;
        private Class.Txtbrand txtBrand;
        private Class.ComboDropDownList comboDropDownList;
        private System.Windows.Forms.Label label1;
        private Win.UI.Label labM;
        private Class.TxtMdivision txtMdivision;
    }
}
