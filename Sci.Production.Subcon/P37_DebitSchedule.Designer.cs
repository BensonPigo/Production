namespace Sci.Production.Subcon
{
    partial class P37_DebitSchedule
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
            this.numericBoxTotal = new Sci.Win.UI.NumericBox();
            this.label1 = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.label1);
            this.btmcont.Controls.Add(this.numericBoxTotal);
            this.btmcont.Location = new System.Drawing.Point(0, 460);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.numericBoxTotal, 0);
            this.btmcont.Controls.SetChildIndex(this.label1, 0);
            // 
            // gridcont
            // 
            this.gridcont.Size = new System.Drawing.Size(807, 438);
            // 
            // numericBoxTotal
            // 
            this.numericBoxTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numericBoxTotal.DecimalPlaces = 2;
            this.numericBoxTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numericBoxTotal.IsSupportEditMode = false;
            this.numericBoxTotal.Location = new System.Drawing.Point(345, 9);
            this.numericBoxTotal.Name = "numericBoxTotal";
            this.numericBoxTotal.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBoxTotal.ReadOnly = true;
            this.numericBoxTotal.Size = new System.Drawing.Size(100, 23);
            this.numericBoxTotal.TabIndex = 95;
            this.numericBoxTotal.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(254, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 23);
            this.label1.TabIndex = 96;
            this.label1.Text = "TTL Amount";
            // 
            // P37_DebitSchedule
            // 
            this.ClientSize = new System.Drawing.Size(831, 500);
            this.Name = "P37_DebitSchedule";
            this.OnLineHelpID = "Sci.Win.Subs.Input4";
            this.Text = "Debit Schedule";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.btmcont.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.NumericBox numericBoxTotal;
    }
}
