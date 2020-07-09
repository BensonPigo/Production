namespace Sci.Production.Quality
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.labelArriveWHDate = new Sci.Win.UI.Label();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtRef = new Sci.Win.UI.TextBox();
            this.txtsupplier = new Sci.Production.Class.Txtsupplier();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelRef = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.dateArriveWHDate = new Sci.Win.UI.DateRange();
            this.labelReportType = new Sci.Win.UI.Label();
            this.radioRefNo = new Sci.Win.UI.RadioButton();
            this.radioSupplier = new Sci.Win.UI.RadioButton();
            this.radioPanel = new Sci.Win.UI.RadioPanel();
            this.panel1.SuspendLayout();
            this.radioPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelReportType);
            this.panel1.Controls.Add(this.labelArriveWHDate);
            this.panel1.Controls.Add(this.txtseason);
            this.panel1.Controls.Add(this.txtbrand);
            this.panel1.Controls.Add(this.txtRef);
            this.panel1.Controls.Add(this.txtsupplier);
            this.panel1.Controls.Add(this.labelSeason);
            this.panel1.Controls.Add(this.labelBrand);
            this.panel1.Controls.Add(this.labelRef);
            this.panel1.Controls.Add(this.labelSupplier);
            this.panel1.Controls.Add(this.dateArriveWHDate);
            this.panel1.Controls.Add(this.radioPanel);
            this.panel1.Location = new System.Drawing.Point(29, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(477, 292);
            this.panel1.TabIndex = 94;
            // 
            // labelArriveWHDate
            // 
            this.labelArriveWHDate.Location = new System.Drawing.Point(16, 23);
            this.labelArriveWHDate.Name = "labelArriveWHDate";
            this.labelArriveWHDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelArriveWHDate.RectStyle.BorderWidth = 1F;
            this.labelArriveWHDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelArriveWHDate.RectStyle.ExtBorderWidth = 1F;
            this.labelArriveWHDate.Size = new System.Drawing.Size(113, 23);
            this.labelArriveWHDate.TabIndex = 97;
            this.labelArriveWHDate.Text = "Arrive W/H Date";
            this.labelArriveWHDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelArriveWHDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(141, 186);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(104, 23);
            this.txtseason.TabIndex = 25;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(141, 141);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(94, 23);
            this.txtbrand.TabIndex = 24;
            // 
            // txtRef
            // 
            this.txtRef.BackColor = System.Drawing.Color.White;
            this.txtRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRef.Location = new System.Drawing.Point(141, 100);
            this.txtRef.Name = "txtRef";
            this.txtRef.Size = new System.Drawing.Size(257, 23);
            this.txtRef.TabIndex = 23;
            // 
            // txtsupplier
            // 
            this.txtsupplier.DisplayBox1Binding = "";
            this.txtsupplier.Location = new System.Drawing.Point(141, 60);
            this.txtsupplier.Name = "txtsupplier";
            this.txtsupplier.Size = new System.Drawing.Size(147, 23);
            this.txtsupplier.TabIndex = 22;
            this.txtsupplier.TextBox1Binding = "";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(16, 186);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(113, 23);
            this.labelSeason.TabIndex = 21;
            this.labelSeason.Text = "Season";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(16, 141);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(113, 23);
            this.labelBrand.TabIndex = 20;
            this.labelBrand.Text = "Brand";
            // 
            // labelRef
            // 
            this.labelRef.Location = new System.Drawing.Point(16, 100);
            this.labelRef.Name = "labelRef";
            this.labelRef.Size = new System.Drawing.Size(113, 23);
            this.labelRef.TabIndex = 19;
            this.labelRef.Text = "Ref#";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(16, 60);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(113, 23);
            this.labelSupplier.TabIndex = 18;
            this.labelSupplier.Text = "Supplier";
            // 
            // dateArriveWHDate
            // 
            // 
            // 
            // 
            this.dateArriveWHDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateArriveWHDate.DateBox1.Name = "";
            this.dateArriveWHDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateArriveWHDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateArriveWHDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateArriveWHDate.DateBox2.Name = "";
            this.dateArriveWHDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateArriveWHDate.DateBox2.TabIndex = 1;
            this.dateArriveWHDate.IsRequired = false;
            this.dateArriveWHDate.Location = new System.Drawing.Point(141, 23);
            this.dateArriveWHDate.Name = "dateArriveWHDate";
            this.dateArriveWHDate.Size = new System.Drawing.Size(280, 23);
            this.dateArriveWHDate.TabIndex = 17;
            // 
            // labelReportType
            // 
            this.labelReportType.Location = new System.Drawing.Point(16, 226);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(113, 23);
            this.labelReportType.TabIndex = 98;
            this.labelReportType.Text = "Report Type";
            // 
            // radioRefNo
            // 
            this.radioRefNo.AutoSize = true;
            this.radioRefNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioRefNo.Location = new System.Drawing.Point(9, 40);
            this.radioRefNo.Name = "radioRefNo";
            this.radioRefNo.Size = new System.Drawing.Size(75, 21);
            this.radioRefNo.TabIndex = 108;
            this.radioRefNo.Text = "by Ref#";
            this.radioRefNo.UseVisualStyleBackColor = true;
            this.radioRefNo.Value = "Refno";
            // 
            // radioSupplier
            // 
            this.radioSupplier.AutoSize = true;
            this.radioSupplier.Checked = true;
            this.radioSupplier.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSupplier.Location = new System.Drawing.Point(9, 13);
            this.radioSupplier.Name = "radioSupplier";
            this.radioSupplier.Size = new System.Drawing.Size(97, 21);
            this.radioSupplier.TabIndex = 107;
            this.radioSupplier.TabStop = true;
            this.radioSupplier.Text = "by Supplier";
            this.radioSupplier.UseVisualStyleBackColor = true;
            this.radioSupplier.Value = "supplier";
            // 
            // radioPanel
            // 
            this.radioPanel.Controls.Add(this.radioRefNo);
            this.radioPanel.Controls.Add(this.radioSupplier);
            this.radioPanel.Location = new System.Drawing.Point(132, 215);
            this.radioPanel.Name = "radioPanel";
            this.radioPanel.Size = new System.Drawing.Size(195, 69);
            this.radioPanel.TabIndex = 109;
            this.radioPanel.Value = "supplier";
            // 
            // R06
            // 
            this.ClientSize = new System.Drawing.Size(627, 345);
            this.Controls.Add(this.panel1);
            this.Name = "R06";
            this.Text = "R06. Supplier Score - Fabric";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.radioPanel.ResumeLayout(false);
            this.radioPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.DateRange dateArriveWHDate;
        private Class.Txtseason txtseason;
        private Class.Txtbrand txtbrand;
        private Win.UI.TextBox txtRef;
        private Class.Txtsupplier txtsupplier;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelRef;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelArriveWHDate;
        private Win.UI.Label labelReportType;
        private Win.UI.RadioButton radioRefNo;
        private Win.UI.RadioButton radioSupplier;
        private Win.UI.RadioPanel radioPanel;
    }
}
