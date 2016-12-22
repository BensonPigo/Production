namespace Sci.Production.Basic
{
    partial class B13
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
            this.displayBox2 = new Sci.Win.UI.DisplayBox();
            this.displayBox3 = new Sci.Win.UI.DisplayBox();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.checkBox2 = new Sci.Win.UI.CheckBox();
            this.checkBox3 = new Sci.Win.UI.CheckBox();
            this.checkBox4 = new Sci.Win.UI.CheckBox();
            this.label6 = new Sci.Win.UI.Label();
            this.txtdropdownlist2 = new Sci.Production.Class.txtdropdownlist();
            this.checkBox5 = new Sci.Win.UI.CheckBox();
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
            this.detail.Size = new System.Drawing.Size(688, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.checkBox5);
            this.detailcont.Controls.Add(this.txtdropdownlist2);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.checkBox4);
            this.detailcont.Controls.Add(this.checkBox3);
            this.detailcont.Controls.Add(this.checkBox2);
            this.detailcont.Controls.Add(this.checkBox1);
            this.detailcont.Controls.Add(this.comboBox1);
            this.detailcont.Controls.Add(this.displayBox3);
            this.detailcont.Controls.Add(this.displayBox2);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Size = new System.Drawing.Size(688, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(688, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(688, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(696, 424);
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(70, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "Material Type";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(70, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "Artwork";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(70, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 23);
            this.label5.TabIndex = 2;
            this.label5.Text = "Production";
            // 
            // displayBox2
            // 
            this.displayBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox2.Location = new System.Drawing.Point(169, 70);
            this.displayBox2.Name = "displayBox2";
            this.displayBox2.Size = new System.Drawing.Size(170, 23);
            this.displayBox2.TabIndex = 4;
            // 
            // displayBox3
            // 
            this.displayBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ProductionType", true));
            this.displayBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox3.Location = new System.Drawing.Point(169, 110);
            this.displayBox3.Name = "displayBox3";
            this.displayBox3.Size = new System.Drawing.Size(100, 23);
            this.displayBox3.TabIndex = 5;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Type", true));
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(169, 30);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.ReadOnly = true;
            this.comboBox1.Size = new System.Drawing.Size(100, 24);
            this.comboBox1.TabIndex = 6;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(400, 30);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(57, 21);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Junk";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsExtensionUnit", true));
            this.checkBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox2.Location = new System.Drawing.Point(400, 70);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(70, 21);
            this.checkBox2.TabIndex = 8;
            this.checkBox2.Text = "Extend";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CheckZipper", true));
            this.checkBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox3.Location = new System.Drawing.Point(400, 110);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(111, 21);
            this.checkBox3.TabIndex = 9;
            this.checkBox3.Text = "Check Zipper";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IrregularCost", true));
            this.checkBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox4.Location = new System.Drawing.Point(400, 150);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(93, 21);
            this.checkBox4.TabIndex = 10;
            this.checkBox4.Text = "is ICR Item";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(70, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 23);
            this.label6.TabIndex = 11;
            this.label6.Text = "Issue Type";
            // 
            // txtdropdownlist2
            // 
            this.txtdropdownlist2.BackColor = System.Drawing.Color.White;
            this.txtdropdownlist2.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "IssueType", true));
            this.txtdropdownlist2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlist2.FormattingEnabled = true;
            this.txtdropdownlist2.IsSupportUnselect = true;
            this.txtdropdownlist2.Location = new System.Drawing.Point(169, 150);
            this.txtdropdownlist2.Name = "txtdropdownlist2";
            this.txtdropdownlist2.Size = new System.Drawing.Size(121, 24);
            this.txtdropdownlist2.TabIndex = 13;
            this.txtdropdownlist2.Type = "IssueType";
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsTrimCardOther", true));
            this.checkBox5.ForeColor = System.Drawing.Color.Red;
            this.checkBox5.Location = new System.Drawing.Point(400, 191);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.ReadOnly = true;
            this.checkBox5.Size = new System.Drawing.Size(131, 21);
            this.checkBox5.TabIndex = 14;
            this.checkBox5.Text = "IsTrimCardOther";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // B13
            // 
            this.ClientSize = new System.Drawing.Size(696, 457);
            this.DefaultOrder = "ID";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B13";
            this.Text = "B13. Material Type";
            this.UniqueExpress = "ID";
            this.WorkAlias = "MtlType";
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

        private Win.UI.ComboBox comboBox1;
        private Win.UI.DisplayBox displayBox3;
        private Win.UI.DisplayBox displayBox2;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.CheckBox checkBox4;
        private Win.UI.CheckBox checkBox3;
        private Win.UI.CheckBox checkBox2;
        private Win.UI.CheckBox checkBox1;
        private Win.UI.Label label6;
        private Class.txtdropdownlist txtdropdownlist2;
        private Win.UI.CheckBox checkBox5;
    }
}
