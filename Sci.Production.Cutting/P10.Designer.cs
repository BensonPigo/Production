namespace Sci.Production.Cutting
{
    partial class P10
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
            this.labelDate = new Sci.Win.UI.Label();
            this.labelCutNo = new Sci.Win.UI.Label();
            this.labelLineNo = new Sci.Win.UI.Label();
            this.labelBeginBundleGroup = new Sci.Win.UI.Label();
            this.labelFabricCombo = new Sci.Win.UI.Label();
            this.labelSewCell = new Sci.Win.UI.Label();
            this.labelCutRef = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelQtyperBundleGroup = new Sci.Win.UI.Label();
            this.labelItem = new Sci.Win.UI.Label();
            this.labelNoofBundle = new Sci.Win.UI.Label();
            this.labelPOID = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelSize = new Sci.Win.UI.Label();
            this.labelSizeRatio = new Sci.Win.UI.Label();
            this.labelArticle = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelPrintDate = new Sci.Win.UI.Label();
            this.labelEstCutDate = new Sci.Win.UI.Label();
            this.labelColor = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.displayPOID = new Sci.Win.UI.DisplayBox();
            this.displayStyle = new Sci.Win.UI.DisplayBox();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.displayM = new Sci.Win.UI.DisplayBox();
            this.numNoofBundle = new Sci.Win.UI.NumericBox();
            this.numBeginBundleGroup = new Sci.Win.UI.NumericBox();
            this.displayPrintDate = new Sci.Win.UI.DisplayBox();
            this.displayEstCutDate = new Sci.Win.UI.DisplayBox();
            this.numQtyperBundleGroup = new Sci.Win.UI.NumericBox();
            this.txtCutRef = new Sci.Win.UI.TextBox();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.txtItem = new Sci.Win.UI.TextBox();
            this.txtSewCell = new Sci.Win.UI.TextBox();
            this.numCutNo = new Sci.Win.UI.NumericBox();
            this.dateDate = new Sci.Win.UI.DateBox();
            this.txtFabricCombo = new Sci.Win.UI.TextBox();
            this.txtSize = new Sci.Win.UI.TextBox();
            this.txtSizeRatio = new Sci.Win.UI.TextBox();
            this.txtArticle = new Sci.Win.UI.TextBox();
            this.txtColorID = new Sci.Win.UI.TextBox();
            this.btnGenerate = new Sci.Win.UI.Button();
            this.btnGarmentList = new Sci.Win.UI.Button();
            this.txtLineNo = new Sci.Win.UI.TextBox();
            this.labelFabPanelCode = new Sci.Win.UI.Label();
            this.txtFabricPanelCode = new Sci.Win.UI.TextBox();
            this.ckIsEXCESS = new Sci.Win.UI.CheckBox();
            this.btnBatchDelete = new Sci.Win.UI.Button();
            this.label1 = new Sci.Win.UI.Label();
            this.dispFabricKind = new Sci.Win.UI.DisplayBox();
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
            this.masterpanel.Controls.Add(this.dispFabricKind);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Controls.Add(this.ckIsEXCESS);
            this.masterpanel.Controls.Add(this.txtFabricPanelCode);
            this.masterpanel.Controls.Add(this.labelFabPanelCode);
            this.masterpanel.Controls.Add(this.txtLineNo);
            this.masterpanel.Controls.Add(this.btnGenerate);
            this.masterpanel.Controls.Add(this.txtColorID);
            this.masterpanel.Controls.Add(this.txtArticle);
            this.masterpanel.Controls.Add(this.txtSizeRatio);
            this.masterpanel.Controls.Add(this.txtSize);
            this.masterpanel.Controls.Add(this.txtFabricCombo);
            this.masterpanel.Controls.Add(this.numCutNo);
            this.masterpanel.Controls.Add(this.txtSewCell);
            this.masterpanel.Controls.Add(this.txtItem);
            this.masterpanel.Controls.Add(this.txtSPNo);
            this.masterpanel.Controls.Add(this.txtCutRef);
            this.masterpanel.Controls.Add(this.numQtyperBundleGroup);
            this.masterpanel.Controls.Add(this.displayEstCutDate);
            this.masterpanel.Controls.Add(this.displayPrintDate);
            this.masterpanel.Controls.Add(this.numBeginBundleGroup);
            this.masterpanel.Controls.Add(this.numNoofBundle);
            this.masterpanel.Controls.Add(this.displayM);
            this.masterpanel.Controls.Add(this.displaySeason);
            this.masterpanel.Controls.Add(this.displayStyle);
            this.masterpanel.Controls.Add(this.displayPOID);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.labelColor);
            this.masterpanel.Controls.Add(this.labelEstCutDate);
            this.masterpanel.Controls.Add(this.labelPrintDate);
            this.masterpanel.Controls.Add(this.labelSeason);
            this.masterpanel.Controls.Add(this.labelM);
            this.masterpanel.Controls.Add(this.labelArticle);
            this.masterpanel.Controls.Add(this.labelSizeRatio);
            this.masterpanel.Controls.Add(this.labelSize);
            this.masterpanel.Controls.Add(this.labelStyle);
            this.masterpanel.Controls.Add(this.labelPOID);
            this.masterpanel.Controls.Add(this.labelNoofBundle);
            this.masterpanel.Controls.Add(this.labelItem);
            this.masterpanel.Controls.Add(this.labelQtyperBundleGroup);
            this.masterpanel.Controls.Add(this.labelSPNo);
            this.masterpanel.Controls.Add(this.labelCutRef);
            this.masterpanel.Controls.Add(this.labelSewCell);
            this.masterpanel.Controls.Add(this.labelFabricCombo);
            this.masterpanel.Controls.Add(this.labelBeginBundleGroup);
            this.masterpanel.Controls.Add(this.labelLineNo);
            this.masterpanel.Controls.Add(this.labelCutNo);
            this.masterpanel.Controls.Add(this.labelDate);
            this.masterpanel.Controls.Add(this.labelID);
            this.masterpanel.Controls.Add(this.dateDate);
            this.masterpanel.Size = new System.Drawing.Size(991, 224);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCutNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelLineNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelBeginBundleGroup, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFabricCombo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSewCell, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCutRef, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSPNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelQtyperBundleGroup, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelItem, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNoofBundle, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPOID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelStyle, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSize, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSizeRatio, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelArticle, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelM, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSeason, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPrintDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelEstCutDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelColor, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayPOID, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayStyle, 0);
            this.masterpanel.Controls.SetChildIndex(this.displaySeason, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayM, 0);
            this.masterpanel.Controls.SetChildIndex(this.numNoofBundle, 0);
            this.masterpanel.Controls.SetChildIndex(this.numBeginBundleGroup, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayPrintDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayEstCutDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.numQtyperBundleGroup, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtCutRef, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSPNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtItem, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSewCell, 0);
            this.masterpanel.Controls.SetChildIndex(this.numCutNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtFabricCombo, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSize, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSizeRatio, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtArticle, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtColorID, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnGenerate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLineNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFabPanelCode, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtFabricPanelCode, 0);
            this.masterpanel.Controls.SetChildIndex(this.ckIsEXCESS, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.dispFabricKind, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 224);
            this.detailpanel.Size = new System.Drawing.Size(991, 240);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(5, 189);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(908, -1);
            this.refresh.TabIndex = 3;
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(991, 240);
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
            this.detail.Size = new System.Drawing.Size(991, 502);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(991, 464);
            // 
            // detailbtm
            // 
            this.detailbtm.Controls.Add(this.btnGarmentList);
            this.detailbtm.Location = new System.Drawing.Point(0, 464);
            this.detailbtm.Size = new System.Drawing.Size(991, 38);
            this.detailbtm.Controls.SetChildIndex(this.lbleditby, 0);
            this.detailbtm.Controls.SetChildIndex(this.lblcreateby, 0);
            this.detailbtm.Controls.SetChildIndex(this.editby, 0);
            this.detailbtm.Controls.SetChildIndex(this.createby, 0);
            this.detailbtm.Controls.SetChildIndex(this.refresh, 0);
            this.detailbtm.Controls.SetChildIndex(this.btnGarmentList, 0);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(991, 502);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(999, 531);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(337, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(461, 7);
            this.editby.Size = new System.Drawing.Size(331, 23);
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(411, 13);
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(18, 13);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 1;
            this.labelID.Text = "ID";
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(19, 47);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(75, 23);
            this.labelDate.TabIndex = 2;
            this.labelDate.Text = "Date";
            // 
            // labelCutNo
            // 
            this.labelCutNo.Location = new System.Drawing.Point(19, 81);
            this.labelCutNo.Name = "labelCutNo";
            this.labelCutNo.Size = new System.Drawing.Size(75, 23);
            this.labelCutNo.TabIndex = 2;
            this.labelCutNo.Text = "Cut#";
            // 
            // labelLineNo
            // 
            this.labelLineNo.Location = new System.Drawing.Point(441, 149);
            this.labelLineNo.Name = "labelLineNo";
            this.labelLineNo.Size = new System.Drawing.Size(75, 23);
            this.labelLineNo.TabIndex = 3;
            this.labelLineNo.Text = "Line#";
            // 
            // labelBeginBundleGroup
            // 
            this.labelBeginBundleGroup.Location = new System.Drawing.Point(19, 183);
            this.labelBeginBundleGroup.Name = "labelBeginBundleGroup";
            this.labelBeginBundleGroup.Size = new System.Drawing.Size(131, 23);
            this.labelBeginBundleGroup.TabIndex = 4;
            this.labelBeginBundleGroup.Text = "Begin Bundle Group";
            // 
            // labelFabricCombo
            // 
            this.labelFabricCombo.Location = new System.Drawing.Point(236, 81);
            this.labelFabricCombo.Name = "labelFabricCombo";
            this.labelFabricCombo.Size = new System.Drawing.Size(95, 23);
            this.labelFabricCombo.TabIndex = 5;
            this.labelFabricCombo.Text = "Fabric Combo";
            // 
            // labelSewCell
            // 
            this.labelSewCell.Location = new System.Drawing.Point(441, 183);
            this.labelSewCell.Name = "labelSewCell";
            this.labelSewCell.Size = new System.Drawing.Size(75, 23);
            this.labelSewCell.TabIndex = 7;
            this.labelSewCell.Text = "Sew Cell";
            // 
            // labelCutRef
            // 
            this.labelCutRef.Location = new System.Drawing.Point(236, 13);
            this.labelCutRef.Name = "labelCutRef";
            this.labelCutRef.Size = new System.Drawing.Size(75, 23);
            this.labelCutRef.TabIndex = 8;
            this.labelCutRef.Text = "Cut Ref#";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(441, 13);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(75, 23);
            this.labelSPNo.TabIndex = 9;
            this.labelSPNo.Text = "SP#";
            // 
            // labelQtyperBundleGroup
            // 
            this.labelQtyperBundleGroup.Location = new System.Drawing.Point(661, 149);
            this.labelQtyperBundleGroup.Name = "labelQtyperBundleGroup";
            this.labelQtyperBundleGroup.Size = new System.Drawing.Size(139, 23);
            this.labelQtyperBundleGroup.TabIndex = 10;
            this.labelQtyperBundleGroup.Text = "Qty per Bundle Group";
            // 
            // labelItem
            // 
            this.labelItem.Location = new System.Drawing.Point(441, 115);
            this.labelItem.Name = "labelItem";
            this.labelItem.Size = new System.Drawing.Size(75, 23);
            this.labelItem.TabIndex = 11;
            this.labelItem.Text = "Item";
            // 
            // labelNoofBundle
            // 
            this.labelNoofBundle.Location = new System.Drawing.Point(236, 183);
            this.labelNoofBundle.Name = "labelNoofBundle";
            this.labelNoofBundle.Size = new System.Drawing.Size(88, 23);
            this.labelNoofBundle.TabIndex = 12;
            this.labelNoofBundle.Text = "No of Bundle";
            // 
            // labelPOID
            // 
            this.labelPOID.Location = new System.Drawing.Point(661, 13);
            this.labelPOID.Name = "labelPOID";
            this.labelPOID.Size = new System.Drawing.Size(75, 23);
            this.labelPOID.TabIndex = 13;
            this.labelPOID.Text = "POID";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(441, 47);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(75, 23);
            this.labelStyle.TabIndex = 14;
            this.labelStyle.Text = "Style";
            // 
            // labelSize
            // 
            this.labelSize.Location = new System.Drawing.Point(19, 149);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(75, 23);
            this.labelSize.TabIndex = 15;
            this.labelSize.Text = "Size";
            // 
            // labelSizeRatio
            // 
            this.labelSizeRatio.Location = new System.Drawing.Point(236, 149);
            this.labelSizeRatio.Name = "labelSizeRatio";
            this.labelSizeRatio.Size = new System.Drawing.Size(75, 23);
            this.labelSizeRatio.TabIndex = 16;
            this.labelSizeRatio.Text = "Size Ratio";
            // 
            // labelArticle
            // 
            this.labelArticle.Location = new System.Drawing.Point(19, 115);
            this.labelArticle.Name = "labelArticle";
            this.labelArticle.Size = new System.Drawing.Size(75, 23);
            this.labelArticle.TabIndex = 17;
            this.labelArticle.Text = "Article";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(236, 47);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(75, 23);
            this.labelM.TabIndex = 18;
            this.labelM.Text = "M";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(661, 47);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(75, 23);
            this.labelSeason.TabIndex = 19;
            this.labelSeason.Text = "Season";
            // 
            // labelPrintDate
            // 
            this.labelPrintDate.Location = new System.Drawing.Point(661, 81);
            this.labelPrintDate.Name = "labelPrintDate";
            this.labelPrintDate.Size = new System.Drawing.Size(75, 23);
            this.labelPrintDate.TabIndex = 20;
            this.labelPrintDate.Text = "Print Date";
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Location = new System.Drawing.Point(661, 115);
            this.labelEstCutDate.Name = "labelEstCutDate";
            this.labelEstCutDate.Size = new System.Drawing.Size(106, 23);
            this.labelEstCutDate.TabIndex = 21;
            this.labelEstCutDate.Text = "Est. Cut Date";
            // 
            // labelColor
            // 
            this.labelColor.Location = new System.Drawing.Point(236, 115);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(75, 23);
            this.labelColor.TabIndex = 22;
            this.labelColor.Text = "Color";
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(96, 13);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(120, 23);
            this.displayID.TabIndex = 0;
            // 
            // displayPOID
            // 
            this.displayPOID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPOID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "POID", true));
            this.displayPOID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPOID.Location = new System.Drawing.Point(739, 13);
            this.displayPOID.Name = "displayPOID";
            this.displayPOID.Size = new System.Drawing.Size(108, 23);
            this.displayPOID.TabIndex = 3;
            // 
            // displayStyle
            // 
            this.displayStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyle.Location = new System.Drawing.Point(519, 47);
            this.displayStyle.Name = "displayStyle";
            this.displayStyle.Size = new System.Drawing.Size(133, 23);
            this.displayStyle.TabIndex = 6;
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(739, 47);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(53, 23);
            this.displaySeason.TabIndex = 7;
            // 
            // displayM
            // 
            this.displayM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayM.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "mDivisionid", true));
            this.displayM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayM.Location = new System.Drawing.Point(314, 47);
            this.displayM.Name = "displayM";
            this.displayM.Size = new System.Drawing.Size(53, 23);
            this.displayM.TabIndex = 5;
            // 
            // numNoofBundle
            // 
            this.numNoofBundle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numNoofBundle.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "qty", true));
            this.numNoofBundle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numNoofBundle.IsSupportEditMode = false;
            this.numNoofBundle.Location = new System.Drawing.Point(327, 183);
            this.numNoofBundle.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numNoofBundle.Name = "numNoofBundle";
            this.numNoofBundle.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numNoofBundle.ReadOnly = true;
            this.numNoofBundle.Size = new System.Drawing.Size(43, 23);
            this.numNoofBundle.TabIndex = 21;
            this.numNoofBundle.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numBeginBundleGroup
            // 
            this.numBeginBundleGroup.BackColor = System.Drawing.Color.White;
            this.numBeginBundleGroup.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "startno", true));
            this.numBeginBundleGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numBeginBundleGroup.Location = new System.Drawing.Point(153, 183);
            this.numBeginBundleGroup.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBeginBundleGroup.Name = "numBeginBundleGroup";
            this.numBeginBundleGroup.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBeginBundleGroup.Size = new System.Drawing.Size(53, 23);
            this.numBeginBundleGroup.TabIndex = 20;
            this.numBeginBundleGroup.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBeginBundleGroup.Validated += new System.EventHandler(this.numBeginBundleGroup_Validated);
            // 
            // displayPrintDate
            // 
            this.displayPrintDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPrintDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "printdate", true));
            this.displayPrintDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPrintDate.Location = new System.Drawing.Point(739, 81);
            this.displayPrintDate.Name = "displayPrintDate";
            this.displayPrintDate.Size = new System.Drawing.Size(133, 23);
            this.displayPrintDate.TabIndex = 11;
            // 
            // displayEstCutDate
            // 
            this.displayEstCutDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayEstCutDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayEstCutDate.Location = new System.Drawing.Point(770, 115);
            this.displayEstCutDate.Name = "displayEstCutDate";
            this.displayEstCutDate.Size = new System.Drawing.Size(84, 23);
            this.displayEstCutDate.TabIndex = 15;
            // 
            // numQtyperBundleGroup
            // 
            this.numQtyperBundleGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numQtyperBundleGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numQtyperBundleGroup.IsSupportEditMode = false;
            this.numQtyperBundleGroup.Location = new System.Drawing.Point(803, 149);
            this.numQtyperBundleGroup.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numQtyperBundleGroup.Name = "numQtyperBundleGroup";
            this.numQtyperBundleGroup.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numQtyperBundleGroup.ReadOnly = true;
            this.numQtyperBundleGroup.Size = new System.Drawing.Size(53, 23);
            this.numQtyperBundleGroup.TabIndex = 19;
            this.numQtyperBundleGroup.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtCutRef
            // 
            this.txtCutRef.BackColor = System.Drawing.Color.White;
            this.txtCutRef.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "cutref", true));
            this.txtCutRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRef.Location = new System.Drawing.Point(314, 13);
            this.txtCutRef.Name = "txtCutRef";
            this.txtCutRef.Size = new System.Drawing.Size(73, 23);
            this.txtCutRef.TabIndex = 1;
            this.txtCutRef.Validating += new System.ComponentModel.CancelEventHandler(this.txtCutRef_Validating);
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Orderid", true));
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(519, 13);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(108, 23);
            this.txtSPNo.TabIndex = 2;
            this.txtSPNo.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtSPNo_PopUp);
            this.txtSPNo.Validating += new System.ComponentModel.CancelEventHandler(this.txtSPNo_Validating);
            // 
            // txtItem
            // 
            this.txtItem.BackColor = System.Drawing.Color.White;
            this.txtItem.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "item", true));
            this.txtItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtItem.Location = new System.Drawing.Point(519, 115);
            this.txtItem.Name = "txtItem";
            this.txtItem.Size = new System.Drawing.Size(108, 23);
            this.txtItem.TabIndex = 14;
            // 
            // txtSewCell
            // 
            this.txtSewCell.BackColor = System.Drawing.Color.White;
            this.txtSewCell.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SewingCell", true));
            this.txtSewCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSewCell.Location = new System.Drawing.Point(519, 183);
            this.txtSewCell.Name = "txtSewCell";
            this.txtSewCell.Size = new System.Drawing.Size(56, 23);
            this.txtSewCell.TabIndex = 22;
            // 
            // numCutNo
            // 
            this.numCutNo.BackColor = System.Drawing.Color.White;
            this.numCutNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "cutno", true));
            this.numCutNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numCutNo.Location = new System.Drawing.Point(97, 81);
            this.numCutNo.Name = "numCutNo";
            this.numCutNo.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCutNo.Size = new System.Drawing.Size(53, 23);
            this.numCutNo.TabIndex = 8;
            this.numCutNo.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // dateDate
            // 
            this.dateDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "cdate", true));
            this.dateDate.Location = new System.Drawing.Point(97, 47);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(130, 23);
            this.dateDate.TabIndex = 4;
            // 
            // txtFabricCombo
            // 
            this.txtFabricCombo.BackColor = System.Drawing.Color.White;
            this.txtFabricCombo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "PatternPanel", true));
            this.txtFabricCombo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFabricCombo.Location = new System.Drawing.Point(334, 81);
            this.txtFabricCombo.Name = "txtFabricCombo";
            this.txtFabricCombo.Size = new System.Drawing.Size(53, 23);
            this.txtFabricCombo.TabIndex = 9;
            this.txtFabricCombo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtFabricCombo_Validating);
            // 
            // txtSize
            // 
            this.txtSize.BackColor = System.Drawing.Color.White;
            this.txtSize.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "sizecode", true));
            this.txtSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSize.Location = new System.Drawing.Point(97, 149);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(93, 23);
            this.txtSize.TabIndex = 16;
            // 
            // txtSizeRatio
            // 
            this.txtSizeRatio.BackColor = System.Drawing.Color.White;
            this.txtSizeRatio.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Ratio", true));
            this.txtSizeRatio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSizeRatio.Location = new System.Drawing.Point(314, 149);
            this.txtSizeRatio.Name = "txtSizeRatio";
            this.txtSizeRatio.Size = new System.Drawing.Size(93, 23);
            this.txtSizeRatio.TabIndex = 17;
            // 
            // txtArticle
            // 
            this.txtArticle.BackColor = System.Drawing.Color.White;
            this.txtArticle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "article", true));
            this.txtArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtArticle.Location = new System.Drawing.Point(97, 115);
            this.txtArticle.Name = "txtArticle";
            this.txtArticle.Size = new System.Drawing.Size(73, 23);
            this.txtArticle.TabIndex = 12;
            this.txtArticle.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtArticle_PopUp);
            this.txtArticle.Validating += new System.ComponentModel.CancelEventHandler(this.txtArticle_Validating);
            // 
            // txtColorID
            // 
            this.txtColorID.BackColor = System.Drawing.Color.White;
            this.txtColorID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "colorid", true));
            this.txtColorID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtColorID.Location = new System.Drawing.Point(314, 115);
            this.txtColorID.Name = "txtColorID";
            this.txtColorID.Size = new System.Drawing.Size(73, 23);
            this.txtColorID.TabIndex = 13;
            // 
            // btnGenerate
            // 
            this.btnGenerate.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnGenerate.Location = new System.Drawing.Point(903, 183);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(80, 30);
            this.btnGenerate.TabIndex = 23;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnGarmentList
            // 
            this.btnGarmentList.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnGarmentList.Location = new System.Drawing.Point(795, 3);
            this.btnGarmentList.Name = "btnGarmentList";
            this.btnGarmentList.Size = new System.Drawing.Size(111, 30);
            this.btnGarmentList.TabIndex = 2;
            this.btnGarmentList.Text = "Garment List";
            this.btnGarmentList.UseVisualStyleBackColor = true;
            this.btnGarmentList.Click += new System.EventHandler(this.btnGarmentList_Click);
            // 
            // txtLineNo
            // 
            this.txtLineNo.BackColor = System.Drawing.Color.White;
            this.txtLineNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "sewinglineid", true));
            this.txtLineNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLineNo.Location = new System.Drawing.Point(520, 149);
            this.txtLineNo.Name = "txtLineNo";
            this.txtLineNo.Size = new System.Drawing.Size(73, 23);
            this.txtLineNo.TabIndex = 18;
            this.txtLineNo.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtLineNo_PopUp);
            this.txtLineNo.Validating += new System.ComponentModel.CancelEventHandler(this.txtLineNo_Validating);
            // 
            // labelFabPanelCode
            // 
            this.labelFabPanelCode.Location = new System.Drawing.Point(441, 81);
            this.labelFabPanelCode.Name = "labelFabPanelCode";
            this.labelFabPanelCode.Size = new System.Drawing.Size(110, 23);
            this.labelFabPanelCode.TabIndex = 48;
            this.labelFabPanelCode.Text = "Fab_Panel Code";
            // 
            // txtFabricPanelCode
            // 
            this.txtFabricPanelCode.BackColor = System.Drawing.Color.White;
            this.txtFabricPanelCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FabricPanelCode", true));
            this.txtFabricPanelCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFabricPanelCode.Location = new System.Drawing.Point(554, 81);
            this.txtFabricPanelCode.Name = "txtFabricPanelCode";
            this.txtFabricPanelCode.Size = new System.Drawing.Size(53, 23);
            this.txtFabricPanelCode.TabIndex = 10;
            // 
            // ckIsEXCESS
            // 
            this.ckIsEXCESS.AutoSize = true;
            this.ckIsEXCESS.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsEXCESS", true));
            this.ckIsEXCESS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ckIsEXCESS.Location = new System.Drawing.Point(892, 151);
            this.ckIsEXCESS.Name = "ckIsEXCESS";
            this.ckIsEXCESS.Size = new System.Drawing.Size(91, 21);
            this.ckIsEXCESS.TabIndex = 49;
            this.ckIsEXCESS.Text = "IsEXCESS";
            this.ckIsEXCESS.UseVisualStyleBackColor = true;
            // 
            // btnBatchDelete
            // 
            this.btnBatchDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBatchDelete.Location = new System.Drawing.Point(876, 3);
            this.btnBatchDelete.Name = "btnBatchDelete";
            this.btnBatchDelete.Size = new System.Drawing.Size(116, 30);
            this.btnBatchDelete.TabIndex = 24;
            this.btnBatchDelete.Text = "Batch Delete";
            this.btnBatchDelete.UseVisualStyleBackColor = true;
            this.btnBatchDelete.Click += new System.EventHandler(this.BtnBatchDelete_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(661, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 50;
            this.label1.Text = "Fabric Kind";
            // 
            // dispFabricKind
            // 
            this.dispFabricKind.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispFabricKind.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispFabricKind.Location = new System.Drawing.Point(739, 183);
            this.dispFabricKind.Name = "dispFabricKind";
            this.dispFabricKind.Size = new System.Drawing.Size(133, 23);
            this.dispFabricKind.TabIndex = 51;
            // 
            // P10
            // 
            this.ClientSize = new System.Drawing.Size(999, 564);
            this.Controls.Add(this.btnBatchDelete);
            this.DefaultControl = "txtCutRef";
            this.DefaultControlForEdit = "txtCutRef";
            this.DefaultDetailOrder = "BundleGroup";
            this.DefaultOrder = "ID";
            this.ExpressQuery = true;
            this.GridAlias = "Bundle_Detail";
            this.GridNew = 0;
            this.IsGridIconVisible = false;
            this.KeyField1 = "id";
            this.Name = "P10";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P10.Bundle Card";
            this.WorkAlias = "Bundle";
            this.Controls.SetChildIndex(this.tabs, 0);
            this.Controls.SetChildIndex(this.btnBatchDelete, 0);
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

        private Win.UI.Label labelFabricCombo;
        private Win.UI.Label labelBeginBundleGroup;
        private Win.UI.Label labelLineNo;
        private Win.UI.Label labelCutNo;
        private Win.UI.Label labelDate;
        private Win.UI.Label labelID;
        private Win.UI.DisplayBox displayEstCutDate;
        private Win.UI.DisplayBox displayPrintDate;
        private Win.UI.NumericBox numBeginBundleGroup;
        private Win.UI.NumericBox numNoofBundle;
        private Win.UI.DisplayBox displayM;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.DisplayBox displayStyle;
        private Win.UI.DisplayBox displayPOID;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label labelColor;
        private Win.UI.Label labelEstCutDate;
        private Win.UI.Label labelPrintDate;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelM;
        private Win.UI.Label labelArticle;
        private Win.UI.Label labelSizeRatio;
        private Win.UI.Label labelSize;
        private Win.UI.Label labelStyle;
        private Win.UI.Label labelPOID;
        private Win.UI.Label labelNoofBundle;
        private Win.UI.Label labelItem;
        private Win.UI.Label labelQtyperBundleGroup;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelCutRef;
        private Win.UI.Label labelSewCell;
        private Win.UI.TextBox txtColorID;
        private Win.UI.TextBox txtArticle;
        private Win.UI.TextBox txtSizeRatio;
        private Win.UI.TextBox txtSize;
        private Win.UI.TextBox txtFabricCombo;
        private Win.UI.DateBox dateDate;
        private Win.UI.NumericBox numCutNo;
        private Win.UI.TextBox txtSewCell;
        private Win.UI.TextBox txtItem;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.TextBox txtCutRef;
        private Win.UI.NumericBox numQtyperBundleGroup;
        private Win.UI.Button btnGenerate;
        private Win.UI.Button btnGarmentList;
        private Win.UI.TextBox txtLineNo;
        private Win.UI.TextBox txtFabricPanelCode;
        private Win.UI.Label labelFabPanelCode;
        private Win.UI.CheckBox ckIsEXCESS;
        private Win.UI.Button btnBatchDelete;
        private Win.UI.DisplayBox dispFabricKind;
        private Win.UI.Label label1;
    }
}
