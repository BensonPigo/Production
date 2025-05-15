namespace Sci.Production.Cutting
{
    partial class Cutting_Print_P02
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
            this.label1 = new Sci.Win.UI.Label();
            this.txtCutPlanEnd = new Sci.Win.UI.TextBox();
            this.txtCutPlanStart = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txtCutRefNoEnd = new Sci.Win.UI.TextBox();
            this.txtCutRefNoStart = new Sci.Win.UI.TextBox();
            this.radioByCutplanId = new Sci.Win.UI.RadioButton();
            this.radioByCutRefNo = new Sci.Win.UI.RadioButton();
            this.radioGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(441, 12);
            this.print.TabIndex = 1;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(441, 48);
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(441, 84);
            this.close.TabIndex = 3;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.label1);
            this.radioGroup1.Controls.Add(this.txtCutPlanEnd);
            this.radioGroup1.Controls.Add(this.txtCutPlanStart);
            this.radioGroup1.Controls.Add(this.label2);
            this.radioGroup1.Controls.Add(this.txtCutRefNoEnd);
            this.radioGroup1.Controls.Add(this.txtCutRefNoStart);
            this.radioGroup1.Controls.Add(this.radioByCutplanId);
            this.radioGroup1.Controls.Add(this.radioByCutRefNo);
            this.radioGroup1.Location = new System.Drawing.Point(12, 12);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(408, 102);
            this.radioGroup1.TabIndex = 0;
            this.radioGroup1.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(241, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 23);
            this.label1.TabIndex = 9;
            this.label1.Text = "~";
            this.label1.TextStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // 
            // txtCutPlanEnd
            // 
            this.txtCutPlanEnd.BackColor = System.Drawing.Color.White;
            this.txtCutPlanEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutPlanEnd.Location = new System.Drawing.Point(263, 57);
            this.txtCutPlanEnd.Name = "txtCutPlanEnd";
            this.txtCutPlanEnd.Size = new System.Drawing.Size(108, 23);
            this.txtCutPlanEnd.TabIndex = 7;
            // 
            // txtCutPlanStart
            // 
            this.txtCutPlanStart.BackColor = System.Drawing.Color.White;
            this.txtCutPlanStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutPlanStart.Location = new System.Drawing.Point(130, 57);
            this.txtCutPlanStart.Name = "txtCutPlanStart";
            this.txtCutPlanStart.Size = new System.Drawing.Size(108, 23);
            this.txtCutPlanStart.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(241, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "~";
            this.label2.TextStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // 
            // txtCutRefNoEnd
            // 
            this.txtCutRefNoEnd.BackColor = System.Drawing.Color.White;
            this.txtCutRefNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefNoEnd.Location = new System.Drawing.Point(263, 21);
            this.txtCutRefNoEnd.Name = "txtCutRefNoEnd";
            this.txtCutRefNoEnd.Size = new System.Drawing.Size(108, 23);
            this.txtCutRefNoEnd.TabIndex = 3;
            // 
            // txtCutRefNoStart
            // 
            this.txtCutRefNoStart.BackColor = System.Drawing.Color.White;
            this.txtCutRefNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefNoStart.Location = new System.Drawing.Point(130, 21);
            this.txtCutRefNoStart.Name = "txtCutRefNoStart";
            this.txtCutRefNoStart.Size = new System.Drawing.Size(108, 23);
            this.txtCutRefNoStart.TabIndex = 2;
            // 
            // radioByCutplanId
            // 
            this.radioByCutplanId.AutoSize = true;
            this.radioByCutplanId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByCutplanId.Location = new System.Drawing.Point(5, 59);
            this.radioByCutplanId.Name = "radioByCutplanId";
            this.radioByCutplanId.Size = new System.Drawing.Size(112, 21);
            this.radioByCutplanId.TabIndex = 1;
            this.radioByCutplanId.Text = "By CutPlan ID";
            this.radioByCutplanId.UseVisualStyleBackColor = true;
            this.radioByCutplanId.CheckedChanged += new System.EventHandler(this.RadioByCutplanId_CheckedChanged);
            // 
            // radioByCutRefNo
            // 
            this.radioByCutRefNo.AutoSize = true;
            this.radioByCutRefNo.Checked = true;
            this.radioByCutRefNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByCutRefNo.Location = new System.Drawing.Point(5, 23);
            this.radioByCutRefNo.Name = "radioByCutRefNo";
            this.radioByCutRefNo.Size = new System.Drawing.Size(89, 21);
            this.radioByCutRefNo.TabIndex = 0;
            this.radioByCutRefNo.TabStop = true;
            this.radioByCutRefNo.Text = "By CutRef";
            this.radioByCutRefNo.UseVisualStyleBackColor = true;
            this.radioByCutRefNo.CheckedChanged += new System.EventHandler(this.RadioByCutRefNo_CheckedChanged);
            // 
            // Cutting_Print_P02
            // 
            this.ClientSize = new System.Drawing.Size(533, 159);
            this.Controls.Add(this.radioGroup1);
            this.DefaultControl = "txtCutRefNoStart";
            this.DefaultControlForEdit = "txtCutRefNoStart";
            this.IsSupportToPrint = false;
            this.Name = "Cutting_Print_P02";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "Spreading Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioGroup1, 0);
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioGroup radioGroup1;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtCutRefNoEnd;
        private Win.UI.TextBox txtCutRefNoStart;
        private Win.UI.RadioButton radioByCutplanId;
        private Win.UI.RadioButton radioByCutRefNo;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtCutPlanEnd;
        private Win.UI.TextBox txtCutPlanStart;
    }
}
