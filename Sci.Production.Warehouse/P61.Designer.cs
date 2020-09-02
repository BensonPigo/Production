namespace Sci.Production.Warehouse
{
    partial class P61
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
            this.txtRemark = new Sci.Win.UI.EditBox();
            this.labelRemark = new Sci.Win.UI.Label();
            this.btnImport = new Sci.Win.UI.Button();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.lblStatus = new Sci.Win.UI.Label();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.labelID = new Sci.Win.UI.Label();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.btnClearEmpty = new Sci.Win.UI.Button();
            this.btnFind = new Sci.Win.UI.Button();
            this.txtLocateForSP = new Sci.Win.UI.TextBox();
            this.labelLocateForSP = new Sci.Win.UI.Label();
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
            this.masterpanel.Controls.Add(this.btnClearEmpty);
            this.masterpanel.Controls.Add(this.btnFind);
            this.masterpanel.Controls.Add(this.txtLocateForSP);
            this.masterpanel.Controls.Add(this.labelLocateForSP);
            this.masterpanel.Controls.Add(this.txtRemark);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.btnImport);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.lblStatus);
            this.masterpanel.Controls.Add(this.labelIssueDate);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Controls.Add(this.dateIssueDate);
            this.masterpanel.Controls.Add(this.shapeContainer1);
            this.masterpanel.Size = new System.Drawing.Size(885, 140);
            this.masterpanel.Controls.SetChildIndex(this.shapeContainer1, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.lblStatus, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImport, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelLocateForSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLocateForSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnFind, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnClearEmpty, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 140);
            this.detailpanel.Size = new System.Drawing.Size(885, 308);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridicon.Location = new System.Drawing.Point(753, 104);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(885, 308);
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
            this.detail.Size = new System.Drawing.Size(885, 486);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(885, 448);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 448);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(885, 486);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(893, 515);
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(87, 39);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(658, 51);
            this.txtRemark.TabIndex = 62;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(6, 39);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 67;
            this.labelRemark.Text = "Remark";
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(796, 39);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(89, 31);
            this.btnImport.TabIndex = 64;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IssueDate", true));
            this.dateIssueDate.Location = new System.Drawing.Point(342, 9);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(130, 23);
            this.dateIssueDate.TabIndex = 60;
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(88, 9);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(120, 23);
            this.displayID.TabIndex = 61;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Status", true));
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblStatus.Location = new System.Drawing.Point(761, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(115, 23);
            this.lblStatus.TabIndex = 66;
            this.lblStatus.Text = "New";
            this.lblStatus.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(231, 9);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.Size = new System.Drawing.Size(108, 23);
            this.labelIssueDate.TabIndex = 65;
            this.labelIssueDate.Text = "Issue Date";
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(6, 9);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 63;
            this.labelID.Text = "ID";
            // 
            // lineShape1
            // 
            this.lineShape1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 5;
            this.lineShape1.X2 = 877;
            this.lineShape1.Y1 = 99;
            this.lineShape1.Y2 = 99;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(885, 140);
            this.shapeContainer1.TabIndex = 68;
            this.shapeContainer1.TabStop = false;
            // 
            // btnClearEmpty
            // 
            this.btnClearEmpty.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnClearEmpty.Location = new System.Drawing.Point(344, 104);
            this.btnClearEmpty.Name = "btnClearEmpty";
            this.btnClearEmpty.Size = new System.Drawing.Size(148, 30);
            this.btnClearEmpty.TabIndex = 71;
            this.btnClearEmpty.Text = "Clear Qty is empty";
            this.btnClearEmpty.UseVisualStyleBackColor = true;
            this.btnClearEmpty.Click += new System.EventHandler(this.BtnClearEmpty_Click);
            // 
            // btnFind
            // 
            this.btnFind.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnFind.Location = new System.Drawing.Point(268, 104);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(70, 30);
            this.btnFind.TabIndex = 70;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // txtLocateForSP
            // 
            this.txtLocateForSP.BackColor = System.Drawing.Color.White;
            this.txtLocateForSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateForSP.IsSupportEditMode = false;
            this.txtLocateForSP.Location = new System.Drawing.Point(117, 108);
            this.txtLocateForSP.Name = "txtLocateForSP";
            this.txtLocateForSP.Size = new System.Drawing.Size(145, 23);
            this.txtLocateForSP.TabIndex = 69;
            // 
            // labelLocateForSP
            // 
            this.labelLocateForSP.Location = new System.Drawing.Point(5, 108);
            this.labelLocateForSP.Name = "labelLocateForSP";
            this.labelLocateForSP.Size = new System.Drawing.Size(109, 23);
            this.labelLocateForSP.TabIndex = 72;
            this.labelLocateForSP.Text = "Locate for SP#";
            // 
            // P61
            // 
            this.ApvChkValue = "New";
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 548);
            this.Grid2New = 0;
            this.GridAlias = "LocalIssue_Detail";
            this.GridNew = 0;
            this.GridUniqueKey = "OrderID,Refno,ThreadColorID";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.KeyField2 = "ID";
            this.Name = "P61";
            this.Text = "P61. Issue Local Item";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "ID";
            this.WorkAlias = "LocalIssue";
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

        private Win.UI.EditBox txtRemark;
        private Win.UI.Label labelRemark;
        private Win.UI.Button btnImport;
        private Win.UI.DateBox dateIssueDate;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label lblStatus;
        private Win.UI.Label labelIssueDate;
        private Win.UI.Label labelID;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.Button btnClearEmpty;
        private Win.UI.Button btnFind;
        private Win.UI.TextBox txtLocateForSP;
        private Win.UI.Label labelLocateForSP;
    }
}