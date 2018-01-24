namespace Sci.Production.SubProcess
{
    partial class B03
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.editRemark = new Sci.Win.UI.EditBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.txtID = new Sci.Win.UI.TextBox();
            this.txtType = new Sci.Win.UI.TextBox();
            this.labType = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labID = new Sci.Win.UI.Label();
            this.labGroup = new Sci.Win.UI.Label();
            this.txtGroup = new Sci.Win.UI.TextBox();
            this.editDesc = new Sci.Win.UI.EditBox();
            this.labRemark = new Sci.Win.UI.Label();
            this.labManPower = new Sci.Win.UI.Label();
            this.numManPower = new Sci.Win.UI.NumericBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.numManPower);
            this.detailcont.Controls.Add(this.labManPower);
            this.detailcont.Controls.Add(this.editDesc);
            this.detailcont.Controls.Add(this.labRemark);
            this.detailcont.Controls.Add(this.txtGroup);
            this.detailcont.Controls.Add(this.labGroup);
            this.detailcont.Controls.Add(this.editRemark);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.txtType);
            this.detailcont.Controls.Add(this.labType);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labID);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(602, 297);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(610, 326);
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(132, 184);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(292, 50);
            this.editRemark.TabIndex = 6;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(367, 14);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 7;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtID.IsSupportEditMode = false;
            this.txtID.Location = new System.Drawing.Point(132, 40);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(166, 23);
            this.txtID.TabIndex = 2;
            // 
            // txtType
            // 
            this.txtType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Type", true));
            this.txtType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtType.IsSupportEditMode = false;
            this.txtType.Location = new System.Drawing.Point(132, 12);
            this.txtType.Name = "txtType";
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(166, 23);
            this.txtType.TabIndex = 0;
            this.txtType.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtType_PopUp);
            this.txtType.Validating += new System.ComponentModel.CancelEventHandler(this.TxtType_Validating);
            // 
            // labType
            // 
            this.labType.Location = new System.Drawing.Point(49, 12);
            this.labType.Name = "labType";
            this.labType.Size = new System.Drawing.Size(80, 23);
            this.labType.TabIndex = 11;
            this.labType.Text = "Type";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(49, 99);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(80, 23);
            this.labelDescription.TabIndex = 13;
            this.labelDescription.Text = "Description";
            // 
            // labID
            // 
            this.labID.Location = new System.Drawing.Point(49, 40);
            this.labID.Name = "labID";
            this.labID.Size = new System.Drawing.Size(80, 23);
            this.labID.TabIndex = 12;
            this.labID.Text = "ID";
            // 
            // labGroup
            // 
            this.labGroup.Location = new System.Drawing.Point(49, 69);
            this.labGroup.Name = "labGroup";
            this.labGroup.Size = new System.Drawing.Size(80, 23);
            this.labGroup.TabIndex = 16;
            this.labGroup.Text = "Group";
            // 
            // txtGroup
            // 
            this.txtGroup.BackColor = System.Drawing.Color.White;
            this.txtGroup.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "GroupID", true));
            this.txtGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGroup.Location = new System.Drawing.Point(132, 69);
            this.txtGroup.Name = "txtGroup";
            this.txtGroup.Size = new System.Drawing.Size(166, 23);
            this.txtGroup.TabIndex = 3;
            // 
            // editDesc
            // 
            this.editDesc.BackColor = System.Drawing.Color.White;
            this.editDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDesc.Location = new System.Drawing.Point(132, 99);
            this.editDesc.Multiline = true;
            this.editDesc.Name = "editDesc";
            this.editDesc.Size = new System.Drawing.Size(292, 50);
            this.editDesc.TabIndex = 4;
            // 
            // labRemark
            // 
            this.labRemark.Location = new System.Drawing.Point(49, 184);
            this.labRemark.Name = "labRemark";
            this.labRemark.Size = new System.Drawing.Size(80, 23);
            this.labRemark.TabIndex = 19;
            this.labRemark.Text = "Remark";
            // 
            // labManPower
            // 
            this.labManPower.Location = new System.Drawing.Point(49, 153);
            this.labManPower.Name = "labManPower";
            this.labManPower.Size = new System.Drawing.Size(80, 23);
            this.labManPower.TabIndex = 20;
            this.labManPower.Text = "ManPower";
            // 
            // numManPower
            // 
            this.numManPower.BackColor = System.Drawing.Color.White;
            this.numManPower.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Manpower", true));
            this.numManPower.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numManPower.Location = new System.Drawing.Point(132, 155);
            this.numManPower.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            131072});
            this.numManPower.MaxLength = 999;
            this.numManPower.Name = "numManPower";
            this.numManPower.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numManPower.Size = new System.Drawing.Size(166, 23);
            this.numManPower.TabIndex = 5;
            this.numManPower.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // B03
            // 
            this.ClientSize = new System.Drawing.Size(610, 359);
            this.DefaultControl = "txtType";
            this.DefaultControlForEdit = "txtGroup";
            this.IsDeleteOnBrowse = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B03";
            this.Text = "B03.SubProcess Line";
            this.UniqueExpress = "Type,ID,MdivisionID";
            this.WorkAlias = "SubProcessLine";
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
        private Win.UI.EditBox editRemark;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.TextBox txtID;
        private Win.UI.TextBox txtType;
        private Win.UI.Label labType;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labID;
        private Win.UI.Label labManPower;
        private Win.UI.EditBox editDesc;
        private Win.UI.Label labRemark;
        private Win.UI.TextBox txtGroup;
        private Win.UI.Label labGroup;
        private Win.UI.NumericBox numManPower;
    }
}
