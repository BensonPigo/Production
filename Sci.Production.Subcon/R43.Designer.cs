namespace Sci.Production.Subcon
{
    partial class R43
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
            this.comboSubProcess = new Sci.Win.UI.ComboBox();
            this.labelM = new Sci.Win.UI.Label();
            this.labelSubProcess = new Sci.Win.UI.Label();
            this.labelBundleReceiveDate = new Sci.Win.UI.Label();
            this.dateBundleReceiveDate = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Production.Class.ComboFactory(this.components);
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(473, 12);
            this.print.TabIndex = 3;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(473, 48);
            this.toexcel.TabIndex = 4;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(473, 84);
            this.close.TabIndex = 5;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(169, 78);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 2;
            // 
            // comboSubProcess
            // 
            this.comboSubProcess.BackColor = System.Drawing.Color.White;
            this.comboSubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSubProcess.FormattingEnabled = true;
            this.comboSubProcess.IsSupportUnselect = true;
            this.comboSubProcess.Location = new System.Drawing.Point(169, 48);
            this.comboSubProcess.Name = "comboSubProcess";
            this.comboSubProcess.Size = new System.Drawing.Size(121, 24);
            this.comboSubProcess.TabIndex = 1;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(32, 79);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(134, 23);
            this.labelM.TabIndex = 115;
            this.labelM.Text = "M";
            // 
            // labelSubProcess
            // 
            this.labelSubProcess.Location = new System.Drawing.Point(32, 49);
            this.labelSubProcess.Name = "labelSubProcess";
            this.labelSubProcess.Size = new System.Drawing.Size(134, 23);
            this.labelSubProcess.TabIndex = 114;
            this.labelSubProcess.Text = "Sub Process";
            // 
            // labelBundleReceiveDate
            // 
            this.labelBundleReceiveDate.Location = new System.Drawing.Point(32, 19);
            this.labelBundleReceiveDate.Name = "labelBundleReceiveDate";
            this.labelBundleReceiveDate.Size = new System.Drawing.Size(134, 23);
            this.labelBundleReceiveDate.TabIndex = 118;
            this.labelBundleReceiveDate.Text = "Bundle Receive Date";
            // 
            // dateBundleReceiveDate
            // 
            this.dateBundleReceiveDate.IsRequired = false;
            this.dateBundleReceiveDate.Location = new System.Drawing.Point(169, 19);
            this.dateBundleReceiveDate.Name = "dateBundleReceiveDate";
            this.dateBundleReceiveDate.Size = new System.Drawing.Size(280, 23);
            this.dateBundleReceiveDate.TabIndex = 0;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(32, 109);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(134, 23);
            this.labelFactory.TabIndex = 115;
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
            this.comboFactory.Location = new System.Drawing.Point(169, 109);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(80, 24);
            this.comboFactory.TabIndex = 119;
            // 
            // R43
            // 
            this.ClientSize = new System.Drawing.Size(565, 160);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.dateBundleReceiveDate);
            this.Controls.Add(this.labelBundleReceiveDate);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.comboSubProcess);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelSubProcess);
            this.DefaultControl = "dateBundleReceiveDate";
            this.DefaultControlForEdit = "dateBundleReceiveDate";
            this.Name = "R43";
            this.Text = "R43.Sub-process BCS report (RFID)";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelSubProcess, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.comboSubProcess, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.labelBundleReceiveDate, 0);
            this.Controls.SetChildIndex(this.dateBundleReceiveDate, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboSubProcess;
        private Win.UI.Label labelM;
        private Win.UI.Label labelSubProcess;
        private Win.UI.Label labelBundleReceiveDate;
        private Win.UI.DateRange dateBundleReceiveDate;
        private Win.UI.Label labelFactory;
        private Class.ComboFactory comboFactory;
    }
}
