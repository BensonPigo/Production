namespace Sci.Production.Shipping
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
            this.labelReportContent = new Sci.Win.UI.Label();
            this.labelPulloutDate = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelCustCD = new Sci.Win.UI.Label();
            this.labelDestination = new Sci.Win.UI.Label();
            this.labelShipMode = new Sci.Win.UI.Label();
            this.labelForwarder = new Sci.Win.UI.Label();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioRawMaterial = new Sci.Win.UI.RadioButton();
            this.radioGarment = new Sci.Win.UI.RadioButton();
            this.datePulloutDate = new Sci.Win.UI.DateRange();
            this.txtbrand = new Sci.Production.Class.txtbrand();
            this.txtcustcd = new Sci.Production.Class.txtcustcd();
            this.txtcountryDestination = new Sci.Production.Class.txtcountry();
            this.txtshipmode = new Sci.Production.Class.txtshipmode();
            this.displayForwarder = new Sci.Win.UI.DisplayBox();
            this.txtForwarder = new Sci.Win.UI.TextBox();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(430, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(430, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(430, 84);
            // 
            // labelReportContent
            // 
            this.labelReportContent.Lines = 0;
            this.labelReportContent.Location = new System.Drawing.Point(13, 12);
            this.labelReportContent.Name = "labelReportContent";
            this.labelReportContent.Size = new System.Drawing.Size(101, 23);
            this.labelReportContent.TabIndex = 94;
            this.labelReportContent.Text = "Report Content";
            // 
            // labelPulloutDate
            // 
            this.labelPulloutDate.Lines = 0;
            this.labelPulloutDate.Location = new System.Drawing.Point(13, 71);
            this.labelPulloutDate.Name = "labelPulloutDate";
            this.labelPulloutDate.Size = new System.Drawing.Size(101, 23);
            this.labelPulloutDate.TabIndex = 95;
            this.labelPulloutDate.Text = "Pullout Date";
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(13, 107);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(101, 23);
            this.labelBrand.TabIndex = 96;
            this.labelBrand.Text = "Brand";
            // 
            // labelCustCD
            // 
            this.labelCustCD.Lines = 0;
            this.labelCustCD.Location = new System.Drawing.Point(13, 143);
            this.labelCustCD.Name = "labelCustCD";
            this.labelCustCD.Size = new System.Drawing.Size(101, 23);
            this.labelCustCD.TabIndex = 97;
            this.labelCustCD.Text = "Cust CD";
            // 
            // labelDestination
            // 
            this.labelDestination.Lines = 0;
            this.labelDestination.Location = new System.Drawing.Point(13, 180);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(101, 23);
            this.labelDestination.TabIndex = 98;
            this.labelDestination.Text = "Destination";
            // 
            // labelShipMode
            // 
            this.labelShipMode.Lines = 0;
            this.labelShipMode.Location = new System.Drawing.Point(13, 216);
            this.labelShipMode.Name = "labelShipMode";
            this.labelShipMode.Size = new System.Drawing.Size(101, 23);
            this.labelShipMode.TabIndex = 99;
            this.labelShipMode.Text = "Ship Mode";
            // 
            // labelForwarder
            // 
            this.labelForwarder.Lines = 0;
            this.labelForwarder.Location = new System.Drawing.Point(13, 253);
            this.labelForwarder.Name = "labelForwarder";
            this.labelForwarder.Size = new System.Drawing.Size(101, 23);
            this.labelForwarder.TabIndex = 100;
            this.labelForwarder.Text = "Forwarder";
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioRawMaterial);
            this.radioPanel1.Controls.Add(this.radioGarment);
            this.radioPanel1.Location = new System.Drawing.Point(117, 10);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(112, 52);
            this.radioPanel1.TabIndex = 101;
            // 
            // radioRawMaterial
            // 
            this.radioRawMaterial.AutoSize = true;
            this.radioRawMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioRawMaterial.Location = new System.Drawing.Point(3, 29);
            this.radioRawMaterial.Name = "radioRawMaterial";
            this.radioRawMaterial.Size = new System.Drawing.Size(107, 21);
            this.radioRawMaterial.TabIndex = 1;
            this.radioRawMaterial.TabStop = true;
            this.radioRawMaterial.Text = "Raw Material";
            this.radioRawMaterial.UseVisualStyleBackColor = true;
            // 
            // radioGarment
            // 
            this.radioGarment.AutoSize = true;
            this.radioGarment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioGarment.Location = new System.Drawing.Point(3, 2);
            this.radioGarment.Name = "radioGarment";
            this.radioGarment.Size = new System.Drawing.Size(81, 21);
            this.radioGarment.TabIndex = 0;
            this.radioGarment.TabStop = true;
            this.radioGarment.Text = "Garment";
            this.radioGarment.UseVisualStyleBackColor = true;
            this.radioGarment.CheckedChanged += new System.EventHandler(this.RadioGarment_CheckedChanged);
            // 
            // datePulloutDate
            // 
            this.datePulloutDate.IsRequired = false;
            this.datePulloutDate.Location = new System.Drawing.Point(120, 71);
            this.datePulloutDate.Name = "datePulloutDate";
            this.datePulloutDate.Size = new System.Drawing.Size(280, 23);
            this.datePulloutDate.TabIndex = 102;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(120, 107);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 103;
            // 
            // txtcustcd
            // 
            this.txtcustcd.BackColor = System.Drawing.Color.White;
            this.txtcustcd.BrandObjectName = null;
            this.txtcustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd.Location = new System.Drawing.Point(120, 143);
            this.txtcustcd.Name = "txtcustcd";
            this.txtcustcd.Size = new System.Drawing.Size(125, 23);
            this.txtcustcd.TabIndex = 104;
            // 
            // txtcountryDestination
            // 
            this.txtcountryDestination.DisplayBox1Binding = "";
            this.txtcountryDestination.Location = new System.Drawing.Point(120, 180);
            this.txtcountryDestination.Name = "txtcountryDestination";
            this.txtcountryDestination.Size = new System.Drawing.Size(232, 22);
            this.txtcountryDestination.TabIndex = 105;
            this.txtcountryDestination.TextBox1Binding = "";
            // 
            // txtshipmode
            // 
            this.txtshipmode.BackColor = System.Drawing.Color.White;
            this.txtshipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshipmode.FormattingEnabled = true;
            this.txtshipmode.IsSupportUnselect = true;
            this.txtshipmode.Location = new System.Drawing.Point(120, 216);
            this.txtshipmode.Name = "txtshipmode";
            this.txtshipmode.Size = new System.Drawing.Size(80, 24);
            this.txtshipmode.TabIndex = 106;
            this.txtshipmode.UseFunction = null;
            // 
            // displayForwarder
            // 
            this.displayForwarder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayForwarder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayForwarder.Location = new System.Drawing.Point(183, 254);
            this.displayForwarder.Name = "displayForwarder";
            this.displayForwarder.Size = new System.Drawing.Size(100, 23);
            this.displayForwarder.TabIndex = 110;
            // 
            // txtForwarder
            // 
            this.txtForwarder.BackColor = System.Drawing.Color.White;
            this.txtForwarder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtForwarder.Location = new System.Drawing.Point(120, 253);
            this.txtForwarder.Name = "txtForwarder";
            this.txtForwarder.Size = new System.Drawing.Size(61, 23);
            this.txtForwarder.TabIndex = 109;
            this.txtForwarder.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtForwarder_PopUp);
            this.txtForwarder.Validating += new System.ComponentModel.CancelEventHandler(this.TxtForwarder_Validating);
            // 
            // R11
            // 
            this.ClientSize = new System.Drawing.Size(522, 314);
            this.Controls.Add(this.displayForwarder);
            this.Controls.Add(this.txtForwarder);
            this.Controls.Add(this.txtshipmode);
            this.Controls.Add(this.txtcountryDestination);
            this.Controls.Add(this.txtcustcd);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.datePulloutDate);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.labelForwarder);
            this.Controls.Add(this.labelShipMode);
            this.Controls.Add(this.labelDestination);
            this.Controls.Add(this.labelCustCD);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelPulloutDate);
            this.Controls.Add(this.labelReportContent);
            this.IsSupportToPrint = false;
            this.Name = "R11";
            this.Text = "R11. Non Shared List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelReportContent, 0);
            this.Controls.SetChildIndex(this.labelPulloutDate, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelCustCD, 0);
            this.Controls.SetChildIndex(this.labelDestination, 0);
            this.Controls.SetChildIndex(this.labelShipMode, 0);
            this.Controls.SetChildIndex(this.labelForwarder, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.datePulloutDate, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtcustcd, 0);
            this.Controls.SetChildIndex(this.txtcountryDestination, 0);
            this.Controls.SetChildIndex(this.txtshipmode, 0);
            this.Controls.SetChildIndex(this.txtForwarder, 0);
            this.Controls.SetChildIndex(this.displayForwarder, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelReportContent;
        private Win.UI.Label labelPulloutDate;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelCustCD;
        private Win.UI.Label labelDestination;
        private Win.UI.Label labelShipMode;
        private Win.UI.Label labelForwarder;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioRawMaterial;
        private Win.UI.RadioButton radioGarment;
        private Win.UI.DateRange datePulloutDate;
        private Class.txtbrand txtbrand;
        private Class.txtcustcd txtcustcd;
        private Class.txtcountry txtcountryDestination;
        private Class.txtshipmode txtshipmode;
        private Win.UI.DisplayBox displayForwarder;
        private Win.UI.TextBox txtForwarder;
    }
}
