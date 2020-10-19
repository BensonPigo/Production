namespace Sci.Production.IE
{
    partial class B06
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
            this.chkDP = new Sci.Win.UI.CheckBox();
            this.chkIsSew = new Sci.Win.UI.CheckBox();
            this.chkIsBD = new Sci.Win.UI.CheckBox();
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
            this.detail.Size = new System.Drawing.Size(825, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.chkIsBD);
            this.detailcont.Controls.Add(this.chkIsSew);
            this.detailcont.Controls.Add(this.chkDP);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.displayDescription);
            this.detailcont.Controls.Add(this.displayCode);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Size = new System.Drawing.Size(825, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(825, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(825, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(833, 424);
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
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(36, 42);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(101, 23);
            this.labelCode.TabIndex = 0;
            this.labelCode.Text = "Machine Group";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(36, 90);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(101, 23);
            this.labelDescription.TabIndex = 1;
            this.labelDescription.Text = "Description";
            // 
            // displayCode
            // 
            this.displayCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCode.Location = new System.Drawing.Point(140, 42);
            this.displayCode.Name = "displayCode";
            this.displayCode.Size = new System.Drawing.Size(50, 23);
            this.displayCode.TabIndex = 0;
            // 
            // displayDescription
            // 
            this.displayDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDescription.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Description", true));
            this.displayDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDescription.Location = new System.Drawing.Point(140, 90);
            this.displayDescription.Name = "displayDescription";
            this.displayDescription.Size = new System.Drawing.Size(414, 23);
            this.displayDescription.TabIndex = 5;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(497, 42);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 4;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // chkDP
            // 
            this.chkDP.AutoSize = true;
            this.chkDP.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsDP", true));
            this.chkDP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkDP.Location = new System.Drawing.Point(285, 42);
            this.chkDP.Name = "chkDP";
            this.chkDP.Size = new System.Drawing.Size(46, 21);
            this.chkDP.TabIndex = 1;
            this.chkDP.Text = "DP";
            this.chkDP.UseVisualStyleBackColor = true;
            // 
            // chkIsSew
            // 
            this.chkIsSew.AutoSize = true;
            this.chkIsSew.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsSew", true));
            this.chkIsSew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsSew.Location = new System.Drawing.Point(389, 42);
            this.chkIsSew.Name = "chkIsSew";
            this.chkIsSew.Size = new System.Drawing.Size(53, 21);
            this.chkIsSew.TabIndex = 2;
            this.chkIsSew.Text = "Sew";
            this.chkIsSew.UseVisualStyleBackColor = true;
            // 
            // chkIsBD
            // 
            this.chkIsBD.AutoSize = true;
            this.chkIsBD.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsBD", true));
            this.chkIsBD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsBD.Location = new System.Drawing.Point(337, 42);
            this.chkIsBD.Name = "chkIsBD";
            this.chkIsBD.Size = new System.Drawing.Size(46, 21);
            this.chkIsBD.TabIndex = 3;
            this.chkIsBD.Text = "BD";
            this.chkIsBD.UseVisualStyleBackColor = true;
            // 
            // B06
            // 
            this.ClientSize = new System.Drawing.Size(833, 457);
            this.ConnectionName = "Machine";
            this.DefaultControl = "displayCode";
            this.DefaultControlForEdit = "displayDescription";
            this.DefaultOrder = "ID";
            this.EnableGridJunkColor = true;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B06";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B06. Machine Group Type";
            this.UniqueExpress = "ID";
            this.WorkAlias = "MachineGroup";
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
        private Win.UI.DisplayBox displayDescription;
        private Win.UI.DisplayBox displayCode;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelCode;
        private Win.UI.CheckBox chkIsBD;
        private Win.UI.CheckBox chkIsSew;
        private Win.UI.CheckBox chkDP;
    }
}
