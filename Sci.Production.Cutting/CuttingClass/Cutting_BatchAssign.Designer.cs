﻿namespace Sci.Production.Cutting
{
    partial class Cutting_BatchAssign
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
            this.gridBatchAssign = new Sci.Win.UI.Grid();
            this.detailgridbs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnClose = new Sci.Win.UI.Button();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.btnConfirm = new Sci.Win.UI.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dateWKETA = new Sci.Win.UI.DateBox();
            this.dateBoxEstCutDate = new Sci.Production.Class.DateEstCutDate();
            this.panel_P09 = new System.Windows.Forms.Panel();
            this.label5 = new Sci.Win.UI.Label();
            this.txtSpreadingNo = new Sci.Production.Class.TxtSpreadingNo();
            this.txtMarkerLength = new Sci.Win.UI.TextBox();
            this.txtMakerName = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.labelBatchUpdateEstCutDate = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.btnBatchUpdateEstCutDate = new Sci.Win.UI.Button();
            this.labSeq = new Sci.Win.UI.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSeq2 = new Sci.Win.UI.TextBox();
            this.txtSeq1 = new Sci.Win.UI.TextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.txtArticle = new Sci.Win.UI.TextBox();
            this.txtMarkerName_Filter = new Sci.Win.UI.TextBox();
            this.txtSizeCode = new Sci.Win.UI.TextBox();
            this.labelMarkerName = new Sci.Win.UI.Label();
            this.labelSizeCode = new Sci.Win.UI.Label();
            this.labelArticle = new Sci.Win.UI.Label();
            this.checkOnlyShowEmptyEstCutDate = new Sci.Win.UI.CheckBox();
            this.btnFilter = new Sci.Win.UI.Button();
            this.labelEstCutDate = new Sci.Win.UI.Label();
            this.txtFabricPanelCode = new Sci.Win.UI.TextBox();
            this.DateEstCutDate = new Sci.Win.UI.DateBox();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelFabricCombo = new Sci.Win.UI.Label();
            this.label17 = new Sci.Win.UI.Label();
            this.txtCell = new Sci.Production.Class.TxtCell();
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchAssign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel_P09.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridBatchAssign
            // 
            this.gridBatchAssign.AllowUserToAddRows = false;
            this.gridBatchAssign.AllowUserToDeleteRows = false;
            this.gridBatchAssign.AllowUserToResizeRows = false;
            this.gridBatchAssign.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridBatchAssign.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBatchAssign.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBatchAssign.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBatchAssign.DataSource = this.detailgridbs;
            this.gridBatchAssign.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBatchAssign.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBatchAssign.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBatchAssign.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBatchAssign.Location = new System.Drawing.Point(3, 233);
            this.gridBatchAssign.Name = "gridBatchAssign";
            this.gridBatchAssign.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBatchAssign.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBatchAssign.RowTemplate.Height = 24;
            this.gridBatchAssign.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBatchAssign.ShowCellToolTips = false;
            this.gridBatchAssign.Size = new System.Drawing.Size(1006, 408);
            this.gridBatchAssign.TabIndex = 0;
            this.gridBatchAssign.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.AutoSize = true;
            this.btnClose.Location = new System.Drawing.Point(920, 644);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
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
            this.btnConfirm.Location = new System.Drawing.Point(823, 644);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(80, 30);
            this.btnConfirm.TabIndex = 22;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtCell);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.dateWKETA);
            this.panel1.Controls.Add(this.dateBoxEstCutDate);
            this.panel1.Controls.Add(this.panel_P09);
            this.panel1.Controls.Add(this.txtMarkerLength);
            this.panel1.Controls.Add(this.txtMakerName);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelBatchUpdateEstCutDate);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnBatchUpdateEstCutDate);
            this.panel1.Controls.Add(this.labSeq);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtSeq2);
            this.panel1.Controls.Add(this.txtSeq1);
            this.panel1.Location = new System.Drawing.Point(3, 128);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1006, 99);
            this.panel1.TabIndex = 50;
            // 
            // dateWKETA
            // 
            this.dateWKETA.Location = new System.Drawing.Point(102, 34);
            this.dateWKETA.Name = "dateWKETA";
            this.dateWKETA.Size = new System.Drawing.Size(130, 23);
            this.dateWKETA.TabIndex = 7;
            // 
            // dateBoxEstCutDate
            // 
            this.dateBoxEstCutDate.Location = new System.Drawing.Point(102, 5);
            this.dateBoxEstCutDate.Name = "dateBoxEstCutDate";
            this.dateBoxEstCutDate.Size = new System.Drawing.Size(130, 23);
            this.dateBoxEstCutDate.TabIndex = 77;
            // 
            // panel_P09
            // 
            this.panel_P09.Controls.Add(this.label5);
            this.panel_P09.Controls.Add(this.txtSpreadingNo);
            this.panel_P09.Location = new System.Drawing.Point(464, 63);
            this.panel_P09.Name = "panel_P09";
            this.panel_P09.Size = new System.Drawing.Size(374, 23);
            this.panel_P09.TabIndex = 75;
            this.panel_P09.Visible = false;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 23);
            this.label5.TabIndex = 72;
            this.label5.Text = "Spreading No";
            // 
            // txtSpreadingNo
            // 
            this.txtSpreadingNo.BackColor = System.Drawing.Color.White;
            this.txtSpreadingNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpreadingNo.IncludeJunk = true;
            this.txtSpreadingNo.Location = new System.Drawing.Point(99, 0);
            this.txtSpreadingNo.MDivision = "";
            this.txtSpreadingNo.Name = "txtSpreadingNo";
            this.txtSpreadingNo.Size = new System.Drawing.Size(108, 23);
            this.txtSpreadingNo.TabIndex = 71;
            // 
            // txtMarkerLength
            // 
            this.txtMarkerLength.BackColor = System.Drawing.Color.White;
            this.txtMarkerLength.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMarkerLength.Location = new System.Drawing.Point(344, 34);
            this.txtMarkerLength.Mask = "00Y00-0/0+0\"";
            this.txtMarkerLength.Name = "txtMarkerLength";
            this.txtMarkerLength.Size = new System.Drawing.Size(108, 23);
            this.txtMarkerLength.TabIndex = 46;
            this.txtMarkerLength.Validating += new System.ComponentModel.CancelEventHandler(this.TxtMakerLength_Validating);
            // 
            // txtMakerName
            // 
            this.txtMakerName.BackColor = System.Drawing.Color.White;
            this.txtMakerName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMakerName.Location = new System.Drawing.Point(344, 5);
            this.txtMakerName.Name = "txtMakerName";
            this.txtMakerName.Size = new System.Drawing.Size(108, 23);
            this.txtMakerName.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(245, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 23);
            this.label4.TabIndex = 45;
            this.label4.Text = "Marker Length";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(245, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 23);
            this.label1.TabIndex = 44;
            this.label1.Text = "Marker Name";
            // 
            // labelBatchUpdateEstCutDate
            // 
            this.labelBatchUpdateEstCutDate.Location = new System.Drawing.Point(11, 5);
            this.labelBatchUpdateEstCutDate.Name = "labelBatchUpdateEstCutDate";
            this.labelBatchUpdateEstCutDate.Size = new System.Drawing.Size(88, 23);
            this.labelBatchUpdateEstCutDate.TabIndex = 21;
            this.labelBatchUpdateEstCutDate.Text = "Est. Cut Date";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(11, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 23);
            this.label3.TabIndex = 43;
            this.label3.Text = "WK ETA";
            // 
            // btnBatchUpdateEstCutDate
            // 
            this.btnBatchUpdateEstCutDate.Location = new System.Drawing.Point(494, 5);
            this.btnBatchUpdateEstCutDate.Name = "btnBatchUpdateEstCutDate";
            this.btnBatchUpdateEstCutDate.Size = new System.Drawing.Size(125, 30);
            this.btnBatchUpdateEstCutDate.TabIndex = 12;
            this.btnBatchUpdateEstCutDate.Text = "Batch Assign";
            this.btnBatchUpdateEstCutDate.UseVisualStyleBackColor = true;
            this.btnBatchUpdateEstCutDate.Click += new System.EventHandler(this.BtnBatchUpdate_Click);
            // 
            // labSeq
            // 
            this.labSeq.Location = new System.Drawing.Point(11, 63);
            this.labSeq.Name = "labSeq";
            this.labSeq.Size = new System.Drawing.Size(88, 23);
            this.labSeq.TabIndex = 27;
            this.labSeq.Text = "SEQ1-SEQ2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label2.Location = new System.Drawing.Point(146, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 25);
            this.label2.TabIndex = 29;
            this.label2.Text = "-";
            // 
            // txtSeq2
            // 
            this.txtSeq2.BackColor = System.Drawing.Color.White;
            this.txtSeq2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq2.Location = new System.Drawing.Point(168, 63);
            this.txtSeq2.Name = "txtSeq2";
            this.txtSeq2.Size = new System.Drawing.Size(41, 23);
            this.txtSeq2.TabIndex = 14;
            this.txtSeq2.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSeq_PopUp);
            this.txtSeq2.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSeq_Validating);
            // 
            // txtSeq1
            // 
            this.txtSeq1.BackColor = System.Drawing.Color.White;
            this.txtSeq1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq1.Location = new System.Drawing.Point(102, 63);
            this.txtSeq1.Name = "txtSeq1";
            this.txtSeq1.Size = new System.Drawing.Size(41, 23);
            this.txtSeq1.TabIndex = 13;
            this.txtSeq1.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSeq_PopUp);
            this.txtSeq1.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSeq_Validating);
            // 
            // panel7
            // 
            this.panel7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.labelSPNo);
            this.panel7.Controls.Add(this.txtArticle);
            this.panel7.Controls.Add(this.txtMarkerName_Filter);
            this.panel7.Controls.Add(this.txtSizeCode);
            this.panel7.Controls.Add(this.labelMarkerName);
            this.panel7.Controls.Add(this.labelSizeCode);
            this.panel7.Controls.Add(this.labelArticle);
            this.panel7.Controls.Add(this.checkOnlyShowEmptyEstCutDate);
            this.panel7.Controls.Add(this.btnFilter);
            this.panel7.Controls.Add(this.labelEstCutDate);
            this.panel7.Controls.Add(this.txtFabricPanelCode);
            this.panel7.Controls.Add(this.DateEstCutDate);
            this.panel7.Controls.Add(this.txtSPNo);
            this.panel7.Controls.Add(this.labelFabricCombo);
            this.panel7.Location = new System.Drawing.Point(3, 5);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1006, 117);
            this.panel7.TabIndex = 49;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(11, 3);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(123, 23);
            this.labelSPNo.TabIndex = 2;
            this.labelSPNo.Text = "SP#";
            // 
            // txtArticle
            // 
            this.txtArticle.BackColor = System.Drawing.Color.White;
            this.txtArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtArticle.Location = new System.Drawing.Point(138, 32);
            this.txtArticle.Name = "txtArticle";
            this.txtArticle.Size = new System.Drawing.Size(108, 23);
            this.txtArticle.TabIndex = 4;
            // 
            // txtMarkerName_Filter
            // 
            this.txtMarkerName_Filter.BackColor = System.Drawing.Color.White;
            this.txtMarkerName_Filter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMarkerName_Filter.Location = new System.Drawing.Point(340, 61);
            this.txtMarkerName_Filter.Name = "txtMarkerName_Filter";
            this.txtMarkerName_Filter.Size = new System.Drawing.Size(108, 23);
            this.txtMarkerName_Filter.TabIndex = 3;
            // 
            // txtSizeCode
            // 
            this.txtSizeCode.BackColor = System.Drawing.Color.White;
            this.txtSizeCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSizeCode.Location = new System.Drawing.Point(340, 32);
            this.txtSizeCode.Name = "txtSizeCode";
            this.txtSizeCode.Size = new System.Drawing.Size(109, 23);
            this.txtSizeCode.TabIndex = 5;
            // 
            // labelMarkerName
            // 
            this.labelMarkerName.Location = new System.Drawing.Point(249, 61);
            this.labelMarkerName.Name = "labelMarkerName";
            this.labelMarkerName.Size = new System.Drawing.Size(88, 23);
            this.labelMarkerName.TabIndex = 7;
            this.labelMarkerName.Text = "Marker Name";
            // 
            // labelSizeCode
            // 
            this.labelSizeCode.Location = new System.Drawing.Point(249, 32);
            this.labelSizeCode.Name = "labelSizeCode";
            this.labelSizeCode.Size = new System.Drawing.Size(88, 23);
            this.labelSizeCode.TabIndex = 4;
            this.labelSizeCode.Text = "SizeCode";
            // 
            // labelArticle
            // 
            this.labelArticle.Location = new System.Drawing.Point(11, 32);
            this.labelArticle.Name = "labelArticle";
            this.labelArticle.Size = new System.Drawing.Size(123, 23);
            this.labelArticle.TabIndex = 3;
            this.labelArticle.Text = "Article";
            // 
            // checkOnlyShowEmptyEstCutDate
            // 
            this.checkOnlyShowEmptyEstCutDate.AutoSize = true;
            this.checkOnlyShowEmptyEstCutDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkOnlyShowEmptyEstCutDate.Location = new System.Drawing.Point(11, 90);
            this.checkOnlyShowEmptyEstCutDate.Name = "checkOnlyShowEmptyEstCutDate";
            this.checkOnlyShowEmptyEstCutDate.Size = new System.Drawing.Size(221, 21);
            this.checkOnlyShowEmptyEstCutDate.TabIndex = 8;
            this.checkOnlyShowEmptyEstCutDate.Text = "Only show empty Est. Cut Date";
            this.checkOnlyShowEmptyEstCutDate.UseVisualStyleBackColor = true;
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(494, 3);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(80, 30);
            this.btnFilter.TabIndex = 24;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.BtnFilter_Click);
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Location = new System.Drawing.Point(249, 3);
            this.labelEstCutDate.Name = "labelEstCutDate";
            this.labelEstCutDate.Size = new System.Drawing.Size(88, 23);
            this.labelEstCutDate.TabIndex = 6;
            this.labelEstCutDate.Text = "Est. Cut Date";
            // 
            // txtFabricPanelCode
            // 
            this.txtFabricPanelCode.BackColor = System.Drawing.Color.White;
            this.txtFabricPanelCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFabricPanelCode.Location = new System.Drawing.Point(138, 61);
            this.txtFabricPanelCode.Name = "txtFabricPanelCode";
            this.txtFabricPanelCode.Size = new System.Drawing.Size(108, 23);
            this.txtFabricPanelCode.TabIndex = 2;
            // 
            // DateEstCutDate
            // 
            this.DateEstCutDate.Location = new System.Drawing.Point(340, 3);
            this.DateEstCutDate.Name = "DateEstCutDate";
            this.DateEstCutDate.Size = new System.Drawing.Size(130, 23);
            this.DateEstCutDate.TabIndex = 6;
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(138, 3);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(108, 23);
            this.txtSPNo.TabIndex = 0;
            this.txtSPNo.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSPNo_PopUp);
            // 
            // labelFabricCombo
            // 
            this.labelFabricCombo.Location = new System.Drawing.Point(11, 61);
            this.labelFabricCombo.Name = "labelFabricCombo";
            this.labelFabricCombo.Size = new System.Drawing.Size(123, 23);
            this.labelFabricCombo.TabIndex = 5;
            this.labelFabricCombo.Text = "Fabric Panel Code";
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label17.Location = new System.Drawing.Point(245, 63);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(96, 23);
            this.label17.TabIndex = 78;
            this.label17.Text = "Cut Cell";
            // 
            // txtCell
            // 
            this.txtCell.BackColor = System.Drawing.Color.White;
            this.txtCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCell.Location = new System.Drawing.Point(344, 63);
            this.txtCell.MDivisionID = "";
            this.txtCell.Name = "txtCell";
            this.txtCell.Size = new System.Drawing.Size(108, 23);
            this.txtCell.TabIndex = 79;
            // 
            // Cutting_BatchAssign
            // 
            this.ClientSize = new System.Drawing.Size(1012, 679);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gridBatchAssign);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnClose);
            this.DefaultControl = "txtSPNo";
            this.Name = "Cutting_BatchAssign";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Batch Assign";
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchAssign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel_P09.ResumeLayout(false);
            this.panel_P09.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid gridBatchAssign;
        private Win.UI.Button btnClose;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.Button btnConfirm;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private Win.UI.ListControlBindingSource detailgridbs;
        private System.Windows.Forms.Panel panel1;
        private Win.UI.Label labelBatchUpdateEstCutDate;
        private Win.UI.Label label3;
        private Win.UI.Button btnBatchUpdateEstCutDate;
        private Win.UI.Label labSeq;
        private System.Windows.Forms.Label label2;
        private Win.UI.TextBox txtSeq2;
        private Win.UI.TextBox txtSeq1;
        private System.Windows.Forms.Panel panel7;
        private Win.UI.Label labelSPNo;
        private Win.UI.TextBox txtArticle;
        private Win.UI.TextBox txtMarkerName_Filter;
        private Win.UI.TextBox txtSizeCode;
        private Win.UI.Label labelMarkerName;
        private Win.UI.Label labelSizeCode;
        private Win.UI.Label labelArticle;
        private Win.UI.CheckBox checkOnlyShowEmptyEstCutDate;
        private Win.UI.Button btnFilter;
        private Win.UI.Label labelEstCutDate;
        private Win.UI.TextBox txtFabricPanelCode;
        private Win.UI.DateBox DateEstCutDate;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelFabricCombo;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtMarkerLength;
        private Win.UI.TextBox txtMakerName;
        private Win.UI.Label label4;
        private Class.TxtSpreadingNo txtSpreadingNo;
        private Win.UI.Label label5;
        private System.Windows.Forms.Panel panel_P09;
        private Class.DateEstCutDate dateBoxEstCutDate;
        private Win.UI.DateBox dateWKETA;
        private Class.TxtCell txtCell;
        private Win.UI.Label label17;
    }
}
