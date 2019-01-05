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
            this.components = new System.ComponentModel.Container();
            this.labelCutRefNo = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelBundleCDate = new Sci.Win.UI.Label();
            this.labelSubProcess = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.dateBundleCDate = new Sci.Win.UI.DateRange();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCutRefStart = new System.Windows.Forms.TextBox();
            this.txtCutRefEnd = new System.Windows.Forms.TextBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Production.Class.comboFactory(this.components);
            this.txtsubprocess = new Sci.Production.Class.txtsubprocess();
            this.lbBundleScanDate = new Sci.Win.UI.Label();
            this.dateBundleScanDate = new Sci.Win.UI.DateRange();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(440, 12);
            this.print.TabIndex = 6;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(440, 48);
            this.toexcel.TabIndex = 7;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(440, 84);
            this.close.TabIndex = 8;
            // 
            // labelCutRefNo
            // 
            this.labelCutRefNo.Location = new System.Drawing.Point(27, 19);
            this.labelCutRefNo.Name = "labelCutRefNo";
            this.labelCutRefNo.Size = new System.Drawing.Size(116, 23);
            this.labelCutRefNo.TabIndex = 94;
            this.labelCutRefNo.Text = "Cut Ref#";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(27, 48);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(116, 23);
            this.labelSPNo.TabIndex = 95;
            this.labelSPNo.Text = "SP#";
            // 
            // labelBundleCDate
            // 
            this.labelBundleCDate.Location = new System.Drawing.Point(27, 77);
            this.labelBundleCDate.Name = "labelBundleCDate";
            this.labelBundleCDate.Size = new System.Drawing.Size(116, 23);
            this.labelBundleCDate.TabIndex = 96;
            this.labelBundleCDate.Text = "Bundle CDate";
            // 
            // labelSubProcess
            // 
            this.labelSubProcess.Location = new System.Drawing.Point(27, 135);
            this.labelSubProcess.Name = "labelSubProcess";
            this.labelSubProcess.Size = new System.Drawing.Size(116, 23);
            this.labelSubProcess.TabIndex = 97;
            this.labelSubProcess.Text = "Sub Process";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(27, 164);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(116, 23);
            this.labelM.TabIndex = 98;
            this.labelM.Text = "M";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(146, 48);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(143, 23);
            this.txtSPNo.TabIndex = 2;
            // 
            // dateBundleCDate
            // 
            // 
            // 
            // 
            this.dateBundleCDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBundleCDate.DateBox1.Name = "";
            this.dateBundleCDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBundleCDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBundleCDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBundleCDate.DateBox2.Name = "";
            this.dateBundleCDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBundleCDate.DateBox2.TabIndex = 1;
            this.dateBundleCDate.IsRequired = false;
            this.dateBundleCDate.Location = new System.Drawing.Point(146, 77);
            this.dateBundleCDate.Name = "dateBundleCDate";
            this.dateBundleCDate.Size = new System.Drawing.Size(280, 23);
            this.dateBundleCDate.TabIndex = 3;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(146, 164);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(273, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 17);
            this.label6.TabIndex = 106;
            this.label6.Text = "~";
            // 
            // txtCutRefStart
            // 
            this.txtCutRefStart.BackColor = System.Drawing.Color.White;
            this.txtCutRefStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefStart.Location = new System.Drawing.Point(146, 19);
            this.txtCutRefStart.Name = "txtCutRefStart";
            this.txtCutRefStart.Size = new System.Drawing.Size(121, 23);
            this.txtCutRefStart.TabIndex = 0;
            // 
            // txtCutRefEnd
            // 
            this.txtCutRefEnd.BackColor = System.Drawing.Color.White;
            this.txtCutRefEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefEnd.Location = new System.Drawing.Point(295, 19);
            this.txtCutRefEnd.Name = "txtCutRefEnd";
            this.txtCutRefEnd.Size = new System.Drawing.Size(131, 23);
            this.txtCutRefEnd.TabIndex = 1;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(27, 193);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(116, 23);
            this.labelFactory.TabIndex = 98;
            this.labelFactory.Text = "Factory";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.FilteMDivision = false;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IssupportJunk = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(146, 194);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(80, 24);
            this.comboFactory.TabIndex = 107;
            // 
            // txtsubprocess
            // 
            this.txtsubprocess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtsubprocess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtsubprocess.IsSupportEditMode = false;
            this.txtsubprocess.Location = new System.Drawing.Point(146, 135);
            this.txtsubprocess.Name = "txtsubprocess";
            this.txtsubprocess.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtsubprocess.ReadOnly = true;
            this.txtsubprocess.Size = new System.Drawing.Size(280, 23);
            this.txtsubprocess.TabIndex = 108;
            // 
            // lbBundleScanDate
            // 
            this.lbBundleScanDate.Location = new System.Drawing.Point(27, 106);
            this.lbBundleScanDate.Name = "lbBundleScanDate";
            this.lbBundleScanDate.Size = new System.Drawing.Size(116, 23);
            this.lbBundleScanDate.TabIndex = 109;
            this.lbBundleScanDate.Text = "Bundle Scan Date";
            // 
            // dateBundleScanDate
            // 
            // 
            // 
            // 
            this.dateBundleScanDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBundleScanDate.DateBox1.Name = "";
            this.dateBundleScanDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBundleScanDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBundleScanDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBundleScanDate.DateBox2.Name = "";
            this.dateBundleScanDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBundleScanDate.DateBox2.TabIndex = 1;
            this.dateBundleScanDate.IsRequired = false;
            this.dateBundleScanDate.Location = new System.Drawing.Point(146, 106);
            this.dateBundleScanDate.Name = "dateBundleScanDate";
            this.dateBundleScanDate.Size = new System.Drawing.Size(280, 23);
            this.dateBundleScanDate.TabIndex = 110;
            // 
            // R41
            // 
            this.ClientSize = new System.Drawing.Size(532, 246);
            this.Controls.Add(this.dateBundleScanDate);
            this.Controls.Add(this.lbBundleScanDate);
            this.Controls.Add(this.txtsubprocess);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.txtCutRefEnd);
            this.Controls.Add(this.txtCutRefStart);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.dateBundleCDate);
            this.Controls.Add(this.txtSPNo);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelSubProcess);
            this.Controls.Add(this.labelBundleCDate);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.labelCutRefNo);
            this.DefaultControl = "txtCutRefStart";
            this.DefaultControlForEdit = "txtCutRefStart";
            this.Name = "R41";
            this.Text = "R41.Bundle tracking list (RFID)";
            this.Controls.SetChildIndex(this.labelCutRefNo, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelBundleCDate, 0);
            this.Controls.SetChildIndex(this.labelSubProcess, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.Controls.SetChildIndex(this.dateBundleCDate, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtCutRefStart, 0);
            this.Controls.SetChildIndex(this.txtCutRefEnd, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.txtsubprocess, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.lbBundleScanDate, 0);
            this.Controls.SetChildIndex(this.dateBundleScanDate, 0);
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
        private Win.UI.ComboBox comboM;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCutRefStart;
        private System.Windows.Forms.TextBox txtCutRefEnd;
        private Win.UI.Label labelFactory;
        private Class.comboFactory comboFactory;
        private Class.txtsubprocess txtsubprocess;
        private Win.UI.Label lbBundleScanDate;
        private Win.UI.DateRange dateBundleScanDate;
    }
}
