namespace Sci.Production.Thread
{
    partial class B02
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
            this.labelThreadColor = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.txtThreadColor = new Sci.Win.UI.TextBox();
            this.txtDescription = new Sci.Win.UI.TextBox();
            this.txtThreadColorGroupID = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
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
            this.detailcont.Controls.Add(this.txtThreadColorGroupID);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.txtDescription);
            this.detailcont.Controls.Add(this.txtThreadColor);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelThreadColor);
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
            // labelThreadColor
            // 
            this.labelThreadColor.Location = new System.Drawing.Point(50, 46);
            this.labelThreadColor.Name = "labelThreadColor";
            this.labelThreadColor.Size = new System.Drawing.Size(131, 23);
            this.labelThreadColor.TabIndex = 3;
            this.labelThreadColor.Text = "Thread Color";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(50, 119);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(131, 23);
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
            // txtThreadColor
            // 
            this.txtThreadColor.BackColor = System.Drawing.Color.White;
            this.txtThreadColor.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtThreadColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtThreadColor.Location = new System.Drawing.Point(184, 46);
            this.txtThreadColor.Name = "txtThreadColor";
            this.txtThreadColor.Size = new System.Drawing.Size(100, 23);
            this.txtThreadColor.TabIndex = 0;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "description", true));
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDescription.Location = new System.Drawing.Point(184, 119);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(249, 23);
            this.txtDescription.TabIndex = 1;
            // 
            // txtThreadColorGroupID
            // 
            this.txtThreadColorGroupID.BackColor = System.Drawing.Color.White;
            this.txtThreadColorGroupID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ThreadColorGroupID", true));
            this.txtThreadColorGroupID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtThreadColorGroupID.Location = new System.Drawing.Point(184, 82);
            this.txtThreadColorGroupID.Name = "txtThreadColorGroupID";
            this.txtThreadColorGroupID.Size = new System.Drawing.Size(219, 23);
            this.txtThreadColorGroupID.TabIndex = 5;
            this.txtThreadColorGroupID.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtThreadColorGroupID_PopUp);
            this.txtThreadColorGroupID.Validating += new System.ComponentModel.CancelEventHandler(this.TxtThreadColorGroupID_Validating);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(50, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Thread Color Group";
            // 
            // B02
            // 
            this.ClientSize = new System.Drawing.Size(905, 457);
            this.DefaultControl = "txtThreadColor";
            this.DefaultControlForEdit = "txtDescription";
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B02";
            this.Text = "B02.Color Index Of Thread";
            this.WorkAlias = "ThreadColor";
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
        private Win.UI.Label labelThreadColor;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.TextBox txtDescription;
        private Win.UI.TextBox txtThreadColor;
        private Win.UI.TextBox txtThreadColorGroupID;
        private Win.UI.Label label1;
    }
}
