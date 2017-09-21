namespace RFID_Monitor_Board_Setup
{
    partial class RFID_Monitor_Board_Setup
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lbMorningShift = new Sci.Win.UI.Label();
            this.lbNightShift = new Sci.Win.UI.Label();
            this.lbUpdateintervalperpage = new Sci.Win.UI.Label();
            this.lbAllupdateinterval = new Sci.Win.UI.Label();
            this.numericBoxUpdateintervalperpage = new Sci.Win.UI.NumericBox();
            this.numericBoxAllupdateinterval = new Sci.Win.UI.NumericBox();
            this.lbsecond = new System.Windows.Forms.Label();
            this.lbsecond2 = new System.Windows.Forms.Label();
            this.txtMorningShiftStart = new Sci.Win.UI.TextBox();
            this.txtMorningShiftEnd = new Sci.Win.UI.TextBox();
            this.txtNightShiftStart = new Sci.Win.UI.TextBox();
            this.txtNightShiftEnd = new Sci.Win.UI.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbFactory = new Sci.Win.UI.Label();
            this.lbSubProcess = new Sci.Win.UI.Label();
            this.txtFactory = new Sci.Win.UI.TextBox();
            this.txtSubProcess = new Sci.Win.UI.TextBox();
            this.lbWIPRange = new Sci.Win.UI.Label();
            this.numWIPRange = new Sci.Win.UI.NumericBox();
            this.lbday = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.SuspendLayout();
            // 
            // lbMorningShift
            // 
            this.lbMorningShift.Location = new System.Drawing.Point(9, 43);
            this.lbMorningShift.Name = "lbMorningShift";
            this.lbMorningShift.Size = new System.Drawing.Size(89, 23);
            this.lbMorningShift.TabIndex = 1;
            this.lbMorningShift.Text = "Morning Shift";
            // 
            // lbNightShift
            // 
            this.lbNightShift.Location = new System.Drawing.Point(9, 72);
            this.lbNightShift.Name = "lbNightShift";
            this.lbNightShift.Size = new System.Drawing.Size(89, 23);
            this.lbNightShift.TabIndex = 2;
            this.lbNightShift.Text = "Night Shift";
            // 
            // lbUpdateintervalperpage
            // 
            this.lbUpdateintervalperpage.Location = new System.Drawing.Point(9, 101);
            this.lbUpdateintervalperpage.Name = "lbUpdateintervalperpage";
            this.lbUpdateintervalperpage.Size = new System.Drawing.Size(157, 23);
            this.lbUpdateintervalperpage.TabIndex = 3;
            this.lbUpdateintervalperpage.Text = "Update interval per page";
            // 
            // lbAllupdateinterval
            // 
            this.lbAllupdateinterval.Location = new System.Drawing.Point(9, 130);
            this.lbAllupdateinterval.Name = "lbAllupdateinterval";
            this.lbAllupdateinterval.Size = new System.Drawing.Size(157, 23);
            this.lbAllupdateinterval.TabIndex = 4;
            this.lbAllupdateinterval.Text = "All update interval";
            // 
            // numericBoxUpdateintervalperpage
            // 
            this.numericBoxUpdateintervalperpage.BackColor = System.Drawing.Color.White;
            this.numericBoxUpdateintervalperpage.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IntervalPerPage", true));
            this.numericBoxUpdateintervalperpage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBoxUpdateintervalperpage.Location = new System.Drawing.Point(169, 101);
            this.numericBoxUpdateintervalperpage.Name = "numericBoxUpdateintervalperpage";
            this.numericBoxUpdateintervalperpage.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBoxUpdateintervalperpage.Size = new System.Drawing.Size(68, 23);
            this.numericBoxUpdateintervalperpage.TabIndex = 5;
            this.numericBoxUpdateintervalperpage.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numericBoxAllupdateinterval
            // 
            this.numericBoxAllupdateinterval.BackColor = System.Drawing.Color.White;
            this.numericBoxAllupdateinterval.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "InteralAll", true));
            this.numericBoxAllupdateinterval.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBoxAllupdateinterval.Location = new System.Drawing.Point(169, 130);
            this.numericBoxAllupdateinterval.Name = "numericBoxAllupdateinterval";
            this.numericBoxAllupdateinterval.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBoxAllupdateinterval.Size = new System.Drawing.Size(68, 23);
            this.numericBoxAllupdateinterval.TabIndex = 6;
            this.numericBoxAllupdateinterval.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // lbsecond
            // 
            this.lbsecond.AutoSize = true;
            this.lbsecond.Location = new System.Drawing.Point(243, 107);
            this.lbsecond.Name = "lbsecond";
            this.lbsecond.Size = new System.Drawing.Size(54, 17);
            this.lbsecond.TabIndex = 10;
            this.lbsecond.Text = "second";
            // 
            // lbsecond2
            // 
            this.lbsecond2.AutoSize = true;
            this.lbsecond2.Location = new System.Drawing.Point(243, 136);
            this.lbsecond2.Name = "lbsecond2";
            this.lbsecond2.Size = new System.Drawing.Size(54, 17);
            this.lbsecond2.TabIndex = 11;
            this.lbsecond2.Text = "second";
            // 
            // txtMorningShiftStart
            // 
            this.txtMorningShiftStart.BackColor = System.Drawing.Color.White;
            this.txtMorningShiftStart.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MorningShiftStart", true));
            this.txtMorningShiftStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMorningShiftStart.Location = new System.Drawing.Point(101, 43);
            this.txtMorningShiftStart.Mask = "90:00";
            this.txtMorningShiftStart.Name = "txtMorningShiftStart";
            this.txtMorningShiftStart.Size = new System.Drawing.Size(100, 23);
            this.txtMorningShiftStart.TabIndex = 1;
            this.txtMorningShiftStart.ValidatingType = typeof(System.DateTime);
            // 
            // txtMorningShiftEnd
            // 
            this.txtMorningShiftEnd.BackColor = System.Drawing.Color.White;
            this.txtMorningShiftEnd.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MorningShiftEnd", true));
            this.txtMorningShiftEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMorningShiftEnd.Location = new System.Drawing.Point(229, 43);
            this.txtMorningShiftEnd.Mask = "90:00";
            this.txtMorningShiftEnd.Name = "txtMorningShiftEnd";
            this.txtMorningShiftEnd.Size = new System.Drawing.Size(100, 23);
            this.txtMorningShiftEnd.TabIndex = 2;
            this.txtMorningShiftEnd.ValidatingType = typeof(System.DateTime);
            // 
            // txtNightShiftStart
            // 
            this.txtNightShiftStart.BackColor = System.Drawing.Color.White;
            this.txtNightShiftStart.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "NightShiftStart", true));
            this.txtNightShiftStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtNightShiftStart.Location = new System.Drawing.Point(101, 72);
            this.txtNightShiftStart.Mask = "90:00";
            this.txtNightShiftStart.Name = "txtNightShiftStart";
            this.txtNightShiftStart.Size = new System.Drawing.Size(100, 23);
            this.txtNightShiftStart.TabIndex = 3;
            this.txtNightShiftStart.ValidatingType = typeof(System.DateTime);
            // 
            // txtNightShiftEnd
            // 
            this.txtNightShiftEnd.BackColor = System.Drawing.Color.White;
            this.txtNightShiftEnd.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "NightShiftEnd", true));
            this.txtNightShiftEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtNightShiftEnd.Location = new System.Drawing.Point(229, 72);
            this.txtNightShiftEnd.Mask = "90:00";
            this.txtNightShiftEnd.Name = "txtNightShiftEnd";
            this.txtNightShiftEnd.Size = new System.Drawing.Size(100, 23);
            this.txtNightShiftEnd.TabIndex = 4;
            this.txtNightShiftEnd.ValidatingType = typeof(System.DateTime);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(207, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 17);
            this.label1.TabIndex = 16;
            this.label1.Text = "~";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(207, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 17);
            this.label2.TabIndex = 17;
            this.label2.Text = "~";
            // 
            // lbFactory
            // 
            this.lbFactory.Location = new System.Drawing.Point(9, 159);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(89, 23);
            this.lbFactory.TabIndex = 18;
            this.lbFactory.Text = "Factory";
            // 
            // lbSubProcess
            // 
            this.lbSubProcess.Location = new System.Drawing.Point(9, 188);
            this.lbSubProcess.Name = "lbSubProcess";
            this.lbSubProcess.Size = new System.Drawing.Size(89, 23);
            this.lbSubProcess.TabIndex = 21;
            this.lbSubProcess.Text = "SubProcess";
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FactoryID", true));
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.Location = new System.Drawing.Point(101, 159);
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(100, 23);
            this.txtFactory.TabIndex = 22;
            this.txtFactory.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtFactory_PopUp);
            this.txtFactory.Validating += new System.ComponentModel.CancelEventHandler(this.txtFactory_Validating);
            // 
            // txtSubProcess
            // 
            this.txtSubProcess.BackColor = System.Drawing.Color.White;
            this.txtSubProcess.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SubProcessID", true));
            this.txtSubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSubProcess.Location = new System.Drawing.Point(101, 188);
            this.txtSubProcess.Name = "txtSubProcess";
            this.txtSubProcess.Size = new System.Drawing.Size(136, 23);
            this.txtSubProcess.TabIndex = 23;
            this.txtSubProcess.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtSubProcess_PopUp);
            this.txtSubProcess.Validating += new System.ComponentModel.CancelEventHandler(this.txtSubProcess_Validating);
            // 
            // lbWIPRange
            // 
            this.lbWIPRange.Location = new System.Drawing.Point(9, 217);
            this.lbWIPRange.Name = "lbWIPRange";
            this.lbWIPRange.Size = new System.Drawing.Size(89, 23);
            this.lbWIPRange.TabIndex = 24;
            this.lbWIPRange.Text = "WIP Range";
            // 
            // numWIPRange
            // 
            this.numWIPRange.BackColor = System.Drawing.Color.White;
            this.numWIPRange.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "WIPRange", true));
            this.numWIPRange.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numWIPRange.Location = new System.Drawing.Point(101, 217);
            this.numWIPRange.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWIPRange.Name = "numWIPRange";
            this.numWIPRange.NullValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWIPRange.Size = new System.Drawing.Size(68, 23);
            this.numWIPRange.TabIndex = 25;
            this.numWIPRange.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbday
            // 
            this.lbday.AutoSize = true;
            this.lbday.Location = new System.Drawing.Point(175, 220);
            this.lbday.Name = "lbday";
            this.lbday.Size = new System.Drawing.Size(31, 17);
            this.lbday.TabIndex = 26;
            this.lbday.Text = "day";
            // 
            // RFID_Monitor_Board_Setup
            // 
            this.ClientSize = new System.Drawing.Size(494, 253);
            this.Controls.Add(this.lbday);
            this.Controls.Add(this.numWIPRange);
            this.Controls.Add(this.lbWIPRange);
            this.Controls.Add(this.txtSubProcess);
            this.Controls.Add(this.txtFactory);
            this.Controls.Add(this.lbSubProcess);
            this.Controls.Add(this.lbFactory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNightShiftEnd);
            this.Controls.Add(this.txtNightShiftStart);
            this.Controls.Add(this.txtMorningShiftEnd);
            this.Controls.Add(this.txtMorningShiftStart);
            this.Controls.Add(this.lbsecond2);
            this.Controls.Add(this.lbsecond);
            this.Controls.Add(this.numericBoxAllupdateinterval);
            this.Controls.Add(this.numericBoxUpdateintervalperpage);
            this.Controls.Add(this.lbAllupdateinterval);
            this.Controls.Add(this.lbUpdateintervalperpage);
            this.Controls.Add(this.lbNightShift);
            this.Controls.Add(this.lbMorningShift);
            this.IsSupportClip = false;
            this.IsSupportLocate = false;
            this.IsSupportPrint = false;
            this.Name = "RFID_Monitor_Board_Setup";
            this.Text = "RFID Monitor Board Setup";
            this.WorkAlias = "MonitorBoardParameter";
            this.Controls.SetChildIndex(this.lbMorningShift, 0);
            this.Controls.SetChildIndex(this.lbNightShift, 0);
            this.Controls.SetChildIndex(this.lbUpdateintervalperpage, 0);
            this.Controls.SetChildIndex(this.lbAllupdateinterval, 0);
            this.Controls.SetChildIndex(this.numericBoxUpdateintervalperpage, 0);
            this.Controls.SetChildIndex(this.numericBoxAllupdateinterval, 0);
            this.Controls.SetChildIndex(this.lbsecond, 0);
            this.Controls.SetChildIndex(this.lbsecond2, 0);
            this.Controls.SetChildIndex(this.txtMorningShiftStart, 0);
            this.Controls.SetChildIndex(this.txtMorningShiftEnd, 0);
            this.Controls.SetChildIndex(this.txtNightShiftStart, 0);
            this.Controls.SetChildIndex(this.txtNightShiftEnd, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.lbFactory, 0);
            this.Controls.SetChildIndex(this.lbSubProcess, 0);
            this.Controls.SetChildIndex(this.txtFactory, 0);
            this.Controls.SetChildIndex(this.txtSubProcess, 0);
            this.Controls.SetChildIndex(this.lbWIPRange, 0);
            this.Controls.SetChildIndex(this.numWIPRange, 0);
            this.Controls.SetChildIndex(this.lbday, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sci.Win.UI.Label lbMorningShift;
        private Sci.Win.UI.Label lbNightShift;
        private Sci.Win.UI.Label lbUpdateintervalperpage;
        private Sci.Win.UI.Label lbAllupdateinterval;
        private Sci.Win.UI.NumericBox numericBoxUpdateintervalperpage;
        private Sci.Win.UI.NumericBox numericBoxAllupdateinterval;
        private System.Windows.Forms.Label lbsecond;
        private System.Windows.Forms.Label lbsecond2;
        private Sci.Win.UI.TextBox txtMorningShiftStart;
        private Sci.Win.UI.TextBox txtMorningShiftEnd;
        private Sci.Win.UI.TextBox txtNightShiftStart;
        private Sci.Win.UI.TextBox txtNightShiftEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Sci.Win.UI.Label lbFactory;
        private Sci.Win.UI.Label lbSubProcess;
        private Sci.Win.UI.TextBox txtFactory;
        private Sci.Win.UI.TextBox txtSubProcess;
        private Sci.Win.UI.Label lbWIPRange;
        private Sci.Win.UI.NumericBox numWIPRange;
        private System.Windows.Forms.Label lbday;
    }
}
