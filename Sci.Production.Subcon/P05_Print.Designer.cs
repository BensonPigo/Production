namespace Sci.Production.Subcon
{
    partial class P05_Print
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
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioByPO = new Sci.Win.UI.RadioButton();
            this.radioByComb = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(281, 12);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 31);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(281, 49);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 31);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // radioPanel1
            // 
            this.radioPanel1.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.radioPanel1.Controls.Add(this.radioByPO);
            this.radioPanel1.Controls.Add(this.radioByComb);
            this.radioPanel1.Location = new System.Drawing.Point(12, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(263, 65);
            this.radioPanel1.TabIndex = 95;
            this.radioPanel1.Title = "Print Style :";
            this.radioPanel1.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            // 
            // radioByPO
            // 
            this.radioByPO.AutoSize = true;
            this.radioByPO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByPO.Location = new System.Drawing.Point(121, 29);
            this.radioByPO.Name = "radioByPO";
            this.radioByPO.Size = new System.Drawing.Size(74, 21);
            this.radioByPO.TabIndex = 1;
            this.radioByPO.TabStop = true;
            this.radioByPO.Text = "By P.O.";
            this.radioByPO.UseVisualStyleBackColor = true;
            // 
            // radioByComb
            // 
            this.radioByComb.AutoSize = true;
            this.radioByComb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByComb.Location = new System.Drawing.Point(121, 2);
            this.radioByComb.Name = "radioByComb";
            this.radioByComb.Size = new System.Drawing.Size(124, 21);
            this.radioByComb.TabIndex = 0;
            this.radioByComb.TabStop = true;
            this.radioByComb.Text = "By Combination";
            this.radioByComb.UseVisualStyleBackColor = true;
            // 
            // P05_Print
            // 
            this.ClientSize = new System.Drawing.Size(368, 87);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Name = "P05_Print";
            this.Text = "P05_Print";
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnClose;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioByPO;
        private Win.UI.RadioButton radioByComb;
    }
}
