namespace Sci.Production.Shipping
{
    partial class P02_EditFromPO
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
            this.editRemark = new Sci.Win.UI.EditBox();
            this.displayBrand = new Sci.Win.UI.DisplayBox();
            this.labelBrand = new Sci.Win.UI.Label();
            this.txtReceiver = new Sci.Win.UI.TextBox();
            this.labelReceiver = new Sci.Win.UI.Label();
            this.numNW = new Sci.Win.UI.NumericBox();
            this.labelUnit = new Sci.Win.UI.Label();
            this.numQty = new Sci.Win.UI.NumericBox();
            this.txtCTNNo = new Sci.Win.UI.TextBox();
            this.labelCTNNo = new Sci.Win.UI.Label();
            this.numPrice = new Sci.Win.UI.NumericBox();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.displaySEQ1 = new Sci.Win.UI.DisplayBox();
            this.labelRemark = new Sci.Win.UI.Label();
            this.labelTeamLeader = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelNW = new Sci.Win.UI.Label();
            this.labelQty = new Sci.Win.UI.Label();
            this.labelPrice = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.displaySPNo = new Sci.Win.UI.DisplayBox();
            this.displaySEQ2 = new Sci.Win.UI.DisplayBox();
            this.labelSuppID = new Sci.Win.UI.Label();
            this.txtsupplierID = new Sci.Production.Class.Txtsupplier();
            this.txttpeuserTeamLeader = new Sci.Production.Class.Txttpeuser();
            this.txtunit_ftyUnit = new Sci.Production.Class.Txtunit_fty();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 319);
            this.btmcont.Size = new System.Drawing.Size(689, 40);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(599, 5);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(519, 5);
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(100, 257);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(572, 56);
            this.editRemark.TabIndex = 137;
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "BrandID", true));
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(487, 229);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(100, 23);
            this.displayBrand.TabIndex = 153;
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(413, 229);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(70, 23);
            this.labelBrand.TabIndex = 152;
            this.labelBrand.Text = "Brand";
            // 
            // txtReceiver
            // 
            this.txtReceiver.BackColor = System.Drawing.Color.White;
            this.txtReceiver.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtReceiver.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Receiver", true));
            this.txtReceiver.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReceiver.Location = new System.Drawing.Point(487, 176);
            this.txtReceiver.Name = "txtReceiver";
            this.txtReceiver.Size = new System.Drawing.Size(185, 23);
            this.txtReceiver.TabIndex = 134;
            // 
            // labelReceiver
            // 
            this.labelReceiver.Lines = 0;
            this.labelReceiver.Location = new System.Drawing.Point(413, 176);
            this.labelReceiver.Name = "labelReceiver";
            this.labelReceiver.Size = new System.Drawing.Size(70, 23);
            this.labelReceiver.TabIndex = 149;
            this.labelReceiver.Text = "Receiver";
            // 
            // numNW
            // 
            this.numNW.BackColor = System.Drawing.Color.White;
            this.numNW.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NW", true));
            this.numNW.DecimalPlaces = 2;
            this.numNW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numNW.Location = new System.Drawing.Point(100, 203);
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
            this.numNW.TabIndex = 133;
            this.numNW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelUnit
            // 
            this.labelUnit.Lines = 0;
            this.labelUnit.Location = new System.Drawing.Point(413, 149);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(70, 23);
            this.labelUnit.TabIndex = 148;
            this.labelUnit.Text = "Unit";
            // 
            // numQty
            // 
            this.numQty.BackColor = System.Drawing.Color.White;
            this.numQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Qty", true));
            this.numQty.DecimalPlaces = 2;
            this.numQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numQty.Location = new System.Drawing.Point(100, 176);
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
            this.numQty.TabIndex = 131;
            this.numQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtCTNNo
            // 
            this.txtCTNNo.BackColor = System.Drawing.Color.White;
            this.txtCTNNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CTNNo", true));
            this.txtCTNNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCTNNo.Location = new System.Drawing.Point(487, 122);
            this.txtCTNNo.Name = "txtCTNNo";
            this.txtCTNNo.Size = new System.Drawing.Size(90, 23);
            this.txtCTNNo.TabIndex = 130;
            this.txtCTNNo.Validated += new System.EventHandler(this.TxtCTNNo_Validated);
            // 
            // labelCTNNo
            // 
            this.labelCTNNo.Lines = 0;
            this.labelCTNNo.Location = new System.Drawing.Point(413, 122);
            this.labelCTNNo.Name = "labelCTNNo";
            this.labelCTNNo.Size = new System.Drawing.Size(70, 23);
            this.labelCTNNo.TabIndex = 147;
            this.labelCTNNo.Text = "CTN No.";
            // 
            // numPrice
            // 
            this.numPrice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numPrice.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Price", true));
            this.numPrice.DecimalPlaces = 4;
            this.numPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numPrice.IsSupportEditMode = false;
            this.numPrice.Location = new System.Drawing.Point(100, 149);
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
            this.numPrice.ReadOnly = true;
            this.numPrice.Size = new System.Drawing.Size(90, 23);
            this.numPrice.TabIndex = 129;
            this.numPrice.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MtlDesc", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editDescription.IsSupportEditMode = false;
            this.editDescription.Location = new System.Drawing.Point(100, 36);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.ReadOnly = true;
            this.editDescription.Size = new System.Drawing.Size(572, 82);
            this.editDescription.TabIndex = 128;
            // 
            // displaySEQ1
            // 
            this.displaySEQ1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySEQ1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Seq1", true));
            this.displaySEQ1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySEQ1.Location = new System.Drawing.Point(225, 9);
            this.displaySEQ1.Name = "displaySEQ1";
            this.displaySEQ1.Size = new System.Drawing.Size(30, 23);
            this.displaySEQ1.TabIndex = 146;
            // 
            // labelRemark
            // 
            this.labelRemark.Lines = 0;
            this.labelRemark.Location = new System.Drawing.Point(9, 257);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(87, 23);
            this.labelRemark.TabIndex = 145;
            this.labelRemark.Text = "Remark";
            // 
            // labelTeamLeader
            // 
            this.labelTeamLeader.Lines = 0;
            this.labelTeamLeader.Location = new System.Drawing.Point(9, 230);
            this.labelTeamLeader.Name = "labelTeamLeader";
            this.labelTeamLeader.Size = new System.Drawing.Size(87, 23);
            this.labelTeamLeader.TabIndex = 144;
            this.labelTeamLeader.Text = "Team Leader";
            // 
            // labelCategory
            // 
            this.labelCategory.Lines = 0;
            this.labelCategory.Location = new System.Drawing.Point(413, 203);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(70, 23);
            this.labelCategory.TabIndex = 143;
            this.labelCategory.Text = "Category";
            // 
            // labelNW
            // 
            this.labelNW.Lines = 0;
            this.labelNW.Location = new System.Drawing.Point(9, 203);
            this.labelNW.Name = "labelNW";
            this.labelNW.Size = new System.Drawing.Size(87, 23);
            this.labelNW.TabIndex = 142;
            this.labelNW.Text = "N.W. (kg)";
            // 
            // labelQty
            // 
            this.labelQty.Lines = 0;
            this.labelQty.Location = new System.Drawing.Point(9, 176);
            this.labelQty.Name = "labelQty";
            this.labelQty.Size = new System.Drawing.Size(87, 23);
            this.labelQty.TabIndex = 141;
            this.labelQty.Text = "Q\'ty";
            // 
            // labelPrice
            // 
            this.labelPrice.Lines = 0;
            this.labelPrice.Location = new System.Drawing.Point(9, 149);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.Size = new System.Drawing.Size(87, 23);
            this.labelPrice.TabIndex = 140;
            this.labelPrice.Text = "Price";
            // 
            // labelDescription
            // 
            this.labelDescription.Lines = 0;
            this.labelDescription.Location = new System.Drawing.Point(9, 36);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(87, 23);
            this.labelDescription.TabIndex = 139;
            this.labelDescription.Text = "Description";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(9, 9);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(87, 23);
            this.labelSPNo.TabIndex = 138;
            this.labelSPNo.Text = "SP#";
            // 
            // displaySPNo
            // 
            this.displaySPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySPNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "OrderID", true));
            this.displaySPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySPNo.Location = new System.Drawing.Point(100, 9);
            this.displaySPNo.Name = "displaySPNo";
            this.displaySPNo.Size = new System.Drawing.Size(120, 23);
            this.displaySPNo.TabIndex = 154;
            // 
            // displaySEQ2
            // 
            this.displaySEQ2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySEQ2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Seq2", true));
            this.displaySEQ2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySEQ2.Location = new System.Drawing.Point(257, 9);
            this.displaySEQ2.Name = "displaySEQ2";
            this.displaySEQ2.Size = new System.Drawing.Size(25, 23);
            this.displaySEQ2.TabIndex = 155;
            // 
            // labelSuppID
            // 
            this.labelSuppID.Lines = 0;
            this.labelSuppID.Location = new System.Drawing.Point(9, 122);
            this.labelSuppID.Name = "labelSuppID";
            this.labelSuppID.Size = new System.Drawing.Size(87, 23);
            this.labelSuppID.TabIndex = 157;
            this.labelSuppID.Text = "Supp ID";
            // 
            // txtsupplierID
            // 
            this.txtsupplierID.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "SuppID", true));
            this.txtsupplierID.DisplayBox1Binding = "";
            this.txtsupplierID.Location = new System.Drawing.Point(100, 122);
            this.txtsupplierID.Name = "txtsupplierID";
            this.txtsupplierID.Size = new System.Drawing.Size(147, 23);
            this.txtsupplierID.TabIndex = 158;
            this.txtsupplierID.TextBox1Binding = "";
            // 
            // txttpeuserTeamLeader
            // 
            this.txttpeuserTeamLeader.DataBindings.Add(new System.Windows.Forms.Binding("DisplayBox1Binding", this.mtbs, "Leader", true));
            this.txttpeuserTeamLeader.DisplayBox1Binding = "";
            this.txttpeuserTeamLeader.DisplayBox2Binding = "";
            this.txttpeuserTeamLeader.Location = new System.Drawing.Point(100, 230);
            this.txttpeuserTeamLeader.Name = "txttpeuserTeamLeader";
            this.txttpeuserTeamLeader.Size = new System.Drawing.Size(302, 23);
            this.txttpeuserTeamLeader.TabIndex = 151;
            // 
            // txtunit_ftyUnit
            // 
            this.txtunit_ftyUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtunit_ftyUnit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "UnitID", true));
            this.txtunit_ftyUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtunit_ftyUnit.IsSupportEditMode = false;
            this.txtunit_ftyUnit.Location = new System.Drawing.Point(487, 149);
            this.txtunit_ftyUnit.Name = "txtunit_ftyUnit";
            this.txtunit_ftyUnit.ReadOnly = true;
            this.txtunit_ftyUnit.Size = new System.Drawing.Size(66, 23);
            this.txtunit_ftyUnit.TabIndex = 132;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboCategory.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Category", true));
            this.comboCategory.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(487, 202);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.ReadOnly = true;
            this.comboCategory.Size = new System.Drawing.Size(90, 24);
            this.comboCategory.TabIndex = 159;
            // 
            // P02_EditFromPO
            // 
            this.ClientSize = new System.Drawing.Size(689, 359);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.txtsupplierID);
            this.Controls.Add(this.labelSuppID);
            this.Controls.Add(this.displaySEQ2);
            this.Controls.Add(this.displaySPNo);
            this.Controls.Add(this.editRemark);
            this.Controls.Add(this.displayBrand);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.txttpeuserTeamLeader);
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
            this.Controls.Add(this.displaySEQ1);
            this.Controls.Add(this.labelRemark);
            this.Controls.Add(this.labelTeamLeader);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelNW);
            this.Controls.Add(this.labelQty);
            this.Controls.Add(this.labelPrice);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelSPNo);
            this.Name = "P02_EditFromPO";
            this.Text = "International Air/Express - Import from Purchase";
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
            this.Controls.SetChildIndex(this.displaySEQ1, 0);
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
            this.Controls.SetChildIndex(this.txttpeuserTeamLeader, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.displayBrand, 0);
            this.Controls.SetChildIndex(this.editRemark, 0);
            this.Controls.SetChildIndex(this.displaySPNo, 0);
            this.Controls.SetChildIndex(this.displaySEQ2, 0);
            this.Controls.SetChildIndex(this.labelSuppID, 0);
            this.Controls.SetChildIndex(this.txtsupplierID, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.EditBox editRemark;
        private Win.UI.DisplayBox displayBrand;
        private Win.UI.Label labelBrand;
        private Class.Txttpeuser txttpeuserTeamLeader;
        private Win.UI.TextBox txtReceiver;
        private Win.UI.Label labelReceiver;
        private Win.UI.NumericBox numNW;
        private Class.Txtunit_fty txtunit_ftyUnit;
        private Win.UI.Label labelUnit;
        private Win.UI.NumericBox numQty;
        private Win.UI.TextBox txtCTNNo;
        private Win.UI.Label labelCTNNo;
        private Win.UI.NumericBox numPrice;
        private Win.UI.EditBox editDescription;
        private Win.UI.DisplayBox displaySEQ1;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelTeamLeader;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelNW;
        private Win.UI.Label labelQty;
        private Win.UI.Label labelPrice;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelSPNo;
        private Win.UI.DisplayBox displaySPNo;
        private Win.UI.DisplayBox displaySEQ2;
        private Win.UI.Label labelSuppID;
        private Class.Txtsupplier txtsupplierID;
        private Win.UI.ComboBox comboCategory;
    }
}
