namespace Sci.Production.Planning
{
    partial class R12
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
            this.rbFactoryCode = new Sci.Win.UI.RadioButton();
            this.rbRegionNo = new Sci.Win.UI.RadioButton();
            this.lbGroupingby = new Sci.Win.UI.Label();
            this.lbSeason = new Sci.Win.UI.Label();
            this.lbCountry = new Sci.Win.UI.Label();
            this.lbBrand = new Sci.Win.UI.Label();
            this.txtcountry1 = new Sci.Production.Class.txtcountry();
            this.txtbrand1 = new Sci.Production.Class.txtbrand();
            this.txtseason1 = new Sci.Production.Class.txtseason();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(397, 12);
            this.print.TabIndex = 5;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(397, 48);
            this.toexcel.TabIndex = 6;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(397, 84);
            this.close.TabIndex = 7;
            // 
            // rbFactoryCode
            // 
            this.rbFactoryCode.AutoSize = true;
            this.rbFactoryCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rbFactoryCode.Location = new System.Drawing.Point(225, 119);
            this.rbFactoryCode.Name = "rbFactoryCode";
            this.rbFactoryCode.Size = new System.Drawing.Size(110, 21);
            this.rbFactoryCode.TabIndex = 4;
            this.rbFactoryCode.Text = "Factory Code";
            this.rbFactoryCode.UseVisualStyleBackColor = true;
            // 
            // rbRegionNo
            // 
            this.rbRegionNo.AutoSize = true;
            this.rbRegionNo.Checked = true;
            this.rbRegionNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rbRegionNo.Location = new System.Drawing.Point(123, 119);
            this.rbRegionNo.Name = "rbRegionNo";
            this.rbRegionNo.Size = new System.Drawing.Size(91, 21);
            this.rbRegionNo.TabIndex = 3;
            this.rbRegionNo.TabStop = true;
            this.rbRegionNo.Text = "Region no";
            this.rbRegionNo.UseVisualStyleBackColor = true;
            // 
            // lbGroupingby
            // 
            this.lbGroupingby.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbGroupingby.Lines = 0;
            this.lbGroupingby.Location = new System.Drawing.Point(9, 117);
            this.lbGroupingby.Name = "lbGroupingby";
            this.lbGroupingby.Size = new System.Drawing.Size(100, 23);
            this.lbGroupingby.TabIndex = 128;
            this.lbGroupingby.Text = "Grouping by";
            this.lbGroupingby.TextStyle.BorderColor = System.Drawing.Color.White;
            this.lbGroupingby.TextStyle.Color = System.Drawing.Color.White;
            // 
            // lbSeason
            // 
            this.lbSeason.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbSeason.Lines = 0;
            this.lbSeason.Location = new System.Drawing.Point(9, 82);
            this.lbSeason.Name = "lbSeason";
            this.lbSeason.Size = new System.Drawing.Size(100, 23);
            this.lbSeason.TabIndex = 127;
            this.lbSeason.Text = "Season";
            this.lbSeason.TextStyle.BorderColor = System.Drawing.Color.White;
            this.lbSeason.TextStyle.Color = System.Drawing.Color.White;
            // 
            // lbCountry
            // 
            this.lbCountry.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbCountry.Lines = 0;
            this.lbCountry.Location = new System.Drawing.Point(9, 47);
            this.lbCountry.Name = "lbCountry";
            this.lbCountry.Size = new System.Drawing.Size(100, 23);
            this.lbCountry.TabIndex = 126;
            this.lbCountry.Text = "Country";
            this.lbCountry.TextStyle.BorderColor = System.Drawing.Color.White;
            this.lbCountry.TextStyle.Color = System.Drawing.Color.White;
            // 
            // lbBrand
            // 
            this.lbBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbBrand.Lines = 0;
            this.lbBrand.Location = new System.Drawing.Point(9, 12);
            this.lbBrand.Name = "lbBrand";
            this.lbBrand.Size = new System.Drawing.Size(100, 23);
            this.lbBrand.TabIndex = 125;
            this.lbBrand.Text = "Brand";
            this.lbBrand.TextStyle.BorderColor = System.Drawing.Color.White;
            this.lbBrand.TextStyle.Color = System.Drawing.Color.White;
            // 
            // txtcountry1
            // 
            this.txtcountry1.DisplayBox1Binding = "";
            this.txtcountry1.Location = new System.Drawing.Point(123, 47);
            this.txtcountry1.Name = "txtcountry1";
            this.txtcountry1.Size = new System.Drawing.Size(232, 22);
            this.txtcountry1.TabIndex = 1;
            this.txtcountry1.TextBox1Binding = "";
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(123, 12);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(153, 23);
            this.txtbrand1.TabIndex = 0;
            // 
            // txtseason1
            // 
            this.txtseason1.BackColor = System.Drawing.Color.White;
            this.txtseason1.BrandObjectName = null;
            this.txtseason1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason1.Location = new System.Drawing.Point(123, 84);
            this.txtseason1.Name = "txtseason1";
            this.txtseason1.Size = new System.Drawing.Size(80, 23);
            this.txtseason1.TabIndex = 2;
            // 
            // R12
            // 
            this.ClientSize = new System.Drawing.Size(489, 181);
            this.Controls.Add(this.txtseason1);
            this.Controls.Add(this.txtbrand1);
            this.Controls.Add(this.txtcountry1);
            this.Controls.Add(this.rbFactoryCode);
            this.Controls.Add(this.rbRegionNo);
            this.Controls.Add(this.lbGroupingby);
            this.Controls.Add(this.lbSeason);
            this.Controls.Add(this.lbCountry);
            this.Controls.Add(this.lbBrand);
            this.DefaultControl = "txtbrand1";
            this.DefaultControlForEdit = "txtbrand1";
            this.Name = "R12";
            this.Text = "R12. Style  Efficiency Matrix and SMV evaluation report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.lbBrand, 0);
            this.Controls.SetChildIndex(this.lbCountry, 0);
            this.Controls.SetChildIndex(this.lbSeason, 0);
            this.Controls.SetChildIndex(this.lbGroupingby, 0);
            this.Controls.SetChildIndex(this.rbRegionNo, 0);
            this.Controls.SetChildIndex(this.rbFactoryCode, 0);
            this.Controls.SetChildIndex(this.txtcountry1, 0);
            this.Controls.SetChildIndex(this.txtbrand1, 0);
            this.Controls.SetChildIndex(this.txtseason1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioButton rbFactoryCode;
        private Win.UI.RadioButton rbRegionNo;
        private Win.UI.Label lbGroupingby;
        private Win.UI.Label lbSeason;
        private Win.UI.Label lbCountry;
        private Win.UI.Label lbBrand;
        private Class.txtcountry txtcountry1;
        private Class.txtbrand txtbrand1;
        private Class.txtseason txtseason1;
    }
}
