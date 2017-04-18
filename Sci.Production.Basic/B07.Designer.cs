namespace Sci.Production.Basic
{
    partial class B07
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
            this.labelTerm = new Sci.Win.UI.Label();
            this.txtID = new Sci.Win.UI.TextBox();
            this.txtTerm = new Sci.Win.UI.TextBox();
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
            this.detailcont.Controls.Add(this.txtTerm);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.labelTerm);
            this.detailcont.Controls.Add(this.labelID);
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
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelID
            // 
            this.labelID.Lines = 0;
            this.labelID.Location = new System.Drawing.Point(60, 27);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(58, 23);
            this.labelID.TabIndex = 0;
            this.labelID.Text = "ID";
            // 
            // labelTerm
            // 
            this.labelTerm.Lines = 0;
            this.labelTerm.Location = new System.Drawing.Point(60, 75);
            this.labelTerm.Name = "labelTerm";
            this.labelTerm.Size = new System.Drawing.Size(58, 23);
            this.labelTerm.TabIndex = 1;
            this.labelTerm.Text = "Term";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtID.Location = new System.Drawing.Point(121, 27);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(50, 23);
            this.txtID.TabIndex = 0;
            // 
            // txtTerm
            // 
            this.txtTerm.BackColor = System.Drawing.Color.White;
            this.txtTerm.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtTerm.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Name", true));
            this.txtTerm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTerm.Location = new System.Drawing.Point(122, 75);
            this.txtTerm.Name = "txtTerm";
            this.txtTerm.Size = new System.Drawing.Size(400, 23);
            this.txtTerm.TabIndex = 1;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(400, 27);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 2;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // B07
            // 
            this.ClientSize = new System.Drawing.Size(837, 457);
            this.DefaultControl = "textBox1";
            this.DefaultControlForEdit = "textBox2";
            this.DefaultOrder = "ID";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B07";
            this.Text = "B07. Payment Term";
            this.UniqueExpress = "ID";
            this.WorkAlias = "PayTerm";
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

        private Win.UI.TextBox txtTerm;
        private Win.UI.TextBox txtID;
        private Win.UI.Label labelTerm;
        private Win.UI.Label labelID;
        private Win.UI.CheckBox checkJunk;
    }
}
