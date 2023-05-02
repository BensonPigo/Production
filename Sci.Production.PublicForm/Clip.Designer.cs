namespace Sci.Production.PublicForm
{
    partial class Clip
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
            this.nem = new Sci.Win.UI.Button();
            this.remove = new Sci.Win.UI.Button();
            this.close = new Sci.Win.UI.Button();
            this.mailto = new Sci.Win.UI.Button();
            this.download = new Sci.Win.UI.Button();
            this.openfile = new Sci.Win.UI.Button();
            this.loc = new Sci.Win.UI.Label();
            this.btm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            this.SuspendLayout();
            // 
            // top
            // 
            this.top.Size = new System.Drawing.Size(1006, 10);
            // 
            // btm
            // 
            this.btm.Controls.Add(this.download);
            this.btm.Controls.Add(this.openfile);
            this.btm.Controls.Add(this.mailto);
            this.btm.Controls.Add(this.close);
            this.btm.Controls.Add(this.remove);
            this.btm.Controls.Add(this.nem);
            this.btm.Controls.Add(this.loc);
            this.btm.Location = new System.Drawing.Point(10, 367);
            this.btm.Size = new System.Drawing.Size(1006, 45);
            // 
            // left
            // 
            this.left.Size = new System.Drawing.Size(10, 412);
            // 
            // right
            // 
            this.right.Location = new System.Drawing.Point(1016, 0);
            this.right.Size = new System.Drawing.Size(10, 412);
            // 
            // cont
            // 
            this.cont.Size = new System.Drawing.Size(1006, 357);
            // 
            // nem
            // 
            this.nem.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.nem.Location = new System.Drawing.Point(7, 7);
            this.nem.Name = "nem";
            this.nem.Size = new System.Drawing.Size(80, 30);
            this.nem.TabIndex = 1;
            this.nem.Text = "New";
            this.nem.UseVisualStyleBackColor = true;
            // 
            // remove
            // 
            this.remove.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.remove.Location = new System.Drawing.Point(93, 7);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(80, 30);
            this.remove.TabIndex = 2;
            this.remove.Text = "Remove";
            this.remove.UseVisualStyleBackColor = true;
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.close.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.close.Location = new System.Drawing.Point(920, 7);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(80, 30);
            this.close.TabIndex = 6;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // mailto
            // 
            this.mailto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mailto.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.mailto.Location = new System.Drawing.Point(834, 7);
            this.mailto.Name = "mailto";
            this.mailto.Size = new System.Drawing.Size(80, 30);
            this.mailto.TabIndex = 5;
            this.mailto.Text = "Mail to";
            this.mailto.UseVisualStyleBackColor = true;
            this.mailto.Visible = false;
            // 
            // download
            // 
            this.download.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.download.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.download.Location = new System.Drawing.Point(734, 7);
            this.download.Name = "download";
            this.download.Size = new System.Drawing.Size(94, 30);
            this.download.TabIndex = 4;
            this.download.Text = "Download";
            this.download.UseVisualStyleBackColor = true;
            // 
            // openfile
            // 
            this.openfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.openfile.Location = new System.Drawing.Point(638, 7);
            this.openfile.Name = "openfile";
            this.openfile.Size = new System.Drawing.Size(90, 30);
            this.openfile.TabIndex = 3;
            this.openfile.Text = "Open file";
            this.openfile.UseVisualStyleBackColor = true;
            // 
            // loc
            // 
            this.loc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loc.Location = new System.Drawing.Point(176, 12);
            this.loc.Name = "loc";
            this.loc.Size = new System.Drawing.Size(459, 23);
            this.loc.TabIndex = 1;
            this.loc.Text = "loc";
            // 
            // Clip
            // 
            this.ClientSize = new System.Drawing.Size(1026, 412);
            this.Name = "Clip";
            this.OnLineHelpID = "Sci.Win.Tools.BaseGrid";
            this.btm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button nem;
        private Win.UI.Button remove;
        private Win.UI.Button mailto;
        private Win.UI.Button close;
        private Win.UI.Button download;
        private Win.UI.Button openfile;
        private Win.UI.Label loc;
    }
}
