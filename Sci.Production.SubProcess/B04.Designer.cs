namespace Sci.Production.SubProcess
{
    partial class B04
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
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.txtType = new Sci.Win.UI.TextBox();
            this.labType = new Sci.Win.UI.Label();
            this.txtID = new Sci.Win.UI.TextBox();
            this.labID = new Sci.Win.UI.Label();
            this.editDesc = new Sci.Win.UI.EditBox();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labRemark = new Sci.Win.UI.Label();
            this.editRemark = new Sci.Win.UI.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).BeginInit();
            this.masterpanel.SuspendLayout();
            this.detailpanel.SuspendLayout();
            this.detail2.SuspendLayout();
            this.detailpanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.labRemark);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.editDesc);
            this.masterpanel.Controls.Add(this.labelDescription);
            this.masterpanel.Controls.Add(this.txtID);
            this.masterpanel.Controls.Add(this.labID);
            this.masterpanel.Controls.Add(this.checkJunk);
            this.masterpanel.Controls.Add(this.txtType);
            this.masterpanel.Controls.Add(this.labType);
            this.masterpanel.Size = new System.Drawing.Size(620, 191);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labType, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtType, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkJunk, 0);
            this.masterpanel.Controls.SetChildIndex(this.labID, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDescription, 0);
            this.masterpanel.Controls.SetChildIndex(this.editDesc, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labRemark, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 191);
            this.detailpanel.Size = new System.Drawing.Size(620, 158);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(474, 153);
            this.gridicon.TabIndex = 5;
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(620, 158);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(892, 387);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(886, 341);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(886, 40);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(620, 387);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(620, 349);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 349);
            this.detailbtm.Size = new System.Drawing.Size(620, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(620, 387);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(628, 416);
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(372, 14);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 4;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // txtType
            // 
            this.txtType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Type", true));
            this.txtType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtType.IsSupportEditMode = false;
            this.txtType.Location = new System.Drawing.Point(137, 12);
            this.txtType.Name = "txtType";
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(166, 23);
            this.txtType.TabIndex = 0;
            this.txtType.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtType_PopUp);
            this.txtType.Validating += new System.ComponentModel.CancelEventHandler(this.TxtType_Validating);
            // 
            // labType
            // 
            this.labType.Location = new System.Drawing.Point(54, 12);
            this.labType.Name = "labType";
            this.labType.Size = new System.Drawing.Size(80, 23);
            this.labType.TabIndex = 14;
            this.labType.Text = "Type";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtID.IsSupportEditMode = false;
            this.txtID.Location = new System.Drawing.Point(137, 38);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(166, 23);
            this.txtID.TabIndex = 1;
            // 
            // labID
            // 
            this.labID.Location = new System.Drawing.Point(54, 38);
            this.labID.Name = "labID";
            this.labID.Size = new System.Drawing.Size(80, 23);
            this.labID.TabIndex = 16;
            this.labID.Text = "ID";
            // 
            // editDesc
            // 
            this.editDesc.BackColor = System.Drawing.Color.White;
            this.editDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDesc.Location = new System.Drawing.Point(137, 64);
            this.editDesc.Multiline = true;
            this.editDesc.Name = "editDesc";
            this.editDesc.Size = new System.Drawing.Size(292, 50);
            this.editDesc.TabIndex = 2;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(54, 64);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(80, 23);
            this.labelDescription.TabIndex = 19;
            this.labelDescription.Text = "Description";
            // 
            // labRemark
            // 
            this.labRemark.Location = new System.Drawing.Point(54, 121);
            this.labRemark.Name = "labRemark";
            this.labRemark.Size = new System.Drawing.Size(80, 23);
            this.labRemark.TabIndex = 22;
            this.labRemark.Text = "Remark";
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(137, 121);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(292, 50);
            this.editRemark.TabIndex = 3;
            // 
            // B04
            // 
            this.ClientSize = new System.Drawing.Size(628, 449);
            this.DefaultControl = "txtType";
            this.DefaultControlForEdit = "editDesc";
            this.GridAlias = "SubProcessLearnCurve_Detail";
            this.GridUniqueKey = "ukey,Day";
            this.KeyField1 = "Ukey";
            this.KeyField2 = "Ukey";
            this.Name = "B04";
            this.Text = "B04.SubProcess Learning Curve";
            this.WorkAlias = "SubProcessLearnCurve";
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).EndInit();
            this.masterpanel.ResumeLayout(false);
            this.masterpanel.PerformLayout();
            this.detailpanel.ResumeLayout(false);
            this.detail2.ResumeLayout(false);
            this.detailpanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.CheckBox checkJunk;
        private Win.UI.TextBox txtType;
        private Win.UI.Label labType;
        private Win.UI.TextBox txtID;
        private Win.UI.Label labID;
        private Win.UI.EditBox editDesc;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labRemark;
        private Win.UI.EditBox editRemark;
    }
}
