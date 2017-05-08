﻿namespace Sci.Production.Quality
{
    partial class B21
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
            this.labelDefectcode = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelDefectType = new Sci.Win.UI.Label();
            this.txtDefectcode = new Sci.Win.UI.TextBox();
            this.txtDefectType = new Sci.Win.UI.TextBox();
            this.editDescription = new Sci.Win.UI.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(831, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.editDescription);
            this.detailcont.Controls.Add(this.txtDefectType);
            this.detailcont.Controls.Add(this.txtDefectcode);
            this.detailcont.Controls.Add(this.labelDefectcode);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelDefectType);
            this.detailcont.Size = new System.Drawing.Size(831, 357);
            this.detailcont.MouseDown += new System.Windows.Forms.MouseEventHandler(this.detailcont_MouseDown);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(831, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(831, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(839, 424);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelDefectcode
            // 
            this.labelDefectcode.Lines = 0;
            this.labelDefectcode.Location = new System.Drawing.Point(58, 66);
            this.labelDefectcode.Name = "labelDefectcode";
            this.labelDefectcode.Size = new System.Drawing.Size(89, 23);
            this.labelDefectcode.TabIndex = 2;
            this.labelDefectcode.Text = "Defect code";
            // 
            // labelDescription
            // 
            this.labelDescription.Lines = 0;
            this.labelDescription.Location = new System.Drawing.Point(58, 132);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(89, 23);
            this.labelDescription.TabIndex = 3;
            this.labelDescription.Text = "Description";
            // 
            // labelDefectType
            // 
            this.labelDefectType.Lines = 0;
            this.labelDefectType.Location = new System.Drawing.Point(58, 99);
            this.labelDefectType.Name = "labelDefectType";
            this.labelDefectType.Size = new System.Drawing.Size(89, 23);
            this.labelDefectType.TabIndex = 4;
            this.labelDefectType.Text = "Defect Type";
            // 
            // txtDefectcode
            // 
            this.txtDefectcode.BackColor = System.Drawing.Color.White;
            this.txtDefectcode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtDefectcode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDefectcode.Location = new System.Drawing.Point(150, 66);
            this.txtDefectcode.Name = "txtDefectcode";
            this.txtDefectcode.Size = new System.Drawing.Size(135, 23);
            this.txtDefectcode.TabIndex = 0;
            this.txtDefectcode.Validating += new System.ComponentModel.CancelEventHandler(this.txtDefectcode_Validating);
            this.txtDefectcode.Validated += new System.EventHandler(this.txtDefectcode_Validated);
            // 
            // txtDefectType
            // 
            this.txtDefectType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtDefectType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "GarmentDefectTypeID", true));
            this.txtDefectType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtDefectType.IsSupportEditMode = false;
            this.txtDefectType.Location = new System.Drawing.Point(150, 99);
            this.txtDefectType.Name = "txtDefectType";
            this.txtDefectType.ReadOnly = true;
            this.txtDefectType.Size = new System.Drawing.Size(135, 23);
            this.txtDefectType.TabIndex = 1;
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.CausesValidation = false;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(150, 132);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(460, 94);
            this.editDescription.TabIndex = 2;
            this.editDescription.Leave += new System.EventHandler(this.editDescription_Leave);
            // 
            // B21
            // 
            this.ClientSize = new System.Drawing.Size(839, 457);
            this.DefaultControl = "txtDefectcode";
            this.DefaultControlForEdit = "editDescription";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportLocate = false;
            this.IsSupportPrint = false;
            this.KeyPreview = true;
            this.Name = "B21";
            this.Text = "B21. Defect Detail for RFT/CFA(Garment)       ";
            this.WorkAlias = "GarmentDefectCode";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelDefectcode;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelDefectType;
        private Win.UI.TextBox txtDefectType;
        private Win.UI.TextBox txtDefectcode;
        private Win.UI.EditBox editDescription;
    }
}
