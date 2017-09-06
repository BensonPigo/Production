namespace Sci.Production.Warehouse
{
    partial class P52_Print
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
            this.groupBoxReportType = new System.Windows.Forms.GroupBox();
            this.radioButtonList = new Sci.Win.UI.RadioButton();
            this.radioButtonBookQty = new Sci.Win.UI.RadioButton();
            this.groupBoxReportType.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(277, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(277, -110);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(277, 48);
            // 
            // groupBoxReportType
            // 
            this.groupBoxReportType.Controls.Add(this.radioButtonList);
            this.groupBoxReportType.Controls.Add(this.radioButtonBookQty);
            this.groupBoxReportType.Location = new System.Drawing.Point(12, 12);
            this.groupBoxReportType.Name = "groupBoxReportType";
            this.groupBoxReportType.Size = new System.Drawing.Size(244, 90);
            this.groupBoxReportType.TabIndex = 94;
            this.groupBoxReportType.TabStop = false;
            this.groupBoxReportType.Text = "Report Type";
            // 
            // radioButtonList
            // 
            this.radioButtonList.AutoSize = true;
            this.radioButtonList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioButtonList.Location = new System.Drawing.Point(16, 49);
            this.radioButtonList.Name = "radioButtonList";
            this.radioButtonList.Size = new System.Drawing.Size(125, 21);
            this.radioButtonList.TabIndex = 0;
            this.radioButtonList.Text = "Stocktaking List";
            this.radioButtonList.UseVisualStyleBackColor = true;
            // 
            // radioButtonBookQty
            // 
            this.radioButtonBookQty.AutoSize = true;
            this.radioButtonBookQty.Checked = true;
            this.radioButtonBookQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioButtonBookQty.Location = new System.Drawing.Point(16, 22);
            this.radioButtonBookQty.Name = "radioButtonBookQty";
            this.radioButtonBookQty.Size = new System.Drawing.Size(209, 21);
            this.radioButtonBookQty.TabIndex = 0;
            this.radioButtonBookQty.TabStop = true;
            this.radioButtonBookQty.Text = "Stocktaking without Book Qty";
            this.radioButtonBookQty.UseVisualStyleBackColor = true;
            // 
            // P52_Print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 141);
            this.Controls.Add(this.groupBoxReportType);
            this.Name = "P52_Print";
            this.Text = "P52_Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.groupBoxReportType, 0);
            this.groupBoxReportType.ResumeLayout(false);
            this.groupBoxReportType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxReportType;
        private Win.UI.RadioButton radioButtonList;
        private Win.UI.RadioButton radioButtonBookQty;
    }
}