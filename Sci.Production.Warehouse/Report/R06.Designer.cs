namespace Sci.Production.Warehouse
{
    partial class R06
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
            this.dateRange1 = new Sci.Win.UI.DateRange();
            this.label4 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.cbbFabricType = new System.Windows.Forms.ComboBox();
            this.txtdropdownlist1 = new Sci.Production.Class.txtdropdownlist();
            this.dateRange2 = new Sci.Win.UI.DateRange();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txtMdivision1 = new Sci.Production.Class.txtMdivision();
            this.txtfactoryByM1 = new Sci.Production.Class.txtfactoryByM();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(439, 12);
            this.print.TabIndex = 4;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(439, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(439, 84);
            this.close.TabIndex = 6;
            // 
            // dateRange1
            // 
            this.dateRange1.Location = new System.Drawing.Point(116, 12);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 23);
            this.dateRange1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(15, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 23);
            this.label4.TabIndex = 103;
            this.label4.Text = "Factory";
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(15, 192);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 23);
            this.label8.TabIndex = 118;
            this.label8.Text = "Fabric Type";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(15, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 23);
            this.label6.TabIndex = 125;
            this.label6.Text = "Shift";
            // 
            // cbbFabricType
            // 
            this.cbbFabricType.FormattingEnabled = true;
            this.cbbFabricType.Location = new System.Drawing.Point(116, 191);
            this.cbbFabricType.Name = "cbbFabricType";
            this.cbbFabricType.Size = new System.Drawing.Size(121, 24);
            this.cbbFabricType.TabIndex = 5;
            // 
            // txtdropdownlist1
            // 
            this.txtdropdownlist1.BackColor = System.Drawing.Color.White;
            this.txtdropdownlist1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlist1.FormattingEnabled = true;
            this.txtdropdownlist1.IsSupportUnselect = true;
            this.txtdropdownlist1.Location = new System.Drawing.Point(116, 155);
            this.txtdropdownlist1.Name = "txtdropdownlist1";
            this.txtdropdownlist1.Size = new System.Drawing.Size(121, 24);
            this.txtdropdownlist1.TabIndex = 4;
            this.txtdropdownlist1.Type = "Shift";
            // 
            // dateRange2
            // 
            this.dateRange2.Location = new System.Drawing.Point(116, 48);
            this.dateRange2.Name = "dateRange2";
            this.dateRange2.Size = new System.Drawing.Size(280, 23);
            this.dateRange2.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.SkyBlue;
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(15, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 23);
            this.label3.TabIndex = 129;
            this.label3.Text = "Approve Date";
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(15, 12);
            this.label2.Name = "label2";
            this.label2.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label2.Size = new System.Drawing.Size(98, 23);
            this.label2.TabIndex = 128;
            this.label2.Text = "Issue Date";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(15, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 23);
            this.label1.TabIndex = 131;
            this.label1.Text = "M";
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(116, 84);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 2;
            this.txtMdivision1.Validated += new System.EventHandler(this.txtMdivision1_Validated);
            // 
            // txtfactoryByM1
            // 
            this.txtfactoryByM1.BackColor = System.Drawing.Color.White;
            this.txtfactoryByM1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactoryByM1.Location = new System.Drawing.Point(117, 120);
            this.txtfactoryByM1.mDivisionID = null;
            this.txtfactoryByM1.Name = "txtfactoryByM1";
            this.txtfactoryByM1.Size = new System.Drawing.Size(66, 23);
            this.txtfactoryByM1.TabIndex = 132;
            // 
            // R06
            // 
            this.ClientSize = new System.Drawing.Size(531, 266);
            this.Controls.Add(this.txtfactoryByM1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMdivision1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateRange2);
            this.Controls.Add(this.txtdropdownlist1);
            this.Controls.Add(this.cbbFabricType);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateRange1);
            this.IsSupportToPrint = false;
            this.Name = "R06";
            this.Text = "R06. Fabric\\Accessory Lacking & Replacement Report";
            this.Controls.SetChildIndex(this.dateRange1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.cbbFabricType, 0);
            this.Controls.SetChildIndex(this.txtdropdownlist1, 0);
            this.Controls.SetChildIndex(this.dateRange2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtfactoryByM1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateRange1;
        private Win.UI.Label label4;
        private Win.UI.Label label8;
        private Win.UI.Label label6;
        private System.Windows.Forms.ComboBox cbbFabricType;
        private Class.txtdropdownlist txtdropdownlist1;
        private Win.UI.DateRange dateRange2;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Class.txtMdivision txtMdivision1;
        private Class.txtfactoryByM txtfactoryByM1;
    }
}
