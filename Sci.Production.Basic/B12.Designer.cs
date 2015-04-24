namespace Sci.Production.Basic
{
    partial class B12
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
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.label7 = new Sci.Win.UI.Label();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.displayBox2 = new Sci.Win.UI.DisplayBox();
            this.displayBox3 = new Sci.Win.UI.DisplayBox();
            this.displayBox4 = new Sci.Win.UI.DisplayBox();
            this.displayBox5 = new Sci.Win.UI.DisplayBox();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            this.masterpanel.SuspendLayout();
            this.detailpanel.SuspendLayout();
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
            this.masterpanel.Controls.Add(this.displayBox5);
            this.masterpanel.Controls.Add(this.displayBox4);
            this.masterpanel.Controls.Add(this.displayBox3);
            this.masterpanel.Controls.Add(this.displayBox2);
            this.masterpanel.Controls.Add(this.displayBox1);
            this.masterpanel.Controls.Add(this.label7);
            this.masterpanel.Controls.Add(this.checkBox1);
            this.masterpanel.Controls.Add(this.label6);
            this.masterpanel.Controls.Add(this.label5);
            this.masterpanel.Controls.Add(this.label4);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Size = new System.Drawing.Size(754, 134);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.label4, 0);
            this.masterpanel.Controls.SetChildIndex(this.label5, 0);
            this.masterpanel.Controls.SetChildIndex(this.label6, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkBox1, 0);
            this.masterpanel.Controls.SetChildIndex(this.label7, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBox1, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBox2, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBox3, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBox4, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBox5, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 134);
            this.detailpanel.Size = new System.Drawing.Size(754, 215);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(653, 95);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(674, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(754, 215);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(754, 387);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(754, 349);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(754, 38);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(762, 416);
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(24, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "Unit";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(24, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 2;
            this.label4.Text = "Price rate";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(24, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 3;
            this.label5.Text = "Round";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(24, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 23);
            this.label6.TabIndex = 4;
            this.label6.Text = "Description";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "Junk", true));
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(413, 14);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(57, 21);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Junk";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(413, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 23);
            this.label7.TabIndex = 6;
            this.label7.Text = "Extend Unit";
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(103, 14);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(70, 23);
            this.displayBox1.TabIndex = 7;
            // 
            // displayBox2
            // 
            this.displayBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PriceRate", true));
            this.displayBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox2.Location = new System.Drawing.Point(103, 44);
            this.displayBox2.Name = "displayBox2";
            this.displayBox2.Size = new System.Drawing.Size(50, 23);
            this.displayBox2.TabIndex = 8;
            // 
            // displayBox3
            // 
            this.displayBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Round", true));
            this.displayBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox3.Location = new System.Drawing.Point(103, 74);
            this.displayBox3.Name = "displayBox3";
            this.displayBox3.Size = new System.Drawing.Size(30, 23);
            this.displayBox3.TabIndex = 9;
            // 
            // displayBox4
            // 
            this.displayBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox4.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Description", true));
            this.displayBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox4.Location = new System.Drawing.Point(103, 104);
            this.displayBox4.Name = "displayBox4";
            this.displayBox4.Size = new System.Drawing.Size(316, 23);
            this.displayBox4.TabIndex = 10;
            // 
            // displayBox5
            // 
            this.displayBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox5.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ExtensionUnit", true));
            this.displayBox5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox5.Location = new System.Drawing.Point(498, 44);
            this.displayBox5.Name = "displayBox5";
            this.displayBox5.Size = new System.Drawing.Size(70, 23);
            this.displayBox5.TabIndex = 11;
            // 
            // B12
            // 
            this.ClientSize = new System.Drawing.Size(762, 449);
            this.DefaultOrder = "ID";
            this.DefaultOrder2 = "UnitTo";
            this.GridAlias = "Unit_Rate";
            this.IsGridIconVisible = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "ID";
            this.KeyField2 = "UnitFrom";
            this.Name = "B12";
            this.UniqueExpress = "ID";
            this.WorkAlias = "Unit";
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            this.masterpanel.ResumeLayout(false);
            this.masterpanel.PerformLayout();
            this.detailpanel.ResumeLayout(false);
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

        private Win.UI.DisplayBox displayBox5;
        private Win.UI.DisplayBox displayBox4;
        private Win.UI.DisplayBox displayBox3;
        private Win.UI.DisplayBox displayBox2;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.Label label7;
        private Win.UI.CheckBox checkBox1;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
    }
}
