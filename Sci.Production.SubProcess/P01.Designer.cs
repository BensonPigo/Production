﻿namespace Sci.Production.SubProcess
{
    partial class P01
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
            this.labelType = new Sci.Win.UI.Label();
            this.labelDate = new Sci.Win.UI.Label();
            this.txtTypeID = new Sci.Win.UI.TextBox();
            this.dateDate = new Sci.Win.UI.DateBox();
            this.labelShift = new Sci.Win.UI.Label();
            this.comboTeam = new Sci.Win.UI.ComboBox();
            this.labelTeam = new Sci.Win.UI.Label();
            this.numTTLhours = new Sci.Win.UI.NumericBox();
            this.labelManhours = new Sci.Win.UI.Label();
            this.numProdQty = new Sci.Win.UI.NumericBox();
            this.numQAOutput = new Sci.Win.UI.NumericBox();
            this.labelProdQty = new Sci.Win.UI.Label();
            this.labelQAOutput = new Sci.Win.UI.Label();
            this.labelDefectOutput = new Sci.Win.UI.Label();
            this.numDefectOutput = new Sci.Win.UI.NumericBox();
            this.txtdropdownlistShift = new Sci.Production.Class.txtdropdownlist();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).BeginInit();
            this.masterpanel.SuspendLayout();
            this.detailpanel.SuspendLayout();
            this.detail2.SuspendLayout();
            this.detailpanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.txtdropdownlistShift);
            this.masterpanel.Controls.Add(this.numDefectOutput);
            this.masterpanel.Controls.Add(this.numProdQty);
            this.masterpanel.Controls.Add(this.numQAOutput);
            this.masterpanel.Controls.Add(this.labelDefectOutput);
            this.masterpanel.Controls.Add(this.labelProdQty);
            this.masterpanel.Controls.Add(this.labelQAOutput);
            this.masterpanel.Controls.Add(this.numTTLhours);
            this.masterpanel.Controls.Add(this.labelManhours);
            this.masterpanel.Controls.Add(this.comboTeam);
            this.masterpanel.Controls.Add(this.labelTeam);
            this.masterpanel.Controls.Add(this.labelShift);
            this.masterpanel.Controls.Add(this.dateDate);
            this.masterpanel.Controls.Add(this.txtTypeID);
            this.masterpanel.Controls.Add(this.labelDate);
            this.masterpanel.Controls.Add(this.labelType);
            this.masterpanel.Size = new System.Drawing.Size(793, 124);
            this.masterpanel.Controls.SetChildIndex(this.labelType, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtTypeID, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShift, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTeam, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboTeam, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelManhours, 0);
            this.masterpanel.Controls.SetChildIndex(this.numTTLhours, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelQAOutput, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelProdQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDefectOutput, 0);
            this.masterpanel.Controls.SetChildIndex(this.numQAOutput, 0);
            this.masterpanel.Controls.SetChildIndex(this.numProdQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.numDefectOutput, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtdropdownlistShift, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 124);
            this.detailpanel.Size = new System.Drawing.Size(793, 247);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(675, 84);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(793, 247);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(793, 409);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(793, 371);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 371);
            this.detailbtm.Size = new System.Drawing.Size(793, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(793, 409);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(801, 438);
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(5, 5);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(40, 23);
            this.labelType.TabIndex = 3;
            this.labelType.Text = "Type";
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(5, 34);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(40, 23);
            this.labelDate.TabIndex = 4;
            this.labelDate.Text = "Date";
            // 
            // txtTypeID
            // 
            this.txtTypeID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtTypeID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "TypeID", true));
            this.txtTypeID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtTypeID.IsSupportEditMode = false;
            this.txtTypeID.Location = new System.Drawing.Point(48, 5);
            this.txtTypeID.Name = "txtTypeID";
            this.txtTypeID.ReadOnly = true;
            this.txtTypeID.Size = new System.Drawing.Size(100, 23);
            this.txtTypeID.TabIndex = 0;
            // 
            // dateDate
            // 
            this.dateDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "OutputDate", true));
            this.dateDate.Location = new System.Drawing.Point(48, 34);
            this.dateDate.Name = "dateDate";
            this.dateDate.ReadOnly = true;
            this.dateDate.Size = new System.Drawing.Size(130, 23);
            this.dateDate.TabIndex = 1;
            this.dateDate.Validating += new System.ComponentModel.CancelEventHandler(this.DateDate_Validating);
            // 
            // labelShift
            // 
            this.labelShift.Location = new System.Drawing.Point(5, 63);
            this.labelShift.Name = "labelShift";
            this.labelShift.Size = new System.Drawing.Size(40, 23);
            this.labelShift.TabIndex = 6;
            this.labelShift.Text = "Shift";
            // 
            // comboTeam
            // 
            this.comboTeam.BackColor = System.Drawing.Color.White;
            this.comboTeam.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Team", true));
            this.comboTeam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboTeam.FormattingEnabled = true;
            this.comboTeam.IsSupportUnselect = true;
            this.comboTeam.Location = new System.Drawing.Point(48, 93);
            this.comboTeam.Name = "comboTeam";
            this.comboTeam.Size = new System.Drawing.Size(57, 24);
            this.comboTeam.TabIndex = 3;
            // 
            // labelTeam
            // 
            this.labelTeam.Location = new System.Drawing.Point(5, 93);
            this.labelTeam.Name = "labelTeam";
            this.labelTeam.Size = new System.Drawing.Size(40, 23);
            this.labelTeam.TabIndex = 8;
            this.labelTeam.Text = "Team";
            // 
            // numTTLhours
            // 
            this.numTTLhours.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTTLhours.DecimalPlaces = 2;
            this.numTTLhours.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTTLhours.IsSupportEditMode = false;
            this.numTTLhours.Location = new System.Drawing.Point(409, 5);
            this.numTTLhours.Name = "numTTLhours";
            this.numTTLhours.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTTLhours.ReadOnly = true;
            this.numTTLhours.Size = new System.Drawing.Size(62, 23);
            this.numTTLhours.TabIndex = 4;
            this.numTTLhours.TabStop = false;
            this.numTTLhours.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelManhours
            // 
            this.labelManhours.Location = new System.Drawing.Point(311, 5);
            this.labelManhours.Name = "labelManhours";
            this.labelManhours.Size = new System.Drawing.Size(93, 23);
            this.labelManhours.TabIndex = 13;
            this.labelManhours.Text = "TTL W\'Hour";
            // 
            // numProdQty
            // 
            this.numProdQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numProdQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ProdQty", true));
            this.numProdQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numProdQty.IsSupportEditMode = false;
            this.numProdQty.Location = new System.Drawing.Point(409, 63);
            this.numProdQty.Name = "numProdQty";
            this.numProdQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numProdQty.ReadOnly = true;
            this.numProdQty.Size = new System.Drawing.Size(62, 23);
            this.numProdQty.TabIndex = 6;
            this.numProdQty.TabStop = false;
            this.numProdQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numQAOutput
            // 
            this.numQAOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numQAOutput.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "QAQty", true));
            this.numQAOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numQAOutput.IsSupportEditMode = false;
            this.numQAOutput.Location = new System.Drawing.Point(409, 34);
            this.numQAOutput.Name = "numQAOutput";
            this.numQAOutput.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numQAOutput.ReadOnly = true;
            this.numQAOutput.Size = new System.Drawing.Size(62, 23);
            this.numQAOutput.TabIndex = 5;
            this.numQAOutput.TabStop = false;
            this.numQAOutput.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelProdQty
            // 
            this.labelProdQty.Location = new System.Drawing.Point(311, 63);
            this.labelProdQty.Name = "labelProdQty";
            this.labelProdQty.Size = new System.Drawing.Size(93, 23);
            this.labelProdQty.TabIndex = 24;
            this.labelProdQty.Text = "Prod. Output";
            // 
            // labelQAOutput
            // 
            this.labelQAOutput.Location = new System.Drawing.Point(311, 34);
            this.labelQAOutput.Name = "labelQAOutput";
            this.labelQAOutput.Size = new System.Drawing.Size(93, 23);
            this.labelQAOutput.TabIndex = 23;
            this.labelQAOutput.Text = "QA Output";
            // 
            // labelDefectOutput
            // 
            this.labelDefectOutput.Location = new System.Drawing.Point(311, 92);
            this.labelDefectOutput.Name = "labelDefectOutput";
            this.labelDefectOutput.Size = new System.Drawing.Size(93, 23);
            this.labelDefectOutput.TabIndex = 25;
            this.labelDefectOutput.Text = "Defect Output";
            // 
            // numDefectOutput
            // 
            this.numDefectOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numDefectOutput.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "DefectQty", true));
            this.numDefectOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numDefectOutput.IsSupportEditMode = false;
            this.numDefectOutput.Location = new System.Drawing.Point(409, 92);
            this.numDefectOutput.Name = "numDefectOutput";
            this.numDefectOutput.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDefectOutput.ReadOnly = true;
            this.numDefectOutput.Size = new System.Drawing.Size(62, 23);
            this.numDefectOutput.TabIndex = 7;
            this.numDefectOutput.TabStop = false;
            this.numDefectOutput.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtdropdownlistShift
            // 
            this.txtdropdownlistShift.BackColor = System.Drawing.Color.White;
            this.txtdropdownlistShift.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Shift", true));
            this.txtdropdownlistShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlistShift.FormattingEnabled = true;
            this.txtdropdownlistShift.IsSupportUnselect = true;
            this.txtdropdownlistShift.Location = new System.Drawing.Point(48, 63);
            this.txtdropdownlistShift.Name = "txtdropdownlistShift";
            this.txtdropdownlistShift.Size = new System.Drawing.Size(121, 24);
            this.txtdropdownlistShift.TabIndex = 2;
            this.txtdropdownlistShift.Type = "PPAOutput_Shift";
            // 
            // P01
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(801, 471);
            this.DefaultDetailOrder = "orderid";
            this.DefaultOrder = "OutputDate,Shift,Team";
            this.GridAlias = "SubProcessOutput_Detail";
            this.GridUniqueKey = "ID,orderID";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P01";
            this.SubDetailKeyField1 = "ID,ukey";
            this.SubDetailKeyField2 = "id,SubProcessOutput_DetailUKey";
            this.SubGridAlias = "SubProcessOutput_Detail_Detail";
            this.SubKeyField1 = "ukey";
            this.Text = "P01. Subprocess Daily Output";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "ID";
            this.WorkAlias = "SubProcessOutput";
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).EndInit();
            this.masterpanel.ResumeLayout(false);
            this.masterpanel.PerformLayout();
            this.detailpanel.ResumeLayout(false);
            this.detail2.ResumeLayout(false);
            this.detailpanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelType;
        private Win.UI.Label labelDate;
        private Win.UI.TextBox txtTypeID;
        private Win.UI.DateBox dateDate;
        private Win.UI.Label labelShift;
        private Win.UI.ComboBox comboTeam;
        private Win.UI.Label labelTeam;
        private Win.UI.NumericBox numTTLhours;
        private Win.UI.Label labelManhours;
        private Win.UI.NumericBox numDefectOutput;
        private Win.UI.NumericBox numProdQty;
        private Win.UI.NumericBox numQAOutput;
        private Win.UI.Label labelDefectOutput;
        private Win.UI.Label labelProdQty;
        private Win.UI.Label labelQAOutput;
        private Class.txtdropdownlist txtdropdownlistShift;
    }
}
