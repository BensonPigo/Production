namespace Sci.Production.Basic
{
    partial class B16
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
            this.labelCode = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.displayCode = new Sci.Win.UI.DisplayBox();
            this.displayDescription = new Sci.Win.UI.DisplayBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.chkIncludeSeaShipping = new Sci.Win.UI.CheckBox();
            this.checkBoxNeedCreateAPP = new Sci.Win.UI.CheckBox();
            this.chkNeedCreateIntExpress = new Sci.Win.UI.CheckBox();
            this.label1 = new Sci.Win.UI.Label();
            this.dispLoadingType = new Sci.Win.UI.DisplayBox();
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
            this.detail.Size = new System.Drawing.Size(826, 340);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.dispLoadingType);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.chkNeedCreateIntExpress);
            this.detailcont.Controls.Add(this.checkBoxNeedCreateAPP);
            this.detailcont.Controls.Add(this.chkIncludeSeaShipping);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.displayDescription);
            this.detailcont.Controls.Add(this.displayCode);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Size = new System.Drawing.Size(826, 302);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 302);
            this.detailbtm.Size = new System.Drawing.Size(826, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(826, 340);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(834, 369);
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
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(38, 36);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(88, 23);
            this.labelCode.TabIndex = 0;
            this.labelCode.Text = "Code";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(38, 65);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(88, 23);
            this.labelDescription.TabIndex = 1;
            this.labelDescription.Text = "Description";
            // 
            // displayCode
            // 
            this.displayCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Id", true));
            this.displayCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCode.Location = new System.Drawing.Point(129, 36);
            this.displayCode.Name = "displayCode";
            this.displayCode.Size = new System.Drawing.Size(96, 23);
            this.displayCode.TabIndex = 2;
            // 
            // displayDescription
            // 
            this.displayDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDescription.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Description", true));
            this.displayDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDescription.Location = new System.Drawing.Point(129, 65);
            this.displayDescription.Name = "displayDescription";
            this.displayDescription.Size = new System.Drawing.Size(433, 23);
            this.displayDescription.TabIndex = 3;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkJunk.Location = new System.Drawing.Point(584, 36);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.ReadOnly = true;
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 4;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // chkIncludeSeaShipping
            // 
            this.chkIncludeSeaShipping.AutoSize = true;
            this.chkIncludeSeaShipping.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IncludeSeaShipping", true));
            this.chkIncludeSeaShipping.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.chkIncludeSeaShipping.Location = new System.Drawing.Point(584, 67);
            this.chkIncludeSeaShipping.Name = "chkIncludeSeaShipping";
            this.chkIncludeSeaShipping.ReadOnly = true;
            this.chkIncludeSeaShipping.Size = new System.Drawing.Size(160, 21);
            this.chkIncludeSeaShipping.TabIndex = 5;
            this.chkIncludeSeaShipping.Text = "Include Sea Shipping";
            this.chkIncludeSeaShipping.UseVisualStyleBackColor = true;
            // 
            // checkBoxNeedCreateAPP
            // 
            this.checkBoxNeedCreateAPP.AutoSize = true;
            this.checkBoxNeedCreateAPP.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NeedCreateAPP", true));
            this.checkBoxNeedCreateAPP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkBoxNeedCreateAPP.Location = new System.Drawing.Point(584, 94);
            this.checkBoxNeedCreateAPP.Name = "checkBoxNeedCreateAPP";
            this.checkBoxNeedCreateAPP.ReadOnly = true;
            this.checkBoxNeedCreateAPP.Size = new System.Drawing.Size(185, 21);
            this.checkBoxNeedCreateAPP.TabIndex = 6;
            this.checkBoxNeedCreateAPP.Text = "Need create Air Pre-Paid";
            this.checkBoxNeedCreateAPP.UseVisualStyleBackColor = true;
            // 
            // chkNeedCreateIntExpress
            // 
            this.chkNeedCreateIntExpress.AutoSize = true;
            this.chkNeedCreateIntExpress.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NeedCreateIntExpress", true));
            this.chkNeedCreateIntExpress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.chkNeedCreateIntExpress.Location = new System.Drawing.Point(584, 121);
            this.chkNeedCreateIntExpress.Name = "chkNeedCreateIntExpress";
            this.chkNeedCreateIntExpress.ReadOnly = true;
            this.chkNeedCreateIntExpress.Size = new System.Drawing.Size(241, 21);
            this.chkNeedCreateIntExpress.TabIndex = 7;
            this.chkNeedCreateIntExpress.Text = "Need create International Express";
            this.chkNeedCreateIntExpress.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(38, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "Loading Type";
            // 
            // dispLoadingType
            // 
            this.dispLoadingType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispLoadingType.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "LoadingType", true));
            this.dispLoadingType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispLoadingType.Location = new System.Drawing.Point(129, 94);
            this.dispLoadingType.Name = "dispLoadingType";
            this.dispLoadingType.Size = new System.Drawing.Size(433, 23);
            this.dispLoadingType.TabIndex = 9;
            // 
            // B16
            // 
            this.ClientSize = new System.Drawing.Size(834, 402);
            this.EnableGridJunkColor = true;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B16";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B16. Shipping Mode";
            this.UniqueExpress = "Id";
            this.WorkAlias = "ShipMode";
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

        private Win.UI.Label labelCode;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.DisplayBox displayDescription;
        private Win.UI.DisplayBox displayCode;
        private Win.UI.Label labelDescription;
        private Win.UI.CheckBox chkIncludeSeaShipping;
        private Win.UI.CheckBox checkBoxNeedCreateAPP;
        private Win.UI.CheckBox chkNeedCreateIntExpress;
        private Win.UI.DisplayBox dispLoadingType;
        private Win.UI.Label label1;
    }
}
