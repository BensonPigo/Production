namespace Sci.Production.Centralized
{
    partial class Cutting_B04
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
            this.editRemark = new Sci.Win.UI.EditBox();
            this.labelRemark = new Sci.Win.UI.Label();
            this.txtDescription = new Sci.Win.UI.TextBox();
            this.labelDescription = new Sci.Win.UI.Label();
            this.txtID = new Sci.Win.UI.TextBox();
            this.labelID = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(828, 400);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.editRemark);
            this.detailcont.Controls.Add(this.labelRemark);
            this.detailcont.Controls.Add(this.txtDescription);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.labelID);
            this.detailcont.Size = new System.Drawing.Size(828, 362);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 362);
            this.detailbtm.Size = new System.Drawing.Size(828, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(828, 400);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(836, 429);
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
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(233, 18);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 14;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(116, 114);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(473, 70);
            this.editRemark.TabIndex = 13;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(21, 114);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(88, 23);
            this.labelRemark.TabIndex = 12;
            this.labelRemark.Text = "Remark";
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDescription.Location = new System.Drawing.Point(116, 66);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(379, 23);
            this.txtDescription.TabIndex = 11;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(21, 66);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(89, 23);
            this.labelDescription.TabIndex = 10;
            this.labelDescription.Text = "Description";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "id", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtID.IsSupportEditMode = false;
            this.txtID.Location = new System.Drawing.Point(116, 18);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(100, 23);
            this.txtID.TabIndex = 9;
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(21, 18);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(88, 23);
            this.labelID.TabIndex = 8;
            this.labelID.Text = "ID";
            // 
            // Cutting_B04
            // 
            this.ClientSize = new System.Drawing.Size(836, 462);
            this.ConnectionName = "Tradedb";
            this.DefaultFilter = "Type=\'RC\'";
            this.DefaultOrder = "ID";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "Cutting_B04";
            this.Text = "Cutting_B04.CutReason";
            this.WorkAlias = "CutReason";
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
        private Win.UI.EditBox editRemark;
        private Win.UI.Label labelRemark;
        private Win.UI.TextBox txtDescription;
        private Win.UI.Label labelDescription;
        private Win.UI.TextBox txtID;
        private Win.UI.Label labelID;
    }
}
