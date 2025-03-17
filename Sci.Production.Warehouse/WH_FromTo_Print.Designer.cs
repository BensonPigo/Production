namespace Sci.Production.Warehouse
{
    partial class WH_FromTo_Print
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
            this.comboType = new Sci.Win.UI.ComboBox();
            this.label2 = new Sci.Win.UI.Label();
            this.radioQRCodeSticker = new Sci.Win.UI.RadioButton();
            this.comboPrint = new Sci.Win.UI.ComboBox();
            this.lbPrint = new Sci.Win.UI.Label();
            this.radioGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(426, 18);
            // 
            // toexcel
            // 
            this.toexcel.Enabled = false;
            this.toexcel.Location = new System.Drawing.Point(426, 54);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(426, 90);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(290, 5);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(406, 131);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(290, 9);
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
            this.radioGroup1.Controls.Add(this.comboPrint);
            this.radioGroup1.Controls.Add(this.lbPrint);
            this.radioGroup1.Controls.Add(this.comboType);
            this.radioGroup1.Controls.Add(this.label2);
            this.radioGroup1.Controls.Add(this.radioQRCodeSticker);
            this.radioGroup1.Controls.Add(this.radioTransferSlip);
            this.radioGroup1.Location = new System.Drawing.Point(22, 5);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(393, 118);
            this.radioGroup1.TabIndex = 95;
            this.radioGroup1.TabStop = false;
            this.radioGroup1.Value = "1";
            this.radioGroup1.ValueChanged += new System.EventHandler(this.RadioGroup1_ValueChanged);
            // 
            // comboType
            // 
            this.comboType.BackColor = System.Drawing.Color.White;
            this.comboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType.FormattingEnabled = true;
            this.comboType.IsSupportUnselect = true;
            this.comboType.Location = new System.Drawing.Point(215, 91);
            this.comboType.Name = "comboType";
            this.comboType.OldText = "";
            this.comboType.Size = new System.Drawing.Size(121, 24);
            this.comboType.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(173, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 23);
            this.label2.TabIndex = 10;
            this.label2.Text = "Type:";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // radioQRCodeSticker
            // 
            this.radioQRCodeSticker.AutoSize = true;
            this.radioQRCodeSticker.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioQRCodeSticker.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioQRCodeSticker.Location = new System.Drawing.Point(17, 60);
            this.radioQRCodeSticker.Name = "radioQRCodeSticker";
            this.radioQRCodeSticker.Size = new System.Drawing.Size(146, 24);
            this.radioQRCodeSticker.TabIndex = 9;
            this.radioQRCodeSticker.Text = "QR Code Sticker";
            this.radioQRCodeSticker.UseVisualStyleBackColor = true;
            this.radioQRCodeSticker.Value = "2";
            // 
            // comboPrint
            // 
            this.comboPrint.BackColor = System.Drawing.Color.White;
            this.comboPrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboPrint.FormattingEnabled = true;
            this.comboPrint.IsSupportUnselect = true;
            this.comboPrint.Location = new System.Drawing.Point(215, 62);
            this.comboPrint.Name = "comboPrint";
            this.comboPrint.OldText = "";
            this.comboPrint.Size = new System.Drawing.Size(121, 24);
            this.comboPrint.TabIndex = 13;
            this.comboPrint.SelectedIndexChanged += new System.EventHandler(this.ComboPrint_SelectedIndexChanged);
            // 
            // lbPrint
            // 
            this.lbPrint.BackColor = System.Drawing.Color.Transparent;
            this.lbPrint.Location = new System.Drawing.Point(173, 63);
            this.lbPrint.Name = "lbPrint";
            this.lbPrint.Size = new System.Drawing.Size(39, 23);
            this.lbPrint.TabIndex = 12;
            this.lbPrint.Text = "Print:";
            this.lbPrint.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // WH_FromTo_Print
            // 
            this.ClientSize = new System.Drawing.Size(518, 176);
            this.Controls.Add(this.radioGroup1);
            this.Name = "WH_FromTo_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "() () ";
            this.Controls.SetChildIndex(this.radioGroup1, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.RadioButton radioTransferSlip;
        private Ict.Win.UI.RadioGroup radioGroup1;
        private Win.UI.RadioButton radioQRCodeSticker;
        private Win.UI.ComboBox comboType;
        private Win.UI.Label label2;
        private Win.UI.ComboBox comboPrint;
        private Win.UI.Label lbPrint;
    }
}
