﻿namespace Sci.Production.Subcon
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
            this.lbFactory = new Sci.Win.UI.Label();
            this.lbArtworkType = new Sci.Win.UI.Label();
            this.lbFarmOutDate = new Sci.Win.UI.Label();
            this.lbSupplier = new Sci.Win.UI.Label();
            this.lbMasterSP = new Sci.Win.UI.Label();
            this.lbStyle = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.dateFarmOutDate = new Sci.Win.UI.DateRange();
            this.txtMasterSPNo = new Sci.Win.UI.TextBox();
            this.lbM = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.lbSCIDelivery = new Sci.Win.UI.Label();
            this.lbBundleNo = new Sci.Win.UI.Label();
            this.txtBundleNoStart = new Sci.Win.UI.TextBox();
            this.txtBundleNoEnd = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.txtM = new Sci.Production.Class.txtMdivision();
            this.txtstyle = new Sci.Production.Class.txtstyle();
            this.txtsubconSupplier = new Sci.Production.Class.txtsubcon();
            this.txtartworktype_ftyArtworkType = new Sci.Production.Class.txtartworktype_fty();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(430, 12);
            this.print.TabIndex = 10;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(430, 48);
            this.toexcel.TabIndex = 11;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(430, 84);
            this.close.TabIndex = 12;
            // 
            // lbFactory
            // 
            this.lbFactory.Lines = 0;
            this.lbFactory.Location = new System.Drawing.Point(13, 189);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(98, 23);
            this.lbFactory.TabIndex = 94;
            this.lbFactory.Text = "Factory";
            // 
            // lbArtworkType
            // 
            this.lbArtworkType.Lines = 0;
            this.lbArtworkType.Location = new System.Drawing.Point(13, 118);
            this.lbArtworkType.Name = "lbArtworkType";
            this.lbArtworkType.Size = new System.Drawing.Size(98, 23);
            this.lbArtworkType.TabIndex = 95;
            this.lbArtworkType.Text = "Artwork Type";
            // 
            // lbFarmOutDate
            // 
            this.lbFarmOutDate.Lines = 0;
            this.lbFarmOutDate.Location = new System.Drawing.Point(13, 12);
            this.lbFarmOutDate.Name = "lbFarmOutDate";
            this.lbFarmOutDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lbFarmOutDate.RectStyle.BorderWidth = 1F;
            this.lbFarmOutDate.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lbFarmOutDate.RectStyle.ExtBorderWidth = 1F;
            this.lbFarmOutDate.Size = new System.Drawing.Size(98, 23);
            this.lbFarmOutDate.TabIndex = 96;
            this.lbFarmOutDate.Text = "Farm Out Date";
            this.lbFarmOutDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lbFarmOutDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lbSupplier
            // 
            this.lbSupplier.Lines = 0;
            this.lbSupplier.Location = new System.Drawing.Point(13, 226);
            this.lbSupplier.Name = "lbSupplier";
            this.lbSupplier.Size = new System.Drawing.Size(98, 23);
            this.lbSupplier.TabIndex = 98;
            this.lbSupplier.Text = "Supplier";
            // 
            // lbMasterSP
            // 
            this.lbMasterSP.Lines = 0;
            this.lbMasterSP.Location = new System.Drawing.Point(13, 262);
            this.lbMasterSP.Name = "lbMasterSP";
            this.lbMasterSP.Size = new System.Drawing.Size(98, 23);
            this.lbMasterSP.TabIndex = 99;
            this.lbMasterSP.Text = "Master SP#";
            // 
            // lbStyle
            // 
            this.lbStyle.Lines = 0;
            this.lbStyle.Location = new System.Drawing.Point(13, 298);
            this.lbStyle.Name = "lbStyle";
            this.lbStyle.Size = new System.Drawing.Size(98, 23);
            this.lbStyle.TabIndex = 100;
            this.lbStyle.Text = "Style";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(114, 188);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 6;
            // 
            // dateFarmOutDate
            // 
            this.dateFarmOutDate.IsRequired = false;
            this.dateFarmOutDate.Location = new System.Drawing.Point(115, 12);
            this.dateFarmOutDate.Name = "dateFarmOutDate";
            this.dateFarmOutDate.Size = new System.Drawing.Size(280, 23);
            this.dateFarmOutDate.TabIndex = 0;
            // 
            // txtMasterSPNo
            // 
            this.txtMasterSPNo.BackColor = System.Drawing.Color.White;
            this.txtMasterSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMasterSPNo.Location = new System.Drawing.Point(115, 262);
            this.txtMasterSPNo.MaxLength = 13;
            this.txtMasterSPNo.Name = "txtMasterSPNo";
            this.txtMasterSPNo.Size = new System.Drawing.Size(130, 23);
            this.txtMasterSPNo.TabIndex = 8;
            // 
            // lbM
            // 
            this.lbM.Lines = 0;
            this.lbM.Location = new System.Drawing.Point(13, 154);
            this.lbM.Name = "lbM";
            this.lbM.Size = new System.Drawing.Size(98, 23);
            this.lbM.TabIndex = 103;
            this.lbM.Text = "M";
            // 
            // dateSCIDelivery
            // 
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(115, 84);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 3;
            // 
            // lbSCIDelivery
            // 
            this.lbSCIDelivery.Lines = 0;
            this.lbSCIDelivery.Location = new System.Drawing.Point(13, 84);
            this.lbSCIDelivery.Name = "lbSCIDelivery";
            this.lbSCIDelivery.Size = new System.Drawing.Size(98, 23);
            this.lbSCIDelivery.TabIndex = 105;
            this.lbSCIDelivery.Text = "SCI Delivery";
            // 
            // lbBundleNo
            // 
            this.lbBundleNo.Lines = 0;
            this.lbBundleNo.Location = new System.Drawing.Point(13, 48);
            this.lbBundleNo.Name = "lbBundleNo";
            this.lbBundleNo.Size = new System.Drawing.Size(98, 23);
            this.lbBundleNo.TabIndex = 106;
            this.lbBundleNo.Text = "Bundle No";
            // 
            // txtBundleNoStart
            // 
            this.txtBundleNoStart.BackColor = System.Drawing.Color.White;
            this.txtBundleNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBundleNoStart.Location = new System.Drawing.Point(115, 48);
            this.txtBundleNoStart.Name = "txtBundleNoStart";
            this.txtBundleNoStart.Size = new System.Drawing.Size(100, 23);
            this.txtBundleNoStart.TabIndex = 1;
            // 
            // txtBundleNoEnd
            // 
            this.txtBundleNoEnd.BackColor = System.Drawing.Color.White;
            this.txtBundleNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBundleNoEnd.Location = new System.Drawing.Point(238, 48);
            this.txtBundleNoEnd.Name = "txtBundleNoEnd";
            this.txtBundleNoEnd.Size = new System.Drawing.Size(100, 23);
            this.txtBundleNoEnd.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(217, 47);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 23);
            this.label10.TabIndex = 109;
            this.label10.Text = "～";
            // 
            // txtM
            // 
            this.txtM.BackColor = System.Drawing.Color.White;
            this.txtM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtM.Location = new System.Drawing.Point(114, 154);
            this.txtM.Name = "txtM";
            this.txtM.Size = new System.Drawing.Size(66, 23);
            this.txtM.TabIndex = 5;
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(115, 298);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.Size = new System.Drawing.Size(170, 23);
            this.txtstyle.TabIndex = 9;
            // 
            // txtsubconSupplier
            // 
            this.txtsubconSupplier.DisplayBox1Binding = "";
            this.txtsubconSupplier.IsIncludeJunk = true;
            this.txtsubconSupplier.Location = new System.Drawing.Point(115, 226);
            this.txtsubconSupplier.Name = "txtsubconSupplier";
            this.txtsubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtsubconSupplier.TabIndex = 7;
            this.txtsubconSupplier.TextBox1Binding = "";
            // 
            // txtartworktype_ftyArtworkType
            // 
            this.txtartworktype_ftyArtworkType.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftyArtworkType.cClassify = "";
            this.txtartworktype_ftyArtworkType.cSubprocess = "Y";
            this.txtartworktype_ftyArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftyArtworkType.Location = new System.Drawing.Point(114, 118);
            this.txtartworktype_ftyArtworkType.Name = "txtartworktype_ftyArtworkType";
            this.txtartworktype_ftyArtworkType.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_ftyArtworkType.TabIndex = 4;
            // 
            // R06
            // 
            this.ClientSize = new System.Drawing.Size(522, 367);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtBundleNoEnd);
            this.Controls.Add(this.txtBundleNoStart);
            this.Controls.Add(this.lbBundleNo);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.lbSCIDelivery);
            this.Controls.Add(this.lbM);
            this.Controls.Add(this.txtM);
            this.Controls.Add(this.txtstyle);
            this.Controls.Add(this.txtsubconSupplier);
            this.Controls.Add(this.txtartworktype_ftyArtworkType);
            this.Controls.Add(this.txtMasterSPNo);
            this.Controls.Add(this.dateFarmOutDate);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.lbStyle);
            this.Controls.Add(this.lbMasterSP);
            this.Controls.Add(this.lbSupplier);
            this.Controls.Add(this.lbFarmOutDate);
            this.Controls.Add(this.lbArtworkType);
            this.Controls.Add(this.lbFactory);
            this.DefaultControl = "FarmOutDate";
            this.DefaultControlForEdit = "FarmOutDate";
            this.IsSupportToPrint = false;
            this.Name = "R06";
            this.Text = "R06. Cutpart Farm Out Tracking List";
            this.Controls.SetChildIndex(this.lbFactory, 0);
            this.Controls.SetChildIndex(this.lbArtworkType, 0);
            this.Controls.SetChildIndex(this.lbFarmOutDate, 0);
            this.Controls.SetChildIndex(this.lbSupplier, 0);
            this.Controls.SetChildIndex(this.lbMasterSP, 0);
            this.Controls.SetChildIndex(this.lbStyle, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.dateFarmOutDate, 0);
            this.Controls.SetChildIndex(this.txtMasterSPNo, 0);
            this.Controls.SetChildIndex(this.txtartworktype_ftyArtworkType, 0);
            this.Controls.SetChildIndex(this.txtsubconSupplier, 0);
            this.Controls.SetChildIndex(this.txtstyle, 0);
            this.Controls.SetChildIndex(this.txtM, 0);
            this.Controls.SetChildIndex(this.lbM, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.lbSCIDelivery, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.lbBundleNo, 0);
            this.Controls.SetChildIndex(this.txtBundleNoStart, 0);
            this.Controls.SetChildIndex(this.txtBundleNoEnd, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbFactory;
        private Win.UI.Label lbArtworkType;
        private Win.UI.Label lbFarmOutDate;
        private Win.UI.Label lbSupplier;
        private Win.UI.Label lbMasterSP;
        private Win.UI.Label lbStyle;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.DateRange dateFarmOutDate;
        private Win.UI.TextBox txtMasterSPNo;
        private Class.txtartworktype_fty txtartworktype_ftyArtworkType;
        private Class.txtsubcon txtsubconSupplier;
        private Class.txtstyle txtstyle;
        private Class.txtMdivision txtM;
        private Win.UI.Label lbM;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.Label lbSCIDelivery;
        private Win.UI.Label lbBundleNo;
        private Win.UI.TextBox txtBundleNoStart;
        private Win.UI.TextBox txtBundleNoEnd;
        private Win.UI.Label label10;
    }
}
