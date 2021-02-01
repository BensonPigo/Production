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
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtcustcd = new Sci.Production.Class.Txtcustcd();
            this.txtcountryDestination = new Sci.Production.Class.Txtcountry();
            this.txtshipmode = new Sci.Production.Class.Txtshipmode();
            this.displayForwarder = new Sci.Win.UI.DisplayBox();
            this.txtForwarder = new Sci.Win.UI.TextBox();
            this.dateOnBoardDate = new Sci.Win.UI.DateRange();
            this.labelOnBoardDate = new Sci.Win.UI.Label();
            this.radioPanel2 = new Sci.Win.UI.RadioPanel();
            this.rdbtnDetailList = new Sci.Win.UI.RadioButton();
            this.rdbtnMainList = new Sci.Win.UI.RadioButton();
            this.label1 = new Sci.Win.UI.Label();
            this.chkExcludePackingFOC = new Sci.Win.UI.CheckBox();
            this.chkExcludePackingLocalOrder = new Sci.Win.UI.CheckBox();
            this.radioPanel1.SuspendLayout();
            this.radioPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(453, 12);
            this.print.TabIndex = 12;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(453, 48);
            this.toexcel.TabIndex = 13;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(453, 84);
            this.close.TabIndex = 14;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(407, 120);
            this.buttonCustomized.TabIndex = 15;
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(433, 156);
            this.checkUseCustomized.TabIndex = 16;
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(433, 183);
            // 
            // labelReportContent
            // 
            this.labelReportContent.Location = new System.Drawing.Point(13, 12);
            this.labelReportContent.Name = "labelReportContent";
            this.labelReportContent.Size = new System.Drawing.Size(101, 23);
            this.labelReportContent.TabIndex = 94;
            this.labelReportContent.Text = "Report Content";
            // 
            // labelPulloutDate
            // 
            this.labelPulloutDate.Location = new System.Drawing.Point(13, 120);
            this.labelPulloutDate.Name = "labelPulloutDate";
            this.labelPulloutDate.Size = new System.Drawing.Size(101, 23);
            this.labelPulloutDate.TabIndex = 95;
            this.labelPulloutDate.Text = "Pullout Date";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 184);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(101, 23);
            this.labelBrand.TabIndex = 96;
            this.labelBrand.Text = "Brand";
            // 
            // labelCustCD
            // 
            this.labelCustCD.Location = new System.Drawing.Point(13, 220);
            this.labelCustCD.Name = "labelCustCD";
            this.labelCustCD.Size = new System.Drawing.Size(101, 23);
            this.labelCustCD.TabIndex = 97;
            this.labelCustCD.Text = "Cust CD";
            // 
            // labelDestination
            // 
            this.labelDestination.Location = new System.Drawing.Point(13, 257);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(101, 23);
            this.labelDestination.TabIndex = 98;
            this.labelDestination.Text = "Destination";
            // 
            // labelShipMode
            // 
            this.labelShipMode.Location = new System.Drawing.Point(13, 293);
            this.labelShipMode.Name = "labelShipMode";
            this.labelShipMode.Size = new System.Drawing.Size(101, 23);
            this.labelShipMode.TabIndex = 99;
            this.labelShipMode.Text = "Ship Mode";
            // 
            // labelForwarder
            // 
            this.labelForwarder.Location = new System.Drawing.Point(13, 330);
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
            this.radioRawMaterial.CheckedChanged += new System.EventHandler(this.RadioRawMaterial_CheckedChanged);
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
            // 
            // 
            // 
            this.datePulloutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.datePulloutDate.DateBox1.Name = "";
            this.datePulloutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.datePulloutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.datePulloutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.datePulloutDate.DateBox2.Name = "";
            this.datePulloutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.datePulloutDate.DateBox2.TabIndex = 1;
            this.datePulloutDate.IsRequired = false;
            this.datePulloutDate.Location = new System.Drawing.Point(120, 120);
            this.datePulloutDate.Name = "datePulloutDate";
            this.datePulloutDate.Size = new System.Drawing.Size(280, 23);
            this.datePulloutDate.TabIndex = 4;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(120, 184);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 6;
            // 
            // txtcustcd
            // 
            this.txtcustcd.BackColor = System.Drawing.Color.White;
            this.txtcustcd.BrandObjectName = null;
            this.txtcustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd.Location = new System.Drawing.Point(120, 220);
            this.txtcustcd.Name = "txtcustcd";
            this.txtcustcd.Size = new System.Drawing.Size(125, 23);
            this.txtcustcd.TabIndex = 7;
            // 
            // txtcountryDestination
            // 
            this.txtcountryDestination.DisplayBox1Binding = "";
            this.txtcountryDestination.Location = new System.Drawing.Point(120, 257);
            this.txtcountryDestination.Name = "txtcountryDestination";
            this.txtcountryDestination.Size = new System.Drawing.Size(232, 22);
            this.txtcountryDestination.TabIndex = 8;
            this.txtcountryDestination.TextBox1Binding = "";
            // 
            // txtshipmode
            // 
            this.txtshipmode.BackColor = System.Drawing.Color.White;
            this.txtshipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshipmode.FormattingEnabled = true;
            this.txtshipmode.IsSupportUnselect = true;
            this.txtshipmode.Location = new System.Drawing.Point(120, 293);
            this.txtshipmode.Name = "txtshipmode";
            this.txtshipmode.OldText = "";
            this.txtshipmode.Size = new System.Drawing.Size(80, 24);
            this.txtshipmode.TabIndex = 9;
            this.txtshipmode.UseFunction = null;
            // 
            // displayForwarder
            // 
            this.displayForwarder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayForwarder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayForwarder.Location = new System.Drawing.Point(183, 331);
            this.displayForwarder.Name = "displayForwarder";
            this.displayForwarder.Size = new System.Drawing.Size(100, 23);
            this.displayForwarder.TabIndex = 11;
            // 
            // txtForwarder
            // 
            this.txtForwarder.BackColor = System.Drawing.Color.White;
            this.txtForwarder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtForwarder.Location = new System.Drawing.Point(120, 330);
            this.txtForwarder.Name = "txtForwarder";
            this.txtForwarder.Size = new System.Drawing.Size(61, 23);
            this.txtForwarder.TabIndex = 10;
            this.txtForwarder.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtForwarder_PopUp);
            this.txtForwarder.Validating += new System.ComponentModel.CancelEventHandler(this.TxtForwarder_Validating);
            // 
            // dateOnBoardDate
            // 
            // 
            // 
            // 
            this.dateOnBoardDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateOnBoardDate.DateBox1.Name = "";
            this.dateOnBoardDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateOnBoardDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateOnBoardDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateOnBoardDate.DateBox2.Name = "";
            this.dateOnBoardDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateOnBoardDate.DateBox2.TabIndex = 1;
            this.dateOnBoardDate.IsRequired = false;
            this.dateOnBoardDate.Location = new System.Drawing.Point(118, 152);
            this.dateOnBoardDate.Name = "dateOnBoardDate";
            this.dateOnBoardDate.Size = new System.Drawing.Size(280, 23);
            this.dateOnBoardDate.TabIndex = 5;
            // 
            // labelOnBoardDate
            // 
            this.labelOnBoardDate.Location = new System.Drawing.Point(13, 152);
            this.labelOnBoardDate.Name = "labelOnBoardDate";
            this.labelOnBoardDate.Size = new System.Drawing.Size(101, 23);
            this.labelOnBoardDate.TabIndex = 114;
            this.labelOnBoardDate.Text = "On Board Date";
            // 
            // radioPanel2
            // 
            this.radioPanel2.Controls.Add(this.rdbtnDetailList);
            this.radioPanel2.Controls.Add(this.rdbtnMainList);
            this.radioPanel2.Location = new System.Drawing.Point(117, 62);
            this.radioPanel2.Name = "radioPanel2";
            this.radioPanel2.Size = new System.Drawing.Size(112, 52);
            this.radioPanel2.TabIndex = 103;
            // 
            // rdbtnDetailList
            // 
            this.rdbtnDetailList.AutoSize = true;
            this.rdbtnDetailList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtnDetailList.Location = new System.Drawing.Point(3, 29);
            this.rdbtnDetailList.Name = "rdbtnDetailList";
            this.rdbtnDetailList.Size = new System.Drawing.Size(88, 21);
            this.rdbtnDetailList.TabIndex = 3;
            this.rdbtnDetailList.TabStop = true;
            this.rdbtnDetailList.Text = "Detail List";
            this.rdbtnDetailList.UseVisualStyleBackColor = true;
            // 
            // rdbtnMainList
            // 
            this.rdbtnMainList.AutoSize = true;
            this.rdbtnMainList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtnMainList.Location = new System.Drawing.Point(3, 2);
            this.rdbtnMainList.Name = "rdbtnMainList";
            this.rdbtnMainList.Size = new System.Drawing.Size(82, 21);
            this.rdbtnMainList.TabIndex = 2;
            this.rdbtnMainList.TabStop = true;
            this.rdbtnMainList.Text = "Main List";
            this.rdbtnMainList.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 23);
            this.label1.TabIndex = 102;
            this.label1.Text = "Report Type";
            // 
            // chkExcludePackingFOC
            // 
            this.chkExcludePackingFOC.AutoSize = true;
            this.chkExcludePackingFOC.Checked = true;
            this.chkExcludePackingFOC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExcludePackingFOC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkExcludePackingFOC.Location = new System.Drawing.Point(13, 359);
            this.chkExcludePackingFOC.Name = "chkExcludePackingFOC";
            this.chkExcludePackingFOC.Size = new System.Drawing.Size(162, 21);
            this.chkExcludePackingFOC.TabIndex = 116;
            this.chkExcludePackingFOC.Text = "Exclude Packing FOC";
            this.chkExcludePackingFOC.UseVisualStyleBackColor = true;
            // 
            // chkExcludePackingLocalOrder
            // 
            this.chkExcludePackingLocalOrder.AutoSize = true;
            this.chkExcludePackingLocalOrder.Checked = true;
            this.chkExcludePackingLocalOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExcludePackingLocalOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkExcludePackingLocalOrder.Location = new System.Drawing.Point(13, 386);
            this.chkExcludePackingLocalOrder.Name = "chkExcludePackingLocalOrder";
            this.chkExcludePackingLocalOrder.Size = new System.Drawing.Size(209, 21);
            this.chkExcludePackingLocalOrder.TabIndex = 117;
            this.chkExcludePackingLocalOrder.Text = "Exclude Packing Local Order";
            this.chkExcludePackingLocalOrder.UseVisualStyleBackColor = true;
            // 
            // R11
            // 
            this.ClientSize = new System.Drawing.Size(545, 437);
            this.Controls.Add(this.chkExcludePackingLocalOrder);
            this.Controls.Add(this.chkExcludePackingFOC);
            this.Controls.Add(this.radioPanel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateOnBoardDate);
            this.Controls.Add(this.labelOnBoardDate);
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
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R11. Non Shared List";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
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
            this.Controls.SetChildIndex(this.labelOnBoardDate, 0);
            this.Controls.SetChildIndex(this.dateOnBoardDate, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.radioPanel2, 0);
            this.Controls.SetChildIndex(this.chkExcludePackingFOC, 0);
            this.Controls.SetChildIndex(this.chkExcludePackingLocalOrder, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.radioPanel2.ResumeLayout(false);
            this.radioPanel2.PerformLayout();
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
        private Class.Txtbrand txtbrand;
        private Class.Txtcustcd txtcustcd;
        private Class.Txtcountry txtcountryDestination;
        private Class.Txtshipmode txtshipmode;
        private Win.UI.DisplayBox displayForwarder;
        private Win.UI.TextBox txtForwarder;
        private Win.UI.DateRange dateOnBoardDate;
        private Win.UI.Label labelOnBoardDate;
        private Win.UI.RadioPanel radioPanel2;
        private Win.UI.RadioButton rdbtnDetailList;
        private Win.UI.RadioButton rdbtnMainList;
        private Win.UI.Label label1;
        private Win.UI.CheckBox chkExcludePackingFOC;
        private Win.UI.CheckBox chkExcludePackingLocalOrder;
    }
}
