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
            this.checkExtendAllParts = new System.Windows.Forms.CheckBox();
            this.radioBundleChecklist = new System.Windows.Forms.RadioButton();
            this.radioBundleCard = new System.Windows.Forms.RadioButton();
            this.comboLayout = new Sci.Win.UI.ComboBox();
            this.radioBundleCardRF = new System.Windows.Forms.RadioButton();
            this.panel1 = new Sci.Win.UI.Panel();
            this.comboBoxSetting = new Sci.Win.UI.ComboBox();
            this.btnSetting = new Sci.Win.UI.Button();
            this.radioBundleErase = new System.Windows.Forms.RadioButton();
            this.linkLabelRFCardEraseBeforePrinting1 = new Sci.Production.Class.LinkLabelRFCardEraseBeforePrinting();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(337, 12);
            this.print.TabIndex = 0;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(337, 48);
            this.toexcel.TabIndex = 1;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(337, 84);
            this.close.TabIndex = 2;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(291, 120);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(317, 126);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(317, 124);
            // 
            // checkExtendAllParts
            // 
            this.checkExtendAllParts.AutoSize = true;
            this.checkExtendAllParts.ForeColor = System.Drawing.Color.Red;
            this.checkExtendAllParts.Location = new System.Drawing.Point(170, 18);
            this.checkExtendAllParts.Name = "checkExtendAllParts";
            this.checkExtendAllParts.Size = new System.Drawing.Size(126, 21);
            this.checkExtendAllParts.TabIndex = 5;
            this.checkExtendAllParts.Text = "Extend All Parts";
            this.checkExtendAllParts.UseVisualStyleBackColor = true;
            // 
            // radioBundleChecklist
            // 
            this.radioBundleChecklist.AutoSize = true;
            this.radioBundleChecklist.Location = new System.Drawing.Point(12, 120);
            this.radioBundleChecklist.Name = "radioBundleChecklist";
            this.radioBundleChecklist.Size = new System.Drawing.Size(134, 21);
            this.radioBundleChecklist.TabIndex = 4;
            this.radioBundleChecklist.Text = "Bundle Check list";
            this.radioBundleChecklist.UseVisualStyleBackColor = true;
            this.radioBundleChecklist.CheckedChanged += new System.EventHandler(this.RadioBundleChecklist_CheckedChanged);
            // 
            // radioBundleCard
            // 
            this.radioBundleCard.AutoSize = true;
            this.radioBundleCard.Checked = true;
            this.radioBundleCard.Location = new System.Drawing.Point(12, 12);
            this.radioBundleCard.Name = "radioBundleCard";
            this.radioBundleCard.Size = new System.Drawing.Size(82, 21);
            this.radioBundleCard.TabIndex = 3;
            this.radioBundleCard.TabStop = true;
            this.radioBundleCard.Text = "Print(A4)";
            this.radioBundleCard.UseVisualStyleBackColor = true;
            this.radioBundleCard.CheckedChanged += new System.EventHandler(this.RadioBundleCard_CheckedChanged);
            // 
            // comboLayout
            // 
            this.comboLayout.BackColor = System.Drawing.Color.White;
            this.comboLayout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboLayout.FormattingEnabled = true;
            this.comboLayout.IsSupportUnselect = true;
            this.comboLayout.Location = new System.Drawing.Point(170, 45);
            this.comboLayout.Name = "comboLayout";
            this.comboLayout.OldText = "";
            this.comboLayout.Size = new System.Drawing.Size(121, 24);
            this.comboLayout.TabIndex = 97;
            // 
            // radioBundleCardRF
            // 
            this.radioBundleCardRF.AutoSize = true;
            this.radioBundleCardRF.Location = new System.Drawing.Point(11, 48);
            this.radioBundleCardRF.Name = "radioBundleCardRF";
            this.radioBundleCardRF.Size = new System.Drawing.Size(83, 21);
            this.radioBundleCardRF.TabIndex = 100;
            this.radioBundleCardRF.Text = "Print(RF)";
            this.radioBundleCardRF.UseVisualStyleBackColor = true;
            this.radioBundleCardRF.CheckedChanged += new System.EventHandler(this.RadioBundleCardRF_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(165, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(131, 66);
            this.panel1.TabIndex = 101;
            // 
            // comboBoxSetting
            // 
            this.comboBoxSetting.BackColor = System.Drawing.Color.White;
            this.comboBoxSetting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxSetting.FormattingEnabled = true;
            this.comboBoxSetting.IsSupportUnselect = true;
            this.comboBoxSetting.Location = new System.Drawing.Point(165, 119);
            this.comboBoxSetting.Name = "comboBoxSetting";
            this.comboBoxSetting.OldText = "";
            this.comboBoxSetting.Size = new System.Drawing.Size(121, 24);
            this.comboBoxSetting.TabIndex = 102;
            this.comboBoxSetting.Visible = false;
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(292, 116);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(80, 30);
            this.btnSetting.TabIndex = 103;
            this.btnSetting.Text = "Setting";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Visible = false;
            this.btnSetting.Click += new System.EventHandler(this.BtnSetting_Click);
            // 
            // radioBundleErase
            // 
            this.radioBundleErase.AutoSize = true;
            this.radioBundleErase.Location = new System.Drawing.Point(12, 84);
            this.radioBundleErase.Name = "radioBundleErase";
            this.radioBundleErase.Size = new System.Drawing.Size(91, 21);
            this.radioBundleErase.TabIndex = 104;
            this.radioBundleErase.Text = "Erase(RF)";
            this.radioBundleErase.UseVisualStyleBackColor = true;
            this.radioBundleErase.CheckedChanged += new System.EventHandler(this.RadioBundleErase_CheckedChanged);
            // 
            // linkLabelRFCardEraseBeforePrinting1
            // 
            this.linkLabelRFCardEraseBeforePrinting1.AutoSize = true;
            this.linkLabelRFCardEraseBeforePrinting1.Location = new System.Drawing.Point(12, 170);
            this.linkLabelRFCardEraseBeforePrinting1.Name = "linkLabelRFCardEraseBeforePrinting1";
            this.linkLabelRFCardEraseBeforePrinting1.Size = new System.Drawing.Size(247, 17);
            this.linkLabelRFCardEraseBeforePrinting1.TabIndex = 105;
            this.linkLabelRFCardEraseBeforePrinting1.TabStop = true;
            this.linkLabelRFCardEraseBeforePrinting1.Text = "linkLabelRFCardEraseBeforePrinting1";
            // 
            // P10_Print
            // 
            this.ClientSize = new System.Drawing.Size(429, 221);
            this.Controls.Add(this.linkLabelRFCardEraseBeforePrinting1);
            this.Controls.Add(this.radioBundleErase);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.comboBoxSetting);
            this.Controls.Add(this.radioBundleCardRF);
            this.Controls.Add(this.comboLayout);
            this.Controls.Add(this.checkExtendAllParts);
            this.Controls.Add(this.radioBundleChecklist);
            this.Controls.Add(this.radioBundleCard);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "radioBundleCard";
            this.Name = "P10_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "P10. Print";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.P10_Print_FormClosed);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.radioBundleCard, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.radioBundleChecklist, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.checkExtendAllParts, 0);
            this.Controls.SetChildIndex(this.comboLayout, 0);
            this.Controls.SetChildIndex(this.radioBundleCardRF, 0);
            this.Controls.SetChildIndex(this.comboBoxSetting, 0);
            this.Controls.SetChildIndex(this.btnSetting, 0);
            this.Controls.SetChildIndex(this.radioBundleErase, 0);
            this.Controls.SetChildIndex(this.linkLabelRFCardEraseBeforePrinting1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox checkExtendAllParts;
        private System.Windows.Forms.RadioButton radioBundleChecklist;
        private System.Windows.Forms.RadioButton radioBundleCard;
        private Win.UI.ComboBox comboLayout;
        private System.Windows.Forms.RadioButton radioBundleCardRF;
        private Win.UI.Panel panel1;
        private Win.UI.ComboBox comboBoxSetting;
        private Win.UI.Button btnSetting;
        private System.Windows.Forms.RadioButton radioBundleErase;
        private Class.LinkLabelRFCardEraseBeforePrinting linkLabelRFCardEraseBeforePrinting1;
    }
}
