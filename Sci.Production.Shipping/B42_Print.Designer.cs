namespace Sci.Production.Shipping
{
    partial class B42_Print
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
            this.labelDate = new Sci.Win.UI.Label();
            this.labelCustomSPNo = new Sci.Win.UI.Label();
            this.dateDate = new Sci.Win.UI.DateRange();
            this.txtCustomSPNoStart = new Sci.Win.UI.TextBox();
            this.txtCustomSPNoEnd = new Sci.Win.UI.TextBox();
            this.label3 = new Sci.Win.UI.Label();
            this.labelReportType = new Sci.Win.UI.Label();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioANNEX = new Sci.Win.UI.RadioButton();
            this.radioEachConsumption = new Sci.Win.UI.RadioButton();
            this.radioFormForCustomSystem = new Sci.Win.UI.RadioButton();
            this.lbContractNo = new Sci.Win.UI.Label();
            this.txtCustomsContract1 = new Sci.Production.Class.TxtCustomsContract();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(387, 12);
            this.print.TabIndex = 7;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(387, 48);
            this.toexcel.TabIndex = 8;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(387, 84);
            this.close.TabIndex = 9;
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(20, 12);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(83, 23);
            this.labelDate.TabIndex = 94;
            this.labelDate.Text = "Date";
            // 
            // labelCustomSPNo
            // 
            this.labelCustomSPNo.Location = new System.Drawing.Point(20, 37);
            this.labelCustomSPNo.Name = "labelCustomSPNo";
            this.labelCustomSPNo.Size = new System.Drawing.Size(83, 23);
            this.labelCustomSPNo.TabIndex = 95;
            this.labelCustomSPNo.Text = "Custom SP#";
            // 
            // dateDate
            // 
            // 
            // 
            // 
            this.dateDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateDate.DateBox1.Name = "";
            this.dateDate.DateBox1.Size = new System.Drawing.Size(114, 23);
            this.dateDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateDate.DateBox2.Location = new System.Drawing.Point(136, 0);
            this.dateDate.DateBox2.Name = "";
            this.dateDate.DateBox2.Size = new System.Drawing.Size(114, 23);
            this.dateDate.DateBox2.TabIndex = 1;
            this.dateDate.IsRequired = false;
            this.dateDate.Location = new System.Drawing.Point(107, 12);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(251, 23);
            this.dateDate.TabIndex = 0;
            // 
            // txtCustomSPNoStart
            // 
            this.txtCustomSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtCustomSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCustomSPNoStart.Location = new System.Drawing.Point(107, 37);
            this.txtCustomSPNoStart.Name = "txtCustomSPNoStart";
            this.txtCustomSPNoStart.Size = new System.Drawing.Size(80, 23);
            this.txtCustomSPNoStart.TabIndex = 1;
            // 
            // txtCustomSPNoEnd
            // 
            this.txtCustomSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtCustomSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCustomSPNoEnd.Location = new System.Drawing.Point(213, 37);
            this.txtCustomSPNoEnd.Name = "txtCustomSPNoEnd";
            this.txtCustomSPNoEnd.Size = new System.Drawing.Size(80, 23);
            this.txtCustomSPNoEnd.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(190, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 23);
            this.label3.TabIndex = 99;
            this.label3.Text = "～";
            this.label3.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            this.label3.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label3.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // labelReportType
            // 
            this.labelReportType.Location = new System.Drawing.Point(20, 87);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(83, 23);
            this.labelReportType.TabIndex = 100;
            this.labelReportType.Text = "Report Type";
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioANNEX);
            this.radioPanel1.Controls.Add(this.radioEachConsumption);
            this.radioPanel1.Controls.Add(this.radioFormForCustomSystem);
            this.radioPanel1.Location = new System.Drawing.Point(107, 87);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(193, 89);
            this.radioPanel1.TabIndex = 4;
            // 
            // radioANNEX
            // 
            this.radioANNEX.AutoSize = true;
            this.radioANNEX.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioANNEX.Location = new System.Drawing.Point(4, 60);
            this.radioANNEX.Name = "radioANNEX";
            this.radioANNEX.Size = new System.Drawing.Size(73, 21);
            this.radioANNEX.TabIndex = 6;
            this.radioANNEX.TabStop = true;
            this.radioANNEX.Text = "ANNEX";
            this.radioANNEX.UseVisualStyleBackColor = true;
            // 
            // radioEachConsumption
            // 
            this.radioEachConsumption.AutoSize = true;
            this.radioEachConsumption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioEachConsumption.Location = new System.Drawing.Point(4, 33);
            this.radioEachConsumption.Name = "radioEachConsumption";
            this.radioEachConsumption.Size = new System.Drawing.Size(142, 21);
            this.radioEachConsumption.TabIndex = 5;
            this.radioEachConsumption.TabStop = true;
            this.radioEachConsumption.Text = "Each consumption";
            this.radioEachConsumption.UseVisualStyleBackColor = true;
            // 
            // radioFormForCustomSystem
            // 
            this.radioFormForCustomSystem.AutoSize = true;
            this.radioFormForCustomSystem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioFormForCustomSystem.Location = new System.Drawing.Point(4, 4);
            this.radioFormForCustomSystem.Name = "radioFormForCustomSystem";
            this.radioFormForCustomSystem.Size = new System.Drawing.Size(176, 21);
            this.radioFormForCustomSystem.TabIndex = 4;
            this.radioFormForCustomSystem.TabStop = true;
            this.radioFormForCustomSystem.Text = "Form for custom system";
            this.radioFormForCustomSystem.UseVisualStyleBackColor = true;
            // 
            // lbContractNo
            // 
            this.lbContractNo.Location = new System.Drawing.Point(20, 62);
            this.lbContractNo.Name = "lbContractNo";
            this.lbContractNo.Size = new System.Drawing.Size(83, 23);
            this.lbContractNo.TabIndex = 101;
            this.lbContractNo.Text = "Contract no.";
            // 
            // txtCustomsContract1
            // 
            this.txtCustomsContract1.BackColor = System.Drawing.Color.White;
            this.txtCustomsContract1.CheckDate = false;
            this.txtCustomsContract1.CheckStatus = true;
            this.txtCustomsContract1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCustomsContract1.Location = new System.Drawing.Point(107, 62);
            this.txtCustomsContract1.Name = "txtCustomsContract1";
            this.txtCustomsContract1.Size = new System.Drawing.Size(186, 23);
            this.txtCustomsContract1.TabIndex = 3;
            // 
            // B42_Print
            // 
            this.ClientSize = new System.Drawing.Size(474, 221);
            this.Controls.Add(this.txtCustomsContract1);
            this.Controls.Add(this.lbContractNo);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.labelReportType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCustomSPNoEnd);
            this.Controls.Add(this.txtCustomSPNoStart);
            this.Controls.Add(this.dateDate);
            this.Controls.Add(this.labelCustomSPNo);
            this.Controls.Add(this.labelDate);
            this.IsSupportToPrint = false;
            this.Name = "B42_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelDate, 0);
            this.Controls.SetChildIndex(this.labelCustomSPNo, 0);
            this.Controls.SetChildIndex(this.dateDate, 0);
            this.Controls.SetChildIndex(this.txtCustomSPNoStart, 0);
            this.Controls.SetChildIndex(this.txtCustomSPNoEnd, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.labelReportType, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.lbContractNo, 0);
            this.Controls.SetChildIndex(this.txtCustomsContract1, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelDate;
        private Win.UI.Label labelCustomSPNo;
        private Win.UI.DateRange dateDate;
        private Win.UI.TextBox txtCustomSPNoStart;
        private Win.UI.TextBox txtCustomSPNoEnd;
        private Win.UI.Label label3;
        private Win.UI.Label labelReportType;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioANNEX;
        private Win.UI.RadioButton radioEachConsumption;
        private Win.UI.RadioButton radioFormForCustomSystem;
        private Win.UI.Label lbContractNo;
        private Class.TxtCustomsContract txtCustomsContract1;
    }
}
