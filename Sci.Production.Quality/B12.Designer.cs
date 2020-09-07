namespace Sci.Production.Quality
{
    partial class B12
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
            this.txtDefectCode = new Sci.Win.UI.TextBox();
            this.lbDefectCode = new Sci.Win.UI.Label();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.txtSubProcessID = new Sci.Win.UI.TextBox();
            this.lbSubProcessID = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(829, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtDefectCode);
            this.detailcont.Controls.Add(this.lbDefectCode);
            this.detailcont.Controls.Add(this.editDescription);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.txtSubProcessID);
            this.detailcont.Controls.Add(this.lbSubProcessID);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Size = new System.Drawing.Size(829, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(829, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(829, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(837, 424);
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
            // txtDefectCode
            // 
            this.txtDefectCode.BackColor = System.Drawing.Color.White;
            this.txtDefectCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DefectCode", true));
            this.txtDefectCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDefectCode.Location = new System.Drawing.Point(128, 63);
            this.txtDefectCode.MaxLength = 50;
            this.txtDefectCode.Name = "txtDefectCode";
            this.txtDefectCode.Size = new System.Drawing.Size(665, 23);
            this.txtDefectCode.TabIndex = 14;
            // 
            // lbDefectCode
            // 
            this.lbDefectCode.Location = new System.Drawing.Point(36, 63);
            this.lbDefectCode.Name = "lbDefectCode";
            this.lbDefectCode.Size = new System.Drawing.Size(89, 23);
            this.lbDefectCode.TabIndex = 13;
            this.lbDefectCode.Text = "DefectCode";
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(128, 92);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(665, 50);
            this.editDescription.TabIndex = 11;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(434, 36);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 9;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // txtSubProcessID
            // 
            this.txtSubProcessID.BackColor = System.Drawing.Color.White;
            this.txtSubProcessID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SubProcessID", true));
            this.txtSubProcessID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSubProcessID.Location = new System.Drawing.Point(128, 34);
            this.txtSubProcessID.MaxLength = 10;
            this.txtSubProcessID.Name = "txtSubProcessID";
            this.txtSubProcessID.Size = new System.Drawing.Size(292, 23);
            this.txtSubProcessID.TabIndex = 8;
            // 
            // lbSubProcessID
            // 
            this.lbSubProcessID.Location = new System.Drawing.Point(36, 34);
            this.lbSubProcessID.Name = "lbSubProcessID";
            this.lbSubProcessID.Size = new System.Drawing.Size(89, 23);
            this.lbSubProcessID.TabIndex = 10;
            this.lbSubProcessID.Text = "SubProcess";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(36, 92);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(89, 23);
            this.labelDescription.TabIndex = 12;
            this.labelDescription.Text = "Description";
            // 
            // B12
            // 
            this.ClientSize = new System.Drawing.Size(837, 457);
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportLocate = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B12";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B12. Sub-Process Defect Code";
            this.WorkAlias = "SubProDefectCode";
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

        private Win.UI.TextBox txtDefectCode;
        private Win.UI.Label lbDefectCode;
        private Win.UI.EditBox editDescription;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.TextBox txtSubProcessID;
        private Win.UI.Label lbSubProcessID;
        private Win.UI.Label labelDescription;
    }
}
