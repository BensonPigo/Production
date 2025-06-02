namespace Sci.Production.Shipping
{
    partial class R48
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
            this.labelBrand = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.lbSCIDelivery = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtCustomsContract = new Sci.Production.Class.TxtCustomsContract();
            this.lbConsignee = new Sci.Win.UI.Label();
            this.txtOrderID = new Sci.Win.UI.TextBox();
            this.lbSP = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(426, 160);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(434, 12);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(434, 48);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(388, 160);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(402, 169);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(414, 167);
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.BackColor = System.Drawing.Color.SkyBlue;
            this.labelBuyerDelivery.Location = new System.Drawing.Point(13, 12);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(102, 23);
            this.labelBuyerDelivery.TabIndex = 94;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            this.labelBuyerDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 154);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(102, 23);
            this.labelBrand.TabIndex = 97;
            this.labelBrand.Text = "Brand";
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
            this.dateBuyerDelivery.Location = new System.Drawing.Point(118, 12);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 102;
            // 
            // lbSCIDelivery
            // 
            this.lbSCIDelivery.BackColor = System.Drawing.Color.SkyBlue;
            this.lbSCIDelivery.Location = new System.Drawing.Point(13, 48);
            this.lbSCIDelivery.Name = "lbSCIDelivery";
            this.lbSCIDelivery.Size = new System.Drawing.Size(102, 23);
            this.lbSCIDelivery.TabIndex = 143;
            this.lbSCIDelivery.Text = "SCI Delivery";
            this.lbSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(118, 48);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 144;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(118, 154);
            this.txtbrand.MyDocumentdName = null;
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(163, 23);
            this.txtbrand.TabIndex = 105;
            // 
            // txtCustomsContract
            // 
            this.txtCustomsContract.BackColor = System.Drawing.Color.White;
            this.txtCustomsContract.CheckDate = false;
            this.txtCustomsContract.CheckStatus = true;
            this.txtCustomsContract.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCustomsContract.Location = new System.Drawing.Point(118, 84);
            this.txtCustomsContract.Name = "txtCustomsContract";
            this.txtCustomsContract.Size = new System.Drawing.Size(163, 23);
            this.txtCustomsContract.TabIndex = 148;
            // 
            // lbConsignee
            // 
            this.lbConsignee.Location = new System.Drawing.Point(13, 84);
            this.lbConsignee.Name = "lbConsignee";
            this.lbConsignee.Size = new System.Drawing.Size(102, 23);
            this.lbConsignee.TabIndex = 147;
            this.lbConsignee.Text = "Contract no.";
            // 
            // txtOrderID
            // 
            this.txtOrderID.BackColor = System.Drawing.Color.White;
            this.txtOrderID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtOrderID.Location = new System.Drawing.Point(118, 121);
            this.txtOrderID.Name = "txtOrderID";
            this.txtOrderID.Size = new System.Drawing.Size(163, 23);
            this.txtOrderID.TabIndex = 145;
            this.txtOrderID.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtOrderID_PopUp);
            // 
            // lbSP
            // 
            this.lbSP.Location = new System.Drawing.Point(13, 121);
            this.lbSP.Name = "lbSP";
            this.lbSP.Size = new System.Drawing.Size(102, 23);
            this.lbSP.TabIndex = 146;
            this.lbSP.Text = "SP#";
            // 
            // R48
            // 
            this.ClientSize = new System.Drawing.Size(526, 215);
            this.Controls.Add(this.txtCustomsContract);
            this.Controls.Add(this.lbConsignee);
            this.Controls.Add(this.txtOrderID);
            this.Controls.Add(this.lbSP);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.lbSCIDelivery);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelBuyerDelivery);
            this.IsSupportToPrint = false;
            this.Name = "R48";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R48. Liquidation Detail Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.lbSCIDelivery, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.lbSP, 0);
            this.Controls.SetChildIndex(this.txtOrderID, 0);
            this.Controls.SetChildIndex(this.lbConsignee, 0);
            this.Controls.SetChildIndex(this.txtCustomsContract, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelBrand;
        private Win.UI.DateRange dateBuyerDelivery;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label lbSCIDelivery;
        private Win.UI.DateRange dateSCIDelivery;
        private Class.TxtCustomsContract txtCustomsContract;
        private Win.UI.Label lbConsignee;
        private Win.UI.TextBox txtOrderID;
        private Win.UI.Label lbSP;
    }
}
