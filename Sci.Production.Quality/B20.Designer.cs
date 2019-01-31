namespace Sci.Production.Quality
{
    partial class B20
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
            this.labelDefectType = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.txtDefectType = new Sci.Win.UI.TextBox();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.editLocalDesc = new Sci.Win.UI.EditBox();
            this.labLocalDesc = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(826, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.editLocalDesc);
            this.detailcont.Controls.Add(this.labLocalDesc);
            this.detailcont.Controls.Add(this.editDescription);
            this.detailcont.Controls.Add(this.txtDefectType);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.labelDefectType);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Size = new System.Drawing.Size(826, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(826, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(826, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(834, 424);
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
            // labelDefectType
            // 
            this.labelDefectType.Location = new System.Drawing.Point(70, 51);
            this.labelDefectType.Name = "labelDefectType";
            this.labelDefectType.Size = new System.Drawing.Size(85, 23);
            this.labelDefectType.TabIndex = 4;
            this.labelDefectType.Text = "Defect Type";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(70, 86);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(85, 23);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "Description";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(351, 53);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 2;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // txtDefectType
            // 
            this.txtDefectType.BackColor = System.Drawing.Color.White;
            this.txtDefectType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtDefectType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDefectType.Location = new System.Drawing.Point(158, 51);
            this.txtDefectType.Name = "txtDefectType";
            this.txtDefectType.Size = new System.Drawing.Size(132, 23);
            this.txtDefectType.TabIndex = 0;
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(158, 86);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(250, 64);
            this.editDescription.TabIndex = 1;
            // 
            // editLocalDesc
            // 
            this.editLocalDesc.BackColor = System.Drawing.Color.White;
            this.editLocalDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "LocalDescription", true));
            this.editLocalDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editLocalDesc.Location = new System.Drawing.Point(158, 156);
            this.editLocalDesc.Multiline = true;
            this.editLocalDesc.Name = "editLocalDesc";
            this.editLocalDesc.Size = new System.Drawing.Size(250, 64);
            this.editLocalDesc.TabIndex = 6;
            // 
            // labLocalDesc
            // 
            this.labLocalDesc.Location = new System.Drawing.Point(70, 156);
            this.labLocalDesc.Name = "labLocalDesc";
            this.labLocalDesc.Size = new System.Drawing.Size(85, 23);
            this.labLocalDesc.TabIndex = 7;
            this.labLocalDesc.Text = "Local Desc.";
            // 
            // B20
            // 
            this.ClientSize = new System.Drawing.Size(834, 457);
            this.DefaultControl = "txtDefectType";
            this.DefaultControlForEdit = "editDescription";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B20";
            this.Text = "B20. Defect type for RFT/CFA(Garment)";
            this.WorkAlias = "GarmentDefectType";
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

        private Win.UI.TextBox txtDefectType;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label labelDefectType;
        private Win.UI.Label labelDescription;
        private Win.UI.EditBox editDescription;
        private Win.UI.EditBox editLocalDesc;
        private Win.UI.Label labLocalDesc;
    }
}
