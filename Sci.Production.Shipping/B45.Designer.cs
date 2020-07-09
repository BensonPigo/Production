namespace Sci.Production.Shipping
{
    partial class B45
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
            this.labelNLCode = new Sci.Win.UI.Label();
            this.labelHSCode = new Sci.Win.UI.Label();
            this.labelUnit = new Sci.Win.UI.Label();
            this.labelQty = new Sci.Win.UI.Label();
            this.txtNLCode = new Sci.Win.UI.TextBox();
            this.displayHSCode = new Sci.Win.UI.DisplayBox();
            this.displayUnit = new Sci.Win.UI.DisplayBox();
            this.numQty = new Sci.Win.UI.NumericBox();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioCalculate = new Sci.Win.UI.RadioButton();
            this.radioFixedQty = new Sci.Win.UI.RadioButton();
            this.checkTissuepaper = new Sci.Win.UI.CheckBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.displayFabricType = new Sci.Win.UI.DisplayBox();
            this.label2 = new Sci.Win.UI.Label();
            this.displayStockUnit = new Sci.Win.UI.DisplayBox();
            this.label3 = new Sci.Win.UI.Label();
            this.labContractNo = new Sci.Win.UI.Label();
            this.txtContractNo = new Sci.Production.Class.TxtCustomsContract();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(828, 321);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtContractNo);
            this.detailcont.Controls.Add(this.labContractNo);
            this.detailcont.Controls.Add(this.displayStockUnit);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.displayFabricType);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.txtRefno);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.checkTissuepaper);
            this.detailcont.Controls.Add(this.radioPanel1);
            this.detailcont.Controls.Add(this.numQty);
            this.detailcont.Controls.Add(this.displayUnit);
            this.detailcont.Controls.Add(this.displayHSCode);
            this.detailcont.Controls.Add(this.txtNLCode);
            this.detailcont.Controls.Add(this.labelQty);
            this.detailcont.Controls.Add(this.labelUnit);
            this.detailcont.Controls.Add(this.labelHSCode);
            this.detailcont.Controls.Add(this.labelNLCode);
            this.detailcont.Size = new System.Drawing.Size(828, 283);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 283);
            this.detailbtm.Size = new System.Drawing.Size(828, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(828, 321);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(836, 350);
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
            // labelNLCode
            // 
            this.labelNLCode.Location = new System.Drawing.Point(46, 45);
            this.labelNLCode.Name = "labelNLCode";
            this.labelNLCode.Size = new System.Drawing.Size(99, 23);
            this.labelNLCode.TabIndex = 11;
            this.labelNLCode.Text = "Customs Code";
            // 
            // labelHSCode
            // 
            this.labelHSCode.Location = new System.Drawing.Point(46, 99);
            this.labelHSCode.Name = "labelHSCode";
            this.labelHSCode.Size = new System.Drawing.Size(99, 23);
            this.labelHSCode.TabIndex = 13;
            this.labelHSCode.Text = "HS Code";
            // 
            // labelUnit
            // 
            this.labelUnit.Location = new System.Drawing.Point(46, 126);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(99, 23);
            this.labelUnit.TabIndex = 14;
            this.labelUnit.Text = "Unit";
            // 
            // labelQty
            // 
            this.labelQty.Location = new System.Drawing.Point(46, 207);
            this.labelQty.Name = "labelQty";
            this.labelQty.Size = new System.Drawing.Size(99, 23);
            this.labelQty.TabIndex = 17;
            this.labelQty.Text = "Q\'ty";
            // 
            // txtNLCode
            // 
            this.txtNLCode.BackColor = System.Drawing.Color.White;
            this.txtNLCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "NLCode", true));
            this.txtNLCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtNLCode.Location = new System.Drawing.Point(148, 45);
            this.txtNLCode.Name = "txtNLCode";
            this.txtNLCode.Size = new System.Drawing.Size(100, 23);
            this.txtNLCode.TabIndex = 1;
            this.txtNLCode.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtNLCode_PopUp);
            this.txtNLCode.Validating += new System.ComponentModel.CancelEventHandler(this.TxtNLCode_Validating);
            // 
            // displayHSCode
            // 
            this.displayHSCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayHSCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "HSCode", true));
            this.displayHSCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayHSCode.Location = new System.Drawing.Point(148, 99);
            this.displayHSCode.Name = "displayHSCode";
            this.displayHSCode.Size = new System.Drawing.Size(110, 23);
            this.displayHSCode.TabIndex = 6;
            // 
            // displayUnit
            // 
            this.displayUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayUnit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "UnitID", true));
            this.displayUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayUnit.Location = new System.Drawing.Point(148, 126);
            this.displayUnit.Name = "displayUnit";
            this.displayUnit.Size = new System.Drawing.Size(80, 23);
            this.displayUnit.TabIndex = 7;
            // 
            // numQty
            // 
            this.numQty.BackColor = System.Drawing.Color.White;
            this.numQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Qty", true));
            this.numQty.DecimalPlaces = 3;
            this.numQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numQty.Location = new System.Drawing.Point(148, 207);
            this.numQty.Name = "numQty";
            this.numQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numQty.Size = new System.Drawing.Size(80, 23);
            this.numQty.TabIndex = 3;
            this.numQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioCalculate);
            this.radioPanel1.Controls.Add(this.radioFixedQty);
            this.radioPanel1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Type", true));
            this.radioPanel1.Location = new System.Drawing.Point(233, 204);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(217, 57);
            this.radioPanel1.TabIndex = 4;
            // 
            // radioCalculate
            // 
            this.radioCalculate.AutoSize = true;
            this.radioCalculate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioCalculate.Location = new System.Drawing.Point(3, 31);
            this.radioCalculate.Name = "radioCalculate";
            this.radioCalculate.Size = new System.Drawing.Size(198, 21);
            this.radioCalculate.TabIndex = 1;
            this.radioCalculate.TabStop = true;
            this.radioCalculate.Text = "Calculate with “Qty per Ctn”";
            this.radioCalculate.UseVisualStyleBackColor = true;
            this.radioCalculate.Value = "2";
            // 
            // radioFixedQty
            // 
            this.radioFixedQty.AutoSize = true;
            this.radioFixedQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioFixedQty.Location = new System.Drawing.Point(3, 3);
            this.radioFixedQty.Name = "radioFixedQty";
            this.radioFixedQty.Size = new System.Drawing.Size(88, 21);
            this.radioFixedQty.TabIndex = 0;
            this.radioFixedQty.TabStop = true;
            this.radioFixedQty.Text = "Fixed Q\'ty";
            this.radioFixedQty.UseVisualStyleBackColor = true;
            this.radioFixedQty.Value = "1";
            // 
            // checkTissuepaper
            // 
            this.checkTissuepaper.AutoSize = true;
            this.checkTissuepaper.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "TissuePaper", true));
            this.checkTissuepaper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkTissuepaper.Location = new System.Drawing.Point(444, 18);
            this.checkTissuepaper.Name = "checkTissuepaper";
            this.checkTissuepaper.Size = new System.Drawing.Size(110, 21);
            this.checkTissuepaper.TabIndex = 5;
            this.checkTissuepaper.Text = "Tissue paper";
            this.checkTissuepaper.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(46, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "Ref No.";
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Refno", true));
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(148, 72);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(143, 23);
            this.txtRefno.TabIndex = 2;
            this.txtRefno.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtRefno_PopUp);
            this.txtRefno.Validating += new System.ComponentModel.CancelEventHandler(this.TxtRefno_Validating);
            // 
            // displayFabricType
            // 
            this.displayFabricType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFabricType.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FabricType", true));
            this.displayFabricType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFabricType.Location = new System.Drawing.Point(148, 180);
            this.displayFabricType.Name = "displayFabricType";
            this.displayFabricType.Size = new System.Drawing.Size(80, 23);
            this.displayFabricType.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(46, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 23);
            this.label2.TabIndex = 16;
            this.label2.Text = "Fabric Type";
            // 
            // displayStockUnit
            // 
            this.displayStockUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStockUnit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "StockUnit", true));
            this.displayStockUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStockUnit.Location = new System.Drawing.Point(148, 153);
            this.displayStockUnit.Name = "displayStockUnit";
            this.displayStockUnit.Size = new System.Drawing.Size(80, 23);
            this.displayStockUnit.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(46, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 23);
            this.label3.TabIndex = 15;
            this.label3.Text = "Stock Unit";
            // 
            // labContractNo
            // 
            this.labContractNo.Location = new System.Drawing.Point(46, 18);
            this.labContractNo.Name = "labContractNo";
            this.labContractNo.Size = new System.Drawing.Size(99, 23);
            this.labContractNo.TabIndex = 10;
            this.labContractNo.Text = "Contract no.";
            // 
            // txtContractNo
            // 
            this.txtContractNo.BackColor = System.Drawing.Color.White;
            this.txtContractNo.CheckDate = false;
            this.txtContractNo.CheckStatus = true;
            this.txtContractNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "VNContractID", true));
            this.txtContractNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtContractNo.Location = new System.Drawing.Point(148, 18);
            this.txtContractNo.Name = "txtContractNo";
            this.txtContractNo.Size = new System.Drawing.Size(143, 23);
            this.txtContractNo.TabIndex = 0;
            // 
            // B45
            // 
            this.ClientSize = new System.Drawing.Size(836, 383);
            this.DefaultControl = "txtContractNo";
            this.DefaultControlForEdit = "txtContractNo";
            this.DefaultOrder = "NLCode";
            this.Name = "B45";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B45. Fixed Export Declare Data";
            this.UniqueExpress = "NLCode,Refno";
            this.WorkAlias = "VNFixedDeclareItem";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.CheckBox checkTissuepaper;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioCalculate;
        private Win.UI.RadioButton radioFixedQty;
        private Win.UI.NumericBox numQty;
        private Win.UI.DisplayBox displayUnit;
        private Win.UI.DisplayBox displayHSCode;
        private Win.UI.TextBox txtNLCode;
        private Win.UI.Label labelQty;
        private Win.UI.Label labelUnit;
        private Win.UI.Label labelHSCode;
        private Win.UI.Label labelNLCode;
        private Win.UI.TextBox txtRefno;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox displayStockUnit;
        private Win.UI.Label label3;
        private Win.UI.DisplayBox displayFabricType;
        private Win.UI.Label label2;
        private Class.TxtCustomsContract txtContractNo;
        private Win.UI.Label labContractNo;
    }
}
