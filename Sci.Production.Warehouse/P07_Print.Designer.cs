namespace Sci.Production.Warehouse
{
    partial class P07_Print
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
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.radioArriveWHReport = new Sci.Win.UI.RadioButton();
            this.radioPLRcvReport = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.txtSPNo);
            this.radioPanel1.Controls.Add(this.label1);
            this.radioPanel1.Controls.Add(this.radioArriveWHReport);
            this.radioPanel1.Controls.Add(this.radioPLRcvReport);
            this.radioPanel1.Location = new System.Drawing.Point(44, 25);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(454, 143);
            this.radioPanel1.TabIndex = 94;
            this.radioPanel1.Value = "P/L Rcv Report";
            this.radioPanel1.ValueChanged += new System.EventHandler(this.RadioGroup1_ValueChanged);
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(242, 59);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(121, 23);
            this.txtSPNo.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(200, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "SP#:";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // radioArriveWHReport
            // 
            this.radioArriveWHReport.AutoSize = true;
            this.radioArriveWHReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioArriveWHReport.Location = new System.Drawing.Point(32, 59);
            this.radioArriveWHReport.Name = "radioArriveWHReport";
            this.radioArriveWHReport.Size = new System.Drawing.Size(141, 21);
            this.radioArriveWHReport.TabIndex = 1;
            this.radioArriveWHReport.TabStop = true;
            this.radioArriveWHReport.Text = "Arrive W/H Report";
            this.radioArriveWHReport.UseVisualStyleBackColor = true;
            this.radioArriveWHReport.Value = "Arrive W/H Report";
            // 
            // radioPLRcvReport
            // 
            this.radioPLRcvReport.AutoSize = true;
            this.radioPLRcvReport.Checked = true;
            this.radioPLRcvReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioPLRcvReport.Location = new System.Drawing.Point(32, 24);
            this.radioPLRcvReport.Name = "radioPLRcvReport";
            this.radioPLRcvReport.Size = new System.Drawing.Size(122, 21);
            this.radioPLRcvReport.TabIndex = 0;
            this.radioPLRcvReport.TabStop = true;
            this.radioPLRcvReport.Text = "P/L Rcv Report";
            this.radioPLRcvReport.UseVisualStyleBackColor = true;
            this.radioPLRcvReport.Value = "P/L Rcv Report";
            this.radioPLRcvReport.CheckedChanged += new System.EventHandler(this.RadioPLRcvReport_CheckedChanged);
            // 
            // P07_Print
            // 
            this.ClientSize = new System.Drawing.Size(627, 314);
            this.Controls.Add(this.radioPanel1);
            this.Name = "P07_Print";
            this.Text = "() ";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioArriveWHReport;
        private Win.UI.RadioButton radioPLRcvReport;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label label1;

    }
}
