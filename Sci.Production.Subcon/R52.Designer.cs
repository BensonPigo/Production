namespace Sci.Production.Subcon
{
    partial class R52
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
         this.txtseason = new Sci.Production.Class.txtseason();
         this.labelSeason = new Sci.Win.UI.Label();
         this.SuspendLayout();
         // 
         // print
         // 
         this.print.Location = new System.Drawing.Point(319, 18);
         this.print.Visible = false;
         // 
         // toexcel
         // 
         this.toexcel.Location = new System.Drawing.Point(319, 18);
         // 
         // close
         // 
         this.close.Location = new System.Drawing.Point(319, 54);
         // 
         // txtseason
         // 
         this.txtseason.BackColor = System.Drawing.Color.White;
         this.txtseason.BrandObjectName = null;
         this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
         this.txtseason.Location = new System.Drawing.Point(98, 25);
         this.txtseason.Name = "txtseason";
         this.txtseason.Size = new System.Drawing.Size(80, 23);
         this.txtseason.TabIndex = 98;
         // 
         // labelSeason
         // 
         this.labelSeason.Location = new System.Drawing.Point(19, 25);
         this.labelSeason.Name = "labelSeason";
         this.labelSeason.Size = new System.Drawing.Size(67, 23);
         this.labelSeason.TabIndex = 99;
         this.labelSeason.Text = "Season";
         // 
         // R52
         // 
         this.ClientSize = new System.Drawing.Size(411, 137);
         this.Controls.Add(this.txtseason);
         this.Controls.Add(this.labelSeason);
         this.Name = "R52";
         this.Text = "R52.Transfer Subcon Style Data To Printing System";
         this.Controls.SetChildIndex(this.print, 0);
         this.Controls.SetChildIndex(this.toexcel, 0);
         this.Controls.SetChildIndex(this.close, 0);
         this.Controls.SetChildIndex(this.labelSeason, 0);
         this.Controls.SetChildIndex(this.txtseason, 0);
         this.ResumeLayout(false);
         this.PerformLayout();

        }

      #endregion

      private Class.txtseason txtseason;
      private Win.UI.Label labelSeason;
   }
}
