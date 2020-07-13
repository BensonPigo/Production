namespace Sci.Production.PPIC
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
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.labelProvideDate = new Sci.Win.UI.Label();
            this.labelFtyMRRcvDate = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelMR = new Sci.Win.UI.Label();
            this.labelSMR = new Sci.Win.UI.Label();
            this.labelPOHandle = new Sci.Win.UI.Label();
            this.labelPOSMR = new Sci.Win.UI.Label();
            this.labelPrintType = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.dateProvideDate = new Sci.Win.UI.DateRange();
            this.dateFtyMRRcvDate = new Sci.Win.UI.DateRange();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.comboPrintType = new Sci.Win.UI.ComboBox();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txttpeuser_caneditMR = new Sci.Production.Class.Txttpeuser_canedit();
            this.txttpeuser_caneditSMR = new Sci.Production.Class.Txttpeuser_canedit();
            this.txttpeuser_caneditPOHandle = new Sci.Production.Class.Txttpeuser_canedit();
            this.txttpeuser_caneditPOSMR = new Sci.Production.Class.Txttpeuser_canedit();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(470, 12);
            this.print.TabIndex = 12;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(470, 48);
            this.toexcel.TabIndex = 13;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(470, 84);
            this.close.TabIndex = 14;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Lines = 0;
            this.labelSCIDelivery.Location = new System.Drawing.Point(13, 12);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(112, 23);
            this.labelSCIDelivery.TabIndex = 94;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // labelProvideDate
            // 
            this.labelProvideDate.Lines = 0;
            this.labelProvideDate.Location = new System.Drawing.Point(13, 48);
            this.labelProvideDate.Name = "labelProvideDate";
            this.labelProvideDate.Size = new System.Drawing.Size(112, 23);
            this.labelProvideDate.TabIndex = 95;
            this.labelProvideDate.Text = "Provide Date";
            // 
            // labelFtyMRRcvDate
            // 
            this.labelFtyMRRcvDate.Lines = 0;
            this.labelFtyMRRcvDate.Location = new System.Drawing.Point(13, 84);
            this.labelFtyMRRcvDate.Name = "labelFtyMRRcvDate";
            this.labelFtyMRRcvDate.Size = new System.Drawing.Size(112, 23);
            this.labelFtyMRRcvDate.TabIndex = 96;
            this.labelFtyMRRcvDate.Text = "Fty MR Rcv. Date";
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(13, 120);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(112, 23);
            this.labelBrand.TabIndex = 97;
            this.labelBrand.Text = "Brand";
            // 
            // labelStyle
            // 
            this.labelStyle.Lines = 0;
            this.labelStyle.Location = new System.Drawing.Point(13, 156);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(112, 23);
            this.labelStyle.TabIndex = 98;
            this.labelStyle.Text = "Style";
            // 
            // labelSeason
            // 
            this.labelSeason.Lines = 0;
            this.labelSeason.Location = new System.Drawing.Point(13, 192);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(112, 23);
            this.labelSeason.TabIndex = 99;
            this.labelSeason.Text = "Season";
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(13, 228);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(112, 23);
            this.labelM.TabIndex = 100;
            this.labelM.Text = "M";
            // 
            // labelMR
            // 
            this.labelMR.Lines = 0;
            this.labelMR.Location = new System.Drawing.Point(13, 264);
            this.labelMR.Name = "labelMR";
            this.labelMR.Size = new System.Drawing.Size(112, 23);
            this.labelMR.TabIndex = 101;
            this.labelMR.Text = "MR";
            // 
            // labelSMR
            // 
            this.labelSMR.Lines = 0;
            this.labelSMR.Location = new System.Drawing.Point(13, 300);
            this.labelSMR.Name = "labelSMR";
            this.labelSMR.Size = new System.Drawing.Size(112, 23);
            this.labelSMR.TabIndex = 102;
            this.labelSMR.Text = "SMR";
            // 
            // labelPOHandle
            // 
            this.labelPOHandle.Lines = 0;
            this.labelPOHandle.Location = new System.Drawing.Point(13, 336);
            this.labelPOHandle.Name = "labelPOHandle";
            this.labelPOHandle.Size = new System.Drawing.Size(112, 23);
            this.labelPOHandle.TabIndex = 103;
            this.labelPOHandle.Text = "P/O Handle";
            // 
            // labelPOSMR
            // 
            this.labelPOSMR.Lines = 0;
            this.labelPOSMR.Location = new System.Drawing.Point(13, 372);
            this.labelPOSMR.Name = "labelPOSMR";
            this.labelPOSMR.Size = new System.Drawing.Size(112, 23);
            this.labelPOSMR.TabIndex = 104;
            this.labelPOSMR.Text = "P/O SMR";
            // 
            // labelPrintType
            // 
            this.labelPrintType.Lines = 0;
            this.labelPrintType.Location = new System.Drawing.Point(13, 408);
            this.labelPrintType.Name = "labelPrintType";
            this.labelPrintType.Size = new System.Drawing.Size(112, 23);
            this.labelPrintType.TabIndex = 105;
            this.labelPrintType.Text = "Print Type";
            // 
            // dateSCIDelivery
            // 
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(129, 12);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 0;
            // 
            // dateProvideDate
            // 
            this.dateProvideDate.IsRequired = false;
            this.dateProvideDate.Location = new System.Drawing.Point(129, 48);
            this.dateProvideDate.Name = "dateProvideDate";
            this.dateProvideDate.Size = new System.Drawing.Size(280, 23);
            this.dateProvideDate.TabIndex = 1;
            // 
            // dateFtyMRRcvDate
            // 
            this.dateFtyMRRcvDate.IsRequired = false;
            this.dateFtyMRRcvDate.Location = new System.Drawing.Point(129, 84);
            this.dateFtyMRRcvDate.Name = "dateFtyMRRcvDate";
            this.dateFtyMRRcvDate.Size = new System.Drawing.Size(280, 23);
            this.dateFtyMRRcvDate.TabIndex = 2;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(129, 228);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(80, 24);
            this.comboM.TabIndex = 6;
            // 
            // comboPrintType
            // 
            this.comboPrintType.BackColor = System.Drawing.Color.White;
            this.comboPrintType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboPrintType.FormattingEnabled = true;
            this.comboPrintType.IsSupportUnselect = true;
            this.comboPrintType.Location = new System.Drawing.Point(129, 408);
            this.comboPrintType.Name = "comboPrintType";
            this.comboPrintType.Size = new System.Drawing.Size(194, 24);
            this.comboPrintType.TabIndex = 11;
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(129, 192);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(80, 23);
            this.txtseason.TabIndex = 5;
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(129, 156);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 4;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(129, 120);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 3;
            // 
            // txttpeuser_caneditMR
            // 
            this.txttpeuser_caneditMR.DisplayBox1Binding = "";
            this.txttpeuser_caneditMR.Location = new System.Drawing.Point(129, 264);
            this.txttpeuser_caneditMR.Name = "txttpeuser_caneditMR";
            this.txttpeuser_caneditMR.Size = new System.Drawing.Size(300, 23);
            this.txttpeuser_caneditMR.TabIndex = 7;
            this.txttpeuser_caneditMR.TextBox1Binding = "";
            // 
            // txttpeuser_caneditSMR
            // 
            this.txttpeuser_caneditSMR.DisplayBox1Binding = "";
            this.txttpeuser_caneditSMR.Location = new System.Drawing.Point(129, 300);
            this.txttpeuser_caneditSMR.Name = "txttpeuser_caneditSMR";
            this.txttpeuser_caneditSMR.Size = new System.Drawing.Size(300, 23);
            this.txttpeuser_caneditSMR.TabIndex = 8;
            this.txttpeuser_caneditSMR.TextBox1Binding = "";
            // 
            // txttpeuser_caneditPOHandle
            // 
            this.txttpeuser_caneditPOHandle.DisplayBox1Binding = "";
            this.txttpeuser_caneditPOHandle.Location = new System.Drawing.Point(129, 336);
            this.txttpeuser_caneditPOHandle.Name = "txttpeuser_caneditPOHandle";
            this.txttpeuser_caneditPOHandle.Size = new System.Drawing.Size(300, 23);
            this.txttpeuser_caneditPOHandle.TabIndex = 9;
            this.txttpeuser_caneditPOHandle.TextBox1Binding = "";
            // 
            // txttpeuser_caneditPOSMR
            // 
            this.txttpeuser_caneditPOSMR.DisplayBox1Binding = "";
            this.txttpeuser_caneditPOSMR.Location = new System.Drawing.Point(129, 372);
            this.txttpeuser_caneditPOSMR.Name = "txttpeuser_caneditPOSMR";
            this.txttpeuser_caneditPOSMR.Size = new System.Drawing.Size(300, 23);
            this.txttpeuser_caneditPOSMR.TabIndex = 10;
            this.txttpeuser_caneditPOSMR.TextBox1Binding = "";
            // 
            // R02
            // 
            this.ClientSize = new System.Drawing.Size(562, 470);
            this.Controls.Add(this.txttpeuser_caneditPOSMR);
            this.Controls.Add(this.txttpeuser_caneditPOHandle);
            this.Controls.Add(this.txttpeuser_caneditSMR);
            this.Controls.Add(this.txttpeuser_caneditMR);
            this.Controls.Add(this.comboPrintType);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.txtseason);
            this.Controls.Add(this.txtstyle);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.dateFtyMRRcvDate);
            this.Controls.Add(this.dateProvideDate);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.labelPrintType);
            this.Controls.Add(this.labelPOSMR);
            this.Controls.Add(this.labelPOHandle);
            this.Controls.Add(this.labelSMR);
            this.Controls.Add(this.labelMR);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelSeason);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelFtyMRRcvDate);
            this.Controls.Add(this.labelProvideDate);
            this.Controls.Add(this.labelSCIDelivery);
            this.DefaultControl = "dateSCIDelivery";
            this.DefaultControlForEdit = "dateSCIDelivery";
            this.IsSupportToPrint = false;
            this.Name = "R02";
            this.Text = "R02. Production Kits Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.labelProvideDate, 0);
            this.Controls.SetChildIndex(this.labelFtyMRRcvDate, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.labelSeason, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelMR, 0);
            this.Controls.SetChildIndex(this.labelSMR, 0);
            this.Controls.SetChildIndex(this.labelPOHandle, 0);
            this.Controls.SetChildIndex(this.labelPOSMR, 0);
            this.Controls.SetChildIndex(this.labelPrintType, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.dateProvideDate, 0);
            this.Controls.SetChildIndex(this.dateFtyMRRcvDate, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtstyle, 0);
            this.Controls.SetChildIndex(this.txtseason, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboPrintType, 0);
            this.Controls.SetChildIndex(this.txttpeuser_caneditMR, 0);
            this.Controls.SetChildIndex(this.txttpeuser_caneditSMR, 0);
            this.Controls.SetChildIndex(this.txttpeuser_caneditPOHandle, 0);
            this.Controls.SetChildIndex(this.txttpeuser_caneditPOSMR, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label labelProvideDate;
        private Win.UI.Label labelFtyMRRcvDate;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelStyle;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelM;
        private Win.UI.Label labelMR;
        private Win.UI.Label labelSMR;
        private Win.UI.Label labelPOHandle;
        private Win.UI.Label labelPOSMR;
        private Win.UI.Label labelPrintType;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.DateRange dateProvideDate;
        private Win.UI.DateRange dateFtyMRRcvDate;
        private Class.Txtbrand txtbrand;
        private Class.Txtstyle txtstyle;
        private Class.Txtseason txtseason;
        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboPrintType;
        private Class.Txttpeuser_canedit txttpeuser_caneditMR;
        private Class.Txttpeuser_canedit txttpeuser_caneditSMR;
        private Class.Txttpeuser_canedit txttpeuser_caneditPOHandle;
        private Class.Txttpeuser_canedit txttpeuser_caneditPOSMR;
    }
}
