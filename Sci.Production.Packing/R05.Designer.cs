namespace Sci.Production.Packing
{
    partial class R05
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
            this.txtMachineNo = new Sci.Win.UI.TextBox();
            this.labMachineNo = new Sci.Win.UI.Label();
            this.dateCalibrationDate = new Sci.Win.UI.DateRange();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(466, 84);
            this.print.TabIndex = 2;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(466, 12);
            this.toexcel.TabIndex = 3;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(466, 48);
            this.close.TabIndex = 4;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(420, 151);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(208, 158);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(314, 158);
            // 
            // txtMachineNo
            // 
            this.txtMachineNo.BackColor = System.Drawing.Color.White;
            this.txtMachineNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMachineNo.Location = new System.Drawing.Point(136, 19);
            this.txtMachineNo.Name = "txtMachineNo";
            this.txtMachineNo.Size = new System.Drawing.Size(130, 23);
            this.txtMachineNo.TabIndex = 0;
            this.txtMachineNo.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtMachineNo_PopUp);
            this.txtMachineNo.Validating += new System.ComponentModel.CancelEventHandler(this.txtMachineNo_Validating);
            // 
            // labMachineNo
            // 
            this.labMachineNo.Location = new System.Drawing.Point(22, 19);
            this.labMachineNo.Name = "labMachineNo";
            this.labMachineNo.Size = new System.Drawing.Size(111, 23);
            this.labMachineNo.TabIndex = 5;
            this.labMachineNo.Text = "Machine#";
            // 
            // dateCalibrationDate
            // 
            // 
            // 
            // 
            this.dateCalibrationDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCalibrationDate.DateBox1.Name = "";
            this.dateCalibrationDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCalibrationDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCalibrationDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCalibrationDate.DateBox2.Name = "";
            this.dateCalibrationDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCalibrationDate.DateBox2.TabIndex = 1;
            this.dateCalibrationDate.Location = new System.Drawing.Point(136, 55);
            this.dateCalibrationDate.Name = "dateCalibrationDate";
            this.dateCalibrationDate.Size = new System.Drawing.Size(280, 23);
            this.dateCalibrationDate.TabIndex = 1;
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(22, 55);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(111, 23);
            this.labelBuyerDelivery.TabIndex = 6;
            this.labelBuyerDelivery.Text = "Calibration Date";
            // 
            // R05
            // 
            this.ClientSize = new System.Drawing.Size(558, 221);
            this.Controls.Add(this.dateCalibrationDate);
            this.Controls.Add(this.labelBuyerDelivery);
            this.Controls.Add(this.txtMachineNo);
            this.Controls.Add(this.labMachineNo);
            this.DefaultControl = "txtMachineNo";
            this.Name = "R05";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R05. Calibration List Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labMachineNo, 0);
            this.Controls.SetChildIndex(this.txtMachineNo, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.dateCalibrationDate, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox txtMachineNo;
        private Win.UI.Label labMachineNo;
        private Win.UI.DateRange dateCalibrationDate;
        private Win.UI.Label labelBuyerDelivery;
    }
}
