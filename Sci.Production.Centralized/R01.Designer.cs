namespace Sci.Production.Centralized
{
    partial class R01
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
            this.lbGroupingby = new Sci.Win.UI.Label();
            this.lbSeason = new Sci.Win.UI.Label();
            this.lbCountry = new Sci.Win.UI.Label();
            this.lbBrand = new Sci.Win.UI.Label();
            this.rbFactoryCode = new Sci.Win.UI.RadioButton();
            this.rbRegionNo = new Sci.Win.UI.RadioButton();
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.txtCountry = new Sci.Production.Class.Txtcountry();
            this.txtSeason = new Sci.Production.Class.Txtseason();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(345, 3);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(345, 39);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(345, 72);
            // 
            // lbGroupingby
            // 
            this.lbGroupingby.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbGroupingby.Location = new System.Drawing.Point(9, 79);
            this.lbGroupingby.Name = "lbGroupingby";
            this.lbGroupingby.Size = new System.Drawing.Size(100, 23);
            this.lbGroupingby.TabIndex = 101;
            this.lbGroupingby.Text = "Grouping by";
            this.lbGroupingby.TextStyle.BorderColor = System.Drawing.Color.White;
            this.lbGroupingby.TextStyle.Color = System.Drawing.Color.White;
            // 
            // lbSeason
            // 
            this.lbSeason.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbSeason.Location = new System.Drawing.Point(9, 55);
            this.lbSeason.Name = "lbSeason";
            this.lbSeason.Size = new System.Drawing.Size(100, 23);
            this.lbSeason.TabIndex = 100;
            this.lbSeason.Text = "Season";
            this.lbSeason.TextStyle.BorderColor = System.Drawing.Color.White;
            this.lbSeason.TextStyle.Color = System.Drawing.Color.White;
            // 
            // lbCountry
            // 
            this.lbCountry.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbCountry.Location = new System.Drawing.Point(9, 31);
            this.lbCountry.Name = "lbCountry";
            this.lbCountry.Size = new System.Drawing.Size(100, 23);
            this.lbCountry.TabIndex = 99;
            this.lbCountry.Text = "Country";
            this.lbCountry.TextStyle.BorderColor = System.Drawing.Color.White;
            this.lbCountry.TextStyle.Color = System.Drawing.Color.White;
            // 
            // lbBrand
            // 
            this.lbBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbBrand.Location = new System.Drawing.Point(9, 7);
            this.lbBrand.Name = "lbBrand";
            this.lbBrand.Size = new System.Drawing.Size(100, 23);
            this.lbBrand.TabIndex = 98;
            this.lbBrand.Text = "Brand";
            this.lbBrand.TextStyle.BorderColor = System.Drawing.Color.White;
            this.lbBrand.TextStyle.Color = System.Drawing.Color.White;
            // 
            // rbFactoryCode
            // 
            this.rbFactoryCode.AutoSize = true;
            this.rbFactoryCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rbFactoryCode.Location = new System.Drawing.Point(218, 81);
            this.rbFactoryCode.Name = "rbFactoryCode";
            this.rbFactoryCode.Size = new System.Drawing.Size(110, 21);
            this.rbFactoryCode.TabIndex = 126;
            this.rbFactoryCode.Text = "Factory Code";
            this.rbFactoryCode.UseVisualStyleBackColor = true;
            // 
            // rbRegionNo
            // 
            this.rbRegionNo.AutoSize = true;
            this.rbRegionNo.Checked = true;
            this.rbRegionNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rbRegionNo.Location = new System.Drawing.Point(112, 81);
            this.rbRegionNo.Name = "rbRegionNo";
            this.rbRegionNo.Size = new System.Drawing.Size(91, 21);
            this.rbRegionNo.TabIndex = 125;
            this.rbRegionNo.TabStop = true;
            this.rbRegionNo.Text = "Region no";
            this.rbRegionNo.UseVisualStyleBackColor = true;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(112, 7);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(227, 23);
            this.txtBrand.TabIndex = 127;
            // 
            // txtCountry
            // 
            this.txtCountry.DisplayBox1Binding = "";
            this.txtCountry.Location = new System.Drawing.Point(112, 31);
            this.txtCountry.Name = "txtCountry";
            this.txtCountry.Size = new System.Drawing.Size(232, 22);
            this.txtCountry.TabIndex = 128;
            this.txtCountry.TextBox1Binding = "";
            // 
            // txtSeason
            // 
            this.txtSeason.BackColor = System.Drawing.Color.White;
            this.txtSeason.BrandObjectName = null;
            this.txtSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeason.Location = new System.Drawing.Point(113, 55);
            this.txtSeason.Name = "txtSeason";
            this.txtSeason.Size = new System.Drawing.Size(226, 23);
            this.txtSeason.TabIndex = 129;
            // 
            // R01
            // 
            this.ClientSize = new System.Drawing.Size(437, 137);
            this.Controls.Add(this.txtSeason);
            this.Controls.Add(this.txtCountry);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.rbFactoryCode);
            this.Controls.Add(this.rbRegionNo);
            this.Controls.Add(this.lbGroupingby);
            this.Controls.Add(this.lbSeason);
            this.Controls.Add(this.lbCountry);
            this.Controls.Add(this.lbBrand);
            this.Name = "R01";
            this.Text = "R01. Style Efficiency Matrix and SMV Evaluation Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.lbBrand, 0);
            this.Controls.SetChildIndex(this.lbCountry, 0);
            this.Controls.SetChildIndex(this.lbSeason, 0);
            this.Controls.SetChildIndex(this.lbGroupingby, 0);
            this.Controls.SetChildIndex(this.rbRegionNo, 0);
            this.Controls.SetChildIndex(this.rbFactoryCode, 0);
            this.Controls.SetChildIndex(this.txtBrand, 0);
            this.Controls.SetChildIndex(this.txtCountry, 0);
            this.Controls.SetChildIndex(this.txtSeason, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbGroupingby;
        private Win.UI.Label lbSeason;
        private Win.UI.Label lbCountry;
        private Win.UI.Label lbBrand;
        private Win.UI.RadioButton rbFactoryCode;
        private Win.UI.RadioButton rbRegionNo;
        private Class.Txtbrand txtBrand;
        private Class.Txtcountry txtCountry;
        private Class.Txtseason txtSeason;
    }
}
