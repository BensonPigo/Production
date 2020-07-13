namespace Sci.Production.Centralized
{
    partial class R11
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
            this.dateCloseDate1 = new Sci.Win.UI.DateBox();
            this.label9 = new Sci.Win.UI.Label();
            this.dateCloseDate2 = new Sci.Win.UI.DateBox();
            this.dateApproveDate1 = new Sci.Win.UI.DateBox();
            this.label10 = new Sci.Win.UI.Label();
            this.dateApproveDate2 = new Sci.Win.UI.DateBox();
            this.dateETA1 = new Sci.Win.UI.DateBox();
            this.label11 = new Sci.Win.UI.Label();
            this.dateETA2 = new Sci.Win.UI.DateBox();
            this.txtBrandBranadID = new Sci.Production.Class.Txtbrand();
            this.txtFactoryFactoryID = new Sci.Production.Class.TxtCentralizedFactory();
            this.txtCountryRegion = new Sci.Production.Class.Txtcountry();
            this.comboExportRateBy = new Sci.Win.UI.ComboBox();
            this.checkExportDetail = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(445, 12);
            this.print.TabIndex = 11;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(445, 48);
            this.toexcel.TabIndex = 12;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(445, 84);
            this.close.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Close Date";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 23);
            this.label2.TabIndex = 95;
            this.label2.Text = "Approve Date";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 23);
            this.label3.TabIndex = 96;
            this.label3.Text = "ETA";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(13, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 23);
            this.label4.TabIndex = 97;
            this.label4.Text = "Brand";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(13, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 23);
            this.label5.TabIndex = 98;
            this.label5.Text = "Factory";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(13, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 23);
            this.label6.TabIndex = 99;
            this.label6.Text = "Region";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(13, 177);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 23);
            this.label7.TabIndex = 100;
            this.label7.Text = "Export Rate By";
            // 
            // dateCloseDate1
            // 
            this.dateCloseDate1.Location = new System.Drawing.Point(123, 13);
            this.dateCloseDate1.Name = "dateCloseDate1";
            this.dateCloseDate1.Size = new System.Drawing.Size(130, 23);
            this.dateCloseDate1.TabIndex = 0;
            this.dateCloseDate1.Validated += new System.EventHandler(this.DateCloseDate1_Validated);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(261, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(22, 23);
            this.label9.TabIndex = 103;
            this.label9.Text = "~";
            // 
            // dateCloseDate2
            // 
            this.dateCloseDate2.Location = new System.Drawing.Point(291, 13);
            this.dateCloseDate2.Name = "dateCloseDate2";
            this.dateCloseDate2.Size = new System.Drawing.Size(130, 23);
            this.dateCloseDate2.TabIndex = 1;
            // 
            // dateApproveDate1
            // 
            this.dateApproveDate1.Location = new System.Drawing.Point(123, 41);
            this.dateApproveDate1.Name = "dateApproveDate1";
            this.dateApproveDate1.Size = new System.Drawing.Size(130, 23);
            this.dateApproveDate1.TabIndex = 2;
            this.dateApproveDate1.Validated += new System.EventHandler(this.DateApproveDate1_Validated);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(261, 41);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(22, 23);
            this.label10.TabIndex = 106;
            this.label10.Text = "~";
            // 
            // dateApproveDate2
            // 
            this.dateApproveDate2.Location = new System.Drawing.Point(291, 41);
            this.dateApproveDate2.Name = "dateApproveDate2";
            this.dateApproveDate2.Size = new System.Drawing.Size(130, 23);
            this.dateApproveDate2.TabIndex = 3;
            // 
            // dateETA1
            // 
            this.dateETA1.Location = new System.Drawing.Point(123, 69);
            this.dateETA1.Name = "dateETA1";
            this.dateETA1.Size = new System.Drawing.Size(130, 23);
            this.dateETA1.TabIndex = 4;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(261, 69);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(22, 23);
            this.label11.TabIndex = 109;
            this.label11.Text = "~";
            // 
            // dateETA2
            // 
            this.dateETA2.Location = new System.Drawing.Point(291, 69);
            this.dateETA2.Name = "dateETA2";
            this.dateETA2.Size = new System.Drawing.Size(130, 23);
            this.dateETA2.TabIndex = 5;
            // 
            // txtBrandBranadID
            // 
            this.txtBrandBranadID.BackColor = System.Drawing.Color.White;
            this.txtBrandBranadID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrandBranadID.Location = new System.Drawing.Point(123, 96);
            this.txtBrandBranadID.Name = "txtBrandBranadID";
            this.txtBrandBranadID.Size = new System.Drawing.Size(92, 23);
            this.txtBrandBranadID.TabIndex = 6;
            // 
            // txtFactoryFactoryID
            // 
            this.txtFactoryFactoryID.BackColor = System.Drawing.Color.White;
            this.txtFactoryFactoryID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactoryFactoryID.Location = new System.Drawing.Point(123, 122);
            this.txtFactoryFactoryID.MaxLength = 8;
            this.txtFactoryFactoryID.Name = "txtFactoryFactoryID";
            this.txtFactoryFactoryID.Size = new System.Drawing.Size(92, 23);
            this.txtFactoryFactoryID.TabIndex = 7;
            // 
            // txtCountryRegion
            // 
            this.txtCountryRegion.DisplayBox1Binding = "";
            this.txtCountryRegion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtCountryRegion.Location = new System.Drawing.Point(123, 149);
            this.txtCountryRegion.Name = "txtCountryRegion";
            this.txtCountryRegion.Size = new System.Drawing.Size(257, 22);
            this.txtCountryRegion.TabIndex = 8;
            // 
            // 
            // 
            this.txtCountryRegion.TextBox1.BackColor = System.Drawing.Color.White;
            this.txtCountryRegion.TextBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtCountryRegion.TextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtCountryRegion.TextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCountryRegion.TextBox1.Location = new System.Drawing.Point(0, 0);
            this.txtCountryRegion.TextBox1.MaxLength = 2;
            this.txtCountryRegion.TextBox1.Name = "textBox1";
            this.txtCountryRegion.TextBox1.Size = new System.Drawing.Size(30, 23);
            this.txtCountryRegion.TextBox1.TabIndex = 0;
            this.txtCountryRegion.TextBox1Binding = "";
            // 
            // comboExportRateBy
            // 
            this.comboExportRateBy.BackColor = System.Drawing.Color.White;
            this.comboExportRateBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboExportRateBy.FormattingEnabled = true;
            this.comboExportRateBy.IsSupportUnselect = true;
            this.comboExportRateBy.Items.AddRange(new object[] {
            "FX",
            "KP"});
            this.comboExportRateBy.Location = new System.Drawing.Point(123, 179);
            this.comboExportRateBy.Name = "comboExportRateBy";
            this.comboExportRateBy.Size = new System.Drawing.Size(121, 24);
            this.comboExportRateBy.TabIndex = 9;
            // 
            // checkExportDetail
            // 
            this.checkExportDetail.AutoSize = true;
            this.checkExportDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkExportDetail.Location = new System.Drawing.Point(13, 209);
            this.checkExportDetail.Name = "checkExportDetail";
            this.checkExportDetail.Size = new System.Drawing.Size(111, 21);
            this.checkExportDetail.TabIndex = 10;
            this.checkExportDetail.Text = "Export Detail ";
            this.checkExportDetail.UseVisualStyleBackColor = true;
            // 
            // R20
            // 
            this.ClientSize = new System.Drawing.Size(537, 257);
            this.Controls.Add(this.checkExportDetail);
            this.Controls.Add(this.comboExportRateBy);
            this.Controls.Add(this.txtCountryRegion);
            this.Controls.Add(this.txtFactoryFactoryID);
            this.Controls.Add(this.txtBrandBranadID);
            this.Controls.Add(this.dateETA2);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.dateETA1);
            this.Controls.Add(this.dateApproveDate2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.dateApproveDate1);
            this.Controls.Add(this.dateCloseDate2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dateCloseDate1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R11";
            this.Text = "R11.Transportation Cost- Material Import clearance";
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
            this.Controls.SetChildIndex(this.dateCloseDate1, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.dateCloseDate2, 0);
            this.Controls.SetChildIndex(this.dateApproveDate1, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.dateApproveDate2, 0);
            this.Controls.SetChildIndex(this.dateETA1, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.dateETA2, 0);
            this.Controls.SetChildIndex(this.txtBrandBranadID, 0);
            this.Controls.SetChildIndex(this.txtFactoryFactoryID, 0);
            this.Controls.SetChildIndex(this.txtCountryRegion, 0);
            this.Controls.SetChildIndex(this.comboExportRateBy, 0);
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
        private Win.UI.DateBox dateCloseDate1;
        private Win.UI.Label label9;
        private Win.UI.DateBox dateCloseDate2;
        private Win.UI.DateBox dateApproveDate1;
        private Win.UI.Label label10;
        private Win.UI.DateBox dateApproveDate2;
        private Win.UI.DateBox dateETA1;
        private Win.UI.Label label11;
        private Win.UI.DateBox dateETA2;
        private Sci.Production.Class.Txtbrand txtBrandBranadID;
        private Sci.Production.Class.TxtCentralizedFactory txtFactoryFactoryID;
        private Sci.Production.Class.Txtcountry txtCountryRegion;
        private Win.UI.ComboBox comboExportRateBy;
        private Win.UI.CheckBox checkExportDetail;
    }
}