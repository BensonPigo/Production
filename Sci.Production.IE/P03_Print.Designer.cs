namespace Sci.Production.IE
{
    partial class P03_Print
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
            this.labelLineMappingDisplay = new Sci.Win.UI.Label();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioU_Right = new Sci.Win.UI.RadioButton();
            this.radioZ = new Sci.Win.UI.RadioButton();
            this.radioU_Left = new Sci.Win.UI.RadioButton();
            this.labelOperationContentType = new Sci.Win.UI.Label();
            this.radioPanel2 = new Sci.Win.UI.RadioPanel();
            this.radioAnnotation = new Sci.Win.UI.RadioButton();
            this.radioDescription = new Sci.Win.UI.RadioButton();
            this.chkpage = new System.Windows.Forms.CheckBox();
            this.numpage = new Sci.Win.UI.NumericBox();
            this.comboLanguage = new Sci.Win.UI.ComboBox();
            this.labLanguage = new Sci.Win.UI.Label();
            this.chkpagePPA = new System.Windows.Forms.CheckBox();
            this.txtPagePPA = new Sci.Win.UI.TextBox();
            this.radioPanel1.SuspendLayout();
            this.radioPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(435, 12);
            this.print.TabIndex = 10;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(435, 48);
            this.toexcel.TabIndex = 11;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(435, 84);
            this.close.TabIndex = 12;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(389, 120);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(415, 148);
            // 
            // labelLineMappingDisplay
            // 
            this.labelLineMappingDisplay.Location = new System.Drawing.Point(13, 12);
            this.labelLineMappingDisplay.Name = "labelLineMappingDisplay";
            this.labelLineMappingDisplay.Size = new System.Drawing.Size(162, 23);
            this.labelLineMappingDisplay.TabIndex = 94;
            this.labelLineMappingDisplay.Text = "Line mapping display:";
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioU_Right);
            this.radioPanel1.Controls.Add(this.radioZ);
            this.radioPanel1.Controls.Add(this.radioU_Left);
            this.radioPanel1.Location = new System.Drawing.Point(181, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(111, 87);
            this.radioPanel1.TabIndex = 0;
            // 
            // radioU_Right
            // 
            this.radioU_Right.AutoSize = true;
            this.radioU_Right.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioU_Right.Location = new System.Drawing.Point(3, 31);
            this.radioU_Right.Name = "radioU_Right";
            this.radioU_Right.Size = new System.Drawing.Size(82, 21);
            this.radioU_Right.TabIndex = 2;
            this.radioU_Right.TabStop = true;
            this.radioU_Right.Text = "U - Right";
            this.radioU_Right.UseVisualStyleBackColor = true;
            // 
            // radioZ
            // 
            this.radioZ.AutoSize = true;
            this.radioZ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioZ.Location = new System.Drawing.Point(3, 58);
            this.radioZ.Name = "radioZ";
            this.radioZ.Size = new System.Drawing.Size(35, 21);
            this.radioZ.TabIndex = 3;
            this.radioZ.TabStop = true;
            this.radioZ.Text = "Z";
            this.radioZ.UseVisualStyleBackColor = true;
            // 
            // radioU_Left
            // 
            this.radioU_Left.AutoSize = true;
            this.radioU_Left.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioU_Left.Location = new System.Drawing.Point(3, 4);
            this.radioU_Left.Name = "radioU_Left";
            this.radioU_Left.Size = new System.Drawing.Size(73, 21);
            this.radioU_Left.TabIndex = 1;
            this.radioU_Left.TabStop = true;
            this.radioU_Left.Text = "U - Left";
            this.radioU_Left.UseVisualStyleBackColor = true;
            // 
            // labelOperationContentType
            // 
            this.labelOperationContentType.Location = new System.Drawing.Point(13, 102);
            this.labelOperationContentType.Name = "labelOperationContentType";
            this.labelOperationContentType.Size = new System.Drawing.Size(162, 23);
            this.labelOperationContentType.TabIndex = 96;
            this.labelOperationContentType.Text = "Operation content type:";
            // 
            // radioPanel2
            // 
            this.radioPanel2.Controls.Add(this.radioAnnotation);
            this.radioPanel2.Controls.Add(this.radioDescription);
            this.radioPanel2.Location = new System.Drawing.Point(181, 102);
            this.radioPanel2.Name = "radioPanel2";
            this.radioPanel2.Size = new System.Drawing.Size(111, 58);
            this.radioPanel2.TabIndex = 4;
            // 
            // radioAnnotation
            // 
            this.radioAnnotation.AutoSize = true;
            this.radioAnnotation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioAnnotation.Location = new System.Drawing.Point(3, 30);
            this.radioAnnotation.Name = "radioAnnotation";
            this.radioAnnotation.Size = new System.Drawing.Size(94, 21);
            this.radioAnnotation.TabIndex = 6;
            this.radioAnnotation.TabStop = true;
            this.radioAnnotation.Text = "Annotation";
            this.radioAnnotation.UseVisualStyleBackColor = true;
            // 
            // radioDescription
            // 
            this.radioDescription.AutoSize = true;
            this.radioDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDescription.Location = new System.Drawing.Point(3, 2);
            this.radioDescription.Name = "radioDescription";
            this.radioDescription.Size = new System.Drawing.Size(97, 21);
            this.radioDescription.TabIndex = 5;
            this.radioDescription.TabStop = true;
            this.radioDescription.Text = "Description";
            this.radioDescription.UseVisualStyleBackColor = true;
            // 
            // chkpage
            // 
            this.chkpage.AutoSize = true;
            this.chkpage.Location = new System.Drawing.Point(16, 173);
            this.chkpage.Name = "chkpage";
            this.chkpage.Size = new System.Drawing.Size(162, 21);
            this.chkpage.TabIndex = 7;
            this.chkpage.Text = "Change page 2 at No";
            this.chkpage.UseVisualStyleBackColor = true;
            // 
            // numpage
            // 
            this.numpage.BackColor = System.Drawing.Color.White;
            this.numpage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numpage.Location = new System.Drawing.Point(239, 173);
            this.numpage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numpage.Name = "numpage";
            this.numpage.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numpage.Size = new System.Drawing.Size(53, 23);
            this.numpage.TabIndex = 8;
            this.numpage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // comboLanguage
            // 
            this.comboLanguage.BackColor = System.Drawing.Color.White;
            this.comboLanguage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboLanguage.FormattingEnabled = true;
            this.comboLanguage.IsSupportUnselect = true;
            this.comboLanguage.Location = new System.Drawing.Point(181, 243);
            this.comboLanguage.Name = "comboLanguage";
            this.comboLanguage.OldText = "";
            this.comboLanguage.Size = new System.Drawing.Size(121, 24);
            this.comboLanguage.TabIndex = 9;
            // 
            // labLanguage
            // 
            this.labLanguage.Location = new System.Drawing.Point(13, 244);
            this.labLanguage.Name = "labLanguage";
            this.labLanguage.Size = new System.Drawing.Size(162, 23);
            this.labLanguage.TabIndex = 102;
            this.labLanguage.Text = "Language:";
            // 
            // chkpagePPA
            // 
            this.chkpagePPA.AutoSize = true;
            this.chkpagePPA.Location = new System.Drawing.Point(16, 207);
            this.chkpagePPA.Name = "chkpagePPA";
            this.chkpagePPA.Size = new System.Drawing.Size(224, 21);
            this.chkpagePPA.TabIndex = 103;
            this.chkpagePPA.Text = "Change page 2 at No(For PPA)";
            this.chkpagePPA.UseVisualStyleBackColor = true;
            // 
            // txtPagePPA
            // 
            this.txtPagePPA.BackColor = System.Drawing.Color.White;
            this.txtPagePPA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPagePPA.Location = new System.Drawing.Point(239, 205);
            this.txtPagePPA.Name = "txtPagePPA";
            this.txtPagePPA.Size = new System.Drawing.Size(55, 23);
            this.txtPagePPA.TabIndex = 104;
            // 
            // P03_Print
            // 
            this.ClientSize = new System.Drawing.Size(527, 308);
            this.Controls.Add(this.txtPagePPA);
            this.Controls.Add(this.chkpagePPA);
            this.Controls.Add(this.labLanguage);
            this.Controls.Add(this.comboLanguage);
            this.Controls.Add(this.numpage);
            this.Controls.Add(this.chkpage);
            this.Controls.Add(this.radioPanel2);
            this.Controls.Add(this.labelOperationContentType);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.labelLineMappingDisplay);
            this.IsSupportToPrint = false;
            this.Name = "P03_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labelLineMappingDisplay, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.labelOperationContentType, 0);
            this.Controls.SetChildIndex(this.radioPanel2, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.chkpage, 0);
            this.Controls.SetChildIndex(this.numpage, 0);
            this.Controls.SetChildIndex(this.comboLanguage, 0);
            this.Controls.SetChildIndex(this.labLanguage, 0);
            this.Controls.SetChildIndex(this.chkpagePPA, 0);
            this.Controls.SetChildIndex(this.txtPagePPA, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.radioPanel2.ResumeLayout(false);
            this.radioPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelLineMappingDisplay;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioZ;
        private Win.UI.RadioButton radioU_Left;
        private Win.UI.Label labelOperationContentType;
        private Win.UI.RadioPanel radioPanel2;
        private Win.UI.RadioButton radioAnnotation;
        private Win.UI.RadioButton radioDescription;
        private System.Windows.Forms.CheckBox chkpage;
        private Win.UI.NumericBox numpage;
        private Win.UI.ComboBox comboLanguage;
        private Win.UI.Label labLanguage;
        private Win.UI.RadioButton radioU_Right;
        private System.Windows.Forms.CheckBox chkpagePPA;
        private Win.UI.TextBox txtPagePPA;
    }
}
