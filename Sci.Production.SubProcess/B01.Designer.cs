namespace Sci.Production.SubProcess
{
    partial class B01
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
            this.txtFeature = new Sci.Win.UI.TextBox();
            this.txtType = new Sci.Win.UI.TextBox();
            this.labelCode = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labeltype = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(627, 299);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.editRemark);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.txtFeature);
            this.detailcont.Controls.Add(this.txtType);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labeltype);
            this.detailcont.Size = new System.Drawing.Size(627, 261);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 261);
            this.detailbtm.Size = new System.Drawing.Size(627, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(726, 362);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(635, 328);
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(133, 98);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(292, 50);
            this.editRemark.TabIndex = 2;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(257, 36);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 3;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // txtFeature
            // 
            this.txtFeature.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtFeature.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Feature", true));
            this.txtFeature.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtFeature.IsSupportEditMode = false;
            this.txtFeature.Location = new System.Drawing.Point(133, 66);
            this.txtFeature.Name = "txtFeature";
            this.txtFeature.ReadOnly = true;
            this.txtFeature.Size = new System.Drawing.Size(100, 23);
            this.txtFeature.TabIndex = 1;
            // 
            // txtType
            // 
            this.txtType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Type", true));
            this.txtType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtType.IsSupportEditMode = false;
            this.txtType.Location = new System.Drawing.Point(133, 34);
            this.txtType.Name = "txtType";
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(100, 23);
            this.txtType.TabIndex = 0;
            this.txtType.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtType_PopUp);
            this.txtType.Validating += new System.ComponentModel.CancelEventHandler(this.TxtType_Validating);
            // 
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(50, 34);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(80, 23);
            this.labelCode.TabIndex = 4;
            this.labelCode.Text = "Type";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(50, 98);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(80, 23);
            this.labelDescription.TabIndex = 6;
            this.labelDescription.Text = "Remark";
            // 
            // labeltype
            // 
            this.labeltype.Location = new System.Drawing.Point(50, 66);
            this.labeltype.Name = "labeltype";
            this.labeltype.Size = new System.Drawing.Size(80, 23);
            this.labeltype.TabIndex = 5;
            this.labeltype.Text = "Feature";
            // 
            // B01
            // 
            this.ClientSize = new System.Drawing.Size(635, 361);
            this.DefaultControl = "txtType";
            this.DefaultControlForEdit = "txtFeature";
            this.IsDeleteOnBrowse = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B01";
            this.Text = "B01. SubProcess Feature";
            this.UniqueExpress = "Type, Feature";
            this.WorkAlias = "SubProcessFeature";
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
        private Win.UI.TextBox txtFeature;
        private Win.UI.TextBox txtType;
        private Win.UI.Label labelCode;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labeltype;
    }
}
