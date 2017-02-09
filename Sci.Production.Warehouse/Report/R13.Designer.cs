namespace Sci.Production.Warehouse
{
    partial class R13
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
            this.txtMdivision1 = new Sci.Production.Class.txtMdivision();
            this.label8 = new Sci.Win.UI.Label();
            this.cbbStockType = new System.Windows.Forms.ComboBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtReason1 = new Sci.Production.Class.txtReason();
            this.label2 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(541, 12);
            this.print.TabIndex = 4;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(541, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(541, 84);
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
            this.label4.Text = "M";
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(114, 84);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(13, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 23);
            this.label8.TabIndex = 118;
            this.label8.Text = "Stock Type";
            // 
            // cbbStockType
            // 
            this.cbbStockType.FormattingEnabled = true;
            this.cbbStockType.Location = new System.Drawing.Point(115, 47);
            this.cbbStockType.Name = "cbbStockType";
            this.cbbStockType.Size = new System.Drawing.Size(121, 24);
            this.cbbStockType.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(13, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 23);
            this.label1.TabIndex = 120;
            this.label1.Text = "Reason Code";
            // 
            // txtReason1
            // 
            this.txtReason1.BackColor = System.Drawing.Color.White;
            this.txtReason1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReason1.FormattingEnabled = true;
            this.txtReason1.IsSupportUnselect = true;
            this.txtReason1.Location = new System.Drawing.Point(115, 123);
            this.txtReason1.Name = "txtReason1";
            this.txtReason1.ReasonTypeID = "Stock_Adjust";
            this.txtReason1.Size = new System.Drawing.Size(388, 24);
            this.txtReason1.TabIndex = 3;
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
            this.label2.Text = "Adjust Date";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // R13
            // 
            this.ClientSize = new System.Drawing.Size(633, 182);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtReason1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbbStockType);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMdivision1);
            this.Controls.Add(this.dateRange1);
            this.IsSupportToPrint = false;
            this.Name = "R13";
            this.Text = "R13. Warehouse Adjust Report";
            this.Controls.SetChildIndex(this.dateRange1, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.cbbStockType, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtReason1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateRange1;
        private Class.txtMdivision txtMdivision1;
        private Win.UI.Label label4;
        private Win.UI.Label label8;
        private System.Windows.Forms.ComboBox cbbStockType;
        private Win.UI.Label label1;
        private Class.txtReason txtReason1;
        private Win.UI.Label label2;
    }
}
