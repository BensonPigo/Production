﻿namespace Sci.Production.Shipping
{
    partial class R09
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
            this.labelArrivePortDate = new Sci.Win.UI.Label();
            this.labelDoxRcvdDate = new Sci.Win.UI.Label();
            this.labelAPApvDate = new Sci.Win.UI.Label();
            this.labelShippingMode = new Sci.Win.UI.Label();
            this.labelForwarder = new Sci.Win.UI.Label();
            this.labelReportType = new Sci.Win.UI.Label();
            this.dateArrivePortDate = new Sci.Win.UI.DateRange();
            this.dateDoxRcvdDate = new Sci.Win.UI.DateRange();
            this.dateAPApvDate = new Sci.Win.UI.DateRange();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioListByWKNoByFeeType = new Sci.Win.UI.RadioButton();
            this.radioListbyWKNo = new Sci.Win.UI.RadioButton();
            this.txtshipmode = new Sci.Production.Class.txtshipmode();
            this.txtForwarder = new Sci.Win.UI.TextBox();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(452, 12);
            this.print.TabIndex = 8;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(452, 48);
            this.toexcel.TabIndex = 6;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(452, 84);
            this.close.TabIndex = 7;
            // 
            // labelArrivePortDate
            // 
            this.labelArrivePortDate.Lines = 0;
            this.labelArrivePortDate.Location = new System.Drawing.Point(24, 12);
            this.labelArrivePortDate.Name = "labelArrivePortDate";
            this.labelArrivePortDate.Size = new System.Drawing.Size(101, 23);
            this.labelArrivePortDate.TabIndex = 94;
            this.labelArrivePortDate.Text = "Arrive Port Date";
            // 
            // labelDoxRcvdDate
            // 
            this.labelDoxRcvdDate.Lines = 0;
            this.labelDoxRcvdDate.Location = new System.Drawing.Point(24, 48);
            this.labelDoxRcvdDate.Name = "labelDoxRcvdDate";
            this.labelDoxRcvdDate.Size = new System.Drawing.Size(101, 23);
            this.labelDoxRcvdDate.TabIndex = 95;
            this.labelDoxRcvdDate.Text = "Dox Rcvd Date";
            // 
            // labelAPApvDate
            // 
            this.labelAPApvDate.Lines = 0;
            this.labelAPApvDate.Location = new System.Drawing.Point(24, 84);
            this.labelAPApvDate.Name = "labelAPApvDate";
            this.labelAPApvDate.Size = new System.Drawing.Size(101, 23);
            this.labelAPApvDate.TabIndex = 96;
            this.labelAPApvDate.Text = "AP Apv. Date";
            // 
            // labelShippingMode
            // 
            this.labelShippingMode.Lines = 0;
            this.labelShippingMode.Location = new System.Drawing.Point(24, 120);
            this.labelShippingMode.Name = "labelShippingMode";
            this.labelShippingMode.Size = new System.Drawing.Size(101, 23);
            this.labelShippingMode.TabIndex = 97;
            this.labelShippingMode.Text = "Shipping Mode";
            // 
            // labelForwarder
            // 
            this.labelForwarder.Lines = 0;
            this.labelForwarder.Location = new System.Drawing.Point(24, 157);
            this.labelForwarder.Name = "labelForwarder";
            this.labelForwarder.Size = new System.Drawing.Size(101, 23);
            this.labelForwarder.TabIndex = 98;
            this.labelForwarder.Text = "Forwarder";
            // 
            // labelReportType
            // 
            this.labelReportType.Lines = 0;
            this.labelReportType.Location = new System.Drawing.Point(24, 194);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(101, 23);
            this.labelReportType.TabIndex = 99;
            this.labelReportType.Text = "Report Type";
            // 
            // dateArrivePortDate
            // 
            this.dateArrivePortDate.IsRequired = false;
            this.dateArrivePortDate.Location = new System.Drawing.Point(129, 12);
            this.dateArrivePortDate.Name = "dateArrivePortDate";
            this.dateArrivePortDate.Size = new System.Drawing.Size(280, 23);
            this.dateArrivePortDate.TabIndex = 0;
            // 
            // dateDoxRcvdDate
            // 
            this.dateDoxRcvdDate.IsRequired = false;
            this.dateDoxRcvdDate.Location = new System.Drawing.Point(129, 48);
            this.dateDoxRcvdDate.Name = "dateDoxRcvdDate";
            this.dateDoxRcvdDate.Size = new System.Drawing.Size(280, 23);
            this.dateDoxRcvdDate.TabIndex = 1;
            // 
            // dateAPApvDate
            // 
            this.dateAPApvDate.IsRequired = false;
            this.dateAPApvDate.Location = new System.Drawing.Point(129, 84);
            this.dateAPApvDate.Name = "dateAPApvDate";
            this.dateAPApvDate.Size = new System.Drawing.Size(280, 23);
            this.dateAPApvDate.TabIndex = 2;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioListByWKNoByFeeType);
            this.radioPanel1.Controls.Add(this.radioListbyWKNo);
            this.radioPanel1.Location = new System.Drawing.Point(129, 193);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(200, 59);
            this.radioPanel1.TabIndex = 5;
            // 
            // radioListByWKNoByFeeType
            // 
            this.radioListByWKNoByFeeType.AutoSize = true;
            this.radioListByWKNoByFeeType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioListByWKNoByFeeType.Location = new System.Drawing.Point(4, 31);
            this.radioListByWKNoByFeeType.Name = "radioListByWKNoByFeeType";
            this.radioListByWKNoByFeeType.Size = new System.Drawing.Size(184, 21);
            this.radioListByWKNoByFeeType.TabIndex = 1;
            this.radioListByWKNoByFeeType.TabStop = true;
            this.radioListByWKNoByFeeType.Text = "List by WK# by Fee Type";
            this.radioListByWKNoByFeeType.UseVisualStyleBackColor = true;
            // 
            // radioListbyWKNo
            // 
            this.radioListbyWKNo.AutoSize = true;
            this.radioListbyWKNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioListbyWKNo.Location = new System.Drawing.Point(3, 3);
            this.radioListbyWKNo.Name = "radioListbyWKNo";
            this.radioListbyWKNo.Size = new System.Drawing.Size(101, 21);
            this.radioListbyWKNo.TabIndex = 0;
            this.radioListbyWKNo.TabStop = true;
            this.radioListbyWKNo.Text = "List by WK#";
            this.radioListbyWKNo.UseVisualStyleBackColor = true;
            // 
            // txtshipmode
            // 
            this.txtshipmode.BackColor = System.Drawing.Color.White;
            this.txtshipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshipmode.FormattingEnabled = true;
            this.txtshipmode.IsSupportUnselect = true;
            this.txtshipmode.Location = new System.Drawing.Point(129, 120);
            this.txtshipmode.Name = "txtshipmode";
            this.txtshipmode.Size = new System.Drawing.Size(80, 24);
            this.txtshipmode.TabIndex = 3;
            this.txtshipmode.UseFunction = null;
            // 
            // txtForwarder
            // 
            this.txtForwarder.BackColor = System.Drawing.Color.White;
            this.txtForwarder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtForwarder.Location = new System.Drawing.Point(129, 157);
            this.txtForwarder.Name = "txtForwarder";
            this.txtForwarder.Size = new System.Drawing.Size(61, 23);
            this.txtForwarder.TabIndex = 4;
            this.txtForwarder.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.textBox1_PopUp);
            this.txtForwarder.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(192, 158);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(100, 23);
            this.displayBox1.TabIndex = 108;
            // 
            // R09
            // 
            this.ClientSize = new System.Drawing.Size(544, 284);
            this.Controls.Add(this.displayBox1);
            this.Controls.Add(this.txtForwarder);
            this.Controls.Add(this.txtshipmode);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.dateAPApvDate);
            this.Controls.Add(this.dateDoxRcvdDate);
            this.Controls.Add(this.dateArrivePortDate);
            this.Controls.Add(this.labelReportType);
            this.Controls.Add(this.labelForwarder);
            this.Controls.Add(this.labelShippingMode);
            this.Controls.Add(this.labelAPApvDate);
            this.Controls.Add(this.labelDoxRcvdDate);
            this.Controls.Add(this.labelArrivePortDate);
            this.IsSupportToPrint = false;
            this.Name = "R09";
            this.Text = "R09. Share Expense Report - Import";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelArrivePortDate, 0);
            this.Controls.SetChildIndex(this.labelDoxRcvdDate, 0);
            this.Controls.SetChildIndex(this.labelAPApvDate, 0);
            this.Controls.SetChildIndex(this.labelShippingMode, 0);
            this.Controls.SetChildIndex(this.labelForwarder, 0);
            this.Controls.SetChildIndex(this.labelReportType, 0);
            this.Controls.SetChildIndex(this.dateArrivePortDate, 0);
            this.Controls.SetChildIndex(this.dateDoxRcvdDate, 0);
            this.Controls.SetChildIndex(this.dateAPApvDate, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.txtshipmode, 0);
            this.Controls.SetChildIndex(this.txtForwarder, 0);
            this.Controls.SetChildIndex(this.displayBox1, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelArrivePortDate;
        private Win.UI.Label labelDoxRcvdDate;
        private Win.UI.Label labelAPApvDate;
        private Win.UI.Label labelShippingMode;
        private Win.UI.Label labelForwarder;
        private Win.UI.Label labelReportType;
        private Win.UI.DateRange dateArrivePortDate;
        private Win.UI.DateRange dateDoxRcvdDate;
        private Win.UI.DateRange dateAPApvDate;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioListByWKNoByFeeType;
        private Win.UI.RadioButton radioListbyWKNo;
        private Class.txtshipmode txtshipmode;
        private Win.UI.TextBox txtForwarder;
        private Win.UI.DisplayBox displayBox1;
    }
}
