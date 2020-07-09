namespace Sci.Production.Shipping
{
    partial class P02_AddByPOItem
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
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelPrice = new Sci.Win.UI.Label();
            this.labelQty = new Sci.Win.UI.Label();
            this.labelNW = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelTeamLeader = new Sci.Win.UI.Label();
            this.labelRemark = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.displaySPNo = new Sci.Win.UI.DisplayBox();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.numPrice = new Sci.Win.UI.NumericBox();
            this.labelCTNNo = new Sci.Win.UI.Label();
            this.txtCTNNo = new Sci.Win.UI.TextBox();
            this.numQty = new Sci.Win.UI.NumericBox();
            this.labelUnit = new Sci.Win.UI.Label();
            this.txtunit_ftyUnit = new Sci.Production.Class.Txtunit_fty();
            this.numNW = new Sci.Win.UI.NumericBox();
            this.labelReceiver = new Sci.Win.UI.Label();
            this.txtReceiver = new Sci.Win.UI.TextBox();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.labelAirPPNo = new Sci.Win.UI.Label();
            this.txtPackingListID = new Sci.Win.UI.TextBox();
            this.txttpeuserTeamLeader = new Sci.Production.Class.Txttpeuser();
            this.labelBrand = new Sci.Win.UI.Label();
            this.displayBrand = new Sci.Win.UI.DisplayBox();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.labelSeason = new Sci.Win.UI.Label();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.labelStyle = new Sci.Win.UI.Label();
            this.displayStyle = new Sci.Win.UI.DisplayBox();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 327);
            this.btmcont.Size = new System.Drawing.Size(690, 40);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(600, 5);
            this.undo.TabIndex = 18;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(520, 5);
            this.save.TabIndex = 17;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(13, 13);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(87, 23);
            this.labelSPNo.TabIndex = 96;
            this.labelSPNo.Text = "SP#";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(13, 40);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(87, 23);
            this.labelDescription.TabIndex = 97;
            this.labelDescription.Text = "Description";
            // 
            // labelPrice
            // 
            this.labelPrice.Location = new System.Drawing.Point(417, 180);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.Size = new System.Drawing.Size(87, 23);
            this.labelPrice.TabIndex = 98;
            this.labelPrice.Text = "Price";
            // 
            // labelQty
            // 
            this.labelQty.Location = new System.Drawing.Point(13, 153);
            this.labelQty.Name = "labelQty";
            this.labelQty.Size = new System.Drawing.Size(87, 23);
            this.labelQty.TabIndex = 99;
            this.labelQty.Text = "Q\'ty";
            // 
            // labelNW
            // 
            this.labelNW.Location = new System.Drawing.Point(13, 180);
            this.labelNW.Name = "labelNW";
            this.labelNW.Size = new System.Drawing.Size(87, 23);
            this.labelNW.TabIndex = 100;
            this.labelNW.Text = "N.W. (kg)";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(13, 207);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(87, 23);
            this.labelCategory.TabIndex = 101;
            this.labelCategory.Text = "Category";
            // 
            // labelTeamLeader
            // 
            this.labelTeamLeader.Location = new System.Drawing.Point(13, 234);
            this.labelTeamLeader.Name = "labelTeamLeader";
            this.labelTeamLeader.Size = new System.Drawing.Size(87, 23);
            this.labelTeamLeader.TabIndex = 102;
            this.labelTeamLeader.Text = "Team Leader";
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(13, 261);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(87, 23);
            this.labelRemark.TabIndex = 103;
            this.labelRemark.Text = "Remark";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OrderID", true));
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(104, 13);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(120, 23);
            this.txtSPNo.TabIndex = 1;
            this.txtSPNo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSPNo_Validating);
            this.txtSPNo.Validated += new System.EventHandler(this.TxtSPNo_Validated);
            // 
            // displaySPNo
            // 
            this.displaySPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySPNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Seq1", true));
            this.displaySPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySPNo.Location = new System.Drawing.Point(229, 13);
            this.displaySPNo.Name = "displaySPNo";
            this.displaySPNo.Size = new System.Drawing.Size(30, 23);
            this.displaySPNo.TabIndex = 2;
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(104, 40);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(572, 82);
            this.editDescription.TabIndex = 5;
            // 
            // numPrice
            // 
            this.numPrice.BackColor = System.Drawing.Color.White;
            this.numPrice.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Price", true));
            this.numPrice.DecimalPlaces = 4;
            this.numPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numPrice.Location = new System.Drawing.Point(508, 180);
            this.numPrice.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            262144});
            this.numPrice.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPrice.Name = "numPrice";
            this.numPrice.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPrice.Size = new System.Drawing.Size(90, 23);
            this.numPrice.TabIndex = 13;
            this.numPrice.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelCTNNo
            // 
            this.labelCTNNo.Location = new System.Drawing.Point(417, 126);
            this.labelCTNNo.Name = "labelCTNNo";
            this.labelCTNNo.Size = new System.Drawing.Size(70, 23);
            this.labelCTNNo.TabIndex = 108;
            this.labelCTNNo.Text = "CTN No.";
            // 
            // txtCTNNo
            // 
            this.txtCTNNo.BackColor = System.Drawing.Color.White;
            this.txtCTNNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CTNNo", true));
            this.txtCTNNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCTNNo.Location = new System.Drawing.Point(491, 126);
            this.txtCTNNo.Name = "txtCTNNo";
            this.txtCTNNo.Size = new System.Drawing.Size(90, 23);
            this.txtCTNNo.TabIndex = 11;
            this.txtCTNNo.Validated += new System.EventHandler(this.TxtCTNNo_Validated);
            // 
            // numQty
            // 
            this.numQty.BackColor = System.Drawing.Color.White;
            this.numQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Qty", true));
            this.numQty.DecimalPlaces = 2;
            this.numQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numQty.Location = new System.Drawing.Point(104, 153);
            this.numQty.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            131072});
            this.numQty.Name = "numQty";
            this.numQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numQty.Size = new System.Drawing.Size(74, 23);
            this.numQty.TabIndex = 7;
            this.numQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelUnit
            // 
            this.labelUnit.Location = new System.Drawing.Point(417, 153);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(70, 23);
            this.labelUnit.TabIndex = 111;
            this.labelUnit.Text = "Unit";
            // 
            // txtunit_ftyUnit
            // 
            this.txtunit_ftyUnit.BackColor = System.Drawing.Color.White;
            this.txtunit_ftyUnit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "UnitID", true));
            this.txtunit_ftyUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtunit_ftyUnit.Location = new System.Drawing.Point(491, 153);
            this.txtunit_ftyUnit.Name = "txtunit_ftyUnit";
            this.txtunit_ftyUnit.Size = new System.Drawing.Size(66, 23);
            this.txtunit_ftyUnit.TabIndex = 12;
            // 
            // numNW
            // 
            this.numNW.BackColor = System.Drawing.Color.White;
            this.numNW.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NW", true));
            this.numNW.DecimalPlaces = 3;
            this.numNW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numNW.Location = new System.Drawing.Point(104, 180);
            this.numNW.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            131072});
            this.numNW.Name = "numNW";
            this.numNW.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numNW.Size = new System.Drawing.Size(65, 23);
            this.numNW.TabIndex = 8;
            this.numNW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelReceiver
            // 
            this.labelReceiver.Location = new System.Drawing.Point(417, 206);
            this.labelReceiver.Name = "labelReceiver";
            this.labelReceiver.Size = new System.Drawing.Size(70, 23);
            this.labelReceiver.TabIndex = 114;
            this.labelReceiver.Text = "Receiver";
            // 
            // txtReceiver
            // 
            this.txtReceiver.BackColor = System.Drawing.Color.White;
            this.txtReceiver.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtReceiver.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Receiver", true));
            this.txtReceiver.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReceiver.Location = new System.Drawing.Point(491, 206);
            this.txtReceiver.Name = "txtReceiver";
            this.txtReceiver.Size = new System.Drawing.Size(185, 23);
            this.txtReceiver.TabIndex = 14;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Category", true));
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(103, 206);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(91, 24);
            this.comboCategory.TabIndex = 9;
            // 
            // labelAirPPNo
            // 
            this.labelAirPPNo.Location = new System.Drawing.Point(13, 126);
            this.labelAirPPNo.Name = "labelAirPPNo";
            this.labelAirPPNo.Size = new System.Drawing.Size(87, 23);
            this.labelAirPPNo.TabIndex = 117;
            this.labelAirPPNo.Text = "PL#";
            // 
            // txtPackingListID
            // 
            this.txtPackingListID.BackColor = System.Drawing.Color.White;
            this.txtPackingListID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "PackingListID", true));
            this.txtPackingListID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackingListID.Location = new System.Drawing.Point(104, 126);
            this.txtPackingListID.Name = "txtPackingListID";
            this.txtPackingListID.Size = new System.Drawing.Size(120, 23);
            this.txtPackingListID.TabIndex = 6;
            this.txtPackingListID.Validating += new System.ComponentModel.CancelEventHandler(this.TxtPackingListID_Validating);
            // 
            // txttpeuserTeamLeader
            // 
            this.txttpeuserTeamLeader.DataBindings.Add(new System.Windows.Forms.Binding("DisplayBox1Binding", this.mtbs, "Leader", true));
            this.txttpeuserTeamLeader.DisplayBox1Binding = "";
            this.txttpeuserTeamLeader.DisplayBox2Binding = "";
            this.txttpeuserTeamLeader.Location = new System.Drawing.Point(104, 234);
            this.txttpeuserTeamLeader.Name = "txttpeuserTeamLeader";
            this.txttpeuserTeamLeader.Size = new System.Drawing.Size(302, 23);
            this.txttpeuserTeamLeader.TabIndex = 10;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(417, 233);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(70, 23);
            this.labelBrand.TabIndex = 120;
            this.labelBrand.Text = "Brand";
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "BrandID", true));
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(491, 233);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(100, 23);
            this.displayBrand.TabIndex = 15;
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(104, 261);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(572, 56);
            this.editRemark.TabIndex = 16;
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(302, 13);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(53, 23);
            this.labelSeason.TabIndex = 123;
            this.labelSeason.Text = "Season";
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SeasonID", true));
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(359, 13);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(92, 23);
            this.displaySeason.TabIndex = 3;
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(494, 13);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(36, 23);
            this.labelStyle.TabIndex = 125;
            this.labelStyle.Text = "Style";
            // 
            // displayStyle
            // 
            this.displayStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyle.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "StyleID", true));
            this.displayStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyle.Location = new System.Drawing.Point(534, 13);
            this.displayStyle.Name = "displayStyle";
            this.displayStyle.Size = new System.Drawing.Size(140, 23);
            this.displayStyle.TabIndex = 4;
            // 
            // P02_AddByPOItem
            // 
            this.ClientSize = new System.Drawing.Size(690, 367);
            this.Controls.Add(this.displayStyle);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.displaySeason);
            this.Controls.Add(this.labelSeason);
            this.Controls.Add(this.editRemark);
            this.Controls.Add(this.displayBrand);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.txttpeuserTeamLeader);
            this.Controls.Add(this.txtPackingListID);
            this.Controls.Add(this.labelAirPPNo);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.txtReceiver);
            this.Controls.Add(this.labelReceiver);
            this.Controls.Add(this.numNW);
            this.Controls.Add(this.txtunit_ftyUnit);
            this.Controls.Add(this.labelUnit);
            this.Controls.Add(this.numQty);
            this.Controls.Add(this.txtCTNNo);
            this.Controls.Add(this.labelCTNNo);
            this.Controls.Add(this.numPrice);
            this.Controls.Add(this.editDescription);
            this.Controls.Add(this.displaySPNo);
            this.Controls.Add(this.txtSPNo);
            this.Controls.Add(this.labelRemark);
            this.Controls.Add(this.labelTeamLeader);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelNW);
            this.Controls.Add(this.labelQty);
            this.Controls.Add(this.labelPrice);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelSPNo);
            this.Name = "P02_AddByPOItem";
            this.OnLineHelpID = "Sci.Win.Subs.Input2A";
            this.Text = "International Air/Express - Add by PO item";
            this.WorkAlias = "Express_Detail";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelDescription, 0);
            this.Controls.SetChildIndex(this.labelPrice, 0);
            this.Controls.SetChildIndex(this.labelQty, 0);
            this.Controls.SetChildIndex(this.labelNW, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.labelTeamLeader, 0);
            this.Controls.SetChildIndex(this.labelRemark, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.Controls.SetChildIndex(this.displaySPNo, 0);
            this.Controls.SetChildIndex(this.editDescription, 0);
            this.Controls.SetChildIndex(this.numPrice, 0);
            this.Controls.SetChildIndex(this.labelCTNNo, 0);
            this.Controls.SetChildIndex(this.txtCTNNo, 0);
            this.Controls.SetChildIndex(this.numQty, 0);
            this.Controls.SetChildIndex(this.labelUnit, 0);
            this.Controls.SetChildIndex(this.txtunit_ftyUnit, 0);
            this.Controls.SetChildIndex(this.numNW, 0);
            this.Controls.SetChildIndex(this.labelReceiver, 0);
            this.Controls.SetChildIndex(this.txtReceiver, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.Controls.SetChildIndex(this.labelAirPPNo, 0);
            this.Controls.SetChildIndex(this.txtPackingListID, 0);
            this.Controls.SetChildIndex(this.txttpeuserTeamLeader, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.displayBrand, 0);
            this.Controls.SetChildIndex(this.editRemark, 0);
            this.Controls.SetChildIndex(this.labelSeason, 0);
            this.Controls.SetChildIndex(this.displaySeason, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.displayStyle, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelPrice;
        private Win.UI.Label labelQty;
        private Win.UI.Label labelNW;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelTeamLeader;
        private Win.UI.Label labelRemark;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.DisplayBox displaySPNo;
        private Win.UI.EditBox editDescription;
        private Win.UI.NumericBox numPrice;
        private Win.UI.Label labelCTNNo;
        private Win.UI.TextBox txtCTNNo;
        private Win.UI.NumericBox numQty;
        private Win.UI.Label labelUnit;
        private Class.Txtunit_fty txtunit_ftyUnit;
        private Win.UI.NumericBox numNW;
        private Win.UI.Label labelReceiver;
        private Win.UI.TextBox txtReceiver;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.Label labelAirPPNo;
        private Win.UI.TextBox txtPackingListID;
        private Class.Txttpeuser txttpeuserTeamLeader;
        private Win.UI.Label labelBrand;
        private Win.UI.DisplayBox displayBrand;
        private Win.UI.EditBox editRemark;
        private Win.UI.Label labelSeason;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.Label labelStyle;
        private Win.UI.DisplayBox displayStyle;
    }
}
