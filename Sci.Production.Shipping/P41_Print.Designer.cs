namespace Sci.Production.Shipping
{
    partial class P41_Print
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
            this.labelDate = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.dateDate = new Sci.Win.UI.DateRange();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtInvNo = new Ict.Win.UI.TextBox();
            this.txtDeclarationNo = new Ict.Win.UI.TextBox();
            this.comboDeclarationStatus = new Ict.Win.UI.ComboBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(435, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(435, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(435, 84);
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(19, 12);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(122, 23);
            this.labelDate.TabIndex = 94;
            this.labelDate.Text = "Date";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(19, 48);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(122, 23);
            this.labelBrand.TabIndex = 95;
            this.labelBrand.Text = "Brand";
            // 
            // dateDate
            // 
            // 
            // 
            // 
            this.dateDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateDate.DateBox1.Name = "";
            this.dateDate.DateBox1.Size = new System.Drawing.Size(114, 23);
            this.dateDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateDate.DateBox2.Location = new System.Drawing.Point(136, 0);
            this.dateDate.DateBox2.Name = "";
            this.dateDate.DateBox2.Size = new System.Drawing.Size(114, 23);
            this.dateDate.DateBox2.TabIndex = 1;
            this.dateDate.IsRequired = false;
            this.dateDate.Location = new System.Drawing.Point(144, 12);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(251, 23);
            this.dateDate.TabIndex = 96;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(144, 48);
            this.txtbrand.MyDocumentdName = null;
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(92, 23);
            this.txtbrand.TabIndex = 97;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 23);
            this.label1.TabIndex = 98;
            this.label1.Text = "Inv No.";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(19, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 23);
            this.label2.TabIndex = 99;
            this.label2.Text = "Declaration No.";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(19, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 23);
            this.label3.TabIndex = 100;
            this.label3.Text = "Declaration Status";
            // 
            // txtInvNo
            // 
            this.txtInvNo.BackColor = System.Drawing.Color.White;
            this.txtInvNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvNo.Location = new System.Drawing.Point(144, 84);
            this.txtInvNo.Name = "txtInvNo";
            this.txtInvNo.Size = new System.Drawing.Size(149, 23);
            this.txtInvNo.TabIndex = 101;
            // 
            // txtDeclarationNo
            // 
            this.txtDeclarationNo.BackColor = System.Drawing.Color.White;
            this.txtDeclarationNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDeclarationNo.Location = new System.Drawing.Point(144, 118);
            this.txtDeclarationNo.Name = "txtDeclarationNo";
            this.txtDeclarationNo.Size = new System.Drawing.Size(149, 23);
            this.txtDeclarationNo.TabIndex = 102;
            // 
            // comboDeclarationStatus
            // 
            this.comboDeclarationStatus.BackColor = System.Drawing.Color.White;
            this.comboDeclarationStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDeclarationStatus.FormattingEnabled = true;
            this.comboDeclarationStatus.IsSupportUnselect = true;
            this.comboDeclarationStatus.Location = new System.Drawing.Point(144, 155);
            this.comboDeclarationStatus.Name = "comboDeclarationStatus";
            this.comboDeclarationStatus.Size = new System.Drawing.Size(121, 24);
            this.comboDeclarationStatus.TabIndex = 103;
            // 
            // P41_Print
            // 
            this.ClientSize = new System.Drawing.Size(527, 230);
            this.Controls.Add(this.comboDeclarationStatus);
            this.Controls.Add(this.txtDeclarationNo);
            this.Controls.Add(this.txtInvNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.dateDate);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelDate);
            this.IsSupportToPrint = false;
            this.Name = "P41_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelDate, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.dateDate, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtInvNo, 0);
            this.Controls.SetChildIndex(this.txtDeclarationNo, 0);
            this.Controls.SetChildIndex(this.comboDeclarationStatus, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelDate;
        private Win.UI.Label labelBrand;
        private Win.UI.DateRange dateDate;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Ict.Win.UI.TextBox txtInvNo;
        private Ict.Win.UI.TextBox txtDeclarationNo;
        private Ict.Win.UI.ComboBox comboDeclarationStatus;
    }
}
