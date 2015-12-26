namespace Sci.Production.Thread
{
    partial class P21
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
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.dateBox1 = new Sci.Win.UI.DateBox();
            this.dateBox2 = new Sci.Win.UI.DateBox();
            this.displayBox2 = new Sci.Win.UI.DisplayBox();
            this.displayBox3 = new Sci.Win.UI.DisplayBox();
            this.dateBox3 = new Sci.Win.UI.DateBox();
            this.dateBox4 = new Sci.Win.UI.DateBox();
            this.displayBox4 = new Sci.Win.UI.DisplayBox();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).BeginInit();
            this.masterpanel.SuspendLayout();
            this.detailpanel.SuspendLayout();
            this.detail2.SuspendLayout();
            this.detailpanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.displayBox4);
            this.masterpanel.Controls.Add(this.dateBox4);
            this.masterpanel.Controls.Add(this.dateBox3);
            this.masterpanel.Controls.Add(this.displayBox3);
            this.masterpanel.Controls.Add(this.displayBox2);
            this.masterpanel.Controls.Add(this.dateBox2);
            this.masterpanel.Controls.Add(this.dateBox1);
            this.masterpanel.Controls.Add(this.label7);
            this.masterpanel.Controls.Add(this.label8);
            this.masterpanel.Controls.Add(this.label9);
            this.masterpanel.Controls.Add(this.label6);
            this.masterpanel.Controls.Add(this.label5);
            this.masterpanel.Controls.Add(this.label4);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.displayBox1);
            this.masterpanel.Controls.Add(this.textBox1);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Size = new System.Drawing.Size(892, 143);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.textBox1, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBox1, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.label4, 0);
            this.masterpanel.Controls.SetChildIndex(this.label5, 0);
            this.masterpanel.Controls.SetChildIndex(this.label6, 0);
            this.masterpanel.Controls.SetChildIndex(this.label9, 0);
            this.masterpanel.Controls.SetChildIndex(this.label8, 0);
            this.masterpanel.Controls.SetChildIndex(this.label7, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateBox1, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateBox2, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBox2, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBox3, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateBox3, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateBox4, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBox4, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 143);
            this.detailpanel.Size = new System.Drawing.Size(892, 206);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(789, 108);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(892, 206);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(892, 387);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(886, 341);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(886, 40);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(42, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "SP No";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "orderid", true));
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(120, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(108, 23);
            this.textBox1.TabIndex = 2;
            this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "styleid", true));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(120, 60);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(120, 23);
            this.displayBox1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(42, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "Style#";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(42, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Season";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(270, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 23);
            this.label4.TabIndex = 6;
            this.label4.Text = "Brand";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(270, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 23);
            this.label5.TabIndex = 7;
            this.label5.Text = "SCI Delivery";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(270, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 23);
            this.label6.TabIndex = 8;
            this.label6.Text = "Sewing In- Line";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(530, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 23);
            this.label7.TabIndex = 11;
            this.label7.Text = "Est. Arrived";
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(530, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 23);
            this.label8.TabIndex = 10;
            this.label8.Text = "Est. Booking";
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(530, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 23);
            this.label9.TabIndex = 9;
            this.label9.Text = "Factory";
            // 
            // dateBox1
            // 
            this.dateBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "estbookDate", true));
            this.dateBox1.Location = new System.Drawing.Point(618, 61);
            this.dateBox1.Name = "dateBox1";
            this.dateBox1.Size = new System.Drawing.Size(130, 23);
            this.dateBox1.TabIndex = 12;
            // 
            // dateBox2
            // 
            this.dateBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "EstArriveDate", true));
            this.dateBox2.Location = new System.Drawing.Point(618, 103);
            this.dateBox2.Name = "dateBox2";
            this.dateBox2.Size = new System.Drawing.Size(130, 23);
            this.dateBox2.TabIndex = 13;
            // 
            // displayBox2
            // 
            this.displayBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "seasonid", true));
            this.displayBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox2.Location = new System.Drawing.Point(120, 102);
            this.displayBox2.Name = "displayBox2";
            this.displayBox2.Size = new System.Drawing.Size(90, 23);
            this.displayBox2.TabIndex = 14;
            // 
            // displayBox3
            // 
            this.displayBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "brandid", true));
            this.displayBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox3.Location = new System.Drawing.Point(376, 19);
            this.displayBox3.Name = "displayBox3";
            this.displayBox3.Size = new System.Drawing.Size(78, 23);
            this.displayBox3.TabIndex = 15;
            // 
            // dateBox3
            // 
            this.dateBox3.IsSupportEditMode = false;
            this.dateBox3.Location = new System.Drawing.Point(376, 60);
            this.dateBox3.Name = "dateBox3";
            this.dateBox3.ReadOnly = true;
            this.dateBox3.Size = new System.Drawing.Size(130, 23);
            this.dateBox3.TabIndex = 13;
            // 
            // dateBox4
            // 
            this.dateBox4.IsSupportEditMode = false;
            this.dateBox4.Location = new System.Drawing.Point(376, 102);
            this.dateBox4.Name = "dateBox4";
            this.dateBox4.ReadOnly = true;
            this.dateBox4.Size = new System.Drawing.Size(130, 23);
            this.dateBox4.TabIndex = 13;
            // 
            // displayBox4
            // 
            this.displayBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox4.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "factoryid", true));
            this.displayBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox4.Location = new System.Drawing.Point(618, 20);
            this.displayBox4.Name = "displayBox4";
            this.displayBox4.Size = new System.Drawing.Size(66, 23);
            this.displayBox4.TabIndex = 16;
            // 
            // P21
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(900, 449);
            this.DefaultDetailOrder = "Refno,ThreadColorid";
            this.DefaultOrder = "OrderID";
            this.GridAlias = "ThreadRequisition_Detail";
            this.GridUniqueKey = "refno,threadcolorid";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "OrderID";
            this.Name = "P21";
            this.Text = "P21.Thread Requisition";
            this.UnApvChkValue = "Approved";
            this.WorkAlias = "ThreadRequisition";
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).EndInit();
            this.masterpanel.ResumeLayout(false);
            this.masterpanel.PerformLayout();
            this.detailpanel.ResumeLayout(false);
            this.detail2.ResumeLayout(false);
            this.detailpanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DisplayBox displayBox4;
        private Win.UI.DateBox dateBox4;
        private Win.UI.DateBox dateBox3;
        private Win.UI.DisplayBox displayBox3;
        private Win.UI.DisplayBox displayBox2;
        private Win.UI.DateBox dateBox2;
        private Win.UI.DateBox dateBox1;
        private Win.UI.Label label7;
        private Win.UI.Label label8;
        private Win.UI.Label label9;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.TextBox textBox1;
        private Win.UI.Label label1;
    }
}
