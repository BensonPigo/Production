namespace Sci.Production.Warehouse
{
    partial class WH_Print
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
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.comboType = new Sci.Win.UI.ComboBox();
            this.label2 = new Sci.Win.UI.Label();
            this.radioQRCodeSticker = new Sci.Win.UI.RadioButton();
            this.radioPrint = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(461, 25);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(461, 61);
            this.toexcel.Visible = false;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(461, 97);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.comboType);
            this.radioPanel1.Controls.Add(this.label2);
            this.radioPanel1.Controls.Add(this.radioQRCodeSticker);
            this.radioPanel1.Controls.Add(this.radioPrint);
            this.radioPanel1.Location = new System.Drawing.Point(44, 25);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(394, 102);
            this.radioPanel1.TabIndex = 94;
            this.radioPanel1.Value = "P/L Rcv Report";
            // 
            // comboType
            // 
            this.comboType.BackColor = System.Drawing.Color.White;
            this.comboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType.FormattingEnabled = true;
            this.comboType.IsSupportUnselect = true;
            this.comboType.Location = new System.Drawing.Point(218, 56);
            this.comboType.Name = "comboType";
            this.comboType.OldText = "";
            this.comboType.Size = new System.Drawing.Size(121, 24);
            this.comboType.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(176, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "Type:";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // radioQRCodeSticker
            // 
            this.radioQRCodeSticker.AutoSize = true;
            this.radioQRCodeSticker.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioQRCodeSticker.Location = new System.Drawing.Point(32, 59);
            this.radioQRCodeSticker.Name = "radioQRCodeSticker";
            this.radioQRCodeSticker.Size = new System.Drawing.Size(131, 21);
            this.radioQRCodeSticker.TabIndex = 4;
            this.radioQRCodeSticker.TabStop = true;
            this.radioQRCodeSticker.Text = "QR Code Sticker";
            this.radioQRCodeSticker.UseVisualStyleBackColor = true;
            this.radioQRCodeSticker.Value = "QR Code Sticker";
            // 
            // radioPrint
            // 
            this.radioPrint.AutoSize = true;
            this.radioPrint.Checked = true;
            this.radioPrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioPrint.Location = new System.Drawing.Point(32, 24);
            this.radioPrint.Name = "radioPrint";
            this.radioPrint.Size = new System.Drawing.Size(55, 21);
            this.radioPrint.TabIndex = 0;
            this.radioPrint.TabStop = true;
            this.radioPrint.Text = "Print";
            this.radioPrint.UseVisualStyleBackColor = true;
            this.radioPrint.Value = "P/L Rcv Report";
            this.radioPrint.CheckedChanged += new System.EventHandler(this.RadioPrint_CheckedChanged);
            // 
            // WH_Print
            // 
            this.ClientSize = new System.Drawing.Size(553, 178);
            this.Controls.Add(this.radioPanel1);
            this.Name = "WH_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "() () ";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioPrint;
        private Win.UI.ComboBox comboType;
        private Win.UI.Label label2;
        private Win.UI.RadioButton radioQRCodeSticker;
    }
}
