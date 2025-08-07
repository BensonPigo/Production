namespace Sci.Production.Centralized
{
    partial class IE_B91
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
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.disID = new Sci.Win.UI.DisplayBox();
            this.label1 = new Sci.Win.UI.Label();
            this.lbName = new Sci.Win.UI.Label();
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
            this.detailcont.Controls.Add(this.editDescription);
            this.detailcont.Controls.Add(this.disID);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.lbName);
            this.detailcont.Controls.Add(this.checkJunk);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(829, 274);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(837, 303);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(644, 28);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 28;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(157, 60);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(497, 128);
            this.editDescription.TabIndex = 40;
            // 
            // disID
            // 
            this.disID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.disID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disID.Location = new System.Drawing.Point(157, 28);
            this.disID.Name = "disID";
            this.disID.Size = new System.Drawing.Size(100, 23);
            this.disID.TabIndex = 39;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(70, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 23);
            this.label1.TabIndex = 38;
            this.label1.Text = "ID";
            // 
            // lbName
            // 
            this.lbName.Location = new System.Drawing.Point(70, 60);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(84, 23);
            this.lbName.TabIndex = 37;
            this.lbName.Text = "Description";
            // 
            // IE_B91
            // 
            this.ClientSize = new System.Drawing.Size(837, 336);
            this.ConnectionName = "ProductionTPE";
            this.DefaultOrder = "ID";
            this.DefaultWhere = "Type=\'LB\'";
            this.EnableGridJunkColor = true;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "IE_B91";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "IE_B91. IE Reason LBR Not Hit 1st";
            this.UniqueExpress = "ID";
            this.WorkAlias = "IEReason";
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
        private Win.UI.CheckBox checkJunk;
        private Win.UI.EditBox editDescription;
        private Win.UI.DisplayBox disID;
        private Win.UI.Label label1;
        private Win.UI.Label lbName;
    }
}
