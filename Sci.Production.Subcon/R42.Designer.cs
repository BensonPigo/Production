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
            this.components = new System.ComponentModel.Container();
            this.comboM = new Sci.Win.UI.ComboBox();
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
            this.labelFactory = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Production.Class.ComboFactory(this.components);
            this.txtsubprocess = new Sci.Production.Class.Txtsubprocess();
            this.comboRFIDProcessLocation = new Sci.Production.Class.ComboRFIDProcessLocation();
            this.label1 = new Sci.Win.UI.Label();
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
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 6;
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
            this.labelM.Location = new System.Drawing.Point(22, 159);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(120, 23);
            this.labelM.TabIndex = 108;
            this.labelM.Text = "M";
            // 
            // labelSubProcess
            // 
            this.labelSubProcess.Location = new System.Drawing.Point(22, 129);
            this.labelSubProcess.Name = "labelSubProcess";
            this.labelSubProcess.Size = new System.Drawing.Size(120, 23);
            this.labelSubProcess.TabIndex = 107;
            this.labelSubProcess.Text = "Sub Process";
            // 
            // labelBundleCDate
            // 
            this.labelBundleCDate.Location = new System.Drawing.Point(22, 70);
            this.labelBundleCDate.Name = "labelBundleCDate";
            this.labelBundleCDate.Size = new System.Drawing.Size(120, 23);
            this.labelBundleCDate.TabIndex = 106;
            this.labelBundleCDate.Text = "Bundle CDate";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(22, 41);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(120, 23);
            this.labelSPNo.TabIndex = 105;
            this.labelSPNo.Text = "SP#";
            // 
            // labelCutRefNo
            // 
            this.labelCutRefNo.Location = new System.Drawing.Point(22, 12);
            this.labelCutRefNo.Name = "labelCutRefNo";
            this.labelCutRefNo.Size = new System.Drawing.Size(120, 23);
            this.labelCutRefNo.TabIndex = 104;
            this.labelCutRefNo.Text = "Cut Ref#";
            // 
            // dateBundleTransDate
            // 
            // 
            // 
            // 
            this.dateBundleTransDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBundleTransDate.DateBox1.Name = "";
            this.dateBundleTransDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBundleTransDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBundleTransDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBundleTransDate.DateBox2.Name = "";
            this.dateBundleTransDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBundleTransDate.DateBox2.TabIndex = 1;
            this.dateBundleTransDate.IsRequired = false;
            this.dateBundleTransDate.Location = new System.Drawing.Point(145, 99);
            this.dateBundleTransDate.Name = "dateBundleTransDate";
            this.dateBundleTransDate.Size = new System.Drawing.Size(280, 23);
            this.dateBundleTransDate.TabIndex = 4;
            // 
            // labelBundleTransDate
            // 
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
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(22, 188);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(120, 23);
            this.labelFactory.TabIndex = 108;
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
            this.comboFactory.Location = new System.Drawing.Point(145, 188);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(80, 24);
            this.comboFactory.TabIndex = 118;
            // 
            // txtsubprocess
            // 
            this.txtsubprocess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtsubprocess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtsubprocess.IsSupportEditMode = false;
            this.txtsubprocess.Location = new System.Drawing.Point(145, 129);
            this.txtsubprocess.Name = "txtsubprocess";
            this.txtsubprocess.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtsubprocess.ReadOnly = true;
            this.txtsubprocess.Size = new System.Drawing.Size(280, 23);
            this.txtsubprocess.TabIndex = 119;
            // 
            // comboRFIDProcessLocation
            // 
            this.comboRFIDProcessLocation.BackColor = System.Drawing.Color.White;
            this.comboRFIDProcessLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboRFIDProcessLocation.FormattingEnabled = true;
            this.comboRFIDProcessLocation.IncludeJunk = true;
            this.comboRFIDProcessLocation.IsSupportUnselect = true;
            this.comboRFIDProcessLocation.Location = new System.Drawing.Point(145, 216);
            this.comboRFIDProcessLocation.Name = "comboRFIDProcessLocation";
            this.comboRFIDProcessLocation.OldText = "";
            this.comboRFIDProcessLocation.Size = new System.Drawing.Size(121, 24);
            this.comboRFIDProcessLocation.TabIndex = 121;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(22, 217);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 23);
            this.label1.TabIndex = 120;
            this.label1.Text = "Process Location";
            // 
            // R42
            // 
            this.ClientSize = new System.Drawing.Size(599, 265);
            this.Controls.Add(this.comboRFIDProcessLocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtsubprocess);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.txtCutRefEnd);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtCutRefStart);
            this.Controls.Add(this.dateBundleTransDate);
            this.Controls.Add(this.labelBundleTransDate);
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
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.Controls.SetChildIndex(this.dateBundleCDate, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.labelBundleTransDate, 0);
            this.Controls.SetChildIndex(this.dateBundleTransDate, 0);
            this.Controls.SetChildIndex(this.txtCutRefStart, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.txtCutRefEnd, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.txtsubprocess, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.comboRFIDProcessLocation, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.ComboBox comboM;
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
        private Win.UI.Label labelFactory;
        private Class.ComboFactory comboFactory;
        private Class.Txtsubprocess txtsubprocess;
        private Class.ComboRFIDProcessLocation comboRFIDProcessLocation;
        private Win.UI.Label label1;
    }
}
