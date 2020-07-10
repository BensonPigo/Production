namespace Sci.Production.Quality
{
    partial class P03_Wash
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
            this.btnToExcel = new Sci.Win.UI.Button();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.labelDescription = new Sci.Win.UI.Label();
            this.txtsupplierSupp = new Sci.Production.Class.Txtsupplier();
            this.dateLastInspectionDate = new Sci.Win.UI.DateBox();
            this.dateArriveWHDate = new Sci.Win.UI.DateBox();
            this.checkNA = new Sci.Win.UI.CheckBox();
            this.btnEncode = new Sci.Win.UI.Button();
            this.txtResult = new Sci.Win.UI.TextBox();
            this.txtArriveQty = new Sci.Win.UI.TextBox();
            this.txtColor = new Sci.Win.UI.TextBox();
            this.txtBrandRefno = new Sci.Win.UI.TextBox();
            this.txtSCIRefno = new Sci.Win.UI.TextBox();
            this.txtSEQ = new Sci.Win.UI.TextBox();
            this.txtBrand = new Sci.Win.UI.TextBox();
            this.txtStyle = new Sci.Win.UI.TextBox();
            this.txtWkno = new Sci.Win.UI.TextBox();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.labelResult = new Sci.Win.UI.Label();
            this.labelLastInspectionDate = new Sci.Win.UI.Label();
            this.labelArriveWHDate = new Sci.Win.UI.Label();
            this.labelArriveQty = new Sci.Win.UI.Label();
            this.labelColor = new Sci.Win.UI.Label();
            this.labelBrandRefno = new Sci.Win.UI.Label();
            this.labelSCIRefno = new Sci.Win.UI.Label();
            this.labelSupp = new Sci.Win.UI.Label();
            this.labelSEQ = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelWkno = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioOption1 = new Sci.Win.UI.RadioButton();
            this.radioOption2 = new Sci.Win.UI.RadioButton();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.labinspector = new Sci.Win.UI.Label();
            this.txtWashInspector = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.radioPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.btnToExcel);
            this.btmcont.Location = new System.Drawing.Point(0, 556);
            this.btmcont.Size = new System.Drawing.Size(962, 41);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.btnToExcel, 0);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 215);
            this.gridcont.Size = new System.Drawing.Size(938, 335);
            // 
            // append
            // 
            this.append.Size = new System.Drawing.Size(80, 31);
            // 
            // revise
            // 
            this.revise.Size = new System.Drawing.Size(80, 31);
            // 
            // delete
            // 
            this.delete.Size = new System.Drawing.Size(80, 31);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(872, 5);
            this.undo.Size = new System.Drawing.Size(80, 31);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(792, 5);
            this.save.Size = new System.Drawing.Size(80, 31);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnToExcel.Location = new System.Drawing.Point(706, 5);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 31);
            this.btnToExcel.TabIndex = 95;
            this.btnToExcel.Text = "ToExcel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.btnToExcel_Click);
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editDescription.Location = new System.Drawing.Point(90, 152);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.ReadOnly = true;
            this.editDescription.Size = new System.Drawing.Size(758, 57);
            this.editDescription.TabIndex = 218;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(12, 152);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 25);
            this.labelDescription.TabIndex = 217;
            this.labelDescription.Text = "Description";
            // 
            // txtsupplierSupp
            // 
            this.txtsupplierSupp.DisplayBox1Binding = "";
            this.txtsupplierSupp.Location = new System.Drawing.Point(276, 38);
            this.txtsupplierSupp.Name = "txtsupplierSupp";
            this.txtsupplierSupp.Size = new System.Drawing.Size(145, 23);
            this.txtsupplierSupp.TabIndex = 216;
            this.txtsupplierSupp.TextBox1Binding = "";
            // 
            // dateLastInspectionDate
            // 
            this.dateLastInspectionDate.IsSupportEditMode = false;
            this.dateLastInspectionDate.Location = new System.Drawing.Point(610, 96);
            this.dateLastInspectionDate.Name = "dateLastInspectionDate";
            this.dateLastInspectionDate.ReadOnly = true;
            this.dateLastInspectionDate.Size = new System.Drawing.Size(155, 23);
            this.dateLastInspectionDate.TabIndex = 215;
            // 
            // dateArriveWHDate
            // 
            this.dateArriveWHDate.IsSupportEditMode = false;
            this.dateArriveWHDate.Location = new System.Drawing.Point(610, 67);
            this.dateArriveWHDate.Name = "dateArriveWHDate";
            this.dateArriveWHDate.ReadOnly = true;
            this.dateArriveWHDate.Size = new System.Drawing.Size(155, 23);
            this.dateArriveWHDate.TabIndex = 214;
            // 
            // checkNA
            // 
            this.checkNA.AutoSize = true;
            this.checkNA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkNA.IsSupportEditMode = false;
            this.checkNA.Location = new System.Drawing.Point(884, 157);
            this.checkNA.Name = "checkNA";
            this.checkNA.ReadOnly = true;
            this.checkNA.Size = new System.Drawing.Size(50, 21);
            this.checkNA.TabIndex = 213;
            this.checkNA.Text = "N/A";
            this.checkNA.UseVisualStyleBackColor = true;
            // 
            // btnEncode
            // 
            this.btnEncode.Location = new System.Drawing.Point(854, 179);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(80, 30);
            this.btnEncode.TabIndex = 212;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtResult.IsSupportEditMode = false;
            this.txtResult.Location = new System.Drawing.Point(553, 38);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(110, 23);
            this.txtResult.TabIndex = 211;
            // 
            // txtArriveQty
            // 
            this.txtArriveQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtArriveQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtArriveQty.IsSupportEditMode = false;
            this.txtArriveQty.Location = new System.Drawing.Point(552, 9);
            this.txtArriveQty.Name = "txtArriveQty";
            this.txtArriveQty.ReadOnly = true;
            this.txtArriveQty.Size = new System.Drawing.Size(111, 23);
            this.txtArriveQty.TabIndex = 210;
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtColor.IsSupportEditMode = false;
            this.txtColor.Location = new System.Drawing.Point(362, 9);
            this.txtColor.Name = "txtColor";
            this.txtColor.ReadOnly = true;
            this.txtColor.Size = new System.Drawing.Size(102, 23);
            this.txtColor.TabIndex = 209;
            // 
            // txtBrandRefno
            // 
            this.txtBrandRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtBrandRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtBrandRefno.IsSupportEditMode = false;
            this.txtBrandRefno.Location = new System.Drawing.Point(277, 96);
            this.txtBrandRefno.Name = "txtBrandRefno";
            this.txtBrandRefno.ReadOnly = true;
            this.txtBrandRefno.Size = new System.Drawing.Size(187, 23);
            this.txtBrandRefno.TabIndex = 208;
            // 
            // txtSCIRefno
            // 
            this.txtSCIRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSCIRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSCIRefno.IsSupportEditMode = false;
            this.txtSCIRefno.Location = new System.Drawing.Point(276, 67);
            this.txtSCIRefno.Name = "txtSCIRefno";
            this.txtSCIRefno.ReadOnly = true;
            this.txtSCIRefno.Size = new System.Drawing.Size(188, 23);
            this.txtSCIRefno.TabIndex = 207;
            // 
            // txtSEQ
            // 
            this.txtSEQ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSEQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSEQ.IsSupportEditMode = false;
            this.txtSEQ.Location = new System.Drawing.Point(241, 9);
            this.txtSEQ.Name = "txtSEQ";
            this.txtSEQ.ReadOnly = true;
            this.txtSEQ.Size = new System.Drawing.Size(60, 23);
            this.txtSEQ.TabIndex = 206;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtBrand.IsSupportEditMode = false;
            this.txtBrand.Location = new System.Drawing.Point(61, 96);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.ReadOnly = true;
            this.txtBrand.Size = new System.Drawing.Size(115, 23);
            this.txtBrand.TabIndex = 205;
            // 
            // txtStyle
            // 
            this.txtStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtStyle.IsSupportEditMode = false;
            this.txtStyle.Location = new System.Drawing.Point(61, 67);
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.ReadOnly = true;
            this.txtStyle.Size = new System.Drawing.Size(115, 23);
            this.txtStyle.TabIndex = 204;
            // 
            // txtWkno
            // 
            this.txtWkno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtWkno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtWkno.IsSupportEditMode = false;
            this.txtWkno.Location = new System.Drawing.Point(61, 38);
            this.txtWkno.Name = "txtWkno";
            this.txtWkno.ReadOnly = true;
            this.txtWkno.Size = new System.Drawing.Size(115, 23);
            this.txtWkno.TabIndex = 203;
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSP.IsSupportEditMode = false;
            this.txtSP.Location = new System.Drawing.Point(61, 9);
            this.txtSP.Name = "txtSP";
            this.txtSP.ReadOnly = true;
            this.txtSP.Size = new System.Drawing.Size(115, 23);
            this.txtSP.TabIndex = 202;
            // 
            // labelResult
            // 
            this.labelResult.Location = new System.Drawing.Point(475, 38);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(75, 23);
            this.labelResult.TabIndex = 201;
            this.labelResult.Text = "Result";
            // 
            // labelLastInspectionDate
            // 
            this.labelLastInspectionDate.Location = new System.Drawing.Point(475, 96);
            this.labelLastInspectionDate.Name = "labelLastInspectionDate";
            this.labelLastInspectionDate.Size = new System.Drawing.Size(132, 23);
            this.labelLastInspectionDate.TabIndex = 200;
            this.labelLastInspectionDate.Text = "Last Inspection Date ";
            // 
            // labelArriveWHDate
            // 
            this.labelArriveWHDate.Location = new System.Drawing.Point(475, 67);
            this.labelArriveWHDate.Name = "labelArriveWHDate";
            this.labelArriveWHDate.Size = new System.Drawing.Size(132, 23);
            this.labelArriveWHDate.TabIndex = 199;
            this.labelArriveWHDate.Text = "Arrive W/H Date";
            // 
            // labelArriveQty
            // 
            this.labelArriveQty.Location = new System.Drawing.Point(475, 9);
            this.labelArriveQty.Name = "labelArriveQty";
            this.labelArriveQty.Size = new System.Drawing.Size(75, 23);
            this.labelArriveQty.TabIndex = 198;
            this.labelArriveQty.Text = "Arrive Qty";
            // 
            // labelColor
            // 
            this.labelColor.Location = new System.Drawing.Point(312, 9);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(47, 23);
            this.labelColor.TabIndex = 197;
            this.labelColor.Text = "Color";
            // 
            // labelBrandRefno
            // 
            this.labelBrandRefno.Location = new System.Drawing.Point(187, 96);
            this.labelBrandRefno.Name = "labelBrandRefno";
            this.labelBrandRefno.Size = new System.Drawing.Size(86, 25);
            this.labelBrandRefno.TabIndex = 196;
            this.labelBrandRefno.Text = "Brand Refno";
            // 
            // labelSCIRefno
            // 
            this.labelSCIRefno.Location = new System.Drawing.Point(187, 67);
            this.labelSCIRefno.Name = "labelSCIRefno";
            this.labelSCIRefno.Size = new System.Drawing.Size(86, 23);
            this.labelSCIRefno.TabIndex = 195;
            this.labelSCIRefno.Text = "SCI Refno";
            // 
            // labelSupp
            // 
            this.labelSupp.Location = new System.Drawing.Point(187, 38);
            this.labelSupp.Name = "labelSupp";
            this.labelSupp.Size = new System.Drawing.Size(86, 23);
            this.labelSupp.TabIndex = 194;
            this.labelSupp.Text = "Supp";
            // 
            // labelSEQ
            // 
            this.labelSEQ.Location = new System.Drawing.Point(187, 9);
            this.labelSEQ.Name = "labelSEQ";
            this.labelSEQ.Size = new System.Drawing.Size(51, 23);
            this.labelSEQ.TabIndex = 193;
            this.labelSEQ.Text = "SEQ#";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(12, 96);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(46, 23);
            this.labelBrand.TabIndex = 192;
            this.labelBrand.Text = "Brand";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(12, 68);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(46, 23);
            this.labelStyle.TabIndex = 191;
            this.labelStyle.Text = "Style#";
            // 
            // labelWkno
            // 
            this.labelWkno.Location = new System.Drawing.Point(12, 38);
            this.labelWkno.Name = "labelWkno";
            this.labelWkno.Size = new System.Drawing.Size(46, 23);
            this.labelWkno.TabIndex = 190;
            this.labelWkno.Text = "Wkno";
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(12, 9);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(46, 23);
            this.labelSP.TabIndex = 189;
            this.labelSP.Text = "SP#";
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioOption1);
            this.radioPanel1.Controls.Add(this.radioOption2);
            this.radioPanel1.Location = new System.Drawing.Point(669, 4);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.ReadOnly = true;
            this.radioPanel1.Size = new System.Drawing.Size(96, 57);
            this.radioPanel1.TabIndex = 219;
            this.radioPanel1.ValueChanged += new System.EventHandler(this.radioPanel1_ValueChanged);
            // 
            // radioOption1
            // 
            this.radioOption1.AutoCheck = false;
            this.radioOption1.AutoSize = true;
            this.radioOption1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.radioOption1.Location = new System.Drawing.Point(9, 4);
            this.radioOption1.Name = "radioOption1";
            this.radioOption1.Size = new System.Drawing.Size(76, 21);
            this.radioOption1.TabIndex = 6;
            this.radioOption1.Text = "Option1";
            this.radioOption1.UseVisualStyleBackColor = true;
            this.radioOption1.Value = "1";
            // 
            // radioOption2
            // 
            this.radioOption2.AutoCheck = false;
            this.radioOption2.AutoSize = true;
            this.radioOption2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.radioOption2.Location = new System.Drawing.Point(9, 31);
            this.radioOption2.Name = "radioOption2";
            this.radioOption2.Size = new System.Drawing.Size(76, 21);
            this.radioOption2.TabIndex = 7;
            this.radioOption2.Text = "Option2";
            this.radioOption2.UseVisualStyleBackColor = true;
            this.radioOption2.Value = "2";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = null;
            this.pictureBox1.Location = new System.Drawing.Point(775, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(163, 136);
            this.pictureBox1.TabIndex = 220;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            // 
            // labinspector
            // 
            this.labinspector.Location = new System.Drawing.Point(475, 125);
            this.labinspector.Name = "labinspector";
            this.labinspector.Size = new System.Drawing.Size(132, 23);
            this.labinspector.TabIndex = 221;
            this.labinspector.Text = "Wash Inspector";
            // 
            // txtWashInspector
            // 
            this.txtWashInspector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtWashInspector.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtWashInspector.IsSupportEditMode = false;
            this.txtWashInspector.Location = new System.Drawing.Point(610, 125);
            this.txtWashInspector.Name = "txtWashInspector";
            this.txtWashInspector.ReadOnly = true;
            this.txtWashInspector.Size = new System.Drawing.Size(155, 23);
            this.txtWashInspector.TabIndex = 222;
            // 
            // P03_Wash
            // 
            this.ClientSize = new System.Drawing.Size(962, 597);
            this.Controls.Add(this.txtWashInspector);
            this.Controls.Add(this.labinspector);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.radioPanel1);
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
            this.KeyField1 = "ID";
            this.Name = "P03_Wash";
            this.Text = "Wash Test";
            this.WorkAlias = "FIR_Laboratory_Wash";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
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
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.labinspector, 0);
            this.Controls.SetChildIndex(this.txtWashInspector, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button btnToExcel;
        private Win.UI.EditBox editDescription;
        private Win.UI.Label labelDescription;
        private Class.Txtsupplier txtsupplierSupp;
        private Win.UI.DateBox dateLastInspectionDate;
        private Win.UI.DateBox dateArriveWHDate;
        private Win.UI.CheckBox checkNA;
        private Win.UI.Button btnEncode;
        private Win.UI.TextBox txtResult;
        private Win.UI.TextBox txtArriveQty;
        private Win.UI.TextBox txtColor;
        private Win.UI.TextBox txtBrandRefno;
        private Win.UI.TextBox txtSCIRefno;
        private Win.UI.TextBox txtSEQ;
        private Win.UI.TextBox txtBrand;
        private Win.UI.TextBox txtStyle;
        private Win.UI.TextBox txtWkno;
        private Win.UI.TextBox txtSP;
        private Win.UI.Label labelResult;
        private Win.UI.Label labelLastInspectionDate;
        private Win.UI.Label labelArriveWHDate;
        private Win.UI.Label labelArriveQty;
        private Win.UI.Label labelColor;
        private Win.UI.Label labelBrandRefno;
        private Win.UI.Label labelSCIRefno;
        private Win.UI.Label labelSupp;
        private Win.UI.Label labelSEQ;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelStyle;
        private Win.UI.Label labelWkno;
        private Win.UI.Label labelSP;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioOption1;
        private Win.UI.RadioButton radioOption2;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.Label labinspector;
        private Win.UI.TextBox txtWashInspector;
    }
}
