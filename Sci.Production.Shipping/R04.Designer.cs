﻿namespace Sci.Production.Shipping
{
    partial class R04
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
            this.labelEstimatePullout = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelOrderNo = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.checkIncludeLocalOrder = new Sci.Win.UI.CheckBox();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.dateEstimatePullout = new Sci.Win.UI.DateRange();
            this.txtbrand = new Sci.Production.Class.txtbrand();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.txtOrderNo = new Sci.Win.UI.TextBox();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(448, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(448, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(448, 84);
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Lines = 0;
            this.labelBuyerDelivery.Location = new System.Drawing.Point(13, 12);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(111, 23);
            this.labelBuyerDelivery.TabIndex = 94;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // labelEstimatePullout
            // 
            this.labelEstimatePullout.Lines = 0;
            this.labelEstimatePullout.Location = new System.Drawing.Point(13, 48);
            this.labelEstimatePullout.Name = "labelEstimatePullout";
            this.labelEstimatePullout.Size = new System.Drawing.Size(111, 23);
            this.labelEstimatePullout.TabIndex = 95;
            this.labelEstimatePullout.Text = "Estimate Pull-out";
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(13, 84);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(111, 23);
            this.labelBrand.TabIndex = 96;
            this.labelBrand.Text = "Brand";
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(13, 120);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(111, 23);
            this.labelM.TabIndex = 97;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(13, 156);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(111, 23);
            this.labelFactory.TabIndex = 98;
            this.labelFactory.Text = "Factory";
            // 
            // labelOrderNo
            // 
            this.labelOrderNo.Lines = 0;
            this.labelOrderNo.Location = new System.Drawing.Point(13, 193);
            this.labelOrderNo.Name = "labelOrderNo";
            this.labelOrderNo.Size = new System.Drawing.Size(111, 23);
            this.labelOrderNo.TabIndex = 99;
            this.labelOrderNo.Text = "Order# (NGC#)";
            // 
            // labelCategory
            // 
            this.labelCategory.Lines = 0;
            this.labelCategory.Location = new System.Drawing.Point(13, 229);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(111, 23);
            this.labelCategory.TabIndex = 100;
            this.labelCategory.Text = "Category";
            // 
            // checkIncludeLocalOrder
            // 
            this.checkIncludeLocalOrder.AutoSize = true;
            this.checkIncludeLocalOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIncludeLocalOrder.Location = new System.Drawing.Point(13, 261);
            this.checkIncludeLocalOrder.Name = "checkIncludeLocalOrder";
            this.checkIncludeLocalOrder.Size = new System.Drawing.Size(151, 21);
            this.checkIncludeLocalOrder.TabIndex = 101;
            this.checkIncludeLocalOrder.Text = "Include Local Order";
            this.checkIncludeLocalOrder.UseVisualStyleBackColor = true;
            // 
            // dateBuyerDelivery
            // 
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(128, 12);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 102;
            // 
            // dateEstimatePullout
            // 
            this.dateEstimatePullout.IsRequired = false;
            this.dateEstimatePullout.Location = new System.Drawing.Point(128, 48);
            this.dateEstimatePullout.Name = "dateEstimatePullout";
            this.dateEstimatePullout.Size = new System.Drawing.Size(280, 23);
            this.dateEstimatePullout.TabIndex = 103;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(128, 84);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(84, 23);
            this.txtbrand.TabIndex = 104;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(128, 120);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(84, 24);
            this.comboM.TabIndex = 105;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(128, 156);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(84, 24);
            this.comboFactory.TabIndex = 106;
            // 
            // txtOrderNo
            // 
            this.txtOrderNo.BackColor = System.Drawing.Color.White;
            this.txtOrderNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtOrderNo.Location = new System.Drawing.Point(128, 193);
            this.txtOrderNo.Name = "txtOrderNo";
            this.txtOrderNo.Size = new System.Drawing.Size(238, 23);
            this.txtOrderNo.TabIndex = 107;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(128, 229);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.Size = new System.Drawing.Size(121, 24);
            this.comboCategory.TabIndex = 108;
            // 
            // R04
            // 
            this.ClientSize = new System.Drawing.Size(540, 314);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.txtOrderNo);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.dateEstimatePullout);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.checkIncludeLocalOrder);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelOrderNo);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelEstimatePullout);
            this.Controls.Add(this.labelBuyerDelivery);
            this.IsSupportToPrint = false;
            this.Name = "R04";
            this.Text = "R04. Estimate/Outstanding Shipment Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labelEstimatePullout, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelOrderNo, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.checkIncludeLocalOrder, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.dateEstimatePullout, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.txtOrderNo, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelEstimatePullout;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelOrderNo;
        private Win.UI.Label labelCategory;
        private Win.UI.CheckBox checkIncludeLocalOrder;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.DateRange dateEstimatePullout;
        private Class.txtbrand txtbrand;
        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.TextBox txtOrderNo;
        private Win.UI.ComboBox comboCategory;
    }
}
