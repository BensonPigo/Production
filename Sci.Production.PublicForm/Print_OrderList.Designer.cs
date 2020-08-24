namespace Sci.Production.PublicForm
{
    partial class Print_OrderList
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
            this.radioEachConsumption = new Sci.Win.UI.RadioButton();
            this.radioTTLConsumption = new Sci.Win.UI.RadioButton();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.label5 = new Sci.Win.UI.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioEachConsumption);
            this.groupBox1.Controls.Add(this.radioTTLConsumption);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 93);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // radioEachConsumption
            // 
            this.radioEachConsumption.AutoSize = true;
            this.radioEachConsumption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioEachConsumption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioEachConsumption.Location = new System.Drawing.Point(6, 16);
            this.radioEachConsumption.Name = "radioEachConsumption";
            this.radioEachConsumption.Size = new System.Drawing.Size(282, 24);
            this.radioEachConsumption.TabIndex = 0;
            this.radioEachConsumption.Text = "Each Consumption (Cutting Combo)";
            this.radioEachConsumption.UseVisualStyleBackColor = true;
            // 
            // radioTTLConsumption
            // 
            this.radioTTLConsumption.AutoSize = true;
            this.radioTTLConsumption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioTTLConsumption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioTTLConsumption.Location = new System.Drawing.Point(6, 46);
            this.radioTTLConsumption.Name = "radioTTLConsumption";
            this.radioTTLConsumption.Size = new System.Drawing.Size(240, 24);
            this.radioTTLConsumption.TabIndex = 1;
            this.radioTTLConsumption.Text = "TTL consumption (PO Combo)";
            this.radioTTLConsumption.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(439, 62);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(439, 26);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 0;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(432, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 23);
            this.label5.TabIndex = 182;
            this.label5.Text = "Pager Size A4";
            this.label5.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label5.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // Print_OrderList
            // 
            this.ClientSize = new System.Drawing.Size(545, 131);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnToExcel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Name = "Print_OrderList";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.btnToExcel, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Win.UI.RadioButton radioEachConsumption;
        private Win.UI.RadioButton radioTTLConsumption;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnToExcel;
        private Win.UI.Label label5;
    }
}
