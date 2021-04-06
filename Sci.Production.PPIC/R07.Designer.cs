namespace Sci.Production.PPIC
{
    partial class R07
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
            this.labelYear = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.numericUpDownYear = new Sci.Win.UI.NumericUpDown();
            this.labelMonth = new Sci.Win.UI.Label();
            this.numericUpDownMonth = new Sci.Win.UI.NumericUpDown();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.comboSubProcess = new Sci.Win.UI.ComboBox();
            this.labelSubProcess = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMonth)).BeginInit();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(453, 12);
            this.print.TabIndex = 4;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(453, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(453, 84);
            this.close.TabIndex = 6;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(407, 120);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(433, 122);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(433, 120);
            // 
            // labelYear
            // 
            this.labelYear.Location = new System.Drawing.Point(14, 12);
            this.labelYear.Name = "labelYear";
            this.labelYear.Size = new System.Drawing.Size(52, 23);
            this.labelYear.TabIndex = 94;
            this.labelYear.Text = "Year";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 48);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(52, 23);
            this.labelM.TabIndex = 95;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 84);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(52, 23);
            this.labelFactory.TabIndex = 96;
            this.labelFactory.Text = "Factory";
            // 
            // numericUpDownYear
            // 
            this.numericUpDownYear.BackColor = System.Drawing.Color.White;
            this.numericUpDownYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericUpDownYear.Location = new System.Drawing.Point(69, 12);
            this.numericUpDownYear.Maximum = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.numericUpDownYear.Minimum = new decimal(new int[] {
            2010,
            0,
            0,
            0});
            this.numericUpDownYear.Name = "numericUpDownYear";
            this.numericUpDownYear.Size = new System.Drawing.Size(70, 23);
            this.numericUpDownYear.TabIndex = 0;
            this.numericUpDownYear.Value = new decimal(new int[] {
            2010,
            0,
            0,
            0});
            // 
            // labelMonth
            // 
            this.labelMonth.Location = new System.Drawing.Point(183, 12);
            this.labelMonth.Name = "labelMonth";
            this.labelMonth.Size = new System.Drawing.Size(46, 23);
            this.labelMonth.TabIndex = 98;
            this.labelMonth.Text = "Month";
            // 
            // numericUpDownMonth
            // 
            this.numericUpDownMonth.BackColor = System.Drawing.Color.White;
            this.numericUpDownMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericUpDownMonth.Location = new System.Drawing.Point(233, 12);
            this.numericUpDownMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numericUpDownMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMonth.Name = "numericUpDownMonth";
            this.numericUpDownMonth.Size = new System.Drawing.Size(47, 23);
            this.numericUpDownMonth.TabIndex = 1;
            this.numericUpDownMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(69, 48);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(70, 24);
            this.comboM.TabIndex = 2;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(69, 84);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(70, 24);
            this.comboFactory.TabIndex = 3;
            // 
            // comboSubProcess
            // 
            this.comboSubProcess.BackColor = System.Drawing.Color.White;
            this.comboSubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSubProcess.FormattingEnabled = true;
            this.comboSubProcess.IsSupportUnselect = true;
            this.comboSubProcess.Location = new System.Drawing.Point(106, 117);
            this.comboSubProcess.Name = "comboSubProcess";
            this.comboSubProcess.OldText = "";
            this.comboSubProcess.Size = new System.Drawing.Size(165, 24);
            this.comboSubProcess.TabIndex = 559;
            // 
            // labelSubProcess
            // 
            this.labelSubProcess.Location = new System.Drawing.Point(13, 117);
            this.labelSubProcess.Name = "labelSubProcess";
            this.labelSubProcess.Size = new System.Drawing.Size(90, 23);
            this.labelSubProcess.TabIndex = 560;
            this.labelSubProcess.Text = "Sub Process";
            // 
            // R07
            // 
            this.ClientSize = new System.Drawing.Size(545, 190);
            this.Controls.Add(this.comboSubProcess);
            this.Controls.Add(this.labelSubProcess);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.numericUpDownMonth);
            this.Controls.Add(this.labelMonth);
            this.Controls.Add(this.numericUpDownYear);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelYear);
            this.DefaultControl = "numericUpDownYear";
            this.DefaultControlForEdit = "numericUpDownYear";
            this.IsSupportToPrint = false;
            this.Name = "R07";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R07. Sewing Schedule Gantt Chart";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelYear, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.numericUpDownYear, 0);
            this.Controls.SetChildIndex(this.labelMonth, 0);
            this.Controls.SetChildIndex(this.numericUpDownMonth, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.labelSubProcess, 0);
            this.Controls.SetChildIndex(this.comboSubProcess, 0);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMonth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelYear;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.NumericUpDown numericUpDownYear;
        private Win.UI.Label labelMonth;
        private Win.UI.NumericUpDown numericUpDownMonth;
        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.ComboBox comboSubProcess;
        private Win.UI.Label labelSubProcess;
    }
}
