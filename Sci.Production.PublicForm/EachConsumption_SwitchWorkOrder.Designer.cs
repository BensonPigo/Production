namespace Sci.Production.PublicForm
{
    partial class EachConsumption_SwitchWorkOrder
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
            this.radioGroup1 = new Sci.Win.UI.RadioGroup();
            this.radioBySP = new Sci.Win.UI.RadioButton();
            this.radioCombination = new Sci.Win.UI.RadioButton();
            this.btnOK = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            this.radioGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.radioBySP);
            this.radioGroup1.Controls.Add(this.radioCombination);
            this.radioGroup1.Location = new System.Drawing.Point(11, 8);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(183, 85);
            this.radioGroup1.TabIndex = 0;
            this.radioGroup1.TabStop = false;
            // 
            // radioBySP
            // 
            this.radioBySP.AutoSize = true;
            this.radioBySP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBySP.Location = new System.Drawing.Point(19, 48);
            this.radioBySP.Name = "radioBySP";
            this.radioBySP.Size = new System.Drawing.Size(72, 21);
            this.radioBySP.TabIndex = 1;
            this.radioBySP.TabStop = true;
            this.radioBySP.Text = "By SP#";
            this.radioBySP.UseVisualStyleBackColor = true;
            // 
            // radioCombination
            // 
            this.radioCombination.AutoSize = true;
            this.radioCombination.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioCombination.Location = new System.Drawing.Point(19, 21);
            this.radioCombination.Name = "radioCombination";
            this.radioCombination.Size = new System.Drawing.Size(104, 21);
            this.radioCombination.TabIndex = 0;
            this.radioCombination.TabStop = true;
            this.radioCombination.Text = "Combination";
            this.radioCombination.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(220, 17);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(220, 53);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // EachConsumption_SwitchWorkOrder
            // 
            this.ClientSize = new System.Drawing.Size(334, 115);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.radioGroup1);
            this.Name = "EachConsumption_SwitchWorkOrder";
            this.Text = "Switch To WorkOrder";
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.RadioGroup radioGroup1;
        private Win.UI.RadioButton radioBySP;
        private Win.UI.RadioButton radioCombination;
        private Win.UI.Button btnOK;
        private Win.UI.Button btnCancel;

    }
}
