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
            this.radioFactoryCode = new Sci.Win.UI.RadioButton();
            this.radioRegionNo = new Sci.Win.UI.RadioButton();
            this.lbGroupingby = new Sci.Win.UI.Label();
            this.lbSeason = new Sci.Win.UI.Label();
            this.lbBrand = new Sci.Win.UI.Label();
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.txtSeason = new Sci.Production.Class.Txtseason();
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
            // radioFactoryCode
            // 
            this.radioFactoryCode.AutoSize = true;
            this.radioFactoryCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioFactoryCode.Location = new System.Drawing.Point(225, 85);
            this.radioFactoryCode.Name = "radioFactoryCode";
            this.radioFactoryCode.Size = new System.Drawing.Size(110, 21);
            this.radioFactoryCode.TabIndex = 4;
            this.radioFactoryCode.Text = "Factory Code";
            this.radioFactoryCode.UseVisualStyleBackColor = true;
            // 
            // radioRegionNo
            // 
            this.radioRegionNo.AutoSize = true;
            this.radioRegionNo.Checked = true;
            this.radioRegionNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioRegionNo.Location = new System.Drawing.Point(123, 85);
            this.radioRegionNo.Name = "radioRegionNo";
            this.radioRegionNo.Size = new System.Drawing.Size(91, 21);
            this.radioRegionNo.TabIndex = 3;
            this.radioRegionNo.TabStop = true;
            this.radioRegionNo.Text = "Region no";
            this.radioRegionNo.UseVisualStyleBackColor = true;
            // 
            // lbGroupingby
            // 
            this.lbGroupingby.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbGroupingby.Lines = 0;
            this.lbGroupingby.Location = new System.Drawing.Point(9, 83);
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
            this.lbSeason.Location = new System.Drawing.Point(9, 48);
            this.lbSeason.Name = "lbSeason";
            this.lbSeason.Size = new System.Drawing.Size(100, 23);
            this.lbSeason.TabIndex = 127;
            this.lbSeason.Text = "Season";
            this.lbSeason.TextStyle.BorderColor = System.Drawing.Color.White;
            this.lbSeason.TextStyle.Color = System.Drawing.Color.White;
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
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(123, 12);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(153, 23);
            this.txtBrand.TabIndex = 0;
            // 
            // txtSeason
            // 
            this.txtSeason.BackColor = System.Drawing.Color.White;
            this.txtSeason.BrandObjectName = null;
            this.txtSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeason.Location = new System.Drawing.Point(123, 50);
            this.txtSeason.Name = "txtSeason";
            this.txtSeason.Size = new System.Drawing.Size(80, 23);
            this.txtSeason.TabIndex = 2;
            // 
            // R12
            // 
            this.ClientSize = new System.Drawing.Size(489, 157);
            this.Controls.Add(this.txtSeason);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.radioFactoryCode);
            this.Controls.Add(this.radioRegionNo);
            this.Controls.Add(this.lbGroupingby);
            this.Controls.Add(this.lbSeason);
            this.Controls.Add(this.lbBrand);
            this.DefaultControl = "txtBrand";
            this.DefaultControlForEdit = "txtBrand";
            this.Name = "R12";
            this.Text = "R12. Style  Efficiency Matrix and SMV evaluation report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.lbBrand, 0);
            this.Controls.SetChildIndex(this.lbSeason, 0);
            this.Controls.SetChildIndex(this.lbGroupingby, 0);
            this.Controls.SetChildIndex(this.radioRegionNo, 0);
            this.Controls.SetChildIndex(this.radioFactoryCode, 0);
            this.Controls.SetChildIndex(this.txtBrand, 0);
            this.Controls.SetChildIndex(this.txtSeason, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioButton radioFactoryCode;
        private Win.UI.RadioButton radioRegionNo;
        private Win.UI.Label lbGroupingby;
        private Win.UI.Label lbSeason;
        private Win.UI.Label lbBrand;
        private Class.Txtbrand txtBrand;
        private Class.Txtseason txtSeason;
    }
}
