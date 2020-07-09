namespace Sci.Production.Sewing
{
    partial class R06
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
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtsp_from = new Sci.Win.UI.TextBox();
            this.txtsp_to = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.txtMdivision1 = new Sci.Production.Class.TxtMdivision();
            this.dateOutput = new Sci.Win.UI.DateRange();
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
            this.labelBuyerDelivery.Text = "OutputDate";
            // 
            // labelSp
            // 
            this.labelSp.Location = new System.Drawing.Point(23, 156);
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
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(23, 84);
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
            this.txtfactory.Location = new System.Drawing.Point(115, 84);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 2;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(114, 120);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 3;
            // 
            // txtsp_from
            // 
            this.txtsp_from.BackColor = System.Drawing.Color.White;
            this.txtsp_from.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtsp_from.Location = new System.Drawing.Point(114, 156);
            this.txtsp_from.Name = "txtsp_from";
            this.txtsp_from.Size = new System.Drawing.Size(128, 23);
            this.txtsp_from.TabIndex = 4;
            // 
            // txtsp_to
            // 
            this.txtsp_to.BackColor = System.Drawing.Color.White;
            this.txtsp_to.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtsp_to.Location = new System.Drawing.Point(269, 156);
            this.txtsp_to.Name = "txtsp_to";
            this.txtsp_to.Size = new System.Drawing.Size(128, 23);
            this.txtsp_to.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(245, 156);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 23);
            this.label9.TabIndex = 106;
            this.label9.Text = "～";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(23, 48);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(88, 23);
            this.labelM.TabIndex = 107;
            this.labelM.Text = "M";
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(115, 48);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 1;
            // 
            // dateOutput
            // 
            // 
            // 
            // 
            this.dateOutput.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateOutput.DateBox1.Name = "";
            this.dateOutput.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateOutput.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateOutput.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateOutput.DateBox2.Name = "";
            this.dateOutput.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateOutput.DateBox2.TabIndex = 1;
            this.dateOutput.IsRequired = false;
            this.dateOutput.Location = new System.Drawing.Point(117, 12);
            this.dateOutput.Name = "dateOutput";
            this.dateOutput.Size = new System.Drawing.Size(280, 23);
            this.dateOutput.TabIndex = 0;
            // 
            // R06
            // 
            this.ClientSize = new System.Drawing.Size(525, 219);
            this.Controls.Add(this.dateOutput);
            this.Controls.Add(this.txtMdivision1);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtsp_to);
            this.Controls.Add(this.txtsp_from);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelSp);
            this.Controls.Add(this.labelBuyerDelivery);
            this.DefaultControl = "dateOoutputDate";
            this.DefaultControlForEdit = "dateOoutputDate";
            this.IsSupportToPrint = false;
            this.Name = "R06";
            this.Text = "R06. Sewing daily output list by Article/Size";
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labelSp, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtsp_from, 0);
            this.Controls.SetChildIndex(this.txtsp_to, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.dateOutput, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelSp;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelFactory;
        private Class.Txtfactory txtfactory;
        private Class.Txtbrand txtbrand;
        private Win.UI.TextBox txtsp_from;
        private Win.UI.TextBox txtsp_to;
        private Win.UI.Label label9;
        private Win.UI.Label labelM;
        private Class.TxtMdivision txtMdivision1;
        private Win.UI.DateRange dateOutput;
    }
}
