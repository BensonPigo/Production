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
            this.radioP18ExcelImport = new Sci.Win.UI.RadioButton();
            this.radioTransferOutReport = new Sci.Win.UI.RadioButton();
            this.radioPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(322, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(322, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(322, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(167, 82);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(193, 91);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(216, 91);
            // 
            // radioPanel
            // 
            this.radioPanel.Controls.Add(this.radioP18ExcelImport);
            this.radioPanel.Controls.Add(this.radioTransferOutReport);
            this.radioPanel.Location = new System.Drawing.Point(12, 12);
            this.radioPanel.Name = "radioPanel";
            this.radioPanel.Size = new System.Drawing.Size(200, 100);
            this.radioPanel.TabIndex = 101;
            this.radioPanel.Value = "1";
            this.radioPanel.ValueChanged += new System.EventHandler(this.RadioPanel_ValueChanged);
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
            // P18_Print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 149);
            this.Controls.Add(this.radioPanel);
            this.Name = "P18_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "P18_Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.radioPanel, 0);
            this.radioPanel.ResumeLayout(false);
            this.radioPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel;
        private Win.UI.RadioButton radioP18ExcelImport;
        private Win.UI.RadioButton radioTransferOutReport;
    }
}