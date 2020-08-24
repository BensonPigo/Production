namespace Sci.Production.Cutting
{
    partial class P02_Print
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
            this.label2 = new Sci.Win.UI.Label();
            this.txtCutRefNoEnd = new Sci.Win.UI.TextBox();
            this.txtCutRefNoStart = new Sci.Win.UI.TextBox();
            this.labelCutRefNo = new Sci.Win.UI.Label();
            this.radioByCutplanId = new Sci.Win.UI.RadioButton();
            this.radioByCutRefNo = new Sci.Win.UI.RadioButton();
            this.label1 = new Sci.Win.UI.Label();
            this.cmbSort = new Sci.Win.UI.ComboBox();
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
            this.radioGroup1.Controls.Add(this.cmbSort);
            this.radioGroup1.Controls.Add(this.label1);
            this.radioGroup1.Controls.Add(this.label2);
            this.radioGroup1.Controls.Add(this.txtCutRefNoEnd);
            this.radioGroup1.Controls.Add(this.txtCutRefNoStart);
            this.radioGroup1.Controls.Add(this.labelCutRefNo);
            this.radioGroup1.Controls.Add(this.radioByCutplanId);
            this.radioGroup1.Controls.Add(this.radioByCutRefNo);
            this.radioGroup1.Location = new System.Drawing.Point(12, 12);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(412, 166);
            this.radioGroup1.TabIndex = 0;
            this.radioGroup1.TabStop = false;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(232, 104);
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
            this.txtCutRefNoEnd.Location = new System.Drawing.Point(254, 104);
            this.txtCutRefNoEnd.Name = "txtCutRefNoEnd";
            this.txtCutRefNoEnd.Size = new System.Drawing.Size(108, 23);
            this.txtCutRefNoEnd.TabIndex = 3;
            // 
            // txtCutRefNoStart
            // 
            this.txtCutRefNoStart.BackColor = System.Drawing.Color.White;
            this.txtCutRefNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefNoStart.Location = new System.Drawing.Point(121, 104);
            this.txtCutRefNoStart.Name = "txtCutRefNoStart";
            this.txtCutRefNoStart.Size = new System.Drawing.Size(108, 23);
            this.txtCutRefNoStart.TabIndex = 2;
            // 
            // labelCutRefNo
            // 
            this.labelCutRefNo.Location = new System.Drawing.Point(43, 104);
            this.labelCutRefNo.Name = "labelCutRefNo";
            this.labelCutRefNo.Size = new System.Drawing.Size(75, 23);
            this.labelCutRefNo.TabIndex = 4;
            this.labelCutRefNo.Text = "Cut RefNo";
            // 
            // radioByCutplanId
            // 
            this.radioByCutplanId.AutoSize = true;
            this.radioByCutplanId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByCutplanId.Location = new System.Drawing.Point(44, 77);
            this.radioByCutplanId.Name = "radioByCutplanId";
            this.radioByCutplanId.Size = new System.Drawing.Size(109, 21);
            this.radioByCutplanId.TabIndex = 1;
            this.radioByCutplanId.Text = "By Cutplan Id";
            this.radioByCutplanId.UseVisualStyleBackColor = true;
            this.radioByCutplanId.CheckedChanged += new System.EventHandler(this.RadioByCutplanId_CheckedChanged);
            // 
            // radioByCutRefNo
            // 
            this.radioByCutRefNo.AutoSize = true;
            this.radioByCutRefNo.Checked = true;
            this.radioByCutRefNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByCutRefNo.Location = new System.Drawing.Point(44, 22);
            this.radioByCutRefNo.Name = "radioByCutRefNo";
            this.radioByCutRefNo.Size = new System.Drawing.Size(111, 21);
            this.radioByCutRefNo.TabIndex = 0;
            this.radioByCutRefNo.TabStop = true;
            this.radioByCutRefNo.Text = "By Cut RefNo";
            this.radioByCutRefNo.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(62, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Sheets sort by";
            // 
            // cmbSort
            // 
            this.cmbSort.BackColor = System.Drawing.Color.White;
            this.cmbSort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbSort.FormattingEnabled = true;
            this.cmbSort.IsSupportUnselect = true;
            this.cmbSort.Items.AddRange(new object[] {
            "CutRef#",
            "SpreadingNo, CutCell"});
            this.cmbSort.Location = new System.Drawing.Point(158, 46);
            this.cmbSort.Name = "cmbSort";
            this.cmbSort.OldText = "";
            this.cmbSort.Size = new System.Drawing.Size(155, 24);
            this.cmbSort.TabIndex = 7;
            // 
            // P02_Print
            // 
            this.ClientSize = new System.Drawing.Size(531, 220);
            this.Controls.Add(this.radioGroup1);
            this.DefaultControl = "txtCutRefNoStart";
            this.DefaultControlForEdit = "txtCutRefNoStart";
            this.IsSupportToPrint = false;
            this.Name = "P02_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "Spreading Report";
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
        private Win.UI.RadioButton radioByCutplanId;
        private Win.UI.RadioButton radioByCutRefNo;
        private Win.UI.ComboBox cmbSort;
        private Win.UI.Label label1;
    }
}
