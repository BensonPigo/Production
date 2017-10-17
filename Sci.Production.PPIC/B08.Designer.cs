namespace Sci.Production.PPIC
{
    partial class B08
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
            this.labelName = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.displayName = new Sci.Win.UI.DisplayBox();
            this.editDescription = new Sci.Win.UI.EditBox();
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
            this.masterpanel.Controls.Add(this.editDescription);
            this.masterpanel.Controls.Add(this.displayName);
            this.masterpanel.Controls.Add(this.labelDescription);
            this.masterpanel.Controls.Add(this.labelName);
            this.masterpanel.Size = new System.Drawing.Size(912, 100);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelName, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDescription, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayName, 0);
            this.masterpanel.Controls.SetChildIndex(this.editDescription, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Size = new System.Drawing.Size(912, 249);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(654, 65);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(827, 1);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(912, 249);
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
            this.detail.Size = new System.Drawing.Size(912, 387);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(912, 349);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 349);
            this.detailbtm.Size = new System.Drawing.Size(912, 38);
            this.detailbtm.TabIndex = 0;
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(912, 387);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(920, 416);
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
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(11, 13);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(75, 23);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "Name";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(11, 40);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = "Description";
            // 
            // displayName
            // 
            this.displayName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayName.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Name", true));
            this.displayName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayName.Location = new System.Drawing.Point(90, 13);
            this.displayName.Name = "displayName";
            this.displayName.Size = new System.Drawing.Size(320, 23);
            this.displayName.TabIndex = 0;
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(90, 40);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(320, 50);
            this.editDescription.TabIndex = 1;
            // 
            // B08
            // 
            this.ClientSize = new System.Drawing.Size(920, 449);
            this.DefaultDetailOrder = "Day";
            this.DefaultOrder = "Name";
            this.GridAlias = "LearnCurve_Detail";
            this.GridEdit = false;
            this.IsGridIconVisible = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "ID";
            this.Name = "B08";
            this.Text = "B08. Learning Curve";
            this.UniqueExpress = "ID";
            this.WorkAlias = "LearnCurve";
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

        private Win.UI.EditBox editDescription;
        private Win.UI.DisplayBox displayName;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelName;
    }
}
