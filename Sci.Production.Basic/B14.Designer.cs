namespace Sci.Production.Basic
{
    partial class B14
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
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.label11 = new Sci.Win.UI.Label();
            this.label12 = new Sci.Win.UI.Label();
            this.label13 = new Sci.Win.UI.Label();
            this.displayCode = new Sci.Win.UI.DisplayBox();
            this.displayAbbr = new Sci.Win.UI.DisplayBox();
            this.displayArtworkType = new Sci.Win.UI.DisplayBox();
            this.displayUnit = new Sci.Win.UI.DisplayBox();
            this.displayRemark = new Sci.Win.UI.DisplayBox();
            this.displayProductionUnit = new Sci.Win.UI.DisplayBox();
            this.txtDropdownlistClassify = new Sci.Production.Class.txtdropdownlist();
            this.comboInHouseOSP = new Sci.Win.UI.ComboBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.checkIsTMS = new Sci.Win.UI.CheckBox();
            this.checkIsPrice = new Sci.Win.UI.CheckBox();
            this.checkIsArtWork = new Sci.Win.UI.CheckBox();
            this.checkIsttlTMS = new Sci.Win.UI.CheckBox();
            this.checkIsSubprocess = new Sci.Win.UI.CheckBox();
            this.btnMachine = new Sci.Win.UI.Button();
            this.editMachineID = new Sci.Win.UI.EditBox();
            this.numSubprocessBCSLeadTime = new Sci.Win.UI.NumericBox();
            this.numStdLTDayb41stCutDateBaseOnSubProcess = new Sci.Win.UI.NumericBox();
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
            this.detail.Size = new System.Drawing.Size(832, 455);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.numStdLTDayb41stCutDateBaseOnSubProcess);
            this.detailcont.Controls.Add(this.numSubprocessBCSLeadTime);
            this.detailcont.Controls.Add(this.editMachineID);
            this.detailcont.Controls.Add(this.btnMachine);
            this.detailcont.Controls.Add(this.checkIsSubprocess);
            this.detailcont.Controls.Add(this.checkIsttlTMS);
            this.detailcont.Controls.Add(this.checkIsArtWork);
            this.detailcont.Controls.Add(this.checkIsPrice);
            this.detailcont.Controls.Add(this.checkIsTMS);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.comboInHouseOSP);
            this.detailcont.Controls.Add(this.displayProductionUnit);
            this.detailcont.Controls.Add(this.txtDropdownlistClassify);
            this.detailcont.Controls.Add(this.displayRemark);
            this.detailcont.Controls.Add(this.displayUnit);
            this.detailcont.Controls.Add(this.displayArtworkType);
            this.detailcont.Controls.Add(this.displayAbbr);
            this.detailcont.Controls.Add(this.displayCode);
            this.detailcont.Controls.Add(this.label13);
            this.detailcont.Controls.Add(this.label12);
            this.detailcont.Controls.Add(this.label11);
            this.detailcont.Controls.Add(this.label10);
            this.detailcont.Controls.Add(this.label9);
            this.detailcont.Controls.Add(this.label8);
            this.detailcont.Controls.Add(this.label7);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Size = new System.Drawing.Size(832, 403);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 403);
            this.detailbtm.Size = new System.Drawing.Size(832, 52);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(832, 455);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(840, 484);
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
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(27, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "Code";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(27, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "Artwork Type";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(27, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 23);
            this.label5.TabIndex = 2;
            this.label5.Text = "Classify";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(27, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 23);
            this.label6.TabIndex = 3;
            this.label6.Text = "Unit";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(27, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 23);
            this.label7.TabIndex = 4;
            this.label7.Text = "Production unit";
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(27, 160);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 23);
            this.label8.TabIndex = 5;
            this.label8.Text = "InHouse/OSP";
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(27, 190);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(110, 46);
            this.label9.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(27, 243);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(110, 69);
            this.label10.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.Lines = 0;
            this.label11.Location = new System.Drawing.Point(27, 319);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(110, 23);
            this.label11.TabIndex = 8;
            this.label11.Text = "Machine ID";
            // 
            // label12
            // 
            this.label12.Lines = 0;
            this.label12.Location = new System.Drawing.Point(27, 373);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(110, 23);
            this.label12.TabIndex = 9;
            this.label12.Text = "Remark";
            // 
            // label13
            // 
            this.label13.Lines = 0;
            this.label13.Location = new System.Drawing.Point(238, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(40, 23);
            this.label13.TabIndex = 10;
            this.label13.Text = "Abbr";
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
            this.displayRemark.Location = new System.Drawing.Point(144, 373);
            this.displayRemark.Name = "displayRemark";
            this.displayRemark.Size = new System.Drawing.Size(380, 23);
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
            this.txtDropdownlistClassify.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtDropdownlistClassify.FormattingEnabled = true;
            this.txtDropdownlistClassify.IsSupportUnselect = true;
            this.txtDropdownlistClassify.Location = new System.Drawing.Point(144, 70);
            this.txtDropdownlistClassify.Name = "txtDropdownlistClassify";
            this.txtDropdownlistClassify.ReadOnly = true;
            this.txtDropdownlistClassify.Size = new System.Drawing.Size(121, 24);
            this.txtDropdownlistClassify.TabIndex = 3;
            this.txtDropdownlistClassify.Type = "Classify";
            // 
            // comboInHouseOSP
            // 
            this.comboInHouseOSP.BackColor = System.Drawing.Color.White;
            this.comboInHouseOSP.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "InhouseOSP", true));
            this.comboInHouseOSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboInHouseOSP.FormattingEnabled = true;
            this.comboInHouseOSP.IsSupportUnselect = true;
            this.comboInHouseOSP.Location = new System.Drawing.Point(144, 160);
            this.comboInHouseOSP.Name = "comboInHouseOSP";
            this.comboInHouseOSP.Size = new System.Drawing.Size(100, 24);
            this.comboInHouseOSP.TabIndex = 6;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(364, 10);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 11;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // checkIsTMS
            // 
            this.checkIsTMS.AutoSize = true;
            this.checkIsTMS.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsTMS", true));
            this.checkIsTMS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIsTMS.Location = new System.Drawing.Point(364, 38);
            this.checkIsTMS.Name = "checkIsTMS";
            this.checkIsTMS.Size = new System.Drawing.Size(168, 21);
            this.checkIsTMS.TabIndex = 12;
            this.checkIsTMS.Text = "秒數換算成本 (Is TMS)";
            this.checkIsTMS.UseVisualStyleBackColor = true;
            // 
            // checkIsPrice
            // 
            this.checkIsPrice.AutoSize = true;
            this.checkIsPrice.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsPrice", true));
            this.checkIsPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIsPrice.Location = new System.Drawing.Point(364, 70);
            this.checkIsPrice.Name = "checkIsPrice";
            this.checkIsPrice.Size = new System.Drawing.Size(171, 21);
            this.checkIsPrice.TabIndex = 13;
            this.checkIsPrice.Text = "直接輸入成本 (Is Price)";
            this.checkIsPrice.UseVisualStyleBackColor = true;
            // 
            // checkIsArtWork
            // 
            this.checkIsArtWork.AutoSize = true;
            this.checkIsArtWork.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsArtwork", true));
            this.checkIsArtWork.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIsArtWork.Location = new System.Drawing.Point(364, 100);
            this.checkIsArtWork.Name = "checkIsArtWork";
            this.checkIsArtWork.Size = new System.Drawing.Size(213, 21);
            this.checkIsArtWork.TabIndex = 14;
            this.checkIsArtWork.Text = "是否加入ArtWork (Is ArtWork)";
            this.checkIsArtWork.UseVisualStyleBackColor = true;
            // 
            // checkIsttlTMS
            // 
            this.checkIsttlTMS.AutoSize = true;
            this.checkIsttlTMS.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsTtlTMS", true));
            this.checkIsttlTMS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIsttlTMS.Location = new System.Drawing.Point(364, 130);
            this.checkIsttlTMS.Name = "checkIsttlTMS";
            this.checkIsttlTMS.Size = new System.Drawing.Size(199, 21);
            this.checkIsttlTMS.TabIndex = 15;
            this.checkIsttlTMS.Text = "是否加入ttl TMS (Is ttl TMS)";
            this.checkIsttlTMS.UseVisualStyleBackColor = true;
            // 
            // checkIsSubprocess
            // 
            this.checkIsSubprocess.AutoSize = true;
            this.checkIsSubprocess.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsSubprocess", true));
            this.checkIsSubprocess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIsSubprocess.Location = new System.Drawing.Point(364, 160);
            this.checkIsSubprocess.Name = "checkIsSubprocess";
            this.checkIsSubprocess.Size = new System.Drawing.Size(247, 21);
            this.checkIsSubprocess.TabIndex = 16;
            this.checkIsSubprocess.Text = "是否為Subprocess (Is Subprocess)";
            this.checkIsSubprocess.UseVisualStyleBackColor = true;
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
            this.btnMachine.Click += new System.EventHandler(this.button1_Click);
            // 
            // editMachineID
            // 
            this.editMachineID.BackColor = System.Drawing.Color.White;
            this.editMachineID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editMachineID.Location = new System.Drawing.Point(144, 319);
            this.editMachineID.Multiline = true;
            this.editMachineID.Name = "editMachineID";
            this.editMachineID.Size = new System.Drawing.Size(380, 46);
            this.editMachineID.TabIndex = 9;
            // 
            // numSubprocessBCSLeadTime
            // 
            this.numSubprocessBCSLeadTime.BackColor = System.Drawing.Color.White;
            this.numSubprocessBCSLeadTime.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "BcsLt", true));
            this.numSubprocessBCSLeadTime.DecimalPlaces = 1;
            this.numSubprocessBCSLeadTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numSubprocessBCSLeadTime.Location = new System.Drawing.Point(144, 202);
            this.numSubprocessBCSLeadTime.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            65536});
            this.numSubprocessBCSLeadTime.MaxLength = 3;
            this.numSubprocessBCSLeadTime.Name = "numSubprocessBCSLeadTime";
            this.numSubprocessBCSLeadTime.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSubprocessBCSLeadTime.Size = new System.Drawing.Size(42, 23);
            this.numSubprocessBCSLeadTime.TabIndex = 7;
            this.numSubprocessBCSLeadTime.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numStdLTDayb41stCutDateBaseOnSubProcess
            // 
            this.numStdLTDayb41stCutDateBaseOnSubProcess.BackColor = System.Drawing.Color.White;
            this.numStdLTDayb41stCutDateBaseOnSubProcess.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CutLt", true));
            this.numStdLTDayb41stCutDateBaseOnSubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numStdLTDayb41stCutDateBaseOnSubProcess.Location = new System.Drawing.Point(144, 265);
            this.numStdLTDayb41stCutDateBaseOnSubProcess.Name = "numStdLTDayb41stCutDateBaseOnSubProcess";
            this.numStdLTDayb41stCutDateBaseOnSubProcess.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numStdLTDayb41stCutDateBaseOnSubProcess.Size = new System.Drawing.Size(36, 23);
            this.numStdLTDayb41stCutDateBaseOnSubProcess.TabIndex = 8;
            this.numStdLTDayb41stCutDateBaseOnSubProcess.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // B14
            // 
            this.ClientSize = new System.Drawing.Size(840, 517);
            this.DefaultControlForEdit = "comboBox1";
            this.DefaultOrder = "Seq";
            this.EnableGridJunkColor = true;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B14";
            this.Text = "B14. TMS & Cost Type";
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

        private Win.UI.Label label11;
        private Win.UI.Label label10;
        private Win.UI.Label label9;
        private Win.UI.Label label8;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label13;
        private Win.UI.Label label12;
        private Win.UI.DisplayBox displayProductionUnit;
        private Class.txtdropdownlist txtDropdownlistClassify;
        private Win.UI.DisplayBox displayRemark;
        private Win.UI.DisplayBox displayUnit;
        private Win.UI.DisplayBox displayArtworkType;
        private Win.UI.DisplayBox displayAbbr;
        private Win.UI.DisplayBox displayCode;
        private Win.UI.CheckBox checkIsSubprocess;
        private Win.UI.CheckBox checkIsttlTMS;
        private Win.UI.CheckBox checkIsArtWork;
        private Win.UI.CheckBox checkIsPrice;
        private Win.UI.CheckBox checkIsTMS;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.ComboBox comboInHouseOSP;
        private Win.UI.Button btnMachine;
        private Win.UI.EditBox editMachineID;
        private Win.UI.NumericBox numStdLTDayb41stCutDateBaseOnSubProcess;
        private Win.UI.NumericBox numSubprocessBCSLeadTime;
    }
}
