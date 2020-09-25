namespace Sci.Production.Warehouse
{
    partial class P08
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
            this.labelArriveWHDate = new Sci.Win.UI.Label();
            this.labelSendDate = new Sci.Win.UI.Label();
            this.labelNotApprove = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.dateArriveWHDate = new Sci.Win.UI.DateBox();
            this.dateSendDate = new Sci.Win.UI.DateBox();
            this.btnAccumulatedQty = new Sci.Win.UI.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.btnFind = new Sci.Win.UI.Button();
            this.txtLocateForSP = new Sci.Win.UI.TextBox();
            this.labelLocateForSP = new Sci.Win.UI.Label();
            this.btnDeleteAll = new Sci.Win.UI.Button();
            this.labelRemark = new Sci.Win.UI.Label();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.btnImport = new Sci.Win.UI.Button();
            this.btnPrintFabricSitcker = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.btnPrintFabricSitcker);
            this.masterpanel.Controls.Add(this.btnImport);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.btnDeleteAll);
            this.masterpanel.Controls.Add(this.btnFind);
            this.masterpanel.Controls.Add(this.txtLocateForSP);
            this.masterpanel.Controls.Add(this.labelLocateForSP);
            this.masterpanel.Controls.Add(this.btnAccumulatedQty);
            this.masterpanel.Controls.Add(this.dateSendDate);
            this.masterpanel.Controls.Add(this.dateArriveWHDate);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.labelNotApprove);
            this.masterpanel.Controls.Add(this.labelSendDate);
            this.masterpanel.Controls.Add(this.labelArriveWHDate);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Controls.Add(this.shapeContainer1);
            this.masterpanel.Size = new System.Drawing.Size(932, 200);
            this.masterpanel.Controls.SetChildIndex(this.shapeContainer1, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelArriveWHDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSendDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNotApprove, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateArriveWHDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateSendDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnAccumulatedQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelLocateForSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLocateForSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnFind, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnDeleteAll, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImport, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnPrintFabricSitcker, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 200);
            this.detailpanel.Size = new System.Drawing.Size(932, 315);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.gridicon.Location = new System.Drawing.Point(803, 162);
            this.gridicon.TabIndex = 15;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(822, 1);
            this.refresh.TabIndex = 0;
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(932, 315);
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
            this.detail.Size = new System.Drawing.Size(932, 553);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(932, 515);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 515);
            this.detailbtm.Size = new System.Drawing.Size(932, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(932, 553);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(940, 582);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(468, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(420, 13);
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(16, 13);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 1;
            this.labelID.Text = "ID";
            // 
            // labelArriveWHDate
            // 
            this.labelArriveWHDate.Location = new System.Drawing.Point(241, 13);
            this.labelArriveWHDate.Name = "labelArriveWHDate";
            this.labelArriveWHDate.Size = new System.Drawing.Size(108, 23);
            this.labelArriveWHDate.TabIndex = 11;
            this.labelArriveWHDate.Text = "Arrive W/H Date";
            // 
            // labelSendDate
            // 
            this.labelSendDate.Location = new System.Drawing.Point(512, 13);
            this.labelSendDate.Name = "labelSendDate";
            this.labelSendDate.Size = new System.Drawing.Size(75, 23);
            this.labelSendDate.TabIndex = 14;
            this.labelSendDate.Text = "Send Date";
            // 
            // labelNotApprove
            // 
            this.labelNotApprove.BackColor = System.Drawing.Color.Transparent;
            this.labelNotApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.labelNotApprove.Location = new System.Drawing.Point(779, 13);
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
            // dateArriveWHDate
            // 
            this.dateArriveWHDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "whseArrival", true));
            this.dateArriveWHDate.Location = new System.Drawing.Point(352, 13);
            this.dateArriveWHDate.Name = "dateArriveWHDate";
            this.dateArriveWHDate.Size = new System.Drawing.Size(130, 23);
            this.dateArriveWHDate.TabIndex = 3;
            // 
            // dateSendDate
            // 
            this.dateSendDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Transfer2Taipei", true));
            this.dateSendDate.IsSupportCalendar = false;
            this.dateSendDate.IsSupportEditMode = false;
            this.dateSendDate.Location = new System.Drawing.Point(590, 13);
            this.dateSendDate.Name = "dateSendDate";
            this.dateSendDate.ReadOnly = true;
            this.dateSendDate.Size = new System.Drawing.Size(130, 23);
            this.dateSendDate.TabIndex = 8;
            // 
            // btnAccumulatedQty
            // 
            this.btnAccumulatedQty.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnAccumulatedQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnAccumulatedQty.Location = new System.Drawing.Point(757, 79);
            this.btnAccumulatedQty.Name = "btnAccumulatedQty";
            this.btnAccumulatedQty.Size = new System.Drawing.Size(145, 31);
            this.btnAccumulatedQty.TabIndex = 10;
            this.btnAccumulatedQty.Text = "Accumulated Qty";
            this.btnAccumulatedQty.UseVisualStyleBackColor = true;
            this.btnAccumulatedQty.Click += new System.EventHandler(this.BtnAccumulatedQty_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(932, 200);
            this.shapeContainer1.TabIndex = 54;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 17;
            this.lineShape1.X2 = 912;
            this.lineShape1.Y1 = 154;
            this.lineShape1.Y2 = 154;
            // 
            // btnFind
            // 
            this.btnFind.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnFind.Location = new System.Drawing.Point(279, 167);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(70, 30);
            this.btnFind.TabIndex = 5;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // txtLocateForSP
            // 
            this.txtLocateForSP.BackColor = System.Drawing.Color.White;
            this.txtLocateForSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateForSP.IsSupportEditMode = false;
            this.txtLocateForSP.Location = new System.Drawing.Point(128, 171);
            this.txtLocateForSP.Name = "txtLocateForSP";
            this.txtLocateForSP.Size = new System.Drawing.Size(145, 23);
            this.txtLocateForSP.TabIndex = 4;
            // 
            // labelLocateForSP
            // 
            this.labelLocateForSP.Location = new System.Drawing.Point(16, 171);
            this.labelLocateForSP.Name = "labelLocateForSP";
            this.labelLocateForSP.Size = new System.Drawing.Size(109, 23);
            this.labelLocateForSP.TabIndex = 58;
            this.labelLocateForSP.Text = "Locate for SP#";
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnDeleteAll.Location = new System.Drawing.Point(355, 167);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(80, 30);
            this.btnDeleteAll.TabIndex = 7;
            this.btnDeleteAll.Text = "Delete all";
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.BtnDeleteAll_Click);
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
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(94, 47);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(658, 63);
            this.editRemark.TabIndex = 60;
            // 
            // btnImport
            // 
            this.btnImport.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(758, 39);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(145, 31);
            this.btnImport.TabIndex = 9;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // btnPrintFabricSitcker
            // 
            this.btnPrintFabricSitcker.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnPrintFabricSitcker.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnPrintFabricSitcker.Location = new System.Drawing.Point(758, 116);
            this.btnPrintFabricSitcker.Name = "btnPrintFabricSitcker";
            this.btnPrintFabricSitcker.Size = new System.Drawing.Size(145, 31);
            this.btnPrintFabricSitcker.TabIndex = 61;
            this.btnPrintFabricSitcker.Text = "Print Fabric Sitcker";
            this.btnPrintFabricSitcker.UseVisualStyleBackColor = true;
            this.btnPrintFabricSitcker.Click += new System.EventHandler(this.BtnPrintFabricSitcker_Click);
            // 
            // P08
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(940, 615);
            this.DefaultControl = "dateArriveWHDate";
            this.DefaultControlForEdit = "dateArriveWHDate";
            this.DefaultDetailOrder = "poid,seq1,seq2,dyelot,roll";
            this.DefaultOrder = "WhseArrival,ID";
            this.Grid2New = 0;
            this.GridAlias = "Receiving_detail";
            this.GridNew = 0;
            this.GridUniqueKey = "Mdivisionid,poid,seq1,seq2,roll,Dyelot";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P08";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P08. Receiving From Factory Supply";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "id";
            this.WorkAlias = "Receiving";
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

        private Win.UI.Label labelSendDate;
        private Win.UI.Label labelArriveWHDate;
        private Win.UI.Label labelID;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label labelNotApprove;
        private Win.UI.Button btnAccumulatedQty;
        private Win.UI.DateBox dateSendDate;
        private Win.UI.DateBox dateArriveWHDate;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.Button btnFind;
        private Win.UI.TextBox txtLocateForSP;
        private Win.UI.Label labelLocateForSP;
        private Win.UI.Button btnDeleteAll;
        private Win.UI.EditBox editRemark;
        private Win.UI.Label labelRemark;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnPrintFabricSitcker;
    }
}
