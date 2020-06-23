namespace Sci.Production.Quality
{
    partial class B11
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
            this.labelDescription = new Sci.Win.UI.Label();
            this.txtID = new Sci.Win.UI.TextBox();
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
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.labelID);
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
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(73, 46);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(80, 23);
            this.labelID.TabIndex = 4;
            this.labelID.Text = "ID";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(73, 84);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(80, 23);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "Description";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtID.Location = new System.Drawing.Point(156, 46);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(100, 23);
            this.txtID.TabIndex = 3;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(280, 48);
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
            this.editDescription.Location = new System.Drawing.Point(156, 84);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(292, 50);
            this.editDescription.TabIndex = 5;
            // 
            // B11
            // 
            this.ClientSize = new System.Drawing.Size(837, 457);
            this.IsDeleteOnBrowse = false;
            this.IsSupportDelete = false;
            this.IsSupportLocate = false;
            this.IsSupportPrint = false;
            this.Name = "B11";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B11. Shadeband Doc Location";
            this.WorkAlias = "ShadebandDocLocation";
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
        private Win.UI.TextBox txtID;
        private Win.UI.Label labelID;
        private Win.UI.Label labelDescription;
        private Win.UI.EditBox editDescription;
    }
}
