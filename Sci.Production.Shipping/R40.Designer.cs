namespace Sci.Production.Shipping
{
    partial class R40
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
            this.labelContractNo = new Sci.Win.UI.Label();
            this.labelHSCode = new Sci.Win.UI.Label();
            this.labelNLCode = new Sci.Win.UI.Label();
            this.checkLiquidationDataOnly = new Sci.Win.UI.CheckBox();
            this.label5 = new Sci.Win.UI.Label();
            this.txtContractNo = new Sci.Win.UI.TextBox();
            this.txtHSCode = new Sci.Win.UI.TextBox();
            this.txtNLCode = new Sci.Win.UI.TextBox();
            this.labGenerateDate = new Sci.Win.UI.Label();
            this.dateGenerate = new Sci.Win.UI.DateBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(373, 18);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(373, 54);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(373, 90);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(214, 175);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(20, 182);
            // 
            // labelContractNo
            // 
            this.labelContractNo.Location = new System.Drawing.Point(20, 12);
            this.labelContractNo.Name = "labelContractNo";
            this.labelContractNo.Size = new System.Drawing.Size(96, 23);
            this.labelContractNo.TabIndex = 94;
            this.labelContractNo.Text = "Contract no.";
            // 
            // labelHSCode
            // 
            this.labelHSCode.Location = new System.Drawing.Point(20, 40);
            this.labelHSCode.Name = "labelHSCode";
            this.labelHSCode.Size = new System.Drawing.Size(96, 23);
            this.labelHSCode.TabIndex = 95;
            this.labelHSCode.Text = "HS Code";
            // 
            // labelNLCode
            // 
            this.labelNLCode.Location = new System.Drawing.Point(20, 68);
            this.labelNLCode.Name = "labelNLCode";
            this.labelNLCode.Size = new System.Drawing.Size(96, 23);
            this.labelNLCode.TabIndex = 96;
            this.labelNLCode.Text = "Customs Code";
            // 
            // checkLiquidationDataOnly
            // 
            this.checkLiquidationDataOnly.AutoSize = true;
            this.checkLiquidationDataOnly.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkLiquidationDataOnly.Location = new System.Drawing.Point(20, 126);
            this.checkLiquidationDataOnly.Name = "checkLiquidationDataOnly";
            this.checkLiquidationDataOnly.Size = new System.Drawing.Size(158, 21);
            this.checkLiquidationDataOnly.TabIndex = 98;
            this.checkLiquidationDataOnly.Text = "Liquidation data only";
            this.checkLiquidationDataOnly.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(20, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 23);
            this.label5.TabIndex = 99;
            this.label5.Text = "Unit: Cutsoms Unit";
            this.label5.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label5.TextStyle.Color = System.Drawing.Color.Black;
            this.label5.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label5.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtContractNo
            // 
            this.txtContractNo.BackColor = System.Drawing.Color.White;
            this.txtContractNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtContractNo.Location = new System.Drawing.Point(120, 12);
            this.txtContractNo.Name = "txtContractNo";
            this.txtContractNo.Size = new System.Drawing.Size(150, 23);
            this.txtContractNo.TabIndex = 100;
            this.txtContractNo.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtContractNo_PopUp);
            // 
            // txtHSCode
            // 
            this.txtHSCode.BackColor = System.Drawing.Color.White;
            this.txtHSCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtHSCode.Location = new System.Drawing.Point(120, 40);
            this.txtHSCode.Name = "txtHSCode";
            this.txtHSCode.Size = new System.Drawing.Size(80, 23);
            this.txtHSCode.TabIndex = 101;
            // 
            // txtNLCode
            // 
            this.txtNLCode.BackColor = System.Drawing.Color.White;
            this.txtNLCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtNLCode.Location = new System.Drawing.Point(120, 68);
            this.txtNLCode.Name = "txtNLCode";
            this.txtNLCode.Size = new System.Drawing.Size(100, 23);
            this.txtNLCode.TabIndex = 102;
            // 
            // labGenerateDate
            // 
            this.labGenerateDate.Location = new System.Drawing.Point(20, 96);
            this.labGenerateDate.Name = "labGenerateDate";
            this.labGenerateDate.Size = new System.Drawing.Size(96, 23);
            this.labGenerateDate.TabIndex = 103;
            this.labGenerateDate.Text = "Generate Date";
            // 
            // dateGenerate
            // 
            this.dateGenerate.Location = new System.Drawing.Point(120, 97);
            this.dateGenerate.Name = "dateGenerate";
            this.dateGenerate.Size = new System.Drawing.Size(130, 23);
            this.dateGenerate.TabIndex = 106;
            this.dateGenerate.Validating += new System.ComponentModel.CancelEventHandler(this.DateGenerate_Validating);
            // 
            // R40
            // 
            this.ClientSize = new System.Drawing.Size(470, 228);
            this.Controls.Add(this.dateGenerate);
            this.Controls.Add(this.labGenerateDate);
            this.Controls.Add(this.txtNLCode);
            this.Controls.Add(this.txtHSCode);
            this.Controls.Add(this.txtContractNo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkLiquidationDataOnly);
            this.Controls.Add(this.labelNLCode);
            this.Controls.Add(this.labelHSCode);
            this.Controls.Add(this.labelContractNo);
            this.IsSupportToPrint = false;
            this.Name = "R40";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R40. Current Contract vs Factory Qty Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelContractNo, 0);
            this.Controls.SetChildIndex(this.labelHSCode, 0);
            this.Controls.SetChildIndex(this.labelNLCode, 0);
            this.Controls.SetChildIndex(this.checkLiquidationDataOnly, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtContractNo, 0);
            this.Controls.SetChildIndex(this.txtHSCode, 0);
            this.Controls.SetChildIndex(this.txtNLCode, 0);
            this.Controls.SetChildIndex(this.labGenerateDate, 0);
            this.Controls.SetChildIndex(this.dateGenerate, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelContractNo;
        private Win.UI.Label labelHSCode;
        private Win.UI.Label labelNLCode;
        private Win.UI.CheckBox checkLiquidationDataOnly;
        private Win.UI.Label label5;
        private Win.UI.TextBox txtContractNo;
        private Win.UI.TextBox txtHSCode;
        private Win.UI.TextBox txtNLCode;
        private Win.UI.Label labGenerateDate;
        private Win.UI.DateBox dateGenerate;
    }
}
