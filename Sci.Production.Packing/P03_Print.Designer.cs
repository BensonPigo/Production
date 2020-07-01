namespace Sci.Production.Packing
{
    partial class P03_Print
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
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioQRcodePrint = new Sci.Win.UI.RadioButton();
            this.radioWeighingform = new Sci.Win.UI.RadioButton();
            this.radioMDform = new Sci.Win.UI.RadioButton();
            this.chkCartonNo = new Sci.Win.UI.CheckBox();
            this.rdbtnShippingMarkToUsaInd = new Sci.Win.UI.RadioButton();
            this.rdbtnShippingMarkToChina = new Sci.Win.UI.RadioButton();
            this.rdbtnShippingMark = new Sci.Win.UI.RadioButton();
            this.radioNewBarcodePrint = new Sci.Win.UI.RadioButton();
            this.radioBarcodePrint = new Sci.Win.UI.RadioButton();
            this.radioPackingGuideReport = new Sci.Win.UI.RadioButton();
            this.radioPackingListReportFormB = new Sci.Win.UI.RadioButton();
            this.radioPackingListReportFormA = new Sci.Win.UI.RadioButton();
            this.txtCTNEnd = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txtCTNStart = new Sci.Win.UI.TextBox();
            this.labelCTN = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.checkBoxCountry = new Sci.Win.UI.CheckBox();
            this.radioCustCTN = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(479, 9);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(479, 45);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(479, 81);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioCustCTN);
            this.radioPanel1.Controls.Add(this.radioQRcodePrint);
            this.radioPanel1.Controls.Add(this.radioWeighingform);
            this.radioPanel1.Controls.Add(this.radioMDform);
            this.radioPanel1.Controls.Add(this.chkCartonNo);
            this.radioPanel1.Controls.Add(this.rdbtnShippingMarkToUsaInd);
            this.radioPanel1.Controls.Add(this.rdbtnShippingMarkToChina);
            this.radioPanel1.Controls.Add(this.rdbtnShippingMark);
            this.radioPanel1.Controls.Add(this.radioNewBarcodePrint);
            this.radioPanel1.Controls.Add(this.radioBarcodePrint);
            this.radioPanel1.Controls.Add(this.radioPackingGuideReport);
            this.radioPanel1.Controls.Add(this.radioPackingListReportFormB);
            this.radioPanel1.Controls.Add(this.radioPackingListReportFormA);
            this.radioPanel1.Location = new System.Drawing.Point(13, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(427, 311);
            this.radioPanel1.TabIndex = 94;
            // 
            // radioQRcodePrint
            // 
            this.radioQRcodePrint.AutoSize = true;
            this.radioQRcodePrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioQRcodePrint.Location = new System.Drawing.Point(3, 232);
            this.radioQRcodePrint.Name = "radioQRcodePrint";
            this.radioQRcodePrint.Size = new System.Drawing.Size(117, 21);
            this.radioQRcodePrint.TabIndex = 11;
            this.radioQRcodePrint.TabStop = true;
            this.radioQRcodePrint.Text = "QR Code Print";
            this.radioQRcodePrint.UseVisualStyleBackColor = true;
            this.radioQRcodePrint.CheckedChanged += new System.EventHandler(this.RadioQRcodePrint_CheckedChanged);
            // 
            // radioWeighingform
            // 
            this.radioWeighingform.AutoSize = true;
            this.radioWeighingform.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioWeighingform.Location = new System.Drawing.Point(3, 182);
            this.radioWeighingform.Name = "radioWeighingform";
            this.radioWeighingform.Size = new System.Drawing.Size(230, 21);
            this.radioWeighingform.TabIndex = 10;
            this.radioWeighingform.TabStop = true;
            this.radioWeighingform.Text = "Carton Weighing Logsheet Form";
            this.radioWeighingform.UseVisualStyleBackColor = true;
            this.radioWeighingform.CheckedChanged += new System.EventHandler(this.RadioMDform_CheckedChanged);
            // 
            // radioMDform
            // 
            this.radioMDform.AutoSize = true;
            this.radioMDform.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioMDform.Location = new System.Drawing.Point(3, 157);
            this.radioMDform.Name = "radioMDform";
            this.radioMDform.Size = new System.Drawing.Size(130, 21);
            this.radioMDform.TabIndex = 9;
            this.radioMDform.TabStop = true;
            this.radioMDform.Text = "MD Form Report";
            this.radioMDform.UseVisualStyleBackColor = true;
            this.radioMDform.CheckedChanged += new System.EventHandler(this.RadioMDform_CheckedChanged);
            // 
            // chkCartonNo
            // 
            this.chkCartonNo.AutoSize = true;
            this.chkCartonNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkCartonNo.Location = new System.Drawing.Point(250, 83);
            this.chkCartonNo.Name = "chkCartonNo";
            this.chkCartonNo.Size = new System.Drawing.Size(95, 21);
            this.chkCartonNo.TabIndex = 8;
            this.chkCartonNo.Text = "Carton No.";
            this.chkCartonNo.UseVisualStyleBackColor = true;
            // 
            // rdbtnShippingMarkToUsaInd
            // 
            this.rdbtnShippingMarkToUsaInd.AutoSize = true;
            this.rdbtnShippingMarkToUsaInd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtnShippingMarkToUsaInd.Location = new System.Drawing.Point(3, 107);
            this.rdbtnShippingMarkToUsaInd.Name = "rdbtnShippingMarkToUsaInd";
            this.rdbtnShippingMarkToUsaInd.Size = new System.Drawing.Size(321, 21);
            this.rdbtnShippingMarkToUsaInd.TabIndex = 7;
            this.rdbtnShippingMarkToUsaInd.TabStop = true;
            this.rdbtnShippingMarkToUsaInd.Text = "Packing Shipping Mark (To USA - Indianapolis)";
            this.rdbtnShippingMarkToUsaInd.UseVisualStyleBackColor = true;
            this.rdbtnShippingMarkToUsaInd.CheckedChanged += new System.EventHandler(this.RdbtnShippingMarkToUsaInd_CheckedChanged);
            // 
            // rdbtnShippingMarkToChina
            // 
            this.rdbtnShippingMarkToChina.AutoSize = true;
            this.rdbtnShippingMarkToChina.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtnShippingMarkToChina.Location = new System.Drawing.Point(3, 82);
            this.rdbtnShippingMarkToChina.Name = "rdbtnShippingMarkToChina";
            this.rdbtnShippingMarkToChina.Size = new System.Drawing.Size(241, 21);
            this.rdbtnShippingMarkToChina.TabIndex = 6;
            this.rdbtnShippingMarkToChina.TabStop = true;
            this.rdbtnShippingMarkToChina.Text = "Packing Shipping Mark (To China)";
            this.rdbtnShippingMarkToChina.UseVisualStyleBackColor = true;
            this.rdbtnShippingMarkToChina.CheckedChanged += new System.EventHandler(this.RdbtnShippingMarkToChina_CheckedChanged);
            // 
            // rdbtnShippingMark
            // 
            this.rdbtnShippingMark.AutoSize = true;
            this.rdbtnShippingMark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtnShippingMark.Location = new System.Drawing.Point(3, 57);
            this.rdbtnShippingMark.Name = "rdbtnShippingMark";
            this.rdbtnShippingMark.Size = new System.Drawing.Size(170, 21);
            this.rdbtnShippingMark.TabIndex = 5;
            this.rdbtnShippingMark.TabStop = true;
            this.rdbtnShippingMark.Text = "Packing Shipping Mark";
            this.rdbtnShippingMark.UseVisualStyleBackColor = true;
            this.rdbtnShippingMark.CheckedChanged += new System.EventHandler(this.RadioBarcodePrint_CheckedChanged);
            // 
            // radioNewBarcodePrint
            // 
            this.radioNewBarcodePrint.AutoSize = true;
            this.radioNewBarcodePrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioNewBarcodePrint.Location = new System.Drawing.Point(3, 257);
            this.radioNewBarcodePrint.Name = "radioNewBarcodePrint";
            this.radioNewBarcodePrint.Size = new System.Drawing.Size(191, 21);
            this.radioNewBarcodePrint.TabIndex = 4;
            this.radioNewBarcodePrint.TabStop = true;
            this.radioNewBarcodePrint.Text = "New Barcode Format Print";
            this.radioNewBarcodePrint.UseVisualStyleBackColor = true;
            this.radioNewBarcodePrint.CheckedChanged += new System.EventHandler(this.RadioBarcodePrint_CheckedChanged);
            // 
            // radioBarcodePrint
            // 
            this.radioBarcodePrint.AutoSize = true;
            this.radioBarcodePrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBarcodePrint.Location = new System.Drawing.Point(3, 207);
            this.radioBarcodePrint.Name = "radioBarcodePrint";
            this.radioBarcodePrint.Size = new System.Drawing.Size(112, 21);
            this.radioBarcodePrint.TabIndex = 3;
            this.radioBarcodePrint.TabStop = true;
            this.radioBarcodePrint.Text = "Barcode Print";
            this.radioBarcodePrint.UseVisualStyleBackColor = true;
            this.radioBarcodePrint.CheckedChanged += new System.EventHandler(this.RadioBarcodePrint_CheckedChanged);
            // 
            // radioPackingGuideReport
            // 
            this.radioPackingGuideReport.AutoSize = true;
            this.radioPackingGuideReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioPackingGuideReport.Location = new System.Drawing.Point(3, 132);
            this.radioPackingGuideReport.Name = "radioPackingGuideReport";
            this.radioPackingGuideReport.Size = new System.Drawing.Size(165, 21);
            this.radioPackingGuideReport.TabIndex = 2;
            this.radioPackingGuideReport.TabStop = true;
            this.radioPackingGuideReport.Text = "Packing Guide Report";
            this.radioPackingGuideReport.UseVisualStyleBackColor = true;
            // 
            // radioPackingListReportFormB
            // 
            this.radioPackingListReportFormB.AutoSize = true;
            this.radioPackingListReportFormB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioPackingListReportFormB.Location = new System.Drawing.Point(3, 32);
            this.radioPackingListReportFormB.Name = "radioPackingListReportFormB";
            this.radioPackingListReportFormB.Size = new System.Drawing.Size(288, 21);
            this.radioPackingListReportFormB.TabIndex = 1;
            this.radioPackingListReportFormB.TabStop = true;
            this.radioPackingListReportFormB.Text = "Packing List Report Form B (for LLL/TNF)";
            this.radioPackingListReportFormB.UseVisualStyleBackColor = true;
            // 
            // radioPackingListReportFormA
            // 
            this.radioPackingListReportFormA.AutoSize = true;
            this.radioPackingListReportFormA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioPackingListReportFormA.Location = new System.Drawing.Point(3, 7);
            this.radioPackingListReportFormA.Name = "radioPackingListReportFormA";
            this.radioPackingListReportFormA.Size = new System.Drawing.Size(381, 21);
            this.radioPackingListReportFormA.TabIndex = 0;
            this.radioPackingListReportFormA.TabStop = true;
            this.radioPackingListReportFormA.Text = "Packing List Report Form A (for Adidas/UA/Saucony/NB)";
            this.radioPackingListReportFormA.UseVisualStyleBackColor = true;
            // 
            // txtCTNEnd
            // 
            this.txtCTNEnd.BackColor = System.Drawing.Color.White;
            this.txtCTNEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCTNEnd.Location = new System.Drawing.Point(171, 340);
            this.txtCTNEnd.Name = "txtCTNEnd";
            this.txtCTNEnd.Size = new System.Drawing.Size(56, 23);
            this.txtCTNEnd.TabIndex = 98;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(148, 340);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 23);
            this.label2.TabIndex = 97;
            this.label2.Text = "～";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            this.label2.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtCTNStart
            // 
            this.txtCTNStart.BackColor = System.Drawing.Color.White;
            this.txtCTNStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCTNStart.Location = new System.Drawing.Point(88, 340);
            this.txtCTNStart.Name = "txtCTNStart";
            this.txtCTNStart.Size = new System.Drawing.Size(56, 23);
            this.txtCTNStart.TabIndex = 96;
            // 
            // labelCTN
            // 
            this.labelCTN.Location = new System.Drawing.Point(41, 340);
            this.labelCTN.Name = "labelCTN";
            this.labelCTN.Size = new System.Drawing.Size(43, 23);
            this.labelCTN.TabIndex = 95;
            this.labelCTN.Text = "CTN#";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(230, 340);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 23);
            this.label1.TabIndex = 99;
            this.label1.Text = "country of origin";
            // 
            // checkBoxCountry
            // 
            this.checkBoxCountry.AutoSize = true;
            this.checkBoxCountry.Enabled = false;
            this.checkBoxCountry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxCountry.Location = new System.Drawing.Point(349, 345);
            this.checkBoxCountry.Name = "checkBoxCountry";
            this.checkBoxCountry.Size = new System.Drawing.Size(15, 14);
            this.checkBoxCountry.TabIndex = 100;
            this.checkBoxCountry.UseVisualStyleBackColor = true;
            // 
            // radioCustCTN
            // 
            this.radioCustCTN.AutoSize = true;
            this.radioCustCTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioCustCTN.Location = new System.Drawing.Point(3, 284);
            this.radioCustCTN.Name = "radioCustCTN";
            this.radioCustCTN.Size = new System.Drawing.Size(210, 21);
            this.radioCustCTN.TabIndex = 12;
            this.radioCustCTN.TabStop = true;
            this.radioCustCTN.Text = "Cust CTN# Barcode List Print";
            this.radioCustCTN.UseVisualStyleBackColor = true;
            this.radioCustCTN.CheckedChanged += new System.EventHandler(this.RadioCustCTN_CheckedChanged);
            // 
            // P03_Print
            // 
            this.ClientSize = new System.Drawing.Size(571, 405);
            this.Controls.Add(this.checkBoxCountry);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCTNEnd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCTNStart);
            this.Controls.Add(this.labelCTN);
            this.Controls.Add(this.radioPanel1);
            this.Name = "P03_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.labelCTN, 0);
            this.Controls.SetChildIndex(this.txtCTNStart, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtCTNEnd, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.checkBoxCountry, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioBarcodePrint;
        private Win.UI.RadioButton radioPackingGuideReport;
        private Win.UI.RadioButton radioPackingListReportFormB;
        private Win.UI.RadioButton radioPackingListReportFormA;
        private Win.UI.TextBox txtCTNEnd;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtCTNStart;
        private Win.UI.Label labelCTN;
        private Win.UI.RadioButton radioNewBarcodePrint;
        private Win.UI.Label label1;
        private Win.UI.CheckBox checkBoxCountry;
        private Win.UI.RadioButton rdbtnShippingMark;
        private Win.UI.RadioButton rdbtnShippingMarkToUsaInd;
        private Win.UI.RadioButton rdbtnShippingMarkToChina;
        private Win.UI.CheckBox chkCartonNo;
        private Win.UI.RadioButton radioWeighingform;
        private Win.UI.RadioButton radioMDform;
        private Win.UI.RadioButton radioQRcodePrint;
        private Win.UI.RadioButton radioCustCTN;
    }
}
