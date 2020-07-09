namespace Sci.Production.Tools
{
    partial class PasswordByUser
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
            this.radioGroup1 = new Sci.Win.UI.RadioGroup();
            this.btnShowImg = new Sci.Win.UI.Button();
            this.btnSetPic = new Sci.Win.UI.Button();
            this.disBoxESignature = new Sci.Win.UI.DisplayBox();
            this.labESignature = new Sci.Win.UI.Label();
            this.checkAdmin = new Sci.Win.UI.CheckBox();
            this.txtPosition = new Sci.Win.UI.TextBox();
            this.txtUserDeputy = new Sci.Production.Class.Txtuser();
            this.txtUserSupervisor = new Sci.Production.Class.Txtuser();
            this.txtUserManager = new Sci.Production.Class.Txtuser();
            this.txtIDStart = new Sci.Win.UI.TextBox();
            this.dateResign = new Sci.Win.UI.DateBox();
            this.dateDateHired = new Sci.Win.UI.DateBox();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.comboLanguage = new Sci.Win.UI.ComboBox();
            this.txtEMailAddr = new Sci.Win.UI.TextBox();
            this.editFactory = new Sci.Win.UI.EditBox();
            this.txtExtNo = new Sci.Win.UI.TextBox();
            this.txtPassword = new Sci.Win.UI.TextBox();
            this.txtIDEnd = new Sci.Win.UI.TextBox();
            this.labelRemark = new Sci.Win.UI.Label();
            this.labelResign = new Sci.Win.UI.Label();
            this.labelLanguage = new Sci.Win.UI.Label();
            this.labelExtNo = new Sci.Win.UI.Label();
            this.labelDateHired = new Sci.Win.UI.Label();
            this.labelPosition = new Sci.Win.UI.Label();
            this.labelEMailAddr = new Sci.Win.UI.Label();
            this.labelDeputy = new Sci.Win.UI.Label();
            this.labelSupervisor = new Sci.Win.UI.Label();
            this.labelManager = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelPassword = new Sci.Win.UI.Label();
            this.labelID = new Sci.Win.UI.Label();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.radioGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(993, 572);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.grid1);
            this.detailcont.Controls.Add(this.radioGroup1);
            this.detailcont.Size = new System.Drawing.Size(993, 534);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 534);
            this.detailbtm.Size = new System.Drawing.Size(993, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(993, 572);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1001, 601);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(505, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(457, 13);
            // 
            // radioGroup1
            // 
            this.radioGroup1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radioGroup1.Controls.Add(this.btnShowImg);
            this.radioGroup1.Controls.Add(this.btnSetPic);
            this.radioGroup1.Controls.Add(this.disBoxESignature);
            this.radioGroup1.Controls.Add(this.labESignature);
            this.radioGroup1.Controls.Add(this.checkAdmin);
            this.radioGroup1.Controls.Add(this.txtPosition);
            this.radioGroup1.Controls.Add(this.txtUserDeputy);
            this.radioGroup1.Controls.Add(this.txtUserSupervisor);
            this.radioGroup1.Controls.Add(this.txtUserManager);
            this.radioGroup1.Controls.Add(this.txtIDStart);
            this.radioGroup1.Controls.Add(this.dateResign);
            this.radioGroup1.Controls.Add(this.dateDateHired);
            this.radioGroup1.Controls.Add(this.editRemark);
            this.radioGroup1.Controls.Add(this.comboLanguage);
            this.radioGroup1.Controls.Add(this.txtEMailAddr);
            this.radioGroup1.Controls.Add(this.editFactory);
            this.radioGroup1.Controls.Add(this.txtExtNo);
            this.radioGroup1.Controls.Add(this.txtPassword);
            this.radioGroup1.Controls.Add(this.txtIDEnd);
            this.radioGroup1.Controls.Add(this.labelRemark);
            this.radioGroup1.Controls.Add(this.labelResign);
            this.radioGroup1.Controls.Add(this.labelLanguage);
            this.radioGroup1.Controls.Add(this.labelExtNo);
            this.radioGroup1.Controls.Add(this.labelDateHired);
            this.radioGroup1.Controls.Add(this.labelPosition);
            this.radioGroup1.Controls.Add(this.labelEMailAddr);
            this.radioGroup1.Controls.Add(this.labelDeputy);
            this.radioGroup1.Controls.Add(this.labelSupervisor);
            this.radioGroup1.Controls.Add(this.labelManager);
            this.radioGroup1.Controls.Add(this.labelFactory);
            this.radioGroup1.Controls.Add(this.labelPassword);
            this.radioGroup1.Controls.Add(this.labelID);
            this.radioGroup1.Location = new System.Drawing.Point(10, 3);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(463, 671);
            this.radioGroup1.TabIndex = 3;
            this.radioGroup1.TabStop = false;
            // 
            // btnShowImg
            // 
            this.btnShowImg.Location = new System.Drawing.Point(293, 493);
            this.btnShowImg.Name = "btnShowImg";
            this.btnShowImg.Size = new System.Drawing.Size(80, 30);
            this.btnShowImg.TabIndex = 57;
            this.btnShowImg.Text = "Show";
            this.btnShowImg.UseVisualStyleBackColor = true;
            this.btnShowImg.Click += new System.EventHandler(this.btnShowImg_Click);
            // 
            // btnSetPic
            // 
            this.btnSetPic.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnSetPic.Location = new System.Drawing.Point(253, 493);
            this.btnSetPic.Name = "btnSetPic";
            this.btnSetPic.Size = new System.Drawing.Size(34, 30);
            this.btnSetPic.TabIndex = 56;
            this.btnSetPic.Text = "...";
            this.btnSetPic.UseVisualStyleBackColor = true;
            this.btnSetPic.Click += new System.EventHandler(this.btnSetPic_Click);
            // 
            // disBoxESignature
            // 
            this.disBoxESignature.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disBoxESignature.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ESignature", true));
            this.disBoxESignature.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disBoxESignature.Location = new System.Drawing.Point(96, 496);
            this.disBoxESignature.Name = "disBoxESignature";
            this.disBoxESignature.Size = new System.Drawing.Size(155, 23);
            this.disBoxESignature.TabIndex = 55;
            // 
            // labESignature
            // 
            this.labESignature.Location = new System.Drawing.Point(12, 496);
            this.labESignature.Name = "labESignature";
            this.labESignature.Size = new System.Drawing.Size(81, 23);
            this.labESignature.TabIndex = 54;
            this.labESignature.Text = "E- Signature";
            // 
            // checkAdmin
            // 
            this.checkAdmin.AutoSize = true;
            this.checkAdmin.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "IsAdmin", true));
            this.checkAdmin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkAdmin.Location = new System.Drawing.Point(96, 81);
            this.checkAdmin.Name = "checkAdmin";
            this.checkAdmin.Size = new System.Drawing.Size(66, 21);
            this.checkAdmin.TabIndex = 53;
            this.checkAdmin.Text = "Admin";
            this.checkAdmin.UseVisualStyleBackColor = true;
            // 
            // txtPosition
            // 
            this.txtPosition.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtPosition.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Position", true));
            this.txtPosition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtPosition.IsSupportEditMode = false;
            this.txtPosition.Location = new System.Drawing.Point(96, 297);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtPosition.ReadOnly = true;
            this.txtPosition.Size = new System.Drawing.Size(133, 23);
            this.txtPosition.TabIndex = 10;
            // 
            // txtUserDeputy
            // 
            this.txtUserDeputy.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Deputy", true));
            this.txtUserDeputy.DisplayBox1Binding = "";
            this.txtUserDeputy.Location = new System.Drawing.Point(96, 229);
            this.txtUserDeputy.Name = "txtUserDeputy";
            this.txtUserDeputy.Size = new System.Drawing.Size(302, 23);
            this.txtUserDeputy.TabIndex = 8;
            this.txtUserDeputy.TextBox1Binding = "";
            // 
            // txtUserSupervisor
            // 
            this.txtUserSupervisor.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Supervisor", true));
            this.txtUserSupervisor.DisplayBox1Binding = "";
            this.txtUserSupervisor.Location = new System.Drawing.Point(96, 197);
            this.txtUserSupervisor.Name = "txtUserSupervisor";
            this.txtUserSupervisor.Size = new System.Drawing.Size(302, 23);
            this.txtUserSupervisor.TabIndex = 7;
            this.txtUserSupervisor.TextBox1Binding = "";
            // 
            // txtUserManager
            // 
            this.txtUserManager.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Manager", true));
            this.txtUserManager.DisplayBox1Binding = "";
            this.txtUserManager.Location = new System.Drawing.Point(96, 164);
            this.txtUserManager.Name = "txtUserManager";
            this.txtUserManager.Size = new System.Drawing.Size(302, 23);
            this.txtUserManager.TabIndex = 6;
            this.txtUserManager.TextBox1Binding = "";
            // 
            // txtIDStart
            // 
            this.txtIDStart.BackColor = System.Drawing.Color.White;
            this.txtIDStart.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtIDStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtIDStart.IsSupportEditMode = false;
            this.txtIDStart.Location = new System.Drawing.Point(96, 19);
            this.txtIDStart.Name = "txtIDStart";
            this.txtIDStart.Size = new System.Drawing.Size(121, 23);
            this.txtIDStart.TabIndex = 1;
            // 
            // dateResign
            // 
            this.dateResign.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Resign", true));
            this.dateResign.Location = new System.Drawing.Point(322, 331);
            this.dateResign.Name = "dateResign";
            this.dateResign.Size = new System.Drawing.Size(130, 23);
            this.dateResign.TabIndex = 13;
            // 
            // dateDateHired
            // 
            this.dateDateHired.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "OnBoard", true));
            this.dateDateHired.Location = new System.Drawing.Point(96, 331);
            this.dateDateHired.Name = "dateDateHired";
            this.dateDateHired.Size = new System.Drawing.Size(130, 23);
            this.dateDateHired.TabIndex = 12;
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(96, 365);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(356, 125);
            this.editRemark.TabIndex = 14;
            // 
            // comboLanguage
            // 
            this.comboLanguage.BackColor = System.Drawing.Color.White;
            this.comboLanguage.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "CodePage", true));
            this.comboLanguage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboLanguage.FormattingEnabled = true;
            this.comboLanguage.IsSupportUnselect = true;
            this.comboLanguage.Location = new System.Drawing.Point(322, 297);
            this.comboLanguage.Name = "comboLanguage";
            this.comboLanguage.OldText = "";
            this.comboLanguage.Size = new System.Drawing.Size(121, 24);
            this.comboLanguage.TabIndex = 11;
            // 
            // txtEMailAddr
            // 
            this.txtEMailAddr.BackColor = System.Drawing.Color.White;
            this.txtEMailAddr.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtEMailAddr.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "EMail", true));
            this.txtEMailAddr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtEMailAddr.Location = new System.Drawing.Point(96, 263);
            this.txtEMailAddr.Name = "txtEMailAddr";
            this.txtEMailAddr.Size = new System.Drawing.Size(362, 23);
            this.txtEMailAddr.TabIndex = 9;
            this.txtEMailAddr.Validating += new System.ComponentModel.CancelEventHandler(this.txtEMailAddr_Validating);
            // 
            // editFactory
            // 
            this.editFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editFactory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Factory", true));
            this.editFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editFactory.IsSupportEditMode = false;
            this.editFactory.Location = new System.Drawing.Point(96, 108);
            this.editFactory.Multiline = true;
            this.editFactory.Name = "editFactory";
            this.editFactory.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.editFactory.ReadOnly = true;
            this.editFactory.Size = new System.Drawing.Size(356, 50);
            this.editFactory.TabIndex = 5;
            // 
            // txtExtNo
            // 
            this.txtExtNo.BackColor = System.Drawing.Color.White;
            this.txtExtNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ExtNo", true));
            this.txtExtNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtExtNo.Location = new System.Drawing.Point(322, 51);
            this.txtExtNo.Name = "txtExtNo";
            this.txtExtNo.Size = new System.Drawing.Size(110, 23);
            this.txtExtNo.TabIndex = 4;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Password", true));
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPassword.Location = new System.Drawing.Point(96, 51);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(121, 23);
            this.txtPassword.TabIndex = 3;
            // 
            // txtIDEnd
            // 
            this.txtIDEnd.BackColor = System.Drawing.Color.White;
            this.txtIDEnd.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtIDEnd.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Name", true));
            this.txtIDEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtIDEnd.Location = new System.Drawing.Point(218, 19);
            this.txtIDEnd.Name = "txtIDEnd";
            this.txtIDEnd.Size = new System.Drawing.Size(240, 23);
            this.txtIDEnd.TabIndex = 2;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(12, 365);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(81, 23);
            this.labelRemark.TabIndex = 13;
            this.labelRemark.Text = "Remark";
            // 
            // labelResign
            // 
            this.labelResign.Location = new System.Drawing.Point(238, 331);
            this.labelResign.Name = "labelResign";
            this.labelResign.Size = new System.Drawing.Size(81, 23);
            this.labelResign.TabIndex = 12;
            this.labelResign.Text = "Resign";
            // 
            // labelLanguage
            // 
            this.labelLanguage.Location = new System.Drawing.Point(238, 297);
            this.labelLanguage.Name = "labelLanguage";
            this.labelLanguage.Size = new System.Drawing.Size(81, 23);
            this.labelLanguage.TabIndex = 11;
            this.labelLanguage.Text = "Language";
            // 
            // labelExtNo
            // 
            this.labelExtNo.Location = new System.Drawing.Point(238, 51);
            this.labelExtNo.Name = "labelExtNo";
            this.labelExtNo.Size = new System.Drawing.Size(81, 23);
            this.labelExtNo.TabIndex = 10;
            this.labelExtNo.Text = "Ext#";
            // 
            // labelDateHired
            // 
            this.labelDateHired.Location = new System.Drawing.Point(12, 331);
            this.labelDateHired.Name = "labelDateHired";
            this.labelDateHired.Size = new System.Drawing.Size(81, 23);
            this.labelDateHired.TabIndex = 9;
            this.labelDateHired.Text = "Date Hired";
            // 
            // labelPosition
            // 
            this.labelPosition.Location = new System.Drawing.Point(12, 297);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(81, 23);
            this.labelPosition.TabIndex = 8;
            this.labelPosition.Text = "Position";
            // 
            // labelEMailAddr
            // 
            this.labelEMailAddr.Location = new System.Drawing.Point(12, 263);
            this.labelEMailAddr.Name = "labelEMailAddr";
            this.labelEMailAddr.Size = new System.Drawing.Size(81, 23);
            this.labelEMailAddr.TabIndex = 7;
            this.labelEMailAddr.Text = "E-Mail Addr.";
            // 
            // labelDeputy
            // 
            this.labelDeputy.Location = new System.Drawing.Point(12, 229);
            this.labelDeputy.Name = "labelDeputy";
            this.labelDeputy.Size = new System.Drawing.Size(81, 23);
            this.labelDeputy.TabIndex = 6;
            this.labelDeputy.Text = "Deputy";
            // 
            // labelSupervisor
            // 
            this.labelSupervisor.Location = new System.Drawing.Point(12, 197);
            this.labelSupervisor.Name = "labelSupervisor";
            this.labelSupervisor.Size = new System.Drawing.Size(81, 23);
            this.labelSupervisor.TabIndex = 5;
            this.labelSupervisor.Text = "Supervisor";
            // 
            // labelManager
            // 
            this.labelManager.Location = new System.Drawing.Point(12, 164);
            this.labelManager.Name = "labelManager";
            this.labelManager.Size = new System.Drawing.Size(81, 23);
            this.labelManager.TabIndex = 4;
            this.labelManager.Text = "Manager";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(12, 108);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(81, 23);
            this.labelFactory.TabIndex = 3;
            this.labelFactory.Text = "Factory";
            // 
            // labelPassword
            // 
            this.labelPassword.Location = new System.Drawing.Point(12, 51);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(81, 23);
            this.labelPassword.TabIndex = 2;
            this.labelPassword.Text = "Password";
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(12, 19);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(81, 23);
            this.labelID.TabIndex = 1;
            this.labelID.Text = "ID";
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
            this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(479, 8);
            this.grid1.Name = "grid1";
            this.grid1.RowHeadersVisible = false;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(514, 526);
            this.grid1.TabIndex = 15;
            this.grid1.TabStop = false;
            // 
            // PasswordByUser
            // 
            this.ClientSize = new System.Drawing.Size(1001, 634);
            this.DefaultControl = "txtIDStart";
            this.GridAlias = "Pass2";
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.Name = "PasswordByUser";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "Password by User";
            this.WorkAlias = "Pass1";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioGroup radioGroup1;
        private Win.UI.TextBox txtIDStart;
        private Win.UI.DateBox dateResign;
        private Win.UI.DateBox dateDateHired;
        private Win.UI.EditBox editRemark;
        private Win.UI.ComboBox comboLanguage;
        private Win.UI.TextBox txtEMailAddr;
        private Win.UI.EditBox editFactory;
        private Win.UI.TextBox txtExtNo;
        private Win.UI.TextBox txtPassword;
        private Win.UI.TextBox txtIDEnd;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelResign;
        private Win.UI.Label labelLanguage;
        private Win.UI.Label labelExtNo;
        private Win.UI.Label labelDateHired;
        private Win.UI.Label labelPosition;
        private Win.UI.Label labelEMailAddr;
        private Win.UI.Label labelDeputy;
        private Win.UI.Label labelSupervisor;
        private Win.UI.Label labelManager;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelPassword;
        private Win.UI.Label labelID;
        private Win.UI.Grid grid1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Class.Txtuser txtUserDeputy;
        private Class.Txtuser txtUserSupervisor;
        private Class.Txtuser txtUserManager;
        private Win.UI.TextBox txtPosition;
        private Win.UI.CheckBox checkAdmin;
        private Win.UI.Label labESignature;
        private Win.UI.Button btnShowImg;
        private Win.UI.Button btnSetPic;
        private Win.UI.DisplayBox disBoxESignature;
    }
}
