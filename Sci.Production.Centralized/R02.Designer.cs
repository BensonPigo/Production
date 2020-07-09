namespace Sci.Production.Centralized
{
    partial class R02
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
            this.txtFactory1 = new Sci.Production.Class.TxtCentralizedFactory();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.numericUpDown1 = new Sci.Win.UI.NumericUpDown();
            this.txtCountry1 = new Sci.Production.Class.Txtcountry();
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(450, 1);
            this.print.Size = new System.Drawing.Size(93, 30);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(450, 30);
            this.toexcel.Size = new System.Drawing.Size(93, 30);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(450, 59);
            this.close.Size = new System.Drawing.Size(93, 30);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "SCI Delivery(YEAR)";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(2, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 23);
            this.label2.TabIndex = 95;
            this.label2.Text = "Factory";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(2, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 23);
            this.label3.TabIndex = 96;
            this.label3.Text = "Country";
            // 
            // txtFactory1
            // 
            this.txtFactory1.BackColor = System.Drawing.Color.White;
            this.txtFactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory1.Location = new System.Drawing.Point(132, 26);
            this.txtFactory1.MaxLength = 8;
            this.txtFactory1.Name = "txtFactory1";
            this.txtFactory1.Size = new System.Drawing.Size(100, 23);
            this.txtFactory1.TabIndex = 99;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(2, 77);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(141, 21);
            this.checkBox1.TabIndex = 101;
            this.checkBox1.Text = "Export Detail Data";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.BackColor = System.Drawing.Color.White;
            this.numericUpDown1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericUpDown1.Location = new System.Drawing.Point(132, 2);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(100, 23);
            this.numericUpDown1.TabIndex = 102;
            this.numericUpDown1.Value = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.NumericBox1_Leave);
            // 
            // txtCountry1
            // 
            this.txtCountry1.DisplayBox1Binding = "";
            this.txtCountry1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtCountry1.Location = new System.Drawing.Point(132, 50);
            this.txtCountry1.Name = "txtCountry1";
            this.txtCountry1.Size = new System.Drawing.Size(257, 22);
            this.txtCountry1.TabIndex = 103;
            this.txtCountry1.TextBox1Binding = "";
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(2, 102);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 172;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // R02
            // 
            this.ClientSize = new System.Drawing.Size(545, 149);
            this.ControlBox = false;
            this.Controls.Add(this.chkIncludeCancelOrder);
            this.Controls.Add(this.txtCountry1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.txtFactory1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "R02";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R02. CPU Loading Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtFactory1, 0);
            this.Controls.SetChildIndex(this.checkBox1, 0);
            this.Controls.SetChildIndex(this.numericUpDown1, 0);
            this.Controls.SetChildIndex(this.txtCountry1, 0);
            this.Controls.SetChildIndex(this.chkIncludeCancelOrder, 0);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Class.TxtCentralizedFactory txtFactory1;
        private Win.UI.CheckBox checkBox1;
        private Win.UI.NumericUpDown numericUpDown1;
        private Class.Txtcountry txtCountry1;
        private Win.UI.CheckBox chkIncludeCancelOrder;
    }
}