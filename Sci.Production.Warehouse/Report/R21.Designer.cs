﻿namespace Sci.Production.Warehouse
{
    partial class R21
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
            this.rdbtnSummary = new Sci.Win.UI.RadioButton();
            this.rdbtnDetail = new Sci.Win.UI.RadioButton();
            this.cmbStockType = new Sci.Win.UI.ComboBox();
            this.cmbMaterialType = new Sci.Win.UI.ComboBox();
            this.labelReportType = new Sci.Win.UI.Label();
            this.labelStockType = new Sci.Win.UI.Label();
            this.labelMaterialType = new Sci.Win.UI.Label();
            this.textColor = new Sci.Win.UI.TextBox();
            this.labelColor = new Sci.Win.UI.Label();
            this.checkQty = new Sci.Win.UI.CheckBox();
            this.txtfactory1 = new Sci.Production.Class.txtfactory();
            this.label2 = new Sci.Win.UI.Label();
            this.textEndRefno = new Sci.Win.UI.TextBox();
            this.textStartRefno = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtMdivision1 = new Sci.Production.Class.txtMdivision();
            this.textEndSP = new Sci.Win.UI.TextBox();
            this.textStartSP = new Sci.Win.UI.TextBox();
            this.labelRefno = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(438, 81);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(438, 9);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(438, 45);
            // 
            // rdbtnSummary
            // 
            this.rdbtnSummary.AutoSize = true;
            this.rdbtnSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtnSummary.Location = new System.Drawing.Point(178, 215);
            this.rdbtnSummary.Name = "rdbtnSummary";
            this.rdbtnSummary.Size = new System.Drawing.Size(85, 21);
            this.rdbtnSummary.TabIndex = 134;
            this.rdbtnSummary.Text = "Summary";
            this.rdbtnSummary.UseVisualStyleBackColor = true;
            // 
            // rdbtnDetail
            // 
            this.rdbtnDetail.AutoSize = true;
            this.rdbtnDetail.Checked = true;
            this.rdbtnDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtnDetail.Location = new System.Drawing.Point(110, 215);
            this.rdbtnDetail.Name = "rdbtnDetail";
            this.rdbtnDetail.Size = new System.Drawing.Size(62, 21);
            this.rdbtnDetail.TabIndex = 133;
            this.rdbtnDetail.TabStop = true;
            this.rdbtnDetail.Text = "Detail";
            this.rdbtnDetail.UseVisualStyleBackColor = true;
            // 
            // cmbStockType
            // 
            this.cmbStockType.BackColor = System.Drawing.Color.White;
            this.cmbStockType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbStockType.FormattingEnabled = true;
            this.cmbStockType.IsSupportUnselect = true;
            this.cmbStockType.Location = new System.Drawing.Point(109, 185);
            this.cmbStockType.Name = "cmbStockType";
            this.cmbStockType.Size = new System.Drawing.Size(117, 24);
            this.cmbStockType.TabIndex = 132;
            // 
            // cmbMaterialType
            // 
            this.cmbMaterialType.BackColor = System.Drawing.Color.White;
            this.cmbMaterialType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbMaterialType.FormattingEnabled = true;
            this.cmbMaterialType.IsSupportUnselect = true;
            this.cmbMaterialType.Location = new System.Drawing.Point(109, 155);
            this.cmbMaterialType.Name = "cmbMaterialType";
            this.cmbMaterialType.Size = new System.Drawing.Size(117, 24);
            this.cmbMaterialType.TabIndex = 131;
            // 
            // labelReportType
            // 
            this.labelReportType.Location = new System.Drawing.Point(9, 215);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(97, 23);
            this.labelReportType.TabIndex = 130;
            this.labelReportType.Text = "Report Type";
            // 
            // labelStockType
            // 
            this.labelStockType.Location = new System.Drawing.Point(9, 185);
            this.labelStockType.Name = "labelStockType";
            this.labelStockType.Size = new System.Drawing.Size(97, 23);
            this.labelStockType.TabIndex = 129;
            this.labelStockType.Text = "Stock Type";
            // 
            // labelMaterialType
            // 
            this.labelMaterialType.Location = new System.Drawing.Point(9, 155);
            this.labelMaterialType.Name = "labelMaterialType";
            this.labelMaterialType.Size = new System.Drawing.Size(97, 23);
            this.labelMaterialType.TabIndex = 128;
            this.labelMaterialType.Text = "Material Type";
            // 
            // textColor
            // 
            this.textColor.BackColor = System.Drawing.Color.White;
            this.textColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textColor.Location = new System.Drawing.Point(110, 126);
            this.textColor.MaxLength = 6;
            this.textColor.Name = "textColor";
            this.textColor.Size = new System.Drawing.Size(66, 23);
            this.textColor.TabIndex = 127;
            // 
            // labelColor
            // 
            this.labelColor.Location = new System.Drawing.Point(9, 126);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(97, 23);
            this.labelColor.TabIndex = 126;
            this.labelColor.Text = "Color";
            // 
            // checkQty
            // 
            this.checkQty.AutoSize = true;
            this.checkQty.Checked = true;
            this.checkQty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkQty.Location = new System.Drawing.Point(9, 244);
            this.checkQty.Name = "checkQty";
            this.checkQty.Size = new System.Drawing.Size(73, 21);
            this.checkQty.TabIndex = 119;
            this.checkQty.Text = "Qty > 0";
            this.checkQty.UseVisualStyleBackColor = true;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(110, 67);
            this.txtfactory1.MaxLength = 8;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 116;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(259, 96);
            this.label2.Name = "label2";
            this.label2.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.RectStyle.Color = System.Drawing.Color.Transparent;
            this.label2.RectStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label2.Size = new System.Drawing.Size(22, 23);
            this.label2.TabIndex = 125;
            this.label2.Text = "～";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // textEndRefno
            // 
            this.textEndRefno.BackColor = System.Drawing.Color.White;
            this.textEndRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textEndRefno.Location = new System.Drawing.Point(284, 96);
            this.textEndRefno.MaxLength = 20;
            this.textEndRefno.Name = "textEndRefno";
            this.textEndRefno.Size = new System.Drawing.Size(146, 23);
            this.textEndRefno.TabIndex = 118;
            // 
            // textStartRefno
            // 
            this.textStartRefno.BackColor = System.Drawing.Color.White;
            this.textStartRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textStartRefno.Location = new System.Drawing.Point(109, 96);
            this.textStartRefno.MaxLength = 20;
            this.textStartRefno.Name = "textStartRefno";
            this.textStartRefno.Size = new System.Drawing.Size(146, 23);
            this.textStartRefno.TabIndex = 117;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(234, 9);
            this.label1.Name = "label1";
            this.label1.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.RectStyle.Color = System.Drawing.Color.Transparent;
            this.label1.RectStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label1.Size = new System.Drawing.Size(22, 23);
            this.label1.TabIndex = 124;
            this.label1.Text = "～";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(109, 38);
            this.txtMdivision1.MaxLength = 8;
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(67, 23);
            this.txtMdivision1.TabIndex = 115;
            // 
            // textEndSP
            // 
            this.textEndSP.BackColor = System.Drawing.Color.White;
            this.textEndSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textEndSP.Location = new System.Drawing.Point(259, 9);
            this.textEndSP.MaxLength = 13;
            this.textEndSP.Name = "textEndSP";
            this.textEndSP.Size = new System.Drawing.Size(121, 23);
            this.textEndSP.TabIndex = 114;
            // 
            // textStartSP
            // 
            this.textStartSP.BackColor = System.Drawing.Color.White;
            this.textStartSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textStartSP.Location = new System.Drawing.Point(110, 9);
            this.textStartSP.MaxLength = 13;
            this.textStartSP.Name = "textStartSP";
            this.textStartSP.Size = new System.Drawing.Size(121, 23);
            this.textStartSP.TabIndex = 113;
            // 
            // labelRefno
            // 
            this.labelRefno.Location = new System.Drawing.Point(9, 96);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(97, 23);
            this.labelRefno.TabIndex = 121;
            this.labelRefno.Text = "Refno";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(9, 67);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(97, 23);
            this.labelFactory.TabIndex = 120;
            this.labelFactory.Text = "Factory";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(9, 38);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(97, 23);
            this.labelM.TabIndex = 122;
            this.labelM.Text = "M";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(9, 9);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(97, 23);
            this.labelSPNo.TabIndex = 123;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // R21
            // 
            this.ClientSize = new System.Drawing.Size(530, 314);
            this.Controls.Add(this.rdbtnSummary);
            this.Controls.Add(this.rdbtnDetail);
            this.Controls.Add(this.cmbStockType);
            this.Controls.Add(this.cmbMaterialType);
            this.Controls.Add(this.labelReportType);
            this.Controls.Add(this.labelStockType);
            this.Controls.Add(this.labelMaterialType);
            this.Controls.Add(this.textColor);
            this.Controls.Add(this.labelColor);
            this.Controls.Add(this.checkQty);
            this.Controls.Add(this.txtfactory1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textEndRefno);
            this.Controls.Add(this.textStartRefno);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMdivision1);
            this.Controls.Add(this.textEndSP);
            this.Controls.Add(this.textStartSP);
            this.Controls.Add(this.labelRefno);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelSPNo);
            this.Name = "R21";
            this.Text = "R21 Stock List Report (Inventory)";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelRefno, 0);
            this.Controls.SetChildIndex(this.textStartSP, 0);
            this.Controls.SetChildIndex(this.textEndSP, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.textStartRefno, 0);
            this.Controls.SetChildIndex(this.textEndRefno, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtfactory1, 0);
            this.Controls.SetChildIndex(this.checkQty, 0);
            this.Controls.SetChildIndex(this.labelColor, 0);
            this.Controls.SetChildIndex(this.textColor, 0);
            this.Controls.SetChildIndex(this.labelMaterialType, 0);
            this.Controls.SetChildIndex(this.labelStockType, 0);
            this.Controls.SetChildIndex(this.labelReportType, 0);
            this.Controls.SetChildIndex(this.cmbMaterialType, 0);
            this.Controls.SetChildIndex(this.cmbStockType, 0);
            this.Controls.SetChildIndex(this.rdbtnDetail, 0);
            this.Controls.SetChildIndex(this.rdbtnSummary, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioButton rdbtnSummary;
        private Win.UI.RadioButton rdbtnDetail;
        private Win.UI.ComboBox cmbStockType;
        private Win.UI.ComboBox cmbMaterialType;
        private Win.UI.Label labelReportType;
        private Win.UI.Label labelStockType;
        private Win.UI.Label labelMaterialType;
        private Win.UI.TextBox textColor;
        private Win.UI.Label labelColor;
        private Win.UI.CheckBox checkQty;
        private Class.txtfactory txtfactory1;
        private Win.UI.Label label2;
        private Win.UI.TextBox textEndRefno;
        private Win.UI.TextBox textStartRefno;
        private Win.UI.Label label1;
        private Class.txtMdivision txtMdivision1;
        private Win.UI.TextBox textEndSP;
        private Win.UI.TextBox textStartSP;
        private Win.UI.Label labelRefno;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelM;
        private Win.UI.Label labelSPNo;
    }
}
