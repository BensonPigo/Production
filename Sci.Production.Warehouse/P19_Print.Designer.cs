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
            this.comboPrint = new Sci.Win.UI.ComboBox();
            this.lbPrint = new Sci.Win.UI.Label();
            this.comboSortBy = new Sci.Win.UI.ComboBox();
            this.lbSortBy = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(356, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(356, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(356, 84);
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
            this.comboType.Location = new System.Drawing.Point(215, 97);
            this.comboType.Name = "comboType";
            this.comboType.OldText = "";
            this.comboType.Size = new System.Drawing.Size(121, 24);
            this.comboType.TabIndex = 101;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(173, 98);
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
            // comboPrint
            // 
            this.comboPrint.BackColor = System.Drawing.Color.White;
            this.comboPrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboPrint.FormattingEnabled = true;
            this.comboPrint.IsSupportUnselect = true;
            this.comboPrint.Location = new System.Drawing.Point(215, 70);
            this.comboPrint.Name = "comboPrint";
            this.comboPrint.OldText = "";
            this.comboPrint.Size = new System.Drawing.Size(121, 24);
            this.comboPrint.TabIndex = 103;
            this.comboPrint.SelectedIndexChanged += new System.EventHandler(this.ComboPrint_SelectedIndexChanged);
            // 
            // lbPrint
            // 
            this.lbPrint.BackColor = System.Drawing.Color.Transparent;
            this.lbPrint.Location = new System.Drawing.Point(173, 71);
            this.lbPrint.Name = "lbPrint";
            this.lbPrint.Size = new System.Drawing.Size(39, 23);
            this.lbPrint.TabIndex = 102;
            this.lbPrint.Text = "Print:";
            this.lbPrint.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // comboSortBy
            // 
            this.comboSortBy.BackColor = System.Drawing.Color.White;
            this.comboSortBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSortBy.FormattingEnabled = true;
            this.comboSortBy.IsSupportUnselect = true;
            this.comboSortBy.Location = new System.Drawing.Point(230, 11);
            this.comboSortBy.Name = "comboSortBy";
            this.comboSortBy.OldText = "";
            this.comboSortBy.Size = new System.Drawing.Size(121, 24);
            this.comboSortBy.TabIndex = 105;
            // 
            // lbSortBy
            // 
            this.lbSortBy.BackColor = System.Drawing.Color.Transparent;
            this.lbSortBy.Location = new System.Drawing.Point(173, 12);
            this.lbSortBy.Name = "lbSortBy";
            this.lbSortBy.Size = new System.Drawing.Size(54, 23);
            this.lbSortBy.TabIndex = 104;
            this.lbSortBy.Text = "Sort By:";
            this.lbSortBy.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // P19_Print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 155);
            this.Controls.Add(this.comboSortBy);
            this.Controls.Add(this.lbSortBy);
            this.Controls.Add(this.comboPrint);
            this.Controls.Add(this.lbPrint);
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
            this.Controls.SetChildIndex(this.lbPrint, 0);
            this.Controls.SetChildIndex(this.comboPrint, 0);
            this.Controls.SetChildIndex(this.lbSortBy, 0);
            this.Controls.SetChildIndex(this.comboSortBy, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioButton radioTransferOutReport;
        private Win.UI.RadioButton radioP18ExcelImport;
        private Win.UI.ComboBox comboType;
        private Win.UI.Label label2;
        private Win.UI.RadioButton radioQRCodeSticker;
        private Win.UI.ComboBox comboPrint;
        private Win.UI.Label lbPrint;
        private Win.UI.ComboBox comboSortBy;
        private Win.UI.Label lbSortBy;
    }
}