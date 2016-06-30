namespace Sci.Production.Quality
{
    partial class B01
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
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.textBox3 = new Sci.Win.UI.TextBox();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.editBox1 = new Sci.Win.UI.EditBox();
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
            this.detail.Size = new System.Drawing.Size(897, 428);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.editBox1);
            this.detailcont.Controls.Add(this.checkBox1);
            this.detailcont.Controls.Add(this.textBox3);
            this.detailcont.Controls.Add(this.textBox1);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Size = new System.Drawing.Size(897, 390);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 390);
            // 
            // tabs
            // 
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Size = new System.Drawing.Size(905, 457);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(53, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Code";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(53, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "Description";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(53, 210);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 6;
            this.label3.Text = "type";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(171, 46);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 23);
            this.textBox1.TabIndex = 7;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.White;
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Type", true));
            this.textBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox3.Location = new System.Drawing.Point(171, 210);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 23);
            this.textBox3.TabIndex = 9;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(311, 46);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(57, 21);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "Junk";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.White;
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescriptionEN", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox1.Location = new System.Drawing.Point(171, 122);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(197, 50);
            this.editBox1.TabIndex = 11;
            // 
            // B01
            // 
            this.ClientSize = new System.Drawing.Size(905, 457);
            this.IsDeleteOnBrowse = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportLocate = false;
            this.IsSupportMove = false;
            this.IsSupportPrint = false;
            this.Name = "B01";
            this.Text = "B01. Defect Code For Fabric Inspection";
            this.WorkAlias = "FabricDefect";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.CheckBox checkBox1;
        private Win.UI.TextBox textBox3;
        private Win.UI.TextBox textBox1;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.EditBox editBox1;
    }
}
