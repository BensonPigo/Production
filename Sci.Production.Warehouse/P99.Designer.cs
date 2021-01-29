namespace Sci.Production.Warehouse
{
    partial class P99
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new Sci.Win.UI.TabControl();
            this.TabPage_UpdateCommand = new System.Windows.Forms.TabPage();
            this.panel7 = new Sci.Win.UI.Panel();
            this.gridUpdate = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel6 = new Sci.Win.UI.Panel();
            this.comboMaterialType_Sheet1 = new System.Windows.Forms.ComboBox();
            this.labMaterialType2 = new Sci.Win.UI.Label();
            this.labSPNo = new Sci.Win.UI.Label();
            this.labFunction = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnDelete = new Sci.Win.UI.Button();
            this.btnUpdate = new Sci.Win.UI.Button();
            this.checkIncludeCompleteItem = new Sci.Win.UI.CheckBox();
            this.comboFunction = new Sci.Win.UI.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.dateCreate = new Sci.Win.UI.DateRange();
            this.labCreateDate = new Sci.Win.UI.Label();
            this.TabPage_UnLock = new System.Windows.Forms.TabPage();
            this.panel9 = new Sci.Win.UI.Panel();
            this.gridUnLock = new Sci.Win.UI.Grid();
            this.panel1 = new Sci.Win.UI.Panel();
            this.labReceivingID = new Sci.Win.UI.Label();
            this.labWkNo = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.comboMaterialType_Sheet2 = new System.Windows.Forms.ComboBox();
            this.labMaterialType = new Sci.Win.UI.Label();
            this.txtReceivingID = new Sci.Win.UI.TextBox();
            this.txtWKNo = new Sci.Win.UI.TextBox();
            this.labSeq = new Sci.Win.UI.Label();
            this.txtSeq = new Sci.Production.Class.TxtSeq();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.btnQuery2 = new Sci.Win.UI.Button();
            this.panel8 = new Sci.Win.UI.Panel();
            this.btnClose1 = new Sci.Win.UI.Button();
            this.btnUnlock = new Sci.Win.UI.Button();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.tabControl1.SuspendLayout();
            this.TabPage_UpdateCommand.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUpdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel6.SuspendLayout();
            this.TabPage_UnLock.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUnLock)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TabPage_UpdateCommand);
            this.tabControl1.Controls.Add(this.TabPage_UnLock);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(984, 546);
            this.tabControl1.TabIndex = 1;
            // 
            // TabPage_UpdateCommand
            // 
            this.TabPage_UpdateCommand.Controls.Add(this.panel7);
            this.TabPage_UpdateCommand.Controls.Add(this.panel6);
            this.TabPage_UpdateCommand.Location = new System.Drawing.Point(4, 25);
            this.TabPage_UpdateCommand.Name = "TabPage_UpdateCommand";
            this.TabPage_UpdateCommand.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_UpdateCommand.Size = new System.Drawing.Size(976, 517);
            this.TabPage_UpdateCommand.TabIndex = 0;
            this.TabPage_UpdateCommand.Text = "Update Command";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.gridUpdate);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 117);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(970, 397);
            this.panel7.TabIndex = 1;
            // 
            // gridUpdate
            // 
            this.gridUpdate.AllowUserToAddRows = false;
            this.gridUpdate.AllowUserToDeleteRows = false;
            this.gridUpdate.AllowUserToResizeRows = false;
            this.gridUpdate.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridUpdate.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridUpdate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridUpdate.DataSource = this.listControlBindingSource1;
            this.gridUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridUpdate.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridUpdate.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridUpdate.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridUpdate.Location = new System.Drawing.Point(0, 0);
            this.gridUpdate.Name = "gridUpdate";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridUpdate.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridUpdate.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridUpdate.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridUpdate.RowTemplate.Height = 24;
            this.gridUpdate.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridUpdate.ShowCellToolTips = false;
            this.gridUpdate.Size = new System.Drawing.Size(970, 397);
            this.gridUpdate.TabIndex = 0;
            this.gridUpdate.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.comboMaterialType_Sheet1);
            this.panel6.Controls.Add(this.labMaterialType2);
            this.panel6.Controls.Add(this.labSPNo);
            this.panel6.Controls.Add(this.labFunction);
            this.panel6.Controls.Add(this.btnQuery);
            this.panel6.Controls.Add(this.btnClose);
            this.panel6.Controls.Add(this.btnDelete);
            this.panel6.Controls.Add(this.btnUpdate);
            this.panel6.Controls.Add(this.checkIncludeCompleteItem);
            this.panel6.Controls.Add(this.comboFunction);
            this.panel6.Controls.Add(this.label2);
            this.panel6.Controls.Add(this.txtSPNoEnd);
            this.panel6.Controls.Add(this.txtSPNoStart);
            this.panel6.Controls.Add(this.dateCreate);
            this.panel6.Controls.Add(this.labCreateDate);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(970, 114);
            this.panel6.TabIndex = 0;
            // 
            // comboMaterialType_Sheet1
            // 
            this.comboMaterialType_Sheet1.FormattingEnabled = true;
            this.comboMaterialType_Sheet1.Location = new System.Drawing.Point(540, 38);
            this.comboMaterialType_Sheet1.Name = "comboMaterialType_Sheet1";
            this.comboMaterialType_Sheet1.Size = new System.Drawing.Size(121, 24);
            this.comboMaterialType_Sheet1.TabIndex = 162;
            this.comboMaterialType_Sheet1.SelectedValueChanged += new System.EventHandler(this.ComboMaterialType_Sheet1_SelectedValueChanged);
            // 
            // labMaterialType2
            // 
            this.labMaterialType2.Location = new System.Drawing.Point(439, 38);
            this.labMaterialType2.Name = "labMaterialType2";
            this.labMaterialType2.Size = new System.Drawing.Size(98, 23);
            this.labMaterialType2.TabIndex = 163;
            this.labMaterialType2.Text = "Material Type";
            // 
            // labSPNo
            // 
            this.labSPNo.Location = new System.Drawing.Point(14, 71);
            this.labSPNo.Name = "labSPNo";
            this.labSPNo.Size = new System.Drawing.Size(111, 23);
            this.labSPNo.TabIndex = 152;
            this.labSPNo.Text = "SP#";
            // 
            // labFunction
            // 
            this.labFunction.Location = new System.Drawing.Point(14, 5);
            this.labFunction.Name = "labFunction";
            this.labFunction.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labFunction.RectStyle.BorderWidth = 1F;
            this.labFunction.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labFunction.RectStyle.ExtBorderWidth = 1F;
            this.labFunction.Size = new System.Drawing.Size(111, 23);
            this.labFunction.TabIndex = 151;
            this.labFunction.Text = "Function";
            this.labFunction.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labFunction.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnQuery.Location = new System.Drawing.Point(725, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(101, 53);
            this.btnQuery.TabIndex = 150;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(864, 77);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(101, 30);
            this.btnClose.TabIndex = 149;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnDelete.Location = new System.Drawing.Point(864, 41);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(101, 30);
            this.btnDelete.TabIndex = 148;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnUpdate.Location = new System.Drawing.Point(864, 5);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(101, 30);
            this.btnUpdate.TabIndex = 147;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // checkIncludeCompleteItem
            // 
            this.checkIncludeCompleteItem.AutoSize = true;
            this.checkIncludeCompleteItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIncludeCompleteItem.Location = new System.Drawing.Point(439, 8);
            this.checkIncludeCompleteItem.Name = "checkIncludeCompleteItem";
            this.checkIncludeCompleteItem.Size = new System.Drawing.Size(202, 21);
            this.checkIncludeCompleteItem.TabIndex = 146;
            this.checkIncludeCompleteItem.Text = "Include Complete Command";
            this.checkIncludeCompleteItem.UseVisualStyleBackColor = true;
            this.checkIncludeCompleteItem.CheckedChanged += new System.EventHandler(this.CheckIncludeCompleteItem_CheckedChanged);
            // 
            // comboFunction
            // 
            this.comboFunction.BackColor = System.Drawing.Color.White;
            this.comboFunction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFunction.FormattingEnabled = true;
            this.comboFunction.IsSupportUnselect = true;
            this.comboFunction.Location = new System.Drawing.Point(128, 5);
            this.comboFunction.Name = "comboFunction";
            this.comboFunction.OldText = "";
            this.comboFunction.Size = new System.Drawing.Size(289, 24);
            this.comboFunction.TabIndex = 145;
            this.comboFunction.SelectedValueChanged += new System.EventHandler(this.ComboFunction_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(257, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 17);
            this.label2.TabIndex = 143;
            this.label2.Text = " ～";
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(289, 71);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(128, 23);
            this.txtSPNoEnd.TabIndex = 140;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(128, 71);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(128, 23);
            this.txtSPNoStart.TabIndex = 139;
            // 
            // dateCreate
            // 
            // 
            // 
            // 
            this.dateCreate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCreate.DateBox1.Name = "";
            this.dateCreate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCreate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCreate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCreate.DateBox2.Name = "";
            this.dateCreate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCreate.DateBox2.TabIndex = 1;
            this.dateCreate.Location = new System.Drawing.Point(128, 38);
            this.dateCreate.Name = "dateCreate";
            this.dateCreate.Size = new System.Drawing.Size(280, 23);
            this.dateCreate.TabIndex = 138;
            // 
            // labCreateDate
            // 
            this.labCreateDate.Location = new System.Drawing.Point(14, 38);
            this.labCreateDate.Name = "labCreateDate";
            this.labCreateDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labCreateDate.RectStyle.BorderWidth = 1F;
            this.labCreateDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labCreateDate.RectStyle.ExtBorderWidth = 1F;
            this.labCreateDate.Size = new System.Drawing.Size(111, 23);
            this.labCreateDate.TabIndex = 141;
            this.labCreateDate.Text = "Create Date";
            this.labCreateDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labCreateDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // TabPage_UnLock
            // 
            this.TabPage_UnLock.Controls.Add(this.panel9);
            this.TabPage_UnLock.Controls.Add(this.panel8);
            this.TabPage_UnLock.Location = new System.Drawing.Point(4, 25);
            this.TabPage_UnLock.Name = "TabPage_UnLock";
            this.TabPage_UnLock.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_UnLock.Size = new System.Drawing.Size(976, 517);
            this.TabPage_UnLock.TabIndex = 1;
            this.TabPage_UnLock.Text = "UnLock";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.gridUnLock);
            this.panel9.Controls.Add(this.panel1);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(3, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(970, 458);
            this.panel9.TabIndex = 1;
            // 
            // gridUnLock
            // 
            this.gridUnLock.AllowUserToAddRows = false;
            this.gridUnLock.AllowUserToDeleteRows = false;
            this.gridUnLock.AllowUserToResizeRows = false;
            this.gridUnLock.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridUnLock.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridUnLock.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridUnLock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridUnLock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridUnLock.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridUnLock.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridUnLock.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridUnLock.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridUnLock.Location = new System.Drawing.Point(0, 74);
            this.gridUnLock.Name = "gridUnLock";
            this.gridUnLock.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridUnLock.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridUnLock.RowTemplate.Height = 24;
            this.gridUnLock.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridUnLock.ShowCellToolTips = false;
            this.gridUnLock.Size = new System.Drawing.Size(970, 384);
            this.gridUnLock.TabIndex = 2;
            this.gridUnLock.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labReceivingID);
            this.panel1.Controls.Add(this.labWkNo);
            this.panel1.Controls.Add(this.labelSPNo);
            this.panel1.Controls.Add(this.comboMaterialType_Sheet2);
            this.panel1.Controls.Add(this.labMaterialType);
            this.panel1.Controls.Add(this.txtReceivingID);
            this.panel1.Controls.Add(this.txtWKNo);
            this.panel1.Controls.Add(this.labSeq);
            this.panel1.Controls.Add(this.txtSeq);
            this.panel1.Controls.Add(this.txtSPNo);
            this.panel1.Controls.Add(this.btnQuery2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(970, 74);
            this.panel1.TabIndex = 1;
            // 
            // labReceivingID
            // 
            this.labReceivingID.Location = new System.Drawing.Point(223, 40);
            this.labReceivingID.Name = "labReceivingID";
            this.labReceivingID.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labReceivingID.RectStyle.BorderWidth = 1F;
            this.labReceivingID.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labReceivingID.RectStyle.ExtBorderWidth = 1F;
            this.labReceivingID.Size = new System.Drawing.Size(91, 23);
            this.labReceivingID.TabIndex = 164;
            this.labReceivingID.Text = "Receiving ID";
            this.labReceivingID.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labReceivingID.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labWkNo
            // 
            this.labWkNo.Location = new System.Drawing.Point(13, 40);
            this.labWkNo.Name = "labWkNo";
            this.labWkNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labWkNo.RectStyle.BorderWidth = 1F;
            this.labWkNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labWkNo.RectStyle.ExtBorderWidth = 1F;
            this.labWkNo.Size = new System.Drawing.Size(70, 23);
            this.labWkNo.TabIndex = 163;
            this.labWkNo.Text = "WK NO";
            this.labWkNo.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labWkNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(13, 11);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.RectStyle.BorderWidth = 1F;
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.RectStyle.ExtBorderWidth = 1F;
            this.labelSPNo.Size = new System.Drawing.Size(70, 23);
            this.labelSPNo.TabIndex = 162;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // comboMaterialType_Sheet2
            // 
            this.comboMaterialType_Sheet2.FormattingEnabled = true;
            this.comboMaterialType_Sheet2.Location = new System.Drawing.Point(496, 11);
            this.comboMaterialType_Sheet2.Name = "comboMaterialType_Sheet2";
            this.comboMaterialType_Sheet2.Size = new System.Drawing.Size(121, 24);
            this.comboMaterialType_Sheet2.TabIndex = 160;
            // 
            // labMaterialType
            // 
            this.labMaterialType.Location = new System.Drawing.Point(395, 11);
            this.labMaterialType.Name = "labMaterialType";
            this.labMaterialType.Size = new System.Drawing.Size(98, 23);
            this.labMaterialType.TabIndex = 161;
            this.labMaterialType.Text = "Material Type";
            // 
            // txtReceivingID
            // 
            this.txtReceivingID.BackColor = System.Drawing.Color.White;
            this.txtReceivingID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReceivingID.Location = new System.Drawing.Point(317, 40);
            this.txtReceivingID.Name = "txtReceivingID";
            this.txtReceivingID.Size = new System.Drawing.Size(122, 23);
            this.txtReceivingID.TabIndex = 159;
            // 
            // txtWKNo
            // 
            this.txtWKNo.BackColor = System.Drawing.Color.White;
            this.txtWKNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWKNo.Location = new System.Drawing.Point(86, 40);
            this.txtWKNo.Name = "txtWKNo";
            this.txtWKNo.Size = new System.Drawing.Size(122, 23);
            this.txtWKNo.TabIndex = 157;
            // 
            // labSeq
            // 
            this.labSeq.Location = new System.Drawing.Point(223, 11);
            this.labSeq.Name = "labSeq";
            this.labSeq.Size = new System.Drawing.Size(91, 23);
            this.labSeq.TabIndex = 155;
            this.labSeq.Text = "SEQ";
            // 
            // txtSeq
            // 
            this.txtSeq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq.Location = new System.Drawing.Point(317, 11);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Seq1 = "";
            this.txtSeq.Seq2 = "";
            this.txtSeq.Size = new System.Drawing.Size(61, 23);
            this.txtSeq.TabIndex = 154;
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(86, 11);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(122, 23);
            this.txtSPNo.TabIndex = 152;
            // 
            // btnQuery2
            // 
            this.btnQuery2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnQuery2.Location = new System.Drawing.Point(855, 11);
            this.btnQuery2.Name = "btnQuery2";
            this.btnQuery2.Size = new System.Drawing.Size(101, 30);
            this.btnQuery2.TabIndex = 151;
            this.btnQuery2.Text = "Query";
            this.btnQuery2.UseVisualStyleBackColor = true;
            this.btnQuery2.Click += new System.EventHandler(this.BtnQuery2_Click);
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.btnClose1);
            this.panel8.Controls.Add(this.btnUnlock);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(3, 461);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(970, 53);
            this.panel8.TabIndex = 0;
            // 
            // btnClose1
            // 
            this.btnClose1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose1.Location = new System.Drawing.Point(855, 11);
            this.btnClose1.Name = "btnClose1";
            this.btnClose1.Size = new System.Drawing.Size(101, 30);
            this.btnClose1.TabIndex = 151;
            this.btnClose1.Text = "Close";
            this.btnClose1.UseVisualStyleBackColor = true;
            this.btnClose1.Click += new System.EventHandler(this.BtnClose1_Click);
            // 
            // btnUnlock
            // 
            this.btnUnlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnUnlock.Location = new System.Drawing.Point(725, 11);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(101, 30);
            this.btnUnlock.TabIndex = 150;
            this.btnUnlock.Text = "Unlock";
            this.btnUnlock.UseVisualStyleBackColor = true;
            this.btnUnlock.Click += new System.EventHandler(this.BtnUnlock_Click);
            // 
            // P99
            // 
            this.ClientSize = new System.Drawing.Size(984, 546);
            this.Controls.Add(this.tabControl1);
            this.Name = "P99";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P99. Send To WMS Command Status";
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.tabControl1.ResumeLayout(false);
            this.TabPage_UpdateCommand.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridUpdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.TabPage_UnLock.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridUnLock)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public Win.UI.TabControl tabControl1;
        private System.Windows.Forms.TabPage TabPage_UpdateCommand;
        private Win.UI.Panel panel7;
        private Win.UI.Grid gridUpdate;
        private Win.UI.Panel panel6;
        public System.Windows.Forms.TabPage TabPage_UnLock;
        private Win.UI.Panel panel9;
        private Win.UI.Panel panel8;
        private System.Windows.Forms.Label label2;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.DateRange dateCreate;
        private Win.UI.Label labCreateDate;
        private Win.UI.ComboBox comboFunction;
        private Win.UI.CheckBox checkIncludeCompleteItem;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnDelete;
        private Win.UI.Button btnUpdate;
        private Win.UI.Panel panel1;
        private Win.UI.Button btnQuery2;
        private Win.UI.Button btnClose1;
        private Win.UI.Button btnUnlock;
        private Win.UI.TextBox txtReceivingID;
        private Win.UI.TextBox txtWKNo;
        private Win.UI.Label labSeq;
        private Class.TxtSeq txtSeq;
        private Win.UI.TextBox txtSPNo;
        private System.Windows.Forms.ComboBox comboMaterialType_Sheet2;
        private Win.UI.Label labMaterialType;
        private Win.UI.Grid gridUnLock;
        private Win.UI.Label labFunction;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label labSPNo;
        private System.Windows.Forms.ComboBox comboMaterialType_Sheet1;
        private Win.UI.Label labMaterialType2;
        private Win.UI.Label labReceivingID;
        private Win.UI.Label labWkNo;
        private Win.UI.Label labelSPNo;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
    }
}
