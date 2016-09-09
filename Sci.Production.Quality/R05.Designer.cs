namespace Sci.Production.Quality
{
    partial class R05
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
            this.label2 = new Sci.Win.UI.Label();
            this.comboMaterialType = new Sci.Win.UI.ComboBox();
            this.label1 = new Sci.Win.UI.Label();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.label10 = new Sci.Win.UI.Label();
            this.DateSCIDelivery = new Sci.Win.UI.DateRange();
            this.label3 = new Sci.Win.UI.Label();
            this.radioSummary = new Sci.Win.UI.RadioButton();
            this.radioDetail = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioDetail);
            this.radioPanel1.Controls.Add(this.radioSummary);
            this.radioPanel1.Controls.Add(this.label2);
            this.radioPanel1.Controls.Add(this.comboMaterialType);
            this.radioPanel1.Controls.Add(this.label1);
            this.radioPanel1.Controls.Add(this.comboCategory);
            this.radioPanel1.Controls.Add(this.label10);
            this.radioPanel1.Controls.Add(this.DateSCIDelivery);
            this.radioPanel1.Controls.Add(this.label3);
            this.radioPanel1.Location = new System.Drawing.Point(30, 24);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(440, 217);
            this.radioPanel1.TabIndex = 94;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(29, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 23);
            this.label2.TabIndex = 104;
            this.label2.Text = "Report Type:";
            // 
            // comboMaterialType
            // 
            this.comboMaterialType.BackColor = System.Drawing.Color.White;
            this.comboMaterialType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMaterialType.FormattingEnabled = true;
            this.comboMaterialType.IsSupportUnselect = true;
            this.comboMaterialType.Location = new System.Drawing.Point(128, 104);
            this.comboMaterialType.Name = "comboMaterialType";
            this.comboMaterialType.Size = new System.Drawing.Size(125, 24);
            this.comboMaterialType.TabIndex = 103;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(29, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 23);
            this.label1.TabIndex = 102;
            this.label1.Text = "Material Type:";
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(128, 64);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.Size = new System.Drawing.Size(125, 24);
            this.comboCategory.TabIndex = 101;
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(29, 64);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 23);
            this.label10.TabIndex = 100;
            this.label10.Text = "Category:";
            // 
            // DateSCIDelivery
            // 
            this.DateSCIDelivery.Location = new System.Drawing.Point(128, 24);
            this.DateSCIDelivery.Name = "DateSCIDelivery";
            this.DateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.DateSCIDelivery.TabIndex = 99;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(29, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 23);
            this.label3.TabIndex = 98;
            this.label3.Text = "SCI Delivery:";
            // 
            // radioSummary
            // 
            this.radioSummary.AutoSize = true;
            this.radioSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSummary.Location = new System.Drawing.Point(128, 146);
            this.radioSummary.Name = "radioSummary";
            this.radioSummary.Size = new System.Drawing.Size(85, 21);
            this.radioSummary.TabIndex = 105;
            this.radioSummary.TabStop = true;
            this.radioSummary.Text = "Summary";
            this.radioSummary.UseVisualStyleBackColor = true;
            // 
            // radioDetail
            // 
            this.radioDetail.AutoSize = true;
            this.radioDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail.Location = new System.Drawing.Point(128, 173);
            this.radioDetail.Name = "radioDetail";
            this.radioDetail.Size = new System.Drawing.Size(62, 21);
            this.radioDetail.TabIndex = 106;
            this.radioDetail.TabStop = true;
            this.radioDetail.Text = "Detail";
            this.radioDetail.UseVisualStyleBackColor = true;
            // 
            // R05
            // 
            this.ClientSize = new System.Drawing.Size(627, 290);
            this.Controls.Add(this.radioPanel1);
            this.Name = "R05";
            this.Text = "R05.Monthly Material Suppliers Evaluation Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioDetail;
        private Win.UI.RadioButton radioSummary;
        private Win.UI.Label label2;
        private Win.UI.ComboBox comboMaterialType;
        private Win.UI.Label label1;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.Label label10;
        private Win.UI.DateRange DateSCIDelivery;
        private Win.UI.Label label3;

    }
}
