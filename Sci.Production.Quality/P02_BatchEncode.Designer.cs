namespace Sci.Production.Quality
{
    partial class P02_BatchEncode
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
            this.grid = new Sci.Win.UI.Grid();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.dateInspectDt = new Sci.Win.UI.DateBox();
            this.comboResult = new Sci.Win.UI.ComboBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.btnEncode = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.txtInspector = new Sci.Production.Class.Txtuser();
            this.numInspectRate = new Sci.Win.UI.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.radioInspected = new Sci.Win.UI.RadioButton();
            this.radioAQL = new Sci.Win.UI.RadioButton();
            this.radioMaterialType = new Sci.Win.UI.RadioButton();
            this.radioPanelInspected = new Sci.Win.UI.RadioPanel();
            this.comboDropDownList1 = new Sci.Production.Class.ComboDropDownList(this.components);
            this.editDefect = new Sci.Win.UI.EditBox();
            this.labelDefect = new Sci.Win.UI.Label();
            this.labReject = new Sci.Win.UI.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxRefno = new Sci.Win.UI.ComboBox();
            this.labRefno = new Sci.Win.UI.Label();
            this.comboBoxResultTop = new Sci.Win.UI.ComboBox();
            this.labResultTop = new Sci.Win.UI.Label();
            this.comboBoxWKNo = new Sci.Win.UI.ComboBox();
            this.labWKno = new Sci.Win.UI.Label();
            this.numRejectPercent = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInspectRate)).BeginInit();
            this.radioPanelInspected.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRejectPercent)).BeginInit();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(13, 39);
            this.grid.Name = "grid";
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(766, 213);
            this.grid.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(13, 264);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Inspected %";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.Location = new System.Drawing.Point(13, 374);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Inspector";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.Location = new System.Drawing.Point(13, 409);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Result";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.Location = new System.Drawing.Point(13, 345);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "Inspect Date";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.Location = new System.Drawing.Point(224, 409);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 23);
            this.label5.TabIndex = 6;
            this.label5.Text = "Remark";
            // 
            // dateInspectDt
            // 
            this.dateInspectDt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dateInspectDt.Location = new System.Drawing.Point(101, 345);
            this.dateInspectDt.Name = "dateInspectDt";
            this.dateInspectDt.Size = new System.Drawing.Size(130, 23);
            this.dateInspectDt.TabIndex = 9;
            // 
            // comboResult
            // 
            this.comboResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboResult.BackColor = System.Drawing.Color.White;
            this.comboResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboResult.FormattingEnabled = true;
            this.comboResult.IsSupportUnselect = true;
            this.comboResult.Location = new System.Drawing.Point(100, 408);
            this.comboResult.Name = "comboResult";
            this.comboResult.OldText = "";
            this.comboResult.Size = new System.Drawing.Size(121, 24);
            this.comboResult.TabIndex = 10;
            // 
            // txtRemark
            // 
            this.txtRemark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(312, 409);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(282, 23);
            this.txtRemark.TabIndex = 11;
            // 
            // btnEncode
            // 
            this.btnEncode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEncode.Location = new System.Drawing.Point(527, 440);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(80, 30);
            this.btnEncode.TabIndex = 13;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.BtnEncode_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(699, 440);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(613, 440);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // txtInspector
            // 
            this.txtInspector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtInspector.DisplayBox1Binding = "";
            this.txtInspector.Location = new System.Drawing.Point(101, 374);
            this.txtInspector.Name = "txtInspector";
            this.txtInspector.Size = new System.Drawing.Size(306, 23);
            this.txtInspector.TabIndex = 12;
            this.txtInspector.TextBox1Binding = "";
            // 
            // numInspectRate
            // 
            this.numInspectRate.BackColor = System.Drawing.Color.White;
            this.numInspectRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numInspectRate.Location = new System.Drawing.Point(91, 0);
            this.numInspectRate.Name = "numInspectRate";
            this.numInspectRate.Size = new System.Drawing.Size(93, 23);
            this.numInspectRate.TabIndex = 16;
            this.numInspectRate.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(186, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "%";
            // 
            // radioInspected
            // 
            this.radioInspected.AutoSize = true;
            this.radioInspected.Checked = true;
            this.radioInspected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioInspected.Location = new System.Drawing.Point(3, 0);
            this.radioInspected.Name = "radioInspected";
            this.radioInspected.Size = new System.Drawing.Size(87, 21);
            this.radioInspected.TabIndex = 18;
            this.radioInspected.TabStop = true;
            this.radioInspected.Text = "Inspected";
            this.radioInspected.UseVisualStyleBackColor = true;
            this.radioInspected.Value = "1";
            // 
            // radioAQL
            // 
            this.radioAQL.AutoSize = true;
            this.radioAQL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioAQL.Location = new System.Drawing.Point(3, 27);
            this.radioAQL.Name = "radioAQL";
            this.radioAQL.Size = new System.Drawing.Size(54, 21);
            this.radioAQL.TabIndex = 19;
            this.radioAQL.Text = "AQL";
            this.radioAQL.UseVisualStyleBackColor = true;
            this.radioAQL.Value = "2";
            // 
            // radioMaterialType
            // 
            this.radioMaterialType.AutoSize = true;
            this.radioMaterialType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioMaterialType.Location = new System.Drawing.Point(3, 54);
            this.radioMaterialType.Name = "radioMaterialType";
            this.radioMaterialType.Size = new System.Drawing.Size(216, 21);
            this.radioMaterialType.TabIndex = 20;
            this.radioMaterialType.Text = "Base on Material Type Setting";
            this.radioMaterialType.UseVisualStyleBackColor = true;
            this.radioMaterialType.Value = "3";
            // 
            // radioPanelInspected
            // 
            this.radioPanelInspected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioPanelInspected.Controls.Add(this.comboDropDownList1);
            this.radioPanelInspected.Controls.Add(this.radioInspected);
            this.radioPanelInspected.Controls.Add(this.label6);
            this.radioPanelInspected.Controls.Add(this.numInspectRate);
            this.radioPanelInspected.Controls.Add(this.radioAQL);
            this.radioPanelInspected.Controls.Add(this.radioMaterialType);
            this.radioPanelInspected.Location = new System.Drawing.Point(101, 264);
            this.radioPanelInspected.Name = "radioPanelInspected";
            this.radioPanelInspected.Size = new System.Drawing.Size(232, 75);
            this.radioPanelInspected.TabIndex = 22;
            this.radioPanelInspected.Value = "1";
            // 
            // comboDropDownList1
            // 
            this.comboDropDownList1.AddAllItem = false;
            this.comboDropDownList1.BackColor = System.Drawing.Color.White;
            this.comboDropDownList1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownList1.FormattingEnabled = true;
            this.comboDropDownList1.IsSupportUnselect = true;
            this.comboDropDownList1.Location = new System.Drawing.Point(63, 26);
            this.comboDropDownList1.Name = "comboDropDownList1";
            this.comboDropDownList1.OldText = "";
            this.comboDropDownList1.Size = new System.Drawing.Size(121, 24);
            this.comboDropDownList1.TabIndex = 23;
            this.comboDropDownList1.Type = "PMS_QA_AQL";
            // 
            // editDefect
            // 
            this.editDefect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editDefect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editDefect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editDefect.IsSupportEditMode = false;
            this.editDefect.Location = new System.Drawing.Point(489, 266);
            this.editDefect.Multiline = true;
            this.editDefect.Name = "editDefect";
            this.editDefect.ReadOnly = true;
            this.editDefect.Size = new System.Drawing.Size(288, 102);
            this.editDefect.TabIndex = 145;
            this.editDefect.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EditDefect_MouseDown);
            // 
            // labelDefect
            // 
            this.labelDefect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDefect.Location = new System.Drawing.Point(410, 266);
            this.labelDefect.Name = "labelDefect";
            this.labelDefect.Size = new System.Drawing.Size(76, 23);
            this.labelDefect.TabIndex = 144;
            this.labelDefect.Text = "Defect";
            // 
            // labReject
            // 
            this.labReject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labReject.Location = new System.Drawing.Point(410, 374);
            this.labReject.Name = "labReject";
            this.labReject.Size = new System.Drawing.Size(76, 23);
            this.labReject.TabIndex = 156;
            this.labReject.Text = "Rejected %";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(607, 377);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 17);
            this.label7.TabIndex = 158;
            this.label7.Text = "%";
            // 
            // comboBoxRefno
            // 
            this.comboBoxRefno.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxRefno.BackColor = System.Drawing.Color.White;
            this.comboBoxRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxRefno.FormattingEnabled = true;
            this.comboBoxRefno.IsSupportUnselect = true;
            this.comboBoxRefno.Location = new System.Drawing.Point(100, 9);
            this.comboBoxRefno.Name = "comboBoxRefno";
            this.comboBoxRefno.OldText = "";
            this.comboBoxRefno.Size = new System.Drawing.Size(142, 24);
            this.comboBoxRefno.TabIndex = 160;
            this.comboBoxRefno.SelectedValueChanged += new System.EventHandler(this.ComboBoxRefno_SelectedValueChanged);
            // 
            // labRefno
            // 
            this.labRefno.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labRefno.Location = new System.Drawing.Point(13, 10);
            this.labRefno.Name = "labRefno";
            this.labRefno.Size = new System.Drawing.Size(85, 23);
            this.labRefno.TabIndex = 159;
            this.labRefno.Text = "Refno";
            // 
            // comboBoxResultTop
            // 
            this.comboBoxResultTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxResultTop.BackColor = System.Drawing.Color.White;
            this.comboBoxResultTop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxResultTop.FormattingEnabled = true;
            this.comboBoxResultTop.IsSupportUnselect = true;
            this.comboBoxResultTop.Location = new System.Drawing.Point(349, 9);
            this.comboBoxResultTop.Name = "comboBoxResultTop";
            this.comboBoxResultTop.OldText = "";
            this.comboBoxResultTop.Size = new System.Drawing.Size(100, 24);
            this.comboBoxResultTop.TabIndex = 162;
            this.comboBoxResultTop.SelectedValueChanged += new System.EventHandler(this.ComboBoxResultTop_SelectedValueChanged);
            // 
            // labResultTop
            // 
            this.labResultTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labResultTop.Location = new System.Drawing.Point(262, 10);
            this.labResultTop.Name = "labResultTop";
            this.labResultTop.Size = new System.Drawing.Size(85, 23);
            this.labResultTop.TabIndex = 161;
            this.labResultTop.Text = "Result";
            // 
            // comboBoxWKNo
            // 
            this.comboBoxWKNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxWKNo.BackColor = System.Drawing.Color.White;
            this.comboBoxWKNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxWKNo.FormattingEnabled = true;
            this.comboBoxWKNo.IsSupportUnselect = true;
            this.comboBoxWKNo.Location = new System.Drawing.Point(554, 9);
            this.comboBoxWKNo.Name = "comboBoxWKNo";
            this.comboBoxWKNo.OldText = "";
            this.comboBoxWKNo.Size = new System.Drawing.Size(154, 24);
            this.comboBoxWKNo.TabIndex = 164;
            this.comboBoxWKNo.SelectedValueChanged += new System.EventHandler(this.ComboBoxWKNo_SelectedValueChanged);
            // 
            // labWKno
            // 
            this.labWKno.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labWKno.Location = new System.Drawing.Point(467, 10);
            this.labWKno.Name = "labWKno";
            this.labWKno.Size = new System.Drawing.Size(85, 23);
            this.labWKno.TabIndex = 163;
            this.labWKno.Text = "WK#";
            // 
            // numRejectPercent
            // 
            this.numRejectPercent.Location = new System.Drawing.Point(489, 375);
            this.numRejectPercent.Name = "numRejectPercent";
            this.numRejectPercent.Size = new System.Drawing.Size(120, 23);
            this.numRejectPercent.TabIndex = 165;
            this.numRejectPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // P02_BatchEncode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 479);
            this.Controls.Add(this.numRejectPercent);
            this.Controls.Add(this.comboBoxWKNo);
            this.Controls.Add(this.labWKno);
            this.Controls.Add(this.comboBoxResultTop);
            this.Controls.Add(this.labResultTop);
            this.Controls.Add(this.comboBoxRefno);
            this.Controls.Add(this.labRefno);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.labReject);
            this.Controls.Add(this.editDefect);
            this.Controls.Add(this.labelDefect);
            this.Controls.Add(this.radioPanelInspected);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnEncode);
            this.Controls.Add(this.txtInspector);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.comboResult);
            this.Controls.Add(this.dateInspectDt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grid);
            this.Name = "P02_BatchEncode";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Batch Encode";
            this.Controls.SetChildIndex(this.grid, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.dateInspectDt, 0);
            this.Controls.SetChildIndex(this.comboResult, 0);
            this.Controls.SetChildIndex(this.txtRemark, 0);
            this.Controls.SetChildIndex(this.txtInspector, 0);
            this.Controls.SetChildIndex(this.btnEncode, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.radioPanelInspected, 0);
            this.Controls.SetChildIndex(this.labelDefect, 0);
            this.Controls.SetChildIndex(this.editDefect, 0);
            this.Controls.SetChildIndex(this.labReject, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.labRefno, 0);
            this.Controls.SetChildIndex(this.comboBoxRefno, 0);
            this.Controls.SetChildIndex(this.labResultTop, 0);
            this.Controls.SetChildIndex(this.comboBoxResultTop, 0);
            this.Controls.SetChildIndex(this.labWKno, 0);
            this.Controls.SetChildIndex(this.comboBoxWKNo, 0);
            this.Controls.SetChildIndex(this.numRejectPercent, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInspectRate)).EndInit();
            this.radioPanelInspected.ResumeLayout(false);
            this.radioPanelInspected.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRejectPercent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid grid;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.DateBox dateInspectDt;
        private Win.UI.ComboBox comboResult;
        private Win.UI.TextBox txtRemark;
        private Class.Txtuser txtInspector;
        private Win.UI.Button btnEncode;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave;
        private Win.UI.NumericUpDown numInspectRate;
        private System.Windows.Forms.Label label6;
        private Win.UI.RadioButton radioInspected;
        private Win.UI.RadioButton radioAQL;
        private Win.UI.RadioButton radioMaterialType;
        private Win.UI.RadioPanel radioPanelInspected;
        private Class.ComboDropDownList comboDropDownList1;
        private Win.UI.EditBox editDefect;
        private Win.UI.Label labelDefect;
        private Win.UI.Label labReject;
        private System.Windows.Forms.Label label7;
        private Win.UI.ComboBox comboBoxRefno;
        private Win.UI.Label labRefno;
        private Win.UI.ComboBox comboBoxResultTop;
        private Win.UI.Label labResultTop;
        private Win.UI.ComboBox comboBoxWKNo;
        private Win.UI.Label labWKno;
        private System.Windows.Forms.NumericUpDown numRejectPercent;
    }
}