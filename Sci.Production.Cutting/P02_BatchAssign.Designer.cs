namespace Sci.Production.Cutting
{
    partial class P02_BatchAssign
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridBatchAssignCellEstCutDate = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.btnConfirm = new Sci.Win.UI.Button();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelBatchUpdateEstCutDate = new Sci.Win.UI.Label();
            this.dateBoxWKETA = new Sci.Win.UI.DateBox();
            this.labelBatchUpdateEstCutCell = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtCell2 = new Sci.Production.Class.txtCell();
            this.txtShift = new Sci.Production.Class.txtDropDownList();
            this.txtBatchUpdateEstCutDate = new Sci.Win.UI.DateBox();
            this.labShift = new Sci.Win.UI.Label();
            this.btnBatchUpdateEstCutDate = new Sci.Win.UI.Button();
            this.txtSpreadingNo = new Sci.Production.Class.txtSpreadingNo();
            this.labSeq = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSeq2 = new Sci.Win.UI.TextBox();
            this.txtSeq1 = new Sci.Win.UI.TextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.txtArticle = new Sci.Win.UI.TextBox();
            this.txtMarkerName = new Sci.Win.UI.TextBox();
            this.txtSizeCode = new Sci.Win.UI.TextBox();
            this.labelMarkerName = new Sci.Win.UI.Label();
            this.labelSizeCode = new Sci.Win.UI.Label();
            this.labelArticle = new Sci.Win.UI.Label();
            this.checkOnlyShowEmptyEstCutDate = new Sci.Win.UI.CheckBox();
            this.labelCutNo = new Sci.Win.UI.Label();
            this.btnFilter = new Sci.Win.UI.Button();
            this.labelEstCutDate = new Sci.Win.UI.Label();
            this.numCutNo = new Sci.Win.UI.NumericBox();
            this.txtFabricCombo = new Sci.Win.UI.TextBox();
            this.txtEstCutDate = new Sci.Win.UI.DateBox();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.txtCutCell = new Sci.Production.Class.txtCell();
            this.labelCutCell = new Sci.Win.UI.Label();
            this.labelFabricCombo = new Sci.Win.UI.Label();
            this.tabControl1 = new Sci.Win.UI.TabControl();
            this.tabByGarment = new System.Windows.Forms.TabPage();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.tabByFabric_Panel_Code = new System.Windows.Forms.TabPage();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.gridGarment = new Sci.Win.UI.Grid();
            this.gridFabric_Panel_Code = new Sci.Win.UI.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchAssignCellEstCutDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabByGarment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.tabByFabric_Panel_Code.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridGarment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFabric_Panel_Code)).BeginInit();
            this.SuspendLayout();
            // 
            // gridBatchAssignCellEstCutDate
            // 
            this.gridBatchAssignCellEstCutDate.AllowUserToAddRows = false;
            this.gridBatchAssignCellEstCutDate.AllowUserToDeleteRows = false;
            this.gridBatchAssignCellEstCutDate.AllowUserToResizeRows = false;
            this.gridBatchAssignCellEstCutDate.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBatchAssignCellEstCutDate.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBatchAssignCellEstCutDate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBatchAssignCellEstCutDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBatchAssignCellEstCutDate.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBatchAssignCellEstCutDate.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBatchAssignCellEstCutDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBatchAssignCellEstCutDate.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBatchAssignCellEstCutDate.Location = new System.Drawing.Point(0, 0);
            this.gridBatchAssignCellEstCutDate.Name = "gridBatchAssignCellEstCutDate";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridBatchAssignCellEstCutDate.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridBatchAssignCellEstCutDate.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBatchAssignCellEstCutDate.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBatchAssignCellEstCutDate.RowTemplate.Height = 24;
            this.gridBatchAssignCellEstCutDate.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBatchAssignCellEstCutDate.ShowCellToolTips = false;
            this.gridBatchAssignCellEstCutDate.Size = new System.Drawing.Size(1003, 269);
            this.gridBatchAssignCellEstCutDate.TabIndex = 0;
            this.gridBatchAssignCellEstCutDate.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.AutoSize = true;
            this.btnClose.Location = new System.Drawing.Point(912, 536);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Size = new System.Drawing.Size(1006, 572);
            this.shapeContainer1.TabIndex = 24;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 2;
            this.lineShape1.X2 = 997;
            this.lineShape1.Y1 = 79;
            this.lineShape1.Y2 = 78;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.AutoSize = true;
            this.btnConfirm.Location = new System.Drawing.Point(815, 536);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(80, 30);
            this.btnConfirm.TabIndex = 22;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer2.Size = new System.Drawing.Size(1003, 503);
            this.shapeContainer2.TabIndex = 24;
            this.shapeContainer2.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(1, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridBatchAssignCellEstCutDate);
            this.splitContainer1.Size = new System.Drawing.Size(1003, 530);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.TabIndex = 44;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel1);
            this.splitContainer2.Panel1.Controls.Add(this.panel7);
            this.splitContainer2.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer2.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer2.Size = new System.Drawing.Size(1003, 257);
            this.splitContainer2.SplitterDistance = 526;
            this.splitContainer2.SplitterWidth = 3;
            this.splitContainer2.TabIndex = 44;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelBatchUpdateEstCutDate);
            this.panel1.Controls.Add(this.dateBoxWKETA);
            this.panel1.Controls.Add(this.labelBatchUpdateEstCutCell);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtCell2);
            this.panel1.Controls.Add(this.txtShift);
            this.panel1.Controls.Add(this.txtBatchUpdateEstCutDate);
            this.panel1.Controls.Add(this.labShift);
            this.panel1.Controls.Add(this.btnBatchUpdateEstCutDate);
            this.panel1.Controls.Add(this.txtSpreadingNo);
            this.panel1.Controls.Add(this.labSeq);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtSeq2);
            this.panel1.Controls.Add(this.txtSeq1);
            this.panel1.Location = new System.Drawing.Point(-7, 142);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(531, 114);
            this.panel1.TabIndex = 48;
            // 
            // labelBatchUpdateEstCutDate
            // 
            this.labelBatchUpdateEstCutDate.Location = new System.Drawing.Point(11, 1);
            this.labelBatchUpdateEstCutDate.Name = "labelBatchUpdateEstCutDate";
            this.labelBatchUpdateEstCutDate.Size = new System.Drawing.Size(88, 23);
            this.labelBatchUpdateEstCutDate.TabIndex = 21;
            this.labelBatchUpdateEstCutDate.Text = "Est. Cut Date";
            // 
            // dateBoxWKETA
            // 
            this.dateBoxWKETA.Location = new System.Drawing.Point(102, 30);
            this.dateBoxWKETA.Name = "dateBoxWKETA";
            this.dateBoxWKETA.Size = new System.Drawing.Size(130, 23);
            this.dateBoxWKETA.TabIndex = 20;
            // 
            // labelBatchUpdateEstCutCell
            // 
            this.labelBatchUpdateEstCutCell.Location = new System.Drawing.Point(11, 59);
            this.labelBatchUpdateEstCutCell.Name = "labelBatchUpdateEstCutCell";
            this.labelBatchUpdateEstCutCell.Size = new System.Drawing.Size(88, 23);
            this.labelBatchUpdateEstCutCell.TabIndex = 19;
            this.labelBatchUpdateEstCutCell.Text = "Cut Cell";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(11, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 23);
            this.label3.TabIndex = 43;
            this.label3.Text = "WK ETA";
            // 
            // txtCell2
            // 
            this.txtCell2.BackColor = System.Drawing.Color.White;
            this.txtCell2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCell2.Location = new System.Drawing.Point(102, 59);
            this.txtCell2.MDivisionID = "";
            this.txtCell2.Name = "txtCell2";
            this.txtCell2.Size = new System.Drawing.Size(108, 23);
            this.txtCell2.TabIndex = 9;
            // 
            // txtShift
            // 
            this.txtShift.BackColor = System.Drawing.Color.White;
            this.txtShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShift.Location = new System.Drawing.Point(304, 30);
            this.txtShift.Name = "txtShift";
            this.txtShift.Size = new System.Drawing.Size(45, 23);
            this.txtShift.TabIndex = 18;
            this.txtShift.Type = "Pms_WorkOrderShift";
            // 
            // txtBatchUpdateEstCutDate
            // 
            this.txtBatchUpdateEstCutDate.Location = new System.Drawing.Point(102, 1);
            this.txtBatchUpdateEstCutDate.Name = "txtBatchUpdateEstCutDate";
            this.txtBatchUpdateEstCutDate.Size = new System.Drawing.Size(130, 23);
            this.txtBatchUpdateEstCutDate.TabIndex = 11;
            // 
            // labShift
            // 
            this.labShift.Location = new System.Drawing.Point(235, 30);
            this.labShift.Name = "labShift";
            this.labShift.Size = new System.Drawing.Size(66, 23);
            this.labShift.TabIndex = 37;
            this.labShift.Text = "Shift";
            // 
            // btnBatchUpdateEstCutDate
            // 
            this.btnBatchUpdateEstCutDate.Location = new System.Drawing.Point(395, 1);
            this.btnBatchUpdateEstCutDate.Name = "btnBatchUpdateEstCutDate";
            this.btnBatchUpdateEstCutDate.Size = new System.Drawing.Size(125, 30);
            this.btnBatchUpdateEstCutDate.TabIndex = 12;
            this.btnBatchUpdateEstCutDate.Text = "Batch Assign";
            this.btnBatchUpdateEstCutDate.UseVisualStyleBackColor = true;
            // 
            // txtSpreadingNo
            // 
            this.txtSpreadingNo.BackColor = System.Drawing.Color.White;
            this.txtSpreadingNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpreadingNo.IncludeJunk = true;
            this.txtSpreadingNo.Location = new System.Drawing.Point(102, 88);
            this.txtSpreadingNo.MDivision = "";
            this.txtSpreadingNo.Name = "txtSpreadingNo";
            this.txtSpreadingNo.Size = new System.Drawing.Size(108, 23);
            this.txtSpreadingNo.TabIndex = 16;
            // 
            // labSeq
            // 
            this.labSeq.Location = new System.Drawing.Point(235, 59);
            this.labSeq.Name = "labSeq";
            this.labSeq.Size = new System.Drawing.Size(66, 23);
            this.labSeq.TabIndex = 27;
            this.labSeq.Text = "Seq";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 23);
            this.label1.TabIndex = 33;
            this.label1.Text = "Spreading No";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label2.Location = new System.Drawing.Point(354, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 25);
            this.label2.TabIndex = 29;
            this.label2.Text = "-";
            // 
            // txtSeq2
            // 
            this.txtSeq2.BackColor = System.Drawing.Color.White;
            this.txtSeq2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq2.Location = new System.Drawing.Point(379, 59);
            this.txtSeq2.Name = "txtSeq2";
            this.txtSeq2.Size = new System.Drawing.Size(41, 23);
            this.txtSeq2.TabIndex = 14;
            // 
            // txtSeq1
            // 
            this.txtSeq1.BackColor = System.Drawing.Color.White;
            this.txtSeq1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq1.Location = new System.Drawing.Point(304, 59);
            this.txtSeq1.Name = "txtSeq1";
            this.txtSeq1.Size = new System.Drawing.Size(54, 23);
            this.txtSeq1.TabIndex = 13;
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.labelSPNo);
            this.panel7.Controls.Add(this.txtArticle);
            this.panel7.Controls.Add(this.txtMarkerName);
            this.panel7.Controls.Add(this.txtSizeCode);
            this.panel7.Controls.Add(this.labelMarkerName);
            this.panel7.Controls.Add(this.labelSizeCode);
            this.panel7.Controls.Add(this.labelArticle);
            this.panel7.Controls.Add(this.checkOnlyShowEmptyEstCutDate);
            this.panel7.Controls.Add(this.labelCutNo);
            this.panel7.Controls.Add(this.btnFilter);
            this.panel7.Controls.Add(this.labelEstCutDate);
            this.panel7.Controls.Add(this.numCutNo);
            this.panel7.Controls.Add(this.txtFabricCombo);
            this.panel7.Controls.Add(this.txtEstCutDate);
            this.panel7.Controls.Add(this.txtSPNo);
            this.panel7.Controls.Add(this.txtCutCell);
            this.panel7.Controls.Add(this.labelCutCell);
            this.panel7.Controls.Add(this.labelFabricCombo);
            this.panel7.Location = new System.Drawing.Point(-1, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(525, 139);
            this.panel7.TabIndex = 47;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(11, 1);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(88, 23);
            this.labelSPNo.TabIndex = 2;
            this.labelSPNo.Text = "SP#";
            // 
            // txtArticle
            // 
            this.txtArticle.BackColor = System.Drawing.Color.White;
            this.txtArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtArticle.Location = new System.Drawing.Point(102, 30);
            this.txtArticle.Name = "txtArticle";
            this.txtArticle.Size = new System.Drawing.Size(108, 23);
            this.txtArticle.TabIndex = 4;
            // 
            // txtMarkerName
            // 
            this.txtMarkerName.BackColor = System.Drawing.Color.White;
            this.txtMarkerName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMarkerName.Location = new System.Drawing.Point(102, 88);
            this.txtMarkerName.Name = "txtMarkerName";
            this.txtMarkerName.Size = new System.Drawing.Size(108, 23);
            this.txtMarkerName.TabIndex = 3;
            // 
            // txtSizeCode
            // 
            this.txtSizeCode.BackColor = System.Drawing.Color.White;
            this.txtSizeCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSizeCode.Location = new System.Drawing.Point(304, 30);
            this.txtSizeCode.Name = "txtSizeCode";
            this.txtSizeCode.Size = new System.Drawing.Size(109, 23);
            this.txtSizeCode.TabIndex = 5;
            // 
            // labelMarkerName
            // 
            this.labelMarkerName.Location = new System.Drawing.Point(11, 88);
            this.labelMarkerName.Name = "labelMarkerName";
            this.labelMarkerName.Size = new System.Drawing.Size(88, 23);
            this.labelMarkerName.TabIndex = 7;
            this.labelMarkerName.Text = "Marker Name";
            // 
            // labelSizeCode
            // 
            this.labelSizeCode.Location = new System.Drawing.Point(213, 30);
            this.labelSizeCode.Name = "labelSizeCode";
            this.labelSizeCode.Size = new System.Drawing.Size(88, 23);
            this.labelSizeCode.TabIndex = 4;
            this.labelSizeCode.Text = "SizeCode";
            // 
            // labelArticle
            // 
            this.labelArticle.Location = new System.Drawing.Point(11, 30);
            this.labelArticle.Name = "labelArticle";
            this.labelArticle.Size = new System.Drawing.Size(88, 23);
            this.labelArticle.TabIndex = 3;
            this.labelArticle.Text = "Article";
            // 
            // checkOnlyShowEmptyEstCutDate
            // 
            this.checkOnlyShowEmptyEstCutDate.AutoSize = true;
            this.checkOnlyShowEmptyEstCutDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkOnlyShowEmptyEstCutDate.Location = new System.Drawing.Point(11, 115);
            this.checkOnlyShowEmptyEstCutDate.Name = "checkOnlyShowEmptyEstCutDate";
            this.checkOnlyShowEmptyEstCutDate.Size = new System.Drawing.Size(221, 21);
            this.checkOnlyShowEmptyEstCutDate.TabIndex = 8;
            this.checkOnlyShowEmptyEstCutDate.Text = "Only show empty Est. Cut Date";
            this.checkOnlyShowEmptyEstCutDate.UseVisualStyleBackColor = true;
            // 
            // labelCutNo
            // 
            this.labelCutNo.Location = new System.Drawing.Point(213, 59);
            this.labelCutNo.Name = "labelCutNo";
            this.labelCutNo.Size = new System.Drawing.Size(88, 23);
            this.labelCutNo.TabIndex = 4;
            this.labelCutNo.Text = "Cut#";
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(440, 1);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(80, 30);
            this.btnFilter.TabIndex = 24;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Location = new System.Drawing.Point(213, 1);
            this.labelEstCutDate.Name = "labelEstCutDate";
            this.labelEstCutDate.Size = new System.Drawing.Size(88, 23);
            this.labelEstCutDate.TabIndex = 6;
            this.labelEstCutDate.Text = "Est. Cut Date";
            // 
            // numCutNo
            // 
            this.numCutNo.BackColor = System.Drawing.Color.White;
            this.numCutNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numCutNo.Location = new System.Drawing.Point(304, 59);
            this.numCutNo.Name = "numCutNo";
            this.numCutNo.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCutNo.Size = new System.Drawing.Size(109, 23);
            this.numCutNo.TabIndex = 1;
            this.numCutNo.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtFabricCombo
            // 
            this.txtFabricCombo.BackColor = System.Drawing.Color.White;
            this.txtFabricCombo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFabricCombo.Location = new System.Drawing.Point(102, 59);
            this.txtFabricCombo.Name = "txtFabricCombo";
            this.txtFabricCombo.Size = new System.Drawing.Size(108, 23);
            this.txtFabricCombo.TabIndex = 2;
            // 
            // txtEstCutDate
            // 
            this.txtEstCutDate.Location = new System.Drawing.Point(304, 1);
            this.txtEstCutDate.Name = "txtEstCutDate";
            this.txtEstCutDate.Size = new System.Drawing.Size(130, 23);
            this.txtEstCutDate.TabIndex = 6;
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(102, 1);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(108, 23);
            this.txtSPNo.TabIndex = 0;
            // 
            // txtCutCell
            // 
            this.txtCutCell.BackColor = System.Drawing.Color.White;
            this.txtCutCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutCell.Location = new System.Drawing.Point(304, 88);
            this.txtCutCell.MDivisionID = "";
            this.txtCutCell.Name = "txtCutCell";
            this.txtCutCell.Size = new System.Drawing.Size(109, 23);
            this.txtCutCell.TabIndex = 7;
            // 
            // labelCutCell
            // 
            this.labelCutCell.Location = new System.Drawing.Point(213, 88);
            this.labelCutCell.Name = "labelCutCell";
            this.labelCutCell.Size = new System.Drawing.Size(88, 23);
            this.labelCutCell.TabIndex = 9;
            this.labelCutCell.Text = "Cut Cell";
            // 
            // labelFabricCombo
            // 
            this.labelFabricCombo.Location = new System.Drawing.Point(11, 59);
            this.labelFabricCombo.Name = "labelFabricCombo";
            this.labelFabricCombo.Size = new System.Drawing.Size(88, 23);
            this.labelFabricCombo.TabIndex = 5;
            this.labelFabricCombo.Text = "FabricCombo";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabByGarment);
            this.tabControl1.Controls.Add(this.tabByFabric_Panel_Code);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(474, 257);
            this.tabControl1.TabIndex = 4;
            // 
            // tabByGarment
            // 
            this.tabByGarment.Controls.Add(this.gridGarment);
            this.tabByGarment.Location = new System.Drawing.Point(4, 25);
            this.tabByGarment.Name = "tabByGarment";
            this.tabByGarment.Padding = new System.Windows.Forms.Padding(3);
            this.tabByGarment.Size = new System.Drawing.Size(466, 228);
            this.tabByGarment.TabIndex = 0;
            this.tabByGarment.Text = "By Garment";
            // 
            // tabByFabric_Panel_Code
            // 
            this.tabByFabric_Panel_Code.Controls.Add(this.gridFabric_Panel_Code);
            this.tabByFabric_Panel_Code.Location = new System.Drawing.Point(4, 25);
            this.tabByFabric_Panel_Code.Name = "tabByFabric_Panel_Code";
            this.tabByFabric_Panel_Code.Padding = new System.Windows.Forms.Padding(3);
            this.tabByFabric_Panel_Code.Size = new System.Drawing.Size(466, 228);
            this.tabByFabric_Panel_Code.TabIndex = 1;
            this.tabByFabric_Panel_Code.Text = "By Fabric_Panel_Code";
            // 
            // gridGarment
            // 
            this.gridGarment.AllowUserToAddRows = false;
            this.gridGarment.AllowUserToDeleteRows = false;
            this.gridGarment.AllowUserToResizeRows = false;
            this.gridGarment.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridGarment.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridGarment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridGarment.DataSource = this.listControlBindingSource1;
            this.gridGarment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridGarment.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridGarment.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridGarment.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridGarment.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridGarment.Location = new System.Drawing.Point(3, 3);
            this.gridGarment.Name = "gridGarment";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridGarment.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridGarment.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridGarment.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridGarment.RowTemplate.Height = 24;
            this.gridGarment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridGarment.ShowCellToolTips = false;
            this.gridGarment.Size = new System.Drawing.Size(460, 222);
            this.gridGarment.TabIndex = 2;
            this.gridGarment.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.Grid_CellFormatting);
            this.gridGarment.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.Grid_CellPainting);
            // 
            // gridFabric_Panel_Code
            // 
            this.gridFabric_Panel_Code.AllowUserToAddRows = false;
            this.gridFabric_Panel_Code.AllowUserToDeleteRows = false;
            this.gridFabric_Panel_Code.AllowUserToResizeRows = false;
            this.gridFabric_Panel_Code.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridFabric_Panel_Code.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridFabric_Panel_Code.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFabric_Panel_Code.DataSource = this.listControlBindingSource2;
            this.gridFabric_Panel_Code.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFabric_Panel_Code.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridFabric_Panel_Code.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridFabric_Panel_Code.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridFabric_Panel_Code.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridFabric_Panel_Code.Location = new System.Drawing.Point(3, 3);
            this.gridFabric_Panel_Code.Name = "gridFabric_Panel_Code";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridFabric_Panel_Code.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridFabric_Panel_Code.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridFabric_Panel_Code.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridFabric_Panel_Code.RowTemplate.Height = 24;
            this.gridFabric_Panel_Code.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFabric_Panel_Code.ShowCellToolTips = false;
            this.gridFabric_Panel_Code.Size = new System.Drawing.Size(460, 222);
            this.gridFabric_Panel_Code.TabIndex = 5;
            this.gridFabric_Panel_Code.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.Grid_CellFormatting);
            this.gridFabric_Panel_Code.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.Grid_CellPainting);
            // 
            // P02_BatchAssign
            // 
            this.ClientSize = new System.Drawing.Size(1006, 572);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.shapeContainer1);
            this.DefaultControl = "txtSPNo";
            this.Name = "P02_BatchAssign";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Batch Assign Cell/Est. Cut Date";
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchAssignCellEstCutDate)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabByGarment.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.tabByFabric_Panel_Code.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridGarment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFabric_Panel_Code)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid gridBatchAssignCellEstCutDate;
        private Win.UI.Button btnClose;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.Button btnConfirm;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private System.Windows.Forms.Panel panel1;
        private Win.UI.Label labelBatchUpdateEstCutDate;
        private Win.UI.DateBox dateBoxWKETA;
        private Win.UI.Label labelBatchUpdateEstCutCell;
        private Win.UI.Label label3;
        private Class.txtCell txtCell2;
        private Class.txtDropDownList txtShift;
        private Win.UI.DateBox txtBatchUpdateEstCutDate;
        private Win.UI.Label labShift;
        private Win.UI.Button btnBatchUpdateEstCutDate;
        private Class.txtSpreadingNo txtSpreadingNo;
        private Win.UI.Label labSeq;
        private Win.UI.Label label1;
        private System.Windows.Forms.Label label2;
        private Win.UI.TextBox txtSeq2;
        private Win.UI.TextBox txtSeq1;
        private System.Windows.Forms.Panel panel7;
        private Win.UI.Label labelSPNo;
        private Win.UI.TextBox txtArticle;
        private Win.UI.TextBox txtMarkerName;
        private Win.UI.TextBox txtSizeCode;
        private Win.UI.Label labelMarkerName;
        private Win.UI.Label labelSizeCode;
        private Win.UI.Label labelArticle;
        private Win.UI.CheckBox checkOnlyShowEmptyEstCutDate;
        private Win.UI.Label labelCutNo;
        private Win.UI.Button btnFilter;
        private Win.UI.Label labelEstCutDate;
        private Win.UI.NumericBox numCutNo;
        private Win.UI.TextBox txtFabricCombo;
        private Win.UI.DateBox txtEstCutDate;
        private Win.UI.TextBox txtSPNo;
        private Class.txtCell txtCutCell;
        private Win.UI.Label labelCutCell;
        private Win.UI.Label labelFabricCombo;
        private Win.UI.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabByGarment;
        private System.Windows.Forms.TabPage tabByFabric_Panel_Code;
        private Win.UI.Grid gridGarment;
        private Win.UI.Grid gridFabric_Panel_Code;
    }
}
