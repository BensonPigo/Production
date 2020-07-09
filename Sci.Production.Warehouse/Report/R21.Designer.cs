namespace Sci.Production.Warehouse
{
    partial class R21
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
            this.rdbtnSummary = new Sci.Win.UI.RadioButton();
            this.rdbtnDetail = new Sci.Win.UI.RadioButton();
            this.cmbStockType = new Sci.Win.UI.ComboBox();
            this.cmbMaterialType = new Sci.Win.UI.ComboBox();
            this.labelReportType = new Sci.Win.UI.Label();
            this.labelStockType = new Sci.Win.UI.Label();
            this.labelMaterialType = new Sci.Win.UI.Label();
            this.textColor = new Sci.Win.UI.TextBox();
            this.labelColor = new Sci.Win.UI.Label();
            this.checkQty = new Sci.Win.UI.CheckBox();
            this.txtfactory1 = new Sci.Production.Class.Txtfactory();
            this.label2 = new Sci.Win.UI.Label();
            this.textEndRefno = new Sci.Win.UI.TextBox();
            this.textStartRefno = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtMdivision1 = new Sci.Production.Class.TxtMdivision();
            this.textEndSP = new Sci.Win.UI.TextBox();
            this.textStartSP = new Sci.Win.UI.TextBox();
            this.labelRefno = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.labBuyDelivery = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.dateETA = new Sci.Win.UI.DateRange();
            this.label4 = new Sci.Win.UI.Label();
            this.dateArriveDate = new Sci.Win.UI.DateRange();
            this.label5 = new Sci.Win.UI.Label();
            this.txtWorkNo = new Sci.Win.UI.TextBox();
            this.checkSMTL = new Sci.Win.UI.CheckBox();
            this.checkMaterial = new Sci.Win.UI.CheckBox();
            this.checkSample = new Sci.Win.UI.CheckBox();
            this.checkBulk = new Sci.Win.UI.CheckBox();
            this.labelCategory = new Sci.Win.UI.Label();
            this.chkComplete = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(437, 79);
            this.print.TabIndex = 18;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(437, 9);
            this.toexcel.TabIndex = 16;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(437, 45);
            this.close.TabIndex = 17;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(378, 122);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(394, 122);
            // 
            // rdbtnSummary
            // 
            this.rdbtnSummary.AutoSize = true;
            this.rdbtnSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtnSummary.Location = new System.Drawing.Point(194, 359);
            this.rdbtnSummary.Name = "rdbtnSummary";
            this.rdbtnSummary.Size = new System.Drawing.Size(85, 21);
            this.rdbtnSummary.TabIndex = 14;
            this.rdbtnSummary.Text = "Summary";
            this.rdbtnSummary.UseVisualStyleBackColor = true;
            // 
            // rdbtnDetail
            // 
            this.rdbtnDetail.AutoSize = true;
            this.rdbtnDetail.Checked = true;
            this.rdbtnDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtnDetail.Location = new System.Drawing.Point(126, 359);
            this.rdbtnDetail.Name = "rdbtnDetail";
            this.rdbtnDetail.Size = new System.Drawing.Size(62, 21);
            this.rdbtnDetail.TabIndex = 13;
            this.rdbtnDetail.TabStop = true;
            this.rdbtnDetail.Text = "Detail";
            this.rdbtnDetail.UseVisualStyleBackColor = true;
            // 
            // cmbStockType
            // 
            this.cmbStockType.BackColor = System.Drawing.Color.White;
            this.cmbStockType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbStockType.FormattingEnabled = true;
            this.cmbStockType.IsSupportUnselect = true;
            this.cmbStockType.Location = new System.Drawing.Point(126, 299);
            this.cmbStockType.Name = "cmbStockType";
            this.cmbStockType.OldText = "";
            this.cmbStockType.Size = new System.Drawing.Size(117, 24);
            this.cmbStockType.TabIndex = 12;
            // 
            // cmbMaterialType
            // 
            this.cmbMaterialType.BackColor = System.Drawing.Color.White;
            this.cmbMaterialType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbMaterialType.FormattingEnabled = true;
            this.cmbMaterialType.IsSupportUnselect = true;
            this.cmbMaterialType.Location = new System.Drawing.Point(126, 269);
            this.cmbMaterialType.Name = "cmbMaterialType";
            this.cmbMaterialType.OldText = "";
            this.cmbMaterialType.Size = new System.Drawing.Size(117, 24);
            this.cmbMaterialType.TabIndex = 11;
            // 
            // labelReportType
            // 
            this.labelReportType.Location = new System.Drawing.Point(8, 359);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(114, 23);
            this.labelReportType.TabIndex = 130;
            this.labelReportType.Text = "Report Type";
            // 
            // labelStockType
            // 
            this.labelStockType.Location = new System.Drawing.Point(8, 299);
            this.labelStockType.Name = "labelStockType";
            this.labelStockType.Size = new System.Drawing.Size(114, 23);
            this.labelStockType.TabIndex = 129;
            this.labelStockType.Text = "Stock Type";
            // 
            // labelMaterialType
            // 
            this.labelMaterialType.Location = new System.Drawing.Point(8, 269);
            this.labelMaterialType.Name = "labelMaterialType";
            this.labelMaterialType.Size = new System.Drawing.Size(114, 23);
            this.labelMaterialType.TabIndex = 128;
            this.labelMaterialType.Text = "Material Type";
            // 
            // textColor
            // 
            this.textColor.BackColor = System.Drawing.Color.White;
            this.textColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textColor.Location = new System.Drawing.Point(126, 240);
            this.textColor.MaxLength = 6;
            this.textColor.Name = "textColor";
            this.textColor.Size = new System.Drawing.Size(66, 23);
            this.textColor.TabIndex = 10;
            // 
            // labelColor
            // 
            this.labelColor.Location = new System.Drawing.Point(8, 240);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(114, 23);
            this.labelColor.TabIndex = 126;
            this.labelColor.Text = "Color";
            // 
            // checkQty
            // 
            this.checkQty.AutoSize = true;
            this.checkQty.Checked = true;
            this.checkQty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkQty.Location = new System.Drawing.Point(8, 389);
            this.checkQty.Name = "checkQty";
            this.checkQty.Size = new System.Drawing.Size(73, 21);
            this.checkQty.TabIndex = 15;
            this.checkQty.Text = "Qty > 0";
            this.checkQty.UseVisualStyleBackColor = true;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.BoolFtyGroupList = true;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IsProduceFty = false;
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(126, 149);
            this.txtfactory1.MaxLength = 8;
            this.txtfactory1.MDivision = null;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(275, 178);
            this.label2.Name = "label2";
            this.label2.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.RectStyle.Color = System.Drawing.Color.Transparent;
            this.label2.RectStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label2.Size = new System.Drawing.Size(22, 23);
            this.label2.TabIndex = 125;
            this.label2.Text = "～";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // textEndRefno
            // 
            this.textEndRefno.BackColor = System.Drawing.Color.White;
            this.textEndRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textEndRefno.Location = new System.Drawing.Point(300, 178);
            this.textEndRefno.MaxLength = 20;
            this.textEndRefno.Name = "textEndRefno";
            this.textEndRefno.Size = new System.Drawing.Size(146, 23);
            this.textEndRefno.TabIndex = 8;
            // 
            // textStartRefno
            // 
            this.textStartRefno.BackColor = System.Drawing.Color.White;
            this.textStartRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textStartRefno.Location = new System.Drawing.Point(126, 178);
            this.textStartRefno.MaxLength = 20;
            this.textStartRefno.Name = "textStartRefno";
            this.textStartRefno.Size = new System.Drawing.Size(146, 23);
            this.textStartRefno.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(251, 9);
            this.label1.Name = "label1";
            this.label1.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.RectStyle.Color = System.Drawing.Color.Transparent;
            this.label1.RectStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label1.Size = new System.Drawing.Size(22, 23);
            this.label1.TabIndex = 124;
            this.label1.Text = "～";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(126, 120);
            this.txtMdivision1.MaxLength = 8;
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(67, 23);
            this.txtMdivision1.TabIndex = 5;
            // 
            // textEndSP
            // 
            this.textEndSP.BackColor = System.Drawing.Color.White;
            this.textEndSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textEndSP.Location = new System.Drawing.Point(276, 9);
            this.textEndSP.MaxLength = 13;
            this.textEndSP.Name = "textEndSP";
            this.textEndSP.Size = new System.Drawing.Size(121, 23);
            this.textEndSP.TabIndex = 1;
            // 
            // textStartSP
            // 
            this.textStartSP.BackColor = System.Drawing.Color.White;
            this.textStartSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textStartSP.Location = new System.Drawing.Point(126, 9);
            this.textStartSP.MaxLength = 13;
            this.textStartSP.Name = "textStartSP";
            this.textStartSP.Size = new System.Drawing.Size(121, 23);
            this.textStartSP.TabIndex = 0;
            // 
            // labelRefno
            // 
            this.labelRefno.Location = new System.Drawing.Point(8, 178);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelRefno.Size = new System.Drawing.Size(114, 23);
            this.labelRefno.TabIndex = 121;
            this.labelRefno.Text = "Refno";
            this.labelRefno.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(8, 149);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(114, 23);
            this.labelFactory.TabIndex = 120;
            this.labelFactory.Text = "Factory";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(8, 120);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(114, 23);
            this.labelM.TabIndex = 122;
            this.labelM.Text = "M";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(9, 9);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(114, 23);
            this.labelSPNo.TabIndex = 123;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(126, 93);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 4;
            // 
            // labBuyDelivery
            // 
            this.labBuyDelivery.Location = new System.Drawing.Point(9, 93);
            this.labBuyDelivery.Name = "labBuyDelivery";
            this.labBuyDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labBuyDelivery.Size = new System.Drawing.Size(114, 23);
            this.labBuyDelivery.TabIndex = 136;
            this.labBuyDelivery.Text = "Buyer Delivery";
            this.labBuyDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 36);
            this.label3.Name = "label3";
            this.label3.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label3.Size = new System.Drawing.Size(114, 23);
            this.label3.TabIndex = 138;
            this.label3.Text = "ETA";
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateETA
            // 
            // 
            // 
            // 
            this.dateETA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateETA.DateBox1.Name = "";
            this.dateETA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateETA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateETA.DateBox2.Name = "";
            this.dateETA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox2.TabIndex = 1;
            this.dateETA.IsRequired = false;
            this.dateETA.Location = new System.Drawing.Point(126, 36);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(280, 23);
            this.dateETA.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 65);
            this.label4.Name = "label4";
            this.label4.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label4.Size = new System.Drawing.Size(114, 23);
            this.label4.TabIndex = 140;
            this.label4.Text = "Arrive W/H Date";
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateArriveDate
            // 
            // 
            // 
            // 
            this.dateArriveDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateArriveDate.DateBox1.Name = "";
            this.dateArriveDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateArriveDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateArriveDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateArriveDate.DateBox2.Name = "";
            this.dateArriveDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateArriveDate.DateBox2.TabIndex = 1;
            this.dateArriveDate.IsRequired = false;
            this.dateArriveDate.Location = new System.Drawing.Point(126, 65);
            this.dateArriveDate.Name = "dateArriveDate";
            this.dateArriveDate.Size = new System.Drawing.Size(280, 23);
            this.dateArriveDate.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 23);
            this.label5.TabIndex = 141;
            this.label5.Text = "WK#";
            // 
            // txtWorkNo
            // 
            this.txtWorkNo.BackColor = System.Drawing.Color.White;
            this.txtWorkNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWorkNo.Location = new System.Drawing.Point(126, 208);
            this.txtWorkNo.MaxLength = 13;
            this.txtWorkNo.Name = "txtWorkNo";
            this.txtWorkNo.Size = new System.Drawing.Size(121, 23);
            this.txtWorkNo.TabIndex = 9;
            // 
            // checkSMTL
            // 
            this.checkSMTL.AutoSize = true;
            this.checkSMTL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkSMTL.Location = new System.Drawing.Point(345, 331);
            this.checkSMTL.Name = "checkSMTL";
            this.checkSMTL.Size = new System.Drawing.Size(64, 21);
            this.checkSMTL.TabIndex = 147;
            this.checkSMTL.Text = "SMTL";
            this.checkSMTL.UseVisualStyleBackColor = true;
            // 
            // checkMaterial
            // 
            this.checkMaterial.AutoSize = true;
            this.checkMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkMaterial.Location = new System.Drawing.Point(267, 331);
            this.checkMaterial.Name = "checkMaterial";
            this.checkMaterial.Size = new System.Drawing.Size(77, 21);
            this.checkMaterial.TabIndex = 144;
            this.checkMaterial.Text = "Material";
            this.checkMaterial.UseVisualStyleBackColor = true;
            // 
            // checkSample
            // 
            this.checkSample.AutoSize = true;
            this.checkSample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkSample.Location = new System.Drawing.Point(187, 331);
            this.checkSample.Name = "checkSample";
            this.checkSample.Size = new System.Drawing.Size(74, 21);
            this.checkSample.TabIndex = 143;
            this.checkSample.Text = "Sample";
            this.checkSample.UseVisualStyleBackColor = true;
            // 
            // checkBulk
            // 
            this.checkBulk.AutoSize = true;
            this.checkBulk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBulk.Location = new System.Drawing.Point(126, 331);
            this.checkBulk.Name = "checkBulk";
            this.checkBulk.Size = new System.Drawing.Size(54, 21);
            this.checkBulk.TabIndex = 142;
            this.checkBulk.Text = "Bulk";
            this.checkBulk.UseVisualStyleBackColor = true;
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(8, 329);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(114, 23);
            this.labelCategory.TabIndex = 148;
            this.labelCategory.Text = "Category";
            // 
            // chkComplete
            // 
            this.chkComplete.AutoSize = true;
            this.chkComplete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkComplete.Location = new System.Drawing.Point(87, 389);
            this.chkComplete.Name = "chkComplete";
            this.chkComplete.Size = new System.Drawing.Size(138, 21);
            this.chkComplete.TabIndex = 149;
            this.chkComplete.Text = "Material complete";
            this.chkComplete.UseVisualStyleBackColor = true;
            // 
            // R21
            // 
            this.ClientSize = new System.Drawing.Size(527, 441);
            this.Controls.Add(this.chkComplete);
            this.Controls.Add(this.checkSMTL);
            this.Controls.Add(this.checkMaterial);
            this.Controls.Add(this.checkSample);
            this.Controls.Add(this.checkBulk);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.txtWorkNo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateArriveDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateETA);
            this.Controls.Add(this.labBuyDelivery);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.rdbtnSummary);
            this.Controls.Add(this.rdbtnDetail);
            this.Controls.Add(this.cmbStockType);
            this.Controls.Add(this.cmbMaterialType);
            this.Controls.Add(this.labelReportType);
            this.Controls.Add(this.labelStockType);
            this.Controls.Add(this.labelMaterialType);
            this.Controls.Add(this.textColor);
            this.Controls.Add(this.labelColor);
            this.Controls.Add(this.checkQty);
            this.Controls.Add(this.txtfactory1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textEndRefno);
            this.Controls.Add(this.textStartRefno);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMdivision1);
            this.Controls.Add(this.textEndSP);
            this.Controls.Add(this.textStartSP);
            this.Controls.Add(this.labelRefno);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelSPNo);
            this.Name = "R21";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R21 Stock List Report (Inventory)";
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelRefno, 0);
            this.Controls.SetChildIndex(this.textStartSP, 0);
            this.Controls.SetChildIndex(this.textEndSP, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.textStartRefno, 0);
            this.Controls.SetChildIndex(this.textEndRefno, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtfactory1, 0);
            this.Controls.SetChildIndex(this.checkQty, 0);
            this.Controls.SetChildIndex(this.labelColor, 0);
            this.Controls.SetChildIndex(this.textColor, 0);
            this.Controls.SetChildIndex(this.labelMaterialType, 0);
            this.Controls.SetChildIndex(this.labelStockType, 0);
            this.Controls.SetChildIndex(this.labelReportType, 0);
            this.Controls.SetChildIndex(this.cmbMaterialType, 0);
            this.Controls.SetChildIndex(this.cmbStockType, 0);
            this.Controls.SetChildIndex(this.rdbtnDetail, 0);
            this.Controls.SetChildIndex(this.rdbtnSummary, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labBuyDelivery, 0);
            this.Controls.SetChildIndex(this.dateETA, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.dateArriveDate, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.txtWorkNo, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.checkBulk, 0);
            this.Controls.SetChildIndex(this.checkSample, 0);
            this.Controls.SetChildIndex(this.checkMaterial, 0);
            this.Controls.SetChildIndex(this.checkSMTL, 0);
            this.Controls.SetChildIndex(this.chkComplete, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioButton rdbtnSummary;
        private Win.UI.RadioButton rdbtnDetail;
        private Win.UI.ComboBox cmbStockType;
        private Win.UI.ComboBox cmbMaterialType;
        private Win.UI.Label labelReportType;
        private Win.UI.Label labelStockType;
        private Win.UI.Label labelMaterialType;
        private Win.UI.TextBox textColor;
        private Win.UI.Label labelColor;
        private Win.UI.CheckBox checkQty;
        private Class.Txtfactory txtfactory1;
        private Win.UI.Label label2;
        private Win.UI.TextBox textEndRefno;
        private Win.UI.TextBox textStartRefno;
        private Win.UI.Label label1;
        private Class.TxtMdivision txtMdivision1;
        private Win.UI.TextBox textEndSP;
        private Win.UI.TextBox textStartSP;
        private Win.UI.Label labelRefno;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelM;
        private Win.UI.Label labelSPNo;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Label labBuyDelivery;
        private Win.UI.Label label3;
        private Win.UI.DateRange dateETA;
        private Win.UI.Label label4;
        private Win.UI.DateRange dateArriveDate;
        private Win.UI.Label label5;
        private Win.UI.TextBox txtWorkNo;
        private Win.UI.CheckBox checkSMTL;
        private Win.UI.CheckBox checkMaterial;
        private Win.UI.CheckBox checkSample;
        private Win.UI.CheckBox checkBulk;
        private Win.UI.Label labelCategory;
        private Win.UI.CheckBox chkComplete;
    }
}
