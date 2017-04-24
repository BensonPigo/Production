namespace Sci.Production.Quality
{
    partial class B06
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
            this.labelAreaCode = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.txtAreaCode = new Sci.Win.UI.TextBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.editDescription = new Sci.Win.UI.EditBox();
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
            this.detailcont.Controls.Add(this.editDescription);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.txtAreaCode);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelAreaCode);
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
            // labelAreaCode
            // 
            this.labelAreaCode.Lines = 0;
            this.labelAreaCode.Location = new System.Drawing.Point(44, 50);
            this.labelAreaCode.Name = "labelAreaCode";
            this.labelAreaCode.Size = new System.Drawing.Size(89, 23);
            this.labelAreaCode.TabIndex = 0;
            this.labelAreaCode.Text = "Area Code";
            // 
            // labelDescription
            // 
            this.labelDescription.Lines = 0;
            this.labelDescription.Location = new System.Drawing.Point(44, 81);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(89, 23);
            this.labelDescription.TabIndex = 1;
            this.labelDescription.Text = "Description";
            // 
            // txtAreaCode
            // 
            this.txtAreaCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtAreaCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "id", true));
            this.txtAreaCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtAreaCode.IsSupportEditMode = false;
            this.txtAreaCode.Location = new System.Drawing.Point(138, 50);
            this.txtAreaCode.Name = "txtAreaCode";
            this.txtAreaCode.ReadOnly = true;
            this.txtAreaCode.Size = new System.Drawing.Size(98, 23);
            this.txtAreaCode.TabIndex = 0;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(257, 52);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 1;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editDescription.Location = new System.Drawing.Point(139, 81);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.ReadOnly = true;
            this.editDescription.Size = new System.Drawing.Size(250, 121);
            this.editDescription.TabIndex = 3;
            // 
            // B06
            // 
            this.ClientSize = new System.Drawing.Size(837, 457);
            this.DefaultControl = "txtID";
            this.DefaultControlForEdit = "txtID";
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B06";
            this.Text = "B06.CFA area code";
            this.WorkAlias = "CFAArea";
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

        private Win.UI.TextBox txtAreaCode;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelAreaCode;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.EditBox editDescription;
    }
}
