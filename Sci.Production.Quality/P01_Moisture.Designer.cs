namespace Sci.Production.Quality
{
    partial class P01_Moisture
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
            this.displayBrand = new Sci.Win.UI.DisplayBox();
            this.displayStyle = new Sci.Win.UI.DisplayBox();
            this.displaySP = new Sci.Win.UI.DisplayBox();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.displayWKNo = new Sci.Win.UI.DisplayBox();
            this.labelWKNo = new Sci.Win.UI.Label();
            this.displaySEQ = new Sci.Win.UI.DisplayBox();
            this.labelSEQ = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelRefno = new Sci.Win.UI.Label();
            this.displaySCIRefno = new Sci.Win.UI.DisplayBox();
            this.displaySCIRefno1 = new Sci.Win.UI.DisplayBox();
            this.labelSCIRefno = new Sci.Win.UI.Label();
            this.displayRefno = new Sci.Win.UI.DisplayBox();
            this.displayColor = new Sci.Win.UI.DisplayBox();
            this.labelColor = new Sci.Win.UI.Label();
            this.labelArriveQty = new Sci.Win.UI.Label();
            this.displayArriveQty = new Sci.Win.UI.DisplayBox();
            this.labelArriveWHDate = new Sci.Win.UI.Label();
            this.labelLastInspectionDate = new Sci.Win.UI.Label();
            this.labelResult = new Sci.Win.UI.Label();
            this.checkNonMoisture = new Sci.Win.UI.CheckBox();
            this.labelApprover = new Sci.Win.UI.Label();
            this.displayResult = new Sci.Win.UI.DisplayBox();
            this.dateArriveWHDate = new Sci.Win.UI.DateBox();
            this.dateBox1 = new Sci.Win.UI.DateBox();
            this.dateLastInspectionDate = new Sci.Win.UI.DateBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtsupplier = new Sci.Production.Class.Txtsupplier();
            this.cbmMaterialCompositionItem = new Sci.Win.UI.ComboBox();
            this.cbmMaterialCompositionGrp = new Sci.Win.UI.ComboBox();
            this.displayMoistureStandard = new Sci.Win.UI.DisplayBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 693);
            this.btmcont.Size = new System.Drawing.Size(1008, 40);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 180);
            this.gridcont.Size = new System.Drawing.Size(984, 503);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(918, 5);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(838, 5);
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(144, 93);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(120, 21);
            this.displayBrand.TabIndex = 103;
            // 
            // displayStyle
            // 
            this.displayStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyle.Location = new System.Drawing.Point(144, 64);
            this.displayStyle.Name = "displayStyle";
            this.displayStyle.Size = new System.Drawing.Size(120, 21);
            this.displayStyle.TabIndex = 102;
            // 
            // displaySP
            // 
            this.displaySP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displaySP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySP.Location = new System.Drawing.Point(144, 8);
            this.displaySP.Name = "displaySP";
            this.displaySP.Size = new System.Drawing.Size(120, 21);
            this.displaySP.TabIndex = 101;
            // 
            // labelBrand
            // 
            this.labelBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelBrand.Location = new System.Drawing.Point(15, 93);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(126, 23);
            this.labelBrand.TabIndex = 100;
            this.labelBrand.Text = "Brand";
            // 
            // labelStyle
            // 
            this.labelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelStyle.Location = new System.Drawing.Point(15, 64);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(126, 23);
            this.labelStyle.TabIndex = 99;
            this.labelStyle.Text = "Style";
            // 
            // labelSP
            // 
            this.labelSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelSP.Location = new System.Drawing.Point(15, 8);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(126, 23);
            this.labelSP.TabIndex = 98;
            this.labelSP.Text = "SP#";
            // 
            // displayWKNo
            // 
            this.displayWKNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayWKNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayWKNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayWKNo.Location = new System.Drawing.Point(144, 36);
            this.displayWKNo.Name = "displayWKNo";
            this.displayWKNo.Size = new System.Drawing.Size(120, 21);
            this.displayWKNo.TabIndex = 105;
            // 
            // labelWKNo
            // 
            this.labelWKNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelWKNo.Location = new System.Drawing.Point(15, 36);
            this.labelWKNo.Name = "labelWKNo";
            this.labelWKNo.Size = new System.Drawing.Size(126, 23);
            this.labelWKNo.TabIndex = 104;
            this.labelWKNo.Text = "WKNo";
            // 
            // displaySEQ
            // 
            this.displaySEQ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySEQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displaySEQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySEQ.Location = new System.Drawing.Point(356, 8);
            this.displaySEQ.Name = "displaySEQ";
            this.displaySEQ.Size = new System.Drawing.Size(62, 21);
            this.displaySEQ.TabIndex = 107;
            // 
            // labelSEQ
            // 
            this.labelSEQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelSEQ.Location = new System.Drawing.Point(278, 7);
            this.labelSEQ.Name = "labelSEQ";
            this.labelSEQ.Size = new System.Drawing.Size(75, 23);
            this.labelSEQ.TabIndex = 106;
            this.labelSEQ.Text = "SEQ";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelSupplier.Location = new System.Drawing.Point(278, 35);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(75, 23);
            this.labelSupplier.TabIndex = 108;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelRefno
            // 
            this.labelRefno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelRefno.Location = new System.Drawing.Point(278, 63);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(75, 23);
            this.labelRefno.TabIndex = 110;
            this.labelRefno.Text = "Refno";
            // 
            // displaySCIRefno
            // 
            this.displaySCIRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySCIRefno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displaySCIRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySCIRefno.Location = new System.Drawing.Point(356, 93);
            this.displaySCIRefno.Name = "displaySCIRefno";
            this.displaySCIRefno.Size = new System.Drawing.Size(186, 21);
            this.displaySCIRefno.TabIndex = 111;
            // 
            // displaySCIRefno1
            // 
            this.displaySCIRefno1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySCIRefno1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displaySCIRefno1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySCIRefno1.Location = new System.Drawing.Point(545, 92);
            this.displaySCIRefno1.Name = "displaySCIRefno1";
            this.displaySCIRefno1.Size = new System.Drawing.Size(293, 21);
            this.displaySCIRefno1.TabIndex = 112;
            // 
            // labelSCIRefno
            // 
            this.labelSCIRefno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelSCIRefno.Location = new System.Drawing.Point(278, 92);
            this.labelSCIRefno.Name = "labelSCIRefno";
            this.labelSCIRefno.Size = new System.Drawing.Size(75, 23);
            this.labelSCIRefno.TabIndex = 113;
            this.labelSCIRefno.Text = "SCI Refno";
            // 
            // displayRefno
            // 
            this.displayRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRefno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRefno.Location = new System.Drawing.Point(356, 63);
            this.displayRefno.Name = "displayRefno";
            this.displayRefno.Size = new System.Drawing.Size(150, 21);
            this.displayRefno.TabIndex = 114;
            // 
            // displayColor
            // 
            this.displayColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayColor.Location = new System.Drawing.Point(602, 9);
            this.displayColor.Name = "displayColor";
            this.displayColor.Size = new System.Drawing.Size(103, 21);
            this.displayColor.TabIndex = 115;
            // 
            // labelColor
            // 
            this.labelColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelColor.Location = new System.Drawing.Point(524, 7);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(75, 23);
            this.labelColor.TabIndex = 116;
            this.labelColor.Text = "Color";
            // 
            // labelArriveQty
            // 
            this.labelArriveQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelArriveQty.Location = new System.Drawing.Point(524, 35);
            this.labelArriveQty.Name = "labelArriveQty";
            this.labelArriveQty.Size = new System.Drawing.Size(75, 23);
            this.labelArriveQty.TabIndex = 117;
            this.labelArriveQty.Text = "Arrive Qty";
            // 
            // displayArriveQty
            // 
            this.displayArriveQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayArriveQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayArriveQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayArriveQty.Location = new System.Drawing.Point(602, 36);
            this.displayArriveQty.Name = "displayArriveQty";
            this.displayArriveQty.Size = new System.Drawing.Size(103, 21);
            this.displayArriveQty.TabIndex = 118;
            // 
            // labelArriveWHDate
            // 
            this.labelArriveWHDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelArriveWHDate.Location = new System.Drawing.Point(718, 7);
            this.labelArriveWHDate.Name = "labelArriveWHDate";
            this.labelArriveWHDate.Size = new System.Drawing.Size(120, 23);
            this.labelArriveWHDate.TabIndex = 120;
            this.labelArriveWHDate.Text = "Arrive W/H Date";
            // 
            // labelLastInspectionDate
            // 
            this.labelLastInspectionDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelLastInspectionDate.Location = new System.Drawing.Point(718, 35);
            this.labelLastInspectionDate.Name = "labelLastInspectionDate";
            this.labelLastInspectionDate.Size = new System.Drawing.Size(120, 23);
            this.labelLastInspectionDate.TabIndex = 121;
            this.labelLastInspectionDate.Text = "Last Inspection Date";
            // 
            // labelResult
            // 
            this.labelResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelResult.Location = new System.Drawing.Point(524, 63);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(75, 23);
            this.labelResult.TabIndex = 124;
            this.labelResult.Text = "Result";
            // 
            // checkNonMoisture
            // 
            this.checkNonMoisture.AutoSize = true;
            this.checkNonMoisture.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkNonMoisture.IsSupportEditMode = false;
            this.checkNonMoisture.Location = new System.Drawing.Point(718, 66);
            this.checkNonMoisture.Name = "checkNonMoisture";
            this.checkNonMoisture.ReadOnly = true;
            this.checkNonMoisture.Size = new System.Drawing.Size(111, 21);
            this.checkNonMoisture.TabIndex = 126;
            this.checkNonMoisture.Text = "Non Moisture";
            this.checkNonMoisture.UseVisualStyleBackColor = true;
            // 
            // labelApprover
            // 
            this.labelApprover.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelApprover.Location = new System.Drawing.Point(15, 121);
            this.labelApprover.Name = "labelApprover";
            this.labelApprover.Size = new System.Drawing.Size(126, 23);
            this.labelApprover.TabIndex = 129;
            this.labelApprover.Text = "Moisture Composition";
            // 
            // displayResult
            // 
            this.displayResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayResult.Location = new System.Drawing.Point(602, 64);
            this.displayResult.Name = "displayResult";
            this.displayResult.Size = new System.Drawing.Size(103, 21);
            this.displayResult.TabIndex = 133;
            // 
            // dateArriveWHDate
            // 
            this.dateArriveWHDate.Location = new System.Drawing.Point(844, 9);
            this.dateArriveWHDate.Name = "dateArriveWHDate";
            this.dateArriveWHDate.ReadOnly = true;
            this.dateArriveWHDate.Size = new System.Drawing.Size(150, 23);
            this.dateArriveWHDate.TabIndex = 135;
            // 
            // dateBox1
            // 
            this.dateBox1.Location = new System.Drawing.Point(429, 355);
            this.dateBox1.Name = "dateBox1";
            this.dateBox1.ReadOnly = true;
            this.dateBox1.Size = new System.Drawing.Size(130, 23);
            this.dateBox1.TabIndex = 136;
            // 
            // dateLastInspectionDate
            // 
            this.dateLastInspectionDate.Location = new System.Drawing.Point(844, 36);
            this.dateLastInspectionDate.Name = "dateLastInspectionDate";
            this.dateLastInspectionDate.ReadOnly = true;
            this.dateLastInspectionDate.Size = new System.Drawing.Size(150, 23);
            this.dateLastInspectionDate.TabIndex = 136;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label1.Location = new System.Drawing.Point(15, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 23);
            this.label1.TabIndex = 137;
            this.label1.Text = "Moisture Standard";
            // 
            // txtsupplier
            // 
            this.txtsupplier.DisplayBox1Binding = "";
            this.txtsupplier.Location = new System.Drawing.Point(356, 35);
            this.txtsupplier.Name = "txtsupplier";
            this.txtsupplier.Size = new System.Drawing.Size(147, 23);
            this.txtsupplier.TabIndex = 109;
            this.txtsupplier.TextBox1Binding = "";
            // 
            // cbmMaterialCompositionItem
            // 
            this.cbmMaterialCompositionItem.BackColor = System.Drawing.Color.White;
            this.cbmMaterialCompositionItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbmMaterialCompositionItem.FormattingEnabled = true;
            this.cbmMaterialCompositionItem.IsSupportUnselect = true;
            this.cbmMaterialCompositionItem.Location = new System.Drawing.Point(356, 121);
            this.cbmMaterialCompositionItem.Margin = new System.Windows.Forms.Padding(0);
            this.cbmMaterialCompositionItem.Name = "cbmMaterialCompositionItem";
            this.cbmMaterialCompositionItem.OldText = "";
            this.cbmMaterialCompositionItem.Size = new System.Drawing.Size(505, 24);
            this.cbmMaterialCompositionItem.TabIndex = 143;
            this.cbmMaterialCompositionItem.SelectedIndexChanged += new System.EventHandler(this.CbmMaterialCompositionItem_SelectedIndexChanged);
            // 
            // cbmMaterialCompositionGrp
            // 
            this.cbmMaterialCompositionGrp.BackColor = System.Drawing.Color.White;
            this.cbmMaterialCompositionGrp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbmMaterialCompositionGrp.FormattingEnabled = true;
            this.cbmMaterialCompositionGrp.IsSupportUnselect = true;
            this.cbmMaterialCompositionGrp.Location = new System.Drawing.Point(144, 121);
            this.cbmMaterialCompositionGrp.Margin = new System.Windows.Forms.Padding(0);
            this.cbmMaterialCompositionGrp.Name = "cbmMaterialCompositionGrp";
            this.cbmMaterialCompositionGrp.OldText = "";
            this.cbmMaterialCompositionGrp.Size = new System.Drawing.Size(209, 24);
            this.cbmMaterialCompositionGrp.TabIndex = 142;
            this.cbmMaterialCompositionGrp.SelectedIndexChanged += new System.EventHandler(this.CbmMaterialCompositionGrp_SelectedIndexChanged);
            // 
            // displayMoistureStandard
            // 
            this.displayMoistureStandard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayMoistureStandard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayMoistureStandard.Location = new System.Drawing.Point(144, 149);
            this.displayMoistureStandard.Name = "displayMoistureStandard";
            this.displayMoistureStandard.Size = new System.Drawing.Size(100, 23);
            this.displayMoistureStandard.TabIndex = 144;
            // 
            // P01_Moisture
            // 
            this.ClientSize = new System.Drawing.Size(1008, 733);
            this.Controls.Add(this.displayMoistureStandard);
            this.Controls.Add(this.cbmMaterialCompositionItem);
            this.Controls.Add(this.cbmMaterialCompositionGrp);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateLastInspectionDate);
            this.Controls.Add(this.dateBox1);
            this.Controls.Add(this.dateArriveWHDate);
            this.Controls.Add(this.displayResult);
            this.Controls.Add(this.labelApprover);
            this.Controls.Add(this.checkNonMoisture);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.labelLastInspectionDate);
            this.Controls.Add(this.labelArriveWHDate);
            this.Controls.Add(this.displayArriveQty);
            this.Controls.Add(this.labelArriveQty);
            this.Controls.Add(this.labelColor);
            this.Controls.Add(this.displayColor);
            this.Controls.Add(this.displayRefno);
            this.Controls.Add(this.labelSCIRefno);
            this.Controls.Add(this.displaySCIRefno1);
            this.Controls.Add(this.displaySCIRefno);
            this.Controls.Add(this.labelRefno);
            this.Controls.Add(this.txtsupplier);
            this.Controls.Add(this.labelSupplier);
            this.Controls.Add(this.displaySEQ);
            this.Controls.Add(this.labelSEQ);
            this.Controls.Add(this.displayWKNo);
            this.Controls.Add(this.labelWKNo);
            this.Controls.Add(this.displayBrand);
            this.Controls.Add(this.displayStyle);
            this.Controls.Add(this.displaySP);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.labelSP);
            this.EditMode = true;
            this.GridPopUp = false;
            this.GridUniqueKey = "Roll,Dyelot";
            this.KeyField1 = "ID";
            this.Name = "P01_Moisture";
            this.OnLineHelpID = "Sci.Win.Subs.Input4";
            this.Text = "Moisture Test";
            this.WorkAlias = "Fir_Moisture";
            this.Controls.SetChildIndex(this.labelSP, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.displaySP, 0);
            this.Controls.SetChildIndex(this.displayStyle, 0);
            this.Controls.SetChildIndex(this.displayBrand, 0);
            this.Controls.SetChildIndex(this.labelWKNo, 0);
            this.Controls.SetChildIndex(this.displayWKNo, 0);
            this.Controls.SetChildIndex(this.labelSEQ, 0);
            this.Controls.SetChildIndex(this.displaySEQ, 0);
            this.Controls.SetChildIndex(this.labelSupplier, 0);
            this.Controls.SetChildIndex(this.txtsupplier, 0);
            this.Controls.SetChildIndex(this.labelRefno, 0);
            this.Controls.SetChildIndex(this.displaySCIRefno, 0);
            this.Controls.SetChildIndex(this.displaySCIRefno1, 0);
            this.Controls.SetChildIndex(this.labelSCIRefno, 0);
            this.Controls.SetChildIndex(this.displayRefno, 0);
            this.Controls.SetChildIndex(this.displayColor, 0);
            this.Controls.SetChildIndex(this.labelColor, 0);
            this.Controls.SetChildIndex(this.labelArriveQty, 0);
            this.Controls.SetChildIndex(this.displayArriveQty, 0);
            this.Controls.SetChildIndex(this.labelArriveWHDate, 0);
            this.Controls.SetChildIndex(this.labelLastInspectionDate, 0);
            this.Controls.SetChildIndex(this.labelResult, 0);
            this.Controls.SetChildIndex(this.checkNonMoisture, 0);
            this.Controls.SetChildIndex(this.labelApprover, 0);
            this.Controls.SetChildIndex(this.displayResult, 0);
            this.Controls.SetChildIndex(this.dateArriveWHDate, 0);
            this.Controls.SetChildIndex(this.dateBox1, 0);
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.dateLastInspectionDate, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cbmMaterialCompositionGrp, 0);
            this.Controls.SetChildIndex(this.cbmMaterialCompositionItem, 0);
            this.Controls.SetChildIndex(this.displayMoistureStandard, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DisplayBox displayBrand;
        private Win.UI.DisplayBox displayStyle;
        private Win.UI.DisplayBox displaySP;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelStyle;
        private Win.UI.Label labelSP;
        private Win.UI.DisplayBox displayWKNo;
        private Win.UI.Label labelWKNo;
        private Win.UI.DisplayBox displaySEQ;
        private Win.UI.Label labelSEQ;
        private Win.UI.Label labelSupplier;
        private Class.Txtsupplier txtsupplier;
        private Win.UI.Label labelRefno;
        private Win.UI.DisplayBox displaySCIRefno;
        private Win.UI.DisplayBox displaySCIRefno1;
        private Win.UI.Label labelSCIRefno;
        private Win.UI.DisplayBox displayRefno;
        private Win.UI.DisplayBox displayColor;
        private Win.UI.Label labelColor;
        private Win.UI.Label labelArriveQty;
        private Win.UI.DisplayBox displayArriveQty;
        private Win.UI.Label labelArriveWHDate;
        private Win.UI.Label labelLastInspectionDate;
        private Win.UI.Label labelResult;
        private Win.UI.CheckBox checkNonMoisture;
        private Win.UI.Label labelApprover;
        private Win.UI.DisplayBox displayResult;
        private Win.UI.DateBox dateArriveWHDate;
        private Win.UI.DateBox dateBox1;
        private Win.UI.DateBox dateLastInspectionDate;
        private Win.UI.Label label1;
        private Win.UI.ComboBox cbmMaterialCompositionItem;
        private Win.UI.ComboBox cbmMaterialCompositionGrp;
        private Win.UI.DisplayBox displayMoistureStandard;
    }
}
