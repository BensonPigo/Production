namespace Sci.Production.Quality
{
    partial class B15
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
            this.labefactory = new Sci.Win.UI.Label();
            this.labeDecription = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.lbSubprocess = new Sci.Win.UI.Label();
            this.txtSubProcessID = new Sci.Win.UI.TextBox();
            this.editBoxDescription = new Sci.Win.UI.EditBox();
            this.txtID = new Sci.Win.UI.TextBox();
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
            this.detail.Size = new System.Drawing.Size(818, 258);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.editBoxDescription);
            this.detailcont.Controls.Add(this.txtSubProcessID);
            this.detailcont.Controls.Add(this.lbSubprocess);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.labeDecription);
            this.detailcont.Controls.Add(this.labefactory);
            this.detailcont.Size = new System.Drawing.Size(818, 220);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 220);
            this.detailbtm.Size = new System.Drawing.Size(818, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(818, 258);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(826, 287);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            this.createby.Visible = false;
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            this.editby.Visible = false;
            // 
            // lblcreateby
            // 
            this.lblcreateby.Visible = false;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            this.lbleditby.Visible = false;
            // 
            // labefactory
            // 
            this.labefactory.Location = new System.Drawing.Point(27, 15);
            this.labefactory.Name = "labefactory";
            this.labefactory.Size = new System.Drawing.Size(108, 23);
            this.labefactory.TabIndex = 0;
            this.labefactory.Text = "Response Team";
            // 
            // labeDecription
            // 
            this.labeDecription.Location = new System.Drawing.Point(27, 73);
            this.labeDecription.Name = "labeDecription";
            this.labeDecription.Size = new System.Drawing.Size(108, 23);
            this.labeDecription.TabIndex = 3;
            this.labeDecription.Text = "Description";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.IsSupportEditMode = false;
            this.checkJunk.Location = new System.Drawing.Point(433, 17);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 15;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // lbSubprocess
            // 
            this.lbSubprocess.Location = new System.Drawing.Point(27, 45);
            this.lbSubprocess.Name = "lbSubprocess";
            this.lbSubprocess.Size = new System.Drawing.Size(108, 23);
            this.lbSubprocess.TabIndex = 17;
            this.lbSubprocess.Text = "SubProcess";
            // 
            // txtSubProcessID
            // 
            this.txtSubProcessID.BackColor = System.Drawing.Color.White;
            this.txtSubProcessID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SubProcessID", true));
            this.txtSubProcessID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSubProcessID.Location = new System.Drawing.Point(138, 44);
            this.txtSubProcessID.Name = "txtSubProcessID";
            this.txtSubProcessID.Size = new System.Drawing.Size(255, 23);
            this.txtSubProcessID.TabIndex = 19;
            this.txtSubProcessID.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSubProcessID_PopUp);
            this.txtSubProcessID.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSubProcessID_Validating);
            // 
            // editBoxDescription
            // 
            this.editBoxDescription.BackColor = System.Drawing.Color.White;
            this.editBoxDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editBoxDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBoxDescription.Location = new System.Drawing.Point(138, 73);
            this.editBoxDescription.Multiline = true;
            this.editBoxDescription.Name = "editBoxDescription";
            this.editBoxDescription.Size = new System.Drawing.Size(447, 99);
            this.editBoxDescription.TabIndex = 21;
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtID.Location = new System.Drawing.Point(138, 15);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(255, 23);
            this.txtID.TabIndex = 0;
            // 
            // B15
            // 
            this.ClientSize = new System.Drawing.Size(826, 320);
            this.EnableGridJunkColor = true;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B15";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B15. Sub-Process Response Team";
            this.WorkAlias = "SubProResponseTeam";
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
        private Win.UI.Label labeDecription;
        private Win.UI.Label labefactory;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label lbSubprocess;
        private Win.UI.EditBox editBoxDescription;
        private Win.UI.TextBox txtSubProcessID;
        private Win.UI.TextBox txtID;
    }
}
