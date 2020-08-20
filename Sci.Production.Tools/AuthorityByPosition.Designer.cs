namespace Sci.Production.Tools
{
    partial class AuthorityByPosition
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
            this.btnModifyHistory = new Sci.Win.UI.Button();
            this.comboMenuFilter = new Sci.Win.UI.ComboBox();
            this.labelMenuFilter = new Sci.Win.UI.Label();
            this.txtDescription = new Sci.Win.UI.TextBox();
            this.labelDescription = new Sci.Win.UI.Label();
            this.checkAdministrator = new Sci.Win.UI.CheckBox();
            this.txtPosition = new Sci.Win.UI.TextBox();
            this.labelPosition = new Sci.Win.UI.Label();
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
            this.masterpanel.Controls.Add(this.btnModifyHistory);
            this.masterpanel.Controls.Add(this.comboMenuFilter);
            this.masterpanel.Controls.Add(this.labelMenuFilter);
            this.masterpanel.Controls.Add(this.txtDescription);
            this.masterpanel.Controls.Add(this.labelDescription);
            this.masterpanel.Controls.Add(this.checkAdministrator);
            this.masterpanel.Controls.Add(this.txtPosition);
            this.masterpanel.Controls.Add(this.labelPosition);
            this.masterpanel.Size = new System.Drawing.Size(918, 90);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPosition, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtPosition, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkAdministrator, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDescription, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtDescription, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelMenuFilter, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboMenuFilter, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnModifyHistory, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 90);
            this.detailpanel.Size = new System.Drawing.Size(918, 385);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gridicon.Location = new System.Drawing.Point(992, 52);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(827, 1);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(918, 385);
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
            this.detail.Size = new System.Drawing.Size(918, 513);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(918, 475);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 475);
            this.detailbtm.Size = new System.Drawing.Size(918, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(918, 513);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(926, 542);
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
            // btnModifyHistory
            // 
            this.btnModifyHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModifyHistory.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnModifyHistory.Location = new System.Drawing.Point(783, 12);
            this.btnModifyHistory.Name = "btnModifyHistory";
            this.btnModifyHistory.Size = new System.Drawing.Size(127, 30);
            this.btnModifyHistory.TabIndex = 16;
            this.btnModifyHistory.Text = "Modify History";
            this.btnModifyHistory.UseVisualStyleBackColor = true;
            this.btnModifyHistory.Click += new System.EventHandler(this.BtnModifyHistory_Click);
            // 
            // comboMenuFilter
            // 
            this.comboMenuFilter.BackColor = System.Drawing.Color.White;
            this.comboMenuFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMenuFilter.FormattingEnabled = true;
            this.comboMenuFilter.IsSupportUnselect = true;
            this.comboMenuFilter.Location = new System.Drawing.Point(506, 18);
            this.comboMenuFilter.Name = "comboMenuFilter";
            this.comboMenuFilter.Size = new System.Drawing.Size(236, 24);
            this.comboMenuFilter.TabIndex = 15;
            this.comboMenuFilter.SelectedIndexChanged += new System.EventHandler(this.ComboMenuFilter_SelectedIndexChanged);
            // 
            // labelMenuFilter
            // 
            this.labelMenuFilter.Location = new System.Drawing.Point(422, 19);
            this.labelMenuFilter.Name = "labelMenuFilter";
            this.labelMenuFilter.Size = new System.Drawing.Size(81, 23);
            this.labelMenuFilter.TabIndex = 14;
            this.labelMenuFilter.Text = "Menu Filter";
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDescription.Location = new System.Drawing.Point(96, 50);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(646, 23);
            this.txtDescription.TabIndex = 13;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(18, 50);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 12;
            this.labelDescription.Text = "Description";
            // 
            // checkAdministrator
            // 
            this.checkAdministrator.AutoSize = true;
            this.checkAdministrator.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsAdmin", true));
            this.checkAdministrator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkAdministrator.Location = new System.Drawing.Point(282, 21);
            this.checkAdministrator.Name = "checkAdministrator";
            this.checkAdministrator.Size = new System.Drawing.Size(110, 21);
            this.checkAdministrator.TabIndex = 11;
            this.checkAdministrator.Text = "Administrator";
            this.checkAdministrator.UseVisualStyleBackColor = true;
            // 
            // txtPosition
            // 
            this.txtPosition.BackColor = System.Drawing.Color.White;
            this.txtPosition.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtPosition.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtPosition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPosition.Location = new System.Drawing.Point(96, 20);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(176, 23);
            this.txtPosition.TabIndex = 10;
            this.txtPosition.Validating += new System.ComponentModel.CancelEventHandler(this.TxtPosition_Validating);
            // 
            // labelPosition
            // 
            this.labelPosition.Location = new System.Drawing.Point(18, 20);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(75, 23);
            this.labelPosition.TabIndex = 9;
            this.labelPosition.Text = "Position";
            // 
            // AuthorityByPosition
            // 
            this.ClientSize = new System.Drawing.Size(926, 575);
            this.GridAlias = "Pass2";
            this.GridNew = 0;
            this.IsGridIconVisible = false;
            this.IsSupportClip = false;
            this.KeyField1 = "PKEY";
            this.KeyField2 = "FKPASS0";
            this.Name = "AuthorityByPosition";
            this.Text = "Authority By Position";
            this.WorkAlias = "Pass0";
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

        private Win.UI.Button btnModifyHistory;
        private Win.UI.ComboBox comboMenuFilter;
        private Win.UI.Label labelMenuFilter;
        private Win.UI.TextBox txtDescription;
        private Win.UI.Label labelDescription;
        private Win.UI.CheckBox checkAdministrator;
        private Win.UI.TextBox txtPosition;
        private Win.UI.Label labelPosition;
    }
}
