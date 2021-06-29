namespace Sci.Production.Centralized
{
    partial class IE_B90
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
            this.txtType = new Sci.Win.UI.TextBox();
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
            this.masterpanel.Controls.Add(this.txtType);
            this.masterpanel.Controls.Add(this.checkJunk);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Size = new System.Drawing.Size(807, 110);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkJunk, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtType, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 110);
            this.detailpanel.Size = new System.Drawing.Size(807, 210);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(5, 75);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(807, 210);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(807, 358);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(801, 312);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(801, 40);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(807, 358);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(807, 320);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 320);
            this.detailbtm.Size = new System.Drawing.Size(807, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(807, 358);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(815, 387);
            // 
            // txtType
            // 
            this.txtType.BackColor = System.Drawing.Color.White;
            this.txtType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Type", true));
            this.txtType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtType.Location = new System.Drawing.Point(100, 14);
            this.txtType.MaxLength = 10;
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(194, 23);
            this.txtType.TabIndex = 13;
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
            this.labelID.Size = new System.Drawing.Size(64, 23);
            this.labelID.TabIndex = 17;
            this.labelID.Text = "Type";
            // 
            // IE_B90
            // 
            this.ClientSize = new System.Drawing.Size(815, 420);
            this.ConnectionName = "ProductionTPE";
            this.DefaultDetailOrder = "Type,TypeGroup";
            this.DefaultOrder = "Type";
            this.GridAlias = "IEReasonTypeGroup";
            this.GridUniqueKey = "Type,TypeGroup";
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "Type";
            this.Name = "IE_B90";
            this.OnLineHelpID = "Sci.Win.Tems.Input2";
            this.Text = "IE_B90. IE Reason Type";
            this.UniqueExpress = "Type";
            this.WorkAlias = "IEReasonType";
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
        private Win.UI.TextBox txtType;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label labelID;
    }
}
