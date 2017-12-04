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
            this.radioZ = new Sci.Win.UI.RadioButton();
            this.radioU = new Sci.Win.UI.RadioButton();
            this.labelOperationContentType = new Sci.Win.UI.Label();
            this.radioPanel2 = new Sci.Win.UI.RadioPanel();
            this.radioAnnotation = new Sci.Win.UI.RadioButton();
            this.radioDescription = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.radioPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(336, 12);
            this.print.TabIndex = 1;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(336, 48);
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(336, 84);
            this.close.TabIndex = 3;
            // 
            // labelLineMappingDisplay
            // 
            this.labelLineMappingDisplay.Location = new System.Drawing.Point(13, 12);
            this.labelLineMappingDisplay.Name = "labelLineMappingDisplay";
            this.labelLineMappingDisplay.Size = new System.Drawing.Size(137, 23);
            this.labelLineMappingDisplay.TabIndex = 94;
            this.labelLineMappingDisplay.Text = "Line mapping display:";
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioZ);
            this.radioPanel1.Controls.Add(this.radioU);
            this.radioPanel1.Location = new System.Drawing.Point(154, 8);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(50, 54);
            this.radioPanel1.TabIndex = 95;
            // 
            // radioZ
            // 
            this.radioZ.AutoSize = true;
            this.radioZ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioZ.Location = new System.Drawing.Point(3, 32);
            this.radioZ.Name = "radioZ";
            this.radioZ.Size = new System.Drawing.Size(35, 21);
            this.radioZ.TabIndex = 1;
            this.radioZ.TabStop = true;
            this.radioZ.Text = "Z";
            this.radioZ.UseVisualStyleBackColor = true;
            // 
            // radioU
            // 
            this.radioU.AutoSize = true;
            this.radioU.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioU.Location = new System.Drawing.Point(3, 4);
            this.radioU.Name = "radioU";
            this.radioU.Size = new System.Drawing.Size(36, 21);
            this.radioU.TabIndex = 0;
            this.radioU.TabStop = true;
            this.radioU.Text = "U";
            this.radioU.UseVisualStyleBackColor = true;
            // 
            // labelOperationContentType
            // 
            this.labelOperationContentType.Location = new System.Drawing.Point(13, 75);
            this.labelOperationContentType.Name = "labelOperationContentType";
            this.labelOperationContentType.Size = new System.Drawing.Size(147, 23);
            this.labelOperationContentType.TabIndex = 96;
            this.labelOperationContentType.Text = "Operation content type:";
            // 
            // radioPanel2
            // 
            this.radioPanel2.Controls.Add(this.radioAnnotation);
            this.radioPanel2.Controls.Add(this.radioDescription);
            this.radioPanel2.Location = new System.Drawing.Point(164, 73);
            this.radioPanel2.Name = "radioPanel2";
            this.radioPanel2.Size = new System.Drawing.Size(111, 58);
            this.radioPanel2.TabIndex = 97;
            // 
            // radioAnnotation
            // 
            this.radioAnnotation.AutoSize = true;
            this.radioAnnotation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioAnnotation.Location = new System.Drawing.Point(3, 30);
            this.radioAnnotation.Name = "radioAnnotation";
            this.radioAnnotation.Size = new System.Drawing.Size(94, 21);
            this.radioAnnotation.TabIndex = 1;
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
            this.radioDescription.TabIndex = 0;
            this.radioDescription.TabStop = true;
            this.radioDescription.Text = "Description";
            this.radioDescription.UseVisualStyleBackColor = true;
            // 
            // P03_Print
            // 
            this.ClientSize = new System.Drawing.Size(428, 166);
            this.Controls.Add(this.radioPanel2);
            this.Controls.Add(this.labelOperationContentType);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.labelLineMappingDisplay);
            this.IsSupportToPrint = false;
            this.Name = "P03_Print";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelLineMappingDisplay, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.labelOperationContentType, 0);
            this.Controls.SetChildIndex(this.radioPanel2, 0);
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
        private Win.UI.RadioButton radioU;
        private Win.UI.Label labelOperationContentType;
        private Win.UI.RadioPanel radioPanel2;
        private Win.UI.RadioButton radioAnnotation;
        private Win.UI.RadioButton radioDescription;
    }
}
