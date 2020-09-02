namespace Sci.Production.Quality
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
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.labelCode = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.txtReson = new Sci.Win.UI.TextBox();
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
            this.detail.Size = new System.Drawing.Size(833, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtReson);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.txtRefno);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Size = new System.Drawing.Size(833, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(833, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(833, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(841, 424);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(475, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(427, 13);
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(415, 50);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 2;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "RefNo", true));
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(151, 48);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(168, 23);
            this.txtRefno.TabIndex = 0;
            this.txtRefno.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtRefno_PopUp);
            this.txtRefno.Validating += new System.ComponentModel.CancelEventHandler(this.TxtRefno_Validating);
            // 
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(70, 48);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(78, 23);
            this.labelCode.TabIndex = 12;
            this.labelCode.Text = "RefNo";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(70, 82);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(78, 23);
            this.labelDescription.TabIndex = 13;
            this.labelDescription.Text = "Reason";
            // 
            // txtReson
            // 
            this.txtReson.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtReson.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtReson.IsSupportEditMode = false;
            this.txtReson.Location = new System.Drawing.Point(151, 82);
            this.txtReson.Name = "txtReson";
            this.txtReson.ReadOnly = true;
            this.txtReson.Size = new System.Drawing.Size(395, 23);
            this.txtReson.TabIndex = 1;
            // 
            // B08
            // 
            this.ClientSize = new System.Drawing.Size(841, 457);
            this.DefaultControl = "txtRefno";
            this.DefaultControlForEdit = "txtRefno";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B08";
            this.Text = "B08. Shrinkage Concern";
            this.WorkAlias = "ShrinkageConcern";
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
        private Win.UI.TextBox txtRefno;
        private Win.UI.Label labelCode;
        private Win.UI.Label labelDescription;
        private Win.UI.TextBox txtReson;
    }
}
