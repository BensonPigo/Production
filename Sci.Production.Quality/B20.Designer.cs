namespace Sci.Production.Quality
{
    partial class B20
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelDefectType = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.txtDefectType = new Sci.Win.UI.TextBox();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.editLocalDesc = new Sci.Win.UI.EditBox();
            this.labLocalDesc = new Sci.Win.UI.Label();
            this.btnSettingSort = new Sci.Win.UI.Button();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.browse.SuspendLayout();
            this.tabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.grid1);
            this.detailcont.Controls.Add(this.editLocalDesc);
            this.detailcont.Controls.Add(this.labLocalDesc);
            this.detailcont.Controls.Add(this.editDescription);
            this.detailcont.Controls.Add(this.txtDefectType);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.labelDefectType);
            this.detailcont.Controls.Add(this.labelDescription);
            // 
            // browse
            // 
            this.browse.Controls.Add(this.btnSettingSort);
            this.browse.Size = new System.Drawing.Size(826, 395);
            this.browse.Controls.SetChildIndex(this.btnSettingSort, 0);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(834, 424);
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
            // labelDefectType
            // 
            this.labelDefectType.Location = new System.Drawing.Point(16, 15);
            this.labelDefectType.Name = "labelDefectType";
            this.labelDefectType.Size = new System.Drawing.Size(85, 23);
            this.labelDefectType.TabIndex = 4;
            this.labelDefectType.Text = "Defect Type";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(16, 50);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(85, 23);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "Description";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(297, 17);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 2;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // txtDefectType
            // 
            this.txtDefectType.BackColor = System.Drawing.Color.White;
            this.txtDefectType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtDefectType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDefectType.Location = new System.Drawing.Point(104, 15);
            this.txtDefectType.Name = "txtDefectType";
            this.txtDefectType.Size = new System.Drawing.Size(132, 23);
            this.txtDefectType.TabIndex = 0;
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(104, 50);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(293, 64);
            this.editDescription.TabIndex = 1;
            // 
            // editLocalDesc
            // 
            this.editLocalDesc.BackColor = System.Drawing.Color.White;
            this.editLocalDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "LocalDescription", true));
            this.editLocalDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editLocalDesc.Location = new System.Drawing.Point(512, 50);
            this.editLocalDesc.Multiline = true;
            this.editLocalDesc.Name = "editLocalDesc";
            this.editLocalDesc.Size = new System.Drawing.Size(293, 64);
            this.editLocalDesc.TabIndex = 6;
            // 
            // labLocalDesc
            // 
            this.labLocalDesc.Location = new System.Drawing.Point(424, 50);
            this.labLocalDesc.Name = "labLocalDesc";
            this.labLocalDesc.Size = new System.Drawing.Size(85, 23);
            this.labLocalDesc.TabIndex = 7;
            this.labLocalDesc.Text = "Local Desc.";
            // 
            // btnSettingSort
            // 
            this.btnSettingSort.Enabled = false;
            this.btnSettingSort.Location = new System.Drawing.Point(627, 5);
            this.btnSettingSort.Name = "btnSettingSort";
            this.btnSettingSort.Size = new System.Drawing.Size(117, 30);
            this.btnSettingSort.TabIndex = 3;
            this.btnSettingSort.Text = "Setting Sort";
            this.btnSettingSort.UseVisualStyleBackColor = true;
            this.btnSettingSort.Click += new System.EventHandler(this.BtnSettingSort_Click);
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(8, 120);
            this.grid1.Name = "grid1";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(881, 231);
            this.grid1.TabIndex = 8;
            // 
            // B20
            // 
            this.ClientSize = new System.Drawing.Size(834, 457);
            this.DefaultControl = "txtDefectType";
            this.DefaultControlForEdit = "editDescription";
            this.DefaultOrder = "Seq";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B20";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B20. Defect type for RFT/CFA(Garment)";
            this.WorkAlias = "GarmentDefectType";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.browse.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox txtDefectType;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label labelDefectType;
        private Win.UI.Label labelDescription;
        private Win.UI.EditBox editDescription;
        private Win.UI.EditBox editLocalDesc;
        private Win.UI.Label labLocalDesc;
        private Win.UI.Button btnSettingSort;
        private Win.UI.Grid grid1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}
