namespace Sci.Production.Logistic
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelCode = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.txtCode = new Sci.Win.UI.TextBox();
            this.txtDescription = new Sci.Win.UI.TextBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
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
            this.detail.Size = new System.Drawing.Size(829, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.txtDescription);
            this.detailcont.Controls.Add(this.txtCode);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Size = new System.Drawing.Size(829, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(829, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(829, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(837, 424);
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
            this.labelCode.Location = new System.Drawing.Point(47, 41);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(75, 23);
            this.labelCode.TabIndex = 3;
            this.labelCode.Text = "Code";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(47, 89);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Description";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCode.Location = new System.Drawing.Point(126, 41);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(80, 23);
            this.txtCode.TabIndex = 0;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDescription.Location = new System.Drawing.Point(126, 89);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(260, 23);
            this.txtDescription.TabIndex = 1;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(325, 41);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 2;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // B01
            // 
            this.ClientSize = new System.Drawing.Size(837, 457);
            this.DefaultControl = "txtCode";
            this.DefaultControlForEdit = "txtCode";
            this.DefaultOrder = "ID";
            this.Name = "B01";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B01. Clog Location Index";
            this.UniqueExpress = "ID";
            this.WorkAlias = "ClogLocation";
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

        private Win.UI.TextBox txtDescription;
        private Win.UI.TextBox txtCode;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelCode;
        private Win.UI.CheckBox checkJunk;
    }
}
