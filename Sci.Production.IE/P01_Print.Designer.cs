namespace Sci.Production.IE
{
    partial class P01_Print
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
            this.labelEfficiencySetting = new Sci.Win.UI.Label();
            this.labelArtworkType = new Sci.Win.UI.Label();
            this.numEfficiencySetting = new Sci.Win.UI.NumericBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.chkCutting = new System.Windows.Forms.CheckBox();
            this.chkPressing = new System.Windows.Forms.CheckBox();
            this.chkPacking = new System.Windows.Forms.CheckBox();
            this.chkInspection = new System.Windows.Forms.CheckBox();
            this.comboLanguage = new Sci.Win.UI.ComboBox();
            this.labLanguage = new Sci.Win.UI.Label();
            this.textArtworkType = new Sci.Win.UI.TextBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(553, 12);
            this.print.TabIndex = 7;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(553, 48);
            this.toexcel.TabIndex = 8;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(553, 84);
            this.close.TabIndex = 9;
            // 
            // labelEfficiencySetting
            // 
            this.labelEfficiencySetting.Location = new System.Drawing.Point(13, 13);
            this.labelEfficiencySetting.Name = "labelEfficiencySetting";
            this.labelEfficiencySetting.Size = new System.Drawing.Size(118, 23);
            this.labelEfficiencySetting.TabIndex = 94;
            this.labelEfficiencySetting.Text = "Efficiency setting";
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.Location = new System.Drawing.Point(13, 48);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.Size = new System.Drawing.Size(118, 23);
            this.labelArtworkType.TabIndex = 95;
            this.labelArtworkType.Text = "Artwork type";
            // 
            // numEfficiencySetting
            // 
            this.numEfficiencySetting.BackColor = System.Drawing.Color.White;
            this.numEfficiencySetting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numEfficiencySetting.Location = new System.Drawing.Point(138, 13);
            this.numEfficiencySetting.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numEfficiencySetting.MaxLength = 3;
            this.numEfficiencySetting.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numEfficiencySetting.Name = "numEfficiencySetting";
            this.numEfficiencySetting.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numEfficiencySetting.Size = new System.Drawing.Size(50, 23);
            this.numEfficiencySetting.TabIndex = 0;
            this.numEfficiencySetting.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Lines = 2;
            this.label1.Location = new System.Drawing.Point(13, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 22);
            this.label1.TabIndex = 96;
            this.label1.Text = "Total time include";
            // 
            // label2
            // 
            this.label2.Lines = 2;
            this.label2.Location = new System.Drawing.Point(13, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 27);
            this.label2.TabIndex = 97;
            this.label2.Text = "(CIPF)";
            this.label2.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkCutting
            // 
            this.chkCutting.AutoSize = true;
            this.chkCutting.Location = new System.Drawing.Point(138, 85);
            this.chkCutting.Name = "chkCutting";
            this.chkCutting.Size = new System.Drawing.Size(71, 21);
            this.chkCutting.TabIndex = 2;
            this.chkCutting.Text = "Cutting";
            this.chkCutting.UseVisualStyleBackColor = true;
            // 
            // chkPressing
            // 
            this.chkPressing.AutoSize = true;
            this.chkPressing.Location = new System.Drawing.Point(138, 112);
            this.chkPressing.Name = "chkPressing";
            this.chkPressing.Size = new System.Drawing.Size(82, 21);
            this.chkPressing.TabIndex = 4;
            this.chkPressing.Text = "Pressing";
            this.chkPressing.UseVisualStyleBackColor = true;
            // 
            // chkPacking
            // 
            this.chkPacking.AutoSize = true;
            this.chkPacking.Location = new System.Drawing.Point(226, 112);
            this.chkPacking.Name = "chkPacking";
            this.chkPacking.Size = new System.Drawing.Size(77, 21);
            this.chkPacking.TabIndex = 5;
            this.chkPacking.Text = "Packing";
            this.chkPacking.UseVisualStyleBackColor = true;
            // 
            // chkInspection
            // 
            this.chkInspection.AutoSize = true;
            this.chkInspection.Location = new System.Drawing.Point(226, 85);
            this.chkInspection.Name = "chkInspection";
            this.chkInspection.Size = new System.Drawing.Size(91, 21);
            this.chkInspection.TabIndex = 3;
            this.chkInspection.Text = "Inspection";
            this.chkInspection.UseVisualStyleBackColor = true;
            // 
            // comboLanguage
            // 
            this.comboLanguage.BackColor = System.Drawing.Color.White;
            this.comboLanguage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboLanguage.FormattingEnabled = true;
            this.comboLanguage.IsSupportUnselect = true;
            this.comboLanguage.Location = new System.Drawing.Point(138, 144);
            this.comboLanguage.Name = "comboLanguage";
            this.comboLanguage.OldText = "";
            this.comboLanguage.Size = new System.Drawing.Size(121, 24);
            this.comboLanguage.TabIndex = 6;
            // 
            // labLanguage
            // 
            this.labLanguage.Location = new System.Drawing.Point(13, 145);
            this.labLanguage.Name = "labLanguage";
            this.labLanguage.Size = new System.Drawing.Size(118, 23);
            this.labLanguage.TabIndex = 100;
            this.labLanguage.Text = "Language";
            // 
            // textArtworkType
            // 
            this.textArtworkType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.textArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.textArtworkType.IsSupportEditMode = false;
            this.textArtworkType.Location = new System.Drawing.Point(138, 48);
            this.textArtworkType.Name = "textArtworkType";
            this.textArtworkType.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.textArtworkType.ReadOnly = true;
            this.textArtworkType.Size = new System.Drawing.Size(409, 23);
            this.textArtworkType.TabIndex = 101;
            this.textArtworkType.Text = "SEWING,PRESSING,PACKING,SEAMSEAL,ULTRASONIC";
            this.textArtworkType.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TextArtworkType_PopUp);
            // 
            // P01_Print
            // 
            this.ClientSize = new System.Drawing.Size(645, 202);
            this.Controls.Add(this.textArtworkType);
            this.Controls.Add(this.labLanguage);
            this.Controls.Add(this.comboLanguage);
            this.Controls.Add(this.chkInspection);
            this.Controls.Add(this.chkPacking);
            this.Controls.Add(this.chkPressing);
            this.Controls.Add(this.chkCutting);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numEfficiencySetting);
            this.Controls.Add(this.labelArtworkType);
            this.Controls.Add(this.labelEfficiencySetting);
            this.DefaultControl = "numEfficiencySetting";
            this.DefaultControlForEdit = "numEfficiencySetting";
            this.IsSupportToPrint = false;
            this.Name = "P01_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelEfficiencySetting, 0);
            this.Controls.SetChildIndex(this.labelArtworkType, 0);
            this.Controls.SetChildIndex(this.numEfficiencySetting, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.chkCutting, 0);
            this.Controls.SetChildIndex(this.chkPressing, 0);
            this.Controls.SetChildIndex(this.chkPacking, 0);
            this.Controls.SetChildIndex(this.chkInspection, 0);
            this.Controls.SetChildIndex(this.comboLanguage, 0);
            this.Controls.SetChildIndex(this.labLanguage, 0);
            this.Controls.SetChildIndex(this.textArtworkType, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelEfficiencySetting;
        private Win.UI.Label labelArtworkType;
        private Win.UI.NumericBox numEfficiencySetting;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private System.Windows.Forms.CheckBox chkCutting;
        private System.Windows.Forms.CheckBox chkPressing;
        private System.Windows.Forms.CheckBox chkPacking;
        private System.Windows.Forms.CheckBox chkInspection;
        private Win.UI.ComboBox comboLanguage;
        private Win.UI.Label labLanguage;
        private Win.UI.TextBox textArtworkType;
    }
}
