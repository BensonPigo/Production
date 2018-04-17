namespace Sci.Production.Cutting
{
    partial class P02_BatchAssignCellCutDate
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridBatchAssignCellEstCutDate = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelArticle = new Sci.Win.UI.Label();
            this.labelSizeCode = new Sci.Win.UI.Label();
            this.labelCutNo = new Sci.Win.UI.Label();
            this.labelFabricCombo = new Sci.Win.UI.Label();
            this.labelEstCutDate = new Sci.Win.UI.Label();
            this.labelMarkerName = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelCutCell = new Sci.Win.UI.Label();
            this.txtArticle = new Sci.Win.UI.TextBox();
            this.numCutNo = new Sci.Win.UI.NumericBox();
            this.txtSizeCode = new Sci.Win.UI.TextBox();
            this.txtEstCutDate = new Sci.Win.UI.DateBox();
            this.txtFabricCombo = new Sci.Win.UI.TextBox();
            this.txtMarkerName = new Sci.Win.UI.TextBox();
            this.checkOnlyShowEmptyEstCutDate = new Sci.Win.UI.CheckBox();
            this.btnFilter = new Sci.Win.UI.Button();
            this.labelBatchUpdateEstCutCell = new Sci.Win.UI.Label();
            this.txtBatchUpdateEstCutDate = new Sci.Win.UI.DateBox();
            this.labelBatchUpdateEstCutDate = new Sci.Win.UI.Label();
            this.panel1 = new Sci.Win.UI.Panel();
            this.btnBatchUpdateEstCutCell = new Sci.Win.UI.Button();
            this.btnBatchUpdateEstCutDate = new Sci.Win.UI.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.txtCell2 = new Sci.Production.Class.txtCell();
            this.txtCutCell = new Sci.Production.Class.txtCell();
            this.btnConfirm = new Sci.Win.UI.Button();
            this.btnBatchUpdSeq = new Sci.Win.UI.Button();
            this.labSeq = new Sci.Win.UI.Label();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSeq1 = new Sci.Win.UI.TextBox();
            this.txtSeq2 = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchAssignCellEstCutDate)).BeginInit();
            this.SuspendLayout();
            // 
            // gridBatchAssignCellEstCutDate
            // 
            this.gridBatchAssignCellEstCutDate.AllowUserToAddRows = false;
            this.gridBatchAssignCellEstCutDate.AllowUserToDeleteRows = false;
            this.gridBatchAssignCellEstCutDate.AllowUserToResizeRows = false;
            this.gridBatchAssignCellEstCutDate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridBatchAssignCellEstCutDate.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBatchAssignCellEstCutDate.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBatchAssignCellEstCutDate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBatchAssignCellEstCutDate.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBatchAssignCellEstCutDate.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBatchAssignCellEstCutDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBatchAssignCellEstCutDate.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBatchAssignCellEstCutDate.Location = new System.Drawing.Point(6, 123);
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
            this.gridBatchAssignCellEstCutDate.Size = new System.Drawing.Size(1052, 338);
            this.gridBatchAssignCellEstCutDate.TabIndex = 0;
            this.gridBatchAssignCellEstCutDate.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.AutoSize = true;
            this.btnClose.Location = new System.Drawing.Point(978, 467);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(12, 9);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(65, 23);
            this.labelSPNo.TabIndex = 2;
            this.labelSPNo.Text = "SP#";
            // 
            // labelArticle
            // 
            this.labelArticle.Location = new System.Drawing.Point(12, 46);
            this.labelArticle.Name = "labelArticle";
            this.labelArticle.Size = new System.Drawing.Size(65, 23);
            this.labelArticle.TabIndex = 3;
            this.labelArticle.Text = "Article";
            // 
            // labelSizeCode
            // 
            this.labelSizeCode.Location = new System.Drawing.Point(197, 46);
            this.labelSizeCode.Name = "labelSizeCode";
            this.labelSizeCode.Size = new System.Drawing.Size(73, 23);
            this.labelSizeCode.TabIndex = 4;
            this.labelSizeCode.Text = "SizeCode";
            // 
            // labelCutNo
            // 
            this.labelCutNo.Location = new System.Drawing.Point(197, 9);
            this.labelCutNo.Name = "labelCutNo";
            this.labelCutNo.Size = new System.Drawing.Size(73, 23);
            this.labelCutNo.TabIndex = 4;
            this.labelCutNo.Text = "Cut#";
            // 
            // labelFabricCombo
            // 
            this.labelFabricCombo.Location = new System.Drawing.Point(359, 9);
            this.labelFabricCombo.Name = "labelFabricCombo";
            this.labelFabricCombo.Size = new System.Drawing.Size(88, 23);
            this.labelFabricCombo.TabIndex = 5;
            this.labelFabricCombo.Text = "FabricCombo";
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Location = new System.Drawing.Point(359, 46);
            this.labelEstCutDate.Name = "labelEstCutDate";
            this.labelEstCutDate.Size = new System.Drawing.Size(88, 23);
            this.labelEstCutDate.TabIndex = 6;
            this.labelEstCutDate.Text = "Est. Cut Date";
            // 
            // labelMarkerName
            // 
            this.labelMarkerName.Location = new System.Drawing.Point(517, 9);
            this.labelMarkerName.Name = "labelMarkerName";
            this.labelMarkerName.Size = new System.Drawing.Size(92, 23);
            this.labelMarkerName.TabIndex = 7;
            this.labelMarkerName.Text = "Marker Name";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(80, 9);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(108, 23);
            this.txtSPNo.TabIndex = 0;
            this.txtSPNo.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtSPNo_PopUp);
            // 
            // labelCutCell
            // 
            this.labelCutCell.Location = new System.Drawing.Point(598, 46);
            this.labelCutCell.Name = "labelCutCell";
            this.labelCutCell.Size = new System.Drawing.Size(92, 23);
            this.labelCutCell.TabIndex = 9;
            this.labelCutCell.Text = "Cut Cell";
            // 
            // txtArticle
            // 
            this.txtArticle.BackColor = System.Drawing.Color.White;
            this.txtArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtArticle.Location = new System.Drawing.Point(80, 46);
            this.txtArticle.Name = "txtArticle";
            this.txtArticle.Size = new System.Drawing.Size(78, 23);
            this.txtArticle.TabIndex = 4;
            // 
            // numCutNo
            // 
            this.numCutNo.BackColor = System.Drawing.Color.White;
            this.numCutNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numCutNo.Location = new System.Drawing.Point(273, 9);
            this.numCutNo.Name = "numCutNo";
            this.numCutNo.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCutNo.Size = new System.Drawing.Size(45, 23);
            this.numCutNo.TabIndex = 1;
            this.numCutNo.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtSizeCode
            // 
            this.txtSizeCode.BackColor = System.Drawing.Color.White;
            this.txtSizeCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSizeCode.Location = new System.Drawing.Point(273, 46);
            this.txtSizeCode.Name = "txtSizeCode";
            this.txtSizeCode.Size = new System.Drawing.Size(78, 23);
            this.txtSizeCode.TabIndex = 5;
            // 
            // txtEstCutDate
            // 
            this.txtEstCutDate.Location = new System.Drawing.Point(450, 46);
            this.txtEstCutDate.Name = "txtEstCutDate";
            this.txtEstCutDate.Size = new System.Drawing.Size(130, 23);
            this.txtEstCutDate.TabIndex = 6;
            // 
            // txtFabricCombo
            // 
            this.txtFabricCombo.BackColor = System.Drawing.Color.White;
            this.txtFabricCombo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFabricCombo.Location = new System.Drawing.Point(450, 9);
            this.txtFabricCombo.Name = "txtFabricCombo";
            this.txtFabricCombo.Size = new System.Drawing.Size(44, 23);
            this.txtFabricCombo.TabIndex = 2;
            // 
            // txtMarkerName
            // 
            this.txtMarkerName.BackColor = System.Drawing.Color.White;
            this.txtMarkerName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMarkerName.Location = new System.Drawing.Point(612, 9);
            this.txtMarkerName.Name = "txtMarkerName";
            this.txtMarkerName.Size = new System.Drawing.Size(60, 23);
            this.txtMarkerName.TabIndex = 3;
            // 
            // checkOnlyShowEmptyEstCutDate
            // 
            this.checkOnlyShowEmptyEstCutDate.AutoSize = true;
            this.checkOnlyShowEmptyEstCutDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkOnlyShowEmptyEstCutDate.Location = new System.Drawing.Point(729, 48);
            this.checkOnlyShowEmptyEstCutDate.Name = "checkOnlyShowEmptyEstCutDate";
            this.checkOnlyShowEmptyEstCutDate.Size = new System.Drawing.Size(221, 21);
            this.checkOnlyShowEmptyEstCutDate.TabIndex = 8;
            this.checkOnlyShowEmptyEstCutDate.Text = "Only show empty Est. Cut Date";
            this.checkOnlyShowEmptyEstCutDate.UseVisualStyleBackColor = true;
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(693, 9);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(80, 30);
            this.btnFilter.TabIndex = 9;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // labelBatchUpdateEstCutCell
            // 
            this.labelBatchUpdateEstCutCell.Location = new System.Drawing.Point(8, 87);
            this.labelBatchUpdateEstCutCell.Name = "labelBatchUpdateEstCutCell";
            this.labelBatchUpdateEstCutCell.Size = new System.Drawing.Size(92, 23);
            this.labelBatchUpdateEstCutCell.TabIndex = 19;
            this.labelBatchUpdateEstCutCell.Text = "Cut Cell";
            // 
            // txtBatchUpdateEstCutDate
            // 
            this.txtBatchUpdateEstCutDate.Location = new System.Drawing.Point(410, 87);
            this.txtBatchUpdateEstCutDate.Name = "txtBatchUpdateEstCutDate";
            this.txtBatchUpdateEstCutDate.Size = new System.Drawing.Size(130, 23);
            this.txtBatchUpdateEstCutDate.TabIndex = 12;
            this.txtBatchUpdateEstCutDate.Validating += new System.ComponentModel.CancelEventHandler(this.txtBatchUpdateEstCutDate_Validating);
            // 
            // labelBatchUpdateEstCutDate
            // 
            this.labelBatchUpdateEstCutDate.Location = new System.Drawing.Point(319, 87);
            this.labelBatchUpdateEstCutDate.Name = "labelBatchUpdateEstCutDate";
            this.labelBatchUpdateEstCutDate.Size = new System.Drawing.Size(88, 23);
            this.labelBatchUpdateEstCutDate.TabIndex = 21;
            this.labelBatchUpdateEstCutDate.Text = "Est. Cut Date";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(6, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1053, 75);
            this.panel1.TabIndex = 23;
            // 
            // btnBatchUpdateEstCutCell
            // 
            this.btnBatchUpdateEstCutCell.Location = new System.Drawing.Point(139, 83);
            this.btnBatchUpdateEstCutCell.Name = "btnBatchUpdateEstCutCell";
            this.btnBatchUpdateEstCutCell.Size = new System.Drawing.Size(175, 30);
            this.btnBatchUpdateEstCutCell.TabIndex = 11;
            this.btnBatchUpdateEstCutCell.Text = "Batch update Cut Cell";
            this.btnBatchUpdateEstCutCell.UseVisualStyleBackColor = true;
            this.btnBatchUpdateEstCutCell.Click += new System.EventHandler(this.btnBatchUpdateEstCutCell_Click);
            // 
            // btnBatchUpdateEstCutDate
            // 
            this.btnBatchUpdateEstCutDate.Location = new System.Drawing.Point(546, 83);
            this.btnBatchUpdateEstCutDate.Name = "btnBatchUpdateEstCutDate";
            this.btnBatchUpdateEstCutDate.Size = new System.Drawing.Size(199, 30);
            this.btnBatchUpdateEstCutDate.TabIndex = 13;
            this.btnBatchUpdateEstCutDate.Text = "Batch update Est. Cut Date";
            this.btnBatchUpdateEstCutDate.UseVisualStyleBackColor = true;
            this.btnBatchUpdateEstCutDate.Click += new System.EventHandler(this.btnBatchUpdateEstCutDate_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Size = new System.Drawing.Size(1072, 503);
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
            // txtCell2
            // 
            this.txtCell2.BackColor = System.Drawing.Color.White;
            this.txtCell2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCell2.Location = new System.Drawing.Point(103, 87);
            this.txtCell2.MDivisionID = "";
            this.txtCell2.Name = "txtCell2";
            this.txtCell2.Size = new System.Drawing.Size(30, 23);
            this.txtCell2.TabIndex = 10;
            // 
            // txtCutCell
            // 
            this.txtCutCell.BackColor = System.Drawing.Color.White;
            this.txtCutCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutCell.Location = new System.Drawing.Point(693, 46);
            this.txtCutCell.MDivisionID = "";
            this.txtCutCell.Name = "txtCutCell";
            this.txtCutCell.Size = new System.Drawing.Size(30, 23);
            this.txtCutCell.TabIndex = 7;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.AutoSize = true;
            this.btnConfirm.Location = new System.Drawing.Point(881, 467);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(80, 30);
            this.btnConfirm.TabIndex = 14;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnBatchUpdSeq
            // 
            this.btnBatchUpdSeq.Location = new System.Drawing.Point(916, 83);
            this.btnBatchUpdSeq.Name = "btnBatchUpdSeq";
            this.btnBatchUpdSeq.Size = new System.Drawing.Size(143, 30);
            this.btnBatchUpdSeq.TabIndex = 26;
            this.btnBatchUpdSeq.Text = "Batch update Seq";
            this.btnBatchUpdSeq.UseVisualStyleBackColor = true;
            this.btnBatchUpdSeq.Click += new System.EventHandler(this.btnBatchUpdSeq_Click);
            // 
            // labSeq
            // 
            this.labSeq.Location = new System.Drawing.Point(757, 87);
            this.labSeq.Name = "labSeq";
            this.labSeq.Size = new System.Drawing.Size(48, 23);
            this.labSeq.TabIndex = 27;
            this.labSeq.Text = "Seq";
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer1";
            this.shapeContainer2.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer2.Size = new System.Drawing.Size(1003, 503);
            this.shapeContainer2.TabIndex = 24;
            this.shapeContainer2.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label2.Location = new System.Drawing.Point(856, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 25);
            this.label2.TabIndex = 29;
            this.label2.Text = "-";
            // 
            // txtSeq1
            // 
            this.txtSeq1.BackColor = System.Drawing.Color.White;
            this.txtSeq1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq1.Location = new System.Drawing.Point(806, 87);
            this.txtSeq1.Name = "txtSeq1";
            this.txtSeq1.Size = new System.Drawing.Size(54, 23);
            this.txtSeq1.TabIndex = 30;
            this.txtSeq1.Validating += new System.ComponentModel.CancelEventHandler(this.txtSeq1_Validating);
            // 
            // txtSeq2
            // 
            this.txtSeq2.BackColor = System.Drawing.Color.White;
            this.txtSeq2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq2.Location = new System.Drawing.Point(869, 87);
            this.txtSeq2.Name = "txtSeq2";
            this.txtSeq2.Size = new System.Drawing.Size(41, 23);
            this.txtSeq2.TabIndex = 31;
            this.txtSeq2.Validating += new System.ComponentModel.CancelEventHandler(this.txtSeq2_Validating);
            // 
            // P02_BatchAssignCellCutDate
            // 
            this.ClientSize = new System.Drawing.Size(1072, 503);
            this.Controls.Add(this.txtSeq2);
            this.Controls.Add(this.txtSeq1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnBatchUpdSeq);
            this.Controls.Add(this.labSeq);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnBatchUpdateEstCutDate);
            this.Controls.Add(this.btnBatchUpdateEstCutCell);
            this.Controls.Add(this.txtBatchUpdateEstCutDate);
            this.Controls.Add(this.labelBatchUpdateEstCutDate);
            this.Controls.Add(this.txtCell2);
            this.Controls.Add(this.labelBatchUpdateEstCutCell);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.checkOnlyShowEmptyEstCutDate);
            this.Controls.Add(this.txtMarkerName);
            this.Controls.Add(this.txtFabricCombo);
            this.Controls.Add(this.txtEstCutDate);
            this.Controls.Add(this.txtSizeCode);
            this.Controls.Add(this.numCutNo);
            this.Controls.Add(this.txtCutCell);
            this.Controls.Add(this.txtArticle);
            this.Controls.Add(this.labelCutCell);
            this.Controls.Add(this.txtSPNo);
            this.Controls.Add(this.labelMarkerName);
            this.Controls.Add(this.labelEstCutDate);
            this.Controls.Add(this.labelFabricCombo);
            this.Controls.Add(this.labelCutNo);
            this.Controls.Add(this.labelSizeCode);
            this.Controls.Add(this.labelArticle);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridBatchAssignCellEstCutDate);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.shapeContainer1);
            this.DefaultControl = "txtSPNo";
            this.Name = "P02_BatchAssignCellCutDate";
            this.Text = "Batch Assign Cell/Est. Cut Date";
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchAssignCellEstCutDate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid gridBatchAssignCellEstCutDate;
        private Win.UI.Button btnClose;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelArticle;
        private Win.UI.Label labelSizeCode;
        private Win.UI.Label labelCutNo;
        private Win.UI.Label labelFabricCombo;
        private Win.UI.Label labelEstCutDate;
        private Win.UI.Label labelMarkerName;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelCutCell;
        private Win.UI.TextBox txtArticle;
        private Class.txtCell txtCutCell;
        private Win.UI.NumericBox numCutNo;
        private Win.UI.TextBox txtSizeCode;
        private Win.UI.DateBox txtEstCutDate;
        private Win.UI.TextBox txtFabricCombo;
        private Win.UI.TextBox txtMarkerName;
        private Win.UI.CheckBox checkOnlyShowEmptyEstCutDate;
        private Win.UI.Button btnFilter;
        private Class.txtCell txtCell2;
        private Win.UI.Label labelBatchUpdateEstCutCell;
        private Win.UI.DateBox txtBatchUpdateEstCutDate;
        private Win.UI.Label labelBatchUpdateEstCutDate;
        private Win.UI.Panel panel1;
        private Win.UI.Button btnBatchUpdateEstCutCell;
        private Win.UI.Button btnBatchUpdateEstCutDate;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.Button btnConfirm;
        private Win.UI.Button btnBatchUpdSeq;
        private Win.UI.Label labSeq;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private System.Windows.Forms.Label label2;
        private Win.UI.TextBox txtSeq1;
        private Win.UI.TextBox txtSeq2;
    }
}
