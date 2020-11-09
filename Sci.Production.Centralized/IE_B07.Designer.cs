namespace Sci.Production.Centralized
{
    partial class IE_B07
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
            this.labelType = new Sci.Win.UI.Label();
            this.labelDescriptionEnglish = new Sci.Win.UI.Label();
            this.labelDescriptionChinese = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.comboType = new Sci.Win.UI.ComboBox();
            this.editDescriptionEnglish = new Sci.Win.UI.EditBox();
            this.editDescriptionChinese = new Sci.Win.UI.EditBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.chkIsAttachment = new Sci.Win.UI.CheckBox();
            this.chkIsTemplate = new Sci.Win.UI.CheckBox();
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
            this.detail.Size = new System.Drawing.Size(828, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.chkIsTemplate);
            this.detailcont.Controls.Add(this.chkIsAttachment);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.editDescriptionChinese);
            this.detailcont.Controls.Add(this.editDescriptionEnglish);
            this.detailcont.Controls.Add(this.comboType);
            this.detailcont.Controls.Add(this.displayID);
            this.detailcont.Controls.Add(this.labelDescriptionChinese);
            this.detailcont.Controls.Add(this.labelDescriptionEnglish);
            this.detailcont.Controls.Add(this.labelType);
            this.detailcont.Controls.Add(this.labelID);
            this.detailcont.Size = new System.Drawing.Size(828, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(828, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(828, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(836, 424);
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
            this.labelID.Location = new System.Drawing.Point(42, 33);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 0;
            this.labelID.Text = "ID";
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(42, 73);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(75, 23);
            this.labelType.TabIndex = 1;
            this.labelType.Text = "Type";
            // 
            // labelDescriptionEnglish
            // 
            this.labelDescriptionEnglish.Location = new System.Drawing.Point(42, 113);
            this.labelDescriptionEnglish.Name = "labelDescriptionEnglish";
            this.labelDescriptionEnglish.Size = new System.Drawing.Size(75, 46);
            this.labelDescriptionEnglish.TabIndex = 2;
            // 
            // labelDescriptionChinese
            // 
            this.labelDescriptionChinese.Location = new System.Drawing.Point(42, 177);
            this.labelDescriptionChinese.Name = "labelDescriptionChinese";
            this.labelDescriptionChinese.Size = new System.Drawing.Size(75, 46);
            this.labelDescriptionChinese.TabIndex = 3;
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(121, 33);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(138, 23);
            this.displayID.TabIndex = 4;
            // 
            // comboType
            // 
            this.comboType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Type", true));
            this.comboType.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboType.FormattingEnabled = true;
            this.comboType.IsSupportUnselect = true;
            this.comboType.Location = new System.Drawing.Point(121, 73);
            this.comboType.Name = "comboType";
            this.comboType.OldText = "";
            this.comboType.ReadOnly = true;
            this.comboType.Size = new System.Drawing.Size(121, 24);
            this.comboType.TabIndex = 5;
            // 
            // editDescriptionEnglish
            // 
            this.editDescriptionEnglish.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editDescriptionEnglish.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescEN", true));
            this.editDescriptionEnglish.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editDescriptionEnglish.IsSupportEditMode = false;
            this.editDescriptionEnglish.Location = new System.Drawing.Point(121, 113);
            this.editDescriptionEnglish.Multiline = true;
            this.editDescriptionEnglish.Name = "editDescriptionEnglish";
            this.editDescriptionEnglish.ReadOnly = true;
            this.editDescriptionEnglish.Size = new System.Drawing.Size(456, 50);
            this.editDescriptionEnglish.TabIndex = 8;
            // 
            // editDescriptionChinese
            // 
            this.editDescriptionChinese.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editDescriptionChinese.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescCH", true));
            this.editDescriptionChinese.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editDescriptionChinese.IsSupportEditMode = false;
            this.editDescriptionChinese.Location = new System.Drawing.Point(121, 177);
            this.editDescriptionChinese.Multiline = true;
            this.editDescriptionChinese.Name = "editDescriptionChinese";
            this.editDescriptionChinese.ReadOnly = true;
            this.editDescriptionChinese.Size = new System.Drawing.Size(456, 50);
            this.editDescriptionChinese.TabIndex = 9;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkJunk.IsSupportEditMode = false;
            this.checkJunk.Location = new System.Drawing.Point(468, 33);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.ReadOnly = true;
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 10;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // chkIsAttachment
            // 
            this.chkIsAttachment.AutoSize = true;
            this.chkIsAttachment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsAttachment.Location = new System.Drawing.Point(468, 60);
            this.chkIsAttachment.Name = "chkIsAttachment";
            this.chkIsAttachment.Size = new System.Drawing.Size(112, 21);
            this.chkIsAttachment.TabIndex = 11;
            this.chkIsAttachment.Text = "Is Attachment";
            this.chkIsAttachment.UseVisualStyleBackColor = true;
            // 
            // chkIsTemplate
            // 
            this.chkIsTemplate.AutoSize = true;
            this.chkIsTemplate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsTemplate.Location = new System.Drawing.Point(468, 87);
            this.chkIsTemplate.Name = "chkIsTemplate";
            this.chkIsTemplate.Size = new System.Drawing.Size(100, 21);
            this.chkIsTemplate.TabIndex = 12;
            this.chkIsTemplate.Text = "Is Template";
            this.chkIsTemplate.UseVisualStyleBackColor = true;
            // 
            // IE_B07
            // 
            this.ClientSize = new System.Drawing.Size(836, 457);
            this.ConnectionName = "Trade";
            this.DefaultOrder = "ID";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "IE_B07";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "IE_B07. Attachment List";
            this.UniqueExpress = "ID";
            this.WorkAlias = "Mold";
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

        private Win.UI.Label labelDescriptionChinese;
        private Win.UI.Label labelDescriptionEnglish;
        private Win.UI.Label labelType;
        private Win.UI.Label labelID;
        private Win.UI.EditBox editDescriptionChinese;
        private Win.UI.EditBox editDescriptionEnglish;
        private Win.UI.ComboBox comboType;
        private Win.UI.DisplayBox displayID;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.CheckBox chkIsTemplate;
        private Win.UI.CheckBox chkIsAttachment;
    }
}
