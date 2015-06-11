namespace Sci.Production.IE
{
    partial class B07
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
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.editBox1 = new Sci.Win.UI.EditBox();
            this.editBox2 = new Sci.Win.UI.EditBox();
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
            this.detail.Size = new System.Drawing.Size(680, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.checkBox1);
            this.detailcont.Controls.Add(this.editBox2);
            this.detailcont.Controls.Add(this.editBox1);
            this.detailcont.Controls.Add(this.comboBox1);
            this.detailcont.Controls.Add(this.displayBox1);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Size = new System.Drawing.Size(680, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(680, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(680, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(688, 424);
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(42, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "ID";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(42, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "Type";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(42, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 46);
            this.label5.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(42, 177);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 46);
            this.label6.TabIndex = 3;
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(121, 33);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(138, 23);
            this.displayBox1.TabIndex = 4;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Type", true));
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(121, 73);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 5;
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.White;
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescEN", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox1.Location = new System.Drawing.Point(121, 113);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(456, 50);
            this.editBox1.TabIndex = 8;
            // 
            // editBox2
            // 
            this.editBox2.BackColor = System.Drawing.Color.White;
            this.editBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescCH", true));
            this.editBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox2.Location = new System.Drawing.Point(121, 177);
            this.editBox2.Multiline = true;
            this.editBox2.Name = "editBox2";
            this.editBox2.Size = new System.Drawing.Size(456, 50);
            this.editBox2.TabIndex = 9;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(468, 33);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(57, 21);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "Junk";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // B07
            // 
            this.ClientSize = new System.Drawing.Size(688, 457);
            this.DefaultOrder = "ID";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B07";
            this.Text = "B07. Attachment List";
            this.UniqueExpress = "ID";
            this.WorkAlias = "Mold";
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

        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.EditBox editBox2;
        private Win.UI.EditBox editBox1;
        private Win.UI.ComboBox comboBox1;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.CheckBox checkBox1;
    }
}
