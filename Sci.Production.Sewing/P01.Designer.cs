namespace Sci.Production.Sewing
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
            this.labelDate = new Sci.Win.UI.Label();
            this.labelLine = new Sci.Win.UI.Label();
            this.labelShift = new Sci.Win.UI.Label();
            this.labelTeam = new Sci.Win.UI.Label();
            this.dateDate = new Sci.Win.UI.DateBox();
            this.displayFactory = new Sci.Win.UI.DisplayBox();
            this.comboTeam = new Sci.Win.UI.ComboBox();
            this.labelManpower = new Sci.Win.UI.Label();
            this.labelWHours = new Sci.Win.UI.Label();
            this.labelManhours = new Sci.Win.UI.Label();
            this.labelCostingTMS = new Sci.Win.UI.Label();
            this.numManpower = new Sci.Win.UI.NumericBox();
            this.numWHours = new Sci.Win.UI.NumericBox();
            this.numManhours = new Sci.Win.UI.NumericBox();
            this.numCostingTMS = new Sci.Win.UI.NumericBox();
            this.labelQAOutput = new Sci.Win.UI.Label();
            this.labelDefectOutput = new Sci.Win.UI.Label();
            this.labelInLineOutput = new Sci.Win.UI.Label();
            this.labelEff = new Sci.Win.UI.Label();
            this.numQAOutput = new Sci.Win.UI.NumericBox();
            this.numDefectOutput = new Sci.Win.UI.NumericBox();
            this.numInLineOutput = new Sci.Win.UI.NumericBox();
            this.numEff = new Sci.Win.UI.NumericBox();
            this.btnRevisedHistory = new Sci.Win.UI.Button();
            this.btnShareWorkingHoursToSP = new Sci.Win.UI.Button();
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtdropdownlistShift = new Sci.Production.Class.Txtdropdownlist();
            this.txtsewinglineLine = new Sci.Production.Class.Txtsewingline();
            this.labelSubconOutFty = new Sci.Win.UI.Label();
            this.txtSubconOutFty = new Sci.Production.Class.TxtLocalSupp();
            this.label1 = new Sci.Win.UI.Label();
            this.txtSubConOutContractNumber = new Sci.Win.UI.TextBox();
            this.btnRequestUnlock = new Sci.Win.UI.Button();
            this.btnBatchRecall = new Sci.Win.UI.Button();
            this.lbstatus = new Sci.Win.UI.Label();
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
            this.masterpanel.Controls.Add(this.lbstatus);
            this.masterpanel.Controls.Add(this.btnRequestUnlock);
            this.masterpanel.Controls.Add(this.txtSubConOutContractNumber);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Controls.Add(this.txtSubconOutFty);
            this.masterpanel.Controls.Add(this.labelSubconOutFty);
            this.masterpanel.Controls.Add(this.labelFactory);
            this.masterpanel.Controls.Add(this.displayFactory);
            this.masterpanel.Controls.Add(this.btnShareWorkingHoursToSP);
            this.masterpanel.Controls.Add(this.btnRevisedHistory);
            this.masterpanel.Controls.Add(this.numEff);
            this.masterpanel.Controls.Add(this.numInLineOutput);
            this.masterpanel.Controls.Add(this.numDefectOutput);
            this.masterpanel.Controls.Add(this.numQAOutput);
            this.masterpanel.Controls.Add(this.labelEff);
            this.masterpanel.Controls.Add(this.labelInLineOutput);
            this.masterpanel.Controls.Add(this.labelDefectOutput);
            this.masterpanel.Controls.Add(this.labelQAOutput);
            this.masterpanel.Controls.Add(this.numCostingTMS);
            this.masterpanel.Controls.Add(this.numManhours);
            this.masterpanel.Controls.Add(this.numWHours);
            this.masterpanel.Controls.Add(this.numManpower);
            this.masterpanel.Controls.Add(this.labelCostingTMS);
            this.masterpanel.Controls.Add(this.labelManhours);
            this.masterpanel.Controls.Add(this.labelWHours);
            this.masterpanel.Controls.Add(this.labelManpower);
            this.masterpanel.Controls.Add(this.comboTeam);
            this.masterpanel.Controls.Add(this.txtdropdownlistShift);
            this.masterpanel.Controls.Add(this.txtsewinglineLine);
            this.masterpanel.Controls.Add(this.labelTeam);
            this.masterpanel.Controls.Add(this.labelShift);
            this.masterpanel.Controls.Add(this.labelLine);
            this.masterpanel.Controls.Add(this.labelDate);
            this.masterpanel.Controls.Add(this.dateDate);
            this.masterpanel.Size = new System.Drawing.Size(1000, 140);
            this.masterpanel.Controls.SetChildIndex(this.dateDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelLine, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelShift, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTeam, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtsewinglineLine, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtdropdownlistShift, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboTeam, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelManpower, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelWHours, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelManhours, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCostingTMS, 0);
            this.masterpanel.Controls.SetChildIndex(this.numManpower, 0);
            this.masterpanel.Controls.SetChildIndex(this.numWHours, 0);
            this.masterpanel.Controls.SetChildIndex(this.numManhours, 0);
            this.masterpanel.Controls.SetChildIndex(this.numCostingTMS, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelQAOutput, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDefectOutput, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelInLineOutput, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelEff, 0);
            this.masterpanel.Controls.SetChildIndex(this.numQAOutput, 0);
            this.masterpanel.Controls.SetChildIndex(this.numDefectOutput, 0);
            this.masterpanel.Controls.SetChildIndex(this.numInLineOutput, 0);
            this.masterpanel.Controls.SetChildIndex(this.numEff, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnRevisedHistory, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnShareWorkingHoursToSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSubconOutFty, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSubconOutFty, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSubConOutContractNumber, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnRequestUnlock, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbstatus, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 140);
            this.detailpanel.Size = new System.Drawing.Size(1000, 285);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(887, 105);
            this.gridicon.TabIndex = 8;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(832, 3);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1000, 285);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(1000, 463);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1000, 425);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 425);
            this.detailbtm.Size = new System.Drawing.Size(1000, 38);
            this.detailbtm.TabIndex = 1;
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1000, 463);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1008, 492);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(476, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(428, 13);
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(5, 4);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(104, 23);
            this.labelDate.TabIndex = 1;
            this.labelDate.Text = "Date";
            // 
            // labelLine
            // 
            this.labelLine.Location = new System.Drawing.Point(5, 31);
            this.labelLine.Name = "labelLine";
            this.labelLine.Size = new System.Drawing.Size(104, 23);
            this.labelLine.TabIndex = 2;
            this.labelLine.Text = "Line#";
            // 
            // labelShift
            // 
            this.labelShift.Location = new System.Drawing.Point(198, 58);
            this.labelShift.Name = "labelShift";
            this.labelShift.Size = new System.Drawing.Size(50, 23);
            this.labelShift.TabIndex = 3;
            this.labelShift.Text = "Shift";
            // 
            // labelTeam
            // 
            this.labelTeam.Location = new System.Drawing.Point(5, 59);
            this.labelTeam.Name = "labelTeam";
            this.labelTeam.Size = new System.Drawing.Size(104, 23);
            this.labelTeam.TabIndex = 4;
            this.labelTeam.Text = "Team";
            // 
            // dateDate
            // 
            this.dateDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "OutputDate", true));
            this.dateDate.Location = new System.Drawing.Point(112, 3);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(130, 23);
            this.dateDate.TabIndex = 0;
            this.dateDate.Validating += new System.ComponentModel.CancelEventHandler(this.DateDate_Validating);
            // 
            // displayFactory
            // 
            this.displayFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFactory.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FactoryID", true));
            this.displayFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFactory.Location = new System.Drawing.Point(249, 31);
            this.displayFactory.Name = "displayFactory";
            this.displayFactory.Size = new System.Drawing.Size(41, 23);
            this.displayFactory.TabIndex = 4;
            // 
            // comboTeam
            // 
            this.comboTeam.BackColor = System.Drawing.Color.White;
            this.comboTeam.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Team", true));
            this.comboTeam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboTeam.FormattingEnabled = true;
            this.comboTeam.IsSupportUnselect = true;
            this.comboTeam.Location = new System.Drawing.Point(111, 58);
            this.comboTeam.Name = "comboTeam";
            this.comboTeam.OldText = "";
            this.comboTeam.Size = new System.Drawing.Size(57, 24);
            this.comboTeam.TabIndex = 3;
            this.comboTeam.SelectedIndexChanged += new System.EventHandler(this.ComboTeam_SelectedIndexChanged);
            // 
            // labelManpower
            // 
            this.labelManpower.Location = new System.Drawing.Point(377, 4);
            this.labelManpower.Name = "labelManpower";
            this.labelManpower.Size = new System.Drawing.Size(92, 23);
            this.labelManpower.TabIndex = 9;
            this.labelManpower.Text = "Manpower";
            // 
            // labelWHours
            // 
            this.labelWHours.Location = new System.Drawing.Point(377, 31);
            this.labelWHours.Name = "labelWHours";
            this.labelWHours.Size = new System.Drawing.Size(92, 23);
            this.labelWHours.TabIndex = 10;
            this.labelWHours.Text = "W/Hours(Day)";
            // 
            // labelManhours
            // 
            this.labelManhours.Location = new System.Drawing.Point(377, 58);
            this.labelManhours.Name = "labelManhours";
            this.labelManhours.Size = new System.Drawing.Size(92, 23);
            this.labelManhours.TabIndex = 11;
            this.labelManhours.Text = "Manhours";
            // 
            // labelCostingTMS
            // 
            this.labelCostingTMS.Location = new System.Drawing.Point(377, 85);
            this.labelCostingTMS.Name = "labelCostingTMS";
            this.labelCostingTMS.Size = new System.Drawing.Size(115, 23);
            this.labelCostingTMS.TabIndex = 12;
            this.labelCostingTMS.Text = "Costing TMS(sec)";
            // 
            // numManpower
            // 
            this.numManpower.BackColor = System.Drawing.Color.White;
            this.numManpower.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Manpower", true));
            this.numManpower.DecimalPlaces = 1;
            this.numManpower.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numManpower.Location = new System.Drawing.Point(473, 4);
            this.numManpower.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            65536});
            this.numManpower.Name = "numManpower";
            this.numManpower.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numManpower.Size = new System.Drawing.Size(54, 23);
            this.numManpower.TabIndex = 4;
            this.numManpower.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numManpower.Validated += new System.EventHandler(this.NumManpower_Validated);
            // 
            // numWHours
            // 
            this.numWHours.BackColor = System.Drawing.Color.White;
            this.numWHours.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "WorkHour", true));
            this.numWHours.DecimalPlaces = 2;
            this.numWHours.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numWHours.Location = new System.Drawing.Point(473, 31);
            this.numWHours.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            65536});
            this.numWHours.Name = "numWHours";
            this.numWHours.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWHours.Size = new System.Drawing.Size(54, 23);
            this.numWHours.TabIndex = 5;
            this.numWHours.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWHours.Validated += new System.EventHandler(this.NumWHours_Validated);
            // 
            // numManhours
            // 
            this.numManhours.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numManhours.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ManHour", true));
            this.numManhours.DecimalPlaces = 3;
            this.numManhours.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numManhours.IsSupportEditMode = false;
            this.numManhours.Location = new System.Drawing.Point(473, 58);
            this.numManhours.Name = "numManhours";
            this.numManhours.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numManhours.ReadOnly = true;
            this.numManhours.Size = new System.Drawing.Size(54, 23);
            this.numManhours.TabIndex = 8;
            this.numManhours.TabStop = false;
            this.numManhours.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numCostingTMS
            // 
            this.numCostingTMS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numCostingTMS.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "TMS", true));
            this.numCostingTMS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numCostingTMS.IsSupportEditMode = false;
            this.numCostingTMS.Location = new System.Drawing.Point(496, 85);
            this.numCostingTMS.Name = "numCostingTMS";
            this.numCostingTMS.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCostingTMS.ReadOnly = true;
            this.numCostingTMS.Size = new System.Drawing.Size(68, 23);
            this.numCostingTMS.TabIndex = 11;
            this.numCostingTMS.TabStop = false;
            this.numCostingTMS.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelQAOutput
            // 
            this.labelQAOutput.Location = new System.Drawing.Point(578, 4);
            this.labelQAOutput.Name = "labelQAOutput";
            this.labelQAOutput.Size = new System.Drawing.Size(93, 23);
            this.labelQAOutput.TabIndex = 17;
            this.labelQAOutput.Text = "QA Output";
            // 
            // labelDefectOutput
            // 
            this.labelDefectOutput.Location = new System.Drawing.Point(578, 31);
            this.labelDefectOutput.Name = "labelDefectOutput";
            this.labelDefectOutput.Size = new System.Drawing.Size(93, 23);
            this.labelDefectOutput.TabIndex = 18;
            this.labelDefectOutput.Text = "Defect Output";
            // 
            // labelInLineOutput
            // 
            this.labelInLineOutput.Location = new System.Drawing.Point(578, 58);
            this.labelInLineOutput.Name = "labelInLineOutput";
            this.labelInLineOutput.Size = new System.Drawing.Size(93, 23);
            this.labelInLineOutput.TabIndex = 19;
            this.labelInLineOutput.Text = "In-Line Output";
            // 
            // labelEff
            // 
            this.labelEff.Location = new System.Drawing.Point(578, 85);
            this.labelEff.Name = "labelEff";
            this.labelEff.Size = new System.Drawing.Size(93, 23);
            this.labelEff.TabIndex = 20;
            this.labelEff.Text = "Eff(%)";
            // 
            // numQAOutput
            // 
            this.numQAOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numQAOutput.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "QAQty", true));
            this.numQAOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numQAOutput.IsSupportEditMode = false;
            this.numQAOutput.Location = new System.Drawing.Point(675, 4);
            this.numQAOutput.Name = "numQAOutput";
            this.numQAOutput.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numQAOutput.ReadOnly = true;
            this.numQAOutput.Size = new System.Drawing.Size(62, 23);
            this.numQAOutput.TabIndex = 2;
            this.numQAOutput.TabStop = false;
            this.numQAOutput.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numDefectOutput
            // 
            this.numDefectOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numDefectOutput.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "DefectQty", true));
            this.numDefectOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numDefectOutput.IsSupportEditMode = false;
            this.numDefectOutput.Location = new System.Drawing.Point(675, 31);
            this.numDefectOutput.Name = "numDefectOutput";
            this.numDefectOutput.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDefectOutput.ReadOnly = true;
            this.numDefectOutput.Size = new System.Drawing.Size(62, 23);
            this.numDefectOutput.TabIndex = 6;
            this.numDefectOutput.TabStop = false;
            this.numDefectOutput.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numInLineOutput
            // 
            this.numInLineOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numInLineOutput.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "InlineQty", true));
            this.numInLineOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numInLineOutput.IsSupportEditMode = false;
            this.numInLineOutput.Location = new System.Drawing.Point(675, 58);
            this.numInLineOutput.Name = "numInLineOutput";
            this.numInLineOutput.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numInLineOutput.ReadOnly = true;
            this.numInLineOutput.Size = new System.Drawing.Size(62, 23);
            this.numInLineOutput.TabIndex = 9;
            this.numInLineOutput.TabStop = false;
            this.numInLineOutput.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numEff
            // 
            this.numEff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numEff.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Efficiency", true));
            this.numEff.DecimalPlaces = 1;
            this.numEff.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numEff.IsSupportEditMode = false;
            this.numEff.Location = new System.Drawing.Point(675, 85);
            this.numEff.Name = "numEff";
            this.numEff.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numEff.ReadOnly = true;
            this.numEff.Size = new System.Drawing.Size(62, 23);
            this.numEff.TabIndex = 12;
            this.numEff.TabStop = false;
            this.numEff.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // btnRevisedHistory
            // 
            this.btnRevisedHistory.Location = new System.Drawing.Point(865, 38);
            this.btnRevisedHistory.Name = "btnRevisedHistory";
            this.btnRevisedHistory.Size = new System.Drawing.Size(122, 30);
            this.btnRevisedHistory.TabIndex = 6;
            this.btnRevisedHistory.Text = "Revised History";
            this.btnRevisedHistory.UseVisualStyleBackColor = true;
            this.btnRevisedHistory.Click += new System.EventHandler(this.BtnRevisedHistory_Click);
            // 
            // btnShareWorkingHoursToSP
            // 
            this.btnShareWorkingHoursToSP.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnShareWorkingHoursToSP.Location = new System.Drawing.Point(750, 72);
            this.btnShareWorkingHoursToSP.Name = "btnShareWorkingHoursToSP";
            this.btnShareWorkingHoursToSP.Size = new System.Drawing.Size(237, 30);
            this.btnShareWorkingHoursToSP.TabIndex = 7;
            this.btnShareWorkingHoursToSP.Text = "Share <working hours> to SP#";
            this.btnShareWorkingHoursToSP.UseVisualStyleBackColor = true;
            this.btnShareWorkingHoursToSP.Click += new System.EventHandler(this.BtnShareWorkingHoursToSP_Click);
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(198, 30);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(50, 23);
            this.labelFactory.TabIndex = 28;
            this.labelFactory.Text = "Factory";
            // 
            // txtdropdownlistShift
            // 
            this.txtdropdownlistShift.BackColor = System.Drawing.Color.White;
            this.txtdropdownlistShift.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Shift", true));
            this.txtdropdownlistShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlistShift.FormattingEnabled = true;
            this.txtdropdownlistShift.IsSupportUnselect = true;
            this.txtdropdownlistShift.Location = new System.Drawing.Point(249, 58);
            this.txtdropdownlistShift.Name = "txtdropdownlistShift";
            this.txtdropdownlistShift.OldText = "";
            this.txtdropdownlistShift.Size = new System.Drawing.Size(115, 24);
            this.txtdropdownlistShift.TabIndex = 2;
            this.txtdropdownlistShift.Type = "SewingOutput_Shift";
            this.txtdropdownlistShift.SelectedValueChanged += new System.EventHandler(this.TxtdropdownlistShift_SelectedValueChanged);
            // 
            // txtsewinglineLine
            // 
            this.txtsewinglineLine.BackColor = System.Drawing.Color.White;
            this.txtsewinglineLine.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SewingLineID", true));
            this.txtsewinglineLine.FactoryobjectName = this.displayFactory;
            this.txtsewinglineLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtsewinglineLine.Location = new System.Drawing.Point(112, 30);
            this.txtsewinglineLine.Name = "txtsewinglineLine";
            this.txtsewinglineLine.Size = new System.Drawing.Size(60, 23);
            this.txtsewinglineLine.TabIndex = 1;
            this.txtsewinglineLine.Validating += new System.ComponentModel.CancelEventHandler(this.TxtsewinglineLine_Validating);
            // 
            // labelSubconOutFty
            // 
            this.labelSubconOutFty.Location = new System.Drawing.Point(5, 85);
            this.labelSubconOutFty.Name = "labelSubconOutFty";
            this.labelSubconOutFty.Size = new System.Drawing.Size(104, 23);
            this.labelSubconOutFty.TabIndex = 29;
            this.labelSubconOutFty.Text = "Subcon-Out-Fty";
            // 
            // txtSubconOutFty
            // 
            this.txtSubconOutFty.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "SubconOutFty", true));
            this.txtSubconOutFty.DisplayBox1Binding = "";
            this.txtSubconOutFty.Location = new System.Drawing.Point(112, 84);
            this.txtSubconOutFty.Name = "txtSubconOutFty";
            this.txtSubconOutFty.Size = new System.Drawing.Size(252, 23);
            this.txtSubconOutFty.TabIndex = 30;
            this.txtSubconOutFty.TextBox1Binding = "";
            this.txtSubconOutFty.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSubconOutFty_Validating);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 23);
            this.label1.TabIndex = 31;
            this.label1.Text = "SubCon-Out Contract Number";
            // 
            // txtSubConOutContractNumber
            // 
            this.txtSubConOutContractNumber.BackColor = System.Drawing.Color.White;
            this.txtSubConOutContractNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SubConOutContractNumber", true));
            this.txtSubConOutContractNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSubConOutContractNumber.Location = new System.Drawing.Point(198, 112);
            this.txtSubConOutContractNumber.Name = "txtSubConOutContractNumber";
            this.txtSubConOutContractNumber.Size = new System.Drawing.Size(248, 23);
            this.txtSubConOutContractNumber.TabIndex = 33;
            this.txtSubConOutContractNumber.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSubConOutContractNumber_Validating);
            // 
            // btnRequestUnlock
            // 
            this.btnRequestUnlock.Location = new System.Drawing.Point(865, 4);
            this.btnRequestUnlock.Name = "btnRequestUnlock";
            this.btnRequestUnlock.Size = new System.Drawing.Size(122, 30);
            this.btnRequestUnlock.TabIndex = 34;
            this.btnRequestUnlock.Text = "Request Unlock";
            this.btnRequestUnlock.UseVisualStyleBackColor = true;
            this.btnRequestUnlock.Click += new System.EventHandler(this.BtnRequestUnlock_Click);
            // 
            // btnBatchRecall
            // 
            this.btnBatchRecall.Location = new System.Drawing.Point(869, 18);
            this.btnBatchRecall.Name = "btnBatchRecall";
            this.btnBatchRecall.Size = new System.Drawing.Size(122, 30);
            this.btnBatchRecall.TabIndex = 35;
            this.btnBatchRecall.Text = "Batch Recall";
            this.btnBatchRecall.UseVisualStyleBackColor = true;
            this.btnBatchRecall.Click += new System.EventHandler(this.BtnBatchRecall_Click);
            // 
            // lbstatus
            // 
            this.lbstatus.AutoSize = true;
            this.lbstatus.BackColor = System.Drawing.Color.Transparent;
            this.lbstatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lbstatus.Location = new System.Drawing.Point(747, 7);
            this.lbstatus.Name = "lbstatus";
            this.lbstatus.Size = new System.Drawing.Size(109, 23);
            this.lbstatus.TabIndex = 35;
            this.lbstatus.TextStyle.BorderColor = System.Drawing.Color.Fuchsia;
            this.lbstatus.TextStyle.Color = System.Drawing.Color.Fuchsia;
            this.lbstatus.TextStyle.ExtBorderColor = System.Drawing.Color.Fuchsia;
            this.lbstatus.TextStyle.GradientColor = System.Drawing.Color.Fuchsia;
            // 
            // P01
            // 
            this.ClientSize = new System.Drawing.Size(1008, 525);
            this.Controls.Add(this.btnBatchRecall);
            this.DefaultControl = "dateDate";
            this.DefaultControlForEdit = "txtdropdownlistShift";
            this.DefaultDetailOrder = "OrderID,ComboType,Article";
            this.DefaultOrder = "OutputDate,SewingLineID,Shift,Team";
            this.GridAlias = "SewingOutput_Detail";
            this.GridUniqueKey = "ID,OrderID,ComboType,Article";
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.IsSupportRecall = true;
            this.IsSupportSend = true;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P01";
            this.OnLineHelpID = "Sci.Win.Tems.Input8";
            this.RecallChkValue = "Sent";
            this.SubDetailKeyField1 = "id,ukey";
            this.SubDetailKeyField2 = "id,SewingOutput_DetailUKey";
            this.SubGridAlias = "SewingOutput_Detail_Detail";
            this.SubKeyField1 = "UKey";
            this.Text = "P01. Sewing Daily Output";
            this.UnApvChkValue = "Locked";
            this.UniqueExpress = "ID";
            this.WorkAlias = "SewingOutput";
            this.Controls.SetChildIndex(this.tabs, 0);
            this.Controls.SetChildIndex(this.btnBatchRecall, 0);
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

        private Win.UI.DisplayBox displayFactory;
        private Win.UI.Button btnShareWorkingHoursToSP;
        private Win.UI.Button btnRevisedHistory;
        private Win.UI.NumericBox numEff;
        private Win.UI.NumericBox numInLineOutput;
        private Win.UI.NumericBox numDefectOutput;
        private Win.UI.NumericBox numQAOutput;
        private Win.UI.Label labelEff;
        private Win.UI.Label labelInLineOutput;
        private Win.UI.Label labelDefectOutput;
        private Win.UI.Label labelQAOutput;
        private Win.UI.NumericBox numCostingTMS;
        private Win.UI.NumericBox numManhours;
        private Win.UI.NumericBox numWHours;
        private Win.UI.NumericBox numManpower;
        private Win.UI.Label labelCostingTMS;
        private Win.UI.Label labelManhours;
        private Win.UI.Label labelWHours;
        private Win.UI.Label labelManpower;
        private Win.UI.ComboBox comboTeam;
        private Class.Txtdropdownlist txtdropdownlistShift;
        private Class.Txtsewingline txtsewinglineLine;
        private Win.UI.DateBox dateDate;
        private Win.UI.Label labelTeam;
        private Win.UI.Label labelShift;
        private Win.UI.Label labelLine;
        private Win.UI.Label labelDate;
        private Win.UI.Label labelFactory;
        private Class.TxtLocalSupp txtSubconOutFty;
        private Win.UI.Label labelSubconOutFty;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtSubConOutContractNumber;
        private Win.UI.Button btnRequestUnlock;
        private Win.UI.Button btnBatchRecall;
        private Win.UI.Label lbstatus;
    }
}
