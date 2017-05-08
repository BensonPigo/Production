namespace Sci.Production.IE
{
    partial class P01_Print
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
            this.labelEfficiencySetting = new Sci.Win.UI.Label();
            this.labelArtworkType = new Sci.Win.UI.Label();
            this.labelLanguage = new Sci.Win.UI.Label();
            this.numEfficiencySetting = new Sci.Win.UI.NumericBox();
            this.comboArtworkType = new Sci.Win.UI.ComboBox();
            this.comboLanguage = new Sci.Win.UI.ComboBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(355, 12);
            this.print.TabIndex = 3;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(355, 48);
            this.toexcel.TabIndex = 4;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(355, 84);
            this.close.TabIndex = 5;
            // 
            // labelEfficiencySetting
            // 
            this.labelEfficiencySetting.Lines = 0;
            this.labelEfficiencySetting.Location = new System.Drawing.Point(13, 13);
            this.labelEfficiencySetting.Name = "labelEfficiencySetting";
            this.labelEfficiencySetting.Size = new System.Drawing.Size(107, 23);
            this.labelEfficiencySetting.TabIndex = 94;
            this.labelEfficiencySetting.Text = "Efficiency setting";
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.Lines = 0;
            this.labelArtworkType.Location = new System.Drawing.Point(13, 48);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.Size = new System.Drawing.Size(107, 23);
            this.labelArtworkType.TabIndex = 95;
            this.labelArtworkType.Text = "Artwork type";
            // 
            // labelLanguage
            // 
            this.labelLanguage.Lines = 0;
            this.labelLanguage.Location = new System.Drawing.Point(13, 83);
            this.labelLanguage.Name = "labelLanguage";
            this.labelLanguage.Size = new System.Drawing.Size(107, 42);
            this.labelLanguage.TabIndex = 97;
            this.labelLanguage.Text = "Language";
            // 
            // numEfficiencySetting
            // 
            this.numEfficiencySetting.BackColor = System.Drawing.Color.White;
            this.numEfficiencySetting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numEfficiencySetting.Location = new System.Drawing.Point(124, 13);
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
            // comboArtworkType
            // 
            this.comboArtworkType.BackColor = System.Drawing.Color.White;
            this.comboArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboArtworkType.FormattingEnabled = true;
            this.comboArtworkType.IsSupportUnselect = true;
            this.comboArtworkType.Location = new System.Drawing.Point(124, 48);
            this.comboArtworkType.Name = "comboArtworkType";
            this.comboArtworkType.Size = new System.Drawing.Size(121, 24);
            this.comboArtworkType.TabIndex = 1;
            // 
            // comboLanguage
            // 
            this.comboLanguage.BackColor = System.Drawing.Color.White;
            this.comboLanguage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboLanguage.FormattingEnabled = true;
            this.comboLanguage.IsSupportUnselect = true;
            this.comboLanguage.Location = new System.Drawing.Point(124, 83);
            this.comboLanguage.Name = "comboLanguage";
            this.comboLanguage.Size = new System.Drawing.Size(121, 24);
            this.comboLanguage.TabIndex = 2;
            // 
            // P01_Print
            // 
            this.ClientSize = new System.Drawing.Size(447, 171);
            this.Controls.Add(this.comboLanguage);
            this.Controls.Add(this.comboArtworkType);
            this.Controls.Add(this.numEfficiencySetting);
            this.Controls.Add(this.labelLanguage);
            this.Controls.Add(this.labelArtworkType);
            this.Controls.Add(this.labelEfficiencySetting);
            this.DefaultControl = "numEfficiencySetting";
            this.DefaultControlForEdit = "numEfficiencySetting";
            this.IsSupportToPrint = false;
            this.Name = "P01_Print";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelEfficiencySetting, 0);
            this.Controls.SetChildIndex(this.labelArtworkType, 0);
            this.Controls.SetChildIndex(this.labelLanguage, 0);
            this.Controls.SetChildIndex(this.numEfficiencySetting, 0);
            this.Controls.SetChildIndex(this.comboArtworkType, 0);
            this.Controls.SetChildIndex(this.comboLanguage, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelEfficiencySetting;
        private Win.UI.Label labelArtworkType;
        private Win.UI.Label labelLanguage;
        private Win.UI.NumericBox numEfficiencySetting;
        private Win.UI.ComboBox comboArtworkType;
        private Win.UI.ComboBox comboLanguage;
    }
}
