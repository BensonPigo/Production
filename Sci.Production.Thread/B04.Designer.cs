namespace Sci.Production.Thread
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
            this.labelThreadLocation = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.txtThreadLocation = new Sci.Win.UI.TextBox();
            this.txtDescription = new Sci.Win.UI.TextBox();
            this.checkAllowAutoAllocate = new Sci.Win.UI.CheckBox();
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
            this.detailcont.Controls.Add(this.checkAllowAutoAllocate);
            this.detailcont.Controls.Add(this.txtDescription);
            this.detailcont.Controls.Add(this.txtThreadLocation);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelThreadLocation);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelThreadLocation
            // 
            this.labelThreadLocation.Location = new System.Drawing.Point(50, 46);
            this.labelThreadLocation.Name = "labelThreadLocation";
            this.labelThreadLocation.Size = new System.Drawing.Size(104, 23);
            this.labelThreadLocation.TabIndex = 3;
            this.labelThreadLocation.Text = "Thread Location";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(50, 103);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(104, 23);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Description";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(412, 46);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 2;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // txtThreadLocation
            // 
            this.txtThreadLocation.BackColor = System.Drawing.Color.White;
            this.txtThreadLocation.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtThreadLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtThreadLocation.Location = new System.Drawing.Point(157, 46);
            this.txtThreadLocation.Name = "txtThreadLocation";
            this.txtThreadLocation.Size = new System.Drawing.Size(100, 23);
            this.txtThreadLocation.TabIndex = 0;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "description", true));
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDescription.Location = new System.Drawing.Point(157, 103);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(249, 23);
            this.txtDescription.TabIndex = 1;
            // 
            // checkAllowAutoAllocate
            // 
            this.checkAllowAutoAllocate.AutoSize = true;
            this.checkAllowAutoAllocate.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "AllowAutoAllocate", true));
            this.checkAllowAutoAllocate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkAllowAutoAllocate.Location = new System.Drawing.Point(412, 105);
            this.checkAllowAutoAllocate.Name = "checkAllowAutoAllocate";
            this.checkAllowAutoAllocate.Size = new System.Drawing.Size(138, 21);
            this.checkAllowAutoAllocate.TabIndex = 5;
            this.checkAllowAutoAllocate.Text = "Allow Auto Allocate";
            this.checkAllowAutoAllocate.UseVisualStyleBackColor = true;
            // 
            // B04
            // 
            this.ClientSize = new System.Drawing.Size(905, 457);
            this.DefaultControl = "txtThreadLocation";
            this.DefaultControlForEdit = "txtDescription";
            this.DefaultOrder = "id";
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B04";
            this.Text = "B04.Thread Stock Location";
            this.WorkAlias = "ThreadLocation";
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
        private Win.UI.Label labelThreadLocation;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.TextBox txtDescription;
        private Win.UI.TextBox txtThreadLocation;
        private Win.UI.CheckBox checkAllowAutoAllocate;
    }
}
