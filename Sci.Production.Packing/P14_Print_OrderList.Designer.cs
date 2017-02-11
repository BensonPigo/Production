namespace Sci.Production.Packing
{
    partial class P14_Print_OrderList
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ListCheck = new Sci.Win.UI.RadioButton();
            this.SlipCheck = new Sci.Win.UI.RadioButton();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.label5 = new Sci.Win.UI.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ListCheck);
            this.groupBox1.Controls.Add(this.SlipCheck);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(161, 118);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // ListCheck
            // 
            this.ListCheck.AutoSize = true;
            this.ListCheck.Checked = true;
            this.ListCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ListCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ListCheck.Location = new System.Drawing.Point(6, 22);
            this.ListCheck.Name = "ListCheck";
            this.ListCheck.Size = new System.Drawing.Size(115, 24);
            this.ListCheck.TabIndex = 0;
            this.ListCheck.TabStop = true;
            this.ListCheck.Text = "Transfer List";
            this.ListCheck.UseVisualStyleBackColor = true;
            // 
            // SlipCheck
            // 
            this.SlipCheck.AutoSize = true;
            this.SlipCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.SlipCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.SlipCheck.Location = new System.Drawing.Point(6, 52);
            this.SlipCheck.Name = "SlipCheck";
            this.SlipCheck.Size = new System.Drawing.Size(116, 24);
            this.SlipCheck.TabIndex = 4;
            this.SlipCheck.Text = "Transfer Slip";
            this.SlipCheck.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(179, 82);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(179, 32);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 0;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.btnToExcel_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(7, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 23);
            this.label5.TabIndex = 182;
            this.label5.Text = "Pager Size A4";
            this.label5.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label5.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // P14_Print_OrderList
            // 
            this.ClientSize = new System.Drawing.Size(270, 139);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnToExcel);
            this.Controls.Add(this.groupBox1);
            this.Name = "P14_Print_OrderList";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.btnToExcel, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Win.UI.RadioButton ListCheck;
        private Win.UI.RadioButton SlipCheck;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnToExcel;
        private Win.UI.Label label5;
    }
}
