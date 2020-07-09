namespace Sci.Production.Warehouse
{
    partial class P60
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
            this.labelID = new Sci.Win.UI.Label();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.label25 = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
            this.btnFind = new Sci.Win.UI.Button();
            this.txtLocateForSP = new Sci.Win.UI.TextBox();
            this.labelLocateForSP = new Sci.Win.UI.Label();
            this.btnDelete = new Sci.Win.UI.Button();
            this.labelRemark = new Sci.Win.UI.Label();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.btnImport = new Sci.Win.UI.Button();
            this.txtsubconLocalSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.labelLocalSupplier = new Sci.Win.UI.Label();
            this.labelInvoice = new Sci.Win.UI.Label();
            this.txtInvoice = new Sci.Win.UI.TextBox();
            this.txtTotal = new Sci.Win.UI.DisplayBox();
            this.lblTotal = new Sci.Win.UI.Label();
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
            this.masterpanel.Controls.Add(this.txtTotal);
            this.masterpanel.Controls.Add(this.lblTotal);
            this.masterpanel.Controls.Add(this.txtInvoice);
            this.masterpanel.Controls.Add(this.labelInvoice);
            this.masterpanel.Controls.Add(this.labelLocalSupplier);
            this.masterpanel.Controls.Add(this.txtsubconLocalSupplier);
            this.masterpanel.Controls.Add(this.btnImport);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.btnDelete);
            this.masterpanel.Controls.Add(this.btnFind);
            this.masterpanel.Controls.Add(this.txtLocateForSP);
            this.masterpanel.Controls.Add(this.labelLocateForSP);
            this.masterpanel.Controls.Add(this.dateIssueDate);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.label25);
            this.masterpanel.Controls.Add(this.labelIssueDate);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Size = new System.Drawing.Size(998, 189);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.label25, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelLocateForSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLocateForSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnFind, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnDelete, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImport, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtsubconLocalSupplier, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelLocalSupplier, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelInvoice, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtInvoice, 0);
            this.masterpanel.Controls.SetChildIndex(this.lblTotal, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtTotal, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 189);
            this.detailpanel.Size = new System.Drawing.Size(998, 288);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.gridicon.Location = new System.Drawing.Point(889, 146);
            this.gridicon.TabIndex = 8;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(909, 1);
            this.refresh.TabIndex = 0;
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(998, 288);
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
            this.detail.Size = new System.Drawing.Size(998, 515);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(998, 477);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 477);
            this.detailbtm.Size = new System.Drawing.Size(998, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(998, 515);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1006, 544);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(467, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(419, 13);
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(16, 13);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 1;
            this.labelID.Text = "ID";
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(241, 13);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.Size = new System.Drawing.Size(108, 23);
            this.labelIssueDate.TabIndex = 11;
            this.labelIssueDate.Text = "Issue Date";
            // 
            // label25
            // 
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label25.Location = new System.Drawing.Point(864, 13);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(115, 23);
            this.label25.TabIndex = 43;
            this.label25.Text = "Not Approve";
            this.label25.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(94, 13);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(120, 23);
            this.displayID.TabIndex = 0;
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IssueDate", true));
            this.dateIssueDate.Location = new System.Drawing.Point(352, 13);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(130, 23);
            this.dateIssueDate.TabIndex = 0;
            // 
            // btnFind
            // 
            this.btnFind.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnFind.Location = new System.Drawing.Point(279, 146);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(70, 30);
            this.btnFind.TabIndex = 5;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // txtLocateForSP
            // 
            this.txtLocateForSP.BackColor = System.Drawing.Color.White;
            this.txtLocateForSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateForSP.IsSupportEditMode = false;
            this.txtLocateForSP.Location = new System.Drawing.Point(128, 150);
            this.txtLocateForSP.Name = "txtLocateForSP";
            this.txtLocateForSP.Size = new System.Drawing.Size(145, 23);
            this.txtLocateForSP.TabIndex = 4;
            // 
            // labelLocateForSP
            // 
            this.labelLocateForSP.Location = new System.Drawing.Point(16, 150);
            this.labelLocateForSP.Name = "labelLocateForSP";
            this.labelLocateForSP.Size = new System.Drawing.Size(109, 23);
            this.labelLocateForSP.TabIndex = 58;
            this.labelLocateForSP.Text = "Locate for SP#";
            // 
            // btnDelete
            // 
            this.btnDelete.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnDelete.Location = new System.Drawing.Point(355, 146);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 30);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete all";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(16, 82);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 59;
            this.labelRemark.Text = "Remark";
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(94, 82);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(658, 51);
            this.editRemark.TabIndex = 3;
            // 
            // btnImport
            // 
            this.btnImport.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImport.Location = new System.Drawing.Point(759, 148);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(123, 30);
            this.btnImport.TabIndex = 7;
            this.btnImport.Text = "Batch Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // txtsubconLocalSupplier
            // 
            this.txtsubconLocalSupplier.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "localsuppid", true));
            this.txtsubconLocalSupplier.DisplayBox1Binding = "";
            this.txtsubconLocalSupplier.IsIncludeJunk = false;
            this.txtsubconLocalSupplier.Location = new System.Drawing.Point(635, 13);
            this.txtsubconLocalSupplier.Name = "txtsubconLocalSupplier";
            this.txtsubconLocalSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtsubconLocalSupplier.TabIndex = 1;
            this.txtsubconLocalSupplier.TextBox1Binding = "";
            // 
            // labelLocalSupplier
            // 
            this.labelLocalSupplier.Location = new System.Drawing.Point(524, 13);
            this.labelLocalSupplier.Name = "labelLocalSupplier";
            this.labelLocalSupplier.Size = new System.Drawing.Size(108, 23);
            this.labelLocalSupplier.TabIndex = 63;
            this.labelLocalSupplier.Text = "Local Supplier";
            // 
            // labelInvoice
            // 
            this.labelInvoice.Location = new System.Drawing.Point(16, 48);
            this.labelInvoice.Name = "labelInvoice";
            this.labelInvoice.Size = new System.Drawing.Size(75, 23);
            this.labelInvoice.TabIndex = 64;
            this.labelInvoice.Text = "Invoice#";
            // 
            // txtInvoice
            // 
            this.txtInvoice.BackColor = System.Drawing.Color.White;
            this.txtInvoice.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "invNO", true));
            this.txtInvoice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvoice.Location = new System.Drawing.Point(94, 48);
            this.txtInvoice.Name = "txtInvoice";
            this.txtInvoice.Size = new System.Drawing.Size(145, 23);
            this.txtInvoice.TabIndex = 2;
            // 
            // txtTotal
            // 
            this.txtTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtTotal.Location = new System.Drawing.Point(635, 48);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(145, 23);
            this.txtTotal.TabIndex = 65;
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(524, 48);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(108, 23);
            this.lblTotal.TabIndex = 66;
            this.lblTotal.Text = "Total Qty";
            // 
            // P60
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(1006, 577);
            this.DefaultControl = "txtsubconLocalSupplier";
            this.DefaultControlForEdit = "txtsubconLocalSupplier";
            this.DefaultDetailOrder = "localpoid,localpo_detailUkey";
            this.DefaultOrder = "IssueDate,ID";
            this.Grid2New = 0;
            this.GridAlias = "LocalReceiving_detail";
            this.GridNew = 0;
            this.GridUniqueKey = "id,localpo_detailUkey";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P60";
            this.Text = "P60. Local PO Receiving - incoming";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "id";
            this.WorkAlias = "LocalReceiving";
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

        private Win.UI.Label labelIssueDate;
        private Win.UI.Label labelID;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label label25;
        private Win.UI.DateBox dateIssueDate;
        private Win.UI.Button btnFind;
        private Win.UI.TextBox txtLocateForSP;
        private Win.UI.Label labelLocateForSP;
        private Win.UI.Button btnDelete;
        private Win.UI.EditBox editRemark;
        private Win.UI.Label labelRemark;
        private Win.UI.Button btnImport;
        private Win.UI.TextBox txtInvoice;
        private Win.UI.Label labelInvoice;
        private Win.UI.Label labelLocalSupplier;
        private Class.TxtsubconNoConfirm txtsubconLocalSupplier;
        private Win.UI.DisplayBox txtTotal;
        private Win.UI.Label lblTotal;
    }
}
