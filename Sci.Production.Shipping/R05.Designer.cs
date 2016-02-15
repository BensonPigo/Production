namespace Sci.Production.Shipping
{
    partial class R05
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.dateRange1 = new Sci.Win.UI.DateRange();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.comboBox2 = new Sci.Win.UI.ComboBox();
            this.txtsubcon1 = new Sci.Production.Class.txtsubcon();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(382, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(382, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(382, 84);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Date";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(13, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 23);
            this.label2.TabIndex = 95;
            this.label2.Text = "M";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(13, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 23);
            this.label3.TabIndex = 96;
            this.label3.Text = "Supplier";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(13, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 23);
            this.label4.TabIndex = 97;
            this.label4.Text = "Order by";
            // 
            // dateRange1
            // 
            this.dateRange1.Location = new System.Drawing.Point(78, 13);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 23);
            this.dateRange1.TabIndex = 98;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(78, 48);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(68, 24);
            this.comboBox1.TabIndex = 99;
            // 
            // comboBox2
            // 
            this.comboBox2.BackColor = System.Drawing.Color.White;
            this.comboBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.IsSupportUnselect = true;
            this.comboBox2.Location = new System.Drawing.Point(78, 120);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(103, 24);
            this.comboBox2.TabIndex = 100;
            // 
            // txtsubcon1
            // 
            this.txtsubcon1.DisplayBox1Binding = "";
            this.txtsubcon1.IsIncludeJunk = false;
            this.txtsubcon1.Location = new System.Drawing.Point(78, 84);
            this.txtsubcon1.Name = "txtsubcon1";
            this.txtsubcon1.Size = new System.Drawing.Size(170, 23);
            this.txtsubcon1.TabIndex = 101;
            this.txtsubcon1.TextBox1Binding = "";
            // 
            // R05
            // 
            this.ClientSize = new System.Drawing.Size(474, 185);
            this.Controls.Add(this.txtsubcon1);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dateRange1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.EditMode = true;
            this.IsSupportToPrint = false;
            this.Name = "R05";
            this.Text = "R05. Outstanding Payment List - Shipping";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.dateRange1, 0);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            this.Controls.SetChildIndex(this.comboBox2, 0);
            this.Controls.SetChildIndex(this.txtsubcon1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.DateRange dateRange1;
        private Win.UI.ComboBox comboBox1;
        private Win.UI.ComboBox comboBox2;
        private Class.txtsubcon txtsubcon1;
    }
}
