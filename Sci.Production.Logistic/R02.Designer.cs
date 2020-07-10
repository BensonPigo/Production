namespace Sci.Production.Logistic
{
    partial class R02
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
            this.labelPONo = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelLocation = new Sci.Win.UI.Label();
            this.txtPONoStart = new Sci.Win.UI.TextBox();
            this.txtPONoEnd = new Sci.Win.UI.TextBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.txtcloglocationLocationStart = new Sci.Production.Class.Txtcloglocation();
            this.txtcloglocationLocationEnd = new Sci.Production.Class.Txtcloglocation();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.comboCancel = new Sci.Win.UI.ComboBox();
            this.label1 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(444, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(444, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(444, 84);
            // 
            // labelPONo
            // 
            this.labelPONo.Location = new System.Drawing.Point(13, 12);
            this.labelPONo.Name = "labelPONo";
            this.labelPONo.Size = new System.Drawing.Size(95, 23);
            this.labelPONo.TabIndex = 94;
            this.labelPONo.Text = "PO#";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(13, 47);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(95, 23);
            this.labelSPNo.TabIndex = 95;
            this.labelSPNo.Text = "SP#";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 117);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(95, 23);
            this.labelBrand.TabIndex = 96;
            this.labelBrand.Text = "Brand";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 152);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(95, 23);
            this.labelM.TabIndex = 97;
            this.labelM.Text = "M";
            // 
            // labelLocation
            // 
            this.labelLocation.Location = new System.Drawing.Point(13, 187);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(95, 23);
            this.labelLocation.TabIndex = 98;
            this.labelLocation.Text = "Location";
            // 
            // txtPONoStart
            // 
            this.txtPONoStart.BackColor = System.Drawing.Color.White;
            this.txtPONoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPONoStart.Location = new System.Drawing.Point(111, 12);
            this.txtPONoStart.Name = "txtPONoStart";
            this.txtPONoStart.Size = new System.Drawing.Size(130, 23);
            this.txtPONoStart.TabIndex = 99;
            // 
            // txtPONoEnd
            // 
            this.txtPONoEnd.BackColor = System.Drawing.Color.White;
            this.txtPONoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPONoEnd.Location = new System.Drawing.Point(269, 12);
            this.txtPONoEnd.Name = "txtPONoEnd";
            this.txtPONoEnd.Size = new System.Drawing.Size(130, 23);
            this.txtPONoEnd.TabIndex = 100;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(111, 47);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(130, 23);
            this.txtSPNoStart.TabIndex = 101;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(269, 47);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(130, 23);
            this.txtSPNoEnd.TabIndex = 102;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(111, 117);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(100, 23);
            this.txtbrand.TabIndex = 103;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(111, 152);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(80, 24);
            this.comboM.TabIndex = 104;
            // 
            // txtcloglocationLocationStart
            // 
            this.txtcloglocationLocationStart.BackColor = System.Drawing.Color.White;
            this.txtcloglocationLocationStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcloglocationLocationStart.IsSupportSytsemContextMenu = false;
            this.txtcloglocationLocationStart.Location = new System.Drawing.Point(111, 187);
            this.txtcloglocationLocationStart.MDivisionObjectName = this.comboM;
            this.txtcloglocationLocationStart.Name = "txtcloglocationLocationStart";
            this.txtcloglocationLocationStart.Size = new System.Drawing.Size(80, 23);
            this.txtcloglocationLocationStart.TabIndex = 105;
            // 
            // txtcloglocationLocationEnd
            // 
            this.txtcloglocationLocationEnd.BackColor = System.Drawing.Color.White;
            this.txtcloglocationLocationEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcloglocationLocationEnd.IsSupportSytsemContextMenu = false;
            this.txtcloglocationLocationEnd.Location = new System.Drawing.Point(215, 187);
            this.txtcloglocationLocationEnd.MDivisionObjectName = this.comboM;
            this.txtcloglocationLocationEnd.Name = "txtcloglocationLocationEnd";
            this.txtcloglocationLocationEnd.Size = new System.Drawing.Size(80, 23);
            this.txtcloglocationLocationEnd.TabIndex = 106;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(245, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 23);
            this.label6.TabIndex = 107;
            this.label6.Text = "～";
            this.label6.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            this.label6.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label6.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(245, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(19, 23);
            this.label7.TabIndex = 108;
            this.label7.Text = "～";
            this.label7.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            this.label7.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(192, 187);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 23);
            this.label8.TabIndex = 109;
            this.label8.Text = "～";
            this.label8.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label8.TextStyle.Color = System.Drawing.Color.Black;
            this.label8.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label8.TextStyle.GradientColor = System.Drawing.Color.Black;
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
            this.dateBuyerDelivery.Location = new System.Drawing.Point(111, 82);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 111;
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(11, 82);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(97, 23);
            this.labelBuyerDelivery.TabIndex = 110;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // comboCancel
            // 
            this.comboCancel.BackColor = System.Drawing.Color.White;
            this.comboCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCancel.FormattingEnabled = true;
            this.comboCancel.IsSupportUnselect = true;
            this.comboCancel.Items.AddRange(new object[] {
            "",
            "Y",
            "N"});
            this.comboCancel.Location = new System.Drawing.Point(111, 221);
            this.comboCancel.Name = "comboCancel";
            this.comboCancel.OldText = "";
            this.comboCancel.Size = new System.Drawing.Size(80, 24);
            this.comboCancel.TabIndex = 113;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 221);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 23);
            this.label1.TabIndex = 112;
            this.label1.Text = "Cancel Order";
            // 
            // R02
            // 
            this.ClientSize = new System.Drawing.Size(536, 294);
            this.Controls.Add(this.comboCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.labelBuyerDelivery);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtcloglocationLocationEnd);
            this.Controls.Add(this.txtcloglocationLocationStart);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.txtSPNoEnd);
            this.Controls.Add(this.txtSPNoStart);
            this.Controls.Add(this.txtPONoEnd);
            this.Controls.Add(this.txtPONoStart);
            this.Controls.Add(this.labelLocation);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.labelPONo);
            this.IsSupportToPrint = false;
            this.Name = "R02";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R02. Clog Audit List";
            this.Controls.SetChildIndex(this.labelPONo, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelLocation, 0);
            this.Controls.SetChildIndex(this.txtPONoStart, 0);
            this.Controls.SetChildIndex(this.txtPONoEnd, 0);
            this.Controls.SetChildIndex(this.txtSPNoStart, 0);
            this.Controls.SetChildIndex(this.txtSPNoEnd, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.txtcloglocationLocationStart, 0);
            this.Controls.SetChildIndex(this.txtcloglocationLocationEnd, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.comboCancel, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelPONo;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelM;
        private Win.UI.Label labelLocation;
        private Win.UI.TextBox txtPONoStart;
        private Win.UI.TextBox txtPONoEnd;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.TextBox txtSPNoEnd;
        private Class.Txtbrand txtbrand;
        private Win.UI.ComboBox comboM;
        private Class.Txtcloglocation txtcloglocationLocationStart;
        private Class.Txtcloglocation txtcloglocationLocationEnd;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.Label label8;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.ComboBox comboCancel;
        private Win.UI.Label label1;
    }
}
