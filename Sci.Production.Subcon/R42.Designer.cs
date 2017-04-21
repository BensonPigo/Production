namespace Sci.Production.Subcon
{
    partial class R42
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
            this.comboM = new Sci.Win.UI.ComboBox();
            this.comboSubProcess = new Sci.Win.UI.ComboBox();
            this.dateBundleCDate = new Sci.Win.UI.DateRange();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelM = new Sci.Win.UI.Label();
            this.labelSubProcess = new Sci.Win.UI.Label();
            this.labelBundleCDate = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelCutRefNo = new Sci.Win.UI.Label();
            this.dateBundleTransDate = new Sci.Win.UI.DateRange();
            this.labelBundleTransDate = new Sci.Win.UI.Label();
            this.txtCutRefStart = new Sci.Win.UI.TextBox();
            this.label7 = new Sci.Win.UI.Label();
            this.txtCutRefEnd = new Sci.Win.UI.TextBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(507, 12);
            this.print.TabIndex = 7;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(507, 48);
            this.toexcel.TabIndex = 8;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(507, 84);
            this.close.TabIndex = 9;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(145, 158);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 6;
            // 
            // comboSubProcess
            // 
            this.comboSubProcess.BackColor = System.Drawing.Color.White;
            this.comboSubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSubProcess.FormattingEnabled = true;
            this.comboSubProcess.IsSupportUnselect = true;
            this.comboSubProcess.Location = new System.Drawing.Point(145, 128);
            this.comboSubProcess.Name = "comboSubProcess";
            this.comboSubProcess.Size = new System.Drawing.Size(121, 24);
            this.comboSubProcess.TabIndex = 5;
            // 
            // dateBundleCDate
            // 
            this.dateBundleCDate.IsRequired = false;
            this.dateBundleCDate.Location = new System.Drawing.Point(145, 70);
            this.dateBundleCDate.Name = "dateBundleCDate";
            this.dateBundleCDate.Size = new System.Drawing.Size(280, 23);
            this.dateBundleCDate.TabIndex = 3;
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(145, 41);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(143, 23);
            this.txtSPNo.TabIndex = 2;
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(22, 159);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(120, 23);
            this.labelM.TabIndex = 108;
            this.labelM.Text = "M";
            // 
            // labelSubProcess
            // 
            this.labelSubProcess.Lines = 0;
            this.labelSubProcess.Location = new System.Drawing.Point(22, 129);
            this.labelSubProcess.Name = "labelSubProcess";
            this.labelSubProcess.Size = new System.Drawing.Size(120, 23);
            this.labelSubProcess.TabIndex = 107;
            this.labelSubProcess.Text = "Sub Process";
            // 
            // labelBundleCDate
            // 
            this.labelBundleCDate.Lines = 0;
            this.labelBundleCDate.Location = new System.Drawing.Point(22, 70);
            this.labelBundleCDate.Name = "labelBundleCDate";
            this.labelBundleCDate.Size = new System.Drawing.Size(120, 23);
            this.labelBundleCDate.TabIndex = 106;
            this.labelBundleCDate.Text = "Bundle CDate";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(22, 41);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(120, 23);
            this.labelSPNo.TabIndex = 105;
            this.labelSPNo.Text = "SP#";
            // 
            // labelCutRefNo
            // 
            this.labelCutRefNo.Lines = 0;
            this.labelCutRefNo.Location = new System.Drawing.Point(22, 12);
            this.labelCutRefNo.Name = "labelCutRefNo";
            this.labelCutRefNo.Size = new System.Drawing.Size(120, 23);
            this.labelCutRefNo.TabIndex = 104;
            this.labelCutRefNo.Text = "Cut Ref#";
            // 
            // dateBundleTransDate
            // 
            this.dateBundleTransDate.IsRequired = false;
            this.dateBundleTransDate.Location = new System.Drawing.Point(145, 99);
            this.dateBundleTransDate.Name = "dateBundleTransDate";
            this.dateBundleTransDate.Size = new System.Drawing.Size(280, 23);
            this.dateBundleTransDate.TabIndex = 4;
            // 
            // labelBundleTransDate
            // 
            this.labelBundleTransDate.Lines = 0;
            this.labelBundleTransDate.Location = new System.Drawing.Point(22, 99);
            this.labelBundleTransDate.Name = "labelBundleTransDate";
            this.labelBundleTransDate.Size = new System.Drawing.Size(120, 23);
            this.labelBundleTransDate.TabIndex = 114;
            this.labelBundleTransDate.Text = "Bundle Trans Date";
            // 
            // txtCutRefStart
            // 
            this.txtCutRefStart.BackColor = System.Drawing.Color.White;
            this.txtCutRefStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefStart.Location = new System.Drawing.Point(145, 12);
            this.txtCutRefStart.Name = "txtCutRefStart";
            this.txtCutRefStart.Size = new System.Drawing.Size(116, 23);
            this.txtCutRefStart.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(264, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 23);
            this.label7.TabIndex = 117;
            this.label7.Text = "～";
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtCutRefEnd
            // 
            this.txtCutRefEnd.BackColor = System.Drawing.Color.White;
            this.txtCutRefEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefEnd.Location = new System.Drawing.Point(291, 12);
            this.txtCutRefEnd.Name = "txtCutRefEnd";
            this.txtCutRefEnd.Size = new System.Drawing.Size(116, 23);
            this.txtCutRefEnd.TabIndex = 1;
            // 
            // R42
            // 
            this.ClientSize = new System.Drawing.Size(599, 240);
            this.Controls.Add(this.txtCutRefEnd);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtCutRefStart);
            this.Controls.Add(this.dateBundleTransDate);
            this.Controls.Add(this.labelBundleTransDate);
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
            this.Name = "R42";
            this.Text = "R42. Bundle Transaction detail (RFID)";
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
            this.Controls.SetChildIndex(this.labelBundleTransDate, 0);
            this.Controls.SetChildIndex(this.dateBundleTransDate, 0);
            this.Controls.SetChildIndex(this.txtCutRefStart, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.txtCutRefEnd, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboSubProcess;
        private Win.UI.DateRange dateBundleCDate;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelM;
        private Win.UI.Label labelSubProcess;
        private Win.UI.Label labelBundleCDate;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelCutRefNo;
        private Win.UI.DateRange dateBundleTransDate;
        private Win.UI.Label labelBundleTransDate;
        private Win.UI.TextBox txtCutRefStart;
        private Win.UI.Label label7;
        private Win.UI.TextBox txtCutRefEnd;
    }
}
