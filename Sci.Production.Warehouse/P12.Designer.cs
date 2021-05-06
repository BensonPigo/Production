namespace Sci.Production.Warehouse
{
    partial class P12
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
            this.labelNotApprove = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
            this.btnImport = new Sci.Win.UI.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.btnFind = new Sci.Win.UI.Button();
            this.txtLocateForSP = new Sci.Win.UI.TextBox();
            this.labelLocateForSP = new Sci.Win.UI.Label();
            this.btnClearQtyIsEmpty = new Sci.Win.UI.Button();
            this.labelRemark = new Sci.Win.UI.Label();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.btnCallP99 = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.btnClearQtyIsEmpty);
            this.masterpanel.Controls.Add(this.btnFind);
            this.masterpanel.Controls.Add(this.txtLocateForSP);
            this.masterpanel.Controls.Add(this.labelLocateForSP);
            this.masterpanel.Controls.Add(this.btnImport);
            this.masterpanel.Controls.Add(this.dateIssueDate);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.labelNotApprove);
            this.masterpanel.Controls.Add(this.labelIssueDate);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Controls.Add(this.shapeContainer1);
            this.masterpanel.Size = new System.Drawing.Size(978, 155);
            this.masterpanel.Controls.SetChildIndex(this.shapeContainer1, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNotApprove, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImport, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelLocateForSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLocateForSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnFind, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnClearQtyIsEmpty, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 155);
            this.detailpanel.Size = new System.Drawing.Size(978, 322);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.gridicon.Location = new System.Drawing.Point(855, 115);
            this.gridicon.TabIndex = 6;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(784, 1);
            this.refresh.TabIndex = 0;
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(978, 322);
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
            this.detail.Size = new System.Drawing.Size(978, 515);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(978, 477);
            // 
            // detailbtm
            // 
            this.detailbtm.Controls.Add(this.btnCallP99);
            this.detailbtm.Location = new System.Drawing.Point(0, 477);
            this.detailbtm.Size = new System.Drawing.Size(978, 38);
            this.detailbtm.Controls.SetChildIndex(this.lbleditby, 0);
            this.detailbtm.Controls.SetChildIndex(this.lblcreateby, 0);
            this.detailbtm.Controls.SetChildIndex(this.editby, 0);
            this.detailbtm.Controls.SetChildIndex(this.createby, 0);
            this.detailbtm.Controls.SetChildIndex(this.refresh, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnCallP99, 0);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(978, 515);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(986, 544);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(325, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(446, 7);
            this.editby.Size = new System.Drawing.Size(332, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(398, 13);
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
            // labelNotApprove
            // 
            this.labelNotApprove.BackColor = System.Drawing.Color.Transparent;
            this.labelNotApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.labelNotApprove.Location = new System.Drawing.Point(764, 13);
            this.labelNotApprove.Name = "labelNotApprove";
            this.labelNotApprove.Size = new System.Drawing.Size(115, 23);
            this.labelNotApprove.TabIndex = 43;
            this.labelNotApprove.Text = "Not Approve";
            this.labelNotApprove.TextStyle.Color = System.Drawing.Color.Red;
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
            // btnImport
            // 
            this.btnImport.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(775, 43);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(89, 31);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(978, 155);
            this.shapeContainer1.TabIndex = 54;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 17;
            this.lineShape1.X2 = 957;
            this.lineShape1.Y1 = 106;
            this.lineShape1.Y2 = 106;
            // 
            // btnFind
            // 
            this.btnFind.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnFind.Location = new System.Drawing.Point(279, 115);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(70, 30);
            this.btnFind.TabIndex = 4;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // txtLocateForSP
            // 
            this.txtLocateForSP.BackColor = System.Drawing.Color.White;
            this.txtLocateForSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateForSP.IsSupportEditMode = false;
            this.txtLocateForSP.Location = new System.Drawing.Point(128, 119);
            this.txtLocateForSP.Name = "txtLocateForSP";
            this.txtLocateForSP.Size = new System.Drawing.Size(145, 23);
            this.txtLocateForSP.TabIndex = 3;
            // 
            // labelLocateForSP
            // 
            this.labelLocateForSP.Location = new System.Drawing.Point(16, 119);
            this.labelLocateForSP.Name = "labelLocateForSP";
            this.labelLocateForSP.Size = new System.Drawing.Size(109, 23);
            this.labelLocateForSP.TabIndex = 58;
            this.labelLocateForSP.Text = "Locate for SP#";
            // 
            // btnClearQtyIsEmpty
            // 
            this.btnClearQtyIsEmpty.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnClearQtyIsEmpty.Location = new System.Drawing.Point(355, 115);
            this.btnClearQtyIsEmpty.Name = "btnClearQtyIsEmpty";
            this.btnClearQtyIsEmpty.Size = new System.Drawing.Size(148, 30);
            this.btnClearQtyIsEmpty.TabIndex = 5;
            this.btnClearQtyIsEmpty.Text = "Clear Qty is empty";
            this.btnClearQtyIsEmpty.UseVisualStyleBackColor = true;
            this.btnClearQtyIsEmpty.Click += new System.EventHandler(this.BtnClearQtyIsEmpty_Click);
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(16, 47);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 59;
            this.labelRemark.Text = "Remark";
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(94, 47);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(658, 51);
            this.editRemark.TabIndex = 1;
            // 
            // btnCallP99
            // 
            this.btnCallP99.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnCallP99.Location = new System.Drawing.Point(866, 1);
            this.btnCallP99.Name = "btnCallP99";
            this.btnCallP99.Size = new System.Drawing.Size(104, 35);
            this.btnCallP99.TabIndex = 71;
            this.btnCallP99.Text = "Link To P99";
            this.btnCallP99.UseVisualStyleBackColor = true;
            this.btnCallP99.Click += new System.EventHandler(this.BtnCallP99_Click);
            // 
            // P12
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(986, 577);
            this.DefaultControl = "editRemark";
            this.DefaultControlForEdit = "editRemark";
            this.DefaultDetailOrder = "poid,seq1,seq2,dyelot,roll";
            this.DefaultOrder = "issuedate,ID";
            this.Grid2New = 0;
            this.GridAlias = "issue_detail";
            this.GridNew = 0;
            this.GridUniqueKey = "mdivisionid,poid,seq1,seq2,roll,Dyelot";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P12";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P12. Issue Packing Material";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "id";
            this.WorkAlias = "Issue";
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
        private Win.UI.Label labelNotApprove;
        private Win.UI.Button btnImport;
        private Win.UI.DateBox dateIssueDate;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.Button btnFind;
        private Win.UI.TextBox txtLocateForSP;
        private Win.UI.Label labelLocateForSP;
        private Win.UI.Button btnClearQtyIsEmpty;
        private Win.UI.EditBox editRemark;
        private Win.UI.Label labelRemark;
        private Win.UI.Button btnCallP99;
    }
}
