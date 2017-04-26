namespace Sci.Production.IE
{
    partial class P01_History_Print
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
            this.numEfficiencySetting = new Sci.Win.UI.NumericBox();
            this.labelEfficiencySetting = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(253, 12);
            this.print.TabIndex = 1;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(253, 48);
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(253, 84);
            this.close.TabIndex = 3;
            // 
            // numEfficiencySetting
            // 
            this.numEfficiencySetting.BackColor = System.Drawing.Color.White;
            this.numEfficiencySetting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numEfficiencySetting.Location = new System.Drawing.Point(148, 48);
            this.numEfficiencySetting.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numEfficiencySetting.MaxLength = 3;
            this.numEfficiencySetting.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numEfficiencySetting.Name = "numEfficiencySetting";
            this.numEfficiencySetting.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numEfficiencySetting.Size = new System.Drawing.Size(50, 23);
            this.numEfficiencySetting.TabIndex = 0;
            this.numEfficiencySetting.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelEfficiencySetting
            // 
            this.labelEfficiencySetting.Lines = 0;
            this.labelEfficiencySetting.Location = new System.Drawing.Point(37, 48);
            this.labelEfficiencySetting.Name = "labelEfficiencySetting";
            this.labelEfficiencySetting.Size = new System.Drawing.Size(107, 23);
            this.labelEfficiencySetting.TabIndex = 99;
            this.labelEfficiencySetting.Text = "Efficiency setting";
            // 
            // P01_History_Print
            // 
            this.ClientSize = new System.Drawing.Size(345, 156);
            this.Controls.Add(this.numEfficiencySetting);
            this.Controls.Add(this.labelEfficiencySetting);
            this.DefaultControl = "numericBox1";
            this.DefaultControlForEdit = "numericBox1";
            this.IsSupportPrint = false;
            this.IsSupportToPrint = false;
            this.Name = "P01_History_Print";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelEfficiencySetting, 0);
            this.Controls.SetChildIndex(this.numEfficiencySetting, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.NumericBox numEfficiencySetting;
        private Win.UI.Label labelEfficiencySetting;
    }
}
