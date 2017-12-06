namespace Sci.Production.PPIC
{
    partial class P01_TMSAndCost
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
            this.labelTTLTMS = new Sci.Win.UI.Label();
            this.numTTLTMS = new Sci.Win.UI.NumericBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.numTTLTMS);
            this.btmcont.Controls.Add(this.labelTTLTMS);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.labelTTLTMS, 0);
            this.btmcont.Controls.SetChildIndex(this.numTTLTMS, 0);
            // 
            // append
            // 
            this.append.Location = new System.Drawing.Point(170, 5);
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(10, 5);
            // 
            // labelTTLTMS
            // 
            this.labelTTLTMS.Lines = 0;
            this.labelTTLTMS.Location = new System.Drawing.Point(312, 12);
            this.labelTTLTMS.Name = "labelTTLTMS";
            this.labelTTLTMS.Size = new System.Drawing.Size(62, 23);
            this.labelTTLTMS.TabIndex = 95;
            this.labelTTLTMS.Text = "TTL TMS";
            // 
            // numTTLTMS
            // 
            this.numTTLTMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTTLTMS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTTLTMS.IsSupportEditMode = false;
            this.numTTLTMS.Location = new System.Drawing.Point(378, 12);
            this.numTTLTMS.Name = "numTTLTMS";
            this.numTTLTMS.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTTLTMS.ReadOnly = true;
            this.numTTLTMS.Size = new System.Drawing.Size(100, 23);
            this.numTTLTMS.TabIndex = 96;
            this.numTTLTMS.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // P01_TMSAndCost
            // 
            this.ClientSize = new System.Drawing.Size(700, 497);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.GridPopUp = false;
            this.KeyField1 = "ID";
            this.Name = "P01_TMSAndCost";
            this.Text = "TMS & Cost";
            this.WorkAlias = "Order_TmsCost";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.btmcont.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.NumericBox numTTLTMS;
        private Win.UI.Label labelTTLTMS;
    }
}
