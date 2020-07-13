namespace Sci.Production.Planning
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
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelStyleNo = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelDestination = new Sci.Win.UI.Label();
            this.labelSewingInlineDate = new Sci.Win.UI.Label();
            this.labelSewingOfflineDate = new Sci.Win.UI.Label();
            this.labelSCIDeliveryDate = new Sci.Win.UI.Label();
            this.labelBuyerDeliveryDate = new Sci.Win.UI.Label();
            this.labelPPICMR = new Sci.Win.UI.Label();
            this.labelSewingLine = new Sci.Win.UI.Label();
            this.labelNeededPerDay = new Sci.Win.UI.Label();
            this.labelFirstCutDate = new Sci.Win.UI.Label();
            this.labelCutQty = new Sci.Win.UI.Label();
            this.labelOrderQty = new Sci.Win.UI.Label();
            this.displaySPNo = new Sci.Win.UI.DisplayBox();
            this.displayStyleNo = new Sci.Win.UI.DisplayBox();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.displayBrand = new Sci.Win.UI.DisplayBox();
            this.txtcountryDestination = new Sci.Production.Class.Txtcountry();
            this.dateSewingInlineDate = new Sci.Win.UI.DateBox();
            this.dateSewingOfflineDate = new Sci.Win.UI.DateBox();
            this.dateSCIDeliveryDate = new Sci.Win.UI.DateBox();
            this.dateBuyerDeliveryDate = new Sci.Win.UI.DateBox();
            this.txtuserPPICMR = new Sci.Production.Class.Txtuser();
            this.dateFirstCutDate = new Sci.Win.UI.DateBox();
            this.numCutQty = new Sci.Win.UI.NumericBox();
            this.numOrderQty = new Sci.Win.UI.NumericBox();
            this.numNeedPerDay = new Sci.Win.UI.NumericBox();
            this.displaySewingLine = new Sci.Win.UI.DisplayBox();
            this.btnBatchApprove = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.displaySewingLine);
            this.masterpanel.Controls.Add(this.numNeedPerDay);
            this.masterpanel.Controls.Add(this.numOrderQty);
            this.masterpanel.Controls.Add(this.numCutQty);
            this.masterpanel.Controls.Add(this.txtuserPPICMR);
            this.masterpanel.Controls.Add(this.txtcountryDestination);
            this.masterpanel.Controls.Add(this.displayBrand);
            this.masterpanel.Controls.Add(this.displaySeason);
            this.masterpanel.Controls.Add(this.displayStyleNo);
            this.masterpanel.Controls.Add(this.displaySPNo);
            this.masterpanel.Controls.Add(this.labelOrderQty);
            this.masterpanel.Controls.Add(this.labelCutQty);
            this.masterpanel.Controls.Add(this.labelFirstCutDate);
            this.masterpanel.Controls.Add(this.labelNeededPerDay);
            this.masterpanel.Controls.Add(this.labelSewingLine);
            this.masterpanel.Controls.Add(this.labelPPICMR);
            this.masterpanel.Controls.Add(this.labelBuyerDeliveryDate);
            this.masterpanel.Controls.Add(this.labelSCIDeliveryDate);
            this.masterpanel.Controls.Add(this.labelSewingOfflineDate);
            this.masterpanel.Controls.Add(this.labelSewingInlineDate);
            this.masterpanel.Controls.Add(this.labelDestination);
            this.masterpanel.Controls.Add(this.labelBrand);
            this.masterpanel.Controls.Add(this.labelSeason);
            this.masterpanel.Controls.Add(this.labelStyleNo);
            this.masterpanel.Controls.Add(this.labelSPNo);
            this.masterpanel.Controls.Add(this.dateFirstCutDate);
            this.masterpanel.Controls.Add(this.dateBuyerDeliveryDate);
            this.masterpanel.Controls.Add(this.dateSCIDeliveryDate);
            this.masterpanel.Controls.Add(this.dateSewingOfflineDate);
            this.masterpanel.Controls.Add(this.dateSewingInlineDate);
            this.masterpanel.Size = new System.Drawing.Size(1000, 191);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateSewingInlineDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateSewingOfflineDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateSCIDeliveryDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateBuyerDeliveryDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateFirstCutDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSPNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelStyleNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSeason, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelBrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDestination, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSewingInlineDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSewingOfflineDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSCIDeliveryDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelBuyerDeliveryDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPPICMR, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSewingLine, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNeededPerDay, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFirstCutDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCutQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelOrderQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.displaySPNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayStyleNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displaySeason, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtcountryDestination, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtuserPPICMR, 0);
            this.masterpanel.Controls.SetChildIndex(this.numCutQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.numOrderQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.numNeedPerDay, 0);
            this.masterpanel.Controls.SetChildIndex(this.displaySewingLine, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 191);
            this.detailpanel.Size = new System.Drawing.Size(1000, 438);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(896, 156);
            this.gridicon.Visible = false;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(920, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1000, 438);
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
            this.detail.Size = new System.Drawing.Size(1000, 667);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1000, 629);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 629);
            this.detailbtm.Size = new System.Drawing.Size(1000, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1000, 667);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1008, 696);
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
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(14, 12);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(110, 23);
            this.labelSPNo.TabIndex = 1;
            this.labelSPNo.Text = "SP#";
            // 
            // labelStyleNo
            // 
            this.labelStyleNo.Location = new System.Drawing.Point(14, 44);
            this.labelStyleNo.Name = "labelStyleNo";
            this.labelStyleNo.Size = new System.Drawing.Size(110, 23);
            this.labelStyleNo.TabIndex = 2;
            this.labelStyleNo.Text = "Style#";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(14, 76);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(110, 23);
            this.labelSeason.TabIndex = 3;
            this.labelSeason.Text = "Season";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(14, 110);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(110, 23);
            this.labelBrand.TabIndex = 4;
            this.labelBrand.Text = "Brand";
            // 
            // labelDestination
            // 
            this.labelDestination.Location = new System.Drawing.Point(14, 144);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(110, 23);
            this.labelDestination.TabIndex = 5;
            this.labelDestination.Text = "Destination";
            // 
            // labelSewingInlineDate
            // 
            this.labelSewingInlineDate.Location = new System.Drawing.Point(318, 12);
            this.labelSewingInlineDate.Name = "labelSewingInlineDate";
            this.labelSewingInlineDate.Size = new System.Drawing.Size(130, 23);
            this.labelSewingInlineDate.TabIndex = 6;
            this.labelSewingInlineDate.Text = "Sewing Inline Date";
            // 
            // labelSewingOfflineDate
            // 
            this.labelSewingOfflineDate.Location = new System.Drawing.Point(318, 44);
            this.labelSewingOfflineDate.Name = "labelSewingOfflineDate";
            this.labelSewingOfflineDate.Size = new System.Drawing.Size(130, 23);
            this.labelSewingOfflineDate.TabIndex = 7;
            this.labelSewingOfflineDate.Text = "Sewing Offline Date";
            // 
            // labelSCIDeliveryDate
            // 
            this.labelSCIDeliveryDate.Location = new System.Drawing.Point(318, 76);
            this.labelSCIDeliveryDate.Name = "labelSCIDeliveryDate";
            this.labelSCIDeliveryDate.Size = new System.Drawing.Size(130, 23);
            this.labelSCIDeliveryDate.TabIndex = 8;
            this.labelSCIDeliveryDate.Text = "SCI Delivery Date";
            // 
            // labelBuyerDeliveryDate
            // 
            this.labelBuyerDeliveryDate.Location = new System.Drawing.Point(318, 110);
            this.labelBuyerDeliveryDate.Name = "labelBuyerDeliveryDate";
            this.labelBuyerDeliveryDate.Size = new System.Drawing.Size(130, 23);
            this.labelBuyerDeliveryDate.TabIndex = 9;
            this.labelBuyerDeliveryDate.Text = "Buyer Delivery Date";
            // 
            // labelPPICMR
            // 
            this.labelPPICMR.Location = new System.Drawing.Point(318, 144);
            this.labelPPICMR.Name = "labelPPICMR";
            this.labelPPICMR.Size = new System.Drawing.Size(68, 23);
            this.labelPPICMR.TabIndex = 10;
            this.labelPPICMR.Text = "PPIC MR";
            // 
            // labelSewingLine
            // 
            this.labelSewingLine.Location = new System.Drawing.Point(640, 12);
            this.labelSewingLine.Name = "labelSewingLine";
            this.labelSewingLine.Size = new System.Drawing.Size(130, 23);
            this.labelSewingLine.TabIndex = 11;
            this.labelSewingLine.Text = "Sewing Line#";
            // 
            // labelNeededPerDay
            // 
            this.labelNeededPerDay.Location = new System.Drawing.Point(640, 44);
            this.labelNeededPerDay.Name = "labelNeededPerDay";
            this.labelNeededPerDay.Size = new System.Drawing.Size(130, 23);
            this.labelNeededPerDay.TabIndex = 12;
            this.labelNeededPerDay.Text = "Needed Per Day";
            // 
            // labelFirstCutDate
            // 
            this.labelFirstCutDate.Location = new System.Drawing.Point(640, 76);
            this.labelFirstCutDate.Name = "labelFirstCutDate";
            this.labelFirstCutDate.Size = new System.Drawing.Size(130, 23);
            this.labelFirstCutDate.TabIndex = 13;
            this.labelFirstCutDate.Text = "First Cut Date";
            // 
            // labelCutQty
            // 
            this.labelCutQty.Location = new System.Drawing.Point(640, 110);
            this.labelCutQty.Name = "labelCutQty";
            this.labelCutQty.Size = new System.Drawing.Size(130, 23);
            this.labelCutQty.TabIndex = 14;
            this.labelCutQty.Text = "Cut Qty";
            // 
            // labelOrderQty
            // 
            this.labelOrderQty.Location = new System.Drawing.Point(640, 144);
            this.labelOrderQty.Name = "labelOrderQty";
            this.labelOrderQty.Size = new System.Drawing.Size(130, 23);
            this.labelOrderQty.TabIndex = 15;
            this.labelOrderQty.Text = "Order Qty";
            // 
            // displaySPNo
            // 
            this.displaySPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySPNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displaySPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySPNo.Location = new System.Drawing.Point(127, 12);
            this.displaySPNo.Name = "displaySPNo";
            this.displaySPNo.Size = new System.Drawing.Size(137, 23);
            this.displaySPNo.TabIndex = 0;
            // 
            // displayStyleNo
            // 
            this.displayStyleNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyleNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "styleid", true));
            this.displayStyleNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyleNo.Location = new System.Drawing.Point(127, 44);
            this.displayStyleNo.Name = "displayStyleNo";
            this.displayStyleNo.Size = new System.Drawing.Size(137, 23);
            this.displayStyleNo.TabIndex = 1;
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "seasonid", true));
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(127, 76);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(137, 23);
            this.displaySeason.TabIndex = 2;
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "brandid", true));
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(127, 110);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(137, 23);
            this.displayBrand.TabIndex = 3;
            // 
            // txtcountryDestination
            // 
            this.txtcountryDestination.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "dest", true));
            this.txtcountryDestination.DisplayBox1Binding = "";
            this.txtcountryDestination.Location = new System.Drawing.Point(127, 144);
            this.txtcountryDestination.Name = "txtcountryDestination";
            this.txtcountryDestination.Size = new System.Drawing.Size(178, 22);
            this.txtcountryDestination.TabIndex = 4;
            this.txtcountryDestination.TextBox1Binding = "";
            // 
            // dateSewingInlineDate
            // 
            this.dateSewingInlineDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "sewinline", true));
            this.dateSewingInlineDate.IsSupportEditMode = false;
            this.dateSewingInlineDate.Location = new System.Drawing.Point(451, 12);
            this.dateSewingInlineDate.Name = "dateSewingInlineDate";
            this.dateSewingInlineDate.ReadOnly = true;
            this.dateSewingInlineDate.Size = new System.Drawing.Size(130, 23);
            this.dateSewingInlineDate.TabIndex = 5;
            // 
            // dateSewingOfflineDate
            // 
            this.dateSewingOfflineDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "sewoffline", true));
            this.dateSewingOfflineDate.IsSupportEditMode = false;
            this.dateSewingOfflineDate.Location = new System.Drawing.Point(451, 44);
            this.dateSewingOfflineDate.Name = "dateSewingOfflineDate";
            this.dateSewingOfflineDate.ReadOnly = true;
            this.dateSewingOfflineDate.Size = new System.Drawing.Size(130, 23);
            this.dateSewingOfflineDate.TabIndex = 6;
            // 
            // dateSCIDeliveryDate
            // 
            this.dateSCIDeliveryDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "scidelivery", true));
            this.dateSCIDeliveryDate.IsSupportEditMode = false;
            this.dateSCIDeliveryDate.Location = new System.Drawing.Point(451, 76);
            this.dateSCIDeliveryDate.Name = "dateSCIDeliveryDate";
            this.dateSCIDeliveryDate.ReadOnly = true;
            this.dateSCIDeliveryDate.Size = new System.Drawing.Size(130, 23);
            this.dateSCIDeliveryDate.TabIndex = 7;
            // 
            // dateBuyerDeliveryDate
            // 
            this.dateBuyerDeliveryDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "buyerdelivery", true));
            this.dateBuyerDeliveryDate.IsSupportEditMode = false;
            this.dateBuyerDeliveryDate.Location = new System.Drawing.Point(451, 110);
            this.dateBuyerDeliveryDate.Name = "dateBuyerDeliveryDate";
            this.dateBuyerDeliveryDate.ReadOnly = true;
            this.dateBuyerDeliveryDate.Size = new System.Drawing.Size(130, 23);
            this.dateBuyerDeliveryDate.TabIndex = 8;
            // 
            // txtuserPPICMR
            // 
            this.txtuserPPICMR.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "mchandle", true));
            this.txtuserPPICMR.DisplayBox1Binding = "";
            this.txtuserPPICMR.Location = new System.Drawing.Point(389, 143);
            this.txtuserPPICMR.Name = "txtuserPPICMR";
            this.txtuserPPICMR.Size = new System.Drawing.Size(248, 23);
            this.txtuserPPICMR.TabIndex = 9;
            this.txtuserPPICMR.TextBox1Binding = "";
            // 
            // dateFirstCutDate
            // 
            this.dateFirstCutDate.IsSupportEditMode = false;
            this.dateFirstCutDate.Location = new System.Drawing.Point(773, 76);
            this.dateFirstCutDate.Name = "dateFirstCutDate";
            this.dateFirstCutDate.ReadOnly = true;
            this.dateFirstCutDate.Size = new System.Drawing.Size(130, 23);
            this.dateFirstCutDate.TabIndex = 12;
            // 
            // numCutQty
            // 
            this.numCutQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numCutQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numCutQty.IsSupportEditMode = false;
            this.numCutQty.Location = new System.Drawing.Point(773, 110);
            this.numCutQty.Name = "numCutQty";
            this.numCutQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCutQty.ReadOnly = true;
            this.numCutQty.Size = new System.Drawing.Size(100, 23);
            this.numCutQty.TabIndex = 13;
            this.numCutQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numOrderQty
            // 
            this.numOrderQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numOrderQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "qty", true));
            this.numOrderQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numOrderQty.IsSupportEditMode = false;
            this.numOrderQty.Location = new System.Drawing.Point(773, 143);
            this.numOrderQty.Name = "numOrderQty";
            this.numOrderQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numOrderQty.ReadOnly = true;
            this.numOrderQty.Size = new System.Drawing.Size(100, 23);
            this.numOrderQty.TabIndex = 14;
            this.numOrderQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numNeedPerDay
            // 
            this.numNeedPerDay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numNeedPerDay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numNeedPerDay.IsSupportEditMode = false;
            this.numNeedPerDay.Location = new System.Drawing.Point(773, 44);
            this.numNeedPerDay.Name = "numNeedPerDay";
            this.numNeedPerDay.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numNeedPerDay.ReadOnly = true;
            this.numNeedPerDay.Size = new System.Drawing.Size(100, 23);
            this.numNeedPerDay.TabIndex = 11;
            this.numNeedPerDay.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // displaySewingLine
            // 
            this.displaySewingLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySewingLine.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "sewline", true));
            this.displaySewingLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySewingLine.Location = new System.Drawing.Point(773, 12);
            this.displaySewingLine.Name = "displaySewingLine";
            this.displaySewingLine.Size = new System.Drawing.Size(137, 23);
            this.displaySewingLine.TabIndex = 10;
            // 
            // btnBatchApprove
            // 
            this.btnBatchApprove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBatchApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBatchApprove.Location = new System.Drawing.Point(869, 2);
            this.btnBatchApprove.Name = "btnBatchApprove";
            this.btnBatchApprove.Size = new System.Drawing.Size(132, 31);
            this.btnBatchApprove.TabIndex = 31;
            this.btnBatchApprove.Text = "Batch Approve";
            this.btnBatchApprove.UseVisualStyleBackColor = true;
            this.btnBatchApprove.Click += new System.EventHandler(this.BtnBatchApprove_Click);
            // 
            // P01
            // 
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.btnBatchApprove);
            this.DefaultOrder = "id";
            this.GridAlias = "order_tmscost";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "id";
            this.Name = "P01";
            this.SelectAllData = true;
            this.Text = "P01 Sub-process master list";
            this.UniqueExpress = "ID";
            this.WorkAlias = "orders";
            this.Controls.SetChildIndex(this.tabs, 0);
            this.Controls.SetChildIndex(this.btnBatchApprove, 0);
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

        private Win.UI.NumericBox numNeedPerDay;
        private Win.UI.NumericBox numOrderQty;
        private Win.UI.NumericBox numCutQty;
        private Win.UI.DateBox dateFirstCutDate;
        private Class.Txtuser txtuserPPICMR;
        private Win.UI.DateBox dateBuyerDeliveryDate;
        private Win.UI.DateBox dateSCIDeliveryDate;
        private Win.UI.DateBox dateSewingOfflineDate;
        private Win.UI.DateBox dateSewingInlineDate;
        private Class.Txtcountry txtcountryDestination;
        private Win.UI.DisplayBox displayBrand;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.DisplayBox displayStyleNo;
        private Win.UI.DisplayBox displaySPNo;
        private Win.UI.Label labelOrderQty;
        private Win.UI.Label labelCutQty;
        private Win.UI.Label labelFirstCutDate;
        private Win.UI.Label labelNeededPerDay;
        private Win.UI.Label labelSewingLine;
        private Win.UI.Label labelPPICMR;
        private Win.UI.Label labelBuyerDeliveryDate;
        private Win.UI.Label labelSCIDeliveryDate;
        private Win.UI.Label labelSewingOfflineDate;
        private Win.UI.Label labelSewingInlineDate;
        private Win.UI.Label labelDestination;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelStyleNo;
        private Win.UI.Label labelSPNo;
        private Win.UI.DisplayBox displaySewingLine;
        private Win.UI.Button btnBatchApprove;
    }
}
