namespace Sci.Production.Subcon
{
    partial class P30_Print
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioByRefno = new Sci.Win.UI.RadioButton();
            this.radioCoatsOrderFormat = new Sci.Win.UI.RadioButton();
            this.radioNmrmalFormat = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(290, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(290, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(290, 84);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioByRefno);
            this.radioPanel1.Controls.Add(this.radioCoatsOrderFormat);
            this.radioPanel1.Controls.Add(this.radioNmrmalFormat);
            this.radioPanel1.Location = new System.Drawing.Point(12, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(270, 122);
            this.radioPanel1.TabIndex = 95;
            this.radioPanel1.Value = "Material Status";
            this.radioPanel1.ValueChanged += new System.EventHandler(this.RadioPanel1_ValueChanged);
            // 
            // radioByRefno
            // 
            this.radioByRefno.AutoSize = true;
            this.radioByRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByRefno.Location = new System.Drawing.Point(26, 51);
            this.radioByRefno.Name = "radioByRefno";
            this.radioByRefno.Size = new System.Drawing.Size(84, 21);
            this.radioByRefno.TabIndex = 2;
            this.radioByRefno.Text = "By Refno";
            this.radioByRefno.UseVisualStyleBackColor = true;
            this.radioByRefno.Value = "Material Status";
            // 
            // radioCoatsOrderFormat
            // 
            this.radioCoatsOrderFormat.AutoSize = true;
            this.radioCoatsOrderFormat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioCoatsOrderFormat.Location = new System.Drawing.Point(26, 81);
            this.radioCoatsOrderFormat.Name = "radioCoatsOrderFormat";
            this.radioCoatsOrderFormat.Size = new System.Drawing.Size(144, 21);
            this.radioCoatsOrderFormat.TabIndex = 1;
            this.radioCoatsOrderFormat.Text = "Coats order format";
            this.radioCoatsOrderFormat.UseVisualStyleBackColor = true;
            this.radioCoatsOrderFormat.Value = "Purchase List";
            // 
            // radioNmrmalFormat
            // 
            this.radioNmrmalFormat.AutoSize = true;
            this.radioNmrmalFormat.Checked = true;
            this.radioNmrmalFormat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioNmrmalFormat.Location = new System.Drawing.Point(26, 24);
            this.radioNmrmalFormat.Name = "radioNmrmalFormat";
            this.radioNmrmalFormat.Size = new System.Drawing.Size(115, 21);
            this.radioNmrmalFormat.TabIndex = 0;
            this.radioNmrmalFormat.TabStop = true;
            this.radioNmrmalFormat.Text = "Normal format";
            this.radioNmrmalFormat.UseVisualStyleBackColor = true;
            this.radioNmrmalFormat.Value = "Material Status";
            // 
            // P30_Print
            // 
            this.ClientSize = new System.Drawing.Size(382, 172);
            this.Controls.Add(this.radioPanel1);
            this.Name = "P30_Print";
            this.Text = "P30 print";
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
        private Win.UI.RadioButton radioCoatsOrderFormat;
        private Win.UI.RadioButton radioNmrmalFormat;
        private Win.UI.RadioButton radioByRefno;
    }
}
