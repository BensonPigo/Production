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
            this.comboPrint = new Sci.Win.UI.ComboBox();
            this.lbPrint = new Sci.Win.UI.Label();
            this.comboType = new Sci.Win.UI.ComboBox();
            this.label2 = new Sci.Win.UI.Label();
            this.radioQRCodeSticker = new Sci.Win.UI.RadioButton();
            this.comboSortBy = new Sci.Win.UI.ComboBox();
            this.lbSortBy = new Sci.Win.UI.Label();
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
            this.radioFabricSticker.Location = new System.Drawing.Point(17, 74);
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
            this.radioTransferSlip.Location = new System.Drawing.Point(17, 16);
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
            this.radioRelaxationSticker.Location = new System.Drawing.Point(17, 108);
            this.radioRelaxationSticker.Name = "radioRelaxationSticker";
            this.radioRelaxationSticker.Size = new System.Drawing.Size(155, 24);
            this.radioRelaxationSticker.TabIndex = 1;
            this.radioRelaxationSticker.Text = "Relaxation Sticker";
            this.radioRelaxationSticker.UseVisualStyleBackColor = true;
            this.radioRelaxationSticker.Value = "3";
            // 
            // radioFabricsRelaxationLogsheet
            // 
            this.radioFabricsRelaxationLogsheet.AutoSize = true;
            this.radioFabricsRelaxationLogsheet.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioFabricsRelaxationLogsheet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioFabricsRelaxationLogsheet.Location = new System.Drawing.Point(17, 138);
            this.radioFabricsRelaxationLogsheet.Name = "radioFabricsRelaxationLogsheet";
            this.radioFabricsRelaxationLogsheet.Size = new System.Drawing.Size(229, 24);
            this.radioFabricsRelaxationLogsheet.TabIndex = 2;
            this.radioFabricsRelaxationLogsheet.Text = "Fabrics Relaxation Logsheet";
            this.radioFabricsRelaxationLogsheet.UseVisualStyleBackColor = true;
            this.radioFabricsRelaxationLogsheet.Value = "4";
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.comboSortBy);
            this.radioGroup1.Controls.Add(this.lbSortBy);
            this.radioGroup1.Controls.Add(this.comboPrint);
            this.radioGroup1.Controls.Add(this.lbPrint);
            this.radioGroup1.Controls.Add(this.comboType);
            this.radioGroup1.Controls.Add(this.label2);
            this.radioGroup1.Controls.Add(this.radioQRCodeSticker);
            this.radioGroup1.Controls.Add(this.radioFabricSticker);
            this.radioGroup1.Controls.Add(this.radioRelaxationSticker);
            this.radioGroup1.Controls.Add(this.radioTransferSlip);
            this.radioGroup1.Controls.Add(this.radioFabricsRelaxationLogsheet);
            this.radioGroup1.Location = new System.Drawing.Point(22, 5);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(266, 263);
            this.radioGroup1.TabIndex = 95;
            this.radioGroup1.TabStop = false;
            this.radioGroup1.Value = "1";
            this.radioGroup1.ValueChanged += new System.EventHandler(this.RadioGroup1_ValueChanged);
            // 
            // comboPrint
            // 
            this.comboPrint.BackColor = System.Drawing.Color.White;
            this.comboPrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboPrint.FormattingEnabled = true;
            this.comboPrint.IsSupportUnselect = true;
            this.comboPrint.Location = new System.Drawing.Point(88, 199);
            this.comboPrint.Name = "comboPrint";
            this.comboPrint.OldText = "";
            this.comboPrint.Size = new System.Drawing.Size(121, 24);
            this.comboPrint.TabIndex = 13;
            this.comboPrint.SelectedIndexChanged += new System.EventHandler(this.ComboPrint_SelectedIndexChanged);
            // 
            // lbPrint
            // 
            this.lbPrint.BackColor = System.Drawing.Color.Transparent;
            this.lbPrint.Location = new System.Drawing.Point(46, 200);
            this.lbPrint.Name = "lbPrint";
            this.lbPrint.Size = new System.Drawing.Size(39, 23);
            this.lbPrint.TabIndex = 12;
            this.lbPrint.Text = "Print:";
            this.lbPrint.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // comboType
            // 
            this.comboType.BackColor = System.Drawing.Color.White;
            this.comboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType.FormattingEnabled = true;
            this.comboType.IsSupportUnselect = true;
            this.comboType.Location = new System.Drawing.Point(88, 232);
            this.comboType.Name = "comboType";
            this.comboType.OldText = "";
            this.comboType.Size = new System.Drawing.Size(121, 24);
            this.comboType.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(46, 233);
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
            this.radioQRCodeSticker.Location = new System.Drawing.Point(17, 166);
            this.radioQRCodeSticker.Name = "radioQRCodeSticker";
            this.radioQRCodeSticker.Size = new System.Drawing.Size(189, 24);
            this.radioQRCodeSticker.TabIndex = 9;
            this.radioQRCodeSticker.Text = "Issue QR Code Sticker";
            this.radioQRCodeSticker.UseVisualStyleBackColor = true;
            this.radioQRCodeSticker.Value = "5";
            // 
            // comboSortBy
            // 
            this.comboSortBy.BackColor = System.Drawing.Color.White;
            this.comboSortBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSortBy.FormattingEnabled = true;
            this.comboSortBy.IsSupportUnselect = true;
            this.comboSortBy.Location = new System.Drawing.Point(103, 44);
            this.comboSortBy.Name = "comboSortBy";
            this.comboSortBy.OldText = "";
            this.comboSortBy.Size = new System.Drawing.Size(121, 24);
            this.comboSortBy.TabIndex = 15;
            // 
            // lbSortBy
            // 
            this.lbSortBy.BackColor = System.Drawing.Color.Transparent;
            this.lbSortBy.Location = new System.Drawing.Point(46, 45);
            this.lbSortBy.Name = "lbSortBy";
            this.lbSortBy.Size = new System.Drawing.Size(54, 23);
            this.lbSortBy.TabIndex = 14;
            this.lbSortBy.Text = "Sort By:";
            this.lbSortBy.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // P10_Print
            // 
            this.ClientSize = new System.Drawing.Size(448, 293);
            this.Controls.Add(this.radioGroup1);
            this.Name = "P10_Print";
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
        private Win.UI.RadioButton radioFabricSticker;
        private Win.UI.RadioButton radioTransferSlip;
        private Win.UI.RadioButton radioRelaxationSticker;
        private Win.UI.RadioButton radioFabricsRelaxationLogsheet;
        private Ict.Win.UI.RadioGroup radioGroup1;
        private Win.UI.RadioButton radioQRCodeSticker;
        private Win.UI.ComboBox comboType;
        private Win.UI.Label label2;
        private Win.UI.ComboBox comboPrint;
        private Win.UI.Label lbPrint;
        private Win.UI.ComboBox comboSortBy;
        private Win.UI.Label lbSortBy;
    }
}
