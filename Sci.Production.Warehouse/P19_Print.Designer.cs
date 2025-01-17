namespace Sci.Production.Warehouse
{
    partial class P19_Print
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
            this.radioTransferOutReport = new Sci.Win.UI.RadioButton();
            this.radioP18ExcelImport = new Sci.Win.UI.RadioButton();
            this.comboType = new Sci.Win.UI.ComboBox();
            this.label2 = new Sci.Win.UI.Label();
            this.radioQRCodeSticker = new Sci.Win.UI.RadioButton();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(348, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(348, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(348, 84);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(307, -1);
            // 
            // radioTransferOutReport
            // 
            this.radioTransferOutReport.AutoSize = true;
            this.radioTransferOutReport.Checked = true;
            this.radioTransferOutReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioTransferOutReport.Location = new System.Drawing.Point(12, 12);
            this.radioTransferOutReport.Name = "radioTransferOutReport";
            this.radioTransferOutReport.Size = new System.Drawing.Size(154, 21);
            this.radioTransferOutReport.TabIndex = 97;
            this.radioTransferOutReport.TabStop = true;
            this.radioTransferOutReport.Text = "Transfer Out Report";
            this.radioTransferOutReport.UseVisualStyleBackColor = true;
            this.radioTransferOutReport.CheckedChanged += new System.EventHandler(this.RadioTransferOutReport_CheckedChanged);
            // 
            // radioP18ExcelImport
            // 
            this.radioP18ExcelImport.AutoSize = true;
            this.radioP18ExcelImport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioP18ExcelImport.Location = new System.Drawing.Point(12, 43);
            this.radioP18ExcelImport.Name = "radioP18ExcelImport";
            this.radioP18ExcelImport.Size = new System.Drawing.Size(310, 21);
            this.radioP18ExcelImport.TabIndex = 98;
            this.radioP18ExcelImport.TabStop = true;
            this.radioP18ExcelImport.Text = "For W/H P18 Transfer In Import format(Excel)";
            this.radioP18ExcelImport.UseVisualStyleBackColor = true;
            this.radioP18ExcelImport.CheckedChanged += new System.EventHandler(this.RadioP18ExcelImport_CheckedChanged);
            // 
            // comboType
            // 
            this.comboType.BackColor = System.Drawing.Color.White;
            this.comboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType.FormattingEnabled = true;
            this.comboType.IsSupportUnselect = true;
            this.comboType.Location = new System.Drawing.Point(209, 70);
            this.comboType.Name = "comboType";
            this.comboType.OldText = "";
            this.comboType.Size = new System.Drawing.Size(121, 24);
            this.comboType.TabIndex = 101;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(167, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 23);
            this.label2.TabIndex = 100;
            this.label2.Text = "Type:";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // radioQRCodeSticker
            // 
            this.radioQRCodeSticker.AutoSize = true;
            this.radioQRCodeSticker.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioQRCodeSticker.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioQRCodeSticker.Location = new System.Drawing.Point(12, 70);
            this.radioQRCodeSticker.Name = "radioQRCodeSticker";
            this.radioQRCodeSticker.Size = new System.Drawing.Size(146, 24);
            this.radioQRCodeSticker.TabIndex = 99;
            this.radioQRCodeSticker.Text = "QR Code Sticker";
            this.radioQRCodeSticker.UseVisualStyleBackColor = true;
            this.radioQRCodeSticker.Value = "2";
            // 
            // P19_Print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 141);
            this.Controls.Add(this.comboType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.radioQRCodeSticker);
            this.Controls.Add(this.radioP18ExcelImport);
            this.Controls.Add(this.radioTransferOutReport);
            this.Name = "P19_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "P19. Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.radioTransferOutReport, 0);
            this.Controls.SetChildIndex(this.radioP18ExcelImport, 0);
            this.Controls.SetChildIndex(this.radioQRCodeSticker, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.comboType, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioButton radioTransferOutReport;
        private Win.UI.RadioButton radioP18ExcelImport;
        private Win.UI.ComboBox comboType;
        private Win.UI.Label label2;
        private Win.UI.RadioButton radioQRCodeSticker;
    }
}