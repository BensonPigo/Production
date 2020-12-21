namespace Sci.Production.Shipping
{
    partial class B49
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtSubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.label17 = new Sci.Win.UI.Label();
            this.label16 = new Sci.Win.UI.Label();
            this.numKGPcs = new Sci.Win.UI.NumericBox();
            this.numLengthPcs = new Sci.Win.UI.NumericBox();
            this.numWidthPcs = new Sci.Win.UI.NumericBox();
            this.labKG = new Sci.Win.UI.Label();
            this.labDesc = new Sci.Win.UI.Label();
            this.labSupplier = new Sci.Win.UI.Label();
            this.dispID = new Sci.Win.UI.DisplayBox();
            this.labType = new Sci.Win.UI.Label();
            this.labCustomsUnit = new Sci.Win.UI.Label();
            this.labHSCode = new Sci.Win.UI.Label();
            this.labCustomsCode = new Sci.Win.UI.Label();
            this.labUsageUnit = new Sci.Win.UI.Label();
            this.labModel = new Sci.Win.UI.Label();
            this.labMisc = new Sci.Win.UI.Label();
            this.labWidth = new Sci.Win.UI.Label();
            this.labLength = new Sci.Win.UI.Label();
            this.dispModel = new Sci.Win.UI.DisplayBox();
            this.txtunit_fty = new Sci.Production.Class.Txtunit_fty();
            this.dispType = new Sci.Win.UI.DisplayBox();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.dispHSCode = new Sci.Win.UI.DisplayBox();
            this.dispCustomerUnit = new Sci.Win.UI.DisplayBox();
            this.numericBoxMiscRate = new Sci.Win.UI.NumericBox();
            this.labMiscRate = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txtNLCode2 = new Sci.Production.Class.TxtNLCode();
            this.txtCustomerCode = new Sci.Production.Class.TxtNLCode();
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
            this.detail.Size = new System.Drawing.Size(758, 335);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtCustomerCode);
            this.detailcont.Controls.Add(this.txtNLCode2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.numericBoxMiscRate);
            this.detailcont.Controls.Add(this.labMiscRate);
            this.detailcont.Controls.Add(this.dispCustomerUnit);
            this.detailcont.Controls.Add(this.dispHSCode);
            this.detailcont.Controls.Add(this.editDescription);
            this.detailcont.Controls.Add(this.dispType);
            this.detailcont.Controls.Add(this.txtunit_fty);
            this.detailcont.Controls.Add(this.dispModel);
            this.detailcont.Controls.Add(this.labLength);
            this.detailcont.Controls.Add(this.labWidth);
            this.detailcont.Controls.Add(this.txtSubconSupplier);
            this.detailcont.Controls.Add(this.label17);
            this.detailcont.Controls.Add(this.label16);
            this.detailcont.Controls.Add(this.numKGPcs);
            this.detailcont.Controls.Add(this.numLengthPcs);
            this.detailcont.Controls.Add(this.numWidthPcs);
            this.detailcont.Controls.Add(this.labKG);
            this.detailcont.Controls.Add(this.labDesc);
            this.detailcont.Controls.Add(this.labSupplier);
            this.detailcont.Controls.Add(this.dispID);
            this.detailcont.Controls.Add(this.labType);
            this.detailcont.Controls.Add(this.labCustomsUnit);
            this.detailcont.Controls.Add(this.labHSCode);
            this.detailcont.Controls.Add(this.labCustomsCode);
            this.detailcont.Controls.Add(this.labUsageUnit);
            this.detailcont.Controls.Add(this.labModel);
            this.detailcont.Controls.Add(this.labMisc);
            this.detailcont.Size = new System.Drawing.Size(758, 297);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 297);
            this.detailbtm.Size = new System.Drawing.Size(758, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(758, 335);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(766, 364);
            // 
            // createby
            // 
            this.createby.Visible = false;
            // 
            // editby
            // 
            this.editby.Visible = false;
            // 
            // lblcreateby
            // 
            this.lblcreateby.Visible = false;
            // 
            // lbleditby
            // 
            this.lbleditby.Visible = false;
            // 
            // txtSubconSupplier
            // 
            this.txtSubconSupplier.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Suppid", true));
            this.txtSubconSupplier.DisplayBox1Binding = "";
            this.txtSubconSupplier.IsIncludeJunk = false;
            this.txtSubconSupplier.IsMisc = false;
            this.txtSubconSupplier.IsShipping = false;
            this.txtSubconSupplier.IsSubcon = false;
            this.txtSubconSupplier.Location = new System.Drawing.Point(434, 62);
            this.txtSubconSupplier.Name = "txtSubconSupplier";
            this.txtSubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtSubconSupplier.TabIndex = 59;
            this.txtSubconSupplier.TextBox1Binding = "";
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Location = new System.Drawing.Point(513, 189);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(16, 23);
            this.label17.TabIndex = 58;
            this.label17.Text = "M";
            this.label17.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label17.TextStyle.Color = System.Drawing.Color.Black;
            this.label17.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label17.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Location = new System.Drawing.Point(513, 157);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(16, 23);
            this.label16.TabIndex = 57;
            this.label16.Text = "M";
            this.label16.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label16.TextStyle.Color = System.Drawing.Color.Black;
            this.label16.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label16.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // numKGPcs
            // 
            this.numKGPcs.BackColor = System.Drawing.Color.White;
            this.numKGPcs.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PcsKg", true));
            this.numKGPcs.DecimalPlaces = 4;
            this.numKGPcs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numKGPcs.Location = new System.Drawing.Point(434, 221);
            this.numKGPcs.Name = "numKGPcs";
            this.numKGPcs.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numKGPcs.Size = new System.Drawing.Size(56, 23);
            this.numKGPcs.TabIndex = 5;
            this.numKGPcs.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numLengthPcs
            // 
            this.numLengthPcs.BackColor = System.Drawing.Color.White;
            this.numLengthPcs.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PcsLength", true));
            this.numLengthPcs.DecimalPlaces = 4;
            this.numLengthPcs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numLengthPcs.Location = new System.Drawing.Point(434, 189);
            this.numLengthPcs.Name = "numLengthPcs";
            this.numLengthPcs.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numLengthPcs.Size = new System.Drawing.Size(76, 23);
            this.numLengthPcs.TabIndex = 4;
            this.numLengthPcs.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numWidthPcs
            // 
            this.numWidthPcs.BackColor = System.Drawing.Color.White;
            this.numWidthPcs.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PcsWidth", true));
            this.numWidthPcs.DecimalPlaces = 4;
            this.numWidthPcs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numWidthPcs.Location = new System.Drawing.Point(434, 157);
            this.numWidthPcs.Name = "numWidthPcs";
            this.numWidthPcs.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWidthPcs.Size = new System.Drawing.Size(76, 23);
            this.numWidthPcs.TabIndex = 3;
            this.numWidthPcs.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labKG
            // 
            this.labKG.Location = new System.Drawing.Point(354, 221);
            this.labKG.Name = "labKG";
            this.labKG.Size = new System.Drawing.Size(75, 23);
            this.labKG.TabIndex = 56;
            this.labKG.Text = "KG/Pcs";
            // 
            // labDesc
            // 
            this.labDesc.Location = new System.Drawing.Point(354, 95);
            this.labDesc.Name = "labDesc";
            this.labDesc.Size = new System.Drawing.Size(75, 23);
            this.labDesc.TabIndex = 55;
            this.labDesc.Text = "Description";
            // 
            // labSupplier
            // 
            this.labSupplier.Location = new System.Drawing.Point(354, 63);
            this.labSupplier.Name = "labSupplier";
            this.labSupplier.Size = new System.Drawing.Size(75, 23);
            this.labSupplier.TabIndex = 54;
            this.labSupplier.Text = "Supplier";
            // 
            // dispID
            // 
            this.dispID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.dispID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispID.Location = new System.Drawing.Point(157, 30);
            this.dispID.Name = "dispID";
            this.dispID.Size = new System.Drawing.Size(149, 23);
            this.dispID.TabIndex = 48;
            // 
            // labType
            // 
            this.labType.Location = new System.Drawing.Point(354, 30);
            this.labType.Name = "labType";
            this.labType.Size = new System.Drawing.Size(75, 23);
            this.labType.TabIndex = 47;
            this.labType.Text = "Type";
            // 
            // labCustomsUnit
            // 
            this.labCustomsUnit.Location = new System.Drawing.Point(19, 253);
            this.labCustomsUnit.Name = "labCustomsUnit";
            this.labCustomsUnit.Size = new System.Drawing.Size(134, 23);
            this.labCustomsUnit.TabIndex = 45;
            this.labCustomsUnit.Text = "Customs Unit";
            // 
            // labHSCode
            // 
            this.labHSCode.Location = new System.Drawing.Point(19, 221);
            this.labHSCode.Name = "labHSCode";
            this.labHSCode.Size = new System.Drawing.Size(134, 23);
            this.labHSCode.TabIndex = 44;
            this.labHSCode.Text = "HS Code";
            // 
            // labCustomsCode
            // 
            this.labCustomsCode.Location = new System.Drawing.Point(19, 157);
            this.labCustomsCode.Name = "labCustomsCode";
            this.labCustomsCode.Size = new System.Drawing.Size(134, 23);
            this.labCustomsCode.TabIndex = 43;
            this.labCustomsCode.Text = "Customs Code";
            // 
            // labUsageUnit
            // 
            this.labUsageUnit.Location = new System.Drawing.Point(19, 94);
            this.labUsageUnit.Name = "labUsageUnit";
            this.labUsageUnit.Size = new System.Drawing.Size(134, 23);
            this.labUsageUnit.TabIndex = 41;
            this.labUsageUnit.Text = "Usage Unit";
            // 
            // labModel
            // 
            this.labModel.Location = new System.Drawing.Point(19, 62);
            this.labModel.Name = "labModel";
            this.labModel.Size = new System.Drawing.Size(134, 23);
            this.labModel.TabIndex = 39;
            this.labModel.Text = "Model";
            // 
            // labMisc
            // 
            this.labMisc.Location = new System.Drawing.Point(19, 30);
            this.labMisc.Name = "labMisc";
            this.labMisc.Size = new System.Drawing.Size(134, 23);
            this.labMisc.TabIndex = 35;
            this.labMisc.Text = "Misc ID";
            // 
            // labWidth
            // 
            this.labWidth.Location = new System.Drawing.Point(354, 157);
            this.labWidth.Name = "labWidth";
            this.labWidth.Size = new System.Drawing.Size(75, 23);
            this.labWidth.TabIndex = 60;
            this.labWidth.Text = "Width/Pcs";
            // 
            // labLength
            // 
            this.labLength.Location = new System.Drawing.Point(354, 189);
            this.labLength.Name = "labLength";
            this.labLength.Size = new System.Drawing.Size(75, 23);
            this.labLength.TabIndex = 61;
            this.labLength.Text = "Length/Pcs";
            // 
            // dispModel
            // 
            this.dispModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispModel.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Model", true));
            this.dispModel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispModel.Location = new System.Drawing.Point(157, 62);
            this.dispModel.Name = "dispModel";
            this.dispModel.Size = new System.Drawing.Size(149, 23);
            this.dispModel.TabIndex = 62;
            // 
            // txtunit_fty
            // 
            this.txtunit_fty.BackColor = System.Drawing.Color.White;
            this.txtunit_fty.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "UsageUnit", true));
            this.txtunit_fty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtunit_fty.Location = new System.Drawing.Point(157, 94);
            this.txtunit_fty.Name = "txtunit_fty";
            this.txtunit_fty.Size = new System.Drawing.Size(100, 23);
            this.txtunit_fty.TabIndex = 0;
            // 
            // dispType
            // 
            this.dispType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispType.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PurchaseType", true));
            this.dispType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispType.Location = new System.Drawing.Point(434, 30);
            this.dispType.Name = "dispType";
            this.dispType.Size = new System.Drawing.Size(190, 23);
            this.dispType.TabIndex = 64;
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editDescription.IsSupportEditMode = false;
            this.editDescription.Location = new System.Drawing.Point(434, 91);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.ReadOnly = true;
            this.editDescription.Size = new System.Drawing.Size(241, 57);
            this.editDescription.TabIndex = 65;
            // 
            // dispHSCode
            // 
            this.dispHSCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispHSCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "HSCode", true));
            this.dispHSCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispHSCode.Location = new System.Drawing.Point(157, 221);
            this.dispHSCode.Name = "dispHSCode";
            this.dispHSCode.Size = new System.Drawing.Size(100, 23);
            this.dispHSCode.TabIndex = 67;
            // 
            // dispCustomerUnit
            // 
            this.dispCustomerUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dispCustomerUnit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CustomsUnit", true));
            this.dispCustomerUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dispCustomerUnit.Location = new System.Drawing.Point(157, 253);
            this.dispCustomerUnit.Name = "dispCustomerUnit";
            this.dispCustomerUnit.Size = new System.Drawing.Size(149, 23);
            this.dispCustomerUnit.TabIndex = 68;
            // 
            // numericBoxMiscRate
            // 
            this.numericBoxMiscRate.BackColor = System.Drawing.Color.White;
            this.numericBoxMiscRate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MiscRate", true));
            this.numericBoxMiscRate.DecimalPlaces = 4;
            this.numericBoxMiscRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBoxMiscRate.Location = new System.Drawing.Point(157, 125);
            this.numericBoxMiscRate.Name = "numericBoxMiscRate";
            this.numericBoxMiscRate.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBoxMiscRate.Size = new System.Drawing.Size(100, 23);
            this.numericBoxMiscRate.TabIndex = 1;
            this.numericBoxMiscRate.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labMiscRate
            // 
            this.labMiscRate.Location = new System.Drawing.Point(19, 125);
            this.labMiscRate.Name = "labMiscRate";
            this.labMiscRate.Size = new System.Drawing.Size(134, 23);
            this.labMiscRate.TabIndex = 70;
            this.labMiscRate.Text = "Misc Rate";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 189);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 23);
            this.label1.TabIndex = 71;
            this.label1.Text = "Customs Code(2021)";
            // 
            // txtNLCode2
            // 
            this.txtNLCode2.BackColor = System.Drawing.Color.White;
            this.txtNLCode2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "NLCode2", true));
            this.txtNLCode2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtNLCode2.Location = new System.Drawing.Point(157, 189);
            this.txtNLCode2.Name = "txtNLCode2";
            this.txtNLCode2.Size = new System.Drawing.Size(100, 23);
            this.txtNLCode2.TabIndex = 72;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.BackColor = System.Drawing.Color.White;
            this.txtCustomerCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "NLCode", true));
            this.txtCustomerCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCustomerCode.Location = new System.Drawing.Point(157, 157);
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Size = new System.Drawing.Size(100, 23);
            this.txtCustomerCode.TabIndex = 73;
            // 
            // B49
            // 
            this.ClientSize = new System.Drawing.Size(766, 397);
            this.ConnectionName = "Machine";
            this.EditMode = true;
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B49";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B49. Customs Code – Miscellaneous";
            this.WorkAlias = "Misc";
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
        private Class.TxtsubconNoConfirm txtSubconSupplier;
        private Win.UI.Label label17;
        private Win.UI.Label label16;
        private Win.UI.NumericBox numKGPcs;
        private Win.UI.NumericBox numLengthPcs;
        private Win.UI.NumericBox numWidthPcs;
        private Win.UI.Label labKG;
        private Win.UI.Label labDesc;
        private Win.UI.Label labSupplier;
        private Win.UI.DisplayBox dispID;
        private Win.UI.Label labType;
        private Win.UI.Label labCustomsUnit;
        private Win.UI.Label labHSCode;
        private Win.UI.Label labCustomsCode;
        private Win.UI.Label labUsageUnit;
        private Win.UI.Label labModel;
        private Win.UI.Label labMisc;
        private Win.UI.DisplayBox dispModel;
        private Win.UI.Label labLength;
        private Win.UI.Label labWidth;
        private Win.UI.DisplayBox dispType;
        private Class.Txtunit_fty txtunit_fty;
        private Win.UI.EditBox editDescription;
        private Win.UI.DisplayBox dispCustomerUnit;
        private Win.UI.DisplayBox dispHSCode;
        private Win.UI.NumericBox numericBoxMiscRate;
        private Win.UI.Label labMiscRate;
        private Win.UI.Label label1;
        private Class.TxtNLCode txtCustomerCode;
        private Class.TxtNLCode txtNLCode2;
    }
}
