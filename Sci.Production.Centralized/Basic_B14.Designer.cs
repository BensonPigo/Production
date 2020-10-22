namespace Sci.Production.Centralized
{
    partial class Basic_B14
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
            this.labelCode = new Sci.Win.UI.Label();
            this.labelArtworkType = new Sci.Win.UI.Label();
            this.labelClassify = new Sci.Win.UI.Label();
            this.labelUnit = new Sci.Win.UI.Label();
            this.labelProductionunit = new Sci.Win.UI.Label();
            this.labelMachineID = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.labelAbbr = new Sci.Win.UI.Label();
            this.displayCode = new Sci.Win.UI.DisplayBox();
            this.displayAbbr = new Sci.Win.UI.DisplayBox();
            this.displayArtworkType = new Sci.Win.UI.DisplayBox();
            this.displayUnit = new Sci.Win.UI.DisplayBox();
            this.displayRemark = new Sci.Win.UI.DisplayBox();
            this.displayProductionUnit = new Sci.Win.UI.DisplayBox();
            this.txtDropdownlistClassify = new Sci.Production.Class.Txtdropdownlist();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.checkIsTMS = new Sci.Win.UI.CheckBox();
            this.checkIsPrice = new Sci.Win.UI.CheckBox();
            this.checkIsArtWork = new Sci.Win.UI.CheckBox();
            this.checkIsttlTMS = new Sci.Win.UI.CheckBox();
            this.btnMachine = new Sci.Win.UI.Button();
            this.editMachineID = new Sci.Win.UI.EditBox();
            this.checkIsPrintToCMP = new Sci.Win.UI.CheckBox();
            this.chkIsShowinIEP01 = new Sci.Win.UI.CheckBox();
            this.chkIsShowinIEP03 = new Sci.Win.UI.CheckBox();
            this.chkIsSewingline = new Sci.Win.UI.CheckBox();
            this.lbFactory1 = new Sci.Win.UI.Label();
            this.lbFactory2 = new Sci.Win.UI.Label();
            this.txtCentralizedmulitFactoryIEP03 = new Sci.Production.Class.TxtCentralizedmulitFactory();
            this.txtCentralizedmulitFactorySewingline = new Sci.Production.Class.TxtCentralizedmulitFactory();
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
            this.detail.Size = new System.Drawing.Size(832, 490);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtCentralizedmulitFactorySewingline);
            this.detailcont.Controls.Add(this.txtCentralizedmulitFactoryIEP03);
            this.detailcont.Controls.Add(this.lbFactory2);
            this.detailcont.Controls.Add(this.lbFactory1);
            this.detailcont.Controls.Add(this.chkIsSewingline);
            this.detailcont.Controls.Add(this.chkIsShowinIEP03);
            this.detailcont.Controls.Add(this.chkIsShowinIEP01);
            this.detailcont.Controls.Add(this.checkIsPrintToCMP);
            this.detailcont.Controls.Add(this.editMachineID);
            this.detailcont.Controls.Add(this.btnMachine);
            this.detailcont.Controls.Add(this.checkIsttlTMS);
            this.detailcont.Controls.Add(this.checkIsArtWork);
            this.detailcont.Controls.Add(this.checkIsPrice);
            this.detailcont.Controls.Add(this.checkIsTMS);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.displayProductionUnit);
            this.detailcont.Controls.Add(this.txtDropdownlistClassify);
            this.detailcont.Controls.Add(this.displayRemark);
            this.detailcont.Controls.Add(this.displayUnit);
            this.detailcont.Controls.Add(this.displayArtworkType);
            this.detailcont.Controls.Add(this.displayAbbr);
            this.detailcont.Controls.Add(this.displayCode);
            this.detailcont.Controls.Add(this.labelAbbr);
            this.detailcont.Controls.Add(this.labelRemark);
            this.detailcont.Controls.Add(this.labelMachineID);
            this.detailcont.Controls.Add(this.labelProductionunit);
            this.detailcont.Controls.Add(this.labelUnit);
            this.detailcont.Controls.Add(this.labelClassify);
            this.detailcont.Controls.Add(this.labelArtworkType);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Size = new System.Drawing.Size(832, 438);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 438);
            this.detailbtm.Size = new System.Drawing.Size(832, 52);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(832, 490);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(840, 519);
            // 
            // createby
            // 
            this.createby.Location = new System.Drawing.Point(70, 21);
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 21);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lblcreateby
            // 
            this.lblcreateby.Location = new System.Drawing.Point(5, 27);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 27);
            // 
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(27, 10);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(110, 23);
            this.labelCode.TabIndex = 0;
            this.labelCode.Text = "Code";
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.Location = new System.Drawing.Point(27, 40);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.Size = new System.Drawing.Size(110, 23);
            this.labelArtworkType.TabIndex = 1;
            this.labelArtworkType.Text = "Artwork Type";
            // 
            // labelClassify
            // 
            this.labelClassify.Location = new System.Drawing.Point(27, 70);
            this.labelClassify.Name = "labelClassify";
            this.labelClassify.Size = new System.Drawing.Size(110, 23);
            this.labelClassify.TabIndex = 2;
            this.labelClassify.Text = "Classify";
            // 
            // labelUnit
            // 
            this.labelUnit.Location = new System.Drawing.Point(27, 100);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(110, 23);
            this.labelUnit.TabIndex = 3;
            this.labelUnit.Text = "Unit";
            // 
            // labelProductionunit
            // 
            this.labelProductionunit.Location = new System.Drawing.Point(27, 130);
            this.labelProductionunit.Name = "labelProductionunit";
            this.labelProductionunit.Size = new System.Drawing.Size(110, 23);
            this.labelProductionunit.TabIndex = 4;
            this.labelProductionunit.Text = "Production unit";
            // 
            // labelMachineID
            // 
            this.labelMachineID.Location = new System.Drawing.Point(27, 160);
            this.labelMachineID.Name = "labelMachineID";
            this.labelMachineID.Size = new System.Drawing.Size(110, 23);
            this.labelMachineID.TabIndex = 8;
            this.labelMachineID.Text = "Machine ID";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(27, 232);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(110, 23);
            this.labelRemark.TabIndex = 9;
            this.labelRemark.Text = "Remark";
            // 
            // labelAbbr
            // 
            this.labelAbbr.Location = new System.Drawing.Point(238, 10);
            this.labelAbbr.Name = "labelAbbr";
            this.labelAbbr.Size = new System.Drawing.Size(40, 23);
            this.labelAbbr.TabIndex = 10;
            this.labelAbbr.Text = "Abbr";
            // 
            // displayCode
            // 
            this.displayCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Seq", true));
            this.displayCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCode.Location = new System.Drawing.Point(144, 10);
            this.displayCode.Name = "displayCode";
            this.displayCode.Size = new System.Drawing.Size(42, 23);
            this.displayCode.TabIndex = 0;
            // 
            // displayAbbr
            // 
            this.displayAbbr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayAbbr.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Abbreviation", true));
            this.displayAbbr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayAbbr.Location = new System.Drawing.Point(282, 10);
            this.displayAbbr.Name = "displayAbbr";
            this.displayAbbr.Size = new System.Drawing.Size(30, 23);
            this.displayAbbr.TabIndex = 1;
            // 
            // displayArtworkType
            // 
            this.displayArtworkType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayArtworkType.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayArtworkType.Location = new System.Drawing.Point(144, 40);
            this.displayArtworkType.Name = "displayArtworkType";
            this.displayArtworkType.Size = new System.Drawing.Size(170, 23);
            this.displayArtworkType.TabIndex = 2;
            // 
            // displayUnit
            // 
            this.displayUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayUnit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ArtworkUnit", true));
            this.displayUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayUnit.Location = new System.Drawing.Point(144, 100);
            this.displayUnit.Name = "displayUnit";
            this.displayUnit.Size = new System.Drawing.Size(78, 23);
            this.displayUnit.TabIndex = 4;
            // 
            // displayRemark
            // 
            this.displayRemark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRemark.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Remark", true));
            this.displayRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRemark.Location = new System.Drawing.Point(144, 232);
            this.displayRemark.Name = "displayRemark";
            this.displayRemark.Size = new System.Drawing.Size(168, 23);
            this.displayRemark.TabIndex = 10;
            // 
            // displayProductionUnit
            // 
            this.displayProductionUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayProductionUnit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ProductionUnit", true));
            this.displayProductionUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayProductionUnit.Location = new System.Drawing.Point(144, 130);
            this.displayProductionUnit.Name = "displayProductionUnit";
            this.displayProductionUnit.Size = new System.Drawing.Size(78, 23);
            this.displayProductionUnit.TabIndex = 5;
            // 
            // txtDropdownlistClassify
            // 
            this.txtDropdownlistClassify.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtDropdownlistClassify.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Classify", true));
            this.txtDropdownlistClassify.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.txtDropdownlistClassify.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtDropdownlistClassify.FormattingEnabled = true;
            this.txtDropdownlistClassify.IsSupportUnselect = true;
            this.txtDropdownlistClassify.Location = new System.Drawing.Point(144, 70);
            this.txtDropdownlistClassify.Name = "txtDropdownlistClassify";
            this.txtDropdownlistClassify.OldText = "";
            this.txtDropdownlistClassify.ReadOnly = true;
            this.txtDropdownlistClassify.Size = new System.Drawing.Size(121, 24);
            this.txtDropdownlistClassify.TabIndex = 3;
            this.txtDropdownlistClassify.Type = "Classify";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkJunk.IsSupportEditMode = false;
            this.checkJunk.Location = new System.Drawing.Point(364, 10);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.ReadOnly = true;
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 11;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // checkIsTMS
            // 
            this.checkIsTMS.AutoSize = true;
            this.checkIsTMS.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsTMS", true));
            this.checkIsTMS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkIsTMS.IsSupportEditMode = false;
            this.checkIsTMS.Location = new System.Drawing.Point(364, 38);
            this.checkIsTMS.Name = "checkIsTMS";
            this.checkIsTMS.ReadOnly = true;
            this.checkIsTMS.Size = new System.Drawing.Size(168, 21);
            this.checkIsTMS.TabIndex = 12;
            this.checkIsTMS.Text = "秒數換算成本 (Is TMS)";
            this.checkIsTMS.UseVisualStyleBackColor = true;
            // 
            // checkIsPrice
            // 
            this.checkIsPrice.AutoSize = true;
            this.checkIsPrice.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsPrice", true));
            this.checkIsPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkIsPrice.IsSupportEditMode = false;
            this.checkIsPrice.Location = new System.Drawing.Point(364, 70);
            this.checkIsPrice.Name = "checkIsPrice";
            this.checkIsPrice.ReadOnly = true;
            this.checkIsPrice.Size = new System.Drawing.Size(171, 21);
            this.checkIsPrice.TabIndex = 13;
            this.checkIsPrice.Text = "直接輸入成本 (Is Price)";
            this.checkIsPrice.UseVisualStyleBackColor = true;
            // 
            // checkIsArtWork
            // 
            this.checkIsArtWork.AutoSize = true;
            this.checkIsArtWork.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsArtwork", true));
            this.checkIsArtWork.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkIsArtWork.IsSupportEditMode = false;
            this.checkIsArtWork.Location = new System.Drawing.Point(364, 100);
            this.checkIsArtWork.Name = "checkIsArtWork";
            this.checkIsArtWork.ReadOnly = true;
            this.checkIsArtWork.Size = new System.Drawing.Size(213, 21);
            this.checkIsArtWork.TabIndex = 14;
            this.checkIsArtWork.Text = "是否加入ArtWork (Is ArtWork)";
            this.checkIsArtWork.UseVisualStyleBackColor = true;
            // 
            // checkIsttlTMS
            // 
            this.checkIsttlTMS.AutoSize = true;
            this.checkIsttlTMS.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsTtlTMS", true));
            this.checkIsttlTMS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkIsttlTMS.IsSupportEditMode = false;
            this.checkIsttlTMS.Location = new System.Drawing.Point(364, 130);
            this.checkIsttlTMS.Name = "checkIsttlTMS";
            this.checkIsttlTMS.ReadOnly = true;
            this.checkIsttlTMS.Size = new System.Drawing.Size(199, 21);
            this.checkIsttlTMS.TabIndex = 15;
            this.checkIsttlTMS.Text = "是否加入ttl TMS (Is ttl TMS)";
            this.checkIsttlTMS.UseVisualStyleBackColor = true;
            // 
            // btnMachine
            // 
            this.btnMachine.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnMachine.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnMachine.Location = new System.Drawing.Point(573, 20);
            this.btnMachine.Name = "btnMachine";
            this.btnMachine.Size = new System.Drawing.Size(80, 30);
            this.btnMachine.TabIndex = 17;
            this.btnMachine.Text = "Machine";
            this.btnMachine.UseVisualStyleBackColor = true;
            this.btnMachine.Click += new System.EventHandler(this.BtnMachine_Click);
            // 
            // editMachineID
            // 
            this.editMachineID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editMachineID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editMachineID.IsSupportEditMode = false;
            this.editMachineID.Location = new System.Drawing.Point(144, 157);
            this.editMachineID.Multiline = true;
            this.editMachineID.Name = "editMachineID";
            this.editMachineID.ReadOnly = true;
            this.editMachineID.Size = new System.Drawing.Size(168, 69);
            this.editMachineID.TabIndex = 9;
            // 
            // checkIsPrintToCMP
            // 
            this.checkIsPrintToCMP.AutoSize = true;
            this.checkIsPrintToCMP.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsPrintToCMP", true));
            this.checkIsPrintToCMP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkIsPrintToCMP.IsSupportEditMode = false;
            this.checkIsPrintToCMP.Location = new System.Drawing.Point(364, 157);
            this.checkIsPrintToCMP.Name = "checkIsPrintToCMP";
            this.checkIsPrintToCMP.ReadOnly = true;
            this.checkIsPrintToCMP.Size = new System.Drawing.Size(272, 21);
            this.checkIsPrintToCMP.TabIndex = 20;
            this.checkIsPrintToCMP.Text = "是否在CMP Report列印 (IsPrintToCMP)";
            this.checkIsPrintToCMP.UseVisualStyleBackColor = true;
            // 
            // chkIsShowinIEP01
            // 
            this.chkIsShowinIEP01.AutoSize = true;
            this.chkIsShowinIEP01.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsShowinIEP01.Location = new System.Drawing.Point(364, 189);
            this.chkIsShowinIEP01.Name = "chkIsShowinIEP01";
            this.chkIsShowinIEP01.Size = new System.Drawing.Size(245, 21);
            this.chkIsShowinIEP01.TabIndex = 21;
            this.chkIsShowinIEP01.Text = "是否加入IE P01(Add into IE P01)";
            this.chkIsShowinIEP01.UseVisualStyleBackColor = true;
            // 
            // chkIsShowinIEP03
            // 
            this.chkIsShowinIEP03.AutoSize = true;
            this.chkIsShowinIEP03.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsShowinIEP03.Location = new System.Drawing.Point(364, 216);
            this.chkIsShowinIEP03.Name = "chkIsShowinIEP03";
            this.chkIsShowinIEP03.Size = new System.Drawing.Size(245, 21);
            this.chkIsShowinIEP03.TabIndex = 22;
            this.chkIsShowinIEP03.Text = "是否加入IE P03(Add into IE P03)";
            this.chkIsShowinIEP03.UseVisualStyleBackColor = true;
            // 
            // chkIsSewingline
            // 
            this.chkIsSewingline.AutoSize = true;
            this.chkIsSewingline.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsSewingline.Location = new System.Drawing.Point(364, 272);
            this.chkIsSewingline.Name = "chkIsSewingline";
            this.chkIsSewingline.Size = new System.Drawing.Size(260, 21);
            this.chkIsSewingline.TabIndex = 23;
            this.chkIsSewingline.Text = "是否在Sewing Line(Is in Sewing Line)";
            this.chkIsSewingline.UseVisualStyleBackColor = true;
            // 
            // lbFactory1
            // 
            this.lbFactory1.Location = new System.Drawing.Point(364, 243);
            this.lbFactory1.Name = "lbFactory1";
            this.lbFactory1.Size = new System.Drawing.Size(56, 23);
            this.lbFactory1.TabIndex = 25;
            this.lbFactory1.Text = "Factory";
            // 
            // lbFactory2
            // 
            this.lbFactory2.Location = new System.Drawing.Point(364, 296);
            this.lbFactory2.Name = "lbFactory2";
            this.lbFactory2.Size = new System.Drawing.Size(56, 22);
            this.lbFactory2.TabIndex = 26;
            this.lbFactory2.Text = "Factory";
            // 
            // txtCentralizedmulitFactoryIEP03
            // 
            this.txtCentralizedmulitFactoryIEP03.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtCentralizedmulitFactoryIEP03.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtCentralizedmulitFactoryIEP03.IsSupportEditMode = false;
            this.txtCentralizedmulitFactoryIEP03.Location = new System.Drawing.Point(424, 243);
            this.txtCentralizedmulitFactoryIEP03.MObjectName = null;
            this.txtCentralizedmulitFactoryIEP03.Name = "txtCentralizedmulitFactoryIEP03";
            this.txtCentralizedmulitFactoryIEP03.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtCentralizedmulitFactoryIEP03.ReadOnly = true;
            this.txtCentralizedmulitFactoryIEP03.Size = new System.Drawing.Size(263, 23);
            this.txtCentralizedmulitFactoryIEP03.TabIndex = 28;
            this.txtCentralizedmulitFactoryIEP03.IsProduceFty = true;
            this.txtCentralizedmulitFactoryIEP03.IsAddConditionJunk = true;
            this.txtCentralizedmulitFactoryIEP03.IsJunk = false;
            // 
            // txtCentralizedmulitFactorySewingline
            // 
            this.txtCentralizedmulitFactorySewingline.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtCentralizedmulitFactorySewingline.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtCentralizedmulitFactorySewingline.IsSupportEditMode = false;
            this.txtCentralizedmulitFactorySewingline.Location = new System.Drawing.Point(424, 295);
            this.txtCentralizedmulitFactorySewingline.MObjectName = null;
            this.txtCentralizedmulitFactorySewingline.Name = "txtCentralizedmulitFactorySewingline";
            this.txtCentralizedmulitFactorySewingline.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtCentralizedmulitFactorySewingline.ReadOnly = true;
            this.txtCentralizedmulitFactorySewingline.Size = new System.Drawing.Size(263, 23);
            this.txtCentralizedmulitFactorySewingline.TabIndex = 29;
            this.txtCentralizedmulitFactorySewingline.IsProduceFty = true;
            this.txtCentralizedmulitFactorySewingline.IsAddConditionJunk = true;
            this.txtCentralizedmulitFactorySewingline.IsJunk = false;
            // 
            // Basic_B14
            // 
            this.ClientSize = new System.Drawing.Size(840, 552);
            this.ConnectionName = "Trade";
            this.DefaultOrder = "Seq";
            this.EnableGridJunkColor = true;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "Basic_B14";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "Basic_B14. TMS & Cost Type";
            this.UniqueExpress = "ID";
            this.WorkAlias = "ArtworkType";
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

        private Win.UI.Label labelMachineID;
        private Win.UI.Label labelProductionunit;
        private Win.UI.Label labelUnit;
        private Win.UI.Label labelClassify;
        private Win.UI.Label labelArtworkType;
        private Win.UI.Label labelCode;
        private Win.UI.Label labelAbbr;
        private Win.UI.Label labelRemark;
        private Win.UI.DisplayBox displayProductionUnit;
        private Class.Txtdropdownlist txtDropdownlistClassify;
        private Win.UI.DisplayBox displayRemark;
        private Win.UI.DisplayBox displayUnit;
        private Win.UI.DisplayBox displayArtworkType;
        private Win.UI.DisplayBox displayAbbr;
        private Win.UI.DisplayBox displayCode;
        private Win.UI.CheckBox checkIsttlTMS;
        private Win.UI.CheckBox checkIsArtWork;
        private Win.UI.CheckBox checkIsPrice;
        private Win.UI.CheckBox checkIsTMS;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Button btnMachine;
        private Win.UI.EditBox editMachineID;
        private Win.UI.CheckBox checkIsPrintToCMP;
        private Win.UI.CheckBox chkIsSewingline;
        private Win.UI.CheckBox chkIsShowinIEP03;
        private Win.UI.CheckBox chkIsShowinIEP01;
        private Win.UI.Label lbFactory2;
        private Win.UI.Label lbFactory1;
        private Class.TxtCentralizedmulitFactory txtCentralizedmulitFactoryIEP03;
        private Class.TxtCentralizedmulitFactory txtCentralizedmulitFactorySewingline;
    }
}
