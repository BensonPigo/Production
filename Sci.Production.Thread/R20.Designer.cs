﻿namespace Sci.Production.Thread
{
    partial class R20
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.comboMDivision = new Sci.Production.Class.comboMDivision(this.components);
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.dateEstArrived = new Sci.Win.UI.DateRange();
            this.dateEstBooking = new Sci.Win.UI.DateRange();
            this.label6 = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelEstArrived = new Sci.Win.UI.Label();
            this.labelEstBooking = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.TabIndex = 1;
            // 
            // toexcel
            // 
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboMDivision);
            this.panel1.Controls.Add(this.comboFactory);
            this.panel1.Controls.Add(this.dateEstArrived);
            this.panel1.Controls.Add(this.dateEstBooking);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtSPNoEnd);
            this.panel1.Controls.Add(this.txtSPNoStart);
            this.panel1.Controls.Add(this.labelM);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelEstArrived);
            this.panel1.Controls.Add(this.labelEstBooking);
            this.panel1.Controls.Add(this.labelSPNo);
            this.panel1.Location = new System.Drawing.Point(12, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(497, 233);
            this.panel1.TabIndex = 0;
            // 
            // comboMDivision
            // 
            this.comboMDivision.BackColor = System.Drawing.Color.White;
            this.comboMDivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMDivision.FormattingEnabled = true;
            this.comboMDivision.IsSupportUnselect = true;
            this.comboMDivision.Location = new System.Drawing.Point(100, 189);
            this.comboMDivision.Name = "comboMDivision";
            this.comboMDivision.Size = new System.Drawing.Size(80, 24);
            this.comboMDivision.TabIndex = 5;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(100, 147);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 4;
            // 
            // dateEstArrived
            // 
            this.dateEstArrived.IsRequired = false;
            this.dateEstArrived.Location = new System.Drawing.Point(100, 104);
            this.dateEstArrived.Name = "dateEstArrived";
            this.dateEstArrived.Size = new System.Drawing.Size(336, 23);
            this.dateEstArrived.TabIndex = 3;
            // 
            // dateEstBooking
            // 
            this.dateEstBooking.IsRequired = false;
            this.dateEstBooking.Location = new System.Drawing.Point(100, 62);
            this.dateEstBooking.Name = "dateEstBooking";
            this.dateEstBooking.Size = new System.Drawing.Size(336, 23);
            this.dateEstBooking.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(260, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 16);
            this.label6.TabIndex = 7;
            this.label6.Text = "~";
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(287, 23);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(149, 23);
            this.txtSPNoEnd.TabIndex = 1;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(100, 23);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(149, 23);
            this.txtSPNoStart.TabIndex = 0;
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(11, 189);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(86, 23);
            this.labelM.TabIndex = 4;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(11, 147);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(86, 23);
            this.labelFactory.TabIndex = 3;
            this.labelFactory.Text = "Factory";
            // 
            // labelEstArrived
            // 
            this.labelEstArrived.Lines = 0;
            this.labelEstArrived.Location = new System.Drawing.Point(11, 104);
            this.labelEstArrived.Name = "labelEstArrived";
            this.labelEstArrived.Size = new System.Drawing.Size(86, 23);
            this.labelEstArrived.TabIndex = 2;
            this.labelEstArrived.Text = "Est. Arrived";
            // 
            // labelEstBooking
            // 
            this.labelEstBooking.Lines = 0;
            this.labelEstBooking.Location = new System.Drawing.Point(11, 64);
            this.labelEstBooking.Name = "labelEstBooking";
            this.labelEstBooking.Size = new System.Drawing.Size(86, 23);
            this.labelEstBooking.TabIndex = 1;
            this.labelEstBooking.Text = "Est. Booking";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(11, 23);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(86, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP No";
            // 
            // R20
            // 
            this.ClientSize = new System.Drawing.Size(627, 314);
            this.Controls.Add(this.panel1);
            this.Name = "R20";
            this.Text = "R20.Thread request List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.DateRange dateEstArrived;
        private Win.UI.DateRange dateEstBooking;
        private Win.UI.Label label6;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelEstArrived;
        private Win.UI.Label labelEstBooking;
        private Win.UI.Label labelSPNo;
        private Class.comboMDivision comboMDivision;
    }
}
