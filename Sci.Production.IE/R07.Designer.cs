namespace Sci.Production.IE
{
    partial class R07
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
            this.dateInlineDate = new Sci.Win.UI.DateRange();
            this.lbVersion = new Sci.Win.UI.Label();
            this.lbInlineDate = new Sci.Win.UI.Label();
            this.lbOperationCode = new Sci.Win.UI.Label();
            this.chkLatestVersion = new Sci.Win.UI.CheckBox();
            this.txtmulitOperation1 = new Sci.Production.Class.TxtmulitOperation();
            this.txtmulitMachineType1 = new Sci.Production.Class.TxtmulitMachineType();
            this.lbSTMCType = new Sci.Win.UI.Label();
            this.lbAnalysisType = new Sci.Win.UI.Label();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioLineMapping = new Sci.Win.UI.RadioButton();
            this.radioFtyGSD = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(419, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(419, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(419, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(373, 148);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(399, 121);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(399, 121);
            // 
            // dateInlineDate
            // 
            // 
            // 
            // 
            this.dateInlineDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInlineDate.DateBox1.Name = "";
            this.dateInlineDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateInlineDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInlineDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateInlineDate.DateBox2.Name = "";
            this.dateInlineDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateInlineDate.DateBox2.TabIndex = 1;
            this.dateInlineDate.IsRequired = false;
            this.dateInlineDate.Location = new System.Drawing.Point(116, 12);
            this.dateInlineDate.Name = "dateInlineDate";
            this.dateInlineDate.Size = new System.Drawing.Size(280, 23);
            this.dateInlineDate.TabIndex = 1;
            // 
            // lbVersion
            // 
            this.lbVersion.Location = new System.Drawing.Point(9, 155);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(104, 23);
            this.lbVersion.TabIndex = 104;
            this.lbVersion.Text = "Version";
            // 
            // lbInlineDate
            // 
            this.lbInlineDate.Location = new System.Drawing.Point(9, 12);
            this.lbInlineDate.Name = "lbInlineDate";
            this.lbInlineDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lbInlineDate.RectStyle.BorderWidth = 1F;
            this.lbInlineDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbInlineDate.RectStyle.ExtBorderWidth = 1F;
            this.lbInlineDate.Size = new System.Drawing.Size(104, 23);
            this.lbInlineDate.TabIndex = 105;
            this.lbInlineDate.Text = "Add/Edit Date";
            this.lbInlineDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lbInlineDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lbOperationCode
            // 
            this.lbOperationCode.Location = new System.Drawing.Point(9, 48);
            this.lbOperationCode.Name = "lbOperationCode";
            this.lbOperationCode.Size = new System.Drawing.Size(104, 23);
            this.lbOperationCode.TabIndex = 107;
            this.lbOperationCode.Text = "Code Type";
            // 
            // chkLatestVersion
            // 
            this.chkLatestVersion.AutoSize = true;
            this.chkLatestVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkLatestVersion.Location = new System.Drawing.Point(116, 157);
            this.chkLatestVersion.Name = "chkLatestVersion";
            this.chkLatestVersion.Size = new System.Drawing.Size(118, 21);
            this.chkLatestVersion.TabIndex = 165;
            this.chkLatestVersion.Text = "Latest Version";
            this.chkLatestVersion.UseVisualStyleBackColor = true;
            // 
            // txtmulitOperation1
            // 
            this.txtmulitOperation1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtmulitOperation1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtmulitOperation1.IsJunk = false;
            this.txtmulitOperation1.IsSupportEditMode = false;
            this.txtmulitOperation1.Location = new System.Drawing.Point(116, 48);
            this.txtmulitOperation1.Name = "txtmulitOperation1";
            this.txtmulitOperation1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtmulitOperation1.ReadOnly = true;
            this.txtmulitOperation1.Size = new System.Drawing.Size(280, 23);
            this.txtmulitOperation1.TabIndex = 110;
            // 
            // txtmulitMachineType1
            // 
            this.txtmulitMachineType1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtmulitMachineType1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtmulitMachineType1.IsJunk = false;
            this.txtmulitMachineType1.IsSupportEditMode = false;
            this.txtmulitMachineType1.Location = new System.Drawing.Point(116, 84);
            this.txtmulitMachineType1.Name = "txtmulitMachineType1";
            this.txtmulitMachineType1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtmulitMachineType1.ReadOnly = true;
            this.txtmulitMachineType1.Size = new System.Drawing.Size(280, 23);
            this.txtmulitMachineType1.TabIndex = 166;
            // 
            // lbSTMCType
            // 
            this.lbSTMCType.Location = new System.Drawing.Point(9, 84);
            this.lbSTMCType.Name = "lbSTMCType";
            this.lbSTMCType.Size = new System.Drawing.Size(104, 23);
            this.lbSTMCType.TabIndex = 167;
            this.lbSTMCType.Text = "ST/MC Type";
            // 
            // lbAnalysisType
            // 
            this.lbAnalysisType.Location = new System.Drawing.Point(9, 119);
            this.lbAnalysisType.Name = "lbAnalysisType";
            this.lbAnalysisType.Size = new System.Drawing.Size(104, 23);
            this.lbAnalysisType.TabIndex = 168;
            this.lbAnalysisType.Text = "Analysis Type";
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioLineMapping);
            this.radioPanel1.Controls.Add(this.radioFtyGSD);
            this.radioPanel1.Location = new System.Drawing.Point(116, 121);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(239, 25);
            this.radioPanel1.TabIndex = 229;
            // 
            // radioLineMapping
            // 
            this.radioLineMapping.AutoSize = true;
            this.radioLineMapping.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioLineMapping.Location = new System.Drawing.Point(94, 3);
            this.radioLineMapping.Name = "radioLineMapping";
            this.radioLineMapping.Size = new System.Drawing.Size(111, 21);
            this.radioLineMapping.TabIndex = 7;
            this.radioLineMapping.Text = "Line Mapping";
            this.radioLineMapping.UseVisualStyleBackColor = true;
            // 
            // radioFtyGSD
            // 
            this.radioFtyGSD.AutoSize = true;
            this.radioFtyGSD.Checked = true;
            this.radioFtyGSD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioFtyGSD.Location = new System.Drawing.Point(3, 3);
            this.radioFtyGSD.Name = "radioFtyGSD";
            this.radioFtyGSD.Size = new System.Drawing.Size(79, 21);
            this.radioFtyGSD.TabIndex = 6;
            this.radioFtyGSD.TabStop = true;
            this.radioFtyGSD.Text = "Fty GSD";
            this.radioFtyGSD.UseVisualStyleBackColor = true;
            // 
            // R07
            // 
            this.ClientSize = new System.Drawing.Size(511, 217);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.lbAnalysisType);
            this.Controls.Add(this.lbSTMCType);
            this.Controls.Add(this.txtmulitMachineType1);
            this.Controls.Add(this.chkLatestVersion);
            this.Controls.Add(this.txtmulitOperation1);
            this.Controls.Add(this.lbOperationCode);
            this.Controls.Add(this.lbInlineDate);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.dateInlineDate);
            this.DefaultControl = "txtFactory";
            this.DefaultControlForEdit = "txtFactory";
            this.IsSupportToPrint = false;
            this.Name = "R07";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R07. Operation Usage Analysis";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.dateInlineDate, 0);
            this.Controls.SetChildIndex(this.lbVersion, 0);
            this.Controls.SetChildIndex(this.lbInlineDate, 0);
            this.Controls.SetChildIndex(this.lbOperationCode, 0);
            this.Controls.SetChildIndex(this.txtmulitOperation1, 0);
            this.Controls.SetChildIndex(this.chkLatestVersion, 0);
            this.Controls.SetChildIndex(this.txtmulitMachineType1, 0);
            this.Controls.SetChildIndex(this.lbSTMCType, 0);
            this.Controls.SetChildIndex(this.lbAnalysisType, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.DateRange dateInlineDate;
        private Win.UI.Label lbVersion;
        private Win.UI.Label lbInlineDate;
        private Win.UI.Label lbOperationCode;
        private Class.TxtmulitOperation txtmulitOperation1;
        private Win.UI.CheckBox chkLatestVersion;
        private Class.TxtmulitMachineType txtmulitMachineType1;
        private Win.UI.Label lbSTMCType;
        private Win.UI.Label lbAnalysisType;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioLineMapping;
        private Win.UI.RadioButton radioFtyGSD;
    }
}
