﻿namespace Sci.Production.Quality
{
    partial class R05
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
            this.radioPanel = new Sci.Win.UI.RadioPanel();
            this.radioDetail = new Sci.Win.UI.RadioButton();
            this.radioSummary = new Sci.Win.UI.RadioButton();
            this.labelReportType = new Sci.Win.UI.Label();
            this.comboMaterialType = new Sci.Win.UI.ComboBox();
            this.labelMaterialType = new Sci.Win.UI.Label();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.labelCategory = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.radioPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toexcel
            // 
            this.toexcel.Click += new System.EventHandler(this.toexcel_Click);
            // 
            // radioPanel
            // 
            this.radioPanel.Controls.Add(this.radioDetail);
            this.radioPanel.Controls.Add(this.radioSummary);
            this.radioPanel.Controls.Add(this.labelReportType);
            this.radioPanel.Controls.Add(this.comboMaterialType);
            this.radioPanel.Controls.Add(this.labelMaterialType);
            this.radioPanel.Controls.Add(this.comboCategory);
            this.radioPanel.Controls.Add(this.labelCategory);
            this.radioPanel.Controls.Add(this.dateSCIDelivery);
            this.radioPanel.Controls.Add(this.labelSCIDelivery);
            this.radioPanel.Location = new System.Drawing.Point(30, 24);
            this.radioPanel.Name = "radioPanel";
            this.radioPanel.Size = new System.Drawing.Size(440, 217);
            this.radioPanel.TabIndex = 94;
            // 
            // radioDetail
            // 
            this.radioDetail.AutoSize = true;
            this.radioDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail.Location = new System.Drawing.Point(128, 173);
            this.radioDetail.Name = "radioDetail";
            this.radioDetail.Size = new System.Drawing.Size(62, 21);
            this.radioDetail.TabIndex = 106;
            this.radioDetail.Text = "Detail";
            this.radioDetail.UseVisualStyleBackColor = true;
            // 
            // radioSummary
            // 
            this.radioSummary.AutoSize = true;
            this.radioSummary.Checked = true;
            this.radioSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSummary.Location = new System.Drawing.Point(128, 146);
            this.radioSummary.Name = "radioSummary";
            this.radioSummary.Size = new System.Drawing.Size(85, 21);
            this.radioSummary.TabIndex = 105;
            this.radioSummary.TabStop = true;
            this.radioSummary.Text = "Summary";
            this.radioSummary.UseVisualStyleBackColor = true;
            // 
            // labelReportType
            // 
            this.labelReportType.Lines = 0;
            this.labelReportType.Location = new System.Drawing.Point(29, 146);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(93, 23);
            this.labelReportType.TabIndex = 104;
            this.labelReportType.Text = "Report Type";
            // 
            // comboMaterialType
            // 
            this.comboMaterialType.BackColor = System.Drawing.Color.White;
            this.comboMaterialType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMaterialType.FormattingEnabled = true;
            this.comboMaterialType.IsSupportUnselect = true;
            this.comboMaterialType.Location = new System.Drawing.Point(128, 104);
            this.comboMaterialType.Name = "comboMaterialType";
            this.comboMaterialType.Size = new System.Drawing.Size(125, 24);
            this.comboMaterialType.TabIndex = 103;
            // 
            // labelMaterialType
            // 
            this.labelMaterialType.Lines = 0;
            this.labelMaterialType.Location = new System.Drawing.Point(29, 104);
            this.labelMaterialType.Name = "labelMaterialType";
            this.labelMaterialType.Size = new System.Drawing.Size(93, 23);
            this.labelMaterialType.TabIndex = 102;
            this.labelMaterialType.Text = "Material Type";
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(128, 64);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.Size = new System.Drawing.Size(125, 24);
            this.comboCategory.TabIndex = 101;
            // 
            // labelCategory
            // 
            this.labelCategory.Lines = 0;
            this.labelCategory.Location = new System.Drawing.Point(29, 64);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(93, 23);
            this.labelCategory.TabIndex = 100;
            this.labelCategory.Text = "Category";
            // 
            // dateSCIDelivery
            // 
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(128, 24);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 99;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Lines = 0;
            this.labelSCIDelivery.Location = new System.Drawing.Point(29, 24);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(93, 23);
            this.labelSCIDelivery.TabIndex = 98;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // R05
            // 
            this.ClientSize = new System.Drawing.Size(627, 290);
            this.Controls.Add(this.radioPanel);
            this.Name = "R05";
            this.Text = "R05.Monthly Material Suppliers Evaluation Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioPanel, 0);
            this.radioPanel.ResumeLayout(false);
            this.radioPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel;
        private Win.UI.RadioButton radioDetail;
        private Win.UI.RadioButton radioSummary;
        private Win.UI.Label labelReportType;
        private Win.UI.ComboBox comboMaterialType;
        private Win.UI.Label labelMaterialType;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.Label labelCategory;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.Label labelSCIDelivery;

    }
}
