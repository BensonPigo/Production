namespace Sci.Production.Warehouse
{
    partial class P64
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
            this.label1 = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.label2 = new Sci.Win.UI.Label();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
            this.label3 = new Sci.Win.UI.Label();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.labelStatus = new Sci.Win.UI.Label();
            this.labelPackages = new Sci.Win.UI.Label();
            this.btnAccumulatedQty = new Sci.Win.UI.Button();
            this.btnDownloadSampleFile = new Sci.Win.UI.Button();
            this.btnImportFromExcel = new Sci.Win.UI.Button();
            this.numPackages = new Sci.Win.UI.NumericBox();
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
            this.masterpanel.Controls.Add(this.numPackages);
            this.masterpanel.Controls.Add(this.btnImportFromExcel);
            this.masterpanel.Controls.Add(this.btnDownloadSampleFile);
            this.masterpanel.Controls.Add(this.btnAccumulatedQty);
            this.masterpanel.Controls.Add(this.labelPackages);
            this.masterpanel.Controls.Add(this.labelStatus);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Controls.Add(this.dateIssueDate);
            this.masterpanel.Size = new System.Drawing.Size(858, 151);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelStatus, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPackages, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnAccumulatedQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnDownloadSampleFile, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportFromExcel, 0);
            this.masterpanel.Controls.SetChildIndex(this.numPackages, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 151);
            this.detailpanel.Size = new System.Drawing.Size(858, 259);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gridicon.Location = new System.Drawing.Point(535, 113);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(858, 259);
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
            this.detail.Size = new System.Drawing.Size(858, 448);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(858, 410);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 410);
            this.detailbtm.Size = new System.Drawing.Size(858, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(858, 448);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(866, 477);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "ID";
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(92, 11);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(129, 23);
            this.displayID.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(224, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Issue Date";
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IssueDate", true));
            this.dateIssueDate.Location = new System.Drawing.Point(313, 11);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(130, 23);
            this.dateIssueDate.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Remark";
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(92, 38);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(577, 67);
            this.editRemark.TabIndex = 6;
            // 
            // labelStatus
            // 
            this.labelStatus.BackColor = System.Drawing.Color.Transparent;
            this.labelStatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Status", true));
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.labelStatus.Location = new System.Drawing.Point(692, 11);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(134, 23);
            this.labelStatus.TabIndex = 44;
            this.labelStatus.Text = "Not Approve";
            this.labelStatus.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // labelPackages
            // 
            this.labelPackages.Location = new System.Drawing.Point(457, 11);
            this.labelPackages.Name = "labelPackages";
            this.labelPackages.Size = new System.Drawing.Size(74, 23);
            this.labelPackages.TabIndex = 45;
            this.labelPackages.Text = "Packages";
            // 
            // btnAccumulatedQty
            // 
            this.btnAccumulatedQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAccumulatedQty.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnAccumulatedQty.Location = new System.Drawing.Point(682, 39);
            this.btnAccumulatedQty.Name = "btnAccumulatedQty";
            this.btnAccumulatedQty.Size = new System.Drawing.Size(158, 30);
            this.btnAccumulatedQty.TabIndex = 47;
            this.btnAccumulatedQty.Text = "Accumulated Qty";
            this.btnAccumulatedQty.UseVisualStyleBackColor = true;
            this.btnAccumulatedQty.Click += new System.EventHandler(this.BtnAccumulatedQty_Click);
            // 
            // btnDownloadSampleFile
            // 
            this.btnDownloadSampleFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownloadSampleFile.Location = new System.Drawing.Point(682, 75);
            this.btnDownloadSampleFile.Name = "btnDownloadSampleFile";
            this.btnDownloadSampleFile.Size = new System.Drawing.Size(158, 30);
            this.btnDownloadSampleFile.TabIndex = 48;
            this.btnDownloadSampleFile.Text = "Download Sample File";
            this.btnDownloadSampleFile.UseVisualStyleBackColor = true;
            this.btnDownloadSampleFile.Click += new System.EventHandler(this.BtnDownloadSampleFile_Click);
            // 
            // btnImportFromExcel
            // 
            this.btnImportFromExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportFromExcel.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImportFromExcel.Location = new System.Drawing.Point(682, 111);
            this.btnImportFromExcel.Name = "btnImportFromExcel";
            this.btnImportFromExcel.Size = new System.Drawing.Size(158, 30);
            this.btnImportFromExcel.TabIndex = 49;
            this.btnImportFromExcel.Text = "Import From Excel";
            this.btnImportFromExcel.UseVisualStyleBackColor = true;
            this.btnImportFromExcel.Click += new System.EventHandler(this.BtnImportFromExcel_Click);
            // 
            // numPackages
            // 
            this.numPackages.BackColor = System.Drawing.Color.White;
            this.numPackages.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Packages", true));
            this.numPackages.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numPackages.Location = new System.Drawing.Point(534, 11);
            this.numPackages.Name = "numPackages";
            this.numPackages.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPackages.Size = new System.Drawing.Size(90, 23);
            this.numPackages.TabIndex = 50;
            this.numPackages.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // P64
            // 
            this.ApvChkValue = "New";
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 510);
            this.GridAlias = "SemiFinishedReceiving_Detail";
            this.IsSupportClip = false;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P64";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P64. Material Receiving (Semi-finished)";
            this.UnApvChkValue = "Confirmed";
            this.WorkAlias = "SemiFinishedReceiving";
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

        private Win.UI.EditBox editRemark;
        private Win.UI.Label label3;
        private Win.UI.DateBox dateIssueDate;
        private Win.UI.Label label2;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label label1;
        private Win.UI.Label labelStatus;
        private Win.UI.Label labelPackages;
        private Win.UI.Button btnImportFromExcel;
        private Win.UI.Button btnDownloadSampleFile;
        private Win.UI.Button btnAccumulatedQty;
        private Win.UI.NumericBox numPackages;
    }
}