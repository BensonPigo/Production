namespace Sci.Production.PPIC
{
    partial class P04_TMSAndCost
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
            this.label1 = new Sci.Win.UI.Label();
            this.numericBox1 = new Sci.Win.UI.NumericBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.numericBox1);
            this.btmcont.Controls.Add(this.label1);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.label1, 0);
            this.btmcont.Controls.SetChildIndex(this.numericBox1, 0);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(312, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 23);
            this.label1.TabIndex = 95;
            this.label1.Text = "TTL TMS";
            // 
            // numericBox1
            // 
            this.numericBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numericBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numericBox1.IsSupportEditMode = false;
            this.numericBox1.Location = new System.Drawing.Point(378, 12);
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.ReadOnly = true;
            this.numericBox1.Size = new System.Drawing.Size(100, 23);
            this.numericBox1.TabIndex = 96;
            // 
            // P04_TMSAndCost
            // 
            this.ClientSize = new System.Drawing.Size(831, 497);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.GridPopUp = false;
            this.KeyField1 = "StyleUkey";
            this.Name = "P04_TMSAndCost";
            this.Text = "TMS & Cost";
            this.WorkAlias = "Style_TmsCost";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.btmcont.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.NumericBox numericBox1;
        private Win.UI.Label label1;
    }
}
