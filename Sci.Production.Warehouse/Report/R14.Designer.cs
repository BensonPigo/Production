namespace Sci.Production.Warehouse
{
    partial class R14
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
            this.label2 = new Sci.Win.UI.Label();
            this.txtdropdownlist1 = new Sci.Production.Class.txtdropdownlist();
            this.label6 = new Sci.Win.UI.Label();
            this.cbbFabricType = new System.Windows.Forms.ComboBox();
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
            this.dateRange1.Location = new System.Drawing.Point(115, 12);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 23);
            this.dateRange1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(13, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 23);
            this.label4.TabIndex = 103;
            this.label4.Text = "Factory";
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(13, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 23);
            this.label8.TabIndex = 118;
            this.label8.Text = "Fabric Type";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(14, 12);
            this.label2.Name = "label2";
            this.label2.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.RectStyle.BorderWidth = 1F;
            this.label2.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label2.RectStyle.ExtBorderWidth = 1F;
            this.label2.Size = new System.Drawing.Size(98, 23);
            this.label2.TabIndex = 122;
            this.label2.Text = "WK# ETA";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtdropdownlist1
            // 
            this.txtdropdownlist1.BackColor = System.Drawing.Color.White;
            this.txtdropdownlist1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlist1.FormattingEnabled = true;
            this.txtdropdownlist1.IsSupportUnselect = true;
            this.txtdropdownlist1.Location = new System.Drawing.Point(115, 123);
            this.txtdropdownlist1.Name = "txtdropdownlist1";
            this.txtdropdownlist1.Size = new System.Drawing.Size(261, 24);
            this.txtdropdownlist1.TabIndex = 3;
            this.txtdropdownlist1.Type = "Category";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(14, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 23);
            this.label6.TabIndex = 125;
            this.label6.Text = "Order Type";
            // 
            // cbbFabricType
            // 
            this.cbbFabricType.FormattingEnabled = true;
            this.cbbFabricType.Location = new System.Drawing.Point(115, 47);
            this.cbbFabricType.Name = "cbbFabricType";
            this.cbbFabricType.Size = new System.Drawing.Size(121, 24);
            this.cbbFabricType.TabIndex = 1;
            // 
            // txtfactoryByM1
            // 
            this.txtfactoryByM1.BackColor = System.Drawing.Color.White;
            this.txtfactoryByM1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactoryByM1.Location = new System.Drawing.Point(115, 84);
            this.txtfactoryByM1.mDivisionID = null;
            this.txtfactoryByM1.Name = "txtfactoryByM1";
            this.txtfactoryByM1.Size = new System.Drawing.Size(66, 23);
            this.txtfactoryByM1.TabIndex = 2;
            // 
            // R14
            // 
            this.ClientSize = new System.Drawing.Size(531, 182);
            this.Controls.Add(this.txtfactoryByM1);
            this.Controls.Add(this.cbbFabricType);
            this.Controls.Add(this.txtdropdownlist1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateRange1);
            this.IsSupportToPrint = false;
            this.Name = "R14";
            this.Text = "R14. Material Outstanding / Lacking Report";
            this.Controls.SetChildIndex(this.dateRange1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtdropdownlist1, 0);
            this.Controls.SetChildIndex(this.cbbFabricType, 0);
            this.Controls.SetChildIndex(this.txtfactoryByM1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateRange1;
        private Win.UI.Label label4;
        private Win.UI.Label label8;
        private Win.UI.Label label2;
        private Class.txtdropdownlist txtdropdownlist1;
        private Win.UI.Label label6;
        private System.Windows.Forms.ComboBox cbbFabricType;
        private Class.txtfactoryByM txtfactoryByM1;
    }
}
