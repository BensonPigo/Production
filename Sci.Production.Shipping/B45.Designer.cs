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
            this.detail.Size = new System.Drawing.Size(828, 347);
            // 
            // detailcont
            // 
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
            this.detailcont.Size = new System.Drawing.Size(828, 309);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 309);
            this.detailbtm.Size = new System.Drawing.Size(828, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(828, 347);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(836, 376);
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
            this.labelNLCode.Location = new System.Drawing.Point(43, 18);
            this.labelNLCode.Name = "labelNLCode";
            this.labelNLCode.Size = new System.Drawing.Size(99, 23);
            this.labelNLCode.TabIndex = 2;
            this.labelNLCode.Text = "Customs Code";
            // 
            // labelHSCode
            // 
            this.labelHSCode.Location = new System.Drawing.Point(43, 56);
            this.labelHSCode.Name = "labelHSCode";
            this.labelHSCode.Size = new System.Drawing.Size(99, 23);
            this.labelHSCode.TabIndex = 3;
            this.labelHSCode.Text = "HS Code";
            // 
            // labelUnit
            // 
            this.labelUnit.Location = new System.Drawing.Point(43, 95);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(99, 23);
            this.labelUnit.TabIndex = 4;
            this.labelUnit.Text = "Unit";
            // 
            // labelQty
            // 
            this.labelQty.Location = new System.Drawing.Point(43, 132);
            this.labelQty.Name = "labelQty";
            this.labelQty.Size = new System.Drawing.Size(99, 23);
            this.labelQty.TabIndex = 5;
            this.labelQty.Text = "Q\'ty";
            // 
            // txtNLCode
            // 
            this.txtNLCode.BackColor = System.Drawing.Color.White;
            this.txtNLCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "NLCode", true));
            this.txtNLCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtNLCode.Location = new System.Drawing.Point(145, 18);
            this.txtNLCode.Name = "txtNLCode";
            this.txtNLCode.Size = new System.Drawing.Size(60, 23);
            this.txtNLCode.TabIndex = 0;
            this.txtNLCode.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtNLCode_PopUp);
            this.txtNLCode.Validating += new System.ComponentModel.CancelEventHandler(this.TxtNLCode_Validating);
            // 
            // displayHSCode
            // 
            this.displayHSCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayHSCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "HSCode", true));
            this.displayHSCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayHSCode.Location = new System.Drawing.Point(145, 56);
            this.displayHSCode.Name = "displayHSCode";
            this.displayHSCode.Size = new System.Drawing.Size(110, 23);
            this.displayHSCode.TabIndex = 5;
            // 
            // displayUnit
            // 
            this.displayUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayUnit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "UnitID", true));
            this.displayUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayUnit.Location = new System.Drawing.Point(145, 95);
            this.displayUnit.Name = "displayUnit";
            this.displayUnit.Size = new System.Drawing.Size(80, 23);
            this.displayUnit.TabIndex = 6;
            // 
            // numQty
            // 
            this.numQty.BackColor = System.Drawing.Color.White;
            this.numQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Qty", true));
            this.numQty.DecimalPlaces = 3;
            this.numQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numQty.Location = new System.Drawing.Point(145, 132);
            this.numQty.Name = "numQty";
            this.numQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numQty.Size = new System.Drawing.Size(80, 23);
            this.numQty.TabIndex = 1;
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
            this.radioPanel1.Location = new System.Drawing.Point(230, 129);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(217, 57);
            this.radioPanel1.TabIndex = 8;
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
            this.checkTissuepaper.TabIndex = 9;
            this.checkTissuepaper.Text = "Tissue paper";
            this.checkTissuepaper.UseVisualStyleBackColor = true;
            // 
            // B45
            // 
            this.ClientSize = new System.Drawing.Size(836, 409);
            this.DefaultControl = "txtNLCode";
            this.DefaultControlForEdit = "txtNLCode";
            this.DefaultOrder = "NLCode";
            this.Name = "B45";
            this.Text = "B45. Fixed Export Declare Data";
            this.UniqueExpress = "NLCode";
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
    }
}
