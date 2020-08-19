namespace Sci.Production.Logistic
{
    partial class P11
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
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.dateDisposeDate = new Sci.Win.UI.DateBox();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.btnImport = new Sci.Win.UI.Button();
            this.lblStatus = new Sci.Win.UI.Label();
            this.txtClogReason = new Sci.Production.Class.TxtClogReason();
            this.labReason = new Sci.Win.UI.Label();
            this.btnDownloadExcel = new Sci.Win.UI.Button();
            this.btnExcelImport = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.btnExcelImport);
            this.masterpanel.Controls.Add(this.btnDownloadExcel);
            this.masterpanel.Controls.Add(this.labReason);
            this.masterpanel.Controls.Add(this.txtClogReason);
            this.masterpanel.Controls.Add(this.lblStatus);
            this.masterpanel.Controls.Add(this.btnImport);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Controls.Add(this.dateDisposeDate);
            this.masterpanel.Size = new System.Drawing.Size(963, 200);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateDisposeDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImport, 0);
            this.masterpanel.Controls.SetChildIndex(this.lblStatus, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtClogReason, 0);
            this.masterpanel.Controls.SetChildIndex(this.labReason, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnDownloadExcel, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnExcelImport, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 200);
            this.detailpanel.Size = new System.Drawing.Size(963, 227);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(855, 162);
            this.gridicon.TabIndex = 5;
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(963, 227);
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
            this.detail.Size = new System.Drawing.Size(963, 465);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(963, 427);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 427);
            this.detailbtm.Size = new System.Drawing.Size(963, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(963, 465);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(971, 494);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(255, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "Date";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(19, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "Remark";
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(97, 13);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(136, 23);
            this.displayID.TabIndex = 0;
            // 
            // dateDisposeDate
            // 
            this.dateDisposeDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "DisposeDate", true));
            this.dateDisposeDate.Location = new System.Drawing.Point(333, 13);
            this.dateDisposeDate.Name = "dateDisposeDate";
            this.dateDisposeDate.ReadOnly = true;
            this.dateDisposeDate.Size = new System.Drawing.Size(130, 23);
            this.dateDisposeDate.TabIndex = 1;
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(97, 72);
            this.editRemark.MaxLength = 100;
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(586, 122);
            this.editRemark.TabIndex = 3;
            // 
            // btnImport
            // 
            this.btnImport.Enabled = false;
            this.btnImport.Location = new System.Drawing.Point(875, 46);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 4;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblStatus.Location = new System.Drawing.Point(840, 13);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(115, 23);
            this.lblStatus.TabIndex = 45;
            this.lblStatus.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // txtClogReason
            // 
            this.txtClogReason.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "ClogReasonID", true));
            this.txtClogReason.DisplayBox1Binding = "";
            this.txtClogReason.Location = new System.Drawing.Point(97, 38);
            this.txtClogReason.Name = "txtClogReason";
            this.txtClogReason.Size = new System.Drawing.Size(386, 27);
            this.txtClogReason.TabIndex = 2;
            this.txtClogReason.TextBox1Binding = "";
            this.txtClogReason.Type = "GD";
            // 
            // labReason
            // 
            this.labReason.Location = new System.Drawing.Point(19, 42);
            this.labReason.Name = "labReason";
            this.labReason.Size = new System.Drawing.Size(75, 23);
            this.labReason.TabIndex = 7;
            this.labReason.Text = "Reason ";
            // 
            // btnDownloadExcel
            // 
            this.btnDownloadExcel.Location = new System.Drawing.Point(820, 119);
            this.btnDownloadExcel.Name = "btnDownloadExcel";
            this.btnDownloadExcel.Size = new System.Drawing.Size(135, 30);
            this.btnDownloadExcel.TabIndex = 6;
            this.btnDownloadExcel.Text = "Download Excel";
            this.btnDownloadExcel.UseVisualStyleBackColor = true;
            this.btnDownloadExcel.Click += new System.EventHandler(this.BtnDownloadExcel_Click);
            // 
            // btnExcelImport
            // 
            this.btnExcelImport.Enabled = false;
            this.btnExcelImport.Location = new System.Drawing.Point(829, 83);
            this.btnExcelImport.Name = "btnExcelImport";
            this.btnExcelImport.Size = new System.Drawing.Size(126, 30);
            this.btnExcelImport.TabIndex = 5;
            this.btnExcelImport.Text = "Excel Import";
            this.btnExcelImport.UseVisualStyleBackColor = true;
            this.btnExcelImport.Click += new System.EventHandler(this.BtnExcelImport_Click);
            // 
            // P11
            // 
            this.ApvChkValue = "New";
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 527);
            this.GridAlias = "ClogGarmentDispose_Detail";
            this.GridUniqueKey = "ID,PackingListID,CTNStartNO";
            this.IsSupportClip = false;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P11";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P11. Clog Garment Dispose";
            this.UnApvChkValue = "Confirmed";
            this.WorkAlias = "ClogGarmentDispose";
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

        private Win.UI.Button btnImport;
        private Win.UI.EditBox editRemark;
        private Win.UI.DateBox dateDisposeDate;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Label lblStatus;
        private Win.UI.Label labReason;
        private Class.TxtClogReason txtClogReason;
        private Win.UI.Button btnExcelImport;
        private Win.UI.Button btnDownloadExcel;
    }
}