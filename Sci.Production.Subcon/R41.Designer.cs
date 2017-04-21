namespace Sci.Production.Subcon
{
    partial class R41
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
            this.labelCutRefNo = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelBundleCDate = new Sci.Win.UI.Label();
            this.labelSubProcess = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.dateBundleCDate = new Sci.Win.UI.DateRange();
            this.comboSubProcess = new Sci.Win.UI.ComboBox();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCutRefStart = new System.Windows.Forms.TextBox();
            this.txtCutRefEnd = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(425, 12);
            this.print.TabIndex = 6;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(425, 48);
            this.toexcel.TabIndex = 7;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(425, 84);
            this.close.TabIndex = 8;
            // 
            // labelCutRefNo
            // 
            this.labelCutRefNo.Lines = 0;
            this.labelCutRefNo.Location = new System.Drawing.Point(27, 19);
            this.labelCutRefNo.Name = "labelCutRefNo";
            this.labelCutRefNo.Size = new System.Drawing.Size(95, 23);
            this.labelCutRefNo.TabIndex = 94;
            this.labelCutRefNo.Text = "Cut Ref#";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(27, 48);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(95, 23);
            this.labelSPNo.TabIndex = 95;
            this.labelSPNo.Text = "SP#";
            // 
            // labelBundleCDate
            // 
            this.labelBundleCDate.Lines = 0;
            this.labelBundleCDate.Location = new System.Drawing.Point(27, 77);
            this.labelBundleCDate.Name = "labelBundleCDate";
            this.labelBundleCDate.Size = new System.Drawing.Size(95, 23);
            this.labelBundleCDate.TabIndex = 96;
            this.labelBundleCDate.Text = "Bundle CDate";
            // 
            // labelSubProcess
            // 
            this.labelSubProcess.Lines = 0;
            this.labelSubProcess.Location = new System.Drawing.Point(27, 107);
            this.labelSubProcess.Name = "labelSubProcess";
            this.labelSubProcess.Size = new System.Drawing.Size(95, 23);
            this.labelSubProcess.TabIndex = 97;
            this.labelSubProcess.Text = "Sub Process";
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(27, 137);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(95, 23);
            this.labelM.TabIndex = 98;
            this.labelM.Text = "M";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(125, 48);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(143, 23);
            this.txtSPNo.TabIndex = 2;
            // 
            // dateBundleCDate
            // 
            this.dateBundleCDate.IsRequired = false;
            this.dateBundleCDate.Location = new System.Drawing.Point(125, 77);
            this.dateBundleCDate.Name = "dateBundleCDate";
            this.dateBundleCDate.Size = new System.Drawing.Size(280, 23);
            this.dateBundleCDate.TabIndex = 3;
            // 
            // comboSubProcess
            // 
            this.comboSubProcess.BackColor = System.Drawing.Color.White;
            this.comboSubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSubProcess.FormattingEnabled = true;
            this.comboSubProcess.IsSupportUnselect = true;
            this.comboSubProcess.Location = new System.Drawing.Point(125, 106);
            this.comboSubProcess.Name = "comboSubProcess";
            this.comboSubProcess.Size = new System.Drawing.Size(121, 24);
            this.comboSubProcess.TabIndex = 4;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(125, 136);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(252, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 17);
            this.label6.TabIndex = 106;
            this.label6.Text = "~";
            // 
            // txtCutRefStart
            // 
            this.txtCutRefStart.BackColor = System.Drawing.Color.White;
            this.txtCutRefStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefStart.Location = new System.Drawing.Point(125, 19);
            this.txtCutRefStart.Name = "txtCutRefStart";
            this.txtCutRefStart.Size = new System.Drawing.Size(121, 23);
            this.txtCutRefStart.TabIndex = 0;
            // 
            // txtCutRefEnd
            // 
            this.txtCutRefEnd.BackColor = System.Drawing.Color.White;
            this.txtCutRefEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefEnd.Location = new System.Drawing.Point(274, 19);
            this.txtCutRefEnd.Name = "txtCutRefEnd";
            this.txtCutRefEnd.Size = new System.Drawing.Size(131, 23);
            this.txtCutRefEnd.TabIndex = 1;
            // 
            // R41
            // 
            this.ClientSize = new System.Drawing.Size(517, 205);
            this.Controls.Add(this.txtCutRefEnd);
            this.Controls.Add(this.txtCutRefStart);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.comboSubProcess);
            this.Controls.Add(this.dateBundleCDate);
            this.Controls.Add(this.txtSPNo);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelSubProcess);
            this.Controls.Add(this.labelBundleCDate);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.labelCutRefNo);
            this.DefaultControl = "textCutRef_Start";
            this.DefaultControlForEdit = "textCutRef_Start";
            this.Name = "R41";
            this.Text = "R41.Bundle tracking list (RFID)";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelCutRefNo, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelBundleCDate, 0);
            this.Controls.SetChildIndex(this.labelSubProcess, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.Controls.SetChildIndex(this.dateBundleCDate, 0);
            this.Controls.SetChildIndex(this.comboSubProcess, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtCutRefStart, 0);
            this.Controls.SetChildIndex(this.txtCutRefEnd, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelCutRefNo;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelBundleCDate;
        private Win.UI.Label labelSubProcess;
        private Win.UI.Label labelM;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.DateRange dateBundleCDate;
        private Win.UI.ComboBox comboSubProcess;
        private Win.UI.ComboBox comboM;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCutRefStart;
        private System.Windows.Forms.TextBox txtCutRefEnd;

    }
}
