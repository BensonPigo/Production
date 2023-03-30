namespace Sci.Production.Quality
{
    partial class P51
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.UI_grid = new Sci.Win.UI.Grid();
            this.gridBS = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.Btn_Find = new Sci.Win.UI.Button();
            this.Btn_NewSearch = new Sci.Win.UI.Button();
            this.txtBrand1 = new Sci.Production.Class.Txtbrand();
            this.cboDocumentname = new Sci.Win.UI.ComboBox();
            this.label14 = new Sci.Win.UI.Label();
            this.label15 = new Sci.Win.UI.Label();
            this.chkUploadRecord = new Sci.Win.UI.CheckBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txtBrandRefno = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.txtUser1 = new Sci.Production.Class.Txtuser();
            this.label3 = new Sci.Win.UI.Label();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.label12 = new Sci.Win.UI.Label();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.txtUpdate = new Sci.Win.UI.TextBox();
            this.label5 = new Sci.Win.UI.Label();
            this.BtnFileUpload = new Sci.Win.UI.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txtWKID = new System.Windows.Forms.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.dateETA = new Sci.Win.UI.DateRange();
            this.BtnUpdate = new Sci.Win.UI.Button();
            this.label7 = new Sci.Win.UI.Label();
            this.txtSeason1 = new Sci.Production.Class.Txtseason();
            this.chkNonValidDoc = new Sci.Win.UI.CheckBox();
            this.txtStyle1 = new Sci.Production.Class.Txtstyle();
            this.label9 = new Sci.Win.UI.Label();
            this.txtColor = new Sci.Win.UI.TextBox();
            this.label8 = new Sci.Win.UI.Label();
            this.txtPINO = new Sci.Win.UI.TextBox();
            this.label11 = new Sci.Win.UI.Label();
            this.txtMultiSupplier1 = new Sci.Production.Class.TxtMultiBrandSupplierGroup();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UI_grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.UI_grid);
            this.panel1.Location = new System.Drawing.Point(12, 243);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(953, 329);
            this.panel1.TabIndex = 1;
            // 
            // UI_grid
            // 
            this.UI_grid.AllowUserToAddRows = false;
            this.UI_grid.AllowUserToDeleteRows = false;
            this.UI_grid.AllowUserToResizeRows = false;
            this.UI_grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.UI_grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.UI_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UI_grid.DataSource = this.gridBS;
            this.UI_grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UI_grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.UI_grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.UI_grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.UI_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.UI_grid.Location = new System.Drawing.Point(0, 0);
            this.UI_grid.Name = "UI_grid";
            this.UI_grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.UI_grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.UI_grid.RowTemplate.Height = 24;
            this.UI_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.UI_grid.ShowCellToolTips = false;
            this.UI_grid.Size = new System.Drawing.Size(953, 329);
            this.UI_grid.TabIndex = 0;
            // 
            // Btn_Find
            // 
            this.Btn_Find.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_Find.Location = new System.Drawing.Point(832, 19);
            this.Btn_Find.Name = "Btn_Find";
            this.Btn_Find.Size = new System.Drawing.Size(119, 30);
            this.Btn_Find.TabIndex = 113;
            this.Btn_Find.Text = "Find";
            this.Btn_Find.UseVisualStyleBackColor = true;
            this.Btn_Find.Click += new System.EventHandler(this.Btn_Find_Click);
            // 
            // Btn_NewSearch
            // 
            this.Btn_NewSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_NewSearch.Location = new System.Drawing.Point(832, 55);
            this.Btn_NewSearch.Name = "Btn_NewSearch";
            this.Btn_NewSearch.Size = new System.Drawing.Size(119, 30);
            this.Btn_NewSearch.TabIndex = 114;
            this.Btn_NewSearch.Text = "Clear";
            this.Btn_NewSearch.UseVisualStyleBackColor = true;
            this.Btn_NewSearch.Click += new System.EventHandler(this.Btn_NewSearch_Click);
            // 
            // txtBrand1
            // 
            this.txtBrand1.BackColor = System.Drawing.Color.White;
            this.txtBrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand1.Location = new System.Drawing.Point(541, 9);
            this.txtBrand1.MyDocumentdName = this.cboDocumentname;
            this.txtBrand1.Name = "txtBrand1";
            this.txtBrand1.Size = new System.Drawing.Size(162, 23);
            this.txtBrand1.TabIndex = 2;
            // 
            // cboDocumentname
            // 
            this.cboDocumentname.BackColor = System.Drawing.Color.White;
            this.cboDocumentname.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.cboDocumentname.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboDocumentname.FormattingEnabled = true;
            this.cboDocumentname.IsSupportUnselect = true;
            this.cboDocumentname.Location = new System.Drawing.Point(128, 9);
            this.cboDocumentname.Name = "cboDocumentname";
            this.cboDocumentname.OldText = "";
            this.cboDocumentname.Size = new System.Drawing.Size(280, 24);
            this.cboDocumentname.TabIndex = 1;
            this.cboDocumentname.SelectedIndexChanged += new System.EventHandler(this.CboDocumentname_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(426, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(112, 23);
            this.label14.TabIndex = 1133;
            this.label14.Text = "Brand";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(12, 9);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(113, 23);
            this.label15.TabIndex = 1131;
            this.label15.Text = "Document Name";
            // 
            // chkUploadRecord
            // 
            this.chkUploadRecord.AutoSize = true;
            this.chkUploadRecord.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkUploadRecord.Location = new System.Drawing.Point(688, 39);
            this.chkUploadRecord.Name = "chkUploadRecord";
            this.chkUploadRecord.Size = new System.Drawing.Size(133, 21);
            this.chkUploadRecord.TabIndex = 4;
            this.chkUploadRecord.Text = "Uploaded record";
            this.chkUploadRecord.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 23);
            this.label2.TabIndex = 1136;
            this.label2.Text = "Supplier Group";
            // 
            // txtBrandRefno
            // 
            this.txtBrandRefno.BackColor = System.Drawing.Color.White;
            this.txtBrandRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrandRefno.Location = new System.Drawing.Point(541, 94);
            this.txtBrandRefno.Name = "txtBrandRefno";
            this.txtBrandRefno.Size = new System.Drawing.Size(162, 23);
            this.txtBrandRefno.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(426, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 23);
            this.label4.TabIndex = 1142;
            this.label4.Text = "Brand Refno";
            // 
            // txtUser1
            // 
            this.txtUser1.AllowSelectResign = false;
            this.txtUser1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtUser1.Location = new System.Drawing.Point(128, 122);
            this.txtUser1.Margin = new System.Windows.Forms.Padding(0);
            this.txtUser1.Name = "txtUser1";
            this.txtUser1.Size = new System.Drawing.Size(280, 22);
            this.txtUser1.TabIndex = 10;
            // 
            // 
            // 
            this.txtUser1.TextBox1.BackColor = System.Drawing.Color.White;
            this.txtUser1.TextBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtUser1.TextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtUser1.TextBox1.ForeColor = System.Drawing.Color.Red;
            this.txtUser1.TextBox1.Location = new System.Drawing.Point(0, 0);
            this.txtUser1.TextBox1.Margin = new System.Windows.Forms.Padding(1);
            this.txtUser1.TextBox1.MaxLength = 10;
            this.txtUser1.TextBox1.Name = "textId";
            this.txtUser1.TextBox1.Size = new System.Drawing.Size(94, 23);
            this.txtUser1.TextBox1.TabIndex = 0;
            this.txtUser1.TextBox1Binding = "";
            // 
            // 
            // 
            this.txtUser1.DisplayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtUser1.DisplayBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUser1.DisplayBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtUser1.DisplayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtUser1.DisplayBox1.Location = new System.Drawing.Point(94, 0);
            this.txtUser1.DisplayBox1.Margin = new System.Windows.Forms.Padding(0);
            this.txtUser1.DisplayBox1.Name = "textName";
            this.txtUser1.DisplayBox1.Size = new System.Drawing.Size(186, 23);
            this.txtUser1.DisplayBox1.TabIndex = 1;
            this.txtUser1.DisplayBox1Binding = "";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 23);
            this.label3.TabIndex = 1140;
            this.label3.Text = "PC Handle";
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(128, 94);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(162, 23);
            this.txtRefno.TabIndex = 8;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(13, 94);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(112, 23);
            this.label12.TabIndex = 1138;
            this.label12.Text = "Refno";
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // txtUpdate
            // 
            this.txtUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtUpdate.BackColor = System.Drawing.Color.White;
            this.txtUpdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtUpdate.Location = new System.Drawing.Point(128, 580);
            this.txtUpdate.Name = "txtUpdate";
            this.txtUpdate.Size = new System.Drawing.Size(139, 23);
            this.txtUpdate.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.Location = new System.Drawing.Point(13, 580);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 23);
            this.label5.TabIndex = 1150;
            this.label5.Text = "Batch Update";
            // 
            // BtnFileUpload
            // 
            this.BtnFileUpload.Location = new System.Drawing.Point(12, 207);
            this.BtnFileUpload.Name = "BtnFileUpload";
            this.BtnFileUpload.Size = new System.Drawing.Size(119, 30);
            this.BtnFileUpload.TabIndex = 1152;
            this.BtnFileUpload.Text = "File Upload";
            this.BtnFileUpload.UseVisualStyleBackColor = true;
            this.BtnFileUpload.Click += new System.EventHandler(this.BtnFileUpload_Click);
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.Location = new System.Drawing.Point(3, 202);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(962, 2);
            this.label10.TabIndex = 1153;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 23);
            this.label1.TabIndex = 1154;
            this.label1.Text = "WK#";
            // 
            // txtWKID
            // 
            this.txtWKID.BackColor = System.Drawing.Color.White;
            this.txtWKID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWKID.Location = new System.Drawing.Point(128, 66);
            this.txtWKID.Name = "txtWKID";
            this.txtWKID.Size = new System.Drawing.Size(162, 23);
            this.txtWKID.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(426, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 23);
            this.label6.TabIndex = 1156;
            this.label6.Text = "ETA";
            // 
            // dateETA
            // 
            // 
            // 
            // 
            this.dateETA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateETA.DateBox1.Name = "";
            this.dateETA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateETA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateETA.DateBox2.Name = "";
            this.dateETA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox2.TabIndex = 1;
            this.dateETA.Location = new System.Drawing.Point(541, 66);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(280, 23);
            this.dateETA.TabIndex = 6;
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnUpdate.Image = global::Sci.Production.Quality.Resource.trffc15;
            this.BtnUpdate.Location = new System.Drawing.Point(273, 576);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(28, 30);
            this.BtnUpdate.TabIndex = 1158;
            this.BtnUpdate.UseVisualStyleBackColor = true;
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(426, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 23);
            this.label7.TabIndex = 1163;
            this.label7.Text = "Season";
            // 
            // txtSeason1
            // 
            this.txtSeason1.BackColor = System.Drawing.Color.White;
            this.txtSeason1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeason1.Location = new System.Drawing.Point(541, 121);
            this.txtSeason1.BrandObjectName = this.txtBrand1;
            this.txtSeason1.Name = "txtSeason1";
            this.txtSeason1.Size = new System.Drawing.Size(162, 23);
            this.txtSeason1.TabIndex = 11;
            // 
            // chkNonValidDoc
            // 
            this.chkNonValidDoc.AutoSize = true;
            this.chkNonValidDoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkNonValidDoc.Location = new System.Drawing.Point(541, 39);
            this.chkNonValidDoc.Name = "chkNonValidDoc";
            this.chkNonValidDoc.Size = new System.Drawing.Size(125, 21);
            this.chkNonValidDoc.TabIndex = 1164;
            this.chkNonValidDoc.Text = "None valid doc.";
            this.chkNonValidDoc.UseVisualStyleBackColor = true;
            // 
            // txtStyle1
            // 
            this.txtStyle1.BackColor = System.Drawing.Color.White;
            this.txtStyle1.BrandObjectName = this.txtBrand1;            
            this.txtStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStyle1.Location = new System.Drawing.Point(128, 176);
            this.txtStyle1.Name = "txtStyle1";
            this.txtStyle1.SeasonObjectName = this.txtSeason1;
            this.txtStyle1.Size = new System.Drawing.Size(162, 23);
            this.txtStyle1.TabIndex = 1171;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(13, 176);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(112, 23);
            this.label9.TabIndex = 1170;
            this.label9.Text = "Style";
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.White;
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtColor.Location = new System.Drawing.Point(128, 148);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(162, 23);
            this.txtColor.TabIndex = 1168;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(13, 148);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 23);
            this.label8.TabIndex = 1169;
            this.label8.Text = "Color";
            // 
            // txtPINO
            // 
            this.txtPINO.BackColor = System.Drawing.Color.White;
            this.txtPINO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPINO.Location = new System.Drawing.Point(541, 148);
            this.txtPINO.Name = "txtPINO";
            this.txtPINO.Size = new System.Drawing.Size(162, 23);
            this.txtPINO.TabIndex = 1172;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(426, 148);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 23);
            this.label11.TabIndex = 1173;
            this.label11.Text = "PI#";
            // 
            // txtMultiSupplier1
            // 
            this.txtMultiSupplier1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtMultiSupplier1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtMultiSupplier1.IsSupportEditMode = false;
            this.txtMultiSupplier1.Location = new System.Drawing.Point(128, 39);
            this.txtMultiSupplier1.myBrandName = this.txtBrand1;
            this.txtMultiSupplier1.Name = "txtMultiSupplier1";
            this.txtMultiSupplier1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtMultiSupplier1.ReadOnly = true;
            this.txtMultiSupplier1.Size = new System.Drawing.Size(200, 23);
            this.txtMultiSupplier1.TabIndex = 1174;
            // 
            // P47
            // 
            this.ClientSize = new System.Drawing.Size(972, 612);
            this.Controls.Add(this.txtMultiSupplier1);
            this.Controls.Add(this.txtPINO);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtStyle1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkNonValidDoc);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtSeason1);
            this.Controls.Add(this.BtnUpdate);
            this.Controls.Add(this.dateETA);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtWKID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.BtnFileUpload);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtUpdate);
            this.Controls.Add(this.txtBrandRefno);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtUser1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRefno);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkUploadRecord);
            this.Controls.Add(this.txtBrand1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.cboDocumentname);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.Btn_NewSearch);
            this.Controls.Add(this.Btn_Find);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P47";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P47. Material Document upload by Shipment";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.Btn_Find, 0);
            this.Controls.SetChildIndex(this.Btn_NewSearch, 0);
            this.Controls.SetChildIndex(this.label15, 0);
            this.Controls.SetChildIndex(this.cboDocumentname, 0);
            this.Controls.SetChildIndex(this.label14, 0);
            this.Controls.SetChildIndex(this.txtBrand1, 0);
            this.Controls.SetChildIndex(this.chkUploadRecord, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.txtRefno, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtUser1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txtBrandRefno, 0);
            this.Controls.SetChildIndex(this.txtUpdate, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.BtnFileUpload, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtWKID, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.dateETA, 0);
            this.Controls.SetChildIndex(this.BtnUpdate, 0);
            this.Controls.SetChildIndex(this.txtSeason1, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.chkNonValidDoc, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.txtColor, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.txtStyle1, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.txtPINO, 0);
            this.Controls.SetChildIndex(this.txtMultiSupplier1, 0);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UI_grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Grid UI_grid;
        private Win.UI.ListControlBindingSource gridBS;
        private Win.UI.Button Btn_Find;
        private Win.UI.Button Btn_NewSearch;
        private Production.Class.Txtbrand txtBrand1;
        private Win.UI.Label label14;
        private Win.UI.ComboBox cboDocumentname;
        private Win.UI.Label label15;
        private Win.UI.CheckBox chkUploadRecord;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtBrandRefno;
        private Win.UI.Label label4;
        private Production.Class.Txtuser txtUser1;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtRefno;
        private Win.UI.Label label12;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private Win.UI.Label label5;
        private Win.UI.TextBox txtUpdate;
        private Win.UI.Button BtnFileUpload;
        private System.Windows.Forms.Label label10;
        private Win.UI.DateRange dateETA;
        private Win.UI.Label label6;
        private System.Windows.Forms.TextBox txtWKID;
        private Win.UI.Label label1;
        private Win.UI.Button BtnUpdate;
        private Win.UI.Label label7;
        private Production.Class.Txtseason txtSeason1;
        private Win.UI.CheckBox chkNonValidDoc;
        private Production.Class.Txtstyle txtStyle1;
        private Win.UI.Label label9;
        private Win.UI.TextBox txtColor;
        private Win.UI.Label label8;
        private Win.UI.TextBox txtPINO;
        private Win.UI.Label label11;
        private Production.Class.TxtMultiBrandSupplierGroup txtMultiSupplier1;
    }
}
