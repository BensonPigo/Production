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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.numericBox1 = new Sci.Win.UI.NumericBox();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.comboBox2 = new Sci.Win.UI.ComboBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(355, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(355, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(355, 84);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Efficiency setting";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(13, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 23);
            this.label2.TabIndex = 95;
            this.label2.Text = "Artwork type";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(13, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 42);
            this.label4.TabIndex = 97;
            this.label4.Text = "Language";
            // 
            // numericBox1
            // 
            this.numericBox1.BackColor = System.Drawing.Color.White;
            this.numericBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox1.Location = new System.Drawing.Point(124, 13);
            this.numericBox1.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericBox1.MaxLength = 3;
            this.numericBox1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.Size = new System.Drawing.Size(50, 23);
            this.numericBox1.TabIndex = 98;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(124, 48);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 99;
            // 
            // comboBox2
            // 
            this.comboBox2.BackColor = System.Drawing.Color.White;
            this.comboBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.IsSupportUnselect = true;
            this.comboBox2.Location = new System.Drawing.Point(124, 83);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 24);
            this.comboBox2.TabIndex = 101;
            // 
            // P01_Print
            // 
            this.ClientSize = new System.Drawing.Size(447, 171);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.numericBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.EditMode = true;
            this.IsSupportToPrint = false;
            this.Name = "P01_Print";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.numericBox1, 0);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            this.Controls.SetChildIndex(this.comboBox2, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label4;
        private Win.UI.NumericBox numericBox1;
        private Win.UI.ComboBox comboBox1;
        private Win.UI.ComboBox comboBox2;
    }
}
