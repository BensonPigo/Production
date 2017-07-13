namespace Sci.Production.Centralized
{
    partial class WH_B01
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
            this.labelID = new Sci.Win.UI.Label();
            this.labelDesc = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.txtID = new Sci.Win.UI.TextBox();
            this.txtDesc = new Sci.Win.UI.TextBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
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
            this.detailcont.Controls.Add(this.txtRemark);
            this.detailcont.Controls.Add(this.txtDesc);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.labelID);
            this.detailcont.Controls.Add(this.labelDesc);
            this.detailcont.Controls.Add(this.labelRemark);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(900, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(908, 424);
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
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(24, 12);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 4;
            this.labelID.Text = "ID";
            // 
            // labelDesc
            // 
            this.labelDesc.Location = new System.Drawing.Point(24, 47);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(75, 23);
            this.labelDesc.TabIndex = 5;
            this.labelDesc.Text = "Desc";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(24, 82);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 6;
            this.labelRemark.Text = "Remark";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(423, 14);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 1;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "id", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtID.IsSupportEditMode = false;
            this.txtID.Location = new System.Drawing.Point(102, 12);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(48, 23);
            this.txtID.TabIndex = 0;
            // 
            // txtDesc
            // 
            this.txtDesc.BackColor = System.Drawing.Color.White;
            this.txtDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "description", true));
            this.txtDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDesc.Location = new System.Drawing.Point(102, 47);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(618, 23);
            this.txtDesc.TabIndex = 2;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(102, 82);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(378, 23);
            this.txtRemark.TabIndex = 3;
            // 
            // WH_B01
            // 
            this.ClientSize = new System.Drawing.Size(908, 457);
            this.ConnectionName = "Tradedb";
            this.DefaultControl = "txtID";
            this.DefaultControlForEdit = "checkJunk";
            this.DefaultFilter = "TYPE=\'IR\'";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "WH_B01";
            this.Text = "WH_B01. Issue R/Mtl Reason";
            this.WorkAlias = "WhseReason";
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

        private Win.UI.TextBox txtRemark;
        private Win.UI.TextBox txtDesc;
        private Win.UI.TextBox txtID;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label labelID;
        private Win.UI.Label labelDesc;
        private Win.UI.Label labelRemark;
    }
}
