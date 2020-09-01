namespace Sci.Production.Cutting
{
    partial class P10_Print
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
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.btnSetting = new Sci.Win.UI.Button();
            this.comboBoxSetting = new Sci.Win.UI.ComboBox();
            this.radioBundleCardRF = new System.Windows.Forms.RadioButton();
            this.checkExtendAllParts = new System.Windows.Forms.CheckBox();
            this.radioBundleChecklist = new System.Windows.Forms.RadioButton();
            this.radioBundleCard = new System.Windows.Forms.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(435, 12);
            this.print.TabIndex = 0;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(435, 48);
            this.toexcel.TabIndex = 1;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(435, 84);
            this.close.TabIndex = 2;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.comboBoxSetting);
            this.radioPanel1.Controls.Add(this.radioBundleCardRF);
            this.radioPanel1.Controls.Add(this.checkExtendAllParts);
            this.radioPanel1.Controls.Add(this.radioBundleChecklist);
            this.radioPanel1.Controls.Add(this.radioBundleCard);
            this.radioPanel1.Location = new System.Drawing.Point(12, 11);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(350, 113);
            this.radioPanel1.TabIndex = 0;
            this.radioPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.RadioPanel1_Paint);
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(349, 60);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(80, 30);
            this.btnSetting.TabIndex = 8;
            this.btnSetting.Text = "Setting";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Visible = false;
            this.btnSetting.Click += new System.EventHandler(this.BtnSetting_Click);
            // 
            // comboBoxSetting
            // 
            this.comboBoxSetting.BackColor = System.Drawing.Color.White;
            this.comboBoxSetting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxSetting.FormattingEnabled = true;
            this.comboBoxSetting.IsSupportUnselect = true;
            this.comboBoxSetting.Location = new System.Drawing.Point(210, 53);
            this.comboBoxSetting.Name = "comboBoxSetting";
            this.comboBoxSetting.OldText = "";
            this.comboBoxSetting.Size = new System.Drawing.Size(121, 24);
            this.comboBoxSetting.TabIndex = 7;
            this.comboBoxSetting.Visible = false;
            // 
            // radioBundleCardRF
            // 
            this.radioBundleCardRF.AutoSize = true;
            this.radioBundleCardRF.Location = new System.Drawing.Point(28, 53);
            this.radioBundleCardRF.Name = "radioBundleCardRF";
            this.radioBundleCardRF.Size = new System.Drawing.Size(132, 21);
            this.radioBundleCardRF.TabIndex = 6;
            this.radioBundleCardRF.Text = "Bundle Card(RF)";
            this.radioBundleCardRF.UseVisualStyleBackColor = true;
            // 
            // checkExtendAllParts
            // 
            this.checkExtendAllParts.AutoSize = true;
            this.checkExtendAllParts.ForeColor = System.Drawing.Color.Red;
            this.checkExtendAllParts.Location = new System.Drawing.Point(210, 18);
            this.checkExtendAllParts.Name = "checkExtendAllParts";
            this.checkExtendAllParts.Size = new System.Drawing.Size(126, 21);
            this.checkExtendAllParts.TabIndex = 5;
            this.checkExtendAllParts.Text = "Extend All Parts";
            this.checkExtendAllParts.UseVisualStyleBackColor = true;
            // 
            // radioBundleChecklist
            // 
            this.radioBundleChecklist.AutoSize = true;
            this.radioBundleChecklist.Location = new System.Drawing.Point(28, 89);
            this.radioBundleChecklist.Name = "radioBundleChecklist";
            this.radioBundleChecklist.Size = new System.Drawing.Size(134, 21);
            this.radioBundleChecklist.TabIndex = 4;
            this.radioBundleChecklist.Text = "Bundle Check list";
            this.radioBundleChecklist.UseVisualStyleBackColor = true;
            // 
            // radioBundleCard
            // 
            this.radioBundleCard.AutoSize = true;
            this.radioBundleCard.Checked = true;
            this.radioBundleCard.Location = new System.Drawing.Point(28, 17);
            this.radioBundleCard.Name = "radioBundleCard";
            this.radioBundleCard.Size = new System.Drawing.Size(131, 21);
            this.radioBundleCard.TabIndex = 3;
            this.radioBundleCard.TabStop = true;
            this.radioBundleCard.Text = "Bundle Card(A4)";
            this.radioBundleCard.UseVisualStyleBackColor = true;
            // 
            // P10_Print
            // 
            this.ClientSize = new System.Drawing.Size(527, 153);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.radioPanel1);
            this.DefaultControl = "radioBundleCard";
            this.Name = "P10_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "P10. Print";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.btnSetting, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private System.Windows.Forms.CheckBox checkExtendAllParts;
        private System.Windows.Forms.RadioButton radioBundleChecklist;
        private System.Windows.Forms.RadioButton radioBundleCard;
        private System.Windows.Forms.RadioButton radioBundleCardRF;
        private Win.UI.Button btnSetting;
        private Win.UI.ComboBox comboBoxSetting;
    }
}
