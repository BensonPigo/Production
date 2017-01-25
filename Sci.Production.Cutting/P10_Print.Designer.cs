namespace Sci.Production.Cutting
{
    partial class P10_Print
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
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(392, 12);
            this.print.TabIndex = 1;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(392, 48);
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(392, 84);
            this.close.TabIndex = 3;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.checkBox1);
            this.radioPanel1.Controls.Add(this.radioButton2);
            this.radioPanel1.Controls.Add(this.radioButton1);
            this.radioPanel1.Location = new System.Drawing.Point(12, 11);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(350, 100);
            this.radioPanel1.TabIndex = 0;
            this.radioPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.radioPanel1_Paint);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.ForeColor = System.Drawing.Color.Red;
            this.checkBox1.Location = new System.Drawing.Point(210, 18);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(126, 21);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Extend All Parts";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(28, 57);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(134, 21);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.Text = "Bundle Check list";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(28, 17);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(131, 21);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Bundle Card(A4)";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // P10_Print
            // 
            this.ClientSize = new System.Drawing.Size(484, 162);
            this.Controls.Add(this.radioPanel1);
            this.DefaultControl = "radioButton1";
            this.Name = "P10_Print";
            this.Text = "P10. Print";
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;

    }
}
