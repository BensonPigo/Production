namespace Sci.Production.Centralized
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dateApproveDate2 = new Sci.Win.UI.DateBox();
            this.dateApproveDate1 = new Sci.Win.UI.DateBox();
            this.datePulloutDate2 = new Sci.Win.UI.DateBox();
            this.datePulloutDate1 = new Sci.Win.UI.DateBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.checkExportDetail = new Sci.Win.UI.CheckBox();
            this.comboRateTypeID = new Sci.Win.UI.ComboBox();
            this.label12 = new Sci.Win.UI.Label();
            this.txtForwarder = new Sci.Win.UI.TextBox();
            this.label11 = new Sci.Win.UI.Label();
            this.comboShipmodeShipmodeID = new Sci.Production.Class.Txtshipmode();
            this.txtCountryDest = new Sci.Production.Class.Txtcountry();
            this.txtCustCDCustCDID = new Sci.Production.Class.Txtcustcd();
            this.txtBrandBrandID = new Sci.Production.Class.Txtbrand();
            this.label7 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(441, 12);
            this.print.TabIndex = 11;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(441, 48);
            this.toexcel.TabIndex = 12;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(441, 84);
            this.close.TabIndex = 13;
            // 
            // dateApproveDate2
            // 
            this.dateApproveDate2.Location = new System.Drawing.Point(280, 37);
            this.dateApproveDate2.Name = "dateApproveDate2";
            this.dateApproveDate2.Size = new System.Drawing.Size(130, 23);
            this.dateApproveDate2.TabIndex = 3;
            // 
            // dateApproveDate1
            // 
            this.dateApproveDate1.Location = new System.Drawing.Point(121, 37);
            this.dateApproveDate1.Name = "dateApproveDate1";
            this.dateApproveDate1.Size = new System.Drawing.Size(130, 23);
            this.dateApproveDate1.TabIndex = 2;
            this.dateApproveDate1.Validated += new System.EventHandler(this.DateApproveDate1_Validated);
            // 
            // datePulloutDate2
            // 
            this.datePulloutDate2.Location = new System.Drawing.Point(280, 9);
            this.datePulloutDate2.Name = "datePulloutDate2";
            this.datePulloutDate2.Size = new System.Drawing.Size(130, 23);
            this.datePulloutDate2.TabIndex = 1;
            // 
            // datePulloutDate1
            // 
            this.datePulloutDate1.Location = new System.Drawing.Point(121, 9);
            this.datePulloutDate1.Name = "datePulloutDate1";
            this.datePulloutDate1.Size = new System.Drawing.Size(130, 23);
            this.datePulloutDate1.TabIndex = 0;
            this.datePulloutDate1.Validated += new System.EventHandler(this.DatePulloutDate1_Validated);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 23);
            this.label2.TabIndex = 108;
            this.label2.Text = "Approve Date";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 23);
            this.label1.TabIndex = 107;
            this.label1.Text = "Pullout Date";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(254, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 23);
            this.label3.TabIndex = 113;
            this.label3.Text = "~";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(254, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 23);
            this.label4.TabIndex = 114;
            this.label4.Text = "~";
            // 
            // checkExportDetail
            // 
            this.checkExportDetail.AutoSize = true;
            this.checkExportDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkExportDetail.Location = new System.Drawing.Point(6, 231);
            this.checkExportDetail.Name = "checkExportDetail";
            this.checkExportDetail.Size = new System.Drawing.Size(111, 21);
            this.checkExportDetail.TabIndex = 10;
            this.checkExportDetail.Text = "Export Detail ";
            this.checkExportDetail.UseVisualStyleBackColor = true;
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
            this.comboRateTypeID.Location = new System.Drawing.Point(121, 201);
            this.comboRateTypeID.Name = "comboRateTypeID";
            this.comboRateTypeID.Size = new System.Drawing.Size(121, 24);
            this.comboRateTypeID.TabIndex = 9;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(9, 199);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 23);
            this.label12.TabIndex = 129;
            this.label12.Text = "Export Rate By";
            // 
            // txtForwarder
            // 
            this.txtForwarder.BackColor = System.Drawing.Color.White;
            this.txtForwarder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtForwarder.Location = new System.Drawing.Point(121, 171);
            this.txtForwarder.Name = "txtForwarder";
            this.txtForwarder.Size = new System.Drawing.Size(100, 23);
            this.txtForwarder.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(9, 171);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 23);
            this.label11.TabIndex = 127;
            this.label11.Text = "Forwarder";
            // 
            // comboShipmodeShipmodeID
            // 
            this.comboShipmodeShipmodeID.BackColor = System.Drawing.Color.White;
            this.comboShipmodeShipmodeID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShipmodeShipmodeID.IsSupportUnselect = true;
            this.comboShipmodeShipmodeID.Location = new System.Drawing.Point(121, 144);
            this.comboShipmodeShipmodeID.Name = "comboShipmodeShipmodeID";
            this.comboShipmodeShipmodeID.Size = new System.Drawing.Size(78, 24);
            this.comboShipmodeShipmodeID.TabIndex = 7;
            this.comboShipmodeShipmodeID.UseFunction = "";
            // 
            // txtCountryDest
            // 
            this.txtCountryDest.DisplayBox1Binding = "";
            this.txtCountryDest.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtCountryDest.Location = new System.Drawing.Point(121, 119);
            this.txtCountryDest.Name = "txtCountryDest";
            this.txtCountryDest.Size = new System.Drawing.Size(257, 22);
            this.txtCountryDest.TabIndex = 6;
            this.txtCountryDest.TextBox1Binding = "";
            // 
            // txtCustCDCustCDID
            // 
            this.txtCustCDCustCDID.BackColor = System.Drawing.Color.White;
            this.txtCustCDCustCDID.BrandObjectName = null;
            this.txtCustCDCustCDID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCustCDCustCDID.Location = new System.Drawing.Point(121, 90);
            this.txtCustCDCustCDID.Name = "txtCustCDCustCDID";
            this.txtCustCDCustCDID.Size = new System.Drawing.Size(130, 23);
            this.txtCustCDCustCDID.TabIndex = 5;
            // 
            // txtBrandBrandID
            // 
            this.txtBrandBrandID.BackColor = System.Drawing.Color.White;
            this.txtBrandBrandID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrandBrandID.Location = new System.Drawing.Point(121, 63);
            this.txtBrandBrandID.Name = "txtBrandBrandID";
            this.txtBrandBrandID.Size = new System.Drawing.Size(90, 23);
            this.txtBrandBrandID.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(9, 144);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 23);
            this.label7.TabIndex = 122;
            this.label7.Text = "Ship Mode";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(9, 118);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 121;
            this.label6.Text = "Destination";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 120;
            this.label5.Text = "CUSTCD";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(9, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 23);
            this.label8.TabIndex = 119;
            this.label8.Text = "Brand";
            // 
            // R14
            // 
            this.ClientSize = new System.Drawing.Size(533, 289);
            this.Controls.Add(this.checkExportDetail);
            this.Controls.Add(this.comboRateTypeID);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtForwarder);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.comboShipmodeShipmodeID);
            this.Controls.Add(this.txtCountryDest);
            this.Controls.Add(this.txtCustCDCustCDID);
            this.Controls.Add(this.txtBrandBrandID);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateApproveDate2);
            this.Controls.Add(this.dateApproveDate1);
            this.Controls.Add(this.datePulloutDate2);
            this.Controls.Add(this.datePulloutDate1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R14";
            this.Text = "R14.Transportation Cost-Air Freight";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.datePulloutDate1, 0);
            this.Controls.SetChildIndex(this.datePulloutDate2, 0);
            this.Controls.SetChildIndex(this.dateApproveDate1, 0);
            this.Controls.SetChildIndex(this.dateApproveDate2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
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

        private Win.UI.DateBox dateApproveDate2;
        private Win.UI.DateBox dateApproveDate1;
        private Win.UI.DateBox datePulloutDate2;
        private Win.UI.DateBox datePulloutDate1;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.CheckBox checkExportDetail;
        private Win.UI.ComboBox comboRateTypeID;
        private Win.UI.Label label12;
        private Win.UI.TextBox txtForwarder;
        private Win.UI.Label label11;
        private Class.Txtshipmode comboShipmodeShipmodeID;
        private Class.Txtcountry txtCountryDest;
        private Class.Txtcustcd txtCustCDCustCDID;
        private Class.Txtbrand txtBrandBrandID;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label8;
    }
}
