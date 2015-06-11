namespace Sci.Production.IE
{
    partial class B05
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
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.displayBox2 = new Sci.Win.UI.DisplayBox();
            this.displayBox3 = new Sci.Win.UI.DisplayBox();
            this.displayBox4 = new Sci.Win.UI.DisplayBox();
            this.numericBox1 = new Sci.Win.UI.NumericBox();
            this.numericBox2 = new Sci.Win.UI.NumericBox();
            this.label9 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(686, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.checkBox1);
            this.detailcont.Controls.Add(this.label10);
            this.detailcont.Controls.Add(this.label9);
            this.detailcont.Controls.Add(this.numericBox2);
            this.detailcont.Controls.Add(this.numericBox1);
            this.detailcont.Controls.Add(this.displayBox4);
            this.detailcont.Controls.Add(this.displayBox3);
            this.detailcont.Controls.Add(this.displayBox2);
            this.detailcont.Controls.Add(this.displayBox1);
            this.detailcont.Controls.Add(this.label8);
            this.detailcont.Controls.Add(this.label7);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Size = new System.Drawing.Size(686, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(686, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(686, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(694, 424);
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(30, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "Code";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(30, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "Description";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(30, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 23);
            this.label5.TabIndex = 2;
            this.label5.Text = "Group ID";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(30, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 23);
            this.label6.TabIndex = 3;
            this.label6.Text = "Artwork Type";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(30, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(125, 23);
            this.label7.TabIndex = 4;
            this.label7.Text = "Machine Allowance";
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(30, 205);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(125, 23);
            this.label8.TabIndex = 5;
            this.label8.Text = "Manual Allowance";
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(159, 30);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(80, 23);
            this.displayBox1.TabIndex = 6;
            // 
            // displayBox2
            // 
            this.displayBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Description", true));
            this.displayBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox2.Location = new System.Drawing.Point(159, 65);
            this.displayBox2.Name = "displayBox2";
            this.displayBox2.Size = new System.Drawing.Size(380, 23);
            this.displayBox2.TabIndex = 7;
            // 
            // displayBox3
            // 
            this.displayBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MachineGroupID", true));
            this.displayBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox3.Location = new System.Drawing.Point(159, 100);
            this.displayBox3.Name = "displayBox3";
            this.displayBox3.Size = new System.Drawing.Size(35, 23);
            this.displayBox3.TabIndex = 8;
            // 
            // displayBox4
            // 
            this.displayBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox4.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ArtworkTypeID", true));
            this.displayBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox4.Location = new System.Drawing.Point(159, 135);
            this.displayBox4.Name = "displayBox4";
            this.displayBox4.Size = new System.Drawing.Size(140, 23);
            this.displayBox4.TabIndex = 9;
            // 
            // numericBox1
            // 
            this.numericBox1.BackColor = System.Drawing.Color.White;
            this.numericBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MachineAllow", true));
            this.numericBox1.DecimalPlaces = 2;
            this.numericBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox1.Location = new System.Drawing.Point(159, 170);
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.Size = new System.Drawing.Size(50, 23);
            this.numericBox1.TabIndex = 10;
            // 
            // numericBox2
            // 
            this.numericBox2.BackColor = System.Drawing.Color.White;
            this.numericBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ManAllow", true));
            this.numericBox2.DecimalPlaces = 2;
            this.numericBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox2.Location = new System.Drawing.Point(159, 205);
            this.numericBox2.Name = "numericBox2";
            this.numericBox2.Size = new System.Drawing.Size(50, 23);
            this.numericBox2.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(212, 170);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 23);
            this.label9.TabIndex = 12;
            this.label9.Text = "%";
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(212, 205);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 23);
            this.label10.TabIndex = 13;
            this.label10.Text = "%";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(432, 30);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(57, 21);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "Junk";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // B05
            // 
            this.ClientSize = new System.Drawing.Size(694, 457);
            this.DefaultOrder = "ID";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B05";
            this.Text = "B05. Machine Type";
            this.UniqueExpress = "ID";
            this.WorkAlias = "MachineType";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.CheckBox checkBox1;
        private Win.UI.Label label10;
        private Win.UI.Label label9;
        private Win.UI.NumericBox numericBox2;
        private Win.UI.NumericBox numericBox1;
        private Win.UI.DisplayBox displayBox4;
        private Win.UI.DisplayBox displayBox3;
        private Win.UI.DisplayBox displayBox2;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.Label label8;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
    }
}
