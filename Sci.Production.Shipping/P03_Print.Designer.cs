﻿namespace Sci.Production.Shipping
{
    partial class P03_Print
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
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.dateETA = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelETA = new Sci.Win.UI.Label();
            this.radioListReport = new Sci.Win.UI.RadioButton();
            this.radioDetailReport = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(409, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(409, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(409, 84);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.txtfactory);
            this.radioPanel1.Controls.Add(this.dateETA);
            this.radioPanel1.Controls.Add(this.labelFactory);
            this.radioPanel1.Controls.Add(this.labelETA);
            this.radioPanel1.Controls.Add(this.radioListReport);
            this.radioPanel1.Controls.Add(this.radioDetailReport);
            this.radioPanel1.Location = new System.Drawing.Point(13, 8);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(373, 121);
            this.radioPanel1.TabIndex = 94;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.Location = new System.Drawing.Point(85, 89);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 5;
            // 
            // dateETA
            // 
            this.dateETA.IsRequired = false;
            this.dateETA.Location = new System.Drawing.Point(84, 59);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(280, 23);
            this.dateETA.TabIndex = 4;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(27, 89);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(54, 23);
            this.labelFactory.TabIndex = 3;
            this.labelFactory.Text = "Factory";
            // 
            // labelETA
            // 
            this.labelETA.Lines = 0;
            this.labelETA.Location = new System.Drawing.Point(27, 60);
            this.labelETA.Name = "labelETA";
            this.labelETA.Size = new System.Drawing.Size(54, 23);
            this.labelETA.TabIndex = 2;
            this.labelETA.Text = "ETA";
            // 
            // radioListReport
            // 
            this.radioListReport.AutoSize = true;
            this.radioListReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioListReport.Location = new System.Drawing.Point(6, 32);
            this.radioListReport.Name = "radioListReport";
            this.radioListReport.Size = new System.Drawing.Size(95, 21);
            this.radioListReport.TabIndex = 1;
            this.radioListReport.TabStop = true;
            this.radioListReport.Text = "List Report";
            this.radioListReport.UseVisualStyleBackColor = true;
            this.radioListReport.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioDetailReport
            // 
            this.radioDetailReport.AutoSize = true;
            this.radioDetailReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetailReport.Location = new System.Drawing.Point(6, 5);
            this.radioDetailReport.Name = "radioDetailReport";
            this.radioDetailReport.Size = new System.Drawing.Size(109, 21);
            this.radioDetailReport.TabIndex = 0;
            this.radioDetailReport.TabStop = true;
            this.radioDetailReport.Text = "Detail Report";
            this.radioDetailReport.UseVisualStyleBackColor = true;
            // 
            // P03_Print
            // 
            this.ClientSize = new System.Drawing.Size(501, 163);
            this.Controls.Add(this.radioPanel1);
            this.IsSupportToPrint = false;
            this.Name = "P03_Print";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.DateRange dateETA;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelETA;
        private Win.UI.RadioButton radioListReport;
        private Win.UI.RadioButton radioDetailReport;
        private Class.txtfactory txtfactory;
    }
}
