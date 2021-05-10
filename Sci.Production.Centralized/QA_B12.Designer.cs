namespace Sci.Production.Centralized
{
    partial class QA_B12
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
            this.lbSubProcessID = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.lbDefectCode = new Sci.Win.UI.Label();
            this.txtDefectCode = new Sci.Win.UI.TextBox();
            this.txtSubProcess = new Sci.Win.UI.TextBox();
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
            this.detailcont.Controls.Add(this.txtSubProcess);
            this.detailcont.Controls.Add(this.txtDefectCode);
            this.detailcont.Controls.Add(this.lbDefectCode);
            this.detailcont.Controls.Add(this.editDescription);
            this.detailcont.Controls.Add(this.checkJunk);
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
            // lbSubProcessID
            // 
            this.lbSubProcessID.Location = new System.Drawing.Point(36, 33);
            this.lbSubProcessID.Name = "lbSubProcessID";
            this.lbSubProcessID.Size = new System.Drawing.Size(89, 23);
            this.lbSubProcessID.TabIndex = 4;
            this.lbSubProcessID.Text = "SubProcess";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(36, 91);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(89, 23);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "Description";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(434, 35);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 4;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(128, 91);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(665, 50);
            this.editDescription.TabIndex = 5;
            // 
            // lbDefectCode
            // 
            this.lbDefectCode.Location = new System.Drawing.Point(36, 62);
            this.lbDefectCode.Name = "lbDefectCode";
            this.lbDefectCode.Size = new System.Drawing.Size(89, 23);
            this.lbDefectCode.TabIndex = 6;
            this.lbDefectCode.Text = "DefectCode";
            // 
            // txtDefectCode
            // 
            this.txtDefectCode.BackColor = System.Drawing.Color.White;
            this.txtDefectCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DefectCode", true));
            this.txtDefectCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDefectCode.Location = new System.Drawing.Point(128, 62);
            this.txtDefectCode.MaxLength = 50;
            this.txtDefectCode.Name = "txtDefectCode";
            this.txtDefectCode.Size = new System.Drawing.Size(665, 23);
            this.txtDefectCode.TabIndex = 7;
            // 
            // txtSubProcess
            // 
            this.txtSubProcess.BackColor = System.Drawing.Color.White;
            this.txtSubProcess.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SubProcessID", true));
            this.txtSubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSubProcess.Location = new System.Drawing.Point(128, 33);
            this.txtSubProcess.MaxLength = 15;
            this.txtSubProcess.Name = "txtSubProcess";
            this.txtSubProcess.Size = new System.Drawing.Size(292, 23);
            this.txtSubProcess.TabIndex = 9;
            this.txtSubProcess.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSubProcess_PopUp);
            // 
            // QA_B12
            // 
            this.ClientSize = new System.Drawing.Size(837, 457);
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportDelete = false;
            this.IsSupportLocate = false;
            this.IsSupportPrint = false;
            this.Name = "QA_B12";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "QA_B12. Sub-Process Defect Code";
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

        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label lbSubProcessID;
        private Win.UI.Label labelDescription;
        private Win.UI.EditBox editDescription;
        private Win.UI.TextBox txtDefectCode;
        private Win.UI.Label lbDefectCode;
        private Win.UI.TextBox txtSubProcess;
    }
}
