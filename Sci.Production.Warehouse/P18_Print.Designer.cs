namespace Sci.Production.Warehouse
{
    partial class P18_Print
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.radioPanel = new Sci.Win.UI.RadioPanel();
            this.comboType = new Sci.Win.UI.ComboBox();
            this.label2 = new Sci.Win.UI.Label();
            this.radioQRCodeSticker = new Sci.Win.UI.RadioButton();
            this.radioP18ExcelImport = new Sci.Win.UI.RadioButton();
            this.radioTransferOutReport = new Sci.Win.UI.RadioButton();
            this.comboPrint = new Sci.Win.UI.ComboBox();
            this.lbPrint = new Sci.Win.UI.Label();
            this.radioPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(437, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(437, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(437, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(380, 123);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(406, 132);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(429, 132);
            // 
            // radioPanel
            // 
            this.radioPanel.Controls.Add(this.comboPrint);
            this.radioPanel.Controls.Add(this.lbPrint);
            this.radioPanel.Controls.Add(this.comboType);
            this.radioPanel.Controls.Add(this.label2);
            this.radioPanel.Controls.Add(this.radioQRCodeSticker);
            this.radioPanel.Controls.Add(this.radioP18ExcelImport);
            this.radioPanel.Controls.Add(this.radioTransferOutReport);
            this.radioPanel.Location = new System.Drawing.Point(12, 12);
            this.radioPanel.Name = "radioPanel";
            this.radioPanel.Size = new System.Drawing.Size(362, 141);
            this.radioPanel.TabIndex = 101;
            this.radioPanel.Value = "1";
            this.radioPanel.ValueChanged += new System.EventHandler(this.RadioPanel_ValueChanged);
            // 
            // comboType
            // 
            this.comboType.BackColor = System.Drawing.Color.White;
            this.comboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType.FormattingEnabled = true;
            this.comboType.IsSupportUnselect = true;
            this.comboType.Location = new System.Drawing.Point(226, 110);
            this.comboType.Name = "comboType";
            this.comboType.OldText = "";
            this.comboType.Size = new System.Drawing.Size(121, 24);
            this.comboType.TabIndex = 105;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(184, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 23);
            this.label2.TabIndex = 104;
            this.label2.Text = "Type:";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // radioQRCodeSticker
            // 
            this.radioQRCodeSticker.AutoSize = true;
            this.radioQRCodeSticker.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioQRCodeSticker.Location = new System.Drawing.Point(21, 85);
            this.radioQRCodeSticker.Name = "radioQRCodeSticker";
            this.radioQRCodeSticker.Size = new System.Drawing.Size(131, 21);
            this.radioQRCodeSticker.TabIndex = 103;
            this.radioQRCodeSticker.TabStop = true;
            this.radioQRCodeSticker.Text = "QR Code Sticker";
            this.radioQRCodeSticker.UseVisualStyleBackColor = true;
            this.radioQRCodeSticker.Value = "3";
            // 
            // radioP18ExcelImport
            // 
            this.radioP18ExcelImport.AutoSize = true;
            this.radioP18ExcelImport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioP18ExcelImport.Location = new System.Drawing.Point(21, 54);
            this.radioP18ExcelImport.Name = "radioP18ExcelImport";
            this.radioP18ExcelImport.Size = new System.Drawing.Size(141, 21);
            this.radioP18ExcelImport.TabIndex = 102;
            this.radioP18ExcelImport.TabStop = true;
            this.radioP18ExcelImport.Text = "Arrive W/H Report";
            this.radioP18ExcelImport.UseVisualStyleBackColor = true;
            this.radioP18ExcelImport.Value = "2";
            // 
            // radioTransferOutReport
            // 
            this.radioTransferOutReport.AutoSize = true;
            this.radioTransferOutReport.Checked = true;
            this.radioTransferOutReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioTransferOutReport.Location = new System.Drawing.Point(21, 23);
            this.radioTransferOutReport.Name = "radioTransferOutReport";
            this.radioTransferOutReport.Size = new System.Drawing.Size(142, 21);
            this.radioTransferOutReport.TabIndex = 101;
            this.radioTransferOutReport.TabStop = true;
            this.radioTransferOutReport.Text = "Transfer In Report";
            this.radioTransferOutReport.UseVisualStyleBackColor = true;
            this.radioTransferOutReport.Value = "1";
            // 
            // comboPrint
            // 
            this.comboPrint.BackColor = System.Drawing.Color.White;
            this.comboPrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboPrint.FormattingEnabled = true;
            this.comboPrint.IsSupportUnselect = true;
            this.comboPrint.Location = new System.Drawing.Point(226, 78);
            this.comboPrint.Name = "comboPrint";
            this.comboPrint.OldText = "";
            this.comboPrint.Size = new System.Drawing.Size(121, 24);
            this.comboPrint.TabIndex = 107;
            this.comboPrint.SelectedIndexChanged += new System.EventHandler(this.ComboPrint_SelectedIndexChanged);
            // 
            // lbPrint
            // 
            this.lbPrint.BackColor = System.Drawing.Color.Transparent;
            this.lbPrint.Location = new System.Drawing.Point(184, 79);
            this.lbPrint.Name = "lbPrint";
            this.lbPrint.Size = new System.Drawing.Size(39, 23);
            this.lbPrint.TabIndex = 106;
            this.lbPrint.Text = "Print:";
            this.lbPrint.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // P18_Print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 180);
            this.Controls.Add(this.radioPanel);
            this.Name = "P18_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "P18_Print";
            this.Controls.SetChildIndex(this.radioPanel, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.radioPanel.ResumeLayout(false);
            this.radioPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel;
        private Win.UI.RadioButton radioP18ExcelImport;
        private Win.UI.RadioButton radioTransferOutReport;
        private Win.UI.ComboBox comboType;
        private Win.UI.Label label2;
        private Win.UI.RadioButton radioQRCodeSticker;
        private Win.UI.ComboBox comboPrint;
        private Win.UI.Label lbPrint;
    }
}