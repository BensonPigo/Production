namespace Sci.Production.Quality
{
    partial class P03_Crocking
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
            this.labelSP = new Sci.Win.UI.Label();
            this.labelWkno = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelSEQ = new Sci.Win.UI.Label();
            this.labelSupp = new Sci.Win.UI.Label();
            this.labelSCIRefno = new Sci.Win.UI.Label();
            this.labelBrandRefno = new Sci.Win.UI.Label();
            this.labelColor = new Sci.Win.UI.Label();
            this.labelArriveQty = new Sci.Win.UI.Label();
            this.labelArriveWHDate = new Sci.Win.UI.Label();
            this.labelLastInspectionDate = new Sci.Win.UI.Label();
            this.labelResult = new Sci.Win.UI.Label();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.txtWkno = new Sci.Win.UI.TextBox();
            this.txtStyle = new Sci.Win.UI.TextBox();
            this.txtBrand = new Sci.Win.UI.TextBox();
            this.txtSEQ = new Sci.Win.UI.TextBox();
            this.txtSCIRefno = new Sci.Win.UI.TextBox();
            this.txtBrandRefno = new Sci.Win.UI.TextBox();
            this.txtColor = new Sci.Win.UI.TextBox();
            this.txtArriveQty = new Sci.Win.UI.TextBox();
            this.txtResult = new Sci.Win.UI.TextBox();
            this.btnEncode = new Sci.Win.UI.Button();
            this.checkNA = new Sci.Win.UI.CheckBox();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.dateArriveWHDate = new Sci.Win.UI.DateBox();
            this.dateLastInspectionDate = new Sci.Win.UI.DateBox();
            this.txtsupplierSupp = new Sci.Production.Class.Txtsupplier();
            this.labelDescription = new Sci.Win.UI.Label();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.btntoPDF = new Sci.Win.UI.Button();
            this.txtCrockingInspector = new Sci.Win.UI.TextBox();
            this.labinspector = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.btntoPDF);
            this.btmcont.Controls.Add(this.btnToExcel);
            this.btmcont.Location = new System.Drawing.Point(0, 545);
            this.btmcont.Size = new System.Drawing.Size(979, 40);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.btnToExcel, 0);
            this.btmcont.Controls.SetChildIndex(this.btntoPDF, 0);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 154);
            this.gridcont.Size = new System.Drawing.Size(955, 381);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(889, 5);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(809, 5);
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(9, 9);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(46, 23);
            this.labelSP.TabIndex = 98;
            this.labelSP.Text = "SP#";
            // 
            // labelWkno
            // 
            this.labelWkno.Location = new System.Drawing.Point(9, 38);
            this.labelWkno.Name = "labelWkno";
            this.labelWkno.Size = new System.Drawing.Size(46, 23);
            this.labelWkno.TabIndex = 99;
            this.labelWkno.Text = "Wkno";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(9, 68);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(46, 23);
            this.labelStyle.TabIndex = 100;
            this.labelStyle.Text = "Style#";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(9, 96);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(46, 23);
            this.labelBrand.TabIndex = 101;
            this.labelBrand.Text = "Brand";
            // 
            // labelSEQ
            // 
            this.labelSEQ.Location = new System.Drawing.Point(184, 9);
            this.labelSEQ.Name = "labelSEQ";
            this.labelSEQ.Size = new System.Drawing.Size(51, 23);
            this.labelSEQ.TabIndex = 102;
            this.labelSEQ.Text = "SEQ#";
            // 
            // labelSupp
            // 
            this.labelSupp.Location = new System.Drawing.Point(184, 38);
            this.labelSupp.Name = "labelSupp";
            this.labelSupp.Size = new System.Drawing.Size(86, 23);
            this.labelSupp.TabIndex = 103;
            this.labelSupp.Text = "Supp";
            // 
            // labelSCIRefno
            // 
            this.labelSCIRefno.Location = new System.Drawing.Point(184, 67);
            this.labelSCIRefno.Name = "labelSCIRefno";
            this.labelSCIRefno.Size = new System.Drawing.Size(86, 23);
            this.labelSCIRefno.TabIndex = 104;
            this.labelSCIRefno.Text = "SCI Refno";
            // 
            // labelBrandRefno
            // 
            this.labelBrandRefno.Location = new System.Drawing.Point(184, 96);
            this.labelBrandRefno.Name = "labelBrandRefno";
            this.labelBrandRefno.Size = new System.Drawing.Size(86, 25);
            this.labelBrandRefno.TabIndex = 105;
            this.labelBrandRefno.Text = "Brand Refno";
            // 
            // labelColor
            // 
            this.labelColor.Location = new System.Drawing.Point(309, 9);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(47, 23);
            this.labelColor.TabIndex = 106;
            this.labelColor.Text = "Color";
            // 
            // labelArriveQty
            // 
            this.labelArriveQty.Location = new System.Drawing.Point(469, 9);
            this.labelArriveQty.Name = "labelArriveQty";
            this.labelArriveQty.Size = new System.Drawing.Size(119, 23);
            this.labelArriveQty.TabIndex = 107;
            this.labelArriveQty.Text = "Arrive Qty";
            // 
            // labelArriveWHDate
            // 
            this.labelArriveWHDate.Location = new System.Drawing.Point(693, 9);
            this.labelArriveWHDate.Name = "labelArriveWHDate";
            this.labelArriveWHDate.Size = new System.Drawing.Size(137, 23);
            this.labelArriveWHDate.TabIndex = 108;
            this.labelArriveWHDate.Text = "Arrive W/H Date";
            // 
            // labelLastInspectionDate
            // 
            this.labelLastInspectionDate.Location = new System.Drawing.Point(693, 38);
            this.labelLastInspectionDate.Name = "labelLastInspectionDate";
            this.labelLastInspectionDate.Size = new System.Drawing.Size(137, 23);
            this.labelLastInspectionDate.TabIndex = 109;
            this.labelLastInspectionDate.Text = "Last Inspection Date ";
            // 
            // labelResult
            // 
            this.labelResult.Location = new System.Drawing.Point(469, 38);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(119, 23);
            this.labelResult.TabIndex = 110;
            this.labelResult.Text = "Result";
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSP.IsSupportEditMode = false;
            this.txtSP.Location = new System.Drawing.Point(58, 9);
            this.txtSP.Name = "txtSP";
            this.txtSP.ReadOnly = true;
            this.txtSP.Size = new System.Drawing.Size(115, 23);
            this.txtSP.TabIndex = 111;
            // 
            // txtWkno
            // 
            this.txtWkno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtWkno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtWkno.IsSupportEditMode = false;
            this.txtWkno.Location = new System.Drawing.Point(58, 38);
            this.txtWkno.Name = "txtWkno";
            this.txtWkno.ReadOnly = true;
            this.txtWkno.Size = new System.Drawing.Size(115, 23);
            this.txtWkno.TabIndex = 112;
            // 
            // txtStyle
            // 
            this.txtStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtStyle.IsSupportEditMode = false;
            this.txtStyle.Location = new System.Drawing.Point(58, 67);
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.ReadOnly = true;
            this.txtStyle.Size = new System.Drawing.Size(115, 23);
            this.txtStyle.TabIndex = 113;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtBrand.IsSupportEditMode = false;
            this.txtBrand.Location = new System.Drawing.Point(58, 96);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.ReadOnly = true;
            this.txtBrand.Size = new System.Drawing.Size(115, 23);
            this.txtBrand.TabIndex = 114;
            // 
            // txtSEQ
            // 
            this.txtSEQ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSEQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSEQ.IsSupportEditMode = false;
            this.txtSEQ.Location = new System.Drawing.Point(238, 9);
            this.txtSEQ.Name = "txtSEQ";
            this.txtSEQ.ReadOnly = true;
            this.txtSEQ.Size = new System.Drawing.Size(60, 23);
            this.txtSEQ.TabIndex = 115;
            // 
            // txtSCIRefno
            // 
            this.txtSCIRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSCIRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSCIRefno.IsSupportEditMode = false;
            this.txtSCIRefno.Location = new System.Drawing.Point(273, 67);
            this.txtSCIRefno.Name = "txtSCIRefno";
            this.txtSCIRefno.ReadOnly = true;
            this.txtSCIRefno.Size = new System.Drawing.Size(188, 23);
            this.txtSCIRefno.TabIndex = 117;
            // 
            // txtBrandRefno
            // 
            this.txtBrandRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtBrandRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtBrandRefno.IsSupportEditMode = false;
            this.txtBrandRefno.Location = new System.Drawing.Point(274, 96);
            this.txtBrandRefno.Name = "txtBrandRefno";
            this.txtBrandRefno.ReadOnly = true;
            this.txtBrandRefno.Size = new System.Drawing.Size(187, 23);
            this.txtBrandRefno.TabIndex = 118;
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtColor.IsSupportEditMode = false;
            this.txtColor.Location = new System.Drawing.Point(359, 9);
            this.txtColor.Name = "txtColor";
            this.txtColor.ReadOnly = true;
            this.txtColor.Size = new System.Drawing.Size(102, 23);
            this.txtColor.TabIndex = 119;
            // 
            // txtArriveQty
            // 
            this.txtArriveQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtArriveQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtArriveQty.IsSupportEditMode = false;
            this.txtArriveQty.Location = new System.Drawing.Point(590, 9);
            this.txtArriveQty.Name = "txtArriveQty";
            this.txtArriveQty.ReadOnly = true;
            this.txtArriveQty.Size = new System.Drawing.Size(90, 23);
            this.txtArriveQty.TabIndex = 120;
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtResult.IsSupportEditMode = false;
            this.txtResult.Location = new System.Drawing.Point(591, 38);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(89, 23);
            this.txtResult.TabIndex = 123;
            // 
            // btnEncode
            // 
            this.btnEncode.Location = new System.Drawing.Point(883, 89);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(80, 30);
            this.btnEncode.TabIndex = 124;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // checkNA
            // 
            this.checkNA.AutoSize = true;
            this.checkNA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkNA.IsSupportEditMode = false;
            this.checkNA.Location = new System.Drawing.Point(913, 67);
            this.checkNA.Name = "checkNA";
            this.checkNA.ReadOnly = true;
            this.checkNA.Size = new System.Drawing.Size(50, 21);
            this.checkNA.TabIndex = 153;
            this.checkNA.Text = "N/A";
            this.checkNA.UseVisualStyleBackColor = true;
            // 
            // btnToExcel
            // 
            this.btnToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnToExcel.Location = new System.Drawing.Point(723, 5);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 95;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.btnToExcel_Click);
            // 
            // dateArriveWHDate
            // 
            this.dateArriveWHDate.IsSupportEditMode = false;
            this.dateArriveWHDate.Location = new System.Drawing.Point(833, 9);
            this.dateArriveWHDate.Name = "dateArriveWHDate";
            this.dateArriveWHDate.ReadOnly = true;
            this.dateArriveWHDate.Size = new System.Drawing.Size(130, 23);
            this.dateArriveWHDate.TabIndex = 154;
            // 
            // dateLastInspectionDate
            // 
            this.dateLastInspectionDate.IsSupportEditMode = false;
            this.dateLastInspectionDate.Location = new System.Drawing.Point(833, 38);
            this.dateLastInspectionDate.Name = "dateLastInspectionDate";
            this.dateLastInspectionDate.ReadOnly = true;
            this.dateLastInspectionDate.Size = new System.Drawing.Size(130, 23);
            this.dateLastInspectionDate.TabIndex = 155;
            // 
            // txtsupplierSupp
            // 
            this.txtsupplierSupp.DisplayBox1Binding = "";
            this.txtsupplierSupp.Location = new System.Drawing.Point(273, 38);
            this.txtsupplierSupp.Name = "txtsupplierSupp";
            this.txtsupplierSupp.Size = new System.Drawing.Size(145, 23);
            this.txtsupplierSupp.TabIndex = 156;
            this.txtsupplierSupp.TextBox1Binding = "";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(469, 96);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(119, 25);
            this.labelDescription.TabIndex = 157;
            this.labelDescription.Text = "Description";
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editDescription.Location = new System.Drawing.Point(591, 96);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.ReadOnly = true;
            this.editDescription.Size = new System.Drawing.Size(279, 54);
            this.editDescription.TabIndex = 158;
            // 
            // btntoPDF
            // 
            this.btntoPDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btntoPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btntoPDF.Location = new System.Drawing.Point(637, 5);
            this.btntoPDF.Name = "btntoPDF";
            this.btntoPDF.Size = new System.Drawing.Size(80, 30);
            this.btntoPDF.TabIndex = 96;
            this.btntoPDF.Text = "To PDF";
            this.btntoPDF.UseVisualStyleBackColor = true;
            this.btntoPDF.Click += new System.EventHandler(this.btntoPDF_Click);
            // 
            // txtCrockingInspector
            // 
            this.txtCrockingInspector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtCrockingInspector.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtCrockingInspector.IsSupportEditMode = false;
            this.txtCrockingInspector.Location = new System.Drawing.Point(591, 67);
            this.txtCrockingInspector.Name = "txtCrockingInspector";
            this.txtCrockingInspector.ReadOnly = true;
            this.txtCrockingInspector.Size = new System.Drawing.Size(239, 23);
            this.txtCrockingInspector.TabIndex = 226;
            // 
            // labinspector
            // 
            this.labinspector.Location = new System.Drawing.Point(469, 67);
            this.labinspector.Name = "labinspector";
            this.labinspector.Size = new System.Drawing.Size(119, 23);
            this.labinspector.TabIndex = 225;
            this.labinspector.Text = "Crocking Inspector";
            // 
            // P03_Crocking
            // 
            this.ClientSize = new System.Drawing.Size(979, 585);
            this.Controls.Add(this.txtCrockingInspector);
            this.Controls.Add(this.labinspector);
            this.Controls.Add(this.editDescription);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.txtsupplierSupp);
            this.Controls.Add(this.dateLastInspectionDate);
            this.Controls.Add(this.dateArriveWHDate);
            this.Controls.Add(this.checkNA);
            this.Controls.Add(this.btnEncode);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.txtArriveQty);
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.txtBrandRefno);
            this.Controls.Add(this.txtSCIRefno);
            this.Controls.Add(this.txtSEQ);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.txtStyle);
            this.Controls.Add(this.txtWkno);
            this.Controls.Add(this.txtSP);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.labelLastInspectionDate);
            this.Controls.Add(this.labelArriveWHDate);
            this.Controls.Add(this.labelArriveQty);
            this.Controls.Add(this.labelColor);
            this.Controls.Add(this.labelBrandRefno);
            this.Controls.Add(this.labelSCIRefno);
            this.Controls.Add(this.labelSupp);
            this.Controls.Add(this.labelSEQ);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.labelWkno);
            this.Controls.Add(this.labelSP);
            this.GridPopUp = false;
            this.KeyField1 = "id";
            this.Name = "P03_Crocking";
            this.Text = "Crocking Test";
            this.WorkAlias = "FIR_Laboratory_Crocking";
            this.Controls.SetChildIndex(this.labelSP, 0);
            this.Controls.SetChildIndex(this.labelWkno, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelSEQ, 0);
            this.Controls.SetChildIndex(this.labelSupp, 0);
            this.Controls.SetChildIndex(this.labelSCIRefno, 0);
            this.Controls.SetChildIndex(this.labelBrandRefno, 0);
            this.Controls.SetChildIndex(this.labelColor, 0);
            this.Controls.SetChildIndex(this.labelArriveQty, 0);
            this.Controls.SetChildIndex(this.labelArriveWHDate, 0);
            this.Controls.SetChildIndex(this.labelLastInspectionDate, 0);
            this.Controls.SetChildIndex(this.labelResult, 0);
            this.Controls.SetChildIndex(this.txtSP, 0);
            this.Controls.SetChildIndex(this.txtWkno, 0);
            this.Controls.SetChildIndex(this.txtStyle, 0);
            this.Controls.SetChildIndex(this.txtBrand, 0);
            this.Controls.SetChildIndex(this.txtSEQ, 0);
            this.Controls.SetChildIndex(this.txtSCIRefno, 0);
            this.Controls.SetChildIndex(this.txtBrandRefno, 0);
            this.Controls.SetChildIndex(this.txtColor, 0);
            this.Controls.SetChildIndex(this.txtArriveQty, 0);
            this.Controls.SetChildIndex(this.txtResult, 0);
            this.Controls.SetChildIndex(this.btnEncode, 0);
            this.Controls.SetChildIndex(this.checkNA, 0);
            this.Controls.SetChildIndex(this.dateArriveWHDate, 0);
            this.Controls.SetChildIndex(this.dateLastInspectionDate, 0);
            this.Controls.SetChildIndex(this.txtsupplierSupp, 0);
            this.Controls.SetChildIndex(this.labelDescription, 0);
            this.Controls.SetChildIndex(this.editDescription, 0);
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.labinspector, 0);
            this.Controls.SetChildIndex(this.txtCrockingInspector, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSP;
        private Win.UI.Label labelWkno;
        private Win.UI.Label labelStyle;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelSEQ;
        private Win.UI.Label labelSupp;
        private Win.UI.Label labelSCIRefno;
        private Win.UI.Label labelBrandRefno;
        private Win.UI.Label labelColor;
        private Win.UI.Label labelArriveQty;
        private Win.UI.Label labelArriveWHDate;
        private Win.UI.Label labelLastInspectionDate;
        private Win.UI.Label labelResult;
        private Win.UI.TextBox txtSP;
        private Win.UI.TextBox txtWkno;
        private Win.UI.TextBox txtStyle;
        private Win.UI.TextBox txtBrand;
        private Win.UI.TextBox txtSEQ;
        private Win.UI.TextBox txtSCIRefno;
        private Win.UI.TextBox txtBrandRefno;
        private Win.UI.TextBox txtColor;
        private Win.UI.TextBox txtArriveQty;
        private Win.UI.TextBox txtResult;
        private Win.UI.Button btnEncode;
        private Win.UI.CheckBox checkNA;
        private Win.UI.Button btnToExcel;
        private Win.UI.DateBox dateArriveWHDate;
        private Win.UI.DateBox dateLastInspectionDate;
        private Class.Txtsupplier txtsupplierSupp;
        private Win.UI.Label labelDescription;
        private Win.UI.EditBox editDescription;
        private Win.UI.Button btntoPDF;
        private Win.UI.TextBox txtCrockingInspector;
        private Win.UI.Label labinspector;
    }
}
