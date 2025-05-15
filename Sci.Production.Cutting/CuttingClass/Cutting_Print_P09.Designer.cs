namespace Sci.Production.Cutting
{
    partial class Cutting_Print_P09
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
            this.label2 = new Sci.Win.UI.Label();
            this.txtCutRefNoEnd = new Sci.Win.UI.TextBox();
            this.txtCutRefNoStart = new Sci.Win.UI.TextBox();
            this.labelCutRefNo = new Sci.Win.UI.Label();
            this.rdoSortCutRef = new Sci.Win.UI.RadioButton();
            this.rdoSnCc = new Sci.Win.UI.RadioButton();
            this.radioGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(439, 12);
            this.print.TabIndex = 1;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(439, 48);
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(439, 84);
            this.close.TabIndex = 3;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.rdoSnCc);
            this.radioGroup1.Controls.Add(this.rdoSortCutRef);
            this.radioGroup1.Controls.Add(this.label1);
            this.radioGroup1.Controls.Add(this.label2);
            this.radioGroup1.Controls.Add(this.txtCutRefNoEnd);
            this.radioGroup1.Controls.Add(this.txtCutRefNoStart);
            this.radioGroup1.Controls.Add(this.labelCutRefNo);
            this.radioGroup1.Location = new System.Drawing.Point(12, 12);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(412, 135);
            this.radioGroup1.TabIndex = 0;
            this.radioGroup1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Sheets Sort By";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(232, 23);
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
            this.txtCutRefNoEnd.Location = new System.Drawing.Point(254, 23);
            this.txtCutRefNoEnd.Name = "txtCutRefNoEnd";
            this.txtCutRefNoEnd.Size = new System.Drawing.Size(108, 23);
            this.txtCutRefNoEnd.TabIndex = 3;
            // 
            // txtCutRefNoStart
            // 
            this.txtCutRefNoStart.BackColor = System.Drawing.Color.White;
            this.txtCutRefNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefNoStart.Location = new System.Drawing.Point(121, 23);
            this.txtCutRefNoStart.Name = "txtCutRefNoStart";
            this.txtCutRefNoStart.Size = new System.Drawing.Size(108, 23);
            this.txtCutRefNoStart.TabIndex = 2;
            // 
            // labelCutRefNo
            // 
            this.labelCutRefNo.Location = new System.Drawing.Point(18, 23);
            this.labelCutRefNo.Name = "labelCutRefNo";
            this.labelCutRefNo.Size = new System.Drawing.Size(100, 23);
            this.labelCutRefNo.TabIndex = 4;
            this.labelCutRefNo.Text = "Cut RefNo";
            // 
            // rdoSortCutRef
            // 
            this.rdoSortCutRef.AutoSize = true;
            this.rdoSortCutRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdoSortCutRef.Location = new System.Drawing.Point(121, 55);
            this.rdoSortCutRef.Name = "rdoSortCutRef";
            this.rdoSortCutRef.Size = new System.Drawing.Size(69, 21);
            this.rdoSortCutRef.TabIndex = 8;
            this.rdoSortCutRef.Text = "CutRef";
            this.rdoSortCutRef.UseVisualStyleBackColor = true;
            // 
            // rdoSnCc
            // 
            this.rdoSnCc.AutoSize = true;
            this.rdoSnCc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdoSnCc.Location = new System.Drawing.Point(121, 82);
            this.rdoSnCc.Name = "rdoSnCc";
            this.rdoSnCc.Size = new System.Drawing.Size(169, 21);
            this.rdoSnCc.TabIndex = 9;
            this.rdoSnCc.Text = "Spreading No, Cut Cell";
            this.rdoSnCc.UseVisualStyleBackColor = true;
            // 
            // Cutting_Print_P09
            // 
            this.ClientSize = new System.Drawing.Size(531, 187);
            this.Controls.Add(this.radioGroup1);
            this.DefaultControl = "txtCutRefNoStart";
            this.DefaultControlForEdit = "txtCutRefNoStart";
            this.IsSupportToPrint = false;
            this.Name = "Cutting_Print_P09";
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
        private Win.UI.Label labelCutRefNo;
        private Win.UI.Label label1;
        private Win.UI.RadioButton rdoSortCutRef;
        private Win.UI.RadioButton rdoSnCc;
    }
}
