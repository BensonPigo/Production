namespace Sci.Production.Centralized
{
    partial class IE_B01
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
            this.labelNo = new Sci.Win.UI.Label();
            this.labelCheckList = new Sci.Win.UI.Label();
            this.chkJunk = new Sci.Win.UI.CheckBox();
            this.txtCheckList = new Sci.Win.UI.TextBox();
            this.txtNo = new Sci.Win.UI.NumericBox();
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
            this.detail.Size = new System.Drawing.Size(828, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtNo);
            this.detailcont.Controls.Add(this.txtCheckList);
            this.detailcont.Controls.Add(this.chkJunk);
            this.detailcont.Controls.Add(this.labelCheckList);
            this.detailcont.Controls.Add(this.labelNo);
            this.detailcont.Size = new System.Drawing.Size(828, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(828, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(828, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(836, 424);
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
            // labelNo
            // 
            this.labelNo.Location = new System.Drawing.Point(70, 37);
            this.labelNo.Name = "labelNo";
            this.labelNo.Size = new System.Drawing.Size(85, 23);
            this.labelNo.TabIndex = 0;
            this.labelNo.Text = "No";
            // 
            // labelCheckList
            // 
            this.labelCheckList.Location = new System.Drawing.Point(70, 84);
            this.labelCheckList.Name = "labelCheckList";
            this.labelCheckList.Size = new System.Drawing.Size(85, 23);
            this.labelCheckList.TabIndex = 1;
            this.labelCheckList.Text = "Check List";
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkJunk.Location = new System.Drawing.Point(364, 39);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(57, 21);
            this.chkJunk.TabIndex = 4;
            this.chkJunk.Text = "Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // txtCheckList
            // 
            this.txtCheckList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtCheckList.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtCheckList.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CheckList", true));
            this.txtCheckList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtCheckList.Location = new System.Drawing.Point(158, 84);
            this.txtCheckList.Name = "txtCheckList";
            this.txtCheckList.ReadOnly = true;
            this.txtCheckList.Size = new System.Drawing.Size(590, 23);
            this.txtCheckList.TabIndex = 53;
            // 
            // txtNo
            // 
            this.txtNo.BackColor = System.Drawing.Color.White;
            this.txtNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "No", true));
            this.txtNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtNo.Location = new System.Drawing.Point(158, 37);
            this.txtNo.Name = "txtNo";
            this.txtNo.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtNo.Size = new System.Drawing.Size(93, 23);
            this.txtNo.TabIndex = 54;
            this.txtNo.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // IE_B01
            // 
            this.ClientSize = new System.Drawing.Size(836, 457);
            this.ConnectionName = "ProductionTPE";
            this.DefaultControlForEdit = "txtNo,txtCheckList";
            this.DefaultOrder = "No";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "IE_B01";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B01. Changeover Check List";
            this.UniqueExpress = "No,CheckList";
            this.WorkAlias = "ChgOverCheckListBase";
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
        private Win.UI.Label labelCheckList;
        private Win.UI.Label labelNo;
        private Win.UI.CheckBox chkJunk;
        private Win.UI.TextBox txtCheckList;
        private Win.UI.NumericBox txtNo;
    }
}
