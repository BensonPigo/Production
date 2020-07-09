namespace Sci.Production.Sewing
{
    partial class R05
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
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.labelSp = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelStatus = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.cb_status = new Sci.Win.UI.ComboBox();
            this.txtsp_from = new Sci.Win.UI.TextBox();
            this.txtsp_to = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(433, 12);
            this.print.TabIndex = 6;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(433, 48);
            this.toexcel.TabIndex = 7;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(433, 84);
            this.close.TabIndex = 8;
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(23, 12);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(88, 23);
            this.labelBuyerDelivery.TabIndex = 94;
            this.labelBuyerDelivery.Text = "BuyerDelivery";
            // 
            // labelSp
            // 
            this.labelSp.Location = new System.Drawing.Point(23, 84);
            this.labelSp.Name = "labelSp";
            this.labelSp.Size = new System.Drawing.Size(88, 23);
            this.labelSp.TabIndex = 96;
            this.labelSp.Text = "SP#";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(23, 120);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(88, 23);
            this.labelBrand.TabIndex = 98;
            this.labelBrand.Text = "Brand";
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(23, 154);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(88, 23);
            this.labelStatus.TabIndex = 99;
            this.labelStatus.Text = "Status";
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
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(115, 12);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 0;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(23, 48);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(88, 23);
            this.labelFactory.TabIndex = 97;
            this.labelFactory.Text = "Factory";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(115, 48);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 100;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(115, 120);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 101;
            // 
            // cb_status
            // 
            this.cb_status.BackColor = System.Drawing.Color.White;
            this.cb_status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cb_status.FormattingEnabled = true;
            this.cb_status.IsSupportUnselect = true;
            this.cb_status.Items.AddRange(new object[] {
            "Unfinished",
            "Finished",
            "Excess",
            "All"});
            this.cb_status.Location = new System.Drawing.Point(115, 154);
            this.cb_status.Name = "cb_status";
            this.cb_status.Size = new System.Drawing.Size(121, 24);
            this.cb_status.TabIndex = 102;
            // 
            // txtsp_from
            // 
            this.txtsp_from.BackColor = System.Drawing.Color.White;
            this.txtsp_from.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtsp_from.Location = new System.Drawing.Point(115, 84);
            this.txtsp_from.Name = "txtsp_from";
            this.txtsp_from.Size = new System.Drawing.Size(128, 23);
            this.txtsp_from.TabIndex = 103;
            // 
            // txtsp_to
            // 
            this.txtsp_to.BackColor = System.Drawing.Color.White;
            this.txtsp_to.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtsp_to.Location = new System.Drawing.Point(270, 84);
            this.txtsp_to.Name = "txtsp_to";
            this.txtsp_to.Size = new System.Drawing.Size(128, 23);
            this.txtsp_to.TabIndex = 104;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(246, 84);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 23);
            this.label9.TabIndex = 106;
            this.label9.Text = "～";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // R05
            // 
            this.ClientSize = new System.Drawing.Size(525, 219);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtsp_to);
            this.Controls.Add(this.txtsp_from);
            this.Controls.Add(this.cb_status);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelSp);
            this.Controls.Add(this.labelBuyerDelivery);
            this.DefaultControl = "dateOoutputDate";
            this.DefaultControlForEdit = "dateOoutputDate";
            this.IsSupportToPrint = false;
            this.Name = "R05";
            this.Text = "R05. Garment Order Allocate Output Report";
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labelSp, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelStatus, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.cb_status, 0);
            this.Controls.SetChildIndex(this.txtsp_from, 0);
            this.Controls.SetChildIndex(this.txtsp_to, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelSp;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelStatus;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Label labelFactory;
        private Class.Txtfactory txtfactory;
        private Class.Txtbrand txtbrand;
        private Win.UI.ComboBox cb_status;
        private Win.UI.TextBox txtsp_from;
        private Win.UI.TextBox txtsp_to;
        private Win.UI.Label label9;
    }
}
