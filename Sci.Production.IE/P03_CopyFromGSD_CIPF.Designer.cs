namespace Sci.Production.IE
{
    partial class P03_CopyFromGSD_CIPF
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.chkInspection = new System.Windows.Forms.CheckBox();
            this.chkPacking = new System.Windows.Forms.CheckBox();
            this.chkPressing = new System.Windows.Forms.CheckBox();
            this.chkCutting = new System.Windows.Forms.CheckBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.button1 = new Sci.Win.UI.Button();
            this.button2 = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // chkInspection
            // 
            this.chkInspection.AutoSize = true;
            this.chkInspection.Location = new System.Drawing.Point(222, 10);
            this.chkInspection.Name = "chkInspection";
            this.chkInspection.Size = new System.Drawing.Size(91, 21);
            this.chkInspection.TabIndex = 99;
            this.chkInspection.Text = "Inspection";
            this.chkInspection.UseVisualStyleBackColor = true;
            // 
            // chkPacking
            // 
            this.chkPacking.AutoSize = true;
            this.chkPacking.Location = new System.Drawing.Point(222, 37);
            this.chkPacking.Name = "chkPacking";
            this.chkPacking.Size = new System.Drawing.Size(77, 21);
            this.chkPacking.TabIndex = 101;
            this.chkPacking.Text = "Packing";
            this.chkPacking.UseVisualStyleBackColor = true;
            // 
            // chkPressing
            // 
            this.chkPressing.AutoSize = true;
            this.chkPressing.Location = new System.Drawing.Point(134, 37);
            this.chkPressing.Name = "chkPressing";
            this.chkPressing.Size = new System.Drawing.Size(82, 21);
            this.chkPressing.TabIndex = 100;
            this.chkPressing.Text = "Pressing";
            this.chkPressing.UseVisualStyleBackColor = true;
            // 
            // chkCutting
            // 
            this.chkCutting.AutoSize = true;
            this.chkCutting.Location = new System.Drawing.Point(134, 10);
            this.chkCutting.Name = "chkCutting";
            this.chkCutting.Size = new System.Drawing.Size(71, 21);
            this.chkCutting.TabIndex = 98;
            this.chkCutting.Text = "Cutting";
            this.chkCutting.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Lines = 2;
            this.label2.Location = new System.Drawing.Point(9, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 27);
            this.label2.TabIndex = 103;
            this.label2.Text = "(CIPF)";
            this.label2.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Lines = 2;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 22);
            this.label1.TabIndex = 102;
            this.label1.Text = "Operation Include";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(56, 64);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 104;
            this.button1.Text = "Comfirm";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(180, 64);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 30);
            this.button2.TabIndex = 105;
            this.button2.Text = "No Need";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // P03_CopyFromGSD_CIPF
            // 
            this.ClientSize = new System.Drawing.Size(323, 100);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chkInspection);
            this.Controls.Add(this.chkPacking);
            this.Controls.Add(this.chkPressing);
            this.Controls.Add(this.chkCutting);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "P03_CopyFromGSD_CIPF";
            this.Text = "P03. CopyFromGSD_CIPF";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkInspection;
        private System.Windows.Forms.CheckBox chkPacking;
        private System.Windows.Forms.CheckBox chkPressing;
        private System.Windows.Forms.CheckBox chkCutting;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Button button1;
        private Win.UI.Button button2;
    }
}
