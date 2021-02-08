namespace Sci.Production.Shipping
{
    partial class B44
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
            this.labelEnglishDesc = new Sci.Win.UI.Label();
            this.labelVietnamDesc = new Sci.Win.UI.Label();
            this.labelVietnamUnit = new Sci.Win.UI.Label();
            this.txtNLCode = new Sci.Win.UI.TextBox();
            this.txtEnglishDesc = new Sci.Win.UI.TextBox();
            this.txtVietnamDesc = new Sci.Win.UI.TextBox();
            this.txtVietnamUnit = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.numericBox2 = new Sci.Win.UI.NumericBox();
            this.numericBox3 = new Sci.Win.UI.NumericBox();
            this.label3 = new System.Windows.Forms.Label();
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
            this.detail.Location = new System.Drawing.Point(4, 27);
            this.detail.Size = new System.Drawing.Size(829, 325);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.numericBox3);
            this.detailcont.Controls.Add(this.numericBox2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.txtVietnamUnit);
            this.detailcont.Controls.Add(this.txtVietnamDesc);
            this.detailcont.Controls.Add(this.txtEnglishDesc);
            this.detailcont.Controls.Add(this.txtNLCode);
            this.detailcont.Controls.Add(this.labelVietnamUnit);
            this.detailcont.Controls.Add(this.labelVietnamDesc);
            this.detailcont.Controls.Add(this.labelEnglishDesc);
            this.detailcont.Controls.Add(this.labelNLCode);
            this.detailcont.Size = new System.Drawing.Size(829, 287);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 287);
            this.detailbtm.Size = new System.Drawing.Size(829, 38);
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(4, 27);
            this.browse.Size = new System.Drawing.Size(829, 325);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(837, 356);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 24);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 24);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelNLCode
            // 
            this.labelNLCode.Location = new System.Drawing.Point(22, 14);
            this.labelNLCode.Name = "labelNLCode";
            this.labelNLCode.Size = new System.Drawing.Size(111, 23);
            this.labelNLCode.TabIndex = 2;
            this.labelNLCode.Text = "Customs Code";
            // 
            // labelEnglishDesc
            // 
            this.labelEnglishDesc.Location = new System.Drawing.Point(22, 50);
            this.labelEnglishDesc.Name = "labelEnglishDesc";
            this.labelEnglishDesc.Size = new System.Drawing.Size(111, 23);
            this.labelEnglishDesc.TabIndex = 3;
            this.labelEnglishDesc.Text = "English Desc";
            // 
            // labelVietnamDesc
            // 
            this.labelVietnamDesc.Location = new System.Drawing.Point(22, 87);
            this.labelVietnamDesc.Name = "labelVietnamDesc";
            this.labelVietnamDesc.Size = new System.Drawing.Size(111, 23);
            this.labelVietnamDesc.TabIndex = 4;
            this.labelVietnamDesc.Text = "Vietnam Desc";
            // 
            // labelVietnamUnit
            // 
            this.labelVietnamUnit.Location = new System.Drawing.Point(22, 125);
            this.labelVietnamUnit.Name = "labelVietnamUnit";
            this.labelVietnamUnit.Size = new System.Drawing.Size(111, 23);
            this.labelVietnamUnit.TabIndex = 5;
            this.labelVietnamUnit.Text = "Vietnam Unit";
            // 
            // txtNLCode
            // 
            this.txtNLCode.BackColor = System.Drawing.Color.White;
            this.txtNLCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "NLCode", true));
            this.txtNLCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtNLCode.Location = new System.Drawing.Point(136, 14);
            this.txtNLCode.Name = "txtNLCode";
            this.txtNLCode.Size = new System.Drawing.Size(100, 24);
            this.txtNLCode.TabIndex = 0;
            this.txtNLCode.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtNLCode_PopUp);
            this.txtNLCode.Validating += new System.ComponentModel.CancelEventHandler(this.TxtNLCode_Validating);
            // 
            // txtEnglishDesc
            // 
            this.txtEnglishDesc.BackColor = System.Drawing.Color.White;
            this.txtEnglishDesc.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtEnglishDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescEN", true));
            this.txtEnglishDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtEnglishDesc.Location = new System.Drawing.Point(136, 50);
            this.txtEnglishDesc.Name = "txtEnglishDesc";
            this.txtEnglishDesc.Size = new System.Drawing.Size(479, 24);
            this.txtEnglishDesc.TabIndex = 6;
            // 
            // txtVietnamDesc
            // 
            this.txtVietnamDesc.BackColor = System.Drawing.Color.White;
            this.txtVietnamDesc.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtVietnamDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescVI", true));
            this.txtVietnamDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtVietnamDesc.Location = new System.Drawing.Point(136, 87);
            this.txtVietnamDesc.Name = "txtVietnamDesc";
            this.txtVietnamDesc.Size = new System.Drawing.Size(479, 24);
            this.txtVietnamDesc.TabIndex = 7;
            // 
            // txtVietnamUnit
            // 
            this.txtVietnamUnit.BackColor = System.Drawing.Color.White;
            this.txtVietnamUnit.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtVietnamUnit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "UnitVI", true));
            this.txtVietnamUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtVietnamUnit.Location = new System.Drawing.Point(136, 125);
            this.txtVietnamUnit.Name = "txtVietnamUnit";
            this.txtVietnamUnit.Size = new System.Drawing.Size(68, 24);
            this.txtVietnamUnit.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(22, 163);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "Waste";
            // 
            // numericBox2
            // 
            this.numericBox2.BackColor = System.Drawing.Color.White;
            this.numericBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "WasteLower", true));
            this.numericBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox2.Location = new System.Drawing.Point(136, 163);
            this.numericBox2.Name = "numericBox2";
            this.numericBox2.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox2.Size = new System.Drawing.Size(68, 24);
            this.numericBox2.TabIndex = 10;
            this.numericBox2.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numericBox3
            // 
            this.numericBox3.BackColor = System.Drawing.Color.White;
            this.numericBox3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "WasteUpper", true));
            this.numericBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox3.Location = new System.Drawing.Point(232, 163);
            this.numericBox3.Name = "numericBox3";
            this.numericBox3.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox3.Size = new System.Drawing.Size(68, 24);
            this.numericBox3.TabIndex = 11;
            this.numericBox3.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(210, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 18);
            this.label3.TabIndex = 13;
            this.label3.Text = "~";
            // 
            // B44
            // 
            this.ClientSize = new System.Drawing.Size(837, 389);
            this.DefaultControl = "txtNLCode";
            this.DefaultControlForEdit = "txtNLCode";
            this.DefaultOrder = "NLCode";
            this.Name = "B44";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B44. Customs Code Description";
            this.UniqueExpress = "NLCode";
            this.WorkAlias = "VNNLCodeDesc";
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

        private Win.UI.TextBox txtVietnamUnit;
        private Win.UI.TextBox txtVietnamDesc;
        private Win.UI.TextBox txtEnglishDesc;
        private Win.UI.TextBox txtNLCode;
        private Win.UI.Label labelVietnamUnit;
        private Win.UI.Label labelVietnamDesc;
        private Win.UI.Label labelEnglishDesc;
        private Win.UI.Label labelNLCode;
        private System.Windows.Forms.Label label3;
        private Win.UI.NumericBox numericBox3;
        private Win.UI.NumericBox numericBox2;
        private Win.UI.Label label1;
    }
}
