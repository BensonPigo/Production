namespace Sci.Production.Warehouse
{
    partial class P62_Print
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.radioTransferSlip = new Sci.Win.UI.RadioButton();
            this.radioGroup1 = new Ict.Win.UI.RadioGroup();
            this.radioGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(445, 21);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(445, 57);
            this.toexcel.Visible = false;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(445, 93);
            // 
            // radioTransferSlip
            // 
            this.radioTransferSlip.AutoSize = true;
            this.radioTransferSlip.Checked = true;
            this.radioTransferSlip.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioTransferSlip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioTransferSlip.Location = new System.Drawing.Point(17, 30);
            this.radioTransferSlip.Name = "radioTransferSlip";
            this.radioTransferSlip.Size = new System.Drawing.Size(116, 24);
            this.radioTransferSlip.TabIndex = 6;
            this.radioTransferSlip.TabStop = true;
            this.radioTransferSlip.Text = "Transfer Slip";
            this.radioTransferSlip.UseVisualStyleBackColor = true;
            this.radioTransferSlip.Value = "1";
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.radioTransferSlip);
            this.radioGroup1.Location = new System.Drawing.Point(22, 5);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(266, 182);
            this.radioGroup1.TabIndex = 95;
            this.radioGroup1.TabStop = false;
            this.radioGroup1.Value = "1";
            // 
            // P62_Print
            // 
            this.ClientSize = new System.Drawing.Size(537, 223);
            this.Controls.Add(this.radioGroup1);
            this.Name = "P62_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "() () ";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioGroup1, 0);
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.RadioButton radioTransferSlip;
        private Ict.Win.UI.RadioGroup radioGroup1;
    }
}
