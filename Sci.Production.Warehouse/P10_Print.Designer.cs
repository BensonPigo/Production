namespace Sci.Production.Warehouse
{
    partial class P10_Print
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
            this.radioFabricSticker = new Sci.Win.UI.RadioButton();
            this.radioTransferSlip = new Sci.Win.UI.RadioButton();
            this.radioRelaxationSticker = new Sci.Win.UI.RadioButton();
            this.radioFabricsRelaxationLogsheet = new Sci.Win.UI.RadioButton();
            this.radioGroup1 = new Ict.Win.UI.RadioGroup();
            this.radioGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(356, 21);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(356, 57);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(356, 93);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(310, 125);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(336, 134);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(336, 134);
            // 
            // radioFabricSticker
            // 
            this.radioFabricSticker.AutoSize = true;
            this.radioFabricSticker.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioFabricSticker.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioFabricSticker.Location = new System.Drawing.Point(17, 60);
            this.radioFabricSticker.Name = "radioFabricSticker";
            this.radioFabricSticker.Size = new System.Drawing.Size(124, 24);
            this.radioFabricSticker.TabIndex = 8;
            this.radioFabricSticker.Text = "Fabric Sticker";
            this.radioFabricSticker.UseVisualStyleBackColor = true;
            this.radioFabricSticker.Value = "2";
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
            // radioRelaxationSticker
            // 
            this.radioRelaxationSticker.AutoSize = true;
            this.radioRelaxationSticker.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioRelaxationSticker.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioRelaxationSticker.Location = new System.Drawing.Point(17, 94);
            this.radioRelaxationSticker.Name = "radioRelaxationSticker";
            this.radioRelaxationSticker.Size = new System.Drawing.Size(155, 24);
            this.radioRelaxationSticker.TabIndex = 1;
            this.radioRelaxationSticker.Text = "Relaxation Sticker";
            this.radioRelaxationSticker.UseVisualStyleBackColor = true;
            this.radioRelaxationSticker.Value = "4";
            // 
            // radioFabricsRelaxationLogsheet
            // 
            this.radioFabricsRelaxationLogsheet.AutoSize = true;
            this.radioFabricsRelaxationLogsheet.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioFabricsRelaxationLogsheet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioFabricsRelaxationLogsheet.Location = new System.Drawing.Point(17, 124);
            this.radioFabricsRelaxationLogsheet.Name = "radioFabricsRelaxationLogsheet";
            this.radioFabricsRelaxationLogsheet.Size = new System.Drawing.Size(229, 24);
            this.radioFabricsRelaxationLogsheet.TabIndex = 2;
            this.radioFabricsRelaxationLogsheet.Text = "Fabrics Relaxation Logsheet";
            this.radioFabricsRelaxationLogsheet.UseVisualStyleBackColor = true;
            this.radioFabricsRelaxationLogsheet.Value = "5";
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.radioFabricSticker);
            this.radioGroup1.Controls.Add(this.radioRelaxationSticker);
            this.radioGroup1.Controls.Add(this.radioTransferSlip);
            this.radioGroup1.Controls.Add(this.radioFabricsRelaxationLogsheet);
            this.radioGroup1.Location = new System.Drawing.Point(22, 5);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(266, 182);
            this.radioGroup1.TabIndex = 95;
            this.radioGroup1.TabStop = false;
            this.radioGroup1.Value = "1";
            this.radioGroup1.ValueChanged += new System.EventHandler(this.RadioGroup1_ValueChanged);
            // 
            // P10_Print
            // 
            this.ClientSize = new System.Drawing.Size(448, 224);
            this.Controls.Add(this.radioGroup1);
            this.Name = "P10_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
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
        private Win.UI.RadioButton radioFabricSticker;
        private Win.UI.RadioButton radioTransferSlip;
        private Win.UI.RadioButton radioRelaxationSticker;
        private Win.UI.RadioButton radioFabricsRelaxationLogsheet;
        private Ict.Win.UI.RadioGroup radioGroup1;
    }
}
