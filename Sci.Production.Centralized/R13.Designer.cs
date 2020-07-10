namespace Sci.Production.Centralized
{
    partial class R13
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.datePulloutDate1 = new Sci.Win.UI.DateBox();
            this.label8 = new Sci.Win.UI.Label();
            this.datePulloutDate2 = new Sci.Win.UI.DateBox();
            this.dateApproveDate1 = new Sci.Win.UI.DateBox();
            this.label9 = new Sci.Win.UI.Label();
            this.dateApproveDate2 = new Sci.Win.UI.DateBox();
            this.dateOnboardDate1 = new Sci.Win.UI.DateBox();
            this.label10 = new Sci.Win.UI.Label();
            this.dateOnboardDate2 = new Sci.Win.UI.DateBox();
            this.txtBrandBrandID = new Sci.Production.Class.Txtbrand();
            this.txtCustCDCustCDID = new Sci.Production.Class.Txtcustcd();
            this.txtCountryDest = new Sci.Production.Class.Txtcountry();
            this.comboShipmodeShipmodeID = new Sci.Production.Class.Txtshipmode();
            this.label11 = new Sci.Win.UI.Label();
            this.txtForwarder = new Sci.Win.UI.TextBox();
            this.label12 = new Sci.Win.UI.Label();
            this.comboRateTypeID = new Sci.Win.UI.ComboBox();
            this.checkExportDetail = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(448, 12);
            this.print.TabIndex = 13;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(448, 48);
            this.toexcel.TabIndex = 14;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(448, 84);
            this.close.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Pullout Date";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 23);
            this.label2.TabIndex = 95;
            this.label2.Text = "Approve Date";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 23);
            this.label3.TabIndex = 96;
            this.label3.Text = "On Board Date";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(13, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 23);
            this.label4.TabIndex = 97;
            this.label4.Text = "Brand";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(13, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 98;
            this.label5.Text = "CUSTCD";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(13, 151);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 99;
            this.label6.Text = "Destination";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(13, 177);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 23);
            this.label7.TabIndex = 100;
            this.label7.Text = "Ship Mode";
            // 
            // datePulloutDate1
            // 
            this.datePulloutDate1.Location = new System.Drawing.Point(125, 13);
            this.datePulloutDate1.Name = "datePulloutDate1";
            this.datePulloutDate1.Size = new System.Drawing.Size(130, 23);
            this.datePulloutDate1.TabIndex = 0;
            this.datePulloutDate1.Validated += new System.EventHandler(this.DatePulloutDate1_Validated);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(262, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(18, 23);
            this.label8.TabIndex = 102;
            this.label8.Text = "~";
            // 
            // datePulloutDate2
            // 
            this.datePulloutDate2.Location = new System.Drawing.Point(284, 13);
            this.datePulloutDate2.Name = "datePulloutDate2";
            this.datePulloutDate2.Size = new System.Drawing.Size(130, 23);
            this.datePulloutDate2.TabIndex = 1;
            // 
            // dateApproveDate1
            // 
            this.dateApproveDate1.Location = new System.Drawing.Point(125, 41);
            this.dateApproveDate1.Name = "dateApproveDate1";
            this.dateApproveDate1.Size = new System.Drawing.Size(130, 23);
            this.dateApproveDate1.TabIndex = 2;
            this.dateApproveDate1.Validated += new System.EventHandler(this.DateApproveDate1_Validated);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(262, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 23);
            this.label9.TabIndex = 105;
            this.label9.Text = "~";
            // 
            // dateApproveDate2
            // 
            this.dateApproveDate2.Location = new System.Drawing.Point(284, 41);
            this.dateApproveDate2.Name = "dateApproveDate2";
            this.dateApproveDate2.Size = new System.Drawing.Size(130, 23);
            this.dateApproveDate2.TabIndex = 3;
            // 
            // dateOnboardDate1
            // 
            this.dateOnboardDate1.Location = new System.Drawing.Point(125, 69);
            this.dateOnboardDate1.Name = "dateOnboardDate1";
            this.dateOnboardDate1.Size = new System.Drawing.Size(130, 23);
            this.dateOnboardDate1.TabIndex = 4;
            this.dateOnboardDate1.Validated += new System.EventHandler(this.DateOnboardDate1_Validated);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(262, 69);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 23);
            this.label10.TabIndex = 108;
            this.label10.Text = "~";
            // 
            // dateOnboardDate2
            // 
            this.dateOnboardDate2.Location = new System.Drawing.Point(284, 69);
            this.dateOnboardDate2.Name = "dateOnboardDate2";
            this.dateOnboardDate2.Size = new System.Drawing.Size(130, 23);
            this.dateOnboardDate2.TabIndex = 5;
            // 
            // txtBrandBrandID
            // 
            this.txtBrandBrandID.BackColor = System.Drawing.Color.White;
            this.txtBrandBrandID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrandBrandID.Location = new System.Drawing.Point(125, 96);
            this.txtBrandBrandID.Name = "txtBrandBrandID";
            this.txtBrandBrandID.Size = new System.Drawing.Size(90, 23);
            this.txtBrandBrandID.TabIndex = 6;
            // 
            // txtCustCDCustCDID
            // 
            this.txtCustCDCustCDID.BackColor = System.Drawing.Color.White;
            this.txtCustCDCustCDID.BrandObjectName = null;
            this.txtCustCDCustCDID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCustCDCustCDID.Location = new System.Drawing.Point(125, 123);
            this.txtCustCDCustCDID.Name = "txtCustCDCustCDID";
            this.txtCustCDCustCDID.Size = new System.Drawing.Size(130, 23);
            this.txtCustCDCustCDID.TabIndex = 7;
            // 
            // txtCountryDest
            // 
            this.txtCountryDest.DisplayBox1Binding = "";
            this.txtCountryDest.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtCountryDest.Location = new System.Drawing.Point(125, 152);
            this.txtCountryDest.Name = "txtCountryDest";
            this.txtCountryDest.Size = new System.Drawing.Size(257, 22);
            this.txtCountryDest.TabIndex = 8;
            this.txtCountryDest.TextBox1Binding = "";
            // 
            // comboShipmodeShipmodeID
            // 
            this.comboShipmodeShipmodeID.BackColor = System.Drawing.Color.White;
            this.comboShipmodeShipmodeID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShipmodeShipmodeID.IsSupportUnselect = true;
            this.comboShipmodeShipmodeID.Location = new System.Drawing.Point(125, 177);
            this.comboShipmodeShipmodeID.Name = "comboShipmodeShipmodeID";
            this.comboShipmodeShipmodeID.Size = new System.Drawing.Size(78, 24);
            this.comboShipmodeShipmodeID.TabIndex = 9;
            this.comboShipmodeShipmodeID.UseFunction = "";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(13, 204);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 23);
            this.label11.TabIndex = 114;
            this.label11.Text = "Forwarder";
            // 
            // txtForwarder
            // 
            this.txtForwarder.BackColor = System.Drawing.Color.White;
            this.txtForwarder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtForwarder.Location = new System.Drawing.Point(125, 204);
            this.txtForwarder.Name = "txtForwarder";
            this.txtForwarder.Size = new System.Drawing.Size(100, 23);
            this.txtForwarder.TabIndex = 10;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(13, 232);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 23);
            this.label12.TabIndex = 116;
            this.label12.Text = "Export Rate By";
            // 
            // comboRateTypeID
            // 
            this.comboRateTypeID.BackColor = System.Drawing.Color.White;
            this.comboRateTypeID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboRateTypeID.FormattingEnabled = true;
            this.comboRateTypeID.IsSupportUnselect = true;
            this.comboRateTypeID.Items.AddRange(new object[] {
            "FX",
            "KP "});
            this.comboRateTypeID.Location = new System.Drawing.Point(125, 234);
            this.comboRateTypeID.Name = "comboRateTypeID";
            this.comboRateTypeID.Size = new System.Drawing.Size(121, 24);
            this.comboRateTypeID.TabIndex = 11;
            // 
            // checkExportDetail
            // 
            this.checkExportDetail.AutoSize = true;
            this.checkExportDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkExportDetail.Location = new System.Drawing.Point(10, 264);
            this.checkExportDetail.Name = "checkExportDetail";
            this.checkExportDetail.Size = new System.Drawing.Size(111, 21);
            this.checkExportDetail.TabIndex = 12;
            this.checkExportDetail.Text = "Export Detail ";
            this.checkExportDetail.UseVisualStyleBackColor = true;
            // 
            // R13
            // 
            this.ClientSize = new System.Drawing.Size(540, 314);
            this.Controls.Add(this.checkExportDetail);
            this.Controls.Add(this.comboRateTypeID);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtForwarder);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.comboShipmodeShipmodeID);
            this.Controls.Add(this.txtCountryDest);
            this.Controls.Add(this.txtCustCDCustCDID);
            this.Controls.Add(this.txtBrandBrandID);
            this.Controls.Add(this.dateOnboardDate2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.dateOnboardDate1);
            this.Controls.Add(this.dateApproveDate2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dateApproveDate1);
            this.Controls.Add(this.datePulloutDate2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.datePulloutDate1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R13";
            this.Text = "R13. Transportation Cost-Garment Export fee";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.datePulloutDate1, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.datePulloutDate2, 0);
            this.Controls.SetChildIndex(this.dateApproveDate1, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.dateApproveDate2, 0);
            this.Controls.SetChildIndex(this.dateOnboardDate1, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.dateOnboardDate2, 0);
            this.Controls.SetChildIndex(this.txtBrandBrandID, 0);
            this.Controls.SetChildIndex(this.txtCustCDCustCDID, 0);
            this.Controls.SetChildIndex(this.txtCountryDest, 0);
            this.Controls.SetChildIndex(this.comboShipmodeShipmodeID, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.txtForwarder, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.comboRateTypeID, 0);
            this.Controls.SetChildIndex(this.checkExportDetail, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.DateBox datePulloutDate1;
        private Win.UI.Label label8;
        private Win.UI.DateBox datePulloutDate2;
        private Win.UI.DateBox dateApproveDate1;
        private Win.UI.Label label9;
        private Win.UI.DateBox dateApproveDate2;
        private Win.UI.DateBox dateOnboardDate1;
        private Win.UI.Label label10;
        private Win.UI.DateBox dateOnboardDate2;
        private Class.Txtbrand txtBrandBrandID;
        private Class.Txtcustcd txtCustCDCustCDID;
        private Class.Txtcountry txtCountryDest;
        private Class.Txtshipmode comboShipmodeShipmodeID;
        private Win.UI.Label label11;
        private Win.UI.TextBox txtForwarder;
        private Win.UI.Label label12;
        private Win.UI.ComboBox comboRateTypeID;
        private Win.UI.CheckBox checkExportDetail;
    }
}
