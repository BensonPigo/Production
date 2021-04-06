namespace Sci.Production.PPIC
{
    partial class R01
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
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelSewingLine = new Sci.Win.UI.Label();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.txtSewingLineStart = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.txtSewingLineEnd = new Sci.Win.UI.TextBox();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.checkForPrintOut = new Sci.Win.UI.CheckBox();
            this.labSummeryBy = new Sci.Win.UI.Label();
            this.comboSummaryBy = new Sci.Win.UI.ComboBox();
            this.dateSewingDate = new Sci.Win.UI.DateRange();
            this.labSewingDate = new Sci.Win.UI.Label();
            this.chkGanttChart = new Sci.Win.UI.CheckBox();
            this.btnLastDownloadAPSDate = new Sci.Win.UI.Button();
            this.comboSubProcess = new Sci.Win.UI.ComboBox();
            this.labelSubProcess = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(379, 28);
            this.print.Size = new System.Drawing.Size(115, 30);
            this.print.TabIndex = 9;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(379, 64);
            this.toexcel.Size = new System.Drawing.Size(115, 30);
            this.toexcel.TabIndex = 10;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(379, 100);
            this.close.Size = new System.Drawing.Size(115, 30);
            this.close.TabIndex = 11;
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(248, 37);
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 12);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(97, 23);
            this.labelM.TabIndex = 94;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 48);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(97, 23);
            this.labelFactory.TabIndex = 95;
            this.labelFactory.Text = "Factory";
            // 
            // labelSewingLine
            // 
            this.labelSewingLine.Location = new System.Drawing.Point(13, 84);
            this.labelSewingLine.Name = "labelSewingLine";
            this.labelSewingLine.Size = new System.Drawing.Size(97, 23);
            this.labelSewingLine.TabIndex = 96;
            this.labelSewingLine.Text = "Sewing Line";
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(13, 155);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(97, 23);
            this.labelBuyerDelivery.TabIndex = 99;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(13, 191);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(97, 23);
            this.labelSCIDelivery.TabIndex = 100;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 227);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(97, 23);
            this.labelBrand.TabIndex = 101;
            this.labelBrand.Text = "Brand";
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(114, 12);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(72, 24);
            this.comboM.TabIndex = 0;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(114, 48);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(72, 24);
            this.comboFactory.TabIndex = 1;
            this.comboFactory.SelectedIndexChanged += new System.EventHandler(this.ComboFactory_SelectedIndexChanged);
            // 
            // txtSewingLineStart
            // 
            this.txtSewingLineStart.BackColor = System.Drawing.Color.White;
            this.txtSewingLineStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSewingLineStart.Location = new System.Drawing.Point(114, 84);
            this.txtSewingLineStart.Name = "txtSewingLineStart";
            this.txtSewingLineStart.Size = new System.Drawing.Size(41, 23);
            this.txtSewingLineStart.TabIndex = 2;
            this.txtSewingLineStart.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSewingLineStart_PopUp);
            this.txtSewingLineStart.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSewingLineStart_Validating);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(156, 84);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 23);
            this.label9.TabIndex = 105;
            this.label9.Text = "～";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtSewingLineEnd
            // 
            this.txtSewingLineEnd.BackColor = System.Drawing.Color.White;
            this.txtSewingLineEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSewingLineEnd.Location = new System.Drawing.Point(177, 84);
            this.txtSewingLineEnd.Name = "txtSewingLineEnd";
            this.txtSewingLineEnd.Size = new System.Drawing.Size(41, 23);
            this.txtSewingLineEnd.TabIndex = 3;
            this.txtSewingLineEnd.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSewingLineEnd_PopUp);
            this.txtSewingLineEnd.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSewingLineEnd_Validating);
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(106, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(128, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(106, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(114, 155);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(234, 23);
            this.dateBuyerDelivery.TabIndex = 5;
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(106, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(128, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(106, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(114, 191);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(234, 23);
            this.dateSCIDelivery.TabIndex = 6;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(114, 227);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(85, 23);
            this.txtbrand.TabIndex = 7;
            // 
            // checkForPrintOut
            // 
            this.checkForPrintOut.AutoSize = true;
            this.checkForPrintOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkForPrintOut.Location = new System.Drawing.Point(240, 227);
            this.checkForPrintOut.Name = "checkForPrintOut";
            this.checkForPrintOut.Size = new System.Drawing.Size(108, 21);
            this.checkForPrintOut.TabIndex = 8;
            this.checkForPrintOut.Text = "For Print Out";
            this.checkForPrintOut.UseVisualStyleBackColor = true;
            // 
            // labSummeryBy
            // 
            this.labSummeryBy.Location = new System.Drawing.Point(13, 262);
            this.labSummeryBy.Name = "labSummeryBy";
            this.labSummeryBy.Size = new System.Drawing.Size(97, 23);
            this.labSummeryBy.TabIndex = 108;
            this.labSummeryBy.Text = "Summary By";
            // 
            // comboSummaryBy
            // 
            this.comboSummaryBy.BackColor = System.Drawing.Color.White;
            this.comboSummaryBy.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboSummaryBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSummaryBy.FormattingEnabled = true;
            this.comboSummaryBy.IsSupportUnselect = true;
            this.comboSummaryBy.Location = new System.Drawing.Point(114, 262);
            this.comboSummaryBy.Name = "comboSummaryBy";
            this.comboSummaryBy.OldText = "";
            this.comboSummaryBy.Size = new System.Drawing.Size(200, 24);
            this.comboSummaryBy.TabIndex = 9;
            this.comboSummaryBy.SelectedValueChanged += new System.EventHandler(this.ComboSummaryBy_SelectedValueChanged);
            // 
            // dateSewingDate
            // 
            // 
            // 
            // 
            this.dateSewingDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSewingDate.DateBox1.Name = "";
            this.dateSewingDate.DateBox1.Size = new System.Drawing.Size(106, 23);
            this.dateSewingDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSewingDate.DateBox2.Location = new System.Drawing.Point(128, 0);
            this.dateSewingDate.DateBox2.Name = "";
            this.dateSewingDate.DateBox2.Size = new System.Drawing.Size(106, 23);
            this.dateSewingDate.DateBox2.TabIndex = 1;
            this.dateSewingDate.IsRequired = false;
            this.dateSewingDate.Location = new System.Drawing.Point(114, 120);
            this.dateSewingDate.Name = "dateSewingDate";
            this.dateSewingDate.Size = new System.Drawing.Size(234, 23);
            this.dateSewingDate.TabIndex = 4;
            // 
            // labSewingDate
            // 
            this.labSewingDate.Location = new System.Drawing.Point(13, 120);
            this.labSewingDate.Name = "labSewingDate";
            this.labSewingDate.Size = new System.Drawing.Size(97, 23);
            this.labSewingDate.TabIndex = 552;
            this.labSewingDate.Text = "Sewing Date";
            // 
            // chkGanttChart
            // 
            this.chkGanttChart.AutoSize = true;
            this.chkGanttChart.Checked = true;
            this.chkGanttChart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGanttChart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.chkGanttChart.Location = new System.Drawing.Point(332, 264);
            this.chkGanttChart.Name = "chkGanttChart";
            this.chkGanttChart.ReadOnly = true;
            this.chkGanttChart.Size = new System.Drawing.Size(149, 21);
            this.chkGanttChart.TabIndex = 553;
            this.chkGanttChart.Text = "Include Gantt Chart";
            this.chkGanttChart.UseVisualStyleBackColor = true;
            // 
            // btnLastDownloadAPSDate
            // 
            this.btnLastDownloadAPSDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLastDownloadAPSDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnLastDownloadAPSDate.Location = new System.Drawing.Point(379, 136);
            this.btnLastDownloadAPSDate.Name = "btnLastDownloadAPSDate";
            this.btnLastDownloadAPSDate.Size = new System.Drawing.Size(115, 66);
            this.btnLastDownloadAPSDate.TabIndex = 554;
            this.btnLastDownloadAPSDate.Text = "Last APS Download Time";
            this.btnLastDownloadAPSDate.UseVisualStyleBackColor = true;
            this.btnLastDownloadAPSDate.Click += new System.EventHandler(this.BtnLastDownloadAPSDate_Click);
            // 
            // comboSubProcess
            // 
            this.comboSubProcess.BackColor = System.Drawing.Color.White;
            this.comboSubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSubProcess.FormattingEnabled = true;
            this.comboSubProcess.IsSupportUnselect = true;
            this.comboSubProcess.Location = new System.Drawing.Point(114, 292);
            this.comboSubProcess.Name = "comboSubProcess";
            this.comboSubProcess.OldText = "";
            this.comboSubProcess.Size = new System.Drawing.Size(165, 24);
            this.comboSubProcess.TabIndex = 557;
            // 
            // labelSubProcess
            // 
            this.labelSubProcess.Location = new System.Drawing.Point(13, 293);
            this.labelSubProcess.Name = "labelSubProcess";
            this.labelSubProcess.Size = new System.Drawing.Size(97, 23);
            this.labelSubProcess.TabIndex = 558;
            this.labelSubProcess.Text = "Sub Process";
            // 
            // R01
            // 
            this.ClientSize = new System.Drawing.Size(506, 345);
            this.Controls.Add(this.comboSubProcess);
            this.Controls.Add(this.labelSubProcess);
            this.Controls.Add(this.btnLastDownloadAPSDate);
            this.Controls.Add(this.chkGanttChart);
            this.Controls.Add(this.dateSewingDate);
            this.Controls.Add(this.labSewingDate);
            this.Controls.Add(this.comboSummaryBy);
            this.Controls.Add(this.labSummeryBy);
            this.Controls.Add(this.checkForPrintOut);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.txtSewingLineEnd);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtSewingLineStart);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelSCIDelivery);
            this.Controls.Add(this.labelBuyerDelivery);
            this.Controls.Add(this.labelSewingLine);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.DefaultControl = "comboM";
            this.DefaultControlForEdit = "comboM";
            this.IsSupportToPrint = false;
            this.Name = "R01";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R01. Sewing Line Schedule Report";
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelSewingLine, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.txtSewingLineStart, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.txtSewingLineEnd, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.checkForPrintOut, 0);
            this.Controls.SetChildIndex(this.labSummeryBy, 0);
            this.Controls.SetChildIndex(this.comboSummaryBy, 0);
            this.Controls.SetChildIndex(this.labSewingDate, 0);
            this.Controls.SetChildIndex(this.dateSewingDate, 0);
            this.Controls.SetChildIndex(this.chkGanttChart, 0);
            this.Controls.SetChildIndex(this.btnLastDownloadAPSDate, 0);
            this.Controls.SetChildIndex(this.labelSubProcess, 0);
            this.Controls.SetChildIndex(this.comboSubProcess, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelSewingLine;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label labelBrand;
        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.TextBox txtSewingLineStart;
        private Win.UI.Label label9;
        private Win.UI.TextBox txtSewingLineEnd;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.DateRange dateSCIDelivery;
        private Class.Txtbrand txtbrand;
        private Win.UI.CheckBox checkForPrintOut;
        private Win.UI.Label labSummeryBy;
        private Win.UI.ComboBox comboSummaryBy;
        private Win.UI.DateRange dateSewingDate;
        private Win.UI.Label labSewingDate;
        private Win.UI.CheckBox chkGanttChart;
        private Win.UI.Button btnLastDownloadAPSDate;
        private Win.UI.ComboBox comboSubProcess;
        private Win.UI.Label labelSubProcess;
    }
}
