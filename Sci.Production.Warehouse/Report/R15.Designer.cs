namespace Sci.Production.Warehouse
{
    partial class R15
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.txtwhseReason1 = new Sci.Production.Class.txtwhseReason();
            this.txtSpno2 = new Sci.Win.UI.TextBox();
            this.txtSpno1 = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(541, 12);
            this.print.TabIndex = 5;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(541, 48);
            this.toexcel.TabIndex = 6;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(541, 84);
            this.close.TabIndex = 7;
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
            this.txtMdivision1.Location = new System.Drawing.Point(115, 84);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 3;
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
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(13, 12);
            this.label2.Name = "label2";
            this.label2.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.RectStyle.BorderWidth = 1F;
            this.label2.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label2.RectStyle.ExtBorderWidth = 1F;
            this.label2.Size = new System.Drawing.Size(98, 23);
            this.label2.TabIndex = 123;
            this.label2.Text = "Issue Date";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtwhseReason1
            // 
            this.txtwhseReason1.DisplayBox1Binding = "";
            this.txtwhseReason1.Location = new System.Drawing.Point(114, 123);
            this.txtwhseReason1.Name = "txtwhseReason1";
            this.txtwhseReason1.Size = new System.Drawing.Size(386, 27);
            this.txtwhseReason1.TabIndex = 4;
            this.txtwhseReason1.TextBox1Binding = "";
            this.txtwhseReason1.Type = "IR";
            // 
            // txtSpno2
            // 
            this.txtSpno2.BackColor = System.Drawing.Color.White;
            this.txtSpno2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpno2.Location = new System.Drawing.Point(268, 48);
            this.txtSpno2.Name = "txtSpno2";
            this.txtSpno2.Size = new System.Drawing.Size(128, 23);
            this.txtSpno2.TabIndex = 2;
            // 
            // txtSpno1
            // 
            this.txtSpno1.BackColor = System.Drawing.Color.White;
            this.txtSpno1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpno1.Location = new System.Drawing.Point(115, 48);
            this.txtSpno1.Name = "txtSpno1";
            this.txtSpno1.Size = new System.Drawing.Size(128, 23);
            this.txtSpno1.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(246, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 23);
            this.label10.TabIndex = 128;
            this.label10.Text = "～";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(14, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 23);
            this.label3.TabIndex = 129;
            this.label3.Text = "SP#";
            // 
            // R15
            // 
            this.ClientSize = new System.Drawing.Size(633, 182);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSpno2);
            this.Controls.Add(this.txtSpno1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtwhseReason1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMdivision1);
            this.Controls.Add(this.dateRange1);
            this.IsSupportToPrint = false;
            this.Name = "R15";
            this.Text = "R15. Issue Material by Item List";
            this.Controls.SetChildIndex(this.dateRange1, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtwhseReason1, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.txtSpno1, 0);
            this.Controls.SetChildIndex(this.txtSpno2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateRange1;
        private Class.txtMdivision txtMdivision1;
        private Win.UI.Label label4;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Class.txtwhseReason txtwhseReason1;
        private Win.UI.TextBox txtSpno2;
        private Win.UI.TextBox txtSpno1;
        private Win.UI.Label label10;
        private Win.UI.Label label3;
    }
}
