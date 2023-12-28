namespace Sci.Production.Shipping
{
    partial class P05_MercuryPostScanShipment
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new Sci.Win.UI.Label();
            this.displayShipmentNo = new Sci.Win.UI.DisplayBox();
            this.btnCreateUpdateShipment = new Sci.Win.UI.Button();
            this.btnEditSave = new Sci.Win.UI.Button();
            this.btnCloseUnDo = new Sci.Win.UI.Button();
            this.GroupShippingInfo = new Sci.Win.UI.RadioGroup();
            this.displayPortOrginDesc = new Sci.Win.UI.DisplayBox();
            this.txtPortOrgin = new Sci.Win.UI.TextBox();
            this.dateShippingDate = new Sci.Win.UI.DateBox();
            this.txtTrackingContainer = new Sci.Win.UI.TextBox();
            this.comboLoadIndicator = new Sci.Win.UI.ComboBox();
            this.displayLSPDesc = new Sci.Win.UI.DisplayBox();
            this.displayNikeLSPCode = new Sci.Win.UI.DisplayBox();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.GroupFinancialInfo = new Sci.Win.UI.RadioGroup();
            this.dateFactoryInvoiceDate = new Sci.Win.UI.DateBox();
            this.displayFSPDesc = new Sci.Win.UI.DisplayBox();
            this.txtFSP = new Sci.Win.UI.TextBox();
            this.txtQAReferenceNbr = new Sci.Win.UI.TextBox();
            this.txtLCReferenceNbr = new Sci.Win.UI.TextBox();
            this.txtFactoryInvoiceNbr = new Sci.Win.UI.TextBox();
            this.txtLSPBookingNumber = new Sci.Win.UI.TextBox();
            this.label12 = new Sci.Win.UI.Label();
            this.label11 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.GroupDocumentsScanFile = new Sci.Win.UI.RadioGroup();
            this.chkMultipleMaterialsPackingList = new Sci.Win.UI.CheckBox();
            this.chkFactoryAddressPackingList = new Sci.Win.UI.CheckBox();
            this.chkPRELMINARY_Watermark = new Sci.Win.UI.CheckBox();
            this.chkGrossWeightPackingList = new Sci.Win.UI.CheckBox();
            this.chkTotalGrossWeightCommercialInvoice = new Sci.Win.UI.CheckBox();
            this.chkConsolidateInvoices = new Sci.Win.UI.CheckBox();
            this.comboSellerAddress = new Sci.Win.UI.ComboBox();
            this.comboFactoryAddress = new Sci.Win.UI.ComboBox();
            this.groupChoiceShippingDoc = new Sci.Win.UI.RadioGroup();
            this.chkTallySheet = new Sci.Win.UI.CheckBox();
            this.chkShipmentSummary = new Sci.Win.UI.CheckBox();
            this.chkCertificateOrigin = new Sci.Win.UI.CheckBox();
            this.chkCertificateInspection = new Sci.Win.UI.CheckBox();
            this.chkTradingCmpyPL = new Sci.Win.UI.CheckBox();
            this.chkPackingList = new Sci.Win.UI.CheckBox();
            this.chkTradingCmpyCI = new Sci.Win.UI.CheckBox();
            this.chkCommercialInvoice = new Sci.Win.UI.CheckBox();
            this.label14 = new Sci.Win.UI.Label();
            this.label13 = new Sci.Win.UI.Label();
            this.btnPrint = new Sci.Win.UI.Button();
            this.GroupShippingInfo.SuspendLayout();
            this.GroupFinancialInfo.SuspendLayout();
            this.GroupDocumentsScanFile.SuspendLayout();
            this.groupChoiceShippingDoc.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(17, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Shipment Nbr";
            // 
            // displayShipmentNo
            // 
            this.displayShipmentNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayShipmentNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayShipmentNo.Location = new System.Drawing.Point(170, 15);
            this.displayShipmentNo.Name = "displayShipmentNo";
            this.displayShipmentNo.Size = new System.Drawing.Size(139, 23);
            this.displayShipmentNo.TabIndex = 2;
            // 
            // btnCreateUpdateShipment
            // 
            this.btnCreateUpdateShipment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateUpdateShipment.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnCreateUpdateShipment.Location = new System.Drawing.Point(732, 11);
            this.btnCreateUpdateShipment.Name = "btnCreateUpdateShipment";
            this.btnCreateUpdateShipment.Size = new System.Drawing.Size(149, 30);
            this.btnCreateUpdateShipment.TabIndex = 3;
            this.btnCreateUpdateShipment.Text = "Create Shipment";
            this.btnCreateUpdateShipment.UseVisualStyleBackColor = true;
            this.btnCreateUpdateShipment.Click += new System.EventHandler(this.BtnCreateUpdateShipment_Click);
            // 
            // btnEditSave
            // 
            this.btnEditSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditSave.Location = new System.Drawing.Point(703, 496);
            this.btnEditSave.Name = "btnEditSave";
            this.btnEditSave.Size = new System.Drawing.Size(83, 30);
            this.btnEditSave.TabIndex = 4;
            this.btnEditSave.Text = "Edit";
            this.btnEditSave.UseVisualStyleBackColor = true;
            this.btnEditSave.Click += new System.EventHandler(this.BtnEditSave_Click);
            // 
            // btnCloseUnDo
            // 
            this.btnCloseUnDo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseUnDo.Location = new System.Drawing.Point(792, 496);
            this.btnCloseUnDo.Name = "btnCloseUnDo";
            this.btnCloseUnDo.Size = new System.Drawing.Size(83, 30);
            this.btnCloseUnDo.TabIndex = 5;
            this.btnCloseUnDo.Text = "Close";
            this.btnCloseUnDo.UseVisualStyleBackColor = true;
            this.btnCloseUnDo.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // GroupShippingInfo
            // 
            this.GroupShippingInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupShippingInfo.Controls.Add(this.displayPortOrginDesc);
            this.GroupShippingInfo.Controls.Add(this.txtPortOrgin);
            this.GroupShippingInfo.Controls.Add(this.dateShippingDate);
            this.GroupShippingInfo.Controls.Add(this.txtTrackingContainer);
            this.GroupShippingInfo.Controls.Add(this.comboLoadIndicator);
            this.GroupShippingInfo.Controls.Add(this.displayLSPDesc);
            this.GroupShippingInfo.Controls.Add(this.displayNikeLSPCode);
            this.GroupShippingInfo.Controls.Add(this.label6);
            this.GroupShippingInfo.Controls.Add(this.label5);
            this.GroupShippingInfo.Controls.Add(this.label4);
            this.GroupShippingInfo.Controls.Add(this.label3);
            this.GroupShippingInfo.Controls.Add(this.label2);
            this.GroupShippingInfo.Location = new System.Drawing.Point(9, 46);
            this.GroupShippingInfo.Name = "GroupShippingInfo";
            this.GroupShippingInfo.Size = new System.Drawing.Size(872, 101);
            this.GroupShippingInfo.TabIndex = 7;
            this.GroupShippingInfo.TabStop = false;
            this.GroupShippingInfo.Text = "Shipping Info.";
            // 
            // displayPortOrginDesc
            // 
            this.displayPortOrginDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPortOrginDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPortOrginDesc.Location = new System.Drawing.Point(224, 69);
            this.displayPortOrginDesc.Name = "displayPortOrginDesc";
            this.displayPortOrginDesc.Size = new System.Drawing.Size(153, 23);
            this.displayPortOrginDesc.TabIndex = 16;
            // 
            // txtPortOrgin
            // 
            this.txtPortOrgin.BackColor = System.Drawing.Color.White;
            this.txtPortOrgin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPortOrgin.Location = new System.Drawing.Point(161, 69);
            this.txtPortOrgin.Name = "txtPortOrgin";
            this.txtPortOrgin.Size = new System.Drawing.Size(62, 23);
            this.txtPortOrgin.TabIndex = 15;
            this.txtPortOrgin.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtPortOrgin_PopUp);
            this.txtPortOrgin.Validating += new System.ComponentModel.CancelEventHandler(this.TxtPortOrgin_Validating);
            // 
            // dateShippingDate
            // 
            this.dateShippingDate.Location = new System.Drawing.Point(161, 44);
            this.dateShippingDate.Name = "dateShippingDate";
            this.dateShippingDate.Size = new System.Drawing.Size(130, 23);
            this.dateShippingDate.TabIndex = 14;
            // 
            // txtTrackingContainer
            // 
            this.txtTrackingContainer.BackColor = System.Drawing.Color.White;
            this.txtTrackingContainer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTrackingContainer.Location = new System.Drawing.Point(540, 44);
            this.txtTrackingContainer.Name = "txtTrackingContainer";
            this.txtTrackingContainer.Size = new System.Drawing.Size(217, 23);
            this.txtTrackingContainer.TabIndex = 13;
            this.txtTrackingContainer.TextChanged += new System.EventHandler(this.OnlyEnglishNumber_TextChanged);
            // 
            // comboLoadIndicator
            // 
            this.comboLoadIndicator.BackColor = System.Drawing.Color.White;
            this.comboLoadIndicator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboLoadIndicator.FormattingEnabled = true;
            this.comboLoadIndicator.IsSupportUnselect = true;
            this.comboLoadIndicator.Location = new System.Drawing.Point(540, 18);
            this.comboLoadIndicator.Name = "comboLoadIndicator";
            this.comboLoadIndicator.OldText = "";
            this.comboLoadIndicator.Size = new System.Drawing.Size(61, 24);
            this.comboLoadIndicator.TabIndex = 12;
            // 
            // displayLSPDesc
            // 
            this.displayLSPDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayLSPDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayLSPDesc.Location = new System.Drawing.Point(224, 19);
            this.displayLSPDesc.Name = "displayLSPDesc";
            this.displayLSPDesc.Size = new System.Drawing.Size(153, 23);
            this.displayLSPDesc.TabIndex = 11;
            // 
            // displayNikeLSPCode
            // 
            this.displayNikeLSPCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayNikeLSPCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayNikeLSPCode.Location = new System.Drawing.Point(161, 19);
            this.displayNikeLSPCode.Name = "displayNikeLSPCode";
            this.displayNikeLSPCode.Size = new System.Drawing.Size(62, 23);
            this.displayNikeLSPCode.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 23);
            this.label6.TabIndex = 9;
            this.label6.Text = "Port of Origin";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 23);
            this.label5.TabIndex = 8;
            this.label5.Text = "Shipping Date";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.SkyBlue;
            this.label4.Location = new System.Drawing.Point(397, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Tracking#/Container#";
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.SkyBlue;
            this.label3.Location = new System.Drawing.Point(397, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Load Indicator";
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.SkyBlue;
            this.label2.Location = new System.Drawing.Point(8, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "LSP";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // GroupFinancialInfo
            // 
            this.GroupFinancialInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupFinancialInfo.Controls.Add(this.dateFactoryInvoiceDate);
            this.GroupFinancialInfo.Controls.Add(this.displayFSPDesc);
            this.GroupFinancialInfo.Controls.Add(this.txtFSP);
            this.GroupFinancialInfo.Controls.Add(this.txtQAReferenceNbr);
            this.GroupFinancialInfo.Controls.Add(this.txtLCReferenceNbr);
            this.GroupFinancialInfo.Controls.Add(this.txtFactoryInvoiceNbr);
            this.GroupFinancialInfo.Controls.Add(this.txtLSPBookingNumber);
            this.GroupFinancialInfo.Controls.Add(this.label12);
            this.GroupFinancialInfo.Controls.Add(this.label11);
            this.GroupFinancialInfo.Controls.Add(this.label10);
            this.GroupFinancialInfo.Controls.Add(this.label9);
            this.GroupFinancialInfo.Controls.Add(this.label8);
            this.GroupFinancialInfo.Controls.Add(this.label7);
            this.GroupFinancialInfo.Location = new System.Drawing.Point(9, 153);
            this.GroupFinancialInfo.Name = "GroupFinancialInfo";
            this.GroupFinancialInfo.Size = new System.Drawing.Size(872, 106);
            this.GroupFinancialInfo.TabIndex = 8;
            this.GroupFinancialInfo.TabStop = false;
            this.GroupFinancialInfo.Text = "Financial Info.";
            // 
            // dateFactoryInvoiceDate
            // 
            this.dateFactoryInvoiceDate.Location = new System.Drawing.Point(161, 69);
            this.dateFactoryInvoiceDate.Name = "dateFactoryInvoiceDate";
            this.dateFactoryInvoiceDate.Size = new System.Drawing.Size(130, 23);
            this.dateFactoryInvoiceDate.TabIndex = 15;
            // 
            // displayFSPDesc
            // 
            this.displayFSPDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFSPDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFSPDesc.Location = new System.Drawing.Point(603, 19);
            this.displayFSPDesc.Name = "displayFSPDesc";
            this.displayFSPDesc.Size = new System.Drawing.Size(93, 23);
            this.displayFSPDesc.TabIndex = 17;
            // 
            // txtFSP
            // 
            this.txtFSP.BackColor = System.Drawing.Color.White;
            this.txtFSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFSP.Location = new System.Drawing.Point(540, 19);
            this.txtFSP.Name = "txtFSP";
            this.txtFSP.Size = new System.Drawing.Size(62, 23);
            this.txtFSP.TabIndex = 17;
            this.txtFSP.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtFSP_PopUp);
            this.txtFSP.Validating += new System.ComponentModel.CancelEventHandler(this.TxtFSP_Validating);
            // 
            // txtQAReferenceNbr
            // 
            this.txtQAReferenceNbr.BackColor = System.Drawing.Color.White;
            this.txtQAReferenceNbr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtQAReferenceNbr.Location = new System.Drawing.Point(540, 69);
            this.txtQAReferenceNbr.Name = "txtQAReferenceNbr";
            this.txtQAReferenceNbr.Size = new System.Drawing.Size(217, 23);
            this.txtQAReferenceNbr.TabIndex = 18;
            this.txtQAReferenceNbr.TextChanged += new System.EventHandler(this.OnlyEnglishNumber_TextChanged);
            // 
            // txtLCReferenceNbr
            // 
            this.txtLCReferenceNbr.BackColor = System.Drawing.Color.White;
            this.txtLCReferenceNbr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLCReferenceNbr.Location = new System.Drawing.Point(540, 44);
            this.txtLCReferenceNbr.Name = "txtLCReferenceNbr";
            this.txtLCReferenceNbr.Size = new System.Drawing.Size(217, 23);
            this.txtLCReferenceNbr.TabIndex = 17;
            this.txtLCReferenceNbr.TextChanged += new System.EventHandler(this.OnlyEnglishNumber_TextChanged);
            // 
            // txtFactoryInvoiceNbr
            // 
            this.txtFactoryInvoiceNbr.BackColor = System.Drawing.Color.White;
            this.txtFactoryInvoiceNbr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactoryInvoiceNbr.Location = new System.Drawing.Point(161, 44);
            this.txtFactoryInvoiceNbr.Name = "txtFactoryInvoiceNbr";
            this.txtFactoryInvoiceNbr.Size = new System.Drawing.Size(217, 23);
            this.txtFactoryInvoiceNbr.TabIndex = 16;
            this.txtFactoryInvoiceNbr.TextChanged += new System.EventHandler(this.OnlyEnglishNumber_TextChanged);
            // 
            // txtLSPBookingNumber
            // 
            this.txtLSPBookingNumber.BackColor = System.Drawing.Color.White;
            this.txtLSPBookingNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLSPBookingNumber.Location = new System.Drawing.Point(161, 19);
            this.txtLSPBookingNumber.Name = "txtLSPBookingNumber";
            this.txtLSPBookingNumber.Size = new System.Drawing.Size(216, 23);
            this.txtLSPBookingNumber.TabIndex = 15;
            this.txtLSPBookingNumber.TextChanged += new System.EventHandler(this.OnlyEnglishNumber_TextChanged);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(397, 69);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(140, 23);
            this.label12.TabIndex = 14;
            this.label12.Text = "QA Reference Nbr";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(397, 44);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(140, 23);
            this.label11.TabIndex = 13;
            this.label11.Text = "LC Reference Nbr";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(397, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(140, 23);
            this.label10.TabIndex = 12;
            this.label10.Text = "FSP";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(8, 69);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(150, 23);
            this.label9.TabIndex = 11;
            this.label9.Text = "Factory Invoice Date";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(150, 23);
            this.label8.TabIndex = 10;
            this.label8.Text = "Factory Invoice Nbr";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(150, 23);
            this.label7.TabIndex = 9;
            this.label7.Text = "LSP Booking Number";
            // 
            // GroupDocumentsScanFile
            // 
            this.GroupDocumentsScanFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupDocumentsScanFile.Controls.Add(this.chkMultipleMaterialsPackingList);
            this.GroupDocumentsScanFile.Controls.Add(this.chkFactoryAddressPackingList);
            this.GroupDocumentsScanFile.Controls.Add(this.chkPRELMINARY_Watermark);
            this.GroupDocumentsScanFile.Controls.Add(this.chkGrossWeightPackingList);
            this.GroupDocumentsScanFile.Controls.Add(this.chkTotalGrossWeightCommercialInvoice);
            this.GroupDocumentsScanFile.Controls.Add(this.chkConsolidateInvoices);
            this.GroupDocumentsScanFile.Controls.Add(this.comboSellerAddress);
            this.GroupDocumentsScanFile.Controls.Add(this.comboFactoryAddress);
            this.GroupDocumentsScanFile.Controls.Add(this.groupChoiceShippingDoc);
            this.GroupDocumentsScanFile.Controls.Add(this.label14);
            this.GroupDocumentsScanFile.Controls.Add(this.label13);
            this.GroupDocumentsScanFile.Controls.Add(this.btnPrint);
            this.GroupDocumentsScanFile.IsSupportEditMode = false;
            this.GroupDocumentsScanFile.Location = new System.Drawing.Point(9, 265);
            this.GroupDocumentsScanFile.Name = "GroupDocumentsScanFile";
            this.GroupDocumentsScanFile.Size = new System.Drawing.Size(872, 218);
            this.GroupDocumentsScanFile.TabIndex = 9;
            this.GroupDocumentsScanFile.TabStop = false;
            this.GroupDocumentsScanFile.Text = "Documents and Scan File";
            // 
            // chkMultipleMaterialsPackingList
            // 
            this.chkMultipleMaterialsPackingList.AutoSize = true;
            this.chkMultipleMaterialsPackingList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkMultipleMaterialsPackingList.IsSupportEditMode = false;
            this.chkMultipleMaterialsPackingList.Location = new System.Drawing.Point(594, 44);
            this.chkMultipleMaterialsPackingList.Name = "chkMultipleMaterialsPackingList";
            this.chkMultipleMaterialsPackingList.Size = new System.Drawing.Size(273, 21);
            this.chkMultipleMaterialsPackingList.TabIndex = 24;
            this.chkMultipleMaterialsPackingList.Text = "Multiple Materials on Packing List Page";
            this.chkMultipleMaterialsPackingList.UseVisualStyleBackColor = true;
            // 
            // chkFactoryAddressPackingList
            // 
            this.chkFactoryAddressPackingList.AutoSize = true;
            this.chkFactoryAddressPackingList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkFactoryAddressPackingList.IsSupportEditMode = false;
            this.chkFactoryAddressPackingList.Location = new System.Drawing.Point(407, 152);
            this.chkFactoryAddressPackingList.Name = "chkFactoryAddressPackingList";
            this.chkFactoryAddressPackingList.Size = new System.Drawing.Size(230, 21);
            this.chkFactoryAddressPackingList.TabIndex = 23;
            this.chkFactoryAddressPackingList.Text = "Factory Address on Packing List";
            this.chkFactoryAddressPackingList.UseVisualStyleBackColor = true;
            // 
            // chkPRELMINARY_Watermark
            // 
            this.chkPRELMINARY_Watermark.AutoSize = true;
            this.chkPRELMINARY_Watermark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkPRELMINARY_Watermark.IsSupportEditMode = false;
            this.chkPRELMINARY_Watermark.Location = new System.Drawing.Point(407, 125);
            this.chkPRELMINARY_Watermark.Name = "chkPRELMINARY_Watermark";
            this.chkPRELMINARY_Watermark.Size = new System.Drawing.Size(194, 21);
            this.chkPRELMINARY_Watermark.TabIndex = 22;
            this.chkPRELMINARY_Watermark.Text = "\'PRELMINARY\' Watermark";
            this.chkPRELMINARY_Watermark.UseVisualStyleBackColor = true;
            // 
            // chkGrossWeightPackingList
            // 
            this.chkGrossWeightPackingList.AutoSize = true;
            this.chkGrossWeightPackingList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkGrossWeightPackingList.IsSupportEditMode = false;
            this.chkGrossWeightPackingList.Location = new System.Drawing.Point(406, 98);
            this.chkGrossWeightPackingList.Name = "chkGrossWeightPackingList";
            this.chkGrossWeightPackingList.Size = new System.Drawing.Size(239, 21);
            this.chkGrossWeightPackingList.TabIndex = 21;
            this.chkGrossWeightPackingList.Text = "Gross/Net Weight on Packing List";
            this.chkGrossWeightPackingList.UseVisualStyleBackColor = true;
            // 
            // chkTotalGrossWeightCommercialInvoice
            // 
            this.chkTotalGrossWeightCommercialInvoice.AutoSize = true;
            this.chkTotalGrossWeightCommercialInvoice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkTotalGrossWeightCommercialInvoice.IsSupportEditMode = false;
            this.chkTotalGrossWeightCommercialInvoice.Location = new System.Drawing.Point(407, 71);
            this.chkTotalGrossWeightCommercialInvoice.Name = "chkTotalGrossWeightCommercialInvoice";
            this.chkTotalGrossWeightCommercialInvoice.Size = new System.Drawing.Size(320, 21);
            this.chkTotalGrossWeightCommercialInvoice.TabIndex = 20;
            this.chkTotalGrossWeightCommercialInvoice.Text = "Total Gross/Net Weight on Commercial Invoice";
            this.chkTotalGrossWeightCommercialInvoice.UseVisualStyleBackColor = true;
            // 
            // chkConsolidateInvoices
            // 
            this.chkConsolidateInvoices.AutoSize = true;
            this.chkConsolidateInvoices.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkConsolidateInvoices.IsSupportEditMode = false;
            this.chkConsolidateInvoices.Location = new System.Drawing.Point(407, 44);
            this.chkConsolidateInvoices.Name = "chkConsolidateInvoices";
            this.chkConsolidateInvoices.Size = new System.Drawing.Size(156, 21);
            this.chkConsolidateInvoices.TabIndex = 8;
            this.chkConsolidateInvoices.Text = "Consolidate Invoices";
            this.chkConsolidateInvoices.UseVisualStyleBackColor = true;
            // 
            // comboSellerAddress
            // 
            this.comboSellerAddress.BackColor = System.Drawing.Color.White;
            this.comboSellerAddress.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboSellerAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSellerAddress.FormattingEnabled = true;
            this.comboSellerAddress.IsSupportUnselect = true;
            this.comboSellerAddress.Location = new System.Drawing.Point(540, 188);
            this.comboSellerAddress.Name = "comboSellerAddress";
            this.comboSellerAddress.OldText = "";
            this.comboSellerAddress.Size = new System.Drawing.Size(140, 24);
            this.comboSellerAddress.TabIndex = 19;
            // 
            // comboFactoryAddress
            // 
            this.comboFactoryAddress.BackColor = System.Drawing.Color.White;
            this.comboFactoryAddress.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboFactoryAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactoryAddress.FormattingEnabled = true;
            this.comboFactoryAddress.IsSupportUnselect = true;
            this.comboFactoryAddress.Location = new System.Drawing.Point(160, 188);
            this.comboFactoryAddress.Name = "comboFactoryAddress";
            this.comboFactoryAddress.OldText = "";
            this.comboFactoryAddress.Size = new System.Drawing.Size(140, 24);
            this.comboFactoryAddress.TabIndex = 18;
            // 
            // groupChoiceShippingDoc
            // 
            this.groupChoiceShippingDoc.Controls.Add(this.chkTallySheet);
            this.groupChoiceShippingDoc.Controls.Add(this.chkShipmentSummary);
            this.groupChoiceShippingDoc.Controls.Add(this.chkCertificateOrigin);
            this.groupChoiceShippingDoc.Controls.Add(this.chkCertificateInspection);
            this.groupChoiceShippingDoc.Controls.Add(this.chkTradingCmpyPL);
            this.groupChoiceShippingDoc.Controls.Add(this.chkPackingList);
            this.groupChoiceShippingDoc.Controls.Add(this.chkTradingCmpyCI);
            this.groupChoiceShippingDoc.Controls.Add(this.chkCommercialInvoice);
            this.groupChoiceShippingDoc.IsSupportEditMode = false;
            this.groupChoiceShippingDoc.Location = new System.Drawing.Point(8, 22);
            this.groupChoiceShippingDoc.Name = "groupChoiceShippingDoc";
            this.groupChoiceShippingDoc.Size = new System.Drawing.Size(369, 164);
            this.groupChoiceShippingDoc.TabIndex = 17;
            this.groupChoiceShippingDoc.TabStop = false;
            this.groupChoiceShippingDoc.Text = "Choice of Shipping Document";
            // 
            // chkTallySheet
            // 
            this.chkTallySheet.AutoSize = true;
            this.chkTallySheet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkTallySheet.IsSupportEditMode = false;
            this.chkTallySheet.Location = new System.Drawing.Point(197, 76);
            this.chkTallySheet.Name = "chkTallySheet";
            this.chkTallySheet.Size = new System.Drawing.Size(98, 21);
            this.chkTallySheet.TabIndex = 7;
            this.chkTallySheet.Text = "Tally Sheet";
            this.chkTallySheet.UseVisualStyleBackColor = true;
            // 
            // chkShipmentSummary
            // 
            this.chkShipmentSummary.AutoSize = true;
            this.chkShipmentSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkShipmentSummary.IsSupportEditMode = false;
            this.chkShipmentSummary.Location = new System.Drawing.Point(197, 49);
            this.chkShipmentSummary.Name = "chkShipmentSummary";
            this.chkShipmentSummary.Size = new System.Drawing.Size(149, 21);
            this.chkShipmentSummary.TabIndex = 6;
            this.chkShipmentSummary.Text = "Shipment Summary";
            this.chkShipmentSummary.UseVisualStyleBackColor = true;
            // 
            // chkCertificateOrigin
            // 
            this.chkCertificateOrigin.AutoSize = true;
            this.chkCertificateOrigin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkCertificateOrigin.IsSupportEditMode = false;
            this.chkCertificateOrigin.Location = new System.Drawing.Point(197, 22);
            this.chkCertificateOrigin.Name = "chkCertificateOrigin";
            this.chkCertificateOrigin.Size = new System.Drawing.Size(148, 21);
            this.chkCertificateOrigin.TabIndex = 5;
            this.chkCertificateOrigin.Text = "Certificate of Origin";
            this.chkCertificateOrigin.UseVisualStyleBackColor = true;
            // 
            // chkCertificateInspection
            // 
            this.chkCertificateInspection.AutoSize = true;
            this.chkCertificateInspection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkCertificateInspection.IsSupportEditMode = false;
            this.chkCertificateInspection.Location = new System.Drawing.Point(6, 130);
            this.chkCertificateInspection.Name = "chkCertificateInspection";
            this.chkCertificateInspection.Size = new System.Drawing.Size(174, 21);
            this.chkCertificateInspection.TabIndex = 4;
            this.chkCertificateInspection.Text = "Certificate of Inspection";
            this.chkCertificateInspection.UseVisualStyleBackColor = true;
            // 
            // chkTradingCmpyPL
            // 
            this.chkTradingCmpyPL.AutoSize = true;
            this.chkTradingCmpyPL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkTradingCmpyPL.IsSupportEditMode = false;
            this.chkTradingCmpyPL.Location = new System.Drawing.Point(6, 103);
            this.chkTradingCmpyPL.Name = "chkTradingCmpyPL";
            this.chkTradingCmpyPL.Size = new System.Drawing.Size(136, 21);
            this.chkTradingCmpyPL.TabIndex = 3;
            this.chkTradingCmpyPL.Text = "Trading Cmpy PL";
            this.chkTradingCmpyPL.UseVisualStyleBackColor = true;
            // 
            // chkPackingList
            // 
            this.chkPackingList.AutoSize = true;
            this.chkPackingList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkPackingList.IsSupportEditMode = false;
            this.chkPackingList.Location = new System.Drawing.Point(6, 76);
            this.chkPackingList.Name = "chkPackingList";
            this.chkPackingList.Size = new System.Drawing.Size(103, 21);
            this.chkPackingList.TabIndex = 2;
            this.chkPackingList.Text = "Packing List";
            this.chkPackingList.UseVisualStyleBackColor = true;
            // 
            // chkTradingCmpyCI
            // 
            this.chkTradingCmpyCI.AutoSize = true;
            this.chkTradingCmpyCI.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkTradingCmpyCI.IsSupportEditMode = false;
            this.chkTradingCmpyCI.Location = new System.Drawing.Point(6, 49);
            this.chkTradingCmpyCI.Name = "chkTradingCmpyCI";
            this.chkTradingCmpyCI.Size = new System.Drawing.Size(131, 21);
            this.chkTradingCmpyCI.TabIndex = 1;
            this.chkTradingCmpyCI.Text = "Trading Cmpy CI";
            this.chkTradingCmpyCI.UseVisualStyleBackColor = true;
            // 
            // chkCommercialInvoice
            // 
            this.chkCommercialInvoice.AutoSize = true;
            this.chkCommercialInvoice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkCommercialInvoice.IsSupportEditMode = false;
            this.chkCommercialInvoice.Location = new System.Drawing.Point(6, 22);
            this.chkCommercialInvoice.Name = "chkCommercialInvoice";
            this.chkCommercialInvoice.Size = new System.Drawing.Size(148, 21);
            this.chkCommercialInvoice.TabIndex = 0;
            this.chkCommercialInvoice.Text = "Commercial Invoice";
            this.chkCommercialInvoice.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label14.Location = new System.Drawing.Point(397, 189);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(140, 23);
            this.label14.TabIndex = 16;
            this.label14.Text = "Seller Address to use";
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label13.Location = new System.Drawing.Point(8, 189);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(150, 23);
            this.label13.TabIndex = 15;
            this.label13.Text = "Factory Address to use";
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(783, 182);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(83, 30);
            this.btnPrint.TabIndex = 10;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // P05_MercuryPostScanShipment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 538);
            this.Controls.Add(this.GroupDocumentsScanFile);
            this.Controls.Add(this.GroupFinancialInfo);
            this.Controls.Add(this.GroupShippingInfo);
            this.Controls.Add(this.btnCloseUnDo);
            this.Controls.Add(this.btnEditSave);
            this.Controls.Add(this.btnCreateUpdateShipment);
            this.Controls.Add(this.displayShipmentNo);
            this.Controls.Add(this.label1);
            this.Name = "P05_MercuryPostScanShipment";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P05. Mercury Post Scan Shipment";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.displayShipmentNo, 0);
            this.Controls.SetChildIndex(this.btnCreateUpdateShipment, 0);
            this.Controls.SetChildIndex(this.btnEditSave, 0);
            this.Controls.SetChildIndex(this.btnCloseUnDo, 0);
            this.Controls.SetChildIndex(this.GroupShippingInfo, 0);
            this.Controls.SetChildIndex(this.GroupFinancialInfo, 0);
            this.Controls.SetChildIndex(this.GroupDocumentsScanFile, 0);
            this.GroupShippingInfo.ResumeLayout(false);
            this.GroupShippingInfo.PerformLayout();
            this.GroupFinancialInfo.ResumeLayout(false);
            this.GroupFinancialInfo.PerformLayout();
            this.GroupDocumentsScanFile.ResumeLayout(false);
            this.GroupDocumentsScanFile.PerformLayout();
            this.groupChoiceShippingDoc.ResumeLayout(false);
            this.groupChoiceShippingDoc.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.DisplayBox displayShipmentNo;
        private Win.UI.Button btnCreateUpdateShipment;
        private Win.UI.Button btnEditSave;
        private Win.UI.Button btnCloseUnDo;
        private Win.UI.RadioGroup GroupShippingInfo;
        private Win.UI.Label label2;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.RadioGroup GroupFinancialInfo;
        private Win.UI.Label label12;
        private Win.UI.Label label11;
        private Win.UI.Label label10;
        private Win.UI.Label label9;
        private Win.UI.Label label8;
        private Win.UI.Label label7;
        private Win.UI.RadioGroup GroupDocumentsScanFile;
        private Win.UI.Button btnPrint;
        private Win.UI.Label label14;
        private Win.UI.Label label13;
        private Win.UI.RadioGroup groupChoiceShippingDoc;
        private Win.UI.DisplayBox displayLSPDesc;
        private Win.UI.DisplayBox displayNikeLSPCode;
        private Win.UI.ComboBox comboLoadIndicator;
        private Win.UI.DateBox dateShippingDate;
        private Win.UI.TextBox txtTrackingContainer;
        private Win.UI.DisplayBox displayPortOrginDesc;
        private Win.UI.TextBox txtPortOrgin;
        private Win.UI.DateBox dateFactoryInvoiceDate;
        private Win.UI.DisplayBox displayFSPDesc;
        private Win.UI.TextBox txtFSP;
        private Win.UI.TextBox txtQAReferenceNbr;
        private Win.UI.TextBox txtLCReferenceNbr;
        private Win.UI.TextBox txtFactoryInvoiceNbr;
        private Win.UI.TextBox txtLSPBookingNumber;
        private Win.UI.ComboBox comboSellerAddress;
        private Win.UI.ComboBox comboFactoryAddress;
        private Win.UI.CheckBox chkCommercialInvoice;
        private Win.UI.CheckBox chkMultipleMaterialsPackingList;
        private Win.UI.CheckBox chkFactoryAddressPackingList;
        private Win.UI.CheckBox chkPRELMINARY_Watermark;
        private Win.UI.CheckBox chkGrossWeightPackingList;
        private Win.UI.CheckBox chkTotalGrossWeightCommercialInvoice;
        private Win.UI.CheckBox chkConsolidateInvoices;
        private Win.UI.CheckBox chkTallySheet;
        private Win.UI.CheckBox chkShipmentSummary;
        private Win.UI.CheckBox chkCertificateOrigin;
        private Win.UI.CheckBox chkCertificateInspection;
        private Win.UI.CheckBox chkTradingCmpyPL;
        private Win.UI.CheckBox chkPackingList;
        private Win.UI.CheckBox chkTradingCmpyCI;
    }
}