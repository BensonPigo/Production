namespace Sci.Production.Centralized
{
    partial class Shipping_B10
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
            this.labelReason = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.editDesc = new Sci.Win.UI.EditBox();
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
            this.detail.Size = new System.Drawing.Size(830, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.editDesc);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.displayID);
            this.detailcont.Controls.Add(this.labelReason);
            this.detailcont.Controls.Add(this.labelID);
            this.detailcont.Size = new System.Drawing.Size(830, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(830, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(830, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(838, 424);
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
            this.labelID.Location = new System.Drawing.Point(35, 28);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 0;
            this.labelID.Text = "ID";
            // 
            // labelReason
            // 
            this.labelReason.Location = new System.Drawing.Point(35, 68);
            this.labelReason.Name = "labelReason";
            this.labelReason.Size = new System.Drawing.Size(75, 23);
            this.labelReason.TabIndex = 1;
            this.labelReason.Text = "Reason";
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(114, 28);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(60, 23);
            this.displayID.TabIndex = 2;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(370, 28);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 4;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // editDesc
            // 
            this.editDesc.BackColor = System.Drawing.Color.White;
            this.editDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDesc.Location = new System.Drawing.Point(114, 68);
            this.editDesc.Multiline = true;
            this.editDesc.Name = "editDesc";
            this.editDesc.Size = new System.Drawing.Size(481, 50);
            this.editDesc.TabIndex = 72;
            // 
            // Shipping_B10
            // 
            this.ClientSize = new System.Drawing.Size(838, 457);
            this.DefaultFilter = "Type=\'OS\'";
            this.DefaultOrder = "ID";
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "Shipping_B10";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "Shipping_B10. Reason for International Express Other Sample";
            this.WorkAlias = "ShippingReason";
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
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label labelReason;
        private Win.UI.Label labelID;
        private Win.UI.EditBox editDesc;
    }
}
