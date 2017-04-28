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
            this.detail.Size = new System.Drawing.Size(829, 327);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtVietnamUnit);
            this.detailcont.Controls.Add(this.txtVietnamDesc);
            this.detailcont.Controls.Add(this.txtEnglishDesc);
            this.detailcont.Controls.Add(this.txtNLCode);
            this.detailcont.Controls.Add(this.labelVietnamUnit);
            this.detailcont.Controls.Add(this.labelVietnamDesc);
            this.detailcont.Controls.Add(this.labelEnglishDesc);
            this.detailcont.Controls.Add(this.labelNLCode);
            this.detailcont.Size = new System.Drawing.Size(829, 289);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 289);
            this.detailbtm.Size = new System.Drawing.Size(829, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(829, 327);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(837, 356);
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
            this.labelNLCode.Lines = 0;
            this.labelNLCode.Location = new System.Drawing.Point(22, 14);
            this.labelNLCode.Name = "labelNLCode";
            this.labelNLCode.Size = new System.Drawing.Size(91, 23);
            this.labelNLCode.TabIndex = 2;
            this.labelNLCode.Text = "NL Code";
            // 
            // labelEnglishDesc
            // 
            this.labelEnglishDesc.Lines = 0;
            this.labelEnglishDesc.Location = new System.Drawing.Point(22, 50);
            this.labelEnglishDesc.Name = "labelEnglishDesc";
            this.labelEnglishDesc.Size = new System.Drawing.Size(91, 23);
            this.labelEnglishDesc.TabIndex = 3;
            this.labelEnglishDesc.Text = "English Desc";
            // 
            // labelVietnamDesc
            // 
            this.labelVietnamDesc.Lines = 0;
            this.labelVietnamDesc.Location = new System.Drawing.Point(22, 87);
            this.labelVietnamDesc.Name = "labelVietnamDesc";
            this.labelVietnamDesc.Size = new System.Drawing.Size(91, 23);
            this.labelVietnamDesc.TabIndex = 4;
            this.labelVietnamDesc.Text = "Vietnam Desc";
            // 
            // labelVietnamUnit
            // 
            this.labelVietnamUnit.Lines = 0;
            this.labelVietnamUnit.Location = new System.Drawing.Point(22, 125);
            this.labelVietnamUnit.Name = "labelVietnamUnit";
            this.labelVietnamUnit.Size = new System.Drawing.Size(91, 23);
            this.labelVietnamUnit.TabIndex = 5;
            this.labelVietnamUnit.Text = "Vietnam Unit";
            // 
            // txtNLCode
            // 
            this.txtNLCode.BackColor = System.Drawing.Color.White;
            this.txtNLCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "NLCode", true));
            this.txtNLCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtNLCode.Location = new System.Drawing.Point(117, 14);
            this.txtNLCode.Name = "txtNLCode";
            this.txtNLCode.Size = new System.Drawing.Size(68, 23);
            this.txtNLCode.TabIndex = 0;
            this.txtNLCode.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtNLCode_PopUp);
            this.txtNLCode.Validating += new System.ComponentModel.CancelEventHandler(this.txtNLCode_Validating);
            // 
            // txtEnglishDesc
            // 
            this.txtEnglishDesc.BackColor = System.Drawing.Color.White;
            this.txtEnglishDesc.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtEnglishDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescEN", true));
            this.txtEnglishDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtEnglishDesc.Location = new System.Drawing.Point(117, 50);
            this.txtEnglishDesc.Name = "txtEnglishDesc";
            this.txtEnglishDesc.Size = new System.Drawing.Size(479, 23);
            this.txtEnglishDesc.TabIndex = 6;
            // 
            // txtVietnamDesc
            // 
            this.txtVietnamDesc.BackColor = System.Drawing.Color.White;
            this.txtVietnamDesc.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtVietnamDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescVI", true));
            this.txtVietnamDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtVietnamDesc.Location = new System.Drawing.Point(117, 87);
            this.txtVietnamDesc.Name = "txtVietnamDesc";
            this.txtVietnamDesc.Size = new System.Drawing.Size(479, 23);
            this.txtVietnamDesc.TabIndex = 7;
            // 
            // txtVietnamUnit
            // 
            this.txtVietnamUnit.BackColor = System.Drawing.Color.White;
            this.txtVietnamUnit.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtVietnamUnit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "UnitVI", true));
            this.txtVietnamUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtVietnamUnit.Location = new System.Drawing.Point(117, 125);
            this.txtVietnamUnit.Name = "txtVietnamUnit";
            this.txtVietnamUnit.Size = new System.Drawing.Size(68, 23);
            this.txtVietnamUnit.TabIndex = 1;
            // 
            // B44
            // 
            this.ClientSize = new System.Drawing.Size(837, 389);
            this.DefaultControl = "textBox1";
            this.DefaultControlForEdit = "textBox1";
            this.DefaultOrder = "NLCode";
            this.Name = "B44";
            this.Text = "B44. NL Code Description";
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
    }
}
