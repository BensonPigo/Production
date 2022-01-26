namespace Sci.Production.Quality
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
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.labeltype = new Sci.Win.UI.Label();
            this.txttype = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.editBox1 = new Sci.Win.UI.EditBox();
            this.groupDefectPicture = new Sci.Win.UI.GroupBox();
            this.btnRemove = new Sci.Win.UI.Button();
            this.btnUploadDefectPicture = new Sci.Win.UI.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cmbDefectPicture = new Sci.Win.UI.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.groupDefectPicture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(804, 532);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.groupDefectPicture);
            this.detailcont.Controls.Add(this.editBox1);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.editDescription);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.txttype);
            this.detailcont.Controls.Add(this.txtCode);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labeltype);
            this.detailcont.Size = new System.Drawing.Size(804, 494);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 494);
            this.detailbtm.Size = new System.Drawing.Size(804, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(829, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(812, 561);
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
            this.labelCode.Location = new System.Drawing.Point(70, 34);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(80, 23);
            this.labelCode.TabIndex = 4;
            this.labelCode.Text = "Code";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(70, 98);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(80, 23);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "Description";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCode.Location = new System.Drawing.Point(153, 34);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(100, 23);
            this.txtCode.TabIndex = 3;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(277, 36);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 4;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescriptionEN", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(153, 98);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(292, 50);
            this.editDescription.TabIndex = 5;
            // 
            // labeltype
            // 
            this.labeltype.Location = new System.Drawing.Point(70, 66);
            this.labeltype.Name = "labeltype";
            this.labeltype.Size = new System.Drawing.Size(80, 23);
            this.labeltype.TabIndex = 6;
            this.labeltype.Text = "type";
            // 
            // txttype
            // 
            this.txttype.BackColor = System.Drawing.Color.White;
            this.txttype.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Type", true));
            this.txttype.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txttype.Location = new System.Drawing.Point(153, 66);
            this.txttype.Name = "txttype";
            this.txttype.Size = new System.Drawing.Size(100, 23);
            this.txttype.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(67, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "Local Desc.";
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.White;
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "LocalDesc", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox1.Location = new System.Drawing.Point(153, 154);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(292, 50);
            this.editBox1.TabIndex = 8;
            // 
            // groupDefectPicture
            // 
            this.groupDefectPicture.Controls.Add(this.btnRemove);
            this.groupDefectPicture.Controls.Add(this.btnUploadDefectPicture);
            this.groupDefectPicture.Controls.Add(this.pictureBox1);
            this.groupDefectPicture.Controls.Add(this.cmbDefectPicture);
            this.groupDefectPicture.Location = new System.Drawing.Point(471, 3);
            this.groupDefectPicture.Name = "groupDefectPicture";
            this.groupDefectPicture.Size = new System.Drawing.Size(324, 472);
            this.groupDefectPicture.TabIndex = 140;
            this.groupDefectPicture.TabStop = false;
            this.groupDefectPicture.Text = "Defect Picture";
            // 
            // btnRemove
            // 
            this.btnRemove.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnRemove.Location = new System.Drawing.Point(111, 23);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(99, 30);
            this.btnRemove.TabIndex = 139;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            // 
            // btnUploadDefectPicture
            // 
            this.btnUploadDefectPicture.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnUploadDefectPicture.Location = new System.Drawing.Point(6, 23);
            this.btnUploadDefectPicture.Name = "btnUploadDefectPicture";
            this.btnUploadDefectPicture.Size = new System.Drawing.Size(99, 30);
            this.btnUploadDefectPicture.TabIndex = 136;
            this.btnUploadDefectPicture.Text = "Upload";
            this.btnUploadDefectPicture.UseVisualStyleBackColor = true;
            this.btnUploadDefectPicture.Click += new System.EventHandler(this.BtnUploadDefectPicture_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(6, 95);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(311, 371);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 138;
            this.pictureBox1.TabStop = false;
            // 
            // cmbDefectPicture
            // 
            this.cmbDefectPicture.BackColor = System.Drawing.Color.White;
            this.cmbDefectPicture.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.cmbDefectPicture.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbDefectPicture.FormattingEnabled = true;
            this.cmbDefectPicture.IsSupportUnselect = true;
            this.cmbDefectPicture.Location = new System.Drawing.Point(6, 63);
            this.cmbDefectPicture.Name = "cmbDefectPicture";
            this.cmbDefectPicture.OldText = "";
            this.cmbDefectPicture.Size = new System.Drawing.Size(309, 24);
            this.cmbDefectPicture.TabIndex = 136;
            this.cmbDefectPicture.SelectedIndexChanged += new System.EventHandler(this.CmbDefectPicture_SelectedIndexChanged);
            // 
            // B01
            // 
            this.ClientSize = new System.Drawing.Size(812, 594);
            this.DefaultControl = "txtCode";
            this.DefaultControlForEdit = "editDescription";
            this.IsDeleteOnBrowse = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportLocate = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B01";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B01. Defect Code For Fabric Inspection";
            this.WorkAlias = "FabricDefect";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.groupDefectPicture.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.CheckBox checkJunk;
        private Win.UI.TextBox txtCode;
        private Win.UI.Label labelCode;
        private Win.UI.Label labelDescription;
        private Win.UI.EditBox editDescription;
        private Win.UI.TextBox txttype;
        private Win.UI.Label labeltype;
        private Win.UI.EditBox editBox1;
        private Win.UI.Label label1;
        private Win.UI.GroupBox groupDefectPicture;
        private Win.UI.Button btnRemove;
        private Win.UI.Button btnUploadDefectPicture;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Win.UI.ComboBox cmbDefectPicture;
    }
}
