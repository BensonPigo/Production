namespace Sci.Production.IE
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
            this.components = new System.ComponentModel.Container();
            this.lbCategory = new Sci.Win.UI.Label();
            this.lbSendMail = new Sci.Win.UI.Label();
            this.lbStyleType = new Sci.Win.UI.Label();
            this.txtCategory = new Sci.Win.UI.TextBox();
            this.txtSendMail = new Sci.Win.UI.TextBox();
            this.cboStyleType = new Sci.Win.UI.ComboBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.lbNotice = new System.Windows.Forms.Label();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.gridDetailBs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.gridBase = new Sci.Win.UI.Grid();
            this.gridBaseBs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.BtnAdd = new Sci.Win.UI.Button();
            this.BtnDelete = new Sci.Win.UI.Button();
            this.labTotal1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.labDetailCount1 = new Sci.Win.UI.Label();
            this.labDetailCount2 = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetailBs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBaseBs)).BeginInit();
            this.SuspendLayout();
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.labDetailCount2);
            this.detailcont.Controls.Add(this.labDetailCount1);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.labTotal1);
            this.detailcont.Controls.Add(this.BtnDelete);
            this.detailcont.Controls.Add(this.BtnAdd);
            this.detailcont.Controls.Add(this.gridBase);
            this.detailcont.Controls.Add(this.gridDetail);
            this.detailcont.Controls.Add(this.lbNotice);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.cboStyleType);
            this.detailcont.Controls.Add(this.txtSendMail);
            this.detailcont.Controls.Add(this.txtCategory);
            this.detailcont.Controls.Add(this.lbStyleType);
            this.detailcont.Controls.Add(this.lbSendMail);
            this.detailcont.Controls.Add(this.lbCategory);
            this.detailcont.TabIndex = 0;
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1062, 438);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1070, 467);
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
            // lbCategory
            // 
            this.lbCategory.Location = new System.Drawing.Point(27, 30);
            this.lbCategory.Name = "lbCategory";
            this.lbCategory.Size = new System.Drawing.Size(103, 23);
            this.lbCategory.TabIndex = 0;
            this.lbCategory.Text = "Category";
            // 
            // lbSendMail
            // 
            this.lbSendMail.Location = new System.Drawing.Point(423, 30);
            this.lbSendMail.Name = "lbSendMail";
            this.lbSendMail.Size = new System.Drawing.Size(122, 23);
            this.lbSendMail.TabIndex = 1;
            this.lbSendMail.Text = "Send Alert Mail to";
            // 
            // lbStyleType
            // 
            this.lbStyleType.Location = new System.Drawing.Point(27, 60);
            this.lbStyleType.Name = "lbStyleType";
            this.lbStyleType.Size = new System.Drawing.Size(103, 23);
            this.lbStyleType.TabIndex = 4;
            this.lbStyleType.Text = "Style Type";
            // 
            // txtCategory
            // 
            this.txtCategory.BackColor = System.Drawing.Color.White;
            this.txtCategory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Category", true));
            this.txtCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCategory.Location = new System.Drawing.Point(134, 30);
            this.txtCategory.MaxLength = 1;
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(50, 23);
            this.txtCategory.TabIndex = 0;
            // 
            // txtSendMail
            // 
            this.txtSendMail.BackColor = System.Drawing.Color.White;
            this.txtSendMail.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtSendMail.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SendMail", true));
            this.txtSendMail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSendMail.Location = new System.Drawing.Point(548, 30);
            this.txtSendMail.Name = "txtSendMail";
            this.txtSendMail.Size = new System.Drawing.Size(404, 23);
            this.txtSendMail.TabIndex = 1;
            // 
            // cboStyleType
            // 
            this.cboStyleType.BackColor = System.Drawing.Color.White;
            this.cboStyleType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "StyleType", true));
            this.cboStyleType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboStyleType.FormattingEnabled = true;
            this.cboStyleType.IsSupportUnselect = true;
            this.cboStyleType.Location = new System.Drawing.Point(134, 60);
            this.cboStyleType.Name = "cboStyleType";
            this.cboStyleType.OldText = "";
            this.cboStyleType.Size = new System.Drawing.Size(92, 24);
            this.cboStyleType.TabIndex = 3;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(211, 32);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 5;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // lbNotice
            // 
            this.lbNotice.AutoSize = true;
            this.lbNotice.Location = new System.Drawing.Point(548, 56);
            this.lbNotice.Name = "lbNotice";
            this.lbNotice.Size = new System.Drawing.Size(226, 17);
            this.lbNotice.TabIndex = 6;
            this.lbNotice.Text = "Use a semicolon (;) for separation.";
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.DataSource = this.gridDetailBs;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(453, 116);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(491, 218);
            this.gridDetail.TabIndex = 21;
            // 
            // gridBase
            // 
            this.gridBase.AllowUserToAddRows = false;
            this.gridBase.AllowUserToDeleteRows = false;
            this.gridBase.AllowUserToResizeRows = false;
            this.gridBase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gridBase.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBase.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBase.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBase.DataSource = this.gridBaseBs;
            this.gridBase.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBase.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBase.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBase.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBase.Location = new System.Drawing.Point(8, 116);
            this.gridBase.Name = "gridBase";
            this.gridBase.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBase.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBase.RowTemplate.Height = 24;
            this.gridBase.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBase.ShowCellToolTips = false;
            this.gridBase.Size = new System.Drawing.Size(392, 218);
            this.gridBase.TabIndex = 22;
            // 
            // BtnAdd
            // 
            this.BtnAdd.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.BtnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.BtnAdd.Location = new System.Drawing.Point(406, 160);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(41, 30);
            this.BtnAdd.TabIndex = 23;
            this.BtnAdd.Text = ">";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.BtnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.BtnDelete.Location = new System.Drawing.Point(406, 234);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(41, 30);
            this.BtnDelete.TabIndex = 24;
            this.BtnDelete.Text = "<";
            this.BtnDelete.UseVisualStyleBackColor = true;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // labTotal1
            // 
            this.labTotal1.BackColor = System.Drawing.Color.Transparent;
            this.labTotal1.Location = new System.Drawing.Point(8, 90);
            this.labTotal1.Name = "labTotal1";
            this.labTotal1.Size = new System.Drawing.Size(68, 23);
            this.labTotal1.TabIndex = 25;
            this.labTotal1.Text = "Total No：";
            this.labTotal1.TextStyle.Alignment = System.Drawing.ContentAlignment.BottomLeft;
            this.labTotal1.TextStyle.BorderColor = System.Drawing.Color.White;
            this.labTotal1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(453, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 23);
            this.label2.TabIndex = 26;
            this.label2.Text = "Total No：";
            this.label2.TextStyle.Alignment = System.Drawing.ContentAlignment.BottomLeft;
            this.label2.TextStyle.BorderColor = System.Drawing.Color.White;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labDetailCount1
            // 
            this.labDetailCount1.BackColor = System.Drawing.Color.Transparent;
            this.labDetailCount1.Location = new System.Drawing.Point(76, 90);
            this.labDetailCount1.Name = "labDetailCount1";
            this.labDetailCount1.Size = new System.Drawing.Size(68, 23);
            this.labDetailCount1.TabIndex = 27;
            this.labDetailCount1.TextStyle.Alignment = System.Drawing.ContentAlignment.BottomLeft;
            this.labDetailCount1.TextStyle.BorderColor = System.Drawing.Color.White;
            this.labDetailCount1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labDetailCount2
            // 
            this.labDetailCount2.BackColor = System.Drawing.Color.Transparent;
            this.labDetailCount2.Location = new System.Drawing.Point(521, 90);
            this.labDetailCount2.Name = "labDetailCount2";
            this.labDetailCount2.Size = new System.Drawing.Size(68, 23);
            this.labDetailCount2.TabIndex = 28;
            this.labDetailCount2.TextStyle.Alignment = System.Drawing.ContentAlignment.BottomLeft;
            this.labDetailCount2.TextStyle.BorderColor = System.Drawing.Color.White;
            this.labDetailCount2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // B01
            // 
            this.ClientSize = new System.Drawing.Size(1070, 500);
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B01";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B01. Changeover Check List";
            this.WorkAlias = "ChgOverCheckList";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetailBs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBaseBs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.CheckBox checkJunk;
        private Win.UI.ComboBox cboStyleType;
        private Win.UI.TextBox txtSendMail;
        private Win.UI.TextBox txtCategory;
        private Win.UI.Label lbStyleType;
        private Win.UI.Label lbSendMail;
        private Win.UI.Label lbCategory;
        private System.Windows.Forms.Label lbNotice;
        private Win.UI.Grid gridBase;
        private Win.UI.Grid gridDetail;
        private Win.UI.ListControlBindingSource gridDetailBs;
        private Win.UI.Button BtnDelete;
        private Win.UI.Button BtnAdd;
        private Win.UI.ListControlBindingSource gridBaseBs;
        private Win.UI.Label labDetailCount2;
        private Win.UI.Label labDetailCount1;
        private Win.UI.Label label2;
        private Win.UI.Label labTotal1;
    }
}
