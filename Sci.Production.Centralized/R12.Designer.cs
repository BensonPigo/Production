namespace Sci.Production.Centralized
{
    partial class R12
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
            this.dateShipDate1 = new Sci.Win.UI.DateBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.checkExportDetail = new Sci.Win.UI.CheckBox();
            this.dateShipDate2 = new Sci.Win.UI.DateBox();
            this.label8 = new Sci.Win.UI.Label();
            this.dateApproveDate1 = new Sci.Win.UI.DateBox();
            this.dateApproveDate2 = new Sci.Win.UI.DateBox();
            this.label9 = new Sci.Win.UI.Label();
            this.txtCountryImportCountry = new Sci.Production.Class.Txtcountry();
            this.comboShipmodeShipmodeID = new Sci.Production.Class.Txtshipmode();
            this.txtForwarder = new Sci.Win.UI.TextBox();
            this.comboRateTypeID = new Sci.Win.UI.ComboBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(428, 12);
            this.print.TabIndex = 9;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(428, 48);
            this.toexcel.TabIndex = 10;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(428, 84);
            this.close.TabIndex = 11;
            // 
            // dateShipDate1
            // 
            this.dateShipDate1.Location = new System.Drawing.Point(139, 9);
            this.dateShipDate1.Name = "dateShipDate1";
            this.dateShipDate1.Size = new System.Drawing.Size(130, 23);
            this.dateShipDate1.TabIndex = 0;
            this.dateShipDate1.Validated += new System.EventHandler(this.DateShipDate1_Validated);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 23);
            this.label1.TabIndex = 95;
            this.label1.Text = "Ship Date";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 23);
            this.label2.TabIndex = 96;
            this.label2.Text = "Approve Date";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 23);
            this.label3.TabIndex = 97;
            this.label3.Text = "Port of Destination";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(13, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 23);
            this.label4.TabIndex = 98;
            this.label4.Text = "Ship Mode";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(13, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 23);
            this.label5.TabIndex = 99;
            this.label5.Text = "Forwarder";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 147);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 23);
            this.label7.TabIndex = 101;
            this.label7.Text = "Export Rate By";
            // 
            // checkExportDetail
            // 
            this.checkExportDetail.AutoSize = true;
            this.checkExportDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkExportDetail.Location = new System.Drawing.Point(12, 177);
            this.checkExportDetail.Name = "checkExportDetail";
            this.checkExportDetail.Size = new System.Drawing.Size(111, 21);
            this.checkExportDetail.TabIndex = 8;
            this.checkExportDetail.Text = "Export Detail ";
            this.checkExportDetail.UseVisualStyleBackColor = true;
            // 
            // dateShipDate2
            // 
            this.dateShipDate2.Location = new System.Drawing.Point(289, 9);
            this.dateShipDate2.Name = "dateShipDate2";
            this.dateShipDate2.Size = new System.Drawing.Size(130, 23);
            this.dateShipDate2.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(272, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 23);
            this.label8.TabIndex = 104;
            this.label8.Text = "~";
            // 
            // dateApproveDate1
            // 
            this.dateApproveDate1.Location = new System.Drawing.Point(139, 38);
            this.dateApproveDate1.Name = "dateApproveDate1";
            this.dateApproveDate1.Size = new System.Drawing.Size(130, 23);
            this.dateApproveDate1.TabIndex = 2;
            this.dateApproveDate1.Validated += new System.EventHandler(this.DateApproveDate1_Validated);
            // 
            // dateApproveDate2
            // 
            this.dateApproveDate2.Location = new System.Drawing.Point(289, 38);
            this.dateApproveDate2.Name = "dateApproveDate2";
            this.dateApproveDate2.Size = new System.Drawing.Size(130, 23);
            this.dateApproveDate2.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(272, 38);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 23);
            this.label9.TabIndex = 107;
            this.label9.Text = "~";
            // 
            // txtCountryImportCountry
            // 
            this.txtCountryImportCountry.DisplayBox1Binding = "";
            this.txtCountryImportCountry.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtCountryImportCountry.Location = new System.Drawing.Point(139, 68);
            this.txtCountryImportCountry.Name = "txtCountryImportCountry";
            this.txtCountryImportCountry.Size = new System.Drawing.Size(257, 22);
            this.txtCountryImportCountry.TabIndex = 4;
            this.txtCountryImportCountry.TextBox1Binding = "";
            // 
            // comboShipmodeShipmodeID
            // 
            this.comboShipmodeShipmodeID.BackColor = System.Drawing.Color.White;
            this.comboShipmodeShipmodeID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShipmodeShipmodeID.IsSupportUnselect = true;
            this.comboShipmodeShipmodeID.Location = new System.Drawing.Point(139, 92);
            this.comboShipmodeShipmodeID.Name = "comboShipmodeShipmodeID";
            this.comboShipmodeShipmodeID.Size = new System.Drawing.Size(78, 24);
            this.comboShipmodeShipmodeID.TabIndex = 5;
            this.comboShipmodeShipmodeID.UseFunction = "";
            // 
            // 
            // 
            this.txtCountryImportCountry.TextBox1.BackColor = System.Drawing.Color.White;
            this.txtCountryImportCountry.TextBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtCountryImportCountry.TextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtCountryImportCountry.TextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCountryImportCountry.TextBox1.Location = new System.Drawing.Point(0, 0);
            this.txtCountryImportCountry.TextBox1.MaxLength = 2;
            this.txtCountryImportCountry.TextBox1.Name = "textBox1";
            this.txtCountryImportCountry.TextBox1.Size = new System.Drawing.Size(30, 23);
            this.txtCountryImportCountry.TextBox1.TabIndex = 0;
            this.txtCountryImportCountry.TextBox1Binding = "";
            // 
            // txtForwarder
            // 
            this.txtForwarder.BackColor = System.Drawing.Color.White;
            this.txtForwarder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtForwarder.Location = new System.Drawing.Point(139, 119);
            this.txtForwarder.Name = "txtForwarder";
            this.txtForwarder.Size = new System.Drawing.Size(100, 23);
            this.txtForwarder.TabIndex = 6;
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
            this.comboRateTypeID.Location = new System.Drawing.Point(139, 147);
            this.comboRateTypeID.Name = "comboRateTypeID";
            this.comboRateTypeID.Size = new System.Drawing.Size(121, 24);
            this.comboRateTypeID.TabIndex = 7;
            // 
            // R12
            // R21
            // 
            this.ClientSize = new System.Drawing.Size(520, 223);
            this.Controls.Add(this.comboRateTypeID);
            this.Controls.Add(this.txtForwarder);
            this.Controls.Add(this.comboShipmodeShipmodeID);
            this.Controls.Add(this.txtCountryImportCountry);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dateApproveDate2);
            this.Controls.Add(this.dateApproveDate1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.dateShipDate2);
            this.Controls.Add(this.checkExportDetail);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateShipDate1);
            this.Name = "R12";
            this.Text = "R12. Transportation Cost- Sister Factory Transfer";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.dateShipDate1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.checkExportDetail, 0);
            this.Controls.SetChildIndex(this.dateShipDate2, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.dateApproveDate1, 0);
            this.Controls.SetChildIndex(this.dateApproveDate2, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.txtCountryImportCountry, 0);
            this.Controls.SetChildIndex(this.comboShipmodeShipmodeID, 0);
            this.Controls.SetChildIndex(this.txtForwarder, 0);
            this.Controls.SetChildIndex(this.comboRateTypeID, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateBox dateShipDate1;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label7;
        private Win.UI.CheckBox checkExportDetail;
        private Win.UI.DateBox dateShipDate2;
        private Win.UI.Label label8;
        private Win.UI.DateBox dateApproveDate1;
        private Win.UI.DateBox dateApproveDate2;
        private Win.UI.Label label9;
        private Class.Txtcountry txtCountryImportCountry;
        private Class.Txtshipmode comboShipmodeShipmodeID;
        private Win.UI.TextBox txtForwarder;
        private Win.UI.ComboBox comboRateTypeID;
    }
}