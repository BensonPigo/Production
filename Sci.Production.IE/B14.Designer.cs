namespace Sci.Production.IE
{
    partial class B14
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
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelID = new Sci.Win.UI.Label();
            this.editType = new Sci.Win.UI.EditBox();
            this.displayID = new Sci.Win.UI.DisplayBox();
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
            this.detailcont.Controls.Add(this.displayID);
            this.detailcont.Controls.Add(this.editType);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelID);
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
            this.checkJunk.Location = new System.Drawing.Point(443, 44);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 2;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(37, 96);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(166, 23);
            this.labelDescription.TabIndex = 14;
            this.labelDescription.Text = "Attachment Measurement";
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(37, 44);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(63, 23);
            this.labelID.TabIndex = 13;
            this.labelID.Text = "ID";
            // 
            // editType
            // 
            this.editType.BackColor = System.Drawing.Color.White;
            this.editType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Measurement", true));
            this.editType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editType.Location = new System.Drawing.Point(206, 96);
            this.editType.Multiline = true;
            this.editType.Name = "editType";
            this.editType.Size = new System.Drawing.Size(405, 50);
            this.editType.TabIndex = 1;
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(103, 44);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(100, 23);
            this.displayID.TabIndex = 15;
            // 
            // B14
            // 
            this.ClientSize = new System.Drawing.Size(837, 457);
            this.DefaultOrder = "ID";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B14";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B14. Attachment Measurement";
            this.WorkAlias = "AttachmentMeasurement";
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
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelID;
        private Win.UI.EditBox editType;
        private Win.UI.DisplayBox displayID;
    }
}
