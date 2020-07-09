namespace Sci.Production.Shipping
{
    partial class B52
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
            this.numWeight = new Sci.Win.UI.NumericBox();
            this.labelWeight = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.displayCustomsUnit = new Sci.Win.UI.DisplayBox();
            this.txtUnitUsageUnit = new Sci.Production.Class.Txtunit();
            this.displayMaterialType = new Sci.Win.UI.DisplayBox();
            this.comboType = new Sci.Win.UI.ComboBox();
            this.editDescDetail = new Sci.Win.UI.EditBox();
            this.displayDescription = new Sci.Win.UI.DisplayBox();
            this.displayRefNo = new Sci.Win.UI.DisplayBox();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelCustomsUnit = new Sci.Win.UI.Label();
            this.labelGoodsDescription = new Sci.Win.UI.Label();
            this.labelUsageUnit = new Sci.Win.UI.Label();
            this.labelMaterialType = new Sci.Win.UI.Label();
            this.labelType = new Sci.Win.UI.Label();
            this.labelDescDetail = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelRefNo = new Sci.Win.UI.Label();
            this.txtGoodsDescription = new Sci.Win.UI.TextBox();
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
            this.detail.Size = new System.Drawing.Size(829, 338);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtGoodsDescription);
            this.detailcont.Controls.Add(this.displayBrand);
            this.detailcont.Controls.Add(this.numWeight);
            this.detailcont.Controls.Add(this.labelWeight);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.displayCustomsUnit);
            this.detailcont.Controls.Add(this.txtUnitUsageUnit);
            this.detailcont.Controls.Add(this.displayMaterialType);
            this.detailcont.Controls.Add(this.comboType);
            this.detailcont.Controls.Add(this.editDescDetail);
            this.detailcont.Controls.Add(this.displayDescription);
            this.detailcont.Controls.Add(this.displayRefNo);
            this.detailcont.Controls.Add(this.labelBrand);
            this.detailcont.Controls.Add(this.labelCustomsUnit);
            this.detailcont.Controls.Add(this.labelGoodsDescription);
            this.detailcont.Controls.Add(this.labelUsageUnit);
            this.detailcont.Controls.Add(this.labelMaterialType);
            this.detailcont.Controls.Add(this.labelType);
            this.detailcont.Controls.Add(this.labelDescDetail);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelRefNo);
            this.detailcont.Size = new System.Drawing.Size(829, 300);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 300);
            this.detailbtm.Size = new System.Drawing.Size(829, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(829, 338);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(837, 367);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "BrandID", true));
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(500, 13);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(94, 23);
            this.displayBrand.TabIndex = 66;
            // 
            // numWeight
            // 
            this.numWeight.BackColor = System.Drawing.Color.White;
            this.numWeight.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PcsKg", true));
            this.numWeight.DecimalPlaces = 4;
            this.numWeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numWeight.Location = new System.Drawing.Point(550, 218);
            this.numWeight.Name = "numWeight";
            this.numWeight.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWeight.Size = new System.Drawing.Size(80, 23);
            this.numWeight.TabIndex = 41;
            this.numWeight.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelWeight
            // 
            this.labelWeight.Lines = 0;
            this.labelWeight.Location = new System.Drawing.Point(454, 218);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(92, 40);
            this.labelWeight.TabIndex = 61;
            this.labelWeight.Text = "Weight (kgs/PX)";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(325, 13);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 57;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // displayCustomsUnit
            // 
            this.displayCustomsUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCustomsUnit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CustomsUnit", true));
            this.displayCustomsUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCustomsUnit.Location = new System.Drawing.Point(550, 186);
            this.displayCustomsUnit.Name = "displayCustomsUnit";
            this.displayCustomsUnit.Size = new System.Drawing.Size(80, 23);
            this.displayCustomsUnit.TabIndex = 56;
            // 
            // txtUnitUsageUnit
            // 
            this.txtUnitUsageUnit.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "UsageUnit", true));
            this.txtUnitUsageUnit.DisplayBox1Binding = "";
            this.txtUnitUsageUnit.Location = new System.Drawing.Point(109, 186);
            this.txtUnitUsageUnit.Name = "txtUnitUsageUnit";
            this.txtUnitUsageUnit.Size = new System.Drawing.Size(320, 23);
            this.txtUnitUsageUnit.TabIndex = 54;
            this.txtUnitUsageUnit.TextBox1Binding = "";
            // 
            // displayMaterialType
            // 
            this.displayMaterialType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayMaterialType.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MtlTypeID", true));
            this.displayMaterialType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayMaterialType.Location = new System.Drawing.Point(550, 154);
            this.displayMaterialType.Name = "displayMaterialType";
            this.displayMaterialType.Size = new System.Drawing.Size(190, 23);
            this.displayMaterialType.TabIndex = 53;
            // 
            // comboType
            // 
            this.comboType.BackColor = System.Drawing.Color.White;
            this.comboType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Type", true));
            this.comboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType.FormattingEnabled = true;
            this.comboType.IsSupportUnselect = true;
            this.comboType.Location = new System.Drawing.Point(109, 154);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(121, 24);
            this.comboType.TabIndex = 52;
            // 
            // editDescDetail
            // 
            this.editDescDetail.BackColor = System.Drawing.Color.White;
            this.editDescDetail.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescDetail", true));
            this.editDescDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescDetail.Location = new System.Drawing.Point(109, 75);
            this.editDescDetail.Multiline = true;
            this.editDescDetail.Name = "editDescDetail";
            this.editDescDetail.Size = new System.Drawing.Size(654, 70);
            this.editDescDetail.TabIndex = 51;
            // 
            // displayDescription
            // 
            this.displayDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDescription.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Description", true));
            this.displayDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDescription.Location = new System.Drawing.Point(109, 45);
            this.displayDescription.Name = "displayDescription";
            this.displayDescription.Size = new System.Drawing.Size(654, 23);
            this.displayDescription.TabIndex = 50;
            // 
            // displayRefNo
            // 
            this.displayRefNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRefNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "RefNo", true));
            this.displayRefNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRefNo.Location = new System.Drawing.Point(109, 13);
            this.displayRefNo.Name = "displayRefNo";
            this.displayRefNo.Size = new System.Drawing.Size(190, 23);
            this.displayRefNo.TabIndex = 49;
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(411, 13);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(85, 23);
            this.labelBrand.TabIndex = 48;
            this.labelBrand.Text = "Brand";
            // 
            // labelCustomsUnit
            // 
            this.labelCustomsUnit.Lines = 0;
            this.labelCustomsUnit.Location = new System.Drawing.Point(454, 186);
            this.labelCustomsUnit.Name = "labelCustomsUnit";
            this.labelCustomsUnit.Size = new System.Drawing.Size(92, 23);
            this.labelCustomsUnit.TabIndex = 47;
            this.labelCustomsUnit.Text = "Customs Unit";
            // 
            // labelGoodsDescription
            // 
            this.labelGoodsDescription.Lines = 0;
            this.labelGoodsDescription.Location = new System.Drawing.Point(13, 218);
            this.labelGoodsDescription.Name = "labelGoodsDescription";
            this.labelGoodsDescription.Size = new System.Drawing.Size(121, 23);
            this.labelGoodsDescription.TabIndex = 45;
            this.labelGoodsDescription.Text = "Good\'s Description";
            // 
            // labelUsageUnit
            // 
            this.labelUsageUnit.Lines = 0;
            this.labelUsageUnit.Location = new System.Drawing.Point(13, 186);
            this.labelUsageUnit.Name = "labelUsageUnit";
            this.labelUsageUnit.Size = new System.Drawing.Size(92, 23);
            this.labelUsageUnit.TabIndex = 44;
            this.labelUsageUnit.Text = "Usage Unit";
            // 
            // labelMaterialType
            // 
            this.labelMaterialType.Lines = 0;
            this.labelMaterialType.Location = new System.Drawing.Point(454, 154);
            this.labelMaterialType.Name = "labelMaterialType";
            this.labelMaterialType.Size = new System.Drawing.Size(92, 23);
            this.labelMaterialType.TabIndex = 42;
            this.labelMaterialType.Text = "Material Type";
            // 
            // labelType
            // 
            this.labelType.Lines = 0;
            this.labelType.Location = new System.Drawing.Point(13, 154);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(92, 23);
            this.labelType.TabIndex = 40;
            this.labelType.Text = "Type";
            // 
            // labelDescDetail
            // 
            this.labelDescDetail.Lines = 0;
            this.labelDescDetail.Location = new System.Drawing.Point(13, 77);
            this.labelDescDetail.Name = "labelDescDetail";
            this.labelDescDetail.Size = new System.Drawing.Size(92, 46);
            this.labelDescDetail.TabIndex = 39;
            this.labelDescDetail.Text = "Description";
            this.labelDescDetail.TextStyle.Alignment = System.Drawing.ContentAlignment.TopLeft;
            // 
            // labelDescription
            // 
            this.labelDescription.Lines = 0;
            this.labelDescription.Location = new System.Drawing.Point(13, 45);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(92, 23);
            this.labelDescription.TabIndex = 36;
            this.labelDescription.Text = "Description";
            // 
            // labelRefNo
            // 
            this.labelRefNo.Lines = 0;
            this.labelRefNo.Location = new System.Drawing.Point(13, 13);
            this.labelRefNo.Name = "labelRefNo";
            this.labelRefNo.Size = new System.Drawing.Size(92, 23);
            this.labelRefNo.TabIndex = 35;
            this.labelRefNo.Text = "RefNo";
            // 
            // txtGoodsDescription
            // 
            this.txtGoodsDescription.BackColor = System.Drawing.Color.White;
            this.txtGoodsDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGoodsDescription.Location = new System.Drawing.Point(138, 218);
            this.txtGoodsDescription.Name = "txtGoodsDescription";
            this.txtGoodsDescription.Size = new System.Drawing.Size(263, 23);
            this.txtGoodsDescription.TabIndex = 67;
            this.txtGoodsDescription.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtGoodsDescription_PopUp);
            this.txtGoodsDescription.Validating += new System.ComponentModel.CancelEventHandler(this.TxtGoodsDescription_Validating);
            // 
            // B52
            // 
            this.ClientSize = new System.Drawing.Size(837, 400);
            this.DefaultOrder = "RefNo";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B52";
            this.Text = "B52. Material Basic data - Fabric/Accessory";
            this.UniqueExpress = "SCIRefno";
            this.WorkAlias = "Fabric";
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

        private Win.UI.DisplayBox displayBrand;
        private Win.UI.NumericBox numWeight;
        private Win.UI.Label labelWeight;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.DisplayBox displayCustomsUnit;
        private Class.Txtunit txtUnitUsageUnit;
        private Win.UI.DisplayBox displayMaterialType;
        private Win.UI.ComboBox comboType;
        private Win.UI.EditBox editDescDetail;
        private Win.UI.DisplayBox displayDescription;
        private Win.UI.DisplayBox displayRefNo;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelCustomsUnit;
        private Win.UI.Label labelGoodsDescription;
        private Win.UI.Label labelUsageUnit;
        private Win.UI.Label labelMaterialType;
        private Win.UI.Label labelType;
        private Win.UI.Label labelDescDetail;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelRefNo;
        private Win.UI.TextBox txtGoodsDescription;
    }
}
