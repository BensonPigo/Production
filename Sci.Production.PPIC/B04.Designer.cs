namespace Sci.Production.PPIC
{
    partial class B04
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
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
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
            this.detail.Size = new System.Drawing.Size(681, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.checkBox1);
            this.detailcont.Controls.Add(this.editBox2);
            this.detailcont.Controls.Add(this.editBox1);
            this.detailcont.Controls.Add(this.displayBox1);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Size = new System.Drawing.Size(681, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(681, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(681, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(689, 424);
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(29, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "ID";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(29, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "Description";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(29, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 3;
            this.label5.Text = "Remark";
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(108, 31);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(50, 23);
            this.displayBox1.TabIndex = 4;
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.White;
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Name", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox1.Location = new System.Drawing.Point(108, 70);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(394, 50);
            this.editBox1.TabIndex = 5;
            // 
            // editBox2
            // 
            this.editBox2.BackColor = System.Drawing.Color.White;
            this.editBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox2.Location = new System.Drawing.Point(108, 138);
            this.editBox2.Multiline = true;
            this.editBox2.Name = "editBox2";
            this.editBox2.Size = new System.Drawing.Size(394, 50);
            this.editBox2.TabIndex = 6;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(352, 31);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(57, 21);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Junk";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // B04
            // 
            this.ClientSize = new System.Drawing.Size(689, 457);
            this.DefaultFilter = "ReasonTypeID = \'Damage Reason\'";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B04";
            this.Text = "B04. Damage Reason";
            this.UniqueExpress = "ReasonTypeID,ID";
            this.WorkAlias = "Reason";
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
        private Win.UI.EditBox editBox2;
        private Win.UI.EditBox editBox1;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
    }
}
