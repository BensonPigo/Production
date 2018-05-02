﻿namespace Sci.Production.Packing
{
    partial class P18
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelPackID = new Sci.Win.UI.Label();
            this.labelCtnNo = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.labelPO = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelPackedCartons = new Sci.Win.UI.Label();
            this.labelttlCatons = new Sci.Win.UI.Label();
            this.labelttlQty = new Sci.Win.UI.Label();
            this.labelttlPackQty = new Sci.Win.UI.Label();
            this.labelRemainCartons = new Sci.Win.UI.Label();
            this.labelRemainQty = new Sci.Win.UI.Label();
            this.displayPackID = new Sci.Win.UI.DisplayBox();
            this.displayCtnNo = new Sci.Win.UI.DisplayBox();
            this.displaySPNo = new Sci.Win.UI.DisplayBox();
            this.displayPoNo = new Sci.Win.UI.DisplayBox();
            this.displayBrand = new Sci.Win.UI.DisplayBox();
            this.displayStyle = new Sci.Win.UI.DisplayBox();
            this.tabControlScanArea = new Sci.Win.UI.TabControl();
            this.tabPageCarton = new System.Windows.Forms.TabPage();
            this.txtScanCartonSP = new Sci.Win.UI.TextBox();
            this.labelTabCarton = new Sci.Win.UI.Label();
            this.tabPageScan = new System.Windows.Forms.TabPage();
            this.numBoxScanQty = new Sci.Win.UI.NumericBox();
            this.numBoxScanTtlQty = new Sci.Win.UI.NumericBox();
            this.txtScanEAN = new Sci.Win.UI.TextBox();
            this.labelEAN = new Sci.Win.UI.Label();
            this.labelQtyScan = new Sci.Win.UI.Label();
            this.labelTabScanTtlQty = new Sci.Win.UI.Label();
            this.labelTabScan = new Sci.Win.UI.Label();
            this.gridScanDetail = new Sci.Win.UI.Grid();
            this.label1 = new Sci.Win.UI.Label();
            this.chkBoxNotScan = new Sci.Win.UI.CheckBox();
            this.labelPKFilter = new Sci.Win.UI.Label();
            this.comboPKFilter = new Sci.Win.UI.ComboBox();
            this.labelQuickSelCTN = new Sci.Win.UI.Label();
            this.txtQuickSelCTN = new Sci.Win.UI.TextBox();
            this.gridSelectCartonDetail = new Sci.Win.UI.Grid();
            this.selcartonBS = new Sci.Win.UI.BindingSource(this.components);
            this.scanDetailBS = new Sci.Win.UI.BindingSource(this.components);
            this.numBoxttlCatons = new Sci.Win.UI.NumericBox();
            this.numBoxttlQty = new Sci.Win.UI.NumericBox();
            this.numBoxPackedCartons = new Sci.Win.UI.NumericBox();
            this.numBoxttlPackQty = new Sci.Win.UI.NumericBox();
            this.numBoxRemainCartons = new Sci.Win.UI.NumericBox();
            this.numBoxRemainQty = new Sci.Win.UI.NumericBox();
            this.tabControlScanArea.SuspendLayout();
            this.tabPageCarton.SuspendLayout();
            this.tabPageScan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridScanDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSelectCartonDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selcartonBS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scanDetailBS)).BeginInit();
            this.SuspendLayout();
            // 
            // labelPackID
            // 
            this.labelPackID.Location = new System.Drawing.Point(9, 9);
            this.labelPackID.Name = "labelPackID";
            this.labelPackID.Size = new System.Drawing.Size(56, 23);
            this.labelPackID.TabIndex = 1;
            this.labelPackID.Text = "Pack ID";
            // 
            // labelCtnNo
            // 
            this.labelCtnNo.Location = new System.Drawing.Point(9, 38);
            this.labelCtnNo.Name = "labelCtnNo";
            this.labelCtnNo.Size = new System.Drawing.Size(56, 23);
            this.labelCtnNo.TabIndex = 2;
            this.labelCtnNo.Text = "CTN#";
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(9, 67);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(56, 23);
            this.labelSP.TabIndex = 3;
            this.labelSP.Text = "SP#";
            // 
            // labelPO
            // 
            this.labelPO.Location = new System.Drawing.Point(9, 96);
            this.labelPO.Name = "labelPO";
            this.labelPO.Size = new System.Drawing.Size(56, 23);
            this.labelPO.TabIndex = 4;
            this.labelPO.Text = "PO#";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(9, 125);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(56, 23);
            this.labelBrand.TabIndex = 5;
            this.labelBrand.Text = "Brand";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(173, 125);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(56, 23);
            this.labelStyle.TabIndex = 6;
            this.labelStyle.Text = "Style";
            // 
            // labelPackedCartons
            // 
            this.labelPackedCartons.Location = new System.Drawing.Point(345, 67);
            this.labelPackedCartons.Name = "labelPackedCartons";
            this.labelPackedCartons.Size = new System.Drawing.Size(118, 23);
            this.labelPackedCartons.TabIndex = 7;
            this.labelPackedCartons.Text = "Packed Cartons";
            // 
            // labelttlCatons
            // 
            this.labelttlCatons.Location = new System.Drawing.Point(345, 9);
            this.labelttlCatons.Name = "labelttlCatons";
            this.labelttlCatons.Size = new System.Drawing.Size(118, 23);
            this.labelttlCatons.TabIndex = 8;
            this.labelttlCatons.Text = "Total Cartons";
            // 
            // labelttlQty
            // 
            this.labelttlQty.Location = new System.Drawing.Point(345, 38);
            this.labelttlQty.Name = "labelttlQty";
            this.labelttlQty.Size = new System.Drawing.Size(118, 23);
            this.labelttlQty.TabIndex = 9;
            this.labelttlQty.Text = "Total Quantity";
            // 
            // labelttlPackQty
            // 
            this.labelttlPackQty.Location = new System.Drawing.Point(345, 96);
            this.labelttlPackQty.Name = "labelttlPackQty";
            this.labelttlPackQty.Size = new System.Drawing.Size(118, 23);
            this.labelttlPackQty.TabIndex = 10;
            this.labelttlPackQty.Text = "Total Packed Qty";
            // 
            // labelRemainCartons
            // 
            this.labelRemainCartons.Location = new System.Drawing.Point(345, 125);
            this.labelRemainCartons.Name = "labelRemainCartons";
            this.labelRemainCartons.Size = new System.Drawing.Size(118, 23);
            this.labelRemainCartons.TabIndex = 11;
            this.labelRemainCartons.Text = "Remain Cartons";
            // 
            // labelRemainQty
            // 
            this.labelRemainQty.Location = new System.Drawing.Point(345, 154);
            this.labelRemainQty.Name = "labelRemainQty";
            this.labelRemainQty.Size = new System.Drawing.Size(118, 23);
            this.labelRemainQty.TabIndex = 12;
            this.labelRemainQty.Text = "Remain Quantity";
            // 
            // displayPackID
            // 
            this.displayPackID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPackID.Location = new System.Drawing.Point(68, 9);
            this.displayPackID.Name = "displayPackID";
            this.displayPackID.Size = new System.Drawing.Size(260, 23);
            this.displayPackID.TabIndex = 15;
            // 
            // displayCtnNo
            // 
            this.displayCtnNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCtnNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCtnNo.Location = new System.Drawing.Point(68, 38);
            this.displayCtnNo.Name = "displayCtnNo";
            this.displayCtnNo.Size = new System.Drawing.Size(96, 23);
            this.displayCtnNo.TabIndex = 16;
            // 
            // displaySPNo
            // 
            this.displaySPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySPNo.Location = new System.Drawing.Point(68, 67);
            this.displaySPNo.Name = "displaySPNo";
            this.displaySPNo.Size = new System.Drawing.Size(260, 23);
            this.displaySPNo.TabIndex = 17;
            // 
            // displayPoNo
            // 
            this.displayPoNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPoNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPoNo.Location = new System.Drawing.Point(68, 96);
            this.displayPoNo.Name = "displayPoNo";
            this.displayPoNo.Size = new System.Drawing.Size(260, 23);
            this.displayPoNo.TabIndex = 18;
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(68, 125);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(96, 23);
            this.displayBrand.TabIndex = 19;
            // 
            // displayStyle
            // 
            this.displayStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyle.Location = new System.Drawing.Point(232, 125);
            this.displayStyle.Name = "displayStyle";
            this.displayStyle.Size = new System.Drawing.Size(96, 23);
            this.displayStyle.TabIndex = 20;
            // 
            // tabControlScanArea
            // 
            this.tabControlScanArea.Controls.Add(this.tabPageCarton);
            this.tabControlScanArea.Controls.Add(this.tabPageScan);
            this.tabControlScanArea.Location = new System.Drawing.Point(9, 179);
            this.tabControlScanArea.Name = "tabControlScanArea";
            this.tabControlScanArea.SelectedIndex = 0;
            this.tabControlScanArea.Size = new System.Drawing.Size(319, 196);
            this.tabControlScanArea.TabIndex = 99;
            this.tabControlScanArea.TabStop = false;
            // 
            // tabPageCarton
            // 
            this.tabPageCarton.Controls.Add(this.txtScanCartonSP);
            this.tabPageCarton.Controls.Add(this.labelTabCarton);
            this.tabPageCarton.Location = new System.Drawing.Point(4, 25);
            this.tabPageCarton.Name = "tabPageCarton";
            this.tabPageCarton.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCarton.Size = new System.Drawing.Size(311, 167);
            this.tabPageCarton.TabIndex = 0;
            this.tabPageCarton.Text = "Carton";
            // 
            // txtScanCartonSP
            // 
            this.txtScanCartonSP.BackColor = System.Drawing.Color.White;
            this.txtScanCartonSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtScanCartonSP.Location = new System.Drawing.Point(14, 65);
            this.txtScanCartonSP.Name = "txtScanCartonSP";
            this.txtScanCartonSP.Size = new System.Drawing.Size(283, 23);
            this.txtScanCartonSP.TabIndex = 1;
            this.txtScanCartonSP.Validating += new System.ComponentModel.CancelEventHandler(this.TxtScanCartonSP_Validating);
            // 
            // labelTabCarton
            // 
            this.labelTabCarton.BackColor = System.Drawing.Color.Transparent;
            this.labelTabCarton.Location = new System.Drawing.Point(36, 32);
            this.labelTabCarton.Name = "labelTabCarton";
            this.labelTabCarton.Size = new System.Drawing.Size(246, 23);
            this.labelTabCarton.TabIndex = 0;
            this.labelTabCarton.Text = "Please Scan Carton Barcode/SP#/PO#";
            this.labelTabCarton.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // tabPageScan
            // 
            this.tabPageScan.Controls.Add(this.numBoxScanQty);
            this.tabPageScan.Controls.Add(this.numBoxScanTtlQty);
            this.tabPageScan.Controls.Add(this.txtScanEAN);
            this.tabPageScan.Controls.Add(this.labelEAN);
            this.tabPageScan.Controls.Add(this.labelQtyScan);
            this.tabPageScan.Controls.Add(this.labelTabScanTtlQty);
            this.tabPageScan.Controls.Add(this.labelTabScan);
            this.tabPageScan.Location = new System.Drawing.Point(4, 25);
            this.tabPageScan.Name = "tabPageScan";
            this.tabPageScan.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageScan.Size = new System.Drawing.Size(311, 167);
            this.tabPageScan.TabIndex = 1;
            this.tabPageScan.Text = "Scan";
            // 
            // numBoxScanQty
            // 
            this.numBoxScanQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numBoxScanQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.numBoxScanQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numBoxScanQty.IsSupportEditMode = false;
            this.numBoxScanQty.Location = new System.Drawing.Point(110, 77);
            this.numBoxScanQty.Name = "numBoxScanQty";
            this.numBoxScanQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBoxScanQty.ReadOnly = true;
            this.numBoxScanQty.Size = new System.Drawing.Size(184, 23);
            this.numBoxScanQty.TabIndex = 33;
            this.numBoxScanQty.TabStop = false;
            this.numBoxScanQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numBoxScanTtlQty
            // 
            this.numBoxScanTtlQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numBoxScanTtlQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.numBoxScanTtlQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numBoxScanTtlQty.IsSupportEditMode = false;
            this.numBoxScanTtlQty.Location = new System.Drawing.Point(110, 109);
            this.numBoxScanTtlQty.Name = "numBoxScanTtlQty";
            this.numBoxScanTtlQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBoxScanTtlQty.ReadOnly = true;
            this.numBoxScanTtlQty.Size = new System.Drawing.Size(184, 23);
            this.numBoxScanTtlQty.TabIndex = 32;
            this.numBoxScanTtlQty.TabStop = false;
            this.numBoxScanTtlQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtScanEAN
            // 
            this.txtScanEAN.BackColor = System.Drawing.Color.White;
            this.txtScanEAN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtScanEAN.Location = new System.Drawing.Point(110, 44);
            this.txtScanEAN.Name = "txtScanEAN";
            this.txtScanEAN.Size = new System.Drawing.Size(184, 23);
            this.txtScanEAN.TabIndex = 1;
            this.txtScanEAN.Validating += new System.ComponentModel.CancelEventHandler(this.TxtScanEAN_Validating);
            // 
            // labelEAN
            // 
            this.labelEAN.Location = new System.Drawing.Point(15, 44);
            this.labelEAN.Name = "labelEAN";
            this.labelEAN.Size = new System.Drawing.Size(92, 23);
            this.labelEAN.TabIndex = 30;
            this.labelEAN.Text = "UPC/EAN";
            // 
            // labelQtyScan
            // 
            this.labelQtyScan.Location = new System.Drawing.Point(15, 77);
            this.labelQtyScan.Name = "labelQtyScan";
            this.labelQtyScan.Size = new System.Drawing.Size(92, 23);
            this.labelQtyScan.TabIndex = 29;
            this.labelQtyScan.Text = "Quantity Scan";
            // 
            // labelTabScanTtlQty
            // 
            this.labelTabScanTtlQty.Location = new System.Drawing.Point(15, 110);
            this.labelTabScanTtlQty.Name = "labelTabScanTtlQty";
            this.labelTabScanTtlQty.Size = new System.Drawing.Size(92, 23);
            this.labelTabScanTtlQty.TabIndex = 28;
            this.labelTabScanTtlQty.Text = "Total Quantity";
            // 
            // labelTabScan
            // 
            this.labelTabScan.BackColor = System.Drawing.Color.Transparent;
            this.labelTabScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.labelTabScan.Location = new System.Drawing.Point(6, 9);
            this.labelTabScan.Name = "labelTabScan";
            this.labelTabScan.Size = new System.Drawing.Size(210, 23);
            this.labelTabScan.TabIndex = 28;
            this.labelTabScan.Text = "Please Scan Inner Label";
            this.labelTabScan.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // gridScanDetail
            // 
            this.gridScanDetail.AllowUserToAddRows = false;
            this.gridScanDetail.AllowUserToDeleteRows = false;
            this.gridScanDetail.AllowUserToResizeRows = false;
            this.gridScanDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridScanDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridScanDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridScanDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridScanDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridScanDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridScanDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridScanDetail.Location = new System.Drawing.Point(345, 203);
            this.gridScanDetail.Name = "gridScanDetail";
            this.gridScanDetail.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridScanDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridScanDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridScanDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridScanDetail.RowTemplate.Height = 24;
            this.gridScanDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridScanDetail.ShowCellToolTips = false;
            this.gridScanDetail.Size = new System.Drawing.Size(577, 172);
            this.gridScanDetail.TabIndex = 28;
            this.gridScanDetail.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(13, 378);
            this.label1.Name = "label1";
            this.label1.RectStyle.BorderColors.Bottom = System.Drawing.Color.Black;
            this.label1.RectStyle.BorderWidth = 2F;
            this.label1.RectStyle.BorderWidths.Bottom = 1F;
            this.label1.Size = new System.Drawing.Size(909, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select Carton";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // chkBoxNotScan
            // 
            this.chkBoxNotScan.AutoSize = true;
            this.chkBoxNotScan.Checked = true;
            this.chkBoxNotScan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxNotScan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkBoxNotScan.Location = new System.Drawing.Point(13, 415);
            this.chkBoxNotScan.Name = "chkBoxNotScan";
            this.chkBoxNotScan.Size = new System.Drawing.Size(198, 21);
            this.chkBoxNotScan.TabIndex = 3;
            this.chkBoxNotScan.Text = "Only not yet scan complete";
            this.chkBoxNotScan.UseVisualStyleBackColor = true;
            this.chkBoxNotScan.CheckedChanged += new System.EventHandler(this.ChkBoxNotScan_CheckedChanged);
            // 
            // labelPKFilter
            // 
            this.labelPKFilter.Location = new System.Drawing.Point(232, 413);
            this.labelPKFilter.Name = "labelPKFilter";
            this.labelPKFilter.Size = new System.Drawing.Size(113, 23);
            this.labelPKFilter.TabIndex = 30;
            this.labelPKFilter.Text = "Packing No Filter";
            // 
            // comboPKFilter
            // 
            this.comboPKFilter.BackColor = System.Drawing.Color.White;
            this.comboPKFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboPKFilter.FormattingEnabled = true;
            this.comboPKFilter.IsSupportUnselect = true;
            this.comboPKFilter.Location = new System.Drawing.Point(348, 413);
            this.comboPKFilter.Name = "comboPKFilter";
            this.comboPKFilter.OldText = "";
            this.comboPKFilter.Size = new System.Drawing.Size(121, 24);
            this.comboPKFilter.TabIndex = 4;
            this.comboPKFilter.SelectedIndexChanged += new System.EventHandler(this.ComboPKFilter_SelectedIndexChanged);
            // 
            // labelQuickSelCTN
            // 
            this.labelQuickSelCTN.Location = new System.Drawing.Point(483, 413);
            this.labelQuickSelCTN.Name = "labelQuickSelCTN";
            this.labelQuickSelCTN.Size = new System.Drawing.Size(122, 23);
            this.labelQuickSelCTN.TabIndex = 32;
            this.labelQuickSelCTN.Text = "Quick Select CTN#";
            // 
            // txtQuickSelCTN
            // 
            this.txtQuickSelCTN.BackColor = System.Drawing.Color.White;
            this.txtQuickSelCTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtQuickSelCTN.Location = new System.Drawing.Point(608, 413);
            this.txtQuickSelCTN.Name = "txtQuickSelCTN";
            this.txtQuickSelCTN.Size = new System.Drawing.Size(130, 23);
            this.txtQuickSelCTN.TabIndex = 5;
            this.txtQuickSelCTN.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtQuickSelCTN_KeyUp);
            // 
            // gridSelectCartonDetail
            // 
            this.gridSelectCartonDetail.AllowUserToAddRows = false;
            this.gridSelectCartonDetail.AllowUserToDeleteRows = false;
            this.gridSelectCartonDetail.AllowUserToResizeRows = false;
            this.gridSelectCartonDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSelectCartonDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSelectCartonDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSelectCartonDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSelectCartonDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSelectCartonDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSelectCartonDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSelectCartonDetail.Location = new System.Drawing.Point(13, 443);
            this.gridSelectCartonDetail.MultiSelect = false;
            this.gridSelectCartonDetail.Name = "gridSelectCartonDetail";
            this.gridSelectCartonDetail.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridSelectCartonDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridSelectCartonDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSelectCartonDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSelectCartonDetail.RowTemplate.Height = 24;
            this.gridSelectCartonDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSelectCartonDetail.ShowCellToolTips = false;
            this.gridSelectCartonDetail.Size = new System.Drawing.Size(909, 234);
            this.gridSelectCartonDetail.TabIndex = 34;
            this.gridSelectCartonDetail.TabStop = false;
            this.gridSelectCartonDetail.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridSelectCartonDetail_CellMouseDoubleClick);
            // 
            // numBoxttlCatons
            // 
            this.numBoxttlCatons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numBoxttlCatons.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numBoxttlCatons.IsSupportEditMode = false;
            this.numBoxttlCatons.Location = new System.Drawing.Point(466, 9);
            this.numBoxttlCatons.Name = "numBoxttlCatons";
            this.numBoxttlCatons.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBoxttlCatons.ReadOnly = true;
            this.numBoxttlCatons.Size = new System.Drawing.Size(242, 23);
            this.numBoxttlCatons.TabIndex = 33;
            this.numBoxttlCatons.TabStop = false;
            this.numBoxttlCatons.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numBoxttlQty
            // 
            this.numBoxttlQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numBoxttlQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numBoxttlQty.IsSupportEditMode = false;
            this.numBoxttlQty.Location = new System.Drawing.Point(466, 38);
            this.numBoxttlQty.Name = "numBoxttlQty";
            this.numBoxttlQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBoxttlQty.ReadOnly = true;
            this.numBoxttlQty.Size = new System.Drawing.Size(242, 23);
            this.numBoxttlQty.TabIndex = 35;
            this.numBoxttlQty.TabStop = false;
            this.numBoxttlQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numBoxPackedCartons
            // 
            this.numBoxPackedCartons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numBoxPackedCartons.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numBoxPackedCartons.IsSupportEditMode = false;
            this.numBoxPackedCartons.Location = new System.Drawing.Point(466, 67);
            this.numBoxPackedCartons.Name = "numBoxPackedCartons";
            this.numBoxPackedCartons.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBoxPackedCartons.ReadOnly = true;
            this.numBoxPackedCartons.Size = new System.Drawing.Size(242, 23);
            this.numBoxPackedCartons.TabIndex = 36;
            this.numBoxPackedCartons.TabStop = false;
            this.numBoxPackedCartons.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numBoxttlPackQty
            // 
            this.numBoxttlPackQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numBoxttlPackQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numBoxttlPackQty.IsSupportEditMode = false;
            this.numBoxttlPackQty.Location = new System.Drawing.Point(466, 96);
            this.numBoxttlPackQty.Name = "numBoxttlPackQty";
            this.numBoxttlPackQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBoxttlPackQty.ReadOnly = true;
            this.numBoxttlPackQty.Size = new System.Drawing.Size(242, 23);
            this.numBoxttlPackQty.TabIndex = 37;
            this.numBoxttlPackQty.TabStop = false;
            this.numBoxttlPackQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numBoxRemainCartons
            // 
            this.numBoxRemainCartons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numBoxRemainCartons.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numBoxRemainCartons.IsSupportEditMode = false;
            this.numBoxRemainCartons.Location = new System.Drawing.Point(466, 125);
            this.numBoxRemainCartons.Name = "numBoxRemainCartons";
            this.numBoxRemainCartons.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBoxRemainCartons.ReadOnly = true;
            this.numBoxRemainCartons.Size = new System.Drawing.Size(242, 23);
            this.numBoxRemainCartons.TabIndex = 38;
            this.numBoxRemainCartons.TabStop = false;
            this.numBoxRemainCartons.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numBoxRemainQty
            // 
            this.numBoxRemainQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numBoxRemainQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numBoxRemainQty.IsSupportEditMode = false;
            this.numBoxRemainQty.Location = new System.Drawing.Point(466, 154);
            this.numBoxRemainQty.Name = "numBoxRemainQty";
            this.numBoxRemainQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBoxRemainQty.ReadOnly = true;
            this.numBoxRemainQty.Size = new System.Drawing.Size(242, 23);
            this.numBoxRemainQty.TabIndex = 39;
            this.numBoxRemainQty.TabStop = false;
            this.numBoxRemainQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // P18
            // 
            this.ClientSize = new System.Drawing.Size(934, 688);
            this.Controls.Add(this.numBoxRemainQty);
            this.Controls.Add(this.numBoxRemainCartons);
            this.Controls.Add(this.numBoxttlPackQty);
            this.Controls.Add(this.numBoxPackedCartons);
            this.Controls.Add(this.numBoxttlQty);
            this.Controls.Add(this.numBoxttlCatons);
            this.Controls.Add(this.gridSelectCartonDetail);
            this.Controls.Add(this.txtQuickSelCTN);
            this.Controls.Add(this.labelQuickSelCTN);
            this.Controls.Add(this.comboPKFilter);
            this.Controls.Add(this.labelPKFilter);
            this.Controls.Add(this.chkBoxNotScan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridScanDetail);
            this.Controls.Add(this.tabControlScanArea);
            this.Controls.Add(this.displayStyle);
            this.Controls.Add(this.displayBrand);
            this.Controls.Add(this.displayPoNo);
            this.Controls.Add(this.displaySPNo);
            this.Controls.Add(this.displayCtnNo);
            this.Controls.Add(this.displayPackID);
            this.Controls.Add(this.labelRemainQty);
            this.Controls.Add(this.labelRemainCartons);
            this.Controls.Add(this.labelttlPackQty);
            this.Controls.Add(this.labelttlQty);
            this.Controls.Add(this.labelttlCatons);
            this.Controls.Add(this.labelPackedCartons);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelPO);
            this.Controls.Add(this.labelSP);
            this.Controls.Add(this.labelCtnNo);
            this.Controls.Add(this.labelPackID);
            this.KeyPreview = true;
            this.Name = "P18";
            this.Text = "P18. Scan & Pack";
            this.Controls.SetChildIndex(this.labelPackID, 0);
            this.Controls.SetChildIndex(this.labelCtnNo, 0);
            this.Controls.SetChildIndex(this.labelSP, 0);
            this.Controls.SetChildIndex(this.labelPO, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.labelPackedCartons, 0);
            this.Controls.SetChildIndex(this.labelttlCatons, 0);
            this.Controls.SetChildIndex(this.labelttlQty, 0);
            this.Controls.SetChildIndex(this.labelttlPackQty, 0);
            this.Controls.SetChildIndex(this.labelRemainCartons, 0);
            this.Controls.SetChildIndex(this.labelRemainQty, 0);
            this.Controls.SetChildIndex(this.displayPackID, 0);
            this.Controls.SetChildIndex(this.displayCtnNo, 0);
            this.Controls.SetChildIndex(this.displaySPNo, 0);
            this.Controls.SetChildIndex(this.displayPoNo, 0);
            this.Controls.SetChildIndex(this.displayBrand, 0);
            this.Controls.SetChildIndex(this.displayStyle, 0);
            this.Controls.SetChildIndex(this.tabControlScanArea, 0);
            this.Controls.SetChildIndex(this.gridScanDetail, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.chkBoxNotScan, 0);
            this.Controls.SetChildIndex(this.labelPKFilter, 0);
            this.Controls.SetChildIndex(this.comboPKFilter, 0);
            this.Controls.SetChildIndex(this.labelQuickSelCTN, 0);
            this.Controls.SetChildIndex(this.txtQuickSelCTN, 0);
            this.Controls.SetChildIndex(this.gridSelectCartonDetail, 0);
            this.Controls.SetChildIndex(this.numBoxttlCatons, 0);
            this.Controls.SetChildIndex(this.numBoxttlQty, 0);
            this.Controls.SetChildIndex(this.numBoxPackedCartons, 0);
            this.Controls.SetChildIndex(this.numBoxttlPackQty, 0);
            this.Controls.SetChildIndex(this.numBoxRemainCartons, 0);
            this.Controls.SetChildIndex(this.numBoxRemainQty, 0);
            this.tabControlScanArea.ResumeLayout(false);
            this.tabPageCarton.ResumeLayout(false);
            this.tabPageCarton.PerformLayout();
            this.tabPageScan.ResumeLayout(false);
            this.tabPageScan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridScanDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSelectCartonDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selcartonBS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scanDetailBS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelPackID;
        private Win.UI.Label labelCtnNo;
        private Win.UI.Label labelSP;
        private Win.UI.Label labelPO;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelStyle;
        private Win.UI.Label labelPackedCartons;
        private Win.UI.Label labelttlCatons;
        private Win.UI.Label labelttlQty;
        private Win.UI.Label labelttlPackQty;
        private Win.UI.Label labelRemainCartons;
        private Win.UI.Label labelRemainQty;
        private Win.UI.DisplayBox displayPackID;
        private Win.UI.DisplayBox displayCtnNo;
        private Win.UI.DisplayBox displaySPNo;
        private Win.UI.DisplayBox displayPoNo;
        private Win.UI.DisplayBox displayBrand;
        private Win.UI.DisplayBox displayStyle;
        private Win.UI.TabControl tabControlScanArea;
        private System.Windows.Forms.TabPage tabPageCarton;
        private Win.UI.Label labelTabCarton;
        private System.Windows.Forms.TabPage tabPageScan;
        private Win.UI.TextBox txtScanCartonSP;
        private Win.UI.TextBox txtScanEAN;
        private Win.UI.Label labelEAN;
        private Win.UI.Label labelQtyScan;
        private Win.UI.Label labelTabScanTtlQty;
        private Win.UI.Label labelTabScan;
        private Win.UI.Grid gridScanDetail;
        private Win.UI.Label label1;
        private Win.UI.CheckBox chkBoxNotScan;
        private Win.UI.Label labelPKFilter;
        private Win.UI.ComboBox comboPKFilter;
        private Win.UI.Label labelQuickSelCTN;
        private Win.UI.TextBox txtQuickSelCTN;
        private Win.UI.Grid gridSelectCartonDetail;
        private Win.UI.BindingSource selcartonBS;
        private Win.UI.BindingSource scanDetailBS;
        private Win.UI.NumericBox numBoxScanTtlQty;
        private Win.UI.NumericBox numBoxttlCatons;
        private Win.UI.NumericBox numBoxttlQty;
        private Win.UI.NumericBox numBoxPackedCartons;
        private Win.UI.NumericBox numBoxttlPackQty;
        private Win.UI.NumericBox numBoxRemainCartons;
        private Win.UI.NumericBox numBoxRemainQty;
        private Win.UI.NumericBox numBoxScanQty;
    }
}
