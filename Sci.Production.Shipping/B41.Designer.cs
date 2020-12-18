namespace Sci.Production.Shipping
{
    partial class B41
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
            this.labelRefNo = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelMaterialType = new Sci.Win.UI.Label();
            this.labelUnit = new Sci.Win.UI.Label();
            this.labelNLCode = new Sci.Win.UI.Label();
            this.labelHSCode = new Sci.Win.UI.Label();
            this.labelCustomsUnit = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.displayRefNo = new Sci.Win.UI.DisplayBox();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.displayMaterialType = new Sci.Win.UI.DisplayBox();
            this.displayHSCode = new Sci.Win.UI.DisplayBox();
            this.displayCustomsUnit = new Sci.Win.UI.DisplayBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.labelWidthPcs = new Sci.Win.UI.Label();
            this.labelLengthPcs = new Sci.Win.UI.Label();
            this.labelKGPcs = new Sci.Win.UI.Label();
            this.numWidthPcs = new Sci.Win.UI.NumericBox();
            this.numLengthPcs = new Sci.Win.UI.NumericBox();
            this.numKGPcs = new Sci.Win.UI.NumericBox();
            this.label16 = new Sci.Win.UI.Label();
            this.label17 = new Sci.Win.UI.Label();
            this.checkNoNeedToDeclare = new Sci.Win.UI.CheckBox();
            this.txtSubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.displayUnit = new Sci.Win.UI.DisplayBox();
            this.txtNLCode2 = new Sci.Production.Class.TxtNLCode();
            this.label1 = new Sci.Win.UI.Label();
            this.txtNLCode = new Sci.Production.Class.TxtNLCode();
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
            this.detail.Size = new System.Drawing.Size(737, 310);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtNLCode);
            this.detailcont.Controls.Add(this.txtNLCode2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.displayUnit);
            this.detailcont.Controls.Add(this.txtSubconSupplier);
            this.detailcont.Controls.Add(this.checkNoNeedToDeclare);
            this.detailcont.Controls.Add(this.label17);
            this.detailcont.Controls.Add(this.label16);
            this.detailcont.Controls.Add(this.numKGPcs);
            this.detailcont.Controls.Add(this.numLengthPcs);
            this.detailcont.Controls.Add(this.numWidthPcs);
            this.detailcont.Controls.Add(this.labelKGPcs);
            this.detailcont.Controls.Add(this.labelLengthPcs);
            this.detailcont.Controls.Add(this.labelWidthPcs);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.displayCustomsUnit);
            this.detailcont.Controls.Add(this.displayHSCode);
            this.detailcont.Controls.Add(this.displayMaterialType);
            this.detailcont.Controls.Add(this.editDescription);
            this.detailcont.Controls.Add(this.displayRefNo);
            this.detailcont.Controls.Add(this.labelSupplier);
            this.detailcont.Controls.Add(this.labelCustomsUnit);
            this.detailcont.Controls.Add(this.labelHSCode);
            this.detailcont.Controls.Add(this.labelNLCode);
            this.detailcont.Controls.Add(this.labelUnit);
            this.detailcont.Controls.Add(this.labelMaterialType);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelRefNo);
            this.detailcont.Size = new System.Drawing.Size(737, 310);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 310);
            this.detailbtm.Size = new System.Drawing.Size(737, 0);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(737, 310);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(745, 339);
            // 
            // createby
            // 
            this.createby.Location = new System.Drawing.Point(70, -31);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(400, -31);
            // 
            // lblcreateby
            // 
            this.lblcreateby.Location = new System.Drawing.Point(5, -25);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(352, -25);
            // 
            // labelRefNo
            // 
            this.labelRefNo.Location = new System.Drawing.Point(15, 13);
            this.labelRefNo.Name = "labelRefNo";
            this.labelRefNo.Size = new System.Drawing.Size(90, 23);
            this.labelRefNo.TabIndex = 0;
            this.labelRefNo.Text = "RefNo";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(15, 45);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(90, 23);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = "Description";
            // 
            // labelMaterialType
            // 
            this.labelMaterialType.Location = new System.Drawing.Point(15, 122);
            this.labelMaterialType.Name = "labelMaterialType";
            this.labelMaterialType.Size = new System.Drawing.Size(90, 23);
            this.labelMaterialType.TabIndex = 4;
            this.labelMaterialType.Text = "Material Type";
            // 
            // labelUnit
            // 
            this.labelUnit.Location = new System.Drawing.Point(15, 154);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(90, 23);
            this.labelUnit.TabIndex = 5;
            this.labelUnit.Text = "Unit";
            // 
            // labelNLCode
            // 
            this.labelNLCode.Location = new System.Drawing.Point(15, 186);
            this.labelNLCode.Name = "labelNLCode";
            this.labelNLCode.Size = new System.Drawing.Size(90, 23);
            this.labelNLCode.TabIndex = 6;
            this.labelNLCode.Text = "Customs Code";
            // 
            // labelHSCode
            // 
            this.labelHSCode.Location = new System.Drawing.Point(15, 218);
            this.labelHSCode.Name = "labelHSCode";
            this.labelHSCode.Size = new System.Drawing.Size(90, 23);
            this.labelHSCode.TabIndex = 7;
            this.labelHSCode.Text = "HS Code";
            // 
            // labelCustomsUnit
            // 
            this.labelCustomsUnit.Location = new System.Drawing.Point(15, 250);
            this.labelCustomsUnit.Name = "labelCustomsUnit";
            this.labelCustomsUnit.Size = new System.Drawing.Size(90, 23);
            this.labelCustomsUnit.TabIndex = 8;
            this.labelCustomsUnit.Text = "Customs Unit";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(464, 13);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(75, 23);
            this.labelSupplier.TabIndex = 9;
            this.labelSupplier.Text = "Supplier";
            // 
            // displayRefNo
            // 
            this.displayRefNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRefNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "RefNo", true));
            this.displayRefNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRefNo.Location = new System.Drawing.Point(109, 13);
            this.displayRefNo.Name = "displayRefNo";
            this.displayRefNo.Size = new System.Drawing.Size(190, 23);
            this.displayRefNo.TabIndex = 10;
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(109, 43);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(585, 70);
            this.editDescription.TabIndex = 12;
            // 
            // displayMaterialType
            // 
            this.displayMaterialType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayMaterialType.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Category", true));
            this.displayMaterialType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayMaterialType.Location = new System.Drawing.Point(109, 122);
            this.displayMaterialType.Name = "displayMaterialType";
            this.displayMaterialType.Size = new System.Drawing.Size(190, 23);
            this.displayMaterialType.TabIndex = 14;
            // 
            // displayHSCode
            // 
            this.displayHSCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayHSCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "HSCode", true));
            this.displayHSCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayHSCode.Location = new System.Drawing.Point(109, 218);
            this.displayHSCode.Name = "displayHSCode";
            this.displayHSCode.Size = new System.Drawing.Size(100, 23);
            this.displayHSCode.TabIndex = 17;
            // 
            // displayCustomsUnit
            // 
            this.displayCustomsUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCustomsUnit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CustomsUnit", true));
            this.displayCustomsUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCustomsUnit.Location = new System.Drawing.Point(109, 250);
            this.displayCustomsUnit.Name = "displayCustomsUnit";
            this.displayCustomsUnit.Size = new System.Drawing.Size(80, 23);
            this.displayCustomsUnit.TabIndex = 18;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(325, 13);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 19;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // labelWidthPcs
            // 
            this.labelWidthPcs.Location = new System.Drawing.Point(464, 122);
            this.labelWidthPcs.Name = "labelWidthPcs";
            this.labelWidthPcs.Size = new System.Drawing.Size(75, 23);
            this.labelWidthPcs.TabIndex = 21;
            this.labelWidthPcs.Text = "Width/Pcs";
            // 
            // labelLengthPcs
            // 
            this.labelLengthPcs.Location = new System.Drawing.Point(464, 154);
            this.labelLengthPcs.Name = "labelLengthPcs";
            this.labelLengthPcs.Size = new System.Drawing.Size(75, 23);
            this.labelLengthPcs.TabIndex = 22;
            this.labelLengthPcs.Text = "Length/Pcs";
            // 
            // labelKGPcs
            // 
            this.labelKGPcs.Location = new System.Drawing.Point(464, 186);
            this.labelKGPcs.Name = "labelKGPcs";
            this.labelKGPcs.Size = new System.Drawing.Size(75, 23);
            this.labelKGPcs.TabIndex = 23;
            this.labelKGPcs.Text = "KG/Pcs";
            // 
            // numWidthPcs
            // 
            this.numWidthPcs.BackColor = System.Drawing.Color.White;
            this.numWidthPcs.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PcsWidth", true));
            this.numWidthPcs.DecimalPlaces = 4;
            this.numWidthPcs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numWidthPcs.Location = new System.Drawing.Point(544, 122);
            this.numWidthPcs.Name = "numWidthPcs";
            this.numWidthPcs.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWidthPcs.Size = new System.Drawing.Size(76, 23);
            this.numWidthPcs.TabIndex = 1;
            this.numWidthPcs.Value = new decimal(new int[] {
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
            this.numLengthPcs.Location = new System.Drawing.Point(544, 154);
            this.numLengthPcs.Name = "numLengthPcs";
            this.numLengthPcs.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numLengthPcs.Size = new System.Drawing.Size(76, 23);
            this.numLengthPcs.TabIndex = 2;
            this.numLengthPcs.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numKGPcs
            // 
            this.numKGPcs.BackColor = System.Drawing.Color.White;
            this.numKGPcs.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PcsKg", true));
            this.numKGPcs.DecimalPlaces = 4;
            this.numKGPcs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numKGPcs.Location = new System.Drawing.Point(544, 186);
            this.numKGPcs.Name = "numKGPcs";
            this.numKGPcs.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numKGPcs.Size = new System.Drawing.Size(56, 23);
            this.numKGPcs.TabIndex = 3;
            this.numKGPcs.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Location = new System.Drawing.Point(623, 122);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(16, 23);
            this.label16.TabIndex = 30;
            this.label16.Text = "M";
            this.label16.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label16.TextStyle.Color = System.Drawing.Color.Black;
            this.label16.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label16.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Location = new System.Drawing.Point(623, 154);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(16, 23);
            this.label17.TabIndex = 31;
            this.label17.Text = "M";
            this.label17.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label17.TextStyle.Color = System.Drawing.Color.Black;
            this.label17.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label17.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // checkNoNeedToDeclare
            // 
            this.checkNoNeedToDeclare.AutoSize = true;
            this.checkNoNeedToDeclare.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NoDeclare", true));
            this.checkNoNeedToDeclare.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkNoNeedToDeclare.Location = new System.Drawing.Point(464, 218);
            this.checkNoNeedToDeclare.Name = "checkNoNeedToDeclare";
            this.checkNoNeedToDeclare.Size = new System.Drawing.Size(148, 21);
            this.checkNoNeedToDeclare.TabIndex = 4;
            this.checkNoNeedToDeclare.Text = "No need to declare";
            this.checkNoNeedToDeclare.UseVisualStyleBackColor = true;
            // 
            // txtSubconSupplier
            // 
            this.txtSubconSupplier.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LocalSuppid", true));
            this.txtSubconSupplier.DisplayBox1Binding = "";
            this.txtSubconSupplier.IsIncludeJunk = false;
            this.txtSubconSupplier.IsMisc = false;
            this.txtSubconSupplier.IsShipping = false;
            this.txtSubconSupplier.IsSubcon = false;
            this.txtSubconSupplier.Location = new System.Drawing.Point(544, 13);
            this.txtSubconSupplier.Name = "txtSubconSupplier";
            this.txtSubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtSubconSupplier.TabIndex = 33;
            this.txtSubconSupplier.TextBox1Binding = "";
            // 
            // displayUnit
            // 
            this.displayUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayUnit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "UnitID", true));
            this.displayUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayUnit.Location = new System.Drawing.Point(109, 154);
            this.displayUnit.Name = "displayUnit";
            this.displayUnit.Size = new System.Drawing.Size(80, 23);
            this.displayUnit.TabIndex = 34;
            // 
            // txtNLCode2
            // 
            this.txtNLCode2.BackColor = System.Drawing.Color.White;
            this.txtNLCode2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "NLCode2", true));
            this.txtNLCode2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtNLCode2.Location = new System.Drawing.Point(351, 186);
            this.txtNLCode2.Name = "txtNLCode2";
            this.txtNLCode2.Size = new System.Drawing.Size(100, 23);
            this.txtNLCode2.TabIndex = 38;
            this.txtNLCode2.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtNLCode2_PopUp);
            this.txtNLCode2.Validating += new System.ComponentModel.CancelEventHandler(this.TxtNLCode2_Validating);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(212, 186);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 23);
            this.label1.TabIndex = 37;
            this.label1.Text = "Customs Code(2021)";
            // 
            // txtNLCode
            // 
            this.txtNLCode.BackColor = System.Drawing.Color.White;
            this.txtNLCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "NLCode", true));
            this.txtNLCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtNLCode.Location = new System.Drawing.Point(109, 186);
            this.txtNLCode.Name = "txtNLCode";
            this.txtNLCode.Size = new System.Drawing.Size(100, 23);
            this.txtNLCode.TabIndex = 39;
            this.txtNLCode.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtNLCode_PopUp);
            this.txtNLCode.Validating += new System.ComponentModel.CancelEventHandler(this.TxtNLCode_Validating);
            // 
            // B41
            // 
            this.ClientSize = new System.Drawing.Size(745, 372);
            this.DefaultControl = "txtNLCode";
            this.DefaultControlForEdit = "txtNLCode";
            this.DefaultOrder = "RefNo";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B41";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B41. Customs Code - Local Purchase Item";
            this.UniqueExpress = "RefNo";
            this.WorkAlias = "LocalItem";
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

        private Win.UI.CheckBox checkNoNeedToDeclare;
        private Win.UI.Label label17;
        private Win.UI.Label label16;
        private Win.UI.NumericBox numKGPcs;
        private Win.UI.NumericBox numLengthPcs;
        private Win.UI.NumericBox numWidthPcs;
        private Win.UI.Label labelKGPcs;
        private Win.UI.Label labelLengthPcs;
        private Win.UI.Label labelWidthPcs;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.DisplayBox displayCustomsUnit;
        private Win.UI.DisplayBox displayHSCode;
        private Win.UI.DisplayBox displayMaterialType;
        private Win.UI.EditBox editDescription;
        private Win.UI.DisplayBox displayRefNo;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelCustomsUnit;
        private Win.UI.Label labelHSCode;
        private Win.UI.Label labelNLCode;
        private Win.UI.Label labelUnit;
        private Win.UI.Label labelMaterialType;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelRefNo;
        private Win.UI.DisplayBox displayUnit;
        private Sci.Production.Class.TxtsubconNoConfirm txtSubconSupplier;
        private Class.TxtNLCode txtNLCode;
        private Class.TxtNLCode txtNLCode2;
        private Win.UI.Label label1;
    }
}
