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
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(317, 12);
            this.print.TabIndex = 0;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(317, 48);
            this.toexcel.TabIndex = 1;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(317, 84);
            this.close.TabIndex = 2;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(271, 120);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(297, 126);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(297, 124);
            // 
            // checkExtendAllParts
            // 
            this.checkExtendAllParts.AutoSize = true;
            this.checkExtendAllParts.ForeColor = System.Drawing.Color.Red;
            this.checkExtendAllParts.Location = new System.Drawing.Point(12, 84);
            this.checkExtendAllParts.Name = "checkExtendAllParts";
            this.checkExtendAllParts.Size = new System.Drawing.Size(126, 21);
            this.checkExtendAllParts.TabIndex = 5;
            this.checkExtendAllParts.Text = "Extend All Parts";
            this.checkExtendAllParts.UseVisualStyleBackColor = true;
            // 
            // radioBundleChecklist
            // 
            this.radioBundleChecklist.AutoSize = true;
            this.radioBundleChecklist.Location = new System.Drawing.Point(12, 48);
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
            this.radioBundleCard.Location = new System.Drawing.Point(12, 12);
            this.radioBundleCard.Name = "radioBundleCard";
            this.radioBundleCard.Size = new System.Drawing.Size(131, 21);
            this.radioBundleCard.TabIndex = 3;
            this.radioBundleCard.TabStop = true;
            this.radioBundleCard.Text = "Bundle Card(A4)";
            this.radioBundleCard.UseVisualStyleBackColor = true;
            this.radioBundleCard.CheckedChanged += new System.EventHandler(this.RadioBundleCard_CheckedChanged);
            // 
            // comboLayout
            // 
            this.comboLayout.BackColor = System.Drawing.Color.White;
            this.comboLayout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboLayout.FormattingEnabled = true;
            this.comboLayout.IsSupportUnselect = true;
            this.comboLayout.Location = new System.Drawing.Point(149, 12);
            this.comboLayout.Name = "comboLayout";
            this.comboLayout.OldText = "";
            this.comboLayout.Size = new System.Drawing.Size(121, 24);
            this.comboLayout.TabIndex = 97;
            // 
            // P10_Print
            // 
            this.ClientSize = new System.Drawing.Size(409, 178);
            this.Controls.Add(this.comboLayout);
            this.Controls.Add(this.checkExtendAllParts);
            this.Controls.Add(this.radioBundleChecklist);
            this.Controls.Add(this.radioBundleCard);
            this.DefaultControl = "radioBundleCard";
            this.Name = "P10_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "P10. Print";
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox checkExtendAllParts;
        private System.Windows.Forms.RadioButton radioBundleChecklist;
        private System.Windows.Forms.RadioButton radioBundleCard;
        private Win.UI.ComboBox comboLayout;
    }
}
