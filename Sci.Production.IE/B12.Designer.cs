namespace Sci.Production.IE
{
    partial class B12
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
            this.txtChineseDesc = new Sci.Win.UI.TextBox();
            this.labChineseDesc = new Sci.Win.UI.Label();
            this.txtDesc = new Sci.Win.UI.TextBox();
            this.labDesc = new Sci.Win.UI.Label();
            this.txtID = new Sci.Win.UI.TextBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.labelID = new Sci.Win.UI.Label();
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
            this.masterpanel.Controls.Add(this.txtChineseDesc);
            this.masterpanel.Controls.Add(this.labChineseDesc);
            this.masterpanel.Controls.Add(this.txtDesc);
            this.masterpanel.Controls.Add(this.labDesc);
            this.masterpanel.Controls.Add(this.txtID);
            this.masterpanel.Controls.Add(this.checkJunk);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Size = new System.Drawing.Size(892, 181);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkJunk, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labDesc, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtDesc, 0);
            this.masterpanel.Controls.SetChildIndex(this.labChineseDesc, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtChineseDesc, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 181);
            this.detailpanel.Size = new System.Drawing.Size(892, 168);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(5, 146);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(892, 168);
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
            this.detail.Size = new System.Drawing.Size(892, 387);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(892, 349);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 349);
            this.detailbtm.Size = new System.Drawing.Size(892, 38);
            // 
            // txtChineseDesc
            // 
            this.txtChineseDesc.BackColor = System.Drawing.Color.White;
            this.txtChineseDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescriptionCH", true));
            this.txtChineseDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtChineseDesc.Location = new System.Drawing.Point(166, 91);
            this.txtChineseDesc.Name = "txtChineseDesc";
            this.txtChineseDesc.Size = new System.Drawing.Size(300, 23);
            this.txtChineseDesc.TabIndex = 15;
            // 
            // labChineseDesc
            // 
            this.labChineseDesc.Location = new System.Drawing.Point(33, 91);
            this.labChineseDesc.Name = "labChineseDesc";
            this.labChineseDesc.Size = new System.Drawing.Size(128, 23);
            this.labChineseDesc.TabIndex = 19;
            this.labChineseDesc.Text = "Chinese Description";
            // 
            // txtDesc
            // 
            this.txtDesc.BackColor = System.Drawing.Color.White;
            this.txtDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.txtDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDesc.Location = new System.Drawing.Point(166, 54);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(300, 23);
            this.txtDesc.TabIndex = 14;
            // 
            // labDesc
            // 
            this.labDesc.Location = new System.Drawing.Point(33, 54);
            this.labDesc.Name = "labDesc";
            this.labDesc.Size = new System.Drawing.Size(128, 23);
            this.labDesc.TabIndex = 18;
            this.labDesc.Text = "Description";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtID.Location = new System.Drawing.Point(166, 14);
            this.txtID.MaxLength = 10;
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(194, 23);
            this.txtID.TabIndex = 13;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(409, 14);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 16;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(33, 14);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(128, 23);
            this.labelID.TabIndex = 17;
            this.labelID.Text = "Machine Master ID";
            // 
            // B12
            // 
            this.ClientSize = new System.Drawing.Size(900, 449);
            this.ConnectionName = "Machine";
            this.GridAlias = "MachineGroup";
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B12";
            this.Text = "B12. Machine Master Group";
            this.WorkAlias = "MachineMasterGroup";
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

        private Win.UI.TextBox txtChineseDesc;
        private Win.UI.Label labChineseDesc;
        private Win.UI.TextBox txtDesc;
        private Win.UI.Label labDesc;
        private Win.UI.TextBox txtID;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label labelID;
    }
}
