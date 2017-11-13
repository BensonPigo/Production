namespace Sci.Production.Packing
{
    partial class P06_Print
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
            this.radioBarcodePrint = new Sci.Win.UI.RadioButton();
            this.radioPackingGuideReport = new Sci.Win.UI.RadioButton();
            this.labelCTN = new Sci.Win.UI.Label();
            this.txtCTNStart = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txtCTNEnd = new Sci.Win.UI.TextBox();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(288, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(288, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(288, 84);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioBarcodePrint);
            this.radioPanel1.Controls.Add(this.radioPackingGuideReport);
            this.radioPanel1.Location = new System.Drawing.Point(20, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(198, 70);
            this.radioPanel1.TabIndex = 94;
            // 
            // radioBarcodePrint
            // 
            this.radioBarcodePrint.AutoSize = true;
            this.radioBarcodePrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBarcodePrint.Location = new System.Drawing.Point(13, 47);
            this.radioBarcodePrint.Name = "radioBarcodePrint";
            this.radioBarcodePrint.Size = new System.Drawing.Size(112, 21);
            this.radioBarcodePrint.TabIndex = 1;
            this.radioBarcodePrint.TabStop = true;
            this.radioBarcodePrint.Text = "Barcode Print";
            this.radioBarcodePrint.UseVisualStyleBackColor = true;
            // 
            // radioPackingGuideReport
            // 
            this.radioPackingGuideReport.AutoSize = true;
            this.radioPackingGuideReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioPackingGuideReport.Location = new System.Drawing.Point(13, 11);
            this.radioPackingGuideReport.Name = "radioPackingGuideReport";
            this.radioPackingGuideReport.Size = new System.Drawing.Size(165, 21);
            this.radioPackingGuideReport.TabIndex = 0;
            this.radioPackingGuideReport.TabStop = true;
            this.radioPackingGuideReport.Text = "Packing Guide Report";
            this.radioPackingGuideReport.UseVisualStyleBackColor = true;
            this.radioPackingGuideReport.CheckedChanged += new System.EventHandler(this.RadioPackingGuideReport_CheckedChanged);
            // 
            // labelCTN
            // 
            this.labelCTN.Lines = 0;
            this.labelCTN.Location = new System.Drawing.Point(55, 86);
            this.labelCTN.Name = "labelCTN";
            this.labelCTN.Size = new System.Drawing.Size(43, 23);
            this.labelCTN.TabIndex = 2;
            this.labelCTN.Text = "CTN#";
            // 
            // txtCTNStart
            // 
            this.txtCTNStart.BackColor = System.Drawing.Color.White;
            this.txtCTNStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCTNStart.Location = new System.Drawing.Point(102, 86);
            this.txtCTNStart.Name = "txtCTNStart";
            this.txtCTNStart.Size = new System.Drawing.Size(56, 23);
            this.txtCTNStart.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(162, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "～";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            this.label2.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtCTNEnd
            // 
            this.txtCTNEnd.BackColor = System.Drawing.Color.White;
            this.txtCTNEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCTNEnd.Location = new System.Drawing.Point(185, 86);
            this.txtCTNEnd.Name = "txtCTNEnd";
            this.txtCTNEnd.Size = new System.Drawing.Size(56, 23);
            this.txtCTNEnd.TabIndex = 5;
            // 
            // P06_Print
            // 
            this.ClientSize = new System.Drawing.Size(380, 159);
            this.Controls.Add(this.txtCTNEnd);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCTNStart);
            this.Controls.Add(this.labelCTN);
            this.Name = "P06_Print";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.labelCTN, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.txtCTNStart, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.txtCTNEnd, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.Label labelCTN;
        private Win.UI.RadioButton radioBarcodePrint;
        private Win.UI.RadioButton radioPackingGuideReport;
        private Win.UI.TextBox txtCTNStart;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtCTNEnd;
    }
}
