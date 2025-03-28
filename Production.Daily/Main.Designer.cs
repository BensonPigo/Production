namespace Production.Daily
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gridBS = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panelFTP = new Sci.Win.UI.Panel();
            this.textImportDataFileName = new Sci.Win.UI.TextBox();
            this.textFtpPwd = new Sci.Win.UI.TextBox();
            this.textFtpID = new Sci.Win.UI.TextBox();
            this.displayRgCode = new Sci.Win.UI.DisplayBox();
            this.labelRgCode = new Sci.Win.UI.Label();
            this.textFtpIP = new Sci.Win.UI.TextBox();
            this.labelImportDataFileName = new Sci.Win.UI.Label();
            this.labelFtpPwd = new Sci.Win.UI.Label();
            this.labelFtpID = new Sci.Win.UI.Label();
            this.labelFtpIP = new Sci.Win.UI.Label();
            this.panelMail = new Sci.Win.UI.Panel();
            this.txtMailServerPort = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.checkSendBack = new Sci.Win.UI.CheckBox();
            this.checkDailyUpdateSendMail = new Sci.Win.UI.CheckBox();
            this.label3 = new Sci.Win.UI.Label();
            this.textEmailPwd = new Sci.Win.UI.TextBox();
            this.labelEmailPwd = new Sci.Win.UI.Label();
            this.textEmailID = new Sci.Win.UI.TextBox();
            this.labelEmailID = new Sci.Win.UI.Label();
            this.labelCcAddress = new Sci.Win.UI.Label();
            this.editCcAddress = new Sci.Win.UI.EditBox();
            this.labelContent = new Sci.Win.UI.Label();
            this.editContent = new Sci.Win.UI.EditBox();
            this.labelToAddress = new Sci.Win.UI.Label();
            this.editToAddress = new Sci.Win.UI.EditBox();
            this.textSendFrom = new Sci.Win.UI.TextBox();
            this.labelSendfrom = new Sci.Win.UI.Label();
            this.textMailserver = new Sci.Win.UI.TextBox();
            this.labelMailserver = new Sci.Win.UI.Label();
            this.panelPath = new Sci.Win.UI.Panel();
            this.chk_import = new System.Windows.Forms.CheckBox();
            this.chk_export = new System.Windows.Forms.CheckBox();
            this.btnGetExportDataPath = new Sci.Win.UI.Button();
            this.btnGetImportDataPath = new Sci.Win.UI.Button();
            this.displayExportDataPath = new Sci.Win.UI.DisplayBox();
            this.labelExportDataPath = new Sci.Win.UI.Label();
            this.displayImportDataPath = new Sci.Win.UI.DisplayBox();
            this.labelImportDataPath = new Sci.Win.UI.Label();
            this.btnUpdate = new Sci.Win.UI.Button();
            this.btnTestFTP = new Sci.Win.UI.Button();
            this.BtnTestMail = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.statusStrip = new Sci.Win.UI.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnTestWebApi = new Sci.Win.UI.Button();
            this.btnFileCopy = new Sci.Win.UI.Button();
            this.btnRARCheck = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBS)).BeginInit();
            this.panelFTP.SuspendLayout();
            this.panelMail.SuspendLayout();
            this.panelPath.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFTP
            // 
            this.panelFTP.Controls.Add(this.textImportDataFileName);
            this.panelFTP.Controls.Add(this.textFtpPwd);
            this.panelFTP.Controls.Add(this.textFtpID);
            this.panelFTP.Controls.Add(this.displayRgCode);
            this.panelFTP.Controls.Add(this.labelRgCode);
            this.panelFTP.Controls.Add(this.textFtpIP);
            this.panelFTP.Controls.Add(this.labelImportDataFileName);
            this.panelFTP.Controls.Add(this.labelFtpPwd);
            this.panelFTP.Controls.Add(this.labelFtpID);
            this.panelFTP.Controls.Add(this.labelFtpIP);
            this.panelFTP.DrawBorder = true;
            this.panelFTP.Location = new System.Drawing.Point(12, 36);
            this.panelFTP.Name = "panelFTP";
            this.panelFTP.Size = new System.Drawing.Size(712, 68);
            this.panelFTP.TabIndex = 1;
            // 
            // textImportDataFileName
            // 
            this.textImportDataFileName.BackColor = System.Drawing.Color.White;
            this.textImportDataFileName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.textImportDataFileName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ImportDataFileName", true));
            this.textImportDataFileName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textImportDataFileName.Location = new System.Drawing.Point(137, 37);
            this.textImportDataFileName.Name = "textImportDataFileName";
            this.textImportDataFileName.Size = new System.Drawing.Size(150, 21);
            this.textImportDataFileName.TabIndex = 7;
            // 
            // textFtpPwd
            // 
            this.textFtpPwd.BackColor = System.Drawing.Color.White;
            this.textFtpPwd.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.textFtpPwd.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SFtpPwd", true));
            this.textFtpPwd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textFtpPwd.Location = new System.Drawing.Point(573, 10);
            this.textFtpPwd.Name = "textFtpPwd";
            this.textFtpPwd.Size = new System.Drawing.Size(92, 21);
            this.textFtpPwd.TabIndex = 5;
            // 
            // textFtpID
            // 
            this.textFtpID.BackColor = System.Drawing.Color.White;
            this.textFtpID.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.textFtpID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SFtpID", true));
            this.textFtpID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textFtpID.Location = new System.Drawing.Point(369, 10);
            this.textFtpID.Name = "textFtpID";
            this.textFtpID.Size = new System.Drawing.Size(100, 21);
            this.textFtpID.TabIndex = 3;
            // 
            // displayRgCode
            // 
            this.displayRgCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRgCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "RgCode", true));
            this.displayRgCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRgCode.Location = new System.Drawing.Point(369, 37);
            this.displayRgCode.Name = "displayRgCode";
            this.displayRgCode.Size = new System.Drawing.Size(70, 21);
            this.displayRgCode.TabIndex = 9;
            // 
            // labelRgCode
            // 
            this.labelRgCode.Location = new System.Drawing.Point(300, 37);
            this.labelRgCode.Name = "labelRgCode";
            this.labelRgCode.Size = new System.Drawing.Size(66, 21);
            this.labelRgCode.TabIndex = 8;
            this.labelRgCode.Text = "Area Code";
            // 
            // textFtpIP
            // 
            this.textFtpIP.BackColor = System.Drawing.Color.White;
            this.textFtpIP.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.textFtpIP.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SFtpIP", true));
            this.textFtpIP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textFtpIP.Location = new System.Drawing.Point(79, 10);
            this.textFtpIP.Name = "textFtpIP";
            this.textFtpIP.Size = new System.Drawing.Size(180, 21);
            this.textFtpIP.TabIndex = 1;
            // 
            // labelImportDataFileName
            // 
            this.labelImportDataFileName.Location = new System.Drawing.Point(10, 37);
            this.labelImportDataFileName.Name = "labelImportDataFileName";
            this.labelImportDataFileName.Size = new System.Drawing.Size(124, 21);
            this.labelImportDataFileName.TabIndex = 6;
            this.labelImportDataFileName.Text = "File Name (with ZIP)";
            // 
            // labelFtpPwd
            // 
            this.labelFtpPwd.Location = new System.Drawing.Point(480, 10);
            this.labelFtpPwd.Name = "labelFtpPwd";
            this.labelFtpPwd.Size = new System.Drawing.Size(90, 21);
            this.labelFtpPwd.TabIndex = 4;
            this.labelFtpPwd.Text = "SFTP Password";
            // 
            // labelFtpID
            // 
            this.labelFtpID.Location = new System.Drawing.Point(300, 10);
            this.labelFtpID.Name = "labelFtpID";
            this.labelFtpID.Size = new System.Drawing.Size(66, 21);
            this.labelFtpID.TabIndex = 2;
            this.labelFtpID.Text = "SFTP ID";
            // 
            // labelFtpIP
            // 
            this.labelFtpIP.Location = new System.Drawing.Point(10, 10);
            this.labelFtpIP.Name = "labelFtpIP";
            this.labelFtpIP.Size = new System.Drawing.Size(66, 21);
            this.labelFtpIP.TabIndex = 0;
            this.labelFtpIP.Text = "SFTP IP";
            // 
            // panelMail
            // 
            this.panelMail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMail.Controls.Add(this.txtMailServerPort);
            this.panelMail.Controls.Add(this.label1);
            this.panelMail.Controls.Add(this.label4);
            this.panelMail.Controls.Add(this.checkSendBack);
            this.panelMail.Controls.Add(this.checkDailyUpdateSendMail);
            this.panelMail.Controls.Add(this.label3);
            this.panelMail.Controls.Add(this.textEmailPwd);
            this.panelMail.Controls.Add(this.labelEmailPwd);
            this.panelMail.Controls.Add(this.textEmailID);
            this.panelMail.Controls.Add(this.labelEmailID);
            this.panelMail.Controls.Add(this.labelCcAddress);
            this.panelMail.Controls.Add(this.editCcAddress);
            this.panelMail.Controls.Add(this.labelContent);
            this.panelMail.Controls.Add(this.editContent);
            this.panelMail.Controls.Add(this.labelToAddress);
            this.panelMail.Controls.Add(this.editToAddress);
            this.panelMail.Controls.Add(this.textSendFrom);
            this.panelMail.Controls.Add(this.labelSendfrom);
            this.panelMail.Controls.Add(this.textMailserver);
            this.panelMail.Controls.Add(this.labelMailserver);
            this.panelMail.DrawBorder = true;
            this.panelMail.Location = new System.Drawing.Point(12, 219);
            this.panelMail.Name = "panelMail";
            this.panelMail.Size = new System.Drawing.Size(928, 191);
            this.panelMail.TabIndex = 3;
            // 
            // txtMailServerPort
            // 
            this.txtMailServerPort.BackColor = System.Drawing.Color.White;
            this.txtMailServerPort.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtMailServerPort.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MailServerPort", true));
            this.txtMailServerPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMailServerPort.Location = new System.Drawing.Point(321, 10);
            this.txtMailServerPort.Name = "txtMailServerPort";
            this.txtMailServerPort.Size = new System.Drawing.Size(44, 21);
            this.txtMailServerPort.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(289, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 21);
            this.label1.TabIndex = 18;
            this.label1.Text = "Port";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(671, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 21);
            this.label4.TabIndex = 17;
            this.label4.Text = "( _DailyDataTransfer.bat)";
            this.label4.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // checkSendBack
            // 
            this.checkSendBack.AutoSize = true;
            this.checkSendBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkSendBack.Location = new System.Drawing.Point(640, 38);
            this.checkSendBack.Name = "checkSendBack";
            this.checkSendBack.Size = new System.Drawing.Size(164, 19);
            this.checkSendBack.TabIndex = 15;
            this.checkSendBack.Text = "Don\'t send back to Taipei";
            this.checkSendBack.UseVisualStyleBackColor = true;
            this.checkSendBack.Visible = false;
            // 
            // checkDailyUpdateSendMail
            // 
            this.checkDailyUpdateSendMail.AutoSize = true;
            this.checkDailyUpdateSendMail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkDailyUpdateSendMail.Location = new System.Drawing.Point(640, 11);
            this.checkDailyUpdateSendMail.Name = "checkDailyUpdateSendMail";
            this.checkDailyUpdateSendMail.Size = new System.Drawing.Size(179, 19);
            this.checkDailyUpdateSendMail.TabIndex = 14;
            this.checkDailyUpdateSendMail.Text = "Send an e-mail from update";
            this.checkDailyUpdateSendMail.UseVisualStyleBackColor = true;
            this.checkDailyUpdateSendMail.Visible = false;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(377, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(306, 21);
            this.label3.TabIndex = 16;
            this.label3.Text = "P.S. if you want to set a schedule for auto run,pls use";
            this.label3.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // textEmailPwd
            // 
            this.textEmailPwd.BackColor = System.Drawing.Color.White;
            this.textEmailPwd.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.textEmailPwd.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "EmailPwd", true));
            this.textEmailPwd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textEmailPwd.Location = new System.Drawing.Point(480, 37);
            this.textEmailPwd.Name = "textEmailPwd";
            this.textEmailPwd.Size = new System.Drawing.Size(150, 21);
            this.textEmailPwd.TabIndex = 5;
            // 
            // labelEmailPwd
            // 
            this.labelEmailPwd.Location = new System.Drawing.Point(383, 37);
            this.labelEmailPwd.Name = "labelEmailPwd";
            this.labelEmailPwd.Size = new System.Drawing.Size(94, 21);
            this.labelEmailPwd.TabIndex = 4;
            this.labelEmailPwd.Text = "EMail Password";
            // 
            // textEmailID
            // 
            this.textEmailID.BackColor = System.Drawing.Color.White;
            this.textEmailID.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.textEmailID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "EmailID", true));
            this.textEmailID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textEmailID.Location = new System.Drawing.Point(480, 10);
            this.textEmailID.Name = "textEmailID";
            this.textEmailID.Size = new System.Drawing.Size(150, 21);
            this.textEmailID.TabIndex = 3;
            // 
            // labelEmailID
            // 
            this.labelEmailID.Location = new System.Drawing.Point(383, 10);
            this.labelEmailID.Name = "labelEmailID";
            this.labelEmailID.Size = new System.Drawing.Size(94, 21);
            this.labelEmailID.TabIndex = 2;
            this.labelEmailID.Text = "EMail ID";
            // 
            // labelCcAddress
            // 
            this.labelCcAddress.Location = new System.Drawing.Point(383, 64);
            this.labelCcAddress.Name = "labelCcAddress";
            this.labelCcAddress.Size = new System.Drawing.Size(94, 21);
            this.labelCcAddress.TabIndex = 10;
            this.labelCcAddress.Text = "Mail to (Taipei)";
            // 
            // editCcAddress
            // 
            this.editCcAddress.BackColor = System.Drawing.Color.White;
            this.editCcAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editCcAddress.Location = new System.Drawing.Point(480, 64);
            this.editCcAddress.Multiline = true;
            this.editCcAddress.Name = "editCcAddress";
            this.editCcAddress.Size = new System.Drawing.Size(260, 50);
            this.editCcAddress.TabIndex = 11;
            // 
            // labelContent
            // 
            this.labelContent.Location = new System.Drawing.Point(10, 120);
            this.labelContent.Name = "labelContent";
            this.labelContent.Size = new System.Drawing.Size(94, 21);
            this.labelContent.TabIndex = 12;
            this.labelContent.Text = "Mail Description";
            // 
            // editContent
            // 
            this.editContent.BackColor = System.Drawing.Color.White;
            this.editContent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editContent.Location = new System.Drawing.Point(107, 120);
            this.editContent.Multiline = true;
            this.editContent.Name = "editContent";
            this.editContent.Size = new System.Drawing.Size(260, 60);
            this.editContent.TabIndex = 13;
            // 
            // labelToAddress
            // 
            this.labelToAddress.Location = new System.Drawing.Point(10, 64);
            this.labelToAddress.Name = "labelToAddress";
            this.labelToAddress.Size = new System.Drawing.Size(94, 21);
            this.labelToAddress.TabIndex = 8;
            this.labelToAddress.Text = "Mail to (FTY)";
            // 
            // editToAddress
            // 
            this.editToAddress.BackColor = System.Drawing.Color.White;
            this.editToAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editToAddress.Location = new System.Drawing.Point(107, 64);
            this.editToAddress.Multiline = true;
            this.editToAddress.Name = "editToAddress";
            this.editToAddress.Size = new System.Drawing.Size(260, 50);
            this.editToAddress.TabIndex = 9;
            // 
            // textSendFrom
            // 
            this.textSendFrom.BackColor = System.Drawing.Color.White;
            this.textSendFrom.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.textSendFrom.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SendFrom", true));
            this.textSendFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textSendFrom.Location = new System.Drawing.Point(107, 37);
            this.textSendFrom.Name = "textSendFrom";
            this.textSendFrom.Size = new System.Drawing.Size(180, 21);
            this.textSendFrom.TabIndex = 7;
            // 
            // labelSendfrom
            // 
            this.labelSendfrom.Location = new System.Drawing.Point(10, 37);
            this.labelSendfrom.Name = "labelSendfrom";
            this.labelSendfrom.Size = new System.Drawing.Size(94, 21);
            this.labelSendfrom.TabIndex = 6;
            this.labelSendfrom.Text = "Send From";
            // 
            // textMailserver
            // 
            this.textMailserver.BackColor = System.Drawing.Color.White;
            this.textMailserver.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.textMailserver.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MailServer", true));
            this.textMailserver.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textMailserver.Location = new System.Drawing.Point(107, 10);
            this.textMailserver.Name = "textMailserver";
            this.textMailserver.Size = new System.Drawing.Size(180, 21);
            this.textMailserver.TabIndex = 1;
            // 
            // labelMailserver
            // 
            this.labelMailserver.Location = new System.Drawing.Point(10, 10);
            this.labelMailserver.Name = "labelMailserver";
            this.labelMailserver.Size = new System.Drawing.Size(94, 21);
            this.labelMailserver.TabIndex = 0;
            this.labelMailserver.Text = "SMTP IP";
            // 
            // panelPath
            // 
            this.panelPath.Controls.Add(this.chk_import);
            this.panelPath.Controls.Add(this.chk_export);
            this.panelPath.Controls.Add(this.btnGetExportDataPath);
            this.panelPath.Controls.Add(this.btnGetImportDataPath);
            this.panelPath.Controls.Add(this.displayExportDataPath);
            this.panelPath.Controls.Add(this.labelExportDataPath);
            this.panelPath.Controls.Add(this.displayImportDataPath);
            this.panelPath.Controls.Add(this.labelImportDataPath);
            this.panelPath.DrawBorder = true;
            this.panelPath.Location = new System.Drawing.Point(12, 110);
            this.panelPath.Name = "panelPath";
            this.panelPath.Size = new System.Drawing.Size(712, 103);
            this.panelPath.TabIndex = 2;
            // 
            // chk_import
            // 
            this.chk_import.AutoSize = true;
            this.chk_import.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_import.Location = new System.Drawing.Point(615, 38);
            this.chk_import.Name = "chk_import";
            this.chk_import.Size = new System.Drawing.Size(74, 24);
            this.chk_import.TabIndex = 8;
            this.chk_import.Text = "Import";
            this.chk_import.UseVisualStyleBackColor = true;
            // 
            // chk_export
            // 
            this.chk_export.AutoSize = true;
            this.chk_export.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_export.Location = new System.Drawing.Point(615, 8);
            this.chk_export.Name = "chk_export";
            this.chk_export.Size = new System.Drawing.Size(74, 24);
            this.chk_export.TabIndex = 7;
            this.chk_export.Text = "Export";
            this.chk_export.UseVisualStyleBackColor = true;
            // 
            // btnGetExportDataPath
            // 
            this.btnGetExportDataPath.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnGetExportDataPath.Location = new System.Drawing.Point(490, 40);
            this.btnGetExportDataPath.Name = "btnGetExportDataPath";
            this.btnGetExportDataPath.Size = new System.Drawing.Size(80, 30);
            this.btnGetExportDataPath.TabIndex = 5;
            this.btnGetExportDataPath.Text = "Get Folder";
            this.btnGetExportDataPath.UseVisualStyleBackColor = true;
            this.btnGetExportDataPath.Click += new System.EventHandler(this.btnGetExportDataPath_Click);
            // 
            // btnGetImportDataPath
            // 
            this.btnGetImportDataPath.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnGetImportDataPath.Location = new System.Drawing.Point(490, 6);
            this.btnGetImportDataPath.Name = "btnGetImportDataPath";
            this.btnGetImportDataPath.Size = new System.Drawing.Size(80, 30);
            this.btnGetImportDataPath.TabIndex = 2;
            this.btnGetImportDataPath.Text = "Get Folder";
            this.btnGetImportDataPath.UseVisualStyleBackColor = true;
            this.btnGetImportDataPath.Click += new System.EventHandler(this.btnGetImportDataPath_Click);
            // 
            // displayExportDataPath
            // 
            this.displayExportDataPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayExportDataPath.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ExportDataPath", true));
            this.displayExportDataPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayExportDataPath.Location = new System.Drawing.Point(167, 45);
            this.displayExportDataPath.Name = "displayExportDataPath";
            this.displayExportDataPath.Size = new System.Drawing.Size(310, 21);
            this.displayExportDataPath.TabIndex = 4;
            // 
            // labelExportDataPath
            // 
            this.labelExportDataPath.Location = new System.Drawing.Point(10, 45);
            this.labelExportDataPath.Name = "labelExportDataPath";
            this.labelExportDataPath.Size = new System.Drawing.Size(154, 21);
            this.labelExportDataPath.TabIndex = 3;
            this.labelExportDataPath.Text = "Export datas path";
            // 
            // displayImportDataPath
            // 
            this.displayImportDataPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayImportDataPath.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ImportDataPath", true));
            this.displayImportDataPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayImportDataPath.Location = new System.Drawing.Point(167, 11);
            this.displayImportDataPath.Name = "displayImportDataPath";
            this.displayImportDataPath.Size = new System.Drawing.Size(310, 21);
            this.displayImportDataPath.TabIndex = 1;
            // 
            // labelImportDataPath
            // 
            this.labelImportDataPath.Location = new System.Drawing.Point(10, 11);
            this.labelImportDataPath.Name = "labelImportDataPath";
            this.labelImportDataPath.Size = new System.Drawing.Size(154, 21);
            this.labelImportDataPath.TabIndex = 0;
            this.labelImportDataPath.Text = "Updatae datas path(Taipei)";
            // 
            // btnUpdate
            // 
            this.btnUpdate.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnUpdate.Location = new System.Drawing.Point(728, 37);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(106, 30);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "Update/Export";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnTestFTP
            // 
            this.btnTestFTP.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnTestFTP.Location = new System.Drawing.Point(728, 74);
            this.btnTestFTP.Name = "btnTestFTP";
            this.btnTestFTP.Size = new System.Drawing.Size(106, 30);
            this.btnTestFTP.TabIndex = 5;
            this.btnTestFTP.Text = "Testing FTP";
            this.btnTestFTP.UseVisualStyleBackColor = true;
            this.btnTestFTP.Click += new System.EventHandler(this.btnTestFTP_Click);
            // 
            // BtnTestMail
            // 
            this.BtnTestMail.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.BtnTestMail.Location = new System.Drawing.Point(728, 110);
            this.BtnTestMail.Name = "BtnTestMail";
            this.BtnTestMail.Size = new System.Drawing.Size(106, 30);
            this.BtnTestMail.TabIndex = 6;
            this.BtnTestMail.Text = "Testing Mail";
            this.BtnTestMail.UseVisualStyleBackColor = true;
            this.BtnTestMail.Click += new System.EventHandler(this.BtnTestMail_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnClose.Location = new System.Drawing.Point(730, 180);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(106, 30);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.labelProgress});
            this.statusStrip.Location = new System.Drawing.Point(0, 547);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(949, 22);
            this.statusStrip.TabIndex = 8;
            this.statusStrip.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(58, 17);
            this.StatusLabel.Text = "Progress:";
            // 
            // labelProgress
            // 
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(64, 17);
            this.labelProgress.Text = "                   ";
            // 
            // btnTestWebApi
            // 
            this.btnTestWebApi.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnTestWebApi.Location = new System.Drawing.Point(728, 144);
            this.btnTestWebApi.Name = "btnTestWebApi";
            this.btnTestWebApi.Size = new System.Drawing.Size(106, 30);
            this.btnTestWebApi.TabIndex = 9;
            this.btnTestWebApi.Text = "Testing Web API";
            this.btnTestWebApi.UseVisualStyleBackColor = true;
            this.btnTestWebApi.Click += new System.EventHandler(this.btnTestWebApi_Click);
            // 
            // btnFileCopy
            // 
            this.btnFileCopy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnFileCopy.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnFileCopy.Location = new System.Drawing.Point(840, 37);
            this.btnFileCopy.Name = "btnFileCopy";
            this.btnFileCopy.Size = new System.Drawing.Size(106, 30);
            this.btnFileCopy.TabIndex = 10;
            this.btnFileCopy.Text = "Test File Upload";
            this.btnFileCopy.UseVisualStyleBackColor = true;
            this.btnFileCopy.Click += new System.EventHandler(this.btnFileCopy_Click);
            // 
            // btnRARCheck
            // 
            this.btnRARCheck.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnRARCheck.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnRARCheck.Location = new System.Drawing.Point(840, 74);
            this.btnRARCheck.Name = "btnRARCheck";
            this.btnRARCheck.Size = new System.Drawing.Size(106, 30);
            this.btnRARCheck.TabIndex = 11;
            this.btnRARCheck.Text = "Test RAR Check";
            this.btnRARCheck.UseVisualStyleBackColor = true;
            this.btnRARCheck.Click += new System.EventHandler(this.btnRARCheck_Click);
            // 
            // Main
            // 
            this.AcceptButton = this.btnUpdate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(949, 569);
            this.Controls.Add(this.btnRARCheck);
            this.Controls.Add(this.btnFileCopy);
            this.Controls.Add(this.btnTestWebApi);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.BtnTestMail);
            this.Controls.Add(this.btnTestFTP);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.panelPath);
            this.Controls.Add(this.panelMail);
            this.Controls.Add(this.panelFTP);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsSupportClip = false;
            this.IsSupportLocate = false;
            this.IsSupportPrint = false;
            this.Name = "Main";
            this.OnLineHelpID = "Sci.Win.Tems.Input7";
            this.Text = "   ";
            this.WorkAlias = "System";
            this.Controls.SetChildIndex(this.panelFTP, 0);
            this.Controls.SetChildIndex(this.panelMail, 0);
            this.Controls.SetChildIndex(this.panelPath, 0);
            this.Controls.SetChildIndex(this.btnUpdate, 0);
            this.Controls.SetChildIndex(this.btnTestFTP, 0);
            this.Controls.SetChildIndex(this.BtnTestMail, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.statusStrip, 0);
            this.Controls.SetChildIndex(this.btnTestWebApi, 0);
            this.Controls.SetChildIndex(this.btnFileCopy, 0);
            this.Controls.SetChildIndex(this.btnRARCheck, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBS)).EndInit();
            this.panelFTP.ResumeLayout(false);
            this.panelFTP.PerformLayout();
            this.panelMail.ResumeLayout(false);
            this.panelMail.PerformLayout();
            this.panelPath.ResumeLayout(false);
            this.panelPath.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sci.Win.UI.Panel panelFTP;
        private Sci.Win.UI.Panel panelMail;
        private Sci.Win.UI.Label labelFtpIP;
        private Sci.Win.UI.ListControlBindingSource gridBS;
        private Sci.Win.UI.Label labelFtpID;
        private Sci.Win.UI.TextBox textImportDataFileName;
        private Sci.Win.UI.TextBox textFtpPwd;
        private Sci.Win.UI.TextBox textFtpID;
        private Sci.Win.UI.DisplayBox displayRgCode;
        private Sci.Win.UI.Label labelRgCode;
        private Sci.Win.UI.TextBox textFtpIP;
        private Sci.Win.UI.Label labelImportDataFileName;
        private Sci.Win.UI.Label labelFtpPwd;
        private Sci.Win.UI.Panel panelPath;
        private Sci.Win.UI.DisplayBox displayImportDataPath;
        private Sci.Win.UI.Label labelImportDataPath;
        private Sci.Win.UI.DisplayBox displayExportDataPath;
        private Sci.Win.UI.Label labelExportDataPath;
        private Sci.Win.UI.Button btnGetExportDataPath;
        private Sci.Win.UI.Button btnGetImportDataPath;
        private Sci.Win.UI.Button btnUpdate;
        private Sci.Win.UI.Button btnTestFTP;
        private Sci.Win.UI.Button BtnTestMail;
        private Sci.Win.UI.Button btnClose;
        private Sci.Win.UI.Label labelCcAddress;
        private Sci.Win.UI.EditBox editCcAddress;
        private Sci.Win.UI.Label labelContent;
        private Sci.Win.UI.EditBox editContent;
        private Sci.Win.UI.Label labelToAddress;
        private Sci.Win.UI.EditBox editToAddress;
        private Sci.Win.UI.TextBox textSendFrom;
        private Sci.Win.UI.Label labelSendfrom;
        private Sci.Win.UI.TextBox textMailserver;
        private Sci.Win.UI.Label labelMailserver;
        private Sci.Win.UI.Label label4;
        private Sci.Win.UI.CheckBox checkSendBack;
        private Sci.Win.UI.CheckBox checkDailyUpdateSendMail;
        private Sci.Win.UI.Label label3;
        private Sci.Win.UI.TextBox textEmailPwd;
        private Sci.Win.UI.Label labelEmailPwd;
        private Sci.Win.UI.TextBox textEmailID;
        private Sci.Win.UI.Label labelEmailID;
        private Sci.Win.UI.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel labelProgress;
        private System.Windows.Forms.CheckBox chk_import;
        private System.Windows.Forms.CheckBox chk_export;
        private Sci.Win.UI.Button btnTestWebApi;
        private Sci.Win.UI.Label label1;
        private Sci.Win.UI.TextBox txtMailServerPort;
        private Sci.Win.UI.Button btnFileCopy;
        private Sci.Win.UI.Button btnRARCheck;
    }
}