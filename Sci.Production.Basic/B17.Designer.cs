namespace Sci.Production.Basic
{
    partial class B17
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
         this.checkJunk = new Sci.Win.UI.CheckBox();
         this.labelID = new Sci.Win.UI.Label();
         this.txtID = new Sci.Win.UI.TextBox();
         this.labelDESC = new Sci.Win.UI.Label();
         this.editDESC = new Sci.Win.UI.EditBox();
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
         this.detail.Size = new System.Drawing.Size(912, 395);
         // 
         // detailcont
         // 
         this.detailcont.Controls.Add(this.editDESC);
         this.detailcont.Controls.Add(this.labelDESC);
         this.detailcont.Controls.Add(this.txtID);
         this.detailcont.Controls.Add(this.checkJunk);
         this.detailcont.Controls.Add(this.labelID);
         this.detailcont.Size = new System.Drawing.Size(912, 357);
         // 
         // detailbtm
         // 
         this.detailbtm.Size = new System.Drawing.Size(912, 38);
         // 
         // browse
         // 
         this.browse.Size = new System.Drawing.Size(912, 395);
         // 
         // tabs
         // 
         this.tabs.Size = new System.Drawing.Size(920, 424);
         // 
         // checkJunk
         // 
         this.checkJunk.AutoSize = true;
         this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "junk", true));
         this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
         this.checkJunk.Location = new System.Drawing.Point(284, 41);
         this.checkJunk.Name = "checkJunk";
         this.checkJunk.Size = new System.Drawing.Size(57, 21);
         this.checkJunk.TabIndex = 7;
         this.checkJunk.Text = "Junk";
         this.checkJunk.UseVisualStyleBackColor = true;
         // 
         // labelID
         // 
         this.labelID.Location = new System.Drawing.Point(61, 39);
         this.labelID.Name = "labelID";
         this.labelID.Size = new System.Drawing.Size(75, 23);
         this.labelID.TabIndex = 5;
         this.labelID.Text = "ID";
         // 
         // txtID
         // 
         this.txtID.BackColor = System.Drawing.Color.White;
         this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
         this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
         this.txtID.Location = new System.Drawing.Point(139, 39);
         this.txtID.Name = "txtID";
         this.txtID.Size = new System.Drawing.Size(110, 23);
         this.txtID.TabIndex = 8;
         // 
         // labelDESC
         // 
         this.labelDESC.Location = new System.Drawing.Point(61, 80);
         this.labelDESC.Name = "labelDESC";
         this.labelDESC.Size = new System.Drawing.Size(75, 23);
         this.labelDESC.TabIndex = 9;
         this.labelDESC.Text = "Description";
         // 
         // editDESC
         // 
         this.editDESC.BackColor = System.Drawing.Color.White;
         this.editDESC.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
         this.editDESC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
         this.editDESC.Location = new System.Drawing.Point(139, 80);
         this.editDESC.Multiline = true;
         this.editDESC.Name = "editDESC";
         this.editDESC.Size = new System.Drawing.Size(345, 97);
         this.editDESC.TabIndex = 11;
         // 
         // B17
         // 
         this.ClientSize = new System.Drawing.Size(920, 457);
         this.IsSupportClip = false;
         this.IsSupportCopy = false;
         this.IsSupportDelete = false;
         this.IsSupportPrint = false;
         this.Name = "B17";
         this.Text = "B17.Local Unit";
         this.WorkAlias = "LocalUnit";
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
        private Win.UI.Label labelID;
        private Win.UI.Label labelDESC;
        private Win.UI.TextBox txtID;
        private Win.UI.EditBox editDESC;
    }
}
