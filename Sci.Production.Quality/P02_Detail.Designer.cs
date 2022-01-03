namespace Sci.Production.Quality
{
    partial class P02_Detail
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
            this.btnAmend = new Sci.Win.UI.Button();
            this.txtUnit = new Sci.Win.UI.TextBox();
            this.txtColor = new Sci.Win.UI.TextBox();
            this.txtSize = new Sci.Win.UI.TextBox();
            this.txtArriveQty = new Sci.Win.UI.TextBox();
            this.txtWKNO = new Sci.Win.UI.TextBox();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.txtSCIRefno = new Sci.Win.UI.TextBox();
            this.txtSEQ = new Sci.Win.UI.TextBox();
            this.labelSEQ = new Sci.Win.UI.Label();
            this.labelWKNO = new Sci.Win.UI.Label();
            this.labelSCIRefno = new Sci.Win.UI.Label();
            this.labelRefno = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelArriveQty = new Sci.Win.UI.Label();
            this.labelSize = new Sci.Win.UI.Label();
            this.labelColor = new Sci.Win.UI.Label();
            this.labelUnit = new Sci.Win.UI.Label();
            this.btnUploadDefectPicture = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnEdit = new Sci.Win.UI.Button();
            this.txtsupplier = new Sci.Production.Class.Txtsupplier();
            this.btnSendMail = new Sci.Win.UI.Button();
            this.cmbDefectPicture = new Sci.Win.UI.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupDefectPicture = new Sci.Win.UI.GroupBox();
            this.btnRemove = new Sci.Win.UI.Button();
            this.labelInspectedQty = new Sci.Win.UI.Label();
            this.txtInspectedQty = new Sci.Win.UI.TextBox();
            this.labelRejectedQty = new Sci.Win.UI.Label();
            this.txtRejectedQty = new Sci.Win.UI.TextBox();
            this.labelInspectDate = new Sci.Win.UI.Label();
            this.labelInspector = new Sci.Win.UI.Label();
            this.labelResult = new Sci.Win.UI.Label();
            this.comboResult = new Sci.Win.UI.ComboBox();
            this.txtInspector = new Sci.Production.Class.Txtuser();
            this.dateInspectDate = new Sci.Win.UI.DateBox();
            this.editDefect = new Sci.Win.UI.EditBox();
            this.labelDefect = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.txtRemark = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupDefectPicture.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.btnEdit);
            this.btmcont.Controls.Add(this.btnClose);
            this.btmcont.Location = new System.Drawing.Point(0, 455);
            this.btmcont.Size = new System.Drawing.Size(873, 40);
            this.btmcont.Controls.SetChildIndex(this.left, 0);
            this.btmcont.Controls.SetChildIndex(this.right, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.btnClose, 0);
            this.btmcont.Controls.SetChildIndex(this.btnEdit, 0);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(783, 5);
            this.undo.Text = "Close";
            this.undo.Visible = false;
            // 
            // save
            // 
            this.save.Enabled = true;
            this.save.Location = new System.Drawing.Point(703, 5);
            this.save.Text = "Edit";
            this.save.Visible = false;
            // 
            // left
            // 
            this.left.Enabled = true;
            this.left.Click += new System.EventHandler(this.Left_Click);
            // 
            // right
            // 
            this.right.Enabled = true;
            this.right.Click += new System.EventHandler(this.Right_Click);
            // 
            // btnAmend
            // 
            this.btnAmend.Location = new System.Drawing.Point(15, 12);
            this.btnAmend.Name = "btnAmend";
            this.btnAmend.Size = new System.Drawing.Size(117, 30);
            this.btnAmend.TabIndex = 117;
            this.btnAmend.Text = "Amend";
            this.btnAmend.UseVisualStyleBackColor = true;
            this.btnAmend.Click += new System.EventHandler(this.BtnAmend_Click);
            // 
            // txtUnit
            // 
            this.txtUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtUnit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "unit", true));
            this.txtUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtUnit.IsSupportEditMode = false;
            this.txtUnit.Location = new System.Drawing.Point(109, 148);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.ReadOnly = true;
            this.txtUnit.Size = new System.Drawing.Size(144, 23);
            this.txtUnit.TabIndex = 115;
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtColor.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "colorid", true));
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtColor.IsSupportEditMode = false;
            this.txtColor.Location = new System.Drawing.Point(342, 148);
            this.txtColor.Name = "txtColor";
            this.txtColor.ReadOnly = true;
            this.txtColor.Size = new System.Drawing.Size(125, 23);
            this.txtColor.TabIndex = 114;
            // 
            // txtSize
            // 
            this.txtSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSize.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "size", true));
            this.txtSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSize.IsSupportEditMode = false;
            this.txtSize.Location = new System.Drawing.Point(109, 180);
            this.txtSize.Name = "txtSize";
            this.txtSize.ReadOnly = true;
            this.txtSize.Size = new System.Drawing.Size(144, 23);
            this.txtSize.TabIndex = 113;
            // 
            // txtArriveQty
            // 
            this.txtArriveQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtArriveQty.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ArriveQty", true));
            this.txtArriveQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtArriveQty.IsSupportEditMode = false;
            this.txtArriveQty.Location = new System.Drawing.Point(109, 117);
            this.txtArriveQty.Name = "txtArriveQty";
            this.txtArriveQty.ReadOnly = true;
            this.txtArriveQty.Size = new System.Drawing.Size(144, 23);
            this.txtArriveQty.TabIndex = 112;
            // 
            // txtWKNO
            // 
            this.txtWKNO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtWKNO.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ExportID", true));
            this.txtWKNO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtWKNO.IsSupportEditMode = false;
            this.txtWKNO.Location = new System.Drawing.Point(109, 83);
            this.txtWKNO.Name = "txtWKNO";
            this.txtWKNO.ReadOnly = true;
            this.txtWKNO.Size = new System.Drawing.Size(144, 23);
            this.txtWKNO.TabIndex = 109;
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtRefno.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Refno", true));
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtRefno.IsSupportEditMode = false;
            this.txtRefno.Location = new System.Drawing.Point(342, 83);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.ReadOnly = true;
            this.txtRefno.Size = new System.Drawing.Size(147, 23);
            this.txtRefno.TabIndex = 108;
            // 
            // txtSCIRefno
            // 
            this.txtSCIRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSCIRefno.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SCIRefno", true));
            this.txtSCIRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSCIRefno.IsSupportEditMode = false;
            this.txtSCIRefno.Location = new System.Drawing.Point(342, 53);
            this.txtSCIRefno.Name = "txtSCIRefno";
            this.txtSCIRefno.ReadOnly = true;
            this.txtSCIRefno.Size = new System.Drawing.Size(147, 23);
            this.txtSCIRefno.TabIndex = 107;
            // 
            // txtSEQ
            // 
            this.txtSEQ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSEQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSEQ.IsSupportEditMode = false;
            this.txtSEQ.Location = new System.Drawing.Point(109, 53);
            this.txtSEQ.Name = "txtSEQ";
            this.txtSEQ.ReadOnly = true;
            this.txtSEQ.Size = new System.Drawing.Size(144, 23);
            this.txtSEQ.TabIndex = 106;
            // 
            // labelSEQ
            // 
            this.labelSEQ.Location = new System.Drawing.Point(15, 53);
            this.labelSEQ.Name = "labelSEQ";
            this.labelSEQ.Size = new System.Drawing.Size(91, 23);
            this.labelSEQ.TabIndex = 95;
            this.labelSEQ.Text = "SEQ#";
            // 
            // labelWKNO
            // 
            this.labelWKNO.Location = new System.Drawing.Point(15, 83);
            this.labelWKNO.Name = "labelWKNO";
            this.labelWKNO.Size = new System.Drawing.Size(91, 23);
            this.labelWKNO.TabIndex = 96;
            this.labelWKNO.Text = "WKNO";
            // 
            // labelSCIRefno
            // 
            this.labelSCIRefno.Location = new System.Drawing.Point(262, 53);
            this.labelSCIRefno.Name = "labelSCIRefno";
            this.labelSCIRefno.Size = new System.Drawing.Size(75, 23);
            this.labelSCIRefno.TabIndex = 97;
            this.labelSCIRefno.Text = "SCI Refno";
            // 
            // labelRefno
            // 
            this.labelRefno.Location = new System.Drawing.Point(262, 83);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(75, 23);
            this.labelRefno.TabIndex = 98;
            this.labelRefno.Text = "Refno";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(262, 117);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(75, 23);
            this.labelSupplier.TabIndex = 99;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelArriveQty
            // 
            this.labelArriveQty.Location = new System.Drawing.Point(15, 117);
            this.labelArriveQty.Name = "labelArriveQty";
            this.labelArriveQty.Size = new System.Drawing.Size(91, 23);
            this.labelArriveQty.TabIndex = 100;
            this.labelArriveQty.Text = "Arrive Qty";
            // 
            // labelSize
            // 
            this.labelSize.Location = new System.Drawing.Point(15, 180);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(91, 23);
            this.labelSize.TabIndex = 101;
            this.labelSize.Text = "Size";
            // 
            // labelColor
            // 
            this.labelColor.Location = new System.Drawing.Point(262, 148);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(75, 23);
            this.labelColor.TabIndex = 102;
            this.labelColor.Text = "Color";
            // 
            // labelUnit
            // 
            this.labelUnit.Location = new System.Drawing.Point(15, 148);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(91, 23);
            this.labelUnit.TabIndex = 103;
            this.labelUnit.Text = "Unit";
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
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(580, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 135;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Location = new System.Drawing.Point(499, 5);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 30);
            this.btnEdit.TabIndex = 136;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // txtsupplier
            // 
            this.txtsupplier.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Suppid", true));
            this.txtsupplier.DisplayBox1Binding = "";
            this.txtsupplier.Location = new System.Drawing.Point(342, 117);
            this.txtsupplier.Name = "txtsupplier";
            this.txtsupplier.Size = new System.Drawing.Size(214, 23);
            this.txtsupplier.TabIndex = 134;
            this.txtsupplier.TextBox1Binding = "";
            // 
            // btnSendMail
            // 
            this.btnSendMail.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnSendMail.Location = new System.Drawing.Point(140, 12);
            this.btnSendMail.Name = "btnSendMail";
            this.btnSendMail.Size = new System.Drawing.Size(117, 30);
            this.btnSendMail.TabIndex = 135;
            this.btnSendMail.Text = "Send Mail";
            this.btnSendMail.UseVisualStyleBackColor = true;
            this.btnSendMail.Click += new System.EventHandler(this.BtnSendMail_Click);
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
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(6, 95);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(305, 336);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 138;
            this.pictureBox1.TabStop = false;
            // 
            // groupDefectPicture
            // 
            this.groupDefectPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupDefectPicture.Controls.Add(this.btnRemove);
            this.groupDefectPicture.Controls.Add(this.btnUploadDefectPicture);
            this.groupDefectPicture.Controls.Add(this.pictureBox1);
            this.groupDefectPicture.Controls.Add(this.cmbDefectPicture);
            this.groupDefectPicture.Location = new System.Drawing.Point(555, 12);
            this.groupDefectPicture.Name = "groupDefectPicture";
            this.groupDefectPicture.Size = new System.Drawing.Size(318, 437);
            this.groupDefectPicture.TabIndex = 139;
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
            // labelInspectedQty
            // 
            this.labelInspectedQty.Location = new System.Drawing.Point(15, 213);
            this.labelInspectedQty.Name = "labelInspectedQty";
            this.labelInspectedQty.Size = new System.Drawing.Size(91, 23);
            this.labelInspectedQty.TabIndex = 105;
            this.labelInspectedQty.Text = "Inspected Qty";
            // 
            // txtInspectedQty
            // 
            this.txtInspectedQty.BackColor = System.Drawing.Color.White;
            this.txtInspectedQty.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "InspQty", true));
            this.txtInspectedQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInspectedQty.Location = new System.Drawing.Point(109, 213);
            this.txtInspectedQty.Name = "txtInspectedQty";
            this.txtInspectedQty.Size = new System.Drawing.Size(144, 23);
            this.txtInspectedQty.TabIndex = 116;
            // 
            // labelRejectedQty
            // 
            this.labelRejectedQty.Location = new System.Drawing.Point(15, 248);
            this.labelRejectedQty.Name = "labelRejectedQty";
            this.labelRejectedQty.Size = new System.Drawing.Size(91, 23);
            this.labelRejectedQty.TabIndex = 119;
            this.labelRejectedQty.Text = "Rejected Qty";
            // 
            // txtRejectedQty
            // 
            this.txtRejectedQty.BackColor = System.Drawing.Color.White;
            this.txtRejectedQty.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "RejectQty", true));
            this.txtRejectedQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRejectedQty.Location = new System.Drawing.Point(109, 248);
            this.txtRejectedQty.Name = "txtRejectedQty";
            this.txtRejectedQty.Size = new System.Drawing.Size(144, 23);
            this.txtRejectedQty.TabIndex = 120;
            // 
            // labelInspectDate
            // 
            this.labelInspectDate.Location = new System.Drawing.Point(15, 282);
            this.labelInspectDate.Name = "labelInspectDate";
            this.labelInspectDate.Size = new System.Drawing.Size(91, 23);
            this.labelInspectDate.TabIndex = 121;
            this.labelInspectDate.Text = "Inspect Date";
            // 
            // labelInspector
            // 
            this.labelInspector.Location = new System.Drawing.Point(15, 347);
            this.labelInspector.Name = "labelInspector";
            this.labelInspector.Size = new System.Drawing.Size(91, 23);
            this.labelInspector.TabIndex = 125;
            this.labelInspector.Text = "Inspector";
            // 
            // labelResult
            // 
            this.labelResult.Location = new System.Drawing.Point(15, 382);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(91, 23);
            this.labelResult.TabIndex = 127;
            this.labelResult.Text = "Result";
            // 
            // comboResult
            // 
            this.comboResult.BackColor = System.Drawing.Color.White;
            this.comboResult.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Result1", true));
            this.comboResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboResult.FormattingEnabled = true;
            this.comboResult.IsSupportUnselect = true;
            this.comboResult.Location = new System.Drawing.Point(109, 382);
            this.comboResult.Name = "comboResult";
            this.comboResult.OldText = "";
            this.comboResult.Size = new System.Drawing.Size(121, 24);
            this.comboResult.TabIndex = 129;
            // 
            // txtInspector
            // 
            this.txtInspector.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Inspector", true));
            this.txtInspector.DisplayBox1Binding = "";
            this.txtInspector.Location = new System.Drawing.Point(110, 347);
            this.txtInspector.Name = "txtInspector";
            this.txtInspector.Size = new System.Drawing.Size(301, 23);
            this.txtInspector.TabIndex = 144;
            this.txtInspector.TextBox1Binding = "";
            // 
            // dateInspectDate
            // 
            this.dateInspectDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "InspDate", true));
            this.dateInspectDate.Location = new System.Drawing.Point(110, 282);
            this.dateInspectDate.Name = "dateInspectDate";
            this.dateInspectDate.Size = new System.Drawing.Size(145, 23);
            this.dateInspectDate.TabIndex = 145;
            // 
            // editDefect
            // 
            this.editDefect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editDefect.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Defect", true));
            this.editDefect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editDefect.IsSupportEditMode = false;
            this.editDefect.Location = new System.Drawing.Point(261, 239);
            this.editDefect.Multiline = true;
            this.editDefect.Name = "editDefect";
            this.editDefect.ReadOnly = true;
            this.editDefect.Size = new System.Drawing.Size(288, 92);
            this.editDefect.TabIndex = 143;
            this.editDefect.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EditDefect_MouseDown);
            // 
            // labelDefect
            // 
            this.labelDefect.Location = new System.Drawing.Point(261, 213);
            this.labelDefect.Name = "labelDefect";
            this.labelDefect.Size = new System.Drawing.Size(76, 23);
            this.labelDefect.TabIndex = 140;
            this.labelDefect.Text = "Defect";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(15, 417);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(91, 23);
            this.labelRemark.TabIndex = 141;
            this.labelRemark.Text = "Remark";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(109, 417);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(287, 23);
            this.txtRemark.TabIndex = 142;
            // 
            // P02_Detail
            // 
            this.ClientSize = new System.Drawing.Size(873, 495);
            this.Controls.Add(this.txtInspector);
            this.Controls.Add(this.dateInspectDate);
            this.Controls.Add(this.editDefect);
            this.Controls.Add(this.labelDefect);
            this.Controls.Add(this.labelRemark);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.groupDefectPicture);
            this.Controls.Add(this.btnSendMail);
            this.Controls.Add(this.txtsupplier);
            this.Controls.Add(this.comboResult);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.labelInspector);
            this.Controls.Add(this.labelInspectDate);
            this.Controls.Add(this.txtRejectedQty);
            this.Controls.Add(this.labelRejectedQty);
            this.Controls.Add(this.btnAmend);
            this.Controls.Add(this.txtInspectedQty);
            this.Controls.Add(this.txtUnit);
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.txtArriveQty);
            this.Controls.Add(this.txtWKNO);
            this.Controls.Add(this.txtRefno);
            this.Controls.Add(this.txtSCIRefno);
            this.Controls.Add(this.txtSEQ);
            this.Controls.Add(this.labelSEQ);
            this.Controls.Add(this.labelWKNO);
            this.Controls.Add(this.labelSCIRefno);
            this.Controls.Add(this.labelRefno);
            this.Controls.Add(this.labelSupplier);
            this.Controls.Add(this.labelArriveQty);
            this.Controls.Add(this.labelSize);
            this.Controls.Add(this.labelColor);
            this.Controls.Add(this.labelUnit);
            this.Controls.Add(this.labelInspectedQty);
            this.Name = "P02_Detail";
            this.OnLineHelpID = "Sci.Win.Subs.Input6A";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Accessory Inspection- SP+SEQ+Detail";
            this.WorkAlias = "AIR";
            this.Controls.SetChildIndex(this.labelInspectedQty, 0);
            this.Controls.SetChildIndex(this.labelUnit, 0);
            this.Controls.SetChildIndex(this.labelColor, 0);
            this.Controls.SetChildIndex(this.labelSize, 0);
            this.Controls.SetChildIndex(this.labelArriveQty, 0);
            this.Controls.SetChildIndex(this.labelSupplier, 0);
            this.Controls.SetChildIndex(this.labelRefno, 0);
            this.Controls.SetChildIndex(this.labelSCIRefno, 0);
            this.Controls.SetChildIndex(this.labelWKNO, 0);
            this.Controls.SetChildIndex(this.labelSEQ, 0);
            this.Controls.SetChildIndex(this.txtSEQ, 0);
            this.Controls.SetChildIndex(this.txtSCIRefno, 0);
            this.Controls.SetChildIndex(this.txtRefno, 0);
            this.Controls.SetChildIndex(this.txtWKNO, 0);
            this.Controls.SetChildIndex(this.txtArriveQty, 0);
            this.Controls.SetChildIndex(this.txtSize, 0);
            this.Controls.SetChildIndex(this.txtColor, 0);
            this.Controls.SetChildIndex(this.txtUnit, 0);
            this.Controls.SetChildIndex(this.txtInspectedQty, 0);
            this.Controls.SetChildIndex(this.btnAmend, 0);
            this.Controls.SetChildIndex(this.labelRejectedQty, 0);
            this.Controls.SetChildIndex(this.txtRejectedQty, 0);
            this.Controls.SetChildIndex(this.labelInspectDate, 0);
            this.Controls.SetChildIndex(this.labelInspector, 0);
            this.Controls.SetChildIndex(this.labelResult, 0);
            this.Controls.SetChildIndex(this.comboResult, 0);
            this.Controls.SetChildIndex(this.txtsupplier, 0);
            this.Controls.SetChildIndex(this.btnSendMail, 0);
            this.Controls.SetChildIndex(this.groupDefectPicture, 0);
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.txtRemark, 0);
            this.Controls.SetChildIndex(this.labelRemark, 0);
            this.Controls.SetChildIndex(this.labelDefect, 0);
            this.Controls.SetChildIndex(this.editDefect, 0);
            this.Controls.SetChildIndex(this.dateInspectDate, 0);
            this.Controls.SetChildIndex(this.txtInspector, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupDefectPicture.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button btnAmend;
        private Win.UI.TextBox txtUnit;
        private Win.UI.TextBox txtColor;
        private Win.UI.TextBox txtSize;
        private Win.UI.TextBox txtArriveQty;
        private Win.UI.TextBox txtWKNO;
        private Win.UI.TextBox txtRefno;
        private Win.UI.TextBox txtSCIRefno;
        private Win.UI.TextBox txtSEQ;
        private Win.UI.Label labelSEQ;
        private Win.UI.Label labelWKNO;
        private Win.UI.Label labelSCIRefno;
        private Win.UI.Label labelRefno;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelArriveQty;
        private Win.UI.Label labelSize;
        private Win.UI.Label labelColor;
        private Win.UI.Label labelUnit;
        private Class.Txtsupplier txtsupplier;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnEdit;
        private Win.UI.Button btnUploadDefectPicture;
        private Win.UI.Button btnSendMail;
        private Win.UI.ComboBox cmbDefectPicture;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Win.UI.GroupBox groupDefectPicture;
        private Win.UI.Label labelInspectedQty;
        private Win.UI.TextBox txtInspectedQty;
        private Win.UI.Label labelRejectedQty;
        private Win.UI.TextBox txtRejectedQty;
        private Win.UI.Label labelInspectDate;
        private Win.UI.Label labelInspector;
        private Win.UI.Label labelResult;
        private Win.UI.ComboBox comboResult;
        private Class.Txtuser txtInspector;
        private Win.UI.DateBox dateInspectDate;
        private Win.UI.EditBox editDefect;
        private Win.UI.Label labelDefect;
        private Win.UI.Label labelRemark;
        private Win.UI.TextBox txtRemark;
        private Win.UI.Button btnRemove;
    }
}
