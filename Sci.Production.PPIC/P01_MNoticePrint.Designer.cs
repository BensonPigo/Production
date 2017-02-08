namespace Sci.Production.PPIC
{
    partial class P01_MNoticePrint
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
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.chkAdditional = new Sci.Win.UI.CheckBox();
            this.radioButton_ByCustCD = new Sci.Win.UI.RadioButton();
            this.radioButton_MNotice = new Sci.Win.UI.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAdditional);
            this.groupBox1.Controls.Add(this.radioButton_ByCustCD);
            this.groupBox1.Controls.Add(this.radioButton_MNotice);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(495, 154);
            this.groupBox1.TabIndex = 96;
            this.groupBox1.TabStop = false;
            // 
            // chkAdditional
            // 
            this.chkAdditional.AutoSize = true;
            this.chkAdditional.Checked = true;
            this.chkAdditional.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdditional.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkAdditional.Location = new System.Drawing.Point(18, 107);
            this.chkAdditional.Name = "chkAdditional";
            this.chkAdditional.Size = new System.Drawing.Size(321, 21);
            this.chkAdditional.TabIndex = 8;
            this.chkAdditional.Text = "Additionally print [Size Spec] with \"z\" beginning";
            this.chkAdditional.UseVisualStyleBackColor = true;
            // 
            // radioButton_ByCustCD
            // 
            this.radioButton_ByCustCD.AutoSize = true;
            this.radioButton_ByCustCD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioButton_ByCustCD.Location = new System.Drawing.Point(18, 63);
            this.radioButton_ByCustCD.Name = "radioButton_ByCustCD";
            this.radioButton_ByCustCD.Size = new System.Drawing.Size(213, 21);
            this.radioButton_ByCustCD.TabIndex = 7;
            this.radioButton_ByCustCD.Text = "M/Notice (Combo by CustCD )";
            this.radioButton_ByCustCD.UseVisualStyleBackColor = true;
            // 
            // radioButton_MNotice
            // 
            this.radioButton_MNotice.AutoSize = true;
            this.radioButton_MNotice.Checked = true;
            this.radioButton_MNotice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioButton_MNotice.Location = new System.Drawing.Point(18, 22);
            this.radioButton_MNotice.Name = "radioButton_MNotice";
            this.radioButton_MNotice.Size = new System.Drawing.Size(81, 21);
            this.radioButton_MNotice.TabIndex = 6;
            this.radioButton_MNotice.TabStop = true;
            this.radioButton_MNotice.Text = "M/Notice";
            this.radioButton_MNotice.UseVisualStyleBackColor = true;
            // 
            // P01_MNoticePrint
            // 
            this.ClientSize = new System.Drawing.Size(627, 200);
            this.Controls.Add(this.groupBox1);
            this.Name = "P01_MNoticePrint";
            this.Text = "P01.M/Notice Print";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.GroupBox groupBox1;
        private Win.UI.CheckBox chkAdditional;
        private Win.UI.RadioButton radioButton_ByCustCD;
        private Win.UI.RadioButton radioButton_MNotice;


    }
}
