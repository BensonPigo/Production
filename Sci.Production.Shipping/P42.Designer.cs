﻿namespace Sci.Production.Shipping
{
    partial class P42
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
            this.labelDate = new Sci.Win.UI.Label();
            this.labelContractNo = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.dateDate = new Sci.Win.UI.DateBox();
            this.txtContractNo = new Sci.Win.UI.TextBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.labelCustomdeclareNo = new Sci.Win.UI.Label();
            this.txtCustomdeclareNo = new Sci.Win.UI.TextBox();
            this.btnImportfromExcel = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.btnImportfromExcel);
            this.masterpanel.Controls.Add(this.txtCustomdeclareNo);
            this.masterpanel.Controls.Add(this.labelCustomdeclareNo);
            this.masterpanel.Controls.Add(this.txtRemark);
            this.masterpanel.Controls.Add(this.txtContractNo);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.labelContractNo);
            this.masterpanel.Controls.Add(this.labelDate);
            this.masterpanel.Controls.Add(this.dateDate);
            this.masterpanel.Size = new System.Drawing.Size(913, 103);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelContractNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtContractNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCustomdeclareNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtCustomdeclareNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportfromExcel, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 103);
            this.detailpanel.Size = new System.Drawing.Size(913, 246);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(663, 68);
            this.gridicon.TabIndex = 7;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(826, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(913, 246);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(892, 387);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(886, 341);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(886, 40);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(913, 387);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(913, 349);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 349);
            this.detailbtm.Size = new System.Drawing.Size(913, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(913, 387);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(921, 416);
            // 
            // createby
            // 
            this.createby.Location = new System.Drawing.Point(69, 7);
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(470, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lblcreateby
            // 
            this.lblcreateby.Location = new System.Drawing.Point(4, 13);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(422, 13);
            // 
            // labelDate
            // 
            this.labelDate.Lines = 0;
            this.labelDate.Location = new System.Drawing.Point(9, 13);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(81, 23);
            this.labelDate.TabIndex = 4;
            this.labelDate.Text = "Date";
            // 
            // labelContractNo
            // 
            this.labelContractNo.Lines = 0;
            this.labelContractNo.Location = new System.Drawing.Point(9, 43);
            this.labelContractNo.Name = "labelContractNo";
            this.labelContractNo.Size = new System.Drawing.Size(81, 23);
            this.labelContractNo.TabIndex = 5;
            this.labelContractNo.Text = "Contract no.";
            // 
            // labelRemark
            // 
            this.labelRemark.Lines = 0;
            this.labelRemark.Location = new System.Drawing.Point(9, 73);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(81, 23);
            this.labelRemark.TabIndex = 6;
            this.labelRemark.Text = "Remark";
            // 
            // dateDate
            // 
            this.dateDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CDate", true));
            this.dateDate.Location = new System.Drawing.Point(94, 13);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(130, 23);
            this.dateDate.TabIndex = 0;
            // 
            // txtContractNo
            // 
            this.txtContractNo.BackColor = System.Drawing.Color.White;
            this.txtContractNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "VNContractID", true));
            this.txtContractNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtContractNo.Location = new System.Drawing.Point(94, 43);
            this.txtContractNo.Name = "txtContractNo";
            this.txtContractNo.Size = new System.Drawing.Size(150, 23);
            this.txtContractNo.TabIndex = 2;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(94, 73);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(536, 23);
            this.txtRemark.TabIndex = 3;
            // 
            // labelCustomdeclareNo
            // 
            this.labelCustomdeclareNo.Lines = 0;
            this.labelCustomdeclareNo.Location = new System.Drawing.Point(313, 13);
            this.labelCustomdeclareNo.Name = "labelCustomdeclareNo";
            this.labelCustomdeclareNo.Size = new System.Drawing.Size(124, 23);
            this.labelCustomdeclareNo.TabIndex = 9;
            this.labelCustomdeclareNo.Text = "Custom declare no.";
            // 
            // txtCustomdeclareNo
            // 
            this.txtCustomdeclareNo.BackColor = System.Drawing.Color.White;
            this.txtCustomdeclareNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DeclareNo", true));
            this.txtCustomdeclareNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCustomdeclareNo.Location = new System.Drawing.Point(441, 13);
            this.txtCustomdeclareNo.Name = "txtCustomdeclareNo";
            this.txtCustomdeclareNo.Size = new System.Drawing.Size(168, 23);
            this.txtCustomdeclareNo.TabIndex = 1;
            // 
            // btnImportfromExcel
            // 
            this.btnImportfromExcel.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImportfromExcel.Location = new System.Drawing.Point(625, 13);
            this.btnImportfromExcel.Name = "btnImportfromExcel";
            this.btnImportfromExcel.Size = new System.Drawing.Size(142, 30);
            this.btnImportfromExcel.TabIndex = 8;
            this.btnImportfromExcel.Text = "Import from excel";
            this.btnImportfromExcel.UseVisualStyleBackColor = true;
            this.btnImportfromExcel.Click += new System.EventHandler(this.btnImportfromExcel_Click);
            // 
            // P42
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(921, 449);
            this.DefaultControl = "dateBox1";
            this.DefaultControlForEdit = "dateBox1";
            this.DefaultOrder = "ID";
            this.GridAlias = "VNContractQtyAdjust_Detail";
            this.GridUniqueKey = "NLCode";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P42";
            this.Text = "P42. Customs Contract Qty Adjust";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "ID";
            this.WorkAlias = "VNContractQtyAdjust";
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

        private Win.UI.TextBox txtCustomdeclareNo;
        private Win.UI.Label labelCustomdeclareNo;
        private Win.UI.TextBox txtRemark;
        private Win.UI.TextBox txtContractNo;
        private Win.UI.DateBox dateDate;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelContractNo;
        private Win.UI.Label labelDate;
        private Win.UI.Button btnImportfromExcel;
    }
}
