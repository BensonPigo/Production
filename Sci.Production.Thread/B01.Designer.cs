namespace Sci.Production.Thread
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
            this.labelThreadCombination = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.txtThreadCombination = new Sci.Win.UI.TextBox();
            this.txtDescription = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtDescription);
            this.detailcont.Controls.Add(this.txtThreadCombination);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelThreadCombination);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(484, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(436, 13);
            // 
            // labelThreadCombination
            // 
            this.labelThreadCombination.Lines = 0;
            this.labelThreadCombination.Location = new System.Drawing.Point(50, 46);
            this.labelThreadCombination.Name = "labelThreadCombination";
            this.labelThreadCombination.Size = new System.Drawing.Size(134, 23);
            this.labelThreadCombination.TabIndex = 3;
            this.labelThreadCombination.Text = "Thread Combination";
            // 
            // labelDescription
            // 
            this.labelDescription.Lines = 0;
            this.labelDescription.Location = new System.Drawing.Point(50, 103);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(134, 23);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Description";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(411, 46);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 2;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // txtThreadCombination
            // 
            this.txtThreadCombination.BackColor = System.Drawing.Color.White;
            this.txtThreadCombination.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtThreadCombination.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtThreadCombination.Location = new System.Drawing.Point(190, 46);
            this.txtThreadCombination.Name = "txtThreadCombination";
            this.txtThreadCombination.Size = new System.Drawing.Size(100, 23);
            this.txtThreadCombination.TabIndex = 0;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "description", true));
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDescription.Location = new System.Drawing.Point(190, 103);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(249, 23);
            this.txtDescription.TabIndex = 1;
            // 
            // B01
            // 
            this.ClientSize = new System.Drawing.Size(905, 457);
            this.DefaultControl = "textBox1";
            this.DefaultControlForEdit = "textBox2";
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B01";
            this.Text = "B01.Thread Combination";
            this.WorkAlias = "ThreadComb";
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

        private Win.UI.Label labelDescription;
        private Win.UI.Label labelThreadCombination;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.TextBox txtDescription;
        private Win.UI.TextBox txtThreadCombination;
    }
}
