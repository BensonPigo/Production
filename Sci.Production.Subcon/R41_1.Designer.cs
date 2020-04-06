namespace Sci.Production.Subcon
{
    partial class R41_1
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
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelBundleCDate = new Sci.Win.UI.Label();
            this.labelSubProcess = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.dateBundleCDate = new Sci.Win.UI.DateRange();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.lbBundleScanDate = new Sci.Win.UI.Label();
            this.dateBundleScanDate = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.dateBDelivery = new Sci.Win.UI.DateRange();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.comboRFIDProcessLocation = new Sci.Production.Class.comboRFIDProcessLocation();
            this.txtsubprocess = new Sci.Production.Class.txtsubprocess();
            this.comboFactory = new Sci.Production.Class.comboFactory(this.components);
            this.dateSewInLine = new Sci.Win.UI.DateRange();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(471, 12);
            this.print.TabIndex = 12;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(471, 48);
            this.toexcel.TabIndex = 13;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(471, 84);
            this.close.TabIndex = 14;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(19, 16);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(132, 23);
            this.labelSPNo.TabIndex = 95;
            this.labelSPNo.Text = "SP#";
            // 
            // labelBundleCDate
            // 
            this.labelBundleCDate.Location = new System.Drawing.Point(19, 45);
            this.labelBundleCDate.Name = "labelBundleCDate";
            this.labelBundleCDate.Size = new System.Drawing.Size(132, 23);
            this.labelBundleCDate.TabIndex = 96;
            this.labelBundleCDate.Text = "Bundle CDate";
            // 
            // labelSubProcess
            // 
            this.labelSubProcess.Location = new System.Drawing.Point(19, 161);
            this.labelSubProcess.Name = "labelSubProcess";
            this.labelSubProcess.Size = new System.Drawing.Size(132, 23);
            this.labelSubProcess.TabIndex = 97;
            this.labelSubProcess.Text = "Sub Process";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(19, 190);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(132, 23);
            this.labelM.TabIndex = 98;
            this.labelM.Text = "M";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(154, 16);
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
            this.dateBundleCDate.Location = new System.Drawing.Point(154, 45);
            this.dateBundleCDate.Name = "dateBundleCDate";
            this.dateBundleCDate.Size = new System.Drawing.Size(280, 23);
            this.dateBundleCDate.TabIndex = 4;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(154, 190);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 9;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(19, 219);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(132, 23);
            this.labelFactory.TabIndex = 98;
            this.labelFactory.Text = "Factory";
            // 
            // lbBundleScanDate
            // 
            this.lbBundleScanDate.Location = new System.Drawing.Point(19, 74);
            this.lbBundleScanDate.Name = "lbBundleScanDate";
            this.lbBundleScanDate.Size = new System.Drawing.Size(132, 23);
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
            this.dateBundleScanDate.Location = new System.Drawing.Point(154, 74);
            this.dateBundleScanDate.Name = "dateBundleScanDate";
            this.dateBundleScanDate.Size = new System.Drawing.Size(280, 23);
            this.dateBundleScanDate.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 246);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 23);
            this.label1.TabIndex = 113;
            this.label1.Text = "Process Location";
            // 
            // dateBDelivery
            // 
            // 
            // 
            // 
            this.dateBDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBDelivery.DateBox1.Name = "";
            this.dateBDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBDelivery.DateBox2.Name = "";
            this.dateBDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBDelivery.DateBox2.TabIndex = 1;
            this.dateBDelivery.IsRequired = false;
            this.dateBDelivery.Location = new System.Drawing.Point(154, 103);
            this.dateBDelivery.Name = "dateBDelivery";
            this.dateBDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBDelivery.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(19, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 23);
            this.label2.TabIndex = 115;
            this.label2.Text = "Buyer Delivery Date";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(19, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 23);
            this.label3.TabIndex = 118;
            this.label3.Text = "Sewing Inline";
            // 
            // comboRFIDProcessLocation
            // 
            this.comboRFIDProcessLocation.BackColor = System.Drawing.Color.White;
            this.comboRFIDProcessLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboRFIDProcessLocation.FormattingEnabled = true;
            this.comboRFIDProcessLocation.IncludeJunk = true;
            this.comboRFIDProcessLocation.IsSupportUnselect = true;
            this.comboRFIDProcessLocation.Location = new System.Drawing.Point(154, 245);
            this.comboRFIDProcessLocation.Name = "comboRFIDProcessLocation";
            this.comboRFIDProcessLocation.OldText = "";
            this.comboRFIDProcessLocation.Size = new System.Drawing.Size(121, 24);
            this.comboRFIDProcessLocation.TabIndex = 11;
            // 
            // txtsubprocess
            // 
            this.txtsubprocess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtsubprocess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtsubprocess.IsSupportEditMode = false;
            this.txtsubprocess.Location = new System.Drawing.Point(154, 161);
            this.txtsubprocess.Name = "txtsubprocess";
            this.txtsubprocess.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtsubprocess.ReadOnly = true;
            this.txtsubprocess.Size = new System.Drawing.Size(280, 23);
            this.txtsubprocess.TabIndex = 8;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.FilteMDivision = false;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IssupportJunk = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(154, 219);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(80, 24);
            this.comboFactory.TabIndex = 10;
            // 
            // dateSewInLine
            // 
            // 
            // 
            // 
            this.dateSewInLine.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSewInLine.DateBox1.Name = "";
            this.dateSewInLine.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSewInLine.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSewInLine.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSewInLine.DateBox2.Name = "";
            this.dateSewInLine.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSewInLine.DateBox2.TabIndex = 1;
            this.dateSewInLine.IsRequired = false;
            this.dateSewInLine.Location = new System.Drawing.Point(154, 132);
            this.dateSewInLine.Name = "dateSewInLine";
            this.dateSewInLine.Size = new System.Drawing.Size(280, 23);
            this.dateSewInLine.TabIndex = 7;
            // 
            // R41_1
            // 
            this.ClientSize = new System.Drawing.Size(563, 309);
            this.Controls.Add(this.dateSewInLine);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateBDelivery);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboRFIDProcessLocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateBundleScanDate);
            this.Controls.Add(this.lbBundleScanDate);
            this.Controls.Add(this.txtsubprocess);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.dateBundleCDate);
            this.Controls.Add(this.txtSPNo);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelSubProcess);
            this.Controls.Add(this.labelBundleCDate);
            this.Controls.Add(this.labelSPNo);
            this.DefaultControl = "txtCutRefStart";
            this.DefaultControlForEdit = "txtCutRefStart";
            this.Name = "R41_1";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R41_1.Bundle tracking list (RFID)";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelBundleCDate, 0);
            this.Controls.SetChildIndex(this.labelSubProcess, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.Controls.SetChildIndex(this.dateBundleCDate, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.txtsubprocess, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.lbBundleScanDate, 0);
            this.Controls.SetChildIndex(this.dateBundleScanDate, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.comboRFIDProcessLocation, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.dateBDelivery, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.dateSewInLine, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelBundleCDate;
        private Win.UI.Label labelSubProcess;
        private Win.UI.Label labelM;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.DateRange dateBundleCDate;
        private Win.UI.ComboBox comboM;
        private Win.UI.Label labelFactory;
        private Class.comboFactory comboFactory;
        private Class.txtsubprocess txtsubprocess;
        private Win.UI.Label lbBundleScanDate;
        private Win.UI.DateRange dateBundleScanDate;
        private Win.UI.Label label1;
        private Class.comboRFIDProcessLocation comboRFIDProcessLocation;
        private Win.UI.DateRange dateBDelivery;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.DateRange dateSewInLine;
    }
}
