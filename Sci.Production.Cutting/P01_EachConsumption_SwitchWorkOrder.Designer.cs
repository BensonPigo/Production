namespace Sci.Production.Cutting
{
    partial class P01_EachConsumption_SwitchWorkOrder
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
            this.BYSP = new Sci.Win.UI.RadioButton();
            this.Combination = new Sci.Win.UI.RadioButton();
            this.OK_But = new Sci.Win.UI.Button();
            this.Cancel_But = new Sci.Win.UI.Button();
            this.radioGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.BYSP);
            this.radioGroup1.Controls.Add(this.Combination);
            this.radioGroup1.Location = new System.Drawing.Point(64, 32);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(183, 114);
            this.radioGroup1.TabIndex = 0;
            this.radioGroup1.TabStop = false;
            // 
            // BYSP
            // 
            this.BYSP.AutoSize = true;
            this.BYSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BYSP.Location = new System.Drawing.Point(26, 60);
            this.BYSP.Name = "BYSP";
            this.BYSP.Size = new System.Drawing.Size(72, 21);
            this.BYSP.TabIndex = 1;
            this.BYSP.TabStop = true;
            this.BYSP.Text = "By SP#";
            this.BYSP.UseVisualStyleBackColor = true;
            // 
            // Combination
            // 
            this.Combination.AutoSize = true;
            this.Combination.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Combination.Location = new System.Drawing.Point(26, 33);
            this.Combination.Name = "Combination";
            this.Combination.Size = new System.Drawing.Size(104, 21);
            this.Combination.TabIndex = 0;
            this.Combination.TabStop = true;
            this.Combination.Text = "Combination";
            this.Combination.UseVisualStyleBackColor = true;
            // 
            // OK_But
            // 
            this.OK_But.Location = new System.Drawing.Point(273, 41);
            this.OK_But.Name = "OK_But";
            this.OK_But.Size = new System.Drawing.Size(80, 30);
            this.OK_But.TabIndex = 1;
            this.OK_But.Text = "OK";
            this.OK_But.UseVisualStyleBackColor = true;
            this.OK_But.Click += new System.EventHandler(this.OK_But_Click);
            // 
            // Cancel_But
            // 
            this.Cancel_But.Location = new System.Drawing.Point(273, 77);
            this.Cancel_But.Name = "Cancel_But";
            this.Cancel_But.Size = new System.Drawing.Size(80, 30);
            this.Cancel_But.TabIndex = 2;
            this.Cancel_But.Text = "Cancel";
            this.Cancel_But.UseVisualStyleBackColor = true;
            this.Cancel_But.Click += new System.EventHandler(this.Cancel_But_Click);
            // 
            // P01_EachCons_SwitchWorkOrder
            // 
            this.ClientSize = new System.Drawing.Size(390, 223);
            this.Controls.Add(this.Cancel_But);
            this.Controls.Add(this.OK_But);
            this.Controls.Add(this.radioGroup1);
            this.Name = "P01_EachCons_SwitchWorkOrder";
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.RadioGroup radioGroup1;
        private Win.UI.RadioButton BYSP;
        private Win.UI.RadioButton Combination;
        private Win.UI.Button OK_But;
        private Win.UI.Button Cancel_But;

    }
}
