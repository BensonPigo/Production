namespace Sci.Production.Warehouse
{
    partial class P51_Print
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
            this.radioBackwardSocktakingLi = new Sci.Win.UI.RadioButton();
            this.radioBackwardSocktakingForm = new Sci.Win.UI.RadioButton();
            this.label1 = new Sci.Win.UI.Label();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(535, 51);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(535, 87);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioBackwardSocktakingLi);
            this.radioPanel1.Controls.Add(this.radioBackwardSocktakingForm);
            this.radioPanel1.Controls.Add(this.label1);
            this.radioPanel1.Location = new System.Drawing.Point(49, 40);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(425, 128);
            this.radioPanel1.TabIndex = 94;
            this.radioPanel1.Value = "Backward Socktaking Form";
            this.radioPanel1.ValueChanged += new System.EventHandler(this.RadioGroup1_ValueChanged);
            // 
            // radioBackwardSocktakingLi
            // 
            this.radioBackwardSocktakingLi.AutoSize = true;
            this.radioBackwardSocktakingLi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBackwardSocktakingLi.Location = new System.Drawing.Point(19, 74);
            this.radioBackwardSocktakingLi.Name = "radioBackwardSocktakingLi";
            this.radioBackwardSocktakingLi.Size = new System.Drawing.Size(186, 21);
            this.radioBackwardSocktakingLi.TabIndex = 2;
            this.radioBackwardSocktakingLi.Text = "Backward Socktaking List";
            this.radioBackwardSocktakingLi.UseVisualStyleBackColor = true;
            this.radioBackwardSocktakingLi.Value = "Backward Socktaking Li";
            // 
            // radioBackwardSocktakingForm
            // 
            this.radioBackwardSocktakingForm.AutoSize = true;
            this.radioBackwardSocktakingForm.Checked = true;
            this.radioBackwardSocktakingForm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBackwardSocktakingForm.Location = new System.Drawing.Point(19, 47);
            this.radioBackwardSocktakingForm.Name = "radioBackwardSocktakingForm";
            this.radioBackwardSocktakingForm.Size = new System.Drawing.Size(196, 21);
            this.radioBackwardSocktakingForm.TabIndex = 1;
            this.radioBackwardSocktakingForm.TabStop = true;
            this.radioBackwardSocktakingForm.Text = "Backward Socktaking Form";
            this.radioBackwardSocktakingForm.UseVisualStyleBackColor = true;
            this.radioBackwardSocktakingForm.Value = "Backward Socktaking Form";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(19, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "Report Type";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // P51_Print
            // 
            this.ClientSize = new System.Drawing.Size(627, 314);
            this.Controls.Add(this.radioPanel1);
            this.Name = "P51_Print";
            this.Text = "() ";
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
        private Win.UI.RadioButton radioBackwardSocktakingLi;
        private Win.UI.RadioButton radioBackwardSocktakingForm;
        private Win.UI.Label label1;
    }
}
