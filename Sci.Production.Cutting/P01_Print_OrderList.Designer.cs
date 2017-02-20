namespace Sci.Production.Cutting
{
    partial class P01_Print_OrderList
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
            this.rdcheck_QtyBreakdown_PoCombbySPList = new Sci.Win.UI.RadioButton();
            this.rdCheck_CuttingWorkOrder = new Sci.Win.UI.RadioButton();
            this.rdCheck1 = new Sci.Win.UI.RadioButton();
            this.rdCheck6 = new Sci.Win.UI.RadioButton();
            this.rdCheck2 = new Sci.Win.UI.RadioButton();
            this.rdCheck5 = new Sci.Win.UI.RadioButton();
            this.rdCheck3 = new Sci.Win.UI.RadioButton();
            this.rdCheck4 = new Sci.Win.UI.RadioButton();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.label5 = new Sci.Win.UI.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdcheck_QtyBreakdown_PoCombbySPList);
            this.groupBox1.Controls.Add(this.rdCheck_CuttingWorkOrder);
            this.groupBox1.Controls.Add(this.rdCheck1);
            this.groupBox1.Controls.Add(this.rdCheck6);
            this.groupBox1.Controls.Add(this.rdCheck2);
            this.groupBox1.Controls.Add(this.rdCheck5);
            this.groupBox1.Controls.Add(this.rdCheck3);
            this.groupBox1.Controls.Add(this.rdCheck4);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(484, 262);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // rdcheck_QtyBreakdown_PoCombbySPList
            // 
            this.rdcheck_QtyBreakdown_PoCombbySPList.AutoSize = true;
            this.rdcheck_QtyBreakdown_PoCombbySPList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.rdcheck_QtyBreakdown_PoCombbySPList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdcheck_QtyBreakdown_PoCombbySPList.Location = new System.Drawing.Point(6, 140);
            this.rdcheck_QtyBreakdown_PoCombbySPList.Name = "rdcheck_QtyBreakdown_PoCombbySPList";
            this.rdcheck_QtyBreakdown_PoCombbySPList.Size = new System.Drawing.Size(296, 24);
            this.rdcheck_QtyBreakdown_PoCombbySPList.TabIndex = 7;
            this.rdcheck_QtyBreakdown_PoCombbySPList.Text = "Q\'ty Breakdown(Po Comb by SP# List)";
            this.rdcheck_QtyBreakdown_PoCombbySPList.UseVisualStyleBackColor = true;
            // 
            // rdCheck_CuttingWorkOrder
            // 
            this.rdCheck_CuttingWorkOrder.AutoSize = true;
            this.rdCheck_CuttingWorkOrder.Checked = true;
            this.rdCheck_CuttingWorkOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.rdCheck_CuttingWorkOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdCheck_CuttingWorkOrder.Location = new System.Drawing.Point(6, 20);
            this.rdCheck_CuttingWorkOrder.Name = "rdCheck_CuttingWorkOrder";
            this.rdCheck_CuttingWorkOrder.Size = new System.Drawing.Size(163, 24);
            this.rdCheck_CuttingWorkOrder.TabIndex = 6;
            this.rdCheck_CuttingWorkOrder.TabStop = true;
            this.rdCheck_CuttingWorkOrder.Text = "Cutting Work Order";
            this.rdCheck_CuttingWorkOrder.UseVisualStyleBackColor = true;
            // 
            // rdCheck1
            // 
            this.rdCheck1.AutoSize = true;
            this.rdCheck1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.rdCheck1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdCheck1.Location = new System.Drawing.Point(6, 50);
            this.rdCheck1.Name = "rdCheck1";
            this.rdCheck1.Size = new System.Drawing.Size(282, 24);
            this.rdCheck1.TabIndex = 0;
            this.rdCheck1.Text = "Each Consumption (Cutting Combo)";
            this.rdCheck1.UseVisualStyleBackColor = true;
            // 
            // rdCheck6
            // 
            this.rdCheck6.AutoSize = true;
            this.rdCheck6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.rdCheck6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdCheck6.Location = new System.Drawing.Point(6, 230);
            this.rdCheck6.Name = "rdCheck6";
            this.rdCheck6.Size = new System.Drawing.Size(383, 24);
            this.rdCheck6.TabIndex = 5;
            this.rdCheck6.Text = "Consumption Calculate by Marker List Cons/Per pc";
            this.rdCheck6.UseVisualStyleBackColor = true;
            // 
            // rdCheck2
            // 
            this.rdCheck2.AutoSize = true;
            this.rdCheck2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.rdCheck2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdCheck2.Location = new System.Drawing.Point(6, 80);
            this.rdCheck2.Name = "rdCheck2";
            this.rdCheck2.Size = new System.Drawing.Size(240, 24);
            this.rdCheck2.TabIndex = 1;
            this.rdCheck2.Text = "TTL consumption (PO Combo)";
            this.rdCheck2.UseVisualStyleBackColor = true;
            // 
            // rdCheck5
            // 
            this.rdCheck5.AutoSize = true;
            this.rdCheck5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.rdCheck5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdCheck5.Location = new System.Drawing.Point(6, 200);
            this.rdCheck5.Name = "rdCheck5";
            this.rdCheck5.Size = new System.Drawing.Size(105, 24);
            this.rdCheck5.TabIndex = 4;
            this.rdCheck5.Text = "Marker List";
            this.rdCheck5.UseVisualStyleBackColor = true;
            // 
            // rdCheck3
            // 
            this.rdCheck3.AutoSize = true;
            this.rdCheck3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.rdCheck3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdCheck3.Location = new System.Drawing.Point(6, 110);
            this.rdCheck3.Name = "rdCheck3";
            this.rdCheck3.Size = new System.Drawing.Size(249, 24);
            this.rdCheck3.TabIndex = 2;
            this.rdCheck3.Text = "Color & Q\'ty B\'Down (PO Combo)";
            this.rdCheck3.UseVisualStyleBackColor = true;
            // 
            // rdCheck4
            // 
            this.rdCheck4.AutoSize = true;
            this.rdCheck4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.rdCheck4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdCheck4.Location = new System.Drawing.Point(6, 170);
            this.rdCheck4.Name = "rdCheck4";
            this.rdCheck4.Size = new System.Drawing.Size(350, 24);
            this.rdCheck4.TabIndex = 3;
            this.rdCheck4.Text = "Each cons. vs Order Q\'ty B\'Down (PO Combo)";
            this.rdCheck4.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(507, 62);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(507, 26);
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
            this.label5.Location = new System.Drawing.Point(507, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 23);
            this.label5.TabIndex = 182;
            this.label5.Text = "Pager Size A4";
            this.label5.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label5.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // P01_Print_OrderList
            // 
            this.ClientSize = new System.Drawing.Size(613, 281);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnToExcel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Name = "P01_Print_OrderList";
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
        private Win.UI.RadioButton rdCheck1;
        private Win.UI.RadioButton rdCheck6;
        private Win.UI.RadioButton rdCheck2;
        private Win.UI.RadioButton rdCheck5;
        private Win.UI.RadioButton rdCheck3;
        private Win.UI.RadioButton rdCheck4;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnToExcel;
        private Win.UI.Label label5;
        private Win.UI.RadioButton rdcheck_QtyBreakdown_PoCombbySPList;
        private Win.UI.RadioButton rdCheck_CuttingWorkOrder;
    }
}
