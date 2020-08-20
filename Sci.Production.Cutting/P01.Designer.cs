namespace Sci.Production.Cutting
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelCuttingSPNo = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelStyleNo = new Sci.Win.UI.Label();
            this.labelProject = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.labelsewingschedule = new Sci.Win.UI.Label();
            this.labelSewingScheduleCuttingInLine = new Sci.Win.UI.Label();
            this.labelSewingScheduleCuttingOffLine = new Sci.Win.UI.Label();
            this.labelSewingScheduleLastMtlETA = new Sci.Win.UI.Label();
            this.labelSewingScheduleRMtlETA = new Sci.Win.UI.Label();
            this.labelPOCombo = new Sci.Win.UI.Label();
            this.labelCuttingCombo = new Sci.Win.UI.Label();
            this.displayCuttingSPNo = new Sci.Win.UI.DisplayBox();
            this.displayStyleNo = new Sci.Win.UI.DisplayBox();
            this.displayProject = new Sci.Win.UI.DisplayBox();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.dateCuttingInLine = new Sci.Win.UI.DateBox();
            this.dateSewingScheduleCuttingOffLine = new Sci.Win.UI.DateBox();
            this.dateSewingScheduleLastMtlETA = new Sci.Win.UI.DateBox();
            this.dateSewingScheduleRMtlETA = new Sci.Win.UI.DateBox();
            this.labelSwitchtoWorkOrder = new Sci.Win.UI.Label();
            this.labelEachConsApvDate = new Sci.Win.UI.Label();
            this.labelFOCQty = new Sci.Win.UI.Label();
            this.labelOrderQty = new Sci.Win.UI.Label();
            this.labelEarliestSewingInline = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.displayCategory = new Sci.Win.UI.DisplayBox();
            this.displaySwitchtoWorkOrder = new Sci.Win.UI.DisplayBox();
            this.labelEarliestSewingOffline = new Sci.Win.UI.Label();
            this.labelWorkOrder = new Sci.Win.UI.Label();
            this.dateWorkOrderLastCutDate = new Sci.Win.UI.DateBox();
            this.dateWorkOrderFirstCutDate = new Sci.Win.UI.DateBox();
            this.dateWorkOrderCuttingOffLine = new Sci.Win.UI.DateBox();
            this.dateWorkOrderCuttingInLine = new Sci.Win.UI.DateBox();
            this.labelWorkOrderLastCutDate = new Sci.Win.UI.Label();
            this.labelWorkOrderFirstCutDate = new Sci.Win.UI.Label();
            this.labelWorkOrderCuttingOffLine = new Sci.Win.UI.Label();
            this.labelWorkOrderCuttingInLine = new Sci.Win.UI.Label();
            this.btnMarkerList = new Sci.Win.UI.Button();
            this.btnEachCons = new Sci.Win.UI.Button();
            this.btnBundleCard = new Sci.Win.UI.Button();
            this.btnQuantitybreakdown = new Sci.Win.UI.Button();
            this.btnCutPartsCheckSummary = new Sci.Win.UI.Button();
            this.btnCutPartsCheck = new Sci.Win.UI.Button();
            this.btnGarmentList = new Sci.Win.UI.Button();
            this.btnProductionkit = new Sci.Win.UI.Button();
            this.btnColorCombo = new Sci.Win.UI.Button();
            this.dateEarliestSewingOffline = new Sci.Win.UI.DateBox();
            this.dateEarliestSewingInline = new Sci.Win.UI.DateBox();
            this.dateEachConsApvDate = new Sci.Win.UI.DateBox();
            this.editPOCombo = new Sci.Win.UI.EditBox();
            this.editCuttingCombo = new Sci.Win.UI.EditBox();
            this.displayOrderQty = new Sci.Win.UI.DisplayBox();
            this.numFOCQty = new Sci.Win.UI.NumericBox();
            this.numOrderQty = new Sci.Win.UI.NumericBox();
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
            this.detail.Size = new System.Drawing.Size(893, 442);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.numOrderQty);
            this.detailcont.Controls.Add(this.numFOCQty);
            this.detailcont.Controls.Add(this.displayOrderQty);
            this.detailcont.Controls.Add(this.editCuttingCombo);
            this.detailcont.Controls.Add(this.editPOCombo);
            this.detailcont.Controls.Add(this.dateEarliestSewingOffline);
            this.detailcont.Controls.Add(this.dateEachConsApvDate);
            this.detailcont.Controls.Add(this.btnGarmentList);
            this.detailcont.Controls.Add(this.dateEarliestSewingInline);
            this.detailcont.Controls.Add(this.btnProductionkit);
            this.detailcont.Controls.Add(this.btnColorCombo);
            this.detailcont.Controls.Add(this.btnQuantitybreakdown);
            this.detailcont.Controls.Add(this.btnCutPartsCheckSummary);
            this.detailcont.Controls.Add(this.btnCutPartsCheck);
            this.detailcont.Controls.Add(this.btnBundleCard);
            this.detailcont.Controls.Add(this.btnEachCons);
            this.detailcont.Controls.Add(this.btnMarkerList);
            this.detailcont.Controls.Add(this.dateWorkOrderLastCutDate);
            this.detailcont.Controls.Add(this.labelWorkOrder);
            this.detailcont.Controls.Add(this.dateWorkOrderFirstCutDate);
            this.detailcont.Controls.Add(this.labelEarliestSewingOffline);
            this.detailcont.Controls.Add(this.dateWorkOrderCuttingOffLine);
            this.detailcont.Controls.Add(this.displaySwitchtoWorkOrder);
            this.detailcont.Controls.Add(this.dateWorkOrderCuttingInLine);
            this.detailcont.Controls.Add(this.labelWorkOrderLastCutDate);
            this.detailcont.Controls.Add(this.displayCategory);
            this.detailcont.Controls.Add(this.labelWorkOrderFirstCutDate);
            this.detailcont.Controls.Add(this.labelCategory);
            this.detailcont.Controls.Add(this.labelWorkOrderCuttingOffLine);
            this.detailcont.Controls.Add(this.labelEarliestSewingInline);
            this.detailcont.Controls.Add(this.labelWorkOrderCuttingInLine);
            this.detailcont.Controls.Add(this.labelOrderQty);
            this.detailcont.Controls.Add(this.labelFOCQty);
            this.detailcont.Controls.Add(this.labelEachConsApvDate);
            this.detailcont.Controls.Add(this.labelSwitchtoWorkOrder);
            this.detailcont.Controls.Add(this.dateCuttingInLine);
            this.detailcont.Controls.Add(this.dateSewingScheduleCuttingOffLine);
            this.detailcont.Controls.Add(this.dateSewingScheduleLastMtlETA);
            this.detailcont.Controls.Add(this.dateSewingScheduleRMtlETA);
            this.detailcont.Controls.Add(this.txtRemark);
            this.detailcont.Controls.Add(this.displaySeason);
            this.detailcont.Controls.Add(this.displayProject);
            this.detailcont.Controls.Add(this.displayStyleNo);
            this.detailcont.Controls.Add(this.displayCuttingSPNo);
            this.detailcont.Controls.Add(this.labelCuttingCombo);
            this.detailcont.Controls.Add(this.labelPOCombo);
            this.detailcont.Controls.Add(this.labelSewingScheduleRMtlETA);
            this.detailcont.Controls.Add(this.labelSewingScheduleLastMtlETA);
            this.detailcont.Controls.Add(this.labelSewingScheduleCuttingOffLine);
            this.detailcont.Controls.Add(this.labelSewingScheduleCuttingInLine);
            this.detailcont.Controls.Add(this.labelsewingschedule);
            this.detailcont.Controls.Add(this.labelRemark);
            this.detailcont.Controls.Add(this.labelProject);
            this.detailcont.Controls.Add(this.labelStyleNo);
            this.detailcont.Controls.Add(this.labelSeason);
            this.detailcont.Controls.Add(this.labelCuttingSPNo);
            this.detailcont.Size = new System.Drawing.Size(893, 404);
            this.detailcont.TabIndex = 1;
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 404);
            this.detailbtm.Size = new System.Drawing.Size(893, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(893, 442);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(901, 471);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelCuttingSPNo
            // 
            this.labelCuttingSPNo.Location = new System.Drawing.Point(28, 21);
            this.labelCuttingSPNo.Name = "labelCuttingSPNo";
            this.labelCuttingSPNo.Size = new System.Drawing.Size(75, 23);
            this.labelCuttingSPNo.TabIndex = 0;
            this.labelCuttingSPNo.Text = "CuttingSP#";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(28, 71);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(75, 23);
            this.labelSeason.TabIndex = 1;
            this.labelSeason.Text = "Season";
            // 
            // labelStyleNo
            // 
            this.labelStyleNo.Location = new System.Drawing.Point(28, 46);
            this.labelStyleNo.Name = "labelStyleNo";
            this.labelStyleNo.Size = new System.Drawing.Size(75, 23);
            this.labelStyleNo.TabIndex = 8;
            this.labelStyleNo.Text = "Style#";
            // 
            // labelProject
            // 
            this.labelProject.Location = new System.Drawing.Point(28, 121);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(75, 23);
            this.labelProject.TabIndex = 11;
            this.labelProject.Text = "Project";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(28, 146);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 12;
            this.labelRemark.Text = "Remark";
            // 
            // labelsewingschedule
            // 
            this.labelsewingschedule.BackColor = System.Drawing.Color.Transparent;
            this.labelsewingschedule.Location = new System.Drawing.Point(28, 171);
            this.labelsewingschedule.Name = "labelsewingschedule";
            this.labelsewingschedule.Size = new System.Drawing.Size(311, 23);
            this.labelsewingschedule.TabIndex = 13;
            this.labelsewingschedule.Text = "Cutting schedule calculate from sewing schedule";
            this.labelsewingschedule.TextStyle.Color = System.Drawing.Color.DimGray;
            // 
            // labelSewingScheduleCuttingInLine
            // 
            this.labelSewingScheduleCuttingInLine.Location = new System.Drawing.Point(28, 196);
            this.labelSewingScheduleCuttingInLine.Name = "labelSewingScheduleCuttingInLine";
            this.labelSewingScheduleCuttingInLine.Size = new System.Drawing.Size(165, 23);
            this.labelSewingScheduleCuttingInLine.TabIndex = 14;
            this.labelSewingScheduleCuttingInLine.Text = "Cutting In-Line";
            // 
            // labelSewingScheduleCuttingOffLine
            // 
            this.labelSewingScheduleCuttingOffLine.Location = new System.Drawing.Point(28, 221);
            this.labelSewingScheduleCuttingOffLine.Name = "labelSewingScheduleCuttingOffLine";
            this.labelSewingScheduleCuttingOffLine.Size = new System.Drawing.Size(165, 23);
            this.labelSewingScheduleCuttingOffLine.TabIndex = 15;
            this.labelSewingScheduleCuttingOffLine.Text = "Cutting Off-Line";
            // 
            // labelSewingScheduleLastMtlETA
            // 
            this.labelSewingScheduleLastMtlETA.Location = new System.Drawing.Point(28, 246);
            this.labelSewingScheduleLastMtlETA.Name = "labelSewingScheduleLastMtlETA";
            this.labelSewingScheduleLastMtlETA.Size = new System.Drawing.Size(165, 23);
            this.labelSewingScheduleLastMtlETA.TabIndex = 16;
            this.labelSewingScheduleLastMtlETA.Text = "Last Mtl ETA";
            // 
            // labelSewingScheduleRMtlETA
            // 
            this.labelSewingScheduleRMtlETA.Location = new System.Drawing.Point(28, 271);
            this.labelSewingScheduleRMtlETA.Name = "labelSewingScheduleRMtlETA";
            this.labelSewingScheduleRMtlETA.Size = new System.Drawing.Size(165, 23);
            this.labelSewingScheduleRMtlETA.TabIndex = 17;
            this.labelSewingScheduleRMtlETA.Text = "Act. MTL ETA(Master SP)";
            // 
            // labelPOCombo
            // 
            this.labelPOCombo.Location = new System.Drawing.Point(28, 296);
            this.labelPOCombo.Name = "labelPOCombo";
            this.labelPOCombo.Size = new System.Drawing.Size(109, 23);
            this.labelPOCombo.TabIndex = 18;
            this.labelPOCombo.Text = "PO Combo";
            // 
            // labelCuttingCombo
            // 
            this.labelCuttingCombo.Location = new System.Drawing.Point(28, 349);
            this.labelCuttingCombo.Name = "labelCuttingCombo";
            this.labelCuttingCombo.Size = new System.Drawing.Size(109, 23);
            this.labelCuttingCombo.TabIndex = 19;
            this.labelCuttingCombo.Text = "Cutting Combo";
            // 
            // displayCuttingSPNo
            // 
            this.displayCuttingSPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCuttingSPNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayCuttingSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCuttingSPNo.Location = new System.Drawing.Point(106, 21);
            this.displayCuttingSPNo.Name = "displayCuttingSPNo";
            this.displayCuttingSPNo.Size = new System.Drawing.Size(113, 23);
            this.displayCuttingSPNo.TabIndex = 1;
            // 
            // displayStyleNo
            // 
            this.displayStyleNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyleNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyleNo.Location = new System.Drawing.Point(106, 46);
            this.displayStyleNo.Name = "displayStyleNo";
            this.displayStyleNo.Size = new System.Drawing.Size(113, 23);
            this.displayStyleNo.TabIndex = 2;
            // 
            // displayProject
            // 
            this.displayProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayProject.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayProject.Location = new System.Drawing.Point(106, 121);
            this.displayProject.Name = "displayProject";
            this.displayProject.Size = new System.Drawing.Size(113, 23);
            this.displayProject.TabIndex = 5;
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(106, 71);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(64, 23);
            this.displaySeason.TabIndex = 3;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(106, 146);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(220, 23);
            this.txtRemark.TabIndex = 0;
            // 
            // dateCuttingInLine
            // 
            this.dateCuttingInLine.IsSupportEditMode = false;
            this.dateCuttingInLine.Location = new System.Drawing.Point(196, 196);
            this.dateCuttingInLine.Name = "dateCuttingInLine";
            this.dateCuttingInLine.ReadOnly = true;
            this.dateCuttingInLine.Size = new System.Drawing.Size(130, 23);
            this.dateCuttingInLine.TabIndex = 6;
            // 
            // dateSewingScheduleCuttingOffLine
            // 
            this.dateSewingScheduleCuttingOffLine.IsSupportEditMode = false;
            this.dateSewingScheduleCuttingOffLine.Location = new System.Drawing.Point(196, 221);
            this.dateSewingScheduleCuttingOffLine.Name = "dateSewingScheduleCuttingOffLine";
            this.dateSewingScheduleCuttingOffLine.ReadOnly = true;
            this.dateSewingScheduleCuttingOffLine.Size = new System.Drawing.Size(130, 23);
            this.dateSewingScheduleCuttingOffLine.TabIndex = 7;
            // 
            // dateSewingScheduleLastMtlETA
            // 
            this.dateSewingScheduleLastMtlETA.IsSupportEditMode = false;
            this.dateSewingScheduleLastMtlETA.Location = new System.Drawing.Point(196, 246);
            this.dateSewingScheduleLastMtlETA.Name = "dateSewingScheduleLastMtlETA";
            this.dateSewingScheduleLastMtlETA.ReadOnly = true;
            this.dateSewingScheduleLastMtlETA.Size = new System.Drawing.Size(130, 23);
            this.dateSewingScheduleLastMtlETA.TabIndex = 8;
            // 
            // dateSewingScheduleRMtlETA
            // 
            this.dateSewingScheduleRMtlETA.IsSupportEditMode = false;
            this.dateSewingScheduleRMtlETA.Location = new System.Drawing.Point(196, 271);
            this.dateSewingScheduleRMtlETA.Name = "dateSewingScheduleRMtlETA";
            this.dateSewingScheduleRMtlETA.ReadOnly = true;
            this.dateSewingScheduleRMtlETA.Size = new System.Drawing.Size(130, 23);
            this.dateSewingScheduleRMtlETA.TabIndex = 9;
            // 
            // labelSwitchtoWorkOrder
            // 
            this.labelSwitchtoWorkOrder.Location = new System.Drawing.Point(352, 21);
            this.labelSwitchtoWorkOrder.Name = "labelSwitchtoWorkOrder";
            this.labelSwitchtoWorkOrder.Size = new System.Drawing.Size(144, 23);
            this.labelSwitchtoWorkOrder.TabIndex = 30;
            this.labelSwitchtoWorkOrder.Text = "Switch to Work Order";
            // 
            // labelEachConsApvDate
            // 
            this.labelEachConsApvDate.Location = new System.Drawing.Point(352, 46);
            this.labelEachConsApvDate.Name = "labelEachConsApvDate";
            this.labelEachConsApvDate.Size = new System.Drawing.Size(144, 23);
            this.labelEachConsApvDate.TabIndex = 31;
            this.labelEachConsApvDate.Text = "Each Cons Apv Date";
            // 
            // labelFOCQty
            // 
            this.labelFOCQty.Location = new System.Drawing.Point(352, 71);
            this.labelFOCQty.Name = "labelFOCQty";
            this.labelFOCQty.Size = new System.Drawing.Size(75, 23);
            this.labelFOCQty.TabIndex = 32;
            this.labelFOCQty.Text = "FOC Qty";
            // 
            // labelOrderQty
            // 
            this.labelOrderQty.Location = new System.Drawing.Point(352, 96);
            this.labelOrderQty.Name = "labelOrderQty";
            this.labelOrderQty.Size = new System.Drawing.Size(75, 23);
            this.labelOrderQty.TabIndex = 33;
            this.labelOrderQty.Text = "Order Qty";
            // 
            // labelEarliestSewingInline
            // 
            this.labelEarliestSewingInline.Location = new System.Drawing.Point(352, 121);
            this.labelEarliestSewingInline.Name = "labelEarliestSewingInline";
            this.labelEarliestSewingInline.Size = new System.Drawing.Size(144, 23);
            this.labelEarliestSewingInline.TabIndex = 34;
            this.labelEarliestSewingInline.Text = "Earliest Sewing Inline";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(28, 96);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(75, 23);
            this.labelCategory.TabIndex = 35;
            this.labelCategory.Text = "Category";
            // 
            // displayCategory
            // 
            this.displayCategory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCategory.Location = new System.Drawing.Point(106, 96);
            this.displayCategory.Name = "displayCategory";
            this.displayCategory.Size = new System.Drawing.Size(113, 23);
            this.displayCategory.TabIndex = 4;
            // 
            // displaySwitchtoWorkOrder
            // 
            this.displaySwitchtoWorkOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySwitchtoWorkOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySwitchtoWorkOrder.Location = new System.Drawing.Point(499, 21);
            this.displaySwitchtoWorkOrder.Name = "displaySwitchtoWorkOrder";
            this.displaySwitchtoWorkOrder.Size = new System.Drawing.Size(198, 23);
            this.displaySwitchtoWorkOrder.TabIndex = 12;
            // 
            // labelEarliestSewingOffline
            // 
            this.labelEarliestSewingOffline.Location = new System.Drawing.Point(352, 146);
            this.labelEarliestSewingOffline.Name = "labelEarliestSewingOffline";
            this.labelEarliestSewingOffline.Size = new System.Drawing.Size(144, 23);
            this.labelEarliestSewingOffline.TabIndex = 38;
            this.labelEarliestSewingOffline.Text = "Earliest Sewing Offline";
            // 
            // labelWorkOrder
            // 
            this.labelWorkOrder.BackColor = System.Drawing.Color.Transparent;
            this.labelWorkOrder.Location = new System.Drawing.Point(352, 171);
            this.labelWorkOrder.Name = "labelWorkOrder";
            this.labelWorkOrder.Size = new System.Drawing.Size(311, 23);
            this.labelWorkOrder.TabIndex = 39;
            this.labelWorkOrder.Text = "Cutting schedule arranged from work order";
            this.labelWorkOrder.TextStyle.Color = System.Drawing.Color.DimGray;
            // 
            // dateWorkOrderLastCutDate
            // 
            this.dateWorkOrderLastCutDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "LastCutDate", true));
            this.dateWorkOrderLastCutDate.IsSupportEditMode = false;
            this.dateWorkOrderLastCutDate.Location = new System.Drawing.Point(464, 271);
            this.dateWorkOrderLastCutDate.Name = "dateWorkOrderLastCutDate";
            this.dateWorkOrderLastCutDate.ReadOnly = true;
            this.dateWorkOrderLastCutDate.Size = new System.Drawing.Size(130, 23);
            this.dateWorkOrderLastCutDate.TabIndex = 22;
            // 
            // dateWorkOrderFirstCutDate
            // 
            this.dateWorkOrderFirstCutDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FirstCutDate", true));
            this.dateWorkOrderFirstCutDate.IsSupportEditMode = false;
            this.dateWorkOrderFirstCutDate.Location = new System.Drawing.Point(464, 246);
            this.dateWorkOrderFirstCutDate.Name = "dateWorkOrderFirstCutDate";
            this.dateWorkOrderFirstCutDate.ReadOnly = true;
            this.dateWorkOrderFirstCutDate.Size = new System.Drawing.Size(130, 23);
            this.dateWorkOrderFirstCutDate.TabIndex = 21;
            // 
            // dateWorkOrderCuttingOffLine
            // 
            this.dateWorkOrderCuttingOffLine.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "cutoffline", true));
            this.dateWorkOrderCuttingOffLine.IsSupportEditMode = false;
            this.dateWorkOrderCuttingOffLine.Location = new System.Drawing.Point(464, 221);
            this.dateWorkOrderCuttingOffLine.Name = "dateWorkOrderCuttingOffLine";
            this.dateWorkOrderCuttingOffLine.ReadOnly = true;
            this.dateWorkOrderCuttingOffLine.Size = new System.Drawing.Size(130, 23);
            this.dateWorkOrderCuttingOffLine.TabIndex = 20;
            // 
            // dateWorkOrderCuttingInLine
            // 
            this.dateWorkOrderCuttingInLine.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "cutinline", true));
            this.dateWorkOrderCuttingInLine.IsSupportEditMode = false;
            this.dateWorkOrderCuttingInLine.Location = new System.Drawing.Point(464, 196);
            this.dateWorkOrderCuttingInLine.Name = "dateWorkOrderCuttingInLine";
            this.dateWorkOrderCuttingInLine.ReadOnly = true;
            this.dateWorkOrderCuttingInLine.Size = new System.Drawing.Size(130, 23);
            this.dateWorkOrderCuttingInLine.TabIndex = 19;
            // 
            // labelWorkOrderLastCutDate
            // 
            this.labelWorkOrderLastCutDate.Location = new System.Drawing.Point(352, 271);
            this.labelWorkOrderLastCutDate.Name = "labelWorkOrderLastCutDate";
            this.labelWorkOrderLastCutDate.Size = new System.Drawing.Size(109, 23);
            this.labelWorkOrderLastCutDate.TabIndex = 31;
            this.labelWorkOrderLastCutDate.Text = "Last Cut Date";
            // 
            // labelWorkOrderFirstCutDate
            // 
            this.labelWorkOrderFirstCutDate.Location = new System.Drawing.Point(352, 246);
            this.labelWorkOrderFirstCutDate.Name = "labelWorkOrderFirstCutDate";
            this.labelWorkOrderFirstCutDate.Size = new System.Drawing.Size(109, 23);
            this.labelWorkOrderFirstCutDate.TabIndex = 30;
            this.labelWorkOrderFirstCutDate.Text = "First Cut Date";
            // 
            // labelWorkOrderCuttingOffLine
            // 
            this.labelWorkOrderCuttingOffLine.Location = new System.Drawing.Point(352, 221);
            this.labelWorkOrderCuttingOffLine.Name = "labelWorkOrderCuttingOffLine";
            this.labelWorkOrderCuttingOffLine.Size = new System.Drawing.Size(109, 23);
            this.labelWorkOrderCuttingOffLine.TabIndex = 29;
            this.labelWorkOrderCuttingOffLine.Text = "Cutting Off-Line";
            // 
            // labelWorkOrderCuttingInLine
            // 
            this.labelWorkOrderCuttingInLine.Location = new System.Drawing.Point(352, 196);
            this.labelWorkOrderCuttingInLine.Name = "labelWorkOrderCuttingInLine";
            this.labelWorkOrderCuttingInLine.Size = new System.Drawing.Size(109, 23);
            this.labelWorkOrderCuttingInLine.TabIndex = 28;
            this.labelWorkOrderCuttingInLine.Text = "Cutting In-Line";
            // 
            // btnMarkerList
            // 
            this.btnMarkerList.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnMarkerList.Location = new System.Drawing.Point(734, 14);
            this.btnMarkerList.Name = "btnMarkerList";
            this.btnMarkerList.Size = new System.Drawing.Size(151, 24);
            this.btnMarkerList.TabIndex = 23;
            this.btnMarkerList.Text = "Marker List";
            this.btnMarkerList.UseVisualStyleBackColor = true;
            this.btnMarkerList.Click += new System.EventHandler(this.BtnMarkerList_Click);
            // 
            // btnEachCons
            // 
            this.btnEachCons.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnEachCons.Location = new System.Drawing.Point(734, 44);
            this.btnEachCons.Name = "btnEachCons";
            this.btnEachCons.Size = new System.Drawing.Size(151, 24);
            this.btnEachCons.TabIndex = 24;
            this.btnEachCons.Text = "Each Cons.";
            this.btnEachCons.UseVisualStyleBackColor = true;
            this.btnEachCons.Click += new System.EventHandler(this.BtnEachCons_Click);
            // 
            // btnBundleCard
            // 
            this.btnBundleCard.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnBundleCard.Location = new System.Drawing.Point(734, 74);
            this.btnBundleCard.Name = "btnBundleCard";
            this.btnBundleCard.Size = new System.Drawing.Size(151, 24);
            this.btnBundleCard.TabIndex = 25;
            this.btnBundleCard.Text = "Bundle Card";
            this.btnBundleCard.UseVisualStyleBackColor = true;
            this.btnBundleCard.Click += new System.EventHandler(this.BtnBundleCard_Click);
            // 
            // btnQuantitybreakdown
            // 
            this.btnQuantitybreakdown.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnQuantitybreakdown.Location = new System.Drawing.Point(734, 180);
            this.btnQuantitybreakdown.Name = "btnQuantitybreakdown";
            this.btnQuantitybreakdown.Size = new System.Drawing.Size(151, 24);
            this.btnQuantitybreakdown.TabIndex = 28;
            this.btnQuantitybreakdown.Text = "Quantity breakdown";
            this.btnQuantitybreakdown.UseVisualStyleBackColor = true;
            this.btnQuantitybreakdown.Click += new System.EventHandler(this.BtnQuantitybreakdown_Click);
            // 
            // btnCutPartsCheckSummary
            // 
            this.btnCutPartsCheckSummary.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnCutPartsCheckSummary.Location = new System.Drawing.Point(734, 134);
            this.btnCutPartsCheckSummary.Name = "btnCutPartsCheckSummary";
            this.btnCutPartsCheckSummary.Size = new System.Drawing.Size(151, 42);
            this.btnCutPartsCheckSummary.TabIndex = 27;
            this.btnCutPartsCheckSummary.Text = "Cut Parts Check Summary";
            this.btnCutPartsCheckSummary.UseVisualStyleBackColor = true;
            this.btnCutPartsCheckSummary.Click += new System.EventHandler(this.BtnCutPartsCheckSummary_Click);
            // 
            // btnCutPartsCheck
            // 
            this.btnCutPartsCheck.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnCutPartsCheck.Location = new System.Drawing.Point(734, 104);
            this.btnCutPartsCheck.Name = "btnCutPartsCheck";
            this.btnCutPartsCheck.Size = new System.Drawing.Size(151, 24);
            this.btnCutPartsCheck.TabIndex = 26;
            this.btnCutPartsCheck.Text = "Cut Parts Check";
            this.btnCutPartsCheck.UseVisualStyleBackColor = true;
            this.btnCutPartsCheck.Click += new System.EventHandler(this.BtnCutPartsCheck_Click);
            // 
            // btnGarmentList
            // 
            this.btnGarmentList.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnGarmentList.Location = new System.Drawing.Point(734, 270);
            this.btnGarmentList.Name = "btnGarmentList";
            this.btnGarmentList.Size = new System.Drawing.Size(151, 24);
            this.btnGarmentList.TabIndex = 31;
            this.btnGarmentList.Text = "Garment List";
            this.btnGarmentList.UseVisualStyleBackColor = true;
            this.btnGarmentList.Click += new System.EventHandler(this.BtnGarmentList_Click);
            // 
            // btnProductionkit
            // 
            this.btnProductionkit.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnProductionkit.Location = new System.Drawing.Point(734, 240);
            this.btnProductionkit.Name = "btnProductionkit";
            this.btnProductionkit.Size = new System.Drawing.Size(151, 24);
            this.btnProductionkit.TabIndex = 30;
            this.btnProductionkit.Text = "Production kit";
            this.btnProductionkit.UseVisualStyleBackColor = true;
            this.btnProductionkit.Click += new System.EventHandler(this.BtnProductionkit_Click);
            // 
            // btnColorCombo
            // 
            this.btnColorCombo.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnColorCombo.Location = new System.Drawing.Point(734, 210);
            this.btnColorCombo.Name = "btnColorCombo";
            this.btnColorCombo.Size = new System.Drawing.Size(151, 24);
            this.btnColorCombo.TabIndex = 29;
            this.btnColorCombo.Text = "Color Combo";
            this.btnColorCombo.UseVisualStyleBackColor = true;
            this.btnColorCombo.Click += new System.EventHandler(this.BtnColorCombo_Click);
            // 
            // dateEarliestSewingOffline
            // 
            this.dateEarliestSewingOffline.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "sewoffline", true));
            this.dateEarliestSewingOffline.IsSupportEditMode = false;
            this.dateEarliestSewingOffline.Location = new System.Drawing.Point(499, 146);
            this.dateEarliestSewingOffline.Name = "dateEarliestSewingOffline";
            this.dateEarliestSewingOffline.ReadOnly = true;
            this.dateEarliestSewingOffline.Size = new System.Drawing.Size(130, 23);
            this.dateEarliestSewingOffline.TabIndex = 18;
            // 
            // dateEarliestSewingInline
            // 
            this.dateEarliestSewingInline.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "sewinline", true));
            this.dateEarliestSewingInline.IsSupportEditMode = false;
            this.dateEarliestSewingInline.Location = new System.Drawing.Point(499, 121);
            this.dateEarliestSewingInline.Name = "dateEarliestSewingInline";
            this.dateEarliestSewingInline.ReadOnly = true;
            this.dateEarliestSewingInline.Size = new System.Drawing.Size(130, 23);
            this.dateEarliestSewingInline.TabIndex = 17;
            // 
            // dateEachConsApvDate
            // 
            this.dateEachConsApvDate.IsSupportEditMode = false;
            this.dateEachConsApvDate.Location = new System.Drawing.Point(499, 46);
            this.dateEachConsApvDate.Name = "dateEachConsApvDate";
            this.dateEachConsApvDate.ReadOnly = true;
            this.dateEachConsApvDate.Size = new System.Drawing.Size(130, 23);
            this.dateEachConsApvDate.TabIndex = 13;
            // 
            // editPOCombo
            // 
            this.editPOCombo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editPOCombo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editPOCombo.IsSupportEditMode = false;
            this.editPOCombo.Location = new System.Drawing.Point(140, 296);
            this.editPOCombo.Multiline = true;
            this.editPOCombo.Name = "editPOCombo";
            this.editPOCombo.ReadOnly = true;
            this.editPOCombo.Size = new System.Drawing.Size(557, 51);
            this.editPOCombo.TabIndex = 10;
            // 
            // editCuttingCombo
            // 
            this.editCuttingCombo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editCuttingCombo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editCuttingCombo.IsSupportEditMode = false;
            this.editCuttingCombo.Location = new System.Drawing.Point(140, 349);
            this.editCuttingCombo.Multiline = true;
            this.editCuttingCombo.Name = "editCuttingCombo";
            this.editCuttingCombo.ReadOnly = true;
            this.editCuttingCombo.Size = new System.Drawing.Size(557, 51);
            this.editCuttingCombo.TabIndex = 11;
            // 
            // displayOrderQty
            // 
            this.displayOrderQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayOrderQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayOrderQty.Location = new System.Drawing.Point(531, 96);
            this.displayOrderQty.Name = "displayOrderQty";
            this.displayOrderQty.Size = new System.Drawing.Size(58, 23);
            this.displayOrderQty.TabIndex = 16;
            // 
            // numFOCQty
            // 
            this.numFOCQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numFOCQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numFOCQty.IsSupportEditMode = false;
            this.numFOCQty.Location = new System.Drawing.Point(429, 71);
            this.numFOCQty.Name = "numFOCQty";
            this.numFOCQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numFOCQty.ReadOnly = true;
            this.numFOCQty.Size = new System.Drawing.Size(100, 23);
            this.numFOCQty.TabIndex = 14;
            this.numFOCQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numOrderQty
            // 
            this.numOrderQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numOrderQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numOrderQty.IsSupportEditMode = false;
            this.numOrderQty.Location = new System.Drawing.Point(429, 96);
            this.numOrderQty.Name = "numOrderQty";
            this.numOrderQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numOrderQty.ReadOnly = true;
            this.numOrderQty.Size = new System.Drawing.Size(100, 23);
            this.numOrderQty.TabIndex = 15;
            this.numOrderQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // P01
            // 
            this.ClientSize = new System.Drawing.Size(901, 504);
            this.DefaultControl = "txtRemark";
            this.DefaultControlForEdit = "txtRemark";
            this.DefaultOrder = "id";
            this.ExpressQuery = true;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.Name = "P01";
            this.Text = "() () ";
            this.WorkAlias = "Cutting";
            this.FormLoaded += new System.EventHandler(this.P01_FormLoaded);
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

        private Win.UI.Label labelStyleNo;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelCuttingSPNo;
        private Win.UI.Label labelCuttingCombo;
        private Win.UI.Label labelPOCombo;
        private Win.UI.Label labelSewingScheduleRMtlETA;
        private Win.UI.Label labelSewingScheduleLastMtlETA;
        private Win.UI.Label labelSewingScheduleCuttingOffLine;
        private Win.UI.Label labelSewingScheduleCuttingInLine;
        private Win.UI.Label labelsewingschedule;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelProject;
        private Win.UI.Button btnEachCons;
        private Win.UI.Button btnMarkerList;
        private Win.UI.DateBox dateWorkOrderLastCutDate;
        private Win.UI.Label labelWorkOrder;
        private Win.UI.DateBox dateWorkOrderFirstCutDate;
        private Win.UI.Label labelEarliestSewingOffline;
        private Win.UI.DateBox dateWorkOrderCuttingOffLine;
        private Win.UI.DisplayBox displaySwitchtoWorkOrder;
        private Win.UI.DateBox dateWorkOrderCuttingInLine;
        private Win.UI.Label labelWorkOrderLastCutDate;
        private Win.UI.DisplayBox displayCategory;
        private Win.UI.Label labelWorkOrderFirstCutDate;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelWorkOrderCuttingOffLine;
        private Win.UI.Label labelEarliestSewingInline;
        private Win.UI.Label labelWorkOrderCuttingInLine;
        private Win.UI.Label labelOrderQty;
        private Win.UI.Label labelFOCQty;
        private Win.UI.Label labelEachConsApvDate;
        private Win.UI.Label labelSwitchtoWorkOrder;
        private Win.UI.DateBox dateSewingScheduleRMtlETA;
        private Win.UI.DateBox dateSewingScheduleLastMtlETA;
        private Win.UI.DateBox dateSewingScheduleCuttingOffLine;
        private Win.UI.DateBox dateCuttingInLine;
        private Win.UI.TextBox txtRemark;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.DisplayBox displayProject;
        private Win.UI.DisplayBox displayStyleNo;
        private Win.UI.DisplayBox displayCuttingSPNo;
        private Win.UI.Button btnGarmentList;
        private Win.UI.Button btnProductionkit;
        private Win.UI.Button btnColorCombo;
        private Win.UI.Button btnQuantitybreakdown;
        private Win.UI.Button btnCutPartsCheckSummary;
        private Win.UI.Button btnCutPartsCheck;
        private Win.UI.Button btnBundleCard;
        private Win.UI.DateBox dateEachConsApvDate;
        private Win.UI.DateBox dateEarliestSewingOffline;
        private Win.UI.DateBox dateEarliestSewingInline;
        private Win.UI.EditBox editCuttingCombo;
        private Win.UI.EditBox editPOCombo;
        private Win.UI.DisplayBox displayOrderQty;
        private Win.UI.NumericBox numOrderQty;
        private Win.UI.NumericBox numFOCQty;
    }
}
