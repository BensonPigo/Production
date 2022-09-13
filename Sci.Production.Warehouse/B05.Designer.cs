namespace Sci.Production.Warehouse
{
    partial class B05
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
            this.txtChineseDesc = new Sci.Win.UI.TextBox();
            this.txtEnglishDesc = new Sci.Win.UI.TextBox();
            this.labelEnglishDesc = new Sci.Win.UI.Label();
            this.labelChineseDesc = new Sci.Win.UI.Label();
            this.chkRawMaterial = new Sci.Win.UI.CheckBox();
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
            this.detail.Size = new System.Drawing.Size(900, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.chkRawMaterial);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.txtChineseDesc);
            this.detailcont.Controls.Add(this.txtEnglishDesc);
            this.detailcont.Controls.Add(this.labelEnglishDesc);
            this.detailcont.Controls.Add(this.labelChineseDesc);
            this.detailcont.Size = new System.Drawing.Size(900, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(900, 38);
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
            this.editby.Location = new System.Drawing.Point(494, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(446, 13);
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(655, 17);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 17;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // txtChineseDesc
            // 
            this.txtChineseDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtChineseDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.txtChineseDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtChineseDesc.IsSupportEditMode = false;
            this.txtChineseDesc.Location = new System.Drawing.Point(125, 58);
            this.txtChineseDesc.Name = "txtChineseDesc";
            this.txtChineseDesc.ReadOnly = true;
            this.txtChineseDesc.Size = new System.Drawing.Size(391, 23);
            this.txtChineseDesc.TabIndex = 16;
            // 
            // txtEnglishDesc
            // 
            this.txtEnglishDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtEnglishDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Name", true));
            this.txtEnglishDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtEnglishDesc.IsSupportEditMode = false;
            this.txtEnglishDesc.Location = new System.Drawing.Point(125, 17);
            this.txtEnglishDesc.Name = "txtEnglishDesc";
            this.txtEnglishDesc.ReadOnly = true;
            this.txtEnglishDesc.Size = new System.Drawing.Size(511, 23);
            this.txtEnglishDesc.TabIndex = 15;
            // 
            // labelEnglishDesc
            // 
            this.labelEnglishDesc.Location = new System.Drawing.Point(16, 17);
            this.labelEnglishDesc.Name = "labelEnglishDesc";
            this.labelEnglishDesc.Size = new System.Drawing.Size(100, 23);
            this.labelEnglishDesc.TabIndex = 12;
            this.labelEnglishDesc.Text = "English Desc";
            // 
            // labelChineseDesc
            // 
            this.labelChineseDesc.Location = new System.Drawing.Point(16, 58);
            this.labelChineseDesc.Name = "labelChineseDesc";
            this.labelChineseDesc.Size = new System.Drawing.Size(100, 23);
            this.labelChineseDesc.TabIndex = 13;
            this.labelChineseDesc.Text = "Chinese Desc";
            // 
            // chkRawMaterial
            // 
            this.chkRawMaterial.AutoSize = true;
            this.chkRawMaterial.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "TransferOut", true));
            this.chkRawMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.chkRawMaterial.IsSupportEditMode = false;
            this.chkRawMaterial.Location = new System.Drawing.Point(16, 101);
            this.chkRawMaterial.Name = "chkRawMaterial";
            this.chkRawMaterial.ReadOnly = true;
            this.chkRawMaterial.Size = new System.Drawing.Size(240, 21);
            this.chkRawMaterial.TabIndex = 19;
            this.chkRawMaterial.Text = "Raw Material Transfer Out to TPE";
            this.chkRawMaterial.UseVisualStyleBackColor = true;
            // 
            // B05
            // 
            this.ClientSize = new System.Drawing.Size(908, 457);
            this.DefaultFilter = "ReasonTypeID=\'Stock_Adjust\'";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B05";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B05.Adjust Reason";
            this.UniqueExpress = "ReasonTypeID,ID";
            this.WorkAlias = "Reason";
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
        private Win.UI.TextBox txtChineseDesc;
        private Win.UI.TextBox txtEnglishDesc;
        private Win.UI.Label labelEnglishDesc;
        private Win.UI.Label labelChineseDesc;
        private Win.UI.CheckBox chkRawMaterial;
    }
}
