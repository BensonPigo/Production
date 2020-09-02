namespace Sci.Production.Quality
{
    partial class P01_PhysicalInspection
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
            this.labelBrandRefno = new Sci.Win.UI.Label();
            this.displaySCIRefno = new Sci.Win.UI.DisplayBox();
            this.displaySCIRefno1 = new Sci.Win.UI.DisplayBox();
            this.labelSCIRefno = new Sci.Win.UI.Label();
            this.displayBrandRefno = new Sci.Win.UI.DisplayBox();
            this.displayColor = new Sci.Win.UI.DisplayBox();
            this.labelColor = new Sci.Win.UI.Label();
            this.labelArriveQty = new Sci.Win.UI.Label();
            this.displayArriveQty = new Sci.Win.UI.DisplayBox();
            this.labelArriveWHDate = new Sci.Win.UI.Label();
            this.labelLastInspectionDate = new Sci.Win.UI.Label();
            this.labelResult = new Sci.Win.UI.Label();
            this.checkNonInspection = new Sci.Win.UI.CheckBox();
            this.btnApprove = new Sci.Win.UI.Button();
            this.btnEncode = new Sci.Win.UI.Button();
            this.labelApprover = new Sci.Win.UI.Label();
            this.displayApprover = new Sci.Win.UI.DisplayBox();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.txtuserApprover = new Sci.Production.Class.Txtuser();
            this.txtsupplier = new Sci.Production.Class.Txtsupplier();
            this.displayResult = new Sci.Win.UI.DisplayBox();
            this.textID = new Sci.Win.UI.TextBox();
            this.dateLastInspectionDate = new Sci.Win.UI.DateBox();
            this.dateArriveWHDate = new Sci.Win.UI.DateBox();
            this.displaydescDetail = new Sci.Win.UI.DisplayBox();
            this.btnToExcel_defect = new Sci.Win.UI.Button();
            this.btnSendMail = new Sci.Win.UI.Button();
            this.txtPhysicalInspector = new Sci.Win.UI.TextBox();
            this.labinspector = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.btnToExcel_defect);
            this.btmcont.Controls.Add(this.btnToExcel);
            this.btmcont.Location = new System.Drawing.Point(0, 693);
            this.btmcont.Size = new System.Drawing.Size(1008, 40);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.btnToExcel, 0);
            this.btmcont.Controls.SetChildIndex(this.btnToExcel_defect, 0);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 195);
            this.gridcont.Size = new System.Drawing.Size(984, 488);
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
            this.displayBrand.Location = new System.Drawing.Point(325, 72);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(147, 21);
            this.displayBrand.TabIndex = 103;
            // 
            // displayStyle
            // 
            this.displayStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyle.Location = new System.Drawing.Point(94, 72);
            this.displayStyle.Name = "displayStyle";
            this.displayStyle.Size = new System.Drawing.Size(147, 21);
            this.displayStyle.TabIndex = 102;
            // 
            // displaySP
            // 
            this.displaySP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displaySP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySP.Location = new System.Drawing.Point(94, 12);
            this.displaySP.Name = "displaySP";
            this.displaySP.Size = new System.Drawing.Size(147, 21);
            this.displaySP.TabIndex = 101;
            // 
            // labelBrand
            // 
            this.labelBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelBrand.Location = new System.Drawing.Point(247, 71);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(75, 23);
            this.labelBrand.TabIndex = 100;
            this.labelBrand.Text = "Brand";
            // 
            // labelStyle
            // 
            this.labelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelStyle.Location = new System.Drawing.Point(15, 71);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(75, 23);
            this.labelStyle.TabIndex = 99;
            this.labelStyle.Text = "Style";
            // 
            // labelSP
            // 
            this.labelSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelSP.Location = new System.Drawing.Point(15, 11);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(75, 23);
            this.labelSP.TabIndex = 98;
            this.labelSP.Text = "SP#";
            // 
            // displayWKNo
            // 
            this.displayWKNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayWKNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayWKNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayWKNo.Location = new System.Drawing.Point(94, 42);
            this.displayWKNo.Name = "displayWKNo";
            this.displayWKNo.Size = new System.Drawing.Size(147, 21);
            this.displayWKNo.TabIndex = 105;
            // 
            // labelWKNo
            // 
            this.labelWKNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelWKNo.Location = new System.Drawing.Point(15, 41);
            this.labelWKNo.Name = "labelWKNo";
            this.labelWKNo.Size = new System.Drawing.Size(75, 23);
            this.labelWKNo.TabIndex = 104;
            this.labelWKNo.Text = "WKNo";
            // 
            // displaySEQ
            // 
            this.displaySEQ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySEQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displaySEQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySEQ.Location = new System.Drawing.Point(325, 11);
            this.displaySEQ.Name = "displaySEQ";
            this.displaySEQ.Size = new System.Drawing.Size(55, 21);
            this.displaySEQ.TabIndex = 107;
            // 
            // labelSEQ
            // 
            this.labelSEQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelSEQ.Location = new System.Drawing.Point(247, 10);
            this.labelSEQ.Name = "labelSEQ";
            this.labelSEQ.Size = new System.Drawing.Size(75, 23);
            this.labelSEQ.TabIndex = 106;
            this.labelSEQ.Text = "SEQ";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelSupplier.Location = new System.Drawing.Point(247, 40);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(75, 23);
            this.labelSupplier.TabIndex = 108;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelBrandRefno
            // 
            this.labelBrandRefno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelBrandRefno.Location = new System.Drawing.Point(16, 102);
            this.labelBrandRefno.Name = "labelBrandRefno";
            this.labelBrandRefno.Size = new System.Drawing.Size(75, 23);
            this.labelBrandRefno.TabIndex = 110;
            this.labelBrandRefno.Text = "Brand Refno";
            // 
            // displaySCIRefno
            // 
            this.displaySCIRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySCIRefno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displaySCIRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySCIRefno.Location = new System.Drawing.Point(94, 133);
            this.displaySCIRefno.Name = "displaySCIRefno";
            this.displaySCIRefno.Size = new System.Drawing.Size(147, 21);
            this.displaySCIRefno.TabIndex = 111;
            // 
            // displaySCIRefno1
            // 
            this.displaySCIRefno1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySCIRefno1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displaySCIRefno1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySCIRefno1.Location = new System.Drawing.Point(247, 132);
            this.displaySCIRefno1.Name = "displaySCIRefno1";
            this.displaySCIRefno1.Size = new System.Drawing.Size(730, 21);
            this.displaySCIRefno1.TabIndex = 112;
            // 
            // labelSCIRefno
            // 
            this.labelSCIRefno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelSCIRefno.Location = new System.Drawing.Point(16, 132);
            this.labelSCIRefno.Name = "labelSCIRefno";
            this.labelSCIRefno.Size = new System.Drawing.Size(75, 23);
            this.labelSCIRefno.TabIndex = 113;
            this.labelSCIRefno.Text = "SCI Refno";
            // 
            // displayBrandRefno
            // 
            this.displayBrandRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrandRefno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayBrandRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrandRefno.Location = new System.Drawing.Point(94, 102);
            this.displayBrandRefno.Name = "displayBrandRefno";
            this.displayBrandRefno.Size = new System.Drawing.Size(147, 21);
            this.displayBrandRefno.TabIndex = 114;
            // 
            // displayColor
            // 
            this.displayColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayColor.Location = new System.Drawing.Point(570, 11);
            this.displayColor.Name = "displayColor";
            this.displayColor.Size = new System.Drawing.Size(101, 21);
            this.displayColor.TabIndex = 115;
            // 
            // labelColor
            // 
            this.labelColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelColor.Location = new System.Drawing.Point(492, 10);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(75, 23);
            this.labelColor.TabIndex = 116;
            this.labelColor.Text = "Color";
            // 
            // labelArriveQty
            // 
            this.labelArriveQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelArriveQty.Location = new System.Drawing.Point(492, 40);
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
            this.displayArriveQty.Location = new System.Drawing.Point(570, 41);
            this.displayArriveQty.Name = "displayArriveQty";
            this.displayArriveQty.Size = new System.Drawing.Size(101, 21);
            this.displayArriveQty.TabIndex = 118;
            // 
            // labelArriveWHDate
            // 
            this.labelArriveWHDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelArriveWHDate.Location = new System.Drawing.Point(700, 10);
            this.labelArriveWHDate.Name = "labelArriveWHDate";
            this.labelArriveWHDate.Size = new System.Drawing.Size(123, 23);
            this.labelArriveWHDate.TabIndex = 120;
            this.labelArriveWHDate.Text = "Arrive W/H Date";
            // 
            // labelLastInspectionDate
            // 
            this.labelLastInspectionDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelLastInspectionDate.Location = new System.Drawing.Point(700, 40);
            this.labelLastInspectionDate.Name = "labelLastInspectionDate";
            this.labelLastInspectionDate.Size = new System.Drawing.Size(123, 23);
            this.labelLastInspectionDate.TabIndex = 121;
            this.labelLastInspectionDate.Text = "Last Inspection Date";
            // 
            // labelResult
            // 
            this.labelResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelResult.Location = new System.Drawing.Point(492, 70);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(75, 23);
            this.labelResult.TabIndex = 124;
            this.labelResult.Text = "Result";
            // 
            // checkNonInspection
            // 
            this.checkNonInspection.AutoSize = true;
            this.checkNonInspection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkNonInspection.IsSupportEditMode = false;
            this.checkNonInspection.Location = new System.Drawing.Point(602, 162);
            this.checkNonInspection.Name = "checkNonInspection";
            this.checkNonInspection.ReadOnly = true;
            this.checkNonInspection.Size = new System.Drawing.Size(121, 21);
            this.checkNonInspection.TabIndex = 126;
            this.checkNonInspection.Text = "Non Inspection";
            this.checkNonInspection.UseVisualStyleBackColor = true;
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(918, 158);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(75, 30);
            this.btnApprove.TabIndex = 127;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.BtnApprove_Click);
            // 
            // btnEncode
            // 
            this.btnEncode.Location = new System.Drawing.Point(827, 158);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(80, 30);
            this.btnEncode.TabIndex = 128;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.BtnEncode_Click);
            // 
            // labelApprover
            // 
            this.labelApprover.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelApprover.Location = new System.Drawing.Point(16, 163);
            this.labelApprover.Name = "labelApprover";
            this.labelApprover.Size = new System.Drawing.Size(75, 23);
            this.labelApprover.TabIndex = 129;
            this.labelApprover.Text = "Approver";
            // 
            // displayApprover
            // 
            this.displayApprover.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayApprover.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayApprover.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayApprover.Location = new System.Drawing.Point(399, 163);
            this.displayApprover.Name = "displayApprover";
            this.displayApprover.Size = new System.Drawing.Size(197, 21);
            this.displayApprover.TabIndex = 130;
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(530, 5);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(90, 30);
            this.btnToExcel.TabIndex = 132;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // txtuserApprover
            // 
            this.txtuserApprover.DisplayBox1Binding = "";
            this.txtuserApprover.Location = new System.Drawing.Point(94, 162);
            this.txtuserApprover.Name = "txtuserApprover";
            this.txtuserApprover.Size = new System.Drawing.Size(305, 23);
            this.txtuserApprover.TabIndex = 131;
            this.txtuserApprover.TextBox1Binding = "";
            // 
            // txtsupplier
            // 
            this.txtsupplier.DisplayBox1Binding = "";
            this.txtsupplier.Location = new System.Drawing.Point(325, 40);
            this.txtsupplier.Name = "txtsupplier";
            this.txtsupplier.Size = new System.Drawing.Size(147, 23);
            this.txtsupplier.TabIndex = 109;
            this.txtsupplier.TextBox1Binding = "";
            // 
            // displayResult
            // 
            this.displayResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayResult.Location = new System.Drawing.Point(570, 71);
            this.displayResult.Name = "displayResult";
            this.displayResult.Size = new System.Drawing.Size(101, 21);
            this.displayResult.TabIndex = 132;
            // 
            // textID
            // 
            this.textID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.textID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.textID.Location = new System.Drawing.Point(996, 119);
            this.textID.Name = "textID";
            this.textID.ReadOnly = true;
            this.textID.Size = new System.Drawing.Size(100, 23);
            this.textID.TabIndex = 1;
            this.textID.Visible = false;
            // 
            // dateLastInspectionDate
            // 
            this.dateLastInspectionDate.Location = new System.Drawing.Point(826, 41);
            this.dateLastInspectionDate.Name = "dateLastInspectionDate";
            this.dateLastInspectionDate.ReadOnly = true;
            this.dateLastInspectionDate.Size = new System.Drawing.Size(150, 23);
            this.dateLastInspectionDate.TabIndex = 138;
            // 
            // dateArriveWHDate
            // 
            this.dateArriveWHDate.Location = new System.Drawing.Point(826, 12);
            this.dateArriveWHDate.Name = "dateArriveWHDate";
            this.dateArriveWHDate.ReadOnly = true;
            this.dateArriveWHDate.Size = new System.Drawing.Size(150, 23);
            this.dateArriveWHDate.TabIndex = 139;
            // 
            // displaydescDetail
            // 
            this.displaydescDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaydescDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displaydescDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaydescDetail.Location = new System.Drawing.Point(247, 102);
            this.displaydescDetail.Name = "displaydescDetail";
            this.displaydescDetail.Size = new System.Drawing.Size(730, 21);
            this.displaydescDetail.TabIndex = 140;
            // 
            // btnToExcel_defect
            // 
            this.btnToExcel_defect.Location = new System.Drawing.Point(626, 5);
            this.btnToExcel_defect.Name = "btnToExcel_defect";
            this.btnToExcel_defect.Size = new System.Drawing.Size(179, 30);
            this.btnToExcel_defect.TabIndex = 133;
            this.btnToExcel_defect.Text = "To Excel (Act. defect Yds)";
            this.btnToExcel_defect.UseVisualStyleBackColor = true;
            this.btnToExcel_defect.Click += new System.EventHandler(this.BtnToExcel_defect_Click);
            // 
            // btnSendMail
            // 
            this.btnSendMail.Location = new System.Drawing.Point(732, 159);
            this.btnSendMail.Name = "btnSendMail";
            this.btnSendMail.Size = new System.Drawing.Size(85, 30);
            this.btnSendMail.TabIndex = 141;
            this.btnSendMail.Text = "Send Mail";
            this.btnSendMail.UseVisualStyleBackColor = true;
            this.btnSendMail.Click += new System.EventHandler(this.BtnSendMail_Click);
            // 
            // txtPhysicalInspector
            // 
            this.txtPhysicalInspector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtPhysicalInspector.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtPhysicalInspector.IsSupportEditMode = false;
            this.txtPhysicalInspector.Location = new System.Drawing.Point(826, 69);
            this.txtPhysicalInspector.Name = "txtPhysicalInspector";
            this.txtPhysicalInspector.ReadOnly = true;
            this.txtPhysicalInspector.Size = new System.Drawing.Size(150, 23);
            this.txtPhysicalInspector.TabIndex = 234;
            // 
            // labinspector
            // 
            this.labinspector.Location = new System.Drawing.Point(700, 69);
            this.labinspector.Name = "labinspector";
            this.labinspector.Size = new System.Drawing.Size(123, 23);
            this.labinspector.TabIndex = 233;
            this.labinspector.Text = "Physical Inspector";
            // 
            // P01_PhysicalInspection
            // 
            this.ClientSize = new System.Drawing.Size(1008, 733);
            this.Controls.Add(this.txtPhysicalInspector);
            this.Controls.Add(this.labinspector);
            this.Controls.Add(this.btnSendMail);
            this.Controls.Add(this.displaydescDetail);
            this.Controls.Add(this.dateArriveWHDate);
            this.Controls.Add(this.dateLastInspectionDate);
            this.Controls.Add(this.textID);
            this.Controls.Add(this.displayResult);
            this.Controls.Add(this.txtuserApprover);
            this.Controls.Add(this.displayApprover);
            this.Controls.Add(this.labelApprover);
            this.Controls.Add(this.btnEncode);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.checkNonInspection);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.labelLastInspectionDate);
            this.Controls.Add(this.labelArriveWHDate);
            this.Controls.Add(this.displayArriveQty);
            this.Controls.Add(this.labelArriveQty);
            this.Controls.Add(this.labelColor);
            this.Controls.Add(this.displayColor);
            this.Controls.Add(this.displayBrandRefno);
            this.Controls.Add(this.labelSCIRefno);
            this.Controls.Add(this.displaySCIRefno1);
            this.Controls.Add(this.displaySCIRefno);
            this.Controls.Add(this.labelBrandRefno);
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
            this.DefaultOrder = "Roll,Dyelot";
            this.EditMode = true;
            this.GridPopUp = false;
            this.GridUniqueKey = "Roll,Dyelot";
            this.KeyField1 = "ID";
            this.Name = "P01_PhysicalInspection";
            this.Text = "Physical Inspection";
            this.WorkAlias = "Fir_Physical";
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
            this.Controls.SetChildIndex(this.labelBrandRefno, 0);
            this.Controls.SetChildIndex(this.displaySCIRefno, 0);
            this.Controls.SetChildIndex(this.displaySCIRefno1, 0);
            this.Controls.SetChildIndex(this.labelSCIRefno, 0);
            this.Controls.SetChildIndex(this.displayBrandRefno, 0);
            this.Controls.SetChildIndex(this.displayColor, 0);
            this.Controls.SetChildIndex(this.labelColor, 0);
            this.Controls.SetChildIndex(this.labelArriveQty, 0);
            this.Controls.SetChildIndex(this.displayArriveQty, 0);
            this.Controls.SetChildIndex(this.labelArriveWHDate, 0);
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.labelLastInspectionDate, 0);
            this.Controls.SetChildIndex(this.labelResult, 0);
            this.Controls.SetChildIndex(this.checkNonInspection, 0);
            this.Controls.SetChildIndex(this.btnApprove, 0);
            this.Controls.SetChildIndex(this.btnEncode, 0);
            this.Controls.SetChildIndex(this.labelApprover, 0);
            this.Controls.SetChildIndex(this.displayApprover, 0);
            this.Controls.SetChildIndex(this.txtuserApprover, 0);
            this.Controls.SetChildIndex(this.displayResult, 0);
            this.Controls.SetChildIndex(this.textID, 0);
            this.Controls.SetChildIndex(this.dateLastInspectionDate, 0);
            this.Controls.SetChildIndex(this.dateArriveWHDate, 0);
            this.Controls.SetChildIndex(this.displaydescDetail, 0);
            this.Controls.SetChildIndex(this.btnSendMail, 0);
            this.Controls.SetChildIndex(this.labinspector, 0);
            this.Controls.SetChildIndex(this.txtPhysicalInspector, 0);
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
        private Win.UI.Label labelBrandRefno;
        private Win.UI.DisplayBox displaySCIRefno;
        private Win.UI.DisplayBox displaySCIRefno1;
        private Win.UI.Label labelSCIRefno;
        private Win.UI.DisplayBox displayBrandRefno;
        private Win.UI.DisplayBox displayColor;
        private Win.UI.Label labelColor;
        private Win.UI.Label labelArriveQty;
        private Win.UI.DisplayBox displayArriveQty;
        private Win.UI.Label labelArriveWHDate;
        private Win.UI.Label labelLastInspectionDate;
        private Win.UI.Button btnToExcel;
        private Win.UI.Label labelResult;
        private Win.UI.CheckBox checkNonInspection;
        private Win.UI.Button btnApprove;
        private Win.UI.Button btnEncode;
        private Win.UI.Label labelApprover;
        private Win.UI.DisplayBox displayApprover;
        private Class.Txtuser txtuserApprover;
        private Win.UI.DisplayBox displayResult;
        private Win.UI.TextBox textID;
        private Win.UI.DateBox dateLastInspectionDate;
        private Win.UI.DateBox dateArriveWHDate;
        private Win.UI.DisplayBox displaydescDetail;
        private Win.UI.Button btnToExcel_defect;
        private Win.UI.Button btnSendMail;
        private Win.UI.TextBox txtPhysicalInspector;
        private Win.UI.Label labinspector;
    }
}
