namespace Sci.Production.Centralized
{
    partial class IE_B91
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
            this.lbRemark = new Sci.Win.UI.Label();
            this.lbName = new Sci.Win.UI.Label();
            this.lbCode = new Sci.Win.UI.Label();
            this.labelType = new Sci.Win.UI.Label();
            this.editName = new Sci.Win.UI.EditBox();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.txtCode = new Sci.Win.UI.TextBox();
            this.txtType = new Sci.Win.UI.TextBox();
            this.displayTypeGroup = new Sci.Win.UI.DisplayBox();
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
            this.detailcont.Controls.Add(this.displayTypeGroup);
            this.detailcont.Controls.Add(this.txtType);
            this.detailcont.Controls.Add(this.txtCode);
            this.detailcont.Controls.Add(this.editRemark);
            this.detailcont.Controls.Add(this.editName);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.lbRemark);
            this.detailcont.Controls.Add(this.lbName);
            this.detailcont.Controls.Add(this.lbCode);
            this.detailcont.Controls.Add(this.labelType);
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
            this.checkJunk.Location = new System.Drawing.Point(644, 28);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 28;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // lbRemark
            // 
            this.lbRemark.Location = new System.Drawing.Point(32, 162);
            this.lbRemark.Name = "lbRemark";
            this.lbRemark.Size = new System.Drawing.Size(58, 23);
            this.lbRemark.TabIndex = 25;
            this.lbRemark.Text = "Remark";
            // 
            // lbName
            // 
            this.lbName.Location = new System.Drawing.Point(32, 98);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(58, 23);
            this.lbName.TabIndex = 23;
            this.lbName.Text = "Name";
            // 
            // lbCode
            // 
            this.lbCode.Location = new System.Drawing.Point(32, 61);
            this.lbCode.Name = "lbCode";
            this.lbCode.Size = new System.Drawing.Size(58, 23);
            this.lbCode.TabIndex = 21;
            this.lbCode.Text = "Code";
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(32, 26);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(58, 23);
            this.labelType.TabIndex = 19;
            this.labelType.Text = "Type";
            // 
            // editName
            // 
            this.editName.BackColor = System.Drawing.Color.White;
            this.editName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Name", true));
            this.editName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editName.Location = new System.Drawing.Point(93, 98);
            this.editName.Multiline = true;
            this.editName.Name = "editName";
            this.editName.Size = new System.Drawing.Size(456, 50);
            this.editName.TabIndex = 29;
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(93, 162);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(456, 50);
            this.editRemark.TabIndex = 30;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Code", true));
            this.txtCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCode.Location = new System.Drawing.Point(93, 61);
            this.txtCode.MaxLength = 10;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(194, 23);
            this.txtCode.TabIndex = 31;
            // 
            // txtType
            // 
            this.txtType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Type", true));
            this.txtType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtType.IsSupportEditMode = false;
            this.txtType.Location = new System.Drawing.Point(93, 26);
            this.txtType.MaxLength = 10;
            this.txtType.Name = "txtType";
            this.txtType.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(261, 23);
            this.txtType.TabIndex = 32;
            this.txtType.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtType_PopUp);
            // 
            // displayTypeGroup
            // 
            this.displayTypeGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTypeGroup.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "TypeGroup", true));
            this.displayTypeGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTypeGroup.Location = new System.Drawing.Point(360, 26);
            this.displayTypeGroup.Name = "displayTypeGroup";
            this.displayTypeGroup.Size = new System.Drawing.Size(256, 23);
            this.displayTypeGroup.TabIndex = 33;
            // 
            // IE_B91
            // 
            this.ClientSize = new System.Drawing.Size(837, 457);
            this.ConnectionName = "ProductionTPE";
            this.DefaultOrder = "Type,TypeGroup,Code";
            this.EnableGridJunkColor = true;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "IE_B91";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "IE_B91. IE Reason LBR Not Hit 1st";
            this.UniqueExpress = "ID";
            this.WorkAlias = "IEReasonLBRNotHit_1st";
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
        private Win.UI.Label lbRemark;
        private Win.UI.Label lbName;
        private Win.UI.Label lbCode;
        private Win.UI.Label labelType;
        private Win.UI.EditBox editRemark;
        private Win.UI.EditBox editName;
        private Win.UI.DisplayBox displayTypeGroup;
        private Win.UI.TextBox txtType;
        private Win.UI.TextBox txtCode;
    }
}
