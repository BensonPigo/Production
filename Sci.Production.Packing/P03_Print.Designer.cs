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
            this.radioNewBarcodePrint = new Sci.Win.UI.RadioButton();
            this.radioBarcodePrint = new Sci.Win.UI.RadioButton();
            this.radioPackingGuideReport = new Sci.Win.UI.RadioButton();
            this.radioPackingListReportFormB = new Sci.Win.UI.RadioButton();
            this.radioPackingListReportFormA = new Sci.Win.UI.RadioButton();
            this.txtCTNEnd = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txtCTNStart = new Sci.Win.UI.TextBox();
            this.labelCTN = new Sci.Win.UI.Label();
            this.txtcountry1 = new Sci.Production.Class.txtcountry();
            this.label1 = new Sci.Win.UI.Label();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(476, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(476, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(476, 84);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioNewBarcodePrint);
            this.radioPanel1.Controls.Add(this.radioBarcodePrint);
            this.radioPanel1.Controls.Add(this.radioPackingGuideReport);
            this.radioPanel1.Controls.Add(this.radioPackingListReportFormB);
            this.radioPanel1.Controls.Add(this.radioPackingListReportFormA);
            this.radioPanel1.Location = new System.Drawing.Point(13, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(427, 125);
            this.radioPanel1.TabIndex = 94;
            // 
            // radioNewBarcodePrint
            // 
            this.radioNewBarcodePrint.AutoSize = true;
            this.radioNewBarcodePrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioNewBarcodePrint.Location = new System.Drawing.Point(3, 100);
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
            this.radioBarcodePrint.Location = new System.Drawing.Point(3, 77);
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
            this.radioPackingGuideReport.Location = new System.Drawing.Point(3, 54);
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
            this.radioPackingListReportFormB.Location = new System.Drawing.Point(3, 27);
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
            this.radioPackingListReportFormA.Location = new System.Drawing.Point(3, 0);
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
            this.txtCTNEnd.Location = new System.Drawing.Point(166, 140);
            this.txtCTNEnd.Name = "txtCTNEnd";
            this.txtCTNEnd.Size = new System.Drawing.Size(56, 23);
            this.txtCTNEnd.TabIndex = 98;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(143, 140);
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
            this.txtCTNStart.Location = new System.Drawing.Point(83, 140);
            this.txtCTNStart.Name = "txtCTNStart";
            this.txtCTNStart.Size = new System.Drawing.Size(56, 23);
            this.txtCTNStart.TabIndex = 96;
            // 
            // labelCTN
            // 
            this.labelCTN.Location = new System.Drawing.Point(36, 140);
            this.labelCTN.Name = "labelCTN";
            this.labelCTN.Size = new System.Drawing.Size(43, 23);
            this.labelCTN.TabIndex = 95;
            this.labelCTN.Text = "CTN#";
            // 
            // txtcountry1
            // 
            this.txtcountry1.DisplayBox1Binding = "";
            this.txtcountry1.Enabled = false;
            this.txtcountry1.Location = new System.Drawing.Point(333, 140);
            this.txtcountry1.Name = "txtcountry1";
            this.txtcountry1.Size = new System.Drawing.Size(232, 22);
            this.txtcountry1.TabIndex = 5;
            this.txtcountry1.TextBox1Binding = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(225, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 23);
            this.label1.TabIndex = 99;
            this.label1.Text = "country of origin";
            // 
            // P03_Print
            // 
            this.ClientSize = new System.Drawing.Size(568, 192);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtcountry1);
            this.Controls.Add(this.txtCTNEnd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCTNStart);
            this.Controls.Add(this.labelCTN);
            this.Controls.Add(this.radioPanel1);
            this.Name = "P03_Print";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.labelCTN, 0);
            this.Controls.SetChildIndex(this.txtCTNStart, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtCTNEnd, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.txtcountry1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
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
        private Class.txtcountry txtcountry1;
        private Win.UI.Label label1;
    }
}
